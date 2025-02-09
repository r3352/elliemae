// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.SecondaryRegistrationAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class SecondaryRegistrationAccessor
  {
    private const string className = "SecondaryRegistrationAccessor�";
    private const string TABLENAME = "SecondaryAdditionalFields�";
    private const string additionalFieldsCacheName = "CachedLRAdditionalFields�";

    public static void UpdateLRAdditionalFields(LRAdditionalFields fields)
    {
      ClientContext.GetCurrent().Cache.Put<LRAdditionalFields>("CachedLRAdditionalFields", (Action) (() =>
      {
        TraceLog.WriteVerbose(nameof (SecondaryRegistrationAccessor), "SecondaryRegistrationAccessor.UpdateLRAdditionalFields: Creating SQL commands for table 'SecondaryAdditionalFields'.");
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("DELETE FROM SecondaryAdditionalFields");
        string[] fields1 = fields.GetFields(false);
        for (int index = 0; index < fields1.Length; ++index)
          dbQueryBuilder.AppendLine("INSERT INTO SecondaryAdditionalFields ([fieldID], [forRequest], [fieldOrder]) VALUES (" + SQL.Encode((object) fields1[index]) + ",'0'," + (object) index + ")");
        string[] fields2 = fields.GetFields(true);
        for (int index = 0; index < fields2.Length; ++index)
          dbQueryBuilder.AppendLine("INSERT INTO SecondaryAdditionalFields ([fieldID], [forRequest], [fieldOrder]) VALUES (" + SQL.Encode((object) fields2[index]) + ",'1'," + (object) index + ")");
        try
        {
          dbQueryBuilder.ExecuteNonQuery();
          TraceLog.WriteVerbose(nameof (SecondaryRegistrationAccessor), "SecondaryRegistrationAccessor.UpdateLRAdditionalFields: Trying to remove Secondary Additional Fields from cache...");
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (SecondaryRegistrationAccessor), ex);
          throw new Exception("Cannot add additional fields to SecondaryAdditionalFields table Due to the following problem:\r\n" + ex.Message);
        }
      }), new Func<LRAdditionalFields>(SecondaryRegistrationAccessor.GetLRAdditionalFieldsFromDB), CacheSetting.Low);
    }

    private static LRAdditionalFields GetLRAdditionalFieldsFromDB()
    {
      TraceLog.WriteVerbose(nameof (SecondaryRegistrationAccessor), "SecondaryRegistrationAccessor.GetLRAdditionalFieldsFromDB: Get Secondary Additional Fields from database...");
      try
      {
        ClientContext current = ClientContext.GetCurrent();
        return EllieMae.EMLite.ReportingDbUtils.Query.SecondaryRegistrationAccessor.GetLRAdditionalFields(DbQueryBuilder.getConnectionString(DBReadReplicaFeature.Login), current.Settings.DbServerType);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (SecondaryRegistrationAccessor), ex);
        throw ex;
      }
    }

    public static LRAdditionalFields GetLRAdditionalFields()
    {
      return ClientContext.GetCurrent().Cache.Get<LRAdditionalFields>("CachedLRAdditionalFields", new Func<LRAdditionalFields>(SecondaryRegistrationAccessor.GetLRAdditionalFieldsFromDB), CacheSetting.Low);
    }

    private static bool isCachingEnabled()
    {
      return ClientContext.GetCurrent().Settings.CacheSetting >= CacheSetting.Low;
    }
  }
}
