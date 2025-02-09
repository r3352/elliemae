// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.OverNightRateProtection.ONRPToleranceSetting
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.OverNightRateProtection
{
  public class ONRPToleranceSetting : Form
  {
    private ONRPEntitySettings originalSettings;
    private LockDeskGlobalSettings globalSettings;
    public ONRPEntitySettings Settings;
    private bool populateValue;
    private bool enableBranch;
    private Sessions.Session session;
    private bool readOnly;
    private IContainer components;
    private Panel panel1;
    private TextBox txtONRPTolerance;
    private TextBox txtDollarLimit;
    private Label label6;
    private Label label5;
    private CheckBox chkMaximumLimit;
    private CheckBox chkWeekendHolidayCoverage;
    private Label label4;
    private TextBox txtONRPStartTime;
    private ComboBox cmbAMPM;
    private ComboBox cmbMinutes;
    private ComboBox cmbHours;
    private Label label3;
    private Label label2;
    private RadioButton rdoSpecifyTimes;
    private RadioButton rdoContinuousCoverage;
    private Label label1;
    private CheckBox chkEnableONRP;
    private Button btnOK;
    private RadioButton rdoCustomizeSettings;
    private RadioButton rdoUseChannelDefault;
    private Panel panel3;
    private Panel panel2;
    private Button btnCancel;
    private Label label10;
    private TextBox txtONRPSunStartTime;
    private ComboBox cmbSunAMPM;
    private ComboBox cmbSunMinutes;
    private ComboBox cmbSunHours;
    private Label label11;
    private Label label12;
    private CheckBox chkEnableSunONRP;
    private Label label7;
    private TextBox txtONRPSatStartTime;
    private ComboBox cmbSatAMPM;
    private ComboBox cmbSatMinutes;
    private ComboBox cmbSatHours;
    private Label label8;
    private Label label9;
    private CheckBox chkEnableSatONRP;
    private Label label14;
    private Label label13;

    public ONRPToleranceSetting(bool readOnly)
      : this(Session.DefaultInstance)
    {
      this.readOnly = readOnly;
    }

    public ONRPToleranceSetting(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtDollarLimit, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      TextBoxFormatter.Attach(this.txtONRPTolerance, TextBoxContentRule.NonNegativeDecimal, "##");
      this.globalSettings = new LockDeskGlobalSettings(Session.ServerManager.GetServerSettings("Policies"), LoanChannel.BankedRetail);
      this.enableBranch = Utils.ParseBoolean((object) this.globalSettings.ONRPEnabled.ToString());
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.chkEnableONRP.Enabled)
      {
        this.Settings = this.originalSettings;
        this.DialogResult = DialogResult.OK;
        this.Dispose();
      }
      else
      {
        this.Settings.EnableONRP = this.chkEnableONRP.Checked;
        this.Settings.ContinuousCoverage = this.rdoContinuousCoverage.Checked;
        this.Settings.DollarLimit = Utils.ParseDouble((object) this.txtDollarLimit.Text, 0.0);
        this.Settings.ONRPStartTime = this.txtONRPStartTime.Text.Replace("ET", string.Empty);
        this.Settings.ONRPSatStartTime = this.txtONRPSatStartTime.Text.Replace("ET", string.Empty);
        this.Settings.ONRPSunStartTime = this.txtONRPSunStartTime.Text.Replace("ET", string.Empty);
        this.Settings.ONRPEndTime = this.cmbHours.Text.ToString() == "" || this.cmbMinutes.Text.ToString() == "" || this.cmbAMPM.Text.ToString() == "" ? "" : ONRPEntitySettings.ConvertToTimeSpan(int.Parse(this.cmbHours.SelectedItem.ToString()), int.Parse(this.cmbMinutes.SelectedItem.ToString()), this.cmbAMPM.SelectedItem.ToString()).ToString("hh\\:mm");
        this.Settings.ONRPSatEndTime = this.cmbSatHours.SelectedItem.ToString() == "" || this.cmbSatMinutes.SelectedItem.ToString() == "" || this.cmbSatAMPM.SelectedItem.ToString() == "" ? "" : ONRPEntitySettings.ConvertToTimeSpan(int.Parse(this.cmbSatHours.SelectedItem.ToString()), int.Parse(this.cmbSatMinutes.SelectedItem.ToString()), this.cmbSatAMPM.SelectedItem.ToString()).ToString("hh\\:mm");
        this.Settings.EnableSatONRP = this.chkEnableSatONRP.Checked;
        this.Settings.ONRPSunEndTime = this.cmbSunHours.SelectedItem.ToString() == "" || this.cmbSunMinutes.SelectedItem.ToString() == "" || this.cmbSunAMPM.SelectedItem.ToString() == "" ? "" : ONRPEntitySettings.ConvertToTimeSpan(int.Parse(this.cmbSunHours.SelectedItem.ToString()), int.Parse(this.cmbSunMinutes.SelectedItem.ToString()), this.cmbSunAMPM.SelectedItem.ToString()).ToString("hh\\:mm");
        this.Settings.EnableSunONRP = this.chkEnableSunONRP.Checked;
        this.Settings.MaximumLimit = this.chkMaximumLimit.Checked;
        this.Settings.Tolerance = Utils.ParseInt((object) this.txtONRPTolerance.Text, 0);
        this.Settings.UseChannelDefault = this.rdoUseChannelDefault.Checked;
        this.Settings.WeekendHolidayCoverage = this.chkWeekendHolidayCoverage.Checked;
        bool resetEndTime = false;
        if (!ONRPUtils.ValidateSettings((IONRPRuleHandler) new EncompassMessageHanlder((IWin32Window) this), this.Settings, out resetEndTime))
        {
          if (!resetEndTime)
            return;
          this.ResetEndTime(DateTime.MinValue, "weekday");
        }
        else
        {
          this.DialogResult = DialogResult.OK;
          this.Dispose();
        }
      }
    }

    public void RefreshData(ONRPEntitySettings settings)
    {
      this.Settings = settings;
      if (this.enableBranch || this.Settings.EnableONRP)
        return;
      this.btnOK.Enabled = false;
    }

    private void ResetEndTime(DateTime endTime, string time)
    {
      ComboBox comboBox1 = this.cmbHours;
      ComboBox comboBox2 = this.cmbMinutes;
      ComboBox comboBox3 = this.cmbAMPM;
      if (time.ToLower() == "sat")
      {
        comboBox1 = this.cmbSatHours;
        comboBox2 = this.cmbSatMinutes;
        comboBox3 = this.cmbSatAMPM;
      }
      else if (time.ToLower() == "sun")
      {
        comboBox1 = this.cmbSunHours;
        comboBox2 = this.cmbSunMinutes;
        comboBox3 = this.cmbSunAMPM;
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

    private void SetViewStates(ONRPEntitySettings settings)
    {
      this.txtONRPStartTime.Enabled = false;
      if (this.populateValue)
        return;
      this.chkEnableONRP.Enabled = settings.rules.EnableONRP;
      this.rdoUseChannelDefault.Enabled = settings.rules.UseChannelDefaultsAndCustomize;
      this.rdoCustomizeSettings.Enabled = settings.rules.UseChannelDefaultsAndCustomize;
      this.rdoContinuousCoverage.Enabled = settings.rules.ContinuousCoverageAndSpecifyTime;
      this.rdoSpecifyTimes.Enabled = settings.rules.ContinuousCoverageAndSpecifyTime;
      this.cmbHours.Enabled = settings.rules.EndTime;
      this.cmbMinutes.Enabled = settings.rules.EndTime;
      this.cmbAMPM.Enabled = settings.rules.EndTime;
      this.chkWeekendHolidayCoverage.Enabled = settings.rules.WeekendHolidayCoverage;
      this.chkMaximumLimit.Enabled = settings.rules.NoMaxLimit;
      this.txtDollarLimit.Enabled = settings.rules.DollarLimit;
      this.txtONRPTolerance.Enabled = settings.rules.Tolerance;
      this.chkEnableSatONRP.Enabled = settings.rules.EnableSat;
      this.cmbSatHours.Enabled = settings.rules.SatEndTime;
      this.cmbSatMinutes.Enabled = settings.rules.SatEndTime;
      this.cmbSatAMPM.Enabled = settings.rules.SatEndTime;
      this.chkEnableSunONRP.Enabled = settings.rules.EnableSun;
      this.cmbSunHours.Enabled = settings.rules.SunEndTime;
      this.cmbSunMinutes.Enabled = settings.rules.SunEndTime;
      this.cmbSunAMPM.Enabled = settings.rules.SunEndTime;
    }

    private void PopulateValues(ONRPEntitySettings settings)
    {
      this.chkEnableONRP.Checked = settings.EnableONRP;
      this.rdoUseChannelDefault.Checked = settings.UseChannelDefault;
      this.rdoCustomizeSettings.Checked = !settings.UseChannelDefault;
      this.rdoContinuousCoverage.Checked = settings.ContinuousCoverage;
      this.rdoSpecifyTimes.Checked = !settings.ContinuousCoverage;
      this.chkEnableSatONRP.Checked = settings.EnableSatONRP && !settings.rules.TwentyfourhrSat && !settings.ContinuousCoverage && settings.rules.EnableSatLD;
      this.chkEnableSunONRP.Checked = settings.EnableSunONRP && !settings.rules.TwentyfourhrSun && !settings.ContinuousCoverage && settings.rules.EnableSunLD;
      TextBox txtOnrpStartTime = this.txtONRPStartTime;
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
      txtOnrpStartTime.Text = str1;
      if (!string.IsNullOrEmpty(settings.ONRPEndTime))
        this.ResetEndTime(DateTime.Parse(settings.ONRPEndTime), "weekday");
      if (settings.ContinuousCoverage)
      {
        endTime = new DateTime();
        this.ResetEndTime(endTime, "weekday");
      }
      this.chkMaximumLimit.Checked = settings.MaximumLimit;
      this.txtONRPTolerance.Enabled = !this.chkMaximumLimit.Checked && this.chkMaximumLimit.Enabled;
      this.txtDollarLimit.Enabled = !this.chkMaximumLimit.Checked && this.chkMaximumLimit.Enabled;
      this.txtDollarLimit.Text = settings.DollarLimit == 0.0 || this.chkMaximumLimit.Checked ? "" : settings.DollarLimit.ToString("#,##0");
      this.txtONRPTolerance.Text = settings.Tolerance == 0 || this.chkMaximumLimit.Checked ? "" : settings.Tolerance.ToString();
      this.chkWeekendHolidayCoverage.Checked = settings.WeekendHolidayCoverage;
      TextBox onrpSatStartTime = this.txtONRPSatStartTime;
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
      onrpSatStartTime.Text = str2;
      if (!string.IsNullOrEmpty(settings.ONRPSatEndTime))
        this.ResetEndTime(DateTime.Parse(settings.ONRPSatEndTime), "sat");
      if (!settings.EnableSatONRP)
      {
        endTime = new DateTime();
        this.ResetEndTime(endTime, "sat");
      }
      TextBox onrpSunStartTime = this.txtONRPSunStartTime;
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
      onrpSunStartTime.Text = str3;
      if (!string.IsNullOrEmpty(settings.ONRPSunEndTime))
        this.ResetEndTime(DateTime.Parse(settings.ONRPSunEndTime), "sun");
      if (settings.EnableSunONRP)
        return;
      endTime = new DateTime();
      this.ResetEndTime(endTime, "sun");
    }

    private void rdoUseChannelDefault_CheckedChanged(object sender, EventArgs e)
    {
      this.Settings.UseChannelDefault = this.rdoUseChannelDefault.Checked;
      this.SetViewStates(this.Settings);
      this.PopulateValues(this.Settings);
    }

    private void rdoContinuousCoverage_CheckedChanged(object sender, EventArgs e)
    {
      this.Settings.ContinuousCoverage = this.rdoContinuousCoverage.Checked;
      this.SetViewStates(this.Settings);
      this.PopulateValues(this.Settings);
    }

    private void ONRPToleranceSetting_Shown(object sender, EventArgs e)
    {
      if (this.Settings != null)
      {
        this.originalSettings = this.Settings.Clone((IONRPRuleHandler) null, (ONRPBaseRule) new ONRPSettingRules(), this.globalSettings);
        this.SetViewStates(this.Settings);
        this.PopulateValues(this.Settings);
      }
      if (!this.readOnly)
        return;
      this.DisableControl();
    }

    private void chkMaximumLimit_CheckedChanged(object sender, EventArgs e)
    {
      this.Settings.MaximumLimit = this.chkMaximumLimit.Checked;
      this.SetViewStates(this.Settings);
      this.PopulateValues(this.Settings);
    }

    private void chkEnableONRP_CheckedChanged(object sender, EventArgs e)
    {
      this.Settings.EnableONRP = this.chkEnableONRP.Checked;
      this.SetViewStates(this.Settings);
      this.PopulateValues(this.Settings);
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.Settings = this.originalSettings;
      this.DialogResult = DialogResult.Cancel;
      this.Dispose();
    }

    public void DisableControl()
    {
      this.chkEnableONRP.Enabled = false;
      this.rdoUseChannelDefault.Enabled = false;
      this.rdoCustomizeSettings.Enabled = false;
      this.rdoContinuousCoverage.Enabled = false;
      this.rdoSpecifyTimes.Enabled = false;
      this.cmbHours.Enabled = false;
      this.cmbMinutes.Enabled = false;
      this.cmbAMPM.Enabled = false;
      this.chkWeekendHolidayCoverage.Enabled = false;
      this.chkEnableSatONRP.Enabled = false;
      this.cmbSatHours.Enabled = false;
      this.cmbSatMinutes.Enabled = false;
      this.cmbSatAMPM.Enabled = false;
      this.chkEnableSunONRP.Enabled = false;
      this.cmbSunHours.Enabled = false;
      this.cmbSunMinutes.Enabled = false;
      this.cmbSunAMPM.Enabled = false;
      this.chkMaximumLimit.Enabled = false;
      this.txtDollarLimit.Enabled = false;
      this.txtONRPTolerance.Enabled = false;
      this.btnOK.Enabled = false;
    }

    private void chkHours_CheckedChanged(object sender, EventArgs e)
    {
      if (sender == this.chkEnableSatONRP)
        this.Settings.EnableSatONRP = this.chkEnableSatONRP.Checked;
      else
        this.Settings.EnableSunONRP = this.chkEnableSunONRP.Checked;
      this.SetViewStates(this.Settings);
      this.PopulateValues(this.Settings);
    }

    private void chkWeekendHolidayCoverage_CheckedChanged(object sender, EventArgs e)
    {
      this.Settings.WeekendHolidayCoverage = this.chkWeekendHolidayCoverage.Checked;
      this.SetViewStates(this.Settings);
      this.PopulateValues(this.Settings);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.label14 = new Label();
      this.label13 = new Label();
      this.label10 = new Label();
      this.txtONRPSunStartTime = new TextBox();
      this.cmbSunAMPM = new ComboBox();
      this.cmbSunMinutes = new ComboBox();
      this.cmbSunHours = new ComboBox();
      this.label11 = new Label();
      this.label12 = new Label();
      this.chkEnableSunONRP = new CheckBox();
      this.label7 = new Label();
      this.txtONRPSatStartTime = new TextBox();
      this.cmbSatAMPM = new ComboBox();
      this.cmbSatMinutes = new ComboBox();
      this.cmbSatHours = new ComboBox();
      this.label8 = new Label();
      this.label9 = new Label();
      this.chkEnableSatONRP = new CheckBox();
      this.panel3 = new Panel();
      this.rdoContinuousCoverage = new RadioButton();
      this.rdoSpecifyTimes = new RadioButton();
      this.panel2 = new Panel();
      this.rdoUseChannelDefault = new RadioButton();
      this.rdoCustomizeSettings = new RadioButton();
      this.txtONRPTolerance = new TextBox();
      this.txtDollarLimit = new TextBox();
      this.label6 = new Label();
      this.label5 = new Label();
      this.chkMaximumLimit = new CheckBox();
      this.chkWeekendHolidayCoverage = new CheckBox();
      this.label4 = new Label();
      this.txtONRPStartTime = new TextBox();
      this.cmbAMPM = new ComboBox();
      this.cmbMinutes = new ComboBox();
      this.cmbHours = new ComboBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.chkEnableONRP = new CheckBox();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.panel1.SuspendLayout();
      this.panel3.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.panel1.BackColor = Color.White;
      this.panel1.BorderStyle = BorderStyle.FixedSingle;
      this.panel1.Controls.Add((Control) this.label14);
      this.panel1.Controls.Add((Control) this.label13);
      this.panel1.Controls.Add((Control) this.label10);
      this.panel1.Controls.Add((Control) this.txtONRPSunStartTime);
      this.panel1.Controls.Add((Control) this.cmbSunAMPM);
      this.panel1.Controls.Add((Control) this.cmbSunMinutes);
      this.panel1.Controls.Add((Control) this.cmbSunHours);
      this.panel1.Controls.Add((Control) this.label11);
      this.panel1.Controls.Add((Control) this.label12);
      this.panel1.Controls.Add((Control) this.chkEnableSunONRP);
      this.panel1.Controls.Add((Control) this.label7);
      this.panel1.Controls.Add((Control) this.txtONRPSatStartTime);
      this.panel1.Controls.Add((Control) this.cmbSatAMPM);
      this.panel1.Controls.Add((Control) this.cmbSatMinutes);
      this.panel1.Controls.Add((Control) this.cmbSatHours);
      this.panel1.Controls.Add((Control) this.label8);
      this.panel1.Controls.Add((Control) this.label9);
      this.panel1.Controls.Add((Control) this.chkEnableSatONRP);
      this.panel1.Controls.Add((Control) this.panel3);
      this.panel1.Controls.Add((Control) this.panel2);
      this.panel1.Controls.Add((Control) this.txtONRPTolerance);
      this.panel1.Controls.Add((Control) this.txtDollarLimit);
      this.panel1.Controls.Add((Control) this.label6);
      this.panel1.Controls.Add((Control) this.label5);
      this.panel1.Controls.Add((Control) this.chkMaximumLimit);
      this.panel1.Controls.Add((Control) this.chkWeekendHolidayCoverage);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.txtONRPStartTime);
      this.panel1.Controls.Add((Control) this.cmbAMPM);
      this.panel1.Controls.Add((Control) this.cmbMinutes);
      this.panel1.Controls.Add((Control) this.cmbHours);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Controls.Add((Control) this.chkEnableONRP);
      this.panel1.Location = new Point(12, 12);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(402, 546);
      this.panel1.TabIndex = 0;
      this.label14.AutoSize = true;
      this.label14.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label14.Location = new Point(94, 171);
      this.label14.Name = "label14";
      this.label14.Size = new Size(97, 13);
      this.label14.TabIndex = 39;
      this.label14.Text = "Weekday Hours";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(306, 509);
      this.label13.Name = "label13";
      this.label13.Size = new Size(15, 13);
      this.label13.TabIndex = 38;
      this.label13.Text = "%";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(362, 433);
      this.label10.Name = "label10";
      this.label10.Size = new Size(21, 13);
      this.label10.TabIndex = 37;
      this.label10.Text = "ET";
      this.txtONRPSunStartTime.Enabled = false;
      this.txtONRPSunStartTime.Location = new Point(197, 401);
      this.txtONRPSunStartTime.Name = "txtONRPSunStartTime";
      this.txtONRPSunStartTime.Size = new Size(145, 20);
      this.txtONRPSunStartTime.TabIndex = 36;
      this.cmbSunAMPM.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbSunAMPM.FormattingEnabled = true;
      this.cmbSunAMPM.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "AM",
        (object) "PM"
      });
      this.cmbSunAMPM.Location = new Point(307, 426);
      this.cmbSunAMPM.Name = "cmbSunAMPM";
      this.cmbSunAMPM.Size = new Size(49, 21);
      this.cmbSunAMPM.TabIndex = 35;
      this.cmbSunMinutes.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbSunMinutes.FormattingEnabled = true;
      this.cmbSunMinutes.Items.AddRange(new object[61]
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
        (object) "29 ",
        (object) "30 ",
        (object) "31 ",
        (object) "32 ",
        (object) "33 ",
        (object) "34 ",
        (object) "35 ",
        (object) "36 ",
        (object) "37 ",
        (object) "38 ",
        (object) "39 ",
        (object) "40 ",
        (object) "41 ",
        (object) "42 ",
        (object) "43 ",
        (object) "44 ",
        (object) "45 ",
        (object) "46 ",
        (object) "47 ",
        (object) "48 ",
        (object) "49 ",
        (object) "50 ",
        (object) "51 ",
        (object) "52 ",
        (object) "53 ",
        (object) "54 ",
        (object) "55 ",
        (object) "56 ",
        (object) "57 ",
        (object) "58 ",
        (object) "59 "
      });
      this.cmbSunMinutes.Location = new Point(252, 426);
      this.cmbSunMinutes.Name = "cmbSunMinutes";
      this.cmbSunMinutes.Size = new Size(49, 21);
      this.cmbSunMinutes.TabIndex = 34;
      this.cmbSunHours.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbSunHours.FormattingEnabled = true;
      this.cmbSunHours.Items.AddRange(new object[13]
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
      this.cmbSunHours.Location = new Point(197, 426);
      this.cmbSunHours.Name = "cmbSunHours";
      this.cmbSunHours.Size = new Size(49, 21);
      this.cmbSunHours.TabIndex = 33;
      this.label11.AutoSize = true;
      this.label11.Location = new Point(94, 430);
      this.label11.Name = "label11";
      this.label11.Size = new Size(86, 13);
      this.label11.TabIndex = 32;
      this.label11.Text = "ONRP End Time";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(94, 404);
      this.label12.Name = "label12";
      this.label12.Size = new Size(89, 13);
      this.label12.TabIndex = 31;
      this.label12.Text = "ONRP Start Time";
      this.chkEnableSunONRP.AutoSize = true;
      this.chkEnableSunONRP.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkEnableSunONRP.Location = new Point(76, 373);
      this.chkEnableSunONRP.Name = "chkEnableSunONRP";
      this.chkEnableSunONRP.Size = new Size(105, 17);
      this.chkEnableSunONRP.TabIndex = 30;
      this.chkEnableSunONRP.Text = "Sunday Hours";
      this.chkEnableSunONRP.UseVisualStyleBackColor = true;
      this.chkEnableSunONRP.CheckedChanged += new EventHandler(this.chkHours_CheckedChanged);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(362, 342);
      this.label7.Name = "label7";
      this.label7.Size = new Size(21, 13);
      this.label7.TabIndex = 29;
      this.label7.Text = "ET";
      this.txtONRPSatStartTime.Enabled = false;
      this.txtONRPSatStartTime.Location = new Point(197, 312);
      this.txtONRPSatStartTime.Name = "txtONRPSatStartTime";
      this.txtONRPSatStartTime.Size = new Size(145, 20);
      this.txtONRPSatStartTime.TabIndex = 28;
      this.cmbSatAMPM.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbSatAMPM.FormattingEnabled = true;
      this.cmbSatAMPM.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "AM",
        (object) "PM"
      });
      this.cmbSatAMPM.Location = new Point(307, 337);
      this.cmbSatAMPM.Name = "cmbSatAMPM";
      this.cmbSatAMPM.Size = new Size(49, 21);
      this.cmbSatAMPM.TabIndex = 27;
      this.cmbSatMinutes.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbSatMinutes.FormattingEnabled = true;
      this.cmbSatMinutes.Items.AddRange(new object[61]
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
        (object) "29 ",
        (object) "30 ",
        (object) "31 ",
        (object) "32 ",
        (object) "33 ",
        (object) "34 ",
        (object) "35 ",
        (object) "36 ",
        (object) "37 ",
        (object) "38 ",
        (object) "39 ",
        (object) "40 ",
        (object) "41 ",
        (object) "42 ",
        (object) "43 ",
        (object) "44 ",
        (object) "45 ",
        (object) "46 ",
        (object) "47 ",
        (object) "48 ",
        (object) "49 ",
        (object) "50 ",
        (object) "51 ",
        (object) "52 ",
        (object) "53 ",
        (object) "54 ",
        (object) "55 ",
        (object) "56 ",
        (object) "57 ",
        (object) "58 ",
        (object) "59 "
      });
      this.cmbSatMinutes.Location = new Point(252, 337);
      this.cmbSatMinutes.Name = "cmbSatMinutes";
      this.cmbSatMinutes.Size = new Size(49, 21);
      this.cmbSatMinutes.TabIndex = 26;
      this.cmbSatHours.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbSatHours.FormattingEnabled = true;
      this.cmbSatHours.Items.AddRange(new object[13]
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
      this.cmbSatHours.Location = new Point(197, 337);
      this.cmbSatHours.Name = "cmbSatHours";
      this.cmbSatHours.Size = new Size(49, 21);
      this.cmbSatHours.TabIndex = 25;
      this.label8.AutoSize = true;
      this.label8.Location = new Point(94, 341);
      this.label8.Name = "label8";
      this.label8.Size = new Size(86, 13);
      this.label8.TabIndex = 24;
      this.label8.Text = "ONRP End Time";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(94, 315);
      this.label9.Name = "label9";
      this.label9.Size = new Size(89, 13);
      this.label9.TabIndex = 23;
      this.label9.Text = "ONRP Start Time";
      this.chkEnableSatONRP.AutoSize = true;
      this.chkEnableSatONRP.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkEnableSatONRP.Location = new Point(76, 285);
      this.chkEnableSatONRP.Name = "chkEnableSatONRP";
      this.chkEnableSatONRP.Size = new Size(113, 17);
      this.chkEnableSatONRP.TabIndex = 22;
      this.chkEnableSatONRP.Text = "Saturday Hours";
      this.chkEnableSatONRP.UseVisualStyleBackColor = true;
      this.chkEnableSatONRP.CheckedChanged += new EventHandler(this.chkHours_CheckedChanged);
      this.panel3.Controls.Add((Control) this.rdoContinuousCoverage);
      this.panel3.Controls.Add((Control) this.rdoSpecifyTimes);
      this.panel3.Location = new Point(76, 110);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(200, 47);
      this.panel3.TabIndex = 21;
      this.rdoContinuousCoverage.AutoSize = true;
      this.rdoContinuousCoverage.Location = new Point(0, 4);
      this.rdoContinuousCoverage.Name = "rdoContinuousCoverage";
      this.rdoContinuousCoverage.Size = new Size(161, 17);
      this.rdoContinuousCoverage.TabIndex = 3;
      this.rdoContinuousCoverage.Text = "Continuous ONRP Coverage";
      this.rdoContinuousCoverage.UseVisualStyleBackColor = true;
      this.rdoContinuousCoverage.CheckedChanged += new EventHandler(this.rdoContinuousCoverage_CheckedChanged);
      this.rdoSpecifyTimes.AutoSize = true;
      this.rdoSpecifyTimes.Checked = true;
      this.rdoSpecifyTimes.Location = new Point(0, 27);
      this.rdoSpecifyTimes.Name = "rdoSpecifyTimes";
      this.rdoSpecifyTimes.Size = new Size(91, 17);
      this.rdoSpecifyTimes.TabIndex = 4;
      this.rdoSpecifyTimes.TabStop = true;
      this.rdoSpecifyTimes.Text = "Specify Times";
      this.rdoSpecifyTimes.UseVisualStyleBackColor = true;
      this.panel2.Controls.Add((Control) this.rdoUseChannelDefault);
      this.panel2.Controls.Add((Control) this.rdoCustomizeSettings);
      this.panel2.Location = new Point(28, 37);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(200, 40);
      this.panel2.TabIndex = 20;
      this.rdoUseChannelDefault.AutoSize = true;
      this.rdoUseChannelDefault.Checked = true;
      this.rdoUseChannelDefault.Location = new Point(0, 0);
      this.rdoUseChannelDefault.Name = "rdoUseChannelDefault";
      this.rdoUseChannelDefault.Size = new Size(128, 17);
      this.rdoUseChannelDefault.TabIndex = 18;
      this.rdoUseChannelDefault.TabStop = true;
      this.rdoUseChannelDefault.Text = "Use Channel Defaults";
      this.rdoUseChannelDefault.UseVisualStyleBackColor = true;
      this.rdoUseChannelDefault.CheckedChanged += new EventHandler(this.rdoUseChannelDefault_CheckedChanged);
      this.rdoCustomizeSettings.AutoSize = true;
      this.rdoCustomizeSettings.Location = new Point(0, 23);
      this.rdoCustomizeSettings.Name = "rdoCustomizeSettings";
      this.rdoCustomizeSettings.Size = new Size(114, 17);
      this.rdoCustomizeSettings.TabIndex = 19;
      this.rdoCustomizeSettings.Text = "Customize Settings";
      this.rdoCustomizeSettings.UseVisualStyleBackColor = true;
      this.txtONRPTolerance.Location = new Point(155, 506);
      this.txtONRPTolerance.MaxLength = 2;
      this.txtONRPTolerance.Name = "txtONRPTolerance";
      this.txtONRPTolerance.Size = new Size(145, 20);
      this.txtONRPTolerance.TabIndex = 17;
      this.txtDollarLimit.Location = new Point(155, 481);
      this.txtDollarLimit.MaxLength = 8;
      this.txtDollarLimit.Name = "txtDollarLimit";
      this.txtDollarLimit.Size = new Size(145, 20);
      this.txtDollarLimit.TabIndex = 16;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(25, 509);
      this.label6.Name = "label6";
      this.label6.Size = new Size(92, 13);
      this.label6.TabIndex = 15;
      this.label6.Text = "ONRP Tolerance ";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(25, 484);
      this.label5.Name = "label5";
      this.label5.Size = new Size(128, 13);
      this.label5.TabIndex = 14;
      this.label5.Text = "ONRP Dollar Limit          $";
      this.chkMaximumLimit.AutoSize = true;
      this.chkMaximumLimit.Location = new Point(28, 457);
      this.chkMaximumLimit.Name = "chkMaximumLimit";
      this.chkMaximumLimit.Size = new Size(111, 17);
      this.chkMaximumLimit.TabIndex = 13;
      this.chkMaximumLimit.Text = "No Maximum Limit";
      this.chkMaximumLimit.UseVisualStyleBackColor = true;
      this.chkMaximumLimit.CheckedChanged += new EventHandler(this.chkMaximumLimit_CheckedChanged);
      this.chkWeekendHolidayCoverage.AutoSize = true;
      this.chkWeekendHolidayCoverage.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkWeekendHolidayCoverage.Location = new Point(76, 249);
      this.chkWeekendHolidayCoverage.Name = "chkWeekendHolidayCoverage";
      this.chkWeekendHolidayCoverage.Size = new Size(194, 17);
      this.chkWeekendHolidayCoverage.TabIndex = 12;
      this.chkWeekendHolidayCoverage.Text = "Weekend / Holiday Coverage";
      this.chkWeekendHolidayCoverage.UseVisualStyleBackColor = true;
      this.chkWeekendHolidayCoverage.CheckedChanged += new EventHandler(this.chkWeekendHolidayCoverage_CheckedChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(362, 222);
      this.label4.Name = "label4";
      this.label4.Size = new Size(21, 13);
      this.label4.TabIndex = 11;
      this.label4.Text = "ET";
      this.txtONRPStartTime.Enabled = false;
      this.txtONRPStartTime.Location = new Point(197, 191);
      this.txtONRPStartTime.Name = "txtONRPStartTime";
      this.txtONRPStartTime.Size = new Size(145, 20);
      this.txtONRPStartTime.TabIndex = 10;
      this.cmbAMPM.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbAMPM.FormattingEnabled = true;
      this.cmbAMPM.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "AM",
        (object) "PM"
      });
      this.cmbAMPM.Location = new Point(307, 217);
      this.cmbAMPM.Name = "cmbAMPM";
      this.cmbAMPM.Size = new Size(49, 21);
      this.cmbAMPM.TabIndex = 9;
      this.cmbMinutes.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbMinutes.FormattingEnabled = true;
      this.cmbMinutes.Items.AddRange(new object[61]
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
        (object) "29 ",
        (object) "30 ",
        (object) "31 ",
        (object) "32 ",
        (object) "33 ",
        (object) "34 ",
        (object) "35 ",
        (object) "36 ",
        (object) "37 ",
        (object) "38 ",
        (object) "39 ",
        (object) "40 ",
        (object) "41 ",
        (object) "42 ",
        (object) "43 ",
        (object) "44 ",
        (object) "45 ",
        (object) "46 ",
        (object) "47 ",
        (object) "48 ",
        (object) "49 ",
        (object) "50 ",
        (object) "51 ",
        (object) "52 ",
        (object) "53 ",
        (object) "54 ",
        (object) "55 ",
        (object) "56 ",
        (object) "57 ",
        (object) "58 ",
        (object) "59 "
      });
      this.cmbMinutes.Location = new Point(252, 217);
      this.cmbMinutes.Name = "cmbMinutes";
      this.cmbMinutes.Size = new Size(49, 21);
      this.cmbMinutes.TabIndex = 8;
      this.cmbHours.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbHours.FormattingEnabled = true;
      this.cmbHours.Items.AddRange(new object[13]
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
      this.cmbHours.Location = new Point(197, 217);
      this.cmbHours.Name = "cmbHours";
      this.cmbHours.Size = new Size(49, 21);
      this.cmbHours.TabIndex = 7;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(94, 221);
      this.label3.Name = "label3";
      this.label3.Size = new Size(86, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "ONRP End Time";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(94, 195);
      this.label2.Name = "label2";
      this.label2.Size = new Size(89, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "ONRP Start Time";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(43, 90);
      this.label1.Name = "label1";
      this.label1.Size = new Size(53, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Coverage";
      this.chkEnableONRP.AutoSize = true;
      this.chkEnableONRP.Location = new Point(28, 14);
      this.chkEnableONRP.Name = "chkEnableONRP";
      this.chkEnableONRP.Size = new Size(163, 17);
      this.chkEnableONRP.TabIndex = 0;
      this.chkEnableONRP.Text = "Enable ONRP for this branch";
      this.chkEnableONRP.UseVisualStyleBackColor = true;
      this.chkEnableONRP.CheckedChanged += new EventHandler(this.chkEnableONRP_CheckedChanged);
      this.btnOK.Location = new Point(259, 568);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Location = new Point(340, 568);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(427, 595);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ONRPToleranceSetting);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Branch Settings for ONRP";
      this.Shown += new EventHandler(this.ONRPToleranceSetting_Shown);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
