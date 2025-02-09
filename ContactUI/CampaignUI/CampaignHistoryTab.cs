// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignHistoryTab
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Calendar;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class CampaignHistoryTab : Form
  {
    private const int LAST_NAME_COLUMN_INDEX = 0;
    private const int FIRST_NAME_COLUMN_INDEX = 1;
    private const int CONTACT_OWNER_COLUMN_INDEX = 2;
    private const int CAMPAIGN_STEP_COLUMN_INDEX = 3;
    private const int TASK_TYPE_COLUMN_INDEX = 4;
    private const int STATUS_COLUMN_INDEX = 5;
    private const int DATE_COLUMN_INDEX = 6;
    private const int NOTES_COLUMN_INDEX = 7;
    private CampaignData campaignData;
    private EllieMae.EMLite.Campaign.Campaign campaign;
    private ActivityStatusNameProvider activityStatusNames = new ActivityStatusNameProvider();
    private ActivityTypeNameProvider activityTypeNames = new ActivityTypeNameProvider();
    private ActivityDateSelectionNameProvider activityDateNames = new ActivityDateSelectionNameProvider();
    private SortField[] sortFields = new SortField[1]
    {
      new SortField("History.TimeOfHistory", FieldSortOrder.Descending)
    };
    private DateTime lastActivityDate = DateTime.MinValue;
    private Dictionary<int, string> activityNotes = new Dictionary<int, string>();
    private GridViewFilterManager filterMnger;
    private string firstNameFilterValue = string.Empty;
    private string lastNameFilterValue = string.Empty;
    private string campaignStepFilterValue = string.Empty;
    private string taskTypeFilterValue = string.Empty;
    private string statusFilterValue = string.Empty;
    private DateTime dateFilterValue = DateTime.MinValue;
    private ContextMenuStrip ctxCampaignHistory;
    private ToolStripMenuItem mnuItmViewContact;
    private string contactOwnerFilterValue = string.Empty;
    private DateTimePicker dtpToDate;
    private DateTimePicker dtpFromDate;
    private Label lblHistoryFor;
    private Label lblFromDate;
    private Label lblToDate;
    private Button btnOK;
    private ComboBox cboDateFilter;
    private ErrorProvider errCampaignHistory;
    private IContainer components;
    private GradientPanel pnlHistorySelection;
    private BorderPanel pnlHistoryTable;
    private GridView gvHistory;

    public CampaignHistoryTab()
    {
      this.InitializeComponent();
      this.campaignData = CampaignData.GetCampaignData();
      this.cboDateFilter.Items.Clear();
      this.cboDateFilter.Items.AddRange((object[]) this.activityDateNames.GetNames());
      this.cboDateFilter.SelectedIndex = this.cboDateFilter.FindStringExact(this.activityDateNames.GetName((object) ActivityDateSelection.ThisMonth));
      this.filterMnger = new GridViewFilterManager(Session.DefaultInstance, this.gvHistory, false);
      this.filterMnger.CreateColumnFilter(0, GridViewFilterControlType.Text);
      this.filterMnger.CreateColumnFilter(1, GridViewFilterControlType.Text);
      this.filterMnger.CreateColumnFilter(3, GridViewFilterControlType.Text);
      this.filterMnger.CreateColumnFilter(4, GridViewFilterControlType.Text);
      this.filterMnger.CreateColumnFilter(5, GridViewFilterControlType.Text);
      this.filterMnger.CreateColumnFilter(6, GridViewFilterControlType.Date);
      this.filterMnger.CreateColumnFilter(2, GridViewFilterControlType.Text);
      this.filterMnger.FilterChanged += new EventHandler(this.onColumnFilterChanged);
    }

    public void PopulateControls(bool forceRefresh)
    {
      if (!forceRefresh && this.campaign == this.campaignData.ActiveCampaign && !(this.lastActivityDate != this.campaign.LastActivityDate))
        return;
      this.campaign = this.campaignData.ActiveCampaign;
      this.lastActivityDate = this.campaign.LastActivityDate;
      this.activityNotes.Clear();
      this.getCampaignHistory();
    }

    private void onColumnFilterChanged(object sender, EventArgs e) => this.getCampaignHistory();

    private void getCampaignHistory()
    {
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        if (CampaignStatus.NotStarted == this.campaign.Status)
          return;
        CampaignHistoryCollectionCritera criteria = new CampaignHistoryCollectionCritera(this.campaign.CampaignId);
        QueryCriterion queryCriterion1 = (QueryCriterion) null;
        QueryCriterion activityDateCriteria = this.getActivityDateCriteria();
        if (activityDateCriteria != null)
          queryCriterion1 = queryCriterion1 == null ? activityDateCriteria : queryCriterion1.And(activityDateCriteria);
        QueryCriterion queryCriterion2 = this.filterMnger.ToQueryCriterion();
        if (queryCriterion2 != null)
          queryCriterion1 = queryCriterion1 == null ? queryCriterion2 : queryCriterion1.And(queryCriterion2);
        criteria.FilterCriteria = queryCriterion1;
        this.gvHistory.DataProvider = (IGVDataProvider) new CursorGVDataProvider(Session.CampaignManager.OpenCampaignHistoryCursor(criteria, this.sortFields), new PopulateGVItemEventHandler(this.historyProvider_PopulateGVItem));
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private QueryCriterion getActivityDateCriteria()
    {
      ActivityDateSelection activityDateSelection = (ActivityDateSelection) this.activityDateNames.GetValue(this.cboDateFilter.SelectedItem.ToString());
      if (ActivityDateSelection.AllDates == activityDateSelection)
        return (QueryCriterion) null;
      DateTime dateTime1 = DateTime.Today;
      DateTime dateTime2 = DateTime.Today;
      if (ActivityDateSelection.ThisWeek == activityDateSelection)
      {
        dateTime1 = DateTime.Today.AddDays((double) -(int) DateTime.Today.DayOfWeek);
        dateTime2 = dateTime1.AddDays(7.0);
      }
      else if (ActivityDateSelection.ThisMonth == activityDateSelection)
      {
        ref DateTime local1 = ref dateTime1;
        DateTime today = DateTime.Today;
        int year1 = today.Year;
        today = DateTime.Today;
        int month1 = today.Month;
        local1 = new DateTime(year1, month1, 1);
        ref DateTime local2 = ref dateTime2;
        int year2 = DateTime.Today.AddMonths(1).Year;
        DateTime dateTime3 = DateTime.Today;
        dateTime3 = dateTime3.AddMonths(1);
        int month2 = dateTime3.Month;
        local2 = new DateTime(year2, month2, 1);
      }
      else if (ActivityDateSelection.ThisYear == activityDateSelection)
      {
        dateTime1 = new DateTime(DateTime.Today.Year, 1, 1);
        dateTime2 = new DateTime(DateTime.Today.Year + 1, 1, 1);
      }
      else if (ActivityDateSelection.CustomDateRange == activityDateSelection)
      {
        dateTime1 = this.dtpFromDate.Value;
        dateTime2 = this.dtpToDate.Value.AddDays(1.0);
      }
      return new DateValueCriterion("History.TimeOfHistory", dateTime1, OrdinalMatchType.GreaterThanOrEquals).And((QueryCriterion) new DateValueCriterion("History.TimeOfHistory", dateTime2, OrdinalMatchType.LessThan));
    }

    private void viewContact()
    {
      if (1 != this.gvHistory.SelectedItems.Count)
        return;
      Cursor.Current = Cursors.WaitCursor;
      CampaignHistoryInfo tag = (CampaignHistoryInfo) this.gvHistory.SelectedItems[0].Tag;
      AttendeeInfo attendeeInfo = new AttendeeInfo();
      attendeeInfo.AssignInfo(tag.ContactId, this.campaign.ContactType);
      Cursor.Current = Cursors.Default;
      if (!attendeeInfo.WasContactFound)
        return;
      int num = (int) attendeeInfo.ShowDialog();
      if (!attendeeInfo.GoToContact)
        return;
      Session.MainScreen.NavigateToContact(attendeeInfo.SelectedContact);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void cboDateFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (ActivityDateSelection.CustomDateRange == (ActivityDateSelection) this.activityDateNames.GetValue(this.cboDateFilter.SelectedItem.ToString()))
      {
        this.dtpFromDate.Value = DateTime.Today.Date;
        this.dtpToDate.Value = DateTime.Today.Date;
        this.lblFromDate.Visible = true;
        this.dtpFromDate.Visible = true;
        this.lblToDate.Visible = true;
        this.dtpToDate.Visible = true;
        this.btnOK.Visible = true;
      }
      else
      {
        this.lblFromDate.Visible = false;
        this.dtpFromDate.Visible = false;
        this.lblToDate.Visible = false;
        this.dtpToDate.Visible = false;
        this.btnOK.Visible = false;
        if (this.campaign == null)
          return;
        this.getCampaignHistory();
      }
    }

    private void dtpFromDate_ValueChanged(object sender, EventArgs e)
    {
      if (this.dtpToDate.Value < this.dtpFromDate.Value)
        this.dtpToDate.Value = this.dtpFromDate.Value;
      this.errCampaignHistory.SetError((Control) this.dtpToDate, "");
      this.btnOK.Enabled = true;
    }

    private void dtpToDate_ValueChanged(object sender, EventArgs e)
    {
      if (this.dtpToDate.Value < this.dtpFromDate.Value)
      {
        this.errCampaignHistory.SetError((Control) this.dtpToDate, "To date cannot be less than From date.");
        this.btnOK.Enabled = false;
      }
      else
      {
        this.errCampaignHistory.SetError((Control) this.dtpToDate, "");
        this.btnOK.Enabled = true;
      }
    }

    private void btnOK_Click(object sender, EventArgs e) => this.getCampaignHistory();

    private void gvHistory_MouseDown(object sender, MouseEventArgs e)
    {
      if (MouseButtons.Right != e.Button)
        return;
      GVItem itemAt = this.gvHistory.GetItemAt(e.X, e.Y);
      if (itemAt != null)
        itemAt.Selected = true;
      else
        this.gvHistory.SelectedItems.Clear();
    }

    private void ctxCampaignHistory_Opening(object sender, CancelEventArgs e)
    {
      this.mnuItmViewContact.Enabled = 1 == this.gvHistory.SelectedItems.Count;
    }

    private void mnuItmViewContact_Click(object sender, EventArgs e) => this.viewContact();

    private void txtLastNameFilter_LostFocus(object sender, EventArgs e)
    {
      string strB = ((Control) sender).Text.Trim();
      if (string.Compare(this.lastNameFilterValue, strB, true) == 0)
        return;
      this.lastNameFilterValue = strB;
      this.getCampaignHistory();
    }

    private void txtFirstNameFilter_LostFocus(object sender, EventArgs e)
    {
      string strB = ((Control) sender).Text.Trim();
      if (string.Compare(this.firstNameFilterValue, strB, true) == 0)
        return;
      this.firstNameFilterValue = strB;
      this.getCampaignHistory();
    }

    private void txtCampaignStepFilter_LostFocus(object sender, EventArgs e)
    {
      string strB = ((Control) sender).Text.Trim();
      if (string.Compare(this.campaignStepFilterValue, strB, true) == 0)
        return;
      this.campaignStepFilterValue = strB;
      this.getCampaignHistory();
    }

    private void txtTaskTypeFilter_LostFocus(object sender, EventArgs e)
    {
      string strB = ((Control) sender).Text.Trim();
      if (string.Compare(this.taskTypeFilterValue, strB, true) == 0)
        return;
      this.taskTypeFilterValue = strB;
      this.getCampaignHistory();
    }

    private void txtStatusFilter_LostFocus(object sender, EventArgs e)
    {
      string strB = ((Control) sender).Text.Trim();
      if (string.Compare(this.statusFilterValue, strB, true) == 0)
        return;
      this.statusFilterValue = strB;
      this.getCampaignHistory();
    }

    private void dpDateFilter_ValueChanged(object sender, EventArgs e)
    {
      DateTime dateTime = ((DatePicker) sender).Value;
      if (!(this.dateFilterValue != dateTime))
        return;
      this.dateFilterValue = dateTime;
      this.getCampaignHistory();
    }

    private void txtContactOwnerFilter_LostFocus(object sender, EventArgs e)
    {
      string strB = ((Control) sender).Text.Trim();
      if (string.Compare(this.contactOwnerFilterValue, strB, true) == 0)
        return;
      this.contactOwnerFilterValue = strB;
      this.getCampaignHistory();
    }

    private void historyProvider_PopulateGVItem(object sender, PopulateGVItemEventArgs e)
    {
      if (e.DataItem == null)
        return;
      CampaignHistoryInfo dataItem = (CampaignHistoryInfo) e.DataItem;
      e.ListItem.SubItems[0].Text = dataItem.LastName;
      e.ListItem.SubItems[1].Text = dataItem.FirstName;
      e.ListItem.SubItems[3].Text = string.Format("{0:###}. {1}", (object) dataItem.StepNumber, (object) dataItem.StepName);
      e.ListItem.SubItems[4].Text = dataItem.ActivityType;
      e.ListItem.SubItems[5].Text = dataItem.ActivityStatus;
      e.ListItem.SubItems[6].Text = dataItem.CompletedDate.ToShortDateString();
      e.ListItem.SubItems[7].Value = (object) null;
      if (dataItem.NoteId != 0)
      {
        ImageLink imageLink = new ImageLink((Element) null, (Image) Resources.notes, (Image) Resources.notes_over, new EventHandler(this.lnkNotes_Click));
        imageLink.Tag = (object) dataItem;
        e.ListItem.SubItems[6].Value = (object) imageLink;
      }
      e.ListItem.SubItems[2].Text = dataItem.ContactOwner;
      e.ListItem.Tag = (object) dataItem;
    }

    private void gvHistory_SortItems(object source, GVColumnSortEventArgs e)
    {
      List<SortField> sortFieldList = new List<SortField>();
      for (int index = 0; index < e.ColumnSorts.Length; ++index)
      {
        GVColumn column = this.gvHistory.Columns[e.ColumnSorts[index].Column];
        FieldSortOrder sortOrder = e.ColumnSorts[index].SortOrder == SortOrder.Ascending ? FieldSortOrder.Ascending : FieldSortOrder.Descending;
        if (string.Concat(column.Tag) == "Notes")
          sortFieldList.Add(new SortField((IQueryTerm) new DataExpression("", "(case {0} when NULL then 0 else 1 end)", new string[1]
          {
            "History.NoteId"
          }), sortOrder));
        else if (string.Concat(column.Tag) == "History.CampaignStepDescription")
        {
          sortFieldList.Add(new SortField("History.CampaignStepNumber", sortOrder));
          sortFieldList.Add(new SortField("History.CampaignStepName", sortOrder));
        }
        else
          sortFieldList.Add(new SortField(string.Concat(column.Tag), sortOrder));
      }
      this.sortFields = sortFieldList.ToArray();
      this.getCampaignHistory();
    }

    private void lnkNotes_Click(object sender, EventArgs e)
    {
      if (!(sender is ImageLink imageLink) || !(imageLink.Tag is CampaignHistoryInfo tag))
        return;
      if (!this.activityNotes.ContainsKey(tag.NoteId))
      {
        ContactHistoryNoteInfo contactHistoryNote = Session.ContactManager.GetContactHistoryNote(tag.NoteId);
        if (contactHistoryNote != null)
          this.activityNotes.Add(contactHistoryNote.NoteId, contactHistoryNote.Note);
      }
      string activityNote = this.activityNotes[tag.NoteId];
      if (string.IsNullOrEmpty(activityNote))
        return;
      using (ActivityNoteDialog activityNoteDialog = new ActivityNoteDialog())
      {
        activityNoteDialog.Note = activityNote;
        int num = (int) activityNoteDialog.ShowDialog();
      }
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
      this.errCampaignHistory = new ErrorProvider(this.components);
      this.pnlHistoryTable = new BorderPanel();
      this.gvHistory = new GridView();
      this.ctxCampaignHistory = new ContextMenuStrip(this.components);
      this.mnuItmViewContact = new ToolStripMenuItem();
      this.pnlHistorySelection = new GradientPanel();
      this.cboDateFilter = new ComboBox();
      this.lblHistoryFor = new Label();
      this.btnOK = new Button();
      this.lblToDate = new Label();
      this.dtpToDate = new DateTimePicker();
      this.lblFromDate = new Label();
      this.dtpFromDate = new DateTimePicker();
      ((ISupportInitialize) this.errCampaignHistory).BeginInit();
      this.pnlHistoryTable.SuspendLayout();
      this.ctxCampaignHistory.SuspendLayout();
      this.pnlHistorySelection.SuspendLayout();
      this.SuspendLayout();
      this.errCampaignHistory.ContainerControl = (ContainerControl) this;
      this.pnlHistoryTable.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlHistoryTable.Controls.Add((Control) this.gvHistory);
      this.pnlHistoryTable.Dock = DockStyle.Fill;
      this.pnlHistoryTable.Location = new Point(0, 26);
      this.pnlHistoryTable.Name = "pnlHistoryTable";
      this.pnlHistoryTable.Size = new Size(784, 396);
      this.pnlHistoryTable.TabIndex = 14;
      this.gvHistory.AllowMultiselect = false;
      this.gvHistory.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "LastName";
      gvColumn1.Tag = (object) "Contact.LastName";
      gvColumn1.Text = "Last Name";
      gvColumn1.Width = 125;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "FirstName";
      gvColumn2.Tag = (object) "Contact.FirstName";
      gvColumn2.Text = "First Name";
      gvColumn2.Width = 125;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ContactOwner";
      gvColumn3.Tag = (object) "History.Sender";
      gvColumn3.Text = "Contact Owner";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "CampaignStep";
      gvColumn4.Tag = (object) "History.CampaignStepDescription";
      gvColumn4.Text = "Campaign Step";
      gvColumn4.Width = 250;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "TaskType";
      gvColumn5.Tag = (object) "History.EventType";
      gvColumn5.Text = "Task Type";
      gvColumn5.Width = 75;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Status";
      gvColumn6.Tag = (object) "History.CampaignActivityStatus";
      gvColumn6.Text = "Status";
      gvColumn6.Width = 118;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Date";
      gvColumn7.SortMethod = GVSortMethod.Date;
      gvColumn7.Tag = (object) "History.TimeOfHistory";
      gvColumn7.Text = "Date";
      gvColumn7.Width = 90;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Notes";
      gvColumn8.Tag = (object) "Notes";
      gvColumn8.Text = "Notes";
      gvColumn8.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn8.Width = 75;
      this.gvHistory.Columns.AddRange(new GVColumn[8]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8
      });
      this.gvHistory.ContextMenuStrip = this.ctxCampaignHistory;
      this.gvHistory.Dock = DockStyle.Fill;
      this.gvHistory.FilterVisible = true;
      this.gvHistory.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gvHistory.Location = new Point(1, 0);
      this.gvHistory.Name = "gvHistory";
      this.gvHistory.Size = new Size(782, 395);
      this.gvHistory.SortOption = GVSortOption.Owner;
      this.gvHistory.TabIndex = 0;
      this.gvHistory.SortItems += new GVColumnSortEventHandler(this.gvHistory_SortItems);
      this.gvHistory.MouseDown += new MouseEventHandler(this.gvHistory_MouseDown);
      this.gvHistory.ItemDoubleClick += new GVItemEventHandler(this.gvHistory_ItemDoubleClick);
      this.ctxCampaignHistory.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.mnuItmViewContact
      });
      this.ctxCampaignHistory.Name = "ctxHistory";
      this.ctxCampaignHistory.Size = new Size(108, 26);
      this.ctxCampaignHistory.Opening += new CancelEventHandler(this.ctxCampaignHistory_Opening);
      this.mnuItmViewContact.Name = "mnuItmViewContact";
      this.mnuItmViewContact.Size = new Size(107, 22);
      this.mnuItmViewContact.Text = "View";
      this.mnuItmViewContact.Click += new EventHandler(this.mnuItmViewContact_Click);
      this.pnlHistorySelection.Controls.Add((Control) this.cboDateFilter);
      this.pnlHistorySelection.Controls.Add((Control) this.lblHistoryFor);
      this.pnlHistorySelection.Controls.Add((Control) this.btnOK);
      this.pnlHistorySelection.Controls.Add((Control) this.lblToDate);
      this.pnlHistorySelection.Controls.Add((Control) this.dtpToDate);
      this.pnlHistorySelection.Controls.Add((Control) this.lblFromDate);
      this.pnlHistorySelection.Controls.Add((Control) this.dtpFromDate);
      this.pnlHistorySelection.Dock = DockStyle.Top;
      this.pnlHistorySelection.Location = new Point(0, 0);
      this.pnlHistorySelection.Name = "pnlHistorySelection";
      this.pnlHistorySelection.Size = new Size(784, 26);
      this.pnlHistorySelection.Style = GradientPanel.PanelStyle.TableHeader;
      this.pnlHistorySelection.TabIndex = 13;
      this.cboDateFilter.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboDateFilter.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboDateFilter.Location = new Point(76, 2);
      this.cboDateFilter.Name = "cboDateFilter";
      this.cboDateFilter.Size = new Size(128, 22);
      this.cboDateFilter.TabIndex = 12;
      this.cboDateFilter.SelectedIndexChanged += new EventHandler(this.cboDateFilter_SelectedIndexChanged);
      this.lblHistoryFor.AutoSize = true;
      this.lblHistoryFor.BackColor = Color.Transparent;
      this.lblHistoryFor.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblHistoryFor.Location = new Point(10, 6);
      this.lblHistoryFor.Name = "lblHistoryFor";
      this.lblHistoryFor.Size = new Size(65, 14);
      this.lblHistoryFor.TabIndex = 4;
      this.lblHistoryFor.Text = "History for";
      this.lblHistoryFor.TextAlign = ContentAlignment.MiddleLeft;
      this.btnOK.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnOK.Location = new Point(481, 3);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(30, 20);
      this.btnOK.TabIndex = 11;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.lblToDate.AutoSize = true;
      this.lblToDate.BackColor = Color.Transparent;
      this.lblToDate.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblToDate.Location = new Point(351, 6);
      this.lblToDate.Name = "lblToDate";
      this.lblToDate.Size = new Size(19, 14);
      this.lblToDate.TabIndex = 7;
      this.lblToDate.Text = "To";
      this.lblToDate.TextAlign = ContentAlignment.MiddleLeft;
      this.dtpToDate.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dtpToDate.Format = DateTimePickerFormat.Short;
      this.dtpToDate.Location = new Point(379, 3);
      this.dtpToDate.Name = "dtpToDate";
      this.dtpToDate.Size = new Size(92, 20);
      this.dtpToDate.TabIndex = 9;
      this.dtpToDate.ValueChanged += new EventHandler(this.dtpToDate_ValueChanged);
      this.lblFromDate.AutoSize = true;
      this.lblFromDate.BackColor = Color.Transparent;
      this.lblFromDate.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblFromDate.Location = new Point(212, 6);
      this.lblFromDate.Name = "lblFromDate";
      this.lblFromDate.Size = new Size(31, 14);
      this.lblFromDate.TabIndex = 6;
      this.lblFromDate.Text = "From";
      this.lblFromDate.TextAlign = ContentAlignment.MiddleLeft;
      this.dtpFromDate.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dtpFromDate.Format = DateTimePickerFormat.Short;
      this.dtpFromDate.Location = new Point(249, 3);
      this.dtpFromDate.Name = "dtpFromDate";
      this.dtpFromDate.Size = new Size(92, 20);
      this.dtpFromDate.TabIndex = 8;
      this.dtpFromDate.ValueChanged += new EventHandler(this.dtpFromDate_ValueChanged);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(784, 422);
      this.ControlBox = false;
      this.Controls.Add((Control) this.pnlHistoryTable);
      this.Controls.Add((Control) this.pnlHistorySelection);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CampaignHistoryTab);
      ((ISupportInitialize) this.errCampaignHistory).EndInit();
      this.pnlHistoryTable.ResumeLayout(false);
      this.ctxCampaignHistory.ResumeLayout(false);
      this.pnlHistorySelection.ResumeLayout(false);
      this.pnlHistorySelection.PerformLayout();
      this.ResumeLayout(false);
    }

    private void gvHistory_ItemDoubleClick(object source, GVItemEventArgs e) => this.viewContact();
  }
}
