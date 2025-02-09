// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeAdvancedPricingItems
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
  [CLSCompliant(true)]
  [Serializable]
  public class TradeAdvancedPricingItems : 
    BinaryConvertible<TradeAdvancedPricingItems>,
    IEnumerable<TradeAdvancedPricingItem>,
    IEnumerable
  {
    private XmlList<TradeAdvancedPricingItem> items = new XmlList<TradeAdvancedPricingItem>();

    public TradeAdvancedPricingItems()
    {
    }

    internal TradeAdvancedPricingItems(TradeAdvancedPricingItems source)
    {
      foreach (TradeAdvancedPricingItem source1 in source)
        this.items.Add(new TradeAdvancedPricingItem(source1));
    }

    public TradeAdvancedPricingItems(XmlSerializationInfo info)
    {
      this.items = (XmlList<TradeAdvancedPricingItem>) info.GetValue(nameof (items), typeof (XmlList<TradeAdvancedPricingItem>));
    }

    public void Add(TradeAdvancedPricingItem item)
    {
      int index = 0;
      if (index >= this.items.Count)
        this.items.Add(item);
      else
        this.items.Insert(index, item);
    }

    public void Clear() => this.items.Clear();

    public int Count => this.items.Count;

    public TradeAdvancedPricingItem GetPricingSetting(Decimal noteRate)
    {
      foreach (TradeAdvancedPricingItem pricingSetting in this)
      {
        if (pricingSetting.NoteRate == noteRate)
          return pricingSetting;
      }
      return (TradeAdvancedPricingItem) null;
    }

    public TradeAdvancedPricingItem this[int index] => this.items[index];

    public IEnumerator<TradeAdvancedPricingItem> GetEnumerator()
    {
      return (IEnumerator<TradeAdvancedPricingItem>) this.items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("items", (object) this.items);
    }

    public static explicit operator TradeAdvancedPricingItems(BinaryObject o)
    {
      return (TradeAdvancedPricingItems) BinaryConvertibleObject.Parse(o, typeof (TradeAdvancedPricingItems));
    }
  }
}
