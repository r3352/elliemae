// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.PostClosingDetailsDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
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
  public class PostClosingDetailsDialog : Form
  {
    private const string className = "PostClosingDetailsDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private static List<PostClosingDetailsDialog> _instanceList = new List<PostClosingDetailsDialog>();
    private LoanDataMgr loanDataMgr;
    private PostClosingConditionLog cond;
    private PostClosingConditionTrackingSetup condSetup;
    private GridViewDataManager gvDocumentsMgr;
    private eFolderAccessRights rights;
    private bool refreshDocuments;
    private IContainer components;
    private Panel pnlDetails;
    private Label lblSource;
    private ComboBox cboBorrower;
    private TextBox txtDescription;
    private Label lblTitle;
    private ComboBox cboTitle;
    private Label lblDescription;
    private Label lblBorrower;
    private StandardIconButton btnAddDocument;
    private GroupContainer gcTracking;
    private TabControl tabTracking;
    private TabPage pageStatus;
    private StandardIconButton btnClearedBy;
    private TextBox txtClearedBy;
    private DateTimePicker dateCleared;
    private CheckBox chkCleared;
    private StandardIconButton btnSentBy;
    private TextBox txtSentBy;
    private DateTimePicker dateSent;
    private CheckBox chkSent;
    private StandardIconButton btnReceivedBy;
    private TextBox txtReceivedBy;
    private DateTimePicker dateReceived;
    private CheckBox chkReceived;
    private StandardIconButton btnRerequestedBy;
    private TextBox txtRerequestedBy;
    private DateTimePicker dateRerequested;
    private CheckBox chkRerequested;
    private TextBox txtAddedDate;
    private StandardIconButton btnRequestedBy;
    private TextBox txtRequestedBy;
    private TextBox txtAddedBy;
    private DateTimePicker dateRequested;
    private CheckBox chkRequested;
    private CheckBox chkAdded;
    private TabPage pageComments;
    private StandardIconButton btnRemoveDocument;
    private StandardIconButton btnEditDocument;
    private Panel pnlLeft;
    private CollapsibleSplitter csDetails;
    private GroupContainer gcDetails;
    private GridView gvDocuments;
    private CollapsibleSplitter csLeft;
    private Panel pnlClose;
    private Button btnClose;
    private Button btnRequestDocument;
    private VerticalSeparator separator;
    private FileAttachmentViewerControl fileViewer;
    private Panel pnlRight;
    private BorderPanel pnlViewer;
    private CollapsibleSplitter csDocuments;
    private GroupContainer gcDocuments;
    private ComboBox cboSource;
    private ComboBox cboRecipient;
    private Label lblRecipient;
    private TextBox txtRequestedFrom;
    private Label lblRequestedFrom;
    private TextBox txtDateDue;
    private TextBox txtDaysDue;
    private Label lblDaysDue;
    private CommentCollectionControl commentCollection;
    private TextBox txtTitle;
    private TextBox txtBorrower;
    private TextBox txtSource;
    private TextBox txtRecipient;
    private TextBox txtDateRequested;
    private TextBox txtDateRerequested;
    private TextBox txtDateReceived;
    private TextBox txtDateSent;
    private TextBox txtDateCleared;
    private FlowLayoutPanel pnlToolbar;
    private ToolTip tooltip;
    private EMHelpLink helpLink;
    private Label lblPrint;
    private CheckBox chkExternal;
    private CheckBox chkInternal;

    public static void ShowInstance(LoanDataMgr loanDataMgr, PostClosingConditionLog cond)
    {
      if (Form.ActiveForm != null && Form.ActiveForm.Modal)
      {
        using (PostClosingDetailsDialog closingDetailsDialog = new PostClosingDetailsDialog(loanDataMgr, cond))
        {
          int num = (int) closingDetailsDialog.ShowDialog((IWin32Window) Form.ActiveForm);
        }
      }
      else
      {
        PostClosingDetailsDialog closingDetailsDialog1 = (PostClosingDetailsDialog) null;
        foreach (PostClosingDetailsDialog instance in PostClosingDetailsDialog._instanceList)
        {
          if (instance.Condition == cond)
            closingDetailsDialog1 = instance;
        }
        if (closingDetailsDialog1 == null)
        {
          PostClosingDetailsDialog closingDetailsDialog2 = new PostClosingDetailsDialog(loanDataMgr, cond);
          closingDetailsDialog2.FormClosing += new FormClosingEventHandler(PostClosingDetailsDialog._instance_FormClosing);
          PostClosingDetailsDialog._instanceList.Add(closingDetailsDialog2);
          closingDetailsDialog2.Show();
        }
        else
        {
          if (closingDetailsDialog1.WindowState == FormWindowState.Minimized)
            closingDetailsDialog1.WindowState = FormWindowState.Normal;
          closingDetailsDialog1.Activate();
        }
      }
    }

    public static void CloseInstances()
    {
      if (PostClosingDetailsDialog._instanceList == null && PostClosingDetailsDialog._instanceList.Count == 0)
        return;
      int num = PostClosingDetailsDialog._instanceList.Count - 1;
      try
      {
        for (int index = num; index >= 0; --index)
          PostClosingDetailsDialog._instanceList[index].Close();
      }
      catch
      {
      }
    }

    private static void _instance_FormClosing(object sender, FormClosingEventArgs e)
    {
      PostClosingDetailsDialog closingDetailsDialog = (PostClosingDetailsDialog) sender;
      PostClosingDetailsDialog._instanceList.Remove(closingDetailsDialog);
    }

    private PostClosingDetailsDialog(LoanDataMgr loanDataMgr, PostClosingConditionLog cond)
    {
      this.InitializeComponent();
      this.setWindowSize();
      List<string> stringList = new List<string>((IEnumerable<string>) LoanConstants.PostClosingConditionSources);
      stringList.RemoveRange(stringList.Count - 2, 2);
      this.cboSource.Items.AddRange((object[]) stringList.ToArray());
      this.loanDataMgr = loanDataMgr;
      this.cond = cond;
      this.rights = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) cond);
      this.initEventHandlers();
      this.initTitleField();
      this.initBorrowerField();
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

    public PostClosingConditionLog Condition => this.cond;

    private void loadConditionDetails()
    {
      this.loadTitleField();
      this.loadDescriptionField();
      this.loadBorrowerField();
      this.loadSourceField();
      this.loadRecipientField();
      this.loadPrintFields();
      this.loadExpectedFields();
      this.loadRequestedFromField();
      this.loadAddedFields();
      this.loadRequestedFields();
      this.loadRerequestedFields();
      this.loadReceivedFields();
      this.loadSentFields();
      this.loadClearedFields();
      this.loadCommentList();
    }

    private void loadPrintFields()
    {
      this.chkInternal.Checked = this.cond.IsInternal;
      this.chkExternal.Checked = this.cond.IsExternal;
    }

    private void chkInternal_Click(object sender, EventArgs e)
    {
      this.cond.IsInternal = this.chkInternal.Checked;
    }

    private void chkExternal_Click(object sender, EventArgs e)
    {
      this.cond.IsExternal = this.chkExternal.Checked;
    }

    private void initTitleField()
    {
      this.condSetup = this.loanDataMgr.SystemConfiguration.PostClosingConditionTrackingSetup;
      foreach (ConditionTemplate conditionTemplate in (CollectionBase) this.condSetup)
        this.cboTitle.Items.Add((object) conditionTemplate.Name);
    }

    private void loadTitleField()
    {
      this.cboTitle.Text = this.cond.Title;
      this.txtTitle.Text = this.cond.Title;
      this.Text = "Post-Closing Condition Details (" + this.cond.Title + ")";
    }

    private void cboTitle_SelectionChangeCommitted(object sender, EventArgs e)
    {
      string selectedItem = (string) this.cboTitle.SelectedItem;
      this.Text = "Post-Closing Condition Details (" + selectedItem + ")";
      PostClosingConditionTemplate byName = this.condSetup.GetByName(selectedItem);
      if (byName == null)
        return;
      this.cond.Title = selectedItem;
      this.cond.Description = byName.Description;
      this.cond.Source = byName.Source;
      this.cond.Recipient = byName.Recipient;
      this.cond.DaysTillDue = byName.DaysTillDue;
      this.cond.IsInternal = byName.IsInternal;
      this.cond.IsExternal = byName.IsExternal;
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

    private void loadDescriptionField() => this.txtDescription.Text = this.cond.Description;

    private void txtDescription_Validated(object sender, EventArgs e)
    {
      if (!(this.cond.Description != this.txtDescription.Text))
        return;
      this.cond.Description = this.txtDescription.Text;
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

    private void loadSourceField()
    {
      this.cboSource.SelectedItem = !this.cboSource.Items.Contains((object) this.cond.Source) ? (object) null : (object) this.cond.Source;
      this.txtSource.Text = this.cboSource.Text;
    }

    private void cboSource_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.cond.Source = (string) this.cboSource.SelectedItem;
    }

    private void loadRecipientField()
    {
      this.cboRecipient.SelectedItem = !this.cboRecipient.Items.Contains((object) this.cond.Recipient) ? (object) null : (object) this.cond.Recipient;
      this.txtRecipient.Text = this.cboRecipient.Text;
    }

    private void cboRecipient_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.cond.Recipient = (string) this.cboRecipient.SelectedItem;
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
      this.dateRequested.Visible = this.chkRequested.Checked && this.rights.CanEditPostClosingCondition;
      this.txtDateRequested.Visible = this.chkRequested.Checked && !this.rights.CanEditPostClosingCondition;
      this.txtRequestedBy.Visible = this.chkRequested.Checked;
      this.btnRequestedBy.Visible = this.chkRequested.Checked && this.rights.CanEditPostClosingCondition;
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
      this.dateRerequested.Visible = this.chkRerequested.Checked && this.rights.CanEditPostClosingCondition;
      this.txtDateRerequested.Visible = this.chkRerequested.Checked && !this.rights.CanEditPostClosingCondition;
      this.txtRerequestedBy.Visible = this.chkRerequested.Checked;
      this.btnRerequestedBy.Visible = this.chkRerequested.Checked && this.rights.CanEditPostClosingCondition;
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
      this.dateReceived.Visible = this.chkReceived.Checked && this.rights.CanEditPostClosingCondition;
      this.txtDateReceived.Visible = this.chkReceived.Checked && !this.rights.CanEditPostClosingCondition;
      this.txtReceivedBy.Visible = this.chkReceived.Checked;
      this.btnReceivedBy.Visible = this.chkReceived.Checked && this.rights.CanEditPostClosingCondition;
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

    private void loadSentFields()
    {
      this.chkSent.Checked = this.cond.Sent;
      if (this.chkSent.Checked)
      {
        this.dateSent.Value = this.cond.DateSent;
        this.txtDateSent.Text = this.cond.DateSent.ToString(this.dateSent.CustomFormat);
        this.txtSentBy.Text = this.cond.SentBy;
      }
      this.dateSent.Visible = this.chkSent.Checked && this.rights.CanEditPostClosingCondition;
      this.txtDateSent.Visible = this.chkSent.Checked && !this.rights.CanEditPostClosingCondition;
      this.txtSentBy.Visible = this.chkSent.Checked;
      this.btnSentBy.Visible = this.chkSent.Checked && this.rights.CanEditPostClosingCondition;
    }

    private void chkSent_Click(object sender, EventArgs e)
    {
      if (this.chkSent.Checked)
        this.cond.MarkAsSent(DateTime.Now, Session.UserID);
      else
        this.cond.UnmarkAsSent();
    }

    private void dateSent_ValueChanged(object sender, EventArgs e)
    {
      if (!(this.dateSent.Value != this.cond.DateSent))
        return;
      this.cond.MarkAsSent(this.dateSent.Value, this.txtSentBy.Text);
    }

    private void btnSentBy_Click(object sender, EventArgs e)
    {
      string user = this.selectUser();
      if (user == null)
        return;
      this.cond.MarkAsSent(this.cond.DateSent, user);
    }

    private void loadClearedFields()
    {
      this.chkCleared.Checked = this.cond.Cleared;
      if (this.chkCleared.Checked)
      {
        this.dateCleared.Value = this.cond.DateCleared;
        this.txtDateCleared.Text = this.cond.DateCleared.ToString(this.dateCleared.CustomFormat);
        this.txtClearedBy.Text = this.cond.ClearedBy;
      }
      this.dateCleared.Visible = this.chkCleared.Checked && this.rights.CanEditPostClosingCondition;
      this.txtDateCleared.Visible = this.chkCleared.Checked && !this.rights.CanEditPostClosingCondition;
      this.txtClearedBy.Visible = this.chkCleared.Checked;
      this.btnClearedBy.Visible = this.chkCleared.Checked && this.rights.CanEditPostClosingCondition;
    }

    private void chkCleared_Click(object sender, EventArgs e)
    {
      if (this.chkCleared.Checked)
        this.cond.MarkAsCleared(DateTime.Now, Session.UserID);
      else
        this.cond.UnmarkAsCleared();
    }

    private void dateCleared_ValueChanged(object sender, EventArgs e)
    {
      if (!(this.dateCleared.Value != this.cond.DateCleared))
        return;
      this.cond.MarkAsCleared(this.dateCleared.Value, this.txtClearedBy.Text);
    }

    private void btnClearedBy_Click(object sender, EventArgs e)
    {
      string user = this.selectUser();
      if (user == null)
        return;
      this.cond.MarkAsCleared(this.cond.DateCleared, user);
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
      this.cboTitle.Visible = this.rights.CanEditPostClosingCondition;
      this.txtTitle.Visible = !this.rights.CanEditPostClosingCondition;
      this.txtDescription.ReadOnly = !this.rights.CanEditPostClosingCondition;
      this.cboBorrower.Visible = this.rights.CanEditPostClosingCondition;
      this.txtBorrower.Visible = !this.rights.CanEditPostClosingCondition;
      this.cboSource.Visible = this.rights.CanEditPostClosingCondition;
      this.txtSource.Visible = !this.rights.CanEditPostClosingCondition;
      this.cboRecipient.Visible = this.rights.CanEditPostClosingCondition;
      this.txtRecipient.Visible = !this.rights.CanEditPostClosingCondition;
      this.txtDaysDue.ReadOnly = !this.rights.CanEditPostClosingCondition;
      this.txtRequestedFrom.ReadOnly = !this.rights.CanEditPostClosingCondition;
      this.chkRequested.Enabled = this.rights.CanEditPostClosingCondition;
      this.chkRerequested.Enabled = this.rights.CanEditPostClosingCondition;
      this.chkReceived.Enabled = this.rights.CanEditPostClosingCondition;
      this.chkSent.Enabled = this.rights.CanEditPostClosingCondition;
      this.chkCleared.Enabled = this.rights.CanEditPostClosingCondition;
      this.chkInternal.Enabled = this.rights.CanEditPostClosingCondition;
      this.chkExternal.Enabled = this.rights.CanEditPostClosingCondition;
      this.commentCollection.CanAddComment = this.rights.CanEditPostClosingCondition;
      this.commentCollection.CanDeleteComment = this.rights.CanEditPostClosingCondition;
      this.btnAddDocument.Visible = this.rights.CanAddPostClosingConditionDocuments;
      this.btnRemoveDocument.Visible = this.rights.CanRemovePostClosingConditionDocuments;
      this.separator.Visible = this.rights.CanRequestDocuments || this.rights.CanRequestServices;
      this.btnRequestDocument.Visible = this.rights.CanRequestDocuments || this.rights.CanRequestServices;
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
      Tracing.Log(PostClosingDetailsDialog.sw, TraceLevel.Verbose, nameof (PostClosingDetailsDialog), "Checking InvokeRequired For LogRecordChanged");
      if (this.InvokeRequired)
      {
        LogRecordEventHandler method = new LogRecordEventHandler(this.logRecordChanged);
        Tracing.Log(PostClosingDetailsDialog.sw, TraceLevel.Verbose, nameof (PostClosingDetailsDialog), "Calling BeginInvoke For LogRecordChanged");
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
      Tracing.Log(PostClosingDetailsDialog.sw, TraceLevel.Verbose, nameof (PostClosingDetailsDialog), "Checking InvokeRequired For LogRecordRemoved");
      if (this.InvokeRequired)
      {
        LogRecordEventHandler method = new LogRecordEventHandler(this.logRecordRemoved);
        Tracing.Log(PostClosingDetailsDialog.sw, TraceLevel.Verbose, nameof (PostClosingDetailsDialog), "Calling BeginInvoke For LogRecordRemoved");
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
      this.cond = (PostClosingConditionLog) this.loanDataMgr.LoanData.GetLogList().GetRecordByID(this.cond.Guid);
      this.loadConditionDetails();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      if (sender is LoanDataMgr)
        this.AutoValidate = AutoValidate.Disable;
      this.Close();
    }

    private void PostClosingDetailsDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      Session.Application.DisplayHelp(this.helpLink.HelpTag);
    }

    private void PostClosingDetailsDialog_Activated(object sender, EventArgs e)
    {
      if (!this.refreshDocuments)
        return;
      this.loadDocumentList(false);
      this.showDocumentFiles();
      this.refreshDocuments = false;
    }

    private void PostClosingDetailsDialog_FormClosing(object sender, FormClosingEventArgs e)
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
      this.pnlLeft = new Panel();
      this.gcTracking = new GroupContainer();
      this.tabTracking = new TabControl();
      this.pageStatus = new TabPage();
      this.btnClearedBy = new StandardIconButton();
      this.txtClearedBy = new TextBox();
      this.dateCleared = new DateTimePicker();
      this.txtDateCleared = new TextBox();
      this.chkCleared = new CheckBox();
      this.btnSentBy = new StandardIconButton();
      this.txtSentBy = new TextBox();
      this.dateSent = new DateTimePicker();
      this.txtDateSent = new TextBox();
      this.chkSent = new CheckBox();
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
      this.txtAddedBy = new TextBox();
      this.txtAddedDate = new TextBox();
      this.chkAdded = new CheckBox();
      this.txtRequestedFrom = new TextBox();
      this.lblRequestedFrom = new Label();
      this.txtDateDue = new TextBox();
      this.txtDaysDue = new TextBox();
      this.lblDaysDue = new Label();
      this.pageComments = new TabPage();
      this.commentCollection = new CommentCollectionControl();
      this.csDetails = new CollapsibleSplitter();
      this.gcDetails = new GroupContainer();
      this.pnlDetails = new Panel();
      this.cboRecipient = new ComboBox();
      this.txtRecipient = new TextBox();
      this.lblRecipient = new Label();
      this.cboSource = new ComboBox();
      this.txtSource = new TextBox();
      this.lblSource = new Label();
      this.cboBorrower = new ComboBox();
      this.txtBorrower = new TextBox();
      this.lblBorrower = new Label();
      this.txtDescription = new TextBox();
      this.lblDescription = new Label();
      this.cboTitle = new ComboBox();
      this.txtTitle = new TextBox();
      this.lblTitle = new Label();
      this.pnlClose = new Panel();
      this.helpLink = new EMHelpLink();
      this.btnClose = new Button();
      this.pnlRight = new Panel();
      this.pnlViewer = new BorderPanel();
      this.fileViewer = new FileAttachmentViewerControl();
      this.csDocuments = new CollapsibleSplitter();
      this.gcDocuments = new GroupContainer();
      this.pnlToolbar = new FlowLayoutPanel();
      this.btnRequestDocument = new Button();
      this.separator = new VerticalSeparator();
      this.btnRemoveDocument = new StandardIconButton();
      this.btnEditDocument = new StandardIconButton();
      this.btnAddDocument = new StandardIconButton();
      this.gvDocuments = new GridView();
      this.tooltip = new ToolTip(this.components);
      this.csLeft = new CollapsibleSplitter();
      this.lblPrint = new Label();
      this.chkExternal = new CheckBox();
      this.chkInternal = new CheckBox();
      this.pnlLeft.SuspendLayout();
      this.gcTracking.SuspendLayout();
      this.tabTracking.SuspendLayout();
      this.pageStatus.SuspendLayout();
      ((ISupportInitialize) this.btnClearedBy).BeginInit();
      ((ISupportInitialize) this.btnSentBy).BeginInit();
      ((ISupportInitialize) this.btnReceivedBy).BeginInit();
      ((ISupportInitialize) this.btnRerequestedBy).BeginInit();
      ((ISupportInitialize) this.btnRequestedBy).BeginInit();
      this.pageComments.SuspendLayout();
      this.gcDetails.SuspendLayout();
      this.pnlDetails.SuspendLayout();
      this.pnlClose.SuspendLayout();
      this.pnlRight.SuspendLayout();
      this.pnlViewer.SuspendLayout();
      this.gcDocuments.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      ((ISupportInitialize) this.btnRemoveDocument).BeginInit();
      ((ISupportInitialize) this.btnEditDocument).BeginInit();
      ((ISupportInitialize) this.btnAddDocument).BeginInit();
      this.SuspendLayout();
      this.pnlLeft.BackColor = Color.WhiteSmoke;
      this.pnlLeft.Controls.Add((Control) this.gcTracking);
      this.pnlLeft.Controls.Add((Control) this.csDetails);
      this.pnlLeft.Controls.Add((Control) this.gcDetails);
      this.pnlLeft.Dock = DockStyle.Left;
      this.pnlLeft.Location = new Point(0, 0);
      this.pnlLeft.Name = "pnlLeft";
      this.pnlLeft.Size = new Size(332, 521);
      this.pnlLeft.TabIndex = 0;
      this.gcTracking.Controls.Add((Control) this.tabTracking);
      this.gcTracking.Dock = DockStyle.Fill;
      this.gcTracking.HeaderForeColor = SystemColors.ControlText;
      this.gcTracking.Location = new Point(0, 272);
      this.gcTracking.Name = "gcTracking";
      this.gcTracking.Padding = new Padding(3, 3, 2, 2);
      this.gcTracking.Size = new Size(332, 249);
      this.gcTracking.TabIndex = 18;
      this.gcTracking.Text = "Tracking";
      this.tabTracking.Controls.Add((Control) this.pageStatus);
      this.tabTracking.Controls.Add((Control) this.pageComments);
      this.tabTracking.Dock = DockStyle.Fill;
      this.tabTracking.Location = new Point(4, 29);
      this.tabTracking.Name = "tabTracking";
      this.tabTracking.SelectedIndex = 0;
      this.tabTracking.Size = new Size(325, 217);
      this.tabTracking.TabIndex = 19;
      this.pageStatus.AutoScroll = true;
      this.pageStatus.AutoScrollMargin = new Size(8, 8);
      this.pageStatus.BackColor = Color.WhiteSmoke;
      this.pageStatus.Controls.Add((Control) this.btnClearedBy);
      this.pageStatus.Controls.Add((Control) this.txtClearedBy);
      this.pageStatus.Controls.Add((Control) this.dateCleared);
      this.pageStatus.Controls.Add((Control) this.txtDateCleared);
      this.pageStatus.Controls.Add((Control) this.chkCleared);
      this.pageStatus.Controls.Add((Control) this.btnSentBy);
      this.pageStatus.Controls.Add((Control) this.txtSentBy);
      this.pageStatus.Controls.Add((Control) this.dateSent);
      this.pageStatus.Controls.Add((Control) this.txtDateSent);
      this.pageStatus.Controls.Add((Control) this.chkSent);
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
      this.pageStatus.Controls.Add((Control) this.txtAddedBy);
      this.pageStatus.Controls.Add((Control) this.txtAddedDate);
      this.pageStatus.Controls.Add((Control) this.chkAdded);
      this.pageStatus.Controls.Add((Control) this.txtRequestedFrom);
      this.pageStatus.Controls.Add((Control) this.lblRequestedFrom);
      this.pageStatus.Controls.Add((Control) this.txtDateDue);
      this.pageStatus.Controls.Add((Control) this.txtDaysDue);
      this.pageStatus.Controls.Add((Control) this.lblDaysDue);
      this.pageStatus.Location = new Point(4, 23);
      this.pageStatus.Name = "pageStatus";
      this.pageStatus.Padding = new Padding(0, 2, 2, 2);
      this.pageStatus.Size = new Size(317, 190);
      this.pageStatus.TabIndex = 0;
      this.pageStatus.Text = "Status";
      this.pageStatus.UseVisualStyleBackColor = true;
      this.btnClearedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClearedBy.BackColor = Color.Transparent;
      this.btnClearedBy.Location = new Point(276, 179);
      this.btnClearedBy.MouseDownImage = (Image) null;
      this.btnClearedBy.Name = "btnClearedBy";
      this.btnClearedBy.Size = new Size(16, 16);
      this.btnClearedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnClearedBy.TabIndex = 46;
      this.btnClearedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnClearedBy, "Select User");
      this.btnClearedBy.Click += new EventHandler(this.btnClearedBy_Click);
      this.txtClearedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtClearedBy.Location = new Point(208, 176);
      this.txtClearedBy.Name = "txtClearedBy";
      this.txtClearedBy.ReadOnly = true;
      this.txtClearedBy.Size = new Size(64, 20);
      this.txtClearedBy.TabIndex = 47;
      this.dateCleared.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateCleared.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateCleared.CalendarTitleForeColor = Color.White;
      this.dateCleared.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateCleared.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateCleared.Format = DateTimePickerFormat.Custom;
      this.dateCleared.Location = new Point(100, 176);
      this.dateCleared.Name = "dateCleared";
      this.dateCleared.Size = new Size(104, 20);
      this.dateCleared.TabIndex = 45;
      this.dateCleared.ValueChanged += new EventHandler(this.dateCleared_ValueChanged);
      this.txtDateCleared.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateCleared.Location = new Point(100, 176);
      this.txtDateCleared.Name = "txtDateCleared";
      this.txtDateCleared.ReadOnly = true;
      this.txtDateCleared.Size = new Size(104, 20);
      this.txtDateCleared.TabIndex = 46;
      this.chkCleared.AutoSize = true;
      this.chkCleared.Location = new Point(8, 179);
      this.chkCleared.Name = "chkCleared";
      this.chkCleared.Size = new Size(63, 18);
      this.chkCleared.TabIndex = 44;
      this.chkCleared.Text = "Cleared";
      this.chkCleared.UseVisualStyleBackColor = true;
      this.chkCleared.Click += new EventHandler(this.chkCleared_Click);
      this.btnSentBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSentBy.BackColor = Color.Transparent;
      this.btnSentBy.Location = new Point(276, 155);
      this.btnSentBy.MouseDownImage = (Image) null;
      this.btnSentBy.Name = "btnSentBy";
      this.btnSentBy.Size = new Size(16, 16);
      this.btnSentBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSentBy.TabIndex = 42;
      this.btnSentBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnSentBy, "Select User");
      this.btnSentBy.Click += new EventHandler(this.btnSentBy_Click);
      this.txtSentBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtSentBy.Location = new Point(208, 152);
      this.txtSentBy.Name = "txtSentBy";
      this.txtSentBy.ReadOnly = true;
      this.txtSentBy.Size = new Size(64, 20);
      this.txtSentBy.TabIndex = 43;
      this.dateSent.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateSent.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateSent.CalendarTitleForeColor = Color.White;
      this.dateSent.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateSent.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateSent.Format = DateTimePickerFormat.Custom;
      this.dateSent.Location = new Point(100, 152);
      this.dateSent.Name = "dateSent";
      this.dateSent.Size = new Size(104, 20);
      this.dateSent.TabIndex = 41;
      this.dateSent.ValueChanged += new EventHandler(this.dateSent_ValueChanged);
      this.txtDateSent.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateSent.Location = new Point(100, 152);
      this.txtDateSent.Name = "txtDateSent";
      this.txtDateSent.ReadOnly = true;
      this.txtDateSent.Size = new Size(104, 20);
      this.txtDateSent.TabIndex = 42;
      this.chkSent.AutoSize = true;
      this.chkSent.Location = new Point(8, 155);
      this.chkSent.Name = "chkSent";
      this.chkSent.Size = new Size(48, 18);
      this.chkSent.TabIndex = 40;
      this.chkSent.Text = "Sent";
      this.chkSent.UseVisualStyleBackColor = true;
      this.chkSent.Click += new EventHandler(this.chkSent_Click);
      this.btnReceivedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReceivedBy.BackColor = Color.Transparent;
      this.btnReceivedBy.Location = new Point(276, 131);
      this.btnReceivedBy.MouseDownImage = (Image) null;
      this.btnReceivedBy.Name = "btnReceivedBy";
      this.btnReceivedBy.Size = new Size(16, 16);
      this.btnReceivedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnReceivedBy.TabIndex = 38;
      this.btnReceivedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnReceivedBy, "Select User");
      this.btnReceivedBy.Click += new EventHandler(this.btnReceivedBy_Click);
      this.txtReceivedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtReceivedBy.Location = new Point(208, 128);
      this.txtReceivedBy.Name = "txtReceivedBy";
      this.txtReceivedBy.ReadOnly = true;
      this.txtReceivedBy.Size = new Size(64, 20);
      this.txtReceivedBy.TabIndex = 39;
      this.dateReceived.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateReceived.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateReceived.CalendarTitleForeColor = Color.White;
      this.dateReceived.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateReceived.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateReceived.Format = DateTimePickerFormat.Custom;
      this.dateReceived.Location = new Point(100, 128);
      this.dateReceived.Name = "dateReceived";
      this.dateReceived.Size = new Size(104, 20);
      this.dateReceived.TabIndex = 37;
      this.dateReceived.ValueChanged += new EventHandler(this.dateReceived_ValueChanged);
      this.txtDateReceived.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateReceived.Location = new Point(100, 128);
      this.txtDateReceived.Name = "txtDateReceived";
      this.txtDateReceived.ReadOnly = true;
      this.txtDateReceived.Size = new Size(104, 20);
      this.txtDateReceived.TabIndex = 38;
      this.chkReceived.AutoSize = true;
      this.chkReceived.Location = new Point(8, 131);
      this.chkReceived.Name = "chkReceived";
      this.chkReceived.Size = new Size(71, 18);
      this.chkReceived.TabIndex = 36;
      this.chkReceived.Text = "Received";
      this.chkReceived.UseVisualStyleBackColor = true;
      this.chkReceived.Click += new EventHandler(this.chkReceived_Click);
      this.btnRerequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRerequestedBy.BackColor = Color.Transparent;
      this.btnRerequestedBy.Location = new Point(276, 107);
      this.btnRerequestedBy.MouseDownImage = (Image) null;
      this.btnRerequestedBy.Name = "btnRerequestedBy";
      this.btnRerequestedBy.Size = new Size(16, 16);
      this.btnRerequestedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnRerequestedBy.TabIndex = 34;
      this.btnRerequestedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnRerequestedBy, "Select User");
      this.btnRerequestedBy.Click += new EventHandler(this.btnRerequestedBy_Click);
      this.txtRerequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtRerequestedBy.Location = new Point(208, 104);
      this.txtRerequestedBy.Name = "txtRerequestedBy";
      this.txtRerequestedBy.ReadOnly = true;
      this.txtRerequestedBy.Size = new Size(64, 20);
      this.txtRerequestedBy.TabIndex = 35;
      this.dateRerequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateRerequested.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateRerequested.CalendarTitleForeColor = Color.White;
      this.dateRerequested.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateRerequested.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateRerequested.Format = DateTimePickerFormat.Custom;
      this.dateRerequested.Location = new Point(100, 104);
      this.dateRerequested.Name = "dateRerequested";
      this.dateRerequested.Size = new Size(104, 20);
      this.dateRerequested.TabIndex = 33;
      this.dateRerequested.ValueChanged += new EventHandler(this.dateRerequested_ValueChanged);
      this.txtDateRerequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateRerequested.Location = new Point(100, 104);
      this.txtDateRerequested.Name = "txtDateRerequested";
      this.txtDateRerequested.ReadOnly = true;
      this.txtDateRerequested.Size = new Size(104, 20);
      this.txtDateRerequested.TabIndex = 34;
      this.chkRerequested.AutoSize = true;
      this.chkRerequested.Location = new Point(8, 107);
      this.chkRerequested.Name = "chkRerequested";
      this.chkRerequested.Size = new Size(92, 18);
      this.chkRerequested.TabIndex = 32;
      this.chkRerequested.Text = "Re-requested";
      this.chkRerequested.UseVisualStyleBackColor = true;
      this.chkRerequested.Click += new EventHandler(this.chkRerequested_Click);
      this.btnRequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRequestedBy.BackColor = Color.Transparent;
      this.btnRequestedBy.Location = new Point(276, 83);
      this.btnRequestedBy.MouseDownImage = (Image) null;
      this.btnRequestedBy.Name = "btnRequestedBy";
      this.btnRequestedBy.Size = new Size(16, 16);
      this.btnRequestedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnRequestedBy.TabIndex = 29;
      this.btnRequestedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnRequestedBy, "Select User");
      this.btnRequestedBy.Click += new EventHandler(this.btnRequestedBy_Click);
      this.txtRequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtRequestedBy.Location = new Point(208, 80);
      this.txtRequestedBy.Name = "txtRequestedBy";
      this.txtRequestedBy.ReadOnly = true;
      this.txtRequestedBy.Size = new Size(64, 20);
      this.txtRequestedBy.TabIndex = 31;
      this.dateRequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateRequested.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateRequested.CalendarTitleForeColor = Color.White;
      this.dateRequested.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateRequested.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateRequested.Format = DateTimePickerFormat.Custom;
      this.dateRequested.Location = new Point(100, 80);
      this.dateRequested.Name = "dateRequested";
      this.dateRequested.Size = new Size(104, 20);
      this.dateRequested.TabIndex = 29;
      this.dateRequested.ValueChanged += new EventHandler(this.dateRequested_ValueChanged);
      this.txtDateRequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateRequested.Location = new Point(100, 80);
      this.txtDateRequested.Name = "txtDateRequested";
      this.txtDateRequested.ReadOnly = true;
      this.txtDateRequested.Size = new Size(104, 20);
      this.txtDateRequested.TabIndex = 30;
      this.chkRequested.AutoSize = true;
      this.chkRequested.Location = new Point(8, 83);
      this.chkRequested.Name = "chkRequested";
      this.chkRequested.Size = new Size(78, 18);
      this.chkRequested.TabIndex = 28;
      this.chkRequested.Text = "Requested";
      this.chkRequested.UseVisualStyleBackColor = true;
      this.chkRequested.Click += new EventHandler(this.chkRequested_Click);
      this.txtAddedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtAddedBy.Location = new Point(208, 56);
      this.txtAddedBy.Name = "txtAddedBy";
      this.txtAddedBy.ReadOnly = true;
      this.txtAddedBy.Size = new Size(64, 20);
      this.txtAddedBy.TabIndex = 27;
      this.txtAddedDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtAddedDate.Location = new Point(100, 56);
      this.txtAddedDate.Name = "txtAddedDate";
      this.txtAddedDate.ReadOnly = true;
      this.txtAddedDate.Size = new Size(104, 20);
      this.txtAddedDate.TabIndex = 26;
      this.chkAdded.AutoSize = true;
      this.chkAdded.Checked = true;
      this.chkAdded.CheckState = CheckState.Checked;
      this.chkAdded.Enabled = false;
      this.chkAdded.Location = new Point(8, 59);
      this.chkAdded.Name = "chkAdded";
      this.chkAdded.Size = new Size(58, 18);
      this.chkAdded.TabIndex = 25;
      this.chkAdded.Text = "Added";
      this.chkAdded.UseVisualStyleBackColor = true;
      this.txtRequestedFrom.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtRequestedFrom.Location = new Point(100, 32);
      this.txtRequestedFrom.Name = "txtRequestedFrom";
      this.txtRequestedFrom.Size = new Size(172, 20);
      this.txtRequestedFrom.TabIndex = 24;
      this.txtRequestedFrom.Validated += new EventHandler(this.txtRequestedFrom_Validated);
      this.lblRequestedFrom.AutoSize = true;
      this.lblRequestedFrom.Location = new Point(8, 35);
      this.lblRequestedFrom.Name = "lblRequestedFrom";
      this.lblRequestedFrom.Size = new Size(86, 14);
      this.lblRequestedFrom.TabIndex = 23;
      this.lblRequestedFrom.Text = "Requested From";
      this.txtDateDue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateDue.Location = new Point(152, 8);
      this.txtDateDue.Name = "txtDateDue";
      this.txtDateDue.ReadOnly = true;
      this.txtDateDue.Size = new Size(120, 20);
      this.txtDateDue.TabIndex = 22;
      this.txtDaysDue.Location = new Point(100, 8);
      this.txtDaysDue.Name = "txtDaysDue";
      this.txtDaysDue.Size = new Size(48, 20);
      this.txtDaysDue.TabIndex = 21;
      this.txtDaysDue.TextAlign = HorizontalAlignment.Right;
      this.txtDaysDue.Validating += new CancelEventHandler(this.txtDaysDue_Validating);
      this.txtDaysDue.Validated += new EventHandler(this.txtDaysDue_Validated);
      this.lblDaysDue.AutoSize = true;
      this.lblDaysDue.Location = new Point(8, 11);
      this.lblDaysDue.Name = "lblDaysDue";
      this.lblDaysDue.Size = new Size(86, 14);
      this.lblDaysDue.TabIndex = 20;
      this.lblDaysDue.Text = "Days to Receive";
      this.pageComments.BackColor = Color.WhiteSmoke;
      this.pageComments.Controls.Add((Control) this.commentCollection);
      this.pageComments.Location = new Point(4, 23);
      this.pageComments.Name = "pageComments";
      this.pageComments.Padding = new Padding(0, 2, 2, 2);
      this.pageComments.Size = new Size(317, 215);
      this.pageComments.TabIndex = 1;
      this.pageComments.Text = "Comments";
      this.pageComments.UseVisualStyleBackColor = true;
      this.commentCollection.CanAddComment = false;
      this.commentCollection.CanDeleteComment = false;
      this.commentCollection.Dock = DockStyle.Fill;
      this.commentCollection.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.commentCollection.Location = new Point(0, 2);
      this.commentCollection.Name = "commentCollection";
      this.commentCollection.Size = new Size(315, 211);
      this.commentCollection.TabIndex = 48;
      this.csDetails.AnimationDelay = 20;
      this.csDetails.AnimationStep = 20;
      this.csDetails.BorderStyle3D = Border3DStyle.Flat;
      this.csDetails.ControlToHide = (Control) this.gcDetails;
      this.csDetails.Dock = DockStyle.Top;
      this.csDetails.ExpandParentForm = false;
      this.csDetails.Location = new Point(0, 265);
      this.csDetails.Name = "csFiles";
      this.csDetails.TabIndex = 17;
      this.csDetails.TabStop = false;
      this.csDetails.UseAnimations = false;
      this.csDetails.VisualStyle = VisualStyles.Encompass;
      this.gcDetails.Controls.Add((Control) this.pnlDetails);
      this.gcDetails.Dock = DockStyle.Top;
      this.gcDetails.HeaderForeColor = SystemColors.ControlText;
      this.gcDetails.Location = new Point(0, 0);
      this.gcDetails.Name = "gcDetails";
      this.gcDetails.Size = new Size(332, 265);
      this.gcDetails.TabIndex = 1;
      this.gcDetails.Text = "Details";
      this.pnlDetails.AutoScroll = true;
      this.pnlDetails.AutoScrollMargin = new Size(8, 8);
      this.pnlDetails.Controls.Add((Control) this.lblPrint);
      this.pnlDetails.Controls.Add((Control) this.cboRecipient);
      this.pnlDetails.Controls.Add((Control) this.chkExternal);
      this.pnlDetails.Controls.Add((Control) this.txtRecipient);
      this.pnlDetails.Controls.Add((Control) this.chkInternal);
      this.pnlDetails.Controls.Add((Control) this.lblRecipient);
      this.pnlDetails.Controls.Add((Control) this.cboSource);
      this.pnlDetails.Controls.Add((Control) this.txtSource);
      this.pnlDetails.Controls.Add((Control) this.lblSource);
      this.pnlDetails.Controls.Add((Control) this.cboBorrower);
      this.pnlDetails.Controls.Add((Control) this.txtBorrower);
      this.pnlDetails.Controls.Add((Control) this.lblBorrower);
      this.pnlDetails.Controls.Add((Control) this.txtDescription);
      this.pnlDetails.Controls.Add((Control) this.lblDescription);
      this.pnlDetails.Controls.Add((Control) this.cboTitle);
      this.pnlDetails.Controls.Add((Control) this.txtTitle);
      this.pnlDetails.Controls.Add((Control) this.lblTitle);
      this.pnlDetails.Dock = DockStyle.Fill;
      this.pnlDetails.Location = new Point(1, 26);
      this.pnlDetails.Name = "pnlDetails";
      this.pnlDetails.Size = new Size(330, 238);
      this.pnlDetails.TabIndex = 2;
      this.cboRecipient.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboRecipient.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRecipient.FormattingEnabled = true;
      this.cboRecipient.Items.AddRange(new object[2]
      {
        (object) "Investor",
        (object) "MERS"
      });
      this.cboRecipient.Location = new Point(84, 184);
      this.cboRecipient.Name = "cboRecipient";
      this.cboRecipient.Size = new Size(236, 22);
      this.cboRecipient.TabIndex = 15;
      this.cboRecipient.SelectionChangeCommitted += new EventHandler(this.cboRecipient_SelectionChangeCommitted);
      this.txtRecipient.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtRecipient.Location = new Point(84, 184);
      this.txtRecipient.Name = "txtRecipient";
      this.txtRecipient.ReadOnly = true;
      this.txtRecipient.Size = new Size(236, 20);
      this.txtRecipient.TabIndex = 16;
      this.lblRecipient.AutoSize = true;
      this.lblRecipient.Location = new Point(8, 188);
      this.lblRecipient.Name = "lblRecipient";
      this.lblRecipient.Size = new Size(51, 14);
      this.lblRecipient.TabIndex = 14;
      this.lblRecipient.Text = "Recipient";
      this.cboSource.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboSource.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSource.FormattingEnabled = true;
      this.cboSource.Location = new Point(84, 156);
      this.cboSource.Name = "cboSource";
      this.cboSource.Size = new Size(236, 22);
      this.cboSource.TabIndex = 12;
      this.cboSource.SelectionChangeCommitted += new EventHandler(this.cboSource_SelectionChangeCommitted);
      this.txtSource.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSource.Location = new Point(84, 156);
      this.txtSource.Name = "txtSource";
      this.txtSource.ReadOnly = true;
      this.txtSource.Size = new Size(236, 20);
      this.txtSource.TabIndex = 13;
      this.lblSource.AutoSize = true;
      this.lblSource.Location = new Point(8, 160);
      this.lblSource.Name = "lblSource";
      this.lblSource.Size = new Size(42, 14);
      this.lblSource.TabIndex = 11;
      this.lblSource.Text = "Source";
      this.cboBorrower.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboBorrower.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboBorrower.FormattingEnabled = true;
      this.cboBorrower.Location = new Point(84, 128);
      this.cboBorrower.Name = "cboBorrower";
      this.cboBorrower.Size = new Size(236, 22);
      this.cboBorrower.TabIndex = 9;
      this.cboBorrower.SelectionChangeCommitted += new EventHandler(this.cboBorrower_SelectionChangeCommitted);
      this.txtBorrower.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBorrower.Location = new Point(84, 128);
      this.txtBorrower.Name = "txtBorrower";
      this.txtBorrower.ReadOnly = true;
      this.txtBorrower.Size = new Size(236, 20);
      this.txtBorrower.TabIndex = 10;
      this.lblBorrower.AutoSize = true;
      this.lblBorrower.Location = new Point(8, 124);
      this.lblBorrower.MaximumSize = new Size(75, 0);
      this.lblBorrower.Name = "lblBorrower";
      this.lblBorrower.Size = new Size(73, 28);
      this.lblBorrower.TabIndex = 8;
      this.lblBorrower.Text = "For Borrower Pair";
      this.txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDescription.Location = new Point(84, 36);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ScrollBars = ScrollBars.Vertical;
      this.txtDescription.Size = new Size(236, 86);
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
      this.cboTitle.Location = new Point(84, 8);
      this.cboTitle.Name = "cboTitle";
      this.cboTitle.Size = new Size(236, 22);
      this.cboTitle.Sorted = true;
      this.cboTitle.TabIndex = 4;
      this.cboTitle.SelectionChangeCommitted += new EventHandler(this.cboTitle_SelectionChangeCommitted);
      this.cboTitle.Validating += new CancelEventHandler(this.cboTitle_Validating);
      this.cboTitle.Validated += new EventHandler(this.cboTitle_Validated);
      this.txtTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtTitle.Location = new Point(84, 8);
      this.txtTitle.Name = "txtTitle";
      this.txtTitle.ReadOnly = true;
      this.txtTitle.Size = new Size(236, 20);
      this.txtTitle.TabIndex = 5;
      this.lblTitle.AutoSize = true;
      this.lblTitle.Location = new Point(8, 12);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(34, 14);
      this.lblTitle.TabIndex = 3;
      this.lblTitle.Text = "Name";
      this.pnlClose.Controls.Add((Control) this.helpLink);
      this.pnlClose.Controls.Add((Control) this.btnClose);
      this.pnlClose.Dock = DockStyle.Bottom;
      this.pnlClose.Location = new Point(0, 521);
      this.pnlClose.Name = "pnlClose";
      this.pnlClose.Size = new Size(774, 40);
      this.pnlClose.TabIndex = 59;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "Condition Details";
      this.helpLink.Location = new Point(8, 11);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 18);
      this.helpLink.TabIndex = 61;
      this.helpLink.TabStop = false;
      this.btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.OK;
      this.btnClose.Location = new Point(691, 9);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 22);
      this.btnClose.TabIndex = 60;
      this.btnClose.TabStop = false;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.pnlRight.Controls.Add((Control) this.pnlViewer);
      this.pnlRight.Controls.Add((Control) this.csDocuments);
      this.pnlRight.Controls.Add((Control) this.gcDocuments);
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(339, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(435, 521);
      this.pnlRight.TabIndex = 50;
      this.pnlViewer.Controls.Add((Control) this.fileViewer);
      this.pnlViewer.Dock = DockStyle.Fill;
      this.pnlViewer.Location = new Point(0, (int) sbyte.MaxValue);
      this.pnlViewer.Name = "pnlViewer";
      this.pnlViewer.Size = new Size(435, 394);
      this.pnlViewer.TabIndex = 57;
      this.fileViewer.Dock = DockStyle.Fill;
      this.fileViewer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.fileViewer.Location = new Point(1, 1);
      this.fileViewer.Name = "fileViewer";
      this.fileViewer.Size = new Size(433, 392);
      this.fileViewer.TabIndex = 58;
      this.csDocuments.AnimationDelay = 20;
      this.csDocuments.AnimationStep = 20;
      this.csDocuments.BorderStyle3D = Border3DStyle.Flat;
      this.csDocuments.ControlToHide = (Control) this.gcDocuments;
      this.csDocuments.Dock = DockStyle.Top;
      this.csDocuments.ExpandParentForm = false;
      this.csDocuments.Location = new Point(0, 120);
      this.csDocuments.Name = "csFiles";
      this.csDocuments.TabIndex = 56;
      this.csDocuments.TabStop = false;
      this.csDocuments.UseAnimations = false;
      this.csDocuments.VisualStyle = VisualStyles.Encompass;
      this.gcDocuments.Controls.Add((Control) this.pnlToolbar);
      this.gcDocuments.Controls.Add((Control) this.gvDocuments);
      this.gcDocuments.Dock = DockStyle.Top;
      this.gcDocuments.HeaderForeColor = SystemColors.ControlText;
      this.gcDocuments.Location = new Point(0, 0);
      this.gcDocuments.Name = "gcDocuments";
      this.gcDocuments.Size = new Size(435, 120);
      this.gcDocuments.TabIndex = 51;
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
      this.pnlToolbar.TabIndex = 52;
      this.btnRequestDocument.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRequestDocument.Location = new Point(96, 0);
      this.btnRequestDocument.Margin = new Padding(0);
      this.btnRequestDocument.Name = "btnRequestDocument";
      this.btnRequestDocument.Size = new Size(64, 22);
      this.btnRequestDocument.TabIndex = 54;
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
      this.separator.TabIndex = 53;
      this.separator.TabStop = false;
      this.btnRemoveDocument.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRemoveDocument.BackColor = Color.Transparent;
      this.btnRemoveDocument.Location = new Point(71, 3);
      this.btnRemoveDocument.Margin = new Padding(4, 3, 0, 3);
      this.btnRemoveDocument.MouseDownImage = (Image) null;
      this.btnRemoveDocument.Name = "btnRemoveDocument";
      this.btnRemoveDocument.Size = new Size(16, 16);
      this.btnRemoveDocument.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemoveDocument.TabIndex = 27;
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
      this.btnEditDocument.TabIndex = 26;
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
      this.btnAddDocument.TabIndex = 30;
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
      this.gvDocuments.TabIndex = 55;
      this.gvDocuments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvDocuments.BeforeSelectedIndexCommitted += new CancelEventHandler(this.gvDocuments_BeforeSelectedIndexCommitted);
      this.gvDocuments.SelectedIndexCommitted += new EventHandler(this.gvDocuments_SelectedIndexCommitted);
      this.gvDocuments.ItemDoubleClick += new GVItemEventHandler(this.gvDocuments_ItemDoubleClick);
      this.csLeft.AnimationDelay = 20;
      this.csLeft.AnimationStep = 20;
      this.csLeft.BorderStyle3D = Border3DStyle.Flat;
      this.csLeft.ControlToHide = (Control) this.pnlLeft;
      this.csLeft.ExpandParentForm = false;
      this.csLeft.Location = new Point(332, 0);
      this.csLeft.Name = "csDocTracking";
      this.csLeft.TabIndex = 49;
      this.csLeft.TabStop = false;
      this.csLeft.UseAnimations = false;
      this.csLeft.VisualStyle = VisualStyles.Encompass;
      this.lblPrint.AutoSize = true;
      this.lblPrint.Location = new Point(8, 213);
      this.lblPrint.Name = "lblPrint";
      this.lblPrint.Size = new Size(28, 14);
      this.lblPrint.TabIndex = 27;
      this.lblPrint.Text = "Print";
      this.chkExternal.AutoSize = true;
      this.chkExternal.Location = new Point(156, 212);
      this.chkExternal.Name = "chkExternal";
      this.chkExternal.Size = new Size(73, 18);
      this.chkExternal.TabIndex = 29;
      this.chkExternal.Text = "Externally";
      this.chkExternal.UseVisualStyleBackColor = true;
      this.chkExternal.Click += new EventHandler(this.chkExternal_Click);
      this.chkInternal.AutoSize = true;
      this.chkInternal.Location = new Point(84, 212);
      this.chkInternal.Name = "chkInternal";
      this.chkInternal.Size = new Size(69, 18);
      this.chkInternal.TabIndex = 28;
      this.chkInternal.Text = "Internally";
      this.chkInternal.UseVisualStyleBackColor = true;
      this.chkInternal.Click += new EventHandler(this.chkInternal_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(774, 561);
      this.Controls.Add((Control) this.pnlRight);
      this.Controls.Add((Control) this.csLeft);
      this.Controls.Add((Control) this.pnlLeft);
      this.Controls.Add((Control) this.pnlClose);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = Resources.icon_allsizes_bug;
      this.KeyPreview = true;
      this.Name = nameof (PostClosingDetailsDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Post-Closing Condition Details";
      this.Activated += new EventHandler(this.PostClosingDetailsDialog_Activated);
      this.FormClosing += new FormClosingEventHandler(this.PostClosingDetailsDialog_FormClosing);
      this.KeyDown += new KeyEventHandler(this.PostClosingDetailsDialog_KeyDown);
      this.pnlLeft.ResumeLayout(false);
      this.gcTracking.ResumeLayout(false);
      this.tabTracking.ResumeLayout(false);
      this.pageStatus.ResumeLayout(false);
      this.pageStatus.PerformLayout();
      ((ISupportInitialize) this.btnClearedBy).EndInit();
      ((ISupportInitialize) this.btnSentBy).EndInit();
      ((ISupportInitialize) this.btnReceivedBy).EndInit();
      ((ISupportInitialize) this.btnRerequestedBy).EndInit();
      ((ISupportInitialize) this.btnRequestedBy).EndInit();
      this.pageComments.ResumeLayout(false);
      this.gcDetails.ResumeLayout(false);
      this.pnlDetails.ResumeLayout(false);
      this.pnlDetails.PerformLayout();
      this.pnlClose.ResumeLayout(false);
      this.pnlRight.ResumeLayout(false);
      this.pnlViewer.ResumeLayout(false);
      this.gcDocuments.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemoveDocument).EndInit();
      ((ISupportInitialize) this.btnEditDocument).EndInit();
      ((ISupportInitialize) this.btnAddDocument).EndInit();
      this.ResumeLayout(false);
    }
  }
}
