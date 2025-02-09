// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.MilestoneManagement.StandardMilestoneTemplateApply
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.MilestoneManagement
{
  public class StandardMilestoneTemplateApply : ILoanMilestoneTemplateOrchestrator
  {
    private const string className = "StandardMilestoneTemplateApply";
    protected static string sw = Tracing.SwInputEngine;
    private bool manualMode;
    private bool createHistory = true;
    private bool showUI;
    private Sessions.Session session;

    public StandardMilestoneTemplateApply(
      Sessions.Session session,
      bool manualMode,
      bool createHistory,
      bool showUI)
    {
      this.manualMode = manualMode;
      this.createHistory = createHistory;
      this.showUI = showUI;
      this.session = session;
    }

    public LoanConditions LoanConditions
    {
      get
      {
        MilestoneLog msCheck = (MilestoneLog) null;
        MilestoneLog msToBeFinished = (MilestoneLog) null;
        this.getLoanMilestoneStatus(ref msCheck, ref msToBeFinished, this.session.LoanData.GetLogList());
        return new LoanBusinessRuleInfo(this.session.LoanData).CurrentLoanForBusinessRule(msCheck, msToBeFinished);
      }
    }

    private void getLoanMilestoneStatus(
      ref MilestoneLog msCheck,
      ref MilestoneLog msToBeFinished,
      LogList logList)
    {
      MilestoneLog[] allMilestones = logList.GetAllMilestones();
      for (int index = 0; index < allMilestones.Length && allMilestones[index].Done; ++index)
      {
        if (index == allMilestones.Length - 1)
        {
          msCheck = allMilestones[index];
          msToBeFinished = msCheck;
        }
        else if (allMilestones[index + 1].RoleID < RoleInfo.FileStarter.ID || allMilestones[index + 1].LoanAssociateID != "")
        {
          msCheck = allMilestones[index];
          msToBeFinished = allMilestones[index + 1];
        }
      }
      msToBeFinished = logList.GetCurrentMilestone();
      if (msToBeFinished != null)
        return;
      msToBeFinished = msCheck;
    }

    public bool IsManualApply() => this.manualMode;

    public bool NeedToCreateHistory() => this.createHistory;

    public bool ShowUI() => this.showUI;

    public bool? SelectMilestoneTemplate(
      IEnumerable<MilestoneTemplate> milestoneTemplates,
      List<string> satisfiedTemplates,
      string currentTemplateName,
      out MilestoneTemplate selectedTemplate)
    {
      selectedTemplate = (MilestoneTemplate) null;
      if (!this.manualMode && satisfiedTemplates.Count > 0 && satisfiedTemplates.FirstOrDefault<string>() == currentTemplateName)
        return new bool?();
      IEnumerable<EllieMae.EMLite.Workflow.Milestone> milestones = this.session.SessionObjects.BpmManager.GetMilestones(false);
      RoleInfo[] allRoleFunctions = this.session.SessionObjects.BpmManager.GetAllRoleFunctions();
      bool hideMilestoneTemplate = !(bool) this.session.SessionObjects.ServerManager.GetServerSetting("Policies.ShowNonMatchingMilestoneTemplate");
      MilestoneTemplateSelector templateSelector = new MilestoneTemplateSelector(milestoneTemplates, satisfiedTemplates, allRoleFunctions, milestones, hideMilestoneTemplate, currentTemplateName, this.manualMode);
      if (templateSelector.ShowDialog() != DialogResult.OK)
        return new bool?(false);
      selectedTemplate = templateSelector.SelectedTemplate;
      return new bool?(true);
    }

    public bool MilestoneLogChangeConfirmation(
      MilestoneTemplate newTemplate,
      LogList logList,
      int startingMilestoneToBeReplaced,
      Dictionary<UserInfo, List<string>> sendEmail,
      Dictionary<UserInfo, List<string>> dontSendEmail,
      List<LogRecordBase> logRecordDifference,
      List<LogRecordBase> completeLogRecords,
      out List<string> confirmedEmailList,
      bool manual)
    {
      confirmedEmailList = new List<string>();
      RoleInfo[] allRoleFunctions = this.session.SessionObjects.BpmManager.GetAllRoleFunctions();
      IEnumerable<EllieMae.EMLite.Workflow.Milestone> milestones = this.session.SessionObjects.BpmManager.GetMilestones(false);
      if (this.ShowUI() && new MilestoneLogDiff(newTemplate, logList.GetAllMilestones(), startingMilestoneToBeReplaced, allRoleFunctions, milestones, sendEmail, dontSendEmail, completeLogRecords, this.session.SessionObjects, logList.MilestoneTemplate.Name, manual).ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
        return false;
      bool emailSend = string.IsNullOrWhiteSpace(this.session.ConfigurationManager.GetCompanySetting("POLICIES", "MilestoneTemplateChangeNotification")) ? (bool) this.session.SessionObjects.ServerManager.GetServerSetting("Policies.MilestoneTemplateChangeNotification") : bool.Parse(this.session.ConfigurationManager.GetCompanySetting("POLICIES", "MilestoneTemplateChangeNotification"));
      if (this.ShowUI())
      {
        if (sendEmail.Count == 0 && dontSendEmail.Count == 0 && logRecordDifference.Count == 0)
          return true;
        LogChangeConfirmation changeConfirmation = new LogChangeConfirmation(sendEmail, dontSendEmail, logRecordDifference, emailSend);
        if (changeConfirmation.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return false;
        if (emailSend)
        {
          foreach (UserInfo key in changeConfirmation.EmailList.Keys)
            confirmedEmailList.Add(key.Userid);
        }
      }
      else if (emailSend)
      {
        foreach (UserInfo key in sendEmail.Keys)
          confirmedEmailList.Add(key.Userid);
      }
      return true;
    }

    public bool OnCompletion(List<string> users)
    {
      try
      {
        if (users.Count > 0)
        {
          Borrower borrower = this.session.LoanData.GetBorrowerPairs()[0].Borrower;
          string str1 = "Loan access alert for ";
          if (borrower.LastName.Trim() != "")
            str1 = str1 + borrower.LastName + " ";
          string subject = str1 + "loan file";
          string str2 = "Dear <user>,\n\nA loan team member has applied a new milestone list to the loan ";
          if (borrower.ToString().Trim() != "")
            str2 = str2 + borrower.LastName + ", " + borrower.FirstName + " ";
          string str3 = str2 + "file. As a result, you are no longer assigned to any milestones for this loan which may result in you losing access to this loan file. You may have also lost access to this loan if you are not associated with the new milestone list.\n\nThe new milestone list being applied to this loan is: \n";
          foreach (MilestoneLog allMilestone in this.session.LoanData.GetLogList().GetAllMilestones())
            str3 = str3 + allMilestone.Stage + "\n";
          string body = str3 + "\nSincerely, \nThe Encompass Team";
          TriggerEmailTemplate triggerEmailTemplate = new TriggerEmailTemplate(subject, body, users.ToArray(), new int[0], true);
          ILoanEditor service = Session.Application.GetService<ILoanEditor>();
          service.MilestoneTemplateEmailTemplate = triggerEmailTemplate;
          service.ClearMilestoneLogArea();
        }
        if (this.IsManualApply())
          this.session.Application.GetService<ILoanConsole>().SaveLoan();
        Session.Application.GetService<ILoanEditor>().ClearMilestoneLogArea();
      }
      catch (MissingPrerequisiteException ex)
      {
        Tracing.Log(StandardMilestoneTemplateApply.sw, nameof (StandardMilestoneTemplateApply), TraceLevel.Error, ex.ToString());
        return false;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The loan cannot be saved due to: " + ex.Message);
        return false;
      }
      return true;
    }
  }
}
