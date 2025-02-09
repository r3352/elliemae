// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.GuarantyFeeItems
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
  public class GuarantyFeeItems : 
    BinaryConvertible<GuarantyFeeItems>,
    IEnumerable<GuarantyFeeItem>,
    IEnumerable
  {
    private XmlList<GuarantyFeeItem> items = new XmlList<GuarantyFeeItem>();

    public GuarantyFeeItems()
    {
    }

    internal GuarantyFeeItems(GuarantyFeeItems source)
    {
      foreach (GuarantyFeeItem source1 in source)
        this.items.Add(new GuarantyFeeItem(source1));
    }

    public GuarantyFeeItems(XmlSerializationInfo info)
    {
      this.items = (XmlList<GuarantyFeeItem>) info.GetValue(nameof (items), typeof (XmlList<GuarantyFeeItem>));
    }

    public void Add(GuarantyFeeItem item, bool isBuyUp)
    {
      int index = 0;
      if (index >= this.items.Count)
        this.items.Add(item);
      else
        this.items.Insert(index, item);
    }

    public void Clear() => this.items.Clear();

    public int Count => this.items.Count;

    public GuarantyFeeItem this[int index] => this.items[index];

    public IEnumerator<GuarantyFeeItem> GetEnumerator()
    {
      return (IEnumerator<GuarantyFeeItem>) this.items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("items", (object) this.items);
    }

    public static explicit operator GuarantyFeeItems(BinaryObject o)
    {
      return (GuarantyFeeItems) BinaryConvertibleObject.Parse(o, typeof (GuarantyFeeItems));
    }
  }
}
