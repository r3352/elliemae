// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServiceObjects.Repositories.SessionCacheRepository
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Domain.Sessions;
using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.ServiceInterface.Repositories;
using System;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServiceObjects.Repositories
{
  public class SessionCacheRepository(IDataCache dataCache) : RepositoryBase(dataCache), ISessionCacheRepository
  {
    private const string ClassName = "SessionCacheRepository�";

    [PgReady]
    public void CreateSessionCache(SessionCache sessionCache)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        DbValueList values = new DbValueList();
        values.Add("SessionID", (object) sessionCache.SessionId);
        values.Add("Name", (object) sessionCache.Name);
        values.Add("Value", (object) sessionCache.Value);
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("SessionCaches");
        pgDbQueryBuilder.InsertInto(table, values, true, false);
        pgDbQueryBuilder.ExecuteNonQuery();
      }
      else
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        DbValueList values = new DbValueList();
        values.Add("SessionID", (object) sessionCache.SessionId);
        values.Add("Name", (object) sessionCache.Name);
        values.Add("Value", (object) sessionCache.Value);
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("SessionCaches");
        dbQueryBuilder.InsertInto(table, values, true, false);
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    [PgReady]
    public SessionCache GetSessionCache(string sessionId, string name)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("SessionCaches");
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select ");
        pgDbQueryBuilder.AppendLineFormat("\t{0},", (object) "SessionID");
        pgDbQueryBuilder.AppendLineFormat("\t{0},", (object) "Name");
        pgDbQueryBuilder.AppendLineFormat("\t{0}", (object) "Value");
        pgDbQueryBuilder.AppendLineFormat("from {0}", (object) "SessionCaches");
        pgDbQueryBuilder.AppendLineFormat("where {0} = '{1}'", (object) "SessionID", (object) table["SessionID"].SizeToFit(sessionId));
        pgDbQueryBuilder.AppendLineFormat("and {0} = '{1}'", (object) "Name", (object) table["Name"].SizeToFit(name));
        DataRow dataRow = pgDbQueryBuilder.ExecuteRowQuery();
        if (dataRow == null)
          return (SessionCache) null;
        string str = Convert.ToString(dataRow["Value"]);
        return new SessionCache(sessionId, name)
        {
          Value = str
        };
      }
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("SessionCaches");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select ");
      dbQueryBuilder.AppendLineFormat("\t{0},", (object) "SessionID");
      dbQueryBuilder.AppendLineFormat("\t{0},", (object) "Name");
      dbQueryBuilder.AppendLineFormat("\t{0}", (object) "Value");
      dbQueryBuilder.AppendLineFormat("from {0}", (object) "SessionCaches");
      dbQueryBuilder.AppendLineFormat("where {0} = '{1}'", (object) "SessionID", (object) table1["SessionID"].SizeToFit(sessionId));
      dbQueryBuilder.AppendLineFormat("and {0} = '{1}'", (object) "Name", (object) table1["Name"].SizeToFit(name));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
      if (dataRowCollection == null || dataRowCollection.Count <= 0)
        return (SessionCache) null;
      string str1 = Convert.ToString(dataRowCollection[0]["Value"]);
      return new SessionCache(sessionId, name)
      {
        Value = str1
      };
    }

    [PgReady]
    public void UpdateSessionCache(SessionCache sessionCache)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("SessionCaches");
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLineFormat("update {0}", (object) "SessionCaches");
        pgDbQueryBuilder.AppendLineFormat("set {0} = {1}", (object) "Value", (object) SQL.Encode((object) sessionCache.Value));
        pgDbQueryBuilder.AppendLineFormat("where {0} = '{1}'", (object) "SessionID", (object) table["SessionID"].SizeToFit(sessionCache.SessionId));
        pgDbQueryBuilder.AppendLineFormat("and {0} = '{1}'", (object) "Name", (object) table["Name"].SizeToFit(sessionCache.Name));
        pgDbQueryBuilder.ExecuteNonQuery();
      }
      else
      {
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("SessionCaches");
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLineFormat("update {0}", (object) "SessionCaches");
        dbQueryBuilder.AppendLineFormat("set {0} = {1}", (object) "Value", (object) SQL.Encode((object) sessionCache.Value));
        dbQueryBuilder.AppendLineFormat("where {0} = '{1}'", (object) "SessionID", (object) table["SessionID"].SizeToFit(sessionCache.SessionId));
        dbQueryBuilder.AppendLineFormat("and {0} = '{1}'", (object) "Name", (object) table["Name"].SizeToFit(sessionCache.Name));
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    [PgReady]
    public void CleanSessionCache(string sessionId)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("SessionCaches");
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLineFormat("delete {0}", (object) "SessionCaches");
        pgDbQueryBuilder.AppendLineFormat("where {0} = '{1}'", (object) "SessionID", (object) table["SessionID"].SizeToFit(sessionId));
        pgDbQueryBuilder.ExecuteNonQuery();
      }
      else
      {
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("SessionCaches");
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLineFormat("delete {0}", (object) "SessionCaches");
        dbQueryBuilder.AppendLineFormat("where {0} = '{1}'", (object) "SessionID", (object) table["SessionID"].SizeToFit(sessionId));
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    [PgReady]
    public void ClearnExpiredSessionCache(int sessionTimeoutMinutes, SessionType type)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLineFormat("delete from {0}", (object) "SessionCaches");
        pgDbQueryBuilder.AppendLineFormat("where {0} in ( Select {0} from Sessions where DATEDIFF('MINUTE',LastAccessed,GetDate()) > {1} AND SessionType = {2} )", (object) "SessionID", (object) sessionTimeoutMinutes, (object) (int) type);
        pgDbQueryBuilder.ExecuteNonQuery();
      }
      else
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLineFormat("delete from {0}", (object) "SessionCaches");
        dbQueryBuilder.AppendLineFormat("where {0} in ( Select {0} from Sessions where DATEDIFF(MINUTE,LastAccessed,GetDate()) > {1} AND SessionType = {2} )", (object) "SessionID", (object) sessionTimeoutMinutes, (object) (int) type);
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    private static class Table
    {
      public const string TableName = "SessionCaches�";

      public static class ColumnNames
      {
        public const string SessionId = "SessionID�";
        public const string Name = "Name�";
        public const string Value = "Value�";
      }
    }
  }
}
