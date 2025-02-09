// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.PersonaSettingsRptCreate
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

#nullable disable
namespace EllieMae.EMLite
{
  public class PersonaSettingsRptCreate : UserControl
  {
    private Panel reportInfoPnl;
    private Panel reportGenPnl;
    private GroupContainer gcPersona;
    private ListViewEx listViewPersonas;
    private ColumnHeader columnCheckBox;
    private ColumnHeader columnPersonaName;
    private ColumnHeader columnInternalExternal;
    private System.Windows.Forms.CheckBox internalPersonaChkBx;
    private IContainer components;
    private Persona[] personas;
    private GroupContainer groupContainer1;
    private System.Windows.Forms.CheckBox TPOAdminAccess;
    private System.Windows.Forms.CheckBox externalSettingsAccess;
    private System.Windows.Forms.CheckBox settingsAccess;
    private System.Windows.Forms.CheckBox tcdrAccess;
    private System.Windows.Forms.CheckBox eFolderAccess;
    private System.Windows.Forms.CheckBox formsAccess;
    private System.Windows.Forms.CheckBox loansAccess;
    private System.Windows.Forms.CheckBox toolsAccess;
    private System.Windows.Forms.CheckBox homePageAccess;
    private System.Windows.Forms.CheckBox pipelineAccess;
    private Label label4;
    private System.Windows.Forms.TextBox reportName;
    private Label label1;
    private Sessions.Session session;
    private System.Windows.Forms.CheckBox selectAll;
    private Label label2;
    private System.Windows.Forms.CheckBox consumerConnectAccess;
    private System.Windows.Forms.CheckBox loConnectAccess;
    private System.Windows.Forms.CheckBox eVaultAccess;
    private System.Windows.Forms.CheckBox iceMortgageTechAiq;
    private System.Windows.Forms.CheckBox chkdevconnect;
    private bool isTPOMVP;

    public string ReportName => this.reportName.Text;

    public bool HomePageAccess => this.homePageAccess.Checked;

    public bool PipelineAccess => this.pipelineAccess.Checked;

    public bool ToolsAccess => this.toolsAccess.Checked;

    public bool LoansAccess => this.loansAccess.Checked;

    public bool FormsAccess => this.formsAccess.Checked;

    public bool EFolderAccess => this.eFolderAccess.Checked;

    public bool TCDRAccess => this.tcdrAccess.Checked;

    public bool SettingsAccess => this.settingsAccess.Checked;

    public bool ExternalSettingsAccess => this.externalSettingsAccess.Checked;

    public bool TPOADMINAccess
    {
      get => this.TPOAdminAccess.Checked;
      set => this.TPOAdminAccess.Checked = value;
    }

    public bool iSTPOMVP
    {
      get => this.isTPOMVP;
      set => this.isTPOMVP = value;
    }

    public bool ConsumerConnectAccess
    {
      get => this.consumerConnectAccess.Checked;
      set => this.consumerConnectAccess.Checked = value;
    }

    public bool LOConnectAccess
    {
      get => this.loConnectAccess.Checked;
      set => this.loConnectAccess.Checked = value;
    }

    public bool EVaultAccess
    {
      get => this.eVaultAccess.Checked;
      set => this.eVaultAccess.Checked = value;
    }

    public bool InternalPersonaCheckBox => this.internalPersonaChkBx.Checked;

    public bool IceMortgageTechAiq
    {
      get => this.iceMortgageTechAiq.Checked;
      set => this.iceMortgageTechAiq.Checked = value;
    }

    public bool DeveloperConnectAccess
    {
      get => this.chkdevconnect.Checked;
      set => this.chkdevconnect.Checked = value;
    }

    public List<string> SelectedPersona => this.getSelectedPersona();

