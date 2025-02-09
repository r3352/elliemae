// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Acl.Interfaces.IMilestonesFreeRoleAclManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Acl.Interfaces
{
  public interface IMilestonesFreeRoleAclManager
  {
    Hashtable GetPermissions(int personaID);

    bool GetPermission(int roleID, int personaID);

    bool GetPermission(int roleID, Persona[] personaList);

    bool GetPermission(int roleID, UserInfo userInfo);

    Hashtable GetPersonalPermissions(UserInfo userInfo);

    void SetPermission(int roleID, string userid, object access);

    void SetPermission(int roleID, int personaID, bool access);

    void DuplicateACLMilestonesFreeRole(int sourcePersonaID, int desPersonaID);

    void SynchronizeAdminRight();
  }
}
