// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LockRequestCalculator
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.LockComparison;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.LoanUtils.RateLocks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LockRequestCalculator : CalculationBase
  {
    private SessionObjects sessionObjects;
    private Hashtable snapshot;
    private LoanData loanData;
    internal Routine CalcLockValidation;

    public LockRequestCalculator(
      SessionObjects sessionObjects,
      LoanData loanData,
      EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs = null)
      : base(loanData, calObjs)
    {
      this.sessionObjects = sessionObjects;
      this.loanData = loanData;
      this.CalcLockValidation = this.RoutineX(new Routine(this.AddLockComparisonFieldsModified));
    }

    public void PerformSnapshotCalculations(Hashtable snapshot)
    {
      this.PerformSnapshotCalculations(snapshot, false, false, false, false);
    }

    public void PerformLockRequestCalculations(Hashtable snapshot)
    {
      this.snapshot = snapshot;
      this.CalcTotalBaseRateAdjAndNetBuyRate(snapshot);
      Decimal num1 = 0M;
      for (int index = 2103; index <= 2141; index += 2)
        num1 += this.getNumField(index.ToString());
      for (int index = 3455; index <= 3473; index += 2)
        num1 += this.getNumField(index.ToString());
      for (int index = 4257; index <= 4275; index += 2)
        num1 += this.getNumField(index.ToString());
      for (int index = 4337; index <= 4355; index += 2)
        num1 += this.getNumField(index.ToString());
      Decimal num2 = num1 + this.getNumField("4787");
      this.setField("2142", num2.ToString("N3"));
      Decimal num3 = num2 + this.getNumField("2101");
      this.setField("2143", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("2143") ? num3.ToString("N10") : num3.ToString("N3"));
      Decimal num4 = 0M;
      for (int index = 2649; index <= 2687; index += 2)
        num4 += this.getNumField(index.ToString());
      if (num4 == 0M)
        this.setField("2688", "");
      else
        this.setField("2688", num4.ToString("N3"));
      num4 += this.getNumField("2647");
      if (num4 == 0M)
        this.setField("2689", "");
      else
        this.setField("2689", num4.ToString("N3"));
    }

    public void PerformSnapshotCalculations(
      Hashtable snapshot,
      bool fromTradeLoanUpdate,
      bool skipBuySideCalcLockExpDate,
      bool skipSellSideCalcLockExpDate,
      bool skipCompSideCalcLockExpDate)
    {
      this.PerformLockRequestCalculations(snapshot);
      Decimal num1 = 0M;
      for (int index = 2649; index <= 2687; index += 2)
        num1 += this.getNumField(index.ToString());
      if (num1 == 0M)
        this.setField("2688", "");
      else
        this.setField("2688", num1.ToString("N3"));
      num1 += this.getNumField("2647");
      if (num1 == 0M)
        this.setField("2689", "");
      else
        this.setField("2689", num1.ToString("N3"));
      num1 = 0M;
      Decimal num2 = 0M;
      int num3 = 2596;
      int num4 = 2612;
      for (int index = 2373; index <= 2395; index += 2)
      {
        num1 += this.getNumField(index.ToString());
        num2 += this.getNumField(num3.ToString());
        Decimal num5 = this.getNumField(index.ToString()) - this.getNumField(num3.ToString());
        this.setField(num4.ToString(), num5.ToString("N3"));
        ++num3;
        ++num4;
      }
      int num6 = 2593;
      num4 = 2609;
      for (int index = 2211; index <= 2213; ++index)
      {
        num1 += this.getNumField(index.ToString());
        num2 += this.getNumField(num6.ToString());
        Decimal num7 = this.getNumField(index.ToString()) - this.getNumField(num6.ToString());
        this.setField(num4.ToString(), num7.ToString("N3"));
        ++num6;
        ++num4;
      }
      int num8 = 2836;
      num4 = 2838;
      for (int index = 2834; index <= 2835; ++index)
      {
        num1 += this.getNumField(index.ToString());
        num2 += this.getNumField(num8.ToString());
        Decimal num9 = this.getNumField(index.ToString()) - this.getNumField(num8.ToString());
        this.setField(num4.ToString(), num9.ToString("N3"));
        ++num8;
        ++num4;
      }
      num1 += this.getNumField("3131");
      Decimal num10 = num2 + this.getNumField("3130");
      this.setField("3132", (this.getNumField("3131") - this.getNumField("3130")).ToString("N3"));
      this.setField("2214", num1.ToString("N3"));
      this.setField("2608", num10.ToString("N3"));
      num1 -= num10;
      this.setField("2624", num1.ToString("N3"));
      if (num1 < 0M)
      {
        num1 = Math.Abs(num1);
        this.setField("2627", "Lender");
        this.setField("2628", "Lender");
      }
      else if (num1 > 0M)
      {
        this.setField("2627", "Investor");
        this.setField("2628", "Investor");
      }
      else
      {
        this.setField("2627", "");
        this.setField("2628", "");
      }
      this.setField("2631", num1.ToString("N3"));
      if (this.sessionObjects.ServerLicense.IsBankerEdition)
      {
        num1 -= this.getNumField("2632");
        this.setField("2629", num1.ToString("N3"));
      }
      else
        this.setField("2629", "");
      if (!fromTradeLoanUpdate && !skipBuySideCalcLockExpDate && !Utils.IsDate((object) this.getField("3364")) && Utils.ParseInt((object) this.getField("3433"), 0) <= 0)
      {
        if (Utils.IsDate((object) this.getField("2149")))
        {
          if (this.getNumField("2150") != 0M)
          {
            try
            {
              this.setField("2151", this.CalculateLockExpirationDate(DateTime.Parse(this.getField("2149")), (int) this.getNumField("2150")).ToString("MM/dd/yyyy"));
              goto label_39;
            }
            catch
            {
              goto label_39;
            }
          }
        }
        if (Utils.IsDate((object) this.getField("2149")))
        {
          if (Utils.IsDate((object) this.getField("2151")))
          {
            try
            {
              this.setField("2150", this.CalculateLockDays(DateTime.Parse(this.getField("2149")), DateTime.Parse(this.getField("2151"))).ToString());
              goto label_39;
            }
            catch
            {
              goto label_39;
            }
          }
        }
        if (this.getNumField("2150") != 0M)
        {
          if (Utils.IsDate((object) this.getField("2151")))
          {
            try
            {
              this.setField("2149", this.CalculateLockDate(DateTime.Parse(this.getField("2151")), (int) this.getNumField("2150")).ToString("MM/dd/yyyy"));
            }
            catch
            {
            }
          }
        }
      }
label_39:
      List<string> fieldIds1 = new List<string>()
      {
        "2154",
        "2156",
        "2158"
      };
      num1 = this.getNumField("2154") + this.getNumField("2156") + this.getNumField("2158");
      for (int index = 2449; index <= 2481; index += 2)
      {
        fieldIds1.Add(index.ToString());
        num1 += this.getNumField(index.ToString());
      }
      if (this.existsValueForFieldList(fieldIds1))
        this.setField("2159", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("2159") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("2159", "");
      List<string> fieldIds2 = new List<string>()
      {
        "2152",
        "2159"
      };
      num1 += this.getNumField("2152");
      if (this.existsValueForFieldList(fieldIds2))
        this.setField("2160", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("2160") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("2160", "");
      num1 = this.getNumField("2161");
      for (int index = 3381; index <= 3407; index += 2)
        num1 += this.getNumField(index.ToString());
      if (num1 != 0M)
        this.setField("3420", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("3420") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("3420", "");
      num1 = 0M;
      Decimal num11 = 0M + this.getNumField("3371") + this.getNumField("4753") + this.getNumField("4757") + this.getNumField("4761") + this.getNumField("4765");
      if (num11 != 0M)
        this.setField("4858", num11.ToString("N3"));
      else
        this.setField("4858", "");
      Decimal num12 = 0M + this.getNumField("3375") + this.getNumField("4769") + this.getNumField("4773") + this.getNumField("4777") + this.getNumField("4781");
      if (num12 != 0M)
        this.setField("4857", num12.ToString("N3"));
      else
        this.setField("4857", "");
      num1 = num11 + num12;
      if (num1 != 0M)
        this.setField("3379", num1.ToString("N3"));
      else
        this.setField("3379", "");
      num1 = 0M;
      for (int index = 2163; index <= 2201; index += 2)
        num1 += this.getNumField(index.ToString());
      for (int index = 3475; index <= 3493; index += 2)
        num1 += this.getNumField(index.ToString());
      for (int index = 4277; index <= 4295; index += 2)
        num1 += this.getNumField(index.ToString());
      for (int index = 4357; index <= 4375; index += 2)
        num1 += this.getNumField(index.ToString());
      num1 += this.getNumField("3371");
      num1 += this.getNumField("4753");
      num1 += this.getNumField("4757");
      num1 += this.getNumField("4761");
      num1 += this.getNumField("4765");
      num1 += this.getNumField("3375");
      num1 += this.getNumField("4769");
      num1 += this.getNumField("4773");
      num1 += this.getNumField("4777");
      num1 += this.getNumField("4781");
      if (num1 != 0M)
        this.setField("2202", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("2202") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("2202", "");
      num1 += this.getNumField("3420");
      if (num1 != 0M)
        this.setField("2203", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("2203") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("2203", "");
      num1 = 0M;
      for (int index = 2735; index <= 2773; index += 2)
        num1 += this.getNumField(index.ToString());
      if (num1 != 0M)
        this.setField("2774", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("2774") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("2774", "");
      num1 += this.getNumField("2733");
      if (num1 != 0M)
        this.setField("2775", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("2775") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("2775", "");
      if (!fromTradeLoanUpdate && !skipSellSideCalcLockExpDate && !Utils.IsDate((object) this.getField("3364")))
      {
        if (Utils.IsDate((object) this.getField("2220")))
        {
          if (Utils.IsDate((object) this.getField("2222")))
          {
            try
            {
              this.setField("2221", this.CalculateLockDays(DateTime.Parse(this.getField("2220")), DateTime.Parse(this.getField("2222"))).ToString());
              goto label_99;
            }
            catch
            {
              goto label_99;
            }
          }
        }
        if (this.getField("2220") != "")
        {
          if (this.getNumField("2221") != 0M)
          {
            try
            {
              this.setField("2222", this.CalculateLockExpirationDate(DateTime.Parse(this.getField("2220")), (int) this.getNumField("2221")).ToString("MM/dd/yyyy"));
            }
            catch
            {
            }
          }
        }
      }
label_99:
      num1 = this.getNumField("2225") + this.getNumField("2227") + this.getNumField("2229");
      for (int index = 2483; index <= 2515; index += 2)
        num1 += this.getNumField(index.ToString());
      if (num1 != 0M)
        this.setField("2230", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("2230") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("2230", "");
      num1 += this.getNumField("2223");
      if (num1 != 0M)
        this.setField("2231", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("2231") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("2231", "");
      num1 = 0M;
      for (int index = 2234; index <= 2272; index += 2)
        num1 += this.getNumField(index.ToString());
      for (int index = 3495; index <= 3513; index += 2)
        num1 += this.getNumField(index.ToString());
      for (int index = 4297; index <= 4315; index += 2)
        num1 += this.getNumField(index.ToString());
      for (int index = 4377; index <= 4395; index += 2)
        num1 += this.getNumField(index.ToString());
      if (num1 != 0M)
        this.setField("2273", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("2273") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("2273", "");
      num1 += this.getNumField("2232");
      if (num1 != 0M)
        this.setField("2274", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("2274") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("2274", "");
      if (num1 != 0M)
      {
        num1 -= 100M;
        this.setField("2277", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("2277") ? num1.ToString("N10") : num1.ToString("N3"));
      }
      num1 = 0M;
      for (int index = 2778; index <= 2816; index += 2)
        num1 += this.getNumField(index.ToString());
      if (num1 != 0M)
        this.setField("2817", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("2817") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("2817", "");
      num1 += this.getNumField("2776");
      if (num1 != 0M)
        this.setField("2818", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("2818") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("2818", "");
      num1 = this.getNumField("2203") + this.getNumField("2205");
      if (num1 != 0M)
        this.setField("2218", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("2218") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("2218", "");
      num1 = this.getNumField("2274") + this.getNumField("2276");
      if (num1 != 0M)
        this.setField("2295", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("2295") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("2295", "");
      if (this.getNumField("2295") != 0M && this.getNumField("2218") != 0M)
      {
        num1 = this.loanData == null || (!(this.loanData.GetField("2626") == "Banked - Retail") || !(bool) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingRetail"]) && (!(this.loanData.GetField("2626") == "Banked - Wholesale") || !(bool) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingWholesale"]) ? this.getNumField("2295") - this.getNumField("2218") : this.getNumField("2295") + this.getNumField("2218") - 100M;
        if (num1 == 0M)
          this.setField("2296", "");
        else
          this.setField("2296", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("2296") ? num1.ToString("N10") : num1.ToString("N3"));
      }
      else
      {
        num1 = 0M;
        this.setField("2296", "");
      }
      num1 /= 100M;
      Decimal numField1 = this.getNumField("2965");
      if (numField1 == 0M)
        numField1 = this.getNumField("1109");
      if (numField1 != 0M)
      {
        num1 *= numField1;
        if (num1 == 0M)
          this.setField("2028", "");
        else
          this.setField("2028", num1.ToString("N2"));
      }
      if (!fromTradeLoanUpdate && !skipCompSideCalcLockExpDate && !Utils.IsDate((object) this.getField("3364")) && this.getField("3664") != "")
      {
        if (this.getNumField("3665") != 0M)
        {
          try
          {
            this.setField("3666", this.CalculateLockExpirationDate(DateTime.Parse(this.getField("3664")), (int) this.getNumField("3665")).ToString("MM/dd/yyyy"));
          }
          catch
          {
          }
        }
      }
      num1 = 0M;
      for (int index = 3673; index <= 3711; index += 2)
        num1 += this.getNumField(index.ToString());
      if (num1 != 0M)
        this.setField("3712", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("3712") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("3712", "");
      num1 += this.getNumField("3671");
      if (num1 != 0M)
        this.setField("3713", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("3713") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("3713", "");
      num1 = 0M;
      for (int index = 3716; index <= 3774; index += 2)
        num1 += this.getNumField(index.ToString());
      for (int index = 4317; index <= 4335; index += 2)
        num1 += this.getNumField(index.ToString());
      for (int index = 4397; index <= 4415; index += 2)
        num1 += this.getNumField(index.ToString());
      if (num1 != 0M)
        this.setField("3775", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("3775") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("3775", "");
      num1 += this.getNumField("3714");
      if (num1 != 0M)
        this.setField("3776", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("3776") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("3776", "");
      if (num1 != 0M)
      {
        num1 -= 100M;
        this.setField("3821", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("3820") ? num1.ToString("N10") : num1.ToString("N3"));
      }
      num1 = 0M;
      for (int index = 3779; index <= 3817; index += 2)
        num1 += this.getNumField(index.ToString());
      if (num1 != 0M)
        this.setField("3818", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("3818") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("3818", "");
      num1 += this.getNumField("3777");
      if (num1 != 0M)
        this.setField("3819", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("3819") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("3819", "");
      num1 = this.getNumField("3776") + this.getNumField("3820");
      if (num1 != 0M)
        this.setField("3835", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("3835") ? num1.ToString("N10") : num1.ToString("N3"));
      else
        this.setField("3835", "");
      num1 = this.getNumField("3835") - this.getNumField("2295");
      if (num1 == 0M)
        this.setField("3836", "");
      else
        this.setField("3836", LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields("3836") ? num1.ToString("N10") : num1.ToString("N3"));
      num1 /= 100M;
      Decimal numField2 = this.getNumField("2965");
      if (numField2 == 0M)
        numField2 = this.getNumField("1109");
      if (!(numField2 != 0M))
        return;
      num1 *= numField2;
      if (num1 == 0M)
        this.setField("3837", "");
      else
        this.setField("3837", num1.ToString("N2"));
    }

    public void CalcTotalBaseRateAdjAndNetBuyRate(Hashtable snapshot)
    {
      Decimal num1 = this.getNumField("2094", snapshot) + this.getNumField("2096", snapshot) + this.getNumField("2098", snapshot);
      for (int index = 2415; index <= 2447; index += 2)
        num1 += this.getNumField(index.ToString(), snapshot);
      if (num1 == 0M)
        snapshot[(object) "2099"] = (object) "";
      else
        snapshot[(object) "2099"] = (object) num1.ToString("N3");
      Decimal num2 = num1 + this.getNumField("2092", snapshot);
      snapshot[(object) "2100"] = (object) num2.ToString("N3");
    }

    private bool existsValueForFieldList(List<string> fieldIds)
    {
      return fieldIds.Exists((Predicate<string>) (i => !string.IsNullOrEmpty(this.getField(i))));
    }

    public DateTime CalculateLockExpirationDate(DateTime lockDate, int lockDays)
    {
      return this.CalculateLockExpirationDate(lockDate, lockDays, false);
    }

    public DateTime CalculateLockExpirationDate(
      DateTime lockDate,
      int lockDays,
      bool ignoreSettings)
    {
      if (lockDays > 9999)
        lockDays = 9999;
      if (ignoreSettings)
        return lockDate.AddDays((double) lockDays);
      DateTime rawExpireDate = lockDate.AddDays((double) (lockDays - 1));
      if ((LockExpDayCountSetting) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.LockExpDayCount"] == LockExpDayCountSetting.OneDayAfter)
        rawExpireDate = rawExpireDate.AddDays(1.0);
      return this.GetNextClosestLockExpirationDate(rawExpireDate);
    }

    public DateTime GetNextClosestLockExpirationDate(DateTime rawExpireDate)
    {
      BusinessCalendar expirationCalendar = LockDeskHoursUtils.GetLockExpirationCalendar(this.sessionObjects.StartupInfo, this.sessionObjects);
      if (expirationCalendar == null)
        return rawExpireDate;
      DateTime closestBusinessDay = expirationCalendar.GetNextClosestBusinessDay(rawExpireDate);
      if ((LockExpDayExcludeSetting) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.LockExpExclude"] == LockExpDayExcludeSetting.PreviousBusinessDay)
        closestBusinessDay = expirationCalendar.GetPreviousClosestBusinessDay(rawExpireDate);
      return closestBusinessDay;
    }

    public DateTime CalculateLockDate(DateTime expireDate, int lockDays)
    {
      return this.CalculateLockDate(expireDate, lockDays, false);
    }

    public DateTime CalculateLockDate(DateTime expireDate, int lockDays, bool ignoreSettings)
    {
      if (lockDays > 9999)
        lockDays = 9999;
      DateTime lockDate = expireDate.AddDays((double) (-1 * (lockDays - 1)));
      if (ignoreSettings || (LockExpDayCountSetting) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.LockExpDayCount"] != LockExpDayCountSetting.OneDayAfter)
        return lockDate;
      lockDate = lockDate.AddDays(-1.0);
      return lockDate;
    }

    public int CalculateLockDays(DateTime lockDate, DateTime expiredDate)
    {
      return this.CalculateLockDays(lockDate, expiredDate, false);
    }

    public int CalculateLockDays(DateTime lockDate, DateTime expireDate, bool ignoreSettings)
    {
      int lockDays = (expireDate - lockDate).Days;
      if (!ignoreSettings && (LockExpDayCountSetting) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.LockExpDayCount"] == LockExpDayCountSetting.OnTheDay)
        ++lockDays;
      if (lockDays > 9999)
        lockDays = 9999;
      return lockDays;
    }

    private Decimal getNumField(string id)
    {
      return LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields(id) && this.sessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas ? Utils.ParseDecimal(this.snapshot[(object) id], 0M, 10) : Utils.ParseDecimal(this.snapshot[(object) id], 0M, 3);
    }

    private Decimal getNumField(string id, Hashtable snapshot)
    {
      return LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields(id) && this.sessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas ? Utils.ParseDecimal(snapshot[(object) id], 0M, 10) : Utils.ParseDecimal(snapshot[(object) id], 0M, 3);
    }

    private void setField(string id, string val) => this.snapshot[(object) id] = (object) val;

    private string getField(string id) => string.Concat(this.snapshot[(object) id]);

    internal void AddInitialLockValidationTrigger()
    {
      IList<LockComparisonField> comparisonFields = this.sessionObjects.StartupInfo.LockComparisonFields;
      if (comparisonFields == null || comparisonFields.Count <= 0)
        return;
      foreach (LockComparisonField lockComparisonField in (IEnumerable<LockComparisonField>) comparisonFields)
        this.AddFieldHandler(lockComparisonField.LoanFieldId, new Routine(this.AddLockComparisonFieldsModified));
    }

    private void AddLockComparisonFieldsModified(string id, string val)
    {
      LoanDataMgr dataMgr = (LoanDataMgr) this.loanData.DataMgr;
      LockComparisonField lockComparisonField = this.sessionObjects.StartupInfo.LockComparisonFields.FirstOrDefault<LockComparisonField>((Func<LockComparisonField, bool>) (f => f.LoanFieldId == id));
      if (lockComparisonField == null)
        return;
      dataMgr.LockComparisonFieldsModified.Add(lockComparisonField);
    }
  }
}
