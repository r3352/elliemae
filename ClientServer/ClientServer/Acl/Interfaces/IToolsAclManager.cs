// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Acl.Interfaces.IToolsAclManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Acl.Interfaces
{
  public interface IToolsAclManager
  {
    ToolsAclInfo[] GetAccessibleToolsAclInfo(string userId, Persona[] personaList);

    ToolsAclInfo GetAccessibleToolsAclInfo(
      string userId,
      Persona[] personaList,
      int roleID,
      string milestoneID);

    ToolsAclInfo[] GetFileContactGrantWriteAccessRights(
      string userID,
      Persona[] personaList,
      string[] loanMilestoneIDs,
      int[] loanFreeRoleIDs);

    ToolsAclInfo GetPermission(int personaID, int roleID);

    ToolsAclInfo GetPermission(Persona[] personaList, int roleID, string milestoneID);

    ToolsAclInfo[] GetPermissions(int personaID);

    ToolsAclInfo GetPermission(string userID, int roleID);

    ToolsAclInfo[] GetPermissions(string userID);

    void SetPermission(ToolsAclInfo toolsAclInfo, string userid);

    void SetPermission(ToolsAclInfo toolsAclInfo, int personaID);

    void SetPermissions(ToolsAclInfo[] toolsAclInfoList, string userid);

    void SetPermissions(ToolsAclInfo[] toolsAclInfoList, int personaID);

    void DuplicateACLTools(int sourcePersonaID, int desPersonaID);

    void SynchronizeAdminRight();

    void SynchronizeBrokerSetting(string baseMilestoneID, string currentMilestoneID);
  }
}
