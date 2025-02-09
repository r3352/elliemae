// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.EditCompanyONRPControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement.RestApi;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class EditCompanyONRPControl : UserControl
  {
    private Sessions.Session session;
    private SessionObjects sessionObjects;
    private ExternalOriginatorManagementData externalOrg;
    private IConfigurationManager config;
    private ExternalOriginatorManagementData parent;
    private bool readOnly;
    private int dataoid;
    private int orgId;
    private bool isTpoFlag;
    private ExternalOrgOnrpSettings onrpSettings;
    private ExternalOrgOnrpSettings onrpSettingsOld;
    private bool brokerFirstTime;
    private bool correspondentFirstTime;
    private bool populateValue;
    public bool validationFailed;
    private IContainer components;
    private GroupContainer groupContainer;
    private Panel panel1;
    private GroupContainer gcCorrespondent;
    private Label lblCorrespondentPercent;
    private RadioButton rbtnCorrespondentCustom;
    private RadioButton rbtnCorrespondentGlobal;
    private ComboBox cmbCorrespondentAMPM;
    private TextBox txtCorrespondentTolerance;
    private Label lblCorrespondentTolerance;
    private TextBox txtCorrespondentDollarLimit;
    private Label lblCorrespondentDollar;
    private CheckBox chkCorrespondentNoLimit;
    private CheckBox chkCorrespondentWHCoverage;
    private Label lblCorrespondentEt;
    private ComboBox cmbCorrespondentMin;
    private ComboBox cmbCorrespondentHr;
    private Label lblCorrespondentEndTime;
    private TextBox txtCorrespondentStartTime;
    private Label lblCorrespondentStartTime;
    private RadioButton rbtnCorrespondentSpecifyTime;
    private RadioButton rbtnCorrespondentContinuous;
    private Label lblCorrespondentCoverage;
    private CheckBox chkCorrespondent;
    private GroupContainer gcBroker;
    private Label lblBrokerPercent;
    private RadioButton rbtnBrokerCustom;
    private RadioButton rbtnBrokerGlobal;
    private ComboBox cmbBrokerAMPM;
    private TextBox txtBrokerTolerance;
    private Label lblBrokerTolerance;
    private TextBox txtBrokerDollarLimit;
    private Label lblBrokerDollar;
    private CheckBox chkBrokerNoLimit;
    private CheckBox chkBrokerWHCoverage;
    private Label lblBrokerEt;
    private ComboBox cmbBrokerMin;
    private ComboBox cmbBrokerHr;
    private Label lblBrokerEndTime;
    private TextBox txtBrokerStartTime;
    private Label lblBrokerStartTime;
    private RadioButton rbtnBrokerSpecifyTime;
    private RadioButton rbtnBrokerContinuous;
    private Label lblBrokerCoverage;
    private CheckBox chkBroker;
    private StandardIconButton btnReset;
    private StandardIconButton btnSave;
    private Panel panel2;
    private Label label1;
    private Panel panel6;
    private Panel panel4;
    private Panel panel5;
    private Panel panel3;
    private ComboBox cmbCorrespondentSatAMPM;
    private Label lblCorrespondentSatEt;
    private ComboBox cmbCorrespondentSatMin;
    private ComboBox cmbCorrespondentSatHr;
    private Label lblCorrespondentSatEndTime;
    private TextBox txtCorrespondentSatStartTime;
    private Label lblCorrespondentSatStartTime;
    private CheckBox chkCorrespondentSatHours;
    private Label label2;
    private ComboBox cmbBrokerSatAMPM;
    private Label lblBrokerSatEt;
    private ComboBox cmbBrokerSatMin;
    private ComboBox cmbBrokerSatHr;
    private Label lblBrokerSatEndTime;
    private TextBox txtBrokerSatStartTime;
    private Label lblBrokerSatStartTime;
    private CheckBox chkBrokerSatHours;
    private Label lblWeekdayHours;
    private ComboBox cmbCorrespondentSunAMPM;
    private Label lblCorrespondentSunEt;
    private ComboBox cmbCorrespondentSunMin;
    private ComboBox cmbCorrespondentSunHr;
    private Label lblCorrespondentSunEndTime;
    private TextBox txtCorrespondentSunStartTime;
    private Label lblCorrespondentSunStartTime;
    private CheckBox chkCorrespondentSunHours;
    private ComboBox cmbBrokerSunAMPM;
    private Label lblBrokerSunEt;
    private ComboBox cmbBrokerSunMin;
    private ComboBox cmbBrokerSunHr;
    private Label lblBrokerSunEndTime;
    private TextBox txtBrokerSunStartTime;
    private Label lblBrokerSunStartTime;
    private CheckBox chkBrokerSunHours;
    private CheckBox chkCTAllowONRPForCancelledExpiredLocks;

    public bool IsDirty => this.btnSave.Enabled && this.btnSave.Visible;

    public bool IsBrokerEnabled => this.onrpSettings != null && this.onrpSettings.Broker.EnableONRP;

    public bool IsCorrespondentEnabled
    {
      get => this.onrpSettings != null && this.onrpSettings.Correspondent.EnableONRP;
    }

    public EditCompanyONRPControl(SessionObjects sessionObjects, int orgID, bool isTPOFlag)
    {
      this.InitializeComponent();
      this.sessionObjects = sessionObjects;
      this.config = this.sessionObjects.ConfigurationManager;
      this.formatControl();
      this.initControl(orgID, isTPOFlag);
      this.disableControls(orgID);
    }

    public EditCompanyONRPControl(Sessions.Session session, int orgID, bool isTPOFlag)
    {
      this.InitializeComponent();
      this.session = session;
      this.config = this.session.ConfigurationManager;
      this.formatControl();
      this.initControl(orgID, isTPOFlag);
      this.disableControls(orgID);
    }

    private void initControl(int orgID, bool isTPOFlag)
    {
      this.orgId = orgID;
      this.isTpoFlag = isTPOFlag;
      this.Dock = DockStyle.Fill;
      this.parent = this.config.GetRootOrganisation(false, orgID);
      this.readOnly = this.parent == null || this.parent.oid != orgID;
      if (isTPOFlag)
        this.readOnly = true;
      if (this.parent != null)
        this.dataoid = this.parent.oid;
      this.LoadData();
      bool hierarchyAccess;
      Session.TpoHierarchyAccessCache.TryGetValue(orgID, out hierarchyAccess);
      TPOClientUtils.DisableControls((UserControl) this, hierarchyAccess);
    }

    private void SetDirty(bool dirty) => this.btnSave.Enabled = this.btnReset.Enabled = dirty;

    private string[] getMinutes()
    {
      string[] minutes = new string[61];
      minutes[0] = "";
      for (int index = 0; index < 60; ++index)
        minutes[index + 1] = index < 10 ? "0" + (object) index : index.ToString();
      return minutes;
    }

    private string[] getHrs()
    {
      return new string[13]
      {
        "",
        "01",
        "02",
        "03",
        "04",
        "05",
        "06",
        "07",
        "08",
        "09",
        "10",
        "11",
        "12"
      };
    }

    private string[] getAMPM()
    {
      return new string[3]{ "", "AM", "PM" };
    }

    private bool ValidateData()
    {
      EncompassMessageHanlder messageHandler = new EncompassMessageHanlder((IWin32Window) this);
      bool resetEndTime;
      return ONRPUtils.ValidateSettings((IONRPRuleHandler) messageHandler, this.onrpSettings.Broker, out resetEndTime) && ONRPUtils.ValidateSettings((IONRPRuleHandler) messageHandler, this.onrpSettings.Correspondent, out resetEndTime);
    }

    private void CommitData()
    {
      if (this.onrpSettings == null)
        this.onrpSettings = new ExternalOrgOnrpSettings();
      this.onrpSettings.Broker.EnableONRP = this.chkBroker.Checked;
      this.onrpSettings.Broker.UseChannelDefault = this.rbtnBrokerGlobal.Checked;
      this.onrpSettings.Broker.ContinuousCoverage = !this.rbtnBrokerSpecifyTime.Checked;
      this.onrpSettings.Broker.ONRPStartTime = this.txtBrokerStartTime.Text.Replace("ET", string.Empty);
      if (this.cmbBrokerHr.SelectedItem.ToString() == "" || this.cmbBrokerMin.SelectedItem.ToString() == "" || this.cmbBrokerAMPM.SelectedItem.ToString() == "")
        this.onrpSettings.Broker.ONRPEndTime = "";
      else
        this.onrpSettings.Broker.ONRPEndTime = ONRPEntitySettings.ConverToDateTime(this.cmbBrokerHr.SelectedItem.ToString() + ":" + this.cmbBrokerMin.SelectedItem.ToString() + " " + this.cmbBrokerAMPM.SelectedItem.ToString()).ToString("HH\\:mm");
      this.onrpSettings.Broker.EnableSatONRP = this.chkBrokerSatHours.Checked;
      this.onrpSettings.Broker.EnableSunONRP = this.chkBrokerSunHours.Checked;
      this.onrpSettings.Broker.ONRPSatStartTime = this.txtBrokerSatStartTime.Text.Replace("ET", string.Empty);
      this.onrpSettings.Broker.ONRPSunStartTime = this.txtBrokerSunStartTime.Text.Replace("ET", string.Empty);
      if (this.cmbBrokerSatHr.SelectedItem.ToString() == "" || this.cmbBrokerSatMin.SelectedItem.ToString() == "" || this.cmbBrokerSatAMPM.SelectedItem.ToString() == "")
        this.onrpSettings.Broker.ONRPSatEndTime = "";
      else
        this.onrpSettings.Broker.ONRPSatEndTime = ONRPEntitySettings.ConverToDateTime(this.cmbBrokerSatHr.SelectedItem.ToString() + ":" + this.cmbBrokerSatMin.SelectedItem.ToString() + " " + this.cmbBrokerSatAMPM.SelectedItem.ToString()).ToString("HH\\:mm");
      if (this.cmbBrokerSunHr.SelectedItem.ToString() == "" || this.cmbBrokerSunMin.SelectedItem.ToString() == "" || this.cmbBrokerSunAMPM.SelectedItem.ToString() == "")
        this.onrpSettings.Broker.ONRPSunEndTime = "";
      else
        this.onrpSettings.Broker.ONRPSunEndTime = ONRPEntitySettings.ConverToDateTime(this.cmbBrokerSunHr.SelectedItem.ToString() + ":" + this.cmbBrokerSunMin.SelectedItem.ToString() + " " + this.cmbBrokerSunAMPM.SelectedItem.ToString()).ToString("HH\\:mm");
      this.onrpSettings.Broker.WeekendHolidayCoverage = this.chkBrokerWHCoverage.Checked;
      this.onrpSettings.Broker.MaximumLimit = this.chkBrokerNoLimit.Checked;
      this.onrpSettings.Broker.DollarLimit = Utils.ParseDouble((object) this.txtBrokerDollarLimit.Text, 0.0);
      this.onrpSettings.Broker.Tolerance = Utils.ParseInt((object) this.txtBrokerTolerance.Text, 0);
      this.onrpSettings.Correspondent.EnableONRP = this.chkCorrespondent.Checked;
      this.onrpSettings.Correspondent.AllowONRPForCancelledExpiredLocks = this.chkCTAllowONRPForCancelledExpiredLocks.Checked;
      this.onrpSettings.Correspondent.UseChannelDefault = this.rbtnCorrespondentGlobal.Checked;
      this.onrpSettings.Correspondent.ContinuousCoverage = !this.rbtnCorrespondentSpecifyTime.Checked;
      this.onrpSettings.Correspondent.ONRPStartTime = this.txtCorrespondentStartTime.Text.Replace("ET", string.Empty);
      if (this.cmbCorrespondentHr.SelectedItem.ToString() == "" || this.cmbCorrespondentMin.SelectedItem.ToString() == "" || this.cmbCorrespondentAMPM.SelectedItem.ToString() == "")
        this.onrpSettings.Correspondent.ONRPEndTime = "";
      else
        this.onrpSettings.Correspondent.ONRPEndTime = ONRPEntitySettings.ConverToDateTime(this.cmbCorrespondentHr.SelectedItem.ToString() + ":" + this.cmbCorrespondentMin.SelectedItem.ToString() + " " + this.cmbCorrespondentAMPM.SelectedItem.ToString()).ToString("HH\\:mm");
      this.onrpSettings.Correspondent.EnableSatONRP = this.chkCorrespondentSatHours.Checked;
      this.onrpSettings.Correspondent.EnableSunONRP = this.chkCorrespondentSunHours.Checked;
      this.onrpSettings.Correspondent.ONRPSatStartTime = this.txtCorrespondentSatStartTime.Text.Replace("ET", string.Empty);
      this.onrpSettings.Correspondent.ONRPSunStartTime = this.txtCorrespondentSunStartTime.Text.Replace("ET", string.Empty);
      if (this.cmbCorrespondentSatHr.SelectedItem.ToString() == "" || this.cmbCorrespondentSatMin.SelectedItem.ToString() == "" || this.cmbCorrespondentSatAMPM.SelectedItem.ToString() == "")
        this.onrpSettings.Correspondent.ONRPSatEndTime = "";
      else
        this.onrpSettings.Correspondent.ONRPSatEndTime = ONRPEntitySettings.ConverToDateTime(this.cmbCorrespondentSatHr.SelectedItem.ToString() + ":" + this.cmbCorrespondentSatMin.SelectedItem.ToString() + " " + this.cmbCorrespondentSatAMPM.SelectedItem.ToString()).ToString("HH\\:mm");
      if (this.cmbCorrespondentSunHr.SelectedItem.ToString() == "" || this.cmbCorrespondentSunMin.SelectedItem.ToString() == "" || this.cmbCorrespondentSunAMPM.SelectedItem.ToString() == "")
        this.onrpSettings.Correspondent.ONRPSunEndTime = "";
      else
        this.onrpSettings.Correspondent.ONRPSunEndTime = ONRPEntitySettings.ConverToDateTime(this.cmbCorrespondentSunHr.SelectedItem.ToString() + ":" + this.cmbCorrespondentSunMin.SelectedItem.ToString() + " " + this.cmbCorrespondentSunAMPM.SelectedItem.ToString()).ToString("HH\\:mm");
      this.onrpSettings.Correspondent.WeekendHolidayCoverage = this.chkCorrespondentWHCoverage.Checked;
      this.onrpSettings.Correspondent.MaximumLimit = this.chkCorrespondentNoLimit.Checked;
      this.onrpSettings.Correspondent.DollarLimit = (double) Utils.ParseInt((object) this.txtCorrespondentDollarLimit.Text, 0);
      this.onrpSettings.Correspondent.Tolerance = Utils.ParseInt((object) this.txtCorrespondentTolerance.Text, 0);
    }

    public void LoadData()
    {
      this.externalOrg = this.config.GetExternalOrganization(false, this.orgId);
      if (this.externalOrg == null || this.externalOrg.Parent > 0)
      {
        this.DisableControls();
        this.PerformDisableBroker();
        this.PerformDisableCorrespondent();
      }
      else
      {
        this.onrpSettings = this.config.GetExternalOrgOnrpSettings(this.dataoid);
        IDictionary serverSettings = Session.ServerManager.GetServerSettings("Policies");
        if (this.onrpSettings.ONRPID == -1)
        {
          this.brokerFirstTime = true;
          this.onrpSettings.Broker = ONRPSettingFactory.GetWholesaleUISetting(serverSettings);
          this.onrpSettings.Broker.UseChannelDefault = true;
        }
        else
          this.onrpSettings.Broker.SetRules((IONRPRuleHandler) null, (ONRPBaseRule) new ONRPSettingRules(), new LockDeskGlobalSettings(serverSettings, LoanChannel.BankedWholesale));
        if (this.onrpSettings.ONRPID == -1)
        {
          this.correspondentFirstTime = true;
          this.onrpSettings.Correspondent = ONRPSettingFactory.GetCorrespondentUISetting(serverSettings);
          this.onrpSettings.Correspondent.UseChannelDefault = true;
        }
        else
          this.onrpSettings.Correspondent.SetRules((IONRPRuleHandler) null, (ONRPBaseRule) new ONRPSettingRules(), new LockDeskGlobalSettings(serverSettings, LoanChannel.Correspondent));
        this.onrpSettingsOld = this.onrpSettings.Clone();
        this.SetBrokerViewStates(this.onrpSettings.Broker);
        this.PopulateBrokerValues(this.onrpSettings.Broker);
        this.SetCorrespondentViewStates(this.onrpSettings.Correspondent);
        this.PopulateCorrespondentValues(this.onrpSettings.Correspondent);
        if (this.readOnly)
          this.Enabled = false;
        this.SetDirty(false);
      }
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to reset? All changes will be lost.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return;
      this.initControl(this.orgId, this.isTpoFlag);
      this.SetDirty(false);
    }

    private void dataChanged(object sender, EventArgs e) => this.SetDirty(true);

    public bool PerformSave()
    {
      if (!this.IsDirty)
        return false;
      this.CommitData();
      if (!this.ValidateData())
        return false;
      this.config.UpdateOnrpSettings(this.onrpSettings, this.dataoid, this.onrpSettingsOld);
      WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.orgId);
      this.LoadData();
      this.SetDirty(false);
      return true;
    }

    private void btnSave_Click(object sender, EventArgs e) => this.PerformSave();

    private void chkBroker_CheckedChanged(object sender, EventArgs e)
    {
      this.onrpSettings.Broker.EnableONRP = this.chkBroker.Checked;
      this.SetBrokerViewStates(this.onrpSettings.Broker);
      this.dataChanged(sender, e);
    }

    private void chkCorrespondent_CheckedChanged(object sender, EventArgs e)
    {
      this.onrpSettings.Correspondent.EnableONRP = this.chkCorrespondent.Checked;
      this.SetCorrespondentViewStates(this.onrpSettings.Correspondent);
      this.dataChanged(sender, e);
    }

    private void ToggleSpecifyTimes(object sender, EventArgs e) => this.dataChanged(sender, e);

    private void chkBrokerNoLimit_CheckedChanged(object sender, EventArgs e)
    {
      this.onrpSettings.Broker.MaximumLimit = this.chkBrokerNoLimit.Checked;
      this.SetBrokerViewStates(this.onrpSettings.Broker);
      this.PopulateBrokerValues(this.onrpSettings.Broker);
      this.dataChanged(sender, e);
    }

    private void chkCorrespondentNoLimit_CheckedChanged(object sender, EventArgs e)
    {
      this.onrpSettings.Correspondent.MaximumLimit = this.chkCorrespondentNoLimit.Checked;
      this.SetCorrespondentViewStates(this.onrpSettings.Correspondent);
      this.PopulateCorrespondentValues(this.onrpSettings.Correspondent);
      this.dataChanged(sender, e);
    }

    private void groupContainer_SizeChanged(object sender, EventArgs e)
    {
      this.panel1.Width = this.groupContainer.Width - 12;
      this.gcBroker.Width = this.gcCorrespondent.Width = (this.panel1.Width - 6) / 2;
    }

    private void ResetEndTime(DateTime endTime, LoanChannel channel, string time)
    {
      ComboBox comboBox1 = this.cmbBrokerHr;
      ComboBox comboBox2 = this.cmbBrokerMin;
      ComboBox comboBox3 = this.cmbBrokerAMPM;
      if (channel == LoanChannel.BankedWholesale && time.ToLower() == "sat")
      {
        comboBox1 = this.cmbBrokerSatHr;
        comboBox2 = this.cmbBrokerSatMin;
        comboBox3 = this.cmbBrokerSatAMPM;
      }
      else if (channel == LoanChannel.BankedWholesale && time.ToLower() == "sun")
      {
        comboBox1 = this.cmbBrokerSunHr;
        comboBox2 = this.cmbBrokerSunMin;
        comboBox3 = this.cmbBrokerSunAMPM;
      }
      else if (channel == LoanChannel.Correspondent && time.ToLower() == "weekday")
      {
        comboBox1 = this.cmbCorrespondentHr;
        comboBox2 = this.cmbCorrespondentMin;
        comboBox3 = this.cmbCorrespondentAMPM;
      }
      else if (channel == LoanChannel.Correspondent && time.ToLower() == "sat")
      {
        comboBox1 = this.cmbCorrespondentSatHr;
        comboBox2 = this.cmbCorrespondentSatMin;
        comboBox3 = this.cmbCorrespondentSatAMPM;
      }
      else if (channel == LoanChannel.Correspondent && time.ToLower() == "sun")
      {
        comboBox1 = this.cmbCorrespondentSunHr;
        comboBox2 = this.cmbCorrespondentSunMin;
        comboBox3 = this.cmbCorrespondentSunAMPM;
      }
      if (endTime != DateTime.MinValue)
      {
        comboBox1.SelectedIndex = int.Parse(endTime.ToString("hh"));
        comboBox2.SelectedIndex = int.Parse(endTime.ToString("mm")) + 1;
        comboBox3.SelectedIndex = endTime.ToString("tt") == "AM" ? 1 : 2;
      }
      else
      {
        comboBox1.SelectedIndex = 0;
        comboBox2.SelectedIndex = 0;
        comboBox3.SelectedIndex = 0;
      }
    }

    private void SetBrokerViewStates(ONRPEntitySettings settings)
    {
      this.txtBrokerStartTime.Enabled = false;
      bool flag = (this.externalOrg.entityType == ExternalOriginatorEntityType.Both || this.externalOrg.entityType == ExternalOriginatorEntityType.Broker) && settings.rules.EnableONRP;
      if (this.populateValue)
        return;
      this.chkBroker.Enabled = settings.rules.EnableONRP & flag;
      this.rbtnBrokerGlobal.Enabled = settings.rules.UseChannelDefaultsAndCustomize & flag;
      this.rbtnBrokerCustom.Enabled = settings.rules.UseChannelDefaultsAndCustomize & flag;
      this.rbtnBrokerContinuous.Enabled = settings.rules.ContinuousCoverageAndSpecifyTime & flag;
      this.rbtnBrokerSpecifyTime.Enabled = settings.rules.ContinuousCoverageAndSpecifyTime & flag;
      this.cmbBrokerHr.Enabled = settings.rules.EndTime & flag;
      this.cmbBrokerMin.Enabled = settings.rules.EndTime & flag;
      this.cmbBrokerAMPM.Enabled = settings.rules.EndTime & flag;
      this.chkBrokerWHCoverage.Enabled = settings.rules.WeekendHolidayCoverage & flag;
      this.chkBrokerNoLimit.Enabled = settings.rules.NoMaxLimit & flag;
      this.txtBrokerDollarLimit.Enabled = settings.rules.DollarLimit & flag;
      this.txtBrokerTolerance.Enabled = settings.rules.Tolerance & flag;
      this.chkBrokerSatHours.Enabled = settings.rules.EnableSat & flag;
      this.cmbBrokerSatMin.Enabled = settings.rules.SatEndTime & flag;
      this.cmbBrokerSatAMPM.Enabled = settings.rules.SatEndTime & flag;
      this.cmbBrokerSatHr.Enabled = settings.rules.SatEndTime & flag;
      this.chkBrokerSunHours.Enabled = settings.rules.EnableSun & flag;
      this.cmbBrokerSunHr.Enabled = settings.rules.SunEndTime & flag;
      this.cmbBrokerSunMin.Enabled = settings.rules.SunEndTime & flag;
      this.cmbBrokerSunAMPM.Enabled = settings.rules.SunEndTime & flag;
    }

    private void SetCorrespondentViewStates(ONRPEntitySettings settings)
    {
      this.txtBrokerStartTime.Enabled = false;
      bool flag = (this.externalOrg.entityType == ExternalOriginatorEntityType.Both || this.externalOrg.entityType == ExternalOriginatorEntityType.Correspondent) && settings.rules.EnableONRP;
      if (this.populateValue)
        return;
      this.chkCorrespondent.Enabled = settings.rules.EnableONRP & flag;
      this.rbtnCorrespondentGlobal.Enabled = settings.rules.UseChannelDefaultsAndCustomize & flag;
      this.rbtnCorrespondentCustom.Enabled = settings.rules.UseChannelDefaultsAndCustomize & flag;
      this.chkCTAllowONRPForCancelledExpiredLocks.Enabled = ((!settings.EnableONRP ? 0 : (Utils.ParseBoolean((object) settings.GlobalSettings.AllowONRPForCancelledExpiredLocks) ? 1 : 0)) & (flag ? 1 : 0)) != 0;
      this.rbtnCorrespondentContinuous.Enabled = settings.rules.ContinuousCoverageAndSpecifyTime & flag;
      this.rbtnCorrespondentSpecifyTime.Enabled = settings.rules.ContinuousCoverageAndSpecifyTime & flag;
      this.cmbCorrespondentHr.Enabled = settings.rules.EndTime & flag;
      this.cmbCorrespondentMin.Enabled = settings.rules.EndTime & flag;
      this.cmbCorrespondentAMPM.Enabled = settings.rules.EndTime & flag;
      this.chkCorrespondentWHCoverage.Enabled = settings.rules.WeekendHolidayCoverage & flag;
      this.chkCorrespondentNoLimit.Enabled = settings.rules.NoMaxLimit & flag;
      this.txtCorrespondentDollarLimit.Enabled = settings.rules.DollarLimit & flag;
      this.txtCorrespondentTolerance.Enabled = settings.rules.Tolerance & flag;
      this.chkCorrespondentSatHours.Enabled = settings.rules.EnableSat & flag;
      this.cmbCorrespondentSatHr.Enabled = settings.rules.SatEndTime & flag;
      this.cmbCorrespondentSatMin.Enabled = settings.rules.SatEndTime & flag;
      this.cmbCorrespondentSatAMPM.Enabled = settings.rules.SatEndTime & flag;
      this.chkCorrespondentSunHours.Enabled = settings.rules.EnableSun & flag;
      this.cmbCorrespondentSunHr.Enabled = settings.rules.SunEndTime & flag;
      this.cmbCorrespondentSunMin.Enabled = settings.rules.SunEndTime & flag;
      this.cmbCorrespondentSunAMPM.Enabled = settings.rules.SunEndTime & flag;
    }

    private void PopulateCorrespondentValues(ONRPEntitySettings settings)
    {
      this.chkCorrespondent.Checked = settings.EnableONRP;
      this.rbtnCorrespondentGlobal.Checked = settings.UseChannelDefault;
      this.rbtnCorrespondentCustom.Checked = !settings.UseChannelDefault;
      this.rbtnCorrespondentContinuous.Checked = settings.ContinuousCoverage;
      this.rbtnCorrespondentSpecifyTime.Checked = !settings.ContinuousCoverage;
      this.chkCTAllowONRPForCancelledExpiredLocks.Checked = settings.AllowONRPForCancelledExpiredLocks;
      TextBox correspondentStartTime = this.txtCorrespondentStartTime;
      DateTime endTime;
      string str1;
      if (string.IsNullOrEmpty(settings.ONRPStartTime))
      {
        str1 = "";
      }
      else
      {
        endTime = ONRPEntitySettings.ConverToDateTime(settings.ONRPStartTime);
        str1 = endTime.ToString("hh\\:mm tt") + " ET";
      }
      correspondentStartTime.Text = str1;
      if (!string.IsNullOrEmpty(settings.ONRPEndTime))
        this.ResetEndTime(DateTime.Parse(settings.ONRPEndTime), LoanChannel.Correspondent, "weekday");
      if (settings.ContinuousCoverage)
      {
        endTime = new DateTime();
        this.ResetEndTime(endTime, LoanChannel.Correspondent, "weekday");
      }
      this.chkCorrespondentNoLimit.Checked = settings.MaximumLimit;
      this.txtCorrespondentTolerance.Enabled = !this.chkCorrespondentNoLimit.Checked && this.chkCorrespondentNoLimit.Enabled;
      this.txtCorrespondentTolerance.Text = settings.Tolerance == 0 || this.chkCorrespondentNoLimit.Checked ? "" : settings.Tolerance.ToString();
      this.txtCorrespondentDollarLimit.Enabled = !this.chkCorrespondentNoLimit.Checked && this.chkCorrespondentNoLimit.Enabled;
      this.txtCorrespondentDollarLimit.Text = settings.DollarLimit == 0.0 || this.chkCorrespondentNoLimit.Checked ? "" : settings.DollarLimit.ToString("#,##0");
      this.chkCorrespondentWHCoverage.Checked = settings.WeekendHolidayCoverage && !settings.EnableSatONRP && !settings.EnableSunONRP && !settings.ContinuousCoverage && !(settings.GlobalSettings.EnableLockDeskSat.ToLower() == "true") && !(settings.GlobalSettings.EnableLockDeskSun.ToLower() == "true");
      this.chkCorrespondentSatHours.Checked = settings.EnableSatONRP && !settings.rules.TwentyfourhrSat && !settings.ContinuousCoverage && settings.rules.EnableSatLD;
      this.chkCorrespondentSunHours.Checked = settings.EnableSunONRP && !settings.rules.TwentyfourhrSun && !settings.ContinuousCoverage && settings.rules.EnableSunLD;
      TextBox correspondentSatStartTime = this.txtCorrespondentSatStartTime;
      string str2;
      if (string.IsNullOrEmpty(settings.ONRPSatStartTime))
      {
        str2 = "";
      }
      else
      {
        endTime = ONRPEntitySettings.ConverToDateTime(settings.ONRPSatStartTime);
        str2 = endTime.ToString("hh\\:mm tt") + " ET";
      }
      correspondentSatStartTime.Text = str2;
      if (!string.IsNullOrEmpty(settings.ONRPSatEndTime))
        this.ResetEndTime(DateTime.Parse(settings.ONRPSatEndTime), LoanChannel.Correspondent, "sat");
      if (!settings.EnableSatONRP)
      {
        endTime = new DateTime();
        this.ResetEndTime(endTime, LoanChannel.Correspondent, "sat");
      }
      this.chkCorrespondentSatHours.Checked = settings.EnableSatONRP;
      TextBox correspondentSunStartTime = this.txtCorrespondentSunStartTime;
      string str3;
      if (string.IsNullOrEmpty(settings.ONRPSunStartTime))
      {
        str3 = "";
      }
      else
      {
        endTime = ONRPEntitySettings.ConverToDateTime(settings.ONRPSunStartTime);
        str3 = endTime.ToString("hh\\:mm tt") + " ET";
      }
      correspondentSunStartTime.Text = str3;
      if (!string.IsNullOrEmpty(settings.ONRPSunEndTime))
        this.ResetEndTime(DateTime.Parse(settings.ONRPSunEndTime), LoanChannel.Correspondent, "sun");
      if (!settings.EnableSunONRP)
      {
        endTime = new DateTime();
        this.ResetEndTime(endTime, LoanChannel.Correspondent, "sun");
      }
      this.chkCorrespondentSunHours.Checked = settings.EnableSunONRP;
    }

    private void PopulateBrokerValues(ONRPEntitySettings settings)
    {
      this.chkBroker.Checked = settings.EnableONRP;
      this.rbtnBrokerGlobal.Checked = settings.UseChannelDefault;
      this.rbtnBrokerCustom.Checked = !settings.UseChannelDefault;
      this.rbtnBrokerContinuous.Checked = settings.ContinuousCoverage;
      this.rbtnBrokerSpecifyTime.Checked = !settings.ContinuousCoverage;
      TextBox txtBrokerStartTime = this.txtBrokerStartTime;
      DateTime endTime;
      string str1;
      if (string.IsNullOrEmpty(settings.ONRPStartTime))
      {
        str1 = "";
      }
      else
      {
        endTime = ONRPEntitySettings.ConverToDateTime(settings.ONRPStartTime);
        str1 = endTime.ToString("hh\\:mm tt") + " ET";
      }
      txtBrokerStartTime.Text = str1;
      if (!string.IsNullOrEmpty(settings.ONRPEndTime))
        this.ResetEndTime(DateTime.Parse(settings.ONRPEndTime), LoanChannel.BankedWholesale, "weekday");
      if (settings.ContinuousCoverage)
      {
        endTime = new DateTime();
        this.ResetEndTime(endTime, LoanChannel.BankedWholesale, "weekday");
      }
      this.chkBrokerNoLimit.Checked = settings.MaximumLimit;
      this.txtBrokerTolerance.Enabled = !this.chkBrokerNoLimit.Checked && this.chkBrokerNoLimit.Enabled;
      this.txtBrokerTolerance.Text = settings.Tolerance == 0 || this.chkBrokerNoLimit.Checked ? "" : settings.Tolerance.ToString();
      this.txtBrokerDollarLimit.Enabled = !this.chkBrokerNoLimit.Checked && this.chkBrokerNoLimit.Enabled;
      this.txtBrokerDollarLimit.Text = settings.DollarLimit == 0.0 || this.chkBrokerNoLimit.Checked ? "" : settings.DollarLimit.ToString("#,##0");
      this.chkBrokerWHCoverage.Checked = settings.WeekendHolidayCoverage && !settings.EnableSatONRP && !settings.EnableSunONRP && !settings.ContinuousCoverage && !(settings.GlobalSettings.EnableLockDeskSat.ToLower() == "true") && !(settings.GlobalSettings.EnableLockDeskSun.ToLower() == "true");
      TextBox brokerSatStartTime = this.txtBrokerSatStartTime;
      string str2;
      if (string.IsNullOrEmpty(settings.ONRPSatStartTime))
      {
        str2 = "";
      }
      else
      {
        endTime = ONRPEntitySettings.ConverToDateTime(settings.ONRPSatStartTime);
        str2 = endTime.ToString("hh\\:mm tt") + " ET";
      }
      brokerSatStartTime.Text = str2;
      if (!string.IsNullOrEmpty(settings.ONRPSatEndTime))
        this.ResetEndTime(DateTime.Parse(settings.ONRPSatEndTime), LoanChannel.BankedWholesale, "sat");
      if (!settings.EnableSatONRP)
      {
        endTime = new DateTime();
        this.ResetEndTime(endTime, LoanChannel.BankedWholesale, "sat");
      }
      this.chkBrokerSatHours.Checked = settings.EnableSatONRP;
      TextBox brokerSunStartTime = this.txtBrokerSunStartTime;
      string str3;
      if (string.IsNullOrEmpty(settings.ONRPSunStartTime))
      {
        str3 = "";
      }
      else
      {
        endTime = ONRPEntitySettings.ConverToDateTime(settings.ONRPSunStartTime);
        str3 = endTime.ToString("hh\\:mmtt") + " ET";
      }
      brokerSunStartTime.Text = str3;
      if (!string.IsNullOrEmpty(settings.ONRPSunEndTime))
        this.ResetEndTime(DateTime.Parse(settings.ONRPSunEndTime), LoanChannel.BankedWholesale, "sun");
      if (!settings.EnableSunONRP)
      {
        endTime = new DateTime();
        this.ResetEndTime(endTime, LoanChannel.BankedWholesale, "sun");
      }
      this.chkBrokerSunHours.Checked = settings.EnableSunONRP;
    }

    private void ToggleBrokerGlobalCustomize(object sender, EventArgs e)
    {
      this.onrpSettings.Broker.UseChannelDefault = this.rbtnBrokerGlobal.Checked;
      this.SetBrokerViewStates(this.onrpSettings.Broker);
      this.PopulateBrokerValues(this.onrpSettings.Broker);
      if (this.brokerFirstTime)
      {
        this.onrpSettings.Broker.MaximumLimit = false;
        this.onrpSettings.Broker.DollarLimit = 0.0;
        this.onrpSettings.Broker.Tolerance = 0;
        this.rbtnBrokerContinuous.Checked = false;
        this.ToggleBrokerContinuous(sender, e);
      }
      this.dataChanged(sender, e);
    }

    private void ToggleBrokerContinuous(object sender, EventArgs e)
    {
      this.onrpSettings.Broker.ContinuousCoverage = this.rbtnBrokerContinuous.Checked;
      this.SetBrokerViewStates(this.onrpSettings.Broker);
      this.PopulateBrokerValues(this.onrpSettings.Broker);
      this.dataChanged(sender, e);
    }

    private void ToggleCorrespondentGlobalCustomize(object sender, EventArgs e)
    {
      this.onrpSettings.Correspondent.UseChannelDefault = this.rbtnCorrespondentGlobal.Checked;
      this.SetCorrespondentViewStates(this.onrpSettings.Correspondent);
      this.PopulateCorrespondentValues(this.onrpSettings.Correspondent);
      if (this.correspondentFirstTime)
      {
        this.onrpSettings.Correspondent.MaximumLimit = false;
        this.onrpSettings.Correspondent.DollarLimit = 0.0;
        this.onrpSettings.Correspondent.Tolerance = 0;
        this.rbtnCorrespondentContinuous.Checked = false;
        this.ToggleCorrespondentContinuous(sender, e);
      }
      this.dataChanged(sender, e);
    }

    private void ToggleCorrespondentContinuous(object sender, EventArgs e)
    {
      this.onrpSettings.Correspondent.ContinuousCoverage = this.rbtnCorrespondentContinuous.Checked;
      this.SetCorrespondentViewStates(this.onrpSettings.Correspondent);
      this.PopulateCorrespondentValues(this.onrpSettings.Correspondent);
      this.dataChanged(sender, e);
    }

    public void DisableControls()
    {
      this.btnSave.Visible = false;
      this.btnReset.Visible = false;
      this.chkBroker.Enabled = false;
      this.chkCorrespondent.Enabled = false;
    }

    public void PerformDisableBroker()
    {
      this.chkBroker.Checked = false;
      this.DisableBrokerSettings();
    }

    public void PerformDisableCorrespondent()
    {
      this.chkCorrespondent.Checked = false;
      this.DisableCorrespondentSettings();
    }

    private void formatControl()
    {
      this.cmbBrokerHr.DataSource = (object) this.getHrs();
      this.cmbBrokerMin.DataSource = (object) this.getMinutes();
      this.cmbBrokerAMPM.DataSource = (object) this.getAMPM();
      this.cmbBrokerSatHr.DataSource = (object) this.getHrs();
      this.cmbBrokerSatMin.DataSource = (object) this.getMinutes();
      this.cmbBrokerSatAMPM.DataSource = (object) this.getAMPM();
      this.cmbBrokerSunHr.DataSource = (object) this.getHrs();
      this.cmbBrokerSunMin.DataSource = (object) this.getMinutes();
      this.cmbBrokerSunAMPM.DataSource = (object) this.getAMPM();
      this.cmbCorrespondentHr.DataSource = (object) this.getHrs();
      this.cmbCorrespondentMin.DataSource = (object) this.getMinutes();
      this.cmbCorrespondentAMPM.DataSource = (object) this.getAMPM();
      this.cmbCorrespondentSatHr.DataSource = (object) this.getHrs();
      this.cmbCorrespondentSatMin.DataSource = (object) this.getMinutes();
      this.cmbCorrespondentSatAMPM.DataSource = (object) this.getAMPM();
      this.cmbCorrespondentSunHr.DataSource = (object) this.getHrs();
      this.cmbCorrespondentSunMin.DataSource = (object) this.getMinutes();
      this.cmbCorrespondentSunAMPM.DataSource = (object) this.getAMPM();
      TextBoxFormatter.Attach(this.txtBrokerDollarLimit, TextBoxContentRule.NonNegativeInteger, "#,##0");
      TextBoxFormatter.Attach(this.txtBrokerTolerance, TextBoxContentRule.NonNegativeInteger, "#0");
      TextBoxFormatter.Attach(this.txtCorrespondentDollarLimit, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      TextBoxFormatter.Attach(this.txtCorrespondentTolerance, TextBoxContentRule.NonNegativeDecimal, "#0");
    }

    private void DisableBrokerSettings()
    {
      this.rbtnBrokerGlobal.Enabled = false;
      this.rbtnBrokerCustom.Enabled = false;
      this.DisableCustomizeBroker();
    }

    private void DisableCustomizeBroker()
    {
      this.rbtnBrokerContinuous.Enabled = false;
      this.rbtnBrokerSpecifyTime.Enabled = false;
      this.txtBrokerStartTime.Enabled = false;
      this.cmbBrokerHr.Enabled = false;
      this.cmbBrokerMin.Enabled = false;
      this.cmbBrokerAMPM.Enabled = false;
      this.chkBrokerNoLimit.Enabled = false;
      this.txtBrokerDollarLimit.Enabled = false;
      this.txtBrokerTolerance.Enabled = false;
      this.chkBrokerWHCoverage.Enabled = false;
      this.lblBrokerCoverage.Enabled = false;
      this.lblBrokerDollar.Enabled = false;
      this.lblBrokerEndTime.Enabled = false;
      this.lblBrokerPercent.Enabled = false;
      this.lblBrokerStartTime.Enabled = false;
      this.lblBrokerTolerance.Enabled = false;
      this.lblBrokerEt.Enabled = false;
      this.chkBrokerSatHours.Enabled = false;
      this.cmbBrokerSatHr.Enabled = false;
      this.cmbBrokerSatMin.Enabled = false;
      this.cmbBrokerSatAMPM.Enabled = false;
      this.chkBrokerSunHours.Enabled = false;
      this.cmbBrokerSunHr.Enabled = false;
      this.cmbBrokerSunMin.Enabled = false;
      this.cmbBrokerSunAMPM.Enabled = false;
    }

    private void DisableCorrespondentSettings()
    {
      this.rbtnCorrespondentGlobal.Enabled = false;
      this.rbtnCorrespondentCustom.Enabled = false;
      this.DisableCustomizeCorrespondent();
    }

    private void DisableCustomizeCorrespondent()
    {
      this.rbtnCorrespondentContinuous.Enabled = false;
      this.rbtnCorrespondentSpecifyTime.Enabled = false;
      this.txtCorrespondentStartTime.Enabled = false;
      this.cmbCorrespondentHr.Enabled = false;
      this.cmbCorrespondentMin.Enabled = false;
      this.cmbCorrespondentAMPM.Enabled = false;
      this.chkCorrespondentNoLimit.Enabled = false;
      this.txtCorrespondentDollarLimit.Enabled = false;
      this.txtCorrespondentTolerance.Enabled = false;
      this.chkCorrespondentWHCoverage.Enabled = false;
      this.lblCorrespondentCoverage.Enabled = false;
      this.lblCorrespondentDollar.Enabled = false;
      this.lblCorrespondentEndTime.Enabled = false;
      this.lblCorrespondentPercent.Enabled = false;
      this.lblCorrespondentStartTime.Enabled = false;
      this.lblCorrespondentTolerance.Enabled = false;
      this.lblCorrespondentEt.Enabled = false;
      this.chkCorrespondentSatHours.Enabled = false;
      this.cmbCorrespondentSatHr.Enabled = false;
      this.cmbCorrespondentSatMin.Enabled = false;
      this.cmbCorrespondentSatAMPM.Enabled = false;
      this.chkCorrespondentSunHours.Enabled = false;
      this.cmbCorrespondentSunHr.Enabled = false;
      this.cmbCorrespondentSunMin.Enabled = false;
      this.cmbCorrespondentSunAMPM.Enabled = false;
    }

    private void chkHours_CheckedChanged(object sender, EventArgs e)
    {
      if (sender == this.chkBrokerSatHours)
        this.onrpSettings.Broker.EnableSatONRP = this.chkBrokerSatHours.Checked;
      else if (sender == this.chkBrokerSunHours)
        this.onrpSettings.Broker.EnableSunONRP = this.chkBrokerSunHours.Checked;
      else if (sender == this.chkCorrespondentSatHours)
        this.onrpSettings.Correspondent.EnableSatONRP = this.chkCorrespondentSatHours.Checked;
      else if (sender == this.chkCorrespondentSunHours)
        this.onrpSettings.Correspondent.EnableSunONRP = this.chkCorrespondentSunHours.Checked;
      if (sender == this.chkBrokerSatHours || sender == this.chkBrokerSunHours)
      {
        this.SetBrokerViewStates(this.onrpSettings.Broker);
        this.PopulateBrokerValues(this.onrpSettings.Broker);
      }
      else
      {
        this.SetCorrespondentViewStates(this.onrpSettings.Correspondent);
        this.PopulateCorrespondentValues(this.onrpSettings.Correspondent);
      }
      this.dataChanged(sender, e);
    }

    private void chkAllowONRPForCancelledExpiredLocks_CheckedChanged(object sender, EventArgs e)
    {
      this.dataChanged(sender, e);
    }

    private void disableControls(int orgID)
    {
      bool hierarchyAccess;
      Session.TpoHierarchyAccessCache.TryGetValue(orgID, out hierarchyAccess);
      TPOClientUtils.DisableControls((UserControl) this, hierarchyAccess);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EditCompanyONRPControl));
      this.groupContainer = new GroupContainer();
      this.panel2 = new Panel();
      this.label1 = new Label();
      this.btnReset = new StandardIconButton();
      this.btnSave = new StandardIconButton();
      this.panel1 = new Panel();
      this.gcCorrespondent = new GroupContainer();
      this.chkCTAllowONRPForCancelledExpiredLocks = new CheckBox();
      this.cmbCorrespondentSunAMPM = new ComboBox();
      this.lblCorrespondentSunEt = new Label();
      this.cmbCorrespondentSunMin = new ComboBox();
      this.cmbCorrespondentSunHr = new ComboBox();
      this.lblCorrespondentSunEndTime = new Label();
      this.txtCorrespondentSunStartTime = new TextBox();
      this.lblCorrespondentSunStartTime = new Label();
      this.chkCorrespondentSunHours = new CheckBox();
      this.cmbCorrespondentSatAMPM = new ComboBox();
      this.lblCorrespondentSatEt = new Label();
      this.cmbCorrespondentSatMin = new ComboBox();
      this.cmbCorrespondentSatHr = new ComboBox();
      this.lblCorrespondentSatEndTime = new Label();
      this.txtCorrespondentSatStartTime = new TextBox();
      this.lblCorrespondentSatStartTime = new Label();
      this.chkCorrespondentSatHours = new CheckBox();
      this.label2 = new Label();
      this.panel6 = new Panel();
      this.rbtnCorrespondentContinuous = new RadioButton();
      this.rbtnCorrespondentSpecifyTime = new RadioButton();
      this.panel4 = new Panel();
      this.rbtnCorrespondentGlobal = new RadioButton();
      this.rbtnCorrespondentCustom = new RadioButton();
      this.lblCorrespondentPercent = new Label();
      this.cmbCorrespondentAMPM = new ComboBox();
      this.txtCorrespondentTolerance = new TextBox();
      this.lblCorrespondentTolerance = new Label();
      this.txtCorrespondentDollarLimit = new TextBox();
      this.lblCorrespondentDollar = new Label();
      this.chkCorrespondentNoLimit = new CheckBox();
      this.chkCorrespondentWHCoverage = new CheckBox();
      this.lblCorrespondentEt = new Label();
      this.cmbCorrespondentMin = new ComboBox();
      this.cmbCorrespondentHr = new ComboBox();
      this.lblCorrespondentEndTime = new Label();
      this.txtCorrespondentStartTime = new TextBox();
      this.lblCorrespondentStartTime = new Label();
      this.lblCorrespondentCoverage = new Label();
      this.chkCorrespondent = new CheckBox();
      this.gcBroker = new GroupContainer();
      this.cmbBrokerSunAMPM = new ComboBox();
      this.lblBrokerSunEt = new Label();
      this.cmbBrokerSunMin = new ComboBox();
      this.cmbBrokerSunHr = new ComboBox();
      this.lblBrokerSunEndTime = new Label();
      this.txtBrokerSunStartTime = new TextBox();
      this.lblBrokerSunStartTime = new Label();
      this.chkBrokerSunHours = new CheckBox();
      this.cmbBrokerSatAMPM = new ComboBox();
      this.lblBrokerSatEt = new Label();
      this.cmbBrokerSatMin = new ComboBox();
      this.cmbBrokerSatHr = new ComboBox();
      this.lblBrokerSatEndTime = new Label();
      this.txtBrokerSatStartTime = new TextBox();
      this.lblBrokerSatStartTime = new Label();
      this.chkBrokerSatHours = new CheckBox();
      this.lblWeekdayHours = new Label();
      this.panel5 = new Panel();
      this.rbtnBrokerContinuous = new RadioButton();
      this.rbtnBrokerSpecifyTime = new RadioButton();
      this.panel3 = new Panel();
      this.rbtnBrokerGlobal = new RadioButton();
      this.rbtnBrokerCustom = new RadioButton();
      this.lblBrokerPercent = new Label();
      this.cmbBrokerAMPM = new ComboBox();
      this.txtBrokerTolerance = new TextBox();
      this.lblBrokerTolerance = new Label();
      this.txtBrokerDollarLimit = new TextBox();
      this.lblBrokerDollar = new Label();
      this.chkBrokerNoLimit = new CheckBox();
      this.chkBrokerWHCoverage = new CheckBox();
      this.lblBrokerEt = new Label();
      this.cmbBrokerMin = new ComboBox();
      this.cmbBrokerHr = new ComboBox();
      this.lblBrokerEndTime = new Label();
      this.txtBrokerStartTime = new TextBox();
      this.lblBrokerStartTime = new Label();
      this.lblBrokerCoverage = new Label();
      this.chkBroker = new CheckBox();
      this.groupContainer.SuspendLayout();
      this.panel2.SuspendLayout();
      ((ISupportInitialize) this.btnReset).BeginInit();
      ((ISupportInitialize) this.btnSave).BeginInit();
      this.panel1.SuspendLayout();
      this.gcCorrespondent.SuspendLayout();
      this.panel6.SuspendLayout();
      this.panel4.SuspendLayout();
      this.gcBroker.SuspendLayout();
      this.panel5.SuspendLayout();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer.Controls.Add((Control) this.panel2);
      this.groupContainer.Controls.Add((Control) this.btnReset);
      this.groupContainer.Controls.Add((Control) this.btnSave);
      this.groupContainer.Controls.Add((Control) this.panel1);
      this.groupContainer.Dock = DockStyle.Fill;
      this.groupContainer.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer.Location = new Point(0, 0);
      this.groupContainer.Name = "groupContainer";
      this.groupContainer.Padding = new Padding(5);
      this.groupContainer.Size = new Size(944, 622);
      this.groupContainer.TabIndex = 5;
      this.groupContainer.Text = "Overnight Rate Protection";
      this.groupContainer.SizeChanged += new EventHandler(this.groupContainer_SizeChanged);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Dock = DockStyle.Top;
      this.panel2.Location = new Point(6, 31);
      this.panel2.Name = "panel2";
      this.panel2.Padding = new Padding(0, 5, 0, 5);
      this.panel2.Size = new Size(932, 42);
      this.panel2.TabIndex = 37;
      this.label1.BackColor = Color.Transparent;
      this.label1.Dock = DockStyle.Fill;
      this.label1.Location = new Point(0, 5);
      this.label1.Name = "label1";
      this.label1.Size = new Size(932, 32);
      this.label1.TabIndex = 1;
      this.label1.Text = componentResourceManager.GetString("label1.Text");
      this.btnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReset.BackColor = Color.Transparent;
      this.btnReset.Enabled = false;
      this.btnReset.Location = new Point(921, 5);
      this.btnReset.MouseDownImage = (Image) null;
      this.btnReset.Name = "btnReset";
      this.btnReset.Size = new Size(16, 16);
      this.btnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnReset.TabIndex = 36;
      this.btnReset.TabStop = false;
      this.btnReset.Click += new EventHandler(this.btnReset_Click);
      this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Enabled = false;
      this.btnSave.Location = new Point(899, 5);
      this.btnSave.MouseDownImage = (Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 35;
      this.btnSave.TabStop = false;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.panel1.Controls.Add((Control) this.gcCorrespondent);
      this.panel1.Controls.Add((Control) this.gcBroker);
      this.panel1.Location = new Point(6, 79);
      this.panel1.Name = "panel1";
      this.panel1.Padding = new Padding(0, 5, 0, 5);
      this.panel1.Size = new Size(932, 534);
      this.panel1.TabIndex = 5;
      this.gcCorrespondent.Controls.Add((Control) this.chkCTAllowONRPForCancelledExpiredLocks);
      this.gcCorrespondent.Controls.Add((Control) this.cmbCorrespondentSunAMPM);
      this.gcCorrespondent.Controls.Add((Control) this.lblCorrespondentSunEt);
      this.gcCorrespondent.Controls.Add((Control) this.cmbCorrespondentSunMin);
      this.gcCorrespondent.Controls.Add((Control) this.cmbCorrespondentSunHr);
      this.gcCorrespondent.Controls.Add((Control) this.lblCorrespondentSunEndTime);
      this.gcCorrespondent.Controls.Add((Control) this.txtCorrespondentSunStartTime);
      this.gcCorrespondent.Controls.Add((Control) this.lblCorrespondentSunStartTime);
      this.gcCorrespondent.Controls.Add((Control) this.chkCorrespondentSunHours);
      this.gcCorrespondent.Controls.Add((Control) this.cmbCorrespondentSatAMPM);
      this.gcCorrespondent.Controls.Add((Control) this.lblCorrespondentSatEt);
      this.gcCorrespondent.Controls.Add((Control) this.cmbCorrespondentSatMin);
      this.gcCorrespondent.Controls.Add((Control) this.cmbCorrespondentSatHr);
      this.gcCorrespondent.Controls.Add((Control) this.lblCorrespondentSatEndTime);
      this.gcCorrespondent.Controls.Add((Control) this.txtCorrespondentSatStartTime);
      this.gcCorrespondent.Controls.Add((Control) this.lblCorrespondentSatStartTime);
      this.gcCorrespondent.Controls.Add((Control) this.chkCorrespondentSatHours);
      this.gcCorrespondent.Controls.Add((Control) this.label2);
      this.gcCorrespondent.Controls.Add((Control) this.panel6);
      this.gcCorrespondent.Controls.Add((Control) this.panel4);
      this.gcCorrespondent.Controls.Add((Control) this.lblCorrespondentPercent);
      this.gcCorrespondent.Controls.Add((Control) this.cmbCorrespondentAMPM);
      this.gcCorrespondent.Controls.Add((Control) this.txtCorrespondentTolerance);
      this.gcCorrespondent.Controls.Add((Control) this.lblCorrespondentTolerance);
      this.gcCorrespondent.Controls.Add((Control) this.txtCorrespondentDollarLimit);
      this.gcCorrespondent.Controls.Add((Control) this.lblCorrespondentDollar);
      this.gcCorrespondent.Controls.Add((Control) this.chkCorrespondentNoLimit);
      this.gcCorrespondent.Controls.Add((Control) this.chkCorrespondentWHCoverage);
      this.gcCorrespondent.Controls.Add((Control) this.lblCorrespondentEt);
      this.gcCorrespondent.Controls.Add((Control) this.cmbCorrespondentMin);
      this.gcCorrespondent.Controls.Add((Control) this.cmbCorrespondentHr);
      this.gcCorrespondent.Controls.Add((Control) this.lblCorrespondentEndTime);
      this.gcCorrespondent.Controls.Add((Control) this.txtCorrespondentStartTime);
      this.gcCorrespondent.Controls.Add((Control) this.lblCorrespondentStartTime);
      this.gcCorrespondent.Controls.Add((Control) this.lblCorrespondentCoverage);
      this.gcCorrespondent.Controls.Add((Control) this.chkCorrespondent);
      this.gcCorrespondent.Dock = DockStyle.Right;
      this.gcCorrespondent.HeaderForeColor = SystemColors.ControlText;
      this.gcCorrespondent.Location = new Point(466, 5);
      this.gcCorrespondent.Name = "gcCorrespondent";
      this.gcCorrespondent.Size = new Size(466, 524);
      this.gcCorrespondent.TabIndex = 4;
      this.gcCorrespondent.Text = "Correspondent Settings for ONRP";
      this.chkCTAllowONRPForCancelledExpiredLocks.AutoSize = true;
      this.chkCTAllowONRPForCancelledExpiredLocks.Location = new Point(29, 59);
      this.chkCTAllowONRPForCancelledExpiredLocks.Name = "chkCTAllowONRPForCancelledExpiredLocks";
      this.chkCTAllowONRPForCancelledExpiredLocks.Size = new Size(222, 17);
      this.chkCTAllowONRPForCancelledExpiredLocks.TabIndex = 84;
      this.chkCTAllowONRPForCancelledExpiredLocks.Text = "Allow ONRP for Cancelled/Expired Locks";
      this.chkCTAllowONRPForCancelledExpiredLocks.UseVisualStyleBackColor = true;
      this.chkCTAllowONRPForCancelledExpiredLocks.CheckedChanged += new EventHandler(this.chkAllowONRPForCancelledExpiredLocks_CheckedChanged);
      this.cmbCorrespondentSunAMPM.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCorrespondentSunAMPM.FormattingEnabled = true;
      this.cmbCorrespondentSunAMPM.Items.AddRange(new object[2]
      {
        (object) "AM",
        (object) "PM"
      });
      this.cmbCorrespondentSunAMPM.Location = new Point(247, 415);
      this.cmbCorrespondentSunAMPM.Name = "cmbCorrespondentSunAMPM";
      this.cmbCorrespondentSunAMPM.Size = new Size(40, 21);
      this.cmbCorrespondentSunAMPM.TabIndex = 83;
      this.cmbCorrespondentSunAMPM.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.lblCorrespondentSunEt.AutoSize = true;
      this.lblCorrespondentSunEt.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.lblCorrespondentSunEt.Location = new Point(292, 419);
      this.lblCorrespondentSunEt.Name = "lblCorrespondentSunEt";
      this.lblCorrespondentSunEt.Size = new Size(21, 13);
      this.lblCorrespondentSunEt.TabIndex = 82;
      this.lblCorrespondentSunEt.Text = "ET";
      this.cmbCorrespondentSunMin.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCorrespondentSunMin.FormattingEnabled = true;
      this.cmbCorrespondentSunMin.Location = new Point(206, 415);
      this.cmbCorrespondentSunMin.Name = "cmbCorrespondentSunMin";
      this.cmbCorrespondentSunMin.Size = new Size(38, 21);
      this.cmbCorrespondentSunMin.TabIndex = 81;
      this.cmbCorrespondentSunMin.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.cmbCorrespondentSunHr.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCorrespondentSunHr.FormattingEnabled = true;
      this.cmbCorrespondentSunHr.Location = new Point(165, 415);
      this.cmbCorrespondentSunHr.Name = "cmbCorrespondentSunHr";
      this.cmbCorrespondentSunHr.Size = new Size(38, 21);
      this.cmbCorrespondentSunHr.TabIndex = 80;
      this.cmbCorrespondentSunHr.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.lblCorrespondentSunEndTime.AutoSize = true;
      this.lblCorrespondentSunEndTime.Location = new Point(64, 417);
      this.lblCorrespondentSunEndTime.Name = "lblCorrespondentSunEndTime";
      this.lblCorrespondentSunEndTime.Size = new Size(86, 13);
      this.lblCorrespondentSunEndTime.TabIndex = 79;
      this.lblCorrespondentSunEndTime.Text = "ONRP End Time";
      this.txtCorrespondentSunStartTime.Enabled = false;
      this.txtCorrespondentSunStartTime.Location = new Point(165, 392);
      this.txtCorrespondentSunStartTime.Name = "txtCorrespondentSunStartTime";
      this.txtCorrespondentSunStartTime.Size = new Size(123, 20);
      this.txtCorrespondentSunStartTime.TabIndex = 78;
      this.lblCorrespondentSunStartTime.AutoSize = true;
      this.lblCorrespondentSunStartTime.Location = new Point(64, 395);
      this.lblCorrespondentSunStartTime.Name = "lblCorrespondentSunStartTime";
      this.lblCorrespondentSunStartTime.Size = new Size(89, 13);
      this.lblCorrespondentSunStartTime.TabIndex = 77;
      this.lblCorrespondentSunStartTime.Text = "ONRP Start Time";
      this.chkCorrespondentSunHours.AutoSize = true;
      this.chkCorrespondentSunHours.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkCorrespondentSunHours.Location = new Point(45, 373);
      this.chkCorrespondentSunHours.Name = "chkCorrespondentSunHours";
      this.chkCorrespondentSunHours.Size = new Size(105, 17);
      this.chkCorrespondentSunHours.TabIndex = 76;
      this.chkCorrespondentSunHours.Text = "Sunday Hours";
      this.chkCorrespondentSunHours.UseVisualStyleBackColor = true;
      this.chkCorrespondentSunHours.CheckedChanged += new EventHandler(this.chkHours_CheckedChanged);
      this.cmbCorrespondentSatAMPM.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCorrespondentSatAMPM.FormattingEnabled = true;
      this.cmbCorrespondentSatAMPM.Items.AddRange(new object[2]
      {
        (object) "AM",
        (object) "PM"
      });
      this.cmbCorrespondentSatAMPM.Location = new Point(247, 343);
      this.cmbCorrespondentSatAMPM.Name = "cmbCorrespondentSatAMPM";
      this.cmbCorrespondentSatAMPM.Size = new Size(40, 21);
      this.cmbCorrespondentSatAMPM.TabIndex = 75;
      this.cmbCorrespondentSatAMPM.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.lblCorrespondentSatEt.AutoSize = true;
      this.lblCorrespondentSatEt.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.lblCorrespondentSatEt.Location = new Point(292, 347);
      this.lblCorrespondentSatEt.Name = "lblCorrespondentSatEt";
      this.lblCorrespondentSatEt.Size = new Size(21, 13);
      this.lblCorrespondentSatEt.TabIndex = 74;
      this.lblCorrespondentSatEt.Text = "ET";
      this.cmbCorrespondentSatMin.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCorrespondentSatMin.FormattingEnabled = true;
      this.cmbCorrespondentSatMin.Location = new Point(206, 343);
      this.cmbCorrespondentSatMin.Name = "cmbCorrespondentSatMin";
      this.cmbCorrespondentSatMin.Size = new Size(38, 21);
      this.cmbCorrespondentSatMin.TabIndex = 73;
      this.cmbCorrespondentSatMin.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.cmbCorrespondentSatHr.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCorrespondentSatHr.FormattingEnabled = true;
      this.cmbCorrespondentSatHr.Location = new Point(165, 343);
      this.cmbCorrespondentSatHr.Name = "cmbCorrespondentSatHr";
      this.cmbCorrespondentSatHr.Size = new Size(38, 21);
      this.cmbCorrespondentSatHr.TabIndex = 72;
      this.cmbCorrespondentSatHr.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.lblCorrespondentSatEndTime.AutoSize = true;
      this.lblCorrespondentSatEndTime.Location = new Point(64, 345);
      this.lblCorrespondentSatEndTime.Name = "lblCorrespondentSatEndTime";
      this.lblCorrespondentSatEndTime.Size = new Size(86, 13);
      this.lblCorrespondentSatEndTime.TabIndex = 71;
      this.lblCorrespondentSatEndTime.Text = "ONRP End Time";
      this.txtCorrespondentSatStartTime.Enabled = false;
      this.txtCorrespondentSatStartTime.Location = new Point(165, 320);
      this.txtCorrespondentSatStartTime.Name = "txtCorrespondentSatStartTime";
      this.txtCorrespondentSatStartTime.Size = new Size(123, 20);
      this.txtCorrespondentSatStartTime.TabIndex = 70;
      this.lblCorrespondentSatStartTime.AutoSize = true;
      this.lblCorrespondentSatStartTime.Location = new Point(64, 323);
      this.lblCorrespondentSatStartTime.Name = "lblCorrespondentSatStartTime";
      this.lblCorrespondentSatStartTime.Size = new Size(89, 13);
      this.lblCorrespondentSatStartTime.TabIndex = 69;
      this.lblCorrespondentSatStartTime.Text = "ONRP Start Time";
      this.chkCorrespondentSatHours.AutoSize = true;
      this.chkCorrespondentSatHours.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkCorrespondentSatHours.Location = new Point(45, 301);
      this.chkCorrespondentSatHours.Name = "chkCorrespondentSatHours";
      this.chkCorrespondentSatHours.Size = new Size(113, 17);
      this.chkCorrespondentSatHours.TabIndex = 68;
      this.chkCorrespondentSatHours.Text = "Saturday Hours";
      this.chkCorrespondentSatHours.UseVisualStyleBackColor = true;
      this.chkCorrespondentSatHours.CheckedChanged += new EventHandler(this.chkHours_CheckedChanged);
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(66, 196);
      this.label2.Name = "label2";
      this.label2.Size = new Size(97, 13);
      this.label2.TabIndex = 50;
      this.label2.Text = "Weekday Hours";
      this.panel6.Controls.Add((Control) this.rbtnCorrespondentContinuous);
      this.panel6.Controls.Add((Control) this.rbtnCorrespondentSpecifyTime);
      this.panel6.Location = new Point(45, 149);
      this.panel6.Name = "panel6";
      this.panel6.Size = new Size(284, 42);
      this.panel6.TabIndex = 67;
      this.rbtnCorrespondentContinuous.AutoSize = true;
      this.rbtnCorrespondentContinuous.Location = new Point(3, 2);
      this.rbtnCorrespondentContinuous.Name = "rbtnCorrespondentContinuous";
      this.rbtnCorrespondentContinuous.Size = new Size(161, 17);
      this.rbtnCorrespondentContinuous.TabIndex = 48;
      this.rbtnCorrespondentContinuous.TabStop = true;
      this.rbtnCorrespondentContinuous.Text = "Continuous ONRP Coverage";
      this.rbtnCorrespondentContinuous.UseVisualStyleBackColor = true;
      this.rbtnCorrespondentContinuous.CheckedChanged += new EventHandler(this.ToggleCorrespondentContinuous);
      this.rbtnCorrespondentSpecifyTime.AutoSize = true;
      this.rbtnCorrespondentSpecifyTime.Location = new Point(3, 20);
      this.rbtnCorrespondentSpecifyTime.Name = "rbtnCorrespondentSpecifyTime";
      this.rbtnCorrespondentSpecifyTime.Size = new Size(91, 17);
      this.rbtnCorrespondentSpecifyTime.TabIndex = 49;
      this.rbtnCorrespondentSpecifyTime.TabStop = true;
      this.rbtnCorrespondentSpecifyTime.Text = "Specify Times";
      this.rbtnCorrespondentSpecifyTime.UseVisualStyleBackColor = true;
      this.panel4.Controls.Add((Control) this.rbtnCorrespondentGlobal);
      this.panel4.Controls.Add((Control) this.rbtnCorrespondentCustom);
      this.panel4.Location = new Point(22, 83);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(284, 42);
      this.panel4.TabIndex = 66;
      this.rbtnCorrespondentGlobal.AutoSize = true;
      this.rbtnCorrespondentGlobal.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rbtnCorrespondentGlobal.Location = new Point(7, 3);
      this.rbtnCorrespondentGlobal.Name = "rbtnCorrespondentGlobal";
      this.rbtnCorrespondentGlobal.Size = new Size(128, 17);
      this.rbtnCorrespondentGlobal.TabIndex = 63;
      this.rbtnCorrespondentGlobal.TabStop = true;
      this.rbtnCorrespondentGlobal.Text = "Use Channel Defaults";
      this.rbtnCorrespondentGlobal.UseVisualStyleBackColor = true;
      this.rbtnCorrespondentGlobal.CheckedChanged += new EventHandler(this.ToggleCorrespondentGlobalCustomize);
      this.rbtnCorrespondentCustom.AutoSize = true;
      this.rbtnCorrespondentCustom.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rbtnCorrespondentCustom.Location = new Point(7, 21);
      this.rbtnCorrespondentCustom.Name = "rbtnCorrespondentCustom";
      this.rbtnCorrespondentCustom.Size = new Size(114, 17);
      this.rbtnCorrespondentCustom.TabIndex = 64;
      this.rbtnCorrespondentCustom.TabStop = true;
      this.rbtnCorrespondentCustom.Text = "Customize Settings";
      this.rbtnCorrespondentCustom.UseVisualStyleBackColor = true;
      this.lblCorrespondentPercent.AutoSize = true;
      this.lblCorrespondentPercent.Location = new Point(273, 497);
      this.lblCorrespondentPercent.Name = "lblCorrespondentPercent";
      this.lblCorrespondentPercent.Size = new Size(15, 13);
      this.lblCorrespondentPercent.TabIndex = 65;
      this.lblCorrespondentPercent.Text = "%";
      this.cmbCorrespondentAMPM.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCorrespondentAMPM.FormattingEnabled = true;
      this.cmbCorrespondentAMPM.Items.AddRange(new object[2]
      {
        (object) "AM",
        (object) "PM"
      });
      this.cmbCorrespondentAMPM.Location = new Point(247, 238);
      this.cmbCorrespondentAMPM.Name = "cmbCorrespondentAMPM";
      this.cmbCorrespondentAMPM.Size = new Size(40, 21);
      this.cmbCorrespondentAMPM.TabIndex = 62;
      this.cmbCorrespondentAMPM.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.txtCorrespondentTolerance.Location = new Point(147, 495);
      this.txtCorrespondentTolerance.MaxLength = 2;
      this.txtCorrespondentTolerance.Name = "txtCorrespondentTolerance";
      this.txtCorrespondentTolerance.Size = new Size(122, 20);
      this.txtCorrespondentTolerance.TabIndex = 61;
      this.txtCorrespondentTolerance.TextChanged += new EventHandler(this.dataChanged);
      this.lblCorrespondentTolerance.AutoSize = true;
      this.lblCorrespondentTolerance.Location = new Point(46, 498);
      this.lblCorrespondentTolerance.Name = "lblCorrespondentTolerance";
      this.lblCorrespondentTolerance.Size = new Size(89, 13);
      this.lblCorrespondentTolerance.TabIndex = 60;
      this.lblCorrespondentTolerance.Text = "ONRP Tolerance";
      this.txtCorrespondentDollarLimit.Location = new Point(147, 472);
      this.txtCorrespondentDollarLimit.MaxLength = 8;
      this.txtCorrespondentDollarLimit.Name = "txtCorrespondentDollarLimit";
      this.txtCorrespondentDollarLimit.Size = new Size(122, 20);
      this.txtCorrespondentDollarLimit.TabIndex = 59;
      this.txtCorrespondentDollarLimit.TextChanged += new EventHandler(this.dataChanged);
      this.lblCorrespondentDollar.AutoSize = true;
      this.lblCorrespondentDollar.Location = new Point(46, 478);
      this.lblCorrespondentDollar.Name = "lblCorrespondentDollar";
      this.lblCorrespondentDollar.Size = new Size(101, 13);
      this.lblCorrespondentDollar.TabIndex = 58;
      this.lblCorrespondentDollar.Text = "ONRP Dollar Limit $";
      this.chkCorrespondentNoLimit.AutoSize = true;
      this.chkCorrespondentNoLimit.Location = new Point(29, 453);
      this.chkCorrespondentNoLimit.Name = "chkCorrespondentNoLimit";
      this.chkCorrespondentNoLimit.Size = new Size(111, 17);
      this.chkCorrespondentNoLimit.TabIndex = 57;
      this.chkCorrespondentNoLimit.Text = "No Maximum Limit";
      this.chkCorrespondentNoLimit.UseVisualStyleBackColor = true;
      this.chkCorrespondentNoLimit.CheckedChanged += new EventHandler(this.chkCorrespondentNoLimit_CheckedChanged);
      this.chkCorrespondentWHCoverage.AutoSize = true;
      this.chkCorrespondentWHCoverage.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkCorrespondentWHCoverage.Location = new Point(45, 263);
      this.chkCorrespondentWHCoverage.Name = "chkCorrespondentWHCoverage";
      this.chkCorrespondentWHCoverage.Size = new Size(190, 17);
      this.chkCorrespondentWHCoverage.TabIndex = 56;
      this.chkCorrespondentWHCoverage.Text = "Weekend/ Holiday Coverage";
      this.chkCorrespondentWHCoverage.UseVisualStyleBackColor = true;
      this.chkCorrespondentWHCoverage.CheckedChanged += new EventHandler(this.dataChanged);
      this.lblCorrespondentEt.AutoSize = true;
      this.lblCorrespondentEt.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.lblCorrespondentEt.Location = new Point(292, 242);
      this.lblCorrespondentEt.Name = "lblCorrespondentEt";
      this.lblCorrespondentEt.Size = new Size(21, 13);
      this.lblCorrespondentEt.TabIndex = 55;
      this.lblCorrespondentEt.Text = "ET";
      this.cmbCorrespondentMin.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCorrespondentMin.FormattingEnabled = true;
      this.cmbCorrespondentMin.Location = new Point(206, 238);
      this.cmbCorrespondentMin.Name = "cmbCorrespondentMin";
      this.cmbCorrespondentMin.Size = new Size(38, 21);
      this.cmbCorrespondentMin.TabIndex = 54;
      this.cmbCorrespondentMin.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.cmbCorrespondentHr.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCorrespondentHr.FormattingEnabled = true;
      this.cmbCorrespondentHr.Location = new Point(165, 238);
      this.cmbCorrespondentHr.Name = "cmbCorrespondentHr";
      this.cmbCorrespondentHr.Size = new Size(38, 21);
      this.cmbCorrespondentHr.TabIndex = 53;
      this.cmbCorrespondentHr.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.lblCorrespondentEndTime.AutoSize = true;
      this.lblCorrespondentEndTime.Location = new Point(66, 238);
      this.lblCorrespondentEndTime.Name = "lblCorrespondentEndTime";
      this.lblCorrespondentEndTime.Size = new Size(86, 13);
      this.lblCorrespondentEndTime.TabIndex = 52;
      this.lblCorrespondentEndTime.Text = "ONRP End Time";
      this.txtCorrespondentStartTime.Enabled = false;
      this.txtCorrespondentStartTime.Location = new Point(165, 215);
      this.txtCorrespondentStartTime.Name = "txtCorrespondentStartTime";
      this.txtCorrespondentStartTime.Size = new Size(123, 20);
      this.txtCorrespondentStartTime.TabIndex = 51;
      this.lblCorrespondentStartTime.AutoSize = true;
      this.lblCorrespondentStartTime.Location = new Point(64, 218);
      this.lblCorrespondentStartTime.Name = "lblCorrespondentStartTime";
      this.lblCorrespondentStartTime.Size = new Size(89, 13);
      this.lblCorrespondentStartTime.TabIndex = 50;
      this.lblCorrespondentStartTime.Text = "ONRP Start Time";
      this.lblCorrespondentCoverage.AutoSize = true;
      this.lblCorrespondentCoverage.Location = new Point(42, (int) sbyte.MaxValue);
      this.lblCorrespondentCoverage.Name = "lblCorrespondentCoverage";
      this.lblCorrespondentCoverage.Size = new Size(53, 13);
      this.lblCorrespondentCoverage.TabIndex = 47;
      this.lblCorrespondentCoverage.Text = "Coverage";
      this.chkCorrespondent.AutoSize = true;
      this.chkCorrespondent.Location = new Point(22, 34);
      this.chkCorrespondent.Name = "chkCorrespondent";
      this.chkCorrespondent.Size = new Size(205, 17);
      this.chkCorrespondent.TabIndex = 0;
      this.chkCorrespondent.Text = "Enable ONRP for TPO Correspondent";
      this.chkCorrespondent.UseVisualStyleBackColor = true;
      this.chkCorrespondent.CheckedChanged += new EventHandler(this.chkCorrespondent_CheckedChanged);
      this.gcBroker.Controls.Add((Control) this.cmbBrokerSunAMPM);
      this.gcBroker.Controls.Add((Control) this.lblBrokerSunEt);
      this.gcBroker.Controls.Add((Control) this.cmbBrokerSunMin);
      this.gcBroker.Controls.Add((Control) this.cmbBrokerSunHr);
      this.gcBroker.Controls.Add((Control) this.lblBrokerSunEndTime);
      this.gcBroker.Controls.Add((Control) this.txtBrokerSunStartTime);
      this.gcBroker.Controls.Add((Control) this.lblBrokerSunStartTime);
      this.gcBroker.Controls.Add((Control) this.chkBrokerSunHours);
      this.gcBroker.Controls.Add((Control) this.cmbBrokerSatAMPM);
      this.gcBroker.Controls.Add((Control) this.lblBrokerSatEt);
      this.gcBroker.Controls.Add((Control) this.cmbBrokerSatMin);
      this.gcBroker.Controls.Add((Control) this.cmbBrokerSatHr);
      this.gcBroker.Controls.Add((Control) this.lblBrokerSatEndTime);
      this.gcBroker.Controls.Add((Control) this.txtBrokerSatStartTime);
      this.gcBroker.Controls.Add((Control) this.lblBrokerSatStartTime);
      this.gcBroker.Controls.Add((Control) this.chkBrokerSatHours);
      this.gcBroker.Controls.Add((Control) this.lblWeekdayHours);
      this.gcBroker.Controls.Add((Control) this.panel5);
      this.gcBroker.Controls.Add((Control) this.panel3);
      this.gcBroker.Controls.Add((Control) this.lblBrokerPercent);
      this.gcBroker.Controls.Add((Control) this.cmbBrokerAMPM);
      this.gcBroker.Controls.Add((Control) this.txtBrokerTolerance);
      this.gcBroker.Controls.Add((Control) this.lblBrokerTolerance);
      this.gcBroker.Controls.Add((Control) this.txtBrokerDollarLimit);
      this.gcBroker.Controls.Add((Control) this.lblBrokerDollar);
      this.gcBroker.Controls.Add((Control) this.chkBrokerNoLimit);
      this.gcBroker.Controls.Add((Control) this.chkBrokerWHCoverage);
      this.gcBroker.Controls.Add((Control) this.lblBrokerEt);
      this.gcBroker.Controls.Add((Control) this.cmbBrokerMin);
      this.gcBroker.Controls.Add((Control) this.cmbBrokerHr);
      this.gcBroker.Controls.Add((Control) this.lblBrokerEndTime);
      this.gcBroker.Controls.Add((Control) this.txtBrokerStartTime);
      this.gcBroker.Controls.Add((Control) this.lblBrokerStartTime);
      this.gcBroker.Controls.Add((Control) this.lblBrokerCoverage);
      this.gcBroker.Controls.Add((Control) this.chkBroker);
      this.gcBroker.Cursor = Cursors.Default;
      this.gcBroker.Dock = DockStyle.Left;
      this.gcBroker.HeaderForeColor = SystemColors.ControlText;
      this.gcBroker.Location = new Point(0, 5);
      this.gcBroker.Name = "gcBroker";
      this.gcBroker.Size = new Size(460, 524);
      this.gcBroker.TabIndex = 3;
      this.gcBroker.Text = "TPO Broker Settings for ONRP";
      this.cmbBrokerSunAMPM.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBrokerSunAMPM.FormattingEnabled = true;
      this.cmbBrokerSunAMPM.Items.AddRange(new object[2]
      {
        (object) "AM",
        (object) "PM"
      });
      this.cmbBrokerSunAMPM.Location = new Point(247, 387);
      this.cmbBrokerSunAMPM.Name = "cmbBrokerSunAMPM";
      this.cmbBrokerSunAMPM.Size = new Size(40, 21);
      this.cmbBrokerSunAMPM.TabIndex = 65;
      this.cmbBrokerSunAMPM.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.lblBrokerSunEt.AutoSize = true;
      this.lblBrokerSunEt.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.lblBrokerSunEt.Location = new Point(292, 391);
      this.lblBrokerSunEt.Name = "lblBrokerSunEt";
      this.lblBrokerSunEt.Size = new Size(21, 13);
      this.lblBrokerSunEt.TabIndex = 64;
      this.lblBrokerSunEt.Text = "ET";
      this.cmbBrokerSunMin.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBrokerSunMin.FormattingEnabled = true;
      this.cmbBrokerSunMin.Location = new Point(206, 387);
      this.cmbBrokerSunMin.Name = "cmbBrokerSunMin";
      this.cmbBrokerSunMin.Size = new Size(38, 21);
      this.cmbBrokerSunMin.TabIndex = 63;
      this.cmbBrokerSunMin.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.cmbBrokerSunHr.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBrokerSunHr.FormattingEnabled = true;
      this.cmbBrokerSunHr.Location = new Point(165, 387);
      this.cmbBrokerSunHr.Name = "cmbBrokerSunHr";
      this.cmbBrokerSunHr.Size = new Size(38, 21);
      this.cmbBrokerSunHr.TabIndex = 62;
      this.cmbBrokerSunHr.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.lblBrokerSunEndTime.AutoSize = true;
      this.lblBrokerSunEndTime.Location = new Point(64, 389);
      this.lblBrokerSunEndTime.Name = "lblBrokerSunEndTime";
      this.lblBrokerSunEndTime.Size = new Size(86, 13);
      this.lblBrokerSunEndTime.TabIndex = 61;
      this.lblBrokerSunEndTime.Text = "ONRP End Time";
      this.txtBrokerSunStartTime.Enabled = false;
      this.txtBrokerSunStartTime.Location = new Point(165, 364);
      this.txtBrokerSunStartTime.Name = "txtBrokerSunStartTime";
      this.txtBrokerSunStartTime.Size = new Size(123, 20);
      this.txtBrokerSunStartTime.TabIndex = 60;
      this.lblBrokerSunStartTime.AutoSize = true;
      this.lblBrokerSunStartTime.Location = new Point(64, 367);
      this.lblBrokerSunStartTime.Name = "lblBrokerSunStartTime";
      this.lblBrokerSunStartTime.Size = new Size(89, 13);
      this.lblBrokerSunStartTime.TabIndex = 59;
      this.lblBrokerSunStartTime.Text = "ONRP Start Time";
      this.chkBrokerSunHours.AutoSize = true;
      this.chkBrokerSunHours.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkBrokerSunHours.Location = new Point(45, 345);
      this.chkBrokerSunHours.Name = "chkBrokerSunHours";
      this.chkBrokerSunHours.Size = new Size(105, 17);
      this.chkBrokerSunHours.TabIndex = 58;
      this.chkBrokerSunHours.Text = "Sunday Hours";
      this.chkBrokerSunHours.UseVisualStyleBackColor = true;
      this.chkBrokerSunHours.CheckedChanged += new EventHandler(this.chkHours_CheckedChanged);
      this.cmbBrokerSatAMPM.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBrokerSatAMPM.FormattingEnabled = true;
      this.cmbBrokerSatAMPM.Items.AddRange(new object[2]
      {
        (object) "AM",
        (object) "PM"
      });
      this.cmbBrokerSatAMPM.Location = new Point(247, 315);
      this.cmbBrokerSatAMPM.Name = "cmbBrokerSatAMPM";
      this.cmbBrokerSatAMPM.Size = new Size(40, 21);
      this.cmbBrokerSatAMPM.TabIndex = 57;
      this.cmbBrokerSatAMPM.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.lblBrokerSatEt.AutoSize = true;
      this.lblBrokerSatEt.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.lblBrokerSatEt.Location = new Point(292, 319);
      this.lblBrokerSatEt.Name = "lblBrokerSatEt";
      this.lblBrokerSatEt.Size = new Size(21, 13);
      this.lblBrokerSatEt.TabIndex = 56;
      this.lblBrokerSatEt.Text = "ET";
      this.cmbBrokerSatMin.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBrokerSatMin.FormattingEnabled = true;
      this.cmbBrokerSatMin.Location = new Point(206, 315);
      this.cmbBrokerSatMin.Name = "cmbBrokerSatMin";
      this.cmbBrokerSatMin.Size = new Size(38, 21);
      this.cmbBrokerSatMin.TabIndex = 55;
      this.cmbBrokerSatMin.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.cmbBrokerSatHr.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBrokerSatHr.FormattingEnabled = true;
      this.cmbBrokerSatHr.Location = new Point(165, 315);
      this.cmbBrokerSatHr.Name = "cmbBrokerSatHr";
      this.cmbBrokerSatHr.Size = new Size(38, 21);
      this.cmbBrokerSatHr.TabIndex = 54;
      this.cmbBrokerSatHr.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.lblBrokerSatEndTime.AutoSize = true;
      this.lblBrokerSatEndTime.Location = new Point(64, 317);
      this.lblBrokerSatEndTime.Name = "lblBrokerSatEndTime";
      this.lblBrokerSatEndTime.Size = new Size(86, 13);
      this.lblBrokerSatEndTime.TabIndex = 53;
      this.lblBrokerSatEndTime.Text = "ONRP End Time";
      this.txtBrokerSatStartTime.Enabled = false;
      this.txtBrokerSatStartTime.Location = new Point(165, 292);
      this.txtBrokerSatStartTime.Name = "txtBrokerSatStartTime";
      this.txtBrokerSatStartTime.Size = new Size(123, 20);
      this.txtBrokerSatStartTime.TabIndex = 52;
      this.lblBrokerSatStartTime.AutoSize = true;
      this.lblBrokerSatStartTime.Location = new Point(64, 295);
      this.lblBrokerSatStartTime.Name = "lblBrokerSatStartTime";
      this.lblBrokerSatStartTime.Size = new Size(89, 13);
      this.lblBrokerSatStartTime.TabIndex = 51;
      this.lblBrokerSatStartTime.Text = "ONRP Start Time";
      this.chkBrokerSatHours.AutoSize = true;
      this.chkBrokerSatHours.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkBrokerSatHours.Location = new Point(45, 273);
      this.chkBrokerSatHours.Name = "chkBrokerSatHours";
      this.chkBrokerSatHours.Size = new Size(113, 17);
      this.chkBrokerSatHours.TabIndex = 50;
      this.chkBrokerSatHours.Text = "Saturday Hours";
      this.chkBrokerSatHours.UseVisualStyleBackColor = true;
      this.chkBrokerSatHours.CheckedChanged += new EventHandler(this.chkHours_CheckedChanged);
      this.lblWeekdayHours.AutoSize = true;
      this.lblWeekdayHours.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblWeekdayHours.Location = new Point(64, 168);
      this.lblWeekdayHours.Name = "lblWeekdayHours";
      this.lblWeekdayHours.Size = new Size(97, 13);
      this.lblWeekdayHours.TabIndex = 49;
      this.lblWeekdayHours.Text = "Weekday Hours";
      this.panel5.Controls.Add((Control) this.rbtnBrokerContinuous);
      this.panel5.Controls.Add((Control) this.rbtnBrokerSpecifyTime);
      this.panel5.Location = new Point(45, 121);
      this.panel5.Name = "panel5";
      this.panel5.Size = new Size(284, 42);
      this.panel5.TabIndex = 48;
      this.rbtnBrokerContinuous.AutoSize = true;
      this.rbtnBrokerContinuous.Location = new Point(3, 2);
      this.rbtnBrokerContinuous.Name = "rbtnBrokerContinuous";
      this.rbtnBrokerContinuous.Size = new Size(161, 17);
      this.rbtnBrokerContinuous.TabIndex = 4;
      this.rbtnBrokerContinuous.TabStop = true;
      this.rbtnBrokerContinuous.Text = "Continuous ONRP Coverage";
      this.rbtnBrokerContinuous.UseVisualStyleBackColor = true;
      this.rbtnBrokerContinuous.CheckedChanged += new EventHandler(this.ToggleBrokerContinuous);
      this.rbtnBrokerSpecifyTime.AutoSize = true;
      this.rbtnBrokerSpecifyTime.Location = new Point(3, 20);
      this.rbtnBrokerSpecifyTime.Name = "rbtnBrokerSpecifyTime";
      this.rbtnBrokerSpecifyTime.Size = new Size(91, 17);
      this.rbtnBrokerSpecifyTime.TabIndex = 5;
      this.rbtnBrokerSpecifyTime.TabStop = true;
      this.rbtnBrokerSpecifyTime.Text = "Specify Times";
      this.rbtnBrokerSpecifyTime.UseVisualStyleBackColor = true;
      this.panel3.Controls.Add((Control) this.rbtnBrokerGlobal);
      this.panel3.Controls.Add((Control) this.rbtnBrokerCustom);
      this.panel3.Location = new Point(22, 54);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(284, 42);
      this.panel3.TabIndex = 47;
      this.rbtnBrokerGlobal.AutoSize = true;
      this.rbtnBrokerGlobal.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rbtnBrokerGlobal.Location = new Point(7, 3);
      this.rbtnBrokerGlobal.Name = "rbtnBrokerGlobal";
      this.rbtnBrokerGlobal.Size = new Size(128, 17);
      this.rbtnBrokerGlobal.TabIndex = 44;
      this.rbtnBrokerGlobal.TabStop = true;
      this.rbtnBrokerGlobal.Text = "Use Channel Defaults";
      this.rbtnBrokerGlobal.UseVisualStyleBackColor = true;
      this.rbtnBrokerGlobal.CheckedChanged += new EventHandler(this.ToggleBrokerGlobalCustomize);
      this.rbtnBrokerCustom.AutoSize = true;
      this.rbtnBrokerCustom.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rbtnBrokerCustom.Location = new Point(7, 21);
      this.rbtnBrokerCustom.Name = "rbtnBrokerCustom";
      this.rbtnBrokerCustom.Size = new Size(114, 17);
      this.rbtnBrokerCustom.TabIndex = 45;
      this.rbtnBrokerCustom.TabStop = true;
      this.rbtnBrokerCustom.Text = "Customize Settings";
      this.rbtnBrokerCustom.UseVisualStyleBackColor = true;
      this.lblBrokerPercent.AutoSize = true;
      this.lblBrokerPercent.Location = new Point(273, 469);
      this.lblBrokerPercent.Name = "lblBrokerPercent";
      this.lblBrokerPercent.Size = new Size(15, 13);
      this.lblBrokerPercent.TabIndex = 46;
      this.lblBrokerPercent.Text = "%";
      this.cmbBrokerAMPM.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBrokerAMPM.FormattingEnabled = true;
      this.cmbBrokerAMPM.Items.AddRange(new object[2]
      {
        (object) "AM",
        (object) "PM"
      });
      this.cmbBrokerAMPM.Location = new Point(247, 210);
      this.cmbBrokerAMPM.Name = "cmbBrokerAMPM";
      this.cmbBrokerAMPM.Size = new Size(40, 21);
      this.cmbBrokerAMPM.TabIndex = 43;
      this.cmbBrokerAMPM.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.txtBrokerTolerance.Location = new Point(146, 466);
      this.txtBrokerTolerance.MaxLength = 2;
      this.txtBrokerTolerance.Name = "txtBrokerTolerance";
      this.txtBrokerTolerance.Size = new Size(121, 20);
      this.txtBrokerTolerance.TabIndex = 41;
      this.txtBrokerTolerance.TextChanged += new EventHandler(this.dataChanged);
      this.lblBrokerTolerance.AutoSize = true;
      this.lblBrokerTolerance.Location = new Point(45, 469);
      this.lblBrokerTolerance.Name = "lblBrokerTolerance";
      this.lblBrokerTolerance.Size = new Size(89, 13);
      this.lblBrokerTolerance.TabIndex = 40;
      this.lblBrokerTolerance.Text = "ONRP Tolerance";
      this.txtBrokerDollarLimit.Location = new Point(146, 443);
      this.txtBrokerDollarLimit.MaxLength = 8;
      this.txtBrokerDollarLimit.Name = "txtBrokerDollarLimit";
      this.txtBrokerDollarLimit.Size = new Size(122, 20);
      this.txtBrokerDollarLimit.TabIndex = 39;
      this.txtBrokerDollarLimit.TextChanged += new EventHandler(this.dataChanged);
      this.lblBrokerDollar.AutoSize = true;
      this.lblBrokerDollar.Location = new Point(45, 449);
      this.lblBrokerDollar.Name = "lblBrokerDollar";
      this.lblBrokerDollar.Size = new Size(101, 13);
      this.lblBrokerDollar.TabIndex = 38;
      this.lblBrokerDollar.Text = "ONRP Dollar Limit $";
      this.chkBrokerNoLimit.AutoSize = true;
      this.chkBrokerNoLimit.Location = new Point(29, 425);
      this.chkBrokerNoLimit.Name = "chkBrokerNoLimit";
      this.chkBrokerNoLimit.Size = new Size(111, 17);
      this.chkBrokerNoLimit.TabIndex = 37;
      this.chkBrokerNoLimit.Text = "No Maximum Limit";
      this.chkBrokerNoLimit.UseVisualStyleBackColor = true;
      this.chkBrokerNoLimit.CheckedChanged += new EventHandler(this.chkBrokerNoLimit_CheckedChanged);
      this.chkBrokerWHCoverage.AutoSize = true;
      this.chkBrokerWHCoverage.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkBrokerWHCoverage.Location = new Point(45, 235);
      this.chkBrokerWHCoverage.Name = "chkBrokerWHCoverage";
      this.chkBrokerWHCoverage.Size = new Size(190, 17);
      this.chkBrokerWHCoverage.TabIndex = 36;
      this.chkBrokerWHCoverage.Text = "Weekend/ Holiday Coverage";
      this.chkBrokerWHCoverage.UseVisualStyleBackColor = true;
      this.chkBrokerWHCoverage.CheckedChanged += new EventHandler(this.dataChanged);
      this.lblBrokerEt.AutoSize = true;
      this.lblBrokerEt.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.lblBrokerEt.Location = new Point(292, 214);
      this.lblBrokerEt.Name = "lblBrokerEt";
      this.lblBrokerEt.Size = new Size(21, 13);
      this.lblBrokerEt.TabIndex = 35;
      this.lblBrokerEt.Text = "ET";
      this.cmbBrokerMin.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBrokerMin.FormattingEnabled = true;
      this.cmbBrokerMin.Location = new Point(206, 210);
      this.cmbBrokerMin.Name = "cmbBrokerMin";
      this.cmbBrokerMin.Size = new Size(38, 21);
      this.cmbBrokerMin.TabIndex = 34;
      this.cmbBrokerMin.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.cmbBrokerHr.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBrokerHr.FormattingEnabled = true;
      this.cmbBrokerHr.Location = new Point(165, 210);
      this.cmbBrokerHr.Name = "cmbBrokerHr";
      this.cmbBrokerHr.Size = new Size(38, 21);
      this.cmbBrokerHr.TabIndex = 33;
      this.cmbBrokerHr.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.lblBrokerEndTime.AutoSize = true;
      this.lblBrokerEndTime.Location = new Point(64, 212);
      this.lblBrokerEndTime.Name = "lblBrokerEndTime";
      this.lblBrokerEndTime.Size = new Size(86, 13);
      this.lblBrokerEndTime.TabIndex = 8;
      this.lblBrokerEndTime.Text = "ONRP End Time";
      this.txtBrokerStartTime.Enabled = false;
      this.txtBrokerStartTime.Location = new Point(165, 187);
      this.txtBrokerStartTime.Name = "txtBrokerStartTime";
      this.txtBrokerStartTime.Size = new Size(123, 20);
      this.txtBrokerStartTime.TabIndex = 7;
      this.lblBrokerStartTime.AutoSize = true;
      this.lblBrokerStartTime.Location = new Point(64, 190);
      this.lblBrokerStartTime.Name = "lblBrokerStartTime";
      this.lblBrokerStartTime.Size = new Size(89, 13);
      this.lblBrokerStartTime.TabIndex = 6;
      this.lblBrokerStartTime.Text = "ONRP Start Time";
      this.lblBrokerCoverage.AutoSize = true;
      this.lblBrokerCoverage.Location = new Point(42, 99);
      this.lblBrokerCoverage.Name = "lblBrokerCoverage";
      this.lblBrokerCoverage.Size = new Size(53, 13);
      this.lblBrokerCoverage.TabIndex = 3;
      this.lblBrokerCoverage.Text = "Coverage";
      this.chkBroker.AutoSize = true;
      this.chkBroker.Location = new Point(22, 34);
      this.chkBroker.Name = "chkBroker";
      this.chkBroker.Size = new Size(167, 17);
      this.chkBroker.TabIndex = 0;
      this.chkBroker.Text = "Enable ONRP for TPO Broker";
      this.chkBroker.UseVisualStyleBackColor = true;
      this.chkBroker.CheckedChanged += new EventHandler(this.chkBroker_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer);
      this.Name = nameof (EditCompanyONRPControl);
      this.Size = new Size(944, 622);
      this.groupContainer.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      ((ISupportInitialize) this.btnReset).EndInit();
      ((ISupportInitialize) this.btnSave).EndInit();
      this.panel1.ResumeLayout(false);
      this.gcCorrespondent.ResumeLayout(false);
      this.gcCorrespondent.PerformLayout();
      this.panel6.ResumeLayout(false);
      this.panel6.PerformLayout();
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      this.gcBroker.ResumeLayout(false);
      this.gcBroker.PerformLayout();
      this.panel5.ResumeLayout(false);
      this.panel5.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
