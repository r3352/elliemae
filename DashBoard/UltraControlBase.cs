// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.UltraControlBase
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.ClientSession.Dashboard;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using Infragistics.Win.Printing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class UltraControlBase : UserControl
  {
    protected DashboardForm frmDashboard;
    protected int snapshotHandle = -1;
    protected DashboardReport dashboardReport;
    protected DashboardTemplate dashboardTemplate;
    protected DashboardChartType dashboardChartType = DashboardChartType.None;
    protected LoanReportFieldDefs fieldDefinitions;
    protected DashboardDataCriteria reportDataCriteria;
    protected PrintDocument printDocument;
    protected UltraPrintPreviewDialog dlgPrintPreview;
    private IContainer components;

    public UltraControlBase() => this.InitializeComponent();

    public UltraControlBase(
      DashboardForm frmDashboard,
      int snapshotHandle,
      DashboardReport dashboardReport,
      DashboardTemplate dashboardTemplate,
      DashboardChartType dashboardChartType)
      : this()
    {
      this.frmDashboard = frmDashboard;
      this.snapshotHandle = snapshotHandle;
      this.dashboardReport = dashboardReport;
      this.dashboardTemplate = dashboardTemplate;
      this.dashboardChartType = dashboardChartType;
      if (dashboardTemplate == null)
        return;
      this.fieldDefinitions = DashboardSettings.FieldDefinitions;
    }

    public virtual void RefreshData(DashboardDataCriteria reportCriteria)
    {
    }

    public virtual bool NoDataFound() => false;

    protected virtual void refreshData() => this.RefreshData(this.reportDataCriteria);

    protected void validateFieldAccess(DashboardTemplate dTemplate, DashboardDataCriteria criteria)
    {
      List<string> stringList = new List<string>();
      foreach (FieldFilter fieldFilter in (List<FieldFilter>) criteria.FieldFilters)
      {
        if (this.fieldDefinitions.GetFieldByCriterionName(fieldFilter.CriterionName) == null)
          stringList.Add(fieldFilter.FieldDescription);
      }
      foreach (ColumnInfo field in dTemplate.Fields)
      {
        if (this.fieldDefinitions.GetFieldByCriterionName(field.CriterionName) == null)
          stringList.Add(field.Description);
      }
      if (dTemplate.GroupByField != "" && dTemplate.GroupByField != "Dashboard.NoGroupBy" && this.fieldDefinitions.GetFieldByCriterionName(dTemplate.GroupByField) == null)
        stringList.Add(dTemplate.GroupByField);
      if (stringList.Count != 0)
        throw new Exception("Violation of field access right: The snapshot '" + dTemplate.TemplateName + "' contains fields or filters that are no longer available for use. The affected fields are:" + Environment.NewLine + Environment.NewLine + string.Join(Environment.NewLine, stringList.ToArray()) + Environment.NewLine + "You must remove these fields from the snapshot in order to run it.");
    }

    public virtual void Zoom(bool zoomIn)
    {
    }

    public virtual void ExportToExcel()
    {
    }

    public virtual void SetPrintProperties()
    {
    }

    public virtual void ResetPrintProperties()
    {
    }

    protected List<string> getFieldCriterionNames(List<ColumnInfo> columns)
    {
      List<string> fieldCriterionNames = new List<string>();
      foreach (ColumnInfo column in columns)
        fieldCriterionNames.Add(column.CriterionName);
      return fieldCriterionNames;
    }

    protected string getFormatString(string criterionName)
    {
      string formatString = string.Empty;
      switch (this.getFieldFormat(criterionName))
      {
        case FieldFormat.INTEGER:
          formatString = "#,##0";
          break;
        case FieldFormat.DECIMAL_1:
          formatString = "#,##0.0";
          break;
        case FieldFormat.DECIMAL_2:
          formatString = "#,##0.00";
          break;
        case FieldFormat.DECIMAL_3:
        case FieldFormat.DECIMAL:
          formatString = "#,##0.000";
          break;
        case FieldFormat.DECIMAL_4:
          formatString = "#,##0.0000";
          break;
        case FieldFormat.DECIMAL_6:
          formatString = "#,##0.000000";
          break;
        case FieldFormat.DECIMAL_5:
          formatString = "#,##0.00000";
          break;
        case FieldFormat.DECIMAL_7:
          formatString = "#,##0.0000000";
          break;
        case FieldFormat.DECIMAL_10:
          formatString = "#,##0.0000000000";
          break;
        case FieldFormat.DATE:
          formatString = "d";
          break;
        case FieldFormat.DATETIME:
          formatString = "dd/MM/yyyy HH:mm:ss";
          break;
      }
      return formatString;
    }

    protected FieldFormat getFieldFormat(string criterionName)
    {
      FieldFormat fieldFormat = FieldFormat.STRING;
      if ("Dashboard.LoanCount" == criterionName)
        return FieldFormat.INTEGER;
      LoanReportFieldDef fieldByCriterionName = this.fieldDefinitions.GetFieldByCriterionName(criterionName);
      if (fieldByCriterionName != null)
      {
        switch (fieldByCriterionName.ReportingDatabaseColumnType)
        {
          case ReportingDatabaseColumnType.Text:
            fieldFormat = FieldFormat.STRING;
            break;
          case ReportingDatabaseColumnType.Numeric:
            fieldFormat = FieldFormat.DECIMAL;
            if (fieldByCriterionName.FieldDefinition != null)
            {
              switch (fieldByCriterionName.FieldDefinition.Format)
              {
                case FieldFormat.INTEGER:
                case FieldFormat.DECIMAL_1:
                case FieldFormat.DECIMAL_2:
                case FieldFormat.DECIMAL_3:
                case FieldFormat.DECIMAL_4:
                case FieldFormat.DECIMAL_6:
                case FieldFormat.DECIMAL_5:
                case FieldFormat.DECIMAL_7:
                case FieldFormat.DECIMAL_10:
                  fieldFormat = fieldByCriterionName.FieldDefinition.Format;
                  break;
              }
            }
            else
              break;
            break;
          case ReportingDatabaseColumnType.Date:
            fieldFormat = FieldFormat.DATE;
            break;
          case ReportingDatabaseColumnType.DateTime:
            fieldFormat = FieldFormat.DATETIME;
            break;
        }
      }
      return fieldFormat;
    }

    protected string getSummaryName(string summaryField, ColumnSummaryType summaryType)
    {
      string empty = string.Empty;
      return !("Dashboard.LoanCount" == summaryField) ? (!("Loan.TotalLoanAmount" == summaryField) ? this.getFieldName(summaryField) + " (" + summaryType.ToString() + ")" : "Total Loan Amount") : "Number of Loans";
    }

    protected string getFieldName(string criterionName)
    {
      string empty = string.Empty;
      LoanReportFieldDef fieldByCriterionName = this.fieldDefinitions.GetFieldByCriterionName(criterionName);
      return fieldByCriterionName == null ? criterionName.Substring(criterionName.LastIndexOf('.') + 1) : fieldByCriterionName.Name;
    }

    private void editSnapshot()
    {
      using (DashboardTemplateFormDialog templateFormDialog = new DashboardTemplateFormDialog((SnapshotBaseControl) null, true, this.frmDashboard.CurrentView.IsViewReadOnly))
        templateFormDialog.ShowDialog();
    }

    private void replaceSnapshot()
    {
      using (DashboardTemplateFormDialog templateFormDialog = new DashboardTemplateFormDialog(DashboardTemplateFormDialog.ProcessingMode.SelectTemplate))
      {
        if (DialogResult.OK != templateFormDialog.ShowDialog() || templateFormDialog.SelectedTemplate == null)
          return;
        FileSystemEntry selectedFileSystemEntry = templateFormDialog.SelectedFileSystemEntry;
      }
    }

    public virtual void PrintSnapshot()
    {
      this.SetPrintProperties();
      using (this.dlgPrintPreview = new UltraPrintPreviewDialog())
      {
        this.dlgPrintPreview.Document = this.printDocument;
        ((Control) this.dlgPrintPreview).Width = 620;
        ((Control) this.dlgPrintPreview).Height = 700;
        this.dlgPrintPreview.PreviewSettings.Zoom = 0.5;
        int num = (int) ((Form) this.dlgPrintPreview).ShowDialog((IWin32Window) this);
      }
      this.ResetPrintProperties();
    }

    private void SnapshotBaseControl_Load(object sender, EventArgs e)
    {
      if (this.DesignMode)
        return;
      this.refreshData();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.AutoScaleMode = AutoScaleMode.Font;
    }

    public delegate void UpdateZoomInIconButton(bool enable);

    public delegate void UpdateExcelIconButton(bool enable);

    public delegate void UpdatePrintIconButton(bool enable);

    public delegate void ChartDataClicked(string filterValue1, string filterValue2);
  }
}
