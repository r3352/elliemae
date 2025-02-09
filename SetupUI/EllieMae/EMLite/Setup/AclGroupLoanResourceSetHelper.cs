// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AclGroupLoanResourceSetHelper
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
  public class AclGroupLoanResourceSetHelper : IResourceSetHelper
  {
    private Sessions.Session session;
    private ArrayList currentOrgs;
    private ArrayList currentUsers;

    public AclGroupLoanResourceSetHelper(Sessions.Session session) => this.session = session;

    public void saveResourceSet(
      object resourceSet,
      TreeNodeCollection nodes,
      Dictionary<string, object> updateList)
    {
      this.session.AclGroupManager.UpdateMembersInGroupLoan(((AclGroupLoanMembers) resourceSet).GroupID, (string[]) updateList["resetUsers"], (int[]) updateList["resetOrgs"], (string[]) updateList["addUsers"], (int[]) updateList["addOrgs"], (int[]) updateList["addInclusiveOrgs"]);
    }

    private void saveCheckedNodeToGroup(TreeNode node, ArrayList orgs, ArrayList users)
    {
      AclResourceAccess access = AclResourceAccess.ReadOnly;
      if (node.ImageIndex == 0)
      {
        OrgInGroupLoan orgInGroupLoan = (OrgInGroupLoan) null;
        if (node.Tag is OrgInfo)
          orgInGroupLoan = new OrgInGroupLoan(((OrgInfo) node.Tag).Oid, true, access, ((OrgInfo) node.Tag).OrgName);
        else if (node.Tag is OrgInGroupLoan)
          orgInGroupLoan = new OrgInGroupLoan(((OrgInGroup) node.Tag).OrgID, true, access, ((OrgInGroup) node.Tag).OrgName);
        if (this.currentOrgs.Contains((object) orgInGroupLoan))
        {
          int index = this.currentOrgs.IndexOf((object) orgInGroupLoan);
          orgInGroupLoan.Access = ((OrgInGroupLoan) this.currentOrgs[index]).Access;
        }
        orgs.Add((object) orgInGroupLoan);
      }
      else if (node.ImageIndex == 1)
      {
        OrgInGroupLoan orgInGroupLoan = (OrgInGroupLoan) null;
        if (node.Tag is OrgInfo)
          orgInGroupLoan = new OrgInGroupLoan(((OrgInfo) node.Tag).Oid, false, access, ((OrgInfo) node.Tag).OrgName);
        else if (node.Tag is OrgInGroupLoan)
          orgInGroupLoan = new OrgInGroupLoan(((OrgInGroup) node.Tag).OrgID, false, access, ((OrgInGroup) node.Tag).OrgName);
        if (this.currentOrgs.Contains((object) orgInGroupLoan))
        {
          int index = this.currentOrgs.IndexOf((object) orgInGroupLoan);
          orgInGroupLoan.Access = ((OrgInGroupLoan) this.currentOrgs[index]).Access;
        }
        orgs.Add((object) orgInGroupLoan);
      }
      else if (node.ImageIndex == 2)
      {
        UserInGroupLoan userInGroupLoan = (UserInGroupLoan) null;
        if ((object) (node.Tag as UserInfo) != null)
          userInGroupLoan = new UserInGroupLoan(((UserInfo) node.Tag).Userid, access);
        else if ((object) (node.Tag as UserInGroupLoan) != null)
          userInGroupLoan = new UserInGroupLoan(((UserInGroupLoan) node.Tag).UserID, access);
        if (this.currentUsers.Contains((object) userInGroupLoan))
        {
          int index = this.currentUsers.IndexOf((object) userInGroupLoan);
          userInGroupLoan.Access = ((UserInGroupLoan) this.currentUsers[index]).Access;
        }
        users.Add((object) userInGroupLoan);
      }
      foreach (TreeNode node1 in node.Nodes)
        this.saveCheckedNodeToGroup(node1, orgs, users);
    }
  }
}
