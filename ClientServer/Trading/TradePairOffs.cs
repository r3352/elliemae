// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradePairOffs
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
  public class TradePairOffs : BinaryConvertible<TradePairOffs>, IEnumerable<PairOff>, IEnumerable
  {
    private XmlList<PairOff> pairOffs = new XmlList<PairOff>();

    public TradePairOffs()
    {
    }

    internal TradePairOffs(PairOff[] pairOffs)
    {
      this.pairOffs.AddRange((IEnumerable<PairOff>) pairOffs);
    }

    internal TradePairOffs(TradePairOffs source)
    {
      for (int index = 0; index < source.Count; ++index)
        this.pairOffs.Add(new PairOff(source[index]));
    }

    public TradePairOffs(XmlSerializationInfo info)
    {
      this.pairOffs = (XmlList<PairOff>) info.GetValue(nameof (pairOffs), typeof (XmlList<PairOff>));
    }

    public void Add(PairOff pairOff)
    {
      pairOff.SetIndex(this.pairOffs.Count + 1);
      this.pairOffs.Add(pairOff);
    }

    public void Clear() => this.pairOffs.Clear();

    public int Count => this.pairOffs.Count;

    public PairOff this[int index]
    {
      get => this.pairOffs[index];
      set => this.pairOffs[index] = value;
    }

    public Decimal GetUndelieveredAmount()
    {
      Decimal undelieveredAmount = 0M;
      foreach (PairOff pairOff in (List<PairOff>) this.pairOffs)
        undelieveredAmount += pairOff.UndeliveredAmount;
      return undelieveredAmount;
    }

    public IEnumerator<PairOff> GetEnumerator()
    {
      return (IEnumerator<PairOff>) this.pairOffs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("pairOffs", (object) this.pairOffs);
    }
  }
}
