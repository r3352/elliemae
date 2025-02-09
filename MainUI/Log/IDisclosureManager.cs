// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.IDisclosureManager
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public interface IDisclosureManager
  {
    bool FeeLevelIndicator { get; }

    string[] DiscloseMethod { get; }

    LoanData LoanData { get; }

    bool CanEditSentDateAndExternalField { get; set; }

    bool HasManualFulfillmentPermission { get; set; }

    bool IsFulfillmentServiceEnabled { get; set; }

    bool IsReasonsEnabled { get; set; }

    bool IsEsignEnabled { get; }

    bool HasAccessRight { get; }

    bool IsPlatformLoan { get; }

    IDisclosureTracking2015Log DisclosureTrackingLog { get; }

    Dictionary<string, INonBorrowerOwnerItem> NBOItems { get; }

    string[] SentMethod { get; }

    string[] BorrowerType { get; }

    string[] NboType { get; }

    bool IntermediateData { get; set; }

    bool FulfillmentUpdated { get; set; }

    string eDisclosureManuallyFulfilledBy { get; set; }

    DateTime eDisclosureManualFulfillmentDate { get; set; }

    string AutomaticFullfillmentServiceName { get; }

    DateTime eDisclosurePresumedDate { get; set; }

    DateTime eDisclosureActualDate { get; set; }

    DisclosureTrackingBase.DisclosedMethod eDisclosureManualFulfillmentMethod { get; set; }

    string eDisclosureManualFulfillmentComment { get; set; }

    Dictionary<string, EnhancedDisclosureTracking2015Log.DisclosureRecipient> RecipientItems { get; }

    string NboInstance { get; set; }

    bool IsLinkedLoan { get; }

    bool IsConstructionPrimaryLoan { get; }

    DateTime BorrowerReceivedDate { get; set; }

    DateTime BorrowerActualReceivedDate { get; set; }

    DateTime DisclosedDate { get; set; }

    string[] eClosingDisclosedMethod { get; }

    string TimezoneString { get; }

    string GetTimeZoneString(DateTime dt);

    DateTime GetReceivedDateByAdding3BusinessDays(DateTime disclosedDate);

    void SetReceivedDateForNBOs(DateTime disclosedDate);

    void GetReceivedDateForNBOs(DateTime disclosedDate);

    DisclosureTrackingBase.DisclosedMethod GetDisclosedMethod(string method);

    DateTime GetNextClosestBusinessDay(DateTime date);

    void UpdateLePage1IntendToProceed(bool intendChanged);

    void UpdateFulfillment(
      string fulfillmentMethod,
      DateTime actualFulfillmentDate,
      DateTime sentDate,
      DateTime presumedFulfillmentDate,
      DateTime recipientActualReceivedDate);

    void UpdateCocFields(bool feeLevelIndicator, bool changedCircumstances);

    void UpdateNboRecievedDates(DateTime sentDate);

    void UpdateFulfillmentFields(
      string RecipientId,
      string FulfillmentOrderBy,
      string DateFulfillOrder,
      DateTime dpPresumedFulfillmentDate,
      DateTime dpActualFulfillmentDate,
      string txtFulfillmentComments,
      string txtFulfillmentMethod);

    Dictionary<string, object> CalculateNew2015DisclosureReceivedDate(
      DateTime sentDate,
      DateTime PresumedFulfillmentDate,
      DateTime ActualFulfillmentDate,
      DisclosureTrackingBase.DisclosedMethod method);

    DateTime? GetRecipientPresumedReceivedDate(bool isLocked, DateTime SentDate);

    List<Tuple<string, string, string, string, string, string>> GetXcocFields();

    void UpdateRecipients(
      Dictionary<string, EnhancedDisclosureTracking2015Log.DisclosureRecipient> localRecipients);
  }
}
