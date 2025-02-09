// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.AssignedLoanTrades
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Trading;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  public class AssignedLoanTrades
  {
    private DateTime assignedDate;
    private string tradeId;
    private string tradeDescription;
    private string investor;
    private string investorCommitmentNumber;
    private Decimal tradeAmount;
    private Decimal assingedAmount;
    private Decimal netProfit;

    public DateTime AssignedDate => this.assignedDate;

    public string TradeId => this.tradeId;

    public string TradeDescription => this.tradeDescription;

    public string Investor => this.investor;

    public string InvestorCommitmentNumber => this.investorCommitmentNumber;

    public Decimal TradeAmount => this.tradeAmount;

    public Decimal AssingedAmount => this.assingedAmount;

    public Decimal NetProfit => this.netProfit;

    internal AssignedLoanTrades(LoanTradeViewModel vm)
    {
      this.assignedDate = vm.AssignedStatusDate;
      this.assingedAmount = ((TradeViewModel) vm).AssignedAmount;
      this.investor = ((TradeViewModel) vm).InvestorName;
      this.investorCommitmentNumber = vm.InvestorCommitmentNumber;
      this.netProfit = vm.NetProfit;
      this.tradeAmount = ((TradeViewModel) vm).TradeAmount;
      this.tradeDescription = vm.TradeDescription;
      this.tradeId = ((TradeBase) vm).Name;
    }
  }
}
