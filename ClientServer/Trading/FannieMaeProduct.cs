// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.FannieMaeProduct
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
  public class FannieMaeProduct : IXmlSerializable
  {
    private int index = -1;
    private string productName = "";
    private string displayName = "";
    private string productDescription = "";
    private TradeAdvancedPricingItems pricingItems = new TradeAdvancedPricingItems();
    private MbsPoolBuyUpDownItems buyUpDownItems = new MbsPoolBuyUpDownItems();

    public FannieMaeProduct()
    {
    }

    public FannieMaeProduct(
      int index,
      string productName,
      string displayName,
      string productDescription,
      TradeAdvancedPricingItems pricingItems,
      MbsPoolBuyUpDownItems buyUpDownItems)
    {
      this.index = index;
      this.productName = productName;
      this.displayName = displayName;
      this.productDescription = productDescription;
      this.pricingItems = pricingItems == null ? this.pricingItems : pricingItems;
      this.buyUpDownItems = buyUpDownItems == null ? this.buyUpDownItems : buyUpDownItems;
    }

    public FannieMaeProduct(FannieMaeProduct source)
    {
      this.index = source.Index;
      this.productName = source.productName;
      this.displayName = source.displayName;
      this.productDescription = source.productDescription;
      this.pricingItems = source.PricingItems;
      this.buyUpDownItems = source.buyUpDownItems;
    }

    public FannieMaeProduct(XmlSerializationInfo info)
    {
      this.index = info.GetInteger(nameof (index));
      this.productName = info.GetString(nameof (productName));
      this.displayName = info.GetString(nameof (displayName));
      this.productDescription = info.GetString(nameof (productDescription));
      try
      {
        this.pricingItems = BinaryConvertible<TradeAdvancedPricingItems>.Parse(info.GetString("pricingItem"));
      }
      catch (Exception ex)
      {
        this.pricingItems = new TradeAdvancedPricingItems();
      }
      try
      {
        this.buyUpDownItems = BinaryConvertible<MbsPoolBuyUpDownItems>.Parse(info.GetString("buyUpDownItem"));
      }
      catch (Exception ex)
      {
        this.buyUpDownItems = new MbsPoolBuyUpDownItems();
      }
    }

    public int Index => this.index;

    public string ProductName
    {
      get => this.productName;
      set => this.productName = value;
    }

    public string DisplayName
    {
      get => this.displayName;
      set => this.displayName = value;
    }

    public string ProductDescription
    {
      get => this.productDescription;
      set => this.productDescription = value;
    }

    public TradeAdvancedPricingItems PricingItems
    {
      get => this.pricingItems;
      set => this.pricingItems = value;
    }

    public MbsPoolBuyUpDownItems BuyUpDownItems
    {
      get => this.buyUpDownItems;
      set => this.buyUpDownItems = value;
    }

    internal void SetIndex(int index) => this.index = index;

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("index", (object) this.index);
      info.AddValue("productName", (object) this.productName);
      info.AddValue("displayName", (object) this.displayName);
      info.AddValue("productDescription", (object) this.productDescription);
      info.AddValue("pricingItem", (object) this.pricingItems.ToXml());
      info.AddValue("buyUpDownItem", (object) this.buyUpDownItems.ToXml());
    }
  }
}
