// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.CCSiteInfoAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataAccess.Postgres;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class CCSiteInfoAccessor
  {
    [PgReady]
    public static void createCCSiteInfo(int orgId, string siteId, string url)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        string str = "org_ccsite";
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        string text = "INSERT INTO " + str + " (oid, siteId, url) VALUES (" + (object) orgId + ", '" + siteId + "', @url)";
        DbCommandParameter parameter = new DbCommandParameter(nameof (url), (object) url.TrimEnd(), DbType.String);
        pgDbQueryBuilder.Append(text);
        pgDbQueryBuilder.ExecuteNonQuery(parameter);
      }
      else
      {
        string str = "org_ccsite";
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        string text = "INSERT INTO " + str + " (oid, siteId, url) VALUES (" + (object) orgId + ", '" + siteId + "', '" + url + "')";
        dbQueryBuilder.Append(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    [PgReady]
    public static void createUserCCSiteInfo(string userId, string siteId, string url)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        string str = "users_ccsite";
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        string text = "INSERT INTO " + str + " (userid, siteId, url) VALUES ('" + userId + "', '" + siteId + "', @url)";
        DbCommandParameter parameter = new DbCommandParameter(nameof (url), (object) url.TrimEnd(), DbType.String);
        pgDbQueryBuilder.Append(text);
        pgDbQueryBuilder.ExecuteNonQuery(parameter);
      }
      else
      {
        string str = "users_ccsite";
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        string text = "INSERT INTO " + str + " (userid, siteId, url) VALUES ('" + userId + "', '" + siteId + "', '" + url + "')";
        dbQueryBuilder.Append(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    [PgReady]
    public static void updateCCSiteInfo(CCSiteInfo site, int orgId)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        string tableName = "org_ccsite";
        PgDbQueryBuilder idbqb = new PgDbQueryBuilder();
        DbValueList nonPkColumnValues = new DbValueList()
        {
          new DbValue("siteId", (object) site.SiteId),
          new DbValue("url", (object) "@url", (IDbEncoder) DbEncoding.None)
        };
        PgQueryHelpers.Upsert((EllieMae.EMLite.DataAccess.PgDbQueryBuilder) idbqb, DbConstraint.None, tableName, new DbValue("oid", (object) orgId), nonPkColumnValues);
        DbCommandParameter[] array = new DbCommandParameter("url", (object) site.Url.TrimEnd(), DbType.AnsiString).ToArray();
        idbqb.ExecuteNonQuery(array);
      }
      else
      {
        string str = "org_ccsite";
        if (CCSiteInfoAccessor.Exists(orgId))
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          string text = "UPDATE " + str + " SET siteId='" + site.SiteId + "', url='" + site.Url + "' WHERE oid=" + (object) orgId ?? "";
          dbQueryBuilder.Append(text);
          dbQueryBuilder.ExecuteNonQuery();
        }
        else
          CCSiteInfoAccessor.createCCSiteInfo(orgId, site.SiteId, site.Url);
      }
    }

    [PgReady]
    public static void updateUserCCSiteInfo(CCSiteInfo site, string userId)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        string tableName = "users_ccsite";
        PgDbQueryBuilder idbqb = new PgDbQueryBuilder();
        DbValueList nonPkColumnValues = new DbValueList()
        {
          new DbValue("siteId", (object) site.SiteId),
          new DbValue("url", (object) "@url", (IDbEncoder) DbEncoding.None)
        };
        PgQueryHelpers.Upsert((EllieMae.EMLite.DataAccess.PgDbQueryBuilder) idbqb, DbConstraint.None, tableName, new DbValue("userid", (object) userId), nonPkColumnValues);
        DbCommandParameter[] array = new DbCommandParameter("url", (object) site.Url.TrimEnd(), DbType.AnsiString).ToArray();
        idbqb.ExecuteNonQuery(array);
      }
      else
      {
        string str = "users_ccsite";
        if (CCSiteInfoAccessor.UserExists(userId))
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          string text = "UPDATE " + str + " SET siteId='" + site.SiteId + "', url='" + site.Url + "' WHERE userid='" + userId + "'";
          dbQueryBuilder.Append(text);
          dbQueryBuilder.ExecuteNonQuery();
        }
        else
          CCSiteInfoAccessor.createUserCCSiteInfo(userId, site.SiteId, site.Url);
      }
    }

    public static void updateCCSiteInfo(string siteId, string url, int orgId)
    {
      string str = "org_ccsite";
      if (CCSiteInfoAccessor.Exists(orgId))
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        string text = "UPDATE " + str + " SET siteId='" + siteId + "', url='" + url + "' WHERE oid=" + (object) orgId ?? "";
        dbQueryBuilder.Append(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
      else
        CCSiteInfoAccessor.createCCSiteInfo(orgId, siteId, url);
    }

    [PgReady]
    public static void updateCCSiteId(string siteId, int orgId, string url)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        string tableName = "org_ccsite";
        PgDbQueryBuilder idbqb = new PgDbQueryBuilder();
        DbValueList nonPkColumnValues = new DbValueList()
        {
          new DbValue(nameof (siteId), (object) siteId),
          new DbValue(nameof (url), (object) "@url", (IDbEncoder) DbEncoding.None)
        };
        PgQueryHelpers.Upsert((EllieMae.EMLite.DataAccess.PgDbQueryBuilder) idbqb, DbConstraint.None, tableName, new DbValue("oid", (object) orgId), nonPkColumnValues);
        DbCommandParameter[] array = new DbCommandParameter(nameof (url), (object) url.TrimEnd(), DbType.AnsiString).ToArray();
        idbqb.ExecuteNonQuery(array);
      }
      else
      {
        string str = "org_ccsite";
        if (CCSiteInfoAccessor.Exists(orgId))
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          string text = "UPDATE " + str + " SET siteId='" + siteId + "', url='" + url + "' WHERE oid=" + (object) orgId ?? "";
          dbQueryBuilder.Append(text);
          dbQueryBuilder.ExecuteNonQuery();
        }
        else
          CCSiteInfoAccessor.createCCSiteInfo(orgId, siteId, url);
      }
    }

    [PgReady]
    public static Dictionary<int, CCSiteInfo> getCCSiteInfo(int[] orgIds)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("SELECT oid, siteId, url FROM org_ccsite WHERE oid in (" + string.Join<int>(", ", (IEnumerable<int>) orgIds) + ");");
        Dictionary<int, CCSiteInfo> dictionary = pgDbQueryBuilder.ExecuteTableQuery().Rows.Cast<DataRow>().ToDictionary<DataRow, int, CCSiteInfo>((System.Func<DataRow, int>) (row => SQL.DecodeInt(row["oid"])), (System.Func<DataRow, CCSiteInfo>) (row => CCSiteInfoAccessor.getCCSiteInfoFromDatarow(row)));
        return dictionary.Count <= 0 ? (Dictionary<int, CCSiteInfo>) null : dictionary;
      }
      string str1 = "org_ccsite";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      bool flag = false;
      string str2 = "SELECT oid, siteId, url FROM " + str1 + " WHERE oid in (";
      foreach (int orgId in orgIds)
      {
        if (flag)
          str2 += ",";
        else
          flag = true;
        str2 += (string) (object) orgId;
      }
      string text = str2 + ")";
      dbQueryBuilder.Append(text);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      if (dataTable.Rows.Count == 0)
        return (Dictionary<int, CCSiteInfo>) null;
      Dictionary<int, CCSiteInfo> ccSiteInfo = new Dictionary<int, CCSiteInfo>();
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        CCSiteInfo siteInfoFromDatarow = CCSiteInfoAccessor.getCCSiteInfoFromDatarow(dataTable.Rows[index]);
        ccSiteInfo[Convert.ToInt32(siteInfoFromDatarow.Id)] = siteInfoFromDatarow;
      }
      return ccSiteInfo;
    }

    [PgReady]
    public static CCSiteInfo getCCSiteInfo(int orgId)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        string str = "org_ccsite";
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        string text = "SELECT oid, siteId, url FROM " + str + " WHERE oid=" + (object) orgId ?? "";
        pgDbQueryBuilder.Append(text);
        DataTable dataTable = pgDbQueryBuilder.ExecuteTableQuery();
        return dataTable.Rows.Count != 0 ? CCSiteInfoAccessor.getCCSiteInfoFromDatarow(dataTable.Rows[0]) : (CCSiteInfo) null;
      }
      string str1 = "org_ccsite";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text1 = "SELECT oid, siteId, url FROM " + str1 + " WHERE oid=" + (object) orgId ?? "";
      dbQueryBuilder.Append(text1);
      DataTable dataTable1 = dbQueryBuilder.ExecuteTableQuery();
      return dataTable1.Rows.Count != 0 ? CCSiteInfoAccessor.getCCSiteInfoFromDatarow(dataTable1.Rows[0]) : (CCSiteInfo) null;
    }

    public static CCSiteInfo getUserCCSiteInfo(string userId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text = "SELECT a.userid, a.siteid, a.url, b.inheritParentccsite  FROM users_ccsite a, users b WHERE a.userid = " + SQL.Encode((object) userId) + " and a.userid = b.userid";
      dbQueryBuilder.Append(text);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      return dataTable.Rows.Count == 0 ? (CCSiteInfo) null : CCSiteInfoAccessor.getUserCCSiteInfoFromDatarow(dataTable.Rows[0]);
    }

    public static DataRow getUserCCSiteInfoRow(int userId)
    {
      string str = "users_ccsite";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text = "SELECT userid, siteId, url FROM " + str + " WHERE userid='" + (object) userId + "'";
      dbQueryBuilder.Append(text);
      return dbQueryBuilder.ExecuteTableQuery()?.Rows[0];
    }

    public static DataRow getCCSiteInfoRow(int orgId)
    {
      string str = "org_ccsite";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text = "SELECT oid, siteId, url FROM " + str + " WHERE oid=" + (object) orgId ?? "";
      dbQueryBuilder.Append(text);
      return dbQueryBuilder.ExecuteTableQuery()?.Rows[0];
    }

    [PgReady]
    private static bool Exists(int orgId)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        string str = "org_ccsite";
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        string text = "SELECT EXISTS (SELECT 1 FROM " + str + " WHERE oid=" + (object) orgId + ")";
        pgDbQueryBuilder.Append(text);
        return SQL.DecodeBoolean(pgDbQueryBuilder.ExecuteScalar());
      }
      string str1 = "org_ccsite";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text1 = "SELECT oid, siteId, url FROM " + str1 + " WHERE oid=" + (object) orgId ?? "";
      dbQueryBuilder.Append(text1);
      return dbQueryBuilder.ExecuteRowQuery() != null;
    }

    [PgReady]
    private static bool UserExists(string userId)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        string str = "users_ccsite";
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        string text = "SELECT EXISTS (SELECT 1 FROM " + str + " WHERE userid='" + userId + "')";
        pgDbQueryBuilder.Append(text);
        return SQL.DecodeBoolean(pgDbQueryBuilder.ExecuteScalar());
      }
      string str1 = "users_ccsite";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text1 = "SELECT userid, siteId, url FROM " + str1 + " WHERE userid='" + userId + "'";
      dbQueryBuilder.Append(text1);
      return dbQueryBuilder.ExecuteRowQuery() != null;
    }

    public static CCSiteInfo getCCSiteInfoFromDatarow(DataRow row)
    {
      return new CCSiteInfo()
      {
        Id = Convert.ToInt32(row["oid"]).ToString(),
        SiteId = string.Concat(SQL.Decode(row["siteId"])),
        Url = string.Concat(SQL.Decode(row["url"]))
      };
    }

    public static CCSiteInfo getUserCCSiteInfoFromDatarow(DataRow row)
    {
      return new CCSiteInfo()
      {
        Id = string.Concat(SQL.Decode(row["userid"])),
        SiteId = string.Concat(SQL.Decode(row["siteId"])),
        Url = string.Concat(SQL.Decode(row["url"])),
        UseParentInfo = string.IsNullOrEmpty(row["inheritParentccsite"].ToString()) || SQL.DecodeBoolean((object) row["inheritParentccsite"].ToString())
      };
    }
  }
}
