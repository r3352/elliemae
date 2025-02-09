// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.UserGroupItemProvider
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class UserGroupItemProvider : IResourceItemProvider
  {
    private Sessions.Session session;
    private OrgItemProvider nodeCntProvider;
    private UserItemProvider leafCntProvider;
    private object resourceToSelect;
    private ArrayList orgPathForItemToSelect;

    public UserGroupItemProvider(
      Sessions.Session session,
      AclGroupLoanMembers aclGroupLoan,
      object resourceToSelect,
      OrgInfo[] allOrgList,
      UserInfo[] allUserList)
    {
      this.session = session;
      this.resourceToSelect = resourceToSelect;
      OrgInGroup orgToSelect = (OrgInGroup) null;
      string userToSelect = (string) null;
      if (resourceToSelect != null)
      {
        if ((object) (resourceToSelect as OrgInGroup) != null)
          orgToSelect = (OrgInGroup) resourceToSelect;
        else
          userToSelect = (string) resourceToSelect;
      }
      this.nodeCntProvider = new OrgItemProvider(new ArrayList((ICollection) aclGroupLoan.OrgMembers), orgToSelect, allOrgList, allUserList);
      this.leafCntProvider = new UserItemProvider(new ArrayList((ICollection) this.getUserIds(aclGroupLoan.UserMembers)), userToSelect, allUserList);
    }

    public UserGroupItemProvider(
      Sessions.Session session,
      AclGroupContactMembers aclGroupContact,
      object resourceToSelect,
      OrgInfo[] allOrgList,
      UserInfo[] allUserList)
    {
      this.session = session;
      this.resourceToSelect = resourceToSelect;
      OrgInGroup orgToSelect = (OrgInGroup) null;
      string userToSelect = (string) null;
      if (resourceToSelect != null)
      {
        if ((object) (resourceToSelect as OrgInGroup) != null)
          orgToSelect = (OrgInGroup) resourceToSelect;
        else
          userToSelect = (string) resourceToSelect;
      }
      this.nodeCntProvider = new OrgItemProvider(new ArrayList((ICollection) aclGroupContact.OrgMembers), orgToSelect, allOrgList, allUserList);
      this.leafCntProvider = new UserItemProvider(new ArrayList((ICollection) this.getUserIds(aclGroupContact.UserMembers)), userToSelect, allUserList);
    }

    public UserGroupItemProvider(
      AclGroupRoleMembers members,
      object resourceToSelect,
      OrgInfo[] allOrgList,
      UserInfo[] allUserList)
      : this(members, resourceToSelect, allOrgList, allUserList, Session.DefaultInstance)
    {
    }

    public UserGroupItemProvider(
      AclGroupRoleMembers members,
      object resourceToSelect,
      OrgInfo[] allOrgList,
      UserInfo[] allUserList,
      Sessions.Session session)
    {
      this.session = session;
      this.resourceToSelect = resourceToSelect;
      OrgInGroup orgToSelect = (OrgInGroup) null;
      string userToSelect = (string) null;
      if (resourceToSelect != null)
      {
        if ((object) (resourceToSelect as OrgInGroup) != null)
          orgToSelect = (OrgInGroup) resourceToSelect;
        else
          userToSelect = (string) resourceToSelect;
      }
      this.nodeCntProvider = new OrgItemProvider(new ArrayList((ICollection) members.OrgMembers), orgToSelect, allOrgList, allUserList);
      this.leafCntProvider = new UserItemProvider(new ArrayList((ICollection) this.getUserIds(members.UserMembers)), userToSelect, string.Concat((object) members.RoleID), allUserList);
    }

    private string[] getUserIds(UserInGroupLoan[] userMembers)
    {
      if (userMembers.Length == 0)
        return new string[0];
      string[] userIds = new string[userMembers.Length];
      for (int index = 0; index < userMembers.Length; ++index)
        userIds[index] = userMembers[index].UserID;
      return userIds;
    }

    private string[] getUserIds(UserInGroupContact[] userMembers)
    {
      if (userMembers.Length == 0)
        return new string[0];
      string[] userIds = new string[userMembers.Length];
      for (int index = 0; index < userMembers.Length; ++index)
        userIds[index] = userMembers[index].UserID;
      return userIds;
    }

    private string[] getUserIds(UserInGroupRole[] userMembers)
    {
      if (userMembers.Length == 0)
        return new string[0];
      string[] userIds = new string[userMembers.Length];
      for (int index = 0; index < userMembers.Length; ++index)
        userIds[index] = userMembers[index].UserID;
      return userIds;
    }

    public TreeNode[] getInitialNodes()
    {
      bool flag = this.session.ConfigurationManager.CheckIfAnyTPOSiteExists();
      IOrganizationManager organizationManager = this.session.OrganizationManager;
      OrgInfo org = (OrgInfo) null;
      TreeNode node = (TreeNode) null;
      OrgInfo organization = organizationManager.GetOrganization(this.session.UserInfo.OrgId);
      if (this.session.ConfigurationManager.GetExternalRootOrgIdFromOrgChart() != 0 & flag)
        org = organizationManager.GetOrganization(this.session.ConfigurationManager.GetExternalRootOrgIdFromOrgChart());
      TreeNode parent = this.nodeCntProvider.addOrgNodeToParent(organization, (TreeNode) null);
      if (org != null)
        node = this.nodeCntProvider.addOrgNodeToParent(org, (TreeNode) null);
      if (this.resourceToSelect != null)
      {
        this.orgPathForItemToSelect = this.findPathOfNodeToSelect();
        this.expandPath(parent);
        if (node != null)
          this.expandPath(node);
      }
      this.resourceToSelect = (object) null;
      return node != null ? new TreeNode[2]{ parent, node } : new TreeNode[1]
      {
        parent
      };
    }

    public void onNodeExpand(TreeNode tn)
    {
      if (!this.isNodeExpandable(tn) || this.nodeIsExpanded(tn))
        return;
      tn.Nodes.Clear();
      OrgInfo tag = (OrgInfo) tn.Tag;
      OrgInfo[] children1 = this.nodeCntProvider.getChildren(tag);
      UserInfo[] children2 = this.leafCntProvider.getChildren(tag);
      Array.Sort<OrgInfo>(children1);
      for (int index = 0; index < children1.Length; ++index)
        this.nodeCntProvider.addOrgNodeToParent(children1[index], tn);
      Array.Sort<UserInfo>(children2);
      for (int index = 0; index < children2.Length; ++index)
        this.leafCntProvider.addUserNodeToParent(children2[index], tn);
    }

    private bool isNodeExpandable(TreeNode node) => node.Tag is OrgInfo;

    private bool nodeIsExpanded(TreeNode node)
    {
      return node.Nodes.Count > 1 || node.Nodes.Count == 1 && node.Nodes[0].Text != "<DUMMY NODE>";
    }

    private ArrayList findPathOfUserToSelect(string userId)
    {
      return this.findPathOfOrgToSelect(this.session.OrganizationManager.GetUser(userId).OrgId);
    }

    private ArrayList findPathOfOrgToSelect(int orgId)
    {
      ArrayList pathOfOrgToSelect = new ArrayList();
      IOrganizationManager organizationManager = this.session.OrganizationManager;
      pathOfOrgToSelect.Add((object) orgId);
      OrgInfo organization = organizationManager.GetOrganization(orgId);
      while (organization.Parent != organization.Oid)
      {
        organization = organizationManager.GetOrganization(organization.Parent);
        pathOfOrgToSelect.Add((object) organization.Oid);
      }
      pathOfOrgToSelect.Reverse();
      return pathOfOrgToSelect;
    }

    private ArrayList findPathOfNodeToSelect()
    {
      if (this.resourceToSelect == null)
        return (ArrayList) null;
      return (object) (this.resourceToSelect as OrgInGroup) != null ? this.findPathOfOrgToSelect(((OrgInGroup) this.resourceToSelect).OrgID) : this.findPathOfUserToSelect((string) this.resourceToSelect);
    }

    public TreeNode getNodeToSelect()
    {
      if (this.nodeCntProvider.NodeToSelect != null)
        return this.nodeCntProvider.NodeToSelect;
      return this.leafCntProvider.NodeToSelect != null ? this.leafCntProvider.NodeToSelect : (TreeNode) null;
    }

    private void expandPath(TreeNode node)
    {
      if (!this.isNodeExpandable(node) || this.orgPathForItemToSelect.Count == 0 || ((OrgInfo) node.Tag).Oid != (int) this.orgPathForItemToSelect[0])
        return;
      this.orgPathForItemToSelect.RemoveAt(0);
      if (this.orgPathForItemToSelect.Count == 0 && this.resourceToSelect != null && (object) (this.resourceToSelect as OrgInGroup) != null)
        return;
      this.onNodeExpand(node);
      foreach (TreeNode node1 in node.Nodes)
        this.expandPath(node1);
    }

    public void SetHeadNode(TreeNode node)
    {
    }
  }
}
