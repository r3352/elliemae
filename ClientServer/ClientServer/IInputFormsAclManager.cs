// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IInputFormsAclManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IInputFormsAclManager
  {
    InputFormInfo[] GetFormInfos(InputFormType formType);

    InputFormInfo[] GetAccessibleForms();

    InputFormInfo[] GetAccessibleForms(int personaID);

    InputFormInfo[] GetAccessibleForms(string userId);

    InputFormInfo[] GetAccessibleForms(int[] personaIDs);

    InputFormInfo[] GetAccessibleForms(Persona[] personas);

    InputFormInfo[] GetAccessibleForms(string userId, Persona[] personas);

    InputFormInfo[] GetAccessibleForms(string userId, Persona[] personas, bool useReadReplica);

    AclTriState GetPermission(string form, string userid);

    bool GetPermission(string form, int personaID);

    Hashtable GetPermissions(string form, int[] personaIDs);

    Hashtable GetPermissions(string form, Persona[] personas);

    Hashtable GetPermissionsForAllForms(string userid);

    Hashtable GetPermissionsForAllForms(int personaID);

    Hashtable GetPermissionsForAllForms(int[] personaIDs);

    Hashtable GetPermissionsForAllForms(Persona[] personas);

    void SetPermission(string form, string userid, object access);

    void SetPermission(string[] forms, int personaID, bool[] accesses);

    void SetPermission(string form, int personaID, bool access);

    bool CheckPermission(string form);

    Hashtable CheckPermissions(string[] form);

    void DuplicateACLInputForms(int sourcePersonaID, int desPersonaID);
  }
}
