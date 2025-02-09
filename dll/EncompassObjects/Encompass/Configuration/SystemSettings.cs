// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.SystemSettings
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Configuration.Packages;
using EllieMae.Encompass.Configuration.Schema;
using EllieMae.Encompass.Configuration.TablesAndFees;
using System;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
  public class SystemSettings : SessionBoundObject, ISystemSettings
  {
    private SecondarySettings secondary;
    private CompensationSettings compSettings;
    private DatabaseSettings dbSettings;
    private TablesFeesSettings tablesFeesSettings;

    internal SystemSettings(Session session)
      : base(session)
    {
      this.compSettings = new CompensationSettings(session);
      this.dbSettings = new DatabaseSettings(session);
    }

    public SecondarySettings Secondary
    {
      get
      {
        if (this.secondary == null)
          this.secondary = new SecondarySettings(this.Session);
        return this.secondary;
      }
    }

    public CompensationSettings CompensationSettings => this.compSettings;

    public DatabaseSettings Database => this.dbSettings;

    public TablesFeesSettings TablesFeesSettings
    {
      get
      {
        if (this.tablesFeesSettings == null)
          this.tablesFeesSettings = new TablesFeesSettings(this.Session);
        return this.tablesFeesSettings;
      }
    }

    public BusinessCalendar GetBusinessCalendar(BusinessCalendarType calendarType)
    {
      return new BusinessCalendar(this.Session, this.Session.SessionObjects.GetBusinessCalendar((CalendarType) calendarType) ?? throw new ArgumentException("Invalid calendar type specified"));
    }

    public object GetCompanySetting(string settingName)
    {
      object serverSetting = this.Session.SessionObjects.ServerManager.GetServerSetting(settingName);
      return serverSetting.GetType().IsEnum ? (object) serverSetting.ToString() : serverSetting;
    }

    public XferPackageImporter CreatePackageImporter()
    {
      return this.Session.GetUserInfo().IsSuperAdministrator() ? new XferPackageImporter(this.Session) : throw new InvalidOperationException("Access denied");
    }
  }
}
