// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeAdvancedPricingInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [CLSCompliant(true)]
  [Serializable]
  public class TradeAdvancedPricingInfo : BinaryConvertible<TradeAdvancedPricingInfo>
  {
    private Decimal coupon;
    private Decimal guaranteeFee;
    private Decimal serviceFee;
    private Decimal price;
    private Decimal earlyDeliveryCredit;
    private Decimal negotiatedIncentive;
    private Decimal minServicingFee;
    private Decimal maxBU;
    private TradeAdvancedPricingItems advancedPricingItems;

    public TradeAdvancedPricingInfo()
    {
      this.advancedPricingItems = new TradeAdvancedPricingItems();
    }

    internal TradeAdvancedPricingInfo(
      Decimal guaranteeFee,
      Decimal serviceFee,
      Decimal earlyDeliveryCredit,
      Decimal negotiatedIncentive,
      TradeAdvancedPricingItems pricingItems,
      Decimal minServicingFee,
      Decimal maxBU)
    {
      this.guaranteeFee = guaranteeFee;
      this.serviceFee = serviceFee;
      this.earlyDeliveryCredit = earlyDeliveryCredit;
      this.negotiatedIncentive = negotiatedIncentive;
      this.advancedPricingItems = pricingItems;
      this.minServicingFee = minServicingFee;
      this.maxBU = maxBU;
    }

    public TradeAdvancedPricingInfo(XmlSerializationInfo info)
    {
      this.coupon = info.GetDecimal(nameof (Coupon), 0M);
      this.guaranteeFee = info.GetDecimal(nameof (GuaranteeFee), 0M);
      this.serviceFee = info.GetDecimal(nameof (ServiceFee), 0M);
      this.price = info.GetDecimal(nameof (Price), 0M);
      this.earlyDeliveryCredit = info.GetDecimal(nameof (EarlyDeliveryCredit), 0M);
      this.negotiatedIncentive = info.GetDecimal(nameof (NegotiatedIncentive), 0M);
      this.advancedPricingItems = BinaryConvertible<TradeAdvancedPricingItems>.Parse(info.GetString("AdvancedPricingItems"));
      this.minServicingFee = info.GetDecimal(nameof (MinServicingFee), 0M);
      this.maxBU = info.GetDecimal(nameof (MaxBU), 0M);
    }

    public Decimal Coupon
    {
      get => this.coupon;
      set => this.coupon = value;
    }

    public Decimal GuaranteeFee
    {
      get => this.guaranteeFee;
      set => this.guaranteeFee = value;
    }

    public Decimal ServiceFee
    {
      get => this.serviceFee;
      set => this.serviceFee = value;
    }

    public Decimal Price
    {
      get => this.price;
      set => this.price = value;
    }

    public Decimal EarlyDeliveryCredit
    {
      get => this.earlyDeliveryCredit;
      set => this.earlyDeliveryCredit = value;
    }

    public Decimal NegotiatedIncentive
    {
      get => this.negotiatedIncentive;
      set => this.negotiatedIncentive = value;
    }

    public TradeAdvancedPricingItems PricingItems
    {
      get => this.advancedPricingItems;
      set => this.advancedPricingItems = value;
    }

    public Decimal MinServicingFee
    {
      get => this.minServicingFee;
      set => this.minServicingFee = value;
    }

    public Decimal MaxBU
    {
      get => this.maxBU;
      set => this.maxBU = value;
    }

    public bool HasPricingSetting(Decimal noteRate)
    {
      return this.advancedPricingItems.GetPricingSetting(noteRate) != null;
    }

    public Decimal GetPricingForNoteRate(Decimal noteRate)
    {
      TradeAdvancedPricingItem pricingSetting = this.advancedPricingItems.GetPricingSetting(noteRate);
      return pricingSetting != null ? this.EarlyDeliveryCredit + this.NegotiatedIncentive + pricingSetting.TotalPrice : 0M;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("Coupon", (object) this.coupon);
      info.AddValue("GuaranteeFee", (object) this.guaranteeFee);
      info.AddValue("ServiceFee", (object) this.serviceFee);
      info.AddValue("Price", (object) this.price);
      info.AddValue("EarlyDeliveryCredit", (object) this.earlyDeliveryCredit);
      info.AddValue("NegotiatedIncentive", (object) this.negotiatedIncentive);
      info.AddValue("AdvancedPricingItems", (object) this.advancedPricingItems.ToXml());
      info.AddValue("MinServicingFee", (object) this.minServicingFee);
      info.AddValue("MaxBU", (object) this.maxBU);
    }

    public static Decimal CalculateTotalPrice(
      TradeAdvancedPricingItem tradeAdvancedPricingItem,
      Decimal price,
      Decimal earlyDeliveryCredit,
      Decimal negotiatedIncentive)
    {
      return price + earlyDeliveryCredit + negotiatedIncentive + tradeAdvancedPricingItem.TotalPrice;
    }
  }
}
