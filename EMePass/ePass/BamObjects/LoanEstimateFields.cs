// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.BamObjects.LoanEstimateFields
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ePass.BamObjects
{
  public class LoanEstimateFields
  {
    public bool IsDisclosedByBroker { get; set; }

    public bool IsChangedCircumstanceSettlementCharges { get; set; }

    public bool IsChangedCircumstanceEligibility { get; set; }

    public bool IsRevisionsRequestedByConsumer { get; set; }

    public bool IsInterestRateDependentCharges { get; set; }

    public bool IsExpiration { get; set; }

    public bool IsDelayedSettlementOnConstructionLoans { get; set; }

    public bool IsOther { get; set; }

    public string OtherDescription { get; set; }

    public DateTime ChangesReceivedDate { get; set; }

    public LoanEstimateFields(
      bool isDisclosedByBroker,
      bool isChangedCircumstanceSettlementCharges,
      bool isChangedCircumstanceEligibility,
      bool isRevisionsRequestedByConsumer,
      bool isInterestRateDependentCharges,
      bool isExpiration,
      bool isDelayedSettlementOnConstructionLoans,
      bool isOther,
      string otherDescription,
      DateTime changesReceivedDate,
      DateTime revisedDueDate)
    {
      this.IsDisclosedByBroker = isDisclosedByBroker;
      this.IsChangedCircumstanceSettlementCharges = isChangedCircumstanceSettlementCharges;
      this.IsChangedCircumstanceEligibility = isChangedCircumstanceEligibility;
      this.IsRevisionsRequestedByConsumer = isRevisionsRequestedByConsumer;
      this.IsInterestRateDependentCharges = isInterestRateDependentCharges;
      this.IsExpiration = isExpiration;
      this.IsDelayedSettlementOnConstructionLoans = isDelayedSettlementOnConstructionLoans;
      this.IsOther = isOther;
      this.OtherDescription = otherDescription;
      this.ChangesReceivedDate = changesReceivedDate;
      this.RevisedDueDate = revisedDueDate;
    }

    public DateTime RevisedDueDate { get; set; }
  }
}
