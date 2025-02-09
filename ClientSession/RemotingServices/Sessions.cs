// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.Sessions
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using Elli.Metrics.Client;
using Elli.Server.Remoting;
using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Cache;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.ClientServer.Interfaces;
using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.RemotingServices.Acl;
using EllieMae.EMLite.RemotingServices.Bpm;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.WebServices;
using EllieMae.Encompass.AsmResolver;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.Services.Protocols;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class Sessions
  {
    private const string className = "Sessions";
    private static readonly string sw = Tracing.SwRemoting;
    private static Hashtable sessions = CollectionsUtil.CreateCaseInsensitiveHashtable();

    public static void AddSession(Sessions.Session session, bool throwExceptionOnExist)
    {
      lock (Sessions.sessions)
      {
        if (throwExceptionOnExist && Sessions.sessions.ContainsKey((object) session.SID))
          throw new Exception("SID '" + session.SID + "' already exists");
        Sessions.sessions[(object) session.SID] = (object) session;
      }
    }

    public static Sessions.Session GetSession(string sid, bool createIfNotExist)
    {
      lock (Sessions.sessions)
      {
        if (Sessions.sessions.ContainsKey((object) sid))
          return (Sessions.Session) Sessions.sessions[(object) sid];
        if (!createIfNotExist)
          return (Sessions.Session) null;
        Sessions.Session session = new Sessions.Session(sid);
        Sessions.sessions.Add((object) sid, (object) session);
        return session;
      }
    }

    public static Sessions.Session GetSession(string sid)
    {
      return sid != null ? Sessions.GetSession(sid, false) : throw new Exception("SID cannot be null");
    }

    public static Sessions.Session GetSession(int sid)
    {
      return Sessions.GetSession(string.Concat((object) sid));
    }

    internal static Sessions.Session GetSession() => Sessions.GetSession("");

    internal static Sessions.Session GetSession(bool createIfNotExist)
    {
      return Sessions.GetSession("", createIfNotExist);
    }

    public static void EndAll()
    {
      lock (Sessions.sessions)
      {
        foreach (string key in (IEnumerable) Sessions.sessions.Keys)
          ((Sessions.Session) Sessions.sessions[(object) key]).End();
        Sessions.sessions.Clear();
      }
    }

    public class Session
    {
      private const string className = "Sessions.Session";
      private static readonly string sw = Tracing.SwRemoting;
      private LoanDataMgr loanDataMgr;
      private IApplicationScreen appScreen;
      private Hashtable dataServicesData;
      public bool ExitOnDisconnect = true;
      private IConnection conn;
      private string remoteServer;
      private ServerIdentity serverIdentity;
      private ISessionStartupInfo startupInfo;
      private SessionObjects sessionObjects;
      private bool isDisconnecting;
      public Hashtable StandardFormCodebaseTypes = new Hashtable((IEqualityComparer) StringComparer.InvariantCultureIgnoreCase);
      internal readonly string SID;
      private FeaturesAclManager featuresAclManager;
      private FeatureConfigsAclManager featureConfigsAclManager;
      private LoanDuplicationAclManager loanDuplicationAclManager;
      private InvestorServicesAclManager investorServiceAclManager;
      private EnhancedConditionsAclManager enhancedConditionsAclManager;
      private LoConnectServiceAclManager loConnectServiceAclManager;
      private StandardWebFormAclManager standardWebFormAclManager;
      private LoanFoldersAclManager loanFoldersAclManager;
      private DDMFieldRulesBpmManager ddmFieldRulesManager;
      private DDMFeeRulesBpmManager ddmFeeRulesManager;
      private FieldAccessAclManager fieldAccessAclManager;
      private PipelineViewAclManager pipelineViewAclManager;
      private ServicesAclManager servicesAclManager;
      private ExportServicesAclManager exportServicesAclManager;
      private MilestonesAclManager milestonesAclManager;
      private MilestoneFreeRoleAclManager milestoneFreeRoleAclManager;
      private InputFormsAclManager inputFormsAclManager;
      private ToolsAclManager toolsAclManager;
      private WorkflowManager workflowManager;
      private LoanFolderRuleManager loanFolderRuleManager;
      private MilestoneRulesBpmManager milestoneRulesBpmManager;
      private LoanActionCompletionRulesBpmManager loanActionCompletionRulesBpmManager;
      private FieldRulesBpmManager fieldRulesBpmManager;
      private AutoLockExclusionRulesBpmManager autoLockExclusionRulesBpmManager;
      private LoanAccessBpmManager loanAccessBpmManager;
      private LoanActionAccessBpmManager loanActionAccessBpmManager;
      private FieldAccessBpmManager fieldAccessBpmManager;
      private InputFormsBpmManager inputFormsBpmManager;
      private DocumentAccessRuleManager documentAccessRuleManager;
      private TriggersBpmManager triggersBpmManager;
      private AutomatedConditionBpmManager automatedConditionsBpmManager;
      private AutomatedEnhancedConditionBpmManager automatedEnhancedConditionsBpmManager;
      private AutomatedPurchaseConditionBpmManager automatedPurchaseConditionsBpmManager;
      private PrintFormsBpmManager printFormsBpmManager;
      private PrintSelectionBpmManager printSelectionBpmManager;
      private MilestoneTemplatesBpmManager milestoneTemplatesBpmManager;
      private DDMFeeRuleScenariosBpmManager ddmFeeRuleScenariosBpmManager;
      private DDMFieldRuleScenariosBpmManager ddmFieldRuleScenariosBpmManager;
      private DDMDataPopTimingBpmManager ddmDataPopTimingBpmManager;
      private DDMDataTableBpmManager ddmDataTableBpmManager;
      private DDMDataTableFieldBpmManager ddmDataTableFieldBpmManager;
      private TemporaryBuydownTypeBpmManager tmpBuydownTypeBpmManager;
      private TimeSpan serverClientTimeDiff = new TimeSpan(0L);
      public List<DDMFeeRule> feerule;
      public DDMDataPopulationTiming dataPopTiming;
      private SettingsManager settingsManager;
      private ACL acl;
      private BPM bpm;
      private Form mainForm;
      private IMainScreen mainScreen;
      private ISetup setup;

      public event EventHandler Started;

      public event EventHandler Ended;

      public event EventHandler ApplicationReady;

      public event EventHandler LoanOpened;

      public event EventHandler LoanClosing;

      public event EventHandler FormLoaded;

      public event EventHandler FormUnloading;

      public event CacheControlEventHandler CacheControl;

      public event ServerSessionEventHandler ServerSessionEvent;

      public string SessionID => this.sessionObjects.SessionID;

      public FeaturesAclManager FeaturesAclManager => this.featuresAclManager;

      public void ResetFeaturesAclManager() => this.featuresAclManager = (FeaturesAclManager) null;

      public FeatureConfigsAclManager FeatureConfigsAclManager => this.featureConfigsAclManager;

      public void ResetFeatureConfigsAclManager()
      {
        this.featureConfigsAclManager = (FeatureConfigsAclManager) null;
      }

      public LoanDuplicationAclManager LoanDuplicationAclManager
      {
        get
        {
          if (this.loanDuplicationAclManager == null)
            this.loanDuplicationAclManager = new LoanDuplicationAclManager(this);
          return this.loanDuplicationAclManager;
        }
      }

      public InvestorServicesAclManager InvestorServieAclManager
      {
        get
        {
          if (this.investorServiceAclManager == null)
            this.investorServiceAclManager = new InvestorServicesAclManager(this);
          return this.investorServiceAclManager;
        }
      }

      public EnhancedConditionsAclManager EnhancedConditionAclManager
      {
        get
        {
          if (this.enhancedConditionsAclManager == null)
            this.enhancedConditionsAclManager = new EnhancedConditionsAclManager(this);
          return this.enhancedConditionsAclManager;
        }
      }

      public LoConnectServiceAclManager LoConnectServicesAclManager
      {
        get
        {
          if (this.loConnectServiceAclManager == null)
            this.loConnectServiceAclManager = new LoConnectServiceAclManager(this);
          return this.loConnectServiceAclManager;
        }
      }

      public StandardWebFormAclManager StandardWebFormAclManager
      {
        get
        {
          if (this.standardWebFormAclManager == null)
            this.standardWebFormAclManager = new StandardWebFormAclManager(this);
          return this.standardWebFormAclManager;
        }
      }

      public void ResetInvestorServieAclManager()
      {
        this.investorServiceAclManager = (InvestorServicesAclManager) null;
      }

      public LoanFoldersAclManager LoanFoldersAclManager
      {
        get
        {
          if (this.loanFoldersAclManager == null)
            this.loanFoldersAclManager = new LoanFoldersAclManager(this);
          return this.loanFoldersAclManager;
        }
      }

      public void ResetLoanFoldersAclManager()
      {
        this.loanFoldersAclManager = (LoanFoldersAclManager) null;
      }

      public DDMFieldRulesBpmManager DDMFieldRulesBpmManager
      {
        get
        {
          if (this.ddmFieldRulesManager == null)
            this.ddmFieldRulesManager = new DDMFieldRulesBpmManager(this);
          return this.ddmFieldRulesManager;
        }
      }

      public void ResetDDMFieldRulesBpmManager()
      {
        this.ddmFieldRulesManager = (DDMFieldRulesBpmManager) null;
      }

      public DDMFeeRulesBpmManager DDMFeeRulesBpmManager
      {
        get
        {
          if (this.ddmFeeRulesManager == null)
            this.ddmFeeRulesManager = new DDMFeeRulesBpmManager(this);
          return this.ddmFeeRulesManager;
        }
      }

      public void ResetDDMFeeRulesBpmManager()
      {
        this.ddmFeeRulesManager = (DDMFeeRulesBpmManager) null;
      }

      public FieldAccessAclManager FieldAccessAclManager
      {
        get
        {
          if (this.fieldAccessAclManager == null)
            this.fieldAccessAclManager = new FieldAccessAclManager(this);
          return this.fieldAccessAclManager;
        }
      }

      public void ResetFieldAccessAclManager()
      {
        this.fieldAccessAclManager = (FieldAccessAclManager) null;
      }

      public PipelineViewAclManager PipelineViewAclManager
      {
        get
        {
          if (this.pipelineViewAclManager == null)
            this.pipelineViewAclManager = new PipelineViewAclManager(this);
          return this.pipelineViewAclManager;
        }
      }

      public void ResetPipelineViewAclManager()
      {
        this.pipelineViewAclManager = (PipelineViewAclManager) null;
      }

      public ServicesAclManager ServicesAclManager
      {
        get
        {
          if (this.servicesAclManager == null)
            this.servicesAclManager = new ServicesAclManager(this);
          return this.servicesAclManager;
        }
      }

      public void ResetServicesAclManager() => this.servicesAclManager = (ServicesAclManager) null;

      public ExportServicesAclManager ExportServicesAclManager
      {
        get
        {
          if (this.exportServicesAclManager == null)
            this.exportServicesAclManager = new ExportServicesAclManager(this);
          return this.exportServicesAclManager;
        }
      }

      public void ResetExportServicesAclManager()
      {
        this.exportServicesAclManager = (ExportServicesAclManager) null;
      }

      public MilestonesAclManager MilestonesAclManager
      {
        get
        {
          if (this.milestonesAclManager == null)
            this.milestonesAclManager = new MilestonesAclManager(this);
          return this.milestonesAclManager;
        }
      }

      public void ResetMilestonesAclManager()
      {
        this.milestonesAclManager = (MilestonesAclManager) null;
      }

      public MilestoneFreeRoleAclManager MilestoneFreeRoleAclManager
      {
        get
        {
          if (this.milestoneFreeRoleAclManager == null)
            this.milestoneFreeRoleAclManager = new MilestoneFreeRoleAclManager(this);
          return this.milestoneFreeRoleAclManager;
        }
      }

      public void ResetMilestoneFreeRoleAclManager()
      {
        this.milestoneFreeRoleAclManager = (MilestoneFreeRoleAclManager) null;
      }

      public InputFormsAclManager InputFormsAclManager
      {
        get
        {
          if (this.inputFormsAclManager == null)
            this.inputFormsAclManager = new InputFormsAclManager(this);
          return this.inputFormsAclManager;
        }
      }

      public void ResetInputFormsAclManager()
      {
        this.inputFormsAclManager = (InputFormsAclManager) null;
      }

      public ToolsAclManager ToolsAclManager
      {
        get
        {
          if (this.toolsAclManager == null)
            this.toolsAclManager = new ToolsAclManager(this);
          return this.toolsAclManager;
        }
      }

      public void ResetToolsAclManager() => this.toolsAclManager = (ToolsAclManager) null;

      public WorkflowManager WorkflowManager
      {
        get
        {
          if (this.workflowManager == null)
            this.workflowManager = new WorkflowManager(this);
          return this.workflowManager;
        }
      }

      public void ResetWorkflowManager() => this.workflowManager = (WorkflowManager) null;

      public LoanFolderRuleManager LoanFolderRuleManager
      {
        get
        {
          if (this.loanFolderRuleManager == null)
            this.loanFolderRuleManager = new LoanFolderRuleManager(this);
          return this.loanFolderRuleManager;
        }
      }

      public void ResetLoanFolderRuleManager()
      {
        this.loanFolderRuleManager = (LoanFolderRuleManager) null;
      }

      public MilestoneRulesBpmManager MilestoneRulesBpmManager
      {
        get
        {
          if (this.milestoneRulesBpmManager == null)
            this.milestoneRulesBpmManager = new MilestoneRulesBpmManager(this);
          return this.milestoneRulesBpmManager;
        }
      }

      public void ResetMilestoneRulesBpmManager()
      {
        this.milestoneRulesBpmManager = (MilestoneRulesBpmManager) null;
      }

      public LoanActionCompletionRulesBpmManager LoanActionCompletionRulesBpmManager
      {
        get
        {
          if (this.loanActionCompletionRulesBpmManager == null)
            this.loanActionCompletionRulesBpmManager = new LoanActionCompletionRulesBpmManager(this);
          return this.loanActionCompletionRulesBpmManager;
        }
      }

      public void ResetLoanActionCompletionRulesBpmManager()
      {
        this.loanActionCompletionRulesBpmManager = (LoanActionCompletionRulesBpmManager) null;
      }

      public FieldRulesBpmManager FieldRulesBpmManager
      {
        get
        {
          if (this.fieldRulesBpmManager == null)
            this.fieldRulesBpmManager = new FieldRulesBpmManager(this);
          return this.fieldRulesBpmManager;
        }
      }

      public void ResetFieldRulesBpmManager()
      {
        this.fieldRulesBpmManager = (FieldRulesBpmManager) null;
      }

      public AutoLockExclusionRulesBpmManager AutoLockExclusionRulesBpmManager
      {
        get
        {
          if (this.autoLockExclusionRulesBpmManager == null)
            this.autoLockExclusionRulesBpmManager = new AutoLockExclusionRulesBpmManager(this);
          return this.autoLockExclusionRulesBpmManager;
        }
      }

      public void ResetAutoLockExclusionRulesBpmManager()
      {
        this.autoLockExclusionRulesBpmManager = (AutoLockExclusionRulesBpmManager) null;
      }

      public LoanAccessBpmManager LoanAccessBpmManager
      {
        get
        {
          if (this.loanAccessBpmManager == null)
            this.loanAccessBpmManager = new LoanAccessBpmManager(this);
          return this.loanAccessBpmManager;
        }
      }

      public void ResetLoanAccessBpmManager()
      {
        this.loanAccessBpmManager = (LoanAccessBpmManager) null;
      }

      public LoanActionAccessBpmManager LoanActionAccessBpmManager
      {
        get
        {
          if (this.loanActionAccessBpmManager == null)
            this.loanActionAccessBpmManager = new LoanActionAccessBpmManager(this);
          return this.loanActionAccessBpmManager;
        }
      }

      public void ResetLoanActionAccessBpmManager()
      {
        this.loanActionAccessBpmManager = (LoanActionAccessBpmManager) null;
      }

      public FieldAccessBpmManager FieldAccessBpmManager
      {
        get
        {
          if (this.fieldAccessBpmManager == null)
            this.fieldAccessBpmManager = new FieldAccessBpmManager(this);
          return this.fieldAccessBpmManager;
        }
      }

      public void ResetFieldAccessBpmManager()
      {
        this.fieldAccessBpmManager = (FieldAccessBpmManager) null;
      }

      public InputFormsBpmManager InputFormsBpmManager
      {
        get
        {
          if (this.inputFormsBpmManager == null)
            this.inputFormsBpmManager = new InputFormsBpmManager(this);
          return this.inputFormsBpmManager;
        }
      }

      public void ResetInputFormsBpmManager()
      {
        this.inputFormsBpmManager = (InputFormsBpmManager) null;
      }

      public DocumentAccessRuleManager DocumentAccessRuleManager
      {
        get
        {
          if (this.documentAccessRuleManager == null)
            this.documentAccessRuleManager = new DocumentAccessRuleManager(this);
          return this.documentAccessRuleManager;
        }
      }

      public void ResetDocumentAccessRuleManager()
      {
        this.documentAccessRuleManager = (DocumentAccessRuleManager) null;
      }

      public TriggersBpmManager TriggersBpmManager
      {
        get
        {
          if (this.triggersBpmManager == null)
            this.triggersBpmManager = new TriggersBpmManager(this);
          return this.triggersBpmManager;
        }
      }

      public AutomatedConditionBpmManager AutomatedConditionsBpmManager
      {
        get
        {
          if (this.automatedConditionsBpmManager == null)
            this.automatedConditionsBpmManager = new AutomatedConditionBpmManager(this);
          return this.automatedConditionsBpmManager;
        }
      }

      public AutomatedEnhancedConditionBpmManager AutomatedEnhancedConditionsBpmManager
      {
        get
        {
          if (this.automatedEnhancedConditionsBpmManager == null)
            this.automatedEnhancedConditionsBpmManager = new AutomatedEnhancedConditionBpmManager(this);
          return this.automatedEnhancedConditionsBpmManager;
        }
      }

      public AutomatedPurchaseConditionBpmManager AutomatedPurchaseConditionsBpmManager
      {
        get
        {
          if (this.automatedPurchaseConditionsBpmManager == null)
            this.automatedPurchaseConditionsBpmManager = new AutomatedPurchaseConditionBpmManager(this);
          return this.automatedPurchaseConditionsBpmManager;
        }
      }

      public void ResetTriggersBpmManager() => this.triggersBpmManager = (TriggersBpmManager) null;

      public PrintFormsBpmManager PrintFormsBpmManager
      {
        get
        {
          if (this.printFormsBpmManager == null)
            this.printFormsBpmManager = new PrintFormsBpmManager(this);
          return this.printFormsBpmManager;
        }
      }

      public void ResetPrintFormsBpmManager()
      {
        this.printFormsBpmManager = (PrintFormsBpmManager) null;
      }

      public PrintSelectionBpmManager PrintSelectionBpmManager
      {
        get
        {
          if (this.printSelectionBpmManager == null)
            this.printSelectionBpmManager = new PrintSelectionBpmManager(this);
          return this.printSelectionBpmManager;
        }
      }

      public void ResetPrintSelectionBpmManager()
      {
        this.printSelectionBpmManager = (PrintSelectionBpmManager) null;
      }

      public MilestoneTemplatesBpmManager MilestoneTemplatesBpmManager
      {
        get
        {
          if (this.milestoneTemplatesBpmManager == null)
            this.milestoneTemplatesBpmManager = new MilestoneTemplatesBpmManager(this);
          return this.milestoneTemplatesBpmManager;
        }
      }

      public DDMFeeRuleScenariosBpmManager DDMFeeRuleScenariosBpmManager
      {
        get
        {
          if (this.ddmFeeRuleScenariosBpmManager == null)
            this.ddmFeeRuleScenariosBpmManager = new DDMFeeRuleScenariosBpmManager(this);
          return this.ddmFeeRuleScenariosBpmManager;
        }
      }

      public DDMFieldRuleScenariosBpmManager DDMFieldRuleScenariosBpmManager
      {
        get
        {
          if (this.ddmFieldRuleScenariosBpmManager == null)
            this.ddmFieldRuleScenariosBpmManager = new DDMFieldRuleScenariosBpmManager(this);
          return this.ddmFieldRuleScenariosBpmManager;
        }
      }

      public void ResetMilestoneTemplatesBpmManager()
      {
        this.milestoneTemplatesBpmManager = (MilestoneTemplatesBpmManager) null;
      }

      public void ResetAutomatedConditionsBpmManager()
      {
        this.automatedConditionsBpmManager = (AutomatedConditionBpmManager) null;
      }

      public void ResetAutomatedEnhancedConditionsBpmManager()
      {
        this.automatedEnhancedConditionsBpmManager = (AutomatedEnhancedConditionBpmManager) null;
      }

      public void ResetDDMFeeRuleScenariosBpmManager()
      {
        this.ddmFeeRuleScenariosBpmManager = (DDMFeeRuleScenariosBpmManager) null;
      }

      public void ResetDDMFieldRuleScenariosBpmManager()
      {
        this.ddmFieldRuleScenariosBpmManager = (DDMFieldRuleScenariosBpmManager) null;
      }

      public DDMDataPopTimingBpmManager DDMDataPopTimingBpmManager
      {
        get
        {
          if (this.ddmDataPopTimingBpmManager == null)
            this.ddmDataPopTimingBpmManager = new DDMDataPopTimingBpmManager(this);
          return this.ddmDataPopTimingBpmManager;
        }
      }

      public void ResetDDMDataPopTimingBpmManager()
      {
        this.ddmDataPopTimingBpmManager = (DDMDataPopTimingBpmManager) null;
      }

      public DDMDataTableBpmManager DDMDataTableBpmManager
      {
        get
        {
          if (this.ddmDataTableBpmManager == null)
            this.ddmDataTableBpmManager = new DDMDataTableBpmManager(this);
          return this.ddmDataTableBpmManager;
        }
      }

      public void ResetDDMDataTableBpmManager()
      {
        this.ddmDataTableBpmManager = (DDMDataTableBpmManager) null;
      }

      public DDMDataTableFieldBpmManager DDMDataTableFieldBpmManager
      {
        get
        {
          if (this.ddmDataTableFieldBpmManager == null)
            this.ddmDataTableFieldBpmManager = new DDMDataTableFieldBpmManager(this);
          return this.ddmDataTableFieldBpmManager;
        }
      }

      public void ResetDDMDataTableFieldBpmManager()
      {
        this.ddmDataTableFieldBpmManager = (DDMDataTableFieldBpmManager) null;
      }

      public TemporaryBuydownTypeBpmManager TemporaryBuydownTypeBpmManager
      {
        get
        {
          if (this.tmpBuydownTypeBpmManager == null)
            this.tmpBuydownTypeBpmManager = new TemporaryBuydownTypeBpmManager(this);
          return this.tmpBuydownTypeBpmManager;
        }
      }

      public Session(string sid)
      {
        this.SID = sid != null ? sid : throw new Exception("SID cannot be null");
      }

      public Session(int sid)
        : this(string.Concat((object) sid))
      {
      }

      internal Session()
        : this("")
      {
      }

      public ISession ISession => this.conn.Session;

      public void Start(string serverUri, string userId, string password, string appName)
      {
        this.Start(serverUri, userId, password, appName, (string) null);
      }

      public void Start(
        string serverUri,
        string userId,
        string password,
        string appName,
        string prevSessionID)
      {
        this.Start(serverUri, userId, password, appName, true, prevSessionID);
      }

      public void Start(
        string serverUri,
        string userId,
        string password,
        string appName,
        bool licenseRequired)
      {
        this.Start(serverUri, userId, password, appName, licenseRequired, (string) null);
      }

      public void Start(
        string serverUri,
        string userId,
        string password,
        string appName,
        bool licenseRequired,
        string prevSessionID,
        string authCode)
      {
        this.End();
        PerformanceMeter.Current.AddCheckpoint("Session.Start - End", 808, nameof (Start), "D:\\ws\\24.3.0.0\\EmLite\\ClientSession\\Sessions.cs");
        if (string.IsNullOrEmpty(password))
          password = this.getSavedPwd(serverUri, userId);
        Connection newConn = new Connection();
        newConn.Open(serverUri, userId, password, appName, licenseRequired, prevSessionID, authCode);
        newConn.ConnectionError += new ConnectionErrorEventHandler(this.handleConnectionError);
        PerformanceMeter.Current.AddCheckpoint("Session.Start - Setup Conn Error Event Handlers", 819, nameof (Start), "D:\\ws\\24.3.0.0\\EmLite\\ClientSession\\Sessions.cs");
        this.initializeSession((IConnection) newConn, password);
        PerformanceMeter.Current.AddCheckpoint("Session.Start - Initialize", 824, nameof (Start), "D:\\ws\\24.3.0.0\\EmLite\\ClientSession\\Sessions.cs");
      }

      public void Start(
        string serverUri,
        string userId,
        string password,
        string appName,
        bool licenseRequired,
        string prevSessionID)
      {
        this.End();
        PerformanceMeter.Current.AddCheckpoint("Session.Start - End", 831, nameof (Start), "D:\\ws\\24.3.0.0\\EmLite\\ClientSession\\Sessions.cs");
        if (string.IsNullOrEmpty(password))
          password = this.getSavedPwd(serverUri, userId);
        Connection newConn = new Connection();
        newConn.Open(serverUri, userId, password, appName, licenseRequired, prevSessionID);
        newConn.ConnectionError += new ConnectionErrorEventHandler(this.handleConnectionError);
        PerformanceMeter.Current.AddCheckpoint("Session.Start - Setup Conn Error Event Handlers", 842, nameof (Start), "D:\\ws\\24.3.0.0\\EmLite\\ClientSession\\Sessions.cs");
        this.initializeSession((IConnection) newConn, password);
        PerformanceMeter.Current.AddCheckpoint("Session.Start - Initialize", 847, nameof (Start), "D:\\ws\\24.3.0.0\\EmLite\\ClientSession\\Sessions.cs");
      }

      private string getSavedPwd(string serverUri, string userId)
      {
        try
        {
          using (RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\Ellie Mae\\Encompass"))
          {
            if (registryKey1 != null)
            {
              using (RegistryKey registryKey2 = registryKey1.OpenSubKey("Credentials"))
              {
                if (registryKey2 != null)
                {
                  string dd = (string) registryKey2.GetValue(serverUri + "#" + userId);
                  if (dd != null)
                    return XT.DSB64(dd, KB.KB64);
                }
              }
              string dd1 = (string) registryKey1.GetValue("Credentials");
              if (dd1 != null)
                return XT.DSB64(dd1, KB.KB64);
            }
          }
        }
        catch
        {
        }
        return "";
      }

      public void Start(string userId, string password, string appName)
      {
        this.Start(userId, password, appName, true);
      }

      public void Start(string userId, string password, string appName, bool licenseRequired)
      {
        this.End();
        if (string.IsNullOrEmpty(password))
          password = this.getSavedPwd("(local)", userId);
        InProcConnection newConn = new InProcConnection();
        newConn.OpenInProcess(userId, password, appName, licenseRequired);
        this.initializeSession((IConnection) newConn, password);
      }

      private void initializeSession(IConnection newConn, string password)
      {
        this.conn = newConn;
        this.isDisconnecting = false;
        newConn.ServerEvent += new ServerEventHandler(this.handleServerEvent);
        if (newConn.Server != null)
        {
          this.serverIdentity = newConn.Server;
          this.remoteServer = newConn.Server.ToString();
        }
        PerformanceMeter.Current.AddCheckpoint("Session.Start - initializeSession - Subscribe", 918, nameof (initializeSession), "D:\\ws\\24.3.0.0\\EmLite\\ClientSession\\Sessions.cs");
        this.startupInfo = this.conn.Session.GetSessionStartupInfo();
        this.sessionObjects = new SessionObjects(this.conn.Session, password, this.startupInfo);
        this.sessionObjects.Interactive = true;
        PerformanceMeter.Current.AddCheckpoint("Session.Start - initializeSession - Startup Info", 926, nameof (initializeSession), "D:\\ws\\24.3.0.0\\EmLite\\ClientSession\\Sessions.cs");
        this.BPM.PreloadRules(this.startupInfo.ActiveRules);
        this.BPM.PreloadMilestoneTemplateConditions(this.startupInfo.MilestoneTemplate);
        this.featuresAclManager = new FeaturesAclManager(this);
        this.featuresAclManager.CacheLoginUserAclRight(this.startupInfo.UserAclFeatureRights);
        this.featureConfigsAclManager = new FeatureConfigsAclManager(this);
        this.featureConfigsAclManager.CacheLoginUserAclRight(this.startupInfo.UserAclFeaturConfigRights);
        PerformanceMeter.Current.AddCheckpoint("Session.Start - initializeSession - Cache Data", 937, nameof (initializeSession), "D:\\ws\\24.3.0.0\\EmLite\\ClientSession\\Sessions.cs");
        if (this.Started != null)
          this.Started((object) null, EventArgs.Empty);
        if (this.SID == "")
          EllieMae.EMLite.RemotingServices.Session.OnStarted((object) null, EventArgs.Empty);
        PerformanceMeter.Current.AddCheckpoint("Session.Start - initializeSession - Notify Others", 945, nameof (initializeSession), "D:\\ws\\24.3.0.0\\EmLite\\ClientSession\\Sessions.cs");
      }

      public string EncompassSystemID => this.sessionObjects.SystemID;

      public string SqlDbID => this.sessionObjects.Session.SqlDbID;

      public void SetConnection(IConnection newConnection, string userPassword)
      {
        this.End();
        this.conn = newConnection;
        this.startupInfo = this.conn.Session.GetSessionStartupInfo();
        this.sessionObjects = new SessionObjects(this.conn.Session, userPassword, this.startupInfo);
        this.isDisconnecting = false;
      }

      public void End()
      {
        this.BPM.RefreshBpmManagerInstances();
        this.ACL.RefreshAclManagerInstances();
        if (this.conn == null)
          return;
        if (this.conn is Connection)
        {
          ((ConnectionBase) this.conn).ServerEvent -= new ServerEventHandler(this.handleServerEvent);
          ((ConnectionBase) this.conn).ConnectionError -= new ConnectionErrorEventHandler(this.handleConnectionError);
        }
        this.conn.Close();
        this.conn = (IConnection) null;
        this.serverIdentity = (ServerIdentity) null;
        this.remoteServer = (string) null;
        this.startupInfo = (ISessionStartupInfo) null;
        this.sessionObjects = (SessionObjects) null;
        MetricsFactory.EndSession();
        if (this.Ended != null)
          this.Ended((object) null, EventArgs.Empty);
        if (!(this.SID == ""))
          return;
        EllieMae.EMLite.RemotingServices.Session.OnEnded((object) null, EventArgs.Empty);
      }

      public bool IsConnected => this.conn != null;

      public bool UsingSso => this.sessionObjects.UsingSso;

      public string Password => this.sessionObjects.UserPassword;

      public IConnection Connection => this.conn;

      public DateTime ServerTime
      {
        get
        {
          try
          {
            DateTime serverTime = this.conn.Session.ServerTime;
            this.serverClientTimeDiff = serverTime - DateTime.Now;
            return serverTime;
          }
          catch
          {
            return DateTime.Now + this.serverClientTimeDiff;
          }
        }
      }

      public DateTime ServerRealTime
      {
        get
        {
          try
          {
            DateTime serverTime = this.conn.Session.ServerTime;
            this.serverClientTimeDiff = serverTime - DateTime.Now;
            return serverTime;
          }
          catch
          {
            return DateTime.MinValue;
          }
        }
      }

      public string ServerTimeZone => this.conn.Session.ServerTimeZone;

      public string UserID => this.sessionObjects.UserID;

      public UserInfo UserInfo => this.sessionObjects.UserInfo;

      public UserInfo RecacheUserInfo()
      {
        this.sessionObjects.InvalidateUserInfo();
        this.startupInfo.UserInfo = this.sessionObjects.UserInfo;
        return this.startupInfo.UserInfo;
      }

      public CompanyInfo CompanyInfo
      {
        get => this.sessionObjects == null ? (CompanyInfo) null : this.sessionObjects.CompanyInfo;
      }

      public CompanyInfo RecacheCompanyInfo()
      {
        this.sessionObjects.InvalidateCompanyInfo();
        this.startupInfo.CompanyInfo = this.sessionObjects.CompanyInfo;
        return this.startupInfo.CompanyInfo;
      }

      public Hashtable InitialDataServicesData
      {
        get => this.dataServicesData;
        set => this.dataServicesData = value;
      }

      public ServerIdentity ServerIdentity => this.serverIdentity;

      public string RemoteServer => this.remoteServer;

      public void ChangePassword(string newPassword) => this.User.ChangePassword(newPassword);

      public string WorkingFolder
      {
        get => this.UserInfo.WorkingFolder;
        set
        {
          if (value == SystemSettings.AllFolders)
            return;
          this.User.SetWorkingFolder(value);
          this.UserInfo.WorkingFolder = value;
        }
      }

      public EncompassEdition EncompassEdition
      {
        get
        {
          return this.IsConnected ? this.sessionObjects.ServerLicense.Edition : EncompassEdition.Broker;
        }
      }

      public bool IsBankerEdition() => this.EncompassEdition == EncompassEdition.Banker;

      public bool IsBrokerEdition() => this.EncompassEdition == EncompassEdition.Broker;

      public string DbVersion => this.startupInfo.DbVersion;

      public ICurrentUser User => this.sessionObjects.CurrentUser;

      public IEncERDBRegMgr EncERDBRegMgr => this.sessionObjects.EncERDBRegMgr;

      public IConfigurationManager ConfigurationManager => this.sessionObjects.ConfigurationManager;

      public IEfolderDocTrackViewManager EfolderDocTrackViewManager
      {
        get => this.sessionObjects.EfolderDocTrackViewManager;
      }

      public IContactManager ContactManager => this.sessionObjects.ContactManager;

      public IAclGroupManager AclGroupManager => this.sessionObjects.AclGroupManager;

      public IContactGroup ContactGroupManager => this.sessionObjects.ContactGroupManager;

      public IFormManager FormManager => this.sessionObjects.FormManager;

      public ILoanManager LoanManager => this.sessionObjects.LoanManager;

      public IDataSyncManager DataSyncManager => this.sessionObjects.DataSyncManager;

      public IReportManager ReportManager => this.sessionObjects.ReportManager;

      public IOrganizationManager OrganizationManager => this.sessionObjects.OrganizationManager;

      public IServerManager ServerManager => this.sessionObjects.ServerManager;

      public SettingsManager SettingsManager
      {
        get
        {
          if (this.settingsManager == null)
            this.settingsManager = new SettingsManager(this.ServerManager);
          return this.settingsManager;
        }
      }

      public IMessengerListManager MessengerListManager => this.sessionObjects.MessengerListManager;

      public IAlertManager AlertManager => this.sessionObjects.AlertManager;

      public IWelcomeScreenSettingMgr WelcomeScreenSettingMgr
      {
        get => this.sessionObjects.WelcomeScreenSettingMgr;
      }

      public ICalendarManager CalendarManager => this.sessionObjects.CalendarManager;

      public ILoanExternalFieldManager LoanExternalFieldManager
      {
        get => this.sessionObjects.LoanExternalFieldManager;
      }

      public ICampaign CampaignManager => this.sessionObjects.CampaignManager;

      public IMasterContractManager MasterContractManager
      {
        get => this.sessionObjects.MasterContractManager;
      }

      public ISecurityTradeManager SecurityTradeManager => this.sessionObjects.SecurityTradeManager;

      public ILoanTradeManager LoanTradeManager => this.sessionObjects.LoanTradeManager;

      public IMbsPoolManager MbsPoolManager => this.sessionObjects.MbsPoolManager;

      public ICorrespondentTradeManager CorrespondentTradeManager
      {
        get => this.sessionObjects.CorrespondentTradeManager;
      }

      public ILockComparisonFieldManager LockComparisonFieldManager
      {
        get => this.sessionObjects.LockComparisonFieldManager;
      }

      public IGseCommitmentManager GseCommitmentManager => this.sessionObjects.GseCommitmentManager;

      public IBatchJobsManager BatchJobsManager => this.sessionObjects.BatchJobsManager;

      public ITradeSynchronizationManager TradeSynchronizationManager
      {
        get => this.sessionObjects.TradeSynchronizationManager;
      }

      public ITradeLoanUpdateQueueManager TradeLoanUpdateQueueManager
      {
        get => this.sessionObjects.TradeLoanUpdateQueueManager;
      }

      public IPersonaManager PersonaManager => this.sessionObjects.PersonaManager;

      public ICorrespondentMasterManager CorrespondentMasterManager
      {
        get => this.sessionObjects.CorrespondentMasterManager;
      }

      public ILoanSummaryExtensionManager LoanSummaryExtensionManager
      {
        get => this.sessionObjects.LoanSummaryExtensionManager;
      }

      public IOverNightRateProtectionManager OverNightRateProtectionManager
      {
        get => this.sessionObjects.OverNightRateProtectionManager;
      }

      public IIdentityManager IdentityManager => this.sessionObjects.IdentityManager;

      public ISpecialFeatureCodeManager SpecialFeatureCodeManager
      {
        get => this.sessionObjects.SpecialFeatureCodeManager;
      }

      public ACL ACL
      {
        get
        {
          if (this.acl == null)
            this.acl = new ACL(this);
          return this.acl;
        }
      }

      public BPM BPM
      {
        get
        {
          if (this.bpm == null)
            this.bpm = new BPM(this);
          return this.bpm;
        }
      }

      public LoanDataMgr LoanDataMgr => this.loanDataMgr;

      public void SetLoanDataMgr(LoanDataMgr val) => this.SetLoanDataMgr(val, false);

      public void SetLoanDataMgr(LoanDataMgr val, bool autoSave)
      {
        this.closeCurrentLoan();
        this.loanDataMgr = val;
        if (this.loanDataMgr == null)
          return;
        this.loanDataMgr.Interactive = true;
        PerformanceMeter.Current.StartCustomization("Starting to fire the OnLoanOpened events");
        if (this.LoanOpened != null)
          this.LoanOpened((object) null, EventArgs.Empty);
        if (this.SID == "")
          EllieMae.EMLite.RemotingServices.Session.OnLoanOpened((object) null, EventArgs.Empty);
        PerformanceMeter.Current.StopCustomization("Finished firing the OnLoanOpened events");
      }

      private void closeCurrentLoan()
      {
        if (this.loanDataMgr == null)
          return;
        if (this.LoanClosing != null)
          this.LoanClosing((object) null, EventArgs.Empty);
        if (this.SID == "")
          EllieMae.EMLite.RemotingServices.Session.OnLoanClosing((object) null, EventArgs.Empty);
        this.loanDataMgr.Close();
      }

      public LoanData LoanData
      {
        get => this.loanDataMgr == null ? (LoanData) null : this.loanDataMgr.LoanData;
      }

      public SessionObjects SessionObjects => this.sessionObjects;

      public Form MainForm
      {
        set
        {
          if (this.mainForm != null)
            return;
          this.mainForm = value;
        }
        get => this.mainForm;
      }

      public IMainScreen MainScreen
      {
        set
        {
          if (this.mainScreen != null)
            return;
          this.mainScreen = value;
        }
        get => this.mainScreen;
      }

      public ISetup Setup
      {
        set
        {
          if (this.setup != null)
            return;
          this.setup = value;
        }
        get => this.setup;
      }

      public IApplicationScreen Application
      {
        get => this.appScreen;
        set => this.appScreen = value;
      }

      public object GetSystemSettings(System.Type settingsType)
      {
        BinaryObject systemSettings = this.ConfigurationManager.GetSystemSettings(settingsType.Name);
        return systemSettings == null ? settingsType.GetConstructor(System.Type.EmptyTypes).Invoke((object[]) null) : new XmlSerializer().Deserialize(systemSettings.OpenStream(), settingsType);
      }

      public void SaveSystemSettings(object settings)
      {
        MemoryStream memoryStream = new MemoryStream();
        new XmlSerializer().Serialize((Stream) memoryStream, settings);
        using (BinaryObject data = new BinaryObject((Stream) memoryStream))
          this.ConfigurationManager.SaveSystemSettings(settings.GetType().Name, data);
      }

      private object getObject(string name) => this.sessionObjects[name];

      internal object GetAclManager(AclCategory category)
      {
        return this.sessionObjects.GetAclManager(category);
      }

      private void handleCacheControlMessage(CacheControlMessage msg)
      {
        try
        {
          Tracing.Log(Sessions.Session.sw, "Sessions.Session", TraceLevel.Verbose, "Cache control message received for cache " + (object) msg.ClientSessionCache);
          CacheControlEventArgs args = new CacheControlEventArgs(msg);
          if (this.CacheControl != null)
            this.CacheControl((object) null, args);
          if (!(this.SID == ""))
            return;
          EllieMae.EMLite.RemotingServices.Session.OnCacheControlEventHandlerReceived((object) null, args);
        }
        catch (Exception ex)
        {
          Tracing.Log(Sessions.Session.sw, "Sessions.Session", TraceLevel.Error, "Error processing cache control message: " + (object) ex);
        }
      }

      private void handleServerEvent(IConnection conn, ServerEvent e)
      {
        if (this.MainForm != null)
        {
          if (this.MainForm.InvokeRequired)
          {
            try
            {
              this.MainForm.Invoke((Delegate) new ServerEventHandler(this.handleServerEvent), (object) conn, (object) e);
              return;
            }
            catch
            {
              return;
            }
          }
        }
        switch (e)
        {
          case MessageEvent _:
            EllieMae.EMLite.ClientServer.Message message = ((MessageEvent) e).Message;
            switch (message)
            {
              case null:
                return;
              case IMMessage _:
                EncompassMessenger.ProcessIMMessage(message as IMMessage);
                return;
              case CSControlMessage _:
                CSManagementDialog.ProcessCSMessage((CSMessage) (message as CSControlMessage));
                return;
              case CacheControlMessage _:
                this.handleCacheControlMessage(message as CacheControlMessage);
                return;
              case CEMessage _:
                EllieMae.EMLite.RemotingServices.Session.MainScreen.HandleCEMessage(message as CEMessage);
                return;
              case ShuMessage _:
                ShuMessage shuMessage = (ShuMessage) message;
                if (shuMessage.WaitTime <= 0)
                  return;
                using (ReconnectForm reconnectForm = new ReconnectForm(true, shuMessage.WaitTime))
                {
                  int num = (int) reconnectForm.ShowDialog((IWin32Window) this.MainForm);
                  return;
                }
              case SmuMessage _:
                SmuMessage smuMessage = (SmuMessage) message;
                if (smuMessage.WaitTime <= 0)
                  return;
                new ServerMajorUpdateForm(smuMessage.Text, smuMessage.WaitTime).Show();
                return;
              case TestMessage _:
                if (((TestMessage) message).ID != 0)
                  return;
                this.handleConnectionError(this.conn, ConnectionErrorType.ConnectionClosed);
                return;
              default:
                this.displayMessage(message);
                return;
            }
          case DisconnectEvent _:
            this.disconnect(e as DisconnectEvent);
            break;
          case SessionEvent _:
            SessionEvent sessionEvent = (SessionEvent) e;
            switch (sessionEvent.EventType)
            {
              case SessionEventType.Login:
                if (EncompassMessenger.Instance != null)
                {
                  EncompassMessenger.Instance.AddUserSession(sessionEvent.Session);
                  break;
                }
                break;
              case SessionEventType.Logout:
              case SessionEventType.Terminated:
                if (sessionEvent.Session.SessionID != this.SessionID)
                {
                  if (EncompassMessenger.Instance != null)
                    EncompassMessenger.Instance.RemoveUserSession(sessionEvent.Session);
                  ChatWindow chatWindow = ChatWindow.GetChatWindow(sessionEvent.Session.SessionID);
                  if (chatWindow != null)
                  {
                    chatWindow.BringToFront();
                    string str = Environment.NewLine + sessionEvent.Session.UserID;
                    string text = (sessionEvent.EventType != SessionEventType.Logout ? str + "'s session has been terminated and won't receive your messages." : str + " has just logged out and won't receive your messages.") + Environment.NewLine + Environment.NewLine;
                    chatWindow.AppendChatText(text, new Font(FontFamily.GenericSansSerif, 10f, FontStyle.Bold | FontStyle.Underline), System.Drawing.Color.Red);
                    break;
                  }
                  break;
                }
                break;
            }
            try
            {
              if (this.ServerSessionEvent == null)
                break;
              this.ServerSessionEvent((object) this, new ServerSessionEventArgs(sessionEvent));
              break;
            }
            catch (Exception ex)
            {
              Tracing.Log(Sessions.Session.sw, "Sessions.Session", TraceLevel.Error, "Error processing server session event: " + ex.Message);
              break;
            }
        }
      }

      public void SimulateConnectionError(ConnectionErrorType errType)
      {
        this.handleConnectionError(this.conn, errType);
      }

      private void handleConnectionError(IConnection conn, ConnectionErrorType errType)
      {
        if (EllieMae.EMLite.RemotingServices.Session.stopAutoReconnect)
        {
          EllieMae.EMLite.RemotingServices.Session.OnWriteDTLog((object) null, (EventArgs) null);
          int num = (int) Utils.Dialog((IWin32Window) EllieMae.EMLite.RemotingServices.Session.MainForm, "Please contact your Encompass System Administrator about this message. Your connection to the Encompass Server was lost and no disclosure tracking information has been saved. However, an eDisclosure email may have been sent to the borrower.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          conn = (IConnection) null;
          this.End();
          if (!this.ExitOnDisconnect)
            return;
          Application.Exit();
        }
        else if (EllieMae.EMLite.RemotingServices.Session.MainForm != null && EllieMae.EMLite.RemotingServices.Session.MainForm.InvokeRequired)
        {
          EllieMae.EMLite.RemotingServices.Session.MainForm.Invoke((Delegate) new ConnectionErrorEventHandler(this.handleConnectionError), (object) conn, (object) errType);
        }
        else
        {
          if (!EllieMae.EMLite.RemotingServices.Session.stopAutoReconnect)
          {
            if (this.reconnect(false))
              return;
            int num = (int) Utils.Dialog((IWin32Window) EllieMae.EMLite.RemotingServices.Session.MainForm, "The connection with the Encompass Server has been lost. You will need to log back in to continue working.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          conn = (IConnection) null;
          this.End();
          if (!this.ExitOnDisconnect)
            return;
          Application.Exit();
        }
      }

      private void displayMessage(EllieMae.EMLite.ClientServer.Message m)
      {
        string str1 = "";
        if (m.DisplayMessageFrom)
        {
          string str2 = m.Source.UserID + "@" + m.Source.Hostname;
          if (m.Source.UserID == "(rmi)")
            str2 = "the System Administrator";
          str1 = "Message from " + str2 + ":\r\n\r\n";
        }
        int num = (int) Utils.Dialog((IWin32Window) EllieMae.EMLite.RemotingServices.Session.MainForm, str1 + m.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }

      private bool reconnect(bool shuReconnect)
      {
        if (this.conn is Connection)
          ((ConnectionBase) this.conn).ConnectionError -= new ConnectionErrorEventHandler(this.handleConnectionError);
        if (EllieMae.EMLite.RemotingServices.Session.DefaultInstance != this)
        {
          if (this.conn is Connection)
            ((ConnectionBase) this.conn).ConnectionError += new ConnectionErrorEventHandler(this.handleConnectionError);
          return false;
        }
        using (ReconnectForm reconnectForm = new ReconnectForm(shuReconnect))
          return reconnectForm.ShowDialog((IWin32Window) this.MainForm) == DialogResult.Yes;
      }

      private void disconnect(DisconnectEvent e)
      {
        IWin32Window owner = (IWin32Window) Form.ActiveForm;
        if (EllieMae.EMLite.RemotingServices.Session.DefaultInstance == null)
        {
          for (int index = 0; index < 2; ++index)
          {
            Sessions.Session session = (Sessions.Session) Sessions.sessions[(object) string.Concat((object) index)];
            if (session.SessionID == e.Context.SessionID)
            {
              owner = (IWin32Window) session.MainForm;
              break;
            }
          }
        }
        else
          owner = (IWin32Window) EllieMae.EMLite.RemotingServices.Session.MainForm;
        if (e.EventArgument == DisconnectEventArgument.Nonforce && this.loanDataMgr != null && this.loanDataMgr.Writable && this.loanDataMgr.LoanData.Dirty)
        {
          if (this.isDisconnecting)
            return;
          this.isDisconnecting = true;
          DialogResult dialogResult = Utils.Dialog(owner, "You have been logged out by a system administrator. Would you like to save your changes to the current loan?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
          if (dialogResult == DialogResult.Yes && this.IsConnected)
            this.loanDataMgr.SaveLoan(true, (ILoanMilestoneTemplateOrchestrator) null, false);
          else if (dialogResult == DialogResult.Yes)
          {
            int num = (int) Utils.Dialog(owner, "Your changes cannot be saved because your session has been closed by an Administrator.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          if (this.IsConnected)
            this.loanDataMgr.Close();
        }
        else
        {
          if (this.isDisconnecting)
          {
            this.conn = (IConnection) null;
            return;
          }
          if (e.EventArgument == DisconnectEventArgument.ShuReconnect)
          {
            if (this.reconnect(true))
              return;
          }
          else if (e.EventArgument == DisconnectEventArgument.Reconnect && this.reconnect(false))
            return;
          this.isDisconnecting = true;
          if (e.EventArgument == DisconnectEventArgument.Force || e.EventArgument == DisconnectEventArgument.Reconnect)
            this.conn = (IConnection) null;
          int num = (int) Utils.Dialog(owner, "You have been logged out by a system administrator.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.ExitOnDisconnect = true;
        }
        this.End();
        if (!this.ExitOnDisconnect)
          return;
        Application.Exit();
      }

      public object GetComponentSetting(string componentName, object defaultSetting)
      {
        return this.GetComponentSetting(componentName) ?? defaultSetting;
      }

      public object GetComponentSetting(string componentName)
      {
        return this.startupInfo.ComponentSettings[(object) ("Components." + componentName)];
      }

      public object GetPrintingSetting(string settingName, object defaultValue)
      {
        return this.GetPrintingSetting(settingName) ?? defaultValue;
      }

      public object GetPrintingSetting(string settingName)
      {
        return this.startupInfo.PrintSettings[(object) ("Printing." + settingName)];
      }

      public string GetFormConfigFile(FormConfigFile fileType)
      {
        if ((OutputFormLocation) this.startupInfo.PrintSettings[(object) "Printing.ServerForm"] == OutputFormLocation.Server)
          return this.ConfigurationManager.GetFormConfigFile(fileType);
        string resourceFileFullPath;
        switch (fileType)
        {
          case FormConfigFile.FormGroupList:
            resourceFileFullPath = AssemblyResolver.GetResourceFileFullPath(SystemSettings.EMFormGroupListRelPath, SystemSettings.LocalAppDir);
            break;
          case FormConfigFile.InOutFormMapping:
            resourceFileFullPath = AssemblyResolver.GetResourceFileFullPath(SystemSettings.InOutFormMappingRelPath, SystemSettings.LocalAppDir);
            break;
          case FormConfigFile.OutFormAndFileMapping:
            resourceFileFullPath = AssemblyResolver.GetResourceFileFullPath(SystemSettings.OutFormAndFileMappingRelPath, SystemSettings.LocalAppDir);
            break;
          default:
            throw new ArgumentException("Invalid form configuration file type: " + (object) fileType);
        }
        using (StreamReader streamReader = new StreamReader(resourceFileFullPath, Encoding.ASCII))
          return streamReader.ReadToEnd();
      }

      public string GetPrivateProfileString(string section, string key)
      {
        if (section.Length > 32)
          section = section.Substring(0, 32);
        lock (this.startupInfo.UserProfileSettings)
          return string.Concat(this.startupInfo.UserProfileSettings[(object) (section + "." + key)]);
      }

      public string GetPrivateProfileString(string path)
      {
        lock (this.startupInfo.UserProfileSettings)
          return string.Concat(this.startupInfo.UserProfileSettings[(object) path]);
      }

      public void WritePrivateProfileString(string section, string key, string value)
      {
        if (section.Length > 32)
          section = section.Substring(0, 32);
        lock (this.startupInfo.UserProfileSettings)
        {
          if (!(this.GetPrivateProfileString(section, key) != value))
            return;
          this.User.WritePrivateProfileString(section, key, value);
          this.startupInfo.UserProfileSettings[(object) (section + "." + key)] = (object) (value ?? "");
        }
      }

      public void WritePrivateProfileString(string path, string value)
      {
        lock (this.startupInfo.UserProfileSettings)
        {
          if (!(this.GetPrivateProfileString(path) != value))
            return;
          string[] strArray = path.Split('.');
          if (strArray.Length < 2)
            throw new Exception("Invalid path specifier");
          this.User.WritePrivateProfileString(strArray[0], string.Join(".", strArray, 1, strArray.Length - 1), value);
          this.startupInfo.UserProfileSettings[(object) path] = (object) (value ?? "");
        }
      }

      public void RenameFavoriteLoanTemplateSet(string oldPath, string newPath, bool isFile)
      {
        lock (this.startupInfo.UserProfileSettings)
          this.User.RenameFavoriteLoanTemplateSet(oldPath, newPath, isFile);
      }

      public void DeleteFavoriteLoanTemplateSet(string path)
      {
        lock (this.startupInfo.UserProfileSettings)
          this.User.DeleteFavoriteLoanTemplateSet(path);
      }

      public UserLicenseInfo GetUserLicense()
      {
        lock (this.startupInfo)
          return this.startupInfo.UserLicense;
      }

      public void UpdateUserLicense(UserLicenseInfo license)
      {
        lock (this.startupInfo)
        {
          this.User.UpdateUserLicense(license);
          this.startupInfo.UserLicense = license;
        }
      }

      public ISessionStartupInfo StartupInfo => this.startupInfo;

      public LicenseInfo ServerLicense
      {
        get
        {
          lock (this.startupInfo)
            return this.sessionObjects.ServerLicense;
        }
      }

      public void RefreshServerLicense()
      {
        lock (this.startupInfo)
        {
          this.sessionObjects.InvalidateServerLicense();
          this.startupInfo.ServerLicense = this.sessionObjects.ServerLicense;
        }
      }

      public void NotifyApplicationReady()
      {
        if (this.ApplicationReady != null)
          this.ApplicationReady((object) null, EventArgs.Empty);
        if (!(this.SID == ""))
          return;
        EllieMae.EMLite.RemotingServices.Session.OnApplicationReady((object) null, EventArgs.Empty);
      }

      public string GetEPassPersonaDescriptor()
      {
        RealWorldRoleIDNameProvider roleIdNameProvider = new RealWorldRoleIDNameProvider();
        ArrayList arrayList = new ArrayList();
        if (this.UserInfo.IsAdministrator())
          arrayList.Add((object) "Admin");
        foreach (RolesMappingInfo roleMapping in this.startupInfo.RoleMappings)
        {
          if (roleMapping.RoleIDList != null)
          {
            foreach (int roleId in roleMapping.RoleIDList)
            {
              foreach (RoleSummaryInfo allowedRole in this.startupInfo.AllowedRoles)
              {
                if (allowedRole.RoleID == roleId)
                {
                  string name = roleIdNameProvider.GetName((object) roleMapping.RealWorldRoleID);
                  if (!arrayList.Contains((object) name))
                  {
                    arrayList.Add((object) name);
                    break;
                  }
                  break;
                }
              }
            }
          }
        }
        if (arrayList.Count == 0)
          arrayList.Add((object) "Other");
        return string.Join(" + ", (string[]) arrayList.ToArray(typeof (string)));
      }

      public void InitializePlugins()
      {
        ArrayList arrayList = new ArrayList();
        int num1 = 0;
        PerformanceMeter.Current.AddCheckpoint("Starting plugin download", 2069, nameof (InitializePlugins), "D:\\ws\\24.3.0.0\\EmLite\\ClientSession\\Sessions.cs");
        using (ShadowedCache shadowedCache = new ShadowedCache(this.conn.Session.SystemID, "Plugins"))
        {
          foreach (PluginInfo plugin in this.startupInfo.Plugins)
          {
            try
            {
              Version fileVersion = shadowedCache.GetFileVersion(plugin.Name);
              Version version = plugin.Version;
              if (fileVersion == (Version) null || version != fileVersion)
                shadowedCache.Put(plugin.Name, this.ConfigurationManager.GetPluginAssembly(plugin.Name));
              arrayList.Add((object) shadowedCache.GetFilePath(plugin.Name));
            }
            catch (Exception ex)
            {
              Tracing.Log(Sessions.Session.sw, "Sessions.Session", TraceLevel.Error, "Error retrieving plugin file '" + plugin.Name + "': " + ex.Message + ". This file will be skipped.");
              int num2 = (int) Utils.Dialog((IWin32Window) EllieMae.EMLite.RemotingServices.Session.MainForm, "An error has occurred while retrieving the '" + plugin.Name + "' plugin. This plugin will not run during the Encompass session.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
        }
        PerformanceMeter.Current.AddCheckpoint("Finished plugin download", 2095, nameof (InitializePlugins), "D:\\ws\\24.3.0.0\\EmLite\\ClientSession\\Sessions.cs");
        bool flag1 = false;
        Dictionary<System.Type, string> dictionary = new Dictionary<System.Type, string>();
        PerformanceMeter.Current.AddCheckpoint("Starting plugin loading and async authentication", 2103, nameof (InitializePlugins), "D:\\ws\\24.3.0.0\\EmLite\\ClientSession\\Sessions.cs");
        BlockingCollection<KeyValuePair<System.Type, string>> finishedBag = new BlockingCollection<KeyValuePair<System.Type, string>>();
        ConcurrentDictionary<string, bool> authenticationResults = new ConcurrentDictionary<string, bool>();
        foreach (string str in arrayList)
        {
          try
          {
            Assembly assembly = RuntimeContext.Current.LoadAssembly(str, this.startupInfo.RevertPluginChanges);
            if (!assembly.FullName.ToLower().Contains("utility4"))
            {
              foreach (System.Type key in ((IEnumerable<System.Type>) assembly.GetTypes()).Select(type => new
              {
                type = type,
                attribs = type.GetCustomAttributes(true)
              }).Where(_param1 => ((IEnumerable<object>) _param1.attribs).Select<object, string>((Func<object, string>) (x => (x as Attribute).GetType().FullName)).Contains<string>("EllieMae.Encompass.ComponentModel.PluginAttribute")).Select(_param1 => _param1.type))
              {
                dictionary.Add(key, str);
                KeyValuePair<System.Type, string> kvPair = new KeyValuePair<System.Type, string>(key, str);
                new Thread((ThreadStart) (() =>
                {
                  bool flag2 = false;
                  try
                  {
                    flag2 = this.authorizePlugin(kvPair.Value, kvPair.Key);
                  }
                  catch (Exception ex)
                  {
                  }
                  authenticationResults[kvPair.Value] = flag2;
                  finishedBag.Add(kvPair);
                })).Start();
              }
            }
          }
          catch (Exception ex)
          {
            PerformanceMeter.Current.Abort();
            Tracing.Log(Sessions.Session.sw, "Sessions.Session", TraceLevel.Error, "Failed to load assembly '" + Path.GetFileName(str) + "': " + (object) ex);
            this.showErrorDialogForAssembly(str);
          }
        }
        PerformanceMeter.Current.AddCheckpoint("Finished plugin loading and launching all async authentication tasks", 2154, nameof (InitializePlugins), "D:\\ws\\24.3.0.0\\EmLite\\ClientSession\\Sessions.cs");
        PerformanceMeter.Current.StartCustomization("Starting plugin initialization");
        for (int index = 0; index < dictionary.Count; ++index)
        {
          KeyValuePair<System.Type, string> keyValuePair = finishedBag.Take();
          System.Type key1 = keyValuePair.Key;
          string key2 = keyValuePair.Value;
          ++num1;
          if (!authenticationResults[key2])
          {
            flag1 = true;
          }
          else
          {
            string assembly = key1.Assembly.ToString();
            using (APICallContext.CreateExecutionBlock((IApiSourceContext) new APICallContext(key1.Name, assembly, APICallSourceType.Plugin, "Plugin.Initialize")))
            {
              try
              {
                using (Tracing.StartTimer(Sessions.Session.sw, "Sessions.Session", TraceLevel.Verbose, "Instantiating plugin '" + key1.FullName + "'"))
                  key1.InvokeMember("", BindingFlags.CreateInstance, (Binder) null, (object) null, new object[0]);
              }
              catch (Exception ex)
              {
                PerformanceMeter.Current.Abort();
                Tracing.Log(Sessions.Session.sw, "Sessions.Session", TraceLevel.Error, "An exception occurred attempting to instantiate plugin type '" + key1.FullName + "': " + (object) ex);
                int num3 = (int) Utils.Dialog((IWin32Window) EllieMae.EMLite.RemotingServices.Session.MainForm, "An error has occurred while initializing the '" + key1.FullName + "' plugin. This plugin will not run during the Encompass session.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              }
            }
          }
        }
        finishedBag.Dispose();
        PerformanceMeter.Current.StopCustomization("Finished plugin initialization");
        if (!flag1)
          return;
        int num4 = (int) Utils.Dialog((IWin32Window) EllieMae.EMLite.RemotingServices.Session.MainForm, "Encompass has blocked the use of one or more plugins. The functionality provided by those plugins will not be available while using Encompass.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }

      private void showErrorDialogForAssembly(string assemblyPath)
      {
        try
        {
          AssemblyName[] referencedAssemblies = Assembly.ReflectionOnlyLoadFrom(assemblyPath).GetReferencedAssemblies();
          List<string> stringList = new List<string>();
          Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
          foreach (AssemblyName assemblyName1 in referencedAssemblies)
          {
            AssemblyName assemblyName = assemblyName1;
            if (((IEnumerable<Assembly>) assemblies).Where<Assembly>((Func<Assembly, bool>) (a => a.GetName().Name == assemblyName.Name)).Count<Assembly>() == 0)
              stringList.Add(assemblyName.Name);
          }
          if (stringList.Count <= 0)
            return;
          List<string> pluginNames = new List<string>();
          using (ShadowedCache shadowedCache = new ShadowedCache(this.conn.Session.SystemID, "Plugins"))
          {
            foreach (string str1 in stringList)
            {
              string str2 = str1 + ".dll";
              if (!RuntimeContext.Current.IsAssemblyLoaded(str2))
              {
                string filePath = shadowedCache.GetFilePath(str2);
                if (!File.Exists(filePath))
                {
                  Tracing.Log(Sessions.Session.sw, "Sessions.Session", TraceLevel.Error, "Assembly does not exist: " + Path.GetFileName(filePath));
                  pluginNames.Add(Path.GetFileName(filePath));
                }
              }
            }
          }
          if (pluginNames.Count <= 0)
            return;
          int num = (int) Utils.Dialog((IWin32Window) EllieMae.EMLite.RemotingServices.Session.MainForm, "An error has occurred while loading the '" + Path.GetFileName(assemblyPath) + "' plugin. This plugin will not run during the Encompass session. Following plugin(s) are missing: " + this.getFormattedPluginString(pluginNames), MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        catch (Exception ex)
        {
        }
      }

      private string getFormattedPluginString(List<string> pluginNames)
      {
        string str = "'" + pluginNames[0] + "'";
        if (pluginNames.Count == 1)
          return str + ".";
        for (int index = 1; index < pluginNames.Count - 1; ++index)
        {
          if (index > 0)
            str = str + ", '" + pluginNames[index] + "'";
        }
        return str + " and '" + pluginNames[pluginNames.Count - 1] + "'.";
      }

      private bool authorizePlugin(string assemblyPath, System.Type pluginType)
      {
        Tracing.Log(Sessions.Session.sw, "Sessions.Session", TraceLevel.Verbose, "Authorizing plugin '" + Path.GetFileName(assemblyPath) + "'...");
        string withoutExtension = Path.GetFileNameWithoutExtension(assemblyPath);
        string str = withoutExtension + "." + pluginType.FullName;
        try
        {
          string crc = AuthenticationUtils.ComputeCRC(assemblyPath);
          string passPhrase = this.SessionObjects.SystemID + "|" + crc + "|" + str;
          if (AuthenticationUtils.IsPreauthorized(crc, PreauthorizedModuleType.Plugin, this.StartupInfo.PreauthorizedModules))
          {
            Tracing.Log(Sessions.Session.sw, "Sessions.Session", TraceLevel.Verbose, "Plugin '" + Path.GetFileName(assemblyPath) + "' is pre-authorized.");
            return true;
          }
          using (PluginService pluginService = new PluginService(this.sessionObjects?.StartupInfo?.ServiceUrls?.JedServicesUrl))
          {
            Tracing.Log(Sessions.Session.sw, "Sessions.Session", TraceLevel.Verbose, "Invoking plugin authorization service for '" + str + "'....");
            pluginService.Timeout = 60000;
            pluginService.AuthorizePlugin(this.CompanyInfo.ClientID, Environment.MachineName, Environment.UserName, withoutExtension, pluginType.FullName, crc, passPhrase);
            Tracing.Log(Sessions.Session.sw, "Sessions.Session", TraceLevel.Verbose, "Plugin '" + str + "' successfully authorized.");
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(Sessions.Session.sw, "Sessions.Session", TraceLevel.Error, "Authorization failed for plugin '" + str + "'. Reason resported = '" + ex.Message + "'.");
          if (ex.ToString().IndexOf("This external module is not authorized") >= 0)
            return false;
          if (ex is SoapException)
          {
            if (ex.Message.IndexOf("--> System.Security.Cryptography.CryptographicException: CryptoAPI cryptographic service provider (CSP) for this implementation could not be acquired.") >= 0)
              Tracing.Log(Sessions.Session.sw, "Sessions.Session", TraceLevel.Warning, "Cryptographic exception: " + ex.Message);
          }
        }
        return true;
      }

      public void InvokeFormLoaded(object formObject)
      {
        if (this.FormLoaded != null)
          this.FormLoaded(formObject, EventArgs.Empty);
        if (!(this.SID == ""))
          return;
        EllieMae.EMLite.RemotingServices.Session.OnFormLoaded(formObject, EventArgs.Empty);
      }

      public void InvokeFormUnloading(object formObject)
      {
        if (this.FormUnloading != null)
          this.FormUnloading(formObject, EventArgs.Empty);
        if (!(this.SID == ""))
          return;
        EllieMae.EMLite.RemotingServices.Session.OnFormUnloading(formObject, EventArgs.Empty);
      }

      public void InsertSystemAuditRecord(SystemAuditRecord record)
      {
        this.conn.Session.InsertAuditRecord(record);
      }

      public SystemAuditRecord[] GetAuditRecord(
        string userID,
        ActionType actionType,
        DateTime startTime,
        DateTime endTime,
        string objectID,
        string objectName)
      {
        return this.conn.Session.GetAuditRecord(userID, actionType, startTime, endTime, objectID, objectName);
      }
    }
  }
}
