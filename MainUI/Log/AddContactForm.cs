// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.AddContactForm
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class AddContactForm : Form
  {
    private BizCategoryUtil catUtil = new BizCategoryUtil(Session.SessionObjects);
    private int contactID = -1;
    private IContainer components;
    private DialogButtons dialogButtons1;
    private Label label1;
    private TextBox txtName;
    private Label label2;
    private ComboBox cboRole;
    private Label label3;
    private TextBox txtPhone;
    private TextBox txtEmail;
    private Label label4;
    private TextBox txtAddress;
    private Label label5;
    private TextBox txtCity;
    private Label label6;
    private TextBox txtState;
    private Label label7;
    private StandardIconButton btnRolodex;
    private Label label8;
    private TextBox txtZip;
    private ToolTip toolTip1;

    public AddContactForm(MilestoneTaskLog.TaskContact taskContact)
    {
      this.InitializeComponent();
      this.cboRole.Items.Clear();
      this.cboRole.Items.AddRange((object[]) Session.ContactManager.GetBizCategories());
      this.cboRole.Items.Insert(0, (object) "Borrower");
      if (taskContact == null)
        return;
      this.txtName.Text = taskContact.ContactName;
      this.cboRole.Text = taskContact.ContactRole;
      this.txtPhone.Text = taskContact.ContactPhone;
      this.txtEmail.Text = taskContact.ContactEmail;
      this.txtAddress.Text = taskContact.ContactAddress;
      this.txtCity.Text = taskContact.ContactCity;
      this.txtState.Text = taskContact.ContactState;
      this.txtZip.Text = taskContact.ContactZip;
    }

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      if (this.txtName.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter a contact name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtName.Focus();
      }
      else if (this.txtPhone.Text.Trim() == string.Empty && this.txtEmail.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter a phone number or personal email address.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtPhone.Focus();
      }
      else if (this.txtEmail.Text.Trim() != string.Empty && !Utils.ValidateEmail(this.txtEmail.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Email Address format is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtEmail.Focus();
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    private void txtName_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      this.pictureBoxContact_Click((object) null, (EventArgs) null);
    }

    private void pictureBoxContact_Click(object sender, EventArgs e)
    {
      RxContactInfo rxContactInfo = new RxContactInfo();
      using (SelectionDialog selectionDialog = new SelectionDialog((ConversationDialog) null, Session.LoanData))
      {
        if (selectionDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        RxContactInfo rxContact = selectionDialog.RxContact;
        this.contactID = rxContact.ContactID;
        this.txtName.Text = rxContact[RolodexField.Name];
        this.txtPhone.Text = rxContact[RolodexField.Phone];
        this.txtEmail.Text = rxContact[RolodexField.Email];
        this.txtAddress.Text = rxContact[RolodexField.FullAddress];
        this.txtCity.Text = rxContact[RolodexField.City];
        this.txtState.Text = rxContact[RolodexField.State];
        this.txtZip.Text = rxContact[RolodexField.ZipCode];
        if (rxContact[RolodexField.Category] == string.Empty)
          this.cboRole.Text = selectionDialog.ContactType;
        else
          this.cboRole.Text = rxContact[RolodexField.Category];
      }
    }

    private void zipcodeField_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      if (textBox.Text.Trim().Length < 5)
        return;
      ZipCodeInfo zipCodeInfo = ZipcodeSelector.GetZipCodeInfo(textBox.Text.Trim().Substring(0, 5), ZipCodeUtils.GetMultipleZipInfoAt(textBox.Text.Trim().Substring(0, 5)));
      if (zipCodeInfo == null)
        return;
      this.txtCity.Text = Utils.CapsConvert(zipCodeInfo.City, false);
      this.txtState.Text = zipCodeInfo.State;
    }

    private void phoneField_KeyUp(object sender, KeyEventArgs e)
    {
      this.formatFieldValue(sender, FieldFormat.PHONE);
    }

    private void zipcodeField_KeyUp(object sender, KeyEventArgs e)
    {
      this.formatFieldValue(sender, FieldFormat.ZIPCODE);
    }

    private void stateField_KeyUp(object sender, KeyEventArgs e)
    {
      this.formatFieldValue(sender, FieldFormat.STATE);
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

    public string ContactName => this.txtName.Text.Trim();

    public string ContactPhone => this.txtPhone.Text.Trim();

    public string ContactEmail => this.txtEmail.Text.Trim();

    public string ContactAddress => this.txtAddress.Text.Trim();

    public string ContactCity => this.txtCity.Text.Trim();

    public string ContactState => this.txtState.Text.Trim();

    public string ContactZip => this.txtZip.Text.Trim();

    public string ContactRole => this.cboRole.Text.Trim();

    public int ContactID => this.contactID;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.dialogButtons1 = new DialogButtons();
      this.label1 = new Label();
      this.txtName = new TextBox();
      this.label2 = new Label();
      this.cboRole = new ComboBox();
      this.label3 = new Label();
      this.txtPhone = new TextBox();
      this.txtEmail = new TextBox();
      this.label4 = new Label();
      this.txtAddress = new TextBox();
      this.label5 = new Label();
      this.txtCity = new TextBox();
      this.label6 = new Label();
      this.txtState = new TextBox();
      this.label7 = new Label();
      this.btnRolodex = new StandardIconButton();
      this.label8 = new Label();
      this.txtZip = new TextBox();
      this.toolTip1 = new ToolTip(this.components);
      ((ISupportInitialize) this.btnRolodex).BeginInit();
      this.SuspendLayout();
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 204);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.Size = new Size(390, 44);
      this.dialogButtons1.TabIndex = 8;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(21, 20);
      this.label1.Name = "label1";
      this.label1.Size = new Size(35, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Name";
      this.txtName.Location = new Point(103, 17);
      this.txtName.MaxLength = 128;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(235, 20);
      this.txtName.TabIndex = 0;
      this.txtName.MouseDown += new MouseEventHandler(this.txtName_MouseDown);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(21, 46);
      this.label2.Name = "label2";
      this.label2.Size = new Size(29, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Role";
      this.cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRole.FormattingEnabled = true;
      this.cboRole.Location = new Point(103, 43);
      this.cboRole.Name = "cboRole";
      this.cboRole.Size = new Size(235, 21);
      this.cboRole.TabIndex = 1;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(21, 73);
      this.label3.Name = "label3";
      this.label3.Size = new Size(69, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "Home Phone";
      this.txtPhone.Location = new Point(103, 70);
      this.txtPhone.MaxLength = 17;
      this.txtPhone.Name = "txtPhone";
      this.txtPhone.Size = new Size(235, 20);
      this.txtPhone.TabIndex = 2;
      this.txtPhone.KeyUp += new KeyEventHandler(this.phoneField_KeyUp);
      this.txtEmail.Location = new Point(103, 96);
      this.txtEmail.MaxLength = 128;
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.Size = new Size(235, 20);
      this.txtEmail.TabIndex = 3;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(21, 99);
      this.label4.Name = "label4";
      this.label4.Size = new Size(76, 13);
      this.label4.TabIndex = 8;
      this.label4.Text = "Personal Email";
      this.txtAddress.Location = new Point(103, 122);
      this.txtAddress.MaxLength = 256;
      this.txtAddress.Name = "txtAddress";
      this.txtAddress.Size = new Size(235, 20);
      this.txtAddress.TabIndex = 4;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(21, 125);
      this.label5.Name = "label5";
      this.label5.Size = new Size(45, 13);
      this.label5.TabIndex = 10;
      this.label5.Text = "Address";
      this.txtCity.Location = new Point(103, 148);
      this.txtCity.MaxLength = 256;
      this.txtCity.Name = "txtCity";
      this.txtCity.Size = new Size(235, 20);
      this.txtCity.TabIndex = 5;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(21, 151);
      this.label6.Name = "label6";
      this.label6.Size = new Size(24, 13);
      this.label6.TabIndex = 12;
      this.label6.Text = "City";
      this.txtState.Location = new Point(103, 174);
      this.txtState.MaxLength = 2;
      this.txtState.Name = "txtState";
      this.txtState.Size = new Size(64, 20);
      this.txtState.TabIndex = 6;
      this.txtState.KeyUp += new KeyEventHandler(this.stateField_KeyUp);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(21, 177);
      this.label7.Name = "label7";
      this.label7.Size = new Size(32, 13);
      this.label7.TabIndex = 14;
      this.label7.Text = "State";
      this.btnRolodex.BackColor = Color.Transparent;
      this.btnRolodex.Location = new Point(344, 18);
      this.btnRolodex.Name = "btnRolodex";
      this.btnRolodex.Size = new Size(16, 16);
      this.btnRolodex.StandardButtonType = StandardIconButton.ButtonType.RolodexButton;
      this.btnRolodex.TabIndex = 19;
      this.btnRolodex.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRolodex, "Find a contact");
      this.btnRolodex.Click += new EventHandler(this.pictureBoxContact_Click);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(191, 177);
      this.label8.Name = "label8";
      this.label8.Size = new Size(46, 13);
      this.label8.TabIndex = 20;
      this.label8.Text = "Zipcode";
      this.txtZip.Location = new Point(243, 174);
      this.txtZip.MaxLength = 10;
      this.txtZip.Name = "txtZip";
      this.txtZip.Size = new Size(95, 20);
      this.txtZip.TabIndex = 7;
      this.txtZip.Leave += new EventHandler(this.zipcodeField_Leave);
      this.txtZip.KeyUp += new KeyEventHandler(this.zipcodeField_KeyUp);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(390, 248);
      this.Controls.Add((Control) this.txtZip);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.btnRolodex);
      this.Controls.Add((Control) this.txtState);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.txtCity);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.txtAddress);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.txtEmail);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.txtPhone);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.cboRole);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.dialogButtons1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddContactForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add/Edit Contact";
      ((ISupportInitialize) this.btnRolodex).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
