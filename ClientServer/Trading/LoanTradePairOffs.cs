// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.LoanTradePairOffs
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
  public class LoanTradePairOffs : 
    BinaryConvertible<LoanTradePairOffs>,
    IEnumerable<LoanTradePairOff>,
    IEnumerable
  {
    private XmlList<LoanTradePairOff> pairOffs = new XmlList<LoanTradePairOff>();

    internal LoanTradePairOffs()
    {
    }

    internal LoanTradePairOffs(LoanTradePairOff[] pairOffs)
    {
      this.pairOffs.AddRange((IEnumerable<LoanTradePairOff>) pairOffs);
    }

    internal LoanTradePairOffs(LoanTradePairOffs source)
    {
      for (int index = 0; index < source.Count; ++index)
        this.pairOffs.Add(new LoanTradePairOff(source[index]));
    }

    public LoanTradePairOffs(XmlSerializationInfo info)
    {
      try
      {
        this.pairOffs = (XmlList<LoanTradePairOff>) info.GetValue("loanTradePairOffs", typeof (XmlList<LoanTradePairOff>));
      }
      catch (Exception ex)
      {
        this.pairOffs = (XmlList<LoanTradePairOff>) info.GetValue(nameof (pairOffs), typeof (XmlList<LoanTradePairOff>));
      }
    }

    public void Add(LoanTradePairOff pairOff)
    {
      pairOff.SetIndex(this.pairOffs.Count + 1);
      this.pairOffs.Add(pairOff);
    }

    public void Clear() => this.pairOffs.Clear();

    public int Count => this.pairOffs.Count;

    public LoanTradePairOff this[int index]
    {
      get => this.pairOffs[index];
      set => this.pairOffs[index] = value;
    }

    public Decimal GetTradeAmount()
    {
      Decimal tradeAmount = 0M;
      foreach (LoanTradePairOff pairOff in (List<LoanTradePairOff>) this.pairOffs)
        tradeAmount += pairOff.TradeAmount;
      return tradeAmount;
    }

    public Decimal GetDisplayTradeAmount() => Decimal.Negate(this.GetTradeAmount());

    public Decimal GetDisplayCalculatedPairOffFee()
    {
      Decimal calculatedPairOffFee = 0M;
      foreach (LoanTradePairOff pairOff in (List<LoanTradePairOff>) this.pairOffs)
        calculatedPairOffFee += pairOff.DisplayCalculatedPairOffFee;
      return calculatedPairOffFee;
    }

    public IEnumerator<LoanTradePairOff> GetEnumerator()
    {
      return (IEnumerator<LoanTradePairOff>) this.pairOffs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("loanTradePairOffs", (object) this.pairOffs);
    }
  }
}
