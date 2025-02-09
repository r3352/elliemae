// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.BamObjects.EnhancedDisclosureTrackingRecord2015
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.ePass.BamEnums;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ePass.BamObjects
{
  public class EnhancedDisclosureTrackingRecord2015
  {
    public string Provider { get; set; }

    public string Status { get; set; }

    public DateTime DisclosedDate { get; set; }

    public virtual bool IsLocked { get; set; }

    public DateTime DateAdded { get; set; }

    public string DisclosedBy { get; set; }

    public string DisclosedByFullName { get; set; }

    public bool DisclosedForSafeHarbor { get; set; }

    public DateTime ReceivedDate { get; set; }

    public bool IsDisclosedReceivedDateLocked { get; set; }

    public List<DisclosureRecipient> DisclosureRecipients { get; set; }

    public List<FulfillmentFields> Fulfillments { get; set; }

    public DateTime DisclosureCreatedDTTM { get; set; }

    public DisclosedMethodEnum DisclosureMethod { get; set; }

    public bool Received { get; set; }

    public Address PropertyAddress { get; set; }

    public string LoanProgram { get; set; }

    public string LoanAmount { get; set; }

    public bool IsManuallyCreated { get; set; }

    public bool IsDisclosedByLocked { get; set; }

    public DateTime ApplicationDate { get; set; }

    public string BorrowerPairID { get; set; }

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

    public LoanEstimateFields LoanEstimate { get; set; }

    public ClosingDisclosureFields ClosingDisclosure { get; set; }

    public IntentToProceedFields IntentToProceed { get; set; }

    public string ChangeInCircumstance { get; set; }

    public string ChangeInCircumstanceComments { get; set; }

    public Dictionary<string, string> DisclosedFields { get; set; }

    public string UCD { get; set; }

    public bool UseForUCDExport { get; set; }

    public List<DisclosureContentType> Contents { get; set; }

    public TrackingFields Tracking { get; set; }
  }
}
