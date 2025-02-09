// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.LoanTradeViewModel
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class LoanTradeViewModel : TradeViewModel
  {
    private Dictionary<LoanTradeStatus, int> pendingLoanCounts;

    public LoanTradeViewModel(
      int tradeID,
      string guid,
      string name,
      int contractId,
      string contractNumber,
      int status,
      bool locked,
      string dealerName,
      string assigneeName,
      string investorName,
      string investorTradeNum,
      string investorCommitmentNum,
      DateTime investorDeliveryDate,
      DateTime earlyDeliveryDate,
      DateTime targetDeliveryDate,
      Decimal tradeAmount,
      Decimal tolerance,
      Decimal pairOffFee,
      DateTime commitmentDate,
      DateTime shipmentDate,
      DateTime purchaseDate,
      Decimal rateAdjustment,
      Decimal buyUpAmount,
      Decimal buyDownAmount,
      Decimal miscAdjustment,
      DateTime lastModified,
      int loanCount,
      Dictionary<LoanTradeStatus, int> pendingLoanCounts,
      Decimal assignedProfit,
      Decimal assignedAmount,
      Decimal pairOffAmount,
      Decimal completePercent,
      int pendingLoanCount,
      Decimal minAmount,
      Decimal maxAmount,
      string commitmentType,
      string tradeDescription,
      int securityTradeID,
      DateTime assignedStatusDate,
      Decimal gainLossAmount,
      Decimal netProfit,
      string assignedSecurityID,
      string servicer,
      int servicingType,
      string pairOffsXml,
      bool isWeightedAvgBulkPriceLocked,
      Decimal weightedAvgBulkPrice,
      bool isBulkDelivery)
      : base(tradeID, guid, name, pairOffAmount)
    {
      this.ContractID = contractId;
      this.ContractNumber = contractNumber;
      this.tradeStatus = status;
      this.Locked = locked;
      this.DealerName = dealerName;
      this.AssigneeName = assigneeName;
      this.InvestorName = investorName;
      this.InvestorTradeNumber = investorTradeNum;
      this.InvestorCommitmentNumber = investorCommitmentNum;
      this.InvestorDeliveryDate = investorDeliveryDate;
      this.EarlyDeliveryDate = earlyDeliveryDate;
      this.TargetDeliveryDate = targetDeliveryDate;
      this.TradeAmount = tradeAmount;
      this.Tolerance = tolerance;
      this.PairOffFee = pairOffFee;
      this.CommitmentDate = commitmentDate;
      this.ShipmentDate = shipmentDate;
      this.PurchaseDate = purchaseDate;
      this.RateAdjustment = rateAdjustment;
      this.BuyUpAmount = buyUpAmount;
      this.BuyDownAmount = buyDownAmount;
      this.MiscAdjustment = miscAdjustment;
      this.LastModified = lastModified;
      this.LoanCount = loanCount;
      this.pendingLoanCounts = pendingLoanCounts;
      this.AssignedAmount = assignedAmount;
      this.AssignedProfit = assignedProfit;
      this.PairOffAmount = pairOffAmount;
      this.CompletionPercent = completePercent;
      this.PendingLoanCount = pendingLoanCount;
      this.MinAmount = minAmount;
      this.MaxAmount = maxAmount;
      this.CommitmentType = commitmentType;
      this.TradeDescription = tradeDescription;
      this.AssignedStatusDate = assignedStatusDate;
      this.GainLossAmount = gainLossAmount;
      this.NetProfit = netProfit;
      this.AssignedSecurityID = assignedSecurityID;
      this.Servicer = servicer;
      this.servicingType = servicingType;
      this.IsBulkDelivery = isBulkDelivery;
      this.IsWeightedAvgBulkPriceLocked = isWeightedAvgBulkPriceLocked;
      this.WeightedAvgBulkPrice = weightedAvgBulkPrice;
      XmlDocument xml = new XmlDocument();
      if (!string.IsNullOrEmpty(pairOffsXml))
        xml.LoadXml(pairOffsXml);
      this.PairOffDate1 = this.GetDateFromXml(xml, "date", 0);
      this.PairOffAmount1 = -this.GetDecimalFromXml(xml, "amt", 0);
      this.PairOffBuyPrice1 = this.GetDecimalFromXml(xml, "pairOffFeePercentage", 0);
      this.PairOffGainLoss1 = this.GetDecimalFromXml(xml, "fee", 0);
      this.PairOffDate2 = this.GetDateFromXml(xml, "date", 1);
      this.PairOffAmount2 = -this.GetDecimalFromXml(xml, "amt", 1);
      this.PairOffBuyPrice2 = this.GetDecimalFromXml(xml, "pairOffFeePercentage", 1);
      this.PairOffGainLoss2 = this.GetDecimalFromXml(xml, "fee", 1);
      this.PairOffDate3 = this.GetDateFromXml(xml, "date", 2);
      this.PairOffAmount3 = -this.GetDecimalFromXml(xml, "amt", 2);
      this.PairOffBuyPrice3 = this.GetDecimalFromXml(xml, "pairOffFeePercentage", 2);
      this.PairOffGainLoss3 = this.GetDecimalFromXml(xml, "fee", 2);
      this.PairOffDate4 = this.GetDateFromXml(xml, "date", 3);
      this.PairOffAmount4 = -this.GetDecimalFromXml(xml, "amt", 3);
      this.PairOffBuyPrice4 = this.GetDecimalFromXml(xml, "pairOffFeePercentage", 3);
      this.PairOffGainLoss4 = this.GetDecimalFromXml(xml, "fee", 3);
    }

    private DateTime GetDateFromXml(XmlDocument xml, string field, int index)
    {
      DateTime result = new DateTime();
      string xpath = string.Format("//element[@name='{0}']/element[@name='{1}']", (object) index, (object) field);
      XmlNode xmlNode = xml.SelectSingleNode(xpath);
      if (xmlNode == null)
        return result;
      DateTime.TryParse(xmlNode.InnerText, out result);
      int num = result == DateTime.MinValue ? 1 : 0;
      return result;
    }

    private Decimal GetDecimalFromXml(XmlDocument xml, string field, int index)
    {
      string xpath = string.Format("//element[@name='{0}']/element[@name='{1}']", (object) index, (object) field);
      XmlNode xmlNode = xml.SelectSingleNode(xpath);
      if (xmlNode == null)
        return 0M;
      Decimal result;
      Decimal.TryParse(xmlNode.InnerText, out result);
      return result == 0M ? 0M : result;
    }

    protected override string dataTableName => "LoanTradeDetails";

    public DateTime PairOffDate1
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".PairOffDate1", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".PairOffDate1", (object) value);
    }

    public DateTime PairOffDate2
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".PairOffDate2", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".PairOffDate2", (object) value);
    }

    public DateTime PairOffDate3
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".PairOffDate3", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".PairOffDate3", (object) value);
    }

    public DateTime PairOffDate4
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".PairOffDate4", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".PairOffDate4", (object) value);
    }

    public Decimal PairOffAmount1
    {
      get => (Decimal) this.getField(this.dataTableName + ".PairOffAmount1", (object) 0M);
      set => this.setField(this.dataTableName + ".PairOffAmount1", (object) value);
    }

    public Decimal PairOffAmount2
    {
      get => (Decimal) this.getField(this.dataTableName + ".PairOffAmount2", (object) 0M);
      set => this.setField(this.dataTableName + ".PairOffAmount2", (object) value);
    }

    public Decimal PairOffAmount3
    {
      get => (Decimal) this.getField(this.dataTableName + ".PairOffAmount3", (object) 0M);
      set => this.setField(this.dataTableName + ".PairOffAmount3", (object) value);
    }

    public Decimal PairOffAmount4
    {
      get => (Decimal) this.getField(this.dataTableName + ".PairOffAmount4", (object) 0M);
      set => this.setField(this.dataTableName + ".PairOffAmount4", (object) value);
    }

    public Decimal PairOffBuyPrice1
    {
      get => (Decimal) this.getField(this.dataTableName + ".PairOffBuyPrice1", (object) 0M);
      set => this.setField(this.dataTableName + ".PairOffBuyPrice1", (object) value);
    }

    public Decimal PairOffBuyPrice2
    {
      get => (Decimal) this.getField(this.dataTableName + ".PairOffBuyPrice2", (object) 0M);
      set => this.setField(this.dataTableName + ".PairOffBuyPrice2", (object) value);
    }

    public Decimal PairOffBuyPrice3
    {
      get => (Decimal) this.getField(this.dataTableName + ".PairOffBuyPrice3", (object) 0M);
      set => this.setField(this.dataTableName + ".PairOffBuyPrice3", (object) value);
    }

    public Decimal PairOffBuyPrice4
    {
      get => (Decimal) this.getField(this.dataTableName + ".PairOffBuyPrice4", (object) 0M);
      set => this.setField(this.dataTableName + ".PairOffBuyPrice4", (object) value);
    }

    public Decimal PairOffGainLoss1
    {
      get => (Decimal) this.getField(this.dataTableName + ".PairOffGainLoss1", (object) 0M);
      set => this.setField(this.dataTableName + ".PairOffGainLoss1", (object) value);
    }

    public Decimal PairOffGainLoss2
    {
      get => (Decimal) this.getField(this.dataTableName + ".PairOffGainLoss2", (object) 0M);
      set => this.setField(this.dataTableName + ".PairOffGainLoss2", (object) value);
    }

    public Decimal PairOffGainLoss3
    {
      get => (Decimal) this.getField(this.dataTableName + ".PairOffGainLoss3", (object) 0M);
      set => this.setField(this.dataTableName + ".PairOffGainLoss3", (object) value);
    }

    public Decimal PairOffGainLoss4
    {
      get => (Decimal) this.getField(this.dataTableName + ".PairOffGainLoss4", (object) 0M);
      set => this.setField(this.dataTableName + ".PairOffGainLoss4", (object) value);
    }

    public override int ContractID
    {
      get => (int) this.getField(this.dataTableName + ".ContractID", (object) -1);
      set => this.setField(this.dataTableName + ".ContractID", (object) value);
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

    public int SecurityTradeID
    {
      get => (int) this.getField(this.dataTableName + ".SecurityTradeID", (object) -1);
      set => this.setField(this.dataTableName + ".SecurityTradeID", (object) value);
    }

    public string AssignedSecurityID
    {
      get => this.getField("AssignedSecurityTradeDetails.Name", (object) "") as string;
      set => this.setField("AssignedSecurityTradeDetails.Name", (object) value);
    }

    public string ContractNumber
    {
      get => this.getField("TradesMasterContract.ContractNumber", (object) "") as string;
      set => this.setField("TradesMasterContract.ContractNumber", (object) value);
    }

    private int tradeStatus
    {
      get => (int) this.getField(this.dataTableName + ".Status", (object) -1);
      set => this.setField(this.dataTableName + ".Status", (object) value);
    }

    private int servicingType
    {
      get => (int) this.getField(this.dataTableName + ".ServicingType", (object) -999);
      set => this.setField(this.dataTableName + ".ServicingType", (object) value);
    }

    public string CommitmentType
    {
      get => this.getField(this.dataTableName + ".CommitmentType", (object) "") as string;
      set => this.setField(this.dataTableName + ".CommitmentType", (object) value);
    }

    public string TradeDescription
    {
      get => this.getField(this.dataTableName + ".TradeDescription", (object) "") as string;
      set => this.setField(this.dataTableName + ".TradeDescription", (object) value);
    }

    public string Servicer
    {
      get => this.getField(this.dataTableName + ".Servicer", (object) "") as string;
      set => this.setField(this.dataTableName + ".Servicer", (object) value);
    }

    public ServicingType ServicingType
    {
      get
      {
        return (ServicingType) this.getField(this.dataTableName + ".ServicingType", (object) ServicingType.None);
      }
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

    public bool Archived
    {
      get => this.Status == TradeStatus.Archived;
      set => this.tradeStatus = value ? 5 : 1;
    }

    public override bool Locked
    {
      get
      {
        return this.Status == TradeStatus.Archived || (bool) this.getField(this.dataTableName + ".Locked", (object) false);
      }
      set => this.setField(this.dataTableName + ".Locked", (object) value);
    }

    public virtual string DealerName
    {
      get => this.getField(this.dataTableName + ".DealerName", (object) "") as string;
      set => this.setField(this.dataTableName + ".DealerName", (object) value);
    }

    public virtual string AssigneeName
    {
      get => this.getField(this.dataTableName + ".AssigneeName", (object) "") as string;
      set => this.setField(this.dataTableName + ".AssigneeName", (object) value);
    }

    public override string InvestorName
    {
      get => this.getField(this.dataTableName + ".InvestorName", (object) "") as string;
      set => this.setField(this.dataTableName + ".InvestorName", (object) value);
    }

    public string InvestorTradeNumber
    {
      get => this.getField(this.dataTableName + ".InvestorTradeNum", (object) "") as string;
      set => this.setField(this.dataTableName + ".InvestorTradeNum", (object) value);
    }

    public string InvestorCommitmentNumber
    {
      get => this.getField(this.dataTableName + ".InvestorCommitmentNum", (object) "") as string;
      set => this.setField(this.dataTableName + ".InvestorCommitmentNum", (object) value);
    }

    public DateTime InvestorDeliveryDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".InvestorDeliveryDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".InvestorDeliveryDate", (object) value);
    }

    public DateTime AssignedStatusDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".AssignedStatusDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".AssignedStatusDate", (object) value);
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

    public override Decimal TradeAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".TradeAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".TradeAmount", (object) value);
    }

    public Decimal MinAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".MinAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".MinAmount", (object) value);
    }

    public Decimal MaxAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".MaxAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".MaxAmount", (object) value);
    }

    public Decimal Tolerance
    {
      get => (Decimal) this.getField(this.dataTableName + ".Tolerance", (object) 0M);
      set => this.setField(this.dataTableName + ".Tolerance", (object) value);
    }

    public virtual Decimal PairOffFee
    {
      get => (Decimal) this.getField(this.dataTableName + ".PairOffFee", (object) 0M);
      set => this.setField(this.dataTableName + ".PairOffFee", (object) value);
    }

    public DateTime CommitmentDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".CommitmentDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".CommitmentDate", (object) value);
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
      get => (int) this.getField("TradeLoanTradeSummary.LoanCount", (object) 0);
      set => this.setField("TradeLoanTradeSummary.LoanCount", (object) value);
    }

    public Decimal AssignedProfit
    {
      get => (Decimal) this.getField("TradeLoanTradeSummary.TotalProfit", (object) 0M);
      set => this.setField("TradeLoanTradeSummary.TotalProfit", (object) value);
    }

    public override Decimal AssignedAmount
    {
      get => (Decimal) this.getField("TradeLoanTradeSummary.TotalAmount", (object) 0M);
      set => this.setField("TradeLoanTradeSummary.TotalAmount", (object) value);
    }

    public int PendingLoanCount
    {
      get => (int) this.getField("TradeLoanTradeSummary.PendingLoanCount", (object) 0);
      set => this.setField("TradeLoanTradeSummary.PendingLoanCount", (object) value);
    }

    public override Decimal CompletionPercent
    {
      get => (Decimal) this.getField("TradeLoanTradeSummary.CompletionPercent", (object) 0M);
      set => this.setField("TradeLoanTradeSummary.CompletionPercent", (object) value);
    }

    public override Decimal PairOffAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".PairOffAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".PairOffAmount", (object) value);
    }

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

    public Decimal OpenAmount
    {
      get => (Decimal) this.getField("TradeLoanTradeSummary.OpenAmount", (object) 0M);
      set => this.setField("TradeLoanTradeSummary.OpenAmount", (object) value);
    }

    public bool IsWeightedAvgBulkPriceLocked
    {
      get
      {
        return (bool) this.getField(this.dataTableName + ".IsWeightedAvgBulkPriceLocked", (object) false);
      }
      set => this.setField(this.dataTableName + ".IsWeightedAvgBulkPriceLocked", (object) value);
    }

    public bool IsBulkDelivery
    {
      get => (bool) this.getField(this.dataTableName + ".IsBulkDelivery", (object) false);
      set => this.setField(this.dataTableName + ".IsBulkDelivery", (object) value);
    }

    public Decimal WeightedAvgBulkPrice
    {
      get => (Decimal) this.getField(this.dataTableName + ".WeightedAvgBulkPrice", (object) 0M);
      set => this.setField(this.dataTableName + ".WeightedAvgBulkPrice", (object) value);
    }

    public int WithdrawnLoanCount => this.LoanCount - this.NotWithdrawnLoanCount;

    public int NotWithdrawnLoanCount
    {
      get => (int) this.getField("TradeLoanTradeSummary.NotWithdrawnLoanCount", (object) 0);
      set => this.setField("TradeLoanTradeSummary.NotWithdrawnLoanCount", (object) value);
    }

    public int GetPendingLoanCount(LoanTradeStatus status)
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
