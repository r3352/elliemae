// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.UserFilterControl
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class UserFilterControl : UserControl
  {
    private bool hasOrganizationRight;
    private bool hasUserGroupRight;
    private List<RoleInfo> roles = new List<RoleInfo>();
    private OrgInfo currOrganization;
    private AclGroup currUserGroup;
    private int origRoleId = RoleInfo.All.ID;
    private int origOrganizationId = OrgInfo.NoOrgId;
    private bool origIncludeChildren;
    private int origUserGroupId = AclGroup.NoUserGroupId;
    private int currRoleId = RoleInfo.All.ID;
    private int currOrganizationId = OrgInfo.NoOrgId;
    private bool currIncludeChildren;
    private int currUserGroupId = AclGroup.NoUserGroupId;
    private Sessions.Session session;
    private IContainer components;
    private ImageList imgList;
    protected internal PictureBox picSearch;
    protected internal TextBox txtMemberOf;
    protected internal ComboBox cboRole;
    protected internal ComboBox cboMemberOf;
    protected internal Label lblMemberOf;
    protected internal Label lblRole;
    private ToolTip toolTip1;

    public int RoleId
    {
      get => this.currRoleId;
      set
      {
        if (this.DesignMode)
          return;
        this.origRoleId = value;
        this.currRoleId = value;
        this.setRoleFilterControls();
      }
    }

    public int OrganizationId
    {
      get => this.currOrganizationId;
      set
      {
        if (this.DesignMode)
          return;
        this.origOrganizationId = value;
        this.currOrganizationId = value;
        this.setMemberOfFilterControls();
      }
    }

    public bool IncludeChildren
    {
      get => this.currIncludeChildren;
      set
      {
        if (this.DesignMode)
          return;
        this.origIncludeChildren = value;
        this.currIncludeChildren = value;
      }
    }

    public int UserGroupId
    {
      get => this.currUserGroupId;
      set
      {
        if (this.DesignMode)
          return;
        this.origUserGroupId = value;
        this.currUserGroupId = value;
        this.setMemberOfFilterControls();
      }
    }

    public UserFilterControl(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      if (this.DesignMode || !this.session.IsConnected)
        return;
      this.initializeControl();
    }

    private void initializeControl()
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.hasOrganizationRight = aclManager.GetUserApplicationRight(AclFeature.DashboardTab_Organization);
      this.hasUserGroupRight = aclManager.GetUserApplicationRight(AclFeature.DashboardTab_UserGroup);
      this.getRoles();
      this.cboRole.DataSource = (object) this.roles;
      this.cboRole.DisplayMember = "Name";
      this.cboRole.ValueMember = "ID";
      this.cboMemberOf.Items.Add((object) "All");
      this.cboMemberOf.Items.Add((object) "Organization");
      this.cboMemberOf.Items.Add((object) "User Group");
    }

    private void getRoles()
    {
      this.roles.Add(RoleInfo.All);
      RoleInfo[] allRoleFunctions = ((WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
      Array.Sort((Array) allRoleFunctions, (IComparer) new RolesComparer());
      this.roles.AddRange((IEnumerable<RoleInfo>) allRoleFunctions);
    }

    private void setRoleFilterControls() => this.cboRole.SelectedValue = (object) this.currRoleId;

    private void setMemberOfFilterControls()
    {
      if (OrgInfo.NoOrgId == this.currOrganizationId && AclGroup.NoUserGroupId == this.currUserGroupId)
      {
        this.cboMemberOf.SelectedIndex = this.cboMemberOf.FindString("All");
        this.txtMemberOf.Text = string.Empty;
        this.setIconButton(this.picSearch, false);
      }
      else if (OrgInfo.NoOrgId != this.currOrganizationId)
      {
        this.cboMemberOf.SelectedIndex = this.cboMemberOf.FindString("Organization");
        this.currOrganization = this.session.OrganizationManager.GetOrganization(this.currOrganizationId);
        this.txtMemberOf.Text = this.currIncludeChildren ? this.currOrganization.Description + " and below" : this.currOrganization.Description;
        this.setIconButton(this.picSearch, true);
      }
      else
      {
        this.cboMemberOf.SelectedIndex = this.cboMemberOf.FindString("User Group");
        this.currUserGroup = this.session.AclGroupManager.GetGroupById(this.currUserGroupId);
        this.txtMemberOf.Text = this.currUserGroup.Name;
        this.setIconButton(this.picSearch, true);
      }
    }

    private void getRoleSelection()
    {
      this.currRoleId = this.roles[this.cboRole.SelectedIndex].ID;
      this.checkDataChanged();
    }

    private void getMemberOfSelection()
    {
      switch (this.cboMemberOf.Text)
      {
        case "All":
          this.clearMemberOfFilterValues();
          break;
        case "Organization":
          if (this.hasOrganizationRight)
          {
            using (OrganizationDialog organizationDialog = new OrganizationDialog(this.session))
            {
              organizationDialog.IncludeChildren = true;
              if (OrgInfo.NoOrgId != this.currOrganizationId)
              {
                if (this.currOrganization == null)
                  this.currOrganization = this.session.OrganizationManager.GetOrganization(this.currOrganizationId);
                organizationDialog.SelectedOrganization = this.currOrganization;
                organizationDialog.IncludeChildren = this.currIncludeChildren;
              }
              if (DialogResult.OK == organizationDialog.ShowDialog())
              {
                this.clearMemberOfFilterValues();
                this.currOrganization = organizationDialog.SelectedOrganization;
                this.currOrganizationId = this.currOrganization.Oid;
                this.currIncludeChildren = organizationDialog.IncludeChildren;
                break;
              }
              break;
            }
          }
          else
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The 'Filter Data by Organization' access right is required for this option. Contact your Administrator for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            break;
          }
        case "User Group":
          if (this.hasUserGroupRight)
          {
            using (UserGroupDialog userGroupDialog = new UserGroupDialog(this.session))
            {
              if (AclGroup.NoUserGroupId != this.currUserGroupId)
              {
                if ((AclGroup) null == this.currUserGroup)
                  this.currUserGroup = this.session.AclGroupManager.GetGroupById(this.currUserGroupId);
                userGroupDialog.SelectedUserGroup = this.currUserGroup;
              }
              if (DialogResult.OK == userGroupDialog.ShowDialog())
              {
                this.clearMemberOfFilterValues();
                this.currUserGroup = userGroupDialog.SelectedUserGroup;
                this.currUserGroupId = this.currUserGroup.ID;
                break;
              }
              break;
            }
          }
          else
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The 'Filter Data by User Group' access right is required for this option. Contact your Administrator for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            break;
          }
      }
      this.setMemberOfFilterControls();
      this.checkDataChanged();
    }

    private void clearMemberOfFilterValues()
    {
      this.currOrganizationId = OrgInfo.NoOrgId;
      this.currIncludeChildren = false;
      this.currUserGroupId = AclGroup.NoUserGroupId;
    }

    private void checkDataChanged()
    {
      if (this.origRoleId == this.currRoleId && this.origOrganizationId == this.currOrganizationId && this.origIncludeChildren == this.currIncludeChildren && this.origUserGroupId == this.currUserGroupId)
        return;
      this.OnDataChanged(EventArgs.Empty);
    }

    private void setIconButton(PictureBox pictureBox, bool enable)
    {
      if (enable && this.Parent.Enabled)
      {
        pictureBox.Image = this.imgList.Images[pictureBox.Name];
        pictureBox.Enabled = true;
      }
      else
      {
        pictureBox.Image = this.imgList.Images[pictureBox.Name + "Disabled"];
        pictureBox.Enabled = false;
      }
    }

    private void cboRole_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.getRoleSelection();
    }

    private void cboMemberOf_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.getMemberOfSelection();
    }

    private void picSearch_Click(object sender, EventArgs e) => this.getMemberOfSelection();

    private void picSearch_MouseEnter(object sender, EventArgs e)
    {
      PictureBox pictureBox = (PictureBox) sender;
      pictureBox.Image = this.imgList.Images[pictureBox.Name + "MouseOver"];
    }

    private void picSearch_MouseLeave(object sender, EventArgs e)
    {
      PictureBox pictureBox = (PictureBox) sender;
      if (!pictureBox.Enabled)
        return;
      pictureBox.Image = this.imgList.Images[pictureBox.Name];
    }

    public event UserFilterControl.DataChangedEventHandler DataChangedEvent;

    protected virtual void OnDataChanged(EventArgs e)
    {
      if (this.DataChangedEvent == null)
        return;
      this.DataChangedEvent((object) this, e);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (UserFilterControl));
      this.picSearch = new PictureBox();
      this.txtMemberOf = new TextBox();
      this.lblMemberOf = new Label();
      this.cboRole = new ComboBox();
      this.lblRole = new Label();
      this.cboMemberOf = new ComboBox();
      this.imgList = new ImageList(this.components);
      this.toolTip1 = new ToolTip(this.components);
      ((ISupportInitialize) this.picSearch).BeginInit();
      this.SuspendLayout();
      this.picSearch.BackColor = SystemColors.Control;
      this.picSearch.Enabled = false;
      this.picSearch.Image = (Image) componentResourceManager.GetObject("picSearch.Image");
      this.picSearch.Location = new Point(477, 29);
      this.picSearch.Name = "picSearch";
      this.picSearch.Size = new Size(16, 16);
      this.picSearch.TabIndex = 459;
      this.picSearch.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.picSearch, "Lookup");
      this.picSearch.MouseLeave += new EventHandler(this.picSearch_MouseLeave);
      this.picSearch.Click += new EventHandler(this.picSearch_Click);
      this.picSearch.MouseEnter += new EventHandler(this.picSearch_MouseEnter);
      this.txtMemberOf.Location = new Point(287, 27);
      this.txtMemberOf.Name = "txtMemberOf";
      this.txtMemberOf.ReadOnly = true;
      this.txtMemberOf.Size = new Size(184, 20);
      this.txtMemberOf.TabIndex = 458;
      this.lblMemberOf.AutoSize = true;
      this.lblMemberOf.Location = new Point(0, 31);
      this.lblMemberOf.Name = "lblMemberOf";
      this.lblMemberOf.Size = new Size(88, 13);
      this.lblMemberOf.TabIndex = 457;
      this.lblMemberOf.Text = "Org/User Groups";
      this.lblMemberOf.TextAlign = ContentAlignment.MiddleRight;
      this.cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRole.FormattingEnabled = true;
      this.cboRole.Location = new Point(97, 0);
      this.cboRole.Name = "cboRole";
      this.cboRole.Size = new Size(184, 21);
      this.cboRole.TabIndex = 456;
      this.cboRole.SelectionChangeCommitted += new EventHandler(this.cboRole_SelectionChangeCommitted);
      this.lblRole.AutoSize = true;
      this.lblRole.Location = new Point(0, 4);
      this.lblRole.Name = "lblRole";
      this.lblRole.Size = new Size(59, 13);
      this.lblRole.TabIndex = 455;
      this.lblRole.Text = "User Roles";
      this.lblRole.TextAlign = ContentAlignment.MiddleRight;
      this.cboMemberOf.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboMemberOf.FormattingEnabled = true;
      this.cboMemberOf.Location = new Point(97, 27);
      this.cboMemberOf.Name = "cboMemberOf";
      this.cboMemberOf.Size = new Size(184, 21);
      this.cboMemberOf.TabIndex = 460;
      this.cboMemberOf.SelectionChangeCommitted += new EventHandler(this.cboMemberOf_SelectionChangeCommitted);
      this.imgList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgList.ImageStream");
      this.imgList.TransparentColor = Color.Transparent;
      this.imgList.Images.SetKeyName(0, "picSearch");
      this.imgList.Images.SetKeyName(1, "picSearchMouseOver");
      this.imgList.Images.SetKeyName(2, "picSearchDisabled");
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.cboMemberOf);
      this.Controls.Add((Control) this.picSearch);
      this.Controls.Add((Control) this.txtMemberOf);
      this.Controls.Add((Control) this.lblMemberOf);
      this.Controls.Add((Control) this.cboRole);
      this.Controls.Add((Control) this.lblRole);
      this.Name = nameof (UserFilterControl);
      this.Size = new Size(493, 48);
      ((ISupportInitialize) this.picSearch).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public delegate void DataChangedEventHandler(object sender, EventArgs e);
  }
}
