// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.AdminTools
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using Elli.Server.Remoting;
using Elli.Web.Host;
using Elli.Web.Host.Login;
using EllieMae.EMLite.AdminTools.EnhancedConditionsTool;
using EllieMae.EMLite.AdminTools.SCTLoanBatchUpdates;
using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Diagnostics;
using EllieMae.EMLite.Common.Diagnostics.ConfigChangeHandlers;
using EllieMae.EMLite.Common.Login;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.AsmResolver;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Config;
using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.Logging.Targets;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class AdminTools : Form, IApplicationWindow
  {
    private IContainer components;
    protected static string sw = Tracing.SwOutsideLoan;
    private Hashtable formCache = new Hashtable();
    private ImageList ilsIcons;
    private GridView gvTools;
    private GradientMenuStrip gradientMenuStrip1;
    private ToolStripMenuItem fileToolStripMenuItem;
    private ToolStripMenuItem logoutMenuItem;
    private ToolStripSeparator logoutMenuSeparator;
    private ToolStripMenuItem exitMenuItem;
    private ToolStripMenuItem helpToolStripMenuItem;
    private ToolStripMenuItem adminToolsHelpToolStripMenuItem;
    private static EllieMae.EMLite.AdminTools.AdminTools mainForm = (EllieMae.EMLite.AdminTools.AdminTools) null;
    private static string serverLogListenerOwner = (string) null;
    private static RemotingLogConfigChangeHandler configChangeHandler = (RemotingLogConfigChangeHandler) null;

    public AdminTools(bool hasAdminRight)
    {
      this.Text = "Encompass Administrative Tools";
      this.InitializeComponent();
      if (WebLoginUtil.IsChromiumForWebLoginEnabled)
        BrowserEngine.StartBinaryDownload();
      if (SystemSettings.InstallationMode != InstallationMode.Server || !hasAdminRight)
        this.gvTools.Items.Remove(this.findListItemByTag("ServerManager"));
      if (SystemSettings.InstallationMode == InstallationMode.Client)
      {
        this.gvTools.Items.Remove(this.findListItemByTag("Registration"));
        if (!AssemblyResolver.IsSmartClient && !hasAdminRight)
          this.gvTools.Items.Remove(this.findListItemByTag("VersionManager"));
      }
      else if (!hasAdminRight)
      {
        this.gvTools.Items.Remove(this.findListItemByTag("Registration"));
        this.gvTools.Items.Remove(this.findListItemByTag("VersionManager"));
      }
      if (SystemSettings.InstallationMode == InstallationMode.Client || !hasAdminRight)
      {
        this.gvTools.Items.Remove(this.findListItemByTag("DBManager"));
        this.gvTools.Items.Remove(this.findListItemByTag("DBUserManager"));
      }
      if (!hasAdminRight)
        this.gvTools.Items.Remove(this.findListItemByTag("RegisterSDK"));
      if (false)
        this.gvTools.Items.Remove(this.findListItemByTag("EnhancedConditionsTool"));
      JedHelp.ApplicationName = nameof (AdminTools);
      JedHelp.ApplicationWindow = (IApplicationWindow) this;
      Application.ApplicationExit += new EventHandler(this.onApplicationExit);
      Session.Ended += new EventHandler(this.onSessionTerminated);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EllieMae.EMLite.AdminTools.AdminTools));
      GVColumn gvColumn = new GVColumn();
      GVItem gvItem1 = new GVItem();
      GVSubItem gvSubItem1 = new GVSubItem();
      GVItem gvItem2 = new GVItem();
      GVSubItem gvSubItem2 = new GVSubItem();
      GVItem gvItem3 = new GVItem();
      GVSubItem gvSubItem3 = new GVSubItem();
      GVItem gvItem4 = new GVItem();
      GVSubItem gvSubItem4 = new GVSubItem();
      GVItem gvItem5 = new GVItem();
      GVSubItem gvSubItem5 = new GVSubItem();
      GVItem gvItem6 = new GVItem();
      GVSubItem gvSubItem6 = new GVSubItem();
      GVItem gvItem7 = new GVItem();
      GVSubItem gvSubItem7 = new GVSubItem();
      GVItem gvItem8 = new GVItem();
      GVSubItem gvSubItem8 = new GVSubItem();
      GVItem gvItem9 = new GVItem();
      GVSubItem gvSubItem9 = new GVSubItem();
      GVItem gvItem10 = new GVItem();
      GVSubItem gvSubItem10 = new GVSubItem();
      GVItem gvItem11 = new GVItem();
      GVSubItem gvSubItem11 = new GVSubItem();
      GVItem gvItem12 = new GVItem();
      GVSubItem gvSubItem12 = new GVSubItem();
      GVItem gvItem13 = new GVItem();
      GVSubItem gvSubItem13 = new GVSubItem();
      this.ilsIcons = new ImageList(this.components);
      this.gvTools = new GridView();
      this.gradientMenuStrip1 = new GradientMenuStrip();
      this.fileToolStripMenuItem = new ToolStripMenuItem();
      this.logoutMenuItem = new ToolStripMenuItem();
      this.logoutMenuSeparator = new ToolStripSeparator();
      this.exitMenuItem = new ToolStripMenuItem();
      this.helpToolStripMenuItem = new ToolStripMenuItem();
      this.adminToolsHelpToolStripMenuItem = new ToolStripMenuItem();
      this.gradientMenuStrip1.SuspendLayout();
      this.SuspendLayout();
      this.ilsIcons.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilsIcons.ImageStream");
      this.ilsIcons.TransparentColor = Color.Transparent;
      this.ilsIcons.Images.SetKeyName(0, "register-encompass.png");
      this.ilsIcons.Images.SetKeyName(1, "encompas-server-manager.png");
      this.ilsIcons.Images.SetKeyName(2, "online-user-manager.png");
      this.ilsIcons.Images.SetKeyName(3, "setings-manager.png");
      this.ilsIcons.Images.SetKeyName(4, "sql-server-manager.png");
      this.ilsIcons.Images.SetKeyName(5, "sql-server-setup.png");
      this.ilsIcons.Images.SetKeyName(6, "version-update.png");
      this.ilsIcons.Images.SetKeyName(7, "log-viewer.png");
      this.ilsIcons.Images.SetKeyName(8, "event-viewer.png");
      this.ilsIcons.Images.SetKeyName(9, "reporting-database.png");
      this.ilsIcons.Images.SetKeyName(10, "genesis-migration-tool-16x16.png");
      this.ilsIcons.Images.SetKeyName(11, "contour-migration-16x16.png");
      this.ilsIcons.Images.SetKeyName(12, "App.ico");
      this.ilsIcons.Images.SetKeyName(13, "settings-tool.ico");
      this.gvTools.AllowMultiselect = false;
      this.gvTools.AlternatingColors = false;
      this.gvTools.BorderStyle = BorderStyle.None;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Tool";
      gvColumn.Width = 394;
      this.gvTools.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvTools.Dock = DockStyle.Fill;
      this.gvTools.FilterRowHeight = 3;
      this.gvTools.FilterVisible = true;
      this.gvTools.FullRowSelect = false;
      this.gvTools.GridLines = GVGridLines.None;
      this.gvTools.HeaderHeight = 0;
      this.gvTools.HeaderVisible = false;
      this.gvTools.HotItemTracking = false;
      this.gvTools.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvTools.ImageList = this.ilsIcons;
      gvItem1.BackColor = Color.Empty;
      gvItem1.ForeColor = Color.Empty;
      gvSubItem1.BackColor = Color.Empty;
      gvSubItem1.ForeColor = Color.Empty;
      gvSubItem1.ImageIndex = 0;
      gvSubItem1.Text = "Register Encompass";
      gvItem1.SubItems.AddRange(new GVSubItem[1]
      {
        gvSubItem1
      });
      gvItem1.Tag = (object) "Registration";
      gvItem2.BackColor = Color.Empty;
      gvItem2.ForeColor = Color.Empty;
      gvSubItem2.BackColor = Color.Empty;
      gvSubItem2.ForeColor = Color.Empty;
      gvSubItem2.ImageIndex = 1;
      gvSubItem2.Text = "Encompass Server Manager";
      gvItem2.SubItems.AddRange(new GVSubItem[1]
      {
        gvSubItem2
      });
      gvItem2.Tag = (object) "ServerManager";
      gvItem3.BackColor = Color.Empty;
      gvItem3.ForeColor = Color.Empty;
      gvSubItem3.BackColor = Color.Empty;
      gvSubItem3.ForeColor = Color.Empty;
      gvSubItem3.ImageIndex = 2;
      gvSubItem3.Text = "Online User Manager";
      gvItem3.SubItems.AddRange(new GVSubItem[1]
      {
        gvSubItem3
      });
      gvItem3.Tag = (object) "UserManager";
      gvItem4.BackColor = Color.Empty;
      gvItem4.ForeColor = Color.Empty;
      gvSubItem4.BackColor = Color.Empty;
      gvSubItem4.ForeColor = Color.Empty;
      gvSubItem4.ImageIndex = 3;
      gvSubItem4.Text = "Settings Manager";
      gvItem4.SubItems.AddRange(new GVSubItem[1]
      {
        gvSubItem4
      });
      gvItem4.Tag = (object) "SettingsManager";
      gvItem5.BackColor = Color.Empty;
      gvItem5.ForeColor = Color.Empty;
      gvSubItem5.BackColor = Color.Empty;
      gvSubItem5.ForeColor = Color.Empty;
      gvSubItem5.ImageIndex = 4;
      gvSubItem5.Text = "SQL Service Manager";
      gvItem5.SubItems.AddRange(new GVSubItem[1]
      {
        gvSubItem5
      });
      gvItem5.Tag = (object) "DBManager";
      gvItem6.BackColor = Color.Empty;
      gvItem6.ForeColor = Color.Empty;
      gvSubItem6.BackColor = Color.Empty;
      gvSubItem6.ForeColor = Color.Empty;
      gvSubItem6.ImageIndex = 5;
      gvSubItem6.Text = "SQL Server Setup";
      gvItem6.SubItems.AddRange(new GVSubItem[1]
      {
        gvSubItem6
      });
      gvItem6.Tag = (object) "DBUserManager";
      gvItem7.BackColor = Color.Empty;
      gvItem7.ForeColor = Color.Empty;
      gvSubItem7.BackColor = Color.Empty;
      gvSubItem7.ForeColor = Color.Empty;
      gvSubItem7.ImageIndex = 12;
      gvSubItem7.Text = "Register Encompass SDK";
      gvItem7.SubItems.AddRange(new GVSubItem[1]
      {
        gvSubItem7
      });
      gvItem7.Tag = (object) "RegisterSDK";
      gvItem8.BackColor = Color.Empty;
      gvItem8.ForeColor = Color.Empty;
      gvSubItem8.BackColor = Color.Empty;
      gvSubItem8.ForeColor = Color.Empty;
      gvSubItem8.ImageIndex = 6;
      gvSubItem8.Text = "Version Manager";
      gvItem8.SubItems.AddRange(new GVSubItem[1]
      {
        gvSubItem8
      });
      gvItem8.Tag = (object) "VersionManager";
      gvItem9.BackColor = Color.Empty;
      gvItem9.ForeColor = Color.Empty;
      gvSubItem9.BackColor = Color.Empty;
      gvSubItem9.ForeColor = Color.Empty;
      gvSubItem9.ImageIndex = 9;
      gvSubItem9.Text = "Reporting Database";
      gvItem9.SubItems.AddRange(new GVSubItem[1]
      {
        gvSubItem9
      });
      gvItem9.Tag = (object) "ReportExtendedDB";
      gvItem10.BackColor = Color.Empty;
      gvItem10.ForeColor = Color.Empty;
      gvSubItem10.BackColor = Color.Empty;
      gvSubItem10.ForeColor = Color.Empty;
      gvSubItem10.ImageIndex = 13;
      gvSubItem10.Text = "Settings Sync Tool";
      gvItem10.SubItems.AddRange(new GVSubItem[1]
      {
        gvSubItem10
      });
      gvItem10.Tag = (object) "SettingsTool";
      gvItem11.BackColor = Color.Empty;
      gvItem11.ForeColor = Color.Empty;
      gvSubItem11.BackColor = Color.Empty;
      gvSubItem11.ForeColor = Color.Empty;
      gvSubItem11.ImageIndex = 6;
      gvSubItem11.Text = "HMDA Batch Update";
      gvItem11.SubItems.AddRange(new GVSubItem[1]
      {
        gvSubItem11
      });
      gvItem11.Tag = (object) "HMDATool";
      gvItem12.BackColor = Color.Empty;
      gvItem12.ForeColor = Color.Empty;
      gvSubItem12.BackColor = Color.Empty;
      gvSubItem12.ForeColor = Color.Empty;
      gvSubItem12.ImageIndex = 6;
      gvSubItem12.Text = "Commitment Terms Data Migration Tool";
      gvItem12.SubItems.AddRange(new GVSubItem[1]
      {
        gvSubItem12
      });
      gvItem12.Tag = (object) "CTDMTool";
      gvItem13.BackColor = Color.Empty;
      gvItem13.ForeColor = Color.Empty;
      gvSubItem13.BackColor = Color.Empty;
      gvSubItem13.ForeColor = Color.Empty;
      gvSubItem13.ImageIndex = 13;
      gvSubItem13.Text = "Enhanced Conditions Tool";
      gvItem13.SubItems.AddRange(new GVSubItem[1]
      {
        gvSubItem13
      });
      gvItem13.Tag = (object) "EnhancedConditionsTool";
      this.gvTools.Items.AddRange(new GVItem[13]
      {
        gvItem1,
        gvItem2,
        gvItem3,
        gvItem4,
        gvItem5,
        gvItem6,
        gvItem7,
        gvItem8,
        gvItem9,
        gvItem10,
        gvItem11,
        gvItem12,
        gvItem13
      });
      this.gvTools.Location = new Point(0, 24);
      this.gvTools.Name = "gvTools";
      this.gvTools.Size = new Size(394, 256);
      this.gvTools.TabIndex = 0;
      this.gvTools.MouseDoubleClick += new MouseEventHandler(this.gvTools_MouseDoubleClick);
      this.gradientMenuStrip1.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.fileToolStripMenuItem,
        (ToolStripItem) this.helpToolStripMenuItem
      });
      this.gradientMenuStrip1.Location = new Point(0, 0);
      this.gradientMenuStrip1.Name = "gradientMenuStrip1";
      this.gradientMenuStrip1.Padding = new Padding(1, 2, 0, 2);
      this.gradientMenuStrip1.Size = new Size(394, 24);
      this.gradientMenuStrip1.TabIndex = 1;
      this.gradientMenuStrip1.Text = "gradientMenuStrip1";
      this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.logoutMenuItem,
        (ToolStripItem) this.logoutMenuSeparator,
        (ToolStripItem) this.exitMenuItem
      });
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.ShortcutKeys = Keys.A | Keys.Alt;
      this.fileToolStripMenuItem.Size = new Size(37, 20);
      this.fileToolStripMenuItem.Text = "&File";
      this.logoutMenuItem.Name = "logoutMenuItem";
      this.logoutMenuItem.Size = new Size(134, 22);
      this.logoutMenuItem.Text = "&Logout";
      this.logoutMenuItem.Visible = false;
      this.logoutMenuItem.Click += new EventHandler(this.logoutMenuItem_Click);
      this.logoutMenuSeparator.Name = "logoutMenuSeparator";
      this.logoutMenuSeparator.Size = new Size(131, 6);
      this.logoutMenuSeparator.Visible = false;
      this.exitMenuItem.Name = "exitMenuItem";
      this.exitMenuItem.ShortcutKeys = Keys.F4 | Keys.Alt;
      this.exitMenuItem.Size = new Size(134, 22);
      this.exitMenuItem.Text = "E&xit";
      this.exitMenuItem.Click += new EventHandler(this.exitMenuItem_Click);
      this.helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.adminToolsHelpToolStripMenuItem
      });
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.ShortcutKeys = Keys.H | Keys.Alt;
      this.helpToolStripMenuItem.Size = new Size(44, 20);
      this.helpToolStripMenuItem.Text = "&Help";
      this.adminToolsHelpToolStripMenuItem.Image = (Image) componentResourceManager.GetObject("adminToolsHelpToolStripMenuItem.Image");
      this.adminToolsHelpToolStripMenuItem.Name = "adminToolsHelpToolStripMenuItem";
      this.adminToolsHelpToolStripMenuItem.ShortcutKeys = Keys.F1;
      this.adminToolsHelpToolStripMenuItem.Size = new Size(198, 22);
      this.adminToolsHelpToolStripMenuItem.Text = "Admin Tools &Help...";
      this.adminToolsHelpToolStripMenuItem.Click += new EventHandler(this.adminToolsHelpToolStripMenuItem_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(394, 304);
      this.Controls.Add((Control) this.gvTools);
      this.Controls.Add((Control) this.gradientMenuStrip1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MainMenuStrip = (MenuStrip) this.gradientMenuStrip1;
      this.MaximizeBox = false;
      this.Name = nameof (AdminTools);
      this.Text = "Encompass Admin Tools";
      this.gradientMenuStrip1.ResumeLayout(false);
      this.gradientMenuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public static void main(string[] args, bool hasAdminRight)
    {
      try
      {
        if (hasAdminRight)
          Win64.SyncEncompass32To64();
        EnConfigurationSettings.ApplyInstanceFromCommandLine(args);
        EnCertificatePolicy.SetDefaultPolicy();
        Tracing.Init(SystemSettings.LogDir + "\\" + Environment.UserName + "\\AdminTools");
        EllieMae.EMLite.AdminTools.AdminTools.mainForm = new EllieMae.EMLite.AdminTools.AdminTools(hasAdminRight);
        Application.Run((Form) EllieMae.EMLite.AdminTools.AdminTools.mainForm);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) null, "An unhandled exception occurred: " + (object) ex + ".  AdminTools is going to terminate.", "AdminTools Exception", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        try
        {
          Tracing.Close();
        }
        catch (Exception ex)
        {
        }
      }
    }

    private void exitMenuItem_Click(object sender, EventArgs e) => Application.Exit();

    private void gvTools_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      if (this.gvTools.SelectedItems.Count == 0)
        return;
      string tag = (string) this.gvTools.SelectedItems[0].Tag;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(tag))
      {
        case 27570975:
          if (!(tag == "HMDATool") || !this.EnsureSessionStarted(LoginType.All))
            break;
          this.openForm(typeof (LoanUpdateToolMainForm));
          break;
        case 207216553:
          if (!(tag == "ServerManager"))
            break;
          if (EnConfigurationSettings.GlobalSettings.ServerMode == ServerMode.Network)
          {
            this.openForm(typeof (NetworkServerManager));
            break;
          }
          if (EnConfigurationSettings.GlobalSettings.ServerMode == ServerMode.IIS)
          {
            this.openForm(typeof (IIsServerManager));
            break;
          }
          if (Utils.Dialog((IWin32Window) this, "The Encompass Server on this machine has not been configured. Would you like to run the Server Configuration Wizard now to install the Encompass Server?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
            break;
          EllieMae.EMLite.AdminTools.AdminTools.StartServerConfig();
          break;
        case 669185655:
          if (!(tag == "SettingsManager") || !this.EnsureSessionStarted(LoginType.All))
            break;
          this.openForm(typeof (ServerSettingsManager));
          break;
        case 1107347489:
          if (!(tag == "Contour"))
            break;
          this.execApp("ContourMigration.exe", "");
          break;
        case 1395864308:
          if (!(tag == "RegisterSDK"))
            break;
          this.execApp("SDKConfig.exe", "");
          break;
        case 1613761408:
          if (!(tag == "LogManager") || !this.EnsureSessionStarted())
            break;
          this.openForm(typeof (LogManager));
          break;
        case 1616342224:
          if (!(tag == "ReportExtendedDB"))
            break;
          bool useERDB = false;
          if (!this.EnsureSessionStarted(LoginType.All))
            break;
          try
          {
            if (!LoanXDBFieldListUpdateDialog.VerifyServerFieldList((IWin32Window) this))
              break;
            LoanXDBStatusInfo loanXdbStatus = Session.LoanManager.GetLoanXDBStatus(useERDB);
            if (loanXdbStatus != null)
            {
              if (loanXdbStatus.Status != LoanXDBStatus.Locked || EnConfigurationSettings.GlobalSettings.Debug)
              {
                this.openForm(typeof (LoanXDBManager), (object) useERDB);
                break;
              }
              int num = (int) Utils.Dialog((IWin32Window) this, "The" + (useERDB ? " External" : "") + " Reporting Database currently is locked by someone for maintenance. Please try it later.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              break;
            }
            int num1 = (int) Utils.Dialog((IWin32Window) this, "Unable to get" + (useERDB ? " External" : "") + " Reporting Database status. Please make sure the SQL server is up and running and accessible.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            break;
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
        case 1663220541:
          if (!(tag == "EventViewer") || !this.EnsureSessionStarted())
            break;
          this.openForm(typeof (EventManager));
          break;
        case 1670302706:
          if (!(tag == "DBManager"))
            break;
          this.openForm(typeof (DbServerManager));
          break;
        case 1704801983:
          if (!(tag == "DBUserManager"))
            break;
          this.openForm(typeof (DbManager));
          break;
        case 1819004303:
          if (!(tag == "CTDMTool") || !this.EnsureSessionStarted(LoginType.All))
            break;
          this.openForm(typeof (SCTLoanUpdateToolMainForm));
          break;
        case 2207813016:
          if (!(tag == "Registration"))
            break;
          EllieMae.EMLite.AdminTools.AdminTools.registerEncompass();
          break;
        case 2937325598:
          if (!(tag == "VersionManager") || !this.EnsureSessionStarted(AssemblyResolver.IsSmartClient ? LoginType.Server : LoginType.Offline))
            break;
          if (AssemblyResolver.IsSmartClient)
          {
            this.openForm(typeof (VersionManagerSC));
            break;
          }
          this.openForm(typeof (VersionManager));
          break;
        case 3220906098:
          if (!(tag == "SettingsTool"))
            break;
          this.execApp("SettingsTool.exe", "");
          break;
        case 3341772985:
          if (!(tag == "Genesis"))
            break;
          this.execApp("Gen2EncImp.exe", "");
          break;
        case 3977856619:
          if (!(tag == "EnhancedConditionsTool"))
            break;
          this.openForm(typeof (MainForm));
          break;
        case 4149473673:
          if (!(tag == "UserManager") || !this.EnsureSessionStarted())
            break;
          this.openForm(typeof (SystemManager));
          break;
      }
    }

    public bool EnsureSessionStarted(LoginType loginType)
    {
      Bitmask bitmask = new Bitmask((object) loginType);
      if (Session.IsConnected)
      {
        if (bitmask.Contains((object) LoginType.Server) && !Session.Connection.IsServerInProcess || bitmask.Contains((object) LoginType.Offline) && Session.Connection.IsServerInProcess)
          return true;
        this.logout();
      }
      using (Form loginForm = LoginFormFactory.GetLoginForm(loginType, "", "", LoginUtil.DefaultInstanceID, EllieMae.EMLite.AdminTools.AdminTools.argumentExists("-us")))
      {
        if (loginForm is LoginForm)
        {
          int num1 = (int) loginForm.ShowDialog();
        }
        else
        {
          int num2 = (int) loginForm.ShowDialog();
        }
      }
      if (Session.IsConnected)
      {
        this.logoutMenuItem.Visible = true;
        this.logoutMenuSeparator.Visible = true;
        if (Session.Connection.IsServerInProcess)
          this.Text = "Encompass Admin Tools - Offline";
        else
          this.Text = "Encompass Admin Tools - " + ((ConnectionBase) Session.Connection).Server.Uri.Host;
        if (string.Equals(EllieMae.EMLite.AdminTools.AdminTools.serverLogListenerOwner, Session.Connection.Session.SessionID))
        {
          DiagConfig<ClientDiagConfigData>.Instance.RemoveGlobalData("SessionId");
          DiagConfig<ClientDiagConfigData>.Instance.RemoveGlobalData("UserId");
          DiagConfig<ClientDiagConfigData>.Instance.RemoveHandler((IDiagConfigChangeHandler<ClientDiagConfigData>) EllieMae.EMLite.AdminTools.AdminTools.configChangeHandler);
          EllieMae.EMLite.AdminTools.AdminTools.configChangeHandler = (RemotingLogConfigChangeHandler) null;
          EllieMae.EMLite.AdminTools.AdminTools.serverLogListenerOwner = (string) null;
        }
        if (EllieMae.EMLite.AdminTools.AdminTools.serverLogListenerOwner == null)
        {
          DiagUtility.LoggerScopeProvider?.ModifyGlobal((Action<ILoggerScopeBuilder>) (globalScope => globalScope.SetInstance(LoginUtil.DefaultInstanceID)));
          DiagConfig<ClientDiagConfigData>.Instance.SetGlobalData<string>("SessionId", Session.Connection.Session.SessionID);
          DiagConfig<ClientDiagConfigData>.Instance.SetGlobalData<string>("UserId", Session.Connection.Session.UserID);
          DiagConfig<ClientDiagConfigData>.Instance.ReloadConfig();
          EllieMae.EMLite.AdminTools.AdminTools.configChangeHandler = new RemotingLogConfigChangeHandler((IRemotingLogConsumer) Session.SessionObjects.ServerManager);
          DiagConfig<ClientDiagConfigData>.Instance.AddHandler((IDiagConfigChangeHandler<ClientDiagConfigData>) EllieMae.EMLite.AdminTools.AdminTools.configChangeHandler);
          EllieMae.EMLite.AdminTools.AdminTools.serverLogListenerOwner = Session.Connection.Session.SessionID;
        }
      }
      return Session.IsConnected;
    }

    public bool EnsureSessionStarted() => this.EnsureSessionStarted(LoginType.Server);

    private static string getCommandLineArgument(string cmd)
    {
      string[] commandLineArgs = Environment.GetCommandLineArgs();
      for (int index = 1; index < commandLineArgs.Length; ++index)
      {
        if (commandLineArgs[index - 1] == cmd)
          return commandLineArgs[index];
      }
      return (string) null;
    }

    private static bool argumentExists(string arg)
    {
      foreach (string commandLineArg in Environment.GetCommandLineArgs())
      {
        if (arg == commandLineArg.ToLower())
          return true;
      }
      return false;
    }

    private void onApplicationExit(object sender, EventArgs e)
    {
      try
      {
        if (Session.IsConnected)
          Session.End();
      }
      catch
      {
      }
      WebLoginUtil.StopBrowserEngine(AppName.AdminTools.ToString());
    }

    private GVItem findListItemByTag(string tag)
    {
      for (int nItemIndex = 0; nItemIndex < this.gvTools.Items.Count; ++nItemIndex)
      {
        if (this.gvTools.Items[nItemIndex].Tag.ToString() == tag)
          return this.gvTools.Items[nItemIndex];
      }
      return (GVItem) null;
    }

    private void logoutMenuItem_Click(object sender, EventArgs e) => this.logout();

    private void logout()
    {
      try
      {
        this.closeForm(typeof (ServerSettingsManager));
        this.closeForm(typeof (SystemManager));
        this.closeForm(typeof (LogManager));
        this.closeForm(typeof (EventManager));
        Session.End();
      }
      catch
      {
      }
      this.logoutMenuItem.Visible = false;
      this.logoutMenuSeparator.Visible = false;
      EllieMae.EMLite.AdminTools.AdminTools.mainForm.Text = "Encompass Admin Tools";
    }

    private void openForm(System.Type formType, params object[] args)
    {
      string name = formType.Name;
      System.Type[] types = System.Type.EmptyTypes;
      if (formType == typeof (LoanXDBManager))
      {
        name += (bool) args[0] ? "|UseERDB" : "";
        types = new System.Type[1]{ typeof (bool) };
      }
      Form form1 = (Form) this.formCache[(object) name];
      if (form1 != null && !form1.Disposing && !form1.IsDisposed)
      {
        form1.WindowState = FormWindowState.Normal;
        form1.BringToFront();
        form1.Focus();
      }
      else
      {
        Form form2 = (Form) formType.GetConstructor(types).Invoke(args);
        this.formCache[(object) name] = (object) form2;
        if (form2 is SystemManager)
          ((SystemManager) form2).SessionTerminatedEvent += new EventHandler(this.onSessionTerminated);
        form2.Show();
        form2.Focus();
      }
    }

    private void closeForm(System.Type formType)
    {
      Form form = (Form) this.formCache[(object) formType.Name];
      if (form == null || form.Disposing || form.IsDisposed)
        return;
      form.Close();
      this.formCache[(object) formType.Name] = (object) null;
    }

    private void execApp(string name, string args)
    {
      try
      {
        Process.Start(Path.Combine(EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory, name), args);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error launching application: " + ex.Message);
      }
    }

    public static Color FormBackgroundColor => SystemColors.Control;

    public static bool StartServerConfig() => EllieMae.EMLite.AdminTools.AdminTools.StartServerConfig((string) null);

    public static bool StartServerConfig(string args)
    {
      try
      {
        Process process = new Process();
        process.StartInfo.FileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "ServerConfig.exe");
        process.StartInfo.WorkingDirectory = Path.GetDirectoryName(Application.ExecutablePath);
        process.StartInfo.Arguments = args;
        if (EnConfigurationSettings.InstanceName != "")
        {
          ProcessStartInfo startInfo = process.StartInfo;
          startInfo.Arguments = startInfo.Arguments + " -i " + EnConfigurationSettings.InstanceName;
        }
        process.Start();
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "An error occurred while attempting to start the Server Configuration Wizard: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
    }

    private static void registerEncompass()
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        using (InProcConnection inProcConnection = new InProcConnection())
        {
          inProcConnection.OpenTrusted();
          if (((IConfigurationManager) inProcConnection.Session.GetObject("ConfigurationManager")).GetServerLicense().IsTrialVersion)
            EllieMae.EMLite.AdminTools.AdminTools.StartServerConfig("-purchase");
          else
            EllieMae.EMLite.AdminTools.AdminTools.StartServerConfig("-register");
        }
      }
      catch
      {
        EllieMae.EMLite.AdminTools.AdminTools.StartServerConfig("-register");
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void onSessionTerminated(object sender, EventArgs e) => this.logout();

    private void adminToolsHelpToolStripMenuItem_Click(object sender, EventArgs e)
    {
      JedHelp.ShowHelp("AdminToolsWelcome");
    }

    public void OpenURL(string url, string title, int width, int height)
    {
      WebViewer.OpenURL(url, title, width, height);
    }

    public Form OpenURL(string windowName, string url, string title, int width, int height)
    {
      return WebViewer.OpenURL(windowName, url, title, width, height);
    }
  }
}
