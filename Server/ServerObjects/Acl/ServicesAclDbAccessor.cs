// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Acl.ServicesAclDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Cache;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Acl
{
  public class ServicesAclDbAccessor
  {
    private const string className = "ServicesAclDbAccessor�";
    private const string tableName = "[Acl_Services]�";
    private const string tableName_User = "[Acl_Services_User]�";
    private const string tableNameDefault = "[Acl_ServicesDefault]�";
    private const string tableNameDefault_User = "[Acl_ServicesDefault_User]�";
    private static int _epassAllServicesCacheExpirationMinutes;

    static ServicesAclDbAccessor()
    {
      int result;
      ServicesAclDbAccessor._epassAllServicesCacheExpirationMinutes = int.TryParse(ConfigurationManager.AppSettings["EpassAllServicesCache.ExpirationMinutes"], out result) ? result : 0;
    }

    public static ServiceAclInfo[] GetPermissions(AclFeature feature, int personaID)
    {
      ServiceAclInfo[] availableServices = ServicesAclDbAccessor.GetAllAvailableServices();
      ServiceAclInfo.ServicesDefaultSetting servicesDefaultSetting = ServicesAclDbAccessor.GetServicesDefaultSetting(feature, personaID);
      IDictionaryEnumerator enumerator = ServicesAclDbAccessor.GetPersonaPermissions(availableServices, feature, personaID, servicesDefaultSetting).GetEnumerator();
      List<ServiceAclInfo> serviceAclInfoList = new List<ServiceAclInfo>();
      while (enumerator.MoveNext())
        serviceAclInfoList.Add((ServiceAclInfo) enumerator.Value);
      return serviceAclInfoList.ToArray();
    }

    public static ServiceAclInfo.ServicesDefaultSetting GetServicesDefaultSetting(
      AclFeature feature,
      int personaID)
    {
      if (personaID == 1 || personaID == 0)
        return ServiceAclInfo.ServicesDefaultSetting.All;
      ServiceAclInfo.ServicesDefaultSetting servicesDefaultSetting = ServiceAclInfo.ServicesDefaultSetting.All;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from [Acl_ServicesDefault] where PersonaID = " + SQL.Encode((object) personaID));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
        servicesDefaultSetting = (ServiceAclInfo.ServicesDefaultSetting) dataRowCollection[0]["Access"];
      return servicesDefaultSetting;
    }

    public static ServiceAclInfo.ServicesDefaultSetting GetServicesDefaultSetting(
      AclFeature feature,
      string userID,
      int[] personaIDs)
    {
      ServiceAclInfo.ServicesDefaultSetting servicesDefaultSetting = ServicesAclDbAccessor.GetUserSpecificServicesDefaultSetting(feature, userID, personaIDs);
      if (servicesDefaultSetting == ServiceAclInfo.ServicesDefaultSetting.NotSpecified)
        servicesDefaultSetting = ServicesAclDbAccessor.GetPersonasServicesDefaultSetting(feature, userID, personaIDs);
      return servicesDefaultSetting;
    }

    [PgReady]
    public static ServiceAclInfo.ServicesDefaultSetting GetUserSpecificServicesDefaultSetting(
      AclFeature feature,
      string userID,
      int[] personaIDs)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        ServiceAclInfo.ServicesDefaultSetting servicesDefaultSetting = ServiceAclInfo.ServicesDefaultSetting.NotSpecified;
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select * from [Acl_ServicesDefault_User] where UserID = @userid");
        DbCommandParameter[] array = new DbCommandParameter("userid", (object) userID.TrimEnd(), DbType.AnsiString).ToArray();
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute(array);
        if (dataRowCollection != null && dataRowCollection.Count > 0)
          servicesDefaultSetting = (ServiceAclInfo.ServicesDefaultSetting) SQL.DecodeInt(dataRowCollection[0]["Access"]);
        return servicesDefaultSetting;
      }
      ServiceAclInfo.ServicesDefaultSetting servicesDefaultSetting1 = ServiceAclInfo.ServicesDefaultSetting.NotSpecified;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from [Acl_ServicesDefault_User] where UserID = " + SQL.Encode((object) userID));
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      if (dataRowCollection1 != null && dataRowCollection1.Count > 0)
        servicesDefaultSetting1 = (ServiceAclInfo.ServicesDefaultSetting) dataRowCollection1[0]["Access"];
      return servicesDefaultSetting1;
    }

    public static ServiceAclInfo.ServicesDefaultSetting GetPersonasServicesDefaultSetting(
      AclFeature feature,
      string userID,
      int[] personaIDs)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      ServiceAclInfo.ServicesDefaultSetting servicesDefaultSetting = ServiceAclInfo.ServicesDefaultSetting.All;
      foreach (int personaId in personaIDs)
      {
        switch (personaId)
        {
          case 0:
          case 1:
            return servicesDefaultSetting;
          default:
            continue;
        }
      }
      dbQueryBuilder.AppendLine("Select * from [Acl_ServicesDefault] where personaID in (" + SQL.EncodeArray((Array) personaIDs) + ") Order by Access DESC");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
        servicesDefaultSetting = (ServiceAclInfo.ServicesDefaultSetting) dataRowCollection[0]["Access"];
      return servicesDefaultSetting;
    }

    private static Hashtable GetPersonaPermissions(
      ServiceAclInfo[] services,
      AclFeature feature,
      int personaID,
      ServiceAclInfo.ServicesDefaultSetting defaultValue)
    {
      if (personaID == 1)
        ServicesAclDbAccessor.updateAdminPersona(feature, services);
      Hashtable personaPermissions = new Hashtable();
      if (defaultValue == ServiceAclInfo.ServicesDefaultSetting.All)
      {
        foreach (ServiceAclInfo service in services)
          service.PersonaAccess = AclResourceAccess.ReadWrite;
      }
      else
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("select * from [Acl_Services] where PersonaID = " + SQL.Encode((object) personaID));
        IEnumerator enumerator = dbQueryBuilder.Execute().GetEnumerator();
        try
        {
label_16:
          while (enumerator.MoveNext())
          {
            DataRow current = (DataRow) enumerator.Current;
            foreach (ServiceAclInfo service in services)
            {
              if (service.ServiceTitle == string.Concat(current["Title"]) && service.PersonaAccess != AclResourceAccess.ReadWrite)
              {
                switch (defaultValue)
                {
                  case ServiceAclInfo.ServicesDefaultSetting.None:
                    service.PersonaAccess = AclResourceAccess.ReadOnly;
                    goto label_16;
                  case ServiceAclInfo.ServicesDefaultSetting.Custom:
                    service.PersonaAccess = (byte) current["Access"] == (byte) 1 ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
                    goto label_16;
                  case ServiceAclInfo.ServicesDefaultSetting.All:
                    service.PersonaAccess = AclResourceAccess.ReadWrite;
                    goto label_16;
                  default:
                    goto label_16;
                }
              }
            }
          }
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
        foreach (ServiceAclInfo service in services)
        {
          if (service.PersonaAccess == AclResourceAccess.None)
          {
            switch (defaultValue)
            {
              case ServiceAclInfo.ServicesDefaultSetting.None:
                service.PersonaAccess = AclResourceAccess.ReadOnly;
                continue;
              case ServiceAclInfo.ServicesDefaultSetting.Custom:
              case ServiceAclInfo.ServicesDefaultSetting.All:
                service.PersonaAccess = AclResourceAccess.ReadWrite;
                continue;
              default:
                continue;
            }
          }
        }
      }
      foreach (ServiceAclInfo service in services)
        personaPermissions.Add((object) service.ServiceTitle, (object) service);
      return personaPermissions;
    }

    public static ServiceAclInfo[] GetPermissions(
      AclFeature feature,
      string userID,
      int[] personaIDs)
    {
      ServiceAclInfo[] availableServices = ServicesAclDbAccessor.GetAllAvailableServices();
      ServiceAclInfo.ServicesDefaultSetting servicesDefaultSetting1 = ServicesAclDbAccessor.GetUserSpecificServicesDefaultSetting(feature, userID, personaIDs);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from [Acl_Services_User] where UserID = " + SQL.Encode((object) userID));
      IEnumerator enumerator = dbQueryBuilder.Execute().GetEnumerator();
      try
      {
label_10:
        while (enumerator.MoveNext())
        {
          DataRow current = (DataRow) enumerator.Current;
          foreach (ServiceAclInfo serviceAclInfo in availableServices)
          {
            if (serviceAclInfo.ServiceTitle == string.Concat(current["Title"]))
            {
              switch (servicesDefaultSetting1)
              {
                case ServiceAclInfo.ServicesDefaultSetting.None:
                  serviceAclInfo.CustomAccess = AclResourceAccess.ReadOnly;
                  goto label_10;
                case ServiceAclInfo.ServicesDefaultSetting.Custom:
                case ServiceAclInfo.ServicesDefaultSetting.NotSpecified:
                  serviceAclInfo.CustomAccess = (byte) current["Access"] == (byte) 1 ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
                  goto label_10;
                case ServiceAclInfo.ServicesDefaultSetting.All:
                  serviceAclInfo.CustomAccess = AclResourceAccess.ReadWrite;
                  goto label_10;
                default:
                  goto label_10;
              }
            }
          }
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      foreach (int personaId in personaIDs)
      {
        ServiceAclInfo.ServicesDefaultSetting servicesDefaultSetting2 = ServicesAclDbAccessor.GetServicesDefaultSetting(feature, personaId);
        Hashtable personaPermissions = ServicesAclDbAccessor.GetPersonaPermissions(availableServices, feature, personaId, servicesDefaultSetting2);
        foreach (ServiceAclInfo serviceAclInfo in availableServices)
        {
          if (serviceAclInfo.PersonaAccess != AclResourceAccess.ReadWrite && personaPermissions[(object) serviceAclInfo.ServiceTitle] != null)
            serviceAclInfo.PersonaAccess = ((ServiceAclInfo) personaPermissions[(object) serviceAclInfo.ServiceTitle]).PersonaAccess;
        }
      }
      return availableServices;
    }

    private static ServiceAclInfo[] GetAllAvailableServices()
    {
      return ServicesAclDbAccessor._epassAllServicesCacheExpirationMinutes != 0 ? TimedDataCache.DataCache.Get<ServiceAclInfo[]>("AvailableServices", (Func<ServiceAclInfo[]>) (() => ServicesAclDbAccessor.getAllAvailableServices()), ServicesAclDbAccessor._epassAllServicesCacheExpirationMinutes) : ServicesAclDbAccessor.getAllAvailableServices();
    }

    public static void SetDefaultValue(
      AclFeature feature,
      string userid,
      ServiceAclInfo.ServicesDefaultSetting defaultValue)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete [Acl_ServicesDefault_User] where FeatureID = " + (object) (int) feature + " and UserID = " + SQL.Encode((object) userid));
      if (defaultValue != ServiceAclInfo.ServicesDefaultSetting.NotSpecified)
        dbQueryBuilder.AppendLine("Insert into [Acl_ServicesDefault_User] (FeatureID, UserID, Access) Values( " + (object) (int) feature + ", " + SQL.Encode((object) userid) + ", " + (object) (int) defaultValue + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetPermissions(
      AclFeature feature,
      ServiceAclInfo[] serviceAclInfoList,
      string userid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete [Acl_Services_User] where FeatureID = " + (object) (int) feature + " and UserID = " + SQL.Encode((object) userid));
      foreach (ServiceAclInfo serviceAclInfo in serviceAclInfoList)
      {
        if (serviceAclInfo.CustomAccess != AclResourceAccess.None)
          dbQueryBuilder.AppendLine("Insert into [Acl_Services_User] (Title, FeatureID, UserID, Access) Values( " + SQL.Encode((object) serviceAclInfo.ServiceTitle) + ", " + (object) (int) feature + ", " + SQL.Encode((object) userid) + ", " + (object) (serviceAclInfo.CustomAccess == AclResourceAccess.ReadWrite ? 1 : 0) + ")");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetDefaultValue(
      AclFeature feature,
      int personaID,
      ServiceAclInfo.ServicesDefaultSetting defaultValue)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete [Acl_ServicesDefault] where FeatureID = " + (object) (int) feature + " and PersonaID = " + SQL.Encode((object) personaID));
      dbQueryBuilder.AppendLine("Insert into [Acl_ServicesDefault] (FeatureID, PersonaID, Access) Values( " + (object) (int) feature + ", " + SQL.Encode((object) personaID) + ", " + (object) (int) defaultValue + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetPermissions(
      AclFeature feature,
      ServiceAclInfo[] serviceAclInfoList,
      int personaID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete [Acl_Services] where PersonaID = " + (object) personaID + " and FeatureID = " + (object) (int) feature);
      foreach (ServiceAclInfo serviceAclInfo in serviceAclInfoList)
        dbQueryBuilder.AppendLine("Insert [Acl_Services] (Title, FeatureID, PersonaID, Access) Values( " + SQL.Encode((object) serviceAclInfo.ServiceTitle) + ", " + (object) (int) feature + ", " + (object) personaID + ", " + (serviceAclInfo.PersonaAccess == AclResourceAccess.ReadWrite ? (object) "1" : (object) "0") + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DuplicateACLServices(int sourcePersonaID, int desPersonaID)
    {
      if (sourcePersonaID == 1)
        ServicesAclDbAccessor.GetPermissions(AclFeature.LoanTab_Other_ePASS, sourcePersonaID);
      string str1 = "";
      string str2 = "";
      ServiceAclInfo.ServicesDefaultSetting servicesDefaultSetting = ServicesAclDbAccessor.GetServicesDefaultSetting(AclFeature.LoanTab_Other_ePASS, sourcePersonaID);
      ServicesAclDbAccessor.SetDefaultValue(AclFeature.LoanTab_Other_ePASS, desPersonaID, servicesDefaultSetting);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from [Acl_Services] where personaID = " + (object) sourcePersonaID);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      dbQueryBuilder.Reset();
      DataColumnCollection columns = dataTable.Columns;
      if (dataTable == null || dataTable.Rows.Count <= 0)
        return;
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        string str3 = "Insert into [Acl_Services] (";
        for (int index = 0; index < columns.Count; ++index)
        {
          if (index == 0)
          {
            str3 += columns[index].ColumnName;
            str2 = !(columns[index].ColumnName.ToLower() != "personaid") ? str2 + SQL.Encode((object) desPersonaID) : str2 + SQL.Encode(row[columns[index].ColumnName]);
          }
          else
          {
            str3 = str3 + ", " + columns[index].ColumnName;
            str2 = !(columns[index].ColumnName.ToLower() != "personaid") ? str2 + ", " + SQL.Encode((object) desPersonaID) : str2 + ", " + SQL.Encode(row[columns[index].ColumnName]);
          }
        }
        string text = str3 + " ) Values (" + str2 + ")";
        dbQueryBuilder.AppendLine(text);
        str1 = "";
        str2 = "";
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static ServiceAclInfo[] getAllAvailableServices()
    {
      List<ServiceAclInfo> serviceAclInfoList = new List<ServiceAclInfo>();
      XmlDocument xmlDocument = new XmlDocument();
      string requestUriString = string.IsNullOrWhiteSpace(EnConfigurationSettings.AppSettings["EpassAiUrl"]) || !Uri.IsWellFormedUriString(EnConfigurationSettings.AppSettings["EpassAiUrl"], UriKind.Absolute) ? "https://www.epassbusinesscenter.com/epassai/getallservices.asp" : EnConfigurationSettings.AppSettings["EpassAiUrl"] + "getallservices.asp";
      try
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
        httpWebRequest.KeepAlive = false;
        httpWebRequest.Method = "GET";
        StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream());
        string end = streamReader.ReadToEnd();
        streamReader.Close();
        xmlDocument.LoadXml(end);
      }
      catch
      {
        return ServicesAclDbAccessor.getLocalServices();
      }
      foreach (XmlElement selectNode in xmlDocument.DocumentElement.SelectNodes("S"))
        serviceAclInfoList.Add(new ServiceAclInfo(selectNode.GetAttribute("Title"), 1023));
      serviceAclInfoList.Add(new ServiceAclInfo("Custom Links", 1023));
      TraceLog.WriteVerbose("ServiceAclDbAccesor", "AvailableServices : getAllAvailableServices() method call fetched data from service '" + requestUriString + "'");
      return serviceAclInfoList.ToArray();
    }

    private static ServiceAclInfo[] getLocalServices()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select distinct Title from Acl_Services");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      List<ServiceAclInfo> serviceAclInfoList = new List<ServiceAclInfo>();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          serviceAclInfoList.Add(new ServiceAclInfo(string.Concat(dataRow["Title"]), 1023));
      }
      return serviceAclInfoList.ToArray();
    }

    private static void updateAdminPersona(AclFeature feature, ServiceAclInfo[] services)
    {
      ServicesAclDbAccessor.SetDefaultValue(feature, 1, ServiceAclInfo.ServicesDefaultSetting.All);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete [Acl_Services] where PersonaID = " + (object) 1);
      foreach (ServiceAclInfo service in services)
        dbQueryBuilder.AppendLine("Insert [Acl_Services] (Title, FeatureID, PersonaID, Access) Values( " + SQL.Encode((object) service.ServiceTitle) + ", " + (object) (int) feature + ", " + (object) 1 + ", 1)");
      dbQueryBuilder.ExecuteNonQuery();
    }
  }
}
