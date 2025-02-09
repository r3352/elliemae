// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentTradePairOffs
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
  public class CorrespondentTradePairOffs : 
    BinaryConvertible<CorrespondentTradePairOffs>,
    IEnumerable<CorrespondentTradePairOff>,
    IEnumerable
  {
    private XmlList<CorrespondentTradePairOff> pairOffs = new XmlList<CorrespondentTradePairOff>();

    internal CorrespondentTradePairOffs()
    {
    }

    internal CorrespondentTradePairOffs(CorrespondentTradePairOff[] pairOffs)
    {
      this.pairOffs.AddRange((IEnumerable<CorrespondentTradePairOff>) pairOffs);
    }

    internal CorrespondentTradePairOffs(CorrespondentTradePairOffs source)
    {
      for (int index = 0; index < source.Count; ++index)
        this.pairOffs.Add(new CorrespondentTradePairOff(source[index]));
    }

    public CorrespondentTradePairOffs(XmlSerializationInfo info)
    {
      try
      {
        this.pairOffs = (XmlList<CorrespondentTradePairOff>) info.GetValue("correspondentTradePairOffs", typeof (XmlList<CorrespondentTradePairOff>));
      }
      catch (Exception ex)
      {
        this.pairOffs = (XmlList<CorrespondentTradePairOff>) info.GetValue(nameof (pairOffs), typeof (XmlList<CorrespondentTradePairOff>));
      }
    }

    public void Add(CorrespondentTradePairOff pairOff)
    {
      pairOff.Index = this.pairOffs.Count + 1;
      this.pairOffs.Add(pairOff);
    }

    public void Remove(CorrespondentTradePairOff pairOff) => this.pairOffs.Remove(pairOff);

    public void Clear() => this.pairOffs.Clear();

    public int Count => this.pairOffs.Count;

    public CorrespondentTradePairOff this[int index]
    {
      get => this.pairOffs[index];
      set => this.pairOffs[index] = value;
    }

    public Decimal GetTradeAmount()
    {
      Decimal tradeAmount = 0M;
      foreach (CorrespondentTradePairOff pairOff in (List<CorrespondentTradePairOff>) this.pairOffs)
        tradeAmount += pairOff.TradeAmount;
      return tradeAmount;
    }

    public Decimal GetDisplayTradeAmount() => Decimal.Negate(this.GetTradeAmount());

    public Decimal GetDisplayCalculatedPairOffFee()
    {
      Decimal calculatedPairOffFee = 0M;
      foreach (CorrespondentTradePairOff pairOff in (List<CorrespondentTradePairOff>) this.pairOffs)
        calculatedPairOffFee += pairOff.DisplayCalculatedPairOffFee;
      return calculatedPairOffFee;
    }

    public IEnumerator<CorrespondentTradePairOff> GetEnumerator()
    {
      return (IEnumerator<CorrespondentTradePairOff>) this.pairOffs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("correspondentTradePairOffs", (object) this.pairOffs);
    }
  }
}
