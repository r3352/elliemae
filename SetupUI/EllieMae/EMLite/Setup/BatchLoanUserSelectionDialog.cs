// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.BatchLoanUserSelectionDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class BatchLoanUserSelectionDialog : Form
  {
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private ListView listvwUser;
    private Button btnSubmit;
    private Button btnCancel;
    private ListViewSortManager sortMgr;
    private System.ComponentModel.Container components;
    private UserInfo[] userInfoArray;
    private IOrganizationManager iorgMgrObj = Session.OrganizationManager;
    private UserInfo selecteduser;

    public BatchLoanUserSelectionDialog()
    {
      this.InitializeComponent();
      this.loadUsers();
    }

    public UserInfo SelectedUser => this.selecteduser;

    private void loadUsers()
    {
      this.listvwUser.Items.Clear();
      this.userInfoArray = this.iorgMgrObj.GetAllAccessibleUsers();
      if (this.userInfoArray == null)
        return;
      this.sortMgr = new ListViewSortManager(this.listvwUser, new System.Type[3]
      {
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort)
      });
      this.sortMgr.Sort(0);
      this.sortMgr.Disable();
      this.listvwUser.BeginUpdate();
      for (int index = 0; index < this.userInfoArray.Length; ++index)
        this.listvwUser.Items.Add(new ListViewItem(this.userInfoArray[index].Userid)
        {
          Tag = (object) this.userInfoArray[index],
          SubItems = {
            this.userInfoArray[index].FirstName,
            this.userInfoArray[index].LastName
          }
        });
      this.sortMgr.Enable();
      this.listvwUser.EndUpdate();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.listvwUser = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.btnSubmit = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.listvwUser.AllowColumnReorder = true;
      this.listvwUser.Columns.AddRange(new ColumnHeader[3]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3
      });
      this.listvwUser.FullRowSelect = true;
      this.listvwUser.GridLines = true;
      this.listvwUser.Location = new Point(4, 4);
      this.listvwUser.MultiSelect = false;
      this.listvwUser.Name = "listvwUser";
      this.listvwUser.Size = new Size(305, 336);
      this.listvwUser.TabIndex = 1;
      this.listvwUser.View = View.Details;
      this.listvwUser.DoubleClick += new EventHandler(this.listvwUser_DoubleClick);
      this.columnHeader1.Text = "User ID";
      this.columnHeader1.Width = 80;
      this.columnHeader2.Text = "First Name";
      this.columnHeader2.Width = 100;
      this.columnHeader3.Text = "Last Name";
      this.columnHeader3.Width = 120;
      this.btnSubmit.Location = new Point(155, 344);
      this.btnSubmit.Name = "btnSubmit";
      this.btnSubmit.Size = new Size(76, 23);
      this.btnSubmit.TabIndex = 2;
      this.btnSubmit.Text = "OK";
      this.btnSubmit.Click += new EventHandler(this.btnSubmit_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(235, 344);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(76, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(314, 372);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSubmit);
      this.Controls.Add((Control) this.listvwUser);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (BatchLoanUserSelectionDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "User Selection Dialog";
      this.ResumeLayout(false);
    }

    private void btnSubmit_Click(object sender, EventArgs e)
    {
      if (this.listvwUser.SelectedItems == null || this.listvwUser.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a person.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.selecteduser = (UserInfo) this.listvwUser.SelectedItems[0].Tag;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void listvwUser_DoubleClick(object sender, EventArgs e)
    {
      this.btnSubmit_Click((object) null, (EventArgs) null);
    }
  }
}
