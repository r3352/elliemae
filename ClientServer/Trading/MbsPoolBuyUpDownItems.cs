// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MbsPoolBuyUpDownItems
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
  public class MbsPoolBuyUpDownItems : 
    BinaryConvertible<MbsPoolBuyUpDownItems>,
    IEnumerable<MbsPoolBuyUpDownItem>,
    IEnumerable
  {
    private XmlList<MbsPoolBuyUpDownItem> items = new XmlList<MbsPoolBuyUpDownItem>();

    public MbsPoolBuyUpDownItems()
    {
    }

    internal MbsPoolBuyUpDownItems(MbsPoolBuyUpDownItems source)
    {
      foreach (MbsPoolBuyUpDownItem source1 in source)
        this.items.Add(new MbsPoolBuyUpDownItem(source1));
    }

    public MbsPoolBuyUpDownItems(XmlSerializationInfo info)
    {
      this.items = (XmlList<MbsPoolBuyUpDownItem>) info.GetValue(nameof (items), typeof (XmlList<MbsPoolBuyUpDownItem>));
    }

    public void Add(MbsPoolBuyUpDownItem item, bool isBuyUp)
    {
      int index = 0;
      if (index >= this.items.Count)
        this.items.Add(item);
      else
        this.items.Insert(index, item);
    }

    public void Clear() => this.items.Clear();

    public int Count => this.items.Count;

    public MbsPoolBuyUpDownItem this[int index] => this.items[index];

    public IEnumerator<MbsPoolBuyUpDownItem> GetEnumerator()
    {
      return (IEnumerator<MbsPoolBuyUpDownItem>) this.items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("items", (object) this.items);
    }

    public static explicit operator MbsPoolBuyUpDownItems(BinaryObject o)
    {
      return (MbsPoolBuyUpDownItems) BinaryConvertibleObject.Parse(o, typeof (MbsPoolBuyUpDownItems));
    }
  }
}
