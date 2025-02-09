// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.Acl.ToolsAclManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Acl.Interfaces;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.RemotingServices.Acl
{
  public class ToolsAclManager
  {
    private IToolsAclManager toolsAclManager;

    internal ToolsAclManager(Sessions.Session session)
    {
      this.toolsAclManager = (IToolsAclManager) session.GetAclManager(AclCategory.ToolsGrantWriteAccess);
    }

    public ToolsAclInfo[] GetAccessibleToolsAclInfo(string userId, Persona[] personaList)
    {
      return this.toolsAclManager.GetAccessibleToolsAclInfo(userId, personaList);
    }

    public ToolsAclInfo GetAccessibleToolsAclInfo(
      string userId,
      Persona[] personaList,
      int roleID,
      string milestoneID)
    {
      return this.toolsAclManager.GetAccessibleToolsAclInfo(userId, personaList, roleID, milestoneID);
    }

    public ToolsAclInfo GetPermission(int personaID, int roleID)
    {
      return this.toolsAclManager.GetPermission(personaID, roleID);
    }

    public ToolsAclInfo GetPermission(Persona[] personaList, int roleID, string milestoneID)
    {
      return this.toolsAclManager.GetPermission(personaList, roleID, milestoneID);
    }

    public ToolsAclInfo[] GetPermissions(int personaID)
    {
      return this.toolsAclManager.GetPermissions(personaID);
    }

    public ToolsAclInfo GetPermission(string userID, int roleID)
    {
      return this.toolsAclManager.GetPermission(userID, roleID);
    }

    public ToolsAclInfo[] GetPermissions(string userID)
    {
      return this.toolsAclManager.GetPermissions(userID);
    }

    public void SetPermission(ToolsAclInfo toolsAclInfo, string userid)
    {
      this.toolsAclManager.SetPermission(toolsAclInfo, userid);
    }

    public void SetPermission(ToolsAclInfo toolsAclInfo, int personaID)
    {
      this.toolsAclManager.SetPermission(toolsAclInfo, personaID);
    }

    public void SetPermissions(ToolsAclInfo[] toolsAclInfoList, string userid)
    {
      this.toolsAclManager.SetPermissions(toolsAclInfoList, userid);
    }

    public void SetPermissions(ToolsAclInfo[] toolsAclInfoList, int personaID)
    {
      this.toolsAclManager.SetPermissions(toolsAclInfoList, personaID);
    }

    public void DuplicateACLTools(int sourcePersonaID, int desPersonaID)
    {
      this.toolsAclManager.DuplicateACLTools(sourcePersonaID, desPersonaID);
    }

    public void SynchronizeAdminRight() => this.toolsAclManager.SynchronizeAdminRight();

    public void SynchronizeBrokerSetting(string baseMilestoneID, string currentMilestoneID)
    {
      this.toolsAclManager.SynchronizeBrokerSetting(baseMilestoneID, currentMilestoneID);
    }
  }
}
