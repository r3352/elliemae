// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanReportGenerator
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Interface;
using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class LoanReportGenerator
  {
    private const string className = "LoanReportGenerator�";
    private UserInfo currentUser;
    private LoanReportParameters parameters;
    private IServerProgressFeedback feedback;
    private ManualResetEvent stopEvent;
    private ReportResults reportResults;
    private Exception reportError;
    private FieldAccessRuleEvaluator fieldRuleEvaluator;
    private EllieMae.EMLite.Server.Query.LoanQuery queryEngine;
    private IEnumerable<EllieMae.EMLite.Workflow.Milestone> milestones;

    public LoanReportGenerator(
      UserInfo currentUser,
      LoanReportParameters parameters,
      IServerProgressFeedback feedback)
    {
      this.currentUser = currentUser;
      this.parameters = parameters;
      this.feedback = feedback;
      bool flag = ClientContext.GetCurrent().UseERDB;
      flag = false;
      this.queryEngine = parameters.Folders == null ? new EllieMae.EMLite.Server.Query.LoanQuery(currentUser, false, LoanInfo.Right.Read, (ICriterionTranslator) new LoanReportCriterionTranslator(LoanXDBStore.GetLoanXDBTableList())) : new EllieMae.EMLite.Server.Query.LoanQuery(currentUser, false, LoanInfo.Right.Read, (ICriterionTranslator) new LoanReportCriterionTranslator(LoanXDBStore.GetLoanXDBTableList()), parameters.Folders);
      this.fieldRuleEvaluator = new FieldAccessRuleEvaluator((FieldAccessRuleInfo[]) BpmDbAccessor.GetAccessor(BizRuleType.FieldAccess).GetRules(true));
      this.milestones = (IEnumerable<EllieMae.EMLite.Workflow.Milestone>) WorkflowBpmDbAccessor.GetMilestones(false);
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
      {
        if (this.reportError is FieldNotInDBException)
        {
          FieldNotInDBException reportError = (FieldNotInDBException) this.reportError;
          if (reportError.IsMilestoneField)
            Err.Raise(nameof (LoanReportGenerator), new ServerException("The '" + reportError.MilestoneName + "' milestone (i.e., the ‘" + reportError.FieldID + "’ field) is missing from the Reporting Database. Please contact your system administrator to add this field, and then try to generate the report again.", this.reportError));
          else
            Err.Raise(nameof (LoanReportGenerator), new ServerException("Report failed due to an error: " + this.reportError.Message, this.reportError));
        }
        else
          Err.Raise(nameof (LoanReportGenerator), new ServerException("Report failed due to an error: " + this.reportError.Message, this.reportError));
      }
      return this.reportResults;
    }

    private void generateAsync(object contextObj)
    {
      try
      {
        using (PerformanceMeter performanceMeter1 = PerformanceMeter.StartNew("Loan Report Generation", 108, nameof (generateAsync), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\LoanReportGenerator.cs"))
        {
          ClientContext clientContext = (ClientContext) contextObj;
          this.reportResults = new ReportResults();
          PerformanceMeter performanceMeter2 = performanceMeter1;
          bool flag = this.parameters.UseDBField;
          string note1 = "Use Reporting DB for fields = " + flag.ToString();
          performanceMeter2.AddNote(note1);
          PerformanceMeter performanceMeter3 = performanceMeter1;
          flag = this.parameters.UseDBFilter;
          string note2 = "Use Reporting DB for filters = " + flag.ToString();
          performanceMeter3.AddNote(note2);
          PerformanceMeter performanceMeter4 = performanceMeter1;
          flag = this.parameters.UseExternalOrganization;
          string note3 = "Use Reporting DB for externalOrganization = " + flag.ToString();
          performanceMeter4.AddNote(note3);
          performanceMeter1.AddNote("Report includes " + (object) this.parameters.Fields.Count + " fields: " + string.Join(", ", this.parameters.GetSelectionFieldIDs()));
          using (clientContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
          {
            if (this.parameters.RequiresDirectLoanAccess)
              this.generateFromLoanFiles(this.parameters.UseExternalOrganization);
            else
              this.generateFromReportingDb(this.parameters.UseExternalOrganization, this.parameters.ExcludeArchiveLoans);
          }
        }
      }
      catch (Exception ex)
      {
        this.reportError = ex;
        TraceLog.WriteException(nameof (LoanReportGenerator), ex);
        this.reportResults = (ReportResults) null;
      }
      finally
      {
        this.stopEvent.Set();
      }
    }

    private void generateFromReportingDb(bool isExternalOrganization, bool excludeArchivedLoans)
    {
      if (this.feedback != null)
        this.feedback.Status = "Preparing to retrieve report data...";
      QueryCriterion combinedFilter = this.parameters.CreateCombinedFilter();
      PerformanceMeter.Current.AddCheckpoint("Prepared combined filter for report", 151, nameof (generateFromReportingDb), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\LoanReportGenerator.cs");
      if (this.feedback != null)
        this.feedback.ResetCounter(this.parameters.Fields.Count + 4);
      if (this.feedback != null && !this.feedback.SetFeedback("Retrieving data...", (string) null, 1))
        return;
      Dictionary<string, PipelineInfo> pinfos = new Dictionary<string, PipelineInfo>();
      Dictionary<string, string[]> loanData1 = new Dictionary<string, string[]>();
      string identitySelectionQuery = this.queryEngine.CreateIdentitySelectionQuery(combinedFilter, true, isExternalOrganization, excludeArchivedLoans: excludeArchivedLoans);
      LoanReportGeneratorUtil.ReturnResult fromReportingDb = ERDBSession.GenerateFromReportingDb(false, ClientContext.GetCurrent(), this.currentUser, (EllieMae.EMLite.ReportingDbUtils.Query.LoanQuery) this.queryEngine, pinfos, loanData1, this.parameters, identitySelectionQuery, this.feedback);
      Dictionary<string, PipelineInfo> pipelineInfos = fromReportingDb.PipelineInfos;
      Dictionary<string, string[]> loanData2 = fromReportingDb.LoanData;
      PerformanceMeter.Current.AddCheckpoint("Retrieved report field data from database", 242, nameof (generateFromReportingDb), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\LoanReportGenerator.cs");
      if (this.feedback != null && !this.feedback.SetFeedback("Applying business rules...", (string) null, this.parameters.Fields.Count + 3))
        return;
      foreach (string key in loanData2.Keys)
      {
        if (!this.currentUser.IsSuperAdministrator())
          this.applyFieldAccessRights(loanData2[key], pipelineInfos[key]);
        this.reportResults.Add(loanData2[key]);
      }
      PerformanceMeter.Current.AddCheckpoint("Applied business rules", 259, nameof (generateFromReportingDb), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\LoanReportGenerator.cs");
      if (this.feedback == null)
        return;
      this.feedback.SetFeedback((string) null, (string) null, this.parameters.Fields.Count + 4);
    }

    private string createReportQuery(string[] reportFields, bool isExternalOrganization)
    {
      QueryCriterion combinedFilter = this.parameters.CreateCombinedFilter();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("-- Create a temp table for the loan guid to return");
      dbQueryBuilder.AppendLine("declare @loan_guids table ( guid varchar(38) primary key )");
      string identitySelectionQuery = this.queryEngine.CreateIdentitySelectionQuery(combinedFilter, isExternalOrganization);
      if ((identitySelectionQuery ?? "") != "")
        dbQueryBuilder.AppendLine("insert into @loan_guids " + identitySelectionQuery);
      else
        dbQueryBuilder.AppendLine("insert into @loan_guids select Guid from LoanSummary");
      string[] ruleFields = LoanBusinessRuleInfo.RuleFields;
      dbQueryBuilder.AppendLine("select Loan.Guid, " + string.Join(", ", ruleFields) + " from LoanSummary Loan inner join @loan_guids Guids on Loan.Guid = Guids.Guid");
      foreach (string reportField in reportFields)
      {
        IQueryTerm[] fields = (IQueryTerm[]) DataField.CreateFields(new string[1]
        {
          reportField
        });
        string fieldSelectionList = this.queryEngine.GetFieldSelectionList(fields);
        string fieldJoinClause = this.queryEngine.GetFieldJoinClause(fields, (QueryCriterion) null, (SortField[]) null);
        dbQueryBuilder.AppendLine("select Loan.Guid, " + fieldSelectionList + " from LoanSummary Loan " + fieldJoinClause + " inner join @loan_guids Guids on Loan.Guid = Guids.Guid");
      }
      return dbQueryBuilder.ToString();
    }

    private void generateFromLoanFiles(bool isExternalOrganization)
    {
      LoanIdentity[] loanIds = this.queryCandidateLoans();
      PerformanceMeter.Current.AddCheckpoint("Built candidate loan list", 350, nameof (generateFromLoanFiles), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\LoanReportGenerator.cs");
      LoanXDBStatusInfo loanXdbStatus = LoanXDBStore.GetLoanXDBStatus(false);
      this.extractDataFromLoanFiles(loanIds, isExternalOrganization);
      PerformanceMeter.Current.AddCheckpoint("Extracted data from loan files", 358, nameof (generateFromLoanFiles), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\LoanReportGenerator.cs");
      if (!this.parameters.UseDBField && !this.parameters.UseDBFilter || !(loanXdbStatus.LastModified != LoanXDBStore.GetLoanXDBStatus(false).LastModified))
        return;
      this.reportResults.SetWarningMessage("The reporting database scheme has been changed during report processing. The data in this report might be not accurate.");
    }

    private void extractDataFromLoanFiles(LoanIdentity[] loanIds, bool isExternalOrganization)
    {
      if (!this.feedback.ResetCounter(loanIds.Length))
        return;
      string[] selectionFieldIds = this.parameters.GetSelectionFieldIDs();
      string[] fieldIdList = this.parameters.FieldFilters.GetFieldIDList();
      FilterEvaluator filterEvaluator = (FilterEvaluator) null;
      if (!this.parameters.UseDBFilter && fieldIdList.Length != 0)
        filterEvaluator = this.parameters.FieldFilters.CreateEvaluator();
      int num1 = loanIds.Length / 10;
      if (num1 == 0)
        num1 = 1;
      int num2 = Utils.ParseInt(EnConfigurationSettings.GlobalSettings["MaxProgressInterval", (object) "50"]);
      if (num2 > 0 && num1 > num2)
        num1 = num2;
      for (int index = 0; index < loanIds.Length; ++index)
      {
        if (index % num1 == 0 && this.feedback != null)
        {
          if (!this.feedback.SetFeedback("Generating Loan Report...", "Completed " + index.ToString("#,##0") + " of " + loanIds.Length.ToString("#,##0") + " loans...", index))
            break;
        }
        try
        {
          string[] strArray = (string[]) null;
          using (Loan loan = LoanStore.CheckOut(loanIds[index].Guid))
          {
            if (loan.LoanData.SnapshotProvider == null)
              loan.LoanData.AttachSnapshotProvider((ILoanSnapshotProvider) new LoanSnapshotProvider(loan));
            if (filterEvaluator != null)
            {
              string[] fieldValues = (string[]) null;
              using (PerformanceMeter.Current.BeginOperation("Loan.SelectFields (Filter)"))
                fieldValues = loan.SelectFields(fieldIdList);
              PipelineInfo pipelineInfo = this.fieldListToPipelineInfo(fieldIdList, fieldValues);
              using (PerformanceMeter.Current.BeginOperation("Filter.Evaluate"))
              {
                if (!filterEvaluator.Evaluate(pipelineInfo, FilterEvaluationOption.None))
                  continue;
              }
            }
            if (!this.parameters.UseDBField)
            {
              using (PerformanceMeter.Current.BeginOperation("Loan.SelectFields (Report)"))
                strArray = loan.SelectFields(selectionFieldIds);
            }
            else
            {
              using (PerformanceMeter.Current.BeginOperation("Extract fields from XDB"))
                strArray = this.getFieldsFromXDatabase(loanIds[index], isExternalOrganization);
            }
            if (strArray != null)
            {
              if (!this.currentUser.IsSuperAdministrator())
              {
                if (strArray != null)
                {
                  if (strArray.Length != 0)
                  {
                    using (PerformanceMeter.Current.BeginOperation("Applying field access rules"))
                      this.applyFieldAccessRights(strArray, loan.GetPipelineFields(isExternalOrganization));
                  }
                }
              }
            }
            else
              continue;
          }
          this.reportResults.Add(strArray);
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (LoanReportGenerator), "The report class can't load " + loanIds[index].LoanName + " file in " + loanIds[index].LoanFolder + " folder. Error: " + (object) ex);
        }
      }
    }

    private PipelineInfo fieldListToPipelineInfo(string[] fieldIds, string[] fieldValues)
    {
      Hashtable info = new Hashtable();
      for (int index = 0; index < fieldIds.Length; ++index)
        info[(object) fieldIds[index]] = (object) fieldValues[index];
      return new PipelineInfo(info, (PipelineInfo.Borrower[]) null, (PipelineInfo.Alert[]) null, (PipelineInfo.LoanAssociateInfo[]) null);
    }

    private void applyFieldAccessRights(string[] data4Report, PipelineInfo pInfo)
    {
      if (pInfo == null)
        return;
      LoanConditions conditions = pInfo.Milestones == null ? LoanBusinessRuleInfo.PipelineInfoForBusinessRule(pInfo, this.milestones) : LoanBusinessRuleInfo.PipelineInfoForBusinessRule(pInfo);
      if (conditions.CurrentMilestoneID == "0")
        return;
      Hashtable hashtable = (Hashtable) this.fieldRuleEvaluator.Evaluate(this.currentUser.UserPersonas, conditions);
      if (hashtable == null)
        return;
      string[] selectionFieldIds = this.parameters.GetSelectionFieldIDs();
      for (int index = 0; index < selectionFieldIds.Length; ++index)
      {
        string key;
        if (selectionFieldIds[index].StartsWith("AuditTrail"))
        {
          string str = selectionFieldIds[index].Replace("AuditTrail.", "");
          string[] strArray = str.Split('.');
          key = str.Replace("." + strArray[strArray.Length - 1], "");
        }
        else
          key = selectionFieldIds[index];
        if (hashtable.ContainsKey((object) key) && (BizRule.FieldAccessRight) hashtable[(object) key] == BizRule.FieldAccessRight.Hide)
          data4Report[index] = "*";
      }
    }

    private LoanIdentity[] queryCandidateLoans()
    {
      QueryCriterion filter = this.parameters.CustomFilter;
      if (this.parameters.UseDBFilter && this.parameters.FieldFilters.Count > 0)
        filter = filter == null ? this.parameters.FieldFilters.CreateEvaluator().ToQueryCriterion() : filter.And(this.parameters.FieldFilters.CreateEvaluator().ToQueryCriterion());
      return Pipeline.GenerateIdentities(this.currentUser, LoanInfo.Right.Read, filter, this.queryEngine.FieldTranslator, this.parameters.UseExternalOrganization);
    }

    private string[] getFieldsFromXDatabase(LoanIdentity loanID, bool isExternalOrganization)
    {
      QueryResult queryResult = this.queryEngine.Execute(new DataQuery((IEnumerable) this.parameters.GetSelectionFieldCriterionNames(), (QueryCriterion) new StringValueCriterion("Loan.Guid", loanID.Guid)), isExternalOrganization, loanFolders: this.parameters.Folders);
      if (queryResult.RecordCount == 0)
        return (string[]) null;
      string[] fieldsFromXdatabase = new string[this.parameters.Fields.Count];
      for (int index = 0; index < this.parameters.Fields.Count; ++index)
        fieldsFromXdatabase[index] = LoanReportGeneratorUtil.FormatFieldValue(queryResult[0, index], this.parameters.Fields[index].Format);
      return fieldsFromXdatabase;
    }
  }
}
