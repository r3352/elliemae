// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.MbsPool
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Trading;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  /// <summary>Represents Mbs pool entity class.</summary>
  public class MbsPool
  {
    /// <summary>
    /// Gets or sets the flag that if the Mbs pool has pending loans
    /// </summary>
    public bool HasPendingLoan { get; set; }

    /// <summary>Gets or sets a flag whether the Mbs pool is locked</summary>
    public bool Locked { get; set; }

    /// <summary>Gets or sets pool ID</summary>
    public string PoolID { get; set; }

    /// <summary>Gets or sets the pool number of the Mbs pool</summary>
    public string PoolNumber { get; set; }

    /// <summary>Gets or sets Trade Description of the Mbs pool</summary>
    public string TradeDescription { get; set; }

    /// <summary>Gets or sets investor name of the Mbs pool</summary>
    public string InvestorName { get; set; }

    /// <summary>Gets or sets the target delivery date of the Mbs pool</summary>
    public DateTime TargetDeliveryDate { get; set; }

    /// <summary>Gets or sets the pool amount of the Mbs pool</summary>
    public Decimal PoolAmount { get; set; }

    /// <summary>Gets or sets coupon of the Mbs pool</summary>
    public Decimal Coupon { get; set; }

    /// <summary>Gets or sets settlement date of the Mbs pool</summary>
    public DateTime SettlementDate { get; set; }

    /// <summary>Gets or sets notification date of the Mbs pool</summary>
    public DateTime NotificationDate { get; set; }

    /// <summary>Gets or sets comitment date of the Mbs pool</summary>
    public DateTime CommitmentDate { get; set; }

    /// <summary>Gets or sets assigned amount of the Mbs pool</summary>
    public Decimal AssignedAmount { get; set; }

    /// <summary>Gets or sets completion percent of the Mbs pool</summary>
    public Decimal CompletionPercent { get; set; }

    /// <summary>Gets or sets open amount of the Mbs pool</summary>
    public Decimal OpenAmount { get; set; }

    /// <summary>Gets or sets master # of the Mbs pool</summary>
    public string ContractNumber { get; set; }

    /// <summary>
    /// Gets or sets accrual rate structure type of the Mbs pool
    /// </summary>
    public string AccrualRateStructureType { get; set; }

    /// <summary>
    /// Gets or sets actual delivery date type of the Mbs pool
    /// </summary>
    public DateTime ActualDeliveryDate { get; set; }

    /// <summary>Gets or sets amortization type of the Mbs pool</summary>
    public string AmortizationType { get; set; }

    /// <summary>Gets or sets ARM index of the Mbs pool</summary>
    public Decimal ARMIndex { get; set; }

    /// <summary>Gets or sets assigned date of the Mbs pool</summary>
    public DateTime AssignedDate { get; set; }

    /// <summary>Gets or sets if Mbs pool is assumability</summary>
    public bool IsAssumability { get; set; }

    /// <summary>Gets or sets balloon indicator of the Mbs pool</summary>
    public bool IsBalloon { get; set; }

    /// <summary>Gets or sets base guaranty fee of the Mbs pool</summary>
    public Decimal BaseGuarantyFee { get; set; }

    /// <summary>Gets or sets bond finance pool of the Mbs pool</summary>
    public bool IsBondFinancePool { get; set; }

    /// <summary>
    /// Gets or sets certification and agreement of the Mbs pool
    /// </summary>
    public string CertificationandAgreement { get; set; }

    /// <summary>Gets or sets change date of the Mbs pool</summary>
    public DateTime ChangeDate { get; set; }

    /// <summary>Gets or sets commitment period of the Mbs pool</summary>
    public string CommitmentPeriod { get; set; }

    /// <summary>Gets or sets commitment type of the Mbs pool</summary>
    public string CommitmentType { get; set; }

    /// <summary>Gets or sets contract type of the Mbs pool</summary>
    public string ContractType { get; set; }

    /// <summary>Gets or sets CUSIP of the Mbs pool</summary>
    public string CUSIP { get; set; }

    /// <summary>Gets or sets delivery region of the Mbs pool</summary>
    public string DeliveryRegion { get; set; }

    /// <summary>Gets or sets document custodian ID of the Mbs pool</summary>
    public string DocCustodianID { get; set; }

    /// <summary>Gets or sets early delivery date of the Mbs pool</summary>
    public DateTime EarlyDeliveryDate { get; set; }

    /// <summary>Gets or sets financial inst number of the Mbs pool</summary>
    public string FinancialInstNum { get; set; }

    /// <summary>
    /// Gets or sets fixed servicing fee percent of the Mbs pool
    /// </summary>
    public Decimal FixedServicingFeePercent { get; set; }

    /// <summary>Gets or sets foreclose risk code of the Mbs pool</summary>
    public string ForecloseRiskCode { get; set; }

    /// <summary>Gets or sets gain loss amount of the Mbs pool</summary>
    public Decimal GainLossAmount { get; set; }

    /// <summary>
    /// Gets or sets G-fee after alt. payment method of the Mbs pool
    /// </summary>
    public Decimal GFeeAfterAltPaymentMethod { get; set; }

    /// <summary>Gets or sets guarantee fee of the Mbs pool</summary>
    public Decimal GuaranteeFee { get; set; }

    /// <summary>Gets or sets guaranty fee add on of the Mbs pool</summary>
    public bool IsGuarantyFeeAddOn { get; set; }

    /// <summary>Gets or sets Initial Date of the Mbs pool</summary>
    public DateTime InitialDate { get; set; }

    /// <summary>
    /// Gets or sets Int. and payment Adj. index lead days of the Mbs pool
    /// </summary>
    public int IntPaymentAdjIndexLeadDays { get; set; }

    /// <summary>Gets or sets Int. rate rounding % of the Mbs pool</summary>
    public Decimal IntRateRoundingPercent { get; set; }

    /// <summary>Gets or sets if Mbs pool is interest only</summary>
    public bool IsInterestOnly { get; set; }

    /// <summary>Gets or sets interest only end date of the Mbs pool</summary>
    public DateTime InterestOnlyEndDate { get; set; }

    /// <summary>Gets or sets investor delivery date of the Mbs pool</summary>
    public DateTime InvestorDeliveryDate { get; set; }

    /// <summary>Gets or sets investor product plan ID of the Mbs pool</summary>
    public string InvestorProductPlanID { get; set; }

    /// <summary>Gets or sets investor remittance day of the Mbs pool</summary>
    public int InvestorRemittanceDay { get; set; }

    /// <summary>Gets or sets investor remittance type of the Mbs pool</summary>
    public string InvestorRemittanceType { get; set; }

    /// <summary>Gets or sets issue date of the Mbs pool</summary>
    public DateTime IssueDate { get; set; }

    /// <summary>Gets or sets issue type of the Mbs pool</summary>
    public string IssueType { get; set; }

    /// <summary>Gets or sets issuer number of the Mbs pool</summary>
    public string IssuerNum { get; set; }

    /// <summary>Gets or sets last modified of the Mbs pool</summary>
    public DateTime LastModified { get; set; }

    /// <summary>
    /// Gets or sets last paid installment date of the Mbs pool
    /// </summary>
    public DateTime LastPaidInstallmentDate { get; set; }

    /// <summary>Gets or sets loan count of the Mbs pool</summary>
    public int LoanCount { get; set; }

    /// <summary>Gets or sets loan default loss party of the Mbs pool</summary>
    public string LoanDefaultLossParty { get; set; }

    /// <summary>Gets or sets margin rate of the Mbs pool</summary>
    public Decimal MarginRate { get; set; }

    /// <summary>Gets or sets Master T and I ABA # of the Mbs pool</summary>
    public string MasterTnIABA { get; set; }

    /// <summary>Gets or sets Master T and I Account # of the Mbs pool</summary>
    public string MasterTnIAcctNum { get; set; }

    /// <summary>Gets or sets maturity date of the Mbs pool</summary>
    public DateTime MaturityDate { get; set; }

    /// <summary>Gets or sets maximum accrual rate of the Mbs pool</summary>
    public Decimal MaxAccuralRate { get; set; }

    /// <summary>Gets or sets MBS Margin of the Mbs pool</summary>
    public Decimal MbsMargin { get; set; }

    /// <summary>Gets or sets minimum accrual rate of the Mbs pool</summary>
    public Decimal MinAccuralRate { get; set; }

    /// <summary>Gets or sets misc. fee of the Mbs pool</summary>
    public Decimal MiscAdjustment { get; set; }

    /// <summary>Gets or sets mortgage type of the Mbs pool</summary>
    public string MortgageType { get; set; }

    /// <summary>Gets or sets Mbs pool is multi family</summary>
    public bool IsMultiFamily { get; set; }

    /// <summary>
    /// Gets or sets new transfer issuer number of the Mbs pool
    /// </summary>
    public string NewTransferIssuerNumber { get; set; }

    /// <summary>Gets or sets note custodian of the Mbs pool</summary>
    public string NoteCustodian { get; set; }

    /// <summary>Gets or sets ownership percent of the Mbs pool</summary>
    public Decimal OwnershipPercent { get; set; }

    /// <summary>Gets or sets P and I custodial ABA # of the Mbs pool</summary>
    public string PnICustodialABA { get; set; }

    /// <summary>
    /// Gets or sets P and I custodial account # of the Mbs pool
    /// </summary>
    public string PnICustodialAcctNum { get; set; }

    /// <summary>Gets or sets pass thru rate of the Mbs pool</summary>
    public Decimal PassThruRate { get; set; }

    /// <summary>Gets or sets payee code of the Mbs pool</summary>
    public string PayeeCode { get; set; }

    /// <summary>Gets or sets plan number of the Mbs pool</summary>
    public string PlanNumber { get; set; }

    /// <summary>Gets or sets pool mortgage type of the Mbs pool</summary>
    public string PoolMortgageType { get; set; }

    /// <summary>Gets or sets pool tax ID of the Mbs pool</summary>
    public string PoolTaxID { get; set; }

    /// <summary>Gets or sets pool type of the Mbs pool</summary>
    public string PoolType { get; set; }

    /// <summary>Gets or sets purchase date of the Mbs pool</summary>
    public DateTime PurchaseDate { get; set; }

    /// <summary>Gets or sets rate adjustment of the Mbs pool</summary>
    public Decimal RateAdjustment { get; set; }

    /// <summary>Gets or sets REO marketing party of the Mbs pool</summary>
    public string REOMarketingParty { get; set; }

    /// <summary>
    /// Gets or sets scheduled remittance payment day of the Mbs pool
    /// </summary>
    public int ScheduledRemittancePaymentDay { get; set; }

    /// <summary>
    /// Gets or sets security issue date Int. rate of the Mbs pool
    /// </summary>
    public Decimal SecurityIssueDateIntRate { get; set; }

    /// <summary>
    /// Gets or sets security trade book entry date of the Mbs pool
    /// </summary>
    public DateTime SecurityTradeBookEntryDate { get; set; }

    /// <summary>Gets or sets if Mbs pool is Sent 1711 to Custodian</summary>
    public bool IsSent1711ToCustodian { get; set; }

    /// <summary>Gets or sets servicer of the Mbs pool</summary>
    public string Servicer { get; set; }

    /// <summary>Gets or sets servicer ID of the Mbs pool</summary>
    public string ServicerID { get; set; }

    /// <summary>Gets or sets settlement month of the Mbs pool</summary>
    public string SettlementMonth { get; set; }

    /// <summary>Gets or sets standard lookback of the Mbs pool</summary>
    public string StandardLookback { get; set; }

    /// <summary>Gets or sets status of the Mbs pool</summary>
    public TradeStatus Status { get; set; }

    /// <summary>Gets or sets structure type of the Mbs pool</summary>
    public string StructureType { get; set; }

    /// <summary>Gets or sets submission type of the Mbs pool</summary>
    public string SubmissionType { get; set; }

    /// <summary>Gets or sets subscriber record ABA # of the Mbs pool</summary>
    public string SubscriberRecordABA { get; set; }

    /// <summary>
    /// Gets or sets subscriber record account # of the Mbs pool
    /// </summary>
    public string SubscriberRecordAcctNum { get; set; }

    /// <summary>
    /// Gets or sets subscriber record frb account desc of the Mbs pool
    /// </summary>
    public string SubscriberRecordFRBAcctDesc { get; set; }

    /// <summary>
    /// Gets or sets subscriber record FRB position desc of the Mbs pool
    /// </summary>
    public string SubscriberRecordFRBPosDesc { get; set; }

    /// <summary>
    /// Gets or sets subservicer issuer number of the Mbs pool
    /// </summary>
    public string SubservicerIssuerNum { get; set; }

    /// <summary>Gets or sets suffix ID of the Mbs pool</summary>
    public string SuffixID { get; set; }

    /// <summary>Gets or sets TBA open amount of the Mbs pool</summary>
    public Decimal TBAOpenAmount { get; set; }

    /// <summary>Gets or sets Term of the Mbs pool</summary>
    public int Term { get; set; }

    /// <summary>Gets or sets unpaid balance date of the Mbs pool</summary>
    public DateTime UnpaidBalanceDate { get; set; }

    /// <summary>Gets or sets weighted average price of the Mbs pool</summary>
    public Decimal WeightedAvgPrice { get; set; }

    /// <summary>
    /// Gets or sets ACH ABA routing and transit # of the Mbs pool
    /// </summary>
    public string ACHABARoutingAndTransitNum { get; set; }

    /// <summary>
    /// Gets or sets ACH ABA routing and transit identifier of the Mbs pool
    /// </summary>
    public string ACHABARoutingAndTransitId { get; set; }

    /// <summary>
    /// Gets or sets ACH bank account description of the Mbs pool
    /// </summary>
    public string ACHBankAccountDescription { get; set; }

    /// <summary>
    /// Gets or sets ACH bank account purpose type of the Mbs pool
    /// </summary>
    public string ACHBankAccountPurposeType { get; set; }

    /// <summary>
    /// Gets or sets ACH institution telegraphic name of the Mbs pool
    /// </summary>
    public string ACHInsitutionTelegraphicName { get; set; }

    /// <summary>
    /// Gets or sets ACH receiver subaccount name of the Mbs pool
    /// </summary>
    public string ACHReceiverSubaccountName { get; set; }

    /// <summary>
    /// Gets or sets bond finance program name of the Mbs pool
    /// </summary>
    public string BondFinanceProgramName { get; set; }

    /// <summary>
    /// Gets or sets bond finance program type of the Mbs pool
    /// </summary>
    public string BondFinanceProgramType { get; set; }

    /// <summary>Gets or sets Contract ID of the Mbs pool</summary>
    public int ContractID { get; set; }

    /// <summary>
    /// Gets or sets document required indicator of the Mbs pool
    /// </summary>
    public bool IsDocumentRequired { get; set; }

    /// <summary>
    /// Gets or sets document submission indicator of the Mbs pool
    /// </summary>
    public bool IsDocumentSubmission { get; set; }

    /// <summary>Gets or sets index type of the Mbs pool</summary>
    public string GinniePoolIndexType { get; set; }

    /// <summary>Gets or sets Int. rate rounding type of the Mbs pool</summary>
    public string IntRateRoundingType { get; set; }

    /// <summary>Gets or sets investor feature ID of the Mbs pool</summary>
    public string InvestorFeatureID { get; set; }

    /// <summary>
    /// Gets or sets pool certificate payment date of the Mbs pool
    /// </summary>
    public DateTime PoolCertificatePaymentDate { get; set; }

    /// <summary>Gets or sets Ginnie pool class type of the Mbs pool</summary>
    public string GinniePoolClassType { get; set; }

    /// <summary>
    /// Gets or sets Ginnie pool concurrent transfer indicator of the Mbs pool
    /// </summary>
    public bool IsGinniePoolConcurrentTransfer { get; set; }

    /// <summary>Gets or sets pool current loan count of the Mbs pool</summary>
    public int PoolCurrentLoanCount { get; set; }

    /// <summary>Gets or sets pool current loan count of the Mbs pool</summary>
    public Decimal PoolCurrentPrincipalBalAmt { get; set; }

    /// <summary>
    /// Gets or sets pool interest adjustment effective date of the Mbs pool
    /// </summary>
    public DateTime PoolInterestAdjustmentEffectiveDate { get; set; }

    /// <summary>Gets or sets pool issuer transferee of the Mbs pool</summary>
    public string PoolIssuerTransferee { get; set; }

    /// <summary>
    /// Gets or sets pool maturity period count of the Mbs pool
    /// </summary>
    public int PoolMaturityPeriodCount { get; set; }

    /// <summary>Gets or sets pooling method type of the Mbs pool</summary>
    public string PoolingMethodType { get; set; }

    /// <summary>
    /// Gets or sets security original subscription amount of the Mbs pool
    /// </summary>
    public Decimal SecurityOrigSubscriptionAmt { get; set; }

    /// <summary>Gets or sets seller ID of the Mbs pool</summary>
    public string SellerId { get; set; }

    /// <summary>Gets or sets servicing type of the Mbs pool</summary>
    public int ServicingType { get; set; }

    /// <summary>
    /// Gets or sets weighted average price locked of the Mbs pool
    /// </summary>
    public bool IsWeightedAvgPriceLocked { get; set; }

    /// <summary>Gets or sets min. servicing fee of the Mbs pool</summary>
    public Decimal MinServicingFee { get; set; }

    /// <summary>Gets or sets MaxBU of the Mbs pool</summary>
    public Decimal MaxBU { get; set; }

    /// <summary>Gets or sets contract number of the Mbs pool</summary>
    public string ContractNum { get; set; }

    /// <summary>Gets or sets assigned security id of the Mbs pool</summary>
    public string AssignedSecurityID { get; set; }

    internal MbsPool(MbsPoolViewModel info)
    {
      this.HasPendingLoan = info.PendingLoanCount > 0;
      this.Locked = info.Locked;
      this.PoolID = info.Name;
      this.PoolNumber = info.PoolNumber;
      this.TradeDescription = info.TradeDescription;
      this.InvestorName = info.InvestorName;
      this.TargetDeliveryDate = info.TargetDeliveryDate;
      this.PoolAmount = info.TradeAmount;
      this.Coupon = info.Coupon;
      this.SettlementDate = info.SettlementDate;
      this.NotificationDate = info.NotificationDate;
      this.CommitmentDate = info.CommitmentDate;
      this.AssignedAmount = info.AssignedAmount;
      this.CompletionPercent = info.CompletionPercent;
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
      this.Status = (TradeStatus) info.Status;
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
      this.ContractID = info.ContractID;
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
