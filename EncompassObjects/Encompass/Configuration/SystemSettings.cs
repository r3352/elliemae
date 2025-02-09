// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.SystemSettings
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Configuration.Packages;
using EllieMae.Encompass.Configuration.Schema;
using EllieMae.Encompass.Configuration.TablesAndFees;
using System;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
  /// <summary>
  /// Provides access to system settings information for Encompass.
  /// </summary>
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

    /// <summary>
    /// Gets the accessor for secondary-related system settings.
    /// </summary>
    public SecondarySettings Secondary
    {
      get
      {
        if (this.secondary == null)
          this.secondary = new SecondarySettings(this.Session);
        return this.secondary;
      }
    }

    /// <summary>
    /// Gets the <see cref="P:EllieMae.Encompass.Configuration.SystemSettings.CompensationSettings" /> object, which will provide access to compensation functions.
    /// </summary>
    public CompensationSettings CompensationSettings => this.compSettings;

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.Configuration.Schema.DatabaseSettings" /> object, which will provide access to database schema information.
    /// </summary>
    public DatabaseSettings Database => this.dbSettings;

    /// <summary>
    /// Gets the <see cref="P:EllieMae.Encompass.Configuration.SystemSettings.TablesFeesSettings" /> object, which will provide access to Tables and Fees information.
    /// </summary>
    public TablesFeesSettings TablesFeesSettings
    {
      get
      {
        if (this.tablesFeesSettings == null)
          this.tablesFeesSettings = new TablesFeesSettings(this.Session);
        return this.tablesFeesSettings;
      }
    }

    /// <summary>
    /// Retrieves one of the configured <see cref="T:EllieMae.Encompass.Configuration.BusinessCalendar" /> settings for the system.
    /// </summary>
    /// <param name="calendarType">The type of calendar to be retrieved.</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.Configuration.BusinessCalendar" /> object based on the specified type.</returns>
    public BusinessCalendar GetBusinessCalendar(BusinessCalendarType calendarType)
    {
      return new BusinessCalendar(this.Session, this.Session.SessionObjects.GetBusinessCalendar((CalendarType) calendarType) ?? throw new ArgumentException("Invalid calendar type specified"));
    }

    /// <summary>
    /// Retrieves the value of one of the Encompass company settings accessible in the AdminTools
    /// Settings Manager.
    /// </summary>
    /// <param name="settingName">The name of the settings, in the form "Category.Name", e.g.
    /// "Password.MinLength"</param>
    /// <returns>Returns the value of the setting. This may be a numeric, bool, datetime or string value,
    /// depending on the setting being retrieved.</returns>
    public object GetCompanySetting(string settingName)
    {
      object serverSetting = this.Session.SessionObjects.ServerManager.GetServerSetting(settingName);
      return serverSetting.GetType().IsEnum ? (object) serverSetting.ToString() : serverSetting;
    }

    /// <summary>
    /// Creates a <see cref="T:EllieMae.Encompass.Configuration.Packages.XferPackageImporter" /> tied to the current Session.
    /// </summary>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Configuration.Packages.XferPackageImporter" /> which can be used to import Transfer
    /// Packages into the system.</returns>
    public XferPackageImporter CreatePackageImporter()
    {
      return this.Session.GetUserInfo().IsSuperAdministrator() ? new XferPackageImporter(this.Session) : throw new InvalidOperationException("Access denied");
    }
  }
}
