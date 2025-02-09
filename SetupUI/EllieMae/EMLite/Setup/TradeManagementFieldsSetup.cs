// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TradeManagementFieldsSetup
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TradeManagementFieldsSetup : SettingsUserControl
  {
    private IContainer components;
    private GroupContainer gcSecurityType;
    private GridView listViewSecurityType;
    private GradientPanel gradientPanel3;
    private Label lblSecurityType;
    private StandardIconButton SecurityType;
    private ToolTip toolTip1;
    private StandardIconButton stdIconBtnEditSecurityType;
    private StandardIconButton stdIconBtnUpSecurityType;
    private StandardIconButton stdIconBtnDownSecurityType;
    private StandardIconButton stdIconBtnDeleteSecurityType;
    private GroupContainer gcManageTabs;
    private CheckBox ckboxEnableCorMaster;
    private CheckBox ckboxEnableCorTrade;
    private CheckBox ckboxEnableMbsPool;
    private CheckBox ckboxEnableTrade;
    private GradientPanel gradientPanel5;
    private Label label1;
    private Panel pnlGlobalSettings;
    private BorderPanel borderPanel1;
    private GroupContainer gcFields;
    private Panel pnlProviderSettings;
    private Panel pnlProviders;
    private Panel panel2;
    private Panel panel1;
    private GroupContainer gcTradeDescription;
    private GridView listViewTradeDescription;
    private GradientPanel gradientPanel2;
    private Label lblTradeDescription;
    private StandardIconButton stdIconBtnNewTradeDescription;
    private StandardIconButton stdIconBtnEditTradeDescription;
    private StandardIconButton stdIconBtnUpTradeDescription;
    private StandardIconButton stdIconBtnDownTradeDescription;
    private StandardIconButton stdIconBtnDeleteTradeDescription;
    private GroupContainer gcCommitmentType;
    private GridView listCommitmentType;
    private GradientPanel gradientPanel1;
    private Label lblCommitmentType;
    private StandardIconButton stdIconBtnNewCommitment;
    private StandardIconButton stdIconBtnEditCommitment;
    private StandardIconButton stdIconBtnUpCommitment;
    private StandardIconButton stdIconBtnDownCommitment;
    private StandardIconButton stdIconBtnDeleteCommitment;
    private Label label3;
    private BorderPanel borderPanel2;
    private string tmpCommitmentAutonumber = "";
    private string tmpCommitmentTradeAutoNumber = "";
    private GroupContainer gcSecurityTerm;
    private GridView listViewSecurityTerm;
    private GradientPanel gradientPanel4;
    private Label lblSecurityTerm;
    private StandardIconButton btnAddSecurityTerm;
    private StandardIconButton btnEditSecurityTerm;
    private StandardIconButton btnUpSecurityTerm;
    private StandardIconButton btnDownSecurityTerm;
    private StandardIconButton btnDeleteSecurityTerm;
    private bool listViewSecurityType_SelectedIndexChanged_Enabled = true;
    private bool listViewProductName_SelectedIndexChanged_Enabled = true;
    private Sessions.Session session;
    private CheckBox ckboxEnableAutoCreation;
    private GroupContainer gcFannieMaeProductNameValue;
    private GridView listViewProductName;
    private StandardIconButton btnAddFannieMaeProductName;
    private StandardIconButton btnEditFannieMaeProductName;
    private StandardIconButton btnUpFannieMaeProductName;
    private StandardIconButton btnDownFannieMaeProductName;
    private StandardIconButton btnDeleteFannieMaeProductName;
    private GroupContainer gcAutoNumberMaster;
    private Label label5;
    private CheckBox ckboxEnableMasterNumber;
    private TextBox txtMasterCommitmentNumber;
    private Label label8;
    private GradientPanel gradientPanel7;
    private Label label9;
    private GroupContainer gcAutoNumberTrade;
    private Label label2;
    private CheckBox ckboxEnableCommitmentNumber;
    private TextBox txtTradeCommitmentNumber;
    private Label label4;
    private GradientPanel gradientPanel6;
    private Label label6;
    private CheckBox ckBox_EnableFMPMandGSE;
    private GroupContainer grpLoanDataSychContainer;
    private TabControl tabLoanTrades;
    private TabPage tabLoanTradeFields;
    private TabPage tabMBSPoolFields;
    private Panel panel4;
    private CheckBox chkApplyImmedLTToAllLoans;
    private CheckBox chkApplyLTToAllLoans;
    private Panel panel3;
    private Label label7;
    private GridView gvMBSPoolTradeFields;
    private Panel panel5;
    private CheckBox chkApplyImmedMBSToAllLoans;
    private CheckBox chkApplyMBSToAllLoans;
    private CheckBox chkAllLoanTradeGV;
    private CheckBox chkAllMBSGV;
    private bool isSettingsSync;
    private bool ltUpdate;
    private CheckBox ckboxAllowBestEfforts;
    private CheckBox ckboxAllowPublishEvent;
    private GridView gvLoanTradeFields;
    private CheckBox chkEnableTpoTradeManagement;
    private Panel panel6;
    private CheckBox ckReceiveComConf;
    private CheckBox ckRequestPairOff;
    private CheckBox ckLoanDeleFromCorrTrade;
    private CheckBox ckLoanAssiToCorrTrade;
    private CheckBox ckEPPSLoanProgEliPricing;
    private CheckBox ckLoanEliCorrTrade;
    private CheckBox ckViewCorrMasterCom;
    private CheckBox ckViewCorrTrade;
    private CheckBox chkBoxBidTapeRegistration;
    private CheckBox chkBoxBidTapeMgmt;
    private bool mbsUpdate;
    private bool isAllowPublishChanged;

    public string[] SelectedFields
    {
      get
      {
        List<string> stringList = new List<string>();
        if (this.listCommitmentType.SelectedItems.Count == 0 && this.listViewTradeDescription.SelectedItems.Count == 0 && this.listViewSecurityTerm.SelectedItems.Count == 0)
          return (string[]) null;
        if (this.listCommitmentType.SelectedItems.Count > 0)
        {
          for (int index = 0; index < this.listCommitmentType.SelectedItems.Count; ++index)
            stringList.Add(string.Format("{0}_{1}", (object) this.listCommitmentType.SelectedItems[index].Text, (object) 6));
        }
        if (this.listViewTradeDescription.SelectedItems.Count > 0)
        {
          for (int index = 0; index < this.listViewTradeDescription.SelectedItems.Count; ++index)
            stringList.Add(string.Format("{0}_{1}", (object) this.listViewTradeDescription.SelectedItems[index].Text, (object) 7));
        }
        if (this.listViewSecurityTerm.SelectedItems.Count > 0)
        {
          for (int index = 0; index < this.listViewSecurityTerm.SelectedItems.Count; ++index)
            stringList.Add(string.Format("{0}_{1}", (object) this.listViewSecurityTerm.SelectedItems[index].Text, (object) 18));
        }
        return stringList.ToArray();
      }
    }

    public string[] SelectedSecurityTypeFields
    {
      get
      {
        return this.listViewSecurityType.SelectedItems.Count == 0 ? (string[]) null : this.listViewSecurityType.SelectedItems.Select<GVItem, string>((System.Func<GVItem, string>) (item => item.Text)).ToArray<string>();
      }
    }

    public TradeManagementFieldsSetup(SetUpContainer setupContainer)
      : this(setupContainer, Session.DefaultInstance)
    {
    }

    public TradeManagementFieldsSetup(SetUpContainer setupContainer, Sessions.Session session)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.session = session;
      this.Reset();
      if (setupContainer == null)
      {
        this.isSettingsSync = true;
        this.listCommitmentType.AllowMultiselect = this.isSettingsSync;
        this.listViewTradeDescription.AllowMultiselect = this.isSettingsSync;
        this.listViewSecurityType.AllowMultiselect = this.isSettingsSync;
        this.listViewSecurityTerm.AllowMultiselect = this.isSettingsSync;
        this.stdIconBtnNewCommitment.Enabled = false;
        this.stdIconBtnEditCommitment.Enabled = false;
        this.stdIconBtnUpCommitment.Enabled = false;
        this.stdIconBtnDownCommitment.Enabled = false;
        this.stdIconBtnDeleteCommitment.Enabled = false;
        this.stdIconBtnNewTradeDescription.Enabled = false;
        this.stdIconBtnEditTradeDescription.Enabled = false;
        this.stdIconBtnUpTradeDescription.Enabled = false;
        this.stdIconBtnDownTradeDescription.Enabled = false;
        this.stdIconBtnDeleteTradeDescription.Enabled = false;
        this.SecurityType.Enabled = false;
        this.stdIconBtnEditSecurityType.Enabled = false;
        this.stdIconBtnUpSecurityType.Enabled = false;
        this.stdIconBtnDownSecurityType.Enabled = false;
        this.stdIconBtnDeleteSecurityType.Enabled = false;
        this.btnAddSecurityTerm.Enabled = false;
        this.btnEditSecurityTerm.Enabled = false;
        this.btnUpSecurityTerm.Enabled = false;
        this.btnDownSecurityTerm.Enabled = false;
        this.btnDeleteSecurityTerm.Enabled = false;
        this.listViewSecurityType.ItemDoubleClick -= new GVItemEventHandler(this.listViewSecurityType_ItemDoubleClick);
        this.listCommitmentType.DoubleClick -= new EventHandler(this.listCommitmentType_DoubleClick);
        this.listViewSecurityTerm.ItemDoubleClick -= new GVItemEventHandler(this.listViewSecurityTerm_ItemDoubleClick);
        this.listViewTradeDescription.DoubleClick -= new EventHandler(this.listViewTradeDescription_DoubleClick);
        this.listViewProductName.ItemDoubleClick -= new GVItemEventHandler(this.listViewProductName_ItemDoubleClick);
        this.gcAutoNumberMaster.Enabled = false;
        this.gcAutoNumberTrade.Enabled = false;
        this.gcManageTabs.Enabled = false;
      }
      this.listCommitmentType_SelectedIndexChanged((object) this, (EventArgs) null);
      this.listViewTradeDescription_SelectedIndexChanged((object) this, (EventArgs) null);
      this.listViewSecurityType_SelectedIndexChanged((object) this, (EventArgs) null);
      this.listViewSecurityTerm_SelectedIndexChanged((object) this, (EventArgs) null);
      this.listViewProductName_SelectedIndexChanged((object) this, (EventArgs) null);
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
      GVColumn gvColumn15 = new GVColumn();
      this.gcSecurityType = new GroupContainer();
      this.listViewSecurityType = new GridView();
      this.gradientPanel3 = new GradientPanel();
      this.lblSecurityType = new Label();
      this.SecurityType = new StandardIconButton();
      this.stdIconBtnEditSecurityType = new StandardIconButton();
      this.stdIconBtnUpSecurityType = new StandardIconButton();
      this.stdIconBtnDownSecurityType = new StandardIconButton();
      this.stdIconBtnDeleteSecurityType = new StandardIconButton();
      this.gcManageTabs = new GroupContainer();
      this.panel6 = new Panel();
      this.ckReceiveComConf = new CheckBox();
      this.ckRequestPairOff = new CheckBox();
      this.ckLoanDeleFromCorrTrade = new CheckBox();
      this.ckLoanAssiToCorrTrade = new CheckBox();
      this.ckEPPSLoanProgEliPricing = new CheckBox();
      this.ckLoanEliCorrTrade = new CheckBox();
      this.ckViewCorrMasterCom = new CheckBox();
      this.ckViewCorrTrade = new CheckBox();
      this.chkEnableTpoTradeManagement = new CheckBox();
      this.ckboxAllowBestEfforts = new CheckBox();
      this.ckboxAllowPublishEvent = new CheckBox();
      this.ckBox_EnableFMPMandGSE = new CheckBox();
      this.ckboxEnableAutoCreation = new CheckBox();
      this.ckboxEnableCorMaster = new CheckBox();
      this.ckboxEnableCorTrade = new CheckBox();
      this.ckboxEnableMbsPool = new CheckBox();
      this.ckboxEnableTrade = new CheckBox();
      this.gradientPanel5 = new GradientPanel();
      this.label1 = new Label();
      this.pnlGlobalSettings = new Panel();
      this.gcFannieMaeProductNameValue = new GroupContainer();
      this.listViewProductName = new GridView();
      this.btnAddFannieMaeProductName = new StandardIconButton();
      this.btnEditFannieMaeProductName = new StandardIconButton();
      this.btnUpFannieMaeProductName = new StandardIconButton();
      this.btnDownFannieMaeProductName = new StandardIconButton();
      this.btnDeleteFannieMaeProductName = new StandardIconButton();
      this.gcAutoNumberMaster = new GroupContainer();
      this.label5 = new Label();
      this.ckboxEnableMasterNumber = new CheckBox();
      this.txtMasterCommitmentNumber = new TextBox();
      this.label8 = new Label();
      this.gradientPanel7 = new GradientPanel();
      this.label9 = new Label();
      this.gcAutoNumberTrade = new GroupContainer();
      this.label2 = new Label();
      this.ckboxEnableCommitmentNumber = new CheckBox();
      this.txtTradeCommitmentNumber = new TextBox();
      this.label4 = new Label();
      this.gradientPanel6 = new GradientPanel();
      this.label6 = new Label();
      this.grpLoanDataSychContainer = new GroupContainer();
      this.tabLoanTrades = new TabControl();
      this.tabLoanTradeFields = new TabPage();
      this.chkAllLoanTradeGV = new CheckBox();
      this.gvLoanTradeFields = new GridView();
      this.panel4 = new Panel();
      this.chkApplyImmedLTToAllLoans = new CheckBox();
      this.chkApplyLTToAllLoans = new CheckBox();
      this.tabMBSPoolFields = new TabPage();
      this.chkAllMBSGV = new CheckBox();
      this.gvMBSPoolTradeFields = new GridView();
      this.panel5 = new Panel();
      this.chkApplyImmedMBSToAllLoans = new CheckBox();
      this.chkApplyMBSToAllLoans = new CheckBox();
      this.panel3 = new Panel();
      this.label7 = new Label();
      this.borderPanel1 = new BorderPanel();
      this.gcFields = new GroupContainer();
      this.pnlProviderSettings = new Panel();
      this.pnlProviders = new Panel();
      this.panel2 = new Panel();
      this.gcSecurityTerm = new GroupContainer();
      this.listViewSecurityTerm = new GridView();
      this.gradientPanel4 = new GradientPanel();
      this.lblSecurityTerm = new Label();
      this.btnAddSecurityTerm = new StandardIconButton();
      this.btnEditSecurityTerm = new StandardIconButton();
      this.btnUpSecurityTerm = new StandardIconButton();
      this.btnDownSecurityTerm = new StandardIconButton();
      this.btnDeleteSecurityTerm = new StandardIconButton();
      this.panel1 = new Panel();
      this.gcTradeDescription = new GroupContainer();
      this.listViewTradeDescription = new GridView();
      this.gradientPanel2 = new GradientPanel();
      this.lblTradeDescription = new Label();
      this.stdIconBtnNewTradeDescription = new StandardIconButton();
      this.stdIconBtnEditTradeDescription = new StandardIconButton();
      this.stdIconBtnUpTradeDescription = new StandardIconButton();
      this.stdIconBtnDownTradeDescription = new StandardIconButton();
      this.stdIconBtnDeleteTradeDescription = new StandardIconButton();
      this.gcCommitmentType = new GroupContainer();
      this.listCommitmentType = new GridView();
      this.gradientPanel1 = new GradientPanel();
      this.lblCommitmentType = new Label();
      this.stdIconBtnNewCommitment = new StandardIconButton();
      this.stdIconBtnEditCommitment = new StandardIconButton();
      this.stdIconBtnUpCommitment = new StandardIconButton();
      this.stdIconBtnDownCommitment = new StandardIconButton();
      this.stdIconBtnDeleteCommitment = new StandardIconButton();
      this.label3 = new Label();
      this.toolTip1 = new ToolTip(this.components);
      this.borderPanel2 = new BorderPanel();
      this.chkBoxBidTapeMgmt = new CheckBox();
      this.chkBoxBidTapeRegistration = new CheckBox();
      this.gcSecurityType.SuspendLayout();
      this.gradientPanel3.SuspendLayout();
      ((ISupportInitialize) this.SecurityType).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEditSecurityType).BeginInit();
      ((ISupportInitialize) this.stdIconBtnUpSecurityType).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDownSecurityType).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDeleteSecurityType).BeginInit();
      this.gcManageTabs.SuspendLayout();
      this.panel6.SuspendLayout();
      this.gradientPanel5.SuspendLayout();
      this.pnlGlobalSettings.SuspendLayout();
      this.gcFannieMaeProductNameValue.SuspendLayout();
      ((ISupportInitialize) this.btnAddFannieMaeProductName).BeginInit();
      ((ISupportInitialize) this.btnEditFannieMaeProductName).BeginInit();
      ((ISupportInitialize) this.btnUpFannieMaeProductName).BeginInit();
      ((ISupportInitialize) this.btnDownFannieMaeProductName).BeginInit();
      ((ISupportInitialize) this.btnDeleteFannieMaeProductName).BeginInit();
      this.gcAutoNumberMaster.SuspendLayout();
      this.gradientPanel7.SuspendLayout();
      this.gcAutoNumberTrade.SuspendLayout();
      this.gradientPanel6.SuspendLayout();
      this.grpLoanDataSychContainer.SuspendLayout();
      this.tabLoanTrades.SuspendLayout();
      this.tabLoanTradeFields.SuspendLayout();
      this.panel4.SuspendLayout();
      this.tabMBSPoolFields.SuspendLayout();
      this.panel5.SuspendLayout();
      this.panel3.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.gcFields.SuspendLayout();
      this.pnlProviders.SuspendLayout();
      this.panel2.SuspendLayout();
      this.gcSecurityTerm.SuspendLayout();
      this.gradientPanel4.SuspendLayout();
      ((ISupportInitialize) this.btnAddSecurityTerm).BeginInit();
      ((ISupportInitialize) this.btnEditSecurityTerm).BeginInit();
      ((ISupportInitialize) this.btnUpSecurityTerm).BeginInit();
      ((ISupportInitialize) this.btnDownSecurityTerm).BeginInit();
      ((ISupportInitialize) this.btnDeleteSecurityTerm).BeginInit();
      this.panel1.SuspendLayout();
      this.gcTradeDescription.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnNewTradeDescription).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEditTradeDescription).BeginInit();
      ((ISupportInitialize) this.stdIconBtnUpTradeDescription).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDownTradeDescription).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDeleteTradeDescription).BeginInit();
      this.gcCommitmentType.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnNewCommitment).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEditCommitment).BeginInit();
      ((ISupportInitialize) this.stdIconBtnUpCommitment).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDownCommitment).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDeleteCommitment).BeginInit();
      this.borderPanel2.SuspendLayout();
      this.SuspendLayout();
      this.gcSecurityType.Controls.Add((Control) this.listViewSecurityType);
      this.gcSecurityType.Controls.Add((Control) this.gradientPanel3);
      this.gcSecurityType.Controls.Add((Control) this.SecurityType);
      this.gcSecurityType.Controls.Add((Control) this.stdIconBtnEditSecurityType);
      this.gcSecurityType.Controls.Add((Control) this.stdIconBtnUpSecurityType);
      this.gcSecurityType.Controls.Add((Control) this.stdIconBtnDownSecurityType);
      this.gcSecurityType.Controls.Add((Control) this.stdIconBtnDeleteSecurityType);
      this.gcSecurityType.Dock = DockStyle.Left;
      this.gcSecurityType.HeaderForeColor = SystemColors.ControlText;
      this.gcSecurityType.Location = new Point(0, 0);
      this.gcSecurityType.Name = "gcSecurityType";
      this.gcSecurityType.Size = new Size(505, 240);
      this.gcSecurityType.TabIndex = 91;
      this.gcSecurityType.Text = "Security Type Field Values";
      this.listViewSecurityType.AllowMultiselect = false;
      this.listViewSecurityType.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnName";
      gvColumn1.Text = "Security Type";
      gvColumn1.Width = 200;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnProgramType";
      gvColumn2.Text = "Program Type";
      gvColumn2.Width = 200;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnTerm";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Term";
      gvColumn3.Width = 103;
      this.listViewSecurityType.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.listViewSecurityType.Dock = DockStyle.Fill;
      this.listViewSecurityType.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewSecurityType.Location = new Point(1, 57);
      this.listViewSecurityType.Name = "listViewSecurityType";
      this.listViewSecurityType.Size = new Size(503, 182);
      this.listViewSecurityType.SortOption = GVSortOption.None;
      this.listViewSecurityType.TabIndex = 60;
      this.listViewSecurityType.SelectedIndexChanged += new EventHandler(this.listViewSecurityType_SelectedIndexChanged);
      this.listViewSecurityType.ItemDoubleClick += new GVItemEventHandler(this.listViewSecurityType_ItemDoubleClick);
      this.gradientPanel3.Borders = AnchorStyles.Bottom;
      this.gradientPanel3.Controls.Add((Control) this.lblSecurityType);
      this.gradientPanel3.Dock = DockStyle.Top;
      this.gradientPanel3.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel3.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel3.Location = new Point(1, 26);
      this.gradientPanel3.Name = "gradientPanel3";
      this.gradientPanel3.Size = new Size(503, 31);
      this.gradientPanel3.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel3.TabIndex = 75;
      this.lblSecurityType.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblSecurityType.BackColor = Color.Transparent;
      this.lblSecurityType.Location = new Point(5, 0);
      this.lblSecurityType.Name = "lblSecurityType";
      this.lblSecurityType.Size = new Size(489, 28);
      this.lblSecurityType.TabIndex = 61;
      this.lblSecurityType.Text = "Create and edit descriptions and settings for Security Types.";
      this.lblSecurityType.TextAlign = ContentAlignment.MiddleLeft;
      this.SecurityType.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.SecurityType.BackColor = Color.Transparent;
      this.SecurityType.Location = new Point(395, 5);
      this.SecurityType.MouseDownImage = (Image) null;
      this.SecurityType.Name = "SecurityType";
      this.SecurityType.Size = new Size(16, 16);
      this.SecurityType.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.SecurityType.TabIndex = 73;
      this.SecurityType.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.SecurityType, "New Security Type");
      this.SecurityType.Click += new EventHandler(this.buttonNew_Click);
      this.stdIconBtnEditSecurityType.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEditSecurityType.BackColor = Color.Transparent;
      this.stdIconBtnEditSecurityType.Location = new Point(417, 5);
      this.stdIconBtnEditSecurityType.MouseDownImage = (Image) null;
      this.stdIconBtnEditSecurityType.Name = "stdIconBtnEditSecurityType";
      this.stdIconBtnEditSecurityType.Size = new Size(16, 16);
      this.stdIconBtnEditSecurityType.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEditSecurityType.TabIndex = 72;
      this.stdIconBtnEditSecurityType.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEditSecurityType, "Edit Security Type");
      this.stdIconBtnEditSecurityType.Click += new EventHandler(this.stdIconBtnEditSecurityType_Click);
      this.stdIconBtnUpSecurityType.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnUpSecurityType.BackColor = Color.Transparent;
      this.stdIconBtnUpSecurityType.Location = new Point(439, 5);
      this.stdIconBtnUpSecurityType.MouseDownImage = (Image) null;
      this.stdIconBtnUpSecurityType.Name = "stdIconBtnUpSecurityType";
      this.stdIconBtnUpSecurityType.Size = new Size(16, 16);
      this.stdIconBtnUpSecurityType.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.stdIconBtnUpSecurityType.TabIndex = 71;
      this.stdIconBtnUpSecurityType.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnUpSecurityType, "Move Security Type Up");
      this.stdIconBtnUpSecurityType.Click += new EventHandler(this.buttonUp_Click);
      this.stdIconBtnDownSecurityType.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDownSecurityType.BackColor = Color.Transparent;
      this.stdIconBtnDownSecurityType.Location = new Point(461, 5);
      this.stdIconBtnDownSecurityType.MouseDownImage = (Image) null;
      this.stdIconBtnDownSecurityType.Name = "stdIconBtnDownSecurityType";
      this.stdIconBtnDownSecurityType.Size = new Size(16, 16);
      this.stdIconBtnDownSecurityType.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.stdIconBtnDownSecurityType.TabIndex = 70;
      this.stdIconBtnDownSecurityType.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDownSecurityType, "Move Security Type Down");
      this.stdIconBtnDownSecurityType.Click += new EventHandler(this.buttonDown_Click);
      this.stdIconBtnDeleteSecurityType.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDeleteSecurityType.BackColor = Color.Transparent;
      this.stdIconBtnDeleteSecurityType.Location = new Point(483, 5);
      this.stdIconBtnDeleteSecurityType.MouseDownImage = (Image) null;
      this.stdIconBtnDeleteSecurityType.Name = "stdIconBtnDeleteSecurityType";
      this.stdIconBtnDeleteSecurityType.Size = new Size(16, 16);
      this.stdIconBtnDeleteSecurityType.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDeleteSecurityType.TabIndex = 68;
      this.stdIconBtnDeleteSecurityType.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDeleteSecurityType, "Delete Security Type");
      this.stdIconBtnDeleteSecurityType.Click += new EventHandler(this.buttonDelete_Click);
      this.gcManageTabs.Controls.Add((Control) this.chkBoxBidTapeRegistration);
      this.gcManageTabs.Controls.Add((Control) this.chkBoxBidTapeMgmt);
      this.gcManageTabs.Controls.Add((Control) this.panel6);
      this.gcManageTabs.Controls.Add((Control) this.ckboxAllowBestEfforts);
      this.gcManageTabs.Controls.Add((Control) this.ckboxAllowPublishEvent);
      this.gcManageTabs.Controls.Add((Control) this.ckBox_EnableFMPMandGSE);
      this.gcManageTabs.Controls.Add((Control) this.ckboxEnableAutoCreation);
      this.gcManageTabs.Controls.Add((Control) this.ckboxEnableCorMaster);
      this.gcManageTabs.Controls.Add((Control) this.ckboxEnableCorTrade);
      this.gcManageTabs.Controls.Add((Control) this.ckboxEnableMbsPool);
      this.gcManageTabs.Controls.Add((Control) this.ckboxEnableTrade);
      this.gcManageTabs.Controls.Add((Control) this.gradientPanel5);
      this.gcManageTabs.Dock = DockStyle.Top;
      this.gcManageTabs.HeaderForeColor = SystemColors.ControlText;
      this.gcManageTabs.Location = new Point(0, 0);
      this.gcManageTabs.Name = "gcManageTabs";
      this.gcManageTabs.Size = new Size(1006, 465);
      this.gcManageTabs.TabIndex = 92;
      this.gcManageTabs.Text = "Trade Management Tabs";
      this.panel6.Controls.Add((Control) this.ckReceiveComConf);
      this.panel6.Controls.Add((Control) this.ckRequestPairOff);
      this.panel6.Controls.Add((Control) this.ckLoanDeleFromCorrTrade);
      this.panel6.Controls.Add((Control) this.ckLoanAssiToCorrTrade);
      this.panel6.Controls.Add((Control) this.ckEPPSLoanProgEliPricing);
      this.panel6.Controls.Add((Control) this.ckLoanEliCorrTrade);
      this.panel6.Controls.Add((Control) this.ckViewCorrMasterCom);
      this.panel6.Controls.Add((Control) this.ckViewCorrTrade);
      this.panel6.Controls.Add((Control) this.chkEnableTpoTradeManagement);
      this.panel6.Dock = DockStyle.Bottom;
      this.panel6.Location = new Point(1, 303);
      this.panel6.Name = "panel6";
      this.panel6.Size = new Size(1004, 188);
      this.panel6.TabIndex = 95;
      this.ckReceiveComConf.AutoSize = true;
      this.ckReceiveComConf.Location = new Point(52, 160);
      this.ckReceiveComConf.Name = "ckReceiveComConf";
      this.ckReceiveComConf.Size = new Size(187, 17);
      this.ckReceiveComConf.TabIndex = 102;
      this.ckReceiveComConf.Text = "Receive Commitment Confirmation";
      this.ckReceiveComConf.UseVisualStyleBackColor = true;
      this.ckReceiveComConf.CheckedChanged += new EventHandler(this.CheckBox_CheckedChanged);
      this.ckRequestPairOff.AutoSize = true;
      this.ckRequestPairOff.Location = new Point(52, 140);
      this.ckRequestPairOff.Name = "ckRequestPairOff";
      this.ckRequestPairOff.Size = new Size(104, 17);
      this.ckRequestPairOff.TabIndex = 101;
      this.ckRequestPairOff.Text = "Request Pair Off";
      this.ckRequestPairOff.UseVisualStyleBackColor = true;
      this.ckRequestPairOff.CheckedChanged += new EventHandler(this.CheckBox_CheckedChanged);
      this.ckLoanDeleFromCorrTrade.AutoSize = true;
      this.ckLoanDeleFromCorrTrade.Location = new Point(52, 120);
      this.ckLoanDeleFromCorrTrade.Name = "ckLoanDeleFromCorrTrade";
      this.ckLoanDeleFromCorrTrade.Size = new Size(218, 17);
      this.ckLoanDeleFromCorrTrade.TabIndex = 100;
      this.ckLoanDeleFromCorrTrade.Text = "Loan Deletion from Correspondent Trade";
      this.ckLoanDeleFromCorrTrade.UseVisualStyleBackColor = true;
      this.ckLoanDeleFromCorrTrade.CheckedChanged += new EventHandler(this.CheckBox_CheckedChanged);
      this.ckLoanAssiToCorrTrade.AutoSize = true;
      this.ckLoanAssiToCorrTrade.Location = new Point(52, 100);
      this.ckLoanAssiToCorrTrade.Name = "ckLoanAssiToCorrTrade";
      this.ckLoanAssiToCorrTrade.Size = new Size(222, 17);
      this.ckLoanAssiToCorrTrade.TabIndex = 99;
      this.ckLoanAssiToCorrTrade.Text = "Loan Assignment to Correspondent Trade";
      this.ckLoanAssiToCorrTrade.UseVisualStyleBackColor = true;
      this.ckLoanAssiToCorrTrade.CheckedChanged += new EventHandler(this.CheckBox_CheckedChanged);
      this.ckEPPSLoanProgEliPricing.AutoSize = true;
      this.ckEPPSLoanProgEliPricing.Location = new Point(52, 80);
      this.ckEPPSLoanProgEliPricing.Name = "ckEPPSLoanProgEliPricing";
      this.ckEPPSLoanProgEliPricing.Size = new Size(202, 17);
      this.ckEPPSLoanProgEliPricing.TabIndex = 98;
      this.ckEPPSLoanProgEliPricing.Text = "ICE PPE Loan Program Eligibility/Pricing";
      this.ckEPPSLoanProgEliPricing.UseVisualStyleBackColor = true;
      this.ckEPPSLoanProgEliPricing.CheckedChanged += new EventHandler(this.CheckBox_CheckedChanged);
      this.ckLoanEliCorrTrade.AutoSize = true;
      this.ckLoanEliCorrTrade.Location = new Point(52, 60);
      this.ckLoanEliCorrTrade.Name = "ckLoanEliCorrTrade";
      this.ckLoanEliCorrTrade.Size = new Size(207, 17);
      this.ckLoanEliCorrTrade.TabIndex = 97;
      this.ckLoanEliCorrTrade.Text = "Loan Eligibility to Correspondent Trade";
      this.ckLoanEliCorrTrade.UseVisualStyleBackColor = true;
      this.ckLoanEliCorrTrade.CheckedChanged += new EventHandler(this.CheckBox_CheckedChanged);
      this.ckViewCorrMasterCom.AutoSize = true;
      this.ckViewCorrMasterCom.Location = new Point(52, 40);
      this.ckViewCorrMasterCom.Name = "ckViewCorrMasterCom";
      this.ckViewCorrMasterCom.Size = new Size(216, 17);
      this.ckViewCorrMasterCom.TabIndex = 96;
      this.ckViewCorrMasterCom.Text = "View Correspondent Master Commitment";
      this.ckViewCorrMasterCom.UseVisualStyleBackColor = true;
      this.ckViewCorrMasterCom.CheckedChanged += new EventHandler(this.CheckBox_CheckedChanged);
      this.ckViewCorrTrade.AutoSize = true;
      this.ckViewCorrTrade.Location = new Point(52, 20);
      this.ckViewCorrTrade.Name = "ckViewCorrTrade";
      this.ckViewCorrTrade.Size = new Size(152, 17);
      this.ckViewCorrTrade.TabIndex = 95;
      this.ckViewCorrTrade.Text = "View Correspondent Trade";
      this.ckViewCorrTrade.UseVisualStyleBackColor = true;
      this.ckViewCorrTrade.CheckedChanged += new EventHandler(this.CheckBox_CheckedChanged);
      this.chkEnableTpoTradeManagement.AutoSize = true;
      this.chkEnableTpoTradeManagement.Location = new Point(24, 0);
      this.chkEnableTpoTradeManagement.Name = "chkEnableTpoTradeManagement";
      this.chkEnableTpoTradeManagement.Size = new Size(180, 17);
      this.chkEnableTpoTradeManagement.TabIndex = 86;
      this.chkEnableTpoTradeManagement.Text = "Enable TPO Trade Management";
      this.chkEnableTpoTradeManagement.UseVisualStyleBackColor = true;
      this.chkEnableTpoTradeManagement.CheckedChanged += new EventHandler(this.chkEnableTpoTradeManagement_CheckedChanged);
      this.ckboxAllowBestEfforts.AutoSize = true;
      this.ckboxAllowBestEfforts.Location = new Point(52, 170);
      this.ckboxAllowBestEfforts.Name = "ckboxAllowBestEfforts";
      this.ckboxAllowBestEfforts.Size = new Size(231, 17);
      this.ckboxAllowBestEfforts.TabIndex = 82;
      this.ckboxAllowBestEfforts.Text = "Allow Best Efforts for Correspondent Trades";
      this.ckboxAllowBestEfforts.UseVisualStyleBackColor = true;
      this.ckboxAllowBestEfforts.CheckedChanged += new EventHandler(this.ckboxAllowBestEfforts_CheckedChanged);
      this.ckboxAllowPublishEvent.AutoSize = true;
      this.ckboxAllowPublishEvent.Location = new Point(52, 148);
      this.ckboxAllowPublishEvent.Name = "ckboxAllowPublishEvent";
      this.ckboxAllowPublishEvent.Size = new Size(242, 17);
      this.ckboxAllowPublishEvent.TabIndex = 82;
      this.ckboxAllowPublishEvent.Text = "Allow Publish Event for Correspondent Trades";
      this.ckboxAllowPublishEvent.UseVisualStyleBackColor = true;
      this.ckBox_EnableFMPMandGSE.AutoSize = true;
      this.ckBox_EnableFMPMandGSE.Enabled = false;
      this.ckBox_EnableFMPMandGSE.Location = new Point(52, 107);
      this.ckBox_EnableFMPMandGSE.Name = "ckBox_EnableFMPMandGSE";
      this.ckBox_EnableFMPMandGSE.Size = new Size(344, 17);
      this.ckBox_EnableFMPMandGSE.TabIndex = 81;
      this.ckBox_EnableFMPMandGSE.Text = "Enable Fannie Mae PE MBS Pool Type and GSE Commitments tab ";
      this.ckBox_EnableFMPMandGSE.UseVisualStyleBackColor = true;
      this.ckBox_EnableFMPMandGSE.CheckedChanged += new EventHandler(this.ckBox_EnableFMPMandGSE_CheckedChanged);
      this.ckboxEnableAutoCreation.AutoSize = true;
      this.ckboxEnableAutoCreation.Location = new Point(52, 213);
      this.ckboxEnableAutoCreation.Name = "ckboxEnableAutoCreation";
      this.ckboxEnableAutoCreation.Size = new Size(374, 17);
      this.ckboxEnableAutoCreation.TabIndex = 80;
      this.ckboxEnableAutoCreation.Text = "Enable Auto - create of Correspondent Trades for Individual Delivery Type";
      this.ckboxEnableAutoCreation.UseVisualStyleBackColor = true;
      this.ckboxEnableAutoCreation.CheckedChanged += new EventHandler(this.ckboxEnableAutoCreation_CheckedChanged);
      this.ckboxEnableCorMaster.AutoSize = true;
      this.ckboxEnableCorMaster.Location = new Point(52, 191);
      this.ckboxEnableCorMaster.Name = "ckboxEnableCorMaster";
      this.ckboxEnableCorMaster.Size = new Size(641, 17);
      this.ckboxEnableCorMaster.TabIndex = 79;
      this.ckboxEnableCorMaster.Text = "Enable Correspondent Masters tab and enforce Masters for all Delivery Types ( Includes Auto - create for individual Delivery Types)";
      this.ckboxEnableCorMaster.UseVisualStyleBackColor = true;
      this.ckboxEnableCorMaster.CheckedChanged += new EventHandler(this.ckboxEnableCorMaster_CheckedChanged);
      this.ckboxEnableCorTrade.AutoSize = true;
      this.ckboxEnableCorTrade.Location = new Point(24, 128);
      this.ckboxEnableCorTrade.Name = "ckboxEnableCorTrade";
      this.ckboxEnableCorTrade.Size = new Size(185, 17);
      this.ckboxEnableCorTrade.TabIndex = 78;
      this.ckboxEnableCorTrade.Text = "Enable Correspondent Trades tab";
      this.ckboxEnableCorTrade.UseVisualStyleBackColor = true;
      this.ckboxEnableCorTrade.CheckedChanged += new EventHandler(this.ckboxEnableCorTrade_CheckedChanged);
      this.ckboxEnableMbsPool.AutoSize = true;
      this.ckboxEnableMbsPool.Location = new Point(52, 86);
      this.ckboxEnableMbsPool.Name = "ckboxEnableMbsPool";
      this.ckboxEnableMbsPool.Size = new Size(132, 17);
      this.ckboxEnableMbsPool.TabIndex = 77;
      this.ckboxEnableMbsPool.Text = "Enable MBS Pools tab";
      this.ckboxEnableMbsPool.UseVisualStyleBackColor = true;
      this.ckboxEnableMbsPool.CheckedChanged += new EventHandler(this.ckboxEnableMbsPool_CheckedChanged);
      this.ckboxEnableTrade.AutoSize = true;
      this.ckboxEnableTrade.Location = new Point(24, 66);
      this.ckboxEnableTrade.Name = "ckboxEnableTrade";
      this.ckboxEnableTrade.Size = new Size(399, 17);
      this.ckboxEnableTrade.TabIndex = 76;
      this.ckboxEnableTrade.Text = "Enable Security Trades, Loan Search, Loan Trades, and Master Contracts tabs";
      this.ckboxEnableTrade.UseVisualStyleBackColor = true;
      this.ckboxEnableTrade.CheckedChanged += new EventHandler(this.ckboxEnableTrade_CheckedChanged);
      this.gradientPanel5.Borders = AnchorStyles.Bottom;
      this.gradientPanel5.Controls.Add((Control) this.label1);
      this.gradientPanel5.Dock = DockStyle.Top;
      this.gradientPanel5.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel5.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel5.Location = new Point(1, 26);
      this.gradientPanel5.Name = "gradientPanel5";
      this.gradientPanel5.Size = new Size(1004, 31);
      this.gradientPanel5.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel5.TabIndex = 75;
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(5, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(990, 28);
      this.label1.TabIndex = 61;
      this.label1.Text = "Configure access to tabs in Trade Management";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlGlobalSettings.Controls.Add((Control) this.gcFannieMaeProductNameValue);
      this.pnlGlobalSettings.Controls.Add((Control) this.gcAutoNumberMaster);
      this.pnlGlobalSettings.Controls.Add((Control) this.gcAutoNumberTrade);
      this.pnlGlobalSettings.Controls.Add((Control) this.grpLoanDataSychContainer);
      this.pnlGlobalSettings.Controls.Add((Control) this.gcManageTabs);
      this.pnlGlobalSettings.Dock = DockStyle.Top;
      this.pnlGlobalSettings.Location = new Point(0, 530);
      this.pnlGlobalSettings.Name = "pnlGlobalSettings";
      this.pnlGlobalSettings.Size = new Size(1006, 1291);
      this.pnlGlobalSettings.TabIndex = 3;
      this.gcFannieMaeProductNameValue.Controls.Add((Control) this.listViewProductName);
      this.gcFannieMaeProductNameValue.Controls.Add((Control) this.btnAddFannieMaeProductName);
      this.gcFannieMaeProductNameValue.Controls.Add((Control) this.btnEditFannieMaeProductName);
      this.gcFannieMaeProductNameValue.Controls.Add((Control) this.btnUpFannieMaeProductName);
      this.gcFannieMaeProductNameValue.Controls.Add((Control) this.btnDownFannieMaeProductName);
      this.gcFannieMaeProductNameValue.Controls.Add((Control) this.btnDeleteFannieMaeProductName);
      this.gcFannieMaeProductNameValue.Dock = DockStyle.Top;
      this.gcFannieMaeProductNameValue.HeaderForeColor = SystemColors.ControlText;
      this.gcFannieMaeProductNameValue.Location = new Point(0, 1129);
      this.gcFannieMaeProductNameValue.Name = "gcFannieMaeProductNameValue";
      this.gcFannieMaeProductNameValue.Size = new Size(1006, 229);
      this.gcFannieMaeProductNameValue.TabIndex = 96;
      this.gcFannieMaeProductNameValue.Text = "Fannie Mae Product Name Values";
      this.listViewProductName.BorderStyle = BorderStyle.None;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColumnName";
      gvColumn4.Text = "Fannie Mae Product Name";
      gvColumn4.Width = 200;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ColumnProductDisplayName";
      gvColumn5.Text = "Product Display Name";
      gvColumn5.Width = 200;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "ColumnDescription";
      gvColumn6.SpringToFit = true;
      gvColumn6.Text = "Product Description";
      gvColumn6.Width = 604;
      this.listViewProductName.Columns.AddRange(new GVColumn[3]
      {
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.listViewProductName.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewProductName.Location = new Point(1, 26);
      this.listViewProductName.Name = "listViewProductName";
      this.listViewProductName.Size = new Size(1004, 143);
      this.listViewProductName.SortOption = GVSortOption.None;
      this.listViewProductName.TabIndex = 60;
      this.listViewProductName.SelectedIndexChanged += new EventHandler(this.listViewProductName_SelectedIndexChanged);
      this.listViewProductName.DoubleClick += new EventHandler(this.listViewProductName_ItemDoubleClick);
      this.btnAddFannieMaeProductName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddFannieMaeProductName.BackColor = Color.Transparent;
      this.btnAddFannieMaeProductName.Location = new Point(882, 5);
      this.btnAddFannieMaeProductName.MouseDownImage = (Image) null;
      this.btnAddFannieMaeProductName.Name = "btnAddFannieMaeProductName";
      this.btnAddFannieMaeProductName.Size = new Size(16, 16);
      this.btnAddFannieMaeProductName.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddFannieMaeProductName.TabIndex = 73;
      this.btnAddFannieMaeProductName.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddFannieMaeProductName, "New Fannie Mae Product Name");
      this.btnAddFannieMaeProductName.Click += new EventHandler(this.buttonNew_Click);
      this.btnEditFannieMaeProductName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEditFannieMaeProductName.BackColor = Color.Transparent;
      this.btnEditFannieMaeProductName.Location = new Point(904, 5);
      this.btnEditFannieMaeProductName.MouseDownImage = (Image) null;
      this.btnEditFannieMaeProductName.Name = "btnEditFannieMaeProductName";
      this.btnEditFannieMaeProductName.Size = new Size(16, 16);
      this.btnEditFannieMaeProductName.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditFannieMaeProductName.TabIndex = 72;
      this.btnEditFannieMaeProductName.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEditFannieMaeProductName, "Edit Security Type");
      this.btnEditFannieMaeProductName.Click += new EventHandler(this.btnEditFannieMaeProductName_Click);
      this.btnUpFannieMaeProductName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnUpFannieMaeProductName.BackColor = Color.Transparent;
      this.btnUpFannieMaeProductName.Location = new Point(926, 5);
      this.btnUpFannieMaeProductName.MouseDownImage = (Image) null;
      this.btnUpFannieMaeProductName.Name = "btnUpFannieMaeProductName";
      this.btnUpFannieMaeProductName.Size = new Size(16, 16);
      this.btnUpFannieMaeProductName.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnUpFannieMaeProductName.TabIndex = 71;
      this.btnUpFannieMaeProductName.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnUpFannieMaeProductName, "Move Security Type Up");
      this.btnUpFannieMaeProductName.Click += new EventHandler(this.buttonUp_Click);
      this.btnDownFannieMaeProductName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDownFannieMaeProductName.BackColor = Color.Transparent;
      this.btnDownFannieMaeProductName.Location = new Point(948, 5);
      this.btnDownFannieMaeProductName.MouseDownImage = (Image) null;
      this.btnDownFannieMaeProductName.Name = "btnDownFannieMaeProductName";
      this.btnDownFannieMaeProductName.Size = new Size(16, 16);
      this.btnDownFannieMaeProductName.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnDownFannieMaeProductName.TabIndex = 70;
      this.btnDownFannieMaeProductName.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDownFannieMaeProductName, "Move Security Type Down");
      this.btnDownFannieMaeProductName.Click += new EventHandler(this.buttonDown_Click);
      this.btnDeleteFannieMaeProductName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeleteFannieMaeProductName.BackColor = Color.Transparent;
      this.btnDeleteFannieMaeProductName.Location = new Point(970, 5);
      this.btnDeleteFannieMaeProductName.MouseDownImage = (Image) null;
      this.btnDeleteFannieMaeProductName.Name = "btnDeleteFannieMaeProductName";
      this.btnDeleteFannieMaeProductName.Size = new Size(16, 16);
      this.btnDeleteFannieMaeProductName.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteFannieMaeProductName.TabIndex = 68;
      this.btnDeleteFannieMaeProductName.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDeleteFannieMaeProductName, "Delete Security Type");
      this.btnDeleteFannieMaeProductName.Click += new EventHandler(this.buttonDelete_Click);
      this.gcAutoNumberMaster.Controls.Add((Control) this.label5);
      this.gcAutoNumberMaster.Controls.Add((Control) this.ckboxEnableMasterNumber);
      this.gcAutoNumberMaster.Controls.Add((Control) this.txtMasterCommitmentNumber);
      this.gcAutoNumberMaster.Controls.Add((Control) this.label8);
      this.gcAutoNumberMaster.Controls.Add((Control) this.gradientPanel7);
      this.gcAutoNumberMaster.Dock = DockStyle.Top;
      this.gcAutoNumberMaster.HeaderForeColor = SystemColors.ControlText;
      this.gcAutoNumberMaster.Location = new Point(0, 973);
      this.gcAutoNumberMaster.Name = "gcAutoNumberMaster";
      this.gcAutoNumberMaster.Size = new Size(1006, 156);
      this.gcAutoNumberMaster.TabIndex = 95;
      this.gcAutoNumberMaster.Text = "Auto Correspondent Master Commitment Numbering";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(18, 105);
      this.label5.Name = "label5";
      this.label5.Size = new Size(83, 13);
      this.label5.TabIndex = 81;
      this.label5.Text = "Starting Number";
      this.ckboxEnableMasterNumber.AutoSize = true;
      this.ckboxEnableMasterNumber.Location = new Point(21, 72);
      this.ckboxEnableMasterNumber.Name = "ckboxEnableMasterNumber";
      this.ckboxEnableMasterNumber.Size = new Size(288, 17);
      this.ckboxEnableMasterNumber.TabIndex = 80;
      this.ckboxEnableMasterNumber.Text = "Auto create correspondent master commitment numbers";
      this.ckboxEnableMasterNumber.UseVisualStyleBackColor = true;
      this.ckboxEnableMasterNumber.CheckedChanged += new EventHandler(this.ckboxEnableMasterNumber_CheckedChanged);
      this.txtMasterCommitmentNumber.Location = new Point(102, 102);
      this.txtMasterCommitmentNumber.Name = "txtMasterCommitmentNumber";
      this.txtMasterCommitmentNumber.Size = new Size(266, 20);
      this.txtMasterCommitmentNumber.TabIndex = 78;
      this.txtMasterCommitmentNumber.TextChanged += new EventHandler(this.txtMasterCommitmentNumber_TextChanged);
      this.label8.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.label8.BackColor = Color.Transparent;
      this.label8.Location = new Point(99, 121);
      this.label8.Name = "label8";
      this.label8.Size = new Size(804, 31);
      this.label8.TabIndex = 76;
      this.label8.Text = "Example: 100001";
      this.label8.TextAlign = ContentAlignment.MiddleLeft;
      this.gradientPanel7.Borders = AnchorStyles.Bottom;
      this.gradientPanel7.Controls.Add((Control) this.label9);
      this.gradientPanel7.Dock = DockStyle.Top;
      this.gradientPanel7.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel7.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel7.Location = new Point(1, 26);
      this.gradientPanel7.Name = "gradientPanel7";
      this.gradientPanel7.Size = new Size(1004, 31);
      this.gradientPanel7.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel7.TabIndex = 75;
      this.label9.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.label9.BackColor = Color.Transparent;
      this.label9.Location = new Point(5, 0);
      this.label9.Name = "label9";
      this.label9.Size = new Size(990, 28);
      this.label9.TabIndex = 61;
      this.label9.Text = "Define the assignment of Correspondent Master Commitment numbers to Correspondent Master Commitments. The maximum correspondent master commitment number length is 10 digits. ";
      this.label9.TextAlign = ContentAlignment.MiddleLeft;
      this.gcAutoNumberTrade.Controls.Add((Control) this.label2);
      this.gcAutoNumberTrade.Controls.Add((Control) this.ckboxEnableCommitmentNumber);
      this.gcAutoNumberTrade.Controls.Add((Control) this.txtTradeCommitmentNumber);
      this.gcAutoNumberTrade.Controls.Add((Control) this.label4);
      this.gcAutoNumberTrade.Controls.Add((Control) this.gradientPanel6);
      this.gcAutoNumberTrade.Dock = DockStyle.Top;
      this.gcAutoNumberTrade.HeaderForeColor = SystemColors.ControlText;
      this.gcAutoNumberTrade.Location = new Point(0, 818);
      this.gcAutoNumberTrade.Name = "gcAutoNumberTrade";
      this.gcAutoNumberTrade.Size = new Size(1006, 155);
      this.gcAutoNumberTrade.TabIndex = 93;
      this.gcAutoNumberTrade.Text = "Auto Correspondent Trade Commitment Numbering";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(18, 112);
      this.label2.Name = "label2";
      this.label2.Size = new Size(83, 13);
      this.label2.TabIndex = 80;
      this.label2.Text = "Starting Number";
      this.ckboxEnableCommitmentNumber.AutoSize = true;
      this.ckboxEnableCommitmentNumber.Location = new Point(21, 74);
      this.ckboxEnableCommitmentNumber.Name = "ckboxEnableCommitmentNumber";
      this.ckboxEnableCommitmentNumber.Size = new Size(281, 17);
      this.ckboxEnableCommitmentNumber.TabIndex = 79;
      this.ckboxEnableCommitmentNumber.Text = "Auto create correspondent trade commitment numbers";
      this.ckboxEnableCommitmentNumber.UseVisualStyleBackColor = true;
      this.ckboxEnableCommitmentNumber.CheckedChanged += new EventHandler(this.ckboxEnableCommitmentNumber_CheckedChanged);
      this.txtTradeCommitmentNumber.Location = new Point(102, 105);
      this.txtTradeCommitmentNumber.Name = "txtTradeCommitmentNumber";
      this.txtTradeCommitmentNumber.Size = new Size(266, 20);
      this.txtTradeCommitmentNumber.TabIndex = 78;
      this.txtTradeCommitmentNumber.TextChanged += new EventHandler(this.txtTradeCommitmentNumber_TextChanged);
      this.label4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(99, 128);
      this.label4.Name = "label4";
      this.label4.Size = new Size(804, 18);
      this.label4.TabIndex = 76;
      this.label4.Text = "Example: 100001";
      this.label4.TextAlign = ContentAlignment.MiddleLeft;
      this.gradientPanel6.Borders = AnchorStyles.Bottom;
      this.gradientPanel6.Controls.Add((Control) this.label6);
      this.gradientPanel6.Dock = DockStyle.Top;
      this.gradientPanel6.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel6.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel6.Location = new Point(1, 26);
      this.gradientPanel6.Name = "gradientPanel6";
      this.gradientPanel6.Size = new Size(1004, 31);
      this.gradientPanel6.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel6.TabIndex = 75;
      this.label6.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.label6.BackColor = Color.Transparent;
      this.label6.Location = new Point(5, 0);
      this.label6.Name = "label6";
      this.label6.Size = new Size(990, 28);
      this.label6.TabIndex = 61;
      this.label6.Text = "Define the assignment of Correspondent Trade Commitment numbers to Correspondent Trades. The maximum correspondent trade commitment number length is 18 digits.";
      this.label6.TextAlign = ContentAlignment.MiddleLeft;
      this.grpLoanDataSychContainer.Controls.Add((Control) this.tabLoanTrades);
      this.grpLoanDataSychContainer.Controls.Add((Control) this.panel3);
      this.grpLoanDataSychContainer.Dock = DockStyle.Top;
      this.grpLoanDataSychContainer.HeaderForeColor = SystemColors.ControlText;
      this.grpLoanDataSychContainer.Location = new Point(0, 475);
      this.grpLoanDataSychContainer.Name = "grpLoanDataSychContainer";
      this.grpLoanDataSychContainer.Padding = new Padding(0, 0, 9, 0);
      this.grpLoanDataSychContainer.Size = new Size(1006, 343);
      this.grpLoanDataSychContainer.TabIndex = 74;
      this.grpLoanDataSychContainer.Text = "Loan Data Synchronization";
      this.tabLoanTrades.Controls.Add((Control) this.tabLoanTradeFields);
      this.tabLoanTrades.Controls.Add((Control) this.tabMBSPoolFields);
      this.tabLoanTrades.Dock = DockStyle.Bottom;
      this.tabLoanTrades.Location = new Point(1, 65);
      this.tabLoanTrades.Name = "tabLoanTrades";
      this.tabLoanTrades.SelectedIndex = 0;
      this.tabLoanTrades.Size = new Size(995, 277);
      this.tabLoanTrades.TabIndex = 0;
      this.tabLoanTradeFields.Controls.Add((Control) this.chkAllLoanTradeGV);
      this.tabLoanTradeFields.Controls.Add((Control) this.gvLoanTradeFields);
      this.tabLoanTradeFields.Controls.Add((Control) this.panel4);
      this.tabLoanTradeFields.Location = new Point(4, 22);
      this.tabLoanTradeFields.Name = "tabLoanTradeFields";
      this.tabLoanTradeFields.Padding = new Padding(3);
      this.tabLoanTradeFields.Size = new Size(987, 251);
      this.tabLoanTradeFields.TabIndex = 0;
      this.tabLoanTradeFields.Text = "Loan Trades";
      this.tabLoanTradeFields.UseVisualStyleBackColor = true;
      this.chkAllLoanTradeGV.AutoSize = true;
      this.chkAllLoanTradeGV.Location = new Point(10, 69);
      this.chkAllLoanTradeGV.Name = "chkAllLoanTradeGV";
      this.chkAllLoanTradeGV.Size = new Size(15, 14);
      this.chkAllLoanTradeGV.TabIndex = 3;
      this.chkAllLoanTradeGV.UseVisualStyleBackColor = true;
      this.chkAllLoanTradeGV.CheckedChanged += new EventHandler(this.chkAllLoanTradeGV_CheckedChanged);
      gvColumn7.CheckBoxes = true;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "select";
      gvColumn7.SortMethod = GVSortMethod.None;
      gvColumn7.Text = "";
      gvColumn7.Width = 50;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "colTradeField";
      gvColumn8.SortMethod = GVSortMethod.None;
      gvColumn8.Text = "Trade Field";
      gvColumn8.Width = 300;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "colLoanField";
      gvColumn9.SortMethod = GVSortMethod.None;
      gvColumn9.Text = "Loan Field";
      gvColumn9.Width = 300;
      this.gvLoanTradeFields.Columns.AddRange(new GVColumn[3]
      {
        gvColumn7,
        gvColumn8,
        gvColumn9
      });
      this.gvLoanTradeFields.Dock = DockStyle.Fill;
      this.gvLoanTradeFields.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvLoanTradeFields.Location = new Point(3, 65);
      this.gvLoanTradeFields.Margin = new Padding(3, 3, 10, 3);
      this.gvLoanTradeFields.Name = "gvLoanTradeFields";
      this.gvLoanTradeFields.Padding = new Padding(0, 0, 20, 0);
      this.gvLoanTradeFields.Size = new Size(981, 183);
      this.gvLoanTradeFields.SortIconVisible = false;
      this.gvLoanTradeFields.SortOption = GVSortOption.None;
      this.gvLoanTradeFields.TabIndex = 0;
      this.gvLoanTradeFields.SubItemCheck += new GVSubItemEventHandler(this.gvLoanTradeFields_SubItemCheck);
      this.panel4.Controls.Add((Control) this.chkApplyImmedLTToAllLoans);
      this.panel4.Controls.Add((Control) this.chkApplyLTToAllLoans);
      this.panel4.Dock = DockStyle.Top;
      this.panel4.Location = new Point(3, 3);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(981, 62);
      this.panel4.TabIndex = 2;
      this.chkApplyImmedLTToAllLoans.AutoSize = true;
      this.chkApplyImmedLTToAllLoans.Location = new Point(51, 32);
      this.chkApplyImmedLTToAllLoans.Name = "chkApplyImmedLTToAllLoans";
      this.chkApplyImmedLTToAllLoans.Size = new Size(280, 17);
      this.chkApplyImmedLTToAllLoans.TabIndex = 1;
      this.chkApplyImmedLTToAllLoans.Text = "Start update immediately after selecting Update Loans";
      this.chkApplyImmedLTToAllLoans.UseVisualStyleBackColor = true;
      this.chkApplyImmedLTToAllLoans.CheckedChanged += new EventHandler(this.chkApplyImmedLTToAllLoans_CheckedChanged);
      this.chkApplyLTToAllLoans.AutoSize = true;
      this.chkApplyLTToAllLoans.Location = new Point(23, 7);
      this.chkApplyLTToAllLoans.Name = "chkApplyLTToAllLoans";
      this.chkApplyLTToAllLoans.Size = new Size(228, 17);
      this.chkApplyLTToAllLoans.TabIndex = 0;
      this.chkApplyLTToAllLoans.Text = "Apply selections below to all updated loans";
      this.chkApplyLTToAllLoans.UseVisualStyleBackColor = true;
      this.chkApplyLTToAllLoans.CheckedChanged += new EventHandler(this.chkApplyLTToAllLoans_CheckedChanged);
      this.tabMBSPoolFields.Controls.Add((Control) this.chkAllMBSGV);
      this.tabMBSPoolFields.Controls.Add((Control) this.gvMBSPoolTradeFields);
      this.tabMBSPoolFields.Controls.Add((Control) this.panel5);
      this.tabMBSPoolFields.Location = new Point(4, 22);
      this.tabMBSPoolFields.Name = "tabMBSPoolFields";
      this.tabMBSPoolFields.Padding = new Padding(3);
      this.tabMBSPoolFields.Size = new Size(987, 251);
      this.tabMBSPoolFields.TabIndex = 1;
      this.tabMBSPoolFields.Text = "MBS Pools";
      this.tabMBSPoolFields.UseVisualStyleBackColor = true;
      this.chkAllMBSGV.AutoSize = true;
      this.chkAllMBSGV.Location = new Point(10, 69);
      this.chkAllMBSGV.Name = "chkAllMBSGV";
      this.chkAllMBSGV.Size = new Size(15, 14);
      this.chkAllMBSGV.TabIndex = 4;
      this.chkAllMBSGV.UseVisualStyleBackColor = true;
      this.chkAllMBSGV.CheckedChanged += new EventHandler(this.chkAllMBSGV_CheckedChanged);
      gvColumn10.CheckBoxes = true;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "select";
      gvColumn10.SortMethod = GVSortMethod.None;
      gvColumn10.Text = "";
      gvColumn10.Width = 50;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "colTradeField";
      gvColumn11.SortMethod = GVSortMethod.None;
      gvColumn11.Text = "Trade Field";
      gvColumn11.Width = 300;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "colLoanField";
      gvColumn12.SortMethod = GVSortMethod.None;
      gvColumn12.Text = "Loan Field";
      gvColumn12.Width = 300;
      this.gvMBSPoolTradeFields.Columns.AddRange(new GVColumn[3]
      {
        gvColumn10,
        gvColumn11,
        gvColumn12
      });
      this.gvMBSPoolTradeFields.Dock = DockStyle.Fill;
      this.gvMBSPoolTradeFields.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvMBSPoolTradeFields.Location = new Point(3, 65);
      this.gvMBSPoolTradeFields.Name = "gvMBSPoolTradeFields";
      this.gvMBSPoolTradeFields.Size = new Size(981, 183);
      this.gvMBSPoolTradeFields.SortIconVisible = false;
      this.gvMBSPoolTradeFields.SortOption = GVSortOption.None;
      this.gvMBSPoolTradeFields.TabIndex = 1;
      this.gvMBSPoolTradeFields.SubItemCheck += new GVSubItemEventHandler(this.gvMBSPoolTradeFields_SubItemCheck);
      this.panel5.Controls.Add((Control) this.chkApplyImmedMBSToAllLoans);
      this.panel5.Controls.Add((Control) this.chkApplyMBSToAllLoans);
      this.panel5.Dock = DockStyle.Top;
      this.panel5.Location = new Point(3, 3);
      this.panel5.Name = "panel5";
      this.panel5.Size = new Size(981, 62);
      this.panel5.TabIndex = 3;
      this.chkApplyImmedMBSToAllLoans.AutoSize = true;
      this.chkApplyImmedMBSToAllLoans.Location = new Point(51, 32);
      this.chkApplyImmedMBSToAllLoans.Name = "chkApplyImmedMBSToAllLoans";
      this.chkApplyImmedMBSToAllLoans.Size = new Size(280, 17);
      this.chkApplyImmedMBSToAllLoans.TabIndex = 1;
      this.chkApplyImmedMBSToAllLoans.Text = "Start update immediately after selecting Update Loans";
      this.chkApplyImmedMBSToAllLoans.UseVisualStyleBackColor = true;
      this.chkApplyImmedMBSToAllLoans.CheckedChanged += new EventHandler(this.chkApplyImmedMBSToAllLoans_CheckedChanged);
      this.chkApplyMBSToAllLoans.AutoSize = true;
      this.chkApplyMBSToAllLoans.Checked = true;
      this.chkApplyMBSToAllLoans.CheckState = CheckState.Checked;
      this.chkApplyMBSToAllLoans.Enabled = false;
      this.chkApplyMBSToAllLoans.Location = new Point(23, 7);
      this.chkApplyMBSToAllLoans.Name = "chkApplyMBSToAllLoans";
      this.chkApplyMBSToAllLoans.Size = new Size(228, 17);
      this.chkApplyMBSToAllLoans.TabIndex = 0;
      this.chkApplyMBSToAllLoans.Text = "Apply selections below to all updated loans";
      this.chkApplyMBSToAllLoans.UseVisualStyleBackColor = true;
      this.panel3.Controls.Add((Control) this.label7);
      this.panel3.Dock = DockStyle.Top;
      this.panel3.Location = new Point(1, 26);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(995, 33);
      this.panel3.TabIndex = 1;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(5, 11);
      this.label7.Name = "label7";
      this.label7.Size = new Size(706, 13);
      this.label7.TabIndex = 0;
      this.label7.Text = "Update the mapping of Trades fields for synchronization with Loan Data fields. Select the Loan Trade or MBS Pool tab to make the desired changes.";
      this.borderPanel1.AutoScroll = true;
      this.borderPanel1.BackColor = Color.WhiteSmoke;
      this.borderPanel1.Borders = AnchorStyles.None;
      this.borderPanel1.Controls.Add((Control) this.pnlGlobalSettings);
      this.borderPanel1.Controls.Add((Control) this.gcFields);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(1, 1);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(1006, 1551);
      this.borderPanel1.TabIndex = 2;
      this.gcFields.Borders = AnchorStyles.None;
      this.gcFields.Controls.Add((Control) this.pnlProviderSettings);
      this.gcFields.Controls.Add((Control) this.pnlProviders);
      this.gcFields.Dock = DockStyle.Top;
      this.gcFields.HeaderForeColor = SystemColors.ControlText;
      this.gcFields.Location = new Point(0, 0);
      this.gcFields.Name = "gcFields";
      this.gcFields.Size = new Size(1006, 530);
      this.gcFields.TabIndex = 1;
      this.gcFields.Text = "Trade Management Fields";
      this.pnlProviderSettings.Dock = DockStyle.Top;
      this.pnlProviderSettings.Location = new Point(0, 530);
      this.pnlProviderSettings.Name = "pnlProviderSettings";
      this.pnlProviderSettings.Size = new Size(1006, 288);
      this.pnlProviderSettings.TabIndex = 12;
      this.pnlProviders.Controls.Add((Control) this.panel2);
      this.pnlProviders.Controls.Add((Control) this.panel1);
      this.pnlProviders.Controls.Add((Control) this.label3);
      this.pnlProviders.Dock = DockStyle.Top;
      this.pnlProviders.Location = new Point(0, 25);
      this.pnlProviders.Name = "pnlProviders";
      this.pnlProviders.Size = new Size(1006, 505);
      this.pnlProviders.TabIndex = 11;
      this.panel2.Controls.Add((Control) this.gcSecurityTerm);
      this.panel2.Controls.Add((Control) this.gcSecurityType);
      this.panel2.Location = new Point(0, 265);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(1006, 240);
      this.panel2.TabIndex = 12;
      this.gcSecurityTerm.Controls.Add((Control) this.listViewSecurityTerm);
      this.gcSecurityTerm.Controls.Add((Control) this.gradientPanel4);
      this.gcSecurityTerm.Controls.Add((Control) this.btnAddSecurityTerm);
      this.gcSecurityTerm.Controls.Add((Control) this.btnEditSecurityTerm);
      this.gcSecurityTerm.Controls.Add((Control) this.btnUpSecurityTerm);
      this.gcSecurityTerm.Controls.Add((Control) this.btnDownSecurityTerm);
      this.gcSecurityTerm.Controls.Add((Control) this.btnDeleteSecurityTerm);
      this.gcSecurityTerm.Dock = DockStyle.Right;
      this.gcSecurityTerm.HeaderForeColor = SystemColors.ControlText;
      this.gcSecurityTerm.Location = new Point(505, 0);
      this.gcSecurityTerm.Name = "gcSecurityTerm";
      this.gcSecurityTerm.Size = new Size(501, 240);
      this.gcSecurityTerm.TabIndex = 92;
      this.gcSecurityTerm.Text = "Security Term Field Values";
      this.listViewSecurityTerm.AllowMultiselect = false;
      this.listViewSecurityTerm.BorderStyle = BorderStyle.None;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "Column1";
      gvColumn13.Text = "Personas";
      gvColumn13.Width = 400;
      this.listViewSecurityTerm.Columns.AddRange(new GVColumn[1]
      {
        gvColumn13
      });
      this.listViewSecurityTerm.Dock = DockStyle.Fill;
      this.listViewSecurityTerm.HeaderHeight = 0;
      this.listViewSecurityTerm.HeaderVisible = false;
      this.listViewSecurityTerm.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewSecurityTerm.Location = new Point(1, 57);
      this.listViewSecurityTerm.Name = "listViewSecurityTerm";
      this.listViewSecurityTerm.Size = new Size(499, 182);
      this.listViewSecurityTerm.TabIndex = 76;
      this.listViewSecurityTerm.SelectedIndexChanged += new EventHandler(this.listViewSecurityTerm_SelectedIndexChanged);
      this.listViewSecurityTerm.ItemDoubleClick += new GVItemEventHandler(this.listViewSecurityTerm_ItemDoubleClick);
      this.gradientPanel4.Borders = AnchorStyles.Bottom;
      this.gradientPanel4.Controls.Add((Control) this.lblSecurityTerm);
      this.gradientPanel4.Dock = DockStyle.Top;
      this.gradientPanel4.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel4.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel4.Location = new Point(1, 26);
      this.gradientPanel4.Name = "gradientPanel4";
      this.gradientPanel4.Size = new Size(499, 31);
      this.gradientPanel4.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel4.TabIndex = 75;
      this.lblSecurityTerm.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblSecurityTerm.BackColor = Color.Transparent;
      this.lblSecurityTerm.Location = new Point(5, 0);
      this.lblSecurityTerm.Name = "lblSecurityTerm";
      this.lblSecurityTerm.Size = new Size(485, 28);
      this.lblSecurityTerm.TabIndex = 61;
      this.lblSecurityTerm.Text = "Create and edit descriptions and settings for Security Terms.";
      this.lblSecurityTerm.TextAlign = ContentAlignment.MiddleLeft;
      this.btnAddSecurityTerm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddSecurityTerm.BackColor = Color.Transparent;
      this.btnAddSecurityTerm.Location = new Point(391, 5);
      this.btnAddSecurityTerm.MouseDownImage = (Image) null;
      this.btnAddSecurityTerm.Name = "btnAddSecurityTerm";
      this.btnAddSecurityTerm.Size = new Size(16, 16);
      this.btnAddSecurityTerm.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddSecurityTerm.TabIndex = 73;
      this.btnAddSecurityTerm.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddSecurityTerm, "New Security Term");
      this.btnAddSecurityTerm.Click += new EventHandler(this.buttonNew_Click);
      this.btnEditSecurityTerm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEditSecurityTerm.BackColor = Color.Transparent;
      this.btnEditSecurityTerm.Location = new Point(413, 5);
      this.btnEditSecurityTerm.MouseDownImage = (Image) null;
      this.btnEditSecurityTerm.Name = "btnEditSecurityTerm";
      this.btnEditSecurityTerm.Size = new Size(16, 16);
      this.btnEditSecurityTerm.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditSecurityTerm.TabIndex = 72;
      this.btnEditSecurityTerm.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEditSecurityTerm, "Edit Security Term");
      this.btnEditSecurityTerm.Click += new EventHandler(this.btnEditSecurityTerm_Click);
      this.btnUpSecurityTerm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnUpSecurityTerm.BackColor = Color.Transparent;
      this.btnUpSecurityTerm.Location = new Point(435, 5);
      this.btnUpSecurityTerm.MouseDownImage = (Image) null;
      this.btnUpSecurityTerm.Name = "btnUpSecurityTerm";
      this.btnUpSecurityTerm.Size = new Size(16, 16);
      this.btnUpSecurityTerm.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnUpSecurityTerm.TabIndex = 71;
      this.btnUpSecurityTerm.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnUpSecurityTerm, "Move Security Term Up");
      this.btnUpSecurityTerm.Click += new EventHandler(this.buttonUp_Click);
      this.btnDownSecurityTerm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDownSecurityTerm.BackColor = Color.Transparent;
      this.btnDownSecurityTerm.Location = new Point(457, 5);
      this.btnDownSecurityTerm.MouseDownImage = (Image) null;
      this.btnDownSecurityTerm.Name = "btnDownSecurityTerm";
      this.btnDownSecurityTerm.Size = new Size(16, 16);
      this.btnDownSecurityTerm.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnDownSecurityTerm.TabIndex = 70;
      this.btnDownSecurityTerm.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDownSecurityTerm, "Move Security Term Down");
      this.btnDownSecurityTerm.Click += new EventHandler(this.buttonDown_Click);
      this.btnDeleteSecurityTerm.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeleteSecurityTerm.BackColor = Color.Transparent;
      this.btnDeleteSecurityTerm.Location = new Point(479, 5);
      this.btnDeleteSecurityTerm.MouseDownImage = (Image) null;
      this.btnDeleteSecurityTerm.Name = "btnDeleteSecurityTerm";
      this.btnDeleteSecurityTerm.Size = new Size(16, 16);
      this.btnDeleteSecurityTerm.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteSecurityTerm.TabIndex = 68;
      this.btnDeleteSecurityTerm.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDeleteSecurityTerm, "Delete Security Term");
      this.btnDeleteSecurityTerm.Click += new EventHandler(this.buttonDelete_Click);
      this.panel1.Controls.Add((Control) this.gcTradeDescription);
      this.panel1.Controls.Add((Control) this.gcCommitmentType);
      this.panel1.Location = new Point(0, 25);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(1008, 240);
      this.panel1.TabIndex = 11;
      this.gcTradeDescription.Controls.Add((Control) this.listViewTradeDescription);
      this.gcTradeDescription.Controls.Add((Control) this.gradientPanel2);
      this.gcTradeDescription.Controls.Add((Control) this.stdIconBtnNewTradeDescription);
      this.gcTradeDescription.Controls.Add((Control) this.stdIconBtnEditTradeDescription);
      this.gcTradeDescription.Controls.Add((Control) this.stdIconBtnUpTradeDescription);
      this.gcTradeDescription.Controls.Add((Control) this.stdIconBtnDownTradeDescription);
      this.gcTradeDescription.Controls.Add((Control) this.stdIconBtnDeleteTradeDescription);
      this.gcTradeDescription.Dock = DockStyle.Right;
      this.gcTradeDescription.HeaderForeColor = SystemColors.ControlText;
      this.gcTradeDescription.Location = new Point(506, 0);
      this.gcTradeDescription.Name = "gcTradeDescription";
      this.gcTradeDescription.Size = new Size(502, 240);
      this.gcTradeDescription.TabIndex = 9;
      this.gcTradeDescription.Text = "Trade Description Dropdown List";
      this.listViewTradeDescription.AllowMultiselect = false;
      this.listViewTradeDescription.BorderStyle = BorderStyle.None;
      gvColumn14.ImageIndex = -1;
      gvColumn14.Name = "Column1";
      gvColumn14.Text = "Personas";
      gvColumn14.Width = 400;
      this.listViewTradeDescription.Columns.AddRange(new GVColumn[1]
      {
        gvColumn14
      });
      this.listViewTradeDescription.Dock = DockStyle.Fill;
      this.listViewTradeDescription.HeaderHeight = 0;
      this.listViewTradeDescription.HeaderVisible = false;
      this.listViewTradeDescription.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewTradeDescription.Location = new Point(1, 57);
      this.listViewTradeDescription.Name = "listViewTradeDescription";
      this.listViewTradeDescription.Size = new Size(500, 182);
      this.listViewTradeDescription.TabIndex = 6;
      this.listViewTradeDescription.SelectedIndexChanged += new EventHandler(this.listViewTradeDescription_SelectedIndexChanged);
      this.listViewTradeDescription.DoubleClick += new EventHandler(this.listViewTradeDescription_DoubleClick);
      this.gradientPanel2.Borders = AnchorStyles.Bottom;
      this.gradientPanel2.Controls.Add((Control) this.lblTradeDescription);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.Location = new Point(1, 26);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(500, 31);
      this.gradientPanel2.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel2.TabIndex = 80;
      this.lblTradeDescription.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblTradeDescription.BackColor = Color.Transparent;
      this.lblTradeDescription.Location = new Point(5, 0);
      this.lblTradeDescription.Name = "lblTradeDescription";
      this.lblTradeDescription.Size = new Size(490, 28);
      this.lblTradeDescription.TabIndex = 58;
      this.lblTradeDescription.Text = "Create and edit Trade Descriptions.";
      this.lblTradeDescription.TextAlign = ContentAlignment.MiddleLeft;
      this.stdIconBtnNewTradeDescription.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNewTradeDescription.BackColor = Color.Transparent;
      this.stdIconBtnNewTradeDescription.Location = new Point(392, 5);
      this.stdIconBtnNewTradeDescription.MouseDownImage = (Image) null;
      this.stdIconBtnNewTradeDescription.Name = "stdIconBtnNewTradeDescription";
      this.stdIconBtnNewTradeDescription.Size = new Size(16, 16);
      this.stdIconBtnNewTradeDescription.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNewTradeDescription.TabIndex = 78;
      this.stdIconBtnNewTradeDescription.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNewTradeDescription, "New Trade Description");
      this.stdIconBtnNewTradeDescription.Click += new EventHandler(this.buttonNew_Click);
      this.stdIconBtnEditTradeDescription.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEditTradeDescription.BackColor = Color.Transparent;
      this.stdIconBtnEditTradeDescription.Location = new Point(414, 5);
      this.stdIconBtnEditTradeDescription.MouseDownImage = (Image) null;
      this.stdIconBtnEditTradeDescription.Name = "stdIconBtnEditTradeDescription";
      this.stdIconBtnEditTradeDescription.Size = new Size(16, 16);
      this.stdIconBtnEditTradeDescription.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEditTradeDescription.TabIndex = 77;
      this.stdIconBtnEditTradeDescription.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEditTradeDescription, "Edit Trade Description");
      this.stdIconBtnEditTradeDescription.Click += new EventHandler(this.stdIconBtnEditTradeDescription_Click);
      this.stdIconBtnUpTradeDescription.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnUpTradeDescription.BackColor = Color.Transparent;
      this.stdIconBtnUpTradeDescription.Location = new Point(436, 5);
      this.stdIconBtnUpTradeDescription.MouseDownImage = (Image) null;
      this.stdIconBtnUpTradeDescription.Name = "stdIconBtnUpTradeDescription";
      this.stdIconBtnUpTradeDescription.Size = new Size(16, 16);
      this.stdIconBtnUpTradeDescription.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.stdIconBtnUpTradeDescription.TabIndex = 76;
      this.stdIconBtnUpTradeDescription.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnUpTradeDescription, "Move Trade Description Up");
      this.stdIconBtnUpTradeDescription.Click += new EventHandler(this.buttonUp_Click);
      this.stdIconBtnDownTradeDescription.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDownTradeDescription.BackColor = Color.Transparent;
      this.stdIconBtnDownTradeDescription.Location = new Point(458, 5);
      this.stdIconBtnDownTradeDescription.MouseDownImage = (Image) null;
      this.stdIconBtnDownTradeDescription.Name = "stdIconBtnDownTradeDescription";
      this.stdIconBtnDownTradeDescription.Size = new Size(16, 16);
      this.stdIconBtnDownTradeDescription.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.stdIconBtnDownTradeDescription.TabIndex = 75;
      this.stdIconBtnDownTradeDescription.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDownTradeDescription, "Move Trade Description Down");
      this.stdIconBtnDownTradeDescription.Click += new EventHandler(this.buttonDown_Click);
      this.stdIconBtnDeleteTradeDescription.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDeleteTradeDescription.BackColor = Color.Transparent;
      this.stdIconBtnDeleteTradeDescription.Location = new Point(480, 5);
      this.stdIconBtnDeleteTradeDescription.MouseDownImage = (Image) null;
      this.stdIconBtnDeleteTradeDescription.Name = "stdIconBtnDeleteTradeDescription";
      this.stdIconBtnDeleteTradeDescription.Size = new Size(16, 16);
      this.stdIconBtnDeleteTradeDescription.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDeleteTradeDescription.TabIndex = 74;
      this.stdIconBtnDeleteTradeDescription.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDeleteTradeDescription, "Delete Trade Description");
      this.stdIconBtnDeleteTradeDescription.Click += new EventHandler(this.buttonDelete_Click);
      this.gcCommitmentType.Controls.Add((Control) this.listCommitmentType);
      this.gcCommitmentType.Controls.Add((Control) this.gradientPanel1);
      this.gcCommitmentType.Controls.Add((Control) this.stdIconBtnNewCommitment);
      this.gcCommitmentType.Controls.Add((Control) this.stdIconBtnEditCommitment);
      this.gcCommitmentType.Controls.Add((Control) this.stdIconBtnUpCommitment);
      this.gcCommitmentType.Controls.Add((Control) this.stdIconBtnDownCommitment);
      this.gcCommitmentType.Controls.Add((Control) this.stdIconBtnDeleteCommitment);
      this.gcCommitmentType.Dock = DockStyle.Left;
      this.gcCommitmentType.HeaderForeColor = SystemColors.ControlText;
      this.gcCommitmentType.Location = new Point(0, 0);
      this.gcCommitmentType.Name = "gcCommitmentType";
      this.gcCommitmentType.Size = new Size(506, 240);
      this.gcCommitmentType.TabIndex = 8;
      this.gcCommitmentType.Text = "Commitment Type Dropdown List";
      this.listCommitmentType.AllowMultiselect = false;
      this.listCommitmentType.BorderStyle = BorderStyle.None;
      gvColumn15.ImageIndex = -1;
      gvColumn15.Name = "Column1";
      gvColumn15.Text = "Personas";
      gvColumn15.Width = 400;
      this.listCommitmentType.Columns.AddRange(new GVColumn[1]
      {
        gvColumn15
      });
      this.listCommitmentType.Dock = DockStyle.Fill;
      this.listCommitmentType.HeaderHeight = 0;
      this.listCommitmentType.HeaderVisible = false;
      this.listCommitmentType.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listCommitmentType.Location = new Point(1, 57);
      this.listCommitmentType.Name = "listCommitmentType";
      this.listCommitmentType.Size = new Size(504, 182);
      this.listCommitmentType.TabIndex = 0;
      this.listCommitmentType.SelectedIndexChanged += new EventHandler(this.listCommitmentType_SelectedIndexChanged);
      this.listCommitmentType.DoubleClick += new EventHandler(this.listCommitmentType_DoubleClick);
      this.gradientPanel1.Borders = AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.lblCommitmentType);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(1, 26);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(504, 31);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel1.TabIndex = 80;
      this.lblCommitmentType.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblCommitmentType.BackColor = Color.Transparent;
      this.lblCommitmentType.Location = new Point(5, 0);
      this.lblCommitmentType.Name = "lblCommitmentType";
      this.lblCommitmentType.Size = new Size(588, 28);
      this.lblCommitmentType.TabIndex = 56;
      this.lblCommitmentType.Text = "Create and edit descriptions for the Commitment Type.";
      this.lblCommitmentType.TextAlign = ContentAlignment.MiddleLeft;
      this.stdIconBtnNewCommitment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNewCommitment.BackColor = Color.Transparent;
      this.stdIconBtnNewCommitment.Location = new Point(395, 4);
      this.stdIconBtnNewCommitment.MouseDownImage = (Image) null;
      this.stdIconBtnNewCommitment.Name = "stdIconBtnNewCommitment";
      this.stdIconBtnNewCommitment.Size = new Size(16, 16);
      this.stdIconBtnNewCommitment.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNewCommitment.TabIndex = 79;
      this.stdIconBtnNewCommitment.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNewCommitment, "New Commitment Type");
      this.stdIconBtnNewCommitment.Click += new EventHandler(this.buttonNew_Click);
      this.stdIconBtnEditCommitment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEditCommitment.BackColor = Color.Transparent;
      this.stdIconBtnEditCommitment.Location = new Point(417, 5);
      this.stdIconBtnEditCommitment.MouseDownImage = (Image) null;
      this.stdIconBtnEditCommitment.Name = "stdIconBtnEditCommitment";
      this.stdIconBtnEditCommitment.Size = new Size(16, 16);
      this.stdIconBtnEditCommitment.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEditCommitment.TabIndex = 77;
      this.stdIconBtnEditCommitment.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEditCommitment, "Edit Commitment Type");
      this.stdIconBtnEditCommitment.Click += new EventHandler(this.stdIconBtnEditCommitment_Click);
      this.stdIconBtnUpCommitment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnUpCommitment.BackColor = Color.Transparent;
      this.stdIconBtnUpCommitment.Location = new Point(439, 5);
      this.stdIconBtnUpCommitment.MouseDownImage = (Image) null;
      this.stdIconBtnUpCommitment.Name = "stdIconBtnUpCommitment";
      this.stdIconBtnUpCommitment.Size = new Size(16, 16);
      this.stdIconBtnUpCommitment.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.stdIconBtnUpCommitment.TabIndex = 76;
      this.stdIconBtnUpCommitment.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnUpCommitment, "Move Commitment Type Up");
      this.stdIconBtnUpCommitment.Click += new EventHandler(this.buttonUp_Click);
      this.stdIconBtnDownCommitment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDownCommitment.BackColor = Color.Transparent;
      this.stdIconBtnDownCommitment.Location = new Point(461, 5);
      this.stdIconBtnDownCommitment.MouseDownImage = (Image) null;
      this.stdIconBtnDownCommitment.Name = "stdIconBtnDownCommitment";
      this.stdIconBtnDownCommitment.Size = new Size(16, 16);
      this.stdIconBtnDownCommitment.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.stdIconBtnDownCommitment.TabIndex = 75;
      this.stdIconBtnDownCommitment.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDownCommitment, "Move Commitment Type Down");
      this.stdIconBtnDownCommitment.Click += new EventHandler(this.buttonDown_Click);
      this.stdIconBtnDeleteCommitment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDeleteCommitment.BackColor = Color.Transparent;
      this.stdIconBtnDeleteCommitment.Location = new Point(483, 5);
      this.stdIconBtnDeleteCommitment.MouseDownImage = (Image) null;
      this.stdIconBtnDeleteCommitment.Name = "stdIconBtnDeleteCommitment";
      this.stdIconBtnDeleteCommitment.Size = new Size(16, 16);
      this.stdIconBtnDeleteCommitment.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDeleteCommitment.TabIndex = 74;
      this.stdIconBtnDeleteCommitment.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDeleteCommitment, "Delete Commitment Type");
      this.stdIconBtnDeleteCommitment.Click += new EventHandler(this.buttonDelete_Click);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(5, 8);
      this.label3.Name = "label3";
      this.label3.Size = new Size(468, 13);
      this.label3.TabIndex = 10;
      this.label3.Text = "Create and edit options that display on specified drop-down lists for all types of Trades (and Pools).";
      this.borderPanel2.Controls.Add((Control) this.borderPanel1);
      this.borderPanel2.Dock = DockStyle.Fill;
      this.borderPanel2.Location = new Point(0, 0);
      this.borderPanel2.Name = "borderPanel2";
      this.borderPanel2.Size = new Size(1008, 1553);
      this.borderPanel2.TabIndex = 5;
      this.chkBoxBidTapeMgmt.AutoSize = true;
      this.chkBoxBidTapeMgmt.Location = new Point(25, 235);
      this.chkBoxBidTapeMgmt.Name = "chkBoxBidTapeMgmt";
      this.chkBoxBidTapeMgmt.Size = new Size(170, 17);
      this.chkBoxBidTapeMgmt.TabIndex = 96;
      this.chkBoxBidTapeMgmt.Visible = true;
      this.chkBoxBidTapeMgmt.Text = "Enable Bid Tape Management";
      this.chkBoxBidTapeMgmt.UseVisualStyleBackColor = true;
      this.chkBoxBidTapeMgmt.CheckedChanged += new EventHandler(this.chkBoxBidTapeMgmt_CheckedChanged);
      this.chkBoxBidTapeRegistration.AutoSize = true;
      this.chkBoxBidTapeRegistration.Location = new Point(52, 254);
      this.chkBoxBidTapeRegistration.Name = "chkBoxBidTapeRegistration";
      this.chkBoxBidTapeRegistration.Size = new Size(164, 17);
      this.chkBoxBidTapeRegistration.TabIndex = 97;
      this.chkBoxBidTapeRegistration.Visible = true;
      this.chkBoxBidTapeRegistration.Text = "Enable Bid Tape Registration";
      this.chkBoxBidTapeRegistration.UseVisualStyleBackColor = true;
      this.chkBoxBidTapeRegistration.CheckedChanged += new EventHandler(this.chkBoxBidTapeRegistration_CheckedChanged);
      this.Controls.Add((Control) this.borderPanel2);
      this.Name = nameof (TradeManagementFieldsSetup);
      this.Size = new Size(1008, 1553);
      this.SizeChanged += new EventHandler(this.SecondaryFieldsSetup_SizeChanged);
      this.gcSecurityType.ResumeLayout(false);
      this.gradientPanel3.ResumeLayout(false);
      ((ISupportInitialize) this.SecurityType).EndInit();
      ((ISupportInitialize) this.stdIconBtnEditSecurityType).EndInit();
      ((ISupportInitialize) this.stdIconBtnUpSecurityType).EndInit();
      ((ISupportInitialize) this.stdIconBtnDownSecurityType).EndInit();
      ((ISupportInitialize) this.stdIconBtnDeleteSecurityType).EndInit();
      this.gcManageTabs.ResumeLayout(false);
      this.gcManageTabs.PerformLayout();
      this.panel6.ResumeLayout(false);
      this.panel6.PerformLayout();
      this.gradientPanel5.ResumeLayout(false);
      this.pnlGlobalSettings.ResumeLayout(false);
      this.gcFannieMaeProductNameValue.ResumeLayout(false);
      ((ISupportInitialize) this.btnAddFannieMaeProductName).EndInit();
      ((ISupportInitialize) this.btnEditFannieMaeProductName).EndInit();
      ((ISupportInitialize) this.btnUpFannieMaeProductName).EndInit();
      ((ISupportInitialize) this.btnDownFannieMaeProductName).EndInit();
      ((ISupportInitialize) this.btnDeleteFannieMaeProductName).EndInit();
      this.gcAutoNumberMaster.ResumeLayout(false);
      this.gcAutoNumberMaster.PerformLayout();
      this.gradientPanel7.ResumeLayout(false);
      this.gcAutoNumberTrade.ResumeLayout(false);
      this.gcAutoNumberTrade.PerformLayout();
      this.gradientPanel6.ResumeLayout(false);
      this.grpLoanDataSychContainer.ResumeLayout(false);
      this.tabLoanTrades.ResumeLayout(false);
      this.tabLoanTradeFields.ResumeLayout(false);
      this.tabLoanTradeFields.PerformLayout();
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      this.tabMBSPoolFields.ResumeLayout(false);
      this.tabMBSPoolFields.PerformLayout();
      this.panel5.ResumeLayout(false);
      this.panel5.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.borderPanel1.ResumeLayout(false);
      this.gcFields.ResumeLayout(false);
      this.pnlProviders.ResumeLayout(false);
      this.pnlProviders.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.gcSecurityTerm.ResumeLayout(false);
      this.gradientPanel4.ResumeLayout(false);
      ((ISupportInitialize) this.btnAddSecurityTerm).EndInit();
      ((ISupportInitialize) this.btnEditSecurityTerm).EndInit();
      ((ISupportInitialize) this.btnUpSecurityTerm).EndInit();
      ((ISupportInitialize) this.btnDownSecurityTerm).EndInit();
      ((ISupportInitialize) this.btnDeleteSecurityTerm).EndInit();
      this.panel1.ResumeLayout(false);
      this.gcTradeDescription.ResumeLayout(false);
      this.gradientPanel2.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnNewTradeDescription).EndInit();
      ((ISupportInitialize) this.stdIconBtnEditTradeDescription).EndInit();
      ((ISupportInitialize) this.stdIconBtnUpTradeDescription).EndInit();
      ((ISupportInitialize) this.stdIconBtnDownTradeDescription).EndInit();
      ((ISupportInitialize) this.stdIconBtnDeleteTradeDescription).EndInit();
      this.gcCommitmentType.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnNewCommitment).EndInit();
      ((ISupportInitialize) this.stdIconBtnEditCommitment).EndInit();
      ((ISupportInitialize) this.stdIconBtnUpCommitment).EndInit();
      ((ISupportInitialize) this.stdIconBtnDownCommitment).EndInit();
      ((ISupportInitialize) this.stdIconBtnDeleteCommitment).EndInit();
      this.borderPanel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void initListView(ArrayList fields, GridView listview)
    {
      this.SecondaryFieldsSetup_SizeChanged((object) null, (EventArgs) null);
      listview.Items.Clear();
      if (fields == null || fields.Count == 0)
        return;
      listview.BeginUpdate();
      for (int index = 0; index < fields.Count; ++index)
      {
        GVItem gvItem = new GVItem(fields[index].ToString());
        listview.Items.Add(gvItem);
      }
      listview.EndUpdate();
    }

    private void initGridView(DataTable fields, GridView gridView, int tradeType, CheckBox chk)
    {
      gridView.Items.Clear();
      if (fields == null || fields.Rows.Count == 0)
        return;
      gridView.BeginUpdate();
      for (int index = 0; index < fields.Rows.Count; ++index)
      {
        if (!(fields.Rows[index]["TradeType"].ToString() != tradeType.ToString()) && fields.Rows[index]["ParentFieldId"].ToString() == "")
        {
          GVItem gvItem = new GVItem()
          {
            SubItems = {
              [0] = {
                Checked = fields.Rows[index]["Value"].ToString() == "True"
              }
            }
          };
          gvItem.SubItems.Add(new GVSubItem(fields.Rows[index]["SourceFieldDesc"]));
          gvItem.SubItems.Add(new GVSubItem(fields.Rows[index]["DestinationFieldDesc"]));
          gvItem.Tag = (object) fields.Rows[index];
          gridView.Items.Add(gvItem);
          DataRow[] dataRowArray = fields.Select("ParentFieldId = '" + fields.Rows[index]["FieldId"].ToString() + "' AND TradeType = '" + (object) tradeType + "'");
          if (((IEnumerable<DataRow>) dataRowArray).Count<DataRow>() > 0)
            this.AddChildLoanSynchFields(dataRowArray, gridView);
        }
      }
      int count1 = gridView.Items.Where<GVItem>((System.Func<GVItem, bool>) (i => i.SubItems[0].Checked && i.SubItems[0].CheckBoxVisible)).ToList<GVItem>().Count;
      int count2 = gridView.Items.Where<GVItem>((System.Func<GVItem, bool>) (i => i.SubItems[0].CheckBoxVisible)).ToList<GVItem>().Count;
      chk.Checked = count1 == count2;
      gridView.EndUpdate();
    }

    private void AddChildLoanSynchFields(DataRow[] rows, GridView gridView)
    {
      for (int index = 0; index < ((IEnumerable<DataRow>) rows).Count<DataRow>(); ++index)
      {
        GVItem gvItem = new GVItem()
        {
          SubItems = {
            [0] = {
              CheckBoxVisible = false
            }
          }
        };
        gvItem.SubItems.Add(new GVSubItem((object) ("            " + rows[index]["SourceFieldDesc"])));
        gvItem.SubItems.Add(new GVSubItem(rows[index]["DestinationFieldDesc"]));
        gvItem.Tag = (object) rows[index];
        gridView.Items.Add(gvItem);
      }
    }

    private void initSecurityTypeList()
    {
      GVItem[] items = this.convertSecurityTypes(this.session.ConfigurationManager.GetSecondarySecurityTypes());
      this.listViewSecurityType.Items.Clear();
      this.listViewSecurityType.Items.AddRange(items);
    }

    private GVItem[] convertSecurityTypes(DataTable securityTypesTable)
    {
      int num = 0;
      GVItem[] gvItemArray = new GVItem[securityTypesTable.Rows.Count];
      foreach (DataRow row in (InternalDataCollectionBase) securityTypesTable.Rows)
        gvItemArray[num++] = new GVItem()
        {
          SubItems = {
            new GVSubItem(row["Name"]),
            new GVSubItem(row["ProgramType"]),
            new GVSubItem((object) this.GetTermsDisplayText((int) row["Term1"], (int) row["Term2"])),
            new GVSubItem(row["Term1"]),
            new GVSubItem(row["Term2"])
          }
        };
      return gvItemArray;
    }

    private DataTable convertSecurityTypes(GVItemCollection gvItems)
    {
      DataTable dataTable = new DataTable("SecondarySecurityTypes");
      dataTable.Columns.Add("Name", typeof (string));
      dataTable.Columns.Add("ProgramType", typeof (string));
      dataTable.Columns.Add("Term1", typeof (int));
      dataTable.Columns.Add("Term2", typeof (int));
      foreach (GVItem gvItem in (IEnumerable<GVItem>) gvItems)
      {
        DataRow row = dataTable.NewRow();
        row["Name"] = (object) (string) gvItem.SubItems[0].Value;
        row["ProgramType"] = (object) (string) gvItem.SubItems[1].Value;
        row["Term1"] = (object) (int) gvItem.SubItems[3].Value;
        row["Term2"] = (object) (int) gvItem.SubItems[4].Value;
        dataTable.Rows.Add(row);
      }
      return dataTable;
    }

    private void initFannieMaeProductNameList()
    {
      GVItem[] items = this.convertProductName(this.session.ConfigurationManager.GetFannieMaeProductNames());
      this.listViewProductName.Items.Clear();
      this.listViewProductName.Items.AddRange(items);
    }

    private GVItem[] convertProductName(DataTable fannieMaeProductNamesTable)
    {
      int num = 0;
      GVItem[] gvItemArray = new GVItem[fannieMaeProductNamesTable.Rows.Count];
      foreach (DataRow row in (InternalDataCollectionBase) fannieMaeProductNamesTable.Rows)
        gvItemArray[num++] = new GVItem()
        {
          SubItems = {
            new GVSubItem(row["ProductName"]),
            new GVSubItem(row["DisplayName"]),
            new GVSubItem(row["Description"])
          }
        };
      return gvItemArray;
    }

    private DataTable convertProductName(GVItemCollection gvItems)
    {
      DataTable dataTable = new DataTable("FannieMaeProductName");
      dataTable.Columns.Add("ProductName", typeof (string));
      dataTable.Columns.Add("DisplayName", typeof (string));
      dataTable.Columns.Add("Description", typeof (string));
      foreach (GVItem gvItem in (IEnumerable<GVItem>) gvItems)
      {
        DataRow row = dataTable.NewRow();
        row["ProductName"] = (object) (string) gvItem.SubItems[0].Value;
        row["DisplayName"] = (object) (string) gvItem.SubItems[1].Value;
        row["Description"] = (object) (string) gvItem.SubItems[2].Value;
        dataTable.Rows.Add(row);
      }
      return dataTable;
    }

    private string GetTermsDisplayText(int term1, int term2)
    {
      return term1 == -1 && term2 == -1 ? "" : (term1 == -1 ? "" : term1.ToString()) + " - " + (term2 == -1 ? "" : term2.ToString()) + " mths";
    }

    public override void Reset()
    {
      this.initListView(this.session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.CommitmentTypeOption), this.listCommitmentType);
      this.initListView(this.session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.TradeDescriptionOption), this.listViewTradeDescription);
      this.initListView(this.session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.SecurityTerm), this.listViewSecurityTerm);
      DataTable synchronizationFields = this.session.ConfigurationManager.GetLoanSynchronizationFields();
      this.initGridView(synchronizationFields, this.gvLoanTradeFields, 2, this.chkAllLoanTradeGV);
      this.chkApplyLTToAllLoans_CheckedChanged((object) null, (EventArgs) null);
      this.initGridView(synchronizationFields, this.gvMBSPoolTradeFields, 3, this.chkAllMBSGV);
      this.initSecurityTypeList();
      this.initFannieMaeProductNameList();
      this.chkEnableTpoTradeManagement.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.EnableTPOTradeManagement"]);
      this.ckViewCorrTrade.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.ViewCorrTrade"]);
      this.ckViewCorrMasterCom.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.ViewCorrMasterCom"]);
      this.ckLoanEliCorrTrade.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.LoanEliCorrTrade"]);
      this.ckEPPSLoanProgEliPricing.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.EPPSLoanProgEliPricing"]);
      this.ckLoanAssiToCorrTrade.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.LoanAssiToCorrTrade"]);
      this.ckLoanDeleFromCorrTrade.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.LoanDeleFromCorrTrade"]);
      this.ckRequestPairOff.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.RequestPairOff"]);
      this.ckReceiveComConf.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.ReceiveComConf"]);
      this.ckboxEnableMbsPool.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.EnableMbsPool"]);
      this.ckboxEnableTrade.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.EnableTrade"]);
      this.ckboxEnableCorTrade.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.EnableCorrespondentTrade"]);
      this.chkBoxBidTapeMgmt.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.EnableBidTapeManagement"]);
      this.chkBoxBidTapeRegistration.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.EnableBidTapeRegistration"]);
      this.ckboxAllowBestEfforts.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.AllowBestEfforts"]);
      this.ckboxEnableCorMaster.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.EnableCorrespondentMaster"]);
      this.ckboxEnableAutoCreation.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.EnableAutoCreationCT"]);
      this.ckboxEnableCommitmentNumber.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.EnableAutoCommitmentNumber"]);
      this.ckboxEnableMasterNumber.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.EnableAutoMasterCommitmentNumber"]);
      this.ckBox_EnableFMPMandGSE.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.EnableFMPMandGSE"]);
      this.chkApplyLTToAllLoans.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.LoanTradeUpdateAllLoans"]);
      this.chkApplyImmedLTToAllLoans.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.LoanTradeApplyImmediately"]);
      this.chkApplyImmedMBSToAllLoans.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.MBSApplyImmediately"]);
      this.ckboxAllowPublishEvent.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.AllowPublishEvent"]);
      this.ckboxAllowPublishEvent.CheckedChanged += new EventHandler(this.ckboxAllowPublishEvent_CheckedChanged);
      this.ckBox_EnableFMPMandGSE.Enabled = this.ckboxEnableMbsPool.Checked;
      if (this.ckboxEnableCorMaster.Checked)
        this.ckboxEnableAutoCreation.Checked = true;
      if (this.ckboxEnableCommitmentNumber.Checked)
      {
        this.txtTradeCommitmentNumber.Text = this.session.StartupInfo.TradeSettings[(object) "Trade.NextCommitmentNumber"].ToString();
        this.tmpCommitmentTradeAutoNumber = this.txtTradeCommitmentNumber.Text;
      }
      else
        this.txtTradeCommitmentNumber.Enabled = false;
      if (this.ckboxEnableMasterNumber.Checked)
      {
        this.txtMasterCommitmentNumber.Text = this.session.StartupInfo.TradeSettings[(object) "Trade.NextMasterCommitmentNumber"].ToString();
        this.tmpCommitmentAutonumber = this.txtMasterCommitmentNumber.Text;
      }
      else
        this.txtMasterCommitmentNumber.Enabled = false;
      if (this.ckboxEnableCorTrade.Checked)
      {
        this.ckboxAllowBestEfforts.Enabled = true;
        this.ckboxEnableCorMaster.Enabled = true;
        this.ckboxAllowPublishEvent.Enabled = true;
        if (this.ckboxEnableCorMaster.Checked)
          this.ckboxEnableAutoCreation.Enabled = false;
        else
          this.ckboxEnableAutoCreation.Enabled = true;
        this.chkEnableTpoTradeManagement.Enabled = true;
      }
      else
      {
        this.ckboxAllowPublishEvent.Enabled = false;
        this.ckboxAllowBestEfforts.Enabled = false;
        this.ckboxEnableCorMaster.Enabled = false;
        this.ckboxEnableAutoCreation.Enabled = false;
        this.chkEnableTpoTradeManagement.Enabled = false;
      }
      this.chkEnableTpoTradeManagement.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.EnableTPOTradeManagement"]);
      this.ckViewCorrTrade.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.ViewCorrTrade"]);
      this.ckViewCorrMasterCom.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.ViewCorrMasterCom"]);
      this.ckLoanEliCorrTrade.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.LoanEliCorrTrade"]);
      this.ckEPPSLoanProgEliPricing.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.EPPSLoanProgEliPricing"]);
      this.ckLoanAssiToCorrTrade.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.LoanAssiToCorrTrade"]);
      this.ckLoanDeleFromCorrTrade.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.LoanDeleFromCorrTrade"]);
      this.ckRequestPairOff.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.RequestPairOff"]);
      this.ckReceiveComConf.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.ReceiveComConf"]);
      this.ClearTpoManagementCheckboxes();
      this.setDirtyFlag(false);
    }

    private bool validateData()
    {
      if (!this.ckboxEnableCorMaster.Checked && !this.ckboxEnableCorTrade.Checked && !this.ckboxEnableTrade.Checked && !this.ckboxEnableMbsPool.Checked)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enable at least one trade management tab.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.ckboxEnableCommitmentNumber.Checked && this.txtTradeCommitmentNumber.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Starting number for auto creating correspondent trade commitment number is required.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.ckboxEnableMasterNumber.Checked && this.txtMasterCommitmentNumber.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Starting number for auto creating correspondent master commitment number is required.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.txtTradeCommitmentNumber.Enabled)
      {
        string s = this.txtTradeCommitmentNumber.Text.Trim();
        if (string.IsNullOrEmpty(s))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Starting number of correspondent trade commitment number is required.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (!double.TryParse(s, out double _) || s.Length > 18)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Starting number of correspondent trade commitment number is invalid. Starting number should be numeric and the maximum length is 18 digits", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
      if (this.txtMasterCommitmentNumber.Enabled)
      {
        string s = this.txtMasterCommitmentNumber.Text.Trim();
        if (string.IsNullOrEmpty(s))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Starting number of correspondent master commitment number is required.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (!double.TryParse(s, out double _) || s.Length > 10)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Starting number of correspondent master commitment number is invalid. Starting number should be numeric and the maximum length is 10 digits", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
      if (this.ckboxEnableMasterNumber.Checked && this.tmpCommitmentAutonumber != "" && this.tmpCommitmentAutonumber != this.txtMasterCommitmentNumber.Text.Trim())
      {
        if (Utils.Dialog((IWin32Window) this, "Changing the starting master commitment number will apply to all new correspondent master commitments created. Are you sure you want to proceed?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
          return false;
        this.tmpCommitmentAutonumber = this.txtMasterCommitmentNumber.Text;
      }
      if (this.ckboxEnableCommitmentNumber.Checked && this.tmpCommitmentTradeAutoNumber != "" && this.tmpCommitmentTradeAutoNumber != this.txtTradeCommitmentNumber.Text.Trim())
      {
        if (Utils.Dialog((IWin32Window) this, "Changing the starting commitment trade number will apply to all new correspondent trades created. Are you sure you want to proceed?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
          return false;
        this.tmpCommitmentTradeAutoNumber = this.txtTradeCommitmentNumber.Text;
      }
      return true;
    }

    public override void Save()
    {
      if (!this.validateData())
        return;
      Session.ConfigurationManager.SetSecondaryFields(this.collectListView(this.listCommitmentType, -1), SecondaryFieldTypes.CommitmentTypeOption);
      Session.ConfigurationManager.SetSecondaryFields(this.collectListView(this.listViewTradeDescription, -1), SecondaryFieldTypes.TradeDescriptionOption);
      Session.ConfigurationManager.SetSecondaryFields(this.collectListView(this.listViewSecurityTerm, -1), SecondaryFieldTypes.SecurityTerm);
      Dictionary<string, object> settings = new Dictionary<string, object>();
      settings.Add("Trade.EnableTrade", (object) this.ckboxEnableTrade.Checked);
      settings.Add("Trade.EnableMbsPool", (object) this.ckboxEnableMbsPool.Checked);
      settings.Add("Trade.EnableCorrespondentTrade", (object) this.ckboxEnableCorTrade.Checked);
      settings.Add("Trade.AllowBestEfforts", (object) this.ckboxAllowBestEfforts.Checked);
      settings.Add("Trade.EnableCorrespondentMaster", (object) this.ckboxEnableCorMaster.Checked);
      settings.Add("Trade.EnableAutoCreationCT", (object) this.ckboxEnableAutoCreation.Checked);
      settings.Add("Trade.EnableFMPMandGSE", (object) this.ckBox_EnableFMPMandGSE.Checked);
      settings.Add("Trade.EnableBidTapeManagement", (object) this.chkBoxBidTapeMgmt.Checked);
      settings.Add("Trade.EnableBidTapeRegistration", (object) this.chkBoxBidTapeRegistration.Checked);
      settings.Add("Trade.EnableAutoCommitmentNumber", (object) this.ckboxEnableCommitmentNumber.Checked);
      if (this.ckboxEnableCommitmentNumber.Checked)
        settings.Add("Trade.NextCommitmentNumber", (object) this.txtTradeCommitmentNumber.Text);
      settings.Add("Trade.EnableAutoMasterCommitmentNumber", (object) this.ckboxEnableMasterNumber.Checked);
      if (this.ckboxEnableMasterNumber.Checked)
        settings.Add("Trade.NextMasterCommitmentNumber", (object) this.txtMasterCommitmentNumber.Text);
      settings.Add("Trade.LoanTradeUpdateAllLoans", (object) this.chkApplyLTToAllLoans.Checked);
      settings.Add("Trade.LoanTradeApplyImmediately", (object) this.chkApplyImmedLTToAllLoans.Checked);
      settings.Add("Trade.MBSApplyImmediately", (object) this.chkApplyImmedMBSToAllLoans.Checked);
      settings.Add("Trade.EnableTPOTradeManagement", (object) this.chkEnableTpoTradeManagement.Checked);
      settings.Add("Trade.ViewCorrTrade", (object) this.ckViewCorrTrade.Checked);
      settings.Add("Trade.ViewCorrMasterCom", (object) this.ckViewCorrMasterCom.Checked);
      settings.Add("Trade.LoanEliCorrTrade", (object) this.ckLoanEliCorrTrade.Checked);
      settings.Add("Trade.EPPSLoanProgEliPricing", (object) this.ckEPPSLoanProgEliPricing.Checked);
      settings.Add("Trade.LoanAssiToCorrTrade", (object) this.ckLoanAssiToCorrTrade.Checked);
      settings.Add("Trade.LoanDeleFromCorrTrade", (object) this.ckLoanDeleFromCorrTrade.Checked);
      settings.Add("Trade.RequestPairOff", (object) this.ckRequestPairOff.Checked);
      settings.Add("Trade.ReceiveComConf", (object) this.ckReceiveComConf.Checked);
      settings.Add("Trade.AllowPublishEvent", (object) this.ckboxAllowPublishEvent.Checked);
      Session.ServerManager.UpdateServerSettings((IDictionary) settings, true, false);
      foreach (string key in settings.Keys)
        Session.StartupInfo.TradeSettings[(object) key] = settings[key];
      Session.ConfigurationManager.SetSecondarySecurityTypes(this.convertSecurityTypes(this.listViewSecurityType.Items));
      Session.ConfigurationManager.SetFannieMaeProductNames(this.convertProductName(this.listViewProductName.Items));
      DataTable table = new DataTable();
      table.Columns.Add("TradeType");
      table.Columns.Add("FieldId");
      table.Columns.Add("Editable");
      table.Columns.Add("SourceFieldDesc");
      table.Columns.Add("ParentFieldId");
      table.Columns.Add("DestinationFieldDesc");
      table.Columns.Add("Value");
      table.Columns.Add("Order");
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvLoanTradeFields.Items)
      {
        DataRow tag = (DataRow) gvItem.Tag;
        tag["Value"] = !gvItem.SubItems[0].CheckBoxVisible || !gvItem.SubItems[0].Checked ? (object) "False" : (object) "True";
        table.ImportRow(tag);
      }
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvMBSPoolTradeFields.Items)
      {
        DataRow tag = (DataRow) gvItem.Tag;
        tag["Value"] = !gvItem.SubItems[0].CheckBoxVisible || !gvItem.SubItems[0].Checked ? (object) "False" : (object) "True";
        table.ImportRow(tag);
      }
      Session.ConfigurationManager.SetLoanSynchronizationFields(table);
      if (this.isAllowPublishChanged)
        Session.ConfigurationManager.ChangeTradeStatus(this.ckboxAllowPublishEvent.Checked);
      this.setDirtyFlag(false);
    }

    private ArrayList collectListView(GridView listview, int excludeMe)
    {
      ArrayList arrayList = new ArrayList();
      for (int nItemIndex = 0; nItemIndex < listview.Items.Count; ++nItemIndex)
      {
        if (nItemIndex != excludeMe)
          arrayList.Add((object) listview.Items[nItemIndex].Text);
      }
      return arrayList;
    }

    private void buttonNew_Click(object sender, EventArgs e)
    {
      StandardIconButton standardIconButton = (StandardIconButton) sender;
      if (standardIconButton == this.stdIconBtnNewCommitment)
        this.addNewOption(this.listCommitmentType, SecondaryFieldTypes.CommitmentTypeOption);
      else if (standardIconButton == this.stdIconBtnNewTradeDescription)
        this.addNewOption(this.listViewTradeDescription, SecondaryFieldTypes.TradeDescriptionOption);
      else if (standardIconButton == this.btnAddSecurityTerm)
        this.addNewOption(this.listViewSecurityTerm, SecondaryFieldTypes.SecurityTerm);
      else if (standardIconButton == this.SecurityType)
      {
        this.addNewSecurityType();
      }
      else
      {
        if (standardIconButton != this.btnAddFannieMaeProductName)
          return;
        this.addNewFannieMaeProductName();
      }
    }

    private void addNewOption(GridView listview, SecondaryFieldTypes type)
    {
      using (SecondaryFieldDatilForm secondaryFieldDatilForm = new SecondaryFieldDatilForm(type, "", this.collectListView(listview, -1)))
      {
        if (secondaryFieldDatilForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        listview.BeginUpdate();
        listview.Items.Add(new GVItem(secondaryFieldDatilForm.NewOption)
        {
          Selected = true
        });
        listview.EndUpdate();
        this.setDirtyFlag(true);
      }
    }

    private void editOption(GridView listview, SecondaryFieldTypes type)
    {
      if (listview.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an option first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (SecondaryFieldDatilForm secondaryFieldDatilForm = new SecondaryFieldDatilForm(type, listview.SelectedItems[0].Text, this.collectListView(listview, listview.SelectedItems[0].Index)))
        {
          if (secondaryFieldDatilForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          listview.BeginUpdate();
          listview.SelectedItems[0].Text = secondaryFieldDatilForm.NewOption;
          listview.EndUpdate();
          this.setDirtyFlag(true);
        }
      }
    }

    private void addNewSecurityType()
    {
      SecurityTypeDetailForm securityTypeDetailForm = new SecurityTypeDetailForm(this.convertSecurityTypes(this.listViewSecurityType.Items), new int?());
      if (securityTypeDetailForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.listViewSecurityType.Items.Add(new GVItem()
      {
        SubItems = {
          new GVSubItem((object) securityTypeDetailForm.SecurityTypeName),
          new GVSubItem((object) securityTypeDetailForm.ProgramType),
          new GVSubItem((object) this.GetTermsDisplayText(securityTypeDetailForm.Term1, securityTypeDetailForm.Term2)),
          new GVSubItem((object) securityTypeDetailForm.Term1),
          new GVSubItem((object) securityTypeDetailForm.Term2)
        }
      });
      this.setDirtyFlag(true);
    }

    private void editSecurityType()
    {
      if (this.listViewSecurityType.SelectedItems.Count == 0)
        return;
      GVItem selectedItem = this.listViewSecurityType.SelectedItems[0];
      SecurityTypeDetailForm securityTypeDetailForm = new SecurityTypeDetailForm(this.convertSecurityTypes(this.listViewSecurityType.Items), new int?(this.listViewSecurityType.SelectedItems[0].Index));
      if (securityTypeDetailForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      selectedItem.SubItems[0].Value = (object) securityTypeDetailForm.SecurityTypeName;
      selectedItem.SubItems[1].Value = (object) securityTypeDetailForm.ProgramType;
      selectedItem.SubItems[2].Value = (object) this.GetTermsDisplayText(securityTypeDetailForm.Term1, securityTypeDetailForm.Term2);
      selectedItem.SubItems[3].Value = (object) securityTypeDetailForm.Term1;
      selectedItem.SubItems[4].Value = (object) securityTypeDetailForm.Term2;
      this.setDirtyFlag(true);
    }

    private void addNewFannieMaeProductName()
    {
      FannieMaeProductNameForm maeProductNameForm = new FannieMaeProductNameForm(this.convertProductName(this.listViewProductName.Items), new int?());
      if (maeProductNameForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.listViewProductName.Items.Add(new GVItem()
      {
        SubItems = {
          new GVSubItem((object) maeProductNameForm.ProductName),
          new GVSubItem((object) maeProductNameForm.DisplayName),
          new GVSubItem((object) maeProductNameForm.Description)
        }
      });
      this.setDirtyFlag(true);
    }

    private void editFannieMaeProductName()
    {
      if (this.listViewProductName.SelectedItems.Count == 0)
        return;
      GVItem selectedItem = this.listViewProductName.SelectedItems[0];
      FannieMaeProductNameForm maeProductNameForm = new FannieMaeProductNameForm(this.convertProductName(this.listViewProductName.Items), new int?(this.listViewProductName.SelectedItems[0].Index));
      if (maeProductNameForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      selectedItem.SubItems[0].Value = (object) maeProductNameForm.ProductName;
      selectedItem.SubItems[1].Value = (object) maeProductNameForm.DisplayName;
      selectedItem.SubItems[2].Value = (object) maeProductNameForm.Description;
      this.setDirtyFlag(true);
    }

    private void buttonUp_Click(object sender, EventArgs e)
    {
      StandardIconButton standardIconButton = (StandardIconButton) sender;
      if (standardIconButton == this.stdIconBtnUpCommitment)
        this.moveOptionUp(this.listCommitmentType, SecondaryFieldTypes.CommitmentTypeOption);
      else if (standardIconButton == this.stdIconBtnUpTradeDescription)
        this.moveOptionUp(this.listViewTradeDescription, SecondaryFieldTypes.TradeDescriptionOption);
      else if (standardIconButton == this.btnUpSecurityTerm)
        this.moveOptionUp(this.listViewSecurityTerm, SecondaryFieldTypes.SecurityTerm);
      else if (standardIconButton == this.stdIconBtnUpSecurityType)
      {
        this.moveSecurityTypeUpDown(true);
      }
      else
      {
        if (standardIconButton != this.btnUpFannieMaeProductName)
          return;
        this.moveFannieMaeProductNameUpDown(true);
      }
    }

    private void moveOptionUp(GridView listview, SecondaryFieldTypes type)
    {
      if (listview.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an option first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (listview.SelectedItems[0].Index == 0)
          return;
        string text = listview.Items[listview.SelectedItems[0].Index - 1].Text;
        listview.Items[listview.SelectedItems[0].Index - 1].Text = listview.SelectedItems[0].Text;
        listview.SelectedItems[0].Text = text;
        listview.Items[listview.SelectedItems[0].Index - 1].Selected = true;
        this.setDirtyFlag(true);
      }
    }

    private void moveSecurityTypeUpDown(bool up)
    {
      if (this.listViewSecurityType.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select an option first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int index = this.listViewSecurityType.SelectedItems[0].Index;
        int num2 = up ? -1 : 1;
        if (up && index == 0 || !up && index == this.listViewSecurityType.Items.Count - 1)
          return;
        this.listViewSecurityType_SelectedIndexChanged_Enabled = false;
        GVItem gvItem = this.listViewSecurityType.Items[index + num2];
        this.listViewSecurityType.Items[index + num2] = this.listViewSecurityType.SelectedItems[0];
        this.listViewSecurityType.Items[index] = gvItem;
        this.listViewSecurityType.Items[index + num2].Selected = true;
        this.listViewSecurityType_SelectedIndexChanged_Enabled = true;
        this.setDirtyFlag(true);
      }
    }

    private void moveFannieMaeProductNameUpDown(bool up)
    {
      if (this.listViewProductName.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please selected an option first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int index = this.listViewProductName.SelectedItems[0].Index;
        int num2 = up ? -1 : 1;
        if (up && index == 0 || !up && index == this.listViewProductName.Items.Count - 1)
          return;
        this.listViewProductName_SelectedIndexChanged_Enabled = false;
        GVItem gvItem = this.listViewProductName.Items[index + num2];
        this.listViewProductName.Items[index + num2] = this.listViewProductName.SelectedItems[0];
        this.listViewProductName.Items[index] = gvItem;
        this.listViewProductName.Items[index + num2].Selected = true;
        this.listViewProductName_SelectedIndexChanged_Enabled = true;
        this.setDirtyFlag(true);
      }
    }

    private void buttonDown_Click(object sender, EventArgs e)
    {
      StandardIconButton standardIconButton = (StandardIconButton) sender;
      if (standardIconButton == this.stdIconBtnDownCommitment)
        this.moveOptionDown(this.listCommitmentType, SecondaryFieldTypes.CommitmentTypeOption);
      else if (standardIconButton == this.stdIconBtnDownTradeDescription)
        this.moveOptionDown(this.listViewTradeDescription, SecondaryFieldTypes.TradeDescriptionOption);
      else if (standardIconButton == this.btnDownSecurityTerm)
        this.moveOptionDown(this.listViewSecurityTerm, SecondaryFieldTypes.SecurityTerm);
      else if (standardIconButton == this.stdIconBtnDownSecurityType)
      {
        this.moveSecurityTypeUpDown(false);
      }
      else
      {
        if (standardIconButton != this.btnDownFannieMaeProductName)
          return;
        this.moveFannieMaeProductNameUpDown(false);
      }
    }

    private void moveOptionDown(GridView listview, SecondaryFieldTypes type)
    {
      if (listview.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an option first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (listview.SelectedItems[0].Index == listview.Items.Count - 1)
          return;
        string text = listview.Items[listview.SelectedItems[0].Index + 1].Text;
        listview.Items[listview.SelectedItems[0].Index + 1].Text = listview.SelectedItems[0].Text;
        listview.SelectedItems[0].Text = text;
        listview.Items[listview.SelectedItems[0].Index + 1].Selected = true;
        this.setDirtyFlag(true);
      }
    }

    private void buttonDelete_Click(object sender, EventArgs e)
    {
      StandardIconButton standardIconButton = (StandardIconButton) sender;
      if (standardIconButton == this.stdIconBtnDeleteCommitment)
        this.deleteOption(this.listCommitmentType, SecondaryFieldTypes.CommitmentTypeOption);
      else if (standardIconButton == this.stdIconBtnDeleteSecurityType)
        this.deleteOption(this.listViewSecurityType, SecondaryFieldTypes.Payouts);
      else if (standardIconButton == this.btnDeleteSecurityTerm)
        this.deleteOption(this.listViewSecurityTerm, SecondaryFieldTypes.SecurityTerm);
      else if (standardIconButton == this.stdIconBtnDeleteTradeDescription)
      {
        this.deleteOption(this.listViewTradeDescription, SecondaryFieldTypes.TradeDescriptionOption);
      }
      else
      {
        if (standardIconButton != this.btnDeleteFannieMaeProductName)
          return;
        this.deleteOption(this.listViewProductName, SecondaryFieldTypes.FannieMaeProductNameValue);
      }
    }

    private void deleteOption(GridView listview, SecondaryFieldTypes type)
    {
      if (listview.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an option first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.setDirtyFlag(true);
        int index = listview.SelectedItems[0].Index;
        listview.Items.Remove(listview.SelectedItems[0]);
        if (listview.Items.Count == 0)
          return;
        if (index + 1 > listview.Items.Count)
          listview.Items[listview.Items.Count - 1].Selected = true;
        else
          listview.Items[index].Selected = true;
      }
    }

    private void SecondaryFieldsSetup_SizeChanged(object sender, EventArgs e)
    {
      this.panel1.Width = this.gcFields.Width - 10;
      this.panel2.Width = this.gcFields.Width - 10;
      this.panel2.Top = this.panel1.Top + 240;
      this.gcCommitmentType.Width = this.gcTradeDescription.Width = this.panel1.Width / 2;
      this.gcSecurityType.Width = this.gcSecurityTerm.Width = this.panel2.Width / 2;
      this.listViewProductName.Width = this.gcFannieMaeProductNameValue.Width - 20;
    }

    private void listCommitmentType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.isSettingsSync)
        return;
      this.stdIconBtnUpCommitment.Enabled = this.stdIconBtnDownCommitment.Enabled = this.stdIconBtnEditCommitment.Enabled = this.listCommitmentType.SelectedItems.Count == 1;
      this.stdIconBtnDeleteCommitment.Enabled = this.listCommitmentType.SelectedItems.Count > 0;
    }

    private void listViewTradeDescription_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.isSettingsSync)
        return;
      this.stdIconBtnUpTradeDescription.Enabled = this.stdIconBtnDownTradeDescription.Enabled = this.stdIconBtnEditTradeDescription.Enabled = this.listViewTradeDescription.SelectedItems.Count == 1;
      this.stdIconBtnDeleteTradeDescription.Enabled = this.listViewTradeDescription.SelectedItems.Count > 0;
    }

    private void listViewSecurityType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.listViewSecurityType_SelectedIndexChanged_Enabled || this.isSettingsSync)
        return;
      this.stdIconBtnUpSecurityType.Enabled = this.stdIconBtnDownSecurityType.Enabled = this.stdIconBtnEditSecurityType.Enabled = this.listViewSecurityType.SelectedItems.Count == 1;
      this.stdIconBtnDeleteSecurityType.Enabled = this.listViewSecurityType.SelectedItems.Count > 0;
    }

    private void listViewSecurityTerm_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.isSettingsSync)
        return;
      this.btnUpSecurityTerm.Enabled = this.btnDownSecurityTerm.Enabled = this.btnEditSecurityTerm.Enabled = this.listViewSecurityTerm.SelectedItems.Count == 1;
      this.btnDeleteSecurityTerm.Enabled = this.listViewSecurityTerm.SelectedItems.Count > 0;
    }

    private void listViewProductName_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.listViewProductName_SelectedIndexChanged_Enabled || this.isSettingsSync)
        return;
      this.btnUpFannieMaeProductName.Enabled = this.btnDownFannieMaeProductName.Enabled = this.btnEditFannieMaeProductName.Enabled = this.listViewProductName.SelectedItems.Count == 1;
      this.btnDeleteFannieMaeProductName.Enabled = this.listViewProductName.SelectedItems.Count > 0;
    }

    private void stdIconBtnEditCommitment_Click(object sender, EventArgs e)
    {
      this.editOption(this.listCommitmentType, SecondaryFieldTypes.CommitmentTypeOption);
    }

    private void stdIconBtnEditTradeDescription_Click(object sender, EventArgs e)
    {
      this.editOption(this.listViewTradeDescription, SecondaryFieldTypes.TradeDescriptionOption);
    }

    private void stdIconBtnEditSecurityType_Click(object sender, EventArgs e)
    {
      this.editSecurityType();
    }

    private void listCommitmentType_DoubleClick(object sender, EventArgs e)
    {
      this.editOption(this.listCommitmentType, SecondaryFieldTypes.CommitmentTypeOption);
    }

    private void listViewTradeDescription_DoubleClick(object sender, EventArgs e)
    {
      this.editOption(this.listViewTradeDescription, SecondaryFieldTypes.TradeDescriptionOption);
    }

    private void listViewSecurityType_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.editSecurityType();
    }

    private void listViewSecurityTerm_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.editOption(this.listViewSecurityTerm, SecondaryFieldTypes.SecurityTerm);
    }

    private void listViewProductName_ItemDoubleClick(object sender, EventArgs e)
    {
      this.editFannieMaeProductName();
    }

    private void ckboxEnableTrade_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.ckboxEnableTrade.Checked)
        this.ckboxEnableMbsPool.Checked = false;
      this.setDirtyFlag(true);
    }

    private void ckboxEnableCorTrade_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.ckboxEnableCorTrade.Checked)
      {
        if (this.session.CorrespondentMasterManager.AreTradesInCorrespondentMasters())
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "To remove this setting, all associated Correspondent Trades must be removed from all Correspondent Masters first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.ckboxEnableCorTrade.Checked = true;
          return;
        }
        this.ckboxAllowBestEfforts.Checked = false;
        this.ckboxAllowPublishEvent.Checked = false;
        this.ckboxEnableCorMaster.Checked = false;
        this.ckboxEnableAutoCreation.Checked = false;
        this.chkEnableTpoTradeManagement.Checked = false;
        this.ckboxAllowPublishEvent.Enabled = false;
        this.ckboxAllowBestEfforts.Enabled = false;
        this.ckboxEnableCorMaster.Enabled = false;
        this.ckboxEnableAutoCreation.Enabled = false;
        this.chkEnableTpoTradeManagement.Enabled = false;
      }
      else
      {
        this.ckboxAllowBestEfforts.Checked = true;
        this.ckboxAllowPublishEvent.Enabled = true;
        this.ckboxAllowBestEfforts.Enabled = true;
        this.ckboxEnableCorMaster.Enabled = true;
        this.ckboxEnableAutoCreation.Enabled = true;
        this.chkEnableTpoTradeManagement.Enabled = true;
      }
      this.ClearTpoManagementCheckboxes();
      this.setDirtyFlag(true);
    }

    private void chkBoxBidTapeMgmt_CheckedChanged(object sender, EventArgs e)
    {
      this.chkBoxBidTapeRegistration.Checked = this.chkBoxBidTapeMgmt.Checked;
      this.setDirtyFlag(true);
    }

    private void chkBoxBidTapeRegistration_CheckedChanged(object sender, EventArgs e)
    {
      this.chkBoxBidTapeMgmt.Checked = this.chkBoxBidTapeRegistration.Checked;
      this.setDirtyFlag(true);
    }

    private void ckboxEnableMbsPool_CheckedChanged(object sender, EventArgs e)
    {
      if (this.ckboxEnableMbsPool.Checked)
      {
        this.ckboxEnableTrade.Checked = true;
        this.ckBox_EnableFMPMandGSE.Enabled = true;
        this.ckBox_EnableFMPMandGSE.Checked = Utils.ParseBoolean(this.session.StartupInfo.TradeSettings[(object) "Trade.EnableFMPMandGSE"]);
      }
      else
      {
        this.ckBox_EnableFMPMandGSE.Checked = false;
        this.ckBox_EnableFMPMandGSE.Enabled = false;
      }
      this.setDirtyFlag(true);
    }

    private void ckboxEnableCorMaster_CheckedChanged(object sender, EventArgs e)
    {
      if (this.ckboxEnableCorMaster.Checked)
      {
        this.ckboxEnableAutoCreation.Checked = true;
        this.ckboxEnableAutoCreation.Enabled = false;
      }
      else
      {
        if (this.session.CorrespondentMasterManager.AreTradesInCorrespondentMasters())
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "To remove this setting, all associated Correspondent Trades must be removed from all Correspondent Masters first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.ckboxEnableCorTrade.Checked = true;
          this.ckboxEnableAutoCreation.Checked = true;
          this.ckboxEnableAutoCreation.Enabled = false;
          this.setDirtyFlag(false);
          return;
        }
        this.ckboxEnableAutoCreation.Checked = false;
        this.ckboxEnableAutoCreation.Enabled = true;
      }
      this.setDirtyFlag(true);
    }

    private void ckboxEnableAutoCreation_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void txtTradeCommitmentNumber_TextChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void txtMasterCommitmentNumber_TextChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void ckboxEnableCommitmentNumber_CheckedChanged(object sender, EventArgs e)
    {
      this.txtTradeCommitmentNumber.Enabled = ((CheckBox) sender).Checked;
      this.setDirtyFlag(true);
    }

    private void ckboxEnableMasterNumber_CheckedChanged(object sender, EventArgs e)
    {
      this.txtMasterCommitmentNumber.Enabled = ((CheckBox) sender).Checked;
      this.setDirtyFlag(true);
    }

    private void btnEditSecurityTerm_Click(object sender, EventArgs e)
    {
      this.editOption(this.listViewSecurityTerm, SecondaryFieldTypes.SecurityTerm);
    }

    public void SetSelectedSecondaryFields(List<string> selectedSecondaryFields)
    {
      for (int index = 0; index < selectedSecondaryFields.Count && selectedSecondaryFields[index].IndexOf("_") >= 0; ++index)
      {
        string str = selectedSecondaryFields[0].Substring(selectedSecondaryFields[0].LastIndexOf("_") + 1);
        if (Convert.ToInt16(str) == (short) 6)
        {
          for (int nItemIndex = 0; nItemIndex < this.listCommitmentType.Items.Count; ++nItemIndex)
          {
            if (this.listCommitmentType.Items[nItemIndex].Text.ToString() + "_" + str == selectedSecondaryFields[index])
            {
              this.listCommitmentType.Items[nItemIndex].Selected = true;
              break;
            }
          }
        }
        if (Convert.ToInt16(str) == (short) 7)
        {
          for (int nItemIndex = 0; nItemIndex < this.listViewTradeDescription.Items.Count; ++nItemIndex)
          {
            if (this.listViewTradeDescription.Items[nItemIndex].Text.ToString() + "_" + str == selectedSecondaryFields[index])
            {
              this.listViewTradeDescription.Items[nItemIndex].Selected = true;
              break;
            }
          }
        }
        if (Convert.ToInt16(str) == (short) 18)
        {
          for (int nItemIndex = 0; nItemIndex < this.listViewSecurityTerm.Items.Count; ++nItemIndex)
          {
            if (this.listViewSecurityTerm.Items[nItemIndex].Text.ToString() + "_" + str == selectedSecondaryFields[index])
            {
              this.listViewSecurityTerm.Items[nItemIndex].Selected = true;
              break;
            }
          }
        }
      }
    }

    public void SetSelectedSecurityTypes(List<string> selectedSecurityTypes)
    {
      for (int index = 0; index < selectedSecurityTypes.Count; ++index)
      {
        for (int nItemIndex = 0; nItemIndex < this.listViewSecurityType.Items.Count; ++nItemIndex)
        {
          if (string.Compare(this.listViewSecurityType.Items[nItemIndex].Text.Trim(), selectedSecurityTypes[index].Trim(), StringComparison.OrdinalIgnoreCase) == 0)
          {
            this.listViewSecurityType.Items[nItemIndex].Selected = true;
            break;
          }
        }
      }
    }

    private void btnEditFannieMaeProductName_Click(object sender, EventArgs e)
    {
      this.editFannieMaeProductName();
    }

    private void ckBox_EnableFMPMandGSE_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void gvLoanTradeFields_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (this.ltUpdate)
        return;
      this.ltUpdate = true;
      this.chkAllLoanTradeGV.Checked = this.gvLoanTradeFields.Items.Where<GVItem>((System.Func<GVItem, bool>) (i => i.SubItems[0].Checked && i.SubItems[0].CheckBoxVisible)).ToList<GVItem>().Count == this.gvLoanTradeFields.Items.Where<GVItem>((System.Func<GVItem, bool>) (i => i.SubItems[0].CheckBoxVisible)).ToList<GVItem>().Count;
      this.ltUpdate = false;
      this.setDirtyFlag(true);
    }

    private void gvMBSPoolTradeFields_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (!e.SubItem.Checked)
        this.mbsUpdate = true;
      this.chkAllMBSGV.Checked = this.gvMBSPoolTradeFields.Items.Where<GVItem>((System.Func<GVItem, bool>) (i => i.SubItems[0].Checked && i.SubItems[0].CheckBoxVisible)).ToList<GVItem>().Count == this.gvMBSPoolTradeFields.Items.Where<GVItem>((System.Func<GVItem, bool>) (i => i.SubItems[0].CheckBoxVisible)).ToList<GVItem>().Count;
      this.mbsUpdate = false;
      this.setDirtyFlag(true);
    }

    private void chkApplyLTToAllLoans_CheckedChanged(object sender, EventArgs e)
    {
      this.chkApplyImmedLTToAllLoans.Enabled = this.chkApplyLTToAllLoans.Checked;
      if (!this.chkApplyLTToAllLoans.Checked)
        this.chkApplyImmedLTToAllLoans.Checked = false;
      this.setDirtyFlag(true);
    }

    private void chkApplyImmedMBSToAllLoans_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void chkApplyImmedLTToAllLoans_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void chkAllLoanTradeGV_CheckedChanged(object sender, EventArgs e)
    {
      if (this.ltUpdate)
        return;
      bool flag = false;
      if (this.chkAllLoanTradeGV.Checked)
        flag = this.ltUpdate = true;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvLoanTradeFields.Items)
        gvItem.SubItems[0].Checked = flag;
      this.ltUpdate = false;
      this.setDirtyFlag(true);
    }

    private void chkAllMBSGV_CheckedChanged(object sender, EventArgs e)
    {
      if (this.mbsUpdate)
        return;
      bool flag = false;
      if (this.chkAllMBSGV.Checked)
        flag = this.mbsUpdate = true;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvMBSPoolTradeFields.Items)
        gvItem.SubItems[0].Checked = flag;
      this.mbsUpdate = false;
      this.setDirtyFlag(true);
    }

    private void ckboxAllowBestEfforts_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void gvTradeFields_SubItemClick(object source, GVSubItemMouseEventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void chkEnableTpoTradeManagement_CheckedChanged(object sender, EventArgs e)
    {
      this.ClearTpoManagementCheckboxes();
      this.setDirtyFlag(true);
    }

    private void ClearTpoManagementCheckboxes()
    {
      if (this.chkEnableTpoTradeManagement.Checked)
      {
        this.ckViewCorrTrade.Enabled = true;
        this.ckViewCorrMasterCom.Enabled = true;
        this.ckLoanEliCorrTrade.Enabled = true;
        if (this.session.StartupInfo.ProductPricingPartner != null && this.session.StartupInfo.ProductPricingPartner.IsEPPS)
        {
          this.ckEPPSLoanProgEliPricing.Enabled = true;
        }
        else
        {
          this.ckEPPSLoanProgEliPricing.Enabled = false;
          this.ckEPPSLoanProgEliPricing.Checked = false;
        }
        this.ckLoanAssiToCorrTrade.Enabled = true;
        this.ckLoanDeleFromCorrTrade.Enabled = true;
        this.ckRequestPairOff.Enabled = true;
        this.ckReceiveComConf.Enabled = true;
      }
      else
      {
        this.ckViewCorrTrade.Enabled = false;
        this.ckViewCorrMasterCom.Enabled = false;
        this.ckLoanEliCorrTrade.Enabled = false;
        this.ckEPPSLoanProgEliPricing.Enabled = false;
        this.ckLoanAssiToCorrTrade.Enabled = false;
        this.ckLoanDeleFromCorrTrade.Enabled = false;
        this.ckRequestPairOff.Enabled = false;
        this.ckReceiveComConf.Enabled = false;
        this.ckViewCorrTrade.Checked = false;
        this.ckViewCorrMasterCom.Checked = false;
        this.ckLoanEliCorrTrade.Checked = false;
        this.ckEPPSLoanProgEliPricing.Checked = false;
        this.ckLoanAssiToCorrTrade.Checked = false;
        this.ckLoanDeleFromCorrTrade.Checked = false;
        this.ckRequestPairOff.Checked = false;
        this.ckReceiveComConf.Checked = false;
      }
      this.ckRequestPairOff.Enabled = false;
      this.ckReceiveComConf.Enabled = false;
      this.setDirtyFlag(true);
    }

    private void CheckBox_CheckedChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void ckboxAllowPublishEvent_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
      this.isAllowPublishChanged = true;
    }
  }
}
