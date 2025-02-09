// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DeleteUserDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class DeleteUserDialog : Form
  {
    private Panel panel1;
    private RadioButton rBtnTake;
    private RadioButton rBtnReassign;
    private RadioButton rBtnDelete;
    private Label label1;
    private Button btnReassign;
    private Button btnOK;
    private Button btnCancel;
    private System.ComponentModel.Container components;
    private TextBox txtBoxAssignee;
    private string _IdOfUserToDelete = string.Empty;
    private Sessions.Session session;

    public DeleteUserDialog(Sessions.Session session, string idOfUserToDelete)
    {
      this.session = session;
      this.InitializeComponent();
      this._IdOfUserToDelete = idOfUserToDelete;
      this.rBtnTake.Checked = true;
      this.btnReassign.Enabled = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.txtBoxAssignee = new TextBox();
      this.btnReassign = new Button();
      this.rBtnDelete = new RadioButton();
      this.rBtnReassign = new RadioButton();
      this.rBtnTake = new RadioButton();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.label1 = new Label();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.panel1.BorderStyle = BorderStyle.Fixed3D;
      this.panel1.Controls.Add((Control) this.txtBoxAssignee);
      this.panel1.Controls.Add((Control) this.btnReassign);
      this.panel1.Controls.Add((Control) this.rBtnDelete);
      this.panel1.Controls.Add((Control) this.rBtnReassign);
      this.panel1.Controls.Add((Control) this.rBtnTake);
      this.panel1.Location = new Point(16, 60);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(292, 112);
      this.panel1.TabIndex = 0;
      this.txtBoxAssignee.Location = new Point(104, 44);
      this.txtBoxAssignee.Name = "txtBoxAssignee";
      this.txtBoxAssignee.ReadOnly = true;
      this.txtBoxAssignee.Size = new Size(132, 20);
      this.txtBoxAssignee.TabIndex = 4;
      this.txtBoxAssignee.Text = "";
      this.btnReassign.Location = new Point(244, 44);
      this.btnReassign.Name = "btnReassign";
      this.btnReassign.Size = new Size(32, 24);
      this.btnReassign.TabIndex = 3;
      this.btnReassign.Text = "...";
      this.btnReassign.Click += new EventHandler(this.btnReassign_Click);
      this.rBtnDelete.Location = new Point(12, 76);
      this.rBtnDelete.Name = "rBtnDelete";
      this.rBtnDelete.Size = new Size(184, 20);
      this.rBtnDelete.TabIndex = 2;
      this.rBtnDelete.Text = "Delete";
      this.rBtnDelete.CheckedChanged += new EventHandler(this.rBtn_CheckedChanged);
      this.rBtnReassign.Location = new Point(12, 40);
      this.rBtnReassign.Name = "rBtnReassign";
      this.rBtnReassign.Size = new Size(88, 28);
      this.rBtnReassign.TabIndex = 1;
      this.rBtnReassign.Text = "Reassign to ";
      this.rBtnReassign.CheckedChanged += new EventHandler(this.rBtn_CheckedChanged);
      this.rBtnTake.Location = new Point(12, 12);
      this.rBtnTake.Name = "rBtnTake";
      this.rBtnTake.Size = new Size(188, 20);
      this.rBtnTake.TabIndex = 0;
      this.rBtnTake.Text = "Take ownership";
      this.rBtnTake.CheckedChanged += new EventHandler(this.rBtn_CheckedChanged);
      this.btnOK.Location = new Point(164, 184);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(68, 24);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(240, 184);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(68, 24);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.label1.Location = new Point(16, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(292, 36);
      this.label1.TabIndex = 3;
      this.label1.Text = "What would you like to do with the contacts owned by the user you are going to delete?";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(322, 223);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DeleteUserDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Delete User";
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void btnReassign_Click(object sender, EventArgs e)
    {
      using (ContactAssignment contactAssignment = new ContactAssignment(this.session, AclFeature.Cnt_Borrower_CreateNew, this._IdOfUserToDelete))
      {
        if (contactAssignment.ShowDialog() == DialogResult.Cancel)
          return;
        this.txtBoxAssignee.Text = contactAssignment.AssigneeID;
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      string str = string.Empty;
      if (this.rBtnReassign.Checked)
      {
        if (this.txtBoxAssignee.Text == string.Empty)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You selected to reassign contacts owned by " + this.session.OrganizationManager.GetUser(this._IdOfUserToDelete).FullName + " to someone else. Please select the user you want to assign the contacts to.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        str = this.txtBoxAssignee.Text;
      }
      if (this.rBtnTake.Checked)
        str = this.session.UserID;
      this.session.OrganizationManager.GetUser(this._IdOfUserToDelete);
      if (str != string.Empty)
      {
        foreach (BorrowerInfo info in this.session.ContactManager.GetBorrowersByOwner(this._IdOfUserToDelete))
        {
          info.OwnerID = str;
          this.session.ContactManager.UpdateBorrower(info);
        }
        foreach (BizPartnerInfo info in this.session.ContactManager.GetBizPartnersByOwner(this._IdOfUserToDelete))
        {
          info.OwnerID = str;
          this.session.ContactManager.UpdateBizPartner(info);
        }
      }
      else if (this.rBtnDelete.Checked)
      {
        foreach (BorrowerInfo borrowerInfo in this.session.ContactManager.GetBorrowersByOwner(this._IdOfUserToDelete))
          this.session.ContactManager.DeleteBorrower(borrowerInfo.ContactID);
        foreach (BizPartnerInfo info in this.session.ContactManager.GetBizPartnersByOwner(this._IdOfUserToDelete))
        {
          if (info.AccessLevel == ContactAccess.Public)
          {
            info.OwnerID = this.session.UserID;
            this.session.ContactManager.UpdateBizPartner(info);
          }
          else
            this.session.ContactManager.DeleteBizPartner(info.ContactID);
        }
      }
      this.DialogResult = DialogResult.OK;
    }

    private void rBtn_CheckedChanged(object sender, EventArgs e)
    {
      if (this.rBtnReassign.Checked)
        this.btnReassign.Enabled = true;
      else
        this.btnReassign.Enabled = false;
    }
  }
}
