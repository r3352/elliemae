// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.HelocRateTable
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [CLSCompliant(true)]
  [Serializable]
  public class HelocRateTable : BinaryConvertibleObject
  {
    private Hashtable tbl = new Hashtable();
    private ArrayList yearList = new ArrayList();
    private bool isNewHELOC;
    private int indexDay;
    private int indexMonth;
    private bool useAlternateSchedule;
    private Decimal defaultHistoricMargin;
    private string indexName = "";
    private string decimalsUseForIndex = "ThreeDecimals";

    public HelocRateTable()
    {
    }

    public HelocRateTable(XmlSerializationInfo info)
    {
      foreach (string str in info)
      {
        if (string.Compare(str, nameof (IsNewHELOC), true) == 0)
          this.isNewHELOC = (bool) info.GetValue(str, typeof (bool));
        else if (string.Compare(str, nameof (IndexDay), true) == 0)
          this.indexDay = (int) info.GetValue(str, typeof (int));
        else if (string.Compare(str, nameof (IndexMonth), true) == 0)
          this.indexMonth = (int) info.GetValue(str, typeof (int));
        else if (string.Compare(str, nameof (IndexName), true) == 0)
          this.indexName = (string) info.GetValue(str, typeof (string));
        else if (string.Compare(str, nameof (UseAlternateSchedule), true) == 0)
          this.useAlternateSchedule = (bool) info.GetValue(str, typeof (bool));
        else if (string.Compare(str, nameof (DefaultHistoricMargin), true) == 0)
          this.defaultHistoricMargin = (Decimal) info.GetValue(str, typeof (Decimal));
        else if (string.Compare(str, nameof (DecimalsUseForIndex), true) == 0)
        {
          this.decimalsUseForIndex = (string) info.GetValue(str, typeof (string));
          if (string.IsNullOrEmpty(this.decimalsUseForIndex))
            this.decimalsUseForIndex = "ThreeDecimals";
        }
        else
          this.InsertYearRecord((HelocRateTable.YearRecord) info.GetValue(str, typeof (HelocRateTable.YearRecord)));
      }
    }

    public void InsertYearRecord(
      string year,
      string periodType,
      string indexRate,
      string marginRate,
      string apr,
      string minimumPayment)
    {
      if ((string.IsNullOrEmpty(this.decimalsUseForIndex) || string.Compare(this.decimalsUseForIndex, "ThreeDecimals", true) == 0) && !string.IsNullOrEmpty(indexRate) && indexRate.IndexOf(".") > -1 && indexRate.Substring(indexRate.IndexOf(".") + 1).Length == 5)
        indexRate = indexRate.Substring(0, indexRate.Length - 2);
      this.InsertYearRecord(new HelocRateTable.YearRecord(year, periodType, indexRate, marginRate, apr, minimumPayment));
    }

    public void InsertYearRecord(HelocRateTable.YearRecord rec)
    {
      if (this.tbl.ContainsKey((object) rec.Year))
        return;
      this.tbl.Add((object) rec.Year, (object) rec);
      this.yearList.Add((object) rec.Year);
    }

    public void UpdateYearRecord(
      string oldYear,
      string newYear,
      string periodType,
      string indexRate,
      string marginRate,
      string apr,
      string minimumPayment)
    {
      this.yearList.Remove((object) oldYear);
      HelocRateTable.YearRecord yearRecord = (HelocRateTable.YearRecord) this.tbl[(object) oldYear];
      this.tbl.Remove((object) oldYear);
      yearRecord.Year = newYear;
      yearRecord.PeriodType = periodType;
      yearRecord.IndexRate = indexRate;
      yearRecord.MarginRate = marginRate;
      yearRecord.APR = apr;
      yearRecord.MinimumPayment = minimumPayment;
      if (this.tbl.ContainsKey((object) yearRecord.Year))
        return;
      this.tbl.Add((object) newYear, (object) yearRecord);
      this.yearList.Add((object) newYear);
    }

    public int Count => this.tbl.Count;

    public bool IsNewHELOC
    {
      get => this.isNewHELOC;
      set => this.isNewHELOC = value;
    }

    public int IndexDay
    {
      get => this.indexDay;
      set => this.indexDay = value;
    }

    public int IndexMonth
    {
      get => this.indexMonth;
      set => this.indexMonth = value;
    }

    public bool UseAlternateSchedule
    {
      get => this.useAlternateSchedule;
      set => this.useAlternateSchedule = value;
    }

    public Decimal DefaultHistoricMargin
    {
      get => this.defaultHistoricMargin;
      set => this.defaultHistoricMargin = value;
    }

    public string IndexName
    {
      get => this.indexName;
      set => this.indexName = value;
    }

    public string DecimalsUseForIndex
    {
      get => this.decimalsUseForIndex;
      set => this.decimalsUseForIndex = value;
    }

    public HelocRateTable.YearRecord GetYearRecordAt(int i)
    {
      return (HelocRateTable.YearRecord) this.tbl[this.yearList[i]];
    }

    public HelocRateTable.YearRecord GetYearRecord(string year)
    {
      return (HelocRateTable.YearRecord) this.tbl[(object) year];
    }

    public bool YearRecordExists(string year) => this.tbl.ContainsKey((object) year);

    public void RemoveYear(string year)
    {
      this.tbl.Remove((object) year);
      this.yearList.Remove((object) year);
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("IsNewHELOC", (object) this.isNewHELOC);
      info.AddValue("IndexDay", (object) this.indexDay);
      info.AddValue("IndexMonth", (object) this.indexMonth);
      info.AddValue("IndexName", (object) this.indexName);
      info.AddValue("UseAlternateSchedule", (object) this.useAlternateSchedule);
      info.AddValue("DefaultHistoricMargin", (object) this.defaultHistoricMargin);
      info.AddValue("DecimalsUseForIndex", string.IsNullOrEmpty(this.decimalsUseForIndex) ? (object) "ThreeDecimals" : (object) this.decimalsUseForIndex);
      for (int index = 0; index < this.yearList.Count; ++index)
        info.AddValue(index.ToString(), this.tbl[this.yearList[index]]);
    }

    public static explicit operator HelocRateTable(BinaryObject obj)
    {
      return obj == null ? (HelocRateTable) null : (HelocRateTable) BinaryConvertibleObject.Parse(obj, typeof (HelocRateTable));
    }

    [Serializable]
    public class YearRecord : IXmlSerializable
    {
      public string Year;
      public string PeriodType;
      public string IndexRate;
      public string MarginRate;
      public string APR;
      public string MinimumPayment;

      public YearRecord(
        string year,
        string periodType,
        string indexRate,
        string marginRate,
        string apr,
        string minimumPayment)
      {
        this.Year = year;
        this.PeriodType = periodType;
        this.IndexRate = indexRate;
        this.MarginRate = marginRate;
        this.APR = apr;
        this.MinimumPayment = minimumPayment;
      }

      public YearRecord(XmlSerializationInfo info)
      {
        this.Year = info.GetString(nameof (Year));
        this.PeriodType = info.GetString(nameof (PeriodType));
        this.IndexRate = info.GetString(nameof (IndexRate));
        this.MarginRate = info.GetString(nameof (MarginRate));
        this.APR = info.GetString(nameof (APR));
        this.MinimumPayment = info.GetString("MinimumMonthlyPayment");
      }

      public void GetXmlObjectData(XmlSerializationInfo info)
      {
        info.AddValue("Year", (object) this.Year);
        info.AddValue("PeriodType", (object) this.PeriodType);
        info.AddValue("IndexRate", (object) this.IndexRate);
        info.AddValue("MarginRate", (object) this.MarginRate);
        info.AddValue("APR", (object) this.APR);
        info.AddValue("MinimumMonthlyPayment", (object) this.MinimumPayment);
      }
    }
  }
}
