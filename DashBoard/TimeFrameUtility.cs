// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.TimeFrameUtility
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public static class TimeFrameUtility
  {
    public static DashboardTimeUnitType GetTimeUnit(DashboardTimePeriodType timePeriod)
    {
      switch (timePeriod)
      {
        case DashboardTimePeriodType.Previous_4_Weeks:
        case DashboardTimePeriodType.Next_4_Weeks:
        case DashboardTimePeriodType.Current_Month:
        case DashboardTimePeriodType.Previous_Month:
        case DashboardTimePeriodType.Last_30_Days:
        case DashboardTimePeriodType.Last_90_Days:
        case DashboardTimePeriodType.Next_Month:
        case DashboardTimePeriodType.Next_30_Days:
        case DashboardTimePeriodType.Next_90_Days:
          return DashboardTimeUnitType.Week;
        case DashboardTimePeriodType.Previous_4_Months:
        case DashboardTimePeriodType.Previous_6_Months:
        case DashboardTimePeriodType.Previous_12_Months:
        case DashboardTimePeriodType.Next_4_Months:
        case DashboardTimePeriodType.Next_6_Months:
        case DashboardTimePeriodType.Next_12_Months:
        case DashboardTimePeriodType.Current_Year:
        case DashboardTimePeriodType.Previous_Year:
        case DashboardTimePeriodType.Last_365_Days:
        case DashboardTimePeriodType.Next_Year:
        case DashboardTimePeriodType.Next_365_Days:
          return DashboardTimeUnitType.Month;
        case DashboardTimePeriodType.Previous_24_Months:
          return DashboardTimeUnitType.Quarter;
        case DashboardTimePeriodType.Last_7_Days:
        case DashboardTimePeriodType.Next_7_Days:
        case DashboardTimePeriodType.Current_Week:
        case DashboardTimePeriodType.Previous_Week:
        case DashboardTimePeriodType.Next_Week:
          return DashboardTimeUnitType.Day;
        default:
          return DashboardTimeUnitType.None;
      }
    }

    public static TimeUnit GetReportTimeUnit(DashboardTimePeriodType timePeriod)
    {
      switch (timePeriod)
      {
        case DashboardTimePeriodType.Previous_4_Weeks:
          return TimeUnit.Week;
        case DashboardTimePeriodType.Previous_4_Months:
        case DashboardTimePeriodType.Previous_6_Months:
        case DashboardTimePeriodType.Previous_12_Months:
          return TimeUnit.Month;
        case DashboardTimePeriodType.Previous_24_Months:
          return TimeUnit.Quarter;
        case DashboardTimePeriodType.Last_7_Days:
        case DashboardTimePeriodType.Next_7_Days:
          return TimeUnit.Day;
        default:
          return TimeUnit.Week;
      }
    }

    public static QueryCriterion GetTimeFrameCriteria(
      DashboardTimeFrameType timeFrameType,
      string timeFrameField)
    {
      if (DashboardTimeFrameType.None == timeFrameType || timeFrameField == null || timeFrameField.Length == 0)
        return (QueryCriterion) null;
      DateTime startDate;
      DateTime endDate;
      TimeFrameUtility.GetStartEndDatesForTimeFrame(timeFrameType, out startDate, out endDate);
      return (QueryCriterion) new BinaryOperation(BinaryOperator.And, (QueryCriterion) new DateValueCriterion(timeFrameField, startDate, OrdinalMatchType.GreaterThanOrEquals, DateMatchPrecision.Day), (QueryCriterion) new DateValueCriterion(timeFrameField, endDate, OrdinalMatchType.LessThanOrEquals, DateMatchPrecision.Day));
    }

    public static void GetStartEndDatesForTimeFrame(
      DashboardTimeFrameType timeFrameType,
      out DateTime startDate,
      out DateTime endDate)
    {
      DateTime today = DateTime.Today;
      startDate = today;
      endDate = today;
      switch (timeFrameType)
      {
        case DashboardTimeFrameType.CurrentWeek:
          startDate = today.AddDays((double) -(int) today.DayOfWeek);
          endDate = startDate.AddDays(6.0);
          break;
        case DashboardTimeFrameType.CurrentMonth:
          startDate = today.AddDays((double) -(today.Day - 1));
          endDate = startDate.AddMonths(1).AddDays(-1.0);
          break;
        case DashboardTimeFrameType.CurrentYear:
          startDate = today.AddDays((double) -(today.DayOfYear - 1));
          endDate = startDate.AddYears(1).AddDays(-1.0);
          break;
        case DashboardTimeFrameType.PreviousWeek:
          startDate = today.AddDays((double) -(int) today.DayOfWeek).AddDays(-7.0);
          endDate = startDate.AddDays(6.0);
          break;
        case DashboardTimeFrameType.PreviousMonth:
          ref DateTime local1 = ref startDate;
          DateTime dateTime1 = today.AddDays((double) -(today.Day - 1));
          DateTime dateTime2 = dateTime1.AddMonths(-1);
          local1 = dateTime2;
          ref DateTime local2 = ref endDate;
          dateTime1 = startDate.AddMonths(1);
          DateTime dateTime3 = dateTime1.AddDays(-1.0);
          local2 = dateTime3;
          break;
        case DashboardTimeFrameType.PreviousYear:
          ref DateTime local3 = ref startDate;
          DateTime dateTime4 = today.AddDays((double) -(today.DayOfYear - 1));
          DateTime dateTime5 = dateTime4.AddYears(-1);
          local3 = dateTime5;
          ref DateTime local4 = ref endDate;
          dateTime4 = startDate.AddYears(1);
          DateTime dateTime6 = dateTime4.AddDays(-1.0);
          local4 = dateTime6;
          break;
        case DashboardTimeFrameType.Last_7_Days:
          startDate = today.AddDays(-7.0);
          endDate = startDate.AddDays(6.0);
          break;
        case DashboardTimeFrameType.Last_30_Days:
          startDate = today.AddDays(-30.0);
          endDate = startDate.AddDays(29.0);
          break;
        case DashboardTimeFrameType.Last_90_Days:
          startDate = today.AddDays(-90.0);
          endDate = startDate.AddDays(89.0);
          break;
        case DashboardTimeFrameType.Last_365_Days:
          startDate = today.AddDays(-365.0);
          endDate = startDate.AddDays(364.0);
          break;
        case DashboardTimeFrameType.Next_Week:
          startDate = today.AddDays((double) -(int) today.DayOfWeek).AddDays(7.0);
          endDate = startDate.AddDays(6.0);
          break;
        case DashboardTimeFrameType.Next_Month:
          ref DateTime local5 = ref startDate;
          DateTime dateTime7 = today.AddDays((double) -(today.Day - 1));
          DateTime dateTime8 = dateTime7.AddMonths(1);
          local5 = dateTime8;
          ref DateTime local6 = ref endDate;
          dateTime7 = startDate.AddMonths(1);
          DateTime dateTime9 = dateTime7.AddDays(-1.0);
          local6 = dateTime9;
          break;
        case DashboardTimeFrameType.Next_Year:
          ref DateTime local7 = ref startDate;
          DateTime dateTime10 = today.AddDays((double) -(today.DayOfYear - 1));
          DateTime dateTime11 = dateTime10.AddYears(1);
          local7 = dateTime11;
          ref DateTime local8 = ref endDate;
          dateTime10 = startDate.AddYears(1);
          DateTime dateTime12 = dateTime10.AddDays(-1.0);
          local8 = dateTime12;
          break;
        case DashboardTimeFrameType.Next_7_Days:
          startDate = today.AddDays(1.0);
          endDate = startDate.AddDays(6.0);
          break;
        case DashboardTimeFrameType.Next_30_Days:
          startDate = today.AddDays(1.0);
          endDate = startDate.AddDays(29.0);
          break;
        case DashboardTimeFrameType.Next_90_Days:
          startDate = today.AddDays(1.0);
          endDate = startDate.AddDays(89.0);
          break;
        case DashboardTimeFrameType.Next_365_Days:
          startDate = today.AddDays(1.0);
          endDate = startDate.AddDays(364.0);
          break;
      }
      startDate = TimeFrameUtility.adjustDateTime(startDate, true);
      endDate = TimeFrameUtility.adjustDateTime(endDate, false);
    }

    public static DashboardTimePeriodType GetTimePeriodTypeFromTimeFrameType(
      DashboardTimeFrameType timeFrameType)
    {
      switch (timeFrameType)
      {
        case DashboardTimeFrameType.CurrentWeek:
          return DashboardTimePeriodType.Current_Week;
        case DashboardTimeFrameType.CurrentMonth:
          return DashboardTimePeriodType.Current_Month;
        case DashboardTimeFrameType.CurrentYear:
          return DashboardTimePeriodType.Current_Year;
        case DashboardTimeFrameType.PreviousWeek:
          return DashboardTimePeriodType.Previous_Week;
        case DashboardTimeFrameType.PreviousMonth:
          return DashboardTimePeriodType.Previous_Month;
        case DashboardTimeFrameType.PreviousYear:
          return DashboardTimePeriodType.Previous_Year;
        case DashboardTimeFrameType.Last_7_Days:
          return DashboardTimePeriodType.Last_7_Days;
        case DashboardTimeFrameType.Last_30_Days:
          return DashboardTimePeriodType.Last_30_Days;
        case DashboardTimeFrameType.Last_90_Days:
          return DashboardTimePeriodType.Last_90_Days;
        case DashboardTimeFrameType.Last_365_Days:
          return DashboardTimePeriodType.Last_365_Days;
        case DashboardTimeFrameType.Next_Week:
          return DashboardTimePeriodType.Next_Week;
        case DashboardTimeFrameType.Next_Month:
          return DashboardTimePeriodType.Next_Month;
        case DashboardTimeFrameType.Next_Year:
          return DashboardTimePeriodType.Next_Year;
        case DashboardTimeFrameType.Next_7_Days:
          return DashboardTimePeriodType.Next_7_Days;
        case DashboardTimeFrameType.Next_30_Days:
          return DashboardTimePeriodType.Next_30_Days;
        case DashboardTimeFrameType.Next_90_Days:
          return DashboardTimePeriodType.Next_90_Days;
        case DashboardTimeFrameType.Next_365_Days:
          return DashboardTimePeriodType.Next_365_Days;
        default:
          return DashboardTimePeriodType.Current_Year;
      }
    }

    public static List<TrendChartDataCriteria.TimePeriodRange> GetTimePeriodRange(
      List<TrendChartDataCriteria.TimePeriodRange> timePeriodRange,
      DateTime startDTTM,
      DateTime endDTTM,
      TimeUnit timeUnit)
    {
      endDTTM = TimeFrameUtility.adjustDateTime(endDTTM, false);
      switch (timeUnit)
      {
        case TimeUnit.Day:
          DateTime endDate1 = TimeFrameUtility.adjustDateTime(startDTTM.AddDays(1.0).AddDays(-1.0), false);
          if (endDate1 >= endDTTM)
          {
            timePeriodRange.Add(new TrendChartDataCriteria.TimePeriodRange(startDTTM, endDTTM));
            break;
          }
          timePeriodRange.Add(new TrendChartDataCriteria.TimePeriodRange(startDTTM, endDate1));
          timePeriodRange = TimeFrameUtility.GetTimePeriodRange(timePeriodRange, TimeFrameUtility.adjustDateTime(endDate1.AddDays(1.0), true), endDTTM, timeUnit);
          break;
        case TimeUnit.Week:
          DateTime input;
          if (startDTTM.DayOfWeek != DayOfWeek.Sunday)
          {
            DateTime dateTime = startDTTM.AddDays((double) -(int) startDTTM.DayOfWeek);
            dateTime = dateTime.AddDays(7.0);
            input = dateTime.AddDays(-1.0);
          }
          else
            input = startDTTM.AddDays(7.0).AddDays(-1.0);
          DateTime endDate2 = TimeFrameUtility.adjustDateTime(input, false);
          if (endDate2 >= endDTTM)
          {
            timePeriodRange.Add(new TrendChartDataCriteria.TimePeriodRange(startDTTM, endDTTM));
            break;
          }
          timePeriodRange.Add(new TrendChartDataCriteria.TimePeriodRange(startDTTM, endDate2));
          timePeriodRange = TimeFrameUtility.GetTimePeriodRange(timePeriodRange, TimeFrameUtility.adjustDateTime(endDate2.AddDays(1.0), true), endDTTM, timeUnit);
          break;
        case TimeUnit.Month:
          DateTime endDate3 = TimeFrameUtility.adjustDateTime(startDTTM.Day == 1 ? startDTTM.AddMonths(1).AddDays(-1.0) : startDTTM.AddDays((double) -(startDTTM.Day - 1)).AddMonths(1).AddDays(-1.0), false);
          if (endDate3 >= endDTTM)
          {
            timePeriodRange.Add(new TrendChartDataCriteria.TimePeriodRange(startDTTM, endDTTM));
            break;
          }
          timePeriodRange.Add(new TrendChartDataCriteria.TimePeriodRange(startDTTM, endDate3));
          timePeriodRange = TimeFrameUtility.GetTimePeriodRange(timePeriodRange, TimeFrameUtility.adjustDateTime(endDate3.AddDays(1.0), true), endDTTM, timeUnit);
          break;
        case TimeUnit.Quarter:
          DateTime endDate4 = TimeFrameUtility.adjustDateTime(startDTTM.Day == 1 ? startDTTM.AddMonths(3).AddDays(-1.0) : startDTTM.AddDays((double) -(startDTTM.Day - 1)).AddMonths(3).AddDays(-1.0), false);
          if (endDate4 >= endDTTM)
          {
            timePeriodRange.Add(new TrendChartDataCriteria.TimePeriodRange(startDTTM, endDTTM));
            break;
          }
          timePeriodRange.Add(new TrendChartDataCriteria.TimePeriodRange(startDTTM, endDate4));
          timePeriodRange = TimeFrameUtility.GetTimePeriodRange(timePeriodRange, TimeFrameUtility.adjustDateTime(endDate4.AddDays(1.0), true), endDTTM, timeUnit);
          break;
      }
      return timePeriodRange;
    }

    public static TrendChartDataCriteria.TimePeriodRange[] GetStartEndDatesForTimePeriod(
      DashboardTimePeriodType timePeriodType,
      DashboardTimeUnitType timeUnit)
    {
      switch (timeUnit)
      {
        case DashboardTimeUnitType.Day:
          return TimeFrameUtility.GetStartEndDatesForTimePeriod(timePeriodType, TimeUnit.Day);
        case DashboardTimeUnitType.Week:
          return TimeFrameUtility.GetStartEndDatesForTimePeriod(timePeriodType, TimeUnit.Week);
        case DashboardTimeUnitType.Month:
          return TimeFrameUtility.GetStartEndDatesForTimePeriod(timePeriodType, TimeUnit.Month);
        case DashboardTimeUnitType.Quarter:
          return TimeFrameUtility.GetStartEndDatesForTimePeriod(timePeriodType, TimeUnit.Quarter);
        default:
          return TimeFrameUtility.GetStartEndDatesForTimePeriod(timePeriodType, TimeUnit.Day);
      }
    }

    public static TrendChartDataCriteria.TimePeriodRange[] GetStartEndDatesForTimePeriod(
      DashboardTimePeriodType timePeriodType,
      TimeUnit timeUnit)
    {
      DateTime today = DateTime.Today;
      List<TrendChartDataCriteria.TimePeriodRange> timePeriodRange = new List<TrendChartDataCriteria.TimePeriodRange>();
      DateTime dateTime1 = DateTime.Today;
      DateTime input = DateTime.Today;
      switch (timePeriodType)
      {
        case DashboardTimePeriodType.Previous_4_Weeks:
          DateTime dateTime2 = today.AddDays((double) -(int) today.DayOfWeek);
          dateTime1 = dateTime2.AddDays(-28.0);
          dateTime2 = dateTime1.AddDays(28.0);
          input = dateTime2.AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Previous_4_Months:
          DateTime dateTime3 = today.AddDays((double) -(today.Day - 1));
          dateTime1 = dateTime3.AddMonths(-4);
          dateTime3 = dateTime1.AddMonths(4);
          input = dateTime3.AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Previous_6_Months:
          DateTime dateTime4 = today.AddDays((double) -(today.Day - 1));
          dateTime1 = dateTime4.AddMonths(-6);
          dateTime4 = dateTime1.AddMonths(6);
          input = dateTime4.AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Previous_12_Months:
          DateTime dateTime5 = today.AddDays((double) -(today.Day - 1));
          dateTime1 = dateTime5.AddMonths(-12);
          dateTime5 = dateTime1.AddMonths(12);
          input = dateTime5.AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Previous_24_Months:
          DateTime dateTime6 = today.AddDays((double) -(today.Day - 1));
          dateTime1 = dateTime6.AddMonths(-24);
          dateTime6 = dateTime1.AddMonths(24);
          input = dateTime6.AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Last_7_Days:
          dateTime1 = today.AddDays(-7.0);
          input = dateTime1.AddDays(7.0).AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Next_7_Days:
          dateTime1 = today.AddDays(1.0);
          input = dateTime1.AddDays(7.0).AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Next_4_Weeks:
          DateTime dateTime7 = today.AddDays((double) -(int) today.DayOfWeek);
          dateTime1 = dateTime7.AddDays(7.0);
          dateTime7 = dateTime1.AddDays(28.0);
          input = dateTime7.AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Next_4_Months:
          DateTime dateTime8 = today.AddDays((double) -(today.Day - 1));
          dateTime1 = dateTime8.AddMonths(1);
          dateTime8 = dateTime1.AddMonths(4);
          input = dateTime8.AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Next_6_Months:
          DateTime dateTime9 = today.AddDays((double) -(today.Day - 1));
          dateTime1 = dateTime9.AddMonths(1);
          dateTime9 = dateTime1.AddMonths(6);
          input = dateTime9.AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Next_12_Months:
          DateTime dateTime10 = today.AddDays((double) -(today.Day - 1));
          dateTime1 = dateTime10.AddMonths(1);
          dateTime10 = dateTime1.AddMonths(12);
          input = dateTime10.AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Current_Week:
          dateTime1 = today.AddDays((double) -(int) today.DayOfWeek);
          input = dateTime1.AddDays(7.0).AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Current_Month:
          dateTime1 = today.AddDays((double) -(today.Day - 1));
          input = dateTime1.AddMonths(1).AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Current_Year:
          dateTime1 = new DateTime(today.Year, 1, 1);
          input = new DateTime(today.Year, 12, 31);
          break;
        case DashboardTimePeriodType.Previous_Week:
          DateTime dateTime11 = today.AddDays((double) -(int) today.DayOfWeek);
          dateTime1 = dateTime11.AddDays(-7.0);
          dateTime11 = dateTime1.AddDays(7.0);
          input = dateTime11.AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Previous_Month:
          DateTime dateTime12 = today.AddDays((double) -(today.Day - 1));
          dateTime1 = dateTime12.AddMonths(-1);
          dateTime12 = dateTime1.AddMonths(1);
          input = dateTime12.AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Previous_Year:
          DateTime dateTime13 = today.AddDays((double) -(today.Day - 1));
          dateTime1 = dateTime13.AddYears(-1);
          dateTime13 = dateTime1.AddYears(1);
          input = dateTime13.AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Last_30_Days:
          dateTime1 = today.AddDays(-1.0).AddDays(-30.0).AddDays(1.0);
          input = dateTime1.AddDays(30.0).AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Last_90_Days:
          dateTime1 = today.AddDays(-1.0).AddDays(-90.0).AddDays(1.0);
          input = dateTime1.AddDays(90.0).AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Last_365_Days:
          dateTime1 = today.AddDays(-1.0).AddDays(-365.0).AddDays(1.0);
          input = dateTime1.AddDays(365.0).AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Next_Week:
          DateTime dateTime14 = today.AddDays((double) -(int) today.DayOfWeek);
          dateTime1 = dateTime14.AddDays(7.0);
          dateTime14 = dateTime1.AddDays(7.0);
          input = dateTime14.AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Next_Month:
          DateTime dateTime15 = today.AddDays((double) -(today.Day - 1));
          dateTime1 = dateTime15.AddMonths(1);
          dateTime15 = dateTime1.AddMonths(1);
          input = dateTime15.AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Next_Year:
          dateTime1 = new DateTime(today.Year + 1, 1, 1);
          input = new DateTime(today.Year + 1, 12, 31);
          break;
        case DashboardTimePeriodType.Next_30_Days:
          dateTime1 = today.AddDays(1.0);
          input = dateTime1.AddDays(30.0).AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Next_90_Days:
          dateTime1 = today.AddDays(1.0);
          input = dateTime1.AddDays(90.0).AddDays(-1.0);
          break;
        case DashboardTimePeriodType.Next_365_Days:
          dateTime1 = today.AddDays(1.0);
          input = dateTime1.AddDays(365.0).AddDays(-1.0);
          break;
      }
      dateTime1 = TimeFrameUtility.adjustDateTime(dateTime1, true);
      DateTime dateTime16 = TimeFrameUtility.adjustDateTime(input, false);
      timePeriodRange.Add(new TrendChartDataCriteria.TimePeriodRange(dateTime1, dateTime16));
      return TimeFrameUtility.GetTimePeriodRange(timePeriodRange, dateTime1, dateTime16, timeUnit).ToArray();
    }

    public static TrendChartDataCriteria.TimePeriodRange[] GetStartEndDatesForTimePeriod(
      TimeUnit timeUnit,
      DateTime startDate,
      DateTime endDate)
    {
      List<TrendChartDataCriteria.TimePeriodRange> timePeriodRange = new List<TrendChartDataCriteria.TimePeriodRange>();
      startDate = TimeFrameUtility.adjustDateTime(startDate, true);
      endDate = TimeFrameUtility.adjustDateTime(endDate, false);
      timePeriodRange.Add(new TrendChartDataCriteria.TimePeriodRange(startDate, endDate));
      return TimeFrameUtility.GetTimePeriodRange(timePeriodRange, startDate, endDate, timeUnit).ToArray();
    }

    private static DateTime adjustDateTime(DateTime input, bool startOftheDate)
    {
      DateTime dateTime = DateTime.Parse(input.ToString("MM/dd/yyyy"));
      return startOftheDate ? dateTime : dateTime.AddDays(1.0).AddSeconds(-1.0);
    }

    public static TimeUnit ToTimeUnit(DashboardTimeUnitType dashboardType)
    {
      switch (dashboardType)
      {
        case DashboardTimeUnitType.None:
          return TimeUnit.Day;
        case DashboardTimeUnitType.Day:
          return TimeUnit.Day;
        case DashboardTimeUnitType.Week:
          return TimeUnit.Week;
        case DashboardTimeUnitType.Month:
          return TimeUnit.Month;
        case DashboardTimeUnitType.Quarter:
          return TimeUnit.Quarter;
        default:
          return TimeUnit.Day;
      }
    }

    public static DashboardTimeUnitType ToDashboardTimeUnitType(TimeUnit timeUnitType)
    {
      switch (timeUnitType)
      {
        case TimeUnit.Day:
          return DashboardTimeUnitType.Day;
        case TimeUnit.Week:
          return DashboardTimeUnitType.Week;
        case TimeUnit.Month:
          return DashboardTimeUnitType.Month;
        case TimeUnit.Quarter:
          return DashboardTimeUnitType.Quarter;
        default:
          return DashboardTimeUnitType.Day;
      }
    }

    public static TrendChartDataCriteria.TimePeriodRange[] GetStartEndDatesForTimePeriod(
      DashboardTimeUnitType timeUnit,
      DateTime startDate,
      DateTime endDate)
    {
      List<TrendChartDataCriteria.TimePeriodRange> timePeriodRange = new List<TrendChartDataCriteria.TimePeriodRange>();
      startDate = TimeFrameUtility.adjustDateTime(startDate, true);
      endDate = TimeFrameUtility.adjustDateTime(endDate, false);
      timePeriodRange.Add(new TrendChartDataCriteria.TimePeriodRange(startDate, endDate));
      return TimeFrameUtility.GetTimePeriodRange(timePeriodRange, startDate, endDate, TimeFrameUtility.ToTimeUnit(timeUnit)).ToArray();
    }

    public static void GetStartEndDatesForTimePeriod(
      DashboardTimePeriodType timePeriodType,
      int dataPoint,
      out DateTime startDate,
      out DateTime endDate)
    {
      DateTime dateTime = DateTime.Parse(DateTime.Today.ToString("MM/dd/yyyy"));
      startDate = dateTime;
      endDate = dateTime;
      TrendChartDataCriteria.TimePeriodRange[] datesForTimePeriod = TimeFrameUtility.GetStartEndDatesForTimePeriod(timePeriodType, TimeFrameUtility.GetTimeUnit(timePeriodType));
      if (dataPoint >= datesForTimePeriod.Length)
        return;
      startDate = datesForTimePeriod[dataPoint].StartDate;
      endDate = datesForTimePeriod[dataPoint].EndDate;
    }
  }
}
