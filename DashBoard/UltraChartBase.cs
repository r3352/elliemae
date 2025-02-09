// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.UltraChartBase
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.ClientSession.Dashboard;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using Infragistics.Win.UltraWinGrid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class UltraChartBase : SnapshotBaseControl
  {
    private const string className = "UltraChartBase";
    private static readonly string sw = Tracing.SwReportControl;
    protected int timeFrameId = -1;
    private UltraChartProvider chartProvider;
    private UltraControlBase drilldownReportProvider;
    protected SortedColumnsCollection ultraGridSortedColumns;
    private IContainer components;
    protected ComboBox cboTimeFrame;
    private GradientPanel gpDVFilter;
    private Label lblSubFilter1;
    private ComboBox cmbFilter1;
    private ComboBox cmbFilter2;
    private Label lblSubFilter2;
    private FlowLayoutPanel flowLayoutPanel1;

    private UltraChartBase()
    {
    }

    public UltraChartBase(
      DashboardForm frmDashboard,
      int snapshotHandle,
      DashboardReport dashboardReport,
      DashboardTemplate dashboardTemplate)
      : this(frmDashboard, snapshotHandle, dashboardReport, dashboardTemplate, dashboardTemplate.ChartType)
    {
    }

    public UltraChartBase(
      DashboardForm frmDashboard,
      int snapshotHandle,
      DashboardReport dashboardReport,
      DashboardTemplate dashboardTemplate,
      DashboardChartType dashboardChartType)
      : base(frmDashboard, snapshotHandle, dashboardReport, dashboardTemplate, dashboardChartType)
    {
      this.InitializeComponent();
      this.setSnapshotProperties();
      this.chartProvider = new UltraChartProvider(frmDashboard, snapshotHandle, dashboardReport, dashboardTemplate, dashboardChartType);
      this.chartProvider.Dock = DockStyle.Fill;
      this.pnlSnapshot.Controls.Add((Control) this.chartProvider);
      this.chartProvider.ChartDataClickedHandler += new UltraControlBase.ChartDataClicked(this.chartProvider_ChartDataClickedHandler);
      this.chartProvider.UpdateZoomInBtnHandler += new UltraControlBase.UpdateZoomInIconButton(((SnapshotBaseControl) this).setZoomInIconButton);
      this.chartProvider.UpdateExcelBtnHandler += new UltraControlBase.UpdateExcelIconButton(((SnapshotBaseControl) this).setExcelIconButton);
      this.chartProvider.UpdatePrintBtnHandler += new UltraControlBase.UpdatePrintIconButton(((SnapshotBaseControl) this).setPrintIconButton);
      if (this.DrillDownTemplate == null)
        return;
      if (this.DrillDownTemplate.ChartType == DashboardChartType.BarChart || this.DrillDownTemplate.ChartType == DashboardChartType.TrendChart)
      {
        this.drilldownReportProvider = (UltraControlBase) new UltraChartProvider(this.frmDashboard, snapshotHandle, dashboardReport, this.DrillDownTemplate, this.DrillDownTemplate.ChartType);
      }
      else
      {
        this.drilldownReportProvider = (UltraControlBase) new UltraGridProvider(this.frmDashboard, snapshotHandle, dashboardReport, this.DrillDownTemplate);
        if (this.DrillDownTemplate.ChartType == DashboardChartType.LoanTable)
          ((UltraGridProvider) this.drilldownReportProvider).ChartDataClickedHandler += new UltraControlBase.ChartDataClicked(((SnapshotBaseControl) this).drilldownProvider_ChartDataClickedHandler);
      }
      this.drilldownReportProvider.Dock = DockStyle.Fill;
      this.gcDrillDownHeader.Controls.Add((Control) this.drilldownReportProvider);
      this.drilldownReportProvider.BringToFront();
    }

    private void relocateTimeFrame()
    {
      using (Graphics graphics = this.lblTitle.CreateGraphics())
      {
        int width = this.lblTitle.Size.Width;
        int num = int.Parse(string.Concat((object) Math.Round((double) graphics.MeasureString(this.lblTitle.Text, this.lblTitle.Font).Width)));
        if (num < width)
          this.cboTimeFrame.Location = new Point(this.lblTitle.Location.X + num + 10, this.cboTimeFrame.Location.Y);
        else
          this.cboTimeFrame.Location = new Point(this.picEdit.Location.X, this.cboTimeFrame.Location.Y);
      }
    }

    protected override void resetDrilldownSnapshot()
    {
      this.gcDrillDownHeader.Controls.Remove((Control) this.drilldownReportProvider);
    }

    protected override void buildDrillDownDashboard()
    {
      this.gcDrillDownHeader.Controls.Remove((Control) this.drilldownReportProvider);
      if (this.DrillDownTemplate != null)
      {
        if (this.DrillDownTemplate.ChartType == DashboardChartType.TrendChart || this.DrillDownTemplate.ChartType == DashboardChartType.BarChart)
        {
          this.drilldownReportProvider = (UltraControlBase) new UltraChartProvider(this.frmDashboard, this.snapshotHandle, this.dashboardReport, this.DrillDownTemplate, this.DrillDownTemplate.ChartType);
        }
        else
        {
          this.drilldownReportProvider = (UltraControlBase) new UltraGridProvider(this.frmDashboard, this.snapshotHandle, this.dashboardReport, this.DrillDownTemplate);
          if (this.DrillDownTemplate.ChartType == DashboardChartType.LoanTable)
            ((UltraGridProvider) this.drilldownReportProvider).ChartDataClickedHandler += new UltraControlBase.ChartDataClicked(((SnapshotBaseControl) this).drilldownProvider_ChartDataClickedHandler);
        }
        this.drilldownReportProvider.Dock = DockStyle.Fill;
        this.gcDrillDownHeader.Controls.Add((Control) this.drilldownReportProvider);
        this.drilldownReportProvider.BringToFront();
        this.refreshDrilldownView();
      }
      else
        this.drilldownReportProvider = (UltraControlBase) null;
    }

    private void chartProvider_ChartDataClickedHandler(string filterValue1, string filterValue2)
    {
      if (this.dashboardTemplate.ChartType == DashboardChartType.LoanTable)
      {
        if (DialogResult.No == Utils.Dialog((IWin32Window) this, "Do you want to open the selected loan file?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
          return;
        string guid = filterValue1;
        ILoanConsole service = Session.Application.GetService<ILoanConsole>();
        if (service.HasOpenLoan)
        {
          if (Session.LoanDataMgr.Writable)
          {
            switch (Utils.Dialog((IWin32Window) this, "Do you want to save the currently opened loan first before opening this loan file?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
            {
              case DialogResult.Yes:
                service.CloseLoanWithoutPrompts(true);
                break;
              case DialogResult.No:
                service.CloseLoanWithoutPrompts(false);
                break;
              default:
                return;
            }
          }
          else
            service.CloseLoanWithoutPrompts(false);
        }
        if (service.OpenLoan(guid, true))
          return;
        int num = (int) Utils.Dialog((IWin32Window) this, "Failed to open the selected loan file.");
      }
      else
      {
        for (int index = 0; index < this.cmbFilter1.Items.Count; ++index)
        {
          if (this.cmbFilter1.Items[index].ToString() == filterValue1)
          {
            this.cmbFilter1.SelectedIndex = index;
            break;
          }
        }
        for (int index = 0; index < this.cmbFilter2.Items.Count; ++index)
        {
          if (this.cmbFilter2.Items[index].ToString() == filterValue2)
          {
            this.cmbFilter2.SelectedIndex = index;
            break;
          }
        }
      }
    }

    protected override void displayModeChanged()
    {
      if (this.currentDisplayMode != DisplayMode.SplitScreen)
        return;
      this.refreshDrilldownView();
    }

    private void refreshDrilldownView()
    {
      if (this.DrillDownTemplate != null && this.currentDisplayMode != DisplayMode.FullReport)
      {
        if (this.chartProvider.NoDataFound())
          this.drilldownReportProvider.RefreshData((DashboardDataCriteria) null);
        else
          this.drilldownReportProvider.RefreshData(this.getReportDataCriteria(this.DrillDownTemplate, true));
      }
      else
      {
        if (this.DrillDownTemplate != null)
          return;
        this.displayDrilldownNotFound();
      }
    }

    protected virtual void setUltraChartCustomProperties()
    {
    }

    protected virtual void setSnapshotProperties()
    {
      this.cboTimeFrame.DataSource = this.dashboardChartType == DashboardChartType.BarChart ? (this.cboTimeFrame.DataSource = SelectionOptions.TimeFrameOptions.Clone()) : (this.cboTimeFrame.DataSource = SelectionOptions.TimePeriodOptions.Clone());
      this.cboTimeFrame.DisplayMember = "Name";
      this.cboTimeFrame.ValueMember = "Id";
      if (1 == this.dashboardReport.ReportParameters.Length)
      {
        this.timeFrameId = int.Parse(this.dashboardReport.ReportParameters[0]);
      }
      else
      {
        this.dashboardReport.ReportParameters = new string[1];
        this.timeFrameId = this.dashboardChartType == DashboardChartType.BarChart ? 9 : 4;
        this.dashboardReport.ReportParameters[0] = this.timeFrameId.ToString();
      }
      this.cboTimeFrame.SelectedValue = (object) this.timeFrameId;
    }

    public override void RefreshData()
    {
      this.chartProvider.RefreshData(this.getReportDataCriteria(this.dashboardTemplate, false));
      this.resetDrilldownViewFilters();
    }

    private void resetDrilldownViewFilters()
    {
      bool flag = false;
      this.lblSubFilter1.Visible = false;
      this.cmbFilter1.Visible = false;
      this.cmbFilter1.Items.Clear();
      this.lblSubFilter2.Visible = false;
      this.cmbFilter2.Visible = false;
      this.cmbFilter2.Items.Clear();
      if (this.chartProvider.Filter1FieldName != "")
      {
        this.lblSubFilter1.Visible = true;
        this.cmbFilter1.Visible = true;
        this.lblSubFilter1.Text = this.chartProvider.Filter1FieldName;
        this.lblSubFilter1.Tag = (object) this.chartProvider.Filter1FieldID;
        if (this.chartProvider.Filter1ValueList != null && this.chartProvider.Filter1ValueList.Length != 0)
        {
          this.cmbFilter1.Items.Add((object) "All");
          this.cmbFilter1.Items.AddRange(this.chartProvider.Filter1ValueList);
          this.cmbFilter1.SelectedIndex = 0;
        }
        else
          flag = true;
      }
      if (this.chartProvider.Filter2FieldName != "" && this.chartProvider.Filter2FieldID.ToLower() != "dashboard.nogroupby")
      {
        this.cmbFilter2.Visible = true;
        this.lblSubFilter2.Visible = true;
        this.lblSubFilter2.Text = this.chartProvider.Filter2FieldName;
        this.lblSubFilter2.Tag = (object) this.chartProvider.Filter2FieldID;
        if (this.chartProvider.Filter2ValueList != null && this.chartProvider.Filter2FieldName.Length > 0)
        {
          this.cmbFilter2.Items.Add((object) "All");
          this.cmbFilter2.Items.AddRange(this.chartProvider.Filter2ValueList);
          this.cmbFilter2.SelectedIndex = 0;
        }
        else
          flag = true;
      }
      this.adjustSubFiltersPosition();
      if (!flag)
        return;
      this.refreshDrilldownView();
    }

    private void adjustSubFiltersPosition()
    {
      int num = (this.Width - 124) / 2;
      using (Graphics graphics = this.lblSubFilter1.CreateGraphics())
      {
        float a = graphics.MeasureString(this.lblSubFilter1.Text, this.lblSubFilter1.Font).Width + 2f;
        if ((double) a > (double) num)
          this.lblSubFilter1.Width = num;
        else
          this.lblSubFilter1.Width = int.Parse(string.Concat((object) Math.Ceiling((double) a)));
      }
      using (Graphics graphics = this.lblSubFilter1.CreateGraphics())
      {
        float a = graphics.MeasureString(this.lblSubFilter2.Text, this.lblSubFilter2.Font).Width + 2f;
        if ((double) a > (double) num)
          this.lblSubFilter2.Width = num;
        else
          this.lblSubFilter2.Width = int.Parse(string.Concat((object) Math.Ceiling((double) a)));
      }
    }

    public override void Zoom(bool zoomIn)
    {
      base.Zoom(zoomIn);
      this.chartProvider.Zoom(zoomIn);
      if (this.drilldownReportProvider != null)
        this.drilldownReportProvider.Zoom(zoomIn);
      this.displayMode(this.currentDisplayMode);
    }

    protected override DashboardDataCriteria getReportDataCriteria(
      DashboardTemplate template,
      bool fromDrillDownView)
    {
      if (fromDrillDownView && template == null)
        return (DashboardDataCriteria) null;
      DashboardDataCriteria reportDataCriteria = base.getReportDataCriteria(template, fromDrillDownView);
      base.getReportDataCriteria(this.dashboardTemplate, false);
      DashboardTimePeriodType timePeriodType = (DashboardTimePeriodType) Enum.Parse(typeof (DashboardTimePeriodType), string.Concat((object) this.timeFrameId));
      if (template.ChartType == DashboardChartType.BarChart)
      {
        if (reportDataCriteria is BarChartDataCriteria chartDataCriteria1)
        {
          chartDataCriteria1.MaxBars = template.MaxBars;
          chartDataCriteria1.SubsetType = template.SubsetType;
          chartDataCriteria1.TimeFrameField = template.TimeFrameField;
          chartDataCriteria1.XAxisField = template.XAxisField;
          chartDataCriteria1.YAxisField = template.YAxisField;
          chartDataCriteria1.YAxisSummaryType = template.YAxisSummaryType;
          chartDataCriteria1.TimeFrameType = (DashboardTimeFrameType) this.timeFrameId;
          QueryCriterion timeFrameCriteria = TimeFrameUtility.GetTimeFrameCriteria(chartDataCriteria1.TimeFrameType, chartDataCriteria1.TimeFrameField);
          chartDataCriteria1.AddQueryCriteria(timeFrameCriteria);
          if (fromDrillDownView)
          {
            BarChartDataCriteria barChart = this.translateToBarChart(reportDataCriteria);
            DateTime startDate;
            DateTime endDate;
            if (this.cmbFilter1.Text != "")
            {
              switch (this.getCriterionFieldType(string.Concat(this.lblSubFilter1.Tag)))
              {
                case FieldTypes.IsDate:
                case FieldTypes.IsMonthDay:
                case FieldTypes.IsDateTime:
                  if (this.cmbFilter1.Text != "All" && this.cmbFilter1.Text != "(Empty)")
                    TimeFrameUtility.GetStartEndDatesForTimePeriod(timePeriodType, this.cmbFilter1.SelectedIndex, out startDate, out endDate);
                  else
                    TimeFrameUtility.GetStartEndDatesForTimePeriod(timePeriodType, 0, out startDate, out endDate);
                  QueryCriterion queryCriterion1 = this.getQueryCriterion(string.Concat(this.lblSubFilter1.Tag), startDate.ToString("MM/dd/yyyy hh:mm:ss tt"), OrdinalMatchType.GreaterThanOrEquals);
                  QueryCriterion queryCriterion2 = this.getQueryCriterion(string.Concat(this.lblSubFilter1.Tag), endDate.ToString("MM/dd/yyyy hh:mm:ss tt"), OrdinalMatchType.LessThanOrEquals);
                  barChart.AddQueryCriteria(queryCriterion1);
                  barChart.AddQueryCriteria(queryCriterion2);
                  break;
                default:
                  if (this.cmbFilter1.Text != "All" && this.cmbFilter1.Text != "(Empty)")
                  {
                    QueryCriterion queryCriterion3 = this.getQueryCriterion(string.Concat(this.lblSubFilter1.Tag), this.cmbFilter1.Text);
                    if (queryCriterion3 != null)
                    {
                      barChart.AddQueryCriteria(queryCriterion3);
                      break;
                    }
                    break;
                  }
                  if (this.cmbFilter1.Text == "(Empty)")
                  {
                    QueryCriterion queryCriterion4 = this.getQueryCriterion(string.Concat(this.lblSubFilter1.Tag), "");
                    if (queryCriterion4 != null)
                    {
                      barChart.AddQueryCriteria(queryCriterion4);
                      break;
                    }
                    break;
                  }
                  break;
              }
            }
            if (this.cmbFilter2.Text != "" && this.cmbFilter2.Text != "All")
            {
              switch (this.getCriterionFieldType(string.Concat(this.lblSubFilter2.Tag)))
              {
                case FieldTypes.IsDate:
                case FieldTypes.IsMonthDay:
                case FieldTypes.IsDateTime:
                  if (this.cmbFilter2.Text != "All" && this.cmbFilter2.Text != "(Empty)")
                    TimeFrameUtility.GetStartEndDatesForTimePeriod(timePeriodType, this.cmbFilter2.SelectedIndex + 1, out startDate, out endDate);
                  else
                    TimeFrameUtility.GetStartEndDatesForTimePeriod(timePeriodType, 0, out startDate, out endDate);
                  QueryCriterion queryCriterion5 = this.getQueryCriterion(string.Concat(this.lblSubFilter2.Tag), startDate.ToString("MM/dd/yyyy hh:mm:ss tt"), OrdinalMatchType.GreaterThanOrEquals);
                  QueryCriterion queryCriterion6 = this.getQueryCriterion(string.Concat(this.lblSubFilter2.Tag), endDate.ToString("MM/dd/yyyy hh:mm:ss tt"), OrdinalMatchType.LessThanOrEquals);
                  barChart.AddQueryCriteria(queryCriterion5);
                  barChart.AddQueryCriteria(queryCriterion6);
                  break;
                default:
                  if (this.cmbFilter2.Text != "All" && this.cmbFilter2.Text != "(Empty)")
                  {
                    QueryCriterion queryCriterion7 = this.getQueryCriterion(string.Concat(this.lblSubFilter2.Tag), this.cmbFilter2.Text);
                    if (queryCriterion7 != null)
                    {
                      barChart.AddQueryCriteria(queryCriterion7);
                      break;
                    }
                    break;
                  }
                  if (this.cmbFilter2.Text == "(Empty)")
                  {
                    QueryCriterion queryCriterion8 = this.getQueryCriterion(string.Concat(this.lblSubFilter2.Tag), "");
                    if (queryCriterion8 != null)
                    {
                      barChart.AddQueryCriteria(queryCriterion8);
                      break;
                    }
                    break;
                  }
                  break;
              }
            }
          }
        }
      }
      else if (DashboardChartType.TrendChart == template.ChartType)
      {
        if (reportDataCriteria is TrendChartDataCriteria chartDataCriteria2)
        {
          chartDataCriteria2.MaxLines = template.MaxLines;
          chartDataCriteria2.SubsetType = template.SubsetType;
          chartDataCriteria2.XAxisField = template.XAxisField;
          chartDataCriteria2.YAxisField = template.YAxisField;
          chartDataCriteria2.YAxisSummaryType = template.YAxisSummaryType;
          chartDataCriteria2.GroupByField = template.GroupByField;
          chartDataCriteria2.TimePeriodType = (DashboardTimePeriodType) this.timeFrameId;
          chartDataCriteria2.TimeUnitType = TimeFrameUtility.GetTimeUnit(chartDataCriteria2.TimePeriodType);
          chartDataCriteria2.TimePeriods = TimeFrameUtility.GetStartEndDatesForTimePeriod(chartDataCriteria2.TimePeriodType, chartDataCriteria2.TimeUnitType);
          chartDataCriteria2.TimePeriodCount = chartDataCriteria2.TimePeriods.Length - 1;
          if (fromDrillDownView)
          {
            TrendChartDataCriteria trendChart = this.translateToTrendChart(reportDataCriteria);
            DateTime startDate;
            DateTime endDate;
            if (this.cmbFilter1.Text != "")
            {
              switch (this.getCriterionFieldType(string.Concat(this.lblSubFilter1.Tag)))
              {
                case FieldTypes.IsDate:
                case FieldTypes.IsMonthDay:
                case FieldTypes.IsDateTime:
                  if (this.cmbFilter1.Text != "All" && this.cmbFilter1.Text != "(Empty)")
                    TimeFrameUtility.GetStartEndDatesForTimePeriod(trendChart.TimePeriodType, this.cmbFilter1.SelectedIndex, out startDate, out endDate);
                  else
                    TimeFrameUtility.GetStartEndDatesForTimePeriod(trendChart.TimePeriodType, 0, out startDate, out endDate);
                  QueryCriterion queryCriterion9 = this.getQueryCriterion(string.Concat(this.lblSubFilter1.Tag), startDate.ToString("MM/dd/yyyy hh:mm:ss tt"), OrdinalMatchType.GreaterThanOrEquals);
                  QueryCriterion queryCriterion10 = this.getQueryCriterion(string.Concat(this.lblSubFilter1.Tag), endDate.ToString("MM/dd/yyyy hh:mm:ss tt"), OrdinalMatchType.LessThanOrEquals);
                  trendChart.AddQueryCriteria(queryCriterion9);
                  trendChart.AddQueryCriteria(queryCriterion10);
                  break;
                default:
                  if (this.cmbFilter1.Text != "All" && this.cmbFilter1.Text != "(Empty)")
                  {
                    QueryCriterion queryCriterion11 = this.getQueryCriterion(string.Concat(this.lblSubFilter1.Tag), this.cmbFilter1.Text);
                    if (queryCriterion11 != null)
                    {
                      trendChart.AddQueryCriteria(queryCriterion11);
                      break;
                    }
                    break;
                  }
                  if (this.cmbFilter1.Text == "(Empty)")
                  {
                    QueryCriterion queryCriterion12 = this.getQueryCriterion(string.Concat(this.lblSubFilter1.Tag), "");
                    if (queryCriterion12 != null)
                    {
                      trendChart.AddQueryCriteria(queryCriterion12);
                      break;
                    }
                    break;
                  }
                  break;
              }
            }
            if (this.cmbFilter2.Text != "")
            {
              switch (this.getCriterionFieldType(string.Concat(this.lblSubFilter2.Tag)))
              {
                case FieldTypes.IsDate:
                case FieldTypes.IsMonthDay:
                case FieldTypes.IsDateTime:
                  if (this.cmbFilter2.Text != "All" && this.cmbFilter2.Text != "(Empty)")
                    TimeFrameUtility.GetStartEndDatesForTimePeriod(trendChart.TimePeriodType, this.cmbFilter2.SelectedIndex + 1, out startDate, out endDate);
                  else
                    TimeFrameUtility.GetStartEndDatesForTimePeriod(trendChart.TimePeriodType, 0, out startDate, out endDate);
                  QueryCriterion queryCriterion13 = this.getQueryCriterion(string.Concat(this.lblSubFilter2.Tag), startDate.ToString("MM/dd/yyyy hh:mm:ss tt"), OrdinalMatchType.GreaterThanOrEquals);
                  QueryCriterion queryCriterion14 = this.getQueryCriterion(string.Concat(this.lblSubFilter2.Tag), endDate.ToString("MM/dd/yyyy hh:mm:ss tt"), OrdinalMatchType.LessThanOrEquals);
                  trendChart.AddQueryCriteria(queryCriterion13);
                  trendChart.AddQueryCriteria(queryCriterion14);
                  break;
                default:
                  if (this.cmbFilter2.Text != "All" && this.cmbFilter2.Text != "(Empty)")
                  {
                    QueryCriterion queryCriterion15 = this.getQueryCriterion(string.Concat(this.lblSubFilter2.Tag), this.cmbFilter2.Text);
                    if (queryCriterion15 != null)
                    {
                      trendChart.AddQueryCriteria(queryCriterion15);
                      break;
                    }
                    break;
                  }
                  if (this.cmbFilter2.Text == "(Empty)")
                  {
                    QueryCriterion queryCriterion16 = this.getQueryCriterion(string.Concat(this.lblSubFilter2.Tag), "");
                    if (queryCriterion16 != null)
                    {
                      trendChart.AddQueryCriteria(queryCriterion16);
                      break;
                    }
                    break;
                  }
                  break;
              }
            }
          }
        }
      }
      else if (DashboardChartType.LoanTable == template.ChartType)
      {
        if (reportDataCriteria is LoanTableDataCriteria loanTableCriteria)
        {
          loanTableCriteria.MaxRows = template.MaxRows;
          loanTableCriteria.TimeFrameField = template.TimeFrameField;
          if (0 < template.Fields.Count)
          {
            loanTableCriteria.FieldCriterionNames = this.getFieldCriterionNames(template.Fields);
            loanTableCriteria.SortFields = this.getSortfields(loanTableCriteria);
          }
          loanTableCriteria.TimeFrameType = (DashboardTimeFrameType) this.timeFrameId;
          QueryCriterion timeFrameCriteria = TimeFrameUtility.GetTimeFrameCriteria(loanTableCriteria.TimeFrameType, loanTableCriteria.TimeFrameField);
          loanTableCriteria.AddQueryCriteria(timeFrameCriteria);
          if (fromDrillDownView)
          {
            LoanTableDataCriteria loanTableChart = this.translateToLoanTableChart(reportDataCriteria);
            DateTime startDate;
            DateTime endDate;
            if (this.cmbFilter1.Text != "")
            {
              switch (this.getCriterionFieldType(string.Concat(this.lblSubFilter1.Tag)))
              {
                case FieldTypes.IsDate:
                case FieldTypes.IsMonthDay:
                case FieldTypes.IsDateTime:
                  if (this.cmbFilter1.Text != "All" && this.cmbFilter1.Text != "(Empty)")
                    TimeFrameUtility.GetStartEndDatesForTimePeriod(timePeriodType, this.cmbFilter1.SelectedIndex, out startDate, out endDate);
                  else
                    TimeFrameUtility.GetStartEndDatesForTimePeriod(timePeriodType, 0, out startDate, out endDate);
                  QueryCriterion queryCriterion17 = this.getQueryCriterion(string.Concat(this.lblSubFilter1.Tag), startDate.ToString("MM/dd/yyyy hh:mm:ss tt"), OrdinalMatchType.GreaterThanOrEquals);
                  QueryCriterion queryCriterion18 = this.getQueryCriterion(string.Concat(this.lblSubFilter1.Tag), endDate.ToString("MM/dd/yyyy hh:mm:ss tt"), OrdinalMatchType.LessThanOrEquals);
                  loanTableChart.AddQueryCriteria(queryCriterion17);
                  loanTableChart.AddQueryCriteria(queryCriterion18);
                  break;
                default:
                  if (this.cmbFilter1.Text != "All" && this.cmbFilter1.Text != "(Empty)")
                  {
                    QueryCriterion queryCriterion19 = this.getQueryCriterion(string.Concat(this.lblSubFilter1.Tag), this.cmbFilter1.Text);
                    if (queryCriterion19 != null)
                    {
                      loanTableChart.AddQueryCriteria(queryCriterion19);
                      break;
                    }
                    break;
                  }
                  if (this.cmbFilter1.Text == "(Empty)")
                  {
                    QueryCriterion queryCriterion20 = this.getQueryCriterion(string.Concat(this.lblSubFilter1.Tag), "");
                    if (queryCriterion20 != null)
                    {
                      loanTableChart.AddQueryCriteria(queryCriterion20);
                      break;
                    }
                    break;
                  }
                  break;
              }
            }
            if (this.cmbFilter2.Text != "")
            {
              switch (this.getCriterionFieldType(string.Concat(this.lblSubFilter2.Tag)))
              {
                case FieldTypes.IsDate:
                case FieldTypes.IsMonthDay:
                case FieldTypes.IsDateTime:
                  if (this.cmbFilter2.Text != "All" && this.cmbFilter2.Text != "(Empty)")
                    TimeFrameUtility.GetStartEndDatesForTimePeriod(timePeriodType, this.cmbFilter2.SelectedIndex + 1, out startDate, out endDate);
                  else
                    TimeFrameUtility.GetStartEndDatesForTimePeriod(timePeriodType, 0, out startDate, out endDate);
                  QueryCriterion queryCriterion21 = this.getQueryCriterion(string.Concat(this.lblSubFilter2.Tag), startDate.ToString("MM/dd/yyyy hh:mm:ss tt"), OrdinalMatchType.GreaterThanOrEquals);
                  QueryCriterion queryCriterion22 = this.getQueryCriterion(string.Concat(this.lblSubFilter2.Tag), endDate.ToString("MM/dd/yyyy hh:mm:ss tt"), OrdinalMatchType.LessThanOrEquals);
                  loanTableChart.AddQueryCriteria(queryCriterion21);
                  loanTableChart.AddQueryCriteria(queryCriterion22);
                  break;
                default:
                  if (this.cmbFilter2.Text != "All" && this.cmbFilter2.Text != "(Empty)")
                  {
                    QueryCriterion queryCriterion23 = this.getQueryCriterion(string.Concat(this.lblSubFilter2.Tag), this.cmbFilter2.Text);
                    if (queryCriterion23 != null)
                    {
                      loanTableChart.AddQueryCriteria(queryCriterion23);
                      break;
                    }
                    break;
                  }
                  if (this.cmbFilter2.Text == "(Empty)")
                  {
                    QueryCriterion queryCriterion24 = this.getQueryCriterion(string.Concat(this.lblSubFilter2.Tag), "");
                    if (queryCriterion24 != null)
                    {
                      loanTableChart.AddQueryCriteria(queryCriterion24);
                      break;
                    }
                    break;
                  }
                  break;
              }
            }
          }
        }
      }
      else if (reportDataCriteria is UserTableDataCriteria tableDataCriteria)
      {
        tableDataCriteria.TimeFrameField = template.TimeFrameField;
        tableDataCriteria.RoleId = template.RoleId;
        tableDataCriteria.OrganizationId = template.OrganizationId;
        tableDataCriteria.IncludeChildren = template.IncludeChildren;
        tableDataCriteria.UserGroupId = template.UserGroupId;
        tableDataCriteria.GroupByField = template.GroupByField;
        tableDataCriteria.SummaryField1 = template.SummaryField1;
        tableDataCriteria.SummaryField2 = template.SummaryField2;
        tableDataCriteria.SummaryField3 = template.SummaryField3;
        tableDataCriteria.SummaryType1 = template.SummaryType1;
        tableDataCriteria.SummaryType2 = template.SummaryType2;
        tableDataCriteria.SummaryType3 = template.SummaryType3;
        tableDataCriteria.TimeFrameType = (DashboardTimeFrameType) this.timeFrameId;
        QueryCriterion timeFrameCriteria = TimeFrameUtility.GetTimeFrameCriteria(tableDataCriteria.TimeFrameType, tableDataCriteria.TimeFrameField);
        tableDataCriteria.AddQueryCriteria(timeFrameCriteria);
        if (fromDrillDownView)
        {
          UserTableDataCriteria userTableChart = this.translateToUserTableChart(reportDataCriteria);
          DateTime startDate;
          DateTime endDate;
          if (this.cmbFilter1.Text != "")
          {
            switch (this.getCriterionFieldType(string.Concat(this.lblSubFilter1.Tag)))
            {
              case FieldTypes.IsDate:
              case FieldTypes.IsMonthDay:
              case FieldTypes.IsDateTime:
                if (this.cmbFilter1.Text != "All" && this.cmbFilter1.Text != "(Empty)")
                  TimeFrameUtility.GetStartEndDatesForTimePeriod(timePeriodType, this.cmbFilter1.SelectedIndex, out startDate, out endDate);
                else
                  TimeFrameUtility.GetStartEndDatesForTimePeriod(timePeriodType, 0, out startDate, out endDate);
                QueryCriterion queryCriterion25 = this.getQueryCriterion(string.Concat(this.lblSubFilter1.Tag), startDate.ToString("MM/dd/yyyy hh:mm:ss tt"), OrdinalMatchType.GreaterThanOrEquals);
                QueryCriterion queryCriterion26 = this.getQueryCriterion(string.Concat(this.lblSubFilter1.Tag), endDate.ToString("MM/dd/yyyy hh:mm:ss tt"), OrdinalMatchType.LessThanOrEquals);
                userTableChart.AddQueryCriteria(queryCriterion25);
                userTableChart.AddQueryCriteria(queryCriterion26);
                break;
              default:
                if (this.cmbFilter1.Text != "All" && this.cmbFilter1.Text != "(Empty)")
                {
                  QueryCriterion queryCriterion27 = this.getQueryCriterion(string.Concat(this.lblSubFilter1.Tag), this.cmbFilter1.Text);
                  if (queryCriterion27 != null)
                  {
                    userTableChart.AddQueryCriteria(queryCriterion27);
                    break;
                  }
                  break;
                }
                if (this.cmbFilter1.Text == "(Empty)")
                {
                  QueryCriterion queryCriterion28 = this.getQueryCriterion(string.Concat(this.lblSubFilter1.Tag), "");
                  if (queryCriterion28 != null)
                  {
                    userTableChart.AddQueryCriteria(queryCriterion28);
                    break;
                  }
                  break;
                }
                break;
            }
          }
          if (this.cmbFilter2.Text != "")
          {
            switch (this.getCriterionFieldType(string.Concat(this.lblSubFilter2.Tag)))
            {
              case FieldTypes.IsDate:
              case FieldTypes.IsMonthDay:
              case FieldTypes.IsDateTime:
                if (this.cmbFilter2.Text != "All" && this.cmbFilter2.Text != "(Empty)")
                  TimeFrameUtility.GetStartEndDatesForTimePeriod(timePeriodType, this.cmbFilter2.SelectedIndex + 1, out startDate, out endDate);
                else
                  TimeFrameUtility.GetStartEndDatesForTimePeriod(timePeriodType, 0, out startDate, out endDate);
                QueryCriterion queryCriterion29 = this.getQueryCriterion(string.Concat(this.lblSubFilter2.Tag), startDate.ToString("MM/dd/yyyy hh:mm:ss tt"), OrdinalMatchType.GreaterThanOrEquals);
                QueryCriterion queryCriterion30 = this.getQueryCriterion(string.Concat(this.lblSubFilter2.Tag), endDate.ToString("MM/dd/yyyy hh:mm:ss tt"), OrdinalMatchType.LessThanOrEquals);
                userTableChart.AddQueryCriteria(queryCriterion29);
                userTableChart.AddQueryCriteria(queryCriterion30);
                break;
              default:
                if (this.cmbFilter2.Text != "All" && this.cmbFilter2.Text != "(Empty)")
                {
                  QueryCriterion queryCriterion31 = this.getQueryCriterion(string.Concat(this.lblSubFilter2.Tag), this.cmbFilter2.Text);
                  if (queryCriterion31 != null)
                  {
                    userTableChart.AddQueryCriteria(queryCriterion31);
                    break;
                  }
                  break;
                }
                if (this.cmbFilter2.Text == "(Empty)")
                {
                  QueryCriterion queryCriterion32 = this.getQueryCriterion(string.Concat(this.lblSubFilter2.Tag), "");
                  if (queryCriterion32 != null)
                  {
                    userTableChart.AddQueryCriteria(queryCriterion32);
                    break;
                  }
                  break;
                }
                break;
            }
          }
        }
      }
      return reportDataCriteria;
    }

    private TrendChartDataCriteria translateToTrendChart(DashboardDataCriteria drilldownView)
    {
      drilldownView.FolderNames = !this.dashboardTemplate.SelectAllFolders ? (!this.dashboardTemplate.SelectAllWOArchiveFolders ? this.dashboardTemplate.Folders : this.AllButArchiveFolders) : this.AllFolders;
      TrendChartDataCriteria criteria = drilldownView as TrendChartDataCriteria;
      criteria.ClearQueryCriteria();
      criteria.FieldFilters.Clear();
      if (this.dashboardTemplate.ChartType == DashboardChartType.BarChart)
        criteria.TimePeriodType = TimeFrameUtility.GetTimePeriodTypeFromTimeFrameType((DashboardTimeFrameType) this.timeFrameId);
      else if (this.dashboardTemplate.ChartType == DashboardChartType.TrendChart)
        criteria.TimePeriodType = (DashboardTimePeriodType) this.timeFrameId;
      if (this.dashboardTemplate.ChartType == DashboardChartType.TrendChart)
      {
        switch (TimeFrameUtility.GetTimeUnit((DashboardTimePeriodType) this.timeFrameId))
        {
          case DashboardTimeUnitType.Day:
            criteria.TimeUnitType = DashboardTimeUnitType.Day;
            break;
          case DashboardTimeUnitType.Week:
            criteria.TimeUnitType = DashboardTimeUnitType.Day;
            break;
          case DashboardTimeUnitType.Month:
            criteria.TimeUnitType = DashboardTimeUnitType.Week;
            break;
          case DashboardTimeUnitType.Quarter:
            criteria.TimeUnitType = DashboardTimeUnitType.Week;
            break;
        }
        DateTime startDate;
        DateTime endDate;
        if (this.cmbFilter1.Text != "All")
          TimeFrameUtility.GetStartEndDatesForTimePeriod(criteria.TimePeriodType, this.cmbFilter1.SelectedIndex, out startDate, out endDate);
        else
          TimeFrameUtility.GetStartEndDatesForTimePeriod(criteria.TimePeriodType, 0, out startDate, out endDate);
        criteria.TimePeriods = TimeFrameUtility.GetStartEndDatesForTimePeriod(criteria.TimeUnitType, startDate, endDate);
      }
      else
      {
        criteria.TimeUnitType = TimeFrameUtility.GetTimeUnit(criteria.TimePeriodType);
        criteria.TimePeriods = TimeFrameUtility.GetStartEndDatesForTimePeriod(criteria.TimePeriodType, criteria.TimeUnitType);
      }
      criteria.TimePeriodCount = criteria.TimePeriods.Length - 1;
      this.setViewCriteria((DashboardDataCriteria) criteria);
      return criteria;
    }

    private BarChartDataCriteria translateToBarChart(DashboardDataCriteria drilldownView)
    {
      drilldownView.FolderNames = !this.dashboardTemplate.SelectAllFolders ? (!this.dashboardTemplate.SelectAllWOArchiveFolders ? this.dashboardTemplate.Folders : this.AllButArchiveFolders) : this.AllFolders;
      BarChartDataCriteria criteria = drilldownView as BarChartDataCriteria;
      criteria.ClearQueryCriteria();
      criteria.FieldFilters.Clear();
      string timeFrameField = criteria.TimeFrameField;
      if (this.dashboardTemplate.ChartType == DashboardChartType.TrendChart)
      {
        criteria.TimeFrameType = DashboardTimeFrameType.None;
      }
      else
      {
        criteria.TimeFrameType = (DashboardTimeFrameType) this.timeFrameId;
        criteria.TimeFrameField = this.dashboardTemplate.TimeFrameField;
      }
      QueryCriterion timeFrameCriteria = TimeFrameUtility.GetTimeFrameCriteria(criteria.TimeFrameType, criteria.TimeFrameField);
      criteria.AddQueryCriteria(timeFrameCriteria);
      this.setViewCriteria((DashboardDataCriteria) criteria);
      return criteria;
    }

    private UserTableDataCriteria translateToUserTableChart(DashboardDataCriteria drilldownView)
    {
      drilldownView.FolderNames = !this.dashboardTemplate.SelectAllFolders ? (!this.dashboardTemplate.SelectAllWOArchiveFolders ? this.dashboardTemplate.Folders : this.AllButArchiveFolders) : this.AllFolders;
      UserTableDataCriteria criteria = drilldownView as UserTableDataCriteria;
      criteria.ClearQueryCriteria();
      criteria.FieldFilters.Clear();
      string timeFrameField = criteria.TimeFrameField;
      if (this.dashboardTemplate.ChartType == DashboardChartType.TrendChart)
      {
        criteria.TimeFrameType = DashboardTimeFrameType.None;
      }
      else
      {
        criteria.TimeFrameType = (DashboardTimeFrameType) this.timeFrameId;
        criteria.TimeFrameField = this.dashboardTemplate.TimeFrameField;
      }
      QueryCriterion timeFrameCriteria = TimeFrameUtility.GetTimeFrameCriteria(criteria.TimeFrameType, criteria.TimeFrameField);
      criteria.AddQueryCriteria(timeFrameCriteria);
      this.setViewCriteria((DashboardDataCriteria) criteria);
      return criteria;
    }

    private LoanTableDataCriteria translateToLoanTableChart(DashboardDataCriteria drilldownView)
    {
      drilldownView.FolderNames = !this.dashboardTemplate.SelectAllFolders ? (!this.dashboardTemplate.SelectAllWOArchiveFolders ? this.dashboardTemplate.Folders : this.AllButArchiveFolders) : this.AllFolders;
      LoanTableDataCriteria criteria = drilldownView as LoanTableDataCriteria;
      criteria.ClearQueryCriteria();
      criteria.FieldFilters = this.dashboardTemplate.Filters;
      if (-2 != this.dashboardTemplate.RoleId)
        criteria.AddQueryCriteria(this.getQueryCriterion("loanassociate.RoleID", string.Concat((object) this.dashboardTemplate.RoleId)));
      if (-1 != this.dashboardTemplate.OrganizationId)
        criteria.AddQueryCriteria(this.getQueryCriterion("currentassociateuser.org_id", string.Concat((object) this.dashboardTemplate.OrganizationId)));
      else if (-1 != this.dashboardTemplate.UserGroupId)
        criteria.AddQueryCriteria(this.getQueryCriterion("associategroup.GroupID", string.Concat((object) this.dashboardTemplate.UserGroupId)));
      string timeFrameField = criteria.TimeFrameField;
      if (this.dashboardTemplate.ChartType == DashboardChartType.TrendChart)
      {
        criteria.TimeFrameType = DashboardTimeFrameType.None;
      }
      else
      {
        criteria.TimeFrameType = (DashboardTimeFrameType) this.timeFrameId;
        criteria.TimeFrameField = this.dashboardTemplate.TimeFrameField;
      }
      QueryCriterion timeFrameCriteria = TimeFrameUtility.GetTimeFrameCriteria(criteria.TimeFrameType, criteria.TimeFrameField);
      criteria.AddQueryCriteria(timeFrameCriteria);
      this.setViewCriteria((DashboardDataCriteria) criteria);
      return criteria;
    }

    private SortField[] getSortfields(LoanTableDataCriteria loanTableCriteria)
    {
      Dictionary<string, SortField> dictionary = new Dictionary<string, SortField>();
      if (this.ultraGridSortedColumns != null && 0 < this.ultraGridSortedColumns.Count)
      {
        foreach (UltraGridColumn gridSortedColumn in (IEnumerable) this.ultraGridSortedColumns)
        {
          if (gridSortedColumn.SortIndicator != SortIndicator.None && SortIndicator.Disabled != gridSortedColumn.SortIndicator)
          {
            ColumnInfo columnInfo = (ColumnInfo) null;
            foreach (ColumnInfo field in this.dashboardTemplate.Fields)
            {
              if (field.Description == gridSortedColumn.Key)
              {
                columnInfo = field;
                break;
              }
            }
            if (columnInfo != null)
            {
              SortField sortField = new SortField(columnInfo.CriterionName, SortIndicator.Ascending == gridSortedColumn.SortIndicator ? FieldSortOrder.Ascending : FieldSortOrder.Descending);
              dictionary.Add(columnInfo.CriterionName, sortField);
            }
          }
        }
      }
      foreach (ColumnInfo field in this.dashboardTemplate.Fields)
      {
        if (field.SortOrder != ColumnSortOrder.None && !dictionary.ContainsKey(field.CriterionName))
        {
          SortField sortField = new SortField(field.CriterionName, ColumnSortOrder.Ascending == field.SortOrder ? FieldSortOrder.Ascending : FieldSortOrder.Descending);
          dictionary.Add(field.CriterionName, sortField);
        }
      }
      SortField[] array = new SortField[dictionary.Values.Count];
      dictionary.Values.CopyTo(array, 0);
      return array;
    }

    protected override void ExportToExcel() => this.chartProvider.ExportToExcel();

    protected override void PrintSnapshot() => this.chartProvider.PrintSnapshot();

    public override void SetPrintProperties()
    {
      DateTime startDate;
      DateTime endDate;
      if (this.dashboardTemplate.ChartType == DashboardChartType.BarChart)
        TimeFrameUtility.GetStartEndDatesForTimeFrame((DashboardTimeFrameType) this.timeFrameId, out startDate, out endDate);
      else
        TimeFrameUtility.GetStartEndDatesForTimePeriod((DashboardTimePeriodType) this.timeFrameId, 0, out startDate, out endDate);
      string str = "Date Range: " + startDate.ToShortDateString() + " through " + endDate.ToShortDateString();
      this.chartProvider.TitleTop.Text = this.GetPrintTitle() + "\n" + str;
      this.chartProvider.TitleTop.Visible = true;
      this.chartProvider.Legend.Visible = true;
    }

    public override void ResetPrintProperties()
    {
      this.chartProvider.TitleTop.Visible = false;
      this.chartProvider.Legend.Visible = 1 == this.frmDashboard.CurrentView.LayoutBlockCount || this.picZoomOut.Visible;
    }

    protected virtual void cboTimeFrame_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (this.cboTimeFrame.SelectedValue == null || this.timeFrameId == (int) this.cboTimeFrame.SelectedValue)
        return;
      this.timeFrameId = (int) this.cboTimeFrame.SelectedValue;
      this.dashboardReport.ReportParameters[0] = this.timeFrameId.ToString();
      this.frmDashboard.IsViewModified = true;
      this.RefreshData();
    }

    private void drillDownView_FilterChanged(object sender, EventArgs e)
    {
      this.refreshDrilldownView();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cboTimeFrame = new ComboBox();
      this.gpDVFilter = new GradientPanel();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.lblSubFilter1 = new Label();
      this.cmbFilter1 = new ComboBox();
      this.lblSubFilter2 = new Label();
      this.cmbFilter2 = new ComboBox();
      ((ISupportInitialize) this.picZoomOut).BeginInit();
      this.gcDrillDownHeader.SuspendLayout();
      this.pnlHeader.SuspendLayout();
      this.pnlSnapshot.SuspendLayout();
      ((ISupportInitialize) this.picEdit).BeginInit();
      this.gpDVFilter.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.lblTitle.Size = new Size(343, 13);
      this.lblNoDataFound.Location = new Point(7, 67);
      this.lblNoDataFound.Size = new Size(576, 33);
      this.gcDrillDownHeader.Controls.Add((Control) this.gpDVFilter);
      this.gcDrillDownHeader.Controls.SetChildIndex((Control) this.lblDrilldownMsg, 0);
      this.gcDrillDownHeader.Controls.SetChildIndex((Control) this.gpDVFilter, 0);
      this.lblDrilldownMsg.Location = new Point(10, 88);
      this.lblDrilldownMsg.Size = new Size(573, 33);
      this.pnlHeader.Controls.Add((Control) this.cboTimeFrame);
      this.pnlHeader.Controls.SetChildIndex((Control) this.picEdit, 0);
      this.pnlHeader.Controls.SetChildIndex((Control) this.picZoomOut, 0);
      this.pnlHeader.Controls.SetChildIndex((Control) this.lblTitle, 0);
      this.pnlHeader.Controls.SetChildIndex((Control) this.cboTimeFrame, 0);
      this.pnlSnapshot.Size = new Size(592, 173);
      this.cboTimeFrame.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cboTimeFrame.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTimeFrame.FormattingEnabled = true;
      this.cboTimeFrame.Items.AddRange(new object[10]
      {
        (object) "Current Week",
        (object) "Current Month",
        (object) "Current Year",
        (object) "Previous Week",
        (object) "Previous Month",
        (object) "Previous Year",
        (object) "Last 7 Days",
        (object) "Last 30 Days",
        (object) "Last 90 Days",
        (object) "Last 365 Days"
      });
      this.cboTimeFrame.Location = new Point(370, 3);
      this.cboTimeFrame.Name = "cboTimeFrame";
      this.cboTimeFrame.Size = new Size(105, 21);
      this.cboTimeFrame.TabIndex = 13;
      this.cboTimeFrame.SelectionChangeCommitted += new EventHandler(this.cboTimeFrame_SelectionChangeCommitted);
      this.gpDVFilter.Borders = AnchorStyles.Bottom;
      this.gpDVFilter.Controls.Add((Control) this.flowLayoutPanel1);
      this.gpDVFilter.Dock = DockStyle.Top;
      this.gpDVFilter.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gpDVFilter.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gpDVFilter.Location = new Point(0, 26);
      this.gpDVFilter.Name = "gpDVFilter";
      this.gpDVFilter.Size = new Size(592, 31);
      this.gpDVFilter.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gpDVFilter.TabIndex = 0;
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.lblSubFilter1);
      this.flowLayoutPanel1.Controls.Add((Control) this.cmbFilter1);
      this.flowLayoutPanel1.Controls.Add((Control) this.lblSubFilter2);
      this.flowLayoutPanel1.Controls.Add((Control) this.cmbFilter2);
      this.flowLayoutPanel1.Location = new Point(6, 5);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(583, 22);
      this.flowLayoutPanel1.TabIndex = 4;
      this.lblSubFilter1.AutoEllipsis = true;
      this.lblSubFilter1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblSubFilter1.Location = new Point(0, 4);
      this.lblSubFilter1.Margin = new Padding(0, 4, 0, 0);
      this.lblSubFilter1.Name = "lblSubFilter1";
      this.lblSubFilter1.Size = new Size(36, 14);
      this.lblSubFilter1.TabIndex = 0;
      this.lblSubFilter1.Text = "Filter1";
      this.cmbFilter1.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbFilter1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cmbFilter1.FormattingEnabled = true;
      this.cmbFilter1.Location = new Point(41, 0);
      this.cmbFilter1.Margin = new Padding(5, 0, 5, 0);
      this.cmbFilter1.Name = "cmbFilter1";
      this.cmbFilter1.Size = new Size(93, 22);
      this.cmbFilter1.TabIndex = 1;
      this.cmbFilter1.SelectedIndexChanged += new EventHandler(this.drillDownView_FilterChanged);
      this.lblSubFilter2.AutoEllipsis = true;
      this.lblSubFilter2.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblSubFilter2.Location = new Point(139, 4);
      this.lblSubFilter2.Margin = new Padding(0, 4, 0, 0);
      this.lblSubFilter2.Name = "lblSubFilter2";
      this.lblSubFilter2.Size = new Size(36, 14);
      this.lblSubFilter2.TabIndex = 2;
      this.lblSubFilter2.Text = "Filter2";
      this.cmbFilter2.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbFilter2.FormattingEnabled = true;
      this.cmbFilter2.Location = new Point(180, 0);
      this.cmbFilter2.Margin = new Padding(5, 0, 0, 0);
      this.cmbFilter2.Name = "cmbFilter2";
      this.cmbFilter2.Size = new Size(93, 21);
      this.cmbFilter2.TabIndex = 3;
      this.cmbFilter2.SelectedIndexChanged += new EventHandler(this.drillDownView_FilterChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.Name = nameof (UltraChartBase);
      ((ISupportInitialize) this.picZoomOut).EndInit();
      this.gcDrillDownHeader.ResumeLayout(false);
      this.pnlHeader.ResumeLayout(false);
      this.pnlSnapshot.ResumeLayout(false);
      ((ISupportInitialize) this.picEdit).EndInit();
      this.gpDVFilter.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
