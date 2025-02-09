// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Export.BamObjects.DisclosureTrackingRecord2015
// Assembly: EMExport, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D06A4C35-7634-4F74-B132-8DD78784C14A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMExport.dll

using EllieMae.EMLite.Export.BamEnums;
using System;

#nullable disable
namespace EllieMae.EMLite.Export.BamObjects
{
  public class DisclosureTrackingRecord2015 : DisclosureTrackingRecordBase
  {
    public bool LEDisclosedByBroker { get; set; }

    public string DisclosureTypeName { get; set; }

    public DateTime LockedDisclosedDateField { get; set; }

    public DateTime OriginalDisclosedDate { get; set; }

    public bool ProviderListSent { get; set; }

    public string LockedDisclosedByField { get; set; }

    public string DisclosedMethodOther { get; set; }

    public string DisclosedMethodName { get; set; }

    public string DisclosedAPR { get; set; }

    public string FinanceCharge { get; set; }

    public string DisclosedDailyInterest { get; set; }

    public bool IsDisclosed { get; set; }

    public bool DisclosedForCD { get; set; }

    public bool DisclosedForLE { get; set; }

    public bool IsDisclosedAPRLocked { get; set; }

    public bool IsDisclosedFinanceChargeLocked { get; set; }

    public bool IsDisclosedDailyInterestLocked { get; set; }

    public DateTime eDisclosureManualFulfillmentDate { get; set; }

    public DisclosedMethodEnum eDisclosureManualFulfillmentMethod { get; set; }

    public bool LEReasonIsChangedCircumstanceSettlementCharges { get; set; }

    public bool LEReasonIsChangedCircumstanceEligibility { get; set; }

    public bool LEReasonIsRevisionsRequestedByConsumer { get; set; }

    public bool LEReasonIsInterestRateDependentCharges { get; set; }

    public bool LEReasonIsExpiration { get; set; }

    public bool LEReasonIsDelayedSettlementOnConstructionLoans { get; set; }

    public bool LEReasonIsOther { get; set; }

    public bool CDReasonIsChangeInAPR { get; set; }

    public bool CDReasonIsChangeInLoanProduct { get; set; }

    public bool CDReasonIsPrepaymentPenaltyAdded { get; set; }

    public bool CDReasonIsChangeInSettlementCharges { get; set; }

    public bool CDReasonIs24HourAdvancePreview { get; set; }

    public bool CDReasonIsToleranceCure { get; set; }

    public bool CDReasonIsClericalErrorCorrection { get; set; }

    public bool CDReasonIsChangedCircumstanceEligibility { get; set; }

    public bool CDReasonIsRevisionsRequestedByConsumer { get; set; }

    public bool CDReasonIsInterestRateDependentCharges { get; set; }

    public bool CDReasonIsOther { get; set; }

    public string LEReasonOther { get; set; }

    public string CDReasonOther { get; set; }

    public string ChangeInCircumstance { get; set; }

    public string ChangeInCircumstanceComments { get; set; }

    public bool IntentToProceed { get; set; }

    public DateTime IntentToProceedDate { get; set; }

    public string IntentToProceedReceivedBy { get; set; }

    public string LockedIntentReceivedByField { get; set; }

    public DisclosedMethodEnum IntentToProceedReceivedMethod { get; set; }

    public string IntentToProceedReceivedMethodOther { get; set; }

    public string IntentToProceedComments { get; set; }

    public bool IsIntentReceivedByLocked { get; set; }

    public DisclosedMethodEnum BorrowerDisclosedMethod { get; set; }

    public string BorrowerDisclosedMethodOther { get; set; }

    public bool IsBorrowerPresumedDateLocked { get; set; }

    public DateTime LockedBorrowerPresumedReceivedDate { get; set; }

    public DateTime BorrowerPresumedReceivedDate { get; set; }

    public DateTime BorrowerActualReceivedDate { get; set; }

    public string BorrowerType { get; set; }

    public DisclosedMethodEnum CoBorrowerDisclosedMethod { get; set; }

    public string CoBorrowerDisclosedMethodOther { get; set; }

    public bool IsCoBorrowerPresumedDateLocked { get; set; }

    public DateTime LockedCoBorrowerPresumedReceivedDate { get; set; }

    public DateTime CoBorrowerPresumedReceivedDate { get; set; }

    public DateTime CoBorrowerActualReceivedDate { get; set; }

    public string CoBorrowerType { get; set; }

    public string UCD { get; set; }

    public bool UseForUCDExport { get; set; }
  }
}
