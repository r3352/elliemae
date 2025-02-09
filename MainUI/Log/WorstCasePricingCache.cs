// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.WorstCasePricingCache
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common.ProductPricing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public static class WorstCasePricingCache
  {
    private static string _loanGuid = "";
    private static string _lastConfirmedLockGuid = "";
    private static Hashtable _historicalPriceTable = new Hashtable();
    private static Hashtable _currentPriceTable = new Hashtable();
    private static List<string> _currentResponseFields = new List<string>();
    private static bool _isLockExtension = false;
    private static List<string> _historicalResponseFields = new List<string>();

    public static Hashtable HistoricalPriceTable => WorstCasePricingCache._historicalPriceTable;

    public static Hashtable CurrentPriceTable => WorstCasePricingCache._currentPriceTable;

    public static List<string> HistoricalResponseFields
    {
      get => WorstCasePricingCache._historicalResponseFields;
      set => WorstCasePricingCache._historicalResponseFields = value;
    }

    public static List<string> CurrentResponseFields
    {
      get => WorstCasePricingCache._currentResponseFields;
      set => WorstCasePricingCache._currentResponseFields = value;
    }

    public static bool IsLockExtension
    {
      get => WorstCasePricingCache._isLockExtension;
      set => WorstCasePricingCache._isLockExtension = value;
    }

    private static void LoadDataTables()
    {
      WorstCasePricingCache.ClearDataTables();
      if (Session.LoanDataMgr == null || Session.LoanDataMgr.LoanData == null || Session.LoanDataMgr.LoanData.GetLogList() == null)
        return;
      WorstCasePricingCache._loanGuid = Session.LoanDataMgr.LoanData.GUID;
      Session.LoanDataMgr.LoanClosing -= new EventHandler(WorstCasePricingCache.LoanDataMgrOnLoanClosing);
      Session.LoanDataMgr.LoanClosing += new EventHandler(WorstCasePricingCache.LoanDataMgrOnLoanClosing);
      LockRequestLog confirmedLockForWcpt = WorstCasePricingCache.GetLastConfirmedLockForWcpt(Session.LoanDataMgr);
      if (confirmedLockForWcpt == null)
        return;
      WorstCasePricingCache._lastConfirmedLockGuid = confirmedLockForWcpt.Guid;
      Hashtable lockRequestSnapshot = confirmedLockForWcpt.GetLockRequestSnapshot();
      if (lockRequestSnapshot != null)
      {
        foreach (object key in (IEnumerable) lockRequestSnapshot.Keys)
          WorstCasePricingCache._historicalPriceTable[key] = lockRequestSnapshot[key];
        Dictionary<string, string> requestMap = LoanLockUtils.InitializePricingFieldsBuySideToRequestMap();
        foreach (string key in requestMap.Values)
        {
          if (WorstCasePricingCache._historicalPriceTable.Contains((object) key))
            WorstCasePricingCache._historicalPriceTable[(object) key] = (object) "";
        }
        foreach (string key in requestMap.Keys)
        {
          if (lockRequestSnapshot[(object) key] != null)
            WorstCasePricingCache._historicalPriceTable[(object) requestMap[key]] = lockRequestSnapshot[(object) key];
        }
        WorstCasePricingCache._historicalPriceTable[(object) "2144"] = lockRequestSnapshot[(object) "2204"] == null ? (object) "" : lockRequestSnapshot[(object) "2204"];
      }
      WorstCasePricingCache._isLockExtension = confirmedLockForWcpt.IsLockExtension;
      if (!confirmedLockForWcpt.IsLockExtension)
        return;
      if (WorstCasePricingCache._historicalPriceTable[(object) "2150"] != null)
        WorstCasePricingCache._historicalPriceTable[(object) "2090"] = WorstCasePricingCache._historicalPriceTable[(object) "2150"];
      if (WorstCasePricingCache._historicalPriceTable[(object) "3358"] != null)
        WorstCasePricingCache._historicalPriceTable[(object) "2091"] = WorstCasePricingCache._historicalPriceTable[(object) "3358"];
      if (WorstCasePricingCache._historicalPriceTable[(object) "2151"] != null)
        WorstCasePricingCache._historicalPriceTable[(object) "3369"] = WorstCasePricingCache._historicalPriceTable[(object) "2151"];
      if (WorstCasePricingCache._historicalPriceTable[(object) "3363"] != null)
        WorstCasePricingCache._historicalPriceTable[(object) "3360"] = WorstCasePricingCache._historicalPriceTable[(object) "3363"];
      if (WorstCasePricingCache._historicalPriceTable[(object) "3364"] != null)
        WorstCasePricingCache._historicalPriceTable[(object) "3361"] = WorstCasePricingCache._historicalPriceTable[(object) "3364"];
      if (WorstCasePricingCache._historicalPriceTable[(object) "3365"] == null)
        return;
      WorstCasePricingCache._historicalPriceTable[(object) "3362"] = WorstCasePricingCache._historicalPriceTable[(object) "3365"];
    }

    public static void RefreshDataTables()
    {
      if (Session.LoanDataMgr == null || Session.LoanDataMgr.LoanData == null || Session.LoanDataMgr.LoanData.GetLogList() == null)
      {
        WorstCasePricingCache.ClearDataTables();
      }
      else
      {
        LockRequestLog lockRequestLog = (LockRequestLog) null;
        string field = Session.LoanDataMgr.LoanData.GetField("LOCKRATE.RATESTATUS");
        if (!(field == "Cancelled") && !(field == "Expired"))
        {
          lockRequestLog = Session.LoanDataMgr.LoanData.GetLogList().GetLastConfirmedLock();
        }
        else
        {
          LockConfirmLog recentConfirmedLock = Session.LoanDataMgr.LoanData.GetLogList().GetMostRecentConfirmedLock();
          if (recentConfirmedLock != null)
            lockRequestLog = Session.LoanDataMgr.LoanData.GetLogList().GetLockRequest(recentConfirmedLock.RequestGUID);
        }
        if (lockRequestLog == null || !(WorstCasePricingCache._loanGuid == "") && !(WorstCasePricingCache._loanGuid != Session.LoanDataMgr.LoanData.GUID) && !(WorstCasePricingCache._lastConfirmedLockGuid != lockRequestLog.Guid))
          return;
        WorstCasePricingCache.LoadDataTables();
      }
    }

    public static void ClearDataTables()
    {
      WorstCasePricingCache._historicalPriceTable.Clear();
      WorstCasePricingCache._currentPriceTable.Clear();
      WorstCasePricingCache._loanGuid = "";
      WorstCasePricingCache._lastConfirmedLockGuid = "";
      WorstCasePricingCache._historicalResponseFields.Clear();
      WorstCasePricingCache._currentResponseFields.Clear();
    }

    private static void LoanDataMgrOnLoanClosing(object sender, EventArgs eventArgs)
    {
      WorstCasePricingCache.ClearDataTables();
      if (Session.LoanDataMgr == null)
        return;
      Session.LoanDataMgr.LoanClosing -= new EventHandler(WorstCasePricingCache.LoanDataMgrOnLoanClosing);
    }

    public static LockRequestLog GetLastConfirmedLockForWcpt(LoanDataMgr dataMgr)
    {
      if (dataMgr == null)
        return (LockRequestLog) null;
      string field = dataMgr.LoanData.GetField("LOCKRATE.RATESTATUS");
      if (!(field == "Locked") && !(field == "Expired") && !(field == "Cancelled"))
        return (LockRequestLog) null;
      LockConfirmLog recentConfirmedLock = dataMgr.LoanData.GetLogList().GetMostRecentConfirmedLock();
      return recentConfirmedLock == null ? (LockRequestLog) null : dataMgr.LoanData.GetLogList().GetLockRequest(recentConfirmedLock.RequestGUID);
    }
  }
}
