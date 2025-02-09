// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.GSECommitment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Trading;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  /// <summary>GSECommitment class</summary>
  public class GSECommitment
  {
    /// <summary>Gets or sets trade id of the GSE Commitment</summary>
    public int ID { get; set; }

    /// <summary>Gets or sets commitment ID of the GSE Commitment</summary>
    public string CommitmentID { get; set; }

    /// <summary>Gets or sets contract number of the GSE Commitment</summary>
    public string ContractNumber { get; set; }

    /// <summary>Gets or sets trade description of the GSE Commitment</summary>
    public string TradeDescription { get; set; }

    /// <summary>Gets or sets commitment date of the GSE Commitment</summary>
    public DateTime CommitmentDate { get; set; }

    /// <summary>Gets or sets seller number of the GSE Commitment</summary>
    public string SellerNumber { get; set; }

    /// <summary>Gets or sets commitment amount of the GSE Commitment</summary>
    public Decimal TradeAmount { get; set; }

    /// <summary>
    /// Gets or sets outstanding balance of the GSE Commitment
    /// </summary>
    public Decimal OutstandingBalance { get; set; }

    /// <summary>Gets or sets issue month of the GSE Commitment</summary>
    public DateTime IssueMonth { get; set; }

    /// <summary>
    /// Gets or sets minimum delivery amount of the GSE Commitment
    /// </summary>
    public Decimal MinDeliveryAmount { get; set; }

    /// <summary>
    /// Gets or sets maximum delivery amount of the GSE Commitment
    /// </summary>
    public Decimal MaxDeliveryAmount { get; set; }

    /// <summary>Gets or sets fulfilled amount of the GSE Commitment</summary>
    public Decimal FulfilledAmount { get; set; }

    /// <summary>Gets or sets pending amount of the GSE Commitment</summary>
    public Decimal PendingAmount { get; set; }

    /// <summary>
    /// Gets or sets total pair-off amount of the GSE Commitment
    /// </summary>
    public Decimal TotalPairoffAmount { get; set; }

    /// <summary>Gets or sets fees of the GSE Commitment</summary>
    public Decimal Fees { get; set; }

    /// <summary>
    /// Gets or sets minimum remaining amount of the GSE Commitment
    /// </summary>
    public Decimal MinRemainingAmount { get; set; }

    /// <summary>
    /// Gets or sets maximum remaining amount of the GSE Commitment
    /// </summary>
    public Decimal MaxRemainingAmount { get; set; }

    /// <summary>Gets or sets pairoff fee factor of the GSE Commitment</summary>
    public Decimal PairoffFeeFactor { get; set; }

    /// <summary>Gets or sets rolled amount of the GSE Commitment</summary>
    public Decimal RolledAmount { get; set; }

    /// <summary>Gets or sets roll fee factory of the GSE Commitment</summary>
    public Decimal RollFeeFactor { get; set; }

    /// <summary>Gets or sets rolled to of the GSE Commitment</summary>
    public string RolledTo { get; set; }

    /// <summary>Gets or sets rolled from of the GSE Commitment</summary>
    public string RolledFrom { get; set; }

    /// <summary>Gets or sets fallout amount of the GSE Commitment</summary>
    public Decimal FalloutAmount { get; set; }

    /// <summary>Gets or sets remittance cycle of the GSE Commitment</summary>
    public string RemittanceCycle { get; set; }

    /// <summary>
    /// Gets or sets remittance cycle month of the GSE Commitment
    /// </summary>
    public int RemittanceCycleMonth { get; set; }

    /// <summary>Gets or sets servicing option of the GSE Commitment</summary>
    public string ServicingOption { get; set; }

    /// <summary>Gets or sets bond type of the GSE Commitment</summary>
    public string BondType { get; set; }

    /// <summary>Gets Participation Percentage of the GSE Commitment</summary>
    public Decimal ParticipationPercentage { get; set; }

    /// <summary>Gets or sets buyup buydown grid of the GSE Commitment</summary>
    public string BuyupBuydownGrid { get; set; }

    /// <summary>Gets max buyup amount of the GSE Commitment</summary>
    public Decimal MaxBuyupAmount { get; set; }

    /// <summary>Gets min G-Fee after buydown of the GSE Commitment</summary>
    public Decimal MinGfeeAfterBuydown { get; set; }

    /// <summary>
    /// Gets how many percent of the GSE Commitment has been allocated by assigned loans
    /// </summary>
    public Decimal CompletionPercentage { get; set; }

    /// <summary>Gets or sets locked of the GSE Commitment</summary>
    public bool Locked { get; set; }

    internal GSECommitment(GSECommitmentViewModel info)
    {
      this.ID = info.TradeID;
      this.CommitmentID = info.Name;
      this.ContractNumber = info.ContractNumber;
      this.CommitmentDate = info.CommitmentDate;
      this.TradeDescription = info.TradeDescription;
      this.TradeAmount = info.TradeAmount;
      this.OutstandingBalance = info.OutstandingBalance;
      this.IssueMonth = info.IssueMonth;
      this.CompletionPercentage = info.CompletionPercent;
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
      this.TotalPairoffAmount = info.PairOffAmount;
    }
  }
}
