// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Acl.InvestorServicesAclDbAccessor
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
  public class InvestorServicesAclDbAccessor
  {
    private const string className = "InvestorServicesAclDbAccessor�";
    private const string tableName = "[Acl_InvestorServices]�";
    private const string tableName_User = "[Acl_InvestorServices_User]�";
    private const string tableNameDefault = "[Acl_InvestorServicesDefault]�";
    private const string tableNameDefault_User = "[Acl_InvestorServicesDefault_User]�";
    private const string featureKey = "FeatureID�";
    private const string defaultAccessKey = "Access�";
    private const string accessKey = "Access�";
    private const string categoryKey = "Category�";
    private const string personaKey = "PersonaID�";
    private const string userKey = "userID�";
    private const string providerKey = "ProviderCompanyCode�";

    public static InvestorServiceAclInfo[] GetPermissions(
      AclFeature feature,
      int personaID,
      string category,
      InvestorServiceAclInfo[] availableServices)
    {
      InvestorServiceAclInfo.InvestorServicesDefaultSetting servicesDefaultSetting = InvestorServicesAclDbAccessor.GetServicesDefaultSetting(feature, personaID, category);
      IDictionaryEnumerator enumerator = InvestorServicesAclDbAccessor.GetPersonaPermissions(availableServices, feature, personaID, category, servicesDefaultSetting).GetEnumerator();
      List<InvestorServiceAclInfo> investorServiceAclInfoList = new List<InvestorServiceAclInfo>();
      while (enumerator.MoveNext())
        investorServiceAclInfoList.Add((InvestorServiceAclInfo) enumerator.Value);
      return investorServiceAclInfoList.ToArray();
    }

    public static InvestorServiceAclInfo.InvestorServicesDefaultSetting GetServicesDefaultSetting(
      AclFeature feature,
      int personaID,
      string category)
    {
      if (personaID == 1 || personaID == 0 || Company.GetCurrentEdition() == EncompassEdition.Broker)
        return InvestorServiceAclInfo.InvestorServicesDefaultSetting.All;
      InvestorServiceAclInfo.InvestorServicesDefaultSetting servicesDefaultSetting = InvestorServiceAclInfo.InvestorServicesDefaultSetting.None;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from [Acl_InvestorServicesDefault] where PersonaID = " + SQL.Encode((object) personaID) + " and FeatureID = " + SQL.Encode((object) (int) feature) + " and Category = " + SQL.Encode((object) category));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
        servicesDefaultSetting = (InvestorServiceAclInfo.InvestorServicesDefaultSetting) dataRowCollection[0]["Access"];
      return servicesDefaultSetting;
    }

    public static InvestorServiceAclInfo.InvestorServicesDefaultSetting GetServicesDefaultSetting(
      AclFeature feature,
      string userID,
      string category,
      int[] personaIDs)
    {
      InvestorServiceAclInfo.InvestorServicesDefaultSetting servicesDefaultSetting = InvestorServicesAclDbAccessor.GetUserSpecificServicesDefaultSetting(feature, userID, category, personaIDs);
      if (servicesDefaultSetting == InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified)
        servicesDefaultSetting = InvestorServicesAclDbAccessor.GetPersonasServicesDefaultSetting(feature, userID, category, personaIDs);
      return servicesDefaultSetting;
    }

    [PgReady]
    public static InvestorServiceAclInfo.InvestorServicesDefaultSetting GetUserSpecificServicesDefaultSetting(
      AclFeature feature,
      string userID,
      string category,
      int[] personaIDs)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        InvestorServiceAclInfo.InvestorServicesDefaultSetting servicesDefaultSetting = InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified;
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select * from [Acl_InvestorServicesDefault_User] where UserID = @userid and FeatureID = " + SQL.Encode((object) (int) feature) + " and Category = " + SQL.Encode((object) category));
        DbCommandParameter parameter = new DbCommandParameter("userid", (object) userID.TrimEnd(), DbType.AnsiString);
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute(parameter);
        if (dataRowCollection != null && dataRowCollection.Count > 0)
          servicesDefaultSetting = (InvestorServiceAclInfo.InvestorServicesDefaultSetting) SQL.DecodeInt(dataRowCollection[0]["Access"]);
        return servicesDefaultSetting;
      }
      InvestorServiceAclInfo.InvestorServicesDefaultSetting servicesDefaultSetting1 = InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from [Acl_InvestorServicesDefault_User] where UserID = " + SQL.Encode((object) userID) + " and FeatureID = " + SQL.Encode((object) (int) feature) + " and Category = " + SQL.Encode((object) category));
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      if (dataRowCollection1 != null && dataRowCollection1.Count > 0)
        servicesDefaultSetting1 = (InvestorServiceAclInfo.InvestorServicesDefaultSetting) dataRowCollection1[0]["Access"];
      return servicesDefaultSetting1;
    }

    public static InvestorServiceAclInfo.InvestorServicesDefaultSetting GetPersonasServicesDefaultSetting(
      AclFeature feature,
      string userID,
      string category,
      int[] personaIDs)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      InvestorServiceAclInfo.InvestorServicesDefaultSetting servicesDefaultSetting = InvestorServiceAclInfo.InvestorServicesDefaultSetting.All;
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
      dbQueryBuilder.AppendLine("Select * from [Acl_InvestorServicesDefault] where personaID in (" + SQL.EncodeArray((Array) personaIDs) + ")  and FeatureID = " + SQL.Encode((object) (int) feature) + " and Category = " + SQL.Encode((object) category) + " Order by Access DESC");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
        servicesDefaultSetting = (InvestorServiceAclInfo.InvestorServicesDefaultSetting) dataRowCollection[0]["Access"];
      return servicesDefaultSetting;
    }

    private static Hashtable GetPersonaPermissions(
      InvestorServiceAclInfo[] services,
      AclFeature feature,
      int personaID,
      string category,
      InvestorServiceAclInfo.InvestorServicesDefaultSetting defaultValue)
    {
      if (personaID == 1)
        InvestorServicesAclDbAccessor.updateAdminPersona(feature, category);
      Hashtable personaPermissions = new Hashtable();
      switch (defaultValue)
      {
        case InvestorServiceAclInfo.InvestorServicesDefaultSetting.None:
          foreach (InvestorServiceAclInfo service in services)
            service.PersonaAccess = AclResourceAccess.ReadOnly;
          break;
        case InvestorServiceAclInfo.InvestorServicesDefaultSetting.All:
          foreach (InvestorServiceAclInfo service in services)
            service.PersonaAccess = AclResourceAccess.ReadWrite;
          break;
        default:
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          dbQueryBuilder.AppendLine("select * from [Acl_InvestorServices] where PersonaID = " + SQL.Encode((object) personaID) + " and FeatureID = " + SQL.Encode((object) (int) feature) + " and Category = " + SQL.Encode((object) category));
          foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          {
            foreach (InvestorServiceAclInfo service in services)
            {
              if (service.ProviderCompanyCode == string.Concat(dataRow["ProviderCompanyCode"]) && service.PersonaAccess != AclResourceAccess.ReadWrite)
                service.PersonaAccess = (byte) dataRow["Access"] == (byte) 1 ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
            }
          }
          foreach (InvestorServiceAclInfo service in services)
          {
            if (service.PersonaAccess == AclResourceAccess.None)
            {
              switch (defaultValue)
              {
                case InvestorServiceAclInfo.InvestorServicesDefaultSetting.None:
                  service.PersonaAccess = AclResourceAccess.ReadOnly;
                  continue;
                case InvestorServiceAclInfo.InvestorServicesDefaultSetting.Custom:
                case InvestorServiceAclInfo.InvestorServicesDefaultSetting.All:
                  service.PersonaAccess = AclResourceAccess.ReadWrite;
                  continue;
                default:
                  continue;
              }
            }
          }
          break;
      }
      foreach (InvestorServiceAclInfo service in services)
        personaPermissions.Add((object) service.ProviderCompanyCode, (object) service);
      return personaPermissions;
    }

    [PgReady]
    public static InvestorServiceAclInfo[] GetPermissions(
      AclFeature feature,
      string userID,
      string category,
      int[] personaIDs,
      InvestorServiceAclInfo[] availableServices)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        InvestorServiceAclInfo.InvestorServicesDefaultSetting servicesDefaultSetting1 = InvestorServicesAclDbAccessor.GetUserSpecificServicesDefaultSetting(feature, userID, category, personaIDs);
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select * from [Acl_InvestorServices_User] where UserID = @userid and FeatureID = " + SQL.Encode((object) (int) feature) + " and Category = " + SQL.Encode((object) category));
        DbCommandParameter parameter = new DbCommandParameter("userid", (object) userID.TrimEnd(), DbType.AnsiString);
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute(parameter);
        switch (servicesDefaultSetting1)
        {
          case InvestorServiceAclInfo.InvestorServicesDefaultSetting.None:
            foreach (InvestorServiceAclInfo availableService in availableServices)
              availableService.CustomAccess = AclResourceAccess.ReadOnly;
            break;
          case InvestorServiceAclInfo.InvestorServicesDefaultSetting.All:
            foreach (InvestorServiceAclInfo availableService in availableServices)
              availableService.CustomAccess = AclResourceAccess.ReadWrite;
            break;
          default:
            IEnumerator enumerator = dataRowCollection.GetEnumerator();
            try
            {
              while (enumerator.MoveNext())
              {
                DataRow current = (DataRow) enumerator.Current;
                foreach (InvestorServiceAclInfo availableService in availableServices)
                {
                  if (availableService.ProviderCompanyCode == string.Concat(current["ProviderCompanyCode"]))
                  {
                    availableService.CustomAccess = SQL.DecodeInt(current["Access"]) == 1 ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
                    break;
                  }
                }
              }
              break;
            }
            finally
            {
              if (enumerator is IDisposable disposable)
                disposable.Dispose();
            }
        }
        foreach (int personaId in personaIDs)
        {
          InvestorServiceAclInfo.InvestorServicesDefaultSetting servicesDefaultSetting2 = InvestorServicesAclDbAccessor.GetServicesDefaultSetting(feature, personaId, category);
          Hashtable personaPermissions = InvestorServicesAclDbAccessor.GetPersonaPermissions(availableServices, feature, personaId, category, servicesDefaultSetting2);
          foreach (InvestorServiceAclInfo availableService in availableServices)
          {
            if (availableService.PersonaAccess != AclResourceAccess.ReadWrite && personaPermissions[(object) availableService.ProviderCompanyCode] != null)
              availableService.PersonaAccess = ((InvestorServiceAclInfo) personaPermissions[(object) availableService.ProviderCompanyCode]).PersonaAccess;
          }
        }
        return availableServices;
      }
      InvestorServiceAclInfo.InvestorServicesDefaultSetting servicesDefaultSetting3 = InvestorServicesAclDbAccessor.GetUserSpecificServicesDefaultSetting(feature, userID, category, personaIDs);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from [Acl_InvestorServices_User] where UserID = " + SQL.Encode((object) userID) + " and FeatureID = " + SQL.Encode((object) (int) feature) + " and Category = " + SQL.Encode((object) category));
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      switch (servicesDefaultSetting3)
      {
        case InvestorServiceAclInfo.InvestorServicesDefaultSetting.None:
          foreach (InvestorServiceAclInfo availableService in availableServices)
            availableService.CustomAccess = AclResourceAccess.ReadOnly;
          break;
        case InvestorServiceAclInfo.InvestorServicesDefaultSetting.All:
          foreach (InvestorServiceAclInfo availableService in availableServices)
            availableService.CustomAccess = AclResourceAccess.ReadWrite;
          break;
        default:
          IEnumerator enumerator1 = dataRowCollection1.GetEnumerator();
          try
          {
            while (enumerator1.MoveNext())
            {
              DataRow current = (DataRow) enumerator1.Current;
              foreach (InvestorServiceAclInfo availableService in availableServices)
              {
                if (availableService.ProviderCompanyCode == string.Concat(current["ProviderCompanyCode"]))
                {
                  availableService.CustomAccess = (byte) current["Access"] == (byte) 1 ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
                  break;
                }
              }
            }
            break;
          }
          finally
          {
            if (enumerator1 is IDisposable disposable)
              disposable.Dispose();
          }
      }
      foreach (int personaId in personaIDs)
      {
        InvestorServiceAclInfo.InvestorServicesDefaultSetting servicesDefaultSetting4 = InvestorServicesAclDbAccessor.GetServicesDefaultSetting(feature, personaId, category);
        InvestorServiceAclInfo[] array = ((IEnumerable<InvestorServiceAclInfo>) availableServices).Where<InvestorServiceAclInfo>((System.Func<InvestorServiceAclInfo, bool>) (s => s.PersonaAccess != AclResourceAccess.ReadWrite)).ToArray<InvestorServiceAclInfo>();
        Hashtable personaPermissions = InvestorServicesAclDbAccessor.GetPersonaPermissions(array, feature, personaId, category, servicesDefaultSetting4);
        foreach (InvestorServiceAclInfo investorServiceAclInfo in array)
        {
          if (investorServiceAclInfo.PersonaAccess != AclResourceAccess.ReadWrite && personaPermissions[(object) investorServiceAclInfo.ProviderCompanyCode] != null)
            investorServiceAclInfo.PersonaAccess = ((InvestorServiceAclInfo) personaPermissions[(object) investorServiceAclInfo.ProviderCompanyCode]).PersonaAccess;
        }
      }
      return availableServices;
    }

    public static InvestorServiceAclInfo[] GetPermissions(
      AclFeature[] features,
      string userID,
      int[] personaIDs,
      InvestorServiceAclInfo[] allAvailableServices)
    {
      List<InvestorServiceAclInfo> investorServiceAclInfoList = new List<InvestorServiceAclInfo>();
      foreach (AclFeature feature1 in features)
      {
        AclFeature feature = feature1;
        InvestorServiceAclInfo[] array = ((IEnumerable<InvestorServiceAclInfo>) allAvailableServices).Where<InvestorServiceAclInfo>((System.Func<InvestorServiceAclInfo, bool>) (a => (AclFeature) a.FeatureID == feature)).ToArray<InvestorServiceAclInfo>();
        if (array != null && ((IEnumerable<InvestorServiceAclInfo>) array).Count<InvestorServiceAclInfo>() != 0)
        {
          string investorCategory = array[0].InvestorCategory;
          investorServiceAclInfoList.AddRange((IEnumerable<InvestorServiceAclInfo>) InvestorServicesAclDbAccessor.GetPermissions(feature, userID, investorCategory, personaIDs, array));
        }
      }
      return investorServiceAclInfoList.ToArray();
    }

    public static void SetDefaultValue(
      AclFeature feature,
      string userid,
      string category,
      InvestorServiceAclInfo.InvestorServicesDefaultSetting defaultValue)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete [Acl_InvestorServicesDefault_User] where FeatureID = " + (object) (int) feature + " and UserID = " + SQL.Encode((object) userid) + " and Category = " + SQL.Encode((object) category));
      if (defaultValue != InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified)
      {
        dbQueryBuilder.AppendLine("Insert into [Acl_InvestorServicesDefault_User] (FeatureID, UserID, Category,  Access) Values( " + (object) (int) feature + ", " + SQL.Encode((object) userid) + ", " + SQL.Encode((object) category) + ", " + (object) (int) defaultValue + ")");
        int num = defaultValue == InvestorServiceAclInfo.InvestorServicesDefaultSetting.None ? 0 : 1;
        dbQueryBuilder.AppendLine("If Exists (Select 1 from Acl_Features_User where featureID = " + (object) (int) feature + " and userid = " + SQL.Encode((object) userid) + ")");
        dbQueryBuilder.AppendLine("Update Acl_Features_User set access = " + (object) num + " where featureID = " + (object) (int) feature + " and userid = " + SQL.Encode((object) userid));
        dbQueryBuilder.AppendLine("Else");
        dbQueryBuilder.AppendLine("Insert into Acl_Features_User (featureID, userid, access) Values( " + (object) (int) feature + ", " + SQL.Encode((object) userid) + ", " + (object) num + ")");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetPermissions(
      AclFeature feature,
      InvestorServiceAclInfo[] InvestorServiceAclInfoList,
      string userid,
      string category,
      InvestorServiceAclInfo.InvestorServicesDefaultSetting defaultValue)
    {
      if (defaultValue == InvestorServiceAclInfo.InvestorServicesDefaultSetting.All)
      {
        if (InvestorServiceAclInfoList == null || ((IEnumerable<InvestorServiceAclInfo>) InvestorServiceAclInfoList).Count<InvestorServiceAclInfo>() == 0)
          return;
        InvestorServiceAclInfo investorServiceAclInfo = ((IEnumerable<InvestorServiceAclInfo>) InvestorServiceAclInfoList).First<InvestorServiceAclInfo>();
        investorServiceAclInfo.ProviderCompanyCode = "*";
        investorServiceAclInfo.CustomAccess = AclResourceAccess.ReadOnly;
        InvestorServiceAclInfoList = new InvestorServiceAclInfo[1]
        {
          investorServiceAclInfo
        };
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete [Acl_InvestorServices_User] where FeatureID = " + (object) (int) feature + " and UserID = " + SQL.Encode((object) userid) + " and Category = " + SQL.Encode((object) category));
      foreach (InvestorServiceAclInfo investorServiceAclInfo in InvestorServiceAclInfoList)
      {
        if (investorServiceAclInfo.CustomAccess != AclResourceAccess.None)
          dbQueryBuilder.AppendLine("Insert into [Acl_InvestorServices_User] (ProviderCompanyCode, FeatureID, UserID, Category, Access) Values( " + SQL.Encode((object) investorServiceAclInfo.ProviderCompanyCode) + ", " + (object) (int) feature + ", " + SQL.Encode((object) userid) + ", " + SQL.Encode((object) category) + ", " + (object) (investorServiceAclInfo.CustomAccess == AclResourceAccess.ReadWrite ? 1 : 0) + ")");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetDefaultValue(
      AclFeature feature,
      int personaID,
      string category,
      InvestorServiceAclInfo.InvestorServicesDefaultSetting defaultValue)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete [Acl_InvestorServicesDefault] where FeatureID = " + (object) (int) feature + " and PersonaID = " + SQL.Encode((object) personaID) + " and Category = " + SQL.Encode((object) category));
      dbQueryBuilder.AppendLine("Insert into [Acl_InvestorServicesDefault] (FeatureID, PersonaID, Category, Access) Values( " + (object) (int) feature + ", " + SQL.Encode((object) personaID) + ", " + SQL.Encode((object) category) + ", " + (object) (int) defaultValue + ")");
      int num = defaultValue == InvestorServiceAclInfo.InvestorServicesDefaultSetting.None ? 0 : 1;
      dbQueryBuilder.AppendLine("If Exists (Select 1 from Acl_Features where featureID = " + (object) (int) feature + " and personaID = " + SQL.Encode((object) personaID) + ")");
      dbQueryBuilder.AppendLine("Update Acl_Features set access = " + (object) num + " where featureID = " + (object) (int) feature + " and personaID = " + SQL.Encode((object) personaID));
      dbQueryBuilder.AppendLine("Else");
      dbQueryBuilder.AppendLine("Insert into Acl_Features (featureID, personaID, access) Values( " + (object) (int) feature + ", " + SQL.Encode((object) personaID) + ", " + (object) num + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetPermissions(
      AclFeature feature,
      InvestorServiceAclInfo[] InvestorServiceAclInfoList,
      int personaID,
      string category,
      InvestorServiceAclInfo.InvestorServicesDefaultSetting defaultValue)
    {
      if (defaultValue == InvestorServiceAclInfo.InvestorServicesDefaultSetting.All)
      {
        if (InvestorServiceAclInfoList == null || ((IEnumerable<InvestorServiceAclInfo>) InvestorServiceAclInfoList).Count<InvestorServiceAclInfo>() == 0)
          return;
        InvestorServiceAclInfo investorServiceAclInfo = ((IEnumerable<InvestorServiceAclInfo>) InvestorServiceAclInfoList).First<InvestorServiceAclInfo>();
        investorServiceAclInfo.ProviderCompanyCode = "*";
        InvestorServiceAclInfoList = new InvestorServiceAclInfo[1]
        {
          investorServiceAclInfo
        };
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete [Acl_InvestorServices] where PersonaID = " + (object) personaID + " and FeatureID = " + (object) (int) feature);
      foreach (InvestorServiceAclInfo investorServiceAclInfo in InvestorServiceAclInfoList)
        dbQueryBuilder.AppendLine("Insert [Acl_InvestorServices] (ProviderCompanyCode, FeatureID, PersonaID, Category, Access) Values( " + SQL.Encode((object) investorServiceAclInfo.ProviderCompanyCode) + ", " + (object) (int) feature + ", " + (object) personaID + ", " + SQL.Encode((object) category) + "," + (investorServiceAclInfo.PersonaAccess == AclResourceAccess.ReadWrite ? (object) "1" : (object) "0") + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DuplicateACLServices(
      AclFeature feature,
      int sourcePersonaID,
      int desPersonaID,
      string category,
      InvestorServiceAclInfo[] availableServices)
    {
      if (sourcePersonaID == 1)
        InvestorServicesAclDbAccessor.GetPermissions(feature, sourcePersonaID, category, availableServices);
      string str1 = "";
      string str2 = "";
      InvestorServiceAclInfo.InvestorServicesDefaultSetting servicesDefaultSetting = InvestorServicesAclDbAccessor.GetServicesDefaultSetting(feature, sourcePersonaID, category);
      InvestorServicesAclDbAccessor.SetDefaultValue(feature, desPersonaID, category, servicesDefaultSetting);
      if (servicesDefaultSetting == InvestorServiceAclInfo.InvestorServicesDefaultSetting.All)
      {
        if (availableServices == null || ((IEnumerable<InvestorServiceAclInfo>) availableServices).Count<InvestorServiceAclInfo>() == 0)
          return;
        InvestorServiceAclInfo investorServiceAclInfo = ((IEnumerable<InvestorServiceAclInfo>) availableServices).First<InvestorServiceAclInfo>();
        investorServiceAclInfo.ProviderCompanyCode = "*";
        availableServices = new InvestorServiceAclInfo[1]
        {
          investorServiceAclInfo
        };
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from [Acl_InvestorServices] where featureID = " + SQL.Encode((object) (int) feature) + " and personaID = " + (object) sourcePersonaID + " and category = " + SQL.Encode((object) category));
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      dbQueryBuilder.Reset();
      DataColumnCollection columns = dataTable.Columns;
      if (dataTable == null || dataTable.Rows.Count <= 0)
        return;
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        string str3 = "Insert into [Acl_InvestorServices] (";
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

    private static void updateAdminPersona(AclFeature feature, string category)
    {
      InvestorServicesAclDbAccessor.SetDefaultValue(feature, 1, category, InvestorServiceAclInfo.InvestorServicesDefaultSetting.All);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete [Acl_InvestorServices] where PersonaID = " + (object) 1);
      dbQueryBuilder.AppendLine("Insert [Acl_InvestorServices] (ProviderCompanyCode, FeatureID, PersonaID, Category, Access) Values( '*' , " + (object) (int) feature + ", " + (object) 1 + ", " + SQL.Encode((object) category) + ", 1)");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static Dictionary<string, EBSInvestorServicesAclInfo> GetAllUserSpecificDefaultSettings(
      string userID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("select {0}, {1}, {2} from {3} where {4} = {5}", (object) "FeatureID", (object) "Access", (object) "Category", (object) "[Acl_InvestorServicesDefault_User]", (object) nameof (userID), (object) SQL.Encode((object) userID)));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null)
        return new Dictionary<string, EBSInvestorServicesAclInfo>();
      Dictionary<string, EBSInvestorServicesAclInfo> specificDefaultSettings = new Dictionary<string, EBSInvestorServicesAclInfo>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        AclFeature feature = (AclFeature) dataRow["FeatureID"];
        string key = string.Concat(dataRow["Category"]);
        InvestorServiceAclInfo.InvestorServicesDefaultSetting servicesDefaultSetting = (InvestorServiceAclInfo.InvestorServicesDefaultSetting) dataRow["Access"];
        EBSInvestorServicesAclInfo investorServicesAclInfo = new EBSInvestorServicesAclInfo(feature)
        {
          DefaultAccess = servicesDefaultSetting,
          Category = key
        };
        specificDefaultSettings[key] = investorServicesAclInfo;
      }
      return specificDefaultSettings;
    }

    public static EBSInvestorServicesAclInfo GetUserSpecificServiceDefaultSettings(
      AclFeature feature,
      string userID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("select {0}, {1} from {2} where {3} = {4} and {5} = {6}", (object) "Access", (object) "Category", (object) "[Acl_InvestorServicesDefault_User]", (object) nameof (userID), (object) SQL.Encode((object) userID), (object) "FeatureID", (object) SQL.Encode((object) (int) feature)));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null || dataRowCollection.Count <= 0)
        return new EBSInvestorServicesAclInfo(feature);
      return new EBSInvestorServicesAclInfo(feature)
      {
        DefaultAccess = (InvestorServiceAclInfo.InvestorServicesDefaultSetting) dataRowCollection[0]["Access"],
        Category = string.Concat(dataRowCollection[0]["Category"])
      };
    }

    public static Dictionary<string, EBSInvestorServicesAclInfo> GetAllCategoriesForAdmin()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("select distinct {0}, {1} from {2}", (object) "Category", (object) "FeatureID", (object) "[Acl_InvestorServicesDefault]"));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null)
        return new Dictionary<string, EBSInvestorServicesAclInfo>();
      Dictionary<string, EBSInvestorServicesAclInfo> categoriesForAdmin = new Dictionary<string, EBSInvestorServicesAclInfo>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        string key = string.Concat(dataRow["Category"]);
        AclFeature feature = (AclFeature) dataRow["FeatureID"];
        categoriesForAdmin[key] = new EBSInvestorServicesAclInfo(feature)
        {
          DefaultAccess = InvestorServiceAclInfo.InvestorServicesDefaultSetting.All,
          Category = key
        };
      }
      return categoriesForAdmin;
    }

    public static Dictionary<string, EBSInvestorServicesAclInfo> GetAllPersonasDefaultAccessSettings(
      int[] personaIDs,
      bool skipAdminCheck)
    {
      Dictionary<string, EBSInvestorServicesAclInfo> categoriesForAdmin = InvestorServicesAclDbAccessor.GetAllCategoriesForAdmin();
      if (!skipAdminCheck)
      {
        foreach (int personaId in personaIDs)
        {
          switch (personaId)
          {
            case 0:
            case 1:
              return categoriesForAdmin;
            default:
              continue;
          }
        }
      }
      string columnName = "top_access";
      string text = string.Format("Select {0}, {1}, max({2}) as {3} from {4} where {5} in ({6}) and {2} < {7} group by {0}, {1}", (object) "Category", (object) "FeatureID", (object) "Access", (object) columnName, (object) "[Acl_InvestorServicesDefault]", (object) "PersonaID", (object) SQL.EncodeArray((Array) personaIDs), (object) SQL.Encode((object) 3));
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(text);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null)
        return categoriesForAdmin;
      Dictionary<string, EBSInvestorServicesAclInfo> defaultAccessSettings = categoriesForAdmin;
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        string key = string.Concat(dataRow["Category"]);
        InvestorServiceAclInfo.InvestorServicesDefaultSetting servicesDefaultSetting = (InvestorServiceAclInfo.InvestorServicesDefaultSetting) dataRow[columnName];
        AclFeature feature = (AclFeature) dataRow["FeatureID"];
        defaultAccessSettings[key] = new EBSInvestorServicesAclInfo(feature)
        {
          Category = key,
          DefaultAccess = servicesDefaultSetting
        };
      }
      return defaultAccessSettings;
    }

    public static EBSInvestorServicesAclInfo GetMaxPersonasDefaultAccessSettings(
      AclFeature investorFeature,
      int[] personaIDs,
      bool skipAdminCheck)
    {
      EBSInvestorServicesAclInfo defaultAccessSettings = new EBSInvestorServicesAclInfo(investorFeature);
      if (!skipAdminCheck)
      {
        foreach (int personaId in personaIDs)
        {
          switch (personaId)
          {
            case 0:
            case 1:
              defaultAccessSettings.DefaultAccess = InvestorServiceAclInfo.InvestorServicesDefaultSetting.All;
              return defaultAccessSettings;
            default:
              continue;
          }
        }
      }
      HashSet<string> stringSet = new HashSet<string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string columnName = "top_access";
      string text = string.Format("Select max({0}) as {1} from {2} where {3} in ({4}) and {5} = {6} and {0} < {7}", (object) "Access", (object) columnName, (object) "[Acl_InvestorServicesDefault]", (object) "PersonaID", (object) SQL.EncodeArray((Array) personaIDs), (object) "FeatureID", (object) SQL.Encode((object) (int) investorFeature), (object) SQL.Encode((object) 3));
      dbQueryBuilder.AppendLine(text);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
        defaultAccessSettings = new EBSInvestorServicesAclInfo(investorFeature)
        {
          DefaultAccess = (InvestorServiceAclInfo.InvestorServicesDefaultSetting) dataRowCollection[0][columnName],
          Category = string.Concat(dataRowCollection[0]["Category"])
        };
      return defaultAccessSettings;
    }

    private static void AddProviderCompanyCodeInfo(
      bool access,
      string providerCode,
      EBSInvestorServicesAclInfo customAccessInfo)
    {
      if (string.IsNullOrEmpty(providerCode))
        return;
      bool flag = false;
      if (customAccessInfo.AccessToProviders.ContainsKey(providerCode))
        flag = customAccessInfo.AccessToProviders[providerCode];
      customAccessInfo.AccessToProviders[providerCode] = flag | access;
    }

    public static void GetAllUserSpecificProvidersAccess(
      string userId,
      Dictionary<string, EBSInvestorServicesAclInfo> knownInfo)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("select {0}, {1}, {2}, {3} from {4} where {5} = {6}", (object) "Access", (object) "FeatureID", (object) "Category", (object) "ProviderCompanyCode", (object) "[Acl_InvestorServices_User]", (object) "userID", (object) SQL.Encode((object) userId)));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null)
        return;
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        AclFeature feature = (AclFeature) dataRow["FeatureID"];
        string key = string.Concat(dataRow["Category"]);
        EBSInvestorServicesAclInfo customAccessInfo;
        if (knownInfo.ContainsKey(key))
        {
          customAccessInfo = knownInfo[key];
        }
        else
        {
          customAccessInfo = new EBSInvestorServicesAclInfo(feature)
          {
            DefaultAccess = InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified,
            Category = key
          };
          knownInfo[key] = customAccessInfo;
        }
        InvestorServicesAclDbAccessor.AddProviderCompanyCodeInfo((byte) dataRow["Access"] == (byte) 1, string.Concat(dataRow["ProviderCompanyCode"]), customAccessInfo);
      }
    }

    public static void GetUserSpecificCustomInfo(
      AclFeature investorFeature,
      string userId,
      ref EBSInvestorServicesAclInfo infoWithCustomAccess)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("select {0}, {1} from {2} where {3} = {4} and {5} = {6}", (object) "Access", (object) "ProviderCompanyCode", (object) "[Acl_InvestorServices_User]", (object) "userID", (object) SQL.Encode((object) userId), (object) "FeatureID", (object) SQL.Encode((object) (int) investorFeature)));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null)
        return;
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        InvestorServicesAclDbAccessor.AddProviderCompanyCodeInfo((byte) dataRow["Access"] == (byte) 1, string.Concat(dataRow["ProviderCompanyCode"]), infoWithCustomAccess);
    }

    public static void GetMaxCustomInfoForPersonas(
      AclFeature investorFeature,
      int[] personaIds,
      ref EBSInvestorServicesAclInfo infoWithCustomAccess)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("select {0}, {1} from {2} where {3} in ({4}) and {5} = {6}", (object) "ProviderCompanyCode", (object) "Access", (object) "[Acl_InvestorServices]", (object) "PersonaID", (object) SQL.EncodeArray((Array) personaIds), (object) "FeatureID", (object) SQL.Encode((object) (int) investorFeature)));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null)
        return;
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        InvestorServicesAclDbAccessor.AddProviderCompanyCodeInfo((byte) dataRow["Access"] == (byte) 1, string.Concat(dataRow["ProviderCompanyCode"]), infoWithCustomAccess);
    }

    public static void GetAllProvidersAccessForPersonas(
      int[] personaIds,
      Dictionary<string, EBSInvestorServicesAclInfo> knownAccessInfo)
    {
      string columnName = "top_access";
      string text = string.Format("select {0}, {1}, {2}, max({3}) as {4} from {5} where {6} in ({7}) and {3} < {8} group by {0}, {1}, {2}", (object) "Category", (object) "ProviderCompanyCode", (object) "FeatureID", (object) "Access", (object) columnName, (object) "[Acl_InvestorServices]", (object) "PersonaID", (object) SQL.EncodeArray((Array) personaIds), (object) SQL.Encode((object) 3));
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(text);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null)
        return;
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        AclFeature feature = (AclFeature) dataRow["FeatureID"];
        string key = string.Concat(dataRow["Category"]);
        EBSInvestorServicesAclInfo customAccessInfo;
        if (knownAccessInfo.ContainsKey(key))
        {
          customAccessInfo = knownAccessInfo[key];
        }
        else
        {
          customAccessInfo = new EBSInvestorServicesAclInfo(feature)
          {
            DefaultAccess = InvestorServiceAclInfo.InvestorServicesDefaultSetting.NotSpecified,
            Category = key
          };
          knownAccessInfo[key] = customAccessInfo;
        }
        InvestorServicesAclDbAccessor.AddProviderCompanyCodeInfo((byte) dataRow[columnName] == (byte) 1, string.Concat(dataRow["ProviderCompanyCode"]), customAccessInfo);
      }
    }
  }
}
