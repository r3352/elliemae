// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AclGroupRoleResourceSetHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AclGroupRoleResourceSetHelper : IResourceSetHelper
  {
    private Sessions.Session session;
    private int roleId = -1;

    public AclGroupRoleResourceSetHelper(Sessions.Session session, int roleId)
    {
      this.session = session;
      this.roleId = roleId;
    }

    public void saveResourceSet(
      object resourceSet,
      TreeNodeCollection nodes,
      Dictionary<string, object> updateList)
    {
      this.session.AclGroupManager.UpdateMembersInGroupRole(((AclGroupRoleMembers) resourceSet).GroupID, this.roleId, (string[]) updateList["resetUsers"], (int[]) updateList["resetOrgs"], (string[]) updateList["addUsers"], (int[]) updateList["addOrgs"], (int[]) updateList["addInclusiveOrgs"]);
    }

    private void saveCheckedNodeToGroup(TreeNode node, ArrayList orgs, ArrayList users)
    {
      int orgId = 0;
      string userId = "";
      string orgName = "";
      if (node.ImageIndex == 0)
      {
        if (node.Tag is OrgInfo)
        {
          orgId = ((OrgInfo) node.Tag).Oid;
          orgName = ((OrgInfo) node.Tag).OrgName;
        }
        else if ((object) (node.Tag as OrgInGroupRole) != null)
        {
          orgId = ((OrgInGroup) node.Tag).OrgID;
          orgName = ((OrgInGroup) node.Tag).OrgName;
        }
        OrgInGroupRole orgInGroupRole = new OrgInGroupRole(this.roleId, orgId, true, orgName);
        orgs.Add((object) orgInGroupRole);
      }
      else if (node.ImageIndex == 1)
      {
        if (node.Tag is OrgInfo)
        {
          orgId = ((OrgInfo) node.Tag).Oid;
          orgName = ((OrgInfo) node.Tag).OrgName;
        }
        else if ((object) (node.Tag as OrgInGroupRole) != null)
        {
          orgId = ((OrgInGroup) node.Tag).OrgID;
          orgName = ((OrgInGroup) node.Tag).OrgName;
        }
        OrgInGroupRole orgInGroupRole = new OrgInGroupRole(this.roleId, orgId, false, orgName);
        orgs.Add((object) orgInGroupRole);
      }
      else if (node.ImageIndex == 2)
      {
        if ((object) (node.Tag as UserInfo) != null)
          userId = ((UserInfo) node.Tag).Userid;
        else if ((object) (node.Tag as UserInGroupRole) != null)
          userId = ((UserInGroupRole) node.Tag).UserID;
        UserInGroupRole userInGroupRole = new UserInGroupRole(userId, this.roleId);
        users.Add((object) userInGroupRole);
      }
      foreach (TreeNode node1 in node.Nodes)
        this.saveCheckedNodeToGroup(node1, orgs, users);
    }
  }
}
