// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.BusinessCalendarPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Calendar;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class BusinessCalendarPanel : SettingsUserControl
  {
    private MonthNavigator[] months;
    private int currentYear = -1;
    private BusinessCalendar businessCalendar;
    private BusinessCalendar postalCalendar;
    private BusinessCalendar regZCalendar;
    private bool suspendEvents;
    private IContainer components;
    private GroupContainer grpCalendar;
    private Label lblYear;
    private StandardIconButton btnPreviousYear;
    private StandardIconButton btnNextYear;
    private GradientPanel gradientPanel1;
    private CheckBox chkSundays;
    private CheckBox chkSaturdays;
    private MonthNavigator monthFeb;
    private MonthNavigator monthJan;
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
    private RadioButton radBusiness;
    private RadioButton radPostal;
    private Label lblHeader;
    private RadioButton radRegz;

    public BusinessCalendarPanel(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
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
      this.Reset();
    }

    public override void Reset()
    {
      if (this.currentYear == -1)
        this.currentYear = DateTime.Today.Year;
      this.businessCalendar = Session.ConfigurationManager.GetBusinessCalendar(CalendarType.Business, new DateTime(this.currentYear, 1, 1), new DateTime(this.currentYear, 12, 31));
      this.postalCalendar = Session.ConfigurationManager.GetBusinessCalendar(CalendarType.Postal, new DateTime(this.currentYear, 1, 1), new DateTime(this.currentYear, 12, 31));
      this.regZCalendar = Session.ConfigurationManager.GetBusinessCalendar(CalendarType.RegZ, new DateTime(this.currentYear, 1, 1), new DateTime(this.currentYear, 12, 31));
      this.loadCalendar();
      base.Reset();
    }

    private void loadCalendar()
    {
      this.suspendEvents = true;
      this.setCurrentYear(this.currentYear);
      this.chkSaturdays.Checked = (this.currentCalendar.WorkDays & DaysOfWeek.Saturday) == DaysOfWeek.None;
      this.chkSundays.Checked = (this.currentCalendar.WorkDays & DaysOfWeek.Sunday) == DaysOfWeek.None;
      this.chkSaturdays.Visible = this.radBusiness.Checked;
      this.chkSundays.Visible = this.radBusiness.Checked;
      this.setupContainer.UpdatesetupDialogSubTitle("The Reg-Z Business Day Calendar will be used for calculating LE && CD receipt dates. The Company Calendar will be used for calculating LE && CD due dates. The Postal Calendar is provided for informational purposes with the postal holidays being included in the Company Calendar by default.");
      if (this.radPostal.Checked)
        this.lblHeader.Text = "The U.S. Postal Calendar excludes Sundays and legal holidays.";
      else if (this.radRegz.Checked)
        this.lblHeader.Text = "The Reg-Z Business Day Calendar excludes Sundays and legal holidays.";
      else
        this.lblHeader.Text = "Click a day to exclude it from your company business calendar.";
      this.suspendEvents = false;
    }

    private BusinessCalendar currentCalendar
    {
      get
      {
        if (this.radPostal.Checked)
          return this.postalCalendar;
        return this.radRegz.Checked ? this.regZCalendar : this.businessCalendar;
      }
    }

    private CalendarType currentCalendarType
    {
      get
      {
        if (this.radPostal.Checked)
          return CalendarType.Postal;
        return this.radRegz.Checked ? CalendarType.RegZ : CalendarType.Business;
      }
    }

    public override void Save()
    {
      using (CursorActivator.Wait())
        Session.ConfigurationManager.SaveBusinessCalendar(this.businessCalendar);
      base.Save();
    }

    private void onDateClicked(object sender, DateEventArgs e)
    {
      if (this.radPostal.Checked || this.radRegz.Checked)
        return;
      switch (this.businessCalendar.GetDayType(e.Date))
      {
        case CalendarDayType.BusinessDay:
          this.businessCalendar.SetDayType(e.Date, CalendarDayType.Holiday);
          this.months[e.Date.Month - 1].SetDateEffects(e.Date, DateEffects.Highlight);
          this.setDirtyFlag(true);
          break;
        case CalendarDayType.Holiday:
          this.businessCalendar.SetDayType(e.Date, CalendarDayType.BusinessDay);
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
        if (this.currentCalendar.GetDayType(date) != CalendarDayType.BusinessDay)
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
        this.currentCalendar.WorkDays &= ~DaysOfWeek.Saturday;
      else
        this.currentCalendar.WorkDays |= DaysOfWeek.Saturday;
      this.setDirtyFlag(true);
      this.setCurrentYear(this.currentYear);
    }

    private void chkSundays_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents)
        return;
      if (this.chkSundays.Checked)
        this.currentCalendar.WorkDays &= ~DaysOfWeek.Sunday;
      else
        this.currentCalendar.WorkDays |= DaysOfWeek.Sunday;
      this.setDirtyFlag(true);
      this.setCurrentYear(this.currentYear);
    }

    private void ensureYearLoaded(int year)
    {
      DateTime dateTime = new DateTime(year, 1, 1);
      if (this.currentCalendar.Contains(dateTime))
        return;
      this.currentCalendar.Merge(!(dateTime < this.currentCalendar.StartDate) ? Session.ConfigurationManager.GetBusinessCalendar(this.currentCalendarType, this.currentCalendar.EndDate.AddDays(1.0), new DateTime(year, 12, 31)) : Session.ConfigurationManager.GetBusinessCalendar(this.currentCalendarType, dateTime, this.currentCalendar.StartDate.AddDays(-1.0)));
    }

    private void radPostal_CheckedChanged(object sender, EventArgs e) => this.loadCalendar();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.grpCalendar = new GroupContainer();
      this.radRegz = new RadioButton();
      this.radBusiness = new RadioButton();
      this.radPostal = new RadioButton();
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
      this.grpCalendar.SuspendLayout();
      this.panel1.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnPreviousYear).BeginInit();
      ((ISupportInitialize) this.btnNextYear).BeginInit();
      this.SuspendLayout();
      this.grpCalendar.Controls.Add((Control) this.radRegz);
      this.grpCalendar.Controls.Add((Control) this.radBusiness);
      this.grpCalendar.Controls.Add((Control) this.radPostal);
      this.grpCalendar.Controls.Add((Control) this.panel1);
      this.grpCalendar.Controls.Add((Control) this.gradientPanel1);
      this.grpCalendar.Controls.Add((Control) this.lblYear);
      this.grpCalendar.Controls.Add((Control) this.btnPreviousYear);
      this.grpCalendar.Controls.Add((Control) this.btnNextYear);
      this.grpCalendar.Dock = DockStyle.Fill;
      this.grpCalendar.HeaderForeColor = SystemColors.ControlText;
      this.grpCalendar.Location = new Point(0, 0);
      this.grpCalendar.Name = "grpCalendar";
      this.grpCalendar.Size = new Size(732, 576);
      this.grpCalendar.TabIndex = 0;
      this.radRegz.AutoSize = true;
      this.radRegz.BackColor = Color.Transparent;
      this.radRegz.Checked = true;
      this.radRegz.Location = new Point(5, 4);
      this.radRegz.Name = "radRegz";
      this.radRegz.Size = new Size(171, 18);
      this.radRegz.TabIndex = 9;
      this.radRegz.TabStop = true;
      this.radRegz.Text = "Reg-Z Business Day Calendar";
      this.radRegz.UseVisualStyleBackColor = false;
      this.radRegz.CheckedChanged += new EventHandler(this.radPostal_CheckedChanged);
      this.radBusiness.AutoSize = true;
      this.radBusiness.BackColor = Color.Transparent;
      this.radBusiness.Location = new Point(308, 4);
      this.radBusiness.Name = "radBusiness";
      this.radBusiness.Size = new Size(137, 18);
      this.radBusiness.TabIndex = 8;
      this.radBusiness.Text = "Our Company Calendar";
      this.radBusiness.UseVisualStyleBackColor = false;
      this.radBusiness.CheckedChanged += new EventHandler(this.radPostal_CheckedChanged);
      this.radPostal.AutoSize = true;
      this.radPostal.BackColor = Color.Transparent;
      this.radPostal.Location = new Point(180, 4);
      this.radPostal.Name = "radPostal";
      this.radPostal.Size = new Size(123, 18);
      this.radPostal.TabIndex = 7;
      this.radPostal.Text = "U.S. Postal Calendar";
      this.radPostal.UseVisualStyleBackColor = false;
      this.radPostal.CheckedChanged += new EventHandler(this.radPostal_CheckedChanged);
      this.panel1.AutoScroll = true;
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
      this.panel1.Size = new Size(730, 518);
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
      this.gradientPanel1.Borders = AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.chkSundays);
      this.gradientPanel1.Controls.Add((Control) this.chkSaturdays);
      this.gradientPanel1.Controls.Add((Control) this.lblHeader);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(1, 26);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(730, 31);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel1.TabIndex = 3;
      this.chkSundays.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkSundays.AutoSize = true;
      this.chkSundays.BackColor = Color.Transparent;
      this.chkSundays.Location = new Point(618, 7);
      this.chkSundays.Name = "chkSundays";
      this.chkSundays.Size = new Size(110, 18);
      this.chkSundays.TabIndex = 1;
      this.chkSundays.Text = "Exclude Sundays";
      this.chkSundays.UseVisualStyleBackColor = false;
      this.chkSundays.CheckedChanged += new EventHandler(this.chkSundays_CheckedChanged);
      this.chkSaturdays.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkSaturdays.AutoSize = true;
      this.chkSaturdays.BackColor = Color.Transparent;
      this.chkSaturdays.Location = new Point(499, 7);
      this.chkSaturdays.Name = "chkSaturdays";
      this.chkSaturdays.Size = new Size(117, 18);
      this.chkSaturdays.TabIndex = 0;
      this.chkSaturdays.Text = "Exclude Saturdays";
      this.chkSaturdays.UseVisualStyleBackColor = false;
      this.chkSaturdays.CheckedChanged += new EventHandler(this.chkSaturdays_CheckedChanged);
      this.lblHeader.AutoSize = true;
      this.lblHeader.BackColor = Color.Transparent;
      this.lblHeader.Location = new Point(7, 8);
      this.lblHeader.Name = "lblHeader";
      this.lblHeader.Size = new Size(311, 14);
      this.lblHeader.TabIndex = 2;
      this.lblHeader.Text = "Click a day to exclude it from your company business calendar.";
      this.lblYear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblYear.AutoSize = true;
      this.lblYear.BackColor = Color.Transparent;
      this.lblYear.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblYear.Location = new Point(675, 6);
      this.lblYear.Name = "lblYear";
      this.lblYear.Size = new Size(31, 14);
      this.lblYear.TabIndex = 2;
      this.lblYear.Text = "2009";
      this.btnPreviousYear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPreviousYear.BackColor = Color.Transparent;
      this.btnPreviousYear.Location = new Point(656, 5);
      this.btnPreviousYear.MouseDownImage = (Image) null;
      this.btnPreviousYear.Name = "btnPreviousYear";
      this.btnPreviousYear.Size = new Size(16, 17);
      this.btnPreviousYear.StandardButtonType = StandardIconButton.ButtonType.MovePreviousButton;
      this.btnPreviousYear.TabIndex = 1;
      this.btnPreviousYear.TabStop = false;
      this.btnPreviousYear.Click += new EventHandler(this.btnPreviousYear_Click);
      this.btnNextYear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNextYear.BackColor = Color.Transparent;
      this.btnNextYear.Location = new Point(708, 5);
      this.btnNextYear.MouseDownImage = (Image) null;
      this.btnNextYear.Name = "btnNextYear";
      this.btnNextYear.Size = new Size(16, 17);
      this.btnNextYear.StandardButtonType = StandardIconButton.ButtonType.MoveNextButton;
      this.btnNextYear.TabIndex = 0;
      this.btnNextYear.TabStop = false;
      this.btnNextYear.Click += new EventHandler(this.btnNextYear_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpCalendar);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (BusinessCalendarPanel);
      this.Size = new Size(732, 576);
      this.grpCalendar.ResumeLayout(false);
      this.grpCalendar.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      ((ISupportInitialize) this.btnPreviousYear).EndInit();
      ((ISupportInitialize) this.btnNextYear).EndInit();
      this.ResumeLayout(false);
    }
  }
}
