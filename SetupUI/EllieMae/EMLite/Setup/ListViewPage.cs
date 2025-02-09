// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ListViewPage
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
  public class ListViewPage : Form, IGroupSecurityPage
  {
    private Sessions.Session session;
    private Hashtable tmpRoleHideDisabledHash;
    private int _currentGroupId = -1;
    private bool loadingSettingsForRole;
    private int _currentRoleId = -1;
    private Label label1;
    private Label label2;
    private Label label6;
    private ComboBox cmbBoxRole;
    private RadioButton rbtnAllAssociates;
    private RadioButton rbtnAssociatesBelow;
    private RadioButton rbtnSelectedAssociates;
    private ListView listViewMembers;
    private ColumnHeader columnHeaderName;
    private ImageList imgListTv;
    private IContainer components;
    private ResourceSetViewer orgView;
    private string userID = "";
    private bool firstTime = true;
    private AclGroup[] groupList;
    private CheckBox cxbAll;
    private CheckBox cxbOrg;
    private CheckBox cxbGroup;
    private CheckBox chbFilter;
    private bool personal;
    private bool dirty;
    private AclGroupRoleMembers members;
    private Hashtable tmpRoleDataHash;
    private Hashtable tmpRoleResetTreeHash;
    private GroupContainer gcRoleList;
    private GroupContainer gcLO;
    private StandardIconButton stdIconBtnAdd;
    private StandardIconButton stdIconBtnDelete;
    private GradientPanel gradientPanel1;
    private GradientPanel gradientPanel3;
    private GradientPanel gradientPanel2;
    private UserGroupLegend legend;

    public event EventHandler DirtyFlagChanged;

    public ListViewPage(Sessions.Session session, int personaId, EventHandler dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.session = session;
      this.tmpRoleDataHash = new Hashtable();
      this.tmpRoleResetTreeHash = new Hashtable();
      this.tmpRoleHideDisabledHash = new Hashtable();
      this.loadRoles();
      this.currentGroupId = personaId;
      this.DirtyFlagChanged += dirtyFlagChanged;
    }

    public ListViewPage(
      Sessions.Session session,
      string userID,
      AclGroup[] groups,
      EventHandler dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.session = session;
      this.personal = true;
      this.userID = userID;
      this.groupList = groups;
      this.makeReadOnly();
      this.tmpRoleDataHash = new Hashtable();
      this.tmpRoleResetTreeHash = new Hashtable();
      this.tmpRoleHideDisabledHash = new Hashtable();
      this.loadRoles();
      this.cmbBoxRole.SelectedIndex = 0;
      this.DirtyFlagChanged += dirtyFlagChanged;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ListViewPage));
      this.label1 = new Label();
      this.cmbBoxRole = new ComboBox();
      this.label2 = new Label();
      this.rbtnAllAssociates = new RadioButton();
      this.rbtnAssociatesBelow = new RadioButton();
      this.rbtnSelectedAssociates = new RadioButton();
      this.chbFilter = new CheckBox();
      this.label6 = new Label();
      this.listViewMembers = new ListView();
      this.columnHeaderName = new ColumnHeader();
      this.imgListTv = new ImageList(this.components);
      this.cxbAll = new CheckBox();
      this.cxbOrg = new CheckBox();
      this.cxbGroup = new CheckBox();
      this.gcRoleList = new GroupContainer();
      this.gradientPanel1 = new GradientPanel();
      this.gcLO = new GroupContainer();
      this.gradientPanel2 = new GradientPanel();
      this.gradientPanel3 = new GradientPanel();
      this.legend = new UserGroupLegend();
      this.stdIconBtnAdd = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.gcRoleList.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.gcLO.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.gradientPanel3.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnAdd).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.SuspendLayout();
      this.label1.Location = new Point(10, 74);
      this.label1.Name = "label1";
      this.label1.Size = new Size(104, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "The group can view";
      this.cmbBoxRole.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxRole.Location = new Point(120, 71);
      this.cmbBoxRole.Name = "cmbBoxRole";
      this.cmbBoxRole.Size = new Size(112, 21);
      this.cmbBoxRole.TabIndex = 1;
      this.cmbBoxRole.SelectedIndexChanged += new EventHandler(this.cmbBoxRole_SelectedIndexChanged);
      this.label2.Location = new Point(238, 74);
      this.label2.Name = "label2";
      this.label2.Size = new Size(40, 16);
      this.label2.TabIndex = 2;
      this.label2.Text = "list of";
      this.rbtnAllAssociates.Location = new Point(15, 99);
      this.rbtnAllAssociates.Name = "rbtnAllAssociates";
      this.rbtnAllAssociates.Size = new Size(320, 16);
      this.rbtnAllAssociates.TabIndex = 3;
      this.rbtnAllAssociates.Text = "All";
      this.rbtnAllAssociates.CheckedChanged += new EventHandler(this.rbtnAllAssociates_CheckedChanged);
      this.rbtnAssociatesBelow.Location = new Point(15, 121);
      this.rbtnAssociatesBelow.Name = "rbtnAssociatesBelow";
      this.rbtnAssociatesBelow.Size = new Size(320, 16);
      this.rbtnAssociatesBelow.TabIndex = 4;
      this.rbtnAssociatesBelow.Text = "below in organization hierarchy";
      this.rbtnAssociatesBelow.CheckedChanged += new EventHandler(this.rbtnAllAssociates_CheckedChanged);
      this.rbtnSelectedAssociates.Location = new Point(15, 143);
      this.rbtnSelectedAssociates.Name = "rbtnSelectedAssociates";
      this.rbtnSelectedAssociates.Size = new Size(219, 16);
      this.rbtnSelectedAssociates.TabIndex = 5;
      this.rbtnSelectedAssociates.Text = "Some";
      this.rbtnSelectedAssociates.CheckedChanged += new EventHandler(this.rbtnAllAssociates_CheckedChanged);
      this.chbFilter.BackColor = Color.Transparent;
      this.chbFilter.Location = new Point(11, 9);
      this.chbFilter.Name = "chbFilter";
      this.chbFilter.Size = new Size(247, 16);
      this.chbFilter.TabIndex = 9;
      this.chbFilter.Text = "Don't show disabled user accounts in the list";
      this.chbFilter.UseVisualStyleBackColor = false;
      this.chbFilter.CheckedChanged += new EventHandler(this.chbFilter_CheckedChanged);
      this.label6.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.label6.BackColor = Color.Transparent;
      this.label6.Location = new Point(7, 3);
      this.label6.Name = "label6";
      this.label6.Size = new Size(598, 28);
      this.label6.TabIndex = 11;
      this.label6.Text = "You can limit the group's access to specified users in the Role lists on milestone worksheets, Pipeline search, and Dashboard views etc.";
      this.listViewMembers.BorderStyle = BorderStyle.None;
      this.listViewMembers.Columns.AddRange(new ColumnHeader[1]
      {
        this.columnHeaderName
      });
      this.listViewMembers.Dock = DockStyle.Fill;
      this.listViewMembers.GridLines = true;
      this.listViewMembers.Location = new Point(1, 26);
      this.listViewMembers.Name = "listViewMembers";
      this.listViewMembers.Size = new Size(569, 235);
      this.listViewMembers.SmallImageList = this.imgListTv;
      this.listViewMembers.TabIndex = 13;
      this.listViewMembers.UseCompatibleStateImageBehavior = false;
      this.listViewMembers.View = View.Details;
      this.listViewMembers.SelectedIndexChanged += new EventHandler(this.listViewMembers_SelectedIndexChanged);
      this.columnHeaderName.Text = "Name";
      this.columnHeaderName.Width = 226;
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.White;
      this.imgListTv.Images.SetKeyName(0, "members-this-group-and-below.png");
      this.imgListTv.Images.SetKeyName(1, "members-this-group.png");
      this.imgListTv.Images.SetKeyName(2, "member-group.png");
      this.imgListTv.Images.SetKeyName(3, "");
      this.imgListTv.Images.SetKeyName(4, "");
      this.imgListTv.Images.SetKeyName(5, "");
      this.imgListTv.Images.SetKeyName(6, "");
      this.cxbAll.Location = new Point(15, 99);
      this.cxbAll.Name = "cxbAll";
      this.cxbAll.Size = new Size(16, 16);
      this.cxbAll.TabIndex = 14;
      this.cxbAll.Visible = false;
      this.cxbOrg.Location = new Point(15, 117);
      this.cxbOrg.Name = "cxbOrg";
      this.cxbOrg.Size = new Size(16, 24);
      this.cxbOrg.TabIndex = 15;
      this.cxbOrg.Visible = false;
      this.cxbGroup.Location = new Point(15, 139);
      this.cxbGroup.Name = "cxbGroup";
      this.cxbGroup.Size = new Size(16, 24);
      this.cxbGroup.TabIndex = 16;
      this.cxbGroup.Visible = false;
      this.gcRoleList.Controls.Add((Control) this.gradientPanel1);
      this.gcRoleList.Controls.Add((Control) this.gcLO);
      this.gcRoleList.Controls.Add((Control) this.cxbGroup);
      this.gcRoleList.Controls.Add((Control) this.label1);
      this.gcRoleList.Controls.Add((Control) this.cxbOrg);
      this.gcRoleList.Controls.Add((Control) this.cmbBoxRole);
      this.gcRoleList.Controls.Add((Control) this.cxbAll);
      this.gcRoleList.Controls.Add((Control) this.label2);
      this.gcRoleList.Controls.Add((Control) this.rbtnSelectedAssociates);
      this.gcRoleList.Controls.Add((Control) this.rbtnAssociatesBelow);
      this.gcRoleList.Controls.Add((Control) this.rbtnAllAssociates);
      this.gcRoleList.Dock = DockStyle.Fill;
      this.gcRoleList.Location = new Point(0, 0);
      this.gcRoleList.Name = "gcRoleList";
      this.gcRoleList.Size = new Size(610, 531);
      this.gcRoleList.TabIndex = 18;
      this.gcRoleList.Text = "Access to Role List";
      this.gradientPanel1.Borders = AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.label6);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(1, 26);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(608, 31);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel1.TabIndex = 19;
      this.gcLO.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcLO.Controls.Add((Control) this.listViewMembers);
      this.gcLO.Controls.Add((Control) this.gradientPanel2);
      this.gcLO.Controls.Add((Control) this.gradientPanel3);
      this.gcLO.Controls.Add((Control) this.stdIconBtnAdd);
      this.gcLO.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcLO.Location = new Point(35, 165);
      this.gcLO.Name = "gcLO";
      this.gcLO.Size = new Size(571, 362);
      this.gcLO.TabIndex = 18;
      this.gcLO.Text = "Select Loan Officers";
      this.gradientPanel2.Borders = AnchorStyles.Top;
      this.gradientPanel2.Controls.Add((Control) this.chbFilter);
      this.gradientPanel2.Dock = DockStyle.Bottom;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.Location = new Point(1, 261);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(569, 31);
      this.gradientPanel2.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel2.TabIndex = 22;
      this.gradientPanel3.Borders = AnchorStyles.Top;
      this.gradientPanel3.Controls.Add((Control) this.legend);
      this.gradientPanel3.Dock = DockStyle.Bottom;
      this.gradientPanel3.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel3.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel3.Location = new Point(1, 292);
      this.gradientPanel3.Name = "gradientPanel3";
      this.gradientPanel3.Size = new Size(569, 69);
      this.gradientPanel3.TabIndex = 23;
      this.legend.BackColor = Color.Transparent;
      this.legend.Location = new Point(3, 0);
      this.legend.Name = "legend";
      this.legend.Size = new Size(438, 72);
      this.legend.TabIndex = 20;
      this.stdIconBtnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnAdd.BackColor = Color.Transparent;
      this.stdIconBtnAdd.Location = new Point(529, 5);
      this.stdIconBtnAdd.Name = "stdIconBtnAdd";
      this.stdIconBtnAdd.Size = new Size(16, 16);
      this.stdIconBtnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnAdd.TabIndex = 19;
      this.stdIconBtnAdd.TabStop = false;
      this.stdIconBtnAdd.Click += new EventHandler(this.btnSelect_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(551, 5);
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 18;
      this.stdIconBtnDelete.TabStop = false;
      this.stdIconBtnDelete.Click += new EventHandler(this.stdIconBtnDelete_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(610, 531);
      this.Controls.Add((Control) this.gcRoleList);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (ListViewPage);
      this.gcRoleList.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gcLO.ResumeLayout(false);
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel3.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnAdd).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.ResumeLayout(false);
    }

    private void setDirtyFlag(bool val)
    {
      this.dirty = val;
      if (this.DirtyFlagChanged == null)
        return;
      this.DirtyFlagChanged((object) this, (EventArgs) null);
    }

    private int currentGroupId
    {
      get => this._currentGroupId;
      set
      {
        this._currentGroupId = value;
        this.setDirtyFlag(false);
        this.tmpRoleDataHash = new Hashtable();
        this.tmpRoleResetTreeHash = new Hashtable();
        this.tmpRoleHideDisabledHash = new Hashtable();
        if (this.cmbBoxRole.SelectedItem == null)
          this.cmbBoxRole.SelectedIndex = 0;
        else
          this.cmbBoxRole_SelectedIndexChanged((object) null, (EventArgs) null);
      }
    }

    public virtual void SetGroup(int groupId) => this.currentGroupId = groupId;

    private void loadRoles()
    {
      this.cmbBoxRole.Items.Clear();
      RoleInfo[] allRoleFunctions = ((WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
      this.cmbBoxRole.Items.AddRange((object[]) allRoleFunctions);
      if (allRoleFunctions.Length == 0)
        return;
      this._currentRoleId = ((RoleSummaryInfo) this.cmbBoxRole.Items[0]).RoleID;
    }

    private int getCurrentRoleId() => ((RoleSummaryInfo) this.cmbBoxRole.SelectedItem).RoleID;

    private void rbtnAllAssociates_CheckedChanged(object sender, EventArgs e)
    {
      this.listViewMembers_SelectedIndexChanged((object) this, (EventArgs) null);
      if (!this.personal)
      {
        if (this.loadingSettingsForRole)
          return;
        int currentRoleId = this.getCurrentRoleId();
        if (this.rbtnAllAssociates.Checked)
        {
          this.listViewMembers.Items.Clear();
          this.stdIconBtnAdd.Enabled = false;
        }
        else if (this.rbtnAssociatesBelow.Checked)
        {
          this.listViewMembers.Items.Clear();
          this.stdIconBtnAdd.Enabled = false;
        }
        else if (this.rbtnSelectedAssociates.Checked)
        {
          this.stdIconBtnAdd.Enabled = true;
          if (!this.firstTime)
            this.loadSettingsForRole((TreeView) this.orgView.GetCurrentTreeView());
          else if (this.session.AclGroupManager.GetAclGroupRoleAccessLevel(this.currentGroupId, currentRoleId).Access == AclGroupRoleAccessEnum.Custom)
          {
            this.btnReset_Click((object) null, (EventArgs) null);
          }
          else
          {
            this.members = this.session.AclGroupManager.GetMembersInGroupRole(this.currentGroupId, currentRoleId);
            this.orgView = (ResourceSetViewer) null;
          }
        }
        this.UpdateTempHash();
      }
      this.setDirtyFlag(true);
    }

    private void cmbBoxRole_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.firstTime = true;
      int currentRoleId = this.getCurrentRoleId();
      this._currentRoleId = currentRoleId;
      OrgInGroupRole[] orgInGroupRoleArray = new OrgInGroupRole[0];
      UserInGroupRole[] userInGroupRoleArray = new UserInGroupRole[0];
      if (!this.tmpRoleDataHash.ContainsKey((object) currentRoleId))
      {
        this.members = this.session.AclGroupManager.GetMembersInGroupRole(this.currentGroupId, currentRoleId);
      }
      else
      {
        ArrayList arrayList1 = new ArrayList();
        if (!(this.tmpRoleDataHash[(object) currentRoleId] is AclGroupRoleAccessEnum))
        {
          ArrayList arrayList2 = (ArrayList) this.tmpRoleDataHash[(object) currentRoleId];
          ArrayList arrayList3 = new ArrayList();
          ArrayList arrayList4 = new ArrayList();
          foreach (object obj in arrayList2)
          {
            if ((object) (obj as OrgInGroupRole) != null)
              arrayList3.Add(obj);
            else if ((object) (obj as UserInGroupRole) != null)
              arrayList4.Add(obj);
          }
          orgInGroupRoleArray = (OrgInGroupRole[]) arrayList3.ToArray(typeof (OrgInGroupRole));
          userInGroupRoleArray = (UserInGroupRole[]) arrayList4.ToArray(typeof (UserInGroupRole));
        }
        this.members = new AclGroupRoleMembers(this.currentGroupId, currentRoleId);
        this.members.OrgMembers = orgInGroupRoleArray;
        this.members.UserMembers = userInGroupRoleArray;
      }
      this.orgView = (ResourceSetViewer) null;
      if (!this.personal)
        this.loadSettingsForRole();
      else
        this.loadSettingsForUserRole();
      this.resetWording();
    }

    private void resetWording()
    {
      string roleName = ((RoleSummaryInfo) this.cmbBoxRole.SelectedItem).RoleName;
      this.rbtnAllAssociates.Text = "All " + roleName + "s";
      this.rbtnAssociatesBelow.Text = roleName + "s below in organization hierarchy";
      this.rbtnSelectedAssociates.Text = "Some " + roleName + "s";
      this.gcLO.Text = "Select " + roleName + "s";
    }

    private void loadSettingsForRole()
    {
      this.loadingSettingsForRole = true;
      this.listViewMembers.Items.Clear();
      int currentRoleId = this.getCurrentRoleId();
      AclGroupRoleAccessLevel groupRoleAccessLevel;
      if (!this.tmpRoleHideDisabledHash.ContainsKey((object) currentRoleId))
      {
        groupRoleAccessLevel = this.session.AclGroupManager.GetAclGroupRoleAccessLevel(this.currentGroupId, currentRoleId);
      }
      else
      {
        bool hideDisabledAccount = (bool) this.tmpRoleHideDisabledHash[(object) currentRoleId];
        AclGroupRoleAccessEnum access = AclGroupRoleAccessEnum.Custom;
        if (this.tmpRoleDataHash[(object) currentRoleId] is AclGroupRoleAccessEnum)
          access = (AclGroupRoleAccessEnum) this.tmpRoleDataHash[(object) currentRoleId];
        groupRoleAccessLevel = new AclGroupRoleAccessLevel(this.currentGroupId, currentRoleId, access, hideDisabledAccount);
      }
      this.chbFilter.Checked = groupRoleAccessLevel.HideDisabledAccount;
      if (groupRoleAccessLevel.Access == AclGroupRoleAccessEnum.All)
      {
        this.rbtnAllAssociates.Checked = true;
        this.stdIconBtnAdd.Enabled = false;
      }
      else if (groupRoleAccessLevel.Access == AclGroupRoleAccessEnum.BelowInOrg)
      {
        this.rbtnAssociatesBelow.Checked = true;
        this.stdIconBtnAdd.Enabled = false;
      }
      else
      {
        this.rbtnSelectedAssociates.Checked = true;
        this.stdIconBtnAdd.Enabled = true;
        OrgInGroupRole[] orgMembers = this.members.OrgMembers;
        UserInGroupRole[] userMembers = this.members.UserMembers;
        ArrayList arrayList1 = new ArrayList();
        for (int index = 0; index < orgMembers.Length; ++index)
        {
          OrgInGroupRole orgInGroupRole = orgMembers[index];
          ListViewItem listViewItem = new ListViewItem(new string[1]
          {
            orgInGroupRole.OrgName
          });
          int num = orgInGroupRole.IsInclusive ? 0 : 1;
          listViewItem.ImageIndex = num;
          listViewItem.ImageIndex = num;
          listViewItem.Tag = (object) new TreeNode(orgInGroupRole.OrgName)
          {
            Tag = (object) orgInGroupRole,
            ImageIndex = num,
            SelectedImageIndex = num
          };
          arrayList1.Add((object) listViewItem);
        }
        ArrayList arrayList2 = new ArrayList();
        if (userMembers != null && userMembers.Length != 0)
        {
          List<string> stringList = new List<string>();
          foreach (UserInGroupRole userInGroupRole in userMembers)
          {
            if (!stringList.Contains(userInGroupRole.UserID))
              stringList.Add(userInGroupRole.UserID);
          }
          Hashtable users = this.session.OrganizationManager.GetUsers(stringList.ToArray());
          for (int index = 0; index < userMembers.Length; ++index)
          {
            UserInfo userInfo = (UserInfo) users[(object) userMembers[index].UserID];
            arrayList2.Add((object) new ListViewItem(new string[1]
            {
              userInfo.FullName + " (" + userInfo.Userid + ")"
            })
            {
              ImageIndex = 2,
              Tag = (object) new TreeNode(userInfo.FullName + " (" + userInfo.Userid + ")")
              {
                Tag = (object) userMembers[index],
                ImageIndex = 2,
                SelectedImageIndex = 2
              }
            });
          }
        }
        this.listViewMembers.Items.AddRange((ListViewItem[]) arrayList1.ToArray(typeof (ListViewItem)));
        this.listViewMembers.Items.AddRange((ListViewItem[]) arrayList2.ToArray(typeof (ListViewItem)));
      }
      this.loadingSettingsForRole = false;
    }

    private void addNodeToList(TreeNode node, ArrayList orgList, ArrayList userList)
    {
      object tag = node.Tag;
      bool flag = false;
      switch (node.ImageIndex)
      {
        case 0:
        case 1:
        case 2:
          flag = true;
          break;
      }
      if (node.Text == "<DUMMY NODE>")
        flag = false;
      if (flag)
      {
        ListViewItem listViewItem = (ListViewItem) null;
        TreeNode treeNode = new TreeNode(node.Text, node.ImageIndex, node.SelectedImageIndex);
        treeNode.Tag = node.Tag;
        switch (node.ImageIndex)
        {
          case 0:
            listViewItem = new ListViewItem(new string[2]
            {
              node.Text,
              "View Only"
            })
            {
              ImageIndex = treeNode.SelectedImageIndex
            };
            listViewItem.ImageIndex = treeNode.ImageIndex;
            listViewItem.Tag = (object) treeNode;
            break;
          case 1:
            listViewItem = new ListViewItem(new string[2]
            {
              node.Text,
              "View Only"
            })
            {
              ImageIndex = treeNode.SelectedImageIndex
            };
            listViewItem.ImageIndex = treeNode.ImageIndex;
            listViewItem.Tag = (object) treeNode;
            break;
          case 2:
            listViewItem = new ListViewItem(new string[2]
            {
              node.Text,
              "View Only"
            })
            {
              ImageIndex = treeNode.SelectedImageIndex
            };
            listViewItem.ImageIndex = treeNode.ImageIndex;
            listViewItem.Tag = (object) treeNode;
            break;
        }
        if (tag is OrgInfo)
          orgList.Add((object) listViewItem);
        else
          userList.Add((object) listViewItem);
      }
      foreach (TreeNode node1 in node.Nodes)
        this.addNodeToList(node1, orgList, userList);
    }

    private void loadSettingsForRole(TreeView tv)
    {
      this.loadingSettingsForRole = true;
      this.listViewMembers.Items.Clear();
      ArrayList orgList = new ArrayList();
      ArrayList userList = new ArrayList();
      foreach (TreeNode node in tv.Nodes)
        this.addNodeToList(node, orgList, userList);
      this.listViewMembers.Items.AddRange((ListViewItem[]) orgList.ToArray(typeof (ListViewItem)));
      this.listViewMembers.Items.AddRange((ListViewItem[]) userList.ToArray(typeof (ListViewItem)));
      this.loadingSettingsForRole = false;
    }

    private void loadSettingsForUserRole()
    {
      this.loadingSettingsForRole = true;
      this.listViewMembers.Items.Clear();
      int currentRoleId = this.getCurrentRoleId();
      this.cxbAll.Checked = false;
      this.cxbGroup.Checked = false;
      this.cxbOrg.Checked = false;
      this.chbFilter.Checked = false;
      Hashtable hashtable1 = new Hashtable();
      Hashtable hashtable2 = new Hashtable();
      foreach (AclGroup group in this.groupList)
      {
        AclGroupRoleAccessLevel groupRoleAccessLevel = this.session.AclGroupManager.GetAclGroupRoleAccessLevel(group.ID, currentRoleId);
        if (groupRoleAccessLevel.Access == AclGroupRoleAccessEnum.All)
        {
          this.cxbAll.Checked = true;
          this.cxbGroup.Checked = false;
          this.cxbOrg.Checked = false;
          hashtable1.Clear();
          hashtable2.Clear();
          this.chbFilter.Checked = groupRoleAccessLevel.HideDisabledAccount;
          break;
        }
        if (groupRoleAccessLevel.Access == AclGroupRoleAccessEnum.BelowInOrg)
        {
          this.cxbOrg.Checked = true;
          if (!this.chbFilter.Checked)
            this.chbFilter.Checked = groupRoleAccessLevel.HideDisabledAccount;
        }
        else
        {
          if (!this.chbFilter.Checked && groupRoleAccessLevel.HideDisabledAccount)
            this.chbFilter.Checked = true;
          this.cxbGroup.Checked = true;
          AclGroupRoleMembers membersInGroupRole = this.session.AclGroupManager.GetMembersInGroupRole(group.ID, currentRoleId);
          OrgInGroupRole[] orgMembers = membersInGroupRole.OrgMembers;
          UserInGroupRole[] userMembers = membersInGroupRole.UserMembers;
          for (int index = 0; index < orgMembers.Length; ++index)
          {
            OrgInGroupRole orgInGroupRole = orgMembers[index];
            string key = orgInGroupRole.OrgName + "_" + orgInGroupRole.IsInclusive.ToString();
            if (hashtable1[(object) key] == null)
            {
              ListViewItem listViewItem = new ListViewItem(new string[1]
              {
                orgInGroupRole.OrgName
              });
              int num = orgInGroupRole.IsInclusive ? 0 : 1;
              listViewItem.ImageIndex = num;
              listViewItem.ImageIndex = num;
              listViewItem.Tag = (object) new TreeNode(orgInGroupRole.OrgName)
              {
                Tag = (object) orgInGroupRole,
                ImageIndex = num,
                SelectedImageIndex = num
              };
              hashtable1.Add((object) key, (object) listViewItem);
            }
          }
          if (userMembers != null && userMembers.Length != 0)
          {
            List<string> stringList = new List<string>();
            foreach (UserInGroupRole userInGroupRole in userMembers)
            {
              if (!stringList.Contains(userInGroupRole.UserID))
                stringList.Add(userInGroupRole.UserID);
            }
            Hashtable users = this.session.OrganizationManager.GetUsers(stringList.ToArray());
            for (int index = 0; index < userMembers.Length; ++index)
            {
              UserInfo userInfo = (UserInfo) users[(object) userMembers[index].UserID];
              if (hashtable2[(object) userInfo.FullName] == null)
                hashtable2.Add((object) userInfo.FullName, (object) new ListViewItem(new string[1]
                {
                  userInfo.FullName + " (" + userInfo.Userid + ")"
                })
                {
                  ImageIndex = 2,
                  Tag = (object) new TreeNode(userInfo.FullName + " (" + userInfo.Userid + ")")
                  {
                    Tag = (object) userMembers[index],
                    ImageIndex = 2,
                    SelectedImageIndex = 2
                  }
                });
            }
          }
        }
      }
      if (hashtable1.Count > 0)
      {
        ArrayList arrayList = new ArrayList();
        IDictionaryEnumerator enumerator = hashtable1.GetEnumerator();
        while (enumerator.MoveNext())
          arrayList.Add((object) (ListViewItem) enumerator.Value);
        this.listViewMembers.Items.AddRange((ListViewItem[]) arrayList.ToArray(typeof (ListViewItem)));
      }
      if (hashtable2.Count > 0)
      {
        ArrayList arrayList = new ArrayList();
        IDictionaryEnumerator enumerator = hashtable2.GetEnumerator();
        while (enumerator.MoveNext())
          arrayList.Add((object) (ListViewItem) enumerator.Value);
        this.listViewMembers.Items.AddRange((ListViewItem[]) arrayList.ToArray(typeof (ListViewItem)));
      }
      this.loadingSettingsForRole = false;
    }

    private TreeNode[] GetResetTreeView()
    {
      ArrayList arrayList = new ArrayList();
      foreach (ListViewItem listViewItem in this.listViewMembers.Items)
        arrayList.Add((object) (TreeNode) listViewItem.Tag);
      return (TreeNode[]) arrayList.ToArray(typeof (TreeNode));
    }

    private ArrayList GetTmpTreeView()
    {
      ArrayList tmpTreeView = new ArrayList();
      foreach (ListViewItem listViewItem in this.listViewMembers.Items)
      {
        TreeNode tag = (TreeNode) listViewItem.Tag;
        if (tag.Tag is OrgInfo)
        {
          if (listViewItem.ImageIndex == 0)
            tmpTreeView.Add((object) new OrgInGroupRole(this._currentRoleId, ((OrgInfo) tag.Tag).Oid, true, ((OrgInfo) tag.Tag).OrgName));
          else
            tmpTreeView.Add((object) new OrgInGroupRole(this._currentRoleId, ((OrgInfo) tag.Tag).Oid, false, ((OrgInfo) tag.Tag).OrgName));
        }
        else if ((object) (tag.Tag as OrgInGroupRole) != null)
          tmpTreeView.Add((object) (OrgInGroupRole) tag.Tag);
        else if ((object) (tag.Tag as UserInfo) != null)
          tmpTreeView.Add((object) new UserInGroupRole(((UserInfo) tag.Tag).Userid, this._currentRoleId));
        else if ((object) (tag.Tag as UserInGroupRole) != null)
          tmpTreeView.Add((object) (UserInGroupRole) tag.Tag);
      }
      return tmpTreeView;
    }

    private void makeReadOnly()
    {
      this.stdIconBtnAdd.Enabled = false;
      this.stdIconBtnDelete.Enabled = false;
      this.chbFilter.Enabled = false;
      this.cxbAll.Visible = true;
      this.cxbAll.BringToFront();
      this.cxbAll.Enabled = false;
      this.cxbGroup.Visible = true;
      this.cxbGroup.BringToFront();
      this.cxbGroup.Enabled = false;
      this.cxbOrg.Visible = true;
      this.cxbOrg.BringToFront();
      this.cxbOrg.Enabled = false;
    }

    public void SaveData()
    {
      OrgInGroupRole[] orgs = new OrgInGroupRole[0];
      UserInGroupRole[] users = new UserInGroupRole[0];
      foreach (int key in (IEnumerable) this.tmpRoleDataHash.Keys)
      {
        bool hideDisabledAccount = (bool) this.tmpRoleHideDisabledHash[(object) key];
        this.session.AclGroupManager.UpdateMembersInGroupRole(this.currentGroupId, key, users, orgs);
        AclGroupRoleAccessEnum access;
        if (this.tmpRoleDataHash[(object) key] is AclGroupRoleAccessEnum)
        {
          access = (AclGroupRoleAccessEnum) this.tmpRoleDataHash[(object) key];
        }
        else
        {
          access = AclGroupRoleAccessEnum.Custom;
          ArrayList arrayList = (ArrayList) this.tmpRoleDataHash[(object) key];
          List<OrgInGroupRole> orgInGroupRoleList = new List<OrgInGroupRole>();
          List<UserInGroupRole> userInGroupRoleList = new List<UserInGroupRole>();
          foreach (object obj in arrayList)
          {
            if ((object) (obj as OrgInGroupRole) != null)
              orgInGroupRoleList.Add((OrgInGroupRole) obj);
            else if ((object) (obj as UserInGroupRole) != null)
              userInGroupRoleList.Add((UserInGroupRole) obj);
          }
          this.session.AclGroupManager.UpdateMembersInGroupRole(this.currentGroupId, key, userInGroupRoleList.ToArray(), orgInGroupRoleList.ToArray());
        }
        this.session.AclGroupManager.UpdateAclGroupRoleAccessLevel(new AclGroupRoleAccessLevel(this.currentGroupId, key, access, hideDisabledAccount));
      }
      this.tmpRoleDataHash = new Hashtable();
      this.tmpRoleHideDisabledHash = new Hashtable();
      this.tmpRoleResetTreeHash = new Hashtable();
      this.setDirtyFlag(false);
    }

    public void ResetData()
    {
      this.firstTime = true;
      this.setDirtyFlag(false);
      this.tmpRoleDataHash = new Hashtable();
      this.tmpRoleResetTreeHash = new Hashtable();
      this.tmpRoleHideDisabledHash = new Hashtable();
      this.cmbBoxRole_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    public bool HasBeenModified() => this.dirty;

    private void btnReset_Click(object sender, EventArgs e)
    {
      this.firstTime = true;
      this.setDirtyFlag(false);
      this.tmpRoleDataHash = new Hashtable();
      this.tmpRoleResetTreeHash = new Hashtable();
      this.tmpRoleHideDisabledHash = new Hashtable();
      this.cmbBoxRole_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void chbFilter_CheckedChanged(object sender, EventArgs e)
    {
      if (this.loadingSettingsForRole)
        return;
      this.setDirtyFlag(true);
      this.UpdateTempHash();
    }

    private void UpdateTempHash()
    {
      int currentRoleId = this._currentRoleId;
      if (this.rbtnAllAssociates.Checked)
      {
        if (!this.tmpRoleDataHash.ContainsKey((object) currentRoleId))
          this.tmpRoleDataHash.Add((object) currentRoleId, (object) "");
        this.tmpRoleDataHash[(object) currentRoleId] = (object) AclGroupRoleAccessEnum.All;
        if (!this.tmpRoleHideDisabledHash.ContainsKey((object) currentRoleId))
          this.tmpRoleHideDisabledHash.Add((object) currentRoleId, (object) false);
        this.tmpRoleHideDisabledHash[(object) currentRoleId] = (object) this.chbFilter.Checked;
      }
      else if (this.rbtnAssociatesBelow.Checked)
      {
        if (!this.tmpRoleDataHash.ContainsKey((object) currentRoleId))
          this.tmpRoleDataHash.Add((object) currentRoleId, (object) "");
        this.tmpRoleDataHash[(object) currentRoleId] = (object) AclGroupRoleAccessEnum.BelowInOrg;
        if (!this.tmpRoleHideDisabledHash.ContainsKey((object) currentRoleId))
          this.tmpRoleHideDisabledHash.Add((object) currentRoleId, (object) false);
        this.tmpRoleHideDisabledHash[(object) currentRoleId] = (object) this.chbFilter.Checked;
      }
      else
      {
        if (!this.rbtnSelectedAssociates.Checked)
          return;
        if (!this.tmpRoleDataHash.ContainsKey((object) currentRoleId))
          this.tmpRoleDataHash.Add((object) currentRoleId, (object) "");
        this.tmpRoleDataHash[(object) currentRoleId] = (object) this.GetTmpTreeView();
        if (!this.tmpRoleHideDisabledHash.ContainsKey((object) currentRoleId))
          this.tmpRoleHideDisabledHash.Add((object) currentRoleId, (object) false);
        this.tmpRoleHideDisabledHash[(object) currentRoleId] = (object) this.chbFilter.Checked;
        if (!this.tmpRoleResetTreeHash.ContainsKey((object) currentRoleId))
          this.tmpRoleResetTreeHash.Add((object) currentRoleId, (object) "");
        this.tmpRoleResetTreeHash[(object) currentRoleId] = (object) this.GetResetTreeView();
      }
    }

    private void listViewMembers_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.personal)
        this.stdIconBtnDelete.Enabled = false;
      else
        this.stdIconBtnDelete.Enabled = this.listViewMembers.SelectedItems.Count > 0;
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      if (this.orgView == null)
      {
        this.orgView = new ResourceSetViewer(this.session, (object) this.members, this.DirtyFlagChanged, this._currentGroupId);
        this.orgView.SetResetDataSourceTreeNode = this.GetResetTreeView();
      }
      this.Cursor = Cursors.Default;
      if (DialogResult.Cancel != this.orgView.ShowDialog((IWin32Window) this))
      {
        this.loadSettingsForRole((TreeView) this.orgView.GetCurrentTreeView());
        this.orgView.SetResetDataSourceTreeNode = this.GetResetTreeView();
        this.setDirtyFlag(true);
      }
      this.UpdateTempHash();
    }

    private void stdIconBtnDelete_Click(object sender, EventArgs e)
    {
      if (this.listViewMembers.SelectedItems == null || this.listViewMembers.SelectedItems.Count == 0)
        return;
      this.Cursor = Cursors.WaitCursor;
      if (this.orgView == null)
        this.orgView = new ResourceSetViewer(this.session, (object) this.members, this.DirtyFlagChanged, this._currentGroupId);
      List<TreeNode> treeNodeList = new List<TreeNode>();
      foreach (ListViewItem selectedItem in this.listViewMembers.SelectedItems)
      {
        treeNodeList.Add((TreeNode) selectedItem.Tag);
        selectedItem.Remove();
      }
      this.orgView.SetResetDataSourceTreeNode = this.GetResetTreeView();
      this.orgView.ResetTreeWithMemoryData();
      foreach (TreeNode node in treeNodeList)
        this.orgView.RemoveTreeNode(node);
      this.UpdateTempHash();
      this.setDirtyFlag(true);
      this.Cursor = Cursors.Default;
    }
  }
}
