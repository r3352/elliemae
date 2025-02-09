// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ProductPricing.LoanLockUtils
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.EPC2;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Common.ProductPricing
{
  public class LoanLockUtils
  {
    private static Dictionary<string, string> pricingFieldsBuySideToRequestMap = LoanLockUtils.InitializePricingFieldsBuySideToRequestMap();

    public static RateLockRequestStatus TranslateRateLockRequestStatus(LockRequestLog l)
    {
      if (l.RequestedStatus == string.Empty)
        return RateLockRequestStatus.Requested;
      if (l.SellSideExpirationDate == DateTime.MinValue && l.BuySideExpirationDate == DateTime.MinValue)
        return l.LockRequestStatus;
      DateTime today = DateTime.Today;
      DateTime t1 = !l.IsLockExtension ? l.BuySideExpirationDate : l.BuySideNewLockExtensionDate;
      if (t1 != DateTime.MinValue && DateTime.Compare(t1, today) < 0)
      {
        if (string.Compare(l.RequestedStatus, RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.Requested), true) == 0)
          return RateLockRequestStatus.RateExpired;
        if (string.Compare(l.RequestedStatus, RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked), true) == 0)
          return RateLockRequestStatus.OldLock;
      }
      return l.LockRequestStatus;
    }

    public static string FindLockStatus(LockRequestLog l)
    {
      if (l.RequestedStatus == string.Empty)
        return RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.Requested);
      if (l.SellSideExpirationDate == DateTime.MinValue && l.BuySideExpirationDate == DateTime.MinValue)
        return l.RequestedStatus;
      DateTime today = DateTime.Today;
      DateTime t1 = !l.IsLockExtension ? l.BuySideExpirationDate : l.BuySideNewLockExtensionDate;
      if (t1 != DateTime.MinValue && DateTime.Compare(t1, today) < 0)
      {
        if (string.Compare(l.RequestedStatus, RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.Requested), true) == 0)
          return RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateExpired);
        if (string.Compare(l.RequestedStatus, RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked), true) == 0)
          return RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.OldLock);
      }
      return l.LockRequestStatus == RateLockRequestStatus.Voided ? RateLockRequestStatus.Voided.ToString() : l.RequestedStatus;
    }

    public static void IncludeAdjustments(
      GridView gView,
      Hashtable loanSnapshotHashtable,
      int startField,
      string extensionDesc)
    {
      for (int index = 0; index < 10; ++index)
      {
        string key1 = (startField + 2 * index).ToString();
        string key2 = (startField + 2 * index + 1).ToString();
        if (loanSnapshotHashtable.ContainsKey((object) key1) || loanSnapshotHashtable.ContainsKey((object) key2))
        {
          string text = "";
          if (loanSnapshotHashtable.ContainsKey((object) key1))
            text = extensionDesc + (index + 1).ToString() + " - " + loanSnapshotHashtable[(object) key1].ToString();
          string str = "";
          if (loanSnapshotHashtable.ContainsKey((object) key2))
            str = loanSnapshotHashtable[(object) key2].ToString();
          gView.Items.Add(new GVItem(text)
          {
            SubItems = {
              (object) str
            }
          });
        }
      }
    }

    public static bool IsLockExtendable(LoanDataMgr loanMgr)
    {
      bool flag = false;
      if (loanMgr.LoanData != null && !loanMgr.LoanData.IsTemplate)
      {
        LockRequestLog lockRequestLog = loanMgr.LoanData.GetLogList().GetCurrentConfirmedLockRequest() ?? LockUtils.GetAssignToTradePostConfirmationLock(loanMgr);
        if (lockRequestLog == null)
          return false;
        if ((!lockRequestLog.IsLockExtension ? lockRequestLog.BuySideExpirationDate : lockRequestLog.BuySideNewLockExtensionDate) >= DateTime.Today)
          flag = true;
      }
      return flag;
    }

    public static bool IsLockCancellable(LoanDataMgr loanMgr)
    {
      bool flag = false;
      if (loanMgr.LoanData == null || loanMgr.LoanData.IsTemplate)
        return flag;
      return (loanMgr.LoanData.GetLogList().GetCurrentConfirmedLockRequest() ?? LockUtils.GetAssignToTradePostConfirmationLock(loanMgr)) != null && !(loanMgr.LoanData.GetField("LOCKRATE.RATEREQUESTSTATUS") == "Expired-NoRequest") && !(loanMgr.LoanData.GetField("LOCKRATE.RATEREQUESTSTATUS") == "Expired-Request");
    }

    public static void CacheLockRequestPriceFields(
      LoanDataMgr loanMgr,
      Dictionary<string, string> cache)
    {
      cache.Clear();
      foreach (string str in LoanLockUtils.PricingFieldsBuySideToRequestMap.Values)
      {
        string field = loanMgr.LoanData.GetField(str);
        cache[str] = field;
      }
    }

    public static void UncacheLockRequestPriceFields(
      LoanDataMgr loanMgr,
      Dictionary<string, string> cache)
    {
      foreach (string str in LoanLockUtils.PricingFieldsBuySideToRequestMap.Values)
      {
        string val = cache.ContainsKey(str) ? cache[str] : "";
        loanMgr.LoanData.SetField(str, val);
      }
    }

    public static Dictionary<string, string> CopyPriceDataFromSnapshotToRequestFields(
      LoanDataMgr loanMgr,
      LockRequestLog source)
    {
      Dictionary<string, string> requestFields = new Dictionary<string, string>();
      Hashtable lockRequestSnapshot = source.GetLockRequestSnapshot();
      foreach (KeyValuePair<string, string> buySideToRequest in LoanLockUtils.PricingFieldsBuySideToRequestMap)
      {
        string key = buySideToRequest.Key;
        if (key == "2151" && source.IsLockExtension && lockRequestSnapshot.ContainsKey((object) "3358"))
          key = "3358";
        string val = lockRequestSnapshot.ContainsKey((object) key) ? lockRequestSnapshot[(object) key].ToString() : "";
        loanMgr.LoanData.SetField(buySideToRequest.Value, val);
      }
      if (lockRequestSnapshot.ContainsKey((object) "3364"))
        requestFields.Add("3364", lockRequestSnapshot[(object) "3364"].ToString());
      if (lockRequestSnapshot.ContainsKey((object) "3431"))
        requestFields.Add("3431", lockRequestSnapshot[(object) "3431"].ToString());
      if (lockRequestSnapshot.ContainsKey((object) "2150"))
        requestFields.Add("2150", lockRequestSnapshot[(object) "2150"].ToString());
      return requestFields;
    }

    public static Dictionary<string, string> PricingFieldsBuySideToRequestMap
    {
      get => LoanLockUtils.pricingFieldsBuySideToRequestMap;
    }

    public static Dictionary<string, string> InitializePricingFieldsBuySideToRequestMap()
    {
      Dictionary<string, string> requestMap = new Dictionary<string, string>();
      for (int index = 2092; index <= 2143; ++index)
      {
        if (index == 2101)
          requestMap.Add("3420", "2101");
        else
          requestMap.Add((index + 60).ToString(), index.ToString());
      }
      for (int index = 2414; index <= 2447; ++index)
        requestMap.Add((index + 34).ToString(), index.ToString());
      for (int index = 2647; index <= 2689; ++index)
        requestMap.Add((index + 86).ToString(), index.ToString());
      for (int index = 3454; index <= 3473; ++index)
        requestMap.Add((index + 20).ToString(), index.ToString());
      for (int index = 4256; index <= 4275; ++index)
        requestMap.Add((index + 20).ToString(), index.ToString());
      for (int index = 4336; index <= 4355; ++index)
        requestMap.Add((index + 20).ToString(), index.ToString());
      requestMap.Add("2148", "2088");
      requestMap.Add("2149", "2089");
      requestMap.Add("2150", "2090");
      requestMap.Add("2151", "2091");
      requestMap.Add("3256", "3254");
      requestMap.Add("3848", "3847");
      requestMap.Add("3873", "3872");
      requestMap.Add("3875", "3874");
      requestMap.Add("3911", "3965");
      requestMap.Add("3910", "4187");
      return requestMap;
    }

    public static bool IsAllowGetPricingForReLock(SessionObjects session, LoanDataMgr loanDatamgr)
    {
      if (loanDatamgr == null)
        return false;
      string field = loanDatamgr.LoanData.GetField("LOCKRATE.RATESTATUS");
      if (!(field == "Cancelled") && !(field == "Expired"))
      {
        if (session.StartupInfo == null || session.StartupInfo.ProductPricingPartner == null || session.ServerManager == null || loanDatamgr == null || loanDatamgr.LoanData == null)
          return false;
        if (session.StartupInfo.ProductPricingPartner.IsEPPS && !LockUtils.IfShipDark(session, "EPPS_EPC2_SHIP_DARK_SR") && session.StartupInfo.ProductPricingPartner.VendorPlatform == VendorPlatform.EPC2)
          return true;
        if (!session.StartupInfo.ProductPricingPartner.IsEPPS || !session.StartupInfo.ProductPricingPartner.GetPricingRelock || !(bool) session.StartupInfo.PolicySettings[(object) "Policies.EnableRelock"] || !ProductPricingUtils.IsHistoricalPricingEnabled)
          return false;
        LockRequestLog confirmedLockForRelocks = LoanLockUtils.GetPreviousConfirmedLockForRelocks(loanDatamgr);
        if (confirmedLockForRelocks == null)
          return false;
        Hashtable lockRequestSnapshot = confirmedLockForRelocks.GetLockRequestSnapshot();
        if (!lockRequestSnapshot.Contains((object) "OPTIMAL.HISTORY") || !lockRequestSnapshot[(object) "OPTIMAL.HISTORY"].ToString().Contains("<MPS_envelope"))
          return false;
      }
      else if (!(bool) session.ServerManager.GetServerSetting("Policies.EnableRelock") || LockUtils.GetRelockState(session, loanDatamgr.LoanData) == LockUtils.RelockState.RelockExpiredWithinDaysCap)
        return false;
      return true;
    }

    public static LockRequestLog GetPreviousConfirmedLockForRelocks(LoanDataMgr loanDatamgr)
    {
      return loanDatamgr == null || loanDatamgr.LoanData.GetLogList() == null ? (LockRequestLog) null : loanDatamgr.LoanData.GetLogList().GetCurrentConfirmedLockRequest() ?? LockUtils.GetAssignToTradePostConfirmationLock(loanDatamgr);
    }

    public static LoanLockUtils.AllowedRequestType GetAllowedRequestType(
      SessionObjects session,
      LoanDataMgr loanDatamgr)
    {
      LoanLockUtils.AllowedRequestType allowedRequestType = LoanLockUtils.AllowedRequestType.NewLockOnly;
      string field = loanDatamgr.LoanData.GetField("LOCKRATE.RATESTATUS");
      bool flag = field == "Cancelled" || field == "Expired";
      if ((bool) session.StartupInfo.PolicySettings[(object) "Policies.EnableRelock"] && LoanLockUtils.GetPreviousConfirmedLockForRelocks(loanDatamgr) != null | flag)
      {
        allowedRequestType = LoanLockUtils.AllowedRequestType.BothNewLockAndReLock;
        if ((bool) session.ServerManager.GetServerSetting("Policies.RelockOnly") | flag)
          allowedRequestType = LoanLockUtils.AllowedRequestType.ReLockOnly;
      }
      return allowedRequestType;
    }

    public enum AllowedRequestType
    {
      NewLockOnly,
      ReLockOnly,
      BothNewLockAndReLock,
    }
  }
}
