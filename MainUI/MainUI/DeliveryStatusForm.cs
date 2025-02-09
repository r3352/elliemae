// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.DeliveryStatusForm
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.DataDocs;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
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
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class DeliveryStatusForm : Form
  {
    private const string className = "DeliveryStatusForm";
    protected static string sw = Tracing.SwOutsideLoan;
    private GridViewStatusFilterManager _activeFilterManager;
    private GridViewStatusFilterManager _submittedFilterManager;
    private FieldFilterList _activeFilter;
    private Random random = new Random();
    private ICursor _activeSubmissionsCursor;
    private ICursor _submissionsCursor;
    private List<SubmissionStatus> _selected = new List<SubmissionStatus>();
    private Sessions.Session _session;
    private IContainer components;
    private GradientPanel gpTopPanel;
    private Button btnRemove;
    private Button btnSubmit;
    private Label label1;
    private TabControl tabControl;
    private TabPage tpActive;
    private TabPage tpSubmitted;
    private GroupContainer groupContainer1;
    private PageListNavigator navActive;
    private StandardIconButton btnActiveRefresh;
    private GridView gvActive;
    private GroupContainer groupContainer2;
    private StandardIconButton btnSubmittedRefresh;
    private PageListNavigator navSubmitted;
    private GradientPanel gpActiveFilter;
    private GradientPanel gpSubmittedFilter;
    private Button btnClose;
    private Label lblActiveFilter;
    private Label lblActiveFilterLabel;
    private Label lblSubmittedFilter;
    private Label lblSubmittedFilterLabel;
    private GridView gvSubmitted;
    private Label lblSelectedLoanCount;
    private Button btnActiveClear;
    private Button btnSubClear;
    private ToolTip toolTip;

    public DeliveryStatusForm()
    {
      this.InitializeComponent();
      this.SetupUI();
    }

    public DeliveryStatusForm(Sessions.Session session)
    {
      this._session = session;
      this.InitializeComponent();
      this.SetupUI();
    }

    private void SetupUI()
    {
      this.HandleStandardButtons();
      this.InitializeCursors();
      this.SetupFilters();
      this.SetupTabPages(this.tpActive);
      this.SetupTabPages(this.tpSubmitted);
      this.SetupToolTips();
      this.UpdateSelectedCount();
    }

    private void SetupToolTips()
    {
      this.gvActive.Hover += new HoverEventDelegate(this.gvActive_Hover);
      this.gvSubmitted.Hover += new HoverEventDelegate(this.gvSubmitted_Hover);
    }

    private void gvSubmitted_Hover(object source, HoverEventArgs e)
    {
      if (e.ItemInfo.ColumnIndex == 8)
        this.toolTip.SetToolTip((Control) this.gvSubmitted, "Validation Failed");
      if (e.ItemInfo.ColumnIndex == 9)
        this.toolTip.SetToolTip((Control) this.gvSubmitted, "Validation Inconclusive");
      if (e.ItemInfo.ColumnIndex != 10)
        return;
      this.toolTip.SetToolTip((Control) this.gvSubmitted, "Validation Passed");
    }

    private void gvActive_Hover(object source, HoverEventArgs e)
    {
      if (e.ItemInfo.ColumnIndex == 9)
        this.toolTip.SetToolTip((Control) this.gvActive, "Validation Failed");
      if (e.ItemInfo.ColumnIndex == 10)
        this.toolTip.SetToolTip((Control) this.gvActive, "Validation Inconclusive");
      if (e.ItemInfo.ColumnIndex != 11)
        return;
      this.toolTip.SetToolTip((Control) this.gvActive, "Validation Passed");
    }

    private void SetupFilters()
    {
      this._activeFilterManager = new GridViewStatusFilterManager(this._session, this.gvActive);
      this._activeFilterManager.AllowTextMatchForListOptions = true;
      this._activeFilterManager.CreateColumnFilter(1, FieldFormat.STRING);
      this._activeFilterManager.CreateColumnFilter(2, FieldFormat.STRING);
      this._activeFilterManager.CreateColumnFilter(3, FieldFormat.STRING);
      this._activeFilterManager.CreateColumnFilter(4, FieldFormat.DATETIME);
      this._activeFilterManager.CreateColumnFilter(5, FieldFormat.STRING);
      this._activeFilterManager.CreateColumnFilter(6, FieldFormat.STRING);
      this._activeFilterManager.CreateColumnFilter(7, FieldFormat.STRING);
      this._activeFilterManager.CreateColumnFilter(8, FieldFormat.STRING, DataDocsConstants.GetActiveDeliveryStatusList(), true);
      this._activeFilterManager.CreateColumnFilter(9, FieldFormat.INTEGER);
      this._activeFilterManager.CreateColumnFilter(10, FieldFormat.INTEGER);
      this._activeFilterManager.CreateColumnFilter(11, FieldFormat.INTEGER);
      this._activeFilterManager.CreateColumnFilter(12, FieldFormat.DATETIME);
      this._activeFilterManager.FilterChanged += new EventHandler(this._activeFilterManager_FilterChanged);
      this.lblActiveFilter.Text = "None";
      this._submittedFilterManager = new GridViewStatusFilterManager(this._session, this.gvSubmitted);
      this._submittedFilterManager.AllowTextMatchForListOptions = true;
      this._submittedFilterManager.CreateColumnFilter(0, FieldFormat.STRING);
      this._submittedFilterManager.CreateColumnFilter(1, FieldFormat.STRING);
      this._submittedFilterManager.CreateColumnFilter(2, FieldFormat.STRING);
      this._submittedFilterManager.CreateColumnFilter(3, FieldFormat.DATETIME);
      this._submittedFilterManager.CreateColumnFilter(4, FieldFormat.STRING);
      this._submittedFilterManager.CreateColumnFilter(5, FieldFormat.STRING);
      this._submittedFilterManager.CreateColumnFilter(6, FieldFormat.STRING);
      this._submittedFilterManager.CreateColumnFilter(7, FieldFormat.STRING, DataDocsConstants.GetSubmittedDeliveryStatusList(), true);
      this._submittedFilterManager.CreateColumnFilter(8, FieldFormat.INTEGER);
      this._submittedFilterManager.CreateColumnFilter(9, FieldFormat.INTEGER);
      this._submittedFilterManager.CreateColumnFilter(10, FieldFormat.INTEGER);
      this._submittedFilterManager.CreateColumnFilter(11, FieldFormat.DATETIME);
      this._submittedFilterManager.FilterChanged += new EventHandler(this._submittedFilterManager_FilterChanged);
      this.lblSubmittedFilter.Text = "None";
    }

    private void _submittedFilterManager_FilterChanged(object sender, EventArgs e)
    {
      this._activeFilter = this._submittedFilterManager.ToFieldFilterList();
      this.SetFilterText(this.lblSubmittedFilter);
      this._submittedFilterManager.ApplyFilter();
      ((SubmissionsCursor) this._submissionsCursor).GetFilteredSubmissions(1, DataDocsConstants.MAX_SUBMISSIONS_PER_PAGE, this._activeFilter);
      this.HandleSubmittedTab();
    }

    private void _activeFilterManager_FilterChanged(object sender, EventArgs e)
    {
      this._activeFilter = this._activeFilterManager.ToFieldFilterList();
      this.SetFilterText(this.lblActiveFilter);
      this._activeFilterManager.ApplyFilter();
      ((SubmissionsCursor) this._activeSubmissionsCursor).GetFilteredSubmissions(1, DataDocsConstants.MAX_SUBMISSIONS_PER_PAGE, this._activeFilter);
      this.HandleActiveTab();
    }

    private void SetFilterText(Label filterLabel)
    {
      List<string> values = new List<string>();
      foreach (FieldFilter fieldFilter in (List<FieldFilter>) this._activeFilter)
        values.Add(fieldFilter.FieldDescription + " " + fieldFilter.OperatorTypeAsString + " " + fieldFilter.ValueDescription);
      string str = string.Join(" AND ", (IEnumerable<string>) values);
      filterLabel.Text = string.IsNullOrEmpty(str) ? "None" : str;
    }

    private void InitializeCursors()
    {
      this._activeSubmissionsCursor = (ICursor) new SubmissionsCursor(DataDocsConstants.GetActiveDeliveryStatusList());
      this._submissionsCursor = (ICursor) new SubmissionsCursor(DataDocsConstants.GetSubmittedDeliveryStatusList());
    }

    private List<SubmissionStatus> GetSubmissions(int startIndex, int count, bool isActive)
    {
      return isActive ? ((IEnumerable<SubmissionStatus>) this._activeSubmissionsCursor.GetItems(startIndex, count)).ToList<SubmissionStatus>() : ((IEnumerable<SubmissionStatus>) this._submissionsCursor.GetItems(startIndex, count)).ToList<SubmissionStatus>();
    }

    private void SetupTabPages(TabPage tabPage)
    {
      if (tabPage == this.tpActive)
        this.HandleActiveTab();
      if (tabPage == this.tpSubmitted)
        this.HandleSubmittedTab();
      this._activeFilter = this._activeFilterManager.ToFieldFilterList();
    }

    private void HandleSubmittedTab()
    {
      this.navSubmitted.NumberOfItems = this._submissionsCursor.GetItemCount();
      this.navSubmitted.ItemsPerPage = DataDocsConstants.MAX_SUBMISSIONS_PER_PAGE;
    }

    private void HandleActiveTab()
    {
      this.btnSubmit.Enabled = this.btnRemove.Enabled = false;
      this.navActive.NumberOfItems = this._activeSubmissionsCursor.GetItemCount();
      this.navActive.ItemsPerPage = DataDocsConstants.MAX_SUBMISSIONS_PER_PAGE;
    }

    private void HandleStandardButtons() => this.btnSubmit.Enabled = this.btnRemove.Enabled = false;

    private void ResizeWindow()
    {
      Size size = MainForm.Instance.Size;
      this.Size = new Size(Convert.ToInt32(0.8 * (double) size.Width), Convert.ToInt32(0.8 * (double) size.Height));
      this.Left = MainForm.Instance.Left + MainForm.Instance.Width / 2 - this.Width / 2;
      this.Top = MainForm.Instance.Top + MainForm.Instance.Height / 2 - this.Height / 2;
    }

    private void DeliveryStatus_Load(object sender, EventArgs e) => this.ResizeWindow();

    private void btnClose_Click(object sender, EventArgs e) => this.Close();

    private void navActive_PageChangedEvent(object sender, PageChangedEventArgs e)
    {
      using (CursorActivator.Wait())
        this.DisplayCurrentActivePage();
    }

    private void navSubmitted_PageChangedEvent(object sender, PageChangedEventArgs e)
    {
      using (CursorActivator.Wait())
        this.DisplayCurrentSubmittedPage();
    }

    private void DisplayCurrentActivePage()
    {
      this.BindActiveSubmissions(this.GetSubmissions(this.navActive.CurrentPageIndex * this.navActive.CurrentPageItemCount, this.navActive.CurrentPageItemCount, true));
    }

    private void BindActiveSubmissions(List<SubmissionStatus> activeSubmissions)
    {
      this.gvActive.Items.Clear();
      Hashtable users = this._session.OrganizationManager.GetUsers(activeSubmissions.Select<SubmissionStatus, string>((Func<SubmissionStatus, string>) (sub => sub.CreatedBy)).ToArray<string>(), true);
      PipelineInfo[] pipeline = this._session.LoanManager.GetPipeline(activeSubmissions.Select<SubmissionStatus, string>((Func<SubmissionStatus, string>) (sub => "{" + sub.LoanGuid + "}")).ToArray<string>(), (string[]) null, PipelineData.Borrowers, false);
      foreach (SubmissionStatus activeSubmission1 in activeSubmissions)
      {
        SubmissionStatus activeSubmission = activeSubmission1;
        ((IEnumerable<PipelineInfo>) pipeline).Where<PipelineInfo>((Func<PipelineInfo, bool>) (info => info.LoanNumber == activeSubmission.LoanNumber)).FirstOrDefault<PipelineInfo>();
        GVItem gvItem = new GVItem();
        this.gvActive.Items.Add(gvItem);
        gvItem.Tag = (object) activeSubmission;
        GVSubItem subItem = gvItem.SubItems[0];
        subItem.CheckBoxVisible = true;
        subItem.Tag = (object) activeSubmission;
        if (activeSubmission.CanSubmit || activeSubmission.CanRemove)
        {
          subItem.CheckBoxEnabled = true;
          if (this.IsSelectedByUser(activeSubmission))
            subItem.Checked = true;
        }
        else
          subItem.CheckBoxEnabled = false;
        gvItem.SubItems.Add((object) activeSubmission.ReferenceID);
        gvItem.SubItems.Add((object) activeSubmission.RecipientTransactionID);
        gvItem.SubItems.Add((object) activeSubmission.SubmittedTo);
        gvItem.SubItems.Add((object) activeSubmission.CreateDate);
        try
        {
          UserInfoSummary userInfoSummary = (UserInfoSummary) users[(object) activeSubmission.CreatedBy];
          gvItem.SubItems.Add((object) (userInfoSummary.LastName + ", " + userInfoSummary.FirstName));
        }
        catch (Exception ex)
        {
          Tracing.Log(DeliveryStatusForm.sw, nameof (DeliveryStatusForm), TraceLevel.Error, ex.ToString());
          gvItem.SubItems.Add((object) activeSubmission.CreatedBy);
        }
        gvItem.SubItems.Add((object) activeSubmission.LoanNumber);
        gvItem.SubItems.Add((object) activeSubmission.StatusDate);
        gvItem.SubItems.Add((object) DataDocsConstants.DeliveryStatusToString(activeSubmission.Status));
        gvItem.SubItems.Add((object) new AuditReportLabel(AuditMessageStyle.Red, activeSubmission.AuditCountRed.ToString()));
        GVSubItemCollection subItems1 = gvItem.SubItems;
        int num = activeSubmission.AuditCountYellow;
        AuditReportLabel auditReportLabel1 = new AuditReportLabel(AuditMessageStyle.Yellow, num.ToString());
        subItems1.Add((object) auditReportLabel1);
        GVSubItemCollection subItems2 = gvItem.SubItems;
        num = activeSubmission.AuditCountGreen;
        AuditReportLabel auditReportLabel2 = new AuditReportLabel(AuditMessageStyle.Green, num.ToString());
        subItems2.Add((object) auditReportLabel2);
        GVSubItem actions = this.CreateActions(activeSubmission);
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
          string auditReport = ((SubmissionsCursor) this._activeSubmissionsCursor).GetAuditReport(tag);
          string nameWithExtension = SystemSettings.GetTempFileNameWithExtension("pdf");
          new BinaryObject(Convert.FromBase64String(auditReport)).Write(nameWithExtension);
          using (PdfPreviewDialog pdfPreviewDialog = new PdfPreviewDialog(nameWithExtension, true, true, false))
          {
            int num = (int) pdfPreviewDialog.ShowDialog((IWin32Window) Form.ActiveForm);
            break;
          }
      }
    }

    private void DisplayCurrentSubmittedPage()
    {
      this.BindSubmissions(this.GetSubmissions(this.navSubmitted.CurrentPageIndex * this.navSubmitted.CurrentPageItemCount, this.navSubmitted.CurrentPageItemCount, false));
    }

    private void BindSubmissions(List<SubmissionStatus> submissions)
    {
      this.gvSubmitted.Items.Clear();
      foreach (SubmissionStatus submission in submissions)
      {
        GVItem gvItem = new GVItem();
        gvItem.SubItems.Add((object) submission.ReferenceID);
        gvItem.SubItems.Add((object) submission.RecipientTransactionID);
        gvItem.SubItems.Add((object) submission.SubmittedTo);
        gvItem.SubItems.Add((object) submission.CreateDate);
        gvItem.SubItems.Add((object) submission.CreatedBy);
        gvItem.SubItems.Add((object) submission.LoanNumber);
        gvItem.SubItems.Add((object) submission.StatusDate);
        gvItem.SubItems.Add((object) DataDocsConstants.DeliveryStatusToString(submission.Status));
        gvItem.SubItems.Add((object) new AuditReportLabel(AuditMessageStyle.Red, submission.AuditCountRed.ToString()));
        gvItem.SubItems.Add((object) new AuditReportLabel(AuditMessageStyle.Yellow, submission.AuditCountYellow.ToString()));
        gvItem.SubItems.Add((object) new AuditReportLabel(AuditMessageStyle.Green, submission.AuditCountGreen.ToString()));
        GVSubItem actions = this.CreateActions(submission);
        gvItem.SubItems.Add(actions);
        gvItem.Tag = (object) submission;
        this.gvSubmitted.Items.Add(gvItem);
      }
    }

    private void gvActive_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      GVSubItem subItem = e.SubItem;
      if (subItem.CheckedState == ButtonState.Checked && !this.IsSelectedByUser((SubmissionStatus) subItem.Tag))
        this._selected.Add((SubmissionStatus) subItem.Tag);
      else if (subItem.CheckedState == ButtonState.Normal)
        this._selected.Remove((SubmissionStatus) subItem.Tag);
      this.btnRemove.Enabled = this.btnSubmit.Enabled = this._selected.Count > 0;
      this.UpdateSelectedCount();
    }

    private void UpdateSelectedCount()
    {
      this.lblSelectedLoanCount.Text = "Number of loans selected: " + (object) this._selected.Count;
    }

    private bool IsSelectedByUser(SubmissionStatus activeSubmission)
    {
      return this._selected.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.LoanGuid == activeSubmission.LoanGuid)).Count<SubmissionStatus>() > 0;
    }

    private void btnSubmit_Click(object sender, EventArgs e)
    {
      int num1 = this._selected.Count<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.Status == DeliveryStatus.Error));
      if (num1 > 0)
      {
        List<string> values = new List<string>();
        foreach (SubmissionStatus submissionStatus in this._selected.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.Status == DeliveryStatus.Error)))
          values.Add(submissionStatus.ReferenceID + " | " + submissionStatus.SubmittedTo + " | " + submissionStatus.LoanNumber);
        string additionalInfo = string.Join("\r\n", (IEnumerable<string>) values);
        if (MessageDialog.Show((IWin32Window) this, string.Format("The following {0} loan(s) will not be submitted as they are in \"Error\" status. Are you sure you want to proceed?", (object) num1), "Encompass", "Loans in error status", additionalInfo, MessageDialogButtons.YesNoMoreInfo, MessageBoxIcon.Question) != DialogResult.Yes)
        {
          this._selected.RemoveAll((Predicate<SubmissionStatus>) (sub => sub.Status == DeliveryStatus.Error));
          this.HandleActiveTab();
          return;
        }
      }
      using (ProgressDialog progressDialog = new ProgressDialog("Submitting Loans", new AsynchronousProcess(this.SubmitToDelivery), (object) this._selected.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.Status != DeliveryStatus.Error)).ToList<SubmissionStatus>(), true))
      {
        int num2 = (int) progressDialog.ShowDialog((IWin32Window) this);
      }
      int num3 = (int) Utils.Dialog((IWin32Window) this, "The selected loans have been submitted. You may check the status of these submissions from the Submitted tab", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      this._selected.RemoveAll((Predicate<SubmissionStatus>) (sub => sub.Status != DeliveryStatus.Error));
      this.HandleActiveTab();
    }

    private DialogResult SubmitToDelivery(object state, IProgressFeedback feedback)
    {
      List<SubmissionStatus> source1 = (List<SubmissionStatus>) state;
      IEnumerable<\u003C\u003Ef__AnonymousType0<string, List<string>>> source2 = source1.GroupBy((Func<SubmissionStatus, string>) (sub => sub.SubmittedTo), (Func<SubmissionStatus, string>) (sub => sub.LoanNumber), (investor, loans) => new
      {
        Investor = investor,
        Loans = loans.ToList<string>()
      });
      int num = 0;
      feedback.ResetCounter(source2.Count());
      foreach (var data in source2)
      {
        var submissionGroup = data;
        feedback.SetFeedback(string.Format("Submitting {0} for {1}...", (object) submissionGroup.Loans.Count, (object) submissionGroup.Investor), string.Format("Loans: {0}", (object) string.Join(", ", (IEnumerable<string>) submissionGroup.Loans)), num);
        ((SubmissionsCursor) this._activeSubmissionsCursor).SubmitForDelivery(source1.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.SubmittedTo == submissionGroup.Investor)).Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => submissionGroup.Loans.Contains(sub.LoanNumber))).ToList<SubmissionStatus>());
        Thread.Sleep(1000);
        ++num;
      }
      return DialogResult.OK;
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      List<string> values = new List<string>();
      foreach (SubmissionStatus submissionStatus in this._selected)
        values.Add(submissionStatus.ReferenceID + " | " + submissionStatus.SubmittedTo + " | " + submissionStatus.LoanNumber + " | " + DataDocsConstants.DeliveryStatusToString(submissionStatus.Status));
      string additionalInfo = string.Join("\r\n", (IEnumerable<string>) values);
      if (MessageDialog.Show((IWin32Window) this, string.Format("The following {0} loan(s) will be removed from delivery. Are you sure you want to proceed?", (object) values.Count), "Encompass", "Loans to be removed", additionalInfo, MessageDialogButtons.YesNoMoreInfo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      using (ProgressDialog progressDialog = new ProgressDialog("Removing Loans From Delivery", new AsynchronousProcess(this.RemoveFromDelivery), (object) this._selected, true))
      {
        int num = (int) progressDialog.ShowDialog((IWin32Window) this);
      }
      int num1 = (int) Utils.Dialog((IWin32Window) this, "The selected loans have been removed from delivery", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      this._selected.Clear();
      this.HandleActiveTab();
    }

    private DialogResult RemoveFromDelivery(object state, IProgressFeedback feedback)
    {
      List<SubmissionStatus> source1 = (List<SubmissionStatus>) state;
      IEnumerable<\u003C\u003Ef__AnonymousType0<string, List<string>>> source2 = source1.GroupBy((Func<SubmissionStatus, string>) (sub => sub.SubmittedTo), (Func<SubmissionStatus, string>) (sub => sub.LoanNumber), (investor, loans) => new
      {
        Investor = investor,
        Loans = loans.ToList<string>()
      });
      int num = 0;
      feedback.ResetCounter(source2.Count());
      foreach (var data in source2)
      {
        var submissionGroup = data;
        feedback.SetFeedback(string.Format("Removing {0} loans from {1}...", (object) submissionGroup.Loans.Count, (object) submissionGroup.Investor), string.Format("Loans: {0}", (object) string.Join(", ", (IEnumerable<string>) submissionGroup.Loans)), num);
        ((SubmissionsCursor) this._activeSubmissionsCursor).RemoveFromDelivery(source1.Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => sub.SubmittedTo == submissionGroup.Investor)).Where<SubmissionStatus>((Func<SubmissionStatus, bool>) (sub => submissionGroup.Loans.Contains(sub.LoanNumber))).ToList<SubmissionStatus>());
        Thread.Sleep(1000);
        ++num;
      }
      return DialogResult.OK;
    }

    private void btnActiveClear_Click(object sender, EventArgs e) => this.RefreshActiveView();

    private void RefreshActiveView()
    {
      this._activeFilterManager.ClearColumnFilters();
      ((SubmissionsCursor) this._activeSubmissionsCursor).ClearFilter();
      this.HandleActiveTab();
      this.SetFilterText(this.lblActiveFilter);
    }

    private void btnActiveRefresh_Click(object sender, EventArgs e) => this.RefreshActiveView();

    private void btnSubClear_Click(object sender, EventArgs e) => this.RefreshSubmittedView();

    private void RefreshSubmittedView()
    {
      this._submittedFilterManager.ClearColumnFilters();
      ((SubmissionsCursor) this._submissionsCursor).ClearFilter();
      this.HandleSubmittedTab();
      this.SetFilterText(this.lblSubmittedFilter);
    }

    private void btnSubmittedRefresh_Click(object sender, EventArgs e)
    {
      this.RefreshSubmittedView();
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
      GVColumn gvColumn14 = new GVColumn();
      GVColumn gvColumn15 = new GVColumn();
      GVColumn gvColumn16 = new GVColumn();
      GVColumn gvColumn17 = new GVColumn();
      GVColumn gvColumn18 = new GVColumn();
      GVColumn gvColumn19 = new GVColumn();
      GVColumn gvColumn20 = new GVColumn();
      GVColumn gvColumn21 = new GVColumn();
      GVColumn gvColumn22 = new GVColumn();
      GVColumn gvColumn23 = new GVColumn();
      GVColumn gvColumn24 = new GVColumn();
      GVColumn gvColumn25 = new GVColumn();
      GVColumn gvColumn26 = new GVColumn();
      GVColumn gvColumn27 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DeliveryStatusForm));
      this.gpTopPanel = new GradientPanel();
      this.label1 = new Label();
      this.btnSubmit = new Button();
      this.btnRemove = new Button();
      this.tabControl = new TabControl();
      this.tpActive = new TabPage();
      this.gpActiveFilter = new GradientPanel();
      this.btnActiveClear = new Button();
      this.lblActiveFilter = new Label();
      this.lblActiveFilterLabel = new Label();
      this.groupContainer1 = new GroupContainer();
      this.gvActive = new GridView();
      this.btnActiveRefresh = new StandardIconButton();
      this.navActive = new PageListNavigator();
      this.tpSubmitted = new TabPage();
      this.gpSubmittedFilter = new GradientPanel();
      this.btnSubClear = new Button();
      this.lblSubmittedFilter = new Label();
      this.lblSubmittedFilterLabel = new Label();
      this.groupContainer2 = new GroupContainer();
      this.gvSubmitted = new GridView();
      this.btnSubmittedRefresh = new StandardIconButton();
      this.navSubmitted = new PageListNavigator();
      this.lblSelectedLoanCount = new Label();
      this.btnClose = new Button();
      this.toolTip = new ToolTip(this.components);
      this.gpTopPanel.SuspendLayout();
      this.tabControl.SuspendLayout();
      this.tpActive.SuspendLayout();
      this.gpActiveFilter.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.btnActiveRefresh).BeginInit();
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
      this.gpTopPanel.Size = new Size(1433, 32);
      this.gpTopPanel.Style = GradientPanel.PanelStyle.PageHeader;
      this.gpTopPanel.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(12, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(132, 16);
      this.label1.TabIndex = 3;
      this.label1.Text = "Review and Submit";
      this.btnSubmit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSubmit.Location = new Point(1151, 1);
      this.btnSubmit.Name = "btnSubmit";
      this.btnSubmit.Size = new Size(108, 23);
      this.btnSubmit.TabIndex = 1;
      this.btnSubmit.Text = "Submit for Delivery";
      this.btnSubmit.UseVisualStyleBackColor = true;
      this.btnSubmit.Click += new EventHandler(this.btnSubmit_Click);
      this.btnRemove.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnRemove.Location = new Point(1265, 1);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(129, 23);
      this.btnRemove.TabIndex = 2;
      this.btnRemove.Text = "Remove from Delivery";
      this.btnRemove.UseVisualStyleBackColor = true;
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tabControl.Controls.Add((Control) this.tpActive);
      this.tabControl.Controls.Add((Control) this.tpSubmitted);
      this.tabControl.Location = new Point(0, 33);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new Size(1433, 630);
      this.tabControl.TabIndex = 1;
      this.tpActive.Controls.Add((Control) this.gpActiveFilter);
      this.tpActive.Controls.Add((Control) this.groupContainer1);
      this.tpActive.Location = new Point(4, 22);
      this.tpActive.Name = "tpActive";
      this.tpActive.Padding = new Padding(3);
      this.tpActive.Size = new Size(1425, 604);
      this.tpActive.TabIndex = 0;
      this.tpActive.Text = "Active";
      this.tpActive.UseVisualStyleBackColor = true;
      this.gpActiveFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gpActiveFilter.Controls.Add((Control) this.btnActiveClear);
      this.gpActiveFilter.Controls.Add((Control) this.lblActiveFilter);
      this.gpActiveFilter.Controls.Add((Control) this.lblActiveFilterLabel);
      this.gpActiveFilter.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gpActiveFilter.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gpActiveFilter.Location = new Point(0, 0);
      this.gpActiveFilter.Name = "gpActiveFilter";
      this.gpActiveFilter.Size = new Size(1425, 32);
      this.gpActiveFilter.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gpActiveFilter.TabIndex = 1;
      this.btnActiveClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnActiveClear.Location = new Point(1344, 4);
      this.btnActiveClear.Name = "btnActiveClear";
      this.btnActiveClear.Size = new Size(75, 23);
      this.btnActiveClear.TabIndex = 10;
      this.btnActiveClear.Text = "Clear";
      this.btnActiveClear.UseVisualStyleBackColor = true;
      this.btnActiveClear.Click += new EventHandler(this.btnActiveClear_Click);
      this.lblActiveFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblActiveFilter.AutoEllipsis = true;
      this.lblActiveFilter.BackColor = Color.Transparent;
      this.lblActiveFilter.Location = new Point(39, 9);
      this.lblActiveFilter.Name = "lblActiveFilter";
      this.lblActiveFilter.Size = new Size(483, 14);
      this.lblActiveFilter.TabIndex = 9;
      this.lblActiveFilterLabel.AutoSize = true;
      this.lblActiveFilterLabel.BackColor = Color.Transparent;
      this.lblActiveFilterLabel.Location = new Point(8, 9);
      this.lblActiveFilterLabel.Name = "lblActiveFilterLabel";
      this.lblActiveFilterLabel.Size = new Size(32, 13);
      this.lblActiveFilterLabel.TabIndex = 8;
      this.lblActiveFilterLabel.Text = "Filter:";
      this.groupContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.gvActive);
      this.groupContainer1.Controls.Add((Control) this.btnActiveRefresh);
      this.groupContainer1.Controls.Add((Control) this.navActive);
      this.groupContainer1.Controls.Add((Control) this.btnRemove);
      this.groupContainer1.Controls.Add((Control) this.btnSubmit);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 32);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(1425, 572);
      this.groupContainer1.TabIndex = 0;
      this.gvActive.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ColumnHeaderCheckbox = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colCheckBox";
      gvColumn1.SortMethod = GVSortMethod.None;
      gvColumn1.Text = "";
      gvColumn1.Width = 25;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colReferenceNumber";
      gvColumn2.Text = "Reference ID";
      gvColumn2.Width = 80;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colRecipientTransactionNumber";
      gvColumn3.Text = "Recipient Transaction ID";
      gvColumn3.Width = 140;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "colInvestor";
      gvColumn4.Text = "Submitted To";
      gvColumn4.Width = 150;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "colSubmittedDate";
      gvColumn5.SortMethod = GVSortMethod.DateTime;
      gvColumn5.Text = "Created Date";
      gvColumn5.Width = 120;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "colUser";
      gvColumn6.Text = "Created By";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "colBorrower";
      gvColumn7.Text = "Borrower Name";
      gvColumn7.Width = 100;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "colLoanNumber";
      gvColumn8.Text = "Loan Number";
      gvColumn8.Width = 100;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "colStatusDate";
      gvColumn9.SortMethod = GVSortMethod.DateTime;
      gvColumn9.Text = "Status Date";
      gvColumn9.Width = 120;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "colStatus";
      gvColumn10.Text = "Status";
      gvColumn10.Width = 100;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "colAuditRed";
      gvColumn11.SortMethod = GVSortMethod.Numeric;
      gvColumn11.Text = "Failed";
      gvColumn11.Width = 75;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "colAuditYellow";
      gvColumn12.Text = "Inconclusive";
      gvColumn12.Width = 75;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "colAuditGreen";
      gvColumn13.Text = "Passed";
      gvColumn13.Width = 75;
      gvColumn14.ImageIndex = -1;
      gvColumn14.Name = "colActions";
      gvColumn14.SortMethod = GVSortMethod.None;
      gvColumn14.Text = "Actions";
      gvColumn14.Width = 240;
      this.gvActive.Columns.AddRange(new GVColumn[14]
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
        gvColumn13,
        gvColumn14
      });
      this.gvActive.FilterVisible = true;
      this.gvActive.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvActive.Location = new Point(0, 26);
      this.gvActive.Name = "gvActive";
      this.gvActive.Size = new Size(1425, 546);
      this.gvActive.TabIndex = 4;
      this.gvActive.SubItemCheck += new GVSubItemEventHandler(this.gvActive_SubItemCheck);
      this.btnActiveRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnActiveRefresh.BackColor = Color.Transparent;
      this.btnActiveRefresh.Location = new Point(1400, 4);
      this.btnActiveRefresh.MouseDownImage = (Image) null;
      this.btnActiveRefresh.Name = "btnActiveRefresh";
      this.btnActiveRefresh.Size = new Size(16, 16);
      this.btnActiveRefresh.StandardButtonType = StandardIconButton.ButtonType.RefreshButton;
      this.btnActiveRefresh.TabIndex = 3;
      this.btnActiveRefresh.TabStop = false;
      this.btnActiveRefresh.Click += new EventHandler(this.btnActiveRefresh_Click);
      this.navActive.BackColor = Color.Transparent;
      this.navActive.Font = new Font("Arial", 8f);
      this.navActive.ItemsPerPage = 30;
      this.navActive.Location = new Point(3, 2);
      this.navActive.Name = "navActive";
      this.navActive.NumberOfItems = 0;
      this.navActive.Size = new Size(254, 22);
      this.navActive.TabIndex = 2;
      this.navActive.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.navActive_PageChangedEvent);
      this.tpSubmitted.Controls.Add((Control) this.gpSubmittedFilter);
      this.tpSubmitted.Controls.Add((Control) this.groupContainer2);
      this.tpSubmitted.Location = new Point(4, 22);
      this.tpSubmitted.Name = "tpSubmitted";
      this.tpSubmitted.Padding = new Padding(3);
      this.tpSubmitted.Size = new Size(1425, 604);
      this.tpSubmitted.TabIndex = 1;
      this.tpSubmitted.Text = "Submitted";
      this.tpSubmitted.UseVisualStyleBackColor = true;
      this.gpSubmittedFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gpSubmittedFilter.Controls.Add((Control) this.btnSubClear);
      this.gpSubmittedFilter.Controls.Add((Control) this.lblSubmittedFilter);
      this.gpSubmittedFilter.Controls.Add((Control) this.lblSubmittedFilterLabel);
      this.gpSubmittedFilter.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gpSubmittedFilter.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gpSubmittedFilter.Location = new Point(0, 0);
      this.gpSubmittedFilter.Name = "gpSubmittedFilter";
      this.gpSubmittedFilter.Size = new Size(1425, 32);
      this.gpSubmittedFilter.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gpSubmittedFilter.TabIndex = 2;
      this.btnSubClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSubClear.Location = new Point(1344, 4);
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
      this.lblSubmittedFilter.Size = new Size(500, 14);
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
      this.groupContainer2.Size = new Size(1425, 573);
      this.groupContainer2.TabIndex = 1;
      this.gvSubmitted.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn15.ImageIndex = -1;
      gvColumn15.Name = "colReferenceNumber";
      gvColumn15.Text = "Reference ID";
      gvColumn15.Width = 80;
      gvColumn16.ImageIndex = -1;
      gvColumn16.Name = "colRecipientTransactionNumber";
      gvColumn16.Text = "Recipient Transaction ID";
      gvColumn16.Width = 140;
      gvColumn17.ImageIndex = -1;
      gvColumn17.Name = "colInvestor";
      gvColumn17.Text = "Submitted To";
      gvColumn17.Width = 150;
      gvColumn18.ImageIndex = -1;
      gvColumn18.Name = "colSubmittedDate";
      gvColumn18.SortMethod = GVSortMethod.DateTime;
      gvColumn18.Text = "Created Date";
      gvColumn18.Width = 120;
      gvColumn19.ImageIndex = -1;
      gvColumn19.Name = "colUser";
      gvColumn19.Text = "Submitted By";
      gvColumn19.Width = 100;
      gvColumn20.ImageIndex = -1;
      gvColumn20.Name = "colBorrower";
      gvColumn20.Text = "Borrower Name";
      gvColumn20.Width = 100;
      gvColumn21.ImageIndex = -1;
      gvColumn21.Name = "colLoanNumber";
      gvColumn21.Text = "Loan Number";
      gvColumn21.Width = 100;
      gvColumn22.ImageIndex = -1;
      gvColumn22.Name = "colStatusDate";
      gvColumn22.SortMethod = GVSortMethod.DateTime;
      gvColumn22.Text = "Status Date";
      gvColumn22.Width = 120;
      gvColumn23.ImageIndex = -1;
      gvColumn23.Name = "colStatus";
      gvColumn23.Text = "Status";
      gvColumn23.Width = 100;
      gvColumn24.ImageIndex = -1;
      gvColumn24.Name = "colAuditRed";
      gvColumn24.SortMethod = GVSortMethod.Numeric;
      gvColumn24.Text = "Audit Red";
      gvColumn24.Width = 75;
      gvColumn25.ImageIndex = -1;
      gvColumn25.Name = "colAuditYellow";
      gvColumn25.SortMethod = GVSortMethod.Numeric;
      gvColumn25.Text = "Audit Yellow";
      gvColumn25.Width = 75;
      gvColumn26.ImageIndex = -1;
      gvColumn26.Name = "colAuditGreen";
      gvColumn26.SortMethod = GVSortMethod.Numeric;
      gvColumn26.Text = "Audit Green";
      gvColumn26.Width = 75;
      gvColumn27.ImageIndex = -1;
      gvColumn27.Name = "colActions";
      gvColumn27.SortMethod = GVSortMethod.None;
      gvColumn27.Text = "Actions";
      gvColumn27.Width = 240;
      this.gvSubmitted.Columns.AddRange(new GVColumn[13]
      {
        gvColumn15,
        gvColumn16,
        gvColumn17,
        gvColumn18,
        gvColumn19,
        gvColumn20,
        gvColumn21,
        gvColumn22,
        gvColumn23,
        gvColumn24,
        gvColumn25,
        gvColumn26,
        gvColumn27
      });
      this.gvSubmitted.FilterVisible = true;
      this.gvSubmitted.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvSubmitted.Location = new Point(0, 26);
      this.gvSubmitted.Name = "gvSubmitted";
      this.gvSubmitted.Size = new Size(1425, 546);
      this.gvSubmitted.TabIndex = 5;
      this.btnSubmittedRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSubmittedRefresh.BackColor = Color.Transparent;
      this.btnSubmittedRefresh.Location = new Point(1400, 4);
      this.btnSubmittedRefresh.MouseDownImage = (Image) null;
      this.btnSubmittedRefresh.Name = "btnSubmittedRefresh";
      this.btnSubmittedRefresh.Size = new Size(16, 16);
      this.btnSubmittedRefresh.StandardButtonType = StandardIconButton.ButtonType.RefreshButton;
      this.btnSubmittedRefresh.TabIndex = 3;
      this.btnSubmittedRefresh.TabStop = false;
      this.btnSubmittedRefresh.Click += new EventHandler(this.btnSubmittedRefresh_Click);
      this.navSubmitted.BackColor = Color.Transparent;
      this.navSubmitted.Font = new Font("Arial", 8f);
      this.navSubmitted.ItemsPerPage = 30;
      this.navSubmitted.Location = new Point(3, 2);
      this.navSubmitted.Name = "navSubmitted";
      this.navSubmitted.NumberOfItems = 0;
      this.navSubmitted.Size = new Size(254, 22);
      this.navSubmitted.TabIndex = 2;
      this.navSubmitted.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.navSubmitted_PageChangedEvent);
      this.lblSelectedLoanCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblSelectedLoanCount.AutoSize = true;
      this.lblSelectedLoanCount.Location = new Point(12, 673);
      this.lblSelectedLoanCount.Name = "lblSelectedLoanCount";
      this.lblSelectedLoanCount.Size = new Size(133, 13);
      this.lblSelectedLoanCount.TabIndex = 5;
      this.lblSelectedLoanCount.Text = "Number of loans selected: ";
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.Location = new Point(1344, 668);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 2;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1433, 700);
      this.Controls.Add((Control) this.lblSelectedLoanCount);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.tabControl);
      this.Controls.Add((Control) this.gpTopPanel);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (DeliveryStatusForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Review and Submit";
      this.Load += new EventHandler(this.DeliveryStatus_Load);
      this.gpTopPanel.ResumeLayout(false);
      this.gpTopPanel.PerformLayout();
      this.tabControl.ResumeLayout(false);
      this.tpActive.ResumeLayout(false);
      this.gpActiveFilter.ResumeLayout(false);
      this.gpActiveFilter.PerformLayout();
      this.groupContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.btnActiveRefresh).EndInit();
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
