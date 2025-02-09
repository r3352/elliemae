// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Documents.DocumentDetailsDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Conditions;
using EllieMae.EMLite.eFolder.Files;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.eFolder.UI;
using EllieMae.EMLite.eFolder.Utilities;
using EllieMae.EMLite.eFolder.Viewers;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.LoanUtils.EnhancedConditions;
using EllieMae.EMLite.LoanUtils.Services;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Documents
{
  public class DocumentDetailsDialog : Form
  {
    private const string className = "DocumentDetailsDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private static List<DocumentDetailsDialog> _instanceList = new List<DocumentDetailsDialog>();
    private LoanDataMgr loanDataMgr;
    private DocumentLog doc;
    private DocumentGroupSetup groupSetup;
    private DocumentTrackingSetup docSetup;
    private GridViewDataManager gvFilesMgr;
    private GVItem previousFileDropItem;
    private Hashtable roleTbl;
    private eFolderAccessRights rights;
    private bool isHideFileSizeEnabled;
    private bool refreshFiles;
    private IContainer components;
    private Panel pnlLeft;
    private GroupContainer gcTracking;
    private GroupContainer gcDetails;
    private GroupContainer gcFiles;
    private CollapsibleSplitter csLeft;
    private TabControl tabTracking;
    private TabPage pageStatus;
    private TabPage pageComments;
    private ComboBox cboTitle;
    private Label lblTitle;
    private ComboBox cboBorrower;
    private Label lblBorrower;
    private Label lblGroups;
    private ListBox lstGroups;
    private TextBox txtAccess;
    private Button btnAccess;
    private Label lblMilestone;
    private Label lblConditions;
    private ListBox lstConditions;
    private ComboBoxEx cboMilestone;
    private FileAttachmentViewerControl fileViewer;
    private Label lblDaysDue;
    private TextBox txtDateDue;
    private TextBox txtDaysDue;
    private TextBox txtDateExpire;
    private TextBox txtDaysExpire;
    private Label lblDaysExpire;
    private TextBox txtRequestedFrom;
    private Label lblCompany;
    private CheckBox chkRequested;
    private DateTimePicker dateRequested;
    private DateTimePicker dateRerequested;
    private CheckBox chkRerequested;
    private DateTimePicker dateReceived;
    private CheckBox chkReceived;
    private StandardIconButton btnRemoveFile;
    private StandardIconButton btnMoveFileDown;
    private StandardIconButton btnMoveFileUp;
    private Panel pnlRight;
    private CollapsibleSplitter csFiles;
    private BorderPanel pnlViewer;
    private CollapsibleSplitter csDetails;
    private Panel pnlDetails;
    private GridView gvFiles;
    private Panel pnlClose;
    private Button btnClose;
    private CommentCollectionControl commentCollection;
    private TextBox txtTitle;
    private TextBox txtBorrower;
    private TextBox txtMilestone;
    private Label lblAccess;
    private TextBox txtDateRequested;
    private TextBox txtDateReceived;
    private TextBox txtDateRerequested;
    private FlowLayoutPanel pnlToolbar;
    private IconButton btnAttachUnassigned;
    private IconButton btnAttachBrowse;
    private IconButton btnAttachScan;
    private IconButton btnAttachForms;
    private ToolTip tooltip;
    private EMHelpLink helpLink;
    private IconButton btnMergeFiles;
    private Label lblAvailable;
    private VerticalSeparator separator;
    private Button btnConditions;
    private DateTimePicker dateReviewed;
    private CheckBox chkReviewed;
    private TextBox txtReviewedBy;
    private TextBox txtReceivedBy;
    private TextBox txtRerequestedBy;
    private TextBox txtRequestedBy;
    private TextBox txtDateReviewed;
    private StandardIconButton btnReviewedBy;
    private StandardIconButton btnReceivedBy;
    private StandardIconButton btnRerequestedBy;
    private StandardIconButton btnRequestedBy;
    private StandardIconButton btnShippingReadyBy;
    private TextBox txtShippingReadyBy;
    private DateTimePicker dateShippingReady;
    private CheckBox chkShippingReady;
    private StandardIconButton btnUnderwritingReadyBy;
    private TextBox txtUnderwritingReadyBy;
    private DateTimePicker dateUnderwritingReady;
    private CheckBox chkUnderwritingReady;
    private TextBox txtDateUnderwritingReady;
    private TextBox txtDateShippingReady;
    private TextBox txtDescription;
    private Label lblDescription;
    private CheckBox chkThirdParty;
    private CheckBox chkTPOWebcenterPortal;
    private CheckBox chkWebCenter;
    private Button btnATRQM;
    private ListBox lstATRQM;
    private Button btnFinalCD;

    public static void ShowInstance(LoanDataMgr loanDataMgr, DocumentLog doc)
    {
      if (Form.ActiveForm != null && Form.ActiveForm.Modal)
      {
        using (DocumentDetailsDialog documentDetailsDialog = new DocumentDetailsDialog(loanDataMgr, doc))
        {
          int num = (int) documentDetailsDialog.ShowDialog((IWin32Window) Form.ActiveForm);
        }
      }
      else
      {
        DocumentDetailsDialog documentDetailsDialog1 = (DocumentDetailsDialog) null;
        foreach (DocumentDetailsDialog instance in DocumentDetailsDialog._instanceList)
        {
          if (instance.Document == doc)
            documentDetailsDialog1 = instance;
        }
        if (documentDetailsDialog1 == null)
        {
          DocumentDetailsDialog documentDetailsDialog2 = new DocumentDetailsDialog(loanDataMgr, doc);
          documentDetailsDialog2.FormClosing += new FormClosingEventHandler(DocumentDetailsDialog._instance_FormClosing);
          documentDetailsDialog2.Show();
          DocumentDetailsDialog._instanceList.Add(documentDetailsDialog2);
        }
        else
        {
          if (documentDetailsDialog1.WindowState == FormWindowState.Minimized)
            documentDetailsDialog1.WindowState = FormWindowState.Normal;
          documentDetailsDialog1.Activate();
        }
      }
    }

    public static void CloseInstances()
    {
      if (DocumentDetailsDialog._instanceList == null && DocumentDetailsDialog._instanceList.Count == 0)
        return;
      int num = DocumentDetailsDialog._instanceList.Count - 1;
      try
      {
        for (int index = num; index >= 0; --index)
          DocumentDetailsDialog._instanceList[index].Close();
      }
      catch
      {
      }
    }

    private static void _instance_FormClosing(object sender, FormClosingEventArgs e)
    {
      DocumentDetailsDialog documentDetailsDialog = (DocumentDetailsDialog) sender;
      DocumentDetailsDialog._instanceList.Remove(documentDetailsDialog);
    }

    private DocumentDetailsDialog(LoanDataMgr loanDataMgr, DocumentLog doc)
    {
      this.InitializeComponent();
      this.setWindowSize();
      this.loanDataMgr = loanDataMgr;
      this.doc = AutoAssignUtils.IsNGAutoAssignEnabled ? loanDataMgr.LoanData.GetLogList().GetRecordByID(doc.Guid) as DocumentLog : doc;
      this.rights = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) doc);
      this.isHideFileSizeEnabled = string.Equals(Session.ConfigurationManager.GetCompanySetting("eFolder", "HideFileSize"), "TRUE", StringComparison.OrdinalIgnoreCase);
      this.initEventHandlers();
      this.initTitleField();
      this.initBorrowerField();
      this.initMilestoneField();
      this.initAccessField();
      this.initGroupsField();
      this.initFileList();
      this.initAccessedBy();
      this.loadDocumentDetails();
      this.loadFileList(true);
      this.loadFinalCDButton();
      this.applySecurity();
      int[] usersAssignedRoles = loanDataMgr.AccessRules.GetUsersAssignedRoles();
      if (doc.Comments.HasUnreviewedEntry(usersAssignedRoles))
      {
        doc.Comments.MarkAsReviewed(DateTime.Now, Session.UserID, usersAssignedRoles);
        this.tabTracking.SelectedTab = this.pageComments;
      }
      if (!(doc.Title != "Untitled"))
        return;
      this.fileViewer.Select();
    }

    private void setWindowSize()
    {
      if (Form.ActiveForm != null)
      {
        Form form = Form.ActiveForm;
        while (form.Owner != null)
          form = form.Owner;
        this.Width = Convert.ToInt32((double) form.Width * 0.95);
        this.Height = Convert.ToInt32((double) form.Height * 0.95);
      }
      else
      {
        Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Width = Convert.ToInt32((double) workingArea.Width * 0.95);
        workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Height = Convert.ToInt32((double) workingArea.Height * 0.95);
      }
    }

    public DocumentLog Document => this.doc;

    private void loadDocumentDetails()
    {
      this.loadTitleField();
      this.loadDescriptionField();
      this.loadBorrowerField();
      this.loadMilestoneField();
      this.loadAccessField();
      this.loadConditionsField();
      this.loadATRQMField();
      this.loadGroupsField();
      this.loadExternalField();
      this.loadFinalCDButton();
      this.loadExpectedFields();
      this.loadExpirationFields();
      this.loadRequestedFromField();
      this.loadRequestedFields();
      this.loadRerequestedFields();
      this.loadReceivedFields();
      this.loadReviewedFields();
      this.loadUnderwritingReadyFields();
      this.loadShippingReadyFields();
      this.loadCommentList();
    }

    private void initTitleField()
    {
      this.docSetup = this.loanDataMgr.SystemConfiguration.DocumentTrackingSetup;
      foreach (DocumentTemplate documentTemplate in this.docSetup)
        this.cboTitle.Items.Add((object) documentTemplate.Name);
    }

    private void loadTitleField()
    {
      this.cboTitle.Text = this.doc.Title;
      this.txtTitle.Text = this.doc.Title;
      this.Text = "Document Details (" + this.doc.Title + ")";
    }

    private void cboTitle_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!(this.cboTitle.Text != this.doc.Title))
        return;
      this.updateDocumentFields();
    }

    private void cboTitle_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (!(this.cboTitle.Text != this.doc.Title))
        return;
      this.updateDocumentFields();
    }

    private void updateDocumentFields()
    {
      string empty = string.Empty;
      string name = this.cboTitle.SelectedIndex >= 0 ? (string) this.cboTitle.SelectedItem : this.cboTitle.Text;
      this.Text = "Document Details (" + name + ")";
      using (CursorActivator.Wait())
      {
        this.doc.Title = name;
        this.loanDataMgr.LinkDocument(this.doc);
        DocumentTemplate byName = this.docSetup.GetByName(name);
        if (byName == null)
          return;
        this.doc.Description = byName.Description;
        this.doc.DaysDue = byName.DaysTillDue;
        this.doc.DaysTillExpire = byName.DaysTillExpire;
        this.doc.IsWebcenter = byName.IsWebcenter;
        this.doc.IsTPOWebcenterPortal = byName.IsTPOWebcenterPortal;
        this.doc.IsThirdPartyDoc = byName.IsThirdPartyDoc;
        this.loadFinalCDButton();
      }
    }

    private void cboTitle_Validating(object sender, CancelEventArgs e)
    {
      if (this.cboTitle.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must specify a name for the document.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        e.Cancel = true;
      }
      if (!(this.cboTitle.Text != this.doc.Title))
        return;
      this.updateDocumentFields();
    }

    private void cboTitle_Validated(object sender, EventArgs e)
    {
      if (this.doc.Title == this.cboTitle.Text)
        return;
      this.doc.Title = this.cboTitle.Text;
    }

    private void loadDescriptionField() => this.txtDescription.Text = this.doc.Description;

    private void txtDescription_Validated(object sender, EventArgs e)
    {
      if (!(this.doc.Description != this.txtDescription.Text))
        return;
      this.doc.Description = this.txtDescription.Text;
    }

    private void initBorrowerField()
    {
      this.cboBorrower.Items.AddRange((object[]) this.loanDataMgr.LoanData.GetBorrowerPairs());
      this.cboBorrower.Items.Add((object) BorrowerPair.All);
    }

    private void loadBorrowerField()
    {
      this.cboBorrower.SelectedItem = (object) null;
      foreach (BorrowerPair borrowerPair in this.cboBorrower.Items)
      {
        if (borrowerPair.Id == this.doc.PairId)
          this.cboBorrower.SelectedItem = (object) borrowerPair;
      }
      this.txtBorrower.Text = this.cboBorrower.Text;
    }

    private void cboBorrower_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.doc.PairId = ((BorrowerPair) this.cboBorrower.SelectedItem).Id;
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
      MilestoneTemplatesBpmManager bpmManager = (MilestoneTemplatesBpmManager) Session.BPM.GetBpmManager(BpmCategory.Milestones);
      LogList logList = this.loanDataMgr.LoanData.GetLogList();
      MilestoneLog milestoneByName = logList.GetMilestoneByName(this.doc.Stage);
      EllieMae.EMLite.Workflow.Milestone milestone = (EllieMae.EMLite.Workflow.Milestone) null;
      if (milestoneByName != null)
        milestone = bpmManager.GetMilestoneByID(milestoneByName.MilestoneID, milestoneByName.Stage, false, milestoneByName.Days, milestoneByName.DoneText, milestoneByName.ExpText, milestoneByName.RoleRequired == "Y", milestoneByName.RoleID);
      if (milestone == null)
      {
        milestone = bpmManager.GetMilestoneByName(this.doc.Stage);
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
      EllieMae.EMLite.Workflow.Milestone tag = (EllieMae.EMLite.Workflow.Milestone) ((Element) this.cboMilestone.SelectedItem).Tag;
      if (tag == null)
        return;
      MilestoneLog milestoneById = this.loanDataMgr.LoanData.GetLogList().GetMilestoneByID(tag.MilestoneID);
      if (milestoneById == null)
        return;
      this.doc.Stage = milestoneById.Stage;
    }

    private void initAccessField()
    {
      this.roleTbl = new Hashtable();
      foreach (RoleInfo allRole in Session.LoanDataMgr.SystemConfiguration.AllRoles)
        this.roleTbl[(object) allRole.ID] = (object) allRole.RoleAbbr;
      this.roleTbl[(object) RoleInfo.Others.ID] = (object) RoleInfo.Others.RoleAbbr;
    }

    private void loadAccessField()
    {
      ArrayList arrayList = new ArrayList();
      foreach (int allowedRole in this.doc.AllowedRoles)
      {
        if (this.roleTbl.Contains((object) allowedRole))
          arrayList.Add(this.roleTbl[(object) allowedRole]);
      }
      if (arrayList.Count > 0)
        arrayList.Sort((IComparer) new CaseInsensitiveComparer());
      else
        arrayList.Add((object) RoleInfo.All.RoleAbbr);
      this.txtAccess.Text = string.Join(", ", (string[]) arrayList.ToArray(typeof (string)));
    }

    private void btnAccess_Click(object sender, EventArgs e)
    {
      using (DocumentAccessDialog documentAccessDialog = new DocumentAccessDialog(this.loanDataMgr, new DocumentLog[1]
      {
        this.doc
      }))
      {
        int num = (int) documentAccessDialog.ShowDialog((IWin32Window) this);
      }
    }

    private void loadConditionsField()
    {
      this.lstConditions.Items.Clear();
      foreach (ConditionLog condition in this.doc.Conditions)
        this.lstConditions.Items.Add((object) condition.Title);
    }

    private void btnConditions_Click(object sender, EventArgs e)
    {
      using (AssignConditionsDialog conditionsDialog = new AssignConditionsDialog(this.loanDataMgr, this.doc))
      {
        int num = (int) conditionsDialog.ShowDialog((IWin32Window) this);
      }
    }

    private void lstConditions_MouseHover(object sender, EventArgs e)
    {
      string caption = string.Empty;
      foreach (object obj in this.lstConditions.Items)
        caption = !string.IsNullOrEmpty(caption) ? caption + "\r\n" + obj.ToString() : obj.ToString();
      this.tooltip.SetToolTip((Control) this.lstConditions, caption);
    }

    private void loadATRQMField()
    {
      this.lstATRQM.Items.Clear();
      if (this.doc.IsEmploymentVerification)
        this.lstATRQM.Items.Add((object) "Employment Status Verification");
      if (this.doc.IsObligationVerification)
        this.lstATRQM.Items.Add((object) "Monthly Obligation Verification");
      if (this.doc.IsIncomeVerification)
        this.lstATRQM.Items.Add((object) "Income Verification");
      if (!this.doc.IsAssetVerification)
        return;
      this.lstATRQM.Items.Add((object) "Asset Verification");
    }

    private void btnATRQM_Click(object sender, EventArgs e)
    {
      using (ATRQMDialog atrqmDialog = new ATRQMDialog(this.doc, VerificationTimelineType.None))
      {
        if (atrqmDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.loadATRQMField();
      }
    }

    private void initGroupsField()
    {
      this.groupSetup = this.loanDataMgr.SystemConfiguration.DocumentGroupSetup;
    }

    private void loadGroupsField()
    {
      this.lstGroups.Items.Clear();
      DocumentTemplate byName = this.docSetup.GetByName(this.doc.Title);
      if (byName == null)
        return;
      foreach (DocumentGroup documentGroup in (CollectionBase) this.groupSetup)
      {
        if (documentGroup.Contains(byName))
          this.lstGroups.Items.Add((object) documentGroup);
      }
    }

    private void loadExternalField()
    {
      this.chkWebCenter.Checked = this.doc.IsWebcenter;
      this.chkTPOWebcenterPortal.Checked = this.doc.IsTPOWebcenterPortal;
      this.chkThirdParty.Checked = this.doc.IsThirdPartyDoc;
    }

    private void chkWebCenter_Click(object sender, EventArgs e)
    {
      this.doc.IsWebcenter = this.chkWebCenter.Checked;
    }

    private void chkTPOWebcenterPortal_Click(object sender, EventArgs e)
    {
      this.doc.IsTPOWebcenterPortal = this.chkTPOWebcenterPortal.Checked;
    }

    private void chkEDMLenders_Click(object sender, EventArgs e)
    {
      this.doc.IsThirdPartyDoc = this.chkThirdParty.Checked;
    }

    private void loadExpectedFields()
    {
      if (this.doc.DaysDue > 0)
        this.txtDaysDue.Text = this.doc.DaysDue.ToString();
      else
        this.txtDaysDue.Text = string.Empty;
      if (this.doc.Expected)
        this.txtDateDue.Text = this.doc.DateExpected.ToString("MM/dd/yy");
      else
        this.txtDateDue.Text = string.Empty;
    }

    private void txtDaysDue_Validating(object sender, CancelEventArgs e)
    {
      if (!(this.txtDaysDue.Text != string.Empty) || Utils.IsInt((object) this.txtDaysDue.Text))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "You must specify a valid number for the Days to Receive.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      e.Cancel = true;
    }

    private void txtDaysDue_Validated(object sender, EventArgs e)
    {
      int num = Utils.ParseInt((object) this.txtDaysDue.Text, 0);
      if (this.doc.DaysDue == num)
        return;
      this.doc.DaysDue = num;
    }

    private void loadExpirationFields()
    {
      if (this.doc.DaysTillExpire > 0)
        this.txtDaysExpire.Text = this.doc.DaysTillExpire.ToString();
      else
        this.txtDaysExpire.Text = string.Empty;
      if (this.doc.Expires)
        this.txtDateExpire.Text = this.doc.DateExpires.ToString("MM/dd/yy");
      else
        this.txtDateExpire.Text = string.Empty;
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
      int num = Utils.ParseInt((object) this.txtDaysExpire.Text, 0);
      if (this.doc.DaysTillExpire == num)
        return;
      this.doc.DaysTillExpire = num;
    }

    private void loadRequestedFromField() => this.txtRequestedFrom.Text = this.doc.RequestedFrom;

    private void txtRequestedFrom_Validated(object sender, EventArgs e)
    {
      if (!(this.doc.RequestedFrom != this.txtRequestedFrom.Text))
        return;
      this.doc.RequestedFrom = this.txtRequestedFrom.Text;
    }

    private void loadRequestedFields()
    {
      this.chkRequested.Checked = this.doc.Requested;
      if (this.chkRequested.Checked)
      {
        this.dateRequested.Value = this.doc.DateRequested;
        this.txtDateRequested.Text = this.doc.DateRequested.ToString(this.dateRequested.CustomFormat);
      }
      this.dateRequested.Visible = this.chkRequested.Checked && this.rights.CanEditDocument;
      this.txtDateRequested.Visible = this.chkRequested.Checked && !this.rights.CanEditDocument;
      this.txtRequestedBy.Text = this.doc.RequestedBy;
      this.txtRequestedBy.Visible = this.chkRequested.Checked;
      this.btnRequestedBy.Visible = this.chkRequested.Checked && this.rights.CanEditDocument;
    }

    private void chkRequested_Click(object sender, EventArgs e)
    {
      if (this.chkRequested.Checked)
        this.doc.MarkAsRequested(DateTime.Now, Session.UserID);
      else
        this.doc.UnmarkAsRequested();
    }

    private void dateRequested_ValueChanged(object sender, EventArgs e)
    {
      if (!(this.dateRequested.Value != this.doc.DateRequested))
        return;
      this.doc.MarkAsRequested(this.dateRequested.Value, this.doc.RequestedBy);
    }

    private void btnRequestedBy_Click(object sender, EventArgs e)
    {
      string user = this.selectUser();
      if (user == null)
        return;
      this.doc.MarkAsRequested(this.dateRequested.Value, user);
    }

    private void loadRerequestedFields()
    {
      this.chkRerequested.Checked = this.doc.Rerequested;
      if (this.chkRerequested.Checked)
      {
        this.dateRerequested.Value = this.doc.DateRerequested;
        this.txtDateRerequested.Text = this.doc.DateRerequested.ToString(this.dateRerequested.CustomFormat);
      }
      this.dateRerequested.Visible = this.chkRerequested.Checked && this.rights.CanEditDocument;
      this.txtDateRerequested.Visible = this.chkRerequested.Checked && !this.rights.CanEditDocument;
      this.txtRerequestedBy.Text = this.doc.RerequestedBy;
      this.txtRerequestedBy.Visible = this.chkRerequested.Checked;
      this.btnRerequestedBy.Visible = this.chkRerequested.Checked && this.rights.CanEditDocument;
    }

    private void chkRerequested_Click(object sender, EventArgs e)
    {
      if (this.chkRerequested.Checked)
        this.doc.MarkAsRerequested(DateTime.Now, Session.UserID);
      else
        this.doc.UnmarkAsRerequested();
    }

    private void dateRerequested_ValueChanged(object sender, EventArgs e)
    {
      if (!(this.dateRerequested.Value != this.doc.DateRerequested))
        return;
      this.doc.MarkAsRerequested(this.dateRerequested.Value, this.doc.RerequestedBy);
    }

    private void btnRerequestedBy_Click(object sender, EventArgs e)
    {
      string user = this.selectUser();
      if (user == null)
        return;
      this.doc.MarkAsRerequested(this.dateRerequested.Value, user);
    }

    private void loadReceivedFields()
    {
      this.chkReceived.Checked = this.doc.Received;
      if (this.chkReceived.Checked)
      {
        this.dateReceived.Value = this.doc.DateReceived;
        this.txtDateReceived.Text = this.doc.DateReceived.ToString(this.dateReceived.CustomFormat);
      }
      this.dateReceived.Visible = this.chkReceived.Checked && this.rights.CanEditDocument;
      this.txtDateReceived.Visible = this.chkReceived.Checked && !this.rights.CanEditDocument;
      this.txtReceivedBy.Text = this.doc.ReceivedBy;
      this.txtReceivedBy.Visible = this.chkReceived.Checked;
      this.btnReceivedBy.Visible = this.chkReceived.Checked && this.rights.CanEditDocument;
    }

    private void chkReceived_Click(object sender, EventArgs e)
    {
      if (this.chkReceived.Checked)
        this.doc.MarkAsReceived(DateTime.Now, Session.UserID);
      else
        this.doc.UnmarkAsReceived();
    }

    private void dateReceived_ValueChanged(object sender, EventArgs e)
    {
      if (!(this.dateReceived.Value != this.doc.DateReceived))
        return;
      this.doc.MarkAsReceived(this.dateReceived.Value, this.doc.ReceivedBy);
    }

    private void btnReceivedBy_Click(object sender, EventArgs e)
    {
      string user = this.selectUser();
      if (user == null)
        return;
      this.doc.MarkAsReceived(this.dateReceived.Value, user);
    }

    private void loadReviewedFields()
    {
      this.chkReviewed.Checked = this.doc.Reviewed;
      if (this.chkReviewed.Checked)
      {
        this.dateReviewed.Value = this.doc.DateReviewed;
        this.txtDateReviewed.Text = this.doc.DateReviewed.ToString(this.dateReviewed.CustomFormat);
      }
      this.dateReviewed.Visible = this.chkReviewed.Checked && this.rights.CanReviewDocuments;
      this.txtDateReviewed.Visible = this.chkReviewed.Checked && !this.rights.CanReviewDocuments;
      this.txtReviewedBy.Text = this.doc.ReviewedBy;
      this.txtReviewedBy.Visible = this.chkReviewed.Checked;
      this.btnReviewedBy.Visible = this.chkReviewed.Checked && this.rights.CanReviewDocuments;
    }

    private void chkReviewed_Click(object sender, EventArgs e)
    {
      if (this.chkReviewed.Checked)
        this.doc.MarkAsReviewed(DateTime.Now, Session.UserID);
      else
        this.doc.UnmarkAsReviewed();
    }

    private void dateReviewed_ValueChanged(object sender, EventArgs e)
    {
      if (!(this.dateReviewed.Value != this.doc.DateReviewed))
        return;
      this.doc.MarkAsReviewed(this.dateReviewed.Value, this.doc.ReviewedBy);
    }

    private void btnReviewedBy_Click(object sender, EventArgs e)
    {
      string user = this.selectUser();
      if (user == null)
        return;
      this.doc.MarkAsReviewed(this.dateReviewed.Value, user);
    }

    private void loadUnderwritingReadyFields()
    {
      this.chkUnderwritingReady.Checked = this.doc.UnderwritingReady;
      if (this.chkUnderwritingReady.Checked)
      {
        this.dateUnderwritingReady.Value = this.doc.DateUnderwritingReady;
        this.txtDateUnderwritingReady.Text = this.doc.DateUnderwritingReady.ToString(this.dateUnderwritingReady.CustomFormat);
      }
      this.dateUnderwritingReady.Visible = this.chkUnderwritingReady.Checked && this.rights.CanEditDocument;
      this.txtDateUnderwritingReady.Visible = this.chkUnderwritingReady.Checked && !this.rights.CanEditDocument;
      this.txtUnderwritingReadyBy.Text = this.doc.UnderwritingReadyBy;
      this.txtUnderwritingReadyBy.Visible = this.chkUnderwritingReady.Checked;
      this.btnUnderwritingReadyBy.Visible = this.chkUnderwritingReady.Checked && this.rights.CanEditDocument;
    }

    private void chkUnderwritingReady_Click(object sender, EventArgs e)
    {
      if (this.chkUnderwritingReady.Checked)
        this.doc.MarkAsUnderwritingReady(DateTime.Now, Session.UserID);
      else
        this.doc.UnmarkAsUnderwritingReady();
    }

    private void dateUnderwritingReady_ValueChanged(object sender, EventArgs e)
    {
      if (!(this.dateUnderwritingReady.Value != this.doc.DateUnderwritingReady))
        return;
      this.doc.MarkAsUnderwritingReady(this.dateUnderwritingReady.Value, this.doc.UnderwritingReadyBy);
    }

    private void btnUnderwritingReadyBy_Click(object sender, EventArgs e)
    {
      string user = this.selectUser();
      if (user == null)
        return;
      this.doc.MarkAsUnderwritingReady(this.dateUnderwritingReady.Value, user);
    }

    private void loadShippingReadyFields()
    {
      this.chkShippingReady.Checked = this.doc.ShippingReady;
      if (this.chkShippingReady.Checked)
      {
        this.dateShippingReady.Value = this.doc.DateShippingReady;
        this.txtDateShippingReady.Text = this.doc.DateShippingReady.ToString(this.dateShippingReady.CustomFormat);
      }
      this.dateShippingReady.Visible = this.chkShippingReady.Checked && this.rights.CanEditDocument;
      this.txtDateShippingReady.Visible = this.chkShippingReady.Checked && !this.rights.CanEditDocument;
      this.txtShippingReadyBy.Text = this.doc.ShippingReadyBy;
      this.txtShippingReadyBy.Visible = this.chkShippingReady.Checked;
      this.btnShippingReadyBy.Visible = this.chkShippingReady.Checked && this.rights.CanEditDocument;
    }

    private void chkShippingReady_Click(object sender, EventArgs e)
    {
      if (this.chkShippingReady.Checked)
        this.doc.MarkAsShippingReady(DateTime.Now, Session.UserID);
      else
        this.doc.UnmarkAsShippingReady();
    }

    private void dateShippingReady_ValueChanged(object sender, EventArgs e)
    {
      if (!(this.dateShippingReady.Value != this.doc.DateShippingReady))
        return;
      this.doc.MarkAsShippingReady(this.dateShippingReady.Value, this.doc.ShippingReadyBy);
    }

    private void btnShippingReadyBy_Click(object sender, EventArgs e)
    {
      string user = this.selectUser();
      if (user == null)
        return;
      this.doc.MarkAsShippingReady(this.dateShippingReady.Value, user);
    }

    private void loadCommentList()
    {
      this.commentCollection.LoadComments(this.loanDataMgr, this.doc.Comments);
    }

    private void initAccessedBy() => this.doc.MarkAsAccessed(DateTime.Now, Session.UserID);

    private void initFileList()
    {
      this.gvFilesMgr = new GridViewDataManager(this.gvFiles, this.loanDataMgr);
      TableLayout.Column[] columnList;
      if (this.rights.CanAccessAIQFeatures)
        columnList = new TableLayout.Column[9]
        {
          GridViewDataManager.NameWithIconColumn,
          GridViewDataManager.AIQAdditionalInfoColumn,
          GridViewDataManager.DateTimeColumn,
          GridViewDataManager.FileSizeColumn,
          GridViewDataManager.ActiveColumn,
          GridViewDataManager.AIQTagsColumn,
          GridViewDataManager.AIQStatusColumn,
          GridViewDataManager.AIQLastUpdatedColumn,
          GridViewDataManager.AIQIDColumn
        };
      else
        columnList = new TableLayout.Column[4]
        {
          GridViewDataManager.NameWithIconColumn,
          GridViewDataManager.DateTimeColumn,
          GridViewDataManager.FileSizeColumn,
          GridViewDataManager.ActiveColumn
        };
      this.gvFilesMgr.CreateLayout(columnList);
      this.gvFiles.Columns[0].ActivatedEditorType = GVActivatedEditorType.TextBox;
      if (!this.isHideFileSizeEnabled)
        return;
      if (this.rights.CanAccessAIQFeatures)
        this.gvFiles.Columns[3].Width = 0;
      else
        this.gvFiles.Columns[2].Width = 0;
    }

    private void loadFileList(bool showAll)
    {
      List<string> stringList = new List<string>();
      foreach (GVItem selectedItem in this.gvFiles.SelectedItems)
        stringList.Add(((FileAttachment) selectedItem.Tag).ID);
      this.gvFilesMgr.ClearItems();
      foreach (FileAttachmentReference file in (CollectionBase) this.doc.Files)
      {
        FileAttachment attachment = this.loanDataMgr.FileAttachments.GetAttachment(file);
        if (attachment != null)
        {
          bool flag = false;
          if (stringList.Contains(attachment.ID))
            flag = true;
          this.gvFilesMgr.AddItem(attachment, file).Selected = flag || showAll && file.IsActive;
        }
      }
      if (!showAll)
        return;
      this.showDocumentFiles();
    }

    private FileAttachment[] getSelectedFiles()
    {
      List<FileAttachment> fileAttachmentList = new List<FileAttachment>();
      foreach (GVItem selectedItem in this.gvFiles.SelectedItems)
        fileAttachmentList.Add((FileAttachment) selectedItem.Tag);
      return fileAttachmentList.ToArray();
    }

    private void showDocumentFiles()
    {
      FileAttachment[] selectedFiles = this.getSelectedFiles();
      if (selectedFiles.Length != 0)
        this.fileViewer.LoadFiles(selectedFiles);
      else
        this.fileViewer.CloseFile();
      this.refreshToolbar();
    }

    private void showDocumentFiles(FileAttachment file)
    {
      this.showDocumentFiles(new FileAttachment[1]{ file });
    }

    private void showDocumentFiles(FileAttachment[] fileList)
    {
      this.gvFiles.SelectedItems.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvFiles.Items)
      {
        if (Array.IndexOf<object>((object[]) fileList, gvItem.Tag) >= 0)
          gvItem.Selected = true;
      }
      this.showDocumentFiles();
    }

    private void gvFiles_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      string title = e.EditorControl.Text.Trim();
      this.gvFiles.CancelEditing();
      if (!(title != e.SubItem.Text) || !(title != string.Empty))
        return;
      FileAttachment tag = (FileAttachment) e.SubItem.Item.Tag;
      if (tag is CloudAttachment)
      {
        try
        {
          Task.WaitAll((Task) new EBSRestClient(this.loanDataMgr).SetAttachmentTitle((CloudAttachment) tag, title));
        }
        catch (Exception ex)
        {
          Tracing.Log(DocumentDetailsDialog.sw, nameof (DocumentDetailsDialog), TraceLevel.Error, "Exception in SetAttachmentTitle: " + ex.Message);
        }
      }
      else
        tag.Title = title;
    }

    private void gvFiles_ItemDoubleClick(object source, GVItemEventArgs e)
    {
    }

    private void gvFiles_BeforeSelectedIndexCommitted(object sender, CancelEventArgs e)
    {
      if (this.fileViewer.CanCloseViewer())
        return;
      e.Cancel = true;
    }

    private void gvFiles_SelectedIndexCommitted(object sender, EventArgs e)
    {
      this.showDocumentFiles();
    }

    private void gvFiles_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      FileAttachmentReference file = this.doc.Files[((FileAttachment) e.SubItem.Item.Tag).ID];
      if (file == null)
        return;
      if (e.SubItem.Checked)
        file.MarkAsActive(Session.UserID);
      else
        file.UnmarkAsActive();
    }

    private void refreshToolbar()
    {
      int count1 = this.gvFiles.Items.Count;
      int count2 = this.gvFiles.SelectedItems.Count;
      this.btnMoveFileUp.Enabled = count2 > 0 && count2 < count1;
      this.btnMoveFileDown.Enabled = count2 > 0 && count2 < count1;
      this.btnRemoveFile.Enabled = count2 > 0;
      this.btnMergeFiles.Enabled = count2 > 1;
    }

    private void btnAttachBrowse_Click(object sender, EventArgs e)
    {
      if (!this.fileViewer.CanCloseViewer())
        return;
      using (AttachFilesDialog attachFilesDialog = new AttachFilesDialog(this.loanDataMgr))
      {
        FileAttachment[] fileList = attachFilesDialog.AttachFiles(this.doc);
        if (fileList == null)
          return;
        this.showDocumentFiles(fileList);
      }
    }

    private void btnAttachScan_Click(object sender, EventArgs e)
    {
      if (!this.fileViewer.CanCloseViewer())
        return;
      using (ScanFileDialog scanFileDialog = new ScanFileDialog(this.loanDataMgr, this.doc))
      {
        if (scanFileDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.showDocumentFiles(scanFileDialog.Files);
      }
    }

    private void btnAttachForms_Click(object sender, EventArgs e)
    {
      if (!this.fileViewer.CanCloseViewer())
        return;
      using (AttachFormsDialog attachFormsDialog = new AttachFormsDialog(this.loanDataMgr, this.doc))
      {
        if (attachFormsDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.showDocumentFiles(attachFormsDialog.Files);
      }
    }

    private void btnAttachUnassigned_Click(object sender, EventArgs e)
    {
      if (!this.fileViewer.CanCloseViewer())
        return;
      using (AssignFilesDialog assignFilesDialog = new AssignFilesDialog(this.loanDataMgr, this.doc))
      {
        if (assignFilesDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.showDocumentFiles(assignFilesDialog.Files);
      }
    }

    private void btnMoveFileUp_Click(object sender, EventArgs e)
    {
      if (!this.loanDataMgr.LockLoanWithExclusiveA())
        return;
      FileAttachment fileAttachment = (FileAttachment) null;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvFiles.Items)
      {
        FileAttachment tag = (FileAttachment) gvItem.Tag;
        if (!gvItem.Selected)
          fileAttachment = tag;
        else if (fileAttachment != null)
          this.doc.Files.Swap(tag.ID, fileAttachment.ID);
      }
      this.showDocumentFiles();
    }

    private void btnMoveFileDown_Click(object sender, EventArgs e)
    {
      if (!this.loanDataMgr.LockLoanWithExclusiveA())
        return;
      FileAttachment fileAttachment = (FileAttachment) null;
      for (int nItemIndex = this.gvFiles.Items.Count - 1; nItemIndex >= 0; --nItemIndex)
      {
        FileAttachment tag = (FileAttachment) this.gvFiles.Items[nItemIndex].Tag;
        if (!this.gvFiles.Items[nItemIndex].Selected)
          fileAttachment = tag;
        else if (fileAttachment != null)
          this.doc.Files.Swap(tag.ID, fileAttachment.ID);
      }
      this.showDocumentFiles();
    }

    private void btnRemoveFile_Click(object sender, EventArgs e)
    {
      FileAttachment[] selectedFiles = this.getSelectedFiles();
      string str = string.Empty;
      foreach (FileAttachment fileAttachment in selectedFiles)
        str = str + fileAttachment.Title + "\r\n";
      if (Utils.Dialog((IWin32Window) this, "Are you sure that you want to remove the following file(s):\r\n\r\n" + str, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
      {
        foreach (FileAttachment fileAttachment in selectedFiles)
          this.doc.Files.Remove(fileAttachment.ID);
      }
      this.showDocumentFiles();
    }

    private void btnMergeFiles_Click(object sender, EventArgs e)
    {
      if (!this.fileViewer.CanCloseViewer())
        return;
      long ticks = DateTime.Now.Ticks;
      if (!this.loanDataMgr.LockLoanWithExclusiveA())
        return;
      FileAttachment[] selectedFiles = this.getSelectedFiles();
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      foreach (FileAttachment fileAttachment in selectedFiles)
      {
        switch (fileAttachment)
        {
          case BackgroundAttachment _:
            ++num1;
            break;
          case ImageAttachment _:
            ++num2;
            break;
          case NativeAttachment _:
            ++num3;
            break;
          case CloudAttachment _:
            ++num4;
            break;
        }
      }
      bool flag1 = this.loanDataMgr.SystemConfiguration.ImageAttachmentSettings.UseImageAttachments;
      bool flag2 = false;
      if (num3 == selectedFiles.Length)
        flag1 = false;
      else if (num2 == selectedFiles.Length)
        flag1 = true;
      else if (num4 == selectedFiles.Length)
        flag2 = true;
      if (!flag1 && !flag2)
      {
        using (PdfFileBuilder pdfFileBuilder = new PdfFileBuilder(this.loanDataMgr))
        {
          string file1 = pdfFileBuilder.CreateFile(selectedFiles);
          if (file1 == null)
            return;
          using (AttachFilesDialog attachFilesDialog = new AttachFilesDialog(this.loanDataMgr))
          {
            FileAttachment file2 = attachFilesDialog.MergeFiles(selectedFiles, file1, this.doc);
            if (file2 == null)
              return;
            RemoteLogger.Write(TraceLevel.Info, "Merged NativeAttachments: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms, " + (object) selectedFiles.Length + " files");
            this.showDocumentFiles(file2);
          }
        }
      }
      else if (flag1 && !flag2)
      {
        List<ImageAttachment> imageAttachmentList = new List<ImageAttachment>();
        foreach (FileAttachment fileAttachment1 in selectedFiles)
        {
          switch (fileAttachment1)
          {
            case ImageAttachment _:
              ImageAttachment imageAttachment1 = (ImageAttachment) fileAttachment1;
              imageAttachmentList.Add(imageAttachment1);
              break;
            case NativeAttachment _:
              NativeAttachment attachment = (NativeAttachment) fileAttachment1;
              string filepath = this.loanDataMgr.FileAttachments.DownloadAttachment(attachment);
              if (filepath == null)
                return;
              using (AttachFilesDialog attachFilesDialog = new AttachFilesDialog(this.loanDataMgr))
              {
                FileAttachment fileAttachment2 = attachFilesDialog.ReplaceFile(AddReasonType.ConversionForMerge, attachment, filepath, this.doc);
                if (fileAttachment2 == null)
                  return;
                ImageAttachment imageAttachment2 = (ImageAttachment) fileAttachment2;
                imageAttachmentList.Add(imageAttachment2);
                break;
              }
          }
        }
        ImageAttachment file = this.loanDataMgr.FileAttachments.MergeAttachments(imageAttachmentList.ToArray(), this.doc);
        RemoteLogger.Write(TraceLevel.Info, "Merged ImageAttachments: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms, " + (object) selectedFiles.Length + " files");
        this.showDocumentFiles((FileAttachment) file);
      }
      else
      {
        if (!flag2)
          return;
        List<CloudAttachment> cloudAttachmentList = new List<CloudAttachment>();
        foreach (CloudAttachment cloudAttachment in selectedFiles)
          cloudAttachmentList.Add(cloudAttachment);
        CloudAttachment file = (CloudAttachment) null;
        using (EOSClientDialog eosClientDialog = new EOSClientDialog(this.loanDataMgr))
          file = eosClientDialog.MergeAttachments(this.fileViewer.MergeJobId, cloudAttachmentList.ToArray(), this.doc);
        if (file == null)
          return;
        RemoteLogger.Write(TraceLevel.Info, "Merged CloudAttachments: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms, " + (object) selectedFiles.Length + " files");
        this.showDocumentFiles((FileAttachment) file);
      }
    }

    private void applySecurity()
    {
      if (!this.rights.CanAddNewDocumentName)
      {
        this.cboTitle.DropDownStyle = ComboBoxStyle.DropDownList;
        if (!this.cboTitle.Items.Contains((object) this.doc.Title))
        {
          this.cboTitle.Items.Add((object) this.doc.Title);
          this.cboTitle.Text = this.doc.Title;
        }
      }
      this.cboTitle.Visible = this.rights.CanEditDocument;
      this.txtTitle.Visible = !this.rights.CanEditDocument;
      this.txtDescription.ReadOnly = !this.rights.CanEditDocument;
      this.cboBorrower.Visible = this.rights.CanEditDocument;
      this.txtBorrower.Visible = !this.rights.CanEditDocument;
      this.cboMilestone.Visible = this.rights.CanEditDocument;
      this.txtMilestone.Visible = !this.rights.CanEditDocument;
      this.btnAccess.Visible = this.rights.CanSetDocumentAccess;
      if (this.loanDataMgr.LoanData.EnableEnhancedConditions)
      {
        this.btnConditions.Visible = false;
        EnhancedConditionType[] conditionTypes = this.getConditionTypes();
        if (conditionTypes != null)
        {
          foreach (EnhancedConditionType enhancedConditionType in conditionTypes)
          {
            if (this.rights.CanAssignEnhancedConditionDocuments(enhancedConditionType.title) || this.rights.CanUnassignEnhancedConditionDocuments(enhancedConditionType.title))
            {
              this.btnConditions.Visible = true;
              break;
            }
          }
        }
      }
      else
        this.btnConditions.Visible = this.rights.CanAccessPreliminaryTab || this.rights.CanAccessUnderwritingTab || this.rights.CanAccessPostClosingTab;
      this.chkWebCenter.Enabled = this.rights.CanEditDocument;
      this.chkTPOWebcenterPortal.Enabled = this.rights.CanEditDocument;
      this.chkThirdParty.Enabled = this.rights.CanEditDocument;
      this.txtDaysDue.ReadOnly = !this.rights.CanEditDocument;
      this.txtDaysExpire.ReadOnly = !this.rights.CanEditDocument;
      this.txtRequestedFrom.ReadOnly = !this.rights.CanEditDocument;
      this.chkRequested.Enabled = this.rights.CanEditDocument;
      this.chkRerequested.Enabled = this.rights.CanEditDocument;
      this.chkReceived.Enabled = this.rights.CanEditDocument;
      this.chkReviewed.Enabled = this.rights.CanReviewDocuments;
      this.chkUnderwritingReady.Enabled = this.rights.CanEditDocument;
      this.chkShippingReady.Enabled = this.rights.CanEditDocument;
      this.commentCollection.CanAddComment = this.rights.CanAddDocumentComments;
      this.commentCollection.CanDeleteComment = this.rights.CanDeleteDocumentComments;
      this.btnAttachBrowse.Visible = this.rights.CanBrowseAttach;
      this.btnAttachScan.Visible = this.rights.CanScanAttach;
      this.btnAttachForms.Visible = this.rights.CanAttachForms;
      this.btnAttachUnassigned.Visible = this.rights.CanAttachUnassignedFiles;
      this.btnMoveFileUp.Visible = this.rights.CanMoveFilesUpDown;
      this.btnMoveFileDown.Visible = this.rights.CanMoveFilesUpDown;
      this.separator.Visible = this.rights.CanMergeFiles || this.rights.CanRemoveDocumentFiles;
      this.btnMergeFiles.Visible = this.rights.CanMergeFiles;
      this.btnRemoveFile.Visible = this.rights.CanRemoveDocumentFiles;
      if ((EnableDisableSetting) Session.GetComponentSetting("Scanning", (object) EnableDisableSetting.Enabled) == EnableDisableSetting.Disabled)
        this.btnAttachScan.Visible = false;
      if (!Session.IsBrokerEdition())
        return;
      this.btnAccess.Visible = false;
      this.lblAccess.Visible = false;
      this.txtAccess.Visible = false;
      this.btnConditions.Top = this.btnAccess.Top;
      this.lblConditions.Top = this.lblAccess.Top;
      this.lstConditions.Bounds = new Rectangle(this.lstConditions.Left, this.txtAccess.Top, this.lstConditions.Width, this.lstConditions.Bottom - this.txtAccess.Top);
    }

    private EnhancedConditionType[] getConditionTypes()
    {
      return new EnhancedConditionsRestClient(this.loanDataMgr).GetEnhancedConditionTypes(true, false);
    }

    private void initEventHandlers()
    {
      this.FormClosed += new FormClosedEventHandler(this.onFormClosed);
      this.loanDataMgr.LoanClosing += new EventHandler(this.btnClose_Click);
      this.loanDataMgr.LoanData.LogRecordChanged += new LogRecordEventHandler(this.logRecordChanged);
      this.loanDataMgr.LoanData.LogRecordRemoved += new LogRecordEventHandler(this.logRecordRemoved);
      this.loanDataMgr.FileAttachments.FileAttachmentChanged += new FileAttachmentEventHandler(this.fileAttachmentChanged);
      this.loanDataMgr.FileAttachments.BackgroundAttachmentUploaded += new FileAttachmentEventHandler(this.backgroundAttachmentUploaded);
    }

    private void releaseEventHandlers()
    {
      this.FormClosed -= new FormClosedEventHandler(this.onFormClosed);
      this.loanDataMgr.LoanClosing -= new EventHandler(this.btnClose_Click);
      this.loanDataMgr.LoanData.LogRecordChanged -= new LogRecordEventHandler(this.logRecordChanged);
      this.loanDataMgr.LoanData.LogRecordRemoved -= new LogRecordEventHandler(this.logRecordRemoved);
      this.loanDataMgr.FileAttachments.FileAttachmentChanged -= new FileAttachmentEventHandler(this.fileAttachmentChanged);
      this.loanDataMgr.FileAttachments.BackgroundAttachmentUploaded -= new FileAttachmentEventHandler(this.backgroundAttachmentUploaded);
    }

    private void onFormClosed(object sender, FormClosedEventArgs e) => this.releaseEventHandlers();

    private void backgroundAttachmentUploaded(object source, FileAttachmentEventArgs e)
    {
      Tracing.Log(DocumentDetailsDialog.sw, TraceLevel.Verbose, nameof (DocumentDetailsDialog), "Checking InvokeRequired For BackgroundAttachmentUploaded");
      if (this.InvokeRequired)
      {
        FileAttachmentEventHandler method = new FileAttachmentEventHandler(this.backgroundAttachmentUploaded);
        Tracing.Log(DocumentDetailsDialog.sw, TraceLevel.Verbose, nameof (DocumentDetailsDialog), "Calling BeginInvoke For BackgroundAttachmentUploaded");
        this.BeginInvoke((Delegate) method, source, (object) e);
      }
      else
      {
        if (!this.doc.Files.Contains(e.File.ID))
          return;
        if (this == Form.ActiveForm)
        {
          this.loadFileList(false);
          bool flag = false;
          foreach (GVItem selectedItem in this.gvFiles.SelectedItems)
          {
            if (selectedItem.Tag == e.File)
              flag = true;
          }
          if (!flag)
            return;
          this.showDocumentFiles();
        }
        else
          this.refreshFiles = true;
      }
    }

    private void fileAttachmentChanged(object source, FileAttachmentEventArgs e)
    {
      Tracing.Log(DocumentDetailsDialog.sw, TraceLevel.Verbose, nameof (DocumentDetailsDialog), "Checking InvokeRequired For FileAttachmentChanged");
      if (this.InvokeRequired)
      {
        FileAttachmentEventHandler method = new FileAttachmentEventHandler(this.fileAttachmentChanged);
        Tracing.Log(DocumentDetailsDialog.sw, TraceLevel.Verbose, nameof (DocumentDetailsDialog), "Calling BeginInvoke For FileAttachmentChanged");
        this.BeginInvoke((Delegate) method, source, (object) e);
      }
      else
      {
        if (!this.gvFiles.Items.ContainsTag((object) e.File) && !this.doc.Files.Contains(e.File.ID))
          return;
        if (this == Form.ActiveForm)
          this.loadFileList(false);
        else
          this.refreshFiles = true;
      }
    }

    private void logRecordChanged(object source, LogRecordEventArgs e)
    {
      Tracing.Log(DocumentDetailsDialog.sw, TraceLevel.Verbose, nameof (DocumentDetailsDialog), "Checking InvokeRequired For LogRecordChanged");
      if (this.InvokeRequired)
      {
        LogRecordEventHandler method = new LogRecordEventHandler(this.logRecordChanged);
        Tracing.Log(DocumentDetailsDialog.sw, TraceLevel.Verbose, nameof (DocumentDetailsDialog), "Calling BeginInvoke For LogRecordChanged");
        this.BeginInvoke((Delegate) method, source, (object) e);
      }
      else
      {
        if (e.LogRecord != this.doc)
          return;
        this.loadDocumentDetails();
        if (this == Form.ActiveForm)
          this.loadFileList(false);
        else
          this.refreshFiles = true;
      }
    }

    private void logRecordRemoved(object source, LogRecordEventArgs e)
    {
      Tracing.Log(DocumentDetailsDialog.sw, TraceLevel.Verbose, nameof (DocumentDetailsDialog), "Checking InvokeRequired For LogRecordRemoved");
      if (this.InvokeRequired)
      {
        LogRecordEventHandler method = new LogRecordEventHandler(this.logRecordRemoved);
        Tracing.Log(DocumentDetailsDialog.sw, TraceLevel.Verbose, nameof (DocumentDetailsDialog), "Calling BeginInvoke For LogRecordRemoved");
        this.BeginInvoke((Delegate) method, source, (object) e);
      }
      else
      {
        if (e.LogRecord != this.doc)
          return;
        this.Close();
      }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      if (sender is LoanDataMgr)
        this.AutoValidate = AutoValidate.Disable;
      this.Close();
    }

    private void fileViewer_LoadAttachments(object source, EventArgs e)
    {
      FileAttachment[] attachments = this.fileViewer.Attachments;
      FileAttachment[] selectedFiles = this.getSelectedFiles();
      bool flag = false;
      if (attachments.Length == selectedFiles.Length)
      {
        for (int index = 0; index < attachments.Length; ++index)
        {
          if (attachments[index].ID != selectedFiles[index].ID)
          {
            flag = true;
            break;
          }
        }
      }
      else
        flag = true;
      if (!flag)
        return;
      this.showDocumentFiles(attachments);
    }

    private void DocumentDetailsDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      Session.Application.DisplayHelp(this.helpLink.HelpTag);
    }

    private void DocumentDetailsDialog_Activated(object sender, EventArgs e)
    {
      if (!this.refreshFiles)
        return;
      this.loadFileList(false);
      this.showDocumentFiles();
      this.refreshFiles = false;
    }

    private void DocumentDetailsDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.fileViewer.CanCloseViewer())
        return;
      e.Cancel = true;
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

    private void gvFiles_DragOver(object sender, DragEventArgs e)
    {
      if (this.previousFileDropItem != null)
        this.previousFileDropItem.BackColor = Color.Transparent;
      Point client = this.gvFiles.PointToClient(new Point(e.X, e.Y));
      GVItem itemAt = this.gvFiles.GetItemAt(client.X, client.Y);
      this.previousFileDropItem = itemAt;
      if (e.Data.GetDataPresent(typeof (PageImage[])))
      {
        if (itemAt != null && itemAt.Tag is NativeAttachment)
        {
          e.Effect = DragDropEffects.None;
        }
        else
        {
          e.Effect = DragDropEffects.Move;
          eFolderUIHelper.SetPageThumbnailCursor((PageImage[]) e.Data.GetData(typeof (PageImage[])));
          if (itemAt == null)
            return;
          itemAt.BackColor = Color.LightYellow;
        }
      }
      else if (e.Data.GetDataPresent(DataFormats.FileDrop))
        e.Effect = DragDropEffects.Copy;
      else
        e.Effect = DragDropEffects.None;
    }

    private void gvFiles_DragDrop(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.FileDrop))
      {
        FileAttachment[] fileList = this.assignFileDrop((string[]) e.Data.GetData(DataFormats.FileDrop));
        if (fileList == null)
          return;
        this.showDocumentFiles(fileList);
      }
      else
      {
        if (!e.Data.GetDataPresent(typeof (PageImage[])))
          return;
        PageImage[] data = (PageImage[]) e.Data.GetData(typeof (PageImage[]));
        Point client = this.gvFiles.PointToClient(new Point(e.X, e.Y));
        GVItem itemAt = this.gvFiles.GetItemAt(client.X, client.Y);
        ImageAttachment attachment = (ImageAttachment) null;
        if (itemAt != null)
          attachment = itemAt.Tag as ImageAttachment;
        if (!this.assignPageDrop(data, attachment))
          return;
        this.showDocumentFiles();
      }
    }

    private FileAttachment[] assignFileDrop(string[] dropList)
    {
      if (!this.rights.CanBrowseAttach)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to attach files to this document.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (FileAttachment[]) null;
      }
      using (AttachFilesDialog attachFilesDialog = new AttachFilesDialog(this.loanDataMgr))
        return attachFilesDialog.AttachFiles(dropList, this.doc);
    }

    private bool assignPageDrop(PageImage[] pageList, ImageAttachment attachment)
    {
      long ticks = DateTime.Now.Ticks;
      bool flag1 = false;
      bool flag2 = false;
      foreach (PageImage page in pageList)
      {
        DocumentLog linkedDocument = this.loanDataMgr.FileAttachments.GetLinkedDocument(page.Attachment.ID);
        if (!new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) linkedDocument).CanSplitFiles)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to split the '" + page.Attachment.Title + "' file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        flag1 = true;
        if (linkedDocument != this.doc)
        {
          if (!new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) linkedDocument).CanRemoveDocumentFiles)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to remove the pages from the '" + page.Attachment.Title + "' file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
          if (!this.rights.CanAttachUnassignedFiles)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to add pages from the '" + page.Attachment.Title + "' file to the '" + this.doc.Title + "' document.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
        }
        else if (attachment != null)
        {
          if (!this.rights.CanMergeFiles)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to add pages from the '" + page.Attachment.Title + "' file to the '" + attachment.Title + "' file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
          flag2 = true;
        }
      }
      if (attachment == null)
      {
        using (AddImageAttachmentDialog attachmentDialog = new AddImageAttachmentDialog(this.loanDataMgr, pageList, this.doc))
        {
          if (attachmentDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return false;
          attachment = attachmentDialog.Attachment;
        }
      }
      else
        attachment.Pages.AddRange(pageList);
      DateTime now;
      TimeSpan timeSpan;
      if (flag1)
      {
        now = DateTime.Now;
        timeSpan = TimeSpan.FromTicks(now.Ticks - ticks);
        RemoteLogger.Write(TraceLevel.Info, "Split ImageAttachment: " + (object) timeSpan.TotalMilliseconds + " ms");
      }
      if (flag2)
      {
        now = DateTime.Now;
        timeSpan = TimeSpan.FromTicks(now.Ticks - ticks);
        RemoteLogger.Write(TraceLevel.Info, "Merged ImageAttachment: " + (object) timeSpan.TotalMilliseconds + " ms");
      }
      return true;
    }

    private void loadFinalCDButton()
    {
      DocumentTemplate byName = this.docSetup.GetByName(this.doc.Title);
      string docTemplateSource = byName != null ? byName.Source : string.Empty;
      if (docTemplateSource.StartsWith("Closing Disclosure"))
        this.populateBtnFinalCDText(docTemplateSource);
      else if (this.loanDataMgr.LoanData.GetField("UCD.X1") == this.doc.Guid || this.loanDataMgr.LoanData.GetField("UCD.X3") == this.doc.Guid || this.loanDataMgr.LoanData.GetField("UCD.X5") == this.doc.Guid)
        this.populateBtnFinalCDText(docTemplateSource);
      else
        this.btnFinalCD.Visible = false;
    }

    private void populateBtnFinalCDText(string docTemplateSource)
    {
      if (this.loanDataMgr.LoanData.GetField("UCD.X1") == this.doc.Guid || this.loanDataMgr.LoanData.GetField("UCD.X3") == this.doc.Guid || this.loanDataMgr.LoanData.GetField("UCD.X5") == this.doc.Guid)
        this.btnFinalCD.Text = "Unmark as Final CD";
      else
        this.btnFinalCD.Text = "Mark as Final CD";
      switch (docTemplateSource)
      {
        case "Closing Disclosure (Alternate)":
          this.btnFinalCD.Text += " (Alternate)";
          break;
        case "Closing Disclosure (Seller)":
          this.btnFinalCD.Text += " (Seller)";
          break;
      }
      this.btnFinalCD.Visible = true;
    }

    private void btnFinalCD_Click(object sender, EventArgs e)
    {
      if (this.btnFinalCD.Text.StartsWith("Mark"))
      {
        DocumentTemplate byName = this.docSetup.GetByName(this.doc.Title);
        string str = byName != null ? byName.Source : string.Empty;
        using (SelectSignatureTypeDialog signatureTypeDialog = new SelectSignatureTypeDialog(str))
        {
          if (signatureTypeDialog.ShowDialog() != DialogResult.OK)
            return;
          this.doc.MarkAsFinalCD(this.loanDataMgr.LoanData, str, signatureTypeDialog.signatureType);
        }
      }
      else
      {
        if (!this.btnFinalCD.Text.StartsWith("Unmark"))
          return;
        this.doc.UnmarkAsFinalCD(this.loanDataMgr.LoanData);
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
      this.pnlLeft = new Panel();
      this.gcTracking = new GroupContainer();
      this.tabTracking = new TabControl();
      this.pageStatus = new TabPage();
      this.btnShippingReadyBy = new StandardIconButton();
      this.txtShippingReadyBy = new TextBox();
      this.dateShippingReady = new DateTimePicker();
      this.chkShippingReady = new CheckBox();
      this.btnUnderwritingReadyBy = new StandardIconButton();
      this.txtUnderwritingReadyBy = new TextBox();
      this.dateUnderwritingReady = new DateTimePicker();
      this.chkUnderwritingReady = new CheckBox();
      this.btnReviewedBy = new StandardIconButton();
      this.btnReceivedBy = new StandardIconButton();
      this.btnRerequestedBy = new StandardIconButton();
      this.btnRequestedBy = new StandardIconButton();
      this.txtReviewedBy = new TextBox();
      this.txtReceivedBy = new TextBox();
      this.txtRerequestedBy = new TextBox();
      this.txtRequestedBy = new TextBox();
      this.dateReviewed = new DateTimePicker();
      this.chkReviewed = new CheckBox();
      this.dateReceived = new DateTimePicker();
      this.txtDateReceived = new TextBox();
      this.chkReceived = new CheckBox();
      this.dateRerequested = new DateTimePicker();
      this.txtDateRerequested = new TextBox();
      this.chkRerequested = new CheckBox();
      this.dateRequested = new DateTimePicker();
      this.txtDateRequested = new TextBox();
      this.chkRequested = new CheckBox();
      this.txtRequestedFrom = new TextBox();
      this.lblCompany = new Label();
      this.txtDateExpire = new TextBox();
      this.txtDaysExpire = new TextBox();
      this.lblDaysExpire = new Label();
      this.txtDateDue = new TextBox();
      this.txtDaysDue = new TextBox();
      this.lblDaysDue = new Label();
      this.txtDateReviewed = new TextBox();
      this.txtDateUnderwritingReady = new TextBox();
      this.txtDateShippingReady = new TextBox();
      this.pageComments = new TabPage();
      this.commentCollection = new CommentCollectionControl();
      this.csDetails = new CollapsibleSplitter();
      this.gcDetails = new GroupContainer();
      this.pnlDetails = new Panel();
      this.btnFinalCD = new Button();
      this.btnATRQM = new Button();
      this.lstATRQM = new ListBox();
      this.chkThirdParty = new CheckBox();
      this.chkTPOWebcenterPortal = new CheckBox();
      this.chkWebCenter = new CheckBox();
      this.txtDescription = new TextBox();
      this.lblDescription = new Label();
      this.btnConditions = new Button();
      this.lblAvailable = new Label();
      this.lstGroups = new ListBox();
      this.lblGroups = new Label();
      this.lstConditions = new ListBox();
      this.lblConditions = new Label();
      this.txtAccess = new TextBox();
      this.btnAccess = new Button();
      this.lblAccess = new Label();
      this.cboMilestone = new ComboBoxEx();
      this.txtMilestone = new TextBox();
      this.lblMilestone = new Label();
      this.cboBorrower = new ComboBox();
      this.txtBorrower = new TextBox();
      this.lblBorrower = new Label();
      this.cboTitle = new ComboBox();
      this.txtTitle = new TextBox();
      this.lblTitle = new Label();
      this.pnlRight = new Panel();
      this.pnlViewer = new BorderPanel();
      this.fileViewer = new FileAttachmentViewerControl();
      this.csFiles = new CollapsibleSplitter();
      this.gcFiles = new GroupContainer();
      this.pnlToolbar = new FlowLayoutPanel();
      this.btnRemoveFile = new StandardIconButton();
      this.btnMergeFiles = new IconButton();
      this.separator = new VerticalSeparator();
      this.btnMoveFileDown = new StandardIconButton();
      this.btnMoveFileUp = new StandardIconButton();
      this.btnAttachUnassigned = new IconButton();
      this.btnAttachForms = new IconButton();
      this.btnAttachScan = new IconButton();
      this.btnAttachBrowse = new IconButton();
      this.gvFiles = new GridView();
      this.tooltip = new ToolTip(this.components);
      this.pnlClose = new Panel();
      this.helpLink = new EMHelpLink();
      this.btnClose = new Button();
      this.csLeft = new CollapsibleSplitter();
      this.pnlLeft.SuspendLayout();
      this.gcTracking.SuspendLayout();
      this.tabTracking.SuspendLayout();
      this.pageStatus.SuspendLayout();
      ((ISupportInitialize) this.btnShippingReadyBy).BeginInit();
      ((ISupportInitialize) this.btnUnderwritingReadyBy).BeginInit();
      ((ISupportInitialize) this.btnReviewedBy).BeginInit();
      ((ISupportInitialize) this.btnReceivedBy).BeginInit();
      ((ISupportInitialize) this.btnRerequestedBy).BeginInit();
      ((ISupportInitialize) this.btnRequestedBy).BeginInit();
      this.pageComments.SuspendLayout();
      this.gcDetails.SuspendLayout();
      this.pnlDetails.SuspendLayout();
      this.pnlRight.SuspendLayout();
      this.pnlViewer.SuspendLayout();
      this.gcFiles.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      ((ISupportInitialize) this.btnRemoveFile).BeginInit();
      ((ISupportInitialize) this.btnMergeFiles).BeginInit();
      ((ISupportInitialize) this.btnMoveFileDown).BeginInit();
      ((ISupportInitialize) this.btnMoveFileUp).BeginInit();
      ((ISupportInitialize) this.btnAttachUnassigned).BeginInit();
      ((ISupportInitialize) this.btnAttachForms).BeginInit();
      ((ISupportInitialize) this.btnAttachScan).BeginInit();
      ((ISupportInitialize) this.btnAttachBrowse).BeginInit();
      this.pnlClose.SuspendLayout();
      this.SuspendLayout();
      this.pnlLeft.BackColor = Color.WhiteSmoke;
      this.pnlLeft.Controls.Add((Control) this.gcTracking);
      this.pnlLeft.Controls.Add((Control) this.csDetails);
      this.pnlLeft.Controls.Add((Control) this.gcDetails);
      this.pnlLeft.Dock = DockStyle.Left;
      this.pnlLeft.Location = new Point(0, 0);
      this.pnlLeft.Name = "pnlLeft";
      this.pnlLeft.Size = new Size(365, 702);
      this.pnlLeft.TabIndex = 0;
      this.gcTracking.Controls.Add((Control) this.tabTracking);
      this.gcTracking.Dock = DockStyle.Fill;
      this.gcTracking.HeaderForeColor = SystemColors.ControlText;
      this.gcTracking.Location = new Point(0, 414);
      this.gcTracking.Name = "gcTracking";
      this.gcTracking.Padding = new Padding(3, 3, 2, 2);
      this.gcTracking.Size = new Size(365, 288);
      this.gcTracking.TabIndex = 2;
      this.gcTracking.Text = "Tracking";
      this.tabTracking.Controls.Add((Control) this.pageStatus);
      this.tabTracking.Controls.Add((Control) this.pageComments);
      this.tabTracking.Dock = DockStyle.Fill;
      this.tabTracking.Location = new Point(4, 29);
      this.tabTracking.Name = "tabTracking";
      this.tabTracking.SelectedIndex = 0;
      this.tabTracking.Size = new Size(358, 256);
      this.tabTracking.TabIndex = 0;
      this.pageStatus.AutoScroll = true;
      this.pageStatus.AutoScrollMargin = new Size(8, 8);
      this.pageStatus.BackColor = Color.WhiteSmoke;
      this.pageStatus.Controls.Add((Control) this.btnShippingReadyBy);
      this.pageStatus.Controls.Add((Control) this.txtShippingReadyBy);
      this.pageStatus.Controls.Add((Control) this.dateShippingReady);
      this.pageStatus.Controls.Add((Control) this.chkShippingReady);
      this.pageStatus.Controls.Add((Control) this.btnUnderwritingReadyBy);
      this.pageStatus.Controls.Add((Control) this.txtUnderwritingReadyBy);
      this.pageStatus.Controls.Add((Control) this.dateUnderwritingReady);
      this.pageStatus.Controls.Add((Control) this.chkUnderwritingReady);
      this.pageStatus.Controls.Add((Control) this.btnReviewedBy);
      this.pageStatus.Controls.Add((Control) this.btnReceivedBy);
      this.pageStatus.Controls.Add((Control) this.btnRerequestedBy);
      this.pageStatus.Controls.Add((Control) this.btnRequestedBy);
      this.pageStatus.Controls.Add((Control) this.txtReviewedBy);
      this.pageStatus.Controls.Add((Control) this.txtReceivedBy);
      this.pageStatus.Controls.Add((Control) this.txtRerequestedBy);
      this.pageStatus.Controls.Add((Control) this.txtRequestedBy);
      this.pageStatus.Controls.Add((Control) this.dateReviewed);
      this.pageStatus.Controls.Add((Control) this.chkReviewed);
      this.pageStatus.Controls.Add((Control) this.dateReceived);
      this.pageStatus.Controls.Add((Control) this.txtDateReceived);
      this.pageStatus.Controls.Add((Control) this.chkReceived);
      this.pageStatus.Controls.Add((Control) this.dateRerequested);
      this.pageStatus.Controls.Add((Control) this.txtDateRerequested);
      this.pageStatus.Controls.Add((Control) this.chkRerequested);
      this.pageStatus.Controls.Add((Control) this.dateRequested);
      this.pageStatus.Controls.Add((Control) this.txtDateRequested);
      this.pageStatus.Controls.Add((Control) this.chkRequested);
      this.pageStatus.Controls.Add((Control) this.txtRequestedFrom);
      this.pageStatus.Controls.Add((Control) this.lblCompany);
      this.pageStatus.Controls.Add((Control) this.txtDateExpire);
      this.pageStatus.Controls.Add((Control) this.txtDaysExpire);
      this.pageStatus.Controls.Add((Control) this.lblDaysExpire);
      this.pageStatus.Controls.Add((Control) this.txtDateDue);
      this.pageStatus.Controls.Add((Control) this.txtDaysDue);
      this.pageStatus.Controls.Add((Control) this.lblDaysDue);
      this.pageStatus.Controls.Add((Control) this.txtDateReviewed);
      this.pageStatus.Controls.Add((Control) this.txtDateUnderwritingReady);
      this.pageStatus.Controls.Add((Control) this.txtDateShippingReady);
      this.pageStatus.Location = new Point(4, 23);
      this.pageStatus.Name = "pageStatus";
      this.pageStatus.Padding = new Padding(0, 2, 2, 2);
      this.pageStatus.Size = new Size(350, 229);
      this.pageStatus.TabIndex = 0;
      this.pageStatus.Text = "Status";
      this.pageStatus.UseVisualStyleBackColor = true;
      this.btnShippingReadyBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnShippingReadyBy.BackColor = Color.Transparent;
      this.btnShippingReadyBy.Location = new Point(330, 202);
      this.btnShippingReadyBy.MouseDownImage = (Image) null;
      this.btnShippingReadyBy.Name = "btnShippingReadyBy";
      this.btnShippingReadyBy.Size = new Size(16, 16);
      this.btnShippingReadyBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnShippingReadyBy.TabIndex = 64;
      this.btnShippingReadyBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnShippingReadyBy, "Select User");
      this.btnShippingReadyBy.Click += new EventHandler(this.btnShippingReadyBy_Click);
      this.txtShippingReadyBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtShippingReadyBy.Location = new Point(268, 200);
      this.txtShippingReadyBy.Name = "txtShippingReadyBy";
      this.txtShippingReadyBy.ReadOnly = true;
      this.txtShippingReadyBy.Size = new Size(59, 20);
      this.txtShippingReadyBy.TabIndex = 31;
      this.dateShippingReady.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateShippingReady.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateShippingReady.CalendarTitleForeColor = Color.White;
      this.dateShippingReady.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateShippingReady.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateShippingReady.Format = DateTimePickerFormat.Custom;
      this.dateShippingReady.Location = new Point(99, 200);
      this.dateShippingReady.Name = "dateShippingReady";
      this.dateShippingReady.Size = new Size(163, 20);
      this.dateShippingReady.TabIndex = 29;
      this.dateShippingReady.ValueChanged += new EventHandler(this.dateShippingReady_ValueChanged);
      this.chkShippingReady.AutoSize = true;
      this.chkShippingReady.Location = new Point(8, 203);
      this.chkShippingReady.Name = "chkShippingReady";
      this.chkShippingReady.Size = new Size(93, 18);
      this.chkShippingReady.TabIndex = 28;
      this.chkShippingReady.Text = "Ready to Ship";
      this.chkShippingReady.UseVisualStyleBackColor = true;
      this.chkShippingReady.Click += new EventHandler(this.chkShippingReady_Click);
      this.btnUnderwritingReadyBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnUnderwritingReadyBy.BackColor = Color.Transparent;
      this.btnUnderwritingReadyBy.Location = new Point(330, 178);
      this.btnUnderwritingReadyBy.MouseDownImage = (Image) null;
      this.btnUnderwritingReadyBy.Name = "btnUnderwritingReadyBy";
      this.btnUnderwritingReadyBy.Size = new Size(16, 16);
      this.btnUnderwritingReadyBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnUnderwritingReadyBy.TabIndex = 60;
      this.btnUnderwritingReadyBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnUnderwritingReadyBy, "Select User");
      this.btnUnderwritingReadyBy.Click += new EventHandler(this.btnUnderwritingReadyBy_Click);
      this.txtUnderwritingReadyBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtUnderwritingReadyBy.Location = new Point(268, 176);
      this.txtUnderwritingReadyBy.Name = "txtUnderwritingReadyBy";
      this.txtUnderwritingReadyBy.ReadOnly = true;
      this.txtUnderwritingReadyBy.Size = new Size(59, 20);
      this.txtUnderwritingReadyBy.TabIndex = 27;
      this.dateUnderwritingReady.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateUnderwritingReady.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateUnderwritingReady.CalendarTitleForeColor = Color.White;
      this.dateUnderwritingReady.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateUnderwritingReady.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateUnderwritingReady.Format = DateTimePickerFormat.Custom;
      this.dateUnderwritingReady.Location = new Point(99, 176);
      this.dateUnderwritingReady.Name = "dateUnderwritingReady";
      this.dateUnderwritingReady.Size = new Size(163, 20);
      this.dateUnderwritingReady.TabIndex = 25;
      this.dateUnderwritingReady.ValueChanged += new EventHandler(this.dateUnderwritingReady_ValueChanged);
      this.chkUnderwritingReady.AutoSize = true;
      this.chkUnderwritingReady.Location = new Point(8, 179);
      this.chkUnderwritingReady.Name = "chkUnderwritingReady";
      this.chkUnderwritingReady.Size = new Size(94, 18);
      this.chkUnderwritingReady.TabIndex = 24;
      this.chkUnderwritingReady.Text = "Ready for UW";
      this.chkUnderwritingReady.UseVisualStyleBackColor = true;
      this.chkUnderwritingReady.Click += new EventHandler(this.chkUnderwritingReady_Click);
      this.btnReviewedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReviewedBy.BackColor = Color.Transparent;
      this.btnReviewedBy.Location = new Point(330, 154);
      this.btnReviewedBy.MouseDownImage = (Image) null;
      this.btnReviewedBy.Name = "btnReviewedBy";
      this.btnReviewedBy.Size = new Size(16, 16);
      this.btnReviewedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnReviewedBy.TabIndex = 56;
      this.btnReviewedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnReviewedBy, "Select User");
      this.btnReviewedBy.Click += new EventHandler(this.btnReviewedBy_Click);
      this.btnReceivedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReceivedBy.BackColor = Color.Transparent;
      this.btnReceivedBy.Location = new Point(330, 129);
      this.btnReceivedBy.MouseDownImage = (Image) null;
      this.btnReceivedBy.Name = "btnReceivedBy";
      this.btnReceivedBy.Size = new Size(16, 16);
      this.btnReceivedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnReceivedBy.TabIndex = 55;
      this.btnReceivedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnReceivedBy, "Select User");
      this.btnReceivedBy.Click += new EventHandler(this.btnReceivedBy_Click);
      this.btnRerequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRerequestedBy.BackColor = Color.Transparent;
      this.btnRerequestedBy.Location = new Point(330, 105);
      this.btnRerequestedBy.MouseDownImage = (Image) null;
      this.btnRerequestedBy.Name = "btnRerequestedBy";
      this.btnRerequestedBy.Size = new Size(16, 16);
      this.btnRerequestedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnRerequestedBy.TabIndex = 54;
      this.btnRerequestedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnRerequestedBy, "Select User");
      this.btnRerequestedBy.Click += new EventHandler(this.btnRerequestedBy_Click);
      this.btnRequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRequestedBy.BackColor = Color.Transparent;
      this.btnRequestedBy.Location = new Point(330, 82);
      this.btnRequestedBy.MouseDownImage = (Image) null;
      this.btnRequestedBy.Name = "btnRequestedBy";
      this.btnRequestedBy.Size = new Size(16, 16);
      this.btnRequestedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnRequestedBy.TabIndex = 53;
      this.btnRequestedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnRequestedBy, "Select User");
      this.btnRequestedBy.Click += new EventHandler(this.btnRequestedBy_Click);
      this.txtReviewedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtReviewedBy.Location = new Point(268, 152);
      this.txtReviewedBy.Name = "txtReviewedBy";
      this.txtReviewedBy.ReadOnly = true;
      this.txtReviewedBy.Size = new Size(59, 20);
      this.txtReviewedBy.TabIndex = 23;
      this.txtReceivedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtReceivedBy.Location = new Point(268, 128);
      this.txtReceivedBy.Name = "txtReceivedBy";
      this.txtReceivedBy.ReadOnly = true;
      this.txtReceivedBy.Size = new Size(59, 20);
      this.txtReceivedBy.TabIndex = 19;
      this.txtRerequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtRerequestedBy.Location = new Point(268, 104);
      this.txtRerequestedBy.Name = "txtRerequestedBy";
      this.txtRerequestedBy.ReadOnly = true;
      this.txtRerequestedBy.Size = new Size(59, 20);
      this.txtRerequestedBy.TabIndex = 15;
      this.txtRequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtRequestedBy.Location = new Point(268, 80);
      this.txtRequestedBy.Name = "txtRequestedBy";
      this.txtRequestedBy.ReadOnly = true;
      this.txtRequestedBy.Size = new Size(59, 20);
      this.txtRequestedBy.TabIndex = 11;
      this.dateReviewed.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateReviewed.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateReviewed.CalendarTitleForeColor = Color.White;
      this.dateReviewed.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateReviewed.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateReviewed.Format = DateTimePickerFormat.Custom;
      this.dateReviewed.Location = new Point(99, 152);
      this.dateReviewed.Name = "dateReviewed";
      this.dateReviewed.Size = new Size(163, 20);
      this.dateReviewed.TabIndex = 21;
      this.dateReviewed.ValueChanged += new EventHandler(this.dateReviewed_ValueChanged);
      this.chkReviewed.AutoSize = true;
      this.chkReviewed.Location = new Point(8, 155);
      this.chkReviewed.Name = "chkReviewed";
      this.chkReviewed.Size = new Size(75, 18);
      this.chkReviewed.TabIndex = 20;
      this.chkReviewed.Text = "Reviewed";
      this.chkReviewed.UseVisualStyleBackColor = true;
      this.chkReviewed.Click += new EventHandler(this.chkReviewed_Click);
      this.dateReceived.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateReceived.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateReceived.CalendarTitleForeColor = Color.White;
      this.dateReceived.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateReceived.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateReceived.Format = DateTimePickerFormat.Custom;
      this.dateReceived.Location = new Point(99, 128);
      this.dateReceived.Name = "dateReceived";
      this.dateReceived.Size = new Size(163, 20);
      this.dateReceived.TabIndex = 17;
      this.dateReceived.ValueChanged += new EventHandler(this.dateReceived_ValueChanged);
      this.txtDateReceived.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateReceived.Location = new Point(100, 128);
      this.txtDateReceived.Name = "txtDateReceived";
      this.txtDateReceived.ReadOnly = true;
      this.txtDateReceived.Size = new Size(85, 20);
      this.txtDateReceived.TabIndex = 18;
      this.chkReceived.AutoSize = true;
      this.chkReceived.Location = new Point(8, 131);
      this.chkReceived.Name = "chkReceived";
      this.chkReceived.Size = new Size(71, 18);
      this.chkReceived.TabIndex = 16;
      this.chkReceived.Text = "Received";
      this.chkReceived.UseVisualStyleBackColor = true;
      this.chkReceived.Click += new EventHandler(this.chkReceived_Click);
      this.dateRerequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateRerequested.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateRerequested.CalendarTitleForeColor = Color.White;
      this.dateRerequested.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateRerequested.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateRerequested.Format = DateTimePickerFormat.Custom;
      this.dateRerequested.Location = new Point(99, 104);
      this.dateRerequested.Name = "dateRerequested";
      this.dateRerequested.Size = new Size(163, 20);
      this.dateRerequested.TabIndex = 13;
      this.dateRerequested.ValueChanged += new EventHandler(this.dateRerequested_ValueChanged);
      this.txtDateRerequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateRerequested.Location = new Point(100, 104);
      this.txtDateRerequested.Name = "txtDateRerequested";
      this.txtDateRerequested.ReadOnly = true;
      this.txtDateRerequested.Size = new Size(85, 20);
      this.txtDateRerequested.TabIndex = 14;
      this.chkRerequested.AutoSize = true;
      this.chkRerequested.Location = new Point(8, 107);
      this.chkRerequested.Name = "chkRerequested";
      this.chkRerequested.Size = new Size(92, 18);
      this.chkRerequested.TabIndex = 12;
      this.chkRerequested.Text = "Re-requested";
      this.chkRerequested.UseVisualStyleBackColor = true;
      this.chkRerequested.Click += new EventHandler(this.chkRerequested_Click);
      this.dateRequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateRequested.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateRequested.CalendarTitleForeColor = Color.White;
      this.dateRequested.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateRequested.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateRequested.Format = DateTimePickerFormat.Custom;
      this.dateRequested.Location = new Point(99, 80);
      this.dateRequested.Name = "dateRequested";
      this.dateRequested.Size = new Size(163, 20);
      this.dateRequested.TabIndex = 9;
      this.dateRequested.ValueChanged += new EventHandler(this.dateRequested_ValueChanged);
      this.txtDateRequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateRequested.Location = new Point(100, 80);
      this.txtDateRequested.Name = "txtDateRequested";
      this.txtDateRequested.ReadOnly = true;
      this.txtDateRequested.Size = new Size(85, 20);
      this.txtDateRequested.TabIndex = 10;
      this.chkRequested.AutoSize = true;
      this.chkRequested.Location = new Point(8, 83);
      this.chkRequested.Name = "chkRequested";
      this.chkRequested.Size = new Size(78, 18);
      this.chkRequested.TabIndex = 8;
      this.chkRequested.Text = "Requested";
      this.chkRequested.UseVisualStyleBackColor = true;
      this.chkRequested.Click += new EventHandler(this.chkRequested_Click);
      this.txtRequestedFrom.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtRequestedFrom.Location = new Point(100, 56);
      this.txtRequestedFrom.Name = "txtRequestedFrom";
      this.txtRequestedFrom.Size = new Size(228, 20);
      this.txtRequestedFrom.TabIndex = 7;
      this.txtRequestedFrom.Validated += new EventHandler(this.txtRequestedFrom_Validated);
      this.lblCompany.AutoSize = true;
      this.lblCompany.Location = new Point(8, 59);
      this.lblCompany.Name = "lblCompany";
      this.lblCompany.Size = new Size(86, 14);
      this.lblCompany.TabIndex = 6;
      this.lblCompany.Text = "Requested From";
      this.txtDateExpire.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateExpire.Location = new Point(152, 32);
      this.txtDateExpire.Name = "txtDateExpire";
      this.txtDateExpire.ReadOnly = true;
      this.txtDateExpire.Size = new Size(176, 20);
      this.txtDateExpire.TabIndex = 5;
      this.txtDaysExpire.Location = new Point(100, 32);
      this.txtDaysExpire.Name = "txtDaysExpire";
      this.txtDaysExpire.Size = new Size(48, 20);
      this.txtDaysExpire.TabIndex = 4;
      this.txtDaysExpire.TextAlign = HorizontalAlignment.Right;
      this.txtDaysExpire.Validating += new CancelEventHandler(this.txtDaysExpire_Validating);
      this.txtDaysExpire.Validated += new EventHandler(this.txtDaysExpire_Validated);
      this.lblDaysExpire.AutoSize = true;
      this.lblDaysExpire.Location = new Point(8, 35);
      this.lblDaysExpire.Name = "lblDaysExpire";
      this.lblDaysExpire.Size = new Size(77, 14);
      this.lblDaysExpire.TabIndex = 3;
      this.lblDaysExpire.Text = "Days to Expire";
      this.txtDateDue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateDue.Location = new Point(152, 8);
      this.txtDateDue.Name = "txtDateDue";
      this.txtDateDue.ReadOnly = true;
      this.txtDateDue.Size = new Size(176, 20);
      this.txtDateDue.TabIndex = 2;
      this.txtDaysDue.Location = new Point(100, 8);
      this.txtDaysDue.Name = "txtDaysDue";
      this.txtDaysDue.Size = new Size(48, 20);
      this.txtDaysDue.TabIndex = 1;
      this.txtDaysDue.TextAlign = HorizontalAlignment.Right;
      this.txtDaysDue.Validating += new CancelEventHandler(this.txtDaysDue_Validating);
      this.txtDaysDue.Validated += new EventHandler(this.txtDaysDue_Validated);
      this.lblDaysDue.AutoSize = true;
      this.lblDaysDue.Location = new Point(8, 11);
      this.lblDaysDue.Name = "lblDaysDue";
      this.lblDaysDue.Size = new Size(86, 14);
      this.lblDaysDue.TabIndex = 0;
      this.lblDaysDue.Text = "Days to Receive";
      this.txtDateReviewed.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateReviewed.Location = new Point(100, 152);
      this.txtDateReviewed.Name = "txtDateReviewed";
      this.txtDateReviewed.ReadOnly = true;
      this.txtDateReviewed.Size = new Size(85, 20);
      this.txtDateReviewed.TabIndex = 22;
      this.txtDateUnderwritingReady.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateUnderwritingReady.Location = new Point(100, 176);
      this.txtDateUnderwritingReady.Name = "txtDateUnderwritingReady";
      this.txtDateUnderwritingReady.ReadOnly = true;
      this.txtDateUnderwritingReady.Size = new Size(85, 20);
      this.txtDateUnderwritingReady.TabIndex = 26;
      this.txtDateShippingReady.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateShippingReady.Location = new Point(100, 200);
      this.txtDateShippingReady.Name = "txtDateShippingReady";
      this.txtDateShippingReady.ReadOnly = true;
      this.txtDateShippingReady.Size = new Size(85, 20);
      this.txtDateShippingReady.TabIndex = 30;
      this.pageComments.BackColor = Color.WhiteSmoke;
      this.pageComments.Controls.Add((Control) this.commentCollection);
      this.pageComments.Location = new Point(4, 23);
      this.pageComments.Name = "pageComments";
      this.pageComments.Padding = new Padding(0, 2, 2, 2);
      this.pageComments.Size = new Size(321, 233);
      this.pageComments.TabIndex = 1;
      this.pageComments.Text = "Comments";
      this.pageComments.UseVisualStyleBackColor = true;
      this.commentCollection.CanAddComment = false;
      this.commentCollection.CanDeleteComment = false;
      this.commentCollection.CanDeliverComment = false;
      this.commentCollection.Dock = DockStyle.Fill;
      this.commentCollection.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.commentCollection.Location = new Point(0, 2);
      this.commentCollection.Name = "commentCollection";
      this.commentCollection.Size = new Size(319, 229);
      this.commentCollection.TabIndex = 0;
      this.csDetails.AnimationDelay = 20;
      this.csDetails.AnimationStep = 20;
      this.csDetails.BorderStyle3D = Border3DStyle.Flat;
      this.csDetails.ControlToHide = (Control) this.gcDetails;
      this.csDetails.Dock = DockStyle.Top;
      this.csDetails.ExpandParentForm = false;
      this.csDetails.Location = new Point(0, 407);
      this.csDetails.Name = "csFiles";
      this.csDetails.TabIndex = 1;
      this.csDetails.TabStop = false;
      this.csDetails.UseAnimations = false;
      this.csDetails.VisualStyle = VisualStyles.Encompass;
      this.gcDetails.Controls.Add((Control) this.pnlDetails);
      this.gcDetails.Dock = DockStyle.Top;
      this.gcDetails.HeaderForeColor = SystemColors.ControlText;
      this.gcDetails.Location = new Point(0, 0);
      this.gcDetails.Name = "gcDetails";
      this.gcDetails.Size = new Size(365, 407);
      this.gcDetails.TabIndex = 0;
      this.gcDetails.Text = "Details";
      this.pnlDetails.AutoScroll = true;
      this.pnlDetails.AutoScrollMargin = new Size(8, 8);
      this.pnlDetails.Controls.Add((Control) this.btnFinalCD);
      this.pnlDetails.Controls.Add((Control) this.btnATRQM);
      this.pnlDetails.Controls.Add((Control) this.lstATRQM);
      this.pnlDetails.Controls.Add((Control) this.chkThirdParty);
      this.pnlDetails.Controls.Add((Control) this.chkTPOWebcenterPortal);
      this.pnlDetails.Controls.Add((Control) this.chkWebCenter);
      this.pnlDetails.Controls.Add((Control) this.txtDescription);
      this.pnlDetails.Controls.Add((Control) this.lblDescription);
      this.pnlDetails.Controls.Add((Control) this.btnConditions);
      this.pnlDetails.Controls.Add((Control) this.lblAvailable);
      this.pnlDetails.Controls.Add((Control) this.lstGroups);
      this.pnlDetails.Controls.Add((Control) this.lblGroups);
      this.pnlDetails.Controls.Add((Control) this.lstConditions);
      this.pnlDetails.Controls.Add((Control) this.lblConditions);
      this.pnlDetails.Controls.Add((Control) this.txtAccess);
      this.pnlDetails.Controls.Add((Control) this.btnAccess);
      this.pnlDetails.Controls.Add((Control) this.lblAccess);
      this.pnlDetails.Controls.Add((Control) this.cboMilestone);
      this.pnlDetails.Controls.Add((Control) this.txtMilestone);
      this.pnlDetails.Controls.Add((Control) this.lblMilestone);
      this.pnlDetails.Controls.Add((Control) this.cboBorrower);
      this.pnlDetails.Controls.Add((Control) this.txtBorrower);
      this.pnlDetails.Controls.Add((Control) this.lblBorrower);
      this.pnlDetails.Controls.Add((Control) this.cboTitle);
      this.pnlDetails.Controls.Add((Control) this.txtTitle);
      this.pnlDetails.Controls.Add((Control) this.lblTitle);
      this.pnlDetails.Dock = DockStyle.Fill;
      this.pnlDetails.Location = new Point(1, 26);
      this.pnlDetails.Name = "pnlDetails";
      this.pnlDetails.Size = new Size(363, 380);
      this.pnlDetails.TabIndex = 0;
      this.btnFinalCD.Location = new Point(84, 390);
      this.btnFinalCD.Name = "btnFinalCD";
      this.btnFinalCD.Size = new Size(250, 22);
      this.btnFinalCD.TabIndex = 32;
      this.btnFinalCD.Text = "Final CD";
      this.btnFinalCD.UseVisualStyleBackColor = true;
      this.btnFinalCD.Visible = false;
      this.btnFinalCD.Click += new EventHandler(this.btnFinalCD_Click);
      this.btnATRQM.Location = new Point(8, 234);
      this.btnATRQM.Name = "btnATRQM";
      this.btnATRQM.Size = new Size(68, 22);
      this.btnATRQM.TabIndex = 31;
      this.btnATRQM.Text = "ATR/QM";
      this.btnATRQM.UseVisualStyleBackColor = true;
      this.btnATRQM.Click += new EventHandler(this.btnATRQM_Click);
      this.lstATRQM.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lstATRQM.BackColor = SystemColors.Control;
      this.lstATRQM.FormattingEnabled = true;
      this.lstATRQM.IntegralHeight = false;
      this.lstATRQM.ItemHeight = 14;
      this.lstATRQM.Location = new Point(84, 234);
      this.lstATRQM.Name = "lstATRQM";
      this.lstATRQM.SelectionMode = SelectionMode.None;
      this.lstATRQM.Size = new Size(260, 48);
      this.lstATRQM.Sorted = true;
      this.lstATRQM.TabIndex = 30;
      this.lstATRQM.TabStop = false;
      this.chkThirdParty.AutoSize = true;
      this.chkThirdParty.Location = new Point(244, 346);
      this.chkThirdParty.Name = "chkThirdParty";
      this.chkThirdParty.Size = new Size(90, 18);
      this.chkThirdParty.TabIndex = 29;
      this.chkThirdParty.Text = "EDM Lenders";
      this.chkThirdParty.Click += new EventHandler(this.chkEDMLenders_Click);
      this.chkTPOWebcenterPortal.AutoSize = true;
      this.chkTPOWebcenterPortal.Location = new Point(166, 346);
      this.chkTPOWebcenterPortal.Name = "chkTPOWebcenterPortal";
      this.chkTPOWebcenterPortal.Size = new Size(76, 18);
      this.chkTPOWebcenterPortal.TabIndex = 0;
      this.chkTPOWebcenterPortal.Text = "TPO Portal";
      this.chkTPOWebcenterPortal.Click += new EventHandler(this.chkTPOWebcenterPortal_Click);
      this.chkWebCenter.AutoSize = true;
      this.chkWebCenter.Location = new Point(84, 346);
      this.chkWebCenter.Name = "chkWebCenter";
      this.chkWebCenter.Size = new Size(80, 18);
      this.chkWebCenter.TabIndex = 27;
      this.chkWebCenter.Text = "WebCenter";
      this.chkWebCenter.Click += new EventHandler(this.chkWebCenter_Click);
      this.txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDescription.Location = new Point(84, 37);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ScrollBars = ScrollBars.Vertical;
      this.txtDescription.Size = new Size(260, 50);
      this.txtDescription.TabIndex = 20;
      this.txtDescription.Validated += new EventHandler(this.txtDescription_Validated);
      this.lblDescription.AutoSize = true;
      this.lblDescription.Location = new Point(7, 40);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(61, 14);
      this.lblDescription.TabIndex = 19;
      this.lblDescription.Text = "Description";
      this.btnConditions.Location = new Point(8, 190);
      this.btnConditions.Name = "btnConditions";
      this.btnConditions.Size = new Size(68, 22);
      this.btnConditions.TabIndex = 12;
      this.btnConditions.Text = "Conditions";
      this.btnConditions.UseVisualStyleBackColor = true;
      this.btnConditions.Click += new EventHandler(this.btnConditions_Click);
      this.lblAvailable.AutoSize = true;
      this.lblAvailable.Location = new Point(7, 346);
      this.lblAvailable.Name = "lblAvailable";
      this.lblAvailable.Size = new Size(51, 14);
      this.lblAvailable.TabIndex = 17;
      this.lblAvailable.Text = "Available";
      this.lstGroups.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lstGroups.BackColor = SystemColors.Control;
      this.lstGroups.FormattingEnabled = true;
      this.lstGroups.IntegralHeight = false;
      this.lstGroups.ItemHeight = 14;
      this.lstGroups.Location = new Point(84, 288);
      this.lstGroups.Name = "lstGroups";
      this.lstGroups.SelectionMode = SelectionMode.None;
      this.lstGroups.Size = new Size(260, 48);
      this.lstGroups.Sorted = true;
      this.lstGroups.TabIndex = 16;
      this.lstGroups.TabStop = false;
      this.lblGroups.AutoSize = true;
      this.lblGroups.Location = new Point(7, 292);
      this.lblGroups.Name = "lblGroups";
      this.lblGroups.Size = new Size(65, 14);
      this.lblGroups.TabIndex = 15;
      this.lblGroups.Text = "Doc Groups";
      this.lstConditions.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lstConditions.BackColor = SystemColors.Control;
      this.lstConditions.FormattingEnabled = true;
      this.lstConditions.IntegralHeight = false;
      this.lstConditions.ItemHeight = 14;
      this.lstConditions.Location = new Point(84, 190);
      this.lstConditions.Name = "lstConditions";
      this.lstConditions.SelectionMode = SelectionMode.None;
      this.lstConditions.Size = new Size(260, 38);
      this.lstConditions.Sorted = true;
      this.lstConditions.TabIndex = 14;
      this.lstConditions.TabStop = false;
      this.lstConditions.MouseHover += new EventHandler(this.lstConditions_MouseHover);
      this.lblConditions.AutoSize = true;
      this.lblConditions.Location = new Point(13, 193);
      this.lblConditions.Name = "lblConditions";
      this.lblConditions.Size = new Size(57, 14);
      this.lblConditions.TabIndex = 13;
      this.lblConditions.Text = "Conditions";
      this.txtAccess.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtAccess.Location = new Point(84, 156);
      this.txtAccess.Name = "txtAccess";
      this.txtAccess.ReadOnly = true;
      this.txtAccess.Size = new Size(260, 20);
      this.txtAccess.TabIndex = 11;
      this.btnAccess.Location = new Point(8, 155);
      this.btnAccess.Name = "btnAccess";
      this.btnAccess.Size = new Size(68, 22);
      this.btnAccess.TabIndex = 9;
      this.btnAccess.Text = "Access";
      this.btnAccess.UseVisualStyleBackColor = true;
      this.btnAccess.Click += new EventHandler(this.btnAccess_Click);
      this.lblAccess.AutoSize = true;
      this.lblAccess.Location = new Point(14, 158);
      this.lblAccess.Name = "lblAccess";
      this.lblAccess.Size = new Size(45, 14);
      this.lblAccess.TabIndex = 10;
      this.lblAccess.Text = "Access";
      this.cboMilestone.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboMilestone.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboMilestone.FormattingEnabled = true;
      this.cboMilestone.ItemHeight = 16;
      this.cboMilestone.Location = new Point(84, 123);
      this.cboMilestone.Name = "cboMilestone";
      this.cboMilestone.SelectedBGColor = SystemColors.Highlight;
      this.cboMilestone.Size = new Size(260, 22);
      this.cboMilestone.TabIndex = 7;
      this.cboMilestone.SelectionChangeCommitted += new EventHandler(this.cboMilestone_SelectionChangeCommitted);
      this.txtMilestone.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtMilestone.Location = new Point(84, 124);
      this.txtMilestone.Name = "txtMilestone";
      this.txtMilestone.ReadOnly = true;
      this.txtMilestone.Size = new Size(267, 20);
      this.txtMilestone.TabIndex = 8;
      this.lblMilestone.AutoSize = true;
      this.lblMilestone.Location = new Point(7, (int) sbyte.MaxValue);
      this.lblMilestone.Name = "lblMilestone";
      this.lblMilestone.Size = new Size(71, 14);
      this.lblMilestone.TabIndex = 6;
      this.lblMilestone.Text = "For Milestone";
      this.cboBorrower.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboBorrower.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboBorrower.FormattingEnabled = true;
      this.cboBorrower.Location = new Point(84, 93);
      this.cboBorrower.Name = "cboBorrower";
      this.cboBorrower.Size = new Size(260, 22);
      this.cboBorrower.TabIndex = 4;
      this.cboBorrower.SelectionChangeCommitted += new EventHandler(this.cboBorrower_SelectionChangeCommitted);
      this.txtBorrower.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBorrower.Location = new Point(84, 94);
      this.txtBorrower.Name = "txtBorrower";
      this.txtBorrower.ReadOnly = true;
      this.txtBorrower.Size = new Size(267, 20);
      this.txtBorrower.TabIndex = 5;
      this.lblBorrower.AutoSize = true;
      this.lblBorrower.Location = new Point(7, 93);
      this.lblBorrower.MaximumSize = new Size(75, 0);
      this.lblBorrower.Name = "lblBorrower";
      this.lblBorrower.Size = new Size(73, 28);
      this.lblBorrower.TabIndex = 3;
      this.lblBorrower.Text = "For Borrower Pair";
      this.cboTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboTitle.FormattingEnabled = true;
      this.cboTitle.Location = new Point(84, 8);
      this.cboTitle.Name = "cboTitle";
      this.cboTitle.Size = new Size(260, 22);
      this.cboTitle.Sorted = true;
      this.cboTitle.TabIndex = 1;
      this.cboTitle.SelectedIndexChanged += new EventHandler(this.cboTitle_SelectedIndexChanged);
      this.cboTitle.SelectionChangeCommitted += new EventHandler(this.cboTitle_SelectionChangeCommitted);
      this.cboTitle.Validating += new CancelEventHandler(this.cboTitle_Validating);
      this.cboTitle.Validated += new EventHandler(this.cboTitle_Validated);
      this.txtTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtTitle.Location = new Point(84, 8);
      this.txtTitle.Name = "txtTitle";
      this.txtTitle.ReadOnly = true;
      this.txtTitle.Size = new Size(267, 20);
      this.txtTitle.TabIndex = 2;
      this.lblTitle.AutoSize = true;
      this.lblTitle.Location = new Point(7, 12);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(34, 14);
      this.lblTitle.TabIndex = 0;
      this.lblTitle.Text = "Name";
      this.pnlRight.Controls.Add((Control) this.pnlViewer);
      this.pnlRight.Controls.Add((Control) this.csFiles);
      this.pnlRight.Controls.Add((Control) this.gcFiles);
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(372, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(440, 702);
      this.pnlRight.TabIndex = 2;
      this.pnlViewer.Controls.Add((Control) this.fileViewer);
      this.pnlViewer.Dock = DockStyle.Fill;
      this.pnlViewer.Location = new Point(0, (int) sbyte.MaxValue);
      this.pnlViewer.Name = "pnlViewer";
      this.pnlViewer.Size = new Size(440, 575);
      this.pnlViewer.TabIndex = 2;
      this.fileViewer.Dock = DockStyle.Fill;
      this.fileViewer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.fileViewer.Location = new Point(1, 1);
      this.fileViewer.Name = "fileViewer";
      this.fileViewer.Size = new Size(438, 573);
      this.fileViewer.TabIndex = 0;
      this.fileViewer.LoadAttachments += new EventHandler(this.fileViewer_LoadAttachments);
      this.csFiles.AnimationDelay = 20;
      this.csFiles.AnimationStep = 20;
      this.csFiles.BorderStyle3D = Border3DStyle.Flat;
      this.csFiles.ControlToHide = (Control) this.gcFiles;
      this.csFiles.Dock = DockStyle.Top;
      this.csFiles.ExpandParentForm = false;
      this.csFiles.Location = new Point(0, 120);
      this.csFiles.Name = "csFiles";
      this.csFiles.TabIndex = 1;
      this.csFiles.TabStop = false;
      this.csFiles.UseAnimations = false;
      this.csFiles.VisualStyle = VisualStyles.Encompass;
      this.gcFiles.Controls.Add((Control) this.pnlToolbar);
      this.gcFiles.Controls.Add((Control) this.gvFiles);
      this.gcFiles.Dock = DockStyle.Top;
      this.gcFiles.HeaderForeColor = SystemColors.ControlText;
      this.gcFiles.Location = new Point(0, 0);
      this.gcFiles.Name = "gcFiles";
      this.gcFiles.Size = new Size(440, 120);
      this.gcFiles.TabIndex = 0;
      this.gcFiles.Text = "Files";
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnRemoveFile);
      this.pnlToolbar.Controls.Add((Control) this.btnMergeFiles);
      this.pnlToolbar.Controls.Add((Control) this.separator);
      this.pnlToolbar.Controls.Add((Control) this.btnMoveFileDown);
      this.pnlToolbar.Controls.Add((Control) this.btnMoveFileUp);
      this.pnlToolbar.Controls.Add((Control) this.btnAttachUnassigned);
      this.pnlToolbar.Controls.Add((Control) this.btnAttachForms);
      this.pnlToolbar.Controls.Add((Control) this.btnAttachScan);
      this.pnlToolbar.Controls.Add((Control) this.btnAttachBrowse);
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(192, 2);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(244, 22);
      this.pnlToolbar.TabIndex = 0;
      this.btnRemoveFile.BackColor = Color.Transparent;
      this.btnRemoveFile.Enabled = false;
      this.btnRemoveFile.Location = new Point(228, 3);
      this.btnRemoveFile.Margin = new Padding(4, 3, 0, 3);
      this.btnRemoveFile.MouseDownImage = (Image) null;
      this.btnRemoveFile.Name = "btnRemoveFile";
      this.btnRemoveFile.Size = new Size(16, 16);
      this.btnRemoveFile.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemoveFile.TabIndex = 28;
      this.btnRemoveFile.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnRemoveFile, "Remove File");
      this.btnRemoveFile.Click += new EventHandler(this.btnRemoveFile_Click);
      this.btnMergeFiles.BackColor = Color.Transparent;
      this.btnMergeFiles.DisabledImage = (Image) Resources.merge_disabled;
      this.btnMergeFiles.Enabled = false;
      this.btnMergeFiles.Image = (Image) Resources.merge;
      this.btnMergeFiles.Location = new Point(208, 3);
      this.btnMergeFiles.Margin = new Padding(4, 3, 0, 3);
      this.btnMergeFiles.MouseDownImage = (Image) null;
      this.btnMergeFiles.MouseOverImage = (Image) Resources.merge_over;
      this.btnMergeFiles.Name = "btnMergeFiles";
      this.btnMergeFiles.Size = new Size(16, 16);
      this.btnMergeFiles.TabIndex = 54;
      this.btnMergeFiles.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnMergeFiles, "Merge Files");
      this.btnMergeFiles.Click += new EventHandler(this.btnMergeFiles_Click);
      this.separator.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.separator.Location = new Point(202, 3);
      this.separator.Margin = new Padding(4, 3, 0, 3);
      this.separator.MaximumSize = new Size(2, 16);
      this.separator.MinimumSize = new Size(2, 16);
      this.separator.Name = "separator";
      this.separator.Size = new Size(2, 16);
      this.separator.TabIndex = 0;
      this.separator.TabStop = false;
      this.btnMoveFileDown.BackColor = Color.Transparent;
      this.btnMoveFileDown.Enabled = false;
      this.btnMoveFileDown.Location = new Point(182, 3);
      this.btnMoveFileDown.Margin = new Padding(4, 3, 0, 3);
      this.btnMoveFileDown.MouseDownImage = (Image) null;
      this.btnMoveFileDown.Name = "btnMoveFileDown";
      this.btnMoveFileDown.Size = new Size(16, 16);
      this.btnMoveFileDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnMoveFileDown.TabIndex = 27;
      this.btnMoveFileDown.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnMoveFileDown, "Move File Down");
      this.btnMoveFileDown.Click += new EventHandler(this.btnMoveFileDown_Click);
      this.btnMoveFileUp.BackColor = Color.Transparent;
      this.btnMoveFileUp.Enabled = false;
      this.btnMoveFileUp.Location = new Point(162, 3);
      this.btnMoveFileUp.Margin = new Padding(4, 3, 0, 3);
      this.btnMoveFileUp.MouseDownImage = (Image) null;
      this.btnMoveFileUp.Name = "btnMoveFileUp";
      this.btnMoveFileUp.Size = new Size(16, 16);
      this.btnMoveFileUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnMoveFileUp.TabIndex = 26;
      this.btnMoveFileUp.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnMoveFileUp, "Move File Up");
      this.btnMoveFileUp.Click += new EventHandler(this.btnMoveFileUp_Click);
      this.btnAttachUnassigned.BackColor = Color.Transparent;
      this.btnAttachUnassigned.DisabledImage = (Image) Resources.attach_unassigned_disabled;
      this.btnAttachUnassigned.Image = (Image) Resources.attach_unassigned;
      this.btnAttachUnassigned.Location = new Point(142, 3);
      this.btnAttachUnassigned.Margin = new Padding(4, 3, 0, 3);
      this.btnAttachUnassigned.MouseDownImage = (Image) null;
      this.btnAttachUnassigned.MouseOverImage = (Image) Resources.attach_unassigned_over;
      this.btnAttachUnassigned.Name = "btnAttachUnassigned";
      this.btnAttachUnassigned.Size = new Size(16, 16);
      this.btnAttachUnassigned.TabIndex = 47;
      this.btnAttachUnassigned.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAttachUnassigned, "Attach Unassigned Files");
      this.btnAttachUnassigned.Click += new EventHandler(this.btnAttachUnassigned_Click);
      this.btnAttachForms.BackColor = Color.Transparent;
      this.btnAttachForms.DisabledImage = (Image) Resources.attach_forms_disabled;
      this.btnAttachForms.Image = (Image) Resources.attach_forms;
      this.btnAttachForms.Location = new Point(122, 3);
      this.btnAttachForms.Margin = new Padding(4, 3, 0, 3);
      this.btnAttachForms.MouseDownImage = (Image) null;
      this.btnAttachForms.MouseOverImage = (Image) Resources.attach_forms_over;
      this.btnAttachForms.Name = "btnAttachForms";
      this.btnAttachForms.Size = new Size(16, 16);
      this.btnAttachForms.TabIndex = 44;
      this.btnAttachForms.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAttachForms, "Attach Encompass Forms");
      this.btnAttachForms.Click += new EventHandler(this.btnAttachForms_Click);
      this.btnAttachScan.BackColor = Color.Transparent;
      this.btnAttachScan.DisabledImage = (Image) Resources.attach_scan_disabled;
      this.btnAttachScan.Image = (Image) Resources.attach_scan;
      this.btnAttachScan.Location = new Point(102, 3);
      this.btnAttachScan.Margin = new Padding(4, 3, 0, 3);
      this.btnAttachScan.MouseDownImage = (Image) null;
      this.btnAttachScan.MouseOverImage = (Image) Resources.attach_scan_over;
      this.btnAttachScan.Name = "btnAttachScan";
      this.btnAttachScan.Size = new Size(16, 16);
      this.btnAttachScan.TabIndex = 45;
      this.btnAttachScan.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAttachScan, "Scan and Attach");
      this.btnAttachScan.Click += new EventHandler(this.btnAttachScan_Click);
      this.btnAttachBrowse.BackColor = Color.Transparent;
      this.btnAttachBrowse.DisabledImage = (Image) Resources.attach_browse_disabled;
      this.btnAttachBrowse.Image = (Image) Resources.attach_browse;
      this.btnAttachBrowse.Location = new Point(82, 3);
      this.btnAttachBrowse.Margin = new Padding(4, 3, 0, 3);
      this.btnAttachBrowse.MouseDownImage = (Image) null;
      this.btnAttachBrowse.MouseOverImage = (Image) Resources.attach_browse_over;
      this.btnAttachBrowse.Name = "btnAttachBrowse";
      this.btnAttachBrowse.Size = new Size(16, 16);
      this.btnAttachBrowse.TabIndex = 46;
      this.btnAttachBrowse.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAttachBrowse, "Browse and Attach");
      this.btnAttachBrowse.Click += new EventHandler(this.btnAttachBrowse_Click);
      this.gvFiles.AllowDrop = true;
      this.gvFiles.BorderStyle = BorderStyle.None;
      this.gvFiles.ClearSelectionsOnEmptyRowClick = false;
      this.gvFiles.Dock = DockStyle.Fill;
      this.gvFiles.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvFiles.Location = new Point(1, 26);
      this.gvFiles.Name = "gvFiles";
      this.gvFiles.Size = new Size(438, 93);
      this.gvFiles.SortingType = SortingType.AlphaNumeric;
      this.gvFiles.TabIndex = 1;
      this.gvFiles.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvFiles.UseCompatibleEditingBehavior = true;
      this.gvFiles.BeforeSelectedIndexCommitted += new CancelEventHandler(this.gvFiles_BeforeSelectedIndexCommitted);
      this.gvFiles.SelectedIndexCommitted += new EventHandler(this.gvFiles_SelectedIndexCommitted);
      this.gvFiles.SubItemCheck += new GVSubItemEventHandler(this.gvFiles_SubItemCheck);
      this.gvFiles.ItemDoubleClick += new GVItemEventHandler(this.gvFiles_ItemDoubleClick);
      this.gvFiles.EditorClosing += new GVSubItemEditingEventHandler(this.gvFiles_EditorClosing);
      this.gvFiles.DragDrop += new DragEventHandler(this.gvFiles_DragDrop);
      this.gvFiles.DragOver += new DragEventHandler(this.gvFiles_DragOver);
      this.pnlClose.Controls.Add((Control) this.helpLink);
      this.pnlClose.Controls.Add((Control) this.btnClose);
      this.pnlClose.Dock = DockStyle.Bottom;
      this.pnlClose.Location = new Point(0, 702);
      this.pnlClose.Name = "pnlClose";
      this.pnlClose.Size = new Size(812, 40);
      this.pnlClose.TabIndex = 3;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "Documents Details";
      this.helpLink.Location = new Point(8, 12);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 17);
      this.helpLink.TabIndex = 1;
      this.helpLink.TabStop = false;
      this.btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.OK;
      this.btnClose.Location = new Point(729, 9);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 22);
      this.btnClose.TabIndex = 0;
      this.btnClose.TabStop = false;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.csLeft.AnimationDelay = 20;
      this.csLeft.AnimationStep = 20;
      this.csLeft.BorderStyle3D = Border3DStyle.Flat;
      this.csLeft.ControlToHide = (Control) this.pnlLeft;
      this.csLeft.ExpandParentForm = false;
      this.csLeft.Location = new Point(365, 0);
      this.csLeft.Name = "csDocTracking";
      this.csLeft.TabIndex = 1;
      this.csLeft.TabStop = false;
      this.csLeft.UseAnimations = false;
      this.csLeft.VisualStyle = VisualStyles.Encompass;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(812, 742);
      this.Controls.Add((Control) this.pnlRight);
      this.Controls.Add((Control) this.csLeft);
      this.Controls.Add((Control) this.pnlLeft);
      this.Controls.Add((Control) this.pnlClose);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = Resources.icon_allsizes_bug;
      this.KeyPreview = true;
      this.Name = nameof (DocumentDetailsDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Document Details";
      this.Activated += new EventHandler(this.DocumentDetailsDialog_Activated);
      this.FormClosing += new FormClosingEventHandler(this.DocumentDetailsDialog_FormClosing);
      this.KeyDown += new KeyEventHandler(this.DocumentDetailsDialog_KeyDown);
      this.pnlLeft.ResumeLayout(false);
      this.gcTracking.ResumeLayout(false);
      this.tabTracking.ResumeLayout(false);
      this.pageStatus.ResumeLayout(false);
      this.pageStatus.PerformLayout();
      ((ISupportInitialize) this.btnShippingReadyBy).EndInit();
      ((ISupportInitialize) this.btnUnderwritingReadyBy).EndInit();
      ((ISupportInitialize) this.btnReviewedBy).EndInit();
      ((ISupportInitialize) this.btnReceivedBy).EndInit();
      ((ISupportInitialize) this.btnRerequestedBy).EndInit();
      ((ISupportInitialize) this.btnRequestedBy).EndInit();
      this.pageComments.ResumeLayout(false);
      this.gcDetails.ResumeLayout(false);
      this.pnlDetails.ResumeLayout(false);
      this.pnlDetails.PerformLayout();
      this.pnlRight.ResumeLayout(false);
      this.pnlViewer.ResumeLayout(false);
      this.gcFiles.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemoveFile).EndInit();
      ((ISupportInitialize) this.btnMergeFiles).EndInit();
      ((ISupportInitialize) this.btnMoveFileDown).EndInit();
      ((ISupportInitialize) this.btnMoveFileUp).EndInit();
      ((ISupportInitialize) this.btnAttachUnassigned).EndInit();
      ((ISupportInitialize) this.btnAttachForms).EndInit();
      ((ISupportInitialize) this.btnAttachScan).EndInit();
      ((ISupportInitialize) this.btnAttachBrowse).EndInit();
      this.pnlClose.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
