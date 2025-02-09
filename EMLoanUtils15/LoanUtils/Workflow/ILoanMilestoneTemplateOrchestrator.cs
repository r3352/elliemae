// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Workflow.ILoanMilestoneTemplateOrchestrator
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Workflow;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Workflow
{
  public interface ILoanMilestoneTemplateOrchestrator
  {
    bool IsManualApply();

    bool NeedToCreateHistory();

    bool ShowUI();

    LoanConditions LoanConditions { get; }

    bool? SelectMilestoneTemplate(
      IEnumerable<MilestoneTemplate> milestoneTemplates,
      List<string> satisfiedTemplates,
      string currentTemplateName,
      out MilestoneTemplate selectedTemplate);

    bool MilestoneLogChangeConfirmation(
      MilestoneTemplate newTemplate,
      LogList logList,
      int startingMilestoneToBeReplaced,
      Dictionary<UserInfo, List<string>> sendEmail,
      Dictionary<UserInfo, List<string>> dontSendEmail,
      List<LogRecordBase> logRecordDifference,
      List<LogRecordBase> logRecords,
      out List<string> confirmedEmailList,
      bool manual);

    bool OnCompletion(List<string> users);
  }
}
