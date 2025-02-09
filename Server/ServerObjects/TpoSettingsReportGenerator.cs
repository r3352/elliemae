// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.TpoSettingsReportGenerator
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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public class TpoSettingsReportGenerator
  {
    private const string className = "TPOSettingsReportGenerator�";
    private UserInfo currentUser;
    private TpoSettingsReportParameters parameters;
    private IServerProgressFeedback feedback;
    private ManualResetEvent stopEvent;
    private ReportResults reportResults;
    private Exception reportError;
    private TpoSettingsQuery queryEngine;
    private List<EllieMae.EMLite.ClientServer.Reporting.ColumnInfo> fields;

    public TpoSettingsReportGenerator(
      UserInfo currentUser,
      TpoSettingsReportParameters parameters,
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
        Err.Raise("TPOSettingsReportGenerator", new ServerException("Report failed due to an error: " + this.reportError.Message, this.reportError));
      return this.reportResults;
    }

    private void generateAsync(object contextObj)
    {
      try
      {
        using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("TPO Report Generation", 76, nameof (generateAsync), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\TpoSettingsReportGenerator.cs"))
        {
          ClientContext clientContext = (ClientContext) contextObj;
          this.reportResults = new ReportResults();
          performanceMeter.AddNote("Report includes " + (object) this.parameters.Fields.Count + " fields: " + string.Join(", ", this.parameters.GetSelectionFieldIDs()));
          using (clientContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
            this.generateReport(true);
        }
      }
      catch (Exception ex)
      {
        this.reportError = ex;
        TraceLog.WriteException("TPOSettingsReportGenerator", ex);
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
      this.queryEngine = new TpoSettingsQuery(this.currentUser);
      PerformanceMeter.Current.AddCheckpoint("Prepared combined filter for report", 114, nameof (generateReport), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\TpoSettingsReportGenerator.cs");
      if (this.feedback != null && !this.feedback.SetFeedback("Retrieving data...", (string) null, 1))
        return;
      Dictionary<int, string[]> dictionary = new Dictionary<int, string[]>();
      using (EllieMae.EMLite.Server.DbAccessManager db = new EllieMae.EMLite.Server.DbAccessManager())
      {
        this.formatFilterValue(this.parameters.FieldFilters);
        string identitySelectionQuery = this.queryEngine.CreateIdentitySelectionQuery(this.parameters.CreateCombinedFilter(), isExternalOrganization);
        string str1 = "create table #tpo_ids ( TpoId int, HierarchyPath varchar(max))" + Environment.NewLine;
        string str2 = !((identitySelectionQuery ?? "") == "") ? identitySelectionQuery : "select oid from " + this.queryEngine.PrimaryKeyTableIdentifier;
        if (!this.parameters.IncludeChildFolder)
          str2 = (this.parameters.CreateCombinedFilter() == null ? str2 + " where" : str2 + " and") + " Parent = 0 ";
        string sql = (str1 + "insert into #tpo_ids " + str2 + " order by HierarchyPath" + Environment.NewLine).Replace("distinct TpoSettingsDetails.oid", "distinct TpoSettingsDetails.oid, HierarchyPath") + "select count(*) from #tpo_ids" + Environment.NewLine;
        DataSet dataSet = db.ExecuteSetQuery(sql, EllieMae.EMLite.DataAccess.ServerGlobals.ReportSQLTimeout, DbTransactionType.Snapshot);
        int num = 0;
        if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
          num = (int) dataSet.Tables[0].Rows[0][0];
        PerformanceMeter.Current.AddCheckpoint("Built temp table with filtered contact IDs", 165, nameof (generateReport), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\TpoSettingsReportGenerator.cs");
        if (num == 0 || this.feedback != null && !this.feedback.SetFeedback("Aggregating field data...", (string) null, 2))
          return;
        this.fields = new List<EllieMae.EMLite.ClientServer.Reporting.ColumnInfo>();
        for (int index = 0; index < this.parameters.Fields.Count; ++index)
        {
          EllieMae.EMLite.ClientServer.Reporting.ColumnInfo field = this.parameters.Fields[index];
          if (!field.IsExcelField)
            this.fields.Add(field);
        }
        DataTable dataTable = (DataTable) null;
        using (PerformanceMeter.Current.BeginOperation("Retrieve report field data"))
          dataTable = this.queryForFieldValues(this.fields.Select<EllieMae.EMLite.ClientServer.Reporting.ColumnInfo, string>((System.Func<EllieMae.EMLite.ClientServer.Reporting.ColumnInfo, string>) (c => c.CriterionName)).ToArray<string>(), db, "#tpo_ids");
        for (int index1 = 0; index1 < dataTable.Rows.Count; ++index1)
        {
          dictionary[index1] = new string[this.fields.Count + 1];
          for (int index2 = 1; index2 < this.fields.Count + 1; ++index2)
          {
            string columnName = dataTable.Columns[index2].ColumnName.Replace("__", ".");
            string lower = this.parameters.Fields[index2 - 1].FieldID.ToLower();
            if (!this.formatEnumFieldValue(ref dictionary[index1][index2 - 1], dataTable.Rows[index1][index2], columnName, lower) && !this.formatSpecialFieldValue(ref dictionary[index1][index2 - 1], dataTable.Rows[index1][index2], columnName))
            {
              string str3 = this.formatFieldValue(dataTable.Rows[index1][index2], this.fields[index2 - 1].Format);
              dictionary[index1][index2 - 1] = str3;
            }
          }
          dictionary[index1][this.fields.Count] = dataTable.Rows[index1][0].ToString();
        }
        if (this.feedback != null)
        {
          if (!this.feedback.Increment(1))
            return;
        }
      }
      if (this.feedback != null && !this.feedback.SetFeedback("Compiling results...", (string) null, this.parameters.Fields.Count + 3))
        return;
      foreach (int key in dictionary.Keys)
        this.reportResults.Add(dictionary[key]);
      if (this.feedback != null)
        this.feedback.SetFeedback("Report Completed.", (string) null, this.parameters.Fields.Count + 4);
      PerformanceMeter.Current.AddCheckpoint("Retrieved report field data from database", 285, nameof (generateReport), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\TpoSettingsReportGenerator.cs");
    }

    private DataTable queryForFieldValues(string[] fields, EllieMae.EMLite.Server.DbAccessManager db, string guidTable)
    {
      IQueryTerm[] fields1 = (IQueryTerm[]) DataField.CreateFields(fields);
      this.queryEngine.GetFieldSelectionList(fields1);
      string fieldJoinClause = this.queryEngine.GetFieldJoinClause(fields1, (QueryCriterion) null, (SortField[]) null);
      List<FieldSource> fieldSourceList = new List<FieldSource>();
      if (((IEnumerable<string>) fields).Contains<string>("CompanySettingsBroker.EPPSPriceGroupBroker"))
      {
        FieldSource fieldSource = this.queryEngine.GetFieldSource("eppspricegroupbroker");
        if (fieldSource != null && !fieldSourceList.Contains(fieldSource))
          fieldSourceList.Add(fieldSource);
      }
      if (((IEnumerable<string>) fields).Contains<string>("CompanySettingsDel.EPPSPriceGroupDel"))
      {
        FieldSource fieldSource = this.queryEngine.GetFieldSource("eppspricegroupdel");
        if (fieldSource != null && !fieldSourceList.Contains(fieldSource))
          fieldSourceList.Add(fieldSource);
      }
      if (((IEnumerable<string>) fields).Contains<string>("CompanySettingsNonDel.EPPSPriceGroupNonDel"))
      {
        FieldSource fieldSource = this.queryEngine.GetFieldSource("eppspricegroupnondel");
        if (fieldSource != null && !fieldSourceList.Contains(fieldSource))
          fieldSourceList.Add(fieldSource);
      }
      string str1 = "";
      foreach (FieldSource fieldSource in fieldSourceList)
      {
        if (str1 != "")
          str1 += Environment.NewLine;
        string str2 = str1 + fieldSource.JoinClause;
        fieldJoinClause += str2;
        str1 = "";
      }
      string empty = string.Empty;
      string cmdText = "select " + this.queryEngine.PrimaryKeyIdentifier + ", " + this.queryEngine.GetFieldSelectionList((IQueryTerm[]) DataField.CreateFields(fields)) + " from " + this.queryEngine.PrimaryKeyTableIdentifier + " Inner join " + guidTable + "  as A on A.TpoId = " + this.queryEngine.PrimaryKeyIdentifier + " " + fieldJoinClause + Environment.NewLine;
      return db.ExecuteTableQuery((IDbCommand) new SqlCommand(cmdText), EllieMae.EMLite.DataAccess.ServerGlobals.ReportSQLTimeout, DbTransactionType.Snapshot);
    }

    private string formatFieldValue(object value, FieldFormat format)
    {
      return Utils.ApplyFieldFormatting(string.Concat(value), format, false);
    }

    private bool formatSpecialFieldValue(ref string dictItem, object value, string columnName)
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
        case "brokerendtime":
        case "brokeronrpsatendtime":
        case "brokeronrpsatstarttime":
        case "brokeronrpsunendtime":
        case "brokeronrpsunstarttime":
        case "brokerstarttime":
        case "correspondentendtime":
        case "correspondentonrpsatendtime":
        case "correspondentonrpsatstarttime":
        case "correspondentonrpsunendtime":
        case "correspondentonrpsunstarttime":
        case "correspondentstarttime":
          DateTime result;
          dictItem = !DateTime.TryParse(value.ToString(), out result) ? "" : result.ToString("h:mm tt") + " ET";
          return true;
        case "brokertolerance":
        case "correspondenttolerance":
          if (!(value.ToString() == "0.0000"))
            return false;
          dictItem = "";
          return true;
        default:
          return false;
      }
    }

    private bool formatEnumFieldValue(
      ref string dictItem,
      object value,
      string columnName,
      string columnId)
    {
      if (value.ToString() == "")
      {
        dictItem = "";
        return false;
      }
      if (columnId.ToLower() == "orgtype")
      {
        if (value.ToString() == "0")
          dictItem = "Company";
        if (value.ToString() == "1")
          dictItem = "Company Extension";
        if (value.ToString() == "2")
          dictItem = "Branch";
        if (value.ToString() == "3")
          dictItem = "Branch Extension";
        return true;
      }
      if (columnId.ToLower() == "brokgeneratedisclosures")
      {
        if (value.ToString() == "0")
          dictItem = "Disable Fee Management";
        if (value.ToString() == "1")
          dictItem = "Request LE & Disclosures";
        if (value.ToString() == "2")
          dictItem = "Generate LE";
        if (value.ToString() == "3")
          dictItem = "Generate LE & Disclosures";
        return true;
      }
      if (columnId == "brokusechdef")
      {
        dictItem = value.ToString() == "Y" ? "Y" : "N";
        return true;
      }
      if (columnId == "brokcustomizesettings")
      {
        dictItem = value.ToString() == "Y" ? "N" : "Y";
        return true;
      }
      if (columnId == "corrusechdef")
      {
        dictItem = value.ToString() == "Y" ? "Y" : "N";
        return true;
      }
      if (columnId == "corrcustomizesettings")
      {
        dictItem = value.ToString() == "Y" ? "N" : "Y";
        return true;
      }
      if (columnId == "brokspecifytime")
      {
        dictItem = value.ToString() == "Y" ? "Y" : "N";
        return true;
      }
      if (columnId == "brokcontinuouscov")
      {
        dictItem = value.ToString() == "Y" ? "N" : "Y";
        return true;
      }
      if (columnId == "corrspecifytime")
      {
        dictItem = value.ToString() == "Y" ? "Y" : "N";
        return true;
      }
      if (columnId == "corrcontinuouscov")
      {
        dictItem = value.ToString() == "Y" ? "N" : "Y";
        return true;
      }
      string str = this.fields.Where<EllieMae.EMLite.ClientServer.Reporting.ColumnInfo>((System.Func<EllieMae.EMLite.ClientServer.Reporting.ColumnInfo, bool>) (selectedfield => selectedfield.ID.ToLower().Equals(columnId))).Select<EllieMae.EMLite.ClientServer.Reporting.ColumnInfo, string>((System.Func<EllieMae.EMLite.ClientServer.Reporting.ColumnInfo, string>) (selectedfield => selectedfield.CriterionName)).First<string>();
      int num1 = str.IndexOf(".");
      int num2 = str.Length - num1;
      if (str.IndexOf(".") > -1)
        str = str.Substring(num1 + 1, num2 - 1);
      switch (str.ToLower())
      {
        case "OrganizationType":
          if (value.ToString() == "0")
            dictItem = "Company";
          else if (value.ToString() == "1")
            dictItem = "Company Extension";
          else if (value.ToString() == "2")
            dictItem = "Branch";
          return true;
        case "allowloanswithissues":
          if (value.ToString() == "0")
            dictItem = "No Restrictions";
          else if (value.ToString() == "1")
            dictItem = "Don't allow lock or submission";
          else if (value.ToString() == "2")
            dictItem = "Don't allow loan creation";
          else if (value.ToString() == "3")
            dictItem = "Don't allow all - loan creation/lock/submission";
          return true;
        case "atrqmexemptcreditor":
          switch (value.ToString())
          {
            case "1":
              dictItem = "Community Development Financial Institution";
              break;
            case "2":
              dictItem = "Community Housing Development Organization";
              break;
            case "3":
              dictItem = "Downpayment Assistance Providern";
              break;
            case "4":
              dictItem = "Nonprofit Organization";
              break;
            default:
              dictItem = "";
              break;
          }
          return true;
        case "atrqmsmallcreditor":
          switch (value.ToString())
          {
            case "1":
              dictItem = "Small Creditor";
              break;
            case "2":
              dictItem = "Rural Small Creditor";
              break;
            default:
              dictItem = "";
              break;
          }
          return true;
        case "bizentitytype":
          switch (value.ToString())
          {
            case "1":
              dictItem = "Individual";
              return true;
            case "2":
              dictItem = "Sole Proprietorship";
              return true;
            case "3":
              dictItem = "Partnership";
              return true;
            case "4":
              dictItem = "Corporation";
              return true;
            case "5":
              dictItem = "Limited Liability Company";
              return true;
            case "6":
              dictItem = "Other (please specify)";
              return true;
            default:
              dictItem = "";
              return true;
          }
        case "brokerenabled":
        case "brokernolimit":
        case "brokerusechanneldefault":
        case "brokerwhcoverage":
        case "correspondentenabled":
        case "correspondentnolimit":
        case "correspondentusechanneldefault":
        case "correspondentwhcoverage":
          dictItem = value.ToString();
          return true;
        case "cancloseinownname":
        case "canfundinownname":
        case "dusponsored":
        case "lpasponsored":
          switch (value.ToString())
          {
            case "1":
              dictItem = "Y";
              break;
            case "2":
              dictItem = "N";
              break;
            default:
              dictItem = "";
              break;
          }
          return true;
        case "commitmentexceedpolicy":
          if (value.ToString() == "Y")
            dictItem = "Don't allow Lock";
          else if (value.ToString() == "N")
            dictItem = "No Restrictions";
          return true;
        case "commitmentexceedtradepolicy":
          if (value.ToString() == "Y")
            dictItem = "Don't allow Trade creation";
          else if (value.ToString() == "N")
            dictItem = "No Restrictions";
          return true;
        case "companyrating":
          return TpoSettingsReportGenerator.getFieldValueByColumn(out dictItem, value, "Company Rating");
        case "currentapprovalstatus":
          return TpoSettingsReportGenerator.getFieldValueByColumn(out dictItem, value, "Current Company Status");
        case "entitytype":
          switch (value.ToString())
          {
            case "1":
              dictItem = "Broker";
              break;
            case "2":
              dictItem = "Correspondent";
              break;
            case "3":
              dictItem = "Broker and Correspondent";
              break;
            default:
              dictItem = "";
              break;
          }
          return true;
        case "eppscompmodel":
          switch (value.ToString())
          {
            case "0":
              dictItem = "Borrower Only";
              return true;
            case "1":
              dictItem = "Creditor Only";
              return true;
            case "2":
              dictItem = "Borrower or Creditor";
              return true;
            default:
              dictItem = "";
              return true;
          }
        case "eppspricegroup":
          dictItem = value.ToString() == "-1" ? "" : value.ToString();
          return true;
        case "inheritparentfhava":
        case "useparentinfo":
          dictItem = (bool) value ? "Y" : "N";
          return true;
        case "licensestatutorykansas":
          switch (value.ToString())
          {
            case "False":
              dictItem = "No Statutory Election";
              break;
            case "True":
              dictItem = "Kansas UCCC Election For All Loans";
              break;
            default:
              dictItem = "";
              break;
          }
          return true;
        case "licensestatutorymaryland":
          switch (value.ToString())
          {
            case "False":
              dictItem = "No Statutory Election";
              break;
            case "True":
              dictItem = "Credit Grantor Law Election (Junior Liens Only)";
              break;
            default:
              dictItem = "";
              break;
          }
          return true;
        default:
          return false;
      }
    }

    private static bool getFieldValueByColumn(out string dictItem, object value, string columnName)
    {
      if (!string.IsNullOrEmpty(value.ToString()) && int.Parse(value.ToString()) > 0)
      {
        IEnumerable<string> source = ExternalOrgManagementAccessor.GetExternalOrgSettingsByName(columnName).Where<ExternalSettingValue>((System.Func<ExternalSettingValue, bool>) (item => item.settingId.Equals(int.Parse(value.ToString())))).Select<ExternalSettingValue, string>((System.Func<ExternalSettingValue, string>) (item => item.settingValue));
        if (source.Count<string>() > 0)
        {
          dictItem = source.First<string>();
          return true;
        }
      }
      dictItem = "";
      return true;
    }

    private void formatFilterValue(FieldFilterList list)
    {
      foreach (FieldFilter fieldFilter in (List<FieldFilter>) list)
      {
        if (fieldFilter.FieldID.ToLower() == "brokonrpstarttime" || fieldFilter.FieldID.ToLower() == "corronrpstarttime")
          fieldFilter.ValueFrom = fieldFilter.ValueDescription.Replace("EST", "").Trim();
        if (fieldFilter.FieldID.ToLower() == "brokonrpendtime" || fieldFilter.FieldID.ToLower() == "corronrpendtime")
        {
          fieldFilter.ValueFrom = fieldFilter.ValueDescription.Replace("EST", "").Trim();
          DateTime result;
          if (DateTime.TryParse(fieldFilter.ValueFrom, out result))
            fieldFilter.ValueFrom = result.ToString("H:mm");
        }
      }
    }
  }
}
