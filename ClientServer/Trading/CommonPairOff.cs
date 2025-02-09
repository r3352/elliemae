// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CommonPairOff
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class CommonPairOff : TradeEntity
  {
    private int index = -1;
    private DateTime date = DateTime.MinValue;
    private Decimal tradeAmt;
    private Decimal pairOffFeePercentage;
    protected Decimal fee;
    private bool locked;
    private string requestedBy = string.Empty;
    private string comments = string.Empty;

    public int Index
    {
      get => this.index;
      set => this.index = value;
    }

    public DateTime Date
    {
      get => this.date;
      set => this.date = value;
    }

    public Decimal TradeAmount
    {
      get => this.tradeAmt;
      set => this.tradeAmt = value;
    }

    public Decimal Fee => !this.locked ? 0M : this.fee;

    public bool Locked
    {
      get => this.locked;
      set => this.locked = value;
    }

    public Decimal CalculatedPairOffFee
    {
      get => Math.Round(this.pairOffFeePercentage * this.tradeAmt / 100M, 2);
    }

    public Decimal PairOffFeePercentage
    {
      get => this.pairOffFeePercentage;
      set => this.pairOffFeePercentage = value;
    }

    public void ReCalculatePairOffPercentage(Decimal calculatedPairOffAmount)
    {
      if (this.tradeAmt == 0M)
        this.pairOffFeePercentage = 0M;
      else
        this.pairOffFeePercentage = calculatedPairOffAmount * 100M / this.tradeAmt;
    }

    public Decimal DisplayedTradeAmount => Decimal.Negate(this.tradeAmt);

    public Decimal DisplayCalculatedPairOffFee
    {
      get => Math.Round(this.pairOffFeePercentage * this.tradeAmt / 100M, 2);
    }

    public string RequestedBy
    {
      get => this.requestedBy;
      set => this.requestedBy = value;
    }

    public string Comments
    {
      get => this.comments;
      set => this.comments = value;
    }
  }
}
