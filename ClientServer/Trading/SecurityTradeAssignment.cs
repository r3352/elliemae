// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.SecurityTradeAssignment
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class SecurityTradeAssignment : TradeAssignmentByTradeBase
  {
    private SecurityTradeInfo securityTrade;
    private LoanTradeInfo assigneeTrade;
    private SecurityLoanTradeStatus assignedStatus;
    private DateTime assignedStatusDate;

    public SecurityTradeAssignment(
      SecurityTradeInfo securityTrade,
      LoanTradeInfo assigneeTrade,
      SecurityLoanTradeStatus assignedStatus,
      DateTime assignedDate)
      : base(assignedDate, 0M)
    {
      this.securityTrade = securityTrade;
      this.assigneeTrade = assigneeTrade;
      this.assignedStatus = assignedStatus;
      this.assignedStatusDate = assignedDate;
    }

    public SecurityTradeAssignment(SecurityTradeInfo securityTrade, LoanTradeInfo assigneeTrade)
      : this(securityTrade, assigneeTrade, SecurityLoanTradeStatus.Assigned, DateTime.Now)
    {
    }

    public SecurityLoanTradeStatus AssignedStatus
    {
      get => this.assignedStatus;
      set => this.assignedStatus = value;
    }

    public new DateTime AssignedStatusDate
    {
      get => this.assignedStatusDate;
      set => this.assignedStatusDate = value;
    }

    public bool Assigned => this.AssignedStatus != SecurityLoanTradeStatus.Unassigned;

    public bool Removed => this.AssignedStatus == SecurityLoanTradeStatus.Unassigned;

    public SecurityTradeInfo Trade => this.securityTrade;

    public LoanTradeInfo AssigneeTrade => this.assigneeTrade;

    public override int? TradeID
    {
      get => this.securityTrade != null ? new int?(this.securityTrade.TradeID) : new int?();
    }

    public override int? AssigneeTradeID
    {
      get => this.assigneeTrade != null ? new int?(this.assigneeTrade.TradeID) : new int?();
    }
  }
}
