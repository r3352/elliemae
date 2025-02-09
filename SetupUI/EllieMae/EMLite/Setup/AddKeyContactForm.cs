// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddKeyContactForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddKeyContactForm : Form
  {
    private int externalOrgID = -1;
    private ExternalOrgContact externalOrgContact;
    private Sessions.Session session;
    private IContainer components;
    private Button btnCancel;
    private Button btnOK;
    private Label label1;
    private TextBox txtName;
    private Label label2;
    private TextBox txtTitle;
    private Label label3;
    private TextBox txtPhone;
    private Label label4;
    private TextBox txtEmail;
    private GroupContainer groupContainer2;
    private Label label5;
    private Label label6;

    public AddKeyContactForm(
      Sessions.Session session,
      int externalOrgID,
      ExternalOrgContact externalOrgContact = null)
    {
      this.InitializeComponent();
      this.externalOrgID = externalOrgID;
      this.session = session;
      this.externalOrgContact = externalOrgContact;
      if (this.externalOrgContact != null)
      {
        this.Text = "Edit TPO Key Contact";
        this.label5.Text = "Enter the key contact’s information in the fields below.";
      }
      this.initialPageValue();
      this.initFieldEvents();
    }

    public ExternalOrgContact ExternalOrgContact => this.externalOrgContact;

    private void populateExternalOrgContactObj()
    {
      int num = this.externalOrgContact == null ? -1 : this.externalOrgContact.ExternalOrgContactID;
      this.externalOrgContact = new ExternalOrgContact()
      {
        ExternalOrgContactID = num,
        ExternalOrgID = this.externalOrgID,
        Name = this.txtName.Text,
        Title = this.txtTitle.Text,
        Phone = this.txtPhone.Text,
        Email = this.txtEmail.Text,
        Type = ExternalOriginatorContactType.FreeEntry
      };
    }

    private void initFieldEvents() => new InputEventHelper().AddPhoneFieldHelper(this.txtPhone);

    private void initialPageValue()
    {
      if (this.externalOrgContact == null)
        return;
      this.txtName.Text = this.externalOrgContact.Name;
      this.txtTitle.Text = this.externalOrgContact.Title;
      this.txtPhone.Text = this.externalOrgContact.Phone;
      this.txtEmail.Text = this.externalOrgContact.Email;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.txtName.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter the key contact’s name.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.txtName.Focus();
      }
      else if (this.txtEmail.Text.Trim() != string.Empty && !Utils.ValidateEmail(this.txtEmail.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The email address format is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtEmail.Focus();
      }
      else
      {
        this.populateExternalOrgContactObj();
        this.DialogResult = DialogResult.OK;
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
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.label1 = new Label();
      this.txtName = new TextBox();
      this.label2 = new Label();
      this.txtTitle = new TextBox();
      this.label3 = new Label();
      this.txtPhone = new TextBox();
      this.label4 = new Label();
      this.txtEmail = new TextBox();
      this.groupContainer2 = new GroupContainer();
      this.label6 = new Label();
      this.label5 = new Label();
      this.groupContainer2.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(398, 174);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Location = new Point(300, 174);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 36);
      this.label1.Name = "label1";
      this.label1.Size = new Size(35, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Name";
      this.txtName.Location = new Point(90, 33);
      this.txtName.MaxLength = 64;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(371, 20);
      this.txtName.TabIndex = 1;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 59);
      this.label2.Name = "label2";
      this.label2.Size = new Size(27, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Title";
      this.txtTitle.Location = new Point(90, 56);
      this.txtTitle.MaxLength = 30;
      this.txtTitle.Name = "txtTitle";
      this.txtTitle.Size = new Size(371, 20);
      this.txtTitle.TabIndex = 3;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(6, 82);
      this.label3.Name = "label3";
      this.label3.Size = new Size(78, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Phone Number";
      this.txtPhone.Location = new Point(90, 79);
      this.txtPhone.Name = "txtPhone";
      this.txtPhone.Size = new Size(371, 20);
      this.txtPhone.TabIndex = 5;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 105);
      this.label4.Name = "label4";
      this.label4.Size = new Size(32, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Email";
      this.txtEmail.Location = new Point(90, 102);
      this.txtEmail.MaxLength = 64;
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.Size = new Size(371, 20);
      this.txtEmail.TabIndex = 7;
      this.groupContainer2.Controls.Add((Control) this.label6);
      this.groupContainer2.Controls.Add((Control) this.txtEmail);
      this.groupContainer2.Controls.Add((Control) this.label4);
      this.groupContainer2.Controls.Add((Control) this.txtPhone);
      this.groupContainer2.Controls.Add((Control) this.label3);
      this.groupContainer2.Controls.Add((Control) this.txtTitle);
      this.groupContainer2.Controls.Add((Control) this.label2);
      this.groupContainer2.Controls.Add((Control) this.txtName);
      this.groupContainer2.Controls.Add((Control) this.label1);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(12, 25);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(476, 134);
      this.groupContainer2.TabIndex = 3;
      this.groupContainer2.Text = "Contact Information";
      this.label6.AutoSize = true;
      this.label6.ForeColor = Color.Red;
      this.label6.Location = new Point(39, 36);
      this.label6.Name = "label6";
      this.label6.Size = new Size(11, 13);
      this.label6.TabIndex = 8;
      this.label6.Text = "*";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(17, 6);
      this.label5.Name = "label5";
      this.label5.Size = new Size(261, 13);
      this.label5.TabIndex = 4;
      this.label5.Text = "Enter the key contact’s information in the fields below.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(509, 218);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.groupContainer2);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddKeyContactForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add TPO Key Contact";
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
