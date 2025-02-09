// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Documents.AllDocumentsDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Conditions;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.eFolder.UI;
using EllieMae.EMLite.eFolder.Viewers;
using EllieMae.EMLite.LoanUtils.EnhancedConditions;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Documents
{
  public class AllDocumentsDialog : Form
  {
    private const string className = "AllDocumentsDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private static List<AllDocumentsDialog> _instanceList = new List<AllDocumentsDialog>();
    private LoanDataMgr loanDataMgr;
    private StackingOrderSetTemplate stackingTemplate;
    private ConditionType condType;
    private DocumentGroupSetup groupSetup;
    private DocumentTrackingSetup docSetup;
    private GridViewDataManager gvDocumentsMgr;
    private GridViewDataManager gvConditionsMgr;
    private GridViewDataManager gvDocumentCommentsMgr;
    private GridViewDataManager gvConditionCommentsMgr;
    private GridViewDataManager gvFilesMgr;
    private GridViewDataManager gvTrackingMgr;
    private Point dragPoint = Point.Empty;
    private eFolderAccessRights userRights;
    private bool canEditCondition;
    private RoleInfo underwriterRole;
    private int statusRowHeight;
    private const string allEnhancedConditionsName = "<All Conditions>";
    private EnhancedConditionType[] enhancedConditionTypes;
    private EnhancedConditionTemplate[] conditionTemplates;
    private bool refreshDocuments;
    private bool refreshConditions;
    private bool isHideFileSizeEnabled;
    private CurrentDocumentsManager docMgr;
    private IContainer components;
    private GridView gvConditions;
    private ImageList imageList;
    private GroupContainer gcConditions;
    private CollapsibleSplitter csLeft;
    private Panel pnlLeft;
    private CollapsibleSplitter csDocuments;
    private GroupContainer gcDocuments;
    private GridView gvDocuments;
    private BorderPanel pnlDragDrop;
    private Label lblDragDrop;
    private Button btnClose;
    private Panel pnlClose;
    private ToolTip tooltip;
    private FlowLayoutPanel pnlToolbar;
    private StandardIconButton btnEditCondition;
    private StandardIconButton btnAddCondition;
    private EMHelpLink helpLink;
    private GradientPanel pnlStackingOrder;
    private ComboBox cboStackingOrder;
    private Label label2;
    private ComboBox cboCondType;
    private Panel pnlRight;
    private BorderPanel pnlViewer;
    private FileAttachmentViewerControl fileViewer;
    private GroupContainer gcInfo;
    private TabControl tabInfo;
    private TabPage pageFiles;
    private TabPage pageDocumentDetails;
    private GroupContainer gcDocumentDetails;
    private Panel pnlDetails;
    private CheckBox chkThirdParty;
    private CheckBox chkTPOWebcenterPortal;
    private CheckBox chkWebCenter;
    private Label lblDocuments;
    private Label lblAvailable;
    private ListBox lstGroups;
    private Label lblGroups;
    private ListBox lstDocumentConditions;
    private Label lblConditions;
    private TextBox txtAccess;
    private Button btnAccess;
    private Label lblAccess;
    private ComboBoxEx cboMilestone;
    private TextBox txtMilestone;
    private Label lblMilestone;
    private ComboBox cboDocumentBorrower;
    private TextBox txtDocumentBorrower;
    private Label lblBorrower;
    private GroupContainer gcDocumentStatus;
    private Panel pnlStatus;
    private StandardIconButton btnShippingReadyBy;
    private TextBox txtShippingReadyBy;
    private CheckBox chkShippingReady;
    private StandardIconButton btnUnderwritingReadyBy;
    private TextBox txtUnderwritingReadyBy;
    private CheckBox chkUnderwritingReady;
    private StandardIconButton btnDocumentReviewedBy;
    private StandardIconButton btnDocumentReceivedBy;
    private StandardIconButton btnDocumentRerequestedBy;
    private StandardIconButton btnDocumentRequestedBy;
    private TextBox txtDocumentReviewedBy;
    private TextBox txtDocumentReceivedBy;
    private TextBox txtDocumentRerequestedBy;
    private TextBox txtDocumentRequestedBy;
    private CheckBox chkDocumentReviewed;
    private CheckBox chkDocumentReceived;
    private CheckBox chkDocumentRerequested;
    private CheckBox chkDocumentRequested;
    private TextBox txtDocumentRequestedFrom;
    private Label lblCompany;
    private TextBox txtDateExpire;
    private TextBox txtDaysExpire;
    private Label lblDaysExpire;
    private TextBox txtDocumentDateDue;
    private TextBox txtDocumentDaysDue;
    private Label lblDaysDue;
    private TextBox txtDocumentDateRequested;
    private TextBox txtDocumentDateRerequested;
    private TextBox txtDocumentDateReceived;
    private TextBox txtDocumentDateReviewed;
    private TextBox txtDateUnderwritingReady;
    private TextBox txtDateShippingReady;
    private GroupContainer gcDocumentComments;
    private GridView gvDocumentComments;
    private GroupContainer gcDocumentCommentText;
    private ListBox lstDocuments;
    private CalendarButton btnDateShippingReady;
    private CalendarButton btnDateUnderwritingReady;
    private CalendarButton btnDocumentDateReviewed;
    private CalendarButton btnDocumentDateReceived;
    private CalendarButton btnDocumentDateRerequested;
    private CalendarButton btnDocumentDateRequested;
    private TextBox txtDocumentComment;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnDeleteDocumentComment;
    private StandardIconButton btnViewDocumentComment;
    private StandardIconButton btnAddDocumentComment;
    private TabPage pageConditionDetails;
    private GroupContainer gcConditionCommentText;
    private TextBox txtConditionComment;
    private GroupContainer gcConditionComments;
    private FlowLayoutPanel flowLayoutPanel2;
    private StandardIconButton btnDeleteConditionComment;
    private StandardIconButton btnViewConditionComment;
    private StandardIconButton btnAddConditionComment;
    private GridView gvConditionComments;
    private GroupContainer gcConditionStatus;
    private Panel panel1;
    private TextBox txtConditionRequestedFrom;
    private Label label1;
    private TextBox txtConditionDateDue;
    private TextBox txtConditionDaysDue;
    private Label label4;
    private GroupContainer gcConditionDetails;
    private Panel panel2;
    private ListBox lstConditions;
    private Label label5;
    private ComboBox cboConditionBorrower;
    private TextBox txtConditionBorrower;
    private Label label11;
    private CheckBox chkUnderwriter;
    private ComboBox cboPriorTo;
    private Label lblPriorTo;
    private ComboBox cboCategory;
    private Label lblCategory;
    private TextBox txtSource;
    private Label lblSource;
    private TextBox txtCategory;
    private TextBox txtPriorTo;
    private StandardIconButton btnConditionReceivedBy;
    private TextBox txtConditionReceivedBy;
    private DateTimePicker dateConditionReceived;
    private CheckBox chkConditionReceived;
    private StandardIconButton btnConditionRerequestedBy;
    private TextBox txtConditionRerequestedBy;
    private DateTimePicker dateConditionRerequested;
    private CheckBox chkConditionRerequested;
    private StandardIconButton btnConditionRequestedBy;
    private TextBox txtConditionRequestedBy;
    private DateTimePicker dateConditionRequested;
    private CheckBox chkConditionRequested;
    private StandardIconButton btnFulfilledBy;
    private TextBox txtFulfilledBy;
    private DateTimePicker dateFulfilled;
    private CheckBox chkFulfilled;
    private TextBox txtAddedBy;
    private TextBox txtAddedDate;
    private CheckBox chkAdded;
    private TextBox txtDateFulfilled;
    private TextBox txtConditionDateRequested;
    private TextBox txtConditionDateRerequested;
    private TextBox txtConditionDateReceived;
    private Label lblPrint;
    private CheckBox chkExternal;
    private CheckBox chkInternal;
    private CheckBox chkAllowToClear;
    private ComboBox cboOwner;
    private TextBox txtOwner;
    private Label lblOwner;
    private StandardIconButton btnWaivedBy;
    private TextBox txtWaivedBy;
    private DateTimePicker dateWaived;
    private CheckBox chkWaived;
    private StandardIconButton btnClearedBy;
    private TextBox txtClearedBy;
    private DateTimePicker dateCleared;
    private CheckBox chkCleared;
    private StandardIconButton btnRejectedBy;
    private TextBox txtRejectedBy;
    private DateTimePicker dateRejected;
    private CheckBox chkRejected;
    private TextBox txtDateCleared;
    private TextBox txtDateWaived;
    private TextBox txtDateRejected;
    private StandardIconButton btnConditionReviewedBy;
    private TextBox txtConditionReviewedBy;
    private DateTimePicker dateConditionReviewed;
    private TextBox txtConditionDateReviewed;
    private CheckBox chkConditionReviewed;
    private ComboBox cboRecipient;
    private TextBox txtRecipient;
    private Label lblRecipient;
    private ComboBox cboSource;
    private StandardIconButton btnSentBy;
    private TextBox txtSentBy;
    private DateTimePicker dateSent;
    private TextBox txtDateSent;
    private CheckBox chkSent;
    private Label label3;
    private ListBox lstATRQM;
    private GridView gvFiles;
    private TabPage pageConditionsEnhanced;
    private GroupContainer gcEnhancedConditionDetails;
    private Panel panel3;
    private ComboBox cboSourceEnhanced;
    private ComboBox cboPriorToEnhanced;
    private Label label9;
    private Label label12;
    private ListBox lstConditionsEnhanced;
    private Label label13;
    private ComboBox cboConditionBorrowerEnhanced;
    private Label label14;
    private TextBox txtConditionType;
    private GroupContainer groupContainer1;
    private Panel panel4;
    private TextBox txtRequestedFromEnhanced;
    private Label label21;
    private TextBox txtDateDueEnhanced;
    private TextBox txtDaysToReceiveEnhanced;
    private Label label22;
    private GridView gvTrackingEnhanced;
    private Panel panel5;
    private CommentCollectionControl commentCollectionEnhanced;
    private TextBox txtCommentEnhanced;
    private Button btnAddCommentEnhanced;
    private CheckBox chkExternalCommentEnhanced;
    private Label label6;

    public static void ShowInstance(
      LoanDataMgr loanDataMgr,
      DocumentLog[] selectedDocs,
      ConditionType condType)
    {
      AllDocumentsDialog allDocumentsDialog1 = (AllDocumentsDialog) null;
      foreach (AllDocumentsDialog instance in AllDocumentsDialog._instanceList)
      {
        if (instance.ConditionType == condType)
          allDocumentsDialog1 = instance;
      }
      if (allDocumentsDialog1 == null)
      {
        AllDocumentsDialog allDocumentsDialog2 = new AllDocumentsDialog(loanDataMgr, selectedDocs, condType);
        allDocumentsDialog2.FormClosing += new FormClosingEventHandler(AllDocumentsDialog._instance_FormClosing);
        allDocumentsDialog2.Show();
        AllDocumentsDialog._instanceList.Add(allDocumentsDialog2);
      }
      else
      {
        if (allDocumentsDialog1.WindowState == FormWindowState.Minimized)
          allDocumentsDialog1.WindowState = FormWindowState.Normal;
        allDocumentsDialog1.Activate();
      }
    }

    private static void _instance_FormClosing(object sender, FormClosingEventArgs e)
    {
      AllDocumentsDialog allDocumentsDialog = (AllDocumentsDialog) sender;
      AllDocumentsDialog._instanceList.Remove(allDocumentsDialog);
    }

    private AllDocumentsDialog(
      LoanDataMgr loanDataMgr,
      DocumentLog[] selectedDocs,
      ConditionType condType)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.docMgr = new CurrentDocumentsManager(this.loanDataMgr);
      this.userRights = new eFolderAccessRights(this.loanDataMgr);
      this.isHideFileSizeEnabled = string.Equals(Session.ConfigurationManager.GetCompanySetting("eFolder", "HideFileSize"), "TRUE", StringComparison.OrdinalIgnoreCase);
      if (condType == ConditionType.Underwriting)
      {
        if (this.userRights.CanAccessUnderwritingTab)
          this.condType = condType;
        else if (this.userRights.CanAccessPreliminaryTab)
          this.condType = ConditionType.Preliminary;
        else if (this.userRights.CanAccessPostClosingTab)
          this.condType = ConditionType.PostClosing;
        else if (this.userRights.CanAccessSellTab)
          this.condType = ConditionType.Sell;
      }
      else
        this.condType = condType;
      this.statusRowHeight = this.chkConditionRequested.Top - this.chkFulfilled.Top;
      this.initConditionTypeList();
      if (this.condType != ConditionType.Enhanced)
      {
        List<string> stringList = new List<string>((IEnumerable<string>) LoanConstants.PostClosingConditionSources);
        stringList.RemoveRange(stringList.Count - 2, 2);
        this.cboSource.Items.AddRange((object[]) stringList.ToArray());
      }
      this.initEventHandlers();
      this.initDocumentList();
      this.initConditionList();
      this.initDocumentCommentList();
      this.initDocumentBorrowerField();
      this.initMilestoneField();
      this.initGroupsField();
      this.initFileList();
      if (this.condType == ConditionType.Enhanced)
      {
        this.initConditionBorrowerEnhancedField();
        this.initTrackingList();
      }
      else
      {
        this.initConditionBorrowerField();
        this.initOwnerField();
        this.initFulfilledFields();
        this.initConditionCommentList();
      }
      this.initializeStackingOrder();
      this.loadDocumentList(selectedDocs);
      this.loadConditionList();
      if (this.condType != ConditionType.Enhanced)
        this.applyBasicConditionSecurity();
      if (selectedDocs != null)
        this.showDocuments();
      this.showHideTabs();
    }

    private void initializeStackingOrder()
    {
      this.cboStackingOrder.Items.Add((object) GridViewDataManager.DefaultStackingOrderName);
      this.cboStackingOrder.SelectedItem = (object) GridViewDataManager.DefaultStackingOrderName;
    }

    private void cboStackingOrder_DropDown(object sender, EventArgs e)
    {
      foreach (FileSystemEntry templateDirEntry in Session.ConfigurationManager.GetTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.StackingOrder, FileSystemEntry.PublicRoot))
      {
        FileSystemEntryListItem systemEntryListItem = new FileSystemEntryListItem(templateDirEntry);
        if (!this.cboStackingOrder.Items.Contains((object) systemEntryListItem))
          this.cboStackingOrder.Items.Add((object) systemEntryListItem);
      }
      this.cboStackingOrder.DropDown -= new EventHandler(this.cboStackingOrder_DropDown);
    }

    private void cboStackingOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboStackingOrder.SelectedItem is FileSystemEntryListItem)
      {
        this.stackingTemplate = (StackingOrderSetTemplate) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.StackingOrder, ((FileSystemEntryListItem) this.cboStackingOrder.SelectedItem).Entry);
        this.gvDocuments.SortOption = GVSortOption.Owner;
        this.gvDocuments.SortIconVisible = false;
        if (this.gvDocuments.Items.Count > 0)
          this.gvDocuments.ReSort();
      }
      else
      {
        this.stackingTemplate = (StackingOrderSetTemplate) null;
        this.gvDocuments.SortOption = GVSortOption.Auto;
        this.gvDocuments.SortIconVisible = true;
      }
      this.loadDocumentList((DocumentLog[]) null);
      this.showDocuments();
      this.showHideTabs();
    }

    private void gvDocuments_SortItems(object source, GVColumnSortEventArgs e)
    {
      if (this.stackingTemplate != null)
        this.gvDocuments.Items.Sort((IComparer<GVItem>) new DocumentSortComparer(this.loanDataMgr.LoanData, this.stackingTemplate));
      e.Cancel = true;
    }

    private void initDocumentList()
    {
      this.gvDocumentsMgr = new GridViewDataManager(this.gvDocuments, this.loanDataMgr);
      this.gvDocumentsMgr.CreateLayout(new TableLayout.Column[6]
      {
        GridViewDataManager.NameColumn,
        GridViewDataManager.BorrowerColumn,
        GridViewDataManager.DocStatusColumn,
        GridViewDataManager.DateColumn,
        GridViewDataManager.MilestoneColumn,
        GridViewDataManager.HasConditionsColumn
      });
      this.gvDocuments.Columns[5].Width = this.gvDocuments.Columns[3].Width;
      this.gvDocuments.Sort(0, SortOrder.Ascending);
    }

    private void loadDocumentList(DocumentLog[] selectedDocs)
    {
      DocumentLog[] array = this.loanDataMgr.LoanData.GetLogList().GetAllDocuments();
      if (this.stackingTemplate != null && this.stackingTemplate.FilterDocuments)
      {
        List<DocumentLog> documentLogList = new List<DocumentLog>();
        foreach (string docName in this.stackingTemplate.DocNames)
        {
          foreach (DocumentLog doc in array)
          {
            if (this.docMatchesStackingItem(doc, docName))
              documentLogList.Add(doc);
          }
        }
        array = documentLogList.ToArray();
      }
      foreach (DocumentLog doc in array)
      {
        int num = 0;
        if (this.loanDataMgr.FileAttachments.ContainsAttachment(doc))
          num = 1;
        GVItem itemByTag = this.gvDocuments.Items.GetItemByTag((object) doc);
        if (itemByTag == null)
        {
          this.gvDocumentsMgr.AddItem(doc).ImageIndex = num;
        }
        else
        {
          this.gvDocumentsMgr.RefreshItem(itemByTag, doc);
          if (itemByTag.ImageIndex != num)
            itemByTag.ImageIndex = num;
          if (selectedDocs != null)
            itemByTag.Selected = Array.IndexOf<DocumentLog>(selectedDocs, doc) >= 0;
        }
      }
      List<GVItem> gvItemList = new List<GVItem>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocuments.Items)
      {
        if (Array.IndexOf<object>((object[]) array, gvItem.Tag) < 0)
          gvItemList.Add(gvItem);
      }
      foreach (GVItem gvItem in gvItemList)
        this.gvDocuments.Items.Remove(gvItem);
      this.gvDocuments.ReSort();
    }

    private bool docMatchesStackingItem(DocumentLog doc, string stackingName)
    {
      return doc.Title.ToLower() == stackingName.ToLower() || this.stackingTemplate.NDEDocGroups.Contains((object) stackingName) && doc.GroupName.ToLower() == stackingName.ToLower();
    }

    private void gvDocuments_Enter(object sender, EventArgs e)
    {
      this.showDocuments();
      this.showHideTabs();
    }

    private void gvDocuments_BeforeSelectedIndexCommitted(object sender, CancelEventArgs e)
    {
      if (this.fileViewer.CanCloseViewer())
        return;
      e.Cancel = true;
    }

    private void gvDocuments_SelectedIndexCommitted(object sender, EventArgs e)
    {
      this.showDocuments();
      this.showHideTabs();
    }

    private void showDocuments()
    {
      this.showDocumentFiles();
      this.refreshDocumentDetailsTab();
    }

    private void refreshDocumentDetailsTab()
    {
      this.cboDocumentBorrower.SelectedItem = (object) null;
      this.chkWebCenter.CheckState = CheckState.Unchecked;
      this.chkTPOWebcenterPortal.CheckState = CheckState.Unchecked;
      this.chkThirdParty.CheckState = CheckState.Unchecked;
      this.applyUserDocumentSecurity();
      this.loadDocumentBorrowerField();
      this.loadMilestoneField();
      this.txtAccess.Text = this.docMgr.Access;
      this.loadDocumentConditionsField();
      this.loadATRQMField();
      this.loadGroupsField();
      if (this.docMgr.WebCenterIntValue == -1)
        this.chkWebCenter.CheckState = CheckState.Indeterminate;
      else
        this.chkWebCenter.Checked = Convert.ToBoolean(this.docMgr.WebCenterIntValue);
      if (this.docMgr.TPOWebCenterPortalIntValue == -1)
        this.chkTPOWebcenterPortal.CheckState = CheckState.Indeterminate;
      else
        this.chkTPOWebcenterPortal.Checked = Convert.ToBoolean(this.docMgr.TPOWebCenterPortalIntValue);
      if (this.docMgr.ThirdPartyIntValue == -1)
        this.chkThirdParty.CheckState = CheckState.Indeterminate;
      else
        this.chkThirdParty.Checked = Convert.ToBoolean(this.docMgr.ThirdPartyIntValue);
      this.txtDocumentDateDue.Text = this.docMgr.DateDue;
      this.txtDocumentDaysDue.Text = string.Empty;
      if (this.docMgr.DaysDue > 0)
        this.txtDocumentDaysDue.Text = this.docMgr.DaysDue.ToString();
      this.txtDateExpire.Text = this.docMgr.DateExpire;
      this.txtDaysExpire.Text = string.Empty;
      if (this.docMgr.DaysExpire > 0)
        this.txtDaysExpire.Text = this.docMgr.DaysExpire.ToString();
      this.txtDocumentRequestedFrom.Text = this.docMgr.RequestedFrom;
      this.loadDocumentRequestedFields();
      this.loadDocumentRerequestedFields();
      this.loadDocumentReceivedFields();
      this.loadDocumentReviewedFields();
      this.loadUnderwritingReadyFields();
      this.loadShippingReadyFields();
      this.loadDocumentCommentList();
    }

    private void initFileList()
    {
      this.gvFilesMgr = new GridViewDataManager(this.gvFiles, this.loanDataMgr);
      TableLayout.Column[] columnList = new TableLayout.Column[4]
      {
        GridViewDataManager.NameWithIconColumn,
        GridViewDataManager.DateTimeColumn,
        GridViewDataManager.FileSizeColumn,
        GridViewDataManager.NameColumn
      };
      columnList[3].Title = "Document Name";
      this.gvFilesMgr.CreateLayout(columnList);
      if (!this.isHideFileSizeEnabled)
        return;
      this.gvFiles.Columns[2].Width = 0;
    }

    private void showDocumentFiles()
    {
      this.gvConditions.SelectedItems.Clear();
      this.lstDocuments.Items.Clear();
      List<DocumentLog> docList = new List<DocumentLog>();
      foreach (GVItem selectedItem in this.gvDocuments.SelectedItems)
      {
        docList.Add((DocumentLog) selectedItem.Tag);
        this.lstDocuments.Items.Add((object) selectedItem.Text);
      }
      this.docMgr.DocumentList = docList.ToArray();
      this.docMgr.ConditionDocuments = false;
      this.loadFiles(docList);
    }

    private void loadFiles(List<DocumentLog> docList)
    {
      this.gvFilesMgr.ClearItems();
      foreach (DocumentLog doc in docList)
      {
        FileAttachment[] attachments = this.loanDataMgr.FileAttachments.GetAttachments(doc);
        if (attachments.Length != 0)
        {
          foreach (FileAttachment file in attachments)
            this.gvFilesMgr.AddItem(file, (FileAttachmentReference) null).SubItems[3].Text = doc.Title;
        }
      }
      FileAttachment[] attachments1 = this.loanDataMgr.FileAttachments.GetAttachments(docList.ToArray());
      if (attachments1.Length != 0)
        this.fileViewer.LoadFiles(attachments1);
      else
        this.fileViewer.CloseFile();
    }

    private void showDocumentFiles(DocumentLog doc)
    {
      this.showDocumentFiles(new DocumentLog[1]{ doc });
    }

    private void showDocumentFiles(DocumentLog[] docList)
    {
      this.gvDocuments.SelectedItems.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocuments.Items)
      {
        if (Array.IndexOf<object>((object[]) docList, gvItem.Tag) >= 0)
          gvItem.Selected = true;
      }
      this.showDocumentFiles();
    }

    private void gvFiles_BeforeSelectedIndexCommitted(object sender, CancelEventArgs e)
    {
      if (this.fileViewer.CanCloseViewer())
        return;
      e.Cancel = true;
    }

    private void gvFiles_SelectedIndexCommitted(object sender, EventArgs e)
    {
      FileAttachment[] selectedFiles = this.getSelectedFiles();
      if (selectedFiles.Length != 0)
        this.fileViewer.LoadFiles(selectedFiles);
      else
        this.fileViewer.CloseFile();
    }

    private FileAttachment[] getSelectedFiles()
    {
      List<FileAttachment> fileAttachmentList = new List<FileAttachment>();
      foreach (GVItem selectedItem in this.gvFiles.SelectedItems)
        fileAttachmentList.Add((FileAttachment) selectedItem.Tag);
      return fileAttachmentList.ToArray();
    }

    public ConditionType ConditionType => this.condType;

    private void initConditionTypeList()
    {
      if (this.condType == ConditionType.Enhanced)
      {
        this.gcConditions.Text = "Condition Type";
        this.cboCondType.Left = Convert.ToInt32(Convert.ToDouble(this.cboCondType.Left) * 1.2);
        this.cboCondType.Width = Convert.ToInt32(Convert.ToDouble(this.cboCondType.Width) * 1.2);
        int num1 = 0;
        eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr);
        EnhancedConditionLog[] enhancedConditions = this.loanDataMgr.LoanData.GetLogList().GetAllEnhancedConditions();
        EnhancedConditionType[] enhancedConditionTypes = this.getEnhancedConditionTypes();
        if (enhancedConditionTypes != null)
        {
          foreach (EnhancedConditionType enhancedConditionType in enhancedConditionTypes)
          {
            int num2 = 0;
            if (folderAccessRights.CanAccessEnhancedCondition(enhancedConditionType.title))
            {
              foreach (EnhancedConditionLog enhancedConditionLog in enhancedConditions)
              {
                if (enhancedConditionLog.EnhancedConditionType == enhancedConditionType.title)
                  ++num2;
              }
              if (num2 > 0)
                this.cboCondType.Items.Add((object) new EnhancedConditionTypeComboItem()
                {
                  Text = (enhancedConditionType.title + " (" + (object) num2 + ")"),
                  Value = (object) enhancedConditionType
                });
              num1 += num2;
            }
          }
        }
        this.cboCondType.Items.Insert(0, (object) new EnhancedConditionTypeComboItem()
        {
          Text = ("<All Conditions> (" + (object) num1 + ")"),
          Value = (object) "<All Conditions>"
        });
        this.cboCondType.SelectedIndex = 0;
      }
      else
      {
        if (this.userRights.CanAccessPreliminaryTab)
          this.cboCondType.Items.Add((object) "Preliminary");
        if (this.userRights.CanAccessUnderwritingTab)
          this.cboCondType.Items.Add((object) "Underwriting");
        if (this.userRights.CanAccessPostClosingTab)
          this.cboCondType.Items.Add((object) "Post-Closing");
        if (this.userRights.CanAccessSellTab)
          this.cboCondType.Items.Add((object) "Delivery");
        switch (this.condType)
        {
          case ConditionType.Underwriting:
            this.cboCondType.SelectedItem = (object) "Underwriting";
            break;
          case ConditionType.PostClosing:
            this.cboCondType.SelectedItem = (object) "Post-Closing";
            break;
          case ConditionType.Preliminary:
            this.cboCondType.SelectedItem = (object) "Preliminary";
            break;
          case ConditionType.Sell:
            this.cboCondType.SelectedItem = (object) "Delivery";
            break;
        }
      }
    }

    private EnhancedConditionType[] getEnhancedConditionTypes()
    {
      return this.enhancedConditionTypes == null ? new EnhancedConditionsRestClient(this.loanDataMgr).GetEnhancedConditionTypes(true, false) : this.enhancedConditionTypes;
    }

    private EnhancedConditionTemplate[] getEnhancedConditionTemplates()
    {
      if (this.conditionTemplates != null)
        return this.conditionTemplates;
      this.conditionTemplates = new EnhancedConditionsRestClient(this.loanDataMgr).GetEnhancedConditionTemplates(true);
      return this.conditionTemplates;
    }

    private ConditionType getSelectedConditionType()
    {
      if (this.condType == ConditionType.Enhanced)
        return this.condType;
      string selectedItem = (string) this.cboCondType.SelectedItem;
      switch (this.cboCondType.Text)
      {
        case "Preliminary":
          return ConditionType.Preliminary;
        case "Underwriting":
          return ConditionType.Underwriting;
        case "Post-Closing":
          return ConditionType.PostClosing;
        case "Delivery":
          return ConditionType.Sell;
        default:
          return ConditionType.Unknown;
      }
    }

    private void cboCondType_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.gvConditions.SelectedItems.Clear();
      this.condType = this.getSelectedConditionType();
      this.loadConditionList();
      this.refreshCondition();
    }

    private void cboCondType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.fileViewer.CloseFile();
      this.condType = this.getSelectedConditionType();
      if (this.condType == ConditionType.Enhanced)
        this.showAddEnhancedCondition();
      else
        this.applyBasicConditionSecurity();
      this.showHideTabs();
    }

    private void showAddEnhancedCondition()
    {
      this.btnAddCondition.Visible = false;
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr);
      if (this.cboCondType.SelectedIndex == 0)
      {
        if (this.getEnhancedConditionTypes() == null)
          return;
        foreach (EnhancedConditionType enhancedConditionType in this.getEnhancedConditionTypes())
        {
          if (folderAccessRights.CanAddEnhancedCondition(enhancedConditionType.title))
          {
            this.btnAddCondition.Visible = true;
            break;
          }
        }
      }
      else
      {
        EnhancedConditionType enhancedConditionType = (EnhancedConditionType) ((EnhancedConditionTypeComboItem) this.cboCondType.SelectedItem).Value;
        if (!folderAccessRights.CanAddEnhancedCondition(enhancedConditionType.title))
          return;
        this.btnAddCondition.Visible = true;
      }
    }

    private void initConditionList()
    {
      this.gvConditionsMgr = new GridViewDataManager(this.gvConditions, this.loanDataMgr);
      this.gvConditionsMgr.CreateLayout(new TableLayout.Column[2]
      {
        GridViewDataManager.NameColumn,
        GridViewDataManager.BorrowerColumn
      });
      this.gvConditions.Columns[1].SpringToFit = true;
      this.gvConditions.Sort(0, SortOrder.Ascending);
    }

    private void loadConditionList()
    {
      LogList logList = this.loanDataMgr.LoanData.GetLogList();
      ConditionLog[] allConditions = logList.GetAllConditions(this.condType);
      DocumentLog[] allDocuments = logList.GetAllDocuments();
      if (allConditions == null)
        return;
      foreach (ConditionLog cond in allConditions)
      {
        if (cond.ConditionType == ConditionType.Enhanced && !this.cboCondType.SelectedItem.ToString().Contains("<All Conditions>"))
        {
          EnhancedConditionType enhancedConditionType = (EnhancedConditionType) ((EnhancedConditionTypeComboItem) this.cboCondType.SelectedItem).Value;
          if (((EnhancedConditionLog) cond).EnhancedConditionType != enhancedConditionType.ToString())
            continue;
        }
        GVItem gvItem1 = this.gvConditions.Items.GetItemByTag((object) cond);
        if (gvItem1 == null)
        {
          gvItem1 = this.gvConditionsMgr.AddItem(cond, allDocuments);
          gvItem1.ImageIndex = 2;
          gvItem1.State = GVItemState.Collapsed;
        }
        else
          this.gvConditionsMgr.RefreshItem(gvItem1, cond, allDocuments);
        foreach (DocumentLog doc in allDocuments)
        {
          GVItem itemByTag = gvItem1.GroupItems.GetItemByTag((object) doc);
          if (doc.Conditions.Contains(cond))
          {
            int num = 0;
            if (this.loanDataMgr.FileAttachments.ContainsAttachment(doc))
              num = 1;
            if (itemByTag == null)
            {
              GVItem gvItem2 = this.gvConditionsMgr.CreateItem(doc);
              gvItem2.ImageIndex = num;
              gvItem1.GroupItems.Add(gvItem2);
            }
            else
            {
              this.gvConditionsMgr.RefreshItem(itemByTag, doc);
              if (itemByTag.ImageIndex != num)
                itemByTag.ImageIndex = num;
            }
          }
          else if (itemByTag != null)
            gvItem1.GroupItems.Remove(itemByTag);
        }
      }
      List<GVItem> gvItemList = new List<GVItem>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvConditions.Items)
      {
        if (Array.IndexOf<object>((object[]) allConditions, gvItem.Tag) < 0)
          gvItemList.Add(gvItem);
      }
      foreach (GVItem gvItem in gvItemList)
        this.gvConditions.Items.Remove(gvItem);
      this.gvConditions.ReSort();
    }

    private void gvConditions_SelectedIndexCommitted(object sender, EventArgs e)
    {
      this.refreshCondition();
      this.showHideTabs();
    }

    private void refreshCondition(ConditionLog cond)
    {
      this.gvConditions.SelectedItems.Clear();
      GVItem itemByTag = this.gvConditions.Items.GetItemByTag((object) cond);
      if (itemByTag != null)
        itemByTag.Selected = true;
      this.refreshCondition();
      this.showHideTabs();
    }

    private void refreshCondition()
    {
      this.gvDocuments.SelectedItems.Clear();
      this.lstDocuments.Items.Clear();
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      if (this.gvConditions.SelectedItems.Count > 0)
      {
        foreach (GVItem selectedItem in this.gvConditions.SelectedItems)
        {
          if (selectedItem.Tag is ConditionLog)
          {
            foreach (GVItem groupItem in (IEnumerable<GVItem>) selectedItem.GroupItems)
            {
              documentLogList.Add((DocumentLog) groupItem.Tag);
              this.lstDocuments.Items.Add((object) groupItem.Text);
            }
          }
          if (selectedItem.Tag is DocumentLog)
          {
            documentLogList.Add((DocumentLog) selectedItem.Tag);
            this.lstDocuments.Items.Add((object) selectedItem.Text);
          }
        }
      }
      this.docMgr.DocumentList = documentLogList.ToArray();
      this.docMgr.ConditionDocuments = true;
      this.showConditionFiles();
    }

    private ConditionLog getSelectedCondition()
    {
      ConditionLog selectedCondition = (ConditionLog) null;
      if (this.gvConditions.SelectedItems.Count > 0)
      {
        GVItem selectedItem = this.gvConditions.SelectedItems[0];
        selectedCondition = !(selectedItem.Tag is ConditionLog) ? (ConditionLog) selectedItem.ParentItem.Tag : (ConditionLog) selectedItem.Tag;
      }
      return selectedCondition;
    }

    private void showConditionFiles()
    {
      List<DocumentLog> docList = new List<DocumentLog>();
      foreach (GVItem selectedItem in this.gvConditions.SelectedItems)
      {
        if (selectedItem.Tag is ConditionLog)
        {
          foreach (GVItem groupItem in (IEnumerable<GVItem>) selectedItem.GroupItems)
            docList.Add((DocumentLog) groupItem.Tag);
        }
        if (selectedItem.Tag is DocumentLog)
          docList.Add((DocumentLog) selectedItem.Tag);
      }
      this.loadFiles(docList);
      this.showConditionDetails();
    }

    private void showConditionFiles(ConditionLog cond)
    {
      this.gvConditions.SelectedItems.Clear();
      GVItem itemByTag = this.gvConditions.Items.GetItemByTag((object) cond);
      if (itemByTag != null)
        itemByTag.Selected = true;
      this.showConditionFiles();
    }

    private void initDocumentBorrowerField()
    {
      this.cboDocumentBorrower.Items.AddRange((object[]) this.loanDataMgr.LoanData.GetBorrowerPairs());
      this.cboDocumentBorrower.Items.Add((object) BorrowerPair.All);
    }

    private void loadDocumentBorrowerField()
    {
      this.cboDocumentBorrower.SelectedItem = (object) null;
      this.txtDocumentBorrower.Text = string.Empty;
      if (!(this.docMgr.DocsPairId != "NOMATCH"))
        return;
      foreach (BorrowerPair borrowerPair in this.cboDocumentBorrower.Items)
      {
        if (borrowerPair.Id == this.docMgr.DocsPairId)
          this.cboDocumentBorrower.SelectedItem = (object) borrowerPair;
      }
      this.txtDocumentBorrower.Text = this.cboDocumentBorrower.Text;
    }

    private void cboDocumentBorrower_SelectionChangeCommitted(object sender, EventArgs e)
    {
      BorrowerPair selectedItem = (BorrowerPair) this.cboDocumentBorrower.SelectedItem;
      if (selectedItem == null)
        return;
      this.docMgr.DocsPairId = selectedItem.Id;
    }

    private void initMilestoneField()
    {
      MilestoneTemplatesBpmManager bpmManager = (MilestoneTemplatesBpmManager) Session.BPM.GetBpmManager(BpmCategory.Milestones);
      foreach (MilestoneLog allMilestone in this.loanDataMgr.LoanData.GetLogList().GetAllMilestones())
      {
        if (!(allMilestone.Stage == "Started"))
        {
          EllieMae.EMLite.Workflow.Milestone milestoneById = bpmManager.GetMilestoneByID(allMilestone.MilestoneID, allMilestone.Stage, false, allMilestone.Days, allMilestone.DoneText, allMilestone.ExpText, allMilestone.RoleRequired == "Y", allMilestone.RoleID);
          if (milestoneById != null)
          {
            MilestoneLabel milestoneLabel = new MilestoneLabel(milestoneById);
            milestoneLabel.Tag = (object) milestoneById;
            this.cboMilestone.Items.Add((object) milestoneLabel);
          }
        }
      }
    }

    private void loadMilestoneField()
    {
      this.cboMilestone.SelectedItem = (object) null;
      this.txtMilestone.Text = string.Empty;
      if (string.IsNullOrEmpty(this.docMgr.DocsStage))
        return;
      MilestoneTemplatesBpmManager bpmManager = (MilestoneTemplatesBpmManager) Session.BPM.GetBpmManager(BpmCategory.Milestones);
      LogList logList = this.loanDataMgr.LoanData.GetLogList();
      MilestoneLog milestoneByName = logList.GetMilestoneByName(this.docMgr.DocsStage);
      EllieMae.EMLite.Workflow.Milestone milestone = (EllieMae.EMLite.Workflow.Milestone) null;
      if (milestoneByName != null)
        milestone = bpmManager.GetMilestoneByID(milestoneByName.MilestoneID, milestoneByName.Stage, false, milestoneByName.Days, milestoneByName.DoneText, milestoneByName.ExpText, milestoneByName.RoleRequired == "Y", milestoneByName.RoleID);
      if (milestone == null)
      {
        milestone = bpmManager.GetMilestoneByName(this.docMgr.DocsStage);
        if (milestone == null || logList.GetMilestoneByID(milestone.MilestoneID) == null)
          return;
      }
      this.cboMilestone.SelectedItem = (object) null;
      foreach (MilestoneLabel milestoneLabel in this.cboMilestone.Items)
      {
        if (milestoneLabel.MilestoneName == milestone.Name)
          this.cboMilestone.SelectedItem = (object) milestoneLabel;
      }
      this.txtMilestone.Text = this.cboMilestone.Text;
    }

    private void cboMilestone_SelectionChangeCommitted(object sender, EventArgs e)
    {
      MilestoneTemplatesBpmManager bpmManager = (MilestoneTemplatesBpmManager) Session.BPM.GetBpmManager(BpmCategory.Milestones);
      MilestoneLabel selectedItem = (MilestoneLabel) this.cboMilestone.SelectedItem;
      if (selectedItem == null)
        return;
      EllieMae.EMLite.Workflow.Milestone tag = (EllieMae.EMLite.Workflow.Milestone) selectedItem.Tag;
      if (tag == null)
        return;
      MilestoneLog milestoneById = this.loanDataMgr.LoanData.GetLogList().GetMilestoneByID(tag.MilestoneID);
      if (milestoneById == null)
        return;
      this.docMgr.DocsStage = milestoneById.Stage;
    }

    private void btnAccess_Click(object sender, EventArgs e)
    {
      using (DocumentAccessDialog documentAccessDialog = new DocumentAccessDialog(this.loanDataMgr, this.docMgr.DocumentList))
      {
        int num = (int) documentAccessDialog.ShowDialog((IWin32Window) this);
      }
    }

    private void loadDocumentConditionsField()
    {
      string str1 = string.Empty;
      ArrayList arrayList = new ArrayList();
      this.lstDocumentConditions.Items.Clear();
      if (this.docMgr.DocumentList == null || this.docMgr.DocumentList.Length == 0)
        return;
      DocumentLog document1 = this.docMgr.DocumentList[0];
      if (document1 != null)
      {
        str1 = document1.Title;
        foreach (ConditionLog condition in document1.Conditions)
          arrayList.Add((object) condition.Title);
      }
      foreach (string str2 in arrayList)
      {
        bool flag1 = true;
        foreach (DocumentLog document2 in this.docMgr.DocumentList)
        {
          if (flag1 && document2.Title != str1)
          {
            bool flag2 = false;
            foreach (ConditionLog condition in document2.Conditions)
            {
              if (!flag2 && condition.Title == str2)
                flag2 = true;
            }
            if (!flag2)
              flag1 = false;
          }
        }
        if (flag1)
          this.lstDocumentConditions.Items.Add((object) str2);
      }
    }

    private void loadATRQMField()
    {
      bool flag1 = true;
      bool flag2 = true;
      bool flag3 = true;
      bool flag4 = true;
      this.lstATRQM.Items.Clear();
      foreach (DocumentLog document in this.docMgr.DocumentList)
      {
        if (!document.IsEmploymentVerification)
          flag1 = false;
        if (!document.IsObligationVerification)
          flag2 = false;
        if (!document.IsIncomeVerification)
          flag3 = false;
        if (!document.IsAssetVerification)
          flag4 = false;
      }
      if (flag1)
        this.lstATRQM.Items.Add((object) "Employment Status Verification");
      if (flag2)
        this.lstATRQM.Items.Add((object) "Monthly Obligation Verification");
      if (flag3)
        this.lstATRQM.Items.Add((object) "Income Verification");
      if (!flag4)
        return;
      this.lstATRQM.Items.Add((object) "Asset Verification");
    }

    private void initGroupsField()
    {
      this.groupSetup = this.loanDataMgr.SystemConfiguration.DocumentGroupSetup;
      this.docSetup = this.loanDataMgr.SystemConfiguration.DocumentTrackingSetup;
    }

    private void loadGroupsField()
    {
      this.lstGroups.Items.Clear();
      if (this.docMgr.DocumentList == null)
        return;
      foreach (DocumentGroup documentGroup in (CollectionBase) this.groupSetup)
      {
        bool flag = true;
        foreach (DocumentLog document in this.docMgr.DocumentList)
        {
          if (flag)
          {
            DocumentTemplate byName = this.docSetup.GetByName(document.Title);
            if (byName != null && !documentGroup.Contains(byName))
              flag = false;
          }
        }
        if (flag)
          this.lstGroups.Items.Add((object) documentGroup);
      }
    }

    private void chkWebCenter_Click(object sender, EventArgs e)
    {
      this.docMgr.WebCenterIntValue = Convert.ToInt32(this.chkWebCenter.Checked);
    }

    private void chkTPOWebcenterPortal_Click(object sender, EventArgs e)
    {
      this.docMgr.TPOWebCenterPortalIntValue = Convert.ToInt32(this.chkTPOWebcenterPortal.Checked);
    }

    private void chkThirdParty_Click(object sender, EventArgs e)
    {
      this.docMgr.ThirdPartyIntValue = Convert.ToInt32(this.chkThirdParty.Checked);
    }

    private void txtDocumentDaysDue_Validating(object sender, CancelEventArgs e)
    {
      if (!(this.txtDocumentDaysDue.Text != string.Empty) || Utils.IsInt((object) this.txtDocumentDaysDue.Text))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "You must specify a valid number for the Days to Receive.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      e.Cancel = true;
    }

    private void txtDocumentDaysDue_Validated(object sender, EventArgs e)
    {
      this.docMgr.DaysDue = Utils.ParseInt((object) this.txtDocumentDaysDue.Text, 0);
    }

    private void txtDaysExpire_Validating(object sender, CancelEventArgs e)
    {
      if (!(this.txtDaysExpire.Text != string.Empty) || Utils.IsInt((object) this.txtDaysExpire.Text))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "You must specify a valid number for the Days to Expire.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      e.Cancel = true;
    }

    private void txtDaysExpire_Validated(object sender, EventArgs e)
    {
      this.docMgr.DaysExpire = Utils.ParseInt((object) this.txtDaysExpire.Text, 0);
    }

    private void txtDocumentRequestedFrom_Validated(object sender, EventArgs e)
    {
      this.docMgr.RequestedFrom = this.txtDocumentRequestedFrom.Text;
    }

    private void loadDocumentRequestedFields()
    {
      this.chkDocumentRequested.CheckState = CheckState.Unchecked;
      if (this.docMgr.RequestedIntValue == -1)
        this.chkDocumentRequested.CheckState = CheckState.Indeterminate;
      else
        this.chkDocumentRequested.Checked = Convert.ToBoolean(this.docMgr.RequestedIntValue);
      if (this.docMgr.RequestedIntValue == 1)
      {
        this.txtDocumentDateRequested.Text = this.docMgr.DateRequestedString;
        this.txtDocumentRequestedBy.Text = this.docMgr.RequestedBy;
      }
      this.txtDocumentDateRequested.Visible = this.docMgr.RequestedIntValue == 1;
      this.btnDocumentDateRequested.Visible = this.docMgr.RequestedIntValue == 1 && this.docMgr.CanEditDocuments;
      this.txtDocumentRequestedBy.Visible = this.docMgr.RequestedIntValue == 1;
      this.btnDocumentRequestedBy.Visible = this.docMgr.RequestedIntValue == 1 && this.docMgr.CanEditDocuments;
    }

    private void chkDocumentRequested_Click(object sender, EventArgs e)
    {
      this.docMgr.Requested = this.chkDocumentRequested.Checked;
    }

    private void txtDocumentDateRequested_TextChanged(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.txtDocumentDateRequested.Text))
        return;
      this.docMgr.DateRequestedString = this.txtDocumentDateRequested.Text;
    }

    private void btnDocumentRequestedBy_Click(object sender, EventArgs e)
    {
      string str = this.selectUser();
      if (str == null)
        return;
      this.docMgr.RequestedBy = str;
    }

    private void loadDocumentRerequestedFields()
    {
      this.chkDocumentRerequested.CheckState = CheckState.Unchecked;
      if (this.docMgr.RerequestedIntValue == -1)
        this.chkDocumentRerequested.CheckState = CheckState.Indeterminate;
      else
        this.chkDocumentRerequested.Checked = Convert.ToBoolean(this.docMgr.RerequestedIntValue);
      if (this.docMgr.RerequestedIntValue == 1)
      {
        this.txtDocumentDateRerequested.Text = this.docMgr.DateRerequestedString;
        this.txtDocumentRerequestedBy.Text = this.docMgr.RerequestedBy;
      }
      this.txtDocumentDateRerequested.Visible = this.docMgr.RerequestedIntValue == 1;
      this.btnDocumentDateRerequested.Visible = this.docMgr.RerequestedIntValue == 1 && this.docMgr.CanEditDocuments;
      this.txtDocumentRerequestedBy.Visible = this.docMgr.RerequestedIntValue == 1;
      this.btnDocumentRerequestedBy.Visible = this.docMgr.RerequestedIntValue == 1 && this.docMgr.CanEditDocuments;
    }

    private void chkDocumentRerequested_Click(object sender, EventArgs e)
    {
      this.docMgr.Rerequested = this.chkDocumentRerequested.Checked;
    }

    private void txtDocumentDateRerequested_TextChanged(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.txtDocumentDateRerequested.Text))
        return;
      this.docMgr.DateRerequestedString = this.txtDocumentDateRerequested.Text;
    }

    private void btnDocumentRerequestedBy_Click(object sender, EventArgs e)
    {
      string str = this.selectUser();
      if (str == null)
        return;
      this.docMgr.RerequestedBy = str;
    }

    private void loadDocumentReceivedFields()
    {
      this.chkDocumentReceived.CheckState = CheckState.Unchecked;
      if (this.docMgr.ReceivedIntValue == -1)
        this.chkDocumentReceived.CheckState = CheckState.Indeterminate;
      else
        this.chkDocumentReceived.Checked = Convert.ToBoolean(this.docMgr.ReceivedIntValue);
      if (this.docMgr.ReceivedIntValue == 1)
      {
        this.txtDocumentDateReceived.Text = this.docMgr.DateReceivedString;
        this.txtDocumentReceivedBy.Text = this.docMgr.ReceivedBy;
      }
      this.txtDocumentDateReceived.Visible = this.docMgr.ReceivedIntValue == 1;
      this.btnDocumentDateReceived.Visible = this.docMgr.ReceivedIntValue == 1 && this.docMgr.CanEditDocuments;
      this.txtDocumentReceivedBy.Visible = this.docMgr.ReceivedIntValue == 1;
      this.btnDocumentReceivedBy.Visible = this.docMgr.ReceivedIntValue == 1 && this.docMgr.CanEditDocuments;
    }

    private void chkDocumentReceived_Click(object sender, EventArgs e)
    {
      this.docMgr.Received = this.chkDocumentReceived.Checked;
    }

    private void txtDocumentDateReceived_TextChanged(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.txtDocumentDateReceived.Text))
        return;
      this.docMgr.DateReceivedString = this.txtDocumentDateReceived.Text;
    }

    private void btnDocumentReceivedBy_Click(object sender, EventArgs e)
    {
      string str = this.selectUser();
      if (str == null)
        return;
      this.docMgr.ReceivedBy = str;
    }

    private void loadDocumentReviewedFields()
    {
      this.chkDocumentReviewed.CheckState = CheckState.Unchecked;
      if (this.docMgr.ReviewedIntValue == -1)
        this.chkDocumentReviewed.CheckState = CheckState.Indeterminate;
      else
        this.chkDocumentReviewed.Checked = Convert.ToBoolean(this.docMgr.ReviewedIntValue);
      if (this.docMgr.ReviewedIntValue == 1)
      {
        this.txtDocumentDateReviewed.Text = this.docMgr.DateReviewedString;
        this.txtDocumentReviewedBy.Text = this.docMgr.ReviewedBy;
      }
      this.txtDocumentDateReviewed.Visible = this.docMgr.ReviewedIntValue == 1;
      this.btnDocumentDateReviewed.Visible = this.docMgr.ReviewedIntValue == 1 && this.docMgr.CanEditDocuments;
      this.txtDocumentReviewedBy.Visible = this.docMgr.ReviewedIntValue == 1;
      this.btnDocumentReviewedBy.Visible = this.docMgr.ReviewedIntValue == 1 && this.docMgr.CanEditDocuments;
    }

    private void chkDocumentReviewed_Click(object sender, EventArgs e)
    {
      this.docMgr.Reviewed = this.chkDocumentReviewed.Checked;
    }

    private void txtDocumentDateReviewed_TextChanged(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.txtDocumentDateReviewed.Text))
        return;
      this.docMgr.DateReviewedString = this.txtDocumentDateReviewed.Text;
    }

    private void btnDocumentReviewedBy_Click(object sender, EventArgs e)
    {
      string str = this.selectUser();
      if (str == null)
        return;
      this.docMgr.ReviewedBy = str;
    }

    private void loadUnderwritingReadyFields()
    {
      this.chkUnderwritingReady.CheckState = CheckState.Unchecked;
      if (this.docMgr.UnderwritingReadyIntValue == -1)
        this.chkUnderwritingReady.CheckState = CheckState.Indeterminate;
      else
        this.chkUnderwritingReady.Checked = Convert.ToBoolean(this.docMgr.UnderwritingReadyIntValue);
      if (this.docMgr.UnderwritingReadyIntValue == 1)
      {
        this.txtDateUnderwritingReady.Text = this.docMgr.DateUnderwritingReadyString;
        this.txtUnderwritingReadyBy.Text = this.docMgr.UnderwritingReadyBy;
      }
      this.txtDateUnderwritingReady.Visible = this.docMgr.UnderwritingReadyIntValue == 1;
      this.btnDateUnderwritingReady.Visible = this.docMgr.UnderwritingReadyIntValue == 1 && this.docMgr.CanEditDocuments;
      this.txtUnderwritingReadyBy.Visible = this.docMgr.UnderwritingReadyIntValue == 1;
      this.btnUnderwritingReadyBy.Visible = this.docMgr.UnderwritingReadyIntValue == 1 && this.docMgr.CanEditDocuments;
    }

    private void chkUnderwritingReady_Click(object sender, EventArgs e)
    {
      this.docMgr.UnderwritingReady = this.chkUnderwritingReady.Checked;
    }

    private void txtDateUnderwritingReady_TextChanged(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.txtDateUnderwritingReady.Text))
        return;
      this.docMgr.DateUnderwritingReadyString = this.txtDateUnderwritingReady.Text;
    }

    private void btnUnderwritingReadyBy_Click(object sender, EventArgs e)
    {
      string str = this.selectUser();
      if (str == null)
        return;
      this.docMgr.UnderwritingReadyBy = str;
    }

    private void loadShippingReadyFields()
    {
      this.chkShippingReady.CheckState = CheckState.Unchecked;
      if (this.docMgr.ShippingReadyIntValue == -1)
        this.chkShippingReady.CheckState = CheckState.Indeterminate;
      else
        this.chkShippingReady.Checked = Convert.ToBoolean(this.docMgr.ShippingReadyIntValue);
      if (this.docMgr.ShippingReadyIntValue == 1)
      {
        this.txtDateShippingReady.Text = this.docMgr.DateShippingReadyString;
        this.txtShippingReadyBy.Text = this.docMgr.ShippingReadyBy;
      }
      this.txtDateShippingReady.Visible = this.docMgr.ShippingReadyIntValue == 1;
      this.btnDateShippingReady.Visible = this.docMgr.ShippingReadyIntValue == 1 && this.docMgr.CanEditDocuments;
      this.txtShippingReadyBy.Visible = this.docMgr.ShippingReadyIntValue == 1;
      this.btnShippingReadyBy.Visible = this.docMgr.ShippingReadyIntValue == 1 && this.docMgr.CanEditDocuments;
    }

    private void chkShippingReady_Click(object sender, EventArgs e)
    {
      this.docMgr.ShippingReady = this.chkShippingReady.Checked;
    }

    private void txtDateShippingReady_TextChanged(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.txtDateShippingReady.Text))
        return;
      this.docMgr.DateShippingReadyString = this.txtDateShippingReady.Text;
    }

    private void btnShippingReadyBy_Click(object sender, EventArgs e)
    {
      string str = this.selectUser();
      if (str == null)
        return;
      this.docMgr.ShippingReadyBy = str;
    }

    private void initDocumentCommentList()
    {
      this.gvDocumentCommentsMgr = new GridViewDataManager(this.gvDocumentComments, this.loanDataMgr);
      this.gvDocumentCommentsMgr.CreateLayout(new TableLayout.Column[4]
      {
        GridViewDataManager.NameColumn,
        GridViewDataManager.DateColumn,
        GridViewDataManager.UserColumn,
        GridViewDataManager.CommentColumn
      });
      this.gvDocumentComments.Columns[3].Width = this.gvDocumentComments.Columns[0].Width;
      this.gvDocumentComments.Sort(0, SortOrder.Ascending);
    }

    private void loadDocumentCommentList()
    {
      this.gvDocumentComments.Items.Clear();
      this.txtDocumentComment.Text = string.Empty;
      if (this.docMgr.DocumentList == null)
        return;
      foreach (DocumentLog document in this.docMgr.DocumentList)
      {
        foreach (CommentEntry comment in (CollectionBase) document.Comments)
          this.gvDocumentCommentsMgr.AddItem(comment, document.Title);
      }
      this.gvDocumentComments.ReSort();
    }

    private void gvDocumentComments_SelectedIndexCommitted(object sender, EventArgs e)
    {
      this.txtDocumentComment.Text = string.Empty;
      if (this.gvDocumentComments.SelectedItems.Count <= 0)
        return;
      this.txtDocumentComment.Text = ((CommentEntry) this.gvDocumentComments.SelectedItems[0].Tag).Comments;
    }

    private void btnAddDocumentComment_Click(object sender, EventArgs e)
    {
      using (CommentEntryDialog commentEntryDialog = new CommentEntryDialog(this.loanDataMgr, (CommentEntry) null, false))
      {
        if (commentEntryDialog.ShowDialog((IWin32Window) Form.ActiveForm) == DialogResult.Cancel)
          return;
        this.docMgr.AddComment(commentEntryDialog.Entry);
        this.loadDocumentCommentList();
      }
    }

    private void btnViewDocumentComment_Click(object sender, EventArgs e)
    {
      if (this.gvDocumentComments.SelectedItems.Count <= 0)
        return;
      using (CommentEntryDialog commentEntryDialog = new CommentEntryDialog(this.loanDataMgr, (CommentEntry) this.gvDocumentComments.SelectedItems[0].Tag, true))
      {
        int num = (int) commentEntryDialog.ShowDialog((IWin32Window) Form.ActiveForm);
      }
    }

    private void btnDeleteDocumentComment_Click(object sender, EventArgs e)
    {
      if (this.gvDocumentComments.SelectedItems.Count <= 0)
        return;
      CommentEntry tag = (CommentEntry) this.gvDocumentComments.SelectedItems[0].Tag;
      if (tag == null || Utils.Dialog((IWin32Window) Form.ActiveForm, "Are you sure that you want to delete the following comment:\r\n\r\n" + tag.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return;
      GVItem selectedItem = this.gvDocumentComments.SelectedItems[0];
      DocumentLog documentLog = (DocumentLog) null;
      foreach (DocumentLog document in this.docMgr.DocumentList)
      {
        if (document.Title == selectedItem.Text)
        {
          documentLog = document;
          break;
        }
      }
      if (documentLog == null)
        return;
      documentLog.Comments.Remove(tag);
      documentLog.MarkLastUpdated();
    }

    private void initConditionCommentList()
    {
      this.gvConditionCommentsMgr = new GridViewDataManager(this.gvConditionComments, this.loanDataMgr);
      this.gvConditionCommentsMgr.CreateLayout(new TableLayout.Column[4]
      {
        GridViewDataManager.NameColumn,
        GridViewDataManager.DateColumn,
        GridViewDataManager.UserColumn,
        GridViewDataManager.CommentColumn
      });
      this.gvConditionComments.Columns[3].Width = this.gvConditionComments.Columns[0].Width;
      this.gvConditionComments.Sort(0, SortOrder.Ascending);
    }

    private void loadConditionCommentList(ConditionLog cond)
    {
      this.gvConditionComments.Items.Clear();
      this.txtConditionComment.Text = string.Empty;
      foreach (CommentEntry comment in (CollectionBase) cond.Comments)
        this.gvConditionCommentsMgr.AddItem(comment, cond.Title);
      this.gvConditionComments.ReSort();
    }

    private void gvConditionComments_SelectedIndexCommitted(object sender, EventArgs e)
    {
      this.txtConditionComment.Text = string.Empty;
      if (this.gvConditionComments.SelectedItems.Count <= 0)
        return;
      this.txtConditionComment.Text = ((CommentEntry) this.gvConditionComments.SelectedItems[0].Tag).Comments;
    }

    private void btnAddConditionComment_Click(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition == null)
        return;
      using (CommentEntryDialog commentEntryDialog = new CommentEntryDialog(this.loanDataMgr, (CommentEntry) null, false, this.condType == ConditionType.Sell))
      {
        if (commentEntryDialog.ShowDialog((IWin32Window) Form.ActiveForm) == DialogResult.Cancel)
          return;
        selectedCondition.Comments.Add(commentEntryDialog.Entry);
        this.loadConditionCommentList(selectedCondition);
      }
    }

    private void btnViewConditionComment_Click(object sender, EventArgs e)
    {
      if (this.gvConditionComments.SelectedItems.Count <= 0)
        return;
      using (CommentEntryDialog commentEntryDialog = new CommentEntryDialog(this.loanDataMgr, (CommentEntry) this.gvConditionComments.SelectedItems[0].Tag, true, this.condType == ConditionType.Sell))
      {
        int num = (int) commentEntryDialog.ShowDialog((IWin32Window) Form.ActiveForm);
      }
    }

    private void btnDeleteConditionComment_Click(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition == null || this.gvConditionComments.SelectedItems.Count <= 0)
        return;
      CommentEntry tag = (CommentEntry) this.gvConditionComments.SelectedItems[0].Tag;
      if (tag == null || Utils.Dialog((IWin32Window) Form.ActiveForm, "Are you sure that you want to delete the following comment:\r\n\r\n" + tag.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return;
      selectedCondition.Comments.Remove(tag);
    }

    private void showConditionDetails()
    {
      if (this.condType == ConditionType.Enhanced)
      {
        this.showEnhancedConditionDetails();
      }
      else
      {
        this.clearConditionDetails();
        if (this.getSelectedCondition() is StandardConditionLog selectedCondition)
        {
          eFolderAccessRights condRights = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) selectedCondition);
          this.applyConditionLevelSecurity((ConditionLog) selectedCondition, condRights);
          this.lstConditions.Items.Add((object) selectedCondition.Title);
          this.loadConditionBorrowerField(selectedCondition.PairId);
          if (selectedCondition is PostClosingConditionLog)
          {
            this.loadSourceField((ConditionLog) selectedCondition);
            this.loadRecipientField((PostClosingConditionLog) selectedCondition);
          }
          else
            this.txtSource.Text = selectedCondition.Source;
          this.loadCategoryField((ConditionLog) selectedCondition);
          this.loadPriorToField((ConditionLog) selectedCondition);
          if (selectedCondition is PreliminaryConditionLog)
            this.loadUnderwriterAccessField((PreliminaryConditionLog) selectedCondition);
          this.loadConditionExpectedFields(selectedCondition);
          this.loadConditionRequestedFromField(selectedCondition.RequestedFrom);
          this.txtAddedDate.Text = selectedCondition.DateAdded.ToString("MM/dd/yy hh:mm tt");
          this.txtAddedBy.Text = selectedCondition.AddedBy;
          this.loadFulfilledFields((ConditionLog) selectedCondition, condRights);
          this.loadConditionRequestedFields(selectedCondition);
          this.loadConditionRerequestedFields(selectedCondition);
          this.loadConditionReceivedFields(selectedCondition, condRights);
          if (selectedCondition is UnderwritingConditionLog)
          {
            UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) selectedCondition;
            this.loadOwnerField((ConditionLog) underwritingConditionLog);
            this.chkAllowToClear.Checked = underwritingConditionLog.AllowToClear;
            this.chkInternal.Checked = underwritingConditionLog.IsInternal;
            this.chkExternal.Checked = underwritingConditionLog.IsExternal;
            this.loadConditionReviewedFields((ConditionLog) underwritingConditionLog, condRights);
            this.loadRejectedFields((ConditionLog) underwritingConditionLog, condRights);
            this.loadWaivedFields((ConditionLog) underwritingConditionLog, condRights);
          }
          if (selectedCondition is SellConditionLog)
          {
            SellConditionLog sellConditionLog = (SellConditionLog) selectedCondition;
            this.loadOwnerField((ConditionLog) sellConditionLog);
            this.chkAllowToClear.Checked = sellConditionLog.AllowToClear;
            this.chkInternal.Checked = sellConditionLog.IsInternal;
            this.chkExternal.Checked = sellConditionLog.IsExternal;
            this.loadConditionReviewedFields((ConditionLog) sellConditionLog, condRights);
            this.loadRejectedFields((ConditionLog) sellConditionLog, condRights);
            this.loadWaivedFields((ConditionLog) sellConditionLog, condRights);
          }
          if (selectedCondition is PostClosingConditionLog)
            this.loadSentFields((PostClosingConditionLog) selectedCondition);
          if (!(selectedCondition is PreliminaryConditionLog))
            this.loadClearedFields((ConditionLog) selectedCondition, condRights);
          this.loadConditionCommentList((ConditionLog) selectedCondition);
        }
        this.refreshDocumentDetailsTab();
      }
    }

    private void clearConditionDetails()
    {
      this.lstConditions.Items.Clear();
      this.cboConditionBorrower.SelectedItem = (object) null;
      this.txtConditionBorrower.Text = string.Empty;
      this.cboSource.SelectedItem = (object) null;
      this.txtSource.Text = string.Empty;
      this.cboRecipient.SelectedItem = (object) null;
      this.txtRecipient.Text = string.Empty;
      this.cboCategory.SelectedItem = (object) null;
      this.txtCategory.Text = string.Empty;
      this.cboPriorTo.SelectedItem = (object) null;
      this.txtPriorTo.Text = string.Empty;
      this.chkUnderwriter.Checked = false;
      this.txtConditionDaysDue.Text = string.Empty;
      this.txtConditionDateDue.Text = string.Empty;
      this.txtConditionRequestedFrom.Text = string.Empty;
      this.txtAddedDate.Text = string.Empty;
      this.txtAddedBy.Text = string.Empty;
      this.chkFulfilled.Checked = false;
      this.dateFulfilled.Visible = false;
      this.txtDateFulfilled.Visible = false;
      this.txtFulfilledBy.Visible = false;
      this.btnFulfilledBy.Visible = false;
      this.chkConditionRequested.Checked = false;
      this.dateConditionRequested.Visible = false;
      this.txtConditionDateRequested.Visible = false;
      this.txtConditionRequestedBy.Visible = false;
      this.btnConditionRequestedBy.Visible = false;
      this.chkConditionRerequested.Checked = false;
      this.dateConditionRerequested.Visible = false;
      this.txtConditionDateRerequested.Visible = false;
      this.txtConditionRerequestedBy.Visible = false;
      this.btnConditionRerequestedBy.Visible = false;
      this.chkConditionReceived.Checked = false;
      this.dateConditionReceived.Visible = false;
      this.txtConditionDateReceived.Visible = false;
      this.txtConditionReceivedBy.Visible = false;
      this.btnConditionReceivedBy.Visible = false;
      this.cboOwner.SelectedItem = (object) null;
      this.txtOwner.Text = string.Empty;
      this.chkAllowToClear.Checked = false;
      this.chkInternal.Checked = false;
      this.chkExternal.Checked = false;
      this.chkConditionReviewed.Checked = false;
      this.dateConditionReviewed.Visible = false;
      this.txtConditionDateReviewed.Visible = false;
      this.txtConditionReviewedBy.Visible = false;
      this.btnConditionReviewedBy.Visible = false;
      this.chkRejected.Checked = false;
      this.dateRejected.Visible = false;
      this.txtDateRejected.Visible = false;
      this.txtRejectedBy.Visible = false;
      this.btnRejectedBy.Visible = false;
      this.chkWaived.Checked = false;
      this.dateWaived.Visible = false;
      this.txtDateWaived.Visible = false;
      this.txtWaivedBy.Visible = false;
      this.btnWaivedBy.Visible = false;
      this.chkSent.Checked = false;
      this.dateSent.Visible = false;
      this.txtDateSent.Visible = false;
      this.txtSentBy.Visible = false;
      this.btnSentBy.Visible = false;
      this.chkCleared.Checked = false;
      this.dateCleared.Visible = false;
      this.txtDateCleared.Visible = false;
      this.txtClearedBy.Visible = false;
      this.btnClearedBy.Visible = false;
      this.gvConditionComments.Items.Clear();
      this.txtConditionComment.Text = string.Empty;
    }

    private void showEnhancedConditionDetails()
    {
      this.clearEnhancedConditionDetails();
      if (!(this.getSelectedCondition() is EnhancedConditionLog selectedCondition))
        return;
      eFolderAccessRights rights = new eFolderAccessRights(this.loanDataMgr);
      this.applyEnhancedConditionSecurity(selectedCondition, rights);
      this.lstConditionsEnhanced.Items.Add((object) selectedCondition.Title);
      this.loadConditionBorrowerEnhancedField(selectedCondition.PairId);
      this.loadConditionTypeField(selectedCondition.EnhancedConditionType);
      this.initSourceEnhancedField(selectedCondition);
      this.loadSourceEnhancedField(selectedCondition);
      this.initPriorToEnhancedField(selectedCondition);
      this.loadPriorToEnhancedField(selectedCondition);
      this.loadDaysToReceiveEnhancedField(selectedCondition);
      this.loadDateDueEnhancedField(selectedCondition);
      this.loadRequestedFromEnhancedField(selectedCondition);
      this.loadTrackingList(selectedCondition);
      this.loadCommentListEnhanced(selectedCondition);
    }

    private void clearEnhancedConditionDetails()
    {
      this.lstConditionsEnhanced.Items.Clear();
      this.txtConditionType.Text = string.Empty;
      this.cboConditionBorrowerEnhanced.SelectedItem = (object) null;
      this.cboSourceEnhanced.SelectedItem = (object) null;
      this.cboPriorToEnhanced.SelectedItem = (object) null;
      this.txtDaysToReceiveEnhanced.Text = string.Empty;
      this.txtDateDueEnhanced.Text = string.Empty;
      this.txtRequestedFromEnhanced.Text = string.Empty;
      this.gvTrackingMgr.ClearItems();
      this.txtCommentEnhanced.Text = string.Empty;
    }

    private void btnAddCondition_Click(object sender, EventArgs e)
    {
      switch (this.condType)
      {
        case ConditionType.Underwriting:
          this.addUnderwritingCondition();
          break;
        case ConditionType.PostClosing:
          this.addPostClosingCondition();
          break;
        case ConditionType.Preliminary:
          this.addPreliminaryCondition();
          break;
        case ConditionType.Sell:
          this.addSellCondition();
          break;
        case ConditionType.Enhanced:
          this.addEnhancedCondition();
          break;
      }
    }

    private void addPreliminaryCondition()
    {
      using (AddPreliminaryDialog preliminaryDialog = new AddPreliminaryDialog(this.loanDataMgr))
      {
        if (preliminaryDialog.ShowDialog((IWin32Window) this) != DialogResult.OK || preliminaryDialog.Conditions == null || preliminaryDialog.Conditions.Length == 0)
          return;
        this.refreshCondition(preliminaryDialog.Conditions[0]);
      }
    }

    private void addUnderwritingCondition()
    {
      using (AddUnderwritingDialog underwritingDialog = new AddUnderwritingDialog(this.loanDataMgr))
      {
        if (underwritingDialog.ShowDialog((IWin32Window) this) != DialogResult.OK || underwritingDialog.Conditions == null || underwritingDialog.Conditions.Length == 0)
          return;
        this.refreshCondition(underwritingDialog.Conditions[0]);
      }
    }

    private void addPostClosingCondition()
    {
      using (AddPostClosingDialog postClosingDialog = new AddPostClosingDialog(this.loanDataMgr))
      {
        if (postClosingDialog.ShowDialog((IWin32Window) this) != DialogResult.OK || postClosingDialog.Conditions == null || postClosingDialog.Conditions.Length == 0)
          return;
        this.refreshCondition(postClosingDialog.Conditions[0]);
      }
    }

    private void addSellCondition()
    {
      using (AddSellDialog addSellDialog = new AddSellDialog(this.loanDataMgr))
      {
        if (addSellDialog.ShowDialog((IWin32Window) this) != DialogResult.OK || addSellDialog.Conditions == null || addSellDialog.Conditions.Length == 0)
          return;
        this.refreshCondition(addSellDialog.Conditions[0]);
      }
    }

    private void addEnhancedCondition()
    {
      List<EnhancedConditionType> enhancedConditionTypeList = new List<EnhancedConditionType>();
      if (this.cboCondType.SelectedIndex == 0)
      {
        foreach (EnhancedConditionType enhancedConditionType in this.getEnhancedConditionTypes())
        {
          if (enhancedConditionType.active)
            enhancedConditionTypeList.Add(enhancedConditionType);
        }
      }
      else
      {
        EnhancedConditionTypeComboItem selectedItem = (EnhancedConditionTypeComboItem) this.cboCondType.SelectedItem;
        enhancedConditionTypeList.Add((EnhancedConditionType) selectedItem.Value);
      }
      EnhancedConditionTemplate[] conditionTemplates = this.getEnhancedConditionTemplates();
      if (conditionTemplates == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "No Condition Templates found for Add.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        using (AddEnhancedConditionsDialog conditionsDialog = new AddEnhancedConditionsDialog(this.loanDataMgr, conditionTemplates, enhancedConditionTypeList.ToArray()))
        {
          if (conditionsDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK || conditionsDialog.Conditions == null || conditionsDialog.Conditions.Length == 0)
            return;
          this.loadConditionList();
        }
      }
    }

    private void initConditionBorrowerField()
    {
      this.cboConditionBorrower.Items.AddRange((object[]) this.loanDataMgr.LoanData.GetBorrowerPairs());
      this.cboConditionBorrower.Items.Add((object) BorrowerPair.All);
    }

    private void loadConditionBorrowerField(string conditionPairId)
    {
      foreach (BorrowerPair borrowerPair in this.cboConditionBorrower.Items)
      {
        if (borrowerPair.Id == conditionPairId)
          this.cboConditionBorrower.SelectedItem = (object) borrowerPair;
      }
      this.txtConditionBorrower.Text = this.cboConditionBorrower.Text;
    }

    private void cboConditionBorrower_SelectionChangeCommitted(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition == null)
        return;
      BorrowerPair selectedItem = (BorrowerPair) this.cboConditionBorrower.SelectedItem;
      if (selectedItem == null)
        return;
      selectedCondition.PairId = selectedItem.Id;
    }

    private void loadSourceField(ConditionLog cond)
    {
      this.cboSource.SelectedItem = !this.cboSource.Items.Contains((object) cond.Source) ? (object) null : (object) cond.Source;
      this.txtSource.Text = this.cboSource.Text;
    }

    private void cboSource_SelectionChangeCommitted(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition == null)
        return;
      selectedCondition.Source = (string) this.cboSource.SelectedItem;
    }

    private void loadRecipientField(PostClosingConditionLog cond)
    {
      this.cboRecipient.SelectedItem = !this.cboRecipient.Items.Contains((object) cond.Recipient) ? (object) null : (object) cond.Recipient;
      this.txtRecipient.Text = this.cboRecipient.Text;
    }

    private void cboRecipient_SelectionChangeCommitted(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition == null || !(selectedCondition is PostClosingConditionLog))
        return;
      ((PostClosingConditionLog) selectedCondition).Recipient = (string) this.cboRecipient.SelectedItem;
    }

    private void loadCategoryField(ConditionLog cond)
    {
      string str = string.Empty;
      if (cond is PreliminaryConditionLog)
        str = cond.Category;
      if (cond is UnderwritingConditionLog)
        str = cond.Category;
      if (cond is SellConditionLog)
        str = cond.Category;
      this.cboCategory.SelectedItem = !this.cboCategory.Items.Contains((object) str) ? (object) null : (object) str;
      this.txtCategory.Text = this.cboCategory.Text;
    }

    private void cboCategory_SelectionChangeCommitted(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition == null)
        return;
      if (selectedCondition is PreliminaryConditionLog)
        selectedCondition.Category = (string) this.cboCategory.SelectedItem;
      if (selectedCondition is UnderwritingConditionLog)
        selectedCondition.Category = (string) this.cboCategory.SelectedItem;
      if (!(selectedCondition is SellConditionLog))
        return;
      selectedCondition.Category = (string) this.cboCategory.SelectedItem;
    }

    private void loadPriorToField(ConditionLog cond)
    {
      string str1 = string.Empty;
      if (cond is PreliminaryConditionLog)
        str1 = ((PreliminaryConditionLog) cond).PriorTo;
      if (cond is UnderwritingConditionLog)
        str1 = ((UnderwritingConditionLog) cond).PriorTo;
      if (cond is SellConditionLog)
        str1 = ((SellConditionLog) cond).PriorTo;
      string str2 = (string) null;
      switch (str1)
      {
        case "PTA":
          str2 = "Approval";
          break;
        case "PTD":
          str2 = "Docs";
          break;
        case "PTF":
          str2 = "Funding";
          break;
        case "AC":
          str2 = "Closing";
          break;
        case "PTP":
          str2 = "Purchase";
          break;
      }
      this.cboPriorTo.SelectedItem = (object) str2;
      this.txtPriorTo.Text = this.cboPriorTo.Text;
    }

    private void cboPriorTo_SelectionChangeCommitted(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition == null)
        return;
      string str = (string) this.cboPriorTo.SelectedItem;
      switch (str)
      {
        case "Approval":
          str = "PTA";
          break;
        case "Docs":
          str = "PTD";
          break;
        case "Funding":
          str = "PTF";
          break;
        case "Closing":
          str = "AC";
          break;
        case "Purchase":
          str = "PTP";
          break;
      }
      if (selectedCondition is PreliminaryConditionLog)
        ((PreliminaryConditionLog) selectedCondition).PriorTo = str;
      if (selectedCondition is UnderwritingConditionLog)
        ((UnderwritingConditionLog) selectedCondition).PriorTo = str;
      if (!(selectedCondition is SellConditionLog))
        return;
      ((SellConditionLog) selectedCondition).PriorTo = str;
    }

    private void initOwnerField()
    {
      this.cboOwner.Items.AddRange((object[]) this.loanDataMgr.SystemConfiguration.AllRoles);
    }

    private void loadOwnerField(ConditionLog cond)
    {
      if (cond != null && cond is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) cond;
        foreach (RoleInfo roleInfo in this.cboOwner.Items)
        {
          if (roleInfo.RoleID == underwritingConditionLog.ForRoleID)
            this.cboOwner.SelectedItem = (object) roleInfo;
        }
      }
      else if (cond != null && cond is SellConditionLog)
      {
        SellConditionLog sellConditionLog = (SellConditionLog) cond;
        foreach (RoleInfo roleInfo in this.cboOwner.Items)
        {
          if (roleInfo.RoleID == sellConditionLog.ForRoleID)
            this.cboOwner.SelectedItem = (object) roleInfo;
        }
      }
      this.txtOwner.Text = this.cboOwner.Text;
    }

    private void cboOwner_SelectionChangeCommitted(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition != null && selectedCondition is UnderwritingConditionLog)
      {
        ((UnderwritingConditionLog) selectedCondition).ForRoleID = ((RoleSummaryInfo) this.cboOwner.SelectedItem).RoleID;
      }
      else
      {
        if (selectedCondition == null || !(selectedCondition is SellConditionLog))
          return;
        ((SellConditionLog) selectedCondition).ForRoleID = ((RoleSummaryInfo) this.cboOwner.SelectedItem).RoleID;
      }
    }

    private void chkAllowToClear_Click(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition != null && selectedCondition is UnderwritingConditionLog)
      {
        ((UnderwritingConditionLog) selectedCondition).AllowToClear = this.chkAllowToClear.Checked;
      }
      else
      {
        if (selectedCondition == null || !(selectedCondition is SellConditionLog))
          return;
        ((SellConditionLog) selectedCondition).AllowToClear = this.chkAllowToClear.Checked;
      }
    }

    private void chkInternal_Click(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition != null && selectedCondition is UnderwritingConditionLog)
      {
        ((UnderwritingConditionLog) selectedCondition).IsInternal = this.chkInternal.Checked;
      }
      else
      {
        if (selectedCondition == null || !(selectedCondition is SellConditionLog))
          return;
        ((SellConditionLog) selectedCondition).IsInternal = this.chkInternal.Checked;
      }
    }

    private void chkExternal_Click(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition != null && selectedCondition is UnderwritingConditionLog)
      {
        ((UnderwritingConditionLog) selectedCondition).IsExternal = this.chkExternal.Checked;
      }
      else
      {
        if (selectedCondition == null || !(selectedCondition is SellConditionLog))
          return;
        ((SellConditionLog) selectedCondition).IsExternal = this.chkExternal.Checked;
      }
    }

    private void loadUnderwriterAccessField(PreliminaryConditionLog cond)
    {
      this.chkUnderwriter.Checked = cond.UnderwriterAccess;
    }

    private void chkUnderwriter_Click(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition == null)
        return;
      ((PreliminaryConditionLog) selectedCondition).UnderwriterAccess = this.chkUnderwriter.Checked;
    }

    private void loadConditionExpectedFields(StandardConditionLog cond)
    {
      if (cond.DaysTillDue > 0)
        this.txtConditionDaysDue.Text = cond.DaysTillDue.ToString();
      else
        this.txtConditionDaysDue.Text = string.Empty;
      if (cond.Expected)
        this.txtConditionDateDue.Text = cond.DateExpected.ToString("MM/dd/yy");
      else
        this.txtConditionDateDue.Text = string.Empty;
    }

    private void txtConditionDaysDue_Validating(object sender, CancelEventArgs e)
    {
      if (!(this.txtConditionDaysDue.Text != string.Empty) || Utils.IsInt((object) this.txtConditionDaysDue.Text))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "You must specify a valid number for the Days to Receive.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      e.Cancel = true;
    }

    private void txtConditionDaysDue_Validated(object sender, EventArgs e)
    {
      if (!(this.getSelectedCondition() is StandardConditionLog selectedCondition))
        return;
      int num = Utils.ParseInt((object) this.txtConditionDaysDue.Text, 0);
      if (selectedCondition.DaysTillDue == num)
        return;
      selectedCondition.DaysTillDue = num;
    }

    private void loadConditionRequestedFromField(string requestedFrom)
    {
      this.txtConditionRequestedFrom.Text = requestedFrom;
    }

    private void txtConditionRequestedFrom_Validated(object sender, EventArgs e)
    {
      if (!(this.getSelectedCondition() is StandardConditionLog selectedCondition) || !(selectedCondition.RequestedFrom != this.txtConditionRequestedFrom.Text))
        return;
      selectedCondition.RequestedFrom = this.txtConditionRequestedFrom.Text;
    }

    private void initFulfilledFields()
    {
      RolesMappingInfo[] roleMappings = this.loanDataMgr.SystemConfiguration.RoleMappings;
      RoleInfo[] allRoles = this.loanDataMgr.SystemConfiguration.AllRoles;
      foreach (RolesMappingInfo rolesMappingInfo in roleMappings)
      {
        if (rolesMappingInfo.RealWorldRoleID == RealWorldRoleID.Underwriter)
        {
          foreach (int roleId in rolesMappingInfo.RoleIDList)
          {
            foreach (RoleInfo roleInfo in allRoles)
            {
              if (roleInfo.RoleID == roleId)
                this.underwriterRole = roleInfo;
            }
          }
        }
      }
    }

    private void loadFulfilledFields(ConditionLog cond, eFolderAccessRights condRights)
    {
      bool flag = false;
      DateTime dateTime = DateTime.MinValue;
      string str = string.Empty;
      if (cond is PreliminaryConditionLog)
      {
        PreliminaryConditionLog preliminaryConditionLog = (PreliminaryConditionLog) cond;
        flag = preliminaryConditionLog.Fulfilled;
        dateTime = preliminaryConditionLog.DateFulfilled;
        str = preliminaryConditionLog.FulfilledBy;
      }
      if (cond is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) cond;
        flag = underwritingConditionLog.Fulfilled;
        dateTime = underwritingConditionLog.DateFulfilled;
        str = underwritingConditionLog.FulfilledBy;
      }
      if (cond is SellConditionLog)
      {
        SellConditionLog sellConditionLog = (SellConditionLog) cond;
        flag = sellConditionLog.Fulfilled;
        dateTime = sellConditionLog.DateFulfilled;
        str = sellConditionLog.FulfilledBy;
      }
      this.chkFulfilled.Checked = flag;
      if (this.chkFulfilled.Checked)
      {
        this.dateFulfilled.Value = dateTime;
        this.txtDateFulfilled.Text = dateTime.ToString(this.dateFulfilled.CustomFormat);
        this.txtFulfilledBy.Text = str;
      }
      this.txtFulfilledBy.Visible = this.chkFulfilled.Checked;
      if (cond is PreliminaryConditionLog)
      {
        this.dateFulfilled.Visible = this.chkFulfilled.Checked && condRights.CanEditPreliminaryCondition;
        this.txtDateFulfilled.Visible = this.chkFulfilled.Checked && !condRights.CanEditPreliminaryCondition;
        this.btnFulfilledBy.Visible = this.chkFulfilled.Checked && condRights.CanEditPreliminaryCondition;
      }
      if (cond is UnderwritingConditionLog)
      {
        this.dateFulfilled.Visible = this.chkFulfilled.Checked && condRights.CanChangeSignoffDate;
        this.txtDateFulfilled.Visible = this.chkFulfilled.Checked && !condRights.CanChangeSignoffDate;
        this.btnFulfilledBy.Visible = this.chkFulfilled.Checked && condRights.CanChangeSignoffName;
      }
      if (!(cond is SellConditionLog))
        return;
      this.dateFulfilled.Visible = this.chkFulfilled.Checked && condRights.CanEditSellCondition;
      this.txtDateFulfilled.Visible = this.chkFulfilled.Checked && !condRights.CanEditSellCondition;
      this.btnFulfilledBy.Visible = this.chkFulfilled.Checked && condRights.CanEditSellCondition;
    }

    private void chkFulfilled_Click(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition == null)
        return;
      if (selectedCondition is PreliminaryConditionLog)
      {
        PreliminaryConditionLog preliminaryConditionLog = (PreliminaryConditionLog) selectedCondition;
        if (this.chkFulfilled.Checked)
          preliminaryConditionLog.MarkAsFulfilled(DateTime.Now, Session.UserID);
        else
          preliminaryConditionLog.UnmarkAsFulfilled();
      }
      if (selectedCondition is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) selectedCondition;
        if (this.chkFulfilled.Checked)
          underwritingConditionLog.MarkAsFulfilled(DateTime.Now, Session.UserID);
        else
          underwritingConditionLog.UnmarkAsFulfilled();
      }
      if (selectedCondition is SellConditionLog)
      {
        SellConditionLog sellConditionLog = (SellConditionLog) selectedCondition;
        if (this.chkFulfilled.Checked)
          sellConditionLog.MarkAsFulfilled(DateTime.Now, Session.UserID);
        else
          sellConditionLog.UnmarkAsFulfilled();
      }
      if (selectedCondition is SellConditionLog)
        return;
      this.checkUnderwriterAccess(selectedCondition);
    }

    private void dateFulfilled_ValueChanged(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition == null)
        return;
      if (selectedCondition is PreliminaryConditionLog)
      {
        PreliminaryConditionLog preliminaryConditionLog = (PreliminaryConditionLog) selectedCondition;
        if (this.dateFulfilled.Value != preliminaryConditionLog.DateFulfilled)
          preliminaryConditionLog.MarkAsFulfilled(this.dateFulfilled.Value, this.txtFulfilledBy.Text);
      }
      if (selectedCondition is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) selectedCondition;
        if (this.dateFulfilled.Value != underwritingConditionLog.DateFulfilled)
          underwritingConditionLog.MarkAsFulfilled(this.dateFulfilled.Value, this.txtFulfilledBy.Text);
      }
      if (!(selectedCondition is SellConditionLog))
        return;
      SellConditionLog sellConditionLog = (SellConditionLog) selectedCondition;
      if (!(this.dateFulfilled.Value != sellConditionLog.DateFulfilled))
        return;
      sellConditionLog.MarkAsFulfilled(this.dateFulfilled.Value, this.txtFulfilledBy.Text);
    }

    private void btnFulfilledBy_Click(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition == null)
        return;
      string user = this.selectUser();
      if (user == null)
        return;
      if (selectedCondition is PreliminaryConditionLog)
      {
        PreliminaryConditionLog preliminaryConditionLog = (PreliminaryConditionLog) selectedCondition;
        preliminaryConditionLog.MarkAsFulfilled(preliminaryConditionLog.DateFulfilled, user);
      }
      if (selectedCondition is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) selectedCondition;
        underwritingConditionLog.MarkAsFulfilled(underwritingConditionLog.DateFulfilled, user);
      }
      if (!(selectedCondition is SellConditionLog))
        return;
      SellConditionLog sellConditionLog = (SellConditionLog) selectedCondition;
      sellConditionLog.MarkAsFulfilled(sellConditionLog.DateFulfilled, user);
    }

    private void checkUnderwriterAccess(ConditionLog cond)
    {
      if (this.underwriterRole == null || !new eFolderAccessRights(this.loanDataMgr).CanSetDocumentAccess)
        return;
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      foreach (DocumentLog linkedDocument in cond.GetLinkedDocuments())
      {
        if (!linkedDocument.IsAccessibleToRole(this.underwriterRole.RoleID))
          documentLogList.Add(linkedDocument);
      }
      if (documentLogList.Count == 0)
        return;
      string str = string.Empty;
      foreach (DocumentLog documentLog in documentLogList)
        str = str + documentLog.Title + "\r\n";
      if (Utils.Dialog((IWin32Window) Form.ActiveForm, "Would you like to grant the '" + this.underwriterRole.Name + "' role access to the following document(s):\r\n\r\n" + str, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        return;
      foreach (DocumentLog documentLog in documentLogList)
        documentLog.GrantAccess(this.underwriterRole.RoleID);
    }

    private void loadConditionRequestedFields(StandardConditionLog cond)
    {
      this.chkConditionRequested.Checked = cond.Requested;
      if (this.chkConditionRequested.Checked)
      {
        this.dateConditionRequested.Value = cond.DateRequested;
        this.txtConditionDateRequested.Text = cond.DateRequested.ToString(this.dateConditionRequested.CustomFormat);
        this.txtConditionRequestedBy.Text = cond.RequestedBy;
      }
      this.dateConditionRequested.Visible = this.chkConditionRequested.Checked && this.canEditCondition;
      this.txtConditionDateRequested.Visible = this.chkConditionRequested.Checked && !this.canEditCondition;
      this.txtConditionRequestedBy.Visible = this.chkConditionRequested.Checked;
      this.btnConditionRequestedBy.Visible = this.chkConditionRequested.Checked && this.canEditCondition;
    }

    private void chkConditionRequested_Click(object sender, EventArgs e)
    {
      if (!(this.getSelectedCondition() is StandardConditionLog selectedCondition))
        return;
      if (this.chkConditionRequested.Checked)
        selectedCondition.MarkAsRequested(DateTime.Now, Session.UserID);
      else
        selectedCondition.UnmarkAsRequested();
    }

    private void dateConditionRequested_ValueChanged(object sender, EventArgs e)
    {
      if (!(this.getSelectedCondition() is StandardConditionLog selectedCondition) || !(this.dateConditionRequested.Value != selectedCondition.DateRequested))
        return;
      selectedCondition.MarkAsRequested(this.dateConditionRequested.Value, this.txtConditionRequestedBy.Text);
    }

    private void btnConditionRequestedBy_Click(object sender, EventArgs e)
    {
      if (!(this.getSelectedCondition() is StandardConditionLog selectedCondition))
        return;
      string user = this.selectUser();
      if (user == null)
        return;
      selectedCondition.MarkAsRequested(selectedCondition.DateRequested, user);
    }

    private void loadConditionRerequestedFields(StandardConditionLog cond)
    {
      this.chkConditionRerequested.Checked = cond.Rerequested;
      if (this.chkConditionRerequested.Checked)
      {
        this.dateConditionRerequested.Value = cond.DateRerequested;
        this.txtConditionDateRerequested.Text = cond.DateRerequested.ToString(this.dateConditionRerequested.CustomFormat);
        this.txtConditionRerequestedBy.Text = cond.RerequestedBy;
      }
      this.dateConditionRerequested.Visible = this.chkConditionRerequested.Checked && this.canEditCondition;
      this.txtConditionDateRerequested.Visible = this.chkConditionRerequested.Checked && !this.canEditCondition;
      this.txtConditionRerequestedBy.Visible = this.chkConditionRerequested.Checked;
      this.btnConditionRerequestedBy.Visible = this.chkConditionRerequested.Checked && this.canEditCondition;
    }

    private void chkConditionRerequested_Click(object sender, EventArgs e)
    {
      if (!(this.getSelectedCondition() is StandardConditionLog selectedCondition))
        return;
      if (this.chkConditionRerequested.Checked)
        selectedCondition.MarkAsRerequested(DateTime.Now, Session.UserID);
      else
        selectedCondition.UnmarkAsRerequested();
    }

    private void dateConditionRerequested_ValueChanged(object sender, EventArgs e)
    {
      if (!(this.getSelectedCondition() is StandardConditionLog selectedCondition) || !(this.dateConditionRerequested.Value != selectedCondition.DateRerequested))
        return;
      selectedCondition.MarkAsRerequested(this.dateConditionRerequested.Value, this.txtConditionRerequestedBy.Text);
    }

    private void btnConditionRerequestedBy_Click(object sender, EventArgs e)
    {
      if (!(this.getSelectedCondition() is StandardConditionLog selectedCondition))
        return;
      string user = this.selectUser();
      if (user == null)
        return;
      selectedCondition.MarkAsRerequested(selectedCondition.DateRerequested, user);
    }

    private void loadConditionReceivedFields(
      StandardConditionLog cond,
      eFolderAccessRights condRights)
    {
      this.chkConditionReceived.Checked = cond.Received;
      if (this.chkConditionReceived.Checked)
      {
        this.dateConditionReceived.Value = cond.DateReceived;
        this.txtConditionDateReceived.Text = cond.DateReceived.ToString(this.dateConditionReceived.CustomFormat);
        this.txtConditionReceivedBy.Text = cond.ReceivedBy;
      }
      this.txtConditionReceivedBy.Visible = this.chkConditionReceived.Checked;
      if (cond is UnderwritingConditionLog)
      {
        this.dateConditionReceived.Visible = this.chkConditionReceived.Checked && condRights.CanChangeSignoffDate;
        this.txtConditionDateReceived.Visible = this.chkConditionReceived.Checked && !condRights.CanChangeSignoffDate;
        this.btnConditionReceivedBy.Visible = this.chkConditionReceived.Checked && condRights.CanChangeSignoffName;
      }
      else
      {
        this.dateConditionReceived.Visible = this.chkConditionReceived.Checked && this.canEditCondition;
        this.txtConditionDateReceived.Visible = this.chkConditionReceived.Checked && !this.canEditCondition;
        this.btnConditionReceivedBy.Visible = this.chkConditionReceived.Checked && this.canEditCondition;
      }
    }

    private void chkConditionReceived_Click(object sender, EventArgs e)
    {
      if (!(this.getSelectedCondition() is StandardConditionLog selectedCondition))
        return;
      if (this.chkConditionReceived.Checked)
        selectedCondition.MarkAsReceived(DateTime.Now, Session.UserID);
      else
        selectedCondition.UnmarkAsReceived();
    }

    private void dateConditionReceived_ValueChanged(object sender, EventArgs e)
    {
      if (!(this.getSelectedCondition() is StandardConditionLog selectedCondition) || !(this.dateConditionReceived.Value != selectedCondition.DateReceived))
        return;
      selectedCondition.MarkAsReceived(this.dateConditionReceived.Value, this.txtConditionReceivedBy.Text);
    }

    private void btnConditionReceivedBy_Click(object sender, EventArgs e)
    {
      if (!(this.getSelectedCondition() is StandardConditionLog selectedCondition))
        return;
      string user = this.selectUser();
      if (user == null)
        return;
      selectedCondition.MarkAsReceived(selectedCondition.DateReceived, user);
    }

    private void loadConditionReviewedFields(ConditionLog condparam, eFolderAccessRights condRights)
    {
      if (condparam != null && condparam is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) condparam;
        this.chkConditionReviewed.Checked = underwritingConditionLog.Reviewed;
        if (this.chkConditionReviewed.Checked)
        {
          this.dateConditionReviewed.Value = underwritingConditionLog.DateReviewed;
          this.txtConditionDateReviewed.Text = underwritingConditionLog.DateReviewed.ToString(this.dateConditionReviewed.CustomFormat);
          this.txtConditionReviewedBy.Text = underwritingConditionLog.ReviewedBy;
        }
        this.dateConditionReviewed.Visible = this.chkConditionReviewed.Checked && condRights.CanChangeSignoffDate;
        this.txtConditionDateReviewed.Visible = this.chkConditionReviewed.Checked && !condRights.CanChangeSignoffDate;
        this.txtConditionReviewedBy.Visible = this.chkConditionReviewed.Checked;
        this.btnConditionReviewedBy.Visible = this.chkConditionReviewed.Checked && condRights.CanChangeSignoffName;
      }
      else
      {
        if (condparam == null || !(condparam is SellConditionLog))
          return;
        SellConditionLog sellConditionLog = (SellConditionLog) condparam;
        this.chkConditionReviewed.Checked = sellConditionLog.Reviewed;
        if (this.chkConditionReviewed.Checked)
        {
          this.dateConditionReviewed.Value = sellConditionLog.DateReviewed;
          this.txtConditionDateReviewed.Text = sellConditionLog.DateReviewed.ToString(this.dateConditionReviewed.CustomFormat);
          this.txtConditionReviewedBy.Text = sellConditionLog.ReviewedBy;
        }
        this.dateConditionReviewed.Visible = this.chkConditionReviewed.Checked && this.canEditCondition;
        this.txtConditionDateReviewed.Visible = this.chkConditionReviewed.Checked && !this.canEditCondition;
        this.txtConditionReviewedBy.Visible = this.chkConditionReviewed.Checked;
        this.btnConditionReviewedBy.Visible = this.chkConditionReviewed.Checked && this.canEditCondition;
      }
    }

    private void chkConditionReviewed_Click(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition != null && selectedCondition is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) selectedCondition;
        if (this.chkConditionReviewed.Checked)
          underwritingConditionLog.MarkAsReviewed(DateTime.Now, Session.UserID);
        else
          underwritingConditionLog.UnmarkAsReviewed();
      }
      else
      {
        if (selectedCondition == null || !(selectedCondition is SellConditionLog))
          return;
        SellConditionLog sellConditionLog = (SellConditionLog) selectedCondition;
        if (this.chkConditionReviewed.Checked)
          sellConditionLog.MarkAsReviewed(DateTime.Now, Session.UserID);
        else
          sellConditionLog.UnmarkAsReviewed();
      }
    }

    private void dateConditionReviewed_ValueChanged(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition != null && selectedCondition is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) selectedCondition;
        if (!(this.dateConditionReviewed.Value != underwritingConditionLog.DateReviewed))
          return;
        underwritingConditionLog.MarkAsReviewed(this.dateConditionReviewed.Value, underwritingConditionLog.ReviewedBy);
      }
      else
      {
        if (selectedCondition == null || !(selectedCondition is SellConditionLog))
          return;
        SellConditionLog sellConditionLog = (SellConditionLog) selectedCondition;
        if (!(this.dateConditionReviewed.Value != sellConditionLog.DateReviewed))
          return;
        sellConditionLog.MarkAsReviewed(this.dateConditionReviewed.Value, sellConditionLog.ReviewedBy);
      }
    }

    private void btnConditionReviewedBy_Click(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition != null && selectedCondition is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) selectedCondition;
        string user = this.selectUser();
        if (user == null)
          return;
        underwritingConditionLog.MarkAsReviewed(underwritingConditionLog.DateReviewed, user);
      }
      else
      {
        if (selectedCondition == null || !(selectedCondition is SellConditionLog))
          return;
        SellConditionLog sellConditionLog = (SellConditionLog) selectedCondition;
        string user = this.selectUser();
        if (user == null)
          return;
        sellConditionLog.MarkAsReviewed(sellConditionLog.DateReviewed, user);
      }
    }

    private void loadRejectedFields(ConditionLog condparam, eFolderAccessRights condRights)
    {
      if (condparam != null && condparam is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) condparam;
        this.chkRejected.Checked = underwritingConditionLog.Rejected;
        if (this.chkRejected.Checked)
        {
          this.dateRejected.Value = underwritingConditionLog.DateRejected;
          this.txtDateRejected.Text = underwritingConditionLog.DateRejected.ToString(this.dateRejected.CustomFormat);
          this.txtRejectedBy.Text = underwritingConditionLog.RejectedBy;
        }
        this.dateRejected.Visible = this.chkRejected.Checked && condRights.CanChangeSignoffDate;
        this.txtDateRejected.Visible = this.chkRejected.Checked && !condRights.CanChangeSignoffDate;
        this.txtRejectedBy.Visible = this.chkRejected.Checked;
        this.btnRejectedBy.Visible = this.chkRejected.Checked && condRights.CanChangeSignoffName;
      }
      else
      {
        if (condparam == null || !(condparam is SellConditionLog))
          return;
        SellConditionLog sellConditionLog = (SellConditionLog) condparam;
        this.chkRejected.Checked = sellConditionLog.Rejected;
        if (this.chkRejected.Checked)
        {
          this.dateRejected.Value = sellConditionLog.DateRejected;
          this.txtDateRejected.Text = sellConditionLog.DateRejected.ToString(this.dateRejected.CustomFormat);
          this.txtRejectedBy.Text = sellConditionLog.RejectedBy;
        }
        this.dateRejected.Visible = this.chkRejected.Checked && condRights.CanEditSellCondition;
        this.txtDateRejected.Visible = this.chkRejected.Checked && !condRights.CanEditSellCondition;
        this.txtRejectedBy.Visible = this.chkRejected.Checked;
        this.btnRejectedBy.Visible = this.chkRejected.Checked && condRights.CanEditSellCondition;
      }
    }

    private void chkRejected_Click(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition != null && selectedCondition is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) selectedCondition;
        if (this.chkRejected.Checked)
          underwritingConditionLog.MarkAsRejected(DateTime.Now, Session.UserID);
        else
          underwritingConditionLog.UnmarkAsRejected();
      }
      else
      {
        if (selectedCondition == null || !(selectedCondition is SellConditionLog))
          return;
        SellConditionLog sellConditionLog = (SellConditionLog) selectedCondition;
        if (this.chkRejected.Checked)
          sellConditionLog.MarkAsRejected(DateTime.Now, Session.UserID);
        else
          sellConditionLog.UnmarkAsRejected();
      }
    }

    private void dateRejected_ValueChanged(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition != null && selectedCondition is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) selectedCondition;
        if (!(this.dateRejected.Value != underwritingConditionLog.DateRejected))
          return;
        underwritingConditionLog.MarkAsRejected(this.dateRejected.Value, underwritingConditionLog.RejectedBy);
      }
      else
      {
        if (selectedCondition == null || !(selectedCondition is SellConditionLog))
          return;
        SellConditionLog sellConditionLog = (SellConditionLog) selectedCondition;
        if (!(this.dateRejected.Value != sellConditionLog.DateRejected))
          return;
        sellConditionLog.MarkAsRejected(this.dateRejected.Value, sellConditionLog.RejectedBy);
      }
    }

    private void btnRejectedBy_Click(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition != null && selectedCondition is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) selectedCondition;
        string user = this.selectUser();
        if (user == null)
          return;
        underwritingConditionLog.MarkAsRejected(underwritingConditionLog.DateRejected, user);
      }
      else
      {
        if (selectedCondition == null || !(selectedCondition is SellConditionLog))
          return;
        SellConditionLog sellConditionLog = (SellConditionLog) selectedCondition;
        string user = this.selectUser();
        if (user == null)
          return;
        sellConditionLog.MarkAsRejected(sellConditionLog.DateRejected, user);
      }
    }

    private void loadSentFields(PostClosingConditionLog cond)
    {
      this.chkSent.Checked = cond.Sent;
      if (this.chkSent.Checked)
      {
        this.dateSent.Value = cond.DateSent;
        this.txtDateSent.Text = cond.DateSent.ToString(this.dateSent.CustomFormat);
        this.txtSentBy.Text = cond.SentBy;
      }
      this.dateSent.Visible = this.chkSent.Checked && this.canEditCondition;
      this.txtDateSent.Visible = this.chkSent.Checked && !this.canEditCondition;
      this.txtSentBy.Visible = this.chkSent.Checked;
      this.btnSentBy.Visible = this.chkSent.Checked && this.canEditCondition;
    }

    private void chkSent_Click(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition == null || !(selectedCondition is PostClosingConditionLog))
        return;
      PostClosingConditionLog closingConditionLog = (PostClosingConditionLog) selectedCondition;
      if (this.chkSent.Checked)
        closingConditionLog.MarkAsSent(DateTime.Now, Session.UserID);
      else
        closingConditionLog.UnmarkAsSent();
    }

    private void dateSent_ValueChanged(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition == null || !(selectedCondition is PostClosingConditionLog))
        return;
      PostClosingConditionLog closingConditionLog = (PostClosingConditionLog) selectedCondition;
      if (!(this.dateSent.Value != closingConditionLog.DateSent))
        return;
      closingConditionLog.MarkAsSent(this.dateSent.Value, closingConditionLog.SentBy);
    }

    private void btnSentBy_Click(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition == null || !(selectedCondition is PostClosingConditionLog))
        return;
      PostClosingConditionLog closingConditionLog = (PostClosingConditionLog) selectedCondition;
      string user = this.selectUser();
      if (user == null)
        return;
      closingConditionLog.MarkAsSent(closingConditionLog.DateSent, user);
    }

    private void loadClearedFields(ConditionLog cond, eFolderAccessRights condRights)
    {
      DateTime dateCleared;
      if (cond is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) cond;
        this.chkCleared.Checked = underwritingConditionLog.Cleared;
        if (this.chkCleared.Checked)
        {
          this.dateCleared.Value = underwritingConditionLog.DateCleared;
          TextBox txtDateCleared = this.txtDateCleared;
          dateCleared = underwritingConditionLog.DateCleared;
          string str = dateCleared.ToString(this.dateCleared.CustomFormat);
          txtDateCleared.Text = str;
          this.txtClearedBy.Text = underwritingConditionLog.ClearedBy;
        }
        this.dateCleared.Visible = this.chkCleared.Checked && condRights.CanChangeSignoffDate;
        this.txtDateCleared.Visible = this.chkCleared.Checked && !condRights.CanChangeSignoffDate;
        this.txtClearedBy.Visible = this.chkCleared.Checked;
        this.btnClearedBy.Visible = this.chkCleared.Checked && condRights.CanChangeSignoffName;
      }
      if (cond is PostClosingConditionLog)
      {
        PostClosingConditionLog closingConditionLog = (PostClosingConditionLog) cond;
        this.chkCleared.Checked = closingConditionLog.Cleared;
        if (this.chkCleared.Checked)
        {
          this.dateCleared.Value = closingConditionLog.DateCleared;
          TextBox txtDateCleared = this.txtDateCleared;
          dateCleared = closingConditionLog.DateCleared;
          string str = dateCleared.ToString(this.dateCleared.CustomFormat);
          txtDateCleared.Text = str;
          this.txtClearedBy.Text = closingConditionLog.ClearedBy;
        }
      }
      if (cond is SellConditionLog)
      {
        SellConditionLog sellConditionLog = (SellConditionLog) cond;
        this.chkCleared.Checked = sellConditionLog.Cleared;
        if (this.chkCleared.Checked)
        {
          this.dateCleared.Value = sellConditionLog.DateCleared;
          TextBox txtDateCleared = this.txtDateCleared;
          dateCleared = sellConditionLog.DateCleared;
          string str = dateCleared.ToString(this.dateCleared.CustomFormat);
          txtDateCleared.Text = str;
          this.txtClearedBy.Text = sellConditionLog.ClearedBy;
        }
      }
      this.dateCleared.Visible = this.chkCleared.Checked && this.canEditCondition;
      this.txtDateCleared.Visible = this.chkCleared.Checked && !this.canEditCondition;
      this.txtClearedBy.Visible = this.chkCleared.Checked;
      this.btnClearedBy.Visible = this.chkCleared.Checked && this.canEditCondition;
    }

    private void chkCleared_Click(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition == null)
        return;
      if (selectedCondition is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) selectedCondition;
        if (this.chkCleared.Checked)
          underwritingConditionLog.MarkAsCleared(DateTime.Now, Session.UserID);
        else
          underwritingConditionLog.UnmarkAsCleared();
      }
      if (selectedCondition is PostClosingConditionLog)
      {
        PostClosingConditionLog closingConditionLog = (PostClosingConditionLog) selectedCondition;
        if (this.chkCleared.Checked)
          closingConditionLog.MarkAsCleared(DateTime.Now, Session.UserID);
        else
          closingConditionLog.UnmarkAsCleared();
      }
      if (!(selectedCondition is SellConditionLog))
        return;
      SellConditionLog sellConditionLog = (SellConditionLog) selectedCondition;
      if (this.chkCleared.Checked)
        sellConditionLog.MarkAsCleared(DateTime.Now, Session.UserID);
      else
        sellConditionLog.UnmarkAsCleared();
    }

    private void dateCleared_ValueChanged(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition == null)
        return;
      if (selectedCondition is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) selectedCondition;
        if (this.dateCleared.Value != underwritingConditionLog.DateCleared)
          underwritingConditionLog.MarkAsCleared(this.dateCleared.Value, underwritingConditionLog.ClearedBy);
      }
      if (selectedCondition is PostClosingConditionLog)
      {
        PostClosingConditionLog closingConditionLog = (PostClosingConditionLog) selectedCondition;
        if (this.dateCleared.Value != closingConditionLog.DateCleared)
          closingConditionLog.MarkAsCleared(this.dateCleared.Value, closingConditionLog.ClearedBy);
      }
      if (!(selectedCondition is SellConditionLog))
        return;
      SellConditionLog sellConditionLog = (SellConditionLog) selectedCondition;
      if (!(this.dateCleared.Value != sellConditionLog.DateCleared))
        return;
      sellConditionLog.MarkAsCleared(this.dateCleared.Value, sellConditionLog.ClearedBy);
    }

    private void btnClearedBy_Click(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition == null)
        return;
      string user = this.selectUser();
      if (user == null)
        return;
      if (selectedCondition is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) selectedCondition;
        underwritingConditionLog.MarkAsCleared(underwritingConditionLog.DateCleared, user);
      }
      if (selectedCondition is PostClosingConditionLog)
      {
        PostClosingConditionLog closingConditionLog = (PostClosingConditionLog) selectedCondition;
        closingConditionLog.MarkAsCleared(closingConditionLog.DateCleared, user);
      }
      if (!(selectedCondition is SellConditionLog))
        return;
      SellConditionLog sellConditionLog = (SellConditionLog) selectedCondition;
      sellConditionLog.MarkAsCleared(sellConditionLog.DateCleared, user);
    }

    private void loadWaivedFields(ConditionLog condparam, eFolderAccessRights condRights)
    {
      if (condparam != null && condparam is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) condparam;
        this.chkWaived.Checked = underwritingConditionLog.Waived;
        if (this.chkWaived.Checked)
        {
          this.dateWaived.Value = underwritingConditionLog.DateWaived;
          this.txtDateWaived.Text = underwritingConditionLog.DateWaived.ToString(this.dateWaived.CustomFormat);
          this.txtWaivedBy.Text = underwritingConditionLog.WaivedBy;
        }
        this.dateWaived.Visible = this.chkWaived.Checked && condRights.CanChangeSignoffDate;
        this.txtDateWaived.Visible = this.chkWaived.Checked && !condRights.CanChangeSignoffDate;
        this.txtWaivedBy.Visible = this.chkWaived.Checked;
        this.btnWaivedBy.Visible = this.chkWaived.Checked && condRights.CanChangeSignoffName;
      }
      else
      {
        if (condparam == null || !(condparam is SellConditionLog))
          return;
        SellConditionLog sellConditionLog = (SellConditionLog) condparam;
        this.chkWaived.Checked = sellConditionLog.Waived;
        if (this.chkWaived.Checked)
        {
          this.dateWaived.Value = sellConditionLog.DateWaived;
          this.txtDateWaived.Text = sellConditionLog.DateWaived.ToString(this.dateWaived.CustomFormat);
          this.txtWaivedBy.Text = sellConditionLog.WaivedBy;
        }
        this.dateWaived.Visible = this.chkWaived.Checked && condRights.CanEditSellCondition;
        this.txtDateWaived.Visible = this.chkWaived.Checked && !condRights.CanEditSellCondition;
        this.txtWaivedBy.Visible = this.chkWaived.Checked;
        this.btnWaivedBy.Visible = this.chkWaived.Checked && condRights.CanEditSellCondition;
      }
    }

    private void chkWaived_Click(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition != null && selectedCondition is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) selectedCondition;
        if (this.chkWaived.Checked)
          underwritingConditionLog.MarkAsWaived(DateTime.Now, Session.UserID);
        else
          underwritingConditionLog.UnmarkAsWaived();
      }
      else
      {
        if (selectedCondition == null || !(selectedCondition is SellConditionLog))
          return;
        SellConditionLog sellConditionLog = (SellConditionLog) selectedCondition;
        if (this.chkWaived.Checked)
          sellConditionLog.MarkAsWaived(DateTime.Now, Session.UserID);
        else
          sellConditionLog.UnmarkAsWaived();
      }
    }

    private void dateWaived_ValueChanged(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition != null && selectedCondition is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) selectedCondition;
        if (!(this.dateWaived.Value != underwritingConditionLog.DateWaived))
          return;
        underwritingConditionLog.MarkAsWaived(this.dateWaived.Value, underwritingConditionLog.WaivedBy);
      }
      else
      {
        if (selectedCondition == null || !(selectedCondition is SellConditionLog))
          return;
        SellConditionLog sellConditionLog = (SellConditionLog) selectedCondition;
        if (!(this.dateWaived.Value != sellConditionLog.DateWaived))
          return;
        sellConditionLog.MarkAsWaived(this.dateWaived.Value, sellConditionLog.WaivedBy);
      }
    }

    private void btnWaivedBy_Click(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition != null && selectedCondition is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) selectedCondition;
        string user = this.selectUser();
        if (user == null)
          return;
        underwritingConditionLog.MarkAsWaived(underwritingConditionLog.DateWaived, user);
      }
      else
      {
        if (selectedCondition == null || !(selectedCondition is SellConditionLog))
          return;
        SellConditionLog sellConditionLog = (SellConditionLog) selectedCondition;
        string user = this.selectUser();
        if (user == null)
          return;
        sellConditionLog.MarkAsWaived(sellConditionLog.DateWaived, user);
      }
    }

    private void gvDocuments_MouseDown(object sender, MouseEventArgs e)
    {
      if (this.gvDocuments.GetItemAt(e.X, e.Y) == null)
        return;
      this.dragPoint = e.Location;
    }

    private void gvDocuments_MouseMove(object sender, MouseEventArgs e)
    {
      if (!(this.dragPoint != Point.Empty) || this.dragPoint.Equals((object) e.Location))
        return;
      int num = (int) this.gvDocuments.DoDragDrop((object) this.gvDocuments.SelectedItems, DragDropEffects.Move);
      this.dragPoint = Point.Empty;
    }

    private void gvDocuments_MouseUp(object sender, MouseEventArgs e)
    {
      this.dragPoint = Point.Empty;
    }

    private void gvDocuments_DragEnter(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(typeof (GVItem)))
        e.Effect = DragDropEffects.Move;
      else
        e.Effect = DragDropEffects.None;
    }

    private void gvDocuments_DragDrop(object sender, DragEventArgs e)
    {
      GVItem data = (GVItem) e.Data.GetData(typeof (GVItem));
      DocumentLog tag1 = (DocumentLog) data.Tag;
      ConditionLog tag2 = (ConditionLog) data.ParentItem.Tag;
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) tag2);
      switch (tag2)
      {
        case PreliminaryConditionLog _:
          if (!folderAccessRights.CanRemovePreliminaryConditionDocuments)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You cannot remove documents from the '" + tag2.Title + "' condition because you do not have rights to remove documents from Preliminary Conditions.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          break;
        case UnderwritingConditionLog _:
          if (!folderAccessRights.CanRemoveUnderwritingConditionDocuments)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You cannot remove documents from the '" + tag2.Title + "' condition because you do not have rights to remove documents from Underwriting Conditions.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          break;
        case PostClosingConditionLog _:
          if (!folderAccessRights.CanRemovePostClosingConditionDocuments)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You cannot remove documents from the '" + tag2.Title + "' condition because you do not have rights to remove documents from Post-Closing Conditions.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          break;
        case SellConditionLog _:
          if (!folderAccessRights.CanRemoveSellConditionDocuments)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You cannot remove documents from the '" + tag2.Title + "' condition because you do not have rights to remove documents from Delivery Conditions.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          break;
      }
      tag1.Conditions.Remove(tag2);
      this.showDocumentFiles(tag1);
      this.showHideTabs();
    }

    private void gvConditions_MouseDown(object sender, MouseEventArgs e)
    {
      GVItem itemAt = this.gvConditions.GetItemAt(e.X, e.Y);
      if (itemAt == null || !(itemAt.Tag is DocumentLog))
        return;
      this.dragPoint = e.Location;
    }

    private void gvConditions_MouseMove(object sender, MouseEventArgs e)
    {
      if (!(this.dragPoint != Point.Empty) || this.dragPoint.Equals((object) e.Location))
        return;
      if (this.gvConditions.SelectedItems.Count > 0)
      {
        int num = (int) this.gvConditions.DoDragDrop((object) this.gvConditions.SelectedItems[0], DragDropEffects.Move);
      }
      this.dragPoint = Point.Empty;
    }

    private void gvConditions_MouseUp(object sender, MouseEventArgs e)
    {
      this.dragPoint = Point.Empty;
    }

    private void gvConditions_DragEnter(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(typeof (GVSelectedItemCollection)))
        e.Effect = DragDropEffects.Move;
      else
        e.Effect = DragDropEffects.None;
    }

    private void gvConditions_DragOver(object sender, DragEventArgs e)
    {
      e.Effect = DragDropEffects.None;
      Point client = this.gvConditions.PointToClient(new Point(e.X, e.Y));
      GVItem itemAt = this.gvConditions.GetItemAt(client.X, client.Y);
      if (itemAt == null)
        return;
      if (e.Data.GetDataPresent(typeof (GVSelectedItemCollection)) && itemAt.Tag is ConditionLog)
        e.Effect = DragDropEffects.Move;
      itemAt.BackColor = EncompassColors.DragAnddropColor;
    }

    private void gvConditions_DragDrop(object sender, DragEventArgs e)
    {
      Point client = this.gvConditions.PointToClient(new Point(e.X, e.Y));
      GVItem itemAt = this.gvConditions.GetItemAt(client.X, client.Y);
      ConditionLog tag = (ConditionLog) itemAt.Tag;
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) tag);
      switch (tag)
      {
        case PreliminaryConditionLog _:
          if (!folderAccessRights.CanAddPreliminaryConditionDocuments)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You cannot add documents to the '" + tag.Title + "' condition because you do not have rights to add documents to Preliminary Conditions.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          break;
        case UnderwritingConditionLog _:
          if (!folderAccessRights.CanAddUnderwritingConditionDocuments)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You cannot add documents to the '" + tag.Title + "' condition because you do not have rights to add documents to Underwriting Conditions.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          break;
        case PostClosingConditionLog _:
          if (!folderAccessRights.CanAddPostClosingConditionDocuments)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You cannot add documents to the '" + tag.Title + "' condition because you do not have rights to add documents to Post-Closing Conditions.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          break;
        case SellConditionLog _:
          if (!folderAccessRights.CanAddSellConditionDocuments)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You cannot add documents to the '" + tag.Title + "' condition because you do not have rights to add documents to Delivery Conditions.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          break;
      }
      foreach (GVItem gvItem in (GVSelectedItemCollection) e.Data.GetData(typeof (GVSelectedItemCollection)))
        ((DocumentLog) gvItem.Tag).Conditions.Add(tag);
      itemAt.State = GVItemState.Normal;
      this.refreshCondition(tag);
    }

    private void initConditionBorrowerEnhancedField()
    {
      this.cboConditionBorrowerEnhanced.Items.AddRange((object[]) this.loanDataMgr.LoanData.GetBorrowerPairs());
      this.cboConditionBorrowerEnhanced.Items.Add((object) BorrowerPair.All);
    }

    private void loadConditionBorrowerEnhancedField(string conditionPairId)
    {
      foreach (BorrowerPair borrowerPair in this.cboConditionBorrowerEnhanced.Items)
      {
        if (borrowerPair.Id == conditionPairId)
          this.cboConditionBorrowerEnhanced.SelectedItem = (object) borrowerPair;
      }
    }

    private void cboConditionBorrowerEnhanced_SelectionChangeCommitted(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedCondition();
      if (selectedCondition == null)
        return;
      BorrowerPair selectedItem = (BorrowerPair) this.cboConditionBorrowerEnhanced.SelectedItem;
      if (selectedItem == null)
        return;
      selectedCondition.PairId = selectedItem.Id;
    }

    private void loadConditionTypeField(string enhancedtype)
    {
      this.txtConditionType.Text = enhancedtype;
    }

    private void initSourceEnhancedField(EnhancedConditionLog cond)
    {
      this.cboSourceEnhanced.Items.Clear();
      foreach (object sourceDefinition in (IEnumerable<OptionDefinition>) cond.Definitions.SourceDefinitions)
        this.cboSourceEnhanced.Items.Add(sourceDefinition);
    }

    private void loadSourceEnhancedField(EnhancedConditionLog cond)
    {
      this.cboSourceEnhanced.SelectedItem = (object) null;
      foreach (OptionDefinition optionDefinition in this.cboSourceEnhanced.Items)
      {
        if (optionDefinition.Name == cond.Source)
          this.cboSourceEnhanced.SelectedItem = (object) optionDefinition;
      }
    }

    private void cboSourceEnhanced_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (!(this.getSelectedCondition() is EnhancedConditionLog selectedCondition))
        return;
      OptionDefinition selectedItem = (OptionDefinition) this.cboSourceEnhanced.SelectedItem;
      if (!(selectedCondition.Source != selectedItem.Name))
        return;
      selectedCondition.Source = selectedItem.Name;
    }

    private void initPriorToEnhancedField(EnhancedConditionLog cond)
    {
      this.cboPriorToEnhanced.Items.Clear();
      foreach (object priorToDefinition in (IEnumerable<OptionDefinition>) cond.Definitions.PriorToDefinitions)
        this.cboPriorToEnhanced.Items.Add(priorToDefinition);
    }

    private void loadPriorToEnhancedField(EnhancedConditionLog cond)
    {
      this.cboPriorToEnhanced.SelectedItem = (object) null;
      foreach (OptionDefinition optionDefinition in this.cboPriorToEnhanced.Items)
      {
        if (optionDefinition.Name == cond.PriorTo)
          this.cboPriorToEnhanced.SelectedItem = (object) optionDefinition;
      }
    }

    private void cboPriorToEnhanced_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (!(this.getSelectedCondition() is EnhancedConditionLog selectedCondition))
        return;
      OptionDefinition selectedItem = (OptionDefinition) this.cboPriorToEnhanced.SelectedItem;
      if (!(selectedCondition.PriorTo != selectedItem.Name))
        return;
      selectedCondition.PriorTo = selectedItem.Name;
    }

    private void loadDaysToReceiveEnhancedField(EnhancedConditionLog cond)
    {
      this.txtDaysToReceiveEnhanced.Text = cond.DaysToReceive.ToString();
    }

    private void txtDaysToReceiveEnhanced_Validated(object sender, EventArgs e)
    {
      EnhancedConditionLog selectedCondition = this.getSelectedCondition() as EnhancedConditionLog;
      int result;
      if (!int.TryParse(this.txtDaysToReceiveEnhanced.Text, out result))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter a numeric value for Days To Receive, MessageBoxButtons.OK, MessageBoxIcon.Stop");
      }
      if (!(selectedCondition.DaysToReceive.ToString() != this.txtDaysToReceiveEnhanced.Text))
        return;
      selectedCondition.DaysToReceive = new int?(result);
      this.loadDateDueEnhancedField(selectedCondition);
    }

    private void loadDateDueEnhancedField(EnhancedConditionLog cond)
    {
      if (!cond.DaysToReceive.HasValue)
        return;
      TextBox txtDateDueEnhanced = this.txtDateDueEnhanced;
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.AddDays((double) cond.DaysToReceive.Value);
      string str = "on " + dateTime.ToShortDateString();
      txtDateDueEnhanced.Text = str;
    }

    private void loadRequestedFromEnhancedField(EnhancedConditionLog cond)
    {
      this.txtRequestedFromEnhanced.Text = cond.RequestedFrom;
    }

    private void txtRequestedFromEnhanced_Validated(object sender, EventArgs e)
    {
      EnhancedConditionLog selectedCondition = this.getSelectedCondition() as EnhancedConditionLog;
      if (!(selectedCondition.RequestedFrom != this.txtRequestedFromEnhanced.Text))
        return;
      selectedCondition.RequestedFrom = this.txtRequestedFromEnhanced.Text;
    }

    private void initTrackingList()
    {
      this.gvTrackingMgr = new GridViewDataManager(this.gvTrackingEnhanced, this.loanDataMgr);
      this.gvTrackingMgr.CreateLayout(new TableLayout.Column[1]
      {
        GridViewDataManager.CheckBoxColumn
      });
    }

    private void loadTrackingList(EnhancedConditionLog cond)
    {
      RoleInfo[] allRoles = this.loanDataMgr.SystemConfiguration.AllRoles;
      this.gvTrackingMgr.ClearItems();
      GVItem gvItem = new GVItem()
      {
        SubItems = {
          [0] = {
            Value = (object) ("Added by " + cond.AddedBy + " on " + cond.DateAdded.ToShortDateString() + " at " + cond.DateAdded.ToShortTimeString())
          }
        },
        Checked = true
      };
      gvItem.SubItems[0].CheckBoxEnabled = false;
      this.gvTrackingMgr.AddItem(gvItem);
      List<string> defaultTrackingOptions = Utils.GetEnhanceConditionsDefaultTrackingOptions();
      if (cond.Definitions == null || cond.Definitions.TrackingDefinitions == null)
        return;
      foreach (string str in defaultTrackingOptions)
      {
        foreach (StatusTrackingDefinition trackingDefinition in (IEnumerable<StatusTrackingDefinition>) cond.Definitions.TrackingDefinitions)
        {
          if (trackingDefinition.Name == str)
            this.addTrackingDefinitionItem(trackingDefinition, allRoles, cond, true);
        }
      }
      foreach (StatusTrackingDefinition trackingDefinition in (IEnumerable<StatusTrackingDefinition>) cond.Definitions.TrackingDefinitions)
      {
        if (!defaultTrackingOptions.Contains(trackingDefinition.Name))
          this.addTrackingDefinitionItem(trackingDefinition, allRoles, cond, false);
      }
    }

    private void addTrackingDefinitionItem(
      StatusTrackingDefinition trackingDefinition,
      RoleInfo[] roleList,
      EnhancedConditionLog cond,
      bool isDefault)
    {
      GVItem gvItem = new GVItem();
      gvItem.SubItems[0].Value = (object) trackingDefinition.Name;
      gvItem.SubItems[0].Checked = false;
      foreach (StatusTrackingEntry statusTrackingEntry in cond.Trackings.GetStatusTrackingEntries())
      {
        if (statusTrackingEntry.Status == trackingDefinition.Name)
        {
          gvItem.SubItems[0].Value = (object) (trackingDefinition.Name + " by " + statusTrackingEntry.UserId + " on " + (object) statusTrackingEntry.Date);
          gvItem.SubItems[0].Checked = true;
          break;
        }
      }
      gvItem.SubItems[0].CheckBoxEnabled = isDefault || this.isRoleAllowed(roleList, trackingDefinition);
      gvItem.SubItems[0].Tag = (object) trackingDefinition;
      this.gvTrackingMgr.AddItem(gvItem);
    }

    private bool isRoleAllowed(RoleInfo[] roleList, StatusTrackingDefinition trackDef)
    {
      if (this.loanDataMgr.SessionObjects.StartupInfo.UserInfo.IsSuperAdministrator())
        return true;
      if (this.canEditCondition)
      {
        foreach (int allowedRole in trackDef.AllowedRoles)
        {
          foreach (RoleSummaryInfo role in roleList)
          {
            if (role.ID == allowedRole)
              return true;
          }
        }
      }
      return false;
    }

    private void gvTrackingEnhanced_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      EnhancedConditionLog selectedCondition = this.getSelectedCondition() as EnhancedConditionLog;
      StatusTrackingDefinition tag = (StatusTrackingDefinition) e.SubItem.Tag;
      if (e.SubItem.Checked)
        selectedCondition.Trackings.Add(tag.Name, Session.UserID);
      else
        selectedCondition.Trackings.Remove(tag.Name);
      this.loadTrackingList(selectedCondition);
    }

    private void loadCommentListEnhanced(EnhancedConditionLog cond)
    {
      this.commentCollectionEnhanced.LoadComments(this.loanDataMgr, cond.Comments);
    }

    private void txtCommentEnhanced_TextChanged(object sender, EventArgs e)
    {
      this.btnAddCommentEnhanced.Enabled = this.txtCommentEnhanced.Text.Length > 0;
      this.chkExternalCommentEnhanced.Enabled = this.btnAddCommentEnhanced.Enabled;
    }

    private void btnAddCommentEnhanced_Click(object sender, EventArgs e)
    {
      EnhancedConditionLog selectedCondition = this.getSelectedCondition() as EnhancedConditionLog;
      CommentEntry entry = new CommentEntry(this.txtCommentEnhanced.Text, Session.UserID, this.loanDataMgr.SessionObjects.UserInfo.FullName, !this.chkExternalCommentEnhanced.Checked);
      selectedCondition.Comments.Add(entry);
      this.loadCommentListEnhanced(selectedCondition);
    }

    private void applyUserDocumentSecurity()
    {
      this.cboDocumentBorrower.Visible = this.docMgr.CanEditDocuments;
      this.txtDocumentBorrower.Visible = !this.docMgr.CanEditDocuments;
      this.cboMilestone.Visible = this.docMgr.CanEditDocuments;
      this.txtMilestone.Visible = !this.docMgr.CanEditDocuments;
      this.btnAccess.Visible = this.docMgr.CanSetDocumentAccess;
      if (Session.IsBrokerEdition())
      {
        this.btnAccess.Visible = false;
        this.lblAccess.Visible = false;
        this.txtAccess.Visible = false;
        this.lblConditions.Top = this.lblAccess.Top;
        this.lstConditions.Bounds = new Rectangle(this.lstConditions.Left, this.txtAccess.Top, this.lstConditions.Width, this.lstConditions.Bottom - this.txtAccess.Top);
      }
      this.chkWebCenter.Enabled = this.docMgr.CanEditDocuments;
      this.chkTPOWebcenterPortal.Enabled = this.docMgr.CanEditDocuments;
      this.chkThirdParty.Enabled = this.docMgr.CanEditDocuments;
      this.txtDocumentDaysDue.ReadOnly = !this.docMgr.CanEditDocuments;
      this.txtDaysExpire.ReadOnly = !this.docMgr.CanEditDocuments;
      this.txtDocumentRequestedFrom.ReadOnly = !this.docMgr.CanEditDocuments;
      this.chkDocumentRequested.Enabled = this.docMgr.CanEditDocuments;
      this.chkDocumentRerequested.Enabled = this.docMgr.CanEditDocuments;
      this.chkDocumentReceived.Enabled = this.docMgr.CanEditDocuments;
      this.chkDocumentReviewed.Enabled = this.docMgr.CanEditDocuments;
      this.chkUnderwritingReady.Enabled = this.docMgr.CanEditDocuments;
      this.chkShippingReady.Enabled = this.docMgr.CanEditDocuments;
      this.btnAddDocumentComment.Visible = this.docMgr.CanAddComments;
      this.btnDeleteDocumentComment.Visible = this.docMgr.CanDeleteComments;
    }

    private void applyBasicConditionSecurity()
    {
      this.chkUnderwriter.Visible = this.condType == ConditionType.Preliminary;
      if (this.condType == ConditionType.Preliminary)
        this.chkUnderwriter.Top = this.chkAllowToClear.Top;
      this.chkCleared.Visible = this.condType != ConditionType.Preliminary;
      this.dateCleared.Visible = false;
      this.txtDateCleared.Visible = false;
      this.txtClearedBy.Visible = false;
      this.btnClearedBy.Visible = false;
      this.lblOwner.Visible = this.condType == ConditionType.Underwriting || this.condType == ConditionType.Sell;
      this.cboOwner.Visible = this.condType == ConditionType.Underwriting || this.condType == ConditionType.Sell;
      this.txtOwner.Visible = this.condType == ConditionType.Underwriting || this.condType == ConditionType.Sell;
      this.chkAllowToClear.Visible = this.condType == ConditionType.Underwriting || this.condType == ConditionType.Sell;
      this.chkInternal.Visible = this.condType == ConditionType.Underwriting || this.condType == ConditionType.Sell;
      this.chkExternal.Visible = this.condType == ConditionType.Underwriting || this.condType == ConditionType.Sell;
      this.chkConditionReviewed.Visible = this.condType == ConditionType.Underwriting || this.condType == ConditionType.Sell;
      this.dateConditionReviewed.Visible = false;
      this.txtConditionDateReviewed.Visible = false;
      this.txtConditionReviewedBy.Visible = false;
      this.btnConditionReviewedBy.Visible = false;
      this.chkRejected.Visible = this.condType == ConditionType.Underwriting || this.condType == ConditionType.Sell;
      this.dateRejected.Visible = false;
      this.txtDateRejected.Visible = false;
      this.txtRejectedBy.Visible = false;
      this.btnRejectedBy.Visible = false;
      this.chkWaived.Visible = this.condType == ConditionType.Underwriting || this.condType == ConditionType.Sell;
      this.dateWaived.Visible = false;
      this.txtDateWaived.Visible = false;
      this.txtWaivedBy.Visible = false;
      this.btnWaivedBy.Visible = false;
      this.lblCategory.Visible = this.condType != ConditionType.PostClosing;
      this.cboCategory.Visible = this.condType != ConditionType.PostClosing;
      this.txtCategory.Visible = this.condType != ConditionType.PostClosing;
      this.lblPriorTo.Visible = this.condType != ConditionType.PostClosing;
      this.cboPriorTo.Visible = this.condType != ConditionType.PostClosing;
      this.txtPriorTo.Visible = this.condType != ConditionType.PostClosing;
      this.chkFulfilled.Visible = this.condType != ConditionType.PostClosing;
      this.dateFulfilled.Visible = false;
      this.txtDateFulfilled.Visible = false;
      this.txtFulfilledBy.Visible = false;
      this.btnFulfilledBy.Visible = false;
      this.cboSource.Visible = this.condType == ConditionType.PostClosing;
      this.lblRecipient.Visible = this.condType == ConditionType.PostClosing;
      this.cboRecipient.Visible = this.condType == ConditionType.PostClosing;
      this.txtRecipient.Visible = this.condType == ConditionType.PostClosing;
      this.chkSent.Visible = this.condType == ConditionType.PostClosing;
      this.dateSent.Visible = false;
      this.txtDateSent.Visible = false;
      this.txtSentBy.Visible = false;
      this.btnSentBy.Visible = false;
      switch (this.condType)
      {
        case ConditionType.Underwriting:
          this.btnAddCondition.Visible = this.userRights.CanAddUnderwritingConditions;
          break;
        case ConditionType.PostClosing:
          this.btnAddCondition.Visible = this.userRights.CanAddPostClosingConditions;
          break;
        case ConditionType.Preliminary:
          this.btnAddCondition.Visible = this.userRights.CanAddPreliminaryConditions;
          break;
        case ConditionType.Sell:
          this.btnAddCondition.Visible = this.userRights.CanAddSellConditions;
          break;
      }
      int num1 = this.statusRowHeight;
      int num2 = this.statusRowHeight * 2;
      int num3 = this.statusRowHeight * 3;
      int num4 = 0;
      if (this.condType == ConditionType.PostClosing)
      {
        this.lblRecipient.Top = this.lblCategory.Top;
        this.cboRecipient.Top = this.cboCategory.Top;
        this.txtRecipient.Top = this.txtCategory.Top;
        num1 = 0;
        num2 = this.statusRowHeight;
        num3 = this.statusRowHeight * 2;
        this.chkSent.Top = this.chkFulfilled.Top + this.statusRowHeight * 3;
        this.dateSent.Top = this.txtDateFulfilled.Top + this.statusRowHeight * 3;
        this.txtDateSent.Top = this.txtDateFulfilled.Top + this.statusRowHeight * 3;
        this.txtSentBy.Top = this.txtDateFulfilled.Top + this.statusRowHeight * 3;
        this.btnSentBy.Top = this.txtDateFulfilled.Top + this.statusRowHeight * 3;
        num4 = this.statusRowHeight * 4;
      }
      if (this.condType == ConditionType.Underwriting || this.condType == ConditionType.Sell)
        num4 = this.statusRowHeight * 6;
      this.chkConditionRequested.Top = this.chkFulfilled.Top + num1;
      this.dateConditionRequested.Top = this.dateFulfilled.Top + num1;
      this.txtConditionDateRequested.Top = this.txtDateFulfilled.Top + num1;
      this.txtConditionRequestedBy.Top = this.txtFulfilledBy.Top + num1;
      this.btnConditionRequestedBy.Top = this.btnFulfilledBy.Top + num1;
      this.chkConditionRerequested.Top = this.chkFulfilled.Top + num2;
      this.dateConditionRerequested.Top = this.txtDateFulfilled.Top + num2;
      this.txtConditionDateRerequested.Top = this.txtDateFulfilled.Top + num2;
      this.txtConditionRerequestedBy.Top = this.txtDateFulfilled.Top + num2;
      this.btnConditionRerequestedBy.Top = this.txtDateFulfilled.Top + num2;
      this.chkConditionReceived.Top = this.chkFulfilled.Top + num3;
      this.dateConditionReceived.Top = this.txtDateFulfilled.Top + num3;
      this.txtConditionDateReceived.Top = this.txtDateFulfilled.Top + num3;
      this.txtConditionReceivedBy.Top = this.txtDateFulfilled.Top + num3;
      this.btnConditionReceivedBy.Top = this.txtDateFulfilled.Top + num3;
      this.chkCleared.Top = this.chkFulfilled.Top + num4;
      this.dateCleared.Top = this.txtDateFulfilled.Top + num4;
      this.txtDateCleared.Top = this.txtDateFulfilled.Top + num4;
      this.txtClearedBy.Top = this.txtDateFulfilled.Top + num4;
      this.btnClearedBy.Top = this.txtDateFulfilled.Top + num4;
    }

    private void applyConditionLevelSecurity(ConditionLog cond, eFolderAccessRights condRights)
    {
      this.canEditCondition = false;
      if (cond is PreliminaryConditionLog)
      {
        this.canEditCondition = condRights.CanEditPreliminaryCondition;
        this.cboPriorTo.Visible = condRights.CanEditPreliminaryCondition;
        this.txtPriorTo.Visible = !condRights.CanEditPreliminaryCondition;
        this.chkFulfilled.Enabled = condRights.CanEditPreliminaryCondition;
        this.chkConditionReceived.Enabled = true;
        this.btnAddConditionComment.Visible = condRights.CanEditPreliminaryCondition;
        this.btnDeleteConditionComment.Visible = condRights.CanEditPreliminaryCondition;
      }
      if (cond is UnderwritingConditionLog)
      {
        this.canEditCondition = condRights.CanEditUnderwritingCondition;
        this.cboPriorTo.Visible = condRights.CanChangePriorTo;
        this.txtPriorTo.Visible = !condRights.CanChangePriorTo;
        this.chkFulfilled.Enabled = condRights.CanFulfillUnderwritingCondition;
        this.chkConditionReceived.Enabled = condRights.CanReceiveUnderwritingCondition;
        this.chkConditionReviewed.Enabled = condRights.CanReviewUnderwritingCondition;
        this.chkRejected.Enabled = condRights.CanRejectUnderwritingCondition;
        this.chkCleared.Enabled = condRights.CanClearUnderwritingCondition;
        this.chkWaived.Enabled = condRights.CanWaiveUnderwritingCondition;
        this.btnAddConditionComment.Visible = condRights.CanAddUnderwritingConditionComments;
        this.btnDeleteConditionComment.Visible = condRights.CanDeleteUnderwritingConditionComments;
      }
      if (cond is PostClosingConditionLog)
      {
        this.canEditCondition = condRights.CanEditPostClosingCondition;
        this.cboSource.Visible = condRights.CanEditPostClosingCondition;
        this.txtSource.Visible = !condRights.CanEditPostClosingCondition;
        this.cboRecipient.Visible = condRights.CanEditPostClosingCondition;
        this.txtRecipient.Visible = !condRights.CanEditPostClosingCondition;
        this.txtConditionDaysDue.ReadOnly = !condRights.CanEditPostClosingCondition;
        this.txtConditionRequestedFrom.ReadOnly = !condRights.CanEditPostClosingCondition;
        this.chkConditionRequested.Enabled = condRights.CanEditPostClosingCondition;
        this.chkConditionRerequested.Enabled = condRights.CanEditPostClosingCondition;
        this.chkConditionReceived.Enabled = condRights.CanEditPostClosingCondition;
        this.chkSent.Enabled = condRights.CanEditPostClosingCondition;
        this.chkCleared.Enabled = condRights.CanEditPostClosingCondition;
        this.btnAddConditionComment.Visible = condRights.CanEditPostClosingCondition;
        this.btnDeleteConditionComment.Visible = condRights.CanEditPostClosingCondition;
      }
      if (cond is SellConditionLog)
      {
        this.canEditCondition = condRights.CanEditSellCondition;
        this.txtSource.Visible = true;
        this.txtSource.Enabled = !condRights.CanEditSellCondition;
        this.cboPriorTo.Visible = condRights.CanEditSellCondition;
        this.txtPriorTo.Visible = !condRights.CanEditSellCondition;
        this.chkFulfilled.Enabled = condRights.CanEditSellCondition;
        this.chkConditionReceived.Enabled = condRights.CanEditSellCondition;
        this.txtConditionDaysDue.ReadOnly = !condRights.CanEditSellCondition;
        this.txtConditionRequestedFrom.ReadOnly = !condRights.CanEditSellCondition;
        this.chkConditionRequested.Enabled = condRights.CanEditSellCondition;
        this.chkConditionRerequested.Enabled = condRights.CanEditSellCondition;
        this.chkConditionReviewed.Enabled = condRights.CanEditSellCondition;
        this.chkRejected.Enabled = condRights.CanEditSellCondition;
        this.chkCleared.Enabled = condRights.CanEditSellCondition;
        this.chkWaived.Enabled = condRights.CanEditSellCondition;
        this.btnAddConditionComment.Visible = condRights.CanEditSellCondition;
        this.btnDeleteConditionComment.Visible = condRights.CanEditSellCondition;
      }
      this.cboConditionBorrower.Visible = this.canEditCondition;
      this.txtConditionBorrower.Visible = !this.canEditCondition;
      this.cboCategory.Visible = this.canEditCondition;
      this.txtCategory.Visible = !this.canEditCondition;
      this.cboOwner.Visible = (cond is UnderwritingConditionLog || cond is SellConditionLog) && this.canEditCondition;
      TextBox txtOwner = this.txtOwner;
      int num;
      switch (cond)
      {
        case UnderwritingConditionLog _:
        case SellConditionLog _:
          num = !this.canEditCondition ? 1 : 0;
          break;
        default:
          num = 0;
          break;
      }
      txtOwner.Visible = num != 0;
      this.chkAllowToClear.Enabled = this.canEditCondition;
      this.chkInternal.Enabled = this.canEditCondition;
      this.chkExternal.Enabled = this.canEditCondition;
      this.chkUnderwriter.Enabled = this.canEditCondition;
      if (!Session.IsBrokerEdition())
        return;
      this.chkUnderwriter.Visible = false;
      this.chkInternal.Visible = false;
      this.chkExternal.Left = this.chkInternal.Left;
      this.chkExternal.Text = "In Mortgage Loan Commitment Letter";
    }

    private void applyEnhancedConditionSecurity(
      EnhancedConditionLog cond,
      eFolderAccessRights rights)
    {
      this.canEditCondition = rights.CanEditEnhancedCondition(cond.EnhancedConditionType);
      this.cboConditionBorrowerEnhanced.Enabled = this.canEditCondition;
      this.cboSourceEnhanced.Enabled = this.canEditCondition;
      this.cboPriorToEnhanced.Enabled = this.canEditCondition;
      this.txtDaysToReceiveEnhanced.Enabled = this.canEditCondition;
      this.txtRequestedFromEnhanced.Enabled = this.canEditCondition;
      this.commentCollectionEnhanced.DisplayEnhanced(rights.CanMarkEnhancedConditionComments(cond.EnhancedConditionType));
      this.commentCollectionEnhanced.CanAddComment = false;
      this.commentCollectionEnhanced.CanDeleteComment = rights.CanDeleteEnhancedConditionComments(cond.EnhancedConditionType);
      this.txtCommentEnhanced.Enabled = rights.CanAddEnhancedConditionComments(cond.EnhancedConditionType);
    }

    private void initEventHandlers()
    {
      this.FormClosed += new FormClosedEventHandler(this.onFormClosed);
      this.loanDataMgr.LoanClosing += new EventHandler(this.btnClose_Click);
      this.loanDataMgr.LoanData.LogRecordAdded += new LogRecordEventHandler(this.logRecordChanged);
      this.loanDataMgr.LoanData.LogRecordChanged += new LogRecordEventHandler(this.logRecordChanged);
      this.loanDataMgr.LoanData.LogRecordRemoved += new LogRecordEventHandler(this.logRecordChanged);
    }

    private void releaseEventHandlers()
    {
      this.FormClosed -= new FormClosedEventHandler(this.onFormClosed);
      this.loanDataMgr.LoanClosing -= new EventHandler(this.btnClose_Click);
      this.loanDataMgr.LoanData.LogRecordAdded -= new LogRecordEventHandler(this.logRecordChanged);
      this.loanDataMgr.LoanData.LogRecordChanged -= new LogRecordEventHandler(this.logRecordChanged);
      this.loanDataMgr.LoanData.LogRecordRemoved -= new LogRecordEventHandler(this.logRecordChanged);
    }

    private void onFormClosed(object sender, FormClosedEventArgs e) => this.releaseEventHandlers();

    private void logRecordChanged(object source, LogRecordEventArgs e)
    {
      Tracing.Log(AllDocumentsDialog.sw, TraceLevel.Verbose, nameof (AllDocumentsDialog), "Checking InvokeRequired For LogRecordEventHandler");
      if (this.InvokeRequired)
      {
        LogRecordEventHandler method = new LogRecordEventHandler(this.logRecordChanged);
        Tracing.Log(AllDocumentsDialog.sw, TraceLevel.Verbose, nameof (AllDocumentsDialog), "Calling BeginInvoke For LogRecordEventHandler");
        this.BeginInvoke((Delegate) method, source, (object) e);
      }
      else
      {
        bool flag1 = false;
        bool flag2 = false;
        if (e.LogRecord is ConditionLog)
        {
          if (((ConditionLog) e.LogRecord).ConditionType == this.condType)
            flag1 = true;
        }
        else if (e.LogRecord is DocumentLog)
        {
          flag2 = true;
          DocumentLog logRecord = (DocumentLog) e.LogRecord;
          foreach (ConditionLog condition in logRecord.Conditions)
          {
            if (condition.ConditionType == this.condType)
              flag1 = true;
          }
          if (!flag1)
          {
            foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvConditions.Items)
            {
              if (gvItem.GroupItems.ContainsTag((object) logRecord))
                flag1 = true;
            }
          }
        }
        if (flag1)
        {
          if (this == Form.ActiveForm)
          {
            this.loadConditionList();
            this.showConditionFiles();
          }
          else
            this.refreshConditions = true;
        }
        if (!flag2)
          return;
        if (this == Form.ActiveForm)
        {
          this.docMgr.Refresh();
          if (this.docMgr.ConditionDocuments)
          {
            this.loadConditionList();
            this.showConditionFiles();
          }
          else
          {
            this.loadDocumentList((DocumentLog[]) null);
            this.showDocuments();
          }
        }
        else
          this.refreshDocuments = true;
      }
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Close();

    private void showHideTabs()
    {
      this.gcInfo.Visible = false;
      this.tabInfo.TabPages.Remove(this.pageFiles);
      this.tabInfo.TabPages.Remove(this.pageDocumentDetails);
      this.tabInfo.TabPages.Remove(this.pageConditionDetails);
      this.tabInfo.TabPages.Remove(this.pageConditionsEnhanced);
      if (this.docMgr.DocumentList != null && this.docMgr.DocumentList.Length != 0)
      {
        this.gcInfo.Visible = true;
        if (this.loanDataMgr.FileAttachments.GetAttachments(this.docMgr.DocumentList).Length != 0)
          this.tabInfo.TabPages.Add(this.pageFiles);
        this.tabInfo.TabPages.Add(this.pageDocumentDetails);
        this.tabInfo.SelectedTab = this.pageDocumentDetails;
      }
      if (this.gvConditions.SelectedItems.Count <= 0)
        return;
      this.gcInfo.Visible = true;
      if (this.condType == ConditionType.Enhanced)
        this.tabInfo.TabPages.Add(this.pageConditionsEnhanced);
      else
        this.tabInfo.TabPages.Add(this.pageConditionDetails);
      if (this.gvConditions.SelectedItems[0].Tag is DocumentLog)
        this.tabInfo.SelectedTab = this.pageDocumentDetails;
      else if (this.condType == ConditionType.Enhanced)
        this.tabInfo.SelectedTab = this.pageConditionsEnhanced;
      else
        this.tabInfo.SelectedTab = this.pageConditionDetails;
    }

    private void AllDocumentsDialog_Resize(object sender, EventArgs e)
    {
      this.gcDocuments.Height = (this.pnlLeft.ClientSize.Height - this.csDocuments.Height) / 2;
    }

    private void AllDocumentsDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      Session.Application.DisplayHelp(this.helpLink.HelpTag);
    }

    private void AllDocumentsDialog_Activated(object sender, EventArgs e)
    {
      if (this.refreshConditions)
      {
        int count = this.gvConditions.SelectedItems.Count;
        this.loadConditionList();
        if (count > 0)
          this.showConditionFiles();
        this.refreshConditions = false;
      }
      if (!this.refreshDocuments)
        return;
      this.docMgr.Refresh();
      if (this.docMgr.ConditionDocuments)
      {
        this.loadConditionList();
        this.showConditionFiles();
      }
      else
      {
        this.loadDocumentList((DocumentLog[]) null);
        this.showDocuments();
      }
      this.refreshDocuments = false;
    }

    private string selectUser()
    {
      int id = RoleInfo.Others.ID;
      int[] usersAssignedRoles = this.loanDataMgr.AccessRules.GetUsersAssignedRoles();
      if (usersAssignedRoles.Length != 0)
        id = usersAssignedRoles[0];
      string str = (string) null;
      using (UserSelectionDialog userSelectionDialog = new UserSelectionDialog(this.loanDataMgr, id))
      {
        if (userSelectionDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
          str = userSelectionDialog.User.Userid;
      }
      return str;
    }

    private void gvDocuments_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.showDocuments();
      this.showHideTabs();
    }

    private void AllDocumentsDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.fileViewer.CanCloseViewer())
        return;
      e.Cancel = true;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AllDocumentsDialog));
      this.imageList = new ImageList(this.components);
      this.tooltip = new ToolTip(this.components);
      this.btnDeleteDocumentComment = new StandardIconButton();
      this.btnViewDocumentComment = new StandardIconButton();
      this.btnAddDocumentComment = new StandardIconButton();
      this.btnShippingReadyBy = new StandardIconButton();
      this.btnUnderwritingReadyBy = new StandardIconButton();
      this.btnDocumentReviewedBy = new StandardIconButton();
      this.btnDocumentReceivedBy = new StandardIconButton();
      this.btnDocumentRerequestedBy = new StandardIconButton();
      this.btnDocumentRequestedBy = new StandardIconButton();
      this.btnDeleteConditionComment = new StandardIconButton();
      this.btnViewConditionComment = new StandardIconButton();
      this.btnAddConditionComment = new StandardIconButton();
      this.btnSentBy = new StandardIconButton();
      this.btnConditionReviewedBy = new StandardIconButton();
      this.btnWaivedBy = new StandardIconButton();
      this.btnClearedBy = new StandardIconButton();
      this.btnRejectedBy = new StandardIconButton();
      this.btnConditionReceivedBy = new StandardIconButton();
      this.btnConditionRerequestedBy = new StandardIconButton();
      this.btnConditionRequestedBy = new StandardIconButton();
      this.btnFulfilledBy = new StandardIconButton();
      this.btnEditCondition = new StandardIconButton();
      this.btnAddCondition = new StandardIconButton();
      this.pnlLeft = new Panel();
      this.gcConditions = new GroupContainer();
      this.cboCondType = new ComboBox();
      this.pnlToolbar = new FlowLayoutPanel();
      this.gvConditions = new GridView();
      this.csDocuments = new CollapsibleSplitter();
      this.gcDocuments = new GroupContainer();
      this.pnlStackingOrder = new GradientPanel();
      this.cboStackingOrder = new ComboBox();
      this.label2 = new Label();
      this.gvDocuments = new GridView();
      this.pnlDragDrop = new BorderPanel();
      this.lblDragDrop = new Label();
      this.btnClose = new Button();
      this.pnlClose = new Panel();
      this.helpLink = new EMHelpLink();
      this.pnlRight = new Panel();
      this.pnlViewer = new BorderPanel();
      this.fileViewer = new FileAttachmentViewerControl();
      this.gcInfo = new GroupContainer();
      this.tabInfo = new TabControl();
      this.pageFiles = new TabPage();
      this.gvFiles = new GridView();
      this.pageDocumentDetails = new TabPage();
      this.gcDocumentCommentText = new GroupContainer();
      this.txtDocumentComment = new TextBox();
      this.gcDocumentComments = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.gvDocumentComments = new GridView();
      this.gcDocumentStatus = new GroupContainer();
      this.pnlStatus = new Panel();
      this.btnDateShippingReady = new CalendarButton();
      this.txtDateShippingReady = new TextBox();
      this.btnDateUnderwritingReady = new CalendarButton();
      this.txtDateUnderwritingReady = new TextBox();
      this.btnDocumentDateReviewed = new CalendarButton();
      this.txtDocumentDateReviewed = new TextBox();
      this.btnDocumentDateReceived = new CalendarButton();
      this.txtDocumentDateReceived = new TextBox();
      this.btnDocumentDateRerequested = new CalendarButton();
      this.txtDocumentDateRerequested = new TextBox();
      this.btnDocumentDateRequested = new CalendarButton();
      this.txtDocumentDateRequested = new TextBox();
      this.txtShippingReadyBy = new TextBox();
      this.chkShippingReady = new CheckBox();
      this.txtUnderwritingReadyBy = new TextBox();
      this.chkUnderwritingReady = new CheckBox();
      this.txtDocumentReviewedBy = new TextBox();
      this.txtDocumentReceivedBy = new TextBox();
      this.txtDocumentRerequestedBy = new TextBox();
      this.txtDocumentRequestedBy = new TextBox();
      this.chkDocumentReviewed = new CheckBox();
      this.chkDocumentReceived = new CheckBox();
      this.chkDocumentRerequested = new CheckBox();
      this.chkDocumentRequested = new CheckBox();
      this.txtDocumentRequestedFrom = new TextBox();
      this.lblCompany = new Label();
      this.txtDateExpire = new TextBox();
      this.txtDaysExpire = new TextBox();
      this.lblDaysExpire = new Label();
      this.txtDocumentDateDue = new TextBox();
      this.txtDocumentDaysDue = new TextBox();
      this.lblDaysDue = new Label();
      this.gcDocumentDetails = new GroupContainer();
      this.pnlDetails = new Panel();
      this.label3 = new Label();
      this.lstATRQM = new ListBox();
      this.lstDocuments = new ListBox();
      this.chkThirdParty = new CheckBox();
      this.chkTPOWebcenterPortal = new CheckBox();
      this.chkWebCenter = new CheckBox();
      this.lblDocuments = new Label();
      this.lblAvailable = new Label();
      this.lstGroups = new ListBox();
      this.lblGroups = new Label();
      this.lstDocumentConditions = new ListBox();
      this.lblConditions = new Label();
      this.txtAccess = new TextBox();
      this.btnAccess = new Button();
      this.lblAccess = new Label();
      this.cboMilestone = new ComboBoxEx();
      this.txtMilestone = new TextBox();
      this.lblMilestone = new Label();
      this.cboDocumentBorrower = new ComboBox();
      this.txtDocumentBorrower = new TextBox();
      this.lblBorrower = new Label();
      this.pageConditionDetails = new TabPage();
      this.gcConditionCommentText = new GroupContainer();
      this.txtConditionComment = new TextBox();
      this.gcConditionComments = new GroupContainer();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.gvConditionComments = new GridView();
      this.gcConditionStatus = new GroupContainer();
      this.panel1 = new Panel();
      this.txtSentBy = new TextBox();
      this.dateSent = new DateTimePicker();
      this.txtDateSent = new TextBox();
      this.chkSent = new CheckBox();
      this.txtConditionReviewedBy = new TextBox();
      this.dateConditionReviewed = new DateTimePicker();
      this.txtConditionDateReviewed = new TextBox();
      this.chkConditionReviewed = new CheckBox();
      this.txtWaivedBy = new TextBox();
      this.dateWaived = new DateTimePicker();
      this.chkWaived = new CheckBox();
      this.txtClearedBy = new TextBox();
      this.dateCleared = new DateTimePicker();
      this.chkCleared = new CheckBox();
      this.txtRejectedBy = new TextBox();
      this.dateRejected = new DateTimePicker();
      this.chkRejected = new CheckBox();
      this.txtConditionReceivedBy = new TextBox();
      this.dateConditionReceived = new DateTimePicker();
      this.chkConditionReceived = new CheckBox();
      this.txtConditionRerequestedBy = new TextBox();
      this.dateConditionRerequested = new DateTimePicker();
      this.chkConditionRerequested = new CheckBox();
      this.txtConditionRequestedBy = new TextBox();
      this.dateConditionRequested = new DateTimePicker();
      this.chkConditionRequested = new CheckBox();
      this.txtFulfilledBy = new TextBox();
      this.dateFulfilled = new DateTimePicker();
      this.chkFulfilled = new CheckBox();
      this.txtAddedBy = new TextBox();
      this.txtAddedDate = new TextBox();
      this.chkAdded = new CheckBox();
      this.txtConditionRequestedFrom = new TextBox();
      this.label1 = new Label();
      this.txtConditionDateDue = new TextBox();
      this.txtConditionDaysDue = new TextBox();
      this.label4 = new Label();
      this.txtDateFulfilled = new TextBox();
      this.txtConditionDateRequested = new TextBox();
      this.txtConditionDateRerequested = new TextBox();
      this.txtConditionDateReceived = new TextBox();
      this.txtDateCleared = new TextBox();
      this.txtDateWaived = new TextBox();
      this.txtDateRejected = new TextBox();
      this.gcConditionDetails = new GroupContainer();
      this.panel2 = new Panel();
      this.cboSource = new ComboBox();
      this.cboRecipient = new ComboBox();
      this.txtRecipient = new TextBox();
      this.lblRecipient = new Label();
      this.lblPrint = new Label();
      this.chkExternal = new CheckBox();
      this.chkInternal = new CheckBox();
      this.chkAllowToClear = new CheckBox();
      this.cboOwner = new ComboBox();
      this.txtOwner = new TextBox();
      this.lblOwner = new Label();
      this.chkUnderwriter = new CheckBox();
      this.cboPriorTo = new ComboBox();
      this.lblPriorTo = new Label();
      this.cboCategory = new ComboBox();
      this.lblCategory = new Label();
      this.txtSource = new TextBox();
      this.lblSource = new Label();
      this.lstConditions = new ListBox();
      this.label5 = new Label();
      this.cboConditionBorrower = new ComboBox();
      this.txtConditionBorrower = new TextBox();
      this.label11 = new Label();
      this.txtCategory = new TextBox();
      this.txtPriorTo = new TextBox();
      this.pageConditionsEnhanced = new TabPage();
      this.panel5 = new Panel();
      this.txtCommentEnhanced = new TextBox();
      this.btnAddCommentEnhanced = new Button();
      this.chkExternalCommentEnhanced = new CheckBox();
      this.commentCollectionEnhanced = new CommentCollectionControl();
      this.groupContainer1 = new GroupContainer();
      this.panel4 = new Panel();
      this.gvTrackingEnhanced = new GridView();
      this.txtRequestedFromEnhanced = new TextBox();
      this.label21 = new Label();
      this.txtDateDueEnhanced = new TextBox();
      this.txtDaysToReceiveEnhanced = new TextBox();
      this.label22 = new Label();
      this.gcEnhancedConditionDetails = new GroupContainer();
      this.panel3 = new Panel();
      this.label6 = new Label();
      this.txtConditionType = new TextBox();
      this.cboSourceEnhanced = new ComboBox();
      this.cboPriorToEnhanced = new ComboBox();
      this.label9 = new Label();
      this.label12 = new Label();
      this.lstConditionsEnhanced = new ListBox();
      this.label13 = new Label();
      this.cboConditionBorrowerEnhanced = new ComboBox();
      this.label14 = new Label();
      this.csLeft = new CollapsibleSplitter();
      ((ISupportInitialize) this.btnDeleteDocumentComment).BeginInit();
      ((ISupportInitialize) this.btnViewDocumentComment).BeginInit();
      ((ISupportInitialize) this.btnAddDocumentComment).BeginInit();
      ((ISupportInitialize) this.btnShippingReadyBy).BeginInit();
      ((ISupportInitialize) this.btnUnderwritingReadyBy).BeginInit();
      ((ISupportInitialize) this.btnDocumentReviewedBy).BeginInit();
      ((ISupportInitialize) this.btnDocumentReceivedBy).BeginInit();
      ((ISupportInitialize) this.btnDocumentRerequestedBy).BeginInit();
      ((ISupportInitialize) this.btnDocumentRequestedBy).BeginInit();
      ((ISupportInitialize) this.btnDeleteConditionComment).BeginInit();
      ((ISupportInitialize) this.btnViewConditionComment).BeginInit();
      ((ISupportInitialize) this.btnAddConditionComment).BeginInit();
      ((ISupportInitialize) this.btnSentBy).BeginInit();
      ((ISupportInitialize) this.btnConditionReviewedBy).BeginInit();
      ((ISupportInitialize) this.btnWaivedBy).BeginInit();
      ((ISupportInitialize) this.btnClearedBy).BeginInit();
      ((ISupportInitialize) this.btnRejectedBy).BeginInit();
      ((ISupportInitialize) this.btnConditionReceivedBy).BeginInit();
      ((ISupportInitialize) this.btnConditionRerequestedBy).BeginInit();
      ((ISupportInitialize) this.btnConditionRequestedBy).BeginInit();
      ((ISupportInitialize) this.btnFulfilledBy).BeginInit();
      ((ISupportInitialize) this.btnEditCondition).BeginInit();
      ((ISupportInitialize) this.btnAddCondition).BeginInit();
      this.pnlLeft.SuspendLayout();
      this.gcConditions.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      this.gcDocuments.SuspendLayout();
      this.pnlStackingOrder.SuspendLayout();
      this.pnlDragDrop.SuspendLayout();
      this.pnlClose.SuspendLayout();
      this.pnlRight.SuspendLayout();
      this.pnlViewer.SuspendLayout();
      this.gcInfo.SuspendLayout();
      this.tabInfo.SuspendLayout();
      this.pageFiles.SuspendLayout();
      this.pageDocumentDetails.SuspendLayout();
      this.gcDocumentCommentText.SuspendLayout();
      this.gcDocumentComments.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.gcDocumentStatus.SuspendLayout();
      this.pnlStatus.SuspendLayout();
      ((ISupportInitialize) this.btnDateShippingReady).BeginInit();
      ((ISupportInitialize) this.btnDateUnderwritingReady).BeginInit();
      ((ISupportInitialize) this.btnDocumentDateReviewed).BeginInit();
      ((ISupportInitialize) this.btnDocumentDateReceived).BeginInit();
      ((ISupportInitialize) this.btnDocumentDateRerequested).BeginInit();
      ((ISupportInitialize) this.btnDocumentDateRequested).BeginInit();
      this.gcDocumentDetails.SuspendLayout();
      this.pnlDetails.SuspendLayout();
      this.pageConditionDetails.SuspendLayout();
      this.gcConditionCommentText.SuspendLayout();
      this.gcConditionComments.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.gcConditionStatus.SuspendLayout();
      this.panel1.SuspendLayout();
      this.gcConditionDetails.SuspendLayout();
      this.panel2.SuspendLayout();
      this.pageConditionsEnhanced.SuspendLayout();
      this.panel5.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.panel4.SuspendLayout();
      this.gcEnhancedConditionDetails.SuspendLayout();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      this.imageList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList.ImageStream");
      this.imageList.TransparentColor = Color.Transparent;
      this.imageList.Images.SetKeyName(0, "document.png");
      this.imageList.Images.SetKeyName(1, "document-with-attachment.png");
      this.imageList.Images.SetKeyName(2, "condition.png");
      this.btnDeleteDocumentComment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeleteDocumentComment.BackColor = Color.Transparent;
      this.btnDeleteDocumentComment.Location = new Point(64, 3);
      this.btnDeleteDocumentComment.Margin = new Padding(4, 3, 0, 3);
      this.btnDeleteDocumentComment.MouseDownImage = (Image) null;
      this.btnDeleteDocumentComment.Name = "btnDeleteDocumentComment";
      this.btnDeleteDocumentComment.Size = new Size(16, 17);
      this.btnDeleteDocumentComment.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteDocumentComment.TabIndex = 28;
      this.btnDeleteDocumentComment.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDeleteDocumentComment, "Delete Comment");
      this.btnDeleteDocumentComment.Click += new EventHandler(this.btnDeleteDocumentComment_Click);
      this.btnViewDocumentComment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnViewDocumentComment.BackColor = Color.Transparent;
      this.btnViewDocumentComment.Location = new Point(44, 3);
      this.btnViewDocumentComment.Margin = new Padding(4, 3, 0, 3);
      this.btnViewDocumentComment.MouseDownImage = (Image) null;
      this.btnViewDocumentComment.Name = "btnViewDocumentComment";
      this.btnViewDocumentComment.Size = new Size(16, 17);
      this.btnViewDocumentComment.StandardButtonType = StandardIconButton.ButtonType.ZoomInButton;
      this.btnViewDocumentComment.TabIndex = 27;
      this.btnViewDocumentComment.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnViewDocumentComment, "View Comment");
      this.btnViewDocumentComment.Click += new EventHandler(this.btnViewDocumentComment_Click);
      this.btnAddDocumentComment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddDocumentComment.BackColor = Color.Transparent;
      this.btnAddDocumentComment.Location = new Point(24, 3);
      this.btnAddDocumentComment.Margin = new Padding(4, 3, 0, 3);
      this.btnAddDocumentComment.MouseDownImage = (Image) null;
      this.btnAddDocumentComment.Name = "btnAddDocumentComment";
      this.btnAddDocumentComment.Size = new Size(16, 17);
      this.btnAddDocumentComment.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddDocumentComment.TabIndex = 26;
      this.btnAddDocumentComment.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddDocumentComment, "Add Comment");
      this.btnAddDocumentComment.Click += new EventHandler(this.btnAddDocumentComment_Click);
      this.btnShippingReadyBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnShippingReadyBy.BackColor = Color.Transparent;
      this.btnShippingReadyBy.Location = new Point(273, 199);
      this.btnShippingReadyBy.MouseDownImage = (Image) null;
      this.btnShippingReadyBy.Name = "btnShippingReadyBy";
      this.btnShippingReadyBy.Size = new Size(16, 16);
      this.btnShippingReadyBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnShippingReadyBy.TabIndex = 96;
      this.btnShippingReadyBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnShippingReadyBy, "Select User");
      this.btnShippingReadyBy.Visible = false;
      this.btnShippingReadyBy.Click += new EventHandler(this.btnShippingReadyBy_Click);
      this.btnUnderwritingReadyBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnUnderwritingReadyBy.BackColor = Color.Transparent;
      this.btnUnderwritingReadyBy.Location = new Point(273, 175);
      this.btnUnderwritingReadyBy.MouseDownImage = (Image) null;
      this.btnUnderwritingReadyBy.Name = "btnUnderwritingReadyBy";
      this.btnUnderwritingReadyBy.Size = new Size(16, 16);
      this.btnUnderwritingReadyBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnUnderwritingReadyBy.TabIndex = 95;
      this.btnUnderwritingReadyBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnUnderwritingReadyBy, "Select User");
      this.btnUnderwritingReadyBy.Visible = false;
      this.btnUnderwritingReadyBy.Click += new EventHandler(this.btnUnderwritingReadyBy_Click);
      this.btnDocumentReviewedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDocumentReviewedBy.BackColor = Color.Transparent;
      this.btnDocumentReviewedBy.Location = new Point(273, 151);
      this.btnDocumentReviewedBy.MouseDownImage = (Image) null;
      this.btnDocumentReviewedBy.Name = "btnDocumentReviewedBy";
      this.btnDocumentReviewedBy.Size = new Size(16, 16);
      this.btnDocumentReviewedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnDocumentReviewedBy.TabIndex = 94;
      this.btnDocumentReviewedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDocumentReviewedBy, "Select User");
      this.btnDocumentReviewedBy.Visible = false;
      this.btnDocumentReviewedBy.Click += new EventHandler(this.btnDocumentReviewedBy_Click);
      this.btnDocumentReceivedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDocumentReceivedBy.BackColor = Color.Transparent;
      this.btnDocumentReceivedBy.Location = new Point(273, 126);
      this.btnDocumentReceivedBy.MouseDownImage = (Image) null;
      this.btnDocumentReceivedBy.Name = "btnDocumentReceivedBy";
      this.btnDocumentReceivedBy.Size = new Size(16, 16);
      this.btnDocumentReceivedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnDocumentReceivedBy.TabIndex = 93;
      this.btnDocumentReceivedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDocumentReceivedBy, "Select User");
      this.btnDocumentReceivedBy.Visible = false;
      this.btnDocumentReceivedBy.Click += new EventHandler(this.btnDocumentReceivedBy_Click);
      this.btnDocumentRerequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDocumentRerequestedBy.BackColor = Color.Transparent;
      this.btnDocumentRerequestedBy.Location = new Point(273, 102);
      this.btnDocumentRerequestedBy.MouseDownImage = (Image) null;
      this.btnDocumentRerequestedBy.Name = "btnDocumentRerequestedBy";
      this.btnDocumentRerequestedBy.Size = new Size(16, 16);
      this.btnDocumentRerequestedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnDocumentRerequestedBy.TabIndex = 92;
      this.btnDocumentRerequestedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDocumentRerequestedBy, "Select User");
      this.btnDocumentRerequestedBy.Visible = false;
      this.btnDocumentRerequestedBy.Click += new EventHandler(this.btnDocumentRerequestedBy_Click);
      this.btnDocumentRequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDocumentRequestedBy.BackColor = Color.Transparent;
      this.btnDocumentRequestedBy.Location = new Point(273, 79);
      this.btnDocumentRequestedBy.MouseDownImage = (Image) null;
      this.btnDocumentRequestedBy.Name = "btnDocumentRequestedBy";
      this.btnDocumentRequestedBy.Size = new Size(16, 16);
      this.btnDocumentRequestedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnDocumentRequestedBy.TabIndex = 91;
      this.btnDocumentRequestedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDocumentRequestedBy, "Select User");
      this.btnDocumentRequestedBy.Visible = false;
      this.btnDocumentRequestedBy.Click += new EventHandler(this.btnDocumentRequestedBy_Click);
      this.btnDeleteConditionComment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeleteConditionComment.BackColor = Color.Transparent;
      this.btnDeleteConditionComment.Location = new Point(64, 3);
      this.btnDeleteConditionComment.Margin = new Padding(4, 3, 0, 3);
      this.btnDeleteConditionComment.MouseDownImage = (Image) null;
      this.btnDeleteConditionComment.Name = "btnDeleteConditionComment";
      this.btnDeleteConditionComment.Size = new Size(16, 17);
      this.btnDeleteConditionComment.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteConditionComment.TabIndex = 28;
      this.btnDeleteConditionComment.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDeleteConditionComment, "Delete Comment");
      this.btnDeleteConditionComment.Visible = false;
      this.btnDeleteConditionComment.Click += new EventHandler(this.btnDeleteConditionComment_Click);
      this.btnViewConditionComment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnViewConditionComment.BackColor = Color.Transparent;
      this.btnViewConditionComment.Location = new Point(44, 3);
      this.btnViewConditionComment.Margin = new Padding(4, 3, 0, 3);
      this.btnViewConditionComment.MouseDownImage = (Image) null;
      this.btnViewConditionComment.Name = "btnViewConditionComment";
      this.btnViewConditionComment.Size = new Size(16, 17);
      this.btnViewConditionComment.StandardButtonType = StandardIconButton.ButtonType.ZoomInButton;
      this.btnViewConditionComment.TabIndex = 27;
      this.btnViewConditionComment.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnViewConditionComment, "View Comment");
      this.btnViewConditionComment.Click += new EventHandler(this.btnViewConditionComment_Click);
      this.btnAddConditionComment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddConditionComment.BackColor = Color.Transparent;
      this.btnAddConditionComment.Location = new Point(24, 3);
      this.btnAddConditionComment.Margin = new Padding(4, 3, 0, 3);
      this.btnAddConditionComment.MouseDownImage = (Image) null;
      this.btnAddConditionComment.Name = "btnAddConditionComment";
      this.btnAddConditionComment.Size = new Size(16, 17);
      this.btnAddConditionComment.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddConditionComment.TabIndex = 26;
      this.btnAddConditionComment.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddConditionComment, "Add Comment");
      this.btnAddConditionComment.Visible = false;
      this.btnAddConditionComment.Click += new EventHandler(this.btnAddConditionComment_Click);
      this.btnSentBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSentBy.BackColor = Color.Transparent;
      this.btnSentBy.Location = new Point(293, 274);
      this.btnSentBy.MouseDownImage = (Image) null;
      this.btnSentBy.Name = "btnSentBy";
      this.btnSentBy.Size = new Size(16, 16);
      this.btnSentBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSentBy.TabIndex = 118;
      this.btnSentBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnSentBy, "Select User");
      this.btnSentBy.Click += new EventHandler(this.btnSentBy_Click);
      this.btnConditionReviewedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnConditionReviewedBy.BackColor = Color.Transparent;
      this.btnConditionReviewedBy.Location = new Point(293, 178);
      this.btnConditionReviewedBy.MouseDownImage = (Image) null;
      this.btnConditionReviewedBy.Name = "btnConditionReviewedBy";
      this.btnConditionReviewedBy.Size = new Size(16, 16);
      this.btnConditionReviewedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnConditionReviewedBy.TabIndex = 111;
      this.btnConditionReviewedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnConditionReviewedBy, "Select User");
      this.btnConditionReviewedBy.Visible = false;
      this.btnConditionReviewedBy.Click += new EventHandler(this.btnConditionReviewedBy_Click);
      this.btnWaivedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnWaivedBy.BackColor = Color.Transparent;
      this.btnWaivedBy.Location = new Point(294, 250);
      this.btnWaivedBy.MouseDownImage = (Image) null;
      this.btnWaivedBy.Name = "btnWaivedBy";
      this.btnWaivedBy.Size = new Size(16, 16);
      this.btnWaivedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnWaivedBy.TabIndex = 98;
      this.btnWaivedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnWaivedBy, "Select User");
      this.btnWaivedBy.Visible = false;
      this.btnWaivedBy.Click += new EventHandler(this.btnWaivedBy_Click);
      this.btnClearedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClearedBy.BackColor = Color.Transparent;
      this.btnClearedBy.Location = new Point(294, 226);
      this.btnClearedBy.MouseDownImage = (Image) null;
      this.btnClearedBy.Name = "btnClearedBy";
      this.btnClearedBy.Size = new Size(16, 16);
      this.btnClearedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnClearedBy.TabIndex = 97;
      this.btnClearedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnClearedBy, "Select User");
      this.btnClearedBy.Visible = false;
      this.btnClearedBy.Click += new EventHandler(this.btnClearedBy_Click);
      this.btnRejectedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRejectedBy.BackColor = Color.Transparent;
      this.btnRejectedBy.Location = new Point(294, 202);
      this.btnRejectedBy.MouseDownImage = (Image) null;
      this.btnRejectedBy.Name = "btnRejectedBy";
      this.btnRejectedBy.Size = new Size(16, 16);
      this.btnRejectedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnRejectedBy.TabIndex = 96;
      this.btnRejectedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnRejectedBy, "Select User");
      this.btnRejectedBy.Visible = false;
      this.btnRejectedBy.Click += new EventHandler(this.btnRejectedBy_Click);
      this.btnConditionReceivedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnConditionReceivedBy.BackColor = Color.Transparent;
      this.btnConditionReceivedBy.Location = new Point(293, 154);
      this.btnConditionReceivedBy.MouseDownImage = (Image) null;
      this.btnConditionReceivedBy.Name = "btnConditionReceivedBy";
      this.btnConditionReceivedBy.Size = new Size(16, 16);
      this.btnConditionReceivedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnConditionReceivedBy.TabIndex = 91;
      this.btnConditionReceivedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnConditionReceivedBy, "Select User");
      this.btnConditionReceivedBy.Visible = false;
      this.btnConditionReceivedBy.Click += new EventHandler(this.btnConditionReceivedBy_Click);
      this.btnConditionRerequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnConditionRerequestedBy.BackColor = Color.Transparent;
      this.btnConditionRerequestedBy.Location = new Point(293, 130);
      this.btnConditionRerequestedBy.MouseDownImage = (Image) null;
      this.btnConditionRerequestedBy.Name = "btnConditionRerequestedBy";
      this.btnConditionRerequestedBy.Size = new Size(16, 16);
      this.btnConditionRerequestedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnConditionRerequestedBy.TabIndex = 90;
      this.btnConditionRerequestedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnConditionRerequestedBy, "Select User");
      this.btnConditionRerequestedBy.Visible = false;
      this.btnConditionRerequestedBy.Click += new EventHandler(this.btnConditionRerequestedBy_Click);
      this.btnConditionRequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnConditionRequestedBy.BackColor = Color.Transparent;
      this.btnConditionRequestedBy.Location = new Point(293, 106);
      this.btnConditionRequestedBy.MouseDownImage = (Image) null;
      this.btnConditionRequestedBy.Name = "btnConditionRequestedBy";
      this.btnConditionRequestedBy.Size = new Size(16, 16);
      this.btnConditionRequestedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnConditionRequestedBy.TabIndex = 88;
      this.btnConditionRequestedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnConditionRequestedBy, "Select User");
      this.btnConditionRequestedBy.Visible = false;
      this.btnConditionRequestedBy.Click += new EventHandler(this.btnConditionRequestedBy_Click);
      this.btnFulfilledBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnFulfilledBy.BackColor = Color.Transparent;
      this.btnFulfilledBy.Location = new Point(293, 82);
      this.btnFulfilledBy.MouseDownImage = (Image) null;
      this.btnFulfilledBy.Name = "btnFulfilledBy";
      this.btnFulfilledBy.Size = new Size(16, 16);
      this.btnFulfilledBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnFulfilledBy.TabIndex = 73;
      this.btnFulfilledBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnFulfilledBy, "Select User");
      this.btnFulfilledBy.Click += new EventHandler(this.btnFulfilledBy_Click);
      this.btnEditCondition.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEditCondition.BackColor = Color.Transparent;
      this.btnEditCondition.Enabled = false;
      this.btnEditCondition.Location = new Point(84, 3);
      this.btnEditCondition.Margin = new Padding(4, 3, 0, 3);
      this.btnEditCondition.MouseDownImage = (Image) null;
      this.btnEditCondition.Name = "btnEditCondition";
      this.btnEditCondition.Size = new Size(16, 16);
      this.btnEditCondition.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditCondition.TabIndex = 13;
      this.btnEditCondition.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnEditCondition, "Edit Condition");
      this.btnEditCondition.Visible = false;
      this.btnAddCondition.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddCondition.BackColor = Color.Transparent;
      this.btnAddCondition.Location = new Point(64, 3);
      this.btnAddCondition.Margin = new Padding(4, 3, 0, 3);
      this.btnAddCondition.MouseDownImage = (Image) null;
      this.btnAddCondition.Name = "btnAddCondition";
      this.btnAddCondition.Size = new Size(16, 16);
      this.btnAddCondition.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddCondition.TabIndex = 12;
      this.btnAddCondition.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddCondition, "New Condition");
      this.btnAddCondition.Visible = false;
      this.btnAddCondition.Click += new EventHandler(this.btnAddCondition_Click);
      this.pnlLeft.Controls.Add((Control) this.gcConditions);
      this.pnlLeft.Controls.Add((Control) this.csDocuments);
      this.pnlLeft.Controls.Add((Control) this.gcDocuments);
      this.pnlLeft.Dock = DockStyle.Left;
      this.pnlLeft.Location = new Point(0, 0);
      this.pnlLeft.Name = "pnlLeft";
      this.pnlLeft.Size = new Size(301, 613);
      this.pnlLeft.TabIndex = 0;
      this.gcConditions.Controls.Add((Control) this.cboCondType);
      this.gcConditions.Controls.Add((Control) this.pnlToolbar);
      this.gcConditions.Controls.Add((Control) this.gvConditions);
      this.gcConditions.Dock = DockStyle.Fill;
      this.gcConditions.HeaderForeColor = SystemColors.ControlText;
      this.gcConditions.Location = new Point(0, 263);
      this.gcConditions.Name = "gcConditions";
      this.gcConditions.Size = new Size(301, 350);
      this.gcConditions.TabIndex = 6;
      this.gcConditions.Text = "Conditions";
      this.cboCondType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCondType.FormattingEnabled = true;
      this.cboCondType.Location = new Point(81, 2);
      this.cboCondType.Name = "cboCondType";
      this.cboCondType.Size = new Size(116, 22);
      this.cboCondType.TabIndex = 9;
      this.cboCondType.TabStop = false;
      this.cboCondType.SelectedIndexChanged += new EventHandler(this.cboCondType_SelectedIndexChanged);
      this.cboCondType.SelectionChangeCommitted += new EventHandler(this.cboCondType_SelectionChangeCommitted);
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnEditCondition);
      this.pnlToolbar.Controls.Add((Control) this.btnAddCondition);
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(197, 2);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(100, 22);
      this.pnlToolbar.TabIndex = 7;
      this.gvConditions.AllowDrop = true;
      this.gvConditions.AllowMultiselect = false;
      this.gvConditions.BorderStyle = BorderStyle.None;
      this.gvConditions.ClearSelectionsOnEmptyRowClick = false;
      this.gvConditions.Dock = DockStyle.Fill;
      this.gvConditions.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvConditions.ImageList = this.imageList;
      this.gvConditions.ItemGrouping = true;
      this.gvConditions.Location = new Point(1, 26);
      this.gvConditions.Name = "gvConditions";
      this.gvConditions.Size = new Size(299, 323);
      this.gvConditions.TabIndex = 8;
      this.gvConditions.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvConditions.SelectedIndexCommitted += new EventHandler(this.gvConditions_SelectedIndexCommitted);
      this.gvConditions.DragDrop += new DragEventHandler(this.gvConditions_DragDrop);
      this.gvConditions.DragEnter += new DragEventHandler(this.gvConditions_DragEnter);
      this.gvConditions.DragOver += new DragEventHandler(this.gvConditions_DragOver);
      this.gvConditions.MouseDown += new MouseEventHandler(this.gvConditions_MouseDown);
      this.gvConditions.MouseMove += new MouseEventHandler(this.gvConditions_MouseMove);
      this.gvConditions.MouseUp += new MouseEventHandler(this.gvConditions_MouseUp);
      this.csDocuments.AnimationDelay = 20;
      this.csDocuments.AnimationStep = 20;
      this.csDocuments.BorderStyle3D = Border3DStyle.Flat;
      this.csDocuments.ControlToHide = (Control) this.gcDocuments;
      this.csDocuments.Dock = DockStyle.Top;
      this.csDocuments.ExpandParentForm = false;
      this.csDocuments.Location = new Point(0, 256);
      this.csDocuments.Name = "csLeft";
      this.csDocuments.TabIndex = 5;
      this.csDocuments.TabStop = false;
      this.csDocuments.UseAnimations = false;
      this.csDocuments.VisualStyle = VisualStyles.Encompass;
      this.gcDocuments.Controls.Add((Control) this.pnlStackingOrder);
      this.gcDocuments.Controls.Add((Control) this.gvDocuments);
      this.gcDocuments.Controls.Add((Control) this.pnlDragDrop);
      this.gcDocuments.Dock = DockStyle.Top;
      this.gcDocuments.HeaderForeColor = SystemColors.ControlText;
      this.gcDocuments.Location = new Point(0, 0);
      this.gcDocuments.Name = "gcDocuments";
      this.gcDocuments.Size = new Size(301, 256);
      this.gcDocuments.TabIndex = 1;
      this.gcDocuments.Text = "Documents";
      this.pnlStackingOrder.Borders = AnchorStyles.Bottom;
      this.pnlStackingOrder.Controls.Add((Control) this.cboStackingOrder);
      this.pnlStackingOrder.Controls.Add((Control) this.label2);
      this.pnlStackingOrder.Dock = DockStyle.Top;
      this.pnlStackingOrder.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlStackingOrder.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlStackingOrder.Location = new Point(1, 26);
      this.pnlStackingOrder.Name = "pnlStackingOrder";
      this.pnlStackingOrder.Size = new Size(299, 25);
      this.pnlStackingOrder.Style = GradientPanel.PanelStyle.TableFooter;
      this.pnlStackingOrder.TabIndex = 4;
      this.cboStackingOrder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboStackingOrder.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboStackingOrder.FormattingEnabled = true;
      this.cboStackingOrder.Location = new Point(92, 1);
      this.cboStackingOrder.Name = "cboStackingOrder";
      this.cboStackingOrder.Size = new Size(200, 22);
      this.cboStackingOrder.TabIndex = 1;
      this.cboStackingOrder.DropDown += new EventHandler(this.cboStackingOrder_DropDown);
      this.cboStackingOrder.SelectedIndexChanged += new EventHandler(this.cboStackingOrder_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(7, 6);
      this.label2.Name = "label2";
      this.label2.Size = new Size(79, 14);
      this.label2.TabIndex = 0;
      this.label2.Text = "Stacking Order";
      this.gvDocuments.AllowDrop = true;
      this.gvDocuments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gvDocuments.BorderStyle = BorderStyle.None;
      this.gvDocuments.ClearSelectionsOnEmptyRowClick = false;
      this.gvDocuments.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDocuments.ImageList = this.imageList;
      this.gvDocuments.Location = new Point(1, 49);
      this.gvDocuments.Name = "gvDocuments";
      this.gvDocuments.Size = new Size(299, 179);
      this.gvDocuments.TabIndex = 2;
      this.gvDocuments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvDocuments.BeforeSelectedIndexCommitted += new CancelEventHandler(this.gvDocuments_BeforeSelectedIndexCommitted);
      this.gvDocuments.SelectedIndexChanged += new EventHandler(this.gvDocuments_SelectedIndexChanged);
      this.gvDocuments.SelectedIndexCommitted += new EventHandler(this.gvDocuments_SelectedIndexCommitted);
      this.gvDocuments.SortItems += new GVColumnSortEventHandler(this.gvDocuments_SortItems);
      this.gvDocuments.DragDrop += new DragEventHandler(this.gvDocuments_DragDrop);
      this.gvDocuments.DragEnter += new DragEventHandler(this.gvDocuments_DragEnter);
      this.gvDocuments.MouseDown += new MouseEventHandler(this.gvDocuments_MouseDown);
      this.gvDocuments.MouseMove += new MouseEventHandler(this.gvDocuments_MouseMove);
      this.gvDocuments.MouseUp += new MouseEventHandler(this.gvDocuments_MouseUp);
      this.pnlDragDrop.Borders = AnchorStyles.Top;
      this.pnlDragDrop.Controls.Add((Control) this.lblDragDrop);
      this.pnlDragDrop.Dock = DockStyle.Bottom;
      this.pnlDragDrop.Location = new Point(1, 228);
      this.pnlDragDrop.Name = "pnlDragDrop";
      this.pnlDragDrop.Size = new Size(299, 27);
      this.pnlDragDrop.TabIndex = 3;
      this.lblDragDrop.BackColor = Color.WhiteSmoke;
      this.lblDragDrop.Dock = DockStyle.Fill;
      this.lblDragDrop.Location = new Point(0, 1);
      this.lblDragDrop.Name = "lblDragDrop";
      this.lblDragDrop.Size = new Size(299, 26);
      this.lblDragDrop.TabIndex = 4;
      this.lblDragDrop.Text = "Drag a document and drop in a condition below";
      this.lblDragDrop.TextAlign = ContentAlignment.MiddleCenter;
      this.btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.OK;
      this.btnClose.Location = new Point(1185, 9);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 22);
      this.btnClose.TabIndex = 13;
      this.btnClose.TabStop = false;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.pnlClose.Controls.Add((Control) this.helpLink);
      this.pnlClose.Controls.Add((Control) this.btnClose);
      this.pnlClose.Dock = DockStyle.Bottom;
      this.pnlClose.Location = new Point(0, 613);
      this.pnlClose.Name = "pnlClose";
      this.pnlClose.Size = new Size(1268, 40);
      this.pnlClose.TabIndex = 12;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "Document Manager";
      this.helpLink.Location = new Point(8, 11);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 18);
      this.helpLink.TabIndex = 14;
      this.helpLink.TabStop = false;
      this.pnlRight.Controls.Add((Control) this.pnlViewer);
      this.pnlRight.Controls.Add((Control) this.gcInfo);
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(308, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(960, 613);
      this.pnlRight.TabIndex = 13;
      this.pnlViewer.Controls.Add((Control) this.fileViewer);
      this.pnlViewer.Dock = DockStyle.Fill;
      this.pnlViewer.Location = new Point(0, 383);
      this.pnlViewer.Name = "pnlViewer";
      this.pnlViewer.Size = new Size(960, 230);
      this.pnlViewer.TabIndex = 2;
      this.fileViewer.Dock = DockStyle.Fill;
      this.fileViewer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.fileViewer.Location = new Point(1, 1);
      this.fileViewer.Name = "fileViewer";
      this.fileViewer.Size = new Size(958, 228);
      this.fileViewer.TabIndex = 0;
      this.gcInfo.Controls.Add((Control) this.tabInfo);
      this.gcInfo.Dock = DockStyle.Top;
      this.gcInfo.HeaderForeColor = SystemColors.ControlText;
      this.gcInfo.Location = new Point(0, 0);
      this.gcInfo.Name = "gcInfo";
      this.gcInfo.Size = new Size(960, 383);
      this.gcInfo.TabIndex = 0;
      this.tabInfo.Controls.Add((Control) this.pageFiles);
      this.tabInfo.Controls.Add((Control) this.pageDocumentDetails);
      this.tabInfo.Controls.Add((Control) this.pageConditionDetails);
      this.tabInfo.Controls.Add((Control) this.pageConditionsEnhanced);
      this.tabInfo.Dock = DockStyle.Top;
      this.tabInfo.Location = new Point(1, 26);
      this.tabInfo.Name = "tabInfo";
      this.tabInfo.SelectedIndex = 0;
      this.tabInfo.Size = new Size(958, 357);
      this.tabInfo.TabIndex = 1;
      this.pageFiles.AutoScroll = true;
      this.pageFiles.AutoScrollMargin = new Size(8, 8);
      this.pageFiles.BackColor = Color.WhiteSmoke;
      this.pageFiles.Controls.Add((Control) this.gvFiles);
      this.pageFiles.Location = new Point(4, 23);
      this.pageFiles.Name = "pageFiles";
      this.pageFiles.Padding = new Padding(0, 2, 2, 2);
      this.pageFiles.Size = new Size(950, 330);
      this.pageFiles.TabIndex = 0;
      this.pageFiles.Text = "Files";
      this.pageFiles.UseVisualStyleBackColor = true;
      this.gvFiles.AllowDrop = true;
      this.gvFiles.BorderStyle = BorderStyle.None;
      this.gvFiles.ClearSelectionsOnEmptyRowClick = false;
      this.gvFiles.Dock = DockStyle.Fill;
      this.gvFiles.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvFiles.Location = new Point(0, 2);
      this.gvFiles.Name = "gvFiles";
      this.gvFiles.Size = new Size(948, 326);
      this.gvFiles.SortOption = GVSortOption.None;
      this.gvFiles.TabIndex = 2;
      this.gvFiles.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvFiles.UseCompatibleEditingBehavior = true;
      this.gvFiles.BeforeSelectedIndexCommitted += new CancelEventHandler(this.gvFiles_BeforeSelectedIndexCommitted);
      this.gvFiles.SelectedIndexCommitted += new EventHandler(this.gvFiles_SelectedIndexCommitted);
      this.pageDocumentDetails.BackColor = Color.WhiteSmoke;
      this.pageDocumentDetails.Controls.Add((Control) this.gcDocumentCommentText);
      this.pageDocumentDetails.Controls.Add((Control) this.gcDocumentComments);
      this.pageDocumentDetails.Controls.Add((Control) this.gcDocumentStatus);
      this.pageDocumentDetails.Controls.Add((Control) this.gcDocumentDetails);
      this.pageDocumentDetails.Location = new Point(4, 23);
      this.pageDocumentDetails.Name = "pageDocumentDetails";
      this.pageDocumentDetails.Padding = new Padding(0, 2, 2, 2);
      this.pageDocumentDetails.Size = new Size(950, 330);
      this.pageDocumentDetails.TabIndex = 1;
      this.pageDocumentDetails.Text = "Document Details";
      this.pageDocumentDetails.UseVisualStyleBackColor = true;
      this.gcDocumentCommentText.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcDocumentCommentText.Controls.Add((Control) this.txtDocumentComment);
      this.gcDocumentCommentText.Dock = DockStyle.Bottom;
      this.gcDocumentCommentText.HeaderForeColor = SystemColors.ControlText;
      this.gcDocumentCommentText.Location = new Point(645, 247);
      this.gcDocumentCommentText.Name = "gcDocumentCommentText";
      this.gcDocumentCommentText.Size = new Size(303, 81);
      this.gcDocumentCommentText.TabIndex = 5;
      this.gcDocumentCommentText.Text = "Text";
      this.txtDocumentComment.Dock = DockStyle.Fill;
      this.txtDocumentComment.Location = new Point(1, 25);
      this.txtDocumentComment.Multiline = true;
      this.txtDocumentComment.Name = "txtDocumentComment";
      this.txtDocumentComment.ReadOnly = true;
      this.txtDocumentComment.ScrollBars = ScrollBars.Vertical;
      this.txtDocumentComment.Size = new Size(301, 55);
      this.txtDocumentComment.TabIndex = 21;
      this.gcDocumentComments.Controls.Add((Control) this.flowLayoutPanel1);
      this.gcDocumentComments.Controls.Add((Control) this.gvDocumentComments);
      this.gcDocumentComments.Dock = DockStyle.Top;
      this.gcDocumentComments.HeaderForeColor = SystemColors.ControlText;
      this.gcDocumentComments.Location = new Point(645, 2);
      this.gcDocumentComments.Name = "gcDocumentComments";
      this.gcDocumentComments.Size = new Size(303, 243);
      this.gcDocumentComments.TabIndex = 3;
      this.gcDocumentComments.Text = "Comments";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnDeleteDocumentComment);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnViewDocumentComment);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAddDocumentComment);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(218, 3);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(80, 22);
      this.flowLayoutPanel1.TabIndex = 10;
      this.gvDocumentComments.AllowDrop = true;
      this.gvDocumentComments.AllowMultiselect = false;
      this.gvDocumentComments.BorderStyle = BorderStyle.None;
      this.gvDocumentComments.ClearSelectionsOnEmptyRowClick = false;
      this.gvDocumentComments.Dock = DockStyle.Fill;
      this.gvDocumentComments.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDocumentComments.ImageList = this.imageList;
      this.gvDocumentComments.ItemGrouping = true;
      this.gvDocumentComments.Location = new Point(1, 26);
      this.gvDocumentComments.Name = "gvDocumentComments";
      this.gvDocumentComments.Size = new Size(301, 216);
      this.gvDocumentComments.TabIndex = 9;
      this.gvDocumentComments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvDocumentComments.SelectedIndexCommitted += new EventHandler(this.gvDocumentComments_SelectedIndexCommitted);
      this.gvDocumentComments.DoubleClick += new EventHandler(this.btnViewDocumentComment_Click);
      this.gcDocumentStatus.Controls.Add((Control) this.pnlStatus);
      this.gcDocumentStatus.Dock = DockStyle.Left;
      this.gcDocumentStatus.HeaderForeColor = SystemColors.ControlText;
      this.gcDocumentStatus.Location = new Point(328, 2);
      this.gcDocumentStatus.Name = "gcDocumentStatus";
      this.gcDocumentStatus.Size = new Size(317, 326);
      this.gcDocumentStatus.TabIndex = 2;
      this.gcDocumentStatus.Text = "Status";
      this.pnlStatus.AutoScroll = true;
      this.pnlStatus.AutoScrollMargin = new Size(8, 8);
      this.pnlStatus.Controls.Add((Control) this.btnDateShippingReady);
      this.pnlStatus.Controls.Add((Control) this.btnDateUnderwritingReady);
      this.pnlStatus.Controls.Add((Control) this.btnDocumentDateReviewed);
      this.pnlStatus.Controls.Add((Control) this.btnDocumentDateReceived);
      this.pnlStatus.Controls.Add((Control) this.btnDocumentDateRerequested);
      this.pnlStatus.Controls.Add((Control) this.btnDocumentDateRequested);
      this.pnlStatus.Controls.Add((Control) this.txtDateShippingReady);
      this.pnlStatus.Controls.Add((Control) this.txtDateUnderwritingReady);
      this.pnlStatus.Controls.Add((Control) this.txtDocumentDateReviewed);
      this.pnlStatus.Controls.Add((Control) this.txtDocumentDateReceived);
      this.pnlStatus.Controls.Add((Control) this.txtDocumentDateRerequested);
      this.pnlStatus.Controls.Add((Control) this.txtDocumentDateRequested);
      this.pnlStatus.Controls.Add((Control) this.btnShippingReadyBy);
      this.pnlStatus.Controls.Add((Control) this.txtShippingReadyBy);
      this.pnlStatus.Controls.Add((Control) this.chkShippingReady);
      this.pnlStatus.Controls.Add((Control) this.btnUnderwritingReadyBy);
      this.pnlStatus.Controls.Add((Control) this.txtUnderwritingReadyBy);
      this.pnlStatus.Controls.Add((Control) this.chkUnderwritingReady);
      this.pnlStatus.Controls.Add((Control) this.btnDocumentReviewedBy);
      this.pnlStatus.Controls.Add((Control) this.btnDocumentReceivedBy);
      this.pnlStatus.Controls.Add((Control) this.btnDocumentRerequestedBy);
      this.pnlStatus.Controls.Add((Control) this.btnDocumentRequestedBy);
      this.pnlStatus.Controls.Add((Control) this.txtDocumentReviewedBy);
      this.pnlStatus.Controls.Add((Control) this.txtDocumentReceivedBy);
      this.pnlStatus.Controls.Add((Control) this.txtDocumentRerequestedBy);
      this.pnlStatus.Controls.Add((Control) this.txtDocumentRequestedBy);
      this.pnlStatus.Controls.Add((Control) this.chkDocumentReviewed);
      this.pnlStatus.Controls.Add((Control) this.chkDocumentReceived);
      this.pnlStatus.Controls.Add((Control) this.chkDocumentRerequested);
      this.pnlStatus.Controls.Add((Control) this.chkDocumentRequested);
      this.pnlStatus.Controls.Add((Control) this.txtDocumentRequestedFrom);
      this.pnlStatus.Controls.Add((Control) this.lblCompany);
      this.pnlStatus.Controls.Add((Control) this.txtDateExpire);
      this.pnlStatus.Controls.Add((Control) this.txtDaysExpire);
      this.pnlStatus.Controls.Add((Control) this.lblDaysExpire);
      this.pnlStatus.Controls.Add((Control) this.txtDocumentDateDue);
      this.pnlStatus.Controls.Add((Control) this.txtDocumentDaysDue);
      this.pnlStatus.Controls.Add((Control) this.lblDaysDue);
      this.pnlStatus.Dock = DockStyle.Fill;
      this.pnlStatus.Location = new Point(1, 26);
      this.pnlStatus.Name = "pnlStatus";
      this.pnlStatus.Size = new Size(315, 299);
      this.pnlStatus.TabIndex = 0;
      this.btnDateShippingReady.DateControl = (Control) this.txtDateShippingReady;
      ((IconButton) this.btnDateShippingReady).Image = (Image) componentResourceManager.GetObject("btnDateShippingReady.Image");
      this.btnDateShippingReady.Location = new Point(163, 194);
      this.btnDateShippingReady.MouseDownImage = (Image) null;
      this.btnDateShippingReady.Name = "btnDateShippingReady";
      this.btnDateShippingReady.Size = new Size(16, 16);
      this.btnDateShippingReady.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnDateShippingReady.TabIndex = 162;
      this.btnDateShippingReady.TabStop = false;
      this.btnDateShippingReady.Visible = false;
      this.txtDateShippingReady.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateShippingReady.Location = new Point(101, 194);
      this.txtDateShippingReady.Name = "txtDateShippingReady";
      this.txtDateShippingReady.ReadOnly = true;
      this.txtDateShippingReady.Size = new Size(56, 20);
      this.txtDateShippingReady.TabIndex = 102;
      this.txtDateShippingReady.Visible = false;
      this.txtDateShippingReady.TextChanged += new EventHandler(this.txtDateShippingReady_TextChanged);
      this.btnDateUnderwritingReady.DateControl = (Control) this.txtDateUnderwritingReady;
      ((IconButton) this.btnDateUnderwritingReady).Image = (Image) componentResourceManager.GetObject("btnDateUnderwritingReady.Image");
      this.btnDateUnderwritingReady.Location = new Point(163, 171);
      this.btnDateUnderwritingReady.MouseDownImage = (Image) null;
      this.btnDateUnderwritingReady.Name = "btnDateUnderwritingReady";
      this.btnDateUnderwritingReady.Size = new Size(16, 16);
      this.btnDateUnderwritingReady.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnDateUnderwritingReady.TabIndex = 161;
      this.btnDateUnderwritingReady.TabStop = false;
      this.btnDateUnderwritingReady.Visible = false;
      this.txtDateUnderwritingReady.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateUnderwritingReady.Location = new Point(101, 172);
      this.txtDateUnderwritingReady.Name = "txtDateUnderwritingReady";
      this.txtDateUnderwritingReady.ReadOnly = true;
      this.txtDateUnderwritingReady.Size = new Size(56, 20);
      this.txtDateUnderwritingReady.TabIndex = 101;
      this.txtDateUnderwritingReady.Visible = false;
      this.txtDateUnderwritingReady.TextChanged += new EventHandler(this.txtDateUnderwritingReady_TextChanged);
      this.btnDocumentDateReviewed.DateControl = (Control) this.txtDocumentDateReviewed;
      ((IconButton) this.btnDocumentDateReviewed).Image = (Image) componentResourceManager.GetObject("btnDocumentDateReviewed.Image");
      this.btnDocumentDateReviewed.Location = new Point(163, 148);
      this.btnDocumentDateReviewed.MouseDownImage = (Image) null;
      this.btnDocumentDateReviewed.Name = "btnDocumentDateReviewed";
      this.btnDocumentDateReviewed.Size = new Size(16, 16);
      this.btnDocumentDateReviewed.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnDocumentDateReviewed.TabIndex = 160;
      this.btnDocumentDateReviewed.TabStop = false;
      this.btnDocumentDateReviewed.Visible = false;
      this.txtDocumentDateReviewed.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDocumentDateReviewed.Location = new Point(101, 149);
      this.txtDocumentDateReviewed.Name = "txtDocumentDateReviewed";
      this.txtDocumentDateReviewed.ReadOnly = true;
      this.txtDocumentDateReviewed.Size = new Size(56, 20);
      this.txtDocumentDateReviewed.TabIndex = 100;
      this.txtDocumentDateReviewed.Visible = false;
      this.txtDocumentDateReviewed.TextChanged += new EventHandler(this.txtDocumentDateReviewed_TextChanged);
      this.btnDocumentDateReceived.DateControl = (Control) this.txtDocumentDateReceived;
      ((IconButton) this.btnDocumentDateReceived).Image = (Image) componentResourceManager.GetObject("btnDocumentDateReceived.Image");
      this.btnDocumentDateReceived.Location = new Point(163, 125);
      this.btnDocumentDateReceived.MouseDownImage = (Image) null;
      this.btnDocumentDateReceived.Name = "btnDocumentDateReceived";
      this.btnDocumentDateReceived.Size = new Size(16, 16);
      this.btnDocumentDateReceived.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnDocumentDateReceived.TabIndex = 159;
      this.btnDocumentDateReceived.TabStop = false;
      this.btnDocumentDateReceived.Visible = false;
      this.txtDocumentDateReceived.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDocumentDateReceived.Location = new Point(101, 125);
      this.txtDocumentDateReceived.Name = "txtDocumentDateReceived";
      this.txtDocumentDateReceived.ReadOnly = true;
      this.txtDocumentDateReceived.Size = new Size(56, 20);
      this.txtDocumentDateReceived.TabIndex = 99;
      this.txtDocumentDateReceived.Visible = false;
      this.txtDocumentDateReceived.TextChanged += new EventHandler(this.txtDocumentDateReceived_TextChanged);
      this.btnDocumentDateRerequested.DateControl = (Control) this.txtDocumentDateRerequested;
      ((IconButton) this.btnDocumentDateRerequested).Image = (Image) componentResourceManager.GetObject("btnDocumentDateRerequested.Image");
      this.btnDocumentDateRerequested.Location = new Point(163, 101);
      this.btnDocumentDateRerequested.MouseDownImage = (Image) null;
      this.btnDocumentDateRerequested.Name = "btnDocumentDateRerequested";
      this.btnDocumentDateRerequested.Size = new Size(16, 16);
      this.btnDocumentDateRerequested.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnDocumentDateRerequested.TabIndex = 158;
      this.btnDocumentDateRerequested.TabStop = false;
      this.btnDocumentDateRerequested.Visible = false;
      this.txtDocumentDateRerequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDocumentDateRerequested.Location = new Point(101, 102);
      this.txtDocumentDateRerequested.Name = "txtDocumentDateRerequested";
      this.txtDocumentDateRerequested.ReadOnly = true;
      this.txtDocumentDateRerequested.Size = new Size(56, 20);
      this.txtDocumentDateRerequested.TabIndex = 98;
      this.txtDocumentDateRerequested.Visible = false;
      this.txtDocumentDateRerequested.TextChanged += new EventHandler(this.txtDocumentDateRerequested_TextChanged);
      this.btnDocumentDateRequested.DateControl = (Control) this.txtDocumentDateRequested;
      ((IconButton) this.btnDocumentDateRequested).Image = (Image) componentResourceManager.GetObject("btnDocumentDateRequested.Image");
      this.btnDocumentDateRequested.Location = new Point(163, 78);
      this.btnDocumentDateRequested.MouseDownImage = (Image) null;
      this.btnDocumentDateRequested.Name = "btnDocumentDateRequested";
      this.btnDocumentDateRequested.Size = new Size(16, 16);
      this.btnDocumentDateRequested.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnDocumentDateRequested.TabIndex = 157;
      this.btnDocumentDateRequested.TabStop = false;
      this.btnDocumentDateRequested.Visible = false;
      this.txtDocumentDateRequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDocumentDateRequested.Location = new Point(101, 76);
      this.txtDocumentDateRequested.Name = "txtDocumentDateRequested";
      this.txtDocumentDateRequested.ReadOnly = true;
      this.txtDocumentDateRequested.Size = new Size(56, 20);
      this.txtDocumentDateRequested.TabIndex = 97;
      this.txtDocumentDateRequested.Visible = false;
      this.txtDocumentDateRequested.TextChanged += new EventHandler(this.txtDocumentDateRequested_TextChanged);
      this.txtShippingReadyBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtShippingReadyBy.Location = new Point(211, 197);
      this.txtShippingReadyBy.Name = "txtShippingReadyBy";
      this.txtShippingReadyBy.ReadOnly = true;
      this.txtShippingReadyBy.Size = new Size(59, 20);
      this.txtShippingReadyBy.TabIndex = 90;
      this.txtShippingReadyBy.Visible = false;
      this.chkShippingReady.AutoSize = true;
      this.chkShippingReady.Location = new Point(10, 199);
      this.chkShippingReady.Name = "chkShippingReady";
      this.chkShippingReady.Size = new Size(93, 18);
      this.chkShippingReady.TabIndex = 88;
      this.chkShippingReady.Text = "Ready to Ship";
      this.chkShippingReady.UseVisualStyleBackColor = true;
      this.chkShippingReady.Click += new EventHandler(this.chkShippingReady_Click);
      this.txtUnderwritingReadyBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtUnderwritingReadyBy.Location = new Point(211, 173);
      this.txtUnderwritingReadyBy.Name = "txtUnderwritingReadyBy";
      this.txtUnderwritingReadyBy.ReadOnly = true;
      this.txtUnderwritingReadyBy.Size = new Size(59, 20);
      this.txtUnderwritingReadyBy.TabIndex = 87;
      this.txtUnderwritingReadyBy.Visible = false;
      this.chkUnderwritingReady.AutoSize = true;
      this.chkUnderwritingReady.Location = new Point(10, 175);
      this.chkUnderwritingReady.Name = "chkUnderwritingReady";
      this.chkUnderwritingReady.Size = new Size(94, 18);
      this.chkUnderwritingReady.TabIndex = 85;
      this.chkUnderwritingReady.Text = "Ready for UW";
      this.chkUnderwritingReady.UseVisualStyleBackColor = true;
      this.chkUnderwritingReady.Click += new EventHandler(this.chkUnderwritingReady_Click);
      this.txtDocumentReviewedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtDocumentReviewedBy.Location = new Point(211, 149);
      this.txtDocumentReviewedBy.Name = "txtDocumentReviewedBy";
      this.txtDocumentReviewedBy.ReadOnly = true;
      this.txtDocumentReviewedBy.Size = new Size(59, 20);
      this.txtDocumentReviewedBy.TabIndex = 84;
      this.txtDocumentReviewedBy.Visible = false;
      this.txtDocumentReceivedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtDocumentReceivedBy.Location = new Point(211, 125);
      this.txtDocumentReceivedBy.Name = "txtDocumentReceivedBy";
      this.txtDocumentReceivedBy.ReadOnly = true;
      this.txtDocumentReceivedBy.Size = new Size(59, 20);
      this.txtDocumentReceivedBy.TabIndex = 81;
      this.txtDocumentReceivedBy.Visible = false;
      this.txtDocumentRerequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtDocumentRerequestedBy.Location = new Point(211, 101);
      this.txtDocumentRerequestedBy.Name = "txtDocumentRerequestedBy";
      this.txtDocumentRerequestedBy.ReadOnly = true;
      this.txtDocumentRerequestedBy.Size = new Size(59, 20);
      this.txtDocumentRerequestedBy.TabIndex = 78;
      this.txtDocumentRerequestedBy.Visible = false;
      this.txtDocumentRequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtDocumentRequestedBy.Location = new Point(211, 77);
      this.txtDocumentRequestedBy.Name = "txtDocumentRequestedBy";
      this.txtDocumentRequestedBy.ReadOnly = true;
      this.txtDocumentRequestedBy.Size = new Size(59, 20);
      this.txtDocumentRequestedBy.TabIndex = 75;
      this.txtDocumentRequestedBy.Visible = false;
      this.chkDocumentReviewed.AutoSize = true;
      this.chkDocumentReviewed.Location = new Point(10, 151);
      this.chkDocumentReviewed.Name = "chkDocumentReviewed";
      this.chkDocumentReviewed.Size = new Size(75, 18);
      this.chkDocumentReviewed.TabIndex = 82;
      this.chkDocumentReviewed.Text = "Reviewed";
      this.chkDocumentReviewed.UseVisualStyleBackColor = true;
      this.chkDocumentReviewed.Click += new EventHandler(this.chkDocumentReviewed_Click);
      this.chkDocumentReceived.AutoSize = true;
      this.chkDocumentReceived.Location = new Point(10, (int) sbyte.MaxValue);
      this.chkDocumentReceived.Name = "chkDocumentReceived";
      this.chkDocumentReceived.Size = new Size(71, 18);
      this.chkDocumentReceived.TabIndex = 79;
      this.chkDocumentReceived.Text = "Received";
      this.chkDocumentReceived.UseVisualStyleBackColor = true;
      this.chkDocumentReceived.Click += new EventHandler(this.chkDocumentReceived_Click);
      this.chkDocumentRerequested.AutoSize = true;
      this.chkDocumentRerequested.Location = new Point(10, 103);
      this.chkDocumentRerequested.Name = "chkDocumentRerequested";
      this.chkDocumentRerequested.Size = new Size(92, 18);
      this.chkDocumentRerequested.TabIndex = 76;
      this.chkDocumentRerequested.Text = "Re-requested";
      this.chkDocumentRerequested.UseVisualStyleBackColor = true;
      this.chkDocumentRerequested.Click += new EventHandler(this.chkDocumentRerequested_Click);
      this.chkDocumentRequested.AutoSize = true;
      this.chkDocumentRequested.Location = new Point(10, 79);
      this.chkDocumentRequested.Name = "chkDocumentRequested";
      this.chkDocumentRequested.Size = new Size(78, 18);
      this.chkDocumentRequested.TabIndex = 73;
      this.chkDocumentRequested.Text = "Requested";
      this.chkDocumentRequested.UseVisualStyleBackColor = true;
      this.chkDocumentRequested.Click += new EventHandler(this.chkDocumentRequested_Click);
      this.txtDocumentRequestedFrom.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDocumentRequestedFrom.Location = new Point(99, 52);
      this.txtDocumentRequestedFrom.Name = "txtDocumentRequestedFrom";
      this.txtDocumentRequestedFrom.Size = new Size(190, 20);
      this.txtDocumentRequestedFrom.TabIndex = 72;
      this.txtDocumentRequestedFrom.Validated += new EventHandler(this.txtDocumentRequestedFrom_Validated);
      this.lblCompany.AutoSize = true;
      this.lblCompany.Location = new Point(7, 55);
      this.lblCompany.Name = "lblCompany";
      this.lblCompany.Size = new Size(86, 14);
      this.lblCompany.TabIndex = 71;
      this.lblCompany.Text = "Requested From";
      this.txtDateExpire.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateExpire.Location = new Point(151, 28);
      this.txtDateExpire.Name = "txtDateExpire";
      this.txtDateExpire.ReadOnly = true;
      this.txtDateExpire.Size = new Size(138, 20);
      this.txtDateExpire.TabIndex = 70;
      this.txtDaysExpire.Location = new Point(99, 28);
      this.txtDaysExpire.Name = "txtDaysExpire";
      this.txtDaysExpire.Size = new Size(48, 20);
      this.txtDaysExpire.TabIndex = 69;
      this.txtDaysExpire.TextAlign = HorizontalAlignment.Right;
      this.txtDaysExpire.Validating += new CancelEventHandler(this.txtDaysExpire_Validating);
      this.txtDaysExpire.Validated += new EventHandler(this.txtDaysExpire_Validated);
      this.lblDaysExpire.AutoSize = true;
      this.lblDaysExpire.Location = new Point(7, 31);
      this.lblDaysExpire.Name = "lblDaysExpire";
      this.lblDaysExpire.Size = new Size(77, 14);
      this.lblDaysExpire.TabIndex = 68;
      this.lblDaysExpire.Text = "Days to Expire";
      this.txtDocumentDateDue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDocumentDateDue.Location = new Point(151, 4);
      this.txtDocumentDateDue.Name = "txtDocumentDateDue";
      this.txtDocumentDateDue.ReadOnly = true;
      this.txtDocumentDateDue.Size = new Size(138, 20);
      this.txtDocumentDateDue.TabIndex = 67;
      this.txtDocumentDaysDue.Location = new Point(99, 4);
      this.txtDocumentDaysDue.Name = "txtDocumentDaysDue";
      this.txtDocumentDaysDue.Size = new Size(48, 20);
      this.txtDocumentDaysDue.TabIndex = 66;
      this.txtDocumentDaysDue.TextAlign = HorizontalAlignment.Right;
      this.txtDocumentDaysDue.Validating += new CancelEventHandler(this.txtDocumentDaysDue_Validating);
      this.txtDocumentDaysDue.Validated += new EventHandler(this.txtDocumentDaysDue_Validated);
      this.lblDaysDue.AutoSize = true;
      this.lblDaysDue.Location = new Point(7, 7);
      this.lblDaysDue.Name = "lblDaysDue";
      this.lblDaysDue.Size = new Size(86, 14);
      this.lblDaysDue.TabIndex = 65;
      this.lblDaysDue.Text = "Days to Receive";
      this.gcDocumentDetails.Controls.Add((Control) this.pnlDetails);
      this.gcDocumentDetails.Dock = DockStyle.Left;
      this.gcDocumentDetails.HeaderForeColor = SystemColors.ControlText;
      this.gcDocumentDetails.Location = new Point(0, 2);
      this.gcDocumentDetails.Name = "gcDocumentDetails";
      this.gcDocumentDetails.Size = new Size(328, 326);
      this.gcDocumentDetails.TabIndex = 1;
      this.gcDocumentDetails.Text = "Details";
      this.pnlDetails.AutoScroll = true;
      this.pnlDetails.AutoScrollMargin = new Size(8, 8);
      this.pnlDetails.Controls.Add((Control) this.label3);
      this.pnlDetails.Controls.Add((Control) this.lstATRQM);
      this.pnlDetails.Controls.Add((Control) this.lstDocuments);
      this.pnlDetails.Controls.Add((Control) this.chkThirdParty);
      this.pnlDetails.Controls.Add((Control) this.chkTPOWebcenterPortal);
      this.pnlDetails.Controls.Add((Control) this.chkWebCenter);
      this.pnlDetails.Controls.Add((Control) this.lblDocuments);
      this.pnlDetails.Controls.Add((Control) this.lblAvailable);
      this.pnlDetails.Controls.Add((Control) this.lstGroups);
      this.pnlDetails.Controls.Add((Control) this.lblGroups);
      this.pnlDetails.Controls.Add((Control) this.lstDocumentConditions);
      this.pnlDetails.Controls.Add((Control) this.lblConditions);
      this.pnlDetails.Controls.Add((Control) this.txtAccess);
      this.pnlDetails.Controls.Add((Control) this.btnAccess);
      this.pnlDetails.Controls.Add((Control) this.lblAccess);
      this.pnlDetails.Controls.Add((Control) this.cboMilestone);
      this.pnlDetails.Controls.Add((Control) this.txtMilestone);
      this.pnlDetails.Controls.Add((Control) this.lblMilestone);
      this.pnlDetails.Controls.Add((Control) this.cboDocumentBorrower);
      this.pnlDetails.Controls.Add((Control) this.txtDocumentBorrower);
      this.pnlDetails.Controls.Add((Control) this.lblBorrower);
      this.pnlDetails.Dock = DockStyle.Fill;
      this.pnlDetails.Location = new Point(1, 26);
      this.pnlDetails.Name = "pnlDetails";
      this.pnlDetails.Size = new Size(326, 299);
      this.pnlDetails.TabIndex = 0;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(15, 206);
      this.label3.Name = "label3";
      this.label3.Size = new Size(46, 14);
      this.label3.TabIndex = 32;
      this.label3.Text = "ATR/QM";
      this.lstATRQM.BackColor = SystemColors.Control;
      this.lstATRQM.FormattingEnabled = true;
      this.lstATRQM.IntegralHeight = false;
      this.lstATRQM.ItemHeight = 14;
      this.lstATRQM.Location = new Point(84, 206);
      this.lstATRQM.Name = "lstATRQM";
      this.lstATRQM.SelectionMode = SelectionMode.None;
      this.lstATRQM.Size = new Size(214, 48);
      this.lstATRQM.Sorted = true;
      this.lstATRQM.TabIndex = 31;
      this.lstATRQM.TabStop = false;
      this.lstDocuments.BackColor = SystemColors.Control;
      this.lstDocuments.FormattingEnabled = true;
      this.lstDocuments.IntegralHeight = false;
      this.lstDocuments.ItemHeight = 14;
      this.lstDocuments.Location = new Point(84, 7);
      this.lstDocuments.Name = "lstDocuments";
      this.lstDocuments.SelectionMode = SelectionMode.None;
      this.lstDocuments.Size = new Size(214, 48);
      this.lstDocuments.Sorted = true;
      this.lstDocuments.TabIndex = 30;
      this.lstDocuments.TabStop = false;
      this.chkThirdParty.AutoSize = true;
      this.chkThirdParty.Location = new Point(208, 312);
      this.chkThirdParty.Name = "chkThirdParty";
      this.chkThirdParty.Size = new Size(90, 18);
      this.chkThirdParty.TabIndex = 29;
      this.chkThirdParty.Text = "EDM Lenders";
      this.chkThirdParty.Click += new EventHandler(this.chkThirdParty_Click);
      this.chkTPOWebcenterPortal.AutoSize = true;
      this.chkTPOWebcenterPortal.Location = new Point(162, 312);
      this.chkTPOWebcenterPortal.Name = "chkTPOWebcenterPortal";
      this.chkTPOWebcenterPortal.Size = new Size(46, 18);
      this.chkTPOWebcenterPortal.TabIndex = 0;
      this.chkTPOWebcenterPortal.Text = "TPO";
      this.chkTPOWebcenterPortal.Click += new EventHandler(this.chkTPOWebcenterPortal_Click);
      this.chkWebCenter.AutoSize = true;
      this.chkWebCenter.Location = new Point(84, 312);
      this.chkWebCenter.Name = "chkWebCenter";
      this.chkWebCenter.Size = new Size(80, 18);
      this.chkWebCenter.TabIndex = 27;
      this.chkWebCenter.Text = "WebCenter";
      this.chkWebCenter.Click += new EventHandler(this.chkWebCenter_Click);
      this.lblDocuments.AutoSize = true;
      this.lblDocuments.Location = new Point(7, 7);
      this.lblDocuments.Name = "lblDocuments";
      this.lblDocuments.Size = new Size(61, 14);
      this.lblDocuments.TabIndex = 19;
      this.lblDocuments.Text = "Documents";
      this.lblAvailable.AutoSize = true;
      this.lblAvailable.Location = new Point(7, 312);
      this.lblAvailable.Name = "lblAvailable";
      this.lblAvailable.Size = new Size(51, 14);
      this.lblAvailable.TabIndex = 17;
      this.lblAvailable.Text = "Available";
      this.lstGroups.BackColor = SystemColors.Control;
      this.lstGroups.FormattingEnabled = true;
      this.lstGroups.IntegralHeight = false;
      this.lstGroups.ItemHeight = 14;
      this.lstGroups.Location = new Point(84, 258);
      this.lstGroups.Name = "lstGroups";
      this.lstGroups.SelectionMode = SelectionMode.None;
      this.lstGroups.Size = new Size(214, 48);
      this.lstGroups.Sorted = true;
      this.lstGroups.TabIndex = 16;
      this.lstGroups.TabStop = false;
      this.lblGroups.AutoSize = true;
      this.lblGroups.Location = new Point(7, 258);
      this.lblGroups.Name = "lblGroups";
      this.lblGroups.Size = new Size(65, 14);
      this.lblGroups.TabIndex = 15;
      this.lblGroups.Text = "Doc Groups";
      this.lstDocumentConditions.BackColor = SystemColors.Control;
      this.lstDocumentConditions.FormattingEnabled = true;
      this.lstDocumentConditions.IntegralHeight = false;
      this.lstDocumentConditions.ItemHeight = 14;
      this.lstDocumentConditions.Location = new Point(84, 156);
      this.lstDocumentConditions.Name = "lstDocumentConditions";
      this.lstDocumentConditions.SelectionMode = SelectionMode.None;
      this.lstDocumentConditions.Size = new Size(214, 47);
      this.lstDocumentConditions.Sorted = true;
      this.lstDocumentConditions.TabIndex = 14;
      this.lstDocumentConditions.TabStop = false;
      this.lblConditions.AutoSize = true;
      this.lblConditions.Location = new Point(13, 159);
      this.lblConditions.Name = "lblConditions";
      this.lblConditions.Size = new Size(57, 14);
      this.lblConditions.TabIndex = 13;
      this.lblConditions.Text = "Conditions";
      this.txtAccess.Location = new Point(82, 124);
      this.txtAccess.Name = "txtAccess";
      this.txtAccess.ReadOnly = true;
      this.txtAccess.Size = new Size(216, 20);
      this.txtAccess.TabIndex = 11;
      this.btnAccess.Location = new Point(8, 123);
      this.btnAccess.Name = "btnAccess";
      this.btnAccess.Size = new Size(68, 22);
      this.btnAccess.TabIndex = 9;
      this.btnAccess.Text = "Access";
      this.btnAccess.UseVisualStyleBackColor = true;
      this.btnAccess.Click += new EventHandler(this.btnAccess_Click);
      this.lblAccess.AutoSize = true;
      this.lblAccess.Location = new Point(31, 128);
      this.lblAccess.Name = "lblAccess";
      this.lblAccess.Size = new Size(45, 14);
      this.lblAccess.TabIndex = 10;
      this.lblAccess.Text = "Access";
      this.cboMilestone.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboMilestone.FormattingEnabled = true;
      this.cboMilestone.ItemHeight = 16;
      this.cboMilestone.Location = new Point(84, 91);
      this.cboMilestone.Name = "cboMilestone";
      this.cboMilestone.SelectedBGColor = SystemColors.Highlight;
      this.cboMilestone.Size = new Size(214, 22);
      this.cboMilestone.TabIndex = 7;
      this.cboMilestone.SelectionChangeCommitted += new EventHandler(this.cboMilestone_SelectionChangeCommitted);
      this.txtMilestone.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtMilestone.Location = new Point(84, 94);
      this.txtMilestone.Name = "txtMilestone";
      this.txtMilestone.ReadOnly = true;
      this.txtMilestone.Size = new Size(0, 20);
      this.txtMilestone.TabIndex = 8;
      this.lblMilestone.AutoSize = true;
      this.lblMilestone.Location = new Point(7, 95);
      this.lblMilestone.Name = "lblMilestone";
      this.lblMilestone.Size = new Size(71, 14);
      this.lblMilestone.TabIndex = 6;
      this.lblMilestone.Text = "For Milestone";
      this.cboDocumentBorrower.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboDocumentBorrower.FormattingEnabled = true;
      this.cboDocumentBorrower.Location = new Point(84, 61);
      this.cboDocumentBorrower.Name = "cboDocumentBorrower";
      this.cboDocumentBorrower.Size = new Size(214, 22);
      this.cboDocumentBorrower.TabIndex = 4;
      this.cboDocumentBorrower.SelectionChangeCommitted += new EventHandler(this.cboDocumentBorrower_SelectionChangeCommitted);
      this.txtDocumentBorrower.Location = new Point(82, 65);
      this.txtDocumentBorrower.Name = "txtDocumentBorrower";
      this.txtDocumentBorrower.ReadOnly = true;
      this.txtDocumentBorrower.Size = new Size(214, 20);
      this.txtDocumentBorrower.TabIndex = 5;
      this.lblBorrower.AutoSize = true;
      this.lblBorrower.Location = new Point(7, 59);
      this.lblBorrower.MaximumSize = new Size(75, 0);
      this.lblBorrower.Name = "lblBorrower";
      this.lblBorrower.Size = new Size(73, 28);
      this.lblBorrower.TabIndex = 3;
      this.lblBorrower.Text = "For Borrower Pair";
      this.pageConditionDetails.BackColor = Color.WhiteSmoke;
      this.pageConditionDetails.Controls.Add((Control) this.gcConditionCommentText);
      this.pageConditionDetails.Controls.Add((Control) this.gcConditionComments);
      this.pageConditionDetails.Controls.Add((Control) this.gcConditionStatus);
      this.pageConditionDetails.Controls.Add((Control) this.gcConditionDetails);
      this.pageConditionDetails.Location = new Point(4, 23);
      this.pageConditionDetails.Name = "pageConditionDetails";
      this.pageConditionDetails.Size = new Size(950, 330);
      this.pageConditionDetails.TabIndex = 2;
      this.pageConditionDetails.Text = "Condition Details";
      this.pageConditionDetails.UseVisualStyleBackColor = true;
      this.gcConditionCommentText.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcConditionCommentText.Controls.Add((Control) this.txtConditionComment);
      this.gcConditionCommentText.Dock = DockStyle.Bottom;
      this.gcConditionCommentText.HeaderForeColor = SystemColors.ControlText;
      this.gcConditionCommentText.Location = new Point(645, 249);
      this.gcConditionCommentText.Name = "gcConditionCommentText";
      this.gcConditionCommentText.Size = new Size(305, 81);
      this.gcConditionCommentText.TabIndex = 9;
      this.gcConditionCommentText.Text = "Text";
      this.txtConditionComment.Dock = DockStyle.Fill;
      this.txtConditionComment.Location = new Point(1, 25);
      this.txtConditionComment.Multiline = true;
      this.txtConditionComment.Name = "txtConditionComment";
      this.txtConditionComment.ReadOnly = true;
      this.txtConditionComment.ScrollBars = ScrollBars.Vertical;
      this.txtConditionComment.Size = new Size(303, 55);
      this.txtConditionComment.TabIndex = 21;
      this.gcConditionComments.Controls.Add((Control) this.flowLayoutPanel2);
      this.gcConditionComments.Controls.Add((Control) this.gvConditionComments);
      this.gcConditionComments.Dock = DockStyle.Top;
      this.gcConditionComments.HeaderForeColor = SystemColors.ControlText;
      this.gcConditionComments.Location = new Point(645, 0);
      this.gcConditionComments.Name = "gcConditionComments";
      this.gcConditionComments.Size = new Size(305, 243);
      this.gcConditionComments.TabIndex = 8;
      this.gcConditionComments.Text = "Comments";
      this.flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.Controls.Add((Control) this.btnDeleteConditionComment);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnViewConditionComment);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnAddConditionComment);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(220, 3);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(80, 22);
      this.flowLayoutPanel2.TabIndex = 10;
      this.gvConditionComments.AllowDrop = true;
      this.gvConditionComments.AllowMultiselect = false;
      this.gvConditionComments.BorderStyle = BorderStyle.None;
      this.gvConditionComments.ClearSelectionsOnEmptyRowClick = false;
      this.gvConditionComments.Dock = DockStyle.Fill;
      this.gvConditionComments.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvConditionComments.ImageList = this.imageList;
      this.gvConditionComments.ItemGrouping = true;
      this.gvConditionComments.Location = new Point(1, 26);
      this.gvConditionComments.Name = "gvConditionComments";
      this.gvConditionComments.Size = new Size(303, 216);
      this.gvConditionComments.TabIndex = 9;
      this.gvConditionComments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvConditionComments.SelectedIndexCommitted += new EventHandler(this.gvConditionComments_SelectedIndexCommitted);
      this.gvConditionComments.DoubleClick += new EventHandler(this.btnViewConditionComment_Click);
      this.gcConditionStatus.Controls.Add((Control) this.panel1);
      this.gcConditionStatus.Dock = DockStyle.Left;
      this.gcConditionStatus.HeaderForeColor = SystemColors.ControlText;
      this.gcConditionStatus.Location = new Point(328, 0);
      this.gcConditionStatus.Name = "gcConditionStatus";
      this.gcConditionStatus.Size = new Size(317, 330);
      this.gcConditionStatus.TabIndex = 7;
      this.gcConditionStatus.Text = "Status";
      this.panel1.AutoScroll = true;
      this.panel1.AutoScrollMargin = new Size(8, 8);
      this.panel1.Controls.Add((Control) this.btnSentBy);
      this.panel1.Controls.Add((Control) this.txtSentBy);
      this.panel1.Controls.Add((Control) this.dateSent);
      this.panel1.Controls.Add((Control) this.txtDateSent);
      this.panel1.Controls.Add((Control) this.chkSent);
      this.panel1.Controls.Add((Control) this.btnConditionReviewedBy);
      this.panel1.Controls.Add((Control) this.txtConditionReviewedBy);
      this.panel1.Controls.Add((Control) this.dateConditionReviewed);
      this.panel1.Controls.Add((Control) this.txtConditionDateReviewed);
      this.panel1.Controls.Add((Control) this.chkConditionReviewed);
      this.panel1.Controls.Add((Control) this.btnWaivedBy);
      this.panel1.Controls.Add((Control) this.txtWaivedBy);
      this.panel1.Controls.Add((Control) this.dateWaived);
      this.panel1.Controls.Add((Control) this.chkWaived);
      this.panel1.Controls.Add((Control) this.btnClearedBy);
      this.panel1.Controls.Add((Control) this.txtClearedBy);
      this.panel1.Controls.Add((Control) this.dateCleared);
      this.panel1.Controls.Add((Control) this.chkCleared);
      this.panel1.Controls.Add((Control) this.btnRejectedBy);
      this.panel1.Controls.Add((Control) this.txtRejectedBy);
      this.panel1.Controls.Add((Control) this.dateRejected);
      this.panel1.Controls.Add((Control) this.chkRejected);
      this.panel1.Controls.Add((Control) this.btnConditionReceivedBy);
      this.panel1.Controls.Add((Control) this.txtConditionReceivedBy);
      this.panel1.Controls.Add((Control) this.dateConditionReceived);
      this.panel1.Controls.Add((Control) this.chkConditionReceived);
      this.panel1.Controls.Add((Control) this.btnConditionRerequestedBy);
      this.panel1.Controls.Add((Control) this.txtConditionRerequestedBy);
      this.panel1.Controls.Add((Control) this.dateConditionRerequested);
      this.panel1.Controls.Add((Control) this.chkConditionRerequested);
      this.panel1.Controls.Add((Control) this.btnConditionRequestedBy);
      this.panel1.Controls.Add((Control) this.txtConditionRequestedBy);
      this.panel1.Controls.Add((Control) this.dateConditionRequested);
      this.panel1.Controls.Add((Control) this.chkConditionRequested);
      this.panel1.Controls.Add((Control) this.btnFulfilledBy);
      this.panel1.Controls.Add((Control) this.txtFulfilledBy);
      this.panel1.Controls.Add((Control) this.dateFulfilled);
      this.panel1.Controls.Add((Control) this.chkFulfilled);
      this.panel1.Controls.Add((Control) this.txtAddedBy);
      this.panel1.Controls.Add((Control) this.txtAddedDate);
      this.panel1.Controls.Add((Control) this.chkAdded);
      this.panel1.Controls.Add((Control) this.txtConditionRequestedFrom);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Controls.Add((Control) this.txtConditionDateDue);
      this.panel1.Controls.Add((Control) this.txtConditionDaysDue);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.txtDateFulfilled);
      this.panel1.Controls.Add((Control) this.txtConditionDateRequested);
      this.panel1.Controls.Add((Control) this.txtConditionDateRerequested);
      this.panel1.Controls.Add((Control) this.txtConditionDateReceived);
      this.panel1.Controls.Add((Control) this.txtDateCleared);
      this.panel1.Controls.Add((Control) this.txtDateWaived);
      this.panel1.Controls.Add((Control) this.txtDateRejected);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(1, 26);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(315, 303);
      this.panel1.TabIndex = 0;
      this.txtSentBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtSentBy.Location = new Point(236, 272);
      this.txtSentBy.Name = "txtSentBy";
      this.txtSentBy.ReadOnly = true;
      this.txtSentBy.Size = new Size(53, 20);
      this.txtSentBy.TabIndex = 120;
      this.dateSent.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateSent.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateSent.CalendarTitleForeColor = Color.White;
      this.dateSent.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateSent.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateSent.Format = DateTimePickerFormat.Custom;
      this.dateSent.Location = new Point(98, 272);
      this.dateSent.Name = "dateSent";
      this.dateSent.Size = new Size(131, 20);
      this.dateSent.TabIndex = 117;
      this.dateSent.ValueChanged += new EventHandler(this.dateSent_ValueChanged);
      this.txtDateSent.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateSent.Location = new Point(100, 274);
      this.txtDateSent.Name = "txtDateSent";
      this.txtDateSent.ReadOnly = true;
      this.txtDateSent.Size = new Size(124, 20);
      this.txtDateSent.TabIndex = 119;
      this.chkSent.AutoSize = true;
      this.chkSent.Location = new Point(7, 274);
      this.chkSent.Name = "chkSent";
      this.chkSent.Size = new Size(48, 18);
      this.chkSent.TabIndex = 116;
      this.chkSent.Text = "Sent";
      this.chkSent.UseVisualStyleBackColor = true;
      this.chkSent.Click += new EventHandler(this.chkSent_Click);
      this.txtConditionReviewedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtConditionReviewedBy.Location = new Point(235, 176);
      this.txtConditionReviewedBy.Name = "txtConditionReviewedBy";
      this.txtConditionReviewedBy.ReadOnly = true;
      this.txtConditionReviewedBy.Size = new Size(53, 20);
      this.txtConditionReviewedBy.TabIndex = 115;
      this.txtConditionReviewedBy.Visible = false;
      this.dateConditionReviewed.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateConditionReviewed.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateConditionReviewed.CalendarTitleForeColor = Color.White;
      this.dateConditionReviewed.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateConditionReviewed.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateConditionReviewed.Format = DateTimePickerFormat.Custom;
      this.dateConditionReviewed.Location = new Point(100, 176);
      this.dateConditionReviewed.Name = "dateConditionReviewed";
      this.dateConditionReviewed.Size = new Size(129, 20);
      this.dateConditionReviewed.TabIndex = 114;
      this.dateConditionReviewed.Visible = false;
      this.dateConditionReviewed.ValueChanged += new EventHandler(this.dateConditionReviewed_ValueChanged);
      this.txtConditionDateReviewed.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtConditionDateReviewed.Location = new Point(98, 176);
      this.txtConditionDateReviewed.Name = "txtConditionDateReviewed";
      this.txtConditionDateReviewed.ReadOnly = true;
      this.txtConditionDateReviewed.Size = new Size(131, 20);
      this.txtConditionDateReviewed.TabIndex = 112;
      this.chkConditionReviewed.AutoSize = true;
      this.chkConditionReviewed.Location = new Point(7, 178);
      this.chkConditionReviewed.Name = "chkConditionReviewed";
      this.chkConditionReviewed.Size = new Size(75, 18);
      this.chkConditionReviewed.TabIndex = 113;
      this.chkConditionReviewed.Text = "Reviewed";
      this.chkConditionReviewed.UseVisualStyleBackColor = true;
      this.chkConditionReviewed.Visible = false;
      this.chkConditionReviewed.Click += new EventHandler(this.chkConditionReviewed_Click);
      this.txtWaivedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtWaivedBy.Location = new Point(236, 248);
      this.txtWaivedBy.Name = "txtWaivedBy";
      this.txtWaivedBy.ReadOnly = true;
      this.txtWaivedBy.Size = new Size(53, 20);
      this.txtWaivedBy.TabIndex = 107;
      this.txtWaivedBy.Visible = false;
      this.dateWaived.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateWaived.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateWaived.CalendarTitleForeColor = Color.White;
      this.dateWaived.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateWaived.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateWaived.Format = DateTimePickerFormat.Custom;
      this.dateWaived.Location = new Point(99, 248);
      this.dateWaived.Name = "dateWaived";
      this.dateWaived.Size = new Size(131, 20);
      this.dateWaived.TabIndex = 106;
      this.dateWaived.Visible = false;
      this.dateWaived.ValueChanged += new EventHandler(this.dateWaived_ValueChanged);
      this.chkWaived.AutoSize = true;
      this.chkWaived.Location = new Point(7, 250);
      this.chkWaived.Name = "chkWaived";
      this.chkWaived.Size = new Size(62, 18);
      this.chkWaived.TabIndex = 105;
      this.chkWaived.Text = "Waived";
      this.chkWaived.UseVisualStyleBackColor = true;
      this.chkWaived.Visible = false;
      this.chkWaived.Click += new EventHandler(this.chkWaived_Click);
      this.txtClearedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtClearedBy.Location = new Point(236, 224);
      this.txtClearedBy.Name = "txtClearedBy";
      this.txtClearedBy.ReadOnly = true;
      this.txtClearedBy.Size = new Size(53, 20);
      this.txtClearedBy.TabIndex = 104;
      this.txtClearedBy.Visible = false;
      this.dateCleared.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateCleared.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateCleared.CalendarTitleForeColor = Color.White;
      this.dateCleared.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateCleared.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateCleared.Format = DateTimePickerFormat.Custom;
      this.dateCleared.Location = new Point(99, 224);
      this.dateCleared.Name = "dateCleared";
      this.dateCleared.Size = new Size(130, 20);
      this.dateCleared.TabIndex = 103;
      this.dateCleared.Visible = false;
      this.dateCleared.ValueChanged += new EventHandler(this.dateCleared_ValueChanged);
      this.chkCleared.AutoSize = true;
      this.chkCleared.Location = new Point(7, 226);
      this.chkCleared.Name = "chkCleared";
      this.chkCleared.Size = new Size(63, 18);
      this.chkCleared.TabIndex = 102;
      this.chkCleared.Text = "Cleared";
      this.chkCleared.UseVisualStyleBackColor = true;
      this.chkCleared.Visible = false;
      this.chkCleared.Click += new EventHandler(this.chkCleared_Click);
      this.txtRejectedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtRejectedBy.Location = new Point(236, 200);
      this.txtRejectedBy.Name = "txtRejectedBy";
      this.txtRejectedBy.ReadOnly = true;
      this.txtRejectedBy.Size = new Size(53, 20);
      this.txtRejectedBy.TabIndex = 101;
      this.txtRejectedBy.Visible = false;
      this.dateRejected.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateRejected.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateRejected.CalendarTitleForeColor = Color.White;
      this.dateRejected.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateRejected.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateRejected.Format = DateTimePickerFormat.Custom;
      this.dateRejected.Location = new Point(99, 200);
      this.dateRejected.Name = "dateRejected";
      this.dateRejected.Size = new Size(130, 20);
      this.dateRejected.TabIndex = 100;
      this.dateRejected.Visible = false;
      this.dateRejected.ValueChanged += new EventHandler(this.dateRejected_ValueChanged);
      this.chkRejected.AutoSize = true;
      this.chkRejected.Location = new Point(7, 202);
      this.chkRejected.Name = "chkRejected";
      this.chkRejected.Size = new Size(68, 18);
      this.chkRejected.TabIndex = 99;
      this.chkRejected.Text = "Rejected";
      this.chkRejected.UseVisualStyleBackColor = true;
      this.chkRejected.Visible = false;
      this.chkRejected.Click += new EventHandler(this.chkRejected_Click);
      this.txtConditionReceivedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtConditionReceivedBy.Location = new Point(235, 152);
      this.txtConditionReceivedBy.Name = "txtConditionReceivedBy";
      this.txtConditionReceivedBy.ReadOnly = true;
      this.txtConditionReceivedBy.Size = new Size(53, 20);
      this.txtConditionReceivedBy.TabIndex = 89;
      this.txtConditionReceivedBy.Visible = false;
      this.dateConditionReceived.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateConditionReceived.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateConditionReceived.CalendarTitleForeColor = Color.White;
      this.dateConditionReceived.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateConditionReceived.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateConditionReceived.Format = DateTimePickerFormat.Custom;
      this.dateConditionReceived.Location = new Point(99, 152);
      this.dateConditionReceived.Name = "dateConditionReceived";
      this.dateConditionReceived.Size = new Size(130, 20);
      this.dateConditionReceived.TabIndex = 87;
      this.dateConditionReceived.Visible = false;
      this.dateConditionReceived.ValueChanged += new EventHandler(this.dateConditionReceived_ValueChanged);
      this.chkConditionReceived.AutoSize = true;
      this.chkConditionReceived.Location = new Point(7, 154);
      this.chkConditionReceived.Name = "chkConditionReceived";
      this.chkConditionReceived.Size = new Size(71, 18);
      this.chkConditionReceived.TabIndex = 86;
      this.chkConditionReceived.Text = "Received";
      this.chkConditionReceived.UseVisualStyleBackColor = true;
      this.chkConditionReceived.Click += new EventHandler(this.chkConditionReceived_Click);
      this.txtConditionRerequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtConditionRerequestedBy.Location = new Point(235, 128);
      this.txtConditionRerequestedBy.Name = "txtConditionRerequestedBy";
      this.txtConditionRerequestedBy.ReadOnly = true;
      this.txtConditionRerequestedBy.Size = new Size(53, 20);
      this.txtConditionRerequestedBy.TabIndex = 85;
      this.txtConditionRerequestedBy.Visible = false;
      this.dateConditionRerequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateConditionRerequested.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateConditionRerequested.CalendarTitleForeColor = Color.White;
      this.dateConditionRerequested.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateConditionRerequested.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateConditionRerequested.Format = DateTimePickerFormat.Custom;
      this.dateConditionRerequested.Location = new Point(99, 128);
      this.dateConditionRerequested.Name = "dateConditionRerequested";
      this.dateConditionRerequested.Size = new Size(130, 20);
      this.dateConditionRerequested.TabIndex = 84;
      this.dateConditionRerequested.Visible = false;
      this.dateConditionRerequested.ValueChanged += new EventHandler(this.dateConditionRerequested_ValueChanged);
      this.chkConditionRerequested.AutoSize = true;
      this.chkConditionRerequested.Location = new Point(7, 130);
      this.chkConditionRerequested.Name = "chkConditionRerequested";
      this.chkConditionRerequested.Size = new Size(92, 18);
      this.chkConditionRerequested.TabIndex = 83;
      this.chkConditionRerequested.Text = "Re-requested";
      this.chkConditionRerequested.UseVisualStyleBackColor = true;
      this.chkConditionRerequested.Click += new EventHandler(this.chkConditionRerequested_Click);
      this.txtConditionRequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtConditionRequestedBy.Location = new Point(235, 104);
      this.txtConditionRequestedBy.Name = "txtConditionRequestedBy";
      this.txtConditionRequestedBy.ReadOnly = true;
      this.txtConditionRequestedBy.Size = new Size(53, 20);
      this.txtConditionRequestedBy.TabIndex = 82;
      this.txtConditionRequestedBy.Visible = false;
      this.dateConditionRequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateConditionRequested.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateConditionRequested.CalendarTitleForeColor = Color.White;
      this.dateConditionRequested.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateConditionRequested.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateConditionRequested.Format = DateTimePickerFormat.Custom;
      this.dateConditionRequested.Location = new Point(99, 104);
      this.dateConditionRequested.Name = "dateConditionRequested";
      this.dateConditionRequested.Size = new Size(130, 20);
      this.dateConditionRequested.TabIndex = 81;
      this.dateConditionRequested.Visible = false;
      this.dateConditionRequested.ValueChanged += new EventHandler(this.dateConditionRequested_ValueChanged);
      this.chkConditionRequested.AutoSize = true;
      this.chkConditionRequested.Location = new Point(7, 106);
      this.chkConditionRequested.Name = "chkConditionRequested";
      this.chkConditionRequested.Size = new Size(78, 18);
      this.chkConditionRequested.TabIndex = 80;
      this.chkConditionRequested.Text = "Requested";
      this.chkConditionRequested.UseVisualStyleBackColor = true;
      this.chkConditionRequested.Click += new EventHandler(this.chkConditionRequested_Click);
      this.txtFulfilledBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtFulfilledBy.Location = new Point(235, 80);
      this.txtFulfilledBy.Name = "txtFulfilledBy";
      this.txtFulfilledBy.ReadOnly = true;
      this.txtFulfilledBy.Size = new Size(53, 20);
      this.txtFulfilledBy.TabIndex = 79;
      this.dateFulfilled.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateFulfilled.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateFulfilled.CalendarTitleForeColor = Color.White;
      this.dateFulfilled.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateFulfilled.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateFulfilled.Format = DateTimePickerFormat.Custom;
      this.dateFulfilled.Location = new Point(98, 80);
      this.dateFulfilled.Name = "dateFulfilled";
      this.dateFulfilled.Size = new Size(131, 20);
      this.dateFulfilled.TabIndex = 78;
      this.dateFulfilled.Visible = false;
      this.dateFulfilled.ValueChanged += new EventHandler(this.dateFulfilled_ValueChanged);
      this.chkFulfilled.AutoSize = true;
      this.chkFulfilled.Location = new Point(7, 82);
      this.chkFulfilled.Name = "chkFulfilled";
      this.chkFulfilled.Size = new Size(62, 18);
      this.chkFulfilled.TabIndex = 77;
      this.chkFulfilled.Text = "Fulfilled";
      this.chkFulfilled.UseVisualStyleBackColor = true;
      this.chkFulfilled.Click += new EventHandler(this.chkFulfilled_Click);
      this.txtAddedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtAddedBy.Location = new Point(235, 56);
      this.txtAddedBy.Name = "txtAddedBy";
      this.txtAddedBy.ReadOnly = true;
      this.txtAddedBy.Size = new Size(54, 20);
      this.txtAddedBy.TabIndex = 76;
      this.txtAddedDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtAddedDate.Location = new Point(99, 56);
      this.txtAddedDate.Name = "txtAddedDate";
      this.txtAddedDate.ReadOnly = true;
      this.txtAddedDate.Size = new Size(130, 20);
      this.txtAddedDate.TabIndex = 75;
      this.chkAdded.AutoSize = true;
      this.chkAdded.Checked = true;
      this.chkAdded.CheckState = CheckState.Checked;
      this.chkAdded.Enabled = false;
      this.chkAdded.Location = new Point(7, 59);
      this.chkAdded.Name = "chkAdded";
      this.chkAdded.Size = new Size(58, 18);
      this.chkAdded.TabIndex = 74;
      this.chkAdded.Text = "Added";
      this.chkAdded.UseVisualStyleBackColor = true;
      this.txtConditionRequestedFrom.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtConditionRequestedFrom.Location = new Point(99, 30);
      this.txtConditionRequestedFrom.Name = "txtConditionRequestedFrom";
      this.txtConditionRequestedFrom.Size = new Size(190, 20);
      this.txtConditionRequestedFrom.TabIndex = 72;
      this.txtConditionRequestedFrom.Validated += new EventHandler(this.txtConditionRequestedFrom_Validated);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 33);
      this.label1.Name = "label1";
      this.label1.Size = new Size(86, 14);
      this.label1.TabIndex = 71;
      this.label1.Text = "Requested From";
      this.txtConditionDateDue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtConditionDateDue.Location = new Point(151, 4);
      this.txtConditionDateDue.Name = "txtConditionDateDue";
      this.txtConditionDateDue.ReadOnly = true;
      this.txtConditionDateDue.Size = new Size(138, 20);
      this.txtConditionDateDue.TabIndex = 67;
      this.txtConditionDaysDue.Location = new Point(99, 4);
      this.txtConditionDaysDue.Name = "txtConditionDaysDue";
      this.txtConditionDaysDue.Size = new Size(48, 20);
      this.txtConditionDaysDue.TabIndex = 66;
      this.txtConditionDaysDue.TextAlign = HorizontalAlignment.Right;
      this.txtConditionDaysDue.Validating += new CancelEventHandler(this.txtConditionDaysDue_Validating);
      this.txtConditionDaysDue.Validated += new EventHandler(this.txtConditionDaysDue_Validated);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(7, 7);
      this.label4.Name = "label4";
      this.label4.Size = new Size(86, 14);
      this.label4.TabIndex = 65;
      this.label4.Text = "Days to Receive";
      this.txtDateFulfilled.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateFulfilled.Location = new Point(101, 82);
      this.txtDateFulfilled.Name = "txtDateFulfilled";
      this.txtDateFulfilled.ReadOnly = true;
      this.txtDateFulfilled.Size = new Size(124, 20);
      this.txtDateFulfilled.TabIndex = 92;
      this.txtConditionDateRequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtConditionDateRequested.Location = new Point(98, 106);
      this.txtConditionDateRequested.Name = "txtConditionDateRequested";
      this.txtConditionDateRequested.ReadOnly = true;
      this.txtConditionDateRequested.Size = new Size(125, 20);
      this.txtConditionDateRequested.TabIndex = 93;
      this.txtConditionDateRequested.Visible = false;
      this.txtConditionDateRerequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtConditionDateRerequested.Location = new Point(99, 130);
      this.txtConditionDateRerequested.Name = "txtConditionDateRerequested";
      this.txtConditionDateRerequested.ReadOnly = true;
      this.txtConditionDateRerequested.Size = new Size(125, 20);
      this.txtConditionDateRerequested.TabIndex = 94;
      this.txtConditionDateRerequested.Visible = false;
      this.txtConditionDateReceived.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtConditionDateReceived.Location = new Point(98, 154);
      this.txtConditionDateReceived.Name = "txtConditionDateReceived";
      this.txtConditionDateReceived.ReadOnly = true;
      this.txtConditionDateReceived.Size = new Size(125, 20);
      this.txtConditionDateReceived.TabIndex = 95;
      this.txtConditionDateReceived.Visible = false;
      this.txtDateCleared.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateCleared.Location = new Point(100, 226);
      this.txtDateCleared.Name = "txtDateCleared";
      this.txtDateCleared.ReadOnly = true;
      this.txtDateCleared.Size = new Size(124, 20);
      this.txtDateCleared.TabIndex = 108;
      this.txtDateWaived.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateWaived.Location = new Point(98, 250);
      this.txtDateWaived.Name = "txtDateWaived";
      this.txtDateWaived.ReadOnly = true;
      this.txtDateWaived.Size = new Size((int) sbyte.MaxValue, 20);
      this.txtDateWaived.TabIndex = 109;
      this.txtDateRejected.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateRejected.Location = new Point(99, 202);
      this.txtDateRejected.Name = "txtDateRejected";
      this.txtDateRejected.ReadOnly = true;
      this.txtDateRejected.Size = new Size(126, 20);
      this.txtDateRejected.TabIndex = 110;
      this.gcConditionDetails.Controls.Add((Control) this.panel2);
      this.gcConditionDetails.Dock = DockStyle.Left;
      this.gcConditionDetails.HeaderForeColor = SystemColors.ControlText;
      this.gcConditionDetails.Location = new Point(0, 0);
      this.gcConditionDetails.Name = "gcConditionDetails";
      this.gcConditionDetails.Size = new Size(328, 330);
      this.gcConditionDetails.TabIndex = 6;
      this.gcConditionDetails.Text = "Details";
      this.panel2.AutoScroll = true;
      this.panel2.AutoScrollMargin = new Size(8, 8);
      this.panel2.Controls.Add((Control) this.cboSource);
      this.panel2.Controls.Add((Control) this.cboRecipient);
      this.panel2.Controls.Add((Control) this.txtRecipient);
      this.panel2.Controls.Add((Control) this.lblRecipient);
      this.panel2.Controls.Add((Control) this.lblPrint);
      this.panel2.Controls.Add((Control) this.chkExternal);
      this.panel2.Controls.Add((Control) this.chkInternal);
      this.panel2.Controls.Add((Control) this.chkAllowToClear);
      this.panel2.Controls.Add((Control) this.cboOwner);
      this.panel2.Controls.Add((Control) this.txtOwner);
      this.panel2.Controls.Add((Control) this.lblOwner);
      this.panel2.Controls.Add((Control) this.chkUnderwriter);
      this.panel2.Controls.Add((Control) this.cboPriorTo);
      this.panel2.Controls.Add((Control) this.lblPriorTo);
      this.panel2.Controls.Add((Control) this.cboCategory);
      this.panel2.Controls.Add((Control) this.lblCategory);
      this.panel2.Controls.Add((Control) this.txtSource);
      this.panel2.Controls.Add((Control) this.lblSource);
      this.panel2.Controls.Add((Control) this.lstConditions);
      this.panel2.Controls.Add((Control) this.label5);
      this.panel2.Controls.Add((Control) this.cboConditionBorrower);
      this.panel2.Controls.Add((Control) this.txtConditionBorrower);
      this.panel2.Controls.Add((Control) this.label11);
      this.panel2.Controls.Add((Control) this.txtCategory);
      this.panel2.Controls.Add((Control) this.txtPriorTo);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(1, 26);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(326, 303);
      this.panel2.TabIndex = 0;
      this.cboSource.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboSource.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSource.FormattingEnabled = true;
      this.cboSource.Location = new Point(84, 89);
      this.cboSource.Name = "cboSource";
      this.cboSource.Size = new Size(214, 22);
      this.cboSource.TabIndex = 50;
      this.cboSource.SelectionChangeCommitted += new EventHandler(this.cboSource_SelectionChangeCommitted);
      this.cboRecipient.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboRecipient.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRecipient.FormattingEnabled = true;
      this.cboRecipient.Items.AddRange(new object[2]
      {
        (object) "Investor",
        (object) "MERS"
      });
      this.cboRecipient.Location = new Point(84, 244);
      this.cboRecipient.Name = "cboRecipient";
      this.cboRecipient.Size = new Size(214, 22);
      this.cboRecipient.TabIndex = 48;
      this.cboRecipient.SelectionChangeCommitted += new EventHandler(this.cboRecipient_SelectionChangeCommitted);
      this.txtRecipient.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtRecipient.Location = new Point(83, 244);
      this.txtRecipient.Name = "txtRecipient";
      this.txtRecipient.ReadOnly = true;
      this.txtRecipient.Size = new Size(215, 20);
      this.txtRecipient.TabIndex = 49;
      this.lblRecipient.AutoSize = true;
      this.lblRecipient.Location = new Point(7, 248);
      this.lblRecipient.Name = "lblRecipient";
      this.lblRecipient.Size = new Size(51, 14);
      this.lblRecipient.TabIndex = 47;
      this.lblRecipient.Text = "Recipient";
      this.lblPrint.AutoSize = true;
      this.lblPrint.Location = new Point(7, 205);
      this.lblPrint.Name = "lblPrint";
      this.lblPrint.Size = new Size(28, 14);
      this.lblPrint.TabIndex = 44;
      this.lblPrint.Text = "Print";
      this.lblPrint.Visible = false;
      this.chkExternal.AutoSize = true;
      this.chkExternal.Location = new Point(156, 204);
      this.chkExternal.Name = "chkExternal";
      this.chkExternal.Size = new Size(73, 18);
      this.chkExternal.TabIndex = 46;
      this.chkExternal.Text = "Externally";
      this.chkExternal.UseVisualStyleBackColor = true;
      this.chkExternal.Visible = false;
      this.chkExternal.Click += new EventHandler(this.chkExternal_Click);
      this.chkInternal.AutoSize = true;
      this.chkInternal.Location = new Point(84, 204);
      this.chkInternal.Name = "chkInternal";
      this.chkInternal.Size = new Size(69, 18);
      this.chkInternal.TabIndex = 45;
      this.chkInternal.Text = "Internally";
      this.chkInternal.UseVisualStyleBackColor = true;
      this.chkInternal.Visible = false;
      this.chkInternal.Click += new EventHandler(this.chkInternal_Click);
      this.chkAllowToClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkAllowToClear.AutoSize = true;
      this.chkAllowToClear.Location = new Point(225, 176);
      this.chkAllowToClear.Name = "chkAllowToClear";
      this.chkAllowToClear.Size = new Size(94, 18);
      this.chkAllowToClear.TabIndex = 43;
      this.chkAllowToClear.Text = "Allow to Clear";
      this.chkAllowToClear.UseVisualStyleBackColor = true;
      this.chkAllowToClear.Visible = false;
      this.chkAllowToClear.Click += new EventHandler(this.chkAllowToClear_Click);
      this.cboOwner.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboOwner.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboOwner.FormattingEnabled = true;
      this.cboOwner.Location = new Point(84, 173);
      this.cboOwner.Name = "cboOwner";
      this.cboOwner.Size = new Size(135, 22);
      this.cboOwner.TabIndex = 41;
      this.cboOwner.Visible = false;
      this.cboOwner.SelectionChangeCommitted += new EventHandler(this.cboOwner_SelectionChangeCommitted);
      this.txtOwner.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtOwner.Location = new Point(84, 173);
      this.txtOwner.Name = "txtOwner";
      this.txtOwner.ReadOnly = true;
      this.txtOwner.Size = new Size(141, 20);
      this.txtOwner.TabIndex = 42;
      this.txtOwner.Visible = false;
      this.lblOwner.AutoSize = true;
      this.lblOwner.Location = new Point(7, 177);
      this.lblOwner.Name = "lblOwner";
      this.lblOwner.Size = new Size(41, 14);
      this.lblOwner.TabIndex = 40;
      this.lblOwner.Text = "Owner";
      this.lblOwner.Visible = false;
      this.chkUnderwriter.AutoSize = true;
      this.chkUnderwriter.Location = new Point(10, 227);
      this.chkUnderwriter.Name = "chkUnderwriter";
      this.chkUnderwriter.Size = new Size(169, 18);
      this.chkUnderwriter.TabIndex = 37;
      this.chkUnderwriter.Text = "UW can access this condition";
      this.chkUnderwriter.UseVisualStyleBackColor = true;
      this.chkUnderwriter.Visible = false;
      this.chkUnderwriter.Click += new EventHandler(this.chkUnderwriter_Click);
      this.cboPriorTo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPriorTo.FormattingEnabled = true;
      this.cboPriorTo.Items.AddRange(new object[5]
      {
        (object) "Approval",
        (object) "Docs",
        (object) "Funding",
        (object) "Closing",
        (object) "Purchase"
      });
      this.cboPriorTo.Location = new Point(84, 145);
      this.cboPriorTo.Name = "cboPriorTo";
      this.cboPriorTo.Size = new Size(214, 22);
      this.cboPriorTo.TabIndex = 36;
      this.cboPriorTo.SelectionChangeCommitted += new EventHandler(this.cboPriorTo_SelectionChangeCommitted);
      this.lblPriorTo.AutoSize = true;
      this.lblPriorTo.Location = new Point(7, 148);
      this.lblPriorTo.Name = "lblPriorTo";
      this.lblPriorTo.Size = new Size(43, 14);
      this.lblPriorTo.TabIndex = 35;
      this.lblPriorTo.Text = "Prior To";
      this.cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCategory.FormattingEnabled = true;
      this.cboCategory.Items.AddRange(new object[6]
      {
        (object) "Assets",
        (object) "Credit",
        (object) "Income",
        (object) "Legal",
        (object) "Misc",
        (object) "Property"
      });
      this.cboCategory.Location = new Point(84, 117);
      this.cboCategory.Name = "cboCategory";
      this.cboCategory.Size = new Size(214, 22);
      this.cboCategory.TabIndex = 34;
      this.cboCategory.SelectionChangeCommitted += new EventHandler(this.cboCategory_SelectionChangeCommitted);
      this.lblCategory.AutoSize = true;
      this.lblCategory.Location = new Point(7, 120);
      this.lblCategory.Name = "lblCategory";
      this.lblCategory.Size = new Size(51, 14);
      this.lblCategory.TabIndex = 33;
      this.lblCategory.Text = "Category";
      this.txtSource.Location = new Point(84, 89);
      this.txtSource.Name = "txtSource";
      this.txtSource.ReadOnly = true;
      this.txtSource.Size = new Size(214, 20);
      this.txtSource.TabIndex = 32;
      this.lblSource.AutoSize = true;
      this.lblSource.Location = new Point(7, 92);
      this.lblSource.Name = "lblSource";
      this.lblSource.Size = new Size(42, 14);
      this.lblSource.TabIndex = 31;
      this.lblSource.Text = "Source";
      this.lstConditions.BackColor = SystemColors.Control;
      this.lstConditions.FormattingEnabled = true;
      this.lstConditions.HorizontalScrollbar = true;
      this.lstConditions.IntegralHeight = false;
      this.lstConditions.ItemHeight = 14;
      this.lstConditions.Location = new Point(84, 7);
      this.lstConditions.Name = "lstConditions";
      this.lstConditions.SelectionMode = SelectionMode.None;
      this.lstConditions.Size = new Size(214, 48);
      this.lstConditions.Sorted = true;
      this.lstConditions.TabIndex = 30;
      this.lstConditions.TabStop = false;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(7, 7);
      this.label5.Name = "label5";
      this.label5.Size = new Size(57, 14);
      this.label5.TabIndex = 19;
      this.label5.Text = "Conditions";
      this.cboConditionBorrower.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboConditionBorrower.FormattingEnabled = true;
      this.cboConditionBorrower.Location = new Point(84, 61);
      this.cboConditionBorrower.Name = "cboConditionBorrower";
      this.cboConditionBorrower.Size = new Size(214, 22);
      this.cboConditionBorrower.TabIndex = 4;
      this.cboConditionBorrower.SelectionChangeCommitted += new EventHandler(this.cboConditionBorrower_SelectionChangeCommitted);
      this.txtConditionBorrower.Location = new Point(84, 63);
      this.txtConditionBorrower.Name = "txtConditionBorrower";
      this.txtConditionBorrower.ReadOnly = true;
      this.txtConditionBorrower.Size = new Size(214, 20);
      this.txtConditionBorrower.TabIndex = 5;
      this.label11.AutoSize = true;
      this.label11.Location = new Point(7, 59);
      this.label11.MaximumSize = new Size(75, 0);
      this.label11.Name = "label11";
      this.label11.Size = new Size(73, 28);
      this.label11.TabIndex = 3;
      this.label11.Text = "For Borrower Pair";
      this.txtCategory.Location = new Point(84, 119);
      this.txtCategory.Name = "txtCategory";
      this.txtCategory.ReadOnly = true;
      this.txtCategory.Size = new Size(214, 20);
      this.txtCategory.TabIndex = 38;
      this.txtPriorTo.Location = new Point(84, 145);
      this.txtPriorTo.Name = "txtPriorTo";
      this.txtPriorTo.ReadOnly = true;
      this.txtPriorTo.Size = new Size(214, 20);
      this.txtPriorTo.TabIndex = 39;
      this.pageConditionsEnhanced.Controls.Add((Control) this.panel5);
      this.pageConditionsEnhanced.Controls.Add((Control) this.groupContainer1);
      this.pageConditionsEnhanced.Controls.Add((Control) this.gcEnhancedConditionDetails);
      this.pageConditionsEnhanced.Location = new Point(4, 23);
      this.pageConditionsEnhanced.Name = "pageConditionsEnhanced";
      this.pageConditionsEnhanced.Padding = new Padding(3);
      this.pageConditionsEnhanced.Size = new Size(950, 330);
      this.pageConditionsEnhanced.TabIndex = 3;
      this.pageConditionsEnhanced.Text = "Condition Details";
      this.pageConditionsEnhanced.UseVisualStyleBackColor = true;
      this.panel5.Controls.Add((Control) this.txtCommentEnhanced);
      this.panel5.Controls.Add((Control) this.btnAddCommentEnhanced);
      this.panel5.Controls.Add((Control) this.chkExternalCommentEnhanced);
      this.panel5.Controls.Add((Control) this.commentCollectionEnhanced);
      this.panel5.Location = new Point(647, 3);
      this.panel5.Name = "panel5";
      this.panel5.Size = new Size(303, 323);
      this.panel5.TabIndex = 46;
      this.txtCommentEnhanced.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtCommentEnhanced.Location = new Point(18, 238);
      this.txtCommentEnhanced.Multiline = true;
      this.txtCommentEnhanced.Name = "txtCommentEnhanced";
      this.txtCommentEnhanced.ScrollBars = ScrollBars.Vertical;
      this.txtCommentEnhanced.Size = new Size(267, 49);
      this.txtCommentEnhanced.TabIndex = 52;
      this.txtCommentEnhanced.TextChanged += new EventHandler(this.txtCommentEnhanced_TextChanged);
      this.btnAddCommentEnhanced.Enabled = false;
      this.btnAddCommentEnhanced.Location = new Point(151, 293);
      this.btnAddCommentEnhanced.Name = "btnAddCommentEnhanced";
      this.btnAddCommentEnhanced.Size = new Size(134, 23);
      this.btnAddCommentEnhanced.TabIndex = 51;
      this.btnAddCommentEnhanced.Text = "Add Comment";
      this.btnAddCommentEnhanced.UseVisualStyleBackColor = true;
      this.btnAddCommentEnhanced.Click += new EventHandler(this.btnAddCommentEnhanced_Click);
      this.chkExternalCommentEnhanced.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkExternalCommentEnhanced.AutoSize = true;
      this.chkExternalCommentEnhanced.Enabled = false;
      this.chkExternalCommentEnhanced.Location = new Point(18, 298);
      this.chkExternalCommentEnhanced.Name = "chkExternalCommentEnhanced";
      this.chkExternalCommentEnhanced.Size = new Size(112, 18);
      this.chkExternalCommentEnhanced.TabIndex = 50;
      this.chkExternalCommentEnhanced.Text = "External Comment";
      this.chkExternalCommentEnhanced.UseVisualStyleBackColor = true;
      this.commentCollectionEnhanced.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.commentCollectionEnhanced.CanAddComment = false;
      this.commentCollectionEnhanced.CanDeleteComment = false;
      this.commentCollectionEnhanced.CanDeliverComment = false;
      this.commentCollectionEnhanced.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.commentCollectionEnhanced.Location = new Point(0, 0);
      this.commentCollectionEnhanced.Name = "commentCollectionEnhanced";
      this.commentCollectionEnhanced.Size = new Size(300, 225);
      this.commentCollectionEnhanced.TabIndex = 33;
      this.groupContainer1.Controls.Add((Control) this.panel4);
      this.groupContainer1.Dock = DockStyle.Left;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(331, 3);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(317, 324);
      this.groupContainer1.TabIndex = 8;
      this.groupContainer1.Text = "Tracking Status";
      this.panel4.AutoScroll = true;
      this.panel4.AutoScrollMargin = new Size(8, 8);
      this.panel4.Controls.Add((Control) this.gvTrackingEnhanced);
      this.panel4.Controls.Add((Control) this.txtRequestedFromEnhanced);
      this.panel4.Controls.Add((Control) this.label21);
      this.panel4.Controls.Add((Control) this.txtDateDueEnhanced);
      this.panel4.Controls.Add((Control) this.txtDaysToReceiveEnhanced);
      this.panel4.Controls.Add((Control) this.label22);
      this.panel4.Dock = DockStyle.Fill;
      this.panel4.Location = new Point(1, 26);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(315, 297);
      this.panel4.TabIndex = 0;
      this.gvTrackingEnhanced.AllowDrop = true;
      this.gvTrackingEnhanced.BorderStyle = BorderStyle.None;
      this.gvTrackingEnhanced.ClearSelectionsOnEmptyRowClick = false;
      this.gvTrackingEnhanced.Dock = DockStyle.Bottom;
      this.gvTrackingEnhanced.HeaderHeight = 0;
      this.gvTrackingEnhanced.HeaderVisible = false;
      this.gvTrackingEnhanced.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvTrackingEnhanced.Location = new Point(0, 74);
      this.gvTrackingEnhanced.Name = "gvTrackingEnhanced";
      this.gvTrackingEnhanced.Size = new Size(315, 223);
      this.gvTrackingEnhanced.SortOption = GVSortOption.None;
      this.gvTrackingEnhanced.TabIndex = 73;
      this.gvTrackingEnhanced.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvTrackingEnhanced.UseCompatibleEditingBehavior = true;
      this.gvTrackingEnhanced.SubItemCheck += new GVSubItemEventHandler(this.gvTrackingEnhanced_SubItemCheck);
      this.txtRequestedFromEnhanced.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtRequestedFromEnhanced.Location = new Point(100, 37);
      this.txtRequestedFromEnhanced.Name = "txtRequestedFromEnhanced";
      this.txtRequestedFromEnhanced.Size = new Size(199, 20);
      this.txtRequestedFromEnhanced.TabIndex = 72;
      this.txtRequestedFromEnhanced.Validated += new EventHandler(this.txtRequestedFromEnhanced_Validated);
      this.label21.AutoSize = true;
      this.label21.Location = new Point(8, 43);
      this.label21.Name = "label21";
      this.label21.Size = new Size(86, 14);
      this.label21.TabIndex = 71;
      this.label21.Text = "Requested From";
      this.txtDateDueEnhanced.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateDueEnhanced.Location = new Point(154, 11);
      this.txtDateDueEnhanced.Name = "txtDateDueEnhanced";
      this.txtDateDueEnhanced.ReadOnly = true;
      this.txtDateDueEnhanced.Size = new Size(145, 20);
      this.txtDateDueEnhanced.TabIndex = 67;
      this.txtDaysToReceiveEnhanced.Location = new Point(100, 11);
      this.txtDaysToReceiveEnhanced.Name = "txtDaysToReceiveEnhanced";
      this.txtDaysToReceiveEnhanced.Size = new Size(48, 20);
      this.txtDaysToReceiveEnhanced.TabIndex = 66;
      this.txtDaysToReceiveEnhanced.TextAlign = HorizontalAlignment.Right;
      this.txtDaysToReceiveEnhanced.Validated += new EventHandler(this.txtDaysToReceiveEnhanced_Validated);
      this.label22.AutoSize = true;
      this.label22.Location = new Point(8, 11);
      this.label22.Name = "label22";
      this.label22.Size = new Size(86, 14);
      this.label22.TabIndex = 65;
      this.label22.Text = "Days to Receive";
      this.gcEnhancedConditionDetails.Controls.Add((Control) this.panel3);
      this.gcEnhancedConditionDetails.Dock = DockStyle.Left;
      this.gcEnhancedConditionDetails.HeaderForeColor = SystemColors.ControlText;
      this.gcEnhancedConditionDetails.Location = new Point(3, 3);
      this.gcEnhancedConditionDetails.Name = "gcEnhancedConditionDetails";
      this.gcEnhancedConditionDetails.Size = new Size(328, 324);
      this.gcEnhancedConditionDetails.TabIndex = 7;
      this.gcEnhancedConditionDetails.Text = "Details";
      this.panel3.AutoScroll = true;
      this.panel3.AutoScrollMargin = new Size(8, 8);
      this.panel3.Controls.Add((Control) this.label6);
      this.panel3.Controls.Add((Control) this.txtConditionType);
      this.panel3.Controls.Add((Control) this.cboSourceEnhanced);
      this.panel3.Controls.Add((Control) this.cboPriorToEnhanced);
      this.panel3.Controls.Add((Control) this.label9);
      this.panel3.Controls.Add((Control) this.label12);
      this.panel3.Controls.Add((Control) this.lstConditionsEnhanced);
      this.panel3.Controls.Add((Control) this.label13);
      this.panel3.Controls.Add((Control) this.cboConditionBorrowerEnhanced);
      this.panel3.Controls.Add((Control) this.label14);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(1, 26);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(326, 297);
      this.panel3.TabIndex = 0;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(10, 56);
      this.label6.Name = "label6";
      this.label6.Size = new Size(77, 14);
      this.label6.TabIndex = 67;
      this.label6.Text = "Condition Type";
      this.txtConditionType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtConditionType.Enabled = false;
      this.txtConditionType.Location = new Point(88, 56);
      this.txtConditionType.Name = "txtConditionType";
      this.txtConditionType.ReadOnly = true;
      this.txtConditionType.ScrollBars = ScrollBars.Vertical;
      this.txtConditionType.Size = new Size(219, 20);
      this.txtConditionType.TabIndex = 65;
      this.cboSourceEnhanced.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboSourceEnhanced.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSourceEnhanced.FormattingEnabled = true;
      this.cboSourceEnhanced.Location = new Point(88, 132);
      this.cboSourceEnhanced.Name = "cboSourceEnhanced";
      this.cboSourceEnhanced.Size = new Size(219, 22);
      this.cboSourceEnhanced.TabIndex = 50;
      this.cboSourceEnhanced.SelectionChangeCommitted += new EventHandler(this.cboSourceEnhanced_SelectionChangeCommitted);
      this.cboPriorToEnhanced.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPriorToEnhanced.FormattingEnabled = true;
      this.cboPriorToEnhanced.Location = new Point(88, 167);
      this.cboPriorToEnhanced.Name = "cboPriorToEnhanced";
      this.cboPriorToEnhanced.Size = new Size(219, 22);
      this.cboPriorToEnhanced.TabIndex = 36;
      this.cboPriorToEnhanced.SelectionChangeCommitted += new EventHandler(this.cboPriorToEnhanced_SelectionChangeCommitted);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(11, 167);
      this.label9.Name = "label9";
      this.label9.Size = new Size(43, 14);
      this.label9.TabIndex = 35;
      this.label9.Text = "Prior To";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(11, 132);
      this.label12.Name = "label12";
      this.label12.Size = new Size(42, 14);
      this.label12.TabIndex = 31;
      this.label12.Text = "Source";
      this.lstConditionsEnhanced.BackColor = SystemColors.Control;
      this.lstConditionsEnhanced.FormattingEnabled = true;
      this.lstConditionsEnhanced.HorizontalScrollbar = true;
      this.lstConditionsEnhanced.IntegralHeight = false;
      this.lstConditionsEnhanced.ItemHeight = 14;
      this.lstConditionsEnhanced.Location = new Point(88, 11);
      this.lstConditionsEnhanced.Name = "lstConditionsEnhanced";
      this.lstConditionsEnhanced.SelectionMode = SelectionMode.None;
      this.lstConditionsEnhanced.Size = new Size(219, 33);
      this.lstConditionsEnhanced.Sorted = true;
      this.lstConditionsEnhanced.TabIndex = 30;
      this.lstConditionsEnhanced.TabStop = false;
      this.label13.AutoSize = true;
      this.label13.Location = new Point(10, 11);
      this.label13.Name = "label13";
      this.label13.Size = new Size(57, 14);
      this.label13.TabIndex = 19;
      this.label13.Text = "Conditions";
      this.cboConditionBorrowerEnhanced.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboConditionBorrowerEnhanced.FormattingEnabled = true;
      this.cboConditionBorrowerEnhanced.Location = new Point(88, 94);
      this.cboConditionBorrowerEnhanced.Name = "cboConditionBorrowerEnhanced";
      this.cboConditionBorrowerEnhanced.Size = new Size(219, 22);
      this.cboConditionBorrowerEnhanced.TabIndex = 4;
      this.cboConditionBorrowerEnhanced.SelectionChangeCommitted += new EventHandler(this.cboConditionBorrowerEnhanced_SelectionChangeCommitted);
      this.label14.AutoSize = true;
      this.label14.Location = new Point(10, 92);
      this.label14.MaximumSize = new Size(75, 0);
      this.label14.Name = "label14";
      this.label14.Size = new Size(73, 28);
      this.label14.TabIndex = 3;
      this.label14.Text = "For Borrower Pair";
      this.csLeft.AnimationDelay = 20;
      this.csLeft.AnimationStep = 20;
      this.csLeft.BorderStyle3D = Border3DStyle.Flat;
      this.csLeft.ControlToHide = (Control) this.pnlLeft;
      this.csLeft.ExpandParentForm = false;
      this.csLeft.Location = new Point(301, 0);
      this.csLeft.Name = "csLeft";
      this.csLeft.TabIndex = 9;
      this.csLeft.TabStop = false;
      this.csLeft.UseAnimations = false;
      this.csLeft.VisualStyle = VisualStyles.Encompass;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(1268, 653);
      this.Controls.Add((Control) this.pnlRight);
      this.Controls.Add((Control) this.csLeft);
      this.Controls.Add((Control) this.pnlLeft);
      this.Controls.Add((Control) this.pnlClose);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = Resources.icon_allsizes_bug;
      this.KeyPreview = true;
      this.Name = nameof (AllDocumentsDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Document Manager";
      this.Activated += new EventHandler(this.AllDocumentsDialog_Activated);
      this.FormClosing += new FormClosingEventHandler(this.AllDocumentsDialog_FormClosing);
      this.KeyDown += new KeyEventHandler(this.AllDocumentsDialog_KeyDown);
      this.Resize += new EventHandler(this.AllDocumentsDialog_Resize);
      ((ISupportInitialize) this.btnDeleteDocumentComment).EndInit();
      ((ISupportInitialize) this.btnViewDocumentComment).EndInit();
      ((ISupportInitialize) this.btnAddDocumentComment).EndInit();
      ((ISupportInitialize) this.btnShippingReadyBy).EndInit();
      ((ISupportInitialize) this.btnUnderwritingReadyBy).EndInit();
      ((ISupportInitialize) this.btnDocumentReviewedBy).EndInit();
      ((ISupportInitialize) this.btnDocumentReceivedBy).EndInit();
      ((ISupportInitialize) this.btnDocumentRerequestedBy).EndInit();
      ((ISupportInitialize) this.btnDocumentRequestedBy).EndInit();
      ((ISupportInitialize) this.btnDeleteConditionComment).EndInit();
      ((ISupportInitialize) this.btnViewConditionComment).EndInit();
      ((ISupportInitialize) this.btnAddConditionComment).EndInit();
      ((ISupportInitialize) this.btnSentBy).EndInit();
      ((ISupportInitialize) this.btnConditionReviewedBy).EndInit();
      ((ISupportInitialize) this.btnWaivedBy).EndInit();
      ((ISupportInitialize) this.btnClearedBy).EndInit();
      ((ISupportInitialize) this.btnRejectedBy).EndInit();
      ((ISupportInitialize) this.btnConditionReceivedBy).EndInit();
      ((ISupportInitialize) this.btnConditionRerequestedBy).EndInit();
      ((ISupportInitialize) this.btnConditionRequestedBy).EndInit();
      ((ISupportInitialize) this.btnFulfilledBy).EndInit();
      ((ISupportInitialize) this.btnEditCondition).EndInit();
      ((ISupportInitialize) this.btnAddCondition).EndInit();
      this.pnlLeft.ResumeLayout(false);
      this.gcConditions.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      this.gcDocuments.ResumeLayout(false);
      this.pnlStackingOrder.ResumeLayout(false);
      this.pnlStackingOrder.PerformLayout();
      this.pnlDragDrop.ResumeLayout(false);
      this.pnlClose.ResumeLayout(false);
      this.pnlRight.ResumeLayout(false);
      this.pnlViewer.ResumeLayout(false);
      this.gcInfo.ResumeLayout(false);
      this.tabInfo.ResumeLayout(false);
      this.pageFiles.ResumeLayout(false);
      this.pageDocumentDetails.ResumeLayout(false);
      this.gcDocumentCommentText.ResumeLayout(false);
      this.gcDocumentCommentText.PerformLayout();
      this.gcDocumentComments.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.gcDocumentStatus.ResumeLayout(false);
      this.pnlStatus.ResumeLayout(false);
      this.pnlStatus.PerformLayout();
      ((ISupportInitialize) this.btnDateShippingReady).EndInit();
      ((ISupportInitialize) this.btnDateUnderwritingReady).EndInit();
      ((ISupportInitialize) this.btnDocumentDateReviewed).EndInit();
      ((ISupportInitialize) this.btnDocumentDateReceived).EndInit();
      ((ISupportInitialize) this.btnDocumentDateRerequested).EndInit();
      ((ISupportInitialize) this.btnDocumentDateRequested).EndInit();
      this.gcDocumentDetails.ResumeLayout(false);
      this.pnlDetails.ResumeLayout(false);
      this.pnlDetails.PerformLayout();
      this.pageConditionDetails.ResumeLayout(false);
      this.gcConditionCommentText.ResumeLayout(false);
      this.gcConditionCommentText.PerformLayout();
      this.gcConditionComments.ResumeLayout(false);
      this.flowLayoutPanel2.ResumeLayout(false);
      this.gcConditionStatus.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.gcConditionDetails.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.pageConditionsEnhanced.ResumeLayout(false);
      this.panel5.ResumeLayout(false);
      this.panel5.PerformLayout();
      this.groupContainer1.ResumeLayout(false);
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      this.gcEnhancedConditionDetails.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
