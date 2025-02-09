// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.ServiceCredentialDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public class ServiceCredentialDbAccessor
  {
    private const string ServiceProductsTable = "ServiceProducts�";
    private const string ServiceCredentialsTable = "ServiceCredentials�";
    private const string Id = "CredentialID�";
    private const string SvcProductId = "SvcProductID�";
    private const string Name = "Name�";
    private const string Description = "Description�";
    private const string Credentials = "Credentials�";
    private const string ForAutoWorkflow = "ForAutoWorkflow�";
    private const string ExpirationDate = "ExpirationDate�";
    private const string Category = "Category�";
    private const string ProductName = "ProductName�";
    private const string PartnerID = "PartnerID�";
    private const string Version = "Version�";
    private const string ServiceCredentialsUserTable = "ServiceCredentialUsers�";
    private const string EntityId = "EntityId�";
    private const string Type = "EntityType�";
    private const string OrgEntiryUri = "/organizations/{0}�";
    private const string RoleEntiryUri = "/settings/roles/{0}�";
    private const string GroupEntiryUri = "/settings/userGroups/{0}�";
    private const string UserEntiryUri = "/users/{0}�";

    public static Guid CreateServiceCredential(ServiceCredential serviceCredential)
    {
      if (serviceCredential == null || serviceCredential.AuthorizedUsers == null)
        return Guid.Empty;
      serviceCredential.Id = Guid.NewGuid();
      EllieMae.EMLite.Server.DbQueryBuilder sql = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServiceCredentials");
      DbValueList dbValueList = ServiceCredentialDbAccessor.GetDbValueList(serviceCredential, true);
      sql.InsertInto(table, dbValueList, true, false);
      ServiceCredentialDbAccessor.AddAuthorizedUsers(serviceCredential, sql);
      sql.ExecuteNonQuery();
      return serviceCredential.Id;
    }

    public static bool UpdateServiceCredential(ServiceCredential serviceCredential)
    {
      if (serviceCredential == null || serviceCredential.AuthorizedUsers == null)
        return false;
      EllieMae.EMLite.Server.DbQueryBuilder sql = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServiceCredentials");
      DbValueList dbValueList = ServiceCredentialDbAccessor.GetDbValueList(serviceCredential, false);
      DbValue key = new DbValue("CredentialID", (object) serviceCredential.Id.ToString());
      sql.Update(table, dbValueList, key);
      ServiceCredentialDbAccessor.DeleteAuthorizedUsers(serviceCredential.Id, sql);
      ServiceCredentialDbAccessor.AddAuthorizedUsers(serviceCredential, sql);
      sql.ExecuteNonQuery();
      return true;
    }

    public static ServiceCredential GetServiceCredentialsById(
      Guid serviceCredentialId,
      bool encryptCredentials = true)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("select {1}.Category,{1}.ProductName,{1}.PartnerID,{1}.Version,{0}.CredentialID, {0}.SvcProductID, {0}.Name, {0}.Description, {0}.Credentials, {0}.ForAutoWorkflow, {0}.ExpirationDate from {0} inner join {1} on {0}.{2} = {1}.{2} where {0}.{3} = {4}", (object) "ServiceCredentials", (object) "ServiceProducts", (object) "SvcProductID", (object) "CredentialID", (object) SQL.Encode((object) serviceCredentialId.ToString())));
      string text = string.Format("SELECT {0}.CredentialID, {0}.EntityType, {0}.EntityId FROM {0} WHERE {1} = {2}", (object) "ServiceCredentialUsers", (object) "CredentialID", (object) SQL.Encode((object) serviceCredentialId.ToString()));
      dbQueryBuilder.AppendLine(text);
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count <= 0 || dataSet.Tables[0].Rows.Count <= 0)
        return (ServiceCredential) null;
      ServiceCredential credential = ServiceCredentialDbAccessor.ConvertDataRowToCredential(dataSet.Tables[0].Rows[0], encryptCredentials);
      if (credential == null)
        return (ServiceCredential) null;
      if (dataSet.Tables.Count != 2 || dataSet.Tables[1].Rows.Count <= 0)
        return (ServiceCredential) null;
      DataRow[] array = dataSet.Tables[1].Rows.Cast<DataRow>().ToArray<DataRow>();
      credential.AuthorizedUsers = ServiceCredentialDbAccessor.GetAuthorizedUsers(array);
      return credential;
    }

    public static List<ServiceCredential> GetServiceCredentials(
      Guid serviceProductId,
      bool encryptCredentials = true)
    {
      List<ServiceCredential> serviceCredentials = new List<ServiceCredential>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text1 = string.Format("select {1}.Category,{1}.ProductName,{1}.PartnerID,{1}.Version, {0}.CredentialID, {0}.SvcProductID, {0}.Name, {0}.Description, {0}.Credentials, {0}.ForAutoWorkflow, {0}.ExpirationDate from {0} inner join {1} on {0}.{2} = {1}.{2}", (object) "ServiceCredentials", (object) "ServiceProducts", (object) "SvcProductID");
      if (!(serviceProductId == Guid.Empty))
        text1 += string.Format(" where {0}.{1} = {2}", (object) "ServiceCredentials", (object) "SvcProductID", (object) SQL.Encode((object) serviceProductId.ToString()));
      dbQueryBuilder.AppendLine(text1);
      string text2 = string.Format("select {0}.CredentialID, {0}.EntityType, {0}.EntityId from {0} inner join {1} on {0}.{2} = {1}.{2}", (object) "ServiceCredentialUsers", (object) "ServiceCredentials", (object) "CredentialID");
      if (!(serviceProductId == Guid.Empty))
        text2 += string.Format(" where {0}.{1} = {2}", (object) "ServiceCredentials", (object) "SvcProductID", (object) SQL.Encode((object) serviceProductId.ToString()));
      dbQueryBuilder.AppendLine(text2);
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet == null || dataSet.Tables.Count == 0)
        return serviceCredentials;
      DataRelation relation = dataSet.Relations.Add(dataSet.Tables[0].Columns["CredentialID"], dataSet.Tables[1].Columns["CredentialID"]);
      DataTable table = dataSet.Tables[0];
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        DataRow row = table.Rows[index];
        ServiceCredential credential = ServiceCredentialDbAccessor.ConvertDataRowToCredential(row, encryptCredentials);
        serviceCredentials.Add(credential);
        DataRow[] childRows = row.GetChildRows(relation);
        if (childRows.Length != 0)
          credential.AuthorizedUsers = ServiceCredentialDbAccessor.GetAuthorizedUsers(childRows);
      }
      return serviceCredentials;
    }

    public static bool DeleteServiceCredential(Guid serviceCredentialId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServiceCredentials");
      DbValue key = new DbValue("CredentialID", (object) serviceCredentialId.ToString());
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.ExecuteNonQuery();
      return true;
    }

    public static bool CheckForDuplicateServiceCredential(
      Guid productId,
      string name,
      string credentialId = "�")
    {
      if (productId == Guid.Empty || string.IsNullOrWhiteSpace(name))
        return true;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT Name FROM ServiceCredentials WHERE SvcProductID = " + SQL.Encode((object) productId.ToString()) + " AND Name = " + SQL.Encode((object) name.Trim()));
      if (!string.IsNullOrEmpty(credentialId))
        dbQueryBuilder.AppendLine(" AND CredentialID <> " + SQL.Encode((object) credentialId));
      return dbQueryBuilder.Execute().Count != 0;
    }

    private static DbValueList GetDbValueList(ServiceCredential serviceCredential, bool isCreate)
    {
      DbValueList dbValueList1 = new DbValueList();
      Guid guid;
      if (isCreate)
      {
        DbValueList dbValueList2 = dbValueList1;
        guid = serviceCredential.Id;
        string str = guid.ToString();
        dbValueList2.Add("CredentialID", (object) str);
      }
      if (!string.IsNullOrEmpty(serviceCredential.Credentials))
        dbValueList1.Add("Credentials", serviceCredential.IsCredentialsEncrypted ? (object) serviceCredential.Credentials : (object) AesManager.Encrypt(serviceCredential.Credentials));
      DbValueList dbValueList3 = dbValueList1;
      guid = serviceCredential.SvcProductId;
      string str1 = guid.ToString();
      dbValueList3.Add("SvcProductID", (object) str1);
      dbValueList1.Add("Name", (object) serviceCredential.Name);
      dbValueList1.Add("Description", (object) serviceCredential.Description);
      dbValueList1.Add("ExpirationDate", (object) serviceCredential.ExpirationDate);
      dbValueList1.Add("ForAutoWorkflow", (object) Convert.ToInt32(serviceCredential.AutoWorkflow));
      return dbValueList1;
    }

    private static void AddAuthorizedUsers(ServiceCredential serviceCredential, EllieMae.EMLite.Server.DbQueryBuilder sql)
    {
      if (serviceCredential == null || serviceCredential.AuthorizedUsers == null)
        return;
      string strSql;
      serviceCredential.AuthorizedUsers.ForEach((Action<AuthorizedUser>) (user =>
      {
        strSql = string.Format("Insert into {0} ({1}, {2},{3}) values ({4}, {5},{6})", (object) "ServiceCredentialUsers", (object) "CredentialID", (object) "EntityId", (object) "EntityType", (object) SQL.Encode((object) serviceCredential.Id.ToString()), (object) SQL.Encode((object) user.EntityId), (object) SQL.Encode((object) (int) user.EntityType));
        sql.AppendLine(strSql);
      }));
    }

    private static void DeleteAuthorizedUsers(Guid credentialGuid, EllieMae.EMLite.Server.DbQueryBuilder sql)
    {
      string text = string.Format("DELETE from {0} where {1} = {2}", (object) "ServiceCredentialUsers", (object) "CredentialID", (object) SQL.Encode((object) credentialGuid.ToString()));
      sql.AppendLine(text);
    }

    private static List<AuthorizedUser> GetAuthorizedUsers(DataRow[] userRows)
    {
      List<AuthorizedUser> authorizedUsers = new List<AuthorizedUser>();
      foreach (DataRow userRow in userRows)
      {
        AuthorizedUser authorizedUser = new AuthorizedUser()
        {
          EntityId = userRow["EntityId"] == DBNull.Value ? string.Empty : (string) userRow["EntityId"],
          CredentialId = userRow["CredentialID"] == DBNull.Value ? Guid.Empty : (Guid) userRow["CredentialID"],
          EntityType = userRow["EntityType"] == DBNull.Value ? EntityType.User : (EntityType) (short) userRow["EntityType"]
        };
        ServiceCredentialDbAccessor.GetEntityDetails(authorizedUser);
        authorizedUsers.Add(authorizedUser);
      }
      return authorizedUsers;
    }

    private static void GetEntityDetails(AuthorizedUser authorizedUser)
    {
      if (authorizedUser.EntityType == EntityType.User)
      {
        UserInfo userById = User.GetUserById(authorizedUser.EntityId);
        if (userById != (UserInfo) null)
        {
          authorizedUser.EntityName = userById.FullName;
          authorizedUser.EntityUri = string.Format("/users/{0}", (object) authorizedUser.EntityId);
        }
      }
      int result;
      if (!int.TryParse(authorizedUser.EntityId, out result))
        return;
      if (authorizedUser.EntityType == EntityType.Group)
      {
        AclGroup groupById = AclGroupAccessor.GetGroupById(result);
        if (groupById != (AclGroup) null)
        {
          authorizedUser.EntityName = groupById.Name;
          authorizedUser.EntityUri = string.Format("/settings/userGroups/{0}", (object) authorizedUser.EntityId);
        }
      }
      if (authorizedUser.EntityType == EntityType.Organization)
      {
        OrgInfo orgInfo = Organization.LoadOrganization(result);
        if (orgInfo != null)
        {
          authorizedUser.EntityName = orgInfo.OrgName;
          authorizedUser.EntityUri = string.Format("/organizations/{0}", (object) authorizedUser.EntityId);
        }
      }
      if (authorizedUser.EntityType != EntityType.Role)
        return;
      RoleInfo roleFunction = WorkflowBpmDbAccessor.GetRoleFunction(result);
      if (roleFunction == null)
        return;
      authorizedUser.EntityName = roleFunction.Name;
      authorizedUser.EntityUri = string.Format("/settings/roles/{0}", (object) authorizedUser.EntityId);
    }

    private static ServiceCredential ConvertDataRowToCredential(
      DataRow credentialRow,
      bool encryptCredentials)
    {
      if (credentialRow == null)
        return (ServiceCredential) null;
      return new ServiceCredential()
      {
        Id = credentialRow["CredentialID"] == DBNull.Value ? Guid.Empty : (Guid) credentialRow["CredentialID"],
        SvcProductId = credentialRow["SvcProductID"] == DBNull.Value ? Guid.Empty : (Guid) credentialRow["SvcProductID"],
        AutoWorkflow = Convert.ToBoolean((short) credentialRow["ForAutoWorkflow"]),
        Credentials = credentialRow["Credentials"] == DBNull.Value ? string.Empty : (encryptCredentials ? (string) credentialRow["Credentials"] : AesManager.Decrypt((string) credentialRow["Credentials"])),
        Description = credentialRow["Description"] == DBNull.Value ? string.Empty : (string) credentialRow["Description"],
        ExpirationDate = credentialRow["ExpirationDate"] == DBNull.Value ? DateTime.MinValue : (DateTime) credentialRow["ExpirationDate"],
        Category = credentialRow["Category"] == DBNull.Value ? string.Empty : (string) credentialRow["Category"],
        ProductName = credentialRow["ProductName"] == DBNull.Value ? string.Empty : (string) credentialRow["ProductName"],
        PartnerID = credentialRow["PartnerID"] == DBNull.Value ? string.Empty : (string) credentialRow["PartnerID"],
        Version = credentialRow["Version"] == DBNull.Value ? (string) null : (string) credentialRow["Version"],
        Name = credentialRow["Name"] == DBNull.Value ? string.Empty : (string) credentialRow["Name"],
        IsCredentialsEncrypted = encryptCredentials
      };
    }
  }
}
