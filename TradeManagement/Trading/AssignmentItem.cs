// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.AssignmentItem
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class AssignmentItem : IAssignmentItem
  {
    public AssignmentItem(
      LoanTradeInfo loanTrade,
      LoanTradeAssignment assignment,
      Decimal securityPrice)
    {
      this.LoanTrade = loanTrade;
      this.Assignment = assignment;
      this.SecurityPrice = securityPrice;
    }

    public LoanTradeInfo LoanTrade { get; set; }

    public LoanTradeAssignment Assignment { get; set; }

    public Decimal SecurityPrice { get; set; }
  }
}
