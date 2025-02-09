// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.ERDBRegistrationForm
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class ERDBRegistrationForm : Form
  {
    private const string failureNotification = "ERDBFailureNotification";
    private bool doNotUpdateTextBoxesChanged = true;
    private bool textBoxesForTestConnectionsChanged;
    private IContainer components;
    private Button btnRegister;
    private Button btnCancel;
    private Label label1;
    private Label label2;
    private Label label4;
    private Label label5;
    private Label label6;
    private Label label7;
    private TextBox textBoxERDBAppServer;
    private TextBox textBoxERDBAppServerPort;
    private TextBox textBoxERDBServer;
    private TextBox textBoxERDBName;
    private TextBox textBoxERDBLogin;
    private TextBox textBoxERDBPwd;
    private Label label8;
    private TextBox textBoxERDBPwd2;
    private GroupBox groupBox1;
    private Label label3;
    private Label label9;
    private Label label10;
    private Label label11;
    private Label label12;
    private TextBox textBoxSMTPServer;
    private TextBox textBoxSMTPPort;
    private TextBox textBoxSMTPUserName;
    private TextBox textBoxSMTPPassword;
    private Button btnTestEmail;
    private GroupBox groupBox2;
    private Label label13;
    private TextBox textBoxEmail;
    private Label label14;
    private TextBox textBoxNotificationEmailInterval;
    private Label label15;
    private Button btnCreateDbAndRegister;
    private Label label16;
    private TextBox textBoxEncDataDir;
    private GroupBox groupBox3;
    private Button btnTestConnections;
    private TextBox textBoxFrom;
    private Label label17;
    private TextBox textBoxSMTPPwd2;
    private Label label18;
    private CheckBox chkBoxUseSSL;
    private Label label19;
    private Label label20;
    private Button SaveNotificationSettings;

    public ERDBRegistrationForm()
    {
      this.InitializeComponent();
      this.doNotUpdateTextBoxesChanged = true;
      try
      {
        string clientId = Session.CompanyInfo.ClientID;
        Dictionary<string, object> registrationInfo = Session.EncERDBRegMgr.GetERDBRegistrationInfo();
        this.textBoxERDBAppServer.Text = (string) registrationInfo["AppServer"] ?? "";
        this.textBoxERDBAppServerPort.Text = "11099";
        try
        {
          string s = (string) registrationInfo["Port"];
          if (s != null)
            this.textBoxERDBAppServerPort.Text = string.Concat((object) int.Parse(s));
        }
        catch
        {
        }
        this.textBoxEncDataDir.Text = (string) registrationInfo["EncDataDir"] ?? "";
        this.textBoxERDBServer.Text = (string) registrationInfo["DbServer"] ?? "";
        this.textBoxERDBName.Text = (string) registrationInfo["DbName"] ?? "";
        this.textBoxERDBLogin.Text = (string) registrationInfo["DbLogin"] ?? "";
        this.textBoxERDBPwd.Text = (string) registrationInfo["DbPwd"] ?? "";
        this.textBoxERDBPwd2.Text = this.textBoxERDBPwd.Text;
        this.textBox_TextChanged((object) this, (EventArgs) null);
        IDictionary serverSettings = Session.ServerManager.GetServerSettings("ERDBFailureNotification");
        this.textBoxSMTPServer.Text = string.Concat(serverSettings[(object) "ERDBFailureNotification.SMTPServer"]);
        if (string.Concat(serverSettings[(object) "ERDBFailureNotification.SMTPPort"]).Trim() != "")
          this.textBoxSMTPPort.Text = string.Concat(serverSettings[(object) "ERDBFailureNotification.SMTPPort"]);
        else
          this.textBoxSMTPPort.Text = "25";
        this.textBoxSMTPUserName.Text = string.Concat(serverSettings[(object) "ERDBFailureNotification.SMTPUserName"]);
        try
        {
          this.textBoxSMTPPassword.Text = XT.DSB64(string.Concat(serverSettings[(object) "ERDBFailureNotification.SMTPPassword"]), KB.KB64) ?? "";
        }
        catch
        {
          this.textBoxSMTPPassword.Text = "";
        }
        this.textBoxSMTPPwd2.Text = this.textBoxSMTPPassword.Text;
        this.chkBoxUseSSL.Checked = (bool) serverSettings[(object) "ERDBFailureNotification.SMTPUseSSL"];
        this.textBoxFrom.Text = string.Concat(serverSettings[(object) "ERDBFailureNotification.FromEmail"]);
        this.textBoxEmail.Text = string.Concat(serverSettings[(object) "ERDBFailureNotification.Email"]);
        if (string.Concat(serverSettings[(object) "ERDBFailureNotification.EmailDeliveryInterval"]).Trim() != "")
          this.textBoxNotificationEmailInterval.Text = string.Concat(serverSettings[(object) "ERDBFailureNotification.EmailDeliveryInterval"]);
        else
          this.textBoxNotificationEmailInterval.Text = "12";
      }
      finally
      {
        this.doNotUpdateTextBoxesChanged = false;
      }
    }

    private void btnRegister_Click(object sender, EventArgs e) => this.registerERDB(sender, true);

    private void registerERDB(object sender, bool showStopServerMsg)
    {
      if (showStopServerMsg && Utils.Dialog((IWin32Window) this, "You must stop the Encompass Server before you can register External Reporting Database. Have you stopped the Encompass Server?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.No)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please stop the Encompass Server before you register External Reporting Database.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.DialogResult = DialogResult.None;
      }
      else
      {
        string text1 = this.textBoxERDBAppServer.Text;
        int erdbAppServerPort;
        try
        {
          erdbAppServerPort = int.Parse(this.textBoxERDBAppServerPort.Text);
        }
        catch (Exception ex)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Invalid ERDB App Server Port: " + ex.Message + "\r\n\r\nPlease re-enter the port.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.DialogResult = DialogResult.None;
          return;
        }
        string text2 = this.textBoxERDBServer.Text;
        string text3 = this.textBoxERDBName.Text;
        string text4 = this.textBoxERDBLogin.Text;
        string text5 = this.textBoxERDBPwd.Text;
        string text6 = this.textBoxERDBPwd2.Text;
        string text7 = this.textBoxEncDataDir.Text;
        string text8 = this.textBoxSMTPServer.Text;
        int smtpPort;
        try
        {
          smtpPort = int.Parse(this.textBoxSMTPPort.Text);
        }
        catch (Exception ex)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Invalid SMTP Server Port: " + ex.Message + "\r\n\r\nPlease re-enter the port.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.DialogResult = DialogResult.None;
          return;
        }
        string text9 = this.textBoxSMTPUserName.Text;
        string text10 = this.textBoxSMTPPassword.Text;
        string text11 = this.textBoxSMTPPwd2.Text;
        if (text10 != text11)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "SMTP server passwords mismatch. Please enter SMTP server passwords again.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.DialogResult = DialogResult.None;
        }
        else
        {
          bool smtpUseSSL = this.chkBoxUseSSL.Checked;
          string text12 = this.textBoxFrom.Text;
          string text13 = this.textBoxEmail.Text;
          int emailDeliveryInterval;
          try
          {
            emailDeliveryInterval = int.Parse(this.textBoxNotificationEmailInterval.Text);
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Invalid numerical number: " + ex.Message + "\r\n\r\nPlease re-enter the time interval.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            this.DialogResult = DialogResult.None;
            return;
          }
          if (text5 != text6)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "ERDB passwords mismatch. Please enter DB passwords again.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            this.DialogResult = DialogResult.None;
          }
          else if (text9.Trim() != "" && text10.Trim() != text11.Trim())
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "SMTP passwords mismatch. Please enter SMTP passwords again.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            this.DialogResult = DialogResult.None;
          }
          else if (this.textBoxEncDataDir.Text.Trim() == "")
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Please enter the EncompassData folder accessed from the Encompass ERDB Server.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            this.DialogResult = DialogResult.None;
          }
          else
          {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
              string saPwd = (string) null;
              if (sender as Button == this.btnCreateDbAndRegister)
              {
                ERDBPromptForSaPwd erdbPromptForSaPwd = new ERDBPromptForSaPwd();
                if (erdbPromptForSaPwd.ShowDialog() == DialogResult.Cancel)
                {
                  this.DialogResult = DialogResult.None;
                  return;
                }
                saPwd = erdbPromptForSaPwd.SAPwd;
              }
              string str = Session.SessionObjects.EncERDBRegMgr.Register(saPwd, text1, erdbAppServerPort, text2, text3, text4, text5, text7, text8, smtpPort, text9, text10, smtpUseSSL, text12, text13, emailDeliveryInterval);
              if (str == null)
                return;
              if (Utils.Dialog((IWin32Window) this, str + "\r\n\r\nTry again?", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                this.registerERDB(sender, false);
              else
                this.DialogResult = DialogResult.None;
            }
            catch (Exception ex)
            {
              int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
              this.DialogResult = DialogResult.None;
            }
            finally
            {
              Cursor.Current = Cursors.Default;
            }
          }
        }
      }
    }

    private void integerTextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
        return;
      e.Handled = true;
    }

    private void btnTestEmail_Click(object sender, EventArgs e)
    {
      if (this.textBoxEmail.Text.Trim() == "" || this.textBoxFrom.Text.Trim() == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "An email address must be configured in order to test SMTP mail delivery.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (this.textBoxSMTPUserName.Text.Trim() != "" && this.textBoxSMTPPassword.Text.Trim() != this.textBoxSMTPPwd2.Text.Trim())
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "SMTP passwords mismatch. Please enter SMTP passwords again.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        string text1 = this.textBoxFrom.Text;
        string text2 = this.textBoxEmail.Text;
        if (Utils.Dialog((IWin32Window) this, "A test email will be sent to '" + text2 + "' using the settings entered. To continue this test, click OK.", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.OK)
          return;
        Cursor.Current = Cursors.WaitCursor;
        try
        {
          string smtpHost = this.textBoxSMTPServer.Text.Trim();
          int smtpPort = int.Parse(this.textBoxSMTPPort.Text.Trim());
          string subject = "Encompass ERDB SMTP Server (" + smtpHost + ":" + (object) smtpPort + ") Test Message";
          string body = "This is a test message from Encompass. If you received this message it means that your ERDB SMTP (" + smtpHost + ":" + (object) smtpPort + ") configuration settings are working correctly.";
          SmtpUtil.SendMail(smtpHost, smtpPort, this.textBoxSMTPUserName.Text.Trim(), this.textBoxSMTPPassword.Text, text1, text2, subject, body, false, this.chkBoxUseSSL.Checked);
          int num3 = (int) Utils.Dialog((IWin32Window) this, "A test email has been successfully sent to " + text2 + ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        catch (Exception ex)
        {
          string message = ex.Message;
          if (ex.InnerException != null)
            message = ex.InnerException.Message;
          int num4 = (int) Utils.Dialog((IWin32Window) this, "The test has failed for the following reason: " + message + Environment.NewLine + "Ensure that the information provided is correct and try again.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        finally
        {
          Cursor.Current = Cursors.Default;
        }
      }
    }

    private void textBox_TextChanged(object sender, EventArgs e)
    {
      string str1 = this.textBoxERDBAppServer.Text.Trim();
      string str2 = this.textBoxERDBAppServerPort.Text.Trim();
      string str3 = this.textBoxEncDataDir.Text.Trim();
      string str4 = this.textBoxERDBServer.Text.Trim();
      string str5 = this.textBoxERDBName.Text.Trim();
      string str6 = this.textBoxERDBLogin.Text.Trim();
      string str7 = this.textBoxERDBPwd.Text.Trim();
      string str8 = this.textBoxERDBPwd2.Text.Trim();
      if (str1 == "" || str2 == "" || str3 == "" || str4 == "" || str5 == "" || str6 == "" || str7 == "" || str8 == "")
      {
        this.btnCreateDbAndRegister.Enabled = this.btnRegister.Enabled = false;
      }
      else
      {
        this.btnCreateDbAndRegister.Enabled = this.btnRegister.Enabled = true;
        if (this.doNotUpdateTextBoxesChanged)
          return;
        this.textBoxesForTestConnectionsChanged = true;
      }
    }

    private void btnTestConnections_Click(object sender, EventArgs e)
    {
      try
      {
        this.doNotUpdateTextBoxesChanged = true;
        this.textBox_TextChanged(sender, e);
      }
      finally
      {
        this.doNotUpdateTextBoxesChanged = false;
      }
      if (!(bool) Session.ServerManager.GetServerSetting("Components.UseERDB"))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "External Reporting Database is not enabled. Please register it first.");
      }
      else if (this.textBoxesForTestConnectionsChanged)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You have made some changes. Please register first before testing the connections.");
      }
      else
      {
        Cursor.Current = Cursors.WaitCursor;
        try
        {
          Dictionary<string, string> results = new Dictionary<string, string>();
          Dictionary<string, string> dictionary = (Dictionary<string, string>) null;
          string text = this.textBoxERDBAppServer.Text;
          int erdbServerPort;
          try
          {
            erdbServerPort = int.Parse(this.textBoxERDBAppServerPort.Text);
          }
          catch
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "Invalid ERDB server port: " + this.textBoxERDBAppServerPort.Text);
            return;
          }
          try
          {
            dictionary = Session.EncERDBRegMgr.CheckConnections(text, erdbServerPort);
            results.Add("Encompass -> ERDB Server", "OK");
          }
          catch (Exception ex)
          {
            results.Add("Encompass -> ERDB Server", "Error connecting to ERDB Server: " + ex.Message);
          }
          int num4 = 10;
          while (num4 > 0)
          {
            try
            {
              if (Session.LoanManager.GetLoanXDBStatus(true) == null)
              {
                results.Add("Encompass -> ERDB SQL Database", "Getting null loan XDB status info");
                break;
              }
              results.Add("Encompass -> ERDB SQL Database", "OK");
              break;
            }
            catch (Exception ex)
            {
              if (--num4 > 0 && ex.InnerException is SqlException)
              {
                Thread.Sleep(1000);
              }
              else
              {
                results.Add("Encompass -> ERDB SQL Database", "Error accessing ERDB SQL database: " + ex.Message);
                break;
              }
            }
          }
          if (dictionary != null)
          {
            foreach (string key in dictionary.Keys)
              results.Add(key, dictionary[key]);
          }
          else
          {
            results.Add("ERDB Server -> ERDB SQL Database", "Unknown");
            results.Add("ERDB Server -> Encompass SQL Database", "Unknown");
            results.Add("ERDB Server -> EncompassData Folder", "Unknonw");
          }
          int num5 = (int) new ERDBConnectionTestResults(results).ShowDialog((IWin32Window) this);
        }
        finally
        {
          Cursor.Current = Cursors.Default;
        }
      }
    }

    private void SaveNotificationSettings_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        string text1 = this.textBoxSMTPServer.Text;
        int smtpServerPort;
        try
        {
          smtpServerPort = int.Parse(this.textBoxSMTPPort.Text);
        }
        catch (Exception ex)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Invalid SMTP Server Port: " + ex.Message + "\r\n\r\nPlease re-enter the port.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.DialogResult = DialogResult.None;
          return;
        }
        string text2 = this.textBoxSMTPUserName.Text;
        string text3 = this.textBoxSMTPPassword.Text;
        string text4 = this.textBoxSMTPPwd2.Text;
        if (text3 != text4)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "SMTP server passwords mismatch. Please enter SMTP server passwords again.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.DialogResult = DialogResult.None;
        }
        else
        {
          bool smtpUseSSL = this.chkBoxUseSSL.Checked;
          string text5 = this.textBoxFrom.Text;
          string text6 = this.textBoxEmail.Text;
          int emailDeliveryInterval;
          try
          {
            emailDeliveryInterval = int.Parse(this.textBoxNotificationEmailInterval.Text);
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Invalid numerical number: " + ex.Message + "\r\n\r\nPlease re-enter the time interval.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            this.DialogResult = DialogResult.None;
            return;
          }
          Session.EncERDBRegMgr.UpdateNotificationSettings(text1, smtpServerPort, text2, text3, smtpUseSSL, text5, text6, emailDeliveryInterval);
          int num1 = (int) Utils.Dialog((IWin32Window) this, "Failure notification settings have been updated. No need to stop and start Encompass server.");
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error saving failure notification settings: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.DialogResult = DialogResult.None;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ERDBRegistrationForm));
      this.btnRegister = new Button();
      this.btnCancel = new Button();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.textBoxERDBAppServer = new TextBox();
      this.textBoxERDBAppServerPort = new TextBox();
      this.textBoxERDBServer = new TextBox();
      this.textBoxERDBName = new TextBox();
      this.textBoxERDBLogin = new TextBox();
      this.textBoxERDBPwd = new TextBox();
      this.label8 = new Label();
      this.textBoxERDBPwd2 = new TextBox();
      this.groupBox1 = new GroupBox();
      this.label3 = new Label();
      this.label9 = new Label();
      this.label10 = new Label();
      this.label11 = new Label();
      this.label12 = new Label();
      this.textBoxSMTPServer = new TextBox();
      this.textBoxSMTPPort = new TextBox();
      this.textBoxSMTPUserName = new TextBox();
      this.textBoxSMTPPassword = new TextBox();
      this.btnTestEmail = new Button();
      this.groupBox2 = new GroupBox();
      this.label13 = new Label();
      this.textBoxEmail = new TextBox();
      this.label14 = new Label();
      this.textBoxNotificationEmailInterval = new TextBox();
      this.label15 = new Label();
      this.btnCreateDbAndRegister = new Button();
      this.label16 = new Label();
      this.textBoxEncDataDir = new TextBox();
      this.groupBox3 = new GroupBox();
      this.btnTestConnections = new Button();
      this.textBoxFrom = new TextBox();
      this.label17 = new Label();
      this.textBoxSMTPPwd2 = new TextBox();
      this.label18 = new Label();
      this.chkBoxUseSSL = new CheckBox();
      this.label19 = new Label();
      this.label20 = new Label();
      this.SaveNotificationSettings = new Button();
      this.SuspendLayout();
      this.btnRegister.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnRegister.DialogResult = DialogResult.OK;
      this.btnRegister.Location = new Point(362, 478);
      this.btnRegister.Name = "btnRegister";
      this.btnRegister.Size = new Size(91, 23);
      this.btnRegister.TabIndex = 21;
      this.btnRegister.Text = "Register Only";
      this.btnRegister.UseVisualStyleBackColor = true;
      this.btnRegister.Click += new EventHandler(this.btnRegister_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(457, 478);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 24;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(129, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Encompass ERDB Server";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(427, 12);
      this.label2.Name = "label2";
      this.label2.Size = new Size(26, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Port";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(8, 110);
      this.label4.Name = "label4";
      this.label4.Size = new Size(120, 13);
      this.label4.TabIndex = 5;
      this.label4.Text = "ERDB Database Server";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(8, 133);
      this.label5.Name = "label5";
      this.label5.Size = new Size(117, 13);
      this.label5.TabIndex = 6;
      this.label5.Text = "ERDB Database Name";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(8, 156);
      this.label6.Name = "label6";
      this.label6.Size = new Size(115, 13);
      this.label6.TabIndex = 7;
      this.label6.Text = "ERDB Database Login";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(8, 179);
      this.label7.Name = "label7";
      this.label7.Size = new Size(135, 13);
      this.label7.TabIndex = 8;
      this.label7.Text = "ERDB Database Password";
      this.textBoxERDBAppServer.Location = new Point(145, 9);
      this.textBoxERDBAppServer.Name = "textBoxERDBAppServer";
      this.textBoxERDBAppServer.Size = new Size(276, 20);
      this.textBoxERDBAppServer.TabIndex = 1;
      this.textBoxERDBAppServer.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.textBoxERDBAppServerPort.Location = new Point(457, 9);
      this.textBoxERDBAppServerPort.Name = "textBoxERDBAppServerPort";
      this.textBoxERDBAppServerPort.Size = new Size(75, 20);
      this.textBoxERDBAppServerPort.TabIndex = 2;
      this.textBoxERDBAppServerPort.Text = "11099";
      this.textBoxERDBAppServerPort.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.textBoxERDBServer.Location = new Point(145, 107);
      this.textBoxERDBServer.Name = "textBoxERDBServer";
      this.textBoxERDBServer.Size = new Size(276, 20);
      this.textBoxERDBServer.TabIndex = 4;
      this.textBoxERDBServer.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.textBoxERDBName.Location = new Point(145, 130);
      this.textBoxERDBName.Name = "textBoxERDBName";
      this.textBoxERDBName.Size = new Size(276, 20);
      this.textBoxERDBName.TabIndex = 5;
      this.textBoxERDBName.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.textBoxERDBLogin.Location = new Point(145, 153);
      this.textBoxERDBLogin.Name = "textBoxERDBLogin";
      this.textBoxERDBLogin.Size = new Size(276, 20);
      this.textBoxERDBLogin.TabIndex = 6;
      this.textBoxERDBLogin.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.textBoxERDBPwd.Location = new Point(145, 176);
      this.textBoxERDBPwd.Name = "textBoxERDBPwd";
      this.textBoxERDBPwd.PasswordChar = '*';
      this.textBoxERDBPwd.Size = new Size(276, 20);
      this.textBoxERDBPwd.TabIndex = 7;
      this.textBoxERDBPwd.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(8, 202);
      this.label8.Name = "label8";
      this.label8.Size = new Size(109, 13);
      this.label8.TabIndex = 10;
      this.label8.Text = "Confirm DB Password";
      this.textBoxERDBPwd2.Location = new Point(145, 199);
      this.textBoxERDBPwd2.Name = "textBoxERDBPwd2";
      this.textBoxERDBPwd2.PasswordChar = '*';
      this.textBoxERDBPwd2.Size = new Size(276, 20);
      this.textBoxERDBPwd2.TabIndex = 8;
      this.textBoxERDBPwd2.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.groupBox1.Location = new Point(12, 223);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(520, 10);
      this.groupBox1.TabIndex = 11;
      this.groupBox1.TabStop = false;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(8, 239);
      this.label3.Name = "label3";
      this.label3.Size = new Size(207, 13);
      this.label3.TabIndex = 12;
      this.label3.Text = "ERDB Server Failure Notification (optional)";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(8, 270);
      this.label9.Name = "label9";
      this.label9.Size = new Size(71, 13);
      this.label9.TabIndex = 13;
      this.label9.Text = "SMTP Server";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(8, 294);
      this.label10.Name = "label10";
      this.label10.Size = new Size(59, 13);
      this.label10.TabIndex = 14;
      this.label10.Text = "SMTP Port";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(8, 318);
      this.label11.Name = "label11";
      this.label11.Size = new Size(60, 13);
      this.label11.TabIndex = 15;
      this.label11.Text = "User Name";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(8, 342);
      this.label12.Name = "label12";
      this.label12.Size = new Size(53, 13);
      this.label12.TabIndex = 16;
      this.label12.Text = "Password";
      this.textBoxSMTPServer.Location = new Point(103, 264);
      this.textBoxSMTPServer.Name = "textBoxSMTPServer";
      this.textBoxSMTPServer.Size = new Size(195, 20);
      this.textBoxSMTPServer.TabIndex = 9;
      this.textBoxSMTPPort.Location = new Point(103, 288);
      this.textBoxSMTPPort.Name = "textBoxSMTPPort";
      this.textBoxSMTPPort.Size = new Size(195, 20);
      this.textBoxSMTPPort.TabIndex = 10;
      this.textBoxSMTPPort.Text = "25";
      this.textBoxSMTPPort.KeyPress += new KeyPressEventHandler(this.integerTextBox_KeyPress);
      this.textBoxSMTPUserName.Location = new Point(103, 312);
      this.textBoxSMTPUserName.Name = "textBoxSMTPUserName";
      this.textBoxSMTPUserName.Size = new Size(195, 20);
      this.textBoxSMTPUserName.TabIndex = 11;
      this.textBoxSMTPPassword.Location = new Point(103, 336);
      this.textBoxSMTPPassword.Name = "textBoxSMTPPassword";
      this.textBoxSMTPPassword.PasswordChar = '*';
      this.textBoxSMTPPassword.Size = new Size(195, 20);
      this.textBoxSMTPPassword.TabIndex = 12;
      this.btnTestEmail.Location = new Point(454, 406);
      this.btnTestEmail.Name = "btnTestEmail";
      this.btnTestEmail.Size = new Size(75, 23);
      this.btnTestEmail.TabIndex = 22;
      this.btnTestEmail.Text = "Test";
      this.btnTestEmail.UseVisualStyleBackColor = true;
      this.btnTestEmail.Click += new EventHandler(this.btnTestEmail_Click);
      this.groupBox2.Location = new Point(9, 464);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(520, 10);
      this.groupBox2.TabIndex = 22;
      this.groupBox2.TabStop = false;
      this.label13.AutoSize = true;
      this.label13.Location = new Point(8, 414);
      this.label13.Name = "label13";
      this.label13.Size = new Size(53, 13);
      this.label13.TabIndex = 23;
      this.label13.Text = "To Emails";
      this.textBoxEmail.Location = new Point(103, 408);
      this.textBoxEmail.Name = "textBoxEmail";
      this.textBoxEmail.Size = new Size(195, 20);
      this.textBoxEmail.TabIndex = 15;
      this.label14.AutoSize = true;
      this.label14.Location = new Point(9, 446);
      this.label14.Name = "label14";
      this.label14.Size = new Size(220, 13);
      this.label14.TabIndex = 24;
      this.label14.Text = "Minimum time between two notification emails";
      this.textBoxNotificationEmailInterval.Location = new Point(232, 443);
      this.textBoxNotificationEmailInterval.Name = "textBoxNotificationEmailInterval";
      this.textBoxNotificationEmailInterval.Size = new Size(66, 20);
      this.textBoxNotificationEmailInterval.TabIndex = 16;
      this.textBoxNotificationEmailInterval.Text = "12";
      this.textBoxNotificationEmailInterval.KeyPress += new KeyPressEventHandler(this.integerTextBox_KeyPress);
      this.label15.AutoSize = true;
      this.label15.Location = new Point(300, 446);
      this.label15.Name = "label15";
      this.label15.Size = new Size(33, 13);
      this.label15.TabIndex = 26;
      this.label15.Text = "hours";
      this.btnCreateDbAndRegister.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCreateDbAndRegister.DialogResult = DialogResult.OK;
      this.btnCreateDbAndRegister.Location = new Point(189, 478);
      this.btnCreateDbAndRegister.Name = "btnCreateDbAndRegister";
      this.btnCreateDbAndRegister.Size = new Size(169, 23);
      this.btnCreateDbAndRegister.TabIndex = 20;
      this.btnCreateDbAndRegister.Text = "Create Database and Register";
      this.btnCreateDbAndRegister.UseVisualStyleBackColor = true;
      this.btnCreateDbAndRegister.Click += new EventHandler(this.btnRegister_Click);
      this.label16.AutoSize = true;
      this.label16.Location = new Point(8, 72);
      this.label16.Name = "label16";
      this.label16.Size = new Size(117, 13);
      this.label16.TabIndex = 28;
      this.label16.Text = "EncompassData Folder";
      this.textBoxEncDataDir.Location = new Point(145, 71);
      this.textBoxEncDataDir.Name = "textBoxEncDataDir";
      this.textBoxEncDataDir.Size = new Size(387, 20);
      this.textBoxEncDataDir.TabIndex = 3;
      this.textBoxEncDataDir.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.groupBox3.Location = new Point(9, 32);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(520, 10);
      this.groupBox3.TabIndex = 30;
      this.groupBox3.TabStop = false;
      this.btnTestConnections.Location = new Point(430, 197);
      this.btnTestConnections.Name = "btnTestConnections";
      this.btnTestConnections.Size = new Size(102, 23);
      this.btnTestConnections.TabIndex = 23;
      this.btnTestConnections.Text = "Test Connections";
      this.btnTestConnections.UseVisualStyleBackColor = true;
      this.btnTestConnections.Click += new EventHandler(this.btnTestConnections_Click);
      this.textBoxFrom.Location = new Point(103, 384);
      this.textBoxFrom.Name = "textBoxFrom";
      this.textBoxFrom.Size = new Size(195, 20);
      this.textBoxFrom.TabIndex = 14;
      this.label17.AutoSize = true;
      this.label17.Location = new Point(8, 390);
      this.label17.Name = "label17";
      this.label17.Size = new Size(58, 13);
      this.label17.TabIndex = 33;
      this.label17.Text = "From Email";
      this.textBoxSMTPPwd2.Location = new Point(103, 360);
      this.textBoxSMTPPwd2.Name = "textBoxSMTPPwd2";
      this.textBoxSMTPPwd2.PasswordChar = '*';
      this.textBoxSMTPPwd2.Size = new Size(195, 20);
      this.textBoxSMTPPwd2.TabIndex = 13;
      this.label18.AutoSize = true;
      this.label18.Location = new Point(8, 366);
      this.label18.Name = "label18";
      this.label18.Size = new Size(91, 13);
      this.label18.TabIndex = 35;
      this.label18.Text = "Confirm Password";
      this.chkBoxUseSSL.AutoSize = true;
      this.chkBoxUseSSL.Location = new Point(305, 314);
      this.chkBoxUseSSL.Name = "chkBoxUseSSL";
      this.chkBoxUseSSL.Size = new Size(68, 17);
      this.chkBoxUseSSL.TabIndex = 17;
      this.chkBoxUseSSL.Text = "Use SSL";
      this.chkBoxUseSSL.UseVisualStyleBackColor = true;
      this.label19.AutoSize = true;
      this.label19.Location = new Point(8, 48);
      this.label19.Name = "label19";
      this.label19.Size = new Size(494, 13);
      this.label19.TabIndex = 36;
      this.label19.Text = "Enter the share location path of EncompassData directory accessed from the ERDB Application Server.";
      this.label20.AutoSize = true;
      this.label20.Location = new Point(306, 411);
      this.label20.Name = "label20";
      this.label20.Size = new Size(138, 13);
      this.label20.TabIndex = 37;
      this.label20.Text = "(emails separated by ',' or ';')";
      this.SaveNotificationSettings.Location = new Point(339, 441);
      this.SaveNotificationSettings.Name = "SaveNotificationSettings";
      this.SaveNotificationSettings.Size = new Size(193, 23);
      this.SaveNotificationSettings.TabIndex = 38;
      this.SaveNotificationSettings.Text = "Save Notification Settings Only";
      this.SaveNotificationSettings.UseVisualStyleBackColor = true;
      this.SaveNotificationSettings.Click += new EventHandler(this.SaveNotificationSettings_Click);
      this.AcceptButton = (IButtonControl) this.btnRegister;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(538, 507);
      this.Controls.Add((Control) this.SaveNotificationSettings);
      this.Controls.Add((Control) this.label20);
      this.Controls.Add((Control) this.label19);
      this.Controls.Add((Control) this.chkBoxUseSSL);
      this.Controls.Add((Control) this.textBoxSMTPPwd2);
      this.Controls.Add((Control) this.label18);
      this.Controls.Add((Control) this.textBoxFrom);
      this.Controls.Add((Control) this.label17);
      this.Controls.Add((Control) this.btnTestConnections);
      this.Controls.Add((Control) this.groupBox3);
      this.Controls.Add((Control) this.textBoxEncDataDir);
      this.Controls.Add((Control) this.label16);
      this.Controls.Add((Control) this.btnCreateDbAndRegister);
      this.Controls.Add((Control) this.label15);
      this.Controls.Add((Control) this.textBoxNotificationEmailInterval);
      this.Controls.Add((Control) this.label14);
      this.Controls.Add((Control) this.textBoxEmail);
      this.Controls.Add((Control) this.label13);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.btnTestEmail);
      this.Controls.Add((Control) this.textBoxSMTPPassword);
      this.Controls.Add((Control) this.textBoxSMTPUserName);
      this.Controls.Add((Control) this.textBoxSMTPPort);
      this.Controls.Add((Control) this.textBoxSMTPServer);
      this.Controls.Add((Control) this.label12);
      this.Controls.Add((Control) this.label11);
      this.Controls.Add((Control) this.label10);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.textBoxERDBPwd2);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.textBoxERDBPwd);
      this.Controls.Add((Control) this.textBoxERDBLogin);
      this.Controls.Add((Control) this.textBoxERDBName);
      this.Controls.Add((Control) this.textBoxERDBServer);
      this.Controls.Add((Control) this.textBoxERDBAppServerPort);
      this.Controls.Add((Control) this.textBoxERDBAppServer);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnRegister);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (ERDBRegistrationForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Register External Reporting Database";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
