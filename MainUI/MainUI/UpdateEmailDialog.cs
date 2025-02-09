// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.UpdateEmailDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class UpdateEmailDialog : Form
  {
    private Button btnCancel;
    private Button btnOK;
    private Label label1;
    private TextBox emailBox;
    private Label label2;
    private GroupBox groupBox1;
    private TextBox firstNameBox;
    private Label label3;
    private Label label4;
    private TextBox lastNameBox;
    private System.ComponentModel.Container components;
    private string firstName = "";
    private string lastName = "";
    private string userEmail = "";

    public UpdateEmailDialog(string firstName, string lastName, string email)
    {
      this.InitializeComponent();
      this.firstNameBox.Text = firstName;
      this.lastNameBox.Text = lastName;
      this.emailBox.Text = email;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public string FirstName => this.firstName;

    public string LastName => this.lastName;

    public string UserEmail => this.userEmail;

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.emailBox = new TextBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.groupBox1 = new GroupBox();
      this.lastNameBox = new TextBox();
      this.firstNameBox = new TextBox();
      this.label4 = new Label();
      this.label3 = new Label();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(232, 168);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(104, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "&Remind Me Later";
      this.btnOK.Location = new Point(160, 168);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(64, 23);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.emailBox.Location = new Point(80, 112);
      this.emailBox.Name = "emailBox";
      this.emailBox.Size = new Size(236, 20);
      this.emailBox.TabIndex = 3;
      this.emailBox.Text = "";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(18, 116);
      this.label1.Name = "label1";
      this.label1.Size = new Size(36, 16);
      this.label1.TabIndex = 15;
      this.label1.Text = "Email:";
      this.label2.Location = new Point(8, 12);
      this.label2.Name = "label2";
      this.label2.Size = new Size(316, 28);
      this.label2.TabIndex = 16;
      this.label2.Text = "Your user's name and email address are required in order to take full advantage of Encompass features.";
      this.groupBox1.Controls.Add((Control) this.lastNameBox);
      this.groupBox1.Controls.Add((Control) this.firstNameBox);
      this.groupBox1.Controls.Add((Control) this.emailBox);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Location = new Point(8, 8);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(328, 148);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.lastNameBox.Location = new Point(80, 84);
      this.lastNameBox.Name = "lastNameBox";
      this.lastNameBox.Size = new Size(144, 20);
      this.lastNameBox.TabIndex = 2;
      this.lastNameBox.Text = "";
      this.firstNameBox.Location = new Point(80, 56);
      this.firstNameBox.Name = "firstNameBox";
      this.firstNameBox.Size = new Size(144, 20);
      this.firstNameBox.TabIndex = 1;
      this.firstNameBox.Text = "";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(18, 88);
      this.label4.Name = "label4";
      this.label4.Size = new Size(62, 16);
      this.label4.TabIndex = 21;
      this.label4.Text = "Last Name:";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(18, 60);
      this.label3.Name = "label3";
      this.label3.Size = new Size(63, 16);
      this.label3.TabIndex = 20;
      this.label3.Text = "First Name:";
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(346, 203);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (UpdateEmailDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Update User Profile";
      this.groupBox1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.firstName = this.firstNameBox.Text.Trim();
      if (this.firstName == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter both a user First name and Last name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.firstNameBox.Focus();
      }
      else
      {
        this.lastName = this.lastNameBox.Text.Trim();
        if (this.lastName == "")
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must enter both a user First name and Last name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.lastNameBox.Focus();
        }
        else
        {
          this.userEmail = this.emailBox.Text.Trim();
          if (this.userEmail == "")
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Many Encompass features require an email address. Please enter one at this time.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            this.emailBox.Focus();
          }
          else if (!Utils.ValidateEmail(this.userEmail))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The e-mail address format is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            this.emailBox.Focus();
          }
          else
            this.DialogResult = DialogResult.OK;
        }
      }
    }
  }
}
