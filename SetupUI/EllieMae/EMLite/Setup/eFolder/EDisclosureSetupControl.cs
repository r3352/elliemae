// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.EDisclosureSetupControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.eFolder.LoanCenter;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class EDisclosureSetupControl : SettingsUserControl
  {
    private EDisclosureSetup setup;
    private EDisclosureSignOrderSetup signOrderSetup;
    private Sessions.Session session;
    private bool isSettingsSync;
    private IEnumerable<EllieMae.EMLite.Workflow.Milestone> milestoneList;
    private IContainer components;
    private EDisclosureChannelControl retailChannel;
    private EDisclosureChannelControl wholesaleChannel;
    private EDisclosureChannelControl brokerChannel;
    private EDisclosureChannelControl correspondentChannel;
    private Panel pnlSigningConsent;
    private GroupContainer gcConsents;
    private ComboBox cboConsent;
    private Label label2;
    private CheckBox chkBranchAddress;
    private GroupContainer gcComplianceReviewReport;
    private GroupContainer gcSigning;
    private CheckBox chkHELOC;
    private CheckBox chkOther;
    private CheckBox chkUSDA;
    private CheckBox chkVA;
    private CheckBox chkFHA;
    private CheckBox chkConventional;
    private CheckBox chkElectronicSigning;
    private CheckBox chkIncludeComplianceReport;
    private Label lblFulfillmentFee;
    private TextBox txtFulfillmentFee;
    private CheckBox chkFulfillmentFee;
    private Panel pnlPackages;
    private GroupContainer gcPackages;
    private Panel pnlDefaultChannel;
    private ComboBox cboDefaultChannel;
    private Label lblDefaultChannel;
    private TabControl tabPackages;
    private TabPage pageRetail;
    private TabPage pageWholesale;
    private TabPage pageBroker;
    private TabPage pageCorrespondent;
    private CheckBox chkSigningOrderDisclosure;
    private Button btnStates;
    private GroupContainer gcAutoRetrieve;
    private CheckBox chkScanAfterMilestone;
    private CheckBox chkScan;
    private CheckBox chkFaxAfterMilestone;
    private CheckBox chkFax;
    private CheckBox chkDocUploadedWithoutaTaskAfterMilestone;
    private CheckBox chkDocUploadedWithoutaTask;
    private CheckBox chkDocUploadedToCCTaskAfterMilestone;
    private CheckBox chkDocUploadedToCCTask;
    private CheckBox chkeSignedDocumentsAfterMilestone;
    private CheckBox chkeSignedDocuments;
    private Label label6;
    private Label label5;
    private Label label4;
    private Label label3;
    private Label label1;
    private ComboBox cboeSignedDocumentsAfterMilestone;
    private ComboBox cboScanAfterMilestone;
    private ComboBox cboFaxAfterMilestone;
    private ComboBox cboDocUploadedWithoutaTaskAfterMilestone;
    private ComboBox cboDocUploadedToCCTaskAfterMilestone;

    public string[] SelectedMilestoneTemplates
    {
      get
      {
        return this.session.ConfigurationManager.GetMilestoneTemplatesByChannelId(this.SelectedChannelID);
      }
    }

    public string[] SelectedMilestones
    {
      get => this.session.ConfigurationManager.GetMilestoneNamesByChannelId(this.SelectedChannelID);
    }

    public int SelectedChannelID
    {
      get
      {
        switch (this.tabPackages.SelectedTab.Name)
        {
          case "pageRetail":
            return 1;
          case "pageWholesale":
            return 2;
          case "pageBroker":
            return 3;
          case "pageCorrespondent":
            return 4;
          default:
            return 1;
        }
      }
    }

    public EDisclosureSetupControl(SetUpContainer setupContainer)
      : this(setupContainer, Session.DefaultInstance, false)
    {
    }

    public EDisclosureSetupControl(
      SetUpContainer setupContainer,
      Sessions.Session session,
      bool isSettingsSync)
      : base(setupContainer)
    {
      this.session = session;
      this.isSettingsSync = isSettingsSync;
      this.InitializeComponent();
      this.Reset();
    }

    public override void Reset()
    {
      new ConsentServiceClient(this.session).ClientConsentDataGet(AclFeature.SettingsTab_EDisclosurePackages);
      this.setup = this.session.ConfigurationManager.GetEDisclosureSetup();
      this.signOrderSetup = this.session.ConfigurationManager.GetEDisclosureSignOrderSetup();
      this.milestoneList = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllMilestonesList();
      string companySetting1 = this.session.ConfigurationManager.GetCompanySetting("eDisclosures", "FulfillmentFee");
      this.retailChannel.LoadConfiguration(LoanChannel.BankedRetail, this.setup.RetailChannel, this.milestoneList);
      this.wholesaleChannel.LoadConfiguration(LoanChannel.BankedWholesale, this.setup.WholesaleChannel, this.milestoneList);
      this.brokerChannel.LoadConfiguration(LoanChannel.Brokered, this.setup.BrokerChannel, this.milestoneList);
      this.correspondentChannel.LoadConfiguration(LoanChannel.Correspondent, this.setup.CorrespondentChannel, this.milestoneList);
      switch (this.setup.DefaultChannel)
      {
        case LoanChannel.BankedRetail:
          this.cboDefaultChannel.SelectedIndex = 0;
          break;
        case LoanChannel.BankedWholesale:
          this.cboDefaultChannel.SelectedIndex = 1;
          break;
        case LoanChannel.Brokered:
          this.cboDefaultChannel.SelectedIndex = 2;
          break;
        case LoanChannel.Correspondent:
          this.cboDefaultChannel.SelectedIndex = 3;
          break;
        default:
          this.cboDefaultChannel.SelectedIndex = -1;
          break;
      }
      this.chkConventional.Checked = this.setup.AllowESigningConventional;
      this.chkFHA.Checked = this.setup.AllowESigningFHA;
      this.chkVA.Checked = this.setup.AllowESigningVA;
      this.chkUSDA.Checked = this.setup.AllowESigningUSDA;
      this.chkOther.Checked = this.setup.AllowESigningOther;
      this.chkHELOC.Checked = this.setup.AllowESigningHELOC;
      this.chkElectronicSigning.Checked = this.chkConventional.Checked || this.chkFHA.Checked || this.chkVA.Checked || this.chkUSDA.Checked || this.chkOther.Checked || this.chkHELOC.Checked;
      this.chkFulfillmentFee.Checked = !string.IsNullOrEmpty(companySetting1);
      this.txtFulfillmentFee.Text = companySetting1;
      switch (this.setup.ConsentModelType)
      {
        case "Loan level consent":
          this.cboConsent.SelectedIndex = 0;
          break;
        case "Package level consent":
          this.cboConsent.SelectedIndex = 1;
          break;
        default:
          this.cboConsent.SelectedIndex = 0;
          this.session.ConfigurationManager.SaveEDisclosureSetup(this.setup);
          break;
      }
      this.chkBranchAddress.Checked = this.setup.UseBranchAddress;
      this.chkSigningOrderDisclosure.Checked = Convert.ToBoolean(this.signOrderSetup.SignOrderEnabled);
      if (this.session.SessionObjects.StartupInfo.EnableAutoRetrieveSettings)
      {
        this.ESignedDocumentSetup();
        this.DocsUploadedToCCTaskSetup();
        this.DocsUploadedWithoutTaskSetup();
        this.FaxSetup();
        this.ScanSetup();
      }
      else
      {
        this.gcAutoRetrieve.Visible = false;
        this.gcComplianceReviewReport.Width += this.gcAutoRetrieve.Width;
        this.gcSigning.Width += this.gcAutoRetrieve.Width;
        this.gcConsents.Width += this.gcAutoRetrieve.Width;
        this.pnlSigningConsent.AutoScroll = false;
      }
      string companySetting2 = this.session.ConfigurationManager.GetCompanySetting("eDisclosures", "IncludeComplianceReportWithAudit");
      if (this.session.SessionObjects.StartupInfo.OtpSupport)
      {
        this.chkIncludeComplianceReport.Checked = !string.IsNullOrEmpty(companySetting2) && Convert.ToBoolean(companySetting2);
      }
      else
      {
        this.gcComplianceReviewReport.Visible = false;
        this.gcSigning.Top -= this.gcComplianceReviewReport.Height;
        this.gcConsents.Top -= this.gcComplianceReviewReport.Height;
        this.gcConsents.Height = this.gcAutoRetrieve.Height - this.gcSigning.Height;
        this.pnlSigningConsent.Height -= this.gcComplianceReviewReport.Height;
        this.chkIncludeComplianceReport.Checked = false;
        if (!string.IsNullOrEmpty(companySetting2) && companySetting2.ToLower() == "true")
          this.session.ConfigurationManager.SetCompanySetting("eDisclosures", "IncludeComplianceReportWithAudit", "False");
      }
      base.Reset();
    }

    public override void Save()
    {
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) this.validateChannel("Banker-Retail", this.setup.RetailChannel));
      stringList.AddRange((IEnumerable<string>) this.validateChannel("Banker-Wholesale", this.setup.WholesaleChannel));
      stringList.AddRange((IEnumerable<string>) this.validateChannel("Broker", this.setup.BrokerChannel));
      stringList.AddRange((IEnumerable<string>) this.validateChannel("Correspondent", this.setup.CorrespondentChannel));
      if (this.setup.DefaultChannel == LoanChannel.None)
        stringList.Add("You must select a default channel.");
      string empty = string.Empty;
      if (this.chkFulfillmentFee.Checked)
      {
        double num = Utils.ParseDouble((object) this.txtFulfillmentFee.Text);
        if (num > 0.0)
          empty = num.ToString("0.00");
        else
          stringList.Add("You must enter a valid amount for the fulfillment charge.");
      }
      if (stringList.Count > 0)
      {
        string text = "You need to resolve the following issues in order to save:";
        for (int index = 0; index < stringList.Count; ++index)
          text = text + "\n\n(" + Convert.ToString(index + 1) + ") " + stringList[index];
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        this.session.ConfigurationManager.SaveEDisclosureSetup(this.setup);
        this.session.ConfigurationManager.SetCompanySetting("eDisclosures", "FulfillmentFee", empty);
        this.session.ConfigurationManager.SaveEDisclosureSignOrderSetup(this.signOrderSetup);
        this.session.ConfigurationManager.SetCompanySetting("eDisclosures", "IncludeComplianceReportWithAudit", this.chkIncludeComplianceReport.Checked ? "True" : "False");
        if (!this.chkIncludeComplianceReport.Checked)
          this.session.FeaturesAclManager.DisablePermission(AclFeature.eFolder_orderdocs_compliance_failure);
        new ConsentServiceClient(this.session).ClientConsentDataSave();
        this.UpdateeDisclosureExceptions();
        base.Save();
      }
    }

    public void SetSelectedChannelTab(string channelID)
    {
      this.tabPackages.SelectTab(int.Parse(channelID) - 1);
    }

    private void UpdateeDisclosureExceptions()
    {
      this.session.SessionObjects.BpmManager.UpdateMilestoneTemplateEDisclosureExceptions(this.setup);
    }

    private string GetEdisclosureElementAttributeID(
      DataRow[] rows,
      int channelID,
      int channelTypeID)
    {
      return Enumerable.Cast<DataRow>(rows).Where<DataRow>((System.Func<DataRow, bool>) (row => row["ChannelID"].Equals((object) channelID) && row["PackageTypeID"].ToString().Equals(channelTypeID.ToString()))).Select<DataRow, int>((System.Func<DataRow, int>) (row => int.Parse(row["eDisclosureElementAttributeID"].ToString()))).FirstOrDefault<int>().ToString();
    }

    private string[] validateChannel(string channelName, EDisclosureChannel channel)
    {
      List<string> stringList = new List<string>();
      if (!channel.IsBroker && !channel.IsLender)
        stringList.Add("You must select at least one Entity Type for the " + channelName + " channel.");
      if (channel.InitialControl == ControlOptionType.Unknown)
        stringList.Add("You must select a control option for Initial Disclosures for the " + channelName + " channel.");
      if (channel.RedisclosureControl == ControlOptionType.Unknown)
        stringList.Add("You must select a control option for Re-disclosures for the " + channelName + " channel.");
      return stringList.ToArray();
    }

    private void configurationChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void cboDefaultChannel_SelectionChangeCommitted(object sender, EventArgs e)
    {
      switch (this.cboDefaultChannel.SelectedIndex)
      {
        case 0:
          this.setup.DefaultChannel = LoanChannel.BankedRetail;
          break;
        case 1:
          this.setup.DefaultChannel = LoanChannel.BankedWholesale;
          break;
        case 2:
          this.setup.DefaultChannel = LoanChannel.Brokered;
          break;
        case 3:
          this.setup.DefaultChannel = LoanChannel.Correspondent;
          break;
      }
      this.setDirtyFlag(true);
    }

    private void chkIncludeComplianceReport_Click(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void chkElectronicSigning_Click(object sender, EventArgs e)
    {
      this.chkConventional.Checked = this.chkElectronicSigning.Checked;
      this.chkFHA.Checked = this.chkElectronicSigning.Checked;
      this.chkVA.Checked = this.chkElectronicSigning.Checked;
      this.chkUSDA.Checked = this.chkElectronicSigning.Checked;
      this.chkOther.Checked = this.chkElectronicSigning.Checked;
      this.chkHELOC.Checked = this.chkElectronicSigning.Checked;
      this.setup.AllowESigningConventional = this.chkConventional.Checked;
      this.setup.AllowESigningFHA = this.chkFHA.Checked;
      this.setup.AllowESigningVA = this.chkVA.Checked;
      this.setup.AllowESigningUSDA = this.chkUSDA.Checked;
      this.setup.AllowESigningOther = this.chkOther.Checked;
      this.setup.AllowESigningHELOC = this.chkHELOC.Checked;
      this.setDirtyFlag(true);
    }

    private void chkConventional_Click(object sender, EventArgs e)
    {
      this.chkElectronicSigning.Checked = this.chkConventional.Checked || this.chkFHA.Checked || this.chkVA.Checked || this.chkUSDA.Checked || this.chkOther.Checked || this.chkHELOC.Checked;
      this.setup.AllowESigningConventional = this.chkConventional.Checked;
      this.setDirtyFlag(true);
    }

    private void chkFHA_Click(object sender, EventArgs e)
    {
      this.chkElectronicSigning.Checked = this.chkConventional.Checked || this.chkFHA.Checked || this.chkVA.Checked || this.chkUSDA.Checked || this.chkOther.Checked || this.chkHELOC.Checked;
      this.setup.AllowESigningFHA = this.chkFHA.Checked;
      this.setDirtyFlag(true);
    }

    private void chkVA_Click(object sender, EventArgs e)
    {
      this.chkElectronicSigning.Checked = this.chkConventional.Checked || this.chkFHA.Checked || this.chkVA.Checked || this.chkUSDA.Checked || this.chkOther.Checked || this.chkHELOC.Checked;
      this.setup.AllowESigningVA = this.chkVA.Checked;
      this.setDirtyFlag(true);
    }

    private void chkUSDA_Click(object sender, EventArgs e)
    {
      this.chkElectronicSigning.Checked = this.chkConventional.Checked || this.chkFHA.Checked || this.chkVA.Checked || this.chkUSDA.Checked || this.chkOther.Checked || this.chkHELOC.Checked;
      this.setup.AllowESigningUSDA = this.chkUSDA.Checked;
      this.setDirtyFlag(true);
    }

    private void chkOther_Click(object sender, EventArgs e)
    {
      this.chkElectronicSigning.Checked = this.chkConventional.Checked || this.chkFHA.Checked || this.chkVA.Checked || this.chkUSDA.Checked || this.chkOther.Checked || this.chkHELOC.Checked;
      this.setup.AllowESigningOther = this.chkOther.Checked;
      this.setDirtyFlag(true);
    }

    private void chkHELOC_Click(object sender, EventArgs e)
    {
      this.chkElectronicSigning.Checked = this.chkConventional.Checked || this.chkFHA.Checked || this.chkVA.Checked || this.chkUSDA.Checked || this.chkOther.Checked || this.chkHELOC.Checked;
      this.setup.AllowESigningHELOC = this.chkHELOC.Checked;
      this.setDirtyFlag(true);
    }

    private void chkFulfillmentFee_CheckedChanged(object sender, EventArgs e)
    {
      this.txtFulfillmentFee.Enabled = this.chkFulfillmentFee.Checked;
    }

    private void chkFulfillmentFee_Click(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void txtFulfillmentFee_TextChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void cboConsent_SelectionChangeCommitted(object sender, EventArgs e)
    {
      switch (this.cboConsent.SelectedIndex)
      {
        case 0:
          this.setup.ConsentModelType = "Loan level consent";
          break;
        case 1:
          this.setup.ConsentModelType = "Package level consent";
          break;
      }
      this.setDirtyFlag(true);
    }

    private void chkBranchAddress_Click(object sender, EventArgs e)
    {
      this.setup.UseBranchAddress = this.chkBranchAddress.Checked;
      this.setDirtyFlag(true);
    }

    private void chkSigningOrderDisclosure_Click(object sender, EventArgs e)
    {
      this.signOrderSetup.SignOrderEnabled = Convert.ToString(this.chkSigningOrderDisclosure.Checked);
      this.setDirtyFlag(true);
    }

    private void btnStates_Click(object sender, EventArgs e)
    {
      using (SigningOrderSettingsDialog orderSettingsDialog = new SigningOrderSettingsDialog(this.signOrderSetup))
      {
        if (orderSettingsDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return;
        this.setDirtyFlag(true);
      }
    }

    private void chkeSignedDocuments_Click(object sender, EventArgs e)
    {
      AutoRetrieveSettings retrieveSettings1 = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 1)).FirstOrDefault<AutoRetrieveSettings>();
      if (retrieveSettings1 != null)
      {
        retrieveSettings1.Access = this.chkeSignedDocuments.Checked;
        if (!retrieveSettings1.Access)
        {
          AutoRetrieveSettings retrieveSettings2 = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 6)).FirstOrDefault<AutoRetrieveSettings>();
          if (retrieveSettings2 != null)
          {
            this.chkeSignedDocumentsAfterMilestone.Checked = false;
            retrieveSettings2.Access = this.chkeSignedDocumentsAfterMilestone.Checked;
            this.cboeSignedDocumentsAfterMilestone.Enabled = false;
            this.cboeSignedDocumentsAfterMilestone.SelectedIndex = -1;
            retrieveSettings2.MilestoneId = (string) null;
          }
        }
      }
      this.setDirtyFlag(true);
    }

    private void chkDocUploadedToCCTask_Click(object sender, EventArgs e)
    {
      AutoRetrieveSettings retrieveSettings1 = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 2)).FirstOrDefault<AutoRetrieveSettings>();
      if (retrieveSettings1 != null)
      {
        retrieveSettings1.Access = this.chkDocUploadedToCCTask.Checked;
        if (!retrieveSettings1.Access)
        {
          AutoRetrieveSettings retrieveSettings2 = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 7)).FirstOrDefault<AutoRetrieveSettings>();
          if (retrieveSettings2 != null)
          {
            this.chkDocUploadedToCCTaskAfterMilestone.Checked = false;
            retrieveSettings2.Access = this.chkDocUploadedToCCTaskAfterMilestone.Checked;
            this.cboDocUploadedToCCTaskAfterMilestone.Enabled = false;
            this.cboDocUploadedToCCTaskAfterMilestone.SelectedIndex = -1;
            retrieveSettings2.MilestoneId = (string) null;
          }
        }
      }
      this.setDirtyFlag(true);
    }

    private void chkDocUploadedWithoutaTask_Click(object sender, EventArgs e)
    {
      AutoRetrieveSettings retrieveSettings1 = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 3)).FirstOrDefault<AutoRetrieveSettings>();
      if (retrieveSettings1 != null)
      {
        retrieveSettings1.Access = this.chkDocUploadedWithoutaTask.Checked;
        if (!retrieveSettings1.Access)
        {
          AutoRetrieveSettings retrieveSettings2 = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 8)).FirstOrDefault<AutoRetrieveSettings>();
          if (retrieveSettings2 != null)
          {
            this.chkDocUploadedWithoutaTaskAfterMilestone.Checked = false;
            retrieveSettings2.Access = this.chkDocUploadedWithoutaTaskAfterMilestone.Checked;
            this.cboDocUploadedWithoutaTaskAfterMilestone.Enabled = false;
            this.cboDocUploadedWithoutaTaskAfterMilestone.SelectedIndex = -1;
            retrieveSettings2.MilestoneId = (string) null;
          }
        }
      }
      this.setDirtyFlag(true);
    }

    private void chkFax_Click(object sender, EventArgs e)
    {
      AutoRetrieveSettings retrieveSettings1 = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 4)).FirstOrDefault<AutoRetrieveSettings>();
      if (retrieveSettings1 != null)
      {
        retrieveSettings1.Access = this.chkFax.Checked;
        if (!retrieveSettings1.Access)
        {
          AutoRetrieveSettings retrieveSettings2 = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 9)).FirstOrDefault<AutoRetrieveSettings>();
          if (retrieveSettings2 != null)
          {
            this.chkFaxAfterMilestone.Checked = false;
            retrieveSettings2.Access = this.chkFaxAfterMilestone.Checked;
            this.cboFaxAfterMilestone.Enabled = false;
            this.cboFaxAfterMilestone.SelectedIndex = -1;
            retrieveSettings2.MilestoneId = (string) null;
          }
        }
      }
      this.setDirtyFlag(true);
    }

    private void chkScan_Click(object sender, EventArgs e)
    {
      AutoRetrieveSettings retrieveSettings1 = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 5)).FirstOrDefault<AutoRetrieveSettings>();
      if (retrieveSettings1 != null)
      {
        retrieveSettings1.Access = this.chkScan.Checked;
        if (!retrieveSettings1.Access)
        {
          AutoRetrieveSettings retrieveSettings2 = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 10)).FirstOrDefault<AutoRetrieveSettings>();
          if (retrieveSettings2 != null)
          {
            this.chkScanAfterMilestone.Checked = false;
            retrieveSettings2.Access = this.chkScanAfterMilestone.Checked;
            this.cboScanAfterMilestone.Enabled = false;
            this.cboScanAfterMilestone.SelectedIndex = -1;
            retrieveSettings2.MilestoneId = (string) null;
          }
        }
      }
      this.setDirtyFlag(true);
    }

    private void chkeSignedDocumentsAfterMilestone_Click(object sender, EventArgs e)
    {
      AutoRetrieveSettings retrieveSettings1 = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 6)).FirstOrDefault<AutoRetrieveSettings>();
      if (retrieveSettings1 != null)
      {
        retrieveSettings1.Access = this.chkeSignedDocumentsAfterMilestone.Checked;
        this.cboeSignedDocumentsAfterMilestone.Enabled = this.chkeSignedDocumentsAfterMilestone.Checked;
        if (!this.cboeSignedDocumentsAfterMilestone.Enabled)
        {
          retrieveSettings1.MilestoneId = (string) null;
          this.cboeSignedDocumentsAfterMilestone.SelectedIndex = -1;
        }
        else if (!this.chkeSignedDocuments.Checked)
        {
          this.chkeSignedDocuments.Checked = true;
          AutoRetrieveSettings retrieveSettings2 = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 1)).FirstOrDefault<AutoRetrieveSettings>();
          if (retrieveSettings2 != null)
            retrieveSettings2.Access = true;
        }
        if (this.cboeSignedDocumentsAfterMilestone.SelectedItem == null && this.cboeSignedDocumentsAfterMilestone.Items.Count == 0)
          this.cboeSignedDocumentsAfterMilestone.Items.AddRange((object[]) this.milestoneList.ToArray<EllieMae.EMLite.Workflow.Milestone>());
      }
      this.setDirtyFlag(true);
    }

    private void chkDocUploadedToCCTaskAfterMilestone_Click(object sender, EventArgs e)
    {
      AutoRetrieveSettings retrieveSettings1 = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 7)).FirstOrDefault<AutoRetrieveSettings>();
      if (retrieveSettings1 != null)
      {
        retrieveSettings1.Access = this.chkDocUploadedToCCTaskAfterMilestone.Checked;
        this.cboDocUploadedToCCTaskAfterMilestone.Enabled = this.chkDocUploadedToCCTaskAfterMilestone.Checked;
        if (!this.cboDocUploadedToCCTaskAfterMilestone.Enabled)
        {
          retrieveSettings1.MilestoneId = (string) null;
          this.cboDocUploadedToCCTaskAfterMilestone.SelectedIndex = -1;
        }
        else if (!this.chkDocUploadedToCCTask.Checked)
        {
          this.chkDocUploadedToCCTask.Checked = true;
          AutoRetrieveSettings retrieveSettings2 = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 2)).FirstOrDefault<AutoRetrieveSettings>();
          if (retrieveSettings2 != null)
            retrieveSettings2.Access = true;
        }
        if (this.cboDocUploadedToCCTaskAfterMilestone.SelectedItem == null && this.cboDocUploadedToCCTaskAfterMilestone.Items.Count == 0)
          this.cboDocUploadedToCCTaskAfterMilestone.Items.AddRange((object[]) this.milestoneList.ToArray<EllieMae.EMLite.Workflow.Milestone>());
      }
      this.setDirtyFlag(true);
    }

    private void chkDocUploadedWithoutaTaskAfterMilestone_Click(object sender, EventArgs e)
    {
      AutoRetrieveSettings retrieveSettings1 = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 8)).FirstOrDefault<AutoRetrieveSettings>();
      if (retrieveSettings1 != null)
      {
        retrieveSettings1.Access = this.chkDocUploadedWithoutaTaskAfterMilestone.Checked;
        this.cboDocUploadedWithoutaTaskAfterMilestone.Enabled = this.chkDocUploadedWithoutaTaskAfterMilestone.Checked;
        if (!this.cboDocUploadedWithoutaTaskAfterMilestone.Enabled)
        {
          retrieveSettings1.MilestoneId = (string) null;
          this.cboDocUploadedWithoutaTaskAfterMilestone.SelectedIndex = -1;
        }
        else if (!this.chkDocUploadedWithoutaTask.Checked)
        {
          this.chkDocUploadedWithoutaTask.Checked = true;
          AutoRetrieveSettings retrieveSettings2 = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 3)).FirstOrDefault<AutoRetrieveSettings>();
          if (retrieveSettings2 != null)
            retrieveSettings2.Access = true;
        }
        if (this.cboDocUploadedWithoutaTaskAfterMilestone.SelectedItem == null && this.cboDocUploadedWithoutaTaskAfterMilestone.Items.Count == 0)
          this.cboDocUploadedWithoutaTaskAfterMilestone.Items.AddRange((object[]) this.milestoneList.ToArray<EllieMae.EMLite.Workflow.Milestone>());
      }
      this.setDirtyFlag(true);
    }

    private void chkFaxAfterMilestone_Click(object sender, EventArgs e)
    {
      AutoRetrieveSettings retrieveSettings1 = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 9)).FirstOrDefault<AutoRetrieveSettings>();
      if (retrieveSettings1 != null)
      {
        retrieveSettings1.Access = this.chkFaxAfterMilestone.Checked;
        this.cboFaxAfterMilestone.Enabled = this.chkFaxAfterMilestone.Checked;
        if (!this.cboFaxAfterMilestone.Enabled)
        {
          retrieveSettings1.MilestoneId = (string) null;
          this.cboFaxAfterMilestone.SelectedIndex = -1;
        }
        else if (!this.chkFax.Checked)
        {
          this.chkFax.Checked = true;
          AutoRetrieveSettings retrieveSettings2 = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 4)).FirstOrDefault<AutoRetrieveSettings>();
          if (retrieveSettings2 != null)
            retrieveSettings2.Access = true;
        }
        if (this.cboFaxAfterMilestone.SelectedItem == null && this.cboFaxAfterMilestone.Items.Count == 0)
          this.cboFaxAfterMilestone.Items.AddRange((object[]) this.milestoneList.ToArray<EllieMae.EMLite.Workflow.Milestone>());
      }
      this.setDirtyFlag(true);
    }

    private void chkScanAfterMilestone_Click(object sender, EventArgs e)
    {
      AutoRetrieveSettings retrieveSettings1 = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 10)).FirstOrDefault<AutoRetrieveSettings>();
      if (retrieveSettings1 != null)
      {
        retrieveSettings1.Access = this.chkScanAfterMilestone.Checked;
        this.cboScanAfterMilestone.Enabled = this.chkScanAfterMilestone.Checked;
        if (!this.cboScanAfterMilestone.Enabled)
        {
          retrieveSettings1.MilestoneId = (string) null;
          this.cboScanAfterMilestone.SelectedIndex = -1;
        }
        else if (!this.chkScan.Checked)
        {
          this.chkScan.Checked = true;
          AutoRetrieveSettings retrieveSettings2 = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 5)).FirstOrDefault<AutoRetrieveSettings>();
          if (retrieveSettings2 != null)
            retrieveSettings2.Access = true;
        }
        if (this.cboScanAfterMilestone.SelectedItem == null && this.cboScanAfterMilestone.Items.Count == 0)
          this.cboScanAfterMilestone.Items.AddRange((object[]) this.milestoneList.ToArray<EllieMae.EMLite.Workflow.Milestone>());
      }
      this.setDirtyFlag(true);
    }

    private void cboeSignedDocumentsAfterMilestone_SelectedIndexChanged(object sender, EventArgs e)
    {
      AutoRetrieveSettings retrieveSettings = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 6)).FirstOrDefault<AutoRetrieveSettings>();
      if (retrieveSettings != null)
      {
        EllieMae.EMLite.Workflow.Milestone selectedItem = (EllieMae.EMLite.Workflow.Milestone) this.cboeSignedDocumentsAfterMilestone.SelectedItem;
        retrieveSettings.MilestoneId = selectedItem?.MilestoneID;
      }
      this.setDirtyFlag(true);
    }

    private void cboDocUploadedToCCTaskAfterMilestone_SelectedIndexChanged(
      object sender,
      EventArgs e)
    {
      AutoRetrieveSettings retrieveSettings = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 7)).FirstOrDefault<AutoRetrieveSettings>();
      if (retrieveSettings != null)
      {
        EllieMae.EMLite.Workflow.Milestone selectedItem = (EllieMae.EMLite.Workflow.Milestone) this.cboDocUploadedToCCTaskAfterMilestone.SelectedItem;
        retrieveSettings.MilestoneId = selectedItem?.MilestoneID;
      }
      this.setDirtyFlag(true);
    }

    private void cboDocUploadedWithoutaTaskAfterMilestone_SelectedIndexChanged(
      object sender,
      EventArgs e)
    {
      AutoRetrieveSettings retrieveSettings = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 8)).FirstOrDefault<AutoRetrieveSettings>();
      if (retrieveSettings != null)
      {
        EllieMae.EMLite.Workflow.Milestone selectedItem = (EllieMae.EMLite.Workflow.Milestone) this.cboDocUploadedWithoutaTaskAfterMilestone.SelectedItem;
        retrieveSettings.MilestoneId = selectedItem?.MilestoneID;
      }
      this.setDirtyFlag(true);
    }

    private void cboFaxAfterMilestone_SelectedIndexChanged(object sender, EventArgs e)
    {
      AutoRetrieveSettings retrieveSettings = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 9)).FirstOrDefault<AutoRetrieveSettings>();
      if (retrieveSettings != null)
      {
        EllieMae.EMLite.Workflow.Milestone selectedItem = (EllieMae.EMLite.Workflow.Milestone) this.cboFaxAfterMilestone.SelectedItem;
        retrieveSettings.MilestoneId = selectedItem?.MilestoneID;
      }
      this.setDirtyFlag(true);
    }

    private void cboScanAfterMilestone_SelectedIndexChanged(object sender, EventArgs e)
    {
      AutoRetrieveSettings retrieveSettings = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 10)).FirstOrDefault<AutoRetrieveSettings>();
      if (retrieveSettings != null)
      {
        EllieMae.EMLite.Workflow.Milestone selectedItem = (EllieMae.EMLite.Workflow.Milestone) this.cboScanAfterMilestone.SelectedItem;
        retrieveSettings.MilestoneId = selectedItem?.MilestoneID;
      }
      this.setDirtyFlag(true);
    }

    private void ESignedDocumentSetup()
    {
      AutoRetrieveSettings retrieveSettings = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 1)).FirstOrDefault<AutoRetrieveSettings>();
      if (retrieveSettings != null)
        this.chkeSignedDocuments.Checked = retrieveSettings.Access;
      IEnumerable<AutoRetrieveSettings> source = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 6));
      if (source != null)
      {
        this.chkeSignedDocumentsAfterMilestone.Checked = source.FirstOrDefault<AutoRetrieveSettings>().Access;
        this.cboeSignedDocumentsAfterMilestone.Enabled = this.chkeSignedDocumentsAfterMilestone.Checked;
      }
      if (!this.cboeSignedDocumentsAfterMilestone.Enabled)
        return;
      this.cboeSignedDocumentsAfterMilestone.Items.AddRange((object[]) this.milestoneList.ToArray<EllieMae.EMLite.Workflow.Milestone>());
      string eSignedDocumentsMilestoneId = source.Select<AutoRetrieveSettings, string>((System.Func<AutoRetrieveSettings, string>) (m => m.MilestoneId)).FirstOrDefault<string>();
      if (eSignedDocumentsMilestoneId == null)
        return;
      EllieMae.EMLite.Workflow.Milestone milestone = this.milestoneList.ToList<EllieMae.EMLite.Workflow.Milestone>().Where<EllieMae.EMLite.Workflow.Milestone>((System.Func<EllieMae.EMLite.Workflow.Milestone, bool>) (m => m.MilestoneID == eSignedDocumentsMilestoneId.ToString())).FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>();
      if (milestone == null)
        return;
      if (!milestone.Archived)
      {
        this.cboeSignedDocumentsAfterMilestone.SelectedIndex = this.milestoneList.ToList<EllieMae.EMLite.Workflow.Milestone>().FindIndex((Predicate<EllieMae.EMLite.Workflow.Milestone>) (m => m.MilestoneID == eSignedDocumentsMilestoneId.ToString()));
      }
      else
      {
        this.cboeSignedDocumentsAfterMilestone.SelectedIndex = -1;
        source.FirstOrDefault<AutoRetrieveSettings>().Access = false;
        source.FirstOrDefault<AutoRetrieveSettings>().MilestoneId = (string) null;
        this.session.ConfigurationManager.SaveEDisclosureSetup(this.setup);
      }
    }

    private void DocsUploadedToCCTaskSetup()
    {
      AutoRetrieveSettings retrieveSettings = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 2)).FirstOrDefault<AutoRetrieveSettings>();
      if (retrieveSettings != null)
        this.chkDocUploadedToCCTask.Checked = retrieveSettings.Access;
      IEnumerable<AutoRetrieveSettings> source = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 7));
      if (source != null)
      {
        this.chkDocUploadedToCCTaskAfterMilestone.Checked = source.FirstOrDefault<AutoRetrieveSettings>().Access;
        this.cboDocUploadedToCCTaskAfterMilestone.Enabled = this.chkDocUploadedToCCTaskAfterMilestone.Checked;
      }
      if (!this.cboDocUploadedToCCTaskAfterMilestone.Enabled)
        return;
      this.cboDocUploadedToCCTaskAfterMilestone.Items.AddRange((object[]) this.milestoneList.ToArray<EllieMae.EMLite.Workflow.Milestone>());
      string docUploadedToCCTaskMilestoneId = source.Select<AutoRetrieveSettings, string>((System.Func<AutoRetrieveSettings, string>) (m => m.MilestoneId)).FirstOrDefault<string>();
      if (docUploadedToCCTaskMilestoneId == null)
        return;
      EllieMae.EMLite.Workflow.Milestone milestone = this.milestoneList.ToList<EllieMae.EMLite.Workflow.Milestone>().Where<EllieMae.EMLite.Workflow.Milestone>((System.Func<EllieMae.EMLite.Workflow.Milestone, bool>) (m => m.MilestoneID == docUploadedToCCTaskMilestoneId.ToString())).FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>();
      if (milestone == null)
        return;
      if (!milestone.Archived)
      {
        this.cboDocUploadedToCCTaskAfterMilestone.SelectedIndex = this.milestoneList.ToList<EllieMae.EMLite.Workflow.Milestone>().FindIndex((Predicate<EllieMae.EMLite.Workflow.Milestone>) (m => m.MilestoneID == docUploadedToCCTaskMilestoneId.ToString()));
      }
      else
      {
        this.cboDocUploadedToCCTaskAfterMilestone.SelectedIndex = -1;
        source.FirstOrDefault<AutoRetrieveSettings>().Access = false;
        source.FirstOrDefault<AutoRetrieveSettings>().MilestoneId = (string) null;
        this.session.ConfigurationManager.SaveEDisclosureSetup(this.setup);
      }
    }

    private void DocsUploadedWithoutTaskSetup()
    {
      AutoRetrieveSettings retrieveSettings = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 3)).FirstOrDefault<AutoRetrieveSettings>();
      if (retrieveSettings != null)
        this.chkDocUploadedWithoutaTask.Checked = retrieveSettings.Access;
      IEnumerable<AutoRetrieveSettings> source = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 8));
      if (source != null)
      {
        this.chkDocUploadedWithoutaTaskAfterMilestone.Checked = source.FirstOrDefault<AutoRetrieveSettings>().Access;
        this.cboDocUploadedWithoutaTaskAfterMilestone.Enabled = this.chkDocUploadedWithoutaTaskAfterMilestone.Checked;
      }
      if (!this.cboDocUploadedWithoutaTaskAfterMilestone.Enabled)
        return;
      this.cboDocUploadedWithoutaTaskAfterMilestone.Items.AddRange((object[]) this.milestoneList.ToArray<EllieMae.EMLite.Workflow.Milestone>());
      string docUploadedWithoutTaskMilestoneId = source.Select<AutoRetrieveSettings, string>((System.Func<AutoRetrieveSettings, string>) (m => m.MilestoneId)).FirstOrDefault<string>();
      if (docUploadedWithoutTaskMilestoneId == null)
        return;
      EllieMae.EMLite.Workflow.Milestone milestone = this.milestoneList.ToList<EllieMae.EMLite.Workflow.Milestone>().Where<EllieMae.EMLite.Workflow.Milestone>((System.Func<EllieMae.EMLite.Workflow.Milestone, bool>) (m => m.MilestoneID == docUploadedWithoutTaskMilestoneId.ToString())).FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>();
      if (milestone == null)
        return;
      if (!milestone.Archived)
      {
        this.cboDocUploadedWithoutaTaskAfterMilestone.SelectedIndex = this.milestoneList.ToList<EllieMae.EMLite.Workflow.Milestone>().FindIndex((Predicate<EllieMae.EMLite.Workflow.Milestone>) (m => m.MilestoneID == docUploadedWithoutTaskMilestoneId.ToString()));
      }
      else
      {
        this.cboDocUploadedWithoutaTaskAfterMilestone.SelectedIndex = -1;
        source.FirstOrDefault<AutoRetrieveSettings>().Access = false;
        source.FirstOrDefault<AutoRetrieveSettings>().MilestoneId = (string) null;
        this.session.ConfigurationManager.SaveEDisclosureSetup(this.setup);
      }
    }

    private void FaxSetup()
    {
      AutoRetrieveSettings retrieveSettings = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 4)).FirstOrDefault<AutoRetrieveSettings>();
      if (retrieveSettings != null)
        this.chkFax.Checked = retrieveSettings.Access;
      IEnumerable<AutoRetrieveSettings> source = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 9));
      if (source != null)
      {
        this.chkFaxAfterMilestone.Checked = source.FirstOrDefault<AutoRetrieveSettings>().Access;
        this.cboFaxAfterMilestone.Enabled = this.chkFaxAfterMilestone.Checked;
      }
      if (!this.cboFaxAfterMilestone.Enabled)
        return;
      this.cboFaxAfterMilestone.Items.AddRange((object[]) this.milestoneList.ToArray<EllieMae.EMLite.Workflow.Milestone>());
      string faxMilestoneId = source.Select<AutoRetrieveSettings, string>((System.Func<AutoRetrieveSettings, string>) (m => m.MilestoneId)).FirstOrDefault<string>();
      if (faxMilestoneId == null)
        return;
      EllieMae.EMLite.Workflow.Milestone milestone = this.milestoneList.ToList<EllieMae.EMLite.Workflow.Milestone>().Where<EllieMae.EMLite.Workflow.Milestone>((System.Func<EllieMae.EMLite.Workflow.Milestone, bool>) (m => m.MilestoneID == faxMilestoneId.ToString())).FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>();
      if (milestone == null)
        return;
      if (!milestone.Archived)
      {
        this.cboFaxAfterMilestone.SelectedIndex = this.milestoneList.ToList<EllieMae.EMLite.Workflow.Milestone>().FindIndex((Predicate<EllieMae.EMLite.Workflow.Milestone>) (m => m.MilestoneID == faxMilestoneId.ToString()));
      }
      else
      {
        this.cboFaxAfterMilestone.SelectedIndex = -1;
        source.FirstOrDefault<AutoRetrieveSettings>().Access = false;
        source.FirstOrDefault<AutoRetrieveSettings>().MilestoneId = (string) null;
        this.session.ConfigurationManager.SaveEDisclosureSetup(this.setup);
      }
    }

    private void ScanSetup()
    {
      AutoRetrieveSettings retrieveSettings = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 5)).FirstOrDefault<AutoRetrieveSettings>();
      if (retrieveSettings != null)
        this.chkScan.Checked = retrieveSettings.Access;
      IEnumerable<AutoRetrieveSettings> source = this.setup.AutoRetrieveSettings.Where<AutoRetrieveSettings>((System.Func<AutoRetrieveSettings, bool>) (m => m.AutoRetrieveSettingsID == 10));
      if (source != null)
      {
        this.chkScanAfterMilestone.Checked = source.FirstOrDefault<AutoRetrieveSettings>().Access;
        this.cboScanAfterMilestone.Enabled = this.chkScanAfterMilestone.Checked;
      }
      if (!this.cboScanAfterMilestone.Enabled)
        return;
      this.cboScanAfterMilestone.Items.AddRange((object[]) this.milestoneList.ToArray<EllieMae.EMLite.Workflow.Milestone>());
      string scanMilestoneId = source.Select<AutoRetrieveSettings, string>((System.Func<AutoRetrieveSettings, string>) (m => m.MilestoneId)).FirstOrDefault<string>();
      if (scanMilestoneId == null)
        return;
      EllieMae.EMLite.Workflow.Milestone milestone = this.milestoneList.ToList<EllieMae.EMLite.Workflow.Milestone>().Where<EllieMae.EMLite.Workflow.Milestone>((System.Func<EllieMae.EMLite.Workflow.Milestone, bool>) (m => m.MilestoneID == scanMilestoneId.ToString())).FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>();
      if (milestone == null)
        return;
      if (!milestone.Archived)
      {
        this.cboScanAfterMilestone.SelectedIndex = this.milestoneList.ToList<EllieMae.EMLite.Workflow.Milestone>().FindIndex((Predicate<EllieMae.EMLite.Workflow.Milestone>) (m => m.MilestoneID == scanMilestoneId.ToString()));
      }
      else
      {
        this.cboScanAfterMilestone.SelectedIndex = -1;
        source.FirstOrDefault<AutoRetrieveSettings>().Access = false;
        source.FirstOrDefault<AutoRetrieveSettings>().MilestoneId = (string) null;
        this.session.ConfigurationManager.SaveEDisclosureSetup(this.setup);
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
      this.pnlSigningConsent = new Panel();
      this.gcAutoRetrieve = new GroupContainer();
      this.cboScanAfterMilestone = new ComboBox();
      this.cboFaxAfterMilestone = new ComboBox();
      this.cboDocUploadedWithoutaTaskAfterMilestone = new ComboBox();
      this.cboDocUploadedToCCTaskAfterMilestone = new ComboBox();
      this.cboeSignedDocumentsAfterMilestone = new ComboBox();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label1 = new Label();
      this.chkScanAfterMilestone = new CheckBox();
      this.chkScan = new CheckBox();
      this.chkFaxAfterMilestone = new CheckBox();
      this.chkFax = new CheckBox();
      this.chkDocUploadedWithoutaTaskAfterMilestone = new CheckBox();
      this.chkDocUploadedWithoutaTask = new CheckBox();
      this.chkDocUploadedToCCTaskAfterMilestone = new CheckBox();
      this.chkDocUploadedToCCTask = new CheckBox();
      this.chkeSignedDocumentsAfterMilestone = new CheckBox();
      this.chkeSignedDocuments = new CheckBox();
      this.gcConsents = new GroupContainer();
      this.lblFulfillmentFee = new Label();
      this.txtFulfillmentFee = new TextBox();
      this.chkFulfillmentFee = new CheckBox();
      this.cboConsent = new ComboBox();
      this.label2 = new Label();
      this.chkBranchAddress = new CheckBox();
      this.gcComplianceReviewReport = new GroupContainer();
      this.gcSigning = new GroupContainer();
      this.chkSigningOrderDisclosure = new CheckBox();
      this.btnStates = new Button();
      this.chkHELOC = new CheckBox();
      this.chkOther = new CheckBox();
      this.chkUSDA = new CheckBox();
      this.chkVA = new CheckBox();
      this.chkFHA = new CheckBox();
      this.chkConventional = new CheckBox();
      this.chkElectronicSigning = new CheckBox();
      this.chkIncludeComplianceReport = new CheckBox();
      this.pnlPackages = new Panel();
      this.gcPackages = new GroupContainer();
      this.pnlDefaultChannel = new Panel();
      this.cboDefaultChannel = new ComboBox();
      this.lblDefaultChannel = new Label();
      this.tabPackages = new TabControl();
      this.pageRetail = new TabPage();
      this.retailChannel = new EDisclosureChannelControl(this.session, this.isSettingsSync);
      this.pageWholesale = new TabPage();
      this.wholesaleChannel = new EDisclosureChannelControl(this.session, this.isSettingsSync);
      this.pageBroker = new TabPage();
      this.brokerChannel = new EDisclosureChannelControl(this.session, this.isSettingsSync);
      this.pageCorrespondent = new TabPage();
      this.correspondentChannel = new EDisclosureChannelControl(this.session, this.isSettingsSync);
      this.pnlSigningConsent.SuspendLayout();
      this.gcAutoRetrieve.SuspendLayout();
      this.gcConsents.SuspendLayout();
      this.gcSigning.SuspendLayout();
      this.gcComplianceReviewReport.SuspendLayout();
      this.pnlPackages.SuspendLayout();
      this.gcPackages.SuspendLayout();
      this.pnlDefaultChannel.SuspendLayout();
      this.tabPackages.SuspendLayout();
      this.pageRetail.SuspendLayout();
      this.pageWholesale.SuspendLayout();
      this.pageBroker.SuspendLayout();
      this.pageCorrespondent.SuspendLayout();
      this.SuspendLayout();
      this.pnlSigningConsent.AutoScroll = true;
      this.pnlSigningConsent.Controls.Add((Control) this.gcAutoRetrieve);
      this.pnlSigningConsent.Controls.Add((Control) this.gcConsents);
      this.pnlSigningConsent.Controls.Add((Control) this.gcSigning);
      this.pnlSigningConsent.Controls.Add((Control) this.gcComplianceReviewReport);
      this.pnlSigningConsent.Dock = DockStyle.Bottom;
      this.pnlSigningConsent.Location = new Point(0, 362);
      this.pnlSigningConsent.Name = "pnlSigningConsent";
      this.pnlSigningConsent.Size = new Size(1114, 282);
      this.pnlSigningConsent.TabIndex = 2;
      this.gcAutoRetrieve.AutoScroll = true;
      this.gcAutoRetrieve.AutoSize = true;
      this.gcAutoRetrieve.Controls.Add((Control) this.cboScanAfterMilestone);
      this.gcAutoRetrieve.Controls.Add((Control) this.cboFaxAfterMilestone);
      this.gcAutoRetrieve.Controls.Add((Control) this.cboDocUploadedWithoutaTaskAfterMilestone);
      this.gcAutoRetrieve.Controls.Add((Control) this.cboDocUploadedToCCTaskAfterMilestone);
      this.gcAutoRetrieve.Controls.Add((Control) this.cboeSignedDocumentsAfterMilestone);
      this.gcAutoRetrieve.Controls.Add((Control) this.label6);
      this.gcAutoRetrieve.Controls.Add((Control) this.label5);
      this.gcAutoRetrieve.Controls.Add((Control) this.label4);
      this.gcAutoRetrieve.Controls.Add((Control) this.label3);
      this.gcAutoRetrieve.Controls.Add((Control) this.label1);
      this.gcAutoRetrieve.Controls.Add((Control) this.chkScanAfterMilestone);
      this.gcAutoRetrieve.Controls.Add((Control) this.chkScan);
      this.gcAutoRetrieve.Controls.Add((Control) this.chkFaxAfterMilestone);
      this.gcAutoRetrieve.Controls.Add((Control) this.chkFax);
      this.gcAutoRetrieve.Controls.Add((Control) this.chkDocUploadedWithoutaTaskAfterMilestone);
      this.gcAutoRetrieve.Controls.Add((Control) this.chkDocUploadedWithoutaTask);
      this.gcAutoRetrieve.Controls.Add((Control) this.chkDocUploadedToCCTaskAfterMilestone);
      this.gcAutoRetrieve.Controls.Add((Control) this.chkDocUploadedToCCTask);
      this.gcAutoRetrieve.Controls.Add((Control) this.chkeSignedDocumentsAfterMilestone);
      this.gcAutoRetrieve.Controls.Add((Control) this.chkeSignedDocuments);
      this.gcAutoRetrieve.HeaderForeColor = SystemColors.ControlText;
      this.gcAutoRetrieve.Location = new Point(851, 0);
      this.gcAutoRetrieve.Name = "gcAutoRetrieve";
      this.gcAutoRetrieve.Size = new Size(1083, 282);
      this.gcAutoRetrieve.TabIndex = 4;
      this.gcAutoRetrieve.Text = "Auto Retrieve";
      this.cboScanAfterMilestone.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboScanAfterMilestone.FormattingEnabled = true;
      this.cboScanAfterMilestone.Location = new Point(79, 232);
      this.cboScanAfterMilestone.Name = "cboScanAfterMilestone";
      this.cboScanAfterMilestone.Size = new Size(155, 27);
      this.cboScanAfterMilestone.TabIndex = 29;
      this.cboScanAfterMilestone.SelectedIndexChanged += new EventHandler(this.cboScanAfterMilestone_SelectedIndexChanged);
      this.cboFaxAfterMilestone.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFaxAfterMilestone.FormattingEnabled = true;
      this.cboFaxAfterMilestone.Location = new Point(79, 190);
      this.cboFaxAfterMilestone.Name = "cboFaxAfterMilestone";
      this.cboFaxAfterMilestone.Size = new Size(155, 27);
      this.cboFaxAfterMilestone.TabIndex = 28;
      this.cboFaxAfterMilestone.SelectedIndexChanged += new EventHandler(this.cboFaxAfterMilestone_SelectedIndexChanged);
      this.cboDocUploadedWithoutaTaskAfterMilestone.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboDocUploadedWithoutaTaskAfterMilestone.FormattingEnabled = true;
      this.cboDocUploadedWithoutaTaskAfterMilestone.Location = new Point(81, 147);
      this.cboDocUploadedWithoutaTaskAfterMilestone.Name = "cboDocUploadedWithoutaTaskAfterMilestone";
      this.cboDocUploadedWithoutaTaskAfterMilestone.Size = new Size(155, 27);
      this.cboDocUploadedWithoutaTaskAfterMilestone.TabIndex = 27;
      this.cboDocUploadedWithoutaTaskAfterMilestone.SelectedIndexChanged += new EventHandler(this.cboDocUploadedWithoutaTaskAfterMilestone_SelectedIndexChanged);
      this.cboDocUploadedToCCTaskAfterMilestone.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboDocUploadedToCCTaskAfterMilestone.FormattingEnabled = true;
      this.cboDocUploadedToCCTaskAfterMilestone.Location = new Point(81, 101);
      this.cboDocUploadedToCCTaskAfterMilestone.Name = "cboDocUploadedToCCTaskAfterMilestone";
      this.cboDocUploadedToCCTaskAfterMilestone.Size = new Size(155, 27);
      this.cboDocUploadedToCCTaskAfterMilestone.TabIndex = 26;
      this.cboDocUploadedToCCTaskAfterMilestone.SelectedIndexChanged += new EventHandler(this.cboDocUploadedToCCTaskAfterMilestone_SelectedIndexChanged);
      this.cboeSignedDocumentsAfterMilestone.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboeSignedDocumentsAfterMilestone.FormattingEnabled = true;
      this.cboeSignedDocumentsAfterMilestone.Location = new Point(81, 56);
      this.cboeSignedDocumentsAfterMilestone.Name = "cboeSignedDocumentsAfterMilestone";
      this.cboeSignedDocumentsAfterMilestone.Size = new Size(155, 27);
      this.cboeSignedDocumentsAfterMilestone.TabIndex = 25;
      this.cboeSignedDocumentsAfterMilestone.SelectionChangeCommitted += new EventHandler(this.cboeSignedDocumentsAfterMilestone_SelectedIndexChanged);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(237, 237);
      this.label6.Name = "label6";
      this.label6.Size = new Size(233, 19);
      this.label6.TabIndex = 24;
      this.label6.Text = "Milestone, disable auto retrieve";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(237, 195);
      this.label5.Name = "label5";
      this.label5.Size = new Size(233, 19);
      this.label5.TabIndex = 23;
      this.label5.Text = "Milestone, disable auto retrieve";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(238, 152);
      this.label4.Name = "label4";
      this.label4.Size = new Size(233, 19);
      this.label4.TabIndex = 22;
      this.label4.Text = "Milestone, disable auto retrieve";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(238, 106);
      this.label3.Name = "label3";
      this.label3.Size = new Size(233, 19);
      this.label3.TabIndex = 21;
      this.label3.Text = "Milestone, disable auto retrieve";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(238, 58);
      this.label1.Name = "label1";
      this.label1.Size = new Size(233, 19);
      this.label1.TabIndex = 20;
      this.label1.Text = "Milestone, disable auto retrieve";
      this.chkScanAfterMilestone.AutoSize = true;
      this.chkScanAfterMilestone.Location = new Point(30, 236);
      this.chkScanAfterMilestone.Name = "chkScanAfterMilestone";
      this.chkScanAfterMilestone.Size = new Size(70, 23);
      this.chkScanAfterMilestone.TabIndex = 9;
      this.chkScanAfterMilestone.Text = "After";
      this.chkScanAfterMilestone.UseVisualStyleBackColor = true;
      this.chkScanAfterMilestone.Click += new EventHandler(this.chkScanAfterMilestone_Click);
      this.chkScan.AutoSize = true;
      this.chkScan.Location = new Point(10, 214);
      this.chkScan.Name = "chkScan";
      this.chkScan.Size = new Size(73, 23);
      this.chkScan.TabIndex = 8;
      this.chkScan.Text = "Scan";
      this.chkScan.UseVisualStyleBackColor = true;
      this.chkScan.Click += new EventHandler(this.chkScan_Click);
      this.chkFaxAfterMilestone.AutoSize = true;
      this.chkFaxAfterMilestone.Location = new Point(30, 192);
      this.chkFaxAfterMilestone.Name = "chkFaxAfterMilestone";
      this.chkFaxAfterMilestone.Size = new Size(70, 23);
      this.chkFaxAfterMilestone.TabIndex = 7;
      this.chkFaxAfterMilestone.Text = "After";
      this.chkFaxAfterMilestone.UseVisualStyleBackColor = true;
      this.chkFaxAfterMilestone.Click += new EventHandler(this.chkFaxAfterMilestone_Click);
      this.chkFax.AutoSize = true;
      this.chkFax.Location = new Point(10, 170);
      this.chkFax.Name = "chkFax";
      this.chkFax.Size = new Size(61, 23);
      this.chkFax.TabIndex = 6;
      this.chkFax.Text = "Fax";
      this.chkFax.UseVisualStyleBackColor = true;
      this.chkFax.Click += new EventHandler(this.chkFax_Click);
      this.chkDocUploadedWithoutaTaskAfterMilestone.AutoSize = true;
      this.chkDocUploadedWithoutaTaskAfterMilestone.Location = new Point(30, 148);
      this.chkDocUploadedWithoutaTaskAfterMilestone.Name = "chkDocUploadedWithoutaTaskAfterMilestone";
      this.chkDocUploadedWithoutaTaskAfterMilestone.Size = new Size(70, 23);
      this.chkDocUploadedWithoutaTaskAfterMilestone.TabIndex = 5;
      this.chkDocUploadedWithoutaTaskAfterMilestone.Text = "After";
      this.chkDocUploadedWithoutaTaskAfterMilestone.UseVisualStyleBackColor = true;
      this.chkDocUploadedWithoutaTaskAfterMilestone.Click += new EventHandler(this.chkDocUploadedWithoutaTaskAfterMilestone_Click);
      this.chkDocUploadedWithoutaTask.AutoSize = true;
      this.chkDocUploadedWithoutaTask.Location = new Point(10, 125);
      this.chkDocUploadedWithoutaTask.Name = "chkDocUploadedWithoutaTask";
      this.chkDocUploadedWithoutaTask.Size = new Size(408, 23);
      this.chkDocUploadedWithoutaTask.TabIndex = 4;
      this.chkDocUploadedWithoutaTask.Text = "Documents Uploaded without a document container";
      this.chkDocUploadedWithoutaTask.UseVisualStyleBackColor = true;
      this.chkDocUploadedWithoutaTask.Click += new EventHandler(this.chkDocUploadedWithoutaTask_Click);
      this.chkDocUploadedToCCTaskAfterMilestone.AutoSize = true;
      this.chkDocUploadedToCCTaskAfterMilestone.Location = new Point(30, 103);
      this.chkDocUploadedToCCTaskAfterMilestone.Name = "chkDocUploadedToCCTaskAfterMilestone";
      this.chkDocUploadedToCCTaskAfterMilestone.Size = new Size(70, 23);
      this.chkDocUploadedToCCTaskAfterMilestone.TabIndex = 3;
      this.chkDocUploadedToCCTaskAfterMilestone.Text = "After";
      this.chkDocUploadedToCCTaskAfterMilestone.UseVisualStyleBackColor = true;
      this.chkDocUploadedToCCTaskAfterMilestone.Click += new EventHandler(this.chkDocUploadedToCCTaskAfterMilestone_Click);
      this.chkDocUploadedToCCTask.AutoSize = true;
      this.chkDocUploadedToCCTask.Location = new Point(10, 81);
      this.chkDocUploadedToCCTask.Name = "chkDocUploadedToCCTask";
      this.chkDocUploadedToCCTask.Size = new Size(384, 23);
      this.chkDocUploadedToCCTask.TabIndex = 2;
      this.chkDocUploadedToCCTask.Text = "Documents Uploaded into a document container";
      this.chkDocUploadedToCCTask.UseVisualStyleBackColor = true;
      this.chkDocUploadedToCCTask.Click += new EventHandler(this.chkDocUploadedToCCTask_Click);
      this.chkeSignedDocumentsAfterMilestone.AutoSize = true;
      this.chkeSignedDocumentsAfterMilestone.Location = new Point(30, 58);
      this.chkeSignedDocumentsAfterMilestone.Name = "chkeSignedDocumentsAfterMilestone";
      this.chkeSignedDocumentsAfterMilestone.Size = new Size(70, 23);
      this.chkeSignedDocumentsAfterMilestone.TabIndex = 1;
      this.chkeSignedDocumentsAfterMilestone.Text = "After";
      this.chkeSignedDocumentsAfterMilestone.UseVisualStyleBackColor = true;
      this.chkeSignedDocumentsAfterMilestone.Click += new EventHandler(this.chkeSignedDocumentsAfterMilestone_Click);
      this.chkeSignedDocuments.AutoSize = true;
      this.chkeSignedDocuments.Location = new Point(10, 35);
      this.chkeSignedDocuments.Name = "chkeSignedDocuments";
      this.chkeSignedDocuments.Size = new Size(182, 23);
      this.chkeSignedDocuments.TabIndex = 0;
      this.chkeSignedDocuments.Text = "eSigned Documents";
      this.chkeSignedDocuments.UseVisualStyleBackColor = true;
      this.chkeSignedDocuments.Click += new EventHandler(this.chkeSignedDocuments_Click);
      this.gcConsents.Controls.Add((Control) this.lblFulfillmentFee);
      this.gcConsents.Controls.Add((Control) this.txtFulfillmentFee);
      this.gcConsents.Controls.Add((Control) this.chkFulfillmentFee);
      this.gcConsents.Controls.Add((Control) this.cboConsent);
      this.gcConsents.Controls.Add((Control) this.label2);
      this.gcConsents.Controls.Add((Control) this.chkBranchAddress);
      this.gcConsents.HeaderForeColor = SystemColors.ControlText;
      this.gcConsents.Location = new Point(0, 175);
      this.gcConsents.Name = "gcConsents";
      this.gcConsents.Size = new Size(851, 107);
      this.gcConsents.TabIndex = 3;
      this.gcConsents.Text = "Consents";
      this.lblFulfillmentFee.AutoSize = true;
      this.lblFulfillmentFee.Location = new Point(475, 78);
      this.lblFulfillmentFee.Name = "lblFulfillmentFee";
      this.lblFulfillmentFee.Size = new Size(249, 19);
      this.lblFulfillmentFee.TabIndex = 8;
      this.lblFulfillmentFee.Text = "fee if paper fulfillment is ordered.";
      this.txtFulfillmentFee.Enabled = false;
      this.txtFulfillmentFee.Location = new Point(420, 76);
      this.txtFulfillmentFee.Name = "txtFulfillmentFee";
      this.txtFulfillmentFee.Size = new Size(52, 26);
      this.txtFulfillmentFee.TabIndex = 7;
      this.txtFulfillmentFee.TextChanged += new EventHandler(this.txtFulfillmentFee_TextChanged);
      this.chkFulfillmentFee.AutoSize = true;
      this.chkFulfillmentFee.Location = new Point(12, 78);
      this.chkFulfillmentFee.Name = "chkFulfillmentFee";
      this.chkFulfillmentFee.Size = new Size(610, 23);
      this.chkFulfillmentFee.TabIndex = 6;
      this.chkFulfillmentFee.Text = "The eDisclosure Consent form will state that the borrower is required to pay a $";
      this.chkFulfillmentFee.UseVisualStyleBackColor = true;
      this.chkFulfillmentFee.CheckedChanged += new EventHandler(this.chkFulfillmentFee_CheckedChanged);
      this.chkFulfillmentFee.Click += new EventHandler(this.chkFulfillmentFee_Click);
      this.cboConsent.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboConsent.FormattingEnabled = true;
      this.cboConsent.Items.AddRange(new object[2]
      {
        (object) "Once per loan",
        (object) "For loan and each package"
      });
      this.cboConsent.Location = new Point(105, 29);
      this.cboConsent.Name = "cboConsent";
      this.cboConsent.Size = new Size(155, 27);
      this.cboConsent.TabIndex = 5;
      this.cboConsent.SelectionChangeCommitted += new EventHandler(this.cboConsent_SelectionChangeCommitted);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(11, 28);
      this.label2.Name = "label2";
      this.label2.Size = new Size(140, 19);
      this.label2.TabIndex = 4;
      this.label2.Text = "Consent Required";
      this.chkBranchAddress.AutoSize = true;
      this.chkBranchAddress.Location = new Point(12, 54);
      this.chkBranchAddress.Name = "chkBranchAddress";
      this.chkBranchAddress.Size = new Size(1009, 23);
      this.chkBranchAddress.TabIndex = 1;
      this.chkBranchAddress.Text = "Use your company's branch address in the borrower consent agreement for electronic document requests and eDisclosure packages.";
      this.chkBranchAddress.UseVisualStyleBackColor = true;
      this.chkBranchAddress.Click += new EventHandler(this.chkBranchAddress_Click);
      this.gcSigning.Controls.Add((Control) this.chkSigningOrderDisclosure);
      this.gcSigning.Controls.Add((Control) this.btnStates);
      this.gcSigning.Controls.Add((Control) this.chkHELOC);
      this.gcSigning.Controls.Add((Control) this.chkOther);
      this.gcSigning.Controls.Add((Control) this.chkUSDA);
      this.gcSigning.Controls.Add((Control) this.chkVA);
      this.gcSigning.Controls.Add((Control) this.chkFHA);
      this.gcSigning.Controls.Add((Control) this.chkConventional);
      this.gcSigning.Controls.Add((Control) this.chkElectronicSigning);
      this.gcSigning.HeaderForeColor = SystemColors.ControlText;
      this.gcSigning.Location = new Point(0, 88);
      this.gcSigning.Name = "gcSigning";
      this.gcSigning.Size = new Size(851, 88);
      this.gcSigning.TabIndex = 2;
      this.gcSigning.Text = "Borrower Signing";
      this.chkSigningOrderDisclosure.AutoSize = true;
      this.chkSigningOrderDisclosure.Location = new Point(12, 61);
      this.chkSigningOrderDisclosure.Name = "chkSigningOrderDisclosure";
      this.chkSigningOrderDisclosure.Size = new Size(370, 23);
      this.chkSigningOrderDisclosure.TabIndex = 10;
      this.chkSigningOrderDisclosure.Text = "Configure signing order with initial disclosures";
      this.chkSigningOrderDisclosure.UseVisualStyleBackColor = true;
      this.chkSigningOrderDisclosure.Click += new EventHandler(this.chkSigningOrderDisclosure_Click);
      this.btnStates.Location = new Point(272, 61);
      this.btnStates.Name = "btnStates";
      this.btnStates.Size = new Size(86, 22);
      this.btnStates.TabIndex = 11;
      this.btnStates.Text = "Select States";
      this.btnStates.UseVisualStyleBackColor = true;
      this.btnStates.Click += new EventHandler(this.btnStates_Click);
      this.chkHELOC.AutoSize = true;
      this.chkHELOC.Location = new Point(528, 36);
      this.chkHELOC.Name = "chkHELOC";
      this.chkHELOC.Size = new Size(90, 23);
      this.chkHELOC.TabIndex = 9;
      this.chkHELOC.Text = "HELOC";
      this.chkHELOC.UseVisualStyleBackColor = true;
      this.chkHELOC.Click += new EventHandler(this.chkHELOC_Click);
      this.chkOther.AutoSize = true;
      this.chkOther.Location = new Point(472, 36);
      this.chkOther.Name = "chkOther";
      this.chkOther.Size = new Size(75, 23);
      this.chkOther.TabIndex = 8;
      this.chkOther.Text = "Other";
      this.chkOther.UseVisualStyleBackColor = true;
      this.chkOther.Click += new EventHandler(this.chkOther_Click);
      this.chkUSDA.AutoSize = true;
      this.chkUSDA.Location = new Point(392, 36);
      this.chkUSDA.Name = "chkUSDA";
      this.chkUSDA.Size = new Size(119, 23);
      this.chkUSDA.TabIndex = 7;
      this.chkUSDA.Text = "USDA-RHS";
      this.chkUSDA.UseVisualStyleBackColor = true;
      this.chkUSDA.Click += new EventHandler(this.chkUSDA_Click);
      this.chkVA.AutoSize = true;
      this.chkVA.Location = new Point(348, 36);
      this.chkVA.Name = "chkVA";
      this.chkVA.Size = new Size(56, 23);
      this.chkVA.TabIndex = 6;
      this.chkVA.Text = "VA";
      this.chkVA.UseVisualStyleBackColor = true;
      this.chkVA.Click += new EventHandler(this.chkVA_Click);
      this.chkFHA.AutoSize = true;
      this.chkFHA.Location = new Point(300, 36);
      this.chkFHA.Name = "chkFHA";
      this.chkFHA.Size = new Size(67, 23);
      this.chkFHA.TabIndex = 5;
      this.chkFHA.Text = "FHA";
      this.chkFHA.UseVisualStyleBackColor = true;
      this.chkFHA.Click += new EventHandler(this.chkFHA_Click);
      this.chkConventional.AutoSize = true;
      this.chkConventional.Location = new Point(248, 36);
      this.chkConventional.Name = "chkConventional";
      this.chkConventional.Size = new Size(72, 23);
      this.chkConventional.TabIndex = 4;
      this.chkConventional.Text = "Conv";
      this.chkConventional.UseVisualStyleBackColor = true;
      this.chkConventional.Click += new EventHandler(this.chkConventional_Click);
      this.chkElectronicSigning.AutoSize = true;
      this.chkElectronicSigning.Location = new Point(12, 36);
      this.chkElectronicSigning.Name = "chkElectronicSigning";
      this.chkElectronicSigning.Size = new Size(350, 23);
      this.chkElectronicSigning.TabIndex = 0;
      this.chkElectronicSigning.Text = "Provide eSigning option - Select Loan Type";
      this.chkElectronicSigning.UseVisualStyleBackColor = true;
      this.chkElectronicSigning.Click += new EventHandler(this.chkElectronicSigning_Click);
      this.gcComplianceReviewReport.Controls.Add((Control) this.chkIncludeComplianceReport);
      this.gcComplianceReviewReport.HeaderForeColor = SystemColors.ControlText;
      this.gcComplianceReviewReport.Location = new Point(0, 0);
      this.gcComplianceReviewReport.Name = "gcComplianceReviewReport";
      this.gcComplianceReviewReport.Size = new Size(855, 88);
      this.gcComplianceReviewReport.TabIndex = 2;
      this.gcComplianceReviewReport.Text = "Compliance Review Report";
      this.chkIncludeComplianceReport.AutoSize = true;
      this.chkIncludeComplianceReport.Location = new Point(12, 36);
      this.chkIncludeComplianceReport.Name = "chkIncludeComplianceReport";
      this.chkIncludeComplianceReport.Size = new Size(762, 23);
      this.chkIncludeComplianceReport.TabIndex = 0;
      this.chkIncludeComplianceReport.Text = "Include Compliance Report With Data Audit (applies only to disclosures sent from Encompass Web)";
      this.chkIncludeComplianceReport.UseVisualStyleBackColor = true;
      this.chkIncludeComplianceReport.Click += new EventHandler(this.chkIncludeComplianceReport_Click);
      this.pnlPackages.Controls.Add((Control) this.gcPackages);
      this.pnlPackages.Dock = DockStyle.Fill;
      this.pnlPackages.Location = new Point(0, 0);
      this.pnlPackages.Name = "pnlPackages";
      this.pnlPackages.Size = new Size(1114, 362);
      this.pnlPackages.TabIndex = 3;
      this.gcPackages.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcPackages.Controls.Add((Control) this.pnlDefaultChannel);
      this.gcPackages.Controls.Add((Control) this.tabPackages);
      this.gcPackages.Dock = DockStyle.Fill;
      this.gcPackages.HeaderForeColor = SystemColors.ControlText;
      this.gcPackages.Location = new Point(0, 0);
      this.gcPackages.Name = "gcPackages";
      this.gcPackages.Padding = new Padding(8, 4, 6, 2);
      this.gcPackages.Size = new Size(1114, 362);
      this.gcPackages.TabIndex = 1;
      this.gcPackages.Text = "ICE Mortgage Technology eDisclosure Packages";
      this.pnlDefaultChannel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlDefaultChannel.Controls.Add((Control) this.cboDefaultChannel);
      this.pnlDefaultChannel.Controls.Add((Control) this.lblDefaultChannel);
      this.pnlDefaultChannel.Location = new Point(805, 30);
      this.pnlDefaultChannel.Name = "pnlDefaultChannel";
      this.pnlDefaultChannel.Size = new Size(300, 30);
      this.pnlDefaultChannel.TabIndex = 1;
      this.cboDefaultChannel.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboDefaultChannel.FormattingEnabled = true;
      this.cboDefaultChannel.Items.AddRange(new object[4]
      {
        (object) "Banker - Retail",
        (object) "Banker - Wholesale",
        (object) "Broker",
        (object) "Correspondent"
      });
      this.cboDefaultChannel.Location = new Point(152, 4);
      this.cboDefaultChannel.Name = "cboDefaultChannel";
      this.cboDefaultChannel.Size = new Size(148, 27);
      this.cboDefaultChannel.TabIndex = 1;
      this.cboDefaultChannel.SelectionChangeCommitted += new EventHandler(this.cboDefaultChannel_SelectionChangeCommitted);
      this.lblDefaultChannel.AutoSize = true;
      this.lblDefaultChannel.Location = new Point(4, 8);
      this.lblDefaultChannel.Name = "lblDefaultChannel";
      this.lblDefaultChannel.Size = new Size(218, 19);
      this.lblDefaultChannel.TabIndex = 0;
      this.lblDefaultChannel.Text = "If no channel selected, apply";
      this.tabPackages.Controls.Add((Control) this.pageRetail);
      this.tabPackages.Controls.Add((Control) this.pageWholesale);
      this.tabPackages.Controls.Add((Control) this.pageBroker);
      this.tabPackages.Controls.Add((Control) this.pageCorrespondent);
      this.tabPackages.Dock = DockStyle.Fill;
      this.tabPackages.HotTrack = true;
      this.tabPackages.ItemSize = new Size(74, 28);
      this.tabPackages.Location = new Point(9, 30);
      this.tabPackages.Margin = new Padding(0);
      this.tabPackages.Name = "tabPackages";
      this.tabPackages.Padding = new Point(11, 3);
      this.tabPackages.SelectedIndex = 0;
      this.tabPackages.Size = new Size(1098, 330);
      this.tabPackages.TabIndex = 0;
      this.pageRetail.Controls.Add((Control) this.retailChannel);
      this.pageRetail.Location = new Point(4, 32);
      this.pageRetail.Name = "pageRetail";
      this.pageRetail.Padding = new Padding(0, 2, 2, 2);
      this.pageRetail.Size = new Size(1090, 294);
      this.pageRetail.TabIndex = 0;
      this.pageRetail.Text = "Banker - Retail";
      this.pageRetail.UseVisualStyleBackColor = true;
      this.retailChannel.Dock = DockStyle.Fill;
      this.retailChannel.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.retailChannel.Location = new Point(0, 2);
      this.retailChannel.Name = "retailChannel";
      this.retailChannel.Size = new Size(1088, 290);
      this.retailChannel.TabIndex = 0;
      this.retailChannel.ConfigurationChanged += new EventHandler(this.configurationChanged);
      this.pageWholesale.Controls.Add((Control) this.wholesaleChannel);
      this.pageWholesale.Location = new Point(4, 32);
      this.pageWholesale.Name = "pageWholesale";
      this.pageWholesale.Padding = new Padding(0, 2, 2, 2);
      this.pageWholesale.Size = new Size(1090, 294);
      this.pageWholesale.TabIndex = 1;
      this.pageWholesale.Text = "Banker - Wholesale";
      this.pageWholesale.UseVisualStyleBackColor = true;
      this.wholesaleChannel.Dock = DockStyle.Fill;
      this.wholesaleChannel.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.wholesaleChannel.Location = new Point(0, 2);
      this.wholesaleChannel.Name = "wholesaleChannel";
      this.wholesaleChannel.Size = new Size(1088, 290);
      this.wholesaleChannel.TabIndex = 1;
      this.wholesaleChannel.ConfigurationChanged += new EventHandler(this.configurationChanged);
      this.pageBroker.Controls.Add((Control) this.brokerChannel);
      this.pageBroker.Location = new Point(4, 32);
      this.pageBroker.Name = "pageBroker";
      this.pageBroker.Padding = new Padding(0, 2, 2, 2);
      this.pageBroker.Size = new Size(1090, 294);
      this.pageBroker.TabIndex = 2;
      this.pageBroker.Text = "Broker";
      this.pageBroker.UseVisualStyleBackColor = true;
      this.brokerChannel.Dock = DockStyle.Fill;
      this.brokerChannel.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.brokerChannel.Location = new Point(0, 2);
      this.brokerChannel.Name = "brokerChannel";
      this.brokerChannel.Size = new Size(1088, 290);
      this.brokerChannel.TabIndex = 1;
      this.brokerChannel.ConfigurationChanged += new EventHandler(this.configurationChanged);
      this.pageCorrespondent.Controls.Add((Control) this.correspondentChannel);
      this.pageCorrespondent.Location = new Point(4, 32);
      this.pageCorrespondent.Name = "pageCorrespondent";
      this.pageCorrespondent.Padding = new Padding(0, 2, 2, 2);
      this.pageCorrespondent.Size = new Size(1090, 294);
      this.pageCorrespondent.TabIndex = 3;
      this.pageCorrespondent.Text = "Correspondent";
      this.pageCorrespondent.UseVisualStyleBackColor = true;
      this.correspondentChannel.Dock = DockStyle.Fill;
      this.correspondentChannel.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.correspondentChannel.Location = new Point(0, 2);
      this.correspondentChannel.Name = "correspondentChannel";
      this.correspondentChannel.Size = new Size(1088, 290);
      this.correspondentChannel.TabIndex = 1;
      this.correspondentChannel.ConfigurationChanged += new EventHandler(this.configurationChanged);
      this.AutoScaleDimensions = new SizeF(9f, 19f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnlPackages);
      this.Controls.Add((Control) this.pnlSigningConsent);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (EDisclosureSetupControl);
      this.Size = new Size(1114, 644);
      this.pnlSigningConsent.ResumeLayout(false);
      this.pnlSigningConsent.PerformLayout();
      this.gcAutoRetrieve.ResumeLayout(false);
      this.gcAutoRetrieve.PerformLayout();
      this.gcConsents.ResumeLayout(false);
      this.gcConsents.PerformLayout();
      this.gcSigning.ResumeLayout(false);
      this.gcSigning.PerformLayout();
      this.gcComplianceReviewReport.ResumeLayout(false);
      this.gcComplianceReviewReport.PerformLayout();
      this.pnlPackages.ResumeLayout(false);
      this.gcPackages.ResumeLayout(false);
      this.pnlDefaultChannel.ResumeLayout(false);
      this.pnlDefaultChannel.PerformLayout();
      this.tabPackages.ResumeLayout(false);
      this.pageRetail.ResumeLayout(false);
      this.pageWholesale.ResumeLayout(false);
      this.pageBroker.ResumeLayout(false);
      this.pageCorrespondent.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
