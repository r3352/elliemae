// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.ePass.AddFileContactDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery.ePass
{
  public class AddFileContactDialog : Form
  {
    public Dictionary<string, string> contactDetails;
    private IContainer components;
    private Label lblCategory;
    private TextBox txtContactName;
    private Label lblContactName;
    private TextBox txtEmailAddress;
    private Label lblEmail;
    private ComboBox cmbContactTypes;
    private Button btnSave;
    private Button btnCancel;
    private TextBox txtAuthorizationCode;
    private Label lblAuthorizationCode;

    public AddFileContactDialog(List<string> ContactTypes)
    {
      this.InitializeComponent();
      this.cmbContactTypes.DataSource = (object) new BindingSource()
      {
        DataSource = (object) ContactTypes
      };
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.txtContactName.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter the value for contact name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtContactName.Focus();
      }
      else if (string.IsNullOrEmpty(this.txtEmailAddress.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter the value for Email Address.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtEmailAddress.Focus();
      }
      else if (!Utils.ValidateEmail(this.txtEmailAddress.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The e-mail address format is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtEmailAddress.Focus();
      }
      else if (string.IsNullOrEmpty(this.txtAuthorizationCode.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter the value for Authentication Code.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtAuthorizationCode.Focus();
      }
      else if (!this.ValidateAuthenticationCode(this.txtAuthorizationCode.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter the valid value for Authentication Code.Minimum 4 and Maximum 10 numbers required.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtAuthorizationCode.Focus();
      }
      else
      {
        this.contactDetails = new Dictionary<string, string>();
        this.contactDetails.Add("ContactType", this.cmbContactTypes.SelectedValue.ToString());
        this.contactDetails.Add("Name", this.txtContactName.Text);
        this.contactDetails.Add("Email", this.txtEmailAddress.Text);
        this.contactDetails.Add("AuthCode", this.txtAuthorizationCode.Text);
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private bool ValidateAuthenticationCode(string authenticationCode)
    {
      return authenticationCode.Length >= 4 && authenticationCode.Length <= 10 && int.TryParse(authenticationCode, out int _) && authenticationCode.All<char>(new Func<char, bool>(char.IsDigit));
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblCategory = new Label();
      this.txtContactName = new TextBox();
      this.lblContactName = new Label();
      this.txtEmailAddress = new TextBox();
      this.lblEmail = new Label();
      this.cmbContactTypes = new ComboBox();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.txtAuthorizationCode = new TextBox();
      this.lblAuthorizationCode = new Label();
      this.SuspendLayout();
      this.lblCategory.AutoSize = true;
      this.lblCategory.Location = new Point(43, 30);
      this.lblCategory.Name = "lblCategory";
      this.lblCategory.Size = new Size(65, 17);
      this.lblCategory.TabIndex = 0;
      this.lblCategory.Text = "Category";
      this.txtContactName.Location = new Point(193, 82);
      this.txtContactName.Name = "txtContactName";
      this.txtContactName.Size = new Size(142, 22);
      this.txtContactName.TabIndex = 3;
      this.lblContactName.AutoSize = true;
      this.lblContactName.Location = new Point(43, 85);
      this.lblContactName.Name = "lblContactName";
      this.lblContactName.Size = new Size(97, 17);
      this.lblContactName.TabIndex = 2;
      this.lblContactName.Text = "Contact Name";
      this.txtEmailAddress.Location = new Point(193, 138);
      this.txtEmailAddress.Name = "txtEmailAddress";
      this.txtEmailAddress.Size = new Size(142, 22);
      this.txtEmailAddress.TabIndex = 5;
      this.lblEmail.AutoSize = true;
      this.lblEmail.Location = new Point(43, 143);
      this.lblEmail.Name = "lblEmail";
      this.lblEmail.Size = new Size(98, 17);
      this.lblEmail.TabIndex = 4;
      this.lblEmail.Text = "Email Address";
      this.cmbContactTypes.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbContactTypes.FormattingEnabled = true;
      this.cmbContactTypes.Location = new Point(193, 23);
      this.cmbContactTypes.Name = "cmbContactTypes";
      this.cmbContactTypes.Size = new Size(142, 24);
      this.cmbContactTypes.TabIndex = 1;
      this.btnSave.Location = new Point(77, 252);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 8;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.Location = new Point(210, 252);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.txtAuthorizationCode.Location = new Point(193, 187);
      this.txtAuthorizationCode.Name = "txtAuthorizationCode";
      this.txtAuthorizationCode.Size = new Size(142, 22);
      this.txtAuthorizationCode.TabIndex = 7;
      this.lblAuthorizationCode.AutoSize = true;
      this.lblAuthorizationCode.Location = new Point(43, 192);
      this.lblAuthorizationCode.Name = "lblAuthorizationCode";
      this.lblAuthorizationCode.Size = new Size(128, 17);
      this.lblAuthorizationCode.TabIndex = 9;
      this.lblAuthorizationCode.Text = "Authorization Code";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.ControlLightLight;
      this.ClientSize = new Size(374, 318);
      this.Controls.Add((Control) this.txtAuthorizationCode);
      this.Controls.Add((Control) this.lblAuthorizationCode);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.cmbContactTypes);
      this.Controls.Add((Control) this.txtEmailAddress);
      this.Controls.Add((Control) this.lblEmail);
      this.Controls.Add((Control) this.txtContactName);
      this.Controls.Add((Control) this.lblContactName);
      this.Controls.Add((Control) this.lblCategory);
      this.Name = nameof (AddFileContactDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add a File Contact";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
