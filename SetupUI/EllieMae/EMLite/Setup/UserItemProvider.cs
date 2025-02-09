// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.UserItemProvider
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
  public class UserItemProvider
  {
    private string UserToSelect;
    private ArrayList _CheckedUserIds;
    private TreeNode nodeToSelect;
    private string roleID = "";
    private UserInfo[] allUserList;

    public TreeNode NodeToSelect => this.nodeToSelect;

    public UserItemProvider(ArrayList usersInGroup, string userToSelect, UserInfo[] allUserList)
    {
      this._CheckedUserIds = usersInGroup;
      this.UserToSelect = userToSelect;
      this.allUserList = allUserList;
    }

    public UserItemProvider(
      ArrayList usersInGroup,
      string userToSelect,
      string roleID,
      UserInfo[] allUserList)
    {
      this._CheckedUserIds = usersInGroup;
      this.UserToSelect = userToSelect;
      this.roleID = roleID;
      this.allUserList = allUserList;
    }

    public UserInfo[] getChildren(OrgInfo parentOrg) => this.getUserList(parentOrg.Oid);

    public void addUserNodeToParent(UserInfo user, TreeNode parentNode)
    {
      TreeNode node = new TreeNode(user.FullName + " (" + user.Userid + ")");
      parentNode.Nodes.Add(node);
      node.Tag = (object) user;
      if (this.UserToSelect != null && user.Userid == this.UserToSelect)
      {
        this.nodeToSelect = node;
        this.UserToSelect = (string) null;
      }
      this.setUserNodeImageIndex(node);
    }

    private void setUserNodeImageIndex(TreeNode node)
    {
      if (this._CheckedUserIds.Contains((object) ((UserInfo) node.Tag).Userid))
        this.setNodeImageIndex(node, 2);
      else if (this.existsInclusiveParent(node))
        this.setNodeImageIndex(node, 4);
      else
        this.setNodeImageIndex(node, 6);
    }

    private void setNodeImageIndex(TreeNode node, int index)
    {
      node.ImageIndex = index;
      node.SelectedImageIndex = index;
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
