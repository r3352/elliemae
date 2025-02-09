// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.InvestorExportDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataServices;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.InputEngine.MilestoneManagement;
using EllieMae.EMLite.JedLib;
using EllieMae.EMLite.LoanServices;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.Autosave;
using EllieMae.EMLite.StatusOnline;
using EllieMae.EMLite.UI;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class InvestorExportDialog : Form
  {
    private GridViewLayoutManager layoutManager;
    private LoanReportFieldDefs fieldDefs;
    private static readonly string[] requiredFields = new string[11]
    {
      "Loan.Guid",
      "Loan.LoanNumber",
      "Loan.LoanName",
      "Loan.LoanFolder",
      "Loan.BorrowerLastName",
      "Loan.BorrowerFirstName",
      "Loan.LoanStatus",
      "Loan.Adverse",
      "Loan.CurrentMilestoneName",
      "Loan.ActionTaken",
      "Loan.NextMilestoneName"
    };
    private bool suspendEvents;
    private ICursor pipelineCursor;
    private FieldFilterList advFilter;
    private bool suspendRefresh;
    private LoanPage loanPage;
    private static InvestorExportDialog instance;
    private static List<string> guidList;
    private bool hasOpenLoan;
    private ServiceSetting investorSetting;
    public LoanDataModifiedEventHandler LoanDataModifiedHandler;
    private const string className = "InvestorExportDialog";
    private ILoanServices loanServices;
    protected static string sw = Tracing.SwOutsideLoan;
    private static b jed = (b) null;
    private static bool passDataChecking;
    private static string autosaveFolderFixedPart = (string) null;
    private IContainer components;
    private Panel pnlAction;
    private Button btnExport;
    private Button btnCancel;
    private TabPage pipelineTabPage;
    private TabPage loanTabPage;
    private TabControl tabControl;
    private GridView gvLoans;
    private GroupContainer groupContainer1;
    private PageListNavigator navPipeline;
    private Timer timerAutosave;
    private GradientPanel gradientPanel1;
    private Label label1;

    public static InvestorExportDialog Instance
    {
      get
      {
        if (InvestorExportDialog.instance == null)
          InvestorExportDialog.instance = new InvestorExportDialog();
        return InvestorExportDialog.instance;
      }
    }

    public static List<string> GuidList => InvestorExportDialog.guidList;

    static InvestorExportDialog()
    {
      if (Session.Connection.IsServerInProcess)
        a.a("mkl9m3X90nM45sY");
      else
        a.a("i9w9j72bidpm93x");
      if (InvestorExportDialog.jed != null)
        return;
      if (Session.Connection.IsServerInProcess)
        InvestorExportDialog.jed = a.b("mk0T8jLZ0");
      else
        InvestorExportDialog.jed = a.b("km0w2khs9");
    }

    private InvestorExportDialog()
    {
      this.InitializeComponent();
      AutosaveConfigManager.OnAutosaveSettingsChanged += new AutosaveSettingsChangeEventHandler(this.processAutosaveSettingsChange);
      Tracing.Log(InvestorExportDialog.sw, nameof (InvestorExportDialog), TraceLevel.Info, "Starting AutoSave timer...");
      this.timerAutosave.Interval = AutosaveConfigManager.GetInterval() * 1000;
      this.LoanDataModifiedHandler = new LoanDataModifiedEventHandler(this.onLoanDataModified);
      this.timerAutosave.Tick += new EventHandler(this.autosaveHandler);
      this.loanServices = Session.Application.GetService<ILoanServices>();
    }

    private void autosaveHandler(object sender, EventArgs e)
    {
      try
      {
        if (Session.LoanDataMgr == null || Session.LoanData == null || !Session.LoanDataMgr.Writable)
          this.timerAutosave.Stop();
        else
          this.PerformAutoSave();
      }
      catch (Exception ex)
      {
        Tracing.Log(InvestorExportDialog.sw, nameof (InvestorExportDialog), TraceLevel.Error, "Error during autosave: " + (object) ex);
      }
    }

    public void PerformAutoSave()
    {
      try
      {
        if (Session.LoanDataMgr == null)
          return;
        if (Session.LoanData != null && Session.LoanData.Dirty)
          this.performAutoSave(Session.LoanDataMgr);
        if (Session.LoanDataMgr.LinkedLoan == null || Session.LoanDataMgr.LinkedLoan.LoanData == null || !Session.LoanDataMgr.LinkedLoan.LoanData.Dirty)
          return;
        this.performAutoSave(Session.LoanDataMgr.LinkedLoan);
      }
      catch (Exception ex)
      {
        Tracing.Log(InvestorExportDialog.sw, nameof (InvestorExportDialog), TraceLevel.Error, "Error during autosave: " + (object) ex);
      }
    }

    private void performAutoSave(LoanDataMgr loanDataMgr)
    {
      string autoRecoverFilePath = this.getAutoRecoverFilePath(loanDataMgr);
      if (autoRecoverFilePath != null && !Directory.Exists(autoRecoverFilePath))
        Directory.CreateDirectory(autoRecoverFilePath);
      string autoRecoverFileName = this.getAutoRecoverFileName(loanDataMgr);
      string recoverAttFileName = this.getAutoRecoverAttFileName(autoRecoverFileName);
      string recoverHistoryFileName = this.getAutoRecoverHistoryFileName(autoRecoverFileName);
      File.WriteAllText(this.getAutoRecoverTsFileName(autoRecoverFileName), Session.ServerTime.ToString("yyyy-MM-dd HH:mm:ss tt"));
      string xml = loanDataMgr.LoanData.ToXml(loanDataMgr.LoanData.ContentAccess, true, false);
      InvestorExportDialog.jed.b();
      byte[] bytes1 = InvestorExportDialog.jed.b(xml);
      File.WriteAllBytes(autoRecoverFileName, bytes1);
      string autoSaveXml = loanDataMgr.FileAttachments.GetAutoSaveXml();
      InvestorExportDialog.jed.b();
      byte[] bytes2 = InvestorExportDialog.jed.b(autoSaveXml);
      File.WriteAllBytes(recoverAttFileName, bytes2);
      string pendingHistory = loanDataMgr.LoanHistory.GetPendingHistory();
      InvestorExportDialog.jed.b();
      byte[] bytes3 = InvestorExportDialog.jed.b(pendingHistory);
      File.WriteAllBytes(recoverHistoryFileName, bytes3);
    }

    private string getAutoRecoverFilePath(LoanDataMgr loanDataMgr)
    {
      if (loanDataMgr == null || loanDataMgr.LoanFolder == null)
        return (string) null;
      if (InvestorExportDialog.autosaveFolderFixedPart == null)
      {
        RegistryKey registryKey = (RegistryKey) null;
        try
        {
          registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass");
          if (registryKey != null)
            InvestorExportDialog.autosaveFolderFixedPart = (string) registryKey.GetValue("AutosaveFolderRoot");
        }
        catch
        {
        }
        finally
        {
          registryKey?.Close();
        }
        if (InvestorExportDialog.autosaveFolderFixedPart != null)
          InvestorExportDialog.autosaveFolderFixedPart = InvestorExportDialog.autosaveFolderFixedPart.Trim();
        if ((InvestorExportDialog.autosaveFolderFixedPart ?? "") == "")
          InvestorExportDialog.autosaveFolderFixedPart = EnConfigurationSettings.GlobalSettings.AppLoanAutosaveDirectory;
        string str = "(local)";
        if (!Session.Connection.IsServerInProcess)
          str = Session.CompanyInfo.ClientID;
        InvestorExportDialog.autosaveFolderFixedPart = Path.Combine(InvestorExportDialog.autosaveFolderFixedPart, str + "\\" + Session.UserID);
      }
      string path = Path.Combine(InvestorExportDialog.autosaveFolderFixedPart, loanDataMgr.LoanFolder);
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      return path;
    }

    private void onLoanDataModified(object sender, EventArgs e)
    {
      if (Session.LoanData == null || !Session.LoanData.Dirty)
        return;
      MainForm.Instance.SetLastSaveTimeDirtyFlag();
    }

    private void processAutosaveSettingsChange(object sender, AutosaveSettingsChangeEventArgs e)
    {
      this.timerAutosave.Stop();
      this.timerAutosave.Interval = e.Interval * 1000;
      if (!e.Enabled)
        return;
      this.StartAutosaveTimer();
    }

    public void Initialize(
      LoanReportFieldDefs fieldDefs,
      string[] loanGuidList,
      TableLayout layout,
      ServiceSetting investorSetting)
    {
      TableLayout layout1 = layout;
      this.investorSetting = investorSetting;
      this.fieldDefs = fieldDefs;
      InvestorExportDialog.passDataChecking = true;
      this.advFilter = new FieldFilterList();
      foreach (string loanGuid in loanGuidList)
        this.advFilter.Add(new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsString, "Loan.Guid", "Loan Guid", OperatorTypes.IsExact, loanGuid), JointTokens.Or);
      this.applyTableLayout(layout1);
      this.RefreshPipeline(false);
      this.tabControl.TabPages.Remove(this.loanTabPage);
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

    private TableLayout getFullTableLayout()
    {
      TableLayout fullTableLayout = new TableLayout();
      foreach (LoanReportFieldDef fieldDef in (ReportFieldDefContainer) this.fieldDefs)
      {
        if (fullTableLayout.GetColumnByID(fieldDef.CriterionFieldName) == null)
          fullTableLayout.AddColumn(ReportFieldClientExtension.ToTableLayoutColumn(fieldDef));
      }
      fullTableLayout.SortByDescription();
      return fullTableLayout;
    }

    private GridViewLayoutManager createLayoutManager()
    {
      GridViewLayoutManager layoutManager = new GridViewLayoutManager(this.gvLoans, this.getFullTableLayout());
      layoutManager.LayoutChanged += new EventHandler(this.onLayoutChanged);
      return layoutManager;
    }

    private void onLayoutChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents)
        return;
      this.RefreshPipeline(true);
    }

    public void RefreshPipeline(bool preserveSelection)
    {
      if (this.suspendRefresh)
        return;
      using (CursorActivator.Wait())
      {
        using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("Export.Pipeline.Refresh", "Refresh the pipeline screen data", true, 336, nameof (RefreshPipeline), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\InvestorExportDialog.cs"))
        {
          if (this.retrievePipelineData())
            this.displayCurrentPage(preserveSelection);
          performanceMeter.AddVariable("LoanCount", (object) this.navPipeline.NumberOfItems);
          performanceMeter.AddVariable("Columns", (object) this.gvLoans.Columns.Count);
          GVColumnSort[] sortOrder = this.gvLoans.Columns.GetSortOrder();
          if (sortOrder.Length == 0)
            return;
          performanceMeter.AddVariable("Sort", this.gvLoans.Columns[sortOrder[0].Column].Tag);
        }
      }
    }

    private bool retrievePipelineData()
    {
      return this.retrievePipelineData((QueryCriterion) null, (SortField[]) null);
    }

    private bool retrievePipelineData(QueryCriterion filter, SortField[] sortFields)
    {
      string[] fieldList = this.generateFieldList();
      string loanFolder = (string) null;
      if (filter == null)
        filter = this.generateQueryCriteria();
      if (sortFields == null)
        sortFields = this.getCurrentSortFields();
      try
      {
        this.suspendEvents = true;
        if (this.pipelineCursor != null)
        {
          this.pipelineCursor.Dispose();
          this.pipelineCursor = (ICursor) null;
        }
        this.pipelineCursor = Session.LoanManager.OpenPipeline(loanFolder, LoanInfo.Right.Access, fieldList, PipelineData.Lock | PipelineData.Milestones | PipelineData.LoanAssociates | PipelineData.CurrentUserAccessRightsOnly, sortFields, filter, false);
        this.navPipeline.NumberOfItems = this.pipelineCursor.GetItemCount();
        return true;
      }
      catch (Exception ex)
      {
        MetricsFactory.IncrementErrorCounter(ex, "Error loading pipeline", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\InvestorExportDialog.cs", nameof (retrievePipelineData), 405);
        int num = (int) Utils.Dialog((IWin32Window) this, "Error loading pipeline: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      finally
      {
        this.suspendEvents = false;
      }
    }

    private SortField[] getCurrentSortFields()
    {
      return this.getSortFieldsForColumnSort(this.gvLoans.Columns.GetSortOrder());
    }

    private SortField[] getSortFieldsForColumnSort(GVColumnSort[] sortOrder)
    {
      List<SortField> sortFieldList = new List<SortField>();
      foreach (GVColumnSort gvColumnSort in sortOrder)
      {
        TableLayout.Column tag = (TableLayout.Column) this.gvLoans.Columns[gvColumnSort.Column].Tag;
        if (this.getSortFieldForColumn(tag, gvColumnSort.SortOrder) != null)
          sortFieldList.Add(this.getSortFieldForColumn(tag, gvColumnSort.SortOrder));
      }
      return sortFieldList.ToArray();
    }

    private SortField getSortFieldForColumn(TableLayout.Column colInfo, SortOrder sortOrder)
    {
      LoanReportFieldDef fieldByCriterionName = this.fieldDefs.GetFieldByCriterionName(colInfo.ColumnID);
      return fieldByCriterionName != null ? new SortField(fieldByCriterionName.SortTerm, sortOrder == SortOrder.Ascending ? FieldSortOrder.Ascending : FieldSortOrder.Descending, fieldByCriterionName.DataConversion) : (SortField) null;
    }

    private string[] generateFieldList()
    {
      List<string> stringList = new List<string>((IEnumerable<string>) InvestorExportDialog.requiredFields);
      foreach (string ruleField in LoanBusinessRuleInfo.RuleFields)
      {
        if (!stringList.Contains(ruleField))
          stringList.Add(ruleField);
      }
      foreach (TableLayout.Column column in this.layoutManager.GetCurrentLayout())
      {
        LoanReportFieldDef fieldByCriterionName = this.fieldDefs.GetFieldByCriterionName(column.ColumnID);
        if (!stringList.Contains(column.ColumnID) && fieldByCriterionName != null)
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

    private void applyTableLayout(TableLayout layout)
    {
      if (this.layoutManager == null)
        this.layoutManager = this.createLayoutManager();
      this.layoutManager.ApplyLayout(layout, false);
      this.gvLoans.Columns.Insert(0, new GVColumn("Export Status")
      {
        Tag = (object) new TableLayout.Column("Export.ULDD.Validation", "Data Validation", "Data Validation", "", HorizontalAlignment.Center, 50, true),
        SortMethod = GVSortMethod.Text
      });
      this.gvLoans.Sort(0, SortOrder.Ascending);
    }

    private QueryCriterion generateQueryCriteria()
    {
      QueryCriterion criterion = (QueryCriterion) null;
      if (this.advFilter != null)
        criterion = this.advFilter.CreateEvaluator().ToQueryCriterion();
      QueryCriterion queryCriteria = (QueryCriterion) null;
      if (criterion != null)
        queryCriteria = queryCriteria == null ? criterion : queryCriteria.And(criterion);
      return queryCriteria;
    }

    private void navPipeline_PageChangedEvent(object sender, PageChangedEventArgs e)
    {
      if (this.suspendEvents)
        return;
      using (CursorActivator.Wait())
        this.displayCurrentPage(false);
    }

    private void displayCurrentPage(bool preserveSelections)
    {
      int currentPageItemIndex = this.navPipeline.CurrentPageItemIndex;
      int currentPageItemCount = this.navPipeline.CurrentPageItemCount;
      PipelineInfo[] pinfos = new PipelineInfo[0];
      if (currentPageItemCount > 0)
        pinfos = (PipelineInfo[]) this.pipelineCursor.GetItems(currentPageItemIndex, currentPageItemCount, false);
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
      if (preserveSelections)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvLoans.Items)
        {
          if (gvItem.Selected)
            dictionary[((PipelineInfo) gvItem.Tag).GUID] = true;
        }
      }
      LoanInfo.Right[] effectiveRightsForLoans = ((LoanAccessBpmManager) Session.BPM.GetBpmManager(BizRuleType.LoanAccess)).GetEffectiveRightsForLoans(pinfos);
      this.gvLoans.Items.Clear();
      if (InvestorExportDialog.guidList == null)
        InvestorExportDialog.guidList = new List<string>();
      else
        InvestorExportDialog.guidList.Clear();
      for (int index = 0; index < pinfos.Length; ++index)
      {
        GVItem itemForPipelineInfo = this.createGVItemForPipelineInfo(pinfos[index], effectiveRightsForLoans[index]);
        this.gvLoans.Items.Add(itemForPipelineInfo);
        if (dictionary.ContainsKey(pinfos[index].GUID))
          itemForPipelineInfo.Selected = true;
      }
      if (this.gvLoans.Items.Count <= 0 || this.gvLoans.SelectedItems.Count != 0)
        return;
      this.gvLoans.Items[0].Selected = true;
    }

    private GVItem createGVItemForPipelineInfo(PipelineInfo pinfo, LoanInfo.Right effectiveRights)
    {
      GVItem itemForPipelineInfo = new GVItem();
      for (int index = 0; index < this.gvLoans.Columns.Count; ++index)
      {
        string columnId = ((TableLayout.Column) this.gvLoans.Columns[index].Tag).ColumnID;
        object obj;
        if (columnId != "Export.ULDD.Validation")
        {
          LoanReportFieldDef fieldByCriterionName = this.fieldDefs.GetFieldByCriterionName(columnId);
          obj = pinfo.Info[(object) columnId];
          if (fieldByCriterionName != null)
            obj = fieldByCriterionName.ToDisplayElement(columnId, pinfo, (Control) this.gvLoans);
        }
        else if (this.investorSetting != null && "Export ULDD to Fannie Mae" != this.investorSetting.DisplayName && "Export ULDD to Freddie Mac" != this.investorSetting.DisplayName && this.loanServices.ExportServiceValidateLoan((LoanDataMgr) null, this.investorSetting, pinfo.GUID))
        {
          obj = (object) "Problem";
          InvestorExportDialog.passDataChecking = false;
        }
        else
          obj = (object) "OK";
        itemForPipelineInfo.SubItems[index].Value = obj;
        itemForPipelineInfo.Tag = (object) pinfo;
        if (!InvestorExportDialog.guidList.Contains(pinfo.GUID))
          InvestorExportDialog.guidList.Add(pinfo.GUID);
        if (pinfo.LockInfo.IsLocked || this.isLoanReadOnly(pinfo, effectiveRights))
        {
          itemForPipelineInfo.BackColor = EncompassColors.Secondary2;
          itemForPipelineInfo.ForeColor = EncompassColors.Secondary5;
        }
        if (string.Concat(pinfo.GetField("Loan.Adverse")) == "Y")
          itemForPipelineInfo.BackColor = EncompassColors.Alert3;
      }
      return itemForPipelineInfo;
    }

    public static bool PassDataCheck => InvestorExportDialog.passDataChecking;

    private bool isLoanReadOnly(PipelineInfo pinfo, LoanInfo.Right effectiveRights)
    {
      if (Session.UserInfo.IsSuperAdministrator())
        return false;
      return effectiveRights == LoanInfo.Right.Read || ((LoanAccessBpmManager) Session.BPM.GetBpmManager(BizRuleType.LoanAccess)).GetLoanContentAccess(pinfo) == LoanContentAccess.None;
    }

    private void gvLoans_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (!e.Item.Selected || this.hasOpenLoan && !this.CloseLoan(true))
        return;
      PipelineInfo tag = (PipelineInfo) e.Item.Tag;
      if (tag == null)
        return;
      if (this.investorSetting.UseLoanTab)
        this.openLoan(tag.GUID);
      else
        this.loanServices.ExportServiceProcessLoan((LoanDataMgr) null, this.investorSetting, tag.GUID);
    }

    private void openLoan(string guid)
    {
      using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("Loan.Open", "Opening a loan file", true, true, 700, nameof (openLoan), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\InvestorExportDialog.cs"))
      {
        if (!this.hasOpenLoan || Session.LoanData.GUID != guid)
        {
          if (this.hasOpenLoan && !this.CloseLoan(true))
            return;
          PipelineInfo pInfo = Session.LoanManager.GetPipeline(new string[1]
          {
            guid
          }, false)[0];
          if (pInfo == null)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The selected loan has been deleted or is not longer accessible.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return;
          }
          LoanInfo.Right userAccessToLoan = this.getUserAccessToLoan(pInfo);
          switch (userAccessToLoan)
          {
            case LoanInfo.Right.NoRight:
              int num1 = (int) Utils.Dialog((IWin32Window) this, "You no longer have the necessary rights to access this loan file. Contact your system administrator if you require further access to this loan.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              return;
            case LoanInfo.Right.Read:
              if (Utils.Dialog((IWin32Window) this, "You only have read-only access to this loan file. You won't be able to save any changes. Do you still want to open this loan?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
                return;
              break;
            default:
              if (string.Compare(pInfo.LoanFolder, SystemSettings.TrashFolder, true) == 0 && Utils.Dialog((IWin32Window) this, "This loan is currently in a Trash folder.  You won't be able to save any changes. Do you still want to open this loan?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
                return;
              break;
          }
          LoanInfo.LockReason lockReason = LoanInfo.LockReason.OpenForWork;
          if (userAccessToLoan == LoanInfo.Right.Read || pInfo.LoanFolder.ToLower() == SystemSettings.TrashFolder.ToLower())
            lockReason = LoanInfo.LockReason.NotLocked;
          string field = (string) pInfo.GetField("ActionTaken");
          if (!EllieMae.EMLite.Common.LoanStatus.ActiveLoan.Contains((object) field))
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "The current status of this loan is \"" + field + "\".   Alerts will not appear in pipeline unless you change the status back to \"Active Loan\" on the Borrower Summary.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          LoanDataMgr loanDataMgr = this.loadLoan(pInfo, lockReason);
          if (loanDataMgr == null)
            return;
          this.startEditor(loanDataMgr);
        }
        performanceMeter.Stop();
        MetricsFactory.RecordIncrementalTimerSample("EMA_Loan_Open", Convert.ToInt64(performanceMeter.Duration.TotalMilliseconds), (SFxTag) new SFxUiTag());
      }
      this.displayEditor();
    }

    private void startEditor(LoanDataMgr loanDataMgr)
    {
      Cursor.Current = Cursors.WaitCursor;
      Session.SetLoanDataMgr(loanDataMgr, true);
      loanDataMgr.LoanData.BaseLastModified = loanDataMgr.LastModified;
      loanDataMgr.LoanData.ULDDExportType = this.investorSetting.ID;
      loanDataMgr.LoanData.Dirty = false;
      this.hasOpenLoan = true;
      this.initializeLoanEditor(false);
      if (loanDataMgr.Writable)
      {
        this.RecoverAutoSavedFile();
        this.StartAutosaveTimer();
      }
      if (loanDataMgr.LoanObject != null)
        loanDataMgr.LoanObject.AddToRecentLoans();
      Session.LoanData.LoanDataModified += MainScreen.Instance.LoanDataModifiedHandler;
      MainForm.Instance.SetLastSaveTime(Session.LoanDataMgr.LastModified, Session.LoanData.Dirty);
      if (Session.StartupInfo.DataServicesOptOut)
        return;
      Session.InitialDataServicesData = DataServicesManager.RetrieveReportData(loanDataMgr);
    }

    public void StartAutosaveTimer()
    {
      if (Session.LoanDataMgr == null || Session.LoanData == null || !Session.LoanDataMgr.Writable || !AutosaveConfigManager.IsAutosaveEnabled())
        return;
      this.timerAutosave.Start();
    }

    public void RecoverAutoSavedFile()
    {
      this.recoverAutoSavedFile(Session.LoanDataMgr);
      if (Session.LoanDataMgr.LinkedLoan == null)
        return;
      this.recoverAutoSavedFile(Session.LoanDataMgr.LinkedLoan);
      Session.LoanDataMgr.LinkTo(Session.LoanDataMgr.LinkedLoan);
    }

    private void removeAutoSavedFiles(
      string loanDataFile,
      string attachmentFile,
      string historyFile,
      string timestampFile)
    {
      if (loanDataFile != null && File.Exists(loanDataFile))
        File.Delete(loanDataFile);
      if (attachmentFile != null && File.Exists(attachmentFile))
        File.Delete(attachmentFile);
      if (historyFile != null && File.Exists(historyFile))
        File.Delete(historyFile);
      if (timestampFile == null || !File.Exists(timestampFile))
        return;
      File.Delete(timestampFile);
    }

    private void recoverAutoSavedFile(LoanDataMgr loanDataMgr)
    {
      if (loanDataMgr == null)
        return;
      string autoRecoverFileName = this.getAutoRecoverFileName(loanDataMgr);
      string recoverAttFileName = this.getAutoRecoverAttFileName(autoRecoverFileName);
      string recoverHistoryFileName = this.getAutoRecoverHistoryFileName(autoRecoverFileName);
      string recoverTsFileName = this.getAutoRecoverTsFileName(autoRecoverFileName);
      if (!File.Exists(autoRecoverFileName))
        return;
      if (loanDataMgr.LoanData == null)
        return;
      try
      {
        DateTime dateTime = Convert.ToDateTime(File.ReadAllText(recoverTsFileName));
        if (loanDataMgr.LastModified > dateTime)
        {
          this.removeAutoSavedFiles(autoRecoverFileName, recoverAttFileName, recoverHistoryFileName, recoverTsFileName);
          return;
        }
      }
      catch
      {
      }
      LoanData loanData;
      try
      {
        InvestorExportDialog.jed.b();
        string xmlData = (string) null;
        using (FileStream A_0 = File.OpenRead(autoRecoverFileName))
          xmlData = InvestorExportDialog.jed.a((Stream) A_0);
        loanData = new LoanData(xmlData, Session.LoanManager.GetLoanSettings());
      }
      catch
      {
        this.removeAutoSavedFiles(autoRecoverFileName, recoverAttFileName, recoverHistoryFileName, recoverTsFileName);
        return;
      }
      string attXml = (string) null;
      try
      {
        InvestorExportDialog.jed.b();
        using (FileStream A_0 = File.OpenRead(recoverAttFileName))
          attXml = InvestorExportDialog.jed.a((Stream) A_0);
      }
      catch
      {
      }
      string hisXml = (string) null;
      try
      {
        InvestorExportDialog.jed.b();
        using (FileStream A_0 = File.OpenRead(recoverHistoryFileName))
          hisXml = InvestorExportDialog.jed.a((Stream) A_0);
      }
      catch
      {
      }
      int num1 = autoRecoverFileName.LastIndexOf("\\");
      string str;
      if (autoRecoverFileName.Substring(num1 + 1, 1) == ".")
      {
        loanData.GUID = loanDataMgr.LoanData.GUID;
        str = "There is Autosave data for a newly-created loan (saved on ";
      }
      else
      {
        if (loanData.GUID != loanDataMgr.LoanData.GUID)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Cannot recover the Autosave data for this loan because the loan GUIDs do not match.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
          return;
        }
        str = "This loan has Autosave data (saved on ";
      }
      DateTime lastWriteTime = File.GetLastWriteTime(autoRecoverFileName);
      if (Utils.Dialog((IWin32Window) this, str + lastWriteTime.ToString("MM/dd/yyyy HH:mm:ss") + ")." + "  Do you want to recover the data?  If you answer No, the Autosave data will be removed and you will not be able to recover it later.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
      {
        using (Stream stream = loanData.ToStream(LoanContentAccess.FullAccess, true))
          loanDataMgr.RecoverAutoSavedLoan(stream, attXml, hisXml);
        loanDataMgr.LoanData.IsAutoSaveFlag = true;
        loanDataMgr.LoanData.AutoSaveDateTime = lastWriteTime;
        this.displayEditor();
      }
      else
        this.removeAutoSavedFiles(autoRecoverFileName, recoverAttFileName, recoverHistoryFileName, recoverTsFileName);
    }

    public void displayEditor()
    {
      if (this.tabControl.TabPages.Count == 1)
        this.tabControl.TabPages.Add(this.loanTabPage);
      this.tabControl.SelectedTab = this.loanTabPage;
    }

    private string getAutoRecoverAttFileName(string autoRecoverFileName)
    {
      return Path.Combine(Path.GetDirectoryName(autoRecoverFileName), Path.GetFileNameWithoutExtension(autoRecoverFileName) + ".att");
    }

    private string getAutoRecoverHistoryFileName(string autoRecoverFileName)
    {
      return Path.Combine(Path.GetDirectoryName(autoRecoverFileName), Path.GetFileNameWithoutExtension(autoRecoverFileName) + ".his");
    }

    private string getAutoRecoverFileName(LoanDataMgr loanDataMgr)
    {
      string autoRecoverFilePath = this.getAutoRecoverFilePath(loanDataMgr);
      if (autoRecoverFilePath == null)
        return (string) null;
      string str = "";
      if (!loanDataMgr.IsNew() && loanDataMgr.LoanData != null && loanDataMgr.LoanData.GUID != null)
        str = loanDataMgr.LoanData.GUID;
      return Path.Combine(autoRecoverFilePath, str + ".tem");
    }

    private string getAutoRecoverTsFileName(string autoRecoverFileName)
    {
      return Path.Combine(Path.GetDirectoryName(autoRecoverFileName), Path.GetFileNameWithoutExtension(autoRecoverFileName) + ".ts");
    }

    private void initializeLoanEditor(bool newLoan)
    {
      if (this.loanPage == null)
      {
        this.loanPage = new LoanPage(Session.DefaultInstance);
        this.loanPage.Parent = (Control) this.loanTabPage;
        this.loanTabPage.Controls.Add((Control) this.loanPage);
      }
      this.loanPage.InitContents(false, !Session.LoanDataMgr.Writable);
    }

    private LoanDataMgr loadLoan(PipelineInfo pInfo, LoanInfo.LockReason lockReason)
    {
      Cursor.Current = Cursors.WaitCursor;
      LoanDataMgr mgr = this.openLoanDataMgr(pInfo.GUID);
      if (mgr == null)
        return (LoanDataMgr) null;
      LoanAccessBpmManager bpmManager = (LoanAccessBpmManager) Session.BPM.GetBpmManager(BpmCategory.LoanAccess);
      LoanContentAccess loancontentAccess = LoanContentAccess.FullAccess;
      if (lockReason != LoanInfo.LockReason.NotLocked)
        loancontentAccess = bpmManager.GetLoanContentAccess(mgr.LoanData);
      try
      {
        if (mgr.AccessRules.AllowFullAccess())
          loancontentAccess = LoanContentAccess.FullAccess;
        mgr.LoanData.ContentAccess = loancontentAccess;
        if (loancontentAccess == LoanContentAccess.None || LoanAccess.GetAccessRightMessage(loancontentAccess) == "")
        {
          if (Utils.Dialog((IWin32Window) Session.MainScreen, Messages.GetMessage("ReadRightOnly"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
          {
            mgr.Close();
            return (LoanDataMgr) null;
          }
          lockReason = LoanInfo.LockReason.NotLocked;
        }
        if (lockReason != LoanInfo.LockReason.NotLocked)
        {
          if (!this.lockLoan(ref mgr, lockReason))
          {
            mgr.Close();
            return (LoanDataMgr) null;
          }
          if (Session.SessionObjects.AllowConcurrentEditing)
          {
            if (mgr.Writable)
            {
              UserShortInfoList workingOnTheLoan = mgr.GetUsersWorkingOnTheLoan(Session.SessionObjects.SessionID, true);
              if (workingOnTheLoan != null)
              {
                if (workingOnTheLoan.Count > 0)
                {
                  if (new CEOpenLoanForm(workingOnTheLoan).ShowDialog((IWin32Window) this) != DialogResult.Yes)
                  {
                    mgr.GetUsersWorkingOnTheLoan(Session.SessionObjects.SessionID, true);
                    mgr.Close();
                    return (LoanDataMgr) null;
                  }
                  Session.ServerManager.SendMessage((EllieMae.EMLite.ClientServer.Message) new CEMessage(Session.UserInfo, CEMessageType.UserOpenLoan), workingOnTheLoan.SessionIDs, true);
                }
              }
            }
          }
        }
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
      return mgr;
    }

    private bool lockLoan(ref LoanDataMgr mgr, LoanInfo.LockReason lockReason)
    {
      bool flag = false;
      string text;
      try
      {
        mgr.Lock(lockReason, LockInfo.ExclusiveLock.Nonexclusive);
        if (mgr.LoanData.ContentAccess == LoanContentAccess.FullAccess)
          return true;
        string accessRightMessage = LoanAccess.GetAccessRightMessage(mgr.LoanData.ContentAccess);
        if (mgr.LoanData.ContentAccess == LoanContentAccess.None || accessRightMessage == string.Empty)
        {
          flag = true;
          text = Messages.GetMessage("ReadRightOnly");
        }
        else
          text = "Your access to this loan file is limited. Only changes made in the following areas will be saved:" + Environment.NewLine + Environment.NewLine + accessRightMessage + Environment.NewLine + "Do you still want to open this loan file?";
      }
      catch (LockException ex1)
      {
        flag = true;
        if (ex1.LockInfo.LockedBy == Session.UserID && !Session.SessionObjects.AllowConcurrentEditing)
          return this.promptUserToUnlockLoan(mgr, lockReason, ex1.LockInfo);
        if ((ex1.LockInfo.LockedBy ?? "").Trim() == "")
        {
          try
          {
            mgr.Unlock(true);
            mgr.Lock(lockReason, LockInfo.ExclusiveLock.Nonexclusive);
          }
          catch (SecurityException ex2)
          {
            if (Utils.Dialog((IWin32Window) Session.MainScreen, Messages.GetMessage("ReadRightOnly"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
              return false;
          }
          return true;
        }
        if (ex1.LockInfo.LockedFor == LoanInfo.LockReason.Downloaded)
          text = Messages.GetMessage("OpenDownLoad", (object) ex1.LockInfo.LockedBy);
        else
          text = Messages.GetMessage("OpenWork", (object) ex1.LockInfo.LockedBy);
      }
      catch (SecurityException ex)
      {
        flag = true;
        text = Messages.GetMessage("ReadRightOnly");
      }
      if (Utils.Dialog((IWin32Window) Session.MainScreen, text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
        return false;
      if (flag)
      {
        string guid = mgr.LoanData.GUID;
        mgr.Close();
        mgr = this.openLoanDataMgr(guid);
        mgr.Lock(LoanInfo.LockReason.NotLocked, LockInfo.ExclusiveLock.Nonexclusive);
      }
      return true;
    }

    private bool promptUserToUnlockLoan(
      LoanDataMgr mgr,
      LoanInfo.LockReason lockReason,
      LockInfo currentLock)
    {
      LoanAccessBpmManager bpmManager = (LoanAccessBpmManager) Session.BPM.GetBpmManager(BpmCategory.LoanAccess);
      LoanInfo.Right effectiveRight = mgr.GetEffectiveRight(Session.UserID);
      string text = "";
      if ((effectiveRight & LoanInfo.Right.Access) == LoanInfo.Right.NoRight)
      {
        if (currentLock.LockedFor == LoanInfo.LockReason.Downloaded)
          text = Messages.GetMessage("OpenDownLoad", (object) currentLock.LockedBy);
        else if (currentLock.LockedFor == LoanInfo.LockReason.OpenForWork)
          text = Messages.GetMessage("OpenWork", (object) currentLock.LockedBy);
        return Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
      }
      switch (new UnlockLoanDialog(currentLock).ShowDialog())
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.No:
          return true;
        default:
          try
          {
            mgr.Unlock(true);
            mgr.Lock(lockReason, LockInfo.ExclusiveLock.Nonexclusive);
          }
          catch (SecurityException ex)
          {
            if (Utils.Dialog((IWin32Window) Session.MainScreen, Messages.GetMessage("ReadRightOnly"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
              return false;
          }
          return true;
      }
    }

    private LoanDataMgr openLoanDataMgr(string guid)
    {
      try
      {
        LoanDataMgr loanDataMgr = LoanDataMgr.OpenLoan(Session.SessionObjects, guid, false);
        loanDataMgr.Calculator.PerformanceEnabled = true;
        if (loanDataMgr.LinkedLoan != null)
        {
          if (loanDataMgr.LinkedLoan.LoanData.LinkGUID == string.Empty)
            loanDataMgr.LinkedLoan.LoanData.SetField("LINKGUID", loanDataMgr.LoanData.GUID);
          loanDataMgr.LinkedLoan.Interactive = true;
          loanDataMgr.LinkedLoan.LoanData.ToPipelineInfo();
          LoanContentAccess loanContentAccess = ((LoanAccessBpmManager) Session.BPM.GetBpmManager(BpmCategory.LoanAccess)).GetLoanContentAccess(loanDataMgr.LinkedLoan.LoanData);
          if (loanDataMgr.LinkedLoan.AccessRules.AllowFullAccess())
            loanContentAccess = LoanContentAccess.FullAccess;
          loanDataMgr.LinkedLoan.LoanData.ContentAccess = loanContentAccess;
          loanDataMgr.LinkedLoan.LoanData.Calculator.PerformanceEnabled = true;
        }
        return loanDataMgr;
      }
      catch (ObjectNotFoundException ex)
      {
        MetricsFactory.IncrementErrorCounter((Exception) ex, "The selected loan has been deleted or is no longer accessible", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\InvestorExportDialog.cs", nameof (openLoanDataMgr), 1291);
        int num = (int) Utils.Dialog((IWin32Window) this, "The selected loan has been deleted or is no longer accessible.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return (LoanDataMgr) null;
      }
      catch (SecurityException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The selected loan cannot be opened because you do not have the necessary permissions.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return (LoanDataMgr) null;
      }
    }

    private LoanInfo.Right getUserAccessToLoan(PipelineInfo pInfo)
    {
      return ((LoanAccessBpmManager) Session.BPM.GetBpmManager(BpmCategory.LoanAccess)).GetEffectiveRightsForLoan(pInfo);
    }

    public bool CloseLoan(bool allowCancel)
    {
      if (!this.hasOpenLoan)
        return true;
      string str1 = (string) null;
      if (Session.LoanDataMgr != null && Session.LoanDataMgr.LoanData != null)
        str1 = Session.LoanDataMgr.LoanData.GUID;
      string str2 = (string) null;
      if (Session.LoanDataMgr.LinkedLoan != null && Session.LoanDataMgr.LinkedLoan.LoanData != null)
        str2 = Session.LoanDataMgr.LinkedLoan.LoanData.GUID;
      if (!this.closeEditor(true, true, false, allowCancel))
        return false;
      this.hasOpenLoan = false;
      return true;
    }

    private bool closeEditor(bool displayPrompts, bool saveLoan, bool forceSave, bool allowCancel)
    {
      if (Session.LoanData.Dirty & saveLoan && !this.promptForSave(displayPrompts, forceSave, allowCancel))
        return false;
      if (Session.LoanDataMgr.Writable & displayPrompts)
      {
        StatusOnlineManager.CheckStatusOnline(Session.LoanDataMgr);
        LoanServiceManager.CheckLoan(Session.LoanDataMgr);
      }
      FieldHelpDialog.Close();
      this.loanPage.Unload();
      if (Session.SessionObjects.AllowConcurrentEditing && Session.LoanDataMgr.Writable)
      {
        UserShortInfoList workingOnTheLoan = Session.LoanDataMgr.GetUsersWorkingOnTheLoan(Session.SessionObjects.SessionID, true);
        if (workingOnTheLoan != null && workingOnTheLoan.Count > 0)
          Session.ServerManager.SendMessage((EllieMae.EMLite.ClientServer.Message) new CEMessage(Session.UserInfo, CEMessageType.UserExitLoan), workingOnTheLoan.SessionIDs, true);
      }
      this.clearCurrentLoan();
      this.tabControl.SelectedTab = this.pipelineTabPage;
      this.tabControl.Visible = false;
      this.tabControl.TabPages.Remove(this.loanTabPage);
      this.tabControl.Visible = true;
      return true;
    }

    private void clearCurrentLoan()
    {
      if (Session.LoanData != null)
        Session.SetLoanDataMgr((LoanDataMgr) null);
      this.hasOpenLoan = false;
    }

    private bool promptForSave(bool displayPrompts, bool forceSave, bool allowCancel)
    {
      if (!Session.LoanDataMgr.Writable)
      {
        if (displayPrompts)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The current loan is opened in read-only mode. All the changes you made will be lost.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        return true;
      }
      if (!forceSave & displayPrompts)
      {
        MessageBoxButtons buttons = MessageBoxButtons.YesNo;
        if (allowCancel)
          buttons = MessageBoxButtons.YesNoCancel;
        switch (Utils.Dialog((IWin32Window) this, "Do you want to save the changes to the current loan?", buttons, MessageBoxIcon.Exclamation))
        {
          case DialogResult.Cancel:
            return false;
          case DialogResult.No:
            return true;
        }
      }
      if (displayPrompts)
      {
        if (RegulationAlertDialog.DisplayAlerts((IWin32Window) this) == DialogResult.Cancel)
          return false;
        LoanServiceManager.SaveLoan();
        if (Session.LoanData.GetField("1240") == string.Empty && Session.LoanData.GetField("3040") != "Y")
        {
          using (EmailCheckDialog emailCheckDialog = new EmailCheckDialog())
          {
            DialogResult dialogResult = emailCheckDialog.ShowDialog((IWin32Window) this);
            if (dialogResult != DialogResult.Cancel)
              Session.LoanData.SetField("3040", emailCheckDialog.DoNotShowAgain ? "Y" : "");
            if (dialogResult == DialogResult.OK)
              Session.LoanData.SetField("1240", emailCheckDialog.emailAddress);
          }
        }
      }
      return this.SaveLoan(false, false);
    }

    public bool SaveLoan(bool mergeOnly, bool refreshScreen)
    {
      using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("Loan.Save", "Saving a loan file to the Encompass Server", true, 1467, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\InvestorExportDialog.cs"))
      {
        ILoanMilestoneTemplateOrchestrator milestoneTemplateController = (ILoanMilestoneTemplateOrchestrator) new StandardMilestoneTemplateApply(Session.DefaultInstance, false, true, ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.LoanTab_DisplayMilestoneChangeScreen));
        MetricsFactory.IncrementCounter("LoanSaveIncCounter", (SFxTag) new SFxInternalTag());
        using (MetricsFactory.GetIncrementalTimer("LoanSaveIncTimer", (SFxTag) new SFxInternalTag()))
        {
          bool flag = Session.LoanDataMgr.SaveLoan(mergeOnly, true, false, milestoneTemplateController, false, out bool _);
          if (refreshScreen)
            this.RefreshContents();
          performanceMeter.Stop();
          MetricsFactory.RecordIncrementalTimerSample("EMA_Loan_Save", Convert.ToInt64(performanceMeter.Duration.TotalMilliseconds), (SFxTag) new SFxUiTag());
          return flag;
        }
      }
    }

    public void RefreshContents()
    {
      if (this.tabControl.SelectedTab != this.loanTabPage)
        return;
      this.loanPage.RefreshContents();
    }

    private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.tabControl.SelectedTab == this.pipelineTabPage)
      {
        if (this.hasOpenLoan && Session.LoanData.Dirty && Utils.Dialog((IWin32Window) this, "Do you want to save your changes to the loan file first?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          this.loanPage.SaveLoan();
        this.RefreshPipeline(true);
        this.btnExport.Enabled = true;
      }
      else
        this.btnExport.Enabled = false;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void ULDDExportDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.hasOpenLoan && !this.CloseLoan(true))
        return;
      InvestorExportDialog.instance = (InvestorExportDialog) null;
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
      if (this.hasOpenLoan && !this.CloseLoan(true))
        return;
      this.DialogResult = DialogResult.OK;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (InvestorExportDialog));
      this.pnlAction = new Panel();
      this.btnExport = new Button();
      this.btnCancel = new Button();
      this.pipelineTabPage = new TabPage();
      this.groupContainer1 = new GroupContainer();
      this.navPipeline = new PageListNavigator();
      this.gvLoans = new GridView();
      this.loanTabPage = new TabPage();
      this.tabControl = new TabControl();
      this.timerAutosave = new Timer(this.components);
      this.gradientPanel1 = new GradientPanel();
      this.label1 = new Label();
      this.pnlAction.SuspendLayout();
      this.pipelineTabPage.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.tabControl.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.SuspendLayout();
      this.pnlAction.Controls.Add((Control) this.btnExport);
      this.pnlAction.Controls.Add((Control) this.btnCancel);
      this.pnlAction.Dock = DockStyle.Bottom;
      this.pnlAction.Location = new Point(0, 456);
      this.pnlAction.Name = "pnlAction";
      this.pnlAction.Size = new Size(859, 44);
      this.pnlAction.TabIndex = 3;
      this.btnExport.Location = new Point(8, 9);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new Size(75, 23);
      this.btnExport.TabIndex = 2;
      this.btnExport.Text = "Export";
      this.btnExport.UseVisualStyleBackColor = true;
      this.btnExport.Click += new EventHandler(this.btnExport_Click);
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(772, 9);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.pipelineTabPage.Controls.Add((Control) this.groupContainer1);
      this.pipelineTabPage.Location = new Point(4, 32);
      this.pipelineTabPage.Name = "pipelineTabPage";
      this.pipelineTabPage.Padding = new Padding(0, 2, 2, 2);
      this.pipelineTabPage.Size = new Size(851, 388);
      this.pipelineTabPage.TabIndex = 2;
      this.pipelineTabPage.Tag = (object) "Pipeline";
      this.pipelineTabPage.Text = "Pipeline";
      this.pipelineTabPage.UseVisualStyleBackColor = true;
      this.groupContainer1.Controls.Add((Control) this.navPipeline);
      this.groupContainer1.Controls.Add((Control) this.gvLoans);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 2);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(849, 384);
      this.groupContainer1.TabIndex = 5;
      this.navPipeline.BackColor = System.Drawing.Color.Transparent;
      this.navPipeline.Font = new Font("Arial", 8f);
      this.navPipeline.Location = new Point(4, 2);
      this.navPipeline.Name = "navPipeline";
      this.navPipeline.NumberOfItems = 0;
      this.navPipeline.Size = new Size(254, 24);
      this.navPipeline.TabIndex = 5;
      this.navPipeline.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.navPipeline_PageChangedEvent);
      this.gvLoans.BorderStyle = BorderStyle.None;
      this.gvLoans.Dock = DockStyle.Fill;
      this.gvLoans.HotTrackingColor = System.Drawing.Color.FromArgb(250, 248, 188);
      this.gvLoans.Location = new Point(1, 26);
      this.gvLoans.Name = "gvLoans";
      this.gvLoans.Size = new Size(847, 357);
      this.gvLoans.TabIndex = 4;
      this.gvLoans.ItemDoubleClick += new GVItemEventHandler(this.gvLoans_ItemDoubleClick);
      this.loanTabPage.Location = new Point(4, 32);
      this.loanTabPage.Name = "loanTabPage";
      this.loanTabPage.Padding = new Padding(0, 2, 2, 2);
      this.loanTabPage.Size = new Size(851, 388);
      this.loanTabPage.TabIndex = 3;
      this.loanTabPage.Tag = (object) "Loans";
      this.loanTabPage.Text = "Loan";
      this.loanTabPage.UseVisualStyleBackColor = true;
      this.tabControl.Controls.Add((Control) this.pipelineTabPage);
      this.tabControl.Controls.Add((Control) this.loanTabPage);
      this.tabControl.Dock = DockStyle.Fill;
      this.tabControl.HotTrack = true;
      this.tabControl.ItemSize = new Size(42, 28);
      this.tabControl.Location = new Point(0, 32);
      this.tabControl.Name = "tabControl";
      this.tabControl.Padding = new Point(11, 3);
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new Size(859, 424);
      this.tabControl.TabIndex = 2;
      this.tabControl.SelectedIndexChanged += new EventHandler(this.tabControl_SelectedIndexChanged);
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = System.Drawing.Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(859, 32);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel1.TabIndex = 4;
      this.label1.AutoSize = true;
      this.label1.BackColor = System.Drawing.Color.Transparent;
      this.label1.Location = new Point(8, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(776, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "The Export Status column indicates problem loans with incomplete or invalid data. Double-click to open the loan. Missing or invalid fields are highlighted on the form.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.WhiteSmoke;
      this.ClientSize = new Size(859, 500);
      this.Controls.Add((Control) this.tabControl);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Controls.Add((Control) this.pnlAction);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (InvestorExportDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Export Problem";
      this.FormClosing += new FormClosingEventHandler(this.ULDDExportDialog_FormClosing);
      this.pnlAction.ResumeLayout(false);
      this.pipelineTabPage.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.tabControl.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
