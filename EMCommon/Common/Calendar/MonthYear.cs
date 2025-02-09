// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Calendar.MonthYear
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common.Calendar
{
  public class MonthYear
  {
    private int month;
    private int year;

    public MonthYear(int year, int month)
    {
      this.year = year;
      this.month = month;
    }

    public MonthYear(DateTime date)
    {
      this.year = date.Year;
      this.month = date.Month;
    }

    public int Year => this.year;

    public int Month => this.month;

    public override bool Equals(object obj)
    {
      MonthYear monthYear = obj as MonthYear;
      return (object) monthYear != null && monthYear.Year == this.Year && monthYear.Month == this.Month;
    }

    public override int GetHashCode() => this.year * 12 + this.month;

    public static bool operator ==(MonthYear a, MonthYear b)
    {
      if ((object) a == (object) b)
        return true;
      return (object) a != null && (object) b != null && a.Equals((object) b);
    }

    public static bool operator !=(MonthYear a, MonthYear b) => !(a == b);

    public static bool operator >(MonthYear a, MonthYear b)
    {
      return (object) a != null && (object) b != null && a.GetHashCode() > b.GetHashCode();
    }

    public static bool operator >=(MonthYear a, MonthYear b)
    {
      return (object) a != null && (object) b != null && a.GetHashCode() >= b.GetHashCode();
    }

    public static bool operator <(MonthYear a, MonthYear b)
    {
      return (object) a != null && (object) b != null && a.GetHashCode() < b.GetHashCode();
    }

    public static bool operator <=(MonthYear a, MonthYear b)
    {
      return (object) a != null && (object) b != null && a.GetHashCode() <= b.GetHashCode();
    }

    public override string ToString() => this.ToString("MMMM yyyy");

    public string ToString(string format)
    {
      return new DateTime(this.year, this.month, 1).ToString(format);
    }
  }
}
