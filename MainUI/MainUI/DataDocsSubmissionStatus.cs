// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.DataDocsSubmissionStatus
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.DataDocs;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class DataDocsSubmissionStatus : Form
  {
    private const string className = "DataDocsSubmissionStatus";
    protected static string sw = Tracing.SwOutsideLoan;
    private GridViewStatusFilterManager _submittedFilterManager;
    private FieldFilterList _filter;
    private Random random = new Random();
    private ICursor _submissionsCursor;
    private Sessions.Session _session;
    private int _totalItemsCount;
    private bool _intialized;
    private bool _isFilterInitialized;
    private IContainer components;
    private GradientPanel gpTopPanel;
    private Label label1;
    private TabControl tabControl;
    private TabPage tpSubmitted;
    private GroupContainer groupContainer2;
    private StandardIconButton btnSubmittedRefresh;
    private PageListNavigator navSubmitted;
    private GradientPanel gpSubmittedFilter;
    private Button btnClose;
    private Label lblSubmittedFilter;
    private Label lblSubmittedFilterLabel;
    private GridView gvSubmitted;
    private Button btnSubClear;
    private ToolTip toolTip;
    private ProgressBar progressBar;
    private BackgroundWorker submissionsWorker;
    private Label lblMessage;

    public DataDocsSubmissionStatus()
    {
      this.InitializeComponent();
      this.SetupUI();
    }

    public DataDocsSubmissionStatus(Sessions.Session session)
    {
      this._session = session;
      this.InitializeComponent();
      this.InitializeCursors();
      this.HandleAuditColumns();
    }

    private void DataDocsSubmissionStatus_Activated(object sender, EventArgs e)
    {
      if (this._intialized)
        return;
      this._intialized = true;
      this.GetSubmissionsAsync();
    }

    private void InitializeAndStartBackgroundWorker()
    {
      if (this.submissionsWorker != null)
      {
        if (this.submissionsWorker.IsBusy)
          return;
        this.submissionsWorker.Dispose();
      }
      this.progressBar.Visible = true;
      this.lblMessage.Visible = true;
      this.progressBar.Value = 50;
      this.submissionsWorker = new BackgroundWorker();
      this.submissionsWorker.WorkerSupportsCancellation = true;
      this.submissionsWorker.WorkerReportsProgress = true;
      this.submissionsWorker.DoWork += new DoWorkEventHandler(this._submissionsWorker_DoWork);
      this.submissionsWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this._submissionsWorker_RunWorkerCompleted);
      this.submissionsWorker.ProgressChanged += new ProgressChangedEventHandler(this._submissionsWorker_ProgressChanged);
      this.submissionsWorker.RunWorkerAsync();
    }

    private void _submissionsWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      this.progressBar.Value = e.ProgressPercentage;
    }

    private void _submissionsWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      this.progressBar.Visible = false;
      this.lblMessage.Visible = false;
      this.SetupUI();
    }

    private void _submissionsWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      this._totalItemsCount = this._submissionsCursor.GetItemCount();
    }

    private void GetSubmissionsAsync()
    {
      DialogResult dialogResult = DialogResult.None;
      try
      {
        using (ProgressDialog progressDialog = new ProgressDialog("Retrieving submissions. Please wait...", new AsynchronousProcess(this.AsynchronousProcessMethod), new object(), false))
          dialogResult = progressDialog.ShowDialog((IWin32Window) this);
        if (dialogResult != DialogResult.OK)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Error retrieving submissions. Please try again. If problem persists please contact your system administrator", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
          this.SetupUI();
      }
      catch
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error retrieving submissions. Please try again. If problem persists please contact your system administrator", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private DialogResult AsynchronousProcessMethod(object notUsed, IProgressFeedback feedback)
    {
      feedback.SetFeedback("Overall process", "Retrieving submissions", 25);
      this._totalItemsCount = this._submissionsCursor.GetItemCount();
      feedback.SetFeedback("Overall process", "Completed retrieving submissions", 100);
      return DialogResult.OK;
    }

    private void SetupUI()
    {
      this.SetupFilters();
      this.SetupTabPages(this.tpSubmitted);
      this.SetupToolTips();
    }

    private void HandleAuditColumns()
    {
      if (this._session.StartupInfo.AllowDataAndDocsReview)
        return;
      GVColumn column1 = this.gvSubmitted.Columns[9];
      GVColumn column2 = this.gvSubmitted.Columns[10];
      GVColumn column3 = this.gvSubmitted.Columns[11];
      this.gvSubmitted.Columns.Remove(column1);
      this.gvSubmitted.Columns.Remove(column2);
      this.gvSubmitted.Columns.Remove(column3);
    }

    private void SetupToolTips()
    {
      this.gvSubmitted.Hover += new HoverEventDelegate(this.gvSubmitted_Hover);
    }

    private void gvSubmitted_Hover(object source, HoverEventArgs e)
    {
      if (e.ItemInfo.ColumnIndex == 9)
        this.toolTip.SetToolTip((Control) this.gvSubmitted, "Validation Failed");
      if (e.ItemInfo.ColumnIndex == 10)
        this.toolTip.SetToolTip((Control) this.gvSubmitted, "Validation Inconclusive");
      if (e.ItemInfo.ColumnIndex != 11)
        return;
      this.toolTip.SetToolTip((Control) this.gvSubmitted, "Validation Passed");
    }

    private void SetupFilters()
    {
      if (!this._isFilterInitialized)
      {
        this._isFilterInitialized = true;
        this._submittedFilterManager = new GridViewStatusFilterManager(this._session, this.gvSubmitted);
        this._submittedFilterManager.AllowTextMatchForListOptions = true;
        this._submittedFilterManager.CreateColumnFilter(0, FieldFormat.STRING);
        this._submittedFilterManager.CreateColumnFilter(1, FieldFormat.STRING);
        this._submittedFilterManager.CreateColumnFilter(2, FieldFormat.STRING);
        this._submittedFilterManager.CreateColumnFilter(3, FieldFormat.STRING);
        this._submittedFilterManager.CreateColumnFilter(4, FieldFormat.STRING);
        this._submittedFilterManager.CreateColumnFilter(5, FieldFormat.STRING);
        this._submittedFilterManager.CreateColumnFilter(6, FieldFormat.DATETIME);
        this._submittedFilterManager.CreateColumnFilter(7, FieldFormat.STRING, DataDocsConstants.GetAllDeliveryStatusList(), true);
        this._submittedFilterManager.CreateColumnFilter(8, FieldFormat.DATETIME);
        if (this._session.StartupInfo.AllowDataAndDocsReview)
        {
          this._submittedFilterManager.CreateColumnFilter(9, FieldFormat.INTEGER);
          this._submittedFilterManager.CreateColumnFilter(10, FieldFormat.INTEGER);
          this._submittedFilterManager.CreateColumnFilter(11, FieldFormat.INTEGER);
        }
        this._submittedFilterManager.FilterChanged += new EventHandler(this._submittedFilterManager_FilterChanged);
      }
      this.lblSubmittedFilter.Text = "None";
    }

    private void _submittedFilterManager_FilterChanged(object sender, EventArgs e)
    {
      this._filter = this._submittedFilterManager.ToFieldFilterList();
      this.SetFilterText(this.lblSubmittedFilter);
      List<SubmissionStatus> filteredSubmissions = this.GetFilteredSubmissions(0, DataDocsConstants.MAX_SUBMISSIONS_PER_PAGE, this._filter);
      this._totalItemsCount = this._submissionsCursor.GetItemCount();
      this.navSubmitted.PageChangedEvent -= new PageListNavigator.PageChangedEventHandler(this.navSubmitted_PageChangedEvent);
      this.HandleSubmittedTab();
      this.navSubmitted.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.navSubmitted_PageChangedEvent);
      this.BindSubmissions(filteredSubmissions);
      this.gvSubmitted.Focus();
    }

    private void SetFilterText(Label filterLabel)
    {
      List<string> values = new List<string>();
      foreach (FieldFilter fieldFilter in (List<FieldFilter>) this._filter)
        values.Add(fieldFilter.FieldDescription + " " + fieldFilter.OperatorTypeAsString + " " + fieldFilter.ValueDescription);
      string str = string.Join(" AND ", (IEnumerable<string>) values);
      filterLabel.Text = string.IsNullOrEmpty(str) ? "None" : str;
    }

    private void InitializeCursors()
    {
      this._submissionsCursor = (ICursor) new SubmissionsCursor(DataDocsConstants.GetSubmittedDeliveryStatusList());
    }

    private List<SubmissionStatus> GetSubmissions(int startIndex, int count)
    {
      return ((IEnumerable<SubmissionStatus>) this._submissionsCursor.GetItems(startIndex, count)).ToList<SubmissionStatus>();
    }

    private List<SubmissionStatus> GetFilteredSubmissions(
      int startIndex,
      int count,
      FieldFilterList filters)
    {
      return ((IEnumerable<SubmissionStatus>) ((SubmissionsCursor) this._submissionsCursor).GetFilteredSubmissions(startIndex, count, filters)).ToList<SubmissionStatus>();
    }

    private void SetupTabPages(TabPage tabPage)
    {
      if (tabPage == this.tpSubmitted)
      {
        this.navSubmitted.PageChangedEvent -= new PageListNavigator.PageChangedEventHandler(this.navSubmitted_PageChangedEvent);
        this.HandleSubmittedTab();
        this.navSubmitted.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.navSubmitted_PageChangedEvent);
        this.DisplayCurrentSubmittedPage();
      }
      this._filter = this._submittedFilterManager.ToFieldFilterList();
    }

    private void HandleSubmittedTab()
    {
      this.navSubmitted.NumberOfItems = this._totalItemsCount;
      this.navSubmitted.ItemsPerPage = DataDocsConstants.MAX_SUBMISSIONS_PER_PAGE;
    }

    private void ResizeWindow()
    {
      Size size = MainForm.Instance.Size;
      this.Size = new Size(Convert.ToInt32(0.8 * (double) size.Width), Convert.ToInt32(0.8 * (double) size.Height));
      this.Left = MainForm.Instance.Left + MainForm.Instance.Width / 2 - this.Width / 2;
      this.Top = MainForm.Instance.Top + MainForm.Instance.Height / 2 - this.Height / 2;
    }

    private void DeliveryStatus_Load(object sender, EventArgs e) => this.ResizeWindow();

    private void btnClose_Click(object sender, EventArgs e)
    {
      if (this.submissionsWorker != null)
      {
        if (this.submissionsWorker.IsBusy)
          this.submissionsWorker.CancelAsync();
        else
          this.submissionsWorker.Dispose();
      }
      this.Close();
    }

    private void navSubmitted_PageChangedEvent(object sender, PageChangedEventArgs e)
    {
      using (CursorActivator.Wait())
        this.DisplayCurrentSubmittedPage();
    }

    private void BindSubmissions(List<SubmissionStatus> submissions)
    {
      this.gvSubmitted.Items.Clear();
      Hashtable users = this._session.OrganizationManager.GetUsers(submissions.Select<SubmissionStatus, string>((Func<SubmissionStatus, string>) (sub => sub.CreatedBy)).ToArray<string>(), true);
      foreach (SubmissionStatus submission in submissions)
      {
        GVItem gvItem = new GVItem();
        this.gvSubmitted.Items.Add(gvItem);
        gvItem.Tag = (object) submission;
        gvItem.SubItems.Add((object) submission.ReferenceID);
        gvItem.SubItems.Add((object) submission.RecipientTransactionID);
        gvItem.SubItems.Add((object) submission.SubmittedTo);
        try
        {
          string createdBy = submission.CreatedBy;
          UserInfoSummary userInfoSummary = (UserInfoSummary) users[(object) createdBy];
          gvItem.SubItems.Add((object) (userInfoSummary.LastName + ", " + userInfoSummary.FirstName));
        }
        catch (Exception ex)
        {
          Tracing.Log(DataDocsSubmissionStatus.sw, nameof (DataDocsSubmissionStatus), TraceLevel.Error, ex.ToString());
          gvItem.SubItems.Add((object) submission.CreatedBy);
        }
        gvItem.SubItems.Add((object) submission.LoanNumber);
        gvItem.SubItems.Add((object) submission.SubmissionType);
        gvItem.SubItems.Add((object) submission.SubmissionDate);
        gvItem.SubItems.Add((object) DataDocsConstants.DeliveryStatusToString(submission.Status));
        gvItem.SubItems.Add((object) submission.StatusDate);
        if (this._session.StartupInfo.AllowDataAndDocsReview)
        {
          GVSubItemCollection subItems1 = gvItem.SubItems;
          int num = submission.AuditCountRed;
          AuditReportLabel auditReportLabel1 = new AuditReportLabel(AuditMessageStyle.Red, num.ToString());
          subItems1.Add((object) auditReportLabel1);
          GVSubItemCollection subItems2 = gvItem.SubItems;
          num = submission.AuditCountYellow;
          AuditReportLabel auditReportLabel2 = new AuditReportLabel(AuditMessageStyle.Yellow, num.ToString());
          subItems2.Add((object) auditReportLabel2);
          gvItem.SubItems.Add((object) new AuditReportLabel(AuditMessageStyle.Green, submission.AuditCountGreen.ToString()));
        }
        GVSubItem actions = this.CreateActions(submission);
        gvItem.SubItems.Add(actions);
      }
    }

    private GVSubItem CreateActions(SubmissionStatus submission)
    {
      GVSubItem actions1 = new GVSubItem();
      DeliveryAction[] actions2 = submission.Actions;
      FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
      for (int index = 0; index < actions2.Length; ++index)
      {
        DeliveryAction action = actions2[index];
        if (action != DeliveryAction.None)
        {
          EllieMae.EMLite.UI.LinkLabel linkLabel = new EllieMae.EMLite.UI.LinkLabel();
          linkLabel.Text = DataDocsConstants.DeliveryActionsToString(action);
          linkLabel.AutoSize = true;
          linkLabel.Click += new EventHandler(this.linkLabel_Click);
          linkLabel.Tag = (object) actions1;
          flowLayoutPanel.Controls.Add((Control) linkLabel);
          if (index < actions2.Length - 1)
            flowLayoutPanel.Controls.Add((Control) new VerticalSeparator());
        }
      }
      actions1.Value = (object) flowLayoutPanel;
      return actions1;
    }

    private void linkLabel_Click(object sender, EventArgs e)
    {
      EllieMae.EMLite.UI.LinkLabel linkLabel = (EllieMae.EMLite.UI.LinkLabel) sender;
      DeliveryAction deliveryActionsEnum = DataDocsConstants.StringToDeliveryActionsEnum(linkLabel.Text);
      SubmissionStatus tag = (SubmissionStatus) ((GVSubItem) linkLabel.Tag).Item.Tag;
      switch (deliveryActionsEnum)
      {
        case DeliveryAction.ViewAuditReport:
          string auditReport = ((SubmissionsCursor) this._submissionsCursor).GetAuditReport(tag);
          string nameWithExtension = SystemSettings.GetTempFileNameWithExtension("pdf");
          new BinaryObject(Convert.FromBase64String(auditReport)).Write(nameWithExtension);
          using (PdfPreviewDialog pdfPreviewDialog = new PdfPreviewDialog(nameWithExtension, true, true, false))
          {
            int num = (int) pdfPreviewDialog.ShowDialog((IWin32Window) Form.ActiveForm);
            break;
          }
        case DeliveryAction.ViewLog:
          if (string.IsNullOrEmpty(tag.LogDetails))
            break;
          string empty = string.Empty;
          string caption;
          MessageBoxIcon icon;
          if (tag.Status == DeliveryStatus.Error)
          {
            caption = "Error";
            icon = MessageBoxIcon.Hand;
          }
          else
          {
            caption = "Log Details";
            icon = MessageBoxIcon.Asterisk;
          }
          if (tag.HasValidationError)
          {
            int num = (int) MessageDialog.Show((IWin32Window) this, tag.LogDetails, caption, "Encompass ID - Field Description", string.Join(Environment.NewLine, tag.ValidationErrors), MessageDialogButtons.OKMoreInfo, icon, MessageBoxDefaultButton.Button1);
            break;
          }
          int num1 = (int) MessageBox.Show((IWin32Window) this, tag.LogDetails, caption, MessageBoxButtons.OK, icon, MessageBoxDefaultButton.Button1);
          break;
      }
    }

    private void DisplayCurrentSubmittedPage()
    {
      int startIndex = this.navSubmitted.CurrentPageIndex * this.navSubmitted.ItemsPerPage + 1;
      int itemsPerPage = this.navSubmitted.ItemsPerPage;
      List<SubmissionStatus> submissionStatusList = new List<SubmissionStatus>();
      this.BindSubmissions(this._filter == null || this._filter.Count<FieldFilter>() == 0 ? this.GetSubmissions(startIndex, itemsPerPage) : this.GetFilteredSubmissions(startIndex, itemsPerPage, this._filter));
    }

    private void btnSubClear_Click(object sender, EventArgs e) => this.RefreshSubmittedView();

    private void RefreshSubmittedView()
    {
      this._submittedFilterManager.ClearColumnFilters();
      this._filter = this._submittedFilterManager.ToFieldFilterList();
      ((SubmissionsCursor) this._submissionsCursor).ClearFilter();
      this.GetSubmissionsAsync();
      this.SetFilterText(this.lblSubmittedFilter);
    }

    private void btnSubmittedRefresh_Click(object sender, EventArgs e)
    {
      this.RefreshSubmittedView();
    }

    private void gvSubmitted_HeaderClick(object sender, MouseEventArgs e)
    {
      int columnAtClientOffset = this.gvSubmitted.GetColumnAtClientOffset(e.Location.X);
      IEnumerable<GVColumn> source = this.gvSubmitted.Columns.Where<GVColumn>((Func<GVColumn, bool>) (c => c.Name == "colStatusDate"));
      int index = source == null ? 0 : source.First<GVColumn>().Index;
      int startIndex = this.navSubmitted.CurrentPageIndex * this.navSubmitted.ItemsPerPage + 1;
      string sortOrder;
      switch (this.gvSubmitted.Columns[index].SortOrder)
      {
        case SortOrder.Ascending:
          sortOrder = "desc";
          break;
        case SortOrder.Descending:
          sortOrder = "asc";
          break;
        default:
          sortOrder = "asc";
          break;
      }
      if (columnAtClientOffset == index)
      {
        this.gvSubmitted.SortOption = GVSortOption.Auto;
        List<SubmissionStatus> submissionStatusList = new List<SubmissionStatus>();
        this.BindSubmissions(this._filter.Count<FieldFilter>() != 0 ? ((IEnumerable<SubmissionStatus>) ((SubmissionsCursor) this._submissionsCursor).GetFilteredSubmissions(startIndex, DataDocsConstants.MAX_SUBMISSIONS_PER_PAGE, this._filter, sortOrder)).ToList<SubmissionStatus>() : ((IEnumerable<SubmissionStatus>) ((SubmissionsCursor) this._submissionsCursor).GetItems(startIndex, DataDocsConstants.MAX_SUBMISSIONS_PER_PAGE, sortOrder)).ToList<SubmissionStatus>());
      }
      else
        this.gvSubmitted.SortOption = GVSortOption.None;
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
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DataDocsSubmissionStatus));
      this.gpTopPanel = new GradientPanel();
      this.label1 = new Label();
      this.tabControl = new TabControl();
      this.tpSubmitted = new TabPage();
      this.gpSubmittedFilter = new GradientPanel();
      this.btnSubClear = new Button();
      this.lblSubmittedFilter = new Label();
      this.lblSubmittedFilterLabel = new Label();
      this.groupContainer2 = new GroupContainer();
      this.gvSubmitted = new GridView();
      this.btnSubmittedRefresh = new StandardIconButton();
      this.navSubmitted = new PageListNavigator();
      this.btnClose = new Button();
      this.toolTip = new ToolTip(this.components);
      this.progressBar = new ProgressBar();
      this.submissionsWorker = new BackgroundWorker();
      this.lblMessage = new Label();
      this.gpTopPanel.SuspendLayout();
      this.tabControl.SuspendLayout();
      this.tpSubmitted.SuspendLayout();
      this.gpSubmittedFilter.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      ((ISupportInitialize) this.btnSubmittedRefresh).BeginInit();
      this.SuspendLayout();
      this.gpTopPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gpTopPanel.BackColorGlassyStyle = true;
      this.gpTopPanel.Controls.Add((Control) this.label1);
      this.gpTopPanel.GradientColor1 = Color.FromArgb(81, 123, 184);
      this.gpTopPanel.GradientColor2 = Color.FromArgb(167, 201, 239);
      this.gpTopPanel.Location = new Point(0, -1);
      this.gpTopPanel.Name = "gpTopPanel";
      this.gpTopPanel.Size = new Size(1434, 32);
      this.gpTopPanel.Style = GradientPanel.PanelStyle.PageHeader;
      this.gpTopPanel.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(12, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(139, 16);
      this.label1.TabIndex = 3;
      this.label1.Text = "Loan Delivery Status";
      this.tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tabControl.Controls.Add((Control) this.tpSubmitted);
      this.tabControl.Location = new Point(0, 33);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new Size(1434, 632);
      this.tabControl.TabIndex = 1;
      this.tpSubmitted.Controls.Add((Control) this.gpSubmittedFilter);
      this.tpSubmitted.Controls.Add((Control) this.groupContainer2);
      this.tpSubmitted.Location = new Point(4, 22);
      this.tpSubmitted.Name = "tpSubmitted";
      this.tpSubmitted.Padding = new Padding(3);
      this.tpSubmitted.Size = new Size(1426, 606);
      this.tpSubmitted.TabIndex = 1;
      this.tpSubmitted.Text = "Status";
      this.tpSubmitted.UseVisualStyleBackColor = true;
      this.gpSubmittedFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gpSubmittedFilter.Controls.Add((Control) this.btnSubClear);
      this.gpSubmittedFilter.Controls.Add((Control) this.lblSubmittedFilter);
      this.gpSubmittedFilter.Controls.Add((Control) this.lblSubmittedFilterLabel);
      this.gpSubmittedFilter.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gpSubmittedFilter.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gpSubmittedFilter.Location = new Point(0, 0);
      this.gpSubmittedFilter.Name = "gpSubmittedFilter";
      this.gpSubmittedFilter.Size = new Size(1426, 32);
      this.gpSubmittedFilter.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gpSubmittedFilter.TabIndex = 2;
      this.btnSubClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSubClear.Location = new Point(1345, 4);
      this.btnSubClear.Name = "btnSubClear";
      this.btnSubClear.Size = new Size(75, 23);
      this.btnSubClear.TabIndex = 12;
      this.btnSubClear.Text = "Clear";
      this.btnSubClear.UseVisualStyleBackColor = true;
      this.btnSubClear.Click += new EventHandler(this.btnSubClear_Click);
      this.lblSubmittedFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblSubmittedFilter.AutoEllipsis = true;
      this.lblSubmittedFilter.BackColor = Color.Transparent;
      this.lblSubmittedFilter.Location = new Point(39, 9);
      this.lblSubmittedFilter.Name = "lblSubmittedFilter";
      this.lblSubmittedFilter.Size = new Size(501, 14);
      this.lblSubmittedFilter.TabIndex = 11;
      this.lblSubmittedFilterLabel.AutoSize = true;
      this.lblSubmittedFilterLabel.BackColor = Color.Transparent;
      this.lblSubmittedFilterLabel.Location = new Point(8, 9);
      this.lblSubmittedFilterLabel.Name = "lblSubmittedFilterLabel";
      this.lblSubmittedFilterLabel.Size = new Size(32, 13);
      this.lblSubmittedFilterLabel.TabIndex = 10;
      this.lblSubmittedFilterLabel.Text = "Filter:";
      this.groupContainer2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer2.Controls.Add((Control) this.gvSubmitted);
      this.groupContainer2.Controls.Add((Control) this.btnSubmittedRefresh);
      this.groupContainer2.Controls.Add((Control) this.navSubmitted);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 32);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(1426, 575);
      this.groupContainer2.TabIndex = 1;
      this.gvSubmitted.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colReferenceNumber";
      gvColumn1.Text = "Transaction ID";
      gvColumn1.Width = 90;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colRecipientTransactionNumber";
      gvColumn2.Text = "Memo";
      gvColumn2.Width = 140;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colInvestor";
      gvColumn3.Text = "Submitted To";
      gvColumn3.Width = 150;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "colUser";
      gvColumn4.Text = "Created By";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "colLoanNumber";
      gvColumn5.Text = "Loan Number";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "colSubmissionType";
      gvColumn6.Text = "Submission Type";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "colSubmittedDate";
      gvColumn7.SortMethod = GVSortMethod.DateTime;
      gvColumn7.Text = "Submission Date";
      gvColumn7.Width = 140;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "colStatus";
      gvColumn8.Text = "Status";
      gvColumn8.Width = 100;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "colStatusDate";
      gvColumn9.SortMethod = GVSortMethod.DateTime;
      gvColumn9.Text = "Status Date";
      gvColumn9.Width = 140;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "colAuditRed";
      gvColumn10.SortMethod = GVSortMethod.Numeric;
      gvColumn10.Text = "Fail";
      gvColumn10.Width = 75;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "colAuditYellow";
      gvColumn11.SortMethod = GVSortMethod.Numeric;
      gvColumn11.Text = "At Risk";
      gvColumn11.Width = 75;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "colAuditGreen";
      gvColumn12.SortMethod = GVSortMethod.Numeric;
      gvColumn12.Text = "Success";
      gvColumn12.Width = 75;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "colActions";
      gvColumn13.SortMethod = GVSortMethod.None;
      gvColumn13.Text = "Actions";
      gvColumn13.Width = 240;
      this.gvSubmitted.Columns.AddRange(new GVColumn[13]
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
        gvColumn11,
        gvColumn12,
        gvColumn13
      });
      this.gvSubmitted.FilterVisible = true;
      this.gvSubmitted.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvSubmitted.Location = new Point(0, 26);
      this.gvSubmitted.Name = "gvSubmitted";
      this.gvSubmitted.Size = new Size(1426, 548);
      this.gvSubmitted.TabIndex = 5;
      this.gvSubmitted.HeaderClick += new MouseEventHandler(this.gvSubmitted_HeaderClick);
      this.btnSubmittedRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSubmittedRefresh.BackColor = Color.Transparent;
      this.btnSubmittedRefresh.Location = new Point(1401, 4);
      this.btnSubmittedRefresh.MouseDownImage = (Image) null;
      this.btnSubmittedRefresh.Name = "btnSubmittedRefresh";
      this.btnSubmittedRefresh.Size = new Size(16, 16);
      this.btnSubmittedRefresh.StandardButtonType = StandardIconButton.ButtonType.RefreshButton;
      this.btnSubmittedRefresh.TabIndex = 3;
      this.btnSubmittedRefresh.TabStop = false;
      this.btnSubmittedRefresh.Click += new EventHandler(this.btnSubmittedRefresh_Click);
      this.navSubmitted.BackColor = Color.Transparent;
      this.navSubmitted.Font = new Font("Arial", 8f);
      this.navSubmitted.Location = new Point(3, 2);
      this.navSubmitted.Name = "navSubmitted";
      this.navSubmitted.NumberOfItems = 0;
      this.navSubmitted.Size = new Size(254, 22);
      this.navSubmitted.TabIndex = 2;
      this.navSubmitted.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.navSubmitted_PageChangedEvent);
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.Location = new Point(1345, 670);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 2;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.progressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.progressBar.Location = new Point(15, 671);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new Size(210, 23);
      this.progressBar.TabIndex = 3;
      this.progressBar.Visible = false;
      this.submissionsWorker.WorkerReportsProgress = true;
      this.submissionsWorker.DoWork += new DoWorkEventHandler(this._submissionsWorker_DoWork);
      this.submissionsWorker.ProgressChanged += new ProgressChangedEventHandler(this._submissionsWorker_ProgressChanged);
      this.submissionsWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this._submissionsWorker_RunWorkerCompleted);
      this.lblMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblMessage.AutoSize = true;
      this.lblMessage.Location = new Point(232, 677);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(113, 13);
      this.lblMessage.TabIndex = 4;
      this.lblMessage.Text = "Loading submissions...";
      this.lblMessage.Visible = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1434, 702);
      this.Controls.Add((Control) this.lblMessage);
      this.Controls.Add((Control) this.progressBar);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.tabControl);
      this.Controls.Add((Control) this.gpTopPanel);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (DataDocsSubmissionStatus);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Loan Delivery";
      this.Activated += new EventHandler(this.DataDocsSubmissionStatus_Activated);
      this.Load += new EventHandler(this.DeliveryStatus_Load);
      this.gpTopPanel.ResumeLayout(false);
      this.gpTopPanel.PerformLayout();
      this.tabControl.ResumeLayout(false);
      this.tpSubmitted.ResumeLayout(false);
      this.gpSubmittedFilter.ResumeLayout(false);
      this.gpSubmittedFilter.PerformLayout();
      this.groupContainer2.ResumeLayout(false);
      ((ISupportInitialize) this.btnSubmittedRefresh).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
