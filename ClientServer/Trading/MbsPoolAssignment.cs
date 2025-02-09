// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MbsPoolAssignment
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class MbsPoolAssignment : TradeAssignmentByTradeBase
  {
    private MbsPoolInfo trade;
    private TradeInfoObj assigneeTrade;
    private MbsPoolSecurityTradeStatus assignedStatus;

    public MbsPoolAssignment(
      MbsPoolInfo trade,
      TradeInfoObj assigneeTrade,
      MbsPoolSecurityTradeStatus assignedStatus,
      DateTime assignedDate,
      Decimal assignedAmount)
      : base(assignedDate, assignedAmount)
    {
      this.trade = trade;
      this.assigneeTrade = assigneeTrade;
      this.assignedStatus = assignedStatus;
    }

    public MbsPoolAssignment(
      MbsPoolInfo trade,
      TradeInfoObj assigneeTrade,
      MbsPoolSecurityTradeStatus assignedStatus,
      DateTime assignedDate)
      : this(trade, assigneeTrade, assignedStatus, assignedDate, 0M)
    {
    }

    public MbsPoolAssignment(MbsPoolInfo trade, TradeInfoObj assigneeTrade)
      : this(trade, assigneeTrade, MbsPoolSecurityTradeStatus.Assigned, DateTime.Now)
    {
    }

    public MbsPoolSecurityTradeStatus AssignedStatus
    {
      get => this.assignedStatus;
      set => this.assignedStatus = value;
    }

    public bool Assigned => this.AssignedStatus != MbsPoolSecurityTradeStatus.Unassigned;

    public bool Removed => this.AssignedStatus == MbsPoolSecurityTradeStatus.Unassigned;

    public MbsPoolInfo Trade => this.trade;

    public TradeInfoObj AssigneeTrade => this.assigneeTrade;

    public override int? TradeID => this.trade != null ? new int?(this.trade.TradeID) : new int?();

    public override int? AssigneeTradeID
    {
      get => this.assigneeTrade != null ? new int?(this.assigneeTrade.TradeID) : new int?();
    }

    public static MbsPoolAssignment[] Convert(TradeAssignmentByTradeBase[] assignments)
    {
      if (assignments == null)
        return (MbsPoolAssignment[]) null;
      MbsPoolAssignment[] mbsPoolAssignmentArray = new MbsPoolAssignment[assignments.Length];
      for (int index = 0; index < assignments.Length; ++index)
        mbsPoolAssignmentArray[index] = (MbsPoolAssignment) assignments[index];
      return mbsPoolAssignmentArray;
    }
  }
}
