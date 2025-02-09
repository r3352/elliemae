// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Acl.StandardWebFormsAclDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Acl
{
  public class StandardWebFormsAclDbAccessor
  {
    private const string tableName = "[Acl_StandardWebForms]�";
    private const string tableName_User = "[Acl_StandardWebForms_User]�";

    public static List<StandardWebFormInfo> GetFormsByPersona(int personaID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select swf.Id as formid, swf.[name] as formname, coalesce(ac.access, 0) as [Access] ");
      dbQueryBuilder.AppendLine(" from StandardWebForms swf ");
      dbQueryBuilder.AppendLine(" left outer join Acl_StandardWebForms ac ");
      dbQueryBuilder.AppendLine(" on swf.ID = ac.FormID and ac.PersonaID = " + SQL.Encode((object) personaID));
      dbQueryBuilder.AppendLine(" where swf.Active = 1 ");
      dbQueryBuilder.AppendLine(" order by swf.ShowOrder ");
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      List<StandardWebFormInfo> formsByPersona = new List<StandardWebFormInfo>();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        formsByPersona.Add(new StandardWebFormInfo()
        {
          FormID = SQL.DecodeInt(row["FormID"]),
          FormName = SQL.DecodeString(row["FormName"]),
          Access = SQL.DecodeBoolean(row["Access"])
        });
      return formsByPersona;
    }

    public static List<StandardWebFormInfo> GetActiveForms()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select [ID] as [FormID], [name] as [FormName], 0 as [Access] ");
      dbQueryBuilder.AppendLine(" from StandardWebForms ");
      dbQueryBuilder.AppendLine(" where active=1 ");
      dbQueryBuilder.AppendLine(" order by showorder ");
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      List<StandardWebFormInfo> activeForms = new List<StandardWebFormInfo>();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        activeForms.Add(new StandardWebFormInfo()
        {
          FormID = SQL.DecodeInt(row["FormID"]),
          FormName = SQL.DecodeString(row["FormName"]),
          Access = SQL.DecodeBoolean(row["Access"])
        });
      return activeForms;
    }

    public static List<StandardWebFormInfo> GetFormsByUser(string userid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select forms.formid as [FormID], forms.formname as [FormName], coalesce(users.access, personas.access, forms.[access]) as [Access] ");
      dbQueryBuilder.AppendLine(" from(select[Id] as formid, [name] as formname, showorder, 0 as [access] from StandardWebForms where active = 1) as forms ");
      dbQueryBuilder.AppendLine(" left join (select ac.FormID as formid, ac.access as [access] from StandardWebForms swf inner join Acl_StandardWebForms ac on swf.ID = ac.FormID inner join UserPersona up on ac.PersonaID = up.personaID where swf.Active = 1 and up.userid = " + SQL.EncodeString(userid) + " ) as personas on forms.formid = personas.formid ");
      dbQueryBuilder.AppendLine(" left join (select acu.FormID as [FormID], acu.access as [Access] from StandardWebForms swf inner join Acl_StandardWebForms_User acu on swf.ID = acu.FormID where swf.active = 1 and acu.userid = " + SQL.EncodeString(userid) + ") as users on forms.formid = users.FormID ");
      dbQueryBuilder.AppendLine(" order by forms.ShowOrder");
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      List<StandardWebFormInfo> formsByUser = new List<StandardWebFormInfo>();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        formsByUser.Add(new StandardWebFormInfo()
        {
          FormID = SQL.DecodeInt(row["FormID"]),
          FormName = SQL.DecodeString(row["FormName"]),
          Access = SQL.DecodeBoolean(row["Access"])
        });
      return formsByUser;
    }

    public static Hashtable GetPermissionsByPersonas(int[] personaIDs)
    {
      Hashtable permissionsByPersonas = new Hashtable();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from [Acl_StandardWebForms] where personaID in (" + SQL.EncodeArray((Array) personaIDs) + ")");
      foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
      {
        bool flag = SQL.DecodeBoolean(dataRow["Access"]);
        int key = SQL.DecodeInt(dataRow["FormID"]);
        if (!permissionsByPersonas.ContainsKey((object) key))
          permissionsByPersonas.Add((object) key, (object) flag);
      }
      return permissionsByPersonas;
    }

    public static Hashtable GetPermissionsByUser(string userid)
    {
      Hashtable permissionsByUser = new Hashtable();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from [Acl_StandardWebForms_User] where userid = " + SQL.EncodeString(userid));
      foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
      {
        bool flag = SQL.DecodeBoolean(dataRow["Access"]);
        int key = SQL.DecodeInt(dataRow["FormID"]);
        permissionsByUser.Add((object) key, (object) flag);
      }
      return permissionsByUser;
    }

    public static void SetPermissions(StandardWebFormInfo[] forms, int personaID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (StandardWebFormInfo form in forms)
      {
        int formId = form.FormID;
        bool access = form.Access;
        string str = "where formid = " + SQL.Encode((object) formId) + " and personaID = " + (object) personaID;
        dbQueryBuilder.AppendLine("if exists (select 1 from Acl_StandardWebForms " + str + ")");
        dbQueryBuilder.AppendLine("update Acl_StandardWebForms set access = " + (access ? "1" : "0") + ", lastmodifiedby = " + SQL.Encode((object) form.LastModifiedBy) + ", lastmodifieddate= getdate() " + str);
        dbQueryBuilder.AppendLine("else");
        dbQueryBuilder.AppendLine("insert into [Acl_StandardWebForms] (FormID, PersonaID, Access, createdby, createddate, lastmodifiedby, lastmodifieddate) values (" + SQL.Encode((object) formId) + ", " + (object) personaID + ", " + (access ? (object) "1" : (object) "0") + ",  " + SQL.Encode((object) form.LastModifiedBy) + ", getdate(), " + SQL.Encode((object) form.LastModifiedBy) + ", getdate())");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetPermissions(StandardWebFormInfo[] forms, string userid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (StandardWebFormInfo form in forms)
      {
        int formId = form.FormID;
        bool access = form.Access;
        string str = "where formid = " + SQL.Encode((object) formId) + " and userID = " + SQL.Encode((object) userid);
        dbQueryBuilder.AppendLine("if exists (select 1 from [Acl_StandardWebForms_User]" + str + ")");
        dbQueryBuilder.AppendLine("update Acl_StandardWebForms_User set access = " + (access ? "1" : "0") + ", lastmodifiedby = " + SQL.Encode((object) form.LastModifiedBy) + ", lastmodifieddate= getdate() " + str);
        dbQueryBuilder.AppendLine("else");
        dbQueryBuilder.AppendLine("insert into [Acl_StandardWebForms_User] (FormID, UserID, Access, createdby, createddate, lastmodifiedby, lastmodifieddate) values (" + SQL.Encode((object) formId) + ", " + SQL.Encode((object) userid) + ", " + (access ? "1" : "0") + ",  " + SQL.Encode((object) form.LastModifiedBy) + ", getdate(), " + SQL.Encode((object) form.LastModifiedBy) + ", getdate())");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DuplicateACLStandardWebForms(
      int sourcePersonaID,
      int desPersonaID,
      string userid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from [Acl_StandardWebForms] where personaID = " + (object) sourcePersonaID);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      dbQueryBuilder.Reset();
      DataColumnCollection columns = dataTable.Columns;
      if (dataTable == null || dataTable.Rows.Count <= 0)
        return;
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        bool flag = SQL.DecodeBoolean(row["Access"]);
        dbQueryBuilder.AppendLine("Insert into [Acl_StandardWebForms](FormID, PersonaID, Access, LastModifiedDate, LastModifiedBy, CreatedDate, CreatedBy) ");
        dbQueryBuilder.AppendLine("values(" + SQL.Encode(row["FormID"]) + ", " + SQL.Encode((object) desPersonaID) + ", " + (object) (flag ? 1 : 0) + ", getdate(), " + SQL.EncodeString(userid) + ", getdate(), " + SQL.EncodeString(userid) + ")");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }
  }
}
