// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Acl.FeatureConfigsAclDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Acl
{
  public class FeatureConfigsAclDbAccessor
  {
    private const string tableName = "[Acl_FeatureConfigs]�";
    private const string tableName_User = "[Acl_FeatureConfigs_User]�";
    private const string featureConfigsDefaultCacheName = "AclFeatureConfigsDefault�";

    private FeatureConfigsAclDbAccessor()
    {
    }

    private static DataRowCollection GetUserAclFeatureConfigsFromDb(
      AclFeature[] features,
      string userid)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select featureID, access from [Acl_FeatureConfigs_User] where userid = @userid and featureID in (" + SQL.EncodeArray((Array) FeatureConfigsAclDbAccessor.getFeatureIDs(features)) + ")");
        DbCommandParameter parameter = new DbCommandParameter(nameof (userid), (object) userid.TrimEnd(), DbType.AnsiString);
        return pgDbQueryBuilder.Execute(parameter);
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select featureID, access from [Acl_FeatureConfigs_User] where userid = " + SQL.EncodeString(userid) + " and featureID in (" + SQL.EncodeArray((Array) FeatureConfigsAclDbAccessor.getFeatureIDs(features)) + ")");
      return dbQueryBuilder.Execute();
    }

    [PgReady]
    public static Dictionary<AclFeature, int> GetPermissions(AclFeature[] features, string userid)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        if (features.Length == 0)
          return new Dictionary<AclFeature, int>();
        DataRowCollection featureConfigsFromDb = FeatureConfigsAclDbAccessor.GetUserAclFeatureConfigsFromDb(features, userid);
        Dictionary<AclFeature, int> permissions = new Dictionary<AclFeature, int>();
        for (int index = 0; index < featureConfigsFromDb.Count; ++index)
          permissions.Add((AclFeature) Enum.ToObject(typeof (AclFeature), (int) featureConfigsFromDb[index]["featureID"]), Utils.ParseInt((object) string.Concat(featureConfigsFromDb[index]["access"])));
        return permissions;
      }
      if (features.Length == 0)
        return new Dictionary<AclFeature, int>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select featureID, access from [Acl_FeatureConfigs_User] where userid = " + SQL.EncodeString(userid) + " and featureID in (" + SQL.EncodeArray((Array) FeatureConfigsAclDbAccessor.getFeatureIDs(features)) + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      Dictionary<AclFeature, int> permissions1 = new Dictionary<AclFeature, int>();
      for (int index = 0; index < dataRowCollection.Count; ++index)
        permissions1.Add((AclFeature) Enum.ToObject(typeof (AclFeature), (int) dataRowCollection[index]["featureID"]), Utils.ParseInt((object) string.Concat(dataRowCollection[index]["access"])));
      return permissions1;
    }

    [PgReady]
    private static DataRowCollection GetPersonaAclFeatureConfigsFromDb(
      AclFeature[] features,
      int personaID)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        if (personaID == 1)
          pgDbQueryBuilder.AppendLine("Update [Acl_FeatureConfigs] set access = 1 where personaID = " + (object) personaID + ";");
        pgDbQueryBuilder.AppendLine("select featureID, access from [Acl_FeatureConfigs] where personaID = " + (object) personaID + " and featureID in (" + SQL.EncodeArray((Array) FeatureConfigsAclDbAccessor.getFeatureIDs(features)) + ")");
        return pgDbQueryBuilder.Execute();
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (personaID == 1)
        dbQueryBuilder.AppendLine("Update [Acl_FeatureConfigs] set access = 1 where personaID = " + (object) personaID);
      dbQueryBuilder.AppendLine("select featureID, access from [Acl_FeatureConfigs] where personaID = " + (object) personaID + " and featureID in (" + SQL.EncodeArray((Array) FeatureConfigsAclDbAccessor.getFeatureIDs(features)) + ")");
      return dbQueryBuilder.Execute();
    }

    [PgReady]
    public static Dictionary<AclFeature, int> GetPermissions(AclFeature[] features, int personaID)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        if (features.Length == 0)
          return new Dictionary<AclFeature, int>();
        DataRowCollection featureConfigsFromDb = FeatureConfigsAclDbAccessor.GetPersonaAclFeatureConfigsFromDb(features, personaID);
        Dictionary<AclFeature, int> permissions = new Dictionary<AclFeature, int>();
        for (int index = 0; index < featureConfigsFromDb.Count; ++index)
          permissions.Add((AclFeature) featureConfigsFromDb[index]["featureID"], Utils.ParseInt((object) string.Concat(featureConfigsFromDb[index]["access"])));
        int aclFeaturesDefault = FeatureConfigsAclDbAccessor.GetAclFeaturesDefault(personaID);
        for (int index = 0; index < features.Length; ++index)
        {
          if (!permissions.ContainsKey(features[index]))
            permissions[features[index]] = aclFeaturesDefault;
        }
        return permissions;
      }
      if (features.Length == 0)
        return new Dictionary<AclFeature, int>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (personaID == 1)
        dbQueryBuilder.AppendLine("Update [Acl_FeatureConfigs] set access = 1 where personaID = " + (object) personaID);
      dbQueryBuilder.AppendLine("select featureID, access from [Acl_FeatureConfigs] where personaID = " + (object) personaID + " and featureID in (" + SQL.EncodeArray((Array) FeatureConfigsAclDbAccessor.getFeatureIDs(features)) + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      Dictionary<AclFeature, int> permissions1 = new Dictionary<AclFeature, int>();
      for (int index = 0; index < dataRowCollection.Count; ++index)
        permissions1.Add((AclFeature) dataRowCollection[index]["featureID"], Utils.ParseInt((object) string.Concat(dataRowCollection[index]["access"])));
      int aclFeaturesDefault1 = FeatureConfigsAclDbAccessor.GetAclFeaturesDefault(personaID);
      for (int index = 0; index < features.Length; ++index)
      {
        if (!permissions1.ContainsKey(features[index]))
          permissions1[features[index]] = aclFeaturesDefault1;
      }
      return permissions1;
    }

    public static void SetPermission(AclFeature feature, string userid, int access)
    {
      FeatureConfigsAclDbAccessor.SetPermissions(new Dictionary<AclFeature, int>()
      {
        [feature] = access
      }, userid);
    }

    public static void SetPermission(AclFeature feature, int personaID, int access)
    {
      FeatureConfigsAclDbAccessor.SetPermissions(new Dictionary<AclFeature, int>()
      {
        [feature] = access
      }, personaID);
    }

    public static void SetPermissions(Dictionary<AclFeature, int> featureAccesses, string userid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (AclFeature key in featureAccesses.Keys)
      {
        dbQueryBuilder.AppendLine("delete from [Acl_FeatureConfigs_User] where featureID = " + (object) (int) key + " and userid = " + SQL.Encode((object) userid));
        if (featureAccesses[key] >= 0)
          dbQueryBuilder.AppendLine("insert into [Acl_FeatureConfigs_User] (featureID, userid, access) values (" + (object) (int) key + ", " + SQL.Encode((object) userid) + ", " + (object) featureAccesses[key] + ")");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetPermissions(Dictionary<AclFeature, int> featureAccesses, int personaID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (AclFeature key in featureAccesses.Keys)
      {
        dbQueryBuilder.AppendLine("delete from [Acl_FeatureConfigs] where featureID = " + (object) (int) key + " and personaId = " + (object) personaID);
        if (featureAccesses[key] >= 0)
          dbQueryBuilder.AppendLine("insert into [Acl_FeatureConfigs] (featureID, personaID, access) values (" + (object) (int) key + ", " + (object) personaID + ", " + (object) featureAccesses[key] + ")");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void ClearUserSpecificSettings(string userid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from [Acl_FeatureConfigs_User] where userid = " + SQL.Encode((object) userid));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void ClearUserSpecificSettings(AclFeature feature, string userid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from [Acl_FeatureConfigs_User] where userid = " + SQL.Encode((object) userid) + " and featureID = " + (object) (int) feature);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DuplicateACLFeatureConfigs(int sourcePersonaID, int desPersonaID)
    {
      string str1 = "";
      string str2 = "";
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from [Acl_FeatureConfigs] where personaID = " + (object) sourcePersonaID);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      dbQueryBuilder.Reset();
      DataColumnCollection columns = dataTable.Columns;
      if (dataTable == null || dataTable.Rows.Count <= 0)
        return;
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        string str3 = "Insert into [Acl_FeatureConfigs] (";
        for (int index = 0; index < columns.Count; ++index)
        {
          if (index == 0)
          {
            str3 += columns[index].ColumnName;
            str2 = !(columns[index].ColumnName != "personaID") ? str2 + SQL.Encode((object) desPersonaID) : str2 + SQL.Encode(row[columns[index].ColumnName]);
          }
          else
          {
            str3 = str3 + ", " + columns[index].ColumnName;
            str2 = !(columns[index].ColumnName != "personaID") ? str2 + ", " + SQL.Encode((object) desPersonaID) : str2 + ", " + SQL.Encode(row[columns[index].ColumnName]);
          }
        }
        string text = str3 + " ) Values (" + str2 + ")";
        dbQueryBuilder.AppendLine(text);
        str1 = "";
        str2 = "";
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static int GetPermission(AclFeature feature, string userId)
    {
      Dictionary<AclFeature, int> permissions = FeatureConfigsAclDbAccessor.GetPermissions(new AclFeature[1]
      {
        feature
      }, userId);
      return !permissions.ContainsKey(feature) ? -1 : permissions[feature];
    }

    public static int GetPermission(AclFeature feature, int personaId)
    {
      Dictionary<AclFeature, int> permissions = FeatureConfigsAclDbAccessor.GetPermissions(new AclFeature[1]
      {
        feature
      }, personaId);
      return !permissions.ContainsKey(feature) ? -1 : permissions[feature];
    }

    public static Dictionary<AclFeature, int> GetPermissions(
      AclFeature[] features,
      Persona[] personas)
    {
      int[] personaIDs = new int[personas.Length];
      for (int index = 0; index < personaIDs.Length; ++index)
        personaIDs[index] = personas[index].ID;
      return FeatureConfigsAclDbAccessor.GetPermissions(features, personaIDs);
    }

    public static Dictionary<AclFeature, int> GetPermissions(
      AclFeature[] features,
      int[] personaIDs)
    {
      Dictionary<AclFeature, int> permissions1 = new Dictionary<AclFeature, int>();
      for (int index = 0; index < features.Length; ++index)
        permissions1.Add(features[index], 0);
      if (personaIDs != null && personaIDs.Length != 0)
      {
        for (int index1 = 0; index1 < personaIDs.Length; ++index1)
        {
          Dictionary<AclFeature, int> permissions2 = FeatureConfigsAclDbAccessor.GetPermissions(features, personaIDs[index1]);
          for (int index2 = 0; index2 < features.Length; ++index2)
            permissions1[features[index2]] = permissions1[features[index2]] > permissions2[features[index2]] ? permissions1[features[index2]] : permissions2[features[index2]];
        }
      }
      return permissions1;
    }

    public static int CheckPermission(AclFeature feature, UserInfo userInfo)
    {
      if (userInfo.Userid != "(trusted)" && Company.GetCurrentEdition() == EncompassEdition.Broker)
        return -1;
      int num = FeatureConfigsAclDbAccessor.GetPermission(feature, userInfo.Userid);
      if (num >= 0)
        return num;
      for (int index = 0; index < userInfo.UserPersonas.Length; ++index)
      {
        int permission = FeatureConfigsAclDbAccessor.GetPermission(feature, userInfo.UserPersonas[index].ID);
        if (num < permission)
          num = permission;
      }
      return num;
    }

    public static Dictionary<AclFeature, int> CheckPermissions(
      AclFeature[] features,
      UserInfo userInfo)
    {
      Dictionary<AclFeature, int> permissions1 = FeatureConfigsAclDbAccessor.GetPermissions(features, AclUtils.GetPersonaIDs(userInfo.UserPersonas));
      Dictionary<AclFeature, int> permissions2 = FeatureConfigsAclDbAccessor.GetPermissions(features, userInfo.Userid);
      foreach (AclFeature key in permissions2.Keys)
        permissions1[key] = permissions2[key];
      if (userInfo.Userid != "(trusted)" && Company.GetCurrentEdition() == EncompassEdition.Broker)
      {
        foreach (AclFeature feature in features)
          permissions1[feature] = 0;
      }
      return permissions1;
    }

    public static int GetAclFeaturesDefault(int[] personaIDs)
    {
      int aclFeaturesDefault = 0;
      for (int index = 0; index < personaIDs.Length; ++index)
        aclFeaturesDefault = aclFeaturesDefault > FeatureConfigsAclDbAccessor.GetAclFeaturesDefault(personaIDs[index]) ? aclFeaturesDefault : FeatureConfigsAclDbAccessor.GetAclFeaturesDefault(personaIDs[index]);
      return aclFeaturesDefault;
    }

    public static int GetAclFeaturesDefault(int personaID)
    {
      return !PersonaAccessor.GetPersonaAclFeaturesDefault(personaID) ? 0 : int.MaxValue;
    }

    private static DataTable getPersonaRecordByFeature(AclFeature[] features, int access)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select distinct personaID from [Acl_FeatureConfigs] where access = " + (object) access + " and featureID in (" + SQL.EncodeArray((Array) FeatureConfigsAclDbAccessor.getFeatureIDs(features)) + ")");
      return dbQueryBuilder.ExecuteTableQuery();
    }

    private static DataTable getUserRecordByFeature(AclFeature[] features, int access)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select distinct userid from [Acl_FeatureConfigs_User] where access = " + (object) access + " and featureID in (" + SQL.EncodeArray((Array) FeatureConfigsAclDbAccessor.getFeatureIDs(features)) + ")");
      return dbQueryBuilder.ExecuteTableQuery();
    }

    private static int[] getFeatureIDs(AclFeature[] features)
    {
      int[] featureIds = new int[features.Length];
      for (int index = 0; index < features.Length; ++index)
        featureIds[index] = (int) features[index];
      return featureIds;
    }
  }
}
