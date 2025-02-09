// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.StandardAlerts
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common.Licensing;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class StandardAlerts
  {
    private static readonly List<AlertDefinition> alertDefs = new List<AlertDefinition>();
    private static readonly Dictionary<StandardAlertID, AlertDefinition> alertConfigMap = new Dictionary<StandardAlertID, AlertDefinition>();

    static StandardAlerts()
    {
      StandardAlerts.createWorkflowAlert(StandardAlertID.MilestoneExpected, EncompassEdition.None, "Milestone Expected", AlertTiming.DaysBefore, AlertNotificationType.None, 0, "Days to Finish");
      StandardAlerts.createWorkflowAlert(StandardAlertID.MilestoneFinished, EncompassEdition.None, "Milestone Finished", AlertTiming.Immediate, AlertNotificationType.Fixed, 1, "Finished Date");
      StandardAlerts.createWorkflowAlert(StandardAlertID.DocumentExpected, EncompassEdition.None, "Document Expected", AlertTiming.DaysBefore, AlertNotificationType.None, 2, "Days to Receive");
      StandardAlerts.createWorkflowAlert(StandardAlertID.DocumentExpired, EncompassEdition.None, "Document Expired", AlertTiming.DaysBefore, AlertNotificationType.None, 3, "Days to Expire");
      StandardAlerts.createWorkflowAlert(StandardAlertID.eFolderUpdate, EncompassEdition.None, "eFolder Update", AlertTiming.Immediate, AlertNotificationType.Fixed, 4, "Send Update Alert check box");
      StandardAlerts.createWorkflowAlert(StandardAlertID.ConversationFollowUp, EncompassEdition.None, "Conversation Follow Up", AlertTiming.DaysBefore, AlertNotificationType.None, 5, "Follow Up Date");
      StandardAlerts.createWorkflowAlert(StandardAlertID.TaskDue, EncompassEdition.None, "Task Expected", AlertTiming.DaysBefore, AlertNotificationType.None, 6, "Days to Complete");
      StandardAlerts.createWorkflowAlert(StandardAlertID.TaskFollowUp, EncompassEdition.None, "Task Follow Up", AlertTiming.DaysBefore, AlertNotificationType.None, 7, "Follow Up Date");
      StandardAlerts.createWorkflowAlert(StandardAlertID.PreliminaryConditionExpected, EncompassEdition.Banker, "Preliminary Condition Expected", AlertTiming.DaysBefore, AlertNotificationType.None, 8, "Days to Receive");
      StandardAlerts.createWorkflowAlert(StandardAlertID.UnderwritingConditionExpected, EncompassEdition.Banker, "Underwriting Condition Expected", AlertTiming.DaysBefore, AlertNotificationType.None, 9, "Days to Receive");
      StandardAlerts.createWorkflowAlert(StandardAlertID.PostClosingConditionExpected, EncompassEdition.Banker, "Post Closing Condition Expected", AlertTiming.DaysBefore, AlertNotificationType.None, 10, "Days to Receive");
      StandardAlerts.createWorkflowAlert(StandardAlertID.RegistrationExpired, EncompassEdition.Banker, "Registration Expiration", AlertTiming.DaysBefore, AlertNotificationType.None, 11, "Registration");
      StandardAlerts.createWorkflowAlert(StandardAlertID.RateLockRequested, EncompassEdition.Banker, "Rate Lock Requested", AlertTiming.Immediate, AlertNotificationType.Configurable, 12, "Lock Request Form");
      StandardAlerts.createWorkflowAlert(StandardAlertID.RateLockConfirm, EncompassEdition.Banker, "Rate Lock Request Confirmed", AlertTiming.Immediate, AlertNotificationType.Configurable, 13, "Secondary Registration");
      StandardAlerts.createWorkflowAlert(StandardAlertID.RateLockDenied, EncompassEdition.Banker, "Rate Lock Request Denied", AlertTiming.Immediate, AlertNotificationType.Configurable, 14, "Secondary Denied");
      StandardAlerts.createWorkflowAlert(StandardAlertID.RateLockExpired, EncompassEdition.None, "Rate Lock Expired", AlertTiming.DaysBefore, AlertNotificationType.None, 15, "Lock Expiration Date (762)");
      StandardAlerts.createWorkflowAlert(StandardAlertID.RateLockCancellationRequested, EncompassEdition.Banker, "Rate Cancellation Requested", AlertTiming.Immediate, AlertNotificationType.Configurable, 16, "Lock Expiration Date (762)");
      StandardAlerts.createWorkflowAlert(StandardAlertID.RateLockCancelled, EncompassEdition.Banker, "Rate Lock Cancelled", AlertTiming.Immediate, AlertNotificationType.Configurable, 17, "Lock Expiration Date (762)");
      StandardAlerts.createWorkflowAlert(StandardAlertID.RateLockRemovedFromTrade, EncompassEdition.Banker, "Rate Lock Removed from Correspondent Trade", AlertTiming.Immediate, AlertNotificationType.Configurable, 44, "Remove from Correspondent Trade");
      StandardAlerts.createWorkflowAlert(StandardAlertID.LockVoided, EncompassEdition.Banker, "Lock Voided", AlertTiming.Immediate, AlertNotificationType.Configurable, 69, "Lock Voided");
      RegulationAlert regulationAlert1 = StandardAlerts.createRegulationAlert(StandardAlertID.KeyPricingFieldAlert, EncompassEdition.None, "Key Pricing Fields", AlertTiming.Immediate, AlertNotificationType.None, true, 18);
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("2"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("1821"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("356"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("14"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("13"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("VASUMM.X23"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("4"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("608"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("19"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("1172"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("CASASRN.X141"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("4645"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("1269"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("1613"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("1270"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("1614"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("1271"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("1615"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("1272"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("1616"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("1273"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("1617"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("1274"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("1618"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("4535"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("4541"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("4536"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("4542"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("4537"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("4543"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("4538"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("4544"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("4539"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("4545"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("4540"));
      regulationAlert1.TriggerFields.Add(new AlertTriggerField("4546"));
      StandardAlerts.createWorkflowAlert(StandardAlertID.PaymentPastDue, EncompassEdition.Banker, "Borrower Payment Past Due", AlertTiming.DaysBefore, AlertNotificationType.None, 19, "Late Payment Date (SERVICE.X15)");
      StandardAlerts.createWorkflowAlert(StandardAlertID.PrintMailPaymentStatement, EncompassEdition.Banker, "Statement Printing/Mailing Due", AlertTiming.DaysBefore, AlertNotificationType.None, 20, "Statement Date (SERVICE.X10)");
      StandardAlerts.createWorkflowAlert(StandardAlertID.EscrowDisbursementDue, EncompassEdition.Banker, "Escrow Disbursement Due", AlertTiming.DaysBefore, AlertNotificationType.None, 21, "Disbursement Due Date (SERVICE.X59-73)");
      StandardAlerts.createWorkflowAlert(StandardAlertID.PurchaseAdviceForm, EncompassEdition.Banker, "Purchase Advice Form Does Not Balance", AlertTiming.Immediate, AlertNotificationType.None, 22, "Reconciled Difference (2629)");
      StandardAlerts.createWorkflowAlert(StandardAlertID.ShippingExpected, EncompassEdition.Banker, "Shipping Due", AlertTiming.DaysBefore, AlertNotificationType.None, 23, "Investor Delivery Date (2012)");
      StandardAlerts.createRegulationAlert(StandardAlertID.RediscloseTILRateChange, EncompassEdition.None, "Redisclose REGZ - TIL (APR Change)", AlertTiming.Immediate, AlertNotificationType.None, false, 24).TriggerFields.Add(new AlertTriggerField("3121", "0.125/0.25%"));
      RegulationAlert regulationAlert2 = StandardAlerts.createRegulationAlert(StandardAlertID.RediscloseCDAPR_Product_Prepay, EncompassEdition.None, "Redisclose Closing Disclosure (APR, Product, Prepay)", AlertTiming.Immediate, AlertNotificationType.None, false, 49);
      regulationAlert2.TriggerFields.Add(new AlertTriggerField("3121", "0.125/0.25%"));
      regulationAlert2.TriggerFields.Add(new AlertTriggerField("LE1.X5", "any change"));
      regulationAlert2.TriggerFields.Add(new AlertTriggerField("675", "Yes"));
      regulationAlert2.TriggerFields.Add(new AlertTriggerField("CD1.X47"));
      RegulationAlert regulationAlert3 = StandardAlerts.createRegulationAlert(StandardAlertID.RediscloseCdAfterVestingNboAdded, EncompassEdition.None, "Redisclose Closing Disclosure (NBO Updated)", AlertTiming.Immediate, AlertNotificationType.None, false, 49);
      regulationAlert3.TriggerFields.Add(new AlertTriggerField("CD1.X47"));
      regulationAlert3.TriggerFields.Add(new AlertTriggerField("NewVestingNboAlert"));
      RegulationAlert regulationAlert4 = StandardAlerts.createRegulationAlert(StandardAlertID.ClosingDateViolation, EncompassEdition.None, "Closing Date Violation", AlertTiming.Immediate, AlertNotificationType.None, false, 25);
      regulationAlert4.TriggerFields.Add(new AlertTriggerField("763"));
      regulationAlert4.TriggerFields.Add(new AlertTriggerField("3147"));
      RegulationAlert regulationAlert5 = StandardAlerts.createRegulationAlert(StandardAlertID.InitialDisclosures, EncompassEdition.None, "Send Initial Disclosures", AlertTiming.DaysBefore, AlertNotificationType.None, true, 26);
      regulationAlert5.TriggerFields.Add(new AlertTriggerField("4000"));
      regulationAlert5.TriggerFields.Add(new AlertTriggerField("4002"));
      regulationAlert5.TriggerFields.Add(new AlertTriggerField("65"));
      regulationAlert5.TriggerFields.Add(new AlertTriggerField("736"));
      regulationAlert5.TriggerFields.Add(new AlertTriggerField("11"));
      regulationAlert5.TriggerFields.Add(new AlertTriggerField("12"));
      regulationAlert5.TriggerFields.Add(new AlertTriggerField("14"));
      regulationAlert5.TriggerFields.Add(new AlertTriggerField("15"));
      regulationAlert5.TriggerFields.Add(new AlertTriggerField("1821"));
      regulationAlert5.TriggerFields.Add(new AlertTriggerField("1109"));
      regulationAlert5.TriggerFields.Add(new AlertTriggerField("MORNET.X40"));
      RegulationAlert regulationAlert6 = StandardAlerts.createRegulationAlert(StandardAlertID.eSignconsentNotYetReceived, EncompassEdition.None, "eConsent Not Yet Received", AlertTiming.Immediate, AlertNotificationType.None, false, 27);
      regulationAlert6.TriggerFields.Add(new AlertTriggerField("3142"));
      regulationAlert6.TriggerFields.Add(new AlertTriggerField("3984"));
      regulationAlert6.TriggerFields.Add(new AlertTriggerField("3988"));
      regulationAlert6.TriggerFields.Add(new AlertTriggerField("3992"));
      regulationAlert6.TriggerFields.Add(new AlertTriggerField("3996"));
      regulationAlert6.TriggerFields.Add(new AlertTriggerField("4023"));
      regulationAlert6.TriggerFields.Add(new AlertTriggerField("4027"));
      regulationAlert6.TriggerFields.Add(new AlertTriggerField("4031"));
      regulationAlert6.TriggerFields.Add(new AlertTriggerField("4035"));
      regulationAlert6.TriggerFields.Add(new AlertTriggerField("4039"));
      regulationAlert6.TriggerFields.Add(new AlertTriggerField("4043"));
      regulationAlert6.TriggerFields.Add(new AlertTriggerField("4047"));
      regulationAlert6.TriggerFields.Add(new AlertTriggerField("4051"));
      regulationAlert6.TriggerFields.Add(new AlertTriggerField("NBOC0017"));
      StandardAlerts.createRegulationAlert(StandardAlertID.GFEExpires, EncompassEdition.None, "GFE Expires", AlertTiming.DaysBefore, AlertNotificationType.None, false, 27).TriggerFields.Add(new AlertTriggerField("3140"));
      StandardAlerts.createRegulationAlert(StandardAlertID.LoanEstimatesExpires, EncompassEdition.None, "Loan Estimate Expires", AlertTiming.DaysBefore, AlertNotificationType.None, false, 27).TriggerFields.Add(new AlertTriggerField("LE1.X28"));
      RegulationAlert regulationAlert7 = StandardAlerts.createRegulationAlert(StandardAlertID.RediscloseGFERateLocked, EncompassEdition.None, "Redisclose GFE (Rate Lock)", AlertTiming.Immediate, AlertNotificationType.None, false, 28);
      regulationAlert7.TriggerFields.Add(new AlertTriggerField("761"));
      regulationAlert7.TriggerFields.Add(new AlertTriggerField("3137"));
      RegulationAlert regulationAlert8 = StandardAlerts.createRegulationAlert(StandardAlertID.RediscloseGFEChangedCircumstances, EncompassEdition.None, "Redisclose GFE (Changed Circumstance)", AlertTiming.DaysBefore, AlertNotificationType.None, false, 29);
      regulationAlert8.TriggerFields.Add(new AlertTriggerField("3168"));
      regulationAlert8.TriggerFields.Add(new AlertTriggerField("3165"));
      RegulationAlertWithDataCompletionFields completionFields1 = StandardAlerts.createRegulationAlertWithDataCompletionFields(StandardAlertID.RediscloseLEChangedCircumstances, EncompassEdition.None, "Redisclose Loan Estimate (Changed Circumstance)", AlertTiming.DaysBefore, AlertNotificationType.None, false, 29, true);
      completionFields1.TriggerFields.Add(new AlertTriggerField("3168"));
      completionFields1.TriggerFields.Add(new AlertTriggerField("3165"));
      RegulationAlert regulationAlert9 = StandardAlerts.createRegulationAlert(StandardAlertID.HUD1ToleranceViolation, EncompassEdition.None, "HUD-1 Tolerance Violated", AlertTiming.Immediate, AlertNotificationType.None, false, 30);
      regulationAlert9.TriggerFields.Add(new AlertTriggerField("NEWHUD.X12", "0"));
      regulationAlert9.TriggerFields.Add(new AlertTriggerField("NEWHUD.X13", "0"));
      regulationAlert9.TriggerFields.Add(new AlertTriggerField("NEWHUD.X16", "0"));
      regulationAlert9.TriggerFields.Add(new AlertTriggerField("NEWHUD.X76", "0"));
      regulationAlert9.TriggerFields.Add(new AlertTriggerField("NEWHUD.X315", "10%"));
      StandardAlerts.createWorkflowAlert(StandardAlertID.ComplianceReview, EncompassEdition.None, "Compliance Review", AlertTiming.Immediate, AlertNotificationType.None, 31, "Compliance Test Result");
      StandardAlerts.createRegulationAlert(StandardAlertID.AbilityToRepayLoanTypeNotDetermined, EncompassEdition.None, "Ability-To-Repay Loan Type Not Determined", AlertTiming.Immediate, AlertNotificationType.None, false, 35).TriggerFields.Add(new AlertTriggerField("QM.X23"));
      RegulationAlert regulationAlert10 = StandardAlerts.createRegulationAlert(StandardAlertID.QualifiedMortgageTypeNotDetermined, EncompassEdition.None, "Qualified Mortgage Type Not Determined", AlertTiming.Immediate, AlertNotificationType.None, false, 36);
      regulationAlert10.TriggerFields.Add(new AlertTriggerField("QM.X23"));
      regulationAlert10.TriggerFields.Add(new AlertTriggerField("QM.X24"));
      RegulationAlert regulationAlert11 = StandardAlerts.createRegulationAlert(StandardAlertID.QMSafeHarborEligibilityNotDetermined, EncompassEdition.None, "QM Safe Harbor Eligibility Not Determined", AlertTiming.Immediate, AlertNotificationType.None, false, 37);
      regulationAlert11.TriggerFields.Add(new AlertTriggerField("QM.X23"));
      regulationAlert11.TriggerFields.Add(new AlertTriggerField("QM.X25"));
      RegulationAlert regulationAlert12 = StandardAlerts.createRegulationAlert(StandardAlertID.ResidualIncomeAssessmentRecommended, EncompassEdition.None, "Residual Income Assessment Recommended", AlertTiming.Immediate, AlertNotificationType.None, false, 38);
      regulationAlert12.TriggerFields.Add(new AlertTriggerField("QM.X23"));
      regulationAlert12.TriggerFields.Add(new AlertTriggerField("QM.X25"));
      RegulationAlert regulationAlert13 = StandardAlerts.createRegulationAlert(StandardAlertID.GeneralQMDTIExceeded, EncompassEdition.None, "General QM DTI Exceeded", AlertTiming.Immediate, AlertNotificationType.None, false, 39);
      regulationAlert13.TriggerFields.Add(new AlertTriggerField("QM.X23"));
      regulationAlert13.TriggerFields.Add(new AlertTriggerField("QM.X24"));
      regulationAlert13.TriggerFields.Add(new AlertTriggerField("QM.X119"));
      regulationAlert13.TriggerFields.Add(new AlertTriggerField("QM.X383"));
      RegulationAlert regulationAlert14 = StandardAlerts.createRegulationAlert(StandardAlertID.GeneralQMLoanFeatureViolation, EncompassEdition.None, "General QM Loan Feature Violation", AlertTiming.Immediate, AlertNotificationType.None, false, 40);
      regulationAlert14.TriggerFields.Add(new AlertTriggerField("QM.X23"));
      regulationAlert14.TriggerFields.Add(new AlertTriggerField("QM.X24"));
      regulationAlert14.TriggerFields.Add(new AlertTriggerField("2982"));
      regulationAlert14.TriggerFields.Add(new AlertTriggerField("NEWHUD.X6"));
      regulationAlert14.TriggerFields.Add(new AlertTriggerField("1659"));
      regulationAlert14.TriggerFields.Add(new AlertTriggerField("RE88395.X316"));
      regulationAlert14.TriggerFields.Add(new AlertTriggerField("QM.X112"));
      regulationAlert14.TriggerFields.Add(new AlertTriggerField("QM.X124"));
      RegulationAlert regulationAlert15 = StandardAlerts.createRegulationAlert(StandardAlertID.AbilitytoRepayExemptionReasonNotDetermined, EncompassEdition.None, "Ability-to-Repay Exemption Reason Not Determined", AlertTiming.Immediate, AlertNotificationType.None, false, 41);
      regulationAlert15.TriggerFields.Add(new AlertTriggerField("QM.X23"));
      regulationAlert15.TriggerFields.Add(new AlertTriggerField("QM.X105"));
      regulationAlert15.TriggerFields.Add(new AlertTriggerField("QM.X107"));
      regulationAlert15.TriggerFields.Add(new AlertTriggerField("QM.X108"));
      regulationAlert15.TriggerFields.Add(new AlertTriggerField("QM.X110"));
      RegulationAlert regulationAlert16 = StandardAlerts.createRegulationAlert(StandardAlertID.AUSDataDiscrepancyAlert, EncompassEdition.None, "AUS Data Discrepancy Alert", AlertTiming.Immediate, AlertNotificationType.None, false, 42);
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("1544"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("2"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("356"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("3"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("1172"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("4"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("608"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("19"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("MORNET.X75"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("MORNET.X76"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("1389"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("912"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("1731"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("740"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("742"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("AUS.X16"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("AUS.X18"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("AUS.X19"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("AUS.X20"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("AUS.X21"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("AUS.X22"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("AUS.X23"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("AUS.X11"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("AUS.X12"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("AUS.X41"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("AUS.X32"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("AUS.X14"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("AUS.X15"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("CASASRN.X201"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("CASASRN.X202"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("CASASRN.X217"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("CASASRN.X218"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("1821"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("MORNET.X158"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("MORNET.X159"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("MORNET.X160"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("4752"));
      regulationAlert16.TriggerFields.Add(new AlertTriggerField("AUS.X199"));
      RegulationAlertWithDataCompletionFields completionFields2 = StandardAlerts.createRegulationAlertWithDataCompletionFields(StandardAlertID.RediscloseLERateLock, EncompassEdition.None, "Redisclose Loan Estimate (Rate Lock)", AlertTiming.Immediate, AlertNotificationType.None, false, 47, true);
      completionFields2.TriggerFields.Add(new AlertTriggerField("761"));
      completionFields2.TriggerFields.Add(new AlertTriggerField("LE1.X33"));
      completionFields2.TriggerFields.Add(new AlertTriggerField("CD1.X1"));
      RegulationAlert regulationAlert17 = StandardAlerts.createRegulationAlert(StandardAlertID.RediscloseCDChangedCircumstances, EncompassEdition.None, "Redisclose Closing Disclosure (Changed Circumstance)", AlertTiming.DaysBefore, AlertNotificationType.None, false, 48);
      regulationAlert17.TriggerFields.Add(new AlertTriggerField("CD1.X61"));
      regulationAlert17.TriggerFields.Add(new AlertTriggerField("CD1.X62"));
      regulationAlert17.TriggerFields.Add(new AlertTriggerField("CD1.X55"));
      regulationAlert17.TriggerFields.Add(new AlertTriggerField("CD1.X57"));
      regulationAlert17.TriggerFields.Add(new AlertTriggerField("CD1.X58"));
      regulationAlert17.TriggerFields.Add(new AlertTriggerField("CD1.X59"));
      regulationAlert17.TriggerFields.Add(new AlertTriggerField("CD1.X68"));
      regulationAlert17.TriggerFields.Add(new AlertTriggerField("CD1.X66"));
      regulationAlert17.TriggerFields.Add(new AlertTriggerField("CD1.X67"));
      RegulationAlert regulationAlert18 = StandardAlerts.createRegulationAlert(StandardAlertID.GoodFaithFeeVarianceViolation, EncompassEdition.None, "Good Faith Fee Variance Violated", AlertTiming.Immediate, AlertNotificationType.None, false, 50);
      regulationAlert18.TriggerFields.Add(new AlertTriggerField("FV.X345"));
      regulationAlert18.TriggerFields.Add(new AlertTriggerField("FV.X347"));
      StandardAlerts.createWorkflowAlert(StandardAlertID.FannieServiceDu, EncompassEdition.None, "Fannie Service DU", AlertTiming.Immediate, AlertNotificationType.None, 51, "Fannie Service DU Result");
      StandardAlerts.createWorkflowAlert(StandardAlertID.FannieServiceEc, EncompassEdition.None, "Fannie Service EC", AlertTiming.Immediate, AlertNotificationType.None, 52, "Fannie Service EC Result");
      StandardAlerts.createWorkflowAlert(StandardAlertID.FreddieServiceLpa, EncompassEdition.None, "Freddie Service LPA", AlertTiming.Immediate, AlertNotificationType.None, 53, "Freddie Service LPA Result");
      StandardAlerts.createWorkflowAlert(StandardAlertID.FreddieServiceLqa, EncompassEdition.None, "Freddie Service LQA", AlertTiming.Immediate, AlertNotificationType.None, 54, "Freddie Service LQA Result");
      RegulationAlert regulationAlert19 = StandardAlerts.createRegulationAlert(StandardAlertID.RediscloseCDRateLock, EncompassEdition.None, "Redisclose Closing Disclosure (Rate Lock)", AlertTiming.Immediate, AlertNotificationType.None, false, 55);
      regulationAlert19.TriggerFields.Add(new AlertTriggerField("761"));
      regulationAlert19.TriggerFields.Add(new AlertTriggerField("CD1.X47"));
      RegulationAlert regulationAlert20 = StandardAlerts.createRegulationAlert(StandardAlertID.WithdrawnLoan, EncompassEdition.None, "Withdrawn Loan", AlertTiming.Immediate, AlertNotificationType.None, false, 58);
      StandardAlerts.createWorkflowAlert(StandardAlertID.MIServiceArch, EncompassEdition.None, "MI Service Arch", AlertTiming.Immediate, AlertNotificationType.None, 59, "MI Service Arch Result");
      StandardAlerts.createWorkflowAlert(StandardAlertID.MIServiceRadian, EncompassEdition.None, "MI Service Radian", AlertTiming.Immediate, AlertNotificationType.None, 60, "MI Service Radian Result");
      StandardAlerts.createWorkflowAlert(StandardAlertID.MIServiceMgic, EncompassEdition.None, "MI Service MGIC", AlertTiming.Immediate, AlertNotificationType.None, 61, "MI Service MGIC Result");
      regulationAlert20.TriggerFields.Add(new AlertTriggerField("4120"));
      RegulationAlert regulationAlert21 = StandardAlerts.createRegulationAlert(StandardAlertID.VADiscountChargeViolation, EncompassEdition.None, "VA Discount Charge Violation", AlertTiming.Immediate, AlertNotificationType.None, false, 62);
      regulationAlert21.TriggerFields.Add(new AlertTriggerField("1172"));
      regulationAlert21.TriggerFields.Add(new AlertTriggerField("VARRRWS.X9"));
      regulationAlert21.TriggerFields.Add(new AlertTriggerField("958"));
      StandardAlerts.createRegulationAlert(StandardAlertID.PositiveAggregateEscrowAdjustment, EncompassEdition.None, "Positive Aggregate Escrow Adjustment", AlertTiming.Immediate, AlertNotificationType.None, false, 63).TriggerFields.Add(new AlertTriggerField("558"));
      RegulationAlert regulationAlert22 = StandardAlerts.createRegulationAlert(StandardAlertID.CreditLimitRequired, EncompassEdition.None, "Credit Limit Required", AlertTiming.Immediate, AlertNotificationType.None, false, 64);
      regulationAlert22.TriggerFields.Add(new AlertTriggerField("FL0031"));
      regulationAlert22.TriggerFields.Add(new AlertTriggerField("FL0008"));
      regulationAlert22.TriggerFields.Add(new AlertTriggerField("FL0018"));
      RegulationAlert regulationAlert23 = StandardAlerts.createRegulationAlert(StandardAlertID.LienPositionRequiredIfResubordinated, EncompassEdition.None, "Current and Proposed Lien position required if Resubordinated", AlertTiming.Immediate, AlertNotificationType.None, false, 65);
      regulationAlert23.TriggerFields.Add(new AlertTriggerField("FL0028"));
      regulationAlert23.TriggerFields.Add(new AlertTriggerField("FL0029"));
      regulationAlert23.TriggerFields.Add(new AlertTriggerField("FL0026"));
      RegulationAlert regulationAlert24 = StandardAlerts.createRegulationAlert(StandardAlertID.LienPositionRequiredIfSubjectProperty, EncompassEdition.None, "Current and Proposed Lien position required if Subject Property", AlertTiming.Immediate, AlertNotificationType.None, false, 66);
      regulationAlert24.TriggerFields.Add(new AlertTriggerField("FL0028"));
      regulationAlert24.TriggerFields.Add(new AlertTriggerField("FL0029"));
      regulationAlert24.TriggerFields.Add(new AlertTriggerField("FL0027"));
      regulationAlert24.TriggerFields.Add(new AlertTriggerField("FL0018"));
      RegulationAlert regulationAlert25 = StandardAlerts.createRegulationAlert(StandardAlertID.GeneralQMPriceExceeded, EncompassEdition.None, "General QM Price Limit Exceeded", AlertTiming.Immediate, AlertNotificationType.None, false, 70);
      regulationAlert25.TriggerFields.Add(new AlertTriggerField("QM.X23"));
      regulationAlert25.TriggerFields.Add(new AlertTriggerField("QM.X24"));
      regulationAlert25.TriggerFields.Add(new AlertTriggerField("QM.X383"));
      regulationAlert25.TriggerFields.Add(new AlertTriggerField("QM.X40"));
      StandardAlerts.createRegulationAlertWithDataCompletionFields(StandardAlertID.ThreeDayDisclosureRequirements, EncompassEdition.None, "Three-Day Disclosure Requirements", AlertTiming.Immediate, AlertNotificationType.None, false, 72, true).TriggerFields.Add(new AlertTriggerField("3142"));
      StandardAlerts.createRegulationAlertWithDataCompletionFields(StandardAlertID.AtAppDisclosureRequirements, EncompassEdition.None, "At App Disclosure Requirements", AlertTiming.Immediate, AlertNotificationType.None, false, 73, true).TriggerFields.Add(new AlertTriggerField("HMDA.X29"));
      StandardAlerts.createWorkflowAlert(StandardAlertID.LiborIndex, EncompassEdition.None, LiborTransitionToSofrAlert.Name, AlertTiming.Immediate, AlertNotificationType.None, 68, LiborTransitionToSofrAlert.TriggerDescription);
      StandardAlerts.alertDefs.Sort();
    }

    public static IEnumerable<AlertDefinition> All
    {
      get => (IEnumerable<AlertDefinition>) StandardAlerts.alertDefs;
    }

    public static AlertDefinition GetDefinition(int alertId)
    {
      return StandardAlerts.GetDefinition((StandardAlertID) alertId);
    }

    public static AlertDefinition GetDefinition(StandardAlertID alertId)
    {
      return StandardAlerts.alertConfigMap.ContainsKey(alertId) ? StandardAlerts.alertConfigMap[alertId] : (AlertDefinition) null;
    }

    public static AlertDefinition[] GetDefinitionsForEdition(EncompassEdition edition)
    {
      List<AlertDefinition> alertDefinitionList = new List<AlertDefinition>();
      foreach (AlertDefinition alertDef in StandardAlerts.alertDefs)
      {
        if (alertDef.AppliesToEdition(edition))
          alertDefinitionList.Add(alertDef);
      }
      return alertDefinitionList.ToArray();
    }

    public static bool IsPredefinedAlertID(int alertId)
    {
      return StandardAlerts.alertConfigMap.ContainsKey((StandardAlertID) alertId);
    }

    private static WorkflowAlert createWorkflowAlert(
      StandardAlertID alertId,
      EncompassEdition edition,
      string name,
      AlertTiming timing,
      AlertNotificationType notificationType,
      int displayOrder,
      string triggerDescription)
    {
      WorkflowAlert alertDef = new WorkflowAlert(alertId, name, edition, timing, notificationType, displayOrder, triggerDescription);
      StandardAlerts.addAlert((AlertDefinition) alertDef);
      return alertDef;
    }

    private static RegulationAlert createRegulationAlert(
      StandardAlertID alertId,
      EncompassEdition edition,
      string name,
      AlertTiming timing,
      AlertNotificationType notificationType,
      bool allowUserDefinedTriggerFields,
      int displayOrder)
    {
      RegulationAlert alertDef = new RegulationAlert(alertId, name, edition, timing, notificationType, allowUserDefinedTriggerFields, displayOrder);
      StandardAlerts.addAlert((AlertDefinition) alertDef);
      return alertDef;
    }

    private static RegulationAlertWithDataCompletionFields createRegulationAlertWithDataCompletionFields(
      StandardAlertID alertId,
      EncompassEdition edition,
      string name,
      AlertTiming timing,
      AlertNotificationType notificationType,
      bool allowUserDefinedTriggerFields,
      int displayOrder,
      bool allowCustomDataCompletionFields)
    {
      RegulationAlertWithDataCompletionFields alertDef = new RegulationAlertWithDataCompletionFields(alertId, name, edition, timing, notificationType, allowUserDefinedTriggerFields, displayOrder, allowCustomDataCompletionFields);
      StandardAlerts.addAlert((AlertDefinition) alertDef);
      return alertDef;
    }

    private static void addAlert(AlertDefinition alertDef)
    {
      StandardAlerts.alertDefs.Add(alertDef);
      StandardAlerts.alertConfigMap.Add((StandardAlertID) alertDef.AlertID, alertDef);
    }
  }
}
