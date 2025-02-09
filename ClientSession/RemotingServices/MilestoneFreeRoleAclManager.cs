// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.MilestoneFreeRoleAclManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Acl.Interfaces;
using EllieMae.EMLite.Common;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class MilestoneFreeRoleAclManager
  {
    private IMilestonesFreeRoleAclManager milestoneFreeRoleAclManager;

    public MilestoneFreeRoleAclManager(Sessions.Session session)
    {
      this.milestoneFreeRoleAclManager = (IMilestonesFreeRoleAclManager) session.GetAclManager(AclCategory.MilestonesFreeRole);
    }

    public bool GetPermission(int roleID, int personaID)
    {
      return this.milestoneFreeRoleAclManager.GetPermission(roleID, personaID);
    }

    public Hashtable GetPermissions(int personaID)
    {
      return this.milestoneFreeRoleAclManager.GetPermissions(personaID);
    }

    public bool GetPermission(int roleID, Persona[] personaList)
    {
      return this.milestoneFreeRoleAclManager.GetPermission(roleID, personaList);
    }

    public bool GetPermission(int roleID, UserInfo userInfo)
    {
      return this.milestoneFreeRoleAclManager.GetPermission(roleID, userInfo);
    }

    public void SetPermission(int roleID, string userid, object access)
    {
      this.milestoneFreeRoleAclManager.SetPermission(roleID, userid, access);
    }

    public void SetPermission(int roleID, int personaID, bool access)
    {
      this.milestoneFreeRoleAclManager.SetPermission(roleID, personaID, access);
    }

    public void DuplicateACLMilestonesFreeRole(int sourcePersonaID, int desPersonaID)
    {
      this.milestoneFreeRoleAclManager.DuplicateACLMilestonesFreeRole(sourcePersonaID, desPersonaID);
    }

    public Hashtable GetPersonalPermissions(UserInfo userInfo)
    {
      return this.milestoneFreeRoleAclManager.GetPersonalPermissions(userInfo);
    }

    public void SynchronizeAdminRight() => this.milestoneFreeRoleAclManager.SynchronizeAdminRight();
  }
}
