// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DatetimeUtils
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class DatetimeUtils
  {
    private string format = "MM/dd/yyyy";
    private DateTime date;
    private DateTimeType type;
    private Hashtable holidays;

    public DatetimeUtils() => this.init(DateTime.Now, DateTimeType.Calendar);

    public DatetimeUtils(string dateTime)
    {
      this.init(Convert.ToDateTime(dateTime), DateTimeType.Calendar);
    }

    public DatetimeUtils(string dateTime, DateTimeType dateType)
    {
      this.init(Convert.ToDateTime(dateTime), dateType);
    }

    public DatetimeUtils(DateTime date) => this.init(date, DateTimeType.Calendar);

    public DatetimeUtils(DateTime date, DateTimeType dateType) => this.init(date, dateType);

    public DateTimeType DateType
    {
      get => this.type;
      set
      {
        this.type = value;
        this.check();
      }
    }

    public DateTime Date
    {
      get => this.date;
      set
      {
        this.date = value;
        this.check();
      }
    }

    public bool IsHoliday => this.holidays.ContainsValue((object) this.date.ToString(this.format));

    public bool IsWorkDay
    {
      get
      {
        return this.date.DayOfWeek != DayOfWeek.Saturday && this.date.DayOfWeek != DayOfWeek.Sunday && !this.IsHoliday;
      }
    }

    public void AddHours(short hours)
    {
      this.date = this.date.AddHours(Convert.ToDouble(hours));
      this.check();
    }

    public void AddDay()
    {
      this.date = this.date.AddDays(1.0);
      this.check();
    }

    public void AddDays(short days)
    {
      this.date = this.date.AddDays(Convert.ToDouble(days));
      this.check();
    }

    public void AddBusinessDays(short days)
    {
      double num1 = Convert.ToDouble(Math.Sign(days));
      int num2 = Math.Sign(days) * (int) days;
      for (int index = 0; index < num2; ++index)
      {
        do
        {
          this.date = this.date.AddDays(num1);
        }
        while (!this.IsWorkDay);
      }
    }

    public DateTime NextBusinessDay()
    {
      DateTime dateTime = this.date;
      do
      {
        dateTime = dateTime.AddDays(1.0);
      }
      while (dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday || this.holidays.ContainsValue((object) dateTime.ToString(this.format)));
      return dateTime;
    }

    public DateTime PreviousBusinessDay()
    {
      DateTime dateTime = this.date;
      do
      {
        dateTime = dateTime.AddDays(-1.0);
      }
      while (dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday || this.holidays.ContainsValue((object) dateTime.ToString(this.format)));
      return dateTime;
    }

    public int NumberOfDaysFrom(DateTime newDate)
    {
      int num = 0;
      bool flag = true;
      DateTime dateTime1 = this.date;
      DateTime dateTime2 = newDate;
      if (newDate < this.date)
      {
        dateTime1 = newDate;
        dateTime2 = this.date;
        flag = false;
      }
      while (dateTime1.ToString("d") != dateTime2.ToString("d"))
      {
        dateTime1 = dateTime1.AddDays(1.0);
        if (this.DateType == DateTimeType.Business || this.DateType == DateTimeType.Company)
        {
          if (dateTime1.DayOfWeek != DayOfWeek.Saturday && dateTime1.DayOfWeek != DayOfWeek.Sunday && !this.holidays.ContainsValue((object) dateTime1.ToString(this.format)))
          {
            if (flag)
              ++num;
            else
              --num;
          }
        }
        else if (flag)
          ++num;
        else
          --num;
      }
      return num;
    }

    public static DateTime GetDateTime(DateTime startDate, DateTimeType type, int numOfDays)
    {
      DatetimeUtils datetimeUtils = new DatetimeUtils(startDate, type);
      if (type == DateTimeType.Calendar)
        datetimeUtils.AddDays(short.Parse(string.Concat((object) numOfDays)));
      else
        datetimeUtils.AddBusinessDays(short.Parse(string.Concat((object) numOfDays)));
      return datetimeUtils.Date;
    }

    private void init(DateTime dateTime, DateTimeType dateType)
    {
      this.date = dateTime;
      this.type = dateType;
      this.initHolidays();
    }

    private void initHolidays() => this.holidays = new Hashtable();

    private void check()
    {
      if (this.type != DateTimeType.Business || this.IsWorkDay)
        return;
      this.date = this.NextBusinessDay();
    }
  }
}
