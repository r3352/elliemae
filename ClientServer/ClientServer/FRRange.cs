// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.FRRange
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class FRRange : IFieldRuleDefinition, IXmlSerializable
  {
    private string lowerBound;
    private string upperBound;
    private bool isDateRange;

    public FRRange(string lowerBound, string upperBound)
    {
      this.lowerBound = (lowerBound ?? "").Trim();
      this.upperBound = (upperBound ?? "").Trim();
      bool flag = false;
      if (Utils.IsEmptyDate((object) this.lowerBound))
        flag = true;
      else if (Utils.IsEmptyDate((object) this.upperBound))
        flag = true;
      if (!flag)
      {
        try
        {
          DateTime date1 = Utils.ParseDate((object) this.lowerBound, true);
          if (date1 == DateTime.MinValue)
            throw new Exception("Invalid upper/lower bound");
          DateTime date2 = Utils.ParseDate((object) this.upperBound, true);
          if (date2 == DateTime.MinValue)
            throw new Exception("Invalid upper/lower bound");
          if ((int) (date2 - date1).TotalDays < 0)
            throw new Exception("Invalid upper/lower bound");
          this.isDateRange = true;
        }
        catch (FormatException ex)
        {
          flag = true;
        }
      }
      if (flag && (!this.isNumber(this.lowerBound) || !this.isNumber(this.upperBound)))
        throw new Exception("Invalid upper/lower bound");
    }

    public FRRange(XmlSerializationInfo info)
    {
      this.lowerBound = info.GetString(nameof (LowerBound));
      this.upperBound = info.GetString(nameof (UpperBound));
      this.isDateRange = info.GetBoolean(nameof (isDateRange));
    }

    public string LowerBound => this.lowerBound;

    public string UpperBound => this.upperBound;

    public bool IsDateRange
    {
      get => this.isDateRange;
      set => this.isDateRange = value;
    }

    private bool isNumber(string str)
    {
      if (str == "")
        return true;
      try
      {
        Convert.ToDouble(str);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public void Combine(FRRange range)
    {
      if (!this.isDateRange && range.IsDateRange)
        throw new Exception("Invalid combination");
      if (this.isDateRange && !range.IsDateRange)
        throw new Exception("Invalid combination");
      if (this.isDateRange && range.IsDateRange)
      {
        if (!range.IsLowerBoundNull && range.LowerBoundAsDate > this.LowerBoundAsDate)
          this.lowerBound = range.LowerBound;
        if (range.IsUpperBoundNull || !(range.UpperBoundAsDate < this.UpperBoundAsDate))
          return;
        this.upperBound = range.UpperBound;
      }
      else
      {
        if (!range.IsLowerBoundNull && range.LowerBoundAsDouble > this.LowerBoundAsDouble)
          this.lowerBound = range.LowerBound;
        if (range.IsUpperBoundNull || range.UpperBoundAsDouble >= this.UpperBoundAsDouble)
          return;
        this.upperBound = range.UpperBound;
      }
    }

    public bool IsLowerBoundNull => this.lowerBound == "";

    public bool IsUpperBoundNull => this.upperBound == "";

    public int LowerBoundAsInt
    {
      get => this.lowerBound == "" ? int.MinValue : Convert.ToInt32(this.lowerBound);
    }

    public int UpperBoundAsInt
    {
      get => this.upperBound == "" ? int.MaxValue : Convert.ToInt32(this.upperBound);
    }

    public double LowerBoundAsDouble
    {
      get => this.lowerBound == "" ? double.MinValue : Convert.ToDouble(this.lowerBound);
    }

    public double UpperBoundAsDouble
    {
      get => this.upperBound == null ? double.MaxValue : Convert.ToDouble(this.upperBound);
    }

    public DateTime LowerBoundAsDate
    {
      get => this.lowerBound == "" ? DateTime.MinValue : Utils.ParseDate((object) this.lowerBound);
    }

    public DateTime UpperBoundAsDate
    {
      get
      {
        return this.upperBound == null ? DateTime.MaxValue : Utils.ParseDate((object) this.upperBound);
      }
    }

    public BizRule.FieldRuleType RuleType => BizRule.FieldRuleType.Range;

    public override string ToString() => this.LowerBound + "\n" + this.UpperBound;

    public static FRRange Parse(string text)
    {
      string[] strArray = text.Split('\n');
      return strArray.Length == 2 ? new FRRange(strArray[0], strArray[1]) : throw new Exception("Field Rule: invalid range values");
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("LowerBound", (object) this.lowerBound);
      info.AddValue("UpperBound", (object) this.upperBound);
      info.AddValue("isDateRange", (object) this.isDateRange);
    }
  }
}
