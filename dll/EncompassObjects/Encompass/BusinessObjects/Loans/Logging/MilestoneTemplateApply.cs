// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTemplateApply
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

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

    public bool IsManualApply() => this.manualMode;

    public bool NeedToCreateHistory() => !this.dataMgr.IsNew();

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
        else if (((LoanAssociateLog) allMilestones[index + 1]).RoleID < ((RoleSummaryInfo) RoleInfo.FileStarter).ID || ((LoanAssociateLog) allMilestones[index + 1]).LoanAssociateID != "")
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

    public bool OnCompletion(List<string> users) => true;

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
