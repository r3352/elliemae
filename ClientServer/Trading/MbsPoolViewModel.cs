// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MbsPoolViewModel
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class MbsPoolViewModel : TradeViewModel
  {
    private Dictionary<MbsPoolLoanStatus, int> pendingLoanCounts;

    public MbsPoolViewModel(
      int tradeID,
      string guid,
      string name,
      int status,
      bool locked,
      DateTime lastModified,
      int loanCount,
      Dictionary<MbsPoolLoanStatus, int> pendingLoanCounts)
      : base(tradeID, guid, name, 0M)
    {
      this.tradeStatus = status;
      this.Locked = locked;
      this.LastModified = lastModified;
      this.LoanCount = loanCount;
      this.pendingLoanCounts = pendingLoanCounts;
    }

    protected override string dataTableName => "MbsPoolDetails";

    public override int TradeID
    {
      get => (int) this.getField(this.dataTableName + ".TradeID", (object) -1);
      set => this.setField(this.dataTableName + ".TradeID", (object) value);
    }

    public override string Guid
    {
      get => this.getField(this.dataTableName + ".Guid", (object) "") as string;
      set => this.setField(this.dataTableName + ".Guid", (object) value);
    }

    public override string Name
    {
      get => this.getField(this.dataTableName + ".Name", (object) "") as string;
      set => this.setField(this.dataTableName + ".Name", (object) value);
    }

    public override TradeStatus Status
    {
      get
      {
        switch ((TradeStatus) this.getField(this.dataTableName + ".Status", (object) TradeStatus.None))
        {
          case TradeStatus.Archived:
            return TradeStatus.Archived;
          case TradeStatus.Pending:
            return TradeStatus.Pending;
          default:
            if (this.PurchaseDate != DateTime.MinValue)
              return TradeStatus.Purchased;
            if (this.ShipmentDate != DateTime.MinValue)
              return TradeStatus.Shipped;
            return this.CommitmentDate != DateTime.MinValue ? TradeStatus.Committed : TradeStatus.Open;
        }
      }
    }

    public override bool Locked
    {
      get
      {
        return this.Status == TradeStatus.Archived || (bool) this.getField(this.dataTableName + ".Locked", (object) false);
      }
      set => this.setField(this.dataTableName + ".Locked", (object) value);
    }

    public Decimal OpenAmount
    {
      get => (Decimal) this.getField("TradeMbsPoolSummary.LoanOpenAmount", (object) 0M);
      set => this.setField("TradeMbsPoolSummary.LoanOpenAmount", (object) value);
    }

    public Decimal TBAOpenAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".OpenAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".OpenAmount", (object) value);
    }

    public override int ContractID
    {
      get => (int) this.getField(this.dataTableName + ".ContractID", (object) -1);
      set => this.setField(this.dataTableName + ".ContractID", (object) value);
    }

    public override Decimal TradeAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".TradeAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".TradeAmount", (object) value);
    }

    public DateTime CommitmentDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".CommitmentDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".CommitmentDate", (object) value);
    }

    public string TradeDescription
    {
      get => this.getField(this.dataTableName + ".TradeDescription", (object) "") as string;
      set => this.setField(this.dataTableName + ".TradeDescription", (object) value);
    }

    public override string InvestorName
    {
      get => this.getField(this.dataTableName + ".InvestorName", (object) "") as string;
      set => this.setField(this.dataTableName + ".InvestorName", (object) value);
    }

    public DateTime InvestorDeliveryDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".InvestorDeliveryDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".InvestorDeliveryDate", (object) value);
    }

    public DateTime EarlyDeliveryDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".EarlyDeliveryDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".EarlyDeliveryDate", (object) value);
    }

    public override DateTime TargetDeliveryDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".TargetDeliveryDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".TargetDeliveryDate", (object) value);
    }

    public DateTime ShipmentDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".ShipmentDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".ShipmentDate", (object) value);
    }

    public DateTime PurchaseDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".PurchaseDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".PurchaseDate", (object) value);
    }

    public string PoolMortgageType
    {
      get => this.getField(this.dataTableName + ".PoolMortgageType", (object) "") as string;
      set => this.setField(this.dataTableName + ".PoolMortgageType", (object) value);
    }

    public string PoolNumber
    {
      get => this.getField(this.dataTableName + ".PoolNumber", (object) "") as string;
      set => this.setField(this.dataTableName + ".PoolNumber", (object) value);
    }

    public string SuffixID
    {
      get => this.getField(this.dataTableName + ".SuffixID", (object) "") as string;
      set => this.setField(this.dataTableName + ".SuffixID", (object) value);
    }

    public string CUSIP
    {
      get => this.getField(this.dataTableName + ".CUSIP", (object) "") as string;
      set => this.setField(this.dataTableName + ".CUSIP", (object) value);
    }

    public string MortgageType
    {
      get => this.getField(this.dataTableName + ".MortgageType", (object) "") as string;
      set => this.setField(this.dataTableName + ".MortgageType", (object) value);
    }

    public string AmortizationType
    {
      get => this.getField(this.dataTableName + ".AmortizationType", (object) "") as string;
      set => this.setField(this.dataTableName + ".AmortizationType", (object) value);
    }

    public int Term
    {
      get => (int) this.getField(this.dataTableName + ".Term", (object) -1);
      set => this.setField(this.dataTableName + ".Term", (object) value);
    }

    public int ServicingType
    {
      get => (int) this.getField(this.dataTableName + ".ServicingType", (object) -1);
      set => this.setField(this.dataTableName + ".ServicingType", (object) value);
    }

    public string Servicer
    {
      get => this.getField(this.dataTableName + ".Servicer", (object) "") as string;
      set => this.setField(this.dataTableName + ".Servicer", (object) value);
    }

    public Decimal Coupon
    {
      get => (Decimal) this.getField(this.dataTableName + ".Coupon", (object) 0M);
      set => this.setField(this.dataTableName + ".Coupon", (object) value);
    }

    public Decimal WeightedAvgPrice
    {
      get => (Decimal) this.getField(this.dataTableName + ".WeightedAvgPrice", (object) 0M);
      set => this.setField(this.dataTableName + ".WeightedAvgPrice", (object) value);
    }

    public DateTime SettlementDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".SettlementDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".SettlementDate", (object) value);
    }

    public string SettlementMonth
    {
      get => this.getField(this.dataTableName + ".SettlementMonth", (object) "") as string;
      set => this.setField(this.dataTableName + ".SettlementMonth", (object) value);
    }

    public DateTime NotificationDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".NotificationDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".NotificationDate", (object) value);
    }

    public string ContractNumber
    {
      get => this.getField("TradesMasterContract1.ContractNumber", (object) "") as string;
      set => this.setField("TradesMasterContract1.ContractNumber", (object) value);
    }

    public Decimal RateAdjustment
    {
      get => (Decimal) this.getField(this.dataTableName + ".RateAdjustment", (object) 0M);
      set => this.setField(this.dataTableName + ".RateAdjustment", (object) value);
    }

    public Decimal BuyUpAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".BuyUpAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".BuyUpAmount", (object) value);
    }

    public Decimal BuyDownAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".BuyDownAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".BuyDownAmount", (object) value);
    }

    public Decimal MiscAdjustment
    {
      get => (Decimal) this.getField(this.dataTableName + ".MiscAdjustment", (object) 0M);
      set => this.setField(this.dataTableName + ".MiscAdjustment", (object) value);
    }

    public string DocCustodianID
    {
      get => this.getField(this.dataTableName + ".DocCustodianID", (object) "") as string;
      set => this.setField(this.dataTableName + ".DocCustodianID", (object) value);
    }

    public string ServicerID
    {
      get => this.getField(this.dataTableName + ".ServicerID", (object) "") as string;
      set => this.setField(this.dataTableName + ".ServicerID", (object) value);
    }

    public string InvestorProductPlanID
    {
      get => this.getField(this.dataTableName + ".InvestorProductPlanID", (object) "") as string;
      set => this.setField(this.dataTableName + ".InvestorProductPlanID", (object) value);
    }

    public string InvestorFeatureID
    {
      get => this.getField(this.dataTableName + ".InvestorFeatureID", (object) "") as string;
      set => this.setField(this.dataTableName + ".InvestorFeatureID", (object) value);
    }

    public string LoanDefaultLossParty
    {
      get => this.getField(this.dataTableName + ".LoanDefaultLossParty", (object) "") as string;
      set => this.setField(this.dataTableName + ".LoanDefaultLossParty", (object) value);
    }

    public string ReoMarketingParty
    {
      get => this.getField(this.dataTableName + ".ReoMarketingParty", (object) "") as string;
      set => this.setField(this.dataTableName + ".ReoMarketingParty", (object) value);
    }

    public Decimal BaseGuarantyFee
    {
      get => (Decimal) this.getField(this.dataTableName + ".BaseGuarantyFee", (object) 0M);
      set => this.setField(this.dataTableName + ".BaseGuarantyFee", (object) value);
    }

    public Decimal GFeeAfterAltPaymentMethod
    {
      get
      {
        return (Decimal) this.getField(this.dataTableName + ".GFeeAfterAltPaymentMethod", (object) 0M);
      }
      set => this.setField(this.dataTableName + ".GFeeAfterAltPaymentMethod", (object) value);
    }

    public Decimal GuaranteeFee
    {
      get => (Decimal) this.getField(this.dataTableName + ".GuaranteeFee", (object) 0M);
      set => this.setField(this.dataTableName + ".GuaranteeFee", (object) value);
    }

    public int InvestorRemittanceDay
    {
      get => (int) this.getField(this.dataTableName + ".InvestorRemittanceDay", (object) -1);
      set => this.setField(this.dataTableName + ".InvestorRemittanceDay", (object) value);
    }

    public string ContractNum
    {
      get => this.getField(this.dataTableName + ".ContractNum", (object) "") as string;
      set => this.setField(this.dataTableName + ".ContractNum", (object) value);
    }

    public DateTime IssueDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".IssueDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".IssueDate", (object) value);
    }

    public Decimal OwnershipPercent
    {
      get => (Decimal) this.getField(this.dataTableName + ".OwnershipPercent", (object) 0M);
      set => this.setField(this.dataTableName + ".OwnershipPercent", (object) value);
    }

    public string StructureType
    {
      get => this.getField(this.dataTableName + ".StructureType", (object) "") as string;
      set => this.setField(this.dataTableName + ".StructureType", (object) value);
    }

    public string AccrualRateStructureType
    {
      get => this.getField(this.dataTableName + ".AccrualRateStructureType", (object) "") as string;
      set => this.setField(this.dataTableName + ".AccrualRateStructureType", (object) value);
    }

    public Decimal SecurityIssueDateIntRate
    {
      get => (Decimal) this.getField(this.dataTableName + ".SecurityIssueDateIntRate", (object) 0M);
      set => this.setField(this.dataTableName + ".SecurityIssueDateIntRate", (object) value);
    }

    public Decimal MinAccuralRate
    {
      get => (Decimal) this.getField(this.dataTableName + ".MinAccuralRate", (object) 0M);
      set => this.setField(this.dataTableName + ".MinAccuralRate", (object) value);
    }

    public Decimal MaxAccuralRate
    {
      get => (Decimal) this.getField(this.dataTableName + ".MaxAccuralRate", (object) 0M);
      set => this.setField(this.dataTableName + ".MaxAccuralRate", (object) value);
    }

    public Decimal MarginRate
    {
      get => (Decimal) this.getField(this.dataTableName + ".MarginRate", (object) 0M);
      set => this.setField(this.dataTableName + ".MarginRate", (object) value);
    }

    public string IntRateRoundingType
    {
      get => this.getField(this.dataTableName + ".IntRateRoundingType", (object) "") as string;
      set => this.setField(this.dataTableName + ".IntRateRoundingType", (object) value);
    }

    public Decimal IntRateRoundingPercent
    {
      get => (Decimal) this.getField(this.dataTableName + ".IntRateRoundingPercent", (object) 0M);
      set => this.setField(this.dataTableName + ".IntRateRoundingPercent", (object) value);
    }

    public string IsInterestOnly
    {
      get => this.getField(this.dataTableName + ".IsInterestOnly", (object) "") as string;
      set => this.setField(this.dataTableName + ".IsInterestOnly", (object) value);
    }

    public int IntPaymentAdjIndexLeadDays
    {
      get => (int) this.getField(this.dataTableName + ".IntPaymentAdjIndexLeadDays", (object) -1);
      set => this.setField(this.dataTableName + ".IntPaymentAdjIndexLeadDays", (object) value);
    }

    public string IsAssumability
    {
      get => this.getField(this.dataTableName + ".IsAssumability", (object) "") as string;
      set => this.setField(this.dataTableName + ".IsAssumability", (object) value);
    }

    public string IsBalloon
    {
      get => this.getField(this.dataTableName + ".IsBalloon", (object) "") as string;
      set => this.setField(this.dataTableName + ".IsBalloon", (object) value);
    }

    public Decimal FixedServicingFeePercent
    {
      get => (Decimal) this.getField(this.dataTableName + ".FixedServicingFeePercent", (object) 0M);
      set => this.setField(this.dataTableName + ".FixedServicingFeePercent", (object) value);
    }

    public int ScheduledRemittancePaymentDay
    {
      get
      {
        return (int) this.getField(this.dataTableName + ".ScheduledRemittancePaymentDay", (object) -1);
      }
      set => this.setField(this.dataTableName + ".ScheduledRemittancePaymentDay", (object) value);
    }

    public DateTime SecurityTradeBookEntryDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".SecurityTradeBookEntryDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".SecurityTradeBookEntryDate", (object) value);
    }

    public string PayeeCode
    {
      get => this.getField(this.dataTableName + ".PayeeCode", (object) "") as string;
      set => this.setField(this.dataTableName + ".PayeeCode", (object) value);
    }

    public string CommitmentPeriod
    {
      get => this.getField(this.dataTableName + ".CommitmentPeriod", (object) "") as string;
      set => this.setField(this.dataTableName + ".CommitmentPeriod", (object) value);
    }

    public string SubmissionType
    {
      get => this.getField(this.dataTableName + ".SubmissionType", (object) "") as string;
      set => this.setField(this.dataTableName + ".SubmissionType", (object) value);
    }

    public string PlanNum
    {
      get => this.getField(this.dataTableName + ".PlanNum", (object) "") as string;
      set => this.setField(this.dataTableName + ".PlanNum", (object) value);
    }

    public Decimal PassThruRate
    {
      get => (Decimal) this.getField(this.dataTableName + ".PassThruRate", (object) 0M);
      set => this.setField(this.dataTableName + ".PassThruRate", (object) value);
    }

    public string ForecloseRiskCode
    {
      get => this.getField(this.dataTableName + ".ForecloseRiskCode", (object) "") as string;
      set => this.setField(this.dataTableName + ".ForecloseRiskCode", (object) value);
    }

    public Decimal MbsMargin
    {
      get => (Decimal) this.getField(this.dataTableName + ".MbsMargin", (object) 0M);
      set => this.setField(this.dataTableName + ".MbsMargin", (object) value);
    }

    public string ContractType
    {
      get => this.getField(this.dataTableName + ".ContractType", (object) "") as string;
      set => this.setField(this.dataTableName + ".ContractType", (object) value);
    }

    public string DeliveryRegion
    {
      get => this.getField(this.dataTableName + ".DeliveryRegion", (object) "") as string;
      set => this.setField(this.dataTableName + ".DeliveryRegion", (object) value);
    }

    public DateTime InterestOnlyEndDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".InterestOnlyEndDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".InterestOnlyEndDate", (object) value);
    }

    public string IsMultiFamily
    {
      get => this.getField(this.dataTableName + ".IsMultiFamily", (object) "") as string;
      set => this.setField(this.dataTableName + ".IsMultiFamily", (object) value);
    }

    public string NoteCustodian
    {
      get => this.getField(this.dataTableName + ".NoteCustodian", (object) "") as string;
      set => this.setField(this.dataTableName + ".NoteCustodian", (object) value);
    }

    public string FinancialInstNum
    {
      get => this.getField(this.dataTableName + ".FinancialInstNum", (object) "") as string;
      set => this.setField(this.dataTableName + ".FinancialInstNum", (object) value);
    }

    public string StandardLookback
    {
      get => this.getField(this.dataTableName + ".StandardLookback", (object) "") as string;
      set => this.setField(this.dataTableName + ".StandardLookback", (object) value);
    }

    public string IsGuarantyFeeAddOn
    {
      get => this.getField(this.dataTableName + ".IsGuarantyFeeAddOn", (object) "") as string;
      set => this.setField(this.dataTableName + ".IsGuarantyFeeAddOn", (object) value);
    }

    public string InvestorRemittanceType
    {
      get => this.getField(this.dataTableName + ".InvestorRemittanceType", (object) "") as string;
      set => this.setField(this.dataTableName + ".InvestorRemittanceType", (object) value);
    }

    public string IssuerNum
    {
      get => this.getField(this.dataTableName + ".IssuerNum", (object) "") as string;
      set => this.setField(this.dataTableName + ".IssuerNum", (object) value);
    }

    public string IssueType
    {
      get => this.getField(this.dataTableName + ".IssueType", (object) "") as string;
      set => this.setField(this.dataTableName + ".IssueType", (object) value);
    }

    public Decimal ARMIndex
    {
      get => (Decimal) this.getField(this.dataTableName + ".ARMIndex", (object) 0M);
      set => this.setField(this.dataTableName + ".ARMIndex", (object) value);
    }

    public string CertAgreement
    {
      get => this.getField(this.dataTableName + ".CertAgreement", (object) "") as string;
      set => this.setField(this.dataTableName + ".CertAgreement", (object) value);
    }

    public string PnICustodialABA
    {
      get => this.getField(this.dataTableName + ".PnICustodialABA", (object) "") as string;
      set => this.setField(this.dataTableName + ".PnICustodialABA", (object) value);
    }

    public string SubscriberRecordABA
    {
      get => this.getField(this.dataTableName + ".SubscriberRecordABA", (object) "") as string;
      set => this.setField(this.dataTableName + ".SubscriberRecordABA", (object) value);
    }

    public string SubscriberRecordFRBPosDesc
    {
      get
      {
        return this.getField(this.dataTableName + ".SubscriberRecordFRBPosDesc", (object) "") as string;
      }
      set => this.setField(this.dataTableName + ".SubscriberRecordFRBPosDesc", (object) value);
    }

    public string SubscriberRecordFRBAcctDesc
    {
      get
      {
        return this.getField(this.dataTableName + ".SubscriberRecordFRBAcctDesc", (object) "") as string;
      }
      set => this.setField(this.dataTableName + ".SubscriberRecordFRBAcctDesc", (object) value);
    }

    public string MasterTnIABA
    {
      get => this.getField(this.dataTableName + ".MasterTnIABA", (object) "") as string;
      set => this.setField(this.dataTableName + ".MasterTnIABA", (object) value);
    }

    public string NewTransferIssuerNum
    {
      get => this.getField(this.dataTableName + ".NewTransferIssuerNum", (object) "") as string;
      set => this.setField(this.dataTableName + ".NewTransferIssuerNum", (object) value);
    }

    public DateTime MaturityDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".MaturityDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".MaturityDate", (object) value);
    }

    public string PoolTaxID
    {
      get => this.getField(this.dataTableName + ".PoolTaxID", (object) "") as string;
      set => this.setField(this.dataTableName + ".PoolTaxID", (object) value);
    }

    public DateTime ChangeDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".ChangeDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".ChangeDate", (object) value);
    }

    public string IsBondFinancePool
    {
      get => this.getField(this.dataTableName + ".IsBondFinancePool", (object) "") as string;
      set => this.setField(this.dataTableName + ".IsBondFinancePool", (object) value);
    }

    public string IsSent1711ToCustodian
    {
      get => this.getField(this.dataTableName + ".IsSent1711ToCustodian", (object) "") as string;
      set => this.setField(this.dataTableName + ".IsSent1711ToCustodian", (object) value);
    }

    public string PnICustodialAcctNum
    {
      get => this.getField(this.dataTableName + ".PnICustodialAcctNum", (object) "") as string;
      set => this.setField(this.dataTableName + ".PnICustodialAcctNum", (object) value);
    }

    public string SubscriberRecordAcctNum
    {
      get => this.getField(this.dataTableName + ".SubscriberRecordAcctNum", (object) "") as string;
      set => this.setField(this.dataTableName + ".SubscriberRecordAcctNum", (object) value);
    }

    public string MasterTnIAcctNum
    {
      get => this.getField(this.dataTableName + ".MasterTnIAcctNum", (object) "") as string;
      set => this.setField(this.dataTableName + ".MasterTnIAcctNum", (object) value);
    }

    public string SubservicerIssuerNum
    {
      get => this.getField(this.dataTableName + ".SubservicerIssuerNum", (object) "") as string;
      set => this.setField(this.dataTableName + ".SubservicerIssuerNum", (object) value);
    }

    public string GinniePoolType
    {
      get => this.getField(this.dataTableName + ".GinniePoolType", (object) "") as string;
      set => this.setField(this.dataTableName + ".GinniePoolType", (object) value);
    }

    public DateTime InitialDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".InitialDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".InitialDate", (object) value);
    }

    public DateTime LastPaidInstallmentDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".LastPaidInstallmentDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".LastPaidInstallmentDate", (object) value);
    }

    public DateTime UnpaidBalanceDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".UnpaidBalanceDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".UnpaidBalanceDate", (object) value);
    }

    public string ACHBankAccountPurposeType
    {
      get
      {
        return this.getField(this.dataTableName + ".ACHBankAccountPurposeType", (object) "") as string;
      }
      set => this.setField(this.dataTableName + ".ACHBankAccountPurposeType", (object) value);
    }

    public string ACHABARoutingAndTransitNum
    {
      get => this.getField(this.dataTableName + ".ACHABARoutingTransitNum", (object) "") as string;
      set => this.setField(this.dataTableName + ".ACHABARoutingTransitNum", (object) value);
    }

    public string ACHABARoutingAndTransitId
    {
      get => this.getField(this.dataTableName + ".ACHABARoutingTransitId", (object) "") as string;
      set => this.setField(this.dataTableName + ".ACHABARoutingTransitId", (object) value);
    }

    public string ACHInsitutionTelegraphicName
    {
      get => this.getField(this.dataTableName + ".ACHInstitTelName", (object) "") as string;
      set => this.setField(this.dataTableName + ".ACHInstitTelName", (object) value);
    }

    public string ACHReceiverSubaccountName
    {
      get => this.getField(this.dataTableName + ".ACHReceiverSubacctName", (object) "") as string;
      set => this.setField(this.dataTableName + ".ACHReceiverSubacctName", (object) value);
    }

    public string ACHBankAccountDescription
    {
      get => this.getField(this.dataTableName + ".ACHBankAccountDesc", (object) "") as string;
      set => this.setField(this.dataTableName + ".ACHBankAccountDesc", (object) value);
    }

    public string GinniePoolIndexType
    {
      get => this.getField(this.dataTableName + ".GinnieIndexType", (object) "") as string;
      set => this.setField(this.dataTableName + ".GinnieIndexType", (object) value);
    }

    public string PoolIssuerTransferee
    {
      get => this.getField(this.dataTableName + ".PoolIssuerTransferee", (object) "") as string;
      set => this.setField(this.dataTableName + ".PoolIssuerTransferee", (object) value);
    }

    public DateTime PoolCertificatePaymentDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".PoolCertPaymentDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".PoolCertPaymentDate", (object) value);
    }

    public string BondFinanceProgramType
    {
      get => this.getField(this.dataTableName + ".BondFinProgType", (object) "") as string;
      set => this.setField(this.dataTableName + ".BondFinProgType", (object) value);
    }

    public string BondFinanceProgramName
    {
      get => this.getField(this.dataTableName + ".BondFinProgName", (object) "") as string;
      set => this.setField(this.dataTableName + ".BondFinProgName", (object) value);
    }

    public string GinniePoolClassType
    {
      get => this.getField(this.dataTableName + ".PoolClassType", (object) "") as string;
      set => this.setField(this.dataTableName + ".PoolClassType", (object) value);
    }

    public string GinniePoolConcurrentTransferIndicator
    {
      get => this.getField(this.dataTableName + ".PoolConcurrTransferIndc", (object) "") as string;
      set => this.setField(this.dataTableName + ".PoolConcurrTransferIndc", (object) value);
    }

    public int PoolCurrentLoanCount
    {
      get => (int) this.getField(this.dataTableName + ".PoolCurrentLoanCount", (object) -1);
      set => this.setField(this.dataTableName + ".PoolCurrentLoanCount", (object) value);
    }

    public Decimal PoolCurrentPrincipalBalAmt
    {
      get => (Decimal) this.getField(this.dataTableName + ".PoolCurrentPrinBal", (object) 0M);
      set => this.setField(this.dataTableName + ".PoolCurrentPrinBal", (object) value);
    }

    public string PoolingMethodType
    {
      get => this.getField(this.dataTableName + ".PoolingMethodType", (object) "") as string;
      set => this.setField(this.dataTableName + ".PoolingMethodType", (object) value);
    }

    public DateTime PoolInterestAdjustmentEffectiveDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".PoolInterestAdjEffectiveDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".PoolInterestAdjEffectiveDate", (object) value);
    }

    public int PoolMaturityPeriodCount
    {
      get => (int) this.getField(this.dataTableName + ".PoolMaturityPeriodCount", (object) -1);
      set => this.setField(this.dataTableName + ".PoolMaturityPeriodCount", (object) value);
    }

    public string DocSubmissionIndicator
    {
      get => this.getField(this.dataTableName + ".DocSubmissionIndic", (object) "") as string;
      set => this.setField(this.dataTableName + ".DocSubmissionIndic", (object) value);
    }

    public string DocReqIndicator
    {
      get => this.getField(this.dataTableName + ".DocReqIndic", (object) "") as string;
      set => this.setField(this.dataTableName + ".DocReqIndic", (object) value);
    }

    public Decimal SecurityOrigSubscriptionAmt
    {
      get
      {
        return (Decimal) this.getField(this.dataTableName + ".SecurityOriginalSubscrAmt", (object) 0M);
      }
      set => this.setField(this.dataTableName + ".SecurityOriginalSubscrAmt", (object) value);
    }

    public bool Archived
    {
      get => this.Status == TradeStatus.Archived;
      set => this.tradeStatus = value ? 5 : 1;
    }

    public DateTime AssignedStatusDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".AssignedStatusDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".AssignedStatusDate", (object) value);
    }

    private int tradeStatus
    {
      get => (int) this.getField(this.dataTableName + ".Status", (object) -1);
      set => this.setField(this.dataTableName + ".Status", (object) value);
    }

    public Decimal GainLossAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".GainLossAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".GainLossAmount", (object) value);
    }

    public Decimal NetProfit
    {
      get => (Decimal) this.getField(this.dataTableName + ".NetProfit", (object) 0M);
      set => this.setField(this.dataTableName + ".NetProfit", (object) value);
    }

    public DateTime LastModified
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".LastModified", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".LastModified", (object) value);
    }

    public int LoanCount
    {
      get => (int) this.getField("TradeMbsPoolSummary.LoanCount", (object) 0);
      set => this.setField("TradeMbsPoolSummary.LoanCount", (object) value);
    }

    public Decimal AssignedProfit
    {
      get => (Decimal) this.getField("TradeMbsPoolSummary.TotalProfit", (object) 0M);
      set => this.setField("TradeMbsPoolSummary.TotalProfit", (object) value);
    }

    public override Decimal AssignedAmount
    {
      get => (Decimal) this.getField("TradeMbsPoolSummary.TotalAmount", (object) 0M);
      set => this.setField("TradeMbsPoolSummary.TotalAmount", (object) value);
    }

    public int PendingLoanCount
    {
      get => (int) this.getField("TradeMbsPoolSummary.PendingLoanCount", (object) 0);
      set => this.setField("TradeMbsPoolSummary.PendingLoanCount", (object) value);
    }

    public override Decimal CompletionPercent
    {
      get => (Decimal) this.getField("TradeMbsPoolSummary.CompletionPercent", (object) 0M);
      set => this.setField("TradeMbsPoolSummary.CompletionPercent", (object) value);
    }

    public string CommitmentType
    {
      get => this.getField(this.dataTableName + ".CommitmentType", (object) "") as string;
      set => this.setField(this.dataTableName + ".CommitmentType", (object) value);
    }

    public string WeightedAvgPriceLocked
    {
      get => this.getField(this.dataTableName + ".WeightedAvgPriceLocked", (object) "") as string;
      set => this.setField(this.dataTableName + ".WeightedAvgPriceLocked", (object) value);
    }

    public string SellerId
    {
      get => this.getField(this.dataTableName + ".SellerId", (object) "") as string;
      set => this.setField(this.dataTableName + ".SellerId", (object) value);
    }

    public Decimal MinServicingFee
    {
      get => (Decimal) this.getField(this.dataTableName + ".MinServicingFee", (object) 0M);
      set => this.setField(this.dataTableName + ".MinServicingFee", (object) value);
    }

    public Decimal MaxBU
    {
      get => (Decimal) this.getField(this.dataTableName + ".MaxBU", (object) 0M);
      set => this.setField(this.dataTableName + ".MaxBU", (object) value);
    }

    public int WithdrawnLoanCount => this.LoanCount - this.NotWithdrawnLoanCount;

    public int NotWithdrawnLoanCount
    {
      get => (int) this.getField("TradeMbsPoolSummary.NotWithdrawnLoanCount", (object) 0);
      set => this.setField("TradeMbsPoolSummary.NotWithdrawnLoanCount", (object) value);
    }

    public string AssignedSecurityID
    {
      get => this.getField(this.dataTableName + ".AssignedSecurityID", (object) "") as string;
      set => this.setField(this.dataTableName + ".AssignedSecurityID", (object) value);
    }

    public int GetPendingLoanCount(MbsPoolLoanStatus status)
    {
      return this.tradeStatus == 6 || this.pendingLoanCounts == null || !this.pendingLoanCounts.ContainsKey(status) ? 0 : this.pendingLoanCounts[status];
    }

    public int GetPendingLoanCount()
    {
      if (this.pendingLoanCounts == null)
        return 0;
      int pendingLoanCount = 0;
      foreach (int num in this.pendingLoanCounts.Values)
        pendingLoanCount += num;
      return pendingLoanCount;
    }

    public override string ToString() => this.Name;

    public override object this[string propertyName]
    {
      get
      {
        return propertyName == this.dataTableName + ".Status" ? (object) (int) this.Status : base[propertyName];
      }
      set => base[propertyName] = value;
    }
  }
}
