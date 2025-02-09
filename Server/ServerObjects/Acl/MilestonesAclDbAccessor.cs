// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Acl.MilestonesAclDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Acl
{
  public class MilestonesAclDbAccessor
  {
    private const string tableName = "[Acl_CustomMilestones]�";
    private const string tableName_User = "[Acl_CustomMilestones_User]�";

    private MilestonesAclDbAccessor()
    {
    }

    public static AclTriState GetPermission(
      AclMilestone feature,
      string customMilestoneId,
      string userid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select access from [Acl_CustomMilestones_User] where milestoneID = " + SQL.Encode((object) customMilestoneId) + " and userid = " + SQL.Encode((object) userid) + " and featureId = " + (object) (int) feature);
      object obj = dbQueryBuilder.ExecuteScalar();
      if (obj == null)
        return AclTriState.Unspecified;
      return (byte) obj != (byte) 1 ? AclTriState.False : AclTriState.True;
    }

    public static Hashtable GetPermissions(
      AclMilestone[] features,
      string customMilestoneId,
      string userid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select featureID, access from [Acl_CustomMilestones_User] where milestoneID = " + SQL.Encode((object) customMilestoneId) + " and userid = " + SQL.Encode((object) userid) + " and featureId in (" + SQL.EncodeArray((Array) MilestonesAclDbAccessor.getFeatureIDs(features)) + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      Hashtable permissions = new Hashtable();
      for (int index = 0; index < dataRowCollection.Count; ++index)
        permissions.Add(Enum.ToObject(typeof (AclMilestone), (int) dataRowCollection[index]["featureID"]), (object) (AclTriState) ((byte) dataRowCollection[index]["access"] == (byte) 1 ? 1 : 0));
      return permissions;
    }

    public static Dictionary<string, Hashtable> GetPermissions(
      List<EllieMae.EMLite.Workflow.Milestone> milestones,
      AclMilestone[] features,
      UserInfo currentuser)
    {
      DataRowCollection rows = new DataTable()
      {
        Columns = {
          {
            "featureID",
            Type.GetType("System.Int32")
          },
          "milestoneID",
          {
            "access",
            Type.GetType("System.Boolean")
          }
        }
      }.Rows;
      bool flag1 = currentuser.IsSuperAdministrator() || Company.GetCurrentEdition() == EncompassEdition.Broker;
      bool flag2 = flag1 || ((IEnumerable<Persona>) currentuser.UserPersonas).Any<Persona>((System.Func<Persona, bool>) (p => p.AclFeaturesDefault));
      foreach (EllieMae.EMLite.Workflow.Milestone milestone in milestones)
      {
        foreach (AclMilestone feature in features)
          rows.Add((object) (int) feature, (object) milestone.MilestoneID, (object) flag2);
      }
      Dictionary<string, Hashtable> aclMilestones = MilestonesAclDbAccessor.GenerateAclMilestones(rows, true);
      if (flag1)
        return aclMilestones;
      string userid = currentuser.Userid;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      int[] personaIds = AclUtils.GetPersonaIDs(currentuser.UserPersonas);
      foreach (int num in personaIds)
      {
        if (num == 1)
        {
          foreach (EllieMae.EMLite.Workflow.Milestone milestone in milestones)
            dbQueryBuilder.AppendLine("Update [Acl_CustomMilestones] set access = 1 where personaID = " + (object) num + " and milestoneID = " + SQL.Encode((object) milestone.MilestoneID));
        }
      }
      dbQueryBuilder.AppendLine(" select A.milestoneID, A.featureID, case when u.milestoneID IS NOT NULL then u.access else A.access end access");
      dbQueryBuilder.AppendLine(" from (\tselect milestoneID, featureID , MAX(access) access ");
      dbQueryBuilder.AppendLine(" from Acl_CustomMilestones ");
      dbQueryBuilder.AppendLine(" where personaID in (" + string.Join<int>(",", (IEnumerable<int>) personaIds) + ") ");
      dbQueryBuilder.AppendLine(" group by milestoneID, featureID) A ");
      dbQueryBuilder.AppendLine(" left join Acl_CustomMilestones_User u ");
      dbQueryBuilder.AppendLine(" on A.milestoneID = u.milestoneID ");
      dbQueryBuilder.AppendLine(" and u.userid= " + SQL.Encode((object) userid));
      foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
      {
        object key = Enum.ToObject(typeof (AclMilestone), (int) dataRow["featureID"]);
        if (aclMilestones.ContainsKey(dataRow["milestoneID"].ToString()) && aclMilestones[dataRow["milestoneID"].ToString()].Contains(key))
          aclMilestones[dataRow["milestoneID"].ToString()][key] = (object) SQL.DecodeBoolean(dataRow["access"]);
      }
      return aclMilestones;
    }

    private static Dictionary<string, Hashtable> GenerateAclMilestones(
      DataRowCollection rows,
      bool isFeature)
    {
      IEnumerable<object> objects = rows.OfType<DataRow>().Select<DataRow, object>((System.Func<DataRow, object>) (r => r["milestoneID"])).Distinct<object>();
      Dictionary<string, Hashtable> aclMilestones = new Dictionary<string, Hashtable>();
      foreach (object obj in objects)
      {
        Hashtable hashtable = new Hashtable();
        foreach (DataRow row in (InternalDataCollectionBase) rows)
        {
          if (row["milestoneID"] == obj)
          {
            if (isFeature)
              hashtable.Add(Enum.ToObject(typeof (AclMilestone), (int) row["featureID"]), (object) (bool) row["access"]);
            else
              hashtable.Add(Enum.ToObject(typeof (AclMilestone), (int) row["featureID"]), (object) ((byte) row["access"] == (byte) 1));
          }
        }
        aclMilestones.Add(obj.ToString(), hashtable);
      }
      return aclMilestones;
    }

    public static Hashtable GetPermissions(
      AclMilestone[] features,
      string customMilestoneId,
      int[] personaIDs)
    {
      Hashtable permissions1 = new Hashtable();
      for (int index = 0; index < features.Length; ++index)
        permissions1.Add((object) features[index], (object) false);
      if (personaIDs != null && personaIDs.Length != 0)
      {
        for (int index1 = 0; index1 < personaIDs.Length; ++index1)
        {
          Hashtable permissions2 = MilestonesAclDbAccessor.GetPermissions(features, customMilestoneId, personaIDs[index1]);
          for (int index2 = 0; index2 < features.Length; ++index2)
            permissions1[(object) features[index2]] = (object) (bool) ((bool) permissions1[(object) features[index2]] ? 1 : ((bool) permissions2[(object) features[index2]] ? 1 : 0));
        }
      }
      return permissions1;
    }

    public static Hashtable GetPermissions(
      AclMilestone[] features,
      string customMilestoneId,
      int personaID)
    {
      if (features.Length == 0)
        return new Hashtable();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (personaID == 1)
        dbQueryBuilder.AppendLine("Update [Acl_CustomMilestones] set access = 1 where personaID = " + (object) personaID + " and milestoneID = " + SQL.Encode((object) customMilestoneId));
      dbQueryBuilder.AppendLine("select featureID, access from [Acl_CustomMilestones] where personaID = " + (object) personaID + " and milestoneID = " + SQL.Encode((object) customMilestoneId) + " and featureID in (" + SQL.EncodeArray((Array) MilestonesAclDbAccessor.getFeatureIDs(features)) + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      Hashtable permissions = new Hashtable();
      for (int index = 0; index < dataRowCollection.Count; ++index)
        permissions.Add((object) (AclMilestone) dataRowCollection[index]["featureID"], (object) ((byte) dataRowCollection[index]["access"] == (byte) 1));
      bool aclFeaturesDefault = MilestonesAclDbAccessor.getAclFeaturesDefault(personaID);
      for (int index = 0; index < features.Length; ++index)
      {
        if (!permissions.Contains((object) features[index]))
          permissions[(object) features[index]] = (object) aclFeaturesDefault;
      }
      return permissions;
    }

    public static Dictionary<string, bool> GetPermissionsForCustomMilestone(
      AclMilestone feature,
      int personaID)
    {
      Dictionary<string, bool> forCustomMilestone = new Dictionary<string, bool>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Login);
      dbQueryBuilder.AppendLine("select A.milestoneid, B.access from Milestones A left join Acl_CustomMilestones B on A.MilestoneID = B.milestoneID where personaID = " + (object) personaID + " and featureId = " + (object) (int) feature);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      bool aclFeaturesDefault = MilestonesAclDbAccessor.getAclFeaturesDefault(personaID);
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        forCustomMilestone.Add(string.Concat(dataRow["MilestoneID"]), aclFeaturesDefault);
        if (dataRow["access"] != null)
          forCustomMilestone[string.Concat(dataRow["MilestoneID"])] = (byte) dataRow["access"] == (byte) 1;
      }
      return forCustomMilestone;
    }

    public static bool GetPermission(AclMilestone feature, string customMilestoneId, int personaID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select access from [Acl_CustomMilestones] where milestoneID = " + SQL.Encode((object) customMilestoneId) + " and personaID = " + (object) personaID + " and featureId = " + (object) (int) feature);
      object obj = dbQueryBuilder.ExecuteScalar();
      if (obj == null)
      {
        bool aclFeaturesDefault = MilestonesAclDbAccessor.getAclFeaturesDefault(personaID);
        MilestonesAclDbAccessor.SetPermission(feature, customMilestoneId, personaID, aclFeaturesDefault);
        return aclFeaturesDefault;
      }
      return (byte) obj == (byte) 1;
    }

    private static bool getAclFeaturesDefault(int personaID)
    {
      return PersonaAccessor.GetPersonaAclFeaturesDefault(personaID);
    }

    public static Dictionary<string, AclTriState> CheckApplicationPermissions(
      AclMilestone feature,
      string[] customMilestoneIDs,
      UserInfo user)
    {
      Dictionary<string, AclTriState> dictionary = new Dictionary<string, AclTriState>();
      List<int> intList = new List<int>();
      foreach (Persona userPersona in user.UserPersonas)
        intList.Add(userPersona.ID);
      foreach (string customMilestoneId in customMilestoneIDs)
      {
        if (user.IsAdministrator())
        {
          dictionary.Add(customMilestoneId, AclTriState.True);
        }
        else
        {
          dictionary.Add(customMilestoneId, MilestonesAclDbAccessor.GetPermission(feature, customMilestoneId, user.Userid));
          if (dictionary[customMilestoneId] == AclTriState.Unspecified)
          {
            Hashtable permissions = MilestonesAclDbAccessor.GetPermissions(feature, customMilestoneId, intList.ToArray());
            foreach (int key in intList.ToArray())
            {
              if (permissions.ContainsKey((object) key) && (bool) permissions[(object) key])
              {
                dictionary[customMilestoneId] = AclTriState.True;
                break;
              }
            }
            if (dictionary[customMilestoneId] == AclTriState.Unspecified)
              dictionary[customMilestoneId] = AclTriState.False;
          }
        }
      }
      return dictionary;
    }

    public static Hashtable GetPermissions(
      AclMilestone feature,
      string customMilestoneId,
      int[] personaIDs)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from Acl_CustomMilestones where milestoneID = " + SQL.Encode((object) customMilestoneId) + " and featureId = " + (object) (int) feature + " and personaID in (" + SQL.EncodeArray((Array) personaIDs) + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null)
      {
        foreach (int personaId in personaIDs)
        {
          bool aclFeaturesDefault = MilestonesAclDbAccessor.getAclFeaturesDefault(personaId);
          MilestonesAclDbAccessor.SetPermission(feature, customMilestoneId, personaId, aclFeaturesDefault);
        }
        dataRowCollection = dbQueryBuilder.Execute();
      }
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

    public static void SetPermission(
      AclMilestone feature,
      string customMilestoneId,
      string userid,
      object access)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from Acl_CustomMilestones_User where milestoneID = " + SQL.Encode((object) customMilestoneId) + " and userid = " + SQL.Encode((object) userid) + " and featureId = " + (object) (int) feature);
      if (access != null)
        dbQueryBuilder.AppendLine("insert into Acl_CustomMilestones_User (milestoneID, featureID, userid, access) values (" + SQL.Encode((object) customMilestoneId) + ", " + (object) (int) feature + ", " + SQL.Encode((object) userid) + ", " + ((bool) access ? (object) "1" : (object) "0") + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetPermission(
      AclMilestone feature,
      string customMilestoneId,
      int personaID,
      bool access)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from Acl_CustomMilestones where milestoneID = " + SQL.Encode((object) customMilestoneId) + " and featureId = " + (object) (int) feature + " and personaID = " + (object) personaID);
      dbQueryBuilder.AppendLine("insert into Acl_CustomMilestones (milestoneID, featureID, personaID, access) values (" + SQL.Encode((object) customMilestoneId) + ", " + (object) (int) feature + ", " + (object) personaID + ", " + (access ? (object) "1" : (object) "0") + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static bool CheckPermission(
      AclMilestone feature,
      string customMilestoneId,
      UserInfo userInfo)
    {
      if (userInfo.IsSuperAdministrator() || Company.GetCurrentEdition() == EncompassEdition.Broker)
        return true;
      switch (MilestonesAclDbAccessor.GetPermission(feature, customMilestoneId, userInfo.Userid))
      {
        case AclTriState.False:
          return false;
        case AclTriState.True:
          return true;
        default:
          Hashtable permissions = MilestonesAclDbAccessor.GetPermissions(feature, customMilestoneId, AclUtils.GetPersonaIDs(userInfo.UserPersonas));
          foreach (int key in (IEnumerable) permissions.Keys)
          {
            if ((bool) permissions[(object) key])
              return true;
          }
          return false;
      }
    }

    public static Hashtable CheckPermissions(
      AclMilestone[] features,
      string milestoneID,
      UserInfo userInfo)
    {
      Hashtable hashtable1 = new Hashtable();
      Hashtable hashtable2 = new Hashtable();
      if (userInfo.IsSuperAdministrator() || Company.GetCurrentEdition() == EncompassEdition.Broker)
      {
        foreach (AclMilestone feature in features)
          hashtable2.Add((object) feature, (object) true);
        return hashtable2;
      }
      CoreMilestoneIDEnumUtil.IsCoreMilestoneID(milestoneID);
      Hashtable permissions1 = MilestonesAclDbAccessor.GetPermissions(features, milestoneID, userInfo.Userid);
      if (permissions1 != null)
      {
        ArrayList arrayList = new ArrayList();
        for (int index = 0; index < features.Length; ++index)
        {
          if (permissions1.ContainsKey((object) features[index]))
          {
            hashtable1.Add((object) features[index], (object) ((AclTriState) permissions1[(object) features[index]] == AclTriState.True));
          }
          else
          {
            hashtable1.Add((object) features[index], (object) false);
            arrayList.Add((object) features[index]);
          }
        }
        features = (AclMilestone[]) arrayList.ToArray(typeof (AclMilestone));
      }
      Hashtable hashtable3 = new Hashtable();
      Hashtable permissions2 = MilestonesAclDbAccessor.GetPermissions(features, milestoneID, AclUtils.GetPersonaIDs(userInfo.UserPersonas));
      foreach (AclMilestone key in (IEnumerable) permissions2.Keys)
        hashtable1[(object) key] = permissions2[(object) key];
      return hashtable1;
    }

    public static void DuplicateACLMilestones(int sourcePersonaID, int desPersonaID)
    {
      string str1 = "";
      string str2 = "";
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from [Acl_CustomMilestones] where personaID = " + (object) sourcePersonaID);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      dbQueryBuilder.Reset();
      DataColumnCollection columns = dataTable.Columns;
      if (dataTable == null || dataTable.Rows.Count <= 0)
        return;
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        string str3 = "Insert into [Acl_CustomMilestones] (";
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

    public static Hashtable GetPersonalPermission(AclMilestone[] features, string userId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string str = "";
      if (features != null && features.Length != 0)
      {
        str = "'" + (object) (int) features[0] + "'";
        for (int index = 1; index < features.Length; ++index)
          str = str + ", '" + (object) (int) features[index] + "'";
      }
      string text = "select * from [Acl_CustomMilestones_User] where userid = '" + userId + "'";
      if (str != "")
        text = text + " AND featureId in (" + str + ")";
      dbQueryBuilder.Append(text);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null)
        return (Hashtable) null;
      Hashtable personalPermission = new Hashtable();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        string key = dataRow["userid"].ToString() + "_" + dataRow["featureId"] + "_" + dataRow["milestoneID"];
        bool flag = (byte) dataRow["access"] == (byte) 1;
        if (personalPermission[(object) key] != null)
          personalPermission[(object) key] = (object) ((bool) personalPermission[(object) key] | flag);
        else
          personalPermission[(object) key] = (object) flag;
      }
      dbQueryBuilder.Reset();
      dbQueryBuilder.AppendLine("Select * from Acl_CustomMilestones_User where userid = '" + userId + "'");
      foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
      {
        string key = dataRow["userid"].ToString() + "_" + dataRow["featureId"] + "_" + dataRow["milestoneID"];
        bool flag = (byte) dataRow["access"] == (byte) 1;
        if (personalPermission[(object) key] != null)
          personalPermission[(object) key] = (object) ((bool) personalPermission[(object) key] | flag);
        else
          personalPermission[(object) key] = (object) flag;
      }
      return personalPermission;
    }

    public static void DeleteUserSpecificSetting(string userID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Delete Acl_CustomMilestones_User where userID = '" + userID + "'");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SynchronizeAdminSetting()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Update Acl_CustomMilestones set access =1 where personaID = 1");
      dbQueryBuilder.AppendLine("SELECT MilestoneID from Milestones where milestoneid not in (select milestoneID from Acl_CustomMilestones where personaID = 1 )");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null || dataRowCollection.Count <= 0)
        return;
      dbQueryBuilder.Reset();
      for (int index = 0; index < 6; ++index)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          dbQueryBuilder.AppendLine("Insert into Acl_CustomMilestones (MilestoneID, featureID, personaID, access) values('" + dataRow["MilestoneID"] + "', " + (object) index + ", 1, 1)");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SynchronizePersonaSettingWithNewMilestone(int personaID, bool defaultAccess)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT MilestoneID from Milestones where milestoneid not in (select milestoneID from Acl_CustomMilestones where personaID = " + (object) personaID + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null || dataRowCollection.Count <= 0)
        return;
      dbQueryBuilder.Reset();
      for (int index = 0; index < 6; ++index)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          dbQueryBuilder.AppendLine("Insert into Acl_CustomMilestones (MilestoneID, featureID, personaID, access) values('" + dataRow["MilestoneID"] + "', " + (object) index + ", " + (object) personaID + "," + (object) Convert.ToInt32(defaultAccess) + ")");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SynchronizeBrokerSetting(string baseMilestoneID, string currentMilestoneID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string str = Utils.IsInt((object) baseMilestoneID) ? "Acl_Milestones" : "Acl_CustomMilestones";
      dbQueryBuilder.AppendLine("INSERT INTO Acl_CustomMilestones");
      dbQueryBuilder.AppendLine("SELECT Distinct " + SQL.EncodeString(currentMilestoneID) + ", A.featureID, A.personaID, A.access");
      dbQueryBuilder.AppendLine("FROM " + str + " A WHERE A.MilestoneID = " + SQL.EncodeString(baseMilestoneID) + " AND A.personaID!=1");
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static int[] getFeatureIDs(AclMilestone[] features)
    {
      int[] featureIds = new int[features.Length];
      for (int index = 0; index < features.Length; ++index)
        featureIds[index] = (int) features[index];
      return featureIds;
    }
  }
}
