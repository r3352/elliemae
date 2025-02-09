// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VerificationDetailsControl
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class VerificationDetailsControl : UserControl
  {
    private VerificationTimelineType editMode = VerificationTimelineType.Employment;
    private Sessions.Session session;
    private LoanData loan;
    private VerificationTimelineList timelineList;
    private VerificationDocumentList documentList;
    private DocumentLog[] allDocsList;
    private bool canAddNew = true;
    private bool canEdit = true;
    private bool allVerificationsComplete = true;
    private bool refreshList;
    private IContainer components;
    private GroupContainer groupContainerTimeline;
    private GridView lvwTimeline;
    private GridView lvwDocuments;
    private ToolTip toolTip1;
    private GradientPanel gradientPanel1;
    private Label labelDocuments;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnEditVerification;
    private PictureBox picBoxComplete;
    private PictureBox picBoxNotComplete;
    private FlowLayoutPanel flowLayoutPanel2;
    private StandardIconButton btnViewDoc;
    private StandardIconButton btnEditDoc;
    private StandardIconButton btnAddDoc;

    public VerificationDetailsControl(
      Sessions.Session session,
      LoanData loan,
      VerificationTimelineType editMode)
    {
      this.session = session;
      this.loan = loan;
      this.editMode = editMode;
      this.InitializeComponent();
      this.timelineList = this.loan.GetVerficiationTimelineLogs(this.editMode);
      this.documentList = this.loan.GetVerficiationDocuments(this.editMode);
      this.allDocsList = this.loan.GetLogList().GetAllDocuments();
      this.applySecurity();
      this.Dock = DockStyle.Fill;
      this.groupContainerTimeline.Text = this.editMode != VerificationTimelineType.Employment ? (this.editMode != VerificationTimelineType.Income ? (this.editMode != VerificationTimelineType.Asset ? "Monthly Obligation Verification Timeline" : "Asset Verification Timeline") : "Income Verification Timeline") : "Employment Status Verification Timeline";
      this.ReloadList();
      this.toolTip1.SetToolTip((Control) this.picBoxComplete, "All verifications completed.");
      this.toolTip1.SetToolTip((Control) this.picBoxNotComplete, "One or more verifications have not been completed, or how they were verified has not been completed.");
      this.toolTip1.SetToolTip((Control) this.btnEditVerification, "Edit Timeline");
      this.lvwTimeline_SelectedIndexChanged((object) null, (EventArgs) null);
      this.lvwDocuments_SelectedIndexChanged((object) null, (EventArgs) null);
      this.countDocuments();
      this.ParentChanged += new EventHandler(this.VerificationDetailsControl_ParentChanged);
    }

    private void applySecurity()
    {
      try
      {
        this.session.LoanDataMgr.LockLoanWithExclusiveA(false);
      }
      catch (Exception ex)
      {
        this.canAddNew = this.canEdit = false;
        this.btnEditVerification.Visible = this.btnAddDoc.Visible = this.btnEditDoc.Visible = false;
        return;
      }
      if (this.session.EncompassEdition == EncompassEdition.Broker || this.session.UserInfo.IsSuperAdministrator())
        return;
      FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.canAddNew = aclManager.GetUserApplicationRight(AclFeature.ToolsTab_VerificationNew);
      this.canEdit = aclManager.GetUserApplicationRight(AclFeature.ToolsTab_VerificationEdit);
    }

    public void ReloadList()
    {
      this.allVerificationsComplete = true;
      this.lvwTimeline.BeginUpdate();
      if (this.lvwTimeline.Items != null)
        this.lvwTimeline.Items.Clear();
      if (this.timelineList != null)
      {
        for (int i = 0; i < this.timelineList.TimelineCount; ++i)
        {
          VerificationTimelineLog timelineAt = this.timelineList.GetTimelineAt(i);
          if (timelineAt.TimelineType == this.editMode)
          {
            GVItem gvItem = new GVItem();
            for (int index = 1; index <= 8; ++index)
              gvItem.SubItems.Add((object) "");
            this.lvwTimeline.Items.Add(this.buildTimelineGVItem(gvItem, timelineAt, false));
            if (string.IsNullOrEmpty(timelineAt.HowCompleted) || string.IsNullOrEmpty(timelineAt.CompletedBy) || timelineAt.DateCompleted == DateTime.MinValue)
              this.allVerificationsComplete = false;
          }
        }
      }
      this.lvwDocuments.BeginUpdate();
      if (this.lvwDocuments.Items != null)
        this.lvwDocuments.Items.Clear();
      if (this.documentList != null && this.documentList.DocumentCount > 0)
      {
        for (int i = 0; i < this.documentList.DocumentCount; ++i)
        {
          VerificationDocument documentAt = this.documentList.GetDocumentAt(i);
          if (documentAt.TimelineType == this.editMode)
          {
            GVItem gvItem = new GVItem();
            for (int index = 1; index <= 2; ++index)
              gvItem.SubItems.Add((object) "");
            this.lvwDocuments.Items.Add(this.buildDocumentGVItem(gvItem, documentAt, false));
          }
        }
      }
      if (this.lvwDocuments.Items != null && this.lvwTimeline.Items != null)
        this.reloadEFolderDocs();
      this.lvwTimeline.EndUpdate();
      this.lvwDocuments.EndUpdate();
      this.showVerificationAlert();
    }

    private void reloadEFolderDocs()
    {
      this.loan.LogRecordAdded -= new LogRecordEventHandler(this.logRecordChanged);
      this.loan.LogRecordChanged -= new LogRecordEventHandler(this.logRecordChanged);
      this.loan.LogRecordRemoved -= new LogRecordEventHandler(this.logRecordChanged);
      foreach (DocumentLog allDocs in this.allDocsList)
      {
        DocumentVerificationType[] verificationTypeArray = allDocs.Verifications.Get(this.editMode);
        if (verificationTypeArray.Length != 0)
        {
          GVItem gvItem = new GVItem();
          for (int index = 1; index <= 2; ++index)
            gvItem.SubItems.Add((object) "");
          gvItem.Text = allDocs.Title;
          gvItem.SubItems[1].Text = DateTime.Now.ToString("MM/dd/yyyy");
          gvItem.SubItems[2].Text = allDocs.DateExpires == DateTime.MinValue ? "" : allDocs.DateExpires.ToString("MM/dd/yyyy");
          gvItem.Tag = (object) allDocs;
          this.lvwDocuments.Items.Add(gvItem);
          foreach (DocumentVerificationType verificationType in verificationTypeArray)
          {
            if (verificationType.BorrowerType == LoanBorrowerType.Both || verificationType.BorrowerType == LoanBorrowerType.Borrower)
              this.lvwTimeline.Items.Add(this.buildTimelineGVItem("B", verificationType, allDocs, false));
            if (verificationType.BorrowerType == LoanBorrowerType.Both || verificationType.BorrowerType == LoanBorrowerType.Coborrower)
              this.lvwTimeline.Items.Add(this.buildTimelineGVItem("C", verificationType, allDocs, false));
            if (string.IsNullOrEmpty(verificationType.HowVerified) || string.IsNullOrEmpty(allDocs.ReviewedBy) || allDocs.DateReviewed == DateTime.MinValue)
              this.allVerificationsComplete = false;
          }
        }
      }
      this.loan.LogRecordAdded += new LogRecordEventHandler(this.logRecordChanged);
      this.loan.LogRecordChanged += new LogRecordEventHandler(this.logRecordChanged);
      this.loan.LogRecordRemoved += new LogRecordEventHandler(this.logRecordChanged);
    }

    private void showVerificationAlert()
    {
      this.picBoxComplete.Visible = this.allVerificationsComplete;
      this.picBoxNotComplete.Visible = !this.allVerificationsComplete;
    }

    private void btnEditVerification_Click(object sender, EventArgs e)
    {
      if (this.lvwTimeline.SelectedItems.Count == 0)
        return;
      if (this.lvwTimeline.SelectedItems[0].Tag is VerificationTimelineLog)
      {
        using (VerificationTimelineDetailForm timelineDetailForm = new VerificationTimelineDetailForm((VerificationTimelineLog) this.lvwTimeline.SelectedItems[0].Tag, this.editMode, !this.canEdit))
        {
          if (timelineDetailForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
          {
            this.loan.UpdateVerificationTimelineLog(timelineDetailForm.VerificationLog);
            this.buildTimelineGVItem(this.lvwTimeline.SelectedItems[0], timelineDetailForm.VerificationLog, true);
            if (string.IsNullOrEmpty(timelineDetailForm.VerificationLog.HowCompleted) || string.IsNullOrEmpty(timelineDetailForm.VerificationLog.CompletedBy) || timelineDetailForm.VerificationLog.DateCompleted == DateTime.MinValue)
              this.allVerificationsComplete = false;
            this.showVerificationAlert();
          }
        }
      }
      if (!(this.lvwTimeline.SelectedItems[0].Tag is DocumentLog))
        return;
      using (ATRQMDialog atrqmDialog = new ATRQMDialog((DocumentLog) this.lvwTimeline.SelectedItems[0].Tag, this.editMode))
      {
        if (atrqmDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.ReloadList();
      }
    }

    private GVItem buildTimelineGVItem(
      string borrText,
      DocumentVerificationType verificationType,
      DocumentLog docLog,
      bool selected)
    {
      GVItem gvItem = new GVItem();
      gvItem.Text = borrText;
      for (int index = 1; index <= 8; ++index)
        gvItem.SubItems.Add((object) "");
      if (this.editMode == VerificationTimelineType.Employment)
      {
        DocumentVerificationEmployment verificationEmployment = (DocumentVerificationEmployment) verificationType;
        gvItem.SubItems[1].Text = EnumUtil.GetEnumDescription((Enum) verificationEmployment.EmploymentType);
      }
      else if (this.editMode == VerificationTimelineType.Income)
      {
        DocumentVerificationIncome verificationIncome = (DocumentVerificationIncome) verificationType;
        gvItem.SubItems[1].Text = EnumUtil.GetEnumDescription((Enum) verificationIncome.IncomeType);
        if (verificationIncome.IncomeType == IncomeType.TaxReturn)
          gvItem.SubItems[1].Text = gvItem.SubItems[1].Text + ": " + verificationIncome.TaxReturnYear.ToString();
        if (verificationIncome.IncomeType == IncomeType.OtherEmployment)
          gvItem.SubItems[1].Text = "Other Employment: " + verificationIncome.OtherDescription;
        if (verificationIncome.IncomeType == IncomeType.OtherNonEmployment)
          gvItem.SubItems[1].Text = "Other Non-Employment: " + verificationIncome.OtherDescription;
      }
      else if (this.editMode == VerificationTimelineType.Asset)
      {
        DocumentVerificationAsset verificationAsset = (DocumentVerificationAsset) verificationType;
        gvItem.SubItems[1].Text = EnumUtil.GetEnumDescription((Enum) verificationAsset.AssetType);
        if (verificationAsset.AssetType == AssetType.Other)
          gvItem.SubItems[1].Text = gvItem.SubItems[1].Text + ": " + verificationAsset.OtherDescription;
      }
      else if (this.editMode == VerificationTimelineType.Obligation)
      {
        DocumentVerificationObligation verificationObligation = (DocumentVerificationObligation) verificationType;
        gvItem.SubItems[1].Text = EnumUtil.GetEnumDescription((Enum) verificationObligation.ObligationType);
        if (verificationObligation.ObligationType == ObligationType.MortgageLate)
          gvItem.SubItems[1].Text = "Mortgage Lates: How Many: " + verificationObligation.MortageLateCount + " delinquencies";
        if (verificationObligation.ObligationType == ObligationType.SecondLien)
          gvItem.SubItems[1].Text = "2nd Lien: How Much: $" + verificationObligation.Amount.ToString() + ".00/month";
        if (verificationObligation.ObligationType == ObligationType.HELOC)
          gvItem.SubItems[1].Text = "HELOC: How Much: $" + verificationObligation.Amount.ToString() + ".00/month";
        if (verificationObligation.ObligationType == ObligationType.OtherMonthlyObligation)
          gvItem.SubItems[1].Text = "Other Monthly Obligatio: " + verificationObligation.OtherDescription;
        if (verificationObligation.ObligationType == ObligationType.OtherCreditHistory)
          gvItem.SubItems[1].Text = "Other Credit History: " + verificationObligation.OtherDescription;
      }
      else
        gvItem.SubItems[1].Text = "";
      gvItem.SubItems[2].Text = verificationType.HowVerified;
      gvItem.SubItems[3].Text = docLog.ReviewedBy;
      gvItem.SubItems[4].Text = docLog.DateReviewed == DateTime.MinValue ? "" : docLog.DateReviewed.ToString("MM/dd/yyyy");
      gvItem.SubItems[5].Text = docLog.ReviewedBy;
      gvItem.SubItems[6].Text = docLog.DateReviewed == DateTime.MinValue ? "" : docLog.DateReviewed.ToString("MM/dd/yyyy");
      gvItem.SubItems[7].Text = "Y";
      gvItem.SubItems[8].Text = docLog.DateReceived == DateTime.MinValue ? "" : docLog.DateReceived.ToString("MM/dd/yyyy");
      gvItem.Tag = (object) docLog;
      gvItem.Selected = selected;
      return gvItem;
    }

    private GVItem buildTimelineGVItem(GVItem item, VerificationTimelineLog log, bool selected)
    {
      item.Text = log.BorrowerType == LoanBorrowerType.Coborrower ? "C" : "B";
      if (this.editMode == VerificationTimelineType.Employment)
      {
        VerificationTimelineEmploymentLog timelineEmploymentLog = (VerificationTimelineEmploymentLog) log;
        item.SubItems[1].Text = timelineEmploymentLog.BuildWhatVerified();
      }
      else if (this.editMode == VerificationTimelineType.Income)
      {
        VerificationTimelineIncomeLog timelineIncomeLog = (VerificationTimelineIncomeLog) log;
        item.SubItems[1].Text = timelineIncomeLog.BuildWhatVerified();
      }
      else if (this.editMode == VerificationTimelineType.Asset)
      {
        VerificationTimelineAssetLog timelineAssetLog = (VerificationTimelineAssetLog) log;
        item.SubItems[1].Text = timelineAssetLog.BuildWhatVerified();
      }
      else if (this.editMode == VerificationTimelineType.Obligation)
      {
        VerificationTimelineObligationLog timelineObligationLog = (VerificationTimelineObligationLog) log;
        item.SubItems[1].Text = timelineObligationLog.BuildWhatVerified();
      }
      else
        item.SubItems[1].Text = "";
      item.SubItems[2].Text = log.HowCompleted;
      item.SubItems[3].Text = log.CompletedBy;
      item.SubItems[4].Text = log.DateCompleted == DateTime.MinValue ? "" : log.DateCompleted.ToString("MM/dd/yyyy");
      item.SubItems[5].Text = log.ReviewedBy;
      item.SubItems[6].Text = log.DateReviewed == DateTime.MinValue ? "" : log.DateReviewed.ToString("MM/dd/yyyy");
      item.SubItems[7].Text = log.EFolderAttached ? "Y" : "N";
      item.SubItems[8].Text = log.DateUploaded == DateTime.MinValue ? "" : log.DateUploaded.ToString("MM/dd/yyyy");
      item.Tag = (object) log;
      item.Selected = selected;
      return item;
    }

    private void btnAddDoc_Click(object sender, EventArgs e)
    {
      using (SelectVerificationDocumentDialog verificationDocumentDialog = new SelectVerificationDocumentDialog(this.session.LoanDataMgr))
      {
        if (verificationDocumentDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        using (ATRQMDialog atrqmDialog = new ATRQMDialog(verificationDocumentDialog.Document, this.editMode))
        {
          if (atrqmDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          this.ReloadList();
        }
      }
    }

    private void btnEditDoc_Click(object sender, EventArgs e)
    {
      if (this.lvwDocuments.SelectedItems.Count == 0)
        return;
      if (this.lvwDocuments.SelectedItems[0].Tag is VerificationDocument)
      {
        using (VerificationDocumentDetailForm documentDetailForm = new VerificationDocumentDetailForm((VerificationDocument) this.lvwDocuments.SelectedItems[0].Tag, this.editMode))
        {
          if (documentDetailForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
          {
            this.loan.UpdateVerificationDocument(documentDetailForm.CurrentVerificationDocument);
            this.buildDocumentGVItem(this.lvwDocuments.SelectedItems[0], documentDetailForm.CurrentVerificationDocument, true);
          }
        }
      }
      if (!(this.lvwDocuments.SelectedItems[0].Tag is DocumentLog))
        return;
      using (ATRQMDialog atrqmDialog = new ATRQMDialog((DocumentLog) this.lvwDocuments.SelectedItems[0].Tag, this.editMode))
      {
        if (atrqmDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.ReloadList();
      }
    }

    private void btnViewDoc_Click(object sender, EventArgs e)
    {
      if (this.lvwDocuments.SelectedItems.Count == 0 || !this.canEdit)
        return;
      if (this.lvwDocuments.SelectedItems[0].Tag is VerificationDocument)
        this.btnEditDoc_Click((object) null, (EventArgs) null);
      if (!(this.lvwDocuments.SelectedItems[0].Tag is DocumentLog))
        return;
      DocumentLog tag = (DocumentLog) this.lvwDocuments.SelectedItems[0].Tag;
      Session.Application.GetService<IEFolder>().View(this.session.LoanDataMgr, tag);
    }

    private GVItem buildDocumentGVItem(GVItem item, VerificationDocument document, bool selected)
    {
      item.Text = document.DocName;
      GVSubItem subItem1 = item.SubItems[1];
      DateTime dateTime;
      string str1;
      if (!(document.CurrentDate == DateTime.MinValue))
      {
        dateTime = document.CurrentDate;
        str1 = dateTime.ToString("MM/dd/yyyy");
      }
      else
        str1 = "";
      subItem1.Text = str1;
      GVSubItem subItem2 = item.SubItems[2];
      string str2;
      if (!(document.ExpirationDate == DateTime.MinValue))
      {
        dateTime = document.ExpirationDate;
        str2 = dateTime.ToString("MM/dd/yyyy");
      }
      else
        str2 = "";
      subItem2.Text = str2;
      item.Tag = (object) document;
      item.Selected = selected;
      return item;
    }

    private void lvwDocuments_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnEditDoc.Enabled = this.lvwDocuments.SelectedItems.Count == 1 && this.canEdit;
      this.btnViewDoc.Enabled = this.lvwDocuments.SelectedItems.Count == 1 && this.canEdit;
      this.btnAddDoc.Enabled = this.canAddNew;
    }

    private void lvwTimeline_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnEditVerification.Enabled = this.lvwTimeline.SelectedItems.Count == 1 && this.canEdit;
    }

    private void countDocuments()
    {
      this.labelDocuments.Text = "Documents (" + (object) this.lvwDocuments.Items.Count + ")";
    }

    private VerificationTimelineLog createVerificationLog()
    {
      VerificationTimelineLog verificationLog = this.editMode != VerificationTimelineType.Employment ? (this.editMode != VerificationTimelineType.Income ? (this.editMode != VerificationTimelineType.Asset ? (VerificationTimelineLog) new VerificationTimelineObligationLog() : (VerificationTimelineLog) new VerificationTimelineAssetLog()) : (VerificationTimelineLog) new VerificationTimelineIncomeLog()) : (VerificationTimelineLog) new VerificationTimelineEmploymentLog();
      verificationLog.Date = DateTime.Now;
      verificationLog.SetGuid(Guid.NewGuid());
      verificationLog.TimelineType = this.editMode;
      return verificationLog;
    }

    private VerificationDocument createVerificationDocument()
    {
      VerificationDocument verificationDocument = new VerificationDocument();
      verificationDocument.TimelineType = this.editMode;
      verificationDocument.Date = DateTime.Now;
      verificationDocument.SetGuid(Guid.NewGuid());
      return verificationDocument;
    }

    private void lvwTimeline_DoubleClick(object sender, EventArgs e)
    {
      this.btnEditVerification_Click((object) null, (EventArgs) null);
    }

    private void lvwDocuments_DoubleClick(object sender, EventArgs e)
    {
      this.btnViewDoc_Click((object) null, (EventArgs) null);
    }

    private void VerificationDetailsControl_ParentChanged(object sender, EventArgs e)
    {
      if (this.Parent == null)
        Session.MainForm.Activated -= new EventHandler(this.MainForm_Activated);
      else
        Session.MainForm.Activated += new EventHandler(this.MainForm_Activated);
    }

    private void MainForm_Activated(object sender, EventArgs e)
    {
      if (!this.refreshList)
        return;
      this.ReloadList();
      this.refreshList = false;
    }

    private void logRecordChanged(object source, LogRecordEventArgs e)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke((Delegate) new LogRecordEventHandler(this.logRecordChanged), source, (object) e);
      }
      else
      {
        if (!(e.LogRecord is DocumentLog))
          return;
        if (Session.MainForm == Form.ActiveForm)
          this.ReloadList();
        else
          this.refreshList = true;
      }
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
      this.lvwDocuments = new GridView();
      this.toolTip1 = new ToolTip(this.components);
      this.btnEditVerification = new StandardIconButton();
      this.btnEditDoc = new StandardIconButton();
      this.btnAddDoc = new StandardIconButton();
      this.groupContainerTimeline = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.picBoxComplete = new PictureBox();
      this.picBoxNotComplete = new PictureBox();
      this.gradientPanel1 = new GradientPanel();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.btnViewDoc = new StandardIconButton();
      this.labelDocuments = new Label();
      this.lvwTimeline = new GridView();
      ((ISupportInitialize) this.btnEditVerification).BeginInit();
      ((ISupportInitialize) this.btnEditDoc).BeginInit();
      ((ISupportInitialize) this.btnAddDoc).BeginInit();
      this.groupContainerTimeline.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.picBoxComplete).BeginInit();
      ((ISupportInitialize) this.picBoxNotComplete).BeginInit();
      this.gradientPanel1.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      ((ISupportInitialize) this.btnViewDoc).BeginInit();
      this.SuspendLayout();
      this.lvwDocuments.AllowMultiselect = false;
      this.lvwDocuments.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "gvName";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 294;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "gvDate";
      gvColumn2.Text = "Current Date";
      gvColumn2.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn2.Width = 86;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "gvExpiration";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Expiration Date";
      gvColumn3.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn3.Width = 620;
      this.lvwDocuments.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.lvwDocuments.Dock = DockStyle.Fill;
      this.lvwDocuments.Location = new Point(1, 157);
      this.lvwDocuments.Name = "lvwDocuments";
      this.lvwDocuments.Size = new Size(1000, 92);
      this.lvwDocuments.SortIconVisible = false;
      this.lvwDocuments.SortOption = GVSortOption.None;
      this.lvwDocuments.TabIndex = 43;
      this.lvwDocuments.SelectedIndexChanged += new EventHandler(this.lvwDocuments_SelectedIndexChanged);
      this.lvwDocuments.DoubleClick += new EventHandler(this.lvwDocuments_DoubleClick);
      this.btnEditVerification.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEditVerification.BackColor = Color.Transparent;
      this.btnEditVerification.Location = new Point(70, 3);
      this.btnEditVerification.MouseDownImage = (Image) null;
      this.btnEditVerification.Name = "btnEditVerification";
      this.btnEditVerification.Size = new Size(16, 16);
      this.btnEditVerification.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditVerification.TabIndex = 47;
      this.btnEditVerification.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEditVerification, "Edit Timeline");
      this.btnEditVerification.Click += new EventHandler(this.btnEditVerification_Click);
      this.btnEditDoc.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEditDoc.BackColor = Color.Transparent;
      this.btnEditDoc.Location = new Point(47, 3);
      this.btnEditDoc.MouseDownImage = (Image) null;
      this.btnEditDoc.Name = "btnEditDoc";
      this.btnEditDoc.Size = new Size(16, 16);
      this.btnEditDoc.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditDoc.TabIndex = 49;
      this.btnEditDoc.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEditDoc, "Edit Document");
      this.btnEditDoc.Click += new EventHandler(this.btnEditDoc_Click);
      this.btnAddDoc.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddDoc.BackColor = Color.Transparent;
      this.btnAddDoc.Location = new Point(25, 3);
      this.btnAddDoc.MouseDownImage = (Image) null;
      this.btnAddDoc.Name = "btnAddDoc";
      this.btnAddDoc.Size = new Size(16, 16);
      this.btnAddDoc.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddDoc.TabIndex = 50;
      this.btnAddDoc.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddDoc, "New Document");
      this.btnAddDoc.Click += new EventHandler(this.btnAddDoc_Click);
      this.groupContainerTimeline.BackColor = Color.White;
      this.groupContainerTimeline.Controls.Add((Control) this.flowLayoutPanel1);
      this.groupContainerTimeline.Controls.Add((Control) this.lvwDocuments);
      this.groupContainerTimeline.Controls.Add((Control) this.gradientPanel1);
      this.groupContainerTimeline.Controls.Add((Control) this.lvwTimeline);
      this.groupContainerTimeline.Dock = DockStyle.Fill;
      this.groupContainerTimeline.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerTimeline.Location = new Point(0, 0);
      this.groupContainerTimeline.Name = "groupContainerTimeline";
      this.groupContainerTimeline.Size = new Size(1002, 250);
      this.groupContainerTimeline.TabIndex = 3;
      this.groupContainerTimeline.Text = "Verification Timeline";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnEditVerification);
      this.flowLayoutPanel1.Controls.Add((Control) this.picBoxComplete);
      this.flowLayoutPanel1.Controls.Add((Control) this.picBoxNotComplete);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(906, 3);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(89, 22);
      this.flowLayoutPanel1.TabIndex = 49;
      this.picBoxComplete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picBoxComplete.BackColor = Color.Transparent;
      this.picBoxComplete.Image = (Image) Resources.check_mark_green;
      this.picBoxComplete.Location = new Point(48, 3);
      this.picBoxComplete.Name = "picBoxComplete";
      this.picBoxComplete.Size = new Size(16, 16);
      this.picBoxComplete.TabIndex = 48;
      this.picBoxComplete.TabStop = false;
      this.picBoxNotComplete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.picBoxNotComplete.BackColor = Color.Transparent;
      this.picBoxNotComplete.Image = (Image) Resources.flag_compliance_10x10;
      this.picBoxNotComplete.Location = new Point(26, 3);
      this.picBoxNotComplete.Name = "picBoxNotComplete";
      this.picBoxNotComplete.Size = new Size(16, 16);
      this.picBoxNotComplete.TabIndex = 49;
      this.picBoxNotComplete.TabStop = false;
      this.gradientPanel1.Controls.Add((Control) this.flowLayoutPanel2);
      this.gradientPanel1.Controls.Add((Control) this.labelDocuments);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.White;
      this.gradientPanel1.GradientColor2 = Color.Silver;
      this.gradientPanel1.Location = new Point(1, 133);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(1000, 24);
      this.gradientPanel1.TabIndex = 6;
      this.flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.Controls.Add((Control) this.btnViewDoc);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnEditDoc);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnAddDoc);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(905, 3);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(86, 22);
      this.flowLayoutPanel2.TabIndex = 50;
      this.btnViewDoc.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnViewDoc.BackColor = Color.Transparent;
      this.btnViewDoc.Location = new Point(70, 3);
      this.btnViewDoc.Margin = new Padding(4, 3, 0, 3);
      this.btnViewDoc.MouseDownImage = (Image) null;
      this.btnViewDoc.Name = "btnViewDoc";
      this.btnViewDoc.Size = new Size(16, 17);
      this.btnViewDoc.StandardButtonType = StandardIconButton.ButtonType.ZoomInButton;
      this.btnViewDoc.TabIndex = 28;
      this.btnViewDoc.TabStop = false;
      this.btnViewDoc.Click += new EventHandler(this.btnViewDoc_Click);
      this.labelDocuments.AutoSize = true;
      this.labelDocuments.BackColor = Color.Transparent;
      this.labelDocuments.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.labelDocuments.Location = new Point(10, 6);
      this.labelDocuments.Name = "labelDocuments";
      this.labelDocuments.Size = new Size(76, 13);
      this.labelDocuments.TabIndex = 49;
      this.labelDocuments.Text = "Documents (0)";
      this.lvwTimeline.AllowMultiselect = false;
      this.lvwTimeline.BorderStyle = BorderStyle.None;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "gvBC";
      gvColumn4.Text = "B/C";
      gvColumn4.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn4.Width = 34;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "gvVerified";
      gvColumn5.Text = "What was verified";
      gvColumn5.Width = 187;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "gvComplete";
      gvColumn6.Text = "How was the Verification Completed";
      gvColumn6.Width = 187;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "gvBy";
      gvColumn7.Text = "Verification Completed By";
      gvColumn7.Width = 137;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "gvDate";
      gvColumn8.Text = "Date Verification Completed";
      gvColumn8.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn8.Width = 147;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "gvReview";
      gvColumn9.Text = "Verifications Reviewed By";
      gvColumn9.Width = 138;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "gvDateReviewed";
      gvColumn10.Text = "Date Reviewed";
      gvColumn10.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn10.Width = 86;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "gvDocument";
      gvColumn11.Text = "Document Attached to eFolder";
      gvColumn11.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn11.Width = 164;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "gvUpload";
      gvColumn12.Text = "Date uploaded to eFolder";
      gvColumn12.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn12.Width = 136;
      this.lvwTimeline.Columns.AddRange(new GVColumn[9]
      {
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9,
        gvColumn10,
        gvColumn11,
        gvColumn12
      });
      this.lvwTimeline.Dock = DockStyle.Top;
      this.lvwTimeline.Location = new Point(1, 26);
      this.lvwTimeline.Name = "lvwTimeline";
      this.lvwTimeline.Size = new Size(1000, 107);
      this.lvwTimeline.SortIconVisible = false;
      this.lvwTimeline.SortOption = GVSortOption.None;
      this.lvwTimeline.TabIndex = 43;
      this.lvwTimeline.SelectedIndexChanged += new EventHandler(this.lvwTimeline_SelectedIndexChanged);
      this.lvwTimeline.DoubleClick += new EventHandler(this.lvwTimeline_DoubleClick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainerTimeline);
      this.Name = nameof (VerificationDetailsControl);
      this.Size = new Size(1002, 250);
      ((ISupportInitialize) this.btnEditVerification).EndInit();
      ((ISupportInitialize) this.btnEditDoc).EndInit();
      ((ISupportInitialize) this.btnAddDoc).EndInit();
      this.groupContainerTimeline.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.picBoxComplete).EndInit();
      ((ISupportInitialize) this.picBoxNotComplete).EndInit();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.flowLayoutPanel2.ResumeLayout(false);
      ((ISupportInitialize) this.btnViewDoc).EndInit();
      this.ResumeLayout(false);
    }

    public enum BorrowerEditMode
    {
      Borrower,
      CoBorrower,
    }
  }
}
