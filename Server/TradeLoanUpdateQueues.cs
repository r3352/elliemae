// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.TradeLoanUpdateQueues
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class TradeLoanUpdateQueues
  {
    public static TradeLoanUpdateJobInfo[] GetAllTradeLoanUpdateQueues()
    {
      return TradeLoanUpdateQueues.getQueuesFromDatabase("");
    }

    private static TradeLoanUpdateJobInfo[] getQueuesFromDatabase(string criteria)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from TradeLoanUpdateQueue");
      if (!string.IsNullOrEmpty(criteria))
        dbQueryBuilder.AppendLine("where JobGuid in ('" + criteria + "')");
      DataTable table = dbQueryBuilder.ExecuteSetQuery().Tables[0];
      TradeLoanUpdateJobInfo[] queuesFromDatabase = new TradeLoanUpdateJobInfo[table.Rows.Count];
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        TradeLoanUpdateJobInfo queue = TradeLoanUpdateQueues.dataRowToQueue(table.Rows[index], table);
        queuesFromDatabase[index] = queue;
      }
      return queuesFromDatabase;
    }

    public static TradeLoanUpdateJobInfo GetQueue(string jobGuid)
    {
      TradeLoanUpdateJobInfo[] queuesFromDatabase = TradeLoanUpdateQueues.getQueuesFromDatabase(jobGuid);
      return queuesFromDatabase.Length == 0 ? (TradeLoanUpdateJobInfo) null : queuesFromDatabase[0];
    }

    private static TradeLoanUpdateJobInfo dataRowToQueue(DataRow r, DataTable queueTable)
    {
      return new TradeLoanUpdateJobInfo()
      {
        JobGuid = SQL.DecodeString(r["JobGuid"]),
        TradeType = (TradeType) SQL.DecodeInt(r["TradeType"]),
        TradeID = SQL.DecodeInt(r["TradeID"]),
        TradeName = SQL.DecodeString(r["TradeName"]),
        CreatedBy = SQL.DecodeString(r["CreatedBy"]),
        CreatedDate = SQL.DecodeDateTime(r["CreatedDate"]),
        JobStartDate = SQL.DecodeDateTime(r["JobStartDate"]),
        JobStatus = (TradeLoanUpdateQueueStatus) SQL.DecodeInt(r["JobStatus"]),
        SessionID = SQL.DecodeString(r["SessionID"]),
        TotalLoans = SQL.DecodeInt(r["TotalLoans"]),
        LoansCompleted = SQL.DecodeInt(r["LoansCompleted"]),
        LastUpdateDate = SQL.DecodeDateTime(r["LastUpdatedDate"]),
        JobEndDate = SQL.DecodeDateTime(r["JobEndDate"]),
        Results = SQL.DecodeString(r["Results"]),
        CancelledBy = SQL.DecodeString(r["CancelledBy"]),
        CancelledDate = SQL.DecodeDateTime(r["CancelledDate"]),
        DeletedBy = SQL.DecodeString(r["DeletedBy"]),
        DeletedDate = SQL.DecodeDateTime(r["DeletedDate"]),
        SessionLastUpdateDate = SQL.DecodeDateTime(r["SessionLastUpdateDate"])
      };
    }

    private static DbValueList createQueuesValueList(TradeLoanUpdateJobInfo info)
    {
      return new DbValueList()
      {
        {
          "JobGuid",
          (object) info.JobGuid
        },
        {
          "TradeType",
          (object) info.TradeType
        },
        {
          "TradeID",
          (object) info.TradeID
        },
        {
          "TradeName",
          (object) info.TradeName
        },
        {
          "CreatedBy",
          (object) info.CreatedBy
        },
        {
          "CreatedDate",
          (object) info.CreatedDate
        },
        {
          "JobStartDate",
          (object) info.JobStartDate
        },
        {
          "JobStatus",
          (object) (int) info.JobStatus
        },
        {
          "SessionID",
          (object) info.SessionID
        },
        {
          "TotalLoans",
          (object) info.TotalLoans
        },
        {
          "LoansCompleted",
          (object) info.LoansCompleted
        },
        {
          "LastUpdatedDate",
          (object) info.LastUpdateDate
        },
        {
          "JobEndDate",
          (object) info.JobEndDate
        },
        {
          "Results",
          (object) info.Results
        },
        {
          "CancelledBy",
          (object) info.CancelledBy
        },
        {
          "CancelledDate",
          (object) info.CancelledDate
        },
        {
          "DeletedBy",
          (object) info.DeletedBy
        },
        {
          "DeletedDate",
          (object) info.DeletedDate
        },
        {
          "SessionLastUpdateDate",
          (object) info.SessionLastUpdateDate
        }
      };
    }

    public static void CreateQueue(TradeLoanUpdateJobInfo info)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("TradeLoanUpdateQueue");
      dbQueryBuilder.InsertInto(table, TradeLoanUpdateQueues.createQueuesValueList(info), true, false);
      dbQueryBuilder.ExecuteScalar();
    }

    public static void DeleteQueue(string jobGuid)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("TradeLoanUpdateQueue");
      DbValue key = new DbValue("JobGuid", (object) jobGuid);
      dbQueryBuilder.AppendLine("");
      dbQueryBuilder.Append(TradeLoanUpdateQueues.GenerateSqlToTrackSessionActivity(jobGuid));
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static int DeleteQueues(DateTime deleteBefore)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from TradeLoanUpdateQueue where createdDate < '" + (object) deleteBefore + "' and JobStatus in (3, 4, 5);");
      dbQueryBuilder.AppendLine("  select @@ROWCOUNT");
      return Convert.ToInt32(dbQueryBuilder.ExecuteScalar());
    }

    public static void UpdateQueue(TradeLoanUpdateJobInfo info)
    {
      TradeLoanUpdateQueues.GetQueue(info.JobGuid);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("TradeLoanUpdateQueue");
      DbValue key = new DbValue("JobGuid", (object) info.JobGuid);
      dbQueryBuilder.Update(table, TradeLoanUpdateQueues.createQueuesValueList(info), key);
      dbQueryBuilder.AppendLine("");
      dbQueryBuilder.Append(TradeLoanUpdateQueues.GenerateSqlToTrackSessionActivity(info.JobGuid));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void UpdateQueue(Dictionary<string, object> updateValue, string jobGuid)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("TradeLoanUpdateQueue");
      DbValue key = new DbValue("JobGuid", (object) jobGuid);
      dbQueryBuilder.Update(table, TradeLoanUpdateQueues.updateQueuesValueList(updateValue), key);
      dbQueryBuilder.AppendLine("");
      dbQueryBuilder.Append(TradeLoanUpdateQueues.GenerateSqlToTrackSessionActivity(jobGuid));
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static DbValueList updateQueuesValueList(Dictionary<string, object> updateValue)
    {
      DbValueList dbValueList = new DbValueList();
      foreach (KeyValuePair<string, object> keyValuePair in updateValue)
        dbValueList.Add(keyValuePair.Key, keyValuePair.Value);
      return dbValueList;
    }

    private static string GenerateSqlToTrackSessionActivity(string keyValue)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("DECLARE @sessionId varchar(50)");
      stringBuilder.AppendLine("DECLARE @lastModifiedDate datetime");
      stringBuilder.AppendLine("SELECT @sessionId = SessionID, @lastModifiedDate = LastUpdatedDate FROM TradeLoanUpdateQueue WHERE JobGuid = '" + keyValue + "'");
      stringBuilder.AppendLine("UPDATE TradeLoanUpdateQueue SET SessionLastUpdateDate = @lastModifiedDate WHERE SessionID = @sessionId");
      return stringBuilder.ToString();
    }
  }
}
