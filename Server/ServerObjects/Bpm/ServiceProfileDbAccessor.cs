// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.ServiceProfileDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public class ServiceProfileDbAccessor
  {
    private const string ServiceProfileTable = "ServiceProfiles�";
    private const string ProfileUsersTable = "ServiceProfileUsers�";
    private const string ProfileColumnProfileID = "SvcProfileID�";
    private const string ProfileColumnProductID = "SvcProductID�";
    private const string ProfileColumnName = "Name�";
    private const string ProfileColumnDescription = "Description�";
    private const string ProfileColumnDefault = "DefaultFlag�";
    private const string ProfileColumnCondition = "Condition�";
    private const string ProfileColumnLastModifiedByUserId = "LastModifiedByUserId�";
    private const string ProfileColumnLastModified = "LastModified�";
    private const string UsersColumnProfileID = "SvcProfileID�";
    private const string UsersColumnType = "Type�";
    private const string UsersColumnUserID = "UserID�";
    private const string UsersColumnPersonaID = "PersonaID�";

    public List<ServiceProfile> GetServiceProfiles()
    {
      List<ServiceProfile> serviceProfiles = new List<ServiceProfile>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServiceProfiles");
      dbQueryBuilder.SelectFrom(table);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataRowCollection)
        {
          ServiceProfile profile = this.ConvertDataRowToProfile(row);
          if (profile != null)
            serviceProfiles.Add(profile);
        }
      }
      return serviceProfiles;
    }

    public ServiceProfile GetServiceProfileById(Guid profileID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServiceProfiles");
      DbValue key = new DbValue("SvcProfileID", (object) profileID.ToString());
      dbQueryBuilder.SelectFrom(table, key);
      string text = string.Format("SELECT * FROM {0} WHERE {1} = {2}", (object) "ServiceProfileUsers", (object) "SvcProfileID", (object) SQL.Encode((object) profileID.ToString()));
      dbQueryBuilder.AppendLine(text);
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet == null)
        return (ServiceProfile) null;
      ServiceProfile profile = this.ConvertDataRowToProfile(dataSet.Tables, 0);
      if (profile == null)
        return (ServiceProfile) null;
      profile.AuthorizedUsers = this.GetAuthorizedUsers(dataSet.Tables, 1);
      return profile;
    }

    public List<ServiceProfile> GetServiceProfilesByProductID(Guid productID)
    {
      List<ServiceProfile> profilesByProductId = new List<ServiceProfile>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServiceProfiles");
      DbValue key = new DbValue("SvcProductID", (object) productID.ToString());
      dbQueryBuilder.SelectFrom(table, key);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataRowCollection)
        {
          ServiceProfile profile = this.ConvertDataRowToProfile(row);
          if (profile != null)
            profilesByProductId.Add(profile);
        }
      }
      return profilesByProductId;
    }

    private ServiceProfile ConvertDataRowToProfile(DataTableCollection tables, int index)
    {
      if (tables == null || tables.Count <= index)
        return (ServiceProfile) null;
      DataTable table = tables[index];
      return table.Rows == null || table.Rows.Count <= 0 ? (ServiceProfile) null : this.ConvertDataRowToProfile(table.Rows[0]);
    }

    private ServiceProfile ConvertDataRowToProfile(DataRow row)
    {
      if (row == null)
        return (ServiceProfile) null;
      ServiceProfile profile = new ServiceProfile();
      profile.SvcProfileID = (Guid) row["SvcProfileID"];
      profile.SvcProductID = (Guid) row["SvcProductID"];
      profile.Name = row["Name"] == DBNull.Value ? string.Empty : (string) row["Name"];
      profile.Description = row["Description"] == DBNull.Value ? string.Empty : (string) row["Description"];
      int int32 = row["DefaultFlag"] == DBNull.Value ? 0 : Convert.ToInt32(row["DefaultFlag"]);
      profile.Default = int32 > 0;
      profile.Condition = row["Condition"] == DBNull.Value ? string.Empty : (string) row["Condition"];
      profile.LastModifiedByUserId = row["LastModifiedByUserId"] == DBNull.Value ? string.Empty : (string) row["LastModifiedByUserId"];
      profile.LastModified = (DateTime) row["LastModified"];
      return profile;
    }

    public void CreateServiceProfile(ServiceProfile serviceProfile)
    {
      if (serviceProfile.SvcProfileID == Guid.Empty)
        serviceProfile.SvcProfileID = Guid.NewGuid();
      EllieMae.EMLite.Server.DbQueryBuilder sql = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (serviceProfile.Default)
      {
        string text = string.Format("UPDATE {0} SET {1} = 0 WHERE {2} = '{3}' AND {1} = 1", (object) "ServiceProfiles", (object) "DefaultFlag", (object) "SvcProductID", (object) serviceProfile.SvcProductID.ToString());
        sql.AppendLine(text);
      }
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServiceProfiles");
      DbValueList dbValueList = this.GetDBValueList(serviceProfile, true);
      sql.InsertInto(table, dbValueList, true, false);
      this.UpdateAuthorizedUsers(serviceProfile, sql);
      sql.ExecuteNonQuery();
    }

    public void UpdateServiceProfile(ServiceProfile serviceProfile)
    {
      EllieMae.EMLite.Server.DbQueryBuilder sql = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (serviceProfile.Default)
      {
        string text = string.Format("UPDATE {0} SET {1} = 0 WHERE {2} = '{3}' AND {1} = 1", (object) "ServiceProfiles", (object) "DefaultFlag", (object) "SvcProductID", (object) serviceProfile.SvcProductID.ToString());
        sql.AppendLine(text);
      }
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServiceProfiles");
      DbValue key = new DbValue("SvcProfileID", (object) serviceProfile.SvcProfileID.ToString());
      DbValueList dbValueList = this.GetDBValueList(serviceProfile);
      sql.Update(table, dbValueList, key);
      this.DeleteAuthorizedUsers(serviceProfile.SvcProfileID, sql);
      this.UpdateAuthorizedUsers(serviceProfile, sql);
      sql.ExecuteNonQuery();
    }

    public void DeleteServiceProfile(Guid profileID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServiceProfiles");
      DbValue key = new DbValue("SvcProfileID", (object) profileID.ToString());
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public void DeleteServiceProfileByProductID(Guid productID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServiceProfiles");
      DbValue key = new DbValue("SvcProductID", (object) productID.ToString());
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.ExecuteScalar();
    }

    private DbValueList GetDBValueList(ServiceProfile profile, bool forCreate = false)
    {
      DbValueList dbValueList = new DbValueList();
      if (forCreate)
      {
        dbValueList.Add("SvcProfileID", (object) profile.SvcProfileID.ToString());
        dbValueList.Add("SvcProductID", (object) profile.SvcProductID.ToString());
      }
      dbValueList.Add("Name", (object) profile.Name);
      dbValueList.Add("Description", (object) profile.Description);
      dbValueList.Add("DefaultFlag", (object) (profile.Default ? 1 : 0));
      dbValueList.Add("Condition", (object) profile.Condition);
      dbValueList.Add("LastModifiedByUserId", (object) profile.LastModifiedByUserId);
      dbValueList.Add("LastModified", (object) profile.LastModified);
      return dbValueList;
    }

    private void DeleteAuthorizedUsers(Guid profileID, EllieMae.EMLite.Server.DbQueryBuilder sql)
    {
      string text = string.Format("DELETE from {0} where {1} = {2}", (object) "ServiceProfileUsers", (object) "SvcProfileID", (object) SQL.Encode((object) profileID.ToString()));
      sql.AppendLine(text);
    }

    public void UpdateAuthorizedUsers(ServiceProfile serviceProfile, EllieMae.EMLite.Server.DbQueryBuilder sql)
    {
      if (serviceProfile.AuthorizedUsers == null)
        return;
      string strSql;
      serviceProfile.AuthorizedUsers.ForEach((Action<ServiceProfileUser>) (user =>
      {
        if (user.Type == ServiceUserType.User)
          strSql = string.Format("Insert into {0} ({1}, {2}, {3}) values ({4}, {5}, {6})", (object) "ServiceProfileUsers", (object) "SvcProfileID", (object) "Type", (object) "UserID", (object) SQL.Encode((object) serviceProfile.SvcProfileID.ToString()), (object) SQL.Encode((object) (int) user.Type), (object) SQL.EncodeString(user.UserID));
        else
          strSql = string.Format("Insert into {0} ({1}, {2}, {3}) values ({4}, {5}, {6})", (object) "ServiceProfileUsers", (object) "SvcProfileID", (object) "Type", (object) "PersonaID", (object) SQL.Encode((object) serviceProfile.SvcProfileID.ToString()), (object) SQL.Encode((object) (int) user.Type), (object) SQL.Encode((object) user.PersonaID));
        sql.AppendLine(strSql);
      }));
    }

    public List<ServiceProfileUser> GetAuthorizedUsers(DataTableCollection tables, int index)
    {
      if (tables == null || tables.Count <= index)
        return (List<ServiceProfileUser>) null;
      DataTable table = tables[index];
      if (table.Rows == null || table.Rows.Count <= 0)
        return (List<ServiceProfileUser>) null;
      List<ServiceProfileUser> authorizedUsers = new List<ServiceProfileUser>();
      foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
      {
        ServiceProfileUser serviceProfileUser = new ServiceProfileUser();
        serviceProfileUser.Type = (ServiceUserType) Convert.ToInt32(row["Type"]);
        if (serviceProfileUser.Type == ServiceUserType.Persona)
          serviceProfileUser.PersonaID = row["PersonaID"] == DBNull.Value ? 0 : Convert.ToInt32(row["PersonaID"]);
        else
          serviceProfileUser.UserID = row["UserID"] == DBNull.Value ? string.Empty : (string) row["UserID"];
        authorizedUsers.Add(serviceProfileUser);
      }
      return authorizedUsers;
    }
  }
}
