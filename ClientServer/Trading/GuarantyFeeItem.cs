// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.GuarantyFeeItem
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class GuarantyFeeItem : TradeEntity, IXmlSerializable
  {
    public string ProductName { get; set; }

    public Decimal CouponMin { get; set; }

    public Decimal? CouponMax { get; set; }

    public Decimal GuarantyFee { get; set; }

    public Decimal CPA { get; set; }

    public GuarantyFeeItem()
    {
    }

    public GuarantyFeeItem(GuarantyFeeItem source)
    {
      this.ProductName = source.ProductName;
      this.CouponMin = source.CouponMin;
      this.CouponMax = source.CouponMax;
      this.GuarantyFee = source.GuarantyFee;
      this.CPA = source.CPA;
    }

    public GuarantyFeeItem(XmlSerializationInfo info)
    {
      this.ReadId(info);
      this.ProductName = info.GetString(nameof (ProductName));
      this.CouponMin = info.GetDecimal(nameof (CouponMin));
      this.CouponMax = string.IsNullOrEmpty(info.GetString(nameof (CouponMax))) ? new Decimal?(this.CouponMin) : new Decimal?(info.GetDecimal(nameof (CouponMax)));
      this.GuarantyFee = info.GetDecimal(nameof (GuarantyFee));
      if (info.GetValue(nameof (CPA), typeof (string), (object) null) != null)
        this.CPA = info.GetDecimal(nameof (CPA));
      else
        this.CPA = 0M;
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      this.WriteId(info);
      info.AddValue("ProductName", (object) this.ProductName);
      info.AddValue("CouponMin", (object) this.CouponMin);
      info.AddValue("CouponMax", (object) this.CouponMax);
      info.AddValue("GuarantyFee", (object) this.GuarantyFee);
      info.AddValue("CPA", (object) this.CPA);
    }

    public string ValidateDataRange() => string.Empty;

    public string ValidateDataFormat(string fieldName, string value) => string.Empty;

    public string ValidateDataFormat() => string.Empty;
  }
}
