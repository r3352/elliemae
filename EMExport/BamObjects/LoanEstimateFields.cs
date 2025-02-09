// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Export.BamObjects.LoanEstimateFields
// Assembly: EMExport, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D06A4C35-7634-4F74-B132-8DD78784C14A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMExport.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Export.BamObjects
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
