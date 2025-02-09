// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.SellDetailsDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.DataDocs;
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
  public class SellDetailsDialog : Form
  {
    private const string className = "SellDetailsDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private static List<SellDetailsDialog> _instanceList = new List<SellDetailsDialog>();
    private List<PartnerResponseBody> _partners = new List<PartnerResponseBody>();
    private LoanDataMgr loanDataMgr;
    private SellConditionLog cond;
    private SellConditionTrackingSetup condSetup;
    private GridViewDataManager gvDocumentsMgr;
    private eFolderAccessRights rights;
    private bool canEditSellCondition = true;
    private bool refreshDocuments;
    private IContainer components;
    private Panel pnlDetails;
    private BorderPanel pnlViewer;
    private Panel pnlRight;
    private CollapsibleSplitter csDocuments;
    private GroupContainer gcDocuments;
    private StandardIconButton btnAddDocument;
    private StandardIconButton btnRemoveDocument;
    private StandardIconButton btnEditDocument;
    private GridView gvDocuments;
    private CollapsibleSplitter csLeft;
    private Panel pnlLeft;
    private GroupContainer gcTracking;
    private TabControl tabTracking;
    private TabPage pageStatus;
    private TabPage pageComments;
    private CollapsibleSplitter csDetails;
    private GroupContainer gcDetails;
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
    private StandardIconButton btnReviewedBy;
    private TextBox txtReviewedBy;
    private DateTimePicker dateReviewed;
    private CheckBox chkReviewed;
    private StandardIconButton btnReceivedBy;
    private CheckBox chkReceived;
    private TextBox txtAddedDate;
    private TextBox txtAddedBy;
    private CheckBox chkAdded;
    private VerticalSeparator separator;
    private CheckBox chkInternal;
    private ComboBox cboPriorTo;
    private System.Windows.Forms.LinkLabel lnkViewMore;
    private ComboBox cboOwner;
    private Label lblOwner;
    private ComboBox cboCategory;
    private Label lblCategory;
    private TextBox txtSource;
    private Label lblSource;
    private ComboBox cboBorrower;
    private TextBox txtDescription;
    private Label lblTitle;
    private ComboBox cboTitle;
    private Label lblDescription;
    private Label lblPriorTo;
    private Label lblBorrower;
    private CheckBox chkAllowToClear;
    private Panel pnlClose;
    private Button btnClose;
    private CommentCollectionControl commentCollection;
    private TextBox txtTitle;
    private TextBox txtBorrower;
    private TextBox txtCategory;
    private TextBox txtPriorTo;
    private TextBox txtOwner;
    private TextBox txtDateReviewed;
    private TextBox txtDateRejected;
    private TextBox txtDateCleared;
    private TextBox txtDateWaived;
    private FlowLayoutPanel pnlToolbar;
    private ToolTip tooltip;
    private Label lblPrint;
    private CheckBox chkExternal;
    private EMHelpLink helpLink;
    private TextBox txtRequestedFrom;
    private Label lblRequestedFrom;
    private TextBox txtDateDue;
    private TextBox txtDaysDue;
    private Label lblDaysDue;
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
    private StandardIconButton btnFulfilledBy;
    private TextBox txtFulfilledBy;
    private DateTimePicker dateFulfilled;
    private TextBox txtDateFulfilled;
    private CheckBox chkFulfilled;
    private TextBox txtReceivedBy;
    private DateTimePicker dateReceived;
    private TextBox txtDateReceived;
    private FileAttachmentViewerControl fileViewer;
    private Button btnRequestDocument;
    private TextBox txtConditionCode;
    private Label lblconditioncode;

    public static void ShowInstance(LoanDataMgr loanDataMgr, SellConditionLog cond)
    {
      if (Form.ActiveForm != null && Form.ActiveForm.Modal)
      {
        using (SellDetailsDialog sellDetailsDialog = new SellDetailsDialog(loanDataMgr, cond))
        {
          int num = (int) sellDetailsDialog.ShowDialog((IWin32Window) Form.ActiveForm);
        }
      }
      else
      {
        SellDetailsDialog sellDetailsDialog1 = (SellDetailsDialog) null;
        foreach (SellDetailsDialog instance in SellDetailsDialog._instanceList)
        {
          if (instance.Condition == cond)
          {
            sellDetailsDialog1 = instance;
            break;
          }
        }
        if (sellDetailsDialog1 == null)
        {
          SellDetailsDialog sellDetailsDialog2 = new SellDetailsDialog(loanDataMgr, cond);
          sellDetailsDialog2.FormClosing += new FormClosingEventHandler(SellDetailsDialog._instance_FormClosing);
          SellDetailsDialog._instanceList.Add(sellDetailsDialog2);
          sellDetailsDialog2.Show();
        }
        else
        {
          if (sellDetailsDialog1.WindowState == FormWindowState.Minimized)
            sellDetailsDialog1.WindowState = FormWindowState.Normal;
          sellDetailsDialog1.Activate();
        }
      }
    }

    public static void CloseInstances()
    {
      if (SellDetailsDialog._instanceList == null && SellDetailsDialog._instanceList.Count == 0)
        return;
      int num = SellDetailsDialog._instanceList.Count - 1;
      try
      {
        for (int index = num; index >= 0; --index)
          SellDetailsDialog._instanceList[index].Close();
      }
      catch
      {
      }
    }

    private static void _instance_FormClosing(object sender, FormClosingEventArgs e)
    {
      SellDetailsDialog sellDetailsDialog = (SellDetailsDialog) sender;
      SellDetailsDialog._instanceList.Remove(sellDetailsDialog);
    }

    private SellDetailsDialog(LoanDataMgr loanDataMgr, SellConditionLog cond)
    {
      this.InitializeComponent();
      this.setWindowSize();
      this.loanDataMgr = loanDataMgr;
      this.cond = cond;
      this.rights = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) cond);
      this.canEditSellCondition = this.rights.CanEditSellCondition;
      this.initEventHandlers();
      this.initTitleField();
      this.initBorrowerField();
      this.initOwnerField();
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

    public SellConditionLog Condition => this.cond;

    private void loadConditionDetails()
    {
      this.loadTitleField();
      this.loadDescriptionField();
      this.loadBorrowerField();
      this.loadSourceField();
      this.loadCategoryField();
      this.loadPriorToField();
      this.loadOwnerField();
      this.loadAllowToClearField();
      this.loadPrintFields();
      this.loadExpectedFields();
      this.loadRequestedFromField();
      this.loadAddedFields();
      this.loadRequestedFields();
      this.loadRerequestedFields();
      this.loadFulfilledFields();
      this.loadReceivedFields();
      this.loadReviewedFields();
      this.loadRejectedFields();
      this.loadClearedFields();
      this.loadWaivedFields();
      this.loadCommentList();
      this.loadConditionCodeField();
    }

    private void initTitleField()
    {
      this.condSetup = this.loanDataMgr.SystemConfiguration.SellConditionTrackingSetup;
      foreach (ConditionTemplate conditionTemplate in (CollectionBase) this.condSetup)
        this.cboTitle.Items.Add((object) conditionTemplate.Name);
    }

    private void loadTitleField()
    {
      this.cboTitle.Text = this.cond.Title;
      this.txtTitle.Text = this.cond.Title;
      this.Text = "Delivery Condition Details (" + this.cond.Title + ")";
    }

    private void cboTitle_SelectionChangeCommitted(object sender, EventArgs e)
    {
      string selectedItem = (string) this.cboTitle.SelectedItem;
      this.Text = "Delivery Condition Details (" + selectedItem + ")";
      SellConditionTemplate byName = this.condSetup.GetByName(selectedItem);
      if (byName == null)
        return;
      this.cond.Title = selectedItem;
      this.cond.Description = byName.Description;
      this.cond.Category = byName.Category;
      this.cond.PriorTo = byName.PriorTo;
      this.cond.ForRoleID = byName.ForRoleID;
      this.cond.AllowToClear = byName.AllowToClear;
      this.cond.IsInternal = byName.IsInternal;
      this.cond.IsExternal = byName.IsExternal;
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

    private void loadSourceField()
    {
      if (!string.IsNullOrEmpty(this.cond.SourceId))
        return;
      this.txtSource.Text = this.cond.Source;
    }

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

    private void initOwnerField()
    {
      this.cboOwner.Items.AddRange((object[]) this.loanDataMgr.SystemConfiguration.AllRoles);
    }

    private void loadOwnerField()
    {
      this.cboOwner.SelectedItem = (object) null;
      foreach (RoleInfo roleInfo in this.cboOwner.Items)
      {
        if (roleInfo.RoleID == this.cond.ForRoleID)
          this.cboOwner.SelectedItem = (object) roleInfo;
      }
      this.txtOwner.Text = this.cboOwner.Text;
    }

    private void cboOwner_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.cond.ForRoleID = ((RoleSummaryInfo) this.cboOwner.SelectedItem).RoleID;
    }

    private void loadAllowToClearField() => this.chkAllowToClear.Checked = this.cond.AllowToClear;

    private void chkAllowToClear_Click(object sender, EventArgs e)
    {
      this.cond.AllowToClear = this.chkAllowToClear.Checked;
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

    private void loadFulfilledFields()
    {
      this.chkFulfilled.Checked = this.cond.Fulfilled;
      if (this.chkFulfilled.Checked)
      {
        this.dateFulfilled.Value = this.cond.DateFulfilled;
        this.txtDateFulfilled.Text = this.cond.DateFulfilled.ToString(this.dateFulfilled.CustomFormat);
        this.txtFulfilledBy.Text = this.cond.FulfilledBy;
      }
      this.dateFulfilled.Visible = this.chkFulfilled.Checked && this.canEditSellCondition;
      this.txtDateFulfilled.Visible = this.chkFulfilled.Checked && !this.canEditSellCondition;
      this.txtFulfilledBy.Visible = this.chkFulfilled.Checked;
      this.btnFulfilledBy.Visible = this.chkFulfilled.Checked && this.canEditSellCondition;
    }

    private void chkFulfilled_Click(object sender, EventArgs e)
    {
      if (this.chkFulfilled.Checked)
        this.cond.MarkAsFulfilled(DateTime.Now, Session.UserID);
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

    private void loadRequestedFields()
    {
      this.chkRequested.Checked = this.cond.Requested;
      if (this.chkRequested.Checked)
      {
        this.dateRequested.Value = this.cond.DateRequested;
        this.txtDateRequested.Text = this.cond.DateRequested.ToString(this.dateRequested.CustomFormat);
        this.txtRequestedBy.Text = this.cond.RequestedBy;
      }
      this.dateRequested.Visible = this.chkRequested.Checked && this.canEditSellCondition;
      this.txtDateRequested.Visible = this.chkRequested.Checked && !this.canEditSellCondition;
      this.txtRequestedBy.Visible = this.chkRequested.Checked;
      this.btnRequestedBy.Visible = this.chkRequested.Checked && this.canEditSellCondition;
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
      this.dateRerequested.Visible = this.chkRerequested.Checked && this.canEditSellCondition;
      this.txtDateRerequested.Visible = this.chkRerequested.Checked && !this.canEditSellCondition;
      this.txtRerequestedBy.Visible = this.chkRerequested.Checked;
      this.btnRerequestedBy.Visible = this.chkRerequested.Checked && this.canEditSellCondition;
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
      this.dateReceived.Visible = this.chkReceived.Checked && this.canEditSellCondition;
      this.txtDateReceived.Visible = this.chkReceived.Checked && !this.canEditSellCondition;
      this.txtReceivedBy.Visible = this.chkReceived.Checked;
      this.btnReceivedBy.Visible = this.chkReceived.Checked && this.canEditSellCondition;
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

    private void loadReviewedFields()
    {
      this.chkReviewed.Checked = this.cond.Reviewed;
      if (this.chkReviewed.Checked)
      {
        this.dateReviewed.Value = this.cond.DateReviewed;
        this.txtDateReviewed.Text = this.cond.DateReviewed.ToString(this.dateReviewed.CustomFormat);
        this.txtReviewedBy.Text = this.cond.ReviewedBy;
      }
      this.dateReviewed.Visible = this.chkReviewed.Checked && this.canEditSellCondition;
      this.txtDateReviewed.Visible = this.chkReviewed.Checked && !this.canEditSellCondition;
      this.txtReviewedBy.Visible = this.chkReviewed.Checked;
      this.btnReviewedBy.Visible = this.chkReviewed.Checked && this.canEditSellCondition;
    }

    private void chkReviewed_Click(object sender, EventArgs e)
    {
      if (this.chkReviewed.Checked)
        this.cond.MarkAsReviewed(DateTime.Now, Session.UserID);
      else
        this.cond.UnmarkAsReviewed();
    }

    private void dateReviewed_ValueChanged(object sender, EventArgs e)
    {
      if (!(this.dateReviewed.Value != this.cond.DateReviewed))
        return;
      this.cond.MarkAsReviewed(this.dateReviewed.Value, this.txtReviewedBy.Text);
    }

    private void btnReviewedBy_Click(object sender, EventArgs e)
    {
      string user = this.selectUser();
      if (user == null)
        return;
      this.cond.MarkAsReviewed(this.cond.DateReviewed, user);
    }

    private void loadRejectedFields()
    {
      this.chkRejected.Checked = this.cond.Rejected;
      if (this.chkRejected.Checked)
      {
        this.dateRejected.Value = this.cond.DateRejected;
        this.txtDateRejected.Text = this.cond.DateRejected.ToString(this.dateRejected.CustomFormat);
        this.txtRejectedBy.Text = this.cond.RejectedBy;
      }
      this.dateRejected.Visible = this.chkRejected.Checked && this.canEditSellCondition;
      this.txtDateRejected.Visible = this.chkRejected.Checked && !this.canEditSellCondition;
      this.txtRejectedBy.Visible = this.chkRejected.Checked;
      this.btnRejectedBy.Visible = this.chkRejected.Checked && this.canEditSellCondition;
    }

    private void chkRejected_Click(object sender, EventArgs e)
    {
      if (this.chkRejected.Checked)
        this.cond.MarkAsRejected(DateTime.Now, Session.UserID);
      else
        this.cond.UnmarkAsRejected();
    }

    private void dateRejected_ValueChanged(object sender, EventArgs e)
    {
      if (!(this.dateRejected.Value != this.cond.DateRejected))
        return;
      this.cond.MarkAsRejected(this.dateRejected.Value, this.txtRejectedBy.Text);
    }

    private void btnRejectedBy_Click(object sender, EventArgs e)
    {
      string user = this.selectUser();
      if (user == null)
        return;
      this.cond.MarkAsRejected(this.cond.DateRejected, user);
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
      this.dateCleared.Visible = this.chkCleared.Checked && this.canEditSellCondition;
      this.txtDateCleared.Visible = this.chkCleared.Checked && !this.canEditSellCondition;
      this.txtClearedBy.Visible = this.chkCleared.Checked;
      this.btnClearedBy.Visible = this.chkCleared.Checked && this.canEditSellCondition;
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

    private void loadWaivedFields()
    {
      this.chkWaived.Checked = this.cond.Waived;
      if (this.chkWaived.Checked)
      {
        this.dateWaived.Value = this.cond.DateWaived;
        this.txtDateWaived.Text = this.cond.DateWaived.ToString(this.dateWaived.CustomFormat);
        this.txtWaivedBy.Text = this.cond.WaivedBy;
      }
      this.dateWaived.Visible = this.chkWaived.Checked && this.canEditSellCondition;
      this.txtDateWaived.Visible = this.chkWaived.Checked && !this.canEditSellCondition;
      this.txtWaivedBy.Visible = this.chkWaived.Checked;
      this.btnWaivedBy.Visible = this.chkWaived.Checked && this.canEditSellCondition;
    }

    private void chkWaived_Click(object sender, EventArgs e)
    {
      if (this.chkWaived.Checked)
        this.cond.MarkAsWaived(DateTime.Now, Session.UserID);
      else
        this.cond.UnmarkAsWaived();
    }

    private void dateWaived_ValueChanged(object sender, EventArgs e)
    {
      if (!(this.dateWaived.Value != this.cond.DateWaived))
        return;
      this.cond.MarkAsWaived(this.dateWaived.Value, this.txtWaivedBy.Text);
    }

    private void btnWaivedBy_Click(object sender, EventArgs e)
    {
      string user = this.selectUser();
      if (user == null)
        return;
      this.cond.MarkAsWaived(this.cond.DateWaived, user);
    }

    private void loadCommentList()
    {
      this.commentCollection.LoadComments(this.loanDataMgr, this.cond.Comments);
    }

    private void loadConditionCodeField()
    {
      if (!string.IsNullOrEmpty(this.cond.ConditionCode))
        this.txtConditionCode.Text = this.cond.ConditionCode;
      else
        this.txtConditionCode.Text = "";
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
      int roleID = RoleInfo.Others.ID;
      int[] usersAssignedRoles = this.loanDataMgr.AccessRules.GetUsersAssignedRoles();
      if (usersAssignedRoles.Length != 0)
        roleID = usersAssignedRoles[0];
      foreach (RoleInfo allRole in this.loanDataMgr.SystemConfiguration.AllRoles)
      {
        if (allRole.RoleID == this.cond.ForRoleID)
          roleID = allRole.RoleID;
      }
      string str = (string) null;
      using (UserSelectionDialog userSelectionDialog = new UserSelectionDialog(this.loanDataMgr, roleID))
      {
        if (userSelectionDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
          str = userSelectionDialog.User.Userid;
      }
      return str;
    }

    private void applySecurity()
    {
      this.cboTitle.Visible = this.canEditSellCondition;
      this.txtTitle.Visible = !this.canEditSellCondition;
      this.txtDescription.ReadOnly = !this.canEditSellCondition;
      this.cboBorrower.Visible = this.canEditSellCondition;
      this.txtBorrower.Visible = !this.canEditSellCondition;
      this.cboCategory.Visible = this.canEditSellCondition;
      this.txtCategory.Visible = !this.canEditSellCondition;
      this.cboPriorTo.Visible = this.canEditSellCondition;
      this.txtPriorTo.Visible = !this.canEditSellCondition;
      this.cboOwner.Visible = this.canEditSellCondition;
      this.txtOwner.Visible = !this.canEditSellCondition;
      this.chkAllowToClear.Enabled = this.canEditSellCondition;
      this.chkInternal.Enabled = this.canEditSellCondition;
      this.chkExternal.Enabled = this.canEditSellCondition;
      this.chkFulfilled.Enabled = this.canEditSellCondition;
      this.chkReceived.Enabled = this.canEditSellCondition;
      this.chkReviewed.Enabled = this.canEditSellCondition;
      this.chkRejected.Enabled = this.canEditSellCondition;
      this.chkCleared.Enabled = this.canEditSellCondition;
      this.chkWaived.Enabled = this.canEditSellCondition;
      this.commentCollection.CanAddComment = this.canEditSellCondition;
      this.commentCollection.CanDeleteComment = this.canEditSellCondition;
      this.btnAddDocument.Visible = this.rights.CanAddSellConditionDocuments;
      this.btnRemoveDocument.Visible = this.rights.CanRemoveSellConditionDocuments;
      this.separator.Visible = this.rights.CanRequestDocuments || this.rights.CanRequestServices;
      this.btnRequestDocument.Visible = this.rights.CanRequestDocuments || this.rights.CanRequestServices;
      this.txtRequestedFrom.ReadOnly = !this.canEditSellCondition;
      this.txtDaysDue.ReadOnly = !this.canEditSellCondition;
      this.chkRequested.Enabled = this.canEditSellCondition;
      this.chkRerequested.Enabled = this.canEditSellCondition;
      this.commentCollection.CanDeliverComment = true;
      this.txtConditionCode.ReadOnly = true;
      if (!Session.IsBrokerEdition())
        return;
      this.chkInternal.Visible = false;
      this.chkExternal.Left = this.chkInternal.Left;
      this.chkExternal.Text = "In Mortgage Loan Commitment Letter";
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
      Tracing.Log(SellDetailsDialog.sw, TraceLevel.Verbose, nameof (SellDetailsDialog), "Checking InvokeRequired For LogRecordChanged");
      if (this.InvokeRequired)
      {
        LogRecordEventHandler method = new LogRecordEventHandler(this.logRecordChanged);
        Tracing.Log(SellDetailsDialog.sw, TraceLevel.Verbose, nameof (SellDetailsDialog), "Calling BeginInvoke For LogRecordChanged");
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
      Tracing.Log(SellDetailsDialog.sw, TraceLevel.Verbose, nameof (SellDetailsDialog), "Checking InvokeRequired For LogRecordRemoved");
      if (this.InvokeRequired)
      {
        LogRecordEventHandler method = new LogRecordEventHandler(this.logRecordRemoved);
        Tracing.Log(SellDetailsDialog.sw, TraceLevel.Verbose, nameof (SellDetailsDialog), "Calling BeginInvoke For LogRecordRemoved");
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
      this.cond = (SellConditionLog) this.loanDataMgr.LoanData.GetLogList().GetRecordByID(this.cond.Guid);
      this.loadConditionDetails();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      if (sender is LoanDataMgr)
        this.AutoValidate = AutoValidate.Disable;
      this.Close();
    }

    private void SellDetailsDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      Session.Application.DisplayHelp(this.helpLink.HelpTag);
    }

    private void SellDetailsDialog_Activated(object sender, EventArgs e)
    {
      if (!this.refreshDocuments)
        return;
      this.loadDocumentList(false);
      this.showDocumentFiles();
      this.refreshDocuments = false;
    }

    private void SellDetailsDialog_FormClosing(object sender, FormClosingEventArgs e)
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
      this.btnWaivedBy = new StandardIconButton();
      this.btnClearedBy = new StandardIconButton();
      this.btnRejectedBy = new StandardIconButton();
      this.btnReviewedBy = new StandardIconButton();
      this.btnReceivedBy = new StandardIconButton();
      this.btnRerequestedBy = new StandardIconButton();
      this.btnRequestedBy = new StandardIconButton();
      this.btnFulfilledBy = new StandardIconButton();
      this.pnlLeft = new Panel();
      this.gcTracking = new GroupContainer();
      this.tabTracking = new TabControl();
      this.pageStatus = new TabPage();
      this.txtFulfilledBy = new TextBox();
      this.dateFulfilled = new DateTimePicker();
      this.txtDateFulfilled = new TextBox();
      this.chkFulfilled = new CheckBox();
      this.txtRerequestedBy = new TextBox();
      this.dateRerequested = new DateTimePicker();
      this.txtDateRerequested = new TextBox();
      this.chkRerequested = new CheckBox();
      this.txtRequestedBy = new TextBox();
      this.dateRequested = new DateTimePicker();
      this.txtDateRequested = new TextBox();
      this.chkRequested = new CheckBox();
      this.txtRequestedFrom = new TextBox();
      this.lblRequestedFrom = new Label();
      this.txtDateDue = new TextBox();
      this.txtDaysDue = new TextBox();
      this.lblDaysDue = new Label();
      this.txtWaivedBy = new TextBox();
      this.dateWaived = new DateTimePicker();
      this.txtDateWaived = new TextBox();
      this.chkWaived = new CheckBox();
      this.txtClearedBy = new TextBox();
      this.dateCleared = new DateTimePicker();
      this.txtDateCleared = new TextBox();
      this.chkCleared = new CheckBox();
      this.txtRejectedBy = new TextBox();
      this.dateRejected = new DateTimePicker();
      this.txtDateRejected = new TextBox();
      this.chkRejected = new CheckBox();
      this.txtReviewedBy = new TextBox();
      this.dateReviewed = new DateTimePicker();
      this.txtDateReviewed = new TextBox();
      this.chkReviewed = new CheckBox();
      this.txtReceivedBy = new TextBox();
      this.dateReceived = new DateTimePicker();
      this.txtDateReceived = new TextBox();
      this.chkReceived = new CheckBox();
      this.txtAddedBy = new TextBox();
      this.txtAddedDate = new TextBox();
      this.chkAdded = new CheckBox();
      this.pageComments = new TabPage();
      this.commentCollection = new CommentCollectionControl();
      this.csDetails = new CollapsibleSplitter();
      this.gcDetails = new GroupContainer();
      this.pnlDetails = new Panel();
      this.txtConditionCode = new TextBox();
      this.lblconditioncode = new Label();
      this.lblPrint = new Label();
      this.chkExternal = new CheckBox();
      this.chkInternal = new CheckBox();
      this.chkAllowToClear = new CheckBox();
      this.cboOwner = new ComboBox();
      this.txtOwner = new TextBox();
      this.lblOwner = new Label();
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
      this.pnlClose = new Panel();
      this.helpLink = new EMHelpLink();
      this.btnClose = new Button();
      this.csLeft = new CollapsibleSplitter();
      this.pnlRight.SuspendLayout();
      this.pnlViewer.SuspendLayout();
      this.gcDocuments.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      ((ISupportInitialize) this.btnRemoveDocument).BeginInit();
      ((ISupportInitialize) this.btnEditDocument).BeginInit();
      ((ISupportInitialize) this.btnAddDocument).BeginInit();
      ((ISupportInitialize) this.btnWaivedBy).BeginInit();
      ((ISupportInitialize) this.btnClearedBy).BeginInit();
      ((ISupportInitialize) this.btnRejectedBy).BeginInit();
      ((ISupportInitialize) this.btnReviewedBy).BeginInit();
      ((ISupportInitialize) this.btnReceivedBy).BeginInit();
      ((ISupportInitialize) this.btnRerequestedBy).BeginInit();
      ((ISupportInitialize) this.btnRequestedBy).BeginInit();
      ((ISupportInitialize) this.btnFulfilledBy).BeginInit();
      this.pnlLeft.SuspendLayout();
      this.gcTracking.SuspendLayout();
      this.tabTracking.SuspendLayout();
      this.pageStatus.SuspendLayout();
      this.pageComments.SuspendLayout();
      this.gcDetails.SuspendLayout();
      this.pnlDetails.SuspendLayout();
      this.pnlClose.SuspendLayout();
      this.SuspendLayout();
      this.pnlRight.Controls.Add((Control) this.pnlViewer);
      this.pnlRight.Controls.Add((Control) this.csDocuments);
      this.pnlRight.Controls.Add((Control) this.gcDocuments);
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(339, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(435, 617);
      this.pnlRight.TabIndex = 59;
      this.pnlViewer.Controls.Add((Control) this.fileViewer);
      this.pnlViewer.Dock = DockStyle.Fill;
      this.pnlViewer.Location = new Point(0, (int) sbyte.MaxValue);
      this.pnlViewer.Name = "pnlViewer";
      this.pnlViewer.Size = new Size(435, 490);
      this.pnlViewer.TabIndex = 66;
      this.fileViewer.Dock = DockStyle.Fill;
      this.fileViewer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.fileViewer.Location = new Point(1, 1);
      this.fileViewer.Name = "fileViewer";
      this.fileViewer.Size = new Size(433, 488);
      this.fileViewer.TabIndex = 68;
      this.csDocuments.AnimationDelay = 20;
      this.csDocuments.AnimationStep = 20;
      this.csDocuments.BorderStyle3D = Border3DStyle.Flat;
      this.csDocuments.ControlToHide = (Control) this.gcDocuments;
      this.csDocuments.Dock = DockStyle.Top;
      this.csDocuments.ExpandParentForm = false;
      this.csDocuments.Location = new Point(0, 120);
      this.csDocuments.Name = "csFiles";
      this.csDocuments.TabIndex = 65;
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
      this.gcDocuments.TabIndex = 60;
      this.gcDocuments.Text = "Supporting Documents";
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnRequestDocument);
      this.pnlToolbar.Controls.Add((Control) this.separator);
      this.pnlToolbar.Controls.Add((Control) this.btnRemoveDocument);
      this.pnlToolbar.Controls.Add((Control) this.btnEditDocument);
      this.pnlToolbar.Controls.Add((Control) this.btnAddDocument);
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(275, 2);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(160, 22);
      this.pnlToolbar.TabIndex = 61;
      this.btnRequestDocument.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRequestDocument.Location = new Point(96, 0);
      this.btnRequestDocument.Margin = new Padding(0);
      this.btnRequestDocument.Name = "btnRequestDocument";
      this.btnRequestDocument.Size = new Size(64, 22);
      this.btnRequestDocument.TabIndex = 64;
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
      this.separator.TabIndex = 62;
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
      this.gvDocuments.TabIndex = 64;
      this.gvDocuments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvDocuments.BeforeSelectedIndexCommitted += new CancelEventHandler(this.gvDocuments_BeforeSelectedIndexCommitted);
      this.gvDocuments.SelectedIndexCommitted += new EventHandler(this.gvDocuments_SelectedIndexCommitted);
      this.gvDocuments.ItemDoubleClick += new GVItemEventHandler(this.gvDocuments_ItemDoubleClick);
      this.btnWaivedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnWaivedBy.BackColor = Color.Transparent;
      this.btnWaivedBy.Location = new Point(277, 250);
      this.btnWaivedBy.MouseDownImage = (Image) null;
      this.btnWaivedBy.Name = "btnWaivedBy";
      this.btnWaivedBy.Size = new Size(16, 16);
      this.btnWaivedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnWaivedBy.TabIndex = 50;
      this.btnWaivedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnWaivedBy, "Select User");
      this.btnWaivedBy.Click += new EventHandler(this.btnWaivedBy_Click);
      this.btnClearedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClearedBy.BackColor = Color.Transparent;
      this.btnClearedBy.Location = new Point(279, 224);
      this.btnClearedBy.MouseDownImage = (Image) null;
      this.btnClearedBy.Name = "btnClearedBy";
      this.btnClearedBy.Size = new Size(16, 16);
      this.btnClearedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnClearedBy.TabIndex = 46;
      this.btnClearedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnClearedBy, "Select User");
      this.btnClearedBy.Click += new EventHandler(this.btnClearedBy_Click);
      this.btnRejectedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRejectedBy.BackColor = Color.Transparent;
      this.btnRejectedBy.Location = new Point(280, 203);
      this.btnRejectedBy.MouseDownImage = (Image) null;
      this.btnRejectedBy.Name = "btnRejectedBy";
      this.btnRejectedBy.Size = new Size(16, 16);
      this.btnRejectedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnRejectedBy.TabIndex = 42;
      this.btnRejectedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnRejectedBy, "Select User");
      this.btnRejectedBy.Click += new EventHandler(this.btnRejectedBy_Click);
      this.btnReviewedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReviewedBy.BackColor = Color.Transparent;
      this.btnReviewedBy.Location = new Point(280, 176);
      this.btnReviewedBy.MouseDownImage = (Image) null;
      this.btnReviewedBy.Name = "btnReviewedBy";
      this.btnReviewedBy.Size = new Size(16, 16);
      this.btnReviewedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnReviewedBy.TabIndex = 38;
      this.btnReviewedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnReviewedBy, "Select User");
      this.btnReviewedBy.Click += new EventHandler(this.btnReviewedBy_Click);
      this.btnReceivedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReceivedBy.BackColor = Color.Transparent;
      this.btnReceivedBy.Location = new Point(280, 152);
      this.btnReceivedBy.MouseDownImage = (Image) null;
      this.btnReceivedBy.Name = "btnReceivedBy";
      this.btnReceivedBy.Size = new Size(16, 16);
      this.btnReceivedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnReceivedBy.TabIndex = 34;
      this.btnReceivedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnReceivedBy, "Select User");
      this.btnReceivedBy.Click += new EventHandler(this.btnReceivedBy_Click);
      this.btnRerequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRerequestedBy.BackColor = Color.Transparent;
      this.btnRerequestedBy.Location = new Point(280, 132);
      this.btnRerequestedBy.MouseDownImage = (Image) null;
      this.btnRerequestedBy.Name = "btnRerequestedBy";
      this.btnRerequestedBy.Size = new Size(16, 16);
      this.btnRerequestedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnRerequestedBy.TabIndex = 70;
      this.btnRerequestedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnRerequestedBy, "Select User");
      this.btnRerequestedBy.Click += new EventHandler(this.btnRerequestedBy_Click);
      this.btnRequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRequestedBy.BackColor = Color.Transparent;
      this.btnRequestedBy.Location = new Point(280, 107);
      this.btnRequestedBy.MouseDownImage = (Image) null;
      this.btnRequestedBy.Name = "btnRequestedBy";
      this.btnRequestedBy.Size = new Size(16, 16);
      this.btnRequestedBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnRequestedBy.TabIndex = 65;
      this.btnRequestedBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnRequestedBy, "Select User");
      this.btnRequestedBy.Click += new EventHandler(this.btnRequestedBy_Click);
      this.btnFulfilledBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnFulfilledBy.BackColor = Color.Transparent;
      this.btnFulfilledBy.Location = new Point(280, 81);
      this.btnFulfilledBy.MouseDownImage = (Image) null;
      this.btnFulfilledBy.Name = "btnFulfilledBy";
      this.btnFulfilledBy.Size = new Size(16, 16);
      this.btnFulfilledBy.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnFulfilledBy.TabIndex = 73;
      this.btnFulfilledBy.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnFulfilledBy, "Select User");
      this.btnFulfilledBy.Click += new EventHandler(this.btnFulfilledBy_Click);
      this.pnlLeft.BackColor = Color.WhiteSmoke;
      this.pnlLeft.Controls.Add((Control) this.gcTracking);
      this.pnlLeft.Controls.Add((Control) this.csDetails);
      this.pnlLeft.Controls.Add((Control) this.gcDetails);
      this.pnlLeft.Dock = DockStyle.Left;
      this.pnlLeft.Location = new Point(0, 0);
      this.pnlLeft.Name = "pnlLeft";
      this.pnlLeft.Size = new Size(332, 617);
      this.pnlLeft.TabIndex = 0;
      this.gcTracking.Controls.Add((Control) this.tabTracking);
      this.gcTracking.Dock = DockStyle.Fill;
      this.gcTracking.HeaderForeColor = SystemColors.ControlText;
      this.gcTracking.Location = new Point(0, 349);
      this.gcTracking.Name = "gcTracking";
      this.gcTracking.Padding = new Padding(3, 3, 2, 2);
      this.gcTracking.Size = new Size(332, 268);
      this.gcTracking.TabIndex = 28;
      this.gcTracking.Text = "Tracking";
      this.tabTracking.Controls.Add((Control) this.pageStatus);
      this.tabTracking.Controls.Add((Control) this.pageComments);
      this.tabTracking.Dock = DockStyle.Fill;
      this.tabTracking.Location = new Point(4, 29);
      this.tabTracking.Name = "tabTracking";
      this.tabTracking.SelectedIndex = 0;
      this.tabTracking.Size = new Size(325, 236);
      this.tabTracking.TabIndex = 29;
      this.pageStatus.AutoScroll = true;
      this.pageStatus.AutoScrollMargin = new Size(8, 8);
      this.pageStatus.BackColor = Color.WhiteSmoke;
      this.pageStatus.Controls.Add((Control) this.btnFulfilledBy);
      this.pageStatus.Controls.Add((Control) this.txtFulfilledBy);
      this.pageStatus.Controls.Add((Control) this.dateFulfilled);
      this.pageStatus.Controls.Add((Control) this.txtDateFulfilled);
      this.pageStatus.Controls.Add((Control) this.chkFulfilled);
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
      this.pageStatus.Controls.Add((Control) this.btnWaivedBy);
      this.pageStatus.Controls.Add((Control) this.txtWaivedBy);
      this.pageStatus.Controls.Add((Control) this.dateWaived);
      this.pageStatus.Controls.Add((Control) this.txtDateWaived);
      this.pageStatus.Controls.Add((Control) this.chkWaived);
      this.pageStatus.Controls.Add((Control) this.btnClearedBy);
      this.pageStatus.Controls.Add((Control) this.txtClearedBy);
      this.pageStatus.Controls.Add((Control) this.dateCleared);
      this.pageStatus.Controls.Add((Control) this.txtDateCleared);
      this.pageStatus.Controls.Add((Control) this.chkCleared);
      this.pageStatus.Controls.Add((Control) this.btnRejectedBy);
      this.pageStatus.Controls.Add((Control) this.txtRejectedBy);
      this.pageStatus.Controls.Add((Control) this.dateRejected);
      this.pageStatus.Controls.Add((Control) this.txtDateRejected);
      this.pageStatus.Controls.Add((Control) this.chkRejected);
      this.pageStatus.Controls.Add((Control) this.btnReviewedBy);
      this.pageStatus.Controls.Add((Control) this.txtReviewedBy);
      this.pageStatus.Controls.Add((Control) this.dateReviewed);
      this.pageStatus.Controls.Add((Control) this.txtDateReviewed);
      this.pageStatus.Controls.Add((Control) this.chkReviewed);
      this.pageStatus.Controls.Add((Control) this.btnReceivedBy);
      this.pageStatus.Controls.Add((Control) this.txtReceivedBy);
      this.pageStatus.Controls.Add((Control) this.dateReceived);
      this.pageStatus.Controls.Add((Control) this.txtDateReceived);
      this.pageStatus.Controls.Add((Control) this.chkReceived);
      this.pageStatus.Controls.Add((Control) this.txtAddedBy);
      this.pageStatus.Controls.Add((Control) this.txtAddedDate);
      this.pageStatus.Controls.Add((Control) this.chkAdded);
      this.pageStatus.Location = new Point(4, 23);
      this.pageStatus.Name = "pageStatus";
      this.pageStatus.Padding = new Padding(0, 2, 2, 2);
      this.pageStatus.Size = new Size(317, 209);
      this.pageStatus.TabIndex = 0;
      this.pageStatus.Text = "Status";
      this.pageStatus.UseVisualStyleBackColor = true;
      this.txtFulfilledBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtFulfilledBy.Location = new Point(220, 79);
      this.txtFulfilledBy.Name = "txtFulfilledBy";
      this.txtFulfilledBy.ReadOnly = true;
      this.txtFulfilledBy.Size = new Size(59, 20);
      this.txtFulfilledBy.TabIndex = 40;
      this.dateFulfilled.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateFulfilled.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateFulfilled.CalendarTitleForeColor = Color.White;
      this.dateFulfilled.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateFulfilled.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateFulfilled.Format = DateTimePickerFormat.Custom;
      this.dateFulfilled.Location = new Point(97, 79);
      this.dateFulfilled.Name = "dateFulfilled";
      this.dateFulfilled.Size = new Size(117, 20);
      this.dateFulfilled.TabIndex = 39;
      this.dateFulfilled.ValueChanged += new EventHandler(this.dateFulfilled_ValueChanged);
      this.txtDateFulfilled.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateFulfilled.Location = new Point(100, 79);
      this.txtDateFulfilled.Name = "txtDateFulfilled";
      this.txtDateFulfilled.ReadOnly = true;
      this.txtDateFulfilled.Size = new Size(114, 20);
      this.txtDateFulfilled.TabIndex = 76;
      this.chkFulfilled.AutoSize = true;
      this.chkFulfilled.Location = new Point(8, 83);
      this.chkFulfilled.Name = "chkFulfilled";
      this.chkFulfilled.Size = new Size(62, 18);
      this.chkFulfilled.TabIndex = 38;
      this.chkFulfilled.Text = "Fulfilled";
      this.chkFulfilled.UseVisualStyleBackColor = true;
      this.chkFulfilled.Click += new EventHandler(this.chkFulfilled_Click);
      this.txtRerequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtRerequestedBy.Location = new Point(220, 129);
      this.txtRerequestedBy.Name = "txtRerequestedBy";
      this.txtRerequestedBy.ReadOnly = true;
      this.txtRerequestedBy.Size = new Size(59, 20);
      this.txtRerequestedBy.TabIndex = 46;
      this.dateRerequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateRerequested.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateRerequested.CalendarTitleForeColor = Color.White;
      this.dateRerequested.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateRerequested.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateRerequested.Format = DateTimePickerFormat.Custom;
      this.dateRerequested.Location = new Point(97, 129);
      this.dateRerequested.Name = "dateRerequested";
      this.dateRerequested.Size = new Size(117, 20);
      this.dateRerequested.TabIndex = 45;
      this.dateRerequested.ValueChanged += new EventHandler(this.dateRerequested_ValueChanged);
      this.txtDateRerequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateRerequested.Location = new Point(100, 129);
      this.txtDateRerequested.Name = "txtDateRerequested";
      this.txtDateRerequested.ReadOnly = true;
      this.txtDateRerequested.Size = new Size(114, 20);
      this.txtDateRerequested.TabIndex = 71;
      this.chkRerequested.AutoSize = true;
      this.chkRerequested.Location = new Point(8, 132);
      this.chkRerequested.Name = "chkRerequested";
      this.chkRerequested.Size = new Size(92, 18);
      this.chkRerequested.TabIndex = 44;
      this.chkRerequested.Text = "Re-requested";
      this.chkRerequested.UseVisualStyleBackColor = true;
      this.chkRerequested.Click += new EventHandler(this.chkRerequested_Click);
      this.txtRequestedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtRequestedBy.Location = new Point(220, 104);
      this.txtRequestedBy.Name = "txtRequestedBy";
      this.txtRequestedBy.ReadOnly = true;
      this.txtRequestedBy.Size = new Size(59, 20);
      this.txtRequestedBy.TabIndex = 43;
      this.dateRequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateRequested.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateRequested.CalendarTitleForeColor = Color.White;
      this.dateRequested.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateRequested.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateRequested.Format = DateTimePickerFormat.Custom;
      this.dateRequested.Location = new Point(97, 104);
      this.dateRequested.Name = "dateRequested";
      this.dateRequested.Size = new Size(117, 20);
      this.dateRequested.TabIndex = 42;
      this.dateRequested.ValueChanged += new EventHandler(this.dateRequested_ValueChanged);
      this.txtDateRequested.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateRequested.Location = new Point(100, 104);
      this.txtDateRequested.Name = "txtDateRequested";
      this.txtDateRequested.ReadOnly = true;
      this.txtDateRequested.Size = new Size(114, 20);
      this.txtDateRequested.TabIndex = 66;
      this.chkRequested.AutoSize = true;
      this.chkRequested.Location = new Point(8, 107);
      this.chkRequested.Name = "chkRequested";
      this.chkRequested.Size = new Size(78, 18);
      this.chkRequested.TabIndex = 41;
      this.chkRequested.Text = "Requested";
      this.chkRequested.UseVisualStyleBackColor = true;
      this.chkRequested.Click += new EventHandler(this.chkRequested_Click);
      this.txtRequestedFrom.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtRequestedFrom.Location = new Point(97, 32);
      this.txtRequestedFrom.Name = "txtRequestedFrom";
      this.txtRequestedFrom.Size = new Size(182, 20);
      this.txtRequestedFrom.TabIndex = 34;
      this.txtRequestedFrom.Validated += new EventHandler(this.txtRequestedFrom_Validated);
      this.lblRequestedFrom.AutoSize = true;
      this.lblRequestedFrom.Location = new Point(5, 32);
      this.lblRequestedFrom.Name = "lblRequestedFrom";
      this.lblRequestedFrom.Size = new Size(86, 14);
      this.lblRequestedFrom.TabIndex = 33;
      this.lblRequestedFrom.Text = "Requested From";
      this.txtDateDue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateDue.Location = new Point(153, 7);
      this.txtDateDue.Name = "txtDateDue";
      this.txtDateDue.ReadOnly = true;
      this.txtDateDue.Size = new Size(126, 20);
      this.txtDateDue.TabIndex = 32;
      this.txtDaysDue.Location = new Point(97, 7);
      this.txtDaysDue.Name = "txtDaysDue";
      this.txtDaysDue.Size = new Size(52, 20);
      this.txtDaysDue.TabIndex = 31;
      this.txtDaysDue.TextAlign = HorizontalAlignment.Right;
      this.txtDaysDue.Validating += new CancelEventHandler(this.txtDaysDue_Validating);
      this.txtDaysDue.Validated += new EventHandler(this.txtDaysDue_Validated);
      this.lblDaysDue.AutoSize = true;
      this.lblDaysDue.Location = new Point(5, 9);
      this.lblDaysDue.Name = "lblDaysDue";
      this.lblDaysDue.Size = new Size(86, 14);
      this.lblDaysDue.TabIndex = 30;
      this.lblDaysDue.Text = "Days to Receive";
      this.txtWaivedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtWaivedBy.Location = new Point(220, 248);
      this.txtWaivedBy.Name = "txtWaivedBy";
      this.txtWaivedBy.ReadOnly = true;
      this.txtWaivedBy.Size = new Size(59, 20);
      this.txtWaivedBy.TabIndex = 61;
      this.dateWaived.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateWaived.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateWaived.CalendarTitleForeColor = Color.White;
      this.dateWaived.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateWaived.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateWaived.Format = DateTimePickerFormat.Custom;
      this.dateWaived.Location = new Point(97, 248);
      this.dateWaived.Name = "dateWaived";
      this.dateWaived.Size = new Size(117, 20);
      this.dateWaived.TabIndex = 60;
      this.dateWaived.ValueChanged += new EventHandler(this.dateWaived_ValueChanged);
      this.txtDateWaived.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateWaived.Location = new Point(100, 248);
      this.txtDateWaived.Name = "txtDateWaived";
      this.txtDateWaived.ReadOnly = true;
      this.txtDateWaived.Size = new Size(114, 20);
      this.txtDateWaived.TabIndex = 55;
      this.chkWaived.AutoSize = true;
      this.chkWaived.Location = new Point(8, 251);
      this.chkWaived.Name = "chkWaived";
      this.chkWaived.Size = new Size(62, 18);
      this.chkWaived.TabIndex = 59;
      this.chkWaived.Text = "Waived";
      this.chkWaived.UseVisualStyleBackColor = true;
      this.chkWaived.Click += new EventHandler(this.chkWaived_Click);
      this.txtClearedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtClearedBy.Location = new Point(220, 224);
      this.txtClearedBy.Name = "txtClearedBy";
      this.txtClearedBy.ReadOnly = true;
      this.txtClearedBy.Size = new Size(59, 20);
      this.txtClearedBy.TabIndex = 58;
      this.dateCleared.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateCleared.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateCleared.CalendarTitleForeColor = Color.White;
      this.dateCleared.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateCleared.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateCleared.Format = DateTimePickerFormat.Custom;
      this.dateCleared.Location = new Point(97, 224);
      this.dateCleared.Name = "dateCleared";
      this.dateCleared.Size = new Size(117, 20);
      this.dateCleared.TabIndex = 57;
      this.dateCleared.ValueChanged += new EventHandler(this.dateCleared_ValueChanged);
      this.txtDateCleared.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateCleared.Location = new Point(100, 224);
      this.txtDateCleared.Name = "txtDateCleared";
      this.txtDateCleared.ReadOnly = true;
      this.txtDateCleared.Size = new Size(114, 20);
      this.txtDateCleared.TabIndex = 51;
      this.chkCleared.AutoSize = true;
      this.chkCleared.Location = new Point(8, 227);
      this.chkCleared.Name = "chkCleared";
      this.chkCleared.Size = new Size(63, 18);
      this.chkCleared.TabIndex = 56;
      this.chkCleared.Text = "Cleared";
      this.chkCleared.UseVisualStyleBackColor = true;
      this.chkCleared.Click += new EventHandler(this.chkCleared_Click);
      this.txtRejectedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtRejectedBy.Location = new Point(220, 200);
      this.txtRejectedBy.Name = "txtRejectedBy";
      this.txtRejectedBy.ReadOnly = true;
      this.txtRejectedBy.Size = new Size(59, 20);
      this.txtRejectedBy.TabIndex = 55;
      this.dateRejected.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateRejected.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateRejected.CalendarTitleForeColor = Color.White;
      this.dateRejected.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateRejected.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateRejected.Format = DateTimePickerFormat.Custom;
      this.dateRejected.Location = new Point(97, 200);
      this.dateRejected.Name = "dateRejected";
      this.dateRejected.Size = new Size(117, 20);
      this.dateRejected.TabIndex = 54;
      this.dateRejected.ValueChanged += new EventHandler(this.dateRejected_ValueChanged);
      this.txtDateRejected.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateRejected.Location = new Point(100, 200);
      this.txtDateRejected.Name = "txtDateRejected";
      this.txtDateRejected.ReadOnly = true;
      this.txtDateRejected.Size = new Size(114, 20);
      this.txtDateRejected.TabIndex = 47;
      this.chkRejected.AutoSize = true;
      this.chkRejected.Location = new Point(8, 203);
      this.chkRejected.Name = "chkRejected";
      this.chkRejected.Size = new Size(68, 18);
      this.chkRejected.TabIndex = 53;
      this.chkRejected.Text = "Rejected";
      this.chkRejected.UseVisualStyleBackColor = true;
      this.chkRejected.Click += new EventHandler(this.chkRejected_Click);
      this.txtReviewedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtReviewedBy.Location = new Point(220, 176);
      this.txtReviewedBy.Name = "txtReviewedBy";
      this.txtReviewedBy.ReadOnly = true;
      this.txtReviewedBy.Size = new Size(59, 20);
      this.txtReviewedBy.TabIndex = 52;
      this.dateReviewed.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateReviewed.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateReviewed.CalendarTitleForeColor = Color.White;
      this.dateReviewed.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateReviewed.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateReviewed.Format = DateTimePickerFormat.Custom;
      this.dateReviewed.Location = new Point(97, 176);
      this.dateReviewed.Name = "dateReviewed";
      this.dateReviewed.Size = new Size(117, 20);
      this.dateReviewed.TabIndex = 51;
      this.dateReviewed.ValueChanged += new EventHandler(this.dateReviewed_ValueChanged);
      this.txtDateReviewed.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateReviewed.Location = new Point(100, 176);
      this.txtDateReviewed.Name = "txtDateReviewed";
      this.txtDateReviewed.ReadOnly = true;
      this.txtDateReviewed.Size = new Size(114, 20);
      this.txtDateReviewed.TabIndex = 43;
      this.chkReviewed.AutoSize = true;
      this.chkReviewed.Location = new Point(8, 179);
      this.chkReviewed.Name = "chkReviewed";
      this.chkReviewed.Size = new Size(75, 18);
      this.chkReviewed.TabIndex = 50;
      this.chkReviewed.Text = "Reviewed";
      this.chkReviewed.UseVisualStyleBackColor = true;
      this.chkReviewed.Click += new EventHandler(this.chkReviewed_Click);
      this.txtReceivedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtReceivedBy.Location = new Point(220, 152);
      this.txtReceivedBy.Name = "txtReceivedBy";
      this.txtReceivedBy.ReadOnly = true;
      this.txtReceivedBy.Size = new Size(59, 20);
      this.txtReceivedBy.TabIndex = 49;
      this.dateReceived.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dateReceived.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateReceived.CalendarTitleForeColor = Color.White;
      this.dateReceived.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateReceived.CustomFormat = "MM/dd/yy hh:mm tt";
      this.dateReceived.Format = DateTimePickerFormat.Custom;
      this.dateReceived.Location = new Point(97, 152);
      this.dateReceived.Name = "dateReceived";
      this.dateReceived.Size = new Size(117, 20);
      this.dateReceived.TabIndex = 48;
      this.dateReceived.ValueChanged += new EventHandler(this.dateReceived_ValueChanged);
      this.txtDateReceived.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateReceived.Location = new Point(100, 152);
      this.txtDateReceived.Name = "txtDateReceived";
      this.txtDateReceived.ReadOnly = true;
      this.txtDateReceived.Size = new Size(114, 20);
      this.txtDateReceived.TabIndex = 39;
      this.chkReceived.AutoSize = true;
      this.chkReceived.Location = new Point(8, 155);
      this.chkReceived.Name = "chkReceived";
      this.chkReceived.Size = new Size(71, 18);
      this.chkReceived.TabIndex = 47;
      this.chkReceived.Text = "Received";
      this.chkReceived.UseVisualStyleBackColor = true;
      this.chkReceived.Click += new EventHandler(this.chkReceived_Click);
      this.txtAddedBy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtAddedBy.Location = new Point(220, 53);
      this.txtAddedBy.Name = "txtAddedBy";
      this.txtAddedBy.ReadOnly = true;
      this.txtAddedBy.Size = new Size(59, 20);
      this.txtAddedBy.TabIndex = 37;
      this.txtAddedDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtAddedDate.Location = new Point(97, 55);
      this.txtAddedDate.Name = "txtAddedDate";
      this.txtAddedDate.ReadOnly = true;
      this.txtAddedDate.Size = new Size(117, 20);
      this.txtAddedDate.TabIndex = 36;
      this.chkAdded.AutoSize = true;
      this.chkAdded.Checked = true;
      this.chkAdded.CheckState = CheckState.Checked;
      this.chkAdded.Enabled = false;
      this.chkAdded.Location = new Point(8, 58);
      this.chkAdded.Name = "chkAdded";
      this.chkAdded.Size = new Size(58, 18);
      this.chkAdded.TabIndex = 35;
      this.chkAdded.Text = "Added";
      this.chkAdded.UseVisualStyleBackColor = true;
      this.pageComments.BackColor = Color.WhiteSmoke;
      this.pageComments.Controls.Add((Control) this.commentCollection);
      this.pageComments.Location = new Point(4, 23);
      this.pageComments.Name = "pageComments";
      this.pageComments.Padding = new Padding(0, 2, 2, 2);
      this.pageComments.Size = new Size(317, 283);
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
      this.commentCollection.Size = new Size(315, 279);
      this.commentCollection.TabIndex = 57;
      this.csDetails.AnimationDelay = 20;
      this.csDetails.AnimationStep = 20;
      this.csDetails.BorderStyle3D = Border3DStyle.Flat;
      this.csDetails.ControlToHide = (Control) this.gcDetails;
      this.csDetails.Dock = DockStyle.Top;
      this.csDetails.ExpandParentForm = false;
      this.csDetails.Location = new Point(0, 342);
      this.csDetails.Name = "csFiles";
      this.csDetails.TabIndex = 27;
      this.csDetails.TabStop = false;
      this.csDetails.UseAnimations = false;
      this.csDetails.VisualStyle = VisualStyles.Encompass;
      this.gcDetails.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.gcDetails.Controls.Add((Control) this.pnlDetails);
      this.gcDetails.Dock = DockStyle.Top;
      this.gcDetails.HeaderForeColor = SystemColors.ControlText;
      this.gcDetails.Location = new Point(0, 0);
      this.gcDetails.Name = "gcDetails";
      this.gcDetails.Size = new Size(332, 342);
      this.gcDetails.TabIndex = 1;
      this.gcDetails.Text = "Details";
      this.pnlDetails.AutoScroll = true;
      this.pnlDetails.AutoScrollMargin = new Size(8, 8);
      this.pnlDetails.Controls.Add((Control) this.txtConditionCode);
      this.pnlDetails.Controls.Add((Control) this.lblconditioncode);
      this.pnlDetails.Controls.Add((Control) this.lblPrint);
      this.pnlDetails.Controls.Add((Control) this.chkExternal);
      this.pnlDetails.Controls.Add((Control) this.chkInternal);
      this.pnlDetails.Controls.Add((Control) this.chkAllowToClear);
      this.pnlDetails.Controls.Add((Control) this.cboOwner);
      this.pnlDetails.Controls.Add((Control) this.txtOwner);
      this.pnlDetails.Controls.Add((Control) this.lblOwner);
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
      this.pnlDetails.Size = new Size(330, 315);
      this.pnlDetails.TabIndex = 2;
      this.txtConditionCode.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtConditionCode.Location = new Point(84, 8);
      this.txtConditionCode.Name = "txtConditionCode";
      this.txtConditionCode.Size = new Size(236, 20);
      this.txtConditionCode.TabIndex = 70;
      this.lblconditioncode.AutoSize = true;
      this.lblconditioncode.Location = new Point(8, 6);
      this.lblconditioncode.MaximumSize = new Size(75, 0);
      this.lblconditioncode.Name = "lblconditioncode";
      this.lblconditioncode.Size = new Size(54, 28);
      this.lblconditioncode.TabIndex = 69;
      this.lblconditioncode.Text = "Condition Code";
      this.lblPrint.AutoSize = true;
      this.lblPrint.Location = new Point(8, 284);
      this.lblPrint.Name = "lblPrint";
      this.lblPrint.Size = new Size(28, 14);
      this.lblPrint.TabIndex = 24;
      this.lblPrint.Text = "Print";
      this.chkExternal.AutoSize = true;
      this.chkExternal.Location = new Point(156, 283);
      this.chkExternal.Name = "chkExternal";
      this.chkExternal.Size = new Size(73, 18);
      this.chkExternal.TabIndex = 26;
      this.chkExternal.Text = "Externally";
      this.chkExternal.UseVisualStyleBackColor = true;
      this.chkExternal.Click += new EventHandler(this.chkExternal_Click);
      this.chkInternal.AutoSize = true;
      this.chkInternal.Location = new Point(84, 283);
      this.chkInternal.Name = "chkInternal";
      this.chkInternal.Size = new Size(69, 18);
      this.chkInternal.TabIndex = 25;
      this.chkInternal.Text = "Internally";
      this.chkInternal.UseVisualStyleBackColor = true;
      this.chkInternal.Click += new EventHandler(this.chkInternal_Click);
      this.chkAllowToClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkAllowToClear.AutoSize = true;
      this.chkAllowToClear.Location = new Point(228, (int) byte.MaxValue);
      this.chkAllowToClear.Name = "chkAllowToClear";
      this.chkAllowToClear.Size = new Size(94, 18);
      this.chkAllowToClear.TabIndex = 23;
      this.chkAllowToClear.Text = "Allow to Clear";
      this.chkAllowToClear.UseVisualStyleBackColor = true;
      this.chkAllowToClear.Click += new EventHandler(this.chkAllowToClear_Click);
      this.cboOwner.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboOwner.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboOwner.FormattingEnabled = true;
      this.cboOwner.Location = new Point(84, 252);
      this.cboOwner.Name = "cboOwner";
      this.cboOwner.Size = new Size(136, 22);
      this.cboOwner.TabIndex = 21;
      this.cboOwner.SelectionChangeCommitted += new EventHandler(this.cboOwner_SelectionChangeCommitted);
      this.txtOwner.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtOwner.Location = new Point(84, 252);
      this.txtOwner.Name = "txtOwner";
      this.txtOwner.ReadOnly = true;
      this.txtOwner.Size = new Size(136, 20);
      this.txtOwner.TabIndex = 22;
      this.lblOwner.AutoSize = true;
      this.lblOwner.Location = new Point(8, 256);
      this.lblOwner.Name = "lblOwner";
      this.lblOwner.Size = new Size(41, 14);
      this.lblOwner.TabIndex = 20;
      this.lblOwner.Text = "Owner";
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
      this.cboPriorTo.Location = new Point(84, 224);
      this.cboPriorTo.Name = "cboPriorTo";
      this.cboPriorTo.Size = new Size(236, 22);
      this.cboPriorTo.TabIndex = 18;
      this.cboPriorTo.SelectionChangeCommitted += new EventHandler(this.cboPriorTo_SelectionChangeCommitted);
      this.txtPriorTo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtPriorTo.Location = new Point(84, 224);
      this.txtPriorTo.Name = "txtPriorTo";
      this.txtPriorTo.ReadOnly = true;
      this.txtPriorTo.Size = new Size(236, 20);
      this.txtPriorTo.TabIndex = 19;
      this.lblPriorTo.AutoSize = true;
      this.lblPriorTo.Location = new Point(8, 228);
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
      this.cboCategory.Location = new Point(84, 196);
      this.cboCategory.Name = "cboCategory";
      this.cboCategory.Size = new Size(236, 22);
      this.cboCategory.TabIndex = 15;
      this.cboCategory.SelectionChangeCommitted += new EventHandler(this.cboCategory_SelectionChangeCommitted);
      this.txtCategory.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtCategory.Location = new Point(84, 196);
      this.txtCategory.Name = "txtCategory";
      this.txtCategory.ReadOnly = true;
      this.txtCategory.Size = new Size(236, 20);
      this.txtCategory.TabIndex = 16;
      this.lblCategory.AutoSize = true;
      this.lblCategory.Location = new Point(8, 200);
      this.lblCategory.Name = "lblCategory";
      this.lblCategory.Size = new Size(51, 14);
      this.lblCategory.TabIndex = 14;
      this.lblCategory.Text = "Category";
      this.txtSource.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSource.Location = new Point(84, 168);
      this.txtSource.Name = "txtSource";
      this.txtSource.ReadOnly = true;
      this.txtSource.Size = new Size(236, 20);
      this.txtSource.TabIndex = 13;
      this.lblSource.AutoSize = true;
      this.lblSource.Location = new Point(8, 172);
      this.lblSource.Name = "lblSource";
      this.lblSource.Size = new Size(42, 14);
      this.lblSource.TabIndex = 12;
      this.lblSource.Text = "Source";
      this.cboBorrower.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboBorrower.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboBorrower.FormattingEnabled = true;
      this.cboBorrower.Location = new Point(84, 140);
      this.cboBorrower.Name = "cboBorrower";
      this.cboBorrower.Size = new Size(236, 22);
      this.cboBorrower.TabIndex = 10;
      this.cboBorrower.SelectionChangeCommitted += new EventHandler(this.cboBorrower_SelectionChangeCommitted);
      this.txtBorrower.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBorrower.Location = new Point(84, 140);
      this.txtBorrower.Name = "txtBorrower";
      this.txtBorrower.ReadOnly = true;
      this.txtBorrower.Size = new Size(236, 20);
      this.txtBorrower.TabIndex = 11;
      this.lblBorrower.AutoSize = true;
      this.lblBorrower.Location = new Point(8, 138);
      this.lblBorrower.MaximumSize = new Size(75, 0);
      this.lblBorrower.Name = "lblBorrower";
      this.lblBorrower.Size = new Size(73, 28);
      this.lblBorrower.TabIndex = 9;
      this.lblBorrower.Text = "For Borrower Pair";
      this.lnkViewMore.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lnkViewMore.AutoSize = true;
      this.lnkViewMore.Location = new Point(252, 120);
      this.lnkViewMore.Name = "lnkViewMore";
      this.lnkViewMore.Size = new Size(69, 14);
      this.lnkViewMore.TabIndex = 8;
      this.lnkViewMore.TabStop = true;
      this.lnkViewMore.Text = "View More >";
      this.lnkViewMore.TextAlign = ContentAlignment.MiddleRight;
      this.lnkViewMore.Click += new EventHandler(this.lnkViewMore_Click);
      this.txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDescription.Location = new Point(84, 64);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ScrollBars = ScrollBars.Vertical;
      this.txtDescription.Size = new Size(236, 52);
      this.txtDescription.TabIndex = 7;
      this.txtDescription.Validated += new EventHandler(this.txtDescription_Validated);
      this.lblDescription.AutoSize = true;
      this.lblDescription.Location = new Point(8, 68);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(61, 14);
      this.lblDescription.TabIndex = 6;
      this.lblDescription.Text = "Description";
      this.cboTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboTitle.FormattingEnabled = true;
      this.cboTitle.Location = new Point(84, 36);
      this.cboTitle.Name = "cboTitle";
      this.cboTitle.Size = new Size(236, 22);
      this.cboTitle.Sorted = true;
      this.cboTitle.TabIndex = 4;
      this.cboTitle.SelectionChangeCommitted += new EventHandler(this.cboTitle_SelectionChangeCommitted);
      this.cboTitle.Validating += new CancelEventHandler(this.cboTitle_Validating);
      this.cboTitle.Validated += new EventHandler(this.cboTitle_Validated);
      this.txtTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtTitle.Location = new Point(84, 36);
      this.txtTitle.Name = "txtTitle";
      this.txtTitle.ReadOnly = true;
      this.txtTitle.Size = new Size(236, 20);
      this.txtTitle.TabIndex = 5;
      this.lblTitle.AutoSize = true;
      this.lblTitle.Location = new Point(8, 40);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(34, 14);
      this.lblTitle.TabIndex = 3;
      this.lblTitle.Text = "Name";
      this.pnlClose.Controls.Add((Control) this.helpLink);
      this.pnlClose.Controls.Add((Control) this.btnClose);
      this.pnlClose.Dock = DockStyle.Bottom;
      this.pnlClose.Location = new Point(0, 617);
      this.pnlClose.Name = "pnlClose";
      this.pnlClose.Size = new Size(774, 40);
      this.pnlClose.TabIndex = 68;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "Condition Details";
      this.helpLink.Location = new Point(8, 11);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 18);
      this.helpLink.TabIndex = 70;
      this.helpLink.TabStop = false;
      this.btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.OK;
      this.btnClose.Location = new Point(691, 9);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 22);
      this.btnClose.TabIndex = 69;
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
      this.csLeft.TabIndex = 58;
      this.csLeft.TabStop = false;
      this.csLeft.UseAnimations = false;
      this.csLeft.VisualStyle = VisualStyles.Encompass;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(774, 657);
      this.Controls.Add((Control) this.pnlRight);
      this.Controls.Add((Control) this.csLeft);
      this.Controls.Add((Control) this.pnlLeft);
      this.Controls.Add((Control) this.pnlClose);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = Resources.icon_allsizes_bug;
      this.KeyPreview = true;
      this.Name = nameof (SellDetailsDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Delivery Condition Details";
      this.Activated += new EventHandler(this.SellDetailsDialog_Activated);
      this.FormClosing += new FormClosingEventHandler(this.SellDetailsDialog_FormClosing);
      this.KeyDown += new KeyEventHandler(this.SellDetailsDialog_KeyDown);
      this.pnlRight.ResumeLayout(false);
      this.pnlViewer.ResumeLayout(false);
      this.gcDocuments.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemoveDocument).EndInit();
      ((ISupportInitialize) this.btnEditDocument).EndInit();
      ((ISupportInitialize) this.btnAddDocument).EndInit();
      ((ISupportInitialize) this.btnWaivedBy).EndInit();
      ((ISupportInitialize) this.btnClearedBy).EndInit();
      ((ISupportInitialize) this.btnRejectedBy).EndInit();
      ((ISupportInitialize) this.btnReviewedBy).EndInit();
      ((ISupportInitialize) this.btnReceivedBy).EndInit();
      ((ISupportInitialize) this.btnRerequestedBy).EndInit();
      ((ISupportInitialize) this.btnRequestedBy).EndInit();
      ((ISupportInitialize) this.btnFulfilledBy).EndInit();
      this.pnlLeft.ResumeLayout(false);
      this.gcTracking.ResumeLayout(false);
      this.tabTracking.ResumeLayout(false);
      this.pageStatus.ResumeLayout(false);
      this.pageStatus.PerformLayout();
      this.pageComments.ResumeLayout(false);
      this.gcDetails.ResumeLayout(false);
      this.pnlDetails.ResumeLayout(false);
      this.pnlDetails.PerformLayout();
      this.pnlClose.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
