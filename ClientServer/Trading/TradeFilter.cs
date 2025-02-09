// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeFilter
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class TradeFilter : BinaryConvertible<TradeFilter>
  {
    private TradeFilterType filterType;
    private IXmlSerializable filter;
    private TableLayout dataLayout;

    public TradeFilter(FieldFilterList advancedFilters)
      : this(advancedFilters, (TableLayout) null)
    {
    }

    public TradeFilter(FieldFilterList advancedFilters, TableLayout dataLayout)
    {
      if (advancedFilters == null)
        throw new ArgumentNullException("Filter set cannot be empty or null", nameof (advancedFilters));
      this.filterType = TradeFilterType.Advanced;
      this.filter = (IXmlSerializable) new FieldFilterList(advancedFilters);
      this.dataLayout = dataLayout;
    }

    public TradeFilter(SimpleTradeFilter simpleFilter)
      : this(simpleFilter, (TableLayout) null)
    {
    }

    public TradeFilter(SimpleTradeFilter simpleFilter, TableLayout dataLayout)
    {
      if (simpleFilter == null)
        throw new ArgumentNullException("Filter set cannot null", nameof (simpleFilter));
      this.filterType = TradeFilterType.Simple;
      this.filter = (IXmlSerializable) new SimpleTradeFilter(simpleFilter);
      this.dataLayout = dataLayout;
    }

    public TradeFilter(XmlSerializationInfo info)
    {
      this.filterType = (TradeFilterType) info.GetValue(nameof (filterType), typeof (TradeFilterType));
      this.filter = this.filterType != TradeFilterType.Advanced ? (this.filterType != TradeFilterType.Simple ? (IXmlSerializable) null : (IXmlSerializable) info.GetValue(nameof (filter), typeof (SimpleTradeFilter))) : (IXmlSerializable) info.GetValue(nameof (filter), typeof (FieldFilterList));
      try
      {
        this.dataLayout = (TableLayout) info.GetValue(nameof (dataLayout), typeof (TableLayout));
      }
      catch
      {
        this.dataLayout = (TableLayout) null;
      }
    }

    public TradeFilterType FilterType => this.filterType;

    public FieldFilterList GetAdvancedFilter()
    {
      if (this.FilterType != TradeFilterType.Advanced)
        throw new Exception("The filter is not an advanced filter");
      return new FieldFilterList((FieldFilterList) this.filter);
    }

    public SimpleTradeFilter GetSimpleFilter()
    {
      if (this.FilterType != TradeFilterType.Simple)
        throw new Exception("The filter is not an advanced filter");
      return new SimpleTradeFilter((SimpleTradeFilter) this.filter);
    }

    public bool MatchesAllUserCriteria(
      string fieldName,
      object value,
      FilterEvaluationOption options)
    {
      if (this.filterType == TradeFilterType.None)
        return true;
      return this.filterType == TradeFilterType.Advanced ? ((FieldFilterList) this.filter).MatchesAllCriteria(fieldName, value, options) : ((SimpleTradeFilter) this.filter).MatchesAllCriteria(fieldName, value, options);
    }

    public TableLayout DataLayout
    {
      get => this.dataLayout;
      set => this.dataLayout = value;
    }

    public IFilterEvaluator CreateEvaluator(Type type)
    {
      if (type == typeof (LoanTradeFilterEvaluator))
        return (IFilterEvaluator) new LoanTradeFilterEvaluator(this);
      if (type == typeof (MbsPoolFilterEvaluator))
        return (IFilterEvaluator) new MbsPoolFilterEvaluator(this);
      return type == typeof (CorrespondentTradeFilterEvaluator) ? (IFilterEvaluator) new CorrespondentTradeFilterEvaluator(this) : (IFilterEvaluator) null;
    }

    public string[] GetFieldList()
    {
      List<string> stringList = new List<string>((IEnumerable<string>) TradeFilterEvaluator.RequiredFields);
      string[] strArray1 = new string[0];
      string[] strArray2 = this.FilterType != TradeFilterType.Advanced ? this.GetSimpleFilter().GetFieldList() : this.GetAdvancedFilter().GetFieldList();
      for (int index = 0; index < strArray2.Length; ++index)
      {
        if (!stringList.Contains(strArray2[index]))
          stringList.Add(strArray2[index]);
      }
      return stringList.ToArray();
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("filterType", (object) this.filterType);
      info.AddValue("filter", (object) this.filter);
      info.AddValue("dataLayout", (object) this.dataLayout);
    }

    public static bool CompareFilters(TradeFilter filter1, TradeFilter filter2)
    {
      return filter1 == null || filter2 == null ? filter1 == filter2 : filter1.ToXml() == filter2.ToXml();
    }

    public static explicit operator TradeFilter(BinaryObject o)
    {
      return (TradeFilter) BinaryConvertibleObject.Parse(o, typeof (TradeFilter));
    }
  }
}
