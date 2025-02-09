// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.PreliminaryDetailsDialog
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
using EllieMae.EMLite.eFolder.Documents;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.eFolder.UI;
using EllieMae.EMLite.eFolder.Utilities;
using EllieMae.EMLite.eFolder.Viewers;
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
namespace EllieMae.EMLite.eFolder.Conditions
{
  public class PreliminaryDetailsDialog : Form
  {
    private const string className = "PreliminaryDetailsDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private static List<PreliminaryDetailsDialog> _instanceList = new List<PreliminaryDetailsDialog>();
    private LoanDataMgr loanDataMgr;
    private PreliminaryConditionLog cond;
    private UnderwritingConditionTrackingSetup condSetup;
    private GridViewDataManager gvDocumentsMgr;
    private RoleInfo underwriterRole;
    private eFolderAccessRights rights;
    private bool refreshDocuments;
    private IContainer components;
    private Panel pnlLeft;
    private GroupContainer gcTracking;
    private GroupContainer gcDetails;
    private GroupContainer gcDocuments;
    private CollapsibleSplitter csLeft;
    internal GridView gvDocuments;
    private TabControl tabTracking;
    private TabPage pageStatus;
    private TabPage pageComments;
    private ComboBox cboTitle;
    private Label lblTitle;
    private Label lblDescription;
    private Label lblBorrower;
    private Label lblPriorTo;
    private FileAttachmentViewerControl fileViewer;
    private CheckBox chkAdded;
    private DateTimePicker dateFulfilled;
    private CheckBox chkFulfilled;
    private Panel pnlRight;
    private CollapsibleSplitter csFiles;
    private BorderPanel pnlViewer;
    private CollapsibleSplitter csDetails;
    private Panel pnlDetails;
    private TextBox txtDescription;
    private ComboBox cboBorrower;
    private TextBox txtSource;
    private Label lblSource;
    private ComboBox cboCategory;
    private Label lblCategory;
    private System.Windows.Forms.LinkLabel lnkViewMore;
    private TextBox txtAddedBy;
    private TextBox txtFulfilledBy;
    private StandardIconButton btnFulfilledBy;
    private ComboBox cboPriorTo;
    private TextBox txtAddedDate;
    private CheckBox chkUnderwriter;
    private Panel pnlClose;
    private Button btnClose;
    private VerticalSeparator separator;
    private Button btnRequestDocument;
    private StandardIconButton btnAddDocument;
    private StandardIconButton btnRemoveDocument;
    private StandardIconButton btnEditDocument;
    private CommentCollectionControl commentCollection;
    private TextBox txtTitle;
    private TextBox txtBorrower;
    private TextBox txtCategory;
    private TextBox txtPriorTo;
    private TextBox txtDateFulfilled;
    private FlowLayoutPanel pnlToolbar;
    private ToolTip tooltip;
    private EMHelpLink helpLink;
    private TextBox txtDateDue;
    private TextBox txtDaysDue;
    private Label lblDaysDue;
    private TextBox txtRequestedFrom;
    private Label lblRequestedFrom;
    private StandardIconButton btnReceivedBy;
    private TextBox txtReceivedBy;
    private DateTimePicker dateReceived;
    private TextBox txtDateReceived;
    private CheckBox chkReceived;
    private StandardIconButton btnRerequestedBy;
    private TextBox txtRerequestedBy;
    private DateTimePicker dateRerequested;
    private TextBox txtDateRerequested;
    private CheckBox chkRerequested;
    private StandardIconButton btnRequestedBy;
    private TextBox txtRequestedBy;
    private DateTimePicker dateRequested;
    private TextBox txtDateRequested;
    private CheckBox chkRequested;

    public static void ShowInstance(LoanDataMgr loanDataMgr, PreliminaryConditionLog cond)
    {
      if (Form.ActiveForm != null && Form.ActiveForm.Modal)
      {
        using (PreliminaryDetailsDialog preliminaryDetailsDialog = new PreliminaryDetailsDialog(loanDataMgr, cond))
        {
          int num = (int) preliminaryDetailsDialog.ShowDialog((IWin32Window) Form.ActiveForm);
        }
      }
      else
      {
        PreliminaryDetailsDialog preliminaryDetailsDialog1 = (PreliminaryDetailsDialog) null;
        foreach (PreliminaryDetailsDialog instance in PreliminaryDetailsDialog._instanceList)
        {
          if (instance.Condition == cond)
            preliminaryDetailsDialog1 = instance;
        }
        if (preliminaryDetailsDialog1 == null)
        {
          PreliminaryDetailsDialog preliminaryDetailsDialog2 = new PreliminaryDetailsDialog(loanDataMgr, cond);
          preliminaryDetailsDialog2.FormClosing += new FormClosingEventHandler(PreliminaryDetailsDialog._instance_FormClosing);
          PreliminaryDetailsDialog._instanceList.Add(preliminaryDetailsDialog2);
          preliminaryDetailsDialog2.Show();
        }
        else
        {
          if (preliminaryDetailsDialog1.WindowState == FormWindowState.Minimized)
            preliminaryDetailsDialog1.WindowState = FormWindowState.Normal;
          preliminaryDetailsDialog1.Activate();
        }
      }
    }

    public static void CloseInstances()
    {
      if (PreliminaryDetailsDialog._instanceList == null && PreliminaryDetailsDialog._instanceList.Count == 0)
        return;
      int num = PreliminaryDetailsDialog._instanceList.Count - 1;
      try
      {
        for (int index = num; index >= 0; --index)
          PreliminaryDetailsDialog._instanceList[index].Close();
      }
      catch
      {
      }
    }

    private static void _instance_FormClosing(object sender, FormClosingEventArgs e)
    {
      PreliminaryDetailsDialog preliminaryDetailsDialog = (PreliminaryDetailsDialog) sender;
      PreliminaryDetailsDialog._instanceList.Remove(preliminaryDetailsDialog);
    }

    private PreliminaryDetailsDialog(LoanDataMgr loanDataMgr, PreliminaryConditionLog cond)
    {
      this.InitializeComponent();
      this.setWindowSize();
      this.loanDataMgr = loanDataMgr;
      this.cond = cond;
      this.rights = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) cond);
      this.initEventHandlers();
      this.initTitleField();
      this.initBorrowerField();
      this.initFulfilledFields();
      this.initDocumentList();
      this.loadConditionDetails();
      this.loadDocumentList(true);
      this.applySecurity();
      int[] usersAssignedRoles = loanDataMgr.AccessRules.GetUsersAssignedRoles();
      if (cond.Comments.HasUnreviewedEntry(usersAssignedRoles))
      {
        cond.Comments.MarkAsReviewed(DateTime.Now, Session.UserID, usersAssignedRoles);
        this.tabTracking.SelectedTab = this.pageComments;
      }
      if (!(cond.Title != "Untitled"))
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

    public PreliminaryConditionLog Condition => this.cond;

    private void loadConditionDetails()
    {
      this.loadTitleField();
      this.loadDescriptionField();
      this.loadBorrowerField();
      this.loadSourceField();
      this.loadCategoryField();
      this.loadPriorToField();
      this.loadUnderwriterField();
      this.loadExpectedFields();
      this.loadRequestedFromField();
      this.loadAddedFields();
      this.loadRequestedFields();
      this.loadRerequestedFields();
      this.loadReceivedFields();
      this.loadFulfilledFields();
      this.loadCommentList();
    }

    private void initTitleField()
    {
      this.condSetup = this.loanDataMgr.SystemConfiguration.UnderwritingConditionTrackingSetup;
      foreach (ConditionTemplate conditionTemplate in (CollectionBase) this.condSetup)
        this.cboTitle.Items.Add((object) conditionTemplate.Name);
    }

    private void loadTitleField()
    {
      this.cboTitle.Text = this.cond.Title;
      this.txtTitle.Text = this.cond.Title;
      this.Text = "Preliminary Condition Details (" + this.cond.Title + ")";
    }

    private void cboTitle_SelectionChangeCommitted(object sender, EventArgs e)
    {
      string selectedItem = (string) this.cboTitle.SelectedItem;
      this.Text = "Preliminary Condition Details (" + selectedItem + ")";
      UnderwritingConditionTemplate byName = this.condSetup.GetByName(selectedItem);
      if (byName == null)
        return;
      this.cond.Title = selectedItem;
      this.cond.Description = byName.Description;
      this.cond.Category = byName.Category;
      this.cond.PriorTo = byName.PriorTo;
      this.cond.DaysTillDue = byName.DaysTillDue;
      this.loanDataMgr.LinkCondition((ConditionLog) this.cond);
    }

