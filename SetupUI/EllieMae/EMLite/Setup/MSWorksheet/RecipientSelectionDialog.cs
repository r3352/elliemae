// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MSWorksheet.RecipientSelectionDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.MSWorksheet
{
  public class RecipientSelectionDialog : Form
  {
    private AlertConfig currentConfig;
    private IContainer components;
    private DialogButtons dlgButtons;
    private GroupContainer groupContainer1;
    private GridView gvRoles;
    private GroupContainer groupContainer2;
    private Label label1;
    private FlowLayoutPanel flowLayoutPanel1;
    private FlowLayoutPanel flowLayoutPanel2;
    private GridView gvUsers;
    private StandardIconButton btnRemoveUser;
    private ToolTip toolTip1;
    private StandardIconButton btnAddUser;
    private GradientPanel gradientPanel1;

    public RecipientSelectionDialog(AlertConfig config)
    {
      this.currentConfig = config;
      this.InitializeComponent();
      this.loadRoles();
      this.loadConfig(config);
    }

    private void loadRoles()
    {
      RoleInfo[] allRoleFunctions = ((WorkflowManager) Session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
      this.gvRoles.Items.Add(new GVItem((object) RoleInfo.FileStarter)
      {
        Tag = (object) RoleInfo.FileStarter
      });
      foreach (RoleInfo roleInfo in allRoleFunctions)
        this.gvRoles.Items.Add(new GVItem(roleInfo.Name)
        {
          Tag = (object) roleInfo
        });
      this.gvRoles.Sort(0, SortOrder.Ascending);
    }

    private void loadConfig(AlertConfig config)
    {
      foreach (int notificationRole in config.NotificationRoleList)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvRoles.Items)
        {
          if (((RoleSummaryInfo) gvItem.Tag).RoleID == notificationRole)
          {
            gvItem.Checked = true;
            break;
          }
        }
      }
      if (config.NotificationUserList.Count > 0)
      {
        foreach (UserInfoSummary user in (IEnumerable) Session.OrganizationManager.GetUsers(config.NotificationUserList.ToArray(), true).Values)
          this.gvUsers.Items.Add(this.createGVItemForUser(user));
      }
      this.gvRoles.Sort(0, SortOrder.Ascending);
      this.gvUsers.Sort(0, SortOrder.Ascending);
    }

    private void btnAddUser_Click(object sender, EventArgs e)
    {
      using (UserPicker userPicker = new UserPicker(Session.OrganizationManager.GetAllUserInfoSummary(), true))
      {
        if (userPicker.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        foreach (UserInfoSummary selectedUserInfoSummary in userPicker.GetSelectedUserInfoSummaries())
        {
          if (!this.isUserInList(selectedUserInfoSummary))
            this.gvUsers.Items.Add(this.createGVItemForUser(selectedUserInfoSummary));
        }
        this.gvUsers.Sort(0, SortOrder.Ascending);
      }
    }

    private bool isUserInList(UserInfoSummary user)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvUsers.Items)
      {
        if (user.Equals(gvItem.Tag))
          return true;
      }
      return false;
    }

    private GVItem createGVItemForUser(UserInfoSummary user)
    {
      return new GVItem(user.FullName + " (" + user.UserID + ")")
      {
        Tag = (object) user
      };
    }

    private void dlgButtons_OK(object sender, EventArgs e)
    {
      GVItem[] checkedItems = this.gvRoles.GetCheckedItems(0);
      if (checkedItems.Length == 0 && this.gvUsers.Items.Count == 0 && Utils.Dialog((IWin32Window) this, "You have not selected any users or loan team members for this notification. Are you sure you want to proceed?", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.No)
        return;
      this.currentConfig.NotificationRoleList.Clear();
      foreach (GVItem gvItem in checkedItems)
        this.currentConfig.NotificationRoleList.Add(((RoleSummaryInfo) gvItem.Tag).RoleID);
      this.currentConfig.NotificationUserList.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvUsers.Items)
        this.currentConfig.NotificationUserList.Add(((UserInfoSummary) gvItem.Tag).UserID);
      this.DialogResult = DialogResult.OK;
    }

    private void btnRemoveUser_Click(object sender, EventArgs e)
    {
      if (this.gvUsers.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more users to be removed from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Remove the selected user(s) from the Notification list?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        while (this.gvUsers.SelectedItems.Count > 0)
          this.gvUsers.Items.Remove(this.gvUsers.SelectedItems[0]);
      }
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
      this.dlgButtons = new DialogButtons();
      this.groupContainer1 = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.gvRoles = new GridView();
      this.groupContainer2 = new GroupContainer();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.btnRemoveUser = new StandardIconButton();
      this.btnAddUser = new StandardIconButton();
      this.gvUsers = new GridView();
      this.label1 = new Label();
      this.toolTip1 = new ToolTip(this.components);
      this.gradientPanel1 = new GradientPanel();
      this.groupContainer1.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      ((ISupportInitialize) this.btnRemoveUser).BeginInit();
      ((ISupportInitialize) this.btnAddUser).BeginInit();
      this.gradientPanel1.SuspendLayout();
      this.SuspendLayout();
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 357);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(515, 40);
      this.dlgButtons.TabIndex = 0;
      this.dlgButtons.OK += new EventHandler(this.dlgButtons_OK);
      this.groupContainer1.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.flowLayoutPanel1);
      this.groupContainer1.Controls.Add((Control) this.gvRoles);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(10, 41);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(242, 311);
      this.groupContainer1.TabIndex = 1;
      this.groupContainer1.Text = "Loan Team Members";
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(135, 1);
      this.flowLayoutPanel1.Margin = new Padding(3, 3, 0, 3);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(102, 22);
      this.flowLayoutPanel1.TabIndex = 1;
      this.gvRoles.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Column";
      gvColumn1.Width = 240;
      this.gvRoles.Columns.AddRange(new GVColumn[1]
      {
        gvColumn1
      });
      this.gvRoles.Dock = DockStyle.Fill;
      this.gvRoles.HeaderHeight = 0;
      this.gvRoles.HeaderVisible = false;
      this.gvRoles.Location = new Point(1, 25);
      this.gvRoles.Name = "gvRoles";
      this.gvRoles.Selectable = false;
      this.gvRoles.Size = new Size(240, 285);
      this.gvRoles.TabIndex = 0;
      this.groupContainer2.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer2.Controls.Add((Control) this.flowLayoutPanel2);
      this.groupContainer2.Controls.Add((Control) this.gvUsers);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(262, 41);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(242, 311);
      this.groupContainer2.TabIndex = 2;
      this.groupContainer2.Text = "Users";
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.Controls.Add((Control) this.btnRemoveUser);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnAddUser);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(135, 1);
      this.flowLayoutPanel2.Margin = new Padding(3, 3, 0, 3);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(102, 22);
      this.flowLayoutPanel2.TabIndex = 2;
      this.btnRemoveUser.BackColor = Color.Transparent;
      this.btnRemoveUser.Location = new Point(86, 3);
      this.btnRemoveUser.Margin = new Padding(5, 3, 0, 3);
      this.btnRemoveUser.Name = "btnRemoveUser";
      this.btnRemoveUser.Size = new Size(16, 17);
      this.btnRemoveUser.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemoveUser.TabIndex = 0;
      this.btnRemoveUser.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemoveUser, "Remove User");
      this.btnRemoveUser.Click += new EventHandler(this.btnRemoveUser_Click);
      this.btnAddUser.BackColor = Color.Transparent;
      this.btnAddUser.Location = new Point(65, 3);
      this.btnAddUser.Margin = new Padding(5, 3, 0, 3);
      this.btnAddUser.Name = "btnAddUser";
      this.btnAddUser.Size = new Size(16, 17);
      this.btnAddUser.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddUser.TabIndex = 1;
      this.btnAddUser.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddUser, "Add User");
      this.btnAddUser.Click += new EventHandler(this.btnAddUser_Click);
      this.gvUsers.BorderStyle = BorderStyle.None;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column1";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Column";
      gvColumn2.Width = 240;
      this.gvUsers.Columns.AddRange(new GVColumn[1]
      {
        gvColumn2
      });
      this.gvUsers.Dock = DockStyle.Fill;
      this.gvUsers.HeaderHeight = 0;
      this.gvUsers.HeaderVisible = false;
      this.gvUsers.Location = new Point(1, 25);
      this.gvUsers.Name = "gvUsers";
      this.gvUsers.Size = new Size(240, 285);
      this.gvUsers.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(8, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(176, 14);
      this.label1.TabIndex = 3;
      this.label1.Text = "Select recipients of the notification.";
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(10, 10);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(494, 32);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel1.TabIndex = 4;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(515, 397);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Controls.Add((Control) this.groupContainer2);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.dlgButtons);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RecipientSelectionDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Notification Recipients";
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer2.ResumeLayout(false);
      this.flowLayoutPanel2.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemoveUser).EndInit();
      ((ISupportInitialize) this.btnAddUser).EndInit();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
