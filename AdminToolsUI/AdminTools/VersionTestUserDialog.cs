// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.VersionTestUserDialog
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class VersionTestUserDialog : Form
  {
    private VersionManagementGroup versionGroup;
    private bool showWarningOnRemove;
    private IContainer components;
    private DialogButtons dlgButtons;
    private Label label1;
    private GroupContainer grpUsers;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnRemoveUsers;
    private StandardIconButton btnAddUsers;
    private GridView gvTestUsers;
    private ToolTip toolTip1;

    public VersionTestUserDialog(VersionManagementGroup group, bool showWarningOnRemove)
    {
      this.InitializeComponent();
      this.versionGroup = group;
      this.showWarningOnRemove = showWarningOnRemove;
      this.loadUsers();
    }

    private void loadUsers()
    {
      UserInfoSummary[] managementGroupUsers = Session.ServerManager.GetVersionManagementGroupUsers(this.versionGroup.GroupID);
      this.gvTestUsers.Items.Clear();
      foreach (UserInfoSummary user in managementGroupUsers)
        this.gvTestUsers.Items.Add(this.createGVItemForUser(user));
      this.refreshUserCount();
    }

    private GVItem createGVItemForUser(UserInfoSummary user)
    {
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Text = user.UserID
          },
          [1] = {
            Text = user.FirstName
          },
          [2] = {
            Text = user.LastName
          }
        },
        Tag = (object) user
      };
    }

    private void refreshUserCount()
    {
      this.grpUsers.Text = "Test Users (" + (object) this.gvTestUsers.Items.Count + ")";
    }

    private void gvTestUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnRemoveUsers.Enabled = this.gvTestUsers.SelectedItems.Count > 0;
    }

    private void btnAddUsers_Click(object sender, EventArgs e)
    {
      using (UserPicker userPicker = new UserPicker(Session.OrganizationManager.GetAllUserInfoSummary(), true))
      {
        if (userPicker.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        foreach (UserInfoSummary selectedUserInfoSummary in userPicker.GetSelectedUserInfoSummaries())
        {
          if (!this.isTestUser(selectedUserInfoSummary))
            this.gvTestUsers.Items.Add(this.createGVItemForUser(selectedUserInfoSummary));
        }
        this.gvTestUsers.ReSort();
      }
      this.refreshUserCount();
    }

    private void btnRemoveUsers_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "The selected user(s) will be removed from the test group going forward. However, any hot updates that have already been applied to their system will remain in place.", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.OK)
        return;
      while (this.gvTestUsers.SelectedItems.Count > 0)
        this.gvTestUsers.Items.Remove(this.gvTestUsers.SelectedItems[0]);
      this.refreshUserCount();
    }

    private bool isTestUser(UserInfoSummary user)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvTestUsers.Items)
      {
        if (gvItem.Tag.Equals((object) user))
          return true;
      }
      return false;
    }

    private void dlgButtons_OK(object sender, EventArgs e)
    {
      List<string> stringList = new List<string>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvTestUsers.Items)
      {
        UserInfoSummary tag = (UserInfoSummary) gvItem.Tag;
        stringList.Add(tag.UserID);
      }
      this.versionGroup.GroupUserIDs = stringList.ToArray();
      Session.ServerManager.UpdateVersionManagementGroup(this.versionGroup);
      this.DialogResult = DialogResult.OK;
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      this.dlgButtons = new DialogButtons();
      this.label1 = new Label();
      this.grpUsers = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnRemoveUsers = new StandardIconButton();
      this.btnAddUsers = new StandardIconButton();
      this.gvTestUsers = new GridView();
      this.toolTip1 = new ToolTip(this.components);
      this.grpUsers.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnRemoveUsers).BeginInit();
      ((ISupportInitialize) this.btnAddUsers).BeginInit();
      this.SuspendLayout();
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 228);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(382, 44);
      this.dlgButtons.TabIndex = 1;
      this.dlgButtons.OK += new EventHandler(this.dlgButtons_OK);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(316, 14);
      this.label1.TabIndex = 2;
      this.label1.Text = "The following users will receive updates marked as \"In Testing.\"";
      this.grpUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpUsers.Controls.Add((Control) this.flowLayoutPanel1);
      this.grpUsers.Controls.Add((Control) this.gvTestUsers);
      this.grpUsers.HeaderForeColor = SystemColors.ControlText;
      this.grpUsers.Location = new Point(10, 32);
      this.grpUsers.Name = "grpUsers";
      this.grpUsers.Size = new Size(361, 196);
      this.grpUsers.TabIndex = 3;
      this.grpUsers.Text = "Test Users";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnRemoveUsers);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAddUsers);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(295, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(66, 22);
      this.flowLayoutPanel1.TabIndex = 1;
      this.btnRemoveUsers.BackColor = Color.Transparent;
      this.btnRemoveUsers.Enabled = false;
      this.btnRemoveUsers.Location = new Point(45, 3);
      this.btnRemoveUsers.Margin = new Padding(0, 3, 5, 3);
      this.btnRemoveUsers.MouseDownImage = (Image) null;
      this.btnRemoveUsers.Name = "btnRemoveUsers";
      this.btnRemoveUsers.Size = new Size(16, 16);
      this.btnRemoveUsers.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemoveUsers.TabIndex = 0;
      this.btnRemoveUsers.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemoveUsers, "Remove Users");
      this.btnRemoveUsers.Click += new EventHandler(this.btnRemoveUsers_Click);
      this.btnAddUsers.BackColor = Color.Transparent;
      this.btnAddUsers.Location = new Point(24, 3);
      this.btnAddUsers.Margin = new Padding(0, 3, 5, 3);
      this.btnAddUsers.MouseDownImage = (Image) null;
      this.btnAddUsers.Name = "btnAddUsers";
      this.btnAddUsers.Size = new Size(16, 16);
      this.btnAddUsers.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddUsers.TabIndex = 1;
      this.btnAddUsers.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddUsers, "Add Users");
      this.btnAddUsers.Click += new EventHandler(this.btnAddUsers_Click);
      this.gvTestUsers.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "User";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "First Name";
      gvColumn2.Width = 120;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Last Name";
      gvColumn3.Width = 120;
      this.gvTestUsers.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvTestUsers.Dock = DockStyle.Fill;
      this.gvTestUsers.Location = new Point(1, 26);
      this.gvTestUsers.Name = "gvTestUsers";
      this.gvTestUsers.Size = new Size(359, 169);
      this.gvTestUsers.TabIndex = 0;
      this.gvTestUsers.SelectedIndexChanged += new EventHandler(this.gvTestUsers_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(382, 272);
      this.Controls.Add((Control) this.grpUsers);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.dlgButtons);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (VersionTestUserDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass Test Users";
      this.grpUsers.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemoveUsers).EndInit();
      ((ISupportInitialize) this.btnAddUsers).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
