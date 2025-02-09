// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.ThinThick.PipelineBrowser
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.ThinThick;
using EllieMae.EMLite.Common.ThinThick.Requests;
using EllieMae.EMLite.Common.ThinThick.Responses;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls.ThinThick;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.Import;
using EllieMae.EMLite.LoanServices;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.ThinThick.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI.ThinThick
{
  public class PipelineBrowser : BrowserBase, IPipeline, IMenuProvider, IOnlineHelpTarget, IBrowser
  {
    private const string ClassName = "PipelineBrowser";
    private static readonly string Sw = Tracing.SwOutsideLoan;
    private static readonly string[] RequiredFields = new string[11]
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
    private PipelineView _currentView;
    private FieldFilterList _advFilter;
    private string _folderName;
    private LoanReportFieldDefs _fieldDefs;
    private PersonaPipelineView[] _personaViews;
    public List<PipelineViewListItem> Views = new List<PipelineViewListItem>();
    private bool _allExternalOrgs;
    private List<ExternalOriginatorManagementData> _externalOrgsList;
    private string _externalOrgName;
    private string _externalOrgId;
    private ToolStripItem _menuItemManageAlerts;
    public ToolStripMenuItem ExportFannieMae;
    private readonly ThinThickBrowserManager _browserManager;
    private readonly OpPipeline _opPipeline = new OpPipeline();
    private readonly OpSimpleRequest _simpleRequest;
    private string _helpTargetName = "PipelinePage";
    private IContainer components;

    public ThinPipelineInfo[] ThinPipelineInfos { get; set; }

    public bool Flag { get; set; }

    public QueryCriterion Filter { get; set; }

    public SortField[] SortFields { get; set; }

    public string[] Fields { get; set; }

    public LoanReportFieldDefs FieldDefs => this._fieldDefs;

    public PipelineView CurrentView => this._currentView;

    public PipelineBrowser()
    {
      this.InitializeComponent();
      this.ThinPipelineInfos = new ThinPipelineInfo[0];
      this._browserManager = new ThinThickBrowserManager(Session.DefaultInstance, this.Browser, (IWin32Window) this);
      this._simpleRequest = new OpSimpleRequest(this._browserManager.CommandContext);
      this._browserManager.NavigatePage(PageRegistry.PipelinePage);
      Session.Application.RegisterService((object) this, typeof (IPipeline));
      this.PopulateServicesMenuItem();
      if (Session.UserInfo.IsSuperAdministrator() || this.ExportFannieMae == null)
        return;
      this.ExportFannieMae.Visible = false;
    }

    public void Initialize(ToolStripItemCollection toolStripItemCollection)
    {
      this.MenuItemCollection = toolStripItemCollection;
      this._menuItemManageAlerts = toolStripItemCollection["tsMenuItemManageAlerts"];
      PipelineScreenData pipelineScreenData = Session.LoanManager.GetPipelineScreenData();
      this._fieldDefs = LoanReportFieldDefs.GetFieldDefs(Session.DefaultInstance, LoanReportFieldFlags.DatabaseFieldsNoAudit);
      this._personaViews = pipelineScreenData.PersonaViews;
      this.LoadViewList(pipelineScreenData.CustomViews);
    }

    public OpSimpleResponse ExportToExcel(OpExportRequest request)
    {
      return this._opPipeline.ExportToExcel(request);
    }

    public OpSimpleResponse Print(OpSimpleRequest request)
    {
      OpSimpleResponse opSimpleResponse = new OpSimpleResponse();
      opSimpleResponse.ErrorCode = ErrorCodes.NotImplemented;
      opSimpleResponse.ErrorMessage = "Pipeline Browser does not implement print function, use printForms function instead.";
      return opSimpleResponse;
    }

    public List<string> Guids
    {
      get
      {
        List<string> guids = new List<string>();
        if (this.ThinPipelineInfos == null)
          return guids;
        guids.AddRange(((IEnumerable<ThinPipelineInfo>) this.ThinPipelineInfos).Select<ThinPipelineInfo, string>((Func<ThinPipelineInfo, string>) (thinPipelineInfo => thinPipelineInfo.LoanGuid.ToString("B"))));
        return guids;
      }
    }

    public string FolderName
    {
      get => this._folderName;
      set
      {
        if (!(this._folderName != value))
          return;
        this._folderName = value;
        this.SelectedFolderChanged();
      }
    }

    public bool SetSelectedPipelineView(string pipelineViewName, string personaName)
    {
      if (!string.IsNullOrEmpty(personaName))
      {
        PipelineViewListItem pipelineViewListItem = this.Views.Find((Predicate<PipelineViewListItem>) (x => x.Static && x.PersonaView.Name.Equals(pipelineViewName, StringComparison.OrdinalIgnoreCase) && x.PersonaView.PersonaName.Equals(personaName, StringComparison.OrdinalIgnoreCase)));
        if (pipelineViewListItem == null)
          return false;
        this._currentView = this.GetPipelineViewForPersonaPipelineView(pipelineViewListItem.PersonaView);
      }
      else
      {
        PipelineViewListItem pipelineViewListItem = this.Views.Find((Predicate<PipelineViewListItem>) (x => !x.Static && x.FileSystemEntry.Name.Equals(pipelineViewName, StringComparison.OrdinalIgnoreCase)));
        if (pipelineViewListItem == null)
          return false;
        this._currentView = (PipelineView) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.PipelineView, pipelineViewListItem.FileSystemEntry);
      }
      this.SetSelectedPipelineView();
      return true;
    }

    public bool SetSelectedPipelineView(string pipelineViewXml)
    {
      return this.SetSelectedPipelineView((PipelineView) new XmlSerializer().Deserialize(pipelineViewXml, typeof (PipelineView)));
    }

    public bool SetSelectedPipelineView(PipelineView pipelineView)
    {
      this._currentView = pipelineView;
      this.SetSelectedPipelineView();
      return true;
    }

    private void SetSelectedPipelineView()
    {
      this._advFilter = this._currentView.Filter;
      if (!string.IsNullOrEmpty(this._currentView.ExternalOrgId))
      {
        if (this._externalOrgsList == null)
          this.LoadTpoSettings();
        ExternalOriginatorManagementData originatorManagementData = this._externalOrgsList.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (e1 => e1.ExternalID == this._currentView.ExternalOrgId));
        if (originatorManagementData != null)
        {
          this._externalOrgName = originatorManagementData.OrganizationName;
          this._externalOrgId = originatorManagementData.ExternalID;
        }
      }
      else
      {
        this._externalOrgName = "All";
        this._externalOrgId = "-1";
      }
      this.FolderName = this.CurrentView.LoanFolder;
    }

    private PipelineView GetPipelineViewForPersonaPipelineView(PersonaPipelineView personaView)
    {
      return PipelineViewListItem.GetPipelineViewForPersonaPipelineView(personaView, (ReportFieldDefContainer) this._fieldDefs, new ReportFieldCommonExtension.GetFieldDefWidth(ReportFieldClientExtension.GetDefaultColumnWidth));
    }

    private void SelectedFolderChanged()
    {
      LoanFolderInfo loanFolderInfo = ((IEnumerable<LoanFolderInfo>) Session.LoanManager.GetPipelineScreenData().LoanFolders).FirstOrDefault<LoanFolderInfo>((Func<LoanFolderInfo, bool>) (x => x.DisplayName == this.FolderName));
      this.SetEnabledStates();
      if (loanFolderInfo == null)
        return;
      Session.WorkingFolder = loanFolderInfo.Name;
    }

    private void SetEnabledStates()
    {
      LoanFolderInfo loanFolderInfo = ((IEnumerable<LoanFolderInfo>) Session.LoanManager.GetPipelineScreenData().LoanFolders).FirstOrDefault<LoanFolderInfo>((Func<LoanFolderInfo, bool>) (x => x.DisplayName == this.FolderName));
      bool flag = loanFolderInfo != null && loanFolderInfo.Type == LoanFolderInfo.LoanFolderType.Trash;
      this._menuItemManageAlerts.Enabled = this.ExactlyOneLoanSelected && !flag;
      if (this.ExportFannieMae != null)
        this.ExportFannieMae.Enabled = false;
      if (!this.ExactlyOneLoanSelected)
        return;
      if (Session.UserInfo.IsSuperAdministrator())
      {
        if (this.ExportFannieMae == null)
          return;
        this.ExportFannieMae.Enabled = true;
      }
      else
      {
        if (this.ExportFannieMae == null)
          return;
        this.ExportFannieMae.Visible = false;
      }
    }

    public void SetHelpTargetName(string name) => this._helpTargetName = name;

    public string GetHelpTargetName() => this._helpTargetName;

    public ServiceSetting GetServiceSetting(string categoryName, string dataServiceId)
    {
      return ServicesMapping.GetServiceSetting(categoryName).Find((Predicate<ServiceSetting>) (x => x.DataServiceID.Equals(dataServiceId)));
    }

    public void toolStripMenuItem1_Click(object sender, EventArgs e)
    {
      this.MenuClicked((ToolStripItem) sender);
    }

    private void PopulateServicesMenuItem()
    {
      List<string> categories = ServicesMapping.Categories;
      ILoanServices service = Session.Application.GetService<ILoanServices>();
      ExportServicesAclManager aclManager = (ExportServicesAclManager) Session.ACL.GetAclManager(AclCategory.ExportServices);
      foreach (string str in categories)
      {
        ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(str);
        foreach (ServiceSetting serviceSetting in ServicesMapping.GetServiceSetting(str))
        {
          if (serviceSetting.SupportedInCurrentVersion() && service.IsExportServiceAccessible(Session.LoanDataMgr, serviceSetting) && aclManager.GetUserApplicationRightForPipelineServices(serviceSetting.CategoryName))
          {
            ToolStripMenuItem serviceMenuItem = this.CreateServiceMenuItem(serviceSetting);
            toolStripMenuItem.DropDownItems.Add((ToolStripItem) serviceMenuItem);
          }
        }
      }
    }

    private ToolStripMenuItem CreateServiceMenuItem(ServiceSetting service)
    {
      ToolStripMenuItem serviceMenuItem = new ToolStripMenuItem(service.DisplayName);
      if (service.LoanFileSpecific)
      {
        ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem("&Selected Loans Only...");
        toolStripMenuItem1.Click += new EventHandler(this.serviceItem_Click);
        ServiceSetting serviceSetting1 = service.Clone();
        serviceSetting1.Tag = (object) "Selected";
        toolStripMenuItem1.Tag = (object) serviceSetting1;
        ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem("&All Loans on All Pages...");
        toolStripMenuItem2.Click += new EventHandler(this.serviceItem_Click);
        ServiceSetting serviceSetting2 = service.Clone();
        serviceSetting2.Tag = (object) "All";
        toolStripMenuItem2.Tag = (object) serviceSetting2;
        serviceMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
        {
          (ToolStripItem) toolStripMenuItem1,
          (ToolStripItem) toolStripMenuItem2
        });
      }
      else
        serviceMenuItem.Click += new EventHandler(this.serviceItem_Click);
      serviceMenuItem.Tag = (object) service;
      return serviceMenuItem;
    }

    private void serviceItem_Click(object sender, EventArgs e)
    {
      this.MenuClicked((ToolStripItem) sender);
    }

    public void MenuClicked(ToolStripItem menuItem)
    {
      string empty = string.Empty;
      string str1;
      if (menuItem.Tag.Equals((object) "SRV_FannieMaeFormattedFile"))
        str1 = menuItem.Tag.ToString();
      else if (menuItem.Tag is ServiceSetting)
      {
        ServiceSetting tag = (ServiceSetting) menuItem.Tag;
        string str2 = string.IsNullOrEmpty((string) tag.Tag) ? string.Empty : "_";
        str1 = string.Format("SRV_{0}{1}{2}", (object) tag.ID, (object) str2, tag.Tag);
      }
      else
        str1 = menuItem.Tag.ToString();
      this.Browser.InvokeScript(ScriptRegistry.EncompassInteractionMenuClicked, (object[]) new string[1]
      {
        str1
      });
    }

    public void SetMenu(ToolStripItemCollection menuItemCollection)
    {
      throw new NotImplementedException();
    }

    public void ImportLoans()
    {
      using (ImportMain importMain = new ImportMain())
      {
        importMain.LoanImported += new EventHandler(this.importMainForm_LoanImported);
        int num = (int) importMain.ShowDialog((IWin32Window) MainScreen.Instance);
      }
    }

    private void importMainForm_LoanImported(object sender, EventArgs e)
    {
      LoanDataMgr loanDataMgr = (LoanDataMgr) sender;
      if (loanDataMgr == null || loanDataMgr.LoanData == null)
        return;
      BorrowerPair[] borrowerPairs = loanDataMgr.LoanData.GetBorrowerPairs();
      if (borrowerPairs == null || borrowerPairs.Length == 0)
        return;
      for (int index = 0; index < borrowerPairs.Length; ++index)
        LoanServiceManager.NewBorrowerPairCreated(borrowerPairs[index].Id, loanDataMgr);
    }

    public void OpenLoan(string guid)
    {
      try
      {
        Session.Application.GetService<ILoanConsole>().OpenLoan(guid);
      }
      catch (Exception ex)
      {
        Tracing.Log(PipelineBrowser.Sw, nameof (PipelineBrowser), TraceLevel.Error, "Error opening loan: " + (object) ex);
        ErrorDialog.Display(ex);
      }
    }

    public ICursor PipelineCursor
    {
      get
      {
        if (this._currentView.LoanFolder != null)
          this.FolderName = this._currentView.LoanFolder;
        this.Filter = this.GenerateQueryCriteria();
        this.Fields = this.GenerateFieldList();
        this.Flag = this._currentView.LoanOrgType == PipelineViewLoanOrgType.TPO;
        TableLayout.Column[] columnsByPriority = this._currentView.Layout.GetSortColumnsByPriority();
        List<SortField> sortFieldList = new List<SortField>();
        if (columnsByPriority.Length != 0)
        {
          for (int index = 0; index < columnsByPriority.Length; ++index)
            sortFieldList.Add(new SortField(columnsByPriority[index].ColumnID, columnsByPriority[index].SortOrder == SortOrder.Ascending ? FieldSortOrder.Ascending : FieldSortOrder.Descending));
        }
        this.SortFields = sortFieldList.ToArray();
        return Session.LoanManager.OpenPipeline(this.FolderName == SystemSettings.AllFolders ? (string) null : this.FolderName, LoanInfo.Right.Access, this.Fields, PipelineData.Lock | PipelineData.Milestones | PipelineData.LoanAssociates | PipelineData.CurrentUserAccessRightsOnly, this.SortFields, this.Filter, this.Flag);
      }
    }

    private string[] GenerateFieldList()
    {
      List<string> stringList = new List<string>((IEnumerable<string>) PipelineBrowser.RequiredFields);
      foreach (string ruleField in LoanBusinessRuleInfo.RuleFields)
      {
        if (!stringList.Contains(ruleField))
          stringList.Add(ruleField);
      }
      foreach (TableLayout.Column column in this._currentView.Layout)
      {
        LoanReportFieldDef fieldByCriterionName = this._fieldDefs.GetFieldByCriterionName(column.ColumnID);
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

    private void LoadTpoSettings()
    {
      bool flag = false;
      if (Session.StartupInfo.UserAclFeatureRights.Contains((object) AclFeature.ExternalSettings_ContactSalesRep) && Session.StartupInfo.UserAclFeatureRights.Contains((object) AclFeature.ExternalSettings_OrganizationSettings))
        flag = !(bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.ExternalSettings_ContactSalesRep] && (bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.ExternalSettings_OrganizationSettings];
      if (Session.UserInfo.IsAdministrator() | flag)
      {
        this._allExternalOrgs = true;
        this._externalOrgsList = Session.ConfigurationManager.GetAllExternalParentOrganizations(false);
      }
      else
        this._externalOrgsList = (List<ExternalOriginatorManagementData>) Session.ConfigurationManager.GetExternalAndInternalUserAndOrgBySalesRep(Session.UserID, Session.UserInfo.OrgId)[1];
    }

    private QueryCriterion GenerateQueryCriteria()
    {
      QueryCriterion criterion1 = (QueryCriterion) null;
      if (this.FolderName == SystemSettings.AllFolders)
        criterion1 = (QueryCriterion) new StringValueCriterion("Loan.LoanFolder", SystemSettings.TrashFolder, StringMatchType.Exact, false);
      QueryCriterion criterion2 = (QueryCriterion) null;
      QueryCriterion criterion3 = (QueryCriterion) null;
      if (this._currentView.LoanOrgType == PipelineViewLoanOrgType.TPO)
      {
        if (this._currentView.LoanOwnership == PipelineViewLoanOwnership.User)
          criterion2 = (QueryCriterion) new StringValueCriterion("Loan.TPOLOID", "-1", StringMatchType.NotEquals);
        else if (this._currentView.LoanOwnership == PipelineViewLoanOwnership.All && this._allExternalOrgs)
          criterion2 = new StringValueCriterion("Loan.TPOLOID", (string) null, StringMatchType.NotEquals).And((QueryCriterion) new StringValueCriterion("Loan.TPOLOID", "", StringMatchType.NotEquals)).Or(new StringValueCriterion("Loan.TPOLPID", (string) null, StringMatchType.NotEquals).And((QueryCriterion) new StringValueCriterion("Loan.TPOLPID", "", StringMatchType.NotEquals)));
        if (this._allExternalOrgs)
        {
          QueryCriterion criterion4 = new StringValueCriterion("Loan.TPOCompanyID", (string) null, StringMatchType.NotEquals).And((QueryCriterion) new StringValueCriterion("Loan.TPOCompanyID", "", StringMatchType.NotEquals));
          criterion2 = criterion2.And(criterion4);
        }
        if (this._externalOrgName != "" && this._externalOrgName != "All")
          criterion3 = (QueryCriterion) new StringValueCriterion("Loan.TPOCompanyID", this._externalOrgId, StringMatchType.Exact);
      }
      else if (this._currentView.LoanOwnership == PipelineViewLoanOwnership.User)
        criterion2 = (QueryCriterion) new StringValueCriterion("LoanAssociateUser.UserID", Session.UserID, StringMatchType.Exact);
      QueryCriterion criterion5 = (QueryCriterion) null;
      if (this._advFilter != null)
        criterion5 = this._advFilter.CreateEvaluator().ToQueryCriterion();
      QueryCriterion queryCriteria = (QueryCriterion) null;
      if (criterion1 != null)
        queryCriteria = queryCriteria == null ? criterion1 : queryCriteria.And(criterion1);
      if (criterion2 != null)
        queryCriteria = queryCriteria == null ? criterion2 : queryCriteria.And(criterion2);
      if (criterion3 != null)
        queryCriteria = queryCriteria == null ? criterion3 : queryCriteria.And(criterion3);
      if (criterion5 != null)
        queryCriteria = queryCriteria == null ? criterion5 : queryCriteria.And(criterion5);
      return queryCriteria;
    }

    public bool SetMenuItemState(ToolStripItem menuItem)
    {
      switch (string.Concat(menuItem.Tag))
      {
        case "PI_Import":
          LoanFolderAclInfo[] foldersForImport = Session.StartupInfo.AccessibleFoldersForImport;
          return foldersForImport != null && foldersForImport.Length != 0 && ((IEnumerable<LoanFolderAclInfo>) foldersForImport).Any<LoanFolderAclInfo>((Func<LoanFolderAclInfo, bool>) (aclInfo => aclInfo.MoveFromAccess == 1 || aclInfo.MoveToAccess == 1));
        case "PI_ManageAlerts":
          return Session.ACL.IsAuthorizedForFeature(AclFeature.LoanMgmt_Pipeline_Alert) && this.ExactlyOneLoanSelected;
        default:
          if (!(menuItem.Tag is ServiceSetting))
            return true;
          ServiceSetting tag = (ServiceSetting) menuItem.Tag;
          if (!Session.Application.GetService<ILoanServices>().IsExportServiceAccessible(Session.LoanDataMgr, tag))
            return false;
          if (string.Concat(tag.Tag) == "Selected")
            menuItem.Enabled = this.OneOrMoreLoansSelected;
          return true;
      }
    }

    private bool ExactlyOneLoanSelected => this.ThinPipelineInfos.Length == 1;

    private bool OneOrMoreLoansSelected => this.ThinPipelineInfos.Length >= 1;

    private void LoadViewList(FileSystemEntry[] fsEntries)
    {
      try
      {
        this.Views.Clear();
        foreach (PersonaPipelineView personaView in this._personaViews)
          this.Views.Add(new PipelineViewListItem(personaView));
        foreach (FileSystemEntry fsEntry in fsEntries)
          this.Views.Add(new PipelineViewListItem(fsEntry));
      }
      catch (Exception ex)
      {
        Tracing.Log(PipelineBrowser.Sw, nameof (PipelineBrowser), TraceLevel.Error, "Error loading view list: " + (object) ex);
        ErrorDialog.Display(ex);
      }
    }

    public string[] GetAvailableViewNames() => new List<string>().ToArray();

    public void RefreshViewableColumns()
    {
    }

    public void DisplayPipeline(string folderName, string viewName)
    {
      this.DisplayPipeline(folderName, viewName, 0);
    }

    public void DisplayPipeline(
      string folderName,
      string viewName,
      int sqlRead,
      bool fromHomePage = false)
    {
    }

    public void DisplayPipeline(string folderName, string viewName, int sqlRead)
    {
    }

    public void DisplayPipeline(
      bool usersLoansOnly,
      FieldFilterList filters,
      SortField[] sortFields)
    {
      this.DisplayPipeline(usersLoansOnly, filters, sortFields, 0);
    }

    public void DisplayPipeline(
      bool usersLoansOnly,
      FieldFilterList filters,
      SortField[] sortFields,
      int sqlRead)
    {
    }

    public void DisplayPipeline(
      string loanFolder,
      bool usersLoansOnly,
      FieldFilterList filters,
      SortField[] sortFields)
    {
      this.DisplayPipeline(loanFolder, usersLoansOnly, filters, sortFields, 0);
    }

    public void DisplayPipeline(
      string loanFolder,
      bool usersLoansOnly,
      FieldFilterList filters,
      SortField[] sortFields,
      int sqlRead)
    {
    }

    public void InvalidatePipeline()
    {
    }

    public void RefreshPipeline()
    {
    }

    public void RefreshPipeline(bool preserveSelection)
    {
    }

    public void RefreshPipeline(bool preserveSelection, bool autoRefresh)
    {
    }

    public void EnableRefreshTimer()
    {
    }

    public void RefreshFolders()
    {
    }

    public void DisplayAlertMessage()
    {
    }

    public void DisableRefreshTimer()
    {
    }

    public string GetSamplePageName() => "encompass.pipeline.html";

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Name = nameof (PipelineBrowser);
      this.Size = new Size(0, 0);
      this.ResumeLayout(false);
    }
  }
}
