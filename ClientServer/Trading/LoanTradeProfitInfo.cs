// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.LoanTradeProfitInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class LoanTradeProfitInfo
  {
    private int tradeId;
    private string loanGuid;
    private ProfitCalculationStatus status;
    private Decimal profit;

    public LoanTradeProfitInfo(
      int tradeId,
      string loanGuid,
      Decimal profit,
      ProfitCalculationStatus status)
    {
      this.tradeId = tradeId;
      this.loanGuid = loanGuid;
      this.profit = profit;
      this.status = status;
    }

    public int TradeID => this.tradeId;

    public string LoanGuid => this.loanGuid;

    public Decimal Profit => this.profit;

    public ProfitCalculationStatus ProfitCalculationStatus => this.status;
  }
}
