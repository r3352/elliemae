// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.LoanLockAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using com.elliemae.services.eventbus.models;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.MessageServices.Kafka.Event;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Server.ServiceObjects.KafkaEvent;
using EllieMae.EMLite.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public static class LoanLockAccessor
  {
    private const string className = "LoanLockAccessor�";
    private const string VIRTUAL_COL_VALUES = "NGSHAREDLOCK!!�";

    public static bool IsLoanLockDbEnabled
    {
      get
      {
        bool result = false;
        bool.TryParse(Company.GetCompanySetting("FEATURE", "ENABLELOANLOCKFLOW"), out result);
        return result && !ClientContext.GetCurrent().AllowConcurrentEditing;
      }
    }

    public static bool IsLoanLockedBySessionId(string loanGuid, string sessionId)
    {
      LockInfo lockInfo = LoanLockAccessor.GetLockInfo(loanGuid, sessionId, false);
      return lockInfo != null && lockInfo.IsLocked && !string.IsNullOrEmpty(lockInfo.LoginSessionID);
    }

    public static List<LockInfo> GetAllLockInfos(string loanGuid)
    {
      List<LockInfo> allLockInfos = (List<LockInfo>) null;
      DataRowCollection dataRowCollection = LoanLockAccessor.buildReadDBLockQuery(loanGuid, "").Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        allLockInfos = new List<LockInfo>();
        foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
          allLockInfos.Add(LoanLockAccessor.dataRowToLockInfo(r));
      }
      return allLockInfos;
    }

    public static LockInfo GetLockInfo(string loanGuid, string sessionId, bool returnAny)
    {
      DataRowCollection dataRowCollection = LoanLockAccessor.buildReadDBLockQuery(loanGuid, sessionId).Execute();
      LockInfo lockInfo = (LockInfo) null;
      if (dataRowCollection.Count == 0)
      {
        lockInfo = new LockInfo(loanGuid);
      }
      else
      {
        bool flag = false;
        if (sessionId != null)
        {
          foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
          {
            if ((string) r["loginSessionID"] == sessionId)
            {
              lockInfo = LoanLockAccessor.dataRowToLockInfo(r);
              flag = true;
              break;
            }
          }
          if (!flag)
            lockInfo = !returnAny ? new LockInfo(loanGuid) : LoanLockAccessor.dataRowToLockInfo(dataRowCollection[0]);
        }
        else
          lockInfo = LoanLockAccessor.dataRowToLockInfo(dataRowCollection[0]);
      }
      return lockInfo;
    }

    public static void updateLock(LockInfo lockInfo)
    {
      if (lockInfo == null || string.IsNullOrWhiteSpace(lockInfo.GUID))
        return;
      if (lockInfo.Exclusive == LockInfo.ExclusiveLock.Exclusive || lockInfo.Exclusive == LockInfo.ExclusiveLock.Nonexclusive && lockInfo.LockedFor == LoanInfo.LockReason.OpenForWork || lockInfo.Exclusive == LockInfo.ExclusiveLock.Nonexclusive && lockInfo.LockedFor == LoanInfo.LockReason.Downloaded)
      {
        LoanLockAccessor.UpdateExclusiveLock(lockInfo.GUID, lockInfo.LoginSessionID, lockInfo.LockedBy, lockInfo.LockedFor);
      }
      else
      {
        if (lockInfo.Exclusive != LockInfo.ExclusiveLock.NGSharedLock)
          return;
        LoanLockAccessor.UpdateNGSharedLock(lockInfo.GUID, lockInfo.LoginSessionID, lockInfo.LockedBy, lockInfo.IsSessionLess);
      }
    }

    public static void removeLock(string loanGuid, string loginSessionId, string userId)
    {
      if (string.IsNullOrWhiteSpace(loanGuid))
        return;
      try
      {
        SqlCommand sqlCmd = new SqlCommand();
        sqlCmd.CommandText = "[DeleteLoanLock]";
        sqlCmd.CommandType = CommandType.StoredProcedure;
        DbValueList dbValueList = new DbValueList();
        SqlParameterCollection parameters1 = sqlCmd.Parameters;
        SqlParameter sqlParameter1 = new SqlParameter();
        sqlParameter1.ParameterName = "@loanGuid";
        sqlParameter1.Value = (object) loanGuid;
        parameters1.Add(sqlParameter1);
        SqlParameterCollection parameters2 = sqlCmd.Parameters;
        SqlParameter sqlParameter2 = new SqlParameter();
        sqlParameter2.ParameterName = "@sessionId";
        sqlParameter2.Value = (object) loginSessionId;
        parameters2.Add(sqlParameter2);
        SqlParameter sqlParameter3 = new SqlParameter("@isExclusiveLock", SqlDbType.Bit);
        sqlParameter3.Value = (object) 0;
        sqlParameter3.Direction = ParameterDirection.Output;
        sqlCmd.Parameters.Add(sqlParameter3);
        SqlParameter sqlParameter4 = new SqlParameter("@isSessionLess", SqlDbType.Bit);
        sqlParameter4.Value = (object) 0;
        sqlParameter4.Direction = ParameterDirection.Output;
        sqlCmd.Parameters.Add(sqlParameter4);
        SqlParameter sqlParameter5 = new SqlParameter("@hasRemainingLocks", SqlDbType.Bit);
        sqlParameter5.Value = (object) 0;
        sqlParameter5.Direction = ParameterDirection.Output;
        sqlCmd.Parameters.Add(sqlParameter5);
        new EllieMae.EMLite.Server.DbAccessManager().ExecuteSetQuery((IDbCommand) sqlCmd, DbTransactionType.Default);
        if (Convert.ToBoolean(sqlParameter3.Value))
          LoanLockAccessor.SendKafkaEventForWebhook(loanGuid, userId, LoanLockAccessor.LockType.UnLock);
        if (Convert.IsDBNull(sqlParameter4.Value) || Convert.ToBoolean(sqlParameter4.Value) || Convert.IsDBNull(sqlParameter5.Value) || Convert.ToBoolean(sqlParameter5.Value))
          return;
        LoanLockAccessor.SendKafkaEventForWebhook(loanGuid, userId, LoanLockAccessor.LockType.UnlockAll);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (LoanLockAccessor), string.Format("Exception while releasing lock for the loan - {0}, sessionId-{1}, userId-{2}, exception-{3}", (object) loanGuid, (object) loginSessionId, (object) userId, (object) ex.StackTrace));
        Err.Reraise(nameof (LoanLockAccessor), ex);
      }
    }

    public static void removeLock(LockInfo lockInfo)
    {
      if (lockInfo == null || string.IsNullOrWhiteSpace(lockInfo.GUID))
        return;
      LoanLockAccessor.removeLock(lockInfo.GUID, lockInfo.LoginSessionID, lockInfo.LockedBy);
    }

    private static void SendKafkaEventForWebhook(
      string loanId,
      string lockedBy,
      LoanLockAccessor.LockType lockType)
    {
      try
      {
        ClientContext current = ClientContext.GetCurrent();
        WebHooksEvent queueEvent = lockType != LoanLockAccessor.LockType.UnlockAll ? new WebHooksEvent("serviceId", current.InstanceName, (string) null, loanId, lockedBy, EncompassServer.ServerMode != EncompassServerMode.Service ? Enums.Source.URN_ELLI_SERVICE_ENCOMPASS : Enums.Source.URN_ELLI_SERVICE_EBS, DateTime.UtcNow) : (WebHooksEvent) new UnlockAllEvent("serviceId", current.InstanceName, (string) null, loanId, lockedBy, EncompassServer.ServerMode != EncompassServerMode.Service ? Enums.Source.URN_ELLI_SERVICE_ENCOMPASS : Enums.Source.URN_ELLI_SERVICE_EBS, DateTime.UtcNow);
        queueEvent.AddLockUnlockKafkaMessage(loanId, ClientContext.CurrentRequest.CorrelationId, lockedBy, current.ClientID, current.InstanceName, EncompassServer.ServerMode != EncompassServerMode.Service, lockType.ToString());
        if (queueEvent.QueueMessages.Count <= 0)
          return;
        IMessageQueueEventService queueEventService = (IMessageQueueEventService) new MessageQueueEventService();
        IMessageQueueProcessor processor = (IMessageQueueProcessor) new KafkaProcessor();
        queueEventService.MessageQueueProducer((MessageQueueEvent) queueEvent, processor);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (LoanLockAccessor), string.Format("Failed to publish lock/unlock/unlockall event. Loan GUID={0}  lockedBy={1}  lockType={2}  Stack={3}", (object) loanId, (object) lockedBy, (object) lockType.ToString(), (object) ex));
      }
    }

    private static void UpdateNGSharedLock(
      string loanId,
      string sessionId,
      string userId,
      bool isSessionLess)
    {
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("[ValidateAndAcquireSharedLock]");
        dbQueryBuilder.ExecuteStoredProc(DbTransactionType.None, new DbValueList()
        {
          {
            "@loanGuid",
            (object) loanId
          },
          {
            "@sessionId",
            (object) sessionId
          },
          {
            "@userId",
            (object) userId
          },
          {
            "@exclusiveLockType",
            (object) 1
          },
          {
            "@shareLockType",
            (object) 4
          },
          {
            "@isSessionless",
            (object) isSessionLess
          }
        });
      }
      catch (ServerDataException ex)
      {
        List<LockInfo> allLockInfos = LoanLockAccessor.GetAllLockInfos(loanId);
        if (ex.Message.IndexOf("is currently locked") > 0)
        {
          Err.Raise(TraceLevel.Info, nameof (LoanLockAccessor), (ServerException) new LockException(allLockInfos != null ? allLockInfos.FirstOrDefault<LockInfo>((System.Func<LockInfo, bool>) (item => item.IsLocked)) : (LockInfo) null));
        }
        else
        {
          if (ex.InnerException != null && ex.InnerException is SqlException && ex.InnerException.Message.Contains("IX_LoanLock_guid_loginSessionID") && allLockInfos != null && allLockInfos.Any<LockInfo>((System.Func<LockInfo, bool>) (item => item.LoginSessionID == sessionId && item.IsLocked)))
            return;
          TraceLog.WriteError(nameof (LoanLockAccessor), string.Format("SQL Exception while acquiring NGShared lock for the loan - {0}, sessionId-{1}, userId-{2}, exception-{3}", (object) loanId, (object) sessionId, (object) userId, (object) ex.Message));
          Err.Reraise(nameof (LoanLockAccessor), (Exception) ex);
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (LoanLockAccessor), string.Format("Exception while acquiring NGShared lock for the loan - {0}, sessionId-{1}, userId-{2}, exception-{3}", (object) loanId, (object) sessionId, (object) userId, (object) ex.StackTrace));
        Err.Reraise(nameof (LoanLockAccessor), ex);
      }
    }

    private static void UpdateExclusiveLock(
      string loanId,
      string sessionId,
      string userId,
      LoanInfo.LockReason lockReason = LoanInfo.LockReason.OpenForWork)
    {
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("[ValidateAndAcquireExclusiveLock]");
        dbQueryBuilder.ExecuteStoredProc(DbTransactionType.None, new DbValueList()
        {
          {
            "@loanGuid",
            (object) loanId
          },
          {
            "@sessionId",
            (object) sessionId
          },
          {
            "@userId",
            (object) userId
          },
          {
            "@lockedforReason",
            (object) (int) lockReason
          },
          {
            "@exclusiveLockType",
            (object) 1
          },
          {
            "@shareLockType",
            (object) 4
          }
        });
        LoanLockAccessor.SendKafkaEventForWebhook(loanId, userId, LoanLockAccessor.LockType.Lock);
      }
      catch (ServerDataException ex)
      {
        LockInfo lockInfo = LoanLockAccessor.GetLockInfo(loanId, (string) null, true);
        if (ex.Message.IndexOf("is currently locked") > 0)
        {
          Err.Raise(TraceLevel.Info, nameof (LoanLockAccessor), (ServerException) new LockException(lockInfo));
        }
        else
        {
          if (ex.InnerException != null && ex.InnerException is SqlException && ex.InnerException.Message.Contains("IX_LoanLock_guid_loginSessionID") && lockInfo != null && lockInfo.LoginSessionID == sessionId && lockInfo.IsLocked)
            return;
          TraceLog.WriteWarning(nameof (LoanLockAccessor), string.Format("SQL Exception while acquiring exclusive lock for the loan - {0}, sessionId-{1}, userId-{2}, exception-{3}", (object) loanId, (object) sessionId, (object) userId, (object) ex.Message));
          Err.Reraise(nameof (LoanLockAccessor), (Exception) ex);
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (LoanLockAccessor), string.Format("Exception while acquiring exclusive lock for the loan - {0}, sessionId-{1}, userId-{2}, exception-{3}", (object) loanId, (object) sessionId, (object) userId, (object) ex.StackTrace));
        Err.Reraise(nameof (LoanLockAccessor), ex);
      }
    }

    private static LockInfo dataRowToLockInfo(DataRow r, bool includeServerUri = true)
    {
      string lockedByFirstName = r["first_name"] == DBNull.Value ? "" : r["first_name"].ToString();
      string lockedByLastName = r["last_name"] == DBNull.Value ? "" : r["last_name"].ToString();
      return includeServerUri ? new LockInfo(r["guid"].ToString(), r["lockedby"].ToString(), lockedByFirstName, lockedByLastName, EllieMae.EMLite.DataAccess.SQL.DecodeString(r["loginSessionID"], (string) null), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ServerUri"], (string) null), (LoanInfo.LockReason) r["lockedfor"], (DateTime) r["locktime"], r["SessionID"] is DBNull ? LockInfo.LockOwnerLoggedOn.False : LockInfo.LockOwnerLoggedOn.True, (LockInfo.ExclusiveLock) (byte) r["exclusive"], (bool) r["IsSessionLess"]) : new LockInfo(r["guid"].ToString(), r["lockedby"].ToString(), lockedByFirstName, lockedByLastName, EllieMae.EMLite.DataAccess.SQL.DecodeString(r["loginSessionID"], (string) null), (string) null, (LoanInfo.LockReason) r["lockedfor"], (DateTime) r["locktime"], (LockInfo.ExclusiveLock) (byte) r["exclusive"], (bool) r["IsSessionLess"]);
    }

    private static EllieMae.EMLite.Server.DbQueryBuilder buildReadDBLockQuery(
      string loanGuid,
      string sessionId = "�")
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select LoanLock.*, Sessions.SessionID, Sessions.ServerUri, users.first_name, users.last_name");
      dbQueryBuilder.AppendLine("from LoanLock join Sessions on LoanLock.loginSessionID = Sessions.SessionID");
      dbQueryBuilder.AppendLine("left outer join users on LoanLock.lockedby = users.userid");
      dbQueryBuilder.AppendLine("where lockedFor <> 0 and LoanLock.guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanGuid));
      if (!string.IsNullOrEmpty(sessionId))
        dbQueryBuilder.AppendLine("and LoanLock.loginSessionID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) sessionId));
      dbQueryBuilder.AppendLine(" Union ");
      dbQueryBuilder.AppendLine("select LoanLock.*, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) "NGSHAREDLOCK!!") + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) "NGSHAREDLOCK!!") + ",  users.first_name, users.last_name from LoanLock");
      dbQueryBuilder.AppendLine("left outer join users on LoanLock.lockedby = users.userid");
      dbQueryBuilder.AppendLine("where LoanLock.IsSessionLess = '1' AND LoanLock.guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanGuid));
      if (!string.IsNullOrEmpty(sessionId))
        dbQueryBuilder.AppendLine("and LoanLock.loginSessionID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) sessionId));
      return dbQueryBuilder;
    }

    private enum LockType
    {
      Lock,
      UnLock,
      UnlockAll,
    }
  }
}
