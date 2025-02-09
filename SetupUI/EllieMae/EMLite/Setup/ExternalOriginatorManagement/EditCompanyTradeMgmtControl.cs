// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.EditCompanyTradeMgmtControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement.RestApi;
using EllieMae.EMLite.Trading;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class EditCompanyTradeMgmtControl : UserControl
  {
    private Sessions.Session session;
    private SessionObjects sessionObjects;
    private ExternalOriginatorManagementData externalOrg;
    private IConfigurationManager config;
    private Dictionary<CorrespondentMasterDeliveryType, Decimal> allocatedCommitments = new Dictionary<CorrespondentMasterDeliveryType, Decimal>();
    private Dictionary<CorrespondentMasterDeliveryType, Decimal> nonAllocatedCommitments = new Dictionary<CorrespondentMasterDeliveryType, Decimal>();
    private Dictionary<CorrespondentMasterDeliveryType, Decimal> outstandingCommitments = new Dictionary<CorrespondentMasterDeliveryType, Decimal>();
    private ExternalOriginatorManagementData parent;
    private bool readOnly;
    private int dataoid;
    private string dataExternalID;
    public bool ValidationFailed;
    private bool CmpnyEnableTpoTradeManagement;
    private bool CmpnyViewCorrTrade;
    private bool CmpnyViewCorrMasterCom;
    private bool CmpnyLoanEliCorrTrade;
    private bool CmpnyEPPSLoanProgEliPricing;
    private bool CmpnyLoanAssiToCorrTrade;
    private bool CmpnyLoanDeleFromCorrTrade;
    private bool CmpnyRequestPairOff;
    private bool CmpnyReceiveComConf;
    private IContainer components;
    private GroupContainer groupContainer1;
    private GradientPanel gradientPanel1;
    private StandardIconButton btnReset;
    private StandardIconButton btnSave;
    private RichTextBox richTextBox1;
    private GroupContainer groupContainer3;
    private BorderPanel borderPanel1;
    private Panel panel1;
    private CheckBox checkBoxViewCorrespondentTrade;
    private CheckBox checkBoxViewCorrespondentMasterCommitment;
    private CheckBox checkBoxEnableTradeMgmt;
    private RadioButton rdoCustomSettings;
    private RadioButton rdoUseCompanySettings;
    private CheckBox checkBoxRequestPairOff;
    private CheckBox checkBoxLoanAssignmentToCorrespondentTrade;
    private CheckBox checkBoxLoanEligibilityToCorrespondentTrade;
    private CheckBox checkBoxReceiveCommitmentConfirmation;
    private CheckBox checkBoxLoanDeletionFromCorrepondentTrade;
    private CheckBox checkBoxEPPSLoanProgramEligibilityPricing;

    public event EventHandler SaveButton_Clicked;

    public EditCompanyTradeMgmtControl(SessionObjects sessionObjects, int orgID, bool isTPOFlag)
    {
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.sessionObjects = sessionObjects;
      this.config = this.sessionObjects.ConfigurationManager;
      this.externalOrg = this.config.GetExternalOrganization(false, orgID);
      if (this.externalOrg == null)
        this.externalOrg = new ExternalOriginatorManagementData();
      this.parent = this.config.GetRootOrganisation(false, orgID);
      this.readOnly = this.parent == null || this.parent.oid != orgID;
      if (isTPOFlag)
        this.readOnly = true;
      if (this.parent != null)
      {
        this.dataoid = this.parent.oid;
        this.dataExternalID = this.parent.ExternalID;
      }
      this.allocatedCommitments = sessionObjects.CorrespondentTradeManager.GetOutStandingCommitments(this.dataoid);
      this.nonAllocatedCommitments = sessionObjects.ConfigurationManager.GetNonAllocatedOutstandingCommitments(this.dataExternalID);
      this.initialPage();
      bool hierarchyAccess;
      Session.TpoHierarchyAccessCache.TryGetValue(orgID, out hierarchyAccess);
      TPOClientUtils.DisableControls((UserControl) this, hierarchyAccess);
      if (isTPOFlag)
        this.Enabled = false;
      this.setDirty(false);
    }

    public EditCompanyTradeMgmtControl(Sessions.Session session, int orgID, bool isTPOFlag)
    {
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.session = session;
      this.config = this.session.ConfigurationManager;
      this.externalOrg = this.config.GetExternalOrganization(false, orgID);
      this.parent = this.config.GetRootOrganisation(false, orgID);
      this.readOnly = this.parent == null || this.parent.oid != orgID;
      if (isTPOFlag)
        this.readOnly = true;
      if (this.parent != null)
      {
        this.dataoid = this.parent.oid;
        this.dataExternalID = this.parent.ExternalID;
      }
      this.allocatedCommitments = Session.CorrespondentTradeManager.GetOutStandingCommitments(this.dataoid);
      this.nonAllocatedCommitments = Session.ConfigurationManager.GetNonAllocatedOutstandingCommitments(this.dataExternalID);
      this.initialPage();
      bool hierarchyAccess;
      Session.TpoHierarchyAccessCache.TryGetValue(orgID, out hierarchyAccess);
      TPOClientUtils.DisableControls((UserControl) this, hierarchyAccess);
      if (this.readOnly)
        this.Enabled = false;
      this.setDirty(false);
    }

    private void initialPage()
    {
      this.CmpnyEnableTpoTradeManagement = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.EnableTPOTradeManagement"]);
      this.CmpnyViewCorrTrade = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.ViewCorrTrade"]);
      this.CmpnyViewCorrMasterCom = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.ViewCorrMasterCom"]);
      this.CmpnyLoanEliCorrTrade = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.LoanEliCorrTrade"]);
      this.CmpnyEPPSLoanProgEliPricing = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.EPPSLoanProgEliPricing"]);
      this.CmpnyLoanAssiToCorrTrade = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.LoanAssiToCorrTrade"]);
      this.CmpnyLoanDeleFromCorrTrade = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.LoanDeleFromCorrTrade"]);
      this.CmpnyRequestPairOff = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.RequestPairOff"]);
      this.CmpnyReceiveComConf = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.ReceiveComConf"]);
      this.checkBoxEnableTradeMgmt.Enabled = this.CmpnyEnableTpoTradeManagement;
      this.checkBoxEnableTradeMgmt.Checked = this.CmpnyEnableTpoTradeManagement && this.externalOrg.TradeMgmtEnableTPOTradeManagementForTPOClient;
      if (this.CmpnyEnableTpoTradeManagement && this.externalOrg.TradeMgmtEnableTPOTradeManagementForTPOClient)
      {
        this.rdoUseCompanySettings.Enabled = true;
        this.rdoUseCompanySettings.Checked = this.externalOrg.TradeMgmtUseCompanyTPOTradeManagementSettings;
        this.rdoCustomSettings.Enabled = true;
        this.rdoCustomSettings.Checked = !this.externalOrg.TradeMgmtUseCompanyTPOTradeManagementSettings;
      }
      else
      {
        this.rdoUseCompanySettings.Enabled = false;
        this.rdoUseCompanySettings.Checked = false;
        this.rdoCustomSettings.Enabled = false;
        this.rdoCustomSettings.Checked = false;
      }
      if (this.CmpnyEnableTpoTradeManagement && this.rdoCustomSettings.Checked)
      {
        this.checkBoxViewCorrespondentTrade.Enabled = this.CmpnyViewCorrTrade;
        this.checkBoxViewCorrespondentMasterCommitment.Enabled = this.CmpnyViewCorrMasterCom;
        this.checkBoxLoanEligibilityToCorrespondentTrade.Enabled = this.CmpnyLoanEliCorrTrade;
        this.checkBoxEPPSLoanProgramEligibilityPricing.Enabled = this.CmpnyEPPSLoanProgEliPricing;
        this.checkBoxLoanAssignmentToCorrespondentTrade.Enabled = this.CmpnyLoanAssiToCorrTrade;
        this.checkBoxLoanDeletionFromCorrepondentTrade.Enabled = this.CmpnyLoanDeleFromCorrTrade;
        this.checkBoxRequestPairOff.Enabled = this.CmpnyRequestPairOff;
        this.checkBoxReceiveCommitmentConfirmation.Enabled = this.CmpnyReceiveComConf;
        this.checkBoxViewCorrespondentTrade.Checked = this.CmpnyViewCorrTrade && this.externalOrg.TradeMgmtViewCorrespondentTrade;
        this.checkBoxViewCorrespondentMasterCommitment.Checked = this.CmpnyViewCorrMasterCom && this.externalOrg.TradeMgmtViewCorrespondentMasterCommitment;
        this.checkBoxLoanEligibilityToCorrespondentTrade.Checked = this.CmpnyLoanEliCorrTrade && this.externalOrg.TradeMgmtLoanEligibilityToCorrespondentTrade;
        this.checkBoxEPPSLoanProgramEligibilityPricing.Checked = this.CmpnyEPPSLoanProgEliPricing && this.externalOrg.TradeMgmtEPPSLoanProgramEligibilityPricing;
        this.checkBoxLoanAssignmentToCorrespondentTrade.Checked = this.CmpnyLoanAssiToCorrTrade && this.externalOrg.TradeMgmtLoanAssignmentToCorrespondentTrade;
        this.checkBoxLoanDeletionFromCorrepondentTrade.Checked = this.CmpnyLoanDeleFromCorrTrade && this.externalOrg.TradeMgmtLoanDeletionFromCorrespondentTrade;
        this.checkBoxRequestPairOff.Checked = this.CmpnyRequestPairOff && this.externalOrg.TradeMgmtRequestPairOff;
        this.checkBoxReceiveCommitmentConfirmation.Checked = this.CmpnyReceiveComConf && this.externalOrg.TradeMgmtReceiveCommitmentConfirmation;
      }
      else
      {
        this.checkBoxViewCorrespondentTrade.Enabled = false;
        this.checkBoxViewCorrespondentMasterCommitment.Enabled = false;
        this.checkBoxLoanEligibilityToCorrespondentTrade.Enabled = false;
        this.checkBoxEPPSLoanProgramEligibilityPricing.Enabled = false;
        this.checkBoxLoanAssignmentToCorrespondentTrade.Enabled = false;
        this.checkBoxLoanDeletionFromCorrepondentTrade.Enabled = false;
        this.checkBoxRequestPairOff.Enabled = false;
        this.checkBoxReceiveCommitmentConfirmation.Enabled = false;
        this.checkBoxViewCorrespondentTrade.Checked = this.CmpnyViewCorrTrade;
        this.checkBoxViewCorrespondentMasterCommitment.Checked = this.CmpnyViewCorrMasterCom;
        this.checkBoxLoanEligibilityToCorrespondentTrade.Checked = this.CmpnyLoanEliCorrTrade;
        this.checkBoxEPPSLoanProgramEligibilityPricing.Checked = this.CmpnyEPPSLoanProgEliPricing;
        this.checkBoxLoanAssignmentToCorrespondentTrade.Checked = this.CmpnyLoanAssiToCorrTrade;
        this.checkBoxLoanDeletionFromCorrepondentTrade.Checked = this.CmpnyLoanDeleFromCorrTrade;
        this.checkBoxRequestPairOff.Checked = this.CmpnyRequestPairOff;
        this.checkBoxReceiveCommitmentConfirmation.Checked = this.CmpnyReceiveComConf;
      }
    }

    public void DisableControls()
    {
      this.btnSave.Visible = false;
      this.btnReset.Visible = false;
    }

    public void PerformSave() => this.btnSave_Click((object) null, (EventArgs) null);

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!this.ValidationFailed)
      {
        this.externalOrg.TradeMgmtEnableTPOTradeManagementForTPOClient = this.checkBoxEnableTradeMgmt.Checked;
        this.externalOrg.TradeMgmtUseCompanyTPOTradeManagementSettings = this.rdoUseCompanySettings.Checked;
        this.externalOrg.TradeMgmtViewCorrespondentTrade = this.checkBoxViewCorrespondentTrade.Checked;
        this.externalOrg.TradeMgmtViewCorrespondentMasterCommitment = this.checkBoxViewCorrespondentMasterCommitment.Checked;
        this.externalOrg.TradeMgmtLoanEligibilityToCorrespondentTrade = this.checkBoxLoanEligibilityToCorrespondentTrade.Checked;
        this.externalOrg.TradeMgmtEPPSLoanProgramEligibilityPricing = this.checkBoxEPPSLoanProgramEligibilityPricing.Checked;
        this.externalOrg.TradeMgmtLoanAssignmentToCorrespondentTrade = this.checkBoxLoanAssignmentToCorrespondentTrade.Checked;
        this.externalOrg.TradeMgmtLoanDeletionFromCorrespondentTrade = this.checkBoxLoanDeletionFromCorrepondentTrade.Checked;
        this.externalOrg.TradeMgmtRequestPairOff = this.checkBoxRequestPairOff.Checked;
        this.externalOrg.TradeMgmtReceiveCommitmentConfirmation = this.checkBoxReceiveCommitmentConfirmation.Checked;
        this.config.UpdateExternalContact(false, this.externalOrg);
        WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.externalOrg.oid);
        this.initialPage();
        this.setDirty(false);
      }
      if (this.SaveButton_Clicked == null)
        return;
      this.SaveButton_Clicked(sender, e);
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to reset? All changes will be lost.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return;
      this.initialPage();
      this.setDirty(false);
    }

    private void setDirty(bool dirty) => this.btnSave.Enabled = this.btnReset.Enabled = dirty;

    public bool IsDirty => this.btnSave.Enabled;

    private void dataChanged(object sender, EventArgs e) => this.setDirty(true);

    public bool DataValidated() => true;

    private void checkBoxEnableTradeMgmt_CheckedChanged(object sender, EventArgs e)
    {
      this.rdoUseCompanySettings.Enabled = this.checkBoxEnableTradeMgmt.Checked && this.CmpnyEnableTpoTradeManagement;
      this.rdoUseCompanySettings.Checked = this.checkBoxEnableTradeMgmt.Checked;
      this.rdoCustomSettings.Enabled = this.checkBoxEnableTradeMgmt.Checked && this.CmpnyEnableTpoTradeManagement;
      this.rdoCustomSettings.Checked = false;
      this.setDirty(true);
    }

    private void rdoCustomSettings_CheckedChanged(object sender, EventArgs e)
    {
      this.panel1.Enabled = this.rdoCustomSettings.Checked;
      this.checkBoxViewCorrespondentTrade.Enabled = this.CmpnyEnableTpoTradeManagement && this.CmpnyViewCorrTrade && this.rdoCustomSettings.Checked;
      this.checkBoxViewCorrespondentMasterCommitment.Enabled = this.CmpnyEnableTpoTradeManagement && this.CmpnyViewCorrMasterCom && this.rdoCustomSettings.Checked;
      this.checkBoxLoanEligibilityToCorrespondentTrade.Enabled = this.CmpnyEnableTpoTradeManagement && this.CmpnyLoanEliCorrTrade && this.rdoCustomSettings.Checked;
      this.checkBoxEPPSLoanProgramEligibilityPricing.Enabled = this.CmpnyEnableTpoTradeManagement && this.CmpnyEPPSLoanProgEliPricing && this.rdoCustomSettings.Checked;
      this.checkBoxLoanAssignmentToCorrespondentTrade.Enabled = this.CmpnyEnableTpoTradeManagement && this.CmpnyLoanAssiToCorrTrade && this.rdoCustomSettings.Checked;
      this.checkBoxLoanDeletionFromCorrepondentTrade.Enabled = this.CmpnyEnableTpoTradeManagement && this.CmpnyLoanDeleFromCorrTrade && this.rdoCustomSettings.Checked;
      this.checkBoxRequestPairOff.Enabled = this.CmpnyEnableTpoTradeManagement && this.CmpnyRequestPairOff && this.rdoCustomSettings.Checked;
      this.checkBoxReceiveCommitmentConfirmation.Enabled = this.CmpnyEnableTpoTradeManagement && this.CmpnyReceiveComConf && this.rdoCustomSettings.Checked;
      this.checkBoxViewCorrespondentTrade.Checked = this.CmpnyEnableTpoTradeManagement && this.CmpnyViewCorrTrade;
      this.checkBoxViewCorrespondentMasterCommitment.Checked = this.CmpnyEnableTpoTradeManagement && this.CmpnyViewCorrMasterCom;
      this.checkBoxLoanEligibilityToCorrespondentTrade.Checked = this.CmpnyEnableTpoTradeManagement && this.CmpnyLoanEliCorrTrade;
      this.checkBoxEPPSLoanProgramEligibilityPricing.Checked = this.CmpnyEnableTpoTradeManagement && this.CmpnyEPPSLoanProgEliPricing;
      this.checkBoxLoanAssignmentToCorrespondentTrade.Checked = this.CmpnyEnableTpoTradeManagement && this.CmpnyLoanAssiToCorrTrade;
      this.checkBoxLoanDeletionFromCorrepondentTrade.Checked = this.CmpnyEnableTpoTradeManagement && this.CmpnyLoanDeleFromCorrTrade;
      this.checkBoxRequestPairOff.Checked = this.CmpnyEnableTpoTradeManagement && this.CmpnyRequestPairOff;
      this.checkBoxReceiveCommitmentConfirmation.Checked = this.CmpnyEnableTpoTradeManagement && this.CmpnyReceiveComConf;
      this.setDirty(true);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EditCompanyTradeMgmtControl));
      this.groupContainer1 = new GroupContainer();
      this.groupContainer3 = new GroupContainer();
      this.borderPanel1 = new BorderPanel();
      this.panel1 = new Panel();
      this.checkBoxRequestPairOff = new CheckBox();
      this.checkBoxLoanAssignmentToCorrespondentTrade = new CheckBox();
      this.checkBoxLoanEligibilityToCorrespondentTrade = new CheckBox();
      this.checkBoxReceiveCommitmentConfirmation = new CheckBox();
      this.checkBoxViewCorrespondentTrade = new CheckBox();
      this.checkBoxLoanDeletionFromCorrepondentTrade = new CheckBox();
      this.checkBoxEPPSLoanProgramEligibilityPricing = new CheckBox();
      this.checkBoxViewCorrespondentMasterCommitment = new CheckBox();
      this.rdoCustomSettings = new RadioButton();
      this.rdoUseCompanySettings = new RadioButton();
      this.checkBoxEnableTradeMgmt = new CheckBox();
      this.btnReset = new StandardIconButton();
      this.btnSave = new StandardIconButton();
      this.gradientPanel1 = new GradientPanel();
      this.richTextBox1 = new RichTextBox();
      this.groupContainer1.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.btnReset).BeginInit();
      ((ISupportInitialize) this.btnSave).BeginInit();
      this.gradientPanel1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.groupContainer3);
      this.groupContainer1.Controls.Add((Control) this.btnReset);
      this.groupContainer1.Controls.Add((Control) this.btnSave);
      this.groupContainer1.Controls.Add((Control) this.gradientPanel1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(944, 621);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "TPO Client - Trade Management Settings";
      this.groupContainer3.Controls.Add((Control) this.borderPanel1);
      this.groupContainer3.Controls.Add((Control) this.checkBoxEnableTradeMgmt);
      this.groupContainer3.Dock = DockStyle.Top;
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(1, 56);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(942, 243);
      this.groupContainer3.TabIndex = 35;
      this.groupContainer3.Text = "Trade Management Settings";
      this.borderPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.borderPanel1.Borders = AnchorStyles.Bottom;
      this.borderPanel1.Controls.Add((Control) this.panel1);
      this.borderPanel1.Controls.Add((Control) this.rdoCustomSettings);
      this.borderPanel1.Controls.Add((Control) this.rdoUseCompanySettings);
      this.borderPanel1.Location = new Point(13, 52);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(938, 191);
      this.borderPanel1.TabIndex = 1;
      this.panel1.Controls.Add((Control) this.checkBoxRequestPairOff);
      this.panel1.Controls.Add((Control) this.checkBoxLoanAssignmentToCorrespondentTrade);
      this.panel1.Controls.Add((Control) this.checkBoxLoanEligibilityToCorrespondentTrade);
      this.panel1.Controls.Add((Control) this.checkBoxReceiveCommitmentConfirmation);
      this.panel1.Controls.Add((Control) this.checkBoxViewCorrespondentTrade);
      this.panel1.Controls.Add((Control) this.checkBoxLoanDeletionFromCorrepondentTrade);
      this.panel1.Controls.Add((Control) this.checkBoxEPPSLoanProgramEligibilityPricing);
      this.panel1.Controls.Add((Control) this.checkBoxViewCorrespondentMasterCommitment);
      this.panel1.Location = new Point(31, 45);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(262, 142);
      this.panel1.TabIndex = 36;
      this.checkBoxRequestPairOff.AutoSize = true;
      this.checkBoxRequestPairOff.Enabled = false;
      this.checkBoxRequestPairOff.Location = new Point(3, 105);
      this.checkBoxRequestPairOff.Name = "checkBoxRequestPairOff";
      this.checkBoxRequestPairOff.Size = new Size(104, 17);
      this.checkBoxRequestPairOff.TabIndex = 36;
      this.checkBoxRequestPairOff.Text = "Request Pair-Off";
      this.checkBoxRequestPairOff.UseVisualStyleBackColor = true;
      this.checkBoxRequestPairOff.CheckedChanged += new EventHandler(this.dataChanged);
      this.checkBoxLoanAssignmentToCorrespondentTrade.AutoSize = true;
      this.checkBoxLoanAssignmentToCorrespondentTrade.Location = new Point(3, 71);
      this.checkBoxLoanAssignmentToCorrespondentTrade.Name = "checkBoxLoanAssignmentToCorrespondentTrade";
      this.checkBoxLoanAssignmentToCorrespondentTrade.Size = new Size(222, 17);
      this.checkBoxLoanAssignmentToCorrespondentTrade.TabIndex = 36;
      this.checkBoxLoanAssignmentToCorrespondentTrade.Text = "Loan Assignment to Correspondent Trade";
      this.checkBoxLoanAssignmentToCorrespondentTrade.UseVisualStyleBackColor = true;
      this.checkBoxLoanAssignmentToCorrespondentTrade.CheckedChanged += new EventHandler(this.dataChanged);
      this.checkBoxLoanEligibilityToCorrespondentTrade.AutoSize = true;
      this.checkBoxLoanEligibilityToCorrespondentTrade.Location = new Point(3, 37);
      this.checkBoxLoanEligibilityToCorrespondentTrade.Name = "checkBoxLoanEligibilityToCorrespondentTrade";
      this.checkBoxLoanEligibilityToCorrespondentTrade.Size = new Size(207, 17);
      this.checkBoxLoanEligibilityToCorrespondentTrade.TabIndex = 36;
      this.checkBoxLoanEligibilityToCorrespondentTrade.Text = "Loan Eligibility to Correspondent Trade";
      this.checkBoxLoanEligibilityToCorrespondentTrade.UseVisualStyleBackColor = true;
      this.checkBoxLoanEligibilityToCorrespondentTrade.CheckedChanged += new EventHandler(this.dataChanged);
      this.checkBoxReceiveCommitmentConfirmation.AutoSize = true;
      this.checkBoxReceiveCommitmentConfirmation.Location = new Point(3, 122);
      this.checkBoxReceiveCommitmentConfirmation.Name = "checkBoxReceiveCommitmentConfirmation";
      this.checkBoxReceiveCommitmentConfirmation.Size = new Size(187, 17);
      this.checkBoxReceiveCommitmentConfirmation.TabIndex = 36;
      this.checkBoxReceiveCommitmentConfirmation.Text = "Receive Commitment Confirmation";
      this.checkBoxReceiveCommitmentConfirmation.UseVisualStyleBackColor = true;
      this.checkBoxReceiveCommitmentConfirmation.CheckedChanged += new EventHandler(this.dataChanged);
      this.checkBoxViewCorrespondentTrade.AutoSize = true;
      this.checkBoxViewCorrespondentTrade.Location = new Point(3, 3);
      this.checkBoxViewCorrespondentTrade.Name = "checkBoxViewCorrespondentTrade";
      this.checkBoxViewCorrespondentTrade.Size = new Size(152, 17);
      this.checkBoxViewCorrespondentTrade.TabIndex = 36;
      this.checkBoxViewCorrespondentTrade.Text = "View Correspondent Trade";
      this.checkBoxViewCorrespondentTrade.UseVisualStyleBackColor = true;
      this.checkBoxViewCorrespondentTrade.CheckedChanged += new EventHandler(this.dataChanged);
      this.checkBoxLoanDeletionFromCorrepondentTrade.AutoSize = true;
      this.checkBoxLoanDeletionFromCorrepondentTrade.Location = new Point(3, 88);
      this.checkBoxLoanDeletionFromCorrepondentTrade.Name = "checkBoxLoanDeletionFromCorrepondentTrade";
      this.checkBoxLoanDeletionFromCorrepondentTrade.Size = new Size(213, 17);
      this.checkBoxLoanDeletionFromCorrepondentTrade.TabIndex = 36;
      this.checkBoxLoanDeletionFromCorrepondentTrade.Text = "Loan Deletion from Correspondent Trade";
      this.checkBoxLoanDeletionFromCorrepondentTrade.UseVisualStyleBackColor = true;
      this.checkBoxLoanDeletionFromCorrepondentTrade.CheckedChanged += new EventHandler(this.dataChanged);
      this.checkBoxEPPSLoanProgramEligibilityPricing.AutoSize = true;
      this.checkBoxEPPSLoanProgramEligibilityPricing.Location = new Point(3, 54);
      this.checkBoxEPPSLoanProgramEligibilityPricing.Name = "checkBoxEPPSLoanProgramEligibilityPricing";
      this.checkBoxEPPSLoanProgramEligibilityPricing.Size = new Size(202, 17);
      this.checkBoxEPPSLoanProgramEligibilityPricing.TabIndex = 36;
      this.checkBoxEPPSLoanProgramEligibilityPricing.Text = "ICE PPE Loan Program Eligibility/Pricing";
      this.checkBoxEPPSLoanProgramEligibilityPricing.UseVisualStyleBackColor = true;
      this.checkBoxEPPSLoanProgramEligibilityPricing.CheckedChanged += new EventHandler(this.dataChanged);
      this.checkBoxViewCorrespondentMasterCommitment.AutoSize = true;
      this.checkBoxViewCorrespondentMasterCommitment.Location = new Point(3, 20);
      this.checkBoxViewCorrespondentMasterCommitment.Name = "checkBoxViewCorrespondentMasterCommitment";
      this.checkBoxViewCorrespondentMasterCommitment.Size = new Size(216, 17);
      this.checkBoxViewCorrespondentMasterCommitment.TabIndex = 36;
      this.checkBoxViewCorrespondentMasterCommitment.Text = "View Correspondent Master Commitment";
      this.checkBoxViewCorrespondentMasterCommitment.UseVisualStyleBackColor = true;
      this.checkBoxViewCorrespondentMasterCommitment.CheckedChanged += new EventHandler(this.dataChanged);
      this.rdoCustomSettings.AutoSize = true;
      this.rdoCustomSettings.Location = new Point(15, 25);
      this.rdoCustomSettings.Name = "rdoCustomSettings";
      this.rdoCustomSettings.Size = new Size(114, 17);
      this.rdoCustomSettings.TabIndex = 1;
      this.rdoCustomSettings.TabStop = true;
      this.rdoCustomSettings.Text = "Customize Settings";
      this.rdoCustomSettings.UseVisualStyleBackColor = true;
      this.rdoCustomSettings.CheckedChanged += new EventHandler(this.rdoCustomSettings_CheckedChanged);
      this.rdoUseCompanySettings.AutoSize = true;
      this.rdoUseCompanySettings.Location = new Point(15, 3);
      this.rdoUseCompanySettings.Name = "rdoUseCompanySettings";
      this.rdoUseCompanySettings.Size = new Size(284, 17);
      this.rdoUseCompanySettings.TabIndex = 0;
      this.rdoUseCompanySettings.TabStop = true;
      this.rdoUseCompanySettings.Text = "Use Company-level - TPO Trade Management Settings";
      this.rdoUseCompanySettings.UseVisualStyleBackColor = true;
      this.checkBoxEnableTradeMgmt.AutoSize = true;
      this.checkBoxEnableTradeMgmt.Location = new Point(11, 29);
      this.checkBoxEnableTradeMgmt.Name = "checkBoxEnableTradeMgmt";
      this.checkBoxEnableTradeMgmt.Size = new Size(224, 17);
      this.checkBoxEnableTradeMgmt.TabIndex = 36;
      this.checkBoxEnableTradeMgmt.Text = "Enable Trade Management for TPO Client";
      this.checkBoxEnableTradeMgmt.UseVisualStyleBackColor = true;
      this.checkBoxEnableTradeMgmt.CheckedChanged += new EventHandler(this.checkBoxEnableTradeMgmt_CheckedChanged);
      this.btnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReset.BackColor = Color.Transparent;
      this.btnReset.Enabled = false;
      this.btnReset.Location = new Point(921, 4);
      this.btnReset.MouseDownImage = (Image) null;
      this.btnReset.Name = "btnReset";
      this.btnReset.Size = new Size(16, 16);
      this.btnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnReset.TabIndex = 34;
      this.btnReset.TabStop = false;
      this.btnReset.Click += new EventHandler(this.btnReset_Click);
      this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Enabled = false;
      this.btnSave.Location = new Point(899, 4);
      this.btnSave.MouseDownImage = (Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 33;
      this.btnSave.TabStop = false;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.gradientPanel1.Borders = AnchorStyles.None;
      this.gradientPanel1.Controls.Add((Control) this.richTextBox1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(1, 26);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(942, 30);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel1.TabIndex = 0;
      this.richTextBox1.BackColor = Color.WhiteSmoke;
      this.richTextBox1.BorderStyle = BorderStyle.None;
      this.richTextBox1.Dock = DockStyle.Fill;
      this.richTextBox1.Location = new Point(0, 0);
      this.richTextBox1.Name = "richTextBox1";
      this.richTextBox1.ScrollBars = RichTextBoxScrollBars.None;
      this.richTextBox1.Size = new Size(942, 30);
      this.richTextBox1.TabIndex = 0;
      this.richTextBox1.Text = componentResourceManager.GetString("richTextBox1.Text");
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (EditCompanyTradeMgmtControl);
      this.Size = new Size(944, 621);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer3.PerformLayout();
      this.borderPanel1.ResumeLayout(false);
      this.borderPanel1.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.btnReset).EndInit();
      ((ISupportInitialize) this.btnSave).EndInit();
      this.gradientPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