    public PersonaSettingsRptCreate(
      Sessions.Session session,
      string userid,
      SettingsRptJobInfo jobinfo)
    {
      this.InitializeComponent();
      this.session = session;
      this.isTPOMVP = session.ConfigurationManager.CheckIfAnyTPOSiteExists();
      if (this.isTPOMVP)
      {
        this.addPersonaTypeColumn();
        this.internalPersonaChkBx.Visible = true;
        this.TPOAdminAccess.Visible = true;
      }
      else
        this.columnPersonaName.Width = 400;
      this.populatePersonas();
      if (jobinfo == null)
        return;
      this.internalPersonaChkBx.Checked = jobinfo.reportParameters.ContainsKey("IsInternalPersonaCheckBoxChecked") && Convert.ToBoolean(jobinfo.reportParameters["IsInternalPersonaCheckBoxChecked"]);
      foreach (ListViewItem listViewItem in this.listViewPersonas.Items)
      {
        if (jobinfo.reportFilters.Contains(listViewItem.SubItems[1].Text))
          listViewItem.Checked = true;
      }
      this.reportName.Text = "CopyOf_" + jobinfo.ReportName;
      this.homePageAccess.Checked = jobinfo.reportParameters.ContainsKey(nameof (HomePageAccess)) && Convert.ToBoolean(jobinfo.reportParameters[nameof (HomePageAccess)]);
      this.toolsAccess.Checked = jobinfo.reportParameters.ContainsKey(nameof (ToolsAccess)) && Convert.ToBoolean(jobinfo.reportParameters[nameof (ToolsAccess)]);
      this.pipelineAccess.Checked = jobinfo.reportParameters.ContainsKey(nameof (PipelineAccess)) && Convert.ToBoolean(jobinfo.reportParameters[nameof (PipelineAccess)]);
      this.loansAccess.Checked = jobinfo.reportParameters.ContainsKey(nameof (LoansAccess)) && Convert.ToBoolean(jobinfo.reportParameters[nameof (LoansAccess)]);
      this.formsAccess.Checked = jobinfo.reportParameters.ContainsKey(nameof (FormsAccess)) && Convert.ToBoolean(jobinfo.reportParameters[nameof (FormsAccess)]);
      this.eFolderAccess.Checked = jobinfo.reportParameters.ContainsKey(nameof (EFolderAccess)) && Convert.ToBoolean(jobinfo.reportParameters[nameof (EFolderAccess)]);
      this.tcdrAccess.Checked = jobinfo.reportParameters.ContainsKey(nameof (TCDRAccess)) && Convert.ToBoolean(jobinfo.reportParameters[nameof (TCDRAccess)]);
      this.settingsAccess.Checked = jobinfo.reportParameters.ContainsKey(nameof (SettingsAccess)) && Convert.ToBoolean(jobinfo.reportParameters[nameof (SettingsAccess)]);
      this.externalSettingsAccess.Checked = jobinfo.reportParameters.ContainsKey(nameof (ExternalSettingsAccess)) && Convert.ToBoolean(jobinfo.reportParameters[nameof (ExternalSettingsAccess)]);
      this.TPOAdminAccess.Checked = jobinfo.reportParameters.ContainsKey(nameof (TPOADMINAccess)) && Convert.ToBoolean(jobinfo.reportParameters[nameof (TPOADMINAccess)]);
      this.consumerConnectAccess.Checked = jobinfo.reportParameters.ContainsKey(nameof (ConsumerConnectAccess)) && Convert.ToBoolean(jobinfo.reportParameters[nameof (ConsumerConnectAccess)]);
      this.loConnectAccess.Checked = jobinfo.reportParameters.ContainsKey(nameof (LOConnectAccess)) && Convert.ToBoolean(jobinfo.reportParameters[nameof (LOConnectAccess)]);
      this.eVaultAccess.Checked = jobinfo.reportParameters.ContainsKey(nameof (EVaultAccess)) && Convert.ToBoolean(jobinfo.reportParameters[nameof (EVaultAccess)]);
      this.iceMortgageTechAiq.Checked = jobinfo.reportParameters.ContainsKey(nameof (IceMortgageTechAiq)) && Convert.ToBoolean(jobinfo.reportParameters[nameof (IceMortgageTechAiq)]);
      this.chkdevconnect.Checked = jobinfo.reportParameters.ContainsKey("DevConnectAccess") && Convert.ToBoolean(jobinfo.reportParameters["DevConnectAccess"]);
    }

    private void addPersonaTypeColumn()
    {
      this.listViewPersonas.HeaderStyle = ColumnHeaderStyle.Clickable;
      this.listViewPersonas.Columns.Add(this.columnInternalExternal);
      this.columnInternalExternal.Text = "Internal/External";
      this.columnInternalExternal.Width = 240;
    }

