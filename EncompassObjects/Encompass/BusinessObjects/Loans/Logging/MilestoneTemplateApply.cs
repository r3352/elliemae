// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTemplateApply
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Implementation of the ILoanMilestoneTemplateOrchestrator interface used for applying a MilestoneTemplate to a loan.
  /// </summary>
  internal class MilestoneTemplateApply : ILoanMilestoneTemplateOrchestrator
  {
    private LoanDataMgr dataMgr;
    private MilestoneTemplate template;
    private bool manualMode;

    public MilestoneTemplateApply(LoanDataMgr dataMgr, MilestoneTemplate template, bool manualMode)
    {
      this.dataMgr = dataMgr;
      this.template = template;
      this.manualMode = manualMode;
    }

    /// <summary>
    /// Returns if the application of the MilestoneTemplate should be in Manual mode.
    /// </summary>
    /// <returns></returns>
    public bool IsManualApply() => this.manualMode;

    public bool NeedToCreateHistory() => !this.dataMgr.IsNew();

    /// <summary>Returns the conditions for the MilestoneTemplate</summary>
    public LoanConditions LoanConditions
    {
      get
      {
        MilestoneLog msCheck = (MilestoneLog) null;
        MilestoneLog msToBeFinished = (MilestoneLog) null;
        this.getLoanMilestoneStatus(ref msCheck, ref msToBeFinished, this.dataMgr.LoanData.GetLogList());
        return new LoanBusinessRuleInfo(this.dataMgr.LoanData).CurrentLoanForBusinessRule(msCheck, msToBeFinished);
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

    /// <summary>
    /// Returns the MilestoneTemplate that is going to be applied to the loan.
    /// </summary>
    /// <param name="milestoneTemplates">The list of all MilestoneTemplate objects</param>
    /// <param name="satisfiedTemplates">The list of MilestoneTemplate that match the conditions of the loan.</param>
    /// <param name="currentTemplateName">The current template</param>
    /// <param name="selectedTemplate">The selected MilestoneTemplate to apply</param>
    /// <returns></returns>
    public bool? SelectMilestoneTemplate(
      IEnumerable<MilestoneTemplate> milestoneTemplates,
      List<string> satisfiedTemplates,
      string currentTemplateName,
      out MilestoneTemplate selectedTemplate)
    {
      string templateName = string.Empty;
      bool policySetting = (bool) this.dataMgr.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.ShowNonMatchingMilestoneTemplate"];
      if (this.template != null && !policySetting && !satisfiedTemplates.Contains(this.template.Name))
      {
        selectedTemplate = (MilestoneTemplate) null;
        return new bool?(false);
      }
      selectedTemplate = this.template;
      if (selectedTemplate == null)
      {
        templateName = satisfiedTemplates.FirstOrDefault<string>();
        selectedTemplate = milestoneTemplates.FirstOrDefault<MilestoneTemplate>((Func<MilestoneTemplate, bool>) (mt => mt.Name == templateName));
      }
      return new bool?(true);
    }

    /// <summary>Used for email confirmation. Unused for SDK.</summary>
    /// <param name="newTemplate"></param>
    /// <param name="logList"></param>
    /// <param name="startingMilestoneToBeReplaced"></param>
    /// <param name="sendEmail"></param>
    /// <param name="dontSendEmail"></param>
    /// <param name="logRecordDifference"></param>
    /// <param name="logRecords"></param>
    /// <param name="confirmedEmailList"></param>
    /// <returns></returns>
    public bool MilestoneLogChangeConfirmation(
      MilestoneTemplate newTemplate,
      LogList logList,
      int startingMilestoneToBeReplaced,
      Dictionary<UserInfo, List<string>> sendEmail,
      Dictionary<UserInfo, List<string>> dontSendEmail,
      List<LogRecordBase> logRecordDifference,
      List<LogRecordBase> logRecords,
      out List<string> confirmedEmailList)
    {
      confirmedEmailList = new List<string>();
      return true;
    }

    /// <summary>Logic for actions after apply is complete.</summary>
    /// <param name="users"></param>
    /// <returns></returns>
    public bool OnCompletion(List<string> users) => true;

    /// <summary>Detrmines if the UI should be shown.</summary>
    /// <returns></returns>
    public bool ShowUI() => false;

    public bool MilestoneLogChangeConfirmation(
      MilestoneTemplate newTemplate,
      LogList logList,
      int startingMilestoneToBeReplaced,
      Dictionary<UserInfo, List<string>> sendEmail,
      Dictionary<UserInfo, List<string>> dontSendEmail,
      List<LogRecordBase> logRecordDifference,
      List<LogRecordBase> logRecords,
      out List<string> confirmedEmailList,
      bool manual)
    {
      confirmedEmailList = new List<string>();
      return true;
    }
  }
}
