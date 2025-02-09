// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanXDBStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.ReportingDbUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class LoanXDBStore
  {
    private const string className = "LoanXDBStore�";
    private const string statusCacheKey = "LoanXDBStatus�";
    private const string mutexName = "LoanXDBMutex�";
    private static readonly TimeSpan lockWaitTimeout = TimeSpan.FromSeconds(30.0);

    public static void SetLoanXDBTableList(bool useERDB, LoanXDBTableList tableList, string userId)
    {
      if (tableList == null)
      {
        TraceLog.WriteWarning(nameof (LoanXDBStore), "Report Table List is null.");
      }
      else
      {
        ClientContext current = ClientContext.GetCurrent();
        string str = nameof (LoanXDBStore) + (useERDB ? "|ERDB" : "");
        using (current.Cache.Lock(str))
        {
          current.Cache.Remove(str);
          ERDBSession.SetDbLoanXDBTableList(useERDB, current, tableList, userId);
        }
      }
    }

    public static string[] GetTableNameList(bool useERDB)
    {
      return LoanXDBStore.GetLoanXDBTableList(useERDB)?.GetTableNameList();
    }

    public static string[] GetTableNameList() => LoanXDBStore.GetTableNameList(false);

    public static LoanXDBTableList GetLoanXDBTableList() => LoanXDBStore.GetLoanXDBTableList(false);

    public static LoanXDBTableList GetLoanXDBTableList(bool useERDB)
    {
      ClientContext context = ClientContext.GetCurrent();
      string key = nameof (LoanXDBStore) + (useERDB ? "|ERDB" : "");
      return context.Cache.Get<LoanXDBTableList>(key, (Func<LoanXDBTableList>) (() => ERDBSession.GetLoanXDBTableListFromDatabase(useERDB, context)), CacheSetting.Low);
    }

    public static void ClearPendingFields(bool useERDB, string userId)
    {
      ClientContext current = ClientContext.GetCurrent();
      string str = nameof (LoanXDBStore) + (useERDB ? "|ERDB" : "");
      using (current.Cache.Lock(str))
      {
        current.Cache.Remove(str);
        ERDBSession.ClearDbPendingFields(useERDB, current, userId);
        current.Cache.Remove("LoanXDBStatus" + (useERDB ? "|ERDB" : ""));
      }
    }

    public static LoanXDBField[] GetAuditTrailLoanXDBField()
    {
      List<LoanXDBField> loanXdbFieldList = new List<LoanXDBField>();
      Hashtable auditableTableList = LoanXDBStore.GetLoanXDBTableList().GetAuditableTableList();
      if (auditableTableList != null)
      {
        foreach (DictionaryEntry dictionaryEntry in auditableTableList)
        {
          LoanXDBTable loanXdbTable = (LoanXDBTable) dictionaryEntry.Value;
          if (loanXdbTable != null)
          {
            for (int i = 0; i < loanXdbTable.FieldCount; ++i)
            {
              LoanXDBField fieldAt = loanXdbTable.GetFieldAt(i);
              if (fieldAt != null)
              {
                if (fieldAt.ComortgagorPair > 1 && fieldAt.Description.IndexOf("Co-Mortgagor Pair:") < 0)
                {
                  LoanXDBField loanXdbField = fieldAt;
                  loanXdbField.Description = loanXdbField.Description + " (Co-Mortgagor Pair: " + (object) fieldAt.ComortgagorPair + ")";
                }
                loanXdbFieldList.Add(fieldAt);
              }
            }
          }
        }
      }
      return loanXdbFieldList.ToArray();
    }

    public static LoanXDBAuditField[] GetAuditTrailReportingLoanXDBField()
    {
      List<LoanXDBAuditField> loanXdbAuditFieldList = new List<LoanXDBAuditField>();
      foreach (LoanXDBField loanXdbField in LoanXDBStore.GetAuditTrailLoanXDBField())
        loanXdbAuditFieldList.AddRange((IEnumerable<LoanXDBAuditField>) loanXdbField.GetAuditTrailFields());
      return loanXdbAuditFieldList.ToArray();
    }

    public static LoanXDBStatusInfo ApplyChanges(
      bool useERDB,
      LoanXDBTableList tableList,
      string validationKey,
      string userId)
    {
      string updateSqlQuery = tableList.GetUpdateSQLQuery();
      LoanXDBStore.acquireLock(useERDB, validationKey);
      try
      {
        ClientContext current = ClientContext.GetCurrent();
        using (current.Cache.Lock(nameof (LoanXDBStore) + (useERDB ? "|ERDB" : "")))
        {
          ERDBSession.ApplyReportingDatabaseChanges(useERDB, current, tableList, updateSqlQuery, userId);
          current.Cache.Remove(nameof (LoanXDBStore) + (useERDB ? "|ERDB" : ""));
          LoanUtil.UpdateXDBSchemaCache((IClientContext) current, tableList);
        }
        LoanXDBStore.releaseLock(useERDB, true);
        return LoanXDBStore.GetLoanXDBStatus(useERDB);
      }
      catch
      {
        LoanXDBStore.releaseLock(useERDB, false);
        throw;
      }
    }

    public static bool ResetReportingDatabase(
      bool useERDB,
      LoanXDBTableList tableList,
      string validationKey,
      bool keepTables,
      IServerProgressFeedback feedback,
      string userId)
    {
      ClientContext current = ClientContext.GetCurrent();
      string str = nameof (LoanXDBStore) + (useERDB ? "|ERDB" : "");
      using (current.Cache.Lock(str))
      {
        LoanXDBTableList loanXdbTableList = keepTables ? tableList : new LoanXDBTableList();
        foreach (LoanXDBField allField in tableList.GetAllFields())
        {
          if (!keepTables)
            allField.TableName = string.Empty;
          loanXdbTableList.AddUpdateList(allField, LoanXDBField.FieldStatus.New);
        }
        if (!keepTables)
        {
          tableList = loanXdbTableList;
          tableList.UpdateTable();
        }
        LoanXDBField[] updateList = tableList.GetUpdateList();
        if (updateList == null || updateList.Length == 0)
          return false;
        LoanXDBStore.acquireLock(useERDB, validationKey);
        try
        {
          ERDBSession.ResetReportingDatabase(useERDB, current, tableList, keepTables, feedback);
          feedback?.SetFeedback((string) null, "", 3);
          LoanXDBStore.SetLoanXDBTableList(useERDB, tableList, userId);
          feedback?.SetFeedback((string) null, "", 4);
          LoanUtil.UpdateXDBSchemaCache((IClientContext) current, tableList);
          LoanXDBStore.releaseLock(useERDB, true);
        }
        catch
        {
          LoanXDBStore.releaseLock(useERDB, false);
          throw;
        }
        current.Cache.Remove(str);
        current.Cache.Remove("LoanXDBStatus" + (useERDB ? "|ERDB" : ""));
      }
      return true;
    }

    public static LoanXDBStatusInfo GetLoanXDBStatus(bool useERDB)
    {
      ClientContext current = ClientContext.GetCurrent();
      string str = "LoanXDBStatus" + (useERDB ? "|ERDB" : "");
      LoanXDBStatusInfo loanXdbStatus1 = (LoanXDBStatusInfo) current.Cache.Get(str);
      if (loanXdbStatus1 != null)
        return loanXdbStatus1;
      try
      {
        using (current.Cache.Lock(str, timeout: ServerGlobals.LockTimeoutDuringGetCache))
        {
          if (ServerGlobals.CacheRegetFromCache)
          {
            LoanXDBStatusInfo loanXdbStatus2 = (LoanXDBStatusInfo) current.Cache.Get(str);
            if (loanXdbStatus2 != null)
              return loanXdbStatus2;
          }
          LoanXDBStatusInfo loanXdbStatus3 = ERDBSession.GetLoanXDBStatus(useERDB, current);
          current.Cache.Put(str, (object) loanXdbStatus3, CacheSetting.Low);
          return loanXdbStatus3;
        }
      }
      catch (TimeoutException ex)
      {
        try
        {
          TraceLog.WriteWarning(nameof (LoanXDBStore), "Timeout expired while acquiring lock on " + str);
        }
        catch
        {
        }
        if (ServerGlobals.CacheRegetFromDB)
          return ERDBSession.GetLoanXDBStatus(useERDB, current);
        throw;
      }
      catch (ApplicationException ex)
      {
        if (ex.Message.IndexOf("timeout period expired") > 0 || ex.HResult == -2147023436)
        {
          try
          {
            TraceLog.WriteWarning(nameof (LoanXDBStore), "Timeout expired while acquiring lock on " + str);
          }
          catch
          {
          }
          if (ServerGlobals.CacheRegetFromDB)
            return ERDBSession.GetLoanXDBStatus(useERDB, current);
          throw;
        }
        else
          throw;
      }
    }

    private static void acquireLock(bool useERDB, string validationKey)
    {
      ClientContext current = ClientContext.GetCurrent();
      string str = "LoanXDBStatus" + (useERDB ? "|ERDB" : "");
      using (current.Cache.Lock(str))
      {
        switch (ERDBSession.AcquireLock(useERDB, ClientContext.GetCurrent(), validationKey))
        {
          case 1:
            throw new LockException("The Reporting Database is currently locked", (LockInfo) null);
          case 2:
            throw new ObjectModifiedException("The Reporting Database has been modified by another user");
          default:
            current.Cache.Remove(str);
            break;
        }
      }
      Thread.Sleep(TimeSpan.FromSeconds(3.0));
    }

    private static void releaseLock(bool useERDB, bool updateTimestamp)
    {
      ClientContext current = ClientContext.GetCurrent();
      string str = "LoanXDBStatus" + (useERDB ? "|ERDB" : "");
      using (current.Cache.Lock(str))
      {
        ERDBSession.ReleaseLock(useERDB, ClientContext.GetCurrent(), updateTimestamp);
        current.Cache.Remove(str);
      }
    }
  }
}
