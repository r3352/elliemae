// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.MbsPool
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Trading;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  public class MbsPool
  {
    public bool HasPendingLoan { get; set; }

    public bool Locked { get; set; }

    public string PoolID { get; set; }

    public string PoolNumber { get; set; }

    public string TradeDescription { get; set; }

    public string InvestorName { get; set; }

    public DateTime TargetDeliveryDate { get; set; }

    public Decimal PoolAmount { get; set; }

    public Decimal Coupon { get; set; }

    public DateTime SettlementDate { get; set; }

    public DateTime NotificationDate { get; set; }

    public DateTime CommitmentDate { get; set; }

    public Decimal AssignedAmount { get; set; }

    public Decimal CompletionPercent { get; set; }

    public Decimal OpenAmount { get; set; }

    public string ContractNumber { get; set; }

    public string AccrualRateStructureType { get; set; }

    public DateTime ActualDeliveryDate { get; set; }

    public string AmortizationType { get; set; }

    public Decimal ARMIndex { get; set; }

    public DateTime AssignedDate { get; set; }

    public bool IsAssumability { get; set; }

    public bool IsBalloon { get; set; }

    public Decimal BaseGuarantyFee { get; set; }

    public bool IsBondFinancePool { get; set; }

    public string CertificationandAgreement { get; set; }

    public DateTime ChangeDate { get; set; }

    public string CommitmentPeriod { get; set; }

    public string CommitmentType { get; set; }

    public string ContractType { get; set; }

    public string CUSIP { get; set; }

    public string DeliveryRegion { get; set; }

    public string DocCustodianID { get; set; }

    public DateTime EarlyDeliveryDate { get; set; }

    public string FinancialInstNum { get; set; }

    public Decimal FixedServicingFeePercent { get; set; }

    public string ForecloseRiskCode { get; set; }

    public Decimal GainLossAmount { get; set; }

    public Decimal GFeeAfterAltPaymentMethod { get; set; }

    public Decimal GuaranteeFee { get; set; }

    public bool IsGuarantyFeeAddOn { get; set; }

    public DateTime InitialDate { get; set; }

    public int IntPaymentAdjIndexLeadDays { get; set; }

    public Decimal IntRateRoundingPercent { get; set; }

    public bool IsInterestOnly { get; set; }

    public DateTime InterestOnlyEndDate { get; set; }

    public DateTime InvestorDeliveryDate { get; set; }

    public string InvestorProductPlanID { get; set; }

    public int InvestorRemittanceDay { get; set; }

    public string InvestorRemittanceType { get; set; }

    public DateTime IssueDate { get; set; }

    public string IssueType { get; set; }

    public string IssuerNum { get; set; }

    public DateTime LastModified { get; set; }

    public DateTime LastPaidInstallmentDate { get; set; }

    public int LoanCount { get; set; }

    public string LoanDefaultLossParty { get; set; }

    public Decimal MarginRate { get; set; }

    public string MasterTnIABA { get; set; }

    public string MasterTnIAcctNum { get; set; }

    public DateTime MaturityDate { get; set; }

    public Decimal MaxAccuralRate { get; set; }

    public Decimal MbsMargin { get; set; }

    public Decimal MinAccuralRate { get; set; }

    public Decimal MiscAdjustment { get; set; }

    public string MortgageType { get; set; }

    public bool IsMultiFamily { get; set; }

    public string NewTransferIssuerNumber { get; set; }

    public string NoteCustodian { get; set; }

    public Decimal OwnershipPercent { get; set; }

    public string PnICustodialABA { get; set; }

    public string PnICustodialAcctNum { get; set; }

    public Decimal PassThruRate { get; set; }

    public string PayeeCode { get; set; }

    public string PlanNumber { get; set; }

    public string PoolMortgageType { get; set; }

    public string PoolTaxID { get; set; }

    public string PoolType { get; set; }

    public DateTime PurchaseDate { get; set; }

    public Decimal RateAdjustment { get; set; }

    public string REOMarketingParty { get; set; }

    public int ScheduledRemittancePaymentDay { get; set; }

    public Decimal SecurityIssueDateIntRate { get; set; }

    public DateTime SecurityTradeBookEntryDate { get; set; }

    public bool IsSent1711ToCustodian { get; set; }

    public string Servicer { get; set; }

    public string ServicerID { get; set; }

    public string SettlementMonth { get; set; }

    public string StandardLookback { get; set; }

    public TradeStatus Status { get; set; }

    public string StructureType { get; set; }

    public string SubmissionType { get; set; }

    public string SubscriberRecordABA { get; set; }

    public string SubscriberRecordAcctNum { get; set; }

    public string SubscriberRecordFRBAcctDesc { get; set; }

    public string SubscriberRecordFRBPosDesc { get; set; }

    public string SubservicerIssuerNum { get; set; }

    public string SuffixID { get; set; }

    public Decimal TBAOpenAmount { get; set; }

    public int Term { get; set; }

    public DateTime UnpaidBalanceDate { get; set; }

    public Decimal WeightedAvgPrice { get; set; }

    public string ACHABARoutingAndTransitNum { get; set; }

    public string ACHABARoutingAndTransitId { get; set; }

    public string ACHBankAccountDescription { get; set; }

    public string ACHBankAccountPurposeType { get; set; }

    public string ACHInsitutionTelegraphicName { get; set; }

    public string ACHReceiverSubaccountName { get; set; }

    public string BondFinanceProgramName { get; set; }

    public string BondFinanceProgramType { get; set; }

    public int ContractID { get; set; }

    public bool IsDocumentRequired { get; set; }

    public bool IsDocumentSubmission { get; set; }

    public string GinniePoolIndexType { get; set; }

    public string IntRateRoundingType { get; set; }

    public string InvestorFeatureID { get; set; }

    public DateTime PoolCertificatePaymentDate { get; set; }

    public string GinniePoolClassType { get; set; }

    public bool IsGinniePoolConcurrentTransfer { get; set; }

    public int PoolCurrentLoanCount { get; set; }

    public Decimal PoolCurrentPrincipalBalAmt { get; set; }

    public DateTime PoolInterestAdjustmentEffectiveDate { get; set; }

    public string PoolIssuerTransferee { get; set; }

    public int PoolMaturityPeriodCount { get; set; }

    public string PoolingMethodType { get; set; }

    public Decimal SecurityOrigSubscriptionAmt { get; set; }

    public string SellerId { get; set; }

    public int ServicingType { get; set; }

    public bool IsWeightedAvgPriceLocked { get; set; }

    public Decimal MinServicingFee { get; set; }

    public Decimal MaxBU { get; set; }

    public string ContractNum { get; set; }

    public string AssignedSecurityID { get; set; }

    internal MbsPool(MbsPoolViewModel info)
    {
      this.HasPendingLoan = info.PendingLoanCount > 0;
      this.Locked = ((TradeViewModel) info).Locked;
      this.PoolID = ((TradeBase) info).Name;
      this.PoolNumber = info.PoolNumber;
      this.TradeDescription = info.TradeDescription;
      this.InvestorName = ((TradeViewModel) info).InvestorName;
      this.TargetDeliveryDate = ((TradeViewModel) info).TargetDeliveryDate;
      this.PoolAmount = ((TradeViewModel) info).TradeAmount;
      this.Coupon = info.Coupon;
      this.SettlementDate = info.SettlementDate;
      this.NotificationDate = info.NotificationDate;
      this.CommitmentDate = info.CommitmentDate;
      this.AssignedAmount = ((TradeViewModel) info).AssignedAmount;
      this.CompletionPercent = ((TradeViewModel) info).CompletionPercent;
      this.OpenAmount = info.OpenAmount;
      this.ContractNumber = info.ContractNumber;
      this.AccrualRateStructureType = info.AccrualRateStructureType;
      this.ActualDeliveryDate = info.ShipmentDate;
      this.AmortizationType = info.AmortizationType;
      this.ARMIndex = info.ARMIndex;
      this.AssignedDate = info.AssignedStatusDate;
      this.AssignedSecurityID = info.AssignedSecurityID;
      this.IsAssumability = info.IsAssumability == "1";
      this.IsBalloon = info.IsBalloon == "1";
      this.BaseGuarantyFee = info.BaseGuarantyFee;
      this.IsBondFinancePool = info.IsBondFinancePool == "1";
      this.CertificationandAgreement = info.CertAgreement;
      this.ChangeDate = info.ChangeDate;
      this.CommitmentPeriod = info.CommitmentPeriod;
      this.CommitmentType = info.CommitmentType;
      this.ContractType = info.ContractType;
      this.CUSIP = info.CUSIP;
      this.DeliveryRegion = info.DeliveryRegion;
      this.DocCustodianID = info.DocCustodianID;
      this.EarlyDeliveryDate = info.EarlyDeliveryDate;
      this.FinancialInstNum = info.FinancialInstNum;
      this.FixedServicingFeePercent = info.FixedServicingFeePercent;
      this.ForecloseRiskCode = info.ForecloseRiskCode;
      this.GainLossAmount = info.AssignedProfit;
      this.GFeeAfterAltPaymentMethod = info.GFeeAfterAltPaymentMethod;
      this.GuaranteeFee = info.GuaranteeFee;
      this.IsGuarantyFeeAddOn = info.IsGuarantyFeeAddOn == "1";
      this.InitialDate = info.InitialDate;
      this.IntPaymentAdjIndexLeadDays = info.IntPaymentAdjIndexLeadDays;
      this.IntRateRoundingPercent = info.IntRateRoundingPercent;
      this.IsInterestOnly = info.IsInterestOnly == "1";
      this.InterestOnlyEndDate = info.InterestOnlyEndDate;
      this.InvestorDeliveryDate = info.InvestorDeliveryDate;
      this.InvestorProductPlanID = info.InvestorProductPlanID;
      this.InvestorRemittanceDay = info.InvestorRemittanceDay;
      this.InvestorRemittanceType = info.InvestorRemittanceType;
      this.IssueDate = info.IssueDate;
      this.IssueType = info.IssueType;
      this.IssuerNum = info.IssuerNum;
      this.LastModified = info.LastModified;
      this.LastPaidInstallmentDate = info.LastPaidInstallmentDate;
      this.LoanCount = info.LoanCount;
      this.LoanDefaultLossParty = info.LoanDefaultLossParty;
      this.MarginRate = info.MarginRate;
      this.MasterTnIABA = info.MasterTnIABA;
      this.MasterTnIAcctNum = info.MasterTnIAcctNum;
      this.MaturityDate = info.MaturityDate;
      this.MaxAccuralRate = info.MaxAccuralRate;
      this.MbsMargin = info.MbsMargin;
      this.MinAccuralRate = info.MinAccuralRate;
      this.MiscAdjustment = info.MiscAdjustment;
      this.MortgageType = info.MortgageType;
      this.IsMultiFamily = info.IsMultiFamily == "1";
      this.NewTransferIssuerNumber = info.NewTransferIssuerNum;
      this.NoteCustodian = info.NoteCustodian;
      this.OwnershipPercent = info.OwnershipPercent;
      this.PnICustodialABA = info.PnICustodialABA;
      this.PnICustodialAcctNum = info.PnICustodialAcctNum;
      this.PassThruRate = info.PassThruRate;
      this.PayeeCode = info.PayeeCode;
      this.PlanNumber = info.PlanNum;
      this.PoolMortgageType = info.PoolMortgageType;
      this.PoolTaxID = info.PoolTaxID;
      this.PoolType = info.GinniePoolType;
      this.PurchaseDate = info.PurchaseDate;
      this.RateAdjustment = info.RateAdjustment;
      this.REOMarketingParty = info.ReoMarketingParty;
      this.ScheduledRemittancePaymentDay = info.ScheduledRemittancePaymentDay;
      this.SecurityIssueDateIntRate = info.SecurityIssueDateIntRate;
      this.SecurityTradeBookEntryDate = info.SecurityTradeBookEntryDate;
      this.IsSent1711ToCustodian = info.IsSent1711ToCustodian == "1";
      this.Servicer = info.Servicer;
      this.ServicerID = info.ServicerID;
      this.SettlementMonth = info.SettlementMonth;
      this.StandardLookback = info.StandardLookback;
      this.Status = (TradeStatus) ((TradeViewModel) info).Status;
      this.StructureType = info.StructureType;
      this.SubmissionType = info.SubmissionType;
      this.SubscriberRecordABA = info.SubscriberRecordABA;
      this.SubscriberRecordAcctNum = info.SubscriberRecordAcctNum;
      this.SubscriberRecordFRBAcctDesc = info.SubscriberRecordFRBAcctDesc;
      this.SubscriberRecordFRBPosDesc = info.SubscriberRecordFRBPosDesc;
      this.SubservicerIssuerNum = info.SubservicerIssuerNum;
      this.SuffixID = info.SuffixID;
      this.TBAOpenAmount = info.TBAOpenAmount;
      this.Term = info.Term;
      this.UnpaidBalanceDate = info.UnpaidBalanceDate;
      this.WeightedAvgPrice = info.WeightedAvgPrice;
      this.ACHABARoutingAndTransitNum = info.ACHABARoutingAndTransitNum;
      this.ACHABARoutingAndTransitId = info.ACHABARoutingAndTransitId;
      this.ACHBankAccountDescription = info.ACHBankAccountDescription;
      this.ACHBankAccountPurposeType = info.ACHBankAccountPurposeType;
      this.ACHInsitutionTelegraphicName = info.ACHInsitutionTelegraphicName;
      this.ACHReceiverSubaccountName = info.ACHReceiverSubaccountName;
      this.BondFinanceProgramName = info.BondFinanceProgramName;
      this.BondFinanceProgramType = info.BondFinanceProgramType;
      this.ContractID = ((TradeViewModel) info).ContractID;
      this.IsDocumentRequired = info.DocReqIndicator == "1";
      this.IsDocumentSubmission = info.DocSubmissionIndicator == "1";
      this.PoolCertificatePaymentDate = info.PoolCertificatePaymentDate;
      this.GinniePoolClassType = info.GinniePoolClassType;
      this.PoolCurrentLoanCount = info.PoolCurrentLoanCount;
      this.PoolCurrentPrincipalBalAmt = info.PoolCurrentPrincipalBalAmt;
      this.PoolInterestAdjustmentEffectiveDate = info.PoolInterestAdjustmentEffectiveDate;
      this.PoolIssuerTransferee = info.PoolIssuerTransferee;
      this.PoolMaturityPeriodCount = info.PoolMaturityPeriodCount;
      this.PoolingMethodType = info.PoolingMethodType;
      this.SecurityOrigSubscriptionAmt = info.SecurityOrigSubscriptionAmt;
      this.SellerId = info.SellerId;
      this.ServicingType = info.ServicingType;
      this.IsWeightedAvgPriceLocked = info.WeightedAvgPriceLocked == "1";
      this.MinServicingFee = info.MinServicingFee;
      this.MaxBU = info.MaxBU;
      this.IsGinniePoolConcurrentTransfer = info.GinniePoolConcurrentTransferIndicator == "1";
      this.IntRateRoundingType = info.IntRateRoundingType;
      this.InvestorFeatureID = info.InvestorFeatureID;
      this.ContractNum = info.ContractNum;
      this.GinniePoolIndexType = info.GinniePoolIndexType;
    }
  }
}
