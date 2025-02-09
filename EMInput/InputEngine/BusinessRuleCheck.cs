// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.BusinessRuleCheck
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class BusinessRuleCheck
  {
    private const string className = "BusinessRuleCheck";
    private static readonly string sw = Tracing.SwDataEngine;
    private LoanBusinessRuleInfo loanBizInfo;
    private DocMilestonePair[] requiredDocs;
    private FieldMilestonePair[] requiredFields;
    private TaskMilestonePair[] requiredTasks;
    private string[] prerequiredFields;

    public BusinessRuleCheck(LoanData loanData, MilestoneLog mslog)
    {
      this.HasRequirement(loanData, mslog);
    }

    public BusinessRuleCheck()
    {
    }

    public DocMilestonePair[] RequiredDocs => this.requiredDocs;

    public FieldMilestonePair[] RequiredFields => this.requiredFields;

    public TaskMilestonePair[] RequiredTasks => this.requiredTasks;

    public string[] PrerequiredFields => this.prerequiredFields;

    public bool HasRequirement(LoanData loanData, MilestoneLog msLog)
    {
      if (this.loanBizInfo == null)
        this.loanBizInfo = new LoanBusinessRuleInfo(loanData);
      Hashtable hashtable1 = new Hashtable((IEqualityComparer) StringComparer.OrdinalIgnoreCase);
      Hashtable hashtable2 = new Hashtable((IEqualityComparer) StringComparer.OrdinalIgnoreCase);
      Hashtable hashtable3 = new Hashtable((IEqualityComparer) StringComparer.OrdinalIgnoreCase);
      MilestoneRulesBpmManager bpmManager = (MilestoneRulesBpmManager) Session.BPM.GetBpmManager(BpmCategory.MilestoneRules);
      MilestoneLog[] allMilestones = loanData.GetLogList().GetAllMilestones();
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      ArrayList arrayList3 = new ArrayList();
      for (int index = 0; index < allMilestones.Length; ++index)
      {
        bool flag = true;
        if (index < allMilestones.Length - 1 && allMilestones[index + 1].RoleID > RoleInfo.FileStarter.ID && allMilestones[index + 1].LoanAssociateID == "" && msLog != null)
          flag = false;
        if (flag || index == 0)
        {
          LoanConditions loanConditions = flag || index != 0 ? this.loanBizInfo.CurrentLoanForBusinessRule(allMilestones[index], msLog) : this.loanBizInfo.CurrentLoanForBusinessRule((MilestoneLog) null, msLog);
          loanConditions.CheckRoleOnly = index != 0;
          DocMilestonePair[] allRequiredDocs = bpmManager.GetAllRequiredDocs(loanConditions, loanData);
          FieldMilestonePair[] allRequiredFields = bpmManager.GetAllRequiredFields(loanConditions, loanData);
          TaskMilestonePair[] allRequiredTasks = bpmManager.GetAllRequiredTasks(loanConditions, loanData);
          foreach (FieldMilestonePair fieldMilestonePair in allRequiredFields)
          {
            if (!hashtable1.ContainsKey((object) fieldMilestonePair.FieldID))
            {
              arrayList2.Add((object) fieldMilestonePair);
              hashtable1.Add((object) fieldMilestonePair.FieldID, (object) "");
            }
          }
          foreach (DocMilestonePair docMilestonePair in allRequiredDocs)
          {
            if (!hashtable2.ContainsKey((object) (docMilestonePair.DocGuid + docMilestonePair.MilestoneID)))
            {
              arrayList1.Add((object) docMilestonePair);
              hashtable2.Add((object) (docMilestonePair.DocGuid + docMilestonePair.MilestoneID), (object) "");
            }
          }
          foreach (TaskMilestonePair taskMilestonePair in allRequiredTasks)
          {
            if (!hashtable3.ContainsKey((object) (taskMilestonePair.TaskGuid + taskMilestonePair.MilestoneID)))
            {
              arrayList3.Add((object) taskMilestonePair);
              hashtable3.Add((object) (taskMilestonePair.TaskGuid + taskMilestonePair.MilestoneID), (object) "");
            }
          }
        }
        if (msLog == null || string.Compare(allMilestones[index].MilestoneID, msLog.MilestoneID, true) == 0)
          break;
      }
      this.requiredDocs = (DocMilestonePair[]) arrayList1.ToArray(typeof (DocMilestonePair));
      this.requiredFields = (FieldMilestonePair[]) arrayList2.ToArray(typeof (FieldMilestonePair));
      this.requiredTasks = (TaskMilestonePair[]) arrayList3.ToArray(typeof (TaskMilestonePair));
      return (this.requiredDocs != null || this.requiredFields != null || this.requiredTasks != null) && (this.requiredDocs.Length != 0 || this.requiredFields.Length != 0 || this.requiredTasks.Length != 0) && (this.hasBlankRequiredFields(this.requiredFields, loanData, msLog) || this.hasBlankRequiredDocuments(this.requiredDocs, loanData, msLog) || this.hasBlankRequiredTasks(this.requiredTasks, loanData, msLog));
    }

    private bool hasBlankRequiredFields(
      FieldMilestonePair[] requiredFields,
      LoanData loanData,
      MilestoneLog msLog)
    {
      if (requiredFields.Length == 0)
        return false;
      string empty = string.Empty;
      for (int index = 0; index < requiredFields.Length; ++index)
      {
        string simpleField = loanData.GetSimpleField(requiredFields[index].FieldID);
        if (!((simpleField ?? "") != string.Empty) || !(simpleField != "//"))
          return true;
      }
      return false;
    }

    private bool hasBlankRequiredDocuments(
      DocMilestonePair[] requiredDocs,
      LoanData loanData,
      MilestoneLog msLog)
    {
      if (requiredDocs.Length == 0)
        return false;
      DocumentTrackingSetup documentTrackingSetup;
      try
      {
        documentTrackingSetup = Session.LoanDataMgr == null ? Session.ConfigurationManager.GetDocumentTrackingSetup() : Session.LoanDataMgr.SystemConfiguration.DocumentTrackingSetup;
      }
      catch (Exception ex)
      {
        Tracing.Log(BusinessRuleCheck.sw, TraceLevel.Error, nameof (BusinessRuleCheck), "hasBlankRequiredDocuments: can't load document tracking settings. Error: " + ex.Message);
        return false;
      }
      DocumentLog[] allDocuments = loanData.GetLogList().GetAllDocuments(false);
      for (int index = 0; index < requiredDocs.Length; ++index)
      {
        DocumentTemplate byId = documentTrackingSetup.GetByID(requiredDocs[index].DocGuid);
        if (byId == null)
        {
          Tracing.Log(BusinessRuleCheck.sw, TraceLevel.Error, nameof (BusinessRuleCheck), "hasBlankRequiredDocuments: can't find document with GUID '" + requiredDocs[index].DocGuid + "'.");
        }
        else
        {
          DocumentLog documentLog1 = (DocumentLog) null;
          foreach (DocumentLog documentLog2 in allDocuments)
          {
            if (string.Compare(documentLog2.Title, byId.Name, true) == 0)
            {
              documentLog1 = documentLog2;
              break;
            }
          }
          if (documentLog1 == null || !documentLog1.Received)
            return true;
        }
      }
      return false;
    }

    private bool hasBlankRequiredTasks(
      TaskMilestonePair[] requiredTasks,
      LoanData loanData,
      MilestoneLog msLog)
    {
      if (requiredTasks.Length == 0)
        return false;
      Hashtable hashtable;
      try
      {
        hashtable = Session.LoanDataMgr == null ? Session.ConfigurationManager.GetMilestoneTasks() : Session.LoanDataMgr.SystemConfiguration.TasksSetup;
      }
      catch (Exception ex)
      {
        Tracing.Log(BusinessRuleCheck.sw, TraceLevel.Error, nameof (BusinessRuleCheck), "hasBlankRequiredTasks: can't load task setup. Error: " + ex.Message);
        return false;
      }
      MilestoneTaskLog[] milestoneTaskLogs = loanData.GetLogList().GetAllMilestoneTaskLogs((string) null);
      for (int index = 0; index < requiredTasks.Length; ++index)
      {
        if (!hashtable.ContainsKey((object) requiredTasks[index].TaskGuid))
        {
          Tracing.Log(BusinessRuleCheck.sw, TraceLevel.Error, nameof (BusinessRuleCheck), "hasBlankRequiredTasks: can't find task with GUID '" + requiredTasks[index].TaskGuid + "'.");
        }
        else
        {
          MilestoneTaskDefinition milestoneTaskDefinition = (MilestoneTaskDefinition) hashtable[(object) requiredTasks[index].TaskGuid];
          bool flag = true;
          foreach (MilestoneTaskLog milestoneTaskLog in milestoneTaskLogs)
          {
            if (string.Compare(milestoneTaskLog.TaskGUID, milestoneTaskDefinition.TaskGUID, true) == 0)
            {
              flag = false;
              break;
            }
          }
          if (flag)
            return true;
        }
      }
      return false;
    }

    public bool HasPrerequiredFields(LoanData loanData, string currentFieldID)
    {
      return this.HasPrerequiredFields(loanData, currentFieldID, false, (Hashtable) null);
    }

    public bool HasPrerequiredFields(
      LoanData loanData,
      string currentFieldID,
      bool showPopup,
      Hashtable temporaryFields)
    {
      if (Session.UserInfo.IsSuperAdministrator())
        return false;
      this.prerequiredFields = (string[]) null;
      string[] prerequiredFields = Session.LoanDataMgr.GetPrerequiredFields(currentFieldID);
      if (prerequiredFields == null || prerequiredFields.Length == 0)
        return false;
      string empty = string.Empty;
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < prerequiredFields.Length; ++index)
      {
        if (!(((temporaryFields == null || !temporaryFields.ContainsKey((object) prerequiredFields[index]) ? loanData.GetSimpleField(prerequiredFields[index]) : temporaryFields[(object) prerequiredFields[index]].ToString()) ?? "") != string.Empty))
          arrayList.Add((object) prerequiredFields[index]);
      }
      if (arrayList.Count == 0)
        return false;
      this.prerequiredFields = (string[]) arrayList.ToArray(typeof (string));
      if (showPopup)
      {
        PreRequiredDialog.Instance.InitForm(Session.LoanData, (MissingPrerequisiteException) null, currentFieldID, this.PrerequiredFields);
        if (!PreRequiredDialog.Instance.Visible)
        {
          PreRequiredDialog.Instance.Owner = Session.MainForm;
          PreRequiredDialog.Instance.Show((IWin32Window) Session.MainForm);
          PreRequiredDialog.Instance.RefreshButtonVisible = false;
        }
        if (PreRequiredDialog.Instance.WindowState == FormWindowState.Minimized)
          PreRequiredDialog.Instance.WindowState = FormWindowState.Normal;
        PreRequiredDialog.Instance.Activate();
      }
      return true;
    }

    public bool SkipReadOnlyFields(object objTemplate, LoanData loan)
    {
      ArrayList arrayList = new ArrayList();
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      switch (objTemplate)
      {
        case ClosingCost _:
          ClosingCost closingCost = (ClosingCost) objTemplate;
          if (closingCost.IgnoreBusinessRules && loan.TemporaryIgnoreRule)
            return true;
          foreach (string id in closingCost.RESPAVersion == "2015" ? ClosingCost.AllTemplateFields : ClosingCost.TemplateFields)
          {
            string simpleField = closingCost.GetSimpleField(id);
            if (simpleField != null && !(simpleField == string.Empty) && !(loan.GetSimpleField(id) == simpleField) && loan.IsFieldReadOnly(id))
              arrayList.Add((object) id);
          }
          break;
        case LoanProgram _:
          LoanProgram loanProgram = (LoanProgram) objTemplate;
          if (loanProgram.IgnoreBusinessRules && loan.TemporaryIgnoreRule)
            return true;
          foreach (string templateField in LoanProgram.TemplateFields)
          {
            if (!(templateField == string.Empty))
            {
              string simpleField = loanProgram.GetSimpleField(templateField);
              if (simpleField != null && !(simpleField == string.Empty) && loan.IsFieldReadOnly(templateField))
                arrayList.Add((object) templateField);
            }
          }
          break;
        case DataTemplate _:
          DataTemplate dataTemplate = (DataTemplate) objTemplate;
          if (dataTemplate.IgnoreBusinessRules && loan.TemporaryIgnoreRule)
            return true;
          try
          {
            foreach (string assignedFieldId in dataTemplate.GetAssignedFieldIDs())
            {
              string simpleField = dataTemplate.GetSimpleField(assignedFieldId);
              if (simpleField != null && !(simpleField == string.Empty) && loan.IsFieldReadOnly(assignedFieldId))
                arrayList.Add((object) assignedFieldId);
            }
            break;
          }
          catch (Exception ex)
          {
            Tracing.Log(BusinessRuleCheck.sw, nameof (BusinessRuleCheck), TraceLevel.Error, "Exception in loading data template. Error: " + ex.Message);
            break;
          }
      }
      if (Session.UserInfo.IsAdministrator() || arrayList.Count == 0)
        return true;
      using (PreRequiredDialog preRequiredDialog = new PreRequiredDialog())
      {
        preRequiredDialog.InitForm((LoanData) null, (MissingPrerequisiteException) null, (string) null, (string[]) arrayList.ToArray(typeof (string)));
        preRequiredDialog.ChangeToRegularFieldList();
        if (preRequiredDialog.ShowDialog((IWin32Window) null) == DialogResult.OK)
          return true;
      }
      return false;
    }

    private void requireDialog_FieldSelectedDoubleClick(object sender, EventArgs e)
    {
      Session.Application.GetService<ILoanEditor>().GoToField(PreRequiredDialog.Instance.SelectedFieldID);
    }
  }
}
