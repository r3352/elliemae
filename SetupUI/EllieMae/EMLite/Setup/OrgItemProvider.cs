// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.OrgItemProvider
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
  public class OrgItemProvider
  {
    private ArrayList _CheckedOrgIds;
    private OrgInGroup orgToSelect;
    private TreeNode nodeToSelect;
    private OrgInfo[] allOrgList;
    private UserInfo[] allUserList;

    public TreeNode NodeToSelect => this.nodeToSelect;

    public OrgItemProvider(
      ArrayList orgsInGroup,
      OrgInGroup orgToSelect,
      OrgInfo[] allOrgList,
      UserInfo[] allUserList)
    {
      this._CheckedOrgIds = orgsInGroup;
      this.orgToSelect = orgToSelect;
      this.allOrgList = allOrgList;
      this.allUserList = allUserList;
    }

    public OrgInfo[] getChildren(OrgInfo parentOrg) => this.getChildOrgList(parentOrg.Oid);

    public TreeNode addOrgNodeToParent(OrgInfo org, TreeNode parentNode)
    {
      TreeNode node = new TreeNode(org.OrgName);
      node.Tag = (object) org;
      parentNode?.Nodes.Add(node);
      if (this.orgToSelect != (OrgInGroup) null && org.Oid == this.orgToSelect.OrgID)
      {
        this.nodeToSelect = node;
        this.orgToSelect = (OrgInGroup) null;
      }
      this.setOrgNodeImageIndex(node);
      this.addDummyNodeIfNecessary(node);
      return node;
    }

    private void setOrgNodeImageIndex(TreeNode node)
    {
      OrgInfo tag = (OrgInfo) node.Tag;
      OrgInGroup orgInGroup = new OrgInGroup(tag.Oid, false, tag.OrgName);
      if (this._CheckedOrgIds.Contains((object) orgInGroup))
      {
        if (((OrgInGroup) this._CheckedOrgIds[this._CheckedOrgIds.IndexOf((object) orgInGroup)]).IsInclusive)
          this.setNodeImageIndex(node, 0);
        else
          this.setNodeImageIndex(node, 1);
      }
      else if (this.existsInclusiveParent(node))
        this.setNodeImageIndex(node, 3);
      else
        this.setNodeImageIndex(node, 5);
    }

    private void setNodeImageIndex(TreeNode node, int index)
    {
      node.ImageIndex = index;
      node.SelectedImageIndex = index;
    }

    private bool shouldAddDummyNode(int[] childOrgs, UserInfo[] users)
    {
      return childOrgs.Length != 0 || users.Length != 0;
    }

    private bool existsInclusiveParent(TreeNode node)
    {
      TreeNode parent = node.Parent;
      if (parent == null)
        return false;
      if (parent.ImageIndex == 0 || parent.ImageIndex == 3)
        return true;
      if (parent.ImageIndex == 5)
        return false;
      return parent.ImageIndex == 1 && (object) (node.Tag as UserInfo) != null || this.existsInclusiveParent(parent);
    }

    private void addDummyNodeIfNecessary(TreeNode node)
    {
      node.Nodes.Clear();
      OrgInfo tag = (OrgInfo) node.Tag;
      UserInfo[] userList = this.getUserList(tag.Oid);
      if (!this.shouldAddDummyNode(tag.Children, userList))
        return;
      node.Nodes.Add(new TreeNode("<DUMMY NODE>", 0, 1));
    }

    private OrgInfo[] getChildOrgList(int parentOrgID)
    {
      List<OrgInfo> orgInfoList = new List<OrgInfo>();
      if (this.allOrgList != null && this.allOrgList.Length != 0)
      {
        foreach (OrgInfo allOrg in this.allOrgList)
        {
          if (allOrg.Parent == parentOrgID && allOrg.Parent != allOrg.Oid)
            orgInfoList.Add(allOrg);
        }
      }
      return orgInfoList.ToArray();
    }

    private UserInfo[] getUserList(int orgID)
    {
      List<UserInfo> userInfoList = new List<UserInfo>();
      if (this.allUserList != null && this.allUserList.Length != 0)
      {
        foreach (UserInfo allUser in this.allUserList)
        {
          if (allUser.OrgId == orgID)
            userInfoList.Add(allUser);
        }
      }
      return userInfoList.ToArray();
    }
  }
}
