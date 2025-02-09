// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.GSECommitmentAssignment
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class GSECommitmentAssignment : TradeAssignmentByTradeBase
  {
    private GSECommitmentInfo gseCommitment;
    private TradeInfoObj assigneeTrade;
    private GseCommitmentLoanStatus assignedStatus;
    private DateTime assignedStatusDate;

    public GSECommitmentAssignment(
      GSECommitmentInfo gseCommitment,
      TradeInfoObj assigneeTrade,
      GseCommitmentLoanStatus assignedStatus,
      DateTime assignedDate)
      : base(assignedDate, 0M)
    {
      this.gseCommitment = gseCommitment;
      this.assigneeTrade = assigneeTrade;
      this.assignedStatus = assignedStatus;
      this.assignedStatusDate = assignedDate;
    }

    public GSECommitmentAssignment(GSECommitmentInfo gseCommitment, TradeInfoObj assigneeTrade)
      : this(gseCommitment, assigneeTrade, GseCommitmentLoanStatus.Assigned, DateTime.Now)
    {
    }

    public GSECommitmentAssignment(
      GSECommitmentInfo gseCommitment,
      TradeInfoObj assigneeTrade,
      GseCommitmentLoanStatus assignedStatus,
      DateTime assignedDate,
      Decimal assignedAmount)
      : base(assignedDate, assignedAmount)
    {
      this.gseCommitment = gseCommitment;
      this.assigneeTrade = assigneeTrade;
      this.assignedStatus = assignedStatus;
      this.assignedStatusDate = assignedDate;
    }

    public GseCommitmentLoanStatus AssignedStatus
    {
      get => this.assignedStatus;
      set => this.assignedStatus = value;
    }

    public new DateTime AssignedStatusDate
    {
      get => this.assignedStatusDate;
      set => this.assignedStatusDate = value;
    }

    public bool Assigned => this.AssignedStatus != GseCommitmentLoanStatus.Unassigned;

    public bool Removed => this.AssignedStatus == GseCommitmentLoanStatus.Unassigned;

    public GSECommitmentInfo Trade => this.gseCommitment;

    public TradeInfoObj AssigneeTrade => this.assigneeTrade;

    public override int? TradeID
    {
      get => this.gseCommitment != null ? new int?(this.gseCommitment.TradeID) : new int?();
    }

    public override int? AssigneeTradeID
    {
      get => this.assigneeTrade != null ? new int?(this.assigneeTrade.TradeID) : new int?();
    }
  }
}
