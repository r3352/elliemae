// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Acl.EnhancedConditionsAclDbAccessor
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
  public class EnhancedConditionsAclDbAccessor
  {
    private const string tableName = "[Acl_EnhancedConditions]�";
    private const string tableName_User = "[Acl_EnhancedConditions_User]�";
    private const string tableName_ConditionType = "[EnhancedConditionTypes]�";

    public static Hashtable GetPermissions(
      AclEnhancedConditionType feature,
      int personaID,
      Guid conditionType)
    {
      return new Hashtable();
    }

    public static Hashtable GetPersonaPermissions(Guid conditionType, int personaID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from [Acl_EnhancedConditions] where PersonaID = " + SQL.Encode((object) personaID) + " and ConditionTypeID = '" + SQL.Encode((object) conditionType) + "'");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      Hashtable personaPermissions = new Hashtable();
      for (int index = 0; index < dataRowCollection.Count; ++index)
        personaPermissions.Add((object) dataRowCollection[index]["featureID"].ToString(), (object) dataRowCollection[index]["access"].ToString());
      return personaPermissions;
    }

    public static void SetPermissions(
      Dictionary<AclEnhancedConditionType, bool> features,
      int personaID,
      Guid conditionTypeID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete from [Acl_EnhancedConditions] where PersonaID = " + (object) personaID + " and conditionTypeID = '" + (object) conditionTypeID + "'");
      foreach (KeyValuePair<AclEnhancedConditionType, bool> feature in features)
      {
        int num = feature.Value ? 1 : 0;
        dbQueryBuilder.AppendLine("Insert [Acl_EnhancedConditions] (personaID, conditionTypeID, featureID, access) Values( " + SQL.Encode((object) personaID) + ", '" + (object) conditionTypeID + "'," + (object) (int) feature.Key + ", " + (object) num + ")");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetPermission(
      AclEnhancedConditionType feature,
      int personaID,
      AclTriState access,
      Guid conditionTypeID)
    {
      EnhancedConditionsAclDbAccessor.SetPermissions(new Hashtable()
      {
        [(object) feature] = (object) access
      }, personaID, conditionTypeID);
    }

    public static void SetPermissions(
      Hashtable featureAccesses,
      int personaID,
      Guid conditionTypeID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (AclEnhancedConditionType key in (IEnumerable) featureAccesses.Keys)
      {
        dbQueryBuilder.AppendLine("delete from [Acl_EnhancedConditions] where featureID = " + (object) (int) key + " and personaId = " + (object) personaID + " and conditionTypeID='" + (object) conditionTypeID + "'");
        AclTriState featureAccess = (AclTriState) featureAccesses[(object) key];
        if (featureAccess != AclTriState.Unspecified)
          dbQueryBuilder.AppendLine("insert into [Acl_EnhancedConditions] (featureID, personaID, conditionTypeID, access) values (" + (object) (int) key + ", " + (object) personaID + ", '" + (object) conditionTypeID + "', " + (object) (int) featureAccess + ")");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetPermission(
      AclEnhancedConditionType feature,
      string userID,
      AclTriState access,
      Guid conditionTypeID)
    {
      EnhancedConditionsAclDbAccessor.SetPermissions(new Hashtable()
      {
        [(object) feature] = (object) access
      }, userID, conditionTypeID);
    }

    public static void SetPermissions(
      Hashtable featureAccesses,
      string userID,
      Guid conditionTypeID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (AclEnhancedConditionType key in (IEnumerable) featureAccesses.Keys)
      {
        dbQueryBuilder.AppendLine("delete from [Acl_EnhancedConditions_User] where featureID = " + (object) (int) key + " and userID = '" + userID + "' and conditionTypeID='" + (object) conditionTypeID + "'");
        AclTriState featureAccess = (AclTriState) featureAccesses[(object) key];
        if (featureAccess != AclTriState.Unspecified)
          dbQueryBuilder.AppendLine("insert into [Acl_EnhancedConditions_User] (featureID, userID, conditionTypeID, access) values (" + (object) (int) key + ", '" + userID + "', '" + (object) conditionTypeID + "', " + (object) (int) featureAccess + ")");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteAllUserSpecificPermissions(string userID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from [Acl_EnhancedConditions_User] where userID = '" + userID + "'");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static Hashtable GetPermissions(
      AclEnhancedConditionType[] features,
      Guid conditionType,
      string userid)
    {
      if (features.Length == 0)
        return new Hashtable();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select featureID, access from [Acl_EnhancedConditions_User] where userid = " + SQL.EncodeString(userid) + " and featureID in (" + SQL.EncodeArray((Array) EnhancedConditionsAclDbAccessor.getFeatureIDs(features)) + ") and conditionTypeID ='" + (object) conditionType + "'");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      Hashtable permissions = new Hashtable();
      for (int index = 0; index < dataRowCollection.Count; ++index)
        permissions.Add(Enum.ToObject(typeof (AclEnhancedConditionType), (int) dataRowCollection[index]["featureID"]), (object) (AclTriState) ((byte) dataRowCollection[index]["access"] == (byte) 1 ? 1 : 0));
      return permissions;
    }

    public static Dictionary<string, Hashtable> GetPermissions(
      AclEnhancedConditionType[] features,
      string userid)
    {
      if (features.Length == 0)
        return new Dictionary<string, Hashtable>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select title, featureID, access from [Acl_EnhancedConditions_User] A join [EnhancedConditionTypes] B on A.conditionTypeID = B.ID where userid = " + SQL.EncodeString(userid) + " and featureID in (" + SQL.EncodeArray((Array) EnhancedConditionsAclDbAccessor.getFeatureIDs(features)) + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      Dictionary<string, Hashtable> permissions = new Dictionary<string, Hashtable>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      for (int index = 0; index < dataRowCollection.Count; ++index)
      {
        string key = dataRowCollection[index]["title"].ToString();
        if (!permissions.ContainsKey(key))
          permissions.Add(key, new Hashtable());
        permissions[key].Add(Enum.ToObject(typeof (AclEnhancedConditionType), (int) dataRowCollection[index]["featureID"]), (object) ((byte) dataRowCollection[index]["access"] == (byte) 1));
      }
      return permissions;
    }

    public static bool GetPermission(
      AclEnhancedConditionType feature,
      Guid conditionType,
      int personaId)
    {
      Hashtable personaPermissions = EnhancedConditionsAclDbAccessor.GetPersonaPermissions(conditionType, personaId);
      return personaPermissions.Contains((object) feature) && (bool) personaPermissions[(object) feature];
    }

    public static Dictionary<string, Hashtable> GetPermissions(
      AclEnhancedConditionType[] features,
      int personaID,
      bool defaultAccess = false)
    {
      Dictionary<string, Hashtable> permissions = new Dictionary<string, Hashtable>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      if (features.Length == 0)
        return permissions;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (!defaultAccess)
      {
        dbQueryBuilder.AppendLine("select title, featureID, access from [Acl_EnhancedConditions] A join [EnhancedConditionTypes] B on A.conditionTypeID = B.ID where personaID = " + (object) personaID + " and featureID in (" + SQL.EncodeArray((Array) EnhancedConditionsAclDbAccessor.getFeatureIDs(features)) + ")");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        for (int index = 0; index < dataRowCollection.Count; ++index)
        {
          string key = dataRowCollection[index]["title"].ToString();
          if (!permissions.ContainsKey(key))
            permissions.Add(key, new Hashtable());
          permissions[key].Add(Enum.ToObject(typeof (AclEnhancedConditionType), (int) dataRowCollection[index]["featureID"]), (object) ((byte) dataRowCollection[index]["access"] == (byte) 1));
        }
      }
      else
      {
        int[] featureIds = EnhancedConditionsAclDbAccessor.getFeatureIDs(features);
        dbQueryBuilder.AppendLine("select title from [EnhancedConditionTypes]");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        for (int index = 0; index < dataRowCollection.Count; ++index)
        {
          string key1 = dataRowCollection[index]["title"].ToString();
          if (!permissions.ContainsKey(key1))
            permissions.Add(key1, new Hashtable());
          foreach (int num in featureIds)
          {
            object key2 = Enum.ToObject(typeof (AclEnhancedConditionType), num);
            if (!permissions[key1].ContainsKey(key2))
              permissions[key1].Add(key2, (object) true);
          }
        }
      }
      return permissions;
    }

    public static Dictionary<string, Hashtable> GetPermissions(
      AclEnhancedConditionType[] features,
      int[] personaIDs,
      bool defaultAccess = false)
    {
      Dictionary<string, Hashtable> permissions1 = new Dictionary<string, Hashtable>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      if (personaIDs != null && personaIDs.Length != 0)
      {
        for (int index1 = 0; index1 < personaIDs.Length; ++index1)
        {
          Dictionary<string, Hashtable> permissions2 = EnhancedConditionsAclDbAccessor.GetPermissions(features, personaIDs[index1], defaultAccess);
          for (int index2 = 0; index2 < features.Length; ++index2)
          {
            foreach (KeyValuePair<string, Hashtable> keyValuePair in permissions2)
            {
              if (!permissions1.ContainsKey(keyValuePair.Key))
                permissions1.Add(keyValuePair.Key, new Hashtable());
              if (!permissions1[keyValuePair.Key].Contains((object) features[index2]))
                permissions1[keyValuePair.Key].Add((object) features[index2], (object) false);
              bool flag = permissions2[keyValuePair.Key].Contains((object) features[index2]) && (bool) permissions2[keyValuePair.Key][(object) features[index2]];
              permissions1[keyValuePair.Key][(object) features[index2]] = (object) ((bool) permissions1[keyValuePair.Key][(object) features[index2]] | flag);
            }
          }
        }
      }
      return permissions1;
    }

    public static Hashtable GetPermissions(
      AclEnhancedConditionType[] features,
      Guid conditionType,
      int[] personaIDs)
    {
      Hashtable permissions1 = new Hashtable();
      for (int index = 0; index < features.Length; ++index)
        permissions1.Add((object) features[index], (object) false);
      if (personaIDs != null && personaIDs.Length != 0)
      {
        for (int index1 = 0; index1 < personaIDs.Length; ++index1)
        {
          Hashtable permissions2 = EnhancedConditionsAclDbAccessor.GetPermissions(features, conditionType, personaIDs[index1]);
          for (int index2 = 0; index2 < features.Length; ++index2)
            permissions1[(object) features[index2]] = (object) (bool) ((bool) permissions1[(object) features[index2]] ? 1 : (permissions2.Contains((object) features[index2]) ? ((bool) permissions2[(object) features[index2]] ? 1 : 0) : 0));
        }
      }
      return permissions1;
    }

    public static Hashtable GetPermissions(
      AclEnhancedConditionType[] features,
      Guid conditionType,
      int personaID)
    {
      if (features.Length == 0)
        return new Hashtable();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select featureID, access from [Acl_EnhancedConditions] where personaID = " + (object) personaID + " and featureID in (" + SQL.EncodeArray((Array) EnhancedConditionsAclDbAccessor.getFeatureIDs(features)) + ") and conditionTypeID ='" + (object) conditionType + "'");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      Hashtable permissions = new Hashtable();
      for (int index = 0; index < dataRowCollection.Count; ++index)
        permissions.Add((object) (AclEnhancedConditionType) dataRowCollection[index]["featureID"], (object) ((byte) dataRowCollection[index]["access"] == (byte) 1));
      return permissions;
    }

    private static int[] getFeatureIDs(AclEnhancedConditionType[] features)
    {
      int[] featureIds = new int[features.Length];
      for (int index = 0; index < features.Length; ++index)
        featureIds[index] = (int) features[index];
      return featureIds;
    }

    public static Dictionary<string, Hashtable> CheckPermissions(
      AclEnhancedConditionType[] features,
      UserInfo userInfo)
    {
      bool defaultAccess = userInfo.IsAdministrator();
      Dictionary<string, Hashtable> permissions1 = EnhancedConditionsAclDbAccessor.GetPermissions(features, AclUtils.GetPersonaIDs(userInfo.UserPersonas), defaultAccess);
      Dictionary<string, Hashtable> permissions2 = EnhancedConditionsAclDbAccessor.GetPermissions(features, userInfo.Userid);
      for (int index = 0; index < features.Length; ++index)
      {
        foreach (KeyValuePair<string, Hashtable> keyValuePair in permissions2)
        {
          if (permissions2[keyValuePair.Key].Contains((object) features[index]))
          {
            if (!permissions1.ContainsKey(keyValuePair.Key))
              permissions1.Add(keyValuePair.Key, new Hashtable());
            if (!permissions1[keyValuePair.Key].ContainsKey((object) features[index]))
              permissions1[keyValuePair.Key].Add((object) features[index], (object) 0);
            permissions1[keyValuePair.Key][(object) features[index]] = permissions2[keyValuePair.Key][(object) features[index]];
          }
        }
      }
      return permissions1;
    }

    public static void DuplicateEnhancedConditionTypeFeatures(int sourcePersonaID, int desPersonaID)
    {
      string str1 = "";
      string str2 = "";
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from [Acl_EnhancedConditions] where personaID = " + SQL.Encode((object) sourcePersonaID));
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      dbQueryBuilder.Reset();
      DataColumnCollection columns = dataTable.Columns;
      if (dataTable == null || dataTable.Rows.Count <= 0)
        return;
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        string str3 = "Insert into [Acl_EnhancedConditions] (";
        for (int index = 0; index < columns.Count; ++index)
        {
          if (index == 0)
          {
            str3 += columns[index].ColumnName;
            str2 = !(columns[index].ColumnName.ToLower() == "conditiontypeid") ? (!(columns[index].ColumnName.ToLower() != "personaid") ? str2 + SQL.Encode((object) desPersonaID) : str2 + SQL.Encode(row[columns[index].ColumnName])) : str2 + "'" + SQL.Encode(row[columns[index].ColumnName]) + "'";
          }
          else
          {
            str3 = str3 + ", " + columns[index].ColumnName;
            str2 = !(columns[index].ColumnName.ToLower() == "conditiontypeid") ? (!(columns[index].ColumnName.ToLower() != "personaid") ? str2 + ", " + SQL.Encode((object) desPersonaID) : str2 + ", " + SQL.Encode(row[columns[index].ColumnName])) : str2 + ", '" + SQL.Encode(row[columns[index].ColumnName]) + "'";
          }
        }
        string text = str3 + " ) Values (" + str2 + ")";
        dbQueryBuilder.AppendLine(text);
        str1 = "";
        str2 = "";
      }
      dbQueryBuilder.ExecuteNonQuery();
    }
  }
}
