// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.EditCompanyCommitmentControl
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
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class EditCompanyCommitmentControl : UserControl
  {
    private Sessions.Session session;
    private SessionObjects sessionObjects;
    private ExternalOriginatorManagementData externalOrg;
    private IConfigurationManager config;
    private Chart chrBestEfforts;
    private Chart chrMandatory;
    private Dictionary<CorrespondentMasterDeliveryType, Decimal> allocatedCommitments = new Dictionary<CorrespondentMasterDeliveryType, Decimal>();
    private Dictionary<CorrespondentMasterDeliveryType, Decimal> nonAllocatedCommitments = new Dictionary<CorrespondentMasterDeliveryType, Decimal>();
    private Dictionary<CorrespondentMasterDeliveryType, Decimal> outstandingCommitments = new Dictionary<CorrespondentMasterDeliveryType, Decimal>();
    private ExternalOriginatorManagementData parent;
    private bool readOnly;
    private int dataoid;
    private string dataExternalID;
    public bool ValidationFailed;
    private bool isResetClick;
    private IContainer components;
    private GroupContainer groupContainer1;
    private GroupContainer groupContainer2;
    private GradientPanel gradientPanel1;
    private GroupContainer groupContainer3;
    private CheckBox chbBestEfforts;
    private CheckBox chkResetLimitForRatesheetId;
    private BorderPanel borderPanel1;
    private RadioButton rdoUnlimited;
    private RadioButton rdoLimited;
    private TextBox txtMaxAuthority;
    private Label label2;
    private Label label13;
    private GroupContainer groupContainer4;
    private Panel panel1;
    private Panel panel2;
    private CheckBox chbMandatory;
    private GridView gvDelivery;
    private GradientPanel gradientPanel2;
    private Label label3;
    private Panel panel3;
    private StandardIconButton btnReset;
    private StandardIconButton btnSave;
    private GroupContainer groupContainer6;
    private Panel panel4;
    private GroupContainer gcPolicy;
    private Label label8;
    private Label label7;
    private Label label6;
    private Label label5;
    private Label label4;
    private TextBox txtMaxAmount;
    private TextBox txtWarningMsg;
    private Label label9;
    private RadioButton rdoNoLockSubmit;
    private RadioButton rdoNoRestriction;
    private Label label10;
    private Label lblPlus;
    private Label lblMandatoryAvailable;
    private Label lblMandatoryOutstanding;
    private Label lblBestEffortAvailable;
    private Label lblBestEffortOutstanding;
    private Panel pnlBestEfforts;
    private Panel pnlMandatory;
    private RichTextBox richTextBox1;
    private GroupContainer gcPolicyForTrades;
    private RadioButton rdoNoTradeCreation;
    private RadioButton rdoNoRestrictionPolicyTrades;
    private Panel panel5;
    private Label label11;
    private TextBox txtDailyVolumeLimit;
    private Label label1;
    private TolerancePolicyControl BestEffortsTolerancePolicyControl;
    private Panel panel6;
    private GroupContainer groupContainer5;
    private TextBox txtDailyWarningMsg;
    private Label label12;
    private Panel panel7;
    private GroupContainer groupContainer7;
    private RadioButton rdoDailyNoLockSubmit;
    private RadioButton rdoNoDailyRestriction;
    private TolerancePolicyControl MandatoryTolerancePolicyControl;

    public event EventHandler SaveButton_Clicked;

    public EditCompanyCommitmentControl(SessionObjects sessionObjects, int orgID, bool isTPOFlag)
    {
      this.InitializeComponent();
      this.InitializeChart();
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
      this.loadBestEffortCharts();
      this.loadMandatoryCharts();
      this.loadDeliveryTypes();
      Session.TpoHierarchyAccessCache.TryGetValue(orgID, out bool _);
      if (isTPOFlag)
        this.Enabled = false;
      this.setDirty(false);
      this.FormatMessage();
      this.rdoNoRestriction.CheckedChanged += new EventHandler(this.rdoNoRestriction_CheckedChanged);
      this.rdoNoRestrictionPolicyTrades.CheckedChanged += new EventHandler(this.rdoNoRestrictionPolicyTrades_CheckedChanged);
      this.rdoNoDailyRestriction.CheckedChanged += new EventHandler(this.rdoNoDailyRestriction_CheckedChanged);
    }

    public EditCompanyCommitmentControl(Sessions.Session session, int orgID, bool isTPOFlag)
    {
      this.InitializeComponent();
      this.InitializeChart();
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
      this.loadBestEffortCharts();
      this.loadMandatoryCharts();
      this.loadDeliveryTypes();
      bool hierarchyAccess;
      Session.TpoHierarchyAccessCache.TryGetValue(orgID, out hierarchyAccess);
      TPOClientUtils.DisableControls((UserControl) this, hierarchyAccess);
      if (this.readOnly)
      {
        this.groupContainer3.Enabled = false;
        this.groupContainer4.Enabled = false;
        this.panel1.Enabled = false;
        this.gcPolicyForTrades.Enabled = false;
        this.richTextBox1.Enabled = false;
      }
      this.setDirty(false);
      this.FormatMessage();
      this.rdoNoRestriction.CheckedChanged += new EventHandler(this.rdoNoRestriction_CheckedChanged);
      this.rdoNoRestrictionPolicyTrades.CheckedChanged += new EventHandler(this.rdoNoRestrictionPolicyTrades_CheckedChanged);
      this.rdoNoDailyRestriction.CheckedChanged += new EventHandler(this.rdoNoDailyRestriction_CheckedChanged);
    }

    private void FormatMessage()
    {
      this.richTextBox1.SelectionStart = 220;
      this.richTextBox1.SelectionLength = 112;
      this.richTextBox1.SelectionFont = new Font(this.richTextBox1.Font, FontStyle.Bold | FontStyle.Underline);
    }

    private void calculateTotalCommitment()
    {
      foreach (CorrespondentMasterDeliveryType key in (CorrespondentMasterDeliveryType[]) Enum.GetValues(typeof (CorrespondentMasterDeliveryType)))
      {
        Decimal num = 0M;
        if (this.allocatedCommitments.ContainsKey(key))
          num += this.allocatedCommitments[key];
        if (this.nonAllocatedCommitments.ContainsKey(key))
          num += this.nonAllocatedCommitments[key];
        if (this.outstandingCommitments.ContainsKey(key))
          this.outstandingCommitments[key] = num;
        else
          this.outstandingCommitments.Add(key, num);
      }
    }

    private void initialPage()
    {
      this.calculateTotalCommitment();
      this.chbBestEfforts.Checked = this.externalOrg.CommitmentUseBestEffort;
      if (this.externalOrg.CommitmentUseBestEffortLimited)
        this.rdoLimited.Checked = true;
      else
        this.rdoUnlimited.Checked = true;
      this.txtMaxAuthority.Text = this.externalOrg.MaxCommitmentAuthorityDisplayValue;
      this.txtDailyVolumeLimit.Text = this.externalOrg.BestEffortDailyVolumeLimit <= 0M ? string.Empty : this.externalOrg.BestEffortDailyVolumeLimit.ToString("###,###");
      this.chkResetLimitForRatesheetId.Checked = this.externalOrg.ResetLimitForRatesheetId;
      this.BestEffortsTolerancePolicyControl.Policy = this.externalOrg.BestEffortTolerencePolicy;
      this.BestEffortsTolerancePolicyControl.ToleranceAmt = this.externalOrg.BestEffortToleranceAmt;
      this.BestEffortsTolerancePolicyControl.TolerancePct = this.externalOrg.BestEffortTolerancePct;
      this.BestEffortsTolerancePolicyControl.Dirty = new TolerancePolicyControl.dirtyDelegate(this.setDirty);
      this.MandatoryTolerancePolicyControl.Policy = this.externalOrg.MandatoryTolerencePolicy;
      this.MandatoryTolerancePolicyControl.ToleranceAmt = this.externalOrg.MandatoryToleranceAmt;
      this.MandatoryTolerancePolicyControl.TolerancePct = this.externalOrg.MandatoryTolerancePct;
      this.MandatoryTolerancePolicyControl.Dirty = new TolerancePolicyControl.dirtyDelegate(this.setDirty);
      this.rdoDailyNoLockSubmit.Checked = this.externalOrg.BestEfforDailyLimitPolicy == ExternalOriginatorBestEffortDailyLimitPolicy.DontAllowLock;
      this.rdoNoDailyRestriction.Checked = this.externalOrg.BestEfforDailyLimitPolicy == ExternalOriginatorBestEffortDailyLimitPolicy.NoLimit;
      this.txtDailyWarningMsg.Text = this.externalOrg.DailyLimitWarningMsg;
      this.gvDelivery.Enabled = this.chbMandatory.Checked = this.externalOrg.CommitmentMandatory;
      this.txtMaxAmount.ReadOnly = !this.externalOrg.CommitmentMandatory;
      this.txtMaxAmount.Text = this.externalOrg.MaxCommitmentAmountDisplayValue;
      this.gvDelivery.Items[0].Checked = this.externalOrg.IsCommitmentDeliveryIndividual;
      this.gvDelivery.Items[1].Checked = this.externalOrg.IsCommitmentDeliveryBulk;
      this.gvDelivery.Items[2].Checked = this.externalOrg.IsCommitmentDeliveryAOT;
      this.gvDelivery.Items[3].Checked = this.externalOrg.IsCommitmentDeliveryBulkAOT;
      this.gvDelivery.Items[4].Checked = this.externalOrg.IsCommitmentDeliveryLiveTrade;
      this.gvDelivery.Items[5].Checked = this.externalOrg.IsCommitmentDeliveryCoIssue;
      this.gvDelivery.Items[6].Checked = this.externalOrg.IsCommitmentDeliveryForward;
      switch (this.externalOrg.CommitmentPolicy)
      {
        case ExternalOriginatorCommitmentPolicy.NoRestriction:
          this.rdoNoRestriction.Checked = true;
          break;
        case ExternalOriginatorCommitmentPolicy.DontAllowLockorSubmit:
          this.rdoNoLockSubmit.Checked = true;
          break;
      }
      if (this.externalOrg.CommitmentTradePolicy == ExternalOriginatorCommitmentTradePolicy.NoRestriction)
        this.rdoNoRestrictionPolicyTrades.Checked = true;
      else if (this.externalOrg.CommitmentTradePolicy == ExternalOriginatorCommitmentTradePolicy.DontAllowTradeCreation)
        this.rdoNoTradeCreation.Checked = true;
      this.txtWarningMsg.Text = this.externalOrg.CommitmentMessage;
      this.chbBestEfforts_CheckedChanged((object) null, (EventArgs) null);
      this.chbMandatory_CheckedChanged((object) null, (EventArgs) null);
      if (this.chbBestEfforts.Checked)
        this.rdoNoDailyRestriction.Enabled = this.rdoDailyNoLockSubmit.Enabled = true;
      else
        this.rdoNoDailyRestriction.Enabled = this.rdoDailyNoLockSubmit.Enabled = false;
    }

    private void loadBestEffortCharts()
    {
      double result = 0.0;
      double.TryParse(this.txtMaxAuthority.Text.Trim(), out result);
      double committed = Convert.ToDouble(this.outstandingCommitments[CorrespondentMasterDeliveryType.IndividualBestEfforts]);
      double available = result - committed;
      double num1 = result != 0.0 ? Math.Round(Convert.ToDouble(committed / result * 100.0), 2) : 0.0;
      double num2 = result != 0.0 ? Math.Round(Convert.ToDouble(available / result * 100.0), 2) : 0.0;
      if (this.chbBestEfforts.Checked)
      {
        if (!this.rdoUnlimited.Checked)
        {
          this.lblBestEffortOutstanding.Text = "$" + committed.ToString("###,##0") + " (" + num1.ToString() + "%)";
          this.lblBestEffortAvailable.Text = "$" + available.ToString("###,###") + " (" + num2.ToString() + "%)";
          if (committed > result)
            this.lblBestEffortOutstanding.ForeColor = Color.Red;
          else
            this.lblBestEffortOutstanding.ForeColor = Color.Black;
          if (available < 0.0)
            this.lblBestEffortAvailable.ForeColor = Color.Red;
          else
            this.lblBestEffortAvailable.ForeColor = Color.Black;
          this.LoadPieChart(committed, available, this.chrBestEfforts, this.pnlBestEfforts);
        }
        else
        {
          this.lblBestEffortOutstanding.Text = "$" + committed.ToString("###,##0");
          this.lblBestEffortAvailable.Text = "";
          this.lblBestEffortOutstanding.ForeColor = Color.Black;
          this.lblBestEffortAvailable.ForeColor = Color.Black;
          this.LoadPieChart(0.0, 100.0, this.chrBestEfforts, this.pnlBestEfforts);
        }
      }
      else
      {
        this.lblBestEffortOutstanding.Text = "";
        this.lblBestEffortAvailable.Text = "";
        this.LoadPieChart(100.0, 0.0, this.chrBestEfforts, this.pnlBestEfforts);
      }
    }

    private void loadMandatoryCharts()
    {
      double result = 0.0;
      double.TryParse(this.txtMaxAmount.Text.Trim(), out result);
      double committed = 0.0;
      if (this.gvDelivery.Items[0].Checked)
        committed += Convert.ToDouble(this.outstandingCommitments[CorrespondentMasterDeliveryType.IndividualMandatory]);
      if (this.gvDelivery.Items[1].Checked)
        committed += Convert.ToDouble(this.outstandingCommitments[CorrespondentMasterDeliveryType.Bulk]);
      if (this.gvDelivery.Items[2].Checked)
        committed += Convert.ToDouble(this.outstandingCommitments[CorrespondentMasterDeliveryType.AOT]);
      if (this.gvDelivery.Items[3].Checked)
        committed += Convert.ToDouble(this.outstandingCommitments[CorrespondentMasterDeliveryType.BulkAOT]);
      if (this.gvDelivery.Items[4].Checked)
        committed += Convert.ToDouble(this.outstandingCommitments[CorrespondentMasterDeliveryType.LiveTrade]);
      if (this.gvDelivery.Items[5].Checked)
        committed += Convert.ToDouble(this.outstandingCommitments[CorrespondentMasterDeliveryType.CoIssue]);
      if (this.gvDelivery.Items[6].Checked)
        committed += Convert.ToDouble(this.outstandingCommitments[CorrespondentMasterDeliveryType.Forwards]);
      double available = result - committed;
      double num1 = result != 0.0 ? Math.Round(Convert.ToDouble(committed / result * 100.0), 2) : 0.0;
      double num2 = result != 0.0 ? Math.Round(Convert.ToDouble(available / result * 100.0), 2) : 0.0;
      if (this.chbMandatory.Checked)
      {
        this.lblMandatoryOutstanding.Text = "$" + committed.ToString("###,##0") + " (" + num1.ToString() + "%)";
        this.lblMandatoryAvailable.Text = "$" + available.ToString("###,###") + " (" + num2.ToString() + "%)";
        if (committed > result)
          this.lblMandatoryOutstanding.ForeColor = Color.Red;
        else
          this.lblMandatoryOutstanding.ForeColor = Color.Black;
        if (available < 0.0)
          this.lblMandatoryAvailable.ForeColor = Color.Red;
        else
          this.lblMandatoryAvailable.ForeColor = Color.Black;
        this.LoadPieChart(committed, available, this.chrMandatory, this.pnlMandatory);
      }
      else
      {
        this.lblMandatoryOutstanding.Text = "";
        this.lblMandatoryAvailable.Text = "";
        this.LoadPieChart(100.0, 0.0, this.chrMandatory, this.pnlMandatory);
      }
    }

    private void loadDeliveryTypes()
    {
      Decimal result = 0M;
      Decimal.TryParse(this.txtMaxAmount.Text.Trim(), out result);
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDelivery.Items)
      {
        gvItem.SubItems[1].Value = (object) "";
        gvItem.SubItems[2].Value = (object) "";
      }
      Decimal outstandingCommitment;
      if (this.gvDelivery.Items[0].Checked)
      {
        GVSubItem subItem = this.gvDelivery.Items[0].SubItems[1];
        outstandingCommitment = this.outstandingCommitments[CorrespondentMasterDeliveryType.IndividualMandatory];
        string str = "$" + outstandingCommitment.ToString("###,##0");
        subItem.Value = (object) str;
        this.gvDelivery.Items[0].SubItems[2].Value = this.LoadBarChart(result != 0M ? Convert.ToDecimal(this.outstandingCommitments[CorrespondentMasterDeliveryType.IndividualMandatory]) / result * 100M : 0M);
      }
      if (this.gvDelivery.Items[1].Checked)
      {
        GVSubItem subItem = this.gvDelivery.Items[1].SubItems[1];
        outstandingCommitment = this.outstandingCommitments[CorrespondentMasterDeliveryType.Bulk];
        string str = "$" + outstandingCommitment.ToString("###,##0");
        subItem.Value = (object) str;
        this.gvDelivery.Items[1].SubItems[2].Value = this.LoadBarChart(result != 0M ? Convert.ToDecimal(this.outstandingCommitments[CorrespondentMasterDeliveryType.Bulk]) / result * 100M : 0M);
      }
      if (this.gvDelivery.Items[2].Checked)
      {
        GVSubItem subItem = this.gvDelivery.Items[2].SubItems[1];
        outstandingCommitment = this.outstandingCommitments[CorrespondentMasterDeliveryType.AOT];
        string str = "$" + outstandingCommitment.ToString("###,##0");
        subItem.Value = (object) str;
        this.gvDelivery.Items[2].SubItems[2].Value = this.LoadBarChart(result != 0M ? Convert.ToDecimal(this.outstandingCommitments[CorrespondentMasterDeliveryType.AOT]) / result * 100M : 0M);
      }
      if (this.gvDelivery.Items[3].Checked)
      {
        GVSubItem subItem = this.gvDelivery.Items[3].SubItems[1];
        outstandingCommitment = this.outstandingCommitments[CorrespondentMasterDeliveryType.BulkAOT];
        string str = "$" + outstandingCommitment.ToString("###,##0");
        subItem.Value = (object) str;
        this.gvDelivery.Items[3].SubItems[2].Value = this.LoadBarChart(result != 0M ? Convert.ToDecimal(this.outstandingCommitments[CorrespondentMasterDeliveryType.BulkAOT]) / result * 100M : 0M);
      }
      if (this.gvDelivery.Items[4].Checked)
      {
        GVSubItem subItem = this.gvDelivery.Items[4].SubItems[1];
        outstandingCommitment = this.outstandingCommitments[CorrespondentMasterDeliveryType.LiveTrade];
        string str = "$" + outstandingCommitment.ToString("###,##0");
        subItem.Value = (object) str;
        this.gvDelivery.Items[4].SubItems[2].Value = this.LoadBarChart(result != 0M ? Convert.ToDecimal(this.outstandingCommitments[CorrespondentMasterDeliveryType.LiveTrade]) / result * 100M : 0M);
      }
      if (this.gvDelivery.Items[5].Checked)
      {
        GVSubItem subItem = this.gvDelivery.Items[5].SubItems[1];
        outstandingCommitment = this.outstandingCommitments[CorrespondentMasterDeliveryType.CoIssue];
        string str = "$" + outstandingCommitment.ToString("###,##0");
        subItem.Value = (object) str;
        this.gvDelivery.Items[5].SubItems[2].Value = this.LoadBarChart(result != 0M ? Convert.ToDecimal(this.outstandingCommitments[CorrespondentMasterDeliveryType.CoIssue]) / result * 100M : 0M);
      }
      if (!this.gvDelivery.Items[6].Checked)
        return;
      GVSubItem subItem1 = this.gvDelivery.Items[6].SubItems[1];
      outstandingCommitment = this.outstandingCommitments[CorrespondentMasterDeliveryType.Forwards];
      string str1 = "$" + outstandingCommitment.ToString("###,##0");
      subItem1.Value = (object) str1;
      this.gvDelivery.Items[6].SubItems[2].Value = this.LoadBarChart(result != 0M ? Convert.ToDecimal(this.outstandingCommitments[CorrespondentMasterDeliveryType.Forwards]) / result * 100M : 0M);
    }

    public void DisableControls()
    {
      this.btnSave.Visible = false;
      this.btnReset.Visible = false;
    }

    private void groupContainer1_SizeChanged(object sender, EventArgs e)
    {
      this.gcPolicy.Size = new Size((this.panel1.Width - 5) / 2, this.gcPolicy.Height);
    }

    public void PerformSave() => this.btnSave_Click((object) null, (EventArgs) null);

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!this.ValidationFailed)
      {
        this.externalOrg.CommitmentUseBestEffort = this.chbBestEfforts.Checked;
        this.externalOrg.CommitmentUseBestEffortLimited = this.rdoLimited.Checked;
        this.externalOrg.MaxCommitmentAuthority = Utils.ParseDecimal((object) this.txtMaxAuthority.Text.Replace("$", ""), 0M);
        this.externalOrg.CommitmentMandatory = this.chbMandatory.Checked;
        this.externalOrg.MaxCommitmentAmount = Utils.ParseDecimal((object) this.txtMaxAmount.Text.Replace("$", ""), 0M);
        this.externalOrg.IsCommitmentDeliveryIndividual = this.gvDelivery.Items[0].Checked;
        this.externalOrg.IsCommitmentDeliveryBulk = this.gvDelivery.Items[1].Checked;
        this.externalOrg.IsCommitmentDeliveryAOT = this.gvDelivery.Items[2].Checked;
        this.externalOrg.IsCommitmentDeliveryBulkAOT = this.gvDelivery.Items[3].Checked;
        this.externalOrg.IsCommitmentDeliveryLiveTrade = this.gvDelivery.Items[4].Checked;
        this.externalOrg.IsCommitmentDeliveryCoIssue = this.gvDelivery.Items[5].Checked;
        this.externalOrg.IsCommitmentDeliveryForward = this.gvDelivery.Items[6].Checked;
        this.externalOrg.BestEffortDailyVolumeLimit = Utils.ParseDecimal((object) this.txtDailyVolumeLimit.Text.Replace("$", ""), 0M);
        this.externalOrg.BestEffortTolerencePolicy = this.BestEffortsTolerancePolicyControl.Policy;
        this.externalOrg.BestEffortToleranceAmt = this.BestEffortsTolerancePolicyControl.ToleranceAmt;
        this.externalOrg.BestEffortTolerancePct = this.BestEffortsTolerancePolicyControl.TolerancePct;
        this.externalOrg.BestEfforDailyLimitPolicy = !this.rdoDailyNoLockSubmit.Checked ? ExternalOriginatorBestEffortDailyLimitPolicy.NoLimit : ExternalOriginatorBestEffortDailyLimitPolicy.DontAllowLock;
        this.externalOrg.DailyLimitWarningMsg = this.txtDailyWarningMsg.Text;
        this.externalOrg.MandatoryTolerencePolicy = this.MandatoryTolerancePolicyControl.Policy;
        this.externalOrg.MandatoryToleranceAmt = this.MandatoryTolerancePolicyControl.ToleranceAmt;
        this.externalOrg.MandatoryTolerancePct = this.MandatoryTolerancePolicyControl.TolerancePct;
        this.externalOrg.CommitmentPolicy = !this.rdoNoRestriction.Checked ? ExternalOriginatorCommitmentPolicy.DontAllowLockorSubmit : ExternalOriginatorCommitmentPolicy.NoRestriction;
        if (this.rdoNoRestrictionPolicyTrades.Checked)
          this.externalOrg.CommitmentTradePolicy = ExternalOriginatorCommitmentTradePolicy.NoRestriction;
        else if (this.rdoNoTradeCreation.Checked)
          this.externalOrg.CommitmentTradePolicy = ExternalOriginatorCommitmentTradePolicy.DontAllowTradeCreation;
        this.externalOrg.CommitmentMessage = this.txtWarningMsg.Text;
        this.externalOrg.ResetLimitForRatesheetId = this.chkResetLimitForRatesheetId.Checked;
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
      this.isResetClick = true;
      this.initialPage();
      this.setDirty(false);
      this.isResetClick = false;
    }

    private void setDirty(bool dirty) => this.btnSave.Enabled = this.btnReset.Enabled = dirty;

    public bool IsDirty => this.btnSave.Enabled;

    private void dataChanged(object sender, EventArgs e) => this.setDirty(true);

    private void gvDelivery_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.loadDeliveryTypes();
      this.setDirty(true);
    }

    private void txtMaxAuthority_CursorChanged(object sender, EventArgs e)
    {
    }

    private void numericOnly_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) System.Windows.Forms.Keys.Delete) || e.KeyChar.Equals('\b'))
        this.setDirty(true);
      else if (!char.IsNumber(e.KeyChar))
      {
        e.Handled = true;
      }
      else
      {
        e.Handled = false;
        this.setDirty(true);
      }
    }

    private void chbBestEfforts_CheckedChanged(object sender, EventArgs e)
    {
      this.rdoLimited.Enabled = this.rdoUnlimited.Enabled = this.BestEffortsTolerancePolicyControl.Enabled = this.chbBestEfforts.Checked;
      if (!this.chbBestEfforts.Checked || this.rdoUnlimited.Checked)
      {
        this.txtMaxAuthority.ReadOnly = true;
        this.txtMaxAuthority.Text = "";
      }
      else
      {
        this.txtMaxAuthority.ReadOnly = false;
        this.txtMaxAuthority.Text = this.externalOrg.MaxCommitmentAuthorityDisplayValue;
      }
      if (!this.chbBestEfforts.Checked)
      {
        this.txtDailyVolumeLimit.ReadOnly = true;
        this.txtDailyVolumeLimit.Text = "";
        this.BestEffortsTolerancePolicyControl.Policy = ExternalOriginatorCommitmentTolerancePolicy.NoPolicy;
        this.BestEffortsTolerancePolicyControl.TolerancePct = 0M;
        this.BestEffortsTolerancePolicyControl.ToleranceAmt = 0M;
        this.rdoNoDailyRestriction.Enabled = this.rdoDailyNoLockSubmit.Enabled = false;
        this.chkResetLimitForRatesheetId.Checked = false;
        this.chkResetLimitForRatesheetId.Enabled = false;
      }
      else
      {
        this.txtDailyVolumeLimit.ReadOnly = false;
        this.txtDailyVolumeLimit.Text = this.externalOrg.BestEffortDailyVolumeLimit <= 0M ? string.Empty : this.externalOrg.BestEffortDailyVolumeLimit.ToString("###,###");
        this.BestEffortsTolerancePolicyControl.Policy = this.externalOrg.BestEffortTolerencePolicy;
        this.BestEffortsTolerancePolicyControl.TolerancePct = this.externalOrg.BestEffortTolerancePct;
        this.BestEffortsTolerancePolicyControl.ToleranceAmt = this.externalOrg.BestEffortToleranceAmt;
        this.rdoNoDailyRestriction.Enabled = this.rdoDailyNoLockSubmit.Enabled = this.chkResetLimitForRatesheetId.Enabled = true;
        this.chkResetLimitForRatesheetId.Checked = this.externalOrg.ResetLimitForRatesheetId;
      }
      this.loadBestEffortCharts();
      this.setDirty(true);
    }

    private void chbMandatory_CheckedChanged(object sender, EventArgs e)
    {
      this.gvDelivery.Enabled = this.MandatoryTolerancePolicyControl.Enabled = this.chbMandatory.Checked;
      this.txtMaxAmount.ReadOnly = !this.chbMandatory.Checked;
      if (!this.chbMandatory.Checked)
      {
        this.txtMaxAmount.ReadOnly = true;
        this.txtMaxAmount.Text = "";
        this.gvDelivery.Items[0].Checked = this.gvDelivery.Items[1].Checked = this.gvDelivery.Items[2].Checked = this.gvDelivery.Items[3].Checked = this.gvDelivery.Items[4].Checked = this.gvDelivery.Items[5].Checked = this.gvDelivery.Items[6].Checked = false;
        this.MandatoryTolerancePolicyControl.Policy = ExternalOriginatorCommitmentTolerancePolicy.NoPolicy;
        this.MandatoryTolerancePolicyControl.TolerancePct = 0M;
        this.MandatoryTolerancePolicyControl.ToleranceAmt = 0M;
      }
      else
      {
        this.txtMaxAmount.ReadOnly = false;
        this.txtMaxAmount.Text = this.externalOrg.MaxCommitmentAmountDisplayValue;
        this.gvDelivery.Items[0].Checked = this.externalOrg.IsCommitmentDeliveryIndividual;
        this.gvDelivery.Items[1].Checked = this.externalOrg.IsCommitmentDeliveryBulk;
        this.gvDelivery.Items[2].Checked = this.externalOrg.IsCommitmentDeliveryAOT;
        this.gvDelivery.Items[3].Checked = this.externalOrg.IsCommitmentDeliveryBulkAOT;
        this.gvDelivery.Items[4].Checked = this.externalOrg.IsCommitmentDeliveryLiveTrade;
        this.gvDelivery.Items[5].Checked = this.externalOrg.IsCommitmentDeliveryCoIssue;
        this.gvDelivery.Items[6].Checked = this.externalOrg.IsCommitmentDeliveryForward;
        this.MandatoryTolerancePolicyControl.Policy = this.externalOrg.MandatoryTolerencePolicy;
        this.MandatoryTolerancePolicyControl.TolerancePct = this.externalOrg.MandatoryTolerancePct;
        this.MandatoryTolerancePolicyControl.ToleranceAmt = this.externalOrg.MandatoryToleranceAmt;
      }
      this.loadMandatoryCharts();
      this.loadDeliveryTypes();
      this.setDirty(true);
    }

    private void txtMaxAuthority_Leave(object sender, EventArgs e)
    {
      this.loadBestEffortCharts();
      this.setDirty(true);
      if (this.txtMaxAuthority.Text == "")
        return;
      try
      {
        if (Convert.ToDouble(this.txtMaxAuthority.Text) == 0.0)
          return;
        this.txtMaxAuthority.Text = Convert.ToDouble(this.txtMaxAuthority.Text).ToString("###,###");
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtMaxAuthority.Text = "";
        this.txtMaxAuthority.Focus();
      }
    }

    private void txtMaxAuthority_Enter(object sender, EventArgs e)
    {
      if (this.txtMaxAuthority.Text == "" || Convert.ToDouble(this.txtMaxAuthority.Text) == 0.0)
        return;
      this.txtMaxAuthority.Text = Convert.ToDouble(this.txtMaxAuthority.Text).ToString("##");
    }

    private void txtMaxAmount_Leave(object sender, EventArgs e)
    {
      this.loadMandatoryCharts();
      this.loadDeliveryTypes();
      this.setDirty(true);
      if (this.txtMaxAmount.Text == "")
        return;
      try
      {
        if (Convert.ToDouble(this.txtMaxAmount.Text) == 0.0)
          return;
        this.txtMaxAmount.Text = Convert.ToDouble(this.txtMaxAmount.Text).ToString("###,###");
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtMaxAmount.Text = "";
        this.txtMaxAmount.Focus();
      }
    }

    private void txtMaxAmount_Enter(object sender, EventArgs e)
    {
      if (this.txtMaxAmount.Text == "" || Convert.ToDouble(this.txtMaxAmount.Text) == 0.0)
        return;
      this.txtMaxAmount.Text = Convert.ToDouble(this.txtMaxAmount.Text).ToString("##");
    }

    private void chkResetLimitForRatesheetId_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirty(true);
    }

    private void InitializeChart()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ChartArea chartArea1 = new ChartArea();
      this.chrBestEfforts = new Chart();
      this.chrMandatory = new Chart();
      this.chrBestEfforts.BeginInit();
      this.chrMandatory.BeginInit();
      this.SuspendLayout();
      chartArea1.Name = "BestEfforts";
      this.chrBestEfforts.ChartAreas.Add(chartArea1);
      this.chrBestEfforts.Dock = DockStyle.Fill;
      this.chrBestEfforts.Location = new Point(0, 50);
      ChartArea chartArea2 = new ChartArea();
      chartArea2.Name = "Mandatory";
      this.chrMandatory.ChartAreas.Add(chartArea2);
      this.chrMandatory.Dock = DockStyle.Fill;
      this.chrMandatory.Location = new Point(0, 50);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.chrBestEfforts.EndInit();
      this.chrMandatory.EndInit();
      this.ResumeLayout(false);
    }

    private void LoadPieChart(double committed, double available, Chart chart, Panel pnl)
    {
      chart.Series.Clear();
      chart.BackColor = Color.Transparent;
      chart.ChartAreas[0].BackColor = Color.Transparent;
      Series series1 = new Series();
      series1.Name = "series1";
      series1.IsVisibleInLegend = false;
      series1.Color = Color.Green;
      series1.ChartType = SeriesChartType.Pie;
      Series series2 = series1;
      chart.Series.Add(series2);
      series2.Points.Add(committed);
      series2.Points.Add(available);
      if (available >= 0.0)
      {
        series2.Points[0].Color = Color.LightGray;
        series2.Points[1].Color = Color.LimeGreen;
      }
      else
      {
        series2.Points[0].Color = Color.Red;
        series2.Points[1].Color = Color.Red;
      }
      DataPoint point1 = series2.Points[0];
      DataPoint point2 = series2.Points[1];
      chart.Invalidate();
      pnl.Controls.Add((Control) chart);
    }

    private object LoadBarChart(Decimal percent) => (object) new ProgressElement(percent, true);

    public bool DataValidated()
    {
      if (this.rdoLimited.Checked && this.rdoLimited.Enabled && string.IsNullOrEmpty(this.txtMaxAuthority.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The max commitment authority cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtMaxAuthority.Focus();
        return false;
      }
      if (this.rdoLimited.Checked && this.rdoLimited.Enabled)
      {
        Decimal num1 = Convert.ToDecimal(this.txtMaxAuthority.Text);
        Decimal result = 0M;
        Decimal.TryParse(this.txtDailyVolumeLimit.Text.Trim(), out result);
        if (result > num1)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The daily volume limit cannot be greater than the max commitment authority.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.txtDailyVolumeLimit.Focus();
          return false;
        }
      }
      if (this.chbMandatory.Checked && string.IsNullOrEmpty(this.txtMaxAmount.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The max commitment amount cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtMaxAmount.Focus();
        return false;
      }
      int num3 = this.gvDelivery.Items.Select<GVItem, GVItem>((Func<GVItem, GVItem>) (item => item)).Where<GVItem>((Func<GVItem, bool>) (item => item.Checked)).Count<GVItem>();
      if (this.chbMandatory.Checked && num3 == 0)
      {
        int num4 = (int) Utils.Dialog((IWin32Window) this, "At least one delivery type must be enabled.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (string.IsNullOrEmpty(this.txtWarningMsg.Text.Trim()))
      {
        int num5 = (int) Utils.Dialog((IWin32Window) this, "The Restricted Loans - Warning Message is a required field. Please enter the Warning Message to be displayed when a TPO loan submission does not match the allowable Commitment or Delivery Type, or exceeds the Maximum Commitment Authority.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtWarningMsg.Focus();
        return false;
      }
      if (this.BestEffortsTolerancePolicyControl.Policy != ExternalOriginatorCommitmentTolerancePolicy.NoPolicy)
      {
        if (this.BestEffortsTolerancePolicyControl.TolerancePct <= 0M)
        {
          int num6 = (int) Utils.Dialog((IWin32Window) this, "If the Best Efforts Tolerance Policy is set,  You must enter a positive value in tolerance percentage.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.BestEffortsTolerancePolicyControl.SetFocus("TolerencePctLimitTxt");
          return false;
        }
        if (this.BestEffortsTolerancePolicyControl.Policy == ExternalOriginatorCommitmentTolerancePolicy.ConditionalTolerance && this.BestEffortsTolerancePolicyControl.ToleranceAmt <= 0M)
        {
          int num7 = (int) Utils.Dialog((IWin32Window) this, "If the Best Efforts Tolerance Policy is Conditional,  You must enter a positive amount in tolerance amount.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.BestEffortsTolerancePolicyControl.SetFocus("TolerenceDollarLimitTxt");
          return false;
        }
      }
      if (this.MandatoryTolerancePolicyControl.Policy != ExternalOriginatorCommitmentTolerancePolicy.NoPolicy)
      {
        if (this.MandatoryTolerancePolicyControl.TolerancePct <= 0M)
        {
          int num8 = (int) Utils.Dialog((IWin32Window) this, "If the Mandatory Tolerance Policy is set,  You must enter a positive value in tolerance percentage.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.MandatoryTolerancePolicyControl.SetFocus("TolerencePctLimitTxt");
          return false;
        }
        if (this.MandatoryTolerancePolicyControl.Policy == ExternalOriginatorCommitmentTolerancePolicy.ConditionalTolerance && this.MandatoryTolerancePolicyControl.ToleranceAmt <= 0M)
        {
          int num9 = (int) Utils.Dialog((IWin32Window) this, "If the Mandatory Tolerance Policy is Conditional,  You must enter a positive amount in tolerance amount.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.MandatoryTolerancePolicyControl.SetFocus("TolerenceDollarLimitTxt");
          return false;
        }
      }
      return true;
    }

    private void rdoNoDailyRestriction_CheckedChanged(object sender, EventArgs e)
    {
      if (this.rdoNoDailyRestriction.Checked && !this.isResetClick)
      {
        int num = (int) MessageBox.Show("Please Note: Selecting 'No Restrictions' will allow this company to submit loans with no limits specific to this policy.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      this.setDirty(true);
    }

    private void txtDailyWarningMsg_TextChanged(object sender, EventArgs e) => this.setDirty(true);

    private void txtDailyVolumeLimit_TextChanged(object sender, EventArgs e) => this.setDirty(true);

    private void txtDailyVolumeLimit_Enter(object sender, EventArgs e)
    {
      if (this.txtDailyVolumeLimit.Text == "" || Convert.ToDouble(this.txtDailyVolumeLimit.Text) == 0.0)
        return;
      this.txtDailyVolumeLimit.Text = Convert.ToDouble(this.txtDailyVolumeLimit.Text).ToString("##");
    }

    private void txtDailyVolumeLimit_Leave(object sender, EventArgs e)
    {
      if (this.txtDailyVolumeLimit.Text == "")
        return;
      try
      {
        if (Convert.ToDouble(this.txtDailyVolumeLimit.Text) == 0.0)
          return;
        this.txtDailyVolumeLimit.Text = Convert.ToDouble(this.txtDailyVolumeLimit.Text).ToString("###,###");
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtDailyVolumeLimit.Text = "";
        this.txtDailyVolumeLimit.Focus();
      }
    }

    private void rdoNoRestrictionPolicyTrades_CheckedChanged(object sender, EventArgs e)
    {
      if (this.rdoNoRestrictionPolicyTrades.Checked && !this.isResetClick)
      {
        int num = (int) MessageBox.Show("Please Note: Selecting ‘No Restrictions’ will allow this company to create trades with no limits specific to this policy.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      this.setDirty(true);
    }

    private void rdoNoRestriction_CheckedChanged(object sender, EventArgs e)
    {
      if (this.rdoNoRestriction.Checked && !this.isResetClick)
      {
        int num = (int) MessageBox.Show("Please Note: Selecting ‘No Restrictions’ will allow this company to submit loans with no limits specific to this policy.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVItem gvItem1 = new GVItem();
      GVSubItem gvSubItem1 = new GVSubItem();
      GVSubItem gvSubItem2 = new GVSubItem();
      GVSubItem gvSubItem3 = new GVSubItem();
      GVItem gvItem2 = new GVItem();
      GVSubItem gvSubItem4 = new GVSubItem();
      GVSubItem gvSubItem5 = new GVSubItem();
      GVSubItem gvSubItem6 = new GVSubItem();
      GVItem gvItem3 = new GVItem();
      GVSubItem gvSubItem7 = new GVSubItem();
      GVSubItem gvSubItem8 = new GVSubItem();
      GVSubItem gvSubItem9 = new GVSubItem();
      GVItem gvItem4 = new GVItem();
      GVSubItem gvSubItem10 = new GVSubItem();
      GVSubItem gvSubItem11 = new GVSubItem();
      GVSubItem gvSubItem12 = new GVSubItem();
      GVItem gvItem5 = new GVItem();
      GVSubItem gvSubItem13 = new GVSubItem();
      GVSubItem gvSubItem14 = new GVSubItem();
      GVSubItem gvSubItem15 = new GVSubItem();
      GVItem gvItem6 = new GVItem();
      GVSubItem gvSubItem16 = new GVSubItem();
      GVSubItem gvSubItem17 = new GVSubItem();
      GVSubItem gvSubItem18 = new GVSubItem();
      GVItem gvItem7 = new GVItem();
      GVSubItem gvSubItem19 = new GVSubItem();
      GVSubItem gvSubItem20 = new GVSubItem();
      GVSubItem gvSubItem21 = new GVSubItem();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EditCompanyCommitmentControl));
      this.groupContainer1 = new GroupContainer();
      this.groupContainer2 = new GroupContainer();
      this.panel5 = new Panel();
      this.gcPolicyForTrades = new GroupContainer();
      this.rdoNoTradeCreation = new RadioButton();
      this.rdoNoRestrictionPolicyTrades = new RadioButton();
      this.panel1 = new Panel();
      this.groupContainer6 = new GroupContainer();
      this.txtWarningMsg = new TextBox();
      this.label9 = new Label();
      this.panel4 = new Panel();
      this.gcPolicy = new GroupContainer();
      this.rdoNoLockSubmit = new RadioButton();
      this.rdoNoRestriction = new RadioButton();
      this.groupContainer4 = new GroupContainer();
      this.gvDelivery = new GridView();
      this.gradientPanel2 = new GradientPanel();
      this.label3 = new Label();
      this.panel3 = new Panel();
      this.pnlMandatory = new Panel();
      this.lblMandatoryAvailable = new Label();
      this.lblMandatoryOutstanding = new Label();
      this.label10 = new Label();
      this.txtMaxAmount = new TextBox();
      this.label8 = new Label();
      this.label7 = new Label();
      this.label6 = new Label();
      this.chbMandatory = new CheckBox();
      this.panel2 = new Panel();
      this.groupContainer3 = new GroupContainer();
      this.panel6 = new Panel();
      this.groupContainer5 = new GroupContainer();
      this.txtDailyWarningMsg = new TextBox();
      this.label12 = new Label();
      this.panel7 = new Panel();
      this.groupContainer7 = new GroupContainer();
      this.rdoDailyNoLockSubmit = new RadioButton();
      this.rdoNoDailyRestriction = new RadioButton();
      this.pnlBestEfforts = new Panel();
      this.lblBestEffortAvailable = new Label();
      this.lblBestEffortOutstanding = new Label();
      this.label11 = new Label();
      this.lblPlus = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.txtDailyVolumeLimit = new TextBox();
      this.txtMaxAuthority = new TextBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label13 = new Label();
      this.borderPanel1 = new BorderPanel();
      this.rdoUnlimited = new RadioButton();
      this.rdoLimited = new RadioButton();
      this.chbBestEfforts = new CheckBox();
      this.chkResetLimitForRatesheetId = new CheckBox();
      this.btnReset = new StandardIconButton();
      this.btnSave = new StandardIconButton();
      this.gradientPanel1 = new GradientPanel();
      this.richTextBox1 = new RichTextBox();
      this.MandatoryTolerancePolicyControl = new TolerancePolicyControl();
      this.BestEffortsTolerancePolicyControl = new TolerancePolicyControl();
      this.groupContainer1.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.panel5.SuspendLayout();
      this.gcPolicyForTrades.SuspendLayout();
      this.panel1.SuspendLayout();
      this.groupContainer6.SuspendLayout();
      this.gcPolicy.SuspendLayout();
      this.groupContainer4.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.panel3.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.panel6.SuspendLayout();
      this.groupContainer5.SuspendLayout();
      this.groupContainer7.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnReset).BeginInit();
      ((ISupportInitialize) this.btnSave).BeginInit();
      this.gradientPanel1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.groupContainer2);
      this.groupContainer1.Controls.Add((Control) this.btnReset);
      this.groupContainer1.Controls.Add((Control) this.btnSave);
      this.groupContainer1.Controls.Add((Control) this.gradientPanel1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(911, 1087);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Commitments Information";
      this.groupContainer1.SizeChanged += new EventHandler(this.groupContainer1_SizeChanged);
      this.groupContainer2.AutoScroll = true;
      this.groupContainer2.Borders = AnchorStyles.Top;
      this.groupContainer2.Controls.Add((Control) this.panel5);
      this.groupContainer2.Controls.Add((Control) this.panel1);
      this.groupContainer2.Controls.Add((Control) this.groupContainer4);
      this.groupContainer2.Controls.Add((Control) this.panel2);
      this.groupContainer2.Controls.Add((Control) this.groupContainer3);
      this.groupContainer2.Dock = DockStyle.Fill;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(1, 56);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Padding = new Padding(5);
      this.groupContainer2.Size = new Size(909, 1030);
      this.groupContainer2.TabIndex = 1;
      this.groupContainer2.Text = "Commitment Authority";
      this.panel5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.panel5.Controls.Add((Control) this.gcPolicyForTrades);
      this.panel5.Location = new Point(5, 990);
      this.panel5.Name = "panel5";
      this.panel5.Size = new Size(882, 79);
      this.panel5.TabIndex = 3;
      this.gcPolicyForTrades.Controls.Add((Control) this.rdoNoTradeCreation);
      this.gcPolicyForTrades.Controls.Add((Control) this.rdoNoRestrictionPolicyTrades);
      this.gcPolicyForTrades.Dock = DockStyle.Fill;
      this.gcPolicyForTrades.HeaderForeColor = SystemColors.ControlText;
      this.gcPolicyForTrades.Location = new Point(0, 0);
      this.gcPolicyForTrades.Name = "gcPolicyForTrades";
      this.gcPolicyForTrades.Size = new Size(882, 79);
      this.gcPolicyForTrades.TabIndex = 4;
      this.gcPolicyForTrades.Text = "Policy for Trades that exceed Max Commitment Authority";
      this.rdoNoTradeCreation.AutoSize = true;
      this.rdoNoTradeCreation.Location = new Point(15, 55);
      this.rdoNoTradeCreation.Name = "rdoNoTradeCreation";
      this.rdoNoTradeCreation.Size = new Size(149, 17);
      this.rdoNoTradeCreation.TabIndex = 1;
      this.rdoNoTradeCreation.TabStop = true;
      this.rdoNoTradeCreation.Text = "Don't allow Trade creation";
      this.rdoNoTradeCreation.UseVisualStyleBackColor = true;
      this.rdoNoTradeCreation.CheckedChanged += new EventHandler(this.dataChanged);
      this.rdoNoRestrictionPolicyTrades.AutoSize = true;
      this.rdoNoRestrictionPolicyTrades.Location = new Point(15, 33);
      this.rdoNoRestrictionPolicyTrades.Name = "rdoNoRestrictionPolicyTrades";
      this.rdoNoRestrictionPolicyTrades.Size = new Size(97, 17);
      this.rdoNoRestrictionPolicyTrades.TabIndex = 0;
      this.rdoNoRestrictionPolicyTrades.TabStop = true;
      this.rdoNoRestrictionPolicyTrades.Text = "No Restrictions";
      this.rdoNoRestrictionPolicyTrades.UseVisualStyleBackColor = true;
      this.rdoNoRestrictionPolicyTrades.CheckedChanged += new EventHandler(this.dataChanged);
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.panel1.Controls.Add((Control) this.groupContainer6);
      this.panel1.Controls.Add((Control) this.panel4);
      this.panel1.Controls.Add((Control) this.gcPolicy);
      this.panel1.Location = new Point(5, 867);
      this.panel1.Name = "panel1";
      this.panel1.Padding = new Padding(0, 5, 0, 5);
      this.panel1.Size = new Size(882, 126);
      this.panel1.TabIndex = 2;
      this.groupContainer6.Controls.Add((Control) this.txtWarningMsg);
      this.groupContainer6.Controls.Add((Control) this.label9);
      this.groupContainer6.Dock = DockStyle.Fill;
      this.groupContainer6.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer6.Location = new Point(461, 5);
      this.groupContainer6.Name = "groupContainer6";
      this.groupContainer6.Size = new Size(421, 116);
      this.groupContainer6.TabIndex = 2;
      this.groupContainer6.Text = "Restricted Loans";
      this.txtWarningMsg.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtWarningMsg.Location = new Point(114, 34);
      this.txtWarningMsg.MaxLength = 512;
      this.txtWarningMsg.Multiline = true;
      this.txtWarningMsg.Name = "txtWarningMsg";
      this.txtWarningMsg.ScrollBars = ScrollBars.Vertical;
      this.txtWarningMsg.Size = new Size(292, 66);
      this.txtWarningMsg.TabIndex = 1;
      this.txtWarningMsg.TextChanged += new EventHandler(this.dataChanged);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(15, 37);
      this.label9.Name = "label9";
      this.label9.Size = new Size(93, 13);
      this.label9.TabIndex = 0;
      this.label9.Text = "Warning Message";
      this.panel4.Dock = DockStyle.Left;
      this.panel4.Location = new Point(456, 5);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(5, 116);
      this.panel4.TabIndex = 1;
      this.gcPolicy.Controls.Add((Control) this.rdoNoLockSubmit);
      this.gcPolicy.Controls.Add((Control) this.rdoNoRestriction);
      this.gcPolicy.Dock = DockStyle.Left;
      this.gcPolicy.HeaderForeColor = SystemColors.ControlText;
      this.gcPolicy.Location = new Point(0, 5);
      this.gcPolicy.Name = "gcPolicy";
      this.gcPolicy.Size = new Size(456, 116);
      this.gcPolicy.TabIndex = 0;
      this.gcPolicy.Text = "Policy for loans that exceed Max Commitment Authority";
      this.rdoNoLockSubmit.AutoSize = true;
      this.rdoNoLockSubmit.Location = new Point(15, 55);
      this.rdoNoLockSubmit.Name = "rdoNoLockSubmit";
      this.rdoNoLockSubmit.Size = new Size(100, 17);
      this.rdoNoLockSubmit.TabIndex = 1;
      this.rdoNoLockSubmit.TabStop = true;
      this.rdoNoLockSubmit.Text = "Don't allow lock";
      this.rdoNoLockSubmit.UseVisualStyleBackColor = true;
      this.rdoNoLockSubmit.CheckedChanged += new EventHandler(this.dataChanged);
      this.rdoNoRestriction.AutoSize = true;
      this.rdoNoRestriction.Location = new Point(15, 33);
      this.rdoNoRestriction.Name = "rdoNoRestriction";
      this.rdoNoRestriction.Size = new Size(97, 17);
      this.rdoNoRestriction.TabIndex = 0;
      this.rdoNoRestriction.TabStop = true;
      this.rdoNoRestriction.Text = "No Restrictions";
      this.rdoNoRestriction.UseVisualStyleBackColor = true;
      this.rdoNoRestriction.CheckedChanged += new EventHandler(this.dataChanged);
      this.groupContainer4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer4.Controls.Add((Control) this.gvDelivery);
      this.groupContainer4.Controls.Add((Control) this.gradientPanel2);
      this.groupContainer4.Controls.Add((Control) this.panel3);
      this.groupContainer4.Controls.Add((Control) this.chbMandatory);
      this.groupContainer4.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer4.Location = new Point(5, 408);
      this.groupContainer4.Name = "groupContainer4";
      this.groupContainer4.Size = new Size(882, 458);
      this.groupContainer4.TabIndex = 1;
      this.groupContainer4.Text = "    Mandatory";
      this.gvDelivery.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gvDelivery.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Delivery Type";
      gvColumn1.Width = 200;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Outstanding Commitments";
      gvColumn2.Width = 200;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Percentage of Max";
      gvColumn3.Width = 471;
      this.gvDelivery.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvDelivery.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDelivery.ItemHeight = 28;
      gvItem1.BackColor = Color.Empty;
      gvItem1.ForeColor = Color.Empty;
      gvSubItem1.BackColor = Color.Empty;
      gvSubItem1.ForeColor = Color.Empty;
      gvSubItem1.Text = "Individual";
      gvSubItem2.BackColor = Color.Empty;
      gvSubItem2.ForeColor = Color.Empty;
      gvSubItem3.BackColor = Color.Empty;
      gvSubItem3.ForeColor = Color.Empty;
      gvItem1.SubItems.AddRange(new GVSubItem[3]
      {
        gvSubItem1,
        gvSubItem2,
        gvSubItem3
      });
      gvItem2.BackColor = Color.Empty;
      gvItem2.ForeColor = Color.Empty;
      gvSubItem4.BackColor = Color.Empty;
      gvSubItem4.ForeColor = Color.Empty;
      gvSubItem4.Text = "Bulk";
      gvSubItem5.BackColor = Color.Empty;
      gvSubItem5.ForeColor = Color.Empty;
      gvSubItem6.BackColor = Color.Empty;
      gvSubItem6.ForeColor = Color.Empty;
      gvItem2.SubItems.AddRange(new GVSubItem[3]
      {
        gvSubItem4,
        gvSubItem5,
        gvSubItem6
      });
      gvItem3.BackColor = Color.Empty;
      gvItem3.ForeColor = Color.Empty;
      gvSubItem7.BackColor = Color.Empty;
      gvSubItem7.ForeColor = Color.Empty;
      gvSubItem7.Text = "AOT";
      gvSubItem8.BackColor = Color.Empty;
      gvSubItem8.ForeColor = Color.Empty;
      gvSubItem9.BackColor = Color.Empty;
      gvSubItem9.ForeColor = Color.Empty;
      gvItem3.SubItems.AddRange(new GVSubItem[3]
      {
        gvSubItem7,
        gvSubItem8,
        gvSubItem9
      });
      gvItem4.BackColor = Color.Empty;
      gvItem4.ForeColor = Color.Empty;
      gvSubItem10.BackColor = Color.Empty;
      gvSubItem10.ForeColor = Color.Empty;
      gvSubItem10.Text = "Bulk AOT";
      gvSubItem11.BackColor = Color.Empty;
      gvSubItem11.ForeColor = Color.Empty;
      gvSubItem12.BackColor = Color.Empty;
      gvSubItem12.ForeColor = Color.Empty;
      gvItem4.SubItems.AddRange(new GVSubItem[3]
      {
        gvSubItem10,
        gvSubItem11,
        gvSubItem12
      });
      gvItem5.BackColor = Color.Empty;
      gvItem5.ForeColor = Color.Empty;
      gvSubItem13.BackColor = Color.Empty;
      gvSubItem13.ForeColor = Color.Empty;
      gvSubItem13.Text = "Direct Trade";
      gvSubItem14.BackColor = Color.Empty;
      gvSubItem14.ForeColor = Color.Empty;
      gvSubItem15.BackColor = Color.Empty;
      gvSubItem15.ForeColor = Color.Empty;
      gvItem5.SubItems.AddRange(new GVSubItem[3]
      {
        gvSubItem13,
        gvSubItem14,
        gvSubItem15
      });
      gvItem6.BackColor = Color.Empty;
      gvItem6.ForeColor = Color.Empty;
      gvSubItem16.BackColor = Color.Empty;
      gvSubItem16.ForeColor = Color.Empty;
      gvSubItem16.Text = "Co-Issue";
      gvSubItem17.BackColor = Color.Empty;
      gvSubItem17.ForeColor = Color.Empty;
      gvSubItem18.BackColor = Color.Empty;
      gvSubItem18.ForeColor = Color.Empty;
      gvItem6.SubItems.AddRange(new GVSubItem[3]
      {
        gvSubItem16,
        gvSubItem17,
        gvSubItem18
      });
      gvItem7.BackColor = Color.Empty;
      gvItem7.ForeColor = Color.Empty;
      gvSubItem19.BackColor = Color.Empty;
      gvSubItem19.ForeColor = Color.Empty;
      gvSubItem19.Text = "Forward";
      gvSubItem20.BackColor = Color.Empty;
      gvSubItem20.ForeColor = Color.Empty;
      gvSubItem21.BackColor = Color.Empty;
      gvSubItem21.ForeColor = Color.Empty;
      gvItem7.SubItems.AddRange(new GVSubItem[3]
      {
        gvSubItem19,
        gvSubItem20,
        gvSubItem21
      });
      this.gvDelivery.Items.AddRange(new GVItem[7]
      {
        gvItem1,
        gvItem2,
        gvItem3,
        gvItem4,
        gvItem5,
        gvItem6,
        gvItem7
      });
      this.gvDelivery.Location = new Point(7, 226);
      this.gvDelivery.Name = "gvDelivery";
      this.gvDelivery.Size = new Size(871, 228);
      this.gvDelivery.TabIndex = 3;
      this.gvDelivery.SubItemCheck += new GVSubItemEventHandler(this.gvDelivery_SubItemCheck);
      this.gradientPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel2.Borders = AnchorStyles.Top;
      this.gradientPanel2.Controls.Add((Control) this.label3);
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.Location = new Point(1, 193);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(877, 31);
      this.gradientPanel2.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel2.TabIndex = 2;
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Location = new Point(3, 10);
      this.label3.Name = "label3";
      this.label3.Size = new Size(183, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "Choose the Delivery Types Accepted";
      this.panel3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.panel3.Controls.Add((Control) this.MandatoryTolerancePolicyControl);
      this.panel3.Controls.Add((Control) this.pnlMandatory);
      this.panel3.Controls.Add((Control) this.lblMandatoryAvailable);
      this.panel3.Controls.Add((Control) this.lblMandatoryOutstanding);
      this.panel3.Controls.Add((Control) this.label10);
      this.panel3.Controls.Add((Control) this.txtMaxAmount);
      this.panel3.Controls.Add((Control) this.label8);
      this.panel3.Controls.Add((Control) this.label7);
      this.panel3.Controls.Add((Control) this.label6);
      this.panel3.Location = new Point(1, 26);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(877, 165);
      this.panel3.TabIndex = 1;
      this.pnlMandatory.Location = new Point(664, 4);
      this.pnlMandatory.Name = "pnlMandatory";
      this.pnlMandatory.Size = new Size(88, 47);
      this.pnlMandatory.TabIndex = 33;
      this.lblMandatoryAvailable.AutoSize = true;
      this.lblMandatoryAvailable.Location = new Point(521, 23);
      this.lblMandatoryAvailable.Name = "lblMandatoryAvailable";
      this.lblMandatoryAvailable.Size = new Size(110, 13);
      this.lblMandatoryAvailable.TabIndex = 32;
      this.lblMandatoryAvailable.Text = "lblMandatoryAvailable";
      this.lblMandatoryOutstanding.AutoSize = true;
      this.lblMandatoryOutstanding.Location = new Point(295, 23);
      this.lblMandatoryOutstanding.Name = "lblMandatoryOutstanding";
      this.lblMandatoryOutstanding.Size = new Size(124, 13);
      this.lblMandatoryOutstanding.TabIndex = 31;
      this.lblMandatoryOutstanding.Text = "lblMandatoryOutstanding";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(20, 23);
      this.label10.Name = "label10";
      this.label10.Size = new Size(13, 13);
      this.label10.TabIndex = 28;
      this.label10.Text = "$";
      this.txtMaxAmount.Location = new Point(34, 20);
      this.txtMaxAmount.MaxLength = 14;
      this.txtMaxAmount.Name = "txtMaxAmount";
      this.txtMaxAmount.ShortcutsEnabled = false;
      this.txtMaxAmount.Size = new Size(177, 20);
      this.txtMaxAmount.TabIndex = 6;
      this.txtMaxAmount.Enter += new EventHandler(this.txtMaxAmount_Enter);
      this.txtMaxAmount.KeyPress += new KeyPressEventHandler(this.numericOnly_KeyPress);
      this.txtMaxAmount.Leave += new EventHandler(this.txtMaxAmount_Leave);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(521, 4);
      this.label8.Name = "label8";
      this.label8.Size = new Size(89, 13);
      this.label8.TabIndex = 6;
      this.label8.Text = "Available Amount";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(295, 4);
      this.label7.Name = "label7";
      this.label7.Size = new Size(129, 13);
      this.label7.TabIndex = 1;
      this.label7.Text = "Outstanding Commitments";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(34, 4);
      this.label6.Name = "label6";
      this.label6.Size = new Size(126, 13);
      this.label6.TabIndex = 0;
      this.label6.Text = "Max Commitment Amount";
      this.chbMandatory.AutoSize = true;
      this.chbMandatory.Location = new Point(6, 6);
      this.chbMandatory.Name = "chbMandatory";
      this.chbMandatory.Size = new Size(15, 14);
      this.chbMandatory.TabIndex = 0;
      this.chbMandatory.UseVisualStyleBackColor = true;
      this.chbMandatory.CheckedChanged += new EventHandler(this.chbMandatory_CheckedChanged);
      this.panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.panel2.Location = new Point(5, 408);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(881, 191);
      this.panel2.TabIndex = 3;
      this.groupContainer3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer3.Controls.Add((Control) this.panel6);
      this.groupContainer3.Controls.Add((Control) this.pnlBestEfforts);
      this.groupContainer3.Controls.Add((Control) this.lblBestEffortAvailable);
      this.groupContainer3.Controls.Add((Control) this.lblBestEffortOutstanding);
      this.groupContainer3.Controls.Add((Control) this.label11);
      this.groupContainer3.Controls.Add((Control) this.lblPlus);
      this.groupContainer3.Controls.Add((Control) this.label5);
      this.groupContainer3.Controls.Add((Control) this.label4);
      this.groupContainer3.Controls.Add((Control) this.txtDailyVolumeLimit);
      this.groupContainer3.Controls.Add((Control) this.txtMaxAuthority);
      this.groupContainer3.Controls.Add((Control) this.label1);
      this.groupContainer3.Controls.Add((Control) this.label2);
      this.groupContainer3.Controls.Add((Control) this.label13);
      this.groupContainer3.Controls.Add((Control) this.borderPanel1);
      this.groupContainer3.Controls.Add((Control) this.chbBestEfforts);
      this.groupContainer3.Controls.Add((Control) this.chkResetLimitForRatesheetId);
      this.groupContainer3.Controls.Add((Control) this.BestEffortsTolerancePolicyControl);
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(5, 31);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(883, 377);
      this.groupContainer3.TabIndex = 0;
      this.groupContainer3.Text = "    Best Efforts";
      this.panel6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.panel6.Controls.Add((Control) this.groupContainer5);
      this.panel6.Controls.Add((Control) this.panel7);
      this.panel6.Controls.Add((Control) this.groupContainer7);
      this.panel6.Location = new Point(0, 248);
      this.panel6.Name = "panel6";
      this.panel6.Padding = new Padding(0, 5, 0, 5);
      this.panel6.Size = new Size(882, 126);
      this.panel6.TabIndex = 33;
      this.groupContainer5.Controls.Add((Control) this.txtDailyWarningMsg);
      this.groupContainer5.Controls.Add((Control) this.label12);
      this.groupContainer5.Dock = DockStyle.Fill;
      this.groupContainer5.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer5.Location = new Point(461, 5);
      this.groupContainer5.Name = "groupContainer5";
      this.groupContainer5.Size = new Size(421, 116);
      this.groupContainer5.TabIndex = 2;
      this.groupContainer5.Text = "Restricted Loans";
      this.txtDailyWarningMsg.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDailyWarningMsg.Location = new Point(114, 34);
      this.txtDailyWarningMsg.MaxLength = 512;
      this.txtDailyWarningMsg.Multiline = true;
      this.txtDailyWarningMsg.Name = "txtDailyWarningMsg";
      this.txtDailyWarningMsg.ScrollBars = ScrollBars.Vertical;
      this.txtDailyWarningMsg.Size = new Size(293, 66);
      this.txtDailyWarningMsg.TabIndex = 1;
      this.txtDailyWarningMsg.TextChanged += new EventHandler(this.txtDailyWarningMsg_TextChanged);
      this.label12.AutoSize = true;
      this.label12.Location = new Point(15, 37);
      this.label12.Name = "label12";
      this.label12.Size = new Size(93, 13);
      this.label12.TabIndex = 0;
      this.label12.Text = "Warning Message";
      this.panel7.Dock = DockStyle.Left;
      this.panel7.Location = new Point(456, 5);
      this.panel7.Name = "panel7";
      this.panel7.Size = new Size(5, 116);
      this.panel7.TabIndex = 1;
      this.groupContainer7.Controls.Add((Control) this.rdoDailyNoLockSubmit);
      this.groupContainer7.Controls.Add((Control) this.rdoNoDailyRestriction);
      this.groupContainer7.Dock = DockStyle.Left;
      this.groupContainer7.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer7.Location = new Point(0, 5);
      this.groupContainer7.Name = "groupContainer7";
      this.groupContainer7.Size = new Size(456, 116);
      this.groupContainer7.TabIndex = 0;
      this.groupContainer7.Text = "Policy for loans that exceed Daily Volume Limit";
      this.rdoDailyNoLockSubmit.AutoSize = true;
      this.rdoDailyNoLockSubmit.Location = new Point(15, 55);
      this.rdoDailyNoLockSubmit.Name = "rdoDailyNoLockSubmit";
      this.rdoDailyNoLockSubmit.Size = new Size(100, 17);
      this.rdoDailyNoLockSubmit.TabIndex = 1;
      this.rdoDailyNoLockSubmit.TabStop = true;
      this.rdoDailyNoLockSubmit.Text = "Don't allow lock";
      this.rdoDailyNoLockSubmit.UseVisualStyleBackColor = true;
      this.rdoDailyNoLockSubmit.CheckedChanged += new EventHandler(this.dataChanged);
      this.rdoNoDailyRestriction.AutoSize = true;
      this.rdoNoDailyRestriction.Location = new Point(15, 33);
      this.rdoNoDailyRestriction.Name = "rdoNoDailyRestriction";
      this.rdoNoDailyRestriction.Size = new Size(97, 17);
      this.rdoNoDailyRestriction.TabIndex = 0;
      this.rdoNoDailyRestriction.TabStop = true;
      this.rdoNoDailyRestriction.Text = "No Restrictions";
      this.rdoNoDailyRestriction.UseVisualStyleBackColor = true;
      this.rdoNoDailyRestriction.CheckedChanged += new EventHandler(this.dataChanged);
      this.pnlBestEfforts.Location = new Point(665, 61);
      this.pnlBestEfforts.Name = "pnlBestEfforts";
      this.pnlBestEfforts.Size = new Size(88, 47);
      this.pnlBestEfforts.TabIndex = 31;
      this.lblBestEffortAvailable.AutoSize = true;
      this.lblBestEffortAvailable.Location = new Point(522, 84);
      this.lblBestEffortAvailable.Name = "lblBestEffortAvailable";
      this.lblBestEffortAvailable.Size = new Size(106, 13);
      this.lblBestEffortAvailable.TabIndex = 30;
      this.lblBestEffortAvailable.Text = "lblBestEffortAvailable";
      this.lblBestEffortOutstanding.AutoSize = true;
      this.lblBestEffortOutstanding.Location = new Point(296, 84);
      this.lblBestEffortOutstanding.Name = "lblBestEffortOutstanding";
      this.lblBestEffortOutstanding.Size = new Size(120, 13);
      this.lblBestEffortOutstanding.TabIndex = 29;
      this.lblBestEffortOutstanding.Text = "lblBestEffortOutstanding";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(21, 123);
      this.label11.Name = "label11";
      this.label11.Size = new Size(13, 13);
      this.label11.TabIndex = 28;
      this.label11.Text = "$";
      this.lblPlus.AutoSize = true;
      this.lblPlus.Location = new Point(21, 84);
      this.lblPlus.Name = "lblPlus";
      this.lblPlus.Size = new Size(13, 13);
      this.lblPlus.TabIndex = 28;
      this.lblPlus.Text = "$";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(521, 65);
      this.label5.Name = "label5";
      this.label5.Size = new Size(89, 13);
      this.label5.TabIndex = 5;
      this.label5.Text = "Available Amount";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(295, 65);
      this.label4.Name = "label4";
      this.label4.Size = new Size(129, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "Outstanding Commitments";
      this.txtDailyVolumeLimit.Location = new Point(35, 120);
      this.txtDailyVolumeLimit.MaxLength = 14;
      this.txtDailyVolumeLimit.Name = "txtDailyVolumeLimit";
      this.txtDailyVolumeLimit.ShortcutsEnabled = false;
      this.txtDailyVolumeLimit.Size = new Size(177, 20);
      this.txtDailyVolumeLimit.TabIndex = 3;
      this.txtDailyVolumeLimit.CursorChanged += new EventHandler(this.txtMaxAuthority_CursorChanged);
      this.txtDailyVolumeLimit.TextChanged += new EventHandler(this.txtDailyVolumeLimit_TextChanged);
      this.txtDailyVolumeLimit.Enter += new EventHandler(this.txtDailyVolumeLimit_Enter);
      this.txtDailyVolumeLimit.KeyPress += new KeyPressEventHandler(this.numericOnly_KeyPress);
      this.txtDailyVolumeLimit.Leave += new EventHandler(this.txtDailyVolumeLimit_Leave);
      this.txtMaxAuthority.Location = new Point(35, 81);
      this.txtMaxAuthority.MaxLength = 14;
      this.txtMaxAuthority.Name = "txtMaxAuthority";
      this.txtMaxAuthority.ShortcutsEnabled = false;
      this.txtMaxAuthority.Size = new Size(177, 20);
      this.txtMaxAuthority.TabIndex = 3;
      this.txtMaxAuthority.CursorChanged += new EventHandler(this.txtMaxAuthority_CursorChanged);
      this.txtMaxAuthority.Enter += new EventHandler(this.txtMaxAuthority_Enter);
      this.txtMaxAuthority.KeyPress += new KeyPressEventHandler(this.numericOnly_KeyPress);
      this.txtMaxAuthority.Leave += new EventHandler(this.txtMaxAuthority_Leave);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(30, 104);
      this.label1.Name = "label1";
      this.label1.Size = new Size(92, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Daily Volume Limit";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(34, 65);
      this.label2.Name = "label2";
      this.label2.Size = new Size(131, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Max Commitment Authority";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(245, 124);
      this.label13.Name = "label13";
      this.label13.Size = new Size(254, 20);
      this.label13.TabIndex = 35;
      this.label13.Text = "Reset Limit with new Ratesheet ID";
      this.chkResetLimitForRatesheetId.AutoSize = true;
      this.chkResetLimitForRatesheetId.Location = new Point(230, 124);
      this.chkResetLimitForRatesheetId.Name = "chkResetLimitForRatesheetID";
      this.chkResetLimitForRatesheetId.Size = new Size(22, 21);
      this.chkResetLimitForRatesheetId.TabIndex = 34;
      this.chkResetLimitForRatesheetId.UseVisualStyleBackColor = true;
      this.chkResetLimitForRatesheetId.CheckedChanged += new EventHandler(this.chkResetLimitForRatesheetId_CheckedChanged);
      this.borderPanel1.Borders = AnchorStyles.Bottom;
      this.borderPanel1.Controls.Add((Control) this.rdoUnlimited);
      this.borderPanel1.Controls.Add((Control) this.rdoLimited);
      this.borderPanel1.Dock = DockStyle.Top;
      this.borderPanel1.Location = new Point(1, 26);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(881, 28);
      this.borderPanel1.TabIndex = 1;
      this.rdoUnlimited.AutoSize = true;
      this.rdoUnlimited.Location = new Point(78, 4);
      this.rdoUnlimited.Name = "rdoUnlimited";
      this.rdoUnlimited.Size = new Size(68, 17);
      this.rdoUnlimited.TabIndex = 1;
      this.rdoUnlimited.TabStop = true;
      this.rdoUnlimited.Text = "Unlimited";
      this.rdoUnlimited.UseVisualStyleBackColor = true;
      this.rdoUnlimited.CheckedChanged += new EventHandler(this.chbBestEfforts_CheckedChanged);
      this.rdoLimited.AutoSize = true;
      this.rdoLimited.Location = new Point(14, 4);
      this.rdoLimited.Name = "rdoLimited";
      this.rdoLimited.Size = new Size(58, 17);
      this.rdoLimited.TabIndex = 0;
      this.rdoLimited.TabStop = true;
      this.rdoLimited.Text = "Limited";
      this.rdoLimited.UseVisualStyleBackColor = true;
      this.rdoLimited.CheckedChanged += new EventHandler(this.chbBestEfforts_CheckedChanged);
      this.chbBestEfforts.AutoSize = true;
      this.chbBestEfforts.Location = new Point(6, 6);
      this.chbBestEfforts.Name = "chbBestEfforts";
      this.chbBestEfforts.Size = new Size(15, 14);
      this.chbBestEfforts.TabIndex = 0;
      this.chbBestEfforts.UseVisualStyleBackColor = true;
      this.chbBestEfforts.CheckedChanged += new EventHandler(this.chbBestEfforts_CheckedChanged);
      this.btnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReset.BackColor = Color.Transparent;
      this.btnReset.Enabled = false;
      this.btnReset.Location = new Point(888, 4);
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
      this.btnSave.Location = new Point(866, 4);
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
      this.gradientPanel1.Size = new Size(909, 30);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel1.TabIndex = 0;
      this.richTextBox1.BackColor = Color.WhiteSmoke;
      this.richTextBox1.BorderStyle = BorderStyle.None;
      this.richTextBox1.Dock = DockStyle.Fill;
      this.richTextBox1.Location = new Point(0, 0);
      this.richTextBox1.Name = "richTextBox1";
      this.richTextBox1.ScrollBars = RichTextBoxScrollBars.None;
      this.richTextBox1.Size = new Size(909, 30);
      this.richTextBox1.TabIndex = 0;
      this.richTextBox1.Text = componentResourceManager.GetString("richTextBox1.Text");
      this.MandatoryTolerancePolicyControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.MandatoryTolerancePolicyControl.Dirty = (TolerancePolicyControl.dirtyDelegate) null;
      this.MandatoryTolerancePolicyControl.Label = "Mandatory";
      this.MandatoryTolerancePolicyControl.Location = new Point(10, 62);
      this.MandatoryTolerancePolicyControl.Name = "MandatoryTolerancePolicyControl";
      this.MandatoryTolerancePolicyControl.Policy = ExternalOriginatorCommitmentTolerancePolicy.NoPolicy;
      this.MandatoryTolerancePolicyControl.Size = new Size(852, 96);
      this.MandatoryTolerancePolicyControl.TabIndex = 34;
      this.MandatoryTolerancePolicyControl.ToleranceAmt = new Decimal(new int[4]);
      this.MandatoryTolerancePolicyControl.TolerancePct = new Decimal(new int[4]);
      this.BestEffortsTolerancePolicyControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.BestEffortsTolerancePolicyControl.Dirty = (TolerancePolicyControl.dirtyDelegate) null;
      this.BestEffortsTolerancePolicyControl.Label = "Best Efforts";
      this.BestEffortsTolerancePolicyControl.Location = new Point(15, 146);
      this.BestEffortsTolerancePolicyControl.Name = "BestEffortsTolerancePolicyControl";
      this.BestEffortsTolerancePolicyControl.Policy = ExternalOriginatorCommitmentTolerancePolicy.NoPolicy;
      this.BestEffortsTolerancePolicyControl.Size = new Size(848, 96);
      this.BestEffortsTolerancePolicyControl.TabIndex = 32;
      this.BestEffortsTolerancePolicyControl.ToleranceAmt = new Decimal(new int[4]);
      this.BestEffortsTolerancePolicyControl.TolerancePct = new Decimal(new int[4]);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (EditCompanyCommitmentControl);
      this.Size = new Size(911, 1087);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer2.ResumeLayout(false);
      this.panel5.ResumeLayout(false);
      this.gcPolicyForTrades.ResumeLayout(false);
      this.gcPolicyForTrades.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.groupContainer6.ResumeLayout(false);
      this.groupContainer6.PerformLayout();
      this.gcPolicy.ResumeLayout(false);
      this.gcPolicy.PerformLayout();
      this.groupContainer4.ResumeLayout(false);
      this.groupContainer4.PerformLayout();
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer3.PerformLayout();
      this.panel6.ResumeLayout(false);
      this.groupContainer5.ResumeLayout(false);
      this.groupContainer5.PerformLayout();
      this.groupContainer7.ResumeLayout(false);
      this.groupContainer7.PerformLayout();
      this.borderPanel1.ResumeLayout(false);
      this.borderPanel1.PerformLayout();
      ((ISupportInitialize) this.btnReset).EndInit();
      ((ISupportInitialize) this.btnSave).EndInit();
      this.gradientPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
