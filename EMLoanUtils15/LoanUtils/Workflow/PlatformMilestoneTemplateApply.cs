// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Workflow.PlatformMilestoneTemplateApply
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Workflow
{
  public class PlatformMilestoneTemplateApply : ILoanMilestoneTemplateOrchestrator
  {
    private LoanData loanData;
    private MilestoneTemplate template;
    private bool manualMode;
    private SessionObjects sessionObjects;
    private bool createHistory = true;

    public PlatformMilestoneTemplateApply(
      SessionObjects sessionObjects,
      LoanData dataMgr,
      MilestoneTemplate template,
      bool manualMode)
    {
      this.sessionObjects = sessionObjects;
      this.loanData = dataMgr;
      this.template = template;
      this.manualMode = manualMode;
    }

    public bool IsManualApply() => this.manualMode;

    public bool CreateHistory
    {
      get => this.createHistory;
      set => this.createHistory = value;
    }

    public bool NeedToCreateHistory() => this.createHistory;

    public LoanConditions LoanConditions
    {
      get
      {
        MilestoneLog msCheck = (MilestoneLog) null;
        MilestoneLog msToBeFinished = (MilestoneLog) null;
        this.getLoanMilestoneStatus(ref msCheck, ref msToBeFinished, this.loanData.GetLogList());
        return new LoanBusinessRuleInfo(this.loanData).CurrentLoanForBusinessRule(msCheck, msToBeFinished);
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

    public bool? SelectMilestoneTemplate(
      IEnumerable<MilestoneTemplate> milestoneTemplates,
      List<string> satisfiedTemplates,
      string currentTemplateName,
      out MilestoneTemplate selectedTemplate)
    {
      string templateName = string.Empty;
      bool policySetting = (bool) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.ShowNonMatchingMilestoneTemplate"];
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
