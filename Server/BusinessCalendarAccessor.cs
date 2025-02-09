// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.BusinessCalendarAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class BusinessCalendarAccessor
  {
    private const string className = "BusinessCalendarAccessor�";

    public static int CreateBusinessCalendar(
      BusinessCalendar calendar,
      HolidaySchedule defaultSchedule)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("BusinessCalendar");
      DbValueList dbValueList = BusinessCalendarAccessor.createDbValueList(calendar);
      dbQueryBuilder.Declare("@calendarId", "int");
      dbQueryBuilder.InsertInto(table1, dbValueList, true, false);
      dbQueryBuilder.SelectIdentity("@calendarId");
      dbQueryBuilder.AppendLine("exec PopulateDefaultBusinessDays @calendarId, " + SQL.Encode((object) BusinessCalendar.MinimumDate) + ", " + SQL.Encode((object) BusinessCalendar.MaximumDate) + ", " + (object) (int) defaultSchedule);
      int businessCalendar = SQL.DecodeInt(dbQueryBuilder.ExecuteScalar());
      DbTableInfo table2 = DbAccessManager.GetTable("BusinessDays");
      dbQueryBuilder.Reset();
      foreach (DateTime recurringNonWorkDay in calendar.GetNonRecurringNonWorkDays())
        dbQueryBuilder.Update(table2, new DbValueList()
        {
          {
            "DayType",
            (object) (int) calendar.GetDayType(recurringNonWorkDay)
          },
          {
            "DayIndex",
            (object) -1
          }
        }, new DbValueList()
        {
          {
            "CalendarID",
            (object) calendar.CalendarID
          },
          {
            "Date",
            (object) recurringNonWorkDay.Date
          }
        });
      dbQueryBuilder.AppendLine("exec ReindexBusinessDays " + (object) businessCalendar);
      dbQueryBuilder.ExecuteNonQuery(TimeSpan.FromMinutes(2.0), DbTransactionType.Default);
      return businessCalendar;
    }

    public static BusinessCalendar GetBusinessCalendar(CalendarType calendarType)
    {
      return BusinessCalendarAccessor.GetBusinessCalendar((int) calendarType);
    }

    public static BusinessCalendar GetBusinessCalendar(
      CalendarType calendarType,
      DateTime startDate)
    {
      return BusinessCalendarAccessor.GetBusinessCalendar((int) calendarType, startDate);
    }

    public static BusinessCalendar GetBusinessCalendar(
      CalendarType calendarType,
      DateTime startDate,
      DateTime endDate)
    {
      return BusinessCalendarAccessor.GetBusinessCalendar((int) calendarType, startDate, endDate);
    }

    private static BusinessCalendar GetBusinessCalendar(int calendarId)
    {
      return ClientContext.GetCurrent().Cache.Get<BusinessCalendar>(BusinessCalendarAccessor.getUniqueObjectID(calendarId), (Func<BusinessCalendar>) (() => BusinessCalendarAccessor.GetBusinessCalendar(calendarId, BusinessCalendar.MinimumDate)), CacheSetting.Low);
    }

    private static BusinessCalendar GetBusinessCalendar(int calendarId, DateTime startDate)
    {
      return BusinessCalendarAccessor.GetBusinessCalendar(calendarId, startDate, BusinessCalendar.MaximumDate);
    }

    private static BusinessCalendar GetBusinessCalendar(
      int calendarId,
      DateTime startDate,
      DateTime endDate)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        DbTableInfo table = DbAccessManager.GetTable("BusinessCalendar");
        DbValue key = new DbValue("CalendarID", (object) calendarId);
        pgDbQueryBuilder.SelectFrom(table, key);
        pgDbQueryBuilder.AppendLine("select [Date], [DayType] from BusinessDays where CalendarID = " + (object) calendarId + " and [Date] >= " + SQL.EncodeDateTime(startDate.Date) + " and [Date] <= " + SQL.EncodeDateTime(endDate.Date) + " and DayType > " + (object) 2 + ";");
        return BusinessCalendarAccessor.dataSetToCalendar(startDate, endDate, pgDbQueryBuilder.ExecuteSetQuery());
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder(DBReadReplicaFeature.Login);
      DbTableInfo table1 = DbAccessManager.GetTable("BusinessCalendar");
      DbValue key1 = new DbValue("CalendarID", (object) calendarId);
      dbQueryBuilder.SelectFrom(table1, key1);
      dbQueryBuilder.AppendLine("select [Date], [DayType] from BusinessDays where CalendarID = " + (object) calendarId + " and [Date] >= " + SQL.EncodeDateTime(startDate.Date) + " and [Date] <= " + SQL.EncodeDateTime(endDate.Date) + " and DayType > " + (object) 2);
      return BusinessCalendarAccessor.dataSetToCalendar(startDate, endDate, dbQueryBuilder.ExecuteSetQuery());
    }

    public static BusinessCalendar GetFullCalendar(
      CalendarType calendarId,
      DateTime startDate,
      DateTime endDate)
    {
      return new BusinessCalendar((int) calendarId, SQL.DecodeEnum<DaysOfWeek>((object) DaysOfWeek.All), startDate, endDate);
    }

    public static void SaveBusinessCalendar(BusinessCalendar calendar)
    {
      if (calendar == null)
        throw new ArgumentNullException(nameof (calendar));
      if (calendar.CalendarID == -1)
        throw new ArgumentException("CalendarSetup must have a valid calendar ID. Call CreateCalendar to create a new calendar.");
      ClientContext.GetCurrent().Cache.Put<BusinessCalendar>(BusinessCalendarAccessor.getUniqueObjectID(calendar.CalendarID), (Action) (() =>
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        DbTableInfo table1 = DbAccessManager.GetTable("BusinessCalendar");
        DbValueList dbValueList = BusinessCalendarAccessor.createDbValueList(calendar);
        DbValue key = new DbValue("CalendarId", (object) calendar.CalendarID);
        dbQueryBuilder.Update(table1, dbValueList, key);
        dbQueryBuilder.AppendLine("update BusinessDays set DayIndex = -1, DayType = " + (object) 1);
        dbQueryBuilder.AppendLine("where CalendarID = " + (object) calendar.CalendarID);
        dbQueryBuilder.AppendLine("  and DayType > " + (object) 2);
        dbQueryBuilder.AppendLine("  and [Date] >= " + SQL.EncodeDateTime(calendar.StartDate));
        dbQueryBuilder.AppendLine("  and [Date] <= " + SQL.EncodeDateTime(calendar.EndDate));
        DbTableInfo table2 = DbAccessManager.GetTable("BusinessDays");
        foreach (DateTime recurringNonWorkDay in calendar.GetNonRecurringNonWorkDays())
          dbQueryBuilder.Update(table2, new DbValueList()
          {
            {
              "DayType",
              (object) (int) calendar.GetDayType(recurringNonWorkDay)
            },
            {
              "DayIndex",
              (object) -1
            }
          }, new DbValueList()
          {
            {
              "CalendarID",
              (object) calendar.CalendarID
            },
            {
              "Date",
              (object) recurringNonWorkDay.Date
            }
          });
        dbQueryBuilder.AppendLine("exec ReindexBusinessDays " + (object) calendar.CalendarID);
        dbQueryBuilder.ExecuteNonQuery(TimeSpan.FromMinutes(2.0), DbTransactionType.Serialized);
      }), (Func<BusinessCalendar>) (() => BusinessCalendarAccessor.GetBusinessCalendar(calendar.CalendarID, BusinessCalendar.MinimumDate)), CacheSetting.Low);
    }

    public static DateTime AddBusinessDays(
      CalendarType calendarType,
      DateTime date,
      int daysToAdd,
      bool startFromClosestBusinessDay)
    {
      return BusinessCalendarAccessor.AddBusinessDays((int) calendarType, date, daysToAdd, startFromClosestBusinessDay);
    }

    [PgReady]
    private static DateTime AddBusinessDays(
      int calendarId,
      DateTime date,
      int daysToAdd,
      bool startFromClosestBusinessDay)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select * from FN_AddBusinessDays(" + (object) calendarId + ", " + SQL.EncodeDateTime(date.Date) + ", " + (object) daysToAdd + ", " + SQL.EncodeFlag(startFromClosestBusinessDay) + ")");
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
        if (dataRowCollection.Count == 0)
          throw new ComplianceCalendarException(date, daysToAdd);
        return SQL.DecodeDateTime(dataRowCollection[0]["Date"]).Date.Add(date.TimeOfDay);
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from FN_AddBusinessDays(" + (object) calendarId + ", " + SQL.EncodeDateTime(date.Date) + ", " + (object) daysToAdd + ", " + SQL.EncodeFlag(startFromClosestBusinessDay) + ")");
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      if (dataRowCollection1.Count == 0)
        throw new ComplianceCalendarException(date, daysToAdd);
      return SQL.DecodeDateTime(dataRowCollection1[0]["Date"]).Date.Add(date.TimeOfDay);
    }

    public static DateTime GetClosestBusinessDay(CalendarType calendarType, DateTime date)
    {
      return BusinessCalendarAccessor.GetClosestBusinessDay((int) calendarType, date);
    }

    public static DateTime GetClosestBusinessDay(int calendarId, DateTime date)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from FN_GetNextClosestBusinessDay(" + (object) calendarId + ", " + SQL.EncodeDateTime(date.Date) + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection.Count == 0)
        throw new ComplianceCalendarException(date, 0);
      return SQL.DecodeDateTime(dataRowCollection[0]["Date"]).Date.Add(date.TimeOfDay);
    }

    [PgReady]
    private static BusinessCalendar dataSetToCalendar(
      DateTime startDate,
      DateTime endDate,
      DataSet ds)
    {
      DataTable table1 = ds.Tables[0];
      DataTable table2 = ds.Tables[1];
      if (table1.Rows.Count == 0)
        return (BusinessCalendar) null;
      DataRow row1 = table1.Rows[0];
      BusinessCalendar calendar = new BusinessCalendar(SQL.DecodeInt(row1["CalendarID"]), SQL.DecodeEnum<DaysOfWeek>(row1["WorkDays"]), startDate, endDate);
      foreach (DataRow row2 in (InternalDataCollectionBase) table2.Rows)
        calendar.SetDayType(SQL.DecodeDateTime(row2["Date"]), SQL.DecodeEnum<CalendarDayType>(row2["DayType"]));
      return calendar;
    }

    public static List<BusinessCalendar> GetAllBusinessCalendars()
    {
      if (BusinessCalendarAccessor.CacheEnabled((IClientContext) ClientContext.GetCurrent()))
      {
        List<BusinessCalendar> businessCalendars = new List<BusinessCalendar>();
        foreach (CalendarType calendarType in Enum.GetValues(typeof (CalendarType)))
        {
          BusinessCalendar businessCalendar = BusinessCalendarAccessor.GetBusinessCalendar(calendarType);
          if (businessCalendar != null)
            businessCalendars.Add(businessCalendar);
        }
        return businessCalendars;
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder(DBReadReplicaFeature.Login);
      dbQueryBuilder.AppendLine(" select bc.calendarID, bc.WorkDays, bd.[date], bd.[daytype]");
      dbQueryBuilder.AppendLine(" from BusinessCalendar bc inner join BusinessDays bd");
      dbQueryBuilder.AppendLine(" on bc.CalendarID = bd.CalendarID");
      dbQueryBuilder.AppendLine(" where DayType > " + (object) 2);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      List<BusinessCalendar> businessCalendars1 = new List<BusinessCalendar>();
      foreach (CalendarType calendarType in Enum.GetValues(typeof (CalendarType)))
      {
        DataRow[] source = dataTable.Select("calendarID =" + (object) (int) calendarType, "[date]");
        if (source != null && ((IEnumerable<DataRow>) source).Count<DataRow>() > 0)
        {
          DataRow dataRow1 = ((IEnumerable<DataRow>) source).AsEnumerable<DataRow>().Take<DataRow>(1).ElementAt<DataRow>(0);
          BusinessCalendar bc = new BusinessCalendar(SQL.DecodeInt(dataRow1["calendarID"]), SQL.DecodeEnum<DaysOfWeek>(dataRow1["WorkDays"]), BusinessCalendar.MinimumDate, BusinessCalendar.MaximumDate);
          foreach (DataRow dataRow2 in source)
            bc.SetDayType(SQL.DecodeDateTime(dataRow2["date"]), SQL.DecodeEnum<CalendarDayType>(dataRow2["daytype"]));
          BusinessCalendarAccessor.CacheBusinessCalendars(bc);
          businessCalendars1.Add(bc);
        }
      }
      return businessCalendars1;
    }

    public static void CacheBusinessCalendars(BusinessCalendar bc)
    {
      ClientContext.GetCurrent().Cache.Put(BusinessCalendarAccessor.getUniqueObjectID(bc.CalendarID), (object) bc, CacheSetting.Low);
    }

    private static DbValueList createDbValueList(BusinessCalendar calendar)
    {
      return new DbValueList()
      {
        {
          "WorkDays",
          (object) calendar.WorkDays
        }
      };
    }

    private static string getUniqueObjectID(int calendarId)
    {
      return "BusinessCalendarAccessor/" + calendarId.ToString();
    }

    private static bool CacheEnabled(IClientContext context) => context.Cache.CacheSetting != 0;
  }
}
