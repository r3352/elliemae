// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.SnapshotBaseControl
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.ClientSession.Dashboard;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using Infragistics.Win.Printing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class SnapshotBaseControl : UserControl
  {
    protected DashboardForm frmDashboard;
    protected int snapshotHandle = -1;
    protected DashboardReport dashboardReport;
    protected DashboardTemplate dashboardTemplate;
    private DashboardTemplate drillDownTemplate;
    protected DashboardChartType dashboardChartType = DashboardChartType.None;
    protected LoanReportFieldDefs fieldDefinitions;
    protected DashboardDataCriteria reportDataCriteria;
    protected UltraPrintPreviewDialog dlgPrintPreview;
    protected PrintDocument printDocument;
    protected DisplayMode currentDisplayMode;
    private List<string> allFolders;
    private List<string> allButArchiveFolders;
    private IContainer components;
    private PictureBox picPrint;
    private PictureBox picZoomIn;
    protected Label lblTitle;
    private ContextMenuStrip contextMenu;
    private ToolStripMenuItem mnuItmRefresh;
    private ToolStripMenuItem mnuItmZoomIn;
    private ToolStripMenuItem mnuItmZoomOut;
    private ToolStripMenuItem mnuItmPrint;
    private ToolStripSeparator mnuItmSeparator1;
    private ToolStripMenuItem mnuItmRefreshAll;
    private ImageList imgsIcons;
    private PictureBox picExcel;
    private ToolStripMenuItem mnuItmEdit;
    private ToolStripMenuItem mnuItmExportToExcel;
    private BorderPanel pnlBorder;
    private ToolTip tipSnapshotBase;
    protected PictureBox picZoomOut;
    private ToolStripMenuItem mnuItmReplace;
    private ToolStripSeparator toolStripMenuItem1;
    protected Label lblNoDataFound;
    private Panel pnlDrillDown;
    protected GroupContainer gcDrillDownHeader;
    private StandardIconButton siBtnView;
    private CollapsibleSplitter collapsibleSplitter1;
    private StandardIconButton siBtnSplit;
    private ToolStripMenuItem mnuItmDrilldownView;
    protected Label lblDrilldownMsg;
    private StandardIconButton siBtnDefaultSnapshot;
    protected GradientPanel pnlHeader;
    protected BorderPanel pnlSnapshot;
    protected PictureBox picEdit;

    public DashboardReport DashboardReport => this.dashboardReport;

    protected DashboardTemplate DrillDownTemplate
    {
      get => this.drillDownTemplate;
      set
      {
        this.drillDownTemplate = value;
        if (this.drillDownTemplate == null)
          return;
        this.gcDrillDownHeader.Text = this.drillDownTemplate.TemplateName;
      }
    }

    protected List<string> AllFolders
    {
      get
      {
        if (this.allFolders == null)
        {
          this.allFolders = new List<string>();
          LoanFolderInfo[] allLoanFolderInfos = Session.LoanManager.GetAllLoanFolderInfos(false);
          if (allLoanFolderInfos == null || allLoanFolderInfos.Length == 0)
            return this.allFolders;
          foreach (LoanFolderInfo loanFolderInfo in allLoanFolderInfos)
          {
            if (!this.allFolders.Contains(loanFolderInfo.Name))
              this.allFolders.Add(loanFolderInfo.Name);
          }
          if (this.allButArchiveFolders == null && allLoanFolderInfos != null)
          {
            this.allButArchiveFolders = new List<string>();
            foreach (LoanFolderInfo loanFolderInfo in allLoanFolderInfos)
            {
              if (loanFolderInfo.Type != LoanFolderInfo.LoanFolderType.Archive && !this.allButArchiveFolders.Contains(loanFolderInfo.Name))
                this.allButArchiveFolders.Add(loanFolderInfo.Name);
            }
          }
        }
        return this.allFolders;
      }
    }

    protected List<string> AllButArchiveFolders
    {
      get
      {
        if (this.allButArchiveFolders == null)
        {
          this.allButArchiveFolders = new List<string>();
          LoanFolderInfo[] allLoanFolderInfos = Session.LoanManager.GetAllLoanFolderInfos(false);
          if (allLoanFolderInfos == null || allLoanFolderInfos.Length == 0)
            return this.allButArchiveFolders;
          foreach (LoanFolderInfo loanFolderInfo in allLoanFolderInfos)
          {
            if (loanFolderInfo.Type != LoanFolderInfo.LoanFolderType.Archive && !this.allButArchiveFolders.Contains(loanFolderInfo.Name))
              this.allButArchiveFolders.Add(loanFolderInfo.Name);
          }
          if (this.allFolders == null && allLoanFolderInfos != null)
          {
            this.allFolders = new List<string>();
            foreach (LoanFolderInfo loanFolderInfo in allLoanFolderInfos)
            {
              if (!this.allFolders.Contains(loanFolderInfo.Name))
                this.allFolders.Add(loanFolderInfo.Name);
            }
          }
        }
        return this.allButArchiveFolders;
      }
    }

    public string DashboardTemplatePath => this.dashboardReport.DashboardTemplatePath;

    public SnapshotBaseControl()
    {
      this.InitializeComponent();
      this.displayMode(DisplayMode.FullReport);
      this.pnlBorder.Controls.Add((Control) this.pnlSnapshot);
      this.pnlBorder.Controls.Add((Control) this.pnlHeader);
    }

    public SnapshotBaseControl(
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
      if (this.dashboardTemplate != null)
        this.DrillDownTemplate = this.frmDashboard.GetMappedSnapshot(this.dashboardTemplate.Guid);
      if (dashboardTemplate != null)
        this.fieldDefinitions = DashboardSettings.FieldDefinitions;
      if (dashboardTemplate != null && this.fieldDefinitions != null)
        return;
      this.setEditIconButton(false);
      this.setZoomInIconButton(false);
      this.setExcelIconButton(false);
      this.setPrintIconButton(false);
    }

    public SnapshotBaseControl(
      DashboardForm frmDashboard,
      int snapshotHandle,
      DashboardReport dashboardReport,
      string messageText)
      : this(frmDashboard, snapshotHandle, dashboardReport, (DashboardTemplate) null, DashboardChartType.None)
    {
      this.lblNoDataFound.Text = messageText;
      this.lblNoDataFound.Visible = true;
    }

    public virtual void RefreshData()
    {
    }

    public virtual void Zoom(bool zoomIn)
    {
      this.picZoomIn.Visible = !zoomIn;
      this.picZoomOut.Visible = zoomIn;
    }

    protected virtual void ExportToExcel()
    {
    }

    public virtual void SetPrintProperties()
    {
    }

    public virtual void ResetPrintProperties()
    {
    }

    public virtual string GetPrintTitle()
    {
      return this.lblTitle.Text + " for " + this.frmDashboard.ViewFilterDescription;
    }

    protected DashboardDataCriteria setViewCriteria(DashboardDataCriteria criteria)
    {
      if (this.frmDashboard != null)
        this.frmDashboard.SetViewCriteria(criteria);
      return criteria;
    }

    protected virtual DashboardDataCriteria getReportDataCriteria(
      DashboardTemplate template,
      bool fromDrillDownView)
    {
      this.reportDataCriteria = (DashboardDataCriteria) null;
      switch (template.ChartType)
      {
        case DashboardChartType.BarChart:
          this.reportDataCriteria = (DashboardDataCriteria) new BarChartDataCriteria();
          break;
        case DashboardChartType.TrendChart:
          this.reportDataCriteria = (DashboardDataCriteria) new TrendChartDataCriteria();
          break;
        case DashboardChartType.LoanTable:
          this.reportDataCriteria = (DashboardDataCriteria) new LoanTableDataCriteria();
          break;
        case DashboardChartType.UserTable:
          this.reportDataCriteria = (DashboardDataCriteria) new UserTableDataCriteria();
          break;
      }
      this.frmDashboard.SetViewCriteria(this.reportDataCriteria);
      this.reportDataCriteria.DataSourceType = template.DataSourceType;
      this.reportDataCriteria.ChartType = template.ChartType;
      if (template.SelectAllFolders)
        this.reportDataCriteria.FolderNames.AddRange((IEnumerable<string>) this.AllFolders);
      else if (template.SelectAllWOArchiveFolders)
        this.reportDataCriteria.FolderNames.AddRange((IEnumerable<string>) this.AllButArchiveFolders);
      else
        this.reportDataCriteria.FolderNames.AddRange((IEnumerable<string>) template.Folders);
      this.reportDataCriteria.FieldFilters.AddRange((IEnumerable<FieldFilter>) template.Filters);
      return this.reportDataCriteria;
    }

    protected void setEditIconButton(bool enable) => this.setIconButton(this.picEdit, enable);

    protected void setZoomInIconButton(bool enable) => this.setIconButton(this.picZoomIn, enable);

    protected void setExcelIconButton(bool enable) => this.setIconButton(this.picExcel, enable);

    protected void setPrintIconButton(bool enable) => this.setIconButton(this.picPrint, enable);

    private void setIconButton(PictureBox pictureBox, bool enable)
    {
      if (enable)
      {
        pictureBox.Image = this.imgsIcons.Images[pictureBox.Name];
        pictureBox.Enabled = true;
      }
      else
      {
        pictureBox.Image = this.imgsIcons.Images[pictureBox.Name + "Disabled"];
        pictureBox.Enabled = false;
      }
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

    protected void displayMode(DisplayMode displayMode)
    {
      this.pnlSnapshot.Visible = false;
      this.pnlDrillDown.Visible = false;
      this.collapsibleSplitter1.Visible = false;
      int height = this.pnlBorder.Height - this.pnlHeader.Height;
      switch (displayMode)
      {
        case DisplayMode.FullReport:
          this.collapsibleSplitter1.IsCollapsed = true;
          this.pnlSnapshot.Visible = true;
          this.pnlSnapshot.Size = new Size(this.pnlSnapshot.Width, height);
          this.siBtnSplit.StandardButtonType = StandardIconButton.ButtonType.SplitViewButton;
          this.tipSnapshotBase.SetToolTip((Control) this.siBtnSplit, "Show Split View");
          this.mnuItmDrilldownView.Text = "Show Split View";
          break;
        case DisplayMode.SplitScreen:
          this.collapsibleSplitter1.Visible = true;
          this.collapsibleSplitter1.IsCollapsed = false;
          this.pnlSnapshot.Visible = true;
          this.pnlSnapshot.Size = new Size(this.pnlSnapshot.Width, height / 2);
          this.pnlDrillDown.Size = new Size(this.pnlDrillDown.Width, height - height / 2);
          this.siBtnSplit.StandardButtonType = StandardIconButton.ButtonType.SingleViewButton;
          this.tipSnapshotBase.SetToolTip((Control) this.siBtnSplit, "Show Single View");
          this.mnuItmDrilldownView.Text = "Show Single View";
          break;
        case DisplayMode.FullDrilldown:
          this.collapsibleSplitter1.IsCollapsed = false;
          this.pnlDrillDown.Size = new Size(this.pnlDrillDown.Width, height);
          this.siBtnSplit.StandardButtonType = StandardIconButton.ButtonType.SingleViewButton;
          this.tipSnapshotBase.SetToolTip((Control) this.siBtnSplit, "Show Single View");
          this.mnuItmDrilldownView.Text = "Show Single View";
          break;
      }
      this.currentDisplayMode = displayMode;
      this.displayModeChanged();
    }

    protected virtual void displayModeChanged()
    {
    }

    protected QueryCriterion getQueryCriterion(string criterionName, string value)
    {
      return this.getQueryCriterion(criterionName, value, OrdinalMatchType.LessThanOrEquals);
    }

    protected QueryCriterion getQueryCriterion(
      string criterionName,
      string value,
      OrdinalMatchType matchType)
    {
      switch (this.getCriterionFieldType(criterionName))
      {
        case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate:
        case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsMonthDay:
        case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDateTime:
          return !Utils.IsDate((object) value) ? (QueryCriterion) null : (QueryCriterion) new DateValueCriterion(criterionName, DateTime.Parse(value), matchType);
        default:
          return (QueryCriterion) new StringValueCriterion(criterionName, value, StringMatchType.Exact);
      }
    }

    protected EllieMae.EMLite.ClientServer.Reporting.FieldTypes getCriterionFieldType(
      string criterionName)
    {
      LoanReportFieldDef fieldByCriterionName = this.fieldDefinitions.GetFieldByCriterionName(criterionName);
      return fieldByCriterionName == null ? EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNothing : fieldByCriterionName.FieldType;
    }

    protected virtual void buildDrillDownDashboard()
    {
    }

    protected void disableDrilldownView()
    {
      this.collapsibleSplitter1.IsCollapsed = true;
      this.collapsibleSplitter1.Visible = false;
      this.siBtnSplit.Enabled = false;
      this.mnuItmDrilldownView.Enabled = false;
    }

    private void editSnapshot()
    {
      using (DashboardTemplateFormDialog templateFormDialog = new DashboardTemplateFormDialog(this, true, this.frmDashboard.CurrentView.IsViewReadOnly))
      {
        if (DialogResult.OK != templateFormDialog.ShowDialog())
          return;
        this.OnAfterEditEvent(new SnapshotEventArgs(this.snapshotHandle));
      }
    }

    private void replaceSnapshot()
    {
      using (DashboardTemplateFormDialog templateFormDialog = new DashboardTemplateFormDialog(DashboardTemplateFormDialog.ProcessingMode.SelectTemplate))
      {
        if (DialogResult.OK != templateFormDialog.ShowDialog() || templateFormDialog.SelectedTemplate == null)
          return;
        this.OnAfterReplaceEvent(new SnapshotEventArgs(this.snapshotHandle, templateFormDialog.SelectedFileSystemEntry.ToString()));
      }
    }

    private void fitLabelText(Label label, string text)
    {
      using (Graphics graphics = label.CreateGraphics())
      {
        int width = label.Size.Width;
        if (Utils.FitLabelText(graphics, label, text))
          this.tipSnapshotBase.SetToolTip((Control) label, string.Empty);
        else
          this.tipSnapshotBase.SetToolTip((Control) label, Utils.FitToolTipText(graphics, label.Font, 400f, text));
      }
    }

    protected virtual void PrintSnapshot()
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
      this.fitLabelText(this.lblTitle, this.dashboardReport.ReportName);
      this.RefreshData();
    }

    private void picControlBox_MouseEnter(object sender, EventArgs e)
    {
      PictureBox pictureBox = (PictureBox) sender;
      string key = pictureBox.Name + "MouseOver";
      pictureBox.Image = this.imgsIcons.Images[key];
    }

    private void picControlBox_MouseLeave(object sender, EventArgs e)
    {
      PictureBox pictureBox = (PictureBox) sender;
      string name = pictureBox.Name;
      pictureBox.Image = this.imgsIcons.Images[name];
    }

    private void picEdit_Click(object sender, EventArgs e) => this.mnuItmEdit_Click(sender, e);

    private void picZoomIn_Click(object sender, EventArgs e) => this.mnuItmZoomIn_Click(sender, e);

    private void picZoomOut_Click(object sender, EventArgs e)
    {
      this.mnuItmZoomOut_Click(sender, e);
    }

    private void picExcel_Click(object sender, EventArgs e)
    {
      this.mnuItmExportToExcel_Click(sender, e);
    }

    private void picPrint_Click(object sender, EventArgs e) => this.mnuItmPrint_Click(sender, e);

    private void pnlSnapshot_Resize(object sender, EventArgs e)
    {
      this.lblNoDataFound.Location = new Point(this.pnlSnapshot.Left, (this.pnlSnapshot.Height - this.lblNoDataFound.Height) / 2);
    }

    private void contextMenu_Opening(object sender, CancelEventArgs e)
    {
      this.mnuItmRefresh.Enabled = this.dashboardTemplate != null;
      this.mnuItmEdit.Enabled = this.picEdit.Enabled;
      this.mnuItmZoomIn.Enabled = this.picZoomIn.Visible && this.picZoomIn.Enabled;
      this.mnuItmZoomOut.Enabled = this.picZoomOut.Visible;
      this.mnuItmExportToExcel.Enabled = this.picExcel.Enabled;
      this.mnuItmPrint.Enabled = this.picPrint.Enabled;
      this.mnuItmReplace.Enabled = true;
      this.mnuItmRefreshAll.Enabled = this.dashboardTemplate != null;
    }

    private void mnuItmRefresh_Click(object sender, EventArgs e) => this.RefreshData();

    private void mnuItmEdit_Click(object sender, EventArgs e) => this.editSnapshot();

    private void mnuItmReplace_Click(object sender, EventArgs e) => this.replaceSnapshot();

    private void mnuItmZoomIn_Click(object sender, EventArgs e)
    {
      this.OnZoomEvent(new SnapshotEventArgs(this.snapshotHandle));
      this.fitLabelText(this.lblTitle, this.dashboardReport.ReportName);
    }

    private void mnuItmZoomOut_Click(object sender, EventArgs e)
    {
      this.OnZoomEvent(new SnapshotEventArgs(this.snapshotHandle));
      this.fitLabelText(this.lblTitle, this.dashboardReport.ReportName);
    }

    private void mnuItmExportToExcel_Click(object sender, EventArgs e) => this.ExportToExcel();

    private void mnuItmPrint_Click(object sender, EventArgs e) => this.PrintSnapshot();

    private void mnuItmRefreshAll_Click(object sender, EventArgs e)
    {
      this.OnRefreshAllEvent(EventArgs.Empty);
    }

    public event AfterEditEventHandler AfterEditEvent;

    protected virtual void OnAfterEditEvent(SnapshotEventArgs e)
    {
      if (this.AfterEditEvent == null)
        return;
      this.AfterEditEvent((object) this, e);
    }

    public event AfterReplaceEventHandler AfterReplaceEvent;

    protected virtual void OnAfterReplaceEvent(SnapshotEventArgs e)
    {
      if (this.AfterReplaceEvent == null)
        return;
      this.AfterReplaceEvent((object) this, e);
    }

    public event RefreshAllEventHandler RefreshAllEvent;

    protected virtual void OnRefreshAllEvent(EventArgs e)
    {
      if (this.RefreshAllEvent == null)
        return;
      this.RefreshAllEvent((object) this, e);
    }

    public event ZoomEventHandler ZoomEvent;

    protected virtual void OnZoomEvent(SnapshotEventArgs e)
    {
      if (this.ZoomEvent == null)
        return;
      this.ZoomEvent((object) this, e);
    }

    private void siBtnView_Click(object sender, EventArgs e)
    {
      using (DashboardTemplateFormDialog templateFormDialog = new DashboardTemplateFormDialog(DashboardTemplateFormDialog.ProcessingMode.SelectTemplate))
      {
        if (DialogResult.OK != templateFormDialog.ShowDialog())
          return;
        DashboardTemplate selectedTemplate = templateFormDialog.SelectedTemplate;
        if (selectedTemplate == null)
          return;
        this.DrillDownTemplate = selectedTemplate;
        if (this.dashboardTemplate != null)
          this.frmDashboard.SetSnapshotMap(this.dashboardTemplate.Guid, templateFormDialog.SelectedFileSystemEntry.ToString());
        this.buildDrillDownDashboard();
      }
    }

    private void siBtnDrilldownView_Click(object sender, EventArgs e)
    {
      if (this.collapsibleSplitter1.Visible)
        this.displayMode(DisplayMode.FullReport);
      else
        this.displayMode(DisplayMode.SplitScreen);
    }

    private void siBtnDefaultSnapshot_Click(object sender, EventArgs e)
    {
      this.resetDrilldownSnapshot();
      this.DrillDownTemplate = this.frmDashboard.DefaultLoanTableTemplate;
      if (this.DrillDownTemplate == null)
        this.displayDrilldownNotFound();
      else
        this.buildDrillDownDashboard();
    }

    protected void displayDrilldownNotFound()
    {
      if (this.DrillDownTemplate != null)
        return;
      this.lblDrilldownMsg.Visible = true;
      this.gcDrillDownHeader.Text = "Drilldown View";
      this.lblDrilldownMsg.BringToFront();
    }

    protected virtual void resetDrilldownSnapshot()
    {
    }

    private void gcDrillDownHeader_Resize(object sender, EventArgs e)
    {
      this.lblDrilldownMsg.Location = new Point(this.gcDrillDownHeader.Left, (this.gcDrillDownHeader.Height - this.lblDrilldownMsg.Height) / 2 + 15);
    }

    protected virtual void drilldownProvider_ChartDataClickedHandler(
      string filterValue1,
      string filterValue2)
    {
      if (this.DrillDownTemplate == null || this.DrillDownTemplate.ChartType != DashboardChartType.LoanTable || DialogResult.No == Utils.Dialog((IWin32Window) this, "Do you want to open the selected loan file?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
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

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SnapshotBaseControl));
      this.pnlBorder = new BorderPanel();
      this.pnlSnapshot = new BorderPanel();
      this.lblNoDataFound = new Label();
      this.pnlHeader = new GradientPanel();
      this.siBtnSplit = new StandardIconButton();
      this.lblTitle = new Label();
      this.picExcel = new PictureBox();
      this.picZoomOut = new PictureBox();
      this.picZoomIn = new PictureBox();
      this.picPrint = new PictureBox();
      this.picEdit = new PictureBox();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.pnlDrillDown = new Panel();
      this.gcDrillDownHeader = new GroupContainer();
      this.siBtnDefaultSnapshot = new StandardIconButton();
      this.lblDrilldownMsg = new Label();
      this.siBtnView = new StandardIconButton();
      this.contextMenu = new ContextMenuStrip(this.components);
      this.mnuItmRefresh = new ToolStripMenuItem();
      this.mnuItmEdit = new ToolStripMenuItem();
      this.mnuItmZoomIn = new ToolStripMenuItem();
      this.mnuItmZoomOut = new ToolStripMenuItem();
      this.mnuItmDrilldownView = new ToolStripMenuItem();
      this.mnuItmExportToExcel = new ToolStripMenuItem();
      this.mnuItmPrint = new ToolStripMenuItem();
      this.mnuItmSeparator1 = new ToolStripSeparator();
      this.mnuItmReplace = new ToolStripMenuItem();
      this.toolStripMenuItem1 = new ToolStripSeparator();
      this.mnuItmRefreshAll = new ToolStripMenuItem();
      this.imgsIcons = new ImageList(this.components);
      this.tipSnapshotBase = new ToolTip(this.components);
      this.pnlBorder.SuspendLayout();
      this.pnlSnapshot.SuspendLayout();
      this.pnlHeader.SuspendLayout();
      ((ISupportInitialize) this.siBtnSplit).BeginInit();
      ((ISupportInitialize) this.picExcel).BeginInit();
      ((ISupportInitialize) this.picZoomOut).BeginInit();
      ((ISupportInitialize) this.picZoomIn).BeginInit();
      ((ISupportInitialize) this.picPrint).BeginInit();
      ((ISupportInitialize) this.picEdit).BeginInit();
      this.pnlDrillDown.SuspendLayout();
      this.gcDrillDownHeader.SuspendLayout();
      ((ISupportInitialize) this.siBtnDefaultSnapshot).BeginInit();
      ((ISupportInitialize) this.siBtnView).BeginInit();
      this.contextMenu.SuspendLayout();
      this.SuspendLayout();
      this.pnlBorder.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlBorder.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlBorder.Controls.Add((Control) this.collapsibleSplitter1);
      this.pnlBorder.Controls.Add((Control) this.pnlDrillDown);
      this.pnlBorder.Location = new Point(3, 3);
      this.pnlBorder.Name = "pnlBorder";
      this.pnlBorder.Size = new Size(594, 394);
      this.pnlBorder.TabIndex = 4;
      this.pnlSnapshot.AutoScroll = true;
      this.pnlSnapshot.BackColor = Color.WhiteSmoke;
      this.pnlSnapshot.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.pnlSnapshot.Controls.Add((Control) this.lblNoDataFound);
      this.pnlSnapshot.Dock = DockStyle.Fill;
      this.pnlSnapshot.Location = new Point(1, 27);
      this.pnlSnapshot.Name = "pnlSnapshot";
      this.pnlSnapshot.Size = new Size(592, 173);
      this.pnlSnapshot.TabIndex = 15;
      this.lblNoDataFound.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblNoDataFound.BackColor = Color.Transparent;
      this.lblNoDataFound.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblNoDataFound.ForeColor = Color.Red;
      this.lblNoDataFound.Location = new Point(3, 36);
      this.lblNoDataFound.Name = "lblNoDataFound";
      this.lblNoDataFound.Size = new Size(584, 67);
      this.lblNoDataFound.TabIndex = 0;
      this.lblNoDataFound.Text = "No data matched the search criteria";
      this.lblNoDataFound.TextAlign = ContentAlignment.MiddleCenter;
      this.lblNoDataFound.Visible = false;
      this.pnlHeader.Borders = AnchorStyles.None;
      this.pnlHeader.Controls.Add((Control) this.siBtnSplit);
      this.pnlHeader.Controls.Add((Control) this.lblTitle);
      this.pnlHeader.Controls.Add((Control) this.picExcel);
      this.pnlHeader.Controls.Add((Control) this.picZoomOut);
      this.pnlHeader.Controls.Add((Control) this.picZoomIn);
      this.pnlHeader.Controls.Add((Control) this.picPrint);
      this.pnlHeader.Controls.Add((Control) this.picEdit);
      this.pnlHeader.Dock = DockStyle.Top;
      this.pnlHeader.Location = new Point(1, 1);
      this.pnlHeader.Name = "pnlHeader";
      this.pnlHeader.Size = new Size(592, 26);
      this.pnlHeader.TabIndex = 14;
      this.siBtnSplit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnSplit.BackColor = Color.Transparent;
      this.siBtnSplit.Location = new Point(508, 6);
      this.siBtnSplit.Name = "siBtnSplit";
      this.siBtnSplit.Size = new Size(16, 16);
      this.siBtnSplit.StandardButtonType = StandardIconButton.ButtonType.SingleViewButton;
      this.siBtnSplit.TabIndex = 12;
      this.siBtnSplit.TabStop = false;
      this.tipSnapshotBase.SetToolTip((Control) this.siBtnSplit, "Drilldown View");
      this.siBtnSplit.Click += new EventHandler(this.siBtnDrilldownView_Click);
      this.lblTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblTitle.AutoEllipsis = true;
      this.lblTitle.BackColor = Color.Transparent;
      this.lblTitle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTitle.Location = new Point(4, 6);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(343, 16);
      this.lblTitle.TabIndex = 6;
      this.lblTitle.Text = "Title...";
      this.picExcel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picExcel.BackColor = SystemColors.Control;
      this.picExcel.Image = (Image) componentResourceManager.GetObject("picExcel.Image");
      this.picExcel.Location = new Point(550, 6);
      this.picExcel.Name = "picExcel";
      this.picExcel.Size = new Size(16, 16);
      this.picExcel.TabIndex = 9;
      this.picExcel.TabStop = false;
      this.tipSnapshotBase.SetToolTip((Control) this.picExcel, "Export to Excel");
      this.picExcel.MouseLeave += new EventHandler(this.picControlBox_MouseLeave);
      this.picExcel.Click += new EventHandler(this.picExcel_Click);
      this.picExcel.MouseEnter += new EventHandler(this.picControlBox_MouseEnter);
      this.picZoomOut.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picZoomOut.BackColor = Color.Transparent;
      this.picZoomOut.Image = (Image) componentResourceManager.GetObject("picZoomOut.Image");
      this.picZoomOut.Location = new Point(529, 6);
      this.picZoomOut.Name = "picZoomOut";
      this.picZoomOut.Size = new Size(16, 16);
      this.picZoomOut.TabIndex = 7;
      this.picZoomOut.TabStop = false;
      this.tipSnapshotBase.SetToolTip((Control) this.picZoomOut, "Zoom Out");
      this.picZoomOut.Visible = false;
      this.picZoomOut.MouseLeave += new EventHandler(this.picControlBox_MouseLeave);
      this.picZoomOut.Click += new EventHandler(this.picZoomOut_Click);
      this.picZoomOut.MouseEnter += new EventHandler(this.picControlBox_MouseEnter);
      this.picZoomIn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picZoomIn.BackColor = Color.Transparent;
      this.picZoomIn.Image = (Image) componentResourceManager.GetObject("picZoomIn.Image");
      this.picZoomIn.Location = new Point(529, 6);
      this.picZoomIn.Name = "picZoomIn";
      this.picZoomIn.Size = new Size(16, 16);
      this.picZoomIn.TabIndex = 4;
      this.picZoomIn.TabStop = false;
      this.tipSnapshotBase.SetToolTip((Control) this.picZoomIn, "Zoom In");
      this.picZoomIn.MouseLeave += new EventHandler(this.picControlBox_MouseLeave);
      this.picZoomIn.Click += new EventHandler(this.picZoomIn_Click);
      this.picZoomIn.MouseEnter += new EventHandler(this.picControlBox_MouseEnter);
      this.picPrint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picPrint.BackColor = Color.Transparent;
      this.picPrint.Image = (Image) componentResourceManager.GetObject("picPrint.Image");
      this.picPrint.Location = new Point(571, 6);
      this.picPrint.Name = "picPrint";
      this.picPrint.Size = new Size(16, 16);
      this.picPrint.TabIndex = 3;
      this.picPrint.TabStop = false;
      this.tipSnapshotBase.SetToolTip((Control) this.picPrint, "Print Snapshot");
      this.picPrint.MouseLeave += new EventHandler(this.picControlBox_MouseLeave);
      this.picPrint.Click += new EventHandler(this.picPrint_Click);
      this.picPrint.MouseEnter += new EventHandler(this.picControlBox_MouseEnter);
      this.picEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picEdit.BackColor = Color.Transparent;
      this.picEdit.Image = (Image) componentResourceManager.GetObject("picEdit.Image");
      this.picEdit.Location = new Point(487, 6);
      this.picEdit.Name = "picEdit";
      this.picEdit.Size = new Size(16, 16);
      this.picEdit.TabIndex = 8;
      this.picEdit.TabStop = false;
      this.tipSnapshotBase.SetToolTip((Control) this.picEdit, "Edit Snapshot");
      this.picEdit.MouseLeave += new EventHandler(this.picControlBox_MouseLeave);
      this.picEdit.Click += new EventHandler(this.picEdit_Click);
      this.picEdit.MouseEnter += new EventHandler(this.picControlBox_MouseEnter);
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.pnlDrillDown;
      this.collapsibleSplitter1.Dock = DockStyle.Bottom;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(1, 200);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 12;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.pnlDrillDown.Controls.Add((Control) this.gcDrillDownHeader);
      this.pnlDrillDown.Dock = DockStyle.Bottom;
      this.pnlDrillDown.Location = new Point(1, 207);
      this.pnlDrillDown.Name = "pnlDrillDown";
      this.pnlDrillDown.Size = new Size(592, 187);
      this.pnlDrillDown.TabIndex = 13;
      this.gcDrillDownHeader.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gcDrillDownHeader.Controls.Add((Control) this.siBtnDefaultSnapshot);
      this.gcDrillDownHeader.Controls.Add((Control) this.lblDrilldownMsg);
      this.gcDrillDownHeader.Controls.Add((Control) this.siBtnView);
      this.gcDrillDownHeader.Dock = DockStyle.Fill;
      this.gcDrillDownHeader.Location = new Point(0, 0);
      this.gcDrillDownHeader.Name = "gcDrillDownHeader";
      this.gcDrillDownHeader.Size = new Size(592, 187);
      this.gcDrillDownHeader.TabIndex = 0;
      this.gcDrillDownHeader.Text = "Drilldown View";
      this.gcDrillDownHeader.Resize += new EventHandler(this.gcDrillDownHeader_Resize);
      this.siBtnDefaultSnapshot.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnDefaultSnapshot.BackColor = Color.Transparent;
      this.siBtnDefaultSnapshot.Location = new Point(571, 6);
      this.siBtnDefaultSnapshot.Name = "siBtnDefaultSnapshot";
      this.siBtnDefaultSnapshot.Size = new Size(16, 16);
      this.siBtnDefaultSnapshot.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.siBtnDefaultSnapshot.TabIndex = 2;
      this.siBtnDefaultSnapshot.TabStop = false;
      this.tipSnapshotBase.SetToolTip((Control) this.siBtnDefaultSnapshot, "Use Default Loan Snapshot");
      this.siBtnDefaultSnapshot.Click += new EventHandler(this.siBtnDefaultSnapshot_Click);
      this.lblDrilldownMsg.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblDrilldownMsg.BackColor = Color.Transparent;
      this.lblDrilldownMsg.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblDrilldownMsg.ForeColor = Color.Red;
      this.lblDrilldownMsg.Location = new Point(3, 74);
      this.lblDrilldownMsg.Name = "lblDrilldownMsg";
      this.lblDrilldownMsg.Size = new Size(586, 55);
      this.lblDrilldownMsg.TabIndex = 1;
      this.lblDrilldownMsg.Text = "Default snapshot cannot be found.";
      this.lblDrilldownMsg.TextAlign = ContentAlignment.MiddleCenter;
      this.lblDrilldownMsg.Visible = false;
      this.siBtnView.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnView.BackColor = Color.Transparent;
      this.siBtnView.Location = new Point(550, 6);
      this.siBtnView.Name = "siBtnView";
      this.siBtnView.Size = new Size(16, 16);
      this.siBtnView.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.siBtnView.TabIndex = 0;
      this.siBtnView.TabStop = false;
      this.tipSnapshotBase.SetToolTip((Control) this.siBtnView, "Select Snapshot");
      this.siBtnView.Click += new EventHandler(this.siBtnView_Click);
      this.contextMenu.Items.AddRange(new ToolStripItem[11]
      {
        (ToolStripItem) this.mnuItmRefresh,
        (ToolStripItem) this.mnuItmEdit,
        (ToolStripItem) this.mnuItmZoomIn,
        (ToolStripItem) this.mnuItmZoomOut,
        (ToolStripItem) this.mnuItmDrilldownView,
        (ToolStripItem) this.mnuItmExportToExcel,
        (ToolStripItem) this.mnuItmPrint,
        (ToolStripItem) this.mnuItmSeparator1,
        (ToolStripItem) this.mnuItmReplace,
        (ToolStripItem) this.toolStripMenuItem1,
        (ToolStripItem) this.mnuItmRefreshAll
      });
      this.contextMenu.Name = "contextMenu";
      this.contextMenu.Size = new Size(177, 214);
      this.contextMenu.Opening += new CancelEventHandler(this.contextMenu_Opening);
      this.mnuItmRefresh.Name = "mnuItmRefresh";
      this.mnuItmRefresh.Size = new Size(176, 22);
      this.mnuItmRefresh.Text = "&Refresh";
      this.mnuItmRefresh.Click += new EventHandler(this.mnuItmRefresh_Click);
      this.mnuItmEdit.Name = "mnuItmEdit";
      this.mnuItmEdit.Size = new Size(176, 22);
      this.mnuItmEdit.Text = "&Edit";
      this.mnuItmEdit.Click += new EventHandler(this.mnuItmEdit_Click);
      this.mnuItmZoomIn.Name = "mnuItmZoomIn";
      this.mnuItmZoomIn.Size = new Size(176, 22);
      this.mnuItmZoomIn.Text = "Zoom &In";
      this.mnuItmZoomIn.Click += new EventHandler(this.mnuItmZoomIn_Click);
      this.mnuItmZoomOut.Name = "mnuItmZoomOut";
      this.mnuItmZoomOut.Size = new Size(176, 22);
      this.mnuItmZoomOut.Text = "Zoom &Out";
      this.mnuItmZoomOut.Click += new EventHandler(this.mnuItmZoomOut_Click);
      this.mnuItmDrilldownView.Name = "mnuItmDrilldownView";
      this.mnuItmDrilldownView.Size = new Size(176, 22);
      this.mnuItmDrilldownView.Text = "Drilldown View";
      this.mnuItmDrilldownView.Click += new EventHandler(this.siBtnDrilldownView_Click);
      this.mnuItmExportToExcel.Name = "mnuItmExportToExcel";
      this.mnuItmExportToExcel.Size = new Size(176, 22);
      this.mnuItmExportToExcel.Text = "Export to E&xcel";
      this.mnuItmExportToExcel.Click += new EventHandler(this.mnuItmExportToExcel_Click);
      this.mnuItmPrint.Name = "mnuItmPrint";
      this.mnuItmPrint.Size = new Size(176, 22);
      this.mnuItmPrint.Text = "&Print";
      this.mnuItmPrint.Click += new EventHandler(this.mnuItmPrint_Click);
      this.mnuItmSeparator1.Name = "mnuItmSeparator1";
      this.mnuItmSeparator1.Size = new Size(173, 6);
      this.mnuItmReplace.Name = "mnuItmReplace";
      this.mnuItmReplace.Size = new Size(176, 22);
      this.mnuItmReplace.Text = "Repla&ce";
      this.mnuItmReplace.Click += new EventHandler(this.mnuItmReplace_Click);
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new Size(173, 6);
      this.mnuItmRefreshAll.Name = "mnuItmRefreshAll";
      this.mnuItmRefreshAll.ShortcutKeys = Keys.A | Keys.Control;
      this.mnuItmRefreshAll.Size = new Size(176, 22);
      this.mnuItmRefreshAll.Text = "Refresh &All";
      this.mnuItmRefreshAll.Click += new EventHandler(this.mnuItmRefreshAll_Click);
      this.imgsIcons.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgsIcons.ImageStream");
      this.imgsIcons.TransparentColor = Color.Transparent;
      this.imgsIcons.Images.SetKeyName(0, "picPrint");
      this.imgsIcons.Images.SetKeyName(1, "picPrintDisabled");
      this.imgsIcons.Images.SetKeyName(2, "picPrintMouseOver");
      this.imgsIcons.Images.SetKeyName(3, "picZoomIn");
      this.imgsIcons.Images.SetKeyName(4, "picZoomInDisabled");
      this.imgsIcons.Images.SetKeyName(5, "picZoomInMouseOver");
      this.imgsIcons.Images.SetKeyName(6, "picZoomOut");
      this.imgsIcons.Images.SetKeyName(7, "picZoomOutMouseOver");
      this.imgsIcons.Images.SetKeyName(8, "picEdit");
      this.imgsIcons.Images.SetKeyName(9, "picEditDisabled");
      this.imgsIcons.Images.SetKeyName(10, "picEditMouseOver");
      this.imgsIcons.Images.SetKeyName(11, "picExcel");
      this.imgsIcons.Images.SetKeyName(12, "picExcelDisabled");
      this.imgsIcons.Images.SetKeyName(13, "picExcelMouseOver");
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Transparent;
      this.ContextMenuStrip = this.contextMenu;
      this.Controls.Add((Control) this.pnlBorder);
      this.Margin = new Padding(0);
      this.Name = nameof (SnapshotBaseControl);
      this.Size = new Size(600, 400);
      this.Load += new EventHandler(this.SnapshotBaseControl_Load);
      this.pnlBorder.ResumeLayout(false);
      this.pnlSnapshot.ResumeLayout(false);
      this.pnlHeader.ResumeLayout(false);
      ((ISupportInitialize) this.siBtnSplit).EndInit();
      ((ISupportInitialize) this.picExcel).EndInit();
      ((ISupportInitialize) this.picZoomOut).EndInit();
      ((ISupportInitialize) this.picZoomIn).EndInit();
      ((ISupportInitialize) this.picPrint).EndInit();
      ((ISupportInitialize) this.picEdit).EndInit();
      this.pnlDrillDown.ResumeLayout(false);
      this.gcDrillDownHeader.ResumeLayout(false);
      ((ISupportInitialize) this.siBtnDefaultSnapshot).EndInit();
      ((ISupportInitialize) this.siBtnView).EndInit();
      this.contextMenu.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
