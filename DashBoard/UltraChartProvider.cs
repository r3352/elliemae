// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.UltraChartProvider
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.ClientSession.Dashboard;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using Infragistics.UltraChart.Resources;
using Infragistics.UltraChart.Resources.Appearance;
using Infragistics.UltraChart.Shared.Events;
using Infragistics.UltraChart.Shared.Styles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class UltraChartProvider : UltraControlBase
  {
    private const string className = "UltraChartProvider";
    private static readonly string sw = Tracing.SwReportControl;
    protected DataTable ultraChartDataTable;
    protected const int Y_AXIS_TICKMARKS = 5;
    protected int timeFrameId = -1;
    protected Color toolTipColor = Color.FromArgb(218, 238, (int) byte.MaxValue);
    protected Color[] chartColors = new Color[10]
    {
      Color.FromArgb(201, 229, 174),
      Color.FromArgb(249, 207, 163),
      Color.FromArgb(174, 224, 229),
      Color.FromArgb(240, 184, 238),
      Color.FromArgb(178, 174, 229),
      Color.FromArgb(244, 243, 156),
      Color.FromArgb(174, 199, 229),
      Color.FromArgb(242, 189, 189),
      Color.FromArgb(174, 229, 212),
      Color.FromArgb(229, 124, 217)
    };
    private string filter1FieldID = "";
    private string filter2FieldID = "";
    private IContainer components;
    protected Infragistics.Win.UltraWinChart.UltraChart ultraChart;
    protected Label lblNoDataFound;

    public event UltraControlBase.UpdateZoomInIconButton UpdateZoomInBtnHandler;

    public event UltraControlBase.UpdateExcelIconButton UpdateExcelBtnHandler;

    public event UltraControlBase.UpdatePrintIconButton UpdatePrintBtnHandler;

    public event UltraControlBase.ChartDataClicked ChartDataClickedHandler;

    private UltraChartProvider()
    {
    }

    public UltraChartProvider(
      DashboardForm frmDashboard,
      int snapshotHandle,
      DashboardReport dashboardReport,
      DashboardTemplate dashboardTemplate)
      : this(frmDashboard, snapshotHandle, dashboardReport, dashboardTemplate, dashboardTemplate.ChartType)
    {
    }

    public UltraChartProvider(
      DashboardForm frmDashboard,
      int snapshotHandle,
      DashboardReport dashboardReport,
      DashboardTemplate dashboardTemplate,
      DashboardChartType dashboardChartType)
      : base(frmDashboard, snapshotHandle, dashboardReport, dashboardTemplate, dashboardChartType)
    {
      this.InitializeComponent();
      this.setUltraChartCommonProperties();
      this.setUltraChartCustomProperties();
      this.setSnapshotProperties();
    }

    protected virtual void setUltraChartCommonProperties()
    {
      if (this.dashboardChartType == DashboardChartType.BarChart)
      {
        this.ultraChart.ChartType = ChartType.ColumnChart;
        this.ultraChart.ColumnChart.ColumnSpacing = 1;
        this.ultraChart.Axis.X.Labels.Visible = true;
        this.ultraChart.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";
        this.ultraChart.Axis.X.Labels.Orientation = TextOrientation.Horizontal;
        this.ultraChart.Axis.X.Labels.HorizontalAlign = StringAlignment.Center;
        this.ultraChart.Axis.X.Labels.VerticalAlign = StringAlignment.Center;
        this.ultraChart.Axis.X.Margin.Far.MarginType = LocationType.Percentage;
        this.ultraChart.Axis.X.Margin.Far.Value = 5.0;
        this.ultraChart.Axis.X.Extent = 10;
        this.ultraChart.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.UseCollection;
        ClipTextAxisLabelLayoutBehavior Behavior = new ClipTextAxisLabelLayoutBehavior();
        Behavior.Enabled = true;
        Behavior.ClipText = true;
        Behavior.UseOnlyToPreventCollisions = true;
        Behavior.EnableRollback = false;
        Behavior.HideText = false;
        Behavior.Trimming = StringTrimming.EllipsisCharacter;
        this.ultraChart.Axis.X.Labels.Layout.BehaviorCollection.Add((AxisLabelLayoutBehavior) Behavior);
        this.ultraChart.Axis.Y.Visible = true;
        this.ultraChart.Axis.Y.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.Auto;
        this.ultraChart.Axis.Y.Margin.Near.MarginType = LocationType.Pixels;
        this.ultraChart.Axis.Y.Margin.Near.Value = 5.0;
        this.ultraChart.Axis.Y.Extent = 60;
        this.ultraChart.Axis.Y.TickmarkStyle = AxisTickStyle.DataInterval;
        this.ultraChart.Axis.Y.TickmarkInterval = 0.0;
        this.ultraChart.Tooltips.FormatString = "<SERIES_LABEL>: <DATA_VALUE:#,##0.000>";
      }
      else
      {
        this.ultraChart.ChartType = ChartType.LineChart;
        this.ultraChart.LineChart.DrawStyle = LineDrawStyle.Solid;
        this.ultraChart.LineChart.StartStyle = LineCapStyle.NoAnchor;
        this.ultraChart.LineChart.EndStyle = LineCapStyle.NoAnchor;
        this.ultraChart.LineChart.MidPointAnchors = false;
        this.ultraChart.LineChart.Thickness = 4;
        this.ultraChart.Data.SwapRowsAndColumns = false;
        this.ultraChart.Data.ZeroAligned = true;
        this.ultraChart.Axis.X.Labels.Visible = true;
        this.ultraChart.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";
        this.ultraChart.Axis.X.Labels.Orientation = TextOrientation.Horizontal;
        this.ultraChart.Axis.X.Labels.HorizontalAlign = StringAlignment.Center;
        this.ultraChart.Axis.X.Labels.VerticalAlign = StringAlignment.Center;
        this.ultraChart.Axis.X.Margin.Far.MarginType = LocationType.Percentage;
        this.ultraChart.Axis.X.Margin.Far.Value = 5.0;
        this.ultraChart.Axis.X.Extent = 10;
        this.ultraChart.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.UseCollection;
        ClipTextAxisLabelLayoutBehavior Behavior = new ClipTextAxisLabelLayoutBehavior();
        Behavior.Enabled = true;
        Behavior.ClipText = true;
        Behavior.UseOnlyToPreventCollisions = true;
        Behavior.EnableRollback = false;
        Behavior.HideText = false;
        Behavior.Trimming = StringTrimming.EllipsisCharacter;
        this.ultraChart.Axis.X.Labels.Layout.BehaviorCollection.Add((AxisLabelLayoutBehavior) Behavior);
        this.ultraChart.Axis.Y.Visible = true;
        this.ultraChart.Axis.Y.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.Auto;
        this.ultraChart.Axis.Y.Margin.Near.MarginType = LocationType.Pixels;
        this.ultraChart.Axis.Y.Margin.Near.Value = 5.0;
        this.ultraChart.Axis.Y.Extent = 60;
        this.ultraChart.Axis.Y.TickmarkStyle = AxisTickStyle.DataInterval;
        this.ultraChart.Axis.Y.RangeType = AxisRangeType.Custom;
        this.ultraChart.Tooltips.FormatString = "<TREND_CHART_TOOLTIP>";
        this.ultraChart.LabelHash = new Hashtable()
        {
          {
            (object) "TREND_CHART_TOOLTIP",
            (object) new TrendChartTooltipRenderer()
          }
        };
      }
      this.printDocument = this.ultraChart.PrintDocument;
      this.ultraChart.TitleTop.Margins.Top = 0;
      this.ultraChart.TitleTop.Margins.Bottom = 0;
      this.ultraChart.TitleTop.Margins.Left = 0;
      this.ultraChart.TitleTop.Margins.Right = 0;
      this.ultraChart.TitleTop.HorizontalAlign = StringAlignment.Center;
      this.ultraChart.TitleTop.Text = string.Empty;
      this.ultraChart.TitleTop.Visible = false;
      this.ultraChart.Legend.Location = LegendLocation.Bottom;
      this.ultraChart.Legend.Margins.Left = 5;
      this.ultraChart.Legend.Margins.Right = 5;
      this.ultraChart.Legend.Margins.Top = 5;
      this.ultraChart.Legend.Margins.Bottom = 5;
      this.ultraChart.Legend.SpanPercentage = 15;
      this.ultraChart.Legend.BackgroundColor = ((Control) this.ultraChart).BackColor;
      this.ultraChart.Legend.Visible = 1 == this.frmDashboard.CurrentView.LayoutBlockCount;
      this.ultraChart.Tooltips.BackColor = this.toolTipColor;
      this.ultraChart.Tooltips.Display = TooltipDisplay.MouseMove;
      this.ultraChart.Tooltips.Overflow = TooltipOverflow.ClientArea;
      this.ultraChart.ColorModel.CustomPalette = this.chartColors;
      this.ultraChart.ColorModel.AlphaLevel = byte.MaxValue;
    }

    protected virtual void setUltraChartCustomProperties()
    {
    }

    public override bool NoDataFound()
    {
      return this.ultraChartDataTable == null || this.ultraChartDataTable.Rows.Count <= 0;
    }

    protected virtual void setSnapshotProperties()
    {
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
    }

    public override void RefreshData(DashboardDataCriteria reportCriteria)
    {
      if (this.ultraChartDataTable != null)
        this.ultraChartDataTable.Rows.Clear();
      string str = "No data matched the search criteria";
      if (this.dashboardTemplate == null)
        str = "This snapshot is missing or has been deleted.";
      else if (reportCriteria == null)
      {
        str = "No data matched the search criteria";
      }
      else
      {
        ((Control) this.ultraChart).Visible = false;
        this.ultraChartDataTable = (DataTable) null;
        try
        {
          this.validateFieldAccess(this.dashboardTemplate, reportCriteria);
          this.ultraChartDataTable = Session.ReportManager.QueryDataForDashboardReport(reportCriteria, !reportCriteria.ViewFilterTPOOrgId.Equals("0"), this.dashboardTemplate.SelectAllWOArchiveFolders);
          if (this.ultraChartDataTable != null)
            this.perpareDataTableForDisplay();
        }
        catch (Exception ex)
        {
          if (ex.Message.Contains("Violation of Business Rule access rights"))
            str = "You do not have permission to view the data in this snapshot";
          else if (ex.Message.Contains("Violation of field access right"))
          {
            str = "You do not have permission to access some fields in this snapshot";
          }
          else
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "An error was encountered while processing the '" + this.dashboardTemplate.TemplateName + "' snapshot.\nPlease review the snapshot's definition and try again.\n\n" + ex.Message + ".", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            str = "An error was encountered while processing this snapshot";
          }
        }
        this.ultraChart.Data.ResetDataSource();
      }
      if (this.ultraChartDataTable != null && 0 < this.ultraChartDataTable.Rows.Count)
      {
        if (this.UpdateZoomInBtnHandler != null)
          this.UpdateZoomInBtnHandler(true);
        if (this.UpdateExcelBtnHandler != null)
          this.UpdateExcelBtnHandler(true);
        if (this.UpdatePrintBtnHandler != null)
          this.UpdatePrintBtnHandler(true);
        ((Control) this.ultraChart).Visible = true;
        this.lblNoDataFound.Visible = false;
        if (this.dashboardTemplate.ChartType == DashboardChartType.BarChart)
        {
          this.buildBarChart();
          this.ultraChart.Data.DataSource = (object) this.ultraChartDataTable;
        }
        else
        {
          this.buildTrendChart();
          this.ultraChart.Data.DataSource = (object) this.ultraChartDataTable;
          string formatString = this.getFormatString(this.dashboardTemplate.YAxisField);
          ((TrendChartTooltipRenderer) this.ultraChart.LabelHash[(object) "TREND_CHART_TOOLTIP"]).SetDataTable(this.ultraChartDataTable);
          ((TrendChartTooltipRenderer) this.ultraChart.LabelHash[(object) "TREND_CHART_TOOLTIP"]).SetFormatString(formatString);
        }
        this.ultraChart.Data.RowLabelsColumn = 0;
        long yRangeMax;
        long yTickmarkInterval;
        this.getYMaxRangeAndYMarkInterval(this.ultraChartDataTable.Rows, out yRangeMax, out yTickmarkInterval);
        this.ultraChart.Axis.Y.RangeMax = (double) yRangeMax;
        this.ultraChart.Axis.Y.TickmarkInterval = (double) yTickmarkInterval;
        this.ultraChart.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:#,##0>";
      }
      else
      {
        if (this.UpdateZoomInBtnHandler != null)
          this.UpdateZoomInBtnHandler(false);
        if (this.UpdateExcelBtnHandler != null)
          this.UpdateExcelBtnHandler(false);
        if (this.UpdatePrintBtnHandler != null)
          this.UpdatePrintBtnHandler(false);
        ((Control) this.ultraChart).Visible = false;
        this.lblNoDataFound.Text = str;
        this.lblNoDataFound.Visible = true;
      }
      this.ultraChart.Data.DataBind();
      this.refreshFilterDrilldownFilters();
    }

    private void refreshFilterDrilldownFilters()
    {
      this.filter1FieldID = "";
      this.filter2FieldID = "";
      this.filter1FieldID = this.dashboardTemplate.XAxisField;
      if (this.dashboardTemplate.ChartType != DashboardChartType.TrendChart)
        return;
      this.filter2FieldID = this.dashboardTemplate.GroupByField;
    }

    public string Filter1FieldID => this.filter1FieldID;

    public string Filter1FieldName
    {
      get => this.filter1FieldID == "" ? "" : this.getFieldName(this.filter1FieldID);
    }

    public object[] Filter1ValueList
    {
      get
      {
        if (this.filter1FieldID == "" || this.ultraChartDataTable == null)
          return (object[]) null;
        List<string> stringList = new List<string>();
        if (this.dashboardTemplate.ChartType != DashboardChartType.TrendChart)
        {
          foreach (DataRow row in (InternalDataCollectionBase) this.ultraChartDataTable.Rows)
            stringList.Add(string.Concat(row[0]));
        }
        else
        {
          for (int index = 1; index < this.ultraChartDataTable.Columns.Count; ++index)
            stringList.Add(this.ultraChartDataTable.Columns[index].ColumnName);
        }
        return (object[]) stringList.ToArray();
      }
    }

    public string Filter2FieldID => this.filter2FieldID;

    public string Filter2FieldName
    {
      get => this.filter2FieldID == "" ? "" : this.getFieldName(this.filter2FieldID);
    }

    public object[] Filter2ValueList
    {
      get
      {
        if (this.filter2FieldID == "" || this.ultraChartDataTable == null)
          return (object[]) null;
        List<string> stringList = new List<string>();
        foreach (DataRow row in (InternalDataCollectionBase) this.ultraChartDataTable.Rows)
          stringList.Add(string.Concat(row[0]));
        return (object[]) stringList.ToArray();
      }
    }

    public override void Zoom(bool zoomIn)
    {
      base.Zoom(zoomIn);
      this.ultraChart.Legend.Visible = zoomIn;
    }

    private void perpareDataTableForDisplay()
    {
      foreach (DataRow row in (InternalDataCollectionBase) this.ultraChartDataTable.Rows)
      {
        if (row[0].ToString().Trim() == "")
          row[0] = (object) "(Empty)";
      }
      if (this.dashboardTemplate.ChartType == DashboardChartType.BarChart)
      {
        if (!("Loan.CurrentMilestoneName" == this.dashboardTemplate.XAxisField))
          return;
        this.sortBarChartInMilestoneOrder();
      }
      else
      {
        if (!("Dashboard.NoGroupBy" == this.dashboardTemplate.GroupByField))
          return;
        this.ultraChartDataTable.Rows[0][0] = (object) "All Loans";
      }
    }

    private void sortBarChartInMilestoneOrder()
    {
      DataTable dataTable = this.ultraChartDataTable.Copy();
      this.ultraChartDataTable.Rows.Clear();
      foreach (EllieMae.EMLite.Workflow.Milestone milestone in Session.StartupInfo.Milestones)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        {
          if (string.Equals(milestone.Name, row[0].ToString(), StringComparison.OrdinalIgnoreCase))
          {
            if ("submittal" == row[0].ToString())
              row[0] = (object) "Submittal";
            this.ultraChartDataTable.Rows.Add(row.ItemArray);
            dataTable.Rows.Remove(row);
            break;
          }
        }
      }
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        this.ultraChartDataTable.Rows.Add(row.ItemArray);
    }

    private void buildBarChart()
    {
      this.ultraChart.TitleLeft.Visible = true;
      this.ultraChart.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
      this.ultraChart.TitleLeft.HorizontalAlign = StringAlignment.Center;
      this.ultraChart.TitleLeft.VerticalAlign = StringAlignment.Center;
      this.ultraChart.TitleLeft.Font = new Font(FontFamily.GenericSansSerif, 8.25f, FontStyle.Regular, GraphicsUnit.Point);
      this.ultraChart.TitleLeft.Text = this.getSummaryName(this.dashboardTemplate.YAxisField, this.dashboardTemplate.YAxisSummaryType);
      this.ultraChart.TitleBottom.Visible = true;
      this.ultraChart.TitleBottom.Orientation = TextOrientation.Horizontal;
      this.ultraChart.TitleBottom.HorizontalAlign = StringAlignment.Center;
      this.ultraChart.TitleBottom.VerticalAlign = StringAlignment.Center;
      this.ultraChart.TitleBottom.Font = new Font(FontFamily.GenericSansSerif, 8.25f, FontStyle.Regular, GraphicsUnit.Point);
      this.ultraChart.TitleBottom.Text = this.getFieldName(this.dashboardTemplate.XAxisField);
      this.ultraChart.Axis.X.Labels.SeriesLabels.Format = AxisSeriesLabelFormat.None;
      this.ultraChart.Tooltips.FormatString = "<SERIES_LABEL>: <DATA_VALUE:" + this.getFormatString(this.dashboardTemplate.YAxisField) + ">";
    }

    private void buildTrendChart()
    {
      this.ultraChart.TitleLeft.Visible = true;
      this.ultraChart.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
      this.ultraChart.TitleLeft.HorizontalAlign = StringAlignment.Center;
      this.ultraChart.TitleLeft.VerticalAlign = StringAlignment.Center;
      this.ultraChart.TitleLeft.Font = new Font(FontFamily.GenericSansSerif, 8.25f, FontStyle.Regular, GraphicsUnit.Point);
      this.ultraChart.TitleLeft.Text = this.getSummaryName(this.dashboardTemplate.YAxisField, this.dashboardTemplate.YAxisSummaryType);
      this.ultraChart.TitleBottom.Visible = true;
      this.ultraChart.TitleBottom.HorizontalAlign = StringAlignment.Center;
      this.ultraChart.TitleBottom.Font = new Font(FontFamily.GenericSansSerif, 8.25f, FontStyle.Regular, GraphicsUnit.Point);
      this.ultraChart.TitleBottom.Text = this.getFieldName(this.dashboardTemplate.XAxisField);
    }

    public TitleAppearance TitleTop => this.ultraChart.TitleTop;

    public LegendAppearance Legend => this.ultraChart.Legend;

    public override void ExportToExcel()
    {
      if (this.ultraChartDataTable != null)
      {
        if (this.ultraChartDataTable.Rows.Count != 0)
        {
          try
          {
            ExcelHandler excelHandler = new ExcelHandler();
            excelHandler.AddDataTable(this.getExcelDataTable());
            excelHandler.CreateExcel();
            return;
          }
          catch (Exception ex)
          {
            Tracing.Log(UltraChartProvider.sw, nameof (UltraChartProvider), TraceLevel.Error, "Error during export: " + (object) ex);
            int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to export to Microsoft Excel.\nEnsure that you have Excel installed and it is working properly.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
      }
      int num1 = (int) Utils.Dialog((IWin32Window) this, "There is currently no data to export.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private DataTable getExcelDataTable()
    {
      DataTable excelDataTable = this.ultraChartDataTable.Clone();
      excelDataTable.Merge(this.ultraChartDataTable, false);
      try
      {
        if (this.dashboardTemplate.ChartType == DashboardChartType.BarChart)
        {
          excelDataTable.Columns[0].ColumnName = this.getFieldName(this.dashboardTemplate.XAxisField);
          excelDataTable.Columns[1].ColumnName = this.getSummaryName(this.dashboardTemplate.YAxisField, this.dashboardTemplate.YAxisSummaryType);
        }
        else if (DashboardChartType.TrendChart == this.dashboardTemplate.ChartType)
        {
          if ("Dashboard.NoGroupBy" == this.dashboardTemplate.GroupByField)
          {
            excelDataTable.Columns[0].ColumnName = this.getFieldName(this.dashboardTemplate.XAxisField);
            excelDataTable.Rows[0][0] = (object) this.getSummaryName(this.dashboardTemplate.YAxisField, this.dashboardTemplate.YAxisSummaryType);
          }
          else
            excelDataTable.Columns[0].ColumnName = this.getFieldName(this.dashboardTemplate.GroupByField);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(UltraChartProvider.sw, nameof (UltraChartProvider), TraceLevel.Error, "Unable to set Excel column headers: " + (object) ex);
      }
      return excelDataTable;
    }

    public override void PrintSnapshot()
    {
      if (this.ultraChartDataTable == null || this.ultraChartDataTable.Rows.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "There is currently no data to print.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        base.PrintSnapshot();
    }

    public override void SetPrintProperties()
    {
      DateTime startDate;
      DateTime endDate;
      if (this.dashboardTemplate.ChartType == DashboardChartType.BarChart)
        TimeFrameUtility.GetStartEndDatesForTimeFrame((DashboardTimeFrameType) this.timeFrameId, out startDate, out endDate);
      else
        TimeFrameUtility.GetStartEndDatesForTimePeriod((DashboardTimePeriodType) this.timeFrameId, 0, out startDate, out endDate);
      string str = "Date Range: " + startDate.ToShortDateString() + " through " + endDate.ToShortDateString();
      this.ultraChart.TitleTop.Visible = true;
      this.ultraChart.Legend.Visible = true;
    }

    public override void ResetPrintProperties() => this.ultraChart.TitleTop.Visible = false;

    protected void getYMaxRangeAndYMarkInterval(
      DataRowCollection dataRows,
      out long yRangeMax,
      out long yTickmarkInterval)
    {
      long num1 = 0;
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRows)
      {
        for (int columnIndex = 1; columnIndex < dataRow.Table.Columns.Count; ++columnIndex)
        {
          if (!(dataRow[columnIndex] is DBNull))
          {
            if (dataRow[columnIndex] != null)
            {
              try
              {
                long num2 = (long) Math.Ceiling(Convert.ToDouble(dataRow[columnIndex].ToString()));
                if (num1 < num2)
                  num1 = num2;
              }
              catch (Exception ex)
              {
              }
            }
          }
        }
      }
      if (5L >= num1)
      {
        yRangeMax = num1 == 0L ? 1L : num1;
        yTickmarkInterval = 1L;
      }
      else
      {
        yRangeMax = num1 % 5L == 0L ? num1 : num1 + (5L - num1 % 5L);
        yTickmarkInterval = yRangeMax / 5L;
      }
    }

    private void ultraChart_InvalidDataReceived(object sender, ChartDataInvalidEventArgs e)
    {
      e.LabelStyle.Font = new Font(FontFamily.GenericSansSerif, 8.25f, FontStyle.Regular, GraphicsUnit.Point);
      e.LabelStyle.FontColor = Color.Brown;
      e.LabelStyle.HorizontalAlign = StringAlignment.Center;
      e.LabelStyle.VerticalAlign = StringAlignment.Center;
      e.Text = "No data matched the search criteria.";
    }

    private void UltraChartProvider_Resize(object sender, EventArgs e)
    {
      this.lblNoDataFound.Location = new Point(this.Left, (this.Height - this.lblNoDataFound.Height) / 2);
    }

    private void ultraChart_ChartDataClicked(object sender, ChartDataEventArgs e)
    {
      if (this.ChartDataClickedHandler == null)
        return;
      string filterValue1 = "";
      string filterValue2 = "";
      if (this.dashboardChartType == DashboardChartType.BarChart)
        filterValue1 = string.Concat(this.ultraChartDataTable.Rows[e.DataRow][0]);
      else if (this.dashboardChartType == DashboardChartType.TrendChart)
      {
        filterValue1 = e.ColumnLabel;
        filterValue2 = string.Concat(this.ultraChartDataTable.Rows[e.DataRow][0]);
      }
      this.ChartDataClickedHandler(filterValue1, filterValue2);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      PaintElement paintElement = new PaintElement();
      GradientEffect gradientEffect = new GradientEffect();
      this.ultraChart = new Infragistics.Win.UltraWinChart.UltraChart();
      this.lblNoDataFound = new Label();
      ((ISupportInitialize) this.ultraChart).BeginInit();
      this.SuspendLayout();
      this.ultraChart.Axis.BackColor = Color.FromArgb((int) byte.MaxValue, 248, 220);
      paintElement.ElementType = PaintElementType.None;
      paintElement.Fill = Color.FromArgb((int) byte.MaxValue, 248, 220);
      this.ultraChart.Axis.PE = paintElement;
      this.ultraChart.Axis.X.Labels.Font = new Font("Verdana", 7f);
      this.ultraChart.Axis.X.Labels.FontColor = Color.DimGray;
      this.ultraChart.Axis.X.Labels.HorizontalAlign = StringAlignment.Near;
      this.ultraChart.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";
      this.ultraChart.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.Auto;
      this.ultraChart.Axis.X.Labels.Orientation = TextOrientation.VerticalLeftFacing;
      this.ultraChart.Axis.X.Labels.SeriesLabels.Font = new Font("Verdana", 7f);
      this.ultraChart.Axis.X.Labels.SeriesLabels.FontColor = Color.DimGray;
      this.ultraChart.Axis.X.Labels.SeriesLabels.HorizontalAlign = StringAlignment.Center;
      this.ultraChart.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.Auto;
      this.ultraChart.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      this.ultraChart.Axis.X.Labels.SeriesLabels.VerticalAlign = StringAlignment.Center;
      this.ultraChart.Axis.X.Labels.VerticalAlign = StringAlignment.Center;
      this.ultraChart.Axis.X.LineThickness = 1;
      this.ultraChart.Axis.X.MajorGridLines.AlphaLevel = byte.MaxValue;
      this.ultraChart.Axis.X.MajorGridLines.Color = Color.Gainsboro;
      this.ultraChart.Axis.X.MajorGridLines.DrawStyle = LineDrawStyle.Dot;
      this.ultraChart.Axis.X.MajorGridLines.Visible = false;
      this.ultraChart.Axis.X.MinorGridLines.AlphaLevel = byte.MaxValue;
      this.ultraChart.Axis.X.MinorGridLines.Color = Color.LightGray;
      this.ultraChart.Axis.X.MinorGridLines.DrawStyle = LineDrawStyle.Dot;
      this.ultraChart.Axis.X.MinorGridLines.Visible = false;
      this.ultraChart.Axis.X.TickmarkStyle = AxisTickStyle.Smart;
      this.ultraChart.Axis.X.Visible = true;
      this.ultraChart.Axis.X2.Labels.Font = new Font("Verdana", 7f);
      this.ultraChart.Axis.X2.Labels.FontColor = Color.Gray;
      this.ultraChart.Axis.X2.Labels.HorizontalAlign = StringAlignment.Far;
      this.ultraChart.Axis.X2.Labels.ItemFormatString = "<ITEM_LABEL>";
      this.ultraChart.Axis.X2.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.Auto;
      this.ultraChart.Axis.X2.Labels.Orientation = TextOrientation.VerticalLeftFacing;
      this.ultraChart.Axis.X2.Labels.SeriesLabels.Font = new Font("Verdana", 7f);
      this.ultraChart.Axis.X2.Labels.SeriesLabels.FontColor = Color.Gray;
      this.ultraChart.Axis.X2.Labels.SeriesLabels.HorizontalAlign = StringAlignment.Center;
      this.ultraChart.Axis.X2.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.Auto;
      this.ultraChart.Axis.X2.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      this.ultraChart.Axis.X2.Labels.SeriesLabels.VerticalAlign = StringAlignment.Center;
      this.ultraChart.Axis.X2.Labels.VerticalAlign = StringAlignment.Center;
      this.ultraChart.Axis.X2.Labels.Visible = false;
      this.ultraChart.Axis.X2.LineThickness = 1;
      this.ultraChart.Axis.X2.MajorGridLines.AlphaLevel = byte.MaxValue;
      this.ultraChart.Axis.X2.MajorGridLines.Color = Color.Gainsboro;
      this.ultraChart.Axis.X2.MajorGridLines.DrawStyle = LineDrawStyle.Dot;
      this.ultraChart.Axis.X2.MajorGridLines.Visible = true;
      this.ultraChart.Axis.X2.MinorGridLines.AlphaLevel = byte.MaxValue;
      this.ultraChart.Axis.X2.MinorGridLines.Color = Color.LightGray;
      this.ultraChart.Axis.X2.MinorGridLines.DrawStyle = LineDrawStyle.Dot;
      this.ultraChart.Axis.X2.MinorGridLines.Visible = false;
      this.ultraChart.Axis.X2.TickmarkStyle = AxisTickStyle.Smart;
      this.ultraChart.Axis.X2.Visible = false;
      this.ultraChart.Axis.Y.Labels.Font = new Font("Verdana", 7f);
      this.ultraChart.Axis.Y.Labels.FontColor = Color.DimGray;
      this.ultraChart.Axis.Y.Labels.HorizontalAlign = StringAlignment.Far;
      this.ultraChart.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
      this.ultraChart.Axis.Y.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.Auto;
      this.ultraChart.Axis.Y.Labels.Orientation = TextOrientation.Horizontal;
      this.ultraChart.Axis.Y.Labels.SeriesLabels.Font = new Font("Verdana", 7f);
      this.ultraChart.Axis.Y.Labels.SeriesLabels.FontColor = Color.DimGray;
      this.ultraChart.Axis.Y.Labels.SeriesLabels.FormatString = "";
      this.ultraChart.Axis.Y.Labels.SeriesLabels.HorizontalAlign = StringAlignment.Far;
      this.ultraChart.Axis.Y.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.Auto;
      this.ultraChart.Axis.Y.Labels.SeriesLabels.Orientation = TextOrientation.VerticalLeftFacing;
      this.ultraChart.Axis.Y.Labels.SeriesLabels.VerticalAlign = StringAlignment.Center;
      this.ultraChart.Axis.Y.Labels.VerticalAlign = StringAlignment.Center;
      this.ultraChart.Axis.Y.LineThickness = 1;
      this.ultraChart.Axis.Y.MajorGridLines.AlphaLevel = byte.MaxValue;
      this.ultraChart.Axis.Y.MajorGridLines.Color = Color.Gainsboro;
      this.ultraChart.Axis.Y.MajorGridLines.DrawStyle = LineDrawStyle.Dot;
      this.ultraChart.Axis.Y.MajorGridLines.Visible = false;
      this.ultraChart.Axis.Y.MinorGridLines.AlphaLevel = byte.MaxValue;
      this.ultraChart.Axis.Y.MinorGridLines.Color = Color.LightGray;
      this.ultraChart.Axis.Y.MinorGridLines.DrawStyle = LineDrawStyle.Dot;
      this.ultraChart.Axis.Y.MinorGridLines.Visible = false;
      this.ultraChart.Axis.Y.TickmarkInterval = 50.0;
      this.ultraChart.Axis.Y.TickmarkStyle = AxisTickStyle.Smart;
      this.ultraChart.Axis.Y.Visible = true;
      this.ultraChart.Axis.Y2.Labels.Font = new Font("Verdana", 7f);
      this.ultraChart.Axis.Y2.Labels.FontColor = Color.Gray;
      this.ultraChart.Axis.Y2.Labels.HorizontalAlign = StringAlignment.Near;
      this.ultraChart.Axis.Y2.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
      this.ultraChart.Axis.Y2.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.Auto;
      this.ultraChart.Axis.Y2.Labels.Orientation = TextOrientation.Horizontal;
      this.ultraChart.Axis.Y2.Labels.SeriesLabels.Font = new Font("Verdana", 7f);
      this.ultraChart.Axis.Y2.Labels.SeriesLabels.FontColor = Color.Gray;
      this.ultraChart.Axis.Y2.Labels.SeriesLabels.FormatString = "";
      this.ultraChart.Axis.Y2.Labels.SeriesLabels.HorizontalAlign = StringAlignment.Near;
      this.ultraChart.Axis.Y2.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.Auto;
      this.ultraChart.Axis.Y2.Labels.SeriesLabels.Orientation = TextOrientation.VerticalLeftFacing;
      this.ultraChart.Axis.Y2.Labels.SeriesLabels.VerticalAlign = StringAlignment.Center;
      this.ultraChart.Axis.Y2.Labels.VerticalAlign = StringAlignment.Center;
      this.ultraChart.Axis.Y2.Labels.Visible = false;
      this.ultraChart.Axis.Y2.LineThickness = 1;
      this.ultraChart.Axis.Y2.MajorGridLines.AlphaLevel = byte.MaxValue;
      this.ultraChart.Axis.Y2.MajorGridLines.Color = Color.Gainsboro;
      this.ultraChart.Axis.Y2.MajorGridLines.DrawStyle = LineDrawStyle.Dot;
      this.ultraChart.Axis.Y2.MajorGridLines.Visible = true;
      this.ultraChart.Axis.Y2.MinorGridLines.AlphaLevel = byte.MaxValue;
      this.ultraChart.Axis.Y2.MinorGridLines.Color = Color.LightGray;
      this.ultraChart.Axis.Y2.MinorGridLines.DrawStyle = LineDrawStyle.Dot;
      this.ultraChart.Axis.Y2.MinorGridLines.Visible = false;
      this.ultraChart.Axis.Y2.TickmarkInterval = 50.0;
      this.ultraChart.Axis.Y2.TickmarkStyle = AxisTickStyle.Smart;
      this.ultraChart.Axis.Y2.Visible = false;
      this.ultraChart.Axis.Z.Labels.Font = new Font("Verdana", 7f);
      this.ultraChart.Axis.Z.Labels.FontColor = Color.DimGray;
      this.ultraChart.Axis.Z.Labels.HorizontalAlign = StringAlignment.Near;
      this.ultraChart.Axis.Z.Labels.ItemFormatString = "";
      this.ultraChart.Axis.Z.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.Auto;
      this.ultraChart.Axis.Z.Labels.Orientation = TextOrientation.Horizontal;
      this.ultraChart.Axis.Z.Labels.SeriesLabels.Font = new Font("Verdana", 7f);
      this.ultraChart.Axis.Z.Labels.SeriesLabels.FontColor = Color.DimGray;
      this.ultraChart.Axis.Z.Labels.SeriesLabels.HorizontalAlign = StringAlignment.Near;
      this.ultraChart.Axis.Z.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.Auto;
      this.ultraChart.Axis.Z.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      this.ultraChart.Axis.Z.Labels.SeriesLabels.VerticalAlign = StringAlignment.Center;
      this.ultraChart.Axis.Z.Labels.VerticalAlign = StringAlignment.Center;
      this.ultraChart.Axis.Z.Labels.Visible = false;
      this.ultraChart.Axis.Z.LineThickness = 1;
      this.ultraChart.Axis.Z.MajorGridLines.AlphaLevel = byte.MaxValue;
      this.ultraChart.Axis.Z.MajorGridLines.Color = Color.Gainsboro;
      this.ultraChart.Axis.Z.MajorGridLines.DrawStyle = LineDrawStyle.Dot;
      this.ultraChart.Axis.Z.MajorGridLines.Visible = true;
      this.ultraChart.Axis.Z.MinorGridLines.AlphaLevel = byte.MaxValue;
      this.ultraChart.Axis.Z.MinorGridLines.Color = Color.LightGray;
      this.ultraChart.Axis.Z.MinorGridLines.DrawStyle = LineDrawStyle.Dot;
      this.ultraChart.Axis.Z.MinorGridLines.Visible = false;
      this.ultraChart.Axis.Z.TickmarkStyle = AxisTickStyle.Smart;
      this.ultraChart.Axis.Z.Visible = false;
      this.ultraChart.Axis.Z2.Labels.Font = new Font("Verdana", 7f);
      this.ultraChart.Axis.Z2.Labels.FontColor = Color.Gray;
      this.ultraChart.Axis.Z2.Labels.HorizontalAlign = StringAlignment.Near;
      this.ultraChart.Axis.Z2.Labels.ItemFormatString = "";
      this.ultraChart.Axis.Z2.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.Auto;
      this.ultraChart.Axis.Z2.Labels.Orientation = TextOrientation.Horizontal;
      this.ultraChart.Axis.Z2.Labels.SeriesLabels.Font = new Font("Verdana", 7f);
      this.ultraChart.Axis.Z2.Labels.SeriesLabels.FontColor = Color.Gray;
      this.ultraChart.Axis.Z2.Labels.SeriesLabels.HorizontalAlign = StringAlignment.Near;
      this.ultraChart.Axis.Z2.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.Auto;
      this.ultraChart.Axis.Z2.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      this.ultraChart.Axis.Z2.Labels.SeriesLabels.VerticalAlign = StringAlignment.Center;
      this.ultraChart.Axis.Z2.Labels.VerticalAlign = StringAlignment.Center;
      this.ultraChart.Axis.Z2.Labels.Visible = false;
      this.ultraChart.Axis.Z2.LineThickness = 1;
      this.ultraChart.Axis.Z2.MajorGridLines.AlphaLevel = byte.MaxValue;
      this.ultraChart.Axis.Z2.MajorGridLines.Color = Color.Gainsboro;
      this.ultraChart.Axis.Z2.MajorGridLines.DrawStyle = LineDrawStyle.Dot;
      this.ultraChart.Axis.Z2.MajorGridLines.Visible = true;
      this.ultraChart.Axis.Z2.MinorGridLines.AlphaLevel = byte.MaxValue;
      this.ultraChart.Axis.Z2.MinorGridLines.Color = Color.LightGray;
      this.ultraChart.Axis.Z2.MinorGridLines.DrawStyle = LineDrawStyle.Dot;
      this.ultraChart.Axis.Z2.MinorGridLines.Visible = false;
      this.ultraChart.Axis.Z2.TickmarkStyle = AxisTickStyle.Smart;
      this.ultraChart.Axis.Z2.Visible = false;
      this.ultraChart.Border.Color = Color.Transparent;
      this.ultraChart.ColorModel.AlphaLevel = (byte) 150;
      this.ultraChart.ColorModel.ModelStyle = ColorModels.CustomLinear;
      this.ultraChart.Data.SwapRowsAndColumns = true;
      this.ultraChart.Data.ZeroAligned = true;
      ((Control) this.ultraChart).Dock = DockStyle.Fill;
      this.ultraChart.Effects.Effects.Add((IEffect) gradientEffect);
      ((Control) this.ultraChart).Location = new Point(0, 0);
      ((Control) this.ultraChart).Name = "ultraChart";
      ((Control) this.ultraChart).Size = new Size(424, 298);
      this.ultraChart.TabIndex = 2;
      this.ultraChart.TitleBottom.Visible = false;
      this.ultraChart.TitleTop.Visible = false;
      this.ultraChart.Tooltips.HighlightFillColor = Color.DimGray;
      this.ultraChart.Tooltips.HighlightOutlineColor = Color.DarkGray;
      this.ultraChart.InvalidDataReceived += new ChartDataInvalidEventHandler(this.ultraChart_InvalidDataReceived);
      this.ultraChart.ChartDataClicked += new ChartDataClickedEventHandler(this.ultraChart_ChartDataClicked);
      this.lblNoDataFound.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblNoDataFound.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblNoDataFound.ForeColor = Color.Red;
      this.lblNoDataFound.Location = new Point(0, 132);
      this.lblNoDataFound.Name = "lblNoDataFound";
      this.lblNoDataFound.Size = new Size(424, 33);
      this.lblNoDataFound.TabIndex = 4;
      this.lblNoDataFound.Text = "No data matched the search criteria";
      this.lblNoDataFound.TextAlign = ContentAlignment.MiddleCenter;
      this.lblNoDataFound.Visible = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.lblNoDataFound);
      this.Controls.Add((Control) this.ultraChart);
      this.Name = nameof (UltraChartProvider);
      this.Size = new Size(424, 298);
      this.Resize += new EventHandler(this.UltraChartProvider_Resize);
      ((ISupportInitialize) this.ultraChart).EndInit();
      this.ResumeLayout(false);
    }
  }
}
