// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradePricingItems
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class TradePricingItems : 
    BinaryConvertible<TradePricingItems>,
    IEnumerable<TradePricingItem>,
    IEnumerable
  {
    private XmlList<TradePricingItem> items = new XmlList<TradePricingItem>();

    internal TradePricingItems()
    {
    }

    internal TradePricingItems(TradePricingItems source)
    {
      foreach (TradePricingItem source1 in source)
        this.items.Add(new TradePricingItem(source1));
    }

    public TradePricingItems(XmlSerializationInfo info)
    {
      this.items = (XmlList<TradePricingItem>) info.GetValue(nameof (items), typeof (XmlList<TradePricingItem>));
    }

    public void Add(TradePricingItem item)
    {
      int index;
      for (index = 0; index < this.items.Count; ++index)
      {
        if (this.items[index].Rate == item.Rate)
          throw new ArgumentException("A price for the rate " + (object) item.Rate + " already exists.");
        if (this.items[index].Rate > item.Rate)
          break;
      }
      if (index >= this.items.Count)
        this.items.Add(item);
      else
        this.items.Insert(index, item);
    }

    public void Clear() => this.items.Clear();

    public int Count => this.items.Count;

    public TradePricingItem this[int index] => this.items[index];

    public Decimal GetPriceExact(Decimal rate)
    {
      for (int index = 0; index < this.items.Count; ++index)
      {
        if (this.items[index].Rate == rate)
          return this.items[index].Price;
        if (this.items[index].Rate > rate)
          break;
      }
      return 0M;
    }

    public Decimal GetPriceFloor(Decimal rate)
    {
      if (this.items.Count == 0 || this.items[this.items.Count].Rate < rate)
        return 0M;
      for (int index = this.items.Count - 1; index > 0; --index)
      {
        if (this.items[index - 1].Rate < rate)
          return this.items[index].Price;
      }
      return this.items[0].Price;
    }

    public Decimal GetPriceCeiling(Decimal rate)
    {
      if (this.items.Count == 0 || this.items[0].Rate > rate)
        return 0M;
      for (int index = 0; index < this.items.Count - 1; ++index)
      {
        if (this.items[index + 1].Rate > rate)
          return this.items[index].Price;
      }
      return this.items[this.items.Count - 1].Price;
    }

    public IEnumerator<TradePricingItem> GetEnumerator()
    {
      return (IEnumerator<TradePricingItem>) this.items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("items", (object) this.items);
    }

    public static explicit operator TradePricingItems(BinaryObject o)
    {
      return (TradePricingItems) BinaryConvertibleObject.Parse(o, typeof (TradePricingItems));
    }
  }
}
