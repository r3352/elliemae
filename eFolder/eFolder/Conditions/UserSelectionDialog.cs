// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.UserSelectionDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  public class UserSelectionDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private UserInfo user;
    private IContainer components;
    private Label lblRole;
    private ComboBox cboRole;
    private GridView gvUsers;
    private Button btnCancel;
    private Button btnOK;
    private ToolTip tooltip;

    public UserSelectionDialog(LoanDataMgr loanDataMgr, int roleID)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.loadRoleList(roleID);
    }

    public UserInfo User => this.user;

    private void loadRoleList(int roleID)
    {
      this.cboRole.Items.AddRange((object[]) this.loanDataMgr.SystemConfiguration.AllRoles);
      foreach (RoleInfo roleInfo in this.cboRole.Items)
      {
        if (roleInfo.RoleID == roleID)
          this.cboRole.SelectedItem = (object) roleInfo;
      }
      if (this.cboRole.SelectedItem != null)
        return;
      this.cboRole.SelectedIndex = 0;
    }

    private void cboRole_SelectedIndexChanged(object sender, EventArgs e)
    {
      RoleInfo selectedItem = (RoleInfo) this.cboRole.SelectedItem;
      if (selectedItem == null)
        return;
      this.loadUserList(selectedItem);
    }

    private void loadUserList(RoleInfo role)
    {
      this.gvUsers.Items.Clear();
      foreach (UserInfo userInfo in Session.OrganizationManager.GetScopedUsersWithRole(role.RoleID))
      {
        GVItem gvItem = this.gvUsers.Items.Add(userInfo.Userid);
        gvItem.SubItems[1].Value = (object) userInfo.LastName;
        gvItem.SubItems[2].Value = (object) userInfo.FirstName;
        gvItem.Tag = (object) userInfo;
      }
      this.gvUsers.ReSort();
    }

    private void gvUsers_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.btnOK_Click(source, EventArgs.Empty);
    }

    private void gvUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnOK.Enabled = this.gvUsers.SelectedItems.Count > 0;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.gvUsers.SelectedItems.Count == 0)
        return;
      this.user = (UserInfo) this.gvUsers.SelectedItems[0].Tag;
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
      this.lblRole = new Label();
      this.cboRole = new ComboBox();
      this.gvUsers = new GridView();
      this.tooltip = new ToolTip(this.components);
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.SuspendLayout();
      this.lblRole.AutoSize = true;
      this.lblRole.Location = new Point(12, 12);
      this.lblRole.Name = "lblRole";
      this.lblRole.Size = new Size(28, 14);
      this.lblRole.TabIndex = 0;
      this.lblRole.Text = "Role";
      this.cboRole.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRole.FormattingEnabled = true;
      this.cboRole.Location = new Point(44, 8);
      this.cboRole.Name = "cboRole";
      this.cboRole.Size = new Size(291, 22);
      this.cboRole.TabIndex = 1;
      this.cboRole.SelectedIndexChanged += new EventHandler(this.cboRole_SelectedIndexChanged);
      this.gvUsers.AllowMultiselect = false;
      this.gvUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colUserID";
      gvColumn1.Text = "User ID";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colLastName";
      gvColumn2.Text = "Last Name";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colFirstName";
      gvColumn3.Text = "First Name";
      gvColumn3.Width = 100;
      this.gvUsers.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvUsers.HoverToolTip = this.tooltip;
      this.gvUsers.Location = new Point(8, 36);
      this.gvUsers.Name = "gvUsers";
      this.gvUsers.Size = new Size(327, 292);
      this.gvUsers.TabIndex = 2;
      this.gvUsers.SelectedIndexChanged += new EventHandler(this.gvUsers_SelectedIndexChanged);
      this.gvUsers.ItemDoubleClick += new GVItemEventHandler(this.gvUsers_ItemDoubleClick);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(261, 336);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Enabled = false;
      this.btnOK.Location = new Point(185, 336);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(343, 366);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.gvUsers);
      this.Controls.Add((Control) this.cboRole);
      this.Controls.Add((Control) this.lblRole);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (UserSelectionDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select User";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
