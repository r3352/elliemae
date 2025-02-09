// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.Packages.ExportPublishLoginPanel
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using Elli.Server.Remoting;
using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Wizard;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Forms.Packages
{
  public class ExportPublishLoginPanel : WizardItemWithHeader
  {
    private const string OfflineServer = "(Offline)";
    private Panel panel2;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private TextBox txtUserID;
    private TextBox txtPassword;
    private ComboBox cboServer;
    private IContainer components;
    private IConnection conn;

    public ExportPublishLoginPanel(WizardItem prevItem)
      : base(prevItem)
    {
      this.InitializeComponent();
      this.txtUserID.Text = SystemSettings.LastLoginName;
      if (EnConfigurationSettings.GlobalSettings.InstallationMode == InstallationMode.Local && (Session.RemoteServer ?? "") != "")
        this.cboServer.Items.Add((object) "(Offline)");
      this.cboServer.Items.AddRange((object[]) SystemSettings.Servers);
      if (this.cboServer.Items.Count <= 0)
        return;
      this.cboServer.SelectedIndex = 0;
    }

    public ExportPublishLoginPanel(WizardItem prevItem, IConnection conn)
      : base(prevItem)
    {
      this.conn = conn;
      this.Next();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel2 = new Panel();
      this.cboServer = new ComboBox();
      this.txtPassword = new TextBox();
      this.txtUserID = new TextBox();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.panel2.BackColor = Color.White;
      this.panel2.Controls.Add((Control) this.cboServer);
      this.panel2.Controls.Add((Control) this.txtPassword);
      this.panel2.Controls.Add((Control) this.txtUserID);
      this.panel2.Controls.Add((Control) this.label4);
      this.panel2.Controls.Add((Control) this.label3);
      this.panel2.Controls.Add((Control) this.label2);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 60);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(496, 254);
      this.panel2.TabIndex = 10;
      this.cboServer.Location = new Point(218, 136);
      this.cboServer.Name = "cboServer";
      this.cboServer.Size = new Size(150, 21);
      this.cboServer.TabIndex = 6;
      this.txtPassword.Location = new Point(218, 114);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.PasswordChar = '*';
      this.txtPassword.Size = new Size(150, 20);
      this.txtPassword.TabIndex = 5;
      this.txtPassword.Text = "";
      this.txtUserID.Location = new Point(218, 92);
      this.txtUserID.Name = "txtUserID";
      this.txtUserID.Size = new Size(150, 20);
      this.txtUserID.TabIndex = 4;
      this.txtUserID.Text = "";
      this.label4.Location = new Point(128, 117);
      this.label4.Name = "label4";
      this.label4.Size = new Size(88, 16);
      this.label4.TabIndex = 3;
      this.label4.Text = "Password:";
      this.label3.Location = new Point(128, 139);
      this.label3.Name = "label3";
      this.label3.Size = new Size(88, 16);
      this.label3.TabIndex = 2;
      this.label3.Text = "Server:";
      this.label2.Location = new Point(128, 94);
      this.label2.Name = "label2";
      this.label2.Size = new Size(88, 16);
      this.label2.TabIndex = 1;
      this.label2.Text = "Login ID:";
      this.label1.Location = new Point(82, 44);
      this.label1.Name = "label1";
      this.label1.Size = new Size(332, 36);
      this.label1.TabIndex = 0;
      this.label1.Text = "Provide the login information for the server to which to publish this package:";
      this.Controls.Add((Control) this.panel2);
      this.Header = "Publish Package - Login";
      this.Name = nameof (ExportPublishLoginPanel);
      this.Subheader = "";
      this.Controls.SetChildIndex((Control) this.panel2, 0);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public override string NextLabel => "&Publish";

    public override WizardItem Next()
    {
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        if (this.conn != null)
        {
          if (!this.publishPackage(this.conn))
            return (WizardItem) null;
        }
        else
        {
          using (IConnection conn = this.openConnection())
          {
            if (conn == null)
              return (WizardItem) null;
            if (!this.publishPackage(conn))
              return (WizardItem) null;
          }
        }
        return WizardItem.Finished;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "An error has occurred while publishing this package: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return (WizardItem) null;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private IConnection openConnection()
    {
      try
      {
        IConnection connection;
        if (this.cboServer.Text == "(Offline)")
        {
          connection = (IConnection) new InProcConnection();
          ((InProcConnection) connection).OpenInProcess(this.txtUserID.Text, this.txtPassword.Text, "PackagePublisher", false);
        }
        else
        {
          connection = (IConnection) new Connection();
          ((Connection) connection).Open(this.cboServer.Text, this.txtUserID.Text, this.txtPassword.Text, "PackagePublisher", false);
          this.updateServerSettings(this.cboServer.Text);
        }
        SystemSettings.LastLoginName = this.txtUserID.Text;
        if (((IFeaturesAclManager) connection.Session.GetAclManager(AclCategory.Features)).CheckPermission(AclFeature.SettingsTab_Company_CustomInputFormEditor, connection.Session.GetUser().GetUserInfo()))
          return connection;
        connection.Close();
        int num = (int) Utils.Dialog((IWin32Window) this, "The specified user is must be granted access to the Custom Input Form Editor in order to publish forms to this server. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (IConnection) null;
      }
      catch (LoginException ex)
      {
        if (ex.LoginReturnCode == LoginReturnCode.USERID_NOT_FOUND || ex.LoginReturnCode == LoginReturnCode.PASSWORD_MISMATCH)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "The username and password entered do not match. The password is case-sensitive. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (ex.LoginReturnCode == LoginReturnCode.USER_DISABLED)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "This user account has been disabled. Please contact your administrator.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (ex.LoginReturnCode == LoginReturnCode.LOGIN_DISABLED)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "Login has been disabled. Only the \"admin\" user can log in to the system.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (ex.LoginReturnCode == LoginReturnCode.USER_LOCKED)
        {
          int num4 = (int) Utils.Dialog((IWin32Window) this, "Your account has been locked. An administrative user must unlock your account in order for you to log in.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      catch (ServerDataException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Make sure that the SQL database server (MSDE) is up and running and the data in the database is not corrupted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      catch (VersionMismatchException ex)
      {
        if (EllieMae.EMLite.Common.VersionControl.QueryInstallVersionUpdate(ex.ServerVersionControl))
          Application.Exit();
      }
      catch (EllieMae.EMLite.ClientServer.Exceptions.LicenseException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        Application.Exit();
      }
      catch (ServerConnectionException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (FormatException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Server name format you have entered is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (Exception ex)
      {
        if (EnConfigurationSettings.GlobalSettings.Debug)
        {
          int num5 = (int) Utils.Dialog((IWin32Window) this, "Unknown login error: " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          int num6 = (int) Utils.Dialog((IWin32Window) this, "Unknown login error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      return (IConnection) null;
    }

    private void updateServerSettings(string serverName)
    {
      this.cboServer.Items.Remove((object) serverName);
      this.cboServer.Items.Insert(0, (object) serverName);
      this.cboServer.SelectedIndex = 0;
      string[] destination = new string[this.cboServer.Items.Count];
      this.cboServer.Items.CopyTo((object[]) destination, 0);
      SystemSettings.Servers = destination;
    }

    private bool publishPackage(IConnection conn)
    {
      try
      {
        if (!new PackageImportProcess(conn).ImportPackage(PackageExportWizard.CurrentPackage))
          return false;
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Package import completed successfully.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Import failed: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
    }
  }
}
