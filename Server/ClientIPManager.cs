// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ClientIPManager
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.DataAccess;
using System;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class ClientIPManager
  {
    private const string className = "ClientIPManager�";
    public static readonly string TableName = "AllowedIPAddresses";

    private static IPRange[] GetAllowedIPRangesForDB(IClientContext context)
    {
      IPRange[] allowedIpRangesForDb = (IPRange[]) null;
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder(context);
        dbQueryBuilder.Append("select * from [" + ClientIPManager.TableName + "]");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        allowedIpRangesForDb = new IPRange[dataRowCollection.Count];
        for (int index = 0; index < dataRowCollection.Count; ++index)
        {
          int int32 = Convert.ToInt32(dataRowCollection[index]["oid"]);
          string startIP = dataRowCollection[index]["startIP"] as string;
          string endIP = dataRowCollection[index]["endIP"] as string;
          string userid = dataRowCollection[index]["userid"] as string;
          allowedIpRangesForDb[index] = new IPRange(int32, userid, startIP, endIP);
        }
      }
      catch (Exception ex)
      {
        Err.Raise(context, nameof (ClientIPManager), new ServerException("Unable to get blocked IP addresses from database table [" + ClientIPManager.TableName + "]: " + ex.Message));
      }
      return allowedIpRangesForDb;
    }

    public static IPRange[] GetAllowedIPRanges(IClientContext context)
    {
      return context.Cache.Get<IPRange[]>(ClientIPManager.TableName, (Func<IPRange[]>) (() => ClientIPManager.GetAllowedIPRangesForDB(context)), CacheSetting.Low);
    }

    public static void DeleteAllAllowedIPRanges(IClientContext context)
    {
      context.Cache.Remove(ClientIPManager.TableName, (Action) (() =>
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("delete from [" + ClientIPManager.TableName + "]");
        dbQueryBuilder.ExecuteNonQuery();
      }), CacheSetting.Low);
    }

    public static void DeleteAllowedIPRange(IClientContext context, int oid)
    {
      ClientIPManager.DeleteAllowedIPRanges(context, new int[1]
      {
        oid
      });
    }

    public static void DeleteAllowedIPRanges(IClientContext context, int[] oids)
    {
      if (oids == null || oids.Length == 0)
        return;
      context.Cache.Put<IPRange[]>(ClientIPManager.TableName, (Action) (() =>
      {
        string[] strArray = new string[oids.Length];
        for (int index = 0; index < oids.Length; ++index)
          strArray[index] = "oid = " + (object) oids[index];
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("delete from [" + ClientIPManager.TableName + "] where " + string.Join(" or ", strArray));
        dbQueryBuilder.ExecuteNonQuery();
      }), (Func<IPRange[]>) (() => ClientIPManager.GetAllowedIPRangesForDB(context)), CacheSetting.Low);
    }

    public static void AddAllowedIPRange(IClientContext context, IPRange ipRange)
    {
      context.Cache.Put<IPRange[]>(ClientIPManager.TableName, (Action) (() =>
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        if (ipRange.Userid == null)
          dbQueryBuilder.Append(string.Format("insert into [{0}] (startIP, endIP) values ('{1}', '{2}')", (object) ClientIPManager.TableName, (object) ipRange.StartIP, (object) ipRange.EndIP));
        else
          dbQueryBuilder.Append(string.Format("insert into [{0}] (startIP, endIP, userid) values ('{1}', '{2}', {3})", (object) ClientIPManager.TableName, (object) ipRange.StartIP, (object) ipRange.EndIP, (object) SQL.Encode((object) ipRange.Userid)));
        dbQueryBuilder.ExecuteNonQuery();
      }), (Func<IPRange[]>) (() => ClientIPManager.GetAllowedIPRangesForDB(context)), CacheSetting.Low);
    }

    public static void UpdateAllowedIPRange(IClientContext context, IPRange ipRange)
    {
      context.Cache.Put<IPRange[]>(ClientIPManager.TableName, (Action) (() =>
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        if (ipRange.Userid == null)
          dbQueryBuilder.Append(string.Format("update [{0}] set userid = null, startIP = '{1}', endIP = '{2}' where oid = {3}", (object) ClientIPManager.TableName, (object) ipRange.StartIP, (object) ipRange.EndIP, (object) ipRange.OID));
        else
          dbQueryBuilder.Append(string.Format("update [{0}] set userid = {1}, startIP = '{2}', endIP = '{3}' where oid = {4}", (object) ClientIPManager.TableName, (object) SQL.Encode((object) ipRange.Userid), (object) ipRange.StartIP, (object) ipRange.EndIP, (object) ipRange.OID));
        dbQueryBuilder.ExecuteNonQuery();
      }), (Func<IPRange[]>) (() => ClientIPManager.GetAllowedIPRangesForDB(context)), CacheSetting.Low);
    }
  }
}
