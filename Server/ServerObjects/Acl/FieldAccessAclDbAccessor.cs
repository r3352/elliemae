// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Acl.FieldAccessAclDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Acl
{
  public class FieldAccessAclDbAccessor
  {
    private const string className = "FieldAccessAclDbAccessor�";
    private const string tableName = "[Acl_FieldAccess]�";
    private const string tableName_User = "[Acl_FieldAccess_User]�";

    private FieldAccessAclDbAccessor()
    {
    }

    public static void SetFieldPermission(int personaID, string fieldID, AclTriState access)
    {
      FieldAccessAclDbAccessor.SetFieldsPermission(personaID, new Dictionary<string, AclTriState>()
      {
        {
          fieldID,
          access
        }
      });
    }

    public static void SetFieldsPermission(int personaID, Dictionary<string, AclTriState> fieldList)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from [Acl_FieldAccess] where personaID = " + (object) personaID);
      foreach (string key in fieldList.Keys)
      {
        AclTriState field = fieldList[key];
        if (field != AclTriState.Unspecified)
          dbQueryBuilder.AppendLine("insert into [Acl_FieldAccess] (personaID, fieldID, access) values (" + (object) personaID + ", " + SQL.EncodeString(key) + ", " + (object) (int) field + ")");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    [PgReady]
    public static AclTriState GetFieldPermission(int personaID, string fieldID, bool filterAccess)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("Select access from [Acl_FieldAccess] where personaID = " + (object) personaID + " and fieldDBName = " + SQL.EncodeString(fieldID));
        object obj = pgDbQueryBuilder.ExecuteScalar();
        if (obj == null)
          return AclTriState.Unspecified;
        return SQL.DecodeInt(obj) != 1 ? AclTriState.False : AclTriState.True;
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select access from [Acl_FieldAccess] where personaID = " + (object) personaID + " and fieldDBName = " + SQL.EncodeString(fieldID));
      object obj1 = dbQueryBuilder.ExecuteScalar();
      if (obj1 == null)
        return AclTriState.Unspecified;
      return (byte) obj1 != (byte) 1 ? AclTriState.False : AclTriState.True;
    }

    [PgReady]
    public static Dictionary<string, AclTriState> GetFieldsPermission(int personaID)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        Dictionary<string, AclTriState> fieldsPermission = new Dictionary<string, AclTriState>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
        pgDbQueryBuilder.AppendLine("Select * from [Acl_FieldAccess] where personaID = " + (object) personaID);
        DataTable dataTable = pgDbQueryBuilder.ExecuteTableQuery();
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
          foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          {
            if (!fieldsPermission.ContainsKey(string.Concat(row["fieldID"])))
              fieldsPermission.Add(string.Concat(row["fieldID"]), SQL.DecodeInt(row["access"]) == 1 ? AclTriState.True : AclTriState.False);
          }
        }
        return fieldsPermission;
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Pipeline);
      Dictionary<string, AclTriState> fieldsPermission1 = new Dictionary<string, AclTriState>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      dbQueryBuilder.AppendLine("Select * from [Acl_FieldAccess] where personaID = " + (object) personaID);
      DataTable dataTable1 = dbQueryBuilder.ExecuteTableQuery();
      if (dataTable1 != null && dataTable1.Rows.Count > 0)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataTable1.Rows)
        {
          if (!fieldsPermission1.ContainsKey(string.Concat(row["fieldID"])))
            fieldsPermission1.Add(string.Concat(row["fieldID"]), (byte) row["access"] == (byte) 1 ? AclTriState.True : AclTriState.False);
        }
      }
      return fieldsPermission1;
    }

    public static Dictionary<string, AclTriState> GetFieldsPermission(int[] personaIDs)
    {
      Dictionary<string, AclTriState> fieldsPermission1 = new Dictionary<string, AclTriState>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      foreach (int personaId in personaIDs)
      {
        Dictionary<string, AclTriState> fieldsPermission2 = FieldAccessAclDbAccessor.GetFieldsPermission(personaId);
        if (fieldsPermission1 == null || fieldsPermission1.Count == 0)
          fieldsPermission1 = fieldsPermission2;
        else if (fieldsPermission2 != null && fieldsPermission2.Count > 0)
        {
          string[] array = new string[fieldsPermission1.Count];
          fieldsPermission1.Keys.CopyTo(array, 0);
          foreach (string key in array)
          {
            if (fieldsPermission1[key] != AclTriState.True && fieldsPermission2.ContainsKey(key) && fieldsPermission2[key] == AclTriState.True)
              fieldsPermission1[key] = AclTriState.True;
          }
        }
      }
      return fieldsPermission1;
    }

    public static void AddFieldPermissionToAllPersonas(string[] fields)
    {
      AclTriState aclTriState = AclTriState.True;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (Persona allPersona in PersonaAccessor.GetAllPersonas())
      {
        foreach (string field in fields)
        {
          dbQueryBuilder.AppendLine("if not exists (select * from [Acl_FieldAccess] where personaID = " + (object) allPersona.ID + " and fieldID = " + SQL.EncodeString(field) + ")");
          dbQueryBuilder.AppendLine("insert into [Acl_FieldAccess] (personaID, fieldID, access) values (" + (object) allPersona.ID + ", " + SQL.EncodeString(field) + ", " + (object) (int) aclTriState + ")");
          dbQueryBuilder.AppendLine("Else");
          dbQueryBuilder.AppendLine("Update [Acl_FieldAccess] SET access = " + (object) (int) aclTriState + " WHERE personaID = " + (object) allPersona.ID + " and fieldID = " + SQL.EncodeString(field));
        }
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetFieldPermission(string userID, string fieldID, AclTriState access)
    {
      FieldAccessAclDbAccessor.SetFieldsPermission(userID, new Dictionary<string, AclTriState>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase)
      {
        {
          fieldID,
          access
        }
      });
    }

    public static void SetFieldsPermission(string userId, Dictionary<string, AclTriState> fieldList)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from [Acl_FieldAccess_User] where userID = " + SQL.EncodeString(userId));
      foreach (string key in fieldList.Keys)
      {
        AclTriState field = fieldList[key];
        if (field != AclTriState.Unspecified)
          dbQueryBuilder.AppendLine("insert into [Acl_FieldAccess_User] (userID, fieldID, access) values (" + SQL.EncodeString(userId) + ", " + SQL.EncodeString(key) + ", " + (object) (int) field + ")");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    [PgReady]
    public static AclTriState GetFieldPermission(string userID, string fieldID)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("Select access from [Acl_FieldAccess_User] where userID = " + SQL.EncodeString(userID) + " and fieldID = " + SQL.EncodeString(fieldID));
        object obj = pgDbQueryBuilder.ExecuteScalar();
        if (obj == null)
          return AclTriState.Unspecified;
        return SQL.DecodeInt(obj) != 1 ? AclTriState.False : AclTriState.True;
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select access from [Acl_FieldAccess_User] where userID = " + SQL.EncodeString(userID) + " and fieldID = " + SQL.EncodeString(fieldID));
      object obj1 = dbQueryBuilder.ExecuteScalar();
      if (obj1 == null)
        return AclTriState.Unspecified;
      return (byte) obj1 != (byte) 1 ? AclTriState.False : AclTriState.True;
    }

    [PgReady]
    public static Dictionary<string, AclTriState> GetFieldsPermission(string userID)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        Dictionary<string, AclTriState> fieldsPermission = new Dictionary<string, AclTriState>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
        pgDbQueryBuilder.AppendLine("Select * from [Acl_FieldAccess_User] where userID = @userid");
        DataTable dataTable = pgDbQueryBuilder.ExecuteTableQuery(new DbCommandParameter("userid", (object) userID.TrimEnd(), DbType.AnsiString).ToArray());
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
          foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          {
            if (!fieldsPermission.ContainsKey(string.Concat(row["fieldID"])))
              fieldsPermission.Add(string.Concat(row["fieldID"]), SQL.DecodeInt(row["access"]) == 1 ? AclTriState.True : AclTriState.False);
          }
        }
        return fieldsPermission;
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Pipeline);
      Dictionary<string, AclTriState> fieldsPermission1 = new Dictionary<string, AclTriState>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      dbQueryBuilder.AppendLine("Select * from [Acl_FieldAccess_User] where userID = " + SQL.EncodeString(userID));
      DataTable dataTable1 = dbQueryBuilder.ExecuteTableQuery();
      if (dataTable1 != null && dataTable1.Rows.Count > 0)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataTable1.Rows)
        {
          if (!fieldsPermission1.ContainsKey(string.Concat(row["fieldID"])))
            fieldsPermission1.Add(string.Concat(row["fieldID"]), (byte) row["access"] == (byte) 1 ? AclTriState.True : AclTriState.False);
        }
      }
      return fieldsPermission1;
    }

    public static void SyncFieldList(
      string[] newFields,
      string[] removeFields,
      bool newFieldAccess)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (removeFields != null && removeFields.Length != 0)
      {
        dbQueryBuilder.AppendLine("Delete Acl_FieldAccess where fieldID in (" + SQL.Encode((object) removeFields) + ")");
        dbQueryBuilder.AppendLine("Delete Acl_FieldAccess_User where fieldID in (" + SQL.Encode((object) removeFields) + ")");
      }
      foreach (string newField in newFields)
        dbQueryBuilder.AppendLine("Insert into Acl_FieldAccess select p.personaId, " + SQL.EncodeString(newField) + ", " + (object) (newFieldAccess ? 1 : 0) + " from Personas p left outer join Acl_FieldAccess afa on p.personaID = afa.PersonaID and afa.fieldID = " + SQL.EncodeString(newField) + " where p.personaID > 0 and afa.PersonaID is NULL");
      dbQueryBuilder.AppendLine("Update Acl_FieldAccess set access = 1 where personaID = 1");
      dbQueryBuilder.ExecuteNonQuery(TimeSpan.FromMinutes(10.0), DbTransactionType.Default);
    }

    public static void DuplicateFieldAccess(int sourcePersonaID, int desPersonaID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from Acl_FieldAccess where personaID = " + (object) desPersonaID);
      dbQueryBuilder.AppendLine("insert into Acl_FieldAccess select " + (object) desPersonaID + ", fieldID, access from Acl_FieldAccess where personaID = " + (object) sourcePersonaID);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void InitialColumnSync(int personaID, bool accessRight)
    {
      FieldAccessAclDbAccessor.DuplicateFieldAccess(1, personaID);
      if (accessRight)
        return;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Update [Acl_FieldAccess] set access = 0 where personaID=" + (object) personaID);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void UpdateFeeManagementPermission(
      FeeManagementPersonaInfo feeManagementPersonaInfo)
    {
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        foreach (Persona allPersona in PersonaAccessor.GetAllPersonas())
        {
          FeeManagementPersonaRights personaRights = feeManagementPersonaInfo.GetPersonaRights(allPersona.ID);
          if (personaRights != null)
          {
            dbQueryBuilder.AppendLine("IF NOT EXISTS (SELECT * FROM [Acl_FeeManagementAccess] WHERE [personaID] = " + (object) allPersona.ID + ")");
            dbQueryBuilder.AppendLine("INSERT INTO [Acl_FeeManagementAccess] (personaID, overwrite700, overwrite800, overwrite900, overwrite1000, overwrite1100, overwrite1200, overwrite1300, overwritePC) values (" + (object) allPersona.ID + ", " + (personaRights.Overwrite700 ? (object) "1" : (object) "0") + ", " + (personaRights.Overwrite800 ? (object) "1" : (object) "0") + ", " + (personaRights.Overwrite900 ? (object) "1" : (object) "0") + ", " + (personaRights.Overwrite1000 ? (object) "1" : (object) "0") + ", " + (personaRights.Overwrite1100 ? (object) "1" : (object) "0") + ", " + (personaRights.Overwrite1200 ? (object) "1" : (object) "0") + ", " + (personaRights.Overwrite1300 ? (object) "1" : (object) "0") + ", " + (personaRights.OverwritePC ? (object) "1" : (object) "0") + ")");
            dbQueryBuilder.AppendLine("ELSE");
            dbQueryBuilder.AppendLine("UPDATE [Acl_FeeManagementAccess] SET overwrite700 = " + (personaRights.Overwrite700 ? (object) "1" : (object) "0") + ", overwrite800 = " + (personaRights.Overwrite800 ? (object) "1" : (object) "0") + ", overwrite900 = " + (personaRights.Overwrite900 ? (object) "1" : (object) "0") + ", overwrite1000 = " + (personaRights.Overwrite1000 ? (object) "1" : (object) "0") + ", overwrite1100 = " + (personaRights.Overwrite1100 ? (object) "1" : (object) "0") + ", overwrite1200 = " + (personaRights.Overwrite1200 ? (object) "1" : (object) "0") + ", overwrite1300 = " + (personaRights.Overwrite1300 ? (object) "1" : (object) "0") + ", overwritePC = " + (personaRights.OverwritePC ? (object) "1" : (object) "0") + " WHERE [personaID] = " + (object) allPersona.ID);
          }
        }
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (FieldAccessAclDbAccessor), "Fee List Persona info cannot be updated. Error: " + ex.Message);
        Err.Reraise(nameof (FieldAccessAclDbAccessor), ex);
      }
    }

    public static FeeManagementPersonaInfo GetFeeManagementPermission(int[] personIDs)
    {
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("SELECT afma.*, p.personaName FROM [Acl_FeeManagementAccess] afma left join [Personas] p on p.personaID = afma.personaId");
        List<int> intList = (List<int>) null;
        if (personIDs != null)
          intList = new List<int>((IEnumerable<int>) personIDs);
        DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
        FeeManagementPersonaInfo managementPermission = new FeeManagementPersonaInfo();
        if (intList != null && intList.Contains(0))
          managementPermission.AddPersonaRights(new FeeManagementPersonaRights(0, Persona.SuperAdministrator.Name, true, true, true, true, true, true, true, true));
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
          foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          {
            int personaID = SQL.DecodeInt(row["personaID"], -1);
            if (personaID != -1 && (intList == null || intList.Contains(personaID) && !intList.Contains(0)))
              managementPermission.AddPersonaRights(new FeeManagementPersonaRights(personaID, SQL.DecodeString(row["personaName"], (string) null), SQL.DecodeBoolean(row["overwrite700"], false), SQL.DecodeBoolean(row["overwrite800"], false), SQL.DecodeBoolean(row["overwrite900"], false), SQL.DecodeBoolean(row["overwrite1000"], false), SQL.DecodeBoolean(row["overwrite1100"], false), SQL.DecodeBoolean(row["overwrite1200"], false), SQL.DecodeBoolean(row["overwrite1300"], false), SQL.DecodeBoolean(row["overwritePC"], false)));
          }
        }
        TraceLog.WriteVerbose(nameof (FieldAccessAclDbAccessor), "Retrieved Fee List Persona info");
        return managementPermission;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FieldAccessAclDbAccessor), ex);
        return (FeeManagementPersonaInfo) null;
      }
    }
  }
}
