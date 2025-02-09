// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.TradeReportGenerator
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils.Query;
using EllieMae.EMLite.Server.Query;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public class TradeReportGenerator
  {
    private const string className = "TradeReportGenerator�";
    private UserInfo currentUser;
    private TradeReportParameters parameters;
    private IServerProgressFeedback feedback;
    private ManualResetEvent stopEvent;
    private ReportResults reportResults;
    private Exception reportError;
    private QueryEngine queryEngine;

    public TradeReportGenerator(
      UserInfo currentUser,
      TradeReportParameters parameters,
      IServerProgressFeedback feedback)
    {
      this.currentUser = currentUser;
      this.parameters = parameters;
      this.feedback = feedback;
    }

    public ReportResults Generate()
    {
      this.stopEvent = new ManualResetEvent(false);
      new Thread(new ParameterizedThreadStart(this.generateAsync))
      {
        Priority = ThreadPriority.BelowNormal,
        IsBackground = true
      }.Start((object) ClientContext.GetCurrent());
      this.stopEvent.WaitOne();
      if (this.reportError != null)
        Err.Raise(nameof (TradeReportGenerator), new ServerException("Report failed due to an error: " + this.reportError.Message, this.reportError));
      return this.reportResults;
    }

    private void generateAsync(object contextObj)
    {
      try
      {
        using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("Contact Report Generation", 72, nameof (generateAsync), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\TradeReportGenerator.cs"))
        {
          ClientContext clientContext = (ClientContext) contextObj;
          this.reportResults = new ReportResults();
          performanceMeter.AddNote("Report includes " + (object) this.parameters.Fields.Count + " fields: " + string.Join(", ", this.parameters.GetSelectionFieldIDs()));
          using (clientContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
            this.generateReport(false);
        }
      }
      catch (Exception ex)
      {
        this.reportError = ex;
        TraceLog.WriteException(nameof (TradeReportGenerator), ex);
        this.reportResults = (ReportResults) null;
      }
      finally
      {
        this.stopEvent.Set();
      }
    }

    private void generateReport(bool isExternalOrganization)
    {
      if (this.feedback != null)
        this.feedback.Status = "Preparing to retrieve report data...";
      switch (this.parameters.TradeType)
      {
        case TradeType.SecurityTrade:
          this.queryEngine = (QueryEngine) new SecurityTradeQuery(this.currentUser);
          break;
        case TradeType.LoanTrade:
          this.queryEngine = (QueryEngine) new TradeQuery(this.currentUser);
          break;
        case TradeType.MbsPool:
          this.queryEngine = (QueryEngine) new MbsPoolQuery(this.currentUser);
          break;
        case TradeType.CorrespondentTrade:
          this.queryEngine = (QueryEngine) new CorrespondentTradeQuery(this.currentUser);
          break;
        case TradeType.CorrespondentMaster:
          this.queryEngine = (QueryEngine) new CorrespondentMasterQuery(this.currentUser);
          break;
        case TradeType.MasterContract:
          this.queryEngine = (QueryEngine) new MasterContractQuery(this.currentUser);
          break;
      }
      this.queryEngine.SplitFiltersByReportsFor(this.parameters.FieldFilters);
      this.queryEngine.GetCategories(this.parameters.Fields);
      PerformanceMeter.Current.AddCheckpoint("Prepared combined filter for report", 132, nameof (generateReport), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\TradeReportGenerator.cs");
      if (this.feedback != null)
        this.feedback.ResetCounter(this.parameters.Fields.Count + 4);
      if (this.feedback != null && !this.feedback.SetFeedback("Retrieving data...", (string) null, 1))
        return;
      Dictionary<int, string[]> tradeData = new Dictionary<int, string[]>();
      bool[] flagArray = (bool[]) null;
      using (EllieMae.EMLite.Server.DbAccessManager db = new EllieMae.EMLite.Server.DbAccessManager())
      {
        string identitySelectionQuery = this.queryEngine.CreateIdentitySelectionQuery(this.parameters.CreateCombinedFilter(this.queryEngine.GetParentFilters()), isExternalOrganization);
        string str = "create table #trade_ids ( TradeID int PRIMARY KEY )" + Environment.NewLine;
        string sql = (!((identitySelectionQuery ?? "") == "") ? str + "insert into #trade_ids " + identitySelectionQuery + Environment.NewLine : str + "insert into #trade_ids select TradeID from " + this.queryEngine.PrimaryKeyTableIdentifier + Environment.NewLine) + this.queryEngine.GetChildrenFilterSql(this.parameters, isExternalOrganization) + "select count(*) from #trade_ids" + Environment.NewLine;
        DataSet dataSet = db.ExecuteSetQuery(sql, EllieMae.EMLite.DataAccess.ServerGlobals.ReportSQLTimeout, DbTransactionType.Snapshot);
        int num1 = 0;
        if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
          num1 = (int) dataSet.Tables[0].Rows[0][0];
        PerformanceMeter.Current.AddCheckpoint("Built temp table with filtered contact IDs", 169, nameof (generateReport), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\TradeReportGenerator.cs");
        if (num1 == 0 || this.feedback != null && !this.feedback.SetFeedback("Aggregating field data...", (string) null, 2))
          return;
        List<EllieMae.EMLite.ClientServer.Reporting.ColumnInfo> fields = new List<EllieMae.EMLite.ClientServer.Reporting.ColumnInfo>();
        for (int index = 0; index < this.parameters.Fields.Count; ++index)
        {
          EllieMae.EMLite.ClientServer.Reporting.ColumnInfo field = this.parameters.Fields[index];
          if (!field.IsExcelField)
            fields.Add(field);
        }
        DataTable dataTable = (DataTable) null;
        using (PerformanceMeter.Current.BeginOperation("Retrieve report field data"))
          dataTable = this.queryForFieldValues(fields, db, "#trade_ids");
        flagArray = new bool[fields.Count];
        for (int index1 = 0; index1 < dataTable.Rows.Count; ++index1)
        {
          tradeData[index1] = new string[this.parameters.Fields.Count];
          int num2 = 1;
          for (int index2 = 0; index2 < this.parameters.Fields.Count; ++index2)
          {
            EllieMae.EMLite.ClientServer.Reporting.ColumnInfo field = this.parameters.Fields[index2];
            if (!field.IsExcelField && num2 < fields.Count + 1)
            {
              string columnName = dataTable.Columns[num2].ColumnName.Replace("__", ".");
              if (index1 == 0)
                flagArray[num2 - 1] = field.IsParent;
              if (!this.formatEnumFieldValue(ref tradeData[index1][index2], dataTable.Rows[index1][num2], columnName))
                tradeData[index1][index2] = this.formatFieldValue(dataTable.Rows[index1][num2], field.Format);
              ++num2;
            }
          }
        }
        if (this.feedback != null)
        {
          if (!this.feedback.Increment(1))
            return;
        }
      }
      if (this.feedback != null && !this.feedback.SetFeedback("Compiling results...", (string) null, this.parameters.Fields.Count + 3))
        return;
      if (this.queryEngine.Categories != null && this.queryEngine.Categories.Count<ReportsFor>((System.Func<ReportsFor, bool>) (c => !this.queryEngine.IsParentReportFor(c))) > 1)
      {
        List<string[]> strArrayList = new List<string[]>();
        foreach (int key1 in tradeData.Keys)
        {
          int key = key1;
          bool flag = true;
          if (tradeData.Count<KeyValuePair<int, string[]>>((System.Func<KeyValuePair<int, string[]>, bool>) (r => r.Value[r.Value.Length - 1] == tradeData[key][tradeData[key].Length - 1])) > 1)
          {
            for (int index = 0; index < flagArray.Length; ++index)
            {
              if (!flagArray[index] && !string.IsNullOrEmpty(tradeData[key][index]))
              {
                flag = false;
                break;
              }
            }
          }
          else
            flag = false;
          if (!flag)
            this.reportResults.Add(tradeData[key]);
          else
            strArrayList.Add(tradeData[key]);
        }
        foreach (string[] strArray in strArrayList)
        {
          string[] removedrow = strArray;
          if (!this.reportResults.GetAllResults().Any<string[]>((System.Func<string[], bool>) (r => r[r.Length - 1] == removedrow[removedrow.Length - 1])))
            this.reportResults.Add(removedrow);
        }
      }
      else
      {
        foreach (int key in tradeData.Keys)
          this.reportResults.Add(tradeData[key]);
      }
      if (this.feedback != null)
        this.feedback.SetFeedback("Report Completed.", (string) null, this.parameters.Fields.Count + 4);
      PerformanceMeter.Current.AddCheckpoint("Retrieved report field data from database", 294, nameof (generateReport), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\TradeReportGenerator.cs");
    }

    private DataTable queryForFieldValues(
      List<EllieMae.EMLite.ClientServer.Reporting.ColumnInfo> fields,
      EllieMae.EMLite.Server.DbAccessManager db,
      string guidTable)
    {
      string fieldJoinClause1 = this.queryEngine.GetFieldJoinClause((IQueryTerm[]) DataField.CreateFields(fields.Select<EllieMae.EMLite.ClientServer.Reporting.ColumnInfo, string>((System.Func<EllieMae.EMLite.ClientServer.Reporting.ColumnInfo, string>) (c => c.CriterionName)).ToArray<string>()), (QueryCriterion) null, (SortField[]) null);
      List<string> source = new List<string>();
      foreach (ReportsFor category in this.queryEngine.Categories)
      {
        if (!this.queryEngine.IsParentReportFor(category))
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append("select " + this.queryEngine.PrimaryKeyIdentifier + ", ");
          stringBuilder.Append(this.queryEngine.GetFieldSelectionList(this.queryEngine.UseNullByReportsFor(category, fields)));
          stringBuilder.AppendLine();
          stringBuilder.AppendLine("from " + this.queryEngine.PrimaryKeyTableIdentifier);
          stringBuilder.AppendLine("inner join " + guidTable + " AccessibleTrades on " + this.queryEngine.PrimaryKeyIdentifier + " = AccessibleTrades.TradeID ");
          stringBuilder.AppendLine(this.queryEngine.GetChildrenTableJoins(category, fieldJoinClause1));
          source.Add(stringBuilder.ToString());
        }
      }
      string empty = string.Empty;
      string cmdText;
      if (source.Count == 0)
      {
        IQueryTerm[] fields1 = (IQueryTerm[]) DataField.CreateFields(fields.Select<EllieMae.EMLite.ClientServer.Reporting.ColumnInfo, string>((System.Func<EllieMae.EMLite.ClientServer.Reporting.ColumnInfo, string>) (f => f.CriterionName)).ToArray<string>());
        string fieldSelectionList = this.queryEngine.GetFieldSelectionList(fields1);
        string fieldJoinClause2 = this.queryEngine.GetFieldJoinClause(fields1, (QueryCriterion) null, (SortField[]) null);
        cmdText = "select " + this.queryEngine.PrimaryKeyIdentifier + ", " + fieldSelectionList + " from " + this.queryEngine.PrimaryKeyTableIdentifier + " inner join " + guidTable + " AccessibleTrades on " + this.queryEngine.PrimaryKeyIdentifier + " = AccessibleTrades.TradeID " + fieldJoinClause2;
        if (this.queryEngine.Categories.Count == 1 && this.queryEngine.Categories[0].ToString() == "CorrespondentMasters" && this.queryEngine.GetParentFilters() != null && this.queryEngine.GetParentFilters().Count != 0 && this.queryEngine.GetParentFilters().CreateEvaluator().ToQueryCriterion().UsesTable("CorrespondentMasterDeliveryMethod"))
          cmdText += " and CorrespondentMasterDeliveryMethod.CorrespondentMasterDeliveryMethodID in (select CorrespondentMasterDeliveryMethodID from #corrDeliveryType_methodids)";
      }
      else if (source.Count == 1)
      {
        cmdText = source.First<string>();
      }
      else
      {
        cmdText = string.Empty;
        foreach (string str in source)
        {
          if (str != source.Last<string>())
            cmdText = cmdText + str + Environment.NewLine + " UNION ALL" + Environment.NewLine + Environment.NewLine;
          else
            cmdText += str;
        }
      }
      return db.ExecuteTableQuery((IDbCommand) new SqlCommand(cmdText), EllieMae.EMLite.DataAccess.ServerGlobals.ReportSQLTimeout, DbTransactionType.Snapshot);
    }

    private string formatFieldValue(object value, FieldFormat format)
    {
      return Utils.ApplyFieldFormatting(string.Concat(value), format, false);
    }

    private bool formatEnumFieldValue(ref string dictItem, object value, string columnName)
    {
      if (value.ToString() == "")
      {
        dictItem = "";
        return false;
      }
      string str1 = "";
      string str2 = "";
      int length = columnName.IndexOf(".");
      int num = columnName.Length - length;
      if (columnName.IndexOf(".") > -1)
      {
        str1 = columnName.Substring(length + 1, num - 1);
        str2 = columnName.Substring(0, length);
      }
      switch (str1.ToLower())
      {
        case "autocreated":
        case "docreqindicator":
        case "docsubmissionindicator":
        case "ginniepoolconcurrenttransferindicator":
        case "isassumability":
        case "isballoon":
        case "isbondfinancepool":
        case "isguarantyfeeaddon":
        case "isinterestonly":
        case "ismultifamily":
        case "issent1711tocustodian":
        case "locked":
        case "weightedavgpricelocked":
          dictItem = (bool) value ? "Y" : "N";
          return true;
        case "commitmenttype":
          switch (str2)
          {
            case "CorrespondentTradeDetails":
              dictItem = ((CorrespondentTradeCommitmentType) Utils.ParseInt((object) value.ToString())).ToDescription();
              return true;
            case "CorrespondentMaster":
              dictItem = ((MasterCommitmentType) Utils.ParseInt((object) value.ToString())).ToDescription();
              return true;
            default:
              return false;
          }
        case "deliverytype":
          dictItem = ((CorrespondentMasterDeliveryType) value).ToDescription();
          return true;
        case "poolmortgagetype":
          dictItem = ((MbsPoolMortgageType) value).ToDescription();
          return true;
        case "servicingtype":
          dictItem = ((ServicingType) value).ToDescription();
          return true;
        case "status":
          dictItem = !(str2 == "MasterContracts") ? ((TradeStatus) value).ToDescription() : (value.ToString() == "0" ? "Active" : "Archived");
          return true;
        case "term":
          if (!(str2 == "MasterContracts"))
            return false;
          if (value.ToString() == "1")
            dictItem = "Monthly";
          else if (value.ToString() == "2")
            dictItem = "Quarterly";
          else if (value.ToString() == "3")
            dictItem = "Annually";
          return true;
        default:
          return false;
      }
    }
  }
}
