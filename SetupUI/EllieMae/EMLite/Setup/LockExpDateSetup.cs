// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LockExpDateSetup
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.EPC2;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Calendar;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.OverNightRateProtection;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LockExpDateSetup : SettingsUserControl
  {
    private MonthNavigator[] months;
    private int currentYear = -1;
    private BusinessCalendar businessCalendar;
    private BusinessCalendar postalCalendar;
    private BusinessCalendar alldaysCalendar;
    private BusinessCalendar customCalendar;
    private bool suspendEvents;
    private LockDeskHourControl retailLockDeskHourCtrl = new LockDeskHourControl(LoanChannel.BankedRetail);
    private LockDeskHourControl wholesaleLockDeskHourCtrl = new LockDeskHourControl(LoanChannel.BankedWholesale);
    private LockDeskHourControl correspondentLockDeskHourCtrl = new LockDeskHourControl(LoanChannel.Correspondent);
    private LockDeskScheduleHours retailLockDeskHours;
    private LockDeskScheduleHours wholesaleLockDeskHours;
    private LockDeskScheduleHours correspondentLockDeskHours;
    private LockDeskScheduleHours retailLockDeskHoursOld;
    private LockDeskScheduleHours wholesaleLockDeskHoursOld;
    private LockDeskScheduleHours correspondentLockDeskHoursOld;
    private OnrpSettings retailONRPOld;
    private OnrpSettings wholesaleONRPOld;
    private OnrpSettings corrONRPOld;
    private bool IsCentralChannelSelected;
    private bool updateLockDeskHours;
    private IContainer components;
    private TabControl tabCalendar;
    private TabPage tpExpiration;
    private TabPage tpCalendarHour;
    private ComboBox cmbTime;
    private ComboBox cmbTimezone;
    private TextBox txtMinute;
    private Label label6;
    private TextBox txtHour;
    private Label label5;
    private Label label4;
    private Label label7;
    private Label label1;
    private RadioButton rbtLockExpCalBusiness;
    private RadioButton rbtLockExpCalNone;
    private RadioButton rbtLockExpCalPostal;
    private CheckBox chkCalendarOnLockExtension;
    private GroupContainer groupContainer1;
    private RadioButton rbtLockExpCalCustom;
    private GroupContainer grpCalendar;
    private Panel panel1;
    private MonthNavigator monthNov;
    private MonthNavigator monthDec;
    private MonthNavigator monthSep;
    private MonthNavigator monthOct;
    private MonthNavigator monthJul;
    private MonthNavigator monthAug;
    private MonthNavigator monthMay;
    private MonthNavigator monthJun;
    private MonthNavigator monthMar;
    private MonthNavigator monthApr;
    private MonthNavigator monthJan;
    private MonthNavigator monthFeb;
    private GradientPanel gradientPanel1;
    private CheckBox chkSundays;
    private CheckBox chkSaturdays;
    private Label lblHeader;
    private Label lblYear;
    private StandardIconButton btnPreviousYear;
    private StandardIconButton btnNextYear;
    private Panel panel2;
    private BorderPanel borderPanel1;
    private Panel panel3;
    private Label label3;
    private RadioButton rbtNextDay;
    private RadioButton rbtPreDay;
    private Panel panel4;
    private Label label2;
    private RadioButton rbtDayAfter;
    private RadioButton rbtTheDate;
    private TabPage tpLockDeskONRP;
    private Panel panel5;
    private GroupContainer gcLockDeskSchedule;
    private CheckBox chkEnableEncompassLockDesk;
    private GroupContainer gcMessage;
    private Button btnViewMsg;
    private TextBox txtAddendumMessage;
    private Label label29;
    private TextBox txtStandardMessage;
    private Label label28;
    private Panel panel6;
    private Label lblONRP;
    private Label label8;
    private Label label13;
    private Label label9;
    private Label label14;
    private TabControl tabLockDeskHours;
    private TabPage tabRetailLockDesk;
    private TabPage tabWholesaleLockDesk;
    private TabPage tabCorrespondentLockDesk;
    private Label label12;
    private Label label10;
    private Panel correspondentPanel;
    private Panel wholesalePanel;
    private Panel retailPanel;
    private Panel panel7;
    private RadioButton rdoChannelLockDeskHours;
    private RadioButton rdoCentralLockDeskHours;
    private GroupContainer groupContainer3;
    private GroupContainer groupContainer2;
    private RadioButton rBtnCommitmentTermFields;
    private RadioButton rBtnLockTermFields;
    private IconButton iconBtnHelpCommitmentTerm;
    private Label label11;
    private Panel panel8;
    private Label label16;
    private Label label15;

    public LockExpDateSetup(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      IDictionary serverSettings = Session.ServerManager.GetServerSettings("Policies");
      this.initTabExpiration(serverSettings);
      this.initCalendar();
      this.initTabCalendar(serverSettings);
      this.retailLockDeskHourCtrl.Dock = DockStyle.Fill;
      this.wholesaleLockDeskHourCtrl.Dock = DockStyle.Fill;
      this.correspondentLockDeskHourCtrl.Dock = DockStyle.Fill;
      this.retailPanel.Dock = DockStyle.Fill;
      this.wholesalePanel.Dock = DockStyle.Fill;
      this.correspondentPanel.Dock = DockStyle.Fill;
      this.retailPanel.Controls.Add((Control) this.retailLockDeskHourCtrl);
      this.wholesalePanel.Controls.Add((Control) this.wholesaleLockDeskHourCtrl);
      this.correspondentPanel.Controls.Add((Control) this.correspondentLockDeskHourCtrl);
      this.retailLockDeskHourCtrl.OnrpSettings = new LockDeskGlobalSettings(serverSettings, LoanChannel.BankedRetail);
      this.wholesaleLockDeskHourCtrl.OnrpSettings = new LockDeskGlobalSettings(serverSettings, LoanChannel.BankedWholesale);
      this.correspondentLockDeskHourCtrl.OnrpSettings = new LockDeskGlobalSettings(serverSettings, LoanChannel.Correspondent);
      this.retailONRPOld = new OnrpSettings(serverSettings, LoanChannel.BankedRetail);
      this.wholesaleONRPOld = new OnrpSettings(serverSettings, LoanChannel.BankedWholesale);
      this.corrONRPOld = new OnrpSettings(serverSettings, LoanChannel.Correspondent);
      if (Session.EncompassEdition == EncompassEdition.Broker)
        this.tabCalendar.TabPages.Remove(this.tpLockDeskONRP);
      this.EnableDisbleAllSettings((bool) serverSettings[(object) "Policies.EnableLockDeskSCHEDULE"]);
      this.chkEnableEncompassLockDesk.CheckedChanged += new EventHandler(this.chkEnableEncompassLockDesk_CheckedChanged);
      this.InitLockDeskHours(serverSettings);
      this.tabLockDeskHours_SelectedIndexChanged((object) this.tabLockDeskHours, (EventArgs) null);
      this.setDirtyFlag(false);
    }

    public override void Reset()
    {
      IDictionary serverSettings = Session.ServerManager.GetServerSettings("Policies");
      this.retailLockDeskHourCtrl.OnrpSettings = new LockDeskGlobalSettings(serverSettings, LoanChannel.BankedRetail);
      this.wholesaleLockDeskHourCtrl.OnrpSettings = new LockDeskGlobalSettings(serverSettings, LoanChannel.BankedWholesale);
      this.correspondentLockDeskHourCtrl.OnrpSettings = new LockDeskGlobalSettings(serverSettings, LoanChannel.Correspondent);
      this.retailONRPOld = new OnrpSettings(serverSettings, LoanChannel.BankedRetail);
      this.wholesaleONRPOld = new OnrpSettings(serverSettings, LoanChannel.BankedWholesale);
      this.corrONRPOld = new OnrpSettings(serverSettings, LoanChannel.Correspondent);
      this.initTabExpiration(serverSettings);
      this.initTabCalendar(serverSettings);
      this.EnableDisbleAllSettings((bool) serverSettings[(object) "Policies.EnableLockDeskSCHEDULE"]);
      this.InitLockDeskHours(serverSettings);
      if ((bool) serverSettings[(object) "Policies.EnableCommitmentTermFields"])
        this.rBtnCommitmentTermFields.Checked = true;
      else
        this.rBtnLockTermFields.Checked = true;
      this.setDirtyFlag(false);
    }

    public override void Save()
    {
      using (CursorActivator.Wait())
      {
        if (this.txtHour.Text.Trim() == "" || this.txtMinute.Text.Trim() == "" || this.cmbTime.SelectedItem.ToString().Trim() == "")
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Expiration time is required.");
          return;
        }
        if (this.cmbTimezone.SelectedItem.ToString().Trim() == "")
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Expiration timezone is required.");
          return;
        }
        if (!this.IsWithin(Convert.ToInt32(this.txtHour.Text), 1, 12))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Expiration hour needs to be between 1 and 12.");
          return;
        }
        if (!this.IsWithin(Convert.ToInt32(this.txtMinute.Text), 0, 59))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Expiration minute needs to be between 0 and 59.");
          return;
        }
        if (this.rbtTheDate.Checked)
          Session.ServerManager.UpdateServerSetting("Policies.LockExpDayCount", (object) LockExpDayCountSetting.OnTheDay);
        else
          Session.ServerManager.UpdateServerSetting("Policies.LockExpDayCount", (object) LockExpDayCountSetting.OneDayAfter);
        Session.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.LockExpDayCount"] = Session.ServerManager.GetServerSetting("Policies.LockExpDayCount");
        Session.ServerManager.UpdateServerSetting("Policies.RateLockExpirationTime", (object) (this.txtHour.Text + ":" + this.txtMinute.Text + " " + this.cmbTime.SelectedItem.ToString()));
        Session.ServerManager.UpdateServerSetting("Policies.RateLockExpirationTimeZone", (object) this.cmbTimezone.SelectedItem.ToString());
        if (this.rbtLockExpCalNone.Checked)
          Session.ServerManager.UpdateServerSetting("Policies.LockExpCalendar", (object) LockExpCalendarSetting.None);
        else if (this.rbtLockExpCalPostal.Checked)
          Session.ServerManager.UpdateServerSetting("Policies.LockExpCalendar", (object) LockExpCalendarSetting.PostalCalendar);
        else if (this.rbtLockExpCalBusiness.Checked)
          Session.ServerManager.UpdateServerSetting("Policies.LockExpCalendar", (object) LockExpCalendarSetting.BusinessCalendar);
        else if (this.rbtLockExpCalCustom.Checked)
          Session.ServerManager.UpdateServerSetting("Policies.LockExpCalendar", (object) LockExpCalendarSetting.CustomCalendar);
        Session.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.LockExpCalendar"] = Session.ServerManager.GetServerSetting("Policies.LockExpCalendar");
        Session.ServerManager.UpdateServerSetting("Policies.LockExtensionCalendarOpt", (object) this.chkCalendarOnLockExtension.Checked);
        Session.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.LockExtensionCalendarOpt"] = Session.ServerManager.GetServerSetting("Policies.LockExtensionCalendarOpt");
        if (this.rbtPreDay.Checked)
          Session.ServerManager.UpdateServerSetting("Policies.LockExpExclude", (object) LockExpDayExcludeSetting.PreviousBusinessDay);
        else if (this.rbtNextDay.Checked)
          Session.ServerManager.UpdateServerSetting("Policies.LockExpExclude", (object) LockExpDayExcludeSetting.NextBusinessDay);
        Session.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.LockExpExclude"] = Session.ServerManager.GetServerSetting("Policies.LockExpExclude");
        Session.ConfigurationManager.SaveBusinessCalendar(this.customCalendar);
        Session.ServerManager.UpdateServerSetting("Policies.EnableLockDeskSCHEDULE", (object) this.chkEnableEncompassLockDesk.Checked.ToString());
        if (this.rBtnCommitmentTermFields.Checked)
        {
          Session.ServerManager.UpdateServerSetting("Policies.EnableCommitmentTermFields", (object) this.rBtnCommitmentTermFields.Checked.ToString());
          Session.StartupInfo.PolicySettings[(object) "Policies.EnableCommitmentTermFields"] = (object) this.rBtnCommitmentTermFields.Checked.ToString();
        }
        else
        {
          Session.ServerManager.UpdateServerSetting("Policies.EnableCommitmentTermFields", (object) this.rBtnCommitmentTermFields.Checked.ToString());
          Session.StartupInfo.PolicySettings[(object) "Policies.EnableCommitmentTermFields"] = (object) this.rBtnCommitmentTermFields.Checked.ToString();
        }
        if (this.chkEnableEncompassLockDesk.Checked)
        {
          this.CopyToOtherChannels(this.GetChannelFromTab(), this.rdoCentralLockDeskHours.Checked);
          if (!this.retailLockDeskHourCtrl.ValidateSettings() || !this.wholesaleLockDeskHourCtrl.ValidateSettings() || !this.correspondentLockDeskHourCtrl.ValidateSettings())
            return;
          if (this.txtAddendumMessage.Text.Trim().Length == 0)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Message Addendum may not be blank. Enter custom Message Addendum prior to saving these settings.");
            return;
          }
          if (this.updateLockDeskHours && this.SaveLockDeskSettings())
          {
            bool flag1 = this.retailLockDeskHours.NeedClearAccruedAmount(this.retailLockDeskHoursOld);
            bool flag2 = this.wholesaleLockDeskHours.NeedClearAccruedAmount(this.wholesaleLockDeskHoursOld);
            bool flag3 = this.correspondentLockDeskHours.NeedClearAccruedAmount(this.correspondentLockDeskHoursOld);
            if (this.IsCentralChannelSelected)
            {
              if (flag1)
                Session.SessionObjects.OverNightRateProtectionManager.DeleteOnrpPeriodAccruedAmount(LoanChannel.None, (string) null, false);
            }
            else
            {
              if (flag1)
                Session.SessionObjects.OverNightRateProtectionManager.DeleteOnrpPeriodAccruedAmount(LoanChannel.BankedRetail, (string) null, false);
              if (flag2)
                Session.SessionObjects.OverNightRateProtectionManager.DeleteOnrpPeriodAccruedAmount(LoanChannel.BankedWholesale, (string) null, false);
              if (flag3)
                Session.SessionObjects.OverNightRateProtectionManager.DeleteOnrpPeriodAccruedAmount(LoanChannel.Correspondent, (string) null, false);
            }
            bool flag4 = this.retailONRPOld.NeedClearAccruedAmount(this.retailLockDeskHourCtrl.OnrpSettings);
            this.SaveOnrpSettings(this.retailLockDeskHourCtrl.OnrpSettings);
            if (flag4)
              Session.SessionObjects.OverNightRateProtectionManager.DeleteOnrpPeriodAccruedAmount(LoanChannel.BankedRetail, (string) null, true);
            bool flag5 = this.wholesaleONRPOld.NeedClearAccruedAmount(this.wholesaleLockDeskHourCtrl.OnrpSettings);
            this.SaveOnrpSettings(this.wholesaleLockDeskHourCtrl.OnrpSettings);
            if (flag5)
              Session.SessionObjects.OverNightRateProtectionManager.DeleteOnrpPeriodAccruedAmount(LoanChannel.BankedWholesale, (string) null, true);
            bool flag6 = this.corrONRPOld.NeedClearAccruedAmount(this.correspondentLockDeskHourCtrl.OnrpSettings);
            this.SaveOnrpSettings(this.correspondentLockDeskHourCtrl.OnrpSettings);
            if (flag6)
              Session.SessionObjects.OverNightRateProtectionManager.DeleteOnrpPeriodAccruedAmount(LoanChannel.Correspondent, (string) null, true);
            Session.ServerManager.UpdateServerSetting("Policies.ONRPOverLimitMsgAddendum", (object) this.txtAddendumMessage.Text);
          }
          this.Reset();
        }
      }
      this.setDirtyFlag(false);
    }

    private void initTabExpiration(IDictionary settings)
    {
      if ((LockExpDayCountSetting) settings[(object) "Policies.LockExpDayCount"] == LockExpDayCountSetting.OnTheDay)
        this.rbtTheDate.Checked = true;
      else
        this.rbtDayAfter.Checked = true;
      this.rbtTheDate.CheckedChanged += new EventHandler(this.rbtOption_CheckedChanged);
      string setting1 = (string) settings[(object) "Policies.RateLockExpirationTime"];
      if (setting1 != null)
      {
        string[] strArray1 = setting1.Split(' ');
        string[] strArray2 = strArray1[0].Split(':');
        this.txtHour.Text = strArray2[0];
        this.txtMinute.Text = strArray2[1];
        this.cmbTime.Text = strArray1[1];
      }
      else
      {
        this.txtHour.Text = "5";
        this.txtMinute.Text = "00";
        this.cmbTime.Text = "PM";
      }
      string setting2 = (string) settings[(object) "Policies.RateLockExpirationTimeZone"];
      if (setting2 != null)
        this.cmbTimezone.Text = setting2;
      else
        this.cmbTimezone.Text = "(UTC -05:00) Eastern Time";
      this.chkCalendarOnLockExtension.Checked = (bool) settings[(object) "Policies.LockExtensionCalendarOpt"];
      this.rBtnLockTermFields.CheckedChanged -= new EventHandler(this.rBtnLockTermFields_CheckedChanged);
      this.rBtnCommitmentTermFields.CheckedChanged -= new EventHandler(this.rBtnCommitmentTermFields_CheckedChanged);
      if ((bool) settings[(object) "Policies.EnableCommitmentTermFields"])
        this.rBtnCommitmentTermFields.Checked = true;
      else
        this.rBtnLockTermFields.Checked = true;
      this.rBtnLockTermFields.CheckedChanged += new EventHandler(this.rBtnLockTermFields_CheckedChanged);
      this.rBtnCommitmentTermFields.CheckedChanged += new EventHandler(this.rBtnCommitmentTermFields_CheckedChanged);
      if ((LockExpDayExcludeSetting) settings[(object) "Policies.LockExpExclude"] == LockExpDayExcludeSetting.PreviousBusinessDay)
        this.rbtPreDay.Checked = true;
      else
        this.rbtNextDay.Checked = true;
      this.rbtPreDay.CheckedChanged += new EventHandler(this.rbtOption_CheckedChanged);
      this.rbtNextDay.CheckedChanged += new EventHandler(this.rbtOption_CheckedChanged);
    }

    private void rbtOption_CheckedChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void rbtLockExpCal_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void chkCalendarOnLockExtension_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void textField_Changed(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void txtHour_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
      {
        this.setDirtyFlag(true);
      }
      else
      {
        if (!char.IsNumber(e.KeyChar))
          e.Handled = true;
        if (this.txtHour.Text.Length < 2)
          return;
        e.Handled = true;
      }
    }

    private void txtMinute_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
      {
        this.setDirtyFlag(true);
      }
      else
      {
        if (!char.IsNumber(e.KeyChar))
          e.Handled = true;
        if (this.txtMinute.Text.Length < 2)
          return;
        e.Handled = true;
      }
    }

    private void txtMinute_Leave(object sender, EventArgs e)
    {
      if (this.txtMinute.Text.Length != 1)
        return;
      this.txtMinute.Text = "0" + this.txtMinute.Text;
    }

    private bool IsWithin(int value, int minimum, int maximum)
    {
      return value >= minimum && value <= maximum;
    }

    private void initTabCalendar(IDictionary settings)
    {
      if (this.currentYear == -1)
        this.currentYear = DateTime.Today.Year;
      this.businessCalendar = Session.ConfigurationManager.GetBusinessCalendar(CalendarType.Business, new DateTime(this.currentYear, 1, 1), new DateTime(this.currentYear, 12, 31));
      this.postalCalendar = Session.ConfigurationManager.GetBusinessCalendar(CalendarType.Postal, new DateTime(this.currentYear, 1, 1), new DateTime(this.currentYear, 12, 31));
      this.alldaysCalendar = Session.ConfigurationManager.GetFullCalendar(CalendarType.AllDays, new DateTime(this.currentYear, 1, 1), new DateTime(this.currentYear, 12, 31));
      this.customCalendar = Session.ConfigurationManager.GetBusinessCalendar(CalendarType.Custom, new DateTime(this.currentYear, 1, 1), new DateTime(this.currentYear, 12, 31));
      if (this.customCalendar == null)
        this.customCalendar = Session.ConfigurationManager.GetBusinessCalendar(CalendarType.Postal, new DateTime(this.currentYear, 1, 1), new DateTime(this.currentYear, 12, 31));
      switch ((LockExpCalendarSetting) settings[(object) "Policies.LockExpCalendar"])
      {
        case LockExpCalendarSetting.None:
          this.rbtLockExpCalNone.Checked = true;
          break;
        case LockExpCalendarSetting.PostalCalendar:
          this.rbtLockExpCalPostal.Checked = true;
          break;
        case LockExpCalendarSetting.BusinessCalendar:
          this.rbtLockExpCalBusiness.Checked = true;
          break;
        case LockExpCalendarSetting.CustomCalendar:
          this.rbtLockExpCalCustom.Checked = true;
          break;
        default:
          this.rbtLockExpCalPostal.Checked = true;
          break;
      }
      this.loadCalendar();
    }

    private void initCalendar()
    {
      this.months = new MonthNavigator[12]
      {
        this.monthJan,
        this.monthFeb,
        this.monthMar,
        this.monthApr,
        this.monthMay,
        this.monthJun,
        this.monthJul,
        this.monthAug,
        this.monthSep,
        this.monthOct,
        this.monthNov,
        this.monthDec
      };
      for (int index = 0; index < this.months.Length; ++index)
        this.months[index].DateClick += new DateEventHandler(this.onDateClicked);
    }

    private void loadCalendar()
    {
      this.suspendEvents = true;
      this.setCurrentYear(this.currentYear);
      this.chkSaturdays.Checked = (this.CurrentCalendar.WorkDays & DaysOfWeek.Saturday) == DaysOfWeek.None;
      this.chkSundays.Checked = (this.CurrentCalendar.WorkDays & DaysOfWeek.Sunday) == DaysOfWeek.None;
      this.chkSaturdays.Visible = this.rbtLockExpCalCustom.Checked;
      this.chkSundays.Visible = this.rbtLockExpCalCustom.Checked;
      if (this.rbtLockExpCalPostal.Checked)
        this.lblHeader.Text = "The U.S. Postal Calendar excludes Sundays and legal holidays. View days that are excluded from your lock expiration calendar.";
      else if (this.rbtLockExpCalBusiness.Checked)
        this.lblHeader.Text = "View days that are excluded from your lock expiration calendar.";
      else if (this.rbtLockExpCalNone.Checked)
        this.lblHeader.Text = "View days that are excluded from your lock expiration calendar.";
      else
        this.lblHeader.Text = "Click a day to exclude it from your lock expiration calendar.";
      this.suspendEvents = false;
    }

    private BusinessCalendar CurrentCalendar
    {
      get
      {
        if (this.rbtLockExpCalPostal.Checked)
          return this.postalCalendar;
        if (this.rbtLockExpCalBusiness.Checked)
          return this.businessCalendar;
        return this.rbtLockExpCalNone.Checked ? this.alldaysCalendar : this.customCalendar;
      }
    }

    private CalendarType CurrentCalendarType
    {
      get
      {
        if (this.rbtLockExpCalPostal.Checked)
          return CalendarType.Postal;
        if (this.rbtLockExpCalBusiness.Checked)
          return CalendarType.Business;
        return this.rbtLockExpCalNone.Checked ? CalendarType.AllDays : CalendarType.Custom;
      }
    }

    private void onDateClicked(object sender, DateEventArgs e)
    {
      if (this.rbtLockExpCalBusiness.Checked || this.rbtLockExpCalNone.Checked || this.rbtLockExpCalPostal.Checked)
        return;
      switch (this.customCalendar.GetDayType(e.Date))
      {
        case CalendarDayType.BusinessDay:
          this.customCalendar.SetDayType(e.Date, CalendarDayType.Holiday);
          this.months[e.Date.Month - 1].SetDateEffects(e.Date, DateEffects.Highlight);
          this.setDirtyFlag(true);
          break;
        case CalendarDayType.Holiday:
          this.customCalendar.SetDayType(e.Date, CalendarDayType.BusinessDay);
          this.months[e.Date.Month - 1].ClearDateEffects(e.Date);
          this.setDirtyFlag(true);
          break;
      }
    }

    private void setCurrentYear(int year)
    {
      this.ensureYearLoaded(year);
      for (int index = 0; index < this.months.Length; ++index)
      {
        this.months[index].MonthAndYear = new MonthYear(year, index + 1);
        this.months[index].ClearAllEffects();
      }
      StandardIconButton btnPreviousYear = this.btnPreviousYear;
      int num1 = year;
      DateTime dateTime = BusinessCalendar.MinimumDate;
      int year1 = dateTime.Year;
      int num2 = num1 > year1 ? 1 : 0;
      btnPreviousYear.Visible = num2 != 0;
      StandardIconButton btnNextYear = this.btnNextYear;
      int num3 = year;
      dateTime = BusinessCalendar.MaximumDate;
      int year2 = dateTime.Year;
      int num4 = num3 < year2 ? 1 : 0;
      btnNextYear.Visible = num4 != 0;
      for (DateTime date = new DateTime(year, 1, 1); date.Year == year; date = date.AddDays(1.0))
      {
        if (this.CurrentCalendar.GetDayType(date) != CalendarDayType.BusinessDay)
          this.months[date.Month - 1].SetDateEffects(date, DateEffects.Highlight);
      }
      this.lblYear.Text = year.ToString("0000");
      this.currentYear = year;
    }

    private void btnNextYear_Click(object sender, EventArgs e)
    {
      this.setCurrentYear(this.currentYear + 1);
    }

    private void btnPreviousYear_Click(object sender, EventArgs e)
    {
      this.setCurrentYear(this.currentYear - 1);
    }

    private void chkSaturdays_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents)
        return;
      if (this.chkSaturdays.Checked)
        this.CurrentCalendar.WorkDays &= ~DaysOfWeek.Saturday;
      else
        this.CurrentCalendar.WorkDays |= DaysOfWeek.Saturday;
      this.setDirtyFlag(true);
      this.setCurrentYear(this.currentYear);
    }

    private void chkSundays_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents)
        return;
      if (this.chkSundays.Checked)
        this.CurrentCalendar.WorkDays &= ~DaysOfWeek.Sunday;
      else
        this.CurrentCalendar.WorkDays |= DaysOfWeek.Sunday;
      this.setDirtyFlag(true);
      this.setCurrentYear(this.currentYear);
    }

    private void ensureYearLoaded(int year)
    {
      DateTime dateTime = new DateTime(year, 1, 1);
      if (this.CurrentCalendar.Contains(dateTime))
        return;
      this.CurrentCalendar.Merge(!this.rbtLockExpCalNone.Checked ? (!(dateTime < this.CurrentCalendar.StartDate) ? Session.ConfigurationManager.GetBusinessCalendar(this.CurrentCalendarType, this.CurrentCalendar.EndDate.AddDays(1.0), new DateTime(year, 12, 31)) : Session.ConfigurationManager.GetBusinessCalendar(this.CurrentCalendarType, dateTime, this.CurrentCalendar.StartDate.AddDays(-1.0))) : (!(dateTime < this.CurrentCalendar.StartDate) ? Session.ConfigurationManager.GetFullCalendar(this.CurrentCalendarType, this.CurrentCalendar.EndDate.AddDays(1.0), new DateTime(year, 12, 31)) : Session.ConfigurationManager.GetFullCalendar(this.CurrentCalendarType, dateTime, this.CurrentCalendar.StartDate.AddDays(-1.0))));
    }

    private void rbtn_CheckedChanged(object sender, EventArgs e)
    {
      this.loadCalendar();
      this.setDirtyFlag(true);
    }

    private void cmbTimezone_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void cmbTime_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void chkEnableEncompassLockDesk_CheckedChanged(object sender, EventArgs e)
    {
      bool enabled;
      if (this.chkEnableEncompassLockDesk.Checked)
      {
        if ((LockUtils.IfShipDark(Session.SessionObjects, "EPPS_EPC2_SHIP_DARK_SR") || Session.StartupInfo.ProductPricingPartner == null || !Session.StartupInfo.ProductPricingPartner.IsEPPS || Session.StartupInfo.ProductPricingPartner.VendorPlatform != VendorPlatform.EPC2 ? Utils.Dialog((IWin32Window) this, "By enabling Encompass Lock Desk Schedule you are transferring control of Lock Desk Schedule from ICE PPE to Encompass. Do you wish to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) : Utils.Dialog((IWin32Window) this, "By enabling the Encompass Lock Desk Schedule you are transferring control of Lock desk Schedule for Wholesale and Correspondent from ICE PPE to Encompass. \n\nWarning! Updating the Channel Lock Desk Hours for this product and pricing provider only allows the Wholesale and Correspondent channels to be configured. Do you wish to Continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation)) == DialogResult.OK)
        {
          enabled = true;
        }
        else
        {
          this.chkEnableEncompassLockDesk.Checked = false;
          enabled = false;
        }
      }
      else
        enabled = false;
      this.retailLockDeskHourCtrl.SetLockDeskSchedule((LockUtils.IfShipDark(Session.SessionObjects, "EPPS_EPC2_SHIP_DARK_SR") || Session.StartupInfo.ProductPricingPartner == null || !Session.StartupInfo.ProductPricingPartner.IsEPPS || Session.StartupInfo.ProductPricingPartner.VendorPlatform != VendorPlatform.EPC2) && enabled);
      this.wholesaleLockDeskHourCtrl.SetLockDeskSchedule(enabled);
      this.correspondentLockDeskHourCtrl.SetLockDeskSchedule(enabled);
      this.txtAddendumMessage.Enabled = enabled && (this.retailLockDeskHourCtrl.OnrpEnabled || this.wholesaleLockDeskHourCtrl.OnrpEnabled || this.correspondentLockDeskHourCtrl.OnrpEnabled);
      this.btnViewMsg.Enabled = enabled && (this.retailLockDeskHourCtrl.OnrpEnabled || this.wholesaleLockDeskHourCtrl.OnrpEnabled || this.correspondentLockDeskHourCtrl.OnrpEnabled);
      this.rdoCentralLockDeskHours.Enabled = this.rdoChannelLockDeskHours.Enabled = enabled;
      if (enabled)
        this.InitLockDeskHours(Session.ServerManager.GetServerSettings("Policies"));
      this.setDirtyFlag(true);
    }

    private void chkShutDownLockDesk_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void setDirty(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void cmb_SelectedValueChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void EnableOnrpChanged(object sender, EventArgs e)
    {
      this.txtAddendumMessage.Enabled = this.retailLockDeskHourCtrl.OnrpEnabled || this.wholesaleLockDeskHourCtrl.OnrpEnabled || this.correspondentLockDeskHourCtrl.OnrpEnabled;
      this.btnViewMsg.Enabled = this.retailLockDeskHourCtrl.OnrpEnabled || this.wholesaleLockDeskHourCtrl.OnrpEnabled || this.correspondentLockDeskHourCtrl.OnrpEnabled;
    }

    private void EnableDisbleAllSettings(bool enableLockDesk)
    {
      this.chkEnableEncompassLockDesk.Checked = enableLockDesk;
      this.rdoCentralLockDeskHours.Enabled = this.rdoChannelLockDeskHours.Enabled = enableLockDesk;
      this.retailLockDeskHourCtrl.SetLockDeskSchedule((LockUtils.IfShipDark(Session.SessionObjects, "EPPS_EPC2_SHIP_DARK_SR") || Session.StartupInfo.ProductPricingPartner == null || !Session.StartupInfo.ProductPricingPartner.IsEPPS || Session.StartupInfo.ProductPricingPartner.VendorPlatform != VendorPlatform.EPC2) && enableLockDesk);
      this.wholesaleLockDeskHourCtrl.SetLockDeskSchedule(enableLockDesk);
      this.correspondentLockDeskHourCtrl.SetLockDeskSchedule(enableLockDesk);
    }

    private void gcONRP_Resize(object sender, EventArgs e)
    {
    }

    private void panel5_SizeChanged(object sender, EventArgs e)
    {
    }

    private bool ValidLDTimeChange(
      string ldStrTime,
      string ldEndTime,
      string ONRPEndTime,
      bool nextDay)
    {
      DateTime result1 = DateTime.MinValue;
      DateTime result2 = DateTime.MinValue;
      DateTime result3 = DateTime.MinValue;
      Convert.ToDateTime("12:00 pm");
      if (!DateTime.TryParse(ldStrTime, out result1) || !DateTime.TryParse(ONRPEndTime, out result3) || !DateTime.TryParse(ldEndTime, out result2))
        return false;
      if (!OnrpSetupUtils.LockDeskHourHasOverlap(result1, result2, result2, result3))
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "ONRP End Time may not overlap Lock Desk Start Time. Please correct entry.");
      return false;
    }

    private bool ONRPTimeChangeMessage(
      string LDStrTime,
      string LDEndTime,
      string ONRPEndTime,
      ONRPChannel channel,
      bool nextDay)
    {
      IDictionary serverSettings = Session.ServerManager.GetServerSettings("Policies");
      string str1 = (string) serverSettings[(object) "Policies.LockDeskStrTime"];
      string str2 = (string) serverSettings[(object) "Policies.LockDeskEndTime"];
      string str3 = "";
      switch (channel)
      {
        case ONRPChannel.Retail:
          str3 = (string) serverSettings[(object) "Policies.ONRPRetEndTime"];
          break;
        case ONRPChannel.Wholesale:
          str3 = (string) serverSettings[(object) "Policies.ONRPBrokerEndTime"];
          break;
        case ONRPChannel.Correspondent:
          str3 = (string) serverSettings[(object) "Policies.ONRPCorEndTime"];
          break;
      }
      return this.ValidLDTimeChange(LDStrTime, LDEndTime, ONRPEndTime, nextDay);
    }

    private void btnViewMsg_Click(object sender, EventArgs e)
    {
      int num = (int) Utils.Dialog((IWin32Window) this, this.txtStandardMessage.Text + "\n" + this.txtAddendumMessage.Text);
    }

    private void setdirty(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void LockExpDateSetup_Scroll(object sender, ScrollEventArgs e)
    {
    }

    private void InitLockDeskHours(IDictionary settings)
    {
      this.retailLockDeskHours = new LockDeskScheduleHours();
      this.wholesaleLockDeskHours = new LockDeskScheduleHours();
      this.correspondentLockDeskHours = new LockDeskScheduleHours();
      this.retailLockDeskHourCtrl.CentralChannel_Changed -= new LockDeskHourControl.CentralChannelSelected(this.CentralChannelSelection);
      this.wholesaleLockDeskHourCtrl.CentralChannel_Changed -= new LockDeskHourControl.CentralChannelSelected(this.CentralChannelSelection);
      this.correspondentLockDeskHourCtrl.CentralChannel_Changed -= new LockDeskHourControl.CentralChannelSelected(this.CentralChannelSelection);
      this.rdoCentralLockDeskHours.CheckedChanged -= new EventHandler(this.rdoCentralLockDeskHours_CheckedChanged);
      bool boolean = Convert.ToBoolean(settings[(object) "Policies.ENABLEALLCHANNEL"].ToString());
      this.IsCentralChannelSelected = boolean;
      if (!LockUtils.IfShipDark(Session.SessionObjects, "EPPS_EPC2_SHIP_DARK_SR") && Session.StartupInfo.ProductPricingPartner != null && Session.StartupInfo.ProductPricingPartner.IsEPPS && Session.StartupInfo.ProductPricingPartner.VendorPlatform == VendorPlatform.EPC2)
        this.rdoCentralLockDeskHours.Enabled = false;
      this.rdoCentralLockDeskHours.Checked = boolean;
      this.rdoChannelLockDeskHours.Checked = !boolean;
      this.retailLockDeskHours.CentralChannel = boolean.ToString();
      this.retailLockDeskHours.LockDeskHoursMessage = settings[(object) "Policies.RETLDHRMSG"].ToString();
      this.retailLockDeskHours.LockDeskShutDownMessage = settings[(object) "Policies.RETLDSHUTDOWNMSG"].ToString();
      this.retailLockDeskHours.SaturdayEnd = settings[(object) "Policies.RETLDSATENDTIME"].ToString();
      this.retailLockDeskHours.SaturdayHoursEnabled = settings[(object) "Policies.ENABLELDRETSAT"].ToString();
      this.retailLockDeskHours.SaturdayStart = settings[(object) "Policies.RETLDSATSTRTIME"].ToString();
      this.retailLockDeskHours.ShutDownLockDesk = settings[(object) "Policies.RETLDSHUTDOWN"].ToString();
      this.retailLockDeskHours.AllowActiveRelockRequests = settings[(object) "Policies.RETLDALLOWACTIVERELOCK"].ToString();
      this.retailLockDeskHours.SundayEnd = settings[(object) "Policies.RETLDSUNENDTIME"].ToString();
      this.retailLockDeskHours.SundayHoursEnabled = settings[(object) "Policies.ENABLELDRETSUN"].ToString();
      this.retailLockDeskHours.SundayStart = settings[(object) "Policies.RETLDSUNSTRTIME"].ToString();
      this.retailLockDeskHours.WeekdayEnd = settings[(object) "Policies.RETLDENDTIME"].ToString();
      this.retailLockDeskHours.WeekdayStart = settings[(object) "Policies.RETLDSTRTIME"].ToString();
      this.retailLockDeskHoursOld = this.retailLockDeskHours;
      this.wholesaleLockDeskHours.CentralChannel = boolean.ToString();
      this.wholesaleLockDeskHours.LockDeskHoursMessage = settings[(object) "Policies.BROKERLDHRMSG"].ToString();
      this.wholesaleLockDeskHours.LockDeskShutDownMessage = settings[(object) "Policies.BROKERLDSHUTDOWNMSG"].ToString();
      this.wholesaleLockDeskHours.SaturdayEnd = settings[(object) "Policies.BROKERLDSATENDTIME"].ToString();
      this.wholesaleLockDeskHours.SaturdayHoursEnabled = settings[(object) "Policies.ENABLELDBROKERSAT"].ToString();
      this.wholesaleLockDeskHours.SaturdayStart = settings[(object) "Policies.BROKERLDSATSTRTIME"].ToString();
      this.wholesaleLockDeskHours.ShutDownLockDesk = settings[(object) "Policies.BROKERLDSHUTDOWN"].ToString();
      this.wholesaleLockDeskHours.AllowActiveRelockRequests = settings[(object) "Policies.BROKERLDALLOWACTIVERELOCK"].ToString();
      this.wholesaleLockDeskHours.SundayEnd = settings[(object) "Policies.BROKERLDSUNENDTIME"].ToString();
      this.wholesaleLockDeskHours.SundayHoursEnabled = settings[(object) "Policies.ENABLELDBROKERSUN"].ToString();
      this.wholesaleLockDeskHours.SundayStart = settings[(object) "Policies.BROKERLDSUNSTRTIME"].ToString();
      this.wholesaleLockDeskHours.WeekdayEnd = settings[(object) "Policies.BROKERLDENDTIME"].ToString();
      this.wholesaleLockDeskHours.WeekdayStart = settings[(object) "Policies.BROKERLDSTRTIME"].ToString();
      this.wholesaleLockDeskHoursOld = this.wholesaleLockDeskHours;
      this.correspondentLockDeskHours.CentralChannel = boolean.ToString();
      this.correspondentLockDeskHours.LockDeskHoursMessage = settings[(object) "Policies.CORLDHRMSG"].ToString();
      this.correspondentLockDeskHours.LockDeskShutDownMessage = settings[(object) "Policies.CORLDSHUTDOWNMSG"].ToString();
      this.correspondentLockDeskHours.SaturdayEnd = settings[(object) "Policies.CORLDSATENDTIME"].ToString();
      this.correspondentLockDeskHours.SaturdayHoursEnabled = settings[(object) "Policies.ENABLELDCORSAT"].ToString();
      this.correspondentLockDeskHours.SaturdayStart = settings[(object) "Policies.CORLDSATSTRTIME"].ToString();
      this.correspondentLockDeskHours.ShutDownLockDesk = settings[(object) "Policies.CORLDSHUTDOWN"].ToString();
      this.correspondentLockDeskHours.AllowActiveRelockRequests = settings[(object) "Policies.CORLDALLOWACTIVERELOCK"].ToString();
      this.correspondentLockDeskHours.SundayEnd = settings[(object) "Policies.CORLDSUNENDTIME"].ToString();
      this.correspondentLockDeskHours.SundayHoursEnabled = settings[(object) "Policies.ENABLELDCORSUN"].ToString();
      this.correspondentLockDeskHours.SundayStart = settings[(object) "Policies.CORLDSUNSTRTIME"].ToString();
      this.correspondentLockDeskHours.WeekdayEnd = settings[(object) "Policies.CORLDENDTIME"].ToString();
      this.correspondentLockDeskHours.WeekdayStart = settings[(object) "Policies.CORLDSTRTIME"].ToString();
      this.correspondentLockDeskHoursOld = this.correspondentLockDeskHours;
      this.retailLockDeskHourCtrl.EnableOnrpChangedEvent += new LockDeskHourControl.EnableOnrpChanged(this.EnableOnrpChanged);
      this.wholesaleLockDeskHourCtrl.EnableOnrpChangedEvent += new LockDeskHourControl.EnableOnrpChanged(this.EnableOnrpChanged);
      this.correspondentLockDeskHourCtrl.EnableOnrpChangedEvent += new LockDeskHourControl.EnableOnrpChanged(this.EnableOnrpChanged);
      this.retailLockDeskHourCtrl.Init(this.retailLockDeskHours);
      this.wholesaleLockDeskHourCtrl.Init(this.wholesaleLockDeskHours);
      this.correspondentLockDeskHourCtrl.Init(this.correspondentLockDeskHours);
      if (this.rdoCentralLockDeskHours.Checked)
        this.tabLockDeskHours.Hide();
      this.txtAddendumMessage.Text = settings[(object) "Policies.ONRPOverLimitMsgAddendum"] as string;
      this.txtStandardMessage.Text = "Overnight Rate Protection for Loan <Loan Number> exceeded Company limit by $<Dollar Amount>.";
      this.txtStandardMessage.Enabled = false;
      this.EnableOnrpChanged((object) null, (EventArgs) null);
      this.retailLockDeskHourCtrl.CentralChannel_Changed += new LockDeskHourControl.CentralChannelSelected(this.CentralChannelSelection);
      this.retailLockDeskHourCtrl.ONRPChannelChanged += new LockDeskHourControl.onrpChannelChanged(this.ONRPChannelChanged);
      this.retailLockDeskHourCtrl.ONRPChannelChangedFrom += new LockDeskHourControl.onrpChannelChangedFrom(this.ONRPChannelChangedFrom);
      this.retailLockDeskHourCtrl.isDirty += new LockDeskHourControl.IsDirty(this.lockdesk_isDirty);
      this.wholesaleLockDeskHourCtrl.CentralChannel_Changed += new LockDeskHourControl.CentralChannelSelected(this.CentralChannelSelection);
      this.wholesaleLockDeskHourCtrl.ONRPChannelChanged += new LockDeskHourControl.onrpChannelChanged(this.ONRPChannelChanged);
      this.wholesaleLockDeskHourCtrl.ONRPChannelChangedFrom += new LockDeskHourControl.onrpChannelChangedFrom(this.ONRPChannelChangedFrom);
      this.wholesaleLockDeskHourCtrl.isDirty += new LockDeskHourControl.IsDirty(this.lockdesk_isDirty);
      this.correspondentLockDeskHourCtrl.CentralChannel_Changed += new LockDeskHourControl.CentralChannelSelected(this.CentralChannelSelection);
      this.correspondentLockDeskHourCtrl.ONRPChannelChanged += new LockDeskHourControl.onrpChannelChanged(this.ONRPChannelChanged);
      this.correspondentLockDeskHourCtrl.ONRPChannelChangedFrom += new LockDeskHourControl.onrpChannelChangedFrom(this.ONRPChannelChangedFrom);
      this.correspondentLockDeskHourCtrl.isDirty += new LockDeskHourControl.IsDirty(this.lockdesk_isDirty);
      this.rdoCentralLockDeskHours.CheckedChanged += new EventHandler(this.rdoCentralLockDeskHours_CheckedChanged);
    }

    private void lockdesk_isDirty(bool value)
    {
      this.setDirtyFlag(true);
      this.updateLockDeskHours = true;
    }

    private bool SaveLockDeskSettings()
    {
      LockDeskScheduleHours settings1 = this.retailLockDeskHourCtrl.GetSettings();
      this.retailLockDeskHours = settings1;
      LockDeskScheduleHours settings2 = this.wholesaleLockDeskHourCtrl.GetSettings();
      this.wholesaleLockDeskHours = settings2;
      LockDeskScheduleHours settings3 = this.correspondentLockDeskHourCtrl.GetSettings();
      this.correspondentLockDeskHours = settings3;
      Session.ServerManager.UpdateServerSetting("Policies.ENABLELDRETONLY", (object) (settings1.CentralChannel.ToLower() == "false"));
      Session.ServerManager.UpdateServerSetting("Policies.RETLDHRMSG", (object) settings1.LockDeskHoursMessage);
      Session.ServerManager.UpdateServerSetting("Policies.RETLDSHUTDOWNMSG", (object) settings1.LockDeskShutDownMessage);
      Session.ServerManager.UpdateServerSetting("Policies.RETLDSATENDTIME", (object) settings1.SaturdayEnd);
      Session.ServerManager.UpdateServerSetting("Policies.ENABLELDRETSAT", (object) settings1.SaturdayHoursEnabled);
      Session.ServerManager.UpdateServerSetting("Policies.RETLDSATSTRTIME", (object) settings1.SaturdayStart);
      Session.ServerManager.UpdateServerSetting("Policies.RETLDSHUTDOWN", (object) settings1.ShutDownLockDesk);
      Session.ServerManager.UpdateServerSetting("Policies.RETLDALLOWACTIVERELOCK", (object) settings1.AllowActiveRelockRequests);
      Session.ServerManager.UpdateServerSetting("Policies.RETLDSUNENDTIME", (object) settings1.SundayEnd);
      Session.ServerManager.UpdateServerSetting("Policies.ENABLELDRETSUN", (object) settings1.SundayHoursEnabled);
      Session.ServerManager.UpdateServerSetting("Policies.RETLDSUNSTRTIME", (object) settings1.SundayStart);
      Session.ServerManager.UpdateServerSetting("Policies.RETLDSTRTIME", (object) settings1.WeekdayStart);
      Session.ServerManager.UpdateServerSetting("Policies.RETLDENDTIME", (object) settings1.WeekdayEnd);
      Session.ServerManager.UpdateServerSetting("Policies.ENABLELDBROKERONLY", (object) (settings2.CentralChannel.ToLower() == "false"));
      Session.ServerManager.UpdateServerSetting("Policies.BROKERLDHRMSG", (object) settings2.LockDeskHoursMessage);
      Session.ServerManager.UpdateServerSetting("Policies.BROKERLDSHUTDOWNMSG", (object) settings2.LockDeskShutDownMessage);
      Session.ServerManager.UpdateServerSetting("Policies.BROKERLDSATENDTIME", (object) settings2.SaturdayEnd);
      Session.ServerManager.UpdateServerSetting("Policies.ENABLELDBROKERSAT", (object) settings2.SaturdayHoursEnabled);
      Session.ServerManager.UpdateServerSetting("Policies.BROKERLDSATSTRTIME", (object) settings2.SaturdayStart);
      Session.ServerManager.UpdateServerSetting("Policies.BROKERLDSHUTDOWN", (object) settings2.ShutDownLockDesk);
      Session.ServerManager.UpdateServerSetting("Policies.BROKERLDALLOWACTIVERELOCK", (object) settings2.AllowActiveRelockRequests);
      Session.ServerManager.UpdateServerSetting("Policies.BROKERLDSUNENDTIME", (object) settings2.SundayEnd);
      Session.ServerManager.UpdateServerSetting("Policies.ENABLELDBROKERSUN", (object) settings2.SundayHoursEnabled);
      Session.ServerManager.UpdateServerSetting("Policies.BROKERLDSUNSTRTIME", (object) settings2.SundayStart);
      Session.ServerManager.UpdateServerSetting("Policies.BROKERLDSTRTIME", (object) settings2.WeekdayStart);
      Session.ServerManager.UpdateServerSetting("Policies.BROKERLDENDTIME", (object) settings2.WeekdayEnd);
      Session.ServerManager.UpdateServerSetting("Policies.ENABLELDCORONLY", (object) (settings3.CentralChannel.ToLower() == "false"));
      Session.ServerManager.UpdateServerSetting("Policies.CORLDHRMSG", (object) settings3.LockDeskHoursMessage);
      Session.ServerManager.UpdateServerSetting("Policies.CORLDSHUTDOWNMSG", (object) settings3.LockDeskShutDownMessage);
      Session.ServerManager.UpdateServerSetting("Policies.CORLDSATENDTIME", (object) settings3.SaturdayEnd);
      Session.ServerManager.UpdateServerSetting("Policies.ENABLELDCORSAT", (object) settings3.SaturdayHoursEnabled);
      Session.ServerManager.UpdateServerSetting("Policies.CORLDSATSTRTIME", (object) settings3.SaturdayStart);
      Session.ServerManager.UpdateServerSetting("Policies.CORLDSHUTDOWN", (object) settings3.ShutDownLockDesk);
      Session.ServerManager.UpdateServerSetting("Policies.CORLDALLOWACTIVERELOCK", (object) settings3.AllowActiveRelockRequests);
      Session.ServerManager.UpdateServerSetting("Policies.CORLDSunEndTime", (object) settings3.SundayEnd);
      Session.ServerManager.UpdateServerSetting("Policies.ENABLELDCORSUN", (object) settings3.SundayHoursEnabled);
      Session.ServerManager.UpdateServerSetting("Policies.CORLDSUNSTRTIME", (object) settings3.SundayStart);
      Session.ServerManager.UpdateServerSetting("Policies.CORLDSTRTIME", (object) settings3.WeekdayStart);
      Session.ServerManager.UpdateServerSetting("Policies.CORLDENDTIME", (object) settings3.WeekdayEnd);
      if (settings1.CentralChannel.ToLower() == "true" && settings2.CentralChannel.ToLower() == "true" && settings3.CentralChannel.ToLower() == "true")
        Session.ServerManager.UpdateServerSetting("Policies.ENABLEALLCHANNEL", (object) "true");
      else
        Session.ServerManager.UpdateServerSetting("Policies.ENABLEALLCHANNEL", (object) "false");
      return true;
    }

    private void SaveOnrpSettings(LockDeskGlobalSettings onrpSettings)
    {
      string channelString = onrpSettings.ChannelString;
      Session.ServerManager.UpdateServerSetting("Policies.EnableONRP" + channelString, (object) onrpSettings.ONRPEnabled);
      if (onrpSettings.Channel == LoanChannel.Correspondent)
        Session.ServerManager.UpdateServerSetting("Policies.ONRPCancelledExpiredLocks" + channelString, (object) onrpSettings.AllowONRPForCancelledExpiredLocks);
      Session.ServerManager.UpdateServerSetting(string.Format("Policies.ONRP{0}CVRG", (object) channelString), (object) onrpSettings.ContinuousCoverage);
      Session.ServerManager.UpdateServerSetting(string.Format("Policies.ONRP{0}ENDTIME", (object) channelString), (object) onrpSettings.ONRPEndTime);
      Session.ServerManager.UpdateServerSetting(string.Format("Policies.ONRP{0}SATENDTIME", (object) channelString), (object) onrpSettings.ONRPSatEndTime);
      Session.ServerManager.UpdateServerSetting(string.Format("Policies.ONRP{0}SUNENDTIME", (object) channelString), (object) onrpSettings.ONRPSunEndTime);
      Session.ServerManager.UpdateServerSetting(string.Format("Policies.ENABLEONRP{0}SAT", (object) channelString), (object) onrpSettings.ONRPSatEnabled);
      Session.ServerManager.UpdateServerSetting(string.Format("Policies.ENABLEONRP{0}SUN", (object) channelString), (object) onrpSettings.ONRPSunEnabled);
      Session.ServerManager.UpdateServerSetting(string.Format("Policies.ENABLEONRPWH{0}CVRG", (object) channelString), (object) onrpSettings.WeekendHoliday);
      Session.ServerManager.UpdateServerSetting(string.Format("Policies.ONRPNOMAXLIMIT{0}", (object) channelString), (object) onrpSettings.NoMaxLimit);
      Session.ServerManager.UpdateServerSetting(string.Format("Policies.ONRP{0}DOLLIMIT", (object) channelString), (object) onrpSettings.DollarLimit);
      Session.ServerManager.UpdateServerSetting(string.Format("Policies.ONRP{0}DOLTOL", (object) channelString), (object) onrpSettings.Tolerance);
    }

    private void tabLockDeskHours_TabIndexChanged(object sender, EventArgs e)
    {
      TabPage tabPage = sender as TabPage;
    }

    private void CentralChannelSelection(LoanChannel sender, bool allchannel)
    {
      if (allchannel)
      {
        this.CopyToOtherChannels(sender, allchannel);
      }
      else
      {
        this.retailLockDeskHourCtrl.SetToCentralLockDeskUI(allchannel);
        this.correspondentLockDeskHourCtrl.SetToCentralLockDeskUI(allchannel);
        this.wholesaleLockDeskHourCtrl.SetToCentralLockDeskUI(allchannel);
      }
    }

    private void ONRPChannelChanged(LoanChannel channel)
    {
      if (channel == LoanChannel.BankedRetail)
      {
        ((LockDeskHourControl) this.retailPanel.Controls[0]).SetChannelTab(LoanChannel.BankedRetail);
        ((LockDeskHourControl) this.wholesalePanel.Controls[0]).SetChannelTab(LoanChannel.BankedRetail);
        ((LockDeskHourControl) this.correspondentPanel.Controls[0]).SetChannelTab(LoanChannel.BankedRetail);
        this.tabLockDeskHours.SelectedIndex = 0;
        this.retailPanel.BringToFront();
      }
      if (channel == LoanChannel.BankedWholesale)
      {
        ((LockDeskHourControl) this.retailPanel.Controls[0]).SetChannelTab(LoanChannel.BankedWholesale);
        ((LockDeskHourControl) this.wholesalePanel.Controls[0]).SetChannelTab(LoanChannel.BankedWholesale);
        ((LockDeskHourControl) this.correspondentPanel.Controls[0]).SetChannelTab(LoanChannel.BankedWholesale);
        this.tabLockDeskHours.SelectedIndex = 1;
        this.wholesalePanel.BringToFront();
      }
      if (channel != LoanChannel.Correspondent)
        return;
      ((LockDeskHourControl) this.retailPanel.Controls[0]).SetChannelTab(LoanChannel.Correspondent);
      ((LockDeskHourControl) this.wholesalePanel.Controls[0]).SetChannelTab(LoanChannel.Correspondent);
      ((LockDeskHourControl) this.correspondentPanel.Controls[0]).SetChannelTab(LoanChannel.Correspondent);
      this.tabLockDeskHours.SelectedIndex = 2;
      this.correspondentPanel.BringToFront();
    }

    private void ONRPChannelChangedFrom(LoanChannel channel)
    {
      if (channel == LoanChannel.BankedRetail)
      {
        this.CopyToOtherChannels(channel, this.IsCentralChannelSelected);
        this.retailPanel.BringToFront();
      }
      if (channel == LoanChannel.BankedWholesale)
      {
        this.CopyToOtherChannels(channel, this.IsCentralChannelSelected);
        this.wholesalePanel.BringToFront();
      }
      if (channel != LoanChannel.Correspondent)
        return;
      this.CopyToOtherChannels(channel, this.IsCentralChannelSelected);
      this.correspondentPanel.BringToFront();
    }

    private void CopyToOtherChannels(LoanChannel sender, bool allchannel)
    {
      if (!allchannel)
        return;
      if (sender == LoanChannel.BankedRetail)
      {
        LockDeskScheduleHours settings = this.retailLockDeskHourCtrl.GetSettings();
        this.wholesaleLockDeskHourCtrl.Refresh(settings);
        this.correspondentLockDeskHourCtrl.Refresh(settings);
      }
      if (sender == LoanChannel.BankedWholesale)
      {
        LockDeskScheduleHours settings = this.wholesaleLockDeskHourCtrl.GetSettings();
        this.retailLockDeskHourCtrl.Refresh(settings);
        this.correspondentLockDeskHourCtrl.Refresh(settings);
      }
      if (sender != LoanChannel.Correspondent)
        return;
      LockDeskScheduleHours settings1 = this.correspondentLockDeskHourCtrl.GetSettings();
      this.retailLockDeskHourCtrl.Refresh(settings1);
      this.wholesaleLockDeskHourCtrl.Refresh(settings1);
    }

    private void tabLockDeskHours_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (((TabControl) sender).SelectedTab.Name == "tabRetailLockDesk")
      {
        this.ONRPChannelChanged(LoanChannel.BankedRetail);
        this.retailPanel.BringToFront();
      }
      if (((TabControl) sender).SelectedTab.Name == "tabWholesaleLockDesk")
      {
        this.ONRPChannelChanged(LoanChannel.BankedWholesale);
        this.wholesalePanel.BringToFront();
      }
      if (!(((TabControl) sender).SelectedTab.Name == "tabCorrespondentLockDesk"))
        return;
      this.ONRPChannelChanged(LoanChannel.Correspondent);
      this.correspondentPanel.BringToFront();
    }

    private void tabLockDeskHours_Deselected(object sender, TabControlEventArgs e)
    {
      if (!this.IsCentralChannelSelected)
        return;
      if (((TabControl) sender).SelectedTab.Name == "tabRetailLockDesk")
      {
        this.retailPanel.BringToFront();
        this.CopyToOtherChannels(LoanChannel.BankedRetail, true);
      }
      if (((TabControl) sender).SelectedTab.Name == "tabWholesaleLockDesk")
      {
        this.wholesalePanel.BringToFront();
        this.CopyToOtherChannels(LoanChannel.BankedWholesale, true);
      }
      if (!(((TabControl) sender).SelectedTab.Name == "tabCorrespondentLockDesk"))
        return;
      this.correspondentPanel.BringToFront();
      this.CopyToOtherChannels(LoanChannel.Correspondent, true);
    }

    private void PropagateToAllCannels(TabControl sender)
    {
      if (sender.SelectedTab.Name == "tabRetailLockDesk")
        this.CopyToOtherChannels(LoanChannel.BankedRetail, true);
      if (sender.SelectedTab.Name == "tabWholesaleLockDesk")
        this.CopyToOtherChannels(LoanChannel.BankedWholesale, true);
      if (!(sender.SelectedTab.Name == "tabCorrespondentLockDesk"))
        return;
      this.CopyToOtherChannels(LoanChannel.Correspondent, true);
    }

    private void rdoCentralLockDeskHours_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.rdoCentralLockDeskHours.Checked)
      {
        if (Utils.Dialog((IWin32Window) this, "Warning! Changing the channel indicator from \"Central Lock Desk Hours\" to \"Channel Lock Desk Hours\" should only be done when there is more than one Lock Desk, or the company wishes to have different Lock Desk Hours for each Channel. Do you wish to Continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
        {
          this.rdoCentralLockDeskHours.CheckedChanged -= new EventHandler(this.rdoCentralLockDeskHours_CheckedChanged);
          this.rdoCentralLockDeskHours.Checked = true;
          this.rdoChannelLockDeskHours.Checked = false;
          this.rdoCentralLockDeskHours.CheckedChanged += new EventHandler(this.rdoCentralLockDeskHours_CheckedChanged);
          return;
        }
        this.tabLockDeskHours.Show();
        this.retailLockDeskHourCtrl.SetToCentralLockDeskUI(false);
        this.wholesaleLockDeskHourCtrl.SetToCentralLockDeskUI(false);
        this.correspondentLockDeskHourCtrl.SetToCentralLockDeskUI(false);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Warning! Changing the channel indicator from \"Channel Lock Desk Hours\" to \"Central Lock Desk Hours\" should only be done when there is only one Lock Desk or the company wishes to have the same Lock Desk Hours for each Channel on a permanent basis. Do you wish to Continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
        {
          this.rdoCentralLockDeskHours.CheckedChanged -= new EventHandler(this.rdoCentralLockDeskHours_CheckedChanged);
          this.rdoCentralLockDeskHours.Checked = false;
          this.rdoChannelLockDeskHours.Checked = true;
          this.rdoCentralLockDeskHours.CheckedChanged += new EventHandler(this.rdoCentralLockDeskHours_CheckedChanged);
          return;
        }
        this.tabLockDeskHours.Hide();
        this.retailLockDeskHourCtrl.SetToCentralLockDeskUI(true);
        this.wholesaleLockDeskHourCtrl.SetToCentralLockDeskUI(true);
        this.correspondentLockDeskHourCtrl.SetToCentralLockDeskUI(true);
      }
      this.IsCentralChannelSelected = this.rdoCentralLockDeskHours.Checked;
      this.CentralChannelSelection(this.GetChannelFromTab(), this.rdoCentralLockDeskHours.Checked);
      this.setDirtyFlag(true);
    }

    private LoanChannel GetChannelFromTab()
    {
      if (this.tabLockDeskHours.SelectedIndex == 1)
        return LoanChannel.BankedWholesale;
      return this.tabLockDeskHours.SelectedIndex == 2 ? LoanChannel.Correspondent : LoanChannel.BankedRetail;
    }

    private void iconBtnHelpCommitmentTerm_Click(object sender, EventArgs e)
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Secondary Lock Desk Setup Expiration Date");
    }

    private void rBtnLockTermFields_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.rBtnLockTermFields.Checked && Utils.Dialog((IWin32Window) this, "By enabling the use of Commitment Term fields, standard Lock Term fields (761, 432, 762) in the Loan File will no longer be updated upon Lock/Commitment confirmation for loans in the Correspondent channel. Do you wish to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
      {
        this.rBtnLockTermFields.CheckedChanged -= new EventHandler(this.rBtnLockTermFields_CheckedChanged);
        this.rBtnLockTermFields.Checked = true;
        this.rBtnCommitmentTermFields.Checked = false;
        this.rBtnLockTermFields.CheckedChanged += new EventHandler(this.rBtnLockTermFields_CheckedChanged);
      }
      else
        this.setDirtyFlag(true);
    }

    private void rBtnCommitmentTermFields_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.tabCalendar = new TabControl();
      this.tpExpiration = new TabPage();
      this.panel2 = new Panel();
      this.groupContainer3 = new GroupContainer();
      this.panel8 = new Panel();
      this.label16 = new Label();
      this.label15 = new Label();
      this.iconBtnHelpCommitmentTerm = new IconButton();
      this.rBtnCommitmentTermFields = new RadioButton();
      this.label11 = new Label();
      this.rBtnLockTermFields = new RadioButton();
      this.groupContainer2 = new GroupContainer();
      this.panel4 = new Panel();
      this.label2 = new Label();
      this.rbtDayAfter = new RadioButton();
      this.rbtTheDate = new RadioButton();
      this.label7 = new Label();
      this.label4 = new Label();
      this.panel3 = new Panel();
      this.label3 = new Label();
      this.rbtNextDay = new RadioButton();
      this.rbtPreDay = new RadioButton();
      this.label5 = new Label();
      this.cmbTimezone = new ComboBox();
      this.txtHour = new TextBox();
      this.txtMinute = new TextBox();
      this.label6 = new Label();
      this.cmbTime = new ComboBox();
      this.tpCalendarHour = new TabPage();
      this.groupContainer1 = new GroupContainer();
      this.rbtLockExpCalCustom = new RadioButton();
      this.rbtLockExpCalNone = new RadioButton();
      this.label1 = new Label();
      this.chkCalendarOnLockExtension = new CheckBox();
      this.rbtLockExpCalBusiness = new RadioButton();
      this.rbtLockExpCalPostal = new RadioButton();
      this.grpCalendar = new GroupContainer();
      this.panel1 = new Panel();
      this.monthNov = new MonthNavigator();
      this.monthDec = new MonthNavigator();
      this.monthSep = new MonthNavigator();
      this.monthOct = new MonthNavigator();
      this.monthJul = new MonthNavigator();
      this.monthAug = new MonthNavigator();
      this.monthMay = new MonthNavigator();
      this.monthJun = new MonthNavigator();
      this.monthMar = new MonthNavigator();
      this.monthApr = new MonthNavigator();
      this.monthJan = new MonthNavigator();
      this.monthFeb = new MonthNavigator();
      this.gradientPanel1 = new GradientPanel();
      this.chkSundays = new CheckBox();
      this.chkSaturdays = new CheckBox();
      this.lblHeader = new Label();
      this.lblYear = new Label();
      this.btnPreviousYear = new StandardIconButton();
      this.btnNextYear = new StandardIconButton();
      this.tpLockDeskONRP = new TabPage();
      this.panel5 = new Panel();
      this.gcMessage = new GroupContainer();
      this.btnViewMsg = new Button();
      this.txtAddendumMessage = new TextBox();
      this.label29 = new Label();
      this.txtStandardMessage = new TextBox();
      this.label28 = new Label();
      this.gcLockDeskSchedule = new GroupContainer();
      this.retailPanel = new Panel();
      this.wholesalePanel = new Panel();
      this.correspondentPanel = new Panel();
      this.tabLockDeskHours = new TabControl();
      this.tabRetailLockDesk = new TabPage();
      this.tabWholesaleLockDesk = new TabPage();
      this.tabCorrespondentLockDesk = new TabPage();
      this.panel7 = new Panel();
      this.rdoChannelLockDeskHours = new RadioButton();
      this.rdoCentralLockDeskHours = new RadioButton();
      this.chkEnableEncompassLockDesk = new CheckBox();
      this.panel6 = new Panel();
      this.label10 = new Label();
      this.label12 = new Label();
      this.label14 = new Label();
      this.label13 = new Label();
      this.label9 = new Label();
      this.label8 = new Label();
      this.lblONRP = new Label();
      this.borderPanel1 = new BorderPanel();
      this.tabCalendar.SuspendLayout();
      this.tpExpiration.SuspendLayout();
      this.panel2.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.panel8.SuspendLayout();
      ((ISupportInitialize) this.iconBtnHelpCommitmentTerm).BeginInit();
      this.groupContainer2.SuspendLayout();
      this.panel4.SuspendLayout();
      this.panel3.SuspendLayout();
      this.tpCalendarHour.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.grpCalendar.SuspendLayout();
      this.panel1.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnPreviousYear).BeginInit();
      ((ISupportInitialize) this.btnNextYear).BeginInit();
      this.tpLockDeskONRP.SuspendLayout();
      this.panel5.SuspendLayout();
      this.gcMessage.SuspendLayout();
      this.gcLockDeskSchedule.SuspendLayout();
      this.tabLockDeskHours.SuspendLayout();
      this.panel7.SuspendLayout();
      this.panel6.SuspendLayout();
      this.SuspendLayout();
      this.tabCalendar.Controls.Add((Control) this.tpExpiration);
      this.tabCalendar.Controls.Add((Control) this.tpCalendarHour);
      this.tabCalendar.Controls.Add((Control) this.tpLockDeskONRP);
      this.tabCalendar.Dock = DockStyle.Fill;
      this.tabCalendar.ItemSize = new Size(100, 28);
      this.tabCalendar.Location = new Point(0, 0);
      this.tabCalendar.Name = "tabCalendar";
      this.tabCalendar.SelectedIndex = 0;
      this.tabCalendar.Size = new Size(1069, 980);
      this.tabCalendar.TabIndex = 0;
      this.tpExpiration.AutoScroll = true;
      this.tpExpiration.Controls.Add((Control) this.panel2);
      this.tpExpiration.Location = new Point(4, 32);
      this.tpExpiration.Name = "tpExpiration";
      this.tpExpiration.Padding = new Padding(3);
      this.tpExpiration.Size = new Size(1061, 944);
      this.tpExpiration.TabIndex = 0;
      this.tpExpiration.Text = "Expiration Settings";
      this.tpExpiration.UseVisualStyleBackColor = true;
      this.panel2.AutoScroll = true;
      this.panel2.AutoSize = true;
      this.panel2.BackColor = Color.WhiteSmoke;
      this.panel2.BorderStyle = BorderStyle.FixedSingle;
      this.panel2.Controls.Add((Control) this.groupContainer3);
      this.panel2.Controls.Add((Control) this.groupContainer2);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(3, 3);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(1055, 938);
      this.panel2.TabIndex = 29;
      this.groupContainer3.Controls.Add((Control) this.panel8);
      this.groupContainer3.Dock = DockStyle.Bottom;
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(0, 283);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(1053, 653);
      this.groupContainer3.TabIndex = 35;
      this.groupContainer3.Text = "Correspondent Commitment Term Settings";
      this.panel8.Controls.Add((Control) this.label16);
      this.panel8.Controls.Add((Control) this.label15);
      this.panel8.Controls.Add((Control) this.iconBtnHelpCommitmentTerm);
      this.panel8.Controls.Add((Control) this.rBtnCommitmentTermFields);
      this.panel8.Controls.Add((Control) this.label11);
      this.panel8.Controls.Add((Control) this.rBtnLockTermFields);
      this.panel8.Dock = DockStyle.Top;
      this.panel8.Location = new Point(1, 26);
      this.panel8.Name = "panel8";
      this.panel8.Size = new Size(1051, 187);
      this.panel8.TabIndex = 40;
      this.label16.AutoSize = true;
      this.label16.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label16.Location = new Point(31, 25);
      this.label16.Name = "label16";
      this.label16.Size = new Size(521, 13);
      this.label16.TabIndex = 43;
      this.label16.Text = "fields will enable new behavior when a loan is committed (locked) between a Correspondent Seller and Buyer.";
      this.label15.AutoSize = true;
      this.label15.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label15.Location = new Point(27, 11);
      this.label15.Name = "label15";
      this.label15.Size = new Size(940, 13);
      this.label15.TabIndex = 42;
      this.label15.Text = " The setting below will allow Correspondent Sellers and Buyers to distinguish between the Borrower Lock Terms and Correspondent Commitment Terms within the loan file. Using the Commitment Term";
      this.iconBtnHelpCommitmentTerm.Anchor = AnchorStyles.Left;
      this.iconBtnHelpCommitmentTerm.DisabledImage = (Image) null;
      this.iconBtnHelpCommitmentTerm.Image = (Image) Resources.help;
      this.iconBtnHelpCommitmentTerm.Location = new Point(562, 25);
      this.iconBtnHelpCommitmentTerm.MouseDownImage = (Image) null;
      this.iconBtnHelpCommitmentTerm.MouseOverImage = (Image) Resources.help_over;
      this.iconBtnHelpCommitmentTerm.Name = "iconBtnHelpCommitmentTerm";
      this.iconBtnHelpCommitmentTerm.Size = new Size(18, 17);
      this.iconBtnHelpCommitmentTerm.TabIndex = 38;
      this.iconBtnHelpCommitmentTerm.TabStop = false;
      this.iconBtnHelpCommitmentTerm.Click += new EventHandler(this.iconBtnHelpCommitmentTerm_Click);
      this.rBtnCommitmentTermFields.AutoSize = true;
      this.rBtnCommitmentTermFields.Location = new Point(34, 99);
      this.rBtnCommitmentTermFields.Name = "rBtnCommitmentTermFields";
      this.rBtnCommitmentTermFields.Size = new Size(587, 17);
      this.rBtnCommitmentTermFields.TabIndex = 36;
      this.rBtnCommitmentTermFields.TabStop = true;
      this.rBtnCommitmentTermFields.Text = "Use Commitment Term fields (4527, 4528, 4529) in the Correspondent Loan Status Tool to represent Commitment Terms";
      this.rBtnCommitmentTermFields.UseVisualStyleBackColor = true;
      this.rBtnCommitmentTermFields.CheckedChanged += new EventHandler(this.rBtnCommitmentTermFields_CheckedChanged);
      this.label11.AutoSize = true;
      this.label11.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label11.Location = new Point(31, 59);
      this.label11.Name = "label11";
      this.label11.Size = new Size(646, 13);
      this.label11.TabIndex = 39;
      this.label11.Text = "For Correspondent loans and upon Lock Confirmation update the following fields to represent commitment dates:";
      this.rBtnLockTermFields.AutoSize = true;
      this.rBtnLockTermFields.Checked = true;
      this.rBtnLockTermFields.Location = new Point(34, 81);
      this.rBtnLockTermFields.Name = "rBtnLockTermFields";
      this.rBtnLockTermFields.Size = new Size(470, 17);
      this.rBtnLockTermFields.TabIndex = 35;
      this.rBtnLockTermFields.TabStop = true;
      this.rBtnLockTermFields.Text = "Use standard Lock Term fields (761, 432, 762) in the Loan File to represent Commitment Terms";
      this.rBtnLockTermFields.UseVisualStyleBackColor = true;
      this.rBtnLockTermFields.CheckedChanged += new EventHandler(this.rBtnLockTermFields_CheckedChanged);
      this.groupContainer2.Controls.Add((Control) this.panel4);
      this.groupContainer2.Controls.Add((Control) this.label7);
      this.groupContainer2.Controls.Add((Control) this.label4);
      this.groupContainer2.Controls.Add((Control) this.panel3);
      this.groupContainer2.Controls.Add((Control) this.label5);
      this.groupContainer2.Controls.Add((Control) this.cmbTimezone);
      this.groupContainer2.Controls.Add((Control) this.txtHour);
      this.groupContainer2.Controls.Add((Control) this.txtMinute);
      this.groupContainer2.Controls.Add((Control) this.label6);
      this.groupContainer2.Controls.Add((Control) this.cmbTime);
      this.groupContainer2.Dock = DockStyle.Top;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 0);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(1053, 283);
      this.groupContainer2.TabIndex = 34;
      this.groupContainer2.Text = "Lock Expiration Settings";
      this.panel4.Controls.Add((Control) this.label2);
      this.panel4.Controls.Add((Control) this.rbtDayAfter);
      this.panel4.Controls.Add((Control) this.rbtTheDate);
      this.panel4.Location = new Point(15, 29);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(409, 69);
      this.panel4.TabIndex = 33;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(12, 8);
      this.label2.Name = "label2";
      this.label2.Size = new Size(206, 13);
      this.label2.TabIndex = 15;
      this.label2.Text = "When should the lock period start?";
      this.rbtDayAfter.AutoSize = true;
      this.rbtDayAfter.Location = new Point(17, 51);
      this.rbtDayAfter.Name = "rbtDayAfter";
      this.rbtDayAfter.Size = new Size(176, 17);
      this.rbtDayAfter.TabIndex = 17;
      this.rbtDayAfter.TabStop = true;
      this.rbtDayAfter.Text = "One day after the rate is locked.";
      this.rbtDayAfter.UseVisualStyleBackColor = true;
      this.rbtTheDate.AutoSize = true;
      this.rbtTheDate.Location = new Point(17, 27);
      this.rbtTheDate.Name = "rbtTheDate";
      this.rbtTheDate.Size = new Size(168, 17);
      this.rbtTheDate.TabIndex = 16;
      this.rbtTheDate.TabStop = true;
      this.rbtTheDate.Text = "On the date the rate is locked.";
      this.rbtTheDate.UseVisualStyleBackColor = true;
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(28, 202);
      this.label7.Name = "label7";
      this.label7.Size = new Size((int) byte.MaxValue, 13);
      this.label7.TabIndex = 28;
      this.label7.Text = "Loan Estimate Rate Lock Expiration Default";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(28, 229);
      this.label4.Name = "label4";
      this.label4.Size = new Size(132, 13);
      this.label4.TabIndex = 17;
      this.label4.Text = "Rate Lock Expiration Time";
      this.panel3.Controls.Add((Control) this.label3);
      this.panel3.Controls.Add((Control) this.rbtNextDay);
      this.panel3.Controls.Add((Control) this.rbtPreDay);
      this.panel3.Location = new Point(15, 111);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(409, 68);
      this.panel3.TabIndex = 32;
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(12, 6);
      this.label3.Name = "label3";
      this.label3.Size = new Size(266, 13);
      this.label3.TabIndex = 32;
      this.label3.Text = "When expiration date is an excluded day use:";
      this.rbtNextDay.AutoSize = true;
      this.rbtNextDay.Location = new Point(17, 49);
      this.rbtNextDay.Name = "rbtNextDay";
      this.rbtNextDay.Size = new Size(114, 17);
      this.rbtNextDay.TabIndex = 34;
      this.rbtNextDay.TabStop = true;
      this.rbtNextDay.Text = "Next Business Day";
      this.rbtNextDay.UseVisualStyleBackColor = true;
      this.rbtPreDay.AutoSize = true;
      this.rbtPreDay.Location = new Point(17, 25);
      this.rbtPreDay.Name = "rbtPreDay";
      this.rbtPreDay.Size = new Size(133, 17);
      this.rbtPreDay.TabIndex = 33;
      this.rbtPreDay.TabStop = true;
      this.rbtPreDay.Text = "Previous Business Day";
      this.rbtPreDay.UseVisualStyleBackColor = true;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(28, 256);
      this.label5.Name = "label5";
      this.label5.Size = new Size(160, 13);
      this.label5.TabIndex = 19;
      this.label5.Text = "Rate Lock Expiration Time Zone";
      this.cmbTimezone.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbTimezone.FormattingEnabled = true;
      this.cmbTimezone.Items.AddRange(new object[7]
      {
        (object) "(UTC -10:00) Hawaii Time",
        (object) "(UTC -09:00) Alaska Time",
        (object) "(UTC -08:00) Pacific Time",
        (object) "(UTC -07:00) Arizona Time",
        (object) "(UTC -07:00) Mountain Time",
        (object) "(UTC -06:00) Central Time",
        (object) "(UTC -05:00) Eastern Time"
      });
      this.cmbTimezone.Location = new Point(198, 256);
      this.cmbTimezone.Name = "cmbTimezone";
      this.cmbTimezone.Size = new Size(163, 21);
      this.cmbTimezone.TabIndex = 27;
      this.cmbTimezone.SelectedIndexChanged += new EventHandler(this.cmbTimezone_SelectedIndexChanged);
      this.txtHour.Location = new Point(197, 226);
      this.txtHour.Name = "txtHour";
      this.txtHour.Size = new Size(28, 20);
      this.txtHour.TabIndex = 21;
      this.txtHour.KeyPress += new KeyPressEventHandler(this.txtHour_KeyPress);
      this.txtMinute.Location = new Point(247, 226);
      this.txtMinute.Name = "txtMinute";
      this.txtMinute.Size = new Size(28, 20);
      this.txtMinute.TabIndex = 25;
      this.txtMinute.KeyPress += new KeyPressEventHandler(this.txtMinute_KeyPress);
      this.txtMinute.Leave += new EventHandler(this.txtMinute_Leave);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(231, 229);
      this.label6.Name = "label6";
      this.label6.Size = new Size(10, 13);
      this.label6.TabIndex = 23;
      this.label6.Text = ":";
      this.cmbTime.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbTime.FormattingEnabled = true;
      this.cmbTime.Items.AddRange(new object[2]
      {
        (object) "AM",
        (object) "PM"
      });
      this.cmbTime.Location = new Point(290, 225);
      this.cmbTime.Name = "cmbTime";
      this.cmbTime.Size = new Size(42, 21);
      this.cmbTime.TabIndex = 26;
      this.cmbTime.SelectedIndexChanged += new EventHandler(this.cmbTime_SelectedIndexChanged);
      this.tpCalendarHour.AutoScroll = true;
      this.tpCalendarHour.Controls.Add((Control) this.groupContainer1);
      this.tpCalendarHour.Controls.Add((Control) this.grpCalendar);
      this.tpCalendarHour.Location = new Point(4, 32);
      this.tpCalendarHour.Name = "tpCalendarHour";
      this.tpCalendarHour.Padding = new Padding(3);
      this.tpCalendarHour.Size = new Size(1061, 944);
      this.tpCalendarHour.TabIndex = 1;
      this.tpCalendarHour.Text = "Calendar";
      this.tpCalendarHour.UseVisualStyleBackColor = true;
      this.groupContainer1.Controls.Add((Control) this.rbtLockExpCalCustom);
      this.groupContainer1.Controls.Add((Control) this.rbtLockExpCalNone);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.chkCalendarOnLockExtension);
      this.groupContainer1.Controls.Add((Control) this.rbtLockExpCalBusiness);
      this.groupContainer1.Controls.Add((Control) this.rbtLockExpCalPostal);
      this.groupContainer1.Dock = DockStyle.Top;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(3, 3);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(1055, 160);
      this.groupContainer1.TabIndex = 31;
      this.groupContainer1.Text = "Configure Your Lock Desk Calendar";
      this.rbtLockExpCalCustom.AutoSize = true;
      this.rbtLockExpCalCustom.Location = new Point(13, (int) sbyte.MaxValue);
      this.rbtLockExpCalCustom.Name = "rbtLockExpCalCustom";
      this.rbtLockExpCalCustom.Size = new Size(160, 17);
      this.rbtLockExpCalCustom.TabIndex = 4;
      this.rbtLockExpCalCustom.TabStop = true;
      this.rbtLockExpCalCustom.Text = "Custom Lock Desk Calendar";
      this.rbtLockExpCalCustom.UseVisualStyleBackColor = true;
      this.rbtLockExpCalCustom.CheckedChanged += new EventHandler(this.rbtn_CheckedChanged);
      this.rbtLockExpCalNone.AutoSize = true;
      this.rbtLockExpCalNone.Location = new Point(13, 104);
      this.rbtLockExpCalNone.Name = "rbtLockExpCalNone";
      this.rbtLockExpCalNone.Size = new Size(256, 17);
      this.rbtLockExpCalNone.TabIndex = 3;
      this.rbtLockExpCalNone.TabStop = true;
      this.rbtLockExpCalNone.Text = "None (locks can expire on a weekend or holiday)";
      this.rbtLockExpCalNone.UseVisualStyleBackColor = true;
      this.rbtLockExpCalNone.CheckedChanged += new EventHandler(this.rbtn_CheckedChanged);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(11, 37);
      this.label1.Name = "label1";
      this.label1.Size = new Size(206, 13);
      this.label1.TabIndex = 25;
      this.label1.Text = "Calendar to use for lock expiration:";
      this.chkCalendarOnLockExtension.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkCalendarOnLockExtension.AutoSize = true;
      this.chkCalendarOnLockExtension.BackColor = Color.Transparent;
      this.chkCalendarOnLockExtension.Location = new Point(906, 3);
      this.chkCalendarOnLockExtension.Name = "chkCalendarOnLockExtension";
      this.chkCalendarOnLockExtension.Size = new Size(145, 17);
      this.chkCalendarOnLockExtension.TabIndex = 5;
      this.chkCalendarOnLockExtension.Text = "Apply to Lock Extensions";
      this.chkCalendarOnLockExtension.UseVisualStyleBackColor = false;
      this.chkCalendarOnLockExtension.CheckedChanged += new EventHandler(this.chkCalendarOnLockExtension_CheckedChanged);
      this.rbtLockExpCalBusiness.AutoSize = true;
      this.rbtLockExpCalBusiness.Location = new Point(13, 81);
      this.rbtLockExpCalBusiness.Name = "rbtLockExpCalBusiness";
      this.rbtLockExpCalBusiness.Size = new Size(398, 17);
      this.rbtLockExpCalBusiness.TabIndex = 2;
      this.rbtLockExpCalBusiness.TabStop = true;
      this.rbtLockExpCalBusiness.Text = "Company Calendar (defined in the Loan Setup > Compliance Calendar Settings)";
      this.rbtLockExpCalBusiness.UseVisualStyleBackColor = true;
      this.rbtLockExpCalBusiness.CheckedChanged += new EventHandler(this.rbtn_CheckedChanged);
      this.rbtLockExpCalPostal.AutoSize = true;
      this.rbtLockExpCalPostal.Location = new Point(14, 59);
      this.rbtLockExpCalPostal.Name = "rbtLockExpCalPostal";
      this.rbtLockExpCalPostal.Size = new Size(123, 17);
      this.rbtLockExpCalPostal.TabIndex = 1;
      this.rbtLockExpCalPostal.TabStop = true;
      this.rbtLockExpCalPostal.Text = "U.S. Postal Calendar";
      this.rbtLockExpCalPostal.UseVisualStyleBackColor = true;
      this.rbtLockExpCalPostal.CheckedChanged += new EventHandler(this.rbtn_CheckedChanged);
      this.grpCalendar.Controls.Add((Control) this.panel1);
      this.grpCalendar.Controls.Add((Control) this.gradientPanel1);
      this.grpCalendar.Controls.Add((Control) this.lblYear);
      this.grpCalendar.Controls.Add((Control) this.btnPreviousYear);
      this.grpCalendar.Controls.Add((Control) this.btnNextYear);
      this.grpCalendar.Dock = DockStyle.Bottom;
      this.grpCalendar.HeaderForeColor = SystemColors.ControlText;
      this.grpCalendar.Location = new Point(3, 388);
      this.grpCalendar.Name = "grpCalendar";
      this.grpCalendar.Size = new Size(1055, 553);
      this.grpCalendar.TabIndex = 32;
      this.panel1.Controls.Add((Control) this.monthNov);
      this.panel1.Controls.Add((Control) this.monthDec);
      this.panel1.Controls.Add((Control) this.monthSep);
      this.panel1.Controls.Add((Control) this.monthOct);
      this.panel1.Controls.Add((Control) this.monthJul);
      this.panel1.Controls.Add((Control) this.monthAug);
      this.panel1.Controls.Add((Control) this.monthMay);
      this.panel1.Controls.Add((Control) this.monthJun);
      this.panel1.Controls.Add((Control) this.monthMar);
      this.panel1.Controls.Add((Control) this.monthApr);
      this.panel1.Controls.Add((Control) this.monthJan);
      this.panel1.Controls.Add((Control) this.monthFeb);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(1, 57);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(1053, 495);
      this.panel1.TabIndex = 6;
      this.monthNov.HighlightedDateForeColor = SystemColors.ControlText;
      this.monthNov.Location = new Point(322, 326);
      this.monthNov.MonthNameForeColor = SystemColors.ControlText;
      this.monthNov.Name = "monthNov";
      this.monthNov.Size = new Size(146, 148);
      this.monthNov.TabIndex = 14;
      this.monthNov.Text = "monthNavigator9";
      this.monthNov.TitleFormat = "MMMM yyyy";
      this.monthDec.HighlightedDateForeColor = SystemColors.ControlText;
      this.monthDec.Location = new Point(478, 326);
      this.monthDec.MonthNameForeColor = SystemColors.ControlText;
      this.monthDec.Name = "monthDec";
      this.monthDec.Size = new Size(146, 148);
      this.monthDec.TabIndex = 15;
      this.monthDec.Text = "monthNavigator10";
      this.monthDec.TitleFormat = "MMMM yyyy";
      this.monthSep.HighlightedDateForeColor = SystemColors.ControlText;
      this.monthSep.Location = new Point(10, 326);
      this.monthSep.MonthNameForeColor = SystemColors.ControlText;
      this.monthSep.Name = "monthSep";
      this.monthSep.Size = new Size(146, 148);
      this.monthSep.TabIndex = 12;
      this.monthSep.Text = "monthNavigator11";
      this.monthSep.TitleFormat = "MMMM yyyy";
      this.monthOct.HighlightedDateForeColor = SystemColors.ControlText;
      this.monthOct.Location = new Point(166, 326);
      this.monthOct.MonthNameForeColor = SystemColors.ControlText;
      this.monthOct.Name = "monthOct";
      this.monthOct.Size = new Size(146, 148);
      this.monthOct.TabIndex = 13;
      this.monthOct.Text = "monthNavigator12";
      this.monthOct.TitleFormat = "MMMM yyyy";
      this.monthJul.HighlightedDateForeColor = SystemColors.ControlText;
      this.monthJul.Location = new Point(322, 168);
      this.monthJul.MonthNameForeColor = SystemColors.ControlText;
      this.monthJul.Name = "monthJul";
      this.monthJul.Size = new Size(146, 148);
      this.monthJul.TabIndex = 10;
      this.monthJul.Text = "monthNavigator5";
      this.monthJul.TitleFormat = "MMMM yyyy";
      this.monthAug.HighlightedDateForeColor = SystemColors.ControlText;
      this.monthAug.Location = new Point(478, 168);
      this.monthAug.MonthNameForeColor = SystemColors.ControlText;
      this.monthAug.Name = "monthAug";
      this.monthAug.Size = new Size(146, 148);
      this.monthAug.TabIndex = 11;
      this.monthAug.Text = "monthNavigator6";
      this.monthAug.TitleFormat = "MMMM yyyy";
      this.monthMay.HighlightedDateForeColor = SystemColors.ControlText;
      this.monthMay.Location = new Point(10, 168);
      this.monthMay.MonthNameForeColor = SystemColors.ControlText;
      this.monthMay.Name = "monthMay";
      this.monthMay.Size = new Size(146, 148);
      this.monthMay.TabIndex = 8;
      this.monthMay.Text = "monthNavigator7";
      this.monthMay.TitleFormat = "MMMM yyyy";
      this.monthJun.HighlightedDateForeColor = SystemColors.ControlText;
      this.monthJun.Location = new Point(166, 168);
      this.monthJun.MonthNameForeColor = SystemColors.ControlText;
      this.monthJun.Name = "monthJun";
      this.monthJun.Size = new Size(146, 148);
      this.monthJun.TabIndex = 9;
      this.monthJun.Text = "monthNavigator8";
      this.monthJun.TitleFormat = "MMMM yyyy";
      this.monthMar.HighlightedDateForeColor = SystemColors.ControlText;
      this.monthMar.Location = new Point(322, 10);
      this.monthMar.MonthNameForeColor = SystemColors.ControlText;
      this.monthMar.Name = "monthMar";
      this.monthMar.Size = new Size(146, 148);
      this.monthMar.TabIndex = 6;
      this.monthMar.Text = "monthNavigator3";
      this.monthMar.TitleFormat = "MMMM yyyy";
      this.monthApr.HighlightedDateForeColor = SystemColors.ControlText;
      this.monthApr.Location = new Point(478, 10);
      this.monthApr.MonthNameForeColor = SystemColors.ControlText;
      this.monthApr.Name = "monthApr";
      this.monthApr.Size = new Size(146, 148);
      this.monthApr.TabIndex = 7;
      this.monthApr.Text = "monthNavigator4";
      this.monthApr.TitleFormat = "MMMM yyyy";
      this.monthJan.HighlightedDateForeColor = SystemColors.ControlText;
      this.monthJan.Location = new Point(10, 10);
      this.monthJan.MonthNameForeColor = SystemColors.ControlText;
      this.monthJan.Name = "monthJan";
      this.monthJan.Size = new Size(146, 148);
      this.monthJan.TabIndex = 4;
      this.monthJan.Text = "monthNavigator1";
      this.monthJan.TitleFormat = "MMMM yyyy";
      this.monthFeb.HighlightedDateForeColor = SystemColors.ControlText;
      this.monthFeb.Location = new Point(166, 10);
      this.monthFeb.MonthNameForeColor = SystemColors.ControlText;
      this.monthFeb.Name = "monthFeb";
      this.monthFeb.Size = new Size(146, 148);
      this.monthFeb.TabIndex = 5;
      this.monthFeb.Text = "monthNavigator2";
      this.monthFeb.TitleFormat = "MMMM yyyy";
      this.gradientPanel1.Borders = AnchorStyles.Top;
      this.gradientPanel1.Controls.Add((Control) this.chkSundays);
      this.gradientPanel1.Controls.Add((Control) this.chkSaturdays);
      this.gradientPanel1.Controls.Add((Control) this.lblHeader);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(1, 26);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(1053, 31);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel1.TabIndex = 3;
      this.chkSundays.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkSundays.AutoSize = true;
      this.chkSundays.BackColor = Color.Transparent;
      this.chkSundays.Location = new Point(943, 7);
      this.chkSundays.Name = "chkSundays";
      this.chkSundays.Size = new Size(108, 17);
      this.chkSundays.TabIndex = 21;
      this.chkSundays.Text = "Exclude Sundays";
      this.chkSundays.UseVisualStyleBackColor = false;
      this.chkSundays.CheckedChanged += new EventHandler(this.chkSundays_CheckedChanged);
      this.chkSaturdays.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkSaturdays.AutoSize = true;
      this.chkSaturdays.BackColor = Color.Transparent;
      this.chkSaturdays.Location = new Point(825, 7);
      this.chkSaturdays.Name = "chkSaturdays";
      this.chkSaturdays.Size = new Size(114, 17);
      this.chkSaturdays.TabIndex = 20;
      this.chkSaturdays.Text = "Exclude Saturdays";
      this.chkSaturdays.UseVisualStyleBackColor = false;
      this.chkSaturdays.CheckedChanged += new EventHandler(this.chkSaturdays_CheckedChanged);
      this.lblHeader.AutoSize = true;
      this.lblHeader.BackColor = Color.Transparent;
      this.lblHeader.Location = new Point(7, 8);
      this.lblHeader.Name = "lblHeader";
      this.lblHeader.Size = new Size(302, 13);
      this.lblHeader.TabIndex = 2;
      this.lblHeader.Text = "Click a day to exclude it from your company business calendar.";
      this.lblYear.AutoSize = true;
      this.lblYear.BackColor = Color.Transparent;
      this.lblYear.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblYear.Location = new Point(29, 7);
      this.lblYear.Name = "lblYear";
      this.lblYear.Size = new Size(31, 14);
      this.lblYear.TabIndex = 2;
      this.lblYear.Text = "2009";
      this.btnPreviousYear.BackColor = Color.Transparent;
      this.btnPreviousYear.Location = new Point(11, 6);
      this.btnPreviousYear.MouseDownImage = (Image) null;
      this.btnPreviousYear.Name = "btnPreviousYear";
      this.btnPreviousYear.Size = new Size(16, 17);
      this.btnPreviousYear.StandardButtonType = StandardIconButton.ButtonType.MovePreviousButton;
      this.btnPreviousYear.TabIndex = 1;
      this.btnPreviousYear.TabStop = false;
      this.btnPreviousYear.Click += new EventHandler(this.btnPreviousYear_Click);
      this.btnNextYear.BackColor = Color.Transparent;
      this.btnNextYear.Location = new Point(61, 6);
      this.btnNextYear.MouseDownImage = (Image) null;
      this.btnNextYear.Name = "btnNextYear";
      this.btnNextYear.Size = new Size(16, 17);
      this.btnNextYear.StandardButtonType = StandardIconButton.ButtonType.MoveNextButton;
      this.btnNextYear.TabIndex = 0;
      this.btnNextYear.TabStop = false;
      this.btnNextYear.Click += new EventHandler(this.btnNextYear_Click);
      this.tpLockDeskONRP.Controls.Add((Control) this.panel5);
      this.tpLockDeskONRP.Location = new Point(4, 32);
      this.tpLockDeskONRP.Name = "tpLockDeskONRP";
      this.tpLockDeskONRP.Size = new Size(1061, 944);
      this.tpLockDeskONRP.TabIndex = 2;
      this.tpLockDeskONRP.Text = "Lock Desk Schedule / ONRP";
      this.tpLockDeskONRP.UseVisualStyleBackColor = true;
      this.panel5.AutoScroll = true;
      this.panel5.AutoSize = true;
      this.panel5.BorderStyle = BorderStyle.FixedSingle;
      this.panel5.Controls.Add((Control) this.gcMessage);
      this.panel5.Controls.Add((Control) this.gcLockDeskSchedule);
      this.panel5.Controls.Add((Control) this.panel6);
      this.panel5.Dock = DockStyle.Fill;
      this.panel5.Location = new Point(0, 0);
      this.panel5.Name = "panel5";
      this.panel5.Size = new Size(1061, 944);
      this.panel5.TabIndex = 0;
      this.panel5.SizeChanged += new EventHandler(this.panel5_SizeChanged);
      this.gcMessage.Controls.Add((Control) this.btnViewMsg);
      this.gcMessage.Controls.Add((Control) this.txtAddendumMessage);
      this.gcMessage.Controls.Add((Control) this.label29);
      this.gcMessage.Controls.Add((Control) this.txtStandardMessage);
      this.gcMessage.Controls.Add((Control) this.label28);
      this.gcMessage.Dock = DockStyle.Top;
      this.gcMessage.HeaderForeColor = SystemColors.ControlText;
      this.gcMessage.Location = new Point(0, 858);
      this.gcMessage.Name = "gcMessage";
      this.gcMessage.Padding = new Padding(0, 0, 0, 3);
      this.gcMessage.Size = new Size(1043, 199);
      this.gcMessage.TabIndex = 3;
      this.gcMessage.Text = "Overnight Rate Protection Over-Limit Message (sent to requestor)";
      this.btnViewMsg.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnViewMsg.Location = new Point(702, 1);
      this.btnViewMsg.Name = "btnViewMsg";
      this.btnViewMsg.Size = new Size(122, 23);
      this.btnViewMsg.TabIndex = 4;
      this.btnViewMsg.Text = "View Message";
      this.btnViewMsg.UseVisualStyleBackColor = true;
      this.btnViewMsg.Click += new EventHandler(this.btnViewMsg_Click);
      this.txtAddendumMessage.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtAddendumMessage.Location = new Point(33, 132);
      this.txtAddendumMessage.MaxLength = 160;
      this.txtAddendumMessage.Multiline = true;
      this.txtAddendumMessage.Name = "txtAddendumMessage";
      this.txtAddendumMessage.Size = new Size(652, 60);
      this.txtAddendumMessage.TabIndex = 3;
      this.txtAddendumMessage.Text = "You will need to price and submit the lock request during Lock Desk hours.";
      this.txtAddendumMessage.TextChanged += new EventHandler(this.setDirty);
      this.label29.AutoSize = true;
      this.label29.Location = new Point(30, 114);
      this.label29.Name = "label29";
      this.label29.Size = new Size(202, 13);
      this.label29.TabIndex = 2;
      this.label29.Text = "Message Addendum (character limit 160):";
      this.txtStandardMessage.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtStandardMessage.Location = new Point(32, 64);
      this.txtStandardMessage.Multiline = true;
      this.txtStandardMessage.Name = "txtStandardMessage";
      this.txtStandardMessage.Size = new Size(653, 30);
      this.txtStandardMessage.TabIndex = 1;
      this.label28.AutoSize = true;
      this.label28.Location = new Point(30, 45);
      this.label28.Name = "label28";
      this.label28.Size = new Size(96, 13);
      this.label28.TabIndex = 0;
      this.label28.Text = "Standard Message";
      this.gcLockDeskSchedule.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcLockDeskSchedule.Controls.Add((Control) this.correspondentPanel);
      this.gcLockDeskSchedule.Controls.Add((Control) this.tabLockDeskHours);
      this.gcLockDeskSchedule.Controls.Add((Control) this.panel7);
      this.gcLockDeskSchedule.Controls.Add((Control) this.chkEnableEncompassLockDesk);
      this.gcLockDeskSchedule.Controls.Add((Control) this.retailPanel);
      this.gcLockDeskSchedule.Controls.Add((Control) this.wholesalePanel);
      this.gcLockDeskSchedule.Dock = DockStyle.Top;
      this.gcLockDeskSchedule.HeaderForeColor = SystemColors.ControlText;
      this.gcLockDeskSchedule.Location = new Point(0, 159);
      this.gcLockDeskSchedule.Margin = new Padding(0);
      this.gcLockDeskSchedule.Name = "gcLockDeskSchedule";
      this.gcLockDeskSchedule.Size = new Size(1043, 729);
      this.gcLockDeskSchedule.TabIndex = 1;
      this.gcLockDeskSchedule.Tag = (object) "0";
      this.gcLockDeskSchedule.Text = "Lock Desk Hours";
      this.retailPanel.BackColor = Color.White;
      this.retailPanel.Dock = DockStyle.Fill;
      this.retailPanel.Location = new Point(1, 83);
      this.retailPanel.Name = "retailPanel";
      this.retailPanel.Size = new Size(1041, 616);
      this.retailPanel.TabIndex = 2;
      this.wholesalePanel.BackColor = Color.White;
      this.wholesalePanel.Dock = DockStyle.Fill;
      this.wholesalePanel.Location = new Point(1, 83);
      this.wholesalePanel.Name = "wholesalePanel";
      this.wholesalePanel.Size = new Size(1041, 616);
      this.wholesalePanel.TabIndex = 3;
      this.correspondentPanel.BackColor = Color.White;
      this.correspondentPanel.Dock = DockStyle.Fill;
      this.correspondentPanel.Location = new Point(1, 83);
      this.correspondentPanel.Name = "correspondentPanel";
      this.correspondentPanel.Size = new Size(1041, 636);
      this.correspondentPanel.TabIndex = 3;
      this.tabLockDeskHours.Controls.Add((Control) this.tabRetailLockDesk);
      this.tabLockDeskHours.Controls.Add((Control) this.tabWholesaleLockDesk);
      this.tabLockDeskHours.Controls.Add((Control) this.tabCorrespondentLockDesk);
      this.tabLockDeskHours.Dock = DockStyle.Top;
      this.tabLockDeskHours.Location = new Point(1, 62);
      this.tabLockDeskHours.Name = "tabLockDeskHours";
      this.tabLockDeskHours.SelectedIndex = 0;
      this.tabLockDeskHours.Size = new Size(1041, 21);
      this.tabLockDeskHours.TabIndex = 1;
      this.tabLockDeskHours.SelectedIndexChanged += new EventHandler(this.tabLockDeskHours_SelectedIndexChanged);
      this.tabLockDeskHours.Deselected += new TabControlEventHandler(this.tabLockDeskHours_Deselected);
      this.tabLockDeskHours.TabIndexChanged += new EventHandler(this.tabLockDeskHours_TabIndexChanged);
      this.tabRetailLockDesk.Location = new Point(4, 22);
      this.tabRetailLockDesk.Margin = new Padding(0);
      this.tabRetailLockDesk.Name = "tabRetailLockDesk";
      this.tabRetailLockDesk.Size = new Size(1033, 0);
      this.tabRetailLockDesk.TabIndex = 0;
      this.tabRetailLockDesk.Text = "Retail";
      this.tabRetailLockDesk.UseVisualStyleBackColor = true;
      this.tabWholesaleLockDesk.Location = new Point(4, 22);
      this.tabWholesaleLockDesk.Margin = new Padding(0);
      this.tabWholesaleLockDesk.Name = "tabWholesaleLockDesk";
      this.tabWholesaleLockDesk.Size = new Size(1033, 0);
      this.tabWholesaleLockDesk.TabIndex = 1;
      this.tabWholesaleLockDesk.Text = "Wholesale";
      this.tabWholesaleLockDesk.UseVisualStyleBackColor = true;
      this.tabCorrespondentLockDesk.Location = new Point(4, 22);
      this.tabCorrespondentLockDesk.Margin = new Padding(0);
      this.tabCorrespondentLockDesk.Name = "tabCorrespondentLockDesk";
      this.tabCorrespondentLockDesk.Size = new Size(1033, 0);
      this.tabCorrespondentLockDesk.TabIndex = 2;
      this.tabCorrespondentLockDesk.Text = "Correspondent";
      this.tabCorrespondentLockDesk.UseVisualStyleBackColor = true;
      this.panel7.BackColor = Color.WhiteSmoke;
      this.panel7.Controls.Add((Control) this.rdoChannelLockDeskHours);
      this.panel7.Controls.Add((Control) this.rdoCentralLockDeskHours);
      this.panel7.Dock = DockStyle.Top;
      this.panel7.Location = new Point(1, 26);
      this.panel7.Name = "panel7";
      this.panel7.Size = new Size(1041, 36);
      this.panel7.TabIndex = 4;
      this.rdoChannelLockDeskHours.AutoSize = true;
      this.rdoChannelLockDeskHours.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.rdoChannelLockDeskHours.ForeColor = SystemColors.ControlText;
      this.rdoChannelLockDeskHours.Location = new Point(236, 9);
      this.rdoChannelLockDeskHours.Name = "rdoChannelLockDeskHours";
      this.rdoChannelLockDeskHours.Size = new Size(173, 17);
      this.rdoChannelLockDeskHours.TabIndex = 1;
      this.rdoChannelLockDeskHours.TabStop = true;
      this.rdoChannelLockDeskHours.Text = "Channel Lock Desk Hours";
      this.rdoChannelLockDeskHours.UseVisualStyleBackColor = true;
      this.rdoCentralLockDeskHours.AutoSize = true;
      this.rdoCentralLockDeskHours.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.rdoCentralLockDeskHours.Location = new Point(32, 9);
      this.rdoCentralLockDeskHours.Name = "rdoCentralLockDeskHours";
      this.rdoCentralLockDeskHours.Size = new Size(167, 17);
      this.rdoCentralLockDeskHours.TabIndex = 0;
      this.rdoCentralLockDeskHours.TabStop = true;
      this.rdoCentralLockDeskHours.Text = "Central Lock Desk Hours";
      this.rdoCentralLockDeskHours.UseVisualStyleBackColor = true;
      this.rdoCentralLockDeskHours.CheckedChanged += new EventHandler(this.rdoCentralLockDeskHours_CheckedChanged);
      this.chkEnableEncompassLockDesk.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkEnableEncompassLockDesk.AutoSize = true;
      this.chkEnableEncompassLockDesk.BackColor = Color.Transparent;
      this.chkEnableEncompassLockDesk.Location = new Point(668, 3);
      this.chkEnableEncompassLockDesk.Name = "chkEnableEncompassLockDesk";
      this.chkEnableEncompassLockDesk.Size = new Size(213, 17);
      this.chkEnableEncompassLockDesk.TabIndex = 0;
      this.chkEnableEncompassLockDesk.Text = "Enable Encompass Lock Desk Settings";
      this.chkEnableEncompassLockDesk.UseVisualStyleBackColor = false;
      this.panel6.BackColor = Color.WhiteSmoke;
      this.panel6.BorderStyle = BorderStyle.FixedSingle;
      this.panel6.Controls.Add((Control) this.label10);
      this.panel6.Controls.Add((Control) this.label12);
      this.panel6.Controls.Add((Control) this.label14);
      this.panel6.Controls.Add((Control) this.label13);
      this.panel6.Controls.Add((Control) this.label9);
      this.panel6.Controls.Add((Control) this.label8);
      this.panel6.Controls.Add((Control) this.lblONRP);
      this.panel6.Dock = DockStyle.Top;
      this.panel6.Location = new Point(0, 0);
      this.panel6.Name = "panel6";
      this.panel6.Padding = new Padding(0, 0, 0, 3);
      this.panel6.Size = new Size(1043, 159);
      this.panel6.TabIndex = 4;
      this.label10.AutoSize = true;
      this.label10.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label10.Location = new Point(17, 121);
      this.label10.Name = "label10";
      this.label10.Size = new Size(133, 13);
      this.label10.TabIndex = 6;
      this.label10.Text = "End Time as 12:00am,";
      this.label12.AutoSize = true;
      this.label12.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label12.Location = new Point(17, 103);
      this.label12.Name = "label12";
      this.label12.Size = new Size(818, 13);
      this.label12.TabIndex = 5;
      this.label12.Text = "Encompass Lock Desk Hours/ONRP settings. When offering Lock Desk service 24-hours/day, for best results enter both Lock Desk Start and ";
      this.label14.AutoSize = true;
      this.label14.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label14.Location = new Point(17, 85);
      this.label14.Name = "label14";
      this.label14.Size = new Size(754, 13);
      this.label14.TabIndex = 4;
      this.label14.Text = "Please note: By enabling Encompass Lock Desk Settings any Lock Desk settings in TPO WebCenter or ICE PPE will be overridden by ";
      this.label13.AutoSize = true;
      this.label13.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label13.Location = new Point(17, 67);
      this.label13.Name = "label13";
      this.label13.Size = new Size(241, 13);
      this.label13.TabIndex = 3;
      this.label13.Text = "Company Setup > Company Details > ONRP Tab.";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(17, 49);
      this.label9.Name = "label9";
      this.label9.Size = new Size(678, 13);
      this.label9.TabIndex = 2;
      this.label9.Text = "under Company/User Setup > Organization/Users > ONRP section. Individual ONRP settings for specific TPO's are maintained under External ";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(17, 30);
      this.label8.Name = "label8";
      this.label8.Size = new Size(719, 13);
      this.label8.TabIndex = 1;
      this.label8.Text = "defined on the calendar that is selected/setup in the Calendar Tab of this section. Individual ONRP settings for specific Retail Branches are maintained ";
      this.lblONRP.AutoSize = true;
      this.lblONRP.Location = new Point(17, 12);
      this.lblONRP.Name = "lblONRP";
      this.lblONRP.Size = new Size(719, 13);
      this.lblONRP.TabIndex = 0;
      this.lblONRP.Text = "Settings for Lock Desk Hours and Overnight Rate Protection for Retail, Wholesale (TPO Broker), and TPO Correspondent channels. Business Days are";
      this.borderPanel1.Location = new Point(0, 0);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(200, 100);
      this.borderPanel1.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tabCalendar);
      this.Name = nameof (LockExpDateSetup);
      this.Size = new Size(1069, 980);
      this.Scroll += new ScrollEventHandler(this.LockExpDateSetup_Scroll);
      this.tabCalendar.ResumeLayout(false);
      this.tpExpiration.ResumeLayout(false);
      this.tpExpiration.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.groupContainer3.ResumeLayout(false);
      this.panel8.ResumeLayout(false);
      this.panel8.PerformLayout();
      ((ISupportInitialize) this.iconBtnHelpCommitmentTerm).EndInit();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.tpCalendarHour.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.grpCalendar.ResumeLayout(false);
      this.grpCalendar.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      ((ISupportInitialize) this.btnPreviousYear).EndInit();
      ((ISupportInitialize) this.btnNextYear).EndInit();
      this.tpLockDeskONRP.ResumeLayout(false);
      this.tpLockDeskONRP.PerformLayout();
      this.panel5.ResumeLayout(false);
      this.gcMessage.ResumeLayout(false);
      this.gcMessage.PerformLayout();
      this.gcLockDeskSchedule.ResumeLayout(false);
      this.gcLockDeskSchedule.PerformLayout();
      this.tabLockDeskHours.ResumeLayout(false);
      this.panel7.ResumeLayout(false);
      this.panel7.PerformLayout();
      this.panel6.ResumeLayout(false);
      this.panel6.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
