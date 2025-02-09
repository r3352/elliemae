// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.LockExtensionSettings
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
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

    public bool AllowExtensions => this.allowExtensions;

    public LockExtensionPricingControlOption PricingControlOption
    {
      get
      {
        return !this.AllowExtensions ? LockExtensionPricingControlOption.None : this.pricingControlOption;
      }
    }

    public bool AreFixedPriceAdjustmentsSupported
    {
      get
      {
        return this.PricingControlOption == LockExtensionPricingControlOption.Company && this.areFixedPriceAdjustmentsSupported;
      }
    }

    public bool AllowDailyAdjustment
    {
      get
      {
        return this.PricingControlOption == LockExtensionPricingControlOption.Company && this.allowDailyAdj;
      }
    }

    public Decimal DailyAdjustment => this.AllowDailyAdjustment ? this.dailyAdj : 0M;

    public bool ApplyAdjustmentOnOffDays
    {
      get => this.AllowDailyAdjustment && this.applyPriceAdjOnOffDays;
    }

    public bool ApplyLockExpirationCalendarToExtensions
    {
      get => this.applyLockExpirationCalendarToExtensions;
    }

    public LockExpirationCalendarOption LockExpirationCalendar => this.calendarOption;

    public LockExtensionPriceAdjustments PriceAdjustments => this.adjustments;

    public bool IsExtensionPeriodAllowed(int daysToExtend)
    {
      return this.PricingControlOption != LockExtensionPricingControlOption.Company || this.pricingUtils.HasPriceAdjustment(daysToExtend);
    }

    public Decimal GetPriceAdjustment(int daysToExtend)
    {
      return this.PricingControlOption == LockExtensionPricingControlOption.User ? 0M : this.pricingUtils.GetPriceAdjustment(daysToExtend);
    }

    public Decimal GetPriceAdjustment(DateTime priorExpirationDate, int daysToExtend)
    {
      return this.PricingControlOption == LockExtensionPricingControlOption.User ? 0M : this.pricingUtils.GetPriceAdjustment(priorExpirationDate, daysToExtend);
    }

    public DateTime GetExtensionExpirationDate(DateTime priorExpirationDate, int daysToExtend)
    {
      return this.pricingUtils.GetExtensionExpirationDate(priorExpirationDate, daysToExtend);
    }

    public void Refresh()
    {
      IDictionary serverSettings = this.Session.SessionObjects.ServerManager.GetServerSettings("Policies");
      this.allowExtensions = (bool) serverSettings[(object) "Policies.EnableLockExtension"];
      this.areFixedPriceAdjustmentsSupported = (bool) serverSettings[(object) "Policies.LockExtensionAllowFixedExt"];
      this.allowDailyAdj = (bool) serverSettings[(object) "Policies.LockExtensionAllowDailyAdj"];
      this.dailyAdj = (Decimal) serverSettings[(object) "Policies.LockExtensionDailyPriceAdj"];
      this.applyPriceAdjOnOffDays = (bool) serverSettings[(object) "Policies.LockExtCalOpt_ApplyPriceAdj"];
      this.pricingControlOption = (int) serverSettings[(object) "Policies.LockExtensionCompanyControlled"] != 1 ? ((int) serverSettings[(object) "Policies.LockExtensionCompanyControlled"] != 2 ? LockExtensionPricingControlOption.User : LockExtensionPricingControlOption.CompanyByExtOccurrence) : LockExtensionPricingControlOption.Company;
      this.calendarOption = (LockExpirationCalendarOption) (LockExpCalendarSetting) serverSettings[(object) "Policies.LockExpCalendar"];
      this.applyLockExpirationCalendarToExtensions = (bool) serverSettings[(object) "Policies.LockExtensionCalendarOpt"];
      LockExtensionPriceAdjustment[] adjustmentObjects = this.pricingControlOption != LockExtensionPricingControlOption.Company ? (this.pricingControlOption != LockExtensionPricingControlOption.CompanyByExtOccurrence ? new LockExtensionPriceAdjustment[0] : this.Session.SessionObjects.ConfigurationManager.GetLockExtPriceAdjustPerOccurrence()) : this.Session.SessionObjects.ConfigurationManager.GetLockExtensionPriceAdjustments();
      this.adjustments = new LockExtensionPriceAdjustments(adjustmentObjects);
      this.pricingUtils = new LockExtensionUtils(this.Session.SessionObjects, serverSettings, adjustmentObjects);
    }
  }
}
