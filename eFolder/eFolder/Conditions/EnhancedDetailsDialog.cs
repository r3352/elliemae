// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.EnhancedDetailsDialog
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  public class EnhancedDetailsDialog : Form
  {
    private const string className = "EnhancedDetailsDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private static List<EnhancedDetailsDialog> _instanceList = new List<EnhancedDetailsDialog>();
    private LoanDataMgr loanDataMgr;
    private EnhancedConditionLog cond;
    private EnhancedConditionType conditionType;
    private GridViewDataManager gvTrackingMgr;
    private GridViewDataManager gvDocumentsMgr;
    private eFolderAccessRights rights;
    private bool canEditCondition;
    private bool canAddComment;
    private bool canMarkComment;
    private bool refreshDocuments;
    private readonly StringEnum sourceOfCondStringEnum = new StringEnum(typeof (SourceOfCondition));
    private IContainer components;
    private Panel pnlLeft;
    private GroupContainer gcDetails;
    private CollapsibleSplitter csLeft;
    private FileAttachmentViewerControl fileViewer;
    private Panel pnlRight;
    private BorderPanel pnlViewer;
    private Panel pnlDetails;
    private Panel pnlClose;
    private Button btnClose;
    private ToolTip tooltip;
    private EMHelpLink helpLink;
    private Panel ctrPanel;
    private ComboBox cboCategory;
    private Label label11;
    private Label label4;
    private Label label3;
    private ComboBox cboBorrower;
    private Label label10;
    private Label label2;
    private Label label1;
    private Label label9;
    private Button btnViewTrackingOwners;
    private CheckBox chkPrintExternal;
    private CheckBox chkPrintInternal;
    private Label label8;
    private TextBox txtExternalId;
    private Label label7;
    private TextBox txtInternalId;
    private ComboBox cboRecipientDetails;
    private ComboBox cboSource;
    private TextBox txtConditionType;
    private TextBox txtExternalDescription;
    private ComboBox cboPriorTo;
    private Label lblPriorTo;
    private Label lblCategory;
    private Label lblSource;
    private TextBox txtInternalDescription;
    private TextBox txtTitle;
    private Label lblTitle;
    private TextBox txtComment;
    private GroupContainer gcTracking;
    private GridView gvTracking;
    private Label lblDaysDue;
    private TextBox txtDaysToReceive;
    private TextBox txtDateDue;
    private Label lblRequestedFrom;
    private TextBox txtRequestedFrom;
    private Button btnAddComment;
    private Panel panel1;
    private CommentCollectionControl commentCollection;
    private CheckBox chkExternal;
    private Label label5;
    private TextBox txtDocumentReceiptDate;
    private CalendarButton calDocumentReceiptDate;
    private GroupContainer gcDocuments;
    private FlowLayoutPanel pnlToolbar;
    private Button btnRequestDocument;
    private VerticalSeparator separator;
    private StandardIconButton btnRemoveDocument;
    private StandardIconButton btnEditDocument;
    private StandardIconButton btnAddDocument;
    internal GridView gvDocuments;
    private CollapsibleSplitter csFiles;
    private Label label6;
    private TextBox txtSourceOfCondition;
    private TextBox txtEffectiveEnd;
    private TextBox txtEffectiveStart;
    private ComboBox cboOwner;
    private Label label12;
    private Label lblPartner;
    private TextBox txtPartner;

    public static void ShowInstance(
      LoanDataMgr loanDataMgr,
      EnhancedConditionLog cond,
      EnhancedConditionType conditionType)
    {
      if (Form.ActiveForm != null && Form.ActiveForm.Modal)
      {
        using (EnhancedDetailsDialog enhancedDetailsDialog = new EnhancedDetailsDialog(loanDataMgr, cond, conditionType))
        {
          int num = (int) enhancedDetailsDialog.ShowDialog((IWin32Window) Form.ActiveForm);
        }
      }
      else
      {
        EnhancedDetailsDialog enhancedDetailsDialog1 = (EnhancedDetailsDialog) null;
        foreach (EnhancedDetailsDialog instance in EnhancedDetailsDialog._instanceList)
        {
          if (instance.Condition == cond)
            enhancedDetailsDialog1 = instance;
        }
        if (enhancedDetailsDialog1 == null)
        {
          EnhancedDetailsDialog enhancedDetailsDialog2 = new EnhancedDetailsDialog(loanDataMgr, cond, conditionType);
          enhancedDetailsDialog2.FormClosing += new FormClosingEventHandler(EnhancedDetailsDialog._instance_FormClosing);
          EnhancedDetailsDialog._instanceList.Add(enhancedDetailsDialog2);
          enhancedDetailsDialog2.Show();
        }
        else
        {
          if (enhancedDetailsDialog1.WindowState == FormWindowState.Minimized)
            enhancedDetailsDialog1.WindowState = FormWindowState.Normal;
          enhancedDetailsDialog1.Activate();
        }
      }
    }

    public static void CloseInstances()
    {
      if (EnhancedDetailsDialog._instanceList == null && EnhancedDetailsDialog._instanceList.Count == 0)
        return;
      int num = EnhancedDetailsDialog._instanceList.Count - 1;
      try
      {
        for (int index = num; index >= 0; --index)
          EnhancedDetailsDialog._instanceList[index].Close();
      }
      catch
      {
      }
    }

    private static void _instance_FormClosing(object sender, FormClosingEventArgs e)
    {
      EnhancedDetailsDialog enhancedDetailsDialog = (EnhancedDetailsDialog) sender;
      EnhancedDetailsDialog._instanceList.Remove(enhancedDetailsDialog);
    }

    private EnhancedDetailsDialog(
      LoanDataMgr loanDataMgr,
      EnhancedConditionLog cond,
      EnhancedConditionType conditionType)
    {
      this.InitializeComponent();
      this.setWindowSize();
      this.loanDataMgr = loanDataMgr;
      this.cond = cond;
      this.conditionType = conditionType;
      this.rights = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) cond);
      this.canEditCondition = this.rights.CanEditEnhancedCondition(this.cond.EnhancedConditionType);
      this.initEventHandlers();
      this.initBorrowerField();
      this.initSourceField();
      this.initRecipientDetailsField();
      this.initPriorToField();
      this.initCategoryField();
      this.initOwnerField();
      this.loadConditionDetails();
    }

    private void setWindowSize()
    {
      if (Form.ActiveForm != null)
      {
        Form form = Form.ActiveForm;
        while (form.Owner != null)
          form = form.Owner;
        if (this.Width < Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width))
          return;
        this.Width = Convert.ToInt32((double) form.Width * 0.99);
        this.Height = Convert.ToInt32((double) form.Height * 0.99);
      }
      else
      {
        Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Width = Convert.ToInt32((double) workingArea.Width * 0.95);
        workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Height = Convert.ToInt32((double) workingArea.Height * 0.95);
      }
    }

    public EnhancedConditionLog Condition => this.cond;

    private void loadConditionDetails(bool isConditionChanged = false)
    {
      this.loadTitleFields();
      this.loadDescriptionFields();
      this.loadBorrowerField();
      this.loadConditionTypeField();
      this.loadSourceField();
      this.loadRecipientDetailsField();
      this.loadPriorToField();
      this.loadCategoryField();
      this.loadOwnerField();
      this.loadSourceOfConditionField();
      if (this.cond.SourceOfCondition.ToString() == SourceOfCondition.PartnerConnect.ToString())
        this.loadPartnerField();
      else
        this.hidePartnerField(isConditionChanged);
      this.loadEffectiveDateFields();
      this.loadIDFields();
      this.loadPrintFields();
      this.loadDaysToReceiveField();
      this.loadDateDueField();
      this.loadRequestedFromField();
      this.loadDocumentReceiptDateField();
      this.initTrackingList();
      this.loadTrackingList();
      this.initDocumentList();
      this.loadDocumentList(true);
      this.showDocumentFiles();
      this.applySecurity();
      this.loadCommentList();
    }

    private void loadTitleFields()
    {
      this.Text = "Condition Details (" + this.cond.Title + ")";
      this.txtTitle.Text = this.cond.Title;
    }

    private void txtTitle_MouseHover(object sender, EventArgs e)
    {
      this.tooltip.SetToolTip((Control) this.txtTitle, this.txtTitle.Text);
    }

    private void loadDescriptionFields()
    {
      this.txtInternalDescription.Text = this.cond.InternalDescription;
      this.txtExternalDescription.Text = this.cond.ExternalDescription;
    }

    private void txtInternalDescription_Validated(object sender, EventArgs e)
    {
      if (!(this.cond.InternalDescription != this.txtInternalDescription.Text))
        return;
      this.cond.InternalDescription = this.txtInternalDescription.Text;
    }

    private void txtExternalDescription_Validated(object sender, EventArgs e)
    {
      if (!(this.cond.ExternalDescription != this.txtExternalDescription.Text))
        return;
      this.cond.ExternalDescription = this.txtExternalDescription.Text;
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
        if (borrowerPair.Id == this.cond.PairId.Trim())
          this.cboBorrower.SelectedItem = (object) borrowerPair;
      }
    }

    private void cboBorrower_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.cond.PairId = ((BorrowerPair) this.cboBorrower.SelectedItem).Id;
    }

    private void loadConditionTypeField()
    {
      this.txtConditionType.Text = this.cond.EnhancedConditionType;
    }

    private void initSourceField()
    {
      foreach (object sourceDefinition in (IEnumerable<OptionDefinition>) this.cond.Definitions.SourceDefinitions)
        this.cboSource.Items.Add(sourceDefinition);
    }

    private void loadSourceField()
    {
      this.cboSource.SelectedItem = (object) null;
      foreach (OptionDefinition optionDefinition in this.cboSource.Items)
      {
        if (optionDefinition.Name == this.cond.Source)
          this.cboSource.SelectedItem = (object) optionDefinition;
      }
    }

    private void cboSource_SelectionChangeCommitted(object sender, EventArgs e)
    {
      OptionDefinition selectedItem = (OptionDefinition) this.cboSource.SelectedItem;
      if (!(this.cond.Source != selectedItem.Name))
        return;
      this.cond.Source = selectedItem.Name;
    }

    private void initRecipientDetailsField()
    {
      foreach (object recipientDefinition in (IEnumerable<OptionDefinition>) this.cond.Definitions.RecipientDefinitions)
        this.cboRecipientDetails.Items.Add(recipientDefinition);
    }

    private void loadRecipientDetailsField()
    {
      this.cboRecipientDetails.SelectedItem = (object) null;
      foreach (OptionDefinition optionDefinition in this.cboRecipientDetails.Items)
      {
        if (optionDefinition.Name == this.cond.Recipient)
          this.cboRecipientDetails.SelectedItem = (object) optionDefinition;
      }
    }

    private void cboRecipientDetails_SelectionChangeCommitted(object sender, EventArgs e)
    {
      OptionDefinition selectedItem = (OptionDefinition) this.cboRecipientDetails.SelectedItem;
      if (!(this.cond.Recipient != selectedItem.Name))
        return;
      this.cond.Recipient = selectedItem.Name;
    }

    private void initPriorToField()
    {
      foreach (object priorToDefinition in (IEnumerable<OptionDefinition>) this.cond.Definitions.PriorToDefinitions)
        this.cboPriorTo.Items.Add(priorToDefinition);
    }

    private void loadPriorToField()
    {
      this.cboPriorTo.SelectedItem = (object) null;
      foreach (OptionDefinition optionDefinition in this.cboPriorTo.Items)
      {
        if (optionDefinition.Name == this.cond.PriorTo)
          this.cboPriorTo.SelectedItem = (object) optionDefinition;
      }
    }

    private void cboPriorTo_SelectionChangeCommitted(object sender, EventArgs e)
    {
      OptionDefinition selectedItem = (OptionDefinition) this.cboPriorTo.SelectedItem;
      if (!(this.cond.PriorTo != selectedItem.Name))
        return;
      this.cond.PriorTo = selectedItem.Name;
    }

    private void initCategoryField()
    {
      foreach (object categoryDefinition in (IEnumerable<OptionDefinition>) this.cond.Definitions.CategoryDefinitions)
        this.cboCategory.Items.Add(categoryDefinition);
    }

    private void loadCategoryField()
    {
      this.cboCategory.SelectedItem = (object) null;
      foreach (OptionDefinition optionDefinition in this.cboCategory.Items)
      {
        if (optionDefinition.Name == this.cond.Category)
          this.cboCategory.SelectedItem = (object) optionDefinition;
      }
    }

    private void cboCategory_SelectionChangeCommitted(object sender, EventArgs e)
    {
      OptionDefinition selectedItem = (OptionDefinition) this.cboCategory.SelectedItem;
      if (!(this.cond.Category != selectedItem.Name))
        return;
      this.cond.Category = selectedItem.Name;
    }

    private void initOwnerField()
    {
      RoleInfo[] array = ((IEnumerable<RoleInfo>) ((WorkflowManager) Session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions()).OrderBy<RoleInfo, string>((Func<RoleInfo, string>) (r => r.RoleName)).ToArray<RoleInfo>();
      if (array.Length == 0)
        return;
      foreach (object obj in array)
        this.cboOwner.Items.Add(obj);
      this.cboOwner.DisplayMember = "RoleName";
      this.cboOwner.ValueMember = "RoleID";
    }

    private void loadOwnerField()
    {
      this.cboOwner.SelectedItem = (object) null;
      foreach (RoleInfo roleInfo in this.cboOwner.Items)
      {
        int roleId = roleInfo.RoleID;
        int? owner = this.cond.Owner;
        int valueOrDefault = owner.GetValueOrDefault();
        if (roleId == valueOrDefault & owner.HasValue)
          this.cboOwner.SelectedItem = (object) roleInfo;
      }
    }

    private void cboOwner_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.cond.Owner = new int?(((RoleSummaryInfo) this.cboOwner.SelectedItem).RoleID);
    }

    private void loadSourceOfConditionField()
    {
      this.txtSourceOfCondition.Text = this.sourceOfCondStringEnum.GetStringValue(this.cond.SourceOfCondition.ToString());
    }

    private void loadPartnerField() => this.txtPartner.Text = this.cond.Partner;

    private void hidePartnerField(bool isConditionChanged)
    {
      if (isConditionChanged)
        return;
      this.lblPartner.Visible = false;
      this.txtPartner.Visible = false;
      this.label4.Top -= this.lblPartner.Height + this.txtPartner.Height;
      this.txtEffectiveStart.Top -= this.lblPartner.Height + this.txtPartner.Height;
      this.label11.Top -= this.lblPartner.Height + this.txtPartner.Height;
      this.txtEffectiveEnd.Top -= this.lblPartner.Height + this.txtPartner.Height;
      this.label7.Top -= this.lblPartner.Height + this.txtPartner.Height;
      this.txtInternalId.Top -= this.lblPartner.Height + this.txtPartner.Height;
      this.label8.Top -= this.lblPartner.Height + this.txtPartner.Height;
      this.txtExternalId.Top -= this.lblPartner.Height + this.txtPartner.Height;
      this.chkPrintInternal.Top -= this.lblPartner.Height + this.txtPartner.Height;
      this.chkPrintExternal.Top -= this.lblPartner.Height + this.txtPartner.Height;
      this.btnViewTrackingOwners.Top -= this.txtPartner.Height;
    }

    private void loadEffectiveDateFields()
    {
      DateTime? nullable;
      if (this.cond.StartDate.HasValue)
      {
        TextBox txtEffectiveStart = this.txtEffectiveStart;
        nullable = this.cond.StartDate;
        string str = nullable.Value.ToString("MM/dd/yyyy");
        txtEffectiveStart.Text = str;
      }
      nullable = this.cond.EndDate;
      if (!nullable.HasValue)
        return;
      TextBox txtEffectiveEnd = this.txtEffectiveEnd;
      nullable = this.cond.EndDate;
      string str1 = nullable.Value.ToString("MM/dd/yyyy");
      txtEffectiveEnd.Text = str1;
    }

    private void loadIDFields()
    {
      this.txtInternalId.Text = this.cond.InternalId;
      this.txtExternalId.Text = this.cond.ExternalId;
    }

    private void loadPrintFields()
    {
      bool? nullable;
      if (this.cond.InternalPrint.HasValue)
      {
        CheckBox chkPrintInternal = this.chkPrintInternal;
        nullable = this.cond.InternalPrint;
        int num = nullable.Value ? 1 : 0;
        chkPrintInternal.Checked = num != 0;
      }
      nullable = this.cond.ExternalPrint;
      if (!nullable.HasValue)
        return;
      CheckBox chkPrintExternal = this.chkPrintExternal;
      nullable = this.cond.ExternalPrint;
      int num1 = nullable.Value ? 1 : 0;
      chkPrintExternal.Checked = num1 != 0;
    }

    private void chkPrintInternal_Click(object sender, EventArgs e)
    {
      if (this.cond.InternalPrint.HasValue)
      {
        if (this.cond.InternalPrint.Value == this.chkPrintInternal.Checked)
          return;
        this.cond.InternalPrint = new bool?(this.chkPrintInternal.Checked);
      }
      else
        this.cond.InternalPrint = new bool?(this.chkPrintInternal.Checked);
    }

    private void chkPrintExternal_Click(object sender, EventArgs e)
    {
      if (this.cond.ExternalPrint.HasValue)
      {
        if (this.cond.ExternalPrint.Value == this.chkPrintExternal.Checked)
          return;
        this.cond.ExternalPrint = new bool?(this.chkPrintExternal.Checked);
      }
      else
        this.cond.ExternalPrint = new bool?(this.chkPrintInternal.Checked);
    }

    private void loadDaysToReceiveField()
    {
      this.txtDaysToReceive.Text = this.cond.DaysToReceive.ToString();
    }

    private void txtDaysToReceive_Validated(object sender, EventArgs e)
    {
      int result;
      if (!int.TryParse(this.txtDaysToReceive.Text, out result))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter a numeric value for Days To Receive", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      if (!(this.cond.DaysToReceive.ToString() != this.txtDaysToReceive.Text))
        return;
      this.cond.DaysToReceive = new int?(result);
      this.loadDateDueField();
    }

    private void loadDateDueField()
    {
      if (!this.cond.DaysToReceive.HasValue)
        return;
      this.txtDateDue.Text = "on " + DateTime.Now.AddDays((double) this.cond.DaysToReceive.Value).ToString("MM/dd/yyyy");
    }

    private void loadRequestedFromField() => this.txtRequestedFrom.Text = this.cond.RequestedFrom;

    private void txtRequestedFrom_Validated(object sender, EventArgs e)
    {
      if (!(this.cond.RequestedFrom != this.txtRequestedFrom.Text))
        return;
      this.cond.RequestedFrom = this.txtRequestedFrom.Text;
    }

    private void loadDocumentReceiptDateField()
    {
      if (this.cond.DocumentReceiptDate.HasValue)
        this.txtDocumentReceiptDate.Text = Convert.ToDateTime((object) this.cond.DocumentReceiptDate).ToString("MM/dd/yyyy");
      else
        this.txtDocumentReceiptDate.Text = "";
    }

    private void txtDocumentReceiptDate_Validating(object sender, CancelEventArgs e)
    {
      if (string.IsNullOrWhiteSpace(this.txtDocumentReceiptDate.Text) || Utils.IsDate((object) this.txtDocumentReceiptDate.Text))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "Invalid date format", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      this.txtDocumentReceiptDate.Text = string.Empty;
      e.Cancel = true;
    }

    private void txtDocumentReceiptDate_Validated(object sender, EventArgs e)
    {
      this.saveDocumentReceiptDate();
    }

    private void calDocumentReceiptDate_DateSelected(object sender, CalendarPopupEventArgs e)
    {
      this.saveDocumentReceiptDate();
    }

    private void saveDocumentReceiptDate()
    {
      try
      {
        if (string.IsNullOrEmpty(this.txtDocumentReceiptDate.Text))
        {
          DateTime? nullable1 = this.cond.DocumentReceiptDate;
          if (!nullable1.HasValue)
            return;
          EnhancedConditionLog cond = this.cond;
          nullable1 = new DateTime?();
          DateTime? nullable2 = nullable1;
          cond.DocumentReceiptDate = nullable2;
        }
        else
        {
          DateTime? documentReceiptDate = this.cond.DocumentReceiptDate;
          if (documentReceiptDate.HasValue)
          {
            documentReceiptDate = this.cond.DocumentReceiptDate;
            if (documentReceiptDate.Value == Convert.ToDateTime(this.txtDocumentReceiptDate.Text))
              return;
          }
          this.cond.DocumentReceiptDate = new DateTime?(Convert.ToDateTime(this.txtDocumentReceiptDate.Text));
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error parsing Document Receipt Date : " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void initTrackingList()
    {
      this.gvTrackingMgr = new GridViewDataManager(this.gvTracking, this.loanDataMgr);
      this.gvTrackingMgr.CreateLayout(new TableLayout.Column[1]
      {
        GridViewDataManager.CheckBoxColumn
      });
    }

    private void loadTrackingList()
    {
      this.gvTrackingMgr.ClearItems();
      GVItem gvItem = new GVItem();
      GVSubItem subItem = gvItem.SubItems[0];
      string[] strArray = new string[6]
      {
        "Added by ",
        this.cond.AddedBy,
        " on ",
        null,
        null,
        null
      };
      DateTime dateTime1 = this.cond.DateAdded;
      dateTime1 = dateTime1.ToLocalTime();
      strArray[3] = dateTime1.ToString("MM/dd/yyyy");
      strArray[4] = " at ";
      DateTime dateTime2 = this.cond.DateAdded;
      dateTime2 = dateTime2.ToLocalTime();
      strArray[5] = dateTime2.ToShortTimeString();
      string str1 = string.Concat(strArray);
      subItem.Value = (object) str1;
      gvItem.Checked = true;
      gvItem.SubItems[0].CheckBoxEnabled = false;
      this.gvTrackingMgr.AddItem(gvItem);
      List<string> defaultTrackingOptions = Utils.GetEnhanceConditionsDefaultTrackingOptions();
      if (this.cond.Definitions == null || this.cond.Definitions.TrackingDefinitions == null)
        return;
      foreach (string str2 in defaultTrackingOptions)
      {
        foreach (StatusTrackingDefinition trackingDefinition in (IEnumerable<StatusTrackingDefinition>) this.cond.Definitions.TrackingDefinitions)
        {
          if (trackingDefinition.Name == str2)
            this.addTrackingDefinitionItem(trackingDefinition, true);
        }
      }
      foreach (StatusTrackingDefinition trackingDefinition in (IEnumerable<StatusTrackingDefinition>) this.cond.Definitions.TrackingDefinitions)
      {
        if (!defaultTrackingOptions.Contains(trackingDefinition.Name))
          this.addTrackingDefinitionItem(trackingDefinition, false);
      }
    }

    private void addTrackingDefinitionItem(
      StatusTrackingDefinition trackingDefinition,
      bool isDefault)
    {
      GVItem gvItem = new GVItem();
      gvItem.SubItems[0].Value = (object) trackingDefinition.Name;
      gvItem.SubItems[0].Checked = false;
      foreach (StatusTrackingEntry statusTrackingEntry in this.cond.Trackings.GetStatusTrackingEntries())
      {
        if (statusTrackingEntry.Status == trackingDefinition.Name)
        {
          gvItem.SubItems[0].Value = (object) (trackingDefinition.Name + " by " + statusTrackingEntry.UserId + " on " + (object) statusTrackingEntry.Date.ToLocalTime());
          gvItem.SubItems[0].Checked = true;
          break;
        }
      }
      gvItem.SubItems[0].CheckBoxEnabled = isDefault || this.canEditCondition && this.rights.CanUpdateEnhancedConditionTrackingStatus(trackingDefinition);
      gvItem.SubItems[0].Tag = (object) trackingDefinition;
      this.gvTrackingMgr.AddItem(gvItem);
    }

    private void gvTracking_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      StatusTrackingDefinition tag = (StatusTrackingDefinition) e.SubItem.Tag;
      if (e.SubItem.Checked)
        this.cond.Trackings.Add(tag.Name, Session.UserID);
      else
        this.cond.Trackings.Remove(tag.Name);
      this.loadTrackingList();
    }

    private void loadCommentList()
    {
      this.commentCollection.LoadComments(this.loanDataMgr, this.cond.Comments);
    }

    private void txtComment_TextChanged(object sender, EventArgs e)
    {
      this.btnAddComment.Enabled = this.canAddComment && this.txtComment.Text.Length > 0;
      this.chkExternal.Enabled = this.canAddComment && this.txtComment.Text.Length > 0;
    }

    private void btnAddComment_Click(object sender, EventArgs e)
    {
      this.cond.Comments.Add(new CommentEntry(this.txtComment.Text, Session.UserID, this.loanDataMgr.SessionObjects.UserInfo.FullName, !this.chkExternal.Checked));
      this.txtComment.Text = string.Empty;
      this.loadCommentList();
    }

    public GVItemCollection GetDocumentItems() => this.gvDocuments.Items;

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
      DocumentLog[] documentLogArray = this.cond.GetLinkedDocuments(false);
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
      this.refreshDocumentToolbar();
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

    private void refreshDocumentToolbar()
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
        this.loadDocumentList(false);
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
      this.loadDocumentList(false);
      this.showDocumentFiles();
    }

    private void btnRequestDocument_Click(object sender, EventArgs e)
    {
      new eFolderManager().Request(this.loanDataMgr, this.getSelectedDocuments(), (ConditionLog) this.cond);
    }

    private void applySecurity()
    {
      this.txtInternalDescription.ReadOnly = !this.rights.CanEditEnhancedConditionInternalDescription(this.cond.EnhancedConditionType);
      this.txtExternalDescription.ReadOnly = !this.rights.CanEditEnhancedConditionExternalDescription(this.cond.EnhancedConditionType) || this.investorDeliveryCondition();
      this.cboBorrower.Enabled = this.canEditCondition;
      this.cboSource.Enabled = this.canEditCondition && !this.investorDeliveryCondition();
      this.cboRecipientDetails.Enabled = this.canEditCondition && !this.investorDeliveryCondition();
      this.cboCategory.Enabled = this.canEditCondition;
      this.cboPriorTo.Enabled = this.rights.CanChangeEnhancedConditionPriorTo(this.cond.EnhancedConditionType);
      this.cboSource.Enabled = this.canEditCondition;
      this.cboOwner.Enabled = this.canEditCondition;
      this.chkPrintInternal.Enabled = this.rights.CanEditEnhancedConditionPrintInternally(this.cond.EnhancedConditionType);
      this.chkPrintExternal.Enabled = this.rights.CanEditEnhancedConditionPrintExternally(this.cond.EnhancedConditionType);
      this.txtDaysToReceive.Enabled = this.canEditCondition;
      this.txtRequestedFrom.Enabled = this.canEditCondition;
      this.canAddComment = this.rights.CanAddEnhancedConditionComments(this.cond.EnhancedConditionType);
      this.canMarkComment = this.rights.CanMarkEnhancedConditionComments(this.cond.EnhancedConditionType);
      this.txtComment.Enabled = this.canAddComment;
      this.chkExternal.Enabled = false;
      this.commentCollection.DisplayEnhanced(this.canMarkComment);
      this.commentCollection.CanAddComment = false;
      this.commentCollection.CanDeleteComment = this.rights.CanDeleteEnhancedConditionComments(this.cond.EnhancedConditionType);
      this.btnAddDocument.Visible = this.rights.CanAssignEnhancedConditionDocuments(this.cond.EnhancedConditionType);
      this.btnRemoveDocument.Visible = this.rights.CanUnassignEnhancedConditionDocuments(this.cond.EnhancedConditionType);
      this.separator.Visible = this.rights.CanRequestDocuments || this.rights.CanRequestServices;
      this.btnRequestDocument.Visible = this.rights.CanRequestDocuments || this.rights.CanRequestServices;
    }

    private bool investorDeliveryCondition()
    {
      return object.Equals((object) this.cond.SourceOfCondition, (object) SourceOfCondition.InvestorDelivery);
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
      Tracing.Log(EnhancedDetailsDialog.sw, TraceLevel.Verbose, nameof (EnhancedDetailsDialog), "Checking InvokeRequired For LogRecordChanged");
      if (this.InvokeRequired)
      {
        LogRecordEventHandler method = new LogRecordEventHandler(this.logRecordChanged);
        Tracing.Log(EnhancedDetailsDialog.sw, TraceLevel.Verbose, nameof (EnhancedDetailsDialog), "Calling BeginInvoke For LogRecordChanged");
        this.BeginInvoke((Delegate) method, source, (object) e);
      }
      else if (e.LogRecord == this.cond)
      {
        this.loadConditionDetails(true);
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
      Tracing.Log(EnhancedDetailsDialog.sw, TraceLevel.Verbose, nameof (EnhancedDetailsDialog), "Checking InvokeRequired For LogRecordRemoved");
      if (this.InvokeRequired)
      {
        LogRecordEventHandler method = new LogRecordEventHandler(this.logRecordRemoved);
        Tracing.Log(EnhancedDetailsDialog.sw, TraceLevel.Verbose, nameof (EnhancedDetailsDialog), "Calling BeginInvoke For LogRecordRemoved");
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
      this.cond = (EnhancedConditionLog) this.loanDataMgr.LoanData.GetLogList().GetRecordByID(this.cond.Guid);
      this.loadConditionDetails();
    }

    private void btnViewTrackingOwners_Click(object sender, EventArgs e)
    {
      using (TrackingOwnersDialog trackingOwnersDialog = new TrackingOwnersDialog(this.loanDataMgr, this.cond))
      {
        int num = (int) trackingOwnersDialog.ShowDialog((IWin32Window) this);
      }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      if (sender is LoanDataMgr)
        this.AutoValidate = AutoValidate.Disable;
      this.Close();
    }

    private void EnhancedDetailsDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      Session.Application.DisplayHelp(this.helpLink.HelpTag);
    }

    private void EnhancedDetailsDialog_Activated(object sender, EventArgs e)
    {
      if (!this.refreshDocuments)
        return;
      this.loadDocumentList(false);
      this.showDocumentFiles();
      this.refreshDocuments = false;
    }

    private void EnhancedDetailsDialog_FormClosing(object sender, FormClosingEventArgs e)
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EnhancedDetailsDialog));
      this.pnlLeft = new Panel();
      this.gcDetails = new GroupContainer();
      this.pnlDetails = new Panel();
      this.lblPartner = new Label();
      this.txtPartner = new TextBox();
      this.cboOwner = new ComboBox();
      this.label12 = new Label();
      this.txtEffectiveEnd = new TextBox();
      this.txtEffectiveStart = new TextBox();
      this.label6 = new Label();
      this.txtSourceOfCondition = new TextBox();
      this.cboCategory = new ComboBox();
      this.label11 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.cboBorrower = new ComboBox();
      this.label10 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.label9 = new Label();
      this.btnViewTrackingOwners = new Button();
      this.chkPrintExternal = new CheckBox();
      this.chkPrintInternal = new CheckBox();
      this.label8 = new Label();
      this.txtExternalId = new TextBox();
      this.label7 = new Label();
      this.txtInternalId = new TextBox();
      this.cboRecipientDetails = new ComboBox();
      this.cboSource = new ComboBox();
      this.txtConditionType = new TextBox();
      this.txtExternalDescription = new TextBox();
      this.cboPriorTo = new ComboBox();
      this.lblPriorTo = new Label();
      this.lblCategory = new Label();
      this.lblSource = new Label();
      this.txtInternalDescription = new TextBox();
      this.txtTitle = new TextBox();
      this.lblTitle = new Label();
      this.ctrPanel = new Panel();
      this.txtComment = new TextBox();
      this.gcTracking = new GroupContainer();
      this.calDocumentReceiptDate = new CalendarButton();
      this.txtDocumentReceiptDate = new TextBox();
      this.label5 = new Label();
      this.gvTracking = new GridView();
      this.lblDaysDue = new Label();
      this.txtDaysToReceive = new TextBox();
      this.txtDateDue = new TextBox();
      this.lblRequestedFrom = new Label();
      this.txtRequestedFrom = new TextBox();
      this.btnAddComment = new Button();
      this.panel1 = new Panel();
      this.commentCollection = new CommentCollectionControl();
      this.chkExternal = new CheckBox();
      this.tooltip = new ToolTip(this.components);
      this.btnRequestDocument = new Button();
      this.btnRemoveDocument = new StandardIconButton();
      this.btnEditDocument = new StandardIconButton();
      this.btnAddDocument = new StandardIconButton();
      this.pnlRight = new Panel();
      this.pnlViewer = new BorderPanel();
      this.fileViewer = new FileAttachmentViewerControl();
      this.csFiles = new CollapsibleSplitter();
      this.gcDocuments = new GroupContainer();
      this.pnlToolbar = new FlowLayoutPanel();
      this.separator = new VerticalSeparator();
      this.gvDocuments = new GridView();
      this.pnlClose = new Panel();
      this.helpLink = new EMHelpLink();
      this.btnClose = new Button();
      this.csLeft = new CollapsibleSplitter();
      this.pnlLeft.SuspendLayout();
      this.gcDetails.SuspendLayout();
      this.pnlDetails.SuspendLayout();
      this.ctrPanel.SuspendLayout();
      this.gcTracking.SuspendLayout();
      ((ISupportInitialize) this.calDocumentReceiptDate).BeginInit();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.btnRemoveDocument).BeginInit();
      ((ISupportInitialize) this.btnEditDocument).BeginInit();
      ((ISupportInitialize) this.btnAddDocument).BeginInit();
      this.pnlRight.SuspendLayout();
      this.pnlViewer.SuspendLayout();
      this.gcDocuments.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      this.pnlClose.SuspendLayout();
      this.SuspendLayout();
      this.pnlLeft.AutoScroll = true;
      this.pnlLeft.Controls.Add((Control) this.gcDetails);
      this.pnlLeft.Controls.Add((Control) this.ctrPanel);
      this.pnlLeft.Dock = DockStyle.Left;
      this.pnlLeft.Location = new Point(0, 0);
      this.pnlLeft.Name = "pnlLeft";
      this.pnlLeft.Size = new Size(660, 638);
      this.pnlLeft.TabIndex = 0;
      this.gcDetails.Controls.Add((Control) this.pnlDetails);
      this.gcDetails.HeaderForeColor = SystemColors.ControlText;
      this.gcDetails.Location = new Point(0, 0);
      this.gcDetails.Name = "gcDetails";
      this.gcDetails.Size = new Size(332, 638);
      this.gcDetails.TabIndex = 1;
      this.gcDetails.Text = "Details";
      this.pnlDetails.AutoScroll = true;
      this.pnlDetails.AutoScrollMargin = new Size(8, 8);
      this.pnlDetails.Controls.Add((Control) this.lblPartner);
      this.pnlDetails.Controls.Add((Control) this.txtPartner);
      this.pnlDetails.Controls.Add((Control) this.cboOwner);
      this.pnlDetails.Controls.Add((Control) this.label12);
      this.pnlDetails.Controls.Add((Control) this.txtEffectiveEnd);
      this.pnlDetails.Controls.Add((Control) this.txtEffectiveStart);
      this.pnlDetails.Controls.Add((Control) this.label6);
      this.pnlDetails.Controls.Add((Control) this.txtSourceOfCondition);
      this.pnlDetails.Controls.Add((Control) this.cboCategory);
      this.pnlDetails.Controls.Add((Control) this.label11);
      this.pnlDetails.Controls.Add((Control) this.label4);
      this.pnlDetails.Controls.Add((Control) this.label3);
      this.pnlDetails.Controls.Add((Control) this.cboBorrower);
      this.pnlDetails.Controls.Add((Control) this.label10);
      this.pnlDetails.Controls.Add((Control) this.label2);
      this.pnlDetails.Controls.Add((Control) this.label1);
      this.pnlDetails.Controls.Add((Control) this.label9);
      this.pnlDetails.Controls.Add((Control) this.btnViewTrackingOwners);
      this.pnlDetails.Controls.Add((Control) this.chkPrintExternal);
      this.pnlDetails.Controls.Add((Control) this.chkPrintInternal);
      this.pnlDetails.Controls.Add((Control) this.label8);
      this.pnlDetails.Controls.Add((Control) this.txtExternalId);
      this.pnlDetails.Controls.Add((Control) this.label7);
      this.pnlDetails.Controls.Add((Control) this.txtInternalId);
      this.pnlDetails.Controls.Add((Control) this.cboRecipientDetails);
      this.pnlDetails.Controls.Add((Control) this.cboSource);
      this.pnlDetails.Controls.Add((Control) this.txtConditionType);
      this.pnlDetails.Controls.Add((Control) this.txtExternalDescription);
      this.pnlDetails.Controls.Add((Control) this.cboPriorTo);
      this.pnlDetails.Controls.Add((Control) this.lblPriorTo);
      this.pnlDetails.Controls.Add((Control) this.lblCategory);
      this.pnlDetails.Controls.Add((Control) this.lblSource);
      this.pnlDetails.Controls.Add((Control) this.txtInternalDescription);
      this.pnlDetails.Controls.Add((Control) this.txtTitle);
      this.pnlDetails.Controls.Add((Control) this.lblTitle);
      this.pnlDetails.Dock = DockStyle.Fill;
      this.pnlDetails.Location = new Point(1, 26);
      this.pnlDetails.Name = "pnlDetails";
      this.pnlDetails.Size = new Size(330, 611);
      this.pnlDetails.TabIndex = 2;
      this.lblPartner.AutoSize = true;
      this.lblPartner.Location = new Point(5, 440);
      this.lblPartner.Name = "lblPartner";
      this.lblPartner.Size = new Size(44, 14);
      this.lblPartner.TabIndex = 92;
      this.lblPartner.Text = "Service";
      this.txtPartner.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtPartner.Enabled = false;
      this.txtPartner.Location = new Point(8, 457);
      this.txtPartner.Name = "txtPartner";
      this.txtPartner.ReadOnly = true;
      this.txtPartner.ScrollBars = ScrollBars.Vertical;
      this.txtPartner.Size = new Size(151, 20);
      this.txtPartner.TabIndex = 65;
      this.cboOwner.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboOwner.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboOwner.FormattingEnabled = true;
      this.cboOwner.Location = new Point(175, 417);
      this.cboOwner.Name = "cboOwner";
      this.cboOwner.Size = new Size(151, 22);
      this.cboOwner.TabIndex = 60;
      this.cboOwner.SelectionChangeCommitted += new EventHandler(this.cboOwner_SelectionChangeCommitted);
      this.label12.AutoSize = true;
      this.label12.Location = new Point(175, 402);
      this.label12.Name = "label12";
      this.label12.Size = new Size(41, 14);
      this.label12.TabIndex = 89;
      this.label12.Text = "Owner";
      this.txtEffectiveEnd.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtEffectiveEnd.Enabled = false;
      this.txtEffectiveEnd.Location = new Point(175, 493);
      this.txtEffectiveEnd.Name = "txtEffectiveEnd";
      this.txtEffectiveEnd.ReadOnly = true;
      this.txtEffectiveEnd.ScrollBars = ScrollBars.Vertical;
      this.txtEffectiveEnd.Size = new Size(151, 20);
      this.txtEffectiveEnd.TabIndex = 75;
      this.txtEffectiveStart.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtEffectiveStart.Enabled = false;
      this.txtEffectiveStart.Location = new Point(8, 493);
      this.txtEffectiveStart.Name = "txtEffectiveStart";
      this.txtEffectiveStart.ReadOnly = true;
      this.txtEffectiveStart.ScrollBars = ScrollBars.Vertical;
      this.txtEffectiveStart.Size = new Size(151, 20);
      this.txtEffectiveStart.TabIndex = 70;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(5, 402);
      this.label6.Name = "label6";
      this.label6.Size = new Size(102, 14);
      this.label6.TabIndex = 86;
      this.label6.Text = "Source of Condition";
      this.txtSourceOfCondition.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSourceOfCondition.Enabled = false;
      this.txtSourceOfCondition.Location = new Point(8, 419);
      this.txtSourceOfCondition.Name = "txtSourceOfCondition";
      this.txtSourceOfCondition.ReadOnly = true;
      this.txtSourceOfCondition.ScrollBars = ScrollBars.Vertical;
      this.txtSourceOfCondition.Size = new Size(151, 20);
      this.txtSourceOfCondition.TabIndex = 55;
      this.cboCategory.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCategory.FormattingEnabled = true;
      this.cboCategory.Location = new Point(175, 376);
      this.cboCategory.Name = "cboCategory";
      this.cboCategory.Size = new Size(151, 22);
      this.cboCategory.TabIndex = 50;
      this.cboCategory.SelectionChangeCommitted += new EventHandler(this.cboCategory_SelectionChangeCommitted);
      this.label11.AutoSize = true;
      this.label11.Location = new Point(174, 477);
      this.label11.Name = "label11";
      this.label11.Size = new Size(96, 14);
      this.label11.TabIndex = 81;
      this.label11.Text = "Effective End Date";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(5, 477);
      this.label4.Name = "label4";
      this.label4.Size = new Size(101, 14);
      this.label4.TabIndex = 80;
      this.label4.Text = "Effective Start Date";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(175, 312);
      this.label3.Name = "label3";
      this.label3.Size = new Size(86, 14);
      this.label3.TabIndex = 79;
      this.label3.Text = "Recipient Details";
      this.cboBorrower.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboBorrower.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboBorrower.FormattingEnabled = true;
      this.cboBorrower.Location = new Point(8, 235);
      this.cboBorrower.Name = "cboBorrower";
      this.cboBorrower.Size = new Size(318, 22);
      this.cboBorrower.TabIndex = 25;
      this.cboBorrower.SelectionChangeCommitted += new EventHandler(this.cboBorrower_SelectionChangeCommitted);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(7, 267);
      this.label10.Name = "label10";
      this.label10.Size = new Size(77, 14);
      this.label10.TabIndex = 76;
      this.label10.Text = "Condition Type";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 219);
      this.label2.Name = "label2";
      this.label2.Size = new Size(94, 14);
      this.label2.TabIndex = 75;
      this.label2.Text = "For Borrower Pair";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 137);
      this.label1.Name = "label1";
      this.label1.Size = new Size(103, 14);
      this.label1.TabIndex = 74;
      this.label1.Text = "External Description";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(8, 56);
      this.label9.Name = "label9";
      this.label9.Size = new Size(99, 14);
      this.label9.TabIndex = 73;
      this.label9.Text = "Internal Description";
      this.btnViewTrackingOwners.Location = new Point(8, 587);
      this.btnViewTrackingOwners.Name = "btnViewTrackingOwners";
      this.btnViewTrackingOwners.Size = new Size(201, 23);
      this.btnViewTrackingOwners.TabIndex = 100;
      this.btnViewTrackingOwners.Text = "View Tracking Owners";
      this.btnViewTrackingOwners.UseVisualStyleBackColor = true;
      this.btnViewTrackingOwners.Click += new EventHandler(this.btnViewTrackingOwners_Click);
      this.chkPrintExternal.AutoSize = true;
      this.chkPrintExternal.Location = new Point(179, 560);
      this.chkPrintExternal.Name = "chkPrintExternal";
      this.chkPrintExternal.Size = new Size(97, 18);
      this.chkPrintExternal.TabIndex = 95;
      this.chkPrintExternal.Text = "Print Externally";
      this.chkPrintExternal.UseVisualStyleBackColor = true;
      this.chkPrintExternal.Click += new EventHandler(this.chkPrintExternal_Click);
      this.chkPrintInternal.AutoSize = true;
      this.chkPrintInternal.Location = new Point(15, 560);
      this.chkPrintInternal.Name = "chkPrintInternal";
      this.chkPrintInternal.Size = new Size(93, 18);
      this.chkPrintInternal.TabIndex = 90;
      this.chkPrintInternal.Text = "Print Internally";
      this.chkPrintInternal.UseVisualStyleBackColor = true;
      this.chkPrintInternal.Click += new EventHandler(this.chkPrintInternal_Click);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(175, 517);
      this.label8.MaximumSize = new Size(75, 0);
      this.label8.Name = "label8";
      this.label8.Size = new Size(58, 14);
      this.label8.TabIndex = 69;
      this.label8.Text = "External ID";
      this.txtExternalId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtExternalId.Enabled = false;
      this.txtExternalId.Location = new Point(175, 533);
      this.txtExternalId.Name = "txtExternalId";
      this.txtExternalId.ReadOnly = true;
      this.txtExternalId.ScrollBars = ScrollBars.Vertical;
      this.txtExternalId.Size = new Size(151, 20);
      this.txtExternalId.TabIndex = 85;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(5, 517);
      this.label7.MaximumSize = new Size(75, 0);
      this.label7.Name = "label7";
      this.label7.Size = new Size(54, 14);
      this.label7.TabIndex = 67;
      this.label7.Text = "Internal ID";
      this.txtInternalId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtInternalId.Enabled = false;
      this.txtInternalId.Location = new Point(8, 533);
      this.txtInternalId.Name = "txtInternalId";
      this.txtInternalId.ReadOnly = true;
      this.txtInternalId.ScrollBars = ScrollBars.Vertical;
      this.txtInternalId.Size = new Size(151, 20);
      this.txtInternalId.TabIndex = 80;
      this.cboRecipientDetails.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboRecipientDetails.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRecipientDetails.FormattingEnabled = true;
      this.cboRecipientDetails.Location = new Point(175, 326);
      this.cboRecipientDetails.Name = "cboRecipientDetails";
      this.cboRecipientDetails.Size = new Size(151, 22);
      this.cboRecipientDetails.TabIndex = 40;
      this.cboRecipientDetails.SelectionChangeCommitted += new EventHandler(this.cboRecipientDetails_SelectionChangeCommitted);
      this.cboSource.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboSource.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSource.FormattingEnabled = true;
      this.cboSource.Location = new Point(8, 326);
      this.cboSource.Name = "cboSource";
      this.cboSource.Size = new Size(151, 22);
      this.cboSource.TabIndex = 35;
      this.cboSource.SelectionChangeCommitted += new EventHandler(this.cboSource_SelectionChangeCommitted);
      this.txtConditionType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtConditionType.Enabled = false;
      this.txtConditionType.Location = new Point(8, 283);
      this.txtConditionType.Name = "txtConditionType";
      this.txtConditionType.ReadOnly = true;
      this.txtConditionType.ScrollBars = ScrollBars.Vertical;
      this.txtConditionType.Size = new Size(318, 20);
      this.txtConditionType.TabIndex = 30;
      this.txtExternalDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtExternalDescription.Location = new Point(8, 153);
      this.txtExternalDescription.Multiline = true;
      this.txtExternalDescription.Name = "txtExternalDescription";
      this.txtExternalDescription.ScrollBars = ScrollBars.Vertical;
      this.txtExternalDescription.Size = new Size(318, 60);
      this.txtExternalDescription.TabIndex = 20;
      this.txtExternalDescription.Validated += new EventHandler(this.txtExternalDescription_Validated);
      this.cboPriorTo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboPriorTo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPriorTo.FormattingEnabled = true;
      this.cboPriorTo.Location = new Point(8, 376);
      this.cboPriorTo.Name = "cboPriorTo";
      this.cboPriorTo.Size = new Size(151, 22);
      this.cboPriorTo.TabIndex = 45;
      this.cboPriorTo.SelectionChangeCommitted += new EventHandler(this.cboPriorTo_SelectionChangeCommitted);
      this.lblPriorTo.AutoSize = true;
      this.lblPriorTo.Location = new Point(8, 361);
      this.lblPriorTo.Name = "lblPriorTo";
      this.lblPriorTo.Size = new Size(43, 14);
      this.lblPriorTo.TabIndex = 59;
      this.lblPriorTo.Text = "Prior To";
      this.lblCategory.AutoSize = true;
      this.lblCategory.Location = new Point(175, 361);
      this.lblCategory.Name = "lblCategory";
      this.lblCategory.Size = new Size(51, 14);
      this.lblCategory.TabIndex = 58;
      this.lblCategory.Text = "Category";
      this.lblSource.AutoSize = true;
      this.lblSource.Location = new Point(7, 312);
      this.lblSource.Name = "lblSource";
      this.lblSource.Size = new Size(42, 14);
      this.lblSource.TabIndex = 57;
      this.lblSource.Text = "Source";
      this.txtInternalDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtInternalDescription.Location = new Point(8, 72);
      this.txtInternalDescription.Multiline = true;
      this.txtInternalDescription.Name = "txtInternalDescription";
      this.txtInternalDescription.ScrollBars = ScrollBars.Vertical;
      this.txtInternalDescription.Size = new Size(318, 60);
      this.txtInternalDescription.TabIndex = 15;
      this.txtInternalDescription.Validated += new EventHandler(this.txtInternalDescription_Validated);
      this.txtTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtTitle.Location = new Point(8, 28);
      this.txtTitle.Name = "txtTitle";
      this.txtTitle.ReadOnly = true;
      this.txtTitle.Size = new Size(318, 20);
      this.txtTitle.TabIndex = 10;
      this.txtTitle.MouseHover += new EventHandler(this.txtTitle_MouseHover);
      this.lblTitle.AutoSize = true;
      this.lblTitle.Location = new Point(6, 12);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(34, 14);
      this.lblTitle.TabIndex = 54;
      this.lblTitle.Text = "Name";
      this.ctrPanel.BorderStyle = BorderStyle.FixedSingle;
      this.ctrPanel.Controls.Add((Control) this.txtComment);
      this.ctrPanel.Controls.Add((Control) this.gcTracking);
      this.ctrPanel.Controls.Add((Control) this.btnAddComment);
      this.ctrPanel.Controls.Add((Control) this.panel1);
      this.ctrPanel.Controls.Add((Control) this.chkExternal);
      this.ctrPanel.Location = new Point(333, 0);
      this.ctrPanel.Name = "ctrPanel";
      this.ctrPanel.Size = new Size(327, 638);
      this.ctrPanel.TabIndex = 43;
      this.txtComment.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtComment.Location = new Point(3, 528);
      this.txtComment.Multiline = true;
      this.txtComment.Name = "txtComment";
      this.txtComment.ScrollBars = ScrollBars.Vertical;
      this.txtComment.Size = new Size(319, 71);
      this.txtComment.TabIndex = 145;
      this.txtComment.TextChanged += new EventHandler(this.txtComment_TextChanged);
      this.gcTracking.Controls.Add((Control) this.calDocumentReceiptDate);
      this.gcTracking.Controls.Add((Control) this.txtDocumentReceiptDate);
      this.gcTracking.Controls.Add((Control) this.label5);
      this.gcTracking.Controls.Add((Control) this.gvTracking);
      this.gcTracking.Controls.Add((Control) this.lblDaysDue);
      this.gcTracking.Controls.Add((Control) this.txtDaysToReceive);
      this.gcTracking.Controls.Add((Control) this.txtDateDue);
      this.gcTracking.Controls.Add((Control) this.lblRequestedFrom);
      this.gcTracking.Controls.Add((Control) this.txtRequestedFrom);
      this.gcTracking.HeaderForeColor = SystemColors.ControlText;
      this.gcTracking.Location = new Point(3, 0);
      this.gcTracking.Name = "gcTracking";
      this.gcTracking.Padding = new Padding(3, 3, 2, 2);
      this.gcTracking.Size = new Size(319, 300);
      this.gcTracking.TabIndex = 105;
      this.gcTracking.Text = "Tracking Status";
      this.calDocumentReceiptDate.DateControl = (Control) this.txtDocumentReceiptDate;
      ((IconButton) this.calDocumentReceiptDate).Image = (Image) componentResourceManager.GetObject("calDocumentReceiptDate.Image");
      this.calDocumentReceiptDate.Location = new Point(169, 85);
      this.calDocumentReceiptDate.MouseDownImage = (Image) null;
      this.calDocumentReceiptDate.Name = "calDocumentReceiptDate";
      this.calDocumentReceiptDate.Size = new Size(16, 16);
      this.calDocumentReceiptDate.SizeMode = PictureBoxSizeMode.AutoSize;
      this.calDocumentReceiptDate.TabIndex = 86;
      this.calDocumentReceiptDate.TabStop = false;
      this.calDocumentReceiptDate.DateSelected += new CalendarPopupEventHandler(this.calDocumentReceiptDate_DateSelected);
      this.txtDocumentReceiptDate.Location = new Point(10, 85);
      this.txtDocumentReceiptDate.Name = "txtDocumentReceiptDate";
      this.txtDocumentReceiptDate.Size = new Size(153, 20);
      this.txtDocumentReceiptDate.TabIndex = 125;
      this.txtDocumentReceiptDate.Validating += new CancelEventHandler(this.txtDocumentReceiptDate_Validating);
      this.txtDocumentReceiptDate.Validated += new EventHandler(this.txtDocumentReceiptDate_Validated);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(7, 68);
      this.label5.Name = "label5";
      this.label5.Size = new Size(119, 14);
      this.label5.TabIndex = 30;
      this.label5.Text = "Document Receipt Date";
      this.gvTracking.AllowDrop = true;
      this.gvTracking.BorderStyle = BorderStyle.None;
      this.gvTracking.ClearSelectionsOnEmptyRowClick = false;
      this.gvTracking.Dock = DockStyle.Bottom;
      this.gvTracking.HeaderHeight = 0;
      this.gvTracking.HeaderVisible = false;
      this.gvTracking.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvTracking.Location = new Point(4, 111);
      this.gvTracking.Name = "gvTracking";
      this.gvTracking.Size = new Size(312, 186);
      this.gvTracking.SortOption = GVSortOption.None;
      this.gvTracking.TabIndex = 130;
      this.gvTracking.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvTracking.UseCompatibleEditingBehavior = true;
      this.gvTracking.SubItemCheck += new GVSubItemEventHandler(this.gvTracking_SubItemCheck);
      this.lblDaysDue.AutoSize = true;
      this.lblDaysDue.Location = new Point(7, 29);
      this.lblDaysDue.Name = "lblDaysDue";
      this.lblDaysDue.Size = new Size(86, 14);
      this.lblDaysDue.TabIndex = 24;
      this.lblDaysDue.Text = "Days to Receive";
      this.txtDaysToReceive.Location = new Point(10, 45);
      this.txtDaysToReceive.Name = "txtDaysToReceive";
      this.txtDaysToReceive.Size = new Size(69, 20);
      this.txtDaysToReceive.TabIndex = 110;
      this.txtDaysToReceive.TextAlign = HorizontalAlignment.Right;
      this.txtDaysToReceive.Validated += new EventHandler(this.txtDaysToReceive_Validated);
      this.txtDateDue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDateDue.Location = new Point(77, 45);
      this.txtDateDue.Name = "txtDateDue";
      this.txtDateDue.ReadOnly = true;
      this.txtDateDue.Size = new Size(86, 20);
      this.txtDateDue.TabIndex = 115;
      this.txtDateDue.TextAlign = HorizontalAlignment.Center;
      this.lblRequestedFrom.AutoSize = true;
      this.lblRequestedFrom.Location = new Point(169, 29);
      this.lblRequestedFrom.Name = "lblRequestedFrom";
      this.lblRequestedFrom.Size = new Size(86, 14);
      this.lblRequestedFrom.TabIndex = 27;
      this.lblRequestedFrom.Text = "Requested From";
      this.txtRequestedFrom.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtRequestedFrom.Location = new Point(169, 45);
      this.txtRequestedFrom.Name = "txtRequestedFrom";
      this.txtRequestedFrom.Size = new Size(143, 20);
      this.txtRequestedFrom.TabIndex = 120;
      this.txtRequestedFrom.Validated += new EventHandler(this.txtRequestedFrom_Validated);
      this.btnAddComment.Enabled = false;
      this.btnAddComment.Location = new Point(173, 605);
      this.btnAddComment.Name = "btnAddComment";
      this.btnAddComment.Size = new Size(145, 23);
      this.btnAddComment.TabIndex = 160;
      this.btnAddComment.Text = "Add Comment";
      this.btnAddComment.UseVisualStyleBackColor = true;
      this.btnAddComment.Click += new EventHandler(this.btnAddComment_Click);
      this.panel1.Controls.Add((Control) this.commentCollection);
      this.panel1.Location = new Point(3, 301);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(320, 221);
      this.panel1.TabIndex = 45;
      this.commentCollection.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.commentCollection.CanAddComment = false;
      this.commentCollection.CanDeleteComment = false;
      this.commentCollection.CanDeliverComment = false;
      this.commentCollection.Dock = DockStyle.Fill;
      this.commentCollection.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.commentCollection.Location = new Point(0, 0);
      this.commentCollection.Name = "commentCollection";
      this.commentCollection.Size = new Size(320, 221);
      this.commentCollection.TabIndex = 140;
      this.commentCollection.TabStop = false;
      this.chkExternal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkExternal.AutoSize = true;
      this.chkExternal.Enabled = false;
      this.chkExternal.Location = new Point(7, 609);
      this.chkExternal.Name = "chkExternal";
      this.chkExternal.Size = new Size(112, 18);
      this.chkExternal.TabIndex = 150;
      this.chkExternal.Text = "External Comment";
      this.chkExternal.UseVisualStyleBackColor = true;
      this.btnRequestDocument.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRequestDocument.Location = new Point(125, 0);
      this.btnRequestDocument.Margin = new Padding(0);
      this.btnRequestDocument.Name = "btnRequestDocument";
      this.btnRequestDocument.Size = new Size(64, 22);
      this.btnRequestDocument.TabIndex = 164;
      this.btnRequestDocument.TabStop = false;
      this.btnRequestDocument.Text = "Request";
      this.tooltip.SetToolTip((Control) this.btnRequestDocument, "Send documents to borrower to sign and request needed documents");
      this.btnRequestDocument.UseVisualStyleBackColor = true;
      this.btnRequestDocument.Click += new EventHandler(this.btnRequestDocument_Click);
      this.btnRemoveDocument.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRemoveDocument.BackColor = Color.Transparent;
      this.btnRemoveDocument.Location = new Point(100, 3);
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
      this.btnEditDocument.Location = new Point(80, 3);
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
      this.btnAddDocument.Location = new Point(60, 3);
      this.btnAddDocument.Margin = new Padding(4, 3, 0, 3);
      this.btnAddDocument.MouseDownImage = (Image) null;
      this.btnAddDocument.Name = "btnAddDocument";
      this.btnAddDocument.Size = new Size(16, 16);
      this.btnAddDocument.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddDocument.TabIndex = 54;
      this.btnAddDocument.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddDocument, "Assign Document");
      this.btnAddDocument.Click += new EventHandler(this.btnAddDocument_Click);
      this.pnlRight.Controls.Add((Control) this.pnlViewer);
      this.pnlRight.Controls.Add((Control) this.csFiles);
      this.pnlRight.Controls.Add((Control) this.gcDocuments);
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(667, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(660, 638);
      this.pnlRight.TabIndex = 33;
      this.pnlViewer.Controls.Add((Control) this.fileViewer);
      this.pnlViewer.Dock = DockStyle.Fill;
      this.pnlViewer.Location = new Point(0, 133);
      this.pnlViewer.Name = "pnlViewer";
      this.pnlViewer.Size = new Size(660, 505);
      this.pnlViewer.TabIndex = 40;
      this.fileViewer.Dock = DockStyle.Fill;
      this.fileViewer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.fileViewer.Location = new Point(1, 1);
      this.fileViewer.Margin = new Padding(4, 5, 4, 5);
      this.fileViewer.Name = "fileViewer";
      this.fileViewer.Size = new Size(658, 503);
      this.fileViewer.TabIndex = 170;
      this.fileViewer.TabStop = false;
      this.csFiles.AnimationDelay = 20;
      this.csFiles.AnimationStep = 20;
      this.csFiles.BorderStyle3D = Border3DStyle.Flat;
      this.csFiles.ControlToHide = (Control) this.gcDocuments;
      this.csFiles.Dock = DockStyle.Top;
      this.csFiles.ExpandParentForm = false;
      this.csFiles.Location = new Point(0, 126);
      this.csFiles.Name = "csFiles";
      this.csFiles.TabIndex = 42;
      this.csFiles.TabStop = false;
      this.csFiles.UseAnimations = false;
      this.csFiles.VisualStyle = VisualStyles.Encompass;
      this.gcDocuments.Controls.Add((Control) this.pnlToolbar);
      this.gcDocuments.Controls.Add((Control) this.gvDocuments);
      this.gcDocuments.Dock = DockStyle.Top;
      this.gcDocuments.HeaderForeColor = SystemColors.ControlText;
      this.gcDocuments.Location = new Point(0, 0);
      this.gcDocuments.Name = "gcDocuments";
      this.gcDocuments.Size = new Size(660, 126);
      this.gcDocuments.TabIndex = 162;
      this.gcDocuments.Text = "Supporting Documents";
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnRequestDocument);
      this.pnlToolbar.Controls.Add((Control) this.separator);
      this.pnlToolbar.Controls.Add((Control) this.btnRemoveDocument);
      this.pnlToolbar.Controls.Add((Control) this.btnEditDocument);
      this.pnlToolbar.Controls.Add((Control) this.btnAddDocument);
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(467, 2);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(189, 22);
      this.pnlToolbar.TabIndex = 163;
      this.separator.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.separator.Location = new Point(120, 3);
      this.separator.Margin = new Padding(4, 3, 3, 3);
      this.separator.MaximumSize = new Size(2, 16);
      this.separator.MinimumSize = new Size(2, 16);
      this.separator.Name = "separator";
      this.separator.Size = new Size(2, 16);
      this.separator.TabIndex = 36;
      this.separator.TabStop = false;
      this.gvDocuments.BorderStyle = BorderStyle.None;
      this.gvDocuments.ClearSelectionsOnEmptyRowClick = false;
      this.gvDocuments.Dock = DockStyle.Fill;
      this.gvDocuments.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDocuments.Location = new Point(1, 26);
      this.gvDocuments.Name = "gvDocuments";
      this.gvDocuments.Size = new Size(658, 99);
      this.gvDocuments.TabIndex = 165;
      this.gvDocuments.TabStop = false;
      this.gvDocuments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvDocuments.BeforeSelectedIndexCommitted += new CancelEventHandler(this.gvDocuments_BeforeSelectedIndexCommitted);
      this.gvDocuments.SelectedIndexCommitted += new EventHandler(this.gvDocuments_SelectedIndexCommitted);
      this.gvDocuments.ItemDoubleClick += new GVItemEventHandler(this.gvDocuments_ItemDoubleClick);
      this.pnlClose.Controls.Add((Control) this.helpLink);
      this.pnlClose.Controls.Add((Control) this.btnClose);
      this.pnlClose.Dock = DockStyle.Bottom;
      this.pnlClose.Location = new Point(0, 638);
      this.pnlClose.Name = "pnlClose";
      this.pnlClose.Size = new Size(1327, 40);
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
      this.btnClose.Location = new Point(1240, 11);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 22);
      this.btnClose.TabIndex = 180;
      this.btnClose.TabStop = false;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.csLeft.AnimationDelay = 20;
      this.csLeft.AnimationStep = 20;
      this.csLeft.BorderStyle3D = Border3DStyle.Flat;
      this.csLeft.ControlToHide = (Control) this.pnlLeft;
      this.csLeft.ExpandParentForm = false;
      this.csLeft.Location = new Point(660, 0);
      this.csLeft.Name = "csDocTracking";
      this.csLeft.TabIndex = 32;
      this.csLeft.TabStop = false;
      this.csLeft.UseAnimations = false;
      this.csLeft.VisualStyle = VisualStyles.Encompass;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(1327, 678);
      this.Controls.Add((Control) this.pnlRight);
      this.Controls.Add((Control) this.csLeft);
      this.Controls.Add((Control) this.pnlLeft);
      this.Controls.Add((Control) this.pnlClose);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = Resources.icon_allsizes_bug;
      this.KeyPreview = true;
      this.Name = nameof (EnhancedDetailsDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Condition Details";
      this.Activated += new EventHandler(this.EnhancedDetailsDialog_Activated);
      this.FormClosing += new FormClosingEventHandler(this.EnhancedDetailsDialog_FormClosing);
      this.KeyDown += new KeyEventHandler(this.EnhancedDetailsDialog_KeyDown);
      this.pnlLeft.ResumeLayout(false);
      this.gcDetails.ResumeLayout(false);
      this.pnlDetails.ResumeLayout(false);
      this.pnlDetails.PerformLayout();
      this.ctrPanel.ResumeLayout(false);
      this.ctrPanel.PerformLayout();
      this.gcTracking.ResumeLayout(false);
      this.gcTracking.PerformLayout();
      ((ISupportInitialize) this.calDocumentReceiptDate).EndInit();
      this.panel1.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemoveDocument).EndInit();
      ((ISupportInitialize) this.btnEditDocument).EndInit();
      ((ISupportInitialize) this.btnAddDocument).EndInit();
      this.pnlRight.ResumeLayout(false);
      this.pnlViewer.ResumeLayout(false);
      this.gcDocuments.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      this.pnlClose.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
