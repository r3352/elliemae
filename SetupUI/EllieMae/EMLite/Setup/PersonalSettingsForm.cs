// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonalSettingsForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.InputEngine.HtmlEmail;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PersonalSettingsForm : SettingsUserControl
  {
    private TextBox emailTextBox;
    private Label emailLabel;
    private TextBox firstNameTextBox;
    private TextBox lastNameTextBox;
    private TextBox passwordTextBox2;
    private TextBox passwordTextBox;
    private Label firstNameLabel;
    private Label lastNameLabel;
    private Label passwordLabel2;
    private Label passwordLabel;
    private Label currPwdLabel;
    private TextBox currPwdTextBox;
    private TextBox phoneTextBox;
    private Label phoneLabel;
    private System.ComponentModel.Container components;
    private bool intermidiateData;
    private Button licBtn;
    private bool deleteBackKey;
    private CheckBox chkFieldHelp;
    private TextBox txtCellPhone;
    private Label label2;
    private TextBox txtFax;
    private GroupContainer groupContainer1;
    private Panel pnlOptions;
    private GroupContainer gcEmailSettings;
    private HtmlEmailControl htmlEmailSignature;
    private Label label3;
    private Button btnPublicProfile;
    private bool changesSaved;
    private UserProfileInfo currentUserProfile;

    public PersonalSettingsForm(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      RolesMappingInfo[] usersRoleMapping = ((WorkflowManager) Session.BPM.GetBpmManager(BpmCategory.Workflow)).GetUsersRoleMapping(Session.UserInfo.Userid);
      if (usersRoleMapping != null && usersRoleMapping.Length != 0)
      {
        foreach (RolesMappingInfo rolesMappingInfo in usersRoleMapping)
        {
          if (rolesMappingInfo.RealWorldRoleID == RealWorldRoleID.LoanOfficer)
          {
            this.licBtn.Visible = true;
            break;
          }
        }
      }
      if (!this.licBtn.Visible)
        this.pnlOptions.Location = this.licBtn.Location;
      this.Reset();
      if (!UserInfo.IsSuperAdministrator(Session.UserID, Session.UserInfo.UserPersonas))
        this.enforceSecurity();
      this.enablePasswordFieldsBasedOnSSO();
      this.htmlEmailSignature.AllowSignatures = false;
      this.htmlEmailSignature.AllowPersonalImages = Session.UserInfo.PersonalStatusOnline;
      this.htmlEmailSignature.ContentChanged += new EventHandler(this.HtmlEmailControl_ContentChanged);
      this.htmlEmailSignature.KeyDown += new KeyEventHandler(this.HtmlEmailControl_DataChanged);
      this.htmlEmailSignature.MouseDown += new MouseEventHandler(this.HtmlEmailControl_DataChanged);
      this.btnPublicProfile.Enabled = ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_Personal_MyProfilePhoto);
    }

    private void enablePasswordFieldsBasedOnSSO()
    {
      bool ssoOnly = Session.UserInfo.SSOOnly;
      this.currPwdTextBox.Enabled = !ssoOnly;
      this.passwordTextBox.Enabled = !ssoOnly;
      this.passwordTextBox2.Enabled = !ssoOnly;
    }

    private void enforceSecurity()
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.txtCellPhone.Enabled = aclManager.GetUserApplicationRight(AclFeature.SettingsTab_Personal_MyProfileCell);
      this.emailTextBox.Enabled = aclManager.GetUserApplicationRight(AclFeature.SettingsTab_Personal_MyProfileEmail);
      this.txtFax.Enabled = aclManager.GetUserApplicationRight(AclFeature.SettingsTab_Personal_MyProfileFax);
      this.firstNameTextBox.Enabled = aclManager.GetUserApplicationRight(AclFeature.SettingsTab_Personal_MyProfileName);
      this.lastNameTextBox.Enabled = aclManager.GetUserApplicationRight(AclFeature.SettingsTab_Personal_MyProfileName);
      this.phoneTextBox.Enabled = aclManager.GetUserApplicationRight(AclFeature.SettingsTab_Personal_MyProfilePhone);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtFax = new TextBox();
      this.label3 = new Label();
      this.txtCellPhone = new TextBox();
      this.label2 = new Label();
      this.chkFieldHelp = new CheckBox();
      this.licBtn = new Button();
      this.phoneLabel = new Label();
      this.phoneTextBox = new TextBox();
      this.currPwdTextBox = new TextBox();
      this.currPwdLabel = new Label();
      this.emailTextBox = new TextBox();
      this.emailLabel = new Label();
      this.firstNameTextBox = new TextBox();
      this.lastNameTextBox = new TextBox();
      this.passwordTextBox2 = new TextBox();
      this.passwordTextBox = new TextBox();
      this.firstNameLabel = new Label();
      this.lastNameLabel = new Label();
      this.passwordLabel2 = new Label();
      this.passwordLabel = new Label();
      this.groupContainer1 = new GroupContainer();
      this.pnlOptions = new Panel();
      this.gcEmailSettings = new GroupContainer();
      this.htmlEmailSignature = new HtmlEmailControl();
      this.btnPublicProfile = new Button();
      this.groupContainer1.SuspendLayout();
      this.pnlOptions.SuspendLayout();
      this.gcEmailSettings.SuspendLayout();
      this.SuspendLayout();
      this.txtFax.Location = new Point(112, 214);
      this.txtFax.Name = "txtFax";
      this.txtFax.Size = new Size(266, 20);
      this.txtFax.TabIndex = 23;
      this.txtFax.TextChanged += new EventHandler(this.textChanged);
      this.txtFax.KeyDown += new KeyEventHandler(this.txtFax_KeyDown);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(8, 217);
      this.label3.Name = "label3";
      this.label3.Size = new Size(25, 14);
      this.label3.TabIndex = 22;
      this.label3.Text = "Fax";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.txtCellPhone.Location = new Point(112, 192);
      this.txtCellPhone.Name = "txtCellPhone";
      this.txtCellPhone.Size = new Size(266, 20);
      this.txtCellPhone.TabIndex = 21;
      this.txtCellPhone.TextChanged += new EventHandler(this.textChanged);
      this.txtCellPhone.KeyDown += new KeyEventHandler(this.txtCellPhone_KeyDown);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(8, 195);
      this.label2.Name = "label2";
      this.label2.Size = new Size(57, 14);
      this.label2.TabIndex = 20;
      this.label2.Text = "Cell Phone";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.chkFieldHelp.Location = new Point(0, 0);
      this.chkFieldHelp.Name = "chkFieldHelp";
      this.chkFieldHelp.Size = new Size(280, 18);
      this.chkFieldHelp.TabIndex = 19;
      this.chkFieldHelp.Text = "Enable field-level help.";
      this.chkFieldHelp.CheckedChanged += new EventHandler(this.textChanged);
      this.licBtn.Location = new Point(112, 242);
      this.licBtn.Name = "licBtn";
      this.licBtn.Size = new Size(116, 22);
      this.licBtn.TabIndex = 10;
      this.licBtn.Text = "My State Licenses";
      this.licBtn.Visible = false;
      this.licBtn.Click += new EventHandler(this.licBtn_Click);
      this.phoneLabel.AutoSize = true;
      this.phoneLabel.Location = new Point(8, 173);
      this.phoneLabel.Name = "phoneLabel";
      this.phoneLabel.Size = new Size(37, 14);
      this.phoneLabel.TabIndex = 13;
      this.phoneLabel.Text = "Phone";
      this.phoneLabel.TextAlign = ContentAlignment.MiddleLeft;
      this.phoneTextBox.Location = new Point(112, 170);
      this.phoneTextBox.Name = "phoneTextBox";
      this.phoneTextBox.Size = new Size(266, 20);
      this.phoneTextBox.TabIndex = 8;
      this.phoneTextBox.TextChanged += new EventHandler(this.textChanged);
      this.phoneTextBox.KeyDown += new KeyEventHandler(this.phoneTextBox_KeyDown);
      this.currPwdTextBox.Location = new Point(112, 38);
      this.currPwdTextBox.Name = "currPwdTextBox";
      this.currPwdTextBox.PasswordChar = '*';
      this.currPwdTextBox.Size = new Size(266, 20);
      this.currPwdTextBox.TabIndex = 2;
      this.currPwdLabel.AutoSize = true;
      this.currPwdLabel.Location = new Point(8, 41);
      this.currPwdLabel.Name = "currPwdLabel";
      this.currPwdLabel.Size = new Size(96, 14);
      this.currPwdLabel.TabIndex = 1;
      this.currPwdLabel.Text = "Current Password";
      this.currPwdLabel.TextAlign = ContentAlignment.MiddleLeft;
      this.emailTextBox.Location = new Point(112, 148);
      this.emailTextBox.Name = "emailTextBox";
      this.emailTextBox.Size = new Size(266, 20);
      this.emailTextBox.TabIndex = 7;
      this.emailTextBox.TextChanged += new EventHandler(this.textChanged);
      this.emailLabel.AutoSize = true;
      this.emailLabel.Location = new Point(8, 151);
      this.emailLabel.Name = "emailLabel";
      this.emailLabel.Size = new Size(31, 14);
      this.emailLabel.TabIndex = 11;
      this.emailLabel.Text = "Email";
      this.emailLabel.TextAlign = ContentAlignment.MiddleLeft;
      this.firstNameTextBox.Location = new Point(112, 126);
      this.firstNameTextBox.Name = "firstNameTextBox";
      this.firstNameTextBox.Size = new Size(266, 20);
      this.firstNameTextBox.TabIndex = 6;
      this.firstNameTextBox.TextChanged += new EventHandler(this.textChanged);
      this.lastNameTextBox.Location = new Point(112, 104);
      this.lastNameTextBox.Name = "lastNameTextBox";
      this.lastNameTextBox.Size = new Size(266, 20);
      this.lastNameTextBox.TabIndex = 5;
      this.lastNameTextBox.TextChanged += new EventHandler(this.textChanged);
      this.passwordTextBox2.Location = new Point(112, 82);
      this.passwordTextBox2.MaxLength = 50;
      this.passwordTextBox2.Name = "passwordTextBox2";
      this.passwordTextBox2.PasswordChar = '*';
      this.passwordTextBox2.Size = new Size(266, 20);
      this.passwordTextBox2.TabIndex = 4;
      this.passwordTextBox2.TextChanged += new EventHandler(this.textChanged);
      this.passwordTextBox.Location = new Point(112, 60);
      this.passwordTextBox.MaxLength = 50;
      this.passwordTextBox.Name = "passwordTextBox";
      this.passwordTextBox.PasswordChar = '*';
      this.passwordTextBox.Size = new Size(266, 20);
      this.passwordTextBox.TabIndex = 3;
      this.passwordTextBox.TextChanged += new EventHandler(this.textChanged);
      this.firstNameLabel.AutoSize = true;
      this.firstNameLabel.Location = new Point(8, 129);
      this.firstNameLabel.Name = "firstNameLabel";
      this.firstNameLabel.Size = new Size(58, 14);
      this.firstNameLabel.TabIndex = 9;
      this.firstNameLabel.Text = "First Name";
      this.firstNameLabel.TextAlign = ContentAlignment.MiddleLeft;
      this.lastNameLabel.AutoSize = true;
      this.lastNameLabel.Location = new Point(8, 107);
      this.lastNameLabel.Name = "lastNameLabel";
      this.lastNameLabel.Size = new Size(58, 14);
      this.lastNameLabel.TabIndex = 7;
      this.lastNameLabel.Text = "Last Name";
      this.lastNameLabel.TextAlign = ContentAlignment.MiddleLeft;
      this.passwordLabel2.AutoSize = true;
      this.passwordLabel2.Location = new Point(8, 85);
      this.passwordLabel2.Name = "passwordLabel2";
      this.passwordLabel2.Size = new Size(98, 14);
      this.passwordLabel2.TabIndex = 5;
      this.passwordLabel2.Text = "Re-type Password";
      this.passwordLabel2.TextAlign = ContentAlignment.MiddleLeft;
      this.passwordLabel.AutoSize = true;
      this.passwordLabel.Location = new Point(8, 63);
      this.passwordLabel.Name = "passwordLabel";
      this.passwordLabel.Size = new Size(83, 14);
      this.passwordLabel.TabIndex = 3;
      this.passwordLabel.Text = "New Password";
      this.passwordLabel.TextAlign = ContentAlignment.MiddleLeft;
      this.groupContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.btnPublicProfile);
      this.groupContainer1.Controls.Add((Control) this.pnlOptions);
      this.groupContainer1.Controls.Add((Control) this.txtFax);
      this.groupContainer1.Controls.Add((Control) this.txtCellPhone);
      this.groupContainer1.Controls.Add((Control) this.passwordTextBox);
      this.groupContainer1.Controls.Add((Control) this.passwordTextBox2);
      this.groupContainer1.Controls.Add((Control) this.lastNameTextBox);
      this.groupContainer1.Controls.Add((Control) this.firstNameTextBox);
      this.groupContainer1.Controls.Add((Control) this.phoneTextBox);
      this.groupContainer1.Controls.Add((Control) this.currPwdTextBox);
      this.groupContainer1.Controls.Add((Control) this.emailTextBox);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.passwordLabel);
      this.groupContainer1.Controls.Add((Control) this.passwordLabel2);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.lastNameLabel);
      this.groupContainer1.Controls.Add((Control) this.firstNameLabel);
      this.groupContainer1.Controls.Add((Control) this.phoneLabel);
      this.groupContainer1.Controls.Add((Control) this.emailLabel);
      this.groupContainer1.Controls.Add((Control) this.currPwdLabel);
      this.groupContainer1.Controls.Add((Control) this.licBtn);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(3, 3);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(609, 341);
      this.groupContainer1.TabIndex = 1;
      this.groupContainer1.Text = "Personal Settings";
      this.pnlOptions.Controls.Add((Control) this.chkFieldHelp);
      this.pnlOptions.Location = new Point(112, 270);
      this.pnlOptions.Name = "pnlOptions";
      this.pnlOptions.Size = new Size(312, 48);
      this.pnlOptions.TabIndex = 25;
      this.gcEmailSettings.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcEmailSettings.Controls.Add((Control) this.htmlEmailSignature);
      this.gcEmailSettings.HeaderForeColor = SystemColors.ControlText;
      this.gcEmailSettings.Location = new Point(3, 350);
      this.gcEmailSettings.Name = "gcEmailSettings";
      this.gcEmailSettings.Size = new Size(609, 233);
      this.gcEmailSettings.TabIndex = 2;
      this.gcEmailSettings.Text = "Email Signature";
      this.htmlEmailSignature.Dock = DockStyle.Fill;
      this.htmlEmailSignature.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.htmlEmailSignature.Location = new Point(1, 26);
      this.htmlEmailSignature.Name = "htmlEmailSignature";
      this.htmlEmailSignature.Size = new Size(607, 206);
      this.htmlEmailSignature.TabIndex = 0;
      this.btnPublicProfile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPublicProfile.Location = new Point(520, 2);
      this.btnPublicProfile.Margin = new Padding(0);
      this.btnPublicProfile.Name = "btnPublicProfile";
      this.btnPublicProfile.Size = new Size(77, 22);
      this.btnPublicProfile.TabIndex = 26;
      this.btnPublicProfile.Text = "Public Profile";
      this.btnPublicProfile.Click += new EventHandler(this.btnPublicProfile_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcEmailSettings);
      this.Controls.Add((Control) this.groupContainer1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (PersonalSettingsForm);
      this.Size = new Size(617, 587);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.pnlOptions.ResumeLayout(false);
      this.gcEmailSettings.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public override void Reset()
    {
      this.currPwdTextBox.Text = "";
      this.passwordTextBox.Text = "";
      this.passwordTextBox2.Text = "";
      this.lastNameTextBox.Text = Session.UserInfo.LastName;
      this.firstNameTextBox.Text = Session.UserInfo.FirstName;
      this.emailTextBox.Text = Session.UserInfo.Email;
      this.intermidiateData = true;
      this.phoneTextBox.Text = Session.UserInfo.Phone;
      this.txtCellPhone.Text = Session.UserInfo.CellPhone;
      this.txtFax.Text = Session.UserInfo.Fax;
      this.chkFieldHelp.Checked = Session.GetPrivateProfileString("HELP", "FieldHelp") != "OFF";
      if (string.IsNullOrEmpty(Session.UserInfo.EmailSignature))
        this.htmlEmailSignature.LoadText(string.Empty, false);
      else
        this.htmlEmailSignature.LoadHtml(Session.UserInfo.EmailSignature, false);
      this.setDirtyFlag(false);
    }

    public bool ChangesSaved => this.changesSaved;

    public override void Save()
    {
      this.changesSaved = false;
      if (!this.IsDirty)
        this.Dispose();
      else if (this.lastNameTextBox.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter both a user last name and first name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.lastNameTextBox.Focus();
      }
      else if (this.firstNameTextBox.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter both a user last name and first name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.firstNameTextBox.Focus();
      }
      else if (this.emailTextBox.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Many Encompass features require an email address. Please enter one at this time.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.emailTextBox.Focus();
      }
      else if (!Utils.ValidateEmail(this.emailTextBox.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The email address format is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.emailTextBox.Focus();
      }
      else
      {
        if (this.passwordTextBox.Text != string.Empty)
        {
          if (string.Compare(Session.UserID, "tpowcadmin", true) == 0)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You cannot change the password for the tpowcadmin user account here. Your TPO WebCenter website relies on this tpowcadmin account to communicate with your Encompass system. Modifying this account's password will cause your TPO WebCenter to stop working and prevent loan data from being passed between your website and your Encompass system. To safely update this account's password, use the Loan Settings page in TPO WebCenter Administration.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
          if (!Session.User.ComparePassword(this.currPwdTextBox.Text))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The password you typed is incorrect. The password is case-sensitive. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          if (!this.passwordTextBox.Text.Equals(this.passwordTextBox2.Text))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The new passwords do not match. The password is case-sensitive. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          if (!this.validateAndSavePassword(this.passwordTextBox.Text))
            return;
        }
        Session.User.UpdatePersonalInfo(this.firstNameTextBox.Text, this.lastNameTextBox.Text, this.emailTextBox.Text, this.phoneTextBox.Text, this.txtCellPhone.Text, this.txtFax.Text, this.htmlEmailSignature.Html, "");
        Session.WritePrivateProfileString("HELP", "FieldHelp", this.chkFieldHelp.Checked ? "ON" : "OFF");
        Session.RecacheUserInfo();
        this.Reset();
        this.changesSaved = true;
      }
    }

    private void textChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
      if (sender != this.phoneTextBox && sender != this.txtCellPhone && sender != this.txtFax)
        return;
      if (this.intermidiateData)
        this.intermidiateData = false;
      else if (this.deleteBackKey)
      {
        this.deleteBackKey = false;
      }
      else
      {
        FieldFormat fieldFormat;
        FieldFormat dataFormat = fieldFormat = FieldFormat.PHONE;
        TextBox textBox = (TextBox) sender;
        bool needsUpdate = false;
        string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
        if (!needsUpdate)
          return;
        this.intermidiateData = true;
        textBox.Text = str;
        textBox.SelectionStart = str.Length;
      }
    }

    private void phoneTextBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete)
        return;
      this.deleteBackKey = true;
    }

    private void licBtn_Click(object sender, EventArgs e)
    {
      int num = (int) new LoanOfficerLicenseDialog(Session.OrganizationManager.GetLOLicenses(Session.UserInfo.Userid), Session.UserInfo.Userid, true).ShowDialog((IWin32Window) this);
    }

    private bool validateAndSavePassword(string password)
    {
      PwdRuleValidator passwordValidator = Session.OrganizationManager.GetPasswordValidator();
      if (!passwordValidator.CheckMinLength(password))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The password must be at least " + (object) passwordValidator.MinimumLength + " characters long.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (!passwordValidator.CheckCompositionRule(password))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The password must contain the following:" + Environment.NewLine + Environment.NewLine + passwordValidator.GetCompositionRuleDescription(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      try
      {
        string empty = string.Empty;
        Session.User.ChangePassword(password);
        return true;
      }
      catch (SecurityException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "An error has occurred attempting to save your password: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      return false;
    }

    private void txtCellPhone_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete)
        return;
      this.deleteBackKey = true;
    }

    private void txtFax_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete)
        return;
      this.deleteBackKey = true;
    }

    private void HtmlEmailControl_DataChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void HtmlEmailControl_ContentChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void btnPublicProfile_Click(object sender, EventArgs e)
    {
      using (PublicProfileDialog publicProfileDialog = new PublicProfileDialog(Session.UserInfo.Userid))
      {
        if (publicProfileDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        this.currentUserProfile = publicProfileDialog.getUserProfileInfo();
      }
    }
  }
}
