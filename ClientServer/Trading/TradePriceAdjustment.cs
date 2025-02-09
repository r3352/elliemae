// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradePriceAdjustment
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class TradePriceAdjustment : TradeEntity, IXmlSerializable
  {
    private FieldFilterList criterionList;
    private Decimal adjustment;

    public TradePriceAdjustment()
    {
      this.criterionList = new FieldFilterList();
      this.adjustment = 0M;
    }

    public TradePriceAdjustment(FieldFilterList criterionList, Decimal adjustment)
    {
      this.criterionList = criterionList;
      this.adjustment = adjustment;
    }

    public TradePriceAdjustment(FieldFilter criterion, Decimal adjustment)
      : this(new FieldFilterList(new FieldFilter[1]
      {
        criterion
      }), adjustment)
    {
    }

    public TradePriceAdjustment(XmlSerializationInfo info)
    {
      this.ReadId(info);
      try
      {
        this.criterionList = new FieldFilterList(new FieldFilter[1]
        {
          (FieldFilter) info.GetValue("criterion", typeof (FieldFilter))
        });
      }
      catch
      {
      }
      try
      {
        this.criterionList = (FieldFilterList) info.GetValue("criterionlist", typeof (FieldFilterList));
      }
      catch
      {
      }
      this.adjustment = info.GetDecimal("adj");
    }

    public TradePriceAdjustment(TradePriceAdjustment source)
    {
      this.Id = source.Id;
      this.criterionList = new FieldFilterList(source.CriterionList);
      this.adjustment = source.adjustment;
    }

    public FieldFilterList CriterionList
    {
      get => this.criterionList;
      set => this.criterionList = value;
    }

    public Decimal PriceAdjustment
    {
      get => this.adjustment;
      set => this.adjustment = value;
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      this.WriteId(info);
      info.AddValue("criterionlist", (object) this.criterionList);
      info.AddValue("adj", (object) this.adjustment);
    }

    public override string ToString()
    {
      return this.criterionList.ToString() + "; Price:" + this.adjustment.ToString();
    }
  }
}
