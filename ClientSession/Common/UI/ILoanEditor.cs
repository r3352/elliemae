// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ILoanEditor
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocEngine;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public interface ILoanEditor
  {
    void ShowMilestoneWorksheet(MilestoneLog ms);

    void RefreshLoan();

    void ShowVerifPanel(string verifType);

    void RefreshContents();

    void RefreshLoanContents();

    void RefreshContents(string fieldId);

    void RefreshLogPanel();

    void StartConversation(ConversationLog con);

    void RefreshLoanTeamMembers();

    void AddMilestoneWorksheet(MilestoneLog milestoneLog);

    void ClearMilestoneLogArea();

    void ApplyBusinessRules();

    void ApplyOnDemandBusinessRules();

    void SetMilestoneStatus(MilestoneLog milestoneLog, int milestoneIndex, bool finished);

    void AddToWorkArea(Control worksheet);

    void AddToWorkArea(Control worksheet, bool rememberCurrentFormID);

    void RemoveFromWorkArea();

    DateTime AddDays(DateTime date, int dayCount);

    int MinusBusinessDays(DateTime previous, DateTime currentLog);

    string CurrentForm { get; set; }

    object GetFormScreen();

    object GetVerifScreen();

    void GoToField(string fieldID);

    void GoToField(string fieldID, bool findNext);

    void GoToField(string fieldID, bool findNext, bool searchToolPageOnly);

    void GoToField(string fieldID, string formName);

    void GoToField(string fieldID, BorrowerPair targetPair);

    void BAMGoToField(string fieldID, bool findNext);

    bool OpenForm(string formOrToolName);

    bool OpenForm(string formOrToolName, Control navControl);

    bool OpenFormByID(string formOrToolID);

    bool OpenFormByID(string formOrToolID, Control navControl);

    bool OpenLogRecord(LogRecordBase rec);

    void OpenMilestoneLogReview(MilestoneLog log);

    void OpenMilestoneLogReview(MilestoneLog log, MilestoneHistoryLog historyLog);

    void PromptCreateNewLogRecord();

    bool IsPrimaryEditor { get; }

    bool GetInputEngineService(LoanData loan, InputEngineServiceType serviceType);

    bool Print(string[] formNames);

    bool ShowRegulationAlerts();

    bool ShowRegulationAlertsOrderDoc();

    void ShoweDisclosureTrackingRecord(string packageID);

    void ShoweDisclosureTrackingRecord(DisclosureTrackingBase selectedLog, bool clearNotification);

    void SaveLoan();

    bool SelectSettlementServiceProviders();

    bool SelectAffilatesTemplate();

    void ShowPlanCodeComparison(string fieldId, DocumentOrderType orderType);

    TriggerEmailTemplate MilestoneTemplateEmailTemplate { get; set; }

    void ShowAUSTrackingTool();

    string[] SelectLinkAndSyncTemplate();

    void ShowAIQAnalyzerMessage(
      string analyzerType,
      DateTime alertDateTime,
      string description,
      string messageID);

    void LaunchAIQIncomeAnalyzer();

    DialogResult OpenModal(string openModalOptions);

    void RedirectToUrl(string targetName);
  }
}
