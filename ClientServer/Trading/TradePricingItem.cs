// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradePricingItem
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class TradePricingItem : TradeEntity, IXmlSerializable
  {
    private Decimal rate;
    private Decimal price = 100M;
    private Decimal serviceFee;

    public TradePricingItem()
    {
    }

    public TradePricingItem(Decimal rate, Decimal price, Decimal serviceFee = 0M)
    {
      this.rate = rate;
      this.price = price;
      this.serviceFee = serviceFee;
    }

    public TradePricingItem(TradePricingItem source)
    {
      this.Id = source.Id;
      this.rate = source.rate;
      this.price = source.price;
      this.serviceFee = source.serviceFee;
    }

    public TradePricingItem(XmlSerializationInfo info)
    {
      this.ReadId(info);
      this.rate = info.GetDecimal(nameof (rate));
      this.price = info.GetDecimal(nameof (price));
      this.serviceFee = info.GetDecimal(nameof (serviceFee), 0M);
    }

    public Decimal Rate
    {
      get => this.rate;
      set => this.rate = value;
    }

    public Decimal Price
    {
      get => this.price;
      set => this.price = value;
    }

    public Decimal ServiceFee
    {
      get => this.serviceFee;
      set => this.serviceFee = value;
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      this.WriteId(info);
      info.AddValue("rate", (object) this.rate);
      info.AddValue("price", (object) this.price);
      info.AddValue("serviceFee", (object) this.serviceFee);
    }
  }
}
