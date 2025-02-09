// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.RateLocks.LockDeskHoursUtils
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.RateLocks
{
  public static class LockDeskHoursUtils
  {
    public const string ONRPLimitStandardMessage = "Overnight Rate Protection for Loan <Loan Number> exceeded Company limit by $<Dollar Amount>.�";
    private const string className = "LockDeskHoursUtils�";
    private static string sw = Tracing.SwOutsideLoan;

    public static DateTime GetServerUtcTime(IClientSession clientSession)
    {
      DateTime time = clientSession != null ? clientSession.ServerTime : throw new ArgumentException("clientSession is required");
      string serverTimeZone = clientSession.ServerTimeZone;
      try
      {
        return LockDeskHoursUtils.GetUtcForTimeZone(time, serverTimeZone);
      }
      catch (Exception ex)
      {
        Tracing.Log(LockDeskHoursUtils.sw, TraceLevel.Error, nameof (LockDeskHoursUtils), "Unable to get server UTC time. Message: " + ex.Message);
        return DateTime.UtcNow;
      }
    }

    public static DateTime GetUtcForTimeZone(DateTime time, string timeZoneId)
    {
      if (string.IsNullOrEmpty(timeZoneId))
        throw new ArgumentException("timeZoneId is required");
      return new DateTimeOffset(DateTime.SpecifyKind(time, DateTimeKind.Unspecified), System.TimeZoneInfo.FindSystemTimeZoneById(timeZoneId).GetUtcOffset(DateTime.Now)).UtcDateTime;
    }

    public static BusinessCalendar GetLockExpirationCalendar(
      ISessionStartupInfo startupInfo,
      SessionObjects sessionObjects)
    {
      LockExpCalendarSetting policySetting = (LockExpCalendarSetting) startupInfo.PolicySettings[(object) "Policies.LockExpCalendar"];
      BusinessCalendar expirationCalendar = (BusinessCalendar) null;
      switch (policySetting)
      {
        case LockExpCalendarSetting.PostalCalendar:
          expirationCalendar = sessionObjects.GetBusinessCalendar(CalendarType.Postal);
          break;
        case LockExpCalendarSetting.BusinessCalendar:
          expirationCalendar = sessionObjects.GetBusinessCalendar(CalendarType.Business);
          break;
        case LockExpCalendarSetting.CustomCalendar:
          expirationCalendar = sessionObjects.GetBusinessCalendar(CalendarType.Custom);
          break;
      }
      return expirationCalendar;
    }

    public static bool IsQAONRPSettingEnabled(string clientId)
    {
      if (clientId != "3010000024")
        return false;
      try
      {
        return File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "QA-ONRP-Enabled.txt"));
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public static LoanChannel GetEnumChannel(string channel)
    {
      if (channel.IndexOf("Wholesale") >= 0)
        return LoanChannel.BankedWholesale;
      if (channel.IndexOf("Retail") >= 0)
        return LoanChannel.BankedRetail;
      if (channel.IndexOf("Correspondent") >= 0)
        return LoanChannel.Correspondent;
      return channel.IndexOf("Brokered") >= 0 ? LoanChannel.Brokered : LoanChannel.None;
    }
  }
}
