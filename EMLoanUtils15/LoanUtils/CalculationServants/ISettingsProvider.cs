// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.CalculationServants.ISettingsProvider
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.CalculationServants
{
  public interface ISettingsProvider
  {
    object GetSystemPolicy(string path);

    object GetSystemCompliance(string path);

    IDictionary GetCategoryServerSettings(string category);

    IDictionary GetAllServerSettings();

    Hashtable GetSystemSettings(string[] names);

    BinaryObject GetSystemSettings(string name);

    BusinessCalendar GetBusinessCalendar(CalendarType type, DateTime startDate, DateTime endDate);

    BusinessCalendar GetbusinessCalendar(CalendarType type);

    DateTime AddBusinessDays(
      CalendarType calendarType,
      DateTime date,
      int daysToAdd,
      bool startFromClosestBusinessDay);

    DateTime GetNextClosestBusinessDay(BusinessCalendar calendar, DateTime date);

    DateTime GetPreviousClosestBusinessDay(BusinessCalendar calendar, DateTime date);

    List<ExternalOriginatorManagementData> GetExternalOrganizationByTpoId(string tpoId);

    List<ExternalFeeManagement> GetFeeManagement(int externalOrgId);

    Hashtable GetCachedTitleEscrowTables();

    Hashtable GetCachedCityStateUserTables();
  }
}
