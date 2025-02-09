// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.VersionManager
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.AdminTools.Properties;
using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.IIs;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.VersionInterface15;
using EllieMae.EMLite.WebServices;
using EllieMae.Encompass.EncAppMgr;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Web.Services.Protocols;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class VersionManager : Form
  {
    private System.ComponentModel.Container components;
    private VersionDownloadItem newVersionInfo;
    private string sw = Tracing.SwVersionControl;
    private Button btnClose;
    private Label label3;
    private RadioButton radManual;
    private RadioButton radAuto;
    private TableContainer grpHotfixes;
    private GridView gvVersions;
    private FlowLayoutPanel flpButtons;
    private Button btnAddTestUser;
    private Button btnTest;
    private VerticalSeparator verticalSeparator1;
    private Button btnApprove;
    private const string className = "VersionManager";
    private VersionManagementGroup productionGroup;
    private BorderPanel pnlUpgradeLinks;
    private PictureBox pictureBox2;
    private Label lblMajorUpgrade;
    private Label label1;
    private GroupContainer groupContainer1;
    private GroupContainer groupContainer2;
    private Label label4;
    private System.Windows.Forms.LinkLabel lnkReleaseNotes;
    private System.Windows.Forms.LinkLabel lnkUpgrade;
    private System.Windows.Forms.LinkLabel lnkMajorReleaseNotes;
    private HelpLink lnkHelp;
    private GroupContainer groupContainer3;
    private Button btnInstallHotUpdates;
    private CheckBox chkBoxUseHTTPS;
    private FlowLayoutPanel flpSHU;
    private TabControlEx tabControlEx1;
    private TabPageEx tabPageClient;
    private TabPageEx tabPageSHU;
    private TabPageEx tabPageSMU;
    private GroupContainer groupContainer4;
    private System.Windows.Forms.LinkLabel linkLabel1;
    private Label label6;
    private Label label5;
    private ComboBox cmbBoxShuSettingTime;
    private ComboBox cmbBoxShuSettingDay;
    private ComboBox cmbBoxShuSetting;
    private Button btnReset;
    private Button btnSaveShuSettings;
    private Label label8;
    private TextBox textBoxShuSettingNotificationTime;
    private Label label7;
    private GroupContainer groupContainer5;
    private System.Windows.Forms.LinkLabel linkSRelease;
    private CheckBox chkBoxUseHTTPSU;
    private Button btnInstallMajorUpdates;
    private GridView gvServerHotVersions;
    private GridView gvServerVersions;
    private bool updateInProgress;
    private VersionManagementGroup testGroup;
    private Hashtable versionmgrSettings;
    private const string encAppMgrIpcUrl = "ipc://EncAppMgr/EncAppMgrRO.rem";

    private bool shuSettingsDirty
    {
      set => this.btnSaveShuSettings.Enabled = value;
      get => this.btnSaveShuSettings.Enabled;
    }

    static VersionManager()
    {
      WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
    }

    public VersionManager()
    {
      this.InitializeComponent();
      this.tabControlEx1.TabPages.Remove(this.tabPageSHU);
      if (!this.showServerVersionManagerTab || Session.StartupInfo.RuntimeEnvironment == RuntimeEnvironment.Hosted)
      {
        this.tabControlEx1.TabPages.Remove(this.tabPageSMU);
        this.tabControlEx1.SelectedPage = this.tabPageClient;
      }
      else
      {
        this.tabControlEx1.TabPages.Remove(this.tabPageClient);
        this.tabControlEx1.SelectedPage = this.tabPageSMU;
      }
      this.setRadioButtonUseHTTPS();
      this.loadVersionSettings();
      this.pnlUpgradeLinks.Visible = this.checkForMajorUpgrade();
    }

    private bool showServerVersionManagerTab => true;

    private void setRadioButtonUseHTTPS()
    {
      this.chkBoxUseHTTPS.Visible = this.chkBoxUseHTTPSU.Visible = EnConfigurationSettings.GlobalSettings.ServerMode == ServerMode.IIS;
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass"))
      {
        string str = (string) registryKey.GetValue("SuUseHttps");
        if (!((str ?? "").Trim() != ""))
          return;
        if (this.tabControlEx1.SelectedPage == this.tabPageSHU)
        {
          this.chkBoxUseHTTPS.Checked = str.Trim() == "1";
        }
        else
        {
          if (this.tabControlEx1.SelectedPage != this.tabPageSMU)
            return;
          this.chkBoxUseHTTPSU.Checked = str.Trim() == "1";
        }
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (VersionManager));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      GVColumn gvColumn14 = new GVColumn();
      this.btnClose = new Button();
      this.label1 = new Label();
      this.tabControlEx1 = new TabControlEx();
      this.tabPageClient = new TabPageEx();
      this.groupContainer1 = new GroupContainer();
      this.label3 = new Label();
      this.grpHotfixes = new TableContainer();
      this.lnkReleaseNotes = new System.Windows.Forms.LinkLabel();
      this.flpButtons = new FlowLayoutPanel();
      this.btnAddTestUser = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.btnApprove = new Button();
      this.btnTest = new Button();
      this.gvVersions = new GridView();
      this.radAuto = new RadioButton();
      this.radManual = new RadioButton();
      this.tabPageSHU = new TabPageEx();
      this.groupContainer4 = new GroupContainer();
      this.label8 = new Label();
      this.textBoxShuSettingNotificationTime = new TextBox();
      this.label7 = new Label();
      this.btnReset = new Button();
      this.btnSaveShuSettings = new Button();
      this.label6 = new Label();
      this.label5 = new Label();
      this.cmbBoxShuSettingTime = new ComboBox();
      this.cmbBoxShuSettingDay = new ComboBox();
      this.cmbBoxShuSetting = new ComboBox();
      this.linkLabel1 = new System.Windows.Forms.LinkLabel();
      this.groupContainer3 = new GroupContainer();
      this.flpSHU = new FlowLayoutPanel();
      this.btnInstallHotUpdates = new Button();
      this.chkBoxUseHTTPS = new CheckBox();
      this.gvServerHotVersions = new GridView();
      this.tabPageSMU = new TabPageEx();
      this.groupContainer5 = new GroupContainer();
      this.gvServerVersions = new GridView();
      this.btnInstallMajorUpdates = new Button();
      this.chkBoxUseHTTPSU = new CheckBox();
      this.linkSRelease = new System.Windows.Forms.LinkLabel();
      this.lnkHelp = new HelpLink();
      this.groupContainer2 = new GroupContainer();
      this.pictureBox2 = new PictureBox();
      this.pnlUpgradeLinks = new BorderPanel();
      this.lnkUpgrade = new System.Windows.Forms.LinkLabel();
      this.lnkMajorReleaseNotes = new System.Windows.Forms.LinkLabel();
      this.lblMajorUpgrade = new Label();
      this.label4 = new Label();
      this.tabPageClient.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.grpHotfixes.SuspendLayout();
      this.flpButtons.SuspendLayout();
      this.tabPageSHU.SuspendLayout();
      this.groupContainer4.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.flpSHU.SuspendLayout();
      this.tabPageSMU.SuspendLayout();
      this.groupContainer5.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      ((ISupportInitialize) this.pictureBox2).BeginInit();
      this.pnlUpgradeLinks.SuspendLayout();
      this.SuspendLayout();
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Location = new Point(526, 614);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(120, 32);
      this.btnClose.TabIndex = 9;
      this.btnClose.Text = "&Close";
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.label1.Location = new Point(16, 15);
      this.label1.Name = "label1";
      this.label1.Size = new Size(1018, 32);
      this.label1.TabIndex = 15;
      this.label1.Text = "Use this tool to manage how and when updates are applied to your Encompass software.";
      this.tabControlEx1.Location = new Point(13, 51);
      this.tabControlEx1.Name = "tabControlEx1";
      this.tabControlEx1.Size = new Size(1025, 671);
      this.tabControlEx1.TabHeight = 20;
      this.tabControlEx1.TabIndex = 20;
      this.tabControlEx1.TabPages.Add(this.tabPageClient);
      this.tabControlEx1.TabPages.Add(this.tabPageSHU);
      this.tabControlEx1.TabPages.Add(this.tabPageSMU);
      this.tabControlEx1.Text = "tabControlEx1";
      this.tabControlEx1.SelectedIndexChanged += new EventHandler(this.tabControlEx1_SelectedIndexChanged);
      this.tabPageClient.BackColor = System.Drawing.Color.Transparent;
      this.tabPageClient.Controls.Add((Control) this.groupContainer1);
      this.tabPageClient.Location = new Point(1, 23);
      this.tabPageClient.Name = "tabPageClient";
      this.tabPageClient.TabIndex = 0;
      this.tabPageClient.TabWidth = 150;
      this.tabPageClient.Text = "Client";
      this.tabPageClient.Value = (object) "Client";
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.grpHotfixes);
      this.groupContainer1.Controls.Add((Control) this.radAuto);
      this.groupContainer1.Controls.Add((Control) this.radManual);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(200, 100);
      this.groupContainer1.TabIndex = 16;
      this.groupContainer1.Text = "Service Pack & Critical Patch Management";
      this.label3.Location = new Point(8, 32);
      this.label3.Name = "label3";
      this.label3.Size = new Size(623, 117);
      this.label3.TabIndex = 12;
      this.label3.Text = componentResourceManager.GetString("label3.Text");
      this.grpHotfixes.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpHotfixes.Controls.Add((Control) this.lnkReleaseNotes);
      this.grpHotfixes.Controls.Add((Control) this.flpButtons);
      this.grpHotfixes.Controls.Add((Control) this.gvVersions);
      this.grpHotfixes.Location = new Point(6, -86);
      this.grpHotfixes.Name = "grpHotfixes";
      this.grpHotfixes.Size = new Size(187, 180);
      this.grpHotfixes.TabIndex = 3;
      this.grpHotfixes.Text = "Service Packs & Critical Patches";
      this.lnkReleaseNotes.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lnkReleaseNotes.AutoSize = true;
      this.lnkReleaseNotes.BackColor = System.Drawing.Color.Transparent;
      this.lnkReleaseNotes.LinkArea = new LinkArea(63, 14);
      this.lnkReleaseNotes.Location = new Point(8, 160);
      this.lnkReleaseNotes.Name = "lnkReleaseNotes";
      this.lnkReleaseNotes.Size = new Size(562, 24);
      this.lnkReleaseNotes.TabIndex = 2;
      this.lnkReleaseNotes.TabStop = true;
      this.lnkReleaseNotes.Text = "For information about the features in each update, refer to the release notes.";
      this.lnkReleaseNotes.UseCompatibleTextRendering = true;
      this.lnkReleaseNotes.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkReleaseNotes_LinkClicked);
      this.flpButtons.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flpButtons.BackColor = System.Drawing.Color.Transparent;
      this.flpButtons.Controls.Add((Control) this.btnAddTestUser);
      this.flpButtons.Controls.Add((Control) this.verticalSeparator1);
      this.flpButtons.Controls.Add((Control) this.btnApprove);
      this.flpButtons.Controls.Add((Control) this.btnTest);
      this.flpButtons.Enabled = false;
      this.flpButtons.FlowDirection = FlowDirection.RightToLeft;
      this.flpButtons.Location = new Point(-146, 2);
      this.flpButtons.Name = "flpButtons";
      this.flpButtons.Size = new Size(328, 22);
      this.flpButtons.TabIndex = 1;
      this.btnAddTestUser.Location = new Point(213, 0);
      this.btnAddTestUser.Margin = new Padding(0);
      this.btnAddTestUser.Name = "btnAddTestUser";
      this.btnAddTestUser.Size = new Size(115, 22);
      this.btnAddTestUser.TabIndex = 3;
      this.btnAddTestUser.Text = "Manage Test Users";
      this.btnAddTestUser.UseVisualStyleBackColor = true;
      this.btnAddTestUser.Click += new EventHandler(this.btnAddTestUser_Click);
      this.verticalSeparator1.Location = new Point(208, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 7;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.btnApprove.Enabled = false;
      this.btnApprove.Location = new Point(138, 0);
      this.btnApprove.Margin = new Padding(0);
      this.btnApprove.Name = "btnApprove";
      this.btnApprove.Size = new Size(67, 22);
      this.btnApprove.TabIndex = 2;
      this.btnApprove.Text = "Approve";
      this.btnApprove.UseVisualStyleBackColor = true;
      this.btnApprove.Click += new EventHandler(this.btnApprove_Click);
      this.btnTest.Enabled = false;
      this.btnTest.Location = new Point(38, 0);
      this.btnTest.Margin = new Padding(0);
      this.btnTest.Name = "btnTest";
      this.btnTest.Size = new Size(100, 22);
      this.btnTest.TabIndex = 1;
      this.btnTest.Text = "Send To Testing";
      this.btnTest.UseVisualStyleBackColor = true;
      this.btnTest.Click += new EventHandler(this.btnTest_Click);
      this.gvVersions.AllowMultiselect = false;
      this.gvVersions.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Version";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column4";
      gvColumn2.Text = "Status";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column2";
      gvColumn3.Text = "Description";
      gvColumn3.Width = 280;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column3";
      gvColumn4.Text = "Release Date";
      gvColumn4.Width = 80;
      this.gvVersions.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.gvVersions.Dock = DockStyle.Fill;
      this.gvVersions.HotTrackingColor = System.Drawing.Color.FromArgb(250, 248, 188);
      this.gvVersions.Location = new Point(1, 26);
      this.gvVersions.Name = "gvVersions";
      this.gvVersions.Selectable = false;
      this.gvVersions.Size = new Size(185, 128);
      this.gvVersions.SortIconVisible = false;
      this.gvVersions.SortOption = GVSortOption.None;
      this.gvVersions.TabIndex = 0;
      this.gvVersions.SelectedIndexChanged += new EventHandler(this.gvVersions_SelectedIndexChanged);
      this.radAuto.AutoSize = true;
      this.radAuto.Location = new Point(23, 163);
      this.radAuto.Name = "radAuto";
      this.radAuto.Size = new Size(666, 24);
      this.radAuto.TabIndex = 1;
      this.radAuto.TabStop = true;
      this.radAuto.Text = "Automatically apply all Encompass service packs and critical patches as they are released.";
      this.radAuto.UseVisualStyleBackColor = true;
      this.radAuto.CheckedChanged += new EventHandler(this.onUpdateMethodChanged);
      this.radManual.Location = new Point(23, 189);
      this.radManual.Name = "radManual";
      this.radManual.Size = new Size(599, 46);
      this.radManual.TabIndex = 2;
      this.radManual.TabStop = true;
      this.radManual.Text = componentResourceManager.GetString("radManual.Text");
      this.radManual.UseVisualStyleBackColor = true;
      this.tabPageSHU.BackColor = System.Drawing.Color.Transparent;
      this.tabPageSHU.Controls.Add((Control) this.groupContainer4);
      this.tabPageSHU.Location = new Point(1, 23);
      this.tabPageSHU.Name = "tabPageSHU";
      this.tabPageSHU.TabIndex = 0;
      this.tabPageSHU.TabWidth = 150;
      this.tabPageSHU.Text = "Server Hot Updates";
      this.tabPageSHU.Value = (object) "Server Hot Updates";
      this.groupContainer4.Controls.Add((Control) this.label8);
      this.groupContainer4.Controls.Add((Control) this.textBoxShuSettingNotificationTime);
      this.groupContainer4.Controls.Add((Control) this.label7);
      this.groupContainer4.Controls.Add((Control) this.btnReset);
      this.groupContainer4.Controls.Add((Control) this.btnSaveShuSettings);
      this.groupContainer4.Controls.Add((Control) this.label6);
      this.groupContainer4.Controls.Add((Control) this.label5);
      this.groupContainer4.Controls.Add((Control) this.cmbBoxShuSettingTime);
      this.groupContainer4.Controls.Add((Control) this.cmbBoxShuSettingDay);
      this.groupContainer4.Controls.Add((Control) this.cmbBoxShuSetting);
      this.groupContainer4.Controls.Add((Control) this.linkLabel1);
      this.groupContainer4.Controls.Add((Control) this.groupContainer3);
      this.groupContainer4.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer4.Location = new Point(-1, 4);
      this.groupContainer4.Name = "groupContainer4";
      this.groupContainer4.Size = new Size(637, 338);
      this.groupContainer4.TabIndex = 0;
      this.groupContainer4.Text = "Server Hot Update Management";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(192, 101);
      this.label8.Name = "label8";
      this.label8.Size = new Size(295, 20);
      this.label8.TabIndex = 29;
      this.label8.Text = "seconds before the server update starts.";
      this.textBoxShuSettingNotificationTime.Location = new Point(88, 98);
      this.textBoxShuSettingNotificationTime.Name = "textBoxShuSettingNotificationTime";
      this.textBoxShuSettingNotificationTime.Size = new Size(100, 26);
      this.textBoxShuSettingNotificationTime.TabIndex = 28;
      this.textBoxShuSettingNotificationTime.TextChanged += new EventHandler(this.textBoxShuSettingNotificationTime_TextChanged);
      this.textBoxShuSettingNotificationTime.KeyPress += new KeyPressEventHandler(this.textBoxShuSettingNotificationTime_KeyPress);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(13, 101);
      this.label7.Name = "label7";
      this.label7.Size = new Size(96, 20);
      this.label7.TabIndex = 27;
      this.label7.Text = "Notify users ";
      this.btnReset.Location = new Point(544, 96);
      this.btnReset.Name = "btnReset";
      this.btnReset.Size = new Size(75, 23);
      this.btnReset.TabIndex = 26;
      this.btnReset.Text = "Reset";
      this.btnReset.UseVisualStyleBackColor = true;
      this.btnReset.Click += new EventHandler(this.btnReset_Click);
      this.btnSaveShuSettings.Enabled = false;
      this.btnSaveShuSettings.Location = new Point(455, 97);
      this.btnSaveShuSettings.Name = "btnSaveShuSettings";
      this.btnSaveShuSettings.Size = new Size(83, 23);
      this.btnSaveShuSettings.TabIndex = 25;
      this.btnSaveShuSettings.Text = "Save Settings";
      this.btnSaveShuSettings.UseVisualStyleBackColor = true;
      this.btnSaveShuSettings.Click += new EventHandler(this.btnUpdateShuSettings_Click);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(257, 70);
      this.label6.Name = "label6";
      this.label6.Size = new Size(23, 20);
      this.label6.TabIndex = 24;
      this.label6.Text = "at";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(13, 70);
      this.label5.Name = "label5";
      this.label5.Size = new Size(150, 20);
      this.label5.TabIndex = 23;
      this.label5.Text = "Install new updates:";
      this.cmbBoxShuSettingTime.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxShuSettingTime.FormattingEnabled = true;
      this.cmbBoxShuSettingTime.Items.AddRange(new object[24]
      {
        (object) "12:00 AM",
        (object) "1:00 AM",
        (object) "2:00 AM",
        (object) "3:00 AM",
        (object) "4:00 AM",
        (object) "5:00 AM",
        (object) "6:00 AM",
        (object) "7:00 AM",
        (object) "8:00 AM",
        (object) "9:00 AM",
        (object) "10:00 AM",
        (object) "11:00 AM",
        (object) "12:00 PM",
        (object) "1:00 PM",
        (object) "2:00 PM",
        (object) "3:00 PM",
        (object) "4:00 PM",
        (object) "5:00 PM",
        (object) "6:00 PM",
        (object) "7:00 PM",
        (object) "8:00 PM",
        (object) "9:00 PM",
        (object) "10:00 PM",
        (object) "11:00 PM"
      });
      this.cmbBoxShuSettingTime.Location = new Point(278, 67);
      this.cmbBoxShuSettingTime.Name = "cmbBoxShuSettingTime";
      this.cmbBoxShuSettingTime.Size = new Size(80, 28);
      this.cmbBoxShuSettingTime.TabIndex = 22;
      this.cmbBoxShuSettingTime.SelectedIndexChanged += new EventHandler(this.cmbBoxShuSettingTime_SelectedIndexChanged);
      this.cmbBoxShuSettingDay.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxShuSettingDay.FormattingEnabled = true;
      this.cmbBoxShuSettingDay.Items.AddRange(new object[8]
      {
        (object) "Every day",
        (object) "Every Sunday",
        (object) "Every Monday",
        (object) "Every Tuesday",
        (object) "Every Wednesday",
        (object) "Every Thursday",
        (object) "Every Friday",
        (object) "Every Saturday"
      });
      this.cmbBoxShuSettingDay.Location = new Point(121, 67);
      this.cmbBoxShuSettingDay.Name = "cmbBoxShuSettingDay";
      this.cmbBoxShuSettingDay.Size = new Size(130, 28);
      this.cmbBoxShuSettingDay.TabIndex = 21;
      this.cmbBoxShuSettingDay.SelectedIndexChanged += new EventHandler(this.cmbBoxShuSettingDay_SelectedIndexChanged);
      this.cmbBoxShuSetting.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxShuSetting.FormattingEnabled = true;
      this.cmbBoxShuSetting.Items.AddRange(new object[3]
      {
        (object) "Install server hot updates automatically",
        (object) "Download server hot updates but let me choose whether to install them",
        (object) "Never download/install server hot updates"
      });
      this.cmbBoxShuSetting.Location = new Point(11, 35);
      this.cmbBoxShuSetting.Name = "cmbBoxShuSetting";
      this.cmbBoxShuSetting.Size = new Size(426, 28);
      this.cmbBoxShuSetting.TabIndex = 20;
      this.cmbBoxShuSetting.SelectedIndexChanged += new EventHandler(this.cmbBoxShuSetting_SelectedIndexChanged);
      this.linkLabel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.linkLabel1.AutoSize = true;
      this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
      this.linkLabel1.LinkArea = new LinkArea(63, 14);
      this.linkLabel1.Location = new Point(8, 318);
      this.linkLabel1.Name = "linkLabel1";
      this.linkLabel1.Size = new Size(562, 24);
      this.linkLabel1.TabIndex = 5;
      this.linkLabel1.TabStop = true;
      this.linkLabel1.Text = "For information about the features in each update, refer to the release notes.";
      this.linkLabel1.UseCompatibleTextRendering = true;
      this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
      this.groupContainer3.Controls.Add((Control) this.flpSHU);
      this.groupContainer3.Controls.Add((Control) this.gvServerHotVersions);
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(5, 126);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(622, 186);
      this.groupContainer3.TabIndex = 19;
      this.groupContainer3.Text = "Server Hot Updates";
      this.flpSHU.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flpSHU.BackColor = System.Drawing.Color.Transparent;
      this.flpSHU.Controls.Add((Control) this.btnInstallHotUpdates);
      this.flpSHU.Controls.Add((Control) this.chkBoxUseHTTPS);
      this.flpSHU.FlowDirection = FlowDirection.RightToLeft;
      this.flpSHU.Location = new Point(330, 2);
      this.flpSHU.Name = "flpSHU";
      this.flpSHU.Size = new Size(287, 22);
      this.flpSHU.TabIndex = 3;
      this.btnInstallHotUpdates.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnInstallHotUpdates.Enabled = false;
      this.btnInstallHotUpdates.Location = new Point(185, 0);
      this.btnInstallHotUpdates.Margin = new Padding(2, 0, 2, 0);
      this.btnInstallHotUpdates.Name = "btnInstallHotUpdates";
      this.btnInstallHotUpdates.Size = new Size(100, 22);
      this.btnInstallHotUpdates.TabIndex = 0;
      this.btnInstallHotUpdates.Text = "Install Updates";
      this.btnInstallHotUpdates.UseVisualStyleBackColor = true;
      this.btnInstallHotUpdates.Click += new EventHandler(this.btnInstallHotUpdates_Click);
      this.chkBoxUseHTTPS.AutoSize = true;
      this.chkBoxUseHTTPS.BackColor = System.Drawing.Color.Transparent;
      this.chkBoxUseHTTPS.Location = new Point(62, 0);
      this.chkBoxUseHTTPS.Margin = new Padding(3, 0, 2, 0);
      this.chkBoxUseHTTPS.Name = "chkBoxUseHTTPS";
      this.chkBoxUseHTTPS.Size = new Size(119, 24);
      this.chkBoxUseHTTPS.TabIndex = 2;
      this.chkBoxUseHTTPS.Text = "Use HTTPS";
      this.chkBoxUseHTTPS.UseVisualStyleBackColor = false;
      this.chkBoxUseHTTPS.CheckedChanged += new EventHandler(this.chkBoxUseHTTPS_CheckedChanged);
      this.gvServerHotVersions.AllowMultiselect = false;
      this.gvServerHotVersions.BorderStyle = BorderStyle.None;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column1";
      gvColumn5.Text = "Version";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column2";
      gvColumn6.Text = "Status";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column3";
      gvColumn7.Text = "Restart Required";
      gvColumn7.Width = 95;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column4";
      gvColumn8.Text = "Description";
      gvColumn8.Width = 220;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column5";
      gvColumn9.Text = "Release Date";
      gvColumn9.Width = 100;
      this.gvServerHotVersions.Columns.AddRange(new GVColumn[5]
      {
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9
      });
      this.gvServerHotVersions.Dock = DockStyle.Fill;
      this.gvServerHotVersions.HotTrackingColor = System.Drawing.Color.FromArgb(250, 248, 188);
      this.gvServerHotVersions.Location = new Point(1, 26);
      this.gvServerHotVersions.Name = "gvServerHotVersions";
      this.gvServerHotVersions.Size = new Size(620, 159);
      this.gvServerHotVersions.TabIndex = 4;
      this.gvServerHotVersions.SelectedIndexChanged += new EventHandler(this.gvServerHotVersions_SelectedIndexChanged);
      this.tabPageSMU.BackColor = System.Drawing.Color.Transparent;
      this.tabPageSMU.Controls.Add((Control) this.groupContainer5);
      this.tabPageSMU.Location = new Point(1, 23);
      this.tabPageSMU.Name = "tabPageSMU";
      this.tabPageSMU.TabIndex = 0;
      this.tabPageSMU.TabWidth = 150;
      this.tabPageSMU.Text = "Server Major Updates";
      this.tabPageSMU.Value = (object) "Server Major Updates";
      this.groupContainer5.Controls.Add((Control) this.gvServerVersions);
      this.groupContainer5.Controls.Add((Control) this.btnInstallMajorUpdates);
      this.groupContainer5.Controls.Add((Control) this.chkBoxUseHTTPSU);
      this.groupContainer5.Controls.Add((Control) this.linkSRelease);
      this.groupContainer5.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer5.Location = new Point(-1, 0);
      this.groupContainer5.Name = "groupContainer5";
      this.groupContainer5.Size = new Size(637, 343);
      this.groupContainer5.TabIndex = 14;
      this.groupContainer5.Text = "Server Update Management";
      this.gvServerVersions.AllowMultiselect = false;
      this.gvServerVersions.BorderStyle = BorderStyle.None;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column1";
      gvColumn10.Text = "Version";
      gvColumn10.Width = 100;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column2";
      gvColumn11.Text = "Status";
      gvColumn11.Width = 100;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "Column3";
      gvColumn12.Text = "Restart Required";
      gvColumn12.Width = 95;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "Column4";
      gvColumn13.Text = "Description";
      gvColumn13.Width = 220;
      gvColumn14.ImageIndex = -1;
      gvColumn14.Name = "Column5";
      gvColumn14.Text = "Release Date";
      gvColumn14.Width = 100;
      this.gvServerVersions.Columns.AddRange(new GVColumn[5]
      {
        gvColumn10,
        gvColumn11,
        gvColumn12,
        gvColumn13,
        gvColumn14
      });
      this.gvServerVersions.Dock = DockStyle.Fill;
      this.gvServerVersions.HotTrackingColor = System.Drawing.Color.FromArgb(250, 248, 188);
      this.gvServerVersions.Location = new Point(1, 26);
      this.gvServerVersions.Name = "gvServerVersions";
      this.gvServerVersions.Size = new Size(635, 316);
      this.gvServerVersions.TabIndex = 6;
      this.gvServerVersions.SelectedIndexChanged += new EventHandler(this.gvServerVersions_SelectedIndexChanged);
      this.btnInstallMajorUpdates.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnInstallMajorUpdates.Enabled = false;
      this.btnInstallMajorUpdates.Location = new Point(515, 3);
      this.btnInstallMajorUpdates.Margin = new Padding(2, 0, 2, 0);
      this.btnInstallMajorUpdates.Name = "btnInstallMajorUpdates";
      this.btnInstallMajorUpdates.Size = new Size(100, 22);
      this.btnInstallMajorUpdates.TabIndex = 0;
      this.btnInstallMajorUpdates.Text = "Install Updates";
      this.btnInstallMajorUpdates.UseVisualStyleBackColor = true;
      this.btnInstallMajorUpdates.Click += new EventHandler(this.btnInstallMajorUpdates_Click);
      this.chkBoxUseHTTPSU.AutoSize = true;
      this.chkBoxUseHTTPSU.BackColor = System.Drawing.Color.Transparent;
      this.chkBoxUseHTTPSU.Location = new Point(405, 7);
      this.chkBoxUseHTTPSU.Margin = new Padding(3, 0, 2, 0);
      this.chkBoxUseHTTPSU.Name = "chkBoxUseHTTPSU";
      this.chkBoxUseHTTPSU.Size = new Size(119, 24);
      this.chkBoxUseHTTPSU.TabIndex = 2;
      this.chkBoxUseHTTPSU.Text = "Use HTTPS";
      this.chkBoxUseHTTPSU.UseVisualStyleBackColor = false;
      this.chkBoxUseHTTPSU.CheckedChanged += new EventHandler(this.chkBoxUseHTTPS_CheckedChanged);
      this.linkSRelease.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.linkSRelease.AutoSize = true;
      this.linkSRelease.BackColor = System.Drawing.Color.Transparent;
      this.linkSRelease.LinkArea = new LinkArea(63, 14);
      this.linkSRelease.Location = new Point(8, 318);
      this.linkSRelease.Name = "linkSRelease";
      this.linkSRelease.Size = new Size(562, 24);
      this.linkSRelease.TabIndex = 5;
      this.linkSRelease.TabStop = true;
      this.linkSRelease.Text = "For information about the features in each update, refer to the release notes.";
      this.linkSRelease.UseCompatibleTextRendering = true;
      this.linkSRelease.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkSRelease_LinkClicked);
      this.lnkHelp.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lnkHelp.BackColor = System.Drawing.Color.Transparent;
      this.lnkHelp.Cursor = Cursors.Hand;
      this.lnkHelp.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lnkHelp.Location = new Point(13, 612);
      this.lnkHelp.Name = "lnkHelp";
      this.lnkHelp.Size = new Size(144, 25);
      this.lnkHelp.TabIndex = 18;
      this.lnkHelp.Help += new EventHandler(this.lnkHelp_Help);
      this.groupContainer2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer2.Controls.Add((Control) this.pictureBox2);
      this.groupContainer2.Controls.Add((Control) this.pnlUpgradeLinks);
      this.groupContainer2.Controls.Add((Control) this.lblMajorUpgrade);
      this.groupContainer2.Controls.Add((Control) this.label4);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(13, 446);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(631, 159);
      this.groupContainer2.TabIndex = 17;
      this.groupContainer2.Text = "Major Upgrade Management";
      this.pictureBox2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pictureBox2.Image = (Image) Resources.bullet;
      this.pictureBox2.Location = new Point(16, (int) sbyte.MaxValue);
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = new Size(7, 5);
      this.pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox2.TabIndex = 8;
      this.pictureBox2.TabStop = false;
      this.pnlUpgradeLinks.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.pnlUpgradeLinks.BackColor = System.Drawing.Color.Transparent;
      this.pnlUpgradeLinks.Borders = AnchorStyles.None;
      this.pnlUpgradeLinks.Controls.Add((Control) this.lnkUpgrade);
      this.pnlUpgradeLinks.Controls.Add((Control) this.lnkMajorReleaseNotes);
      this.pnlUpgradeLinks.Location = new Point(376, 115);
      this.pnlUpgradeLinks.Name = "pnlUpgradeLinks";
      this.pnlUpgradeLinks.Size = new Size(248, 33);
      this.pnlUpgradeLinks.TabIndex = 14;
      this.lnkUpgrade.AutoSize = true;
      this.lnkUpgrade.Location = new Point(128, 4);
      this.lnkUpgrade.Name = "lnkUpgrade";
      this.lnkUpgrade.Size = new Size(107, 19);
      this.lnkUpgrade.TabIndex = 12;
      this.lnkUpgrade.TabStop = true;
      this.lnkUpgrade.Text = "Upgrade Now";
      this.lnkUpgrade.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkUpgrade_LinkClicked);
      this.lnkMajorReleaseNotes.AutoSize = true;
      this.lnkMajorReleaseNotes.Location = new Point(2, 4);
      this.lnkMajorReleaseNotes.Name = "lnkMajorReleaseNotes";
      this.lnkMajorReleaseNotes.Size = new Size(113, 19);
      this.lnkMajorReleaseNotes.TabIndex = 11;
      this.lnkMajorReleaseNotes.TabStop = true;
      this.lnkMajorReleaseNotes.Text = "Release Notes";
      this.lnkMajorReleaseNotes.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkMajorReleaseNotes_LinkClicked);
      this.lblMajorUpgrade.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblMajorUpgrade.AutoEllipsis = true;
      this.lblMajorUpgrade.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblMajorUpgrade.Location = new Point(32, 120);
      this.lblMajorUpgrade.Name = "lblMajorUpgrade";
      this.lblMajorUpgrade.Size = new Size(337, 23);
      this.lblMajorUpgrade.TabIndex = 7;
      this.lblMajorUpgrade.Text = "No major upgrades of Encompass are available at this time.";
      this.label4.Location = new Point(13, 47);
      this.label4.Name = "label4";
      this.label4.Size = new Size(992, 70);
      this.label4.TabIndex = 15;
      this.label4.Text = componentResourceManager.GetString("label4.Text");
      this.AutoScaleBaseSize = new Size(8, 19);
      this.BackColor = System.Drawing.Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(656, 659);
      this.Controls.Add((Control) this.tabControlEx1);
      this.Controls.Add((Control) this.lnkHelp);
      this.Controls.Add((Control) this.groupContainer2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnClose);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.Name = nameof (VersionManager);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Encompass Version Manager";
      this.KeyDown += new KeyEventHandler(this.VersionManager_KeyDown);
      this.tabPageClient.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.grpHotfixes.ResumeLayout(false);
      this.grpHotfixes.PerformLayout();
      this.flpButtons.ResumeLayout(false);
      this.tabPageSHU.ResumeLayout(false);
      this.groupContainer4.ResumeLayout(false);
      this.groupContainer4.PerformLayout();
      this.groupContainer3.ResumeLayout(false);
      this.flpSHU.ResumeLayout(false);
      this.flpSHU.PerformLayout();
      this.tabPageSMU.ResumeLayout(false);
      this.groupContainer5.ResumeLayout(false);
      this.groupContainer5.PerformLayout();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      ((ISupportInitialize) this.pictureBox2).EndInit();
      this.pnlUpgradeLinks.ResumeLayout(false);
      this.pnlUpgradeLinks.PerformLayout();
      this.ResumeLayout(false);
    }

    private void loadVersionSettings()
    {
      foreach (VersionManagementGroup versionManagementGroup in Session.ServerManager.GetVersionManagementGroups())
      {
        if (versionManagementGroup.IsDefaultGroup)
          this.productionGroup = versionManagementGroup;
        else
          this.testGroup = versionManagementGroup;
      }
      if (this.productionGroup == null)
        throw new Exception("Default version management group is missing. Version management feature cannot be used");
      if (this.testGroup == null)
        throw new Exception("Testing version management group is missing. Version management feature cannot be used");
      if (this.productionGroup.AuthorizedVersion == null)
        this.radAuto.Checked = true;
      else
        this.radManual.Checked = true;
      this.gvVersions.Items.Clear();
      using (UpdateService updateService = new UpdateService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.UpdateServicesUrl))
      {
        ReleaseInfo[] versionHotfixes = updateService.GetVersionHotfixes(VersionInformation.CurrentVersion.Version.NormalizedVersion, Session.CompanyInfo.ClientID);
        for (int index = versionHotfixes.Length - 1; index >= 0; --index)
          this.gvServerVersions.Items.Add(this.createHotfixItem(versionHotfixes[index]));
        this.refreshVersionStatuses();
      }
      this.updateControlStates();
    }

    private void loadServerVersionSettings()
    {
      if (this.tabControlEx1.SelectedPage == this.tabPageSHU)
        this.resetShuSettings();
      this.gvServerVersions.Items.Clear();
      this.gvServerHotVersions.Items.Clear();
      string userid = "(rmi)";
      string password = EnConfigurationSettings.GlobalSettings.RMIPassword;
      if ((password ?? "").Trim() == "")
      {
        userid = "(trusted)";
        password = EnConfigurationSettings.GlobalSettings.DatabasePassword;
      }
      IEncAppMgrRO encAppMgrRo = (IEncAppMgrRO) Activator.GetObject(typeof (IEncAppMgrRO), "ipc://EncAppMgr/EncAppMgrRO.rem");
      string[] strArray = (string[]) null;
      try
      {
        VersionInformation.ReloadVersionInformation();
        strArray = this.tabControlEx1.SelectedPage != this.tabPageSHU ? encAppMgrRo.GetServerVersionUpdates(userid, password, Session.CompanyInfo.ClientID, VersionInformation.CurrentVersion.Version.FullVersion, "SMU") : encAppMgrRo.GetServerVersionUpdates(userid, password, Session.CompanyInfo.ClientID, VersionInformation.CurrentVersion.Version.FullVersion, "SHU");
      }
      catch (Exception ex)
      {
        this.Log(TraceLevel.Error, "Error getting server version hotfixes: " + ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) this, "Unable to connect to EncAppMgr: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      if (strArray == null)
        return;
      for (int index = strArray.Length - 1; index >= 0; --index)
      {
        if (this.tabControlEx1.SelectedPage == this.tabPageSHU)
          this.gvServerHotVersions.Items.Add(this.createServerHotUpdateItem(new ServerHotUpdateInfo(strArray[index])));
        else if (this.tabControlEx1.SelectedPage == this.tabPageSMU)
          this.gvServerVersions.Items.Add(this.createServerMajorUpdateItem(new ServerMajorUpdateInfo(strArray[index])));
      }
      this.refreshServerVersionStatuses(false);
    }

    private void resetShuSettings()
    {
      IDictionary serverSettings = Session.ServerManager.GetServerSettings("SHU");
      ShuSetting shuSetting = (ShuSetting) serverSettings[(object) "SHU.Setting"];
      ShuSettingDay shuSettingDay = (ShuSettingDay) serverSettings[(object) "SHU.SettingDay"];
      ShuSettingTime shuSettingTime = (ShuSettingTime) serverSettings[(object) "SHU.SettingTime"];
      int num = (int) serverSettings[(object) "SHU.SettingNotificationTime"];
      this.cmbBoxShuSetting.SelectedIndex = (int) shuSetting;
      this.cmbBoxShuSettingDay.SelectedIndex = (int) shuSettingDay;
      this.cmbBoxShuSettingTime.SelectedIndex = (int) shuSettingTime;
      this.textBoxShuSettingNotificationTime.Text = string.Concat((object) num);
      this.shuSettingsDirty = false;
    }

    private void updateShuSettings()
    {
      Session.ServerManager.UpdateServerSettings((IDictionary) new Dictionary<string, object>()
      {
        {
          "SHU.Setting",
          (object) (ShuSetting) this.cmbBoxShuSetting.SelectedIndex
        },
        {
          "SHU.SettingDay",
          (object) (ShuSettingDay) this.cmbBoxShuSettingDay.SelectedIndex
        },
        {
          "SHU.SettingTime",
          (object) (ShuSettingTime) this.cmbBoxShuSettingTime.SelectedIndex
        },
        {
          "SHU.SettingNotificationTime",
          (object) int.Parse(this.textBoxShuSettingNotificationTime.Text)
        }
      });
      this.shuSettingsDirty = false;
    }

    private string getDisplayVersionStringFromDescription(ReleaseInfo hotfix)
    {
      string stringFromDescription = (string) null;
      if (hotfix.Description.StartsWith("Encompass360 v"))
      {
        stringFromDescription = hotfix.Description.Substring(14);
        int length = stringFromDescription.IndexOf(" ");
        if (length > 0)
          stringFromDescription = stringFromDescription.Substring(0, length);
      }
      return stringFromDescription;
    }

    private GVItem createHotfixItem(ReleaseInfo hotfix)
    {
      string stringFromDescription = this.getDisplayVersionStringFromDescription(hotfix);
      ClientAppVersion clientAppVersion = new ClientAppVersion(hotfix.MajorVersion, hotfix.SequenceNumber, stringFromDescription);
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Value = (object) clientAppVersion.DisplayVersionString
          },
          [2] = {
            Value = (object) hotfix.Description
          },
          [3] = {
            Value = (object) hotfix.ReleaseDate.ToString("MM/dd/yyyy")
          }
        },
        Tag = (object) clientAppVersion
      };
    }

    private GVItem createServerHotUpdateItem(ServerHotUpdateInfo shu)
    {
      string version = shu.Version;
      string shortDateString = shu.ReleaseDate.ToShortDateString();
      string description = shu.Description;
      JedServerVersion jedServerVersion = new JedServerVersion(version);
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Value = (object) jedServerVersion.DisplayVersionString
          },
          [2] = {
            Value = shu.ServerRestartRequired ? (object) "Yes" : (object) "No",
            ForeColor = shu.ServerRestartRequired ? System.Drawing.Color.Red : System.Drawing.Color.Black
          },
          [3] = {
            Value = (object) description
          },
          [4] = {
            Value = (object) shortDateString
          }
        },
        Tag = (object) jedServerVersion
      };
    }

    private GVItem createServerMajorUpdateItem(ServerMajorUpdateInfo su)
    {
      string version = su.Version;
      string shortDateString = su.ReleaseDate.ToShortDateString();
      string description = su.Description;
      JedServerVersion jedServerVersion = new JedServerVersion(version);
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Value = (object) jedServerVersion.DisplayVersionString
          },
          [2] = {
            Value = (object) "Yes",
            ForeColor = su.ServerRestartRequired ? System.Drawing.Color.Red : System.Drawing.Color.Black
          },
          [3] = {
            Value = (object) description
          },
          [4] = {
            Value = (object) shortDateString
          }
        },
        Tag = (object) jedServerVersion
      };
    }

    private void refreshVersionStatuses()
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvVersions.Items)
      {
        VersionManager.AppVersionStatus appVersionStatus = this.getAppVersionStatus(gvItem.Tag as ClientAppVersion);
        gvItem.SubItems[1].Text = this.getVersionStatusDescription(appVersionStatus);
        gvItem.SubItems[1].ForeColor = this.getVersionStatusColor(appVersionStatus);
      }
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvServerHotVersions.Items)
      {
        VersionManager.AppVersionStatus appVersionStatus = this.getAppVersionStatus(gvItem.Tag as ClientAppVersion);
        gvItem.SubItems[1].Text = this.getVersionStatusDescription(appVersionStatus);
        gvItem.SubItems[1].ForeColor = this.getVersionStatusColor(appVersionStatus);
      }
    }

    private void refreshServerVersionStatuses(bool reloadVersionInformation)
    {
      if (reloadVersionInformation)
        VersionInformation.ReloadVersionInformation();
      int maxInstalledVersion = 0;
      if (this.tabControlEx1.SelectedPage == this.tabPageSHU)
        maxInstalledVersion = VersionInformation.CurrentVersion.GetMaxInstalledServerHotUpdateNumber();
      else if (this.tabControlEx1.SelectedPage == this.tabPageSMU)
        maxInstalledVersion = VersionInformation.CurrentVersion.GetMaxInstalledServerMajorUpdateNumber();
      string[] downloadedSuFiles = VersionInformation.CurrentVersion.GetDownloadedSuFiles(new System.Version(VersionInformation.CurrentVersion.Version.FullVersion + ".0"), this.tabControlEx1.SelectedPage == this.tabPageSHU);
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvServerVersions.Items)
      {
        VersionManager.SuVersionStatus suVersionStatus = this.getSuVersionStatus(gvItem.Tag as JedServerVersion, maxInstalledVersion, downloadedSuFiles);
        gvItem.SubItems[1].Text = this.getVersionStatusDescription(suVersionStatus);
        gvItem.SubItems[1].ForeColor = this.getVersionStatusColor(suVersionStatus);
      }
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvServerHotVersions.Items)
      {
        VersionManager.SuVersionStatus suVersionStatus = this.getSuVersionStatus(gvItem.Tag as JedServerVersion, maxInstalledVersion, downloadedSuFiles);
        gvItem.SubItems[1].Text = this.getVersionStatusDescription(suVersionStatus);
        gvItem.SubItems[1].ForeColor = this.getVersionStatusColor(suVersionStatus);
      }
      this.gvServerVersions_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    private VersionManager.SuVersionStatus getSuVersionStatus(
      JedServerVersion suVersion,
      int maxInstalledVersion,
      string[] downloadedSuFiles)
    {
      if (suVersion.UpdateNumber <= maxInstalledVersion)
        return VersionManager.SuVersionStatus.Installed;
      foreach (string downloadedSuFile in downloadedSuFiles)
      {
        if (suVersion.FullVersion == Path.GetFileNameWithoutExtension(downloadedSuFile))
          return VersionManager.SuVersionStatus.Downloaded;
      }
      return VersionManager.SuVersionStatus.New;
    }

    private VersionManager.AppVersionStatus getAppVersionStatus(ClientAppVersion appVersion)
    {
      if (this.productionGroup == null || this.productionGroup.AuthorizedVersion == null || appVersion.CompareTo((object) this.productionGroup.AuthorizedVersion) <= 0)
        return VersionManager.AppVersionStatus.Approved;
      return this.testGroup != null && this.testGroup.AuthorizedVersion != null && appVersion.CompareTo((object) this.testGroup.AuthorizedVersion) <= 0 ? VersionManager.AppVersionStatus.Testing : VersionManager.AppVersionStatus.New;
    }

    private System.Drawing.Color getVersionStatusColor(VersionManager.AppVersionStatus status)
    {
      if (status == VersionManager.AppVersionStatus.Testing)
        return System.Drawing.Color.DarkGoldenrod;
      return status == VersionManager.AppVersionStatus.Approved ? System.Drawing.Color.Green : EncompassColors.Alert2;
    }

    private System.Drawing.Color getVersionStatusColor(VersionManager.SuVersionStatus status)
    {
      if (status == VersionManager.SuVersionStatus.Downloaded)
        return System.Drawing.Color.DarkGoldenrod;
      return status == VersionManager.SuVersionStatus.Installed ? System.Drawing.Color.Green : EncompassColors.Alert2;
    }

    private string getVersionStatusDescription(VersionManager.AppVersionStatus status)
    {
      if (status == VersionManager.AppVersionStatus.Testing)
        return "In Testing";
      return status == VersionManager.AppVersionStatus.Approved ? "Approved" : "New";
    }

    private string getVersionStatusDescription(VersionManager.SuVersionStatus status)
    {
      if (status == VersionManager.SuVersionStatus.Downloaded)
        return "Downloaded";
      return status == VersionManager.SuVersionStatus.Installed ? "Installed" : "New";
    }

    private void onVersionLinkClick(object sender, EventArgs e)
    {
      Process.Start(((Element) sender).Tag.ToString());
    }

    private bool checkForMajorUpgrade()
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        try
        {
          LicenseInfo licenseInfo = this.retrieveLicenseInfo();
          if (licenseInfo == null)
            return false;
          using (UpdateService updateService = new UpdateService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.UpdateServicesUrl))
          {
            updateService.Proxy = WebRequest.DefaultWebProxy;
            updateService.Proxy.Credentials = CredentialCache.DefaultCredentials;
            string extendedVersion = VersionInformation.CurrentVersion.GetExtendedVersion(licenseInfo.Edition);
            this.Log(TraceLevel.Info, "Retrieving product update information from web service at " + updateService.Url + ". (ver = " + extendedVersion + ", cid = " + licenseInfo.ClientID + ")");
            this.newVersionInfo = this.parseUpdateServiceResponse(updateService.GetUpdateVersionInfo(extendedVersion, licenseInfo.ClientID));
          }
        }
        catch (WebException ex)
        {
          this.Log(TraceLevel.Warning, "Error communicating with server: " + ex.Message);
          this.lblMajorUpgrade.Text = "Encompass upgrade information cannot be determined at this time.";
          return false;
        }
        catch (SoapException ex)
        {
          string msg = ex.Message;
          if (msg.IndexOf("--> ") > 0)
            msg = msg.Substring(msg.IndexOf("--> ") + 4);
          this.Log(TraceLevel.Info, msg);
          this.lblMajorUpgrade.Text = "Encompass upgrade information cannot be determined at this time.";
          return false;
        }
        if (this.newVersionInfo == null)
        {
          this.Log(TraceLevel.Info, "No new update available from remote server");
          return false;
        }
        if (this.newVersionInfo.TargetVersion <= VersionControl.CurrentVersion.Version)
        {
          this.Log(TraceLevel.Info, "Remote server returned update information for version prior to one installed (" + this.newVersionInfo.TargetVersion.ToString() + ")");
          return false;
        }
        this.Log(TraceLevel.Info, "New version (" + this.newVersionInfo.TargetVersion.ToString() + ") available from remote server.");
        this.lblMajorUpgrade.Text = this.newVersionInfo.Description + " is now available.";
        return true;
      }
      catch (Exception ex)
      {
        this.Log(TraceLevel.Error, "Unexpected error while checking for new release: " + ex.ToString());
        return false;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void lnkUpgrade_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      try
      {
        VersionDownloadItem versionDownloadItem = this.fetchUpdateFile();
        if (versionDownloadItem == null || !this.ensureClientStopped() || SystemSettings.InstallationMode == InstallationMode.Server && versionDownloadItem.IsAffected(AffectedSystems.Server) && !this.ensureServerStopped())
          return;
        if (Utils.Dialog((IWin32Window) this, "The Encompass Admin Tools will now shut down in order to complete the update installation.", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel && Utils.Dialog((IWin32Window) this, "You are about to cancel the Encompass update. You may install this update at a later time by returning to the Version Manager in the Encompass Admin Tools.\r\n\r\nCancel now?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        {
          this.enableServer();
          this.Log(TraceLevel.Info, "Update process terminated by user.");
        }
        else if (this.startServerVersionUpdate(versionDownloadItem.FullUpdateFilename, versionDownloadItem.AffectedSystems))
          Application.Exit();
        else
          this.enableServer();
      }
      catch (Exception ex)
      {
        ErrorDialog.Display(ex);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void onUpdateMethodChanged(object sender, EventArgs e)
    {
      if (this.productionGroup.AuthorizedVersion == null && this.radManual.Checked)
        this.switchToManualUpdates();
      else if (this.productionGroup.AuthorizedVersion != null && this.radAuto.Checked)
        this.switchToAutoUpdates();
      this.updateControlStates();
      this.refreshVersionStatuses();
    }

    private void switchToManualUpdates()
    {
      if (Utils.ShowChekboxDialog("WARNING: Should you elect to manually accept Service Pack and Critical Patch updates you are accepting the risk associated. By accepting to manually update your Encompass instance, you acknowledge that you understand that this could cause the data between Encompass and any application, including ICE Mortgage Technology Connect products, built on the Encompass Lending Platform APIs to be out of sync. The ICE Mortgage Technology Platform APIs support the most current minor version(s) of Encompass. It is Highly Recommended that you accept updates automatically to ensure accurate data and calculations.", "I understand and accept the risk", "Encompass") != DialogResult.OK)
      {
        this.radAuto.Checked = true;
      }
      else
      {
        this.versionmgrSettings = CollectionsUtil.CreateCaseInsensitiveHashtable();
        this.versionmgrSettings.Add((object) "POLICIES.VersionMgrWarningMsg", (object) "Accepted");
        this.versionmgrSettings.Add((object) "POLICIES.VersionMgrWarningMsgDT", (object) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        this.versionmgrSettings.Add((object) "POLICIES.VersionMgrWarningMsgUser", (object) Session.UserID);
        Session.ServerManager.UpdateServerSettings((IDictionary) this.versionmgrSettings);
        ClientAppVersion clientAppVersion = new ClientAppVersion(VersionInformation.CurrentVersion.Version, 0, (string) null);
        if (this.gvVersions.Items.Count > 0)
          clientAppVersion = (ClientAppVersion) this.gvVersions.Items[0].Tag;
        this.productionGroup.AuthorizedVersion = clientAppVersion;
        this.testGroup.AuthorizedVersion = clientAppVersion;
        Session.ServerManager.UpdateVersionManagementGroup(this.productionGroup);
        Session.ServerManager.UpdateVersionManagementGroup(this.testGroup);
      }
    }

    private Persona[] getPersonas()
    {
      Persona[] allPersonas = Session.PersonaManager.GetAllPersonas();
      ArrayList arrayList = new ArrayList();
      foreach (Persona persona in allPersonas)
      {
        if (persona.Name != "Administrator" && persona.ID != 1 && persona.Name != "Super Administrator" && persona.ID != 0)
          arrayList.Add((object) persona);
      }
      return (Persona[]) arrayList.ToArray(typeof (Persona));
    }

    private void switchToAutoUpdates()
    {
      if (Utils.Dialog((IWin32Window) this, "By switching to the automatic approval option, all Encompass hot updates will be applied to your users' computers as soon as they are available. This setting does not affect major product upgrades, which you must still perform manually." + Environment.NewLine + Environment.NewLine + "Are you sure you want to switch to the automatic approval option for Encompass hot updates?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
      {
        this.radManual.Checked = true;
      }
      else
      {
        this.versionmgrSettings = CollectionsUtil.CreateCaseInsensitiveHashtable();
        this.versionmgrSettings.Add((object) "POLICIES.VersionMgrWarningMsg", (object) null);
        this.versionmgrSettings.Add((object) "POLICIES.VersionMgrWarningMsgDT", (object) null);
        this.versionmgrSettings.Add((object) "POLICIES.VersionMgrWarningMsgUser", (object) null);
        Session.ServerManager.UpdateServerSettings((IDictionary) this.versionmgrSettings);
        this.productionGroup.AuthorizedVersion = (ClientAppVersion) null;
        this.testGroup.AuthorizedVersion = (ClientAppVersion) null;
        Session.ServerManager.UpdateVersionManagementGroup(this.productionGroup);
        Session.ServerManager.UpdateVersionManagementGroup(this.testGroup);
      }
    }

    private void updateControlStates()
    {
      this.flpButtons.Enabled = this.radManual.Checked;
      this.gvVersions.Selectable = this.radManual.Checked;
    }

    private void enableServer()
    {
      try
      {
        if (EnConfigurationSettings.GlobalSettings.ServerMode == ServerMode.IIS)
        {
          Cursor.Current = Cursors.WaitCursor;
          using (IIsServerManager iisServerManager = new IIsServerManager())
            iisServerManager.StartServer();
        }
        else
        {
          if (EnConfigurationSettings.GlobalSettings.ServerMode != ServerMode.Network)
            return;
          using (NetworkServerManager networkServerManager = new NetworkServerManager())
            networkServerManager.StartService();
        }
      }
      catch (Exception ex)
      {
        Cursor.Current = Cursors.Default;
        this.Log(TraceLevel.Warning, "Error enabling web application: " + ex.Message);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private VersionDownloadItem fetchUpdateFile()
    {
      VersionDownloadHistory downloadHistory = VersionControl.DownloadHistory;
      VersionDownloadItem versionDownloadItem = downloadHistory.FindItem(VersionControl.CurrentVersion.Version, this.newVersionInfo.TargetVersion, VersionMatchType.Exact);
      if (versionDownloadItem != null && versionDownloadItem.UpdateFileExists())
      {
        this.Log(TraceLevel.Info, "Existing update file found on server in file " + versionDownloadItem.FullUpdateFilename);
        switch (Utils.Dialog((IWin32Window) this, "It appears that the update for this version has already been successfully downloaded. Would you like to use this file to perform the upgrade?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
        {
          case DialogResult.Cancel:
            this.Log(TraceLevel.Info, "Update process cancelled by user");
            return (VersionDownloadItem) null;
          case DialogResult.Yes:
            this.Log(TraceLevel.Info, "Exiting version update file will be used for update.");
            return versionDownloadItem;
          default:
            this.Log(TraceLevel.Info, "Exiting version update file will not be used for update.");
            break;
        }
      }
      downloadHistory.DeleteVersion(this.newVersionInfo.SourceVersion, this.newVersionInfo.TargetVersion);
      if (!this.downloadUpdateFile())
        return (VersionDownloadItem) null;
      string updateFilename = this.newVersionInfo.UpdateFilename;
      this.newVersionInfo.UpdateFilename = "SH2SCInstaller.exe";
      downloadHistory.AddItem(this.newVersionInfo);
      this.newVersionInfo.UpdateFilename = updateFilename;
      return this.newVersionInfo;
    }

    private bool ensureServerStopped()
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        if (EnConfigurationSettings.GlobalSettings.ServerMode == ServerMode.IIS)
          return this.ensureIISServerStopped();
        return EnConfigurationSettings.GlobalSettings.ServerMode != ServerMode.Network || this.ensureServiceStopped();
      }
      catch (Exception ex)
      {
        return Utils.Dialog((IWin32Window) this, "An error occurred while attempting to stop the Encompass Server: " + ex.Message + "\r\n\r\nContinue with the update?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private bool ensureIISServerStopped()
    {
      if (EllieMae.EMLite.Common.IIs.IIs.GetVersion() == IIsVersion.IIs5)
      {
        if (Utils.Dialog((IWin32Window) this, "In order to update Encompass on this computer, the Internet Information Server (IIS) services must be stopped. Stopping these services will prevent access to any web and/or FTP sites hosted by this server until the update is completed." + Environment.NewLine + Environment.NewLine + "Would you like to stop the IIS services now and proceed with the update?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
          return false;
      }
      else if (Utils.Dialog((IWin32Window) this, "In order to update Encompass on this computer, the Encompass Server must be stopped. Stopping this service will cause all users to be logged out immediately and prevent further logins until the update is completed." + Environment.NewLine + Environment.NewLine + "Would you like to stop the Encompass Server now and proceed with the update?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        return false;
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        try
        {
          Session.End();
        }
        catch
        {
        }
        using (IIsServerManager iisServerManager = new IIsServerManager())
          iisServerManager.StopServer();
        return true;
      }
      catch (Exception ex)
      {
        Cursor.Current = Cursors.Default;
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private bool ensureServiceStopped()
    {
      using (NetworkServerManager networkServerManager = new NetworkServerManager())
      {
        if (!networkServerManager.IsServiceRunning())
          return true;
        if (Utils.Dialog((IWin32Window) this, "In order to update Encompass on this computer, the Encompass Server must be stopped. Stopping this service will cause all users to be logged out immediately and prevent further logins until the update is completed." + Environment.NewLine + Environment.NewLine + "Would you like to stop the Encompass Server now and proceed with the update?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
          return false;
        try
        {
          Session.End();
        }
        catch
        {
        }
        if (!networkServerManager.StopService())
          return false;
        this.Log(TraceLevel.Info, "Server service shut down successfully");
        return true;
      }
    }

    private bool ensureClientStopped()
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        foreach (Process process in Process.GetProcessesByName("Encompass"))
        {
          while (!process.HasExited)
          {
            if (Utils.Dialog((IWin32Window) this, "Please close the Encompass application in order to proceed with the update. Do not restart the application until the update has been completed.", MessageBoxButtons.RetryCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
              return false;
          }
        }
        return true;
      }
      catch
      {
        return true;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private bool downloadUpdateFile()
    {
      Uri sourceUri = new Uri(this.newVersionInfo.UpdateURL);
      DownloadManager downloadManager = new DownloadManager();
      DownloadResult downloadResult = downloadManager.BeginDownload(sourceUri, this.newVersionInfo.FullUpdateFilename);
      int num = (int) downloadManager.ShowDialog((IWin32Window) this);
      if (downloadResult.DownloadStatus != DownloadStatus.Complete)
        return false;
      this.newVersionInfo.UpdateDownloadDate(DateTime.Now);
      return true;
    }

    private VersionDownloadItem parseUpdateServiceResponse(object[] info)
    {
      if (info == null)
        return (VersionDownloadItem) null;
      return new VersionDownloadItem(VersionControl.CurrentVersion.Version, JedVersion.Parse((string) info[0]), (string) info[2], (string) info[1], (AffectedSystems) info[3])
      {
        DownloadFilePath = SystemSettings.UpdatesDir
      };
    }

    private bool startServerVersionUpdate(string updateFilename, AffectedSystems affectedSystems)
    {
      try
      {
        SystemUtil.ExecSystemCmd(Path.Combine(SystemSettings.LocalAppDir, "VersionControl.exe"), "update " + affectedSystems.ToString().ToLower() + " \"" + updateFilename + "\"");
        this.Log(TraceLevel.Info, "Lauched VersionController application to begin update using file \"" + updateFilename + "\"");
        return true;
      }
      catch (Exception ex)
      {
        this.Log(TraceLevel.Error, "Error lauching VersionController application -- update failed.");
        ErrorDialog.Display(ex);
        return false;
      }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      this.promptToUpdateShuSettingsIfDirty();
      this.Close();
    }

    private void Log(TraceLevel l, string msg)
    {
      Tracing.Log(this.sw, l, nameof (VersionManager), msg);
    }

    private LicenseInfo retrieveLicenseInfo()
    {
      try
      {
        return Session.ConfigurationManager.GetServerLicense();
      }
      catch (Exception ex)
      {
        this.Log(TraceLevel.Info, "Error retrieving server license information over trusted connection: " + (object) ex);
        this.lblMajorUpgrade.Text = "Encompass upgrade information cannot be determined at this time.";
        return (LicenseInfo) null;
      }
    }

    private void gvVersions_SelectedIndexChanged(object sender, EventArgs e)
    {
      ClientAppVersion clientAppVersion = (ClientAppVersion) null;
      if (this.gvVersions.SelectedItems.Count > 0)
        clientAppVersion = this.gvVersions.SelectedItems[0].Tag as ClientAppVersion;
      if (clientAppVersion == null)
      {
        this.btnTest.Enabled = false;
        this.btnApprove.Enabled = false;
      }
      else if (clientAppVersion.CompareTo((object) this.productionGroup.AuthorizedVersion) <= 0)
      {
        this.btnTest.Enabled = false;
        this.btnApprove.Enabled = false;
      }
      else if (clientAppVersion.CompareTo((object) this.testGroup.AuthorizedVersion) <= 0)
      {
        this.btnTest.Enabled = false;
        this.btnApprove.Enabled = true;
      }
      else
      {
        this.btnTest.Enabled = true;
        this.btnApprove.Enabled = true;
      }
    }

    private void btnTest_Click(object sender, EventArgs e)
    {
      ClientAppVersion tag = this.gvVersions.SelectedItems[0].Tag as ClientAppVersion;
      using (VersionTestUserDialog versionTestUserDialog = new VersionTestUserDialog(this.testGroup, false))
      {
        if (versionTestUserDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.testGroup.AuthorizedVersion = tag;
        Session.ServerManager.UpdateVersionManagementGroup(this.testGroup);
        this.refreshVersionStatuses();
      }
    }

    private void btnApprove_Click(object sender, EventArgs e)
    {
      ClientAppVersion tag = this.gvVersions.SelectedItems[0].Tag as ClientAppVersion;
      if (Utils.Dialog((IWin32Window) this, "By approving this hot update, it will be applied to your users' computers when they restart Encompass. This operation cannot be undone." + Environment.NewLine + Environment.NewLine + "Are you sure you want to approve the update '" + tag.DisplayVersionString + "' and all prior updates?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      this.productionGroup.AuthorizedVersion = tag;
      Session.ServerManager.UpdateVersionManagementGroup(this.productionGroup);
      this.refreshVersionStatuses();
    }

    private void btnAddTestUser_Click(object sender, EventArgs e)
    {
      using (VersionTestUserDialog versionTestUserDialog = new VersionTestUserDialog(this.testGroup, true))
      {
        int num = (int) versionTestUserDialog.ShowDialog((IWin32Window) this);
      }
    }

    private void lnkReleaseNotes_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      this.openReleaseNotes();
    }

    private void lnkMajorReleaseNotes_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      this.openReleaseNotes();
    }

    private void openReleaseNotes()
    {
      Process.Start("https://help.icemortgagetechnology.com/documentation/encompass/Content/encompass/release_notes/release-notes.htm");
    }

    private void lnkHelp_Help(object sender, EventArgs e)
    {
      JedHelp.ShowHelp(nameof (VersionManager));
    }

    private void VersionManager_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.lnkHelp_Help((object) null, (EventArgs) null);
    }

    private void displaySMUMessage(string text, int waitTime)
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) new VersionManager.displaySUMMessageCallback(this.displaySMUMessage), (object) text, (object) waitTime);
      }
      else
      {
        int num = (int) new ServerMajorUpdateForm(text, waitTime).ShowDialog();
      }
    }

    private void handleServerEvent(IConnection conn, ServerEvent e)
    {
      if (!(e is MessageEvent))
        return;
      EllieMae.EMLite.ClientServer.Message message = ((MessageEvent) e).Message;
      if (message == null || !(message is SmuMessage))
        return;
      SmuMessage smuMessage = (SmuMessage) message;
      if (smuMessage.WaitTime <= 0)
        return;
      this.displaySMUMessage(smuMessage.Text, smuMessage.WaitTime);
    }

    public void installUpdates(object obj)
    {
      if (!(obj is Dictionary<string, object>))
        return;
      this.installUpdates((Dictionary<string, object>) obj);
    }

    private Dictionary<string, object> prepareUpdatePackage(bool isSHU)
    {
      Dictionary<string, object> dictionary = new Dictionary<string, object>();
      JedServerVersion jedServerVersion = !isSHU ? this.gvServerVersions.SelectedItems[0].Tag as JedServerVersion : this.gvServerHotVersions.SelectedItems[0].Tag as JedServerVersion;
      dictionary.Add(nameof (isSHU), (object) isSHU);
      dictionary.Add("UpdateNumber", (object) jedServerVersion.UpdateNumber);
      dictionary.Add("UseHTTPS", (object) this.chkBoxUseHTTPSU.Checked);
      return dictionary;
    }

    public void installUpdates(bool isSHU) => this.installUpdates(this.prepareUpdatePackage(isSHU));

    private void installUpdates(Dictionary<string, object> parameters)
    {
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        using (EllieMae.EMLite.Client.Connection connection = new EllieMae.EMLite.Client.Connection())
        {
          if (EnConfigurationSettings.GlobalSettings.ServerMode == ServerMode.IIS)
          {
            string iisVirtualRootName = EnConfigurationSettings.GlobalSettings.IIsVirtualRootName;
            string str1 = (bool) parameters["UseHTTPS"] ? "https://127.0.0.1/" : "http://127.0.0.1/";
            string str2 = (bool) parameters["UseHTTPS"] ? "http://127.0.0.1/" : "https://127.0.0.1/";
            try
            {
              connection.Open(str1 + iisVirtualRootName, "admin", Session.Password, "AdminTools", false, (string) null);
            }
            catch
            {
              connection.Open(str2 + iisVirtualRootName, "admin", Session.Password, "AdminTools", false, (string) null);
            }
          }
          else
            connection.Open("localhost", "admin", Session.Password, "AdminTools", false, (string) null);
          connection.ServerEvent += new ServerEventHandler(this.handleServerEvent);
          string userid = "(rmi)";
          string password = EnConfigurationSettings.GlobalSettings.RMIPassword;
          if ((password ?? "").Trim() == "")
          {
            userid = "(trusted)";
            password = EnConfigurationSettings.GlobalSettings.DatabasePassword;
          }
          IServerManager serverManager = (IServerManager) connection.Session.GetObject("ServerManager");
          string text;
          if ((bool) parameters["isSHU"])
          {
            text = serverManager.DownloadAndApplyServerHotUpdates(userid, password, Utils.ParseInt((object) string.Concat(parameters["UpdateNumber"])), true);
            Tracing.Log(this.sw, TraceLevel.Info, "SHU", "Download and apply server hot updates");
          }
          else
          {
            text = serverManager.DownloadAndApplyServerMajorUpdates(userid, password, Utils.ParseInt((object) string.Concat(parameters["UpdateNumber"])), true);
            Tracing.Log(this.sw, TraceLevel.Info, "SMU", "Download and apply server major updates");
          }
          if (text == null)
            return;
          int num = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "AdminTools: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        VersionInformation.WaitForVersionControlToExit();
        this.refreshServerVersionStatuses(true);
        Cursor.Current = Cursors.Default;
      }
    }

    private void chkBoxUseHTTPS_CheckedChanged(object sender, EventArgs e)
    {
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass", true))
      {
        if (this.tabControlEx1.SelectedPage == this.tabPageSHU)
        {
          registryKey.SetValue("SuUseHttps", this.chkBoxUseHTTPS.Checked ? (object) "1" : (object) "0");
        }
        else
        {
          if (this.tabControlEx1.SelectedPage != this.tabPageSMU)
            return;
          registryKey.SetValue("SuUseHttps", this.chkBoxUseHTTPSU.Checked ? (object) "1" : (object) "0");
        }
      }
    }

    private void promptToUpdateShuSettingsIfDirty()
    {
      if (!this.shuSettingsDirty || Utils.Dialog((IWin32Window) this, "Update server hot update settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.updateShuSettings();
    }

    private void tabControlEx1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void btnUpdateShuSettings_Click(object sender, EventArgs e) => this.updateShuSettings();

    private void btnReset_Click(object sender, EventArgs e) => this.resetShuSettings();

    private void cmbBoxShuSetting_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.cmbBoxShuSettingDay.Enabled = this.cmbBoxShuSettingTime.Enabled = this.cmbBoxShuSetting.SelectedIndex != 2;
      this.shuSettingsDirty = true;
    }

    private void cmbBoxShuSettingDay_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.shuSettingsDirty = true;
    }

    private void cmbBoxShuSettingTime_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.shuSettingsDirty = true;
    }

    private void textBoxShuSettingNotificationTime_TextChanged(object sender, EventArgs e)
    {
      this.shuSettingsDirty = true;
    }

    private void textBoxShuSettingNotificationTime_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
        return;
      e.Handled = true;
    }

    private void btnInstallMajorUpdates_Click(object sender, EventArgs e)
    {
      if (new VersionManagerNotification().ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      new Thread(new ParameterizedThreadStart(this.installUpdates))
      {
        IsBackground = true
      }.Start((object) this.prepareUpdatePackage(false));
      this.Cursor = Cursors.WaitCursor;
      this.updateInProgress = true;
      this.btnClose.Enabled = this.btnInstallMajorUpdates.Enabled = false;
    }

    private void btnInstallHotUpdates_Click(object sender, EventArgs e)
    {
      this.installUpdates(true);
    }

    private void gvServerHotVersions_SelectedIndexChanged(object sender, EventArgs e)
    {
      JedServerVersion jedServerVersion = (JedServerVersion) null;
      if (this.gvServerHotVersions.SelectedItems.Count > 0)
        jedServerVersion = this.gvServerHotVersions.SelectedItems[0].Tag as JedServerVersion;
      if (jedServerVersion == (JedServerVersion) null)
        this.btnInstallHotUpdates.Enabled = false;
      else if (jedServerVersion.UpdateNumber <= VersionInformation.CurrentVersion.GetMaxInstalledServerHotUpdateNumber())
        this.btnInstallHotUpdates.Enabled = false;
      else
        this.btnInstallHotUpdates.Enabled = true;
    }

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      this.openReleaseNotes();
    }

    private void linkSRelease_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      this.openReleaseNotes();
    }

    private void gvServerVersions_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.updateInProgress)
        return;
      JedServerVersion jedServerVersion = (JedServerVersion) null;
      if (this.gvServerVersions.SelectedItems.Count > 0)
        jedServerVersion = this.gvServerVersions.SelectedItems[0].Tag as JedServerVersion;
      if (jedServerVersion == (JedServerVersion) null)
        this.btnInstallMajorUpdates.Enabled = false;
      else if (jedServerVersion.UpdateNumber <= VersionInformation.CurrentVersion.GetMaxInstalledServerMajorUpdateNumber())
        this.btnInstallMajorUpdates.Enabled = false;
      else
        this.btnInstallMajorUpdates.Enabled = true;
    }

    private enum AppVersionStatus
    {
      Unknown,
      New,
      Testing,
      Approved,
    }

    private enum SuVersionStatus
    {
      Unknown,
      New,
      Downloaded,
      Installed,
    }

    private delegate void displaySUMMessageCallback(string text, int waitTime);
  }
}
