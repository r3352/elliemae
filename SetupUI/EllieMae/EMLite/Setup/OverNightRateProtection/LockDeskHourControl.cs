// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.OverNightRateProtection.LockDeskHourControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup.OverNightRateProtection
{
  public class LockDeskHourControl : UserControl
  {
    private LockDeskGlobalSettings policySettings;
    private LockDeskScheduleHours lockDeskSettings;
    public bool IsCentralChannelSelected;
    private bool enableLockDesk;
    public LoanChannel channel;
    private string strChannel;
    private bool isValid = true;
    private IContainer components;
    private Label label1;
    private ComboBox cmbLDEndWeekdayBN;
    private ComboBox cmbLDStartWeekdayBN;
    private Label lblLockDeskEnd;
    private Label label15;
    private ComboBox cmbLDEndWeekdayMM;
    private ComboBox cmbLDEndWeekdayHH;
    private Label lblLockDeskStart;
    private Label label12;
    private ComboBox cmbLDStartWeekdayMM;
    private ComboBox cmbLDStartWeekdayHH;
    private ComboBox cmbLDEndSatBN;
    private ComboBox cmbLDStartSatBN;
    private Label label2;
    private Label label3;
    private ComboBox cmbLDEndSatMM;
    private ComboBox cmbLDEndSatHH;
    private Label label4;
    private Label label5;
    private ComboBox cmbLDStartSatMM;
    private ComboBox cmbLDStartSatHH;
    private CheckBox chkLDSatHours;
    private CheckBox chkLDSunHours;
    private ComboBox cmbLDEndSunBN;
    private ComboBox cmbLDStartSunBN;
    private Label label6;
    private Label label7;
    private ComboBox cmbLDEndSunMM;
    private ComboBox cmbLDEndSunHH;
    private Label label8;
    private Label label9;
    private ComboBox cmbLDStartSunMM;
    private ComboBox cmbLDStartSunHH;
    private TextBox txtLockDeskHourMessage;
    private Label label10;
    private Label label11;
    private TextBox txtLockDeskHourShutDownMessage;
    private CheckBox chkShutDownLockDesk;
    private Panel panel1;
    private Panel panel2;
    private GroupContainer groupONRPContainer;
    private TabControl onrpChannelTab;
    private TabPage tabRetail;
    private TabPage tabWholesale;
    private TabPage tabCorrespondent;
    private Panel panelFiller;
    private CheckBox chkAllowActiveRelock;
    private Panel panel3;
    private CheckBox chkAllowONRPForCancelledExpiredLocks;
    private CheckBox chkEnableONRP;
    private Label lblCoverage;
    private Label lblSunEndTime;
    private RadioButton rdoContinuousCoverage;
    private Label lblSunStartTime;
    private RadioButton rdoSpecifyTime;
    private Label lblSatEndTime;
    private TextBox txtOnrpWkdayStartTime;
    private Label lblSatStartTime;
    private ComboBox cmbOnrpWkdayEndHH;
    private TextBox txtOnrpSunStartTime;
    private ComboBox cmbOnrpWkdayEndMM;
    private TextBox txtOnrpSatStartTime;
    private CheckBox chkWeekendHolidayCoverage;
    private CheckBox chkOnrpSunHours;
    private CheckBox chkNoMaxLimit;
    private ComboBox cmbOnrpSunEndBN;
    private Label lblDollarLimit;
    private Label lblSunET;
    private TextBox txtDollarLimit;
    private ComboBox cmbOnrpSunEndMM;
    private Label lblTolerance;
    private ComboBox cmbOnrpSunEndHH;
    private TextBox txtTolerance;
    private ComboBox cmbOnrpSatEndHH;
    private ComboBox cmbOnrpWkdayEndBN;
    private CheckBox chkOnrpSatHours;
    private Label lblWdStartTime;
    private ComboBox cmbOnrpSatEndMM;
    private Label lblWdEndTime;
    private ComboBox cmbOnrpSatEndBN;
    private Label lblWeekdayHours;
    private Label lblSatET;
    private Label lblWdET;

    public event LockDeskHourControl.CentralChannelSelected CentralChannel_Changed;

    public event LockDeskHourControl.onrpChannelChanged ONRPChannelChanged;

    public event LockDeskHourControl.onrpChannelChangedFrom ONRPChannelChangedFrom;

    public event LockDeskHourControl.IsDirty isDirty;

    public event LockDeskHourControl.EnableOnrpChanged EnableOnrpChangedEvent;

    public bool OnrpEnabled => this.chkEnableONRP.Checked;

    public LockDeskGlobalSettings OnrpSettings
    {
      get
      {
        this.policySettings.ONRPEnabled = this.chkEnableONRP.Checked.ToString();
        this.policySettings.AllowONRPForCancelledExpiredLocks = this.chkAllowONRPForCancelledExpiredLocks.Checked.ToString();
        if (this.rdoContinuousCoverage.Checked)
          this.policySettings.ContinuousCoverage = ONRPCoverageType.Continuous.ToString();
        if (this.rdoSpecifyTime.Checked)
          this.policySettings.ContinuousCoverage = ONRPCoverageType.Specify.ToString();
        this.policySettings.ONRPSatEnabled = this.chkOnrpSatHours.Checked.ToString();
        this.policySettings.ONRPSunEnabled = this.chkOnrpSunHours.Checked.ToString();
        this.policySettings.ONRPStartTime = this.txtOnrpWkdayStartTime.Text;
        this.policySettings.ONRPSatStartTime = this.txtOnrpSatStartTime.Text;
        this.policySettings.ONRPSunStartTime = this.txtOnrpSunStartTime.Text;
        this.policySettings.ONRPEndTime = this.PopulateTimeFromControls(this.cmbOnrpWkdayEndHH, this.cmbOnrpWkdayEndMM, this.cmbOnrpWkdayEndBN);
        this.policySettings.ONRPSatEndTime = this.PopulateTimeFromControls(this.cmbOnrpSatEndHH, this.cmbOnrpSatEndMM, this.cmbOnrpSatEndBN);
        this.policySettings.ONRPSunEndTime = this.PopulateTimeFromControls(this.cmbOnrpSunEndHH, this.cmbOnrpSunEndMM, this.cmbOnrpSunEndBN);
        LockDeskGlobalSettings policySettings1 = this.policySettings;
        bool flag = this.chkWeekendHolidayCoverage.Checked;
        string str1 = flag.ToString();
        policySettings1.WeekendHoliday = str1;
        LockDeskGlobalSettings policySettings2 = this.policySettings;
        flag = this.chkNoMaxLimit.Checked;
        string str2 = flag.ToString();
        policySettings2.NoMaxLimit = str2;
        this.policySettings.DollarLimit = this.txtDollarLimit.Text;
        this.policySettings.Tolerance = this.txtTolerance.Text;
        return this.policySettings;
      }
      set
      {
        LockDeskGlobalSettings deskGlobalSettings = value;
        if (this.channel != deskGlobalSettings.Channel)
        {
          this.policySettings.ONRPEnabled = deskGlobalSettings.ONRPEnabled;
          this.policySettings.AllowONRPForCancelledExpiredLocks = deskGlobalSettings.AllowONRPForCancelledExpiredLocks;
          this.policySettings.ContinuousCoverage = deskGlobalSettings.ContinuousCoverage;
          this.policySettings.ONRPSatEnabled = deskGlobalSettings.ONRPSatEnabled;
          this.policySettings.ONRPSunEnabled = deskGlobalSettings.ONRPSunEnabled;
          this.policySettings.ONRPStartTime = deskGlobalSettings.ONRPStartTime;
          this.policySettings.ONRPSatStartTime = deskGlobalSettings.ONRPSatStartTime;
          this.policySettings.ONRPSunStartTime = deskGlobalSettings.ONRPSunStartTime;
          this.policySettings.ONRPEndTime = deskGlobalSettings.ONRPEndTime;
          this.policySettings.ONRPSatEndTime = deskGlobalSettings.ONRPSatEndTime;
          this.policySettings.ONRPSunEndTime = deskGlobalSettings.ONRPSunEndTime;
          this.policySettings.WeekendHoliday = deskGlobalSettings.WeekendHoliday;
          this.policySettings.NoMaxLimit = deskGlobalSettings.NoMaxLimit;
          this.policySettings.DollarLimit = deskGlobalSettings.DollarLimit;
          this.policySettings.Tolerance = deskGlobalSettings.Tolerance;
        }
        else
          this.policySettings = value;
      }
    }

    public LockDeskHourControl(LoanChannel channel)
    {
      this.InitializeComponent();
      this.channel = channel;
      TextBoxFormatter.Attach(this.txtDollarLimit, TextBoxContentRule.NonNegativeInteger, "#,##0");
      TextBoxFormatter.Attach(this.txtTolerance, TextBoxContentRule.NonNegativeInteger, "#,##0");
      this.SetupControl();
    }

    private void SetupControl()
    {
      switch (this.channel)
      {
        case LoanChannel.BankedRetail:
          this.groupONRPContainer.Text = this.groupONRPContainer.Text.Replace("[Branch]", "Retail");
          this.chkEnableONRP.Text = this.chkEnableONRP.Text.Replace("[Branch]", "Retail");
          this.strChannel = "Retail";
          break;
        case LoanChannel.BankedWholesale:
          this.groupONRPContainer.Text = this.groupONRPContainer.Text.Replace("[Branch]", "Wholesale");
          this.chkEnableONRP.Text = this.chkEnableONRP.Text.Replace("[Branch]", "Wholesale");
          this.strChannel = "Wholesale";
          break;
        case LoanChannel.Correspondent:
          this.groupONRPContainer.Text = this.groupONRPContainer.Text.Replace("[Branch]", "Correspondent");
          this.chkEnableONRP.Text = this.chkEnableONRP.Text.Replace("[Branch]", "Correspondent");
          this.strChannel = "Correspondent";
          break;
      }
    }

    public void SetLockDeskSchedule(bool enabled)
    {
      this.enableLockDesk = enabled;
      this.EnableControls(enabled);
      this.SetLockDeskHours(enabled);
      this.SetONRPControls(enabled);
    }

    public void Init(LockDeskScheduleHours settings)
    {
      this.lockDeskSettings = settings;
      if (this.lockDeskSettings.CentralChannel.ToString() != "true")
      {
        this.onrpChannelTab.Hide();
        this.panelFiller.Hide();
      }
      this.IsCentralChannelSelected = bool.Parse(this.lockDeskSettings.CentralChannel);
      this.CopyGlobalLockDeskSettings(settings);
      this.CopyOnrpSettings();
      this.SetToCentralLockDeskUI(bool.Parse(settings.CentralChannel));
    }

    public void SetToCentralLockDeskUI(bool value)
    {
      this.lockDeskSettings.CentralChannel = value.ToString();
      this.IsCentralChannelSelected = value;
      if (value)
      {
        this.onrpChannelTab.Show();
        this.panelFiller.Show();
        this.groupONRPContainer.Text = this.strChannel + " ONRP Settings";
      }
      else
      {
        this.onrpChannelTab.Hide();
        this.panelFiller.Hide();
        this.groupONRPContainer.Text = "ONRP Settings";
      }
    }

    public void Refresh(LockDeskScheduleHours settings)
    {
      this.lockDeskSettings = settings;
      this.CopyGlobalLockDeskSettings(settings);
      if (this.chkLDSatHours.Checked != bool.Parse(settings.SaturdayHoursEnabled))
        this.chkLDSatHours_CheckedChanged((object) this.chkLDSatHours, (EventArgs) null);
      if (this.chkLDSunHours.Checked == bool.Parse(settings.SundayHoursEnabled))
        return;
      this.chkLDSunHours_CheckedChanged((object) this.chkLDSunHours, (EventArgs) null);
    }

    private List<Control> EnableControls(Control container, List<Control> list, bool value)
    {
      foreach (Control control in (ArrangedElementCollection) container.Controls)
      {
        switch (control)
        {
          case TextBox _:
          case ComboBox _:
          case CheckBox _:
          case RadioButton _:
          case Label _:
            control.Enabled = value;
            break;
        }
        if (control.Controls.Count > 0)
          list = this.EnableControls(control, list, value);
      }
      return list;
    }

    private List<Control> EnableControls(bool value)
    {
      return this.EnableControls((Control) this, new List<Control>(), value);
    }

    public void MarkDirty()
    {
      if (this.isDirty == null)
        return;
      this.isDirty(true);
    }

    public virtual bool IsValid => this.isValid;

    private DateTime FromString(string dateTime)
    {
      DateTime result = new DateTime();
      DateTime.TryParse(dateTime, out result);
      return result;
    }

    private void PopulateDateToContrl(ComboBox hh, ComboBox mm, ComboBox ampm, DateTime date)
    {
      if (date > DateTime.MinValue)
      {
        hh.Text = date.ToString(nameof (hh));
        mm.Text = date.ToString(nameof (mm));
        ampm.Text = date.ToString("tt");
      }
      else
      {
        hh.Text = "";
        mm.Text = "";
        ampm.Text = "";
      }
    }

    private void PopulateTimeToControls(ComboBox hh, ComboBox mm, ComboBox ampm, string strTime)
    {
      string[] strArray1 = new string[2];
      string[] strArray2 = new string[2];
      hh.Text = "";
      mm.Text = "";
      ampm.Text = "";
      string[] strArray3 = strTime.Split(' ');
      if (strArray3.Length == 0)
        return;
      if (strArray3.Length > 1)
        ampm.Text = strArray3[1];
      if (!(strArray3[0].Trim() != ":"))
        return;
      string[] strArray4 = strArray3[0].Split(':');
      if (strArray4.Length == 0)
        return;
      if (strArray4.Length > 1)
      {
        string str = strArray4[0];
        if (str.Trim().Length == 1)
          str = "0" + strArray4[0];
        hh.Text = str;
        mm.Text = strArray4[1];
      }
      else if (strArray3[0].Trim().StartsWith(":"))
        mm.Text = strArray4[0];
      else
        hh.Text = strArray4[0];
    }

    private string PopulateTimeFromControls(
      ComboBox hh,
      ComboBox mm,
      ComboBox ampm,
      bool isRequired = true)
    {
      if (isRequired)
      {
        if (hh.SelectedItem == null || mm.SelectedItem == null || ampm.SelectedItem == null || !(hh.SelectedItem.ToString().Trim() != "") || !(mm.SelectedItem.ToString().Trim() != "") || !(ampm.SelectedItem.ToString().Trim() != ""))
          return "";
        return hh.SelectedItem.ToString().Trim() + ":" + mm.SelectedItem.ToString().Trim() + " " + ampm.SelectedItem.ToString().Trim();
      }
      return (hh.SelectedItem != null ? hh.SelectedItem.ToString().Trim() : "") + ":" + (mm.SelectedItem != null ? mm.SelectedItem.ToString().Trim() : "") + " " + (ampm.SelectedItem != null ? ampm.SelectedItem.ToString().Trim() : "");
    }

    private void SetLockDeskHours(bool enabled)
    {
      this.cmbLDEndSatBN.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDEndSatHH.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDEndSatMM.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDEndSunBN.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDEndSunHH.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDEndSunMM.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDStartWeekdayBN.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDStartWeekdayHH.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDStartWeekdayMM.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDEndWeekdayBN.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDEndWeekdayHH.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDEndWeekdayMM.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDStartSatBN.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDStartSatHH.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDStartSatMM.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDStartSunBN.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDStartSunHH.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDStartSunMM.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.txtLockDeskHourMessage.TextChanged -= new EventHandler(this.txtLockDeskHourMessage_TextChanged);
      this.txtLockDeskHourShutDownMessage.TextChanged -= new EventHandler(this.txtLockDeskHourMessage_TextChanged);
      this.chkShutDownLockDesk.TextChanged -= new EventHandler(this.chkShutDownLockDesk_CheckedChanged);
      if (!enabled)
        return;
      this.cmbLDEndSatBN.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDEndSatHH.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDEndSatMM.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDEndSunBN.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDEndSunHH.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDEndSunMM.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDStartWeekdayBN.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDStartWeekdayHH.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDStartWeekdayMM.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDEndWeekdayBN.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDEndWeekdayHH.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDEndWeekdayMM.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDStartSatBN.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDStartSatHH.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDStartSatMM.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDStartSunBN.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDStartSunHH.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbLDStartSunMM.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.txtLockDeskHourMessage.TextChanged += new EventHandler(this.txtLockDeskHourMessage_TextChanged);
      this.txtLockDeskHourShutDownMessage.TextChanged += new EventHandler(this.txtLockDeskHourMessage_TextChanged);
      this.chkShutDownLockDesk.TextChanged += new EventHandler(this.chkShutDownLockDesk_CheckedChanged);
      ComboBox cmbLdStartSunHh = this.cmbLDStartSunHH;
      ComboBox cmbLdStartSunMm = this.cmbLDStartSunMM;
      ComboBox cmbLdStartSunBn = this.cmbLDStartSunBN;
      ComboBox cmbLdEndSunHh = this.cmbLDEndSunHH;
      ComboBox cmbLdEndSunMm = this.cmbLDEndSunMM;
      bool flag1;
      this.cmbLDEndSunBN.Enabled = flag1 = this.chkLDSunHours.Checked && this.enableLockDesk;
      int num1;
      bool flag2 = (num1 = flag1 ? 1 : 0) != 0;
      cmbLdEndSunMm.Enabled = num1 != 0;
      int num2;
      bool flag3 = (num2 = flag2 ? 1 : 0) != 0;
      cmbLdEndSunHh.Enabled = num2 != 0;
      int num3;
      bool flag4 = (num3 = flag3 ? 1 : 0) != 0;
      cmbLdStartSunBn.Enabled = num3 != 0;
      int num4;
      bool flag5 = (num4 = flag4 ? 1 : 0) != 0;
      cmbLdStartSunMm.Enabled = num4 != 0;
      int num5 = flag5 ? 1 : 0;
      cmbLdStartSunHh.Enabled = num5 != 0;
      ComboBox cmbLdStartSatHh = this.cmbLDStartSatHH;
      ComboBox cmbLdStartSatMm = this.cmbLDStartSatMM;
      ComboBox cmbLdStartSatBn = this.cmbLDStartSatBN;
      ComboBox cmbLdEndSatHh = this.cmbLDEndSatHH;
      ComboBox cmbLdEndSatMm = this.cmbLDEndSatMM;
      bool flag6;
      this.cmbLDEndSatBN.Enabled = flag6 = this.chkLDSatHours.Checked && this.enableLockDesk;
      int num6;
      bool flag7 = (num6 = flag6 ? 1 : 0) != 0;
      cmbLdEndSatMm.Enabled = num6 != 0;
      int num7;
      bool flag8 = (num7 = flag7 ? 1 : 0) != 0;
      cmbLdEndSatHh.Enabled = num7 != 0;
      int num8;
      bool flag9 = (num8 = flag8 ? 1 : 0) != 0;
      cmbLdStartSatBn.Enabled = num8 != 0;
      int num9;
      bool flag10 = (num9 = flag9 ? 1 : 0) != 0;
      cmbLdStartSatMm.Enabled = num9 != 0;
      int num10 = flag10 ? 1 : 0;
      cmbLdStartSatHh.Enabled = num10 != 0;
      this.txtLockDeskHourShutDownMessage.Enabled = this.chkShutDownLockDesk.Checked && this.enableLockDesk;
      this.chkAllowActiveRelock.Enabled = this.chkShutDownLockDesk.Checked && this.enableLockDesk;
    }

    public void CopyGlobalLockDeskSettings(LockDeskScheduleHours settings)
    {
      this.cmbLDStartSunHH.Tag = this.cmbLDStartSunMM.Tag = this.cmbLDStartSunBN.Tag = this.cmbLDEndSunHH.Tag = this.cmbLDEndSunMM.Tag = this.cmbLDEndSunBN.Tag = (object) "";
      this.cmbLDStartSatHH.Tag = this.cmbLDStartSatMM.Tag = this.cmbLDStartSatBN.Tag = this.cmbLDEndSatHH.Tag = this.cmbLDEndSatMM.Tag = this.cmbLDEndSatBN.Tag = (object) "";
      this.PopulateDateToContrl(this.cmbLDStartWeekdayHH, this.cmbLDStartWeekdayMM, this.cmbLDStartWeekdayBN, Utils.ParseDate((object) settings.WeekdayStart));
      this.PopulateDateToContrl(this.cmbLDEndWeekdayHH, this.cmbLDEndWeekdayMM, this.cmbLDEndWeekdayBN, Utils.ParseDate((object) settings.WeekdayEnd));
      this.chkLDSatHours.Checked = Convert.ToBoolean(settings.SaturdayHoursEnabled);
      this.PopulateDateToContrl(this.cmbLDStartSatHH, this.cmbLDStartSatMM, this.cmbLDStartSatBN, Utils.ParseDate((object) settings.SaturdayStart));
      this.PopulateDateToContrl(this.cmbLDEndSatHH, this.cmbLDEndSatMM, this.cmbLDEndSatBN, Utils.ParseDate((object) settings.SaturdayEnd));
      this.chkLDSunHours.Checked = Convert.ToBoolean(settings.SundayHoursEnabled);
      this.PopulateDateToContrl(this.cmbLDStartSunHH, this.cmbLDStartSunMM, this.cmbLDStartSunBN, Utils.ParseDate((object) settings.SundayStart));
      this.PopulateDateToContrl(this.cmbLDEndSunHH, this.cmbLDEndSunMM, this.cmbLDEndSunBN, Utils.ParseDate((object) settings.SundayEnd));
      this.chkShutDownLockDesk.Checked = Convert.ToBoolean(settings.ShutDownLockDesk);
      this.txtLockDeskHourMessage.Text = settings.LockDeskHoursMessage;
      this.txtLockDeskHourShutDownMessage.Text = settings.LockDeskShutDownMessage;
      this.chkAllowActiveRelock.Checked = Convert.ToBoolean(settings.AllowActiveRelockRequests);
    }

    public LockDeskScheduleHours GetSettings()
    {
      LockDeskScheduleHours settings = new LockDeskScheduleHours();
      settings.CentralChannel = this.IsCentralChannelSelected.ToString();
      settings.LockDeskHoursMessage = this.txtLockDeskHourMessage.Text;
      settings.LockDeskShutDownMessage = this.txtLockDeskHourShutDownMessage.Text;
      settings.AllowActiveRelockRequests = this.chkAllowActiveRelock.Checked.ToString();
      settings.SaturdayHoursEnabled = this.chkLDSatHours.Checked.ToString();
      if (this.chkLDSatHours.Checked)
      {
        settings.SaturdayEnd = this.cmbLDEndSatHH.Text + ":" + this.cmbLDEndSatMM.Text + " " + this.cmbLDEndSatBN.Text;
        settings.SaturdayStart = this.cmbLDStartSatHH.Text + ":" + this.cmbLDStartSatMM.Text + " " + this.cmbLDStartSatBN.Text;
      }
      else
      {
        settings.SaturdayEnd = "";
        settings.SaturdayStart = "";
      }
      settings.SundayHoursEnabled = this.chkLDSunHours.Checked.ToString();
      if (this.chkLDSunHours.Checked)
      {
        settings.SundayEnd = this.cmbLDEndSunHH.Text + ":" + this.cmbLDEndSunMM.Text + " " + this.cmbLDEndSunBN.Text;
        settings.SundayStart = this.cmbLDStartSunHH.Text + ":" + this.cmbLDStartSunMM.Text + " " + this.cmbLDStartSunBN.Text;
      }
      else
      {
        settings.SundayEnd = "";
        settings.SundayStart = "";
      }
      settings.WeekdayStart = this.cmbLDStartWeekdayHH.Text + ":" + this.cmbLDStartWeekdayMM.Text + " " + this.cmbLDStartWeekdayBN.Text;
      settings.WeekdayEnd = this.cmbLDEndWeekdayHH.Text + ":" + this.cmbLDEndWeekdayMM.Text + " " + this.cmbLDEndWeekdayBN.Text;
      settings.ShutDownLockDesk = this.chkShutDownLockDesk.Checked.ToString();
      return settings;
    }

    public bool ValidateSettings()
    {
      if (this.txtLockDeskHourMessage.Text.Trim().Length == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, string.Format(this.ComposeLDErrorMessage("Please enter a {0} Lock Desk Hours Message."), (object) this.strChannel), MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (!this.enableLockDesk)
        return true;
      if (this.cmbLDStartWeekdayHH.Text == "" || this.cmbLDStartWeekdayMM.Text == "" || this.cmbLDStartWeekdayBN.Text == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, string.Format(this.ComposeLDErrorMessage("{0} Lock Desk Hours Weekday Start Time is required."), (object) this.strChannel), MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.cmbLDEndWeekdayHH.Text == "" || this.cmbLDEndWeekdayMM.Text == "" || this.cmbLDEndWeekdayBN.Text == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, string.Format(this.ComposeLDErrorMessage("{0} Lock Desk Hours Weekday End Time is required."), (object) this.strChannel), MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.chkLDSatHours.Checked && (this.cmbLDStartSatHH.Text == "" || this.cmbLDStartSatMM.Text == "" || this.cmbLDStartSatBN.Text == ""))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, string.Format(this.ComposeLDErrorMessage("{0} Lock Desk Hours Saturday Start Time is required."), (object) this.strChannel), MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.chkLDSatHours.Checked && (this.cmbLDEndSatHH.Text == "" || this.cmbLDEndSatMM.Text == "" || this.cmbLDEndSatBN.Text == ""))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, string.Format(this.ComposeLDErrorMessage("{0} Lock Desk Hours Saturday End Time is required."), (object) this.strChannel), MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.chkLDSunHours.Checked && (this.cmbLDStartSunHH.Text == "" || this.cmbLDStartSunMM.Text == "" || this.cmbLDStartSunBN.Text == ""))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, string.Format(this.ComposeLDErrorMessage("{0} Lock Desk Hours Sunday Start Time is required."), (object) this.strChannel), MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.chkLDSunHours.Checked && (this.cmbLDEndSunHH.Text == "" || this.cmbLDEndSunMM.Text == "" || this.cmbLDEndSunBN.Text == ""))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, string.Format(this.ComposeLDErrorMessage("{0} Lock Desk Hours Sunday End Time is required."), (object) this.strChannel), MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      DateTime dateTime1 = Convert.ToDateTime(this.cmbLDStartWeekdayHH.Text + ":" + this.cmbLDStartWeekdayMM.Text + " " + this.cmbLDStartWeekdayBN.Text);
      DateTime dateTime2 = Convert.ToDateTime(this.cmbLDEndWeekdayHH.Text + ":" + this.cmbLDEndWeekdayMM.Text + " " + this.cmbLDEndWeekdayBN.Text);
      DateTime endTime1 = new DateTime();
      DateTime dateTime3 = new DateTime();
      DateTime dateTime4 = new DateTime();
      DateTime dateTime5 = new DateTime();
      if (this.chkLDSatHours.Checked)
      {
        endTime1 = Convert.ToDateTime(this.cmbLDEndSatHH.Text + ":" + this.cmbLDEndSatMM.Text + " " + this.cmbLDEndSatBN.Text);
        dateTime3 = Convert.ToDateTime(this.cmbLDStartSatHH.Text + ":" + this.cmbLDStartSatMM.Text + " " + this.cmbLDStartSatBN.Text);
        if (!ONRPUtils.ValidateLDWithWeekendOverlap(dateTime1, dateTime2, dateTime3))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, string.Format(this.ComposeLDErrorMessage("{0} Weekday Lock Desk Hours overlap Saturday Lock Desk Hours and must be corrected before these settings may be saved."), (object) this.strChannel), MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
      if (this.chkLDSatHours.Checked && this.chkLDSunHours.Checked)
      {
        dateTime4 = Convert.ToDateTime(this.cmbLDEndSunHH.Text + ":" + this.cmbLDEndSunMM.Text + " " + this.cmbLDEndSunBN.Text);
        DateTime dateTime6 = Convert.ToDateTime(this.cmbLDStartSunHH.Text + ":" + this.cmbLDStartSunMM.Text + " " + this.cmbLDStartSunBN.Text);
        if (this.chkLDSunHours.Checked && !ONRPUtils.ValidateLDWithWeekendOverlap(dateTime3, endTime1, dateTime6))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, string.Format(this.ComposeLDErrorMessage("{0} Saturday Lock Desk Hours overlap Sunday Lock Desk Hours and must be corrected before these settings may be saved."), (object) this.strChannel), MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
      if (this.chkLDSunHours.Checked)
      {
        DateTime dateTime7 = Convert.ToDateTime(this.cmbLDEndSunHH.Text + ":" + this.cmbLDEndSunMM.Text + " " + this.cmbLDEndSunBN.Text);
        if (!ONRPUtils.ValidateLDWithWeekendOverlap(Convert.ToDateTime(this.cmbLDStartSunHH.Text + ":" + this.cmbLDStartSunMM.Text + " " + this.cmbLDStartSunBN.Text), dateTime7, dateTime1))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, string.Format(this.ComposeLDErrorMessage("{0} Sunday Lock Desk Hours overlap Weekday Lock Desk Hours and must be corrected before these settings may be saved."), (object) this.strChannel), MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
      if (!this.chkShutDownLockDesk.Checked || this.txtLockDeskHourShutDownMessage.Text.Trim().Length != 0)
        return this.ValidateOnrp();
      int num1 = (int) Utils.Dialog((IWin32Window) this, string.Format(this.ComposeLDErrorMessage("Please enter a {0} Lock Desk Shutdown Message."), (object) this.strChannel), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private string ComposeLDErrorMessage(string message)
    {
      return this.IsCentralChannelSelected ? message.Replace("{0} ", "") : message;
    }

    private string ComposeONRPErrorMessage(string message)
    {
      return this.IsCentralChannelSelected ? message.Replace("{0}", "Central ") : message;
    }

    private void chkShutDownLockDesk_CheckedChanged(object sender, EventArgs e)
    {
      this.txtLockDeskHourShutDownMessage.Enabled = this.chkShutDownLockDesk.Checked && this.enableLockDesk;
      this.chkAllowActiveRelock.Enabled = this.chkShutDownLockDesk.Checked && this.enableLockDesk;
      if (!this.chkShutDownLockDesk.Checked)
        this.chkAllowActiveRelock.Checked = false;
      this.MarkDirty();
    }

    private void cmb_SelectedValueChanged(object sender, EventArgs e)
    {
      this.MarkDirty();
      string text = "";
      if (sender == this.cmbLDStartWeekdayHH && this.cmbLDStartWeekdayHH.SelectedItem.ToString() == " " || sender == this.cmbLDStartWeekdayMM && this.cmbLDStartWeekdayMM.SelectedItem.ToString() == " " || sender == this.cmbLDStartWeekdayBN && this.cmbLDStartWeekdayBN.SelectedItem.ToString() == " ")
        text = "The Start Time of Lock Desk is required.";
      if (sender == this.cmbLDEndWeekdayHH && this.cmbLDEndWeekdayHH.SelectedItem.ToString() == " " || sender == this.cmbLDEndWeekdayMM && this.cmbLDEndWeekdayMM.SelectedItem.ToString() == " " || sender == this.cmbLDEndWeekdayBN && this.cmbLDEndWeekdayBN.SelectedItem.ToString() == " ")
        text = "The End Time of Lock Desk is required.";
      if ((sender == this.cmbLDStartSatHH && this.cmbLDStartSatHH.SelectedItem.ToString() == " " || sender == this.cmbLDStartSatMM && this.cmbLDStartSatMM.SelectedItem.ToString() == " " || sender == this.cmbLDStartSatBN && this.cmbLDStartSatBN.SelectedItem.ToString() == " ") && this.chkLDSatHours.Checked)
        text = "The Saturday Start Time of Lock Desk is required.";
      if ((sender == this.cmbLDEndSatHH && this.cmbLDEndSatHH.SelectedItem.ToString() == " " || sender == this.cmbLDEndSatMM && this.cmbLDEndSatMM.SelectedItem.ToString() == " " || sender == this.cmbLDEndSatBN && this.cmbLDEndSatBN.SelectedItem.ToString() == " ") && this.chkLDSatHours.Checked)
        text = "The Saturday End Time of Lock Desk is required.";
      if ((sender == this.cmbLDStartSunHH && this.cmbLDStartSunHH.SelectedItem.ToString() == " " || sender == this.cmbLDStartSunMM && this.cmbLDStartSunMM.SelectedItem.ToString() == " " || sender == this.cmbLDStartSunBN && this.cmbLDStartSunBN.SelectedItem.ToString() == " ") && this.chkLDSunHours.Checked)
        text = "The Sunday Start Time of Lock Desk is required.";
      if ((sender == this.cmbLDEndSunHH && this.cmbLDEndSunHH.SelectedItem.ToString() == " " || sender == this.cmbLDEndSunMM && this.cmbLDEndSunMM.SelectedItem.ToString() == " " || sender == this.cmbLDEndSunBN && this.cmbLDEndSunBN.SelectedItem.ToString() == " ") && this.chkLDSunHours.Checked)
        text = "The Sunday End Time of Lock Desk is required.";
      if ((sender == this.cmbOnrpWkdayEndHH || sender == this.cmbOnrpWkdayEndMM || sender == this.cmbOnrpWkdayEndBN) && this.policySettings != null)
        this.policySettings.ONRPEndTime = this.PopulateTimeFromControls(this.cmbOnrpWkdayEndHH, this.cmbOnrpWkdayEndMM, this.cmbOnrpWkdayEndBN, false);
      if ((sender == this.cmbOnrpSatEndHH || sender == this.cmbOnrpSatEndMM || sender == this.cmbOnrpSatEndBN) && this.policySettings != null)
        this.policySettings.ONRPSatEndTime = this.PopulateTimeFromControls(this.cmbOnrpSatEndHH, this.cmbOnrpSatEndMM, this.cmbOnrpSatEndBN, false);
      if ((sender == this.cmbOnrpSunEndHH || sender == this.cmbOnrpSunEndMM || sender == this.cmbOnrpSunEndBN) && this.policySettings != null)
        this.policySettings.ONRPSunEndTime = this.PopulateTimeFromControls(this.cmbOnrpSunEndHH, this.cmbOnrpSunEndMM, this.cmbOnrpSunEndBN, false);
      if (text != "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, text);
      }
      else
      {
        if ((sender == this.cmbLDEndWeekdayHH || sender == this.cmbLDEndWeekdayMM || sender == this.cmbLDEndWeekdayBN || sender == this.cmbLDStartWeekdayHH || sender == this.cmbLDStartWeekdayMM || sender == this.cmbLDStartWeekdayBN) && this.cmbLDEndWeekdayHH.SelectedIndex > 0 && this.cmbLDEndWeekdayMM.SelectedIndex > 0 && this.cmbLDEndWeekdayBN.SelectedIndex > 0 && this.cmbLDStartWeekdayHH.SelectedIndex > 0 && this.cmbLDStartWeekdayMM.SelectedIndex > 0 && this.cmbLDStartWeekdayBN.SelectedIndex > 0 && this.chkEnableONRP.Checked)
          this.SetONRPSpecifyTime(this.rdoSpecifyTime.Checked, true);
        if ((sender == this.cmbLDEndSatHH || sender == this.cmbLDEndSatMM || sender == this.cmbLDEndSatBN || sender == this.cmbLDStartSatHH || sender == this.cmbLDStartSatMM || sender == this.cmbLDStartSatBN) && this.cmbLDEndSatHH.SelectedIndex > 0 && this.cmbLDEndSatMM.SelectedIndex > 0 && this.cmbLDEndSatBN.SelectedIndex > 0 && this.cmbLDStartSatHH.SelectedIndex > 0 && this.cmbLDStartSatMM.SelectedIndex > 0 && this.cmbLDStartSatBN.SelectedIndex > 0 && this.chkEnableONRP.Checked)
          this.SetONRPSpecifyTime(this.rdoSpecifyTime.Checked, true);
        if (sender != this.cmbLDEndSunHH && sender != this.cmbLDEndSunMM && sender != this.cmbLDEndSunBN && sender != this.cmbLDStartSunHH && sender != this.cmbLDStartSunMM && sender != this.cmbLDStartSunBN || this.cmbLDEndSunHH.SelectedIndex <= 0 || this.cmbLDEndSunMM.SelectedIndex <= 0 || this.cmbLDEndSunBN.SelectedIndex <= 0 || this.cmbLDStartSunHH.SelectedIndex <= 0 || this.cmbLDStartSunMM.SelectedIndex <= 0 || this.cmbLDStartSunBN.SelectedIndex <= 0 || !this.chkEnableONRP.Checked)
          return;
        this.SetONRPSpecifyTime(this.rdoSpecifyTime.Checked, true);
      }
    }

    private void chkLDSatHours_CheckedChanged(object sender, EventArgs e)
    {
      this.cmbLDStartSatHH.Enabled = this.cmbLDStartSatMM.Enabled = this.cmbLDStartSatBN.Enabled = this.chkLDSatHours.Checked;
      this.cmbLDEndSatHH.Enabled = this.cmbLDEndSatMM.Enabled = this.cmbLDEndSatBN.Enabled = this.chkLDSatHours.Checked;
      if (e != null)
      {
        if (this.chkLDSatHours.Checked)
        {
          this.cmbLDStartSatHH.Text = this.cmbLDStartSatHH.Tag.ToString();
          this.cmbLDStartSatMM.Text = this.cmbLDStartSatMM.Tag.ToString();
          this.cmbLDStartSatBN.Text = this.cmbLDStartSatBN.Tag.ToString();
          this.cmbLDEndSatHH.Text = this.cmbLDEndSatHH.Tag.ToString();
          this.cmbLDEndSatMM.Text = this.cmbLDEndSatMM.Tag.ToString();
          this.cmbLDEndSatBN.Text = this.cmbLDEndSatBN.Tag.ToString();
        }
        else
        {
          this.cmbLDStartSatHH.Tag = (object) this.cmbLDStartSatHH.Text.ToString();
          this.cmbLDStartSatMM.Tag = (object) this.cmbLDStartSatMM.Text.ToString();
          this.cmbLDStartSatBN.Tag = (object) this.cmbLDStartSatBN.Text.ToString();
          this.cmbLDEndSatHH.Tag = (object) this.cmbLDEndSatHH.Text.ToString();
          this.cmbLDEndSatMM.Tag = (object) this.cmbLDEndSatMM.Text.ToString();
          this.cmbLDEndSatBN.Tag = (object) this.cmbLDEndSatBN.Text.ToString();
          this.cmbLDStartSatHH.Text = "";
          this.cmbLDStartSatMM.Text = "";
          this.cmbLDStartSatBN.Text = "";
          this.cmbLDEndSatHH.Text = "";
          this.cmbLDEndSatMM.Text = "";
          this.cmbLDEndSatBN.Text = "";
        }
      }
      this.UpdateOnrpHoursByLDHours(HourType.Saturday, this.chkLDSatHours.Checked, this.PopulateTimeFromControls(this.cmbLDStartSatHH, this.cmbLDStartSatMM, this.cmbLDStartSatBN), this.PopulateTimeFromControls(this.cmbLDEndSatHH, this.cmbLDEndSatMM, this.cmbLDEndSatBN));
      this.MarkDirty();
    }

    private void chkLDSunHours_CheckedChanged(object sender, EventArgs e)
    {
      this.cmbLDStartSunHH.Enabled = this.cmbLDStartSunMM.Enabled = this.cmbLDStartSunBN.Enabled = this.chkLDSunHours.Checked;
      this.cmbLDEndSunHH.Enabled = this.cmbLDEndSunMM.Enabled = this.cmbLDEndSunBN.Enabled = this.chkLDSunHours.Checked;
      if (e != null)
      {
        if (this.chkLDSunHours.Checked)
        {
          this.cmbLDStartSunHH.Text = this.cmbLDStartSunHH.Tag.ToString();
          this.cmbLDStartSunMM.Text = this.cmbLDStartSunMM.Tag.ToString();
          this.cmbLDStartSunBN.Text = this.cmbLDStartSunBN.Tag.ToString();
          this.cmbLDEndSunHH.Text = this.cmbLDEndSunHH.Tag.ToString();
          this.cmbLDEndSunMM.Text = this.cmbLDEndSunMM.Tag.ToString();
          this.cmbLDEndSunBN.Text = this.cmbLDEndSunBN.Tag.ToString();
        }
        else
        {
          this.cmbLDStartSunHH.Tag = (object) this.cmbLDStartSunHH.Text.ToString();
          this.cmbLDStartSunMM.Tag = (object) this.cmbLDStartSunMM.Text.ToString();
          this.cmbLDStartSunBN.Tag = (object) this.cmbLDStartSunBN.Text.ToString();
          this.cmbLDEndSunHH.Tag = (object) this.cmbLDEndSunHH.Text.ToString();
          this.cmbLDEndSunMM.Tag = (object) this.cmbLDEndSunMM.Text.ToString();
          this.cmbLDEndSunBN.Tag = (object) this.cmbLDEndSunBN.Text.ToString();
          this.cmbLDStartSunHH.Text = "";
          this.cmbLDStartSunMM.Text = "";
          this.cmbLDStartSunBN.Text = "";
          this.cmbLDEndSunHH.Text = "";
          this.cmbLDEndSunMM.Text = "";
          this.cmbLDEndSunBN.Text = "";
        }
      }
      this.UpdateOnrpHoursByLDHours(HourType.Sunday, this.chkLDSunHours.Checked, this.PopulateTimeFromControls(this.cmbLDStartSunHH, this.cmbLDStartSunMM, this.cmbLDStartSunBN), this.PopulateTimeFromControls(this.cmbLDEndSunHH, this.cmbLDEndSunMM, this.cmbLDEndSunBN));
      this.MarkDirty();
    }

    private void txtLockDeskHourMessage_TextChanged(object sender, EventArgs e) => this.MarkDirty();

    private void txtLockDeskHourShutDownMessage_TextChanged(object sender, EventArgs e)
    {
      this.MarkDirty();
    }

    private void SetONRPControls(bool enabled, bool isInit = true)
    {
      this.cmbOnrpWkdayEndHH.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpWkdayEndMM.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpWkdayEndBN.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpSatEndHH.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpSatEndMM.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpSatEndBN.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpSunEndHH.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpSunEndMM.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpSunEndBN.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      enabled = !(this.policySettings != null & isInit) ? this.enableLockDesk && this.chkEnableONRP.Checked : this.enableLockDesk && Utils.ParseBoolean((object) this.policySettings.ONRPEnabled);
      this.rdoContinuousCoverage.Enabled = this.rdoSpecifyTime.Enabled = this.chkWeekendHolidayCoverage.Enabled = enabled;
      if (this.channel == LoanChannel.Correspondent)
      {
        this.chkAllowONRPForCancelledExpiredLocks.Enabled = enabled;
        this.chkAllowONRPForCancelledExpiredLocks.Visible = true;
      }
      else
        this.chkAllowONRPForCancelledExpiredLocks.Visible = false;
      this.chkNoMaxLimit.Enabled = this.txtDollarLimit.Enabled = this.txtTolerance.Enabled = enabled;
      this.lblCoverage.Enabled = this.lblWdStartTime.Enabled = this.lblWdEndTime.Enabled = this.lblWdET.Enabled = this.lblDollarLimit.Enabled = this.lblTolerance.Enabled = enabled;
      this.cmbOnrpWkdayEndHH.Enabled = this.cmbOnrpWkdayEndMM.Enabled = this.cmbOnrpWkdayEndBN.Enabled = enabled;
      this.cmbOnrpSatEndHH.Enabled = this.cmbOnrpSatEndMM.Enabled = this.cmbOnrpSatEndBN.Enabled = this.chkOnrpSatHours.Enabled = enabled;
      this.cmbOnrpSunEndHH.Enabled = this.cmbOnrpSunEndMM.Enabled = this.cmbOnrpSunEndBN.Enabled = this.chkOnrpSunHours.Enabled = enabled;
      this.cmbOnrpWkdayEndHH.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpWkdayEndMM.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpWkdayEndBN.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpSatEndHH.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpSatEndMM.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpSatEndBN.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpSunEndHH.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpSunEndMM.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpSunEndBN.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
    }

    private void CopyOnrpSettings()
    {
      this.rdoContinuousCoverage.CheckedChanged -= new EventHandler(this.rbSpecifyTime_CheckedChanged);
      this.rdoSpecifyTime.CheckedChanged -= new EventHandler(this.rbSpecifyTime_CheckedChanged);
      this.chkOnrpSatHours.CheckedChanged -= new EventHandler(this.chkOnrpSatHours_CheckedChanged);
      this.chkOnrpSunHours.CheckedChanged -= new EventHandler(this.chkOnrpSunHours_CheckedChanged);
      this.chkNoMaxLimit.CheckedChanged -= new EventHandler(this.chkNoMaxLimit_CheckedChanged);
      this.chkEnableONRP.Checked = Utils.ParseBoolean((object) this.policySettings.ONRPEnabled);
      this.chkAllowONRPForCancelledExpiredLocks.Checked = Utils.ParseBoolean((object) this.policySettings.AllowONRPForCancelledExpiredLocks);
      this.rdoContinuousCoverage.Checked = this.policySettings.ContinuousCoverage == ONRPCoverageType.Continuous.ToString();
      this.rdoSpecifyTime.Checked = this.policySettings.ContinuousCoverage == ONRPCoverageType.Specify.ToString();
      this.chkOnrpSatHours.Checked = Utils.ParseBoolean((object) this.policySettings.ONRPSatEnabled);
      this.chkOnrpSunHours.Checked = Utils.ParseBoolean((object) this.policySettings.ONRPSunEnabled);
      this.chkWeekendHolidayCoverage.Checked = Utils.ParseBoolean((object) this.policySettings.WeekendHoliday);
      this.chkNoMaxLimit.Checked = Utils.ParseBoolean((object) this.policySettings.NoMaxLimit);
      this.rdoContinuousCoverage.CheckedChanged += new EventHandler(this.rbSpecifyTime_CheckedChanged);
      this.rdoSpecifyTime.CheckedChanged += new EventHandler(this.rbSpecifyTime_CheckedChanged);
      this.chkOnrpSatHours.CheckedChanged += new EventHandler(this.chkOnrpSatHours_CheckedChanged);
      this.chkOnrpSunHours.CheckedChanged += new EventHandler(this.chkOnrpSunHours_CheckedChanged);
      this.chkNoMaxLimit.CheckedChanged += new EventHandler(this.chkNoMaxLimit_CheckedChanged);
      if (!this.chkEnableONRP.Enabled || !this.chkEnableONRP.Checked)
        return;
      this.SetONRPSpecifyTime(this.rdoSpecifyTime.Checked, true);
      this.SetONRPDollar(this.chkNoMaxLimit.Enabled && !this.chkNoMaxLimit.Checked);
    }

    private void ClearOnrpWkdayHours()
    {
      this.cmbOnrpWkdayEndHH.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpWkdayEndMM.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpWkdayEndBN.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.txtOnrpWkdayStartTime.Text = "";
      this.cmbOnrpWkdayEndHH.Text = this.cmbOnrpWkdayEndMM.Text = this.cmbOnrpWkdayEndBN.Text = "";
      this.cmbOnrpWkdayEndHH.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpWkdayEndMM.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpWkdayEndBN.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
    }

    private void ClearOnrpSatHours()
    {
      this.cmbOnrpSatEndHH.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpSatEndMM.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpSatEndBN.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.txtOnrpSatStartTime.Text = "";
      this.cmbOnrpSatEndHH.Text = this.cmbOnrpSatEndMM.Text = this.cmbOnrpSatEndBN.Text = "";
      this.cmbOnrpSatEndHH.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpSatEndMM.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpSatEndBN.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
    }

    private void ClearOnrpSunHours()
    {
      this.cmbOnrpSunEndHH.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpSunEndMM.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpSunEndBN.SelectedValueChanged -= new EventHandler(this.cmb_SelectedValueChanged);
      this.txtOnrpSunStartTime.Text = "";
      this.cmbOnrpSunEndHH.Text = this.cmbOnrpSunEndMM.Text = this.cmbOnrpSunEndBN.Text = "";
      this.cmbOnrpSunEndHH.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpSunEndMM.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
      this.cmbOnrpSunEndBN.SelectedValueChanged += new EventHandler(this.cmb_SelectedValueChanged);
    }

    private void SetONRPSpecifyTime(bool isSpecify, bool isUpdateStartTime = false)
    {
      string str1 = this.PopulateTimeFromControls(this.cmbLDStartWeekdayHH, this.cmbLDStartWeekdayMM, this.cmbLDStartWeekdayBN);
      string str2 = this.PopulateTimeFromControls(this.cmbLDEndWeekdayHH, this.cmbLDEndWeekdayMM, this.cmbLDEndWeekdayBN);
      string str3 = this.PopulateTimeFromControls(this.cmbLDStartSatHH, this.cmbLDStartSatMM, this.cmbLDStartSatBN);
      string str4 = this.PopulateTimeFromControls(this.cmbLDEndSatHH, this.cmbLDEndSatMM, this.cmbLDEndSatBN);
      string str5 = this.PopulateTimeFromControls(this.cmbLDStartSunHH, this.cmbLDStartSunMM, this.cmbLDStartSunBN);
      string str6 = this.PopulateTimeFromControls(this.cmbLDEndSunHH, this.cmbLDEndSunMM, this.cmbLDEndSunBN);
      this.txtOnrpSatStartTime.Enabled = this.txtOnrpSunStartTime.Enabled = this.txtOnrpWkdayStartTime.Enabled = false;
      if (isSpecify)
      {
        this.chkOnrpSatHours.Enabled = this.rdoSpecifyTime.Enabled && this.rdoSpecifyTime.Checked && this.chkLDSatHours.Enabled && this.chkLDSatHours.Checked && str3 != str4 && str4 != "";
        this.chkOnrpSunHours.Enabled = this.rdoSpecifyTime.Enabled && this.rdoSpecifyTime.Checked && this.chkLDSunHours.Enabled && this.chkLDSunHours.Checked && str5 != str6 && str6 != "";
        Label lblCoverage = this.lblCoverage;
        Label lblWdStartTime = this.lblWdStartTime;
        Label lblWdEndTime = this.lblWdEndTime;
        Label lblWdEt = this.lblWdET;
        Label lblDollarLimit = this.lblDollarLimit;
        Label lblTolerance = this.lblTolerance;
        bool flag1;
        this.lblWeekdayHours.Enabled = flag1 = this.rdoSpecifyTime.Enabled && this.rdoSpecifyTime.Checked;
        int num1;
        bool flag2 = (num1 = flag1 ? 1 : 0) != 0;
        lblTolerance.Enabled = num1 != 0;
        int num2;
        bool flag3 = (num2 = flag2 ? 1 : 0) != 0;
        lblDollarLimit.Enabled = num2 != 0;
        int num3;
        bool flag4 = (num3 = flag3 ? 1 : 0) != 0;
        lblWdEt.Enabled = num3 != 0;
        int num4;
        bool flag5 = (num4 = flag4 ? 1 : 0) != 0;
        lblWdEndTime.Enabled = num4 != 0;
        int num5;
        bool flag6 = (num5 = flag5 ? 1 : 0) != 0;
        lblWdStartTime.Enabled = num5 != 0;
        int num6 = flag6 ? 1 : 0;
        lblCoverage.Enabled = num6 != 0;
        ComboBox cmbOnrpWkdayEndHh = this.cmbOnrpWkdayEndHH;
        ComboBox cmbOnrpWkdayEndMm = this.cmbOnrpWkdayEndMM;
        bool flag7;
        this.cmbOnrpWkdayEndBN.Enabled = flag7 = this.rdoSpecifyTime.Enabled && this.rdoSpecifyTime.Checked;
        int num7;
        bool flag8 = (num7 = flag7 ? 1 : 0) != 0;
        cmbOnrpWkdayEndMm.Enabled = num7 != 0;
        int num8 = flag8 ? 1 : 0;
        cmbOnrpWkdayEndHh.Enabled = num8 != 0;
        ComboBox cmbOnrpSatEndHh = this.cmbOnrpSatEndHH;
        ComboBox cmbOnrpSatEndMm = this.cmbOnrpSatEndMM;
        bool flag9;
        this.cmbOnrpSatEndBN.Enabled = flag9 = this.rdoSpecifyTime.Enabled && this.rdoSpecifyTime.Checked && this.chkOnrpSatHours.Enabled && this.chkOnrpSatHours.Checked;
        int num9;
        bool flag10 = (num9 = flag9 ? 1 : 0) != 0;
        cmbOnrpSatEndMm.Enabled = num9 != 0;
        int num10 = flag10 ? 1 : 0;
        cmbOnrpSatEndHh.Enabled = num10 != 0;
        ComboBox cmbOnrpSunEndHh = this.cmbOnrpSunEndHH;
        ComboBox cmbOnrpSunEndMm = this.cmbOnrpSunEndMM;
        bool flag11;
        this.cmbOnrpSunEndBN.Enabled = flag11 = this.rdoSpecifyTime.Enabled && this.rdoSpecifyTime.Checked && this.chkOnrpSunHours.Enabled && this.chkOnrpSunHours.Checked;
        int num11;
        bool flag12 = (num11 = flag11 ? 1 : 0) != 0;
        cmbOnrpSunEndMm.Enabled = num11 != 0;
        int num12 = flag12 ? 1 : 0;
        cmbOnrpSunEndHh.Enabled = num12 != 0;
        this.chkWeekendHolidayCoverage.Enabled = this.rdoSpecifyTime.Enabled && this.rdoSpecifyTime.Checked && !this.chkLDSatHours.Checked && !this.chkLDSunHours.Checked;
        string onrpEndTime = this.policySettings.ONRPEndTime;
        string onrpSatEndTime = this.policySettings.ONRPSatEndTime;
        string onrpSunEndTime = this.policySettings.ONRPSunEndTime;
        if (str1 != "" && str1 == str2)
        {
          this.ClearOnrpWkdayHours();
          this.cmbOnrpWkdayEndHH.Enabled = this.cmbOnrpWkdayEndMM.Enabled = this.cmbOnrpWkdayEndBN.Enabled = false;
          this.txtOnrpWkdayStartTime.Text = "";
        }
        else
        {
          if (!string.IsNullOrEmpty(onrpEndTime))
            this.PopulateTimeToControls(this.cmbOnrpWkdayEndHH, this.cmbOnrpWkdayEndMM, this.cmbOnrpWkdayEndBN, onrpEndTime);
          else
            this.ClearOnrpWkdayHours();
          if (isUpdateStartTime)
          {
            if (this.chkEnableONRP.Checked && str2 != "")
              this.txtOnrpWkdayStartTime.Text = str2 + " ET";
            else
              this.txtOnrpWkdayStartTime.Text = "";
          }
        }
        if (str3 != "" && str3 == str4)
        {
          this.chkOnrpSatHours.Checked = false;
        }
        else
        {
          if (!this.chkLDSatHours.Checked)
            this.chkOnrpSatHours.Checked = false;
          if (this.chkOnrpSatHours.Checked)
          {
            if (!string.IsNullOrEmpty(onrpSatEndTime))
              this.PopulateTimeToControls(this.cmbOnrpSatEndHH, this.cmbOnrpSatEndMM, this.cmbOnrpSatEndBN, onrpSatEndTime);
            else
              this.ClearOnrpSatHours();
            if (isUpdateStartTime)
            {
              if (this.chkEnableONRP.Checked && this.chkOnrpSatHours.Checked && str4 != "")
                this.txtOnrpSatStartTime.Text = str4 + " ET";
              else
                this.txtOnrpSatStartTime.Text = "";
            }
          }
          else
            this.ClearOnrpSatHours();
        }
        if (str5 != "" && str5 == str6)
        {
          this.chkOnrpSunHours.Checked = false;
        }
        else
        {
          if (!this.chkLDSunHours.Checked)
            this.chkOnrpSunHours.Checked = false;
          if (this.chkOnrpSunHours.Checked)
          {
            if (!string.IsNullOrEmpty(onrpSunEndTime))
              this.PopulateTimeToControls(this.cmbOnrpSunEndHH, this.cmbOnrpSunEndMM, this.cmbOnrpSunEndBN, onrpSunEndTime);
            else
              this.ClearOnrpSunHours();
            if (isUpdateStartTime)
            {
              if (this.chkEnableONRP.Checked && this.chkOnrpSunHours.Checked && str6 != "")
                this.txtOnrpSunStartTime.Text = str6 + " ET";
              else
                this.txtOnrpSunStartTime.Text = "";
            }
          }
          else
            this.ClearOnrpSunHours();
        }
        if (!this.rdoSpecifyTime.Checked || !this.chkLDSatHours.Checked && !this.chkLDSunHours.Checked)
          return;
        this.chkWeekendHolidayCoverage.Checked = false;
      }
      else
      {
        this.rdoContinuousCoverage.Checked = true;
        this.lblWeekdayHours.Enabled = false;
        this.cmbOnrpWkdayEndHH.Enabled = this.cmbOnrpWkdayEndMM.Enabled = this.cmbOnrpWkdayEndBN.Enabled = false;
        this.cmbOnrpSatEndHH.Enabled = this.cmbOnrpSatEndMM.Enabled = this.cmbOnrpSatEndBN.Enabled = this.chkOnrpSatHours.Enabled = false;
        this.cmbOnrpSunEndHH.Enabled = this.cmbOnrpSunEndMM.Enabled = this.cmbOnrpSunEndBN.Enabled = this.chkOnrpSunHours.Enabled = false;
        this.chkWeekendHolidayCoverage.Enabled = false;
        this.txtOnrpWkdayStartTime.Text = "";
        this.ClearOnrpWkdayHours();
        this.chkWeekendHolidayCoverage.Checked = false;
        this.chkOnrpSatHours.Checked = false;
        this.chkOnrpSunHours.Checked = false;
      }
    }

    private void SetONRPDollar(bool enabled)
    {
      this.txtDollarLimit.Enabled = enabled;
      this.txtTolerance.Enabled = enabled;
      if (enabled)
      {
        this.txtDollarLimit.Text = this.policySettings.DollarLimit;
        this.txtTolerance.Text = this.policySettings.Tolerance;
      }
      else
        this.txtDollarLimit.Text = this.txtTolerance.Text = "";
    }

    private bool ValidateOnrp()
    {
      string str1 = this.PopulateTimeFromControls(this.cmbLDStartWeekdayHH, this.cmbLDStartWeekdayMM, this.cmbLDStartWeekdayBN);
      string str2 = this.PopulateTimeFromControls(this.cmbLDStartSatHH, this.cmbLDStartSatMM, this.cmbLDStartSatBN);
      string str3 = this.PopulateTimeFromControls(this.cmbLDStartSunHH, this.cmbLDStartSunMM, this.cmbLDStartSunBN);
      string ldEndTime1 = this.PopulateTimeFromControls(this.cmbLDEndWeekdayHH, this.cmbLDEndWeekdayMM, this.cmbLDEndWeekdayBN);
      string ldEndTime2 = this.PopulateTimeFromControls(this.cmbLDEndSatHH, this.cmbLDEndSatMM, this.cmbLDEndSatBN);
      string ldEndTime3 = this.PopulateTimeFromControls(this.cmbLDEndSunHH, this.cmbLDEndSunMM, this.cmbLDEndSunBN);
      if (this.chkEnableONRP.Checked)
      {
        if (this.rdoSpecifyTime.Checked)
        {
          string str4 = this.PopulateTimeFromControls(this.cmbOnrpWkdayEndHH, this.cmbOnrpWkdayEndMM, this.cmbOnrpWkdayEndBN);
          string str5 = this.PopulateTimeFromControls(this.cmbOnrpSatEndHH, this.cmbOnrpSatEndMM, this.cmbOnrpSatEndBN);
          string str6 = this.PopulateTimeFromControls(this.cmbOnrpSunEndHH, this.cmbOnrpSunEndMM, this.cmbOnrpSunEndBN);
          if (str1 != ldEndTime1 && str4 == "")
          {
            int num = (int) Utils.Dialog((IWin32Window) this, string.Format("The ONRP Weekday End time must be set for {0} channel before the ONRP Setting may be saved.", (object) this.strChannel));
            return false;
          }
          if (this.chkOnrpSatHours.Checked && str2 != ldEndTime2 && str5 == "")
          {
            int num = (int) Utils.Dialog((IWin32Window) this, string.Format("The ONRP Saturday End time must be set for {0} channel before the ONRP Setting may be saved.", (object) this.strChannel));
            return false;
          }
          if (this.chkOnrpSunHours.Checked && str3 != ldEndTime3 && str6 == "")
          {
            int num = (int) Utils.Dialog((IWin32Window) this, string.Format("The ONRP Sunday End time must be set for {0} channel before the ONRP Setting may be saved.", (object) this.strChannel));
            return false;
          }
          if (ldEndTime1 != "" && str4 != "")
          {
            string text = ONRPUtils.TimeStampSameErrorMessage(Utils.ParseDate((object) ldEndTime1), Utils.ParseDate((object) str4), false, "Weekday", this.strChannel);
            if (!string.IsNullOrEmpty(text))
            {
              int num = (int) Utils.Dialog((IWin32Window) this, text);
              return false;
            }
          }
          if (ldEndTime2 != "" && str5 != "")
          {
            string text = ONRPUtils.TimeStampSameErrorMessage(Utils.ParseDate((object) ldEndTime2), Utils.ParseDate((object) str5), false, "Saturday", this.strChannel);
            if (!string.IsNullOrEmpty(text))
            {
              int num = (int) Utils.Dialog((IWin32Window) this, text);
              return false;
            }
          }
          if (ldEndTime3 != "" && str6 != "")
          {
            string text = ONRPUtils.TimeStampSameErrorMessage(Utils.ParseDate((object) ldEndTime3), Utils.ParseDate((object) str6), false, "Sunday", this.strChannel);
            if (!string.IsNullOrEmpty(text))
            {
              int num = (int) Utils.Dialog((IWin32Window) this, text);
              return false;
            }
          }
          if (str1 != "" && str2 != "" && str4 != "" && !this.ValidateOnrpTimeChange(str1, str4, str2))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, this.strChannel + " Weekday ONRP End Time overlaps Lock Desk Saturday Hours and must be corrected before these settings can be saved.");
            return false;
          }
          if (str2 != "" && str5 != "" && str3 != "" && !this.ValidateOnrpTimeChange(str2, str5, str3))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, this.strChannel + " Saturday ONRP End Time overlaps Lock Desk Sunday Hours and must be corrected before these settings can be saved.");
            return false;
          }
          if (str3 != "" && str6 != "" && str1 != "" && !this.ValidateOnrpTimeChange(str3, str6, str1))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, this.strChannel + " Sunday ONRP End Time overlaps Lock Desk Weekday Hours and must be corrected before these settings can be saved.");
            return false;
          }
          if (str1 != "" && ldEndTime1 != "" && str4 != "" && !this.ValidLDTimeChange(str1, ldEndTime1, str4, HourType.Weekday) || str2 != "" && ldEndTime2 != "" && str5 != "" && !this.ValidLDTimeChange(str2, ldEndTime2, str5, HourType.Saturday) || str3 != "" && ldEndTime3 != "" && str6 != "" && !this.ValidLDTimeChange(str3, ldEndTime3, str6, HourType.Sunday))
            return false;
        }
        if (!this.chkNoMaxLimit.Checked && Utils.ParseDouble((object) this.txtDollarLimit.Text.Trim(), 0.0) == 0.0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "An ONRP Dollar Limit must be entered for " + this.strChannel + " channel if the No Maximum Limit is disabled");
          return false;
        }
      }
      return true;
    }

    private bool ValidateOnrpTimeChange(string startTime1, string endTime1, string startTime2)
    {
      DateTime result1 = DateTime.MinValue;
      DateTime result2 = DateTime.MinValue;
      DateTime result3 = DateTime.MinValue;
      return DateTime.TryParse(startTime1, out result1) && DateTime.TryParse(endTime1, out result2) && DateTime.TryParse(startTime2, out result3) && ONRPUtils.ValidateLDWithWeekendOverlap(result1, result2, result3);
    }

    private bool ValidLDTimeChange(
      string ldStrTime,
      string ldEndTime,
      string ONRPEndTime,
      HourType hourType)
    {
      DateTime result1 = DateTime.MinValue;
      DateTime result2 = DateTime.MinValue;
      DateTime result3 = DateTime.MinValue;
      Convert.ToDateTime("12:00 pm");
      if (!DateTime.TryParse(ldStrTime, out result1) || !DateTime.TryParse(ONRPEndTime, out result3) || !DateTime.TryParse(ldEndTime, out result2))
        return false;
      if (!OnrpSetupUtils.LockDeskHourHasOverlap(result1, result2, result2, result3))
        return true;
      switch (hourType)
      {
        case HourType.Weekday:
          int num1 = (int) Utils.Dialog((IWin32Window) this, this.strChannel + " Weekday Lock Desk Hours plus Weekday ONRP Hours exceed 24 hours and must be corrected before these settings can be saved.");
          break;
        case HourType.Saturday:
          int num2 = (int) Utils.Dialog((IWin32Window) this, this.strChannel + " Saturday Lock Desk Hours plus Saturday ONRP Hours exceed 24 hours and must be corrected before these settings can be saved.");
          break;
        case HourType.Sunday:
          int num3 = (int) Utils.Dialog((IWin32Window) this, this.strChannel + " Sunday Lock Desk Hours plus Sunday ONRP Hours exceed 24 hours and must be corrected before these settings can be saved.");
          break;
      }
      return false;
    }

    private void UpdateOnrpHoursByLDHours(
      HourType hourType,
      bool lockDeskEnabled,
      string lockDeskStartTime,
      string lockDeskEndTime)
    {
      switch (hourType)
      {
        case HourType.Weekday:
          this.policySettings.LockDeskStartTime = lockDeskStartTime;
          this.policySettings.LockDeskEndTime = lockDeskEndTime;
          break;
        case HourType.Saturday:
          this.policySettings.EnableLockDeskSat = lockDeskEnabled.ToString();
          this.policySettings.LockDeskStartTimeSat = lockDeskStartTime;
          this.policySettings.LockDeskEndTimeSat = lockDeskEndTime;
          break;
        case HourType.Sunday:
          this.policySettings.EnableLockDeskSun = lockDeskEnabled.ToString();
          this.policySettings.LockDeskStartTimeSun = lockDeskStartTime;
          this.policySettings.LockDeskEndTimeSun = lockDeskEndTime;
          break;
      }
      this.SetONRPSpecifyTime(this.rdoSpecifyTime.Checked, true);
    }

    private void chkEnableOnrp_CheckedChanged(object sender, EventArgs e)
    {
      this.SetONRPControls(((CheckBox) sender).Checked, false);
      if (((CheckBox) sender).Checked)
      {
        this.SetONRPSpecifyTime(this.rdoSpecifyTime.Checked, true);
        this.SetONRPDollar(this.chkNoMaxLimit.Enabled && !this.chkNoMaxLimit.Checked);
      }
      this.MarkDirty();
      if (this.EnableOnrpChangedEvent == null)
        return;
      this.EnableOnrpChangedEvent(sender, e);
    }

    private void rbSpecifyTime_CheckedChanged(object sender, EventArgs e)
    {
      if (this.rdoContinuousCoverage.Checked)
        this.SetONRPSpecifyTime(false, true);
      else if (this.rdoSpecifyTime.Checked)
      {
        this.chkOnrpSatHours.Checked = Utils.ParseBoolean((object) this.policySettings.ONRPSatEnabled);
        this.chkOnrpSunHours.Checked = Utils.ParseBoolean((object) this.policySettings.ONRPSunEnabled);
        this.SetONRPSpecifyTime(true, true);
      }
      this.MarkDirty();
    }

    private void chkNoMaxLimit_CheckedChanged(object sender, EventArgs e)
    {
      this.SetONRPDollar(((Control) sender).Enabled && !((CheckBox) sender).Checked);
      this.MarkDirty();
    }

    private void txtDollarLimit_TextChanged(object sender, EventArgs e) => this.MarkDirty();

    private void txtTolerance_TextChanged(object sender, EventArgs e) => this.MarkDirty();

    private void chkWkHolidayCoverage_CheckedChanged(object sender, EventArgs e)
    {
      this.MarkDirty();
    }

    private void chkOnrpSatHours_CheckedChanged(object sender, EventArgs e)
    {
      this.cmbOnrpSatEndHH.Enabled = this.cmbOnrpSatEndMM.Enabled = this.cmbOnrpSatEndBN.Enabled = this.chkOnrpSatHours.Checked;
      if (!this.chkOnrpSatHours.Checked)
      {
        this.txtOnrpSatStartTime.Enabled = false;
        this.ClearOnrpSatHours();
      }
      else
        this.SetONRPSpecifyTime(this.rdoSpecifyTime.Checked, true);
      this.MarkDirty();
    }

    private void chkOnrpSunHours_CheckedChanged(object sender, EventArgs e)
    {
      this.cmbOnrpSunEndHH.Enabled = this.cmbOnrpSunEndMM.Enabled = this.cmbOnrpSunEndBN.Enabled = this.chkOnrpSunHours.Checked;
      if (!this.chkOnrpSunHours.Checked)
      {
        this.txtOnrpSunStartTime.Enabled = false;
        this.ClearOnrpSunHours();
      }
      else
        this.SetONRPSpecifyTime(this.rdoSpecifyTime.Checked, true);
      this.MarkDirty();
    }

    private void gcONRP_Resize(object sender, EventArgs e)
    {
    }

    private void panel5_SizeChanged(object sender, EventArgs e)
    {
    }

    public void Reset()
    {
    }

    public bool EnableLockDesk
    {
      get => this.enableLockDesk;
      set => this.enableLockDesk = value;
    }

    private void onrpChannelTab_TabIndexChanged(object sender, EventArgs e)
    {
      this.ONRPChannelChangedFrom(this.channel);
    }

    private void onrpChannelTab_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.ONRPChannelChanged(!(this.onrpChannelTab.SelectedTab.Text == "Retail") ? (!(this.onrpChannelTab.SelectedTab.Text == "Wholesale") ? LoanChannel.Correspondent : LoanChannel.BankedWholesale) : LoanChannel.BankedRetail);
    }

    public void SetChannelTab(LoanChannel channel)
    {
      if (channel == LoanChannel.BankedRetail)
        this.onrpChannelTab.SelectedIndex = 0;
      if (channel == LoanChannel.BankedWholesale)
        this.onrpChannelTab.SelectedIndex = 1;
      if (channel != LoanChannel.Correspondent)
        return;
      this.onrpChannelTab.SelectedIndex = 2;
    }

    private void onrpChannelTab_Deselected(object sender, TabControlEventArgs e)
    {
      if (!(this.lockDeskSettings.CentralChannel.ToLower() == "true"))
        return;
      if (((TabControl) sender).SelectedTab.Name == "tabRetail")
        this.ONRPChannelChangedFrom(LoanChannel.BankedRetail);
      if (((TabControl) sender).SelectedTab.Name == "tabWholesale")
        this.ONRPChannelChangedFrom(LoanChannel.BankedWholesale);
      if (!(((TabControl) sender).SelectedTab.Name == "tabCorrespondent"))
        return;
      this.ONRPChannelChangedFrom(LoanChannel.Correspondent);
    }

    private void chkAllowActiveRelock_CheckedChanged(object sender, EventArgs e)
    {
      this.MarkDirty();
    }

    private void chkAllowONRPForCancelledExpiredLocks_CheckedChanged(object sender, EventArgs e)
    {
      this.MarkDirty();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.cmbLDEndWeekdayBN = new ComboBox();
      this.cmbLDStartWeekdayBN = new ComboBox();
      this.lblLockDeskEnd = new Label();
      this.label15 = new Label();
      this.cmbLDEndWeekdayMM = new ComboBox();
      this.cmbLDEndWeekdayHH = new ComboBox();
      this.lblLockDeskStart = new Label();
      this.label12 = new Label();
      this.cmbLDStartWeekdayMM = new ComboBox();
      this.cmbLDStartWeekdayHH = new ComboBox();
      this.cmbLDEndSatBN = new ComboBox();
      this.cmbLDStartSatBN = new ComboBox();
      this.label2 = new Label();
      this.label3 = new Label();
      this.cmbLDEndSatMM = new ComboBox();
      this.cmbLDEndSatHH = new ComboBox();
      this.label4 = new Label();
      this.label5 = new Label();
      this.cmbLDStartSatMM = new ComboBox();
      this.cmbLDStartSatHH = new ComboBox();
      this.chkLDSatHours = new CheckBox();
      this.chkLDSunHours = new CheckBox();
      this.cmbLDEndSunBN = new ComboBox();
      this.cmbLDStartSunBN = new ComboBox();
      this.label6 = new Label();
      this.label7 = new Label();
      this.cmbLDEndSunMM = new ComboBox();
      this.cmbLDEndSunHH = new ComboBox();
      this.label8 = new Label();
      this.label9 = new Label();
      this.cmbLDStartSunMM = new ComboBox();
      this.cmbLDStartSunHH = new ComboBox();
      this.txtLockDeskHourMessage = new TextBox();
      this.label10 = new Label();
      this.label11 = new Label();
      this.txtLockDeskHourShutDownMessage = new TextBox();
      this.chkShutDownLockDesk = new CheckBox();
      this.panel1 = new Panel();
      this.chkAllowActiveRelock = new CheckBox();
      this.panel2 = new Panel();
      this.groupONRPContainer = new GroupContainer();
      this.onrpChannelTab = new TabControl();
      this.tabRetail = new TabPage();
      this.tabWholesale = new TabPage();
      this.tabCorrespondent = new TabPage();
      this.panel3 = new Panel();
      this.chkAllowONRPForCancelledExpiredLocks = new CheckBox();
      this.chkEnableONRP = new CheckBox();
      this.lblCoverage = new Label();
      this.lblSunEndTime = new Label();
      this.rdoContinuousCoverage = new RadioButton();
      this.lblSunStartTime = new Label();
      this.rdoSpecifyTime = new RadioButton();
      this.lblSatEndTime = new Label();
      this.txtOnrpWkdayStartTime = new TextBox();
      this.lblSatStartTime = new Label();
      this.cmbOnrpWkdayEndHH = new ComboBox();
      this.txtOnrpSunStartTime = new TextBox();
      this.cmbOnrpWkdayEndMM = new ComboBox();
      this.txtOnrpSatStartTime = new TextBox();
      this.chkWeekendHolidayCoverage = new CheckBox();
      this.chkOnrpSunHours = new CheckBox();
      this.chkNoMaxLimit = new CheckBox();
      this.cmbOnrpSunEndBN = new ComboBox();
      this.lblDollarLimit = new Label();
      this.lblSunET = new Label();
      this.txtDollarLimit = new TextBox();
      this.cmbOnrpSunEndMM = new ComboBox();
      this.lblTolerance = new Label();
      this.cmbOnrpSunEndHH = new ComboBox();
      this.txtTolerance = new TextBox();
      this.cmbOnrpSatEndHH = new ComboBox();
      this.cmbOnrpWkdayEndBN = new ComboBox();
      this.chkOnrpSatHours = new CheckBox();
      this.lblWdStartTime = new Label();
      this.cmbOnrpSatEndMM = new ComboBox();
      this.lblWdEndTime = new Label();
      this.cmbOnrpSatEndBN = new ComboBox();
      this.lblWeekdayHours = new Label();
      this.lblSatET = new Label();
      this.lblWdET = new Label();
      this.panelFiller = new Panel();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.groupONRPContainer.SuspendLayout();
      this.onrpChannelTab.SuspendLayout();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(85, 18);
      this.label1.Name = "label1";
      this.label1.Size = new Size(97, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Weekday Hours";
      this.cmbLDEndWeekdayBN.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLDEndWeekdayBN.FormattingEnabled = true;
      this.cmbLDEndWeekdayBN.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "AM",
        (object) "PM"
      });
      this.cmbLDEndWeekdayBN.Location = new Point(180, 73);
      this.cmbLDEndWeekdayBN.Name = "cmbLDEndWeekdayBN";
      this.cmbLDEndWeekdayBN.Size = new Size(40, 21);
      this.cmbLDEndWeekdayBN.TabIndex = 63;
      this.cmbLDStartWeekdayBN.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLDStartWeekdayBN.FormattingEnabled = true;
      this.cmbLDStartWeekdayBN.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "AM",
        (object) "PM"
      });
      this.cmbLDStartWeekdayBN.Location = new Point(180, 46);
      this.cmbLDStartWeekdayBN.Name = "cmbLDStartWeekdayBN";
      this.cmbLDStartWeekdayBN.Size = new Size(40, 21);
      this.cmbLDStartWeekdayBN.TabIndex = 62;
      this.lblLockDeskEnd.AutoSize = true;
      this.lblLockDeskEnd.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.lblLockDeskEnd.Location = new Point(20, 76);
      this.lblLockDeskEnd.Name = "lblLockDeskEnd";
      this.lblLockDeskEnd.Size = new Size(52, 13);
      this.lblLockDeskEnd.TabIndex = 61;
      this.lblLockDeskEnd.Text = "End Time";
      this.label15.AutoSize = true;
      this.label15.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label15.Location = new Point(226, 76);
      this.label15.Name = "label15";
      this.label15.Size = new Size(21, 13);
      this.label15.TabIndex = 60;
      this.label15.Text = "ET";
      this.cmbLDEndWeekdayMM.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLDEndWeekdayMM.FormattingEnabled = true;
      this.cmbLDEndWeekdayMM.Items.AddRange(new object[61]
      {
        (object) "",
        (object) "00",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12",
        (object) "13",
        (object) "14",
        (object) "15",
        (object) "16",
        (object) "17",
        (object) "18",
        (object) "19",
        (object) "20",
        (object) "21",
        (object) "22",
        (object) "23",
        (object) "24",
        (object) "25",
        (object) "26",
        (object) "27",
        (object) "28",
        (object) "29",
        (object) "30",
        (object) "31",
        (object) "32",
        (object) "33",
        (object) "34",
        (object) "35",
        (object) "36",
        (object) "37",
        (object) "38",
        (object) "39",
        (object) "40",
        (object) "41",
        (object) "42",
        (object) "43",
        (object) "44",
        (object) "45",
        (object) "46",
        (object) "47",
        (object) "48",
        (object) "49",
        (object) "50",
        (object) "51",
        (object) "52",
        (object) "53",
        (object) "54",
        (object) "55",
        (object) "56",
        (object) "57",
        (object) "58",
        (object) "59"
      });
      this.cmbLDEndWeekdayMM.Location = new Point(134, 73);
      this.cmbLDEndWeekdayMM.Name = "cmbLDEndWeekdayMM";
      this.cmbLDEndWeekdayMM.Size = new Size(40, 21);
      this.cmbLDEndWeekdayMM.TabIndex = 59;
      this.cmbLDEndWeekdayHH.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLDEndWeekdayHH.FormattingEnabled = true;
      this.cmbLDEndWeekdayHH.Items.AddRange(new object[13]
      {
        (object) "",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12"
      });
      this.cmbLDEndWeekdayHH.Location = new Point(88, 73);
      this.cmbLDEndWeekdayHH.Name = "cmbLDEndWeekdayHH";
      this.cmbLDEndWeekdayHH.Size = new Size(40, 21);
      this.cmbLDEndWeekdayHH.TabIndex = 58;
      this.lblLockDeskStart.AutoSize = true;
      this.lblLockDeskStart.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.lblLockDeskStart.Location = new Point(20, 49);
      this.lblLockDeskStart.Name = "lblLockDeskStart";
      this.lblLockDeskStart.Size = new Size(55, 13);
      this.lblLockDeskStart.TabIndex = 57;
      this.lblLockDeskStart.Text = "Start Time";
      this.label12.AutoSize = true;
      this.label12.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label12.Location = new Point(226, 49);
      this.label12.Name = "label12";
      this.label12.Size = new Size(21, 13);
      this.label12.TabIndex = 56;
      this.label12.Text = "ET";
      this.cmbLDStartWeekdayMM.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLDStartWeekdayMM.FormattingEnabled = true;
      this.cmbLDStartWeekdayMM.Items.AddRange(new object[61]
      {
        (object) "",
        (object) "00",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12",
        (object) "13",
        (object) "14",
        (object) "15",
        (object) "16",
        (object) "17",
        (object) "18",
        (object) "19",
        (object) "20",
        (object) "21",
        (object) "22",
        (object) "23",
        (object) "24",
        (object) "25",
        (object) "26",
        (object) "27",
        (object) "28",
        (object) "29",
        (object) "30",
        (object) "31",
        (object) "32",
        (object) "33",
        (object) "34",
        (object) "35",
        (object) "36",
        (object) "37",
        (object) "38",
        (object) "39",
        (object) "40",
        (object) "41",
        (object) "42",
        (object) "43",
        (object) "44",
        (object) "45",
        (object) "46",
        (object) "47",
        (object) "48",
        (object) "49",
        (object) "50",
        (object) "51",
        (object) "52",
        (object) "53",
        (object) "54",
        (object) "55",
        (object) "56",
        (object) "57",
        (object) "58",
        (object) "59"
      });
      this.cmbLDStartWeekdayMM.Location = new Point(134, 46);
      this.cmbLDStartWeekdayMM.Name = "cmbLDStartWeekdayMM";
      this.cmbLDStartWeekdayMM.Size = new Size(40, 21);
      this.cmbLDStartWeekdayMM.TabIndex = 55;
      this.cmbLDStartWeekdayHH.AutoCompleteCustomSource.AddRange(new string[13]
      {
        "",
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9",
        "10",
        "11",
        "12"
      });
      this.cmbLDStartWeekdayHH.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLDStartWeekdayHH.FormattingEnabled = true;
      this.cmbLDStartWeekdayHH.Items.AddRange(new object[13]
      {
        (object) "",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12"
      });
      this.cmbLDStartWeekdayHH.Location = new Point(88, 46);
      this.cmbLDStartWeekdayHH.Name = "cmbLDStartWeekdayHH";
      this.cmbLDStartWeekdayHH.Size = new Size(40, 21);
      this.cmbLDStartWeekdayHH.TabIndex = 54;
      this.cmbLDEndSatBN.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLDEndSatBN.Enabled = false;
      this.cmbLDEndSatBN.FormattingEnabled = true;
      this.cmbLDEndSatBN.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "AM",
        (object) "PM"
      });
      this.cmbLDEndSatBN.Location = new Point(498, 73);
      this.cmbLDEndSatBN.Name = "cmbLDEndSatBN";
      this.cmbLDEndSatBN.Size = new Size(40, 21);
      this.cmbLDEndSatBN.TabIndex = 73;
      this.cmbLDStartSatBN.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLDStartSatBN.Enabled = false;
      this.cmbLDStartSatBN.FormattingEnabled = true;
      this.cmbLDStartSatBN.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "AM",
        (object) "PM"
      });
      this.cmbLDStartSatBN.Location = new Point(498, 46);
      this.cmbLDStartSatBN.Name = "cmbLDStartSatBN";
      this.cmbLDStartSatBN.Size = new Size(40, 21);
      this.cmbLDStartSatBN.TabIndex = 72;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label2.Location = new Point(338, 76);
      this.label2.Name = "label2";
      this.label2.Size = new Size(52, 13);
      this.label2.TabIndex = 71;
      this.label2.Text = "End Time";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label3.Location = new Point(544, 76);
      this.label3.Name = "label3";
      this.label3.Size = new Size(21, 13);
      this.label3.TabIndex = 70;
      this.label3.Text = "ET";
      this.cmbLDEndSatMM.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLDEndSatMM.Enabled = false;
      this.cmbLDEndSatMM.FormattingEnabled = true;
      this.cmbLDEndSatMM.Items.AddRange(new object[61]
      {
        (object) "",
        (object) "00",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12",
        (object) "13",
        (object) "14",
        (object) "15",
        (object) "16",
        (object) "17",
        (object) "18",
        (object) "19",
        (object) "20",
        (object) "21",
        (object) "22",
        (object) "23",
        (object) "24",
        (object) "25",
        (object) "26",
        (object) "27",
        (object) "28",
        (object) "29",
        (object) "30",
        (object) "31",
        (object) "32",
        (object) "33",
        (object) "34",
        (object) "35",
        (object) "36",
        (object) "37",
        (object) "38",
        (object) "39",
        (object) "40",
        (object) "41",
        (object) "42",
        (object) "43",
        (object) "44",
        (object) "45",
        (object) "46",
        (object) "47",
        (object) "48",
        (object) "49",
        (object) "50",
        (object) "51",
        (object) "52",
        (object) "53",
        (object) "54",
        (object) "55",
        (object) "56",
        (object) "57",
        (object) "58",
        (object) "59"
      });
      this.cmbLDEndSatMM.Location = new Point(452, 73);
      this.cmbLDEndSatMM.Name = "cmbLDEndSatMM";
      this.cmbLDEndSatMM.Size = new Size(40, 21);
      this.cmbLDEndSatMM.TabIndex = 69;
      this.cmbLDEndSatHH.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLDEndSatHH.Enabled = false;
      this.cmbLDEndSatHH.FormattingEnabled = true;
      this.cmbLDEndSatHH.Items.AddRange(new object[13]
      {
        (object) "",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12"
      });
      this.cmbLDEndSatHH.Location = new Point(406, 73);
      this.cmbLDEndSatHH.Name = "cmbLDEndSatHH";
      this.cmbLDEndSatHH.Size = new Size(40, 21);
      this.cmbLDEndSatHH.TabIndex = 68;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label4.Location = new Point(338, 49);
      this.label4.Name = "label4";
      this.label4.Size = new Size(55, 13);
      this.label4.TabIndex = 67;
      this.label4.Text = "Start Time";
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label5.Location = new Point(544, 49);
      this.label5.Name = "label5";
      this.label5.Size = new Size(21, 13);
      this.label5.TabIndex = 66;
      this.label5.Text = "ET";
      this.cmbLDStartSatMM.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLDStartSatMM.Enabled = false;
      this.cmbLDStartSatMM.FormattingEnabled = true;
      this.cmbLDStartSatMM.Items.AddRange(new object[61]
      {
        (object) "",
        (object) "00",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12",
        (object) "13",
        (object) "14",
        (object) "15",
        (object) "16",
        (object) "17",
        (object) "18",
        (object) "19",
        (object) "20",
        (object) "21",
        (object) "22",
        (object) "23",
        (object) "24",
        (object) "25",
        (object) "26",
        (object) "27",
        (object) "28",
        (object) "29",
        (object) "30",
        (object) "31",
        (object) "32",
        (object) "33",
        (object) "34",
        (object) "35",
        (object) "36",
        (object) "37",
        (object) "38",
        (object) "39",
        (object) "40",
        (object) "41",
        (object) "42",
        (object) "43",
        (object) "44",
        (object) "45",
        (object) "46",
        (object) "47",
        (object) "48",
        (object) "49",
        (object) "50",
        (object) "51",
        (object) "52",
        (object) "53",
        (object) "54",
        (object) "55",
        (object) "56",
        (object) "57",
        (object) "58",
        (object) "59"
      });
      this.cmbLDStartSatMM.Location = new Point(452, 46);
      this.cmbLDStartSatMM.Name = "cmbLDStartSatMM";
      this.cmbLDStartSatMM.Size = new Size(40, 21);
      this.cmbLDStartSatMM.TabIndex = 65;
      this.cmbLDStartSatHH.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLDStartSatHH.Enabled = false;
      this.cmbLDStartSatHH.FormattingEnabled = true;
      this.cmbLDStartSatHH.Items.AddRange(new object[13]
      {
        (object) "",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12"
      });
      this.cmbLDStartSatHH.Location = new Point(406, 46);
      this.cmbLDStartSatHH.Name = "cmbLDStartSatHH";
      this.cmbLDStartSatHH.Size = new Size(40, 21);
      this.cmbLDStartSatHH.TabIndex = 64;
      this.chkLDSatHours.AutoSize = true;
      this.chkLDSatHours.Checked = true;
      this.chkLDSatHours.CheckState = CheckState.Checked;
      this.chkLDSatHours.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkLDSatHours.Location = new Point(406, 13);
      this.chkLDSatHours.Name = "chkLDSatHours";
      this.chkLDSatHours.Size = new Size(113, 17);
      this.chkLDSatHours.TabIndex = 74;
      this.chkLDSatHours.Text = "Saturday Hours";
      this.chkLDSatHours.UseVisualStyleBackColor = true;
      this.chkLDSatHours.CheckedChanged += new EventHandler(this.chkLDSatHours_CheckedChanged);
      this.chkLDSunHours.AutoSize = true;
      this.chkLDSunHours.Checked = true;
      this.chkLDSunHours.CheckState = CheckState.Checked;
      this.chkLDSunHours.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkLDSunHours.Location = new Point(719, 13);
      this.chkLDSunHours.Name = "chkLDSunHours";
      this.chkLDSunHours.Size = new Size(105, 17);
      this.chkLDSunHours.TabIndex = 85;
      this.chkLDSunHours.Text = "Sunday Hours";
      this.chkLDSunHours.UseVisualStyleBackColor = true;
      this.chkLDSunHours.CheckedChanged += new EventHandler(this.chkLDSunHours_CheckedChanged);
      this.cmbLDEndSunBN.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLDEndSunBN.Enabled = false;
      this.cmbLDEndSunBN.FormattingEnabled = true;
      this.cmbLDEndSunBN.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "AM",
        (object) "PM"
      });
      this.cmbLDEndSunBN.Location = new Point(811, 73);
      this.cmbLDEndSunBN.Name = "cmbLDEndSunBN";
      this.cmbLDEndSunBN.Size = new Size(40, 21);
      this.cmbLDEndSunBN.TabIndex = 84;
      this.cmbLDStartSunBN.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLDStartSunBN.Enabled = false;
      this.cmbLDStartSunBN.FormattingEnabled = true;
      this.cmbLDStartSunBN.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "AM",
        (object) "PM"
      });
      this.cmbLDStartSunBN.Location = new Point(811, 46);
      this.cmbLDStartSunBN.Name = "cmbLDStartSunBN";
      this.cmbLDStartSunBN.Size = new Size(40, 21);
      this.cmbLDStartSunBN.TabIndex = 83;
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label6.Location = new Point(651, 76);
      this.label6.Name = "label6";
      this.label6.Size = new Size(52, 13);
      this.label6.TabIndex = 82;
      this.label6.Text = "End Time";
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label7.Location = new Point(857, 76);
      this.label7.Name = "label7";
      this.label7.Size = new Size(21, 13);
      this.label7.TabIndex = 81;
      this.label7.Text = "ET";
      this.cmbLDEndSunMM.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLDEndSunMM.Enabled = false;
      this.cmbLDEndSunMM.FormattingEnabled = true;
      this.cmbLDEndSunMM.Items.AddRange(new object[61]
      {
        (object) "",
        (object) "00",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12",
        (object) "13",
        (object) "14",
        (object) "15",
        (object) "16",
        (object) "17",
        (object) "18",
        (object) "19",
        (object) "20",
        (object) "21",
        (object) "22",
        (object) "23",
        (object) "24",
        (object) "25",
        (object) "26",
        (object) "27",
        (object) "28",
        (object) "29",
        (object) "30",
        (object) "31",
        (object) "32",
        (object) "33",
        (object) "34",
        (object) "35",
        (object) "36",
        (object) "37",
        (object) "38",
        (object) "39",
        (object) "40",
        (object) "41",
        (object) "42",
        (object) "43",
        (object) "44",
        (object) "45",
        (object) "46",
        (object) "47",
        (object) "48",
        (object) "49",
        (object) "50",
        (object) "51",
        (object) "52",
        (object) "53",
        (object) "54",
        (object) "55",
        (object) "56",
        (object) "57",
        (object) "58",
        (object) "59"
      });
      this.cmbLDEndSunMM.Location = new Point(765, 73);
      this.cmbLDEndSunMM.Name = "cmbLDEndSunMM";
      this.cmbLDEndSunMM.Size = new Size(40, 21);
      this.cmbLDEndSunMM.TabIndex = 80;
      this.cmbLDEndSunHH.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLDEndSunHH.Enabled = false;
      this.cmbLDEndSunHH.FormattingEnabled = true;
      this.cmbLDEndSunHH.Items.AddRange(new object[13]
      {
        (object) "",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12"
      });
      this.cmbLDEndSunHH.Location = new Point(719, 73);
      this.cmbLDEndSunHH.Name = "cmbLDEndSunHH";
      this.cmbLDEndSunHH.Size = new Size(40, 21);
      this.cmbLDEndSunHH.TabIndex = 79;
      this.label8.AutoSize = true;
      this.label8.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label8.Location = new Point(651, 49);
      this.label8.Name = "label8";
      this.label8.Size = new Size(55, 13);
      this.label8.TabIndex = 78;
      this.label8.Text = "Start Time";
      this.label9.AutoSize = true;
      this.label9.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label9.Location = new Point(857, 49);
      this.label9.Name = "label9";
      this.label9.Size = new Size(21, 13);
      this.label9.TabIndex = 77;
      this.label9.Text = "ET";
      this.cmbLDStartSunMM.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLDStartSunMM.Enabled = false;
      this.cmbLDStartSunMM.FormattingEnabled = true;
      this.cmbLDStartSunMM.Items.AddRange(new object[61]
      {
        (object) "",
        (object) "00",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12",
        (object) "13",
        (object) "14",
        (object) "15",
        (object) "16",
        (object) "17",
        (object) "18",
        (object) "19",
        (object) "20",
        (object) "21",
        (object) "22",
        (object) "23",
        (object) "24",
        (object) "25",
        (object) "26",
        (object) "27",
        (object) "28",
        (object) "29",
        (object) "30",
        (object) "31",
        (object) "32",
        (object) "33",
        (object) "34",
        (object) "35",
        (object) "36",
        (object) "37",
        (object) "38",
        (object) "39",
        (object) "40",
        (object) "41",
        (object) "42",
        (object) "43",
        (object) "44",
        (object) "45",
        (object) "46",
        (object) "47",
        (object) "48",
        (object) "49",
        (object) "50",
        (object) "51",
        (object) "52",
        (object) "53",
        (object) "54",
        (object) "55",
        (object) "56",
        (object) "57",
        (object) "58",
        (object) "59"
      });
      this.cmbLDStartSunMM.Location = new Point(765, 46);
      this.cmbLDStartSunMM.Name = "cmbLDStartSunMM";
      this.cmbLDStartSunMM.Size = new Size(40, 21);
      this.cmbLDStartSunMM.TabIndex = 76;
      this.cmbLDStartSunHH.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLDStartSunHH.Enabled = false;
      this.cmbLDStartSunHH.FormattingEnabled = true;
      this.cmbLDStartSunHH.Items.AddRange(new object[13]
      {
        (object) "",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12"
      });
      this.cmbLDStartSunHH.Location = new Point(719, 46);
      this.cmbLDStartSunHH.Name = "cmbLDStartSunHH";
      this.cmbLDStartSunHH.Size = new Size(40, 21);
      this.cmbLDStartSunHH.TabIndex = 75;
      this.txtLockDeskHourMessage.Enabled = false;
      this.txtLockDeskHourMessage.Location = new Point(23, 201);
      this.txtLockDeskHourMessage.MaxLength = 250;
      this.txtLockDeskHourMessage.Multiline = true;
      this.txtLockDeskHourMessage.Name = "txtLockDeskHourMessage";
      this.txtLockDeskHourMessage.Size = new Size(391, 68);
      this.txtLockDeskHourMessage.TabIndex = 86;
      this.txtLockDeskHourMessage.TextChanged += new EventHandler(this.txtLockDeskHourMessage_TextChanged);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(20, 182);
      this.label10.Name = "label10";
      this.label10.Size = new Size(234, 13);
      this.label10.TabIndex = 87;
      this.label10.Text = "Lock Desk Hours Message (character limit 250):";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(476, 182);
      this.label11.Name = "label11";
      this.label11.Size = new Size(259, 13);
      this.label11.TabIndex = 89;
      this.label11.Text = "Lock Desk Shut Down Message (character limit 250):";
      this.txtLockDeskHourShutDownMessage.Location = new Point(479, 201);
      this.txtLockDeskHourShutDownMessage.MaxLength = 250;
      this.txtLockDeskHourShutDownMessage.Multiline = true;
      this.txtLockDeskHourShutDownMessage.Name = "txtLockDeskHourShutDownMessage";
      this.txtLockDeskHourShutDownMessage.Size = new Size(391, 68);
      this.txtLockDeskHourShutDownMessage.TabIndex = 88;
      this.txtLockDeskHourShutDownMessage.TextChanged += new EventHandler(this.txtLockDeskHourShutDownMessage_TextChanged);
      this.chkShutDownLockDesk.AutoSize = true;
      this.chkShutDownLockDesk.Location = new Point(479, 131);
      this.chkShutDownLockDesk.Name = "chkShutDownLockDesk";
      this.chkShutDownLockDesk.Size = new Size(193, 17);
      this.chkShutDownLockDesk.TabIndex = 90;
      this.chkShutDownLockDesk.Text = "Shut Down Lock Desk (Temporary)";
      this.chkShutDownLockDesk.UseVisualStyleBackColor = true;
      this.chkShutDownLockDesk.CheckedChanged += new EventHandler(this.chkShutDownLockDesk_CheckedChanged);
      this.panel1.BackColor = Color.White;
      this.panel1.Controls.Add((Control) this.chkAllowActiveRelock);
      this.panel1.Controls.Add((Control) this.cmbLDStartWeekdayMM);
      this.panel1.Controls.Add((Control) this.chkShutDownLockDesk);
      this.panel1.Controls.Add((Control) this.label11);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Controls.Add((Control) this.txtLockDeskHourShutDownMessage);
      this.panel1.Controls.Add((Control) this.cmbLDStartWeekdayHH);
      this.panel1.Controls.Add((Control) this.label10);
      this.panel1.Controls.Add((Control) this.label12);
      this.panel1.Controls.Add((Control) this.txtLockDeskHourMessage);
      this.panel1.Controls.Add((Control) this.lblLockDeskStart);
      this.panel1.Controls.Add((Control) this.chkLDSunHours);
      this.panel1.Controls.Add((Control) this.cmbLDEndWeekdayHH);
      this.panel1.Controls.Add((Control) this.cmbLDEndSunBN);
      this.panel1.Controls.Add((Control) this.cmbLDEndWeekdayMM);
      this.panel1.Controls.Add((Control) this.cmbLDStartSunBN);
      this.panel1.Controls.Add((Control) this.label15);
      this.panel1.Controls.Add((Control) this.label6);
      this.panel1.Controls.Add((Control) this.lblLockDeskEnd);
      this.panel1.Controls.Add((Control) this.label7);
      this.panel1.Controls.Add((Control) this.cmbLDStartWeekdayBN);
      this.panel1.Controls.Add((Control) this.cmbLDEndSunMM);
      this.panel1.Controls.Add((Control) this.cmbLDEndWeekdayBN);
      this.panel1.Controls.Add((Control) this.cmbLDEndSunHH);
      this.panel1.Controls.Add((Control) this.cmbLDStartSatHH);
      this.panel1.Controls.Add((Control) this.label8);
      this.panel1.Controls.Add((Control) this.cmbLDStartSatMM);
      this.panel1.Controls.Add((Control) this.label9);
      this.panel1.Controls.Add((Control) this.label5);
      this.panel1.Controls.Add((Control) this.cmbLDStartSunMM);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.cmbLDStartSunHH);
      this.panel1.Controls.Add((Control) this.cmbLDEndSatHH);
      this.panel1.Controls.Add((Control) this.chkLDSatHours);
      this.panel1.Controls.Add((Control) this.cmbLDEndSatMM);
      this.panel1.Controls.Add((Control) this.cmbLDEndSatBN);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.cmbLDStartSatBN);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(1592, 291);
      this.panel1.TabIndex = 92;
      this.chkAllowActiveRelock.AutoSize = true;
      this.chkAllowActiveRelock.Location = new Point(498, 155);
      this.chkAllowActiveRelock.Name = "chkAllowActiveRelock";
      this.chkAllowActiveRelock.Size = new Size(368, 17);
      this.chkAllowActiveRelock.TabIndex = 91;
      this.chkAllowActiveRelock.Text = "Allow Lock Update Requests (Active Locks) during lock desk shut down";
      this.chkAllowActiveRelock.UseVisualStyleBackColor = true;
      this.chkAllowActiveRelock.CheckedChanged += new EventHandler(this.chkAllowActiveRelock_CheckedChanged);
      this.panel2.Controls.Add((Control) this.groupONRPContainer);
      this.panel2.Dock = DockStyle.Top;
      this.panel2.Location = new Point(0, 291);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(1592, 382);
      this.panel2.TabIndex = 93;
      this.groupONRPContainer.Controls.Add((Control) this.onrpChannelTab);
      this.groupONRPContainer.Controls.Add((Control) this.panel3);
      this.groupONRPContainer.Controls.Add((Control) this.panelFiller);
      this.groupONRPContainer.Dock = DockStyle.Fill;
      this.groupONRPContainer.HeaderForeColor = SystemColors.ControlText;
      this.groupONRPContainer.Location = new Point(0, 0);
      this.groupONRPContainer.Name = "groupONRPContainer";
      this.groupONRPContainer.Size = new Size(1592, 382);
      this.groupONRPContainer.TabIndex = 91;
      this.groupONRPContainer.Text = "[Branch] ONRP Settings";
      this.onrpChannelTab.Controls.Add((Control) this.tabRetail);
      this.onrpChannelTab.Controls.Add((Control) this.tabWholesale);
      this.onrpChannelTab.Controls.Add((Control) this.tabCorrespondent);
      this.onrpChannelTab.Dock = DockStyle.Top;
      this.onrpChannelTab.Location = new Point(1, 43);
      this.onrpChannelTab.Name = "onrpChannelTab";
      this.onrpChannelTab.SelectedIndex = 0;
      this.onrpChannelTab.Size = new Size(1590, 21);
      this.onrpChannelTab.TabIndex = 114;
      this.onrpChannelTab.SelectedIndexChanged += new EventHandler(this.onrpChannelTab_SelectedIndexChanged);
      this.onrpChannelTab.Deselected += new TabControlEventHandler(this.onrpChannelTab_Deselected);
      this.onrpChannelTab.TabIndexChanged += new EventHandler(this.onrpChannelTab_TabIndexChanged);
      this.tabRetail.Location = new Point(4, 22);
      this.tabRetail.Name = "tabRetail";
      this.tabRetail.Padding = new Padding(3);
      this.tabRetail.Size = new Size(1582, 0);
      this.tabRetail.TabIndex = 0;
      this.tabRetail.Text = "Retail";
      this.tabRetail.UseVisualStyleBackColor = true;
      this.tabWholesale.Location = new Point(4, 22);
      this.tabWholesale.Name = "tabWholesale";
      this.tabWholesale.Padding = new Padding(3);
      this.tabWholesale.Size = new Size(1582, 0);
      this.tabWholesale.TabIndex = 1;
      this.tabWholesale.Text = "Wholesale";
      this.tabWholesale.UseVisualStyleBackColor = true;
      this.tabCorrespondent.Location = new Point(4, 22);
      this.tabCorrespondent.Name = "tabCorrespondent";
      this.tabCorrespondent.Size = new Size(1582, 0);
      this.tabCorrespondent.TabIndex = 2;
      this.tabCorrespondent.Text = "Correspondent";
      this.tabCorrespondent.UseVisualStyleBackColor = true;
      this.panel3.BackColor = Color.White;
      this.panel3.Controls.Add((Control) this.chkAllowONRPForCancelledExpiredLocks);
      this.panel3.Controls.Add((Control) this.chkEnableONRP);
      this.panel3.Controls.Add((Control) this.lblCoverage);
      this.panel3.Controls.Add((Control) this.lblSunEndTime);
      this.panel3.Controls.Add((Control) this.rdoContinuousCoverage);
      this.panel3.Controls.Add((Control) this.lblSunStartTime);
      this.panel3.Controls.Add((Control) this.rdoSpecifyTime);
      this.panel3.Controls.Add((Control) this.lblSatEndTime);
      this.panel3.Controls.Add((Control) this.txtOnrpWkdayStartTime);
      this.panel3.Controls.Add((Control) this.lblSatStartTime);
      this.panel3.Controls.Add((Control) this.cmbOnrpWkdayEndHH);
      this.panel3.Controls.Add((Control) this.txtOnrpSunStartTime);
      this.panel3.Controls.Add((Control) this.cmbOnrpWkdayEndMM);
      this.panel3.Controls.Add((Control) this.txtOnrpSatStartTime);
      this.panel3.Controls.Add((Control) this.chkWeekendHolidayCoverage);
      this.panel3.Controls.Add((Control) this.chkOnrpSunHours);
      this.panel3.Controls.Add((Control) this.chkNoMaxLimit);
      this.panel3.Controls.Add((Control) this.cmbOnrpSunEndBN);
      this.panel3.Controls.Add((Control) this.lblDollarLimit);
      this.panel3.Controls.Add((Control) this.lblSunET);
      this.panel3.Controls.Add((Control) this.txtDollarLimit);
      this.panel3.Controls.Add((Control) this.cmbOnrpSunEndMM);
      this.panel3.Controls.Add((Control) this.lblTolerance);
      this.panel3.Controls.Add((Control) this.cmbOnrpSunEndHH);
      this.panel3.Controls.Add((Control) this.txtTolerance);
      this.panel3.Controls.Add((Control) this.cmbOnrpSatEndHH);
      this.panel3.Controls.Add((Control) this.cmbOnrpWkdayEndBN);
      this.panel3.Controls.Add((Control) this.chkOnrpSatHours);
      this.panel3.Controls.Add((Control) this.lblWdStartTime);
      this.panel3.Controls.Add((Control) this.cmbOnrpSatEndMM);
      this.panel3.Controls.Add((Control) this.lblWdEndTime);
      this.panel3.Controls.Add((Control) this.cmbOnrpSatEndBN);
      this.panel3.Controls.Add((Control) this.lblWeekdayHours);
      this.panel3.Controls.Add((Control) this.lblSatET);
      this.panel3.Controls.Add((Control) this.lblWdET);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(1, 43);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(1590, 338);
      this.panel3.TabIndex = 115;
      this.chkAllowONRPForCancelledExpiredLocks.AutoSize = true;
      this.chkAllowONRPForCancelledExpiredLocks.Location = new Point(52, 56);
      this.chkAllowONRPForCancelledExpiredLocks.Name = "chkAllowONRPForCancelledExpiredLocks";
      this.chkAllowONRPForCancelledExpiredLocks.Size = new Size(222, 17);
      this.chkAllowONRPForCancelledExpiredLocks.TabIndex = 114;
      this.chkAllowONRPForCancelledExpiredLocks.Text = "Allow ONRP for Cancelled/Expired Locks";
      this.chkAllowONRPForCancelledExpiredLocks.UseVisualStyleBackColor = true;
      this.chkAllowONRPForCancelledExpiredLocks.Visible = false;
      this.chkAllowONRPForCancelledExpiredLocks.CheckedChanged += new EventHandler(this.chkAllowONRPForCancelledExpiredLocks_CheckedChanged);
      this.chkEnableONRP.AutoSize = true;
      this.chkEnableONRP.Location = new Point(25, 36);
      this.chkEnableONRP.Name = "chkEnableONRP";
      this.chkEnableONRP.Size = new Size(151, 17);
      this.chkEnableONRP.TabIndex = 44;
      this.chkEnableONRP.Text = "Enable ONRP for [Branch]";
      this.chkEnableONRP.UseVisualStyleBackColor = true;
      this.chkEnableONRP.CheckedChanged += new EventHandler(this.chkEnableOnrp_CheckedChanged);
      this.lblCoverage.AutoSize = true;
      this.lblCoverage.Location = new Point(42, 74);
      this.lblCoverage.Name = "lblCoverage";
      this.lblCoverage.Size = new Size(53, 13);
      this.lblCoverage.TabIndex = 45;
      this.lblCoverage.Text = "Coverage";
      this.lblSunEndTime.AutoSize = true;
      this.lblSunEndTime.Location = new Point(680, 197);
      this.lblSunEndTime.Name = "lblSunEndTime";
      this.lblSunEndTime.Size = new Size(86, 13);
      this.lblSunEndTime.TabIndex = 113;
      this.lblSunEndTime.Text = "ONRP End Time";
      this.rdoContinuousCoverage.AutoSize = true;
      this.rdoContinuousCoverage.Location = new Point(50, 95);
      this.rdoContinuousCoverage.Name = "rdoContinuousCoverage";
      this.rdoContinuousCoverage.Size = new Size(161, 17);
      this.rdoContinuousCoverage.TabIndex = 46;
      this.rdoContinuousCoverage.TabStop = true;
      this.rdoContinuousCoverage.Text = "Continuous ONRP Coverage";
      this.rdoContinuousCoverage.UseVisualStyleBackColor = true;
      this.rdoContinuousCoverage.CheckedChanged += new EventHandler(this.rbSpecifyTime_CheckedChanged);
      this.lblSunStartTime.AutoSize = true;
      this.lblSunStartTime.Location = new Point(680, 170);
      this.lblSunStartTime.Name = "lblSunStartTime";
      this.lblSunStartTime.Size = new Size(89, 13);
      this.lblSunStartTime.TabIndex = 112;
      this.lblSunStartTime.Text = "ONRP Start Time";
      this.rdoSpecifyTime.AutoSize = true;
      this.rdoSpecifyTime.Location = new Point(50, 118);
      this.rdoSpecifyTime.Name = "rdoSpecifyTime";
      this.rdoSpecifyTime.Size = new Size(86, 17);
      this.rdoSpecifyTime.TabIndex = 47;
      this.rdoSpecifyTime.TabStop = true;
      this.rdoSpecifyTime.Text = "Specify Time";
      this.rdoSpecifyTime.UseVisualStyleBackColor = true;
      this.rdoSpecifyTime.CheckedChanged += new EventHandler(this.rbSpecifyTime_CheckedChanged);
      this.lblSatEndTime.AutoSize = true;
      this.lblSatEndTime.Location = new Point(367, 193);
      this.lblSatEndTime.Name = "lblSatEndTime";
      this.lblSatEndTime.Size = new Size(86, 13);
      this.lblSatEndTime.TabIndex = 111;
      this.lblSatEndTime.Text = "ONRP End Time";
      this.txtOnrpWkdayStartTime.Enabled = false;
      this.txtOnrpWkdayStartTime.Location = new Point(165, 163);
      this.txtOnrpWkdayStartTime.Name = "txtOnrpWkdayStartTime";
      this.txtOnrpWkdayStartTime.ReadOnly = true;
      this.txtOnrpWkdayStartTime.Size = new Size(130, 20);
      this.txtOnrpWkdayStartTime.TabIndex = 48;
      this.lblSatStartTime.AutoSize = true;
      this.lblSatStartTime.Location = new Point(367, 166);
      this.lblSatStartTime.Name = "lblSatStartTime";
      this.lblSatStartTime.Size = new Size(89, 13);
      this.lblSatStartTime.TabIndex = 110;
      this.lblSatStartTime.Text = "ONRP Start Time";
      this.cmbOnrpWkdayEndHH.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbOnrpWkdayEndHH.FormattingEnabled = true;
      this.cmbOnrpWkdayEndHH.Items.AddRange(new object[13]
      {
        (object) "",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12"
      });
      this.cmbOnrpWkdayEndHH.Location = new Point(165, 190);
      this.cmbOnrpWkdayEndHH.Name = "cmbOnrpWkdayEndHH";
      this.cmbOnrpWkdayEndHH.Size = new Size(40, 21);
      this.cmbOnrpWkdayEndHH.TabIndex = 49;
      this.txtOnrpSunStartTime.Enabled = false;
      this.txtOnrpSunStartTime.Location = new Point(776, 163);
      this.txtOnrpSunStartTime.Name = "txtOnrpSunStartTime";
      this.txtOnrpSunStartTime.ReadOnly = true;
      this.txtOnrpSunStartTime.Size = new Size(132, 20);
      this.txtOnrpSunStartTime.TabIndex = 109;
      this.cmbOnrpWkdayEndMM.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbOnrpWkdayEndMM.FormattingEnabled = true;
      this.cmbOnrpWkdayEndMM.Items.AddRange(new object[61]
      {
        (object) "",
        (object) "00",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12",
        (object) "13",
        (object) "14",
        (object) "15",
        (object) "16",
        (object) "17",
        (object) "18",
        (object) "19",
        (object) "20",
        (object) "21",
        (object) "22",
        (object) "23",
        (object) "24",
        (object) "25",
        (object) "26",
        (object) "27",
        (object) "28",
        (object) "29",
        (object) "30",
        (object) "31",
        (object) "32",
        (object) "33",
        (object) "34",
        (object) "35",
        (object) "36",
        (object) "37",
        (object) "38",
        (object) "39",
        (object) "40",
        (object) "41",
        (object) "42",
        (object) "43",
        (object) "44",
        (object) "45",
        (object) "46",
        (object) "47",
        (object) "48",
        (object) "49",
        (object) "50",
        (object) "51",
        (object) "52",
        (object) "53",
        (object) "54",
        (object) "55",
        (object) "56",
        (object) "57",
        (object) "58",
        (object) "59"
      });
      this.cmbOnrpWkdayEndMM.Location = new Point(209, 190);
      this.cmbOnrpWkdayEndMM.Name = "cmbOnrpWkdayEndMM";
      this.cmbOnrpWkdayEndMM.Size = new Size(40, 21);
      this.cmbOnrpWkdayEndMM.TabIndex = 50;
      this.txtOnrpSatStartTime.Enabled = false;
      this.txtOnrpSatStartTime.Location = new Point(463, 163);
      this.txtOnrpSatStartTime.Name = "txtOnrpSatStartTime";
      this.txtOnrpSatStartTime.ReadOnly = true;
      this.txtOnrpSatStartTime.Size = new Size(132, 20);
      this.txtOnrpSatStartTime.TabIndex = 108;
      this.chkWeekendHolidayCoverage.AutoSize = true;
      this.chkWeekendHolidayCoverage.Location = new Point(55, 220);
      this.chkWeekendHolidayCoverage.Name = "chkWeekendHolidayCoverage";
      this.chkWeekendHolidayCoverage.Size = new Size(168, 17);
      this.chkWeekendHolidayCoverage.TabIndex = 51;
      this.chkWeekendHolidayCoverage.Text = "Weekend / Holiday Coverage";
      this.chkWeekendHolidayCoverage.UseVisualStyleBackColor = true;
      this.chkWeekendHolidayCoverage.CheckedChanged += new EventHandler(this.chkWkHolidayCoverage_CheckedChanged);
      this.chkOnrpSunHours.AutoSize = true;
      this.chkOnrpSunHours.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkOnrpSunHours.Location = new Point(776, 133);
      this.chkOnrpSunHours.Name = "chkOnrpSunHours";
      this.chkOnrpSunHours.Size = new Size(105, 17);
      this.chkOnrpSunHours.TabIndex = 107;
      this.chkOnrpSunHours.Text = "Sunday Hours";
      this.chkOnrpSunHours.UseVisualStyleBackColor = true;
      this.chkOnrpSunHours.CheckedChanged += new EventHandler(this.chkOnrpSunHours_CheckedChanged);
      this.chkNoMaxLimit.AutoSize = true;
      this.chkNoMaxLimit.Location = new Point(29, 251);
      this.chkNoMaxLimit.Name = "chkNoMaxLimit";
      this.chkNoMaxLimit.Size = new Size(111, 17);
      this.chkNoMaxLimit.TabIndex = 52;
      this.chkNoMaxLimit.Text = "No Maximum Limit";
      this.chkNoMaxLimit.UseVisualStyleBackColor = true;
      this.chkNoMaxLimit.CheckedChanged += new EventHandler(this.chkNoMaxLimit_CheckedChanged);
      this.cmbOnrpSunEndBN.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbOnrpSunEndBN.FormattingEnabled = true;
      this.cmbOnrpSunEndBN.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "AM",
        (object) "PM"
      });
      this.cmbOnrpSunEndBN.Location = new Point(868, 189);
      this.cmbOnrpSunEndBN.Name = "cmbOnrpSunEndBN";
      this.cmbOnrpSunEndBN.Size = new Size(40, 21);
      this.cmbOnrpSunEndBN.TabIndex = 106;
      this.lblDollarLimit.AutoSize = true;
      this.lblDollarLimit.Location = new Point(26, 281);
      this.lblDollarLimit.Name = "lblDollarLimit";
      this.lblDollarLimit.Size = new Size(101, 13);
      this.lblDollarLimit.TabIndex = 53;
      this.lblDollarLimit.Text = "ONRP Dollar Limit $";
      this.lblSunET.AutoSize = true;
      this.lblSunET.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.lblSunET.Location = new Point(914, 194);
      this.lblSunET.Name = "lblSunET";
      this.lblSunET.Size = new Size(21, 13);
      this.lblSunET.TabIndex = 103;
      this.lblSunET.Text = "ET";
      this.txtDollarLimit.Cursor = Cursors.IBeam;
      this.txtDollarLimit.Location = new Point(131, 277);
      this.txtDollarLimit.MaxLength = 8;
      this.txtDollarLimit.Name = "txtDollarLimit";
      this.txtDollarLimit.Size = new Size(120, 20);
      this.txtDollarLimit.TabIndex = 54;
      this.txtDollarLimit.TextChanged += new EventHandler(this.txtDollarLimit_TextChanged);
      this.cmbOnrpSunEndMM.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbOnrpSunEndMM.FormattingEnabled = true;
      this.cmbOnrpSunEndMM.Items.AddRange(new object[61]
      {
        (object) "",
        (object) "00",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12",
        (object) "13",
        (object) "14",
        (object) "15",
        (object) "16",
        (object) "17",
        (object) "18",
        (object) "19",
        (object) "20",
        (object) "21",
        (object) "22",
        (object) "23",
        (object) "24",
        (object) "25",
        (object) "26",
        (object) "27",
        (object) "28",
        (object) "29",
        (object) "30",
        (object) "31",
        (object) "32",
        (object) "33",
        (object) "34",
        (object) "35",
        (object) "36",
        (object) "37",
        (object) "38",
        (object) "39",
        (object) "40",
        (object) "41",
        (object) "42",
        (object) "43",
        (object) "44",
        (object) "45",
        (object) "46",
        (object) "47",
        (object) "48",
        (object) "49",
        (object) "50",
        (object) "51",
        (object) "52",
        (object) "53",
        (object) "54",
        (object) "55",
        (object) "56",
        (object) "57",
        (object) "58",
        (object) "59"
      });
      this.cmbOnrpSunEndMM.Location = new Point(822, 189);
      this.cmbOnrpSunEndMM.Name = "cmbOnrpSunEndMM";
      this.cmbOnrpSunEndMM.Size = new Size(40, 21);
      this.cmbOnrpSunEndMM.TabIndex = 102;
      this.lblTolerance.AutoSize = true;
      this.lblTolerance.Location = new Point(26, 303);
      this.lblTolerance.Name = "lblTolerance";
      this.lblTolerance.Size = new Size(100, 13);
      this.lblTolerance.TabIndex = 55;
      this.lblTolerance.Text = "ONRP Tolerance %";
      this.cmbOnrpSunEndHH.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbOnrpSunEndHH.FormattingEnabled = true;
      this.cmbOnrpSunEndHH.Items.AddRange(new object[13]
      {
        (object) "",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12"
      });
      this.cmbOnrpSunEndHH.Location = new Point(776, 189);
      this.cmbOnrpSunEndHH.Name = "cmbOnrpSunEndHH";
      this.cmbOnrpSunEndHH.Size = new Size(40, 21);
      this.cmbOnrpSunEndHH.TabIndex = 101;
      this.txtTolerance.Location = new Point(131, 299);
      this.txtTolerance.MaxLength = 2;
      this.txtTolerance.Name = "txtTolerance";
      this.txtTolerance.Size = new Size(120, 20);
      this.txtTolerance.TabIndex = 56;
      this.txtTolerance.TextChanged += new EventHandler(this.txtTolerance_TextChanged);
      this.cmbOnrpSatEndHH.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbOnrpSatEndHH.FormattingEnabled = true;
      this.cmbOnrpSatEndHH.Items.AddRange(new object[13]
      {
        (object) "",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12"
      });
      this.cmbOnrpSatEndHH.Location = new Point(463, 189);
      this.cmbOnrpSatEndHH.Name = "cmbOnrpSatEndHH";
      this.cmbOnrpSatEndHH.Size = new Size(40, 21);
      this.cmbOnrpSatEndHH.TabIndex = 90;
      this.cmbOnrpWkdayEndBN.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbOnrpWkdayEndBN.FormattingEnabled = true;
      this.cmbOnrpWkdayEndBN.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "AM",
        (object) "PM"
      });
      this.cmbOnrpWkdayEndBN.Location = new Point((int) byte.MaxValue, 190);
      this.cmbOnrpWkdayEndBN.Name = "cmbOnrpWkdayEndBN";
      this.cmbOnrpWkdayEndBN.Size = new Size(40, 21);
      this.cmbOnrpWkdayEndBN.TabIndex = 57;
      this.chkOnrpSatHours.AutoSize = true;
      this.chkOnrpSatHours.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkOnrpSatHours.Location = new Point(463, 133);
      this.chkOnrpSatHours.Name = "chkOnrpSatHours";
      this.chkOnrpSatHours.Size = new Size(113, 17);
      this.chkOnrpSatHours.TabIndex = 96;
      this.chkOnrpSatHours.Text = "Saturday Hours";
      this.chkOnrpSatHours.UseVisualStyleBackColor = true;
      this.chkOnrpSatHours.CheckedChanged += new EventHandler(this.chkOnrpSatHours_CheckedChanged);
      this.lblWdStartTime.AutoSize = true;
      this.lblWdStartTime.Location = new Point(70, 166);
      this.lblWdStartTime.Name = "lblWdStartTime";
      this.lblWdStartTime.Size = new Size(89, 13);
      this.lblWdStartTime.TabIndex = 58;
      this.lblWdStartTime.Text = "ONRP Start Time";
      this.cmbOnrpSatEndMM.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbOnrpSatEndMM.FormattingEnabled = true;
      this.cmbOnrpSatEndMM.Items.AddRange(new object[61]
      {
        (object) "",
        (object) "00",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12",
        (object) "13",
        (object) "14",
        (object) "15",
        (object) "16",
        (object) "17",
        (object) "18",
        (object) "19",
        (object) "20",
        (object) "21",
        (object) "22",
        (object) "23",
        (object) "24",
        (object) "25",
        (object) "26",
        (object) "27",
        (object) "28",
        (object) "29",
        (object) "30",
        (object) "31",
        (object) "32",
        (object) "33",
        (object) "34",
        (object) "35",
        (object) "36",
        (object) "37",
        (object) "38",
        (object) "39",
        (object) "40",
        (object) "41",
        (object) "42",
        (object) "43",
        (object) "44",
        (object) "45",
        (object) "46",
        (object) "47",
        (object) "48",
        (object) "49",
        (object) "50",
        (object) "51",
        (object) "52",
        (object) "53",
        (object) "54",
        (object) "55",
        (object) "56",
        (object) "57",
        (object) "58",
        (object) "59"
      });
      this.cmbOnrpSatEndMM.Location = new Point(509, 189);
      this.cmbOnrpSatEndMM.Name = "cmbOnrpSatEndMM";
      this.cmbOnrpSatEndMM.Size = new Size(40, 21);
      this.cmbOnrpSatEndMM.TabIndex = 91;
      this.lblWdEndTime.AutoSize = true;
      this.lblWdEndTime.Location = new Point(70, 193);
      this.lblWdEndTime.Name = "lblWdEndTime";
      this.lblWdEndTime.Size = new Size(86, 13);
      this.lblWdEndTime.TabIndex = 59;
      this.lblWdEndTime.Text = "ONRP End Time";
      this.cmbOnrpSatEndBN.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbOnrpSatEndBN.FormattingEnabled = true;
      this.cmbOnrpSatEndBN.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "AM",
        (object) "PM"
      });
      this.cmbOnrpSatEndBN.Location = new Point(555, 189);
      this.cmbOnrpSatEndBN.Name = "cmbOnrpSatEndBN";
      this.cmbOnrpSatEndBN.Size = new Size(40, 21);
      this.cmbOnrpSatEndBN.TabIndex = 95;
      this.lblWeekdayHours.AutoSize = true;
      this.lblWeekdayHours.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblWeekdayHours.Location = new Point(162, 133);
      this.lblWeekdayHours.Name = "lblWeekdayHours";
      this.lblWeekdayHours.Size = new Size(97, 13);
      this.lblWeekdayHours.TabIndex = 60;
      this.lblWeekdayHours.Text = "Weekday Hours";
      this.lblSatET.AutoSize = true;
      this.lblSatET.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.lblSatET.Location = new Point(601, 194);
      this.lblSatET.Name = "lblSatET";
      this.lblSatET.Size = new Size(21, 13);
      this.lblSatET.TabIndex = 92;
      this.lblSatET.Text = "ET";
      this.lblWdET.AutoSize = true;
      this.lblWdET.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.lblWdET.Location = new Point(301, 194);
      this.lblWdET.Name = "lblWdET";
      this.lblWdET.Size = new Size(21, 13);
      this.lblWdET.TabIndex = 61;
      this.lblWdET.Text = "ET";
      this.panelFiller.BackColor = Color.WhiteSmoke;
      this.panelFiller.Dock = DockStyle.Top;
      this.panelFiller.Location = new Point(1, 26);
      this.panelFiller.Name = "panelFiller";
      this.panelFiller.Size = new Size(1590, 17);
      this.panelFiller.TabIndex = 114;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSize = true;
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.Name = nameof (LockDeskHourControl);
      this.Size = new Size(1592, 680);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.groupONRPContainer.ResumeLayout(false);
      this.onrpChannelTab.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.ResumeLayout(false);
    }

    public delegate void CentralChannelSelected(LoanChannel channel, bool centralChannel);

    public delegate void onrpChannelChanged(LoanChannel channel);

    public delegate void onrpChannelChangedFrom(LoanChannel channel);

    public delegate void IsDirty(bool value);

    public delegate void EnableOnrpChanged(object sender, EventArgs e);
  }
}
