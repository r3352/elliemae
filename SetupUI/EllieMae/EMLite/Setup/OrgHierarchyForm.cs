// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.OrgHierarchyForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
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
  public class OrgHierarchyForm : Form
  {
    private Sessions.Session session;
    private TreeNode startNode;
    private HierarchyTree hierarchyTree;
    private ToolTip hierarchyToolTip;
    private GridView gvUsers;
    private ImageList imgListTv;
    private ContextMenu contextMenuOrg;
    private ContextMenu contextMenuUser;
    private MenuItem addOrgMenuItem;
    private MenuItem delOrgMenuItem;
    private MenuItem addUserMenuItem;
    private MenuItem delUserMenuItem;
    private IContainer components;
    private MenuItem editOrgMenuItem;
    private MenuItem editUserMenuItem;
    private string oldNode = "";
    private IOrganizationManager rOrg;
    private string userid;
    private Persona[] personaList;
    private GroupContainer gcOrg;
    private StandardIconButton stdIconBtnDeleteOrg;
    private StandardIconButton stdIconBtnEditOrg;
    private StandardIconButton stdIconBtnNewOrg;
    private Splitter splitter1;
    private GroupContainer gcUsers;
    private StandardIconButton stdIconBtnDeleteUser;
    private StandardIconButton stdIconBtnEditUser;
    private StandardIconButton stdIconBtnNewUser;
    private ToolTip toolTip1;
    private bool readOnly;

    public OrgHierarchyForm(Sessions.Session session)
      : this(session, (string) null)
    {
    }

    public OrgHierarchyForm(Sessions.Session session, string userid)
    {
      this.session = session;
      this.InitializeComponent();
      this.hierarchyTree.SetSession(this.session);
      this.rOrg = this.session.OrganizationManager;
      this.personaList = this.session.PersonaManager.GetAllPersonas();
      this.locateUserInOrganization(userid, false, "User '" + this.userid + "' doesn't exist. The user may have been deleted.");
      this.hierarchyToolTip = new ToolTip(this.components);
      this.hierarchyToolTip.AutoPopDelay = 3000;
      this.hierarchyToolTip.InitialDelay = 500;
      this.hierarchyToolTip.ReshowDelay = 100;
      this.hierarchyToolTip.ShowAlways = true;
    }

    private void locateUserInOrganization(string userid, bool returnIfNotFound, string errMsg)
    {
      this.userid = userid;
      int orgId;
      if (this.userid == null)
      {
        orgId = this.session.UserInfo.OrgId;
      }
      else
      {
        UserInfo user = this.rOrg.GetUser(this.userid);
        if (user == (UserInfo) null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, errMsg ?? "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
          if (returnIfNotFound)
            return;
          orgId = 0;
        }
        else
          orgId = user.OrgId;
      }
      this.hierarchyTree.ImageList = this.imgListTv;
      this.hierarchyTree.ParentOrgId = this.session.UserInfo.OrgId;
      this.hierarchyTree.RootNodes(orgId);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (OrgHierarchyForm));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      this.contextMenuOrg = new ContextMenu();
      this.addOrgMenuItem = new MenuItem();
      this.editOrgMenuItem = new MenuItem();
      this.delOrgMenuItem = new MenuItem();
      this.contextMenuUser = new ContextMenu();
      this.addUserMenuItem = new MenuItem();
      this.editUserMenuItem = new MenuItem();
      this.delUserMenuItem = new MenuItem();
      this.imgListTv = new ImageList(this.components);
      this.splitter1 = new Splitter();
      this.toolTip1 = new ToolTip(this.components);
      this.stdIconBtnDeleteUser = new StandardIconButton();
      this.stdIconBtnEditUser = new StandardIconButton();
      this.stdIconBtnNewUser = new StandardIconButton();
      this.stdIconBtnDeleteOrg = new StandardIconButton();
      this.stdIconBtnEditOrg = new StandardIconButton();
      this.stdIconBtnNewOrg = new StandardIconButton();
      this.gcUsers = new GroupContainer();
      this.gvUsers = new GridView();
      this.gcOrg = new GroupContainer();
      this.hierarchyTree = new HierarchyTree();
      ((ISupportInitialize) this.stdIconBtnDeleteUser).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEditUser).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNewUser).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDeleteOrg).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEditOrg).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNewOrg).BeginInit();
      this.gcUsers.SuspendLayout();
      this.gcOrg.SuspendLayout();
      this.SuspendLayout();
      this.contextMenuOrg.MenuItems.AddRange(new MenuItem[3]
      {
        this.addOrgMenuItem,
        this.editOrgMenuItem,
        this.delOrgMenuItem
      });
      this.addOrgMenuItem.Index = 0;
      this.addOrgMenuItem.Text = "Add Organization";
      this.addOrgMenuItem.Click += new EventHandler(this.addOrgMenuItem_Click);
      this.editOrgMenuItem.Index = 1;
      this.editOrgMenuItem.Text = "Edit Organization";
      this.editOrgMenuItem.Click += new EventHandler(this.editOrgMenuItem_Click);
      this.delOrgMenuItem.Index = 2;
      this.delOrgMenuItem.Text = "Delete Organization";
      this.delOrgMenuItem.Click += new EventHandler(this.delOrgMenuItem_Click);
      this.contextMenuUser.MenuItems.AddRange(new MenuItem[3]
      {
        this.addUserMenuItem,
        this.editUserMenuItem,
        this.delUserMenuItem
      });
      this.addUserMenuItem.Index = 0;
      this.addUserMenuItem.Text = "Add User";
      this.addUserMenuItem.Click += new EventHandler(this.addUserMenuItem_Click);
      this.editUserMenuItem.Index = 1;
      this.editUserMenuItem.Text = "Edit User";
      this.editUserMenuItem.Click += new EventHandler(this.editUserMenuItem_Click);
      this.delUserMenuItem.Index = 2;
      this.delUserMenuItem.Text = "Delete User";
      this.delUserMenuItem.Click += new EventHandler(this.delUserMenuItem_Click);
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.Transparent;
      this.imgListTv.Images.SetKeyName(0, "folder.bmp");
      this.imgListTv.Images.SetKeyName(1, "folder-open.bmp");
      this.splitter1.Dock = DockStyle.Top;
      this.splitter1.Location = new Point(0, 211);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(965, 5);
      this.splitter1.TabIndex = 6;
      this.splitter1.TabStop = false;
      this.stdIconBtnDeleteUser.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDeleteUser.BackColor = Color.Transparent;
      this.stdIconBtnDeleteUser.Location = new Point(944, 6);
      this.stdIconBtnDeleteUser.MouseDownImage = (Image) null;
      this.stdIconBtnDeleteUser.Name = "stdIconBtnDeleteUser";
      this.stdIconBtnDeleteUser.Size = new Size(16, 17);
      this.stdIconBtnDeleteUser.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDeleteUser.TabIndex = 6;
      this.stdIconBtnDeleteUser.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDeleteUser, "Delete User");
      this.stdIconBtnDeleteUser.Click += new EventHandler(this.stdIconBtnDeleteUser_Click);
      this.stdIconBtnEditUser.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEditUser.BackColor = Color.Transparent;
      this.stdIconBtnEditUser.Location = new Point(923, 6);
      this.stdIconBtnEditUser.MouseDownImage = (Image) null;
      this.stdIconBtnEditUser.Name = "stdIconBtnEditUser";
      this.stdIconBtnEditUser.Size = new Size(16, 17);
      this.stdIconBtnEditUser.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEditUser.TabIndex = 5;
      this.stdIconBtnEditUser.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEditUser, "Edit User");
      this.stdIconBtnEditUser.Click += new EventHandler(this.stdIconBtnEditUser_Click);
      this.stdIconBtnNewUser.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNewUser.BackColor = Color.Transparent;
      this.stdIconBtnNewUser.Location = new Point(902, 6);
      this.stdIconBtnNewUser.MouseDownImage = (Image) null;
      this.stdIconBtnNewUser.Name = "stdIconBtnNewUser";
      this.stdIconBtnNewUser.Size = new Size(16, 17);
      this.stdIconBtnNewUser.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNewUser.TabIndex = 4;
      this.stdIconBtnNewUser.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNewUser, "Add User");
      this.stdIconBtnNewUser.Click += new EventHandler(this.stdIconBtnNewUser_Click);
      this.stdIconBtnDeleteOrg.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDeleteOrg.BackColor = Color.Transparent;
      this.stdIconBtnDeleteOrg.Location = new Point(944, 5);
      this.stdIconBtnDeleteOrg.MouseDownImage = (Image) null;
      this.stdIconBtnDeleteOrg.Name = "stdIconBtnDeleteOrg";
      this.stdIconBtnDeleteOrg.Size = new Size(16, 17);
      this.stdIconBtnDeleteOrg.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDeleteOrg.TabIndex = 3;
      this.stdIconBtnDeleteOrg.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDeleteOrg, "Delete Organization");
      this.stdIconBtnDeleteOrg.Click += new EventHandler(this.stdIconBtnDeleteOrg_Click);
      this.stdIconBtnEditOrg.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEditOrg.BackColor = Color.Transparent;
      this.stdIconBtnEditOrg.Location = new Point(923, 5);
      this.stdIconBtnEditOrg.MouseDownImage = (Image) null;
      this.stdIconBtnEditOrg.Name = "stdIconBtnEditOrg";
      this.stdIconBtnEditOrg.Size = new Size(16, 17);
      this.stdIconBtnEditOrg.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEditOrg.TabIndex = 2;
      this.stdIconBtnEditOrg.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEditOrg, "Edit Organization");
      this.stdIconBtnEditOrg.Click += new EventHandler(this.stdIconBtnEditOrg_Click);
      this.stdIconBtnNewOrg.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNewOrg.BackColor = Color.Transparent;
      this.stdIconBtnNewOrg.Location = new Point(902, 5);
      this.stdIconBtnNewOrg.MouseDownImage = (Image) null;
      this.stdIconBtnNewOrg.Name = "stdIconBtnNewOrg";
      this.stdIconBtnNewOrg.Size = new Size(16, 17);
      this.stdIconBtnNewOrg.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNewOrg.TabIndex = 1;
      this.stdIconBtnNewOrg.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNewOrg, "Add Organization");
      this.stdIconBtnNewOrg.Click += new EventHandler(this.stdIconBtnNewOrg_Click);
      this.gcUsers.Controls.Add((Control) this.stdIconBtnDeleteUser);
      this.gcUsers.Controls.Add((Control) this.stdIconBtnEditUser);
      this.gcUsers.Controls.Add((Control) this.stdIconBtnNewUser);
      this.gcUsers.Controls.Add((Control) this.gvUsers);
      this.gcUsers.Dock = DockStyle.Fill;
      this.gcUsers.HeaderForeColor = SystemColors.ControlText;
      this.gcUsers.Location = new Point(0, 216);
      this.gcUsers.Name = "gcUsers";
      this.gcUsers.Size = new Size(965, 348);
      this.gcUsers.TabIndex = 7;
      this.gcUsers.Text = "Current Users ()        User Licenses ()";
      this.gvUsers.AllowDrop = true;
      this.gvUsers.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "User ID";
      gvColumn1.Width = 72;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Last Name";
      gvColumn2.Width = 88;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "First Name";
      gvColumn3.Width = 89;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Persona";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Email";
      gvColumn5.Width = 98;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "Phone";
      gvColumn6.Width = 93;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.Text = "Login";
      gvColumn7.Width = 100;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column8";
      gvColumn8.Text = "Account";
      gvColumn8.Width = 70;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column9";
      gvColumn9.Text = "Login Access";
      gvColumn9.Width = 100;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column10";
      gvColumn10.Text = "PW Required";
      gvColumn10.Width = 80;
      this.gvUsers.Columns.AddRange(new GVColumn[10]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9,
        gvColumn10
      });
      this.gvUsers.ContextMenu = this.contextMenuUser;
      this.gvUsers.Dock = DockStyle.Fill;
      this.gvUsers.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvUsers.Location = new Point(1, 26);
      this.gvUsers.Name = "gvUsers";
      this.gvUsers.Size = new Size(963, 321);
      this.gvUsers.TabIndex = 2;
      this.gvUsers.SelectedIndexChanged += new EventHandler(this.gvUsers_SelectedIndexChanged);
      this.gvUsers.ItemDoubleClick += new GVItemEventHandler(this.gvUsers_ItemDoubleClick);
      this.gvUsers.ItemDrag += new GVItemEventHandler(this.gvUsers_ItemDrag);
      this.gvUsers.DragOver += new DragEventHandler(this.hierarchyTree_DragOver);
      this.gcOrg.Controls.Add((Control) this.stdIconBtnDeleteOrg);
      this.gcOrg.Controls.Add((Control) this.stdIconBtnEditOrg);
      this.gcOrg.Controls.Add((Control) this.stdIconBtnNewOrg);
      this.gcOrg.Controls.Add((Control) this.hierarchyTree);
      this.gcOrg.Dock = DockStyle.Top;
      this.gcOrg.HeaderForeColor = SystemColors.ControlText;
      this.gcOrg.Location = new Point(0, 0);
      this.gcOrg.Name = "gcOrg";
      this.gcOrg.Size = new Size(965, 211);
      this.gcOrg.TabIndex = 5;
      this.gcOrg.Text = "Organization";
      this.hierarchyTree.AllowDrop = true;
      this.hierarchyTree.BorderStyle = BorderStyle.None;
      this.hierarchyTree.ContextMenu = this.contextMenuOrg;
      this.hierarchyTree.Cursor = Cursors.Default;
      this.hierarchyTree.Dock = DockStyle.Fill;
      this.hierarchyTree.FullRowSelect = true;
      this.hierarchyTree.HideSelection = false;
      this.hierarchyTree.Location = new Point(1, 26);
      this.hierarchyTree.Name = "hierarchyTree";
      this.hierarchyTree.ParentOrgId = 0;
      this.hierarchyTree.Size = new Size(963, 184);
      this.hierarchyTree.Sorted = true;
      this.hierarchyTree.TabIndex = 0;
      this.hierarchyTree.ItemDrag += new ItemDragEventHandler(this.hierarchyTree_ItemDrag);
      this.hierarchyTree.AfterSelect += new TreeViewEventHandler(this.hierarchyTree_AfterSelect);
      this.hierarchyTree.DragDrop += new DragEventHandler(this.hierarchyTree_DragDrop);
      this.hierarchyTree.DragOver += new DragEventHandler(this.hierarchyTree_DragOver);
      this.hierarchyTree.MouseDown += new MouseEventHandler(this.hierarchyTree_MouseDown);
      this.hierarchyTree.MouseMove += new MouseEventHandler(this.hierarchyTree_MouseMove);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(965, 564);
      this.Controls.Add((Control) this.gcUsers);
      this.Controls.Add((Control) this.splitter1);
      this.Controls.Add((Control) this.gcOrg);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (OrgHierarchyForm);
      this.Text = "Organization Hierarchy";
      this.MouseDown += new MouseEventHandler(this.hierarchyTree_MouseDown);
      ((ISupportInitialize) this.stdIconBtnDeleteUser).EndInit();
      ((ISupportInitialize) this.stdIconBtnEditUser).EndInit();
      ((ISupportInitialize) this.stdIconBtnNewUser).EndInit();
      ((ISupportInitialize) this.stdIconBtnDeleteOrg).EndInit();
      ((ISupportInitialize) this.stdIconBtnEditOrg).EndInit();
      ((ISupportInitialize) this.stdIconBtnNewOrg).EndInit();
      this.gcUsers.ResumeLayout(false);
      this.gcOrg.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private bool IsCurrentOrg(TreeNode tn)
    {
      return tn != null && ((OrgNodeTag) tn.Tag).Oid == this.session.UserInfo.OrgId;
    }

    private bool IsUnderCurrentOrg(TreeNode tn)
    {
      if (tn == null)
        return false;
      int orgId = this.session.UserInfo.OrgId;
      bool flag = false;
      for (TreeNode parent = tn.Parent; parent != null; parent = parent.Parent)
      {
        if (((OrgNodeTag) parent.Tag).Oid == orgId)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    private void hierarchyTree_AfterSelect(object sender, TreeViewEventArgs e)
    {
      this.refreshForm((string) null);
    }

    private void refreshForm(string selectedUserid)
    {
      TreeNode selectedNode = this.hierarchyTree.SelectedNode;
      if (selectedNode == null)
        return;
      if (this.IsCurrentOrg(selectedNode))
      {
        this.stdIconBtnNewOrg.Enabled = true;
        this.addOrgMenuItem.Enabled = true;
        this.stdIconBtnDeleteOrg.Enabled = false;
        this.delOrgMenuItem.Enabled = false;
        this.stdIconBtnEditOrg.Text = "Edit Organization";
        this.stdIconBtnNewUser.Enabled = true;
        this.addUserMenuItem.Enabled = true;
        this.stdIconBtnDeleteUser.Enabled = true;
        this.delUserMenuItem.Enabled = true;
        this.stdIconBtnEditUser.Text = "Edit User";
        this.readOnly = false;
      }
      else if (this.IsUnderCurrentOrg(selectedNode))
      {
        this.stdIconBtnNewOrg.Enabled = true;
        this.addOrgMenuItem.Enabled = true;
        this.stdIconBtnDeleteOrg.Enabled = true;
        this.delOrgMenuItem.Enabled = true;
        this.stdIconBtnEditOrg.Text = "Edit Organization";
        this.stdIconBtnNewUser.Enabled = true;
        this.addUserMenuItem.Enabled = true;
        this.stdIconBtnDeleteUser.Enabled = true;
        this.delUserMenuItem.Enabled = true;
        this.stdIconBtnEditUser.Text = "Edit User";
        this.readOnly = false;
      }
      else
      {
        this.stdIconBtnNewOrg.Enabled = false;
        this.addOrgMenuItem.Enabled = false;
        this.stdIconBtnDeleteOrg.Enabled = false;
        this.delOrgMenuItem.Enabled = false;
        this.stdIconBtnEditOrg.Text = "View Organization";
        this.stdIconBtnNewUser.Enabled = false;
        this.addUserMenuItem.Enabled = false;
        this.stdIconBtnDeleteUser.Enabled = false;
        this.delUserMenuItem.Enabled = false;
        this.stdIconBtnEditUser.Text = "View User";
        this.readOnly = this.session.UserInfo.IsAdministrator() && !UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas);
      }
      this.refreshLicenseCounts();
      this.gvUsers.Items.Clear();
      if (!this.session.StartupInfo.EnableSSO && this.gvUsers.Columns.Count >= 9)
      {
        this.gvUsers.Columns.RemoveAt(9);
        this.gvUsers.Columns.RemoveAt(8);
      }
      OrgNodeTag tag = (OrgNodeTag) selectedNode.Tag;
      UserInfo[] usersInOrganization;
      try
      {
        usersInOrganization = this.rOrg.GetUsersInOrganization(tag.Oid);
      }
      catch (ObjectNotFoundException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The selected organization has been deleted and is no longer available.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return;
      }
      if (usersInOrganization == null)
        return;
      for (int index = 0; index < usersInOrganization.Length; ++index)
      {
        string userid = usersInOrganization[index].Userid;
        string lastName = usersInOrganization[index].LastName;
        string firstName = usersInOrganization[index].FirstName;
        ArrayList arrayList = new ArrayList();
        foreach (Persona userPersona in usersInOrganization[index].UserPersonas)
        {
          foreach (Persona persona in this.personaList)
          {
            if (persona.ID == userPersona.ID)
              arrayList.Add((object) persona);
          }
        }
        string str1 = Persona.ToString((Persona[]) arrayList.ToArray(typeof (Persona)));
        string email = usersInOrganization[index].Email;
        string phone = usersInOrganization[index].Phone;
        string str2 = usersInOrganization[index].Locked ? "Disabled" : "Enabled";
        string str3 = usersInOrganization[index].Status == UserInfo.UserStatusEnum.Enabled ? "Enabled" : "Disabled";
        GVItem gvItem = new GVItem(userid);
        gvItem.SubItems.Add((object) lastName);
        gvItem.SubItems.Add((object) firstName);
        gvItem.SubItems.Add((object) str1);
        gvItem.SubItems.Add((object) email);
        gvItem.SubItems.Add((object) phone);
        gvItem.SubItems.Add((object) str2);
        gvItem.SubItems.Add((object) str3);
        if (this.session.StartupInfo.EnableSSO)
        {
          if (usersInOrganization[index].SSOOnly)
            gvItem.SubItems.Add((object) "Restricted");
          else
            gvItem.SubItems.Add((object) "Full");
          GVSubItem gvSubItem = new GVSubItem();
          if (usersInOrganization[index].PasswordRequired)
          {
            gvSubItem.ForeColor = AppColors.AlertRed;
            gvSubItem.Text = "Yes";
          }
          else
            gvSubItem.Text = "No";
          gvItem.SubItems.Add(gvSubItem);
        }
        this.gvUsers.Items.Add(gvItem);
        if (selectedUserid != null && selectedUserid == userid)
          gvItem.Selected = true;
      }
      this.gvUsers.ReSort();
      if (this.userid != null)
      {
        for (int nItemIndex = 0; nItemIndex < this.gvUsers.Items.Count; ++nItemIndex)
        {
          if (this.gvUsers.Items[nItemIndex].Text == this.userid)
          {
            this.gvUsers.SelectedItems.Clear();
            this.gvUsers.Items[nItemIndex].Selected = true;
            break;
          }
        }
      }
      this.userid = (string) null;
      this.enableDisableListViewButtons();
    }

    public string[] SelectedOrgPath
    {
      get
      {
        TreeNode selectedNode = this.hierarchyTree.SelectedNode;
        if (selectedNode == null)
          return (string[]) null;
        if (!(selectedNode.Tag is OrgNodeTag))
          return (string[]) null;
        OrgInfo organization = this.session.OrganizationManager.GetOrganization(((OrgNodeTag) selectedNode.Tag).Oid);
        if (organization == null)
          return new string[0];
        return new string[1]
        {
          this.session.OrganizationManager.GetOrgPath(organization.Oid)
        };
      }
      set
      {
        if (value == null || !(value[0] != ""))
          return;
        this.hierarchyTree.ExpandAll();
        this.MakeTreeNodeSelected(this.hierarchyTree.Nodes[0], value[0]);
      }
    }

    private bool MakeTreeNodeSelected(TreeNode node, string selectedOrgPath)
    {
      bool flag = false;
      if (this.session.OrganizationManager.GetOrgPath(this.session.OrganizationManager.GetOrganization(((OrgNodeTag) node.Tag).Oid).Oid) == selectedOrgPath)
      {
        flag = true;
        this.hierarchyTree.SelectedNode = node;
      }
      if (!flag)
      {
        foreach (TreeNode node1 in node.Nodes)
        {
          if (this.MakeTreeNodeSelected(node1, selectedOrgPath))
          {
            flag = true;
            break;
          }
        }
      }
      return flag;
    }

    public string[] SelectedUserList
    {
      get
      {
        GVSelectedItemCollection selectedItems = this.gvUsers.SelectedItems;
        List<string> stringList = new List<string>();
        if (selectedItems.Count > 0)
        {
          foreach (GVItem gvItem in selectedItems)
            stringList.Add(gvItem.Text);
        }
        return stringList.ToArray();
      }
      set
      {
        List<string> stringList = new List<string>((IEnumerable<string>) value);
        if (stringList.Count == 0)
          return;
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvUsers.Items)
        {
          if (stringList.Contains(gvItem.Text))
            gvItem.Selected = true;
        }
      }
    }

    private void refreshLicenseCounts()
    {
      string str1 = this.rOrg.GetEnabledUserCount().ToString("#,##0");
      LicenseInfo serverLicense = this.session.ConfigurationManager.GetServerLicense();
      string str2 = "Unknown";
      if (serverLicense.UserLimit > 0 && serverLicense.UserLimit < 999999)
        str2 = serverLicense.UserLimit.ToString("#,##0");
      else if (serverLicense.UserLimit == 0 || serverLicense.UserLimit >= 999999)
        str2 = "Unlimited";
      this.gcUsers.Text = "Enabled Users (" + str1 + ")     User Licenses (" + str2 + ")";
    }

    private void hierarchyTree_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      TreeNode nodeAt = this.hierarchyTree.GetNodeAt(e.X, e.Y);
      if (nodeAt == null)
        return;
      this.hierarchyTree.SelectedNode = nodeAt;
    }

    private void addOrgMenuItem_Click(object sender, EventArgs e) => this.addEditOrg(false);

    private void editOrgMenuItem_Click(object sender, EventArgs e)
    {
      this.addEditOrg(true);
      this.refreshForm((string) null);
    }

    private void delOrgMenuItem_Click(object sender, EventArgs e) => this.delOrg();

    private void addUserMenuItem_Click(object sender, EventArgs e) => this.addEditUser(false);

    private void editUserMenuItem_Click(object sender, EventArgs e) => this.addEditUser(true);

    private void gvUsers_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.addEditUser(true);
    }

    private void delUserMenuItem_Click(object sender, EventArgs e) => this.delUser();

    private void addEditOrg(bool edit)
    {
      if (this.hierarchyTree.SelectedNode == null)
        return;
      int oid = ((OrgNodeTag) this.hierarchyTree.SelectedNode.Tag).Oid;
      AddEditOrgDialog addEditOrgDialog = !edit ? new AddEditOrgDialog(this.session, -1, oid) : new AddEditOrgDialog(this.session, oid, this.readOnly);
      if (addEditOrgDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
        return;
      string orgName = addEditOrgDialog.OrgName;
      string orgDescription = addEditOrgDialog.OrgDescription;
      string orgCode = addEditOrgDialog.OrgCode;
      string orgCoName = addEditOrgDialog.OrgCoName;
      Address companyAddress = new Address(addEditOrgDialog.OrgAddress1, addEditOrgDialog.OrgAddress2, addEditOrgDialog.OrgCity, addEditOrgDialog.OrgState, addEditOrgDialog.OrgZip, addEditOrgDialog.OrgUnitType);
      string orgPhone = addEditOrgDialog.OrgPhone;
      string orgFax = addEditOrgDialog.OrgFax;
      string nmlsCode = addEditOrgDialog.NMLSCode;
      string mersMinCode = addEditOrgDialog.MERSMinCode;
      bool showOrgInLoSearch = addEditOrgDialog.ShowOrgInLOSearch;
      string loSearchOrgName = addEditOrgDialog.LOSearch_OrgName;
      int hmdaProfileId = addEditOrgDialog.HMDAProfileId;
      string[] dbNames = (string[]) null;
      if (addEditOrgDialog.DBNames != null)
        dbNames = new string[4]
        {
          addEditOrgDialog.DBNames[0],
          addEditOrgDialog.DBNames[1],
          addEditOrgDialog.DBNames[2],
          addEditOrgDialog.DBNames[3]
        };
      BranchExtLicensing orgBranchLicensing = addEditOrgDialog.OrgBranchLicensing;
      LoanCompHistoryList loCompHistoryList = addEditOrgDialog.LOCompHistoryList;
      ONRPEntitySettings retailBranchSettings = addEditOrgDialog.ONRPRetailBranchSettings;
      CCSiteInfo ccSiteSettings = addEditOrgDialog.CCSiteSettings;
      SSOInfo ssoSettings = addEditOrgDialog.SSOSettings;
      if (!edit)
      {
        int organization = this.rOrg.CreateOrganization(new OrgInfo(orgName, orgDescription, oid, orgCode, orgCoName, companyAddress, orgPhone, orgFax, nmlsCode, mersMinCode, dbNames, (BranchExtLicensing) orgBranchLicensing.Clone(), loCompHistoryList, retailBranchSettings, ccSiteSettings, ssoSettings, showOrgInLoSearch, loSearchOrgName, hmdaProfileId));
        if (ssoSettings != null)
          this.rOrg.UpdateOrgSSOUsers(organization, ssoSettings.LoginAccess, false);
        this.rOrg.CreateCCSiteInfo(ccSiteSettings, organization);
        if (!this.hierarchyTree.SelectedNode.IsExpanded && this.hierarchyTree.SelectedNode.Nodes.Count == 1 && this.hierarchyTree.SelectedNode.Nodes[0].Text == "<DUMMY NODE>")
        {
          this.hierarchyTree.SelectedNode.Expand();
          foreach (TreeNode node in this.hierarchyTree.SelectedNode.Nodes)
          {
            if (node.Text == orgName)
            {
              this.hierarchyTree.SelectedNode = node;
              break;
            }
          }
        }
        else
        {
          TreeNode node = new TreeNode(orgName, 0, 1);
          node.Tag = (object) new OrgNodeTag(organization, orgDescription);
          this.hierarchyTree.SelectedNode.Nodes.Add(node);
          this.hierarchyTree.SelectedNode = node;
        }
      }
      else
      {
        OrgInfo info = new OrgInfo(oid, orgName, orgDescription, -1, orgCode, orgCoName, companyAddress, orgPhone, orgFax, (int[]) null, nmlsCode, mersMinCode, dbNames, (BranchExtLicensing) orgBranchLicensing.Clone(), loCompHistoryList != null ? (LoanCompHistoryList) loCompHistoryList.Clone() : (LoanCompHistoryList) null, retailBranchSettings, ccSiteSettings, ssoSettings, showOrgInLoSearch, loSearchOrgName, hmdaProfileId);
        Cursor.Current = Cursors.WaitCursor;
        this.rOrg.UpdateOrganization(info);
        if (info.SSOSettings != null)
          this.rOrg.UpdateOrgSSOUsers(info.Oid, info.SSOSettings.LoginAccess, false);
        Cursor.Current = Cursors.Default;
        this.hierarchyTree.SelectedNode.Text = orgName;
        this.hierarchyTree.SelectedNode.Tag = (object) new OrgNodeTag(oid, orgDescription);
      }
    }

    private void delOrg()
    {
      if (this.hierarchyTree.SelectedNode == null)
        return;
      if (this.hierarchyTree.SelectedNode.Parent == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You cannot delete the root of the hierarchy.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        int oid = ((OrgNodeTag) this.hierarchyTree.SelectedNode.Tag).Oid;
        UserInfo[] usersInOrganization = this.rOrg.GetUsersInOrganization(oid);
        if (this.hierarchyTree.SelectedNode.Nodes.Count > 0 || usersInOrganization != null && usersInOrganization.Length != 0)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The selected organization is not empty - it contains sub-organizations and/or users.  You can only delete empty organizations.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          if (MessageBox.Show((IWin32Window) this, "Are you sure you want to delete the organization '" + this.hierarchyTree.SelectedNode.Text + "'?", "Delete Confirmation", MessageBoxButtons.YesNo) == DialogResult.No)
            return;
          this.rOrg.DeleteOrganization(oid);
          this.hierarchyTree.SelectedNode.Remove();
        }
      }
    }

    private void addEditUser(bool edit)
    {
      if (this.hierarchyTree.SelectedNode == null)
        return;
      int oid = ((OrgNodeTag) this.hierarchyTree.SelectedNode.Tag).Oid;
      string str1 = string.Empty;
      if (edit)
      {
        if (this.gvUsers.SelectedItems != null && this.gvUsers.SelectedItems.Count > 1)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You can only select one user to edit at a time.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        if (this.gvUsers.SelectedItems == null || this.gvUsers.SelectedItems.Count == 0 || this.gvUsers.SelectedItems[0] == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must select a user.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return;
        }
        str1 = this.gvUsers.SelectedItems[0].Text;
        if (this.session.OrganizationManager.GetUser(str1) == (UserInfo) null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "User \"" + str1 + "\" does not exist.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.gvUsers.Items.Remove(this.gvUsers.SelectedItems[0]);
          return;
        }
      }
      UserInfo userInfoFromDialog = SetupUtil.GetNewUserInfoFromDialog(this.session, (IWin32Window) this, edit, str1, oid, this.readOnly);
      if (userInfoFromDialog == (UserInfo) null)
        return;
      GVItem gvItem;
      if (!edit)
      {
        gvItem = new GVItem(userInfoFromDialog.Userid);
      }
      else
      {
        if (userInfoFromDialog.Userid == this.session.UserID)
          this.refreshForm(userInfoFromDialog.Userid);
        gvItem = this.gvUsers.SelectedItems[0];
      }
      if (edit)
      {
        gvItem.SubItems.Clear();
        gvItem.Text = userInfoFromDialog.Userid;
      }
      gvItem.SubItems.Add((object) userInfoFromDialog.LastName);
      gvItem.SubItems.Add((object) userInfoFromDialog.FirstName);
      string str2 = "";
      if (userInfoFromDialog.UserPersonas != null)
      {
        for (int index = 0; index < userInfoFromDialog.UserPersonas.Length; ++index)
          str2 = index != 0 ? str2 + " + " + userInfoFromDialog.UserPersonas[index].Name : userInfoFromDialog.UserPersonas[index].Name;
      }
      gvItem.SubItems.Add((object) str2);
      gvItem.SubItems.Add((object) userInfoFromDialog.Email);
      gvItem.SubItems.Add((object) userInfoFromDialog.Phone);
      gvItem.SubItems.Add(userInfoFromDialog.Locked ? (object) "Disabled" : (object) "Enabled");
      gvItem.SubItems.Add(userInfoFromDialog.Status == UserInfo.UserStatusEnum.Enabled ? (object) "Enabled" : (object) "Disabled");
      if (this.session.StartupInfo.EnableSSO)
      {
        gvItem.SubItems.Add(userInfoFromDialog.SSOOnly ? (object) "Restricted" : (object) "Full");
        GVSubItem gvSubItem = new GVSubItem();
        if (userInfoFromDialog.PasswordRequired)
        {
          gvSubItem.ForeColor = AppColors.AlertRed;
          gvSubItem.Text = "Yes";
        }
        else
          gvSubItem.Text = "No";
        gvItem.SubItems.Add(gvSubItem);
      }
      if (!edit)
        this.gvUsers.Items.Add(gvItem);
      this.refreshLicenseCounts();
    }

    private void delUser()
    {
      if (this.hierarchyTree.SelectedNode == null)
        return;
      if (this.gvUsers.SelectedItems == null || this.gvUsers.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must select at least one user to delete.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        IEnumerator enumerator = (IEnumerator) this.gvUsers.SelectedItems.GetEnumerator();
        while (enumerator.MoveNext())
        {
          GVItem current = (GVItem) enumerator.Current;
          string text = current.Text;
          if (string.Compare(text, "tpowcadmin", true) == 0)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "You cannot delete or disable the tpowcadmin user account. Your TPO WebCenter website relies on this tpowcadmin account to communicate with your Encompass system. Disabling or deleting this account will cause your TPO WebCenter to stop working and prevent loan data from being passed between your website and your Encompass system.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
          if (this.session.ConfigurationManager.CheckIfSalesRepHasAnyContacts(text, -1))
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "The user you are deleting is assigned to one or more TPO contacts. You must reassign their TPO contacts prior to deleting them", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
          if (this.session.OrganizationManager.GetUser(text) == (UserInfo) null)
          {
            int num4 = (int) Utils.Dialog((IWin32Window) this, "User \"" + text + "\" does not exist.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.gvUsers.Items.Remove(current);
          }
          else if ("admin".Equals(text))
          {
            int num5 = (int) Utils.Dialog((IWin32Window) this, "You cannot delete user Admin.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          else if (this.session.ServerManager.GetAllUserSessionIds(text).Length != 0)
          {
            int num6 = (int) Utils.Dialog((IWin32Window) this, "You cannot delete user \"" + text + "\" because this user account is currently logged in.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          else if (Utils.Dialog((IWin32Window) this, "Before you proceed, reassign the loans that are owned by this user." + Environment.NewLine + Environment.NewLine + "On the next window, you can reassign or delete the contacts owned by the user. All other personal settings will be removed." + Environment.NewLine + Environment.NewLine + "Click Yes to continue.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.No && DialogResult.Cancel != new DeleteUserDialog(this.session, text).ShowDialog((IWin32Window) this.session.MainScreen))
          {
            this.session.OrganizationManager.DeleteAllLOLicenses(text);
            this.session.OrganizationManager.DeleteUser(text);
            this.gvUsers.Items.Remove(current);
          }
        }
        this.refreshLicenseCounts();
      }
    }

    private void hierarchyTree_MouseMove(object sender, MouseEventArgs e)
    {
      TreeNode nodeAt = this.hierarchyTree.GetNodeAt(e.X, e.Y);
      if (nodeAt != null)
      {
        if (nodeAt.Text.Equals(this.oldNode))
          return;
        this.oldNode = nodeAt.Text;
        this.hierarchyToolTip.Active = false;
        this.hierarchyToolTip.SetToolTip((Control) this.hierarchyTree, ((OrgNodeTag) nodeAt.Tag).Description);
        this.hierarchyToolTip.Active = true;
      }
      else
      {
        this.hierarchyToolTip.Active = false;
        this.oldNode = "";
      }
    }

    private void hierarchyTree_ItemDrag(object sender, ItemDragEventArgs e)
    {
      this.startNode = (TreeNode) e.Item;
      int num = (int) this.DoDragDrop(e.Item, DragDropEffects.Move);
    }

    private void hierarchyTree_DragOver(object sender, DragEventArgs e)
    {
      TreeNode nodeAt = this.hierarchyTree.GetNodeAt(this.hierarchyTree.PointToClient(new Point(e.X, e.Y)));
      if (nodeAt == null)
        e.Effect = DragDropEffects.Move;
      else if (!e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
      {
        if (this.readOnly || !this.IsCurrentOrg(nodeAt) && !this.IsUnderCurrentOrg(nodeAt))
          e.Effect = DragDropEffects.None;
        else
          e.Effect = DragDropEffects.Move;
      }
      else if (this.IsCurrentOrg(this.startNode) || !this.IsUnderCurrentOrg(this.startNode))
        e.Effect = DragDropEffects.None;
      else if (this.IsCurrentOrg(nodeAt) || this.IsUnderCurrentOrg(nodeAt))
        e.Effect = DragDropEffects.Move;
      else
        e.Effect = e.Effect = DragDropEffects.None;
    }

    private void hierarchyTree_DragDrop(object sender, DragEventArgs e)
    {
      TreeNode nodeAt = this.hierarchyTree.GetNodeAt(this.hierarchyTree.PointToClient(new Point(e.X, e.Y)));
      if (!e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
      {
        ArrayList arrayList = new ArrayList();
        List<string> coonectedUsers = new List<string>();
        int oid = ((OrgNodeTag) nodeAt.Tag).Oid;
        OrgInfo organization = this.session.OrganizationManager.GetOrganization(oid);
        bool flag = organization.Oid != organization.Parent;
        for (int index = 0; index < this.gvUsers.SelectedItems.Count; ++index)
        {
          string text = this.gvUsers.SelectedItems[index].Text;
          UserInfo user = this.session.OrganizationManager.GetUser(text);
          if (!UserInfo.IsSuperAdministrator(user.Userid, user.UserPersonas))
          {
            arrayList.Add((object) text);
            if (!user.SSODisconnectedFromOrg)
              coonectedUsers.Add(text);
          }
          else if (flag)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Super Administrators have to be located at the top of the organization hierarchy and can not be moved to lower levels", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
        }
        if (Utils.Dialog((IWin32Window) this, "The LO Comp plans will no longer be associated with the parent organization comp plan. Do you want to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.OK)
          return;
        this.rOrg.MoveUsersIntoOrganization((string[]) arrayList.ToArray(typeof (string)), oid, coonectedUsers, this.session.StartupInfo.EnableSSO);
        this.hierarchyTree.SelectedNode = nodeAt;
        nodeAt.Expand();
      }
      else if (this.startNode.Parent == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You cannot move the root of the hierarchy.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (this.startNode.Parent == nodeAt || nodeAt.Equals((object) this.startNode))
          return;
        this.hierarchyTree.SelectedNode = nodeAt;
        int oid1 = ((OrgNodeTag) this.startNode.Tag).Oid;
        for (TreeNode parent = nodeAt.Parent; parent != null; parent = parent.Parent)
        {
          if (((OrgNodeTag) parent.Tag).Oid == oid1)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "You cannot move an organization to its descendent organization.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        if (Utils.Dialog((IWin32Window) this, "The LO Comp plans will no longer be associated with the parent organization comp plan. Do you want to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.OK)
          return;
        TreeNode node = new TreeNode(this.startNode.Text, 0, 1);
        node.Tag = this.startNode.Tag;
        int oid2 = ((OrgNodeTag) nodeAt.Tag).Oid;
        int oid3 = ((OrgNodeTag) node.Tag).Oid;
        OrgInfo organization = this.rOrg.GetOrganization(oid3);
        if (this.session.StartupInfo.EnableSSO && organization.SSOSettings.UseParentInfo)
        {
          SSOOrgConfirmationDialog confirmationDialog = new SSOOrgConfirmationDialog(this.session);
          if (DialogResult.OK != confirmationDialog.ShowDialog())
            return;
          this.rOrg.UpdateOrgSSOUsers(oid3, this.rOrg.GetFirstOrganizationForSSO(oid2).SSOSettings.LoginAccess, confirmationDialog.ApplyToAll);
        }
        this.rOrg.MoveOrganization(oid3, oid2);
        this.startNode.Remove();
        this.startNode = (TreeNode) null;
        if (organization.Children.Length != 0)
          node.Nodes.Add(new TreeNode("<DUMMY NODE>", 0, 1));
        if (nodeAt.Nodes.Count > 1 || nodeAt.Nodes.Count == 1 && string.Compare(nodeAt.Nodes[0].Text, "<DUMMY NODE>", true) != 0)
          nodeAt.Nodes.Add(node);
        nodeAt.Collapse();
        nodeAt.Expand();
        if (nodeAt.Nodes.Count != 0)
          return;
        nodeAt.Nodes.Add(node);
      }
    }

    private void gvUsers_ItemDrag(object sender, GVItemEventArgs e)
    {
      foreach (GVItem selectedItem in this.gvUsers.SelectedItems)
      {
        if (selectedItem.Text.ToLower() == this.session.UserInfo.Userid.ToLower())
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You cannot move yourself to a different organization.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
      }
      int num1 = (int) this.DoDragDrop((object) e.Item, DragDropEffects.Move);
    }

    private void stdIconBtnNewOrg_Click(object sender, EventArgs e) => this.addEditOrg(false);

    private void stdIconBtnEditOrg_Click(object sender, EventArgs e)
    {
      this.addEditOrg(true);
      this.refreshForm((string) null);
    }

    private void stdIconBtnDeleteOrg_Click(object sender, EventArgs e) => this.delOrg();

    private void stdIconBtnNewUser_Click(object sender, EventArgs e) => this.addEditUser(false);

    private void stdIconBtnEditUser_Click(object sender, EventArgs e) => this.addEditUser(true);

    private void stdIconBtnDeleteUser_Click(object sender, EventArgs e) => this.delUser();

    private void selectAllBtn_Click(object sender, EventArgs e)
    {
      for (int nItemIndex = 0; nItemIndex < this.gvUsers.Items.Count; ++nItemIndex)
        this.gvUsers.Items[nItemIndex].Selected = true;
    }

    private void enableDisableListViewButtons()
    {
      if (this.gvUsers.SelectedItems.Count > 0)
      {
        this.stdIconBtnEditUser.Enabled = this.gvUsers.SelectedItems.Count == 1;
        this.editUserMenuItem.Enabled = this.gvUsers.SelectedItems.Count == 1;
        this.stdIconBtnDeleteUser.Enabled = true;
        this.delUserMenuItem.Enabled = true;
      }
      else
      {
        this.stdIconBtnEditUser.Enabled = false;
        this.editUserMenuItem.Enabled = false;
        this.stdIconBtnDeleteUser.Enabled = false;
        this.delUserMenuItem.Enabled = false;
      }
    }

    private void gvUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.enableDisableListViewButtons();
      IEnumerator enumerator = (IEnumerator) this.gvUsers.SelectedItems.GetEnumerator();
      while (enumerator.MoveNext())
      {
        this.userid = ((GVItem) enumerator.Current).Text;
        if (this.userid == this.session.UserID)
        {
          this.stdIconBtnDeleteUser.Enabled = false;
          this.delUserMenuItem.Enabled = false;
          break;
        }
        if (this.userid == "admin")
        {
          this.stdIconBtnDeleteUser.Enabled = false;
          this.delUserMenuItem.Enabled = false;
          break;
        }
      }
    }

    private void gvUsers_DragOver(object sender, DragEventArgs e) => e.Effect = e.AllowedEffect;
  }
}
