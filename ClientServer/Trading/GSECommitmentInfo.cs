// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.GSECommitmentInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class GSECommitmentInfo : TradeInfoObj
  {
    public string CommitmentId { get; set; }

    public string ContractNumber { get; set; }

    public string SellerNumber { get; set; }

    public Decimal OutstandingBalance { get; set; }

    public string IssueMonth { get; set; }

    public Decimal MinDeliveryAmount { get; set; }

    public Decimal MaxDeliveryAmount { get; set; }

    public Decimal FulfilledAmount { get; set; }

    public Decimal PendingAmount { get; set; }

    public Decimal MinRemainingAmount { get; set; }

    public Decimal MaxRemainingAmount { get; set; }

    public Decimal PairOffFeeFactor { get; set; }

    public Decimal RollFeeFactor { get; set; }

    public string RemittanceCycle { get; set; }

    public Decimal RemittanceDayOfMonth { get; set; }

    public string ServicingOption { get; set; }

    public Decimal ParticipationPercent { get; set; }

    public string BuyUpBuyDownGrid { get; set; }

    public Decimal MaxBuyupAmount { get; set; }

    public GSECommitmentPairOffs GSECommitmentPairOffs { get; set; }

    public FannieMaeProducts Products { get; set; }

    private Decimal pairOffAmount { get; set; }

    public Decimal Fees { get; set; }

    public Decimal RolledAmount { get; set; }

    public string RolledTo { get; set; }

    public string RolledFrom { get; set; }

    public Decimal FalloutAmount { get; set; }

    public string BondType { get; set; }

    public Decimal MinGFeeAfterBuydown { get; set; }

    public bool OutstandingBalanceLock { get; set; }

    public bool PairOffAmountLock { get; set; }

    public Decimal CommitmentAmount { get; set; }

    public Decimal GetTotalDisplayPairOffAmount()
    {
      return Decimal.Negate(GSECommitmentCalculation.CalculatePairOffAmount(this.GSECommitmentPairOffs));
    }

    public Decimal GetTotalPairOffAmount()
    {
      return GSECommitmentCalculation.CalculatePairOffAmount(this.GSECommitmentPairOffs);
    }

    public GSECommitmentInfo Duplicate()
    {
      GSECommitmentInfo gseCommitmentInfo = new GSECommitmentInfo(this);
      gseCommitmentInfo.IsCloned = true;
      return gseCommitmentInfo;
    }

    public GSECommitmentInfo(int tradeID, string guid, string name)
      : base(tradeID, guid, name)
    {
      this.TradeType = TradeType.GSECommitment;
    }

    public GSECommitmentInfo(GSECommitmentInfo source)
      : base(-1, "", "")
    {
      this.ParseTradeObjects("", "", "", "", "", "", "", "", "", "", "", "", "");
      this.TradeType = TradeType.GSECommitment;
      this.TradeDescription = source.TradeDescription;
      this.GSECommitmentPairOffs = new GSECommitmentPairOffs();
      this.TradeAmount = source.TradeAmount;
      this.BuyUpDownItems = source.BuyUpDownItems;
      this.PriceAdjustments = source.PriceAdjustments;
      this.GuarantyFees = source.GuarantyFees;
      this.ProductNames = source.ProductNames;
    }

    public GSECommitmentInfo()
    {
      this.ParseTradeObjects("", "", "", "", "", "", "", "", "", "", "", "", "");
      this.TradeType = TradeType.GSECommitment;
      this.GSECommitmentPairOffs = new GSECommitmentPairOffs();
    }

    public override void ParseTradeObjects(
      string notes,
      string filterQueryXml,
      string pairOffXml,
      string pricingXml,
      string adjustmentsXml,
      string srpTableXml,
      string investorXml,
      string dealerXml,
      string assigneeXml,
      string buyUpDownXml = "�",
      string productXml = "�",
      string guarantyFeeXml = "�",
      string eppsLoanProgram = "�")
    {
      base.ParseTradeObjects(notes, filterQueryXml, pairOffXml, pricingXml, adjustmentsXml, srpTableXml, investorXml, dealerXml, assigneeXml, buyUpDownXml, "", "", "");
    }

    public CorrespondentTradeCalculation Calculation
    {
      get
      {
        return base.Calculation != null ? (CorrespondentTradeCalculation) base.Calculation : new CorrespondentTradeCalculation((ITradeInfoObject) this);
      }
    }

    public Decimal GetDisplayCalculatedPairOffFee()
    {
      Decimal calculatedPairOffFee = 0M;
      foreach (GSECommitmentPairOff commitmentPairOff in this.GSECommitmentPairOffs)
        calculatedPairOffFee += commitmentPairOff.DisplayCalculatedPairOffFee;
      return calculatedPairOffFee;
    }

    public void ResetPairoffAmount() => this.pairOffAmount = this.PairOffAmount;

    public string[] GetPricingFields()
    {
      List<string> stringList = new List<string>();
      stringList.Add("Loan.TotalLoanAmount");
      stringList.Add("Loan.State");
      stringList.Add("Loan.LoanRate");
      stringList.Add("Loan.EscrowWaived");
      stringList.Add("Loan.TotalBuyPrice");
      if (this.PriceAdjustments != null)
      {
        foreach (TradePriceAdjustment priceAdjustment in (List<TradePriceAdjustment>) this.PriceAdjustments)
        {
          foreach (string field in priceAdjustment.CriterionList.GetFieldList())
          {
            if (!stringList.Contains(field))
              stringList.Add(field);
          }
        }
      }
      return stringList.ToArray();
    }

    public Decimal CalculateGainLoss(IEnumerable<PipelineInfo> loans)
    {
      return loans == null ? 0M : this.CalculateGainLoss(new List<PipelineInfo>(loans).ToArray());
    }

    public Decimal CalculateGainLoss(PipelineInfo[] loanList)
    {
      Decimal gainLoss = 0M;
      foreach (PipelineInfo loan in loanList)
        gainLoss += this.CalculateProfit(loan, 0M);
      return gainLoss;
    }

    public Decimal CalculateProfit(PipelineInfo info, Decimal securityPrice)
    {
      Decimal num1 = Utils.ParseDecimal(info.GetField("TotalLoanAmount"));
      Decimal num2 = Utils.ParseDecimal(info.GetField("TotalBuyPrice"));
      Decimal num3 = Utils.ParseDecimal(info.GetField("TotalSellPrice"));
      return num2 <= 0M || num1 <= 0M || num3 == 0M ? 0M : Math.Round((num3 - num2) * num1 / 100M, 2);
    }

    public string[] GetPricingAndEligibilityFields()
    {
      List<string> stringList = new List<string>((IEnumerable<string>) this.GetPricingFields());
      if (this.Filter != null)
      {
        string[] fieldList = this.Filter.GetFieldList();
        for (int index = 0; index < fieldList.Length; ++index)
        {
          if (!stringList.Contains(fieldList[index]))
            stringList.Add(fieldList[index]);
        }
      }
      return stringList.ToArray();
    }

    public static bool ComparePricing(GSECommitmentInfo trade1, GSECommitmentInfo trade2)
    {
      return !(trade1.PriceAdjustments.ToXml() != trade2.PriceAdjustments.ToXml()) && !(trade1.PairOffAmount != trade2.PairOffAmount) && !(trade1.ProductNames.ToXml() != trade2.ProductNames.ToXml()) && !(trade1.MinDeliveryAmount != trade2.MinDeliveryAmount) && !(trade1.MaxDeliveryAmount != trade2.MaxDeliveryAmount);
    }

    public Decimal GetOutstandingBalance(
      Decimal tradeAmount,
      Decimal pairOffAmount,
      Decimal allocatedPoolAmount)
    {
      return GSECommitmentCalculation.CalculateOutstandingBalance(tradeAmount, pairOffAmount, allocatedPoolAmount);
    }
  }
}
