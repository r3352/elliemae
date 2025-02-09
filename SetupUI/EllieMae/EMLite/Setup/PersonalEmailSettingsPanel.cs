// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonalEmailSettingsPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PersonalEmailSettingsPanel : SettingsUserControl
  {
    private Panel pnlGlobalOptions;
    private RadioButton radOutlook;
    private System.ComponentModel.Container components;
    private RadioButton radSMTP;
    private Label lblInfo;
    private GroupContainer gbSMTP;
    private Panel pnlLogin;
    private TextBox txtSMTPPassword;
    private Button btnTestSMTP;
    private Label label5;
    private Label label4;
    private TextBox txtSMTPUserName;
    private Label label6;
    private Panel pnlServer;
    private TextBox txtSMTPServer;
    private Label label1;
    private Label label2;
    private Label label3;
    private TextBox txtSMTPPort;
    private Panel pnlOverride;
    private CheckBox chkSMTPUseCompany;
    private BorderPanel borderPanel1;
    private CheckBox chkSSL;
    private Panel pnlInfo;
    private Label label9;

    public PersonalEmailSettingsPanel(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.resetForm();
      this.setDirtyFlag(false);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlGlobalOptions = new Panel();
      this.radSMTP = new RadioButton();
      this.radOutlook = new RadioButton();
      this.lblInfo = new Label();
      this.gbSMTP = new GroupContainer();
      this.pnlLogin = new Panel();
      this.txtSMTPPassword = new TextBox();
      this.btnTestSMTP = new Button();
      this.label5 = new Label();
      this.label4 = new Label();
      this.txtSMTPUserName = new TextBox();
      this.label6 = new Label();
      this.pnlServer = new Panel();
      this.chkSSL = new CheckBox();
      this.txtSMTPServer = new TextBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.txtSMTPPort = new TextBox();
      this.pnlOverride = new Panel();
      this.chkSMTPUseCompany = new CheckBox();
      this.label9 = new Label();
      this.borderPanel1 = new BorderPanel();
      this.pnlInfo = new Panel();
      this.pnlGlobalOptions.SuspendLayout();
      this.gbSMTP.SuspendLayout();
      this.pnlLogin.SuspendLayout();
      this.pnlServer.SuspendLayout();
      this.pnlOverride.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.pnlInfo.SuspendLayout();
      this.SuspendLayout();
      this.pnlGlobalOptions.Controls.Add((Control) this.radSMTP);
      this.pnlGlobalOptions.Controls.Add((Control) this.radOutlook);
      this.pnlGlobalOptions.Dock = DockStyle.Top;
      this.pnlGlobalOptions.Location = new Point(1, 1);
      this.pnlGlobalOptions.Name = "pnlGlobalOptions";
      this.pnlGlobalOptions.Size = new Size(533, 46);
      this.pnlGlobalOptions.TabIndex = 7;
      this.radSMTP.Location = new Point(7, 24);
      this.radSMTP.Name = "radSMTP";
      this.radSMTP.Size = new Size(97, 18);
      this.radSMTP.TabIndex = 1;
      this.radSMTP.Text = "Use SMTP";
      this.radSMTP.CheckedChanged += new EventHandler(this.onGlobalOptionChanged);
      this.radOutlook.Location = new Point(7, 4);
      this.radOutlook.Name = "radOutlook";
      this.radOutlook.Size = new Size(97, 18);
      this.radOutlook.TabIndex = 0;
      this.radOutlook.Text = "Use Outlook";
      this.radOutlook.CheckedChanged += new EventHandler(this.onGlobalOptionChanged);
      this.lblInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblInfo.Location = new Point(7, 2);
      this.lblInfo.Name = "lblInfo";
      this.lblInfo.Size = new Size(514, 33);
      this.lblInfo.TabIndex = 9;
      this.lblInfo.Text = "Your account is configured to use the company's SMTP settings for email delivery. Contact your Encompass administrator for additional details.";
      this.lblInfo.TextAlign = ContentAlignment.MiddleLeft;
      this.gbSMTP.Borders = AnchorStyles.Top;
      this.gbSMTP.Controls.Add((Control) this.pnlLogin);
      this.gbSMTP.Controls.Add((Control) this.pnlServer);
      this.gbSMTP.Controls.Add((Control) this.pnlOverride);
      this.gbSMTP.Dock = DockStyle.Fill;
      this.gbSMTP.HeaderForeColor = SystemColors.ControlText;
      this.gbSMTP.Location = new Point(1, 83);
      this.gbSMTP.Name = "gbSMTP";
      this.gbSMTP.Size = new Size(533, (int) byte.MaxValue);
      this.gbSMTP.TabIndex = 0;
      this.gbSMTP.Text = "SMTP Settings";
      this.pnlLogin.Controls.Add((Control) this.txtSMTPPassword);
      this.pnlLogin.Controls.Add((Control) this.btnTestSMTP);
      this.pnlLogin.Controls.Add((Control) this.label5);
      this.pnlLogin.Controls.Add((Control) this.label4);
      this.pnlLogin.Controls.Add((Control) this.txtSMTPUserName);
      this.pnlLogin.Controls.Add((Control) this.label6);
      this.pnlLogin.Dock = DockStyle.Fill;
      this.pnlLogin.Location = new Point(0, 165);
      this.pnlLogin.Name = "pnlLogin";
      this.pnlLogin.Size = new Size(533, 90);
      this.pnlLogin.TabIndex = 15;
      this.txtSMTPPassword.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtSMTPPassword.Location = new Point(166, 45);
      this.txtSMTPPassword.MaxLength = 50;
      this.txtSMTPPassword.Name = "txtSMTPPassword";
      this.txtSMTPPassword.PasswordChar = '*';
      this.txtSMTPPassword.Size = new Size(149, 20);
      this.txtSMTPPassword.TabIndex = 9;
      this.txtSMTPPassword.TextChanged += new EventHandler(this.settingsChanged);
      this.btnTestSMTP.Enabled = false;
      this.btnTestSMTP.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnTestSMTP.Location = new Point(321, 43);
      this.btnTestSMTP.Name = "btnTestSMTP";
      this.btnTestSMTP.Size = new Size(62, 23);
      this.btnTestSMTP.TabIndex = 12;
      this.btnTestSMTP.Text = "&Test";
      this.btnTestSMTP.UseVisualStyleBackColor = true;
      this.btnTestSMTP.Click += new EventHandler(this.btnTestSMTP_Click);
      this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(8, 5);
      this.label5.Name = "label5";
      this.label5.Size = new Size(170, 16);
      this.label5.TabIndex = 5;
      this.label5.Text = "Login Information";
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(8, 27);
      this.label4.Name = "label4";
      this.label4.Size = new Size(155, 16);
      this.label4.TabIndex = 6;
      this.label4.Text = "User Name";
      this.txtSMTPUserName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtSMTPUserName.Location = new Point(166, 24);
      this.txtSMTPUserName.MaxLength = 50;
      this.txtSMTPUserName.Name = "txtSMTPUserName";
      this.txtSMTPUserName.Size = new Size(149, 20);
      this.txtSMTPUserName.TabIndex = 7;
      this.txtSMTPUserName.TextChanged += new EventHandler(this.settingsChanged);
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(8, 48);
      this.label6.Name = "label6";
      this.label6.Size = new Size(155, 16);
      this.label6.TabIndex = 8;
      this.label6.Text = "Password";
      this.pnlServer.Controls.Add((Control) this.chkSSL);
      this.pnlServer.Controls.Add((Control) this.txtSMTPServer);
      this.pnlServer.Controls.Add((Control) this.label1);
      this.pnlServer.Controls.Add((Control) this.label2);
      this.pnlServer.Controls.Add((Control) this.label3);
      this.pnlServer.Controls.Add((Control) this.txtSMTPPort);
      this.pnlServer.Dock = DockStyle.Top;
      this.pnlServer.Location = new Point(0, 72);
      this.pnlServer.Name = "pnlServer";
      this.pnlServer.Size = new Size(533, 93);
      this.pnlServer.TabIndex = 14;
      this.chkSSL.AutoSize = true;
      this.chkSSL.Location = new Point(166, 69);
      this.chkSSL.Name = "chkSSL";
      this.chkSSL.Size = new Size(192, 17);
      this.chkSSL.TabIndex = 5;
      this.chkSSL.Text = "Use SSL for secure communication";
      this.chkSSL.UseVisualStyleBackColor = true;
      this.chkSSL.CheckedChanged += new EventHandler(this.onSMTPSettingsChanged);
      this.txtSMTPServer.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtSMTPServer.Location = new Point(166, 25);
      this.txtSMTPServer.MaxLength = 100;
      this.txtSMTPServer.Name = "txtSMTPServer";
      this.txtSMTPServer.Size = new Size(149, 20);
      this.txtSMTPServer.TabIndex = 3;
      this.txtSMTPServer.TextChanged += new EventHandler(this.onSMTPSettingsChanged);
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(8, 7);
      this.label1.Name = "label1";
      this.label1.Size = new Size(166, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "Server Information";
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(8, 29);
      this.label2.Name = "label2";
      this.label2.Size = new Size(156, 16);
      this.label2.TabIndex = 1;
      this.label2.Text = "Outgoing Mail Server (SMTP)";
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(8, 48);
      this.label3.Name = "label3";
      this.label3.Size = new Size(155, 16);
      this.label3.TabIndex = 2;
      this.label3.Text = "Port";
      this.txtSMTPPort.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtSMTPPort.Location = new Point(166, 46);
      this.txtSMTPPort.MaxLength = 5;
      this.txtSMTPPort.Name = "txtSMTPPort";
      this.txtSMTPPort.Size = new Size(149, 20);
      this.txtSMTPPort.TabIndex = 4;
      this.txtSMTPPort.Text = "25";
      this.txtSMTPPort.TextChanged += new EventHandler(this.onSMTPSettingsChanged);
      this.pnlOverride.Controls.Add((Control) this.chkSMTPUseCompany);
      this.pnlOverride.Controls.Add((Control) this.label9);
      this.pnlOverride.Dock = DockStyle.Top;
      this.pnlOverride.Location = new Point(0, 26);
      this.pnlOverride.Name = "pnlOverride";
      this.pnlOverride.Size = new Size(533, 46);
      this.pnlOverride.TabIndex = 1;
      this.chkSMTPUseCompany.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.chkSMTPUseCompany.Location = new Point(10, 9);
      this.chkSMTPUseCompany.Name = "chkSMTPUseCompany";
      this.chkSMTPUseCompany.Size = new Size(216, 16);
      this.chkSMTPUseCompany.TabIndex = 5;
      this.chkSMTPUseCompany.Text = "Use company settings";
      this.chkSMTPUseCompany.CheckedChanged += new EventHandler(this.chkSMTPUseCompany_CheckedChanged);
      this.label9.BorderStyle = BorderStyle.Fixed3D;
      this.label9.Location = new Point(10, 35);
      this.label9.Name = "label9";
      this.label9.Size = new Size(402, 3);
      this.label9.TabIndex = 14;
      this.borderPanel1.Controls.Add((Control) this.gbSMTP);
      this.borderPanel1.Controls.Add((Control) this.pnlInfo);
      this.borderPanel1.Controls.Add((Control) this.pnlGlobalOptions);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(0, 0);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(535, 339);
      this.borderPanel1.TabIndex = 11;
      this.pnlInfo.Controls.Add((Control) this.lblInfo);
      this.pnlInfo.Dock = DockStyle.Top;
      this.pnlInfo.Location = new Point(1, 47);
      this.pnlInfo.Name = "pnlInfo";
      this.pnlInfo.Size = new Size(533, 36);
      this.pnlInfo.TabIndex = 10;
      this.AutoScroll = true;
      this.Controls.Add((Control) this.borderPanel1);
      this.Name = nameof (PersonalEmailSettingsPanel);
      this.Size = new Size(535, 339);
      this.pnlGlobalOptions.ResumeLayout(false);
      this.gbSMTP.ResumeLayout(false);
      this.pnlLogin.ResumeLayout(false);
      this.pnlLogin.PerformLayout();
      this.pnlServer.ResumeLayout(false);
      this.pnlServer.PerformLayout();
      this.pnlOverride.ResumeLayout(false);
      this.borderPanel1.ResumeLayout(false);
      this.pnlInfo.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void onGlobalOptionChanged(object sender, EventArgs e)
    {
      this.gbSMTP.Enabled = !this.radOutlook.Checked;
      this.chkSMTPUseCompany.Enabled = !this.radOutlook.Checked;
      this.setDirtyFlag(true);
    }

    private void txtSMTPPort_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
        return;
      e.Handled = true;
    }

    private void onSMTPSettingsChanged(object sender, EventArgs e)
    {
      if (this.txtSMTPServer.Text.Length > 0 && this.txtSMTPPort.Text.Length > 0)
        this.btnTestSMTP.Enabled = true;
      else
        this.btnTestSMTP.Enabled = false;
      this.setDirtyFlag(true);
    }

    public override void Save()
    {
      EmailDeliveryMethod emailDeliveryMethod = EmailDeliveryMethod.NotSpecified;
      if (this.radOutlook.Checked)
        emailDeliveryMethod = EmailDeliveryMethod.Outlook;
      else if (this.radSMTP.Checked)
        emailDeliveryMethod = EmailDeliveryMethod.SMTP;
      if (emailDeliveryMethod == EmailDeliveryMethod.SMTP && !this.chkSMTPUseCompany.Checked && (this.txtSMTPServer.Text.Trim() == "" || this.txtSMTPPort.Text.Trim() == ""))
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "You must provide a name and port number for your SMTP server.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        Session.WritePrivateProfileString("Mail", "DeliveryMethod", emailDeliveryMethod.ToString());
        bool flag = !this.chkSMTPUseCompany.Checked;
        Session.WritePrivateProfileString("Mail", "SMTPOverride", flag.ToString() ?? "");
        Session.WritePrivateProfileString("Mail", "SMTPServer", this.txtSMTPServer.Text);
        Session.WritePrivateProfileString("Mail", "SMTPPort", this.txtSMTPPort.Text);
        flag = this.chkSSL.Checked;
        Session.WritePrivateProfileString("Mail", "SMTPUseSSL", flag.ToString());
        Session.WritePrivateProfileString("Mail", "SMTPUserName", this.txtSMTPUserName.Text);
        Session.WritePrivateProfileString("Mail", "SMTPPassword", this.txtSMTPPassword.Text);
        this.setDirtyFlag(false);
      }
    }

    public override void Reset() => this.resetForm();

    private void showConfiguration(bool showOptions, bool showSMTP)
    {
      this.pnlGlobalOptions.Visible = showOptions;
      this.pnlInfo.Visible = !showSMTP && !showOptions;
      this.gbSMTP.Visible = showSMTP;
    }

    private void resetForm()
    {
      EmailDeliveryMethod emailDeliveryMethod = (EmailDeliveryMethod) Session.ServerManager.GetServerSetting("Mail.DeliveryMethod");
      bool serverSetting1 = (bool) Session.ServerManager.GetServerSetting("Mail.SMTPAllowOverride");
      bool serverSetting2 = (bool) Session.ServerManager.GetServerSetting("Mail.SMTPIndividualLogin");
      bool flag1 = string.Concat(Session.ServerManager.GetServerSetting("Mail.SMTPServer")) != "";
      bool flag2 = !flag1 || (Session.GetPrivateProfileString("Mail", "SMTPOverride") ?? "").ToLower() == "true";
      if (EnConfigurationSettings.GlobalSettings.RuntimeEnvironment == RuntimeEnvironment.Hosted)
        emailDeliveryMethod = EmailDeliveryMethod.SMTP;
      switch (emailDeliveryMethod)
      {
        case EmailDeliveryMethod.NotSpecified:
          this.showConfiguration(true, serverSetting1 | serverSetting2);
          try
          {
            emailDeliveryMethod = (EmailDeliveryMethod) System.Enum.Parse(typeof (EmailDeliveryMethod), Session.GetPrivateProfileString("Mail", "DeliveryMethod"), true);
            break;
          }
          catch
          {
            emailDeliveryMethod = EmailDeliveryMethod.Outlook;
            break;
          }
        case EmailDeliveryMethod.SMTP:
          this.lblInfo.Text = "Your account is configured to use the company's SMTP settings for email delivery. Contact your Encompass administrator for additional details.";
          this.showConfiguration(false, serverSetting1 | serverSetting2);
          break;
        default:
          this.lblInfo.Text = "Your account is configured to use Outlook integration for email delivery. Contact your Encompass administrator for additional details.";
          this.showConfiguration(false, false);
          break;
      }
      if (serverSetting1 & flag2)
      {
        this.txtSMTPServer.Text = Session.GetPrivateProfileString("Mail", "SMTPServer") ?? "";
        this.txtSMTPPort.Text = Session.GetPrivateProfileString("Mail", "SMTPPort") ?? "";
        this.txtSMTPUserName.Text = Session.GetPrivateProfileString("Mail", "SMTPUserName") ?? "";
        this.txtSMTPPassword.Text = Session.GetPrivateProfileString("Mail", "SMTPPassword") ?? "";
        this.chkSSL.Checked = Session.GetPrivateProfileString("Mail", "SMTPUseSSL") == "True";
        if (this.txtSMTPPort.Text == "")
          this.txtSMTPPort.Text = "25";
        this.chkSMTPUseCompany.Checked = false;
        this.txtSMTPServer.Enabled = true;
        this.txtSMTPPort.Enabled = true;
        this.chkSSL.Enabled = true;
        this.txtSMTPUserName.Enabled = true;
        this.txtSMTPPassword.Enabled = true;
        this.chkSSL.Enabled = true;
      }
      else
      {
        this.txtSMTPServer.Text = string.Concat(Session.ServerManager.GetServerSetting("Mail.SMTPServer"));
        this.txtSMTPPort.Text = string.Concat(Session.ServerManager.GetServerSetting("Mail.SMTPPort"));
        this.chkSSL.Checked = Session.ConfigurationManager.GetCompanySetting("Mail", "SMTPUseSSL") == "True";
        this.txtSMTPUserName.Text = "";
        this.txtSMTPPassword.Text = "";
        this.txtSMTPServer.Enabled = false;
        this.txtSMTPPort.Enabled = false;
        this.chkSSL.Enabled = false;
        this.txtSMTPUserName.Enabled = serverSetting2;
        this.txtSMTPPassword.Enabled = serverSetting2;
        this.chkSMTPUseCompany.Checked = true;
      }
      this.pnlOverride.Visible = serverSetting1 & flag1;
      if (emailDeliveryMethod == EmailDeliveryMethod.Outlook)
      {
        this.radOutlook.Checked = true;
      }
      else
      {
        if (emailDeliveryMethod != EmailDeliveryMethod.SMTP)
          return;
        this.radSMTP.Checked = true;
      }
    }

    private void btnTestSMTP_Click(object sender, EventArgs e)
    {
      if (Session.UserInfo.Email == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "An email address must be configured for your account in order to test SMTP mail delivery.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (!this.txtSMTPServer.Enabled && this.txtSMTPUserName.Enabled && this.txtSMTPUserName.Text.Trim() == "")
      {
        int num2 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "You must provide your SMTP login information in order to test your settings.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        string email = Session.UserInfo.Email;
        if (Utils.Dialog((IWin32Window) Form.ActiveForm, "A test email will be sent to " + email + " using the settings above. To continue this test, click OK.", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.OK)
          return;
        try
        {
          MailMessage message = new MailMessage();
          message.From = new MailAddress(email, Session.UserInfo.FullName);
          message.To.Add(new MailAddress(email, Session.UserInfo.FullName));
          message.Subject = "Encompass SMTP Server Test Message";
          message.Body = "This is a test message from Encompass. If you have received this message then your SMTP configuration settings are working correctly.";
          SmtpClient smtpClient = new SmtpClient();
          smtpClient.Host = this.txtSMTPServer.Text;
          smtpClient.Port = int.Parse(this.txtSMTPPort.Text);
          smtpClient.EnableSsl = this.chkSSL.Checked;
          string userName = this.txtSMTPUserName.Enabled ? this.txtSMTPUserName.Text : Session.ServerManager.GetServerSetting("Mail.SMTPUserName").ToString();
          string password = this.txtSMTPPassword.Enabled ? this.txtSMTPPassword.Text : Session.ServerManager.GetServerSetting("Mail.SMTPPassword").ToString();
          if (userName != "")
            smtpClient.Credentials = (ICredentialsByHost) new NetworkCredential(userName, password);
          smtpClient.Send(message);
          int num3 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "A test email has been successfully sent to " + email + ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        catch (Exception ex)
        {
          int num4 = (int) Utils.Dialog((IWin32Window) this, "The test has failed for the following reason: " + ex.Message + "." + Environment.NewLine + "Ensure that the information provided above is correct and try again.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
    }

    private void chkSMTPUseCompany_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkSMTPUseCompany.Checked)
      {
        this.txtSMTPServer.Text = string.Concat(Session.ServerManager.GetServerSetting("Mail.SMTPServer"));
        this.txtSMTPPort.Text = string.Concat(Session.ServerManager.GetServerSetting("Mail.SMTPPort"));
        this.chkSSL.Checked = Session.ConfigurationManager.GetCompanySetting("Mail", "SMTPUseSSL") == "True";
        this.txtSMTPServer.Enabled = false;
        this.txtSMTPPort.Enabled = false;
        this.chkSSL.Enabled = false;
        if ((bool) Session.ServerManager.GetServerSetting("Mail.SMTPIndividualLogin"))
        {
          this.txtSMTPUserName.Enabled = true;
          this.txtSMTPPassword.Enabled = true;
          this.txtSMTPUserName.Text = Session.GetPrivateProfileString("Mail", "SMTPUserName") ?? "";
          this.txtSMTPPassword.Text = Session.GetPrivateProfileString("Mail", "SMTPPassword") ?? "";
        }
        else
        {
          this.txtSMTPUserName.Enabled = false;
          this.txtSMTPPassword.Enabled = false;
          this.txtSMTPUserName.Text = "";
          this.txtSMTPPassword.Text = "";
        }
      }
      else
      {
        this.txtSMTPServer.Text = Session.GetPrivateProfileString("Mail", "SMTPServer") ?? "";
        this.txtSMTPPort.Text = Session.GetPrivateProfileString("Mail", "SMTPPort") ?? "";
        this.txtSMTPUserName.Text = Session.GetPrivateProfileString("Mail", "SMTPUserName") ?? "";
        this.txtSMTPPassword.Text = Session.GetPrivateProfileString("Mail", "SMTPPassword") ?? "";
        this.chkSSL.Checked = Session.GetPrivateProfileString("Mail", "SMTPUseSSL") == "True";
        this.txtSMTPServer.Enabled = true;
        this.txtSMTPPort.Enabled = true;
        this.chkSSL.Enabled = true;
        this.txtSMTPUserName.Enabled = true;
        this.txtSMTPPassword.Enabled = true;
      }
      this.setDirtyFlag(true);
    }

    private void settingsChanged(object sender, EventArgs e) => this.setDirtyFlag(true);
  }
}
