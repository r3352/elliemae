// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MbsPoolTradeAssignment
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.TradeSynchronization;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Script.Serialization;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class MbsPoolTradeAssignment : TradeAssignment
  {
    private TradePriceAdjustments priceAdjustments;
    private Investor investor;
    private TradePricingInfo pricing;
    private ContactInformation assignee;

    [ScriptIgnore]
    public MbsPoolInfo MbsPoolTradeInfo { get; set; }

    public string Guid { get; set; }

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

    public MbsPoolMortgageType PoolMortgageType { get; set; }

    public string PoolTaxID { get; set; }

    public string GinniePoolType { get; set; }

    public DateTime PurchaseDate { get; set; }

    public Decimal RateAdjustment { get; set; }

    public string ReoMarketingParty { get; set; }

    public int ScheduledRemittancePaymentDay { get; set; }

    public Decimal SecurityIssueDateIntRate { get; set; }

    public DateTime SecurityTradeBookEntryDate { get; set; }

    public bool IsSent1711ToCustodian { get; set; }

    public string Servicer { get; set; }

    public string ServicerID { get; set; }

    public string SettlementMonth { get; set; }

    public string StandardLookback { get; set; }

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

    public long PoolCurrentPrincipalBalAmt { get; set; }

    public DateTime PoolInterestAdjustmentEffectiveDate { get; set; }

    public string PoolIssuerTransferee { get; set; }

    public int PoolMaturityPeriodCount { get; set; }

    public string PoolingMethodType { get; set; }

    public long SecurityOrigSubscriptionAmt { get; set; }

    public string SellerId { get; set; }

    public ServicingType ServicingType { get; set; }

    public bool IsWeightedAvgPriceLocked { get; set; }

    public Decimal MinServicingFee { get; set; }

    public Decimal MaxBU { get; set; }

    public string ContractNum { get; set; }

    public string AssignedSecurityID { get; set; }

    public TradePriceAdjustments PriceAdjustments
    {
      get => this.priceAdjustments;
      set => this.priceAdjustments = value;
    }

    public Investor Investor
    {
      get => this.investor;
      set => this.investor = value;
    }

    public TradePricingInfo Pricing
    {
      get => this.pricing;
      set => this.pricing = value;
    }

    public ContactInformation Assignee
    {
      get => this.assignee;
      set => this.assignee = value;
    }

    public MbsPoolTradeAssignment()
    {
    }

    public MbsPoolTradeAssignment(MbsPoolInfo tradeInfo, List<string> skipFieldList)
      : base(tradeInfo.TradeID, TradeType.MbsPool, skipFieldList)
    {
      this.ConvertFromTradeInfoToTradeAssignment(tradeInfo);
      this.SkipFieldList = skipFieldList;
    }

    public MbsPoolTradeAssignment(string jsonString)
    {
      this.ConvertFromTradeAssignmentToTradeInfo(jsonString);
    }

    private void ConvertFromTradeInfoToTradeAssignment(MbsPoolInfo info)
    {
      this.MbsPoolTradeInfo = info;
      this.TradeID = info.TradeID;
      this.TradeType = info.TradeType;
      this.Guid = info.Guid;
      this.HasPendingLoan = info.PendingLoanList.Count > 0;
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
      this.CompletionPercent = info.CompletionPercent;
      this.OpenAmount = info.OpenAmount;
      this.ContractNumber = info.ContractNumber;
      this.AccrualRateStructureType = info.AccrualRateStructureType;
      this.ActualDeliveryDate = info.ShipmentDate;
      this.AmortizationType = info.AmortizationType;
      this.ARMIndex = info.ARMIndex;
      this.IsAssumability = info.IsAssumability;
      this.IsBalloon = info.IsBalloon;
      this.BaseGuarantyFee = info.BaseGuarantyFee;
      this.IsBondFinancePool = info.IsBondFinancePool;
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
      this.GFeeAfterAltPaymentMethod = info.GFeeAfterAltPaymentMethod;
      this.GuaranteeFee = info.GuaranteeFee;
      this.InitialDate = info.InitialDate;
      this.IntPaymentAdjIndexLeadDays = info.IntPaymentAdjIndexLeadDays;
      this.IntRateRoundingPercent = info.IntRateRoundingPercent;
      this.IsInterestOnly = info.IsInterestOnly;
      this.InterestOnlyEndDate = info.InterestOnlyEndDate;
      this.InvestorDeliveryDate = info.InvestorDeliveryDate;
      this.InvestorProductPlanID = info.InvestorProductPlanID;
      this.InvestorRemittanceDay = info.InvestorRemittanceDay;
      this.InvestorRemittanceType = info.InvestorRemittanceType;
      this.IssueDate = info.IssueDate;
      this.IssueType = info.IssueType;
      this.IssuerNum = info.IssuerNum;
      this.LastPaidInstallmentDate = info.LastPaidInstallmentDate;
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
      this.GinniePoolType = info.GinniePoolType;
      this.PurchaseDate = info.PurchaseDate;
      this.RateAdjustment = info.RateAdjustment;
      this.ReoMarketingParty = info.ReoMarketingParty;
      this.ScheduledRemittancePaymentDay = info.ScheduledRemittancePaymentDay;
      this.SecurityIssueDateIntRate = info.SecurityIssueDateIntRate;
      this.SecurityTradeBookEntryDate = info.SecurityTradeBookEntryDate;
      this.Servicer = info.Servicer;
      this.ServicerID = info.ServicerID;
      this.SettlementMonth = info.SettlementMonth;
      this.StandardLookback = info.StandardLookback;
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
      this.IsDocumentRequired = info.DocReqIndicator;
      this.IsDocumentSubmission = info.DocSubmissionIndicator;
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
      this.MinServicingFee = info.MinServicingFee;
      this.MaxBU = info.MaxBU;
      this.IsGinniePoolConcurrentTransfer = info.GinniePoolConcurrentTransferIndicator;
      this.IntRateRoundingType = info.IntRateRoundingType;
      this.InvestorFeatureID = info.InvestorFeatureID;
      this.ContractNum = info.ContractNum;
      this.GinniePoolIndexType = info.GinniePoolIndexType;
      this.priceAdjustments = info.PriceAdjustments;
      this.Investor = info.Investor;
      this.Assignee = info.Assignee;
      this.Pricing = info.Pricing;
    }

    private void ConvertFromTradeAssignmentToTradeInfo(string jsonString)
    {
      object obj1 = JsonConvert.DeserializeObject<object>(jsonString);
      this.MbsPoolTradeInfo = new MbsPoolInfo();
      MbsPoolInfo mbsPoolTradeInfo1 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target1 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p1 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TradeID", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__0.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__0, obj1);
      int num1 = target1((CallSite) p1, obj2);
      mbsPoolTradeInfo1.TradeID = num1;
      MbsPoolInfo mbsPoolTradeInfo2 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, TradeType>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (TradeType), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, TradeType> target2 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, TradeType>> p3 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TradeType", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__2.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__2, obj1);
      int num2 = (int) target2((CallSite) p3, obj3);
      mbsPoolTradeInfo2.TradeType = (TradeType) num2;
      MbsPoolInfo mbsPoolTradeInfo3 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target3 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p5 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Guid", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__4.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__4, obj1);
      string str1 = target3((CallSite) p5, obj4);
      mbsPoolTradeInfo3.Guid = str1;
      MbsPoolInfo mbsPoolTradeInfo4 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target4 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p7 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__7;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Locked", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__6.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__6, obj1);
      int num3 = target4((CallSite) p7, obj5) ? 1 : 0;
      mbsPoolTradeInfo4.Locked = num3 != 0;
      MbsPoolInfo mbsPoolTradeInfo5 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target5 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__9.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p9 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__9;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PoolID", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__8.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__8, obj1);
      string str2 = target5((CallSite) p9, obj6);
      mbsPoolTradeInfo5.Name = str2;
      MbsPoolInfo mbsPoolTradeInfo6 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target6 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p11 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__11;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PoolNumber", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj7 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__10.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__10, obj1);
      string str3 = target6((CallSite) p11, obj7);
      mbsPoolTradeInfo6.PoolNumber = str3;
      MbsPoolInfo mbsPoolTradeInfo7 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target7 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__13.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p13 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__13;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TradeDescription", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj8 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__12.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__12, obj1);
      string str4 = target7((CallSite) p13, obj8);
      mbsPoolTradeInfo7.TradeDescription = str4;
      MbsPoolInfo mbsPoolTradeInfo8 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__15 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target8 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__15.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p15 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__15;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__14 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "InvestorName", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj9 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__14.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__14, obj1);
      string str5 = target8((CallSite) p15, obj9);
      mbsPoolTradeInfo8.InvestorName = str5;
      MbsPoolInfo mbsPoolTradeInfo9 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__19 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target9 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__19.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p19 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__19;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__18 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target10 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__18.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p18 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__18;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__17 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__17 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target11 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__17.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p17 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__17;
      Type type1 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__16 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TargetDeliveryDate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj10 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__16.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__16, obj1);
      object obj11 = target11((CallSite) p17, type1, obj10);
      object obj12 = target10((CallSite) p18, obj11);
      DateTime dateTime1 = target9((CallSite) p19, obj12);
      mbsPoolTradeInfo9.TargetDeliveryDate = dateTime1;
      MbsPoolInfo mbsPoolTradeInfo10 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__21 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target12 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__21.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p21 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__21;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__20 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PoolAmount", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj13 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__20.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__20, obj1);
      Decimal num4 = target12((CallSite) p21, obj13);
      mbsPoolTradeInfo10.TradeAmount = num4;
      MbsPoolInfo mbsPoolTradeInfo11 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__23 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target13 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__23.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p23 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__23;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__22 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Coupon", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj14 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__22.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__22, obj1);
      Decimal num5 = target13((CallSite) p23, obj14);
      mbsPoolTradeInfo11.Coupon = num5;
      MbsPoolInfo mbsPoolTradeInfo12 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__27 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__27 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target14 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__27.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p27 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__27;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__26 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target15 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__26.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p26 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__26;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__25 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__25 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target16 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__25.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p25 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__25;
      Type type2 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__24 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SettlementDate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj15 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__24.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__24, obj1);
      object obj16 = target16((CallSite) p25, type2, obj15);
      object obj17 = target15((CallSite) p26, obj16);
      DateTime dateTime2 = target14((CallSite) p27, obj17);
      mbsPoolTradeInfo12.SettlementDate = dateTime2;
      MbsPoolInfo mbsPoolTradeInfo13 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__31 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__31 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target17 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__31.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p31 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__31;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__30 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target18 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__30.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p30 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__30;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__29 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__29 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target19 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__29.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p29 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__29;
      Type type3 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__28 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__28 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "NotificationDate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj18 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__28.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__28, obj1);
      object obj19 = target19((CallSite) p29, type3, obj18);
      object obj20 = target18((CallSite) p30, obj19);
      DateTime dateTime3 = target17((CallSite) p31, obj20);
      mbsPoolTradeInfo13.NotificationDate = dateTime3;
      MbsPoolInfo mbsPoolTradeInfo14 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__35 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__35 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target20 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__35.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p35 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__35;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__34 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__34 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target21 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__34.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p34 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__34;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__33 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__33 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target22 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__33.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p33 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__33;
      Type type4 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__32 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CommitmentDate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj21 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__32.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__32, obj1);
      object obj22 = target22((CallSite) p33, type4, obj21);
      object obj23 = target21((CallSite) p34, obj22);
      DateTime dateTime4 = target20((CallSite) p35, obj23);
      mbsPoolTradeInfo14.CommitmentDate = dateTime4;
      MbsPoolInfo mbsPoolTradeInfo15 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__37 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__37 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target23 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__37.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p37 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__37;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__36 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__36 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CompletionPercent", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj24 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__36.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__36, obj1);
      Decimal num6 = target23((CallSite) p37, obj24);
      mbsPoolTradeInfo15.CompletionPercent = num6;
      MbsPoolInfo mbsPoolTradeInfo16 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__39 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__39 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target24 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__39.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p39 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__39;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__38 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__38 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "OpenAmount", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj25 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__38.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__38, obj1);
      Decimal num7 = target24((CallSite) p39, obj25);
      mbsPoolTradeInfo16.OpenAmount = num7;
      MbsPoolInfo mbsPoolTradeInfo17 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__41 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__41 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target25 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__41.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p41 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__41;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__40 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__40 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ContractNumber", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj26 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__40.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__40, obj1);
      string str6 = target25((CallSite) p41, obj26);
      mbsPoolTradeInfo17.ContractNumber = str6;
      MbsPoolInfo mbsPoolTradeInfo18 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__43 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__43 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target26 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__43.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p43 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__43;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__42 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__42 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "AccrualRateStructureType", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj27 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__42.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__42, obj1);
      string str7 = target26((CallSite) p43, obj27);
      mbsPoolTradeInfo18.AccrualRateStructureType = str7;
      MbsPoolInfo mbsPoolTradeInfo19 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__47 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__47 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target27 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__47.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p47 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__47;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__46 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__46 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target28 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__46.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p46 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__46;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__45 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__45 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target29 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__45.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p45 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__45;
      Type type5 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__44 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__44 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ActualDeliveryDate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj28 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__44.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__44, obj1);
      object obj29 = target29((CallSite) p45, type5, obj28);
      object obj30 = target28((CallSite) p46, obj29);
      DateTime dateTime5 = target27((CallSite) p47, obj30);
      mbsPoolTradeInfo19.ShipmentDate = dateTime5;
      MbsPoolInfo mbsPoolTradeInfo20 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__49 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__49 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target30 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__49.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p49 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__49;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__48 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__48 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "AmortizationType", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj31 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__48.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__48, obj1);
      string str8 = target30((CallSite) p49, obj31);
      mbsPoolTradeInfo20.AmortizationType = str8;
      MbsPoolInfo mbsPoolTradeInfo21 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__51 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__51 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target31 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__51.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p51 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__51;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__50 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__50 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ARMIndex", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj32 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__50.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__50, obj1);
      Decimal num8 = target31((CallSite) p51, obj32);
      mbsPoolTradeInfo21.ARMIndex = num8;
      MbsPoolInfo mbsPoolTradeInfo22 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__53 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__53 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target32 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__53.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p53 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__53;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__52 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__52 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "IsAssumability", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj33 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__52.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__52, obj1);
      int num9 = target32((CallSite) p53, obj33) ? 1 : 0;
      mbsPoolTradeInfo22.IsAssumability = num9 != 0;
      MbsPoolInfo mbsPoolTradeInfo23 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__55 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__55 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target33 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__55.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p55 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__55;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__54 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__54 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "IsBalloon", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj34 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__54.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__54, obj1);
      int num10 = target33((CallSite) p55, obj34) ? 1 : 0;
      mbsPoolTradeInfo23.IsBalloon = num10 != 0;
      MbsPoolInfo mbsPoolTradeInfo24 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__57 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__57 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target34 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__57.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p57 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__57;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__56 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__56 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BaseGuarantyFee", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj35 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__56.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__56, obj1);
      Decimal num11 = target34((CallSite) p57, obj35);
      mbsPoolTradeInfo24.BaseGuarantyFee = num11;
      MbsPoolInfo mbsPoolTradeInfo25 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__59 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__59 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target35 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__59.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p59 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__59;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__58 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__58 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "IsBondFinancePool", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj36 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__58.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__58, obj1);
      int num12 = target35((CallSite) p59, obj36) ? 1 : 0;
      mbsPoolTradeInfo25.IsBondFinancePool = num12 != 0;
      MbsPoolInfo mbsPoolTradeInfo26 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__63 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__63 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target36 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__63.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p63 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__63;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__62 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__62 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target37 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__62.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p62 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__62;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__61 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__61 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target38 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__61.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p61 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__61;
      Type type6 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__60 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__60 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ChangeDate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj37 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__60.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__60, obj1);
      object obj38 = target38((CallSite) p61, type6, obj37);
      object obj39 = target37((CallSite) p62, obj38);
      DateTime dateTime6 = target36((CallSite) p63, obj39);
      mbsPoolTradeInfo26.ChangeDate = dateTime6;
      MbsPoolInfo mbsPoolTradeInfo27 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__65 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__65 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target39 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__65.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p65 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__65;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__64 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__64 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CommitmentPeriod", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj40 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__64.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__64, obj1);
      string str9 = target39((CallSite) p65, obj40);
      mbsPoolTradeInfo27.CommitmentPeriod = str9;
      MbsPoolInfo mbsPoolTradeInfo28 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__67 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__67 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target40 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__67.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p67 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__67;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__66 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__66 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CommitmentType", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj41 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__66.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__66, obj1);
      string str10 = target40((CallSite) p67, obj41);
      mbsPoolTradeInfo28.CommitmentType = str10;
      MbsPoolInfo mbsPoolTradeInfo29 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__69 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__69 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target41 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__69.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p69 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__69;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__68 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__68 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ContractType", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj42 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__68.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__68, obj1);
      string str11 = target41((CallSite) p69, obj42);
      mbsPoolTradeInfo29.ContractType = str11;
      MbsPoolInfo mbsPoolTradeInfo30 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__71 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__71 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target42 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__71.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p71 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__71;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__70 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__70 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CUSIP", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj43 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__70.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__70, obj1);
      string str12 = target42((CallSite) p71, obj43);
      mbsPoolTradeInfo30.CUSIP = str12;
      MbsPoolInfo mbsPoolTradeInfo31 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__73 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__73 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target43 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__73.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p73 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__73;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__72 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__72 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "DeliveryRegion", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj44 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__72.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__72, obj1);
      string str13 = target43((CallSite) p73, obj44);
      mbsPoolTradeInfo31.DeliveryRegion = str13;
      MbsPoolInfo mbsPoolTradeInfo32 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__75 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__75 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target44 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__75.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p75 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__75;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__74 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__74 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "DocCustodianID", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj45 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__74.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__74, obj1);
      string str14 = target44((CallSite) p75, obj45);
      mbsPoolTradeInfo32.DocCustodianID = str14;
      MbsPoolInfo mbsPoolTradeInfo33 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__79 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__79 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target45 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__79.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p79 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__79;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__78 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__78 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target46 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__78.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p78 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__78;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__77 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__77 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target47 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__77.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p77 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__77;
      Type type7 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__76 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__76 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "EarlyDeliveryDate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj46 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__76.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__76, obj1);
      object obj47 = target47((CallSite) p77, type7, obj46);
      object obj48 = target46((CallSite) p78, obj47);
      DateTime dateTime7 = target45((CallSite) p79, obj48);
      mbsPoolTradeInfo33.EarlyDeliveryDate = dateTime7;
      MbsPoolInfo mbsPoolTradeInfo34 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__81 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__81 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target48 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__81.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p81 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__81;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__80 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__80 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "FinancialInstNum", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj49 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__80.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__80, obj1);
      string str15 = target48((CallSite) p81, obj49);
      mbsPoolTradeInfo34.FinancialInstNum = str15;
      MbsPoolInfo mbsPoolTradeInfo35 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__83 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__83 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target49 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__83.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p83 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__83;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__82 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__82 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "FixedServicingFeePercent", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj50 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__82.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__82, obj1);
      Decimal num13 = target49((CallSite) p83, obj50);
      mbsPoolTradeInfo35.FixedServicingFeePercent = num13;
      MbsPoolInfo mbsPoolTradeInfo36 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__85 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__85 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target50 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__85.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p85 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__85;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__84 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__84 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ForecloseRiskCode", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj51 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__84.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__84, obj1);
      string str16 = target50((CallSite) p85, obj51);
      mbsPoolTradeInfo36.ForecloseRiskCode = str16;
      MbsPoolInfo mbsPoolTradeInfo37 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__87 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__87 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target51 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__87.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p87 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__87;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__86 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__86 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "GFeeAfterAltPaymentMethod", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj52 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__86.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__86, obj1);
      Decimal num14 = target51((CallSite) p87, obj52);
      mbsPoolTradeInfo37.GFeeAfterAltPaymentMethod = num14;
      MbsPoolInfo mbsPoolTradeInfo38 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__89 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__89 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target52 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__89.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p89 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__89;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__88 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__88 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "GuaranteeFee", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj53 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__88.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__88, obj1);
      Decimal num15 = target52((CallSite) p89, obj53);
      mbsPoolTradeInfo38.GuaranteeFee = num15;
      MbsPoolInfo mbsPoolTradeInfo39 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__93 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__93 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target53 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__93.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p93 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__93;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__92 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__92 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target54 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__92.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p92 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__92;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__91 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__91 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target55 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__91.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p91 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__91;
      Type type8 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__90 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__90 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "InitialDate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj54 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__90.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__90, obj1);
      object obj55 = target55((CallSite) p91, type8, obj54);
      object obj56 = target54((CallSite) p92, obj55);
      DateTime dateTime8 = target53((CallSite) p93, obj56);
      mbsPoolTradeInfo39.InitialDate = dateTime8;
      MbsPoolInfo mbsPoolTradeInfo40 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__95 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__95 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target56 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__95.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p95 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__95;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__94 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__94 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "IntPaymentAdjIndexLeadDays", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj57 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__94.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__94, obj1);
      int num16 = target56((CallSite) p95, obj57);
      mbsPoolTradeInfo40.IntPaymentAdjIndexLeadDays = num16;
      MbsPoolInfo mbsPoolTradeInfo41 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__97 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__97 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target57 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__97.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p97 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__97;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__96 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__96 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "IntRateRoundingPercent", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj58 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__96.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__96, obj1);
      Decimal num17 = target57((CallSite) p97, obj58);
      mbsPoolTradeInfo41.IntRateRoundingPercent = num17;
      MbsPoolInfo mbsPoolTradeInfo42 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__99 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__99 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target58 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__99.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p99 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__99;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__98 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__98 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "IsInterestOnly", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj59 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__98.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__98, obj1);
      int num18 = target58((CallSite) p99, obj59) ? 1 : 0;
      mbsPoolTradeInfo42.IsInterestOnly = num18 != 0;
      MbsPoolInfo mbsPoolTradeInfo43 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__103 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__103 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target59 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__103.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p103 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__103;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__102 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__102 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target60 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__102.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p102 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__102;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__101 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__101 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target61 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__101.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p101 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__101;
      Type type9 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__100 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__100 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "InterestOnlyEndDate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj60 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__100.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__100, obj1);
      object obj61 = target61((CallSite) p101, type9, obj60);
      object obj62 = target60((CallSite) p102, obj61);
      DateTime dateTime9 = target59((CallSite) p103, obj62);
      mbsPoolTradeInfo43.InterestOnlyEndDate = dateTime9;
      MbsPoolInfo mbsPoolTradeInfo44 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__107 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__107 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target62 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__107.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p107 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__107;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__106 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__106 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target63 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__106.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p106 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__106;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__105 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__105 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target64 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__105.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p105 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__105;
      Type type10 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__104 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__104 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "InvestorDeliveryDate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj63 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__104.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__104, obj1);
      object obj64 = target64((CallSite) p105, type10, obj63);
      object obj65 = target63((CallSite) p106, obj64);
      DateTime dateTime10 = target62((CallSite) p107, obj65);
      mbsPoolTradeInfo44.InvestorDeliveryDate = dateTime10;
      MbsPoolInfo mbsPoolTradeInfo45 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__109 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__109 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target65 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__109.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p109 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__109;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__108 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__108 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "InvestorProductPlanID", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj66 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__108.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__108, obj1);
      string str17 = target65((CallSite) p109, obj66);
      mbsPoolTradeInfo45.InvestorProductPlanID = str17;
      MbsPoolInfo mbsPoolTradeInfo46 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__111 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__111 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target66 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__111.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p111 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__111;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__110 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__110 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "InvestorRemittanceDay", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj67 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__110.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__110, obj1);
      int num19 = target66((CallSite) p111, obj67);
      mbsPoolTradeInfo46.InvestorRemittanceDay = num19;
      MbsPoolInfo mbsPoolTradeInfo47 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__113 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__113 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target67 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__113.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p113 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__113;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__112 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__112 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "InvestorRemittanceType", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj68 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__112.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__112, obj1);
      string str18 = target67((CallSite) p113, obj68);
      mbsPoolTradeInfo47.InvestorRemittanceType = str18;
      MbsPoolInfo mbsPoolTradeInfo48 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__117 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__117 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target68 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__117.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p117 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__117;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__116 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__116 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target69 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__116.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p116 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__116;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__115 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__115 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target70 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__115.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p115 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__115;
      Type type11 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__114 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__114 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "IssueDate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj69 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__114.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__114, obj1);
      object obj70 = target70((CallSite) p115, type11, obj69);
      object obj71 = target69((CallSite) p116, obj70);
      DateTime dateTime11 = target68((CallSite) p117, obj71);
      mbsPoolTradeInfo48.IssueDate = dateTime11;
      MbsPoolInfo mbsPoolTradeInfo49 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__119 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__119 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target71 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__119.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p119 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__119;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__118 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__118 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "IssueType", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj72 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__118.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__118, obj1);
      string str19 = target71((CallSite) p119, obj72);
      mbsPoolTradeInfo49.IssueType = str19;
      MbsPoolInfo mbsPoolTradeInfo50 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__121 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__121 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target72 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__121.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p121 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__121;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__120 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__120 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "IssuerNum", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj73 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__120.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__120, obj1);
      string str20 = target72((CallSite) p121, obj73);
      mbsPoolTradeInfo50.IssuerNum = str20;
      MbsPoolInfo mbsPoolTradeInfo51 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__125 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__125 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target73 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__125.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p125 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__125;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__124 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__124 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target74 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__124.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p124 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__124;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__123 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__123 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target75 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__123.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p123 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__123;
      Type type12 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__122 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__122 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastPaidInstallmentDate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj74 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__122.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__122, obj1);
      object obj75 = target75((CallSite) p123, type12, obj74);
      object obj76 = target74((CallSite) p124, obj75);
      DateTime dateTime12 = target73((CallSite) p125, obj76);
      mbsPoolTradeInfo51.LastPaidInstallmentDate = dateTime12;
      MbsPoolInfo mbsPoolTradeInfo52 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__127 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__127 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target76 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__127.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p127 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__127;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__126 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__126 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LoanDefaultLossParty", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj77 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__126.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__126, obj1);
      string str21 = target76((CallSite) p127, obj77);
      mbsPoolTradeInfo52.LoanDefaultLossParty = str21;
      MbsPoolInfo mbsPoolTradeInfo53 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__129 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__129 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target77 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__129.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p129 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__129;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__128 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__128 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "MarginRate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj78 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__128.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__128, obj1);
      Decimal num20 = target77((CallSite) p129, obj78);
      mbsPoolTradeInfo53.MarginRate = num20;
      MbsPoolInfo mbsPoolTradeInfo54 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__131 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__131 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target78 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__131.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p131 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__131;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__130 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__130 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "MasterTnIABA", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj79 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__130.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__130, obj1);
      string str22 = target78((CallSite) p131, obj79);
      mbsPoolTradeInfo54.MasterTnIABA = str22;
      MbsPoolInfo mbsPoolTradeInfo55 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__133 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__133 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target79 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__133.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p133 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__133;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__132 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__132 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "MasterTnIAcctNum", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj80 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__132.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__132, obj1);
      string str23 = target79((CallSite) p133, obj80);
      mbsPoolTradeInfo55.MasterTnIAcctNum = str23;
      MbsPoolInfo mbsPoolTradeInfo56 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__137 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__137 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target80 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__137.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p137 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__137;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__136 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__136 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target81 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__136.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p136 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__136;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__135 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__135 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target82 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__135.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p135 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__135;
      Type type13 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__134 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__134 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "MaturityDate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj81 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__134.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__134, obj1);
      object obj82 = target82((CallSite) p135, type13, obj81);
      object obj83 = target81((CallSite) p136, obj82);
      DateTime dateTime13 = target80((CallSite) p137, obj83);
      mbsPoolTradeInfo56.MaturityDate = dateTime13;
      MbsPoolInfo mbsPoolTradeInfo57 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__139 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__139 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target83 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__139.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p139 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__139;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__138 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__138 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "MaxAccuralRate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj84 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__138.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__138, obj1);
      Decimal num21 = target83((CallSite) p139, obj84);
      mbsPoolTradeInfo57.MaxAccuralRate = num21;
      MbsPoolInfo mbsPoolTradeInfo58 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__141 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__141 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target84 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__141.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p141 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__141;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__140 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__140 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "MbsMargin", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj85 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__140.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__140, obj1);
      Decimal num22 = target84((CallSite) p141, obj85);
      mbsPoolTradeInfo58.MbsMargin = num22;
      MbsPoolInfo mbsPoolTradeInfo59 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__143 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__143 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target85 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__143.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p143 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__143;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__142 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__142 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "MinAccuralRate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj86 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__142.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__142, obj1);
      Decimal num23 = target85((CallSite) p143, obj86);
      mbsPoolTradeInfo59.MinAccuralRate = num23;
      MbsPoolInfo mbsPoolTradeInfo60 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__145 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__145 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target86 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__145.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p145 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__145;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__144 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__144 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "MiscAdjustment", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj87 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__144.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__144, obj1);
      Decimal num24 = target86((CallSite) p145, obj87);
      mbsPoolTradeInfo60.MiscAdjustment = num24;
      MbsPoolInfo mbsPoolTradeInfo61 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__147 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__147 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target87 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__147.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p147 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__147;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__146 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__146 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "MortgageType", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj88 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__146.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__146, obj1);
      string str24 = target87((CallSite) p147, obj88);
      mbsPoolTradeInfo61.MortgageType = str24;
      MbsPoolInfo mbsPoolTradeInfo62 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__149 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__149 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target88 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__149.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p149 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__149;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__148 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__148 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "NoteCustodian", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj89 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__148.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__148, obj1);
      string str25 = target88((CallSite) p149, obj89);
      mbsPoolTradeInfo62.NoteCustodian = str25;
      MbsPoolInfo mbsPoolTradeInfo63 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__151 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__151 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target89 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__151.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p151 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__151;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__150 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__150 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "OwnershipPercent", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj90 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__150.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__150, obj1);
      Decimal num25 = target89((CallSite) p151, obj90);
      mbsPoolTradeInfo63.OwnershipPercent = num25;
      MbsPoolInfo mbsPoolTradeInfo64 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__153 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__153 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target90 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__153.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p153 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__153;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__152 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__152 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PnICustodialABA", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj91 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__152.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__152, obj1);
      string str26 = target90((CallSite) p153, obj91);
      mbsPoolTradeInfo64.PnICustodialABA = str26;
      MbsPoolInfo mbsPoolTradeInfo65 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__155 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__155 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target91 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__155.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p155 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__155;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__154 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__154 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PnICustodialAcctNum", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj92 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__154.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__154, obj1);
      string str27 = target91((CallSite) p155, obj92);
      mbsPoolTradeInfo65.PnICustodialAcctNum = str27;
      MbsPoolInfo mbsPoolTradeInfo66 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__157 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__157 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target92 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__157.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p157 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__157;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__156 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__156 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PassThruRate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj93 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__156.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__156, obj1);
      Decimal num26 = target92((CallSite) p157, obj93);
      mbsPoolTradeInfo66.PassThruRate = num26;
      MbsPoolInfo mbsPoolTradeInfo67 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__159 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__159 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target93 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__159.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p159 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__159;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__158 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__158 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PayeeCode", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj94 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__158.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__158, obj1);
      string str28 = target93((CallSite) p159, obj94);
      mbsPoolTradeInfo67.PayeeCode = str28;
      MbsPoolInfo mbsPoolTradeInfo68 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__161 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__161 = CallSite<Func<CallSite, object, MbsPoolMortgageType>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (MbsPoolMortgageType), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, MbsPoolMortgageType> target94 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__161.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, MbsPoolMortgageType>> p161 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__161;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__160 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__160 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PoolMortgageType", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj95 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__160.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__160, obj1);
      int num27 = (int) target94((CallSite) p161, obj95);
      mbsPoolTradeInfo68.PoolMortgageType = (MbsPoolMortgageType) num27;
      MbsPoolInfo mbsPoolTradeInfo69 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__163 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__163 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target95 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__163.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p163 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__163;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__162 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__162 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PoolTaxID", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj96 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__162.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__162, obj1);
      string str29 = target95((CallSite) p163, obj96);
      mbsPoolTradeInfo69.PoolTaxID = str29;
      MbsPoolInfo mbsPoolTradeInfo70 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__165 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__165 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target96 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__165.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p165 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__165;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__164 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__164 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "GinniePoolType", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj97 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__164.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__164, obj1);
      string str30 = target96((CallSite) p165, obj97);
      mbsPoolTradeInfo70.GinniePoolType = str30;
      MbsPoolInfo mbsPoolTradeInfo71 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__169 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__169 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target97 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__169.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p169 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__169;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__168 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__168 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target98 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__168.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p168 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__168;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__167 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__167 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target99 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__167.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p167 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__167;
      Type type14 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__166 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__166 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PurchaseDate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj98 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__166.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__166, obj1);
      object obj99 = target99((CallSite) p167, type14, obj98);
      object obj100 = target98((CallSite) p168, obj99);
      DateTime dateTime14 = target97((CallSite) p169, obj100);
      mbsPoolTradeInfo71.PurchaseDate = dateTime14;
      MbsPoolInfo mbsPoolTradeInfo72 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__171 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__171 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target100 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__171.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p171 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__171;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__170 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__170 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "RateAdjustment", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj101 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__170.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__170, obj1);
      Decimal num28 = target100((CallSite) p171, obj101);
      mbsPoolTradeInfo72.RateAdjustment = num28;
      MbsPoolInfo mbsPoolTradeInfo73 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__173 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__173 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target101 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__173.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p173 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__173;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__172 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__172 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ReoMarketingParty", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj102 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__172.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__172, obj1);
      string str31 = target101((CallSite) p173, obj102);
      mbsPoolTradeInfo73.ReoMarketingParty = str31;
      MbsPoolInfo mbsPoolTradeInfo74 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__175 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__175 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target102 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__175.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p175 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__175;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__174 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__174 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ScheduledRemittancePaymentDay", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj103 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__174.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__174, obj1);
      int num29 = target102((CallSite) p175, obj103);
      mbsPoolTradeInfo74.ScheduledRemittancePaymentDay = num29;
      MbsPoolInfo mbsPoolTradeInfo75 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__177 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__177 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target103 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__177.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p177 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__177;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__176 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__176 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SecurityIssueDateIntRate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj104 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__176.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__176, obj1);
      Decimal num30 = target103((CallSite) p177, obj104);
      mbsPoolTradeInfo75.SecurityIssueDateIntRate = num30;
      MbsPoolInfo mbsPoolTradeInfo76 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__181 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__181 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target104 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__181.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p181 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__181;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__180 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__180 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target105 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__180.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p180 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__180;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__179 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__179 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target106 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__179.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p179 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__179;
      Type type15 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__178 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__178 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SecurityTradeBookEntryDate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj105 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__178.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__178, obj1);
      object obj106 = target106((CallSite) p179, type15, obj105);
      object obj107 = target105((CallSite) p180, obj106);
      DateTime dateTime15 = target104((CallSite) p181, obj107);
      mbsPoolTradeInfo76.SecurityTradeBookEntryDate = dateTime15;
      MbsPoolInfo mbsPoolTradeInfo77 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__183 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__183 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target107 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__183.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p183 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__183;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__182 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__182 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Servicer", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj108 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__182.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__182, obj1);
      string str32 = target107((CallSite) p183, obj108);
      mbsPoolTradeInfo77.Servicer = str32;
      MbsPoolInfo mbsPoolTradeInfo78 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__185 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__185 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target108 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__185.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p185 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__185;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__184 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__184 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ServicerID", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj109 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__184.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__184, obj1);
      string str33 = target108((CallSite) p185, obj109);
      mbsPoolTradeInfo78.ServicerID = str33;
      MbsPoolInfo mbsPoolTradeInfo79 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__187 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__187 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target109 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__187.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p187 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__187;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__186 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__186 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SettlementMonth", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj110 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__186.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__186, obj1);
      string str34 = target109((CallSite) p187, obj110);
      mbsPoolTradeInfo79.SettlementMonth = str34;
      MbsPoolInfo mbsPoolTradeInfo80 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__189 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__189 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target110 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__189.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p189 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__189;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__188 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__188 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "StandardLookback", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj111 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__188.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__188, obj1);
      string str35 = target110((CallSite) p189, obj111);
      mbsPoolTradeInfo80.StandardLookback = str35;
      MbsPoolInfo mbsPoolTradeInfo81 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__191 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__191 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target111 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__191.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p191 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__191;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__190 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__190 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "StructureType", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj112 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__190.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__190, obj1);
      string str36 = target111((CallSite) p191, obj112);
      mbsPoolTradeInfo81.StructureType = str36;
      MbsPoolInfo mbsPoolTradeInfo82 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__193 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__193 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target112 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__193.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p193 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__193;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__192 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__192 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SubmissionType", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj113 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__192.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__192, obj1);
      string str37 = target112((CallSite) p193, obj113);
      mbsPoolTradeInfo82.SubmissionType = str37;
      MbsPoolInfo mbsPoolTradeInfo83 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__195 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__195 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target113 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__195.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p195 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__195;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__194 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__194 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SubscriberRecordABA", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj114 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__194.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__194, obj1);
      string str38 = target113((CallSite) p195, obj114);
      mbsPoolTradeInfo83.SubscriberRecordABA = str38;
      MbsPoolInfo mbsPoolTradeInfo84 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__197 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__197 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target114 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__197.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p197 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__197;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__196 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__196 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SubscriberRecordAcctNum", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj115 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__196.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__196, obj1);
      string str39 = target114((CallSite) p197, obj115);
      mbsPoolTradeInfo84.SubscriberRecordAcctNum = str39;
      MbsPoolInfo mbsPoolTradeInfo85 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__199 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__199 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target115 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__199.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p199 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__199;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__198 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__198 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SubscriberRecordFRBAcctDesc", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj116 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__198.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__198, obj1);
      string str40 = target115((CallSite) p199, obj116);
      mbsPoolTradeInfo85.SubscriberRecordFRBAcctDesc = str40;
      MbsPoolInfo mbsPoolTradeInfo86 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__201 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__201 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target116 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__201.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p201 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__201;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__200 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__200 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SubscriberRecordFRBPosDesc", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj117 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__200.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__200, obj1);
      string str41 = target116((CallSite) p201, obj117);
      mbsPoolTradeInfo86.SubscriberRecordFRBPosDesc = str41;
      MbsPoolInfo mbsPoolTradeInfo87 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__203 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__203 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target117 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__203.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p203 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__203;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__202 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__202 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SubservicerIssuerNum", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj118 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__202.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__202, obj1);
      string str42 = target117((CallSite) p203, obj118);
      mbsPoolTradeInfo87.SubservicerIssuerNum = str42;
      MbsPoolInfo mbsPoolTradeInfo88 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__205 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__205 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target118 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__205.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p205 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__205;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__204 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__204 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SuffixID", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj119 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__204.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__204, obj1);
      string str43 = target118((CallSite) p205, obj119);
      mbsPoolTradeInfo88.SuffixID = str43;
      MbsPoolInfo mbsPoolTradeInfo89 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__207 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__207 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target119 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__207.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p207 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__207;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__206 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__206 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TBAOpenAmount", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj120 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__206.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__206, obj1);
      Decimal num31 = target119((CallSite) p207, obj120);
      mbsPoolTradeInfo89.TBAOpenAmount = num31;
      MbsPoolInfo mbsPoolTradeInfo90 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__209 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__209 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target120 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__209.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p209 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__209;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__208 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__208 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Term", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj121 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__208.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__208, obj1);
      int num32 = target120((CallSite) p209, obj121);
      mbsPoolTradeInfo90.Term = num32;
      MbsPoolInfo mbsPoolTradeInfo91 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__213 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__213 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target121 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__213.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p213 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__213;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__212 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__212 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target122 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__212.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p212 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__212;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__211 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__211 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target123 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__211.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p211 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__211;
      Type type16 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__210 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__210 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "UnpaidBalanceDate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj122 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__210.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__210, obj1);
      object obj123 = target123((CallSite) p211, type16, obj122);
      object obj124 = target122((CallSite) p212, obj123);
      DateTime dateTime16 = target121((CallSite) p213, obj124);
      mbsPoolTradeInfo91.UnpaidBalanceDate = dateTime16;
      MbsPoolInfo mbsPoolTradeInfo92 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__215 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__215 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target124 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__215.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p215 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__215;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__214 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__214 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "WeightedAvgPrice", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj125 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__214.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__214, obj1);
      Decimal num33 = target124((CallSite) p215, obj125);
      mbsPoolTradeInfo92.WeightedAvgPrice = num33;
      MbsPoolInfo mbsPoolTradeInfo93 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__217 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__217 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target125 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__217.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p217 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__217;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__216 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__216 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ACHABARoutingAndTransitNum", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj126 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__216.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__216, obj1);
      string str44 = target125((CallSite) p217, obj126);
      mbsPoolTradeInfo93.ACHABARoutingAndTransitNum = str44;
      MbsPoolInfo mbsPoolTradeInfo94 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__219 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__219 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target126 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__219.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p219 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__219;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__218 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__218 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ACHABARoutingAndTransitId", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj127 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__218.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__218, obj1);
      string str45 = target126((CallSite) p219, obj127);
      mbsPoolTradeInfo94.ACHABARoutingAndTransitId = str45;
      MbsPoolInfo mbsPoolTradeInfo95 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__221 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__221 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target127 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__221.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p221 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__221;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__220 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__220 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ACHBankAccountDescription", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj128 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__220.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__220, obj1);
      string str46 = target127((CallSite) p221, obj128);
      mbsPoolTradeInfo95.ACHBankAccountDescription = str46;
      MbsPoolInfo mbsPoolTradeInfo96 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__223 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__223 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target128 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__223.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p223 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__223;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__222 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__222 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ACHBankAccountPurposeType", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj129 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__222.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__222, obj1);
      string str47 = target128((CallSite) p223, obj129);
      mbsPoolTradeInfo96.ACHBankAccountPurposeType = str47;
      MbsPoolInfo mbsPoolTradeInfo97 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__225 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__225 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target129 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__225.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p225 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__225;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__224 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__224 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ACHInsitutionTelegraphicName", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj130 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__224.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__224, obj1);
      string str48 = target129((CallSite) p225, obj130);
      mbsPoolTradeInfo97.ACHInsitutionTelegraphicName = str48;
      MbsPoolInfo mbsPoolTradeInfo98 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__227 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__227 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target130 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__227.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p227 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__227;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__226 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__226 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ACHReceiverSubaccountName", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj131 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__226.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__226, obj1);
      string str49 = target130((CallSite) p227, obj131);
      mbsPoolTradeInfo98.ACHReceiverSubaccountName = str49;
      MbsPoolInfo mbsPoolTradeInfo99 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__229 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__229 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target131 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__229.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p229 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__229;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__228 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__228 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BondFinanceProgramName", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj132 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__228.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__228, obj1);
      string str50 = target131((CallSite) p229, obj132);
      mbsPoolTradeInfo99.BondFinanceProgramName = str50;
      MbsPoolInfo mbsPoolTradeInfo100 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__231 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__231 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target132 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__231.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p231 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__231;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__230 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__230 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "BondFinanceProgramType", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj133 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__230.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__230, obj1);
      string str51 = target132((CallSite) p231, obj133);
      mbsPoolTradeInfo100.BondFinanceProgramType = str51;
      MbsPoolInfo mbsPoolTradeInfo101 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__233 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__233 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target133 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__233.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p233 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__233;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__232 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__232 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ContractID", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj134 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__232.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__232, obj1);
      int num34 = target133((CallSite) p233, obj134);
      mbsPoolTradeInfo101.ContractID = num34;
      MbsPoolInfo mbsPoolTradeInfo102 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__235 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__235 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target134 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__235.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p235 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__235;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__234 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__234 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "IsDocumentRequired", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj135 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__234.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__234, obj1);
      int num35 = target134((CallSite) p235, obj135) ? 1 : 0;
      mbsPoolTradeInfo102.DocReqIndicator = num35 != 0;
      MbsPoolInfo mbsPoolTradeInfo103 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__237 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__237 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target135 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__237.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p237 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__237;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__236 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__236 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "IsDocumentSubmission", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj136 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__236.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__236, obj1);
      int num36 = target135((CallSite) p237, obj136) ? 1 : 0;
      mbsPoolTradeInfo103.DocSubmissionIndicator = num36 != 0;
      MbsPoolInfo mbsPoolTradeInfo104 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__241 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__241 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target136 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__241.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p241 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__241;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__240 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__240 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target137 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__240.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p240 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__240;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__239 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__239 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target138 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__239.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p239 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__239;
      Type type17 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__238 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__238 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PoolCertificatePaymentDate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj137 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__238.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__238, obj1);
      object obj138 = target138((CallSite) p239, type17, obj137);
      object obj139 = target137((CallSite) p240, obj138);
      DateTime dateTime17 = target136((CallSite) p241, obj139);
      mbsPoolTradeInfo104.PoolCertificatePaymentDate = dateTime17;
      MbsPoolInfo mbsPoolTradeInfo105 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__243 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__243 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target139 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__243.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p243 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__243;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__242 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__242 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "GinniePoolClassType", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj140 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__242.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__242, obj1);
      string str52 = target139((CallSite) p243, obj140);
      mbsPoolTradeInfo105.GinniePoolClassType = str52;
      MbsPoolInfo mbsPoolTradeInfo106 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__245 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__245 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target140 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__245.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p245 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__245;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__244 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__244 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PoolCurrentLoanCount", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj141 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__244.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__244, obj1);
      int num37 = target140((CallSite) p245, obj141);
      mbsPoolTradeInfo106.PoolCurrentLoanCount = num37;
      MbsPoolInfo mbsPoolTradeInfo107 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__247 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__247 = CallSite<Func<CallSite, object, long>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (long), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, long> target141 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__247.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, long>> p247 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__247;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__246 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__246 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PoolCurrentPrincipalBalAmt", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj142 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__246.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__246, obj1);
      long num38 = target141((CallSite) p247, obj142);
      mbsPoolTradeInfo107.PoolCurrentPrincipalBalAmt = num38;
      MbsPoolInfo mbsPoolTradeInfo108 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__251 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__251 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target142 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__251.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p251 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__251;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__250 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__250 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target143 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__250.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p250 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__250;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__249 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__249 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDateTime", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target144 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__249.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p249 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__249;
      Type type18 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__248 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__248 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PoolInterestAdjustmentEffectiveDate", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj143 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__248.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__248, obj1);
      object obj144 = target144((CallSite) p249, type18, obj143);
      object obj145 = target143((CallSite) p250, obj144);
      DateTime dateTime18 = target142((CallSite) p251, obj145);
      mbsPoolTradeInfo108.PoolInterestAdjustmentEffectiveDate = dateTime18;
      MbsPoolInfo mbsPoolTradeInfo109 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__253 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__253 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target145 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__253.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p253 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__253;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__252 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__252 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PoolIssuerTransferee", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj146 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__252.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__252, obj1);
      string str53 = target145((CallSite) p253, obj146);
      mbsPoolTradeInfo109.PoolIssuerTransferee = str53;
      MbsPoolInfo mbsPoolTradeInfo110 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__255 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__255 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target146 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__255.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p255 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__255;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__254 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__254 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PoolMaturityPeriodCount", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj147 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__254.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__254, obj1);
      int num39 = target146((CallSite) p255, obj147);
      mbsPoolTradeInfo110.PoolMaturityPeriodCount = num39;
      MbsPoolInfo mbsPoolTradeInfo111 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__257 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__257 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target147 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__257.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p257 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__257;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__256 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__256 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PoolingMethodType", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj148 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__256.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__256, obj1);
      string str54 = target147((CallSite) p257, obj148);
      mbsPoolTradeInfo111.PoolingMethodType = str54;
      MbsPoolInfo mbsPoolTradeInfo112 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__259 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__259 = CallSite<Func<CallSite, object, long>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (long), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, long> target148 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__259.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, long>> p259 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__259;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__258 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__258 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SecurityOrigSubscriptionAmt", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj149 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__258.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__258, obj1);
      long num40 = target148((CallSite) p259, obj149);
      mbsPoolTradeInfo112.SecurityOrigSubscriptionAmt = num40;
      MbsPoolInfo mbsPoolTradeInfo113 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__261 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__261 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target149 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__261.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p261 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__261;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__260 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__260 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SellerId", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj150 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__260.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__260, obj1);
      string str55 = target149((CallSite) p261, obj150);
      mbsPoolTradeInfo113.SellerId = str55;
      MbsPoolInfo mbsPoolTradeInfo114 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__263 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__263 = CallSite<Func<CallSite, object, ServicingType>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (ServicingType), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, ServicingType> target150 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__263.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, ServicingType>> p263 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__263;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__262 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__262 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ServicingType", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj151 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__262.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__262, obj1);
      int num41 = (int) target150((CallSite) p263, obj151);
      mbsPoolTradeInfo114.ServicingType = (ServicingType) num41;
      MbsPoolInfo mbsPoolTradeInfo115 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__265 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__265 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target151 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__265.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p265 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__265;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__264 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__264 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "MinServicingFee", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj152 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__264.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__264, obj1);
      Decimal num42 = target151((CallSite) p265, obj152);
      mbsPoolTradeInfo115.MinServicingFee = num42;
      MbsPoolInfo mbsPoolTradeInfo116 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__267 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__267 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target152 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__267.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p267 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__267;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__266 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__266 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "MaxBU", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj153 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__266.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__266, obj1);
      Decimal num43 = target152((CallSite) p267, obj153);
      mbsPoolTradeInfo116.MaxBU = num43;
      MbsPoolInfo mbsPoolTradeInfo117 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__269 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__269 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target153 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__269.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p269 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__269;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__268 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__268 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "IsGinniePoolConcurrentTransfer", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj154 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__268.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__268, obj1);
      int num44 = target153((CallSite) p269, obj154) ? 1 : 0;
      mbsPoolTradeInfo117.GinniePoolConcurrentTransferIndicator = num44 != 0;
      MbsPoolInfo mbsPoolTradeInfo118 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__271 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__271 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target154 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__271.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p271 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__271;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__270 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__270 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "IntRateRoundingType", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj155 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__270.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__270, obj1);
      string str56 = target154((CallSite) p271, obj155);
      mbsPoolTradeInfo118.IntRateRoundingType = str56;
      MbsPoolInfo mbsPoolTradeInfo119 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__273 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__273 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target155 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__273.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p273 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__273;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__272 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__272 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "InvestorFeatureID", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj156 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__272.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__272, obj1);
      string str57 = target155((CallSite) p273, obj156);
      mbsPoolTradeInfo119.InvestorFeatureID = str57;
      MbsPoolInfo mbsPoolTradeInfo120 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__275 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__275 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target156 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__275.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p275 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__275;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__274 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__274 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ContractNum", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj157 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__274.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__274, obj1);
      string str58 = target156((CallSite) p275, obj157);
      mbsPoolTradeInfo120.ContractNum = str58;
      MbsPoolInfo mbsPoolTradeInfo121 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__277 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__277 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target157 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__277.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p277 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__277;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__276 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__276 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "GinniePoolIndexType", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj158 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__276.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__276, obj1);
      string str59 = target157((CallSite) p277, obj158);
      mbsPoolTradeInfo121.GinniePoolIndexType = str59;
      MbsPoolInfo mbsPoolTradeInfo122 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__280 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__280 = CallSite<Func<CallSite, object, TradePriceAdjustments>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (TradePriceAdjustments), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, TradePriceAdjustments> target158 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__280.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, TradePriceAdjustments>> p280 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__280;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__279 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__279 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "BuildPriceAdjustments", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target159 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__279.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p279 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__279;
      Type type19 = typeof (TradeLoanUpdateUtils);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__278 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__278 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PriceAdjustments", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj159 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__278.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__278, obj1);
      object obj160 = target159((CallSite) p279, type19, obj159);
      TradePriceAdjustments priceAdjustments = target158((CallSite) p280, obj160);
      mbsPoolTradeInfo122.PriceAdjustments = priceAdjustments;
      MbsPoolInfo mbsPoolTradeInfo123 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__283 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__283 = CallSite<Func<CallSite, object, Investor>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Investor), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Investor> target160 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__283.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Investor>> p283 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__283;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__282 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__282 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "BuildInvestors", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target161 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__282.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p282 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__282;
      Type type20 = typeof (TradeLoanUpdateUtils);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__281 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__281 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Investor", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj161 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__281.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__281, obj1);
      object obj162 = target161((CallSite) p282, type20, obj161);
      Investor investor = target160((CallSite) p283, obj162);
      mbsPoolTradeInfo123.Investor = investor;
      MbsPoolInfo mbsPoolTradeInfo124 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__287 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__287 = CallSite<Func<CallSite, object, ContactInformation>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (ContactInformation), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, ContactInformation> target162 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__287.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, ContactInformation>> p287 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__287;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__286 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__286 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", (IEnumerable<Type>) new Type[1]
        {
          typeof (ContactInformation)
        }, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target163 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__286.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p286 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__286;
      Type type21 = typeof (JsonConvert);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__285 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__285 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target164 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__285.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p285 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__285;
      Type type22 = typeof (JsonConvert);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__284 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__284 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Assignee", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj163 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__284.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__284, obj1);
      object obj164 = target164((CallSite) p285, type22, obj163);
      object obj165 = target163((CallSite) p286, type21, obj164);
      ContactInformation contactInformation = target162((CallSite) p287, obj165);
      mbsPoolTradeInfo124.Assignee = contactInformation;
      MbsPoolInfo mbsPoolTradeInfo125 = this.MbsPoolTradeInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__290 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__290 = CallSite<Func<CallSite, object, TradePricingInfo>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (TradePricingInfo), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, TradePricingInfo> target165 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__290.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, TradePricingInfo>> p290 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__290;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__289 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__289 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "BuildPricingInfo", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target166 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__289.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p289 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__289;
      Type type23 = typeof (TradeLoanUpdateUtils);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__288 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__288 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Pricing", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj166 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__288.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__288, obj1);
      object obj167 = target166((CallSite) p289, type23, obj166);
      TradePricingInfo tradePricingInfo = target165((CallSite) p290, obj167);
      mbsPoolTradeInfo125.Pricing = tradePricingInfo;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__293 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__293 = CallSite<Func<CallSite, object, List<string>>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (List<string>), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, List<string>> target167 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__293.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, List<string>>> p293 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__293;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__292 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__292 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "BuildSkipFieldList", (IEnumerable<Type>) null, typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target168 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__292.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p292 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__292;
      Type type24 = typeof (TradeLoanUpdateUtils);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__291 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__291 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SkipFieldList", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj168 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__291.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__291, obj1);
      object obj169 = target168((CallSite) p292, type24, obj168);
      this.SkipFieldList = target167((CallSite) p293, obj169);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__295 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__295 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target169 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__295.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p295 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__295;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__294 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__294 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TradeID", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj170 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__294.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__294, obj1);
      this.TradeID = target169((CallSite) p295, obj170);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__297 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__297 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target170 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__297.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p297 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__297;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__296 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__296 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Guid", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj171 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__296.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__296, obj1);
      this.Guid = target170((CallSite) p297, obj171);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__299 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__299 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target171 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__299.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p299 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__299;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__298 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__298 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TradeDescription", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj172 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__298.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__298, obj1);
      this.TradeDescription = target171((CallSite) p299, obj172);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__301 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__301 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target172 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__301.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p301 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__301;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__300 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__300 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LoanSyncOption", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj173 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__300.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__300, obj1);
      this.LoanSyncOption = target172((CallSite) p301, obj173);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__303 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__303 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target173 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__303.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p303 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__303;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__302 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__302 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "FinalStatus", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj174 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__302.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__302, obj1);
      this.FinalStatus = target173((CallSite) p303, obj174);
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__305 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__305 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (MbsPoolTradeAssignment)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target174 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__305.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p305 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__305;
      // ISSUE: reference to a compiler-generated field
      if (MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__304 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__304 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SessionId", typeof (MbsPoolTradeAssignment), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj175 = MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__304.Target((CallSite) MbsPoolTradeAssignment.\u003C\u003Eo__556.\u003C\u003Ep__304, obj1);
      this.SessionId = target174((CallSite) p305, obj175);
    }

    [CLSCompliant(false)]
    public static string SerializeToJson(CorrespondentTradeInfo info)
    {
      return new JavaScriptSerializer().Serialize((object) info);
    }
  }
}
