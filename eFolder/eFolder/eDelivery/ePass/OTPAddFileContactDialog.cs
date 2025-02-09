// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.ePass.OTPAddFileContactDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery.ePass
{
  public class OTPAddFileContactDialog : Form
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
    private TextBox txtPhoneNumber;
    private Label lblPhoneNumber;

    public OTPAddFileContactDialog(List<string> ContactTypes)
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
      else if (string.IsNullOrEmpty(this.txtPhoneNumber.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter the cell phone number.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtPhoneNumber.Focus();
      }
      else if (!this.ValidateMobileNumber(this.txtPhoneNumber.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Plese enter 10 digit cell phone number.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtPhoneNumber.Focus();
      }
      else
      {
        this.contactDetails = new Dictionary<string, string>();
        this.contactDetails.Add("ContactType", this.cmbContactTypes.SelectedValue.ToString());
        this.contactDetails.Add("Name", this.txtContactName.Text);
        this.contactDetails.Add("Email", this.txtEmailAddress.Text);
        this.contactDetails.Add("CellPhoneNumber", this.txtPhoneNumber.Text);
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private bool ValidateMobileNumber(string mobileNumber) => mobileNumber.Length >= 12;

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void phoneField_KeyUp(object sender, KeyPressEventArgs e)
    {
      if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
        e.Handled = true;
      else
        this.formatFieldValue(sender, FieldFormat.PHONE);
    }

    private void formatFieldValue(object sender, FieldFormat format)
    {
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, format, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

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
      this.txtPhoneNumber = new TextBox();
      this.lblPhoneNumber = new Label();
      this.SuspendLayout();
      this.lblCategory.AutoSize = true;
      this.lblCategory.Location = new Point(32, 24);
      this.lblCategory.Margin = new Padding(2, 0, 2, 0);
      this.lblCategory.Name = "lblCategory";
      this.lblCategory.Size = new Size(49, 13);
      this.lblCategory.TabIndex = 0;
      this.lblCategory.Text = "Category";
      this.txtContactName.Location = new Point(145, 67);
      this.txtContactName.Margin = new Padding(2, 2, 2, 2);
      this.txtContactName.Name = "txtContactName";
      this.txtContactName.Size = new Size(108, 20);
      this.txtContactName.TabIndex = 3;
      this.lblContactName.AutoSize = true;
      this.lblContactName.Location = new Point(32, 69);
      this.lblContactName.Margin = new Padding(2, 0, 2, 0);
      this.lblContactName.Name = "lblContactName";
      this.lblContactName.Size = new Size(75, 13);
      this.lblContactName.TabIndex = 2;
      this.lblContactName.Text = "Contact Name";
      this.txtEmailAddress.Location = new Point(145, 112);
      this.txtEmailAddress.Margin = new Padding(2, 2, 2, 2);
      this.txtEmailAddress.Name = "txtEmailAddress";
      this.txtEmailAddress.Size = new Size(108, 20);
      this.txtEmailAddress.TabIndex = 5;
      this.lblEmail.AutoSize = true;
      this.lblEmail.Location = new Point(32, 116);
      this.lblEmail.Margin = new Padding(2, 0, 2, 0);
      this.lblEmail.Name = "lblEmail";
      this.lblEmail.Size = new Size(73, 13);
      this.lblEmail.TabIndex = 4;
      this.lblEmail.Text = "Email Address";
      this.cmbContactTypes.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbContactTypes.FormattingEnabled = true;
      this.cmbContactTypes.Location = new Point(145, 19);
      this.cmbContactTypes.Margin = new Padding(2, 2, 2, 2);
      this.cmbContactTypes.Name = "cmbContactTypes";
      this.cmbContactTypes.Size = new Size(108, 21);
      this.cmbContactTypes.TabIndex = 1;
      this.btnSave.Location = new Point(58, 205);
      this.btnSave.Margin = new Padding(2, 2, 2, 2);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(56, 19);
      this.btnSave.TabIndex = 8;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.Location = new Point(158, 205);
      this.btnCancel.Margin = new Padding(2, 2, 2, 2);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(56, 19);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.txtPhoneNumber.Location = new Point(145, 152);
      this.txtPhoneNumber.Margin = new Padding(2, 2, 2, 2);
      this.txtPhoneNumber.Name = "txtPhoneNumber";
      this.txtPhoneNumber.Size = new Size(108, 20);
      this.txtPhoneNumber.TabIndex = 7;
      this.txtPhoneNumber.MaxLength = 12;
      this.txtPhoneNumber.KeyPress += new KeyPressEventHandler(this.phoneField_KeyUp);
      this.lblPhoneNumber.AutoSize = true;
      this.lblPhoneNumber.Location = new Point(32, 156);
      this.lblPhoneNumber.Margin = new Padding(2, 0, 2, 0);
      this.lblPhoneNumber.Name = "lblPhoneNumber";
      this.lblPhoneNumber.Size = new Size(78, 13);
      this.lblPhoneNumber.TabIndex = 9;
      this.lblPhoneNumber.Text = "Cell Phone Number";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.ControlLightLight;
      this.ClientSize = new Size(280, 258);
      this.Controls.Add((Control) this.txtPhoneNumber);
      this.Controls.Add((Control) this.lblPhoneNumber);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.cmbContactTypes);
      this.Controls.Add((Control) this.txtEmailAddress);
      this.Controls.Add((Control) this.lblEmail);
      this.Controls.Add((Control) this.txtContactName);
      this.Controls.Add((Control) this.lblContactName);
      this.Controls.Add((Control) this.lblCategory);
      this.Margin = new Padding(2, 2, 2, 2);
      this.Name = nameof (OTPAddFileContactDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add a File Contact";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
