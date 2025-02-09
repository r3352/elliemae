// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.LockExtensionSettings
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
  /// <summary>
  /// Provides access to lock extension-related system settings
  /// </summary>
  public class LockExtensionSettings : SessionBoundObject
  {
    private bool allowExtensions;
    private LockExtensionPricingControlOption pricingControlOption;
    private bool areFixedPriceAdjustmentsSupported;
    private bool allowDailyAdj;
    private Decimal dailyAdj;
    private bool applyPriceAdjOnOffDays;
    private bool applyLockExpirationCalendarToExtensions;
    private LockExpirationCalendarOption calendarOption;
    private LockExtensionPriceAdjustments adjustments;
    private LockExtensionUtils pricingUtils;

    internal LockExtensionSettings(Session session)
      : base(session)
    {
      this.Refresh();
    }

    /// <summary>
    /// Indicates if lock extensions are permitted by the Encompass system
    /// </summary>
    public bool AllowExtensions => this.allowExtensions;

    /// <summary>Gets the pricing control option for lock extensions</summary>
    public LockExtensionPricingControlOption PricingControlOption
    {
      get
      {
        return !this.AllowExtensions ? LockExtensionPricingControlOption.None : this.pricingControlOption;
      }
    }

    /// <summary>
    /// Gets a flag indicating if pricing for pre-defined extension period lengths is supported.
    /// </summary>
    public bool AreFixedPriceAdjustmentsSupported
    {
      get
      {
        return this.PricingControlOption == LockExtensionPricingControlOption.Company && this.areFixedPriceAdjustmentsSupported;
      }
    }

    /// <summary>
    /// Indicates if extra days for an extension use a daily adjustment rate
    /// </summary>
    public bool AllowDailyAdjustment
    {
      get
      {
        return this.PricingControlOption == LockExtensionPricingControlOption.Company && this.allowDailyAdj;
      }
    }

    /// <summary>
    /// Gets the daily price adjustment amount for an extension
    /// </summary>
    public Decimal DailyAdjustment => this.AllowDailyAdjustment ? this.dailyAdj : 0M;

    /// <summary>
    /// Indicates if the adjustment applies to weekends and holidays as well as work days
    /// </summary>
    public bool ApplyAdjustmentOnOffDays
    {
      get => this.AllowDailyAdjustment && this.applyPriceAdjOnOffDays;
    }

    /// <summary>
    /// Indicates if the lock expiration calendar setting is applied to lock extensions
    /// </summary>
    public bool ApplyLockExpirationCalendarToExtensions
    {
      get => this.applyLockExpirationCalendarToExtensions;
    }

    /// <summary>
    /// Indicates if the lock expiration calendar setting is applied to lock extensions
    /// </summary>
    public LockExpirationCalendarOption LockExpirationCalendar => this.calendarOption;

    /// <summary>
    /// Gets the collection of price adjustments as determined by the settings
    /// </summary>
    public LockExtensionPriceAdjustments PriceAdjustments => this.adjustments;

    /// <summary>
    /// Indicates if the specified extension is permitted based on the settings
    /// </summary>
    /// <param name="daysToExtend">The number of days the extension is requested for.</param>
    /// <returns>Returns <c>true</c> if the price adjustment is permitted by the company policy,
    /// <c>false</c> otherwise.</returns>
    public bool IsExtensionPeriodAllowed(int daysToExtend)
    {
      return this.PricingControlOption != LockExtensionPricingControlOption.Company || this.pricingUtils.HasPriceAdjustment(daysToExtend);
    }

    /// <summary>
    /// Returns the price adjustment based on the specified extension period length.
    /// </summary>
    /// <param name="daysToExtend">The number of days to extend the lock request.</param>
    /// <returns>Returns the price adjustment for the specified number of days. If the
    /// pricing is user-controlled, this method will return 0. If the number of days specified
    /// is not permitted by company policy, an exception will occur.</returns>
    public Decimal GetPriceAdjustment(int daysToExtend)
    {
      return this.PricingControlOption == LockExtensionPricingControlOption.User ? 0M : this.pricingUtils.GetPriceAdjustment(daysToExtend);
    }

    /// <summary>
    /// Returns the price adjustment based on the specified extension period length and starting
    /// expiration date.
    /// </summary>
    /// <param name="priorExpirationDate">The prior expiration date of the rate lock.</param>
    /// <param name="daysToExtend">The number of days to extend the lock request.</param>
    /// <returns>Returns the price adjustment for a lock extension of the specified number of days
    /// starting on the date specified as the prior expiration date. If the
    /// pricing is user-controlled, this method will return 0. If the number of days specified
    /// is not permitted by company policy, an exception will occur.</returns>
    public Decimal GetPriceAdjustment(DateTime priorExpirationDate, int daysToExtend)
    {
      return this.PricingControlOption == LockExtensionPricingControlOption.User ? 0M : this.pricingUtils.GetPriceAdjustment(priorExpirationDate, daysToExtend);
    }

    /// <summary>
    /// Returns the adjusted expiration date of a lock request when an extension is requested
    /// for a specified number of days.
    /// </summary>
    /// <param name="priorExpirationDate">The prior expiration date of the lock request.</param>
    /// <param name="daysToExtend">The number of days to extend the lock request.</param>
    /// <returns>Returns the new expiration date for the lock extension, based on the
    /// calendar rules specified in the Encompass Settings.</returns>
    public DateTime GetExtensionExpirationDate(DateTime priorExpirationDate, int daysToExtend)
    {
      return this.pricingUtils.GetExtensionExpirationDate(priorExpirationDate, daysToExtend);
    }

    /// <summary>
    /// Refreshes the object with the latest data from the Encompass Server.
    /// </summary>
    public void Refresh()
    {
      IDictionary serverSettings = this.Session.SessionObjects.ServerManager.GetServerSettings("Policies");
      this.allowExtensions = (bool) serverSettings[(object) "Policies.EnableLockExtension"];
      this.areFixedPriceAdjustmentsSupported = (bool) serverSettings[(object) "Policies.LockExtensionAllowFixedExt"];
      this.allowDailyAdj = (bool) serverSettings[(object) "Policies.LockExtensionAllowDailyAdj"];
      this.dailyAdj = (Decimal) serverSettings[(object) "Policies.LockExtensionDailyPriceAdj"];
      this.applyPriceAdjOnOffDays = (bool) serverSettings[(object) "Policies.LockExtCalOpt_ApplyPriceAdj"];
      this.pricingControlOption = (int) serverSettings[(object) "Policies.LockExtensionCompanyControlled"] != 1 ? ((int) serverSettings[(object) "Policies.LockExtensionCompanyControlled"] != 2 ? LockExtensionPricingControlOption.User : LockExtensionPricingControlOption.CompanyByExtOccurrence) : LockExtensionPricingControlOption.Company;
      this.calendarOption = (LockExpirationCalendarOption) serverSettings[(object) "Policies.LockExpCalendar"];
      this.applyLockExpirationCalendarToExtensions = (bool) serverSettings[(object) "Policies.LockExtensionCalendarOpt"];
      EllieMae.EMLite.ClientServer.LockExtensionPriceAdjustment[] extensionPriceAdjustmentArray = this.pricingControlOption != LockExtensionPricingControlOption.Company ? (this.pricingControlOption != LockExtensionPricingControlOption.CompanyByExtOccurrence ? new EllieMae.EMLite.ClientServer.LockExtensionPriceAdjustment[0] : this.Session.SessionObjects.ConfigurationManager.GetLockExtPriceAdjustPerOccurrence()) : this.Session.SessionObjects.ConfigurationManager.GetLockExtensionPriceAdjustments();
      this.adjustments = new LockExtensionPriceAdjustments(extensionPriceAdjustmentArray);
      this.pricingUtils = new LockExtensionUtils(this.Session.SessionObjects, serverSettings, extensionPriceAdjustmentArray);
    }
  }
}
