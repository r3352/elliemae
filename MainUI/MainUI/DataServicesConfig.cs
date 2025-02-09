// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.DataServicesConfig
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.JedLib;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class DataServicesConfig : Form
  {
    private static string optOutKey = "";
    private static string errorMsg = "";
    private static string applicationName = "DataServices";
    private static string userID = "";
    private static string password = "";
    private static string serverName = "";
    private static bool optOut = true;
    private static DataServicesConfigHelper helper = new DataServicesConfigHelper();
    private static b jed = (b) null;
    private IContainer components;
    private Label lblUserID;
    private Label lblPassword;
    private TextBox txtUserID;
    private TextBox txtPassword;
    private TextBox txtPath;
    private Label lblPath;
    private OpenFileDialog openFileDialog1;
    private Button btnBrowse;
    private Button btnOK;
    private Button btnCancel;
    private Label label2;
    private Label label3;
    private Panel panel1;
    private RadioButton rbnNetwork;
    private RadioButton rbnOffline;
    private Label lblServer;
    private ComboBox cmbServer;
    private Label lblTitle;
    private Label label1;
    private Panel panel2;
    private RadioButton rdoOptIn;
    private RadioButton rdoOptOut;
    private Label lblSubtitle;
    private Label label4;

    public DataServicesConfig()
    {
      this.InitializeComponent();
      DataServicesConfig.jed = a.b("8ddrw372kr0WXky0");
    }

    public DataServicesConfig(
      string userName,
      string password,
      string fileDirectory,
      bool networkMode,
      string networkPath)
    {
      this.InitializeComponent();
      if (networkMode)
      {
        this.rbnNetwork.Checked = true;
        this.cmbServer.Text = networkPath;
      }
      this.txtUserID.Text = userName;
      this.txtPassword.Text = password;
      this.txtPath.Text = fileDirectory;
      this.btnOK_Click((object) null, (EventArgs) null);
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
      this.openFileDialog1.Filter = "Text File (*.txt)|*.txt";
      if (this.openFileDialog1.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
        return;
      this.txtPath.Text = this.openFileDialog1.FileName;
    }

    private void rbnConnection_CheckedChanged(object sender, EventArgs e)
    {
      if (this.rbnOffline.Checked)
        this.cmbServer.Enabled = false;
      else
        this.cmbServer.Enabled = true;
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void CloseApplication()
    {
      if (this.InvokeRequired)
        new MethodInvoker(this.CloseApplication)();
      else
        this.Close();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!DataServicesConfig.CheckRequiredData(this.txtUserID.Text.Trim(), this.txtPassword.Text.Trim(), !this.rbnOffline.Checked, this.cmbServer.Text.Trim(), this.txtPath.Text.Trim()))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, DataServicesConfig.errorMsg, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        DataServicesConfig.serverName = !this.rbnOffline.Checked ? this.cmbServer.Text.Trim() : "(Local)";
        DataServicesConfig.userID = this.txtUserID.Text.Trim();
        DataServicesConfig.password = this.txtPassword.Text.Trim();
        DataServicesConfig.optOut = this.rdoOptOut.Checked;
        try
        {
          if (DataServicesConfig.UpdateOptOut())
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "Your request has been executed successfully", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          else
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, DataServicesConfig.errorMsg, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        catch (Exception ex)
        {
          int num4 = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        finally
        {
          if (Session.IsConnected)
            Session.End();
        }
      }
    }

    private static bool CheckRequiredData(
      string userID,
      string password,
      bool networkMode,
      string networkPath,
      string filePath)
    {
      DataServicesConfig.errorMsg = "";
      bool flag = true;
      if (userID == "" || password == "")
      {
        flag = false;
        DataServicesConfig.errorMsg = "Please provide both userid and password information";
      }
      else if (networkMode && networkPath == "")
      {
        flag = false;
        DataServicesConfig.errorMsg = "Please provide server location";
      }
      else if (filePath == "")
      {
        flag = false;
        DataServicesConfig.errorMsg = "Please select a file";
      }
      else
      {
        try
        {
          using (StreamReader streamReader = new StreamReader(filePath))
          {
            DataServicesConfig.optOutKey = streamReader.ReadToEnd();
            streamReader.Close();
          }
        }
        catch (Exception ex)
        {
          flag = false;
          DataServicesConfig.errorMsg = "Unable to read data from the selected file.";
        }
      }
      return flag;
    }

    private static bool UpdateOptOut()
    {
      bool flag = false;
      try
      {
        if (DataServicesConfig.serverName == "(Local)")
          Session.Start(DataServicesConfig.userID, DataServicesConfig.password, DataServicesConfig.applicationName, false);
        else
          Session.Start(DataServicesConfig.serverName, DataServicesConfig.userID, DataServicesConfig.password, DataServicesConfig.applicationName, false);
        Session.ExitOnDisconnect = true;
        if (!Session.IsConnected)
          DataServicesConfig.errorMsg = "Session is not started.";
        else
          flag = true;
      }
      catch (FormatException ex)
      {
        DataServicesConfig.errorMsg = "The Server name format you have entered is invalid.";
      }
      catch (LoginException ex)
      {
        if (ex.LoginReturnCode == LoginReturnCode.USERID_NOT_FOUND || ex.LoginReturnCode == LoginReturnCode.PASSWORD_MISMATCH)
          DataServicesConfig.errorMsg = "The username and password entered do not match. The password is case-sensitive. Please try again.";
        else if (ex.LoginReturnCode == LoginReturnCode.USER_DISABLED)
          DataServicesConfig.errorMsg = "This user account has been disabled.";
        else if (ex.LoginReturnCode == LoginReturnCode.LOGIN_DISABLED)
          DataServicesConfig.errorMsg = "Login has been disabled. Only the \"admin\" user can login to the system.";
      }
      catch (ServerDataException ex)
      {
        MetricsFactory.IncrementErrorCounter((Exception) ex, (string) null, "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\DataServicesConfig.cs", nameof (UpdateOptOut), 210);
        DataServicesConfig.errorMsg = "Make sure that the SQL database server is up and running and the data in the database is not corrupt.";
      }
      catch (VersionMismatchException ex)
      {
        MetricsFactory.IncrementErrorCounter((Exception) ex, (string) null, "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\DataServicesConfig.cs", nameof (UpdateOptOut), 215);
        DataServicesConfig.errorMsg = ex.Message;
      }
      catch (EllieMae.EMLite.ClientServer.Exceptions.LicenseException ex)
      {
        MetricsFactory.IncrementErrorCounter((Exception) ex, (string) null, "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\DataServicesConfig.cs", nameof (UpdateOptOut), 220);
        DataServicesConfig.errorMsg = ex.Message;
      }
      catch (ServerConnectionException ex)
      {
        MetricsFactory.IncrementErrorCounter((Exception) ex, (string) null, "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\DataServicesConfig.cs", nameof (UpdateOptOut), 225);
        DataServicesConfig.errorMsg = ex.Message;
      }
      catch (Exception ex)
      {
        MetricsFactory.IncrementErrorCounter(ex, (string) null, "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\DataServicesConfig.cs", nameof (UpdateOptOut), 230);
        DataServicesConfig.errorMsg = "Unknown login error: " + ex.Message;
      }
      try
      {
        if (flag)
        {
          string str = "";
          lock (DataServicesConfig.jed)
          {
            byte[] A_0 = Convert.FromBase64String(DataServicesConfig.optOutKey);
            if (A_0 != null)
            {
              DataServicesConfig.jed.b();
              str = DataServicesConfig.jed.a(A_0, 0, A_0.Length);
            }
          }
          if (str != null && str != "")
          {
            if (str.IndexOf("|") > 0)
            {
              string[] strArray = str.Split('|');
              if (strArray.Length > 2)
              {
                DataServicesConfig.errorMsg = "The provided opt out file is not a valid file.";
                flag = false;
              }
              else if (Session.CompanyInfo.ClientID != strArray[0])
              {
                DataServicesConfig.errorMsg = "The provided opt out file can not be applied to this company";
                flag = false;
              }
              else if (DataServicesConfig.userID != strArray[1])
              {
                DataServicesConfig.errorMsg = "The provided opt out file can not be applied for this user account.";
                flag = false;
              }
              else if (DataServicesConfig.optOut)
                Session.ConfigurationManager.UpdateUserDataServicesOpt(DataServicesConfig.userID, DataServicesConfig.optOutKey);
              else
                Session.ConfigurationManager.UpdateUserDataServicesOpt(DataServicesConfig.userID, "");
            }
            else if (Session.CompanyInfo.ClientID != str)
            {
              DataServicesConfig.errorMsg = "The provided opt out file can not be applied to this company";
              flag = false;
            }
            else if (Session.UserID != "admin")
            {
              DataServicesConfig.errorMsg = "The login user account does not have enough security right to perform this request.";
              flag = false;
            }
            else if (DataServicesConfig.optOut)
              Session.ConfigurationManager.UpdateCompanyDataServicesOpt(DataServicesConfig.optOutKey);
            else
              Session.ConfigurationManager.UpdateCompanyDataServicesOpt("");
          }
          else
            flag = false;
        }
      }
      catch (Exception ex)
      {
        DataServicesConfig.errorMsg = "The provided opt out file is not a valide file.";
        flag = false;
      }
      finally
      {
        if (Session.IsConnected)
          Session.End();
      }
      return flag;
    }

    public static bool Execute(
      string userID,
      string password,
      bool networkMode,
      string networkPath,
      string filePath)
    {
      DataServicesConfig.jed = a.b("8ddrw372kr0WXky0");
      bool flag = true;
      DataServicesConfig.userID = userID;
      DataServicesConfig.password = password;
      DataServicesConfig.serverName = !networkMode ? "(Local)" : networkPath;
      if (!DataServicesConfig.CheckRequiredData(userID, password, networkMode, networkPath, filePath))
        flag = false;
      else if (!DataServicesConfig.UpdateOptOut())
        flag = false;
      return flag;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblUserID = new Label();
      this.lblPassword = new Label();
      this.txtUserID = new TextBox();
      this.txtPassword = new TextBox();
      this.txtPath = new TextBox();
      this.lblPath = new Label();
      this.openFileDialog1 = new OpenFileDialog();
      this.btnBrowse = new Button();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.label2 = new Label();
      this.label3 = new Label();
      this.panel1 = new Panel();
      this.cmbServer = new ComboBox();
      this.rbnNetwork = new RadioButton();
      this.rbnOffline = new RadioButton();
      this.lblServer = new Label();
      this.lblTitle = new Label();
      this.label1 = new Label();
      this.panel2 = new Panel();
      this.rdoOptIn = new RadioButton();
      this.rdoOptOut = new RadioButton();
      this.lblSubtitle = new Label();
      this.label4 = new Label();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.lblUserID.AutoSize = true;
      this.lblUserID.Location = new Point(12, 107);
      this.lblUserID.Name = "lblUserID";
      this.lblUserID.Size = new Size(45, 14);
      this.lblUserID.TabIndex = 2;
      this.lblUserID.Text = "User ID:";
      this.lblPassword.AutoSize = true;
      this.lblPassword.Location = new Point(12, 131);
      this.lblPassword.Name = "lblPassword";
      this.lblPassword.Size = new Size(60, 14);
      this.lblPassword.TabIndex = 3;
      this.lblPassword.Text = "Password:";
      this.txtUserID.Location = new Point(83, 103);
      this.txtUserID.Name = "txtUserID";
      this.txtUserID.Size = new Size(140, 20);
      this.txtUserID.TabIndex = 4;
      this.txtPassword.Location = new Point(83, 128);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.Size = new Size(140, 20);
      this.txtPassword.TabIndex = 5;
      this.txtPassword.UseSystemPasswordChar = true;
      this.txtPath.Location = new Point(83, 282);
      this.txtPath.Name = "txtPath";
      this.txtPath.Size = new Size(288, 20);
      this.txtPath.TabIndex = 6;
      this.lblPath.AutoSize = true;
      this.lblPath.Location = new Point(13, 282);
      this.lblPath.Name = "lblPath";
      this.lblPath.Size = new Size(50, 14);
      this.lblPath.TabIndex = 7;
      this.lblPath.Text = "File Path:";
      this.openFileDialog1.FileName = "DataServicesOptOut.txt";
      this.btnBrowse.Location = new Point(374, 281);
      this.btnBrowse.Name = "btnBrowse";
      this.btnBrowse.Size = new Size(57, 22);
      this.btnBrowse.TabIndex = 8;
      this.btnBrowse.Text = "Browse";
      this.btnBrowse.UseVisualStyleBackColor = true;
      this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(273, 377);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 9;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.Location = new Point(354, 377);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 10;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(12, 70);
      this.label2.Name = "label2";
      this.label2.Size = new Size(402, 32);
      this.label2.TabIndex = 12;
      this.label2.Text = "1.  Enter your Encompass login information.  Use the \"admin\" login only if you intend to update settings for the entire company.";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(12, 264);
      this.label3.Name = "label3";
      this.label3.Size = new Size(343, 13);
      this.label3.TabIndex = 13;
      this.label3.Text = "3.  Enter the path to the settings file provided by ICE Mortgage Technology.";
      this.panel1.Controls.Add((Control) this.cmbServer);
      this.panel1.Controls.Add((Control) this.rbnNetwork);
      this.panel1.Controls.Add((Control) this.rbnOffline);
      this.panel1.Controls.Add((Control) this.lblServer);
      this.panel1.Location = new Point(12, 183);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(359, 66);
      this.panel1.TabIndex = 14;
      this.cmbServer.FormattingEnabled = true;
      this.cmbServer.Items.AddRange(new object[1]
      {
        (object) "(Local)"
      });
      this.cmbServer.Location = new Point(122, 29);
      this.cmbServer.Name = "cmbServer";
      this.cmbServer.Size = new Size(178, 22);
      this.cmbServer.TabIndex = 4;
      this.rbnNetwork.AutoSize = true;
      this.rbnNetwork.Location = new Point(4, 30);
      this.rbnNetwork.Name = "rbnNetwork";
      this.rbnNetwork.Size = new Size(66, 18);
      this.rbnNetwork.TabIndex = 2;
      this.rbnNetwork.Text = "Network";
      this.rbnNetwork.UseVisualStyleBackColor = true;
      this.rbnNetwork.CheckedChanged += new EventHandler(this.rbnConnection_CheckedChanged);
      this.rbnOffline.AutoSize = true;
      this.rbnOffline.Checked = true;
      this.rbnOffline.Location = new Point(4, 4);
      this.rbnOffline.Name = "rbnOffline";
      this.rbnOffline.Size = new Size(57, 18);
      this.rbnOffline.TabIndex = 1;
      this.rbnOffline.TabStop = true;
      this.rbnOffline.Text = "Offline";
      this.rbnOffline.UseVisualStyleBackColor = true;
      this.rbnOffline.CheckedChanged += new EventHandler(this.rbnConnection_CheckedChanged);
      this.lblServer.AutoSize = true;
      this.lblServer.Location = new Point(75, 32);
      this.lblServer.Name = "lblServer";
      this.lblServer.Size = new Size(43, 14);
      this.lblServer.TabIndex = 0;
      this.lblServer.Text = "Server:";
      this.lblTitle.AutoSize = true;
      this.lblTitle.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTitle.ForeColor = Color.Black;
      this.lblTitle.Location = new Point(5, 10);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(0, 20);
      this.lblTitle.TabIndex = 18;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(12, 317);
      this.label1.Name = "label1";
      this.label1.Size = new Size(141, 13);
      this.label1.TabIndex = 19;
      this.label1.Text = "4.  Update your setting.";
      this.panel2.Controls.Add((Control) this.rdoOptIn);
      this.panel2.Controls.Add((Control) this.rdoOptOut);
      this.panel2.Location = new Point(15, 334);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(356, 24);
      this.panel2.TabIndex = 20;
      this.rdoOptIn.AutoSize = true;
      this.rdoOptIn.Location = new Point(75, 2);
      this.rdoOptIn.Name = "rdoOptIn";
      this.rdoOptIn.Size = new Size(53, 18);
      this.rdoOptIn.TabIndex = 1;
      this.rdoOptIn.Text = "Opt In";
      this.rdoOptIn.UseVisualStyleBackColor = true;
      this.rdoOptOut.AutoSize = true;
      this.rdoOptOut.Checked = true;
      this.rdoOptOut.Location = new Point(1, 2);
      this.rdoOptOut.Name = "rdoOptOut";
      this.rdoOptOut.Size = new Size(60, 18);
      this.rdoOptOut.TabIndex = 0;
      this.rdoOptOut.TabStop = true;
      this.rdoOptOut.Text = "Opt out";
      this.rdoOptOut.UseVisualStyleBackColor = true;
      this.lblSubtitle.Location = new Point(6, 15);
      this.lblSubtitle.Name = "lblSubtitle";
      this.lblSubtitle.Size = new Size(427, 44);
      this.lblSubtitle.TabIndex = 21;
      this.lblSubtitle.Text = "Follow the steps below to disable or enable Encompass Data Services for a single user or for your entire Encompass client ID.  Please contact ICE Mortgage Technology customer support before using this tool.";
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(12, 166);
      this.label4.Name = "label4";
      this.label4.Size = new Size(189, 13);
      this.label4.TabIndex = 22;
      this.label4.Text = "2.  Select your connection type.";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(446, 411);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.lblSubtitle);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.lblTitle);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnBrowse);
      this.Controls.Add((Control) this.lblPath);
      this.Controls.Add((Control) this.txtPath);
      this.Controls.Add((Control) this.txtPassword);
      this.Controls.Add((Control) this.txtUserID);
      this.Controls.Add((Control) this.lblPassword);
      this.Controls.Add((Control) this.lblUserID);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DataServicesConfig);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Encompass Data Services - Settings";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
