// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ContactReportGenerator
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils.Query;
using EllieMae.EMLite.Server.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class ContactReportGenerator
  {
    private const string className = "ContactReportGenerator�";
    private UserInfo currentUser;
    private ContactReportParameters parameters;
    private IServerProgressFeedback feedback;
    private ManualResetEvent stopEvent;
    private ReportResults reportResults;
    private Exception reportError;
    private QueryEngine filterEngine;
    private QueryEngine fieldEngine;

    public ContactReportGenerator(
      UserInfo currentUser,
      ContactReportParameters parameters,
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
        Err.Raise(nameof (ContactReportGenerator), new ServerException("Report failed due to an error: " + this.reportError.Message, this.reportError));
      return this.reportResults;
    }

    private void generateAsync(object contextObj)
    {
      try
      {
        using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("Contact Report Generation", 75, nameof (generateAsync), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\ContactReportGenerator.cs"))
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
        TraceLog.WriteException(nameof (ContactReportGenerator), ex);
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
      if (this.parameters.ContactType == ContactType.Borrower)
      {
        this.filterEngine = (QueryEngine) new BorrowerQuery(this.currentUser, this.parameters.FieldFilters.RelatedLoanMatchType);
        this.fieldEngine = (QueryEngine) new BorrowerQuery(this.currentUser, this.parameters.LoanFieldSelectionSource);
      }
      else
      {
        this.filterEngine = (QueryEngine) new BizPartnerQuery(this.currentUser, this.parameters.FieldFilters.RelatedLoanMatchType);
        this.fieldEngine = (QueryEngine) new BizPartnerQuery(this.currentUser, this.parameters.LoanFieldSelectionSource);
      }
      QueryCriterion combinedFilter = this.parameters.CreateCombinedFilter();
      PerformanceMeter.Current.AddCheckpoint("Prepared combined filter for report", 121, nameof (generateReport), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\ContactReportGenerator.cs");
      if (this.feedback != null)
        this.feedback.ResetCounter(this.parameters.Fields.Count + 4);
      if (this.feedback != null && !this.feedback.SetFeedback("Retrieving data...", (string) null, 1))
        return;
      Dictionary<int, string[]> dictionary = new Dictionary<int, string[]>();
      using (DbAccessManager db = new DbAccessManager())
      {
        string str1 = this.filterEngine.CreateIdentitySelectionQuery(combinedFilter, isExternalOrganization);
        string str2 = "create table #contact_ids ( ContactID int PRIMARY KEY )" + Environment.NewLine;
        string str3;
        if ((str1 ?? "") == "")
        {
          str3 = str2 + "insert into #contact_ids select ContactID from " + this.filterEngine.PrimaryKeyTableIdentifier + Environment.NewLine;
        }
        else
        {
          if (str1.StartsWith("SELECT distinct ", StringComparison.CurrentCultureIgnoreCase))
            str1 = "SELECT " + str1.Substring("SELECT distinct ".Length) + " GROUP BY " + this.filterEngine.PrimaryKeyIdentifier;
          str3 = str2 + "insert into #contact_ids " + str1 + Environment.NewLine;
        }
        string sql = str3 + "select count(*) from #contact_ids";
        int num = (int) db.ExecuteScalar(sql, EllieMae.EMLite.DataAccess.ServerGlobals.ReportSQLTimeout);
        PerformanceMeter.Current.AddCheckpoint("Built temp table with filtered contact IDs", 156, nameof (generateReport), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\ContactReportGenerator.cs");
        if (num == 0 || this.feedback != null && !this.feedback.SetFeedback("Aggregating field data...", (string) null, 2))
          return;
        for (int index1 = 0; index1 < this.parameters.Fields.Count; ++index1)
        {
          EllieMae.EMLite.ClientServer.Reporting.ColumnInfo field = this.parameters.Fields[index1];
          if (!field.IsExcelField)
          {
            DataTable dataTable = (DataTable) null;
            using (PerformanceMeter.Current.BeginOperation("Retrieve report field data"))
              dataTable = this.queryForFieldValues(new string[1]
              {
                field.CriterionName
              }, db, "#contact_ids");
            for (int index2 = 0; index2 < dataTable.Rows.Count; ++index2)
            {
              int key = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataTable.Rows[index2][0], -1);
              if (!dictionary.ContainsKey(key))
                dictionary[key] = new string[this.parameters.Fields.Count];
              dictionary[key][index1] = this.formatFieldValue(dataTable.Rows[index2][1], field.Format);
            }
            if (this.feedback != null && !this.feedback.Increment(1))
              return;
          }
        }
      }
      if (this.feedback != null && !this.feedback.SetFeedback("Compiling results...", (string) null, this.parameters.Fields.Count + 3))
        return;
      foreach (int key in dictionary.Keys)
        this.reportResults.Add(dictionary[key]);
      if (this.feedback != null)
        this.feedback.SetFeedback("Report Completed.", (string) null, this.parameters.Fields.Count + 4);
      PerformanceMeter.Current.AddCheckpoint("Retrieved report field data from database", 209, nameof (generateReport), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\ContactReportGenerator.cs");
    }

    private DataTable queryForFieldValues(string[] fields, DbAccessManager db, string guidTable)
    {
      for (int index = 0; index < fields.Length; ++index)
      {
        if (fields[index] == "Contact.CategoryID")
        {
          fields[index] = "BizCategory.CategoryName";
          break;
        }
      }
      IQueryTerm[] fields1 = (IQueryTerm[]) DataField.CreateFields(fields);
      string fieldSelectionList = this.fieldEngine.GetFieldSelectionList(fields1);
      string fieldJoinClause = this.fieldEngine.GetFieldJoinClause(fields1, (QueryCriterion) null, (SortField[]) null);
      string cmdText = "select Contact.ContactID, " + fieldSelectionList + " from " + this.fieldEngine.PrimaryKeyTableIdentifier + " inner join " + guidTable + " AccessibleContacts on Contact.ContactID = AccessibleContacts.ContactID " + fieldJoinClause;
      return db.ExecuteTableQuery((IDbCommand) new SqlCommand(cmdText), EllieMae.EMLite.DataAccess.ServerGlobals.ReportSQLTimeout, DbTransactionType.Snapshot);
    }

    private string formatFieldValue(object value, FieldFormat format)
    {
      return Utils.ApplyFieldFormatting(string.Concat(value), format);
    }
  }
}
