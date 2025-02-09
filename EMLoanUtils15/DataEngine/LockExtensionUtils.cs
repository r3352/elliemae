// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LockExtensionUtils
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LockExtensionUtils
  {
    private SessionObjects sessionObjects;
    private IDictionary policySettings;
    private Decimal dailyPriceAdjustment;
    private Dictionary<int, Decimal> priceAdjustments = new Dictionary<int, Decimal>();
    private bool isCompanyControlled;
    private LoanData loan;

    public bool IsCompanyControlled => this.isCompanyControlled;

    public bool IsCompanyControlledOccur { get; set; }

    public LockExtensionPriceAdjustment[] AdjOccurrence { get; set; }

    public LockExtensionUtils(SessionObjects sessionObjects, LoanData loanData)
    {
      this.sessionObjects = sessionObjects;
      this.policySettings = sessionObjects.StartupInfo.PolicySettings;
      this.loan = loanData;
      this.IsCompanyControlledOccur = false;
      switch ((int) this.policySettings[(object) "Policies.LockExtensionCompanyControlled"])
      {
        case 1:
          this.Init(sessionObjects.ConfigurationManager.GetLockExtensionPriceAdjustments());
          break;
        case 2:
          this.Init(sessionObjects.ConfigurationManager.GetLockExtPriceAdjustPerOccurrence());
          break;
      }
    }

    public LockExtensionUtils(
      SessionObjects sessionObjects,
      IDictionary policySettings,
      LockExtensionPriceAdjustment[] priceAdjustments)
    {
      this.sessionObjects = sessionObjects;
      this.policySettings = policySettings;
      this.IsCompanyControlledOccur = false;
      this.Init(priceAdjustments);
    }

    public void Init(LockExtensionPriceAdjustment[] priceAdjustments)
    {
      switch ((int) this.policySettings[(object) "Policies.LockExtensionCompanyControlled"])
      {
        case 1:
          this.isCompanyControlled = true;
          if ((bool) this.policySettings[(object) "Policies.LockExtensionAllowDailyAdj"])
            this.dailyPriceAdjustment = (Decimal) this.policySettings[(object) "Policies.LockExtensionDailyPriceAdj"];
          if (!(bool) this.policySettings[(object) "Policies.LockExtensionAllowFixedExt"])
            break;
          foreach (LockExtensionPriceAdjustment priceAdjustment in priceAdjustments)
            this.priceAdjustments.Add(priceAdjustment.DaysToExtend, priceAdjustment.PriceAdjustment);
          break;
        case 2:
          this.IsCompanyControlledOccur = true;
          if ((int) this.policySettings[(object) "Policies.LockExtensionCompanyControlled"] != 2)
            break;
          this.AdjOccurrence = priceAdjustments;
          break;
      }
    }

    public bool HasPriceAdjustment(int daysToExtend)
    {
      return this.priceAdjustments.ContainsKey(daysToExtend) || this.dailyPriceAdjustment != 0M;
    }

    public Decimal GetPriceAdjustment(int daysToExtend)
    {
      Decimal d = 0M;
      if (this.priceAdjustments.ContainsKey(daysToExtend))
        d = this.priceAdjustments[daysToExtend];
      else if (this.dailyPriceAdjustment != 0M)
        d = this.dailyPriceAdjustment * (Decimal) daysToExtend;
      return this.loan != null && (this.loan.GetField("2626") == "Banked - Retail" && (bool) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingRetail"] || this.loan.GetField("2626") == "Banked - Wholesale" && (bool) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingWholesale"]) ? Decimal.Negate(d) : d;
    }

    public Decimal GetPriceAdjustment(DateTime priorExpDate, int daysToExtend)
    {
      Decimal priceAdjustment = 0M;
      if (this.HasPriceAdjustment(daysToExtend))
        priceAdjustment = this.GetPriceAdjustment(daysToExtend);
      if (!(bool) this.policySettings[(object) "Policies.LockExtensionCalendarOpt"] || !(bool) this.policySettings[(object) "Policies.LockExtCalOpt_ApplyPriceAdj"])
        return priceAdjustment;
      int days = (this.GetExtensionExpirationDate(priorExpDate, daysToExtend) - priorExpDate).Days;
      if (this.HasPriceAdjustment(days))
        priceAdjustment = this.GetPriceAdjustment(days);
      return priceAdjustment;
    }

    public Decimal GetPriceAdjustmentOccur(int extensionNumber)
    {
      if (this.AdjOccurrence != null)
      {
        foreach (LockExtensionPriceAdjustment extensionPriceAdjustment in this.AdjOccurrence)
        {
          if (extensionPriceAdjustment.GetExtensionNumber(extensionPriceAdjustment.ExtensionNumber) == extensionNumber)
            return this.loan != null && (this.loan.GetField("2626") == "Banked - Retail" && (bool) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingRetail"] || this.loan.GetField("2626") == "Banked - Wholesale" && (bool) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingWholesale"]) ? Decimal.Negate(extensionPriceAdjustment.PriceAdjustment) : extensionPriceAdjustment.PriceAdjustment;
        }
      }
      return 0M;
    }

    public int GetExtensionDays(int extensionNumber)
    {
      if (this.AdjOccurrence != null)
      {
        foreach (LockExtensionPriceAdjustment extensionPriceAdjustment in this.AdjOccurrence)
        {
          if (extensionPriceAdjustment.GetExtensionNumber(extensionPriceAdjustment.ExtensionNumber) == extensionNumber)
            return extensionPriceAdjustment.DaysToExtend;
        }
      }
      return 0;
    }

    public DateTime GetExtensionExpirationDate(DateTime priorExpDate, int daysToExtend)
    {
      DateTime rawExpireDate = priorExpDate.AddDays((double) daysToExtend);
      if ((bool) this.policySettings[(object) "Policies.LockExtensionCalendarOpt"])
        rawExpireDate = new LockRequestCalculator(this.sessionObjects, (LoanData) null).GetNextClosestLockExpirationDate(rawExpireDate);
      return rawExpireDate;
    }

    public bool IfExceedNumExtensionsLimit(int numLockExts)
    {
      bool flag = false;
      try
      {
        if ((int) this.policySettings[(object) "Policies.LockExtensionCompanyControlled"] != 2)
        {
          if ((bool) this.policySettings[(object) "Policies.LockExtAllowTotalTimesCapEnabled"])
          {
            if (numLockExts > (int) this.policySettings[(object) "Policies.LockExtAllowTotalTimesCap"])
              flag = true;
          }
        }
      }
      catch (Exception ex)
      {
        flag = false;
      }
      return flag;
    }

    public bool IsLockExtensionEnabled()
    {
      return Utils.ParseBoolean(this.policySettings[(object) "POLICIES.EnableLockExtension"]);
    }

    public bool IfExceedsCapDays(DateTime lockDate, DateTime newExpireDate)
    {
      if (Utils.ParseBoolean(this.policySettings[(object) "Policies.LockExtensionAllowTotalCap"]))
      {
        int num = Utils.ParseInt(this.policySettings[(object) "Policies.LockExtensionAllowTotalCapDays"], 0);
        DateTime dateTime = lockDate.AddDays((double) num);
        if (newExpireDate >= dateTime)
          return true;
      }
      return false;
    }

    public int MaxDaysToExtend(DateTime oriExpireDate, DateTime lockDate)
    {
      switch (Utils.ParseInt(this.policySettings[(object) "Policies.LOCKEXTENSION_CAP_TYPE"], 0))
      {
        case 0:
          return int.MaxValue;
        case 1:
          return (oriExpireDate - lockDate).Days;
        case 2:
          return Utils.ParseInt(this.policySettings[(object) "Policies.LOCKEXTENSION_CAP_DAYS"], 0);
        default:
          return 0;
      }
    }
  }
}
