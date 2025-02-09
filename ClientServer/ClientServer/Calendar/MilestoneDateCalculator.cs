// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Calendar.MilestoneDateCalculator
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Calendar
{
  [Serializable]
  public class MilestoneDateCalculator : IMilestoneDateCalculator
  {
    private BusinessCalendar bizCalendar;
    private AutoDayCountSetting calendarType;
    private string className = nameof (MilestoneDateCalculator);
    private static string sw = Tracing.SwOutsideLoan;

    public MilestoneDateCalculator(BusinessCalendar bizCalendar, AutoDayCountSetting calendarType)
    {
      this.bizCalendar = bizCalendar;
      this.calendarType = calendarType;
    }

    public void AdjustMilestoneDateWithBusinessCalendar(
      int index,
      KeyValuePair<DateTime, int> getValue,
      LogList logList,
      MilestoneLog currentMilestoneLog)
    {
      int num = 0;
      DateTime key = getValue.Key;
      int dayCount = getValue.Value;
      DateTime date1 = logList.GetMilestoneAt(index).Date;
      if (key != DateTime.MinValue && key < date1)
      {
        for (DateTime date2 = key.Date; !date2.Date.Equals(date1.Date); date2 = date2.AddDays(1.0))
        {
          if (!this.bizCalendar.IsBusinessDay(date2) && !this.bizCalendar.IsWeekendDay(date2))
            --num;
        }
      }
      else
      {
        for (DateTime date3 = date1; !date3.Date.Equals(key.Date); date3 = date3.AddDays(1.0))
        {
          if (!this.bizCalendar.IsBusinessDay(date3) && !this.bizCalendar.IsWeekendDay(date3))
            --num;
        }
      }
      if (dayCount < 0)
        dayCount -= num;
      else if (dayCount > 0)
        dayCount += num;
      for (int i = index + 1; i < logList.GetNumberOfMilestones(); ++i)
      {
        MilestoneLog milestoneAt = logList.GetMilestoneAt(i);
        if (milestoneAt.Done)
          break;
        DateTime dateTime = milestoneAt.Date;
        DateTime date4 = dateTime.Date;
        dateTime = DateTime.MaxValue;
        DateTime date5 = dateTime.Date;
        if (date4 != date5)
        {
          DateTime date6 = currentMilestoneLog.Date;
          dateTime = DateTime.MaxValue;
          DateTime date7 = dateTime.Date;
          if (date6 == date7)
          {
            milestoneAt.AdjustAllDates(DateTime.MaxValue);
          }
          else
          {
            try
            {
              DateTime newDate = this.AddDays(milestoneAt.Date, dayCount);
              DateTime date8 = newDate.Date;
              dateTime = logList.GetMilestoneAt(i - 1).Date;
              DateTime date9 = dateTime.Date;
              if (date8 == date9 && i > 0)
                newDate = logList.GetMilestoneAt(i - 1).Date;
              milestoneAt.AdjustAllDates(newDate);
            }
            catch
            {
              milestoneAt.AdjustAllDates(DateTime.MaxValue);
            }
          }
        }
      }
    }

    private DateTime AddDays(DateTime date, int dayCount)
    {
      DateTime date1 = date;
      switch (this.calendarType)
      {
        case AutoDayCountSetting.CalendarDays:
          date1 = dayCount == 0 ? date1.AddMinutes(1.0) : date1.AddDays((double) dayCount);
          break;
        case AutoDayCountSetting.CompanyDays:
          try
          {
            date1 = this.bizCalendar.AddBusinessDays(date1, dayCount, false);
            break;
          }
          catch (ArgumentOutOfRangeException ex)
          {
            Tracing.Log(MilestoneDateCalculator.sw, this.className, TraceLevel.Error, ex.ToString());
            break;
          }
        default:
          int num = dayCount;
          if (num == 0)
            date1 = date1.AddMinutes(1.0);
          while (num != 0)
          {
            date1 = date1.AddDays(1.0);
            if (date1.DayOfWeek < DayOfWeek.Saturday && date1.DayOfWeek > DayOfWeek.Sunday)
              --num;
          }
          break;
      }
      return date1;
    }
  }
}
