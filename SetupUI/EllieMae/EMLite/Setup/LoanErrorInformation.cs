// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanErrorInformation
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.LoanUtils.Services;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanErrorInformation : SettingsUserControl
  {
    private Sessions.Session session;
    private GroupContainer gcCurrentLoanErrors;
    private GridView loanErrorView;
    private IContainer components;
    private PageListNavigator navigator;
    private int currentOffsetIntoErrors;
    private EBSServiceClient client;
    private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private Label label1;
    private List<LoanErrorEntry> totalLoanErrorsForQuery;
    private GroupContainer groupContainer1;
    private Button btnClear;
    private TextBox loanGuidFilterBox;
    private Label label2;
    private Button btnSearch;
    private TextBox borrowerFirstNameFilterBox;
    private Label lblObject;
    private Panel panel1;
    private Label label3;
    private TextBox borrowerLastNameFilterBox;
    private Label label4;
    private Label label5;
    private ComboBox cmbDateRange;
    private TextBox endDateBox;
    private Label endDateLabel;
    private TextBox startDateBox;
    private Label startDateLabel;
    private CalendarButton endDateCalendar;
    private CalendarButton startDateCalendar;
    private Label label6;
    private Label label8;
    private Label label7;

    public LoanErrorInformation(SetUpContainer setupContainer, Sessions.Session session)
      : base(setupContainer)
    {
      this.setupContainer = setupContainer;
      this.session = session;
      try
      {
        this.client = new EBSServiceClient(this.session.StartupInfo.ServerInstanceName, this.session.StartupInfo.SessionID, this.session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(this.session.SessionObjects));
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error occured while connecting to the server: " + ex.Message, "Encompass");
      }
      this.InitializeComponent();
      this.initDateRangeComboBox();
      this.btnSearch_Click((object) null, (EventArgs) null);
      this.navigator.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.eventPageChanged);
      this.startDateCalendar.DateSelected += new CalendarPopupEventHandler(this.startDateSelectedFromCalendar);
      this.endDateCalendar.DateSelected += new CalendarPopupEventHandler(this.endDateSelectedFromCalendar);
      this.loanErrorView.SortItems += new GVColumnSortEventHandler(this.loanErrorView_SortItems);
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
    }

    private void populatePage(List<LoanErrorEntry> errors)
    {
      this.loanErrorView.Items.Clear();
      foreach (LoanErrorEntry error in errors)
      {
        try
        {
          this.loanErrorView.Items.Add(new GVItem()
          {
            SubItems = {
              (object) this.jsonDateFormatToDateTime(error.createdDate),
              (object) this.nullToEmpty(error.elliApplication),
              (object) this.nullToEmpty(error.userId),
              (object) this.nullToEmpty(error.borrowerFirstName),
              (object) this.nullToEmpty(error.borrowerLastName),
              (object) this.nullToEmpty(error.siteId),
              (object) this.nullToEmpty(error.loanId),
              (object) this.nullToEmpty(error.busRule),
              (object) this.nullToEmpty(error.fieldId),
              (object) this.nullToEmpty(error.id),
              (object) new LoanErrorDetailsLink(this.nullToEmpty(error.summary), this.nullToEmpty(error.stacktrace), this.nullToEmpty(error.errorType), this.nullToEmpty(error.source), this.nullToEmpty(error.code), this.nullToEmpty(error.id))
            }
          });
        }
        catch (Exception ex)
        {
        }
      }
    }

    private string nullToEmpty(string str) => str != null ? str : "";

    private DateTime jsonDateFormatToDateTime(string date)
    {
      Match match = Regex.Match(date, "\\d+");
      DateTime dateTime = LoanErrorInformation.epoch;
      dateTime = dateTime.AddMilliseconds((double) long.Parse(match.Value));
      return dateTime.ToLocalTime();
    }

    private void refreshLoanErrorData()
    {
      this.totalLoanErrorsForQuery = this.setParametersAndMakeCall();
      this.currentOffsetIntoErrors = 0;
      this.navigator.NumberOfItems = this.totalLoanErrorsForQuery.Count;
    }

    private List<LoanErrorEntry> setParametersAndMakeCall()
    {
      string firstName = this.borrowerFirstNameFilterBox.Text.Trim();
      string lastName = this.borrowerLastNameFilterBox.Text.Trim();
      string loanGuid = this.loanGuidFilterBox.Text.Trim();
      DateTime dateTime1;
      DateTime dateTime2;
      if (this.selectedToday())
      {
        dateTime1 = DateTime.Today;
        dateTime2 = dateTime1.AddDays(2.0);
      }
      else
      {
        dateTime1 = Convert.ToDateTime(this.startDateBox.Text);
        dateTime2 = Convert.ToDateTime(this.endDateBox.Text);
      }
      return this.client.GetCCLoanErrors(firstName, lastName, loanGuid, dateTime1.ToString("yyyy-MM-dd"), dateTime2.ToString("yyyy-MM-dd")).Result;
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      this.loanGuidFilterBox.Clear();
      this.borrowerFirstNameFilterBox.Clear();
      this.borrowerLastNameFilterBox.Clear();
      this.startDateBox.Clear();
      this.endDateBox.Clear();
    }

    private void populateAtOffset(int offset)
    {
      this.populatePage(this.totalLoanErrorsForQuery.GetRange(offset, Math.Min(50, this.totalLoanErrorsForQuery.Count - offset)));
    }

    private void eventPageChanged(object sender, PageChangedEventArgs e)
    {
      this.navigator.PageChangedEvent -= new PageListNavigator.PageChangedEventHandler(this.eventPageChanged);
      if (e.ItemIndex != -1)
      {
        this.currentOffsetIntoErrors = e.ItemIndex;
        this.populateAtOffset(this.currentOffsetIntoErrors);
      }
      this.navigator.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.eventPageChanged);
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
      if (!this.validateDates() || !this.validateLoanGuid())
        return;
      this.btnSearch.Enabled = false;
      try
      {
        this.navigator.ClearSelection();
        this.loanErrorView.ClearSort();
        this.refreshLoanErrorData();
        this.populateAtOffset(0);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error occured while retrieving loan error data: " + ex.Message + "InnerException : " + (object) ex.InnerException, "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        this.btnSearch.Enabled = true;
      }
    }

    private bool validateLoanGuid()
    {
      if (this.loanGuidFilterBox.Text.Trim() == "")
        return true;
      try
      {
        Guid.Parse(this.loanGuidFilterBox.Text.Trim());
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Loan GUID must be in the format XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
    }

    private bool validateDates()
    {
      if (this.selectedToday())
        return true;
      if (!(this.startDateBox.Text.Trim() == ""))
      {
        if (!(this.endDateBox.Text.Trim() == ""))
        {
          DateTime dateTime1;
          try
          {
            dateTime1 = Convert.ToDateTime(this.startDateBox.Text);
          }
          catch
          {
            int num = (int) MessageBox.Show("Start date has an invalid format.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
          }
          DateTime dateTime2;
          try
          {
            dateTime2 = Convert.ToDateTime(this.endDateBox.Text);
          }
          catch
          {
            int num = (int) MessageBox.Show("End date has an invalid format.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
          }
          if (dateTime1 > dateTime2)
          {
            int num = (int) MessageBox.Show("End date must be after the start date.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
          }
          if (!(dateTime2.Subtract(dateTime1) > new TimeSpan(90, 0, 0, 0)))
            return true;
          int num1 = (int) MessageBox.Show("The date range cannot be longer than 90 days.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
      }
      int num2 = (int) MessageBox.Show("Please enter a start date and end date.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private void initDateRangeComboBox()
    {
      this.cmbDateRange.SelectedIndexChanged += new EventHandler(this.dateRangeChanged);
      this.cmbDateRange.Items.Clear();
      this.cmbDateRange.Items.Add((object) "Today");
      this.cmbDateRange.Items.Add((object) "Range");
      this.cmbDateRange.SelectedIndex = 0;
    }

    private void dateRangeChanged(object sender, EventArgs e)
    {
      if (this.selectedToday())
      {
        this.startDateBox.Clear();
        this.endDateBox.Clear();
        this.toggleDateRangeUI(false);
      }
      else
        this.toggleDateRangeUI(true);
    }

    private void toggleDateRangeUI(bool show)
    {
      this.endDateBox.Visible = show;
      this.endDateLabel.Visible = show;
      this.startDateBox.Visible = show;
      this.startDateLabel.Visible = show;
      this.startDateCalendar.Visible = show;
      this.endDateCalendar.Visible = show;
      this.label6.Visible = show;
      this.label7.Visible = show;
      this.label8.Visible = show;
    }

    private bool selectedToday() => this.cmbDateRange.SelectedIndex == 0;

    private void startDateSelectedFromCalendar(object sender, CalendarPopupEventArgs e)
    {
      this.startDateBox.Text = e.Date.ToShortDateString();
    }

    private void endDateSelectedFromCalendar(object sender, CalendarPopupEventArgs e)
    {
      this.endDateBox.Text = e.Date.ToShortDateString();
    }

    private void loanErrorView_SortItems(object sender, GVColumnSortEventArgs e)
    {
      LoanErrorInformation.LoanErrorEntryToField[] funcs = new LoanErrorInformation.LoanErrorEntryToField[11]
      {
        (LoanErrorInformation.LoanErrorEntryToField) (entry => entry.createdDate),
        (LoanErrorInformation.LoanErrorEntryToField) (entry => entry.elliApplication),
        (LoanErrorInformation.LoanErrorEntryToField) (entry => entry.userId),
        (LoanErrorInformation.LoanErrorEntryToField) (entry => entry.borrowerFirstName),
        (LoanErrorInformation.LoanErrorEntryToField) (entry => entry.borrowerLastName),
        (LoanErrorInformation.LoanErrorEntryToField) (entry => entry.siteId),
        (LoanErrorInformation.LoanErrorEntryToField) (entry => entry.loanId),
        (LoanErrorInformation.LoanErrorEntryToField) (entry => entry.busRule),
        (LoanErrorInformation.LoanErrorEntryToField) (entry => entry.fieldId),
        (LoanErrorInformation.LoanErrorEntryToField) (entry => entry.id),
        (LoanErrorInformation.LoanErrorEntryToField) (entry => entry.errorType)
      };
      int col = e.Column;
      LoanErrorInformation.LoanErrorEntryToField projection = (LoanErrorInformation.LoanErrorEntryToField) (x => this.nullToEmpty(funcs[col](x)));
      if (col == 0)
        this.totalLoanErrorsForQuery.Sort((Comparison<LoanErrorEntry>) ((x, y) => this.jsonDateFormatToDateTime(projection(x)).CompareTo(this.jsonDateFormatToDateTime(projection(y)))));
      else
        this.totalLoanErrorsForQuery.Sort((Comparison<LoanErrorEntry>) ((x, y) => projection(x).CompareTo(projection(y))));
      if (e.SortOrder == SortOrder.Descending)
        this.totalLoanErrorsForQuery.Reverse();
      this.populateAtOffset(this.currentOffsetIntoErrors);
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      JedHelp.ShowHelp("Setup\\Loan Error Information");
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
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoanErrorInformation));
      this.groupContainer1 = new GroupContainer();
      this.label3 = new Label();
      this.gcCurrentLoanErrors = new GroupContainer();
      this.loanErrorView = new GridView();
      this.label1 = new Label();
      this.navigator = new PageListNavigator();
      this.panel1 = new Panel();
      this.label8 = new Label();
      this.label7 = new Label();
      this.label6 = new Label();
      this.endDateCalendar = new CalendarButton();
      this.startDateCalendar = new CalendarButton();
      this.endDateBox = new TextBox();
      this.endDateLabel = new Label();
      this.startDateBox = new TextBox();
      this.startDateLabel = new Label();
      this.label5 = new Label();
      this.cmbDateRange = new ComboBox();
      this.borrowerLastNameFilterBox = new TextBox();
      this.label4 = new Label();
      this.btnClear = new Button();
      this.loanGuidFilterBox = new TextBox();
      this.label2 = new Label();
      this.btnSearch = new Button();
      this.borrowerFirstNameFilterBox = new TextBox();
      this.lblObject = new Label();
      this.groupContainer1.SuspendLayout();
      this.gcCurrentLoanErrors.SuspendLayout();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.endDateCalendar).BeginInit();
      ((ISupportInitialize) this.startDateCalendar).BeginInit();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.gcCurrentLoanErrors);
      this.groupContainer1.Controls.Add((Control) this.panel1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(1144, 673);
      this.groupContainer1.TabIndex = 13;
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(5, 6);
      this.label3.Name = "label3";
      this.label3.Size = new Size(45, 14);
      this.label3.TabIndex = 29;
      this.label3.Text = "Search";
      this.gcCurrentLoanErrors.Controls.Add((Control) this.loanErrorView);
      this.gcCurrentLoanErrors.Controls.Add((Control) this.label1);
      this.gcCurrentLoanErrors.Controls.Add((Control) this.navigator);
      this.gcCurrentLoanErrors.Dock = DockStyle.Fill;
      this.gcCurrentLoanErrors.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gcCurrentLoanErrors.HeaderForeColor = SystemColors.ControlText;
      this.gcCurrentLoanErrors.Location = new Point(1, 113);
      this.gcCurrentLoanErrors.Name = "gcCurrentLoanErrors";
      this.gcCurrentLoanErrors.Size = new Size(1142, 559);
      this.gcCurrentLoanErrors.TabIndex = 12;
      this.loanErrorView.AllowColumnReorder = true;
      this.loanErrorView.AllowDrop = true;
      this.loanErrorView.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.DateTime;
      gvColumn1.Tag = (object) "Date/Time";
      gvColumn1.Text = "Date/Time";
      gvColumn1.Width = 140;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column3";
      gvColumn2.Tag = (object) "Application";
      gvColumn2.Text = "Application";
      gvColumn2.Width = 120;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column0";
      gvColumn3.Tag = (object) "User ID";
      gvColumn3.Text = "User ID";
      gvColumn3.Width = 80;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column5";
      gvColumn4.Tag = (object) "Borrower First Name";
      gvColumn4.Text = "Borrower First Name";
      gvColumn4.Width = 120;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column12";
      gvColumn5.Tag = (object) "Borrower Last Name";
      gvColumn5.Text = "Borrower Last Name";
      gvColumn5.Width = 120;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Tag = (object) "Consumer Connect Site ID";
      gvColumn6.Text = "Consumer Connect Site ID";
      gvColumn6.Width = 145;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.Tag = (object) "Loan GUID";
      gvColumn7.Text = "Loan GUID";
      gvColumn7.Width = 100;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column14";
      gvColumn8.Tag = (object) "Business Rule";
      gvColumn8.Text = "Business Rule";
      gvColumn8.Width = 140;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column8";
      gvColumn9.Tag = (object) "FieldID";
      gvColumn9.Text = "FieldID";
      gvColumn9.Width = 80;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column10";
      gvColumn10.Tag = (object) "Error ID";
      gvColumn10.Text = "Error ID";
      gvColumn10.Width = 100;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column11";
      gvColumn11.Tag = (object) "Error Details";
      gvColumn11.Text = "Error Details";
      gvColumn11.Width = 100;
      this.loanErrorView.Columns.AddRange(new GVColumn[11]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9,
        gvColumn10,
        gvColumn11
      });
      this.loanErrorView.Cursor = Cursors.Default;
      this.loanErrorView.Dock = DockStyle.Fill;
      this.loanErrorView.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.loanErrorView.Location = new Point(1, 26);
      this.loanErrorView.Name = "loanErrorView";
      this.loanErrorView.Size = new Size(1140, 508);
      this.loanErrorView.SortOption = GVSortOption.Owner;
      this.loanErrorView.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(4, 4);
      this.label1.Name = "label1";
      this.label1.Size = new Size(132, 14);
      this.label1.TabIndex = 28;
      this.label1.Text = "List of Loan File Errors";
      this.navigator.BackColor = Color.Transparent;
      this.navigator.Dock = DockStyle.Bottom;
      this.navigator.Font = new Font("Arial", 8f);
      this.navigator.Location = new Point(1, 534);
      this.navigator.Name = "navigator";
      this.navigator.NumberOfItems = 0;
      this.navigator.Size = new Size(1140, 24);
      this.navigator.TabIndex = 5;
      this.panel1.Controls.Add((Control) this.label8);
      this.panel1.Controls.Add((Control) this.label7);
      this.panel1.Controls.Add((Control) this.label6);
      this.panel1.Controls.Add((Control) this.endDateCalendar);
      this.panel1.Controls.Add((Control) this.startDateCalendar);
      this.panel1.Controls.Add((Control) this.endDateBox);
      this.panel1.Controls.Add((Control) this.endDateLabel);
      this.panel1.Controls.Add((Control) this.startDateBox);
      this.panel1.Controls.Add((Control) this.startDateLabel);
      this.panel1.Controls.Add((Control) this.label5);
      this.panel1.Controls.Add((Control) this.cmbDateRange);
      this.panel1.Controls.Add((Control) this.borrowerLastNameFilterBox);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.btnClear);
      this.panel1.Controls.Add((Control) this.loanGuidFilterBox);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.btnSearch);
      this.panel1.Controls.Add((Control) this.borrowerFirstNameFilterBox);
      this.panel1.Controls.Add((Control) this.lblObject);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(1, 26);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(1142, 87);
      this.panel1.TabIndex = 22;
      this.label8.AutoSize = true;
      this.label8.Font = new Font("Microsoft Sans Serif", 9f);
      this.label8.ForeColor = Color.Red;
      this.label8.Location = new Point(872, 7);
      this.label8.Name = "label8";
      this.label8.Size = new Size(12, 15);
      this.label8.TabIndex = 53;
      this.label8.Text = "*";
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Microsoft Sans Serif", 9f);
      this.label7.ForeColor = Color.Red;
      this.label7.Location = new Point(711, 7);
      this.label7.Name = "label7";
      this.label7.Size = new Size(12, 15);
      this.label7.TabIndex = 52;
      this.label7.Text = "*";
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Italic, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(826, 49);
      this.label6.Name = "label6";
      this.label6.Size = new Size(97, 12);
      this.label6.TabIndex = 51;
      this.label6.Text = "(Maximum of 90 days)";
      ((IconButton) this.endDateCalendar).Image = (Image) componentResourceManager.GetObject("endDateCalendar.Image");
      this.endDateCalendar.Location = new Point(963, 30);
      this.endDateCalendar.MouseDownImage = (Image) null;
      this.endDateCalendar.Name = "endDateCalendar";
      this.endDateCalendar.Size = new Size(16, 16);
      this.endDateCalendar.SizeMode = PictureBoxSizeMode.AutoSize;
      this.endDateCalendar.TabIndex = 50;
      this.endDateCalendar.TabStop = false;
      this.endDateCalendar.Visible = false;
      ((IconButton) this.startDateCalendar).Image = (Image) componentResourceManager.GetObject("startDateCalendar.Image");
      this.startDateCalendar.Location = new Point(797, 29);
      this.startDateCalendar.MouseDownImage = (Image) null;
      this.startDateCalendar.Name = "startDateCalendar";
      this.startDateCalendar.Size = new Size(16, 16);
      this.startDateCalendar.SizeMode = PictureBoxSizeMode.AutoSize;
      this.startDateCalendar.TabIndex = 49;
      this.startDateCalendar.TabStop = false;
      this.startDateCalendar.Visible = false;
      this.endDateBox.Font = new Font("Arial", 8.25f);
      this.endDateBox.Location = new Point(828, 26);
      this.endDateBox.Name = "endDateBox";
      this.endDateBox.Size = new Size(129, 20);
      this.endDateBox.TabIndex = 29;
      this.endDateLabel.AutoSize = true;
      this.endDateLabel.Font = new Font("Arial", 8.25f);
      this.endDateLabel.Location = new Point(825, 8);
      this.endDateLabel.Name = "endDateLabel";
      this.endDateLabel.Size = new Size(50, 14);
      this.endDateLabel.TabIndex = 28;
      this.endDateLabel.Text = "End Date";
      this.startDateBox.Font = new Font("Arial", 8.25f);
      this.startDateBox.Location = new Point(662, 25);
      this.startDateBox.Name = "startDateBox";
      this.startDateBox.Size = new Size(129, 20);
      this.startDateBox.TabIndex = 27;
      this.startDateLabel.AutoSize = true;
      this.startDateLabel.Font = new Font("Arial", 8.25f);
      this.startDateLabel.Location = new Point(659, 7);
      this.startDateLabel.Name = "startDateLabel";
      this.startDateLabel.Size = new Size(55, 14);
      this.startDateLabel.TabIndex = 26;
      this.startDateLabel.Text = "Start Date";
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Arial", 8.25f);
      this.label5.Location = new Point(524, 8);
      this.label5.Name = "label5";
      this.label5.Size = new Size(97, 14);
      this.label5.TabIndex = 25;
      this.label5.Text = "Error Created Date";
      this.cmbDateRange.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbDateRange.ForeColor = SystemColors.WindowText;
      this.cmbDateRange.FormattingEnabled = true;
      this.cmbDateRange.Location = new Point(527, 25);
      this.cmbDateRange.Name = "cmbDateRange";
      this.cmbDateRange.Size = new Size(129, 21);
      this.cmbDateRange.TabIndex = 24;
      this.borrowerLastNameFilterBox.Font = new Font("Arial", 8.25f);
      this.borrowerLastNameFilterBox.Location = new Point(142, 26);
      this.borrowerLastNameFilterBox.Name = "borrowerLastNameFilterBox";
      this.borrowerLastNameFilterBox.Size = new Size(129, 20);
      this.borrowerLastNameFilterBox.TabIndex = 23;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Arial", 8.25f);
      this.label4.Location = new Point(139, 8);
      this.label4.Name = "label4";
      this.label4.Size = new Size(58, 14);
      this.label4.TabIndex = 22;
      this.label4.Text = "Last Name";
      this.btnClear.BackColor = SystemColors.Control;
      this.btnClear.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnClear.Location = new Point(527, 55);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new Size(129, 21);
      this.btnClear.TabIndex = 21;
      this.btnClear.Text = "Clear";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new EventHandler(this.btnClear_Click);
      this.loanGuidFilterBox.Font = new Font("Arial", 8.25f);
      this.loanGuidFilterBox.Location = new Point(277, 26);
      this.loanGuidFilterBox.Name = "loanGuidFilterBox";
      this.loanGuidFilterBox.Size = new Size(244, 20);
      this.loanGuidFilterBox.TabIndex = 20;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f);
      this.label2.Location = new Point(274, 8);
      this.label2.Name = "label2";
      this.label2.Size = new Size(58, 14);
      this.label2.TabIndex = 19;
      this.label2.Text = "Loan GUID";
      this.btnSearch.BackColor = SystemColors.Control;
      this.btnSearch.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnSearch.Location = new Point(392, 55);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new Size(129, 21);
      this.btnSearch.TabIndex = 18;
      this.btnSearch.Text = "Search";
      this.btnSearch.UseVisualStyleBackColor = true;
      this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
      this.borrowerFirstNameFilterBox.Font = new Font("Arial", 8.25f);
      this.borrowerFirstNameFilterBox.Location = new Point(7, 26);
      this.borrowerFirstNameFilterBox.Name = "borrowerFirstNameFilterBox";
      this.borrowerFirstNameFilterBox.Size = new Size(129, 20);
      this.borrowerFirstNameFilterBox.TabIndex = 17;
      this.lblObject.AutoSize = true;
      this.lblObject.Font = new Font("Arial", 8.25f);
      this.lblObject.Location = new Point(4, 8);
      this.lblObject.Name = "lblObject";
      this.lblObject.Size = new Size(58, 14);
      this.lblObject.TabIndex = 16;
      this.lblObject.Text = "First Name";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (LoanErrorInformation);
      this.Size = new Size(1144, 673);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.gcCurrentLoanErrors.ResumeLayout(false);
      this.gcCurrentLoanErrors.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.endDateCalendar).EndInit();
      ((ISupportInitialize) this.startDateCalendar).EndInit();
      this.ResumeLayout(false);
    }

    private delegate string LoanErrorEntryToField(LoanErrorEntry e);
  }
}
