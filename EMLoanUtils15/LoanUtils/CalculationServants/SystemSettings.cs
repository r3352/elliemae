// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.CalculationServants.SystemSettings
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
  public class SystemSettings : ISettingsProvider
  {
    private readonly SessionObjects sessionObjects;

    public SystemSettings(SessionObjects sessionObjects) => this.sessionObjects = sessionObjects;

    public Hashtable GetSystemSettings(string[] names)
    {
      return this.sessionObjects.ConfigurationManager.GetSystemSettings(names);
    }

    public BinaryObject GetSystemSettings(string name)
    {
      return this.sessionObjects.ConfigurationManager.GetSystemSettings(name);
    }

    public BusinessCalendar GetbusinessCalendar(CalendarType type)
    {
      throw new NotImplementedException();
    }

    public DateTime AddBusinessDays(
      CalendarType calendarType,
      DateTime date,
      int daysToAdd,
      bool startFromClosestBusinessDay)
    {
      return this.sessionObjects.ConfigurationManager.AddBusinessDays(CalendarType.Business, date, daysToAdd, startFromClosestBusinessDay);
    }

    public DateTime GetNextClosestBusinessDay(BusinessCalendar calendar, DateTime date)
    {
      throw new NotImplementedException();
    }

    public DateTime GetPreviousClosestBusinessDay(BusinessCalendar calendar, DateTime date)
    {
      throw new NotImplementedException();
    }

    public List<ExternalOriginatorManagementData> GetExternalOrganizationByTpoId(string tpoId)
    {
      throw new NotImplementedException();
    }

    public List<ExternalFeeManagement> GetFeeManagement(int externalOrgId)
    {
      throw new NotImplementedException();
    }

    public object GetSystemPolicy(string path)
    {
      return this.sessionObjects.StartupInfo.PolicySettings[(object) path];
    }

    public object GetSystemCompliance(string path) => throw new NotImplementedException();

    public IDictionary GetCategoryServerSettings(string category)
    {
      return this.sessionObjects.ServerManager.GetServerSettings(category);
    }

    public IDictionary GetAllServerSettings()
    {
      return this.sessionObjects.ServerManager.GetServerSettings();
    }

    public Hashtable GetCachedTitleEscrowTables()
    {
      return this.sessionObjects.GetCachedTitleEscrowTables();
    }

    public Hashtable GetCachedCityStateUserTables()
    {
      return this.sessionObjects.GetCachedCityStateUserTables();
    }

    public BusinessCalendar GetBusinessCalendar(
      CalendarType type,
      DateTime startDate,
      DateTime endDate)
    {
      throw new NotImplementedException();
    }
  }
}
