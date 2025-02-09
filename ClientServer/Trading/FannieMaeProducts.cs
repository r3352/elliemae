// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.FannieMaeProducts
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
  public class FannieMaeProducts : 
    BinaryConvertible<FannieMaeProducts>,
    IEnumerable<FannieMaeProduct>,
    IEnumerable
  {
    private XmlList<FannieMaeProduct> products = new XmlList<FannieMaeProduct>();

    public FannieMaeProducts()
    {
    }

    internal FannieMaeProducts(FannieMaeProduct[] pairOffs)
    {
      this.products.AddRange((IEnumerable<FannieMaeProduct>) pairOffs);
    }

    internal FannieMaeProducts(FannieMaeProducts source)
    {
      for (int index = 0; index < source.Count; ++index)
        this.products.Add(new FannieMaeProduct(source[index]));
    }

    public FannieMaeProducts(XmlSerializationInfo info)
    {
      this.products = (XmlList<FannieMaeProduct>) info.GetValue(nameof (products), typeof (XmlList<FannieMaeProduct>));
    }

    public void Add(FannieMaeProduct pairOff)
    {
      pairOff.SetIndex(this.products.Count + 1);
      this.products.Add(pairOff);
    }

    public void Clear() => this.products.Clear();

    public int Count => this.products.Count;

    public FannieMaeProduct this[int index]
    {
      get => this.products[index];
      set => this.products[index] = value;
    }

    public IEnumerator<FannieMaeProduct> GetEnumerator()
    {
      return (IEnumerator<FannieMaeProduct>) this.products.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("products", (object) this.products);
    }
  }
}
