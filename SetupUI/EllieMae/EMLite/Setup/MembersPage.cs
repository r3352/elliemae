// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MembersPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
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
  public class MembersPage : Form, IGroupSecurityPage
  {
    private Sessions.Session session;
    private int _currentGroupId;
    private bool reloadFromDB = true;
    private bool firstTime = true;
    private ResourceSetViewer orgView;
    private OrgInGroup[] orgs;
    private UserInfo[] users;
    private AclGroup aclGroup;
    private IContainer components;
    private ListView lvGroupMember;
    private ColumnHeader columnHeaderName;
    private ColumnHeader columnHeaderInclusive;
    private ColumnHeader columnHeaderUserID;
    private ColumnHeader columnHeaderAccount;
    private ColumnHeader columnHeaderLogin;
    private Button btnFindInHierarchy;
    private ImageList imgListTv;
    private GroupContainer gcMembers;
    private StandardIconButton stdIconBtnConfigGroup;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton stdIconBtnRemoveFromGroup;
    private UserGroupLegend legend;
    private ToolTip toolTip1;
    private ListViewColumnSorter lvColumnSorter;

    public event EventHandler DirtyFlagChanged;

    public MembersPage(Sessions.Session session, int groupId, EventHandler dirtyFlagChanged)
    {
      this.session = session;
      if (dirtyFlagChanged != null)
        this.DirtyFlagChanged += dirtyFlagChanged;
      this.InitializeComponent();
      this.btnFindInHierarchy.Enabled = false;
      this.stdIconBtnRemoveFromGroup.Enabled = false;
      this.lvColumnSorter = new ListViewColumnSorter();
      this.lvColumnSorter.SortColumn = 0;
      this.lvColumnSorter.SortType = 0;
      this.lvColumnSorter.Order = SortOrder.Ascending;
      this.lvGroupMember.ListViewItemSorter = (IComparer) this.lvColumnSorter;
      this.SetSortArrow(this.lvGroupMember.Columns[0], SortOrder.Ascending);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MembersPage));
      this.btnFindInHierarchy = new Button();
      this.lvGroupMember = new ListView();
      this.columnHeaderName = new ColumnHeader();
      this.columnHeaderInclusive = new ColumnHeader();
      this.columnHeaderUserID = new ColumnHeader();
      this.columnHeaderAccount = new ColumnHeader();
      this.columnHeaderLogin = new ColumnHeader();
      this.imgListTv = new ImageList(this.components);
      this.gcMembers = new GroupContainer();
      this.stdIconBtnConfigGroup = new StandardIconButton();
      this.verticalSeparator1 = new VerticalSeparator();
      this.stdIconBtnRemoveFromGroup = new StandardIconButton();
      this.legend = new UserGroupLegend();
      this.toolTip1 = new ToolTip(this.components);
      this.gcMembers.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnConfigGroup).BeginInit();
      ((ISupportInitialize) this.stdIconBtnRemoveFromGroup).BeginInit();
      this.SuspendLayout();
      this.btnFindInHierarchy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnFindInHierarchy.BackColor = SystemColors.Control;
      this.btnFindInHierarchy.Location = new Point(375, 4);
      this.btnFindInHierarchy.Name = "btnFindInHierarchy";
      this.btnFindInHierarchy.Size = new Size(164, 31);
      this.btnFindInHierarchy.TabIndex = 1;
      this.btnFindInHierarchy.Text = "Find in Hierarchy";
      this.btnFindInHierarchy.UseVisualStyleBackColor = true;
      this.btnFindInHierarchy.Click += new EventHandler(this.btnFindInHierarchy_Click);
      this.lvGroupMember.AllowDrop = true;
      this.lvGroupMember.BorderStyle = BorderStyle.None;
      this.lvGroupMember.Columns.AddRange(new ColumnHeader[5]
      {
        this.columnHeaderName,
        this.columnHeaderInclusive,
        this.columnHeaderUserID,
        this.columnHeaderLogin,
        this.columnHeaderAccount
      });
      this.lvGroupMember.Dock = DockStyle.Fill;
      this.lvGroupMember.FullRowSelect = true;
      this.lvGroupMember.GridLines = true;
      this.lvGroupMember.HideSelection = false;
      this.lvGroupMember.Location = new Point(1, 26);
      this.lvGroupMember.MultiSelect = false;
      this.lvGroupMember.Name = "lvGroupMember";
      this.lvGroupMember.Size = new Size(543, 309);
      this.lvGroupMember.SmallImageList = this.imgListTv;
      this.lvGroupMember.Sorting = SortOrder.Ascending;
      this.lvGroupMember.TabIndex = 17;
      this.lvGroupMember.UseCompatibleStateImageBehavior = false;
      this.lvGroupMember.View = View.Details;
      this.lvGroupMember.ColumnClick += new ColumnClickEventHandler(this.lvGroupMember_ColumnClick);
      this.lvGroupMember.SelectedIndexChanged += new EventHandler(this.lvGroupMember_SelectedIndexChanged);
      this.columnHeaderName.Text = "Name";
      this.columnHeaderName.Width = 148;
      this.columnHeaderInclusive.Text = "Include levels below";
      this.columnHeaderInclusive.Width = 114;
      this.columnHeaderUserID.Text = "User ID";
      this.columnHeaderUserID.Width = 120;
      this.columnHeaderLogin.Text = "Login";
      this.columnHeaderLogin.Width = 80;
      this.columnHeaderAccount.Text = "Account";
      this.columnHeaderAccount.Width = 80;
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.White;
      this.imgListTv.Images.SetKeyName(0, "members-this-group-and-below.png");
      this.imgListTv.Images.SetKeyName(1, "members-this-group.png");
      this.imgListTv.Images.SetKeyName(2, "member-group.png");
      this.imgListTv.Images.SetKeyName(3, "");
      this.imgListTv.Images.SetKeyName(4, "");
      this.imgListTv.Images.SetKeyName(5, "");
      this.imgListTv.Images.SetKeyName(6, "");
      this.gcMembers.Controls.Add((Control) this.stdIconBtnConfigGroup);
      this.gcMembers.Controls.Add((Control) this.verticalSeparator1);
      this.gcMembers.Controls.Add((Control) this.stdIconBtnRemoveFromGroup);
      this.gcMembers.Controls.Add((Control) this.lvGroupMember);
      this.gcMembers.Controls.Add((Control) this.btnFindInHierarchy);
      this.gcMembers.Controls.Add((Control) this.legend);
      this.gcMembers.Dock = DockStyle.Fill;
      this.gcMembers.HeaderForeColor = SystemColors.ControlText;
      this.gcMembers.Location = new Point(0, 0);
      this.gcMembers.Name = "gcMembers";
      this.gcMembers.Size = new Size(545, 431);
      this.gcMembers.TabIndex = 20;
      this.gcMembers.Text = "Current Members";
      this.stdIconBtnConfigGroup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnConfigGroup.BackColor = Color.Transparent;
      this.stdIconBtnConfigGroup.Location = new Point(294, 7);
      this.stdIconBtnConfigGroup.Name = "stdIconBtnConfigGroup";
      this.stdIconBtnConfigGroup.Size = new Size(25, 24);
      this.stdIconBtnConfigGroup.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnConfigGroup.TabIndex = 20;
      this.stdIconBtnConfigGroup.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnConfigGroup, "Configure Group");
      this.stdIconBtnConfigGroup.Click += new EventHandler(this.btnConfigGroup_Click);
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(364, 7);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 19;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.stdIconBtnRemoveFromGroup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnRemoveFromGroup.BackColor = Color.Transparent;
      this.stdIconBtnRemoveFromGroup.Location = new Point(329, 7);
      this.stdIconBtnRemoveFromGroup.Name = "stdIconBtnRemoveFromGroup";
      this.stdIconBtnRemoveFromGroup.Size = new Size(26, 24);
      this.stdIconBtnRemoveFromGroup.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnRemoveFromGroup.TabIndex = 18;
      this.stdIconBtnRemoveFromGroup.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnRemoveFromGroup, "Remove from Group");
      this.stdIconBtnRemoveFromGroup.Click += new EventHandler(this.btnRemoveFromGroup_Click);
      this.legend.BackColor = Color.Transparent;
      this.legend.Dock = DockStyle.Bottom;
      this.legend.Location = new Point(1, 335);
      this.legend.Margin = new Padding(4, 5, 4, 5);
      this.legend.Name = "legend";
      this.legend.Size = new Size(543, 95);
      this.legend.TabIndex = 0;
      this.AutoScaleBaseSize = new Size(8, 19);
      this.AutoScroll = true;
      this.ClientSize = new Size(545, 431);
      this.Controls.Add((Control) this.gcMembers);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (MembersPage);
      this.gcMembers.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnConfigGroup).EndInit();
      ((ISupportInitialize) this.stdIconBtnRemoveFromGroup).EndInit();
      this.ResumeLayout(false);
    }

    private int currentGroupId
    {
      get => this._currentGroupId;
      set
      {
        this.aclGroup = this.session.AclGroupManager.GetGroupById(value);
        if (this.aclGroup != (AclGroup) null && this.aclGroup.Name == "All Users")
        {
          this.stdIconBtnConfigGroup.Enabled = false;
          this.stdIconBtnRemoveFromGroup.Enabled = false;
          this.btnFindInHierarchy.Enabled = false;
        }
        else
        {
          this.stdIconBtnConfigGroup.Enabled = true;
          this.btnFindInHierarchy.Enabled = this.lvGroupMember.SelectedItems.Count > 0;
        }
        if (this.firstTime)
        {
          this._currentGroupId = value;
          this.loadGroupMembers(value);
          this.reloadFromDB = true;
          this.orgView = (ResourceSetViewer) null;
        }
        else if (!this.firstTime && !this._currentGroupId.Equals(value))
        {
          this.firstTime = true;
          this._currentGroupId = value;
          this.loadGroupMembers(value);
          this.reloadFromDB = true;
          this.orgView = (ResourceSetViewer) null;
        }
        else if (!this.firstTime && this._currentGroupId.Equals(value))
          this.reloadFromDB = false;
        this.btnFindInHierarchy.Enabled = false;
      }
    }

    public AclGroup CurrentAclGroup => this.aclGroup;

    public virtual void SetGroup(int groupId) => this.currentGroupId = groupId;

    private void loadGroupMembers(int aclGroupId)
    {
      this.lvGroupMember.Items.Clear();
      Dictionary<string, object> membersInGroup = this.session.AclGroupManager.GetMembersInGroup(aclGroupId);
      this.orgs = !membersInGroup.ContainsKey("OrgList") || membersInGroup["OrgList"] == null ? new OrgInGroup[0] : (OrgInGroup[]) membersInGroup["OrgList"];
      this.users = !membersInGroup.ContainsKey("UserList") || membersInGroup["UserList"] == null ? new UserInfo[0] : (UserInfo[]) membersInGroup["UserList"];
      int index1 = 0;
      ListViewItem[] items = new ListViewItem[this.orgs.Length];
      for (; index1 < this.orgs.Length; ++index1)
      {
        OrgInGroup org = this.orgs[index1];
        string str = org.IsInclusive ? "Yes" : "No";
        int num = org.IsInclusive ? 0 : 1;
        items[index1] = new ListViewItem(new string[5]
        {
          org.OrgName,
          str,
          string.Empty,
          string.Empty,
          string.Empty
        });
        items[index1].ImageIndex = num;
        items[index1].Tag = (object) new TreeNode(org.OrgName)
        {
          Tag = (object) new OrgInfo(org.OrgID, org.OrgName, ""),
          ImageIndex = num,
          SelectedImageIndex = num
        };
      }
      this.lvGroupMember.Items.AddRange(items);
      List<ListViewItem> listViewItemList = new List<ListViewItem>();
      for (int index2 = 0; index2 < this.users.Length; ++index2)
      {
        UserInfo user = this.users[index2];
        if (!(user == (UserInfo) null))
        {
          listViewItemList.Add(new ListViewItem(new string[5]
          {
            user.FullName + " (" + user.Userid + ")",
            string.Empty,
            user.Userid,
            user.Locked ? "Disabled" : "Enabled",
            user.Status.ToString()
          }));
          listViewItemList[listViewItemList.Count - 1].ImageIndex = 2;
          listViewItemList[listViewItemList.Count - 1].Tag = (object) new TreeNode(user.FullName + " (" + user.Userid + ")")
          {
            Tag = (object) user,
            ImageIndex = 2,
            SelectedImageIndex = 2
          };
        }
      }
      if (listViewItemList.Count <= 0)
        return;
      this.lvGroupMember.Items.AddRange(listViewItemList.ToArray());
    }

    private void addNodeToList(TreeNode node)
    {
      if (node.Text == "<DUMMY NODE>")
        return;
      TreeNode treeNode = (TreeNode) node.Clone();
      switch (treeNode.ImageIndex)
      {
        case 0:
          this.lvGroupMember.Items.Add(new ListViewItem(new string[5]
          {
            treeNode.Text,
            "Yes",
            string.Empty,
            string.Empty,
            string.Empty
          })
          {
            ImageIndex = treeNode.ImageIndex,
            Tag = (object) treeNode
          });
          break;
        case 1:
          this.lvGroupMember.Items.Add(new ListViewItem(new string[5]
          {
            treeNode.Text,
            "No",
            string.Empty,
            string.Empty,
            string.Empty
          })
          {
            ImageIndex = treeNode.ImageIndex,
            Tag = (object) treeNode
          });
          break;
        case 2:
          UserInfo tag = (UserInfo) treeNode.Tag;
          this.lvGroupMember.Items.Add(new ListViewItem(new string[5]
          {
            treeNode.Text,
            string.Empty,
            tag.Userid,
            tag.Locked ? "Disabled" : "Enabled",
            tag.Status.ToString()
          })
          {
            ImageIndex = treeNode.ImageIndex,
            Tag = (object) treeNode
          });
          break;
      }
      foreach (TreeNode node1 in node.Nodes)
        this.addNodeToList(node1);
    }

    private void loadGroupMembers(TreeViewer tv)
    {
      this.lvGroupMember.Items.Clear();
      foreach (TreeNode node in tv.Nodes)
        this.addNodeToList(node);
    }

    private void btnConfigGroup_Click(object sender, EventArgs e)
    {
      if (this.reloadFromDB)
      {
        this.Cursor = Cursors.WaitCursor;
        this.orgView = new ResourceSetViewer(this.session, (object) this.aclGroup, this.DirtyFlagChanged, this.aclGroup.ID);
        this.Cursor = Cursors.Default;
        this.orgView.SetResetDataSourceTreeNode = this.GetResetTreeView();
      }
      this.firstTime = false;
      this.reloadFromDB = false;
      if (DialogResult.Cancel == this.orgView.ShowDialog((IWin32Window) this))
        return;
      this.loadGroupMembers(this.orgView.GetCurrentTreeView());
      this.orgView.SetResetDataSourceTreeNode = this.GetResetTreeView();
    }

    private void SetSortArrow(ColumnHeader head, SortOrder order)
    {
      if (head.Text.EndsWith(" ▲") || head.Text.EndsWith(" ▼"))
        head.Text = head.Text.Substring(0, head.Text.Length - 2);
      if (order != SortOrder.Ascending)
      {
        if (order != SortOrder.Descending)
          return;
        head.Text += " ▼";
      }
      else
        head.Text += " ▲";
    }

    private void lvGroupMember_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      if (e.Column == this.lvColumnSorter.SortColumn)
      {
        this.lvColumnSorter.Order = this.lvColumnSorter.Order != SortOrder.Ascending ? SortOrder.Ascending : SortOrder.Descending;
      }
      else
      {
        this.lvColumnSorter.SortColumn = e.Column;
        this.lvColumnSorter.Order = SortOrder.Ascending;
      }
      this.lvGroupMember.Sort();
      for (int index = 0; index < this.lvGroupMember.Columns.Count; ++index)
      {
        if (index == e.Column)
          this.SetSortArrow(this.lvGroupMember.Columns[index], this.lvColumnSorter.Order);
        else
          this.SetSortArrow(this.lvGroupMember.Columns[index], SortOrder.None);
      }
    }

    private TreeNode[] GetResetTreeView()
    {
      ArrayList arrayList = new ArrayList();
      foreach (ListViewItem listViewItem in this.lvGroupMember.Items)
        arrayList.Add((object) (TreeNode) listViewItem.Tag);
      return (TreeNode[]) arrayList.ToArray(typeof (TreeNode));
    }

    private void btnFindInHierarchy_Click(object sender, EventArgs e)
    {
      if (this.lvGroupMember.SelectedItems == null || this.lvGroupMember.SelectedItems.Count <= 0)
        return;
      if (this.reloadFromDB)
      {
        this.Cursor = Cursors.WaitCursor;
        this.orgView = new ResourceSetViewer(this.session, (object) this.aclGroup, this.DirtyFlagChanged, this.aclGroup.ID);
        this.Cursor = Cursors.Default;
        this.orgView.SetResetDataSourceTreeNode = this.GetResetTreeView();
      }
      this.firstTime = false;
      this.reloadFromDB = false;
      this.orgView.PreSelectNode((TreeNode) this.lvGroupMember.SelectedItems[0].Tag);
      if (DialogResult.Cancel == this.orgView.ShowDialog((IWin32Window) this))
        return;
      this.loadGroupMembers(this.orgView.GetCurrentTreeView());
      this.orgView.SetResetDataSourceTreeNode = this.GetResetTreeView();
    }

    public void SaveData()
    {
      if (this.firstTime || this.orgView == null)
        return;
      this.orgView.Save();
    }

    public bool HasBeenModified()
    {
      return !this.firstTime && this.orgView != null && this.orgView.HasBeenModified();
    }

    public void ResetData()
    {
      this.firstTime = true;
      this.currentGroupId = this.currentGroupId;
    }

    private void lvGroupMember_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.lvGroupMember.SelectedItems != null && this.lvGroupMember.SelectedItems.Count > 0)
      {
        if (this.aclGroup != (AclGroup) null && this.aclGroup.Name != "All Users")
          this.btnFindInHierarchy.Enabled = this.stdIconBtnRemoveFromGroup.Enabled = this.lvGroupMember.SelectedItems.Count > 0;
        else
          this.btnFindInHierarchy.Enabled = this.stdIconBtnRemoveFromGroup.Enabled = false;
      }
      else
        this.btnFindInHierarchy.Enabled = this.stdIconBtnRemoveFromGroup.Enabled = false;
    }

    private void btnRemoveFromGroup_Click(object sender, EventArgs e)
    {
      if (this.lvGroupMember.SelectedItems == null || this.lvGroupMember.SelectedItems.Count <= 0)
        return;
      this.Cursor = Cursors.WaitCursor;
      if (this.reloadFromDB)
        this.orgView = new ResourceSetViewer(this.session, (object) this.aclGroup, this.DirtyFlagChanged, this.aclGroup.ID);
      List<TreeNode> treeNodeList = new List<TreeNode>();
      foreach (ListViewItem selectedItem in this.lvGroupMember.SelectedItems)
      {
        treeNodeList.Add((TreeNode) selectedItem.Tag);
        selectedItem.Remove();
      }
      this.firstTime = false;
      this.reloadFromDB = false;
      this.orgView.SetResetDataSourceTreeNode = this.GetResetTreeView();
      this.orgView.ResetTreeWithMemoryData();
      foreach (TreeNode node in treeNodeList)
        this.orgView.RemoveTreeNode(node);
      this.Cursor = Cursors.Default;
    }
  }
}
