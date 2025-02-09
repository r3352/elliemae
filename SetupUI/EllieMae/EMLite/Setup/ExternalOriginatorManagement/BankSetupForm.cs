// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.BankSetupForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class BankSetupForm : Form
  {
    private Sessions.Session session;
    private bool edit;
    private int oid;
    private InputEventHelper inputEventHelper;
    private Dictionary<int, string> hierarchyNodes = new Dictionary<int, string>();
    private ExternalBank externalBank;
    private bool hasBankEditRight = true;
    private SessionObjects objs;
    private IContainer components;
    private Label name;
    private Label address;
    private Label city;
    private Label state;
    private Label zip;
    private Label contactName;
    private Label contactPhone;
    private Label contactFax;
    private Label abaNo;
    private TextBox txtName;
    private TextBox txtAddress;
    private TextBox txtCity;
    private ComboBox cbState;
    private TextBox txtZip;
    private TextBox txtContactName;
    private TextBox txtPhone;
    private TextBox txtFax;
    private TextBox txtAba;
    private Button button1;
    private Button button2;
    private Label label1;
    private Label label2;
    private Label label3;
    private TextBox txtAddress1;
    private Label label4;
    private TextBox txtEmail;
    private Label label5;
    private ComboBox cmbTimeZone;

    public Dictionary<int, string> HierarchyNodes => this.hierarchyNodes;

    public BankSetupForm() => this.InitializeComponent();

    public BankSetupForm(Sessions.Session session, int oid, bool edit)
    {
      this.InitializeComponent();
      this.session = session;
      this.oid = oid;
      this.edit = edit;
      this.inputEventHelper = new InputEventHelper();
      this.cbState.Items.AddRange((object[]) Utils.GetStates());
      this.initFieldEvents();
      if (edit)
        this.populateFields(oid);
      this.hasBankEditRight = ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ExternalSettings_EditBanks);
      if (this.hasBankEditRight)
        return;
      TPOClientUtils.SecureControls((Control) this);
    }

    public BankSetupForm(SessionObjects session, int oid, bool edit)
    {
      this.InitializeComponent();
      this.objs = session;
      this.oid = oid;
      this.edit = edit;
      this.inputEventHelper = new InputEventHelper();
      this.cbState.Items.AddRange((object[]) Utils.GetStates());
      this.initFieldEvents();
      if (edit)
      {
        this.externalBank = this.objs.ConfigurationManager.GetExternalBankById(oid);
        this.txtName.Text = this.externalBank.BankName;
        this.txtAddress.Text = this.externalBank.Address;
        this.txtAddress1.Text = this.externalBank.Address1;
        this.txtCity.Text = this.externalBank.City;
        this.cbState.Text = this.externalBank.State;
        this.txtZip.Text = this.externalBank.Zip;
        this.txtContactName.Text = this.externalBank.ContactName;
        this.txtEmail.Text = this.externalBank.ContactEmail;
        this.txtPhone.Text = this.externalBank.ContactPhone;
        this.txtFax.Text = this.externalBank.ContactFax;
        this.txtAba.Text = this.externalBank.ABANumber;
        this.cmbTimeZone.SelectedItem = string.IsNullOrEmpty(this.externalBank.TimeZone) ? (object) (string) null : (object) this.externalBank.TimeZone;
      }
      this.hasBankEditRight = ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ExternalSettings_EditBanks);
      if (this.hasBankEditRight)
        return;
      TPOClientUtils.SecureControls((Control) this);
    }

    public void populateFields(int oid)
    {
      this.externalBank = this.session.ConfigurationManager.GetExternalBankById(oid);
      this.txtName.Text = this.externalBank.BankName;
      this.txtAddress.Text = this.externalBank.Address;
      this.txtAddress1.Text = this.externalBank.Address1;
      this.txtCity.Text = this.externalBank.City;
      this.cbState.Text = this.externalBank.State;
      this.txtZip.Text = this.externalBank.Zip;
      this.txtContactName.Text = this.externalBank.ContactName;
      this.txtPhone.Text = this.externalBank.ContactPhone;
      this.txtEmail.Text = this.externalBank.ContactEmail;
      this.txtFax.Text = this.externalBank.ContactFax;
      this.txtAba.Text = this.externalBank.ABANumber;
      this.cmbTimeZone.SelectedItem = string.IsNullOrEmpty(this.externalBank.TimeZone) ? (object) (string) null : (object) this.externalBank.TimeZone;
    }

    private void initFieldEvents()
    {
      this.inputEventHelper.AddZipcodeFieldHelper(this.txtZip, this.txtCity, this.cbState);
      this.inputEventHelper.AddPhoneFieldHelper(this.txtPhone);
      this.inputEventHelper.AddPhoneFieldHelper(this.txtFax);
      this.SetButtonStatus(false);
    }

    private void textField_Changed(object sender, EventArgs e) => this.SetButtonStatus(true);

    public void SetButtonStatus(bool enabled) => this.button1.Enabled = enabled;

    private void button1_Click(object sender, EventArgs e)
    {
      if (!this.DataValidated())
        return;
      if (this.externalBank == null)
        this.externalBank = new ExternalBank();
      this.externalBank.BankName = this.txtName.Text.Trim();
      this.externalBank.Address = this.txtAddress.Text;
      this.externalBank.Address1 = this.txtAddress1.Text;
      this.externalBank.City = this.txtCity.Text;
      this.externalBank.State = this.cbState.Text;
      this.externalBank.Zip = this.txtZip.Text;
      this.externalBank.ContactName = this.txtContactName.Text;
      this.externalBank.ContactEmail = this.txtEmail.Text;
      this.externalBank.ContactPhone = this.txtPhone.Text;
      this.externalBank.ContactFax = this.txtFax.Text;
      this.externalBank.ABANumber = this.txtAba.Text;
      this.externalBank.TimeZone = Convert.ToString(this.cmbTimeZone.SelectedItem);
      if (this.edit)
        this.session.ConfigurationManager.UpdateExternalBank(this.oid, this.externalBank);
      else
        this.oid = this.session.ConfigurationManager.AddExternalBank(this.externalBank);
      this.hierarchyNodes.Add(this.oid, this.externalBank.BankName);
      this.DialogResult = DialogResult.OK;
    }

    public bool DataValidated()
    {
      if (this.txtName.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Bank Name cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtName.Focus();
        return false;
      }
      if (this.txtAba.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "ABA Number cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtAba.Focus();
        return false;
      }
      if (!(this.txtEmail.Text.Trim() != "") || Utils.ValidateEmail(this.txtEmail.Text.Trim()))
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "The email address format is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      this.txtEmail.Focus();
      return false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.name = new Label();
      this.address = new Label();
      this.city = new Label();
      this.state = new Label();
      this.zip = new Label();
      this.contactName = new Label();
      this.contactPhone = new Label();
      this.contactFax = new Label();
      this.abaNo = new Label();
      this.txtName = new TextBox();
      this.txtAddress = new TextBox();
      this.txtCity = new TextBox();
      this.cbState = new ComboBox();
      this.txtZip = new TextBox();
      this.txtContactName = new TextBox();
      this.txtPhone = new TextBox();
      this.txtFax = new TextBox();
      this.txtAba = new TextBox();
      this.button1 = new Button();
      this.button2 = new Button();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.txtAddress1 = new TextBox();
      this.label4 = new Label();
      this.txtEmail = new TextBox();
      this.label5 = new Label();
      this.cmbTimeZone = new ComboBox();
      this.SuspendLayout();
      this.name.AutoSize = true;
      this.name.Location = new Point(6, 16);
      this.name.Name = "name";
      this.name.Size = new Size(63, 13);
      this.name.TabIndex = 0;
      this.name.Text = "Bank Name";
      this.address.AutoSize = true;
      this.address.Location = new Point(6, 38);
      this.address.Name = "address";
      this.address.Size = new Size(51, 13);
      this.address.TabIndex = 1;
      this.address.Text = "Address1";
      this.city.AutoSize = true;
      this.city.Location = new Point(7, 82);
      this.city.Name = "city";
      this.city.Size = new Size(24, 13);
      this.city.TabIndex = 2;
      this.city.Text = "City";
      this.state.AutoSize = true;
      this.state.Location = new Point(6, 104);
      this.state.Name = "state";
      this.state.Size = new Size(32, 13);
      this.state.TabIndex = 3;
      this.state.Text = "State";
      this.zip.AutoSize = true;
      this.zip.Location = new Point(6, 126);
      this.zip.Name = "zip";
      this.zip.Size = new Size(22, 13);
      this.zip.TabIndex = 4;
      this.zip.Text = "Zip";
      this.contactName.AutoSize = true;
      this.contactName.Location = new Point(6, 148);
      this.contactName.Name = "contactName";
      this.contactName.Size = new Size(75, 13);
      this.contactName.TabIndex = 5;
      this.contactName.Text = "Contact Name";
      this.contactPhone.AutoSize = true;
      this.contactPhone.Location = new Point(6, 192);
      this.contactPhone.Name = "contactPhone";
      this.contactPhone.Size = new Size(118, 13);
      this.contactPhone.TabIndex = 6;
      this.contactPhone.Text = "Contact Phone Number";
      this.contactFax.AutoSize = true;
      this.contactFax.Location = new Point(6, 214);
      this.contactFax.Name = "contactFax";
      this.contactFax.Size = new Size(104, 13);
      this.contactFax.TabIndex = 7;
      this.contactFax.Text = "Contact Fax Number";
      this.abaNo.AutoSize = true;
      this.abaNo.Location = new Point(6, 236);
      this.abaNo.Name = "abaNo";
      this.abaNo.Size = new Size(68, 13);
      this.abaNo.TabIndex = 8;
      this.abaNo.Text = "ABA Number";
      this.txtName.Location = new Point(130, 14);
      this.txtName.MaxLength = 64;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(153, 20);
      this.txtName.TabIndex = 9;
      this.txtName.TextChanged += new EventHandler(this.textField_Changed);
      this.txtAddress.Location = new Point(130, 36);
      this.txtAddress.MaxLength = (int) byte.MaxValue;
      this.txtAddress.Name = "txtAddress";
      this.txtAddress.Size = new Size(153, 20);
      this.txtAddress.TabIndex = 10;
      this.txtAddress.TextChanged += new EventHandler(this.textField_Changed);
      this.txtCity.Location = new Point(130, 80);
      this.txtCity.MaxLength = 50;
      this.txtCity.Name = "txtCity";
      this.txtCity.Size = new Size(153, 20);
      this.txtCity.TabIndex = 12;
      this.txtCity.TextChanged += new EventHandler(this.textField_Changed);
      this.cbState.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbState.FormattingEnabled = true;
      this.cbState.Location = new Point(130, 102);
      this.cbState.Name = "cbState";
      this.cbState.Size = new Size(60, 21);
      this.cbState.TabIndex = 13;
      this.cbState.SelectedIndexChanged += new EventHandler(this.textField_Changed);
      this.txtZip.Location = new Point(130, 124);
      this.txtZip.MaxLength = 20;
      this.txtZip.Name = "txtZip";
      this.txtZip.Size = new Size(153, 20);
      this.txtZip.TabIndex = 14;
      this.txtZip.TextChanged += new EventHandler(this.textField_Changed);
      this.txtContactName.Location = new Point(130, 146);
      this.txtContactName.MaxLength = 64;
      this.txtContactName.Name = "txtContactName";
      this.txtContactName.Size = new Size(153, 20);
      this.txtContactName.TabIndex = 15;
      this.txtContactName.TextChanged += new EventHandler(this.textField_Changed);
      this.txtPhone.Location = new Point(130, 190);
      this.txtPhone.MaxLength = 30;
      this.txtPhone.Name = "txtPhone";
      this.txtPhone.Size = new Size(153, 20);
      this.txtPhone.TabIndex = 17;
      this.txtPhone.TextChanged += new EventHandler(this.textField_Changed);
      this.txtFax.Location = new Point(130, 212);
      this.txtFax.MaxLength = 30;
      this.txtFax.Name = "txtFax";
      this.txtFax.Size = new Size(153, 20);
      this.txtFax.TabIndex = 18;
      this.txtFax.TextChanged += new EventHandler(this.textField_Changed);
      this.txtAba.Location = new Point(130, 234);
      this.txtAba.MaxLength = 64;
      this.txtAba.Name = "txtAba";
      this.txtAba.Size = new Size(153, 20);
      this.txtAba.TabIndex = 19;
      this.txtAba.TextChanged += new EventHandler(this.textField_Changed);
      this.button1.Location = new Point(124, 293);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 21;
      this.button1.Text = "OK";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.button2.DialogResult = DialogResult.Cancel;
      this.button2.Location = new Point(208, 293);
      this.button2.Name = "button2";
      this.button2.Size = new Size(75, 23);
      this.button2.TabIndex = 22;
      this.button2.Text = "Cancel";
      this.button2.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.ForeColor = Color.Red;
      this.label1.Location = new Point(67, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(11, 13);
      this.label1.TabIndex = 20;
      this.label1.Text = "*";
      this.label2.AutoSize = true;
      this.label2.ForeColor = Color.Red;
      this.label2.Location = new Point(70, 236);
      this.label2.Name = "label2";
      this.label2.Size = new Size(11, 13);
      this.label2.TabIndex = 21;
      this.label2.Text = "*";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(6, 60);
      this.label3.Name = "label3";
      this.label3.Size = new Size(51, 13);
      this.label3.TabIndex = 22;
      this.label3.Text = "Address2";
      this.txtAddress1.Location = new Point(130, 58);
      this.txtAddress1.MaxLength = (int) byte.MaxValue;
      this.txtAddress1.Name = "txtAddress1";
      this.txtAddress1.Size = new Size(153, 20);
      this.txtAddress1.TabIndex = 11;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 170);
      this.label4.Name = "label4";
      this.label4.Size = new Size(72, 13);
      this.label4.TabIndex = 24;
      this.label4.Text = "Contact Email";
      this.txtEmail.Location = new Point(130, 168);
      this.txtEmail.MaxLength = 64;
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.Size = new Size(153, 20);
      this.txtEmail.TabIndex = 16;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(6, 261);
      this.label5.Name = "label5";
      this.label5.Size = new Size(55, 13);
      this.label5.TabIndex = 25;
      this.label5.Text = "TimeZone";
      this.cmbTimeZone.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbTimeZone.FormattingEnabled = true;
      this.cmbTimeZone.Items.AddRange(new object[7]
      {
        (object) "(UTC -10:00) Hawaii Time",
        (object) "(UTC -09:00) Alaska Time",
        (object) "(UTC -08:00) Pacific Time",
        (object) "(UTC -07:00) Arizona Time",
        (object) "(UTC -07:00) Mountain Time",
        (object) "(UTC -06:00) Central Time",
        (object) "(UTC -05:00) Eastern Time"
      });
      this.cmbTimeZone.Location = new Point(130, 257);
      this.cmbTimeZone.Name = "cmbTimeZone";
      this.cmbTimeZone.Size = new Size(153, 21);
      this.cmbTimeZone.TabIndex = 20;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(293, 328);
      this.Controls.Add((Control) this.cmbTimeZone);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.txtEmail);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.txtAddress1);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.txtAba);
      this.Controls.Add((Control) this.txtFax);
      this.Controls.Add((Control) this.txtPhone);
      this.Controls.Add((Control) this.txtContactName);
      this.Controls.Add((Control) this.txtZip);
      this.Controls.Add((Control) this.cbState);
      this.Controls.Add((Control) this.txtCity);
      this.Controls.Add((Control) this.txtAddress);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.abaNo);
      this.Controls.Add((Control) this.contactFax);
      this.Controls.Add((Control) this.contactPhone);
      this.Controls.Add((Control) this.contactName);
      this.Controls.Add((Control) this.zip);
      this.Controls.Add((Control) this.state);
      this.Controls.Add((Control) this.city);
      this.Controls.Add((Control) this.address);
      this.Controls.Add((Control) this.name);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (BankSetupForm);
      this.Text = "Bank Details";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
