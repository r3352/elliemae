// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.AssignedLoanTrades
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Trading;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  /// <summary>Assinged Loan Trades</summary>
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

    /// <summary>Gets Assigned Date</summary>
    public DateTime AssignedDate => this.assignedDate;

    /// <summary>Gets Trade Id</summary>
    public string TradeId => this.tradeId;

    /// <summary>Gets Trade Description</summary>
    public string TradeDescription => this.tradeDescription;

    /// <summary>Gets Investor Name</summary>
    public string Investor => this.investor;

    /// <summary>Gets Investment commitment number</summary>
    public string InvestorCommitmentNumber => this.investorCommitmentNumber;

    /// <summary>Gets Trade Amount</summary>
    public Decimal TradeAmount => this.tradeAmount;

    /// <summary>Gets Assigned amount</summary>
    public Decimal AssingedAmount => this.assingedAmount;

    /// <summary>Gets Net profit</summary>
    public Decimal NetProfit => this.netProfit;

    internal AssignedLoanTrades(LoanTradeViewModel vm)
    {
      this.assignedDate = vm.AssignedStatusDate;
      this.assingedAmount = vm.AssignedAmount;
      this.investor = vm.InvestorName;
      this.investorCommitmentNumber = vm.InvestorCommitmentNumber;
      this.netProfit = vm.NetProfit;
      this.tradeAmount = vm.TradeAmount;
      this.tradeDescription = vm.TradeDescription;
      this.tradeId = vm.Name;
    }
  }
}
