// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public interface IDisclosureTracking2015Log : IDisclosureTrackingLog
  {
    string Guid { get; }

    bool IsLocked { get; set; }

    DateTime Date { get; }

    TimeZoneInfo TimeZoneInfo { get; }

    DateTime DisclosedDate { get; set; }

    DateTime DisclosedDateTime { get; }

    DateTime DateAdded { get; }

    bool IsDisclosedByLocked { get; set; }

    string LockedDisclosedByField { get; set; }

    string DisclosedByFullName { get; set; }

    string DisclosedBy { get; set; }

    bool IsDisclosedReceivedDateLocked { get; set; }

    DateTime ReceivedDate { get; set; }

    DateTime PresumedFulfillmentDate { get; set; }

    DateTime ActualFulfillmentDate { get; set; }

    DisclosureTrackingBase.DisclosedMethod DisclosureMethod { get; set; }

    string DisclosedMethodOther { get; set; }

    string DisclosedMethodName { get; }

    bool DisclosedForLE { get; }

    bool DisclosedForCD { get; }

    bool DisclosedForSafeHarbor { get; }

    bool ProviderListSent { get; }

    bool ProviderListNoFeeSent { get; }

    bool IsNboExist { get; }

    bool UseForUCDExport { get; set; }

    DisclosureTracking2015Log.DisclosureTypeEnum DisclosureType { get; set; }

    string ChangeInCircumstanceComments { get; set; }

    string DisclosedAPR { get; set; }

    bool IsDisclosedAPRLocked { get; set; }

    string FinanceCharge { get; set; }

    string DisclosedDailyInterest { get; set; }

    bool IsDisclosed { get; set; }

    int NumOfDisclosedDocs { get; }

    Dictionary<string, INonBorrowerOwnerItem> NonBorrowerOwnerCollections { get; }

    string LinkedGuid { get; set; }

    DateTime eDisclosurePackageCreatedDate { get; set; }

    string eDisclosurePackageID { get; set; }

    DisclosedLoanItem[] DisclosedData { get; }

    string BorrowerName { get; }

    DisclosureTrackingBase.DisclosedMethod BorrowerDisclosedMethod { get; set; }

    string BorrowerDisclosedMethodOther { get; set; }

    string BorrowerFulfillmentMethodDescription { get; set; }

    bool IsBorrowerPresumedDateLocked { get; set; }

    DateTime BorrowerPresumedReceivedDate { get; set; }

    DateTime LockedBorrowerPresumedReceivedDate { get; set; }

    DateTime BorrowerActualReceivedDate { get; set; }

    bool IsBorrowerTypeLocked { get; set; }

    string BorrowerType { get; set; }

    string LockedBorrowerType { get; set; }

    DateTime FullfillmentProcessedDate { get; set; }

    bool IsWetSigned { get; set; }

    EnhancedDisclosureTracking2015Log.DisclosureTrackingDocuments Documents { get; set; }

    string EDisclosureBorrowerLoanLevelConsent { get; set; }

    DateTime EDisclosureBorrowerDocumentViewedDate { get; }

    string eDisclosureBorrowerName { get; set; }

    DateTime eDisclosureBorrowerInformationalViewedDate { get; set; }

    string eDisclosureBorrowerInformationalViewedIP { get; set; }

    DateTime eDisclosureBorrowerInformationalCompletedDate { get; set; }

    string eDisclosureBorrowerInformationalCompletedIP { get; set; }

    DateTime eDisclosureBorrowereSignedDate { get; set; }

    DateTime eDisclosureBorrowerViewMessageDate { get; set; }

    DateTime eDisclosureBorrowerAcceptConsentDate { get; set; }

    DateTime eDisclosureBorrowerRejectConsentDate { get; set; }

    DateTime eDisclosureBorrowerAuthenticatedDate { get; set; }

    string eDisclosureBorrowerAuthenticatedIP { get; set; }

    string eDisclosureBorrowerEmail { get; set; }

    string eDisclosureBorrowerAcceptConsentIP { get; set; }

    string eDisclosureBorrowerRejectConsentIP { get; set; }

    string eDisclosureBorrowereSignedIP { get; set; }

    string Comments { get; set; }

    string CoBorrowerName { get; }

    DisclosureTrackingBase.DisclosedMethod CoBorrowerDisclosedMethod { get; set; }

    string CoBorrowerDisclosedMethodOther { get; set; }

    string CoBorrowerFulfillmentMethodDescription { get; set; }

    bool IsCoBorrowerPresumedDateLocked { get; set; }

    DateTime CoBorrowerPresumedReceivedDate { get; set; }

    DateTime LockedCoBorrowerPresumedReceivedDate { get; set; }

    DateTime CoBorrowerActualReceivedDate { get; set; }

    bool IsCoBorrowerTypeLocked { get; set; }

    string CoBorrowerType { get; set; }

    string LockedCoBorrowerType { get; set; }

    bool IntentToProceed { get; set; }

    bool LEDisclosedByBroker { get; }

    DateTime FullfillmentProcessedDate_CoBorrower { get; set; }

    string EDisclosureCoBorrowerLoanLevelConsent { get; set; }

    DateTime EDisclosureCoborrowerDocumentViewedDate { get; }

    string eDisclosureCoBorrowerName { get; set; }

    DateTime eDisclosureCoBorrowerInformationalViewedDate { get; set; }

    string eDisclosureCoBorrowerInformationalViewedIP { get; set; }

    DateTime eDisclosureCoBorrowerInformationalCompletedDate { get; set; }

    string eDisclosureCoBorrowerInformationalCompletedIP { get; set; }

    DateTime eDisclosureCoBorrowereSignedDate { get; set; }

    DateTime eDisclosureCoBorrowerViewMessageDate { get; set; }

    DateTime eDisclosureCoBorrowerAcceptConsentDate { get; set; }

    DateTime eDisclosureCoBorrowerRejectConsentDate { get; set; }

    DateTime eDisclosureCoBorrowerAuthenticatedDate { get; set; }

    string eDisclosureCoBorrowerAuthenticatedIP { get; set; }

    string eDisclosureCoBorrowerEmail { get; set; }

    string eDisclosureCoBorrowerAcceptConsentIP { get; set; }

    string eDisclosureCoBorrowerRejectConsentIP { get; set; }

    string eDisclosureCoBorrowereSignedIP { get; set; }

    string eDisclosureLOeSignedIP { get; set; }

    string eDisclosureLOName { get; set; }

    string eDisclosureLOUserId { get; set; }

    DateTime eDisclosureLOViewMessageDate { get; set; }

    DateTime eDisclosureLOInformationalViewedDate { get; set; }

    string eDisclosureLOInformationalViewedIP { get; set; }

    DateTime eDisclosureLOInformationalCompletedDate { get; set; }

    string eDisclosureLOInformationalCompletedIP { get; set; }

    DateTime eDisclosureLOeSignedDate { get; set; }

    bool IsIntentReceivedByLocked { get; set; }

    string LockedIntentReceivedByField { get; set; }

    DateTime IntentToProceedDate { get; set; }

    string IntentToProceedReceivedBy { get; set; }

    DisclosureTrackingBase.DisclosedMethod IntentToProceedReceivedMethod { get; set; }

    string IntentToProceedReceivedMethodOther { get; set; }

    string IntentToProceedComments { get; set; }

    string GetDisclosedField(string fieldId, bool retrieveFromOtherLog);

    Dictionary<string, INonBorrowerOwnerItem> GetAllnboItems();

    void SetnboAttributeValue(
      string nboInstance,
      object value,
      DisclosureTracking2015Log.NBOUpdatableFields fld);

    string GetnboAttributeValue(
      string nboInstance,
      DisclosureTracking2015Log.NBOUpdatableFields flds);

    void RemoveShapsnot();

    bool CDReasonIsChangeInAPR { get; set; }

    bool CDReasonIsChangeInLoanProduct { get; set; }

    bool CDReasonIsPrepaymentPenaltyAdded { get; set; }

    bool CDReasonIsChangeInSettlementCharges { get; set; }

    bool CDReasonIs24HourAdvancePreview { get; set; }

    bool CDReasonIsToleranceCure { get; set; }

    bool CDReasonIsClericalErrorCorrection { get; set; }

    bool CDReasonIsChangedCircumstanceEligibility { get; set; }

    bool CDReasonIsRevisionsRequestedByConsumer { get; set; }

    bool CDReasonIsInterestRateDependentCharges { get; set; }

    bool CDReasonIsOther { get; set; }

    bool LEReasonIsChangedCircumstanceSettlementCharges { get; set; }

    bool LEReasonIsChangedCircumstanceEligibility { get; set; }

    bool LEReasonIsRevisionsRequestedByConsumer { get; set; }

    bool LEReasonIsInterestRateDependentCharges { get; set; }

    bool LEReasonIsExpiration { get; set; }

    bool LEReasonIsDelayedSettlementOnConstructionLoans { get; set; }

    bool LEReasonIsOther { get; set; }

    bool LEReasonAnyChecked { get; }

    void SetLoanDataFromOtherLogs(string id, string val);

    void ResetLoanDataFromOtherLogs();

    int NumberOfGoodFaithChangeOfCircumstance { get; }

    bool FeeLevelDisclosuresIndicator { get; }

    DisclosureTracking2015Log.TrackingLogStatus Status { get; set; }

    bool UCDCreationError { get; set; }

    string UCD { get; set; }

    string UDT { get; set; }

    bool IsLoanDataListExist { get; }

    Dictionary<string, string> AttributeList { get; }

    LoanData LogLoanData { get; }

    string DisclosureTypeName { get; }

    DateTime LockedDisclosedDateField { get; set; }

    DateTime OriginalDisclosedDate { get; set; }

    string eDisclosureConsentPDF { get; set; }

    bool Received { get; }

    void PopulateLoanDataList(Dictionary<string, string> dataFields);

    void AddDisclosedLoanInfo(string fieldID, string fieldValue);

    string GetCureLogComment();

    void CreateCureLog(
      double appliedCureAmount,
      string cureLogComment,
      Hashtable triggerFields,
      string resolvedById,
      string resolvedBy,
      bool newLog);

    void AppendFieldValues(Dictionary<string, string> fields);

    void SetItemizationFields(List<string[]> fields, LoanData loanDataForRead);

    void AdjustDisclosedType(DisclosureTracking2015Log.DisclosureTypeEnum type);

    int CompareDisclosedDate(IDisclosureTracking2015Log obj);
  }
}