    private List<string> getSelectedPersona()
    {
      List<string> selectedPersona = new List<string>();
      foreach (ListViewItem listViewItem in this.listViewPersonas.Items)
      {
        if (listViewItem.Checked)
          selectedPersona.Add(listViewItem.SubItems[1].Text);
      }
      return selectedPersona;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.reportInfoPnl = new Panel();
      this.gcPersona = new GroupContainer();
      this.listViewPersonas = new ListViewEx();
      this.columnCheckBox = new ColumnHeader();
      this.columnPersonaName = new ColumnHeader();
      this.internalPersonaChkBx = new System.Windows.Forms.CheckBox();
      this.columnInternalExternal = new ColumnHeader();
      this.reportGenPnl = new Panel();
      this.groupContainer1 = new GroupContainer();
      this.chkdevconnect = new System.Windows.Forms.CheckBox();
      this.iceMortgageTechAiq = new System.Windows.Forms.CheckBox();
      this.loConnectAccess = new System.Windows.Forms.CheckBox();
      this.eVaultAccess = new System.Windows.Forms.CheckBox();
      this.consumerConnectAccess = new System.Windows.Forms.CheckBox();
      this.label2 = new Label();
      this.selectAll = new System.Windows.Forms.CheckBox();
      this.TPOAdminAccess = new System.Windows.Forms.CheckBox();
      this.externalSettingsAccess = new System.Windows.Forms.CheckBox();
      this.settingsAccess = new System.Windows.Forms.CheckBox();
      this.tcdrAccess = new System.Windows.Forms.CheckBox();
      this.eFolderAccess = new System.Windows.Forms.CheckBox();
      this.formsAccess = new System.Windows.Forms.CheckBox();
      this.loansAccess = new System.Windows.Forms.CheckBox();
      this.toolsAccess = new System.Windows.Forms.CheckBox();
      this.homePageAccess = new System.Windows.Forms.CheckBox();
      this.pipelineAccess = new System.Windows.Forms.CheckBox();
      this.label4 = new Label();
      this.reportName = new System.Windows.Forms.TextBox();
      this.label1 = new Label();
      this.reportInfoPnl.SuspendLayout();
      this.gcPersona.SuspendLayout();
      this.reportGenPnl.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.reportInfoPnl.Controls.Add((Control) this.gcPersona);
      this.reportInfoPnl.Location = new Point(2, 2);
      this.reportInfoPnl.Margin = new Padding(2, 2, 2, 2);
      this.reportInfoPnl.Name = "reportInfoPnl";
      this.reportInfoPnl.Size = new Size(392, 456);
      this.reportInfoPnl.TabIndex = 11;
      this.gcPersona.AutoScroll = true;
      this.gcPersona.Controls.Add((Control) this.listViewPersonas);
      this.gcPersona.Controls.Add((Control) this.internalPersonaChkBx);
      this.gcPersona.Dock = DockStyle.Fill;
      this.gcPersona.HeaderForeColor = SystemColors.ControlText;
      this.gcPersona.Location = new Point(0, 0);
      this.gcPersona.Margin = new Padding(2, 2, 2, 2);
      this.gcPersona.Name = "gcPersona";
      this.gcPersona.Size = new Size(392, 456);
      this.gcPersona.TabIndex = 9;
      this.gcPersona.Text = "1. Choose Persona(s)";
      this.listViewPersonas.AllowColumnReorder = true;
      this.listViewPersonas.BorderStyle = BorderStyle.None;
      this.listViewPersonas.CheckBoxes = true;
      this.listViewPersonas.Columns.AddRange(new ColumnHeader[2]
      {
        this.columnCheckBox,
        this.columnPersonaName
      });
      this.listViewPersonas.Dock = DockStyle.Fill;
      this.listViewPersonas.DoubleClickActivation = false;
      this.listViewPersonas.FullRowSelect = true;
      this.listViewPersonas.GridLines = true;
      this.listViewPersonas.HideSelection = false;
      this.listViewPersonas.Location = new Point(1, 26);
      this.listViewPersonas.Margin = new Padding(2, 2, 2, 2);
      this.listViewPersonas.Name = "listViewPersonas";
      this.listViewPersonas.OwnerDraw = true;
      this.listViewPersonas.Size = new Size(390, 429);
      this.listViewPersonas.TabIndex = 5;
      this.listViewPersonas.UseCompatibleStateImageBehavior = false;
      this.listViewPersonas.View = View.Details;
      this.listViewPersonas.ColumnClick += new ColumnClickEventHandler(this.listView1_ColumnClick);
      this.listViewPersonas.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(this.listView1_DrawColumnHeader);
      this.listViewPersonas.DrawItem += new DrawListViewItemEventHandler(this.listView1_DrawItem);
      this.listViewPersonas.DrawSubItem += new DrawListViewSubItemEventHandler(this.listView1_DrawSubItem);
      this.listViewPersonas.SelectedIndexChanged += new EventHandler(this.listViewPersonas_SelectedIndexChanged);
      this.columnCheckBox.Text = "";
      this.columnPersonaName.Text = "Persona Name";
      this.columnPersonaName.Width = 216;
      this.internalPersonaChkBx.AutoSize = true;
      this.internalPersonaChkBx.BackColor = Color.Transparent;
      this.internalPersonaChkBx.Checked = true;
      this.internalPersonaChkBx.CheckState = CheckState.Checked;
      this.internalPersonaChkBx.Location = new Point(137, 4);
      this.internalPersonaChkBx.Margin = new Padding(2, 2, 2, 2);
      this.internalPersonaChkBx.Name = "internalPersonaChkBx";
      this.internalPersonaChkBx.Size = new Size(198, 17);
      this.internalPersonaChkBx.TabIndex = 0;
      this.internalPersonaChkBx.Text = "Show Personas with Internal Access";
      this.internalPersonaChkBx.UseVisualStyleBackColor = false;
      this.internalPersonaChkBx.Visible = false;
      this.internalPersonaChkBx.CheckedChanged += new EventHandler(this.internalPersonaChkBx_CheckedChanged);
      this.columnInternalExternal.Text = "Internal/External";
      this.columnInternalExternal.Width = 240;
      this.reportGenPnl.Controls.Add((Control) this.groupContainer1);
      this.reportGenPnl.Location = new Point(399, 2);
      this.reportGenPnl.Margin = new Padding(2, 2, 2, 2);
      this.reportGenPnl.Name = "reportGenPnl";
      this.reportGenPnl.Size = new Size(306, 455);
      this.reportGenPnl.TabIndex = 12;
      this.groupContainer1.BackColor = Color.White;
      this.groupContainer1.Controls.Add((Control) this.chkdevconnect);
      this.groupContainer1.Controls.Add((Control) this.iceMortgageTechAiq);
      this.groupContainer1.Controls.Add((Control) this.loConnectAccess);
      this.groupContainer1.Controls.Add((Control) this.eVaultAccess);
      this.groupContainer1.Controls.Add((Control) this.consumerConnectAccess);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.selectAll);
      this.groupContainer1.Controls.Add((Control) this.TPOAdminAccess);
      this.groupContainer1.Controls.Add((Control) this.externalSettingsAccess);
      this.groupContainer1.Controls.Add((Control) this.settingsAccess);
      this.groupContainer1.Controls.Add((Control) this.tcdrAccess);
      this.groupContainer1.Controls.Add((Control) this.eFolderAccess);
      this.groupContainer1.Controls.Add((Control) this.formsAccess);
      this.groupContainer1.Controls.Add((Control) this.loansAccess);
      this.groupContainer1.Controls.Add((Control) this.toolsAccess);
      this.groupContainer1.Controls.Add((Control) this.homePageAccess);
      this.groupContainer1.Controls.Add((Control) this.pipelineAccess);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.reportName);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Margin = new Padding(2, 2, 2, 2);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(306, 455);
      this.groupContainer1.TabIndex = 10;
      this.groupContainer1.Text = "2. Select Report Options";
      this.groupContainer1.Paint += new PaintEventHandler(this.groupContainer1_Paint);
      this.chkdevconnect.AutoSize = true;
      this.chkdevconnect.Location = new Point(16, 415);
      this.chkdevconnect.Margin = new Padding(2, 2, 2, 2);
      this.chkdevconnect.Name = "chkdevconnect";
      this.chkdevconnect.Size = new Size(155, 17);
      this.chkdevconnect.TabIndex = 28;
      this.chkdevconnect.Text = "Developer Connect access";
      this.chkdevconnect.UseVisualStyleBackColor = true;
      this.iceMortgageTechAiq.AutoSize = true;
      this.iceMortgageTechAiq.Location = new Point(16, 435);
      this.iceMortgageTechAiq.Margin = new Padding(2, 2, 2, 2);
      this.iceMortgageTechAiq.Name = "iceMortgageTechAiq";
      this.iceMortgageTechAiq.Size = new Size(283, 17);
      this.iceMortgageTechAiq.TabIndex = 27;
      this.iceMortgageTechAiq.Text = "Data && Document Automation and Mortgage Analyzers";
      this.iceMortgageTechAiq.UseVisualStyleBackColor = true;
      this.loConnectAccess.AutoSize = true;
      this.loConnectAccess.Location = new Point(16, 397);
      this.loConnectAccess.Margin = new Padding(2, 2, 2, 2);
      this.loConnectAccess.Name = "loConnectAccess";
      this.loConnectAccess.Size = new Size(124, 17);
      this.loConnectAccess.TabIndex = 25;
      this.loConnectAccess.Text = "Web Version access";
      this.loConnectAccess.UseVisualStyleBackColor = true;
      this.eVaultAccess.AutoSize = true;
      this.eVaultAccess.Location = new Point(16, 377);
      this.eVaultAccess.Margin = new Padding(2, 2, 2, 2);
      this.eVaultAccess.Name = "eVaultAccess";
      this.eVaultAccess.Size = new Size(93, 17);
      this.eVaultAccess.TabIndex = 26;
      this.eVaultAccess.Text = "eVault access";
      this.eVaultAccess.UseVisualStyleBackColor = true;
      this.consumerConnectAccess.AutoSize = true;
      this.consumerConnectAccess.Location = new Point(16, 356);
      this.consumerConnectAccess.Margin = new Padding(2, 2, 2, 2);
      this.consumerConnectAccess.Name = "consumerConnectAccess";
      this.consumerConnectAccess.Size = new Size(153, 17);
      this.consumerConnectAccess.TabIndex = 24;
      this.consumerConnectAccess.Text = "Consumer Connect access";
      this.consumerConnectAccess.UseVisualStyleBackColor = true;
      this.label2.BorderStyle = BorderStyle.Fixed3D;
      this.label2.ForeColor = SystemColors.ControlDark;
      this.label2.Location = new Point(16, 128);
      this.label2.Margin = new Padding(2, 0, 2, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(248, 2);
      this.label2.TabIndex = 23;
      this.selectAll.AutoSize = true;
      this.selectAll.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.selectAll.Location = new Point(16, 103);
      this.selectAll.Margin = new Padding(2, 2, 2, 2);
      this.selectAll.Name = "selectAll";
      this.selectAll.Size = new Size(70, 17);
      this.selectAll.TabIndex = 22;
      this.selectAll.Text = "Select All";
      this.selectAll.UseVisualStyleBackColor = true;
      this.selectAll.CheckedChanged += new EventHandler(this.selectAll_CheckedChanged);
      this.TPOAdminAccess.AutoSize = true;
      this.TPOAdminAccess.Location = new Point(16, 335);
      this.TPOAdminAccess.Margin = new Padding(2, 2, 2, 2);
      this.TPOAdminAccess.Name = "TPOAdminAccess";
      this.TPOAdminAccess.Size = new Size(128, 17);
      this.TPOAdminAccess.TabIndex = 21;
      this.TPOAdminAccess.Text = "TPO Connect access";
      this.TPOAdminAccess.UseVisualStyleBackColor = true;
      this.TPOAdminAccess.Visible = false;
      this.externalSettingsAccess.AutoSize = true;
      this.externalSettingsAccess.Location = new Point(16, 313);
      this.externalSettingsAccess.Margin = new Padding(2, 2, 2, 2);
      this.externalSettingsAccess.Name = "externalSettingsAccess";
      this.externalSettingsAccess.Size = new Size(142, 17);
      this.externalSettingsAccess.TabIndex = 20;
      this.externalSettingsAccess.Text = "External Settings access";
      this.externalSettingsAccess.UseVisualStyleBackColor = true;
      this.settingsAccess.AutoSize = true;
      this.settingsAccess.Location = new Point(16, 291);
      this.settingsAccess.Margin = new Padding(2, 2, 2, 2);
      this.settingsAccess.Name = "settingsAccess";
      this.settingsAccess.Size = new Size(101, 17);
      this.settingsAccess.TabIndex = 19;
      this.settingsAccess.Text = "Settings access";
      this.settingsAccess.UseVisualStyleBackColor = true;
      this.tcdrAccess.AutoSize = true;
      this.tcdrAccess.Location = new Point(16, 269);
      this.tcdrAccess.Margin = new Padding(2, 2, 2, 2);
      this.tcdrAccess.Name = "tcdrAccess";
      this.tcdrAccess.Size = new Size(242, 17);
      this.tcdrAccess.TabIndex = 18;
      this.tcdrAccess.Text = "Trades/Contacts/Dashboard/Reports access";
      this.tcdrAccess.UseVisualStyleBackColor = true;
      this.eFolderAccess.AutoSize = true;
      this.eFolderAccess.Location = new Point(16, 247);
      this.eFolderAccess.Margin = new Padding(2, 2, 2, 2);
      this.eFolderAccess.Name = "eFolderAccess";
      this.eFolderAccess.Size = new Size(98, 17);
      this.eFolderAccess.TabIndex = 17;
      this.eFolderAccess.Text = "eFolder access";
      this.eFolderAccess.UseVisualStyleBackColor = true;
      this.formsAccess.AutoSize = true;
      this.formsAccess.Location = new Point(16, 203);
      this.formsAccess.Margin = new Padding(2, 2, 2, 2);
      this.formsAccess.Name = "formsAccess";
      this.formsAccess.Size = new Size(91, 17);
      this.formsAccess.TabIndex = 13;
      this.formsAccess.Text = "Forms access";
      this.formsAccess.UseVisualStyleBackColor = true;
      this.loansAccess.AutoSize = true;
      this.loansAccess.Checked = true;
      this.loansAccess.CheckState = CheckState.Checked;
      this.loansAccess.Location = new Point(16, 181);
      this.loansAccess.Margin = new Padding(2, 2, 2, 2);
      this.loansAccess.Name = "loansAccess";
      this.loansAccess.Size = new Size(87, 17);
      this.loansAccess.TabIndex = 12;
      this.loansAccess.Text = "Loan access";
      this.loansAccess.UseVisualStyleBackColor = true;
      this.toolsAccess.AutoSize = true;
      this.toolsAccess.Location = new Point(16, 225);
      this.toolsAccess.Margin = new Padding(2, 2, 2, 2);
      this.toolsAccess.Name = "toolsAccess";
      this.toolsAccess.Size = new Size(89, 17);
      this.toolsAccess.TabIndex = 14;
      this.toolsAccess.Text = "Tools access";
      this.toolsAccess.UseVisualStyleBackColor = true;
      this.homePageAccess.AutoSize = true;
      this.homePageAccess.Location = new Point(16, 137);
      this.homePageAccess.Margin = new Padding(2, 2, 2, 2);
      this.homePageAccess.Name = "homePageAccess";
      this.homePageAccess.Size = new Size(162, 17);
      this.homePageAccess.TabIndex = 8;
      this.homePageAccess.Text = "Home Page Modules access";
      this.homePageAccess.UseVisualStyleBackColor = true;
      this.pipelineAccess.AutoSize = true;
      this.pipelineAccess.Checked = true;
      this.pipelineAccess.CheckState = CheckState.Checked;
      this.pipelineAccess.Location = new Point(16, 159);
      this.pipelineAccess.Margin = new Padding(2, 2, 2, 2);
      this.pipelineAccess.Name = "pipelineAccess";
      this.pipelineAccess.Size = new Size(100, 17);
      this.pipelineAccess.TabIndex = 9;
      this.pipelineAccess.Text = "Pipeline access";
      this.pipelineAccess.UseVisualStyleBackColor = true;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(14, 87);
      this.label4.Margin = new Padding(2, 0, 2, 0);
      this.label4.Name = "label4";
      this.label4.Size = new Size(129, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "Include the following:";
      this.reportName.Location = new Point(16, 50);
      this.reportName.Margin = new Padding(2, 2, 2, 2);
      this.reportName.Name = "reportName";
      this.reportName.Size = new Size(246, 20);
      this.reportName.TabIndex = 2;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(14, 33);
      this.label1.Margin = new Padding(2, 0, 2, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(81, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Report Name";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.reportGenPnl);
      this.Controls.Add((Control) this.reportInfoPnl);
      this.Margin = new Padding(2, 2, 2, 2);
      this.Name = nameof (PersonaSettingsRptCreate);
      this.Size = new Size(707, 460);
      this.reportInfoPnl.ResumeLayout(false);
      this.gcPersona.ResumeLayout(false);
      this.gcPersona.PerformLayout();
      this.reportGenPnl.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }

    private void populatePersonas()
    {
      this.personas = this.getPersonas();
      this.listViewPersonas.Items.Clear();
      foreach (Persona persona in this.personas)
      {
        string str = "";
        if (persona.IsExternal && persona.IsInternal)
          str = "Both Internal and External";
        else if (persona.IsInternal)
          str = "Internal";
        else if (persona.IsExternal)
          str = "External";
        if (this.isTPOMVP)
        {
          if (this.internalPersonaChkBx.Checked && str.Contains("Internal") || !this.internalPersonaChkBx.Checked)
            this.listViewPersonas.Items.Add(new ListViewItem(new string[3]
            {
              "",
              persona.Name,
              str
            }));
        }
        else
          this.listViewPersonas.Items.Add(new ListViewItem(new string[2]
          {
            "",
            persona.Name
          }));
      }
    }

    private Persona[] getPersonas()
    {
      Persona[] allPersonas = this.session.PersonaManager.GetAllPersonas();
      ArrayList arrayList = new ArrayList();
      foreach (Persona persona in allPersonas)
      {
        if (persona.Name != "Administrator" && persona.ID != 1 && persona.Name != "Super Administrator" && persona.ID != 0)
          arrayList.Add((object) persona);
      }
      return (Persona[]) arrayList.ToArray(typeof (Persona));
    }

    private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
    {
      if (e.ColumnIndex == 0)
      {
        e.DrawBackground();
        bool flag = false;
        try
        {
          flag = Convert.ToBoolean(e.Header.Tag);
        }
        catch (Exception ex)
        {
        }
        CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(e.Bounds.Left + 5, e.Bounds.Top + 2), flag ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal);
      }
      else
        e.DrawDefault = true;
    }

    private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      if (e.Column != 0)
        return;
      bool flag = false;
      try
      {
        flag = Convert.ToBoolean(this.listViewPersonas.Columns[e.Column].Tag);
      }
      catch (Exception ex)
      {
      }
      this.listViewPersonas.Columns[e.Column].Tag = (object) !flag;
      foreach (ListViewItem listViewItem in this.listViewPersonas.Items)
        listViewItem.Checked = !flag;
      this.listViewPersonas.Invalidate();
    }

    private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
    {
      e.DrawDefault = true;
    }

    private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
    {
      e.DrawDefault = true;
    }

    private void listViewPersonas_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void selectAll_CheckedChanged(object sender, EventArgs e)
    {
      if (this.selectAll.Checked)
      {
        this.homePageAccess.Checked = true;
        this.loansAccess.Checked = true;
        if (this.isTPOMVP)
          this.TPOAdminAccess.Checked = true;
        this.pipelineAccess.Checked = true;
        this.formsAccess.Checked = true;
        this.toolsAccess.Checked = true;
        this.eFolderAccess.Checked = true;
        this.tcdrAccess.Checked = true;
        this.settingsAccess.Checked = true;
        this.externalSettingsAccess.Checked = true;
        this.consumerConnectAccess.Checked = true;
        this.loConnectAccess.Checked = true;
        this.eVaultAccess.Checked = true;
        this.iceMortgageTechAiq.Checked = true;
        this.chkdevconnect.Checked = true;
      }
      else
      {
        this.homePageAccess.Checked = false;
        this.loansAccess.Checked = false;
        this.TPOAdminAccess.Checked = false;
        this.pipelineAccess.Checked = false;
        this.formsAccess.Checked = false;
        this.toolsAccess.Checked = false;
        this.eFolderAccess.Checked = false;
        this.tcdrAccess.Checked = false;
        this.settingsAccess.Checked = false;
        this.externalSettingsAccess.Checked = false;
        this.consumerConnectAccess.Checked = false;
        this.loConnectAccess.Checked = false;
        this.eVaultAccess.Checked = false;
        this.iceMortgageTechAiq.Checked = false;
        this.chkdevconnect.Checked = false;
      }
    }

    private void groupContainer1_Paint(object sender, PaintEventArgs e)
    {
    }

    private void internalPersonaChkBx_CheckedChanged(object sender, EventArgs e)
    {
      this.populatePersonas();
    }
  }
}
