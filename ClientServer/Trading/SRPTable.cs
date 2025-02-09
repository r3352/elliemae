// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.SRPTable
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class SRPTable : BinaryConvertible<SRPTable>
  {
    private SRPTable.SRPPricingItems pricingItems = new SRPTable.SRPPricingItems();

    public SRPTable()
    {
    }

    public SRPTable(SRPTable source)
    {
      foreach (SRPTable.PricingItem pricingItem in source.pricingItems)
        this.pricingItems.Add(new SRPTable.PricingItem(pricingItem));
    }

    public SRPTable(SRPTable.SRPPricingItems source)
    {
      foreach (SRPTable.PricingItem source1 in source)
        this.pricingItems.Add(new SRPTable.PricingItem(source1));
    }

    public SRPTable(XmlSerializationInfo info)
    {
      this.pricingItems = (SRPTable.SRPPricingItems) info.GetValue(nameof (pricingItems), typeof (SRPTable.SRPPricingItems));
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("pricingItems", (object) this.pricingItems);
    }

    public SRPTable.SRPPricingItems PricingItems => this.pricingItems;

    public Decimal GetAdjustmentForLoan(PipelineInfo info)
    {
      return this.GetAdjustmentForLoan(Utils.ParseDecimal(info.GetField("TotalLoanAmount")), string.Concat(info.GetField("EscrowWaived")) == "Y", string.Concat(info.GetField("State")));
    }

    public Decimal GetAdjustmentForLoan(Decimal loanAmount, bool impoundsWaived, string state)
    {
      SRPTable.PricingItem pricingForLoanAmount = this.pricingItems.GetPricingForLoanAmount(loanAmount);
      if (pricingForLoanAmount == null)
        return 0M;
      Decimal baseAdjustment = pricingForLoanAmount.BaseAdjustment;
      if (impoundsWaived)
        baseAdjustment += pricingForLoanAmount.ImpoundsAdjustment;
      Decimal adjustmentForLoan = baseAdjustment + pricingForLoanAmount.StateAdjustments.GetAdjustmentForState(state);
      if (impoundsWaived)
        adjustmentForLoan += pricingForLoanAmount.StateAdjustments.GetImpoundAdjustmentForState(state);
      return adjustmentForLoan;
    }

    public static explicit operator SRPTable(BinaryObject o)
    {
      return (SRPTable) BinaryConvertibleObject.Parse(o, typeof (SRPTable));
    }

    [Serializable]
    public class PricingItem : TradeEntity, IXmlSerializable
    {
      private Range<Decimal> loanAmount;
      private Decimal baseAdjustment;
      private Decimal impoundsAdjustment;
      private SRPTable.SRPStateAdjustments stateAdjustments = new SRPTable.SRPStateAdjustments();

      public PricingItem(
        Range<Decimal> loanAmount,
        Decimal baseAdjustment,
        Decimal impoundsAdjustment)
      {
        this.loanAmount = loanAmount;
        this.baseAdjustment = baseAdjustment;
        this.impoundsAdjustment = impoundsAdjustment;
      }

      public PricingItem(
        Guid id,
        Range<Decimal> loanAmount,
        Decimal baseAdjustment,
        Decimal impoundsAdjustment)
      {
        if (id != Guid.Empty)
          this.Id = id;
        this.loanAmount = loanAmount;
        this.baseAdjustment = baseAdjustment;
        this.impoundsAdjustment = impoundsAdjustment;
      }

      public PricingItem(
        Guid id,
        Range<Decimal> loanAmount,
        Decimal baseAdjustment,
        Decimal impoundsAdjustment,
        SRPTable.SRPStateAdjustments stateAdjustments)
      {
        if (id != Guid.Empty)
          this.Id = id;
        this.loanAmount = loanAmount;
        this.baseAdjustment = baseAdjustment;
        this.impoundsAdjustment = impoundsAdjustment;
        this.stateAdjustments = stateAdjustments;
      }

      public PricingItem(SRPTable.PricingItem source)
      {
        this.Id = source.Id;
        this.loanAmount = source.loanAmount;
        this.baseAdjustment = source.baseAdjustment;
        this.impoundsAdjustment = source.impoundsAdjustment;
        foreach (SRPTable.StateAdjustment stateAdjustment in source.stateAdjustments)
          this.stateAdjustments.Add(new SRPTable.StateAdjustment(stateAdjustment));
      }

      public PricingItem(XmlSerializationInfo info)
      {
        this.ReadId(info);
        this.loanAmount = (Range<Decimal>) info.GetValue("range", typeof (Range<Decimal>));
        this.baseAdjustment = info.GetDecimal("base");
        this.impoundsAdjustment = info.GetDecimal("impounds");
        this.stateAdjustments = (SRPTable.SRPStateAdjustments) info.GetValue(nameof (stateAdjustments), typeof (SRPTable.SRPStateAdjustments));
      }

      public void GetXmlObjectData(XmlSerializationInfo info)
      {
        this.WriteId(info);
        info.AddValue("range", (object) this.loanAmount);
        info.AddValue("base", (object) this.baseAdjustment);
        info.AddValue("impounds", (object) this.impoundsAdjustment);
        info.AddValue("stateAdjustments", (object) this.stateAdjustments);
      }

      public Range<Decimal> LoanAmount => this.loanAmount;

      public Decimal BaseAdjustment
      {
        get => this.baseAdjustment;
        set => this.baseAdjustment = value;
      }

      public Decimal ImpoundsAdjustment
      {
        get => this.impoundsAdjustment;
        set => this.impoundsAdjustment = value;
      }

      public SRPTable.SRPStateAdjustments StateAdjustments => this.stateAdjustments;

      public bool AppliesTo(Decimal loanAmount) => this.LoanAmount.IsInRange(loanAmount);
    }

    [Serializable]
    public class StateAdjustment : TradeEntity, IXmlSerializable
    {
      public const string AllStates = "ALL�";
      private string state;
      private Decimal adjustment;
      private Decimal impoundAdjustment;

      public StateAdjustment(string state, Decimal adjustment, Decimal impoundAdjustment)
      {
        this.state = state;
        this.adjustment = adjustment;
        this.impoundAdjustment = impoundAdjustment;
      }

      public StateAdjustment(Guid id, string state, Decimal adjustment, Decimal impoundAdjustment)
      {
        if (id != Guid.Empty)
          this.Id = id;
        this.state = state;
        this.adjustment = adjustment;
        this.impoundAdjustment = impoundAdjustment;
      }

      public StateAdjustment(SRPTable.StateAdjustment source)
      {
        this.Id = source.Id;
        this.state = source.state;
        this.adjustment = source.adjustment;
        this.impoundAdjustment = source.impoundAdjustment;
      }

      public StateAdjustment(XmlSerializationInfo info)
      {
        this.ReadId(info);
        this.state = info.GetString(nameof (state));
        this.adjustment = info.GetDecimal("adj");
        try
        {
          this.impoundAdjustment = info.GetDecimal("impoundAdj");
        }
        catch
        {
          this.impoundAdjustment = 0M;
        }
      }

      public void GetXmlObjectData(XmlSerializationInfo info)
      {
        this.WriteId(info);
        info.AddValue("state", (object) this.state);
        info.AddValue("adj", (object) this.adjustment);
        info.AddValue("impoundAdj", (object) this.impoundAdjustment);
      }

      public string State => this.state;

      public Decimal Adjustment
      {
        get => this.adjustment;
        set => this.adjustment = value;
      }

      public Decimal ImpoundAdjustment
      {
        get => this.impoundAdjustment;
        set => this.impoundAdjustment = value;
      }
    }

    [Serializable]
    public class SRPPricingItems : IEnumerable<SRPTable.PricingItem>, IEnumerable, IXmlSerializable
    {
      private XmlList<SRPTable.PricingItem> items = new XmlList<SRPTable.PricingItem>();

      public SRPPricingItems()
      {
      }

      public SRPPricingItems(XmlSerializationInfo info)
      {
        this.items = (XmlList<SRPTable.PricingItem>) info.GetValue(nameof (items), typeof (XmlList<SRPTable.PricingItem>));
      }

      public void Add(SRPTable.PricingItem item)
      {
        int index = 0;
        foreach (SRPTable.PricingItem pricingItem in (List<SRPTable.PricingItem>) this.items)
        {
          if (!(item.LoanAmount.Maximum < pricingItem.LoanAmount.Minimum))
          {
            if (!(item.LoanAmount.Minimum > pricingItem.LoanAmount.Minimum))
              throw new ArgumentException("The specified range cannot be added because it overlaps an existing price range.");
            ++index;
          }
          else
            break;
        }
        this.items.Insert(index, item);
      }

      public void Remove(SRPTable.PricingItem item) => this.items.Remove(item);

      public void Clear() => this.items.Clear();

      public int Count => this.items.Count;

      public SRPTable.PricingItem this[int index] => this.items[index];

      public SRPTable.PricingItem GetPricingForLoanAmount(Decimal loanAmount)
      {
        loanAmount = Math.Round(loanAmount);
        foreach (SRPTable.PricingItem pricingForLoanAmount in this)
        {
          if (pricingForLoanAmount.LoanAmount.IsInRange(loanAmount))
            return pricingForLoanAmount;
        }
        return (SRPTable.PricingItem) null;
      }

      public IEnumerator<SRPTable.PricingItem> GetEnumerator()
      {
        return (IEnumerator<SRPTable.PricingItem>) this.items.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.items.GetEnumerator();

      public void GetXmlObjectData(XmlSerializationInfo info)
      {
        info.AddValue("items", (object) this.items);
      }
    }

    [Serializable]
    public class SRPStateAdjustments : 
      IEnumerable<SRPTable.StateAdjustment>,
      IEnumerable,
      IXmlSerializable
    {
      private XmlDictionary<SRPTable.StateAdjustment> items = new XmlDictionary<SRPTable.StateAdjustment>();

      public SRPStateAdjustments()
      {
      }

      public SRPStateAdjustments(XmlSerializationInfo info)
      {
        this.items = (XmlDictionary<SRPTable.StateAdjustment>) info.GetValue(nameof (items), typeof (XmlDictionary<SRPTable.StateAdjustment>));
      }

      public void Clear() => this.items.Clear();

      public int Count => this.items.Count;

      public void Add(SRPTable.StateAdjustment adj)
      {
        if (this.items.ContainsKey(adj.State))
          this.Remove(adj.State);
        if (!(adj.Adjustment != 0M) && !(adj.ImpoundAdjustment != 0M))
          return;
        this.items[adj.State] = adj;
      }

      public void Remove(string state)
      {
        if (!this.items.ContainsKey(state))
          return;
        this.items.Remove(state);
      }

      public Decimal GetAdjustmentForState(string state)
      {
        Decimal adjustmentForState = 0M;
        if (this.items.ContainsKey(state))
          adjustmentForState = this.items[state].Adjustment;
        if (adjustmentForState != 0M)
          return adjustmentForState;
        if (this.items.ContainsKey("ALL"))
          adjustmentForState = this.items["ALL"].Adjustment;
        return adjustmentForState != 0M ? adjustmentForState : 0M;
      }

      public Decimal GetImpoundAdjustmentForState(string state)
      {
        Decimal adjustmentForState = 0M;
        if (this.items.ContainsKey(state))
          adjustmentForState = this.items[state].ImpoundAdjustment;
        if (adjustmentForState != 0M)
          return adjustmentForState;
        if (this.items.ContainsKey("ALL"))
          adjustmentForState = this.items["ALL"].ImpoundAdjustment;
        return adjustmentForState != 0M ? adjustmentForState : 0M;
      }

      public IEnumerator<SRPTable.StateAdjustment> GetEnumerator()
      {
        return (IEnumerator<SRPTable.StateAdjustment>) this.items.Values.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.items.Values.GetEnumerator();

      public void GetXmlObjectData(XmlSerializationInfo info)
      {
        info.AddValue("items", (object) this.items);
      }
    }
  }
}
