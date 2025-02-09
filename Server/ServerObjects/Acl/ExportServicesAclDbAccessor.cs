// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Acl.ExportServicesAclDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Acl
{
  public class ExportServicesAclDbAccessor
  {
    private const string className = "ExportServicesAclDbAccessor�";
    private const string tableName = "[Acl_ExportServices]�";
    private const string tableName_User = "[Acl_ExportServices_User]�";
    private const string tableNameDefault = "[Acl_ExportServicesDefault]�";
    private const string tableNameDefault_User = "[Acl_ExportServicesDefault_User]�";
    private const string userKey = "UserID�";
    private const string personaKey = "PersonaID�";
    private const string featureKey = "FeatureID�";
    private const string groupKey = "GroupName�";
    private const string accessKey = "Access�";

    public static ExportServiceAclInfo[] GetPermissions(
      AclFeature feature,
      int personaID,
      ExportServiceAclInfo[] availableServices)
    {
      ExportServiceAclInfo.ExportServicesDefaultSetting servicesDefaultSetting = ExportServicesAclDbAccessor.GetServicesDefaultSetting(feature, personaID);
      IDictionaryEnumerator enumerator = ExportServicesAclDbAccessor.GetPersonaPermissions(availableServices, feature, personaID, servicesDefaultSetting).GetEnumerator();
      List<ExportServiceAclInfo> exportServiceAclInfoList = new List<ExportServiceAclInfo>();
      while (enumerator.MoveNext())
        exportServiceAclInfoList.Add((ExportServiceAclInfo) enumerator.Value);
      return exportServiceAclInfoList.ToArray();
    }

    [PgReady]
    public static ExportServiceAclInfo.ExportServicesDefaultSetting GetServicesDefaultSetting(
      AclFeature feature,
      int personaID)
    {
      if (personaID == 1 || personaID == 0 || Company.GetCurrentEdition() == EncompassEdition.Broker)
        return ExportServiceAclInfo.ExportServicesDefaultSetting.All;
      ExportServiceAclInfo.ExportServicesDefaultSetting servicesDefaultSetting = ExportServiceAclInfo.ExportServicesDefaultSetting.All;
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select * from [Acl_ExportServicesDefault] where PersonaID = " + SQL.Encode((object) personaID) + " and FeatureID = " + SQL.Encode((object) (int) feature));
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
          servicesDefaultSetting = (ExportServiceAclInfo.ExportServicesDefaultSetting) dataRowCollection[0]["Access"];
        return servicesDefaultSetting;
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from [Acl_ExportServicesDefault] where PersonaID = " + SQL.Encode((object) personaID) + " and FeatureID = " + SQL.Encode((object) (int) feature));
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      if (dataRowCollection1 != null && dataRowCollection1.Count > 0)
        servicesDefaultSetting = (ExportServiceAclInfo.ExportServicesDefaultSetting) dataRowCollection1[0]["Access"];
      return servicesDefaultSetting;
    }

    public static ExportServiceAclInfo.ExportServicesDefaultSetting GetServicesDefaultSetting(
      AclFeature feature,
      string userID,
      int[] personaIDs)
    {
      ExportServiceAclInfo.ExportServicesDefaultSetting servicesDefaultSetting = ExportServicesAclDbAccessor.GetUserSpecificServicesDefaultSetting(feature, userID, personaIDs);
      if (servicesDefaultSetting == ExportServiceAclInfo.ExportServicesDefaultSetting.NotSpecified)
        servicesDefaultSetting = ExportServicesAclDbAccessor.GetPersonasServicesDefaultSetting(feature, userID, personaIDs);
      return servicesDefaultSetting;
    }

    [PgReady]
    public static ExportServiceAclInfo.ExportServicesDefaultSetting GetUserSpecificServicesDefaultSetting(
      AclFeature feature,
      string userID,
      int[] personaIDs)
    {
      ExportServiceAclInfo.ExportServicesDefaultSetting servicesDefaultSetting = ExportServiceAclInfo.ExportServicesDefaultSetting.NotSpecified;
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select * from [Acl_ExportServicesDefault_User] where UserID = @userid and FeatureID = " + SQL.Encode((object) (int) feature));
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute(DbTransactionType.Default, new DbCommandParameter("userid", (object) userID.TrimEnd(), DbType.AnsiString).ToArray());
        if (dataRowCollection != null && dataRowCollection.Count > 0)
          servicesDefaultSetting = (ExportServiceAclInfo.ExportServicesDefaultSetting) dataRowCollection[0]["Access"];
        return servicesDefaultSetting;
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from [Acl_ExportServicesDefault_User] where UserID = " + SQL.Encode((object) userID) + " and FeatureID = " + SQL.Encode((object) (int) feature));
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      if (dataRowCollection1 != null && dataRowCollection1.Count > 0)
        servicesDefaultSetting = (ExportServiceAclInfo.ExportServicesDefaultSetting) dataRowCollection1[0]["Access"];
      return servicesDefaultSetting;
    }

    public static ExportServiceAclInfo.ExportServicesDefaultSetting GetPersonasServicesDefaultSetting(
      AclFeature feature,
      string userID,
      int[] personaIDs)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      ExportServiceAclInfo.ExportServicesDefaultSetting servicesDefaultSetting = ExportServiceAclInfo.ExportServicesDefaultSetting.All;
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
      dbQueryBuilder.AppendLine("Select * from [Acl_ExportServicesDefault] where personaID in (" + SQL.EncodeArray((Array) personaIDs) + ")  and FeatureID = " + SQL.Encode((object) (int) feature) + " Order by Access DESC");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
        servicesDefaultSetting = (ExportServiceAclInfo.ExportServicesDefaultSetting) dataRowCollection[0]["Access"];
      return servicesDefaultSetting;
    }

    [PgReady]
    private static Hashtable GetPersonaPermissions(
      ExportServiceAclInfo[] services,
      AclFeature feature,
      int personaID,
      ExportServiceAclInfo.ExportServicesDefaultSetting defaultValue)
    {
      Hashtable personaPermissions = new Hashtable();
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        if (personaID == 1)
          ExportServicesAclDbAccessor.updateAdminPersona(feature, services);
        if (defaultValue == ExportServiceAclInfo.ExportServicesDefaultSetting.All)
        {
          foreach (ExportServiceAclInfo service in services)
            service.PersonaAccess = AclResourceAccess.ReadWrite;
        }
        else
        {
          EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
          pgDbQueryBuilder.AppendLine("select * from [Acl_ExportServices] where PersonaID = " + SQL.Encode((object) personaID) + " and FeatureID = " + SQL.Encode((object) (int) feature));
          IEnumerator enumerator = pgDbQueryBuilder.Execute().GetEnumerator();
          try
          {
label_17:
            while (enumerator.MoveNext())
            {
              DataRow current = (DataRow) enumerator.Current;
              foreach (ExportServiceAclInfo service in services)
              {
                if (service.ExportGroup == string.Concat(current["GroupName"]) && service.PersonaAccess != AclResourceAccess.ReadWrite)
                {
                  switch (defaultValue)
                  {
                    case ExportServiceAclInfo.ExportServicesDefaultSetting.None:
                      service.PersonaAccess = AclResourceAccess.ReadOnly;
                      goto label_17;
                    case ExportServiceAclInfo.ExportServicesDefaultSetting.Custom:
                      service.PersonaAccess = SQL.DecodeInt(current["Access"]) == 1 ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
                      goto label_17;
                    case ExportServiceAclInfo.ExportServicesDefaultSetting.All:
                      service.PersonaAccess = AclResourceAccess.ReadWrite;
                      goto label_17;
                    default:
                      goto label_17;
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
          foreach (ExportServiceAclInfo service in services)
          {
            if (service.PersonaAccess == AclResourceAccess.None)
            {
              switch (defaultValue)
              {
                case ExportServiceAclInfo.ExportServicesDefaultSetting.None:
                  service.PersonaAccess = AclResourceAccess.ReadOnly;
                  continue;
                case ExportServiceAclInfo.ExportServicesDefaultSetting.Custom:
                case ExportServiceAclInfo.ExportServicesDefaultSetting.All:
                  service.PersonaAccess = AclResourceAccess.ReadWrite;
                  continue;
                default:
                  continue;
              }
            }
          }
        }
        foreach (ExportServiceAclInfo service in services)
          personaPermissions.Add((object) service.ExportGroup, (object) service);
        return personaPermissions;
      }
      if (personaID == 1)
        ExportServicesAclDbAccessor.updateAdminPersona(feature, services);
      if (defaultValue == ExportServiceAclInfo.ExportServicesDefaultSetting.All)
      {
        foreach (ExportServiceAclInfo service in services)
          service.PersonaAccess = AclResourceAccess.ReadWrite;
      }
      else
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("select * from [Acl_ExportServices] where PersonaID = " + SQL.Encode((object) personaID) + " and FeatureID = " + SQL.Encode((object) (int) feature));
        IEnumerator enumerator = dbQueryBuilder.Execute().GetEnumerator();
        try
        {
label_48:
          while (enumerator.MoveNext())
          {
            DataRow current = (DataRow) enumerator.Current;
            foreach (ExportServiceAclInfo service in services)
            {
              if (service.ExportGroup == string.Concat(current["GroupName"]) && service.PersonaAccess != AclResourceAccess.ReadWrite)
              {
                switch (defaultValue)
                {
                  case ExportServiceAclInfo.ExportServicesDefaultSetting.None:
                    service.PersonaAccess = AclResourceAccess.ReadOnly;
                    goto label_48;
                  case ExportServiceAclInfo.ExportServicesDefaultSetting.Custom:
                    service.PersonaAccess = (byte) current["Access"] == (byte) 1 ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
                    goto label_48;
                  case ExportServiceAclInfo.ExportServicesDefaultSetting.All:
                    service.PersonaAccess = AclResourceAccess.ReadWrite;
                    goto label_48;
                  default:
                    goto label_48;
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
        foreach (ExportServiceAclInfo service in services)
        {
          if (service.PersonaAccess == AclResourceAccess.None)
          {
            switch (defaultValue)
            {
              case ExportServiceAclInfo.ExportServicesDefaultSetting.None:
                service.PersonaAccess = AclResourceAccess.ReadOnly;
                continue;
              case ExportServiceAclInfo.ExportServicesDefaultSetting.Custom:
              case ExportServiceAclInfo.ExportServicesDefaultSetting.All:
                service.PersonaAccess = AclResourceAccess.ReadWrite;
                continue;
              default:
                continue;
            }
          }
        }
      }
      foreach (ExportServiceAclInfo service in services)
        personaPermissions.Add((object) service.ExportGroup, (object) service);
      return personaPermissions;
    }

    [PgReady]
    public static ExportServiceAclInfo[] GetPermissions(
      AclFeature feature,
      string userID,
      int[] personaIDs,
      ExportServiceAclInfo[] availableServices)
    {
      ExportServiceAclInfo.ExportServicesDefaultSetting servicesDefaultSetting1 = ExportServicesAclDbAccessor.GetUserSpecificServicesDefaultSetting(feature, userID, personaIDs);
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select * from [Acl_ExportServices_User] where UserID = @userid and FeatureID = " + SQL.Encode((object) (int) feature));
        IEnumerator enumerator = pgDbQueryBuilder.Execute(DbTransactionType.Default, new DbCommandParameter("userid", (object) userID.TrimEnd(), DbType.AnsiString).ToArray()).GetEnumerator();
        try
        {
label_11:
          while (enumerator.MoveNext())
          {
            DataRow current = (DataRow) enumerator.Current;
            foreach (ExportServiceAclInfo availableService in availableServices)
            {
              if (availableService.ExportGroup == string.Concat(current["GroupName"]))
              {
                switch (servicesDefaultSetting1)
                {
                  case ExportServiceAclInfo.ExportServicesDefaultSetting.None:
                    availableService.CustomAccess = AclResourceAccess.ReadOnly;
                    goto label_11;
                  case ExportServiceAclInfo.ExportServicesDefaultSetting.Custom:
                  case ExportServiceAclInfo.ExportServicesDefaultSetting.NotSpecified:
                    availableService.CustomAccess = SQL.DecodeInt(current["Access"]) == 1 ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
                    goto label_11;
                  case ExportServiceAclInfo.ExportServicesDefaultSetting.All:
                    availableService.CustomAccess = AclResourceAccess.ReadWrite;
                    goto label_11;
                  default:
                    goto label_11;
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
          ExportServiceAclInfo.ExportServicesDefaultSetting servicesDefaultSetting2 = ExportServicesAclDbAccessor.GetServicesDefaultSetting(feature, personaId);
          Hashtable personaPermissions = ExportServicesAclDbAccessor.GetPersonaPermissions(availableServices, feature, personaId, servicesDefaultSetting2);
          foreach (ExportServiceAclInfo availableService in availableServices)
          {
            if (availableService.PersonaAccess != AclResourceAccess.ReadWrite && personaPermissions[(object) availableService.ExportGroup] != null)
              availableService.PersonaAccess = ((ExportServiceAclInfo) personaPermissions[(object) availableService.ExportGroup]).PersonaAccess;
          }
        }
        return availableServices;
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from [Acl_ExportServices_User] where UserID = " + SQL.Encode((object) userID) + " and FeatureID = " + SQL.Encode((object) (int) feature));
      IEnumerator enumerator1 = dbQueryBuilder.Execute().GetEnumerator();
      try
      {
label_34:
        while (enumerator1.MoveNext())
        {
          DataRow current = (DataRow) enumerator1.Current;
          foreach (ExportServiceAclInfo availableService in availableServices)
          {
            if (availableService.ExportGroup == string.Concat(current["GroupName"]))
            {
              switch (servicesDefaultSetting1)
              {
                case ExportServiceAclInfo.ExportServicesDefaultSetting.None:
                  availableService.CustomAccess = AclResourceAccess.ReadOnly;
                  goto label_34;
                case ExportServiceAclInfo.ExportServicesDefaultSetting.Custom:
                case ExportServiceAclInfo.ExportServicesDefaultSetting.NotSpecified:
                  availableService.CustomAccess = (byte) current["Access"] == (byte) 1 ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
                  goto label_34;
                case ExportServiceAclInfo.ExportServicesDefaultSetting.All:
                  availableService.CustomAccess = AclResourceAccess.ReadWrite;
                  goto label_34;
                default:
                  goto label_34;
              }
            }
          }
        }
      }
      finally
      {
        if (enumerator1 is IDisposable disposable)
          disposable.Dispose();
      }
      foreach (int personaId in personaIDs)
      {
        ExportServiceAclInfo.ExportServicesDefaultSetting servicesDefaultSetting3 = ExportServicesAclDbAccessor.GetServicesDefaultSetting(feature, personaId);
        Hashtable personaPermissions = ExportServicesAclDbAccessor.GetPersonaPermissions(availableServices, feature, personaId, servicesDefaultSetting3);
        foreach (ExportServiceAclInfo availableService in availableServices)
        {
          if (availableService.PersonaAccess != AclResourceAccess.ReadWrite && personaPermissions[(object) availableService.ExportGroup] != null)
            availableService.PersonaAccess = ((ExportServiceAclInfo) personaPermissions[(object) availableService.ExportGroup]).PersonaAccess;
        }
      }
      return availableServices;
    }

    public static void SetDefaultValue(
      AclFeature feature,
      string userid,
      ExportServiceAclInfo.ExportServicesDefaultSetting defaultValue)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete [Acl_ExportServicesDefault_User] where FeatureID = " + (object) (int) feature + " and UserID = " + SQL.Encode((object) userid));
      if (defaultValue != ExportServiceAclInfo.ExportServicesDefaultSetting.NotSpecified)
        dbQueryBuilder.AppendLine("Insert into [Acl_ExportServicesDefault_User] (FeatureID, UserID, Access) Values( " + (object) (int) feature + ", " + SQL.Encode((object) userid) + ", " + (object) (int) defaultValue + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetPermissions(
      AclFeature feature,
      ExportServiceAclInfo[] ExportServiceAclInfoList,
      string userid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete [Acl_ExportServices_User] where FeatureID = " + (object) (int) feature + " and UserID = " + SQL.Encode((object) userid));
      foreach (ExportServiceAclInfo exportServiceAclInfo in ExportServiceAclInfoList)
      {
        if (exportServiceAclInfo.CustomAccess != AclResourceAccess.None)
          dbQueryBuilder.AppendLine("Insert into [Acl_ExportServices_User] (GroupName, FeatureID, UserID, Access) Values( " + SQL.Encode((object) exportServiceAclInfo.ExportGroup) + ", " + (object) (int) feature + ", " + SQL.Encode((object) userid) + ", " + (object) (exportServiceAclInfo.CustomAccess == AclResourceAccess.ReadWrite ? 1 : 0) + ")");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetDefaultValue(
      AclFeature feature,
      int personaID,
      ExportServiceAclInfo.ExportServicesDefaultSetting defaultValue)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete [Acl_ExportServicesDefault] where FeatureID = " + (object) (int) feature + " and PersonaID = " + SQL.Encode((object) personaID));
      dbQueryBuilder.AppendLine("Insert into [Acl_ExportServicesDefault] (FeatureID, PersonaID, Access) Values( " + (object) (int) feature + ", " + SQL.Encode((object) personaID) + ", " + (object) (int) defaultValue + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetPermissions(
      AclFeature feature,
      ExportServiceAclInfo[] ExportServiceAclInfoList,
      int personaID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete [Acl_ExportServices] where PersonaID = " + (object) personaID + " and FeatureID = " + (object) (int) feature);
      foreach (ExportServiceAclInfo exportServiceAclInfo in ExportServiceAclInfoList)
        dbQueryBuilder.AppendLine("Insert [Acl_ExportServices] (GroupName, FeatureID, PersonaID, Access) Values( " + SQL.Encode((object) exportServiceAclInfo.ExportGroup) + ", " + (object) (int) feature + ", " + (object) personaID + ", " + (exportServiceAclInfo.PersonaAccess == AclResourceAccess.ReadWrite ? (object) "1" : (object) "0") + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DuplicateACLServices(
      int sourcePersonaID,
      int desPersonaID,
      ExportServiceAclInfo[] availableServices)
    {
      if (sourcePersonaID == 1)
        ExportServicesAclDbAccessor.GetPermissions(AclFeature.LoanMgmt_MgmtPipelineServices, sourcePersonaID, availableServices);
      string str1 = "";
      string str2 = "";
      ExportServiceAclInfo.ExportServicesDefaultSetting servicesDefaultSetting = ExportServicesAclDbAccessor.GetServicesDefaultSetting(AclFeature.LoanMgmt_MgmtPipelineServices, sourcePersonaID);
      ExportServicesAclDbAccessor.SetDefaultValue(AclFeature.LoanMgmt_MgmtPipelineServices, desPersonaID, servicesDefaultSetting);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from [Acl_ExportServices] where personaID = " + (object) sourcePersonaID);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      dbQueryBuilder.Reset();
      DataColumnCollection columns = dataTable.Columns;
      if (dataTable == null || dataTable.Rows.Count <= 0)
        return;
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        string str3 = "Insert into [Acl_ExportServices] (";
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

    private static ExportServiceAclInfo[] getLocalServices()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select distinct GroupName from [Acl_ExportServices]");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      List<ExportServiceAclInfo> exportServiceAclInfoList = new List<ExportServiceAclInfo>();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          exportServiceAclInfoList.Add(new ExportServiceAclInfo(string.Concat(dataRow["GroupName"]), 229));
      }
      return exportServiceAclInfoList.ToArray();
    }

    private static void updateAdminPersona(AclFeature feature, ExportServiceAclInfo[] services)
    {
      ExportServicesAclDbAccessor.SetDefaultValue(feature, 1, ExportServiceAclInfo.ExportServicesDefaultSetting.All);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete [Acl_ExportServices] where PersonaID = " + (object) 1);
      foreach (ExportServiceAclInfo service in services)
        dbQueryBuilder.AppendLine("Insert [Acl_ExportServices] (GroupName, FeatureID, PersonaID, Access) Values( " + SQL.Encode((object) service.ExportGroup) + ", " + (object) (int) feature + ", " + (object) 1 + ", 1)");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static Dictionary<string, ExportServiceAclInfo> GetAllAccessInfo(
      AclFeature feature,
      string userID,
      int[] personaIDs,
      ExportServiceAclInfo.ExportServicesDefaultSetting userDefault,
      ExportServiceAclInfo.ExportServicesDefaultSetting personaDefault)
    {
      Dictionary<string, ExportServiceAclInfo> knownExportInfo = new Dictionary<string, ExportServiceAclInfo>();
      if (userID == null)
        return ExportServicesAclDbAccessor.GetMaxPermissionsForPersonas(feature, personaIDs, personaDefault, knownExportInfo);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("select * from {0} where {1} = {2} and {3} = {4}", (object) "[Acl_ExportServices_User]", (object) "UserID", (object) SQL.Encode((object) userID), (object) "FeatureID", (object) SQL.Encode((object) (int) feature)));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null)
        return ExportServicesAclDbAccessor.GetMaxPermissionsForPersonas(feature, personaIDs, personaDefault, knownExportInfo);
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        string str = string.Concat(dataRow["GroupName"]);
        AclResourceAccess aclResourceAccess = (byte) dataRow["Access"] == (byte) 1 ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
        switch (userDefault)
        {
          case ExportServiceAclInfo.ExportServicesDefaultSetting.None:
            aclResourceAccess = AclResourceAccess.ReadOnly;
            break;
          case ExportServiceAclInfo.ExportServicesDefaultSetting.All:
            aclResourceAccess = AclResourceAccess.ReadWrite;
            break;
        }
        ExportServiceAclInfo exportServiceAclInfo;
        if (knownExportInfo.ContainsKey(str))
        {
          exportServiceAclInfo = knownExportInfo[str];
        }
        else
        {
          exportServiceAclInfo = new ExportServiceAclInfo(str, (int) feature);
          knownExportInfo[str] = exportServiceAclInfo;
        }
        exportServiceAclInfo.CustomAccess = aclResourceAccess;
      }
      return personaIDs == null || !((IEnumerable<int>) personaIDs).Any<int>() ? knownExportInfo : ExportServicesAclDbAccessor.GetMaxPermissionsForPersonas(feature, personaIDs, personaDefault, knownExportInfo);
    }

    public static Dictionary<string, ExportServiceAclInfo> GetMaxPermissionsForPersonas(
      AclFeature feature,
      int[] personaIDs,
      ExportServiceAclInfo.ExportServicesDefaultSetting personaDefault,
      Dictionary<string, ExportServiceAclInfo> knownExportInfo = null)
    {
      Dictionary<string, ExportServiceAclInfo> permissionsForPersonas = knownExportInfo ?? new Dictionary<string, ExportServiceAclInfo>();
      string columnName = "top_access";
      string text = string.Format("select {0}, max({1}) as {2} from {3} where {4} in ({5}) group by {0}", (object) "GroupName", (object) "Access", (object) columnName, (object) "[Acl_ExportServices]", (object) "PersonaID", (object) SQL.EncodeArray((Array) personaIDs), (object) "GroupName");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(text);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null)
        return permissionsForPersonas;
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        string str = string.Concat(dataRow["GroupName"]);
        AclResourceAccess aclResourceAccess = (byte) dataRow[columnName] == (byte) 1 ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
        switch (personaDefault)
        {
          case ExportServiceAclInfo.ExportServicesDefaultSetting.None:
            aclResourceAccess = AclResourceAccess.ReadOnly;
            break;
          case ExportServiceAclInfo.ExportServicesDefaultSetting.All:
            aclResourceAccess = AclResourceAccess.ReadWrite;
            break;
        }
        ExportServiceAclInfo exportServiceAclInfo;
        if (permissionsForPersonas.ContainsKey(str))
        {
          exportServiceAclInfo = permissionsForPersonas[str];
        }
        else
        {
          exportServiceAclInfo = new ExportServiceAclInfo(str, (int) feature);
          permissionsForPersonas[str] = exportServiceAclInfo;
        }
        exportServiceAclInfo.PersonaAccess = aclResourceAccess;
      }
      return permissionsForPersonas;
    }
  }
}
