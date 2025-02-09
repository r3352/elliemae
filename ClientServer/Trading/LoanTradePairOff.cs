// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.LoanTradePairOff
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class LoanTradePairOff : IXmlSerializable
  {
    private int index = -1;
    private DateTime date = DateTime.MinValue;
    private Decimal tradeAmt;
    private Decimal pairOffFeePercentage;
    private Decimal fee;
    private bool locked;

    public LoanTradePairOff()
    {
    }

    public LoanTradePairOff(
      int index,
      DateTime date,
      Decimal tradeAmt,
      Decimal pairOffFeePercentage)
    {
      this.index = index;
      this.date = date;
      this.tradeAmt = tradeAmt;
      this.pairOffFeePercentage = pairOffFeePercentage;
      this.fee = this.CalculatedPairOffFee;
    }

    public LoanTradePairOff(LoanTradePairOff source)
    {
      this.index = source.Index;
      this.date = source.date;
      this.tradeAmt = source.tradeAmt;
      this.pairOffFeePercentage = source.pairOffFeePercentage;
      this.fee = source.fee;
    }

    public LoanTradePairOff(XmlSerializationInfo info)
    {
      this.index = info.GetInteger(nameof (index));
      this.date = info.GetDateTime(nameof (date));
      this.tradeAmt = info.GetDecimal("amt");
      try
      {
        this.pairOffFeePercentage = info.GetDecimal(nameof (pairOffFeePercentage));
      }
      catch (Exception ex)
      {
        this.pairOffFeePercentage = 0M;
      }
      this.fee = info.GetDecimal(nameof (fee));
      this.locked = info.GetBoolean(nameof (locked));
    }

    public int Index => this.index;

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

    public Decimal DisplayedTradeAmount => Decimal.Negate(this.tradeAmt);

    public Decimal PairOffFeePercentage
    {
      get => this.pairOffFeePercentage;
      set => this.pairOffFeePercentage = value;
    }

    public Decimal CalculatedPairOffFee
    {
      get => Math.Round(this.pairOffFeePercentage * this.tradeAmt / 100M, 2);
    }

    public Decimal DisplayCalculatedPairOffFee
    {
      get => Math.Round(this.pairOffFeePercentage * this.tradeAmt / 100M, 2);
    }

    public void ReCalculatePairOffPercentage(Decimal calculatedPairOffAmount)
    {
      if (this.tradeAmt == 0M)
        this.pairOffFeePercentage = 0M;
      else
        this.pairOffFeePercentage = calculatedPairOffAmount * 100M / this.tradeAmt;
    }

    public Decimal Fee => !this.locked ? 0M : this.fee;

    public bool Locked
    {
      get => this.locked;
      set => this.locked = value;
    }

    internal void SetIndex(int index) => this.index = index;

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("index", (object) this.index);
      info.AddValue("date", (object) this.date);
      info.AddValue("amt", (object) this.tradeAmt);
      info.AddValue("pairOffFeePercentage", (object) this.pairOffFeePercentage);
      info.AddValue("fee", (object) this.CalculatedPairOffFee);
      info.AddValue("locked", (object) this.locked);
    }
  }
}
