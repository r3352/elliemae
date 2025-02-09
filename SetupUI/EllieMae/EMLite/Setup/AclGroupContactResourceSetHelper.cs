// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AclGroupContactResourceSetHelper
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
  public class AclGroupContactResourceSetHelper : IResourceSetHelper
  {
    private Sessions.Session session;
    private ArrayList currentOrgs;
    private ArrayList currentUsers;

    public AclGroupContactResourceSetHelper(Sessions.Session session) => this.session = session;

    public void saveResourceSet(
      object resourceSet,
      TreeNodeCollection nodes,
      Dictionary<string, object> updateList)
    {
      this.session.AclGroupManager.UpdateMembersInGroupContact(((AclGroupContactMembers) resourceSet).GroupID, (string[]) updateList["resetUsers"], (int[]) updateList["resetOrgs"], (string[]) updateList["addUsers"], (int[]) updateList["addOrgs"], (int[]) updateList["addInclusiveOrgs"]);
    }

    private void saveCheckedNodeToGroup(TreeNode node, ArrayList orgs, ArrayList users)
    {
      AclResourceAccess access = AclResourceAccess.ReadOnly;
      if (node.ImageIndex == 0)
      {
        OrgInGroupContact orgInGroupContact = (OrgInGroupContact) null;
        if (node.Tag is OrgInfo)
          orgInGroupContact = new OrgInGroupContact(((OrgInfo) node.Tag).Oid, true, access, ((OrgInfo) node.Tag).OrgName);
        else if (node.Tag is OrgInGroupContact)
          orgInGroupContact = new OrgInGroupContact(((OrgInGroup) node.Tag).OrgID, true, access, ((OrgInGroup) node.Tag).OrgName);
        if (this.currentOrgs.Contains((object) orgInGroupContact))
        {
          int index = this.currentOrgs.IndexOf((object) orgInGroupContact);
          orgInGroupContact.Access = ((OrgInGroupContact) this.currentOrgs[index]).Access;
        }
        orgs.Add((object) orgInGroupContact);
      }
      else if (node.ImageIndex == 1)
      {
        OrgInGroupContact orgInGroupContact = (OrgInGroupContact) null;
        if (node.Tag is OrgInfo)
          orgInGroupContact = new OrgInGroupContact(((OrgInfo) node.Tag).Oid, false, access, ((OrgInfo) node.Tag).OrgName);
        else if (node.Tag is OrgInGroupContact)
          orgInGroupContact = new OrgInGroupContact(((OrgInGroup) node.Tag).OrgID, false, access, ((OrgInGroup) node.Tag).OrgName);
        if (this.currentOrgs.Contains((object) orgInGroupContact))
        {
          int index = this.currentOrgs.IndexOf((object) orgInGroupContact);
          orgInGroupContact.Access = ((OrgInGroupContact) this.currentOrgs[index]).Access;
        }
        orgs.Add((object) orgInGroupContact);
      }
      else if (node.ImageIndex == 2)
      {
        UserInGroupContact userInGroupContact = (UserInGroupContact) null;
        if ((object) (node.Tag as UserInfo) != null)
          userInGroupContact = new UserInGroupContact(((UserInfo) node.Tag).Userid, access);
        else if ((object) (node.Tag as UserInGroupContact) != null)
          userInGroupContact = new UserInGroupContact(((UserInGroupContact) node.Tag).UserID, access);
        if (this.currentUsers.Contains((object) userInGroupContact))
        {
          int index = this.currentUsers.IndexOf((object) userInGroupContact);
          userInGroupContact.Access = ((UserInGroupContact) this.currentUsers[index]).Access;
        }
        users.Add((object) userInGroupContact);
      }
      foreach (TreeNode node1 in node.Nodes)
        this.saveCheckedNodeToGroup(node1, orgs, users);
    }
  }
}
