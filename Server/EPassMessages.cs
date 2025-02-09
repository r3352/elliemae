// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.EPassMessages
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class EPassMessages
  {
    public static void UpsertEPassMessage(EPassMessageInfo ePassMessage)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbAccessManager.GetTable(nameof (EPassMessages));
      dbQueryBuilder.AppendLine("select * from EPassMessages where LoanGuid = " + SQL.Encode((object) ePassMessage.LoanGuid));
      dbQueryBuilder.AppendLine(" and MessageID = " + SQL.Encode((object) ePassMessage.MessageID));
      if (dbQueryBuilder.Execute().Count > 0)
      {
        EPassMessages.DeleteMessage(ePassMessage.MessageID);
        EPassMessages.CreateMessage(ePassMessage);
      }
      else
        EPassMessages.CreateMessage(ePassMessage);
    }

    public static EPassMessageInfo GetEPassMessage(string messageId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable(nameof (EPassMessages));
      DbValue key = new DbValue("MessageID", (object) messageId);
      dbQueryBuilder.SelectFrom(table, key);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection.Count == 0 ? (EPassMessageInfo) null : EPassMessages.dataRowToEPassMessageInfo(dataRowCollection[0]);
    }

    public static EPassMessageInfo[] GetEPassMessages(string[] messageTypes)
    {
      DbQueryBuilder sql = new DbQueryBuilder();
      sql.AppendLine("select * from EPassMessages where MsgType in (" + SQL.EncodeArray((Array) messageTypes) + ")");
      sql.AppendLine("order by MessageTime");
      return EPassMessages.selectMessages((BaseDbQueryBuilder) sql);
    }

    public static int GetEPassMessageCount(string[] messageTypes)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select count(*) from EPassMessages where MsgType in (" + SQL.EncodeArray((Array) messageTypes) + ")");
      return (int) dbQueryBuilder.ExecuteScalar();
    }

    public static EPassMessageInfo[] GetEPassMessagesForLoan(string loanGuid)
    {
      DbQueryBuilder sql = new DbQueryBuilder();
      sql.AppendLine("select * from EPassMessages where LoanGuid = " + SQL.Encode((object) loanGuid));
      sql.AppendLine("order by MessageTime");
      return EPassMessages.selectMessages((BaseDbQueryBuilder) sql);
    }

    [PgReady]
    public static EPassMessageInfo[] GetEPassMessagesForLoan(string loanGuid, string userId)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder sql = new PgDbQueryBuilder();
        sql.AppendLine("select * from EPassMessages where LoanGuid = " + SQL.Encode((object) loanGuid));
        sql.AppendLine("  and ((UserID is NULL) or (UserID = " + SQL.Encode((object) userId) + "))");
        sql.AppendLine("order by MessageTime");
        return EPassMessages.selectMessages((BaseDbQueryBuilder) sql);
      }
      DbQueryBuilder sql1 = new DbQueryBuilder();
      sql1.AppendLine("select * from EPassMessages where LoanGuid = " + SQL.Encode((object) loanGuid));
      sql1.AppendLine("  and ((UserID is NULL) or (UserID = " + SQL.Encode((object) userId) + "))");
      sql1.AppendLine("order by MessageTime");
      return EPassMessages.selectMessages((BaseDbQueryBuilder) sql1);
    }

    public static EPassMessageInfo[] GetEPassMessagesForUser(string userId)
    {
      DbQueryBuilder sql = new DbQueryBuilder();
      sql.AppendLine("select * from EPassMessages where UserID = " + SQL.Encode((object) userId));
      sql.AppendLine("order by MessageTime");
      return EPassMessages.selectMessages((BaseDbQueryBuilder) sql);
    }

    public static EPassMessageInfo[] GetLoanMailboxMessagesForUser(string userId)
    {
      DbQueryBuilder sql = new DbQueryBuilder();
      sql.AppendLine("select * from EPassMessages where UserID = " + SQL.Encode((object) userId));
      sql.AppendLine("   and LoanGuid is NULL");
      sql.AppendLine("   and MsgType <> " + SQL.Encode((object) "LC_LEADS"));
      sql.AppendLine("order by MessageTime");
      return EPassMessages.selectMessages((BaseDbQueryBuilder) sql);
    }

    [PgReady]
    public static int GetLoanMailboxMessageCountForUser(string userId)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select count(*) from EPassMessages where UserID = " + SQL.Encode((object) userId));
        pgDbQueryBuilder.AppendLine("   and LoanGuid is NULL");
        pgDbQueryBuilder.AppendLine("   and MsgType <> " + SQL.Encode((object) "LC_LEADS"));
        return SQL.DecodeInt(pgDbQueryBuilder.ExecuteScalar());
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select count(*) from EPassMessages where UserID = " + SQL.Encode((object) userId));
      dbQueryBuilder.AppendLine("   and LoanGuid is NULL");
      dbQueryBuilder.AppendLine("   and MsgType <> " + SQL.Encode((object) "LC_LEADS"));
      return (int) dbQueryBuilder.ExecuteScalar();
    }

    public static EPassMessageInfo[] GetEPassMessagesForUser(string userId, string[] messageTypes)
    {
      DbQueryBuilder sql = new DbQueryBuilder();
      sql.AppendLine("select * from EPassMessages where UserID = " + SQL.Encode((object) userId));
      sql.AppendLine("  and MsgType in (" + SQL.EncodeArray((Array) messageTypes) + ")");
      sql.AppendLine("order by MessageTime");
      return EPassMessages.selectMessages((BaseDbQueryBuilder) sql);
    }

    public static int GetEPassMessageCountForUser(string userId, string[] messageTypes)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select count(*) from EPassMessages where UserID = " + SQL.Encode((object) userId));
      dbQueryBuilder.AppendLine("  and MsgType in (" + SQL.EncodeArray((Array) messageTypes) + ")");
      return (int) dbQueryBuilder.ExecuteScalar();
    }

    public static string[] GetEPassMessageIDs()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable(nameof (EPassMessages));
      dbQueryBuilder.SelectFrom(table, new string[1]
      {
        "MessageID"
      });
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      List<string> stringList = new List<string>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        stringList.Add(SQL.DecodeString(dataRow["MessageID"]));
      return stringList.ToArray();
    }

    public static bool CreateMessage(EPassMessageInfo msg)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable(nameof (EPassMessages));
      DbValue keyValue = new DbValue("MessageID", (object) msg.MessageID);
      DbValueList dbValueList = EPassMessages.createDbValueList(msg);
      dbQueryBuilder.IfExists(table, keyValue);
      dbQueryBuilder.AppendLine("select 0 as RowCreated");
      dbQueryBuilder.Else();
      dbQueryBuilder.Begin();
      dbQueryBuilder.InsertInto(table, dbValueList, true, false);
      dbQueryBuilder.AppendLine("select 1 as RowCreated");
      dbQueryBuilder.End();
      return SQL.DecodeInt(dbQueryBuilder.ExecuteScalar()) == 1;
    }

    [PgReady]
    public static void DeleteMessage(string messageId)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        DbTableInfo table = DbAccessManager.GetTable(nameof (EPassMessages));
        DbValue key = new DbValue("MessageID", (object) messageId);
        pgDbQueryBuilder.DeleteFrom(table, key);
        pgDbQueryBuilder.ExecuteNonQuery();
      }
      else
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        DbTableInfo table = DbAccessManager.GetTable(nameof (EPassMessages));
        DbValue key = new DbValue("MessageID", (object) messageId);
        dbQueryBuilder.DeleteFrom(table, key);
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    public static void DeleteMessages(string[] messageIds)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable(nameof (EPassMessages));
      DbValue key = new DbValue("MessageID", (object) messageIds);
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.ExecuteNonQuery();
    }

    [PgReady]
    public static void DeleteMessagesBefore(DateTime firstMsgDate)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("delete from EPassMessages where MessageTime < " + SQL.Encode((object) firstMsgDate));
        pgDbQueryBuilder.ExecuteNonQuery();
      }
      else
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("delete from EPassMessages where MessageTime < " + SQL.Encode((object) firstMsgDate));
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    public static void ResetMessages()
    {
      Company.DeleteCompanySettings("EPASSMSGS");
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from EPassMessages");
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static EPassMessageInfo[] selectMessages(BaseDbQueryBuilder sql)
    {
      List<EPassMessageInfo> epassMessageInfoList = new List<EPassMessageInfo>();
      foreach (DataRow r in (InternalDataCollectionBase) sql.Execute())
        epassMessageInfoList.Add(EPassMessages.dataRowToEPassMessageInfo(r));
      return epassMessageInfoList.ToArray();
    }

    private static EPassMessageInfo dataRowToEPassMessageInfo(DataRow r)
    {
      return new EPassMessageInfo(SQL.DecodeString(r["MessageID"]), SQL.DecodeString(r["MsgType"]), SQL.DecodeString(r["Source"]), SQL.DecodeString(r["LoanGuid"]), SQL.DecodeString(r["UserID"]), SQL.DecodeString(r["Description"]), SQL.DecodeDateTime(r["MessageTime"]), SQL.DecodeBoolean(r["Active"]), SQL.DecodeString(r["MessageXml"], (string) null));
    }

    private static DbValueList createDbValueList(EPassMessageInfo info)
    {
      return new DbValueList()
      {
        {
          "MessageID",
          (object) info.MessageID
        },
        {
          "MsgType",
          (object) info.MessageType
        },
        {
          "Source",
          (object) info.Source,
          (IDbEncoder) DbEncoding.EmptyStringAsNull
        },
        {
          "LoanGuid",
          (object) info.LoanGuid,
          (IDbEncoder) DbEncoding.EmptyStringAsNull
        },
        {
          "UserID",
          (object) info.UserID,
          (IDbEncoder) DbEncoding.EmptyStringAsNull
        },
        {
          "Description",
          (object) info.Description
        },
        {
          "MessageTime",
          (object) info.Timestamp
        },
        {
          "Active",
          (object) info.Active,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "MessageXml",
          (object) info.MessageXml
        }
      };
    }
  }
}
