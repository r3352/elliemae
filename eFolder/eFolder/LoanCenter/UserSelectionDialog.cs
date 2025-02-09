// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.UserSelectionDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class UserSelectionDialog : Form
  {
    private GridViewReportFilterManager filterManager;
    private IContainer components;
    private GridView gvUsers;
    private Button btnCancel;
    private Button btnOK;
    private Panel panel1;
    private GroupContainer grpUsers;
    private Label lblNoUsers;

    public UserInfo SelectedUser => this.gvUsers.SelectedItems[0].Tag as UserInfo;

    public UserSelectionDialog()
    {
      this.InitializeComponent();
      this.initUserList();
      this.loadUserList();
    }

    private void filterManager_FilterChanged(object sender, EventArgs e)
    {
      this.filterManager.ApplyFilter();
    }

    private void gvUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnOK.Enabled = 0 < this.gvUsers.SelectedItems.Count;
    }

    private void initUserList()
    {
      this.gvUsers.SelectedIndexChanged += new EventHandler(this.gvUsers_SelectedIndexChanged);
      this.gvUsers.ItemDoubleClick += new GVItemEventHandler(this.gvUsers_ItemDoubleClick);
      this.gvUsers.FilterVisible = true;
      this.filterManager = new GridViewReportFilterManager(Session.DefaultInstance, this.gvUsers);
      this.filterManager.FilterChanged += new EventHandler(this.filterManager_FilterChanged);
      this.filterManager.CreateColumnFilter(0, FieldFormat.STRING);
      this.filterManager.CreateColumnFilter(1, FieldFormat.STRING);
      this.filterManager.CreateColumnFilter(2, FieldFormat.STRING);
    }

    private void gvUsers_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void loadUserList()
    {
      UserInfo[] scopedUsers = Session.OrganizationManager.GetScopedUsers();
      List<OrgInfo> list = ((IEnumerable<OrgInfo>) Session.OrganizationManager.GetAllOrganizations()).ToList<OrgInfo>();
      foreach (UserInfo userInfo in scopedUsers)
      {
        UserInfo user = userInfo;
        if (!string.IsNullOrEmpty(user.Email))
        {
          GVItem gvItem = new GVItem();
          gvItem.Tag = (object) user;
          gvItem.SubItems.Add((object) user.FullName);
          gvItem.SubItems.Add((object) user.Email);
          OrgInfo orgInfo = list.Find((Predicate<OrgInfo>) (x => x.Oid == user.OrgId));
          if (orgInfo != null)
            gvItem.SubItems.Add((object) orgInfo.OrgName);
          this.gvUsers.Items.Add(gvItem);
        }
      }
    }

    private void cancelBtn_Click(object sender, EventArgs e) => this.Close();

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (UserSelectionDialog));
      this.gvUsers = new GridView();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.panel1 = new Panel();
      this.grpUsers = new GroupContainer();
      this.lblNoUsers = new Label();
      this.panel1.SuspendLayout();
      this.grpUsers.SuspendLayout();
      this.SuspendLayout();
      this.gvUsers.AllowMultiselect = false;
      this.gvUsers.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Contact Name";
      gvColumn1.Text = "Contact Name";
      gvColumn1.Width = 130;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Email Address";
      gvColumn2.Text = "Email Address";
      gvColumn2.Width = 150;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Organization";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Organization";
      gvColumn3.Width = 165;
      this.gvUsers.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvUsers.Location = new Point(1, 26);
      this.gvUsers.Name = "gvUsers";
      this.gvUsers.Size = new Size(445, 324);
      this.gvUsers.TabIndex = 4;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(364, 359);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.cancelBtn_Click);
      this.btnOK.Enabled = false;
      this.btnOK.Location = new Point(283, 359);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel1.Controls.Add((Control) this.grpUsers);
      this.panel1.Controls.Add((Control) this.lblNoUsers);
      this.panel1.Location = new Point(1, -31);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(472, 430);
      this.panel1.TabIndex = 11;
      this.grpUsers.Controls.Add((Control) this.btnOK);
      this.grpUsers.Controls.Add((Control) this.btnCancel);
      this.grpUsers.Controls.Add((Control) this.gvUsers);
      this.grpUsers.HeaderForeColor = SystemColors.ControlText;
      this.grpUsers.Location = new Point(0, 32);
      this.grpUsers.Name = "grpUsers";
      this.grpUsers.Size = new Size(447, 395);
      this.grpUsers.TabIndex = 6;
      this.grpUsers.Text = "Select a User";
      this.lblNoUsers.Dock = DockStyle.Top;
      this.lblNoUsers.Location = new Point(0, 0);
      this.lblNoUsers.Name = "lblNoUsers";
      this.lblNoUsers.Size = new Size(472, 32);
      this.lblNoUsers.TabIndex = 9;
      this.lblNoUsers.Text = "There are no users or User Groups eligible for assignment to the selected Role.";
      this.lblNoUsers.Visible = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(446, 395);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = Resources.icon_allsizes_bug;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (UserSelectionDialog);
      this.Text = "Contacts";
      this.panel1.ResumeLayout(false);
      this.grpUsers.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
