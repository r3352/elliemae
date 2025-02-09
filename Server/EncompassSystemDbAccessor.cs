// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.EncompassSystemDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.DataAccess;
using System;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class EncompassSystemDbAccessor
  {
    private const string className = "EncompassSystemDbAccessor�";
    private const string cacheName = "EncompassSystemInfo�";

    private EncompassSystemDbAccessor()
    {
    }

    public static EncompassSystemInfo GetEncompassSystemInfo()
    {
      return EncompassSystemDbAccessor.GetEncompassSystemInfo((IClientContext) ClientContext.GetCurrent());
    }

    public static EncompassSystemInfo GetEncompassSystemInfo(IClientContext context)
    {
      if (context.Cache.Get("EncompassSystemInfo") is EncompassSystemInfo encompassSystemInfo)
        return encompassSystemInfo;
      Guid.NewGuid().ToString();
      bool DataServicesOptOut = false;
      string DataServicesOptKey = "";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder(context);
      dbQueryBuilder.AppendLine("select * from [EncompassSystem]");
      DataRowCollection dataRowCollection;
      try
      {
        dataRowCollection = dbQueryBuilder.Execute();
      }
      catch (Exception ex)
      {
        context.TraceLog.WriteException(TraceLevel.Error, nameof (EncompassSystemDbAccessor), ex);
        throw new Exception("Cannot get system ID from database.\r\n" + ex.Message);
      }
      if (dataRowCollection == null)
        throw new Exception("Error getting system ID.");
      string systemID = dataRowCollection.Count == 1 ? dataRowCollection[0]["systemID"].ToString().Trim() : throw new Exception("Error getting system ID.  System ID count = " + (object) dataRowCollection.Count);
      if (dataRowCollection[0]["data_services_opt"] != DBNull.Value && !string.IsNullOrEmpty(dataRowCollection[0]["data_services_opt"].ToString()))
        DataServicesOptOut = true;
      string dbFullVersion = "";
      try
      {
        if (dataRowCollection[0]["DbFullVersion"] != DBNull.Value)
          dbFullVersion = (string) dataRowCollection[0]["DbFullVersion"];
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (EncompassSystemDbAccessor), "Error getting Encompass DB full version: " + ex.Message);
      }
      EncompassSystemInfo o = new EncompassSystemInfo(systemID, string.Concat(dataRowCollection[0]["DbVersion"]), (DateTime) dataRowCollection[0]["creationTime"], DataServicesOptOut, DataServicesOptKey, dbFullVersion);
      context.Cache.Put("EncompassSystemInfo", (object) o, CacheSetting.Low);
      return o;
    }

    public static void UpdateCompanyDataServicesOpt(string key)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Update [EncompassSystem] set data_services_opt = " + SQL.Encode((object) key));
      try
      {
        dbQueryBuilder.ExecuteNonQuery();
        if (ClientContext.GetCurrent().Cache.Get("EncompassSystemInfo") is EncompassSystemInfo)
          ClientContext.GetCurrent().Cache.Remove("EncompassSystemInfo");
        EncompassSystemDbAccessor.GetEncompassSystemInfo();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (EncompassSystemDbAccessor), ex);
        throw new Exception("Cannot update data services opt in database.\r\n" + ex.Message);
      }
    }
  }
}
