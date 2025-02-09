// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Acl.InputFormsAclDbAccessor
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
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Acl
{
  public class InputFormsAclDbAccessor
  {
    private const string tableName = "[Acl_InputForms]�";
    private const string tableName_User = "[Acl_InputForms_User]�";

    private InputFormsAclDbAccessor()
    {
    }

    public static InputFormInfo[] GetAccessibleForms(string userId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select form.* from InputForms form, [Acl_InputForms_User] formAcl where form.FormID = formAcl.formID and formAcl.access = 1 and formAcl.userid = " + SQL.Encode((object) userId));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      ArrayList arrayList = new ArrayList();
      foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
        arrayList.Add((object) InputForms.DataRowToInputFormInfo(r));
      return (InputFormInfo[]) arrayList.ToArray(typeof (InputFormInfo));
    }

    public static InputFormInfo[] GetAccessibleForms(Persona persona)
    {
      return InputFormsAclDbAccessor.GetAccessibleForms(persona.ID);
    }

    public static InputFormInfo[] GetAccessibleForms(int personaID)
    {
      return InputFormsAclDbAccessor.GetAccessibleForms(new int[1]
      {
        personaID
      });
    }

    public static InputFormInfo[] GetAccessibleForms(Persona[] personas)
    {
      return InputFormsAclDbAccessor.GetAccessibleForms(AclUtils.GetPersonaIDs(personas));
    }

    public static InputFormInfo[] GetAccessibleForms(int[] personaIDs)
    {
      if (personaIDs.Length == 0)
        return new InputFormInfo[0];
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select form.* from InputForms form, [Acl_InputForms] formAcl where form.FormID = formAcl.formID and formAcl.access = 1 and formAcl.personaID in (" + SQL.EncodeArray((Array) personaIDs) + ") order by form.FormOrder");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      ArrayList arrayList = new ArrayList();
      foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
        arrayList.Add((object) InputForms.DataRowToInputFormInfo(r));
      return (InputFormInfo[]) arrayList.ToArray(typeof (InputFormInfo));
    }

    public static AclTriState GetPermission(string form, string userid)
    {
      return (AclTriState) InputFormsAclDbAccessor.GetPermissions(new string[1]
      {
        form
      }, userid)[(object) form];
    }

    public static Hashtable GetPermissions(string[] forms, string userid)
    {
      Hashtable permissions = new Hashtable();
      if (forms.Length == 0)
        return permissions;
      foreach (string form in forms)
        permissions[(object) form] = (object) AclTriState.Unspecified;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select formID, access from [Acl_InputForms_User] where formID in (" + SQL.Encode((object) forms) + ") and userid = " + SQL.Encode((object) userid));
      foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
      {
        string key = (string) dataRow["formID"];
        AclTriState aclTriState = AclTriState.Unspecified;
        object obj = SQL.Decode(dataRow["access"], (object) null);
        if (obj != null)
          aclTriState = (byte) obj == (byte) 1 ? AclTriState.True : AclTriState.False;
        permissions[(object) key] = (object) aclTriState;
      }
      return permissions;
    }

    public static bool GetPermission(string form, Persona persona)
    {
      return InputFormsAclDbAccessor.GetPermission(form, persona.ID);
    }

    public static bool GetPermission(string form, int personaID)
    {
      return InputFormsAclDbAccessor.GetPermission(form, new int[1]
      {
        personaID
      });
    }

    public static bool GetPermission(string form, Persona[] personas)
    {
      return InputFormsAclDbAccessor.GetPermission(form, AclUtils.GetPersonaIDs(personas));
    }

    public static bool GetPermission(string form, int[] personaIDs)
    {
      return (bool) InputFormsAclDbAccessor.GetPermissions(new string[1]
      {
        form
      }, personaIDs)[(object) form];
    }

    public static Hashtable GetPermissions(string[] forms, Persona[] personas)
    {
      return InputFormsAclDbAccessor.GetPermissions(forms, AclUtils.GetPersonaIDs(personas));
    }

    public static Hashtable GetPermissions(string[] forms, int[] personaIDs)
    {
      Hashtable permissions = new Hashtable();
      foreach (string form in forms)
        permissions[(object) form] = (object) false;
      if (personaIDs.Length == 0 || forms.Length == 0)
        return permissions;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select formID, max(access) as maxaccess from [Acl_InputForms]");
      dbQueryBuilder.AppendLine("  where formID in (" + SQL.EncodeArray((Array) forms) + ")");
      dbQueryBuilder.AppendLine("    and personaID in (" + SQL.EncodeArray((Array) personaIDs) + ")");
      dbQueryBuilder.AppendLine("  group by formID");
      foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        permissions[(object) string.Concat(dataRow["formID"])] = (object) ((byte) dataRow["maxaccess"] > (byte) 0);
      return permissions;
    }

    public static Hashtable GetPermissionsForAllForms(Persona persona)
    {
      return InputFormsAclDbAccessor.GetPermissionsForAllForms(persona.ID);
    }

    public static Hashtable GetPermissionsForAllForms(int personaID)
    {
      return InputFormsAclDbAccessor.GetPermissionsForAllForms(new int[1]
      {
        personaID
      });
    }

    public static Hashtable GetPermissionsForAllForms(Persona[] personas)
    {
      return InputFormsAclDbAccessor.GetPermissionsForAllForms(AclUtils.GetPersonaIDs(personas));
    }

    public static Hashtable GetPermissionsForAllForms(int[] personaIDs)
    {
      if (personaIDs.Length == 0)
        return new Hashtable();
      Hashtable permissionsForAllForms = new Hashtable();
      if (Array.Exists<int>(personaIDs, (Predicate<int>) (element => element == Persona.Administrator.ID)) || Array.Exists<int>(personaIDs, (Predicate<int>) (element => element == Persona.SuperAdministrator.ID)))
        return InputFormsAclDbAccessor.GetAdministratorPermissions();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from [Acl_InputForms] where personaID in (" + SQL.EncodeArray((Array) personaIDs) + ")");
      foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
      {
        string key = (string) dataRow["formID"];
        bool flag = (byte) dataRow["access"] == (byte) 1;
        if (!permissionsForAllForms.ContainsKey((object) key) || !(bool) permissionsForAllForms[(object) key])
          permissionsForAllForms[(object) key] = (object) flag;
      }
      return permissionsForAllForms;
    }

    public static Hashtable GetPermissionsForAllForms(string userid)
    {
      Hashtable permissionsForAllForms = new Hashtable();
      UserInfo userById = User.GetUserById(userid);
      if (userById.IsAdministrator() || userById.IsSuperAdministrator())
        return InputFormsAclDbAccessor.GetAdministratorPermissions();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Login);
      dbQueryBuilder.AppendLine("select * from [Acl_InputForms_User] where userid = " + SQL.Encode((object) userid));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null)
        return (Hashtable) null;
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        string key = (string) dataRow["formID"];
        bool flag = (byte) dataRow["access"] == (byte) 1;
        permissionsForAllForms[(object) key] = (object) flag;
      }
      return permissionsForAllForms;
    }

    public static Hashtable GetPermissions(string form, Persona[] personas)
    {
      return InputFormsAclDbAccessor.GetPermissions(form, AclUtils.GetPersonaIDs(personas));
    }

    public static Hashtable GetPermissions(string form, int[] personaIDs)
    {
      if (personaIDs.Length == 0)
        return new Hashtable();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Login);
      dbQueryBuilder.AppendLine("select * from [Acl_InputForms] where formID = " + SQL.Encode((object) form) + " and personaID in (" + SQL.EncodeArray((Array) personaIDs) + ")");
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

    public static void SetPermission(string form, string userid, object access)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from [Acl_InputForms_User] where formID = " + SQL.Encode((object) form) + " and userid = " + SQL.Encode((object) userid));
      if (access != null)
        dbQueryBuilder.AppendLine("insert into [Acl_InputForms_User] (formID, userid, access) values (" + SQL.Encode((object) form) + ", " + SQL.Encode((object) userid) + ", " + ((bool) access ? "1" : "0") + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetPermission(string[] forms, int personaID, bool[] accesses)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      for (int index = 0; index < forms.Length; ++index)
      {
        string form = forms[index];
        bool access = accesses[index];
        if (dbQueryBuilder.Length > 0)
          dbQueryBuilder.AppendLine("");
        string str = "where formID = " + SQL.Encode((object) form) + " and personaID = " + (object) personaID;
        dbQueryBuilder.AppendLine("if exists (select * from Acl_InputForms " + str + ")");
        dbQueryBuilder.AppendLine("update Acl_InputForms set access = " + (access ? "1" : "0") + " " + str);
        dbQueryBuilder.AppendLine("else");
        dbQueryBuilder.AppendLine("insert into [Acl_InputForms] (formID, personaID, access) values (" + SQL.Encode((object) form) + ", " + (object) personaID + ", " + (access ? (object) "1" : (object) "0") + ")");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetPermission(string form, int personaID, bool access)
    {
      InputFormsAclDbAccessor.SetPermission(new string[1]
      {
        form
      }, personaID, new bool[1]{ access });
    }

    public static void DuplicateACLInputForms(int sourcePersonaID, int desPersonaID)
    {
      string str1 = "";
      string str2 = "";
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from [Acl_InputForms] where personaID = " + (object) sourcePersonaID);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      dbQueryBuilder.Reset();
      DataColumnCollection columns = dataTable.Columns;
      if (dataTable == null || dataTable.Rows.Count <= 0)
        return;
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        string str3 = "Insert into [Acl_InputForms] (";
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

    public static void SynchrnizeAdminAccessRight()
    {
      InputFormInfo[] formInfos = InputForms.GetFormInfos(InputFormType.All, InputFormCategory.Form);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from [Acl_InputForms] where personaID = " + (object) 1);
      foreach (InputFormInfo inputFormInfo in formInfos)
        dbQueryBuilder.AppendLine("insert into [Acl_InputForms] (formID, personaID, access) values (" + SQL.Encode((object) inputFormInfo.FormID) + ", " + (object) 1 + ", 1)");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static InputFormInfo[] GetAccessibleForms(UserInfo userInfo)
    {
      return userInfo.IsSuperAdministrator() ? InputForms.GetAllFormInfos() : InputFormsAclDbAccessor.GetAccessibleForms(userInfo.Userid, userInfo.UserPersonas, false);
    }

    public static InputFormInfo[] GetAccessibleForms(
      string userId,
      Persona[] personas,
      bool useReadReplica)
    {
      InputFormInfo[] accessibleForms1 = InputFormsAclDbAccessor.GetAccessibleForms(personas);
      Hashtable permissionsForAllForms = InputFormsAclDbAccessor.GetPermissionsForAllForms(userId);
      Set set = (Set) new ListSet();
      set.AddAll((ICollection) accessibleForms1);
      if (permissionsForAllForms.Count > 0)
      {
        string[] formIds = new string[permissionsForAllForms.Count];
        permissionsForAllForms.Keys.CopyTo((Array) formIds, 0);
        foreach (InputFormInfo formInfo in InputForms.GetFormInfos(formIds))
        {
          if ((bool) permissionsForAllForms[(object) formInfo.FormID] || UserInfo.IsSuperAdministrator(userId, personas))
          {
            if (!set.Contains((object) formInfo))
              set.Add((object) formInfo);
          }
          else if (set.Contains((object) formInfo))
            set.Remove((object) formInfo);
        }
      }
      InputFormInfo[] accessibleForms2 = new InputFormInfo[set.Count];
      set.CopyTo((Array) accessibleForms2, 0);
      return accessibleForms2;
    }

    public static bool CheckPermission(string form, UserInfo userInfo)
    {
      if (userInfo.IsSuperAdministrator() || Company.GetCurrentEdition() == EncompassEdition.Broker)
        return true;
      object permission = (object) InputFormsAclDbAccessor.GetPermission(form, userInfo.Userid);
      if (permission != null)
        return (bool) permission;
      Hashtable permissions = InputFormsAclDbAccessor.GetPermissions(form, userInfo.UserPersonas);
      foreach (int key in (IEnumerable) permissions.Keys)
      {
        if ((bool) permissions[(object) key])
          return true;
      }
      return false;
    }

    public static Hashtable CheckPermissions(string[] forms, UserInfo userInfo)
    {
      if (userInfo.IsSuperAdministrator() || Company.GetCurrentEdition() == EncompassEdition.Broker)
      {
        Hashtable hashtable = new Hashtable();
        foreach (string form in forms)
          hashtable[(object) form] = (object) true;
        return hashtable;
      }
      Hashtable permissions1 = InputFormsAclDbAccessor.GetPermissions(forms, userInfo.Userid);
      Hashtable permissions2 = InputFormsAclDbAccessor.GetPermissions(forms, userInfo.UserPersonas);
      for (int index = 0; index < forms.Length; ++index)
      {
        AclTriState aclTriState = (AclTriState) permissions1[(object) forms[index]];
        if (aclTriState == AclTriState.Unspecified)
          permissions1[(object) forms[index]] = (object) (bool) permissions2[(object) forms[index]];
        else
          permissions1[(object) forms[index]] = (object) (aclTriState == AclTriState.True);
      }
      return permissions1;
    }

    private static Hashtable GetAdministratorPermissions()
    {
      Hashtable administratorPermissions = new Hashtable();
      foreach (InputFormInfo allFormInfo in InputForms.GetAllFormInfos())
        administratorPermissions[(object) allFormInfo.FormID] = (object) true;
      return administratorPermissions;
    }
  }
}
