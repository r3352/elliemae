// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.UltraGridProvider
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.ClientSession.Dashboard;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using Infragistics.Win;
using Infragistics.Win.Layout;
using Infragistics.Win.Printing;
using Infragistics.Win.UltraWinGrid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class UltraGridProvider : UltraControlBase
  {
    private const string className = "UltraGridBase";
    private static readonly string sw = Tracing.SwReportControl;
    private const int SUMMARY_COLUMN_WIDTH = 35;
    protected DataSet ultraGridDataSet;
    protected DataTable ultraGridDataTable;
    protected int timeFrameId = -1;
    protected SortedColumnsCollection ultraGridSortedColumns;
    private string filter1FieldID = "";
    private string filter2FieldID = "";
    private IContainer components;
    protected UltraGrid ultraGrid;
    protected Label lblNoDataFound;

    public event UltraControlBase.UpdateZoomInIconButton UpdateZoomInBtnHandler;

    public event UltraControlBase.UpdateExcelIconButton UpdateExcelBtnHandler;

    public event UltraControlBase.UpdatePrintIconButton UpdatePrintBtnHandler;

    public event UltraControlBase.ChartDataClicked ChartDataClickedHandler;

    private UltraGridProvider()
    {
    }

    public UltraGridProvider(
      DashboardForm frmDashboard,
      int snapshotHandle,
      DashboardReport dashboardReport,
      DashboardTemplate dashboardTemplate)
      : base(frmDashboard, snapshotHandle, dashboardReport, dashboardTemplate, dashboardTemplate.ChartType)
    {
      this.InitializeComponent();
      this.setUltraGridCommonProperties();
      this.setUltraGridCustomProperties();
      this.setSnapshotProperties();
      this.ultraGrid.InitializePrint += new InitializePrintEventHandler(this.ultraGrid_InitializePrint);
    }

    protected virtual void setUltraGridCommonProperties()
    {
      this.ultraGridDataSet = new DataSet();
      this.ultraGridDataTable = new DataTable();
      this.ultraGridDataSet.Tables.Add(this.ultraGridDataTable);
      this.ultraGrid.DataSource = (object) this.ultraGridDataSet;
      UltraGridPrintDocument gridPrintDocument = new UltraGridPrintDocument();
      gridPrintDocument.Grid = this.ultraGrid;
      gridPrintDocument.ColumnClipMode = ColumnClipMode.RepeatClippedColumns;
      gridPrintDocument.Header.Height = 50;
      gridPrintDocument.Header.Appearance.FontData.SizeInPoints = 10f;
      gridPrintDocument.Header.Appearance.TextHAlign = HAlign.Center;
      gridPrintDocument.Header.Appearance.TextVAlign = VAlign.Middle;
      gridPrintDocument.Header.BorderStyle = UIElementBorderStyle.None;
      gridPrintDocument.Footer.Height = 20;
      gridPrintDocument.Footer.Appearance.FontData.Italic = DefaultableBoolean.True;
      gridPrintDocument.Footer.Appearance.TextHAlign = HAlign.Right;
      gridPrintDocument.Footer.BorderStyle = UIElementBorderStyle.None;
      gridPrintDocument.Footer.TextRight = "Page [Page #]";
      this.printDocument = (PrintDocument) gridPrintDocument;
      this.ultraGrid.DisplayLayout.Appearance.BackColor = Color.White;
      this.ultraGrid.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
      this.ultraGrid.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;
      this.ultraGrid.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
      this.ultraGrid.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;
      this.ultraGrid.DisplayLayout.Override.AllowGroupBy = DefaultableBoolean.False;
      this.ultraGrid.DisplayLayout.Override.AllowGroupMoving = AllowGroupMoving.NotAllowed;
      this.ultraGrid.DisplayLayout.Override.AllowGroupSwapping = AllowGroupSwapping.NotAllowed;
      this.ultraGrid.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;
      this.ultraGrid.DisplayLayout.Override.RowSelectors = DefaultableBoolean.False;
      this.ultraGrid.DisplayLayout.Override.BorderStyleRow = UIElementBorderStyle.Dotted;
      this.ultraGrid.DisplayLayout.ScrollStyle = ScrollStyle.Immediate;
      this.ultraGrid.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
      this.ultraGrid.DisplayLayout.Override.HeaderStyle = HeaderStyle.WindowsXPCommand;
      this.ultraGrid.DisplayLayout.Override.SupportDataErrorInfo = SupportDataErrorInfo.CellsOnly;
      this.ultraGrid.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.SortSingle;
    }

    protected virtual void setUltraGridCustomProperties()
    {
      ((ISupportInitialize) this.ultraGrid).BeginInit();
      this.ultraGridDataSet.BeginInit();
      this.ultraGridDataTable.BeginInit();
      this.SuspendLayout();
      if (DashboardChartType.LoanTable == this.dashboardTemplate.ChartType)
        this.setupLoanTable();
      else
        this.setupUserTable();
      ((ISupportInitialize) this.ultraGrid).EndInit();
      this.ultraGridDataSet.EndInit();
      this.ultraGridDataTable.EndInit();
      this.ResumeLayout(false);
      this.ultraGrid.DisplayLayout.RowScrollRegions[0].SizingMode = SizingMode.Fixed;
    }

    public override bool NoDataFound()
    {
      return this.ultraGridDataTable == null || this.ultraGridDataTable.Rows.Count <= 0;
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
        this.timeFrameId = 9;
        this.dashboardReport.ReportParameters[0] = this.timeFrameId.ToString();
      }
    }

    private void setupLoanTable()
    {
      this.ultraGridDataTable.Columns.Add(new DataColumn("Loan Guid", typeof (string)));
      foreach (EllieMae.EMLite.ClientServer.Reporting.ColumnInfo field in this.dashboardTemplate.Fields)
      {
        DataColumn column = this.newDataColumn(field.Description, field.CriterionName);
        for (int index = 2; index < this.dashboardTemplate.Fields.Count + 2; ++index)
        {
          try
          {
            this.ultraGridDataTable.Columns.Add(column);
            break;
          }
          catch (DuplicateNameException ex)
          {
            column.ColumnName = field.Description + "_" + index.ToString();
          }
        }
        field.Description = column.ColumnName;
      }
      UltraGridBand ultraGridBand = new UltraGridBand("BandOne", -1);
      ultraGridBand.Columns.Add((object) new UltraGridColumn("Loan Guid")
      {
        Hidden = true
      });
      foreach (EllieMae.EMLite.ClientServer.Reporting.ColumnInfo field in this.dashboardTemplate.Fields)
      {
        UltraGridColumn ultraGridColumn = this.newUltraGridColumn(field);
        ultraGridBand.Columns.Add((object) ultraGridColumn);
      }
      this.ultraGrid.DisplayLayout.BandsSerializer.Add((object) ultraGridBand);
      this.ultraGrid.DisplayLayout.AutoFitStyle = AutoFitStyle.None;
      this.ultraGrid.DisplayLayout.Scrollbars = Scrollbars.Automatic;
    }

    private DataColumn newDataColumn(string columnName, string criterionName)
    {
      System.Type dataType = typeof (string);
      FieldFormat fieldFormat = this.getFieldFormat(criterionName);
      switch (fieldFormat)
      {
        case FieldFormat.INTEGER:
          dataType = typeof (int);
          break;
        case FieldFormat.DECIMAL_1:
        case FieldFormat.DECIMAL_2:
        case FieldFormat.DECIMAL_3:
        case FieldFormat.DECIMAL_4:
        case FieldFormat.DECIMAL_6:
        case FieldFormat.DECIMAL_5:
        case FieldFormat.DECIMAL_7:
        case FieldFormat.DECIMAL:
        case FieldFormat.DECIMAL_10:
          dataType = typeof (Decimal);
          break;
        case FieldFormat.DATE:
        case FieldFormat.DATETIME:
          dataType = typeof (DateTime);
          break;
      }
      DataColumn dataColumn = new DataColumn(columnName, dataType);
      dataColumn.ExtendedProperties.Add((object) "FieldFormat", (object) fieldFormat.ToString());
      return dataColumn;
    }

    private UltraGridColumn newUltraGridColumn(EllieMae.EMLite.ClientServer.Reporting.ColumnInfo columnInfo)
    {
      UltraGridColumn ultraGridColumn = new UltraGridColumn(columnInfo.Description);
      ultraGridColumn.Header.Caption = columnInfo.Description;
      ultraGridColumn.AllowRowSummaries = AllowRowSummaries.False;
      ultraGridColumn.Format = this.getFormatString(columnInfo.CriterionName);
      HAlign halign = string.Empty == ultraGridColumn.Format ? HAlign.Left : HAlign.Right;
      ultraGridColumn.Header.Appearance.TextHAlign = halign;
      ultraGridColumn.CellAppearance.TextHAlign = halign;
      return ultraGridColumn;
    }

    private void setupUserTable()
    {
      this.ultraGrid.DisplayLayout.BandsSerializer.Add((object) new UltraGridBand("BandOne", -1));
      this.ultraGrid.DisplayLayout.Scrollbars = Scrollbars.Automatic;
    }

    private void refreshFilterDrilldownFilters()
    {
      this.filter1FieldID = "";
      this.filter2FieldID = "";
      this.filter1FieldID = "loanassociate.userid";
      this.filter2FieldID = this.dashboardTemplate.GroupByField;
    }

    public string Filter1FieldID => this.filter1FieldID;

    public string Filter1FieldName => "User ID";

    public object[] Filter1ValueList
    {
      get
      {
        if (this.filter1FieldID == "" || this.ultraGridDataTable == null)
          return (object[]) null;
        List<string> stringList = new List<string>();
        if (this.dashboardTemplate.ChartType != DashboardChartType.TrendChart)
        {
          foreach (DataRow row in (InternalDataCollectionBase) this.ultraGridDataTable.Rows)
            stringList.Add(string.Concat(row[0]));
        }
        else
        {
          for (int index = 1; index < this.ultraGridDataTable.Columns.Count; ++index)
            stringList.Add(this.ultraGridDataTable.Columns[index].ColumnName);
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
        if (this.filter2FieldID == "" || this.ultraGridDataTable == null)
          return (object[]) null;
        List<string> stringList = new List<string>();
        foreach (DataColumn column in (InternalDataCollectionBase) this.ultraGridDataTable.Columns)
        {
          if (!(column.Caption == "User ID"))
          {
            string str = column.ColumnName.Replace("_*_" + column.Caption, "");
            if (!stringList.Contains(str))
              stringList.Add(str);
          }
        }
        return (object[]) stringList.ToArray();
      }
    }

    public override void RefreshData(DashboardDataCriteria reportCriteria)
    {
      string str = "No data matched the search criteria";
      DataTable dataTable = (DataTable) null;
      if (this.dashboardTemplate == null)
        str = "This snapshot is missing or has been deleted.";
      else if (reportCriteria == null)
      {
        str = "No data matched the search criteria";
      }
      else
      {
        this.reportDataCriteria = reportCriteria;
        ((Control) this.ultraGrid).Visible = false;
        try
        {
          this.validateFieldAccess(this.dashboardTemplate, reportCriteria);
          dataTable = Session.ReportManager.QueryDataForDashboardReport(reportCriteria, !reportCriteria.ViewFilterTPOOrgId.Equals("0"), this.dashboardTemplate.SelectAllWOArchiveFolders);
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
        this.ultraGridDataTable.Rows.Clear();
      }
      if (dataTable != null && 0 < dataTable.Rows.Count)
      {
        if (this.UpdateZoomInBtnHandler != null)
          this.UpdateZoomInBtnHandler(true);
        if (this.UpdateExcelBtnHandler != null)
          this.UpdateExcelBtnHandler(true);
        if (this.UpdatePrintBtnHandler != null)
          this.UpdatePrintBtnHandler(true);
        ((Control) this.ultraGrid).Visible = true;
        this.lblNoDataFound.Visible = false;
        if (DashboardChartType.LoanTable == this.dashboardTemplate.ChartType)
        {
          this.buildLoanTable(dataTable);
          this.ultraGrid.DataBind();
        }
        else
        {
          this.buildUserTable(dataTable);
          this.ultraGrid.DataBind();
        }
      }
      else
      {
        if (this.UpdateZoomInBtnHandler != null)
          this.UpdateZoomInBtnHandler(false);
        if (this.UpdateExcelBtnHandler != null)
          this.UpdateExcelBtnHandler(false);
        if (this.UpdatePrintBtnHandler != null)
          this.UpdatePrintBtnHandler(false);
        this.ultraGrid.BeforeSortChange -= new BeforeSortChangeEventHandler(this.ultraGrid_BeforeSortChange);
        this.ultraGrid.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.None;
        this.ultraGrid.DataBind();
        ((Control) this.ultraGrid).Visible = false;
        this.lblNoDataFound.Text = str;
        this.lblNoDataFound.Visible = true;
      }
      this.refreshFilterDrilldownFilters();
    }

    public int SetTimeFrameID
    {
      set => this.timeFrameId = value;
    }

    private void buildLoanTable(DataTable dtReportData)
    {
      List<DataColumn> collection = new List<DataColumn>();
      foreach (DataColumn column in (InternalDataCollectionBase) dtReportData.Columns)
      {
        if (column.ColumnName.StartsWith("_Hide_"))
          collection.Add(column);
      }
      foreach (DataRow row1 in (InternalDataCollectionBase) dtReportData.Rows)
      {
        DataRow row2 = this.ultraGridDataTable.NewRow();
        if (row2.ItemArray.Length == row1.ItemArray.Length)
        {
          for (int columnIndex = 0; columnIndex < dtReportData.Columns.Count; ++columnIndex)
          {
            try
            {
              row2[columnIndex] = row1[columnIndex];
            }
            catch (ArgumentException ex)
            {
              row2[columnIndex] = (object) DBNull.Value;
            }
          }
          List<DataColumn> dataColumnList = new List<DataColumn>((IEnumerable<DataColumn>) collection);
          if (row1.HasErrors)
            dataColumnList.AddRange((IEnumerable<DataColumn>) row1.GetColumnsInError());
          foreach (DataColumn dataColumn in dataColumnList)
            row2.SetColumnError(dataColumn.Ordinal, "BizRule.FieldAccessRight.Hide");
          this.ultraGridDataTable.Rows.Add(row2);
        }
      }
      if (this.ultraGridSortedColumns == null)
      {
        foreach (EllieMae.EMLite.ClientServer.Reporting.ColumnInfo field in this.dashboardTemplate.Fields)
        {
          if (field.SortOrder != ColumnSortOrder.None)
          {
            this.ultraGrid.DisplayLayout.Bands[0].Columns[field.Description].SortIndicator = ColumnSortOrder.Ascending == field.SortOrder ? SortIndicator.Ascending : SortIndicator.Descending;
            break;
          }
        }
      }
      for (int index = 1; index < this.ultraGrid.DisplayLayout.Bands[0].Columns.BoundColumnsCount; ++index)
      {
        if (this.ultraGridDataTable.Columns[index].DataType == typeof (DateTime))
          this.ultraGrid.DisplayLayout.Bands[0].Columns[index].Format = "MM/dd/yyyy hh:mm:ss tt";
      }
      if (this.dashboardTemplate.MaxRows > dtReportData.Rows.Count)
      {
        this.ultraGrid.BeforeSortChange -= new BeforeSortChangeEventHandler(this.ultraGrid_BeforeSortChange);
        this.ultraGrid.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
        this.setSummaries();
      }
      else
      {
        this.ultraGrid.BeforeSortChange += new BeforeSortChangeEventHandler(this.ultraGrid_BeforeSortChange);
        this.ultraGrid.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.None;
      }
    }

    private void buildUserTable(DataTable sourceTable)
    {
      List<string> milestoneNames = new List<string>();
      if ("Dashboard.NoGroupBy" != this.dashboardTemplate.GroupByField)
      {
        foreach (DataRow row in (InternalDataCollectionBase) sourceTable.Rows)
        {
          if (row.ItemArray[1] is string str && !milestoneNames.Contains(str))
            milestoneNames.Add(str);
        }
        if ("Loan.CurrentMilestoneName" == this.dashboardTemplate.GroupByField)
          this.sortUserTableInMilestoneOrder(milestoneNames);
      }
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      stringList1.Add(this.dashboardTemplate.SummaryField1);
      stringList2.Add(this.getSummaryName(this.dashboardTemplate.SummaryField1, this.dashboardTemplate.SummaryType1));
      if ("Dashboard.NoSummary" != this.dashboardTemplate.SummaryField2)
      {
        stringList1.Add(this.dashboardTemplate.SummaryField2);
        stringList2.Add(this.getSummaryName(this.dashboardTemplate.SummaryField2, this.dashboardTemplate.SummaryType2));
      }
      if ("Dashboard.NoSummary" != this.dashboardTemplate.SummaryField3)
      {
        stringList1.Add(this.dashboardTemplate.SummaryField3);
        stringList2.Add(this.getSummaryName(this.dashboardTemplate.SummaryField3, this.dashboardTemplate.SummaryType3));
      }
      this.ultraGridDataTable.Columns.Clear();
      this.ultraGridDataTable.Columns.Add(new DataColumn("User ID", typeof (string)));
      if ("Dashboard.NoGroupBy" != this.dashboardTemplate.GroupByField)
      {
        foreach (string str1 in milestoneNames)
        {
          for (int index1 = 0; index1 < stringList1.Count; ++index1)
          {
            string criterionName = stringList1[index1];
            string str2 = stringList2[index1];
            DataColumn column = this.newDataColumn(str1 + "_*_" + str2, criterionName);
            column.Caption = str2;
            for (int index2 = 2; index2 < stringList2.Count + 2; ++index2)
            {
              try
              {
                this.ultraGridDataTable.Columns.Add(column);
                break;
              }
              catch (DuplicateNameException ex)
              {
                column.ColumnName = str1 + "_*_" + str2 + "_" + index2.ToString();
              }
            }
          }
        }
      }
      else
      {
        for (int index3 = 0; index3 < stringList1.Count; ++index3)
        {
          string criterionName = stringList1[index3];
          string columnName = stringList2[index3];
          DataColumn column = this.newDataColumn(columnName, criterionName);
          column.Caption = columnName;
          for (int index4 = 2; index4 < stringList2.Count + 2; ++index4)
          {
            try
            {
              this.ultraGridDataTable.Columns.Add(column);
              break;
            }
            catch (DuplicateNameException ex)
            {
              column.ColumnName = columnName + "_" + index4.ToString();
            }
          }
        }
      }
      DataRow row1 = (DataRow) null;
      object[] objArray = new object[this.ultraGridDataTable.Columns.Count];
      foreach (DataRow row2 in (InternalDataCollectionBase) sourceTable.Rows)
      {
        if (row1 != null && (string) objArray[0] != (string) row2.ItemArray[0])
        {
          row1.ItemArray = objArray;
          this.ultraGridDataTable.Rows.Add(row1);
          row1 = (DataRow) null;
        }
        if (row1 == null)
        {
          row1 = this.ultraGridDataTable.NewRow();
          objArray[0] = (object) (string) row2.ItemArray[0];
          for (int index = 1; index < objArray.Length; ++index)
            objArray[index] = (object) DBNull.Value;
        }
        int num1;
        int num2;
        if ("Dashboard.NoGroupBy" == this.dashboardTemplate.GroupByField)
        {
          num1 = 1;
          num2 = 1;
        }
        else
        {
          num1 = 2;
          string str = row2.ItemArray[1].ToString();
          if ("Loan.CurrentMilestoneName" == this.dashboardTemplate.GroupByField && "submittal" == str)
            str = "Submittal";
          int num3 = milestoneNames.IndexOf(str);
          if (-1 != num3)
            num2 = num3 * stringList1.Count + 1;
          else
            continue;
        }
        for (int index = num1; index < row2.ItemArray.Length; ++index)
          objArray[num2++] = row2.ItemArray[index];
      }
      if (row1 != null)
      {
        row1.ItemArray = objArray;
        this.ultraGridDataTable.Rows.Add(row1);
      }
      this.ultraGrid.InitializeLayout += new InitializeLayoutEventHandler(this.ultraGrid_InitializeLayout);
    }

    private void sortUserTableInMilestoneOrder(List<string> milestoneNames)
    {
      List<string> stringList = new List<string>((IEnumerable<string>) milestoneNames);
      milestoneNames.Clear();
      foreach (EllieMae.EMLite.Workflow.Milestone milestone in Session.StartupInfo.Milestones)
      {
        if (stringList.Contains(milestone.Name))
        {
          milestoneNames.Add("submittal" == milestone.Name ? "Submittal" : milestone.Name);
          stringList.Remove(milestone.Name);
        }
      }
      foreach (string str in stringList)
        milestoneNames.Add(str);
    }

    private void ultraGrid_InitializeLayout(object sender, InitializeLayoutEventArgs e)
    {
      UltraGridBand band = this.ultraGrid.DisplayLayout.Bands[0];
      band.Columns.ClearUnbound();
      band.Groups.Clear();
      band.Summaries.Clear();
      if ("Dashboard.NoGroupBy" != this.dashboardTemplate.GroupByField)
      {
        List<string> stringList = new List<string>();
        for (int index = 1; index < this.ultraGridDataTable.Columns.Count; ++index)
        {
          int length = this.ultraGridDataTable.Columns[index].ColumnName.IndexOf("_*_");
          string str = this.ultraGridDataTable.Columns[index].ColumnName.Substring(0, length);
          if (str != null && !stringList.Contains(str))
            stringList.Add(str);
        }
        band.Groups.Add("User ID", string.Empty);
        foreach (string str in stringList)
          band.Groups.Add(str, str).Header.Appearance.TextHAlign = HAlign.Center;
        this.ultraGrid.DisplayLayout.Bands[0].Columns[0].Group = band.Groups[0];
        int num = (this.ultraGridDataTable.Columns.Count - 1) / stringList.Count;
        for (int index = 1; index < this.ultraGrid.DisplayLayout.Bands[0].Columns.BoundColumnsCount; ++index)
          this.ultraGrid.DisplayLayout.Bands[0].Columns[index].Group = band.Groups[(index - 1) / num + 1];
      }
      this.ultraGrid.DisplayLayout.AutoFitStyle = AutoFitStyle.None;
      this.ultraGrid.DisplayLayout.Bands[0].Columns[0].Width = 100;
      for (int index = 1; index < this.ultraGrid.DisplayLayout.Bands[0].Columns.BoundColumnsCount; ++index)
        this.ultraGrid.DisplayLayout.Bands[0].Columns[index].PerformAutoResize();
      List<string> stringList1 = new List<string>();
      stringList1.Add(this.getFormatString(this.dashboardTemplate.SummaryField1));
      if ("Dashboard.NoSummary" != this.dashboardTemplate.SummaryField2)
        stringList1.Add(this.getFormatString(this.dashboardTemplate.SummaryField2));
      if ("Dashboard.NoSummary" != this.dashboardTemplate.SummaryField3)
        stringList1.Add(this.getFormatString(this.dashboardTemplate.SummaryField3));
      for (int index = 1; index < this.ultraGridDataTable.Columns.Count; ++index)
      {
        UltraGridColumn column = this.ultraGrid.DisplayLayout.Bands[0].Columns[index];
        column.Format = stringList1[(index - 1) % stringList1.Count];
        column.Header.Appearance.TextHAlign = HAlign.Right;
        column.CellAppearance.TextHAlign = HAlign.Right;
      }
      this.ultraGrid.DisplayLayout.Bands[0].Columns["User ID"].SortIndicator = SortIndicator.Ascending;
      this.ultraGrid.BeforeSortChange -= new BeforeSortChangeEventHandler(this.ultraGrid_BeforeSortChange);
      this.ultraGrid.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
      this.setSummaries();
      this.ultraGrid.InitializeLayout -= new InitializeLayoutEventHandler(this.ultraGrid_InitializeLayout);
    }

    private void ultraGrid_BeforeSortChange(object sender, BeforeSortChangeEventArgs e)
    {
      this.ultraGridSortedColumns = e.SortedColumns;
      this.ultraGrid.BeforeSortChange -= new BeforeSortChangeEventHandler(this.ultraGrid_BeforeSortChange);
      this.refreshData();
    }

    private void setSummaries()
    {
      UltraGridBand band = this.ultraGrid.DisplayLayout.Bands[0];
      if (0 < band.Columns.UnboundColumnsCount)
        return;
      UltraGridColumn ultraGridColumn = band.Columns.Add("SummaryColumn", string.Empty);
      ultraGridColumn.Width = 35;
      if (DashboardChartType.LoanTable == this.dashboardTemplate.ChartType || "Dashboard.NoGroupBy" == this.dashboardTemplate.GroupByField)
        ultraGridColumn.Header.VisiblePosition = 0;
      else
        band.Groups[0].Columns.Add(ultraGridColumn, 0);
      band.Override.SummaryFooterCaptionVisible = DefaultableBoolean.False;
      if (this.dashboardTemplate.IncludeMin)
        band.Summaries.Add(SummaryType.Minimum, ultraGridColumn).DisplayFormat = "Min";
      if (this.dashboardTemplate.IncludeMax)
        band.Summaries.Add(SummaryType.Maximum, ultraGridColumn).DisplayFormat = "Max";
      if (this.dashboardTemplate.IncludeAverage)
        band.Summaries.Add(SummaryType.Average, ultraGridColumn).DisplayFormat = "Avg";
      bool flag = DashboardChartType.UserTable == this.dashboardTemplate.ChartType && RoleInfo.All.RoleID == this.dashboardTemplate.RoleId;
      if (!flag && this.dashboardTemplate.IncludeTotal)
        band.Summaries.Add(SummaryType.Sum, ultraGridColumn).DisplayFormat = "Total";
      for (int index = 1; index < band.Columns.BoundColumnsCount; ++index)
      {
        UltraGridColumn column = band.Columns[index];
        string str1 = column.DataType.ToString().ToLower();
        if (str1.StartsWith("system."))
          str1 = str1.Substring("system.".Length);
        switch (str1)
        {
          case "byte":
          case "decimal":
          case "double":
          case "int16":
          case "int32":
          case "int64":
          case "sbyte":
          case "single":
          case "uint16":
          case "uint32":
          case "uint64":
            string str2 = "{0:" + column.Format + "}";
            if (this.dashboardTemplate.IncludeMin)
            {
              SummarySettings summarySettings = band.Summaries.Add(SummaryType.Minimum, column);
              summarySettings.DisplayFormat = str2;
              summarySettings.Appearance.TextHAlign = HAlign.Right;
            }
            if (this.dashboardTemplate.IncludeMax)
            {
              SummarySettings summarySettings = band.Summaries.Add(SummaryType.Maximum, column);
              summarySettings.DisplayFormat = str2;
              summarySettings.Appearance.TextHAlign = HAlign.Right;
            }
            if (this.dashboardTemplate.IncludeAverage)
            {
              SummarySettings summarySettings = band.Summaries.Add(SummaryType.Average, column);
              summarySettings.DisplayFormat = str2;
              summarySettings.Appearance.TextHAlign = HAlign.Right;
            }
            if (!flag && this.dashboardTemplate.IncludeTotal)
            {
              SummarySettings summarySettings = band.Summaries.Add(SummaryType.Sum, column);
              summarySettings.DisplayFormat = str2;
              summarySettings.Appearance.TextHAlign = HAlign.Right;
              break;
            }
            break;
        }
      }
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
            EllieMae.EMLite.ClientServer.Reporting.ColumnInfo columnInfo = (EllieMae.EMLite.ClientServer.Reporting.ColumnInfo) null;
            foreach (EllieMae.EMLite.ClientServer.Reporting.ColumnInfo field in this.dashboardTemplate.Fields)
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
      foreach (EllieMae.EMLite.ClientServer.Reporting.ColumnInfo field in this.dashboardTemplate.Fields)
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

    public override void ExportToExcel()
    {
      if (this.ultraGridDataTable != null)
      {
        if (this.ultraGridDataTable.Rows.Count != 0)
        {
          try
          {
            ExcelHandler excelHandler = new ExcelHandler();
            excelHandler.AddDataTable(this.ultraGridDataTable);
            excelHandler.CreateExcel();
            return;
          }
          catch (Exception ex)
          {
            Tracing.Log(UltraGridProvider.sw, "UltraGridBase", TraceLevel.Error, "Error during export: " + (object) ex);
            int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to export to Microsoft Excel.\nEnsure that you have Excel installed and it is working properly.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
      }
      int num1 = (int) Utils.Dialog((IWin32Window) this, "There is currently no data to export.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    public override void PrintSnapshot()
    {
      if (this.ultraGridDataTable == null || this.ultraGridDataTable.Rows.Count == 0)
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
      TimeFrameUtility.GetStartEndDatesForTimeFrame((DashboardTimeFrameType) this.timeFrameId, out startDate, out endDate);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Date Range: " + startDate.ToShortDateString() + " through " + endDate.ToShortDateString());
      ((UltraPrintDocument) this.printDocument).Header.TextCenter = stringBuilder.ToString();
    }

    private void ultraGrid_InitializePrint(object sender, CancelablePrintEventArgs e)
    {
      e.PrintLayout.EmptyRowSettings.ShowEmptyRows = false;
      e.PrintLayout.AutoFitStyle = AutoFitStyle.None;
      foreach (UltraGridColumn column in e.PrintLayout.Bands[0].Columns)
      {
        if ("SummaryColumn" != column.Key)
          column.PerformAutoResize();
      }
    }

    private void ultraGrid_Error(object sender, ErrorEventArgs e)
    {
      string errorText = e.DataErrorInfo.ErrorText;
    }

    private void UltraGridProvider_Resize(object sender, EventArgs e)
    {
      this.lblNoDataFound.Location = new Point(this.Left, (this.Height - this.lblNoDataFound.Height) / 2);
    }

    private void ultraGrid_BeforeCellActivate(object sender, CancelableCellEventArgs e)
    {
      if (this.ChartDataClickedHandler == null)
        return;
      string filterValue1 = string.Concat(e.Cell.Row.Cells[0].OriginalValue);
      string filterValue2;
      if (this.ultraGridDataTable.Columns.Count > e.Cell.Column.Index)
      {
        DataColumn column = this.ultraGridDataTable.Columns[e.Cell.Column.Index];
        filterValue2 = column.ColumnName.Replace("_*_" + column.Caption, "");
      }
      else
        filterValue2 = "";
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
      Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
      Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
      Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
      Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
      Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
      Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
      Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
      Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
      Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
      Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
      Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
      Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
      Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
      this.ultraGrid = new UltraGrid();
      this.lblNoDataFound = new Label();
      ((ISupportInitialize) this.ultraGrid).BeginInit();
      this.SuspendLayout();
      appearance1.BackColor = SystemColors.Window;
      appearance1.BorderColor = SystemColors.InactiveCaption;
      this.ultraGrid.DisplayLayout.Appearance = (AppearanceBase) appearance1;
      this.ultraGrid.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
      this.ultraGrid.DisplayLayout.BorderStyle = UIElementBorderStyle.None;
      this.ultraGrid.DisplayLayout.CaptionVisible = DefaultableBoolean.False;
      appearance2.BackColor = SystemColors.ActiveBorder;
      appearance2.BackColor2 = SystemColors.ControlDark;
      appearance2.BackGradientStyle = GradientStyle.Vertical;
      appearance2.BorderColor = SystemColors.Window;
      this.ultraGrid.DisplayLayout.GroupByBox.Appearance = (AppearanceBase) appearance2;
      appearance3.ForeColor = SystemColors.GrayText;
      this.ultraGrid.DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase) appearance3;
      this.ultraGrid.DisplayLayout.GroupByBox.BorderStyle = UIElementBorderStyle.Solid;
      this.ultraGrid.DisplayLayout.GroupByBox.Hidden = true;
      appearance4.BackColor = SystemColors.ControlLightLight;
      appearance4.BackColor2 = SystemColors.Control;
      appearance4.BackGradientStyle = GradientStyle.Horizontal;
      appearance4.ForeColor = SystemColors.GrayText;
      this.ultraGrid.DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase) appearance4;
      this.ultraGrid.DisplayLayout.MaxColScrollRegions = 1;
      this.ultraGrid.DisplayLayout.MaxRowScrollRegions = 1;
      appearance5.BackColor = SystemColors.Window;
      appearance5.ForeColor = SystemColors.ControlText;
      this.ultraGrid.DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase) appearance5;
      appearance6.BackColor = SystemColors.Highlight;
      appearance6.ForeColor = SystemColors.HighlightText;
      this.ultraGrid.DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase) appearance6;
      this.ultraGrid.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
      this.ultraGrid.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;
      this.ultraGrid.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
      this.ultraGrid.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;
      this.ultraGrid.DisplayLayout.Override.AllowGroupBy = DefaultableBoolean.False;
      this.ultraGrid.DisplayLayout.Override.AllowGroupMoving = AllowGroupMoving.NotAllowed;
      this.ultraGrid.DisplayLayout.Override.AllowGroupSwapping = AllowGroupSwapping.NotAllowed;
      this.ultraGrid.DisplayLayout.Override.AllowMultiCellOperations = AllowMultiCellOperation.None;
      this.ultraGrid.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.False;
      this.ultraGrid.DisplayLayout.Override.AllowRowLayoutCellSizing = RowLayoutSizing.None;
      this.ultraGrid.DisplayLayout.Override.AllowRowLayoutCellSpanSizing = GridBagLayoutAllowSpanSizing.None;
      this.ultraGrid.DisplayLayout.Override.AllowRowLayoutColMoving = GridBagLayoutAllowMoving.None;
      this.ultraGrid.DisplayLayout.Override.AllowRowLayoutLabelSizing = RowLayoutSizing.None;
      this.ultraGrid.DisplayLayout.Override.AllowRowLayoutLabelSpanSizing = GridBagLayoutAllowSpanSizing.None;
      this.ultraGrid.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;
      this.ultraGrid.DisplayLayout.Override.BorderStyleCell = UIElementBorderStyle.Dotted;
      this.ultraGrid.DisplayLayout.Override.BorderStyleRow = UIElementBorderStyle.Dotted;
      appearance7.BackColor = SystemColors.Window;
      this.ultraGrid.DisplayLayout.Override.CardAreaAppearance = (AppearanceBase) appearance7;
      appearance8.BorderColor = Color.Silver;
      appearance8.TextTrimming = TextTrimming.EllipsisCharacter;
      this.ultraGrid.DisplayLayout.Override.CellAppearance = (AppearanceBase) appearance8;
      this.ultraGrid.DisplayLayout.Override.CellClickAction = CellClickAction.CellSelect;
      this.ultraGrid.DisplayLayout.Override.CellPadding = 0;
      appearance9.BackColor = Color.FromArgb(224, 224, 224);
      appearance9.ImageAlpha = Alpha.Transparent;
      this.ultraGrid.DisplayLayout.Override.DataErrorCellAppearance = (AppearanceBase) appearance9;
      appearance10.BackColor = SystemColors.Control;
      appearance10.BackColor2 = SystemColors.ControlDark;
      appearance10.BackGradientAlignment = GradientAlignment.Element;
      appearance10.BackGradientStyle = GradientStyle.Horizontal;
      appearance10.BorderColor = SystemColors.Window;
      this.ultraGrid.DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase) appearance10;
      appearance11.TextHAlignAsString = "Left";
      this.ultraGrid.DisplayLayout.Override.HeaderAppearance = (AppearanceBase) appearance11;
      this.ultraGrid.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.SortMulti;
      this.ultraGrid.DisplayLayout.Override.HeaderStyle = HeaderStyle.WindowsXPCommand;
      this.ultraGrid.DisplayLayout.Override.MaxSelectedCells = 0;
      this.ultraGrid.DisplayLayout.Override.MaxSelectedRows = 0;
      appearance12.BackColor = SystemColors.Window;
      appearance12.BorderColor = Color.Silver;
      this.ultraGrid.DisplayLayout.Override.RowAppearance = (AppearanceBase) appearance12;
      this.ultraGrid.DisplayLayout.Override.RowSelectors = DefaultableBoolean.False;
      appearance13.BackColor = SystemColors.ControlLight;
      this.ultraGrid.DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase) appearance13;
      this.ultraGrid.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToFill;
      this.ultraGrid.DisplayLayout.ScrollStyle = ScrollStyle.Immediate;
      this.ultraGrid.DisplayLayout.ViewStyleBand = ViewStyleBand.OutlookGroupBy;
      ((Control) this.ultraGrid).Dock = DockStyle.Fill;
      ((Control) this.ultraGrid).Location = new Point(0, 0);
      ((Control) this.ultraGrid).Name = "ultraGrid";
      ((Control) this.ultraGrid).Size = new Size(497, 305);
      ((Control) this.ultraGrid).TabIndex = 1;
      ((Control) this.ultraGrid).Text = "ultraGrid1";
      this.ultraGrid.BeforeCellActivate += new CancelableCellEventHandler(this.ultraGrid_BeforeCellActivate);
      this.lblNoDataFound.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblNoDataFound.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblNoDataFound.ForeColor = Color.Red;
      this.lblNoDataFound.Location = new Point(0, 136);
      this.lblNoDataFound.Name = "lblNoDataFound";
      this.lblNoDataFound.Size = new Size(497, 33);
      this.lblNoDataFound.TabIndex = 2;
      this.lblNoDataFound.Text = "No data matched the search criteria";
      this.lblNoDataFound.TextAlign = ContentAlignment.MiddleCenter;
      this.lblNoDataFound.Visible = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.lblNoDataFound);
      this.Controls.Add((Control) this.ultraGrid);
      this.Name = nameof (UltraGridProvider);
      this.Size = new Size(497, 305);
      this.Resize += new EventHandler(this.UltraGridProvider_Resize);
      ((ISupportInitialize) this.ultraGrid).EndInit();
      this.ResumeLayout(false);
    }
  }
}