    private void cboTitle_Validating(object sender, CancelEventArgs e)
    {
      if (!(this.cboTitle.Text.Trim() == string.Empty))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "You must specify a name for the condition.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      e.Cancel = true;
    }

    private void cboTitle_Validated(object sender, EventArgs e)
    {
      if (this.cond.Title == this.cboTitle.Text)
        return;
      this.cond.Title = this.cboTitle.Text;
    }

    private void loadDescriptionField()
    {
      this.txtDescription.Text = this.cond.Description;
      this.lnkViewMore.Enabled = this.cond.Details != string.Empty;
    }

    private void txtDescription_Validated(object sender, EventArgs e)
    {
      if (!(this.cond.Description != this.txtDescription.Text))
        return;
      this.cond.Description = this.txtDescription.Text;
    }

    private void lnkViewMore_Click(object sender, EventArgs e)
    {
      using (DescriptionDetailsDialog descriptionDetailsDialog = new DescriptionDetailsDialog(this.cond.Details))
      {
        int num = (int) descriptionDetailsDialog.ShowDialog((IWin32Window) this);
      }
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
        if (borrowerPair.Id == this.cond.PairId)
          this.cboBorrower.SelectedItem = (object) borrowerPair;
      }
      this.txtBorrower.Text = this.cboBorrower.Text;
    }

    private void cboBorrower_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.cond.PairId = ((BorrowerPair) this.cboBorrower.SelectedItem).Id;
    }

    private void loadSourceField() => this.txtSource.Text = this.cond.Source;

    private void loadCategoryField()
    {
      this.cboCategory.SelectedItem = !this.cboCategory.Items.Contains((object) this.cond.Category) ? (object) null : (object) this.cond.Category;
      this.txtCategory.Text = this.cboCategory.Text;
    }

    private void cboCategory_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.cond.Category = (string) this.cboCategory.SelectedItem;
    }

    private void loadPriorToField()
    {
      string str = (string) null;
      switch (this.cond.PriorTo)
      {
        case "PTA":
          str = "Approval";
          break;
        case "PTD":
          str = "Docs";
          break;
        case "PTF":
          str = "Funding";
          break;
        case "AC":
          str = "Closing";
          break;
        case "PTP":
          str = "Purchase";
          break;
      }
      this.cboPriorTo.SelectedItem = (object) str;
      this.txtPriorTo.Text = this.cboPriorTo.Text;
    }

    private void cboPriorTo_SelectionChangeCommitted(object sender, EventArgs e)
    {
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
      this.cond.PriorTo = str;
    }

    private void loadUnderwriterField()
    {
      this.chkUnderwriter.Checked = this.cond.UnderwriterAccess;
    }

    private void chkUnderwriter_Click(object sender, EventArgs e)
    {
      this.cond.UnderwriterAccess = this.chkUnderwriter.Checked;
    }

    private void loadExpectedFields()
    {
      if (this.cond.DaysTillDue > 0)
        this.txtDaysDue.Text = this.cond.DaysTillDue.ToString();
      else
        this.txtDaysDue.Text = string.Empty;
      if (this.cond.Expected)
        this.txtDateDue.Text = this.cond.DateExpected.ToString("MM/dd/yy");
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
      if (this.cond.DaysTillDue == num)
        return;
      this.cond.DaysTillDue = num;
    }

    private void loadRequestedFromField() => this.txtRequestedFrom.Text = this.cond.RequestedFrom;

    private void txtRequestedFrom_Validated(object sender, EventArgs e)
    {
      if (!(this.cond.RequestedFrom != this.txtRequestedFrom.Text))
        return;
      this.cond.RequestedFrom = this.txtRequestedFrom.Text;
    }

    private void loadAddedFields()
    {
      this.txtAddedDate.Text = this.cond.DateAdded.ToString("MM/dd/yy hh:mm tt");
      this.txtAddedBy.Text = this.cond.AddedBy;
    }

    private void loadRequestedFields()
    {
      this.chkRequested.Checked = this.cond.Requested;
      if (this.chkRequested.Checked)
      {
        this.dateRequested.Value = this.cond.DateRequested;
        this.txtDateRequested.Text = this.cond.DateRequested.ToString(this.dateRequested.CustomFormat);
        this.txtRequestedBy.Text = this.cond.RequestedBy;
      }
      this.dateRequested.Visible = this.chkRequested.Checked && this.rights.CanEditPreliminaryCondition;
      this.txtDateRequested.Visible = this.chkRequested.Checked && !this.rights.CanEditPreliminaryCondition;
      this.txtRequestedBy.Visible = this.chkRequested.Checked;
      this.btnRequestedBy.Visible = this.chkRequested.Checked && this.rights.CanEditPreliminaryCondition;
    }

    private void chkRequested_Click(object sender, EventArgs e)
    {
      if (this.chkRequested.Checked)
        this.cond.MarkAsRequested(DateTime.Now, Session.UserID);
      else
        this.cond.UnmarkAsRequested();
    }

    private void dateRequested_ValueChanged(object sender, EventArgs e)
    {
      if (!(this.dateRequested.Value != this.cond.DateRequested))
        return;
      this.cond.MarkAsRequested(this.dateRequested.Value, this.txtRequestedBy.Text);
    }

    private void btnRequestedBy_Click(object sender, EventArgs e)
    {
      string user = this.selectUser();
      if (user == null)
        return;
      this.cond.MarkAsRequested(this.cond.DateRequested, user);
    }

    private void loadRerequestedFields()
    {
      this.chkRerequested.Checked = this.cond.Rerequested;
      if (this.chkRerequested.Checked)
      {
        this.dateRerequested.Value = this.cond.DateRerequested;
        this.txtDateRerequested.Text = this.cond.DateRerequested.ToString(this.dateRerequested.CustomFormat);
        this.txtRerequestedBy.Text = this.cond.RerequestedBy;
      }
      this.dateRerequested.Visible = this.chkRerequested.Checked && this.rights.CanEditPreliminaryCondition;
      this.txtDateRerequested.Visible = this.chkRerequested.Checked && !this.rights.CanEditPreliminaryCondition;
      this.txtRerequestedBy.Visible = this.chkRerequested.Checked;
      this.btnRerequestedBy.Visible = this.chkRerequested.Checked && this.rights.CanEditPreliminaryCondition;
    }

    private void chkRerequested_Click(object sender, EventArgs e)
    {
      if (this.chkRerequested.Checked)
        this.cond.MarkAsRerequested(DateTime.Now, Session.UserID);
      else
        this.cond.UnmarkAsRerequested();
    }

    private void dateRerequested_ValueChanged(object sender, EventArgs e)
    {
      if (!(this.dateRerequested.Value != this.cond.DateRerequested))
        return;
      this.cond.MarkAsRerequested(this.dateRerequested.Value, this.txtRerequestedBy.Text);
    }

    private void btnRerequestedBy_Click(object sender, EventArgs e)
    {
      string user = this.selectUser();
      if (user == null)
        return;
      this.cond.MarkAsRerequested(this.cond.DateRerequested, user);
    }

    private void loadReceivedFields()
    {
      this.chkReceived.Checked = this.cond.Received;
      if (this.chkReceived.Checked)
      {
        this.dateReceived.Value = this.cond.DateReceived;
        this.txtDateReceived.Text = this.cond.DateReceived.ToString(this.dateReceived.CustomFormat);
        this.txtReceivedBy.Text = this.cond.ReceivedBy;
      }
      this.dateReceived.Visible = this.chkReceived.Checked && this.rights.CanEditPreliminaryCondition;
      this.txtDateReceived.Visible = this.chkReceived.Checked && !this.rights.CanEditPreliminaryCondition;
      this.txtReceivedBy.Visible = this.chkReceived.Checked;
      this.btnReceivedBy.Visible = this.chkReceived.Checked && this.rights.CanEditPreliminaryCondition;
    }

    private void chkReceived_Click(object sender, EventArgs e)
    {
      if (this.chkReceived.Checked)
        this.cond.MarkAsReceived(DateTime.Now, Session.UserID);
      else
        this.cond.UnmarkAsReceived();
    }

    private void dateReceived_ValueChanged(object sender, EventArgs e)
    {
      if (!(this.dateReceived.Value != this.cond.DateReceived))
        return;
      this.cond.MarkAsReceived(this.dateReceived.Value, this.txtReceivedBy.Text);
    }

    private void btnReceivedBy_Click(object sender, EventArgs e)
    {
      string user = this.selectUser();
      if (user == null)
        return;
      this.cond.MarkAsReceived(this.cond.DateReceived, user);
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

    private void loadFulfilledFields()
    {
      this.chkFulfilled.Checked = this.cond.Fulfilled;
      if (this.chkFulfilled.Checked)
      {
        this.dateFulfilled.Value = this.cond.DateFulfilled;
        this.txtDateFulfilled.Text = this.cond.DateFulfilled.ToString(this.dateFulfilled.CustomFormat);
        this.txtFulfilledBy.Text = this.cond.FulfilledBy;
      }
      this.dateFulfilled.Visible = this.chkFulfilled.Checked && this.rights.CanEditPreliminaryCondition;
      this.txtDateFulfilled.Visible = this.chkFulfilled.Checked && !this.rights.CanEditPreliminaryCondition;
      this.txtFulfilledBy.Visible = this.chkFulfilled.Checked;
      this.btnFulfilledBy.Visible = this.chkFulfilled.Checked && this.rights.CanEditPreliminaryCondition;
    }

    private void checkUnderwriterAccess()
    {
      if (this.underwriterRole == null || !new eFolderAccessRights(this.loanDataMgr).CanSetDocumentAccess)
        return;
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      foreach (DocumentLog linkedDocument in this.cond.GetLinkedDocuments())
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

    private void chkFulfilled_Click(object sender, EventArgs e)
    {
      if (this.chkFulfilled.Checked)
      {
        this.cond.MarkAsFulfilled(DateTime.Now, Session.UserID);
        this.checkUnderwriterAccess();
      }
      else
        this.cond.UnmarkAsFulfilled();
    }

    private void dateFulfilled_ValueChanged(object sender, EventArgs e)
    {
      if (!(this.dateFulfilled.Value != this.cond.DateFulfilled))
        return;
      this.cond.MarkAsFulfilled(this.dateFulfilled.Value, this.txtFulfilledBy.Text);
    }

    private void btnFulfilledBy_Click(object sender, EventArgs e)
    {
      string user = this.selectUser();
      if (user == null)
        return;
      this.cond.MarkAsFulfilled(this.cond.DateFulfilled, user);
    }

    private void loadCommentList()
    {
      this.commentCollection.LoadComments(this.loanDataMgr, this.cond.Comments);
    }

    private void initDocumentList()
    {
      this.gvDocumentsMgr = new GridViewDataManager(this.gvDocuments, this.loanDataMgr);
      this.gvDocumentsMgr.CreateLayout(new TableLayout.Column[6]
      {
        GridViewDataManager.HasAttachmentsColumn,
        GridViewDataManager.NameColumn,
        GridViewDataManager.BorrowerColumn,
        GridViewDataManager.DocAccessColumn,
        GridViewDataManager.DocStatusColumn,
        GridViewDataManager.DateColumn
      });
      this.gvDocuments.Sort(1, SortOrder.Ascending);
    }

    private void loadDocumentList(bool showAll)
    {
      DocumentLog[] documentLogArray = this.cond.GetLinkedDocuments();
      if (AutoAssignUtils.IsNGAutoAssignEnabled)
        documentLogArray = new AutoAssignUtils().GetRefreshedLoanDocumentLogs(documentLogArray);
      foreach (DocumentLog doc in documentLogArray)
      {
        GVItem itemByTag = this.gvDocuments.Items.GetItemByTag((object) doc);
        if (itemByTag == null)
          this.gvDocumentsMgr.AddItem(doc);
        else
          this.gvDocumentsMgr.RefreshItem(itemByTag, doc);
      }
      List<GVItem> gvItemList = new List<GVItem>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocuments.Items)
      {
        if (Array.IndexOf<object>((object[]) documentLogArray, gvItem.Tag) < 0)
          gvItemList.Add(gvItem);
      }
      foreach (GVItem gvItem in gvItemList)
        this.gvDocuments.Items.Remove(gvItem);
      this.gvDocuments.ReSort();
      if (!showAll)
        return;
      this.showDocumentFiles(documentLogArray);
    }

    private DocumentLog[] getSelectedDocuments()
    {
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      foreach (GVItem selectedItem in this.gvDocuments.SelectedItems)
        documentLogList.Add((DocumentLog) selectedItem.Tag);
      return documentLogList.ToArray();
    }

    private FileAttachment[] getDocumentFiles()
    {
      return this.loanDataMgr.FileAttachments.GetAttachments(this.getSelectedDocuments());
    }

    private void showDocumentFiles()
    {
      FileAttachment[] documentFiles = this.getDocumentFiles();
      if (documentFiles.Length != 0)
        this.fileViewer.LoadFiles(documentFiles);
      else
        this.fileViewer.CloseFile();
      this.refreshToolbar();
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

    private void gvDocuments_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.btnEditDocument_Click(source, EventArgs.Empty);
    }

    private void gvDocuments_BeforeSelectedIndexCommitted(object sender, CancelEventArgs e)
    {
      if (this.fileViewer.CanCloseViewer())
        return;
      e.Cancel = true;
    }

    private void gvDocuments_SelectedIndexCommitted(object sender, EventArgs e)
    {
      this.showDocumentFiles();
    }

    private void refreshToolbar()
    {
      int count = this.gvDocuments.SelectedItems.Count;
      this.btnEditDocument.Enabled = count == 1;
      this.btnRemoveDocument.Enabled = count > 0;
    }

    private void btnAddDocument_Click(object sender, EventArgs e)
    {
      using (AssignDocumentsDialog assignDocumentsDialog = new AssignDocumentsDialog(this.loanDataMgr, (ConditionLog) this.cond))
      {
        if (assignDocumentsDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return;
        this.showDocumentFiles(assignDocumentsDialog.Documents);
      }
    }

    private void btnEditDocument_Click(object sender, EventArgs e)
    {
      DocumentLog[] selectedDocuments = this.getSelectedDocuments();
      if (selectedDocuments.Length != 1)
        return;
      DocumentDetailsDialog.ShowInstance(this.loanDataMgr, selectedDocuments[0]);
    }

    private void btnRemoveDocument_Click(object sender, EventArgs e)
    {
      DocumentLog[] selectedDocuments = this.getSelectedDocuments();
      string str = string.Empty;
      foreach (DocumentLog documentLog in selectedDocuments)
        str = str + documentLog.Title + "\r\n";
      if (Utils.Dialog((IWin32Window) Form.ActiveForm, "Are you sure that you want to remove the following document(s):\r\n\r\n" + str, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return;
      foreach (DocumentLog documentLog in selectedDocuments)
        documentLog.Conditions.Remove((ConditionLog) this.cond);
    }

    private void btnRequestDocument_Click(object sender, EventArgs e)
    {
      new eFolderManager().Request(this.loanDataMgr, this.getSelectedDocuments(), (ConditionLog) this.cond);
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

    private void applySecurity()
    {
      this.cboTitle.Visible = this.rights.CanEditPreliminaryCondition;
      this.txtTitle.Visible = !this.rights.CanEditPreliminaryCondition;
      this.txtDescription.ReadOnly = !this.rights.CanEditPreliminaryCondition;
      this.cboBorrower.Visible = this.rights.CanEditPreliminaryCondition;
      this.txtBorrower.Visible = !this.rights.CanEditPreliminaryCondition;
      this.cboCategory.Visible = this.rights.CanEditPreliminaryCondition;
      this.txtCategory.Visible = !this.rights.CanEditPreliminaryCondition;
      this.cboPriorTo.Visible = this.rights.CanEditPreliminaryCondition;
      this.txtPriorTo.Visible = !this.rights.CanEditPreliminaryCondition;
      this.chkUnderwriter.Enabled = this.rights.CanEditPreliminaryCondition;
      this.chkFulfilled.Enabled = this.rights.CanEditPreliminaryCondition;
      this.commentCollection.CanAddComment = this.rights.CanEditPreliminaryCondition;
      this.commentCollection.CanDeleteComment = this.rights.CanEditPreliminaryCondition;
      this.btnAddDocument.Visible = this.rights.CanAddPreliminaryConditionDocuments;
      this.btnRemoveDocument.Visible = this.rights.CanRemovePreliminaryConditionDocuments;
      this.separator.Visible = this.rights.CanRequestDocuments || this.rights.CanRequestServices;
      this.btnRequestDocument.Visible = this.rights.CanRequestDocuments || this.rights.CanRequestServices;
      if (!Session.IsBrokerEdition())
        return;
      this.chkUnderwriter.Visible = false;
      this.gcDetails.Height -= this.pnlDetails.Height - this.chkUnderwriter.Top;
    }

    private void initEventHandlers()
    {
      this.FormClosed += new FormClosedEventHandler(this.onFormClosed);
      this.loanDataMgr.LoanClosing += new EventHandler(this.btnClose_Click);
      this.loanDataMgr.OnLoanRefreshedFromServer += new EventHandler(this.onLoanRefreshedFromServer);
      this.loanDataMgr.LoanData.LogRecordChanged += new LogRecordEventHandler(this.logRecordChanged);
      this.loanDataMgr.LoanData.LogRecordRemoved += new LogRecordEventHandler(this.logRecordRemoved);
    }

    private void releaseEventHandlers()
    {
      this.FormClosed -= new FormClosedEventHandler(this.onFormClosed);
      this.loanDataMgr.LoanClosing -= new EventHandler(this.btnClose_Click);
      this.loanDataMgr.OnLoanRefreshedFromServer -= new EventHandler(this.onLoanRefreshedFromServer);
      this.loanDataMgr.LoanData.LogRecordChanged -= new LogRecordEventHandler(this.logRecordChanged);
      this.loanDataMgr.LoanData.LogRecordRemoved -= new LogRecordEventHandler(this.logRecordRemoved);
    }

    private void onFormClosed(object sender, FormClosedEventArgs e) => this.releaseEventHandlers();

    private void logRecordChanged(object source, LogRecordEventArgs e)
    {
      Tracing.Log(PreliminaryDetailsDialog.sw, TraceLevel.Verbose, nameof (PreliminaryDetailsDialog), "Checking InvokeRequired For LogRecordChanged");
      if (this.InvokeRequired)
      {
        LogRecordEventHandler method = new LogRecordEventHandler(this.logRecordChanged);
        Tracing.Log(PreliminaryDetailsDialog.sw, TraceLevel.Verbose, nameof (PreliminaryDetailsDialog), "Calling BeginInvoke For LogRecordChanged");
        this.BeginInvoke((Delegate) method, source, (object) e);
      }
      else if (e.LogRecord == this.cond)
      {
        this.loadConditionDetails();
      }
      else
      {
        if (!(e.LogRecord is DocumentLog))
          return;
        DocumentLog logRecord = (DocumentLog) e.LogRecord;
        if (!this.gvDocuments.Items.ContainsTag((object) logRecord) && !logRecord.Conditions.Contains((ConditionLog) this.cond))
          return;
        if (this == Form.ActiveForm)
          this.loadDocumentList(false);
        else
          this.refreshDocuments = true;
      }
    }

    private void logRecordRemoved(object source, LogRecordEventArgs e)
    {
      Tracing.Log(PreliminaryDetailsDialog.sw, TraceLevel.Verbose, nameof (PreliminaryDetailsDialog), "Checking InvokeRequired For LogRecordRemoved");
      if (this.InvokeRequired)
      {
        LogRecordEventHandler method = new LogRecordEventHandler(this.logRecordRemoved);
        Tracing.Log(PreliminaryDetailsDialog.sw, TraceLevel.Verbose, nameof (PreliminaryDetailsDialog), "Calling BeginInvoke For LogRecordRemoved");
        this.BeginInvoke((Delegate) method, source, (object) e);
      }
      else
      {
        if (e.LogRecord != this.cond)
          return;
        this.Close();
      }
    }

    private void onLoanRefreshedFromServer(object sender, EventArgs e)
    {
      this.cond = (PreliminaryConditionLog) this.loanDataMgr.LoanData.GetLogList().GetRecordByID(this.cond.Guid);
      this.loadConditionDetails();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      if (sender is LoanDataMgr)
        this.AutoValidate = AutoValidate.Disable;
      this.Close();
    }

    private void PreliminaryDetailsDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      Session.Application.DisplayHelp(this.helpLink.HelpTag);
    }

    private void PreliminaryDetailsDialog_Activated(object sender, EventArgs e)
    {
      if (!this.refreshDocuments)
        return;
      this.loadDocumentList(false);
      this.showDocumentFiles();
      this.refreshDocuments = false;
    }

    private void PreliminaryDetailsDialog_FormClosing(object sender, FormClosingEventArgs e)
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PreliminaryDetailsDialog));
      this.pnlLeft = new Panel();
      this.gcTracking = new GroupContainer();
      this.tabTracking = new TabControl();
      this.pageStatus = new TabPage();
      this.btnReceivedBy = new StandardIconButton();
      this.txtReceivedBy = new TextBox();
      this.dateReceived = new DateTimePicker();
      this.txtDateReceived = new TextBox();
      this.chkReceived = new CheckBox();
      this.btnRerequestedBy = new StandardIconButton();
      this.txtRerequestedBy = new TextBox();
      this.dateRerequested = new DateTimePicker();
      this.txtDateRerequested = new TextBox();
      this.chkRerequested = new CheckBox();
      this.btnRequestedBy = new StandardIconButton();
      this.txtRequestedBy = new TextBox();
      this.dateRequested = new DateTimePicker();
      this.txtDateRequested = new TextBox();
      this.chkRequested = new CheckBox();
      this.txtRequestedFrom = new TextBox();
      this.lblRequestedFrom = new Label();
      this.txtDateDue = new TextBox();
      this.txtDaysDue = new TextBox();
      this.lblDaysDue = new Label();
      this.btnFulfilledBy = new StandardIconButton();
      this.txtFulfilledBy = new TextBox();
      this.dateFulfilled = new DateTimePicker();
      this.txtDateFulfilled = new TextBox();
      this.chkFulfilled = new CheckBox();
      this.txtAddedBy = new TextBox();
      this.txtAddedDate = new TextBox();
      this.chkAdded = new CheckBox();
      this.pageComments = new TabPage();
      this.commentCollection = new CommentCollectionControl();
      this.csDetails = new CollapsibleSplitter();
      this.gcDetails = new GroupContainer();
      this.pnlDetails = new Panel();
      this.chkUnderwriter = new CheckBox();
      this.cboPriorTo = new ComboBox();
      this.txtPriorTo = new TextBox();
      this.lblPriorTo = new Label();
      this.cboCategory = new ComboBox();
      this.txtCategory = new TextBox();
      this.lblCategory = new Label();
      this.txtSource = new TextBox();
      this.lblSource = new Label();
      this.cboBorrower = new ComboBox();
      this.txtBorrower = new TextBox();
      this.lblBorrower = new Label();
      this.lnkViewMore = new System.Windows.Forms.LinkLabel();
      this.txtDescription = new TextBox();
      this.lblDescription = new Label();
      this.cboTitle = new ComboBox();
      this.txtTitle = new TextBox();
      this.lblTitle = new Label();
      this.gcDocuments = new GroupContainer();
      this.pnlToolbar = new FlowLayoutPanel();
      this.btnRequestDocument = new Button();
      this.separator = new VerticalSeparator();
      this.btnRemoveDocument = new StandardIconButton();
      this.btnEditDocument = new StandardIconButton();
      this.btnAddDocument = new StandardIconButton();
      this.gvDocuments = new GridView();
      this.tooltip = new ToolTip(this.components);
      this.pnlRight = new Panel();
      this.pnlViewer = new BorderPanel();
      this.fileViewer = new FileAttachmentViewerControl();
      this.csFiles = new CollapsibleSplitter();
      this.pnlClose = new Panel();
      this.helpLink = new EMHelpLink();
      this.btnClose = new Button();
      this.csLeft = new CollapsibleSplitter();
      this.pnlLeft.SuspendLayout();
      this.gcTracking.SuspendLayout();
      this.tabTracking.SuspendLayout();
      this.pageStatus.SuspendLayout();
      ((ISupportInitialize) this.btnReceivedBy).BeginInit();
      ((ISupportInitialize) this.btnRerequestedBy).BeginInit();
      ((ISupportInitialize) this.btnRequestedBy).BeginInit();
      ((ISupportInitialize) this.btnFulfilledBy).BeginInit();
      this.pageComments.SuspendLayout();
      this.gcDetails.SuspendLayout();
      this.pnlDetails.SuspendLayout();
      this.gcDocuments.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      ((ISupportInitialize) this.btnRemoveDocument).BeginInit();
      ((ISupportInitialize) this.btnEditDocument).BeginInit();
      ((ISupportInitialize) this.btnAddDocument).BeginInit();
      this.pnlRight.SuspendLayout();
      this.pnlViewer.SuspendLayout();
      this.pnlClose.SuspendLayout();
      this.SuspendLayout();
      this.pnlLeft.Controls.Add((Control) this.gcTracking);
      this.pnlLeft.Controls.Add((Control) this.csDetails);
      this.pnlLeft.Controls.Add((Control) this.gcDetails);
      this.pnlLeft.Dock = DockStyle.Left;
      this.pnlLeft.Location = new Point(0, 0);
      this.pnlLeft.Name = "pnlLeft";
      this.pnlLeft.Size = new Size(332, 553);
      this.pnlLeft.TabIndex = 0;
      this.gcTracking.Controls.Add((Control) this.tabTracking);
      this.gcTracking.Dock = DockStyle.Fill;
      this.gcTracking.HeaderForeColor = SystemColors.ControlText;
      this.gcTracking.Location = new Point(0, 291);
      this.gcTracking.Name = "gcTracking";
      this.gcTracking.Padding = new Padding(3, 3, 2, 2);
      this.gcTracking.Size = new Size(332, 262);
      this.gcTracking.TabIndex = 22;
      this.gcTracking.Text = "Tracking";
      this.tabTracking.Controls.Add((Control) this.pageStatus);
      this.tabTracking.Controls.Add((Control) this.pageComments);
      this.tabTracking.Dock = DockStyle.Fill;
      this.tabTracking.Location = new Point(4, 29);
      this.tabTracking.Name = "tabTracking";
      this.tabTracking.SelectedIndex = 0;
      this.tabTracking.Size = new Size(325, 230);
      this.tabTracking.TabIndex = 23;
      this.pageStatus.AutoScroll = true;
      this.pageStatus.AutoScrollMargin = new Size(8, 8);
      this.pageStatus.BackColor = Color.WhiteSmoke;
      this.pageStatus.Controls.Add((Control) this.btnReceivedBy);
      this.pageStatus.Controls.Add((Control) this.txtReceivedBy);
      this.pageStatus.Controls.Add((Control) this.dateReceived);
      this.pageStatus.Controls.Add((Control) this.txtDateReceived);
      this.pageStatus.Controls.Add((Control) this.chkReceived);
      this.pageStatus.Controls.Add((Control) this.btnRerequestedBy);
      this.pageStatus.Controls.Add((Control) this.txtRerequestedBy);
      this.pageStatus.Controls.Add((Control) this.dateRerequested);
      this.pageStatus.Controls.Add((Control) this.txtDateRerequested);
      this.pageStatus.Controls.Add((Control) this.chkRerequested);
      this.pageStatus.Controls.Add((Control) this.btnRequestedBy);
      this.pageStatus.Controls.Add((Control) this.txtRequestedBy);
      this.pageStatus.Controls.Add((Control) this.dateRequested);
      this.pageStatus.Controls.Add((Control) this.txtDateRequested);
      this.pageStatus.Controls.Add((Control) this.chkRequested);
      this.pageStatus.Controls.Add((Control) this.txtRequestedFrom);
      this.pageStatus.Controls.Add((Control) this.lblRequestedFrom);
      this.pageStatus.Controls.Add((Control) this.txtDateDue);
      this.pageStatus.Controls.Add((Control) this.txtDaysDue);
      this.pageStatus.Controls.Add((Control) this.lblDaysDue);
      this.pageStatus.Controls.Add((Control) this.btnFulfilledBy);
      this.pageStatus.Controls.Add((Control) this.txtFulfilledBy);
      this.pageStatus.Controls.Add((Control) this.dateFulfilled);
      this.pageStatus.Controls.Add((Control) this.txtDateFulfilled);
      this.pageStatus.Controls.Add((Control) this.chkFulfilled);
      this.pageStatus.Controls.Add((Control) this.txtAddedBy);
      this.pageStatus.Controls.Add((Control) this.txtAddedDate);
      this.pageStatus.Controls.Add((Control) this.chkAdded);
      this.pageStatus.Location = new Point(4, 23);
      this.pageStatus.Name = "pageStatus";
      this.pageStatus.Padding = new Padding(0, 2, 2, 2);
      this.pageStatus.Size = new Size(317, 203);
      this.pageStatus.TabIndex = 0;
      this.pageStatus.Text = "Status";
      this.pageStatus.UseVisualStyleBackColor = true;
      this.btnReceivedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReceivedBy.BackColor = Color.Transparent;
      this.btnReceivedBy.Location = new Point(295, 158);
      this.btnReceivedBy.MouseDownImage = (Image) null;
      this.btnReceivedBy.Name = "btnReceivedBy";
      this.btnReceivedBy.Size = new Size(16, 16);
      this.btnReceivedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnReceivedBy.TabIndex = 52;
      this.btnReceivedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnReceivedBy, "Select User");
      this.btnReceivedBy.Click += new EventHandler(this.btnReceivedBy_Click);
      this.txtReceivedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtReceivedBy.Location = new Point(237, 155);
      this.txtReceivedBy.Name = "txtReceivedBy";
      this.txtReceivedBy.ReadOnly = true;
      this.txtReceivedBy.Size = new Size(53, 20);
      this.txtReceivedBy.TabIndex = 43;
      this.dateReceived.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateReceived.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateReceived.CalendarTitleForeColor = Color.White;
      this.dateReceived.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateReceived.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateReceived.Format = DateTimePickerFormat.Custom;
      this.dateReceived.Location = new Point(97, 155);
      this.dateReceived.Name = "dateReceived";
      this.dateReceived.Size = new Size(133, 20);
      this.dateReceived.TabIndex = 42;
      this.dateReceived.ValueChanged += new EventHandler(this.dateReceived_ValueChanged);
      this.txtDateReceived.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateReceived.Location = new Point(97, 155);
      this.txtDateReceived.Name = "txtDateReceived";
      this.txtDateReceived.ReadOnly = true;
      this.txtDateReceived.Size = new Size(125, 20);
      this.txtDateReceived.TabIndex = 53;
      this.chkReceived.AutoSize = true;
      this.chkReceived.Location = new Point(5, 158);
      this.chkReceived.Name = "chkReceived";
      this.chkReceived.Size = new Size(71, 18);
      this.chkReceived.TabIndex = 41;
      this.chkReceived.Text = "Received";
      this.chkReceived.UseVisualStyleBackColor = true;
      this.chkReceived.Click += new EventHandler(this.chkReceived_Click);
      this.btnRerequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRerequestedBy.BackColor = Color.Transparent;
      this.btnRerequestedBy.Location = new Point(295, 134);
      this.btnRerequestedBy.MouseDownImage = (Image) null;
      this.btnRerequestedBy.Name = "btnRerequestedBy";
      this.btnRerequestedBy.Size = new Size(16, 16);
      this.btnRerequestedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnRerequestedBy.TabIndex = 47;
      this.btnRerequestedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnRerequestedBy, "Select User");
      this.btnRerequestedBy.Click += new EventHandler(this.btnRerequestedBy_Click);
      this.txtRerequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtRerequestedBy.Location = new Point(237, 131);
      this.txtRerequestedBy.Name = "txtRerequestedBy";
      this.txtRerequestedBy.ReadOnly = true;
      this.txtRerequestedBy.Size = new Size(53, 20);
      this.txtRerequestedBy.TabIndex = 40;
      this.dateRerequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateRerequested.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateRerequested.CalendarTitleForeColor = Color.White;
      this.dateRerequested.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateRerequested.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateRerequested.Format = DateTimePickerFormat.Custom;
      this.dateRerequested.Location = new Point(97, 131);
      this.dateRerequested.Name = "dateRerequested";
      this.dateRerequested.Size = new Size(133, 20);
      this.dateRerequested.TabIndex = 39;
      this.dateRerequested.ValueChanged += new EventHandler(this.dateRerequested_ValueChanged);
      this.txtDateRerequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateRerequested.Location = new Point(97, 131);
      this.txtDateRerequested.Name = "txtDateRerequested";
      this.txtDateRerequested.ReadOnly = true;
      this.txtDateRerequested.Size = new Size(125, 20);
      this.txtDateRerequested.TabIndex = 48;
      this.chkRerequested.AutoSize = true;
      this.chkRerequested.Location = new Point(5, 134);
      this.chkRerequested.Name = "chkRerequested";
      this.chkRerequested.Size = new Size(92, 18);
      this.chkRerequested.TabIndex = 38;
      this.chkRerequested.Text = "Re-requested";
      this.chkRerequested.UseVisualStyleBackColor = true;
      this.chkRerequested.Click += new EventHandler(this.chkRerequested_Click);
      this.btnRequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRequestedBy.BackColor = Color.Transparent;
      this.btnRequestedBy.Location = new Point(295, 109);
      this.btnRequestedBy.MouseDownImage = (Image) null;
      this.btnRequestedBy.Name = "btnRequestedBy";
      this.btnRequestedBy.Size = new Size(16, 16);
      this.btnRequestedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnRequestedBy.TabIndex = 42;
      this.btnRequestedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnRequestedBy, "Select User");
      this.btnRequestedBy.Click += new EventHandler(this.btnRequestedBy_Click);
      this.txtRequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtRequestedBy.Location = new Point(237, 106);
      this.txtRequestedBy.Name = "txtRequestedBy";
      this.txtRequestedBy.ReadOnly = true;
      this.txtRequestedBy.Size = new Size(53, 20);
      this.txtRequestedBy.TabIndex = 37;
      this.dateRequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateRequested.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateRequested.CalendarTitleForeColor = Color.White;
      this.dateRequested.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateRequested.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateRequested.Format = DateTimePickerFormat.Custom;
      this.dateRequested.Location = new Point(97, 106);
      this.dateRequested.Name = "dateRequested";
      this.dateRequested.Size = new Size(133, 20);
      this.dateRequested.TabIndex = 36;
      this.dateRequested.ValueChanged += new EventHandler(this.dateRequested_ValueChanged);
      this.txtDateRequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateRequested.Location = new Point(97, 106);
      this.txtDateRequested.Name = "txtDateRequested";
      this.txtDateRequested.ReadOnly = true;
      this.txtDateRequested.Size = new Size(125, 20);
      this.txtDateRequested.TabIndex = 43;
      this.chkRequested.AutoSize = true;
      this.chkRequested.Location = new Point(5, 109);
      this.chkRequested.Name = "chkRequested";
      this.chkRequested.Size = new Size(78, 18);
      this.chkRequested.TabIndex = 35;
      this.chkRequested.Text = "Requested";
      this.chkRequested.UseVisualStyleBackColor = true;
      this.chkRequested.Click += new EventHandler(this.chkRequested_Click);
      this.txtRequestedFrom.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtRequestedFrom.Location = new Point(97, 34);
      this.txtRequestedFrom.Name = "txtRequestedFrom";
      this.txtRequestedFrom.Size = new Size(195, 20);
      this.txtRequestedFrom.TabIndex = 28;
      this.txtRequestedFrom.Validated += new EventHandler(this.txtRequestedFrom_Validated);
      this.lblRequestedFrom.AutoSize = true;
      this.lblRequestedFrom.Location = new Point(5, 36);
      this.lblRequestedFrom.Name = "lblRequestedFrom";
      this.lblRequestedFrom.Size = new Size(86, 14);
      this.lblRequestedFrom.TabIndex = 27;
      this.lblRequestedFrom.Text = "Requested From";
      this.txtDateDue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateDue.Location = new Point(149, 10);
      this.txtDateDue.Name = "txtDateDue";
      this.txtDateDue.ReadOnly = true;
      this.txtDateDue.Size = new Size(143, 20);
      this.txtDateDue.TabIndex = 26;
      this.txtDaysDue.Location = new Point(97, 10);
      this.txtDaysDue.Name = "txtDaysDue";
      this.txtDaysDue.Size = new Size(48, 20);
      this.txtDaysDue.TabIndex = 25;
      this.txtDaysDue.TextAlign = HorizontalAlignment.Right;
      this.txtDaysDue.Validating += new CancelEventHandler(this.txtDaysDue_Validating);
      this.txtDaysDue.Validated += new EventHandler(this.txtDaysDue_Validated);
      this.lblDaysDue.AutoSize = true;
      this.lblDaysDue.Location = new Point(5, 13);
      this.lblDaysDue.Name = "lblDaysDue";
      this.lblDaysDue.Size = new Size(86, 14);
      this.lblDaysDue.TabIndex = 24;
      this.lblDaysDue.Text = "Days to Receive";
      this.btnFulfilledBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnFulfilledBy.BackColor = Color.Transparent;
      this.btnFulfilledBy.Location = new Point(296, 84);
      this.btnFulfilledBy.MouseDownImage = (Image) null;
      this.btnFulfilledBy.Name = "btnFulfilledBy";
      this.btnFulfilledBy.Size = new Size(16, 16);
      this.btnFulfilledBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnFulfilledBy.TabIndex = 22;
      this.btnFulfilledBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnFulfilledBy, "Select User");
      this.btnFulfilledBy.Click += new EventHandler(this.btnFulfilledBy_Click);
      this.txtFulfilledBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtFulfilledBy.Location = new Point(238, 81);
      this.txtFulfilledBy.Name = "txtFulfilledBy";
      this.txtFulfilledBy.ReadOnly = true;
      this.txtFulfilledBy.Size = new Size(53, 20);
      this.txtFulfilledBy.TabIndex = 34;
      this.dateFulfilled.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateFulfilled.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateFulfilled.CalendarTitleForeColor = Color.White;
      this.dateFulfilled.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateFulfilled.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateFulfilled.Format = DateTimePickerFormat.Custom;
      this.dateFulfilled.Location = new Point(96, 82);
      this.dateFulfilled.Name = "dateFulfilled";
      this.dateFulfilled.Size = new Size(135, 20);
      this.dateFulfilled.TabIndex = 33;
      this.dateFulfilled.Visible = false;
      this.dateFulfilled.ValueChanged += new EventHandler(this.dateFulfilled_ValueChanged);
      this.txtDateFulfilled.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateFulfilled.Location = new Point(96, 82);
      this.txtDateFulfilled.Name = "txtDateFulfilled";
      this.txtDateFulfilled.ReadOnly = true;
      this.txtDateFulfilled.Size = new Size(124, 20);
      this.txtDateFulfilled.TabIndex = 29;
      this.chkFulfilled.AutoSize = true;
      this.chkFulfilled.Location = new Point(6, 84);
      this.chkFulfilled.Name = "chkFulfilled";
      this.chkFulfilled.Size = new Size(62, 18);
      this.chkFulfilled.TabIndex = 32;
      this.chkFulfilled.Text = "Fulfilled";
      this.chkFulfilled.UseVisualStyleBackColor = true;
      this.chkFulfilled.Click += new EventHandler(this.chkFulfilled_Click);
      this.txtAddedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtAddedBy.Location = new Point(238, 58);
      this.txtAddedBy.Name = "txtAddedBy";
      this.txtAddedBy.ReadOnly = true;
      this.txtAddedBy.Size = new Size(54, 20);
      this.txtAddedBy.TabIndex = 31;
      this.txtAddedDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtAddedDate.Location = new Point(97, 58);
      this.txtAddedDate.Name = "txtAddedDate";
      this.txtAddedDate.ReadOnly = true;
      this.txtAddedDate.Size = new Size(135, 20);
      this.txtAddedDate.TabIndex = 30;
      this.chkAdded.AutoSize = true;
      this.chkAdded.Checked = true;
      this.chkAdded.CheckState = CheckState.Checked;
      this.chkAdded.Enabled = false;
      this.chkAdded.Location = new Point(8, 61);
      this.chkAdded.Name = "chkAdded";
      this.chkAdded.Size = new Size(58, 18);
      this.chkAdded.TabIndex = 29;
      this.chkAdded.Text = "Added";
      this.chkAdded.UseVisualStyleBackColor = true;
      this.pageComments.BackColor = Color.WhiteSmoke;
      this.pageComments.Controls.Add((Control) this.commentCollection);
      this.pageComments.Location = new Point(4, 23);
      this.pageComments.Name = "pageComments";
      this.pageComments.Padding = new Padding(0, 2, 2, 2);
      this.pageComments.Size = new Size(317, 207);
      this.pageComments.TabIndex = 1;
      this.pageComments.Text = "Comments";
      this.pageComments.UseVisualStyleBackColor = true;
      this.commentCollection.CanAddComment = false;
      this.commentCollection.CanDeleteComment = false;
      this.commentCollection.Dock = DockStyle.Fill;
      this.commentCollection.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.commentCollection.Location = new Point(0, 2);
      this.commentCollection.Name = "commentCollection";
      this.commentCollection.Size = new Size(315, 203);
      this.commentCollection.TabIndex = 31;
      this.csDetails.AnimationDelay = 20;
      this.csDetails.AnimationStep = 20;
      this.csDetails.BorderStyle3D = Border3DStyle.Flat;
      this.csDetails.ControlToHide = (Control) this.gcDetails;
      this.csDetails.Dock = DockStyle.Top;
      this.csDetails.ExpandParentForm = false;
      this.csDetails.Location = new Point(0, 284);
      this.csDetails.Name = "csFiles";
      this.csDetails.TabIndex = 21;
      this.csDetails.TabStop = false;
      this.csDetails.UseAnimations = false;
      this.csDetails.VisualStyle = VisualStyles.Encompass;
      this.gcDetails.Controls.Add((Control) this.pnlDetails);
      this.gcDetails.Dock = DockStyle.Top;
      this.gcDetails.HeaderForeColor = SystemColors.ControlText;
      this.gcDetails.Location = new Point(0, 0);
      this.gcDetails.Name = "gcDetails";
      this.gcDetails.Size = new Size(332, 284);
      this.gcDetails.TabIndex = 1;
      this.gcDetails.Text = "Details";
      this.pnlDetails.AutoScroll = true;
      this.pnlDetails.AutoScrollMargin = new Size(8, 8);
      this.pnlDetails.Controls.Add((Control) this.chkUnderwriter);
      this.pnlDetails.Controls.Add((Control) this.cboPriorTo);
      this.pnlDetails.Controls.Add((Control) this.txtPriorTo);
      this.pnlDetails.Controls.Add((Control) this.lblPriorTo);
      this.pnlDetails.Controls.Add((Control) this.cboCategory);
      this.pnlDetails.Controls.Add((Control) this.txtCategory);
      this.pnlDetails.Controls.Add((Control) this.lblCategory);
      this.pnlDetails.Controls.Add((Control) this.txtSource);
      this.pnlDetails.Controls.Add((Control) this.lblSource);
      this.pnlDetails.Controls.Add((Control) this.cboBorrower);
      this.pnlDetails.Controls.Add((Control) this.txtBorrower);
      this.pnlDetails.Controls.Add((Control) this.lblBorrower);
      this.pnlDetails.Controls.Add((Control) this.lnkViewMore);
      this.pnlDetails.Controls.Add((Control) this.txtDescription);
      this.pnlDetails.Controls.Add((Control) this.lblDescription);
      this.pnlDetails.Controls.Add((Control) this.cboTitle);
      this.pnlDetails.Controls.Add((Control) this.txtTitle);
      this.pnlDetails.Controls.Add((Control) this.lblTitle);
      this.pnlDetails.Dock = DockStyle.Fill;
      this.pnlDetails.Location = new Point(1, 26);
      this.pnlDetails.Name = "pnlDetails";
      this.pnlDetails.Size = new Size(330, 257);
      this.pnlDetails.TabIndex = 2;
      this.chkUnderwriter.AutoSize = true;
      this.chkUnderwriter.Location = new Point(8, 228);
      this.chkUnderwriter.Name = "chkUnderwriter";
      this.chkUnderwriter.Size = new Size(169, 18);
      this.chkUnderwriter.TabIndex = 20;
      this.chkUnderwriter.Text = "UW can access this condition";
      this.chkUnderwriter.UseVisualStyleBackColor = true;
      this.chkUnderwriter.Click += new EventHandler(this.chkUnderwriter_Click);
      this.cboPriorTo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
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
      this.cboPriorTo.Location = new Point(88, 196);
      this.cboPriorTo.Name = "cboPriorTo";
      this.cboPriorTo.Size = new Size(232, 22);
      this.cboPriorTo.TabIndex = 18;
      this.cboPriorTo.SelectionChangeCommitted += new EventHandler(this.cboPriorTo_SelectionChangeCommitted);
      this.txtPriorTo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtPriorTo.Location = new Point(88, 196);
      this.txtPriorTo.Name = "txtPriorTo";
      this.txtPriorTo.ReadOnly = true;
      this.txtPriorTo.Size = new Size(232, 20);
      this.txtPriorTo.TabIndex = 19;
      this.lblPriorTo.AutoSize = true;
      this.lblPriorTo.Location = new Point(8, 200);
      this.lblPriorTo.Name = "lblPriorTo";
      this.lblPriorTo.Size = new Size(43, 14);
      this.lblPriorTo.TabIndex = 17;
      this.lblPriorTo.Text = "Prior To";
      this.cboCategory.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
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
      this.cboCategory.Location = new Point(88, 168);
      this.cboCategory.Name = "cboCategory";
      this.cboCategory.Size = new Size(232, 22);
      this.cboCategory.TabIndex = 15;
      this.cboCategory.SelectionChangeCommitted += new EventHandler(this.cboCategory_SelectionChangeCommitted);
      this.txtCategory.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtCategory.Location = new Point(88, 168);
      this.txtCategory.Name = "txtCategory";
      this.txtCategory.ReadOnly = true;
      this.txtCategory.Size = new Size(232, 20);
      this.txtCategory.TabIndex = 16;
      this.lblCategory.AutoSize = true;
      this.lblCategory.Location = new Point(8, 172);
      this.lblCategory.Name = "lblCategory";
      this.lblCategory.Size = new Size(51, 14);
      this.lblCategory.TabIndex = 14;
      this.lblCategory.Text = "Category";
      this.txtSource.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSource.Location = new Point(88, 140);
      this.txtSource.Name = "txtSource";
      this.txtSource.ReadOnly = true;
      this.txtSource.Size = new Size(232, 20);
      this.txtSource.TabIndex = 13;
      this.lblSource.AutoSize = true;
      this.lblSource.Location = new Point(8, 144);
      this.lblSource.Name = "lblSource";
      this.lblSource.Size = new Size(42, 14);
      this.lblSource.TabIndex = 12;
      this.lblSource.Text = "Source";
      this.cboBorrower.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboBorrower.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboBorrower.FormattingEnabled = true;
      this.cboBorrower.Location = new Point(88, 112);
      this.cboBorrower.Name = "cboBorrower";
      this.cboBorrower.Size = new Size(232, 22);
      this.cboBorrower.TabIndex = 10;
      this.cboBorrower.SelectionChangeCommitted += new EventHandler(this.cboBorrower_SelectionChangeCommitted);
      this.txtBorrower.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBorrower.Location = new Point(88, 112);
      this.txtBorrower.Name = "txtBorrower";
      this.txtBorrower.ReadOnly = true;
      this.txtBorrower.Size = new Size(232, 20);
      this.txtBorrower.TabIndex = 11;
      this.lblBorrower.AutoSize = true;
      this.lblBorrower.Location = new Point(8, 110);
      this.lblBorrower.MaximumSize = new Size(75, 0);
      this.lblBorrower.Name = "lblBorrower";
      this.lblBorrower.Size = new Size(73, 28);
      this.lblBorrower.TabIndex = 9;
      this.lblBorrower.Text = "For Borrower Pair";
      this.lnkViewMore.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lnkViewMore.AutoSize = true;
      this.lnkViewMore.Location = new Point(252, 92);
      this.lnkViewMore.Name = "lnkViewMore";
      this.lnkViewMore.Size = new Size(69, 14);
      this.lnkViewMore.TabIndex = 8;
      this.lnkViewMore.TabStop = true;
      this.lnkViewMore.Text = "View More >";
      this.lnkViewMore.TextAlign = ContentAlignment.MiddleRight;
      this.lnkViewMore.Click += new EventHandler(this.lnkViewMore_Click);
      this.txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDescription.Location = new Point(88, 36);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ScrollBars = ScrollBars.Vertical;
      this.txtDescription.Size = new Size(232, 52);
      this.txtDescription.TabIndex = 7;
      this.txtDescription.Validated += new EventHandler(this.txtDescription_Validated);
      this.lblDescription.AutoSize = true;
      this.lblDescription.Location = new Point(8, 40);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(61, 14);
      this.lblDescription.TabIndex = 6;
      this.lblDescription.Text = "Description";
      this.cboTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboTitle.FormattingEnabled = true;
      this.cboTitle.Location = new Point(88, 8);
      this.cboTitle.Name = "cboTitle";
      this.cboTitle.Size = new Size(232, 22);
      this.cboTitle.Sorted = true;
      this.cboTitle.TabIndex = 4;
      this.cboTitle.SelectionChangeCommitted += new EventHandler(this.cboTitle_SelectionChangeCommitted);
      this.cboTitle.Validating += new CancelEventHandler(this.cboTitle_Validating);
      this.cboTitle.Validated += new EventHandler(this.cboTitle_Validated);
      this.txtTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtTitle.Location = new Point(88, 8);
      this.txtTitle.Name = "txtTitle";
      this.txtTitle.ReadOnly = true;
      this.txtTitle.Size = new Size(232, 20);
      this.txtTitle.TabIndex = 5;
      this.lblTitle.AutoSize = true;
      this.lblTitle.Location = new Point(8, 12);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(34, 14);
      this.lblTitle.TabIndex = 3;
      this.lblTitle.Text = "Name";
      this.gcDocuments.Controls.Add((Control) this.pnlToolbar);
      this.gcDocuments.Controls.Add((Control) this.gvDocuments);
      this.gcDocuments.Dock = DockStyle.Top;
      this.gcDocuments.HeaderForeColor = SystemColors.ControlText;
      this.gcDocuments.Location = new Point(0, 0);
      this.gcDocuments.Name = "gcDocuments";
      this.gcDocuments.Size = new Size(435, 120);
      this.gcDocuments.TabIndex = 34;
      this.gcDocuments.Text = "Supporting Documents";
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnRequestDocument);
      this.pnlToolbar.Controls.Add((Control) this.separator);
      this.pnlToolbar.Controls.Add((Control) this.btnRemoveDocument);
      this.pnlToolbar.Controls.Add((Control) this.btnEditDocument);
      this.pnlToolbar.Controls.Add((Control) this.btnAddDocument);
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(271, 2);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(160, 22);
      this.pnlToolbar.TabIndex = 35;
      this.btnRequestDocument.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRequestDocument.Location = new Point(96, 0);
      this.btnRequestDocument.Margin = new Padding(0);
      this.btnRequestDocument.Name = "btnRequestDocument";
      this.btnRequestDocument.Size = new Size(64, 22);
      this.btnRequestDocument.TabIndex = 37;
      this.btnRequestDocument.TabStop = false;
      this.btnRequestDocument.Text = "Request";
      this.tooltip.SetToolTip((Control) this.btnRequestDocument, "Send documents to borrower to sign and request needed documents");
      this.btnRequestDocument.UseVisualStyleBackColor = true;
      this.btnRequestDocument.Click += new EventHandler(this.btnRequestDocument_Click);
      this.separator.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.separator.Location = new Point(91, 3);
      this.separator.Margin = new Padding(4, 3, 3, 3);
      this.separator.MaximumSize = new Size(2, 16);
      this.separator.MinimumSize = new Size(2, 16);
      this.separator.Name = "separator";
      this.separator.Size = new Size(2, 16);
      this.separator.TabIndex = 36;
      this.separator.TabStop = false;
      this.btnRemoveDocument.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRemoveDocument.BackColor = Color.Transparent;
      this.btnRemoveDocument.Location = new Point(71, 3);
      this.btnRemoveDocument.Margin = new Padding(4, 3, 0, 3);
      this.btnRemoveDocument.MouseDownImage = (Image) null;
      this.btnRemoveDocument.Name = "btnRemoveDocument";
      this.btnRemoveDocument.Size = new Size(16, 16);
      this.btnRemoveDocument.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemoveDocument.TabIndex = 52;
      this.btnRemoveDocument.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnRemoveDocument, "Remove Document");
      this.btnRemoveDocument.Click += new EventHandler(this.btnRemoveDocument_Click);
      this.btnEditDocument.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEditDocument.BackColor = Color.Transparent;
      this.btnEditDocument.Location = new Point(51, 3);
      this.btnEditDocument.Margin = new Padding(4, 3, 0, 3);
      this.btnEditDocument.MouseDownImage = (Image) null;
      this.btnEditDocument.Name = "btnEditDocument";
      this.btnEditDocument.Size = new Size(16, 16);
      this.btnEditDocument.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditDocument.TabIndex = 51;
      this.btnEditDocument.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnEditDocument, "Edit Document");
      this.btnEditDocument.Click += new EventHandler(this.btnEditDocument_Click);
      this.btnAddDocument.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddDocument.BackColor = Color.Transparent;
      this.btnAddDocument.Location = new Point(31, 3);
      this.btnAddDocument.Margin = new Padding(4, 3, 0, 3);
      this.btnAddDocument.MouseDownImage = (Image) null;
      this.btnAddDocument.Name = "btnAddDocument";
      this.btnAddDocument.Size = new Size(16, 16);
      this.btnAddDocument.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddDocument.TabIndex = 54;
      this.btnAddDocument.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddDocument, "Assign Document");
      this.btnAddDocument.Click += new EventHandler(this.btnAddDocument_Click);
      this.gvDocuments.BorderStyle = BorderStyle.None;
      this.gvDocuments.ClearSelectionsOnEmptyRowClick = false;
      this.gvDocuments.Dock = DockStyle.Fill;
      this.gvDocuments.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDocuments.Location = new Point(1, 26);
      this.gvDocuments.Name = "gvDocuments";
      this.gvDocuments.Size = new Size(433, 93);
      this.gvDocuments.TabIndex = 38;
      this.gvDocuments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvDocuments.BeforeSelectedIndexCommitted += new CancelEventHandler(this.gvDocuments_BeforeSelectedIndexCommitted);
      this.gvDocuments.SelectedIndexCommitted += new EventHandler(this.gvDocuments_SelectedIndexCommitted);
      this.gvDocuments.ItemDoubleClick += new GVItemEventHandler(this.gvDocuments_ItemDoubleClick);
      this.pnlRight.Controls.Add((Control) this.pnlViewer);
      this.pnlRight.Controls.Add((Control) this.csFiles);
      this.pnlRight.Controls.Add((Control) this.gcDocuments);
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(339, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(435, 553);
      this.pnlRight.TabIndex = 33;
      this.pnlViewer.Controls.Add((Control) this.fileViewer);
      this.pnlViewer.Dock = DockStyle.Fill;
      this.pnlViewer.Location = new Point(0, (int) sbyte.MaxValue);
      this.pnlViewer.Name = "pnlViewer";
      this.pnlViewer.Size = new Size(435, 426);
      this.pnlViewer.TabIndex = 40;
      this.fileViewer.Dock = DockStyle.Fill;
      this.fileViewer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.fileViewer.Location = new Point(1, 1);
      this.fileViewer.Name = "fileViewer";
      this.fileViewer.Size = new Size(433, 424);
      this.fileViewer.TabIndex = 41;
      this.csFiles.AnimationDelay = 20;
      this.csFiles.AnimationStep = 20;
      this.csFiles.BorderStyle3D = Border3DStyle.Flat;
      this.csFiles.ControlToHide = (Control) this.gcDocuments;
      this.csFiles.Dock = DockStyle.Top;
      this.csFiles.ExpandParentForm = false;
      this.csFiles.Location = new Point(0, 120);
      this.csFiles.Name = "csFiles";
      this.csFiles.TabIndex = 39;
      this.csFiles.TabStop = false;
      this.csFiles.UseAnimations = false;
      this.csFiles.VisualStyle = VisualStyles.Encompass;
      this.pnlClose.Controls.Add((Control) this.helpLink);
      this.pnlClose.Controls.Add((Control) this.btnClose);
      this.pnlClose.Dock = DockStyle.Bottom;
      this.pnlClose.Location = new Point(0, 553);
      this.pnlClose.Name = "pnlClose";
      this.pnlClose.Size = new Size(774, 40);
      this.pnlClose.TabIndex = 42;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "Condition Details";
      this.helpLink.Location = new Point(8, 11);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 18);
      this.helpLink.TabIndex = 44;
      this.helpLink.TabStop = false;
      this.btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.OK;
      this.btnClose.Location = new Point(691, 9);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 22);
      this.btnClose.TabIndex = 43;
      this.btnClose.TabStop = false;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.csLeft.AnimationDelay = 20;
      this.csLeft.AnimationStep = 20;
      this.csLeft.BorderStyle3D = Border3DStyle.Flat;
      this.csLeft.ControlToHide = (Control) this.pnlLeft;
      this.csLeft.ExpandParentForm = false;
      this.csLeft.Location = new Point(332, 0);
      this.csLeft.Name = "csDocTracking";
      this.csLeft.TabIndex = 32;
      this.csLeft.TabStop = false;
      this.csLeft.UseAnimations = false;
      this.csLeft.VisualStyle = VisualStyles.Encompass;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(774, 593);
      this.Controls.Add((Control) this.pnlRight);
      this.Controls.Add((Control) this.csLeft);
      this.Controls.Add((Control) this.pnlLeft);
      this.Controls.Add((Control) this.pnlClose);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = Resources.icon_allsizes_bug;
      this.KeyPreview = true;
      this.Name = nameof (PreliminaryDetailsDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Preliminary Condition Details";
      this.Activated += new EventHandler(this.PreliminaryDetailsDialog_Activated);
      this.FormClosing += new FormClosingEventHandler(this.PreliminaryDetailsDialog_FormClosing);
      this.KeyDown += new KeyEventHandler(this.PreliminaryDetailsDialog_KeyDown);
      this.pnlLeft.ResumeLayout(false);
      this.gcTracking.ResumeLayout(false);
      this.tabTracking.ResumeLayout(false);
      this.pageStatus.ResumeLayout(false);
      this.pageStatus.PerformLayout();
      ((ISupportInitialize) this.btnReceivedBy).EndInit();
      ((ISupportInitialize) this.btnRerequestedBy).EndInit();
      ((ISupportInitialize) this.btnRequestedBy).EndInit();
      ((ISupportInitialize) this.btnFulfilledBy).EndInit();
      this.pageComments.ResumeLayout(false);
      this.gcDetails.ResumeLayout(false);
      this.pnlDetails.ResumeLayout(false);
      this.pnlDetails.PerformLayout();
      this.gcDocuments.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemoveDocument).EndInit();
      ((ISupportInitialize) this.btnEditDocument).EndInit();
      ((ISupportInitialize) this.btnAddDocument).EndInit();
      this.pnlRight.ResumeLayout(false);
      this.pnlViewer.ResumeLayout(false);
      this.pnlClose.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
