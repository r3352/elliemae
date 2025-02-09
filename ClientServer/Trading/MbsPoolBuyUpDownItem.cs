// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MbsPoolBuyUpDownItem
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class MbsPoolBuyUpDownItem : TradeEntity, IXmlSerializable
  {
    public Decimal GnrMin { get; set; }

    public Decimal GnrMax { get; set; }

    public Decimal Ratio { get; set; }

    public bool IsBuyUp { get; set; }

    public MbsPoolBuyUpDownItem()
    {
    }

    public MbsPoolBuyUpDownItem(MbsPoolBuyUpDownItem source)
    {
      this.GnrMin = source.GnrMin;
      this.GnrMax = source.GnrMax;
      this.Ratio = source.Ratio;
      this.IsBuyUp = source.IsBuyUp;
    }

    public MbsPoolBuyUpDownItem(XmlSerializationInfo info)
    {
      this.ReadId(info);
      this.GnrMin = info.GetDecimal(nameof (GnrMin));
      this.GnrMax = info.GetDecimal(nameof (GnrMax));
      this.Ratio = info.GetDecimal(nameof (Ratio));
      this.IsBuyUp = info.GetBoolean(nameof (IsBuyUp));
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      this.WriteId(info);
      info.AddValue("GnrMin", (object) this.GnrMin);
      info.AddValue("GnrMax", (object) this.GnrMax);
      info.AddValue("Ratio", (object) this.Ratio);
      info.AddValue("IsBuyUp", (object) this.IsBuyUp);
    }

    public string ValidateDataRange()
    {
      string empty = string.Empty;
      if (this.GnrMin > 100M || this.GnrMin < 0M)
        empty += "GNR Min must be between 0 and 100.\n";
      if (this.GnrMax > 100M || this.GnrMax < 0M)
        empty += "GNR Max must be between 0 and 100.\n";
      if (this.Ratio > 10M || this.Ratio < 0M)
        empty += this.IsBuyUp ? "Buy Up Ratio must be between 0 and 10.\n" : "Buy Down Ratio must be between 0 and 10.\n";
      if (this.GnrMin != 0M && this.GnrMax != 0M && this.GnrMin > this.GnrMax)
        empty += "GNR Max must be greater than GNR Min. \n";
      return empty;
    }

    public string ValidateDataFormat(string fieldName, string value)
    {
      string str = string.Empty;
      if (!Decimal.TryParse(value, out Decimal _))
      {
        switch (fieldName)
        {
          case "GnrMin":
            str = this.IsBuyUp ? "Invalid Data Format for BuyUp GNR Min\n" : "Invalid Data Format for BuyDown GNR Min\n";
            break;
          case "GnrMax":
            str = this.IsBuyUp ? "Invalid Data Format for BuyUp GNR Max\n" : "Invalid Data Format for BuyDown GNR Max\n";
            break;
          case "Ratio":
            str = this.IsBuyUp ? "Invalid Data Format for BuyUp Ratio\n" : "Invalid Data Format for BuyDown Ratio\n";
            break;
        }
      }
      return str;
    }

    public string ValidateDataFormat()
    {
      string empty = string.Empty;
      string str1 = this.ValidateDataFormat("GnrMin", this.GnrMin.ToString());
      if (str1 != string.Empty)
        return str1;
      string str2 = this.ValidateDataFormat("GnrMax", this.GnrMax.ToString());
      if (str2 != string.Empty)
        return str2;
      string str3 = this.ValidateDataFormat("Ratio", this.Ratio.ToString());
      int num = str3 != string.Empty ? 1 : 0;
      return str3;
    }
  }
}
