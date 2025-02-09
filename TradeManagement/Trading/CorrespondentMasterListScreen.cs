// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentMasterListScreen
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class CorrespondentMasterListScreen : UserControl, IMenuProvider
  {
    private string standardViewName = "Standard View";
    private FileSystemEntry fsViewEntry;
    private FieldFilterList advFilter;
    private GridViewLayoutManager gvLayoutManager;
    private GridViewReportFilterManager gvFilterManager;
    private CorrespondentMasterReportFieldDefs correspondentMasterFieldDefs;
    private TradeView currentView;
    private bool suspendEvents;
    private static string className = nameof (CorrespondentMasterListScreen);
    private static string sw = Tracing.SwContact;
    private bool suspendRefresh;
    private ICursor corrMasterCursor;
    private TableLayout mainTableLayout;
    private IContainer components;
    private GroupContainer grpContracts;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button btnActivate;
    private Button btnArchive;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton btnDelete;
    private StandardIconButton btnNew;
    private Label label1;
    private ToolTip toolTip1;
    private GridView gvMain;
    private ComboBoxEx cBoxView;
    private PageListNavigator navPage;
    private GradientPanel gradientPanel1;
    private StandardIconButton btnSaveView;
    private StandardIconButton btnEditView;
    private StandardIconButton btnRefreshView;
    private GradientPanel gradientPanel2;
    private Label label2;
    private Label lblFilter;
    private Button btnClearSearch;
    private Button btnAdvSearch;
    private StandardIconButton siBtnRefresh;
    private StandardIconButton siBtnPrint;
    private StandardIconButton siBtnExcel;
    private Label label3;
    private ComboBox cboFilterView;

    public bool IsReadOnly { get; set; }

    public CorrespondentMasterListScreen()
    {
      this.InitializeComponent();
      this.init();
    }

    private void init()
    {
      this.resetFieldDefs();
      this.loadViewList(Session.ConfigurationManager.GetTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.CorrespondentMasterView, FileSystemEntry.PrivateRoot(Session.UserID)));
      string privateProfileString = Session.GetPrivateProfileString("CorrespondentMaster", "DefaultView");
      if (privateProfileString != "")
      {
        if (privateProfileString != this.standardViewName && ClientCommonUtils.ItemExistInDropDown((ComboBox) this.cBoxView, (object) new FileSystemEntryListItem(FileSystemEntry.Parse(privateProfileString))))
          ClientCommonUtils.PopulateDropdown((ComboBox) this.cBoxView, (object) new FileSystemEntryListItem(FileSystemEntry.Parse(privateProfileString)), false);
        else
          ClientCommonUtils.PopulateDropdown((ComboBox) this.cBoxView, (object) new FileSystemEntryListItem(FileSystemEntry.Parse("personal:\\" + this.standardViewName, Session.UserID)), false);
      }
      else
        ClientCommonUtils.PopulateDropdown((ComboBox) this.cBoxView, (object) new FileSystemEntryListItem(FileSystemEntry.Parse("personal:\\" + this.standardViewName, Session.UserID)), false);
      this.cboFilterView.SelectedIndex = 0;
    }

    public void SetReadOnly()
    {
      this.btnNew.Enabled = false;
      this.btnEditView.Enabled = false;
      this.btnDelete.Enabled = false;
      this.btnArchive.Enabled = false;
    }

    private void resetFieldDefs()
    {
      this.correspondentMasterFieldDefs = CorrespondentMasterReportFieldDefs.GetFieldDefs();
    }

    private void loadViewList(FileSystemEntry[] fsEntries)
    {
      this.suspendEvents = true;
      try
      {
        this.cBoxView.Items.Clear();
        this.cBoxView.Dividers.Clear();
        foreach (FileSystemEntry fsEntry in fsEntries)
          this.cBoxView.Items.Add((object) new FileSystemEntryListItem(fsEntry));
        this.cBoxView.Dividers.Add(this.cBoxView.Items.Count);
        this.cBoxView.Items.Add((object) new FileSystemEntryListItem(new FileSystemEntry("\\" + this.standardViewName, FileSystemEntry.Types.File, Session.UserID)));
        if (this.currentView == null)
          return;
        ClientCommonUtils.PopulateDropdown((ComboBox) this.cBoxView, (object) new FileSystemEntryListItem(new FileSystemEntry("\\" + this.currentView.Name, FileSystemEntry.Types.File, Session.UserID)), false);
      }
      catch (Exception ex)
      {
        Tracing.Log(CorrespondentMasterListScreen.sw, CorrespondentMasterListScreen.className, TraceLevel.Error, "Error loading view list: " + (object) ex);
        ErrorDialog.Display(ex);
      }
      finally
      {
        this.suspendEvents = false;
      }
    }

    public void RefreshContents()
    {
      if (this.gvLayoutManager == null)
        this.init();
      this.RefreshList(true);
    }

    private void newCorrespondentMaster(CorrespondentMasterInfo correspondent)
    {
      TradeManagementConsole.Instance.OpenCorrespondentMaster(correspondent);
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      CorrespondentMasterInfo correspondentMasterInfo = new CorrespondentMasterInfo();
      using (NewCorrespondentMaster correspondentMaster = new NewCorrespondentMaster())
      {
        if (correspondentMaster.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.newCorrespondentMaster(correspondentMaster.info);
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.gvMain.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more Master Commitments to be deleted.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        try
        {
          bool flag = true;
          for (int index = 0; index < this.gvMain.SelectedItems.Count; ++index)
          {
            if (((IEnumerable<CorrespondentTradeViewModel>) Session.SessionObjects.CorrespondentTradeManager.GetTradeViewsByMasterId(((CorrespondentMasterViewModel) this.gvMain.SelectedItems[index].Tag).CorrespondentMasterID)).Count<CorrespondentTradeViewModel>() > 0)
            {
              flag = false;
              break;
            }
          }
          if (!flag)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "To delete the Master Commitment(s), loans and correspondent trades assigned to the master commitment(s) must be unassigned first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        catch (Exception ex)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "An unexpected error has occurred while attempting to delete the Master Commitment(s): " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        if (Utils.Dialog((IWin32Window) this, "The selected Master Commitment(s) and all associated Delivery Types will be permanently deleted. Are you sure you want to proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          return;
        Cursor.Current = Cursors.WaitCursor;
        try
        {
          CorrespondentMasterViewModel[] state = new CorrespondentMasterViewModel[this.gvMain.SelectedItems.Count];
          for (int index = 0; index < this.gvMain.SelectedItems.Count; ++index)
            state[index] = (CorrespondentMasterViewModel) this.gvMain.SelectedItems[index].Tag;
          using (ProgressDialog progressDialog = new ProgressDialog("Deleting Master Commitment", new AsynchronousProcess(this.deleteTradesAsync), (object) state, true))
          {
            int num4 = (int) progressDialog.ShowDialog();
          }
        }
        catch (Exception ex)
        {
          int num5 = (int) Utils.Dialog((IWin32Window) this, "An unexpected error has occurred while attempting to delete the Master Commitment(s): " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        this.RefreshContents();
        this.RefreshList(false);
        Cursor.Current = Cursors.Default;
      }
    }

    private DialogResult deleteTradesAsync(object tradesAsObj, IProgressFeedback feedback)
    {
      try
      {
        CorrespondentMasterViewModel[] correspondentMasterViewModelArray = (CorrespondentMasterViewModel[]) tradesAsObj;
        int num1 = 0;
        feedback.ResetCounter(correspondentMasterViewModelArray.Length);
        foreach (CorrespondentMasterViewModel info in correspondentMasterViewModelArray)
        {
          if (feedback.Cancel)
            return DialogResult.Cancel;
          switch (this.deleteTrade(info, feedback))
          {
            case DialogResult.OK:
              ++num1;
              break;
            case DialogResult.Cancel:
              return DialogResult.Cancel;
          }
        }
        if (num1 == 1)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "1 Master Commitment trade has been successfully deleted.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, num1.ToString() + " Master Commitment have been successfully deleted.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        Tracing.Log(CorrespondentMasterListScreen.sw, CorrespondentMasterListScreen.className, TraceLevel.Error, "Error deleting Master Commitment: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "An unexpected error occurred while attempting to delete the selected Master Commitment(s): " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return DialogResult.Abort;
      }
    }

    private DialogResult deleteTrade(CorrespondentMasterViewModel info, IProgressFeedback feedback)
    {
      try
      {
        Session.CorrespondentMasterManager.DeleteCorrespondentMaster(info.CorrespondentMasterID);
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        Tracing.Log(CorrespondentMasterListScreen.sw, CorrespondentMasterListScreen.className, TraceLevel.Error, "Error processing Master Commitment " + (object) info.CorrespondentMasterID + ": " + (object) ex);
        return Utils.Dialog((IWin32Window) this, "An unexpected error occurred while attempting to delete the  Master Commitment " + info.ContractNumber + ": " + ex.Message, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel ? DialogResult.Cancel : DialogResult.Abort;
      }
    }

    private void btnArchive_Click(object sender, EventArgs e)
    {
      if (this.gvMain.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more correspondent master commitments to be archived.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to archive the selected Correspondent Master Commitment?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          return;
        try
        {
          Cursor.Current = Cursors.WaitCursor;
          IEnumerable<int> uniqueCmcid = this.findUniqueCMCID(this.gvMain.SelectedItems);
          if (!Session.CorrespondentMasterManager.IsAllLoanPurcahsedInMaster(uniqueCmcid.ToArray<int>()))
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "The loans associated with this Correspondent Master Commitment must be purchased before archiving.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
          foreach (int masterId in uniqueCmcid)
          {
            CorrespondentMasterInfo correspondentMaster = Session.CorrespondentMasterManager.GetCorrespondentMaster(masterId);
            if (correspondentMaster.Status != MasterCommitmentStatus.Archived)
            {
              correspondentMaster.Status = MasterCommitmentStatus.Archived;
              Session.CorrespondentMasterManager.SetCorrespondentMasterStatus(masterId, MasterCommitmentStatus.Archived);
            }
          }
        }
        catch (Exception ex)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to update a commitment: " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        this.RefreshList(false);
        Cursor.Current = Cursors.Default;
      }
    }

    private void btnActivate_Click(object sender, EventArgs e)
    {
      if (this.gvMain.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more commitments to be activated.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to activate the selected commitment(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          return;
        try
        {
          Cursor.Current = Cursors.WaitCursor;
          foreach (int masterId in this.findUniqueCMCID(this.gvMain.SelectedItems))
          {
            if (Session.CorrespondentMasterManager.GetCorrespondentMaster(masterId).Status != MasterCommitmentStatus.Active)
              Session.CorrespondentMasterManager.SetCorrespondentMasterStatus(masterId, MasterCommitmentStatus.Active);
          }
        }
        catch (Exception ex)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to update a commitment: " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        this.RefreshList(false);
        Cursor.Current = Cursors.Default;
      }
    }

    public void MenuClicked(ToolStripItem menuItem)
    {
      switch (string.Concat(menuItem.Tag))
      {
        case "CMC_New":
          this.btnNew_Click((object) null, (EventArgs) null);
          break;
        case "CMC_Delete":
          this.btnDelete_Click((object) null, (EventArgs) null);
          break;
        case "CMC_Export":
          this.siBtnExcel_Click((object) null, (EventArgs) null);
          break;
        case "CMC_Print":
          this.siBtnPrint_Click((object) null, (EventArgs) null);
          break;
        case "CMC_Archive":
          this.btnArchive_Click((object) null, (EventArgs) null);
          break;
        case "CMC_NewTrade":
          this.OpenNewCorrespondentTrade();
          break;
      }
    }

    private void OpenNewCorrespondentTrade()
    {
      CorrespondentTradeInfo correspondentTradeInfo = new CorrespondentTradeInfo();
      using (NewCorrespondentTrade correspondentTrade = new NewCorrespondentTrade())
      {
        if (correspondentTrade.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        CorrespondentTradeInfo info = correspondentTrade.info;
        TradeManagementConsole.Instance.OpenCorrespondentTrade(info);
      }
    }

    public bool SetMenuItemState(ToolStripItem menuItem)
    {
      Control stateControl = (Control) null;
      switch (string.Concat(menuItem.Tag))
      {
        case "CMC_New":
          stateControl = (Control) this.btnNew;
          break;
        case "CMC_Delete":
          stateControl = (Control) this.btnDelete;
          break;
        case "CMC_Export":
          stateControl = (Control) this.siBtnExcel;
          break;
        case "CMC_Print":
          stateControl = (Control) this.siBtnPrint;
          break;
        case "CMC_Archive":
          stateControl = (Control) this.btnArchive;
          break;
        case "CMC_NewTrade":
          menuItem.Enabled = this.gvMain.SelectedItems.Count == 1;
          break;
      }
      if (stateControl == null)
        return true;
      ClientCommonUtils.ApplyControlStateToMenu(menuItem, stateControl);
      return stateControl.Visible;
    }

    public void SetCurrentView(FileSystemEntry fsEntry)
    {
      try
      {
        if (fsEntry.Name == this.standardViewName)
        {
          TradeView defaultCommitmentView = this.getDefaultCommitmentView();
          this.fsViewEntry = fsEntry;
          this.setCurrentView(defaultCommitmentView);
        }
        else
        {
          TradeView templateSettings = (TradeView) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.CorrespondentMasterView, fsEntry);
          if (templateSettings == null)
            throw new ArgumentException();
          this.btnEditView.Enabled = !fsEntry.IsPublic && !this.IsReadOnly;
          this.fsViewEntry = fsEntry;
          this.setCurrentView(templateSettings);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(CorrespondentMasterListScreen.sw, CorrespondentMasterListScreen.className, TraceLevel.Error, "Error opening view: " + (object) ex);
        ErrorDialog.Display(ex);
      }
    }

    private TradeView getDefaultCommitmentView()
    {
      TradeView defaultCommitmentView = new TradeView(this.standardViewName);
      TableLayout tableLayout = new TableLayout();
      tableLayout.AddColumn(new TableLayout.Column("CorrespondentMaster.ContractNumber", "Master Commitment #", HorizontalAlignment.Left, 110));
      tableLayout.AddColumn(new TableLayout.Column("CorrespondentMaster.CompanyName", "Company Name", HorizontalAlignment.Left, 90));
      tableLayout.AddColumn(new TableLayout.Column("CorrespondentMaster.ExternalID", "TPO ID", HorizontalAlignment.Left, 90));
      tableLayout.AddColumn(new TableLayout.Column("CorrespondentMaster.OrganizationID", "Organization ID", HorizontalAlignment.Left, 100));
      tableLayout.AddColumn(new TableLayout.Column("CorrespondentMaster.CommitmentAmount", "Master Commitment Amount", HorizontalAlignment.Right, 150));
      tableLayout.AddColumn(new TableLayout.Column("CorrespondentMaster.CommitmentType", "Master Commitment Type", HorizontalAlignment.Center, 150));
      tableLayout.AddColumn(new TableLayout.Column("CorrespondentMasterDeliveryMethod.DeliveryType", "Delivery Type", HorizontalAlignment.Left, 100));
      tableLayout.AddColumn(new TableLayout.Column("CorrespondentMasterDeliveryMethod.DeliveryDays", "Delivery Days", HorizontalAlignment.Left, 100));
      tableLayout.AddColumn(new TableLayout.Column("CorrespondentMasterDeliveryMethod.Tolerance", "Tolerance %", HorizontalAlignment.Left, 100));
      tableLayout.AddColumn(new TableLayout.Column("CorrespondentMasterDeliveryMethod.EffectiveDate", "Delivery Effective Date", HorizontalAlignment.Center, 100));
      tableLayout.AddColumn(new TableLayout.Column("CorrespondentMasterDeliveryMethod.ExpirationDate", "Delivery Expire Date", HorizontalAlignment.Center, 100));
      tableLayout.AddColumn(new TableLayout.Column("CorrespondentMaster.MasterEffectiveDate", "Master Effective Date", HorizontalAlignment.Center, 100));
      tableLayout.AddColumn(new TableLayout.Column("CorrespondentMaster.MasterExpirationDate", "Master Expiration Date", HorizontalAlignment.Center, 100));
      defaultCommitmentView.Layout = tableLayout;
      return defaultCommitmentView;
    }

    private void setCurrentView(TradeView view)
    {
      this.currentView = view;
      this.suspendEvents = true;
      this.applyTableLayout(view.Layout);
      this.cboFilterView.SelectedIndex = view.ViewActive ? 0 : 1;
      this.advFilter = view.Filter;
      this.gvFilterManager.ClearColumnFilters();
      this.setViewChanged(false);
      this.suspendEvents = false;
      this.SetCurrentFilter(view.Filter);
      this.btnSaveView.Enabled = false;
      this.btnRefreshView.Enabled = false;
    }

    private void setViewChanged(bool modified)
    {
      this.btnSaveView.Enabled = modified;
      this.btnRefreshView.Enabled = modified;
    }

    public void SetCurrentFilter(FieldFilterList filter)
    {
      this.advFilter = filter;
      this.gvFilterManager.ClearColumnFilters();
      this.refreshFilterDescription();
      this.RefreshList(false);
      this.setViewChanged(true);
    }

    public FieldFilterList GetCurrentFilter()
    {
      FieldFilterList fieldFilterList = this.gvFilterManager.ToFieldFilterList();
      if (this.advFilter != null)
        fieldFilterList.AddRange((IEnumerable<FieldFilter>) this.advFilter);
      return fieldFilterList;
    }

    private void refreshFilterDescription()
    {
      FieldFilterList currentFilter = this.GetCurrentFilter();
      if (currentFilter.Count == 0)
      {
        this.lblFilter.Text = "None";
        this.btnClearSearch.Enabled = false;
      }
      else
      {
        this.lblFilter.Text = currentFilter.ToString(true);
        this.btnClearSearch.Enabled = true;
      }
    }

    public void RefreshList(bool preserveSelections)
    {
      if (this.suspendRefresh)
        return;
      using (CursorActivator.Wait())
      {
        using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("CorrespondentMaster.Refresh", "Refresh the Correspondent Master Commitment screen data", true, 577, nameof (RefreshList), "D:\\ws\\24.3.0.0\\EmLite\\TradeManagement\\Trading\\CorrespondentMasterListScreen.cs"))
        {
          this.retrieveCorrespondentMasterData();
          this.displayCurrentPage(preserveSelections);
          this.refreshFilterDescription();
          performanceMeter.AddVariable("ContactCount", (object) this.navPage.NumberOfItems);
          performanceMeter.AddVariable("Columns", (object) this.gvMain.Columns.Count);
          performanceMeter.AddVariable("Filter", (object) this.lblFilter.Text);
          GVColumnSort[] sortOrder = this.gvMain.Columns.GetSortOrder();
          if (sortOrder.Length != 0)
            performanceMeter.AddVariable("Sort", this.gvMain.Columns[sortOrder[0].Column].Tag);
        }
      }
      if (!this.IsReadOnly)
        return;
      this.SetReadOnly();
    }

    private void displayCurrentPage(bool preserveSelections)
    {
      int currentPageItemIndex = this.navPage.CurrentPageItemIndex;
      int currentPageItemCount = this.navPage.CurrentPageItemCount;
      CorrespondentMasterViewModel[] correspondentMasterViewModelArray = new CorrespondentMasterViewModel[0];
      if (currentPageItemCount > 0)
        correspondentMasterViewModelArray = (CorrespondentMasterViewModel[]) this.corrMasterCursor.GetItems(currentPageItemIndex, currentPageItemCount);
      List<CorrespondentMasterViewModelID> masterViewModelIdList = new List<CorrespondentMasterViewModelID>();
      if (preserveSelections)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvMain.Items)
        {
          if (gvItem.Selected)
            masterViewModelIdList.Add(((CorrespondentMasterViewModel) gvItem.Tag).GetID());
        }
      }
      this.gvMain.Items.Clear();
      for (int index = 0; index < correspondentMasterViewModelArray.Length; ++index)
      {
        GVItem gvItem = this.createGVItem(correspondentMasterViewModelArray[index]);
        this.gvMain.Items.Add(gvItem);
        if (masterViewModelIdList.Contains(correspondentMasterViewModelArray[index].GetID()))
          gvItem.Selected = true;
      }
      if (this.gvMain.Items.Count <= 0 || this.gvMain.SelectedItems.Count != 0)
        return;
      this.gvMain.Items[0].Selected = true;
    }

    private GVItem createGVItem(CorrespondentMasterViewModel correspondentMaster)
    {
      GVItem gvItem = new GVItem();
      gvItem.Tag = (object) correspondentMaster;
      for (int index = 0; index < this.gvMain.Columns.Count; ++index)
      {
        string columnId = ((TableLayout.Column) this.gvMain.Columns[index].Tag).ColumnID;
        object obj = (object) null;
        CorrespondentMasterReportFieldDef fieldByCriterionName = this.correspondentMasterFieldDefs.GetFieldByCriterionName(columnId);
        if (fieldByCriterionName != null && correspondentMaster[columnId] != null)
          obj = fieldByCriterionName.ToDisplayElement(columnId, (IPropertyDictionary) correspondentMaster, (EventHandler) null);
        gvItem.SubItems[index].Value = obj;
      }
      return gvItem;
    }

    private void retrieveCorrespondentMasterData()
    {
      this.retrieveCorrespondentMasterData((QueryCriterion) null, (SortField[]) null);
    }

    private void retrieveCorrespondentMasterData(QueryCriterion filter, SortField[] sortFields)
    {
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        if (this.gvMain.SelectedItems.Count == 1)
          this.saveChanges(true);
        this.generateFieldList();
        if (filter == null)
          filter = this.generateQueryCriteria();
        ListValueCriterion criterion;
        if (this.cboFilterView.SelectedIndex == 0)
          criterion = new ListValueCriterion("CorrespondentMaster.Status", (Array) new int[1], true);
        else
          criterion = new ListValueCriterion("CorrespondentMaster.Status", (Array) new int[1]
          {
            1
          }, true);
        filter = filter != null ? filter.And((QueryCriterion) criterion) : (QueryCriterion) criterion;
        if (sortFields == null)
          sortFields = this.generateSortFields();
        this.suspendEvents = true;
        if (this.corrMasterCursor != null)
        {
          this.corrMasterCursor.Dispose();
          this.corrMasterCursor = (ICursor) null;
        }
        if (filter == null)
          this.corrMasterCursor = Session.CorrespondentMasterManager.OpenCorrespondentMasterCursor(new QueryCriterion[0], sortFields, false);
        else
          this.corrMasterCursor = Session.CorrespondentMasterManager.OpenCorrespondentMasterCursor(new QueryCriterion[1]
          {
            filter
          }, sortFields, false);
        this.navPage.NumberOfItems = this.corrMasterCursor.GetItemCount();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error loading items: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
        this.suspendEvents = false;
      }
    }

    private SortField[] generateSortFields()
    {
      List<SortField> sortFieldList = new List<SortField>();
      foreach (GVColumn column in this.gvMain.Columns)
      {
        if (column.SortOrder != SortOrder.None)
        {
          TableLayout.Column tag = (TableLayout.Column) column.Tag;
          sortFieldList.Add(new SortField(tag.ColumnID, SortOrder.Ascending == column.SortOrder ? FieldSortOrder.Ascending : FieldSortOrder.Descending));
        }
      }
      return sortFieldList.ToArray();
    }

    private SortField[] generateSortFields(GVColumnSort[] sortOrder)
    {
      List<SortField> sortFieldList = new List<SortField>();
      foreach (GVColumnSort gvColumnSort in sortOrder)
      {
        TableLayout.Column tag = (TableLayout.Column) this.gvMain.Columns[gvColumnSort.Column].Tag;
        sortFieldList.Add(new SortField(tag.ColumnID, SortOrder.Ascending == gvColumnSort.SortOrder ? FieldSortOrder.Ascending : FieldSortOrder.Descending));
      }
      return sortFieldList.ToArray();
    }

    private QueryCriterion generateQueryCriteria()
    {
      QueryCriterion criterion = (QueryCriterion) null;
      if (this.advFilter != null)
        criterion = this.advFilter.CreateEvaluator().ToQueryCriterion();
      QueryCriterion queryCriterion = this.gvFilterManager.ToQueryCriterion();
      QueryCriterion queryCriteria = (QueryCriterion) null;
      if (criterion != null)
        queryCriteria = queryCriteria == null ? criterion : queryCriteria.And(criterion);
      if (queryCriterion != null)
        queryCriteria = queryCriteria == null ? queryCriterion : queryCriteria.And(queryCriterion);
      return queryCriteria;
    }

    private string[] generateFieldList()
    {
      List<string> stringList = new List<string>();
      if (this.gvLayoutManager == null)
        this.gvLayoutManager = this.createLayoutManager();
      foreach (TableLayout.Column column in this.gvLayoutManager.GetCurrentLayout())
      {
        CorrespondentMasterReportFieldDef fieldByCriterionName = this.correspondentMasterFieldDefs.GetFieldByCriterionName(column.ColumnID);
        if (!stringList.Contains(column.ColumnID))
          stringList.Add(column.ColumnID);
        if (fieldByCriterionName != null)
        {
          foreach (string relatedField in fieldByCriterionName.RelatedFields)
          {
            if (!stringList.Contains(relatedField))
              stringList.Add(relatedField);
          }
        }
      }
      return stringList.ToArray();
    }

    private bool saveChanges(bool prompt) => true;

    private void applyTableLayout(TableLayout layout)
    {
      if (this.gvLayoutManager == null)
        this.gvLayoutManager = this.createLayoutManager();
      this.gvLayoutManager.ApplyLayout(layout, false);
      if (this.gvFilterManager == null)
      {
        this.gvFilterManager = new GridViewReportFilterManager(Session.DefaultInstance, this.gvMain);
        this.gvFilterManager.FilterChanged += new EventHandler(this.gvFilterManager_FilterChanged);
      }
      this.gvFilterManager.CreateColumnFilters((ReportFieldDefs) this.correspondentMasterFieldDefs);
    }

    private GridViewLayoutManager createLayoutManager()
    {
      GridViewLayoutManager layoutManager = new GridViewLayoutManager(this.gvMain, this.getFullTableLayout());
      layoutManager.LayoutChanged += new EventHandler(this.mngr_LayoutChanged);
      return layoutManager;
    }

    private void mngr_LayoutChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents)
        return;
      if (this.gvFilterManager != null)
        this.gvFilterManager.CreateColumnFilters((ReportFieldDefs) this.correspondentMasterFieldDefs);
      this.mainTableLayout = this.gvLayoutManager.GetCurrentLayout();
      this.gvFilterManager.CreateColumnFilters((ReportFieldDefs) this.correspondentMasterFieldDefs);
      this.setViewChanged(true);
      this.RefreshList(true);
    }

    private TableLayout getFullTableLayout()
    {
      TableLayout fullTableLayout = new TableLayout();
      foreach (CorrespondentMasterReportFieldDef correspondentMasterFieldDef in (ReportFieldDefContainer) this.correspondentMasterFieldDefs)
      {
        if (fullTableLayout.GetColumnByID(correspondentMasterFieldDef.CriterionFieldName) == null)
          fullTableLayout.AddColumn(ReportFieldClientExtension.ToTableLayoutColumn(correspondentMasterFieldDef));
      }
      fullTableLayout.SortByDescription();
      return fullTableLayout;
    }

    private void gvFilterManager_FilterChanged(object sender, EventArgs e)
    {
      this.RefreshList(false);
      this.setViewChanged(true);
    }

    private void btnSaveView_Click(object sender, EventArgs e) => this.saveCurrentView();

    private void saveCurrentView()
    {
      TradeView tradeView = new TradeView(this.currentView.Name);
      tradeView.Layout = this.gvLayoutManager.GetCurrentLayout();
      tradeView.Filter = this.GetCurrentFilter();
      tradeView.ViewActive = this.cboFilterView.SelectedIndex == 0;
      using (SaveViewTemplateDialog viewTemplateDialog = new SaveViewTemplateDialog(EllieMae.EMLite.ClientServer.TemplateSettingsType.CorrespondentMasterView, (object) tradeView, this.getViewNameList(), this.currentView.Name != this.standardViewName))
      {
        if (viewTemplateDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        if (!viewTemplateDialog.SelectedEntry.Equals((object) this.fsViewEntry))
          this.updateCurrentView(tradeView, viewTemplateDialog.SelectedEntry);
      }
      this.currentView = tradeView;
      this.btnSaveView.Enabled = false;
      this.btnRefreshView.Enabled = false;
    }

    private void updateCurrentView(TradeView view, FileSystemEntry e)
    {
      this.suspendEvents = true;
      this.currentView = view;
      ClientCommonUtils.PopulateDropdown((ComboBox) this.cBoxView, (object) new FileSystemEntryListItem(e), true);
      this.suspendEvents = false;
    }

    private string[] getViewNameList()
    {
      List<string> stringList = new List<string>();
      foreach (object obj in this.cBoxView.Items)
        stringList.Add(obj.ToString());
      return stringList.ToArray();
    }

    private void btnRefreshView_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to reset the selected view?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.setCurrentView(this.currentView);
    }

    private void btnEditView_Click(object sender, EventArgs e)
    {
      using (ViewManagementDialog managementDialog = new ViewManagementDialog(EllieMae.EMLite.ClientServer.TemplateSettingsType.CorrespondentMasterView, false, "CorrespondentMaster.DefaultView"))
      {
        managementDialog.AddStaticView((BinaryConvertibleObject) this.getDefaultCommitmentView());
        int num = (int) managementDialog.ShowDialog((IWin32Window) this);
      }
      this.RefreshViews();
    }

    public void RefreshViews()
    {
      this.loadViewList(Session.ConfigurationManager.GetTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.CorrespondentMasterView, FileSystemEntry.PrivateRoot(Session.UserID)));
      if (this.cBoxView.Items.Count <= 0 || this.cBoxView.SelectedIndex >= 0)
        return;
      this.cBoxView.SelectedIndex = 0;
    }

    private void gvMain_SelectedIndexChanged(object sender, EventArgs e)
    {
      int count = this.gvMain.SelectedItems.Count;
      this.btnDelete.Enabled = !this.IsReadOnly && count > 0;
      this.btnArchive.Enabled = !this.IsReadOnly && count > 0;
      this.btnActivate.Enabled = !this.IsReadOnly && count > 0;
    }

    private void cBoxView_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents)
        return;
      this.SetCurrentView(((FileSystemEntryListItem) this.cBoxView.SelectedItem).Entry);
    }

    private bool exportContracts()
    {
      if (this.gvMain.Columns.Count > ExcelHandler.GetMaximumColumnCount())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You commitment list cannot be exported because the number of columns exceeds the limit supported by Excel (" + (object) ExcelHandler.GetMaximumColumnCount() + ")", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (this.gvMain.Items.Count > ExcelHandler.GetMaximumRowCount() - 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You commitment list cannot be exported because the number of rows exceeds the limit supported by Excel (" + (object) ExcelHandler.GetMaximumRowCount() + ")", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      this.exportSelectedRowsToExcel();
      return true;
    }

    private void exportSelectedRowsToExcel()
    {
      ExcelHandler excelHandler = new ExcelHandler();
      excelHandler.AddDataTable(this.gvMain, (ReportFieldDefs) this.correspondentMasterFieldDefs, true);
      excelHandler.CreateExcel();
    }

    private void btnClearSearch_Click(object sender, EventArgs e)
    {
      this.SetCurrentFilter((FieldFilterList) null);
    }

    private void btnAdvSearch_Click(object sender, EventArgs e)
    {
      FieldFilterList fieldFilterList = this.gvFilterManager.ToFieldFilterList();
      if (this.advFilter != null)
        fieldFilterList.Merge(this.advFilter);
      if (this.correspondentMasterFieldDefs == null)
        this.resetFieldDefs();
      using (ViewAdvSearchDialog viewAdvSearchDialog = new ViewAdvSearchDialog((ReportFieldDefs) this.correspondentMasterFieldDefs, fieldFilterList, "Correspondent Master View Advanced Search"))
      {
        if (viewAdvSearchDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.SetCurrentFilter(viewAdvSearchDialog.GetSelectedFilter());
      }
    }

    private void siBtnRefresh_Click(object sender, EventArgs e) => this.RefreshList(true);

    private void siBtnExcel_Click(object sender, EventArgs e)
    {
      if (this.gvMain.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more correspondent master commitments from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.exportContracts();
    }

    private void siBtnPrint_Click(object sender, EventArgs e)
    {
      if (this.gvMain.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more correspondent master commitments from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.printSelectedContracts();
    }

    private void printSelectedContracts()
    {
      ExcelHandler excelHandler = new ExcelHandler();
      excelHandler.AddDataTable(this.gvMain, (ReportFieldDefs) this.correspondentMasterFieldDefs, true);
      excelHandler.Print();
    }

    private void gvMain_ColumnReorder(object source, GVColumnEventArgs e)
    {
      this.setViewChanged(true);
    }

    private void gvMain_ColumnResize(object source, GVColumnEventArgs e)
    {
      this.setViewChanged(true);
    }

    private void gvMain_SortItems(object source, GVColumnSortEventArgs e)
    {
      if (this.suspendEvents)
        return;
      using (CursorActivator.Wait())
      {
        this.retrieveCorrespondentMasterData(this.generateQueryCriteria(), this.generateSortFields(e.ColumnSorts));
        this.displayCurrentPage(true);
        this.setViewChanged(true);
      }
    }

    private void navContracts_PageChangedEvent(object sender, PageChangedEventArgs e)
    {
      if (this.suspendEvents)
        return;
      this.displayCurrentPage(false);
    }

    private void cboFilterView_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.RefreshList(true);
      this.showHideButtons();
      this.setViewChanged(true);
    }

    private void showHideButtons()
    {
      bool flag = this.cboFilterView.SelectedIndex == 0;
      this.btnNew.Visible = flag;
      this.btnArchive.Visible = flag;
      this.btnActivate.Visible = !flag;
    }

    private void gvMain_ItemDoubleClick(object source, GVItemEventArgs e) => this.editTrade(e.Item);

    private void editTrade(GVItem item)
    {
      TradeManagementConsole.Instance.OpenCorrespondentMaster(((CorrespondentMasterViewModel) item.Tag).CorrespondentMasterID);
    }

    private IEnumerable<int> findUniqueCMCID(GVSelectedItemCollection collection)
    {
      List<int> source = new List<int>();
      foreach (GVItem gvItem in collection)
      {
        CorrespondentMasterViewModel tag = (CorrespondentMasterViewModel) gvItem.Tag;
        source.Add(tag.CorrespondentMasterID);
      }
      return source.Distinct<int>();
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
      this.grpContracts = new GroupContainer();
      this.navPage = new PageListNavigator();
      this.gvMain = new GridView();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnActivate = new Button();
      this.btnArchive = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.siBtnPrint = new StandardIconButton();
      this.siBtnExcel = new StandardIconButton();
      this.siBtnRefresh = new StandardIconButton();
      this.btnDelete = new StandardIconButton();
      this.btnNew = new StandardIconButton();
      this.cBoxView = new ComboBoxEx();
      this.label1 = new Label();
      this.toolTip1 = new ToolTip(this.components);
      this.btnEditView = new StandardIconButton();
      this.btnRefreshView = new StandardIconButton();
      this.btnSaveView = new StandardIconButton();
      this.gradientPanel1 = new GradientPanel();
      this.gradientPanel2 = new GradientPanel();
      this.label3 = new Label();
      this.cboFilterView = new ComboBox();
      this.btnAdvSearch = new Button();
      this.btnClearSearch = new Button();
      this.lblFilter = new Label();
      this.label2 = new Label();
      this.grpContracts.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.siBtnPrint).BeginInit();
      ((ISupportInitialize) this.siBtnExcel).BeginInit();
      ((ISupportInitialize) this.siBtnRefresh).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnNew).BeginInit();
      ((ISupportInitialize) this.btnEditView).BeginInit();
      ((ISupportInitialize) this.btnRefreshView).BeginInit();
      ((ISupportInitialize) this.btnSaveView).BeginInit();
      this.gradientPanel1.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.SuspendLayout();
      this.grpContracts.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.grpContracts.Controls.Add((Control) this.navPage);
      this.grpContracts.Controls.Add((Control) this.gvMain);
      this.grpContracts.Controls.Add((Control) this.flowLayoutPanel1);
      this.grpContracts.Dock = DockStyle.Fill;
      this.grpContracts.HeaderForeColor = SystemColors.ControlText;
      this.grpContracts.Location = new Point(0, 61);
      this.grpContracts.Name = "grpContracts";
      this.grpContracts.Size = new Size(734, 540);
      this.grpContracts.TabIndex = 6;
      this.navPage.BackColor = Color.Transparent;
      this.navPage.Font = new Font("Arial", 8f);
      this.navPage.Location = new Point(3, 2);
      this.navPage.Name = "navPage";
      this.navPage.NumberOfItems = 0;
      this.navPage.Size = new Size(254, 24);
      this.navPage.TabIndex = 8;
      this.navPage.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.navContracts_PageChangedEvent);
      this.gvMain.AllowColumnReorder = true;
      this.gvMain.BorderStyle = BorderStyle.None;
      this.gvMain.Dock = DockStyle.Fill;
      this.gvMain.FilterVisible = true;
      this.gvMain.Location = new Point(0, 26);
      this.gvMain.Name = "gvMain";
      this.gvMain.Size = new Size(734, 513);
      this.gvMain.SortOption = GVSortOption.Owner;
      this.gvMain.TabIndex = 6;
      this.gvMain.SelectedIndexChanged += new EventHandler(this.gvMain_SelectedIndexChanged);
      this.gvMain.ColumnReorder += new GVColumnEventHandler(this.gvMain_ColumnReorder);
      this.gvMain.ColumnResize += new GVColumnEventHandler(this.gvMain_ColumnResize);
      this.gvMain.SortItems += new GVColumnSortEventHandler(this.gvMain_SortItems);
      this.gvMain.ItemDoubleClick += new GVItemEventHandler(this.gvMain_ItemDoubleClick);
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnActivate);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnArchive);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparator1);
      this.flowLayoutPanel1.Controls.Add((Control) this.siBtnPrint);
      this.flowLayoutPanel1.Controls.Add((Control) this.siBtnExcel);
      this.flowLayoutPanel1.Controls.Add((Control) this.siBtnRefresh);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnDelete);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnNew);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(446, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(283, 22);
      this.flowLayoutPanel1.TabIndex = 4;
      this.flowLayoutPanel1.WrapContents = false;
      this.btnActivate.BackColor = SystemColors.Control;
      this.btnActivate.Enabled = false;
      this.btnActivate.Location = new Point(211, 0);
      this.btnActivate.Margin = new Padding(0);
      this.btnActivate.Name = "btnActivate";
      this.btnActivate.Size = new Size(72, 22);
      this.btnActivate.TabIndex = 6;
      this.btnActivate.Text = "Ac&tivate";
      this.btnActivate.UseVisualStyleBackColor = true;
      this.btnActivate.Click += new EventHandler(this.btnActivate_Click);
      this.btnArchive.BackColor = SystemColors.Control;
      this.btnArchive.Enabled = false;
      this.btnArchive.Location = new Point(138, 0);
      this.btnArchive.Margin = new Padding(0);
      this.btnArchive.Name = "btnArchive";
      this.btnArchive.Size = new Size(73, 22);
      this.btnArchive.TabIndex = 6;
      this.btnArchive.Text = "A&rchive";
      this.btnArchive.UseVisualStyleBackColor = true;
      this.btnArchive.Click += new EventHandler(this.btnArchive_Click);
      this.verticalSeparator1.Location = new Point(133, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 4;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.siBtnPrint.BackColor = Color.Transparent;
      this.siBtnPrint.Location = new Point(112, 3);
      this.siBtnPrint.Margin = new Padding(3, 3, 2, 3);
      this.siBtnPrint.MouseDownImage = (Image) null;
      this.siBtnPrint.Name = "siBtnPrint";
      this.siBtnPrint.Size = new Size(16, 16);
      this.siBtnPrint.StandardButtonType = StandardIconButton.ButtonType.PrintButton;
      this.siBtnPrint.TabIndex = 13;
      this.siBtnPrint.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnPrint, "Print Forms");
      this.siBtnPrint.Click += new EventHandler(this.siBtnPrint_Click);
      this.siBtnExcel.BackColor = Color.Transparent;
      this.siBtnExcel.Location = new Point(91, 3);
      this.siBtnExcel.Margin = new Padding(3, 3, 2, 3);
      this.siBtnExcel.MouseDownImage = (Image) null;
      this.siBtnExcel.Name = "siBtnExcel";
      this.siBtnExcel.Size = new Size(16, 16);
      this.siBtnExcel.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.siBtnExcel.TabIndex = 12;
      this.siBtnExcel.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnExcel, "Exports to Excel");
      this.siBtnExcel.Click += new EventHandler(this.siBtnExcel_Click);
      this.siBtnRefresh.BackColor = Color.Transparent;
      this.siBtnRefresh.Location = new Point(70, 3);
      this.siBtnRefresh.Margin = new Padding(3, 3, 2, 3);
      this.siBtnRefresh.MouseDownImage = (Image) null;
      this.siBtnRefresh.Name = "siBtnRefresh";
      this.siBtnRefresh.Size = new Size(16, 16);
      this.siBtnRefresh.StandardButtonType = StandardIconButton.ButtonType.RefreshButton;
      this.siBtnRefresh.TabIndex = 11;
      this.siBtnRefresh.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnRefresh, "Refresh");
      this.siBtnRefresh.Click += new EventHandler(this.siBtnRefresh_Click);
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(49, 3);
      this.btnDelete.Margin = new Padding(3, 3, 2, 3);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 0;
      this.btnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDelete, "Delete Commitment");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnNew.BackColor = Color.Transparent;
      this.btnNew.Location = new Point(28, 3);
      this.btnNew.Margin = new Padding(3, 3, 2, 3);
      this.btnNew.MouseDownImage = (Image) null;
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(16, 16);
      this.btnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNew.TabIndex = 3;
      this.btnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnNew, "New Commitment");
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.cBoxView.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cBoxView.FormattingEnabled = true;
      this.cBoxView.Items.AddRange(new object[2]
      {
        (object) "Current Commitments",
        (object) "Archived Commitments"
      });
      this.cBoxView.Location = new Point(263, 6);
      this.cBoxView.Name = "cBoxView";
      this.cBoxView.SelectedBGColor = SystemColors.Highlight;
      this.cBoxView.Size = new Size(158, 21);
      this.cBoxView.TabIndex = 7;
      this.cBoxView.SelectedIndexChanged += new EventHandler(this.cBoxView_SelectedIndexChanged);
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(14, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(248, 14);
      this.label1.TabIndex = 5;
      this.label1.Text = "Correspondent Master Commitments View";
      this.btnEditView.BackColor = Color.Transparent;
      this.btnEditView.Location = new Point(472, 9);
      this.btnEditView.MouseDownImage = (Image) null;
      this.btnEditView.Name = "btnEditView";
      this.btnEditView.Size = new Size(16, 16);
      this.btnEditView.StandardButtonType = StandardIconButton.ButtonType.ManageButton;
      this.btnEditView.TabIndex = 10;
      this.btnEditView.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEditView, "Manage Views");
      this.btnEditView.Click += new EventHandler(this.btnEditView_Click);
      this.btnRefreshView.BackColor = Color.Transparent;
      this.btnRefreshView.Enabled = false;
      this.btnRefreshView.Location = new Point(450, 9);
      this.btnRefreshView.MouseDownImage = (Image) null;
      this.btnRefreshView.Name = "btnRefreshView";
      this.btnRefreshView.Size = new Size(16, 16);
      this.btnRefreshView.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnRefreshView.TabIndex = 9;
      this.btnRefreshView.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRefreshView, "Reset View");
      this.btnRefreshView.Click += new EventHandler(this.btnRefreshView_Click);
      this.btnSaveView.BackColor = Color.Transparent;
      this.btnSaveView.Enabled = false;
      this.btnSaveView.Location = new Point(428, 9);
      this.btnSaveView.MouseDownImage = (Image) null;
      this.btnSaveView.Name = "btnSaveView";
      this.btnSaveView.Size = new Size(16, 16);
      this.btnSaveView.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSaveView.TabIndex = 8;
      this.btnSaveView.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnSaveView, "Save View");
      this.btnSaveView.Click += new EventHandler(this.btnSaveView_Click);
      this.gradientPanel1.BackColorGlassyStyle = true;
      this.gradientPanel1.Borders = AnchorStyles.None;
      this.gradientPanel1.Controls.Add((Control) this.btnEditView);
      this.gradientPanel1.Controls.Add((Control) this.btnRefreshView);
      this.gradientPanel1.Controls.Add((Control) this.btnSaveView);
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Controls.Add((Control) this.cBoxView);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(81, 123, 184);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(167, 201, 239);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(734, 30);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageHeader;
      this.gradientPanel1.TabIndex = 8;
      this.gradientPanel2.Borders = AnchorStyles.Top;
      this.gradientPanel2.Controls.Add((Control) this.label3);
      this.gradientPanel2.Controls.Add((Control) this.cboFilterView);
      this.gradientPanel2.Controls.Add((Control) this.btnAdvSearch);
      this.gradientPanel2.Controls.Add((Control) this.btnClearSearch);
      this.gradientPanel2.Controls.Add((Control) this.lblFilter);
      this.gradientPanel2.Controls.Add((Control) this.label2);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.Location = new Point(0, 30);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(734, 31);
      this.gradientPanel2.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel2.TabIndex = 9;
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Location = new Point(14, 9);
      this.label3.Name = "label3";
      this.label3.Size = new Size(36, 14);
      this.label3.TabIndex = 16;
      this.label3.Text = "View:";
      this.cboFilterView.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFilterView.FormattingEnabled = true;
      this.cboFilterView.Items.AddRange(new object[2]
      {
        (object) "Current Commitments",
        (object) "Archived Commitments"
      });
      this.cboFilterView.Location = new Point(56, 4);
      this.cboFilterView.Name = "cboFilterView";
      this.cboFilterView.Size = new Size(158, 22);
      this.cboFilterView.TabIndex = 15;
      this.cboFilterView.SelectedIndexChanged += new EventHandler(this.cboFilterView_SelectedIndexChanged);
      this.btnAdvSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdvSearch.Location = new Point(555, 4);
      this.btnAdvSearch.Name = "btnAdvSearch";
      this.btnAdvSearch.Size = new Size(122, 23);
      this.btnAdvSearch.TabIndex = 10;
      this.btnAdvSearch.Text = "Advanced Search";
      this.btnAdvSearch.UseVisualStyleBackColor = true;
      this.btnAdvSearch.Click += new EventHandler(this.btnAdvSearch_Click);
      this.btnClearSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClearSearch.Location = new Point(678, 4);
      this.btnClearSearch.Name = "btnClearSearch";
      this.btnClearSearch.Size = new Size(53, 23);
      this.btnClearSearch.TabIndex = 9;
      this.btnClearSearch.Text = "Clear";
      this.btnClearSearch.UseVisualStyleBackColor = true;
      this.btnClearSearch.Click += new EventHandler(this.btnClearSearch_Click);
      this.lblFilter.AutoSize = true;
      this.lblFilter.Location = new Point(273, 8);
      this.lblFilter.Name = "lblFilter";
      this.lblFilter.Size = new Size(32, 14);
      this.lblFilter.TabIndex = 1;
      this.lblFilter.Text = "None";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(233, 8);
      this.label2.Name = "label2";
      this.label2.Size = new Size(33, 14);
      this.label2.TabIndex = 0;
      this.label2.Text = "Filter:";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Transparent;
      this.Controls.Add((Control) this.grpContracts);
      this.Controls.Add((Control) this.gradientPanel2);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (CorrespondentMasterListScreen);
      this.Size = new Size(734, 601);
      this.grpContracts.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.siBtnPrint).EndInit();
      ((ISupportInitialize) this.siBtnExcel).EndInit();
      ((ISupportInitialize) this.siBtnRefresh).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnNew).EndInit();
      ((ISupportInitialize) this.btnEditView).EndInit();
      ((ISupportInitialize) this.btnRefreshView).EndInit();
      ((ISupportInitialize) this.btnSaveView).EndInit();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
