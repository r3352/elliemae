// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ResourceSetViewer
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ResourceSetViewer : Form
  {
    private TreeViewer treeViewer;
    private ImageList imgListTv;
    private Label label2;
    private Label label3;
    private ContextMenu contextMenuOrg;
    private MenuItem addOrgToGroupMenuItem;
    private MenuItem addOrgInclusiveMenuItem;
    private MenuItem delOrgFromGroupMenuItem;
    private MenuItem menuItemExpandAll;
    private Button btnOK;
    private Button btnCancel;
    private IContainer components;
    private Label lblResourcesInThisLevel;
    private IResourceSetHelper setHelper;
    private object currentResourceSet;
    private bool dirty;
    private TreeNode[] resetDataSourceTreeNode;
    private AclFileResource[] resetDataSourceAclFileResource;
    private List<int> resetOrgs = new List<int>();
    private List<string> resetUsers = new List<string>();
    private List<FileSystemEntry> resetFolders = new List<FileSystemEntry>();
    private List<FileSystemEntry> resetFiles = new List<FileSystemEntry>();
    private List<int> addOrgList = new List<int>();
    private List<int> addInclusiveOrgList = new List<int>();
    private List<string> addUsers = new List<string>();
    private List<FileSystemEntry> addFolderList = new List<FileSystemEntry>();
    private List<FileSystemEntry> addInclusiveFolderList = new List<FileSystemEntry>();
    private List<FileSystemEntry> addFileList = new List<FileSystemEntry>();
    private GroupContainer groupContainer1;
    private GradientPanel gradientPanel1;
    private PanelEx panelEx1;
    private IconButton btnAddToGroup;
    private IconButton btnAddAllToGroup;
    private ToolTip toolTip1;
    private MenuItem menuItemCollapseAll;
    private StandardIconButton btnRemoveFromGroup;
    private Dictionary<string, object> latestState = new Dictionary<string, object>();

    public event EventHandler DirtyFlagChanged;

    public object CurrentResourceSet
    {
      get => this.currentResourceSet;
      set
      {
        this.currentResourceSet = value;
        if (value == null)
          return;
        this.treeViewer.ShowTree(value);
      }
    }

    public AclFileResource[] SetResetDataSourceAclFileResource
    {
      set => this.resetDataSourceAclFileResource = value;
    }

    public TreeNode[] SetResetDataSourceTreeNode
    {
      set => this.resetDataSourceTreeNode = value;
    }

    public ResourceSetViewer(
      Sessions.Session session,
      object resourceSet,
      EventHandler dirtyFlagChanged,
      int groupID)
      : this(session, resourceSet, (object) null, dirtyFlagChanged, groupID)
    {
    }

    public ResourceSetViewer(
      Sessions.Session session,
      object resourceSet,
      object resourceToSelect,
      EventHandler dirtyFlagChanged,
      int groupID)
    {
      this.treeViewer = new TreeViewer(session);
      this.InitializeComponent();
      if ((object) (resourceSet as AclGroup) == null)
      {
        switch (resourceSet)
        {
          case AclGroupLoanMembers _:
          case AclGroupRoleMembers _:
          case AclGroupContactMembers _:
            break;
          default:
            this.lblResourcesInThisLevel.Visible = false;
            this.label2.Text = "       The group can access all items in this level and below";
            this.label3.Text = "       The group can access this item";
            this.btnAddToGroup.Text = "Add to Group";
            break;
        }
      }
      if ((object) (resourceSet as AclGroup) != null)
      {
        this.setHelper = (IResourceSetHelper) new AclGroupResourceSetHelper(session);
      }
      else
      {
        switch (resourceSet)
        {
          case AclGroupLoanMembers _:
            this.setHelper = (IResourceSetHelper) new AclGroupLoanResourceSetHelper(session);
            break;
          case AclGroupContactMembers _:
            this.setHelper = (IResourceSetHelper) new AclGroupContactResourceSetHelper(session);
            break;
          case AclGroupRoleMembers _:
            this.setHelper = (IResourceSetHelper) new AclGroupRoleResourceSetHelper(session, ((AclGroupRoleMembers) resourceSet).RoleID);
            break;
          case FileSystemResourceSet _:
            this.setHelper = (IResourceSetHelper) new FileSystemResourceSetHelper(session);
            break;
        }
      }
      this.treeViewer.resourceToSelect = resourceToSelect;
      this.CurrentResourceSet = resourceSet;
      this.resetUpdateList();
      this.FormClosing += new FormClosingEventHandler(this.ResourceSetViewer_FormClosing);
      if (dirtyFlagChanged != null)
        this.DirtyFlagChanged += dirtyFlagChanged;
      this.groupContainer1.Text = session.AclGroupManager.GetGroupById(groupID).Name + " Group";
    }

    public void PreSelectNode(TreeNode selectedNode)
    {
      this.treeViewer.ExpandTree(this.treeViewer.Nodes[0]);
      if (this.MakeTreeNodeSelected(this.treeViewer.Nodes[0], selectedNode) || this.treeViewer.Nodes.Count <= 1)
        return;
      this.treeViewer.ExpandTree(this.treeViewer.Nodes[1]);
      this.MakeTreeNodeSelected(this.treeViewer.Nodes[1], selectedNode);
    }

    public void SetTempState(TreeNode[] treeNodes)
    {
      this.SetTreeView(this.treeViewer.Nodes[0], treeNodes);
      if (this.treeViewer.Nodes.Count <= 1)
        return;
      this.SetTreeView(this.treeViewer.Nodes[1], treeNodes);
    }

    private void SetTreeView(TreeNode node, TreeNode[] nodeCol)
    {
      bool flag1 = false;
      bool flag2 = false;
      if ((object) (node.Tag as UserInfo) != null || (object) (node.Tag as UserInGroupLoan) != null || (object) (node.Tag as UserInGroupRole) != null || (object) (node.Tag as UserInGroupContact) != null)
        flag2 = true;
      if (flag2)
      {
        foreach (TreeNode treeNode in nodeCol)
        {
          if ((object) (treeNode.Tag as UserInfo) != null)
          {
            if ((object) (node.Tag as UserInfo) != null)
            {
              if (((UserInfo) node.Tag).Userid == ((UserInfo) treeNode.Tag).Userid)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if ((object) (node.Tag as UserInGroupRole) != null)
            {
              if (((UserInGroupRole) node.Tag).UserID == ((UserInfo) treeNode.Tag).Userid)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if ((object) (node.Tag as UserInGroupLoan) != null)
            {
              if (((UserInGroupLoan) node.Tag).UserID == ((UserInfo) treeNode.Tag).Userid)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if ((object) (node.Tag as UserInGroupContact) != null && ((UserInGroupContact) node.Tag).UserID == ((UserInfo) treeNode.Tag).Userid)
            {
              node.ImageIndex = treeNode.ImageIndex;
              node.SelectedImageIndex = treeNode.SelectedImageIndex;
              flag1 = true;
              break;
            }
          }
          else if ((object) (treeNode.Tag as UserInGroupLoan) != null)
          {
            if ((object) (node.Tag as UserInfo) != null)
            {
              if (((UserInfo) node.Tag).Userid == ((UserInGroupLoan) treeNode.Tag).UserID)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if ((object) (node.Tag as UserInGroupRole) != null)
            {
              if (((UserInGroupRole) node.Tag).UserID == ((UserInGroupLoan) treeNode.Tag).UserID)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if ((object) (node.Tag as UserInGroupLoan) != null)
            {
              if (((UserInGroupLoan) node.Tag).UserID == ((UserInGroupLoan) treeNode.Tag).UserID)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if ((object) (node.Tag as UserInGroupContact) != null && ((UserInGroupContact) node.Tag).UserID == ((UserInGroupContact) treeNode.Tag).UserID)
            {
              node.ImageIndex = treeNode.ImageIndex;
              node.SelectedImageIndex = treeNode.SelectedImageIndex;
              flag1 = true;
              break;
            }
          }
          else if ((object) (treeNode.Tag as UserInGroupRole) != null)
          {
            if ((object) (node.Tag as UserInfo) != null)
            {
              if (((UserInfo) node.Tag).Userid == ((UserInGroupRole) treeNode.Tag).UserID)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if ((object) (node.Tag as UserInGroupRole) != null)
            {
              if (((UserInGroupRole) node.Tag).UserID == ((UserInGroupRole) treeNode.Tag).UserID)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if ((object) (node.Tag as UserInGroupLoan) != null)
            {
              if (((UserInGroupLoan) node.Tag).UserID == ((UserInGroupRole) treeNode.Tag).UserID)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if ((object) (node.Tag as UserInGroupContact) != null && ((UserInGroupContact) node.Tag).UserID == ((UserInGroupContact) treeNode.Tag).UserID)
            {
              node.ImageIndex = treeNode.ImageIndex;
              node.SelectedImageIndex = treeNode.SelectedImageIndex;
              flag1 = true;
              break;
            }
          }
          else if ((object) (treeNode.Tag as UserInGroupContact) != null)
          {
            if ((object) (node.Tag as UserInfo) != null)
            {
              if (((UserInfo) node.Tag).Userid == ((UserInGroupContact) treeNode.Tag).UserID)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if ((object) (node.Tag as UserInGroupRole) != null)
            {
              if (((UserInGroupRole) node.Tag).UserID == ((UserInGroupContact) treeNode.Tag).UserID)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if ((object) (node.Tag as UserInGroupLoan) != null)
            {
              if (((UserInGroupLoan) node.Tag).UserID == ((UserInGroupContact) treeNode.Tag).UserID)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if ((object) (node.Tag as UserInGroupContact) != null && ((UserInGroupContact) node.Tag).UserID == ((UserInGroupContact) treeNode.Tag).UserID)
            {
              node.ImageIndex = treeNode.ImageIndex;
              node.SelectedImageIndex = treeNode.SelectedImageIndex;
              flag1 = true;
              break;
            }
          }
        }
      }
      else
      {
        foreach (TreeNode treeNode in nodeCol)
        {
          if (treeNode.Tag is OrgInfo)
          {
            if (node.Tag is OrgInfo)
            {
              if (((OrgInfo) node.Tag).Oid == ((OrgInfo) treeNode.Tag).Oid)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if ((object) (node.Tag as OrgInGroupRole) != null)
            {
              if (((OrgInGroup) node.Tag).OrgID == ((OrgInfo) treeNode.Tag).Oid)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if (node.Tag is OrgInGroupLoan)
            {
              if (((OrgInGroup) node.Tag).OrgID == ((OrgInfo) treeNode.Tag).Oid)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if (node.Tag is OrgInGroupContact && ((OrgInGroup) node.Tag).OrgID == ((OrgInfo) treeNode.Tag).Oid)
            {
              node.ImageIndex = treeNode.ImageIndex;
              node.SelectedImageIndex = treeNode.SelectedImageIndex;
              flag1 = true;
              break;
            }
          }
          else if (treeNode.Tag is OrgInGroupLoan)
          {
            if (node.Tag is OrgInfo)
            {
              if (((OrgInfo) node.Tag).Oid == ((OrgInGroup) treeNode.Tag).OrgID)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if ((object) (node.Tag as OrgInGroupRole) != null)
            {
              if (((OrgInGroup) node.Tag).OrgID == ((OrgInGroup) treeNode.Tag).OrgID)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if (node.Tag is OrgInGroupLoan)
            {
              if (((OrgInGroup) node.Tag).OrgID == ((OrgInGroup) treeNode.Tag).OrgID)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if (node.Tag is OrgInGroupContact && ((OrgInGroup) node.Tag).OrgID == ((OrgInGroup) treeNode.Tag).OrgID)
            {
              node.ImageIndex = treeNode.ImageIndex;
              node.SelectedImageIndex = treeNode.SelectedImageIndex;
              flag1 = true;
              break;
            }
          }
          else if ((object) (treeNode.Tag as OrgInGroupRole) != null)
          {
            if (node.Tag is OrgInfo)
            {
              if (((OrgInfo) node.Tag).Oid == ((OrgInGroup) treeNode.Tag).OrgID)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if ((object) (node.Tag as OrgInGroupRole) != null)
            {
              if (((OrgInGroup) node.Tag).OrgID == ((OrgInGroup) treeNode.Tag).OrgID)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if (node.Tag is OrgInGroupLoan)
            {
              if (((OrgInGroup) node.Tag).OrgID == ((OrgInGroup) treeNode.Tag).OrgID)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if (node.Tag is OrgInGroupContact && ((OrgInGroup) node.Tag).OrgID == ((OrgInGroup) treeNode.Tag).OrgID)
            {
              node.ImageIndex = treeNode.ImageIndex;
              node.SelectedImageIndex = treeNode.SelectedImageIndex;
              flag1 = true;
              break;
            }
          }
          else if (treeNode.Tag is OrgInGroupContact)
          {
            if (node.Tag is OrgInfo)
            {
              if (((OrgInfo) node.Tag).Oid == ((OrgInGroup) treeNode.Tag).OrgID)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if ((object) (node.Tag as OrgInGroupRole) != null)
            {
              if (((OrgInGroup) node.Tag).OrgID == ((OrgInGroup) treeNode.Tag).OrgID)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if (node.Tag is OrgInGroupLoan)
            {
              if (((OrgInGroup) node.Tag).OrgID == ((OrgInGroup) treeNode.Tag).OrgID)
              {
                node.ImageIndex = treeNode.ImageIndex;
                node.SelectedImageIndex = treeNode.SelectedImageIndex;
                flag1 = true;
                break;
              }
            }
            else if (node.Tag is OrgInGroupContact && ((OrgInGroup) node.Tag).OrgID == ((OrgInGroup) treeNode.Tag).OrgID)
            {
              node.ImageIndex = treeNode.ImageIndex;
              node.SelectedImageIndex = treeNode.SelectedImageIndex;
              flag1 = true;
              break;
            }
          }
        }
      }
      if (!flag1)
      {
        if (flag2)
        {
          node.SelectedImageIndex = 6;
          node.ImageIndex = 6;
        }
        else
        {
          node.SelectedImageIndex = 5;
          node.ImageIndex = 5;
        }
      }
      foreach (TreeNode node1 in node.Nodes)
        this.SetTreeView(node1, nodeCol);
    }

    public void SetTempState(AclFileResource[] fileResources)
    {
      this.Cursor = Cursors.WaitCursor;
      this.treeViewer.SuspendLayout();
      this.SetTreeView(this.treeViewer.Nodes[0], fileResources);
      this.treeViewer.ResumeLayout(false);
      this.Cursor = Cursors.Default;
    }

    private void SetTreeView(TreeNode node, AclFileResource[] fileResources)
    {
      bool flag1 = false;
      bool flag2 = false;
      if (((FileSystemEntry) node.Tag).Type == FileSystemEntry.Types.Folder)
        flag2 = true;
      foreach (AclFileResource fileResource in fileResources)
      {
        string str = fileResource.FilePath;
        if (str.IndexOf(":") > 0)
          str = str.Substring(str.IndexOf(":") + 1);
        if (((FileSystemEntry) node.Tag).Path == str)
        {
          if (flag2)
          {
            node.ImageIndex = 0;
            node.SelectedImageIndex = 0;
          }
          else
          {
            node.ImageIndex = 2;
            node.SelectedImageIndex = 2;
          }
          flag1 = true;
          break;
        }
      }
      if (!flag1)
      {
        if (flag2)
        {
          node.ImageIndex = 5;
          node.SelectedImageIndex = 5;
        }
        else
        {
          node.ImageIndex = 6;
          node.SelectedImageIndex = 6;
        }
      }
      foreach (TreeNode node1 in node.Nodes)
        this.SetTreeView(node1, fileResources);
    }

    private bool MakeTreeNodeSelected(TreeNode node, TreeNode selectedNode)
    {
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      if ((object) (node.Tag as UserInfo) != null)
        flag2 = true;
      if ((object) (selectedNode.Tag as UserInfo) != null)
        flag3 = true;
      if (flag2 & flag3)
      {
        if (((UserInfo) node.Tag).Userid == ((UserInfo) selectedNode.Tag).Userid)
        {
          flag1 = true;
          this.treeViewer.SelectedNode = node;
        }
      }
      else if (!flag2 && !flag3 && ((OrgInfo) node.Tag).Oid == ((OrgInfo) selectedNode.Tag).Oid)
      {
        flag1 = true;
        this.treeViewer.SelectedNode = node;
      }
      if (!flag1)
      {
        foreach (TreeNode node1 in node.Nodes)
        {
          if (this.MakeTreeNodeSelected(node1, selectedNode))
          {
            flag1 = true;
            break;
          }
        }
      }
      return flag1;
    }

    private void resetUpdateList()
    {
      this.resetUsers = new List<string>();
      this.resetOrgs = new List<int>();
      this.resetFolders = new List<FileSystemEntry>();
      this.resetFiles = new List<FileSystemEntry>();
      this.addInclusiveOrgList = new List<int>();
      this.addUsers = new List<string>();
      this.addOrgList = new List<int>();
      this.addFolderList = new List<FileSystemEntry>();
      this.addInclusiveFolderList = new List<FileSystemEntry>();
      this.addFileList = new List<FileSystemEntry>();
    }

    private void resetLatestState()
    {
      this.latestState = new Dictionary<string, object>();
      this.latestState.Add("resetUsers", (object) new string[0]);
      this.latestState.Add("addUsers", (object) new string[0]);
      this.latestState.Add("resetOrgs", (object) new int[0]);
      this.latestState.Add("addOrgs", (object) new int[0]);
      this.latestState.Add("addInclusiveOrgs", (object) new int[0]);
      this.latestState.Add("resetFolders", (object) new FileSystemEntry[0]);
      this.latestState.Add("resetFiles", (object) new FileSystemEntry[0]);
      this.latestState.Add("addFolderList", (object) new FileSystemEntry[0]);
      this.latestState.Add("addInclusiveFolderList", (object) new FileSystemEntry[0]);
      this.latestState.Add("addFileList", (object) new FileSystemEntry[0]);
    }

    private void updateLatestState()
    {
      this.latestState = new Dictionary<string, object>();
      this.latestState.Add("resetUsers", (object) this.resetUsers.ToArray());
      this.latestState.Add("addUsers", (object) this.addUsers.ToArray());
      this.latestState.Add("resetOrgs", (object) this.resetOrgs.ToArray());
      this.latestState.Add("addOrgs", (object) this.addOrgList.ToArray());
      this.latestState.Add("addInclusiveOrgs", (object) this.addInclusiveOrgList.ToArray());
      this.latestState.Add("resetFolders", (object) this.resetFolders.ToArray());
      this.latestState.Add("resetFiles", (object) this.resetFiles.ToArray());
      this.latestState.Add("addFolderList", (object) this.addFolderList.ToArray());
      this.latestState.Add("addInclusiveFolderList", (object) this.addInclusiveFolderList.ToArray());
      this.latestState.Add("addFileList", (object) this.addFileList.ToArray());
    }

    private void restoreUpdateListWithLatestState()
    {
      this.resetUpdateList();
      if (!this.latestState.ContainsKey("resetUsers"))
      {
        this.resetLatestState();
      }
      else
      {
        this.resetUsers.AddRange((IEnumerable<string>) (string[]) this.latestState["resetUsers"]);
        this.resetOrgs.AddRange((IEnumerable<int>) (int[]) this.latestState["resetOrgs"]);
        this.resetFolders.AddRange((IEnumerable<FileSystemEntry>) (FileSystemEntry[]) this.latestState["resetFolders"]);
        this.resetFiles.AddRange((IEnumerable<FileSystemEntry>) (FileSystemEntry[]) this.latestState["resetFiles"]);
        this.addInclusiveOrgList.AddRange((IEnumerable<int>) (int[]) this.latestState["addInclusiveOrgs"]);
        this.addUsers.AddRange((IEnumerable<string>) (string[]) this.latestState["addUsers"]);
        this.addOrgList.AddRange((IEnumerable<int>) (int[]) this.latestState["addOrgs"]);
        this.addFolderList.AddRange((IEnumerable<FileSystemEntry>) (FileSystemEntry[]) this.latestState["addFolderList"]);
        this.addInclusiveFolderList.AddRange((IEnumerable<FileSystemEntry>) (FileSystemEntry[]) this.latestState["addInclusiveFolderList"]);
        this.addFileList.AddRange((IEnumerable<FileSystemEntry>) (FileSystemEntry[]) this.latestState["addFileList"]);
      }
    }

    private void ResourceSetViewer_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.DialogResult != DialogResult.Cancel)
        return;
      this.Cursor = Cursors.WaitCursor;
      this.treeViewer.SuspendLayout();
      if (this.resetDataSourceTreeNode != null)
      {
        this.SetTempState(this.resetDataSourceTreeNode);
        this.restoreUpdateListWithLatestState();
      }
      else
      {
        this.SetTempState(this.resetDataSourceAclFileResource);
        this.resetUpdateList();
        this.resetLatestState();
      }
      this.treeViewer.ResumeLayout(false);
      this.treeViewer.Focus();
      this.Cursor = Cursors.Default;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ResourceSetViewer));
      this.contextMenuOrg = new ContextMenu();
      this.addOrgToGroupMenuItem = new MenuItem();
      this.addOrgInclusiveMenuItem = new MenuItem();
      this.delOrgFromGroupMenuItem = new MenuItem();
      this.menuItemExpandAll = new MenuItem();
      this.menuItemCollapseAll = new MenuItem();
      this.imgListTv = new ImageList(this.components);
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.groupContainer1 = new GroupContainer();
      this.gradientPanel1 = new GradientPanel();
      this.panelEx1 = new PanelEx();
      this.toolTip1 = new ToolTip(this.components);
      this.btnRemoveFromGroup = new StandardIconButton();
      this.btnAddAllToGroup = new IconButton();
      this.btnAddToGroup = new IconButton();
      this.lblResourcesInThisLevel = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.groupContainer1.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.panelEx1.SuspendLayout();
      ((ISupportInitialize) this.btnRemoveFromGroup).BeginInit();
      ((ISupportInitialize) this.btnAddAllToGroup).BeginInit();
      ((ISupportInitialize) this.btnAddToGroup).BeginInit();
      this.SuspendLayout();
      this.contextMenuOrg.MenuItems.AddRange(new MenuItem[5]
      {
        this.addOrgToGroupMenuItem,
        this.addOrgInclusiveMenuItem,
        this.delOrgFromGroupMenuItem,
        this.menuItemExpandAll,
        this.menuItemCollapseAll
      });
      this.addOrgToGroupMenuItem.Index = 0;
      this.addOrgToGroupMenuItem.Text = "Add to Group";
      this.addOrgToGroupMenuItem.Click += new EventHandler(this.addOrgToGroupMenuItem_Click);
      this.addOrgInclusiveMenuItem.Index = 1;
      this.addOrgInclusiveMenuItem.Text = "Add this and children to Group";
      this.addOrgInclusiveMenuItem.Click += new EventHandler(this.addOrgInclusiveMenuItem_Click);
      this.delOrgFromGroupMenuItem.Index = 2;
      this.delOrgFromGroupMenuItem.Text = "Remove from Group";
      this.delOrgFromGroupMenuItem.Click += new EventHandler(this.delOrgFromGroupMenuItem_Click);
      this.menuItemExpandAll.Index = 3;
      this.menuItemExpandAll.Text = "Expand All";
      this.menuItemExpandAll.Click += new EventHandler(this.menuItemExpandAll_Click);
      this.menuItemCollapseAll.Index = 4;
      this.menuItemCollapseAll.Text = "Collapse All";
      this.menuItemCollapseAll.Click += new EventHandler(this.menuItemCollapseAll_Click);
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.Transparent;
      this.imgListTv.Images.SetKeyName(0, "user_group_members-this-group-and-below.bmp");
      this.imgListTv.Images.SetKeyName(1, "user_group_members-this-group.bmp");
      this.imgListTv.Images.SetKeyName(2, "user_group_member-group.bmp");
      this.imgListTv.Images.SetKeyName(3, "user_group_members-this-group-default.bmp");
      this.imgListTv.Images.SetKeyName(4, "user_group_member-group-disabled.bmp");
      this.imgListTv.Images.SetKeyName(5, "user_group_folder.bmp");
      this.imgListTv.Images.SetKeyName(6, "");
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.btnOK.Location = new Point(427, 8);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(71, 22);
      this.btnOK.TabIndex = 26;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.btnCancel.Location = new Point(504, 8);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 27;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.treeViewer.AllowDrop = true;
      this.treeViewer.BorderStyle = BorderStyle.None;
      this.treeViewer.ContextMenu = this.contextMenuOrg;
      this.treeViewer.Cursor = Cursors.Default;
      this.treeViewer.Dock = DockStyle.Fill;
      this.treeViewer.Font = new Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.treeViewer.FullRowSelect = true;
      this.treeViewer.HideSelection = false;
      this.treeViewer.ImageIndex = 0;
      this.treeViewer.ImageList = this.imgListTv;
      this.treeViewer.Location = new Point(1, 26);
      this.treeViewer.Name = "treeViewer";
      this.treeViewer.SelectedImageIndex = 0;
      this.treeViewer.Size = new Size(584, 322);
      this.treeViewer.TabIndex = 1;
      this.treeViewer.AfterSelect += new TreeViewEventHandler(this.treeViewer_AfterSelect);
      this.treeViewer.MouseDown += new MouseEventHandler(this.treeViewer_MouseDown);
      this.groupContainer1.Controls.Add((Control) this.btnRemoveFromGroup);
      this.groupContainer1.Controls.Add((Control) this.btnAddAllToGroup);
      this.groupContainer1.Controls.Add((Control) this.btnAddToGroup);
      this.groupContainer1.Controls.Add((Control) this.treeViewer);
      this.groupContainer1.Controls.Add((Control) this.gradientPanel1);
      this.groupContainer1.Controls.Add((Control) this.panelEx1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(586, 455);
      this.groupContainer1.TabIndex = 28;
      this.groupContainer1.Text = "User Group Name";
      this.gradientPanel1.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.lblResourcesInThisLevel);
      this.gradientPanel1.Controls.Add((Control) this.label2);
      this.gradientPanel1.Controls.Add((Control) this.label3);
      this.gradientPanel1.Dock = DockStyle.Bottom;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(1, 348);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(584, 70);
      this.gradientPanel1.TabIndex = 1;
      this.panelEx1.Controls.Add((Control) this.btnOK);
      this.panelEx1.Controls.Add((Control) this.btnCancel);
      this.panelEx1.Dock = DockStyle.Bottom;
      this.panelEx1.Location = new Point(1, 418);
      this.panelEx1.Name = "panelEx1";
      this.panelEx1.Size = new Size(584, 36);
      this.panelEx1.TabIndex = 0;
      this.btnRemoveFromGroup.BackColor = Color.Transparent;
      this.btnRemoveFromGroup.Location = new Point(564, 4);
      this.btnRemoveFromGroup.Name = "btnRemoveFromGroup";
      this.btnRemoveFromGroup.Size = new Size(16, 16);
      this.btnRemoveFromGroup.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemoveFromGroup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRemoveFromGroup.TabIndex = 5;
      this.btnRemoveFromGroup.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemoveFromGroup, "Remove from Group");
      this.btnRemoveFromGroup.Click += new EventHandler(this.btnRemoveFromGroup_Click);
      this.btnAddAllToGroup.BackColor = Color.Transparent;
      this.btnAddAllToGroup.DisabledImage = (Image) Resources.member_user_groups_and_below_disabled;
      this.btnAddAllToGroup.Image = (Image) Resources.members_this_group_and_below;
      this.btnAddAllToGroup.Location = new Point(542, 4);
      this.btnAddAllToGroup.MouseOverImage = (Image) Resources.member_user_groups_and_below_over;
      this.btnAddAllToGroup.Name = "btnAddAllToGroup";
      this.btnAddAllToGroup.Size = new Size(16, 16);
      this.btnAddAllToGroup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddAllToGroup.TabIndex = 3;
      this.btnAddAllToGroup.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddAllToGroup, "Add to Group - this level and below");
      this.btnAddAllToGroup.Click += new EventHandler(this.btnAddAllToGroup_Click);
      this.btnAddToGroup.BackColor = Color.Transparent;
      this.btnAddToGroup.DisabledImage = (Image) Resources.member_user_groups_disabled;
      this.btnAddToGroup.Image = (Image) Resources.members_this_group;
      this.btnAddToGroup.Location = new Point(520, 4);
      this.btnAddToGroup.MouseOverImage = (Image) Resources.member_user_groups_over;
      this.btnAddToGroup.Name = "btnAddToGroup";
      this.btnAddToGroup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddToGroup.Size = new Size(16, 16);
      this.btnAddToGroup.TabIndex = 2;
      this.btnAddToGroup.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddToGroup, "Add to Group - this level/user only");
      this.btnAddToGroup.Click += new EventHandler(this.btnAddToGroup_Click);
      this.lblResourcesInThisLevel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblResourcesInThisLevel.BackColor = Color.Transparent;
      this.lblResourcesInThisLevel.Image = (Image) Resources.members_this_group;
      this.lblResourcesInThisLevel.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblResourcesInThisLevel.Location = new Point(12, 8);
      this.lblResourcesInThisLevel.Name = "lblResourcesInThisLevel";
      this.lblResourcesInThisLevel.Size = new Size(368, 16);
      this.lblResourcesInThisLevel.TabIndex = 2;
      this.lblResourcesInThisLevel.Text = "        All users in this level are members of the group";
      this.lblResourcesInThisLevel.TextAlign = ContentAlignment.MiddleLeft;
      this.label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label2.BackColor = Color.Transparent;
      this.label2.Image = (Image) Resources.members_this_group_and_below;
      this.label2.ImageAlign = ContentAlignment.MiddleLeft;
      this.label2.Location = new Point(12, 29);
      this.label2.Name = "label2";
      this.label2.Size = new Size(432, 16);
      this.label2.TabIndex = 3;
      this.label2.Text = "        All users in this level and the levels below are members of the group";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label3.BackColor = Color.Transparent;
      this.label3.Image = (Image) Resources.member_group;
      this.label3.ImageAlign = ContentAlignment.MiddleLeft;
      this.label3.Location = new Point(12, 49);
      this.label3.Name = "label3";
      this.label3.Size = new Size(368, 16);
      this.label3.TabIndex = 4;
      this.label3.Text = "        This user is a member of the group";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(586, 455);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (ResourceSetViewer);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "User Group Configuration";
      this.groupContainer1.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.panelEx1.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemoveFromGroup).EndInit();
      ((ISupportInitialize) this.btnAddAllToGroup).EndInit();
      ((ISupportInitialize) this.btnAddToGroup).EndInit();
      this.ResumeLayout(false);
    }

    private void saveCurrentResourceSet()
    {
      this.setHelper.saveResourceSet(this.currentResourceSet, this.treeViewer.Nodes, this.latestState);
    }

    private void treeViewer_Leave(object sender, EventArgs e)
    {
      if (this.currentResourceSet == null)
        return;
      this.saveCurrentResourceSet();
    }

    private bool nodeIsFolder(TreeNode tn)
    {
      bool flag = false;
      if (tn == null || tn.Tag == null)
        return false;
      if (tn.Tag is FileSystemEntry)
        flag = ((FileSystemEntry) tn.Tag).Type == FileSystemEntry.Types.Folder;
      if (tn.Tag is OrgInfo || (object) (tn.Tag as OrgInGroup) != null || (object) (tn.Tag as OrgInGroupRole) != null || tn.Tag is OrgInGroupContact || tn.Tag is FileSystemEntry & flag)
        return true;
      if ((object) (tn.Tag as UserInfo) != null || (object) (tn.Tag as UserInGroupLoan) != null || (object) (tn.Tag as UserInGroupRole) != null || (object) (tn.Tag as UserInGroupContact) != null || tn.Tag is FileSystemEntry && !flag)
        return false;
      throw new ApplicationException("Tree node's element type is not valid: " + tn.Tag.ToString());
    }

    private void treeViewer_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      TreeNode nodeAt = this.treeViewer.GetNodeAt(e.X, e.Y);
      if (nodeAt != null)
        this.treeViewer.SelectedNode = nodeAt;
      if (this.treeViewer.SelectedNode == null)
        return;
      TreeNode selectedNode = this.treeViewer.SelectedNode;
      if (this.nodeIsFolder(selectedNode))
      {
        if (selectedNode.ImageIndex == 0)
        {
          this.addOrgInclusiveMenuItem.Enabled = false;
          this.addOrgToGroupMenuItem.Enabled = true;
          this.delOrgFromGroupMenuItem.Enabled = true;
        }
        else if (selectedNode.ImageIndex == 1)
        {
          this.addOrgInclusiveMenuItem.Enabled = true;
          this.addOrgToGroupMenuItem.Enabled = false;
          this.delOrgFromGroupMenuItem.Enabled = true;
        }
        else
        {
          this.addOrgInclusiveMenuItem.Enabled = true;
          this.addOrgToGroupMenuItem.Enabled = true;
          this.delOrgFromGroupMenuItem.Enabled = false;
        }
        if (!(selectedNode.Tag is FileSystemEntry))
          return;
        this.addOrgToGroupMenuItem.Enabled = false;
      }
      else if (selectedNode.ImageIndex == 2)
      {
        this.addOrgInclusiveMenuItem.Enabled = false;
        this.addOrgToGroupMenuItem.Enabled = false;
        this.delOrgFromGroupMenuItem.Enabled = true;
      }
      else
      {
        this.addOrgInclusiveMenuItem.Enabled = false;
        this.addOrgToGroupMenuItem.Enabled = true;
        this.delOrgFromGroupMenuItem.Enabled = false;
      }
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
      return parent.ImageIndex == 1 && !this.nodeIsFolder(node) || this.existsInclusiveParent(parent);
    }

    private void unmarkChildNodesAsIncluded(TreeNode parentNode)
    {
      foreach (TreeNode node in parentNode.Nodes)
      {
        if (node.ImageIndex == 3)
          this.setNodeImageIndex(node, 5);
        else if (node.ImageIndex == 4 && parentNode.ImageIndex == 5)
          this.setNodeImageIndex(node, 6);
        if (node.ImageIndex != 0 && node.Nodes.Count > 0)
          this.unmarkChildNodesAsIncluded(node);
      }
    }

    private void markChildNodesAsIncluded(TreeNode parentNode)
    {
      foreach (TreeNode node in parentNode.Nodes)
      {
        if (node.ImageIndex == 5)
          this.setNodeImageIndex(node, 3);
        else if (node.ImageIndex == 6)
          this.setNodeImageIndex(node, 4);
        if (node.ImageIndex != 0 && node.Nodes.Count > 0)
          this.markChildNodesAsIncluded(node);
      }
    }

    private void updateList(TreeNode node, bool remove, bool inclusive)
    {
      if (node.Tag is OrgInfo || (object) (node.Tag as UserInfo) != null)
      {
        bool flag = false;
        if (node.Tag is OrgInfo)
          flag = true;
        if (flag)
        {
          int oid = ((OrgInfo) node.Tag).Oid;
          if (!this.resetOrgs.Contains(oid))
            this.resetOrgs.Add(oid);
          if (remove)
          {
            if (this.addInclusiveOrgList.Contains(oid))
              this.addInclusiveOrgList.Remove(oid);
            if (!this.addOrgList.Contains(oid))
              return;
            this.addOrgList.Remove(oid);
          }
          else if (inclusive)
          {
            if (!this.addInclusiveOrgList.Contains(oid))
              this.addInclusiveOrgList.Add(oid);
            if (!this.addOrgList.Contains(oid))
              return;
            this.addOrgList.Remove(oid);
          }
          else
          {
            if (this.addInclusiveOrgList.Contains(oid))
              this.addInclusiveOrgList.Remove(oid);
            if (this.addOrgList.Contains(oid))
              return;
            this.addOrgList.Add(oid);
          }
        }
        else
        {
          string userid = ((UserInfo) node.Tag).Userid;
          if (!this.resetUsers.Contains(userid))
            this.resetUsers.Add(userid);
          if (remove)
          {
            if (!this.addUsers.Contains(userid))
              return;
            this.addUsers.Remove(userid);
          }
          else
          {
            if (this.addUsers.Contains(userid))
              return;
            this.addUsers.Add(userid);
          }
        }
      }
      else
      {
        if (!(node.Tag is FileSystemEntry))
          return;
        FileSystemEntry tag = (FileSystemEntry) node.Tag;
        if (tag.Type == FileSystemEntry.Types.Folder)
        {
          if (!this.resetFolders.Contains(tag))
            this.resetFolders.Add(tag);
          if (remove)
          {
            if (this.addInclusiveFolderList.Contains(tag))
              this.addInclusiveFolderList.Remove(tag);
            if (!this.addFolderList.Contains(tag))
              return;
            this.addFolderList.Remove(tag);
          }
          else if (inclusive)
          {
            if (!this.addInclusiveFolderList.Contains(tag))
              this.addInclusiveFolderList.Add(tag);
            if (!this.addFolderList.Contains(tag))
              return;
            this.addFolderList.Remove(tag);
          }
          else
          {
            if (this.addInclusiveFolderList.Contains(tag))
              this.addInclusiveFolderList.Remove(tag);
            if (this.addFolderList.Contains(tag))
              return;
            this.addFolderList.Add(tag);
          }
        }
        else
        {
          if (tag.Type != FileSystemEntry.Types.File)
            return;
          if (!this.resetFiles.Contains(tag))
            this.resetFiles.Add(tag);
          if (remove)
          {
            if (!this.addFileList.Contains(tag))
              return;
            this.addFileList.Remove(tag);
          }
          else
          {
            if (this.addFileList.Contains(tag))
              return;
            this.addFileList.Add(tag);
          }
        }
      }
    }

    private void addOrgToGroupMenuItem_Click(object sender, EventArgs e)
    {
      this.dirty = true;
      TreeNode selectedNode = this.treeViewer.SelectedNode;
      int imageIndex = selectedNode.ImageIndex;
      if (this.nodeIsFolder(selectedNode))
        this.setNodeImageIndex(selectedNode, 1);
      else
        this.setNodeImageIndex(selectedNode, 2);
      if (imageIndex == 0 && !this.existsInclusiveParent(selectedNode))
        this.unmarkChildNodesAsIncluded(selectedNode);
      if (this.nodeIsFolder(selectedNode))
      {
        foreach (TreeNode node in selectedNode.Nodes)
        {
          if (!this.nodeIsFolder(node) && node.ImageIndex == 6)
            this.setNodeImageIndex(node, 4);
        }
      }
      this.updateList(selectedNode, false, false);
      this.treeViewer_AfterSelect((object) null, (TreeViewEventArgs) null);
    }

    private void addOrgInclusiveMenuItem_Click(object sender, EventArgs e)
    {
      this.dirty = true;
      TreeNode selectedNode = this.treeViewer.SelectedNode;
      this.setNodeImageIndex(selectedNode, 0);
      this.markChildNodesAsIncluded(selectedNode);
      this.updateList(selectedNode, false, true);
      this.treeViewer_AfterSelect((object) null, (TreeViewEventArgs) null);
    }

    private void delOrgFromGroupMenuItem_Click(object sender, EventArgs e)
    {
      this.dirty = true;
      TreeNode selectedNode = this.treeViewer.SelectedNode;
      int imageIndex = selectedNode.ImageIndex;
      bool flag = this.existsInclusiveParent(selectedNode);
      if (this.nodeIsFolder(selectedNode))
      {
        if (flag)
          this.setNodeImageIndex(selectedNode, 3);
        else
          this.setNodeImageIndex(selectedNode, 5);
      }
      else if (flag)
        this.setNodeImageIndex(selectedNode, 4);
      else
        this.setNodeImageIndex(selectedNode, 6);
      if (!flag)
      {
        switch (imageIndex)
        {
          case 0:
            this.unmarkChildNodesAsIncluded(selectedNode);
            break;
          case 1:
            IEnumerator enumerator = selectedNode.Nodes.GetEnumerator();
            try
            {
              while (enumerator.MoveNext())
              {
                TreeNode current = (TreeNode) enumerator.Current;
                if (!this.nodeIsFolder(current) && current.ImageIndex == 4)
                  this.setNodeImageIndex(current, 6);
              }
              break;
            }
            finally
            {
              if (enumerator is IDisposable disposable)
                disposable.Dispose();
            }
        }
      }
      this.updateList(selectedNode, true, false);
      this.treeViewer_AfterSelect((object) null, (TreeViewEventArgs) null);
    }

    private void btnAddToGroup_Click(object sender, EventArgs e)
    {
      this.addOrgToGroupMenuItem_Click((object) null, (EventArgs) null);
    }

    private void btnAddAllToGroup_Click(object sender, EventArgs e)
    {
      this.addOrgInclusiveMenuItem_Click((object) null, (EventArgs) null);
    }

    private void btnRemoveFromGroup_Click(object sender, EventArgs e)
    {
      this.delOrgFromGroupMenuItem_Click((object) null, (EventArgs) null);
    }

    private void btnExpand_Click(object sender, EventArgs e)
    {
      if (this.treeViewer.SelectedNode == null)
        return;
      this.treeViewer.SelectedNode.Expand();
    }

    private void btnExpandAll_Click(object sender, EventArgs e)
    {
      if (this.treeViewer.SelectedNode == null)
        return;
      this.treeViewer.SelectedNode.ExpandAll();
    }

    private void btnCollapse_Click(object sender, EventArgs e)
    {
      if (this.treeViewer.SelectedNode == null)
        return;
      this.treeViewer.SelectedNode.Collapse();
    }

    private void menuItemExpand_Click(object sender, EventArgs e)
    {
      this.btnExpand_Click((object) null, (EventArgs) null);
    }

    private void menuItemExpandAll_Click(object sender, EventArgs e)
    {
      this.btnExpandAll_Click((object) null, (EventArgs) null);
    }

    private void menuItemCollapse_Click(object sender, EventArgs e)
    {
      this.btnCollapse_Click((object) null, (EventArgs) null);
    }

    private void treeViewer_AfterSelect(object sender, TreeViewEventArgs e)
    {
      TreeNode selectedNode = this.treeViewer.SelectedNode;
      if (selectedNode == null)
        return;
      if (this.nodeIsFolder(selectedNode))
      {
        if (selectedNode.ImageIndex == 0)
        {
          this.btnAddAllToGroup.Enabled = false;
          if (this.lblResourcesInThisLevel.Visible)
            this.btnAddToGroup.Enabled = true;
          else
            this.btnAddToGroup.Enabled = false;
          this.btnRemoveFromGroup.Enabled = true;
        }
        else if (selectedNode.ImageIndex == 1)
        {
          this.btnAddAllToGroup.Enabled = true;
          this.btnAddToGroup.Enabled = false;
          this.btnRemoveFromGroup.Enabled = true;
        }
        else
        {
          this.btnAddAllToGroup.Enabled = true;
          if (this.lblResourcesInThisLevel.Visible)
            this.btnAddToGroup.Enabled = true;
          else
            this.btnAddToGroup.Enabled = false;
          this.btnRemoveFromGroup.Enabled = false;
        }
      }
      else if (selectedNode.ImageIndex == 2)
      {
        this.btnAddAllToGroup.Enabled = false;
        this.btnAddToGroup.Enabled = false;
        this.btnRemoveFromGroup.Enabled = true;
      }
      else
      {
        this.btnAddAllToGroup.Enabled = false;
        this.btnAddToGroup.Enabled = true;
        this.btnRemoveFromGroup.Enabled = false;
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.updateLatestState();
      if (this.DirtyFlagChanged != null)
        this.DirtyFlagChanged((object) this, (EventArgs) null);
      this.DialogResult = DialogResult.OK;
    }

    public void Save()
    {
      this.saveCurrentResourceSet();
      this.dirty = false;
    }

    public bool HasBeenModified() => this.dirty;

    public TreeViewer GetCurrentTreeView() => this.treeViewer;

    public void deselectNode(TreeNode node)
    {
      this.treeViewer.SelectedNode = node;
      this.delOrgFromGroupMenuItem_Click((object) null, (EventArgs) null);
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.ResourceSetViewer_FormClosing((object) null, (FormClosingEventArgs) null);
    }

    public void ResetTreeWithMemoryData()
    {
      this.dirty = true;
      this.btnCancel_Click((object) null, (EventArgs) null);
    }

    public void RemoveTreeNode(TreeNode node)
    {
      if (node.Tag is OrgInfo || node.Tag is OrgInGroupLoan || node.Tag is OrgInGroupContact || (object) (node.Tag as OrgInGroupRole) != null)
      {
        int num = -1;
        if (node.Tag is OrgInfo)
          num = ((OrgInfo) node.Tag).Oid;
        else if (node.Tag is OrgInGroupLoan)
          num = ((OrgInGroup) node.Tag).OrgID;
        else if (node.Tag is OrgInGroupContact)
          num = ((OrgInGroup) node.Tag).OrgID;
        else if ((object) (node.Tag as OrgInGroupRole) != null)
          num = ((OrgInGroup) node.Tag).OrgID;
        if (!this.resetOrgs.Contains(num))
          this.resetOrgs.Add(num);
        if (this.addOrgList.Contains(num))
          this.addOrgList.Remove(num);
        if (this.addInclusiveOrgList.Contains(num))
          this.addInclusiveOrgList.Remove(num);
      }
      else if ((object) (node.Tag as UserInfo) != null || (object) (node.Tag as UserInGroupLoan) != null || (object) (node.Tag as UserInGroupContact) != null || (object) (node.Tag as UserInGroupRole) != null)
      {
        string str = (string) null;
        if ((object) (node.Tag as UserInfo) != null)
          str = ((UserInfo) node.Tag).Userid;
        else if ((object) (node.Tag as UserInGroupLoan) != null)
          str = ((UserInGroupLoan) node.Tag).UserID;
        else if ((object) (node.Tag as UserInGroupContact) != null)
          str = ((UserInGroupContact) node.Tag).UserID;
        else if ((object) (node.Tag as UserInGroupRole) != null)
          str = ((UserInGroupRole) node.Tag).UserID;
        if (!this.resetUsers.Contains(str))
          this.resetUsers.Add(str);
        if (this.addUsers.Contains(str))
          this.addUsers.Remove(str);
      }
      else
      {
        FileSystemEntry fileSystemEntry = node.Tag is FileSystemEntry ? (FileSystemEntry) node.Tag : throw new Exception("Unknown type " + (object) node.Tag.GetType());
        if (fileSystemEntry.Type == FileSystemEntry.Types.File)
        {
          if (!this.resetFiles.Contains(fileSystemEntry))
            this.resetFiles.Add(fileSystemEntry);
          if (this.addFileList.Contains(fileSystemEntry))
            this.addFileList.Remove(fileSystemEntry);
        }
        else
        {
          if (!this.resetFolders.Contains(fileSystemEntry))
            this.resetFolders.Add(fileSystemEntry);
          if (this.addFolderList.Contains(fileSystemEntry))
            this.addFolderList.Remove(fileSystemEntry);
          if (this.addInclusiveFolderList.Contains(fileSystemEntry))
            this.addInclusiveFolderList.Remove(fileSystemEntry);
        }
      }
      this.updateLatestState();
      this.dirty = true;
      if (this.DirtyFlagChanged == null)
        return;
      this.DirtyFlagChanged((object) this, (EventArgs) null);
    }

    private void menuItemCollapseAll_Click(object sender, EventArgs e)
    {
      this.btnCollapse_Click((object) null, (EventArgs) null);
    }

    private enum ActionType
    {
      Add,
      Remove,
      Update,
    }
  }
}
