// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradePriceAdjustments
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [CLSCompliant(true)]
  [Serializable]
  public class TradePriceAdjustments : XmlList<TradePriceAdjustment>
  {
    public TradePriceAdjustments()
    {
    }

    public TradePriceAdjustments(XmlSerializationInfo info)
      : base(info)
    {
    }

    public TradePriceAdjustments(TradePriceAdjustments source)
    {
      foreach (TradePriceAdjustment source1 in (List<TradePriceAdjustment>) source)
        this.Add(new TradePriceAdjustment(source1));
    }

    public void Append(TradePriceAdjustments source)
    {
      foreach (TradePriceAdjustment source1 in (List<TradePriceAdjustment>) source)
        this.Add(new TradePriceAdjustment(source1));
    }

    public static TradePriceAdjustments Parse(string xml)
    {
      return (xml ?? "") == "" ? (TradePriceAdjustments) null : (TradePriceAdjustments) new XmlSerializer().Deserialize(xml, typeof (TradePriceAdjustments));
    }
  }
}
