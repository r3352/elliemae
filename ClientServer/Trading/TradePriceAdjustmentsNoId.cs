// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradePriceAdjustmentsNoId
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
  public class TradePriceAdjustmentsNoId : XmlList<TradePriceAdjustmentNoId>
  {
    public TradePriceAdjustmentsNoId()
    {
    }

    public TradePriceAdjustmentsNoId(XmlSerializationInfo info)
      : base(info)
    {
    }

    public TradePriceAdjustmentsNoId(TradePriceAdjustmentsNoId source)
    {
      foreach (TradePriceAdjustmentNoId source1 in (List<TradePriceAdjustmentNoId>) source)
        this.Add(new TradePriceAdjustmentNoId(source1));
    }

    public void Append(TradePriceAdjustmentsNoId source)
    {
      foreach (TradePriceAdjustmentNoId source1 in (List<TradePriceAdjustmentNoId>) source)
        this.Add(new TradePriceAdjustmentNoId(source1));
    }

    public static TradePriceAdjustmentsNoId Parse(string xml)
    {
      return (xml ?? "") == "" ? (TradePriceAdjustmentsNoId) null : (TradePriceAdjustmentsNoId) new XmlSerializer().Deserialize(xml, typeof (TradePriceAdjustmentsNoId));
    }
  }
}
