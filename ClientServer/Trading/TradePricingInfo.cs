// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradePricingInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class TradePricingInfo : BinaryConvertible<TradePricingInfo>
  {
    private bool isAdvancedPricing;
    private TradePricingItems simplePricingItems;
    private TradeAdvancedPricingInfo advancedPricingInfo;
    private TradePricingItems msrPricingItems;

    public TradePricingInfo()
    {
      this.simplePricingItems = new TradePricingItems();
      this.advancedPricingInfo = new TradeAdvancedPricingInfo();
      this.msrPricingItems = new TradePricingItems();
    }

    public TradePricingInfo(
      bool isAdvancedPricing,
      TradePricingItems simplePricingItems,
      TradeAdvancedPricingInfo advancedPricingInfo)
    {
      this.isAdvancedPricing = isAdvancedPricing;
      this.simplePricingItems = simplePricingItems;
      this.advancedPricingInfo = advancedPricingInfo;
    }

    public TradePricingInfo(XmlSerializationInfo info)
    {
      try
      {
        this.isAdvancedPricing = info.GetBoolean(nameof (isAdvancedPricing));
      }
      catch
      {
        this.isAdvancedPricing = false;
        this.simplePricingItems = new TradePricingItems(info);
        this.advancedPricingInfo = new TradeAdvancedPricingInfo();
        this.msrPricingItems = new TradePricingItems(info);
        return;
      }
      this.simplePricingItems = BinaryConvertible<TradePricingItems>.Parse(info.GetString("SimplePricingSettings"));
      this.advancedPricingInfo = BinaryConvertible<TradeAdvancedPricingInfo>.Parse(info.GetString(nameof (AdvancedPricingInfo)));
      this.msrPricingItems = BinaryConvertible<TradePricingItems>.Parse(info.GetString("MSRPricingSettings", ""));
      if (this.msrPricingItems != null)
        return;
      this.msrPricingItems = new TradePricingItems();
    }

    public bool IsAdvancedPricing
    {
      get => this.isAdvancedPricing;
      set => this.isAdvancedPricing = value;
    }

    public TradePricingItems SimplePricingItems
    {
      get => this.simplePricingItems;
      set => this.simplePricingItems = value;
    }

    public TradeAdvancedPricingInfo AdvancedPricingInfo
    {
      get => this.advancedPricingInfo;
      set => this.advancedPricingInfo = value;
    }

    public TradePricingItems MSRPricingItems
    {
      get => this.msrPricingItems;
      set => this.msrPricingItems = value;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("isAdvancedPricing", (object) this.isAdvancedPricing);
      info.AddValue("SimplePricingSettings", (object) this.simplePricingItems.ToXml());
      info.AddValue("AdvancedPricingInfo", (object) this.advancedPricingInfo.ToXml());
      info.AddValue("MSRPricingSettings", (object) this.msrPricingItems.ToXml());
    }

    public static explicit operator TradePricingInfo(BinaryObject o)
    {
      return (TradePricingInfo) BinaryConvertibleObject.Parse(o, typeof (TradePricingInfo));
    }
  }
}
