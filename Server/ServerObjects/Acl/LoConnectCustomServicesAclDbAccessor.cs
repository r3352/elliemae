// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Acl.LoConnectCustomServicesAclDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Acl
{
  public class LoConnectCustomServicesAclDbAccessor
  {
    private const string tableName = "[Acl_LoConnectCustomServices]�";
    private const string tableName_User = "[Acl_LoConnectCustomServices_User]�";
    private const string customtools_Table = "[CustomTools]�";

    public static void SetPermission(LoConnectCustomServiceInfo[] services, string userid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (LoConnectCustomServiceInfo service in services)
      {
        string customEntityId = service.CustomEntityId;
        bool access = service.Access;
        if (dbQueryBuilder.Length > 0)
          dbQueryBuilder.AppendLine("");
        string str = " where CustomEntityId = " + SQL.Encode((object) customEntityId) + " and userID = " + SQL.Encode((object) userid);
        dbQueryBuilder.AppendLine("if exists (select * from [Acl_LoConnectCustomServices_User]" + str + ")");
        dbQueryBuilder.AppendLine("update [Acl_LoConnectCustomServices_User] set access = " + (access ? "1" : "0") + " " + str);
        dbQueryBuilder.AppendLine("else");
        dbQueryBuilder.AppendLine("insert into [Acl_LoConnectCustomServices_User] (CustomEntityId, ServiceType, UserID, Access) values (" + SQL.Encode((object) customEntityId) + ", " + (object) (int) service.ServiceType + ", " + SQL.Encode((object) userid) + ", " + (access ? (object) "1" : (object) "0") + ")");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetPermission(LoConnectCustomServiceInfo[] services, int personaID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (LoConnectCustomServiceInfo service in services)
      {
        string customEntityId = service.CustomEntityId;
        bool access = service.Access;
        if (dbQueryBuilder.Length > 0)
          dbQueryBuilder.AppendLine("");
        string str = "where CustomEntityId = " + SQL.Encode((object) customEntityId) + " and personaID = " + (object) personaID;
        dbQueryBuilder.AppendLine("if exists (select * from Acl_LoConnectCustomServices " + str + ")");
        dbQueryBuilder.AppendLine("update Acl_LoConnectCustomServices set access = " + (access ? "1" : "0") + " " + str);
        dbQueryBuilder.AppendLine("else");
        dbQueryBuilder.AppendLine("insert into [Acl_LoConnectCustomServices] (CustomEntityId, ServiceType, PersonaID, Access) values (" + SQL.Encode((object) customEntityId) + ", " + (object) (int) service.ServiceType + ", " + (object) personaID + ", " + (access ? (object) "1" : (object) "0") + ")");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static Hashtable GetPermissions(LoConnectCustomServiceInfo service, int personaID)
    {
      return LoConnectCustomServicesAclDbAccessor.GetPermissions(service, new int[1]
      {
        personaID
      });
    }

    public static Hashtable GetUserPermissionByPersonas(int[] personaIDs)
    {
      Hashtable permissionByPersonas = new Hashtable();
      if (personaIDs.Length == 0)
        return permissionByPersonas;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from [Acl_LoConnectCustomServices] where personaID in (" + SQL.EncodeArray((Array) personaIDs) + ")");
      foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
      {
        bool flag = (byte) dataRow["access"] == (byte) 1;
        string key = dataRow["CustomEntityId"].ToString();
        if (permissionByPersonas.ContainsKey((object) key))
        {
          if (!(bool) permissionByPersonas[(object) key])
            permissionByPersonas[(object) key] = (object) flag;
        }
        else
          permissionByPersonas.Add((object) key, (object) flag);
      }
      return permissionByPersonas;
    }

    public static Hashtable GetPermissions(LoConnectCustomServiceInfo service, int[] personaIDs)
    {
      if (personaIDs.Length == 0)
        return new Hashtable();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from [Acl_LoConnectCustomServices] where CustomEntityId = " + SQL.Encode((object) service.CustomEntityId) + " and personaID in (" + SQL.EncodeArray((Array) personaIDs) + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      Hashtable permissions = new Hashtable();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        int key = (int) dataRow["personaID"];
        bool flag = (byte) dataRow["access"] == (byte) 1;
        if (permissions[(object) key] != null)
          permissions[(object) key] = (object) ((bool) permissions[(object) key] | flag);
        else
          permissions[(object) key] = (object) flag;
      }
      return permissions;
    }

    public static Hashtable GetPermissions(LoConnectCustomServiceInfo[] services, int personaID)
    {
      if (services.Length == 0)
        return new Hashtable();
      Hashtable permissions = new Hashtable();
      if (personaID == 1 || personaID == 0)
      {
        foreach (LoConnectCustomServiceInfo service in services)
          permissions[(object) service.CustomEntityId] = (object) true;
        return permissions;
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string[] data = new string[services.Length];
      for (int index = 0; index < services.Length; ++index)
        data[index] = services[index].CustomEntityId;
      dbQueryBuilder.AppendLine("select CustomEntityId, Access from [Acl_LoConnectCustomServices] where personaID = " + (object) personaID + " and CustomEntityId in (" + SQL.EncodeArray((Array) data) + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      foreach (LoConnectCustomServiceInfo service in services)
        permissions[(object) service.CustomEntityId] = (object) false;
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        string key = (string) dataRow["CustomEntityId"];
        bool flag = (byte) dataRow["access"] == (byte) 1;
        permissions[(object) key] = (object) flag;
      }
      return permissions;
    }

    public static LoConnectCustomServiceInfo[] GetUserAccessibleServicesByPersonas(int[] personaIDs)
    {
      ArrayList arrayList = new ArrayList();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (personaIDs.Length == 0)
        return new LoConnectCustomServiceInfo[0];
      dbQueryBuilder.AppendLine(" SELECT LOCS.CustomEntityId, CASE WHEN LOCS.ServiceType =1 THEN 1 ELSE CASE WHEN CT.IsGlobal = 0 THEN 2 ELSE 3 END END AS ServiceType FROM [Acl_LoConnectCustomServices] LOCS LEFT JOIN [CustomTools] CT ON CT.CustomToolId = LOCS.CustomEntityId  WHERE LOCS.Access = 1 AND LOCS.PersonaID IN (" + SQL.EncodeArray((Array) personaIDs) + ") ");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection.Count == 0)
        return new LoConnectCustomServiceInfo[0];
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        string customentityId = Convert.ToString(dataRow["CustomEntityId"]);
        LoServiceType serviceType = (LoServiceType) int.Parse(dataRow["ServiceType"].ToString());
        arrayList.Add((object) new LoConnectCustomServiceInfo(customentityId, serviceType, true));
      }
      return (LoConnectCustomServiceInfo[]) arrayList.ToArray(typeof (LoConnectCustomServiceInfo));
    }

    public static Hashtable GetPermissions(LoConnectCustomServiceInfo service, string userid)
    {
      return LoConnectCustomServicesAclDbAccessor.GetPermissions(new LoConnectCustomServiceInfo[1]
      {
        service
      }, userid);
    }

    public static Hashtable GetPermissions(LoConnectCustomServiceInfo[] services, string userid)
    {
      if (services == null || services.Length == 0)
        return (Hashtable) null;
      Hashtable permissions = new Hashtable();
      UserInfo userById = User.GetUserById(userid);
      if (userById.IsAdministrator() || userById.IsSuperAdministrator())
      {
        foreach (LoConnectCustomServiceInfo service in services)
          permissions[(object) service.CustomEntityId] = (object) true;
        return permissions;
      }
      string[] data = new string[services.Length];
      for (int index = 0; index < services.Length; ++index)
        data[index] = services[index].CustomEntityId;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select CustomEntityId, access from [Acl_LoConnectCustomServices_User] where userid = " + SQL.Encode((object) userid) + " and CustomEntityID in (" + SQL.EncodeArray((Array) data) + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null)
        return (Hashtable) null;
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        string key = (string) dataRow["CustomEntityId"];
        bool flag = (byte) dataRow["access"] == (byte) 1;
        permissions[(object) key] = (object) flag;
      }
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        string key = (string) dataRow["CustomEntityId"];
        bool flag = (byte) dataRow["access"] == (byte) 1;
        permissions[(object) key] = (object) flag;
      }
      return permissions;
    }

    public static LoConnectCustomServiceInfo[] GetUserAccessibleServicesByUserInfo(UserInfo user)
    {
      List<LoConnectCustomServiceInfo> customServiceInfoList = new List<LoConnectCustomServiceInfo>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (user == (UserInfo) null)
        return customServiceInfoList.ToArray();
      int[] personaIds = user.GetPersonaIDs();
      dbQueryBuilder.AppendLine(" SELECT LOCUS.CustomEntityId, LOCUS.Access, CASE WHEN LOCUS.ServiceType = 1 THEN 1 ELSE CASE WHEN CT.IsGlobal = 0 THEN 2 ELSE 3 END END AS ServiceType FROM [Acl_LoConnectCustomServices_User] LOCUS LEFT JOIN [CustomTools] CT ON CT.CustomToolId = LOCUS.CustomEntityId WHERE LOCUS.UserId IN (" + SQL.Encode((object) user.Userid) + ")  UNION ALL SELECT DISTINCT LOCS.CustomEntityId, LOCS.Access, CASE WHEN LOCS.ServiceType = 1 THEN 1 ELSE CASE WHEN CT.IsGlobal = 0 THEN 2 ELSE 3 END END AS ServiceType FROM [Acl_LoConnectCustomServices] LOCS  LEFT JOIN [CustomTools] CT ON CT.CustomToolId = LOCS.CustomEntityId  WHERE LOCS.PersonaID IN (" + SQL.EncodeArray((Array) personaIds) + ") AND LOCS.Access=1");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection.Count == 0)
        return customServiceInfoList.ToArray();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        string customEntityId = dataRow["CustomEntityId"].ToString();
        LoServiceType result;
        Enum.TryParse<LoServiceType>(dataRow["ServiceType"].ToString(), out result);
        bool boolean = Convert.ToBoolean(dataRow["Access"]);
        if (!customServiceInfoList.Exists((Predicate<LoConnectCustomServiceInfo>) (x => x.CustomEntityId == customEntityId)))
          customServiceInfoList.Add(new LoConnectCustomServiceInfo(customEntityId, result, boolean));
      }
      return customServiceInfoList.FindAll((Predicate<LoConnectCustomServiceInfo>) (x => x.Access)).ToArray();
    }

    public static Hashtable CheckPermission(LoConnectCustomServiceInfo service, UserInfo user)
    {
      return LoConnectCustomServicesAclDbAccessor.CheckPermission(new LoConnectCustomServiceInfo[1]
      {
        service
      }, user);
    }

    public static Hashtable CheckPermission(LoConnectCustomServiceInfo[] services, UserInfo user)
    {
      Hashtable hashtable = new Hashtable();
      if (user.IsAdministrator() || user.IsSuperAdministrator())
      {
        foreach (LoConnectCustomServiceInfo service in services)
          hashtable[(object) service.CustomEntityId] = (object) true;
        return hashtable;
      }
      int[] personaIds = user.GetPersonaIDs();
      string[] data = new string[services.Length];
      for (int index = 0; index < services.Length; ++index)
        data[index] = services[index].CustomEntityId;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select CustomEntityId, access from [Acl_LoConnectCustomServices] where personaId in (" + SQL.EncodeArray((Array) personaIds) + ") and CustomEntityId in (" + SQL.EncodeArray((Array) data) + ")");
      dbQueryBuilder.AppendLine("select CustomEntityId, access from [Acl_LoConnectCustomServices_User] where userid =" + SQL.Encode((object) user.Userid) + " and CustomEntityId in (" + SQL.EncodeArray((Array) data) + ")");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      foreach (LoConnectCustomServiceInfo service in services)
        hashtable[(object) service.CustomEntityId] = (object) false;
      DataTable table1 = dataSet.Tables[0];
      if (table1.Rows.Count > 0)
      {
        for (int index = 0; index < table1.Rows.Count; ++index)
        {
          string key = (string) table1.Rows[index]["CustomEntityId"];
          bool flag = (byte) table1.Rows[index]["access"] == (byte) 1;
          hashtable[(object) key] = (object) flag;
        }
      }
      DataTable table2 = dataSet.Tables[1];
      if (table2.Rows.Count > 0)
      {
        for (int index = 0; index < table2.Rows.Count; ++index)
        {
          string key = (string) table2.Rows[index]["CustomEntityId"];
          bool flag = (byte) table2.Rows[index]["access"] == (byte) 1;
          hashtable[(object) key] = (object) flag;
        }
      }
      return hashtable;
    }

    public static void DeleteUserPermission(string userid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from [Acl_LoConnectCustomServices_User]");
      dbQueryBuilder.AppendLine(" where userid = " + SQL.Encode((object) userid));
      dbQueryBuilder.Execute();
    }

    public static void UpdateServiceType(string customToolId, LoServiceType serviceType)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format(" UPDATE {0} SET ServiceType = {1} WHERE CustomEntityId = '{2}'", (object) "[Acl_LoConnectCustomServices]", (object) (int) serviceType, (object) customToolId));
      dbQueryBuilder.AppendLine(string.Format(" UPDATE {0} SET ServiceType = {1} WHERE CustomEntityId = '{2}'", (object) "[Acl_LoConnectCustomServices_User]", (object) (int) serviceType, (object) customToolId));
      dbQueryBuilder.ExecuteSetQuery();
    }
  }
}
