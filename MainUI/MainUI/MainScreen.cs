// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.MainScreen
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.ElliEnum.Triggers;
using Elli.Metrics.Client;
using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.DataDocs;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.Trading;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ConcurrentEditing;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.ContactUI.CustomFields;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DashBoard;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DataServices;
using EllieMae.EMLite.eFolder.Conditions;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.ePass.Messaging;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.HomePage;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.InputEngine.MilestoneManagement;
using EllieMae.EMLite.JedLib;
using EllieMae.EMLite.LoanServices;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.Log;
using EllieMae.EMLite.MainUI.ThinThick;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.Setup.Autosave;
using EllieMae.EMLite.StatusOnline;
using EllieMae.EMLite.ThinThick;
using EllieMae.EMLite.Trading;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.Workflow;
using Encompass.Diagnostics;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class MainScreen : 
    Form,
    IMainScreen,
    IWin32Window,
    IApplicationWindow,
    ISynchronizeInvoke,
    IRefreshContents,
    IEncompassApplication,
    IApplicationScreen,
    ILoanConsole,
    ISaveLoan
  {
    private const string className = "MainScreen";
    protected static string sw = Tracing.SwOutsideLoan;
    private TabPage[] tabPages = new TabPage[8];
    private static b jed = (b) null;
    private Hashtable browserWindows = new Hashtable();
    public LoanDataModifiedEventHandler LoanDataModifiedHandler;
    private IContainer components;
    private Dictionary<System.Type, object> services = new Dictionary<System.Type, object>();
    private TabControl tabControl;
    private TabPage pipelineTabPage;
    private TabPage loanTabPage;
    private TabPage ePASSTabPage;
    private TabPage contactTabPage;
    private TabPage homeTabPage;
    private TabPage dashboardTabPage;
    private TabPage reportsTabPage;
    private TabPage tradesTabPage;
    private TabPage defaultTabAfterLoanClosed;
    public PipelineScreen PipelineScreen;
    public PipelineBrowser PipelineBrowser;
    public IPipeline PipelineScreenBrowser;
    public IMenuProvider PipelineScreenBrowserMenuProvider;
    private TradeManagementConsole tradingConsole;
    public TradingBrowser TradingBrowser;
    public ITradeConsole TradingScreenBrowser;
    public IMenuProvider TradingScreenBrowserMenuProvider;
    private int loanOpenCount;
    private LoanPage loanPage;
    private EpassControl epassControl;
    private ContactMainForm contactPage;
    private HomePageControl homeControl;
    private DashboardForm frmDashboard;
    private ReportMainControl reportControl;
    private SetUpContainer setupPanel;
    public static Task<bool> initializeInvestorServicesTask;
    private static MainScreen instance;
    private System.Windows.Forms.Timer timerAutosave;
    private bool hasOpenLoan;
    public static readonly bool IMEnabled;
    private Panel pnlMain;
    public static readonly bool CSEnabled;
    private bool saveButtonIsClicked;
    private static string autosaveFolderFixedPart = (string) null;
    private static string cachedDefaultMilestoneID = (string) null;
    private TabPage currentTabPage;
    private static int numberOfLoansOpened = 0;
    private static Task<LoanReportFieldDefs> fieldDefinitionsTask;

    public static event SyncAllDisclosurePackageEventHandler RefreshAlertLogAsync;

    public EllieMae.EMLite.Common.ThinThick.IBrowser Browser
    {
      get
      {
        if (this.tabControl.SelectedTab == this.pipelineTabPage)
          return (EllieMae.EMLite.Common.ThinThick.IBrowser) this.PipelineBrowser;
        return this.tabControl.SelectedTab == this.tradesTabPage ? (EllieMae.EMLite.Common.ThinThick.IBrowser) this.TradingBrowser : (EllieMae.EMLite.Common.ThinThick.IBrowser) null;
      }
    }

    internal LoanPage OpenLoanPage => this.loanPage;

    internal EpassControl Epass => this.epassControl;

    internal ContactMainForm ContactPage => this.contactPage;

    internal HomePageControl HomePage => this.homeControl;

    internal DashboardForm DashboardPage => this.frmDashboard;

    internal TradeManagementConsole TradeConsole => this.tradingConsole;

    internal ReportMainControl ReportControl => this.reportControl;

    internal SetUpContainer SettingsPage => this.setupPanel;

    public static bool InitializeInvesterServicesSettings { get; set; }

    public static bool AllowDataAndDocs { get; set; }

    public static bool AllowWarehouseLenders { get; set; }

    public static bool AllowDueDiligenceServices { get; set; }

    public static bool AllowHedgeAdvisoryServices { get; set; }

    public static bool AllowSubservicingServices { get; set; }

    public static bool AllowBidTapServices { get; set; }

    public static bool AllowQCAuditServices { get; set; }

    public static bool AllowWholesaleLenderServices { get; set; }

    public static bool AllowServicingServices { get; set; }

    public static bool homePageLoaded { get; set; }

    public static List<PartnerResponseBody> DataAndDocsPartners { get; private set; }

    public static List<PartnerResponseBody> WarehouseLenders { get; private set; }

    public static List<PartnerResponseBody> DueDiligenceServices { get; private set; }

    public static List<PartnerResponseBody> HedgeAdvisoryServices { get; private set; }

    public static List<PartnerResponseBody> SubservicingServices { get; private set; }

    public static List<PartnerResponseBody> BidTapeServices { get; private set; }

    public static List<PartnerResponseBody> QCAuditServices { get; private set; }

    public static List<PartnerResponseBody> WholesaleLenderServices { get; private set; }

    public static List<PartnerResponseBody> ServicingServices { get; private set; }

    public static MainScreen Instance
    {
      get
      {
        if (MainScreen.instance == null)
        {
          using (Tracing.StartTimer(MainScreen.sw, nameof (MainScreen), TraceLevel.Info, "Instantiating MainScreen control..."))
            MainScreen.instance = new MainScreen();
          Session.MainScreen = (IMainScreen) MainScreen.instance;
        }
        return MainScreen.instance;
      }
    }

    private void loadPipelineScreen() => this.loadPipelineScreen(0);

    private void loadPipelineScreen(int sqlRead)
    {
      if (this.PipelineScreenBrowser == null && this.tabControl.TabPages.Contains(this.pipelineTabPage))
      {
        if (AccessUtils.CanAccessFeature(Feature.WebPipeline))
        {
          this.PipelineBrowser = new PipelineBrowser();
          this.PipelineScreenBrowser = (IPipeline) this.PipelineBrowser;
          this.PipelineScreenBrowserMenuProvider = (IMenuProvider) this.PipelineBrowser;
          this.PipelineBrowser.Dock = DockStyle.Fill;
          this.PipelineBrowser.Initialize(MainForm.Instance.PipelineMenuItems);
        }
        else
        {
          this.PipelineScreen = new PipelineScreen();
          this.PipelineScreenBrowser = (IPipeline) this.PipelineScreen;
          this.PipelineScreenBrowserMenuProvider = (IMenuProvider) this.PipelineScreen;
          this.PipelineScreen.Dock = DockStyle.Fill;
          this.PipelineScreen.Initialize(sqlRead);
        }
      }
      this.pipelineTabPage.Controls.Add((Control) this.PipelineScreenBrowser);
    }

    private void LoadTradingBrowser()
    {
      if (this.TradingScreenBrowser == null && this.tabControl.TabPages.Contains(this.tradesTabPage))
      {
        this.TradingBrowser = new TradingBrowser();
        this.TradingScreenBrowser = (ITradeConsole) this.TradingBrowser;
        this.TradingScreenBrowserMenuProvider = (IMenuProvider) this.TradingBrowser;
        this.TradingBrowser.Dock = DockStyle.Fill;
        this.TradingBrowser.Initialize();
      }
      MainForm.Instance.SetMenu("Trades");
      this.tradesTabPage.Controls.Add((Control) this.TradingBrowser);
    }

    private void loadReportScreen()
    {
      if (this.reportControl != null || !this.tabControl.TabPages.Contains(this.reportsTabPage))
        return;
      this.reportControl = new ReportMainControl(Session.DefaultInstance, false);
    }

    public bool HasOpenLoan => this.hasOpenLoan;

    public bool HasAccessToLoanTab => this.tabControl.TabPages.Contains(this.pipelineTabPage);

    static MainScreen()
    {
      if (Session.Connection.IsServerInProcess)
        a.a("mkl9m3X90nM45sY");
      else
        a.a("i9w9j72bidpm93x");
      EnableDisableSetting componentSetting = (EnableDisableSetting) Session.GetComponentSetting("IM", (object) EnableDisableSetting.Disabled);
      MainScreen.IMEnabled = !Session.Connection.IsServerInProcess && componentSetting == EnableDisableSetting.Enabled;
      MainScreen.CSEnabled = false;
      if (!Session.Connection.IsServerInProcess && EnableDisableSetting.Enabled == (EnableDisableSetting) Session.GetComponentSetting("CS", (object) EnableDisableSetting.Disabled))
        MainScreen.CSEnabled = true;
      if (MainScreen.jed == null)
        MainScreen.jed = !Session.Connection.IsServerInProcess ? a.b("km0w2khs9") : a.b("mk0T8jLZ0");
      if (!Session.StartupInfo.AllowDataAndDocs)
        return;
      MainScreen.initializeInvestorServicesTask = Task.Run<bool>((Func<bool>) (() => MainScreen.InitializeInvestorServiceFeatureSettings()));
    }

    private static bool InitializeInvestorServiceFeatureSettings()
    {
      try
      {
        List<PartnerResponseBody> investorsList = new DataDocsServiceHelper().GetInvestorsList();
        string[] array1 = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (provider => provider.Category.Equals(SelectInvestorsPage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToArray<string>();
        string[] array2 = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (provider => provider.Category.Equals(WarehouseLendersServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToArray<string>();
        string[] array3 = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (provider => provider.Category.Equals(DueDiligenceServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToArray<string>();
        string[] array4 = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (provider => provider.Category.Equals(HedgeAdvisoryServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToArray<string>();
        string[] array5 = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (provider => provider.Category.Equals(SubservicingServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToArray<string>();
        string[] array6 = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (provider => provider.Category.Equals(BidTapeServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToArray<string>();
        string[] array7 = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (provider => provider.Category.Equals(QCAuditServicesPage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToArray<string>();
        string[] array8 = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (provider => provider.Category.Equals(WholesaleLenderServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToArray<string>();
        string[] array9 = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (provider => provider.Category.Equals(ServicingServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToArray<string>();
        List<InvestorServiceAclInfo> investorServiceAclInfoList = new List<InvestorServiceAclInfo>();
        investorServiceAclInfoList.AddRange((IEnumerable<InvestorServiceAclInfo>) InvestorServiceAclInfo.GetInvestorServicesList(array1, 9007, SelectInvestorsPage.InvestorCategory));
        investorServiceAclInfoList.AddRange((IEnumerable<InvestorServiceAclInfo>) InvestorServiceAclInfo.GetInvestorServicesList(array2, 9009, WarehouseLendersServicePage.InvestorCategory));
        investorServiceAclInfoList.AddRange((IEnumerable<InvestorServiceAclInfo>) InvestorServiceAclInfo.GetInvestorServicesList(array3, 9010, DueDiligenceServicePage.InvestorCategory));
        investorServiceAclInfoList.AddRange((IEnumerable<InvestorServiceAclInfo>) InvestorServiceAclInfo.GetInvestorServicesList(array4, 9011, HedgeAdvisoryServicePage.InvestorCategory));
        investorServiceAclInfoList.AddRange((IEnumerable<InvestorServiceAclInfo>) InvestorServiceAclInfo.GetInvestorServicesList(array5, 9012, SubservicingServicePage.InvestorCategory));
        investorServiceAclInfoList.AddRange((IEnumerable<InvestorServiceAclInfo>) InvestorServiceAclInfo.GetInvestorServicesList(array6, 9013, BidTapeServicePage.InvestorCategory));
        investorServiceAclInfoList.AddRange((IEnumerable<InvestorServiceAclInfo>) InvestorServiceAclInfo.GetInvestorServicesList(array7, 9014, QCAuditServicesPage.InvestorCategory));
        investorServiceAclInfoList.AddRange((IEnumerable<InvestorServiceAclInfo>) InvestorServiceAclInfo.GetInvestorServicesList(array8, 9017, WholesaleLenderServicePage.InvestorCategory));
        investorServiceAclInfoList.AddRange((IEnumerable<InvestorServiceAclInfo>) InvestorServiceAclInfo.GetInvestorServicesList(array9, 9020, ServicingServicePage.InvestorCategory));
        InvestorServiceAclInfo[] permissions = ((InvestorServicesAclManager) Session.ACL.GetAclManager(AclCategory.InvestorServices)).GetPermissions(new AclFeature[9]
        {
          AclFeature.LoanMgmt_Investor_Service,
          AclFeature.LoanMgmt_Investor_Due_Diligence,
          AclFeature.LoanMgmt_Investor_Hedge_Advisory,
          AclFeature.LoanMgmt_Investor_Warehouse_Lenders,
          AclFeature.LoanMgmt_Investor_Subservicing_Services,
          AclFeature.LoanMgmt_Investor_Bid_Tape_Services,
          AclFeature.LoanMgmt_Investor_QC_Audit_Services,
          AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services,
          AclFeature.LoanMgmt_Investor_Servicing_Services
        }, Session.UserID, Session.UserInfo.UserPersonas, investorServiceAclInfoList.ToArray());
        Dictionary<string, string> investorServiceAssignedPartners = ((IEnumerable<InvestorServiceAclInfo>) permissions).Where<InvestorServiceAclInfo>((Func<InvestorServiceAclInfo, bool>) (s => s.FeatureID == 9007 && s.Access)).Select<InvestorServiceAclInfo, string>((Func<InvestorServiceAclInfo, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToDictionary<string, string>((Func<string, string>) (s => s));
        MainScreen.DataAndDocsPartners = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => investorServiceAssignedPartners.ContainsKey(s.ProviderCompanyCode))).ToList<PartnerResponseBody>();
        MainScreen.AllowDataAndDocs = MainScreen.DataAndDocsPartners != null && MainScreen.DataAndDocsPartners.Count != 0;
        Dictionary<string, string> dueDiligenceAssignedPartners = ((IEnumerable<InvestorServiceAclInfo>) permissions).Where<InvestorServiceAclInfo>((Func<InvestorServiceAclInfo, bool>) (s => s.FeatureID == 9010 && s.Access)).Select<InvestorServiceAclInfo, string>((Func<InvestorServiceAclInfo, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToDictionary<string, string>((Func<string, string>) (s => s));
        MainScreen.DueDiligenceServices = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => dueDiligenceAssignedPartners.ContainsKey(s.ProviderCompanyCode))).ToList<PartnerResponseBody>();
        MainScreen.AllowDueDiligenceServices = MainScreen.DueDiligenceServices != null && MainScreen.DueDiligenceServices.Count != 0;
        Dictionary<string, string> hedgeAdvisoryAssignedPartners = ((IEnumerable<InvestorServiceAclInfo>) permissions).Where<InvestorServiceAclInfo>((Func<InvestorServiceAclInfo, bool>) (s => s.FeatureID == 9011 && s.Access)).Select<InvestorServiceAclInfo, string>((Func<InvestorServiceAclInfo, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToDictionary<string, string>((Func<string, string>) (s => s));
        MainScreen.HedgeAdvisoryServices = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => hedgeAdvisoryAssignedPartners.ContainsKey(s.ProviderCompanyCode))).ToList<PartnerResponseBody>();
        MainScreen.AllowHedgeAdvisoryServices = MainScreen.HedgeAdvisoryServices != null && MainScreen.HedgeAdvisoryServices.Count != 0;
        Dictionary<string, string> warehouseLendersAssignedPartners = ((IEnumerable<InvestorServiceAclInfo>) permissions).Where<InvestorServiceAclInfo>((Func<InvestorServiceAclInfo, bool>) (s => s.FeatureID == 9009 && s.Access)).Select<InvestorServiceAclInfo, string>((Func<InvestorServiceAclInfo, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToDictionary<string, string>((Func<string, string>) (s => s));
        MainScreen.WarehouseLenders = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => warehouseLendersAssignedPartners.ContainsKey(s.ProviderCompanyCode))).ToList<PartnerResponseBody>();
        MainScreen.AllowWarehouseLenders = MainScreen.WarehouseLenders != null && MainScreen.WarehouseLenders.Count != 0;
        Dictionary<string, string> subservicingAssignedPartners = ((IEnumerable<InvestorServiceAclInfo>) permissions).Where<InvestorServiceAclInfo>((Func<InvestorServiceAclInfo, bool>) (s => s.FeatureID == 9012 && s.Access)).Select<InvestorServiceAclInfo, string>((Func<InvestorServiceAclInfo, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToDictionary<string, string>((Func<string, string>) (s => s));
        MainScreen.SubservicingServices = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => subservicingAssignedPartners.ContainsKey(s.ProviderCompanyCode))).ToList<PartnerResponseBody>();
        MainScreen.AllowSubservicingServices = MainScreen.SubservicingServices != null && MainScreen.SubservicingServices.Count != 0;
        Dictionary<string, string> bidTapeAssignedPartners = ((IEnumerable<InvestorServiceAclInfo>) permissions).Where<InvestorServiceAclInfo>((Func<InvestorServiceAclInfo, bool>) (s => s.FeatureID == 9013 && s.Access)).Select<InvestorServiceAclInfo, string>((Func<InvestorServiceAclInfo, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToDictionary<string, string>((Func<string, string>) (s => s));
        MainScreen.BidTapeServices = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => bidTapeAssignedPartners.ContainsKey(s.ProviderCompanyCode))).ToList<PartnerResponseBody>();
        MainScreen.AllowBidTapServices = MainScreen.BidTapeServices != null && MainScreen.BidTapeServices.Count != 0;
        Dictionary<string, string> qcAuditAssignedPartners = ((IEnumerable<InvestorServiceAclInfo>) permissions).Where<InvestorServiceAclInfo>((Func<InvestorServiceAclInfo, bool>) (s => s.FeatureID == 9014 && s.Access)).Select<InvestorServiceAclInfo, string>((Func<InvestorServiceAclInfo, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToDictionary<string, string>((Func<string, string>) (s => s));
        MainScreen.QCAuditServices = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => qcAuditAssignedPartners.ContainsKey(s.ProviderCompanyCode))).ToList<PartnerResponseBody>();
        MainScreen.AllowQCAuditServices = MainScreen.QCAuditServices != null && MainScreen.QCAuditServices.Count != 0;
        Dictionary<string, string> wholesaleLenderAssignedPartners = ((IEnumerable<InvestorServiceAclInfo>) permissions).Where<InvestorServiceAclInfo>((Func<InvestorServiceAclInfo, bool>) (s => s.FeatureID == 9017 && s.Access)).Select<InvestorServiceAclInfo, string>((Func<InvestorServiceAclInfo, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToDictionary<string, string>((Func<string, string>) (s => s));
        MainScreen.WholesaleLenderServices = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => wholesaleLenderAssignedPartners.ContainsKey(s.ProviderCompanyCode))).ToList<PartnerResponseBody>();
        MainScreen.AllowWholesaleLenderServices = MainScreen.WholesaleLenderServices != null && MainScreen.WholesaleLenderServices.Count != 0;
        Dictionary<string, string> servicingAssignedPartners = ((IEnumerable<InvestorServiceAclInfo>) permissions).Where<InvestorServiceAclInfo>((Func<InvestorServiceAclInfo, bool>) (s => s.FeatureID == 9020 && s.Access)).Select<InvestorServiceAclInfo, string>((Func<InvestorServiceAclInfo, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToDictionary<string, string>((Func<string, string>) (s => s));
        MainScreen.ServicingServices = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => servicingAssignedPartners.ContainsKey(s.ProviderCompanyCode))).ToList<PartnerResponseBody>();
        MainScreen.AllowServicingServices = MainScreen.ServicingServices != null && MainScreen.ServicingServices.Count != 0;
        return true;
      }
      catch
      {
        MainScreen.DataAndDocsPartners = new List<PartnerResponseBody>();
        MainScreen.HedgeAdvisoryServices = new List<PartnerResponseBody>();
        MainScreen.DueDiligenceServices = new List<PartnerResponseBody>();
        MainScreen.WarehouseLenders = new List<PartnerResponseBody>();
        MainScreen.SubservicingServices = new List<PartnerResponseBody>();
        MainScreen.BidTapeServices = new List<PartnerResponseBody>();
        MainScreen.QCAuditServices = new List<PartnerResponseBody>();
        MainScreen.WholesaleLenderServices = new List<PartnerResponseBody>();
        MainScreen.ServicingServices = new List<PartnerResponseBody>();
        return false;
      }
    }

    private static bool GetDataAndDocsSetting()
    {
      try
      {
        List<PartnerResponseBody> investorsList = new DataDocsServiceHelper().GetInvestorsList();
        List<InvestorServiceAclInfo> investorServicesList = InvestorServiceAclInfo.GetInvestorServicesList(investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (provider => provider.Category.Equals(SelectInvestorsPage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToArray<string>(), 9007, SelectInvestorsPage.InvestorCategory);
        Dictionary<string, string> assignedPartners = ((IEnumerable<InvestorServiceAclInfo>) ((InvestorServicesAclManager) Session.ACL.GetAclManager(AclCategory.InvestorServices)).GetPermissions(AclFeature.LoanMgmt_Investor_Service, Session.UserID, SelectInvestorsPage.InvestorCategory, Session.UserInfo.UserPersonas, investorServicesList.ToArray())).Where<InvestorServiceAclInfo>((Func<InvestorServiceAclInfo, bool>) (s => s.Access)).Select<InvestorServiceAclInfo, string>((Func<InvestorServiceAclInfo, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToDictionary<string, string>((Func<string, string>) (s => s));
        MainScreen.DataAndDocsPartners = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => assignedPartners.ContainsKey(s.ProviderCompanyCode))).ToList<PartnerResponseBody>();
        return MainScreen.DataAndDocsPartners != null && MainScreen.DataAndDocsPartners.Count != 0;
      }
      catch
      {
        MainScreen.DataAndDocsPartners = new List<PartnerResponseBody>();
        return false;
      }
    }

    private static bool GetWarehouseLenders()
    {
      try
      {
        List<PartnerResponseBody> investorsList = new DataDocsServiceHelper().GetInvestorsList();
        List<InvestorServiceAclInfo> investorServicesList = InvestorServiceAclInfo.GetInvestorServicesList(investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (provider => provider.Category.Equals(WarehouseLendersServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToArray<string>(), 9009, WarehouseLendersServicePage.InvestorCategory);
        Dictionary<string, string> assignedPartners = ((IEnumerable<InvestorServiceAclInfo>) ((InvestorServicesAclManager) Session.ACL.GetAclManager(AclCategory.InvestorServices)).GetPermissions(AclFeature.LoanMgmt_Investor_Warehouse_Lenders, Session.UserID, WarehouseLendersServicePage.InvestorCategory, Session.UserInfo.UserPersonas, investorServicesList.ToArray())).Where<InvestorServiceAclInfo>((Func<InvestorServiceAclInfo, bool>) (s => s.Access)).Select<InvestorServiceAclInfo, string>((Func<InvestorServiceAclInfo, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToDictionary<string, string>((Func<string, string>) (s => s));
        MainScreen.WarehouseLenders = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => assignedPartners.ContainsKey(s.ProviderCompanyCode))).ToList<PartnerResponseBody>();
        return MainScreen.WarehouseLenders != null && MainScreen.WarehouseLenders.Count != 0;
      }
      catch
      {
        MainScreen.WarehouseLenders = new List<PartnerResponseBody>();
        return false;
      }
    }

    private static bool GetDueDiligenceServices()
    {
      try
      {
        List<PartnerResponseBody> investorsList = new DataDocsServiceHelper().GetInvestorsList();
        List<InvestorServiceAclInfo> investorServicesList = InvestorServiceAclInfo.GetInvestorServicesList(investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (provider => provider.Category.Equals(DueDiligenceServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToArray<string>(), 9010, DueDiligenceServicePage.InvestorCategory);
        Dictionary<string, string> assignedPartners = ((IEnumerable<InvestorServiceAclInfo>) ((InvestorServicesAclManager) Session.ACL.GetAclManager(AclCategory.InvestorServices)).GetPermissions(AclFeature.LoanMgmt_Investor_Due_Diligence, Session.UserID, DueDiligenceServicePage.InvestorCategory, Session.UserInfo.UserPersonas, investorServicesList.ToArray())).Where<InvestorServiceAclInfo>((Func<InvestorServiceAclInfo, bool>) (s => s.Access)).Select<InvestorServiceAclInfo, string>((Func<InvestorServiceAclInfo, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToDictionary<string, string>((Func<string, string>) (s => s));
        MainScreen.DueDiligenceServices = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => assignedPartners.ContainsKey(s.ProviderCompanyCode))).ToList<PartnerResponseBody>();
        return MainScreen.DueDiligenceServices != null && MainScreen.DueDiligenceServices.Count != 0;
      }
      catch
      {
        MainScreen.DueDiligenceServices = new List<PartnerResponseBody>();
        return false;
      }
    }

    private static bool GetHedgeAdvisoryServices()
    {
      try
      {
        List<PartnerResponseBody> investorsList = new DataDocsServiceHelper().GetInvestorsList();
        List<InvestorServiceAclInfo> investorServicesList = InvestorServiceAclInfo.GetInvestorServicesList(investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (provider => provider.Category.Equals(HedgeAdvisoryServicePage.InvestorCategory, StringComparison.InvariantCultureIgnoreCase))).Select<PartnerResponseBody, string>((Func<PartnerResponseBody, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToArray<string>(), 9011, HedgeAdvisoryServicePage.InvestorCategory);
        Dictionary<string, string> assignedPartners = ((IEnumerable<InvestorServiceAclInfo>) ((InvestorServicesAclManager) Session.ACL.GetAclManager(AclCategory.InvestorServices)).GetPermissions(AclFeature.LoanMgmt_Investor_Hedge_Advisory, Session.UserID, HedgeAdvisoryServicePage.InvestorCategory, Session.UserInfo.UserPersonas, investorServicesList.ToArray())).Where<InvestorServiceAclInfo>((Func<InvestorServiceAclInfo, bool>) (s => s.Access)).Select<InvestorServiceAclInfo, string>((Func<InvestorServiceAclInfo, string>) (s => s.ProviderCompanyCode)).Distinct<string>().ToDictionary<string, string>((Func<string, string>) (s => s));
        MainScreen.HedgeAdvisoryServices = investorsList.Where<PartnerResponseBody>((Func<PartnerResponseBody, bool>) (s => assignedPartners.ContainsKey(s.ProviderCompanyCode))).ToList<PartnerResponseBody>();
        return MainScreen.HedgeAdvisoryServices != null && MainScreen.HedgeAdvisoryServices.Count != 0;
      }
      catch
      {
        MainScreen.HedgeAdvisoryServices = new List<PartnerResponseBody>();
        return false;
      }
    }

    private void processAutosaveSettingsChange(object sender, AutosaveSettingsChangeEventArgs e)
    {
      this.timerAutosave.Stop();
      this.timerAutosave.Interval = e.Interval * 1000;
      if (!e.Enabled)
        return;
      this.StartAutosaveTimer();
    }

    private MainScreen()
    {
      Tracing.Log(MainScreen.sw, nameof (MainScreen), TraceLevel.Info, "Creating MainScreen UI components...");
      this.InitializeComponent();
      PerformanceMeter.Current.AddCheckpoint("MainScreen - Creating MainScreen UI components", 563, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
      this.tabPages[0] = this.homeTabPage;
      this.tabPages[1] = this.pipelineTabPage;
      this.tabPages[2] = this.loanTabPage;
      this.tabPages[3] = this.tradesTabPage;
      this.tabPages[4] = this.contactTabPage;
      this.tabPages[5] = this.dashboardTabPage;
      this.tabPages[6] = this.reportsTabPage;
      this.tabPages[7] = this.ePASSTabPage;
      Tracing.Log(MainScreen.sw, nameof (MainScreen), TraceLevel.Info, "Hiding main tab control...");
      this.tabControl.Visible = false;
      Tracing.Log(MainScreen.sw, nameof (MainScreen), TraceLevel.Info, "Registering MainScreen app services...");
      JedHelp.ApplicationWindow = (IApplicationWindow) this;
      Session.Application = (IApplicationScreen) this;
      this.RegisterService((object) this, typeof (ILoanConsole));
      this.RegisterService((object) this, typeof (IEncompassApplication));
      if (MainForm.Instance.IsPipelineTabDefault())
        this.RegisterService((object) new LoanServiceManager(Session.DefaultInstance), typeof (ILoanServices));
      AutosaveConfigManager.OnAutosaveSettingsChanged += new AutosaveSettingsChangeEventHandler(this.processAutosaveSettingsChange);
      PerformanceMeter.Current.AddCheckpoint("MainScreen - Registering app services", 594, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
      using (Tracing.StartTimer(MainScreen.sw, nameof (MainScreen), TraceLevel.Info, "Creating ePASS control..."))
      {
        this.epassControl = new EpassControl();
        this.epassControl.BeforeNavigate += new WebBrowserNavigatingEventHandler(this.epassControl_BeforeNavigate);
      }
      PerformanceMeter.Current.AddCheckpoint("MainScreen - Creating ePASS control", 606, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
      using (Tracing.StartTimer(MainScreen.sw, nameof (MainScreen), TraceLevel.Info, "Creating HomePage control..."))
        this.homeControl = new HomePageControl();
      if (MainForm.Instance.IsPipelineTabDefault())
      {
        this.homeControl.Login();
        this.currentTabPage = this.pipelineTabPage;
      }
      else
        this.currentTabPage = this.homeTabPage;
      PerformanceMeter.Current.AddCheckpoint("MainScreen - Creating HomePage control", 621, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
      using (Tracing.StartTimer(MainScreen.sw, nameof (MainScreen), TraceLevel.Info, "Registering all Thin Thick operations..."))
        OperationLocationInitializer.RegisterAll();
      PerformanceMeter.Current.AddCheckpoint("MainScreen - Registering Thin Thick", 635, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
      Tracing.Log(MainScreen.sw, nameof (MainScreen), TraceLevel.Info, "Starting AutoSave timer...");
      this.timerAutosave.Interval = AutosaveConfigManager.GetInterval() * 1000;
      this.LoanDataModifiedHandler = new LoanDataModifiedEventHandler(this.onLoanDataModified);
      this.timerAutosave.Tick += new EventHandler(this.autosaveHandler);
      this.Load += new EventHandler(this.MainScreen_Load);
      PerformanceMeter.Current.AddCheckpoint("MainScreen - Start AutoSave timer", 644, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
      Tracing.Log(MainScreen.sw, nameof (MainScreen), TraceLevel.Info, "MainScreen constructor completed successfully.");
    }

    private void MainScreen_Load(object sender, EventArgs e)
    {
      if (MainForm.Instance.IsPipelineTabDefault())
        return;
      Task.Run((System.Action) (() => this.homeControl.GoHome()));
      MainScreen.homePageLoaded = true;
    }

    private void epassControl_BeforeNavigate(object sender, WebBrowserNavigatingEventArgs e)
    {
      if (!this.tabControl.TabPages.Contains(this.ePASSTabPage))
        this.tabControl.TabPages.Insert(this.tabControl.TabPages.IndexOf(this.loanTabPage) + 1, this.ePASSTabPage);
      if (this.tabControl.SelectedTab == this.ePASSTabPage)
        return;
      this.tabControl.SelectedTab = this.ePASSTabPage;
    }

    public void PerformAutoSave()
    {
      try
      {
        if (Session.LoanDataMgr == null)
          return;
        if (Session.LoanData != null && Session.LoanData.Dirty)
        {
          if (this.loanPage != null && this.loanPage.StdIconBtnSynchEnabled)
          {
            this.SaveLoan(true, true, false, false, false);
            this.loanPage.RefreshSynchButton();
          }
          this.performAutoSave(Session.LoanDataMgr);
        }
        if (Session.LoanDataMgr.LinkedLoan == null || Session.LoanDataMgr.LinkedLoan.LoanData == null || !Session.LoanDataMgr.LinkedLoan.LoanData.Dirty)
          return;
        this.performAutoSave(Session.LoanDataMgr.LinkedLoan);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, nameof (MainScreen), string.Format("Error during autosave for loan guid {0}, clientID {1}, UserID {2}.", (object) Session.LoanDataMgr.LoanData.GUID, (object) Session.LoanDataMgr.SessionObjects.CompanyInfo.ClientID, (object) Session.LoanDataMgr.SessionObjects.UserID), ex);
      }
    }

    private void performAutoSave(LoanDataMgr loanDataMgr)
    {
      if (loanDataMgr == null || loanDataMgr.IsNew())
        return;
      string autoRecoverFilePath = this.getAutoRecoverFilePath(loanDataMgr);
      if (autoRecoverFilePath != null && !Directory.Exists(autoRecoverFilePath))
        Directory.CreateDirectory(autoRecoverFilePath);
      string autoRecoverFileName = this.getAutoRecoverFileName(loanDataMgr);
      string recoverAttFileName = this.getAutoRecoverAttFileName(autoRecoverFileName);
      string recoverHistoryFileName = this.getAutoRecoverHistoryFileName(autoRecoverFileName);
      File.WriteAllText(this.getAutoRecoverTsFileName(autoRecoverFileName), Session.ServerTime.ToString("yyyy-MM-dd HH:mm:ss tt") + Environment.NewLine + (object) loanDataMgr.LoanData.LoanVersionNumber);
      string xml = loanDataMgr.LoanData.ToXml(loanDataMgr.LoanData.ContentAccess, true, false);
      MainScreen.jed.b();
      byte[] bytes1 = MainScreen.jed.b(xml);
      File.WriteAllBytes(autoRecoverFileName, bytes1);
      string autoSaveXml = loanDataMgr.FileAttachments.GetAutoSaveXml();
      MainScreen.jed.b();
      byte[] bytes2 = MainScreen.jed.b(autoSaveXml);
      File.WriteAllBytes(recoverAttFileName, bytes2);
      string pendingHistory = loanDataMgr.LoanHistory.GetPendingHistory();
      MainScreen.jed.b();
      byte[] bytes3 = MainScreen.jed.b(pendingHistory);
      File.WriteAllBytes(recoverHistoryFileName, bytes3);
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
        Tracing.Log(MainScreen.sw, nameof (MainScreen), TraceLevel.Error, "Error during autosave: " + (object) ex);
      }
    }

    public void RecoverAutoSavedFile()
    {
      this.recoverAutoSavedFile(Session.LoanDataMgr);
      if (Session.LoanDataMgr.LinkedLoan == null)
        return;
      this.recoverAutoSavedFile(Session.LoanDataMgr.LinkedLoan);
      Session.LoanDataMgr.LinkTo(Session.LoanDataMgr.LinkedLoan);
    }

    private void recoverAutoSavedFile(LoanDataMgr loanDataMgr)
    {
      if (loanDataMgr == null || loanDataMgr.IsNew())
        return;
      RemoteLogger.Write(TraceLevel.Info, string.Format("Autosaved files recovery begins for loan guid {0}, clientID {1}, UserID {2}", (object) Session.LoanDataMgr.LoanData.GUID, (object) Session.LoanDataMgr.SessionObjects.CompanyInfo.ClientID, (object) Session.LoanDataMgr.SessionObjects.UserID));
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
        string[] strArray = File.ReadAllText(recoverTsFileName).Split(new string[1]
        {
          Environment.NewLine
        }, StringSplitOptions.None);
        DateTime dateTime = Convert.ToDateTime(strArray[0]);
        int num = -1;
        bool flag = false;
        if (strArray.Length > 1 && !string.IsNullOrWhiteSpace(strArray[1]))
        {
          num = Convert.ToInt32(strArray[1]);
          flag = true;
        }
        if (flag)
        {
          RemoteLogger.Write(TraceLevel.Info, string.Format("Autosaved file timestamp {0} and loan last modified timestamp {1} for loan guid {2}, clientID {3}, UserID {4}, LoanFileSequenceNumber {5}", (object) dateTime, (object) loanDataMgr.LastModified.ToString(), (object) Session.LoanDataMgr.LoanData.GUID, (object) Session.LoanDataMgr.SessionObjects.CompanyInfo.ClientID, (object) Session.LoanDataMgr.SessionObjects.UserID, (object) num));
          if (!(loanDataMgr.LastModified > dateTime))
          {
            if (loanDataMgr.LoanData.LoanVersionNumber == num)
              goto label_14;
          }
          this.removeAutoSavedFiles(autoRecoverFileName, recoverAttFileName, recoverHistoryFileName, recoverTsFileName);
          RemoteLogger.Write(TraceLevel.Info, string.Format("Autosaved files deleted without recovery for the loan guid {0}, clientID {1}, UserID {2}, LVN {3}", (object) Session.LoanDataMgr.LoanData.GUID, (object) Session.LoanDataMgr.SessionObjects.CompanyInfo.ClientID, (object) Session.LoanDataMgr.SessionObjects.UserID, (object) num));
          return;
        }
        RemoteLogger.Write(TraceLevel.Info, string.Format("Autosaved file timestamp {0} and loan last modified timestamp {1} for loan guid {2}, clientID {3}, UserID {4}", (object) dateTime, (object) loanDataMgr.LastModified.ToString(), (object) Session.LoanDataMgr.LoanData.GUID, (object) Session.LoanDataMgr.SessionObjects.CompanyInfo.ClientID, (object) Session.LoanDataMgr.SessionObjects.UserID));
        if (loanDataMgr.LastModified > dateTime)
        {
          this.removeAutoSavedFiles(autoRecoverFileName, recoverAttFileName, recoverHistoryFileName, recoverTsFileName);
          RemoteLogger.Write(TraceLevel.Info, string.Format("Autosaved files deleted without recovery for the loan guid {0}, clientID {1}, UserID {2}", (object) Session.LoanDataMgr.LoanData.GUID, (object) Session.LoanDataMgr.SessionObjects.CompanyInfo.ClientID, (object) Session.LoanDataMgr.SessionObjects.UserID));
          return;
        }
      }
      catch
      {
      }
label_14:
      LoanData loanData;
      try
      {
        MainScreen.jed.b();
        string xmlData = (string) null;
        using (FileStream A_0 = File.OpenRead(autoRecoverFileName))
          xmlData = MainScreen.jed.a((Stream) A_0);
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
        MainScreen.jed.b();
        using (FileStream A_0 = File.OpenRead(recoverAttFileName))
          attXml = MainScreen.jed.a((Stream) A_0);
      }
      catch
      {
      }
      string hisXml = (string) null;
      try
      {
        MainScreen.jed.b();
        using (FileStream A_0 = File.OpenRead(recoverHistoryFileName))
          hisXml = MainScreen.jed.a((Stream) A_0);
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
        RemoteLogger.Write(TraceLevel.Info, string.Format("User agrees to load autosaved file for loan guid {0}, clientID {1}, UserID {2}", (object) Session.LoanDataMgr.LoanData.GUID, (object) Session.LoanDataMgr.SessionObjects.CompanyInfo.ClientID, (object) Session.LoanDataMgr.SessionObjects.UserID));
        using (Stream stream = loanData.ToStream(LoanContentAccess.FullAccess, true))
          loanDataMgr.RecoverAutoSavedLoan(stream, attXml, hisXml);
        loanDataMgr.LoanData.IsAutoSaveFlag = true;
        loanDataMgr.LoanData.AutoSaveDateTime = lastWriteTime;
        this.DisplayEditor();
      }
      else
      {
        RemoteLogger.Write(TraceLevel.Info, string.Format("User denies to load autosaved file for loan guid {0}, clientID {1}, UserID {2}", (object) Session.LoanDataMgr.LoanData.GUID, (object) Session.LoanDataMgr.SessionObjects.CompanyInfo.ClientID, (object) Session.LoanDataMgr.SessionObjects.UserID));
        this.removeAutoSavedFiles(autoRecoverFileName, recoverAttFileName, recoverHistoryFileName, recoverTsFileName);
      }
      RemoteLogger.Write(TraceLevel.Info, string.Format("Autosaved files recovery ends for loan guid {0}, clientID {1}, UserID {2}", (object) Session.LoanDataMgr.LoanData.GUID, (object) Session.LoanDataMgr.SessionObjects.CompanyInfo.ClientID, (object) Session.LoanDataMgr.SessionObjects.UserID));
    }

    public void StartAutosaveTimer()
    {
      if (Session.LoanDataMgr == null || Session.LoanData == null || !Session.LoanDataMgr.Writable)
        return;
      if (AutosaveConfigManager.IsAutosaveEnabled())
      {
        this.timerAutosave.Start();
        Session.LoanDataMgr.IsAutosaveEnabled = true;
      }
      else
        Session.LoanDataMgr.IsAutosaveEnabled = false;
    }

    public void StopAutosaveTimer()
    {
      if (AutosaveConfigManager.IsAutosaveEnabled())
        return;
      this.timerAutosave.Stop();
    }

    public void InitContents()
    {
      Tracing.Log(MainScreen.sw, TraceLevel.Info, nameof (MainScreen), "MainScreen.InitContents started...");
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.tabControl.TabPages.Clear();
      PerformanceMeter.Current.AddCheckpoint("MainScreen.Init - Clear", 1026, nameof (InitContents), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
      Tracing.Log(MainScreen.sw, TraceLevel.Info, nameof (MainScreen), "Adding tab pages to main tab control...");
      this.tabControl.TabPages.Add(this.homeTabPage);
      Tracing.Log(MainScreen.sw, TraceLevel.Info, nameof (MainScreen), "HomePage tab added successfully...");
      PerformanceMeter.Current.AddCheckpoint("MainScreen.Init - HomePage", 1033, nameof (InitContents), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
      if (aclManager.GetUserApplicationRight(AclFeature.GlobalTab_Pipeline))
      {
        this.tabControl.TabPages.Add(this.pipelineTabPage);
        Tracing.Log(MainScreen.sw, TraceLevel.Info, nameof (MainScreen), "Pipeline tab added successfully...");
        PerformanceMeter.Current.AddCheckpoint("MainScreen.Init - Pipeline", 1039, nameof (InitContents), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
      }
      if (Session.EncompassEdition == EncompassEdition.Banker && aclManager.GetUserApplicationRight(AclFeature.TradeTab_Trades))
      {
        this.tabControl.TabPages.Add(this.tradesTabPage);
        Tracing.Log(MainScreen.sw, TraceLevel.Info, nameof (MainScreen), "Trades tab added successfully...");
        PerformanceMeter.Current.AddCheckpoint("MainScreen.Init - Trades", 1048, nameof (InitContents), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
      }
      if (aclManager.GetUserApplicationRight(AclFeature.GlobalTab_Contacts))
      {
        using (Tracing.StartTimer(MainScreen.sw, nameof (MainScreen), TraceLevel.Info, "Constructing ContactMainForm for current user session"))
          this.contactPage = new ContactMainForm(Session.DefaultInstance, new ContactMainForm.ContactTabChanged(this.contactPage_OnContactTabChange));
        this.contactPage.TopLevel = false;
        this.contactPage.Visible = true;
        this.tabControl.TabPages.Add(this.contactTabPage);
        Tracing.Log(MainScreen.sw, TraceLevel.Info, nameof (MainScreen), "Contacts tab added successfully...");
        PerformanceMeter.Current.AddCheckpoint("MainScreen.Init - Contacts", 1063, nameof (InitContents), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
      }
      if (aclManager.GetUserApplicationRight(AclFeature.DashboardTab_Dashboard))
      {
        this.tabControl.TabPages.Add(this.dashboardTabPage);
        Tracing.Log(MainScreen.sw, TraceLevel.Info, nameof (MainScreen), "Dashboard tab added successfully...");
        PerformanceMeter.Current.AddCheckpoint("MainScreen.Init - Dashboard", 1072, nameof (InitContents), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
      }
      if (aclManager.GetUserApplicationRight(AclFeature.ReportTab_LoanReport) || aclManager.GetUserApplicationRight(AclFeature.ReportTab_BorrowerContactReport) || aclManager.GetUserApplicationRight(AclFeature.ReportTab_BusinessContactReport))
      {
        this.tabControl.TabPages.Add(this.reportsTabPage);
        Tracing.Log(MainScreen.sw, TraceLevel.Info, nameof (MainScreen), "Reports tab added successfully...");
        PerformanceMeter.Current.AddCheckpoint("MainScreen.Init - Reports", 1083, nameof (InitContents), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
      }
      if (MainForm.Instance.IsPipelineTabDefault())
      {
        Tracing.Log(MainScreen.sw, TraceLevel.Info, nameof (MainScreen), "Setting Pipeline as selected tab...");
        this.tabControl.SelectedTab = this.pipelineTabPage;
      }
      else
      {
        Tracing.Log(MainScreen.sw, TraceLevel.Info, nameof (MainScreen), "Setting HomePage as selected tab...");
        this.tabControl.SelectedTab = this.homeTabPage;
      }
      this.tabControl.Visible = true;
      Tracing.Log(MainScreen.sw, TraceLevel.Info, nameof (MainScreen), "Activating tab page...");
      this.activateCurrentTabPage(1);
      MainForm.Instance.SetBackgroundUploadIcon();
      MainForm.Instance.SetTradeLoanUpdateIcon();
      LoanDataMgr.ReplaceTemplateExisting += new EventHandler(this.applyTemplateToExisting);
      PerformanceMeter.Current.AddCheckpoint("MainScreen.Init - Display Homepage", 1111, nameof (InitContents), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
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
      this.tabControl = new TabControl();
      this.homeTabPage = new TabPage();
      this.loanTabPage = new TabPage();
      this.ePASSTabPage = new TabPage();
      this.tradesTabPage = new TabPage();
      this.contactTabPage = new TabPage();
      this.dashboardTabPage = new TabPage();
      this.reportsTabPage = new TabPage();
      this.pipelineTabPage = new TabPage();
      this.timerAutosave = new System.Windows.Forms.Timer(this.components);
      this.pnlMain = new Panel();
      this.tabControl.SuspendLayout();
      this.pnlMain.SuspendLayout();
      this.SuspendLayout();
      this.tabControl.Controls.Add((Control) this.homeTabPage);
      this.tabControl.Controls.Add((Control) this.loanTabPage);
      this.tabControl.Controls.Add((Control) this.ePASSTabPage);
      this.tabControl.Controls.Add((Control) this.tradesTabPage);
      this.tabControl.Controls.Add((Control) this.contactTabPage);
      this.tabControl.Controls.Add((Control) this.dashboardTabPage);
      this.tabControl.Controls.Add((Control) this.reportsTabPage);
      this.tabControl.Controls.Add((Control) this.pipelineTabPage);
      this.tabControl.Dock = DockStyle.Fill;
      this.tabControl.HotTrack = true;
      this.tabControl.ItemSize = new Size(42, 28);
      this.tabControl.Location = new Point(2, 2);
      this.tabControl.Name = "tabControl";
      this.tabControl.Padding = new Point(11, 3);
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new Size(770, 480);
      this.tabControl.TabIndex = 1;
      this.tabControl.SelectedIndexChanged += new EventHandler(this.tabControl_SelectedIndexChanged);
      this.homeTabPage.Location = new Point(4, 32);
      this.homeTabPage.Name = "homeTabPage";
      this.homeTabPage.Padding = new Padding(0, 2, 2, 2);
      this.homeTabPage.Size = new Size(762, 444);
      this.homeTabPage.TabIndex = 7;
      this.homeTabPage.Tag = (object) "Home";
      this.homeTabPage.Text = "Home";
      this.homeTabPage.UseVisualStyleBackColor = true;
      this.loanTabPage.Location = new Point(4, 32);
      this.loanTabPage.Name = "loanTabPage";
      this.loanTabPage.Padding = new Padding(0, 2, 2, 2);
      this.loanTabPage.Size = new Size(762, 444);
      this.loanTabPage.TabIndex = 3;
      this.loanTabPage.Tag = (object) "Loans";
      this.loanTabPage.Text = "Loan";
      this.loanTabPage.UseVisualStyleBackColor = true;
      this.ePASSTabPage.Location = new Point(4, 32);
      this.ePASSTabPage.Name = "ePASSTabPage";
      this.ePASSTabPage.Padding = new Padding(0, 2, 2, 2);
      this.ePASSTabPage.Size = new Size(762, 444);
      this.ePASSTabPage.TabIndex = 4;
      this.ePASSTabPage.Tag = (object) "ePASS";
      this.ePASSTabPage.Text = "Services View";
      this.ePASSTabPage.UseVisualStyleBackColor = true;
      this.tradesTabPage.Location = new Point(4, 32);
      this.tradesTabPage.Name = "tradesTabPage";
      this.tradesTabPage.Padding = new Padding(0, 2, 2, 2);
      this.tradesTabPage.Size = new Size(762, 444);
      this.tradesTabPage.TabIndex = 12;
      this.tradesTabPage.Tag = (object) "Trades";
      this.tradesTabPage.Text = "Trades";
      this.tradesTabPage.UseVisualStyleBackColor = true;
      this.contactTabPage.Location = new Point(4, 32);
      this.contactTabPage.Name = "contactTabPage";
      this.contactTabPage.Padding = new Padding(0, 2, 2, 2);
      this.contactTabPage.Size = new Size(762, 444);
      this.contactTabPage.TabIndex = 6;
      this.contactTabPage.Tag = (object) "Contacts";
      this.contactTabPage.Text = "Contacts";
      this.contactTabPage.UseVisualStyleBackColor = true;
      this.dashboardTabPage.Location = new Point(4, 32);
      this.dashboardTabPage.Name = "dashboardTabPage";
      this.dashboardTabPage.Padding = new Padding(0, 2, 2, 2);
      this.dashboardTabPage.Size = new Size(762, 444);
      this.dashboardTabPage.TabIndex = 8;
      this.dashboardTabPage.Tag = (object) "Dashboard";
      this.dashboardTabPage.Text = "Dashboard";
      this.dashboardTabPage.UseVisualStyleBackColor = true;
      this.reportsTabPage.Location = new Point(4, 32);
      this.reportsTabPage.Name = "reportsTabPage";
      this.reportsTabPage.Padding = new Padding(0, 2, 2, 2);
      this.reportsTabPage.Size = new Size(762, 444);
      this.reportsTabPage.TabIndex = 9;
      this.reportsTabPage.Tag = (object) "Reports";
      this.reportsTabPage.Text = "Reports";
      this.reportsTabPage.UseVisualStyleBackColor = true;
      this.pipelineTabPage.Location = new Point(4, 32);
      this.pipelineTabPage.Name = "pipelineTabPage";
      this.pipelineTabPage.Padding = new Padding(0, 2, 2, 2);
      this.pipelineTabPage.Size = new Size(762, 444);
      this.pipelineTabPage.TabIndex = 2;
      this.pipelineTabPage.Tag = (object) "Pipeline";
      this.pipelineTabPage.Text = "Pipeline";
      this.pipelineTabPage.UseVisualStyleBackColor = true;
      this.pnlMain.BackColor = System.Drawing.Color.WhiteSmoke;
      this.pnlMain.Controls.Add((Control) this.tabControl);
      this.pnlMain.Dock = DockStyle.Fill;
      this.pnlMain.Location = new Point(0, 0);
      this.pnlMain.Name = "pnlMain";
      this.pnlMain.Padding = new Padding(2, 2, 0, 0);
      this.pnlMain.Size = new Size(772, 482);
      this.pnlMain.TabIndex = 15;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(772, 482);
      this.Controls.Add((Control) this.pnlMain);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MainScreen);
      this.ShowInTaskbar = false;
      this.tabControl.ResumeLayout(false);
      this.pnlMain.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public string CurrentScreen
    {
      get
      {
        return this.tabControl.SelectedTab.Text == "Contacts" ? this.contactPage.GetCurrentScreen() : this.tabControl.SelectedTab.Text;
      }
      set
      {
        foreach (TabPage tabPage in this.tabControl.TabPages)
        {
          if (tabPage.Text == value)
          {
            this.tabControl.SelectedTab = tabPage;
            return;
          }
        }
        if (!this.contactPage.SetCurrentScreen(value))
          throw new ArgumentException("The specified screen is not currently available");
        this.tabControl.SelectedTab = this.contactTabPage;
        this.contactPage.SetCurrentScreen(value);
      }
    }

    private void initializeLoanEditor(bool newLoan)
    {
      if (this.loanPage == null)
      {
        this.loanPage = new LoanPage(Session.DefaultInstance);
        this.loanPage.Parent = (Control) this.loanTabPage;
        this.loanTabPage.Controls.Add((Control) this.loanPage);
      }
      this.tabControl.SelectedIndexChanged -= new EventHandler(this.tabControl_SelectedIndexChanged);
      this.tabControl.Hide();
      int num = this.tabControl.TabPages.IndexOf(this.pipelineTabPage);
      if (num >= 0)
        this.tabControl.TabPages.Insert(num + 1, this.loanTabPage);
      else
        this.tabControl.TabPages.Insert(1, this.loanTabPage);
      this.tabControl.Show();
      this.loanPage.InitContents(newLoan, !Session.LoanDataMgr.Writable);
      this.tabControl.SelectedIndexChanged += new EventHandler(this.tabControl_SelectedIndexChanged);
      if (Session.LoanDataMgr == null)
        return;
      Session.LoanDataMgr.ExecuteEmailTriggers -= new EventHandler(this.loanDataMgr_ExecuteEmailTriggers);
      Session.LoanDataMgr.ExecuteEmailTriggers += new EventHandler(this.loanDataMgr_ExecuteEmailTriggers);
    }

    private void loanDataMgr_ExecuteEmailTriggers(object sender, EventArgs e)
    {
      this.executeDelayedTriggers((LoanDataMgr) sender);
    }

    public void ShowLeadCenter() => this.tabControl.SelectedTab = this.contactTabPage;

    public void ShowCalendar(
      IWin32Window owner,
      string userID,
      CSMessage.AccessLevel accessLevel,
      bool accessUpdate)
    {
      if (((Control) owner).InvokeRequired)
      {
        this.Invoke((Delegate) new MainScreen.ShowCalendarDelegate(this.ShowCalendar), (object) owner, (object) userID, (object) accessLevel, (object) accessUpdate);
      }
      else
      {
        if (accessUpdate && (this.contactPage == null || !this.contactPage.IsCurrentCalendarOwner(userID)))
          return;
        this.tabControl.SelectedTab = this.contactTabPage;
        this.contactPage.ShowCalendar(owner, userID, accessLevel, accessUpdate);
      }
    }

    public void RefreshContents()
    {
      if (this.tabControl.SelectedTab != this.loanTabPage)
        return;
      this.loanPage.RefreshContents();
    }

    public void RefreshLoanContents()
    {
      if (this.tabControl.SelectedTab != this.loanTabPage)
        return;
      this.loanPage.RefreshLoanContents();
    }

    public void DisableScreen() => MainForm.Instance.Enabled = false;

    public void EnableScreen() => MainForm.Instance.Enabled = true;

    public TipControl GetTipControl() => MainForm.Instance.Tips;

    public BalloonToolTip GetCEBalloonToolTip() => MainForm.Instance.CEBalloonToolTip;

    public BalloonToolTip GetCEGetUpdatesBalloonToolTip()
    {
      return MainForm.Instance.CEGetUpdatesBalloonToolTip;
    }

    internal void CloseCEGetUpdatesBalloonToolTip()
    {
      MainForm.Instance.CloseCEGetUpdatesBalloonToolTip();
    }

    public void HandleCEMessage(CEMessage message)
    {
      if (this.loanPage == null)
        return;
      if (message.MessageType == CEMessageType.LoanFileSaved)
        this.loanPage.RefreshSynchButton();
      else
        this.loanPage.HandleOtherUserOpenCloseLoan(message.Text);
    }

    public void RefreshCE()
    {
      if (this.loanPage == null)
        return;
      this.loanPage.RefreshCE();
    }

    private string getAutoRecoverFilePath(LoanDataMgr loanDataMgr)
    {
      if (loanDataMgr == null || loanDataMgr.LoanFolder == null)
        return (string) null;
      if (MainScreen.autosaveFolderFixedPart == null)
      {
        RegistryKey registryKey = (RegistryKey) null;
        try
        {
          registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass");
          if (registryKey != null)
            MainScreen.autosaveFolderFixedPart = (string) registryKey.GetValue("AutosaveFolderRoot");
        }
        catch
        {
        }
        finally
        {
          registryKey?.Close();
        }
        if (MainScreen.autosaveFolderFixedPart != null)
          MainScreen.autosaveFolderFixedPart = MainScreen.autosaveFolderFixedPart.Trim();
        if ((MainScreen.autosaveFolderFixedPart ?? "") == "")
          MainScreen.autosaveFolderFixedPart = EnConfigurationSettings.GlobalSettings.AppLoanAutosaveDirectory;
        string str = "(local)";
        if (!Session.Connection.IsServerInProcess)
          str = Session.CompanyInfo.ClientID;
        MainScreen.autosaveFolderFixedPart = Path.Combine(MainScreen.autosaveFolderFixedPart, str + "\\" + Session.UserID);
      }
      string path = Path.Combine(MainScreen.autosaveFolderFixedPart, loanDataMgr.LoanFolder);
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      return path;
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

    private string getAutoRecoverAttFileName(string autoRecoverFileName)
    {
      return Path.Combine(Path.GetDirectoryName(autoRecoverFileName), Path.GetFileNameWithoutExtension(autoRecoverFileName) + ".att");
    }

    private string getAutoRecoverHistoryFileName(string autoRecoverFileName)
    {
      return Path.Combine(Path.GetDirectoryName(autoRecoverFileName), Path.GetFileNameWithoutExtension(autoRecoverFileName) + ".his");
    }

    private string getAutoRecoverTsFileName(string autoRecoverFileName)
    {
      return Path.Combine(Path.GetDirectoryName(autoRecoverFileName), Path.GetFileNameWithoutExtension(autoRecoverFileName) + ".ts");
    }

    public void PreSave(
      out string autosaveFile,
      out string autosaveAttFile,
      out string autosaveHisFile,
      out string autosaveTsFile)
    {
      if (Session.LoanDataMgr == null)
      {
        autosaveFile = autosaveAttFile = autosaveHisFile = autosaveTsFile = (string) null;
      }
      else
      {
        autosaveFile = this.getAutoRecoverFileName(Session.LoanDataMgr);
        autosaveAttFile = this.getAutoRecoverAttFileName(autosaveFile);
        autosaveHisFile = this.getAutoRecoverHistoryFileName(autosaveFile);
        autosaveTsFile = this.getAutoRecoverTsFileName(autosaveFile);
      }
    }

    public void PostMerge(bool saveLoan)
    {
      if (Session.LoanDataMgr == null)
        return;
      if (!this.checkMilestoneDates())
      {
        int num = (int) new ChangeMilestoneDates(Session.DefaultInstance, true).ShowDialog((IWin32Window) this);
      }
      if (!saveLoan)
        return;
      if (MainScreen.cachedDefaultMilestoneID == null)
        MainScreen.cachedDefaultMilestoneID = Session.SessionObjects.BpmManager.GetMilestoneTemplateDefaultSettings()[(object) "POLICIES.CONTACTUPDATEMILESTONE"].ToString();
      if (!ContactUtil.IsContactUpdateRequired(Session.LoanDataMgr.LoanData, Session.LoanDataMgr.SystemConfiguration, MainScreen.cachedDefaultMilestoneID))
        return;
      this.updateLoanContacts();
    }

    private bool checkMilestoneDates()
    {
      DateTime dateTime = DateTime.MinValue;
      foreach (MilestoneLog allMilestone in Session.LoanData.GetLogList().GetAllMilestones())
      {
        if (allMilestone.Date != DateTime.MinValue)
        {
          if (dateTime != DateTime.MinValue)
          {
            if (allMilestone.Done)
            {
              if (dateTime > allMilestone.Date)
                return false;
            }
            else if (dateTime.Date > allMilestone.Date.Date)
              return false;
          }
          else if (allMilestone.MilestoneID != "1")
            return false;
        }
        else if (dateTime != DateTime.MinValue)
          return false;
        dateTime = allMilestone.Date;
      }
      return true;
    }

    public bool SaveLoan(
      bool mergeOnly,
      bool refreshScreen,
      bool enableRateLockValidation = false,
      bool enableBackupLoanFile = false,
      bool interactive = true)
    {
      using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("Loan.Save", "Saving a loan file to the Encompass Server", true, 1649, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs"))
      {
        if (Session.LoanData != null)
          performanceMeter.AddCheckpoint("Loan GUID: " + Session.LoanData.GUID, 1652, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
        ILoanMilestoneTemplateOrchestrator milestoneTemplateController = (ILoanMilestoneTemplateOrchestrator) new StandardMilestoneTemplateApply(Session.DefaultInstance, false, true, ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.LoanTab_DisplayMilestoneChangeScreen));
        MetricsFactory.IncrementCounter("LoanSaveIncCounter", (SFxTag) new SFxInternalTag());
        using (MetricsFactory.GetIncrementalTimer("LoanSaveIncTimer", (SFxTag) new SFxInternalTag()))
        {
          try
          {
            bool flag = Session.LoanDataMgr.SaveLoan(mergeOnly, interactive, false, milestoneTemplateController, false, out bool _, enableRateLockValidation, enableBackupLoanFile);
            if (flag && this.loanPage.MilestoneTemplateEmailTemplate != null)
            {
              this.sendEmailTriggerTemplate(this.loanPage.MilestoneTemplateEmailTemplate, true);
              this.loanPage.MilestoneTemplateEmailTemplate = (TriggerEmailTemplate) null;
            }
            if (refreshScreen)
              this.RefreshContents();
            performanceMeter.Stop();
            MetricsFactory.RecordIncrementalTimerSample("EMA_Loan_Save", Convert.ToInt64(performanceMeter.Duration.TotalMilliseconds), (SFxTag) new SFxUiTag());
            return flag;
          }
          catch (LoanVersionNumberMismatchException ex)
          {
            Tracing.Log(MainScreen.sw, nameof (MainScreen), TraceLevel.Error, ex.ToString());
            int num = (int) Utils.Dialog((IWin32Window) this, "The loan can not be saved as a more recent version may already be available on the server.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
        }
      }
    }

    private void applyTemplateToExisting(object selectedTemplate, EventArgs e)
    {
      KeyValuePair<MilestoneTemplate, string> keyValuePair = (KeyValuePair<MilestoneTemplate, string>) selectedTemplate;
      if (keyValuePair.Key == null || Session.LoanData.GetLogList().GetMilestoneByID("7").Done || !(keyValuePair.Key.Name != Session.LoanDataMgr.LoanData.GetLogList().MilestoneTemplate.Name))
        return;
      bool? nullable = new MilestoneTemplatesManager().ApplyMilestoneTemplate(Session.SessionObjects, Session.LoanData, (ILoanMilestoneTemplateOrchestrator) new StandardMilestoneTemplateApply(Session.DefaultInstance, true, false, ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.LoanTab_DisplayMilestoneChangeScreen)), keyValuePair.Key, keyValuePair.Value);
      bool flag = false;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      int num = (int) MessageBox.Show((IWin32Window) this, "The loan template has been applied successfully. You chose not to apply the milestone template associated with the loan template, therefore the loan's milestones were not affected.", "Loan Template Applied", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private bool checkMilestoneTemplate(bool saveLoan)
    {
      switch ((MilestoneTemplatesSetting) Session.StartupInfo.PolicySettings[(object) "Policies.MilestoneTemplateSettings"])
      {
        case MilestoneTemplatesSetting.Manual:
        case MilestoneTemplatesSetting.None:
          return true;
        default:
          if (!Session.LoanData.GetLogList().MSLock && !Session.LoanData.GetLogList().GetMilestoneByID("7").Done && LoanDataMgr.GetBestMatchingMilestoneTemplate(Session.SessionObjects, Session.LoanData).Name != Session.LoanDataMgr.LoanData.GetLogList().MilestoneTemplate.Name)
          {
            ApplyMilestoneTemplates milestoneTemplates = new ApplyMilestoneTemplates(LoanDataMgr.GetLoanConditionsForMilestoneTemplate(Session.LoanData), Session.LoanDataMgr.LoanData, saveLoan);
            if (milestoneTemplates.DialogResult != DialogResult.OK)
              return false;
            this.loanPage.MilestoneTemplateEmailTemplate = milestoneTemplates.EmailTemplate;
            return true;
          }
          goto case MilestoneTemplatesSetting.Manual;
      }
    }

    private void getLoanMilestoneStatus(
      ref MilestoneLog msCheck,
      ref MilestoneLog msToBeFinished,
      LogList logList)
    {
      MilestoneLog[] allMilestones = logList.GetAllMilestones();
      for (int index = 0; index < allMilestones.Length && allMilestones[index].Done; ++index)
      {
        if (index == allMilestones.Length - 1)
        {
          msCheck = allMilestones[index];
          msToBeFinished = msCheck;
        }
        else if (allMilestones[index + 1].RoleID < RoleInfo.FileStarter.ID || allMilestones[index + 1].LoanAssociateID != "")
        {
          msCheck = allMilestones[index];
          msToBeFinished = allMilestones[index + 1];
        }
      }
      msToBeFinished = logList.GetCurrentMilestone();
      if (msToBeFinished != null)
        return;
      msToBeFinished = msCheck;
    }

    public bool SaveLoanNoCalcAll(
      bool mergeOnly,
      bool enableRateLockValidation = false,
      bool enableBackupLoanFile = false)
    {
      Session.LoanDataMgr.LoanData.Calculator.IsCalcAllRequired = false;
      bool flag = this.SaveLoan(mergeOnly, true, enableRateLockValidation, enableBackupLoanFile);
      Session.LoanDataMgr.LoanData.Calculator.IsCalcAllRequired = true;
      this.saveButtonIsClicked = true;
      return flag;
    }

    public bool SaveLoan(bool enableRateLockValidation = false, bool enableBackupLoanFile = false)
    {
      return this.SaveLoan(false, enableRateLockValidation, enableBackupLoanFile);
    }

    public bool SaveLoan(bool mergeOnly, bool enableRateLockValidation = false, bool enableBackupLoanFile = false)
    {
      return this.SaveLoan(mergeOnly, true, enableRateLockValidation, enableBackupLoanFile);
    }

    public void PostSave(
      LoanDataMgr loanDataMgr,
      string autosaveFile,
      string autosaveAttFile,
      string autosaveHisFile,
      string autoSaveTsFile)
    {
      if (loanDataMgr != null && loanDataMgr.LoanData != null && Session.LoanDataMgr != null && Session.LoanDataMgr.LoanData != null && loanDataMgr.LoanData.GUID == Session.LoanDataMgr.LoanData.GUID)
        this.resetAutosaveFiles(autosaveFile, autosaveAttFile, autosaveHisFile, autoSaveTsFile);
      if (loanDataMgr != null)
      {
        ImportConditionFactory.CallResourceImporterAPI(loanDataMgr.LoanData);
        MainForm.Instance.SetLastSaveTime(loanDataMgr.LastModified, false);
        if (TPONotificationQueue.IsTPOWebCenterLoan(loanDataMgr.LoanData))
          TPONotificationQueue.ClearMessages(loanDataMgr.LoanData.GUID, true);
      }
      this.executeDelayedTriggers(loanDataMgr);
      Session.Application.GetService<IPipeline>()?.InvalidatePipeline();
    }

    private void executeDelayedTriggers(LoanDataMgr loanDataMgr)
    {
      bool flag1 = false;
      foreach (DelayedTrigger activatedTrigger in loanDataMgr.Triggers.GetDelayActivatedTriggers())
      {
        if (!activatedTrigger.SupportsDirectExecution)
        {
          if (activatedTrigger.IsActivated())
          {
            bool flag2 = false;
            if (activatedTrigger is DelayedCompiledTrigger)
            {
              DelayedCompiledTrigger delayedCompiledTrigger = (DelayedCompiledTrigger) activatedTrigger;
              if (delayedCompiledTrigger.TriggerEvent.Action.ActionType == TriggerActionType.Email)
                flag2 = this.sendEmailTrigger(delayedCompiledTrigger.TriggerEvent.Action as TriggerEmailAction);
              try
              {
                if (delayedCompiledTrigger.TriggerEvent.Action.ActionType == TriggerActionType.LoanMove)
                {
                  string loanFolderName = ((TriggerMoveLoanFolderAction) delayedCompiledTrigger.TriggerEvent.Action).LoanFolderName;
                  if (loanFolderName.ToLower() != loanDataMgr.LoanFolder.ToLower())
                  {
                    loanDataMgr.Move(loanFolderName, loanDataMgr.LoanName, DuplicateLoanAction.Rename);
                    flag2 = true;
                  }
                  else
                    flag2 = false;
                }
              }
              catch (Exception ex)
              {
                Tracing.Log(MainScreen.sw, nameof (MainScreen), TraceLevel.Error, "Failed to process trigger move loan action: " + (object) ex);
                flag2 = false;
              }
            }
            if (flag2)
            {
              activatedTrigger.Reset();
              flag1 = true;
            }
          }
          else
            activatedTrigger.Reset();
        }
      }
      if (!flag1)
        return;
      this.loanPage.RefreshLogPanel();
    }

    private bool sendEmailTrigger(TriggerEmailAction action)
    {
      try
      {
        bool flag = false;
        foreach (TriggerEmailTemplate template in action.Templates)
          flag |= this.sendEmailTriggerTemplate(template, false);
        return flag;
      }
      catch (Exception ex)
      {
        Tracing.Log(MainScreen.sw, nameof (MainScreen), TraceLevel.Error, "Failed to process trigger email action: " + (object) ex);
        return false;
      }
    }

    private bool sendEmailTriggerTemplate(TriggerEmailTemplate template, bool isUserSpecific)
    {
      MailMessage message1 = new MailMessage();
      try
      {
        message1.From = new MailAddress(Session.UserInfo.Email, Session.UserInfo.FullName);
        message1.Subject = FieldReplacementRegex.ReplaceLiteral(template.Subject, Session.LoanData);
        message1.Body = FieldReplacementRegex.ReplaceLiteral(template.Body, Session.LoanData);
      }
      catch (Exception ex)
      {
        Tracing.Log(MainScreen.sw, nameof (MainScreen), TraceLevel.Info, "Error initializing MailMessage object for '" + template.Subject + "' -- sender address is invalid or subject/body are corrupt. (" + ex.Message + ")");
        return false;
      }
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) template.RecipientUsers);
      foreach (int recipientRole in template.RecipientRoles)
      {
        foreach (string loanAssociateUser in Session.LoanDataMgr.GetLoanAssociateUsers(recipientRole))
        {
          if (!stringList.Contains(loanAssociateUser))
            stringList.Add(loanAssociateUser);
        }
      }
      if (stringList.Count == 0)
      {
        Tracing.Log(MainScreen.sw, nameof (MainScreen), TraceLevel.Info, "Cannot send trigger email '" + template.Subject + "' -- no recipients found");
        return false;
      }
      bool flag = false;
      Hashtable users = Session.OrganizationManager.GetUsers(stringList.ToArray());
      List<MailMessage> mailMessageList = new List<MailMessage>();
      foreach (UserInfo userInfo in (IEnumerable) users.Values)
      {
        if (userInfo != (UserInfo) null)
        {
          if ((userInfo.Email ?? "") == "")
          {
            Tracing.Log(MainScreen.sw, nameof (MainScreen), TraceLevel.Warning, "Cannot send trigger email '" + template.Subject + "' to user '" + userInfo.Userid + "' -- no email address available.");
            stringList.Remove(userInfo.Userid);
          }
          else
          {
            if (isUserSpecific)
            {
              MailMessage mailMessage = new MailMessage();
              mailMessage.From = message1.From;
              mailMessage.Subject = message1.Subject;
              mailMessage.Body = message1.Body.Replace("<user>", userInfo.FirstName);
              mailMessage.To.Add(new MailAddress(userInfo.Email, userInfo.FullName));
              mailMessageList.Add(mailMessage);
            }
            else
              message1.To.Add(new MailAddress(userInfo.Email, userInfo.FullName));
            flag = true;
          }
        }
      }
      if (!flag)
      {
        Tracing.Log(MainScreen.sw, nameof (MainScreen), TraceLevel.Info, "Cannot send trigger email '" + template.Subject + "' -- no valid recipients found");
        return false;
      }
      try
      {
        if (isUserSpecific)
        {
          foreach (MailMessage message2 in mailMessageList)
            ContactUtils.SendMail(message2);
        }
        else
          ContactUtils.SendMail(message1);
        string body = stringList.Count <= 1 ? message1.Body.Replace("<user>", "Encompass User") : message1.Body.Replace("<user>", "Encompass User(s)");
        Session.LoanDataMgr.AddOperationLog((LogRecordBase) new EmailTriggerLog(message1.Subject, body, Session.UserID, stringList.ToArray(), template.DisplayInLog));
        Tracing.Log(MainScreen.sw, nameof (MainScreen), TraceLevel.Info, "Successfully sent trigger email '" + template.Subject + "' to recipients: " + string.Join(", ", stringList.ToArray()));
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(MainScreen.sw, nameof (MainScreen), TraceLevel.Error, "Failed to send trigger email '" + template.Subject + "' to recipients " + string.Join(", ", stringList.ToArray()) + ": " + (object) ex);
        return false;
      }
    }

    private void updateLoanContacts()
    {
      BorrowerPair currentBorrowerPair = Session.LoanData.CurrentBorrowerPair;
      BorrowerPair borrowerPair = Session.LoanData.GetBorrowerPairs()[0];
      Session.LoanData.SetBorrowerPair(borrowerPair);
      bool applicationRight = ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.Cnt_Contacts_Update);
      CRMLog crmMapping1 = Session.LoanData.GetLogList().GetCRMMapping(borrowerPair.Borrower.Id);
      bool flag1 = false;
      if (!(borrowerPair.Borrower.FirstName.Trim() == "") || !(borrowerPair.Borrower.LastName.Trim() == ""))
      {
        BorrowerInfo borrowerInfo = (BorrowerInfo) null;
        if (crmMapping1 != null)
          borrowerInfo = Session.ContactManager.GetBorrower(crmMapping1.ContactGuid);
        if (crmMapping1 == null || borrowerInfo == null)
        {
          DialogResult dialogResult = !applicationRight ? Utils.Dialog((IWin32Window) this, "Your primary borrower contact is not currently associated with any borrower contact.  You will need to establish a link.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) : Utils.Dialog((IWin32Window) this, "Your primary borrower contact is not currently associated with any borrower contact.  Would you like to establish a link.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
          if (dialogResult == DialogResult.Yes || dialogResult == DialogResult.OK)
          {
            using (RxBorrowerContact rxBorrowerContact = new RxBorrowerContact(false, false, true))
            {
              if (!rxBorrowerContact.ForcedClose)
              {
                if (rxBorrowerContact.ShowDialog((IWin32Window) this) == DialogResult.OK)
                {
                  flag1 = true;
                  borrowerInfo = rxBorrowerContact.BorrowerObj;
                }
              }
            }
          }
        }
        else
        {
          flag1 = true;
          RxBorrowerSSN rxBorrowerSsn = new RxBorrowerSSN(false, borrowerInfo, false, true);
          if (rxBorrowerSsn.HasConflict)
          {
            int num = (int) rxBorrowerSsn.ShowDialog((IWin32Window) this);
            borrowerInfo = rxBorrowerSsn.BorrowerObj;
          }
          RxBorrowerSync rxBorrowerSync = new RxBorrowerSync(false, borrowerInfo, true, true);
          if (rxBorrowerSync.HasConflict)
          {
            int num = (int) rxBorrowerSync.ShowDialog((IWin32Window) this);
            borrowerInfo = rxBorrowerSync.BorrowerObj;
          }
        }
        if (flag1)
        {
          DialogResult dialogResult = DialogResult.Yes;
          if (applicationRight)
            dialogResult = Utils.Dialog((IWin32Window) this, "Do you want to update linked borrower contact's home address with the subject property address?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
          if (dialogResult == DialogResult.Yes)
          {
            borrowerInfo.HomeAddress.Street1 = Session.LoanData.GetField("11");
            borrowerInfo.HomeAddress.City = Session.LoanData.GetField("12");
            borrowerInfo.HomeAddress.State = Session.LoanData.GetField("14");
            borrowerInfo.HomeAddress.Zip = Session.LoanData.GetField("15");
            Session.ContactManager.UpdateBorrower(borrowerInfo);
          }
        }
      }
      bool flag2 = false;
      CRMLog crmMapping2 = Session.LoanData.GetLogList().GetCRMMapping(borrowerPair.CoBorrower.Id);
      if (!(borrowerPair.CoBorrower.FirstName.Trim() == "") || !(borrowerPair.CoBorrower.LastName.Trim() == ""))
      {
        BorrowerInfo borrowerInfo = (BorrowerInfo) null;
        if (crmMapping2 != null)
          borrowerInfo = Session.ContactManager.GetBorrower(crmMapping2.ContactGuid);
        if (crmMapping2 == null || borrowerInfo == null)
        {
          if (Utils.Dialog((IWin32Window) this, "Your primary coborrower contact is not currently associated with any borrower contact.  Would you like to establish a link.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
          {
            using (RxBorrowerContact rxBorrowerContact = new RxBorrowerContact(true, false, true))
            {
              if (!rxBorrowerContact.ForcedClose)
              {
                if (rxBorrowerContact.ShowDialog((IWin32Window) this) == DialogResult.OK)
                {
                  flag2 = true;
                  borrowerInfo = rxBorrowerContact.BorrowerObj;
                }
              }
            }
          }
        }
        else
        {
          flag2 = true;
          borrowerInfo = Session.ContactManager.GetBorrower(crmMapping2.ContactGuid);
          RxBorrowerSSN rxBorrowerSsn = new RxBorrowerSSN(true, borrowerInfo, false, true);
          if (rxBorrowerSsn.HasConflict)
          {
            int num = (int) rxBorrowerSsn.ShowDialog((IWin32Window) this);
            borrowerInfo = rxBorrowerSsn.BorrowerObj;
          }
          RxBorrowerSync rxBorrowerSync = new RxBorrowerSync(true, borrowerInfo, true, true);
          if (rxBorrowerSync.HasConflict)
          {
            int num = (int) rxBorrowerSync.ShowDialog((IWin32Window) this);
            borrowerInfo = rxBorrowerSync.BorrowerObj;
          }
        }
        if (flag2)
        {
          DialogResult dialogResult = DialogResult.Yes;
          if (applicationRight)
            dialogResult = Utils.Dialog((IWin32Window) this, "Do you want to update linked coborrower contact's home address with the subject property address?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
          if (dialogResult == DialogResult.Yes)
          {
            borrowerInfo.HomeAddress.Street1 = Session.LoanData.GetField("11");
            borrowerInfo.HomeAddress.City = Session.LoanData.GetField("12");
            borrowerInfo.HomeAddress.State = Session.LoanData.GetField("14");
            borrowerInfo.HomeAddress.Zip = Session.LoanData.GetField("15");
            Session.ContactManager.UpdateBorrower(borrowerInfo);
          }
        }
      }
      new ContactUtil(Session.SessionObjects).InsertLinkedContactHistoryOnLoanClosed(Session.LoanData, true, false);
      Session.LoanData.SetField("2821", "Y");
      Session.LoanData.SetBorrowerPair(currentBorrowerPair);
      CRMLog[] allCrmMapping = Session.LoanData.GetLogList().GetAllCRMMapping();
      List<string> stringList = new List<string>();
      foreach (CRMLog crmLog in allCrmMapping)
      {
        if (crmLog.MappingType != CRMLogType.BorrowerContact && !stringList.Contains(crmLog.ContactGuid))
          stringList.Add(crmLog.ContactGuid);
      }
      if (stringList.Count == 0)
        return;
      BizPartnerInfo[] bizPartners = Session.ContactManager.GetBizPartners(stringList.ToArray());
      if (bizPartners == null || bizPartners.Length == 0)
        return;
      foreach (BizPartnerInfo bizPartnerInfo in bizPartners)
      {
        CustomFieldMappingCollection mappingCollection = CustomFieldMappingCollection.GetCustomFieldMappingCollection(Session.SessionObjects, new CustomFieldMappingCollection.Criteria(CustomFieldsType.BizCategoryCustom, bizPartnerInfo.CategoryID, true));
        if (mappingCollection.Count == 0)
          break;
        CustomFieldValueCollection fieldValueCollection = CustomFieldValueCollection.NewCustomFieldValueCollection(Session.SessionObjects, new CustomFieldValueCollection.Criteria(bizPartnerInfo.ContactID, bizPartnerInfo.CategoryID));
        foreach (CustomFieldMapping customFieldMapping in (CollectionBase) mappingCollection)
        {
          string str1 = (string) null;
          string str2;
          try
          {
            str2 = Session.LoanData.GetField(customFieldMapping.LoanFieldId);
          }
          catch (Exception ex)
          {
            Tracing.Log(MainScreen.sw, TraceLevel.Info, "Custom Field Mapping", string.Format("Loan Field ID '{0}', Value '{1}' to Business Category '{0} - Custom Field {1}' failed.", (object) customFieldMapping.LoanFieldId, str1 == null ? (object) "UNKNOWN" : (object) str1, (object) bizPartnerInfo.CategoryID, (object) customFieldMapping.FieldNumber.ToString()));
            str2 = (string) null;
          }
          if (str2 != null)
          {
            CustomFieldValue customFieldValue = CustomFieldValue.NewCustomFieldValue(customFieldMapping.RecordId, bizPartnerInfo.ContactID, customFieldMapping.FieldFormat);
            customFieldValue.FieldValue = str2;
            fieldValueCollection.Add(customFieldValue);
          }
        }
        if (0 < fieldValueCollection.Count)
          fieldValueCollection.Save();
      }
    }

    private void resetAutosaveFiles(
      string autosaveFile,
      string autosaveAttFile,
      string autosaveHisFile,
      string autosaveTsFile)
    {
      if (Session.LoanDataMgr == null || !Session.LoanDataMgr.Writable || !this.timerAutosave.Enabled)
        return;
      this.timerAutosave.Stop();
      this.removeAutoSavedFiles(autosaveFile, autosaveAttFile, autosaveHisFile, autosaveTsFile);
      this.StartAutosaveTimer();
    }

    public bool CloseLoan(bool allowCancel)
    {
      if (!this.HasOpenLoan)
        return true;
      string str1 = (string) null;
      if (Session.LoanDataMgr != null && Session.LoanDataMgr.LoanData != null)
        str1 = Session.LoanDataMgr.LoanData.GUID;
      string str2 = (string) null;
      if (Session.LoanDataMgr.LinkedLoan != null && Session.LoanDataMgr.LinkedLoan.LoanData != null)
        str2 = Session.LoanDataMgr.LinkedLoan.LoanData.GUID;
      if (!this.closeEditor(true, true, false, allowCancel))
        return false;
      BalloonToolTip ceBalloonToolTip = this.GetCEBalloonToolTip();
      if (ceBalloonToolTip.Visible)
        ceBalloonToolTip.Close();
      BalloonToolTip updatesBalloonToolTip = this.GetCEGetUpdatesBalloonToolTip();
      if (updatesBalloonToolTip.Visible)
        updatesBalloonToolTip.Close();
      return true;
    }

    public bool CloseLoanWithoutPrompts(bool saveChanges)
    {
      return !this.HasOpenLoan || this.closeEditor(false, saveChanges, saveChanges, false);
    }

    private bool closeEditor(bool displayPrompts, bool saveLoan, bool forceSave, bool allowCancel)
    {
      Cursor.Current = Cursors.WaitCursor;
      if (saveLoan)
      {
        if (Session.LoanData.Dirty)
        {
          if (!this.promptForSave(displayPrompts, forceSave, allowCancel))
            return false;
        }
        else if (this.saveButtonIsClicked)
          this.SaveLoan(false, false);
        this.saveButtonIsClicked = false;
      }
      if (Session.LoanDataMgr.Writable)
      {
        TradeManagementUtils.SyncLoanTradeData(Session.LoanData.GUID, false);
        ImportConditionFactory.ClearImportedResourceIds();
      }
      if (Session.LoanDataMgr.Writable & displayPrompts)
      {
        StatusOnlineManager.CheckStatusOnline(Session.LoanDataMgr);
        LoanServiceManager.CheckLoan(Session.LoanDataMgr);
      }
      if (this.timerAutosave.Enabled)
        this.timerAutosave.Stop();
      if (Session.LoanData != null)
      {
        Session.LoanData.LoanDataModified -= this.LoanDataModifiedHandler;
        Session.LoanData.Close();
      }
      FieldHelpDialog.Close();
      MainForm.Instance.ClearLastSaveTime();
      this.loanPage.Unload();
      if (!Session.StartupInfo.DataServicesOptOut)
        DataServicesManager.UpdateReport();
      this.clearCurrentAutosaveFiles();
      if (Session.SessionObjects.AllowConcurrentEditing && Session.LoanDataMgr.Writable)
      {
        UserShortInfoList workingOnTheLoan = Session.LoanDataMgr.GetUsersWorkingOnTheLoan(Session.SessionObjects.SessionID, true);
        if (workingOnTheLoan != null && workingOnTheLoan.Count > 0)
          Session.ServerManager.SendMessage((EllieMae.EMLite.ClientServer.Message) new CEMessage(Session.UserInfo, CEMessageType.UserExitLoan), workingOnTheLoan.SessionIDs, true);
      }
      this.clearCurrentLoan();
      if (this.defaultTabAfterLoanClosed != null && this.tabControl.TabPages.Contains(this.defaultTabAfterLoanClosed))
        this.tabControl.SelectedTab = this.defaultTabAfterLoanClosed;
      this.tabControl.Visible = false;
      this.tabControl.TabPages.Remove(this.loanTabPage);
      this.tabControl.TabPages.Remove(this.ePASSTabPage);
      this.tabControl.Visible = true;
      Cursor.Current = Cursors.Default;
      return true;
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
        if (Utils.CheckIf2015RespaTila(Session.LoanData.GetField("3969")))
        {
          if (Session.LoanData.Calculator != null)
            Session.LoanData.Calculator.CalculateFeeVariance();
          if (Session.LoanDataMgr != null)
            Session.LoanDataMgr.SyncESignConsentData();
        }
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
      bool flag = false;
      DDMTrigger ddmTrigger = (DDMTrigger) Session.LoanData.DDMTrigger;
      if (ddmTrigger != null)
      {
        if (string.Compare(Session.LoanData.GUID, ddmTrigger.LoanGUID, true) != 0)
        {
          try
          {
            ddmTrigger.ReassignUnsavedLoanBacktoDDM(Session.LoanData, Session.LoanDataMgr.ConfigInfo);
            flag = this.SaveLoan(false, false);
            ddmTrigger.ReassignNewlyCreatedLoanBacktoDDM();
            goto label_32;
          }
          catch (Exception ex)
          {
            Tracing.Log(MainScreen.sw, TraceLevel.Error, nameof (MainScreen), "The loan in DDMTrigger is different than the loan in Session.LoanDataMgr, but DDMTrigger cannot reset FieldProvider to use the loan available in Session.LoanDataMgr due to this error: " + ex.Message);
            goto label_32;
          }
        }
      }
      flag = this.SaveLoan(false, false);
label_32:
      return flag;
    }

    private void clearCurrentAutosaveFiles()
    {
      if (Session.LoanDataMgr == null || !Session.LoanDataMgr.Writable)
        return;
      string autoRecoverFileName = this.getAutoRecoverFileName(Session.LoanDataMgr);
      string recoverAttFileName = this.getAutoRecoverAttFileName(autoRecoverFileName);
      string recoverHistoryFileName = this.getAutoRecoverHistoryFileName(autoRecoverFileName);
      string recoverTsFileName = this.getAutoRecoverTsFileName(autoRecoverFileName);
      this.removeAutoSavedFiles(autoRecoverFileName, recoverAttFileName, recoverHistoryFileName, recoverTsFileName);
    }

    private void clearCurrentLoan()
    {
      if (Session.LoanData != null)
        Session.SetLoanDataMgr((LoanDataMgr) null);
      this.hasOpenLoan = false;
    }

    public bool ClosingApp(bool displayLogoutPrompt)
    {
      if (!TradeUpdateLoansDialog.Instance.IsNoJobRunning())
      {
        if (Utils.Dialog((IWin32Window) this, "There are trades pending in the Trade Update Queue, closing Encompass will cancel the update process. Are you sure you want to proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          return false;
        Cursor.Current = Cursors.WaitCursor;
        TradeUpdateLoansDialog.Instance.CancelAllJobs();
        while (!TradeUpdateLoansDialog.Instance.IsNoJobRunning())
          this.WaitSomeTime(1000);
        Cursor.Current = Cursors.Default;
      }
      return (!displayLogoutPrompt || LogoutDialog.Display((IWin32Window) this)) && this.exitCleanUp(true);
    }

    public void WaitSomeTime(int interval) => Task.Delay(interval).Wait();

    private bool exitCleanUp(bool withCancelButton)
    {
      if (Session.LoanDataMgr != null && !this.CloseLoan(withCancelButton) || this.tradingConsole != null && !this.tradingConsole.QueryCloseConsole() || this.reportControl != null && !this.reportControl.PromptSaveCurrentReport(withCancelButton))
        return false;
      if (this.contactPage != null)
        this.contactPage.Close();
      if (this.homeControl != null)
        this.homeControl.LogoutUser();
      return true;
    }

    public void OpenCalculator()
    {
      try
      {
        Process.Start("C:\\Windows\\System32\\Calc.exe");
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Starting calculator is failed with error message " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    public void OpenLoanMailbox()
    {
      using (LoanMailboxDialog loanMailboxDialog = new LoanMailboxDialog())
      {
        int num = (int) loanMailboxDialog.ShowDialog((IWin32Window) this);
      }
      EPassMessages.SyncReadMessages(false);
      if (this.PipelineScreenBrowser == null)
        return;
      this.PipelineScreenBrowser.RefreshPipeline(true);
    }

    public bool IsModalDialogOpen()
    {
      foreach (Form ownedForm in this.OwnedForms)
      {
        if (ownedForm.Modal)
          return true;
      }
      return false;
    }

    public bool CloseModalDialogs()
    {
      bool flag = true;
      foreach (Form form in new List<Form>((IEnumerable<Form>) Session.MainForm.OwnedForms))
      {
        if (form.Modal)
        {
          try
          {
            if (form.DialogResult == DialogResult.None)
              form.DialogResult = DialogResult.Cancel;
            form.Close();
          }
          catch
          {
            flag = false;
          }
        }
      }
      return flag;
    }

    private void displayHelpText(string text) => MainForm.Instance.DisplayHelpText(text);

    private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
      MainForm.Instance.HideInsightsOnStatusBar();
      MainForm.Instance.EnableDisableMenuItems(MainForm.MenuItemEnum.eFolder, this.tabControl.Controls.Contains((Control) this.loanTabPage));
      MainForm.Instance.EnableDisableMenuItems(MainForm.MenuItemEnum.Loan, this.tabControl.Controls.Contains((Control) this.loanTabPage));
      this.activateCurrentTabPage(this.tabControl.Controls.Contains((Control) this.loanTabPage) ? 0 : 1);
    }

    private void activateTabPage(MainScreen.TabPageEnum tabPage)
    {
      this.tabControl.SelectedTab = this.tabPages[(int) tabPage];
      this.activateCurrentTabPage();
    }

    private void activateCurrentTabPage() => this.activateCurrentTabPage(0);

    private void activateCurrentTabPage(int sqlRead)
    {
      PerformanceMeter performanceMeter = (PerformanceMeter) null;
      Tracing.Log(MainScreen.sw, TraceLevel.Info, nameof (MainScreen), "Activating current tab page...");
      using (Tracing.StartTimer(MainScreen.sw, nameof (MainScreen), TraceLevel.Info, "Tab page: " + this.tabControl.SelectedTab.Text))
      {
        this.displayHelpText("Press F1 for Help");
        if (this.currentTabPage == this.contactTabPage && this.contactPage != null)
          this.contactPage.SaveContactChanges();
        foreach (TabPage tabPage in this.tabControl.TabPages)
        {
          if (tabPage != this.tabControl.SelectedTab && tabPage.Controls.Count != 0 && (tabPage == this.pipelineTabPage || tabPage == this.loanTabPage || tabPage == this.contactTabPage || tabPage == this.dashboardTabPage || tabPage == this.reportsTabPage || tabPage == this.ePASSTabPage || tabPage == this.homeTabPage || tabPage == this.tradesTabPage))
            tabPage.Controls.Clear();
        }
        if (this.tabControl.SelectedTab != this.pipelineTabPage && this.PipelineScreenBrowser != null)
          this.PipelineScreenBrowser.DisableRefreshTimer();
        if (this.tabControl.SelectedTab != this.loanTabPage && this.tabControl.SelectedTab != this.ePASSTabPage)
          this.defaultTabAfterLoanClosed = this.tabControl.SelectedTab;
        try
        {
          if (this.tabControl.SelectedTab == this.pipelineTabPage)
          {
            using (MetricsFactory.GetIncrementalTimer("PipelineRefreshIncTimer", (SFxTag) new SFxUiTag()))
            {
              if (this.PipelineScreenBrowser == null)
                performanceMeter = PerformanceMeter.StartNew("Pipeline.Load", "Initial load of the Pipeline screen", true, 2712, nameof (activateCurrentTabPage), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
              this.loadPipelineScreen(sqlRead);
              this.PipelineScreenBrowser.EnableRefreshTimer();
              using (PerformanceMeter.StartNew("Pipeline.Load.CheckPoint6", "Refreshing Folders", false, 2716, nameof (activateCurrentTabPage), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs"))
                this.PipelineScreenBrowser.RefreshFolders();
              MainForm.Instance.SetMenu("Pipeline");
              this.displayHelpText("Right click on a loan for more options");
            }
          }
          else if (this.tabControl.SelectedTab == this.ePASSTabPage)
          {
            if (this.ePASSTabPage.Controls.Count == 0)
            {
              this.ePASSTabPage.Controls.Add((Control) this.epassControl);
              this.epassControl.Dock = DockStyle.Fill;
            }
            MainForm.Instance.SetMenu("Services View");
          }
          else if (this.tabControl.SelectedTab == this.contactTabPage)
          {
            if (this.contactTabPage.Controls.Count == 0)
            {
              this.contactPage.Parent = (Control) this.contactTabPage;
              this.contactTabPage.Controls.Add((Control) this.contactPage);
            }
            this.contactPage.ShowContent();
            if (this.contactPage.CurrentFormState == ContactMainForm.FormState.Borrower)
              this.contactPage_OnContactTabChange(ContactMainForm.ContactsContentEnum.BorrowerContacts);
            else if (this.contactPage.CurrentFormState == ContactMainForm.FormState.BizContact)
              this.contactPage_OnContactTabChange(ContactMainForm.ContactsContentEnum.BusinessContacts);
            else if (this.contactPage.CurrentFormState == ContactMainForm.FormState.Calendar)
              this.contactPage_OnContactTabChange(ContactMainForm.ContactsContentEnum.Calendar);
            else if (this.contactPage.CurrentFormState == ContactMainForm.FormState.Campaign)
              this.contactPage_OnContactTabChange(ContactMainForm.ContactsContentEnum.Campaigns);
            else
              this.contactPage_OnContactTabChange(ContactMainForm.ContactsContentEnum.Tasks);
          }
          else if (this.tabControl.SelectedTab == this.homeTabPage)
          {
            if (!MainScreen.homePageLoaded)
            {
              Task.Run((System.Action) (() => this.homeControl.GoHome()));
              MainScreen.homePageLoaded = true;
            }
            if (this.homeTabPage.Controls.Count == 0)
            {
              this.homeTabPage.Controls.Add((Control) this.homeControl);
              this.homeControl.Dock = DockStyle.Fill;
              this.homeControl.Focus();
            }
            MainForm.Instance.SetMenu("Home");
          }
          else if (this.tabControl.SelectedTab == this.dashboardTabPage)
          {
            if (this.frmDashboard == null)
            {
              performanceMeter = PerformanceMeter.StartNew("Dashboard.Load", "Initial load of the Dashboard screen", true, 2790, nameof (activateCurrentTabPage), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
              this.frmDashboard = new DashboardForm(Session.DefaultInstance);
              this.frmDashboard.TopLevel = false;
              this.frmDashboard.Visible = true;
              this.frmDashboard.Dock = DockStyle.Fill;
              this.frmDashboard.Parent = (Control) this.dashboardTabPage;
              this.frmDashboard.ShowContent();
            }
            this.dashboardTabPage.Controls.Add((Control) this.frmDashboard);
            MainForm.Instance.SetMenu("Dashboard");
            this.displayHelpText("Right click on a view for more options");
          }
          else if (this.tabControl.SelectedTab == this.reportsTabPage)
          {
            if (this.reportControl == null)
              performanceMeter = PerformanceMeter.StartNew("Reports.Load", "Initial load of the Reports screen", true, 2808, nameof (activateCurrentTabPage), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
            this.loadReportScreen();
            this.reportsTabPage.Controls.Add((Control) this.reportControl);
            this.reportControl.Dock = DockStyle.Fill;
            MainForm.Instance.SetMenu("Reports");
            this.displayHelpText("Right click on a report for more options");
          }
          else if (this.tabControl.SelectedTab == this.loanTabPage)
          {
            this.loanTabPage.Controls.Add((Control) this.loanPage);
            MainForm.Instance.SetMenu("Loan");
          }
          else if (this.tabControl.SelectedTab == this.tradesTabPage)
          {
            if (AccessUtils.CanAccessFeature(Feature.WebTrading))
            {
              if (this.TradingScreenBrowser == null)
                performanceMeter = PerformanceMeter.StartNew("Trades.Load", "Initial load of Trades screen", true, 2829, nameof (activateCurrentTabPage), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
              this.LoadTradingBrowser();
            }
            else
            {
              if (this.tradingConsole == null)
              {
                performanceMeter = PerformanceMeter.StartNew("Trades.Load", "Initial load of Trades screen", true, 2837, nameof (activateCurrentTabPage), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
                this.tradingConsole = TradeManagementConsole.Instance;
                this.TradingScreenBrowser = (ITradeConsole) TradeManagementConsole.Instance;
                this.TradingScreenBrowserMenuProvider = (IMenuProvider) TradeManagementConsole.Instance;
              }
              this.tradesTabPage.Controls.Add((Control) this.tradingConsole);
              this.tradingConsole.RefreshContents();
              MainForm.Instance.SetMenu("Trades");
            }
          }
          if (this.tabControl.SelectedTab != this.loanTabPage)
          {
            BalloonToolTip ceBalloonToolTip = this.GetCEBalloonToolTip();
            if (ceBalloonToolTip.Visible)
              ceBalloonToolTip.Close();
            BalloonToolTip updatesBalloonToolTip = this.GetCEGetUpdatesBalloonToolTip();
            if (updatesBalloonToolTip.Visible)
              updatesBalloonToolTip.Close();
          }
          this.currentTabPage = this.tabControl.SelectedTab;
          if (this.homeControl != null)
            this.homeControl.LogAction("TabChange", this.currentTabPage.Text);
          MainForm.Instance.CheckViewMenuItem(this.currentTabPage);
        }
        catch
        {
          performanceMeter?.Abort();
          performanceMeter = (PerformanceMeter) null;
          throw;
        }
        finally
        {
          if (performanceMeter != null)
          {
            performanceMeter.Stop();
            MetricsFactory.RecordIncrementalTimerSample("EMA_Pipeline_Load", Convert.ToInt64(performanceMeter.Duration.TotalMilliseconds), (SFxTag) new SFxUiTag());
          }
        }
      }
    }

    private void contactPage_OnContactTabChange(ContactMainForm.ContactsContentEnum tab)
    {
      switch (tab)
      {
        case ContactMainForm.ContactsContentEnum.BorrowerContacts:
          MainForm.Instance.SetMenu("Contact - Borrower");
          break;
        case ContactMainForm.ContactsContentEnum.BusinessContacts:
          MainForm.Instance.SetMenu("Contact - Business");
          break;
        case ContactMainForm.ContactsContentEnum.Calendar:
          MainForm.Instance.SetMenu("Contact - Calendar");
          break;
        case ContactMainForm.ContactsContentEnum.Tasks:
          MainForm.Instance.SetMenu("Contact - Tasks");
          break;
        case ContactMainForm.ContactsContentEnum.Campaigns:
          MainForm.Instance.SetMenu("Contact - Campaigns");
          break;
      }
    }

    public void OpenURL(string url, string title, int width, int height)
    {
      WebViewer.OpenURL(url, title, width, height);
    }

    public Form OpenURL(string windowName, string url, string title, int width, int height)
    {
      return WebViewer.OpenURL(windowName, url, title, width, height);
    }

    public void DisplayHelp(string helpTopic)
    {
      JedHelp.ShowHelp((Control) this, (string) null, helpTopic);
    }

    public void ShowTab(string tabText)
    {
      foreach (TabPage tabPage in this.tabControl.TabPages)
      {
        if (tabPage.Text == tabText)
          this.tabControl.SelectedTab = tabPage;
      }
    }

    public void ShowVerifPanel(string verifType) => this.loanPage.ShowVerifPanel(verifType);

    public void DisplayEditor() => this.SetCurrentActivity(EncompassActivity.Loans);

    public bool OpenLoan(string guid)
    {
      if (!this.InvokeRequired)
        return this.OpenLoan(guid, LoanInfo.LockReason.OpenForWork);
      return (bool) this.Invoke((Delegate) new MainScreen.OpenLoanDelegate(this.OpenLoan), (object) guid, (object) LoanInfo.LockReason.OpenForWork);
    }

    public bool OpenLoan(string guid, LoanInfo.LockReason lockReason)
    {
      return this.OpenLoan(guid, lockReason, true);
    }

    public bool OpenLoan(string guid, bool displayEditor)
    {
      return this.OpenLoan(guid, LoanInfo.LockReason.OpenForWork, displayEditor);
    }

    public bool OpenLoan(string guid, bool displayEditor, bool interactive)
    {
      return this.OpenLoan(guid, LoanInfo.LockReason.OpenForWork, displayEditor, interactive);
    }

    public bool OpenLoan(LoanDataMgr dataMgr, bool displayEditor)
    {
      if (this.HasOpenLoan && !this.CloseLoan(true))
        return false;
      this.startEditor(dataMgr);
      this.startBackGroundThreadTask();
      if (displayEditor)
        this.DisplayEditor();
      return true;
    }

    private void startBackGroundThreadTask()
    {
      new Thread(new ThreadStart(this.synchronizeDisclosureTrackingRecords))
      {
        IsBackground = true,
        Priority = ThreadPriority.BelowNormal
      }.Start();
      if (Session.LoanDataMgr == null || Session.LoanDataMgr.LoanData == null)
        return;
      new Thread(new ThreadStart(this.submitWebCenterImportStatus))
      {
        IsBackground = true,
        Priority = ThreadPriority.BelowNormal
      }.Start();
    }

    private void synchronizeDisclosureTrackingRecords()
    {
      try
      {
        Session.LoanDataMgr.SyncAllEDisclosurePackageStatuses(false);
        if (MainScreen.RefreshAlertLogAsync == null)
          return;
        MainScreen.RefreshAlertLogAsync((object) null, (EventArgs) null);
      }
      catch (Exception ex)
      {
      }
    }

    private void submitWebCenterImportStatus()
    {
      try
      {
        Session.LoanDataMgr.SubmitTPOLoanImportStatus(Session.LoanDataMgr.LoanData.GUID);
      }
      catch (Exception ex)
      {
      }
    }

    public bool OpenLoan(string guid, LoanInfo.LockReason lockReason, bool displayEditor)
    {
      return this.OpenLoan(guid, lockReason, displayEditor, true);
    }

    public bool OpenLoan(
      string guid,
      LoanInfo.LockReason lockReason,
      bool displayEditor,
      bool interactive)
    {
      if (!this.HasAccessToLoanTab)
      {
        if (interactive)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You do not have access to the Pipeline/Loan tab.");
        }
        return false;
      }
      using (PerformanceMeter perfMeter = PerformanceMeter.StartNew("Loan.Open", "Opening a loan file", true, true, 3095, nameof (OpenLoan), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs"))
      {
        perfMeter.AddCheckpoint("Loan GUID: " + guid, 3097, nameof (OpenLoan), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
        if (MainScreen.numberOfLoansOpened == 0)
          perfMeter.AddCheckpoint("First loan open", 3099, nameof (OpenLoan), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
        if (!this.HasOpenLoan || Session.LoanData.GUID != guid)
        {
          if (this.HasOpenLoan && !this.CloseLoan(true))
            return false;
          if (MainScreen.fieldDefinitionsTask == null)
            MainScreen.fieldDefinitionsTask = Task.Run<LoanReportFieldDefs>((Func<LoanReportFieldDefs>) (() => LoanReportFieldDefs.GetFieldDefs(Session.DefaultInstance, LoanReportFieldFlags.AllDatabaseFields)));
          FastLoanAccessResponse loanAccessResponse = (FastLoanAccessResponse) null;
          bool fastLoanLoad = Session.StartupInfo.FastLoanLoad;
          PipelineInfo pipelineInfo;
          if (fastLoanLoad)
          {
            loanAccessResponse = Session.LoanManager.GetFastLoanAccess(guid);
            pipelineInfo = loanAccessResponse.PipelineInfo;
          }
          else
            pipelineInfo = Session.LoanManager.GetPipeline(new string[1]
            {
              guid
            }, false, 0)[0];
          if (pipelineInfo == null)
          {
            if (interactive)
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "The selected loan has been deleted or is not longer accessible.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return false;
          }
          LoanInfo.Right right = !fastLoanLoad ? this.getUserAccessToLoan(pipelineInfo) : loanAccessResponse.Right;
          switch (right)
          {
            case LoanInfo.Right.NoRight:
              if (interactive)
              {
                int num1 = (int) Utils.Dialog((IWin32Window) this, "You no longer have the necessary rights to access this loan file. Contact your system administrator if you require further access to this loan.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              }
              return false;
            case LoanInfo.Right.Read:
              perfMeter.Abort();
              if (interactive && Utils.Dialog((IWin32Window) this, "You only have read-only access to this loan file. You won't be able to save any changes. Do you still want to open this loan?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
                return false;
              break;
            default:
              if (string.Compare(pipelineInfo.LoanFolder, SystemSettings.TrashFolder, true) == 0)
              {
                perfMeter.Abort();
                if (interactive && Utils.Dialog((IWin32Window) this, "This loan is currently in a Trash folder.  You won't be able to save any changes. Do you still want to open this loan?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
                  return false;
                break;
              }
              break;
          }
          if (right == LoanInfo.Right.Read || pipelineInfo.LoanFolder.ToLower() == SystemSettings.TrashFolder.ToLower())
            lockReason = LoanInfo.LockReason.NotLocked;
          string field = (string) pipelineInfo.GetField("ActionTaken");
          if (!EllieMae.EMLite.Common.LoanStatus.ActiveLoan.Contains((object) field))
          {
            perfMeter.Abort();
            if (interactive)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "The current status of this loan is \"" + field + "\".   Alerts will not appear in pipeline unless you change the status back to \"Active Loan\" on the Borrower Summary.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
          }
          Session.SessionObjects.StartAsyncGetEpassMessagesTask(guid, Session.UserID);
          ++MainScreen.numberOfLoansOpened;
          LoanDataMgr loanDataMgr = this.loadLoan(pipelineInfo, lockReason, perfMeter, 0);
          if (loanDataMgr == null)
            return false;
          this.startEditor(loanDataMgr);
          perfMeter.AddCheckpoint("startEditor(dataMgr)", 3193, nameof (OpenLoan), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
          this.startBackGroundThreadTask();
        }
        if (displayEditor)
          this.DisplayEditor();
        try
        {
          this.SetLateDaysEndDefaultValue(MainScreen.fieldDefinitionsTask.Result);
        }
        catch (Exception ex)
        {
          Tracing.Log(MainScreen.sw, nameof (MainScreen), TraceLevel.Error, "SetLateDaysEndDefaultValue: " + ex.Message);
        }
        perfMeter.Stop();
        MetricsFactory.RecordIncrementalTimerSample("EMA_Loan_Open", Convert.ToInt64(perfMeter.Duration.TotalMilliseconds), (SFxTag) new SFxUiTag());
      }
      return true;
    }

    private void SetLateDaysEndDefaultValue(LoanReportFieldDefs fieldDefinitions)
    {
      LoanReportFieldDef fieldByCriterionName = fieldDefinitions.GetFieldByCriterionName(Session.LoanData.GetField("4112"));
      if (fieldByCriterionName == null || string.IsNullOrEmpty(fieldByCriterionName.Description))
        return;
      Session.LoanData.SetField("4112", fieldByCriterionName.Description);
    }

    private LoanInfo.Right getUserAccessToLoan(PipelineInfo pInfo)
    {
      return ((LoanAccessBpmManager) Session.BPM.GetBpmManager(BpmCategory.LoanAccess)).GetEffectiveRightsForLoan(pInfo);
    }

    private bool askForReadRightAccess(string msg = null)
    {
      if (msg == null)
        msg = Messages.GetMessage("ReadRightOnly");
      return Utils.Dialog((IWin32Window) Session.MainScreen, msg, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
    }

    private MainScreen.LoanAccessResult checkLoanAccessPermissions(LoanDataMgr loanDataMgr)
    {
      MainScreen.LoanAccessResult loanAccessResult = MainScreen.LoanAccessResult.Success;
      if (loanDataMgr.LoanData.ContentAccess == LoanContentAccess.FullAccess)
        return loanAccessResult;
      string accessRightMessage = LoanAccess.GetAccessRightMessage(loanDataMgr.LoanData.ContentAccess);
      string text;
      if (loanDataMgr.LoanData.ContentAccess == LoanContentAccess.None || accessRightMessage == string.Empty)
      {
        loanAccessResult = MainScreen.LoanAccessResult.ReadOnly;
        text = Messages.GetMessage("ReadRightOnly");
      }
      else
        text = "Your access to this loan file is limited. Only changes made in the following areas will be saved:" + Environment.NewLine + Environment.NewLine + accessRightMessage + Environment.NewLine + "Do you still want to open this loan file?";
      PerformanceMeter.Current.Abort();
      return Utils.Dialog((IWin32Window) Session.MainScreen, text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No ? MainScreen.LoanAccessResult.UserRefusedToProceed : loanAccessResult;
    }

    private LoanDataMgr closeAndAbort(LoanDataMgr loanDataMgr)
    {
      loanDataMgr?.Close();
      return (LoanDataMgr) null;
    }

    private MainScreen.LoanAccessResult forceUnlock(LoanDataMgr loanDataMgr)
    {
      try
      {
        loanDataMgr.Unlock(true);
        return MainScreen.LoanAccessResult.Success;
      }
      catch (EllieMae.EMLite.ClientServer.Exceptions.SecurityException ex)
      {
        return this.askForReadRightAccess() ? MainScreen.LoanAccessResult.ReadOnly : MainScreen.LoanAccessResult.UserRefusedToProceed;
      }
    }

    private string getMessageForLock(LockInfo currentLock)
    {
      return currentLock.LockedFor == LoanInfo.LockReason.Downloaded ? Messages.GetMessage("OpenDownLoad", (object) currentLock.LockedBy) : Messages.GetMessage("OpenWork", (object) currentLock.LockedBy);
    }

    private void assignLoanContentAccess(LoanDataMgr loanDataMgr, LoanInfo.LockReason lockReason)
    {
      LoanContentAccess loanContentAccess = LoanContentAccess.FullAccess;
      LoanAccessBpmManager bpmManager = (LoanAccessBpmManager) Session.BPM.GetBpmManager(BpmCategory.LoanAccess);
      if (lockReason != LoanInfo.LockReason.NotLocked)
        loanContentAccess = bpmManager.GetLoanContentAccess(loanDataMgr.LoanData);
      if (loanDataMgr.AccessRules.AllowFullAccess())
        loanContentAccess = LoanContentAccess.FullAccess;
      loanDataMgr.LoanData.ContentAccess = loanContentAccess;
    }

    private LoanDataMgr loadLoan(
      PipelineInfo pInfo,
      LoanInfo.LockReason lockReason,
      PerformanceMeter perfMeter,
      int sqlRead)
    {
      Cursor.Current = Cursors.WaitCursor;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      LoanDataMgr loanDataMgr = (LoanDataMgr) null;
      try
      {
        if (lockReason == LoanInfo.LockReason.NotLocked)
        {
          loanDataMgr = this.openLoanDataMgr(pInfo.GUID, sqlRead);
        }
        else
        {
          loanDataMgr = this.openLoanDataMgr(pInfo.GUID, sqlRead, true);
          if (loanDataMgr == null)
            return (LoanDataMgr) null;
          this.assignLoanContentAccess(loanDataMgr, lockReason);
          flag3 = true;
          switch (this.checkLoanAccessPermissions(loanDataMgr))
          {
            case MainScreen.LoanAccessResult.ReadOnly:
              flag1 = true;
              loanDataMgr.Unlock(true);
              break;
            case MainScreen.LoanAccessResult.UserRefusedToProceed:
              return this.closeAndAbort(loanDataMgr);
          }
        }
      }
      catch (LockException ex)
      {
        perfMeter.Abort();
        loanDataMgr = this.openLoanDataMgr(pInfo.GUID, sqlRead);
        if (loanDataMgr == null)
          return (LoanDataMgr) null;
        flag2 = true;
        LockInfo lockInfo = ex.LockInfo;
        if (ex.LockInfo.LockedBy == Session.UserID && !Session.SessionObjects.AllowConcurrentEditing)
        {
          if ((loanDataMgr.GetEffectiveRight(Session.UserID) & LoanInfo.Right.Access) == LoanInfo.Right.NoRight)
          {
            if (!this.askForReadRightAccess(this.getMessageForLock(lockInfo)))
              return this.closeAndAbort(loanDataMgr);
          }
          else
          {
            switch (new UnlockLoanDialog(lockInfo).ShowDialog())
            {
              case DialogResult.Cancel:
                return this.closeAndAbort(loanDataMgr);
              case DialogResult.Yes:
                switch (this.forceUnlock(loanDataMgr))
                {
                  case MainScreen.LoanAccessResult.UserRefusedToProceed:
                    return this.closeAndAbort(loanDataMgr);
                  case MainScreen.LoanAccessResult.Success:
                    loanDataMgr.Close();
                    return this.loadLoan(pInfo, lockReason, perfMeter, sqlRead);
                }
                break;
            }
          }
        }
        else if ((ex.LockInfo.LockedBy ?? "").Trim() == "")
        {
          switch (this.forceUnlock(loanDataMgr))
          {
            case MainScreen.LoanAccessResult.UserRefusedToProceed:
              return this.closeAndAbort(loanDataMgr);
            case MainScreen.LoanAccessResult.Success:
              loanDataMgr.Close();
              return this.loadLoan(pInfo, lockReason, perfMeter, sqlRead);
          }
        }
        else if (!this.askForReadRightAccess(this.getMessageForLock(lockInfo)))
          return this.closeAndAbort(loanDataMgr);
      }
      catch (EllieMae.EMLite.ClientServer.Exceptions.SecurityException ex)
      {
        perfMeter.Abort();
        if (!this.askForReadRightAccess())
          return this.closeAndAbort(loanDataMgr);
        loanDataMgr?.Close();
        loanDataMgr = this.openLoanDataMgr(pInfo.GUID, sqlRead);
        if (loanDataMgr == null)
          return (LoanDataMgr) null;
        flag2 = true;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
      if (loanDataMgr == null)
        return (LoanDataMgr) null;
      if (!flag3)
        this.assignLoanContentAccess(loanDataMgr, lockReason);
      if (lockReason != LoanInfo.LockReason.NotLocked && !(flag1 | flag2) && Session.SessionObjects.AllowConcurrentEditing && loanDataMgr.Writable)
      {
        UserShortInfoList workingOnTheLoan = loanDataMgr.GetUsersWorkingOnTheLoan(Session.SessionObjects.SessionID, true);
        if (workingOnTheLoan != null && workingOnTheLoan.Count > 0)
        {
          perfMeter.Abort();
          if (new CEOpenLoanForm(workingOnTheLoan).ShowDialog((IWin32Window) this) != DialogResult.Yes)
          {
            loanDataMgr.GetUsersWorkingOnTheLoan(Session.SessionObjects.SessionID, true);
            return this.closeAndAbort(loanDataMgr);
          }
          Session.ServerManager.SendMessage((EllieMae.EMLite.ClientServer.Message) new CEMessage(Session.UserInfo, CEMessageType.UserOpenLoan), workingOnTheLoan.SessionIDs, true);
        }
      }
      return loanDataMgr;
    }

    private void startEditor(LoanDataMgr loanDataMgr)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.loadPipelineScreen();
      Session.SetLoanDataMgr(loanDataMgr, true);
      if (!loanDataMgr.IsNew())
      {
        loanDataMgr.LoanData.BaseLastModified = loanDataMgr.LastModified;
        loanDataMgr.LoanData.Dirty = false;
      }
      this.hasOpenLoan = true;
      this.initializeLoanEditor(false);
      PerformanceMeter.Current.AddCheckpoint("Finished initializing the loan editor", 3563, nameof (startEditor), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
      if (loanDataMgr.Writable)
      {
        this.RecoverAutoSavedFile();
        this.StartAutosaveTimer();
        PerformanceMeter.Current.AddCheckpoint("Finished recovering the auto save data", 3570, nameof (startEditor), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
      }
      if (loanDataMgr.LoanObject != null)
        Task.Run((System.Action) (() => loanDataMgr.LoanObject.AddToRecentLoans()));
      Session.LoanData.LoanDataModified += MainScreen.Instance.LoanDataModifiedHandler;
      MainForm.Instance.SetLastSaveTime(Session.LoanDataMgr.LastModified, Session.LoanData.Dirty);
      if (Session.StartupInfo.DataServicesOptOut)
        return;
      Session.InitialDataServicesData = DataServicesManager.RetrieveReportData(loanDataMgr);
      PerformanceMeter.Current.AddCheckpoint("Finished retrieving report data for DataServicesManager", 3585, nameof (startEditor), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs");
    }

    public bool StartNewLoan(bool displayEditor)
    {
      string[] foldersForAction = ((LoanFolderRuleManager) Session.BPM.GetBpmManager(BpmCategory.LoanFolder)).GetLoanFoldersForAction(LoanFolderAction.Originate);
      if (foldersForAction.Length == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You are not authorized to originate loans in any loan folder.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      using (LoanFolderSelectionDialog folderSelectionDialog = new LoanFolderSelectionDialog(foldersForAction, Session.WorkingFolder))
        return folderSelectionDialog.ShowDialog((IWin32Window) this) == DialogResult.OK && this.StartNewLoan(folderSelectionDialog.SelectedFolder, displayEditor);
    }

    public bool StartNewLoan(string loanFolder, bool displayEditor)
    {
      LoanTemplateSelection loanTemplate = (LoanTemplateSelection) null;
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      using (LoanTemplateSelectDialog templateSelectDialog = new LoanTemplateSelectDialog(Session.DefaultInstance, false, aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_CreateBlank), aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_CreateFromTmpl)))
      {
        templateSelectDialog.DialogTitle = "New Loan";
        if (templateSelectDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return false;
        FileSystemEntry selectedItem = templateSelectDialog.SelectedItem;
        if (selectedItem != null)
          loanTemplate = new LoanTemplateSelection(selectedItem, templateSelectDialog.AppendData);
      }
      return this.OpenNewLoan((string) null, loanFolder, loanTemplate, displayEditor);
    }

    public bool OpenNewLoan(LoanTemplateSelection loanTemplate)
    {
      return this.OpenNewLoan((string) null, (string) null, loanTemplate);
    }

    public bool OpenNewLoan(string loanName, LoanTemplateSelection loanTemplate)
    {
      return this.OpenNewLoan(loanName, (string) null, loanTemplate);
    }

    public bool OpenNewLoan(string loanName, string loanFolder, LoanTemplateSelection loanTemplate)
    {
      return this.OpenNewLoan(loanName, loanFolder, loanTemplate, true);
    }

    public bool OpenNewLoan(
      string loanName,
      string loanFolder,
      LoanTemplateSelection loanTemplate,
      bool displayEditor)
    {
      if ((loanFolder ?? "") == "")
        loanFolder = Session.UserInfo.WorkingFolder;
      if (!((LoanFolderRuleManager) Session.BPM.GetBpmManager(BpmCategory.LoanFolder)).IsActionPermitted(loanFolder, LoanFolderAction.Originate))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You are not authorized to originate loans in the current folder.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (Session.LoanManager.GetLoanFolder(loanFolder) == null)
        throw new ArgumentException("Specified loan folder does not exist or is not accessible");
      Cursor.Current = Cursors.WaitCursor;
      LoanDataMgr dataMgr = LoanDataMgr.NewLoan(Session.SessionObjects, loanTemplate, loanFolder, loanName ?? "");
      dataMgr.VerifyAssignFirstMilestoneRole();
      dataMgr.Calculator.PerformanceEnabled = true;
      if (dataMgr.SystemConfiguration != null && dataMgr.SystemConfiguration.LoanOfficerCompensationSetting != null)
        LOCompensationInputHandler.CheckLOCompRuleConfliction(dataMgr.SystemConfiguration.LoanOfficerCompensationSetting, (IHtmlInput) dataMgr.LoanData, (string) null, (string) null, (string) null, true);
      return this.OpenLoan(dataMgr, displayEditor);
    }

    private LoanDataMgr openLoanDataMgr(string guid) => this.openLoanDataMgr(guid, 0);

    private LoanDataMgr openLoanDataMgr(string guid, int sqlRead, bool lockImmediately = false)
    {
      try
      {
        LoanDataMgr loanDataMgr = (LoanDataMgr) null;
        MetricsFactory.IncrementCounter("LoanOpenIncCounter", (SFxTag) new SFxInternalTag());
        using (MetricsFactory.GetIncrementalTimer("LoanOpenIncTimer", (SFxTag) new SFxInternalTag()))
          loanDataMgr = LoanDataMgr.OpenLoan(Session.SessionObjects, guid, false, sqlRead, immediateLockType: lockImmediately ? LoanDataMgr.ImmediateExclusiveLockType.NonExclusive : LoanDataMgr.ImmediateExclusiveLockType.NoLock);
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
        this.loanOpenCount = 0;
        return loanDataMgr;
      }
      catch (ObjectNotFoundException ex)
      {
        string str = Session.LoanManager.SyncLoanFolder(guid, false);
        if (!string.IsNullOrEmpty(str) && this.loanOpenCount < 1)
        {
          ++this.loanOpenCount;
          MetricsFactory.IncrementErrorCounter((Exception) ex, str, "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs", nameof (openLoanDataMgr), 3758);
          int num = (int) Utils.Dialog((IWin32Window) this, str, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return this.openLoanDataMgr(guid, sqlRead);
        }
        MetricsFactory.IncrementErrorCounter((Exception) ex, "The selected loan is no longer accessible.", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainScreen.cs", nameof (openLoanDataMgr), 3765);
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The selected loan is no longer accessible.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.loanOpenCount = 0;
        return (LoanDataMgr) null;
      }
      catch (EllieMae.EMLite.ClientServer.Exceptions.SecurityException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The selected loan cannot be opened because you do not have the necessary permissions.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return (LoanDataMgr) null;
      }
      catch (LockException ex)
      {
        throw ex;
      }
      catch (Exception ex)
      {
        this.HandleFileSizeLimitException(ex);
        return (LoanDataMgr) null;
      }
    }

    public bool HandleFileSizeLimitException(Exception ex)
    {
      bool flag = ex.FindType<FileSizeLimitExceededException>() != null;
      if (flag)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Loan file size exceeded the limit set for the server.  Please contact administrator for assistance.");
      }
      return flag;
    }

    public void ShowHelp(Control parent)
    {
      string helpTargetName = "";
      if (Form.ActiveForm != MainForm.Instance)
      {
        if (Form.ActiveForm is IOnlineHelpTarget)
        {
          helpTargetName = ((IOnlineHelpTarget) Form.ActiveForm).GetHelpTargetName();
        }
        else
        {
          foreach (Control control in (ArrangedElementCollection) this.tabControl.SelectedTab.Controls)
          {
            if (control is IOnlineHelpTarget)
            {
              helpTargetName = ((IOnlineHelpTarget) control).GetHelpTargetName();
              break;
            }
          }
        }
      }
      else
      {
        foreach (Control control in (ArrangedElementCollection) this.tabControl.SelectedTab.Controls)
        {
          if (control is IOnlineHelpTarget)
          {
            helpTargetName = ((IOnlineHelpTarget) control).GetHelpTargetName();
            break;
          }
        }
      }
      this.ShowHelp(parent, helpTargetName);
    }

    public void ShowHelp(Control parent, string helpTargetName)
    {
      JedHelp.ShowHelp(parent, SystemSettings.HelpFile, helpTargetName);
    }

    public string[] CommandLineArguments => MainForm.ApplicationArgs;

    internal void MainMenuClick(
      string item,
      GradientMenuStrip mainMenu,
      ToolStripMenuItem helpMenuItem)
    {
      switch (item)
      {
        case "Borrower Contacts":
          this.activateTabPage(MainScreen.TabPageEnum.Contacts);
          this.contactPage.ShowContent(ContactMainForm.ContactsContentEnum.BorrowerContacts);
          break;
        case "Business Contacts":
          this.activateTabPage(MainScreen.TabPageEnum.Contacts);
          this.contactPage.ShowContent(ContactMainForm.ContactsContentEnum.BusinessContacts);
          break;
        case "Calculator":
          this.OpenCalculator();
          break;
        case "Calendar":
          this.activateTabPage(MainScreen.TabPageEnum.Contacts);
          this.contactPage.ShowContent(ContactMainForm.ContactsContentEnum.Calendar);
          break;
        case "Campaigns":
          this.activateTabPage(MainScreen.TabPageEnum.Contacts);
          this.contactPage.ShowContent(ContactMainForm.ContactsContentEnum.Campaigns);
          break;
        case "Dashboard":
          this.activateTabPage(MainScreen.TabPageEnum.Dashboard);
          break;
        case "Exit":
          this.CloseApplication();
          break;
        case "Home":
          this.activateTabPage(MainScreen.TabPageEnum.Home);
          break;
        case "Instant Messenger":
          if (MainScreen.IMEnabled)
          {
            EncompassMessenger.Start((IMainScreen) this);
            break;
          }
          int num1 = (int) Utils.Dialog((IWin32Window) this, "You don't have right to use Instant Messanger.");
          break;
        case "Loan":
          this.activateTabPage(MainScreen.TabPageEnum.Loan);
          break;
        case "Loan Mailbox":
          this.OpenLoanMailbox();
          break;
        case "Loan Search":
          TradeManagementConsole.Instance.SetCurrentScreen(TradeManagementScreen.Search);
          this.activateTabPage(MainScreen.TabPageEnum.Trades);
          break;
        case "Loan Trades":
          TradeManagementConsole.Instance.SetCurrentScreen(TradeManagementScreen.Trades);
          this.activateTabPage(MainScreen.TabPageEnum.Trades);
          break;
        case "Master Contracts":
          TradeManagementConsole.Instance.SetCurrentScreen(TradeManagementScreen.Contracts);
          this.activateTabPage(MainScreen.TabPageEnum.Trades);
          break;
        case "Mbs Pools":
          TradeManagementConsole.Instance.SetCurrentScreen(TradeManagementScreen.MbsPools);
          this.activateTabPage(MainScreen.TabPageEnum.Trades);
          break;
        case "Pipeline":
          this.activateTabPage(MainScreen.TabPageEnum.Pipeline);
          break;
        case "Reports":
          this.activateTabPage(MainScreen.TabPageEnum.Reports);
          break;
        case "Security Trades":
          TradeManagementConsole.Instance.SetCurrentScreen(TradeManagementScreen.SecurityTrades);
          this.activateTabPage(MainScreen.TabPageEnum.Trades);
          break;
        case "Services View":
          this.activateTabPage(MainScreen.TabPageEnum.ePASS);
          break;
        case "Settings":
          if (this.setupPanel == null)
          {
            this.setupPanel = new SetUpContainer((Form) MainForm.Instance, (IMainScreen) MainScreen.instance, mainMenu, helpMenuItem, Session.DefaultInstance);
            Session.Setup = (ISetup) this.setupPanel;
          }
          this.setupPanel.Width = (int) ((double) this.Width * 0.95);
          this.setupPanel.Height = (int) ((double) this.Height * 0.95);
          int num2 = (int) this.setupPanel.ShowDialog((IWin32Window) this);
          break;
        case "Tasks":
          this.activateTabPage(MainScreen.TabPageEnum.Contacts);
          this.contactPage.ShowContent(ContactMainForm.ContactsContentEnum.Tasks);
          break;
        case "eFolder":
          this.OpenLoanPage.ToolsMenuClick("Document Tracking");
          break;
        default:
          int num3 = (int) MessageBox.Show("Unknown menu item " + item);
          break;
      }
    }

    public void CloseApplication() => MainForm.Instance.Close();

    internal bool IsMenuItemEnabled(string item)
    {
      bool flag = true;
      switch (item)
      {
        case "Borrower Contacts":
          flag = this.tabControl.TabPages.Contains(this.contactTabPage) && this.contactPage.IsMenuItemVisible(ContactMainForm.ContactsActionEnum.Borrower_Access);
          break;
        case "Business Contacts":
          flag = this.tabControl.TabPages.Contains(this.contactTabPage) && this.contactPage.IsMenuItemVisible(ContactMainForm.ContactsActionEnum.Biz_Access);
          break;
        case "Campaigns":
          flag = this.tabControl.TabPages.Contains(this.contactTabPage) && this.contactPage.IsMenuItemVisible(ContactMainForm.ContactsActionEnum.Campaign_Access);
          break;
        case "Contacts":
          flag = this.tabControl.TabPages.Contains(this.contactTabPage);
          break;
        case "Dashboard":
          flag = this.tabControl.TabPages.Contains(this.dashboardTabPage);
          break;
        case "Home":
          flag = this.tabControl.TabPages.Contains(this.homeTabPage);
          break;
        case "Loan":
        case "eFolder":
          flag = this.tabControl.TabPages.Contains(this.loanTabPage);
          break;
        case "Pipeline":
          flag = this.tabControl.TabPages.Contains(this.pipelineTabPage);
          break;
        case "Print":
          if (this.tabControl.SelectedTab.Controls.Count > 0)
          {
            Control control = this.tabControl.SelectedTab.Controls[0];
            flag = control is IPrint;
            switch (control)
            {
              case ContactMainForm _:
                flag = ((ContactMainForm) control).IsPrintEnabled;
                break;
              case LoanPage _:
                flag = ((LoanPage) control).IsPrintEnabled;
                break;
            }
          }
          else
            break;
          break;
        case "Reports":
          flag = this.tabControl.TabPages.Contains(this.reportsTabPage);
          break;
        case "Save":
          flag = this.currentTabPage == this.loanTabPage && this.loanPage.saveBtn.Enabled;
          break;
        case "Services View":
          flag = this.tabControl.TabPages.Contains(this.ePASSTabPage);
          break;
        case "Trades":
          flag = this.tabControl.TabPages.Contains(this.tradesTabPage);
          break;
      }
      return flag;
    }

    public void SwitchToOrgUserSetup(string userid) => this.setupPanel.ShowOrgUserSetupPage(userid);

    public void SwitchToExternalOrgUserSetup(string userid)
    {
      this.setupPanel.ShowExternalUserSetupPage(userid);
    }

    private void onLoanDataModified(object sender, EventArgs e)
    {
      if (Session.LoanData == null || !Session.LoanData.Dirty)
        return;
      MainForm.Instance.SetLastSaveTimeDirtyFlag();
    }

    public bool RemoveLinkedLoan()
    {
      try
      {
        Session.LoanDataMgr.Unlink();
      }
      catch (Exception ex)
      {
        Tracing.Log(MainScreen.sw, nameof (MainScreen), TraceLevel.Error, "RemoveLinkedLoan: " + (object) ex);
        return false;
      }
      return true;
    }

    public bool AllowCalendarSharing()
    {
      bool flag = false;
      if (!Session.Connection.IsServerInProcess && EnableDisableSetting.Enabled == (EnableDisableSetting) Session.GetComponentSetting("CS", (object) EnableDisableSetting.Disabled))
        flag = true;
      return flag;
    }

    public void NavigateHome(string url)
    {
      this.SetCurrentActivity(EncompassActivity.Home);
      this.homeControl.Navigate(url);
    }

    public void NavigateToContact(CategoryType contactType)
    {
      if (contactType == CategoryType.Borrower)
        this.SetCurrentActivity(EncompassActivity.BorrowerContacts);
      else
        this.SetCurrentActivity(EncompassActivity.BusinessContacts);
    }

    public void NavigateToContact(ContactInfo selectedContact)
    {
      if (selectedContact == null)
        return;
      this.NavigateToContact(selectedContact.ContactType);
      this.contactPage.GotoContact(selectedContact);
    }

    public void NavigateToTradesTab(int tradeId)
    {
      this.SetCurrentActivity(EncompassActivity.Trades);
    }

    public void AddNewBorrowerToContactManagerList(int contactID)
    {
      this.contactPage.AddBorrowerContactToList(contactID);
    }

    public bool IsClientEnabledToExportFNMFRE
    {
      get
      {
        try
        {
          ServiceSetting serviceSettingFromId = ServicesMapping.GetServiceSettingFromID("Fannie");
          return serviceSettingFromId != null && Session.Application.GetService<ILoanServices>().IsExportServiceAccessible(Session.LoanDataMgr, serviceSettingFromId);
        }
        catch (Exception ex)
        {
          Tracing.Log(MainScreen.sw, nameof (MainScreen), TraceLevel.Error, "Cannot get ULDD client list allowable list. Error: " + ex.Message);
        }
        return false;
      }
    }

    public bool IsUnderwriterSummaryAccessibleForBroker
    {
      get
      {
        string str1 = string.Empty;
        try
        {
          str1 = SmartClientUtils.GetAttribute(Session.CompanyInfo.ClientID, "Encompass.exe", "UnderwriterSummaryBrokerAccessible");
        }
        catch (Exception ex)
        {
        }
        if (str1 == "1")
          return true;
        try
        {
          RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass");
          if (registryKey != null)
          {
            string str2 = (string) registryKey.GetValue("UnderwriterSummaryBrokerAccessible");
            if (!string.IsNullOrEmpty(str2))
            {
              if (str2.Trim() == "1")
                return true;
            }
          }
        }
        catch
        {
        }
        return false;
      }
    }

    public void DisplayTPOCompanySetting(ExternalOriginatorManagementData o)
    {
      using (EditCompanyDetailsDialog companyDetailsDialog = new EditCompanyDetailsDialog(Session.DefaultInstance, o))
      {
        int num = (int) companyDetailsDialog.ShowDialog((IWin32Window) Session.MainScreen);
      }
    }

    public void Enable() => this.Enabled = true;

    public void Disable() => this.Enabled = false;

    public string[] GetScreenNames(bool recursive)
    {
      List<string> stringList = new List<string>();
      foreach (TabPage tabPage in this.tabControl.TabPages)
        stringList.Add(tabPage.Tag.ToString());
      return stringList.ToArray();
    }

    public IApplicationScreen GetScreen(string name)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public IApplicationScreen GetCurrentScreen() => (IApplicationScreen) null;

    public bool SetCurrentScreen(string name)
    {
      foreach (TabPage tabPage in this.tabControl.TabPages)
      {
        if (string.Concat(tabPage.Tag) == name && tabPage != this.tabControl.SelectedTab)
        {
          this.tabControl.SelectedTab = tabPage;
          if (tabPage.Controls[0] is IApplicationScreen control)
            control.RefreshContents();
          return true;
        }
      }
      return false;
    }

    public void RegisterService(object service, System.Type serviceType)
    {
      this.services[serviceType] = service;
    }

    public T GetService<T>()
    {
      if (typeof (T) == typeof (IPipeline))
        this.loadPipelineScreen(1);
      else if (typeof (T) == typeof (IReportConsole))
        this.loadReportScreen();
      else if (typeof (T) == typeof (ITaskList) && this.contactPage != null)
        this.contactPage.LoadTasksScreen();
      else if (typeof (T) == typeof (ICampaigns) && this.contactPage != null)
        this.contactPage.LoadCampaignScreen();
      else if (typeof (T) == typeof (IBorrowerContacts) && this.contactPage != null)
        this.contactPage.LoadBorrowerScreen();
      else if (typeof (T) == typeof (IBizContacts) && this.contactPage != null)
        this.contactPage.LoadBizPartnerScreen();
      return this.services.ContainsKey(typeof (T)) ? (T) this.services[typeof (T)] : default (T);
    }

    public EncompassActivity GetCurrentActivity()
    {
      return (EncompassActivity) Enum.Parse(typeof (EncompassActivity), !(this.tabControl.SelectedTab.Text == "Contacts") ? this.tabControl.SelectedTab.Tag.ToString() : this.contactPage.GetCurrentScreen());
    }

    public bool SetCurrentActivity(EncompassActivity activity)
    {
      foreach (TabPage tabPage in this.tabControl.TabPages)
      {
        if (tabPage.Tag.ToString() == activity.ToString())
        {
          this.tabControl.SelectedTab = tabPage;
          return true;
        }
      }
      if (this.contactPage == null || !this.contactPage.SetCurrentScreen(activity.ToString()))
        return false;
      this.tabControl.SelectedTab = this.contactTabPage;
      this.contactPage.SetCurrentScreen(activity.ToString());
      return true;
    }

    public void SetMenu(string menuName) => MainForm.Instance.SetMenu(menuName);

    public ToolStripDropDown HelpDropDown => MainForm.Instance.HelpDropDown;

    public bool ContainsBrowserWindow(string browserWindowName)
    {
      return this.browserWindows.ContainsKey((object) browserWindowName);
    }

    public Form GetBrowserWindow(string browserWindowName)
    {
      return this.browserWindows.ContainsKey((object) browserWindowName) ? (Form) this.browserWindows[(object) browserWindowName] : (Form) null;
    }

    public void AddBrowserWindow(string browserWindowName, Form browserWindow)
    {
      this.browserWindows.Add((object) browserWindowName, (object) browserWindow);
    }

    public void RemoveBrowserWindow(string browserWindowName)
    {
      this.browserWindows.Remove((object) browserWindowName);
    }

    public void Navigate(string target, string type)
    {
      try
      {
        if (!(target.ToLower() == "trade"))
          return;
        string[] strArray = type.Split('/');
        int tradeId = Utils.ParseInt((object) strArray[1], 0);
        string tradeType = strArray[0];
        if (!this.InvokeRequired)
          return;
        this.Invoke((Delegate) (() =>
        {
          this.NavigateToTradesTab(tradeId);
          if (tradeType.Equals(TradeType.LoanTrade.ToString(), StringComparison.OrdinalIgnoreCase))
          {
            TradeManagementConsole.Instance.OpenTrade(tradeId, true);
          }
          else
          {
            string str1 = tradeType;
            TradeType tradeType1 = TradeType.MbsPool;
            string str2 = tradeType1.ToString();
            if (str1.Equals(str2, StringComparison.OrdinalIgnoreCase))
            {
              TradeManagementConsole.Instance.OpenMbsPool(tradeId, true);
            }
            else
            {
              string str3 = tradeType;
              tradeType1 = TradeType.CorrespondentTrade;
              string str4 = tradeType1.ToString();
              if (!str3.Equals(str4, StringComparison.OrdinalIgnoreCase))
                throw new Exception(string.Format("Trade type {0} is not suppored", (object) tradeType));
              TradeManagementConsole.Instance.OpenCorrespondentTrade(tradeId, true);
            }
          }
        }));
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Open trade failed due to {0}", (object) ex.Message));
      }
    }

    [SpecialName]
    string IApplicationScreen.get_Name() => this.Name;

    public enum TabPageEnum
    {
      Home,
      Pipeline,
      Loan,
      Trades,
      Contacts,
      Dashboard,
      Reports,
      ePASS,
    }

    private enum helpQualifier
    {
      tabName,
      treeView,
    }

    private delegate void ShowCalendarDelegate(
      IWin32Window owner,
      string userID,
      CSMessage.AccessLevel accessLevel,
      bool accessUpdate);

    private delegate bool OpenLoanDelegate(string guid, LoanInfo.LockReason reason);

    private enum LoanAccessResult
    {
      ReadOnly,
      UserRefusedToProceed,
      Success,
    }
  }
}
