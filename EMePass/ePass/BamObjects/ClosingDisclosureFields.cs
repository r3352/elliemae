// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.BamObjects.ClosingDisclosureFields
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ePass.BamObjects
{
  public class ClosingDisclosureFields
  {
    public bool IsChangeInAPR { get; set; }

    public bool IsChangeInLoanProduct { get; set; }

    public bool IsPrepaymentPenaltyAdded { get; set; }

    public bool IsChangeInSettlementCharges { get; set; }

    public bool Is24HourAdvancePreview { get; set; }

    public bool IsToleranceCure { get; set; }

    public bool IsClericalErrorCorrection { get; set; }

    public bool IsChangedCircumstanceEligibility { get; set; }

    public bool IsInterestRateDependentCharges { get; set; }

    public bool IsRevisionsRequestedByConsumer { get; set; }

    public bool IsOther { get; set; }

    public string OtherDescription { get; set; }

    public DateTime ChangesReceivedDate { get; set; }

    public ClosingDisclosureFields(
      bool isChangeInAPR,
      bool isChangeInLoanProduct,
      bool isPrepaymentPenaltyAdded,
      bool isChangeInSettlementCharges,
      bool is24HourAdvancePreview,
      bool isToleranceCure,
      bool isClericalErrorCorrection,
      bool isChangedCircumstanceEligibility,
      bool isInterestRateDependentCharges,
      bool isRevisionsRequestedByConsumer,
      bool isOther,
      string otherDescription,
      DateTime changesReceivedDate,
      DateTime revisedDueDate)
    {
      this.IsChangeInAPR = isChangeInAPR;
      this.IsChangeInLoanProduct = isChangeInLoanProduct;
      this.IsPrepaymentPenaltyAdded = isPrepaymentPenaltyAdded;
      this.IsChangeInSettlementCharges = isChangeInSettlementCharges;
      this.Is24HourAdvancePreview = is24HourAdvancePreview;
      this.IsToleranceCure = isToleranceCure;
      this.IsClericalErrorCorrection = isClericalErrorCorrection;
      this.IsChangedCircumstanceEligibility = isChangedCircumstanceEligibility;
      this.IsInterestRateDependentCharges = isInterestRateDependentCharges;
      this.IsRevisionsRequestedByConsumer = isRevisionsRequestedByConsumer;
      this.IsOther = isOther;
      this.OtherDescription = otherDescription;
      this.ChangesReceivedDate = changesReceivedDate;
      this.RevisedDueDate = revisedDueDate;
    }

    public DateTime RevisedDueDate { get; set; }
  }
}
