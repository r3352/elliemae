// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.GSECommitmentPairOffs
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
  public class GSECommitmentPairOffs : 
    BinaryConvertible<GSECommitmentPairOffs>,
    IEnumerable<GSECommitmentPairOff>,
    IEnumerable
  {
    private XmlList<GSECommitmentPairOff> pairOffs = new XmlList<GSECommitmentPairOff>();

    public GSECommitmentPairOffs()
    {
    }

    internal GSECommitmentPairOffs(GSECommitmentPairOff[] pairOffs)
    {
      this.pairOffs.AddRange((IEnumerable<GSECommitmentPairOff>) pairOffs);
    }

    internal GSECommitmentPairOffs(GSECommitmentPairOffs source)
    {
      for (int index = 0; index < source.Count; ++index)
        this.pairOffs.Add(new GSECommitmentPairOff(source[index]));
    }

    public GSECommitmentPairOffs(XmlSerializationInfo info)
    {
      this.pairOffs = (XmlList<GSECommitmentPairOff>) info.GetValue(nameof (GSECommitmentPairOffs), typeof (XmlList<GSECommitmentPairOff>));
    }

    public void Add(GSECommitmentPairOff pairOff)
    {
      pairOff.Index = this.pairOffs.Count + 1;
      this.pairOffs.Add(pairOff);
    }

    public void Remove(GSECommitmentPairOff pairOff) => this.pairOffs.Remove(pairOff);

    public void Clear() => this.pairOffs.Clear();

    public int Count => this.pairOffs.Count;

    public GSECommitmentPairOff this[int index]
    {
      get => this.pairOffs[index];
      set => this.pairOffs[index] = value;
    }

    public Decimal GetTradeAmount()
    {
      Decimal tradeAmount = 0M;
      foreach (GSECommitmentPairOff pairOff in (List<GSECommitmentPairOff>) this.pairOffs)
        tradeAmount += pairOff.TradeAmount;
      return tradeAmount;
    }

    public Decimal GetDisplayTradeAmount() => Decimal.Negate(this.GetTradeAmount());

    public Decimal GetDisplayCalculatedPairOffFee()
    {
      Decimal calculatedPairOffFee = 0M;
      foreach (GSECommitmentPairOff pairOff in (List<GSECommitmentPairOff>) this.pairOffs)
        calculatedPairOffFee += pairOff.DisplayCalculatedPairOffFee;
      return calculatedPairOffFee;
    }

    public IEnumerator<GSECommitmentPairOff> GetEnumerator()
    {
      return (IEnumerator<GSECommitmentPairOff>) this.pairOffs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue(nameof (GSECommitmentPairOffs), (object) this.pairOffs);
    }
  }
}
