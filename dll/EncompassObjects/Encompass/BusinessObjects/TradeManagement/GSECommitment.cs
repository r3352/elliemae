// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.GSECommitment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Trading;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  public class GSECommitment
  {
    public int ID { get; set; }

    public string CommitmentID { get; set; }

    public string ContractNumber { get; set; }

    public string TradeDescription { get; set; }

    public DateTime CommitmentDate { get; set; }

    public string SellerNumber { get; set; }

    public Decimal TradeAmount { get; set; }

    public Decimal OutstandingBalance { get; set; }

    public DateTime IssueMonth { get; set; }

    public Decimal MinDeliveryAmount { get; set; }

    public Decimal MaxDeliveryAmount { get; set; }

    public Decimal FulfilledAmount { get; set; }

    public Decimal PendingAmount { get; set; }

    public Decimal TotalPairoffAmount { get; set; }

    public Decimal Fees { get; set; }

    public Decimal MinRemainingAmount { get; set; }

    public Decimal MaxRemainingAmount { get; set; }

    public Decimal PairoffFeeFactor { get; set; }

    public Decimal RolledAmount { get; set; }

    public Decimal RollFeeFactor { get; set; }

    public string RolledTo { get; set; }

    public string RolledFrom { get; set; }

    public Decimal FalloutAmount { get; set; }

    public string RemittanceCycle { get; set; }

    public int RemittanceCycleMonth { get; set; }

    public string ServicingOption { get; set; }

    public string BondType { get; set; }

    public Decimal ParticipationPercentage { get; set; }

    public string BuyupBuydownGrid { get; set; }

    public Decimal MaxBuyupAmount { get; set; }

    public Decimal MinGfeeAfterBuydown { get; set; }

    public Decimal CompletionPercentage { get; set; }

    public bool Locked { get; set; }

    internal GSECommitment(GSECommitmentViewModel info)
    {
      this.ID = ((TradeBase) info).TradeID;
      this.CommitmentID = ((TradeBase) info).Name;
      this.ContractNumber = info.ContractNumber;
      this.CommitmentDate = info.CommitmentDate;
      this.TradeDescription = info.TradeDescription;
      this.TradeAmount = ((TradeViewModel) info).TradeAmount;
      this.OutstandingBalance = info.OutstandingBalance;
      this.IssueMonth = info.IssueMonth;
      this.CompletionPercentage = ((TradeViewModel) info).CompletionPercent;
      this.BondType = info.BondType;
      this.BuyupBuydownGrid = info.BuyupBuydownGrid;
      this.FalloutAmount = info.FalloutAmount;
      this.Fees = info.Fees;
      this.FulfilledAmount = info.FulfilledAmount;
      this.Locked = info.Locked;
      this.MaxBuyupAmount = info.MaxBuyupAmount;
      this.MaxDeliveryAmount = info.MaxDeliveryAmount;
      this.MaxRemainingAmount = info.MaxRemainingAmount;
      this.MinDeliveryAmount = info.MinDeliveryAmount;
      this.MinGfeeAfterBuydown = info.MinGFeeAfterBuydown;
      this.MinRemainingAmount = info.MinRemainingAmount;
      this.PairoffFeeFactor = info.PairOffFeeFactor;
      this.ParticipationPercentage = info.ParticipationPercent;
      this.PendingAmount = info.PendingAmount;
      this.RemittanceCycle = info.RemittanceCycle;
      this.RemittanceCycleMonth = info.RemittanceCycleMonth;
      this.RollFeeFactor = info.RollFeeFactor;
      this.RolledAmount = info.RolledAmount;
      this.RolledFrom = info.RolledFrom;
      this.RolledTo = info.RolledTo;
      this.SellerNumber = info.SellerNumber;
      this.ServicingOption = info.ServicingOption;
      this.TotalPairoffAmount = ((TradeViewModel) info).PairOffAmount;
    }
  }
}
