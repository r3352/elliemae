// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EmailSettingsPanel
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
  public class EmailSettingsPanel : SettingsUserControl
  {
    private RadioButton radOutlook;
    private RadioButton radSMTP;
    private RadioButton radAny;
    private Label label1;
    private Label label2;
    private Label label3;
    private TextBox txtSMTPServer;
    private TextBox txtSMTPPort;
    private Label label4;
    private Label label5;
    private TextBox txtSMTPUserName;
    private TextBox txtSMTPPassword;
    private Label label6;
    private Button btnTestSMTP;
    private Panel pnlBottom;
    private System.ComponentModel.Container components;
    private CheckBox chkAllowOverride;
    private GroupContainer gbSMTP;
    private GradientPanel gPnlGlobalOptions;
    private CheckBox chkSSL;
    private CheckBox chkIndividualLogin;

    public EmailSettingsPanel(SetUpContainer setupContainer)
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
      this.radOutlook = new RadioButton();
      this.radSMTP = new RadioButton();
      this.radAny = new RadioButton();
      this.chkIndividualLogin = new CheckBox();
      this.chkAllowOverride = new CheckBox();
      this.btnTestSMTP = new Button();
      this.txtSMTPPassword = new TextBox();
      this.label6 = new Label();
      this.txtSMTPUserName = new TextBox();
      this.label4 = new Label();
      this.label5 = new Label();
      this.txtSMTPPort = new TextBox();
      this.txtSMTPServer = new TextBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.gPnlGlobalOptions = new GradientPanel();
      this.pnlBottom = new Panel();
      this.gbSMTP = new GroupContainer();
      this.chkSSL = new CheckBox();
      this.gPnlGlobalOptions.SuspendLayout();
      this.pnlBottom.SuspendLayout();
      this.gbSMTP.SuspendLayout();
      this.SuspendLayout();
      this.radOutlook.BackColor = Color.Transparent;
      this.radOutlook.Location = new Point(11, 9);
      this.radOutlook.Name = "radOutlook";
      this.radOutlook.Size = new Size(264, 18);
      this.radOutlook.TabIndex = 0;
      this.radOutlook.Text = "Use Outlook for all users";
      this.radOutlook.UseVisualStyleBackColor = false;
      this.radOutlook.CheckedChanged += new EventHandler(this.onGlobalOptionChanged);
      this.radSMTP.BackColor = Color.Transparent;
      this.radSMTP.Location = new Point(11, 26);
      this.radSMTP.Name = "radSMTP";
      this.radSMTP.Size = new Size(264, 18);
      this.radSMTP.TabIndex = 1;
      this.radSMTP.Text = "Use SMTP for all users";
      this.radSMTP.UseVisualStyleBackColor = false;
      this.radSMTP.CheckedChanged += new EventHandler(this.onGlobalOptionChanged);
      this.radAny.BackColor = Color.Transparent;
      this.radAny.Location = new Point(11, 43);
      this.radAny.Name = "radAny";
      this.radAny.Size = new Size(264, 18);
      this.radAny.TabIndex = 2;
      this.radAny.Text = "Allow Outlook or SMTP configuration by user";
      this.radAny.UseVisualStyleBackColor = false;
      this.radAny.CheckedChanged += new EventHandler(this.onGlobalOptionChanged);
      this.chkIndividualLogin.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.chkIndividualLogin.Location = new Point(155, 218);
      this.chkIndividualLogin.Name = "chkIndividualLogin";
      this.chkIndividualLogin.Size = new Size(248, 20);
      this.chkIndividualLogin.TabIndex = 23;
      this.chkIndividualLogin.Text = "Users must enter their own login information";
      this.chkIndividualLogin.CheckedChanged += new EventHandler(this.chkIndividualLogin_CheckedChanged);
      this.chkAllowOverride.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.chkAllowOverride.Location = new Point(155, 98);
      this.chkAllowOverride.Name = "chkAllowOverride";
      this.chkAllowOverride.Size = new Size(248, 20);
      this.chkAllowOverride.TabIndex = 22;
      this.chkAllowOverride.Text = "Allow users to enter personal SMTP settings";
      this.chkAllowOverride.CheckedChanged += new EventHandler(this.settingsChanged);
      this.btnTestSMTP.BackColor = SystemColors.Control;
      this.btnTestSMTP.Enabled = false;
      this.btnTestSMTP.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnTestSMTP.Location = new Point(311, 196);
      this.btnTestSMTP.Name = "btnTestSMTP";
      this.btnTestSMTP.Size = new Size(66, 20);
      this.btnTestSMTP.TabIndex = 12;
      this.btnTestSMTP.Text = "&Test";
      this.btnTestSMTP.UseVisualStyleBackColor = true;
      this.btnTestSMTP.Click += new EventHandler(this.btnTestSMTP_Click);
      this.txtSMTPPassword.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtSMTPPassword.Location = new Point(155, 196);
      this.txtSMTPPassword.MaxLength = 50;
      this.txtSMTPPassword.Name = "txtSMTPPassword";
      this.txtSMTPPassword.PasswordChar = '*';
      this.txtSMTPPassword.Size = new Size(150, 20);
      this.txtSMTPPassword.TabIndex = 9;
      this.txtSMTPPassword.TextChanged += new EventHandler(this.onSMTPSettingsChanged);
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(3, 198);
      this.label6.Name = "label6";
      this.label6.Size = new Size(122, 16);
      this.label6.TabIndex = 8;
      this.label6.Text = "Password";
      this.txtSMTPUserName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtSMTPUserName.Location = new Point(155, 174);
      this.txtSMTPUserName.MaxLength = 50;
      this.txtSMTPUserName.Name = "txtSMTPUserName";
      this.txtSMTPUserName.Size = new Size(150, 20);
      this.txtSMTPUserName.TabIndex = 7;
      this.txtSMTPUserName.TextChanged += new EventHandler(this.onSMTPSettingsChanged);
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(3, 176);
      this.label4.Name = "label4";
      this.label4.Size = new Size(122, 16);
      this.label4.TabIndex = 6;
      this.label4.Text = "User Name";
      this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(3, 150);
      this.label5.Name = "label5";
      this.label5.Size = new Size(170, 16);
      this.label5.TabIndex = 5;
      this.label5.Text = "Login Information";
      this.txtSMTPPort.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtSMTPPort.Location = new Point(155, 75);
      this.txtSMTPPort.MaxLength = 5;
      this.txtSMTPPort.Name = "txtSMTPPort";
      this.txtSMTPPort.Size = new Size(150, 20);
      this.txtSMTPPort.TabIndex = 4;
      this.txtSMTPPort.Text = "25";
      this.txtSMTPPort.TextChanged += new EventHandler(this.onSMTPSettingsChanged);
      this.txtSMTPPort.KeyPress += new KeyPressEventHandler(this.txtSMTPPort_KeyPress);
      this.txtSMTPServer.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtSMTPServer.Location = new Point(155, 53);
      this.txtSMTPServer.MaxLength = 100;
      this.txtSMTPServer.Name = "txtSMTPServer";
      this.txtSMTPServer.Size = new Size(150, 20);
      this.txtSMTPServer.TabIndex = 3;
      this.txtSMTPServer.TextChanged += new EventHandler(this.onSMTPSettingsChanged);
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(3, 78);
      this.label3.Name = "label3";
      this.label3.Size = new Size(26, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Port";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(3, 56);
      this.label2.Name = "label2";
      this.label2.Size = new Size(145, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Outgoing Mail Server (SMTP)";
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(3, 31);
      this.label1.Name = "label1";
      this.label1.Size = new Size(170, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "Server Information";
      this.gPnlGlobalOptions.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gPnlGlobalOptions.Controls.Add((Control) this.radSMTP);
      this.gPnlGlobalOptions.Controls.Add((Control) this.radAny);
      this.gPnlGlobalOptions.Controls.Add((Control) this.radOutlook);
      this.gPnlGlobalOptions.Dock = DockStyle.Top;
      this.gPnlGlobalOptions.GradientColor1 = Color.WhiteSmoke;
      this.gPnlGlobalOptions.GradientColor2 = Color.WhiteSmoke;
      this.gPnlGlobalOptions.Location = new Point(0, 0);
      this.gPnlGlobalOptions.Name = "gPnlGlobalOptions";
      this.gPnlGlobalOptions.Size = new Size(530, 67);
      this.gPnlGlobalOptions.TabIndex = 3;
      this.pnlBottom.Controls.Add((Control) this.gbSMTP);
      this.pnlBottom.Dock = DockStyle.Fill;
      this.pnlBottom.Location = new Point(0, 67);
      this.pnlBottom.Name = "pnlBottom";
      this.pnlBottom.Size = new Size(530, 298);
      this.pnlBottom.TabIndex = 6;
      this.gbSMTP.Controls.Add((Control) this.chkSSL);
      this.gbSMTP.Controls.Add((Control) this.chkIndividualLogin);
      this.gbSMTP.Controls.Add((Control) this.txtSMTPPassword);
      this.gbSMTP.Controls.Add((Control) this.chkAllowOverride);
      this.gbSMTP.Controls.Add((Control) this.label1);
      this.gbSMTP.Controls.Add((Control) this.btnTestSMTP);
      this.gbSMTP.Controls.Add((Control) this.label2);
      this.gbSMTP.Controls.Add((Control) this.label3);
      this.gbSMTP.Controls.Add((Control) this.label6);
      this.gbSMTP.Controls.Add((Control) this.txtSMTPServer);
      this.gbSMTP.Controls.Add((Control) this.txtSMTPUserName);
      this.gbSMTP.Controls.Add((Control) this.txtSMTPPort);
      this.gbSMTP.Controls.Add((Control) this.label4);
      this.gbSMTP.Controls.Add((Control) this.label5);
      this.gbSMTP.Dock = DockStyle.Fill;
      this.gbSMTP.HeaderForeColor = SystemColors.ControlText;
      this.gbSMTP.Location = new Point(0, 0);
      this.gbSMTP.Name = "gbSMTP";
      this.gbSMTP.Size = new Size(530, 298);
      this.gbSMTP.TabIndex = 0;
      this.gbSMTP.Text = "SMTP Settings";
      this.chkSSL.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.chkSSL.Location = new Point(155, 117);
      this.chkSSL.Name = "chkSSL";
      this.chkSSL.Size = new Size(248, 20);
      this.chkSSL.TabIndex = 24;
      this.chkSSL.Text = "Use SSL for secure communication";
      this.chkSSL.CheckedChanged += new EventHandler(this.settingsChanged);
      this.AutoScroll = true;
      this.Controls.Add((Control) this.pnlBottom);
      this.Controls.Add((Control) this.gPnlGlobalOptions);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (EmailSettingsPanel);
      this.Size = new Size(530, 365);
      this.gPnlGlobalOptions.ResumeLayout(false);
      this.pnlBottom.ResumeLayout(false);
      this.gbSMTP.ResumeLayout(false);
      this.gbSMTP.PerformLayout();
      this.ResumeLayout(false);
    }

    private void onGlobalOptionChanged(object sender, EventArgs e)
    {
      this.gbSMTP.Enabled = !this.radOutlook.Checked;
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
      this.enableDisableTestButton();
      this.setDirtyFlag(true);
    }

    private void enableDisableTestButton()
    {
      if (!this.chkIndividualLogin.Checked)
      {
        if (this.txtSMTPServer.Text.Length > 0 && this.txtSMTPPort.Text.Length > 0)
          this.btnTestSMTP.Enabled = true;
        else
          this.btnTestSMTP.Enabled = false;
      }
      else
        this.btnTestSMTP.Enabled = false;
    }

    public override void Save()
    {
      EmailDeliveryMethod emailDeliveryMethod = EmailDeliveryMethod.NotSpecified;
      if (this.radOutlook.Checked)
        emailDeliveryMethod = EmailDeliveryMethod.Outlook;
      else if (this.radSMTP.Checked)
        emailDeliveryMethod = EmailDeliveryMethod.SMTP;
      if (this.txtSMTPPort.Text == "")
        this.txtSMTPPort.Text = "25";
      if (emailDeliveryMethod != EmailDeliveryMethod.Outlook && this.txtSMTPServer.Text.Trim() == "" && !this.chkAllowOverride.Checked)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "You must specify a default SMTP Server or enable the 'Allow users to provide custom SMTP settings' option.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        Session.ServerManager.UpdateServerSetting("Mail.DeliveryMethod", (object) emailDeliveryMethod);
        Session.ServerManager.UpdateServerSetting("Mail.SMTPServer", (object) this.txtSMTPServer.Text.Trim());
        Session.ServerManager.UpdateServerSetting("Mail.SMTPPort", (object) int.Parse(this.txtSMTPPort.Text.Trim()));
        Session.ServerManager.UpdateServerSetting("Mail.SMTPUserName", (object) this.txtSMTPUserName.Text.Trim());
        Session.ServerManager.UpdateServerSetting("Mail.SMTPPassword", (object) this.txtSMTPPassword.Text);
        Session.ServerManager.UpdateServerSetting("Mail.SMTPAllowOverride", (object) this.chkAllowOverride.Checked);
        Session.ServerManager.UpdateServerSetting("Mail.SMTPIndividualLogin", (object) this.chkIndividualLogin.Checked);
        Session.ConfigurationManager.SetCompanySetting("Mail", "SMTPUseSSL", this.chkSSL.Checked.ToString());
        this.setDirtyFlag(false);
      }
    }

    public override void Reset()
    {
      this.resetForm();
      this.setDirtyFlag(false);
    }

    private void resetForm()
    {
      EmailDeliveryMethod emailDeliveryMethod = (EmailDeliveryMethod) Session.ServerManager.GetServerSetting("Mail.DeliveryMethod");
      if (EnConfigurationSettings.GlobalSettings.RuntimeEnvironment == RuntimeEnvironment.Hosted)
      {
        emailDeliveryMethod = EmailDeliveryMethod.SMTP;
        this.gPnlGlobalOptions.Visible = false;
        this.pnlBottom.Top = this.gPnlGlobalOptions.Top;
      }
      switch (emailDeliveryMethod)
      {
        case EmailDeliveryMethod.Outlook:
          this.radOutlook.Checked = true;
          break;
        case EmailDeliveryMethod.SMTP:
          this.radSMTP.Checked = true;
          break;
        default:
          this.radAny.Checked = true;
          break;
      }
      this.txtSMTPServer.Text = string.Concat(Session.ServerManager.GetServerSetting("Mail.SMTPServer"));
      this.txtSMTPPort.Text = string.Concat(Session.ServerManager.GetServerSetting("Mail.SMTPPort"));
      this.txtSMTPUserName.Text = string.Concat(Session.ServerManager.GetServerSetting("Mail.SMTPUserName"));
      this.txtSMTPPassword.Text = string.Concat(Session.ServerManager.GetServerSetting("Mail.SMTPPassword"));
      this.chkIndividualLogin.Checked = (bool) Session.ServerManager.GetServerSetting("Mail.SMTPIndividualLogin");
      this.chkAllowOverride.Checked = (bool) Session.ServerManager.GetServerSetting("Mail.SMTPAllowOverride");
      this.chkSSL.Checked = Session.ConfigurationManager.GetCompanySetting("Mail", "SMTPUseSSL").ToLower() == "true";
    }

    private void btnTestSMTP_Click(object sender, EventArgs e)
    {
      if (Session.UserInfo.Email == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "An email address must be configured for your account in order to test SMTP mail delivery.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
          if (this.txtSMTPUserName.Text != "")
            smtpClient.Credentials = (ICredentialsByHost) new NetworkCredential(this.txtSMTPUserName.Text, this.txtSMTPPassword.Text);
          smtpClient.Send(message);
          int num2 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "A test email has been successfully sent to " + email + ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        catch (Exception ex)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "The test has failed for the following reason: " + ex.Message + "." + Environment.NewLine + "Ensure that the information provided above is correct and try again.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
    }

    private void chkIndividualLogin_CheckedChanged(object sender, EventArgs e)
    {
      this.txtSMTPUserName.Enabled = !this.chkIndividualLogin.Checked;
      this.txtSMTPPassword.Enabled = !this.chkIndividualLogin.Checked;
      this.enableDisableTestButton();
      this.setDirtyFlag(true);
    }

    private void settingsChanged(object sender, EventArgs e) => this.setDirtyFlag(true);
  }
}
