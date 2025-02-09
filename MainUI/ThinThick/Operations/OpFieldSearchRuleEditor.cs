// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ThinThick.Operations.OpFieldSearchRuleEditor
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.ThinThick;
using EllieMae.EMLite.Common.ThinThick.Operation;
using EllieMae.EMLite.Common.ThinThick.Operation.Interfaces;
using EllieMae.EMLite.Common.ThinThick.Requests.Interaction;
using EllieMae.EMLite.Common.ThinThick.Responses;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.Setup.MSWorksheet;
using EllieMae.EMLite.Setup.StatusOnline;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ThinThick.Operations
{
  public class OpFieldSearchRuleEditor : 
    OperationBase,
    IOpFieldSearchRuleEditor,
    IOperation,
    IDisposable
  {
    public OpDialogResponse OpenEditor(OpFieldSearchRuleOpenEditorRequest request)
    {
      OpDialogResponse opDialogResponse = new OpDialogResponse();
      FieldSearchRuleType ruleType = request.CommandContext.Session.SessionObjects.BpmManager.GetRuleType(request.FsRuleId);
      switch (ruleType)
      {
        case FieldSearchRuleType.None:
          opDialogResponse.ErrorMessage = "Rule not found";
          opDialogResponse.ErrorCode = ErrorCodes.RuleNotFound;
          int num = (int) Utils.Dialog((IWin32Window) null, "Please select a valid rule and try again", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        case FieldSearchRuleType.MilestoneRules:
          opDialogResponse = this.ShowConditionRuleDialog(request);
          break;
        case FieldSearchRuleType.LoanAccess:
          opDialogResponse = this.ShowLoanAccessRuleDialog(request);
          break;
        case FieldSearchRuleType.FieldAccess:
          opDialogResponse = this.ShowFieldAccessRuleDialog(request);
          break;
        case FieldSearchRuleType.FieldRules:
          opDialogResponse = this.ShowFieldRuleDialog(request);
          break;
        case FieldSearchRuleType.InputForms:
          opDialogResponse = this.ShowInputFormRuleDialog(request);
          break;
        case FieldSearchRuleType.Triggers:
          opDialogResponse = this.ShowTriggerEditor(request);
          break;
        case FieldSearchRuleType.PrintForms:
          opDialogResponse = this.ShowPrintFormRuleDialog(request);
          break;
        case FieldSearchRuleType.PrintSelection:
          opDialogResponse = this.ShowPrintSelectionRuleDialog(request);
          break;
        case FieldSearchRuleType.AutomatedConditions:
          opDialogResponse = this.ShowAutomatedConditionEditor(request);
          break;
        case FieldSearchRuleType.PiggyBackingFields:
        case FieldSearchRuleType.BorrowerCustomFields:
        case FieldSearchRuleType.BusinessCustomFields:
        case FieldSearchRuleType.LockRequestAdditionalFields:
        case FieldSearchRuleType.TPOCustomFields:
          request.CommandContext.Session.Setup.ShowPage(FieldSearchUtil.Type2Name(ruleType));
          break;
        case FieldSearchRuleType.LoanCustomFields:
          string ruleIdentifier = request.CommandContext.Session.SessionObjects.BpmManager.GetRuleIdentifier(request.FsRuleId);
          request.CommandContext.Session.Setup.SetLoanCustomFieldId(ruleIdentifier);
          request.CommandContext.Session.Setup.EditSelectedLoanCustomField();
          break;
        case FieldSearchRuleType.Alerts:
          opDialogResponse = this.ShowAlertEditor(request);
          break;
        case FieldSearchRuleType.HtmlEmailTemplate:
          opDialogResponse = this.ShowEmailTemplateDialog(request);
          break;
        case FieldSearchRuleType.CompanyStatusOnline:
          opDialogResponse = this.ShowStatusOnlineTriggerDialog(request);
          break;
        case FieldSearchRuleType.LoanActionAccess:
          opDialogResponse = this.ShowLoanActionAccessDialog(request);
          break;
        case FieldSearchRuleType.LoanActionCompletionRules:
          opDialogResponse = this.ShowLoanActionCompletionDialog(request);
          break;
        case FieldSearchRuleType.AutomatedEnhancedConditions:
          opDialogResponse = this.ShowAutomatedConditionEditor(request);
          break;
      }
      return opDialogResponse;
    }

    private OpDialogResponse ShowConditionRuleDialog(OpFieldSearchRuleOpenEditorRequest request)
    {
      OpDialogResponse opDialogResponse = new OpDialogResponse();
      MilestoneRulesBpmManager bpmManager = (MilestoneRulesBpmManager) request.CommandContext.Session.BPM.GetBpmManager(BpmCategory.MilestoneRules);
      int ruleId = request.CommandContext.Session.SessionObjects.BpmManager.GetRuleId(request.FsRuleId);
      BizRuleInfo rule = bpmManager.GetRule(ruleId);
      if (rule == null)
      {
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "This Milestone Requirement Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return opDialogResponse;
      }
      MilestoneRuleInfo milestoneRule1 = (MilestoneRuleInfo) rule.Clone();
      ConditionRulePanel container = new ConditionRulePanel(request.CommandContext.Session, false);
      using (ConditionRuleDialog conditionRuleDialog = new ConditionRuleDialog(request.CommandContext.Session, milestoneRule1, container))
      {
        if (conditionRuleDialog.ShowDialog(request.CommandContext.SourceWindow) == DialogResult.OK)
        {
          opDialogResponse.DialogResult = conditionRuleDialog.DialogResult.ToString();
          try
          {
            MilestoneRuleInfo milestoneRule2 = conditionRuleDialog.MilestoneRule;
            bpmManager.UpdateRule((BizRuleInfo) milestoneRule2);
            if (BizRule.RuleStatus.Active == rule.Status)
            {
              int num = (int) bpmManager.ActivateRule(milestoneRule2.RuleID);
            }
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "The Milestone Rule can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        conditionRuleDialog.Dispose();
      }
      return opDialogResponse;
    }

    private OpDialogResponse ShowFieldRuleDialog(OpFieldSearchRuleOpenEditorRequest request)
    {
      OpDialogResponse opDialogResponse = new OpDialogResponse();
      FieldRulesBpmManager bpmManager = (FieldRulesBpmManager) request.CommandContext.Session.BPM.GetBpmManager(BpmCategory.FieldRules);
      int ruleId = request.CommandContext.Session.SessionObjects.BpmManager.GetRuleId(request.FsRuleId);
      BizRuleInfo rule = bpmManager.GetRule(ruleId);
      if (rule == null)
      {
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "This Field Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return opDialogResponse;
      }
      FieldRuleInfo fieldRule1 = (FieldRuleInfo) rule.Clone();
      using (FieldRuleDialog fieldRuleDialog = new FieldRuleDialog(request.CommandContext.Session, fieldRule1))
      {
        if (fieldRuleDialog.ShowDialog(request.CommandContext.SourceWindow) == DialogResult.OK)
        {
          opDialogResponse.DialogResult = fieldRuleDialog.DialogResult.ToString();
          try
          {
            FieldRuleInfo fieldRule2 = fieldRuleDialog.FieldRule;
            bpmManager.UpdateRule((BizRuleInfo) fieldRule2);
            if (BizRule.RuleStatus.Active == rule.Status)
            {
              int num = (int) bpmManager.ActivateRule(fieldRule2.RuleID);
            }
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "The Field Rule can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        fieldRuleDialog.Dispose();
      }
      return opDialogResponse;
    }

    private OpDialogResponse ShowTriggerEditor(OpFieldSearchRuleOpenEditorRequest request)
    {
      OpDialogResponse opDialogResponse = new OpDialogResponse();
      TriggersBpmManager bpmManager = (TriggersBpmManager) request.CommandContext.Session.BPM.GetBpmManager(BpmCategory.Triggers);
      int ruleId = request.CommandContext.Session.SessionObjects.BpmManager.GetRuleId(request.FsRuleId);
      BizRuleInfo rule = bpmManager.GetRule(ruleId);
      if (rule == null)
      {
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "This Trigger Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return opDialogResponse;
      }
      using (TriggerEditor triggerEditor = new TriggerEditor((TriggerInfo) rule.Clone(), request.CommandContext.Session))
      {
        if (triggerEditor.ShowDialog(request.CommandContext.SourceWindow) == DialogResult.OK)
        {
          opDialogResponse.DialogResult = triggerEditor.DialogResult.ToString();
          try
          {
            TriggerInfo trigger = triggerEditor.Trigger;
            bpmManager.UpdateRule((BizRuleInfo) trigger);
            if (BizRule.RuleStatus.Active == rule.Status)
            {
              int num = (int) bpmManager.ActivateRule(trigger.RuleID);
            }
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "The Trigger Rule can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        triggerEditor.Dispose();
      }
      return opDialogResponse;
    }

    private OpDialogResponse ShowAutomatedConditionEditor(OpFieldSearchRuleOpenEditorRequest request)
    {
      OpDialogResponse opDialogResponse = new OpDialogResponse();
      AutomatedConditionBpmManager bpmManager = (AutomatedConditionBpmManager) request.CommandContext.Session.BPM.GetBpmManager(BpmCategory.AutomatedConditions);
      int ruleId = request.CommandContext.Session.SessionObjects.BpmManager.GetRuleId(request.FsRuleId);
      BizRuleInfo rule = bpmManager.GetRule(ruleId);
      if (rule == null)
      {
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "This Automated Conditions Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return opDialogResponse;
      }
      using (AutomatedConditionEditor automatedConditionEditor = new AutomatedConditionEditor((AutomatedConditionRuleInfo) rule.Clone(), request.CommandContext.Session))
      {
        if (automatedConditionEditor.ShowDialog(request.CommandContext.SourceWindow) == DialogResult.OK)
        {
          opDialogResponse.DialogResult = automatedConditionEditor.DialogResult.ToString();
          try
          {
            AutomatedConditionRuleInfo automatedCondition = automatedConditionEditor.AutomatedCondition;
            bpmManager.UpdateRule((BizRuleInfo) automatedCondition);
            if (BizRule.RuleStatus.Active == rule.Status)
            {
              int num = (int) bpmManager.ActivateRule(automatedCondition.RuleID);
            }
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "The Automated Condition Rule can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        automatedConditionEditor.Dispose();
      }
      return opDialogResponse;
    }

    private OpDialogResponse ShowAutomatedEnhancedConditionEditor(
      OpFieldSearchRuleOpenEditorRequest request)
    {
      OpDialogResponse opDialogResponse = new OpDialogResponse();
      AutomatedEnhancedConditionBpmManager bpmManager = (AutomatedEnhancedConditionBpmManager) request.CommandContext.Session.BPM.GetBpmManager(BpmCategory.AutomatedEnhancedConditions);
      int ruleId = request.CommandContext.Session.SessionObjects.BpmManager.GetRuleId(request.FsRuleId);
      BizRuleInfo rule = bpmManager.GetRule(ruleId);
      if (rule == null)
      {
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "This Automated Enhanced Conditions Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return opDialogResponse;
      }
      using (AutomatedEnhancedConditionEditor enhancedConditionEditor = new AutomatedEnhancedConditionEditor((AutomatedEnhancedConditionRuleInfo) rule.Clone(), request.CommandContext.Session))
      {
        if (enhancedConditionEditor.ShowDialog(request.CommandContext.SourceWindow) == DialogResult.OK)
        {
          opDialogResponse.DialogResult = enhancedConditionEditor.DialogResult.ToString();
          try
          {
            AutomatedEnhancedConditionRuleInfo enhancedCondition = enhancedConditionEditor.AutomatedEnhancedCondition;
            bpmManager.UpdateRule((BizRuleInfo) enhancedCondition);
            if (BizRule.RuleStatus.Active == rule.Status)
            {
              int num = (int) bpmManager.ActivateRule(enhancedCondition.RuleID);
            }
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "The Automated Enhanced Condition Rule can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        enhancedConditionEditor.Dispose();
      }
      return opDialogResponse;
    }

    private OpDialogResponse ShowFieldAccessRuleDialog(OpFieldSearchRuleOpenEditorRequest request)
    {
      OpDialogResponse opDialogResponse = new OpDialogResponse();
      FieldAccessBpmManager bpmManager = (FieldAccessBpmManager) request.CommandContext.Session.BPM.GetBpmManager(BpmCategory.FieldAccess);
      int ruleId = request.CommandContext.Session.SessionObjects.BpmManager.GetRuleId(request.FsRuleId);
      BizRuleInfo rule = bpmManager.GetRule(ruleId);
      if (rule == null)
      {
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "This Field Access Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return opDialogResponse;
      }
      FieldAccessRuleInfo ruleInfo1 = (FieldAccessRuleInfo) rule.Clone();
      using (FieldAccessRuleDialog accessRuleDialog = new FieldAccessRuleDialog(request.CommandContext.Session, ruleInfo1))
      {
        if (accessRuleDialog.ShowDialog(request.CommandContext.SourceWindow) == DialogResult.OK)
        {
          opDialogResponse.DialogResult = accessRuleDialog.DialogResult.ToString();
          try
          {
            FieldAccessRuleInfo ruleInfo2 = accessRuleDialog.RuleInfo;
            bpmManager.UpdateRule((BizRuleInfo) ruleInfo2);
            if (BizRule.RuleStatus.Active == rule.Status)
            {
              int num = (int) bpmManager.ActivateRule(ruleInfo2.RuleID);
            }
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "The Field Access Rule can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        accessRuleDialog.Dispose();
      }
      return opDialogResponse;
    }

    private OpDialogResponse ShowLoanAccessRuleDialog(OpFieldSearchRuleOpenEditorRequest request)
    {
      OpDialogResponse opDialogResponse = new OpDialogResponse();
      LoanAccessBpmManager bpmManager = (LoanAccessBpmManager) request.CommandContext.Session.BPM.GetBpmManager(BpmCategory.LoanAccess);
      int ruleId = request.CommandContext.Session.SessionObjects.BpmManager.GetRuleId(request.FsRuleId);
      BizRuleInfo rule = bpmManager.GetRule(ruleId);
      if (rule == null)
      {
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "This Loan Access Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return opDialogResponse;
      }
      LoanAccessRuleInfo ruleInfo1 = (LoanAccessRuleInfo) rule.Clone();
      using (LoanAccessRuleDialog accessRuleDialog = new LoanAccessRuleDialog(request.CommandContext.Session, ruleInfo1))
      {
        if (accessRuleDialog.ShowDialog(request.CommandContext.SourceWindow) == DialogResult.OK)
        {
          opDialogResponse.DialogResult = accessRuleDialog.DialogResult.ToString();
          try
          {
            LoanAccessRuleInfo ruleInfo2 = accessRuleDialog.RuleInfo;
            bpmManager.UpdateRule((BizRuleInfo) ruleInfo2);
            if (BizRule.RuleStatus.Active == rule.Status)
            {
              int num = (int) bpmManager.ActivateRule(ruleInfo2.RuleID);
            }
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "The Loan Access Rule can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        accessRuleDialog.Dispose();
      }
      return opDialogResponse;
    }

    private OpDialogResponse ShowInputFormRuleDialog(OpFieldSearchRuleOpenEditorRequest request)
    {
      OpDialogResponse opDialogResponse = new OpDialogResponse();
      InputFormsBpmManager bpmManager = (InputFormsBpmManager) request.CommandContext.Session.BPM.GetBpmManager(BpmCategory.InputForms);
      int ruleId = request.CommandContext.Session.SessionObjects.BpmManager.GetRuleId(request.FsRuleId);
      BizRuleInfo rule = bpmManager.GetRule(ruleId);
      if (rule == null)
      {
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "This Input Form Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return opDialogResponse;
      }
      InputFormRuleInfo inputFormRuleInfo = (InputFormRuleInfo) rule.Clone();
      InputFormInfo[] forms = bpmManager.GetForms(inputFormRuleInfo);
      using (InputFormRuleDialog inputFormRuleDialog = new InputFormRuleDialog(request.CommandContext.Session, inputFormRuleInfo, forms))
      {
        if (inputFormRuleDialog.ShowDialog(request.CommandContext.SourceWindow) == DialogResult.OK)
        {
          opDialogResponse.DialogResult = inputFormRuleDialog.DialogResult.ToString();
          try
          {
            InputFormRuleInfo inputFormRule = inputFormRuleDialog.InputFormRule;
            bpmManager.UpdateRule((BizRuleInfo) inputFormRule);
            if (BizRule.RuleStatus.Active == rule.Status)
            {
              int num = (int) bpmManager.ActivateRule(inputFormRule.RuleID);
            }
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "The Input Form Rule can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        inputFormRuleDialog.Dispose();
      }
      return opDialogResponse;
    }

    private OpDialogResponse ShowPrintFormRuleDialog(OpFieldSearchRuleOpenEditorRequest request)
    {
      OpDialogResponse opDialogResponse = new OpDialogResponse();
      PrintFormsBpmManager bpmManager = (PrintFormsBpmManager) request.CommandContext.Session.BPM.GetBpmManager(BpmCategory.PrintForms);
      int ruleId = request.CommandContext.Session.SessionObjects.BpmManager.GetRuleId(request.FsRuleId);
      BizRuleInfo rule = bpmManager.GetRule(ruleId);
      if (rule == null)
      {
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "This Print Form Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return opDialogResponse;
      }
      PrintFormRuleInfo printRuleInfo1 = (PrintFormRuleInfo) rule.Clone();
      using (PrintFormRuleDialog printFormRuleDialog = new PrintFormRuleDialog(request.CommandContext.Session, printRuleInfo1))
      {
        if (printFormRuleDialog.ShowDialog(request.CommandContext.SourceWindow) == DialogResult.OK)
        {
          opDialogResponse.DialogResult = printFormRuleDialog.DialogResult.ToString();
          try
          {
            PrintFormRuleInfo printRuleInfo2 = printFormRuleDialog.PrintRuleInfo;
            bpmManager.UpdateRule((BizRuleInfo) printRuleInfo2);
            if (BizRule.RuleStatus.Active == rule.Status)
            {
              int num = (int) bpmManager.ActivateRule(printRuleInfo2.RuleID);
            }
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "The Print Form Rule can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        printFormRuleDialog.Dispose();
      }
      return opDialogResponse;
    }

    private OpDialogResponse ShowPrintSelectionRuleDialog(OpFieldSearchRuleOpenEditorRequest request)
    {
      OpDialogResponse opDialogResponse = new OpDialogResponse();
      PrintSelectionBpmManager bpmManager = (PrintSelectionBpmManager) request.CommandContext.Session.BPM.GetBpmManager(BpmCategory.PrintSelection);
      int ruleId = request.CommandContext.Session.SessionObjects.BpmManager.GetRuleId(request.FsRuleId);
      BizRuleInfo rule = bpmManager.GetRule(ruleId);
      if (rule == null)
      {
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "This Print Selection Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return opDialogResponse;
      }
      PrintSelectionRuleInfo printSelectionRule1 = (PrintSelectionRuleInfo) rule.Clone();
      PrintSelectionRuleListPanel selectionRuleListPanel = new PrintSelectionRuleListPanel(request.CommandContext.Session, false);
      using (PrintSelectionRuleDialog selectionRuleDialog = new PrintSelectionRuleDialog(request.CommandContext.Session, printSelectionRule1, request.CommandContext.Session.LoanManager.GetFieldSettings()))
      {
        if (selectionRuleDialog.ShowDialog(request.CommandContext.SourceWindow) == DialogResult.OK)
        {
          opDialogResponse.DialogResult = selectionRuleDialog.DialogResult.ToString();
          try
          {
            PrintSelectionRuleInfo printSelectionRule2 = selectionRuleDialog.PrintSelectionRule;
            bpmManager.UpdateRule((BizRuleInfo) printSelectionRule2);
            if (BizRule.RuleStatus.Active == rule.Status)
            {
              int num = (int) bpmManager.ActivateRule(printSelectionRule2.RuleID);
            }
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "The Print Selection Rule can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        selectionRuleDialog.Dispose();
      }
      return opDialogResponse;
    }

    private OpDialogResponse ShowStatusOnlineTriggerDialog(
      OpFieldSearchRuleOpenEditorRequest request)
    {
      OpDialogResponse opDialogResponse = new OpDialogResponse();
      string ruleIdentifier = request.CommandContext.Session.SessionObjects.BpmManager.GetRuleIdentifier(request.FsRuleId);
      StatusOnlineSetup statusOnlineSetup = request.CommandContext.Session.SessionObjects.ConfigurationManager.GetStatusOnlineSetup((string) null);
      StatusOnlineTrigger byId = statusOnlineSetup.Triggers.GetByID(ruleIdentifier);
      using (StatusOnlineTriggerDialog onlineTriggerDialog = new StatusOnlineTriggerDialog(request.CommandContext.Session, statusOnlineSetup, byId))
      {
        int num = (int) onlineTriggerDialog.ShowDialog(request.CommandContext.SourceWindow);
        opDialogResponse.DialogResult = onlineTriggerDialog.DialogResult.ToString();
        onlineTriggerDialog.Dispose();
      }
      return opDialogResponse;
    }

    private OpDialogResponse ShowEmailTemplateDialog(OpFieldSearchRuleOpenEditorRequest request)
    {
      OpDialogResponse opDialogResponse = new OpDialogResponse();
      string ruleIdentifier = request.CommandContext.Session.SessionObjects.BpmManager.GetRuleIdentifier(request.FsRuleId);
      HtmlEmailTemplate htmlEmailTemplate = request.CommandContext.Session.SessionObjects.ConfigurationManager.GetHtmlEmailTemplate((string) null, ruleIdentifier);
      using (EmailTemplateDialog emailTemplateDialog = new EmailTemplateDialog(request.CommandContext.Session, htmlEmailTemplate, false))
      {
        int num = (int) emailTemplateDialog.ShowDialog(request.CommandContext.SourceWindow);
        opDialogResponse.DialogResult = emailTemplateDialog.DialogResult.ToString();
        emailTemplateDialog.Dispose();
      }
      return opDialogResponse;
    }

    private OpDialogResponse ShowAlertEditor(OpFieldSearchRuleOpenEditorRequest request)
    {
      OpDialogResponse opDialogResponse = new OpDialogResponse();
      using (AlertEditor alertEditor = new AlertEditor(request.CommandContext.Session.LoanManager.GetAlertSetupData().GetAlertConfig(request.CommandContext.Session.SessionObjects.BpmManager.GetRuleId(request.FsRuleId)), request.CommandContext.Session.LoanManager.GetFieldSettings(), request.CommandContext.Session))
      {
        int num = (int) alertEditor.ShowDialog(request.CommandContext.SourceWindow);
        opDialogResponse.DialogResult = alertEditor.DialogResult.ToString();
        alertEditor.Dispose();
      }
      return opDialogResponse;
    }

    private OpDialogResponse ShowLoanActionAccessDialog(OpFieldSearchRuleOpenEditorRequest request)
    {
      OpDialogResponse opDialogResponse = new OpDialogResponse();
      LoanActionAccessBpmManager bpmManager = (LoanActionAccessBpmManager) request.CommandContext.Session.BPM.GetBpmManager(BpmCategory.LoanActionAccess);
      int ruleId = request.CommandContext.Session.SessionObjects.BpmManager.GetRuleId(request.FsRuleId);
      BizRuleInfo rule = bpmManager.GetRule(ruleId);
      if (rule == null)
      {
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "This Persona Access To Loan Action Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return opDialogResponse;
      }
      LoanActionAccessRuleInfo ruleInfo1 = (LoanActionAccessRuleInfo) rule.Clone();
      using (LoanActionAccessRuleDialog accessRuleDialog = new LoanActionAccessRuleDialog(request.CommandContext.Session, ruleInfo1))
      {
        if (accessRuleDialog.ShowDialog(request.CommandContext.SourceWindow) == DialogResult.OK)
        {
          opDialogResponse.DialogResult = accessRuleDialog.DialogResult.ToString();
          try
          {
            LoanActionAccessRuleInfo ruleInfo2 = accessRuleDialog.RuleInfo;
            bpmManager.UpdateRule((BizRuleInfo) ruleInfo2);
            if (BizRule.RuleStatus.Active == rule.Status)
            {
              int num = (int) bpmManager.ActivateRule(ruleInfo2.RuleID);
            }
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "The Persona Access To Loan Action Rule can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        accessRuleDialog.Dispose();
      }
      return opDialogResponse;
    }

    private OpDialogResponse ShowLoanActionCompletionDialog(
      OpFieldSearchRuleOpenEditorRequest request)
    {
      OpDialogResponse opDialogResponse = new OpDialogResponse();
      LoanActionCompletionRulesBpmManager bpmManager = (LoanActionCompletionRulesBpmManager) request.CommandContext.Session.BPM.GetBpmManager(BpmCategory.LoanActionCompletionRules);
      int ruleId = request.CommandContext.Session.SessionObjects.BpmManager.GetRuleId(request.FsRuleId);
      BizRuleInfo rule = bpmManager.GetRule(ruleId);
      if (rule == null)
      {
        int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "This Loan Action Completion Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return opDialogResponse;
      }
      LoanActionCompletionRuleInfo loanActionCompletionRule = (LoanActionCompletionRuleInfo) rule.Clone();
      using (LoanActionCompletionRuleDialog completionRuleDialog = new LoanActionCompletionRuleDialog(request.CommandContext.Session, loanActionCompletionRule, new LoanActionCompletionRulePanel(request.CommandContext.Session, false)))
      {
        if (completionRuleDialog.ShowDialog(request.CommandContext.SourceWindow) == DialogResult.OK)
        {
          opDialogResponse.DialogResult = completionRuleDialog.DialogResult.ToString();
          try
          {
            LoanActionCompletionRuleInfo actionCompletionRule = completionRuleDialog.LoanActionCompletionRule;
            bpmManager.UpdateRule((BizRuleInfo) actionCompletionRule);
            if (BizRule.RuleStatus.Active == rule.Status)
            {
              int num = (int) bpmManager.ActivateRule(actionCompletionRule.RuleID);
            }
          }
          catch (Exception ex)
          {
            MetricsFactory.IncrementErrorCounter(ex, "Rule MAnager Exceptions", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\ThinThick\\Operations\\OpFieldSearchRuleEditor.cs", nameof (ShowLoanActionCompletionDialog), 695);
            int num = (int) Utils.Dialog(request.CommandContext.SourceWindow, "The Loan Action Completion Rule can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        completionRuleDialog.Dispose();
      }
      return opDialogResponse;
    }
  }
}
