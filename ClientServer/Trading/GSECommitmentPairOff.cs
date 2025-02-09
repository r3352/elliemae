// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.GSECommitmentPairOff
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class GSECommitmentPairOff : CommonPairOff, IXmlSerializable
  {
    public GSECommitmentPairOff()
    {
    }

    public GSECommitmentPairOff(
      int index,
      DateTime date,
      Decimal tradeAmt,
      Decimal pairOffFeePercentage)
    {
      this.Index = index;
      this.Date = date;
      this.TradeAmount = tradeAmt;
      this.PairOffFeePercentage = pairOffFeePercentage;
      this.fee = this.CalculatedPairOffFee;
    }

    public GSECommitmentPairOff(GSECommitmentPairOff source)
    {
      this.Index = source.Index;
      this.Date = source.Date;
      this.TradeAmount = source.TradeAmount;
      this.PairOffFeePercentage = source.PairOffFeePercentage;
      this.fee = source.fee;
    }

    public GSECommitmentPairOff(XmlSerializationInfo info)
    {
      this.ReadId(info);
      this.Index = info.GetInteger("index");
      this.Date = info.GetDateTime("date");
      this.TradeAmount = info.GetDecimal("amt");
      try
      {
        this.PairOffFeePercentage = info.GetDecimal("pairOffFeePercentage");
      }
      catch (Exception ex)
      {
        this.PairOffFeePercentage = 0M;
      }
      this.fee = info.GetDecimal("fee");
      this.Locked = info.GetBoolean("locked");
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      this.WriteId(info);
      info.AddValue("index", (object) this.Index);
      info.AddValue("date", (object) this.Date);
      info.AddValue("amt", (object) this.TradeAmount);
      info.AddValue("pairOffFeePercentage", (object) this.PairOffFeePercentage);
      info.AddValue("fee", (object) this.CalculatedPairOffFee);
      info.AddValue("locked", (object) this.Locked);
    }
  }
}
