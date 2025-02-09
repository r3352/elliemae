// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanDataMgr
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using Elli.AdvCode.Runtime;
using Elli.Domain.FileFormats.Encompass360Xml;
using Elli.Domain.Mortgage;
using Elli.ElliEnum;
using Elli.Interface;
using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.EPC2;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.ClientServer.LockComparison;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Metrics;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.ConcurrentEditing;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.InterimServicing;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.eFolder;
using EllieMae.EMLite.eFolder.Files;
using EllieMae.EMLite.LoanUtils.DataEngine;
using EllieMae.EMLite.LoanUtils.EDelivery;
using EllieMae.EMLite.LoanUtils.eFolder.SkyDriveClassic;
using EllieMae.EMLite.LoanUtils.EnhancedConditions;
using EllieMae.EMLite.LoanUtils.RateLocks;
using EllieMae.EMLite.LoanUtils.Services;
using EllieMae.EMLite.LoanUtils.SkyDrive;
using EllieMae.EMLite.LoanUtils.Trading;
using EllieMae.EMLite.LoanUtils.WcfExtensions;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.Trading;
using EllieMae.EMLite.WebServices;
using EllieMae.EMLite.Workflow;
using EllieMae.Encompass.AsmResolver;
using EllieMae.Encompass.Xml3WayMerge.Utils;
using Encompass.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LoanDataMgr : IDisposable
  {
    private const string className = "LoanDataMgr�";
    protected static string sw = Tracing.SwOutsideLoan;
    public const string NewFileLoanSource = "Encompass - New File�";
    public const string DuplicateFileLoanSource = "Encompass - Loan Duplication�";
    public const int StartSellSideAdjFieldID = 2233;
    public const int StartSellSideExtFieldID = 3495;
    public const int StartBuySideAdjFieldID = 2162;
    public const int StartRequestSideAdjFieldID = 2102;
    public const int MaxAdjCount = 20;
    public const int MaxExtCount = 10;
    private const string NON_CONSUMER_CONNECT_SITEID = "0�";
    private static ISaveLoan iSave = (ISaveLoan) null;
    public string CurrentFormOrTool;
    private ILoan loan;
    private LoanData loanData;
    private LoanCalculator calc;
    private LoanValidator validator;
    private LoanAccessRules accessRules;
    private LoanTriggers triggers;
    private LoanAutoPrintSelector printSelector;
    private LoanAlertMonitor alertMonitor;
    private InputFormList formSettings;
    private bool readOnly = true;
    private LoanIdentity id;
    private Hashtable rights;
    private LoanInfo.LockReason loanLock;
    private bool isClosed;
    private DateTime lastModified = DateTime.MinValue;
    private string newFolderName = "";
    private string newLoanName = "";
    private FileAttachmentCollection fileAttachments;
    private LoanProperty[] loanPropertySettings;
    private bool useSkyDrive;
    private bool useSkyDriveLite;
    private bool useSkyDriveClassic;
    private LoanHistoryManager loanHistory;
    private SortedList supportingDataList;
    private Dictionary<string, Task> skyDriveFileTasks = new Dictionary<string, Task>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private LoanDataMgr linkedLoan;
    private bool linkLoanLoaded;
    private bool interactive;
    private LoanDataMgr.ImmediateExclusiveLockType immediateExclusiveLockType;
    private EnhancedConditionTemplate[] enhancedConditionTemplates;
    private Task ngLoanCreateTask;
    private Loan ngLoan;
    private HashSet<LockComparisonField> lockComparisonFieldsModified = new HashSet<LockComparisonField>();
    private ILoanConfigurationInfo configInfo;
    private bool fromLoanImport;
    private string hideChangesMadeByOthers = "";
    private bool fromPlatform;
    private string respaSetByTemplate;
    private string x3wmBaseLoanDataXml;
    private bool launcheFolderNeeded;
    private SessionObjects sessionObjects;
    public static Task asyncTriggerCompileTask;
    private Task<PipelineInfo> pipelineInfoTask;
    private Task<LoanData> loanDataTask;
    private Task<LoanProperty[]> loanPropertySettingsTask;
    private Dictionary<string, EllieMae.EMLite.ClientServer.LoanAssociateInfo[]> loanAssociatesCache = new Dictionary<string, EllieMae.EMLite.ClientServer.LoanAssociateInfo[]>((IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase);
    private CEMergeResultsOption mergeResultsOption;
    private const int maxX3wmTries = 5;
    private ILoanMilestoneTemplateOrchestrator milestoneTemplateController = NoMilestoneTemplateApply.GetInstance();
    private Hashtable fieldRuleTable;
    private Hashtable preRequiredFields;
    private Hashtable requiredFieldTable;
    private Hashtable fieldRightsTable;
    private const string defaultExclusiveLockErrMsg = "You cannot perform this task while other users are editing the loan.�";
    private string wcNotAllowedMessage;

    public event EventHandler OnLoanDataXmlReplaced;

    public event EventHandler AfterDDMApplied;

    public event EventHandler BeforeLoanRefreshedFromServer;

    public event EventHandler OnLoanRefreshedFromServer;

    public static ISaveLoan ISave
    {
      get => LoanDataMgr.iSave;
      set
      {
        LoanDataMgr.iSave = LoanDataMgr.iSave == null ? value : throw new Exception("ISave has already been set");
      }
    }

    public ILoanConfigurationInfo ConfigInfo => this.configInfo;

    public bool IsFromPlatform => this.fromPlatform;

    public string X3wmBaseLoanDataXml => this.x3wmBaseLoanDataXml;

    public bool LauncheFolderNeeded
    {
      set => this.launcheFolderNeeded = value;
      get => this.launcheFolderNeeded;
    }

    public HashSet<LockComparisonField> LockComparisonFieldsModified
    {
      get => this.lockComparisonFieldsModified;
      set => this.lockComparisonFieldsModified = value;
    }

    public LoanActivityMetricData LastActivity { get; set; }

    public event CancelableEventHandler BeforeSavingLoanFiles;

    public event SavingLoanFilesEventHandler AfterSavingLoanFiles;

    public event EventHandler AccessRightsChanged;

    public event EventHandler FieldRulesChanged;

    public event EventHandler LoanClosing;

    public event EventHandler BeforeTriggerRuleApplied;

    public event EventHandler ExecuteEmailTriggers;

    public event EventHandler OnEPCIntegrationClose;

    public event LoanActivityEventHandler BeforeLoanActivity;

    public event LoanActivityEventHandler AfterLoanActivity;

    public void HandleEPCIntegrationClose()
    {
      if (this.OnEPCIntegrationClose == null)
        return;
      this.OnEPCIntegrationClose((object) this, (EventArgs) null);
    }

    private ReauthenticateOnUnauthorised ReauthenticateOnUnauthorised { get; set; }

    public static LoanDataMgr NewLoanFromPlatformService(
      SessionObjects sessionObjects,
      bool forImport,
      bool skipLoanOfficerAssignment = false,
      string loanFolder = "�")
    {
      if (!forImport)
        return LoanDataMgr.NewLoan(sessionObjects, (LoanTemplateSelection) null, loanFolder, "", true, skipLoanOfficerAssignment);
      LoanDataMgr loanDataMgr = LoanDataMgr.BlankLoan(sessionObjects, string.Empty, string.Empty, true);
      loanDataMgr.CopyTPOCustomFieldsToLoanFields(sessionObjects.UserInfo);
      LoanDataMgr.setMilestoneTemplateOnNew(sessionObjects, loanDataMgr.LoanData, string.Empty, true);
      loanDataMgr.ApplyTemplateOnNew(sessionObjects, loanDataMgr.LoanData, (Hashtable) null);
      EllieMae.EMLite.DataEngine.Log.MilestoneLog milestoneByName = loanDataMgr.LoanData.GetLogList().GetMilestoneByName("Started");
      milestoneByName.RoleID = 0;
      milestoneByName.RoleName = "File Starter";
      milestoneByName.SetLoanAssociate(sessionObjects.UserInfo);
      try
      {
        object policySetting = sessionObjects.StartupInfo.PolicySettings[(object) "Policies.NewRESPA2015"];
        if (policySetting != null)
        {
          if (DateTime.Today.Date >= ((DateTime) policySetting).Date)
          {
            if (!EllieMae.EMLite.Common.Utils.CheckIf2015RespaTila(loanDataMgr.LoanData.GetField("3969")))
              loanDataMgr.LoanData.SetField("3969", "RESPA-TILA 2015 LE and CD");
          }
          else
          {
            if (loanDataMgr.LoanData.GetField("3969") != "RESPA 2010 GFE and HUD-1")
              loanDataMgr.LoanData.SetField("3969", "RESPA 2010 GFE and HUD-1");
            loanDataMgr.LoanData.SetField("NEWHUD.X354", "Y");
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), "Can't setup field NEWHUD.X354. Error: " + ex.Message);
      }
      LoanDefaults loanDefaultData = sessionObjects.LoanManager.GetLoanDefaultData();
      loanDataMgr.populateField(loanDefaultData.PrivacyPolicyFields.Map);
      return loanDataMgr;
    }

    public static LoanDataMgr NewLoan(
      SessionObjects sessionObjects,
      LoanTemplateSelection template,
      string loanFolder,
      string loanName,
      bool fromPlatform = false,
      bool skipLoanOfficerAssignment = false)
    {
      LoanDataMgr loanDataMgr = LoanDataMgr.BlankLoan(sessionObjects, loanFolder, loanName, fromPlatform);
      loanDataMgr.InitializeNewLoan(template, skipLoanOfficerAssignment);
      MilestoneTemplate milestoneTemplate = (MilestoneTemplate) null;
      Hashtable templateSettings = (Hashtable) null;
      if (template != null)
      {
        templateSettings = sessionObjects.ConfigurationManager.GetLoanTemplateComponents(template.TemplateEntry);
        if (templateSettings != null && Convert.ToString(templateSettings[(object) "MILETEMP"]) != string.Empty)
          milestoneTemplate = sessionObjects.BpmManager.GetMilestoneTemplateByGuid(Convert.ToString(templateSettings[(object) "MILETEMP"]));
      }
      if (milestoneTemplate == null)
      {
        LoanDataMgr.setMilestoneTemplateOnNew(sessionObjects, loanDataMgr.LoanData, template != null ? template.TemplateEntry.Name : "", false);
        loanDataMgr.ApplyTemplateOnNew(sessionObjects, loanDataMgr.LoanData, templateSettings);
      }
      return loanDataMgr;
    }

    private static void setMilestoneTemplateOnNew(
      SessionObjects sessionObjects,
      LoanData loanData,
      string loanTemplate,
      bool overRide)
    {
      MilestoneTemplate milestoneTemplate1 = LoanDataMgr.GetBestMatchingMilestoneTemplate(sessionObjects, loanData);
      MilestoneTemplate milestoneTemplate2 = loanData.GetLogList().MilestoneTemplate;
      if (!milestoneTemplate1.Equals(milestoneTemplate2))
      {
        LoanDataMgr.replaceTemplate(sessionObjects, milestoneTemplate1, loanTemplate, false, false, loanData, false);
      }
      else
      {
        if (!milestoneTemplate2.IsDefaultTemplate)
          return;
        LoanDataMgr.replaceTemplate(sessionObjects, milestoneTemplate1, loanTemplate, false, false, loanData, overRide);
      }
    }

    public void SetMilestoneTemplateOnNew(
      string loanTemplate,
      Hashtable templateSettings,
      bool overRide)
    {
      LoanDataMgr.setMilestoneTemplateOnNew(this.sessionObjects, this.loanData, loanTemplate, overRide);
      this.ApplyTemplateOnNew(this.sessionObjects, this.loanData, templateSettings);
    }

    public static MilestoneTemplate GetBestMatchingMilestoneTemplate(
      SessionObjects sessionObjects,
      LoanData loanData)
    {
      List<string> satisfiedTemplates = new MilestoneTemplatesManager().GetSatisfiedMilestoneTemplate(LoanDataMgr.GetLoanConditionsForMilestoneTemplate(loanData), loanData, sessionObjects.UserInfo, sessionObjects);
      IEnumerable<MilestoneTemplate> milestoneTemplates = sessionObjects.BpmManager.GetMilestoneTemplates(true);
      return satisfiedTemplates.Count > 0 ? milestoneTemplates.FirstOrDefault<MilestoneTemplate>((Func<MilestoneTemplate, bool>) (x => satisfiedTemplates.Contains(x.Name))) : (MilestoneTemplate) null;
    }

    public static LoanConditions GetLoanConditionsForMilestoneTemplate(LoanData loanData)
    {
      EllieMae.EMLite.DataEngine.Log.MilestoneLog msCheck = (EllieMae.EMLite.DataEngine.Log.MilestoneLog) null;
      EllieMae.EMLite.DataEngine.Log.MilestoneLog msToBeFinished = (EllieMae.EMLite.DataEngine.Log.MilestoneLog) null;
      LoanDataMgr.getLoanMilestoneStatus(ref msCheck, ref msToBeFinished, loanData.GetLogList());
      return new LoanBusinessRuleInfo(loanData).CurrentLoanForBusinessRule(msCheck, msToBeFinished);
    }

    private void ApplyTemplateOnNew(
      SessionObjects sessionObjects,
      LoanData loanData,
      Hashtable templateSettings)
    {
      if (templateSettings == null)
        return;
      TaskSetTemplate templateSetting1 = (TaskSetTemplate) (BinaryObject) templateSettings[(object) "TASKSET"];
      DocumentSetTemplate templateSetting2 = (DocumentSetTemplate) (BinaryObject) templateSettings[(object) "DOCSET"];
      LogList logList = loanData.GetLogList();
      try
      {
        if (templateSetting2 != null)
        {
          foreach (EllieMae.EMLite.DataEngine.Log.DocumentLog allDocument in logList.GetAllDocuments())
            logList.RemoveRecord((LogRecordBase) allDocument, true);
          this.ApplyDocumentTemplate(logList, templateSetting2);
          loanData.SetField("2863", string.Concat(templateSettings[(object) "DOCSETFILE"]));
        }
        foreach (EllieMae.EMLite.DataEngine.Log.MilestoneTaskLog milestoneTaskLog in logList.GetAllMilestoneTaskLogs())
          logList.RemoveRecord((LogRecordBase) milestoneTaskLog);
      }
      catch (Exception ex)
      {
      }
      foreach (EllieMae.EMLite.DataEngine.Log.MilestoneTaskLog milestoneTaskLog in logList.GetAllMilestoneTaskLogs())
        logList.RemoveRecord((LogRecordBase) milestoneTaskLog);
      loanData.SetTaskSetTemplate(templateSetting1, this.configInfo.TasksSetup, this.configInfo.MilestonesList, sessionObjects.UserInfo);
    }

    private static void getLoanMilestoneStatus(
      ref EllieMae.EMLite.DataEngine.Log.MilestoneLog msCheck,
      ref EllieMae.EMLite.DataEngine.Log.MilestoneLog msToBeFinished,
      LogList logList)
    {
      EllieMae.EMLite.DataEngine.Log.MilestoneLog[] allMilestones = logList.GetAllMilestones();
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

    public static LoanDataMgr NewLoan(
      SessionObjects sessionObjects,
      FileSystemEntry template,
      string loanFolder,
      string loanName)
    {
      return LoanDataMgr.NewLoan(sessionObjects, new LoanTemplateSelection(template), loanFolder, loanName);
    }

    public static LoanDataMgr NewLoan(
      SessionObjects sessionObjects,
      string loanFolder,
      string loanName)
    {
      return LoanDataMgr.NewLoan(sessionObjects, (LoanTemplateSelection) null, loanFolder, loanName);
    }

    public static LoanDataMgr NewLoan(SessionObjects sessionObjects, string loanFolder)
    {
      return LoanDataMgr.NewLoan(sessionObjects, loanFolder, "");
    }

    public static LoanDataMgr NewLoan(SessionObjects sessionObjects)
    {
      return LoanDataMgr.NewLoan(sessionObjects, "", "");
    }

    public static LoanDataMgr BlankLoan(SessionObjects sessionObjects)
    {
      return LoanDataMgr.BlankLoan(sessionObjects, "", "");
    }

    public static LoanDataMgr BlankLoan(
      SessionObjects sessionObjects,
      LoanDataMgr.ImportSource source)
    {
      return LoanDataMgr.BlankLoan(sessionObjects, "", "", source: string.Format("Loan Import - {0}", (object) source));
    }

    public static LoanDataMgr BlankLoan(SessionObjects sessionObjects, string loanFolder)
    {
      return LoanDataMgr.BlankLoan(sessionObjects, loanFolder, "");
    }

    public static LoanDataMgr BlankLoan(
      SessionObjects sessionObjects,
      string loanFolder,
      string loanName,
      bool fromPlatform = false,
      string source = null)
    {
      LoanDataMgr loanDataMgr = new LoanDataMgr(sessionObjects, nameof (BlankLoan), loanFolder, loanName, fromPlatform);
      LoanDataMgr.setEncompassVersion(loanDataMgr);
      if (source != null)
        loanDataMgr.initLoanData((LoanTemplateSelection) null, "dummy", false);
      if (!fromPlatform)
        loanDataMgr.CreateDDMTrigger();
      return loanDataMgr;
    }

    public static LoanDataMgr OpenLoan(
      SessionObjects sessionObjects,
      string loanFolder,
      string loanName,
      bool isExternalOrganization)
    {
      return LoanDataMgr.OpenLoan(sessionObjects, loanFolder, loanName, (LoanDataMgr) null, isExternalOrganization);
    }

    public static LoanDataMgr OpenLoan(
      SessionObjects sessionObjects,
      string loanFolder,
      string loanName,
      LoanDataMgr linkedLoan,
      bool isExternalOrganization)
    {
      LoanDataMgr loanDataMgr = new LoanDataMgr(sessionObjects, loanFolder, loanName, linkedLoan, isExternalOrganization);
      loanDataMgr.CreateDDMTrigger();
      loanDataMgr.UpdateTPOConnectStatus();
      return loanDataMgr;
    }

    public static LoanDataMgr OpenLoan(
      SessionObjects sessionObjects,
      string guid,
      bool isExternalOrganization)
    {
      return LoanDataMgr.OpenLoan(sessionObjects, guid, isExternalOrganization, 0);
    }

    public static LoanDataMgr OpenLoan(
      SessionObjects sessionObjects,
      string guid,
      bool isExternalOrganization,
      int sqlRead,
      bool executeDdmTrigger = true,
      LoanDataMgr.ImmediateExclusiveLockType immediateLockType = LoanDataMgr.ImmediateExclusiveLockType.NoLock)
    {
      LoanDataMgr loanDataMgr = new LoanDataMgr(sessionObjects, guid, (LoanDataMgr) null, isExternalOrganization, sqlRead, immediateLockType);
      if (executeDdmTrigger)
        loanDataMgr.CreateDDMTrigger();
      loanDataMgr.UpdateTPOConnectStatus();
      return loanDataMgr;
    }

    public static LoanDataMgr OpenLoan(
      SessionObjects sessionObjects,
      string guid,
      LoanDataMgr linkedLoan,
      bool isExternalOrganization,
      LoanDataMgr.ImmediateExclusiveLockType immediateLockType = LoanDataMgr.ImmediateExclusiveLockType.NoLock)
    {
      LoanDataMgr loanDataMgr = new LoanDataMgr(sessionObjects, guid, linkedLoan, isExternalOrganization, 0, immediateLockType);
      loanDataMgr.CreateDDMTrigger();
      loanDataMgr.UpdateTPOConnectStatus();
      return loanDataMgr;
    }

    public static LoanDataMgr AttachNew(
      SessionObjects sessionObjects,
      LoanData data,
      string loanFolder,
      string loanName)
    {
      return new LoanDataMgr(sessionObjects, data, loanFolder, loanName);
    }

    public static LoanDataMgr GetLoanDataMgr(
      SessionObjects sessionObjects,
      LoanData data,
      bool fromPlatform,
      bool requireLoanProxy = false,
      bool enforceCountyLimit = true)
    {
      return new LoanDataMgr(sessionObjects, data, fromPlatform, requireLoanProxy, enforceCountyLimit);
    }

    public static LoanDataMgr CopyLoan(
      SessionObjects sessionObjects,
      LoanData data,
      string loanFolder,
      string loanName)
    {
      return LoanDataMgr.CopyLoan(sessionObjects, data, loanFolder, loanName, "");
    }

    public static LoanDataMgr CopyLoan(
      SessionObjects sessionObjects,
      LoanData data,
      string loanFolder,
      string loanName,
      string templateToUse)
    {
      string systemId = sessionObjects.SystemID;
      LoanDataMgr loanDataMgr = !(templateToUse == string.Empty) ? LoanDataMgr.BlankLoan(sessionObjects, loanFolder, loanName) : new LoanDataMgr(sessionObjects, new LoanData(data, sessionObjects.LoanManager.GetLoanSettings(data?.GetField("HMDA.X100"))), loanFolder, loanName);
      loanDataMgr.initializeCopiedLoan(data, templateToUse);
      return loanDataMgr;
    }

    private LoanDataMgr(
      SessionObjects sessionObjects,
      string templateName,
      string loanFolder,
      string loanName,
      bool fromPlatform = false)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Initializing LoanDataMgr object from template...");
      this.sessionObjects = sessionObjects;
      this.configInfo = sessionObjects.LoanManager.GetLoanConfigurationInfo(LoanDataMgr.getLoanConfigurationParameters(sessionObjects), this.loan, loanFolder, loanName);
      if (this.configInfo.UnderwritingConditionTrackingSetup == null)
        this.configInfo.UnderwritingConditionTrackingSetup = new UnderwritingConditionTrackingSetup();
      if (this.configInfo.PostClosingConditionTrackingSetup == null)
        this.configInfo.PostClosingConditionTrackingSetup = new PostClosingConditionTrackingSetup();
      if (this.configInfo.SellConditionTrackingSetup == null)
        this.configInfo.SellConditionTrackingSetup = new SellConditionTrackingSetup();
      this.loanData = fromPlatform ? this.LoanManager.InstantiateLoanTemplateFromPlatform(templateName) : this.LoanManager.InstantiateLoanTemplate(templateName);
      this.loanData.Settings.HMDAInfo = this.configInfo.LoanSettings.HMDAInfo;
      this.readOnly = false;
      this.rights = new Hashtable();
      this.rights.Add((object) sessionObjects.UserID, (object) LoanInfo.Right.FullRight);
      this.isClosed = false;
      this.LoanFolder = loanFolder;
      this.newLoanName = loanName;
      this.supportingDataList = new SortedList();
      this.loanData.TPOConnectStatus = true;
      this.InitializeNgLoan();
      this.setEnforceCountyLoanLimit();
      if (!fromPlatform)
      {
        this.attachCalculator();
        this.updateX3wmBase();
      }
      else
      {
        if (!this.loanData.IsLocked("HMDA.X27") && this.loanData != null && !this.loanData.IsTemplate)
          this.loanData.SetField("HMDA.X27", ToolCalculation.GetHmdaReportingYear(EllieMae.EMLite.Common.Utils.ParseDate((object) this.loanData.GetField("749")), this.loanData.GetSimpleField("1393")).ToString());
        this.InitFeeLevelDisclosuresIndicator();
      }
      this.fromPlatform = fromPlatform;
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr initialized from template '" + templateName + "'...");
    }

    private void InitFeeLevelDisclosuresIndicator()
    {
      EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log idisclosureTracking = this.loanData.GetLogList().GetFirst2015IDisclosureTracking();
      if (idisclosureTracking == null)
      {
        if (!this.ConfigInfo.RequireCoCPriorDisclosure)
          return;
        this.loanData.SetCurrentField("4461", "Y");
      }
      else
        this.loanData.SetCurrentField("4461", idisclosureTracking.GetDisclosedField("4461"));
    }

    private LoanDataMgr(
      SessionObjects sessionObjects,
      LoanData loanData,
      bool fromPlatform = false,
      bool requireLoanProxy = false,
      bool enforceCountyLimit = true)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Initializing LoanDataMgr object from loanData...");
      this.sessionObjects = sessionObjects;
      this.loanData = loanData;
      if (this.loanData == null)
        throw new ObjectNotFoundException("Loan not found", ObjectType.Loan, (object) this.id.Guid);
      this.id = new LoanIdentity(loanData.GUID);
      this.InitializeNgLoan();
      this.configInfo = sessionObjects.LoanManager.GetLoanConfigurationInfo(LoanDataMgr.getLoanConfigurationParameters(sessionObjects), this.id.LoanFolder, this.id.LoanName, loanData?.GetField("HMDA.X100"));
      if (this.configInfo.UnderwritingConditionTrackingSetup == null)
        this.configInfo.UnderwritingConditionTrackingSetup = new UnderwritingConditionTrackingSetup();
      if (this.configInfo.PostClosingConditionTrackingSetup == null)
        this.configInfo.PostClosingConditionTrackingSetup = new PostClosingConditionTrackingSetup();
      if (this.configInfo.SellConditionTrackingSetup == null)
        this.configInfo.SellConditionTrackingSetup = new SellConditionTrackingSetup();
      this.readOnly = false;
      this.rights = new Hashtable();
      this.rights.Add((object) sessionObjects.UserID, (object) LoanInfo.Right.FullRight);
      this.isClosed = false;
      this.newFolderName = this.id.LoanFolder;
      this.newLoanName = this.id.LoanName;
      this.supportingDataList = new SortedList();
      if (enforceCountyLimit)
        this.setEnforceCountyLoanLimit();
      if (!fromPlatform)
      {
        this.attachCalculator();
        this.updateX3wmBase();
      }
      if (requireLoanProxy)
        this.loan = this.LoanManager.OpenLoan(this.id.Guid);
      this.fromPlatform = fromPlatform;
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr initialized from loanData guid '" + loanData.GUID + "'...");
    }

    private LoanDataMgr(
      SessionObjects sessionObjects,
      string loanFolder,
      string loanName,
      bool isExternalOrganization,
      bool fromPlatform)
      : this(sessionObjects, loanFolder, loanName, (LoanDataMgr) null, isExternalOrganization, fromPlatform)
    {
    }

    public void ResetSessionObjects(SessionObjects sessionObjects)
    {
      this.sessionObjects = sessionObjects;
      this.loan = this.LoanManager.OpenLoan(this.LoanFolder, this.LoanName);
      this.loan.Lock(this.loanLock, this.GetCurrentLock().Exclusive, true);
      if (this.calc == null)
        return;
      if (this.loanData == null)
        return;
      try
      {
        this.calc = new LoanCalculator(this.sessionObjects, this.configInfo, this.loanData, this.calc.NoUSDAWarning, (ExternalLateFeeSettings) null);
        this.attachCalculator();
      }
      catch (Exception ex)
      {
        try
        {
          Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Error, "Unable to attach calculator: " + ex.Message);
        }
        catch
        {
        }
      }
    }

    private LoanDataMgr(
      SessionObjects sessionObjects,
      string loanFolder,
      string loanName,
      LoanDataMgr linkedLoan,
      bool isExternalOrganization,
      bool fromPlatform = false)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Initializing linked LoanDataMgr object...");
      this.sessionObjects = sessionObjects;
      this.loan = this.LoanManager.OpenLoan(loanFolder, loanName);
      if (this.loan == null)
        throw new ObjectNotFoundException("Loan not found", ObjectType.Loan, (object) loanName);
      if (linkedLoan != null)
      {
        this.configInfo = linkedLoan.configInfo;
      }
      else
      {
        this.configInfo = sessionObjects.LoanManager.GetLoanConfigurationInfo(LoanDataMgr.getLoanConfigurationParameters(sessionObjects), loanFolder, loanName);
        if (this.configInfo.UnderwritingConditionTrackingSetup == null)
          this.configInfo.UnderwritingConditionTrackingSetup = new UnderwritingConditionTrackingSetup();
        if (this.configInfo.PostClosingConditionTrackingSetup == null)
          this.configInfo.PostClosingConditionTrackingSetup = new PostClosingConditionTrackingSetup();
        if (this.configInfo.SellConditionTrackingSetup == null)
          this.configInfo.SellConditionTrackingSetup = new SellConditionTrackingSetup();
      }
      this.linkedLoan = linkedLoan;
      this.linkLoanLoaded = this.linkedLoan != null;
      this.Refresh(isExternalOrganization);
      this.updateX3wmBase();
      this.fromPlatform = fromPlatform;
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr initialized for linked loan '" + loanFolder + "/" + loanName + "'...");
    }

    private LoanDataMgr(
      SessionObjects sessionObjects,
      string guid,
      LoanDataMgr linkedLoan,
      bool isExternalOrganization)
      : this(sessionObjects, guid, linkedLoan, isExternalOrganization, 0)
    {
    }

    private void applyLoanSpecificConfigInfoOnLoanConfigInfo(
      ILoanSpecificConfigurationInfo configInfo)
    {
      ILoanConfigurationInfo configurationInfo = this.SessionObjects.LoanConfigurationInfo;
      configurationInfo.LoanSettings.HMDAInfo = configInfo.HmdaInfo;
      configurationInfo.IsDuplicateLoanCheckLoanOnly = configInfo.IsDuplicateLoanCheckLoanOnly;
    }

    private LoanDataMgr(
      SessionObjects sessionObjects,
      string guid,
      LoanDataMgr linkedLoan,
      bool isExternalOrganization,
      int sqlRead,
      LoanDataMgr.ImmediateExclusiveLockType immediateLockType = LoanDataMgr.ImmediateExclusiveLockType.NoLock)
    {
      LoanDataMgr loanDataMgr = this;
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Initializing LoanDataMgr object...");
      PerformanceMeter ldmCons = PerformanceMeter.Current;
      this.sessionObjects = sessionObjects;
      this.loan = (ILoan) null;
      FastLoanLoadResponse response = (FastLoanLoadResponse) null;
      if (sessionObjects.FastLoanLoad && linkedLoan == null && !this.SessionObjects.AllowConcurrentEditing)
      {
        DateTime dateTime = this.SessionObjects.DDMTrigger != null ? ((DDMTrigger) this.SessionObjects.DDMTrigger).DDMLastModifiedDateTime : DateTime.MinValue;
        DateTime modificationTime1 = this.SessionObjects.LoanConfigurationInfo.FieldRulesModificationTime;
        DateTime modificationTime2 = this.SessionObjects.LoanConfigurationInfo.PrintSelectionModificationTime;
        DateTime modificationTime3 = this.SessionObjects.LoanConfigurationInfo.TriggersModificationTime;
        DateTime modificationTime4 = sessionObjects.LoanConfigurationInfo.RolesModificationTime;
        FastLoanLoadRequest request = new FastLoanLoadRequest()
        {
          Guid = guid,
          ShouldLock = immediateLockType != 0,
          ExclusiveLock = sessionObjects.EnableConcurrentLoanEditing ? LockInfo.ExclusiveLock.NGSharedLock : LockInfo.ExclusiveLock.Nonexclusive,
          ConfigParams = new LoanConfigurationParameters()
          {
            DDMLastModifiedDateTime = dateTime,
            FieldRulesModificationTime = modificationTime1,
            PrintSelectionModificationTime = modificationTime2,
            TriggersModificationTime = modificationTime3,
            RolesModificationTime = modificationTime4
          }
        };
        try
        {
          ldmCons.AddCheckpoint("LoanManager.FastLoanLoad(guid)", 853, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
          this.immediateExclusiveLockType = immediateLockType;
          response = this.LoanManager.FastLoanLoad(request);
          ldmCons.AddCheckpoint("Finished LoanManager.FastLoanLoad", 856, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        }
        catch (Exception ex)
        {
          throw;
        }
        try
        {
          this.loan = response.Proxy;
          this.loanLock = request.ShouldLock ? LoanInfo.LockReason.OpenForWork : LoanInfo.LockReason.NotLocked;
          this.readOnly = !request.ShouldLock;
          this.applyLoanSpecificConfigInfoOnLoanConfigInfo(response.LoanSpecificConfigInfo);
          response.LoanData.Settings = this.SessionObjects.LoanConfigurationInfo.LoanSettings;
          response.LoanData.Settings.DDMLastModifiedDateTime = response.ConfigParameters.DDMLastModifiedDateTime;
          ILoanConfigurationInfo cachedConfigInfo = this.SessionObjects.LoanConfigurationInfo;
          if (cachedConfigInfo.FieldRulesModificationTime < response.ConfigParameters.FieldRulesModificationTime)
          {
            cachedConfigInfo.FieldRules = response.FieldRulesInfo;
            cachedConfigInfo.FieldRulesModificationTime = response.ConfigParameters.FieldRulesModificationTime;
          }
          if (cachedConfigInfo.TriggersModificationTime < response.ConfigParameters.TriggersModificationTime)
          {
            this.SessionObjects.TriggersConfigInfo = new ConfigInfoForTriggers(response.TriggersInfo, this.SessionObjects.LoanConfigurationInfo.MilestonesList, response.ConfigParameters.TriggersModificationTime);
            TriggerCache.GetTriggers(this.SessionObjects);
            cachedConfigInfo.TriggersModificationTime = response.ConfigParameters.TriggersModificationTime;
          }
          if (cachedConfigInfo.PrintSelectionModificationTime < response.ConfigParameters.PrintSelectionModificationTime)
          {
            cachedConfigInfo.PrintSelectionRules = response.PrintSelectionRulesInfo;
            cachedConfigInfo.PrintSelectionModificationTime = response.ConfigParameters.PrintSelectionModificationTime;
          }
          if (cachedConfigInfo.CustomFieldsModificationTime < response.ConfigParameters.CustomFieldsModificationTime)
            Task.Run((Action) (() =>
            {
              ldmCons.AddCheckpoint("Recompiling custom calcs Started..", 894, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
              cachedConfigInfo.LoanSettings.FieldSettings.CustomFields = response.CustomFields;
              CustomCalculationCache.GetFieldCalculators(closure_0.SessionObjects, cachedConfigInfo);
              cachedConfigInfo.CustomFieldsModificationTime = response.ConfigParameters.CustomFieldsModificationTime;
              ldmCons.AddCheckpoint("Recompiling custom calcs Completed..", 898, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
              if (!closure_0.SessionObjects.SkipCustomCalcsExecutionOnLoanOpen)
                return;
              ldmCons.AddCheckpoint("Starting to recalculate custom fields", 901, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
              closure_0.calc.RecalculateCustomFields();
              ldmCons.AddCheckpoint("Finished recalculating custom fields", 903, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
            }));
          if (request.ConfigParams.DDMLastModifiedDateTime < response.ConfigParameters.DDMLastModifiedDateTime)
          {
            ldmCons.AddCheckpoint("Refetched DDM Fee Rules and Fields.", 909, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
            this.SessionObjects.DDMFeeRules = response.DDMFeeRules;
            this.SessionObjects.DDMFieldRules = response.DDMFieldRules;
          }
          if (request.ConfigParams.RolesModificationTime < response.ConfigParameters.RolesModificationTime)
          {
            ldmCons.AddCheckpoint("Refetched Roles.", 916, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
            cachedConfigInfo.AllRoles = response.AllRoles;
            cachedConfigInfo.RolesModificationTime = response.ConfigParameters.RolesModificationTime;
          }
          this.configInfo = this.SessionObjects.LoanConfigurationInfo;
          this.linkedLoan = linkedLoan;
          this.linkLoanLoaded = this.linkedLoan != null;
          this.refreshInternal(true, isExternalOrganization, response);
          this.updateX3wmBase();
          if (this.linkedLoan != null)
          {
            if (this.calc != null)
            {
              this.calc.EditCalcOnDemandEnum(CalcOnDemandEnum.CreateConstructionLinkedSubsetFields, true);
              if (this.linkedLoan.Calculator != null)
                this.linkedLoan.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.CreateConstructionLinkedSubsetFields, true);
            }
          }
        }
        catch (Exception ex)
        {
          if (request.ShouldLock)
            this.loan.Unlock(true, this.SessionObjects.SessionID);
          throw;
        }
        Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "FastLoanLoad LoanDataMgr initialized for loan '" + guid + "'...");
      }
      else
      {
        this.loan = this.LoanManager.OpenLoan(guid);
        ldmCons.AddCheckpoint("LoanManager.OpenLoan(guid)", 948, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        if (this.loan == null)
          throw new ObjectNotFoundException("Loan not found", ObjectType.Loan, (object) guid);
        try
        {
          if (!sessionObjects.EnableConcurrentLoanEditing)
          {
            this.immediateExclusiveLockType = immediateLockType;
            switch (immediateLockType)
            {
              case LoanDataMgr.ImmediateExclusiveLockType.Exclusive:
                this.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.Exclusive);
                break;
              case LoanDataMgr.ImmediateExclusiveLockType.NonExclusive:
                this.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.Nonexclusive);
                break;
            }
          }
          else
            this.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.NGSharedLock);
        }
        catch (Exception ex)
        {
          this.loan.Close();
          throw;
        }
        ldmCons.AddCheckpoint("Finished acquiring the lock on the loan", 972, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        try
        {
          this.pipelineInfoTask = Task.Run<PipelineInfo>((Func<PipelineInfo>) (() => loanDataMgr.loan.GetPipelineInfo(isExternalOrganization, sqlRead)));
          this.loanDataTask = Task.Run<LoanData>((Func<LoanData>) (() => loanDataMgr.loan.GetLoanData(isExternalOrganization)));
          this.loanPropertySettingsTask = Task.Run<LoanProperty[]>((Func<LoanProperty[]>) (() => loanDataMgr.loan.GetLoanPropertySettings()));
          if (linkedLoan != null)
          {
            this.configInfo = linkedLoan.configInfo;
          }
          else
          {
            LoanIdentity identity = this.loan.GetIdentity();
            ldmCons.AddCheckpoint("this.loan.GetIdentity()", 986, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
            this.configInfo = !(identity != (LoanIdentity) null) ? sessionObjects.LoanManager.GetLoanConfigurationInfo(LoanDataMgr.getLoanConfigurationParameters(sessionObjects)) : sessionObjects.LoanManager.GetLoanConfigurationInfo(LoanDataMgr.getLoanConfigurationParameters(sessionObjects), identity.LoanFolder, identity.LoanName);
            ldmCons.AddCheckpoint("sessionObjects.LoanManager.GetLoanConfigurationInfo", 995, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
            if (this.configInfo.UnderwritingConditionTrackingSetup == null)
              this.configInfo.UnderwritingConditionTrackingSetup = new UnderwritingConditionTrackingSetup();
            if (this.configInfo.PostClosingConditionTrackingSetup == null)
              this.configInfo.PostClosingConditionTrackingSetup = new PostClosingConditionTrackingSetup();
            if (this.configInfo.SellConditionTrackingSetup == null)
              this.configInfo.SellConditionTrackingSetup = new SellConditionTrackingSetup();
          }
          this.linkedLoan = linkedLoan;
          this.linkLoanLoaded = this.linkedLoan != null;
          this.Refresh(isExternalOrganization, sqlRead);
          this.updateX3wmBase();
          if (this.linkedLoan != null && this.calc != null)
          {
            this.calc.EditCalcOnDemandEnum(CalcOnDemandEnum.CreateConstructionLinkedSubsetFields, true);
            if (this.linkedLoan.Calculator != null)
              this.linkedLoan.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.CreateConstructionLinkedSubsetFields, true);
          }
          Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr initialized for loan '" + guid + "'...");
        }
        catch (Exception ex)
        {
          if (immediateLockType != LoanDataMgr.ImmediateExclusiveLockType.NoLock)
            this.loan.Unlock(true);
          throw;
        }
      }
    }

    private LoanDataMgr(
      SessionObjects sessionObjects,
      LoanData data,
      string loanFolder,
      string loanName)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Initializing LoanDataMgr object...");
      this.sessionObjects = sessionObjects;
      this.configInfo = sessionObjects.LoanManager.GetLoanConfigurationInfo(LoanDataMgr.getLoanConfigurationParameters(sessionObjects), loanFolder, loanName, data?.GetField("HMDA.X100"));
      if (this.configInfo.UnderwritingConditionTrackingSetup == null)
        this.configInfo.UnderwritingConditionTrackingSetup = new UnderwritingConditionTrackingSetup();
      if (this.configInfo.PostClosingConditionTrackingSetup == null)
        this.configInfo.PostClosingConditionTrackingSetup = new PostClosingConditionTrackingSetup();
      if (this.configInfo.SellConditionTrackingSetup == null)
        this.configInfo.SellConditionTrackingSetup = new SellConditionTrackingSetup();
      this.loanData = data;
      this.readOnly = false;
      this.id = new LoanIdentity(loanFolder, loanName, data.GUID);
      this.rights = new Hashtable();
      this.rights.Add((object) sessionObjects.UserID, (object) LoanInfo.Right.FullRight);
      this.isClosed = false;
      this.LoanFolder = loanFolder;
      this.newLoanName = loanName;
      this.supportingDataList = new SortedList();
      this.InitializeNgLoan();
      this.attachCalculator();
      this.calc.CalculateAll();
      this.updateX3wmBase();
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr initialized for loan '" + loanFolder + "/" + loanName + "'...");
    }

    private static LoanConfigurationParameters getLoanConfigurationParameters(
      SessionObjects sessionObjects)
    {
      try
      {
        if (LoanDataMgr.asyncTriggerCompileTask != null)
          LoanDataMgr.asyncTriggerCompileTask.Wait();
      }
      catch
      {
      }
      return new LoanConfigurationParameters()
      {
        TriggersModificationTime = TriggerCache.GetTriggersModificationTimestamp(sessionObjects),
        FieldRulesModificationTime = FieldValidatorCache.GetRulesModificationTimestamp(sessionObjects),
        PrintSelectionModificationTime = PrintFormSelectorCache.GetFormSelectorsModificationTimestamp(sessionObjects)
      };
    }

    public void Attach(LoanData newData)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.Attach(" + newData.GUID + ")");
      if (this.IsNew())
        throw new InvalidOperationException("Operation only valid on existing loans");
      newData.GUID = this.loanData.GUID;
      this.loanData = newData;
      this.InitializeNgLoan();
      this.attachCalculator();
      this.updateX3wmBase();
    }

    public LoanAccessRules AccessRules => this.accessRules;

    public LoanTriggers Triggers => this.triggers;

    public ILoanConfigurationInfo SystemConfiguration => this.configInfo;

    public InputFormList InputFormSettings => this.formSettings;

    public SessionObjects SessionObjects => this.sessionObjects;

    public bool FromLoanImport
    {
      get => this.fromLoanImport;
      set => this.fromLoanImport = value;
    }

    public string LoanFolder
    {
      get => !this.IsNew() ? this.id.LoanFolder : this.newFolderName;
      set
      {
        if (!this.IsNew())
          throw new InvalidOperationException("Cannot change folder for persistent loan");
        this.newFolderName = value;
        if (this.loanData == null || string.IsNullOrWhiteSpace(value))
          return;
        this.LoanData.SetField("5016", this.GetLoanFolderType() == LoanFolderInfo.LoanFolderType.Archive ? "Y" : "N");
      }
    }

    public LoanFolderInfo.LoanFolderType GetLoanFolderType()
    {
      if (this.sessionObjects == null)
        return LoanFolderInfo.LoanFolderType.NotSpecified;
      if (this.sessionObjects.LoanFolderInfos == null)
        return this.sessionObjects.LoanManager.GetLoanFolderType(this.newFolderName);
      LoanFolderInfo loanFolderInfo = this.sessionObjects.LoanFolderInfos.Find((Predicate<LoanFolderInfo>) (a => a.Name == this.newFolderName));
      return loanFolderInfo != null ? loanFolderInfo.Type : LoanFolderInfo.LoanFolderType.NotSpecified;
    }

    public string LoanName
    {
      get => !this.IsNew() ? this.id.LoanName : this.newLoanName;
      set
      {
        if (!this.IsNew())
          throw new InvalidOperationException("Cannot change name for persistent loan");
        if ((value ?? "") != "")
        {
          LoanIdentity loanIdentity = new LoanIdentity("test", value);
          if (value.Replace(".", "") == "")
            throw new ArgumentException("Loan name contains one or more invalid characters");
        }
        this.newLoanName = value;
      }
    }

    public bool Writable => !this.readOnly;

    public DateTime LastModified => this.lastModified;

    public bool Interactive
    {
      get => this.interactive;
      set => this.interactive = value;
    }

    public FileAttachmentCollection FileAttachments
    {
      get
      {
        if (this.fileAttachments == null)
          this.fileAttachments = new FileAttachmentCollection(this);
        return this.fileAttachments;
      }
    }

    public ImageAttachment GetImageAttachment(string attachmentName)
    {
      if (this.fileAttachments == null)
        this.fileAttachments = new FileAttachmentCollection(this);
      return (ImageAttachment) this.fileAttachments[attachmentName];
    }

    public LoanProperty[] LoanPropertySettings => this.loanPropertySettings;

    public bool UseSkyDrive
    {
      get
      {
        return this.IsNew() ? string.Equals(this.sessionObjects.ConfigurationManager.GetCompanySetting("LoanStorage", "SupportingData"), "SkyDrive", StringComparison.OrdinalIgnoreCase) : this.useSkyDrive;
      }
    }

    public bool UseSkyDriveLite
    {
      get => this.useSkyDriveLite;
      internal set => this.useSkyDriveLite = value;
    }

    public bool UseSkyDriveClassic
    {
      get
      {
        return this.IsNew() ? string.Equals(this.sessionObjects.ConfigurationManager.GetCompanySetting("LoanStorage", "SupportingData"), "SkyDriveClassic", StringComparison.OrdinalIgnoreCase) : this.useSkyDriveClassic;
      }
    }

    public bool IsAutosaveEnabled { get; set; }

    public LoanHistoryManager LoanHistory => this.loanHistory;

    public LoanCalculator Calculator => this.calc;

    public bool IsLoanLocked() => !this.readOnly;

    public void Lock(LoanInfo.LockReason lockReason, LockInfo.ExclusiveLock exclusive)
    {
      this.Lock(lockReason, exclusive, false);
    }

    public void Lock(
      LoanInfo.LockReason lockReason,
      LockInfo.ExclusiveLock exclusive,
      bool addToRecentLoans)
    {
      this.Lock(lockReason, exclusive, addToRecentLoans, true);
    }

    public void Lock(
      LoanInfo.LockReason lockReason,
      LockInfo.ExclusiveLock exclusive,
      bool addToRecentLoans,
      bool includeLinkedLoan)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.Lock(" + (object) lockReason + ", " + (object) exclusive + ", " + addToRecentLoans.ToString() + ", " + includeLinkedLoan.ToString() + ")");
      if (lockReason == LoanInfo.LockReason.NotLocked)
        return;
      this.loan.Lock(lockReason, exclusive, addToRecentLoans);
      this.loanLock = lockReason;
      this.readOnly = false;
      this.InitializeNgLoan();
      try
      {
        if (!includeLinkedLoan || this.LinkedLoan == null)
          return;
        if (this.SessionObjects.AllowConcurrentEditing)
        {
          if (!this.linkedLoan.Writable && exclusive != LockInfo.ExclusiveLock.ReleaseExclusive && exclusive != LockInfo.ExclusiveLock.ReleaseExclusiveA)
            this.linkedLoan.loan.Unlock(true);
          this.linkedLoan.loan.Lock(lockReason, exclusive);
          this.linkedLoan.loanLock = lockReason;
          this.linkedLoan.readOnly = false;
        }
        else
        {
          if (this.linkedLoan.Writable)
            return;
          this.linkedLoan.loan.Unlock(true);
          this.linkedLoan.loan.Lock(lockReason, exclusive);
          this.linkedLoan.loanLock = lockReason;
          this.linkedLoan.readOnly = false;
        }
      }
      catch
      {
        this.Unlock();
        throw;
      }
    }

    public void Unlock() => this.Unlock(false);

    public void Unlock(bool force) => this.Unlock(force, true);

    public void Unlock(bool force, bool unlockLinkedLoan)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.Unlock()");
      if (!force && this.loanLock == LoanInfo.LockReason.NotLocked)
        return;
      try
      {
        this.loan.Unlock(force);
      }
      catch (LockException ex)
      {
      }
      this.loanLock = LoanInfo.LockReason.NotLocked;
      this.readOnly = true;
      if (this.LinkedLoan == null || !(this.linkedLoan.Writable & unlockLinkedLoan))
        return;
      this.linkedLoan.Unlock();
    }

    public ILoan LoanObject => this.loan;

    public LockInfo[] CurrentLocks => this.loan.CurrentLocks;

    public LockInfo[] GetCurrentLocks()
    {
      if (!this.IsNew())
        return this.loan.GetAllLockInfo();
      return new LockInfo[1]
      {
        new LockInfo(this.loanData.GUID, this.sessionObjects.UserID, this.sessionObjects.UserInfo.FirstName, this.sessionObjects.UserInfo.LastName, this.sessionObjects.Session.SessionID, this.sessionObjects.Session.LoginParams.Server.ToString(), LoanInfo.LockReason.OpenForWork, DateTime.Now, LockInfo.ExclusiveLock.Nonexclusive)
      };
    }

    public LockInfo GetCurrentLock() => this.GetCurrentLocks()?[0];

    private void loadLinkedLoan(bool isExternalOrganization)
    {
      if (this.linkLoanLoaded)
        return;
      this.linkLoanLoaded = true;
      this.linkedLoan = (LoanDataMgr) null;
      if (this.loanData == null)
        return;
      if ((this.loanData.LinkGUID ?? "") == "")
        return;
      try
      {
        this.linkedLoan = LoanDataMgr.OpenLoan(this.sessionObjects, this.loanData.LinkGUID, this, isExternalOrganization, this.immediateExclusiveLockType);
        this.loanData.LinkedData = this.linkedLoan.loanData;
        this.linkedLoan.loanData.LinkedData = this.loanData;
        this.loanData.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.CreateConstructionLinkedSubsetFields, true);
        if (this.linkedLoan == null || this.linkedLoan.Calculator == null)
          return;
        this.linkedLoan.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.CreateConstructionLinkedSubsetFields, true);
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Warning, "Unable to load linked loan '" + this.loanData.LinkGUID + "': " + ex.Message);
        this.linkedLoan = (LoanDataMgr) null;
      }
    }

    public LoanDataMgr LinkedLoan
    {
      get
      {
        this.loadLinkedLoan(false);
        return this.linkedLoan;
      }
    }

    public void LinkTo(LoanDataMgr mgr)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.LinkTo()");
      if (this.readOnly)
        throw new InvalidOperationException("Operation cannot be performed on an unlocked loan");
      if (mgr.readOnly)
        throw new InvalidOperationException("The specified loan must be locked in order to establish a link");
      if (this.loanData.GUID == mgr.loanData.GUID)
        throw new InvalidOperationException("Cannot link a loan to itself");
      this.loanData.LinkedData = mgr.loanData;
      mgr.loanData.LinkedData = this.loanData;
      this.linkedLoan = mgr;
      mgr.linkedLoan = this;
      this.Calculator.FormCalculation("LINKLOAN", (string) null, (string) null);
    }

    public void Unlink()
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.Unlink()");
      if (this.readOnly)
        throw new InvalidOperationException("Operation cannot be performed on an unlocked loan");
      if (this.loanData.LinkGUID == "")
        return;
      if (this.LinkedLoan != null)
      {
        this.linkedLoan.loanData.LinkedData = (LoanData) null;
        this.linkedLoan.linkedLoan = (LoanDataMgr) null;
      }
      this.linkedLoan.Unlock(false, false);
      this.loanData.LinkedData = (LoanData) null;
      this.linkedLoan = (LoanDataMgr) null;
    }

    public void Refresh(bool isExternalOrganization)
    {
      this.refreshInternal(true, isExternalOrganization, 0);
    }

    public void Refresh(bool isExternalOrganization, int sqlRead)
    {
      this.refreshInternal(true, isExternalOrganization, sqlRead);
    }

    private void refreshInternal(bool refreshLinkedLoan, bool isExternalOrganization)
    {
      this.refreshInternal(refreshLinkedLoan, isExternalOrganization, 0);
    }

    private void refreshInternal(
      bool refreshLinkedLoan,
      bool isExternalOrganization,
      FastLoanLoadResponse response)
    {
      this.loanData = response.LoanData;
      if (this.loanData == null)
        throw new ObjectNotFoundException("Loan not found", ObjectType.Loan, (object) this.id.Guid);
      this.loanPropertySettings = response.LoanProperties;
      this.InitializeNgLoan();
      this.commonRefreshInternal(true, isExternalOrganization, PerformanceMeter.Current, response.PipelineInfo);
    }

    private void refreshInternal(bool refreshLinkedLoan, bool isExternalOrganization, int sqlRead)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.refreshInternal(" + refreshLinkedLoan.ToString() + ")");
      PerformanceMeter current = PerformanceMeter.Current;
      if (this.loan == null)
        throw new InvalidOperationException("Operation only valid for existing loan");
      current.AddCheckpoint("Started waiting for GetLoanData and GetPipelineInfo and GetLoanPropertySettings.", 1592, nameof (refreshInternal), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      if (this.pipelineInfoTask == null)
        this.pipelineInfoTask = Task.Run<PipelineInfo>((Func<PipelineInfo>) (() => this.loan.GetPipelineInfo(isExternalOrganization, sqlRead)));
      if (this.loanDataTask == null)
        this.loanDataTask = Task.Run<LoanData>((Func<LoanData>) (() => this.loan.GetLoanData(isExternalOrganization)));
      if (this.loanPropertySettingsTask == null)
        this.loanPropertySettingsTask = Task.Run<LoanProperty[]>((Func<LoanProperty[]>) (() => this.loan.GetLoanPropertySettings()));
      if (this.BeforeLoanRefreshedFromServer != null)
        this.BeforeLoanRefreshedFromServer((object) this, new EventArgs());
      this.loanData = this.loanDataTask.Result;
      this.loanDataTask = (Task<LoanData>) null;
      this.InitializeNgLoan();
      if (this.loanData == null)
        throw new ObjectNotFoundException("Loan not found", ObjectType.Loan, (object) this.id.Guid);
      PipelineInfo result = this.pipelineInfoTask.Result;
      this.pipelineInfoTask = (Task<PipelineInfo>) null;
      this.loanPropertySettings = this.loanPropertySettingsTask.Result;
      this.loanPropertySettingsTask = (Task<LoanProperty[]>) null;
      current.AddCheckpoint("Finished waiting for GetLoanData, GetPipelineInfo and GetLoanPropertySettings.", 1624, nameof (refreshInternal), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      this.commonRefreshInternal(refreshLinkedLoan, isExternalOrganization, current, result);
    }

    private void commonRefreshInternal(
      bool refreshLinkedLoan,
      bool isExternalOrganization,
      PerformanceMeter refreshMeter,
      PipelineInfo pData)
    {
      this.id = pData.Identity;
      this.rights = pData.Rights;
      this.lastModified = pData.LastModified;
      this.useSkyDrive = false;
      this.useSkyDriveLite = false;
      this.useSkyDriveClassic = false;
      foreach (LoanProperty loanPropertySetting in this.loanPropertySettings)
      {
        if (string.Equals(loanPropertySetting.Category, "LoanStorage", StringComparison.OrdinalIgnoreCase) && string.Equals(loanPropertySetting.Attribute, "SupportingData", StringComparison.OrdinalIgnoreCase))
        {
          if (string.Equals(loanPropertySetting.Value, "SkyDrive", StringComparison.OrdinalIgnoreCase))
          {
            this.useSkyDrive = true;
            break;
          }
          if (string.Equals(loanPropertySetting.Value, "SkyDriveLite", StringComparison.OrdinalIgnoreCase))
          {
            this.useSkyDriveLite = true;
            break;
          }
          if (string.Equals(loanPropertySetting.Value, "SkyDriveClassic", StringComparison.OrdinalIgnoreCase))
          {
            this.useSkyDriveClassic = true;
            break;
          }
          break;
        }
      }
      try
      {
        EllieMae.EMLite.DataEngine.Log.MilestoneLog milestone = this.loanData.GetLogList().GetMilestone("Completion");
        refreshMeter.AddCheckpoint("Finish calling loanData.GetLogList().GetMilestone()", 1668, nameof (commonRefreshInternal), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        this.isClosed = milestone.Done;
      }
      catch
      {
        this.isClosed = false;
      }
      this.attachCalculator();
      this.setWatchListFields();
      if (refreshLinkedLoan && this.LinkedLoan != null)
      {
        refreshMeter.AddCheckpoint("Starting refreshInternal on linked loan", 1686, nameof (commonRefreshInternal), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        this.linkedLoan.refreshInternal(false, isExternalOrganization);
        refreshMeter.AddCheckpoint("Finished refreshInternal on linked loan", 1688, nameof (commonRefreshInternal), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      }
      if (this.OnLoanRefreshedFromServer == null)
        return;
      this.OnLoanRefreshedFromServer((object) this, new EventArgs());
    }

    public LoanData LoanData => this.loanData;

    public bool ValidationsEnabled
    {
      get => this.validator.Enabled;
      set => this.validator.Enabled = value;
    }

    public List<string> FailedValidationListFieldIds => this.validator.FailedValidationListFieldIds;

    public bool Dirty
    {
      get
      {
        if (this.loanData.Dirty)
          return true;
        return this.linkLoanLoaded && this.linkedLoan != null && this.linkedLoan.LoanData.Dirty;
      }
    }

    public string[] SelectFields(string[] fieldIds) => this.loan.SelectFields(fieldIds);

    public BinaryObject GetSupportingData(string key)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.GetSupportingData(" + key + ") starts at " + DateTime.Now.ToString());
      lock (this)
      {
        BinaryObject supportingData1 = (BinaryObject) null;
        if (this.loan == null)
        {
          if (this.supportingDataList.Contains((object) key))
            supportingData1 = (BinaryObject) this.supportingDataList[(object) key];
        }
        else if ((this.useSkyDrive || this.useSkyDriveLite || this.useSkyDriveClassic) && !EllieMae.EMLite.Common.Utils.IsCIFsOnlyFile(key))
        {
          if (this.skyDriveFileTasks.ContainsKey(key))
          {
            Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Checking file streaming task: " + key);
            try
            {
              Task.WaitAll(this.skyDriveFileTasks[key]);
              this.skyDriveFileTasks.Remove(key);
            }
            catch (Exception ex)
            {
              Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Error, "File streaming failed for " + key + ": " + ex.ToString());
            }
          }
          Task<BinaryObject> supportingData2 = new SkyDriveStreamingClient(this).GetSupportingData(key);
          Task.WaitAll((Task) supportingData2);
          supportingData1 = supportingData2.Result;
        }
        else
          supportingData1 = this.loan.GetSupportingDataOnCIFs(key);
        Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.GetSupportingData(" + key + ") ends at " + DateTime.Now.ToString(), supportingData1 != null ? supportingData1.Length : 0L);
        return supportingData1;
      }
    }

    public SnapshotObject GetSupportingSnapshotData(
      LogSnapshotType type,
      Guid snapshotGuid,
      string fileNameAsKey)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.GetSupportingSnapshotData(" + (object) snapshotGuid + "," + fileNameAsKey + ")");
      lock (this)
      {
        if (this.loan == null)
        {
          if (!this.supportingDataList.Contains((object) fileNameAsKey))
            return (SnapshotObject) null;
          SnapshotObject supportingData = (SnapshotObject) this.supportingDataList[(object) fileNameAsKey];
          Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.GetSupportingSnapshotData(" + (object) snapshotGuid + "," + fileNameAsKey + ") ends at " + DateTime.Now.ToString(), supportingData != null ? supportingData.Length : 0L);
          return supportingData;
        }
        SnapshotObject supportingSnapshotData = this.loan.GetSupportingSnapshotData(type, snapshotGuid, fileNameAsKey);
        Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.GetSupportingSnapshotData(" + (object) snapshotGuid + "," + fileNameAsKey + ") ends at " + DateTime.Now.ToString(), supportingSnapshotData != null ? supportingSnapshotData.Length : 0L);
        return supportingSnapshotData;
      }
    }

    public BinaryObject GetSupportingLinkedData(string loanFolder, string loanName, string key)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.GetSupportingLinkedData(" + key + ") starts at " + DateTime.Now.ToString());
      lock (this)
      {
        BinaryObject supportingLinkedData = (BinaryObject) null;
        if (this.loan == null)
        {
          if (this.supportingDataList.Contains((object) key))
            supportingLinkedData = (BinaryObject) this.supportingDataList[(object) key];
        }
        else if (this.LinkedLoan != null)
          supportingLinkedData = this.LinkedLoan.GetSupportingData(key);
        Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.GetSupportingLinkedData(" + key + ") ends at " + DateTime.Now.ToString(), supportingLinkedData != null ? supportingLinkedData.Length : 0L);
        return supportingLinkedData;
      }
    }

    public void SaveSupportingSnapshotData(
      LogSnapshotType type,
      Guid snapshotGuid,
      string fileNameAsKey,
      SnapshotObject data)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.SaveSupportingSnapshotData(" + fileNameAsKey + ") starts at " + DateTime.Now.ToString(), data != null ? data.Length : 0L);
      lock (this)
      {
        if (this.loan == null)
        {
          if (this.supportingDataList.Contains((object) fileNameAsKey))
            this.supportingDataList.Remove((object) fileNameAsKey);
          if (data != null)
            this.supportingDataList.Add((object) fileNameAsKey, (object) data);
        }
        else
          this.loan.SaveSupportingSnapshotData(type, snapshotGuid, fileNameAsKey, data);
      }
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.SaveSupportingSnapshotData(" + fileNameAsKey + ") ends at " + DateTime.Now.ToString());
    }

    public void SaveSupportingData(string key, BinaryObject data)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.SaveSupportingData(" + key + ") starts at " + DateTime.Now.ToString(), data != null ? data.Length : 0L);
      lock (this)
      {
        if (this.loan == null)
        {
          if (this.supportingDataList.Contains((object) key))
            this.supportingDataList.Remove((object) key);
          if (data != null)
            this.supportingDataList.Add((object) key, (object) data);
        }
        else if ((this.useSkyDrive || this.useSkyDriveLite || this.useSkyDriveClassic) && !EllieMae.EMLite.Common.Utils.IsCIFsOnlyFile(key))
        {
          if (data != null)
          {
            if (this.skyDriveFileTasks.ContainsKey(key))
            {
              Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Checking file streaming task: " + key);
              try
              {
                Task.WaitAll(this.skyDriveFileTasks[key]);
              }
              catch (Exception ex)
              {
                Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Error, "File streaming failed for " + key + ": " + ex.ToString());
              }
              finally
              {
                this.skyDriveFileTasks.Remove(key);
              }
            }
            Task.WaitAll((Task) new SkyDriveStreamingClient(this).SaveSupportingData(key, data));
          }
        }
        else
          this.loan.SaveSupportingDataOnCIFs(key, data);
      }
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.SaveSupportingData(" + key + ") ends at " + DateTime.Now.ToString());
    }

    public void SaveSupportingLinkedData(
      string loanFolder,
      string loanName,
      string key,
      BinaryObject data)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.SaveSupportingLinkedData(" + key + ")starts at " + DateTime.Now.ToString(), data != null ? data.Length : 0L);
      lock (this)
      {
        if (this.loan == null)
        {
          if (this.supportingDataList.Contains((object) key))
            this.supportingDataList.Remove((object) key);
          if (data != null)
            this.supportingDataList.Add((object) key, (object) data);
        }
        else if (this.LinkedLoan != null)
          this.LinkedLoan.SaveSupportingData(key, data);
      }
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.SaveSupportingLinkedData(" + key + ")ends at " + DateTime.Now.ToString());
    }

    public void DeleteSupportingData(string key)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.DeleteSupportingData(" + key + ")");
      lock (this)
      {
        if (this.loan == null)
        {
          if (!this.supportingDataList.Contains((object) key))
            return;
          this.supportingDataList.Remove((object) key);
        }
        else
        {
          if (!EllieMae.EMLite.Common.Utils.IsCIFsOnlyFile(key))
            return;
          this.loan.DeleteSupportingDataOnCIFs(key);
        }
      }
    }

    public void DeleteSupportingData(string[] keys)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.DeleteSupportingData(" + (object) keys.Length + ")");
      lock (this)
      {
        if (this.loan == null)
        {
          foreach (string key in keys)
          {
            if (this.supportingDataList.Contains((object) key))
              this.supportingDataList.Remove((object) key);
          }
        }
        else
          this.loan.DeleteSupportingDataOnCIFs(((IEnumerable<string>) keys).Where<string>((Func<string, bool>) (key => EllieMae.EMLite.Common.Utils.IsCIFsOnlyFile(key))).ToArray<string>());
      }
    }

    public void AppendSupportingData(string key, BinaryObject data)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.AppendSupportingDataOnCIFs(" + key + ")");
      lock (this)
      {
        if (this.loan == null)
        {
          if (this.supportingDataList.Contains((object) key))
          {
            BinaryObject supportingData = (BinaryObject) this.supportingDataList[(object) key];
            this.supportingDataList[(object) key] = (object) supportingData.Append(data);
          }
          else
            this.supportingDataList[(object) key] = (object) data;
        }
        else if ((this.useSkyDrive || this.useSkyDriveLite || this.useSkyDriveClassic) && !EllieMae.EMLite.Common.Utils.IsCIFsOnlyFile(key))
        {
          using (BinaryObject supportingData = this.GetSupportingData(key))
          {
            if (supportingData != null)
            {
              using (BinaryObject data1 = supportingData.Append(data))
                this.SaveSupportingData(key, data1);
            }
            else
              this.SaveSupportingData(key, data);
          }
        }
        else
          this.loan.AppendSupportingDataOnCIFs(key, data);
      }
    }

    public Dictionary<string, string> GetLoanSnapshot(
      LogSnapshotType type,
      Guid snapshotGuid,
      bool ucdExists)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.GetLoanSnapShot(" + (object) snapshotGuid + ")");
      lock (this)
        return this.loan != null ? this.loan.GetLoanSnapshot(type, snapshotGuid, ucdExists) : new Dictionary<string, string>();
    }

    public Dictionary<string, Dictionary<string, string>> GetLoanSnapshots(
      LogSnapshotType type,
      Dictionary<string, bool> snapshotGuids)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.GetLoanSnapshots(" + (object) type + ")");
      lock (this)
        return this.loan != null ? this.loan.GetLoanSnapshots(type, snapshotGuids) : new Dictionary<string, Dictionary<string, string>>();
    }

    public bool SupportingDataExists(string key)
    {
      lock (this)
      {
        if (this.loan == null)
          return this.supportingDataList.Contains((object) key);
        return (this.useSkyDrive || this.useSkyDriveLite || this.useSkyDriveClassic) && !EllieMae.EMLite.Common.Utils.IsCIFsOnlyFile(key) ? this.loan.GetSkyDriveUrlForGet(key) != null : this.loan.SupportingDataExistsOnCIFs(key);
      }
    }

    public void ExportLoan(string zipFile)
    {
      Tracing.Log(LoanDataMgr.sw, TraceLevel.Verbose, nameof (LoanDataMgr), "ExportLoan: zipFile(" + zipFile + ") starts at" + DateTime.Now.ToString());
      string str = SystemSettings.TempFolderRoot + this.loan.Guid + "\\";
      if (Directory.Exists(str))
        Directory.Delete(str, true);
      Directory.CreateDirectory(str);
      string path1 = str + "loan.em";
      using (BinaryObject binaryObject = this.loan.Export())
        binaryObject.Write(path1);
      List<string> stringList = new List<string>((IEnumerable<string>) this.loan.GetSupportingDataKeysOnCIFs());
      if (this.useSkyDrive || this.useSkyDriveLite || this.useSkyDriveClassic)
      {
        string[] supportingDataKeys = this.loan.GetSkyDriveSupportingDataKeys();
        stringList.AddRange((IEnumerable<string>) supportingDataKeys);
      }
      foreach (string key in stringList)
      {
        string path2 = str + key;
        using (BinaryObject supportingData = this.GetSupportingData(key))
          supportingData?.Write(path2);
      }
      FileCompressor.Instance.ZipDirectory(str, zipFile);
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "ExportLoan: zipFile(" + zipFile + ") ends at" + DateTime.Now.ToString());
    }

    public void ImportLoan(string zipFile, bool isExternalOrganization)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "ImportLoan: zipFile(" + zipFile + ") starts at" + DateTime.Now.ToString());
      string str = SystemSettings.TempFolderRoot + this.loanData.GUID + "\\";
      if (Directory.Exists(str))
        Directory.Delete(str, true);
      FileCompressor.Instance.Unzip(zipFile, str);
      using (BinaryObject binaryObject = new BinaryObject(str + "loan.em"))
      {
        Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "ImportLoanDataFile:", binaryObject != null ? binaryObject.Length : 0L);
        if (this.loan != null)
        {
          LoanInfo.LockReason loanLock = this.loanLock;
          if (loanLock != LoanInfo.LockReason.Downloaded)
          {
            this.Unlock();
            this.Lock(LoanInfo.LockReason.Downloaded, LockInfo.ExclusiveLock.Exclusive);
          }
          this.loan.Import(binaryObject, this.LoanFolder);
          if (loanLock != LoanInfo.LockReason.Downloaded)
          {
            this.Unlock();
            this.Lock(loanLock, LockInfo.ExclusiveLock.Exclusive);
          }
        }
        else
        {
          this.loan = this.LoanManager.Import(this.LoanFolder, this.LoanName, binaryObject, DuplicateLoanAction.Rename);
          this.loanLock = this.loan.GetLockInfo().LockedFor;
        }
      }
      foreach (string file in Directory.GetFiles(str))
      {
        string fileName = Path.GetFileName(file);
        if (fileName.ToLower() != "loan.em".ToLower())
        {
          using (BinaryObject data = new BinaryObject(file))
          {
            Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "ImportLoan: zipFile(" + zipFile + ")", data != null ? data.Length : 0L);
            this.SaveSupportingData(fileName, data);
            if (fileName.ToLower() == "attachments.xml")
              this.fileAttachments = (FileAttachmentCollection) null;
          }
        }
      }
      this.Refresh(isExternalOrganization);
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "ImportLoan: zipFile(" + zipFile + ") ends at" + DateTime.Now.ToString());
    }

    public void AddToRecentLoans()
    {
      if (this.IsNew())
        return;
      this.loan.AddToRecentLoans();
    }

    public void Move(string loanFolder, string loanName, DuplicateLoanAction dupAction)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.Move()");
      if (this.loan == null)
        throw new InvalidOperationException("Operation only valid for existing loan");
      this.loan.Move(loanFolder, loanName, dupAction);
      this.id = this.loan.GetIdentity();
    }

    public void AddLoanEventLogs(LoanEventLogList loanEventLog)
    {
      if (this.IsNew())
        throw new Exception("Loan must be saved prior to saving operational logs");
      this.loan.AddLoanEventLog(loanEventLog);
    }

    public UserShortInfoList GetUsersWorkingOnTheLoan(
      string excludeSessionID,
      bool useLoanAssociatesCache)
    {
      List<UserShortInfo> userShortInfoList = new List<UserShortInfo>();
      EllieMae.EMLite.ClientServer.LoanAssociateInfo[] loanAssociateInfoArray = (EllieMae.EMLite.ClientServer.LoanAssociateInfo[]) null;
      if (this.loan != null)
      {
        if (useLoanAssociatesCache && this.loanAssociatesCache.ContainsKey(this.loan.Guid))
        {
          loanAssociateInfoArray = this.loanAssociatesCache[this.loan.Guid];
        }
        else
        {
          loanAssociateInfoArray = this.loan.GetLoanAssociates(false);
          this.loanAssociatesCache[this.loan.Guid] = loanAssociateInfoArray;
        }
      }
      LockInfo[] currentLocks = this.GetCurrentLocks();
      if (currentLocks != null && currentLocks.Length != 0)
      {
        foreach (LockInfo lockInfo in currentLocks)
        {
          if (lockInfo.CurrentlyLoggedOn != LockInfo.LockOwnerLoggedOn.False && (excludeSessionID == null || !(lockInfo.LoginSessionID == excludeSessionID)) && lockInfo.LockedFor != LoanInfo.LockReason.NotLocked)
          {
            List<string> stringList = new List<string>();
            if (loanAssociateInfoArray != null)
            {
              foreach (EllieMae.EMLite.ClientServer.LoanAssociateInfo loanAssociateInfo in loanAssociateInfoArray)
              {
                if (loanAssociateInfo.AssociateUserID == lockInfo.LockedBy)
                  stringList.Add(loanAssociateInfo.RoleAbbr);
              }
            }
            userShortInfoList.Add(new UserShortInfo(lockInfo.LoginSessionID, lockInfo.LockedBy, lockInfo.LockedByFirstName, lockInfo.LockedByLastName, lockInfo.Exclusive, stringList.ToArray()));
          }
        }
      }
      return new UserShortInfoList(userShortInfoList.ToArray());
    }

    private ConcurrentEditingDialog.Actions xml3WayMerge(
      ConcurrentEditingDialog.Actions action,
      bool mergeOnly,
      bool interactive,
      out bool loanFileMerged,
      out Exception exEncountered)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.xml3WayMerge(" + (object) action + ", " + mergeOnly.ToString() + ", " + interactive.ToString() + ")");
      exEncountered = (Exception) null;
      loanFileMerged = false;
      bool flag1 = true;
      if (action == ConcurrentEditingDialog.Actions.Cancel)
        flag1 = false;
      if (!mergeOnly)
      {
        if (interactive)
        {
          if (action == ConcurrentEditingDialog.Actions.Cancel || action == ConcurrentEditingDialog.Actions.MergeAndShowResult)
            action = ConcurrentEditingDialog.Actions.MergeAndShowResult;
        }
        else
          action = ConcurrentEditingDialog.Actions.MergeAndSave;
        if (action == ConcurrentEditingDialog.Actions.Cancel)
          return ConcurrentEditingDialog.Actions.Cancel;
      }
      else
        action = ConcurrentEditingDialog.Actions.Merge;
      LoanMergeTool loanMergeTool = new LoanMergeTool();
      try
      {
        loanMergeTool.Merge(this, false);
        this.loanData.PopulateLatestSubmissionAusTracking();
      }
      catch (MergeException ex)
      {
        string str1 = "Loan file merge failed: " + ex.Message;
        if (!interactive)
          throw new Exception(str1);
        int num1 = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) System.Windows.Forms.Form.ActiveForm, str1, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        folderBrowserDialog.Description = "Please select a folder to save debugging information to.";
        if (DialogResult.Cancel == folderBrowserDialog.ShowDialog((IWin32Window) System.Windows.Forms.Form.ActiveForm))
          return ConcurrentEditingDialog.Actions.Cancel;
        LoanDataFormatterUtils dataFormatterUtils = new LoanDataFormatterUtils();
        string selectedPath = folderBrowserDialog.SelectedPath;
        string str2 = Path.Combine(selectedPath, this.loanData.GUID);
        if (Directory.Exists(str2))
          Directory.Delete(str2, true);
        Directory.CreateDirectory(str2);
        string path1 = Path.Combine(str2, "base.em");
        string path2 = Path.Combine(str2, "first.em");
        string path3 = Path.Combine(str2, "second.em");
        string path4 = Path.Combine(str2, "log.em");
        using (FileStream fileStream = new FileStream(path1, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
        {
          byte[] buffer = dataFormatterUtils.Serialize(ex.BaseDoc);
          fileStream.Write(buffer, 0, buffer.Length);
        }
        using (FileStream fileStream = new FileStream(path2, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
        {
          byte[] buffer = dataFormatterUtils.Serialize(ex.FirstDoc);
          fileStream.Write(buffer, 0, buffer.Length);
        }
        using (FileStream fileStream = new FileStream(path3, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
        {
          byte[] buffer = dataFormatterUtils.Serialize(ex.SecondDoc);
          fileStream.Write(buffer, 0, buffer.Length);
        }
        using (FileStream fileStream = new FileStream(path4, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
        {
          string rawData = "Message:" + Environment.NewLine + ex.Message + Environment.NewLine + "Stack Trace" + Environment.NewLine + ex.StackTrace;
          byte[] buffer = dataFormatterUtils.Serialize(rawData);
          fileStream.Write(buffer, 0, buffer.Length);
        }
        string str3 = Path.Combine(selectedPath, this.loan.Guid + ".zip");
        if (System.IO.File.Exists(str3))
          System.IO.File.Delete(str3);
        FileCompressor.Instance.ZipDirectory(str2, str3);
        if (Directory.Exists(str2))
          Directory.Delete(str2, true);
        int num2 = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) System.Windows.Forms.Form.ActiveForm, "Debugging information have been saved to '" + str3 + "'", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return ConcurrentEditingDialog.Actions.Cancel;
      }
      catch (Exception ex)
      {
        string str = "Unexpected error during merge: " + ex.Message + "\r\n\r\nLoan cannot be saved.";
        exEncountered = ex;
        if (!interactive)
          throw new Exception(str);
        int num = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, str);
        if (ex is SecurityException)
          this.Unlock(true);
        return ConcurrentEditingDialog.Actions.Cancel;
      }
      DialogResult dialogResult = DialogResult.OK;
      if (action == ConcurrentEditingDialog.Actions.MergeAndShowResult || mergeOnly & interactive)
      {
        bool flag2 = true;
        if (mergeOnly)
          flag2 = interactive;
        if (flag2)
        {
          List<MergedObject> piggyBackLoanReport = (List<MergedObject>) null;
          if (loanMergeTool.LinkedLoanMergedResult != null)
            piggyBackLoanReport = loanMergeTool.LinkedLoanMergedResult.MergedReport;
          if (this.hideChangesMadeByOthers == "")
            this.hideChangesMadeByOthers = this.sessionObjects.ConfigurationManager.GetCompanySetting("Policies", "HideChangesMadeByOthers");
          if (this.mergeResultsOption == CEMergeResultsOption.Null)
            this.mergeResultsOption = (CEMergeResultsOption) this.sessionObjects.ServerManager.GetServerSetting("ConcurrentEditing.MergeResultsOption");
          if (this.mergeResultsOption == CEMergeResultsOption.NotifyConflictsOnly)
          {
            MergeResultForm mergeResultForm = new MergeResultForm(loanMergeTool.LoanMergedResult.MergedReport, piggyBackLoanReport, this.GetHiddenFields(), action != ConcurrentEditingDialog.Actions.Merge, !(this.hideChangesMadeByOthers == "True"), loanMergeTool.LoanMergedResult.BaseLoanDataXml, loanMergeTool.LoanMergedResult.FirstLoanDataXml, loanMergeTool.LoanMergedResult.SecondLoanDataXml, this.mergeResultsOption);
            if (mergeOnly)
              mergeResultForm.MergeOnly = true;
            if (!mergeResultForm.NoConflicts)
            {
              int num = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, "Some of your changes will overwrite the changes made by others while you were editing this loan.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
          }
          else if (this.mergeResultsOption != CEMergeResultsOption.None)
          {
            if (this.mergeResultsOption == CEMergeResultsOption.ConflictsOnly)
              this.hideChangesMadeByOthers = "True";
            using (MergeResultForm mergeResultForm = new MergeResultForm(loanMergeTool.LoanMergedResult.MergedReport, piggyBackLoanReport, this.GetHiddenFields(), action != ConcurrentEditingDialog.Actions.Merge, !(this.hideChangesMadeByOthers == "True"), loanMergeTool.LoanMergedResult.BaseLoanDataXml, loanMergeTool.LoanMergedResult.FirstLoanDataXml, loanMergeTool.LoanMergedResult.SecondLoanDataXml, this.mergeResultsOption))
            {
              if (mergeOnly)
                mergeResultForm.MergeOnly = true;
              if (this.hideChangesMadeByOthers == "True")
              {
                if (!mergeResultForm.HasConflictKeyChanges)
                  goto label_75;
              }
              dialogResult = mergeResultForm.ShowDialog();
            }
          }
        }
      }
label_75:
      if (dialogResult == DialogResult.Cancel)
      {
        string str = "Your changes have been merged with others but have not been saved yet.";
        if (interactive & flag1)
        {
          int num = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, str, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
          Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Warning, str);
        return ConcurrentEditingDialog.Actions.Cancel;
      }
      using (Stream utf8StreamWithBom = loanMergeTool.LoanMergedResult.MergedLoanDataXml.ToUtf8StreamWithBOM())
      {
        this.ReplaceLoanDataXml(utf8StreamWithBom, loanMergeTool.LoanMergedResult.NewBaseLastModTime, loanMergeTool.LoanMergedResult.NewBaseLoanDataXml, true);
        if (this.LinkedLoan != null)
        {
          this.LinkedLoan.ReplaceLoanDataXml(utf8StreamWithBom, loanMergeTool.LinkedLoanMergedResult.NewBaseLastModTime, loanMergeTool.LinkedLoanMergedResult.NewBaseLoanDataXml, true);
          this.LinkTo(this.LinkedLoan);
        }
      }
      loanFileMerged = true;
      return action;
    }

    public ILoanMilestoneTemplateOrchestrator DefaultMilestoneTemplateHandler
    {
      get => this.milestoneTemplateController;
      set => this.milestoneTemplateController = value;
    }

    public void SaveWithoutAuditRecord(LoanData loanData)
    {
      this.loan.SaveWithoutAuditRecord(loanData);
    }

    public bool SaveLoanInteractive()
    {
      return this.SaveLoan(true, this.milestoneTemplateController, false);
    }

    public bool Save() => this.SaveLoan(false, this.milestoneTemplateController, false);

    public bool SaveLoan(
      bool interactive,
      ILoanMilestoneTemplateOrchestrator milestoneTemplateController,
      bool isExternalOrganization)
    {
      return this.SaveLoan(false, interactive, false, milestoneTemplateController, isExternalOrganization, out bool _);
    }

    public bool SaveLoan(
      bool mergeOnly,
      bool interactive,
      bool sdkSave,
      ILoanMilestoneTemplateOrchestrator milestoneTemplateController,
      bool isExternalOrganization,
      out bool loanFileMerged,
      bool enableRateLockValidation = false,
      bool enableBackupLoanFile = false)
    {
      return this.SaveLoan(mergeOnly, interactive, sdkSave, true, milestoneTemplateController, isExternalOrganization, out loanFileMerged, enableRateLockValidation, enableBackupLoanFile);
    }

    public bool SaveLoan(
      bool mergeOnly,
      bool interactive,
      bool sdkSave,
      bool saveLinkedLoan,
      ILoanMilestoneTemplateOrchestrator milestoneTemplateController,
      bool isExternalOrganization,
      out bool loanFileMerged,
      bool enableRateLockValidation = false,
      bool enableBackupLoanFile = false)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 2518, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        PerformanceMeter.Current.AddCheckpoint(string.Format("PARAMS - mergeOnly: {0}, interactive: {1}, sdkSave: {2}, saveLinkedLoan: {3}, isExternalOrganization: {4}, enableRateLockValidation: {5}, enableBackupLoanFile: {6}", (object) mergeOnly, (object) interactive, (object) sdkSave, (object) saveLinkedLoan, (object) isExternalOrganization, (object) enableRateLockValidation, (object) enableBackupLoanFile), 2519, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.SaveLoan(" + mergeOnly.ToString() + ", " + interactive.ToString() + ", " + sdkSave.ToString() + ", " + saveLinkedLoan.ToString() + ")");
        loanFileMerged = false;
        if (interactive)
          Cursor.Current = Cursors.WaitCursor;
        using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("LoanDataMgr.DDMTriggerExecute", "DDM Execution", true, 2528, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs"))
        {
          this.DDMTriggerExecute(DDMStartPopulationTrigger.LoanSave, false);
          performanceMeter.Stop();
        }
        if (this.LoanData.Calculator != null)
        {
          PerformanceMeter.Current.AddCheckpoint("Starting Calculator.CalcOnDemand()", 2537, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
          Stopwatch stopwatch = new Stopwatch();
          stopwatch.Start();
          LoanActivityEventHandler beforeLoanActivity1 = this.BeforeLoanActivity;
          if (beforeLoanActivity1 != null)
            beforeLoanActivity1((object) this, LoanActivityEventArgs.Before(EllieMae.EMLite.LoanUtils.DataEngine.LoanActivityType.CalcOnDemand));
          stopwatch.Stop();
          this.LoanData.Calculator.CalcOnDemand();
          LoanActivityEventHandler afterLoanActivity1 = this.AfterLoanActivity;
          if (afterLoanActivity1 != null)
            afterLoanActivity1((object) this, LoanActivityEventArgs.After(EllieMae.EMLite.LoanUtils.DataEngine.LoanActivityType.CalcOnDemand, new TimeSpan?(stopwatch.Elapsed)));
          PerformanceMeter.Current.AddCheckpoint("Finished Calculator.CalcOnDemand()", 2548, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
          if (this.LoanData.Calculator.IsCalcAllRequired)
          {
            PerformanceMeter.Current.AddCheckpoint("Starting Calculator.CalculateAll()", 2552, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
            stopwatch = Stopwatch.StartNew();
            LoanActivityEventHandler beforeLoanActivity2 = this.BeforeLoanActivity;
            if (beforeLoanActivity2 != null)
              beforeLoanActivity2((object) this, LoanActivityEventArgs.Before(EllieMae.EMLite.LoanUtils.DataEngine.LoanActivityType.CalcAll));
            try
            {
              bool skipLockRequestSync = this.loanData.Calculator.SkipLockRequestSync;
              this.LoanData.Calculator.SkipLockRequestSync = true;
              this.LoanData.Calculator.CalculateAll(false);
              this.LoanData.Calculator.SkipLockRequestSync = skipLockRequestSync;
            }
            catch (Exception ex)
            {
            }
            stopwatch.Stop();
            LoanActivityEventHandler afterLoanActivity2 = this.AfterLoanActivity;
            if (afterLoanActivity2 != null)
              afterLoanActivity2((object) this, LoanActivityEventArgs.After(EllieMae.EMLite.LoanUtils.DataEngine.LoanActivityType.CalcAll, new TimeSpan?(stopwatch.Elapsed)));
            PerformanceMeter.Current.AddCheckpoint("Finished Calculator.CalculateAll()", 2571, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
          }
          if (this.LoanData.LinkedData != null && this.LoanData.LinkedData.Calculator != null)
          {
            PerformanceMeter.Current.AddCheckpoint("Starting Calculator.CalcOnDemand() for linked loan", 2577, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
            if (EnConfigurationSettings.GlobalSettings.Debug)
            {
              stopwatch = Stopwatch.StartNew();
              LoanActivityEventHandler beforeLoanActivity3 = this.BeforeLoanActivity;
              if (beforeLoanActivity3 != null)
                beforeLoanActivity3((object) this, new LoanActivityEventArgs(EllieMae.EMLite.LoanUtils.DataEngine.LoanActivityType.CalcOnDemand, "Starting Calculator.CalcOnDemand() for linked loan"));
            }
            this.LoanData.LinkedData.Calculator.CalcOnDemand();
            if (EnConfigurationSettings.GlobalSettings.Debug)
            {
              stopwatch.Stop();
              LoanActivityEventHandler afterLoanActivity3 = this.AfterLoanActivity;
              if (afterLoanActivity3 != null)
                afterLoanActivity3((object) this, new LoanActivityEventArgs(EllieMae.EMLite.LoanUtils.DataEngine.LoanActivityType.CalcOnDemand, "Finished Calculator.CalcOnDemand() for linked loan")
                {
                  Elapsed = new TimeSpan?(stopwatch.Elapsed)
                });
            }
            PerformanceMeter.Current.AddCheckpoint("Finished Calculator.CalcOnDemand() for linked loan", 2589, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
            if (this.LoanData.LinkedData.Calculator.IsCalcAllRequired)
            {
              PerformanceMeter.Current.AddCheckpoint("Starting Calculator.CalculateAll() for linked loan", 2593, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
              if (EnConfigurationSettings.GlobalSettings.Debug)
              {
                stopwatch = Stopwatch.StartNew();
                LoanActivityEventHandler beforeLoanActivity4 = this.BeforeLoanActivity;
                if (beforeLoanActivity4 != null)
                  beforeLoanActivity4((object) this, new LoanActivityEventArgs(EllieMae.EMLite.LoanUtils.DataEngine.LoanActivityType.CalcAll, "Starting Calculator.CalculateAll() for linked loan"));
              }
              try
              {
                bool skipLockRequestSync = this.LoanData.LinkedData.Calculator.SkipLockRequestSync;
                this.LoanData.LinkedData.Calculator.SkipLockRequestSync = true;
                this.LoanData.LinkedData.Calculator.SkipLinkedSync = true;
                this.LoanData.LinkedData.Calculator.CalculateAll(false);
                this.LoanData.LinkedData.Calculator.SkipLockRequestSync = skipLockRequestSync;
              }
              catch (Exception ex)
              {
              }
              if (EnConfigurationSettings.GlobalSettings.Debug)
              {
                stopwatch.Stop();
                LoanActivityEventHandler afterLoanActivity4 = this.AfterLoanActivity;
                if (afterLoanActivity4 != null)
                  afterLoanActivity4((object) this, new LoanActivityEventArgs(EllieMae.EMLite.LoanUtils.DataEngine.LoanActivityType.CalcAll, "Finished Calculator.CalculateAll() for linked loan")
                  {
                    Elapsed = new TimeSpan?(stopwatch.Elapsed)
                  });
              }
              PerformanceMeter.Current.AddCheckpoint("Finished Calculator.CalculateAll() for linked loan", 2616, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
            }
          }
        }
        if (this.LoanData.Calculator != null && this.LoanData.Calculator.TriggerTradeOffCalculation)
          this.LoanData.Calculator.FormCalculation("TRADEOFFTABLE", (string) null, (string) null);
        if (this.loanData.Calculator != null)
          this.loanData.Calculator.FormCalculation("CLEARULDD", (string) null, (string) null);
        PerformanceMeter.Current.AddCheckpoint("Starting UpdateLockValidationStatus()", 2629, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        this.UpdateLockValidationStatus(this.loanData.GetLogList().GetCurrentConfirmedLockRequest(), !this.IsFromPlatform);
        PerformanceMeter.Current.AddCheckpoint("Finished UpdateLockValidationStatus()", 2631, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        PerformanceMeter.Current.AddCheckpoint("Starting this.alertMonitor.ActivateAlerts()", 2633, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        this.alertMonitor.ActivateAlerts();
        PerformanceMeter.Current.AddCheckpoint("Finished this.alertMonitor.ActivateAlerts()", 2636, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        string autosaveFile = (string) null;
        string autosaveAttFile = (string) null;
        string autosaveHisFile = (string) null;
        string autosaveTsFile = (string) null;
        if (LoanDataMgr.ISave != null)
        {
          LoanDataMgr.ISave.PreSave(out autosaveFile, out autosaveAttFile, out autosaveHisFile, out autosaveTsFile);
          PerformanceMeter.Current.AddCheckpoint("Finished firing the PreSave event", 2646, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        }
        bool flag1 = !mergeOnly;
        if (!mergeOnly & interactive && !this.Writable)
        {
          Cursor.Current = Cursors.Default;
          PerformanceMeter.Current.AddCheckpoint("BEFORE Warning Dialog - file is READ ONLY", 2654, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
          int num = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, "You cannot save the file because it is read-only.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          PerformanceMeter.Current.AddCheckpoint("AFTER Warning Dialog - file is READ ONLY", 2656, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
          return false;
        }
        Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Saving LoanData object with ContentAccess = " + (object) this.LoanData.ContentAccess);
        if (this.LoanData.ContentAccess != LoanContentAccess.FullAccess)
        {
          Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Performing XML extraction based on content access...");
          if (interactive & flag1)
          {
            string accessRightMessage = LoanAccess.GetAccessRightMessage(this.LoanData.ContentAccess);
            if (accessRightMessage != string.Empty)
            {
              PerformanceMeter.Current.AddCheckpoint("BEFORE Warning Dialog - Partial Access, Limited Save", 2673, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
              int num = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, "Only changes to the following areas will be saved:" + Environment.NewLine + Environment.NewLine + accessRightMessage + Environment.NewLine + "All other changes you have made to this loan will be discarded.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              PerformanceMeter.Current.AddCheckpoint("AFTER Warning Dialog - Partial Access, Limited Save", 2678, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
            }
            else if (!mergeOnly)
            {
              Cursor.Current = Cursors.Default;
              PerformanceMeter.Current.AddCheckpoint("BEFORE Warning Dialog - Partial Access, Read Only Mode", 2683, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
              int num = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, "This loan file is in read only mode. All changes you have made to this loan will be discarded.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              PerformanceMeter.Current.AddCheckpoint("AFDTER Warning Dialog - Partial Access, Read Only Mode", 2686, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
              return false;
            }
          }
          Stream loanDataXml1 = (Stream) null;
          Stream loanDataXml2 = (Stream) null;
          try
          {
            loanDataXml1 = this.LoanData.ToStream();
            if (this.LoanData.LinkedData != null)
            {
              Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Performing Linked Loan XML extraction based on content access...");
              loanDataXml2 = this.LoanData.LinkedData.ToStream();
            }
            Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Replacing the LoanData's XML with merged XML...");
            this.ReplaceLoanDataXml(loanDataXml1, false);
            if (this.LoanData.LinkedData != null)
            {
              Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Replacing Linked Loan XML with merged XML...");
              this.LinkedLoan.ReplaceLoanDataXml(loanDataXml2, false);
              this.LinkTo(this.LinkedLoan);
            }
          }
          finally
          {
            loanDataXml2?.Dispose();
            loanDataXml1?.Dispose();
          }
          PerformanceMeter.Current.AddCheckpoint("Finished merging loan data (main and possibly linked loan)", 2724, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        }
        Exception exEncountered = (Exception) null;
        try
        {
          if (this.SessionObjects.EditLoanConcurrently)
          {
            Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Concurrent editing is enabled. Evaluating whether a 3-way merge is required");
            ConcurrentEditingDialog.Actions action = ConcurrentEditingDialog.Actions.Cancel;
            LockInfo lockInfo = (LockInfo) null;
            foreach (LockInfo currentLock in this.GetCurrentLocks())
            {
              if (currentLock.LoginSessionID == this.sessionObjects.SessionID)
                lockInfo = currentLock;
            }
            if (lockInfo == null)
            {
              Cursor.Current = Cursors.Default;
              PerformanceMeter.Current.AddCheckpoint("BEFORE Warning Dialog - lock removed", 2748, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
              int num = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, "You cannot save this file because your lock on this loan has been removed.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              PerformanceMeter.Current.AddCheckpoint("AFTER Warning Dialog - lock removed", 2750, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
              return false;
            }
            if (this.IsLoanFileOnServerNewer(isExternalOrganization))
            {
              Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Detected newer version of loan on server.");
              if (!mergeOnly && !interactive && !sdkSave || lockInfo.Exclusive == LockInfo.ExclusiveLock.Exclusive || lockInfo.Exclusive == LockInfo.ExclusiveLock.ExclusiveA || lockInfo.Exclusive == LockInfo.ExclusiveLock.Both)
              {
                Cursor.Current = Cursors.Default;
                PerformanceMeter.Current.AddCheckpoint("THROW - newer version on the Server", 2762, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
                throw new ObjectModifiedException("There is a newer version of the loan on server. Merge may not be performed.");
              }
              action = this.xml3WayMerge(action, mergeOnly, interactive, out loanFileMerged, out exEncountered);
              if (action != ConcurrentEditingDialog.Actions.Cancel)
                this.FileAttachments.Resync();
              if (action == ConcurrentEditingDialog.Actions.Cancel || action == ConcurrentEditingDialog.Actions.Merge)
              {
                if (interactive && !mergeOnly && action == ConcurrentEditingDialog.Actions.Merge)
                {
                  Cursor.Current = Cursors.Default;
                  PerformanceMeter.Current.AddCheckpoint("BEFORE Warning Dialog - newer version, not yet saved", 2775, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
                  int num = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, "Note that the loan has not been saved yet.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                  PerformanceMeter.Current.AddCheckpoint("AFTER Warning Dialog - newer version, not yet saved", 2777, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
                }
                if (mergeOnly && exEncountered == null)
                {
                  if (LoanDataMgr.ISave != null)
                    LoanDataMgr.ISave.PostMerge(false);
                  Cursor.Current = Cursors.Default;
                  return false;
                }
                if (exEncountered != null)
                {
                  Cursor.Current = Cursors.Default;
                  return false;
                }
                if (action == ConcurrentEditingDialog.Actions.Cancel)
                  return false;
              }
              if (exEncountered != null)
              {
                Cursor.Current = Cursors.Default;
                return false;
              }
            }
            if (mergeOnly)
            {
              if (LoanDataMgr.ISave != null)
                LoanDataMgr.ISave.PostMerge(false);
              Cursor.Current = Cursors.Default;
              return false;
            }
            int num1 = 5;
            bool flag2 = true;
            do
            {
              try
              {
                Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Performing post-merge actions");
                if (LoanDataMgr.ISave != null)
                  LoanDataMgr.ISave.PostMerge(!mergeOnly);
                if (!this.checkMilestoneTemplate(interactive, milestoneTemplateController))
                {
                  Cursor.Current = Cursors.Default;
                  return false;
                }
                if (this.saveInternal(true, saveLinkedLoan) <= 0)
                {
                  Cursor.Current = Cursors.Default;
                  return false;
                }
                flag2 = false;
              }
              catch (ObjectModifiedException ex)
              {
                if (!interactive || lockInfo.Exclusive == LockInfo.ExclusiveLock.Exclusive || lockInfo.Exclusive == LockInfo.ExclusiveLock.ExclusiveA || lockInfo.Exclusive == LockInfo.ExclusiveLock.Both)
                {
                  Cursor.Current = Cursors.Default;
                  PerformanceMeter.Current.AddCheckpoint("THROW - newer version on server", 2840, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
                  throw new ObjectModifiedException("There is a newer version of the loan on server. Merge may not be performed.");
                }
                Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "ObjectModifiedException caught. Re-running merge.");
                if (action == ConcurrentEditingDialog.Actions.Cancel || action == ConcurrentEditingDialog.Actions.MergeAndShowResult)
                  num1 = 5;
                action = this.xml3WayMerge(action, false, false, out loanFileMerged, out exEncountered);
                if (action == ConcurrentEditingDialog.Actions.Cancel || action == ConcurrentEditingDialog.Actions.Merge || exEncountered != null)
                {
                  Cursor.Current = Cursors.Default;
                  if (action == ConcurrentEditingDialog.Actions.Merge)
                  {
                    PerformanceMeter.Current.AddCheckpoint("BEFORE Warning Dialog - Partial Access, Limited Save", 2857, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
                    int num2 = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, "Note that the loan has not been saved yet.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    PerformanceMeter.Current.AddCheckpoint("AFTER Warning Dialog - Partial Access, Limited Save", 2859, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
                  }
                  return false;
                }
                switch (action)
                {
                  case ConcurrentEditingDialog.Actions.MergeAndShowResult:
                    num1 = 5;
                    break;
                  case ConcurrentEditingDialog.Actions.MergeAndSave:
                    --num1;
                    break;
                }
              }
            }
            while (flag2 && num1 > 0);
            if (flag2 || num1 == 0)
            {
              Cursor.Current = Cursors.Default;
              PerformanceMeter.Current.AddCheckpoint("BEFORE Warning Dialog - number of merges exceeded, loan not saved", 2873, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
              int num3 = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, "Maximun number of merges exceeded. Loan is not saved.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              PerformanceMeter.Current.AddCheckpoint("AFTER Warning Dialog - number of merges exceeded, loan not saved", 2875, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
              return false;
            }
            if (this.SetLoanNumber(""))
            {
              if (this.saveInternal(true, saveLinkedLoan) <= 0)
              {
                Cursor.Current = Cursors.Default;
                return false;
              }
            }
          }
          else
          {
            PerformanceMeter.Current.AddCheckpoint("Starting this.SetLoanNumber()", 2890, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
            this.SetLoanNumber("");
            PerformanceMeter.Current.AddCheckpoint("Finished this.SetLoanNumber()", 2892, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
            if (LoanDataMgr.ISave != null)
              LoanDataMgr.ISave.PostMerge(!mergeOnly);
            PerformanceMeter.Current.AddCheckpoint("Starting to check/apply the milestone templates", 2896, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
            if (!this.checkMilestoneTemplate(interactive, milestoneTemplateController))
            {
              Cursor.Current = Cursors.Default;
              return false;
            }
            PerformanceMeter.Current.AddCheckpoint("Finished checking/applying the milestone templates", 2902, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
            if (this.saveInternal(true, saveLinkedLoan, enableRateLockValidation, enableBackupLoanFile) <= 0)
            {
              Cursor.Current = Cursors.Default;
              return false;
            }
          }
        }
        catch (LockException ex)
        {
          if (!interactive)
          {
            Cursor.Current = Cursors.Default;
            PerformanceMeter.Current.AddCheckpoint("Catch and re-throw", 2920, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
            throw ex;
          }
          Cursor.Current = Cursors.Default;
          PerformanceMeter.Current.AddCheckpoint("BEFORE Warning Dialog - cannot save, lock removed by another user", 2924, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
          int num = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, "You cannot save this file because your lock on it has been removed by another user. Any changes made to this loan will be lost.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          PerformanceMeter.Current.AddCheckpoint("AFTER Warning Dialog - cannot save, lock removed by another user", 2926, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
          flag1 = false;
        }
        catch (SecurityException ex)
        {
          Cursor.Current = Cursors.Default;
          if (!interactive)
          {
            PerformanceMeter.Current.AddCheckpoint("Catch and THROW", 2934, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
            throw ex;
          }
          Cursor.Current = Cursors.Default;
          PerformanceMeter.Current.AddCheckpoint("BEFORE Warning Dialog - you no longer have rights, changes lost", 2939, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
          int num = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, "You no longer have the necessary rights to save changes to this loan. Any changes made will be lost.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          PerformanceMeter.Current.AddCheckpoint("AFTER Warning Dialog - you no longer have rights, changes lost", 2941, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
          flag1 = false;
          this.Unlock(true);
        }
        catch (ValidationException ex)
        {
          Cursor.Current = Cursors.Default;
          if (!interactive)
          {
            PerformanceMeter.Current.AddCheckpoint("Catch and THROW", 2950, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
            throw ex;
          }
          Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Warning, "Validation error '" + ex.Message + "' while saving loan. " + (ex.Rule != null ? "Source = '" + ex.Rule.Description + "'" : ""));
          PerformanceMeter.Current.AddCheckpoint("BEFORE Warning Dialog - not saved, validation error", 2957, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
          int num = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, "The loan cannot be saved due to a validation error: " + ex.Message + (ex.Message.EndsWith(".") ? "" : "."), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          PerformanceMeter.Current.AddCheckpoint("AFTER Warning Dialog - not saved, validation error", 2959, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
          return false;
        }
        if (flag1)
        {
          this.ReleaseExclusiveALock(saveLinkedLoan);
          if (LoanDataMgr.ISave != null)
            LoanDataMgr.ISave.PostSave(this, autosaveFile, autosaveAttFile, autosaveHisFile, autosaveTsFile);
        }
        Cursor.Current = Cursors.Default;
        return flag1;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 2975, nameof (SaveLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      }
    }

    private bool checkMilestoneTemplate(
      bool interactive,
      ILoanMilestoneTemplateOrchestrator milestoneTemplateController)
    {
      if (this.loanData.GetLogList().GetMilestoneByID("7").Done)
        return true;
      bool? nullable = new MilestoneTemplatesManager().ApplyMilestoneTemplate(this.sessionObjects, this.loanData, milestoneTemplateController == null ? this.DefaultMilestoneTemplateHandler : milestoneTemplateController, (MilestoneTemplate) null, "");
      bool flag = false;
      return !(nullable.GetValueOrDefault() == flag & nullable.HasValue);
    }

    private bool MockSkipSaving(string clientId)
    {
      if (clientId != "3010000024")
        return false;
      try
      {
        return System.IO.File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "QA-SEC-11394.txt"));
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public bool Save(bool updateRecentLoanInfo) => this.Save(updateRecentLoanInfo, true, false);

    public bool Save(bool updateRecentLoanInfo, bool triggerCalcAll)
    {
      return this.Save(updateRecentLoanInfo, triggerCalcAll, false);
    }

    public bool SaveAndIgnoreRuleException(bool updateRecentLoanInfo)
    {
      return this.Save(updateRecentLoanInfo, true, true);
    }

    public bool SaveAndIgnoreRuleException(bool updateRecentLoanInfo, bool triggerCalcAll)
    {
      return this.Save(updateRecentLoanInfo, triggerCalcAll, true);
    }

    public bool Save(bool updateRecentLoanInfo, bool triggerCalcAll, bool ignoreRules)
    {
      return this.Save(updateRecentLoanInfo, triggerCalcAll, ignoreRules, false);
    }

    public bool Save(
      bool updateRecentLoanInfo,
      bool triggerCalcAll,
      bool ignoreRules,
      bool throwException)
    {
      if (this.MockSkipSaving(this.SessionObjects.CompanyInfo.ClientID))
        return true;
      try
      {
        if (((this.LoanData == null ? 0 : (this.LoanData.Calculator != null ? 1 : 0)) & (triggerCalcAll ? 1 : 0)) != 0)
        {
          this.LoanData.Calculator.CalcOnDemand();
          bool skipLockRequestSync = this.loanData.Calculator.SkipLockRequestSync;
          this.LoanData.Calculator.SkipLockRequestSync = true;
          this.LoanData.Calculator.CalculateAll(false);
          this.LoanData.Calculator.SkipLockRequestSync = skipLockRequestSync;
        }
        if (((this.LoanData.LinkedData == null ? 0 : (this.LoanData.LinkedData.Calculator != null ? 1 : 0)) & (triggerCalcAll ? 1 : 0)) != 0)
        {
          this.LoanData.LinkedData.Calculator.CalcOnDemand();
          bool skipLockRequestSync = this.LoanData.LinkedData.Calculator.SkipLockRequestSync;
          this.LoanData.LinkedData.Calculator.SkipLockRequestSync = true;
          this.LoanData.LinkedData.Calculator.SkipLinkedSync = true;
          this.LoanData.LinkedData.Calculator.CalculateAll(false);
          this.LoanData.LinkedData.Calculator.SkipLockRequestSync = skipLockRequestSync;
        }
      }
      catch (Exception ex)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(ex.Message);
        if (ex.StackTrace != null)
          stringBuilder.AppendLine(ex.StackTrace.ToString());
        if (ex.InnerException != null)
          stringBuilder.AppendLine(ex.InnerException.Message);
        Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Error, "LoanDataMgr.Save() calculator error '" + stringBuilder.ToString());
        RemoteLogger.Write(TraceLevel.Info, "LoanDataMgr.Save() calculator error loanGuid:" + this.loanData.GUID + " errormsg:" + stringBuilder.ToString());
        if (!ignoreRules)
        {
          if (!throwException)
            return false;
          throw ex;
        }
      }
      try
      {
        bool flag = this.saveInternal(updateRecentLoanInfo, true, ignoreRules: ignoreRules) > 0;
        if (flag && this.ExecuteEmailTriggers != null)
          this.ExecuteEmailTriggers((object) this, new EventArgs());
        return flag;
      }
      catch (Exception ex)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(ex.Message);
        if (ex.StackTrace != null)
          stringBuilder.AppendLine(ex.StackTrace.ToString());
        if (ex.InnerException != null)
          stringBuilder.AppendLine(ex.InnerException.Message);
        Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Error, "LoanDataMgr.Save() saveInternal error '" + stringBuilder.ToString());
        RemoteLogger.Write(TraceLevel.Info, string.Format("LoanDataMgr.Save() saveInternal error LoanGuid {0}, userID {1}, sessionId {2}, err{3}", (object) this.loanData.GUID, (object) this.sessionObjects.UserID, (object) this.sessionObjects.SessionID, (object) stringBuilder.ToString()));
        if (!throwException)
          return false;
        throw ex;
      }
    }

    private int saveInternal(
      bool updateRecentLoanInfo,
      bool saveLinkedLoan,
      bool enableRateLockValidation = false,
      bool enableBackupLoanFile = false,
      bool ignoreRules = false)
    {
      PerformanceMeter.Current.AddCheckpoint("Starting LoanDataMgr.saveInternal()", 3109, nameof (saveInternal), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.saveInternal(" + updateRecentLoanInfo.ToString() + ", " + saveLinkedLoan.ToString() + ")");
      int num = -1;
      if (this.readOnly)
      {
        if (this.AfterSavingLoanFiles != null)
          this.AfterSavingLoanFiles((object) this, (SavingLoanFilesEventArgs) null);
        throw new ApplicationException("This loan file is read only.  Any changes made to this file will not be saved.");
      }
      if (!ignoreRules)
      {
        this.validator.ValidateAll(this.FromLoanImport, (List<string>) null);
      }
      else
      {
        try
        {
          this.validator.ValidateAll(false, (List<string>) null);
        }
        catch (Exception ex)
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.AppendLine(ex.Message);
          if (ex.StackTrace != null)
            stringBuilder.AppendLine(ex.StackTrace.ToString());
          if (ex.InnerException != null)
            stringBuilder.AppendLine(ex.InnerException.Message);
          string msg = "LoanDataMgr.Save() saveInternal error '" + stringBuilder.ToString();
          Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Error, msg);
          RemoteLogger.Write(TraceLevel.Info, string.Format("LoanDataMgr.saveInternal error LoanGuid:{0} userID:{1} sessionId:{2} msg:{3}", (object) this.loanData.GUID, (object) this.sessionObjects.UserID, (object) this.sessionObjects.Session, (object) msg));
          Tracing.SendBusinessRuleErrorToServer(TraceLevel.Error, "loanGuid: " + this.loanData.GUID + " - ValidationException from SilentSave - LoanDataMgr - " + msg);
        }
      }
      PerformanceMeter.Current.AddCheckpoint("Finished validator.ValidateAll()", 3148, nameof (saveInternal), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      CancelableEventArgs e1 = new CancelableEventArgs();
      if (this.BeforeSavingLoanFiles != null)
      {
        PerformanceMeter.Current.StartCustomization("Started firing BeforeSavingLoanFiles event");
        this.BeforeSavingLoanFiles((object) this, e1);
        PerformanceMeter.Current.StopCustomization("Finished firing BeforeSavingLoanFiles event");
      }
      if (e1.Cancel)
      {
        PerformanceMeter.Current.Abort();
        return -1;
      }
      if (EllieMae.EMLite.Common.Utils.CheckIf2015RespaTila(this.loanData.GetField("3969")) && this.loanData.Calculator != null)
        this.loanData.Calculator.CalculateFeeVariance();
      this.loanData.SetField("4460", string.Concat((object) this.loanData.GetBorrowerPairs().Length));
      if (this.FromLoanImport && this.FailedValidationListFieldIds.Count > 0)
      {
        foreach (string validationListFieldId in this.FailedValidationListFieldIds)
          this.loanData.SetField(validationListFieldId, "");
      }
      SavingLoanFilesEventArgs e2 = (SavingLoanFilesEventArgs) null;
      PerformanceMeter.Current.AddCheckpoint("Starting roundtrip heavy section", 3180, nameof (saveInternal), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      this.SaveLockRequestLogSnapshot();
      PerformanceMeter.Current.AddCheckpoint("Finished SaveLockRequestLogSnapshot()", 3182, nameof (saveInternal), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      this.SaveDocumentTrackingLogSnapshot();
      PerformanceMeter.Current.AddCheckpoint("Finished SaveDocumentTrackingLogSnapshot", 3184, nameof (saveInternal), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      if (this.IsNew())
      {
        this.createNewLoan(DuplicateLoanAction.Rename);
        PerformanceMeter.Current.AddCheckpoint("Finished createNewLoan()", 3188, nameof (saveInternal), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        e2 = new SavingLoanFilesEventArgs(true);
        if (this.loanData.LoanVersionNumber > 0)
          num = this.loanData.LoanVersionNumber;
      }
      else
      {
        this.syncCoreMilestoneDatesToStandardFields();
        this.PopulateFieldChangesUsingNgDiff(PerformanceMeter.Current);
        string stackTrace = Tracing.GetStackTrace();
        PerformanceMeter.Current.AddCheckpoint("Starting LoanProxy.Save()", 3201, nameof (saveInternal), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        LoanActivityEventHandler beforeLoanActivity = this.BeforeLoanActivity;
        if (beforeLoanActivity != null)
          beforeLoanActivity((object) this, new LoanActivityEventArgs(EllieMae.EMLite.LoanUtils.DataEngine.LoanActivityType.LoanCommit, "Saving Loan"));
        Stopwatch stopwatch = Stopwatch.StartNew();
        num = this.loan.Save(this.loanData, false, stackTrace, enableRateLockValidation, enableBackupLoanFile);
        stopwatch.Stop();
        LoanActivityEventHandler afterLoanActivity = this.AfterLoanActivity;
        if (afterLoanActivity != null)
          afterLoanActivity((object) this, new LoanActivityEventArgs(EllieMae.EMLite.LoanUtils.DataEngine.LoanActivityType.LoanCommit, "Loan Saved")
          {
            Elapsed = new TimeSpan?(stopwatch.Elapsed)
          });
        PerformanceMeter.Current.AddCheckpoint("Finished LoanProxy.Save()", 3208, nameof (saveInternal), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        if (num <= 0)
        {
          PerformanceMeter.Current.Abort();
          if (!this.SessionObjects.EditLoanConcurrently)
            throw new LoanVersionNumberMismatchException("The loan can not be saved as a more recent version may already be available on the server.");
          throw new ObjectModifiedException("The loan file on the server has been modified.");
        }
        this.loanData.LoanVersionNumber = num;
        this.loanData.IsAutoSaveFlag = false;
        this.loanData.AutoSaveDateTime = DateTime.MinValue;
      }
      Task<DateTime> task = Task.Run<DateTime>((Func<DateTime>) (() => this.loan.GetLastModifiedDate(true, 0)));
      PerformanceMeter.Current.AddCheckpoint("Starting to save disclosure tracking snapshots", 3225, nameof (saveInternal), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      this.saveDisclosureTracking2015UCD();
      this.saveDisclosureTracking2015UDT();
      PerformanceMeter.Current.AddCheckpoint("Finished saving disclosure tracking snapshots", 3228, nameof (saveInternal), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      this.saveAuxiliaryData(updateRecentLoanInfo);
      this.lastModified = task.Result;
      this.loanData.BaseLastModified = this.lastModified;
      this.updateX3wmBase();
      if (saveLinkedLoan && this.LinkedLoan != null)
      {
        this.linkedLoan.LoanData.SetField("4460", string.Concat((object) this.linkedLoan.LoanData.GetBorrowerPairs().Length));
        PerformanceMeter.Current.AddCheckpoint("Starting saveInternal for linked loan", 3239, nameof (saveInternal), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        this.linkedLoan.saveInternal(false, false);
        PerformanceMeter.Current.AddCheckpoint("Finished saveInternal for linked loan", 3241, nameof (saveInternal), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      }
      this.loanData.DDMIsRequired = false;
      this.loanData.ResetDDMVirtualFieldTable();
      PerformanceMeter.Current.AddCheckpoint("Starting to execute the post-save triggers", 3247, nameof (saveInternal), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      this.executeDelayedTriggers();
      PerformanceMeter.Current.AddCheckpoint("Finished executing the post-save triggers", 3250, nameof (saveInternal), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      if (this.AfterSavingLoanFiles != null)
      {
        PerformanceMeter.Current.StartCustomization("Starting to fire the AfterSavingLoanFiles event");
        this.AfterSavingLoanFiles((object) this, e2);
        PerformanceMeter.Current.StopCustomization("Finished firing the AfterSavingLoanFiles event");
      }
      if (this.SessionObjects.AllowConcurrentEditing)
      {
        UserShortInfoList workingOnTheLoan = this.GetUsersWorkingOnTheLoan(this.sessionObjects.SessionID, true);
        if (workingOnTheLoan != null && workingOnTheLoan.Count > 0)
          this.sessionObjects.ServerManager.SendMessage((EllieMae.EMLite.ClientServer.Message) new CEMessage(this.sessionObjects.UserInfo, CEMessageType.LoanFileSaved), workingOnTheLoan.SessionIDs, true);
      }
      if (!this.loanData.IsLocked("4063"))
      {
        if (this.loanData.GetField("2626") == "Banked - Retail" || this.loanData.GetField("2626") == "Banked - Wholesale")
          this.loanData.SetField("4063", this.loanData.GetField("364"));
        else
          this.loanData.SetField("4063", "");
      }
      PerformanceMeter.Current.AddCheckpoint("Finished saveInternal()", 3273, nameof (saveInternal), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      return num;
    }

    private void syncCoreMilestoneDatesToStandardFields()
    {
      try
      {
        EllieMae.EMLite.DataEngine.Log.MilestoneLog[] allMilestones = this.loanData.GetLogList().GetAllMilestones();
        for (int index = 0; index < allMilestones.Length; ++index)
        {
          if (string.Compare(allMilestones[index].Stage, "Started", true) == 0 && allMilestones[index].Date != EllieMae.EMLite.Common.Utils.ParseDate((object) this.loanData.GetField("MS.START")))
          {
            this.loanData.SetField("MS.START", allMilestones[index].Date.ToString("M/d/yy"));
            break;
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Warning, "Unable to sync milestone dates to standard fields. Error: " + ex.Message);
      }
    }

    public void SaveDisclosureTracking2015()
    {
      this.saveDisclosureTracking2015UCD();
      this.saveDisclosureTracking2015UDT();
    }

    private void saveDisclosureTracking2015UCD()
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.saveAuxiliaryData()");
      foreach (EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log disclosureTracking2015Log in this.loanData.GetLogList().GetAllIDisclosureTracking2015Log(false))
      {
        if (disclosureTracking2015Log.UCD != "")
        {
          this.loanData.SnapshotProvider.SaveSnapshot(LogSnapshotType.DisclosureTrackingUCD, new Guid(disclosureTracking2015Log.Guid), disclosureTracking2015Log.UCD);
          disclosureTracking2015Log.UCD = "";
        }
      }
    }

    private void saveDisclosureTracking2015UDT()
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.saveDisclosureTracking2015UDT()");
      foreach (EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log disclosureTracking2015Log in this.loanData.GetLogList().GetAllIDisclosureTracking2015Log(false))
      {
        if (disclosureTracking2015Log.UDT != "")
        {
          this.loanData.SnapshotProvider.SaveSnapshot(LogSnapshotType.DisclosureTracking, new Guid(disclosureTracking2015Log.Guid), disclosureTracking2015Log.UDT);
          disclosureTracking2015Log.UDT = "";
        }
      }
    }

    private void SaveLockRequestLogSnapshot()
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.SaveLockRequestLogSnapshot()");
      foreach (EllieMae.EMLite.DataEngine.Log.LockRequestLog allLockRequest in this.loanData.GetLogList().GetAllLockRequests())
      {
        try
        {
          string requestSnapshotString = allLockRequest.LockRequestSnapshotString;
          if (!string.IsNullOrEmpty(requestSnapshotString))
          {
            if (allLockRequest.IsLockRequestSnapshotDirty)
            {
              this.loanData.SnapshotProvider.SaveSnapshot(LogSnapshotType.LockRequest, new Guid(allLockRequest.Guid), requestSnapshotString, true);
              allLockRequest.IsLockRequestSnapshotDirty = false;
              allLockRequest.ClearSnapshot();
            }
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), "SaveLockRequestLogSnapshot : Cannot save lock request log snapshot. Error: " + ex.Message);
        }
      }
    }

    private void SaveDocumentTrackingLogSnapshot()
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.SaveDocumentTrackingLogSnapshot()");
      foreach (EllieMae.EMLite.DataEngine.Log.DocumentTrackingLog documentTrackingLog in this.loanData.GetLogList().GetAllDocumentTrackingLogs())
      {
        string trackingSnapshotString = documentTrackingLog.DocTrackingSnapshotString;
        if (!string.IsNullOrEmpty(trackingSnapshotString) && documentTrackingLog.IsSnapShotDirty)
        {
          this.loanData.SnapshotProvider.SaveSnapshot(LogSnapshotType.DocumentTracking, new Guid(documentTrackingLog.Guid), trackingSnapshotString, true);
          documentTrackingLog.IsSnapShotDirty = false;
        }
      }
    }

    private void executeDelayedTriggers()
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.executeDelayedTriggers()");
      foreach (DelayedTrigger activatedTrigger in this.triggers.GetDelayActivatedTriggers())
      {
        if (activatedTrigger.SupportsDirectExecution)
        {
          if (activatedTrigger.IsActivated())
          {
            if (activatedTrigger.Execute(this))
              activatedTrigger.Reset();
          }
          else
            activatedTrigger.Reset();
        }
      }
    }

    public void Create(string loanName) => this.Create(this.LoanFolder, loanName);

    public void Create(string loanFolder, string loanName)
    {
      this.LoanName = loanName;
      this.LoanFolder = loanFolder;
      this.Save(false);
    }

    private void saveAuxiliaryData(bool updateRecentLoanInfo)
    {
      PerformanceMeter.Current.AddCheckpoint("Starting LoanDataMgr.saveAuxiliaryData()", 3419, nameof (saveAuxiliaryData), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.saveAuxiliaryData()");
      ILoan localLoanCopy = this.loan;
      if (updateRecentLoanInfo)
        Task.Run((Action) (() => localLoanCopy.AddToRecentLoans()));
      if (this.fileAttachments != null && this.useSkyDriveClassic)
        this.SavePartnerFilesToSkyDrive();
      if (this.fileAttachments != null)
      {
        this.fileAttachments.SaveXml();
        PerformanceMeter.Current.AddCheckpoint("Finished saving file attachments", 3435, nameof (saveAuxiliaryData), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      }
      if (this.supportingDataList != null)
      {
        foreach (string key in (IEnumerable) this.supportingDataList.Keys)
        {
          if (this.supportingDataList[(object) key] is BinaryObject)
          {
            BinaryObject supportingData = (BinaryObject) this.supportingDataList[(object) key];
            this.SaveSupportingData(key, supportingData);
          }
          if (this.supportingDataList[(object) key] is SnapshotObject)
          {
            SnapshotObject supportingData = (SnapshotObject) this.supportingDataList[(object) key];
            this.SaveSupportingSnapshotData(supportingData.Type, supportingData.ParentId, SnapshotObject.GetLoanSnapshotFileName(supportingData.Type, supportingData.ParentId.ToString()), supportingData);
          }
        }
        this.supportingDataList = (SortedList) null;
        PerformanceMeter.Current.AddCheckpoint("Finished saving supporting data", 3459, nameof (saveAuxiliaryData), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      }
      this.loanHistory.SavePendingHistory();
      PerformanceMeter.Current.AddCheckpoint("Finished this.loanHistory.SavePendingHistory()", 3464, nameof (saveAuxiliaryData), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      this.loanData.Dirty = false;
      PerformanceMeter.Current.AddCheckpoint("Finished LoanDataMgr.saveAuxiliaryData()", 3468, nameof (saveAuxiliaryData), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
    }

    public void SavePartnerFilesToSkyDrive()
    {
      try
      {
        FileAttachment[] allFiles = this.fileAttachments.GetAllFiles(false, false);
        SDCHelper helper = new SDCHelper(this);
        List<FileAttachment> list = ((IEnumerable<FileAttachment>) allFiles).Where<FileAttachment>((Func<FileAttachment, bool>) (a => a.AttachmentType == AttachmentType.Native)).ToList<FileAttachment>();
        int result;
        int num = int.TryParse(EnConfigurationSettings.AppSettings["ParallelismForSDC"], out result) ? result : 5;
        Tracing.Log(LoanDataMgr.sw, TraceLevel.Verbose, nameof (LoanDataMgr), string.Format("SkyDriveClassic: Setting MaxDegreeOfParallelism to {0} for uploading files on loan save", (object) num));
        List<FileAttachment> source = list;
        ParallelOptions parallelOptions = new ParallelOptions();
        parallelOptions.MaxDegreeOfParallelism = num;
        Action<FileAttachment> body = (Action<FileAttachment>) (attachment =>
        {
          if (attachment.AttachmentType != AttachmentType.Native)
            return;
          NativeAttachment nativeAttachment = attachment as NativeAttachment;
          Tracing.Log(LoanDataMgr.sw, TraceLevel.Verbose, nameof (LoanDataMgr), "SkyDriveClassic::Calling SavePartnerFilesToSkyDrive for NativeAttachment");
          Task.WaitAll(Task.Run((Func<Task>) (() => helper.SavePartnerFilesToSkyDrive(nativeAttachment))));
        });
        Parallel.ForEach<FileAttachment>((IEnumerable<FileAttachment>) source, parallelOptions, body);
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), string.Format("SkyDriveClassic: Error in saving partner files on skydrive. Ex: {0}", (object) ex));
        throw;
      }
    }

    private void createNewLoan(DuplicateLoanAction duplicateAction)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.createNewLoan()");
      if (!this.IsNew())
        throw new ApplicationException("Create can only be called on new loan");
      if ((this.newFolderName ?? "") == "")
        throw new ArgumentException("Must specify loan folder", "loanFolder");
      this.newLoanName = !((this.newLoanName ?? "") == "") ? this.removeInvalidChars(this.newLoanName) : this.removeInvalidChars(this.loanData.GetField("37") ?? "");
      if ((this.newLoanName ?? "") == "")
        this.newLoanName = "unnamed";
      if (duplicateAction == DuplicateLoanAction.Rename && this.newLoanName.Length > 64)
        this.newLoanName = this.newLoanName.Substring(0, 64);
      this.initLoanIdentifiers();
      this.PopulateFieldChangesUsingNgDiff(PerformanceMeter.Current);
      PerformanceMeter.Current.AddCheckpoint("Starting to call LoanManager.CreateLoan", 3536, nameof (createNewLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      this.loan = this.LoanManager.CreateLoan(this.newFolderName, this.newLoanName, this.loanData, duplicateAction, this.loanData.AddLoanNumber);
      PerformanceMeter.Current.AddCheckpoint("Finished calling LoanManager.CreateLoan", 3538, nameof (createNewLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      this.id = this.loan.GetIdentity();
      LockInfo lockInfo = this.loan.GetLockInfo();
      if (lockInfo.LockedBy == this.sessionObjects.UserID)
        this.loanLock = lockInfo.LockedFor;
      this.syncCoreMilestoneDatesToStandardFields();
      this.loanPropertySettings = this.loan.GetLoanPropertySettings();
      this.useSkyDrive = false;
      this.useSkyDriveLite = false;
      this.useSkyDriveClassic = false;
      foreach (LoanProperty loanPropertySetting in this.loanPropertySettings)
      {
        if (string.Equals(loanPropertySetting.Category, "LoanStorage", StringComparison.OrdinalIgnoreCase) && string.Equals(loanPropertySetting.Attribute, "SupportingData", StringComparison.OrdinalIgnoreCase))
        {
          if (string.Equals(loanPropertySetting.Value, "SkyDrive", StringComparison.OrdinalIgnoreCase))
          {
            this.useSkyDrive = true;
            break;
          }
          if (string.Equals(loanPropertySetting.Value, "SkyDriveLite", StringComparison.OrdinalIgnoreCase))
          {
            this.useSkyDriveLite = true;
            break;
          }
          if (!string.Equals(loanPropertySetting.Value, "SkyDriveClassic", StringComparison.OrdinalIgnoreCase))
            break;
          this.useSkyDriveClassic = true;
          break;
        }
      }
    }

    private void initLoanIdentifiers()
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.initiLoanIdentifiers()");
      if (this.loanData.LoanNumber == "" && this.loanData.AddLoanNumber && !this.loanData.IsCreatedWithoutLoanNumber() && this.LoanManager.IsTimeToSetLoanNumber(this.loanData))
        this.loanData.LoanNumber = this.LoanManager.GetNextLoanNumber();
      try
      {
        if (this.loanData.MersNumber == "")
        {
          this.loanData.MersNumber = this.GetNextMersNumber();
          if (!string.IsNullOrEmpty(this.loanData.MersNumber))
            this.loanData.Mom = "Y";
        }
        string mersOrgId = this.loanData.MersOrgId;
        if (this.loanData.MersNumber == null || this.loanData.MersNumber.Length < 7)
          return;
        this.loanData.MersOrgId = this.loanData.MersNumber.Substring(0, 7);
      }
      catch
      {
      }
    }

    private string removeInvalidChars(string value)
    {
      for (int index = 0; index < "\\/:*?\"<>|".Length; ++index)
        value = value.Replace("\\/:*?\"<>|"[index], '_');
      value = value.Replace('.', '_');
      return value;
    }

    public bool IsNew() => this.loan == null;

    public void Delete()
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.Delete()");
      if (!this.IsNew())
      {
        this.LoanManager.DeleteLoan(this.LoanData.GUID);
        this.loanLock = LoanInfo.LockReason.NotLocked;
      }
      this.Close();
    }

    public void Close()
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.Close()");
      if (this.LoanClosing != null)
        this.LoanClosing((object) this, new EventArgs());
      if (this.skyDriveFileTasks.Count > 0)
      {
        int num = 0;
        foreach (string key in this.skyDriveFileTasks.Keys)
        {
          Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Checking file streaming task: " + key);
          try
          {
            Task.WaitAll(this.skyDriveFileTasks[key]);
          }
          catch (Exception ex)
          {
            Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Error, "File streaming failed for " + key + ": " + ex.ToString());
            ++num;
          }
        }
        this.skyDriveFileTasks.Clear();
        if (num > 0)
          throw new FileUploadException("There was a problem saving " + (object) num + " Supporting Data File(s) for this loan. Please contact your System Administrator if you continue to see this error in the future.");
      }
      if (this.IsNew())
      {
        if (this.linkedLoan != null && this.linkedLoan.loanLock == LoanInfo.LockReason.OpenForWork)
          this.linkedLoan.Close();
        this.loanData = (LoanData) null;
        this.ngLoan = (Loan) null;
        this.ngLoanCreateTask = (Task) null;
      }
      else
      {
        if (this.loanLock == LoanInfo.LockReason.OpenForWork)
          this.Unlock();
        this.loan.Close();
        this.loan = (ILoan) null;
        this.loanData = (LoanData) null;
        this.ngLoan = (Loan) null;
        this.ngLoanCreateTask = (Task) null;
        if (this.linkedLoan == null)
          return;
        this.linkedLoan.Close();
        if (this.linkedLoan == null)
          return;
        this.linkedLoan.linkedLoan = (LoanDataMgr) null;
        this.linkedLoan.linkLoanLoaded = false;
      }
    }

    public void Dispose()
    {
      try
      {
        this.Close();
      }
      catch
      {
      }
    }

    public LoanInfo.Right GetEffectiveRight(string userId)
    {
      return this.loan == null ? this.GetRight(userId) : this.loan.GetRights(userId);
    }

    public LoanInfo.Right GetRight(string userId)
    {
      return this.rights.ContainsKey((object) userId) ? (LoanInfo.Right) this.rights[(object) userId] : LoanInfo.Right.NoRight;
    }

    public void SetRight(string userId, LoanInfo.Right right)
    {
      if (this.loan == null)
        throw new InvalidOperationException("The specified operation in not permitted on an unsaved loan");
      this.loan.SetRights(userId, right);
      this.rights[(object) userId] = (object) right;
    }

    public Hashtable GetRights() => this.rights;

    public PipelineInfo ToPipelineInfo() => this.loanData.ToPipelineInfo();

    public ILoanManager LoanManager => this.sessionObjects.LoanManager;

    public IConfigurationManager ConfigurationManager => this.sessionObjects.ConfigurationManager;

    public IBpmManager BpmManager => this.sessionObjects.BpmManager;

    private bool setWatchListFields()
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "setWatchListFields() called for LoanDataMgr");
      string simpleField1 = this.loanData.GetSimpleField("TPO.X15");
      string simpleField2 = this.loanData.GetSimpleField("TPO.X39");
      string simpleField3 = this.loanData.GetSimpleField("TPO.X75");
      string simpleField4 = this.loanData.GetSimpleField("TPO.X62");
      if (simpleField1 != string.Empty || simpleField2 != string.Empty || simpleField3 != string.Empty || simpleField4 != string.Empty)
      {
        Dictionary<string, string> dictionary = this.sessionObjects.ConfigurationManager.IsTpoOnWatchList(simpleField1, simpleField2, simpleField3, simpleField4);
        this.loanData.SetField("TPO.X86", dictionary["Flag"]);
        this.loanData.SetField("TPO.X87", dictionary["Reason"]);
      }
      return true;
    }

    private bool attachCalculator()
    {
      PerformanceMeter current = PerformanceMeter.Current;
      if (current != null)
        return this.attachCalculator(current);
      using (PerformanceMeter attCal = new PerformanceMeter(nameof (attachCalculator), 3818, nameof (attachCalculator), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs"))
        return this.attachCalculator(attCal);
    }

    private bool attachCalculator(PerformanceMeter attCal)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "attachCalculator() called for LoanDataMgr");
      this.loanData.AttachDataMgr((object) this);
      attCal.AddCheckpoint("this.loanData.AttachDataMgr", 3832, nameof (attachCalculator), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      this.loanData.LogRecordAdded += new LogRecordEventHandler(this.loanData_LogRecordAdded);
      this.loanData.LogRecordRemoved += new LogRecordEventHandler(this.loanData_LogRecordRemoved);
      this.loanData.LogRecordChanged += new LogRecordEventHandler(this.loanData_LogRecordChanged);
      this.loanData.BeforeTriggerRuleApplied += new EventHandler(this.loanData_BeforeTriggerRuleApplied);
      this.loanData.TriggerRuleLoanTemplateChecked += new EventHandler(this.loanData_TriggerRuleLoanTemplateChecked);
      if (this.formSettings == null)
        this.formSettings = new InputFormList(this.SystemConfiguration.AllForms, this.SystemConfiguration.UserAccessibleForms);
      attCal.AddCheckpoint("Attach the form list", 3845, nameof (attachCalculator), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      if (this.loanData.Validator == null)
        this.validator = new LoanValidator(this.sessionObjects, this.configInfo, this.loanData);
      else if (this.loanData.Validator is LoanValidator)
        this.validator = (LoanValidator) this.loanData.Validator;
      this.loanData.AttachSnapshotProvider((ILoanSnapshotProvider) new LoanSnapshotProvider(this));
      attCal.AddCheckpoint("Attach a field validator", 3853, nameof (attachCalculator), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      this.triggers = this.loanData.Triggers != null ? (LoanTriggers) this.loanData.Triggers : new LoanTriggers(this.sessionObjects, this.configInfo, this.loanData);
      this.printSelector = this.loanData.PrintFormSelector != null ? (LoanAutoPrintSelector) this.loanData.PrintFormSelector : new LoanAutoPrintSelector(this.sessionObjects, this.configInfo, this.loanData);
      attCal.AddCheckpoint("Attach Print Form Auto Selector", 3866, nameof (attachCalculator), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      this.alertMonitor = this.loanData.AlertMonitor != null ? (LoanAlertMonitor) this.loanData.AlertMonitor : new LoanAlertMonitor(this.sessionObjects, this.loanData);
      attCal.AddCheckpoint("Attach the alert monitor", 3872, nameof (attachCalculator), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      this.loanHistory = this.loanData.LoanHistoryMonitor != null ? (LoanHistoryManager) this.loanData.LoanHistoryMonitor : new LoanHistoryManager(this);
      attCal.AddCheckpoint("Attach the history monitor", 3878, nameof (attachCalculator), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      if (this.loanData.Calculator == null)
      {
        this.calc = new LoanCalculator(this.sessionObjects, this.configInfo, this.loanData);
        attCal.AddCheckpoint("Attach a loan calculator", 3883, nameof (attachCalculator), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        this.loanData.PrefixedCalculations();
        attCal.AddCheckpoint("this.loanData.PrefixedCalculations", 3885, nameof (attachCalculator), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        if (!this.SessionObjects.SkipCustomCalcsExecutionOnLoanOpen)
        {
          attCal.StartCustomization("Starting to recalculate custom fields");
          this.calc.RecalculateCustomFields();
          attCal.StopCustomization("Finished recalculating custom fields");
        }
        this.calc.RecalculateBillingFields();
        attCal.AddCheckpoint("this.calc.RecalculateBillingFields", 3893, nameof (attachCalculator), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      }
      else if (this.loanData.Calculator is LoanCalculator)
        this.calc = (LoanCalculator) this.loanData.Calculator;
      this.calc.AttachLOCompensation(this.configInfo.LoanOfficerCompensationSetting);
      attCal.AddCheckpoint("this.calc.AttachLOCompensation", 3899, nameof (attachCalculator), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      if (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.OriginalLoanVersion) < 6.207 && this.loanData.Use2010RESPA)
        this.calc.FormCalculation("REGZGFE_2010", (string) null, (string) null);
      attCal.AddCheckpoint("this.calc.FormCalculation", 3903, nameof (attachCalculator), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      if (this.loanData.GetSimpleField("3310") == "")
        this.calc.CalculateBrokerLenderFeeTotals();
      attCal.AddCheckpoint("this.calc.CalculateBrokerLenderFeeTotals", 3908, nameof (attachCalculator), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      if (this.loanData.GetSimpleField("NMLS.X1") == "")
        this.calc.RecalculateNMLS();
      else if (this.loanData.GetSimpleField("3331") == "")
        this.calc.RecalculateNMLS();
      attCal.AddCheckpoint("this.calc.RecalculateNMLS", 3914, nameof (attachCalculator), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      if (this.loanData.GetSimpleField("PlanCode.ID") != "" && this.loanData.GetSimpleField("Docs.Engine") == "")
        EncompassDocs.SetDocEngine((IHtmlInput) this.loanData, "New_Encompass_Docs_Solution");
      attCal.AddCheckpoint("DocEngine.EncompassDocs.SetDocEngine", 3919, nameof (attachCalculator), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      if (this.loanData.GetSimpleField("748") != "" && this.loanData.GetSimpleField("3614") == "")
        this.calc.FormCalculation("IRS1098", (string) null, (string) null);
      attCal.AddCheckpoint("this.calc.FormCalculation(IRS1098)", 3924, nameof (attachCalculator), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      if (this.loanData.GetSimpleField("SERVICE.X999") == "" || EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetSimpleField("SERVICE.X999")) >= 17.1 && EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetSimpleField("SERVICE.X999")) < 17.104 || EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetSimpleField("SERVICE.X999")) != EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.OriginalLoanVersion) && EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetSimpleField("SERVICE.X999")) < 17.3 && this.loanData.GetSimpleField("1393").Equals("") && !this.isCompletedLoan())
      {
        if (this.loanData.GetSimpleField("SERVICE.X8") == "Current" || this.loanData.GetSimpleField("SERVICE.X8") == "Past Due")
        {
          if (this.loanData.XmlDocClone.DocumentElement.SelectSingleNode("EllieMae/InterimServicing/LoanSnapshot") != null)
            this.calc.CalculateInterimServicing(true);
          attCal.AddCheckpoint("this.calc.CalculateInterimServicing", 3938, nameof (attachCalculator), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
          if (this.loanData.GetField("2626") == "Correspondent" && EllieMae.EMLite.Common.Utils.ParseDecimal((object) this.loanData.GetField("3579")) <= 0M && this.loanData.GetField("3571") != "")
            this.loanData.SetField("SERVICE.X144", this.loanData.GetField("3571"));
        }
        this.loanData.SetCurrentField("SERVICE.X999", this.loanData.OriginalLoanVersion);
      }
      if (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.OriginalLoanVersion) < 9.005)
        this.calc.FormCalculation("3", "3", "");
      attCal.AddCheckpoint("this.calc.FormCalculation(3, 3)", 3951, nameof (attachCalculator), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      if (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.OriginalLoanVersion) < 9.102)
        this.calc.FormCalculation("QM.X23", (string) null, (string) null);
      attCal.AddCheckpoint("this.calc.FormCalculation(QM.X23)", 3955, nameof (attachCalculator), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      if (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.OriginalLoanVersion) == 15.201)
        this.calc.FormCalculation("REGZGFE_2010", (string) null, (string) null);
      if (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.OriginalLoanVersion) < 16.103)
        this.calc.FormCalculation("3164", (string) null, (string) null);
      if (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.OriginalLoanVersion) <= 16.203)
      {
        if (!this.IsNew() && !this.loanData.IsLocked("CD3.X104"))
          this.loanData.AddLock("CD3.X104");
        this.loanData.CurrentLoanVersion = "16.203";
      }
      if (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.OriginalLoanVersion) < 3241.0 / 200.0)
      {
        if (this.loanData.GetSimpleField("LE2.X31") == "")
          this.loanData.SetCurrentField("LE2.X31", EllieMae.EMLite.Common.Utils.ParseInt((object) EllieMae.EMLite.Common.Utils.ArithmeticRounding(EllieMae.EMLite.Common.Utils.ParseDecimal((object) this.loanData.GetSimpleField("1092")), 0), 0).ToString());
        this.calc.FormCalculation("MIGRATECDPAGE3LIABILITIES", (string) null, (string) null);
        this.calc.FormCalculation("CLOSINGDISCLOSUREPAGE3", (string) null, (string) null);
        this.loanData.CurrentLoanVersion = "16.205";
      }
      if (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.OriginalLoanVersion) < 16.206)
      {
        if (!this.IsNew() && !this.loanData.IsLocked("CD3.X104"))
          this.loanData.TriggerCalculation("LE2.X28", (string) null, false);
        this.loanData.CurrentLoanVersion = "16.206";
      }
      if (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.OriginalLoanVersion) < 16.32)
      {
        if (!this.IsNew() && !this.loanData.GetSimpleField("1393").Equals("") && !this.loanData.IsLocked("RE88395.X121"))
          this.loanData.AddLock("RE88395.X121");
        this.loanData.CurrentLoanVersion = "16.320";
      }
      if (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.OriginalLoanVersion) < 16.325)
      {
        if (this.loanData.GetField("1393") != "")
          this.loanData.TriggerCalculation("MAX23K.X11", this.loanData.GetField("MAX23K.X11"));
        this.loanData.CurrentLoanVersion = "16.325";
      }
      if (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.OriginalLoanVersion) < 17.307)
      {
        if (!this.IsNew() && !this.loanData.GetSimpleField("608").Equals("Fixed"))
          this.loanData.Calculator.CalculateProjectedPaymentTable();
        this.loanData.CurrentLoanVersion = "17.307";
      }
      if (string.IsNullOrEmpty(this.loanData.GetField("COMPLIANCEVERSION.CD3X1505")))
      {
        if (this.loanData.GetLogList().GetAllIDisclosureTracking2015Log(false).Length == 0)
        {
          this.loanData.SetCurrentField("COMPLIANCEVERSION.CD3X1505", "1");
          this.loanData.TriggerCalculation("COMPLIANCEVERSION.CD3X1505", "1");
        }
        else
          this.loanData.SetCurrentField("COMPLIANCEVERSION.CD3X1505", "0");
      }
      if (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.OriginalLoanVersion) < 18.3)
      {
        if (this.loanData.GetSimpleField("CD4.X51") == "")
        {
          this.loanData.SetCurrentField("CD4.X51", "ConsummationDate");
          this.calc.FormCalculation("CD4.X51");
        }
        if (EllieMae.EMLite.Common.Utils.ParseDate((object) this.loanData.GetField("L770")) == DateTime.MinValue)
          this.calc.FormCalculation("1659", (string) null, (string) null);
        else if (!this.loanData.IsLocked("78"))
          this.loanData.AddLock("78");
        if (EllieMae.EMLite.Common.Utils.ParseInt((object) this.loanData.GetSimpleField("906")) > 0)
          this.calc.FormCalculation("906");
      }
      this.initComplianceVersionField(this.loanData, "1.0");
      if (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.OriginalLoanVersion) < 18.3 && this.calc.UseNewCompliance(18.3M))
      {
        this.calc.FormCalculation("DISCLOSURE.X913");
        this.calc.FormCalculation("NEWHUD.X337");
        this.calc.CalculateProjectedPaymentTable();
      }
      if (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.OriginalLoanVersion) < 18.401)
      {
        this.calc.FormCalculation("HMDACREDITSCOREDECISION", (string) null, (string) null);
        this.calc.FormCalculation("HMDACREDITSCOREMODEL", (string) null, (string) null);
        if (this.LoanData.GetField("HMDA.X100") == "")
        {
          List<HMDAProfile> cachedHmdaProfile = this.sessionObjects.GetCachedHMDAProfile();
          if (cachedHmdaProfile != null && cachedHmdaProfile.Count == 1)
          {
            this.LoanData.SetField("HMDA.X100", cachedHmdaProfile[0].HMDAProfileID.ToString());
            this.Calculator.FormCalculation("UPDATEHMDA2018");
          }
        }
      }
      this.accessRules = new LoanAccessRules(this.sessionObjects, this.configInfo, this.loanData);
      this.loanData.ApplyAccessRules((ILoanAccessRules) this.accessRules);
      attCal.AddCheckpoint("Finished applying Loan Access Rules", 4109, nameof (attachCalculator), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      this.InitFeeLevelDisclosuresIndicator();
      if (string.IsNullOrEmpty(this.LoanData.GetField("4463")))
        this.calc.FormCalculation("COPYLOCOMPTOLR");
      if (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.OriginalLoanVersion) < 18.4)
      {
        BorrowerPair[] borrowerPairs = this.LoanData.GetBorrowerPairs();
        if (borrowerPairs.Length > 1)
        {
          foreach (BorrowerPair pair in borrowerPairs)
          {
            this.LoanData.SetBorrowerPair(pair);
            this.calc.FormCalculation("USDAWORKSHEET");
          }
        }
        this.LoanData.SetBorrowerPair(borrowerPairs[0]);
      }
      if (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.LoanData.OriginalLoanVersion) < 19.1)
        this.loanData.TriggerCalculation("2952", "");
      if (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.LoanData.OriginalLoanVersion) < 19.104)
      {
        BorrowerPair[] borrowerPairs = this.loanData.GetBorrowerPairs();
        if (borrowerPairs != null && borrowerPairs.Length != 0)
          this.loanData.TriggerCalculation("26", "");
      }
      if (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.LoanData.OriginalLoanVersion) < 19.2)
        this.loanData.TriggerCalculation("3548", "");
      if (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.LoanData.OriginalLoanVersion) < 19.3 && this.loanData.GetField("L770") == "" && this.loanData.GetField("1172") == "HELOC")
        this.calc.FormCalculation("CALCMATURITY");
      this.loanData.Calculator.FormCalculationTriggered += new EventHandler(this.calculator_FormCalculationTriggered);
      if (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.LoanData.OriginalLoanVersion) < 19.4 && this.loanData.GetField("1172") == "HELOC")
        this.calc.CalculateHELOCQualifyingPayment();
      if (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.LoanData.OriginalLoanVersion) < 20.105)
      {
        this.loanData.SetCurrentField("VASUMM.X140", "Y");
        this.loanData.SetCurrentField("VASUMM.X141", "Y");
        this.loanData.SetCurrentField("VASUMM.X142", "Y");
        this.loanData.SetCurrentField("VASUMM.X143", "Y");
        this.loanData.SetCurrentField("VASUMM.X144", "Y");
        this.loanData.SetCurrentField("VASUMM.X145", "N");
        this.loanData.SetCurrentField("VASUMM.X146", "N");
        this.loanData.SetCurrentField("VASUMM.X147", "Y");
        this.loanData.SetCurrentField("VASUMM.X148", "Y");
        this.calc.FormCalculation("CD2.XSTB");
      }
      if (this.loanData.CurrentLoanVersion_LDM == "")
        this.loanData.CurrentLoanVersion_LDM = "20.105";
      double num = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.CurrentLoanVersion_LDM);
      if (num < 20.2)
        this.loanData.TriggerCalculation("HMDA.X57", "");
      else if (num < 21.3)
        this.loanData.TriggerCalculation("4745", "");
      if (this.loanData.CurrentLoanVersion_LDM != this.loanData.CurrentLoanVersion)
        this.loanData.CurrentLoanVersion_LDM = this.loanData.CurrentLoanVersion;
      return false;
    }

    private void initComplianceVersionField(LoanData loan, string defaultEMVersion)
    {
      if (!string.IsNullOrEmpty(loan.GetField("COMPLIANCEVERSION.X1")))
        return;
      LogList logList = loan.GetLogList();
      if (logList == null || logList.GetAllIDisclosureTracking2015Log(false).Length == 0)
        loan.SetCurrentField("COMPLIANCEVERSION.X1", "No Log");
      else
        loan.SetCurrentField("COMPLIANCEVERSION.X1", defaultEMVersion);
    }

    private bool isCompletedLoan()
    {
      bool flag = false;
      LogList logList = this.loanData.GetLogList();
      if (logList != null)
      {
        int numberOfMilestones = logList.GetNumberOfMilestones();
        int num;
        if (numberOfMilestones > 0 && logList.GetMilestoneAt(num = numberOfMilestones - 1).Done)
          flag = true;
      }
      return flag;
    }

    private void setEnforceCountyLoanLimit()
    {
      this.loanData.SetField("3894", (bool) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnforceCountyLimit"] ? "Y" : "N");
    }

    private void setDefaultValuesForItemizationFeesPrintSettings()
    {
      if (this.loanData.GetField("NEWHUD.X750") == string.Empty)
        this.loanData.SetField("NEWHUD.X750", "Y");
      if (!(this.loanData.GetField("NEWHUD.X1017") == string.Empty))
        return;
      this.loanData.SetField("NEWHUD.X1017", "Y");
    }

    private void loanData_LogRecordChanged(object source, LogRecordEventArgs e)
    {
      if (!(e.LogRecord is EllieMae.EMLite.DataEngine.Log.DisclosureTrackingLog) || this.loanData.Calculator == null)
        return;
      this.loanData.Calculator.CalculateLatestDisclosure((EllieMae.EMLite.DataEngine.Log.DisclosureTrackingLog) e.LogRecord);
    }

    private void loanData_BeforeTriggerRuleApplied(object sender, EventArgs e)
    {
      if (this.BeforeTriggerRuleApplied == null)
        return;
      this.BeforeTriggerRuleApplied(sender, e);
    }

    private void InitializeNewLoan(
      LoanTemplateSelection loanTemplate,
      bool skipLoanOfficerAssignment = false)
    {
      UserInfo userInfo = this.sessionObjects.UserInfo;
      LoanDefaults loanDefaultData = this.sessionObjects.LoanManager.GetLoanDefaultData();
      bool allowUrlA2020 = this.sessionObjects.StartupInfo.AllowURLA2020;
      DateTime date = EllieMae.EMLite.Common.Utils.ParseDate((object) this.sessionObjects.ConfigurationManager.GetCompanySetting("Policies", "URLA2020"));
      if (allowUrlA2020)
        this.loanData.SetCurrentField("1825", DateTime.Now.Date >= date.Date ? "2020" : "2009");
      else if (EllieMae.EMLite.Common.Utils.ParseInt((object) this.loanData.GetSimpleField("HMDA.X27")) > 2017)
        this.loanData.SetCurrentField("1825", "2009");
      this.populateDefaultFields(userInfo, loanDefaultData, loanTemplate);
      this.initLoanData(loanTemplate, "Encompass - New File", false, skipLoanOfficerAssignment);
      this.loanData.AccessRules.Refresh();
    }

    private void loanData_TriggerRuleLoanTemplateChecked(object sender, EventArgs e)
    {
      if (this.loanData == null || this.loanData.IsTemplate)
        return;
      string[] strArray = (string[]) sender;
      if (strArray == null || strArray.Length != 2 || (strArray[0] ?? "") == "")
        return;
      if (strArray[1] == null)
        strArray[1] = "";
      try
      {
        TriggerImplDef def = this.ApplyLoanTemplateTrigger(strArray[0], strArray[1]);
        if (def == null)
          return;
        Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "loanData_TriggerRuleLoanTemplateChecked: Apply loan template");
        Cursor.Current = Cursors.WaitCursor;
        this.ApplyLoanTemplate(def);
        Cursor.Current = Cursors.Default;
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Error, "loanData_TriggerRuleLoanTemplateChecked: Cannot apply loan template due to error - " + ex.Message);
        RemoteLogger.Write(TraceLevel.Info, "loanData_TriggerRuleLoanTemplateChecked: Cannot apply loan template due to error - " + ex.Message);
      }
    }

    public void ClearCollateralTrackingInformation()
    {
      this.loanData.SetCurrentField("CT.DOT.InitialRequest.PurchaseDate", "");
      this.loanData.SetCurrentField("CT.DOT.InitialRequest.NextFollowupDate", "");
      this.loanData.SetCurrentField("CT.DOT.InitialRequest.Received", "");
      this.loanData.SetCurrentField("CT.DOT.InitialRequest.DaysOutstanding", "");
      this.loanData.SetCurrentField("CT.DOT.InitialRequest.LastRequested", "");
      this.loanData.SetCurrentField("CT.DOT.InitialRequest.RequestedBy", "");
      this.loanData.SetCurrentField("CT.DOT.InitialRequest.Organization", "");
      this.loanData.SetCurrentField("CT.DOT.InitialRequest.Contact", "");
      this.loanData.SetCurrentField("CT.DOT.InitialRequest.Email", "");
      this.loanData.SetCurrentField("CT.DOT.InitialRequest.Phone", "");
      this.loanData.SetCurrentField("CT.DOT.InitialRequest.DocumentNumber", "");
      this.loanData.SetCurrentField("CT.DOT.InitialRequest.BookNumber", "");
      this.loanData.SetCurrentField("CT.DOT.InitialRequest.PageNumber", "");
      this.loanData.SetCurrentField("CT.DOT.InitialRequest.RiderReceived", "");
      this.loanData.SetCurrentField("CT.DOT.ShippingStatus.ShippingDate", "");
      this.loanData.SetCurrentField("CT.DOT.ShippingStatus.ShippedBy", "");
      this.loanData.SetCurrentField("CT.DOT.ShippingStatus.Carrier", "");
      this.loanData.SetCurrentField("CT.DOT.ShippingStatus.TrackingNumber", "");
      this.loanData.SetCurrentField("CT.DOT.ShippingStatus.Organization", "");
      this.loanData.SetCurrentField("CT.DOT.ShippingStatus.Address", "");
      this.loanData.SetCurrentField("CT.DOT.ShippingStatus.Contact", "");
      this.loanData.SetCurrentField("CT.DOT.ShippingStatus.Email", "");
      this.loanData.SetCurrentField("CT.DOT.ShippingStatus.Phone", "");
      this.loanData.SetCurrentField("CT.DOT.ReturnRequest.NextFollowUpDate", "");
      this.loanData.SetCurrentField("CT.DOT.ReturnRequest.Received", "");
      this.loanData.SetCurrentField("CT.DOT.ReturnRequest.LastRequested", "");
      this.loanData.SetCurrentField("CT.DOT.ReturnRequest.RequestedBy", "");
      this.loanData.SetCurrentField("CT.DOT.ReturnRequest.Organization", "");
      this.loanData.SetCurrentField("CT.DOT.ReturnRequest.Contact", "");
      this.loanData.SetCurrentField("CT.DOT.ReturnRequest.Email", "");
      this.loanData.SetCurrentField("CT.DOT.ReturnRequest.Phone", "");
      this.loanData.SetCurrentField("CT.FTP.InitialRequest.NextFollowupDate", "");
      this.loanData.SetCurrentField("CT.FTP.InitialRequest.Received", "");
      this.loanData.SetCurrentField("CT.FTP.InitialRequest.DaysOutstanding", "");
      this.loanData.SetCurrentField("CT.FTP.InitialRequest.LastRequested", "");
      this.loanData.SetCurrentField("CT.FTP.InitialRequest.RequestedBy", "");
      this.loanData.SetCurrentField("CT.FTP.InitialRequest.Organization", "");
      this.loanData.SetCurrentField("CT.FTP.InitialRequest.Contact", "");
      this.loanData.SetCurrentField("CT.FTP.InitialRequest.Email", "");
      this.loanData.SetCurrentField("CT.FTP.InitialRequest.Phone", "");
      this.loanData.SetCurrentField("CT.FTP.ShippingStatus.ShippingDate", "");
      this.loanData.SetCurrentField("CT.FTP.ShippingStatus.ShippedBy", "");
      this.loanData.SetCurrentField("CT.FTP.ShippingStatus.Carrier", "");
      this.loanData.SetCurrentField("CT.FTP.ShippingStatus.TrackingNumber", "");
      this.loanData.SetCurrentField("CT.FTP.ShippingStatus.Organization", "");
      this.loanData.SetCurrentField("CT.FTP.ShippingStatus.Address", "");
      this.loanData.SetCurrentField("CT.FTP.ShippingStatus.Contact", "");
      this.loanData.SetCurrentField("CT.FTP.ShippingStatus.Email", "");
      this.loanData.SetCurrentField("CT.FTP.ShippingStatus.Phone", "");
      this.loanData.SetCurrentField("CT.FTP.ReturnRequest.NextFollowUpDate", "");
      this.loanData.SetCurrentField("CT.FTP.ReturnRequest.Received", "");
      this.loanData.SetCurrentField("CT.FTP.ReturnRequest.LastRequested", "");
      this.loanData.SetCurrentField("CT.FTP.ReturnRequest.RequestedBy", "");
      this.loanData.SetCurrentField("CT.FTP.ReturnRequest.Organization", "");
      this.loanData.SetCurrentField("CT.FTP.ReturnRequest.Contact", "");
      this.loanData.SetCurrentField("CT.FTP.ReturnRequest.Email", "");
      this.loanData.SetCurrentField("CT.FTP.ReturnRequest.Phone", "");
      this.loanData.SetCurrentField("CT.Comment.AllComments", "");
      this.loanData.SetCurrentField("CT.Comment.TransactionComment", "");
      this.loanData.SetCurrentField("CT.EN.ShippingStatus.ShippingDate", "");
      this.loanData.SetCurrentField("CT.EN.ShippingStatus.ShippedBy", "");
      this.loanData.SetCurrentField("CT.EN.ShippingStatus.Carrier", "");
      this.loanData.SetCurrentField("CT.EN.ShippingStatus.TrackingNumber", "");
      this.loanData.SetCurrentField("CT.EN.ShippingStatus.Organization", "");
      this.loanData.SetCurrentField("CT.EN.ShippingStatus.Contact", "");
      this.loanData.SetCurrentField("CT.EN.ShippingStatus.Address", "");
      this.loanData.SetCurrentField("CT.EN.ShippingStatus.Email", "");
      this.loanData.SetCurrentField("CT.EN.ShippingStatus.Phone", "");
    }

    private void clearShippingInformationDetail()
    {
      for (int index = 2012; index <= 2019; ++index)
        this.loanData.SetCurrentField(index.ToString(), "");
      this.loanData.SetCurrentField("VEND.X263", "");
      this.loanData.SetCurrentField("VEND.X276", "");
      this.loanData.SetCurrentField("996", "");
      this.loanData.SetCurrentField("4020", "");
      this.loanData.SetCurrentField("4021", "");
      for (int index = 369; index <= 395; ++index)
        this.loanData.SetCurrentField("VEND.X" + index.ToString(), "");
      int[] source = new int[12]
      {
        531,
        541,
        551,
        561,
        571,
        581,
        591,
        601,
        611,
        621,
        631,
        641
      };
      for (int index = 529; index <= 648; ++index)
      {
        if (!((IEnumerable<int>) source).Contains<int>(index))
          this.loanData.SetCurrentField("VEND.X" + index.ToString(), "");
      }
      this.loanData.SetCurrentField("1051", "");
      for (int index = 2020; index <= 2023; ++index)
        this.loanData.SetCurrentField(index.ToString(), "");
      this.loanData.SetCurrentField("4663", "");
      this.loanData.SetCurrentField("4664", "");
    }

    private void initializeCopiedLoan(LoanData sourceLoanData, string templateToUse)
    {
      this.loanData.SetCurrentField("MS.PROC", "");
      this.loanData.SetCurrentField("MS.APP", "");
      this.loanData.SetCurrentField("MS.SUB", "");
      this.loanData.SetCurrentField("MS.DOC", "");
      this.loanData.SetCurrentField("MS.FUN", "");
      this.loanData.SetCurrentField("MS.CLO", "");
      this.loanData.SetCurrentField("MS.SUB.DUE", "");
      this.loanData.SetCurrentField("MS.APP.DUE", "");
      this.loanData.SetCurrentField("MS.DOC.DUE", "");
      this.loanData.SetCurrentField("MS.FUN.DUE", "");
      this.loanData.SetCurrentField("MS.CLO.DUE", "");
      this.loanData.SetCurrentField("1984", "");
      this.loanData.RemoveLock("3142");
      this.loanData.SetCurrentField("3142", "");
      this.loanData.SetCurrentField("3143", "");
      this.loanData.SetCurrentField("3168", "");
      this.loanData.SetCurrentField("3169", "");
      this.loanData.SetCurrentField("LE1.X90", "");
      this.loanData.SetCurrentField("3166", "");
      this.loanData.SetCurrentField("3165", "");
      this.loanData.SetCurrentField("3167", "");
      this.loanData.SetCurrentField("3164", "");
      this.loanData.SetCurrentField("3197", "");
      this.loanData.SetCurrentField("3972", "");
      this.loanData.SetCurrentField("3973", "");
      this.loanData.SetCurrentField("3974", "");
      this.loanData.SetCurrentField("3975", "");
      this.loanData.SetCurrentField("3976", "");
      this.loanData.SetCurrentField("3317", "");
      this.loanData.SetCurrentField("3318", "");
      this.loanData.SetCurrentField("3333", "");
      this.loanData.SetCurrentField("CCVP.X1", "");
      this.loanData.SetCurrentField("3148", "");
      this.loanData.SetCurrentField("3149", "");
      this.loanData.SetCurrentField("3150", "");
      this.loanData.SetCurrentField("3151", "");
      this.loanData.SetCurrentField("4014", "");
      this.loanData.SetCurrentField("4015", "");
      for (int index = 3977; index <= 3982; ++index)
        this.loanData.SetCurrentField(index.ToString(), "");
      this.loanData.SetCurrentField("3544", "");
      this.loanData.SetCurrentField("3545", "");
      this.loanData.SetCurrentField("3546", "");
      this.loanData.SetCurrentField("3547", "");
      this.loanData.SetCurrentField("3624", "");
      this.loanData.SetCurrentField("3857", "");
      this.loanData.SetCurrentField("3858", "");
      this.loanData.SetCurrentField("3859", "");
      this.loanData.SetCurrentField("4022", "");
      this.loanData.SetCurrentField("AIQ.FOLDERID", "");
      this.loanData.SetCurrentField("AIQ.THREADID", "");
      this.loanData.SetCurrentField("AIQ.CABINETID", "");
      this.loanData.SetCurrentField("AIQ.INFLIGHTLOANID", "");
      this.loanData.SetCurrentField("AIQ.SEGMENTID", "");
      this.loanData.SetCurrentField("AIQ.LOANWASMIRROREDONPST", "");
      this.loanData.SetCurrentField("AIQ.LASTUPDATEBYLISINPST", "");
      this.loanData.SetCurrentField("AIQ.PROPAGATEDATA", "");
      this.loanData.SetCurrentField("AIQ.DOCMIRRORINFLIGHT", "");
      this.loanData.RemoveLockRequest();
      this.loanData.RemoveInterimServicing();
      this.loanData.SetCurrentField("3967", "");
      this.loanData.SetCurrentField("4105", "");
      this.loanData.SetCurrentField("4532", "N");
      this.ClearCollateralTrackingInformation();
      this.clearShippingInformationDetail();
      UserInfo userInfo = this.sessionObjects.UserInfo;
      LoanDefaults loanDefaultData = this.sessionObjects.LoanManager.GetLoanDefaultData();
      string field1 = this.loanData.GetField("Docs.Engine");
      string field2 = this.loanData.GetField("1881");
      string field3 = this.loanData.GetField("PlanCode.ID");
      string field4 = this.loanData.GetField("LE2.X28");
      this.populateDefaultFields(userInfo, loanDefaultData, (LoanTemplateSelection) null);
      this.initLoanData((LoanTemplateSelection) null, "Encompass - Loan Duplication", true);
      this.loanData.SetCurrentField("LE2.X28", field4);
      this.loanData.SetCurrentField("CD3.X129", "");
      if (this.loanData.GetField("PlanCode.ID") == field3 && this.loanData.GetField("1881") == field2)
        this.loanData.SetCurrentField("Docs.Engine", field1);
      ILoanManager loanManager = this.LoanManager;
      this.loanData.MersNumber = this.GetNextMersNumber();
      string mersOrgId = this.loanData.MersOrgId;
      if (!string.IsNullOrEmpty(this.loanData.MersNumber) && this.loanData.MersNumber.Length >= 7)
      {
        this.loanData.Mom = "Y";
        this.loanData.MersOrgId = this.loanData.MersNumber.Substring(0, 7);
      }
      this.loanData.SetCurrentField("LINKGUID", "");
      for (int index = 1; index <= 12; ++index)
      {
        if (index != 2 && index != 7)
          this.loanData.SetCurrentField("TPO.X" + (object) index, "");
      }
      this.loanData.SetCurrentField("TPO.X92", "");
      this.loanData.SetCurrentField("2336", "");
      foreach (BorrowerPair borrowerPair in this.loanData.GetBorrowerPairs())
        this.loanData.SetCurrentField("5040", "", borrowerPair);
      this.loanData.RemoveTQLFraudAlerts();
      this.loanData.RemoveTQLComplianceAlerts();
      this.loanData.RemoveTQLDocDeliveryDates();
      for (int index = 1; index <= 32; ++index)
        this.loanData.SetCurrentField("COMPLIANCEREVIEW.X" + (object) index, "");
      this.loanData.Remove4506TReports();
      for (int index = 1; index <= 4; ++index)
        this.loanData.SetCurrentField("TQL.X" + (object) index, "");
      for (int index = 27; index <= 79; ++index)
        this.loanData.SetCurrentField("TQL.X" + (object) index, "");
      this.loanData.SetCurrentField("3340", "");
      this.loanData.SetCurrentField("3356", "");
      for (int index = 1; index <= 38; ++index)
        this.loanData.SetCurrentField("LCP.X" + (object) index, "");
      this.loanData.SetCurrentField("2400", "");
      this.loanData.SetCurrentField("763", "");
      this.loanData.SetCurrentField("431", "");
      this.loanData.SetCurrentField("761", "");
      this.loanData.SetCurrentField("432", "");
      this.loanData.SetCurrentField("762", "");
      this.loanData.SetCurrentField("3253", "");
      this.loanData.SetCurrentField("3941", "");
      this.loanData.SetCurrentField("OPTIMAL.HISTORY", "");
      this.loanData.SetCurrentField("4527", "");
      this.loanData.SetCurrentField("4528", "");
      this.loanData.SetCurrentField("4529", "");
      this.loanData.SetCurrentField("1885", "");
      this.loanData.SetCurrentField("1886", "");
      this.loanData.SetCurrentField("1887", "");
      this.loanData.SetCurrentField("745", DateTime.Today.ToString("MM/dd/yyyy"));
      this.loanData.SetCurrentField("3122", "");
      this.loanData.SetCurrentField("3170", "");
      this.loanData.SetCurrentField("2399", "");
      this.loanData.SetCurrentField("3355", "");
      this.loanData.SetCurrentField("3293", "");
      this.loanData.SetCurrentField("3900", "");
      int numberOfDeposits = this.loanData.GetNumberOfDeposits();
      for (int index = 1; index <= numberOfDeposits; ++index)
        this.loanData.SetField("DD" + index.ToString("00") + "97", "Y");
      int numberOfEmployer1 = this.loanData.GetNumberOfEmployer(true);
      for (int index = 1; index <= numberOfEmployer1; ++index)
        this.loanData.SetField("BE" + index.ToString("00") + "97", "Y");
      int numberOfEmployer2 = this.loanData.GetNumberOfEmployer(false);
      for (int index = 1; index <= numberOfEmployer2; ++index)
        this.loanData.SetField("CE" + index.ToString("00") + "97", "Y");
      int exlcudingAlimonyJobExp = this.loanData.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
        this.loanData.SetField("FL" + index.ToString("00") + "97", "Y");
      int numberOfMortgages = this.loanData.GetNumberOfMortgages();
      for (int index = 1; index <= numberOfMortgages; ++index)
        this.loanData.SetField("FM" + index.ToString("00") + "97", "Y");
      int numberOfResidence1 = this.loanData.GetNumberOfResidence(true);
      for (int index = 1; index <= numberOfResidence1; ++index)
        this.loanData.SetField("BR" + index.ToString("00") + "97", "Y");
      int numberOfResidence2 = this.loanData.GetNumberOfResidence(false);
      for (int index = 1; index <= numberOfResidence2; ++index)
        this.loanData.SetField("CR" + index.ToString("00") + "97", "Y");
      this.loanData.SetCurrentField("3908", "");
      this.loanData.SetCurrentField("3907", "");
      for (int index = 3943; index <= 3964; ++index)
        this.loanData.SetCurrentField(index.ToString(), "");
      this.loanData.SetCurrentField("3908", "");
      for (int index = 3916; index <= 3930; ++index)
        this.loanData.SetCurrentField(string.Concat((object) index), "");
      this.loanData.SetCurrentField("3578", "");
      this.loanData.SetCurrentField("3940", "");
      this.loanData.SetCurrentField("3935", "");
      this.loanData.SetField("CORRESPONDENT.X565", "");
      for (int index = 4789; index <= 4791; ++index)
        this.loanData.SetCurrentField(string.Concat((object) index), "");
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) new string[73]
      {
        "3983",
        "3984",
        "3985",
        "3986",
        "3987",
        "3988",
        "3989",
        "3990",
        "3991",
        "3992",
        "3993",
        "3994",
        "3995",
        "3996",
        "3997",
        "3998",
        "3999",
        "4023",
        "4024",
        "4025",
        "4026",
        "4027",
        "4028",
        "4029",
        "4030",
        "4031",
        "4032",
        "4033",
        "4034",
        "4035",
        "4036",
        "4037",
        "4038",
        "4039",
        "4040",
        "4041",
        "4042",
        "4043",
        "4044",
        "4045",
        "4046",
        "4047",
        "4048",
        "4049",
        "4050",
        "4051",
        "4052",
        "4053",
        "4054",
        "4989",
        "4990",
        "4991",
        "4992",
        "4993",
        "4994",
        "4995",
        "4996",
        "4997",
        "4998",
        "4999",
        "5000",
        "4956",
        "4957",
        "4958",
        "4959",
        "4960",
        "4961",
        "4962",
        "4963",
        "4964",
        "4965",
        "4966",
        "4967"
      });
      List<int> linkedVestingIdxList = this.loanData.GetNBOLinkedVestingIdxList();
      string[] strArray = new string[6]
      {
        "17",
        "18",
        "19",
        "20",
        "39",
        "40"
      };
      foreach (int num in linkedVestingIdxList)
      {
        string str1 = "NBOC";
        string str2 = num.ToString("00");
        foreach (string str3 in strArray)
        {
          string str4 = str1 + str2 + str3;
          stringList.Add(str4);
        }
      }
      foreach (string id in stringList)
        this.loanData.SetCurrentField(id, "");
      this.loanData.SetCurrentField("UCD.X1", "");
      this.loanData.SetCurrentField("4797", "");
      this.loanData.SetCurrentField("4798", "");
      this.loanData.SetCurrentField("4799", "");
      this.loanData.SetCurrentField("4800", "");
      this.loanData.RemoveDisasters();
      this.loanData.SetCurrentField("4953", "");
      for (int index = 201; index <= 206; ++index)
        this.loanData.SetCurrentField("ULDD.X" + (object) index, "");
      try
      {
        BorrowerPair[] borrowerPairs = this.loanData.GetBorrowerPairs();
        for (int index1 = 0; index1 < borrowerPairs.Length; ++index1)
        {
          this.loanData.SetCurrentField("4894", "", borrowerPairs[index1]);
          this.loanData.SetCurrentField("4895", "", borrowerPairs[index1]);
          this.loanData.SetCurrentField("4896", "", borrowerPairs[index1]);
          this.loanData.SetCurrentField("4897", "", borrowerPairs[index1]);
          for (int index2 = 4899; index2 <= 4906; ++index2)
            this.loanData.SetCurrentField(index2.ToString(), "", borrowerPairs[index1]);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), ex.Message);
      }
      LoanDuplicationTemplate duplicationTemplate = (LoanDuplicationTemplate) null;
      try
      {
        if (templateToUse != string.Empty)
          duplicationTemplate = (LoanDuplicationTemplate) this.sessionObjects.ConfigurationManager.GetTemplateSettings(TemplateSettingsType.LoanDuplicationTemplate, new FileSystemEntry(FileSystemEntry.PublicRoot.Path, templateToUse, FileSystemEntry.Types.File, (string) null));
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), "Can't open Loan Duplication Template '" + templateToUse + "'. Message: " + ex.Message);
      }
      if (duplicationTemplate != null)
      {
        bool allowUrlA2020 = this.sessionObjects.StartupInfo.AllowURLA2020;
        string[] additionalFields = duplicationTemplate.GetAdditionalFields();
        bool flag = true;
        if (additionalFields != null && additionalFields.Length != 0)
        {
          for (int index = 0; index < additionalFields.Length; ++index)
          {
            if (additionalFields[index] == "1825")
            {
              flag = false;
              break;
            }
          }
        }
        if (flag)
        {
          DateTime date = EllieMae.EMLite.Common.Utils.ParseDate((object) this.sessionObjects.ConfigurationManager.GetCompanySetting("Policies", "URLA2020"));
          if (allowUrlA2020)
            this.loanData.SetCurrentField("1825", DateTime.Now.Date >= date.Date ? "2020" : "2009");
          else if (EllieMae.EMLite.Common.Utils.ParseInt((object) this.loanData.GetSimpleField("HMDA.X27")) > 2017)
            this.loanData.SetCurrentField("1825", "2009");
        }
        if (additionalFields != null && additionalFields.Length != 0)
          this.copyDuplicatedFields(additionalFields, (string[]) null, sourceLoanData, true);
        LoanDuplicationDefaultTemplate duplicationDefaultTemplate = (LoanDuplicationDefaultTemplate) null;
        try
        {
          string empty = string.Empty;
          using (BinaryObject binaryObject = new BinaryObject(!AssemblyResolver.IsSmartClient ? Path.Combine(SystemSettings.DocDirAbsPath, "LoanDuplicationDefaultTemplate.xml") : AssemblyResolver.GetResourceFileFullPath(SystemSettings.DocDirRelPath + "LoanDuplicationDefaultTemplate.xml")))
          {
            if (binaryObject != null)
              duplicationDefaultTemplate = (LoanDuplicationDefaultTemplate) binaryObject;
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), "Can't find LoanDuplicationDefaultTemplate.xml file in Documents folder. Message: " + ex.Message);
        }
        if (duplicationDefaultTemplate != null)
        {
          if (duplicationTemplate.GetField("BorrowerInformation") == "1")
            this.copyDuplicatedFields(duplicationDefaultTemplate.GetFieldIDs(LoanDuplicationDefaultTemplate.FieldTypes.BorrowerInformation, allowUrlA2020), (string[]) null, sourceLoanData, true);
          if (duplicationTemplate.GetField("BorrowerEmployerInformation") == "1")
            this.copyDuplicatedFields(duplicationDefaultTemplate.GetFieldIDs(LoanDuplicationDefaultTemplate.FieldTypes.BorrowerEmployerInformation, allowUrlA2020), (string[]) null, sourceLoanData, true);
          if (duplicationTemplate.GetField("Co-BorrowerInformation") == "1")
            this.copyDuplicatedFields(duplicationDefaultTemplate.GetFieldIDs(LoanDuplicationDefaultTemplate.FieldTypes.CoBorrowerInformation, allowUrlA2020), (string[]) null, sourceLoanData, true);
          if (duplicationTemplate.GetField("Co-BorrowerEmployerInformation") == "1")
            this.copyDuplicatedFields(duplicationDefaultTemplate.GetFieldIDs(LoanDuplicationDefaultTemplate.FieldTypes.CoBorrowerEmployerInformation, allowUrlA2020), (string[]) null, sourceLoanData, true);
          if (duplicationTemplate.GetField("BorrowerPresentAddress") == "1")
          {
            string field5 = duplicationTemplate.GetField("BorrowerAddressTo");
            string[] fieldIds = duplicationDefaultTemplate.GetFieldIDs(LoanDuplicationDefaultTemplate.FieldTypes.BorrowerPresentAddress, allowUrlA2020);
            if (field5.ToLower() == "borrower's prior residence")
              this.copyDuplicatedFields(fieldIds, duplicationDefaultTemplate.GetFieldIDs(LoanDuplicationDefaultTemplate.FieldTypes.BorrowerPriorAddress, allowUrlA2020), sourceLoanData, true);
            else
              this.copyDuplicatedFields(fieldIds, (string[]) null, sourceLoanData, true);
          }
          if (duplicationTemplate.GetField("Co-BorrowerPresentAddress") == "1")
          {
            string field6 = duplicationTemplate.GetField("CoBorrowerAddressTo");
            string[] fieldIds = duplicationDefaultTemplate.GetFieldIDs(LoanDuplicationDefaultTemplate.FieldTypes.CoBorrowerPresentAddress, allowUrlA2020);
            if (field6.ToLower() == "co-borrower's prior residence")
              this.copyDuplicatedFields(fieldIds, duplicationDefaultTemplate.GetFieldIDs(LoanDuplicationDefaultTemplate.FieldTypes.CoBorrowerPriorAddress, allowUrlA2020), sourceLoanData, true);
            else
              this.copyDuplicatedFields(fieldIds, (string[]) null, sourceLoanData, true);
          }
          if (duplicationTemplate.GetField("Property") == "1")
          {
            string field7 = duplicationTemplate.GetField("PropertyAddressTo");
            string[] fieldIds = duplicationDefaultTemplate.GetFieldIDs(LoanDuplicationDefaultTemplate.FieldTypes.PropertyAddress, allowUrlA2020);
            switch (field7.ToLower())
            {
              case "borrower's and co-borrower's present address":
                this.copyDuplicatedFields(fieldIds, duplicationDefaultTemplate.GetFieldIDs(LoanDuplicationDefaultTemplate.FieldTypes.BorrowerPresentAddress, allowUrlA2020), sourceLoanData, true);
                this.copyDuplicatedFields(fieldIds, duplicationDefaultTemplate.GetFieldIDs(LoanDuplicationDefaultTemplate.FieldTypes.CoBorrowerPresentAddress, allowUrlA2020), sourceLoanData, true);
                break;
              case "borrower's present address":
                this.copyDuplicatedFields(fieldIds, duplicationDefaultTemplate.GetFieldIDs(LoanDuplicationDefaultTemplate.FieldTypes.BorrowerPresentAddress, allowUrlA2020), sourceLoanData, true);
                break;
              case "co-borrower's present address":
                this.copyDuplicatedFields(fieldIds, duplicationDefaultTemplate.GetFieldIDs(LoanDuplicationDefaultTemplate.FieldTypes.CoBorrowerPresentAddress, allowUrlA2020), sourceLoanData, true);
                break;
              default:
                this.copyDuplicatedFields(fieldIds, (string[]) null, sourceLoanData, false);
                break;
            }
          }
        }
        foreach (BorrowerPair borrowerPair in this.loanData.GetBorrowerPairs())
        {
          this.loanData.SetBorrowerPair(borrowerPair);
          if (this.loanData.Calculator != null)
          {
            this.loanData.Calculator.FormCalculation("4000", (string) null, (string) null);
            this.loanData.Calculator.FormCalculation("4002", (string) null, (string) null);
            this.loanData.Calculator.FormCalculation("4004", (string) null, (string) null);
            this.loanData.Calculator.FormCalculation("4006", (string) null, (string) null);
          }
        }
      }
      this.loanData.LoanNumber = !loanManager.IsTimeToSetLoanNumber(this.loanData) ? "" : loanManager.GetNextLoanNumber();
      if (this.loanData.Calculator != null)
      {
        this.loanData.Calculator.FormCalculation("CALCULATEULI");
        this.loanData.Calculator.FormCalculation("CALCURLALOANIDENTIFIER");
        this.loanData.Calculator.SpecialCalculation(CalculationActionID.UpdateBorrowerOnLockRequestForm);
        this.loanData.Calculator.FormCalculation("CALCAUTOMATEDDISCLOSURES");
        this.loanData.Calculator.CalculateAll();
      }
      this.loanData.SetField("FV.X379", "");
      this.loanData.SetField("FV.X380", "");
      this.loanData.SetField("FV.X383", "");
      this.loanData.SetField("FV.X384", "");
      this.initComplianceVersionField(this.loanData, this.loanData.CurrentLoanVersion);
      if (this.loanData.Calculator != null)
        this.loanData.Calculator.UpdateLogs();
      if (this.loanData.GetField("3164") != "Y")
        this.loanData.SetField("ConsumerHIOrderEligible", "");
      try
      {
        this.loanData.SetField("CORRESPONDENT.X556", "");
        this.loanData.SetField("CORRESPONDENT.X557", "");
        this.loanData.SetField("CORRESPONDENT.X558", "");
        this.loanData.SetField("CORRESPONDENT.X559", "");
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), "Can't set values to blank" + templateToUse + "'. Message: " + ex.Message);
      }
    }

    public void copyDuplicatedFields(
      string[] sourceFields,
      string[] targetFields,
      LoanData sourceLoanData,
      bool allPairs)
    {
      if (sourceFields == null || sourceFields.Length == 0)
        return;
      if (targetFields == null)
        targetFields = sourceFields;
      BorrowerPair[] borrowerPairs1 = this.loanData.GetBorrowerPairs();
      BorrowerPair[] borrowerPairs2 = sourceLoanData.GetBorrowerPairs();
      if (allPairs)
      {
        for (; borrowerPairs1.Length < borrowerPairs2.Length; borrowerPairs1 = this.loanData.GetBorrowerPairs())
          this.loanData.CreateBorrowerPair();
      }
      int numberOfMortgages = sourceLoanData.GetNumberOfMortgages();
      int numberOfEmployer1 = sourceLoanData.GetNumberOfEmployer(true);
      int numberOfEmployer2 = sourceLoanData.GetNumberOfEmployer(false);
      for (int borIndex = 0; borIndex < borrowerPairs1.Length; ++borIndex)
      {
        this.loanData.SetBorrowerPair(borrowerPairs1[borIndex]);
        for (int index = 0; index < targetFields.Length && (!(sourceFields[index] == "13") || !targetFields[index].StartsWith("FR")); ++index)
        {
          try
          {
            FieldDefinition field = EncompassFields.GetField(targetFields[index]);
            if (field != null && field.Category == FieldCategory.Common)
            {
              if (borIndex > 0)
                continue;
            }
            if ((targetFields[index].StartsWith("FM") || targetFields[index].StartsWith("BE") || targetFields[index].StartsWith("CE") || targetFields[index].StartsWith("FE") && !targetFields[index].StartsWith("FEMA")) && targetFields[index].Length == 6)
            {
              int num = EllieMae.EMLite.Common.Utils.ParseInt((object) targetFields[index].Substring(2, 2));
              if (targetFields[index].StartsWith("FM"))
              {
                if (num > numberOfMortgages)
                  continue;
              }
              else if (targetFields[index].StartsWith("BE"))
              {
                if (num > numberOfEmployer1)
                  continue;
              }
              else if (targetFields[index].StartsWith("CE"))
              {
                if (num > numberOfEmployer2)
                  continue;
              }
              else if (targetFields[index].StartsWith("FE"))
              {
                if (num == 1)
                {
                  if (numberOfEmployer1 < 1)
                    continue;
                }
                if (num == 3)
                {
                  if (numberOfEmployer1 < 2)
                    continue;
                }
                if (num == 5)
                {
                  if (numberOfEmployer1 < 3)
                    continue;
                }
                if (num == 2)
                {
                  if (numberOfEmployer2 < 1)
                    continue;
                }
                if (num == 4)
                {
                  if (numberOfEmployer2 < 2)
                    continue;
                }
                if (num == 6)
                {
                  if (numberOfEmployer2 < 3)
                    continue;
                }
              }
            }
            this.loanData.SetCurrentField(targetFields[index], sourceLoanData.GetField(sourceFields[index], borIndex), false);
            if (string.Compare(sourceFields[index], targetFields[index], true) == 0)
            {
              if (sourceLoanData.IsLocked(sourceFields[index]))
                this.loanData.AddLock(targetFields[index]);
            }
          }
          catch (Exception ex)
          {
            Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), "Can't copy field '" + sourceFields[index] + "' to '" + targetFields[index] + "' field in new loan file. Message: " + ex.Message);
          }
        }
        if (!allPairs)
          break;
      }
    }

    public string GetNextMersNumber()
    {
      string userId = this.loanData.GetField("LOID");
      if (userId == "")
        userId = this.loanData.GetLogList().GetMilestone("Started").LoanAssociateID;
      if (userId == "")
        userId = this.sessionObjects.UserID;
      UserInfo user = this.sessionObjects.OrganizationManager.GetUser(userId);
      if (user == (UserInfo) null)
        return "";
      OrgInfo organizationWithMersmin = this.sessionObjects.OrganizationManager.GetFirstOrganizationWithMERSMIN(user.OrgId);
      if (organizationWithMersmin.MERSMINCode == "")
      {
        MersNumberingInfo mersNumberingInfo = this.sessionObjects.ConfigurationManager.GetMersNumberingInfo();
        if (mersNumberingInfo.CompanyID.Trim() == "" || mersNumberingInfo.NextNumber.Trim() == string.Empty)
          return "";
      }
      else
      {
        BranchMERSMINNumberingInfo mersNumberingInfo = this.sessionObjects.ConfigurationManager.GetBranchMERSNumberingInfo(organizationWithMersmin.MERSMINCode);
        if (mersNumberingInfo.MERSMINCode.Trim() == "" || mersNumberingInfo.NextNumber.Trim() == string.Empty)
          return "";
      }
      return this.sessionObjects.LoanManager.GetNextMersNumber(false, organizationWithMersmin);
    }

    private void initLoanData(
      LoanTemplateSelection templateSelection,
      string loanSource,
      bool isDuplicated,
      bool skipLoanOfficerAssignment = false)
    {
      DateTime now = DateTime.Now;
      UserInfo userInfo = this.sessionObjects.UserInfo;
      this.loanData.SetCurrentField("1601", "PREQUAL");
      if (!isDuplicated)
      {
        this.loanData.SetCurrentField("NEWHUD.X1139", "Y");
        this.loanData.SetCurrentField("NEWHUD.X1140", "Y");
        this.loanData.SetCurrentField("4460", "1");
      }
      this.loanData.SetCurrentField("Docs.Engine", "New_Encompass_Docs_Solution");
      this.loanData.SetCurrentField("675", "N");
      this.loanData.SetCurrentField("PAYMENTTABLE.USEACTUALPAYMENTCHANGE", "");
      RolesMappingInfo[] userRoleMappings = this.configInfo.UserRoleMappings;
      if (userRoleMappings != null)
      {
        foreach (RolesMappingInfo rolesMappingInfo in userRoleMappings)
        {
          if (rolesMappingInfo.RealWorldRoleID == RealWorldRoleID.LoanProcessor)
          {
            this.loanData.SetField("2006", userInfo.FullName);
            this.loanData.SetField("2843", userInfo.Phone);
            this.loanData.SetField("2844", userInfo.Fax);
            break;
          }
        }
      }
      List<EllieMae.EMLite.Workflow.Milestone> milestonesList = this.configInfo.MilestonesList;
      OrgInfo displayOrganization = this.configInfo.DisplayOrganization;
      string val = string.Empty;
      string str = string.Empty;
      if (displayOrganization != null)
      {
        str = displayOrganization.CompanyPhone;
        val = displayOrganization.CompanyFax;
      }
      if (this.loanData.IsFieldDefined("HMDA.X91"))
        this.loanData.SetField("HMDA.X91", "Y");
      this.loanData.SetField("IRS4506.X93", "4506-COct2022");
      this.loanData.SetField("IRS4506.X92", "Oct2022");
      if (this.loanData.Settings != null && this.loanData.Settings.HMDAInfo != null && this.loanData.Calculator != null)
        this.loanData.Calculator.FormCalculation("UPDATEHMDA2018", (string) null, (string) null);
      if (this.loanData.Settings != null && this.loanData.Settings.HMDAInfo != null)
      {
        this.loanData.SetField("HMDA.X100", this.loanData.Settings.HMDAInfo.HMDAProfileID);
        if (EllieMae.EMLite.Common.Utils.ParseInt((object) this.loanData.GetSimpleField("HMDA.X27")) > 2017 && this.loanData.Settings.HMDAInfo.HMDAChannelInfoNoChannel == "Correspondent" && this.loanData.Settings.HMDAInfo.HMDAChannelInfoCorrespondent == "Purchased")
          this.loanData.SetField("1393", "Loan purchased by your institution");
      }
      if (EllieMae.EMLite.Common.Utils.ParseInt((object) this.loanData.GetSimpleField("HMDA.X27")) > 2017)
      {
        this.loanData.SetField("4142", "Y");
        this.loanData.SetField("HMDA.X121", "Y");
      }
      string field1 = this.loanData.GetField("1825");
      LoanTemplateSet loanTemplateSet = new LoanTemplateSet();
      Hashtable hashtable = (Hashtable) null;
      if (templateSelection != null)
      {
        hashtable = this.sessionObjects.ConfigurationManager.GetLoanTemplateComponents(templateSelection.TemplateEntry);
        if (Convert.ToString(hashtable[(object) "MILETEMP"]) != string.Empty)
          loanTemplateSet.MilestoneTemplate = this.sessionObjects.BpmManager.GetMilestoneTemplateByGuid(Convert.ToString(hashtable[(object) "MILETEMP"]));
      }
      LogList logList = this.loanData.GetLogList();
      if (loanTemplateSet.MilestoneTemplate != null)
      {
        if (loanTemplateSet.MilestoneTemplate.Active)
        {
          LoanDataMgr.replaceTemplate(this.sessionObjects, loanTemplateSet.MilestoneTemplate, hashtable[(object) "LOANTEMPLATEFILE"].ToString(), true, false, this.loanData, false);
        }
        else
        {
          loanTemplateSet.MilestoneTemplate = this.sessionObjects.BpmManager.GetDefaultMilestoneTemplate();
          LoanDataMgr.replaceTemplate(this.sessionObjects, loanTemplateSet.MilestoneTemplate, (string) null, false, false, this.loanData, false);
        }
      }
      else
      {
        loanTemplateSet.MilestoneTemplate = this.sessionObjects.BpmManager.GetDefaultMilestoneTemplate();
        LoanDataMgr.replaceTemplate(this.sessionObjects, loanTemplateSet.MilestoneTemplate, (string) null, false, false, this.loanData, false);
      }
      if (isDuplicated)
        LoanDataMgr.setMilestoneTemplateOnNew(this.sessionObjects, this.loanData, "", false);
      bool flag1 = false;
      int num = -1;
      if (userRoleMappings != null)
      {
        foreach (RolesMappingInfo rolesMappingInfo in userRoleMappings)
        {
          if (rolesMappingInfo.RealWorldRoleID == RealWorldRoleID.LoanOfficer || rolesMappingInfo.RealWorldRoleID == RealWorldRoleID.TPOLoanOfficer)
          {
            num = rolesMappingInfo.RoleIDList[0];
            flag1 = true;
            break;
          }
        }
      }
      LOLicenseInfo[] loLicInfo = (LOLicenseInfo[]) null;
      if (flag1)
      {
        string field2 = this.loanData.GetField("14");
        if (field2 != "")
        {
          loLicInfo = this.isUserLicensedForState(userInfo.Userid, field2);
          if (loLicInfo == null)
            flag1 = false;
        }
      }
      if (!skipLoanOfficerAssignment & flag1)
      {
        try
        {
          this.loanData.SetField("317", userInfo.FullName);
          this.loanData.SetField("4508", userInfo.JobTitle);
          if (this.loanData.Calculator != null)
            this.loanData.Calculator.FormCalculation("317", (string) null, (string) null);
          this.loanData.SetField("1407", userInfo.Email);
          this.loanData.SetField("1406", userInfo.Phone);
          this.loanData.SetField("LOID", userInfo.Userid);
          if ((userInfo.Fax ?? "") != string.Empty)
            this.loanData.SetField("2411", userInfo.Fax);
          else
            this.loanData.SetField("2411", val);
          this.loanData.SetField("2854", userInfo.CellPhone);
          this.populateStateLicense(userInfo, loLicInfo);
          if (logList.GetMilestoneAt(1).RoleID == num)
          {
            EllieMae.EMLite.DataEngine.Log.MilestoneLog milestoneAt = logList.GetMilestoneAt(1);
            milestoneAt.SetLoanAssociate(userInfo);
            milestoneAt.Reviewed = true;
            if ((userInfo.Fax ?? "") == string.Empty)
              logList.GetMilestoneAt(1).LoanAssociateFax = val;
          }
          if (this.configInfo.InterviewerPopulation == InterviewerInfoSetting.LoanOfficer)
          {
            DateTime dateTime = DateTime.Today;
            dateTime = dateTime.Date;
            if (dateTime.CompareTo(userInfo.NMLSExpirationDate.Date) <= 0)
            {
              OrgInfo organizationWithNmls = this.sessionObjects.OrganizationManager.GetFirstOrganizationWithNMLS(userInfo.OrgId);
              this.setLoanOriginatorInfo(userInfo, organizationWithNmls, 1, this.loanData.GetField("1825") == "2020");
            }
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), "initLoanData: can't get role mapping info for LO. Error: " + ex.Message);
        }
      }
      this.loanData.SetField("3639", "Y");
      this.loanData.SetField("3297", this.loanData.GetField("315"));
      this.loanData.SetField("3298", this.loanData.GetField("319"));
      this.loanData.SetField("3299", this.loanData.GetField("313"));
      this.loanData.SetField("3300", this.loanData.GetField("321"));
      this.loanData.SetField("3301", this.loanData.GetField("323"));
      if (this.configInfo.LoanCompDefaultPlan != null && this.configInfo.LoanCompDefaultPlan.PaidByString != string.Empty)
        this.loanData.SetCurrentField("LCP.X1", this.configInfo.LoanCompDefaultPlan.PaidByString);
      else
        this.loanData.SetCurrentField("LCP.X1", "Lender Paid");
      this.LoanData.SetCurrentField("4463", this.LoanData.GetField("LCP.X1"));
      this.loanData.SetField("LE2.X30", "N");
      this.loanData.SetField("LE3.X19", "AsApplicant");
      this.loanData.SetField("CD5.X67", "AsApplicant");
      this.loanData.SetField("CPA.RetainUserInputs", "N");
      this.loanData.SetCurrentField("VASUMM.X140", "Y");
      this.loanData.SetCurrentField("VASUMM.X141", "Y");
      this.loanData.SetCurrentField("VASUMM.X142", "Y");
      this.loanData.SetCurrentField("VASUMM.X143", "Y");
      this.loanData.SetCurrentField("VASUMM.X144", "Y");
      this.loanData.SetCurrentField("VASUMM.X145", "Y");
      this.loanData.SetCurrentField("VASUMM.X146", "Y");
      this.loanData.SetCurrentField("VASUMM.X147", "Y");
      this.loanData.SetCurrentField("VASUMM.X148", "Y");
      if (templateSelection != null)
      {
        Hashtable templateComponents = this.sessionObjects.ConfigurationManager.GetLoanTemplateComponents(templateSelection.TemplateEntry);
        if (this.configInfo.DisplayBusinessRuleOption == EnableDisableSetting.Enabled)
          this.loanData.TemporaryIgnoreRule = true;
        this.ApplyLoanTemplate(templateComponents, templateSelection.AppendData, false);
        if (this.respaSetByTemplate != null)
          this.loanData.SetCurrentField("3969", this.respaSetByTemplate);
      }
      if (field1 == "2020" && this.loanData.GetField("1825") != "2020")
        this.loanData.SetField("1825", "2020");
      this.setDefaultValuesForItemizationFeesPrintSettings();
      this.loanData.SetField("317", string.Empty);
      this.loanData.SetField("4508", string.Empty);
      this.loanData.SetField("1407", string.Empty);
      this.loanData.SetField("1406", string.Empty);
      this.loanData.SetField("LOID", string.Empty);
      this.loanData.SetField("2411", string.Empty);
      this.loanData.SetField("2854", string.Empty);
      this.loanData.SetField("362", string.Empty);
      this.loanData.SetField("4509", string.Empty);
      this.loanData.SetField("1409", string.Empty);
      this.loanData.SetField("1408", string.Empty);
      this.loanData.SetField("LPID", string.Empty);
      this.loanData.SetField("2412", string.Empty);
      this.loanData.SetField("2855", string.Empty);
      this.loanData.SetField("1855", string.Empty);
      this.loanData.SetField("1856", string.Empty);
      this.loanData.SetField("1857", string.Empty);
      this.loanData.SetField("CLID", string.Empty);
      this.loanData.SetField("2413", string.Empty);
      this.loanData.SetField("2856", string.Empty);
      if (!skipLoanOfficerAssignment & flag1)
      {
        try
        {
          this.loanData.SetField("317", userInfo.FullName);
          this.loanData.SetField("4508", userInfo.JobTitle);
          if (this.loanData.Calculator != null)
            this.loanData.Calculator.FormCalculation("317", (string) null, (string) null);
          this.loanData.SetField("1407", userInfo.Email);
          this.loanData.SetField("1406", userInfo.Phone);
          this.loanData.SetField("LOID", userInfo.Userid);
          if ((userInfo.Fax ?? "") != string.Empty)
            this.loanData.SetField("2411", userInfo.Fax);
          else
            this.loanData.SetField("2411", val);
          this.loanData.SetField("2854", userInfo.CellPhone);
          this.populateStateLicense(userInfo, loLicInfo);
          if (logList.GetMilestoneAt(1).RoleID == num)
          {
            logList.GetMilestoneAt(1).SetLoanAssociate(userInfo);
            if ((userInfo.Fax ?? "") == string.Empty)
              logList.GetMilestoneAt(1).LoanAssociateFax = val;
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), "initLoanData: can't get role mapping info for LO. Error: " + ex.Message);
        }
        try
        {
          if (this.loanData.Calculator != null)
            this.loanData.Calculator.FormCalculation("UPDATECOMP", (string) null, (string) null);
        }
        catch (Exception ex)
        {
          Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), "Cannot apply Loan Compensation due to this error: " + ex.Message);
        }
      }
      if (this.loanData.Calculator != null)
      {
        if (displayOrganization != null && displayOrganization.OrgBranchLicensing != null)
          this.loanData.Calculator.PopulateCompanyStateLicense(displayOrganization.OrgBranchLicensing, false);
        else if (this.configInfo.CompanyInfo != null && this.configInfo.CompanyInfo.StateBranchLicensing != null)
          this.loanData.Calculator.PopulateCompanyStateLicense(this.configInfo.CompanyInfo.StateBranchLicensing, false);
      }
      RoleInfo[] allRoles = this.configInfo.AllRoles;
      string systemId = this.sessionObjects.SystemID;
      if (this.loanData.GetField("1969") == "Y")
      {
        this.loanData.SetField("1264", this.loanData.GetField("315"));
        this.loanData.SetField("1257", this.loanData.GetField("319"));
        this.loanData.SetField("1258", this.loanData.GetField("313"));
        this.loanData.SetField("1259", this.loanData.GetField("321"));
        this.loanData.SetField("1260", this.loanData.GetField("323"));
        this.loanData.SetField("1263", this.loanData.GetField("326"));
        this.loanData.SetField("1256", this.loanData.GetField("317"));
        this.loanData.SetField("1262", this.loanData.GetField("1406") == string.Empty ? this.loanData.GetField("324") : this.loanData.GetField("1406"));
        this.loanData.SetField("95", this.loanData.GetField("1407"));
        this.loanData.SetField("3032", this.loanData.GetField("3629"));
        this.loanData.SetField("3244", this.loanData.GetField("3237"));
      }
      this.loanData.SetField("2026", userInfo.FullName);
      this.loanData.SetField("2027", userInfo.Phone);
      this.loanData.SetCurrentField("2024", loanSource);
      this.loanData.SetCurrentField("2025", now.ToString("MM/dd/yyyy") + " " + now.ToShortTimeString());
      this.loanData.TriggerCalculation("2025", "");
      this.loanData.SetCreatedDateInUTC(now);
      this.loanData.SetCurrentField("2982", EllieMae.EMLite.Common.Utils.ParseInt((object) this.loanData.GetField("1177")) > 0 ? "Y" : "N");
      if (this.loanData.GetField("1757") == string.Empty)
      {
        if (this.loanData.GetField("1172") == "FHA")
          this.loanData.SetCurrentField("1757", "Base Loan Amount");
        else
          this.loanData.SetCurrentField("1757", "Loan Amount");
      }
      if (this.loanData.Calculator != null)
        this.loanData.Calculator.FormCalculation("2982", (string) null, (string) null);
      this.loanData.SetCurrentField("ULDD.X27", "US");
      switch (this.loanData.GetField("19"))
      {
        case "Purchase":
          this.loanData.SetCurrentField("3942", "");
          break;
        case "ConstructionOnly":
        case "ConstructionToPermanent":
          if (!(this.loanData.GetField("CONST.X2") != "Y"))
            goto default;
          else
            goto case "Purchase";
        default:
          this.loanData.SetCurrentField("3942", "N");
          break;
      }
      if (this.loanData.GetField("CD4.X25") == string.Empty)
        this.loanData.SetCurrentField("CD4.X25", "N");
      if (this.loanData.GetField("CD4.X27") == string.Empty)
        this.loanData.SetCurrentField("CD4.X27", "N");
      this.loanData.SetCurrentField("COMPLIANCEVERSION.X1", "");
      if (this.loanData.GetField("CD4.X51") == string.Empty)
        this.loanData.SetCurrentField("CD4.X51", "ConsummationDate");
      this.initComplianceVersionField(this.loanData, this.loanData.CurrentLoanVersion);
      this.loanData.SetField("4568", "Y");
      this.loanData.SetField("4559", "Y");
      bool flag2 = EllieMae.EMLite.Common.Utils.ParseDate((object) this.sessionObjects.ConfigurationManager.GetCompanySetting("Policies", "ENHANCEDCONDITIONSWORKFLOWSTDATE")) <= DateTime.Now.Date;
      this.loanData.SetField("ENHANCEDCOND.X1", this.sessionObjects.ConfigurationManager.GetCompanySetting("Policies", "ENHANCEDCONDITIONSWORKFLOW").ToLower() == "true" & flag2 ? "Y" : "N");
      this.loanData.SetField("TQL.X107", "N");
      if (this.loanData.GetField("4746") == string.Empty)
        this.loanData.SetCurrentField("4746", "AmortizingPayment");
      if (this.loanData.GetField("4747") == string.Empty)
        this.loanData.SetCurrentField("4747", "Actuarial");
      if (this.loanData.GetField("TPO.X119") == string.Empty)
        this.loanData.SetField("TPO.X119", "NoPreference");
      if (!(this.loanData.GetField("DISCLOSURE.X1217") == string.Empty) && !(this.loanData.GetField("DISCLOSURE.X1218") == string.Empty))
        return;
      OrgInfo withStateLicensing = this.sessionObjects.OrganizationManager.GetFirstOrganizationWithStateLicensing(userInfo.OrgId);
      if (withStateLicensing == null)
        return;
      if (this.loanData.GetField("DISCLOSURE.X1218") == string.Empty)
      {
        switch (withStateLicensing.OrgBranchLicensing.StatutoryElectionInMaryland2)
        {
          case "00":
            this.loanData.SetField("DISCLOSURE.X1218", "Credit Grantor Law Election (for 1-4 unit, Jr. Lien only)");
            break;
          case "01":
            this.loanData.SetField("DISCLOSURE.X1218", "Credit Grantor Law Election (for 1-4 unit only)");
            break;
          case "10":
            this.loanData.SetField("DISCLOSURE.X1218", "Credit Grantor Law Election (for All Loans)");
            break;
          default:
            this.loanData.SetField("DISCLOSURE.X1218", "No Statutory Election");
            break;
        }
      }
      if (!(this.loanData.GetField("DISCLOSURE.X1217") == string.Empty))
        return;
      if (withStateLicensing.OrgBranchLicensing.StatutoryElectionInKansas)
        this.loanData.SetField("DISCLOSURE.X1217", "Kansas UCCC Election For All Loans");
      else
        this.loanData.SetField("DISCLOSURE.X1217", "No Statutory Election");
    }

    public void SetMilestoneStatus(string milestoneID, bool finished)
    {
      EllieMae.EMLite.DataEngine.Log.MilestoneLog milestoneById = this.loanData.GetLogList().GetMilestoneByID(milestoneID);
      if (milestoneById == null)
        throw new ArgumentException("A milestone with ID " + milestoneID + " could not be found", nameof (milestoneID));
      this.loanData.GetLogList().GetMilestoneIndex(milestoneById.Stage);
      if (!finished)
      {
        milestoneById.Done = false;
      }
      else
      {
        bool done = milestoneById.Done;
        milestoneById.Done = true;
        milestoneById.Reviewed = true;
        if (!(milestoneById.Stage == "Docs Signing") || !milestoneById.Done)
          return;
        this.loanData.SetCurrentField("1992", DateTime.Now.ToString("MM/dd/yyyy"));
      }
    }

    public TriggerImplDef ApplyLoanTemplateTrigger(
      TriggerConditionType triggerConditionType,
      string milestoneID = null)
    {
      LoanTriggers triggers = (LoanTriggers) this.LoanData.Triggers;
      TriggerImplDef triggerImplDef = (TriggerImplDef) null;
      if (triggers != null)
      {
        TriggerInvoker[] activatedTriggers = triggers.GetCompliedActivatedTriggers();
        if (activatedTriggers != null && activatedTriggers.Length != 0)
        {
          ExecutionContext context = new ExecutionContext(this.sessionObjects.UserInfo, this.loanData, (IServerDataProvider) new CustomCodeSessionDataProvider(this.sessionObjects));
          foreach (TriggerInvoker triggerInvoker in activatedTriggers)
          {
            CompiledTrigger trigger = triggerInvoker.Trigger;
            triggerImplDef = (TriggerImplDef) trigger.Definition;
            if (triggerImplDef.Event.Conditions[0].ConditionType == triggerConditionType && triggerImplDef.Event.Action is TriggerApplyLoanTemplateAction && triggerInvoker.Trigger.ExecuteCondition(context))
            {
              if (triggerConditionType == TriggerConditionType.MilestoneCompleted)
              {
                if (((TriggerMilestoneCompletionCondition) triggerImplDef.Event.Conditions[0]).MilestoneID.Equals(milestoneID))
                {
                  if (triggerImplDef == null)
                  {
                    triggerImplDef = (TriggerImplDef) trigger.Definition;
                    break;
                  }
                  break;
                }
                triggerImplDef = (TriggerImplDef) null;
              }
              else
                break;
            }
            else
              triggerImplDef = (TriggerImplDef) null;
          }
        }
      }
      return triggerImplDef;
    }

    public void ApplyLoanTemplate(TriggerImplDef def)
    {
      TriggerApplyLoanTemplateAction action = (TriggerApplyLoanTemplateAction) def.Event.Action;
      if (string.IsNullOrEmpty(action.FilePath))
        return;
      FileSystemEntry fileSystemEntry = new FileSystemEntry(action.FilePath, FileSystemEntry.Types.File, (string) null);
      try
      {
        LoanTemplate templateSettings = (LoanTemplate) this.sessionObjects.ConfigurationManager.GetTemplateSettings(TemplateSettingsType.LoanTemplate, fileSystemEntry);
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), "Can't open " + fileSystemEntry.Name + " loan template file. Message: " + ex.Message);
      }
      this.ApplyLoanTemplate(fileSystemEntry, true, true);
    }

    public TriggerImplDef ApplyLoanTemplateTrigger(string id, string val)
    {
      LoanTriggers triggers = (LoanTriggers) this.LoanData.Triggers;
      TriggerImplDef triggerImplDef = (TriggerImplDef) null;
      if (triggers != null)
      {
        TriggerInvoker[] activatedTriggers = triggers.GetCompliedActivatedTriggers();
        if (activatedTriggers != null && activatedTriggers.Length != 0)
        {
          ExecutionContext context = new ExecutionContext(this.sessionObjects.UserInfo, this.loanData, (IServerDataProvider) new CustomCodeSessionDataProvider(this.sessionObjects));
          foreach (TriggerInvoker triggerInvoker in activatedTriggers)
          {
            triggerImplDef = (TriggerImplDef) triggerInvoker.Trigger.Definition;
            if (!(triggerImplDef.Event.Conditions[0] is TriggerFieldCondition) || !(triggerImplDef.Event.Action is TriggerApplyLoanTemplateAction) || !this.validateFieldCondition(triggerImplDef.Event.Conditions[0], id, val) || !triggerInvoker.Trigger.ExecuteCondition(context))
              triggerImplDef = (TriggerImplDef) null;
            else
              break;
          }
        }
      }
      return triggerImplDef;
    }

    private bool validateFieldCondition(TriggerCondition cond, string id, string val)
    {
      if (cond.ConditionType == TriggerConditionType.FixedValue)
      {
        TriggerFixedValueCondition fixedValueCondition = (TriggerFixedValueCondition) cond;
        return fixedValueCondition.FieldID.Equals(id) && fixedValueCondition.Value.Equals(val);
      }
      if (cond.ConditionType == TriggerConditionType.NonEmptyValue)
        return ((TriggerFieldCondition) cond).FieldID.Equals(id) && !string.IsNullOrEmpty(val);
      if (cond.ConditionType == TriggerConditionType.Range)
      {
        TriggerRangeCondition triggerRangeCondition = (TriggerRangeCondition) cond;
        if (!triggerRangeCondition.FieldID.Equals(id) || string.IsNullOrEmpty(val))
          return false;
        if (!string.IsNullOrEmpty(triggerRangeCondition.Minimum) && !string.IsNullOrEmpty(triggerRangeCondition.Maximum))
        {
          if (triggerRangeCondition.Minimum.CompareTo(val) <= 0 && triggerRangeCondition.Maximum.CompareTo(val) >= 0)
            return true;
        }
        else if (!string.IsNullOrEmpty(triggerRangeCondition.Minimum))
        {
          if (triggerRangeCondition.Minimum.CompareTo(val) <= 0)
            return true;
        }
        else if (!string.IsNullOrEmpty(triggerRangeCondition.Maximum) && triggerRangeCondition.Maximum.CompareTo(val) >= 0)
          return true;
      }
      else
      {
        if (cond.ConditionType != TriggerConditionType.ValueList)
          return ((TriggerFieldCondition) cond).FieldID.Equals(id);
        TriggerValueListCondition valueListCondition = (TriggerValueListCondition) cond;
        if (!valueListCondition.FieldID.Equals(id))
          return false;
        string empty = string.Empty;
        for (int index = 0; index < valueListCondition.Values.Length; ++index)
        {
          string str = valueListCondition.Values[index];
          if (val.Equals(str))
            return true;
        }
      }
      return false;
    }

    public void SetFutureMilestoneDates(int currentIndex)
    {
      LogList logList = this.loanData.GetLogList();
      AutoDayCountSetting policySetting = (AutoDayCountSetting) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.MilestoneExpDayCount"];
      if (currentIndex == logList.GetNumberOfMilestones() - 1)
        return;
      DateTime dateTime = logList.GetMilestoneAt(currentIndex + 1).Date;
      DateTime date1 = dateTime.Date;
      dateTime = DateTime.MaxValue;
      DateTime date2 = dateTime.Date;
      if (date1 != date2)
      {
        logList.GetMilestoneAt(currentIndex).AdjustDate(logList.GetMilestoneAt(currentIndex).Date, false, true);
      }
      else
      {
        EllieMae.EMLite.DataEngine.Log.MilestoneLog[] allMilestones = logList.GetAllMilestones();
        int numberOfMilestones = logList.GetNumberOfMilestones();
        bool flag1 = false;
        for (int i = currentIndex; i < numberOfMilestones - 1; ++i)
        {
          if (logList.GetMilestoneAt(i).Days != 0)
          {
            flag1 = true;
            break;
          }
        }
        if (flag1)
        {
          EllieMae.EMLite.DataEngine.Log.MilestoneLog milestoneLog = logList.GetMilestoneAt(currentIndex);
          for (int i = currentIndex + 1; i < numberOfMilestones; ++i)
          {
            EllieMae.EMLite.DataEngine.Log.MilestoneLog milestoneAt = logList.GetMilestoneAt(i);
            milestoneAt.AdjustDate(LoanDataMgr.AddDays(logList.GetMilestoneAt(i - 1).Date, milestoneLog.Days, this.sessionObjects), false, false);
            milestoneLog = milestoneAt;
          }
        }
        else
        {
          bool flag2 = true;
          if (allMilestones.Length == logList.GetNumberOfMilestones())
          {
            for (int i = 0; i < allMilestones.Length; ++i)
            {
              if (allMilestones[i].Stage != logList.GetMilestoneAt(i).Stage)
              {
                flag2 = false;
                break;
              }
            }
            if (flag2)
            {
              flag2 = false;
              for (int index = currentIndex + 1; index < allMilestones.Length - 1; ++index)
              {
                if (allMilestones[index].Days != 0)
                {
                  flag2 = true;
                  break;
                }
              }
            }
          }
          else
            flag2 = false;
          if (!flag2)
            return;
          for (int i = currentIndex + 1; i < allMilestones.Length; ++i)
          {
            EllieMae.EMLite.DataEngine.Log.MilestoneLog milestoneAt = logList.GetMilestoneAt(i);
            EllieMae.EMLite.DataEngine.Log.MilestoneLog milestoneLog = allMilestones[i - 1];
            milestoneAt.AdjustDate(LoanDataMgr.AddDays(logList.GetMilestoneAt(i - 1).Date, milestoneLog.Days, this.sessionObjects), false, false);
          }
        }
      }
    }

    private static DateTime AddDays(DateTime date, int dayCount, SessionObjects sessionObjects)
    {
      DateTime date1 = date;
      switch ((AutoDayCountSetting) sessionObjects.StartupInfo.PolicySettings[(object) "Policies.MilestoneExpDayCount"])
      {
        case AutoDayCountSetting.CalendarDays:
          date1 = dayCount == 0 ? date1.AddMinutes(1.0) : date1.AddDays((double) dayCount);
          break;
        case AutoDayCountSetting.CompanyDays:
          try
          {
            date1 = sessionObjects.GetBusinessCalendar(CalendarType.Business).AddBusinessDays(date1, dayCount, false);
            break;
          }
          catch (ArgumentOutOfRangeException ex)
          {
            break;
          }
        default:
          int num = dayCount;
          if (num == 0)
            date1 = date1.AddMinutes(1.0);
          while (num != 0)
          {
            date1 = date1.AddDays(1.0);
            if (date1.DayOfWeek < DayOfWeek.Saturday && date1.DayOfWeek > DayOfWeek.Sunday)
              --num;
          }
          break;
      }
      return date1;
    }

    public DateTime AddBusinessDates(DateTime date, int dayCount)
    {
      return this.sessionObjects.GetBusinessCalendar(CalendarType.RegZ).AddBusinessDays(date, dayCount, false);
    }

    private void populateDefaultFields(
      UserInfo info,
      LoanDefaults defaultData,
      LoanTemplateSelection loanTemplate)
    {
      OrgInfo displayOrganization = this.configInfo.DisplayOrganization;
      if (displayOrganization != null)
      {
        this.loanData.SetCurrentField("ORGID", displayOrganization.OrgCode);
        this.loanData.SetCurrentField("315", displayOrganization.CompanyName);
        this.loanData.SetCurrentField("319", this.getOrgAddress(displayOrganization, this.loanData.GetField("1825") == "2020"));
        this.loanData.SetCurrentField("313", displayOrganization.CompanyAddress.City);
        this.loanData.SetCurrentField("321", displayOrganization.CompanyAddress.State);
        this.loanData.SetCurrentField("323", displayOrganization.CompanyAddress.Zip);
        this.loanData.SetCurrentField("324", displayOrganization.CompanyPhone);
        this.loanData.SetCurrentField("326", displayOrganization.CompanyFax);
        this.loanData.SetCurrentField("3626", displayOrganization.OrgBranchLicensing == null || !displayOrganization.OrgBranchLicensing.UseCustomLenderProfile ? "" : "Y");
      }
      else
      {
        CompanyInfo companyInfo = this.configInfo.CompanyInfo;
        this.loanData.SetCurrentField("ORGID", "");
        this.loanData.SetCurrentField("315", companyInfo.Name);
        if (this.loanData.GetField("1825") == "2020")
          this.loanData.SetCurrentField("URLA.X188", companyInfo.Address);
        this.loanData.SetCurrentField("319", companyInfo.Address);
        this.loanData.SetCurrentField("313", companyInfo.City);
        this.loanData.SetCurrentField("321", companyInfo.State);
        this.loanData.SetCurrentField("323", companyInfo.Zip);
        this.loanData.SetCurrentField("324", companyInfo.Phone);
        this.loanData.SetCurrentField("326", companyInfo.Fax);
        this.loanData.SetCurrentField("3626", companyInfo.StateBranchLicensing == null || !companyInfo.StateBranchLicensing.UseCustomLenderProfile ? "" : "Y");
      }
      if (this.configInfo.InterviewerPopulation == InterviewerInfoSetting.FileStarter)
      {
        OrgInfo organizationWithNmls = this.sessionObjects.OrganizationManager.GetFirstOrganizationWithNMLS(info.OrgId);
        if (organizationWithNmls != null)
          this.loanData.SetCurrentField("3237", organizationWithNmls.NMLSCode);
        else
          this.loanData.SetCurrentField("3237", "");
      }
      if (new LoanDefaultProviders(defaultData.DefaultProviders).ApplyToLoan(this.loanData))
        Tracing.Log(LoanDataMgr.sw, TraceLevel.Verbose, nameof (LoanDataMgr), "Default provider data loaded successfully.");
      DateTime dateTime1;
      if (this.configInfo.InterviewerPopulation != InterviewerInfoSetting.LoanOfficer)
      {
        dateTime1 = DateTime.Today.Date;
        if (dateTime1.CompareTo(info.NMLSExpirationDate.Date) <= 0 && !info.UserType.Equals("External", StringComparison.InvariantCultureIgnoreCase))
          this.setLoanOriginatorInfo(info, (OrgInfo) null, 2, this.loanData.GetField("1825") == "2020");
      }
      if ((info.CHUMId ?? "") != string.Empty)
        this.loanData.SetField("980", info.CHUMId);
      this.populateField(defaultData.RESPAFields.Map);
      this.populateField(defaultData.PrivacyPolicyFields.Map);
      this.populateField(defaultData.FHAConsumerChoiceFieldList.Map);
      if (this.loanData.IsFieldDefined("NOTICES.X98") && defaultData.PrivacyPolicyFields.GetField("NOTICES.X98") == "")
        this.loanData.SetField("NOTICES.X98", "1");
      if (this.loanData.IsFieldDefined("NOTICES.X99") && defaultData.PrivacyPolicyFields.GetField("NOTICES.X99") == "")
        this.loanData.SetField("NOTICES.X99", "11");
      this.loanData.SetField("RE88395.X150", this.loanData.GetSimpleField("315"));
      this.loanData.SetField("RE88395.X338", this.loanData.GetSimpleField("319"));
      this.loanData.SetField("RE88395.X339", this.loanData.GetSimpleField("313"));
      this.loanData.SetField("RE88395.X340", this.loanData.GetSimpleField("321"));
      this.loanData.SetField("RE88395.X341", this.loanData.GetSimpleField("323"));
      this.loanData.SetField("DISCLOSURE.X171", this.loanData.GetSimpleField("315"));
      this.loanData.SetField("RE88395.X151", info.FullName.Trim());
      dateTime1 = DateTime.Now;
      DateTime date = dateTime1.Date;
      this.loanData.SetField("745", date.ToString("MM/dd/yy"));
      if (this.UsePriceBasedQM(date))
        this.loanData.SetField("QM.X383", "Y");
      EncompassDocs.SetDocEngine((IHtmlInput) this.loanData, "New_Encompass_Docs_Solution");
      this.loanData.SetField("181", "");
      this.loanData.SetField("CD3.X137", "N");
      this.loanData.SetCurrentField("CD4.X31", "N", true);
      this.loanData.SetCurrentField("2852", "Y");
      this.loanData.SetCurrentField("NEWHUD.X354", "");
      try
      {
        object policySetting = this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.NewRESPA2015"];
        if (policySetting != null)
        {
          DateTime dateTime2 = (DateTime) policySetting;
          dateTime1 = DateTime.Today;
          if (dateTime1.Date >= dateTime2.Date)
          {
            if (!EllieMae.EMLite.Common.Utils.CheckIf2015RespaTila(this.loanData.GetField("3969")))
              this.loanData.SetField("3969", "RESPA-TILA 2015 LE and CD");
          }
          else
          {
            if (this.loanData.GetField("3969") != "RESPA 2010 GFE and HUD-1")
              this.loanData.SetField("3969", "RESPA 2010 GFE and HUD-1");
            this.loanData.SetField("NEWHUD.X354", "Y");
          }
        }
        if (loanTemplate != null)
          this.setRESPAVersionFromTemplates(this.sessionObjects.ConfigurationManager.GetLoanTemplateComponents(loanTemplate.TemplateEntry));
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), "populateDefaultFields: Can't setup field NEWHUD.X354. Error: " + ex.Message);
      }
      if (EllieMae.EMLite.Common.Utils.CheckIf2015RespaTila(this.loanData.GetField("3969")) && this.loanData.GetField("NEWHUD.X1139") != "Y")
        this.loanData.SetField("NEWHUD.X1139", "Y");
      this.loanData.SetField("ULDD.FNM.FloodSpecialFeatureCode", "180");
      this.loanData.SetField("3841", "NewLock");
      this.loanData.SetField("LE2.X28", "N");
      this.loanData.SetField("1550", "N");
      this.loanData.SetField("3171", "//");
      this.loanData.SetField("3173", "");
      this.loanData.SetField("3172", "");
      this.loanData.SetField("FV.X366", "");
      this.resetDisclousureInformation();
      this.loanData.ResetLogList();
      this.loanData.SetCurrentField("LE1.XG9", EllieMae.EMLite.Common.Utils.TransformSettingTimezoneToCommonFormatTimezone((string) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.CDExpirationTimeZone"]));
      this.loanData.RemoveCurrentLock("LE1.XG9");
      this.SetDefaultValuesForClosingCostExpiration();
      this.setDefaultValuesForRateLockExpiration();
      if (this.IsFromPlatform)
      {
        this.SetExternalOrgLateFeeSettings();
        this.CopyTPOCustomFieldsToLoanFields(info);
      }
      this.loanData.SetField("4912", this.configInfo.LoanSettings.Use5DecimalsForIndexRates ? "FiveDecimals" : "ThreeDecimals");
    }

    private void SetExternalOrgLateFeeSettings()
    {
      ExternalLateFeeSettings orgLateFeeSettings = this.sessionObjects.GetCachedExternalOrgLateFeeSettings(this.loanData.GetField("TPO.X15"));
      if (orgLateFeeSettings == null)
      {
        for (int index = 1; index <= 17; ++index)
          this.loanData.SetField("LATEFEESETTING.X" + (object) index, "");
      }
      else
      {
        this.loanData.SetField("LATEFEESETTING.X1", string.Concat((object) orgLateFeeSettings.CalculateAs));
        this.loanData.SetField("LATEFEESETTING.X2", string.Concat((object) orgLateFeeSettings.FeeHandledAs));
        this.loanData.SetField("LATEFEESETTING.X3", string.Concat((object) orgLateFeeSettings.GracePeriodCalendar));
        this.loanData.SetField("LATEFEESETTING.X4", string.Concat((object) orgLateFeeSettings.GracePeriodLaterOf));
        this.loanData.SetField("LATEFEESETTING.X5", string.Concat((object) orgLateFeeSettings.GracePeriodStarts));
        this.loanData.SetField("LATEFEESETTING.X6", string.Concat((object) orgLateFeeSettings.IncludeDay));
        this.loanData.SetField("LATEFEESETTING.X7", string.Concat((object) orgLateFeeSettings.LateFeeBasedOn));
        this.loanData.SetField("LATEFEESETTING.X8", string.Concat((object) orgLateFeeSettings.MaxLateDays));
        this.loanData.SetField("LATEFEESETTING.X9", orgLateFeeSettings.OtherDate);
        this.loanData.SetField("LATEFEESETTING.X10", string.Concat((object) orgLateFeeSettings.StartOnWeekend));
        this.loanData.SetField("LATEFEESETTING.X11", string.Concat((object) orgLateFeeSettings.GracePeriodDays));
        this.loanData.SetField("LATEFEESETTING.X12", string.Concat((object) orgLateFeeSettings.DayCleared));
        this.loanData.SetField("LATEFEESETTING.X13", orgLateFeeSettings.DayClearedOtherDate);
        this.loanData.SetField("LATEFEESETTING.X18", this.sessionObjects.LoanManager.CalculateDescription(orgLateFeeSettings.DayClearedOtherDate));
        string id1 = orgLateFeeSettings.OtherDate;
        if (!string.IsNullOrEmpty(id1) && id1.StartsWith("Fields."))
          id1 = id1.Substring(7);
        string id2 = orgLateFeeSettings.DayClearedOtherDate;
        if (!string.IsNullOrEmpty(id2) && id2.StartsWith("Fields."))
          id2 = id2.Substring(7);
        this.loanData.SetField("LATEFEESETTING.X14", !string.IsNullOrEmpty(id1) ? this.loanData.GetSimpleField(id1) : "");
        this.loanData.SetField("LATEFEESETTING.X15", !string.IsNullOrEmpty(id2) ? this.loanData.GetSimpleField(id2) : "");
        this.loanData.SetField("LATEFEESETTING.X16", string.Concat((object) orgLateFeeSettings.LateFee));
        this.loanData.SetField("LATEFEESETTING.X17", string.Concat((object) orgLateFeeSettings.Amount));
      }
    }

    private void setRESPAVersionFromTemplates(Hashtable templateSettings)
    {
      if (templateSettings == null)
        return;
      LoanTemplateSet loanTemplateSet = new LoanTemplateSet();
      ClosingCost templateSetting1 = (ClosingCost) (BinaryObject) templateSettings[(object) "COST"];
      DataTemplate templateSetting2 = (DataTemplate) (BinaryObject) templateSettings[(object) "MISCDATA"];
      if (templateSetting1 != null && !string.IsNullOrEmpty(templateSetting1.RESPAVersion))
      {
        if (templateSetting1.RESPAVersion == "2009")
          this.loanData.SetField("3969", "Old GFE and HUD-1");
        else if (templateSetting1.RESPAVersion == "2010")
        {
          this.loanData.SetField("3969", "RESPA 2010 GFE and HUD-1");
        }
        else
        {
          if (!(templateSetting1.RESPAVersion == "2015"))
            return;
          this.loanData.SetField("3969", "RESPA-TILA 2015 LE and CD");
        }
      }
      else
      {
        if (loanTemplateSet.DataTemplate == null || string.IsNullOrEmpty(templateSetting2.RESPAVersion))
          return;
        if (templateSetting2.RESPAVersion == "2009")
          this.loanData.SetField("3969", "Old GFE and HUD-1");
        else if (templateSetting2.RESPAVersion == "2010")
          this.loanData.SetField("3969", "RESPA 2010 GFE and HUD-1");
        else
          this.loanData.SetField("3969", "RESPA-TILA 2015 LE and CD");
      }
    }

    private void resetDisclousureInformation()
    {
      this.loanData.SetField("CD1.X64", "");
      this.loanData.SetField("CD1.X70", "");
      this.loanData.SetField("CD1.X65", "");
      this.loanData.SetField("CD1.X52", "N");
      this.loanData.SetField("CD1.X53", "N");
      this.loanData.SetField("CD1.X54", "N");
      this.loanData.SetField("CD1.X55", "N");
      this.loanData.SetField("CD1.X56", "N");
      this.loanData.SetField("CD1.X57", "N");
      this.loanData.SetField("CD1.X58", "N");
      this.loanData.SetField("CD1.X59", "N");
      this.loanData.SetField("CD1.X66", "N");
      this.loanData.SetField("CD1.X67", "N");
      this.loanData.SetField("CD1.X68", "N");
      this.loanData.SetField("CD1.X60", "");
      this.loanData.SetField("CD1.X62", "//");
      this.loanData.SetField("CD1.X63", "//");
      this.loanData.SetField("CD1.X61", "N");
      this.loanData.SetField("3169", "");
      this.loanData.SetField("LE1.X90", "");
      this.loanData.SetField("LE1.X86", "");
      this.loanData.SetField("LE1.X78", "N");
      this.loanData.SetField("LE1.X79", "N");
      this.loanData.SetField("LE1.X80", "N");
      this.loanData.SetField("LE1.X81", "N");
      this.loanData.SetField("LE1.X82", "N");
      this.loanData.SetField("LE1.X83", "N");
      this.loanData.SetField("LE1.X84", "N");
      this.loanData.SetField("LE1.X85", "");
      this.loanData.SetField("3165", "//");
      this.loanData.SetField("3167", "//");
      this.loanData.SetField("3168", "N");
      this.loanData.SetField("LE1.X33", "//");
      this.loanData.SetField("LE1.X34", "");
      this.loanData.SetField("LE1.X35", "");
      this.loanData.SetField("LE1.X39", "//");
    }

    public void SetDefaultValuesForClosingCostExpiration()
    {
      string policySetting = (string) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.CDExpirationTime"];
      if (policySetting != null)
      {
        this.loanData.SetField("LE1.X8", policySetting);
        if (this.loanData.GetField("3164") != "Y")
          this.loanData.SetField("LE1.XD8", this.loanData.GetField("LE1.X8"));
        else
          this.loanData.SetField("LE1.XD8", "");
      }
      string empty = string.Empty;
      DateTime date = EllieMae.EMLite.Common.Utils.ParseDate((object) this.loanData.GetField("LE1.X28"));
      bool isDaylightSavingTime = !(date == DateTime.MinValue) && System.TimeZoneInfo.Local.IsDaylightSavingTime(date);
      string field = this.loanData.GetField("LE1.XG9");
      string val = !string.IsNullOrEmpty(field) ? EllieMae.EMLite.Common.Utils.TransformTimezoneToStandardTimezone(field, isDaylightSavingTime) : EllieMae.EMLite.Common.Utils.TransformSettingTimezoneToStandardTimezone((string) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.CDExpirationTimeZone"], isDaylightSavingTime);
      if (string.IsNullOrEmpty(val))
        return;
      this.loanData.SetField("LE1.X9", val);
      this.loanData.RemoveCurrentLock("LE1.X9");
      if (this.loanData.GetField("3164") != "Y")
        this.loanData.SetField("LE1.XD9", this.loanData.GetField("LE1.X9"));
      else
        this.loanData.SetField("LE1.XD9", "");
    }

    private void setDefaultValuesForRateLockExpiration()
    {
      string policySetting1 = (string) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.RateLockExpirationTime"];
      if (policySetting1 != null)
        this.loanData.SetField("LE1.X6", policySetting1);
      string policySetting2 = (string) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.RateLockExpirationTimeZone"];
      string empty = string.Empty;
      DateTime date = EllieMae.EMLite.Common.Utils.ParseDate((object) this.loanData.GetField("762"));
      string val = !(date != DateTime.MinValue) ? EllieMae.EMLite.Common.Utils.TransformSettingTimezoneToStandardTimezone(policySetting2, false) : EllieMae.EMLite.Common.Utils.TransformSettingTimezoneToStandardTimezone(policySetting2, System.TimeZoneInfo.Local.IsDaylightSavingTime(date));
      if (!(val != string.Empty))
        return;
      this.loanData.SetField("LE1.X7", val);
    }

    private bool UsePriceBasedQM(DateTime originationDate)
    {
      DateTime date = EllieMae.EMLite.Common.Utils.ParseDate(this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.PRICEBASEDQMDEFINATIONDATE"]);
      return !(originationDate == DateTime.MinValue) && !(date == DateTime.MinValue) && DateTime.Compare(date, originationDate) <= 0;
    }

    private void CopyTPOCustomFieldsToLoanFields(UserInfo userInfo)
    {
      try
      {
        ExternalUserInfo userInfoByContactId = this.GetExternalUserInfoByContactId(userInfo.Userid);
        if ((UserInfo) userInfoByContactId == (UserInfo) null)
          return;
        ContactCustomFieldInfoCollection customFieldInfo = this.sessionObjects.ConfigurationManager.GetCustomFieldInfo();
        if (customFieldInfo == null)
          return;
        IEnumerable<ContactCustomField> companyTpoCustomFields = this.FindCompanyTPOCustomFields(this.GetTPOParentOrgId(this.sessionObjects.ConfigurationManager.GetExternalOrganization(false, userInfoByContactId.ExternalOrgID)));
        if (companyTpoCustomFields == null)
          return;
        foreach (ContactCustomField contactCustomField in companyTpoCustomFields.Where<ContactCustomField>((Func<ContactCustomField, bool>) (x => !string.IsNullOrEmpty(x.FieldValue))))
        {
          foreach (ContactCustomFieldInfo contactCustomFieldInfo in customFieldInfo.Items)
          {
            if (contactCustomFieldInfo.LabelID == contactCustomField.FieldID)
            {
              if (!string.IsNullOrEmpty(contactCustomFieldInfo.LoanFieldId))
              {
                try
                {
                  this.loanData.SetField(contactCustomFieldInfo.LoanFieldId, contactCustomField.FieldValue);
                  if (this.loanData.Settings.FieldSettings.LockRequestAdditionalFields.IsAdditionalField(contactCustomFieldInfo.LoanFieldId, true))
                    this.loanData.SetField(LockRequestCustomField.GenerateCustomFieldID(contactCustomFieldInfo.LoanFieldId), contactCustomField.FieldValue);
                }
                catch (Exception ex)
                {
                  Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), "CopyTPOCustomFieldsToLoanFields: Failed to copy tpo custom fields. Error: " + ex.Message);
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), "CopyTPOCustomFieldsToLoanFields: Failed to copy tpo custom fields. Error: " + ex.Message);
      }
    }

    private ExternalUserInfo GetExternalUserInfoByContactId(string contactId)
    {
      return ((IEnumerable<ExternalUserInfo>) this.sessionObjects.ConfigurationManager.QueryExternalUsers(new List<QueryCriterion>()
      {
        (QueryCriterion) new StringValueCriterion("ContactId", contactId, StringMatchType.Exact)
      }.ToArray())).FirstOrDefault<ExternalUserInfo>();
    }

    private int GetTPOParentOrgId(
      ExternalOriginatorManagementData externalOrganization)
    {
      List<int> intList = new List<int>();
      ExternalOriginatorManagementData originatorManagementData;
      for (originatorManagementData = externalOrganization; originatorManagementData.Parent != 0; originatorManagementData = this.sessionObjects.ConfigurationManager.GetExternalOrganization(false, originatorManagementData.Parent))
      {
        if (intList.Contains(originatorManagementData.Parent))
          throw new Exception("Circular reference found when trying to retrieve parent organization.");
        intList.Add(originatorManagementData.Parent);
        if (!originatorManagementData.InheritCustomFields)
          return originatorManagementData.oid;
      }
      return originatorManagementData.oid;
    }

    private IEnumerable<ContactCustomField> FindCompanyTPOCustomFields(int orgId)
    {
      ContactCustomField[] customFieldValues = this.sessionObjects.ConfigurationManager.GetCustomFieldValues(orgId);
      return customFieldValues != null ? (IEnumerable<ContactCustomField>) ((IEnumerable<ContactCustomField>) customFieldValues).ToList<ContactCustomField>() : (IEnumerable<ContactCustomField>) null;
    }

    public void UpdateCompanyStateLicense(bool clearFieldIfNotFound)
    {
      if (this.loanData == null)
        return;
      string field1 = this.loanData.GetField("315");
      if (string.IsNullOrEmpty(field1))
      {
        if (clearFieldIfNotFound)
          this.loanData.SetField("3629", "");
      }
      else
      {
        string field2 = this.loanData.GetField("319");
        string field3 = this.loanData.GetField("LOID");
        if (string.IsNullOrWhiteSpace(field3))
          field3 = this.loanData.GetField("3239");
        OrgInfo withStateLicensing = this.sessionObjects.OrganizationManager.GetFirstOrganizationWithStateLicensing(field1, field2, field3);
        if (withStateLicensing != null && withStateLicensing.OrgBranchLicensing != null && this.loanData.Calculator != null)
          this.loanData.Calculator.PopulateCompanyStateLicense(withStateLicensing.OrgBranchLicensing, clearFieldIfNotFound);
        else if (clearFieldIfNotFound)
          this.loanData.SetField("3629", "");
      }
      if (this.loanData.Calculator == null)
        return;
      this.loanData.Calculator.FormCalculation("1696", (string) null, (string) null);
    }

    private void populateField(Hashtable map)
    {
      if (map == null)
        return;
      IDictionaryEnumerator enumerator = map.GetEnumerator();
      while (enumerator.MoveNext())
      {
        string key = (string) enumerator.Key;
        string val = (string) enumerator.Value;
        if (key != string.Empty && val != string.Empty && val != null)
          this.loanData.SetCurrentField(key, val);
      }
    }

    public void SetLoanFunder(UserInfo user)
    {
      this.loanData.SetCurrentField("1991", user.FullName);
    }

    public void ClearLoanFunder() => this.loanData.SetCurrentField("1991", "");

    public void SetLoanOfficer(UserInfo userInfo)
    {
      if (userInfo.NMLSExpirationDate != DateTime.MaxValue && DateTime.Today.Date.CompareTo(userInfo.NMLSExpirationDate) > 0)
        return;
      this.loanData.SetField("317", userInfo.FullName);
      this.loanData.SetField("4508", userInfo.JobTitle);
      if (this.loanData.Calculator != null)
        this.loanData.Calculator.FormCalculation("317", (string) null, (string) null);
      this.loanData.SetField("1407", userInfo.Email);
      this.loanData.SetField("LOID", userInfo.Userid);
      this.loanData.SetField("1406", userInfo.Phone);
      this.loanData.SetField("2854", userInfo.CellPhone);
      if ((userInfo.Fax ?? "") != string.Empty)
      {
        this.loanData.SetField("2411", userInfo.Fax);
      }
      else
      {
        OrgInfo displayOrganization = this.configInfo.DisplayOrganization;
        if (displayOrganization != null)
          this.loanData.SetField("2411", displayOrganization.CompanyFax);
        else
          this.loanData.SetField("2411", "");
      }
      this.loanData.SetField("RE88395.X151", userInfo.FullName);
      if (this.loanData.GetField("1969") == "Y")
      {
        this.loanData.SetField("1256", userInfo.FullName);
        this.loanData.SetField("1262", userInfo.Phone == string.Empty ? this.loanData.GetField("324") : userInfo.Phone);
        this.loanData.SetField("95", userInfo.Email);
      }
      OrgInfo avaliableOrganization = this.sessionObjects.OrganizationManager.GetFirstAvaliableOrganization(userInfo.OrgId, true);
      if (avaliableOrganization != null)
      {
        this.loanData.SetField("315", avaliableOrganization.CompanyName);
        this.loanData.SetField("319", this.getOrgAddress(avaliableOrganization, this.loanData.GetField("1825") == "2020"));
        this.loanData.SetField("313", avaliableOrganization.CompanyAddress.City);
        this.loanData.SetField("321", avaliableOrganization.CompanyAddress.State);
        this.loanData.SetField("323", avaliableOrganization.CompanyAddress.Zip);
        this.loanData.SetField("324", avaliableOrganization.CompanyPhone);
        if ((userInfo.Fax ?? "") != string.Empty)
          this.loanData.SetField("326", userInfo.Fax);
        else
          this.loanData.SetField("326", avaliableOrganization.CompanyFax);
        if (this.loanData.Calculator != null)
          this.loanData.Calculator.PopulateCompanyStateLicense(avaliableOrganization.OrgBranchLicensing, true);
      }
      if (this.configInfo.InterviewerPopulation == InterviewerInfoSetting.LoanOfficer)
        this.setInterviewerInformation(userInfo, avaliableOrganization);
      this.loanData.DBANames = (string[]) null;
      this.populateStateLicense(userInfo, (LOLicenseInfo[]) null);
      if (!(this.loanData.GetField("LCP.X19") == string.Empty))
        return;
      try
      {
        if (this.loanData.Calculator == null)
          return;
        this.loanData.Calculator.FormCalculation("updatecomp", (string) null, (string) null);
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), "SetLoanOfficer: Can't setup LO Comp Plan. Error: " + ex.Message);
      }
    }

    private string getOrgAddress(OrgInfo ogi, bool setURLA2020Fields)
    {
      string street1 = ogi.CompanyAddress.Street1;
      this.loanData.SetCurrentField("URLA.X188", street1.Trim());
      this.loanData.SetCurrentField("URLA.X189", ogi.CompanyAddress.UnitType);
      this.loanData.SetCurrentField("URLA.X190", ogi.CompanyAddress.Street2);
      string str = street1 + (!(street1 != "") || !(ogi.CompanyAddress.UnitType != "") ? "" : " ") + ogi.CompanyAddress.UnitType;
      return str + (!(str != "") || !(ogi.CompanyAddress.Street2 != "") ? "" : " ") + ogi.CompanyAddress.Street2;
    }

    public void ClearLoanOfficer()
    {
      this.loanData.SetField("317", "");
      this.loanData.SetField("4508", "");
      if (this.loanData.Calculator != null)
        this.loanData.Calculator.FormCalculation("317", (string) null, (string) null);
      this.loanData.SetField("1407", "");
      this.loanData.SetField("LOID", "");
      this.loanData.SetField("1406", "");
      this.loanData.SetField("2854", "");
      this.loanData.SetField("2411", "");
      this.loanData.SetField("RE88395.X151", "");
      if (this.loanData.GetField("1969") == "Y")
      {
        this.loanData.SetField("1256", "");
        this.loanData.SetField("1262", "");
        this.loanData.SetField("95", "");
      }
      if (this.configInfo.InterviewerPopulation == InterviewerInfoSetting.LoanOfficer)
        this.setInterviewerInformation((UserInfo) null, (OrgInfo) null);
      this.loanData.RemoveLicenseNodes();
    }

    private void setInterviewerInformation(UserInfo userInfo, OrgInfo ogi)
    {
      this.loanData.SetField("3239", userInfo != (UserInfo) null ? userInfo.Userid : "");
      string str = string.Empty;
      EnableDisableSetting enableDisableSetting = EnableDisableSetting.Disabled;
      try
      {
        str = this.loanData.GetField("TPO.X15");
        enableDisableSetting = (EnableDisableSetting) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.TPOOriginator"];
      }
      catch (Exception ex)
      {
      }
      if (str != string.Empty && enableDisableSetting == EnableDisableSetting.Disabled)
        return;
      OrgInfo organizationWithNmls = userInfo != (UserInfo) null ? this.sessionObjects.OrganizationManager.GetFirstOrganizationWithNMLS(userInfo.OrgId) : (OrgInfo) null;
      this.setLoanOriginatorInfo(userInfo, organizationWithNmls, 3, this.loanData.GetField("1825") == "2020");
    }

    private void setLoanOriginatorInfo(
      UserInfo userInfo,
      OrgInfo ogi,
      int setType,
      bool isURLA2020)
    {
      this.loanData.SetField("1612", userInfo != (UserInfo) null ? userInfo.FullName : "");
      if (isURLA2020)
      {
        this.loanData.SetField("URLA.X170", userInfo != (UserInfo) null ? userInfo.FirstName : "");
        this.loanData.SetField("URLA.X171", userInfo != (UserInfo) null ? userInfo.MiddleName : "");
        this.loanData.SetField("URLA.X172", userInfo != (UserInfo) null ? userInfo.LastName : "");
        this.loanData.SetField("URLA.X173", userInfo != (UserInfo) null ? userInfo.SuffixName : "");
      }
      this.loanData.SetField("1823", userInfo != (UserInfo) null ? userInfo.Phone : "");
      this.loanData.SetField("3968", userInfo != (UserInfo) null ? userInfo.Email : "");
      this.loanData.SetField("3238", userInfo != (UserInfo) null ? userInfo.NMLSOriginatorID : "");
      if (setType == 1 || setType == 2)
        this.loanData.SetField("3239", userInfo != (UserInfo) null ? userInfo.Userid : "");
      if (setType != 1 && (setType != 3 || !(userInfo != (UserInfo) null)))
        return;
      if (ogi != null)
        this.loanData.SetField("3237", ogi.NMLSCode);
      else
        this.loanData.SetField("3237", "");
    }

    public string GetCurrentLOInLoanData() => this.GetLoanDataFieldValue("LOID");

    public string GetCurrentLPInLoanData() => this.GetLoanDataFieldValue("LPID");

    public string GetCurrentCLInLoanData() => this.GetLoanDataFieldValue("CLID");

    public string GetCurrentUWInLoanData() => this.GetLoanDataFieldValue("UWID");

    private string GetLoanDataFieldValue(string fieldID) => this.loanData.GetField(fieldID);

    public void ApplyRequiredTasks(TaskMilestonePair[] reqTasks)
    {
      if (reqTasks == null || reqTasks.Length == 0)
        return;
      EllieMae.EMLite.DataEngine.Log.MilestoneLog milestoneLog = (EllieMae.EMLite.DataEngine.Log.MilestoneLog) null;
      EllieMae.EMLite.DataEngine.Log.MilestoneLog[] allMilestones = this.loanData.GetLogList().GetAllMilestones();
      for (int index = 0; index < allMilestones.Length; ++index)
      {
        if (!allMilestones[index].Done)
        {
          milestoneLog = allMilestones[index];
          break;
        }
      }
      if (milestoneLog == null)
        return;
      Hashtable tasksSetup = this.SystemConfiguration.TasksSetup;
      if (tasksSetup == null)
      {
        Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Warning, "No Task definition in Company setting.");
      }
      else
      {
        List<EllieMae.EMLite.Workflow.Milestone> milestonesList = this.SystemConfiguration.MilestonesList;
        EllieMae.EMLite.DataEngine.Log.MilestoneTaskLog[] milestoneTaskLogs = this.loanData.GetLogList().GetAllMilestoneTaskLogs((string) null);
        Hashtable hashtable = new Hashtable();
        for (int index = 0; index < milestoneTaskLogs.Length; ++index)
        {
          if (!hashtable.ContainsKey((object) (milestoneTaskLogs[index].TaskGUID + milestoneTaskLogs[index].Stage)))
            hashtable[(object) (milestoneTaskLogs[index].TaskGUID + milestoneTaskLogs[index].Stage)] = (object) milestoneTaskLogs[index];
        }
        for (int index = 0; index < reqTasks.Length; ++index)
        {
          string empty = string.Empty;
          string name = this.SessionObjects.BpmManager.GetMilestone(reqTasks[index].MilestoneID).Name;
          if (!hashtable.ContainsKey((object) (reqTasks[index].TaskGuid + name)) && tasksSetup.ContainsKey((object) reqTasks[index].TaskGuid))
          {
            MilestoneTaskDefinition milestoneTaskDefinition = (MilestoneTaskDefinition) tasksSetup[(object) reqTasks[index].TaskGuid];
            this.LoanData.GetLogList().AddRecord((LogRecordBase) new EllieMae.EMLite.DataEngine.Log.MilestoneTaskLog(this.sessionObjects.UserInfo, milestoneTaskDefinition.TaskName, milestoneTaskDefinition.TaskDescription)
            {
              TaskGUID = milestoneTaskDefinition.TaskGUID,
              Stage = name,
              IsRequired = true,
              TaskPriority = milestoneTaskDefinition.TaskPriority.ToString(),
              DaysToComplete = milestoneTaskDefinition.DaysToComplete
            });
          }
        }
      }
    }

    public void AttachFieldRules(
      Hashtable fieldRules,
      Hashtable preRequiredFields,
      FieldMilestonePair[] requiredFields)
    {
      this.fieldRuleTable = fieldRules;
      this.preRequiredFields = preRequiredFields;
      if (requiredFields != null)
      {
        this.requiredFieldTable = new Hashtable();
        for (int index = 0; index < requiredFields.Length; ++index)
        {
          if (!this.requiredFieldTable.ContainsKey((object) requiredFields[index].FieldID))
            this.requiredFieldTable.Add((object) requiredFields[index].FieldID, (object) requiredFields[index].FieldID);
        }
      }
      if (this.FieldRulesChanged == null)
        return;
      this.FieldRulesChanged((object) this, EventArgs.Empty);
    }

    public string[] GetPrerequiredFields(string id)
    {
      if (this.preRequiredFields == null)
        return (string[]) null;
      return !this.preRequiredFields.ContainsKey((object) id.ToUpper()) ? (string[]) null : (string[]) this.preRequiredFields[(object) id.ToUpper()];
    }

    public bool FieldIsRequired(string id)
    {
      return this.requiredFieldTable != null && this.requiredFieldTable.ContainsKey((object) id);
    }

    public Hashtable GetDropdownFieldRuleList()
    {
      if (this.fieldRuleTable == null)
        return (Hashtable) null;
      Hashtable hashtable = new Hashtable();
      foreach (DictionaryEntry dictionaryEntry in this.fieldRuleTable)
      {
        object obj = dictionaryEntry.Value;
        if (obj is FRList)
          hashtable.Add((object) dictionaryEntry.Key.ToString(), (object) (FRList) obj);
      }
      return hashtable.Count == 0 ? (Hashtable) null : hashtable;
    }

    public string[] GetFieldRuleDropdownOptions(string id)
    {
      if (this.fieldRuleTable == null)
        return (string[]) null;
      if (!this.fieldRuleTable.ContainsKey((object) id))
        return (string[]) null;
      try
      {
        object obj = this.fieldRuleTable[(object) id];
        if (obj is FRList)
          return ((FRList) obj).List;
        Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), "GetFieldRuleDropdownOptions: field " + id + " is not a dropdown field type.");
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), "GetFieldRuleDropdownOptions: Can't get dropdown list for field " + id + ". Error: " + ex.Message);
      }
      return (string[]) null;
    }

    public string FieldRuleError(string id, string val)
    {
      if (val == string.Empty || this.fieldRuleTable == null || !this.fieldRuleTable.ContainsKey((object) id))
        return string.Empty;
      object obj = this.fieldRuleTable[(object) id];
      if (obj is FRRange)
      {
        FRRange frRange = (FRRange) obj;
        Decimal num1 = EllieMae.EMLite.Common.Utils.ParseDecimal((object) frRange.LowerBound);
        Decimal num2 = EllieMae.EMLite.Common.Utils.ParseDecimal((object) frRange.UpperBound);
        Decimal num3 = EllieMae.EMLite.Common.Utils.ParseDecimal((object) val);
        if (num1 > num3 || num2 < num3)
          return "The field " + id + " is not in range between " + frRange.LowerBound + " and " + frRange.UpperBound;
      }
      else
      {
        string str = obj as string;
      }
      return string.Empty;
    }

    public void AttachEditableFields(Hashtable editableFields)
    {
      this.loanData.AttachEditableFields(editableFields);
    }

    public void AttachFieldAccessRights(Hashtable fieldRights)
    {
      this.fieldRightsTable = fieldRights;
      if (this.fieldRightsTable != null)
      {
        Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
        foreach (DictionaryEntry dictionaryEntry in this.fieldRightsTable)
        {
          string key = dictionaryEntry.Key.ToString();
          switch ((BizRule.FieldAccessRight) dictionaryEntry.Value)
          {
            case BizRule.FieldAccessRight.Hide:
            case BizRule.FieldAccessRight.ViewOnly:
              if (!insensitiveHashtable.ContainsKey((object) key))
              {
                insensitiveHashtable.Add((object) key, (object) "");
                continue;
              }
              continue;
            default:
              continue;
          }
        }
        this.loanData.AttachReadOnlyFields(insensitiveHashtable);
      }
      if (!this.loanData.TemporaryIgnoreRule && this.configInfo.DisplayBusinessRuleOption == EnableDisableSetting.Enabled)
        this.loanData.TemporaryIgnoreRule = true;
      if (this.AccessRightsChanged == null)
        return;
      this.AccessRightsChanged((object) this, EventArgs.Empty);
    }

    public BizRule.FieldAccessRight GetFieldAccessRights(string id)
    {
      BizRule.FieldAccessRight fieldAccessRights = BizRule.FieldAccessRight.Edit;
      if (this.fieldRightsTable != null && this.fieldRightsTable.ContainsKey((object) id))
        fieldAccessRights = (BizRule.FieldAccessRight) this.fieldRightsTable[(object) id];
      return fieldAccessRights;
    }

    public Hashtable GetHiddenFields()
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      if (this.fieldRightsTable == null)
        return insensitiveHashtable;
      foreach (DictionaryEntry dictionaryEntry in this.fieldRightsTable)
      {
        if ((BizRule.FieldAccessRight) dictionaryEntry.Value == BizRule.FieldAccessRight.Hide)
          insensitiveHashtable.Add((object) dictionaryEntry.Key.ToString(), (object) "");
      }
      return insensitiveHashtable;
    }

    public Hashtable GetFieldAccessList() => this.fieldRightsTable;

    private LOLicenseInfo[] isUserLicensedForState(string userId, string stateAbbr)
    {
      LOLicenseInfo[] loLicenses = this.sessionObjects.OrganizationManager.GetLOLicenses(userId);
      foreach (LOLicenseInfo loLicenseInfo in loLicenses)
      {
        if (string.Compare(loLicenseInfo.StateAbbr, stateAbbr, true) == 0)
          return loLicenseInfo.Enabled ? loLicenses : (LOLicenseInfo[]) null;
      }
      return (LOLicenseInfo[]) null;
    }

    private void populateStateLicense(UserInfo userInfo, LOLicenseInfo[] loLicInfo)
    {
      try
      {
        this.loanData.RemoveLicenseNodes();
        OrgInfo avaliableOrganization = this.sessionObjects.OrganizationManager.GetFirstAvaliableOrganization(userInfo.OrgId);
        if (avaliableOrganization != null)
          this.loanData.SetField("ORGID", avaliableOrganization.OrgCode);
        if (loLicInfo == null)
          loLicInfo = this.sessionObjects.OrganizationManager.GetLOLicenses(userInfo.Userid);
        string[] states = EllieMae.EMLite.Common.Utils.GetStates();
        string field = this.loanData.GetField("14");
        DateTime date = DateTime.Today.Date;
        for (int index1 = 0; index1 < states.Length; ++index1)
        {
          bool flag = false;
          if (loLicInfo != null)
          {
            for (int index2 = 0; index2 < loLicInfo.Length; ++index2)
            {
              if (loLicInfo[index2].StateAbbr == states[index1])
              {
                if (states[index1] == "CA")
                  this.loanData.SetField("RE88395.X183", loLicInfo[index2].License);
                this.loanData.SetField("LIC." + states[index1], loLicInfo[index2].License);
                if (loLicInfo[index2].Enabled && date.CompareTo(loLicInfo[index2].ExpirationDate) <= 0)
                {
                  this.loanData.SetCurrentField("LO.ALLOWED." + states[index1], "Y");
                  if (string.Compare(field, loLicInfo[index2].StateAbbr, true) == 0)
                    this.loanData.SetCurrentField("2306", loLicInfo[index2].License);
                }
                else
                  this.loanData.SetCurrentField("LO.ALLOWED." + states[index1], "");
                flag = true;
                break;
              }
            }
          }
          if (!flag)
          {
            this.loanData.SetCurrentField("LIC." + states[index1], "");
            this.loanData.SetCurrentField("LO.ALLOWED." + states[index1], "");
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), "Can't set state license information. Error: " + ex.Message);
      }
    }

    public void SetLoanUnderwriter(UserInfo userInfo)
    {
      if (userInfo != (UserInfo) null)
      {
        this.loanData.SetField("2574", userInfo.FullName);
        this.loanData.SetField("2576", userInfo.Email);
        this.loanData.SetField("UWID", userInfo.Userid);
        this.loanData.SetField("2575", userInfo.Phone);
        this.loanData.SetField("2577", userInfo.Fax);
        this.loanData.SetField("984", userInfo.FullName);
        this.loanData.SetField("1410", userInfo.Phone);
        this.loanData.SetField("1411", userInfo.Email);
        this.loanData.SetField("VEND.X176", userInfo.Fax);
        OrgInfo avaliableOrganization = this.sessionObjects.OrganizationManager.GetFirstAvaliableOrganization(userInfo.OrgId);
        if (avaliableOrganization == null)
        {
          this.loanData.SetField("REGZGFE.X8", this.sessionObjects.CompanyInfo.Name);
          this.loanData.SetField("VEND.X171", this.sessionObjects.CompanyInfo.Address);
          this.loanData.SetField("VEND.X172", this.sessionObjects.CompanyInfo.City);
          this.loanData.SetField("VEND.X173", this.sessionObjects.CompanyInfo.State);
          this.loanData.SetField("VEND.X174", this.sessionObjects.CompanyInfo.Zip);
          if (!((userInfo.Fax ?? "") == string.Empty))
            return;
          this.loanData.SetField("VEND.X176", this.sessionObjects.CompanyInfo.Fax);
          this.loanData.SetField("2577", this.sessionObjects.CompanyInfo.Fax);
        }
        else
        {
          this.loanData.SetField("REGZGFE.X8", avaliableOrganization.CompanyName);
          this.loanData.SetField("VEND.X171", avaliableOrganization.CompanyAddress.Street1);
          this.loanData.SetField("VEND.X172", avaliableOrganization.CompanyAddress.City);
          this.loanData.SetField("VEND.X173", avaliableOrganization.CompanyAddress.State);
          this.loanData.SetField("VEND.X174", avaliableOrganization.CompanyAddress.Zip);
          if (!((userInfo.Fax ?? "") == string.Empty))
            return;
          this.loanData.SetField("VEND.X176", avaliableOrganization.CompanyFax);
          this.loanData.SetField("2577", avaliableOrganization.CompanyFax);
        }
      }
      else
      {
        this.loanData.SetField("2574", "");
        this.loanData.SetField("2575", "");
        this.loanData.SetField("UWID", "");
        this.loanData.SetField("2576", "");
        this.loanData.SetField("2577", "");
        this.loanData.SetField("984", "");
        this.loanData.SetField("1410", "");
        this.loanData.SetField("1411", "");
        this.loanData.SetField("REGZGFE.X8", "");
        this.loanData.SetField("VEND.X171", "");
        this.loanData.SetField("VEND.X172", "");
        this.loanData.SetField("VEND.X173", "");
        this.loanData.SetField("VEND.X174", "");
        this.loanData.SetField("VEND.X176", "");
      }
    }

    public void ClearLoanUnderwriter() => this.SetLoanUnderwriter((UserInfo) null);

    public void SetLoanCloser(UserInfo userInfo)
    {
      if (userInfo != (UserInfo) null)
      {
        this.loanData.SetField("1855", userInfo.FullName);
        this.loanData.SetField("1857", userInfo.Email);
        this.loanData.SetField("CLID", userInfo.Userid);
        this.loanData.SetField("1856", userInfo.Phone);
        this.loanData.SetField("2856", userInfo.CellPhone);
        if ((userInfo.Fax ?? "") != string.Empty)
        {
          this.loanData.SetField("2413", userInfo.Fax);
        }
        else
        {
          OrgInfo displayOrganization = this.configInfo.DisplayOrganization;
          if (displayOrganization != null)
            this.loanData.SetField("2413", displayOrganization.CompanyFax);
          else
            this.loanData.SetField("2413", "");
        }
      }
      else
      {
        this.loanData.SetField("1855", "");
        this.loanData.SetField("1857", "");
        this.loanData.SetField("CLID", "");
        this.loanData.SetField("1856", "");
        this.loanData.SetField("2413", "");
        this.loanData.SetField("2856", "");
      }
    }

    public void ClearLoanCloser() => this.SetLoanCloser((UserInfo) null);

    public void SetLoanProcessor(UserInfo userInfo)
    {
      this.loanData.GetSimpleField("LPID");
      if (userInfo != (UserInfo) null)
      {
        this.loanData.SetField("362", userInfo.FullName);
        this.loanData.SetField("4509", userInfo.JobTitle);
        this.loanData.SetField("1409", userInfo.Email);
        this.loanData.SetField("LPID", userInfo.Userid);
        this.loanData.SetField("1408", userInfo.Phone);
        this.loanData.SetField("2855", userInfo.CellPhone);
        if ((userInfo.Fax ?? "") != string.Empty)
        {
          this.loanData.SetField("2412", userInfo.Fax);
        }
        else
        {
          OrgInfo displayOrganization = this.configInfo.DisplayOrganization;
          if (displayOrganization != null)
            this.loanData.SetField("2412", displayOrganization.CompanyFax);
          else
            this.loanData.SetField("2412", "");
        }
      }
      else
      {
        this.loanData.SetField("362", "");
        this.loanData.SetField("4509", "");
        this.loanData.SetField("1409", "");
        this.loanData.SetField("LPID", "");
        this.loanData.SetField("1408", "");
        this.loanData.SetField("2855", "");
        this.loanData.SetField("2412", "");
      }
    }

    public void ClearLoanProcessor() => this.SetLoanProcessor((UserInfo) null);

    public bool SetLoanNumber(string _loid = "�")
    {
      bool flag = false;
      if ((this.loanData.LoanNumber ?? "") == "")
      {
        ILoanManager loanManager = this.sessionObjects.LoanManager;
        if (loanManager.IsTimeToSetLoanNumber(this.loanData))
        {
          OrgInfo orgInfo = (OrgInfo) null;
          string userId = string.IsNullOrEmpty(_loid) ? this.loanData.GetField("LOID") : _loid;
          if (userId != string.Empty)
          {
            UserInfo user = this.sessionObjects.OrganizationManager.GetUser(userId);
            if (user != (UserInfo) null)
              orgInfo = this.sessionObjects.OrganizationManager.GetFirstAvaliableOrganization(user.OrgId);
          }
          this.loanData.LoanNumber = orgInfo != null ? loanManager.GetNextLoanNumber(orgInfo) : loanManager.GetNextLoanNumber();
          flag = true;
        }
      }
      return flag;
    }

    public bool SetTpoLoanNumber()
    {
      bool flag = false;
      if (string.IsNullOrWhiteSpace(this.loanData.LoanNumber))
      {
        ILoanManager loanManager = this.sessionObjects.LoanManager;
        if (loanManager.IsTimeToSetLoanNumber(this.loanData))
        {
          OrgInfo orgInfo = (OrgInfo) null;
          UserInfo userInfo = (UserInfo) null;
          string field = this.loanData.GetField("LOID");
          if (!string.IsNullOrWhiteSpace(field))
            userInfo = this.sessionObjects.OrganizationManager.GetUser(field);
          else if (this.sessionObjects.UserInfo.UserType.Equals("External", StringComparison.InvariantCultureIgnoreCase))
            userInfo = this.sessionObjects.UserInfo;
          if (userInfo != (UserInfo) null)
          {
            orgInfo = this.sessionObjects.OrganizationManager.GetFirstAvaliableOrganization(userInfo.OrgId, false);
            if (orgInfo != null)
            {
              ExternalUserInfo userInfoByContactId = this.GetExternalUserInfoByContactId(userInfo);
              if ((UserInfo) userInfoByContactId != (UserInfo) null)
              {
                UserInfo user = this.sessionObjects.OrganizationManager.GetUser(userInfoByContactId.SalesRepID);
                if (user != (UserInfo) null)
                {
                  OrgInfo avaliableOrganization = this.sessionObjects.OrganizationManager.GetFirstAvaliableOrganization(user.OrgId, false);
                  if (avaliableOrganization != null)
                  {
                    orgInfo.OrgCode = avaliableOrganization.OrgCode;
                    this.loanData.SetField("ORGID", orgInfo.OrgCode);
                  }
                }
              }
            }
          }
          this.loanData.LoanNumber = orgInfo != null ? loanManager.GetNextLoanNumber(orgInfo) : loanManager.GetNextLoanNumber();
          flag = true;
        }
      }
      return flag;
    }

    private ExternalUserInfo GetExternalUserInfoByContactId(UserInfo userInfo)
    {
      ExternalUserInfo userInfoByContactId = this.sessionObjects.ConfigurationManager.GetExternalUserInfoByContactId(userInfo.Userid);
      if ((UserInfo) userInfoByContactId != (UserInfo) null)
        userInfoByContactId = this.sessionObjects.ConfigurationManager.GetExternalUserInfo(userInfoByContactId.ExternalUserID);
      return userInfoByContactId;
    }

    private static void setEncompassVersion(LoanDataMgr loanDataMgr)
    {
      loanDataMgr.LoanData.SetField("SYS.X611", VersionInformation.CurrentVersion.DisplayVersion);
    }

    public void SendToProcessing(UserInfo userInfo)
    {
      this.loanData.SetCurrentField("1601", "PROCESSING");
      if (!((this.loanData.LoanNumber ?? "") == "") || !(this.loanData.GetField("LPID") != ""))
        return;
      this.SetLoanNumber("");
    }

    public void ApplyDocumentTemplate(LogList logList, DocumentSetTemplate docuTemplate)
    {
      Hashtable docList = docuTemplate.DocList;
      if (docList == null)
        return;
      List<EllieMae.EMLite.Workflow.Milestone> milestonesList = this.configInfo.MilestonesList;
      if (milestonesList == null)
        Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), "Can't find Log Instance file.");
      DocumentTrackingSetup documentTrackingSetup = this.configInfo.DocumentTrackingSetup;
      string pairId = this.loanData.PairId;
      if (this.LoanData.EnableEnhancedConditions)
        this.SetEnhancedConditionTemplates();
      foreach (object key in (IEnumerable) docList.Keys)
      {
        foreach (string name in docuTemplate.GetDocumentsByMilestone((string) key))
        {
          if (name != null && !(name == string.Empty))
          {
            DocumentTemplate byName = documentTrackingSetup.GetByName(name);
            if (byName != null)
            {
              EllieMae.EMLite.DataEngine.Log.DocumentLog rec = new EllieMae.EMLite.DataEngine.Log.DocumentLog(byName, this.sessionObjects.UserID, pairId);
              string str = (string) key;
              if (milestonesList != null)
              {
                foreach (EllieMae.EMLite.Workflow.Milestone milestone in milestonesList)
                {
                  if (milestone.Name == str)
                  {
                    rec.Stage = str;
                    break;
                  }
                }
              }
              else
                rec.Stage = string.Empty;
              if (rec.Stage != "")
              {
                if (logList.GetMilestone(rec.Stage) != null)
                  logList.AddRecord((LogRecordBase) rec);
              }
              else
                logList.AddRecord((LogRecordBase) rec);
            }
          }
        }
      }
    }

    public bool ApplyTaskSetTemplate(TaskSetTemplate taskSet)
    {
      using (Tracing.StartTimer(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Applying Task Set Template '" + taskSet.Description + "'"))
        return this.loanData.SetTaskSetTemplate(taskSet, this.configInfo.TasksSetup, this.configInfo.MilestonesList, this.sessionObjects.UserInfo);
    }

    public void ApplyTaskSet(FileSystemEntry fsEntry, TaskSetTemplate taskSet)
    {
      this.ApplyTaskSetTemplate(taskSet);
    }

    public void ApplyClosingCost(FileSystemEntry fsEntry, ClosingCost closingCost, bool appendData)
    {
      this.applyClosingCostWithoutCalculating(fsEntry, closingCost, appendData);
      this.Calculator.CalculateAll();
    }

    private void applyClosingCostWithoutCalculating(
      FileSystemEntry fsEntry,
      ClosingCost closingCost,
      bool appendData)
    {
      using (Tracing.StartTimer(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Applying Closing Cost Template '" + fsEntry.ToDisplayString() + "'"))
      {
        this.loanData.SelectClosingCostProgram(closingCost, appendData);
        this.loanData.SetCurrentField("1785", closingCost.TemplateName);
        this.loanData.SetCurrentField("2862", fsEntry.ToDisplayString());
      }
    }

    public void ApplyLoanProgram(FileSystemEntry fsEntry, LoanProgram loanProgram, bool appendData)
    {
      this.ApplyLoanProgram(fsEntry, loanProgram, appendData, appendData);
    }

    public void ApplyLoanProgram(
      FileSystemEntry fsEntry,
      LoanProgram loanProgram,
      bool appendLPData,
      bool appendCCData)
    {
      this.ApplyLoanProgram(fsEntry, loanProgram, appendLPData, appendCCData, true);
    }

    public void ApplyLoanProgram(
      FileSystemEntry fsEntry,
      LoanProgram loanProgram,
      bool appendLPData,
      bool appendCCData,
      bool syncPlanData)
    {
      using (Tracing.StartTimer(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Applying Loan Program '" + fsEntry.ToDisplayString() + "'"))
      {
        string field = loanProgram.GetField("1985");
        if (field == "")
          field = loanProgram.GetField("LP125");
        HelocRateTable helocTable = (HelocRateTable) null;
        if (field != string.Empty)
          helocTable = (HelocRateTable) this.sessionObjects.ConfigurationManager.GetHelocTable(field);
        if (loanProgram.IsLinkedToDocEnginePlan)
        {
          if (syncPlanData)
          {
            try
            {
              Plan.Synchronize(this.sessionObjects, loanProgram);
            }
            catch (Exception ex)
            {
              Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Warning, "Failed to sync Plan Code data: " + (object) ex);
            }
          }
          EncompassDocs.SetDocEngine((IHtmlInput) this.loanData, "New_Encompass_Docs_Solution");
        }
        this.loanData.SelectLoanProgram(loanProgram, helocTable, appendLPData);
        if ((loanProgram.ClosingCostPath ?? "") != "")
        {
          FileSystemEntry fileSystemEntry = FileSystemEntry.Parse(loanProgram.ClosingCostPath, this.sessionObjects.UserID);
          ClosingCost templateSettings = (ClosingCost) this.sessionObjects.ConfigurationManager.GetTemplateSettings(TemplateSettingsType.ClosingCost, fileSystemEntry);
          if (templateSettings != null)
            this.applyClosingCostWithoutCalculating(fileSystemEntry, templateSettings, appendCCData);
        }
        this.loanData.SetCurrentField("1960", LoanDataMgr.GetARMDisclosureDescription(this.loanData.GetField("1959")));
        this.loanData.SetCurrentField("1401", loanProgram.TemplateName);
        this.loanData.SetCurrentField("2861", fsEntry.ToDisplayString());
        this.Calculator.CalculateAll();
        this.loanData.Calculator.FormCalculation("677");
        this.loanData.Calculator.FormCalculation("682");
        if (!(this.LoanData.GetField("1825") == "2020"))
          return;
        this.loanData.Calculator.FormCalculation("19");
      }
    }

    public void ClearLoanProgram()
    {
      this.loanData.SetCurrentField("1401", "");
      this.loanData.SetCurrentField("2861", "");
    }

    public void ApplyDocumentSet(FileSystemEntry fsEntry, DocumentSetTemplate documentSet)
    {
      using (Tracing.StartTimer(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Applying Document Set Template '" + fsEntry.ToDisplayString() + "'"))
      {
        this.ApplyDocumentTemplate(this.loanData.GetLogList(), documentSet);
        this.loanData.SetField("2863", fsEntry.ToDisplayString());
        this.Calculator.CalculateAll();
      }
    }

    public void ApplyInputFormSet(FileSystemEntry fsEntry, FormTemplate formSet)
    {
      using (Tracing.StartTimer(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Applying Input Form Set Template '" + fsEntry.ToDisplayString() + "'"))
      {
        this.loanData.SetFormListTemplate(formSet);
        this.loanData.SetCurrentField("2864", fsEntry.ToDisplayString());
      }
    }

    public void ApplyDataTemplate(
      FileSystemEntry fsEntry,
      DataTemplate dataTemplate,
      bool appendData)
    {
      using (Tracing.StartTimer(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Applying Data Template '" + fsEntry.ToDisplayString() + "'"))
      {
        this.loanData.SetDataTemplate(dataTemplate);
        this.loanData.SetCurrentField("2865", fsEntry.ToDisplayString());
        this.Calculator.CalculateAll();
      }
    }

    public void ApplyLoanTemplate(
      FileSystemEntry fsEntry,
      bool appendData,
      bool createHistory,
      bool interactive)
    {
      this.ApplyLoanTemplate(this.sessionObjects.ConfigurationManager.GetLoanTemplateComponents(fsEntry), appendData, createHistory, interactive);
    }

    public void ApplyLoanTemplate(
      Hashtable templateSettings,
      bool appendData,
      bool createHistory,
      bool interactive)
    {
      this.ApplyLoanTemplate(templateSettings, (IHtmlInput) null, false, appendData, createHistory, interactive);
    }

    public void ApplyLoanTemplate(FileSystemEntry fsEntry, bool appendData, bool createHistory)
    {
      this.ApplyLoanTemplate(this.sessionObjects.ConfigurationManager.GetLoanTemplateComponents(fsEntry), appendData, createHistory);
    }

    public void ApplyLoanTemplate(Hashtable templateSettings, bool appendData, bool createHistory)
    {
      this.ApplyLoanTemplate(templateSettings, (IHtmlInput) null, false, appendData, createHistory, true);
    }

    public void ApplyLoanTemplate(
      Hashtable templateSettings,
      IHtmlInput templateObject,
      bool isCCInLP,
      bool appendData,
      bool createHistory,
      bool interactive)
    {
      using (Tracing.StartTimer(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Applying Loan Template '" + templateSettings[(object) "LOANTEMPLATEFILE"] + "'"))
      {
        this.loanData.VALoanValidation = true;
        LoanTemplateSet templateSet = new LoanTemplateSet();
        templateSet.FormTemplate = (FormTemplate) (BinaryObject) templateSettings[(object) "FORMLIST"];
        if (templateObject == null || !(templateObject is DataTemplate))
          templateSet.DataTemplate = (DataTemplate) (BinaryObject) templateSettings[(object) "MISCDATA"];
        else if (templateObject is DataTemplate)
          templateSet.DataTemplate = (DataTemplate) templateObject;
        templateSet.LoanProgram = (LoanProgram) (BinaryObject) templateSettings[(object) "PROGRAM"];
        if (templateObject == null || !isCCInLP || !(templateObject is ClosingCost))
          templateSet.LoanProgramClosingCost = (ClosingCost) (BinaryObject) templateSettings[(object) "LPCOST"];
        else if (templateObject is ClosingCost & isCCInLP)
          templateSet.LoanProgramClosingCost = (ClosingCost) templateObject;
        if (templateObject == null | isCCInLP || !(templateObject is ClosingCost))
          templateSet.ClosingCost = (ClosingCost) (BinaryObject) templateSettings[(object) "COST"];
        else if (templateObject is ClosingCost && !isCCInLP)
          templateSet.ClosingCost = (ClosingCost) templateObject;
        TaskSetTemplate templateSetting1 = (TaskSetTemplate) (BinaryObject) templateSettings[(object) "TASKSET"];
        DocumentSetTemplate templateSetting2 = (DocumentSetTemplate) (BinaryObject) templateSettings[(object) "DOCSET"];
        templateSet.ProviderTemplate = (SettlementServiceTemplate) (BinaryObject) templateSettings[(object) "PROVIDERLIST"];
        templateSet.AffiliateTemplate = (AffiliateTemplate) (BinaryObject) templateSettings[(object) "AFFILIATELIST"];
        if (templateSettings[(object) "MILETEMP"] != null && Convert.ToString(templateSettings[(object) "MILETEMP"]) != string.Empty)
        {
          templateSet.MilestoneTemplate = this.sessionObjects.BpmManager.GetMilestoneTemplateByGuid(Convert.ToString(templateSettings[(object) "MILETEMP"]));
          if (templateSet.MilestoneTemplate != null && templateSet.MilestoneTemplate.Active)
            LoanDataMgr.replaceTemplate(this.sessionObjects, templateSet.MilestoneTemplate, templateSettings[(object) "LOANTEMPLATEFILE"].ToString(), true, createHistory, this.loanData, false);
        }
        if (templateSetting2 != null)
        {
          this.ApplyDocumentTemplate(this.loanData.GetLogList(), templateSetting2);
          this.loanData.SetField("2863", string.Concat(templateSettings[(object) "DOCSETFILE"]));
        }
        if (templateSet.LoanProgram != null)
        {
          string field = templateSet.LoanProgram.GetField("1985");
          if (field == "")
            templateSet.LoanProgram.GetField("LP125");
          if (field != string.Empty)
            templateSet.HELOCTable = (HelocRateTable) this.sessionObjects.ConfigurationManager.GetHelocTable(field);
        }
        string field1 = this.loanData.GetField("HMDA.X100");
        this.loanData.SetLoanTemplate(templateSet, appendData);
        this.setHMDAProfile(this.loanData, templateSet.DataTemplate, field1, interactive);
        this.loanData.SetTaskSetTemplate(templateSetting1, this.configInfo.TasksSetup, this.configInfo.MilestonesList, this.sessionObjects.UserInfo);
        if (templateSettings.ContainsKey((object) "LOANTEMPLATEFILE"))
          this.loanData.SetCurrentField("2860", templateSettings[(object) "LOANTEMPLATEFILE"].ToString());
        else
          this.loanData.SetCurrentField("2860", "");
        if (templateSet.FormTemplate != null)
          this.loanData.SetCurrentField("2864", templateSettings[(object) "FORMLISTFILE"].ToString());
        else if (!appendData)
          this.loanData.SetCurrentField("2864", "");
        if (templateSet.DataTemplate != null)
          this.loanData.SetCurrentField("2865", templateSettings[(object) "MISCDATAFILE"].ToString());
        else if (!appendData)
          this.loanData.SetCurrentField("2865", "");
        if (templateSet.LoanProgram != null)
        {
          this.loanData.SetCurrentField("1401", templateSet.LoanProgram.TemplateName);
          this.loanData.SetCurrentField("2861", templateSettings[(object) "PROGRAMFILE"].ToString());
        }
        else if (!appendData)
        {
          this.loanData.SetCurrentField("1401", "");
          this.loanData.SetCurrentField("2861", "");
        }
        if (templateSet.ClosingCost != null)
        {
          this.loanData.SetCurrentField("1785", templateSet.ClosingCost.TemplateName);
          this.loanData.SetCurrentField("2862", templateSettings[(object) "COSTFILE"].ToString());
        }
        else if (templateSet.LoanProgramClosingCost != null)
        {
          this.loanData.SetCurrentField("1785", templateSet.LoanProgramClosingCost.TemplateName);
          this.loanData.SetCurrentField("2862", templateSettings[(object) "LPCOSTFILE"].ToString());
        }
        else if (!appendData)
        {
          this.loanData.SetCurrentField("1785", "");
          this.loanData.SetCurrentField("2862", "");
        }
        if (templateSet.LoanProgram != null)
        {
          this.loanData.SetCurrentField("1960", LoanDataMgr.GetARMDisclosureDescription(this.loanData.GetField("1959")));
          this.loanData.Calculator.FormCalculation("677");
        }
        if (!this.loanData.VALoanValidation)
        {
          if (interactive)
          {
            int num = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) System.Windows.Forms.Form.ActiveForm, this.loanData.VALoanWarningMessage, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          this.loanData.VALoanValidation = true;
        }
        this.setRESPAVersionFromTemplates(templateSettings);
      }
    }

    public void setHMDAProfile(
      LoanData ld,
      DataTemplate lts,
      string hmdaProfileID,
      bool interactive)
    {
      if (lts == null)
        return;
      string simpleField = lts.GetSimpleField("HMDA.X100");
      bool flag = false;
      if (string.IsNullOrEmpty(simpleField) || !(hmdaProfileID != simpleField))
        return;
      if (string.IsNullOrEmpty(ld.GetField("HMDA.X28")))
        flag = true;
      else if (interactive && DialogResult.OK == EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) System.Windows.Forms.Form.ActiveForm, "Do you wish to update Universal Loan ID already calculated for this loan?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
        flag = true;
      if (flag)
      {
        HMDAProfile hmdaProfileById = this.sessionObjects.ConfigurationManager.GetHMDAProfileById(EllieMae.EMLite.Common.Utils.ParseInt((object) simpleField));
        if (hmdaProfileById == null)
          return;
        ld.Settings.HMDAInfo = new HMDAInformation(hmdaProfileById.HMDAProfileSetting);
        ld.Settings.HMDAInfo.HMDAProfileID = hmdaProfileById.HMDAProfileID.ToString();
        ld.Calculator.FormCalculation("UPDATEHMDA2018", "", (string) null);
        ld.SetField("HMDA.X100", simpleField);
      }
      else
        ld.SetField("HMDA.X100", hmdaProfileID);
    }

    private void updateX3wmBase()
    {
      if (!this.SessionObjects.EditLoanConcurrently)
        return;
      this.updateX3wmBase(this.loanData.ToXml(LoanContentAccess.FullAccess, true));
    }

    private void updateX3wmBase(string loanDataXml)
    {
      if (!this.SessionObjects.EditLoanConcurrently)
        return;
      this.x3wmBaseLoanDataXml = loanDataXml;
    }

    public bool IsLoanFileOnServerNewer(bool isExternalOrganization)
    {
      string[] guids;
      if (this.LinkedLoan != null && this.LinkedLoan.LoanData != null)
        guids = new string[2]
        {
          null,
          this.LinkedLoan.LoanData.GUID
        };
      else
        guids = new string[1];
      guids[0] = this.LoanData.GUID;
      PipelineInfo[] pipeline = this.SessionObjects.LoanManager.GetPipeline(guids, isExternalOrganization);
      PipelineInfo pipelineInfo1 = pipeline[0];
      PipelineInfo pipelineInfo2 = (PipelineInfo) null;
      if (pipeline.Length == 2)
        pipelineInfo2 = pipeline[1];
      return pipelineInfo1 != null && pipelineInfo1.LastModified > this.LoanData.BaseLastModified || pipelineInfo2 != null && pipelineInfo2.LastModified > this.LinkedLoan.LoanData.BaseLastModified || pipelineInfo1 != null && pipelineInfo1.LoanVersionNumber > this.LoanData.LoanVersionNumber || pipelineInfo2 != null && pipelineInfo2.LoanVersionNumber > this.LinkedLoan.LoanData.LoanVersionNumber;
    }

    private void displayErrorMessage(bool piggybackLoanException, string errMsg, LockInfo[] locks)
    {
      if (errMsg == null || errMsg.Trim() == "")
        errMsg = "You cannot perform this task while other users are editing the loan.";
      StringBuilder stringBuilder = new StringBuilder();
      if (this.SessionObjects.ExclusiveLockCurrLoginsOnly)
      {
        string str = "this loan";
        if (piggybackLoanException)
          str = "the piggyback/linked loan of this loan";
        stringBuilder.Append(" The following users are currently working on " + str + ":\r\n\r\n");
      }
      else
      {
        string str = "The loan";
        if (piggybackLoanException)
          str = "The piggyback/linked loan of this loan";
        stringBuilder.Append(" " + str + " is currently locked by the following users:\r\n\r\n");
      }
      bool flag = true;
      foreach (LockInfo lockInfo in locks)
      {
        if (flag)
          flag = false;
        else
          stringBuilder.Append(", ");
        stringBuilder.Append(lockInfo.LockedByFirstName + " " + lockInfo.LockedByLastName);
      }
      int num = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, errMsg + stringBuilder.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    private bool lockLoan(
      bool interactive,
      string errMsg,
      LockInfo.ExclusiveLock exclusiveType,
      bool includeLinkedLoan)
    {
      if (!this.SessionObjects.AllowConcurrentEditing)
        return true;
      bool piggybackLoanException = false;
      try
      {
        this.Lock(LoanInfo.LockReason.OpenForWork, exclusiveType, false, false);
        piggybackLoanException = true;
        if (includeLinkedLoan)
        {
          if (this.LinkedLoan != null)
            this.LinkedLoan.Lock(LoanInfo.LockReason.OpenForWork, exclusiveType, false, false);
        }
      }
      catch (ExclusiveLockException ex)
      {
        if (piggybackLoanException)
          this.Lock(LoanInfo.LockReason.OpenForWork, exclusiveType == LockInfo.ExclusiveLock.Exclusive ? LockInfo.ExclusiveLock.ReleaseExclusive : LockInfo.ExclusiveLock.ReleaseExclusiveA);
        if (interactive)
          this.displayErrorMessage(piggybackLoanException, errMsg, ex.Locks);
        return false;
      }
      return true;
    }

    public bool LockLoanWithExclusiveA() => this.LockLoanWithExclusiveA(true, (string) null);

    public bool LockLoanWithExclusiveA(bool interactive)
    {
      return this.LockLoanWithExclusiveA(interactive, (string) null);
    }

    public bool LockLoanWithExclusiveA(bool interactive, string errMsg)
    {
      return this.LockLoanWithExclusiveA(interactive, errMsg, true);
    }

    public bool LockLoanWithExclusiveA(bool interactive, string errMsg, bool includeLinkedLoan)
    {
      if (this.IsNew())
        return true;
      if (!this.Writable)
      {
        string str = "The current loan is opened in read-only mode. You do not have the right to perform this task.";
        if (!interactive)
          throw new LockException(str);
        int num = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, str, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (!this.lockLoan(interactive, errMsg, LockInfo.ExclusiveLock.ExclusiveA, includeLinkedLoan))
      {
        if (interactive)
          return false;
        throw new LockException("Cannot get ExclusiveA lock");
      }
      if (!this.IsLoanFileOnServerNewer(false))
        return true;
      this.ReleaseExclusiveALock();
      if (!interactive)
        throw new ObjectModifiedException("The loan file on the server has been modified.");
      int num1 = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, "This loan was updated by other users. You must receive the latest updates before you perform this task.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    public bool LockLoanWithExclusive() => this.LockLoanWithExclusive(true, (string) null);

    public bool LockLoanWithExclusive(bool interactive)
    {
      return this.LockLoanWithExclusive(interactive, (string) null);
    }

    public bool LockLoanWithExclusive(bool interactive, string errMsg)
    {
      if (this.IsNew())
        return true;
      if (!this.Writable)
        return false;
      if (!this.lockLoan(interactive, errMsg, LockInfo.ExclusiveLock.Exclusive, true))
      {
        if (interactive)
          return false;
        throw new LockException("Cannot get Exclusive lock");
      }
      if (!this.IsLoanFileOnServerNewer(false))
        return true;
      this.ReleaseExclusiveLock();
      if (!interactive)
        throw new ObjectModifiedException("The loan file on the server has been modified.");
      int num = (int) EllieMae.EMLite.Common.Utils.Dialog((IWin32Window) null, "This loan was updated by other users. You must receive the latest updates before you can block Multi-User Editing.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private void releaseLock(LockInfo.ExclusiveLock releaseType, bool includeLinkedLoan)
    {
      if (!this.SessionObjects.AllowConcurrentEditing || this.IsNew() || !this.Writable)
        return;
      this.Lock(LoanInfo.LockReason.OpenForWork, releaseType, false, includeLinkedLoan);
    }

    public void ReleaseExclusiveALock()
    {
      this.releaseLock(LockInfo.ExclusiveLock.ReleaseExclusiveA, true);
    }

    public void ReleaseExclusiveALock(bool includeLinkedLoan)
    {
      this.releaseLock(LockInfo.ExclusiveLock.ReleaseExclusiveA, includeLinkedLoan);
    }

    public void ReleaseExclusiveLock()
    {
      this.releaseLock(LockInfo.ExclusiveLock.ReleaseExclusive, true);
    }

    public void RecoverAutoSavedLoan(Stream loanDataXml, string attXml, string hisXml)
    {
      this.recoverAutoSavedLoan(loanDataXml, false, (string) null);
      if (attXml != null)
        this.FileAttachments.LoadAutoSaveXml(attXml);
      if (hisXml == null)
        return;
      this.LoanHistory.RestorePendingHistory(hisXml);
    }

    public void refreshLoanFromServer(bool closeAllPopups = false)
    {
      this.launcheFolderNeeded = false;
      if (closeAllPopups && this.BeforeLoanRefreshedFromServer != null)
        this.BeforeLoanRefreshedFromServer((object) this, new EventArgs());
      LoanData loanData = this.SessionObjects.LoanManager.OpenLoan(this.loanData.GUID).GetLoanData(false);
      using (Stream stream = loanData.ToStream())
        this.loanData.ReplaceXml(stream);
      this.loanData.BaseLastModified = loanData.BaseLastModified;
      this.attachCalculator();
      this.loanData.Dirty = false;
      this.updateX3wmBase();
      this.loanData.ReplaceCachedXML();
      this.loanData.ClearDirtyTable();
      if (this.fileAttachments != null)
      {
        this.fileAttachments.Resync();
        this.loanData.Dirty = false;
      }
      this.loanHistory.ClearCachedHistory();
      if (this.OnLoanDataXmlReplaced != null)
        this.OnLoanDataXmlReplaced((object) this, new EventArgs());
      if (this.OnLoanRefreshedFromServer != null)
        this.OnLoanRefreshedFromServer((object) this, new EventArgs());
      this.launcheFolderNeeded = false;
    }

    private void recoverAutoSavedLoan(
      Stream loanDataXml,
      bool updateX3wmBase,
      string x3wmBaseLoanDataXml)
    {
      this.loanData.ReplaceXml(loanDataXml);
      this.attachCalculator();
      this.loanData.Dirty = true;
      if (updateX3wmBase)
      {
        if (x3wmBaseLoanDataXml != null)
          this.updateX3wmBase(x3wmBaseLoanDataXml);
        else
          this.updateX3wmBase();
      }
      this.loanData.ReplaceCachedXML();
      this.loanData.ClearDirtyTable();
      this.loanHistory.ClearCachedHistory();
      if (this.OnLoanDataXmlReplaced != null)
        this.OnLoanDataXmlReplaced((object) this, new EventArgs());
      RemoteLogger.Write(TraceLevel.Info, string.Format("Autosaved data successfully replaced for loan guid {0}, clientID {1}, UserID {2} ", (object) this.LoanData.GUID, (object) this.SessionObjects.CompanyInfo.ClientID, (object) this.SessionObjects.UserID));
    }

    public void ReplaceLoanDataXml(Stream loanDataXml, bool callCalculation)
    {
      this.ReplaceLoanDataXml(loanDataXml, DateTime.MaxValue, (string) null, callCalculation);
    }

    public void ReplaceLoanDataXml(
      Stream loanDataXml,
      DateTime baseLastModified,
      bool callCalculation)
    {
      this.ReplaceLoanDataXml(loanDataXml, baseLastModified, (string) null, callCalculation);
    }

    public void ReplaceLoanDataXml(
      Stream loanDataXml,
      DateTime baseLastModified,
      string x3wmBaseLoanDataXml,
      bool callCalculation)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "ReplaceLoanDataXml method invoked...");
      this.recoverAutoSavedLoan(loanDataXml, baseLastModified != DateTime.MaxValue && baseLastModified != DateTime.MinValue, x3wmBaseLoanDataXml);
      if (callCalculation && this.loanData.Calculator != null)
      {
        this.loanData.Calculator.SkipLockRequestSync = true;
        this.loanData.Calculator.UpdateAccountName("36");
        this.loanData.Calculator.UpdateAccountName("11");
        this.loanData.Calculator.CalculateAll();
        this.loanData.Calculator.SkipLockRequestSync = false;
      }
      if (!(baseLastModified != DateTime.MinValue) || !(baseLastModified != DateTime.MaxValue))
        return;
      this.loanData.BaseLastModified = baseLastModified;
    }

    public static string GetARMDisclosureDescription(string armIndex)
    {
      if (armIndex.Trim() == "")
        return "";
      Hashtable hashtable = LoanDataMgr.LoadARMDisclosureList();
      return hashtable == null || !hashtable.ContainsKey((object) armIndex) ? "" : hashtable[(object) armIndex].ToString();
    }

    public static Hashtable LoadARMDisclosureList()
    {
      Hashtable hashtable = new Hashtable();
      XPathDocument xpathDocument;
      try
      {
        xpathDocument = new XPathDocument(!AssemblyResolver.IsSmartClient ? Path.Combine(SystemSettings.DocDirAbsPath, "ARMDisclosureList.xml") : AssemblyResolver.GetResourceFileFullPath(SystemSettings.DocDirRelPath + "ARMDisclosureList.xml", SystemSettings.LocalAppDir));
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), "Can't load ARMDisclosureList.xml file. Error: " + ex.Message);
        return (Hashtable) null;
      }
      XPathNodeIterator xpathNodeIterator = xpathDocument.CreateNavigator().Select("/LIST/ITEM");
      while (xpathNodeIterator.MoveNext())
      {
        string attribute1 = xpathNodeIterator.Current.GetAttribute("id", string.Empty);
        string attribute2 = xpathNodeIterator.Current.GetAttribute("desc", string.Empty);
        hashtable.Add((object) attribute1, (object) attribute2);
      }
      return hashtable;
    }

    public void RemoveFromTrade(TradeInfoObj trade, bool rejected)
    {
      this.RemoveFromTrade(trade, rejected, new List<string>());
    }

    public void RemoveFromTrade(TradeInfoObj trade, bool rejected, List<string> skipFieldList)
    {
      new LoanTradeDataManager(this.sessionObjects, this).RemoveFromTrade(trade, rejected, skipFieldList);
    }

    public void AssignToTrade(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal securityPrice)
    {
      new LoanTradeDataManager(this.sessionObjects, this).AssignToTrade(trade, skipFieldList, securityPrice);
    }

    public void AssignToTrade(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal securityPrice,
      Dictionary<string, string> updateFieldList,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
      new LoanTradeDataManager(this.sessionObjects, this).AssignToTrade(trade, skipFieldList, securityPrice, updateFieldList, syncOption);
    }

    public void ModifyTradeStatus(
      TradeInfoObj trade,
      object status,
      List<string> skipFieldList,
      Decimal securityPrice)
    {
      new LoanTradeDataManager(this.sessionObjects, this).ModifyTradeStatus(trade, status, skipFieldList, securityPrice);
    }

    public void ModifyTradeStatus(
      TradeInfoObj trade,
      object status,
      List<string> skipFieldList,
      Decimal securityPrice,
      Dictionary<string, string> updateFieldList)
    {
      new LoanTradeDataManager(this.sessionObjects, this).ModifyTradeStatus(trade, status, skipFieldList, securityPrice, updateFieldList);
    }

    public void RefreshTradeData(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal securityPrice)
    {
      new LoanTradeDataManager(this.sessionObjects, this).RefreshTradeData(trade, skipFieldList, securityPrice);
    }

    public void RefreshTradeData(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal securityPrice,
      Dictionary<string, string> updateFieldList,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
      new LoanTradeDataManager(this.sessionObjects, this).RefreshTradeData(trade, skipFieldList, securityPrice, updateFieldList, syncOption);
    }

    public void ExtendLockWithTrade(
      TradeInfoObj trade,
      Dictionary<string, string> updateFieldList,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
      new LoanTradeDataManager(this.sessionObjects, this).ExtendLockWithTrade(trade, updateFieldList, syncOption);
    }

    public void ApplyInvestorToLoan(
      EllieMae.EMLite.ClientServer.Investor investor,
      ContactInformation assignee,
      bool updateInvestor)
    {
      this.ApplyInvestorToLoan(investor, assignee, updateInvestor, new List<string>());
    }

    public void ApplyInvestorToLoan(
      EllieMae.EMLite.ClientServer.Investor investor,
      ContactInformation assignee,
      bool updateInvestor,
      List<string> skipFieldList)
    {
      new LoanTradeDataManager(this.sessionObjects, this).ApplyInvestorToLoan(investor, assignee, updateInvestor, skipFieldList);
    }

    public EllieMae.EMLite.DataEngine.Log.LockRequestLog CreateRateLockRequest()
    {
      return this.CreateRateLockRequest(false);
    }

    public EllieMae.EMLite.DataEngine.Log.LockRequestLog CreateRateLockRequest(
      bool historyFromCurrentLock)
    {
      return this.CreateRateLockRequest(historyFromCurrentLock, false);
    }

    public EllieMae.EMLite.DataEngine.Log.LockRequestLog CreateRateLockRequest(
      bool historyFromCurrentLock,
      bool suppressAutoLock)
    {
      return this.CreateRateLockRequest(this.sessionObjects.UserInfo, historyFromCurrentLock, suppressAutoLock);
    }

    public EllieMae.EMLite.DataEngine.Log.LockRequestLog CreateRateLockRequest(
      bool historyFromCurrentLock,
      bool suppressAutoLock,
      bool suppressLockDeskHours,
      RateLockAction rateLockAction = RateLockAction.UnKnown)
    {
      return this.CreateRateLockRequest(this.sessionObjects.UserInfo, historyFromCurrentLock, suppressAutoLock, suppressLockDeskHours, rateLockAction);
    }

    public EllieMae.EMLite.DataEngine.Log.LockRequestLog CreateRateLockRequest(
      UserInfo requestingUser)
    {
      return this.CreateRateLockRequest(requestingUser, false);
    }

    public EllieMae.EMLite.DataEngine.Log.LockRequestLog CreateRateLockRequest(
      UserInfo requestingUser,
      bool historyFromCurrentLock)
    {
      return this.CreateRateLockRequest(requestingUser, historyFromCurrentLock, false);
    }

    public EllieMae.EMLite.DataEngine.Log.LockRequestLog CreateRateLockRequest(
      UserInfo requestingUser,
      bool historyFromCurrentLock,
      bool suppressAutoLock)
    {
      return this.CreateRateLockRequest(requestingUser, historyFromCurrentLock, suppressAutoLock, false);
    }

    public string GetChannel()
    {
      string field = this.loanData.GetField("2626");
      if (string.IsNullOrEmpty(field))
      {
        Hashtable settingsFromCache = this.sessionObjects.GetCompanySettingsFromCache("NMLS");
        if (settingsFromCache != null && settingsFromCache.Contains((object) "ChannelMap/"))
          field = settingsFromCache[(object) "ChannelMap/"].ToString();
      }
      return field;
    }

    public EllieMae.EMLite.DataEngine.Log.LockRequestLog CreateRateLockRequest(
      UserInfo requestingUser,
      bool historyFromCurrentLock,
      bool suppressAutoLock,
      bool suppressLockDeskHours,
      RateLockAction rateLockAction = RateLockAction.UnKnown,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
      bool isRelock = LockUtils.IsRelock(this.loanData.GetField("3841"));
      if (this.CheckProviderAndFlag(isRelock: false) && this.GetChannel().Contains("Retail"))
        suppressLockDeskHours = true;
      using (Tracing.StartTimer(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.CreateRateLockRequest"))
      {
        OnrpCalcInfo onrpCalcInfo = (OnrpCalcInfo) null;
        if (!suppressLockDeskHours && !this.AllowNewLockOutsideLDHours())
        {
          LockDeskHoursManager.ValidateLockRequestTimeThickClient((IClientSession) this.sessionObjects.Session, this.sessionObjects, this, new bool?(), out onrpCalcInfo, isRelock);
          if (onrpCalcInfo != null)
            LockDeskHoursManager.PerformOnrpRegistrationThickClient(this.sessionObjects, this, onrpCalcInfo);
        }
        string field = this.loanData.GetField("2088");
        this.ValidateBestEffortDailyLimit(ref field, this.loanData.GetField("2089"), this.loanData.GetField("3965"), this.loanData.FltVal("2965"), out bool _);
        if (isRelock)
          this.ValidateLockPeriodRestriction();
        EllieMae.EMLite.DataEngine.Log.LockRequestLog rateLockRequest = new EllieMae.EMLite.DataEngine.Log.LockRequestLog(this.loanData.GetLogList());
        rateLockRequest.Date = this.SessionObjects.Session.ServerTime;
        rateLockRequest.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.Requested;
        rateLockRequest.SetRequestingUser(requestingUser.Userid, requestingUser.FullName);
        Hashtable snapshotForLockRequest = this.CreateSnapshotForLockRequest(onrpCalcInfo);
        if (historyFromCurrentLock && this.loanData.GetLogList().GetCurrentLockRequest() != null)
        {
          Hashtable lockRequestSnapshot = this.loanData.GetLogList().GetCurrentLockRequest().GetLockRequestSnapshot();
          if (lockRequestSnapshot.ContainsKey((object) "OPTIMAL.HISTORY"))
            snapshotForLockRequest[(object) "OPTIMAL.HISTORY"] = (object) string.Concat(lockRequestSnapshot[(object) "OPTIMAL.HISTORY"]);
          rateLockRequest.ParentLockGuid = this.loanData.GetLogList().GetCurrentLockRequest().Guid;
        }
        if (isRelock)
        {
          this.InactiveRelockProcLockReqLogCntrCalc(rateLockRequest, snapshotForLockRequest, isRelock, rateLockAction);
          this.ClearItemsForRelocks(snapshotForLockRequest, isRelock);
        }
        rateLockRequest.AddLockRequestSnapshot(snapshotForLockRequest);
        foreach (EllieMae.EMLite.DataEngine.Log.LockRequestLog allLockRequest in this.loanData.GetLogList().GetAllLockRequests())
        {
          if (allLockRequest.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.Requested)
            allLockRequest.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.OldRequest;
        }
        this.LoanData.GetLogList().AddRecord((LogRecordBase) rateLockRequest);
        this.loanData.TriggerCalculation("761", this.loanData.GetField("761"));
        if (this.AllowAutoLock(suppressAutoLock, false, false, false, false))
          LockUtils.PerformAutoLock(this.sessionObjects, rateLockRequest, this, syncOption: syncOption);
        return rateLockRequest;
      }
    }

    public void ValidateLockPeriodRestriction(int currentLockDays = 0)
    {
      IDictionary policySettings = this.sessionObjects.StartupInfo.PolicySettings;
      string a = policySettings.Contains((object) "Policies.RestrictLockPeriod") ? Convert.ToString(policySettings[(object) "Policies.RestrictLockPeriod"]) : string.Empty;
      if (string.IsNullOrEmpty(a) || !string.Equals(a, "true", StringComparison.CurrentCultureIgnoreCase))
        return;
      EllieMae.EMLite.DataEngine.Log.LockConfirmLog lockConfirmation = this.loanData.GetLogList().GetCurrentLockConfirmation();
      if (lockConfirmation == null)
        return;
      int lockDays = this.getLockDays(this.loanData.GetLogList().GetLockRequest(lockConfirmation.RequestGUID));
      if (currentLockDays <= 0)
        currentLockDays = int.Parse(this.loanData.GetField("2090"));
      if (currentLockDays <= lockDays)
        return;
      string message = "The initial lock period has been exceeded. The Re-Lock Request cannot be processed.";
      if (this.IsFromPlatform)
        throw new ApplicationException(message);
      throw new LockPeriodRestrictionException(message);
    }

    private int getLockDays(EllieMae.EMLite.DataEngine.Log.LockRequestLog lrl)
    {
      lrl.GetLockRequestSnapshot();
      int lockDays = 0;
      if (lrl.BuySideNumDayLocked > 0)
      {
        lockDays = lrl.BuySideNumDayLocked;
      }
      else
      {
        Hashtable lockRequestSnapshot = lrl.GetLockRequestSnapshot();
        if (lockRequestSnapshot != null && lockRequestSnapshot.ContainsKey((object) "2150"))
          lockDays = EllieMae.EMLite.Common.Utils.ParseInt(lockRequestSnapshot[(object) "2150"]);
      }
      return lockDays;
    }

    private Hashtable CreateSnapshotForLockRequest(OnrpCalcInfo onrpInfo)
    {
      Hashtable snapshot1 = this.loanData.PrepareLockRequestData();
      if (onrpInfo != null && onrpInfo.OnrpLockTime != null && onrpInfo.OnrpLockDate != null)
      {
        snapshot1[(object) "4060"] = (object) onrpInfo.OnrpLockTime;
        snapshot1[(object) "4069"] = (object) onrpInfo.OnrpLockDate;
        snapshot1[(object) "4061"] = onrpInfo.IsONRPEligible ? (object) "Y" : (object) "N";
        this.setSnapshotField(snapshot1, (object) "4058", snapshot1[(object) "4060"]);
        this.setSnapshotField(snapshot1, (object) "4059", snapshot1[(object) "4061"]);
        this.setSnapshotField(snapshot1, (object) "4070", snapshot1[(object) "4069"]);
        if (onrpInfo.IsONRPEligible && snapshot1.Contains((object) "4069") && snapshot1.Contains((object) "4060"))
        {
          Hashtable snapshot2 = snapshot1;
          DateTime dateTime = LockDeskHoursManager.GetLockDateForOnrp((IClientSession) this.sessionObjects.Session, this.sessionObjects, this, EllieMae.EMLite.Common.Utils.ParseDate((object) (snapshot1[(object) "4069"].ToString() + " " + snapshot1[(object) "4060"].ToString())));
          string shortDateString = dateTime.ToShortDateString();
          this.setSnapshotField(snapshot2, (object) "2089", (object) shortDateString);
          if (snapshot1.Contains((object) "2090"))
          {
            LockRequestCalculator requestCalculator = new LockRequestCalculator(this.SessionObjects, this.loanData);
            Hashtable snapshot3 = snapshot1;
            dateTime = requestCalculator.CalculateLockExpirationDate(EllieMae.EMLite.Common.Utils.ParseDate(snapshot1[(object) "2089"]), EllieMae.EMLite.Common.Utils.ParseInt(snapshot1[(object) "2090"], 0));
            string str = dateTime.ToString("MM/dd/yyyy");
            this.setSnapshotField(snapshot3, (object) "2091", (object) str);
          }
        }
      }
      else
      {
        snapshot1[(object) "4060"] = snapshot1[(object) "4069"] = (object) "";
        snapshot1[(object) "4061"] = (object) "N";
        this.setSnapshotField(snapshot1, (object) "4058", (object) "");
        this.setSnapshotField(snapshot1, (object) "4059", (object) "N");
        this.setSnapshotField(snapshot1, (object) "4070", (object) "");
      }
      this.setSnapshotField(snapshot1, (object) "4209", (object) LockUtils.GetRequestLockStatus(this.loanData));
      return snapshot1;
    }

    private void InactiveRelockProcLockReqLogCntrCalc(
      EllieMae.EMLite.DataEngine.Log.LockRequestLog lockLog,
      Hashtable requestData,
      bool isRelock,
      RateLockAction rateLockAction = RateLockAction.UnKnown)
    {
      string field = this.loanData.GetField("LOCKRATE.RATESTATUS");
      if (!(field == "Cancelled") && !(field == "Expired"))
        return;
      switch (field)
      {
        case "Cancelled":
          if (this.CheckProviderAndFlag(false))
            this.GetParentDataForCancelledLock(requestData);
          EllieMae.EMLite.DataEngine.Log.LockCancellationLog lockCancellation = this.loanData.GetLogList().GetMostRecentLockCancellation();
          lockLog.ParentLockGuid = lockCancellation == null ? "" : lockCancellation.Guid;
          break;
        case "Expired":
          EllieMae.EMLite.DataEngine.Log.LockConfirmLog lockConfirmation = this.loanData.GetLogList().GetCurrentLockConfirmation();
          lockLog.ParentLockGuid = lockConfirmation == null ? "" : lockConfirmation.Guid;
          break;
      }
      int numberForInactiveLock = LockUtils.GetReLockSequenceNumberForInactiveLock(this.loanData);
      lockLog.ReLockSequenceNumberForInactiveLock = numberForInactiveLock + 1;
      if (rateLockAction != RateLockAction.WcpcCurrent && rateLockAction != RateLockAction.WcpcHistorical)
      {
        if (LockUtils.isApplyReLockFee(this.sessionObjects, this.loanData, isRelock))
        {
          int num1 = 4256;
          int num2 = 1;
          if (this.CheckProviderAndFlag(false))
          {
            num2 = lockLog.ReLockSequenceNumberForInactiveLock;
            num1 += 2 * (num2 - 1);
          }
          requestData[(object) string.Concat((object) num1)] = (object) ("Re-Lock #" + (object) num2);
          requestData[(object) string.Concat((object) (num1 + 1))] = (object) LockUtils.GetRelockFee(this.SessionObjects, this.loanData);
        }
        lockLog.RateLockAction = RateLockAction.ReLockInactiveCurrentPricing;
        new LockRequestCalculator(this.sessionObjects, this.loanData).PerformLockRequestCalculations(requestData);
      }
      else
        lockLog.RateLockAction = rateLockAction;
    }

    private void GetParentDataForCancelledLock(Hashtable requestData)
    {
      EllieMae.EMLite.DataEngine.Log.LockConfirmLog confirmLockLog = this.loanData.GetLogList().GetConfirmLockLog();
      if (confirmLockLog == null)
        return;
      Hashtable parentData = this.loanData.GetLogList().GetLockRequest(confirmLockLog.RequestGUID).GetLockRequestSnapshot();
      Action<int, int> action = (Action<int, int>) ((startIdx, endIdx) =>
      {
        for (int index = startIdx; index <= endIdx; ++index)
        {
          string key = index.ToString();
          if (requestData.Contains((object) key) && parentData.Contains((object) key))
            requestData[(object) key] = parentData[(object) key];
          else if (requestData.Contains((object) key) && !parentData.Contains((object) key))
            requestData[(object) key] = (object) "";
          else if (!requestData.Contains((object) key) && parentData.Contains((object) key))
            requestData.Add((object) key, parentData[(object) key]);
        }
      });
      action(3474, 3513);
      action(3755, 3774);
      action(4276, 4315);
      action(4316, 4335);
      action(4356, 4395);
      action(4396, 4415);
      action(3371, 3378);
      if (requestData.Contains((object) "3372") && string.IsNullOrEmpty(requestData[(object) "3372"].ToString()))
        requestData[(object) "3372"] = (object) "//";
      if (!requestData.Contains((object) "3376") || !string.IsNullOrEmpty(requestData[(object) "3376"].ToString()))
        return;
      requestData[(object) "3376"] = (object) "//";
    }

    public bool AllowAutoLock(
      bool suppressAutoLock,
      bool isRelock,
      bool isLockExtension,
      bool isLockCancellation,
      bool isGetPricing)
    {
      if (suppressAutoLock || this.SessionObjects.StartupInfo.ProductPricingPartner == null || !this.SessionObjects.StartupInfo.ProductPricingPartner.IsEPPS)
        return false;
      if (this.SessionObjects.StartupInfo.ProductPricingPartner.EnableAutoLockRelock && this.loanData.GetField("2626") == "Correspondent" && !string.IsNullOrEmpty(this.loanData.GetField("4207")) && this.loanData.GetField("4207") != "//")
      {
        this.loanData.SetField("4207", "");
        this.loanData.SetField("4120", "");
        this.loanData.SetField("4208", "");
      }
      return isRelock ? ((!this.SessionObjects.StartupInfo.ProductPricingPartner.EnableAutoLockRequest ? 0 : (this.SessionObjects.StartupInfo.ProductPricingPartner.EnableAutoLockRelock ? 1 : 0)) & (isGetPricing ? 1 : 0)) != 0 && !this.AutoLockExcluded() : (isLockExtension ? this.SessionObjects.StartupInfo.ProductPricingPartner.EnableAutoLockExtensionRequest && !this.AutoLockExcluded() : (isLockCancellation ? this.SessionObjects.StartupInfo.ProductPricingPartner.EnableAutoCancelRequest && !this.AutoLockExcluded() : this.SessionObjects.StartupInfo.ProductPricingPartner.EnableAutoLockRequest && !this.AutoLockExcluded() && !this.ExcludeWithdrawnLoan()));
    }

    private bool AutoLockExcluded()
    {
      if (this.IsCriteriaMatch(this.loanData.GetField("2952"), this.sessionObjects.StartupInfo.ProductPricingPartner.ExcludeAutoLockLoanType) || this.IsCriteriaMatch(this.loanData.GetField("2945"), this.sessionObjects.StartupInfo.ProductPricingPartner.ExcludeAutoLockPropertyState) || this.IsCriteriaMatch(this.loanData.GetField("2626"), this.sessionObjects.StartupInfo.ProductPricingPartner.ExcludeAutoLockChannel) || this.IsCriteriaMatch(this.loanData.GetField("2951"), this.sessionObjects.StartupInfo.ProductPricingPartner.ExcludeAutoLockLoanPurpose) || this.IsCriteriaMatch(this.loanData.GetField("2953"), this.sessionObjects.StartupInfo.ProductPricingPartner.ExcludeAutoLockAmortizationType) || this.IsCriteriaMatch(this.loanData.GetField("2950"), this.sessionObjects.StartupInfo.ProductPricingPartner.ExcludeAutoLockPropertyWillBe) || this.IsCriteriaMatch(this.loanData.GetField("2958"), this.sessionObjects.StartupInfo.ProductPricingPartner.ExcludeAutoLockLienPosition) || this.IsCriteriaMatch(this.loanData.GetField("2866"), this.sessionObjects.StartupInfo.ProductPricingPartner.ExcludeAutoLockLoanProgram) || this.IsCriteriaMatch(this.loanData.GetField("3041"), this.sessionObjects.StartupInfo.ProductPricingPartner.ExcludeAutoLockPlanCode))
        return true;
      ConditionEvaluators conditionEvaluators = new ConditionEvaluators(this.SessionObjects.BpmManager.GetRules(BizRuleType.AutoLockExclusionRules, true), false);
      ExecutionContext context = new ExecutionContext(this.sessionObjects.UserInfo, this.loanData, (IServerDataProvider) new CustomCodeSessionDataProvider(this.sessionObjects));
      bool flag = false;
      foreach (ConditionEvaluator conditionEvaluator in conditionEvaluators)
      {
        if (conditionEvaluator.Rule.Condition == BizRule.Condition.AdvancedCoding && conditionEvaluator.AppliesTo(context))
          flag = true;
      }
      return flag;
    }

    public bool ExcludeWithdrawnLoan()
    {
      string field = this.loanData.GetField("4120");
      if (!string.IsNullOrEmpty(field) && !(field == "//"))
      {
        if (this.LoanData.GetLogList().GetCurrentLockRequest() == null)
          return true;
        EllieMae.EMLite.DataEngine.Log.LockConfirmLog[] allConfirmLocks = this.LoanData.GetLogList().GetAllConfirmLocks();
        if (string.Compare(this.loanData.GetField("LOCKRATE.RATESTATUS"), "Expired", true) == 0 || allConfirmLocks.Length == 0)
          return true;
      }
      return false;
    }

    private bool IsCriteriaMatch(string value, string criteria)
    {
      if (string.IsNullOrEmpty(criteria.Trim()) || string.IsNullOrEmpty(value.Trim()))
        return false;
      string[] source = criteria.Split(';');
      return source.Length != 0 && !(source[0] != "$") && ((IEnumerable<string>) source).Any<string>((Func<string, bool>) (lt => string.Compare(lt, value, true) == 0));
    }

    public void SetNumFieldDecimal(Hashtable snapshotData)
    {
      foreach (string str in snapshotData.Keys.Cast<string>().ToList<string>())
      {
        Decimal result = 0M;
        if (Decimal.TryParse(snapshotData[(object) str].ToString(), out result) && LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields(str))
        {
          if (this.sessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas)
            snapshotData[(object) str] = (object) result.ToString("N10");
          else
            snapshotData[(object) str] = (object) result.ToString("N3");
        }
      }
    }

    internal void CreateExtendRateLockRequest(
      EllieMae.EMLite.DataEngine.Log.LockRequestLog lockLog,
      Hashtable requestData,
      bool isParentExtension,
      bool isAutoLockAllowed,
      int daysToExtend,
      DateTime newExpirationDate,
      Decimal priceAdjustment,
      string comment,
      Decimal reLockFee,
      Decimal customPriceAdj,
      string cpaDesc,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
      string str1 = string.Empty;
      for (int index = 2148; index <= 2203; ++index)
      {
        if (index != 2161)
        {
          string str2 = string.Concat(this.getSnapshotField(requestData, (object) index.ToString()));
          int num = index - 60;
          this.setSnapshotField(requestData, (object) num.ToString(), (object) str2);
        }
      }
      if (isParentExtension)
        this.setSnapshotField(requestData, (object) "2091", this.getSnapshotField(requestData, (object) "3358"));
      this.setSnapshotField(requestData, (object) "2101", (object) string.Concat(this.getSnapshotField(requestData, (object) "3420")));
      for (int index = 2448; index <= 2481; ++index)
      {
        string str3 = string.Concat(this.getSnapshotField(requestData, (object) index.ToString()));
        int num = index - 34;
        this.setSnapshotField(requestData, (object) num.ToString(), (object) str3);
      }
      for (int index = 2733; index <= 2775; ++index)
      {
        string str4 = string.Concat(this.getSnapshotField(requestData, (object) index.ToString()));
        int num = index - 86;
        this.setSnapshotField(requestData, (object) num.ToString(), (object) str4);
      }
      for (int index = 3474; index <= 3493; ++index)
      {
        string str5 = string.Concat(this.getSnapshotField(requestData, (object) index.ToString()));
        int num = index - 20;
        this.setSnapshotField(requestData, (object) num.ToString(), (object) str5);
      }
      int num1;
      if (lockLog.RateLockAction == RateLockAction.TradeExtension && reLockFee != 0M)
      {
        int num2 = 0;
        for (int index = 0; index < 10; ++index)
        {
          Hashtable snapshot1 = requestData;
          num1 = 4276 + index * 2;
          string key1 = num1.ToString();
          if (string.IsNullOrEmpty(string.Concat(this.getSnapshotField(snapshot1, (object) key1))))
          {
            Hashtable snapshot2 = requestData;
            num1 = 4276 + index * 2;
            string key2 = num1.ToString();
            string str6 = "(Trade) Re-Lock #" + (object) (index + 1);
            this.setSnapshotField(snapshot2, (object) key2, (object) str6);
            Hashtable snapshot3 = requestData;
            num1 = 4276 + index * 2 + 1;
            string key3 = num1.ToString();
            string str7 = string.Concat((object) reLockFee);
            this.setSnapshotField(snapshot3, (object) key3, (object) str7);
            break;
          }
          ++num2;
        }
        if (num2 == 10)
          str1 += "Re-lock limit has been reached. ";
      }
      for (int index = 0; index < 20; ++index)
      {
        int num3 = 4276 + index;
        int num4 = 4256 + index;
        string str8 = string.Concat(this.getSnapshotField(requestData, (object) num3.ToString()));
        this.setSnapshotField(requestData, (object) num4.ToString(), (object) str8);
      }
      if (lockLog.RateLockAction == RateLockAction.TradeExtension && customPriceAdj != 0M)
      {
        this.setSnapshotField(requestData, (object) "2204", (object) comment);
        int num5 = 0;
        for (int index = 0; index < 10; ++index)
        {
          Hashtable snapshot4 = requestData;
          num1 = 4356 + index * 2;
          string key4 = num1.ToString();
          if (string.IsNullOrEmpty(string.Concat(this.getSnapshotField(snapshot4, (object) key4))))
          {
            Hashtable snapshot5 = requestData;
            num1 = 4356 + index * 2;
            string key5 = num1.ToString();
            string str9 = "(Trade) " + cpaDesc;
            this.setSnapshotField(snapshot5, (object) key5, (object) str9);
            Hashtable snapshot6 = requestData;
            num1 = 4356 + index * 2 + 1;
            string key6 = num1.ToString();
            string str10 = string.Concat((object) customPriceAdj);
            this.setSnapshotField(snapshot6, (object) key6, (object) str10);
            break;
          }
          ++num5;
        }
        if (num5 == 10)
          str1 += "Custom Price limit has been reached. ";
        if (num5 == 10)
          str1 += "Custom Price limit has been reached. ";
      }
      for (int index = 0; index < 20; ++index)
      {
        int num6 = 4356 + index;
        int num7 = 4336 + index;
        string str11 = string.Concat(this.getSnapshotField(requestData, (object) num6.ToString()));
        this.setSnapshotField(requestData, (object) num7.ToString(), (object) str11);
      }
      this.setSnapshotField(requestData, (object) "2848", (object) string.Concat(this.getSnapshotField(requestData, (object) "2205")));
      this.setSnapshotField(requestData, (object) "3254", (object) string.Concat(this.getSnapshotField(requestData, (object) "3256")));
      if (string.Concat(this.getSnapshotField(requestData, (object) "3364")) != "" && string.Concat(this.getSnapshotField(requestData, (object) "3364")) != "//")
        this.setSnapshotField(requestData, (object) "3369", (object) string.Concat(this.getSnapshotField(requestData, (object) "3364")));
      else if (string.Concat(this.getSnapshotField(requestData, (object) "2151")) != "")
        this.setSnapshotField(requestData, (object) "3369", (object) string.Concat(this.getSnapshotField(requestData, (object) "2151")));
      this.setSnapshotField(requestData, (object) "3360", (object) daysToExtend);
      this.setSnapshotField(requestData, (object) "3361", (object) newExpirationDate.ToString("MM/dd/yyyy"));
      this.setSnapshotField(requestData, (object) "3362", (object) priceAdjustment);
      this.setSnapshotField(requestData, (object) "2144", (object) comment);
      if (string.Concat(this.getSnapshotField(requestData, (object) "3358")) == "" || string.Concat(this.getSnapshotField(requestData, (object) "3358")) == "//")
      {
        if (string.Concat(this.getSnapshotField(requestData, (object) "3364")) != "" && string.Concat(this.getSnapshotField(requestData, (object) "3364")) != "//")
          this.setSnapshotField(requestData, (object) "3358", (object) string.Concat(this.getSnapshotField(requestData, (object) "3364")));
        else if (string.Concat(this.getSnapshotField(requestData, (object) "2151")) != "")
          this.setSnapshotField(requestData, (object) "3358", (object) string.Concat(this.getSnapshotField(requestData, (object) "2151")));
      }
      this.setSnapshotField(requestData, (object) "3363", (object) daysToExtend);
      this.setSnapshotField(requestData, (object) "3364", (object) newExpirationDate.ToString("MM/dd/yyyy"));
      this.setSnapshotField(requestData, (object) "3365", (object) priceAdjustment);
      int num8 = 0;
      for (int index = 0; index < 10; ++index)
      {
        Hashtable snapshot = requestData;
        num1 = 3474 + index * 2;
        string key = num1.ToString();
        if (!string.IsNullOrEmpty(string.Concat(this.getSnapshotField(snapshot, (object) key))))
          ++num8;
      }
      int num9 = EllieMae.EMLite.Common.Utils.ParseInt((object) string.Concat(this.getSnapshotField(requestData, (object) "3433")), 0) + 1;
      if (num9 > 10 || num8 == 10)
        str1 = "Lock extension limit has been reached. " + str1;
      if (str1 != string.Empty)
        throw new Exception("Failed to create lock extension for loan " + this.loanData.LoanNumber + ": " + str1);
      this.setSnapshotField(requestData, (object) "3433", (object) num9);
      string str12 = string.Empty;
      if (lockLog.RateLockAction == RateLockAction.TradeExtension)
        str12 = "Trade Ext - ";
      for (int index = 3454; index <= 3473; index += 2)
      {
        if (string.IsNullOrEmpty(string.Concat(this.getSnapshotField(requestData, (object) index.ToString()))))
        {
          Hashtable snapshot7 = requestData;
          num1 = index + 1;
          string key7 = num1.ToString();
          if (string.IsNullOrEmpty(string.Concat(this.getSnapshotField(snapshot7, (object) key7))))
          {
            this.setSnapshotField(requestData, (object) index.ToString(), (object) ("(" + str12 + (object) daysToExtend + (daysToExtend == 1 ? (object) " day" : (object) " days") + ")"));
            Hashtable snapshot8 = requestData;
            num1 = index + 1;
            string key8 = num1.ToString();
            string str13 = priceAdjustment.ToString("N3");
            this.setSnapshotField(snapshot8, (object) key8, (object) str13);
            break;
          }
        }
      }
      for (int index = 3474; index <= 3493; index += 2)
      {
        if (string.IsNullOrEmpty(string.Concat(this.getSnapshotField(requestData, (object) index.ToString()))))
        {
          Hashtable snapshot9 = requestData;
          num1 = index + 1;
          string key9 = num1.ToString();
          if (string.IsNullOrEmpty(string.Concat(this.getSnapshotField(snapshot9, (object) key9))))
          {
            this.setSnapshotField(requestData, (object) index.ToString(), (object) ("(" + str12 + (object) daysToExtend + (daysToExtend == 1 ? (object) " day" : (object) " days") + ")"));
            Hashtable snapshot10 = requestData;
            num1 = index + 1;
            string key10 = num1.ToString();
            string str14 = priceAdjustment.ToString("N3");
            this.setSnapshotField(snapshot10, (object) key10, (object) str14);
            break;
          }
        }
      }
      if (EllieMae.EMLite.Common.Utils.ParseInt((object) string.Concat(this.getSnapshotField(requestData, (object) "3431")), 0) > 0)
      {
        int num10 = EllieMae.EMLite.Common.Utils.ParseInt((object) string.Concat(this.getSnapshotField(requestData, (object) "3431")), 0);
        this.setSnapshotField(requestData, (object) "3431", (object) (num10 + daysToExtend));
      }
      else
        this.setSnapshotField(requestData, (object) "3431", (object) daysToExtend);
      this.setSnapshotField(requestData, (object) "2151", this.getSnapshotField(requestData, (object) "3369"));
      this.setSnapshotField(requestData, (object) "4209", (object) LockUtils.GetRequestLockStatus(this.loanData));
      new LockRequestCalculator(this.sessionObjects, this.loanData).PerformSnapshotCalculations(requestData);
      lockLog.AddLockRequestSnapshot(requestData);
      foreach (EllieMae.EMLite.DataEngine.Log.LockRequestLog allLockRequest in this.loanData.GetLogList().GetAllLockRequests())
      {
        if (allLockRequest.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.Requested)
          allLockRequest.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.OldRequest;
      }
      this.LoanData.GetLogList().AddRecord((LogRecordBase) lockLog);
      this.loanData.TriggerCalculation("761", this.loanData.GetField("761"));
      this.loanData.SetField("3360", "");
      this.loanData.SetField("3361", "");
      this.loanData.SetField("3362", "");
      this.loanData.SetField("3370", "");
      if (!isAutoLockAllowed)
        return;
      requestData = lockLog.GetLockRequestSnapshot();
      new LockRequestCalculator(this.SessionObjects, this.loanData).PerformSnapshotCalculations(requestData);
      this.LockRateRequest(lockLog, requestData, this.SessionObjects.UserInfo, true, syncOption: syncOption);
    }

    public EllieMae.EMLite.DataEngine.Log.LockRequestLog CreateExtendedRateLockRequest(
      UserInfo requestingUser,
      int daysToExtend,
      DateTime newExpirationDate,
      Decimal priceAdjustment,
      string comment)
    {
      using (Tracing.StartTimer(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.CreateExtendedRateLockRequest"))
      {
        bool isParentExtension = false;
        EllieMae.EMLite.DataEngine.Log.LockConfirmLog confirmLockLog = this.loanData.GetLogList().GetConfirmLockLog();
        if (confirmLockLog == null)
          return (EllieMae.EMLite.DataEngine.Log.LockRequestLog) null;
        EllieMae.EMLite.DataEngine.Log.LockRequestLog confirmedLockRequest = this.loanData.GetLogList().GetCurrentConfirmedLockRequest();
        EllieMae.EMLite.DataEngine.Log.LockRequestLog confirmationLock = LockUtils.GetAssignToTradePostConfirmationLock(this);
        if (confirmedLockRequest != null)
          isParentExtension = confirmedLockRequest.IsLockExtension;
        else if (confirmationLock != null)
          isParentExtension = confirmationLock.IsLockExtension;
        string requestGUID = confirmLockLog.RequestGUID;
        if (confirmationLock != null)
          requestGUID = confirmationLock.Guid;
        EllieMae.EMLite.DataEngine.Log.LockRequestLog lockLog = new EllieMae.EMLite.DataEngine.Log.LockRequestLog(this.loanData.GetLogList());
        lockLog.Date = this.SessionObjects.Session.ServerTime;
        lockLog.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.Requested;
        lockLog.IsLockExtension = true;
        lockLog.SetRequestingUser(requestingUser.Userid, requestingUser.FullName);
        lockLog.ParentLockGuid = requestGUID;
        lockLog.ReLockSequenceNumberForInactiveLock = LockUtils.GetReLockSequenceNumberForInactiveLock(this.loanData);
        Hashtable lockRequestSnapshot = this.loanData.GetLogList().GetLockRequest(requestGUID).GetLockRequestSnapshot();
        bool isAutoLockAllowed = this.AllowAutoLock(false, false, true, false, false);
        this.CreateExtendRateLockRequest(lockLog, lockRequestSnapshot, isParentExtension, isAutoLockAllowed, daysToExtend, newExpirationDate, priceAdjustment, comment, 0M, 0M, string.Empty);
        return lockLog;
      }
    }

    public EllieMae.EMLite.DataEngine.Log.LockRequestLog CreateHistoricalExtendedRateLockRequest(
      UserInfo requestingUser,
      RateLockAction rateLockAction)
    {
      using (Tracing.StartTimer(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.CreateHistoricalExtendedRateLockRequest"))
      {
        bool flag = false;
        EllieMae.EMLite.DataEngine.Log.LockConfirmLog confirmLockLog = this.loanData.GetLogList().GetConfirmLockLog();
        if (confirmLockLog == null)
          return (EllieMae.EMLite.DataEngine.Log.LockRequestLog) null;
        EllieMae.EMLite.DataEngine.Log.LockRequestLog confirmedLockRequest = this.loanData.GetLogList().GetCurrentConfirmedLockRequest();
        EllieMae.EMLite.DataEngine.Log.LockRequestLog confirmationLock = LockUtils.GetAssignToTradePostConfirmationLock(this);
        if (confirmedLockRequest != null)
          flag = confirmedLockRequest.IsLockExtension;
        else if (confirmationLock != null)
          flag = confirmationLock.IsLockExtension;
        string str = confirmLockLog.RequestGUID;
        if (confirmationLock != null)
          str = confirmationLock.Guid;
        EllieMae.EMLite.DataEngine.Log.LockRequestLog rec = new EllieMae.EMLite.DataEngine.Log.LockRequestLog(this.loanData.GetLogList());
        rec.Date = this.SessionObjects.Session.ServerTime;
        rec.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.Requested;
        rec.IsLockExtension = true;
        rec.SetRequestingUser(requestingUser.Userid, requestingUser.FullName);
        rec.ParentLockGuid = str;
        Hashtable hashtable = this.loanData.PrepareLockRequestData();
        int num1 = EllieMae.EMLite.Common.Utils.ParseInt((object) string.Concat(this.getSnapshotField(hashtable, (object) "3360")));
        EllieMae.EMLite.Common.Utils.ParseDate((object) string.Concat(this.getSnapshotField(hashtable, (object) "3361")));
        EllieMae.EMLite.Common.Utils.ParseDecimal((object) string.Concat(this.getSnapshotField(hashtable, (object) "3362")));
        if (confirmedLockRequest != null && confirmedLockRequest.GetLockRequestSnapshot() != null)
          this.setSnapshotField(hashtable, (object) "3433", confirmedLockRequest.GetLockRequestSnapshot()[(object) "3433"]);
        if (EllieMae.EMLite.Common.Utils.ParseInt((object) string.Concat(this.getSnapshotField(hashtable, (object) "3431")), 0) > 0)
        {
          int num2 = EllieMae.EMLite.Common.Utils.ParseInt((object) string.Concat(this.getSnapshotField(hashtable, (object) "3431")), 0);
          this.setSnapshotField(hashtable, (object) "3431", (object) (num2 + num1));
        }
        else
          this.setSnapshotField(hashtable, (object) "3431", (object) num1);
        this.setSnapshotField(hashtable, (object) "4209", (object) LockUtils.GetRequestLockStatus(this.loanData));
        rec.RateLockAction = rateLockAction;
        string field = this.loanData.GetField("LOCKRATE.RATESTATUS");
        if (field == "Cancelled" || field == "Expired")
        {
          int numberForInactiveLock = LockUtils.GetReLockSequenceNumberForInactiveLock(this.loanData);
          rec.ReLockSequenceNumberForInactiveLock = numberForInactiveLock + 1;
          switch (field)
          {
            case "Cancelled":
              rec.ParentLockGuid = this.loanData.GetLogList().GetMostRecentLockCancellation().Guid;
              break;
            case "Expired":
              rec.ParentLockGuid = this.loanData.GetLogList().GetCurrentLockConfirmation().Guid;
              break;
          }
        }
        else
          rec.ReLockSequenceNumberForInactiveLock = LockUtils.GetReLockSequenceNumberForInactiveLock(this.loanData);
        new LockRequestCalculator(this.sessionObjects, this.loanData).PerformSnapshotCalculations(hashtable);
        rec.AddLockRequestSnapshot(hashtable);
        foreach (EllieMae.EMLite.DataEngine.Log.LockRequestLog allLockRequest in this.loanData.GetLogList().GetAllLockRequests())
        {
          if (allLockRequest.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.Requested)
            allLockRequest.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.OldRequest;
        }
        this.LoanData.GetLogList().AddRecord((LogRecordBase) rec);
        this.loanData.TriggerCalculation("761", this.loanData.GetField("761"));
        return rec;
      }
    }

    public EllieMae.EMLite.DataEngine.Log.LockRequestLog CreateRateLockCancellationRequest(
      UserInfo requestingUser,
      DateTime cancellationDate,
      string comment)
    {
      using (Tracing.StartTimer(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.CreateLockCancellationRequest"))
      {
        string cancellationParentGuid = this.getLockCancellationParentGuid();
        if (string.IsNullOrWhiteSpace(cancellationParentGuid))
          return (EllieMae.EMLite.DataEngine.Log.LockRequestLog) null;
        EllieMae.EMLite.DataEngine.Log.LockRequestLog cancellationRequest = new EllieMae.EMLite.DataEngine.Log.LockRequestLog(this.loanData.GetLogList());
        cancellationRequest.Date = this.SessionObjects.Session.ServerTime;
        cancellationRequest.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.Requested;
        cancellationRequest.IsLockCancellation = true;
        cancellationRequest.SetRequestingUser(requestingUser.Userid, requestingUser.FullName);
        cancellationRequest.ParentLockGuid = cancellationParentGuid;
        cancellationRequest.ReLockSequenceNumberForInactiveLock = LockUtils.GetReLockSequenceNumberForInactiveLock(this.loanData);
        Hashtable hashtable = this.loanData.PrepareLockCancellationData(this.loanData.GetLogList().GetLockRequest(cancellationParentGuid).GetLockRequestSnapshot());
        this.setSnapshotField(hashtable, (object) "2144", (object) comment);
        this.setSnapshotField(hashtable, (object) "4209", (object) LockUtils.GetRequestLockStatus(this.loanData));
        this.setSnapshotField(hashtable, (object) "5029", (object) string.Empty);
        cancellationRequest.AddLockRequestSnapshot(hashtable);
        foreach (EllieMae.EMLite.DataEngine.Log.LockRequestLog allLockRequest in this.loanData.GetLogList().GetAllLockRequests())
        {
          if (allLockRequest.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.Requested)
            allLockRequest.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.OldRequest;
        }
        this.LoanData.GetLogList().AddRecord((LogRecordBase) cancellationRequest);
        this.loanData.TriggerCalculation("761", this.loanData.GetField("761"));
        if (this.AllowAutoLock(false, false, false, true, false))
          this.CancelRateLock(cancellationRequest, hashtable, this.SessionObjects.UserInfo);
        return cancellationRequest;
      }
    }

    public EllieMae.EMLite.DataEngine.Log.LockRequestLog CreateRateRelockRequest(bool isGetPricing)
    {
      return this.CreateRateRelockRequest(this.sessionObjects.UserInfo, isGetPricing);
    }

    public EllieMae.EMLite.DataEngine.Log.LockRequestLog CreateRateRelockRequest(
      UserInfo requestingUser,
      bool isGetPricing,
      bool fromPlatform = false)
    {
      using (Tracing.StartTimer(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.CreateRateRelockRequest"))
      {
        EllieMae.EMLite.DataEngine.Log.LockConfirmLog confirmLockLog = this.loanData.GetLogList().GetConfirmLockLog();
        if (confirmLockLog == null)
          return (EllieMae.EMLite.DataEngine.Log.LockRequestLog) null;
        LockDeskHoursManager.ValidateLockRequestTimeThickClient((IClientSession) this.sessionObjects.Session, this.sessionObjects, this, new bool?(), out OnrpCalcInfo _, true, fromPlatform);
        string field1 = this.loanData.GetField("2088");
        this.ValidateBestEffortDailyLimit(ref field1, this.loanData.GetField("2089"), this.loanData.GetField("3965"), this.loanData.FltVal("2965"), out bool _);
        this.ValidateLockPeriodRestriction();
        EllieMae.EMLite.DataEngine.Log.LockRequestLog lockRequest = this.loanData.GetLogList().GetLockRequest(confirmLockLog.RequestGUID);
        Hashtable lockRequestSnapshot = lockRequest.GetLockRequestSnapshot();
        EllieMae.EMLite.DataEngine.Log.LockRequestLog rateRelockRequest = new EllieMae.EMLite.DataEngine.Log.LockRequestLog(this.loanData.GetLogList());
        rateRelockRequest.Date = this.SessionObjects.Session.ServerTime;
        rateRelockRequest.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.Requested;
        rateRelockRequest.SetRequestingUser(requestingUser.Userid, requestingUser.FullName);
        rateRelockRequest.IsRelock = true;
        rateRelockRequest.IsLockExtension = lockRequest.IsLockExtension;
        rateRelockRequest.ParentLockGuid = lockRequest.Guid;
        rateRelockRequest.ReLockSequenceNumberForInactiveLock = LockUtils.GetReLockSequenceNumberForInactiveLock(this.loanData);
        if (!string.IsNullOrEmpty(this.loanData.GetField("4789")))
          rateRelockRequest.PriceConcessionIndicator = this.loanData.GetField("4789");
        if (!string.IsNullOrEmpty(this.loanData.GetField("4790")))
          rateRelockRequest.LockExtensionIndicator = this.loanData.GetField("4790");
        if (!string.IsNullOrEmpty(this.loanData.GetField("4791")))
          rateRelockRequest.PriceConcessionRequestStatus = this.loanData.GetField("4791");
        Hashtable snapshotForLockUpdate = this.CreateSnapshotForLockUpdate(lockRequestSnapshot, rateRelockRequest.IsLockExtension, fromPlatform);
        this.ClearItemsForLockUpdate(snapshotForLockUpdate);
        rateRelockRequest.AddLockRequestSnapshot(snapshotForLockUpdate);
        foreach (EllieMae.EMLite.DataEngine.Log.LockRequestLog allLockRequest in this.loanData.GetLogList().GetAllLockRequests())
        {
          if (allLockRequest.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.Requested)
            allLockRequest.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.OldRequest;
        }
        this.LoanData.GetLogList().AddRecord((LogRecordBase) rateRelockRequest);
        this.loanData.TriggerCalculation("761", this.loanData.GetField("761"));
        if (this.sessionObjects.GetPolicySetting("NotAllowPricingChange"))
        {
          string field2 = this.loanData.GetField("3039");
          if (field2 != "//" && EllieMae.EMLite.Common.Utils.IsDate((object) field2))
            this.loanData.SetField("3039", "");
        }
        if (this.AllowAutoLock(false, true, false, false, isGetPricing))
          LockUtils.PerformAutoLock(this.sessionObjects, rateRelockRequest, this);
        return rateRelockRequest;
      }
    }

    private Hashtable CreateSnapshotForLockUpdate(
      Hashtable parentData,
      bool isLockExtension,
      bool fromPlatform = false)
    {
      Hashtable snapshotForLockUpdate = this.loanData.PrepareLockRequestData();
      string field = this.loanData.GetField("LOCKRATE.RATESTATUS");
      bool flag = field != "Cancelled" || field != "Expired";
      if (fromPlatform & flag)
      {
        string empty = string.Empty;
        for (int index = 3474; index <= 3493; ++index)
        {
          string key1 = index.ToString();
          if (parentData.ContainsKey((object) key1))
          {
            string key2 = (index - 20).ToString();
            if (snapshotForLockUpdate.ContainsKey((object) key2))
              snapshotForLockUpdate[(object) key2] = parentData[(object) key1];
            else
              snapshotForLockUpdate.Add((object) key2, parentData[(object) key1]);
          }
        }
        for (int index = 4276; index <= 4295; ++index)
        {
          string key3 = index.ToString();
          if (parentData.ContainsKey((object) key3))
          {
            string key4 = (index - 20).ToString();
            if (snapshotForLockUpdate.ContainsKey((object) key4))
              snapshotForLockUpdate[(object) key4] = parentData[(object) key3];
            else
              snapshotForLockUpdate.Add((object) key4, parentData[(object) key3]);
          }
        }
        for (int index = 4356; index <= 4375; ++index)
        {
          string key5 = index.ToString();
          if (parentData.ContainsKey((object) key5))
          {
            string key6 = (index - 20).ToString();
            if (snapshotForLockUpdate.ContainsKey((object) key6))
              snapshotForLockUpdate[(object) key6] = parentData[(object) key5];
            else
              snapshotForLockUpdate.Add((object) key6, parentData[(object) key5]);
          }
        }
      }
      if (snapshotForLockUpdate.ContainsKey((object) "OPTIMAL.HISTORY") && !LockUtils.IsValidateRelockTrans(snapshotForLockUpdate[(object) "OPTIMAL.HISTORY"].ToString()))
      {
        if (parentData.ContainsKey((object) "OPTIMAL.HISTORY"))
          snapshotForLockUpdate[(object) "OPTIMAL.HISTORY"] = (object) string.Concat(parentData[(object) "OPTIMAL.HISTORY"]);
        if (parentData.ContainsKey((object) "3848"))
          snapshotForLockUpdate[(object) "3848"] = (object) parentData[(object) "3848"].ToString();
        if (parentData.ContainsKey((object) "3873"))
          snapshotForLockUpdate[(object) "3873"] = (object) parentData[(object) "3873"].ToString();
        if (parentData.ContainsKey((object) "3875"))
          snapshotForLockUpdate[(object) "3875"] = (object) parentData[(object) "3875"].ToString();
      }
      if (isLockExtension)
      {
        foreach (string key in new List<string>()
        {
          "2088",
          "3254",
          "2090",
          "2091"
        })
        {
          if (parentData.ContainsKey((object) key) && (!(key == "3254") && !(key == "2091") || EllieMae.EMLite.Common.Utils.IsDate(parentData[(object) key])))
            snapshotForLockUpdate[(object) key] = (object) parentData[(object) key].ToString();
        }
        for (int index = 0; index < EllieMae.EMLite.DataEngine.Log.LockRequestLog.LockExtensionFields.Count; ++index)
        {
          string lockExtensionField = EllieMae.EMLite.DataEngine.Log.LockRequestLog.LockExtensionFields[index];
          if (parentData.ContainsKey((object) lockExtensionField))
            snapshotForLockUpdate[(object) lockExtensionField] = (object) parentData[(object) lockExtensionField].ToString();
        }
        if (parentData.ContainsKey((object) "3431"))
          snapshotForLockUpdate[(object) "3431"] = (object) parentData[(object) "3431"].ToString();
        if (parentData.ContainsKey((object) "3433"))
          snapshotForLockUpdate[(object) "3433"] = (object) parentData[(object) "3433"].ToString();
      }
      if (snapshotForLockUpdate.Contains((object) "4058"))
        snapshotForLockUpdate[(object) "4058"] = (object) "";
      if (snapshotForLockUpdate.Contains((object) "4070"))
        snapshotForLockUpdate[(object) "4070"] = (object) "";
      if (snapshotForLockUpdate.Contains((object) "4059"))
        snapshotForLockUpdate[(object) "4059"] = (object) "N";
      return snapshotForLockUpdate;
    }

    private void ClearItemsForLockUpdate(Hashtable requestData)
    {
      if (!this.CheckProviderAndFlag() || !LockUtils.IsActiveRelock(this.loanData))
        return;
      IDictionary policySettings = this.sessionObjects.StartupInfo.PolicySettings;
      this.ClearItems(requestData, !policySettings.Contains((object) "Policies.LOCKUPDATEEXTENSIONFEES") || string.Equals(Convert.ToString(policySettings[(object) "Policies.LOCKUPDATEEXTENSIONFEES"]), "True", StringComparison.CurrentCultureIgnoreCase), !policySettings.Contains((object) "Policies.LOCKUPDATECUSTOMPRICEADJUSTMENTS") || string.Equals(Convert.ToString(policySettings[(object) "Policies.LOCKUPDATECUSTOMPRICEADJUSTMENTS"]), "True", StringComparison.CurrentCultureIgnoreCase), !policySettings.Contains((object) "Policies.LOCKUPDATEPRICECONCESSIONS") || string.Equals(Convert.ToString(policySettings[(object) "Policies.LOCKUPDATEPRICECONCESSIONS"]), "True", StringComparison.CurrentCultureIgnoreCase), !policySettings.Contains((object) "Policies.LOCKUPDATERELOCKFEES") || string.Equals(Convert.ToString(policySettings[(object) "Policies.LOCKUPDATERELOCKFEES"]), "True", StringComparison.CurrentCultureIgnoreCase));
    }

    private void ClearItemsForRelocks(Hashtable requestData, bool isRelock)
    {
      if (!this.CheckProviderAndFlag(false) || LockUtils.GetWcpStateForRelock(this.sessionObjects, this.loanData, isRelock) != LockUtils.WorstCasePriceState.WorstCasePriceWithinDaysCap)
        return;
      IDictionary policySettings = this.sessionObjects.StartupInfo.PolicySettings;
      this.ClearItems(requestData, !policySettings.Contains((object) "Policies.RELOCKEXTENSIONFEES") || string.Equals(Convert.ToString(policySettings[(object) "Policies.RELOCKEXTENSIONFEES"]), "True", StringComparison.CurrentCultureIgnoreCase), !policySettings.Contains((object) "Policies.RELOCKCUSTOMPRICEADJUSTMENTS") || string.Equals(Convert.ToString(policySettings[(object) "Policies.RELOCKCUSTOMPRICEADJUSTMENTS"]), "True", StringComparison.CurrentCultureIgnoreCase), !policySettings.Contains((object) "Policies.RELOCKPRICECONCESSIONS") || string.Equals(Convert.ToString(policySettings[(object) "Policies.RELOCKPRICECONCESSIONS"]), "True", StringComparison.CurrentCultureIgnoreCase), !policySettings.Contains((object) "Policies.RELOCKRELOCKFEES") || string.Equals(Convert.ToString(policySettings[(object) "Policies.RELOCKRELOCKFEES"]), "True", StringComparison.CurrentCultureIgnoreCase));
    }

    public bool CheckProviderAndFlag(bool allowNoProvider = true, bool isRelock = true)
    {
      if (isRelock && string.IsNullOrEmpty(this.loanData.GetField("OPTIMAL.HISTORY")))
        return false;
      string a = Convert.ToString(this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EPPS_EPC2_SHIP_DARK_SR"]);
      if (string.IsNullOrEmpty(a) || string.Equals(a, "true", StringComparison.CurrentCultureIgnoreCase))
        return false;
      if (this.sessionObjects.StartupInfo.ProductPricingPartner == null)
        return allowNoProvider;
      return this.SessionObjects.StartupInfo.ProductPricingPartner.IsEPPS && this.SessionObjects.StartupInfo.ProductPricingPartner.VendorPlatform == VendorPlatform.EPC2;
    }

    private void ClearItems(
      Hashtable requestData,
      bool includeExtensionFees,
      bool includeCustomPriceAdjustments,
      bool includePriceConcessions,
      bool includeReLockFees)
    {
      Action<int, int> action = (Action<int, int>) ((startIdx, endIdx) =>
      {
        for (int index = startIdx; index <= endIdx; ++index)
        {
          string key = index.ToString();
          if (requestData.Contains((object) key))
            requestData[(object) key] = (object) "";
        }
      });
      if (!includeExtensionFees)
        action(3454, 3493);
      if (!includeCustomPriceAdjustments)
        action(4336, 4375);
      if (!includeReLockFees)
        action(4256, 4295);
      if (includePriceConcessions)
        return;
      action(3371, 3378);
      if (requestData.Contains((object) "3372"))
        requestData[(object) "3372"] = (object) "//";
      if (!requestData.Contains((object) "3376"))
        return;
      requestData[(object) "3376"] = (object) "//";
    }

    public EllieMae.EMLite.DataEngine.Log.LockVoidLog CreateVoidLockRequest(
      UserInfo requestingUser,
      DateTime voidDate,
      string comments,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
      using (Tracing.StartTimer(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.CreateVoidLockRequest"))
      {
        EllieMae.EMLite.DataEngine.Log.LockRequestLog[] allLockRequests = this.loanData.GetLogList().GetAllLockRequests(true);
        bool flag = ((IEnumerable<EllieMae.EMLite.DataEngine.Log.LockRequestLog>) allLockRequests).Where<EllieMae.EMLite.DataEngine.Log.LockRequestLog>((Func<EllieMae.EMLite.DataEngine.Log.LockRequestLog, bool>) (log =>
        {
          if (log.Voided)
            return false;
          return log.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.RateLocked || log.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.OldLock || log.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.Cancelled || log.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.RequestDenied;
        })).Count<EllieMae.EMLite.DataEngine.Log.LockRequestLog>() == 1;
        EllieMae.EMLite.DataEngine.Log.LockRequestLog latestLockRequestLog = (EllieMae.EMLite.DataEngine.Log.LockRequestLog) null;
        IEnumerable<EllieMae.EMLite.DataEngine.Log.LockRequestLog> source = ((IEnumerable<EllieMae.EMLite.DataEngine.Log.LockRequestLog>) allLockRequests).Where<EllieMae.EMLite.DataEngine.Log.LockRequestLog>((Func<EllieMae.EMLite.DataEngine.Log.LockRequestLog, bool>) (requestLog =>
        {
          if (requestLog.Voided)
            return false;
          if (requestLog.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.Requested || requestLog.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.ExtensionRequested)
            return true;
          return requestLog.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.RequestDenied && requestLog.LockRequestOldStatus == EllieMae.EMLite.Common.RateLockRequestStatus.Requested;
        }));
        if (source.Count<EllieMae.EMLite.DataEngine.Log.LockRequestLog>() > 0)
          latestLockRequestLog = source.First<EllieMae.EMLite.DataEngine.Log.LockRequestLog>();
        EllieMae.EMLite.DataEngine.Log.LockRequestLog lockRequestForVoid = LockUtils.GetLastLockRequestForVoid(this);
        if (lockRequestForVoid == null)
          throw new Exception("Cannot void lock without a confirmed/cancelled/denied lock.");
        if (lockRequestForVoid.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.RequestDenied && latestLockRequestLog == null)
          latestLockRequestLog = lockRequestForVoid;
        EllieMae.EMLite.DataEngine.Log.LockConfirmLog confirmLog = (EllieMae.EMLite.DataEngine.Log.LockConfirmLog) null;
        EllieMae.EMLite.DataEngine.Log.LockVoidLog lockVoidLog = (EllieMae.EMLite.DataEngine.Log.LockVoidLog) null;
        bool isLockConfirmed = false;
        if (flag && latestLockRequestLog == null && lockRequestForVoid.LockRequestStatus != EllieMae.EMLite.Common.RateLockRequestStatus.RequestDenied)
        {
          EllieMae.EMLite.DataEngine.Log.LockRequestLog lockRequestLogForVoid;
          this.GenerateVoidRequest(requestingUser, comments, lockRequestForVoid, out lockRequestLogForVoid, out lockVoidLog);
          this.updateOldLockStatus(lockRequestLogForVoid, new List<string>()
          {
            lockRequestLogForVoid.Guid
          });
        }
        else
        {
          EllieMae.EMLite.DataEngine.Log.LockRequestLog lockRequestSourceLog = (EllieMae.EMLite.DataEngine.Log.LockRequestLog) null;
          string requestGUID = string.Empty;
          lockRequestForVoid.Voided = true;
          if (lockRequestForVoid != null && latestLockRequestLog != null)
          {
            if (!string.IsNullOrEmpty(latestLockRequestLog.ParentLockGuid))
            {
              EllieMae.EMLite.DataEngine.Log.LockRequestLog initialLockRequest = this.loanData.GetLogList().GetInitialLockRequest(this.loanData.GetLogList().GetLockRequest(latestLockRequestLog.ParentLockGuid));
              if (initialLockRequest != null)
                lockRequestSourceLog = initialLockRequest;
            }
            if (lockRequestSourceLog == null)
              lockRequestSourceLog = LockUtils.GetLastLockRequestForVoid(this);
          }
          if (lockRequestSourceLog == null)
            lockRequestSourceLog = latestLockRequestLog ?? LockUtils.GetLastLockRequestForVoid(this);
          if (lockRequestSourceLog != null)
          {
            bool isUpdate = false;
            string rateSheetId = "";
            string str = "";
            double loanAmount = 0.0;
            isLockConfirmed = this.IsPreviousLockConfirmed(latestLockRequestLog, lockRequestSourceLog);
            if (isLockConfirmed)
            {
              try
              {
                Hashtable lockRequestSnapshot = lockRequestSourceLog.GetLockRequestSnapshot();
                rateSheetId = Convert.ToString(lockRequestSnapshot[(object) "2148"]);
                str = Convert.ToString(lockRequestSnapshot[(object) "2149"]);
                string deliveryType = Convert.ToString(lockRequestSnapshot[(object) "3911"]);
                loanAmount = EllieMae.EMLite.Common.Utils.ParseDouble(lockRequestSnapshot[(object) "2965"]);
                this.ValidateBestEffortDailyLimit(ref rateSheetId, str, deliveryType, loanAmount, out isUpdate);
              }
              catch (Exception ex)
              {
                lockRequestForVoid.Voided = false;
                throw;
              }
            }
            EllieMae.EMLite.DataEngine.Log.LockRequestLog lockRequestLogForVoid;
            this.GenerateVoidRequest(requestingUser, comments, lockRequestForVoid, out lockRequestLogForVoid, out lockVoidLog);
            List<string> ignoreOldStatusGuids = new List<string>()
            {
              lockRequestLogForVoid.Guid
            };
            requestGUID = this.generateRequestAfterVoid(requestingUser, latestLockRequestLog, lockRequestSourceLog, ignoreOldStatusGuids, isLockConfirmed, syncOption);
            ignoreOldStatusGuids.Add(requestGUID);
            if (isUpdate)
            {
              string guid = this.loanData.GUID;
              string field = this.loanData.GetField("TPO.X15");
              DateTime result;
              DateTime.TryParse(str, out result);
              this.sessionObjects.ConfigurationManager.UpdateBestEffortDailyLimit(field, result, loanAmount, guid, rateSheetId);
            }
          }
          if (lockRequestSourceLog != null && lockRequestSourceLog.IsLockCancellation)
          {
            EllieMae.EMLite.DataEngine.Log.LockRequestLog lockRequest = this.loanData.GetLogList().GetLockRequest(requestGUID);
            this.CancelRateLock(lockRequest, lockRequest.GetLockRequestSnapshot(), requestingUser);
          }
          else
          {
            confirmLog = this.loanData.GetLogList().GetMostRecentConfirmForCurrentLock();
            if (confirmLog != null)
            {
              EllieMae.EMLite.DataEngine.Log.LockRequestLog lockRequest = this.loanData.GetLogList().GetLockRequest(confirmLog.RequestGUID);
              if (lockRequest != null)
              {
                Hashtable lockRequestSnapshot = lockRequest.GetLockRequestSnapshot();
                if (lockRequestSnapshot != null)
                  this.SyncLockRequestSnapshotToLoan(lockRequestSnapshot, true, lockRequest.IsLockExtension, syncOption);
              }
            }
          }
        }
        if (confirmLog == null)
          confirmLog = this.loanData.GetLogList().GetMostRecentConfirmForCurrentLock();
        this.clearLockDateInformation(lockVoidLog.Guid, lockVoidLog.Date, confirmLog, true);
        if (!isLockConfirmed && lockRequestForVoid != null && lockRequestForVoid.LockRequestStatus != EllieMae.EMLite.Common.RateLockRequestStatus.RequestDenied)
        {
          this.ResetBestEffortDailyLimit(lockRequestForVoid.GetLockRequestSnapshot());
          this.ClearLockValidationStatus(lockVoidLog.Guid);
        }
        return lockVoidLog;
      }
    }

    private void GenerateVoidRequest(
      UserInfo requestingUser,
      string comments,
      EllieMae.EMLite.DataEngine.Log.LockRequestLog lastConfirmLogToVoid,
      out EllieMae.EMLite.DataEngine.Log.LockRequestLog lockRequestLogForVoid,
      out EllieMae.EMLite.DataEngine.Log.LockVoidLog lockVoidLog)
    {
      lockRequestLogForVoid = new EllieMae.EMLite.DataEngine.Log.LockRequestLog(this.loanData.GetLogList());
      lastConfirmLogToVoid.Comments = comments;
      this.createLogRequestFromConfirmRequest(lastConfirmLogToVoid, ref lockRequestLogForVoid, false);
      lockRequestLogForVoid.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.Voided;
      this.loanData.GetLogList().AddRecord((LogRecordBase) lockRequestLogForVoid);
      lockVoidLog = new EllieMae.EMLite.DataEngine.Log.LockVoidLog();
      lockVoidLog.SetVoidingUser(requestingUser);
      DateTime serverTime = this.SessionObjects.Session.ServerTime;
      lockVoidLog.Date = serverTime;
      lockVoidLog.TimeVoided = serverTime.ToLongTimeString();
      lockVoidLog.RequestGUID = lockRequestLogForVoid.Guid;
      lockVoidLog.AlertLoanOfficer = true;
      lockVoidLog.Comments = comments;
      lockVoidLog.Voided = true;
      this.LoanData.GetLogList().AddRecord((LogRecordBase) lockVoidLog);
    }

    private string generateRequestAfterVoid(
      UserInfo requestingUser,
      EllieMae.EMLite.DataEngine.Log.LockRequestLog latestLockRequestLog,
      EllieMae.EMLite.DataEngine.Log.LockRequestLog lockRequestSourceLog,
      List<string> ignoreOldStatusGuids,
      bool isLockConfirmed,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
      EllieMae.EMLite.DataEngine.Log.LockRequestLog lockRequestLogToCopy = new EllieMae.EMLite.DataEngine.Log.LockRequestLog(this.loanData.GetLogList());
      this.createLogRequestFromConfirmRequest(lockRequestSourceLog, ref lockRequestLogToCopy, true);
      if (ignoreOldStatusGuids == null)
        ignoreOldStatusGuids = new List<string>();
      if (!ignoreOldStatusGuids.Contains(lockRequestLogToCopy.Guid))
        ignoreOldStatusGuids.Add(lockRequestLogToCopy.Guid);
      bool preserveLockRateStatus = latestLockRequestLog != null || lockRequestSourceLog.IsLockCancellation;
      this.LockRateRequest(lockRequestLogToCopy, lockRequestLogToCopy.GetLockRequestSnapshot(), requestingUser, isLockConfirmed, syncOption: syncOption, alertLoanOfficer: false, displayInLog: false, preserveLockRateStatus: preserveLockRateStatus, ignoreOldStatusGuids: ignoreOldStatusGuids, isValidateBestEffort: false);
      return lockRequestLogToCopy.Guid;
    }

    private bool IsPreviousLockConfirmed(
      EllieMae.EMLite.DataEngine.Log.LockRequestLog latestLockRequestLog,
      EllieMae.EMLite.DataEngine.Log.LockRequestLog lockRequestSourceLog)
    {
      bool flag = latestLockRequestLog == null && !lockRequestSourceLog.IsLockCancellation;
      if (latestLockRequestLog != null && latestLockRequestLog.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.RequestDenied && latestLockRequestLog.LockRequestOldStatus == EllieMae.EMLite.Common.RateLockRequestStatus.RateLocked)
        flag = true;
      else if ((lockRequestSourceLog.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.RateLocked || lockRequestSourceLog.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.OldLock) && !lockRequestSourceLog.IsLockCancellation)
        flag = true;
      return flag;
    }

    private void createLogRequestFromConfirmRequest(
      EllieMae.EMLite.DataEngine.Log.LockRequestLog confirmedLock,
      ref EllieMae.EMLite.DataEngine.Log.LockRequestLog lockRequestLogToCopy,
      bool isLatestRequest)
    {
      confirmedLock.Copy(ref lockRequestLogToCopy);
      DateTime dateTime = isLatestRequest ? this.SessionObjects.Session.ServerTime.AddSeconds(1.0) : this.SessionObjects.Session.ServerTime;
      lockRequestLogToCopy.Date = dateTime;
      if (!isLatestRequest)
        lockRequestLogToCopy.Voided = true;
      confirmedLock.Voided = true;
      if (confirmedLock.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.OldRequest)
        lockRequestLogToCopy.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.Requested;
      else if (confirmedLock.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.OldLock)
        lockRequestLogToCopy.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.RateLocked;
      else if (confirmedLock.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.RequestDenied & isLatestRequest)
        lockRequestLogToCopy.LockRequestStatus = confirmedLock.LockRequestOldStatus;
      lockRequestLogToCopy.DisplayInLog = false;
      lockRequestLogToCopy.AlertLoanOfficer = false;
      Hashtable lockRequestSnapshot = this.loanData.GetLogList().GetLockRequest(confirmedLock.Guid).GetLockRequestSnapshot();
      this.setSnapshotField(lockRequestSnapshot, (object) "2144", (object) confirmedLock.Comments);
      if (!isLatestRequest)
        this.setSnapshotField(lockRequestSnapshot, (object) "5029", (object) string.Empty);
      this.loanData.SetField("5029", string.Empty);
      lockRequestLogToCopy.AddLockRequestSnapshot(lockRequestSnapshot);
    }

    private void setSnapshotField(Hashtable snapshot, object key, object value)
    {
      if (snapshot.Contains(key))
        snapshot[key] = value;
      else
        snapshot.Add(key, value);
    }

    private object getSnapshotField(Hashtable snapshot, object key)
    {
      return snapshot.Contains(key) ? snapshot[key] : (object) null;
    }

    private void ValidateLoanAmount(
      Hashtable requestSnapshot,
      ICorrespondentTradeManager correspondentTradeManager = null)
    {
      ExternalOriginatorManagementData externalCompanyByTpoid = this.sessionObjects.ConfigurationManager.GetExternalCompanyByTPOID(false, this.loanData.GetField("TPO.X15"));
      Decimal num = 0M;
      if (externalCompanyByTpoid == null)
        return;
      ExternalOriginatorManagementData externalOrganization = this.sessionObjects.ConfigurationManager.GetExternalOrganization(false, externalCompanyByTpoid.oid);
      if (externalOrganization.CommitmentPolicy != ExternalOriginatorCommitmentPolicy.DontAllowLockorSubmit || requestSnapshot[(object) "3911"] == null)
        return;
      CorrespondentMasterDeliveryType valueFromDescription = EllieMae.EMLite.Common.Utils.GetEnumValueFromDescription<CorrespondentMasterDeliveryType>(requestSnapshot[(object) "3911"].ToString());
      Dictionary<CorrespondentTradeCommitmentType, Decimal> dictionary1 = new Dictionary<CorrespondentTradeCommitmentType, Decimal>();
      Dictionary<CorrespondentTradeCommitmentType, Decimal> dictionary2 = correspondentTradeManager != null ? this.sessionObjects.ConfigurationManager.GetCommitmentAvailableAmounts(externalOrganization, correspondentTradeManager) : this.sessionObjects.ConfigurationManager.GetCommitmentAvailableAmounts(externalOrganization, this.sessionObjects.CorrespondentTradeManager);
      if (valueFromDescription == CorrespondentMasterDeliveryType.IndividualBestEfforts)
      {
        if (dictionary2.ContainsKey(CorrespondentTradeCommitmentType.BestEfforts))
          num = dictionary2[CorrespondentTradeCommitmentType.BestEfforts];
      }
      else if (dictionary2.ContainsKey(CorrespondentTradeCommitmentType.Mandatory))
        num = dictionary2[CorrespondentTradeCommitmentType.Mandatory];
      if (num < EllieMae.EMLite.Common.Utils.ParseDecimal((object) this.loanData.GetField("1109"), 0M))
        throw new RateLockNotConfirmException(externalOrganization.CommitmentMessage);
    }

    public void LockRateRequest(
      EllieMae.EMLite.DataEngine.Log.LockRequestLog requestLog,
      Hashtable requestSnapshot,
      UserInfo confirmingUser,
      bool isConfirmed,
      bool fromAssignToTrade = false,
      bool isPersistent = true,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan,
      bool alertLoanOfficer = true,
      bool displayInLog = true,
      bool preserveLockRateStatus = false,
      List<string> ignoreOldStatusGuids = null,
      bool isValidateBestEffort = true)
    {
      this.LockRateRequest(requestLog, requestSnapshot, confirmingUser?.Userid, confirmingUser?.FullName, isConfirmed, fromAssignToTrade, isPersistent, syncOption, alertLoanOfficer, displayInLog, preserveLockRateStatus, ignoreOldStatusGuids, isValidateBestEffort);
    }

    public void LockRateRequest(
      EllieMae.EMLite.DataEngine.Log.LockRequestLog requestLog,
      Hashtable requestSnapshot,
      string confirmingUserId,
      string confirmingUserFullName,
      bool isConfirmed,
      bool fromAssignToTrade = false,
      bool isPersistent = true,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan,
      bool alertLoanOfficer = true,
      bool displayInLog = true,
      bool preserveLockRateStatus = false,
      List<string> ignoreOldStatusGuids = null,
      bool isValidateBestEffort = true)
    {
      string requestType = requestLog.RequestType;
      LoanChannel valueFromDescription = EllieMae.EMLite.Common.Utils.GetEnumValueFromDescription<LoanChannel>(this.loanData.GetField("2626"));
      bool flag = this.sessionObjects.ConfigurationManager.IsInOutstandingCommitments(this.loanData.GUID);
      if (((!(requestType == "Lock") ? 0 : (valueFromDescription == LoanChannel.Correspondent ? 1 : 0)) & (isConfirmed ? 1 : 0)) != 0 && !fromAssignToTrade && !flag)
        this.ValidateLoanAmount(requestSnapshot);
      if (!requestSnapshot.Contains((object) "5029"))
        requestSnapshot[(object) "5029"] = (object) this.loanData.GetField("5029");
      this.loanData.SetField("5029", "");
      if (LockUtils.IsRelock(this.loanData.GetField("3841")))
      {
        int result = 0;
        if (requestSnapshot.Contains((object) "2150"))
          int.TryParse(Convert.ToString(requestSnapshot[(object) "2150"]), out result);
        try
        {
          this.ValidateLockPeriodRestriction(result);
        }
        catch (LockPeriodRestrictionException ex)
        {
          throw new RateLockNotConfirmException(ex.Message);
        }
      }
      if (((!(requestType == "Lock") && !(requestType == "Extension") || valueFromDescription != LoanChannel.Correspondent ? 0 : (!fromAssignToTrade ? 1 : 0)) & (isConfirmed ? 1 : 0) & (isValidateBestEffort ? 1 : 0)) != 0)
      {
        try
        {
          this.ValidateAndUpdateBestEffortDailyLimit(requestSnapshot);
        }
        catch (OverDailyLimitRateLockRejectedException ex)
        {
          throw new RateLockNotConfirmException(ex.Message);
        }
      }
      if (!string.IsNullOrEmpty(this.loanData.GetField("4789")))
        requestLog.PriceConcessionIndicator = this.loanData.GetField("4789");
      if (!string.IsNullOrEmpty(this.loanData.GetField("4790")))
        requestLog.LockExtensionIndicator = this.loanData.GetField("4790");
      if (!string.IsNullOrEmpty(this.loanData.GetField("4791")))
        requestLog.PriceConcessionRequestStatus = this.loanData.GetField("4791");
      using (Tracing.StartTimer(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.LockRateRequest"))
      {
        requestSnapshot[(object) "2592"] = (object) this.SessionObjects.Session.ServerTime;
        if (requestLog.IsLockExtension && requestSnapshot.Contains((object) "3431") && (string) requestSnapshot[(object) "3431"] != "")
        {
          Hashtable hashtable = (Hashtable) null;
          if (requestLog.ParentLockGuid != "")
          {
            EllieMae.EMLite.DataEngine.Log.LockRequestLog lockRequest = this.LoanData.GetLogList().GetLockRequest(requestLog.ParentLockGuid);
            if (lockRequest != null)
              hashtable = lockRequest.GetLockRequestSnapshot();
          }
          if (hashtable != null && EllieMae.EMLite.Common.Utils.ParseInt(hashtable[(object) "3433"], 0) == EllieMae.EMLite.Common.Utils.ParseInt(requestSnapshot[(object) "3433"], 0))
          {
            int num = EllieMae.EMLite.Common.Utils.ParseInt(hashtable[(object) "3363"], 0);
            requestSnapshot[(object) "3431"] = (object) string.Concat((object) (EllieMae.EMLite.Common.Utils.ParseInt(requestSnapshot[(object) "3431"], 0) + (EllieMae.EMLite.Common.Utils.ParseInt(requestSnapshot[(object) "3363"], 0) - num)));
          }
          else
            requestSnapshot[(object) "3431"] = (object) string.Concat((object) (EllieMae.EMLite.Common.Utils.ParseInt(requestSnapshot[(object) "3431"], 0) - EllieMae.EMLite.Common.Utils.ParseInt(requestSnapshot[(object) "3360"], 0) + EllieMae.EMLite.Common.Utils.ParseInt(requestSnapshot[(object) "3363"], 0)));
        }
        requestLog.AddLockRequestSnapshot(requestSnapshot);
        if (!preserveLockRateStatus)
        {
          this.UpdateLockRequestLog(requestLog, requestSnapshot);
          requestLog.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.RateLocked;
        }
        if (this.loanData.GetLogList().GetLockRequest(requestLog.Guid) == null)
        {
          requestLog.DisplayInLog = false;
          this.loanData.GetLogList().AddRecord((LogRecordBase) requestLog);
        }
        this.SyncLockRequestSnapshotToLoan(requestSnapshot, isConfirmed, requestLog.IsLockExtension, syncOption);
        this.updateOldLockStatus(requestLog, ignoreOldStatusGuids);
        if (isConfirmed)
        {
          EllieMae.EMLite.DataEngine.Log.LockConfirmLog rec = new EllieMae.EMLite.DataEngine.Log.LockConfirmLog();
          rec.BuySideExpirationDate = requestLog.BuySideExpirationDate;
          rec.SetConfirmingUser(confirmingUserId, confirmingUserFullName);
          DateTime serverTime = this.SessionObjects.Session.ServerTime;
          rec.Date = EllieMae.EMLite.Common.Utils.ParseDate(requestSnapshot[(object) "2592"], serverTime);
          rec.DateConfirmed = requestSnapshot[(object) "2592"].ToString();
          rec.RequestGUID = requestLog.Guid;
          rec.SellSideDeliveryDate = requestLog.SellSideDeliveryDate;
          rec.SellSideDeliveredBy = requestLog.SellSideDeliveredBy;
          rec.SellSideExpirationDate = requestLog.SellSideExpirationDate;
          rec.AlertLoanOfficer = alertLoanOfficer;
          rec.DisplayInLog = displayInLog;
          rec.CommitmentTermEnabled = EllieMae.EMLite.Common.Utils.ParseBoolean(this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableCommitmentTermFields"]);
          rec.EnableZeroParPricingRetail = EllieMae.EMLite.Common.Utils.ParseBoolean(this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingRetail"]);
          rec.EnableZeroParPricingWholesale = EllieMae.EMLite.Common.Utils.ParseBoolean(this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingWholesale"]);
          rec.Voided = ignoreOldStatusGuids != null && ignoreOldStatusGuids.Count > 0;
          this.loanData.GetLogList().AddRecord((LogRecordBase) rec);
          if (!fromAssignToTrade)
          {
            bool isAutoCT = this.IsAutoCreationCTEnabled(requestSnapshot);
            if (((!(requestType == "Lock") ? 0 : (valueFromDescription == LoanChannel.Correspondent ? 1 : 0)) & (isAutoCT ? 1 : 0) & (isPersistent ? 1 : 0)) != 0)
            {
              CorrespondentTradeInfo correspondentTrade = CorrespondentTradeManager.CreateCorrespondentTrade(requestSnapshot, this.loanData, this.sessionObjects, isAutoCT);
              if (correspondentTrade != null)
                CorrespondentTradeManager.AssignLoanToCorrespondentTrade(correspondentTrade, this.loanData.GUID, this.sessionObjects, this, syncOption);
            }
          }
        }
        this.LoanData.TriggerCalculation("761", this.LoanData.GetField("761"));
        this.LockComparisonFieldsModified = this.sessionObjects.StartupInfo.LockComparisonFields.ToHashSet<LockComparisonField>();
        this.UpdateLockValidationStatus(requestLog);
      }
    }

    private void ValidateAndUpdateBestEffortDailyLimit(Hashtable requestSnapshot)
    {
      string rateSheetId = Convert.ToString(requestSnapshot[(object) "2148"]);
      string str = Convert.ToString(requestSnapshot[(object) "2149"]);
      string deliveryType = Convert.ToString(requestSnapshot[(object) "3911"]);
      double loanAmount = EllieMae.EMLite.Common.Utils.ParseDouble(requestSnapshot[(object) "2965"]);
      bool isUpdate;
      this.ValidateBestEffortDailyLimit(ref rateSheetId, str, deliveryType, loanAmount, out isUpdate);
      if (!isUpdate)
        return;
      string guid = this.loanData.GUID;
      string field = this.loanData.GetField("TPO.X15");
      DateTime result;
      DateTime.TryParse(str, out result);
      this.sessionObjects.ConfigurationManager.UpdateBestEffortDailyLimit(field, result, loanAmount, guid, rateSheetId);
    }

    public void updateOldLockStatus(EllieMae.EMLite.DataEngine.Log.LockRequestLog requestLog, List<string> voidGuid)
    {
      foreach (EllieMae.EMLite.DataEngine.Log.LockRequestLog allLockRequest in this.LoanData.GetLogList().GetAllLockRequests())
      {
        if (allLockRequest != requestLog && (voidGuid == null || !voidGuid.Contains(allLockRequest.Guid)))
        {
          if (allLockRequest.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.RateLocked || allLockRequest.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.Cancelled)
            allLockRequest.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.OldLock;
          else if (allLockRequest.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.Requested)
            allLockRequest.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.OldRequest;
          else if (allLockRequest.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.Voided)
            allLockRequest.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.OldVoid;
        }
      }
    }

    private bool IsAutoCreationCTEnabled(Hashtable requestSnapshot)
    {
      bool flag = false;
      if (this.loanData.GetField("2626") != "Correspondent" || !requestSnapshot.ContainsKey((object) "3911"))
        return flag;
      CorrespondentMasterDeliveryType valueFromDescription = EllieMae.EMLite.Common.Utils.GetEnumValueFromDescription<CorrespondentMasterDeliveryType>(requestSnapshot[(object) "3911"].ToString());
      switch (valueFromDescription)
      {
        case CorrespondentMasterDeliveryType.IndividualBestEfforts:
        case CorrespondentMasterDeliveryType.IndividualMandatory:
          if (EllieMae.EMLite.Common.Utils.ParseBoolean(this.sessionObjects.StartupInfo.TradeSettings[(object) "Trade.EnableCorrespondentTrade"]) && EllieMae.EMLite.Common.Utils.ParseBoolean(this.sessionObjects.StartupInfo.TradeSettings[(object) "Trade.EnableAutoCreationCT"]))
          {
            switch (valueFromDescription)
            {
              case CorrespondentMasterDeliveryType.IndividualBestEfforts:
                if (!EllieMae.EMLite.Common.Utils.ParseBoolean(this.sessionObjects.StartupInfo.TradeSettings[(object) "Trade.AllowBestEfforts"]))
                  break;
                goto case CorrespondentMasterDeliveryType.IndividualMandatory;
              case CorrespondentMasterDeliveryType.IndividualMandatory:
                flag = true;
                break;
            }
          }
          return flag;
        default:
          return false;
      }
    }

    public void DenyRateRequest(
      EllieMae.EMLite.DataEngine.Log.LockRequestLog requestLog,
      Hashtable requestSnapshot,
      UserInfo denyingUser,
      string comments)
    {
      using (Tracing.StartTimer(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.DenyRateRequest"))
      {
        EllieMae.EMLite.DataEngine.Log.LockConfirmLog confirmForCurrentLock = this.loanData.GetLogList().GetMostRecentConfirmForCurrentLock();
        if (!requestLog.IsLockExtension && requestLog.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.NoRequest)
        {
          for (int index = 2219; index <= 2292; ++index)
            requestSnapshot[(object) index.ToString()] = (object) "";
          for (int index = 2482; index <= 2515; ++index)
            requestSnapshot[(object) index.ToString()] = (object) "";
          for (int index = 2733; index <= 2818; ++index)
            requestSnapshot[(object) index.ToString()] = (object) "";
          requestSnapshot[(object) "2205"] = (object) "";
          requestSnapshot[(object) "2030"] = (object) "";
          requestSnapshot[(object) "2295"] = (object) "";
          requestSnapshot[(object) "2297"] = (object) "";
          requestSnapshot[(object) "3123"] = (object) "";
          for (int index = 2148; index <= 2205; ++index)
            requestSnapshot[(object) index.ToString()] = (object) "";
          for (int index = 2448; index <= 2481; ++index)
            requestSnapshot[(object) index.ToString()] = (object) "";
          requestSnapshot[(object) "2215"] = (object) "";
        }
        if (requestSnapshot.ContainsKey((object) "5029"))
          requestSnapshot[(object) "5029"] = (object) string.Empty;
        this.loanData.SetField("5029", string.Empty);
        if (requestLog.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.RateLocked)
          this.ResetBestEffortDailyLimit(requestSnapshot);
        requestSnapshot[(object) "2592"] = (object) this.SessionObjects.Session.ServerTime;
        requestSnapshot[(object) "2204"] = (object) comments;
        requestLog.AddLockRequestSnapshot(requestSnapshot);
        requestLog.LockRequestOldStatus = requestLog.LockRequestStatus;
        requestLog.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.RequestDenied;
        if (this.loanData.GetLogList().GetLockRequest(requestLog.Guid) == null)
        {
          requestLog.DisplayInLog = false;
          this.loanData.GetLogList().AddRecord((LogRecordBase) requestLog);
        }
        EllieMae.EMLite.DataEngine.Log.LockDenialLog rec = new EllieMae.EMLite.DataEngine.Log.LockDenialLog();
        rec.SetDenyingUser(denyingUser);
        DateTime serverTime = this.SessionObjects.Session.ServerTime;
        rec.Date = serverTime;
        rec.TimeDenied = serverTime.ToLongTimeString();
        rec.RequestGUID = requestLog.Guid;
        rec.AlertLoanOfficer = true;
        rec.Comments = comments;
        this.loanData.GetLogList().AddRecord((LogRecordBase) rec);
        this.clearLockDateInformation(requestLog.Guid, requestLog.Date, confirmForCurrentLock, false);
        if (requestLog.LockRequestOldStatus != EllieMae.EMLite.Common.RateLockRequestStatus.RateLocked)
          return;
        this.ClearLockValidationStatus(rec.Guid);
      }
    }

    private void clearLockDateInformation(
      string requestedGuid,
      DateTime requestLogDate,
      EllieMae.EMLite.DataEngine.Log.LockConfirmLog confirmLog,
      bool checkForVoid)
    {
      if (this.LoanData.GetLogList().GetLastConfirmedLock(checkForVoid) == null)
      {
        bool flag = false;
        if (confirmLog != null)
          flag = confirmLog.CommitmentTermEnabled;
        if (string.Equals(this.loanData.GetField("2626"), "correspondent", StringComparison.InvariantCultureIgnoreCase) & flag)
        {
          this.loanData.SetField("4532", "N");
          this.LoanData.SetCurrentField("4527", "");
          this.LoanData.SetCurrentField("4528", "");
          this.LoanData.SetCurrentField("4529", "");
        }
        else
        {
          this.LoanData.SetCurrentField("761", "");
          this.LoanData.SetCurrentField("432", "");
          this.LoanData.SetCurrentField("762", "");
          this.LoanData.SetCurrentField("2400", "");
        }
      }
      EllieMae.EMLite.DataEngine.Log.LockConfirmLog lockConfirmLog = (EllieMae.EMLite.DataEngine.Log.LockConfirmLog) null;
      foreach (EllieMae.EMLite.DataEngine.Log.LockConfirmLog allConfirmLock in this.loanData.GetLogList().GetAllConfirmLocks())
        lockConfirmLog = allConfirmLock;
      if (lockConfirmLog != null && lockConfirmLog.RequestGUID == requestedGuid)
        this.loanData.SetCurrentField("3941", "N");
      this.LoanData.TriggerCalculation("761", this.LoanData.GetField("761"));
      if (!this.LoanData.Use2010RESPA && !this.LoanData.Use2015RESPA)
        return;
      this.LoanData.TriggerCalculation("2400", this.LoanData.GetField("2400"));
    }

    public void CancelRateLockWithoutRequest(
      UserInfo cancellingUser,
      DateTime cancellationDate,
      string comment)
    {
      string cancellationParentGuid = this.getLockCancellationParentGuid();
      if (string.IsNullOrWhiteSpace(cancellationParentGuid))
        return;
      EllieMae.EMLite.DataEngine.Log.LockRequestLog lockRequestLog = new EllieMae.EMLite.DataEngine.Log.LockRequestLog(this.loanData.GetLogList());
      lockRequestLog.Date = this.SessionObjects.Session.ServerTime;
      lockRequestLog.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.Requested;
      lockRequestLog.IsLockCancellation = true;
      lockRequestLog.SetRequestingUser(cancellingUser.Userid, cancellingUser.FullName);
      lockRequestLog.DisplayInLog = false;
      lockRequestLog.ParentLockGuid = cancellationParentGuid;
      lockRequestLog.ReLockSequenceNumberForInactiveLock = LockUtils.GetReLockSequenceNumberForInactiveLock(this.loanData);
      Hashtable hashtable = this.loanData.PrepareLockCancellationData(this.loanData.GetLogList().GetLockRequest(cancellationParentGuid).GetLockRequestSnapshot());
      this.setSnapshotField(hashtable, (object) "2144", (object) comment);
      this.setSnapshotField(hashtable, (object) "4209", (object) LockUtils.GetRequestLockStatus(this.loanData));
      this.setSnapshotField(hashtable, (object) "5029", (object) string.Empty);
      this.loanData.SetField("5029", string.Empty);
      this.LoanData.GetLogList().AddRecord((LogRecordBase) lockRequestLog);
      this.CancelRateLock(lockRequestLog, hashtable, cancellingUser);
    }

    public EllieMae.EMLite.DataEngine.Log.LockRequestLog CancelRateLockWithoutRequest(
      UserInfo cancellingUser,
      DateTime cancellationDate,
      string comment,
      bool fromPlatform = false)
    {
      string cancellationParentGuid = this.getLockCancellationParentGuid();
      if (string.IsNullOrWhiteSpace(cancellationParentGuid))
        return (EllieMae.EMLite.DataEngine.Log.LockRequestLog) null;
      EllieMae.EMLite.DataEngine.Log.LockRequestLog lockRequestLog = new EllieMae.EMLite.DataEngine.Log.LockRequestLog(this.loanData.GetLogList());
      lockRequestLog.Date = cancellationDate;
      lockRequestLog.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.Requested;
      lockRequestLog.IsLockCancellation = true;
      lockRequestLog.SetRequestingUser(cancellingUser.Userid, cancellingUser.FullName);
      lockRequestLog.DisplayInLog = false;
      lockRequestLog.ParentLockGuid = cancellationParentGuid;
      lockRequestLog.ReLockSequenceNumberForInactiveLock = LockUtils.GetReLockSequenceNumberForInactiveLock(this.loanData);
      Hashtable lockRequestSnapshot = this.loanData.GetLogList().GetLockRequest(cancellationParentGuid).GetLockRequestSnapshot();
      if (fromPlatform)
        this.loanData.SetField("3433", "0");
      Hashtable hashtable = this.loanData.PrepareLockCancellationData(lockRequestSnapshot);
      this.setSnapshotField(hashtable, (object) "2144", (object) comment);
      this.setSnapshotField(hashtable, (object) "4209", (object) LockUtils.GetRequestLockStatus(this.loanData));
      this.setSnapshotField(hashtable, (object) "5029", (object) string.Empty);
      this.loanData.SetField("5029", string.Empty);
      this.LoanData.GetLogList().AddRecord((LogRecordBase) lockRequestLog);
      this.CancelRateLock(lockRequestLog, hashtable, cancellingUser);
      return fromPlatform ? lockRequestLog : (EllieMae.EMLite.DataEngine.Log.LockRequestLog) null;
    }

    private string getLockCancellationParentGuid()
    {
      EllieMae.EMLite.DataEngine.Log.LockRequestLog confirmedLockRequest = this.loanData.GetLogList().GetCurrentConfirmedLockRequest();
      string guid;
      if (confirmedLockRequest == null)
      {
        EllieMae.EMLite.DataEngine.Log.LockRequestLog confirmationLock = LockUtils.GetAssignToTradePostConfirmationLock(this);
        if (confirmationLock == null)
          return "";
        guid = confirmationLock.Guid;
      }
      else
        guid = confirmedLockRequest.Guid;
      return guid;
    }

    public void CancelRateLock(
      EllieMae.EMLite.DataEngine.Log.LockRequestLog requestLog,
      Hashtable requestSnapshot,
      UserInfo cancellingUser)
    {
      using (Tracing.StartTimer(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.CancelRateRequest"))
      {
        EllieMae.EMLite.DataEngine.Log.LockConfirmLog confirmForCurrentLock = this.loanData.GetLogList().GetMostRecentConfirmForCurrentLock();
        this.loanData.SetField("5029", string.Empty);
        requestSnapshot[(object) "2592"] = (object) this.SessionObjects.Session.ServerTime;
        requestLog.AddLockRequestSnapshot(requestSnapshot);
        requestLog.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.Cancelled;
        this.loanData.GetLogList().AddRecord((LogRecordBase) requestLog);
        foreach (string currentLockField in EllieMae.EMLite.DataEngine.Log.LockRequestLog.CurrentLockFields)
          this.loanData.SetCurrentField(currentLockField, "");
        foreach (string buySideField in EllieMae.EMLite.DataEngine.Log.LockRequestLog.BuySideFields)
          this.loanData.SetCurrentField(buySideField, "");
        foreach (string sellSideField in EllieMae.EMLite.DataEngine.Log.LockRequestLog.SellSideFields)
          this.loanData.SetCurrentField(sellSideField, "");
        foreach (EllieMae.EMLite.DataEngine.Log.LockRequestLog allLockRequest in this.LoanData.GetLogList().GetAllLockRequests())
        {
          if (allLockRequest != requestLog)
          {
            if (allLockRequest.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.RateLocked || allLockRequest.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.Cancelled)
              allLockRequest.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.OldLock;
            else if (allLockRequest.LockRequestStatus == EllieMae.EMLite.Common.RateLockRequestStatus.Requested)
              allLockRequest.LockRequestStatus = EllieMae.EMLite.Common.RateLockRequestStatus.OldRequest;
          }
        }
        EllieMae.EMLite.DataEngine.Log.LockCancellationLog rec = new EllieMae.EMLite.DataEngine.Log.LockCancellationLog();
        rec.SetCancellingUser(cancellingUser);
        DateTime serverTime = this.SessionObjects.Session.ServerTime;
        rec.Date = serverTime;
        rec.TimeCancelled = serverTime.ToLongTimeString();
        rec.RequestGUID = requestLog.Guid;
        rec.AlertLoanOfficer = true;
        this.loanData.GetLogList().AddRecord((LogRecordBase) rec);
        this.loanData.SetCurrentField("3941", "N");
        if (string.Equals(this.loanData.GetField("2626"), "correspondent", StringComparison.InvariantCultureIgnoreCase) && confirmForCurrentLock != null && confirmForCurrentLock.CommitmentTermEnabled)
        {
          this.LoanData.SetCurrentField("4527", "");
          this.LoanData.SetCurrentField("4528", "");
          this.LoanData.SetCurrentField("4529", "");
          this.loanData.SetCurrentField("4532", "N");
        }
        else
        {
          this.LoanData.SetCurrentField("761", "");
          this.LoanData.SetCurrentField("432", "");
          this.LoanData.SetCurrentField("762", "");
          this.LoanData.SetCurrentField("2400", "");
        }
        if (this.LoanData.GetField("2626") == "Correspondent" && requestLog.IsLockCancellation)
        {
          this.LoanData.SetCurrentField("4207", rec.Date.ToShortDateString());
          this.loanData.SetCurrentField("4120", "");
          this.LoanData.SetCurrentField("4208", "");
        }
        this.LoanData.TriggerCalculation("761", this.LoanData.GetField("761"));
        if (this.LoanData.Use2010RESPA || this.LoanData.Use2015RESPA)
          this.LoanData.TriggerCalculation("2400", this.LoanData.GetField("2400"));
        this.LoanData.SetCurrentField("3841", "ReLock");
        this.loanData.SetCurrentField("4209", "Cancelled Lock");
        if (confirmForCurrentLock != null)
        {
          EllieMae.EMLite.DataEngine.Log.LockRequestLog lockRequest = this.loanData.GetLogList().GetLockRequest(confirmForCurrentLock.RequestGUID);
          if (lockRequest != null)
            this.ResetBestEffortDailyLimit(lockRequest.GetLockRequestSnapshot());
        }
        this.ClearLockValidationStatus(rec.Guid);
      }
    }

    public void SyncLockRequestSnapshotToLoan(
      Hashtable fields,
      bool isConfirmed,
      bool isExtension,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Copying lock request snapshot to loan...");
      string empty = string.Empty;
      for (int index = 0; index < EllieMae.EMLite.DataEngine.Log.LockRequestLog.CurrentLockFields.Count; ++index)
      {
        int num1 = EllieMae.EMLite.Common.Utils.ParseInt((object) EllieMae.EMLite.DataEngine.Log.LockRequestLog.CurrentLockFields[index]);
        int num2;
        if (num1 >= 2036 && num1 <= 2044)
          num2 = num1 + 116;
        else if (num1 >= 2516 && num1 <= 2549)
          num2 = num1 - 68;
        else if (num1 == 2045)
          num2 = 3420;
        else if (num1 >= 2046 && num1 <= 2087)
          num2 = num1 + 116;
        else if (num1 >= 3434 && num1 <= 3453)
          num2 = num1 + 40;
        else if (num1 >= 4416 && num1 <= 4435)
          num2 = num1 - 140;
        else if (num1 >= 4436 && num1 <= 4455)
          num2 = num1 - 80;
        else if (num1 >= 2690 && num1 <= 2732)
        {
          num2 = num1 + 43;
        }
        else
        {
          switch (num1)
          {
            case 2034:
              num2 = 2148;
              break;
            case 2035:
              num2 = 2204;
              break;
            case 2145:
              num2 = 2149;
              break;
            case 2146:
              num2 = 2150;
              break;
            case 2147:
              num2 = 2151;
              break;
            case 3255:
              num2 = 3256;
              break;
            default:
              num2 = num1;
              break;
          }
        }
        this.syncSnapshotFieldToLoan(fields, num1.ToString(), num2.ToString());
      }
      for (int index = 0; index < EllieMae.EMLite.DataEngine.Log.LockRequestLog.BuySideFields.Count; ++index)
      {
        string buySideField = EllieMae.EMLite.DataEngine.Log.LockRequestLog.BuySideFields[index];
        this.syncSnapshotFieldToLoan(fields, buySideField, buySideField);
      }
      for (int index = 0; index < EllieMae.EMLite.DataEngine.Log.LockRequestLog.SellSideFields.Count; ++index)
      {
        string sellSideField = EllieMae.EMLite.DataEngine.Log.LockRequestLog.SellSideFields[index];
        this.syncSnapshotFieldToLoan(fields, sellSideField, sellSideField);
      }
      for (int index = 0; index < EllieMae.EMLite.DataEngine.Log.LockRequestLog.CompSideFields.Count; ++index)
      {
        string compSideField = EllieMae.EMLite.DataEngine.Log.LockRequestLog.CompSideFields[index];
        this.syncSnapshotFieldToLoan(fields, compSideField, compSideField);
      }
      this.syncSnapshotFieldToLoan(fields, "2012", "2297");
      this.syncSnapshotFieldToLoan(fields, "2013", "2206");
      this.syncSnapshotFieldToLoan(fields, "2207", "2274");
      this.syncSnapshotFieldToLoan(fields, "2209", "2276");
      this.syncSnapshotFieldToLoan(fields, "996", "2286");
      if (fields.Contains((object) "2288") && (string) fields[(object) "2288"] != "")
        this.syncSnapshotFieldToLoan(fields, "352", "2288");
      this.syncSnapshotFieldToLoan(fields, "VEND.X276", "2288");
      this.syncSnapshotFieldToLoan(fields, "VEND.X263", "2278");
      if (fields.Contains((object) "2278") && string.Equals(((string) fields[(object) "2278"]).Trim(), ""))
        this.LoanData.TriggerCalculation("VEND.X263", "");
      this.syncSnapshotFieldToLoan(fields, "VEND.X264", "2281");
      this.syncSnapshotFieldToLoan(fields, "VEND.X265", "2282");
      this.syncSnapshotFieldToLoan(fields, "VEND.X266", "2283");
      this.syncSnapshotFieldToLoan(fields, "VEND.X267", "2284");
      this.syncSnapshotFieldToLoan(fields, "VEND.X271", "2279");
      this.syncSnapshotFieldToLoan(fields, "VEND.X272", "2280");
      this.syncSnapshotFieldToLoan(fields, "VEND.X273", "3055");
      if (syncOption == LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
        this.syncSnapshotFieldToLoan(fields, "VEND.X178", "3535");
      this.syncSnapshotFieldToLoan(fields, "4021", "3890");
      this.syncSnapshotFieldToLoan(fields, "4020", "4019");
      if (string.Equals(this.loanData.GetField("2626"), "correspondent", StringComparison.InvariantCultureIgnoreCase) && EllieMae.EMLite.Common.Utils.ParseBoolean(this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableCommitmentTermFields"]))
      {
        if (!string.IsNullOrEmpty(this.loanData.GetField("TPO.X14")) & isConfirmed)
          this.loanData.SetField("4532", "Y");
        else
          this.loanData.SetField("4532", "N");
      }
      else
      {
        this.loanData.SetField("2400", "Y");
        Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Verbose, "SyncLockRequestSnapshotToLoan: target ID - 2400   Value - Y");
      }
      if (!isConfirmed)
      {
        if ((!string.Equals(this.loanData.GetField("2626"), "correspondent", StringComparison.InvariantCultureIgnoreCase) || !EllieMae.EMLite.Common.Utils.ParseBoolean(this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableCommitmentTermFields"])) && (this.LoanData.Use2010RESPA || this.LoanData.Use2015RESPA))
          this.LoanData.TriggerCalculation("2400", "Y");
        if (fields.ContainsKey((object) "3364"))
        {
          if (string.IsNullOrEmpty(fields[(object) "3364"].ToString()) || EllieMae.EMLite.Common.Utils.ParseDate(fields[(object) "3364"], DateTime.MinValue) == DateTime.MinValue)
            this.loanData.SetCurrentField("3364", "");
        }
        else
          this.loanData.SetCurrentField("3364", "");
        if (string.Equals(this.loanData.GetField("2626"), "correspondent", StringComparison.InvariantCultureIgnoreCase) && EllieMae.EMLite.Common.Utils.ParseBoolean(this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableCommitmentTermFields"]))
        {
          this.loanData.SetCurrentField("4527", "");
          this.loanData.SetCurrentField("4528", "");
          this.loanData.SetCurrentField("4529", "");
        }
        if (this.loanData.Calculator == null)
          return;
        this.loanData.Calculator.FormCalculation("PurchaseAdvice", (string) null, (string) null);
      }
      else
      {
        this.syncSnapshotFieldToLoan(fields, "4105", "3910");
        if (string.Equals(this.loanData.GetField("2626"), "correspondent", StringComparison.InvariantCultureIgnoreCase) && EllieMae.EMLite.Common.Utils.ParseBoolean(this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableCommitmentTermFields"]))
          this.loanData.SetCurrentField("3941", "N");
        else
          this.loanData.SetCurrentField("3941", "Y");
        if (fields.ContainsKey((object) "2278") && !string.IsNullOrEmpty(fields[(object) "2278"].ToString()) && fields.ContainsKey((object) "3263"))
        {
          string str = fields[(object) "3263"].ToString();
          if (str != string.Empty)
          {
            try
            {
              InvestorTemplate templateSettings = (InvestorTemplate) this.sessionObjects.ConfigurationManager.GetTemplateSettings(TemplateSettingsType.Investor, new FileSystemEntry("\\" + str, FileSystemEntry.Types.File, (string) null));
              if (templateSettings != null)
                this.ApplyInvestorToLoan(templateSettings.CompanyInformation, (ContactInformation) null, false);
            }
            catch (Exception ex)
            {
              Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Error, "Cannot apply investor template " + str + " to loan! Error: " + ex.Message);
            }
          }
        }
        this.syncLockRequestAdditionalFields(fields, syncOption);
        if (this.loanData != null && fields.ContainsKey((object) "2967") && syncOption == LoanDataMgr.LockLoanSyncOption.syncLockToLoan && fields.ContainsKey((object) "2866"))
        {
          if (this.loanData.GetField("1401").ToLower() != fields[(object) "2866"].ToString().ToLower())
          {
            try
            {
              LoanProgram templateSettings = (LoanProgram) this.sessionObjects.ConfigurationManager.GetTemplateSettings(TemplateSettingsType.LoanProgram, FileSystemEntry.Parse(fields[(object) "2967"].ToString()));
              if (templateSettings.GetField("LP101") == "0")
                templateSettings.SetField("LP101", "");
              if (templateSettings.GetField("LP104") == "0")
                templateSettings.SetField("LP104", "");
              if (templateSettings.GetField("LP103") == "0")
                templateSettings.SetField("LP103", "");
              Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Verbose, "SyncLockRequestSnapshotToLoan: Applying Loan Program - " + fields[(object) "2967"].ToString());
              this.loanData.SelectLoanProgram(templateSettings, (HelocRateTable) null, true);
              if (templateSettings.GetField("2861") != "")
                templateSettings.SetField("2861", "");
            }
            catch (Exception ex)
            {
              Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Error, "Cannot apply loan program " + fields.ContainsKey((object) "2967").ToString() + "! Error: " + ex.Message);
            }
          }
        }
        bool useNewCalc = false;
        if (fields.ContainsKey((object) "3043") && fields[(object) "3043"].ToString() != "")
          useNewCalc = true;
        this.syncRequestFieldMapFields(fields, useNewCalc, syncOption);
        if (!useNewCalc && this.loanData.GetField("1107") != string.Empty && fields.ContainsKey((object) "2965") && EllieMae.EMLite.Common.Utils.ParseDouble(fields[(object) "2965"]) != EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("2")))
        {
          double num = EllieMae.EMLite.Common.Utils.ArithmeticRounding(EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("1109")) + EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("1760")), 0);
          this.loanData.SetFieldFromCal("1109", num.ToString("N2"));
          Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Verbose, "SyncLockRequestSnapshotToLoan: Target Field - 1109  Value - " + num.ToString("N2"));
        }
        if (syncOption == LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
        {
          this.syncSnapshotFieldToLoan(fields, "VASUMM.X23", "2853");
          this.syncSnapshotFieldToLoan(fields, "689", "2775");
        }
        this.loanData.SetCurrentField("KBYO.XD689", EllieMae.EMLite.Common.Utils.RemoveEndingZeros(this.loanData.GetField("689")));
        this.syncBuyDownFields(fields);
        string str1 = "761";
        string str2 = "432";
        string targetID = "762";
        if (string.Equals(this.loanData.GetField("2626"), "correspondent", StringComparison.InvariantCultureIgnoreCase) && EllieMae.EMLite.Common.Utils.ParseBoolean(this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableCommitmentTermFields"]))
        {
          str1 = "4527";
          str2 = "4528";
          targetID = "4529";
        }
        if (this.loanData.GetField(str1) != fields[(object) "2149"].ToString())
          this.syncSnapshotFieldToLoan(fields, str1, "2149");
        if (!isExtension)
        {
          this.syncSnapshotFieldToLoan(fields, str2, "2150");
          this.syncSnapshotFieldToLoan(fields, targetID, "2151");
          if (fields.ContainsKey((object) "3364"))
          {
            if (string.IsNullOrEmpty(fields[(object) "3364"].ToString()) || EllieMae.EMLite.Common.Utils.ParseDate(fields[(object) "3364"], DateTime.MinValue) == DateTime.MinValue)
              this.loanData.SetCurrentField("3364", "");
          }
          else
            this.loanData.SetCurrentField("3364", "");
        }
        else
        {
          int num3 = EllieMae.EMLite.Common.Utils.ParseInt(fields[(object) "2150"], 0);
          int num4 = EllieMae.EMLite.Common.Utils.ParseInt(fields[(object) "3363"], 0);
          int num5 = EllieMae.EMLite.Common.Utils.ParseInt(fields[(object) "3431"], 0);
          if (num5 > 0)
          {
            this.loanData.SetCurrentField(str2, string.Concat((object) (num3 + num5)));
            this.LoanData.SetCurrentField("2146", string.Concat((object) (num3 + num5)));
          }
          else
          {
            this.loanData.SetCurrentField(str2, string.Concat((object) (num3 + num4)));
            this.LoanData.SetCurrentField("2146", string.Concat((object) (num3 + num4)));
          }
          this.syncSnapshotFieldToLoan(fields, targetID, "3364");
          this.syncSnapshotFieldToLoan(fields, "2147", "3364");
          this.syncSnapshotFieldToLoan(fields, "3364", "3364");
        }
        this.syncSnapshotFieldToLoan(fields, "3253", "3256");
        this.syncSnapshotFieldToLoan(fields, "996", "2286");
        if (fields.Contains((object) "2288") && (string) fields[(object) "2288"] != "")
          this.syncSnapshotFieldToLoan(fields, "352", "2288");
        if (syncOption == LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
          this.syncSnapshotFieldToLoan(fields, "3", "2160", false);
        this.syncSnapshotFieldToLoan(fields, "3293", "3848", false);
        this.syncSnapshotFieldToLoan(fields, "NEWHUD.X1720", "3873", false);
        this.syncSnapshotFieldToLoan(fields, "3907", "3902");
        this.syncSnapshotFieldToLoan(fields, "3908", "3903");
        this.syncSnapshotFieldToLoan(fields, "3923", "2148");
        this.syncSnapshotFieldToLoan(fields, "3967", "3911");
        this.syncSnapshotFieldToLoan(fields, "3924", "3420");
        if (!this.loanData.IsLocked("3926"))
          this.syncSnapshotFieldToLoan(fields, "3926", "3913");
        this.syncSnapshotFieldToLoan(fields, "4057", "4059");
        if (!string.IsNullOrWhiteSpace(this.loanData.GetField("3875")))
        {
          if (!LockUtils.GetZeroBasedParPricingSetting(this.sessionObjects, this.loanData))
            this.loanData.SetField("NEWHUD.X1721", (100M - EllieMae.EMLite.Common.Utils.ParseDecimal((object) this.loanData.GetField("3875"))).ToString());
          else
            this.syncSnapshotFieldToLoan(fields, "NEWHUD.X1721", "3875");
        }
        else
          this.loanData.SetField("NEWHUD.X1721", "");
        if (this.loanData.Calculator != null)
          this.loanData.Calculator.FormCalculation("PurchaseAdvice", (string) null, (string) null);
        if (this.LoanData.Use2010RESPA || this.LoanData.Use2015RESPA)
          this.LoanData.TriggerCalculation("2400", "Y");
        if (this.loanData.Calculator == null)
          return;
        if (fields.Contains((object) "2092"))
          this.loanData.Calculator.FormCalculation("3");
        this.loanData.Calculator.CalculateAll();
      }
    }

    private void syncLockRequestAdditionalFields(
      Hashtable fields,
      LoanDataMgr.LockLoanSyncOption syncOption)
    {
      if (this.loanData == null || syncOption != LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
        return;
      string[] fields1 = this.loanData.Settings.FieldSettings.LockRequestAdditionalFields.GetFields(true);
      if (fields1 == null || fields1.Length == 0)
        return;
      for (int index = 0; index < fields1.Length; ++index)
      {
        string customFieldId = LockRequestCustomField.GenerateCustomFieldID(fields1[index]);
        this.syncSnapshotFieldToLoan(fields, fields1[index], customFieldId);
        if (this.IsFromPlatform)
          this.syncSnapshotFieldToLoan(fields, customFieldId, customFieldId);
        this.LoanData.SetField(customFieldId, this.LoanData.GetField(fields1[index]));
      }
    }

    private void syncRequestFieldMapFields(
      Hashtable fields,
      bool useNewCalc,
      LoanDataMgr.LockLoanSyncOption syncOption)
    {
      if (syncOption != LoanDataMgr.LockLoanSyncOption.syncLockToLoan || fields == null)
        return;
      for (int index = 0; index < EllieMae.EMLite.DataEngine.Log.LockRequestLog.RequestFieldMap.Count; ++index)
      {
        KeyValuePair<string, string> requestField;
        if (!useNewCalc)
        {
          requestField = EllieMae.EMLite.DataEngine.Log.LockRequestLog.RequestFieldMap[index];
          if (!(requestField.Value == "1109"))
          {
            requestField = EllieMae.EMLite.DataEngine.Log.LockRequestLog.RequestFieldMap[index];
            if (!(requestField.Value == "1107"))
            {
              requestField = EllieMae.EMLite.DataEngine.Log.LockRequestLog.RequestFieldMap[index];
              if (!(requestField.Value == "1826"))
              {
                requestField = EllieMae.EMLite.DataEngine.Log.LockRequestLog.RequestFieldMap[index];
                if (!(requestField.Value == "1765"))
                {
                  requestField = EllieMae.EMLite.DataEngine.Log.LockRequestLog.RequestFieldMap[index];
                  if (requestField.Value == "1760")
                    continue;
                }
                else
                  continue;
              }
              else
                continue;
            }
            else
              continue;
          }
          else
            continue;
        }
        requestField = EllieMae.EMLite.DataEngine.Log.LockRequestLog.RequestFieldMap[index];
        if (!(requestField.Value == "353"))
        {
          requestField = EllieMae.EMLite.DataEngine.Log.LockRequestLog.RequestFieldMap[index];
          if (!(requestField.Value == "976"))
          {
            requestField = EllieMae.EMLite.DataEngine.Log.LockRequestLog.RequestFieldMap[index];
            if (requestField.Key == "4463")
            {
              Hashtable hashtable1 = fields;
              requestField = EllieMae.EMLite.DataEngine.Log.LockRequestLog.RequestFieldMap[index];
              string key1 = requestField.Key;
              if (hashtable1[(object) key1] != null)
              {
                Hashtable hashtable2 = fields;
                requestField = EllieMae.EMLite.DataEngine.Log.LockRequestLog.RequestFieldMap[index];
                string key2 = requestField.Key;
                if (string.IsNullOrEmpty(hashtable2[(object) key2].ToString()))
                  continue;
              }
              else
                continue;
            }
            Hashtable fields1 = fields;
            requestField = EllieMae.EMLite.DataEngine.Log.LockRequestLog.RequestFieldMap[index];
            string targetID = requestField.Value;
            requestField = EllieMae.EMLite.DataEngine.Log.LockRequestLog.RequestFieldMap[index];
            string key3 = requestField.Key;
            this.syncSnapshotFieldToLoan(fields1, targetID, key3);
            if (this.IsFromPlatform)
            {
              Hashtable fields2 = fields;
              requestField = EllieMae.EMLite.DataEngine.Log.LockRequestLog.RequestFieldMap[index];
              string key4 = requestField.Key;
              requestField = EllieMae.EMLite.DataEngine.Log.LockRequestLog.RequestFieldMap[index];
              string key5 = requestField.Key;
              this.syncSnapshotFieldToLoan(fields2, key4, key5);
            }
          }
        }
      }
    }

    private void syncBuyDownFields(Hashtable fields)
    {
      if (!fields.ContainsKey((object) "4631"))
        return;
      if (fields[(object) "4631"].Equals((object) "Borrower"))
      {
        for (int index = 1; index <= 2; ++index)
        {
          int num1 = index == 1 ? 1269 : 1613;
          int num2 = index == 1 ? 4633 : 4639;
          int num3 = index == 1 ? 1274 : 1618;
          int num4 = num1;
          while (num4 <= num3)
          {
            string val = fields.Contains((object) num2.ToString()) ? fields[(object) num2.ToString()].ToString() : "";
            if (val != "")
            {
              if (!this.loanData.IsLocked(string.Concat((object) num4)))
                this.loanData.AddLock(string.Concat((object) num4));
              this.loanData.SetCurrentField(string.Concat((object) num4), val);
            }
            else if (this.loanData.IsLocked(string.Concat((object) num4)))
              this.loanData.RemoveLock(string.Concat((object) num4));
            ++num4;
            ++num2;
          }
        }
      }
      else
      {
        int num5 = 4633;
        int num6 = 4535;
        while (num5 < 4645)
        {
          this.syncSnapshotFieldToLoan(fields, string.Concat((object) num6), string.Concat((object) num5));
          ++num5;
          ++num6;
        }
      }
      for (int index = 4633; index < 4645; ++index)
      {
        if (this.loanData.IsLocked(string.Concat((object) index)))
          this.loanData.RemoveLock(string.Concat((object) index));
        this.syncSnapshotFieldToLoan(fields, string.Concat((object) index), string.Concat((object) index));
      }
    }

    public void SyncSellComparisonToLoan(
      Hashtable fields,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Copying lock request snapshot to loan...");
      string empty = string.Empty;
      for (int index = 0; index < EllieMae.EMLite.DataEngine.Log.LockRequestLog.SellSideFields.Count; ++index)
      {
        string sellSideField = EllieMae.EMLite.DataEngine.Log.LockRequestLog.SellSideFields[index];
        this.syncSnapshotFieldToLoan(fields, sellSideField, sellSideField);
      }
      for (int index = 0; index < EllieMae.EMLite.DataEngine.Log.LockRequestLog.CompSideFields.Count; ++index)
      {
        string compSideField = EllieMae.EMLite.DataEngine.Log.LockRequestLog.CompSideFields[index];
        this.syncSnapshotFieldToLoan(fields, compSideField, compSideField);
      }
      this.syncSnapshotFieldToLoan(fields, "2012", "2297");
      this.syncSnapshotFieldToLoan(fields, "2013", "2206");
      this.syncSnapshotFieldToLoan(fields, "2207", "2274");
      this.syncSnapshotFieldToLoan(fields, "2209", "2276");
      this.syncSnapshotFieldToLoan(fields, "996", "2286");
      if (fields.Contains((object) "2288") && (string) fields[(object) "2288"] != "")
        this.syncSnapshotFieldToLoan(fields, "352", "2288");
      this.syncSnapshotFieldToLoan(fields, "VEND.X276", "2288");
      this.syncSnapshotFieldToLoan(fields, "VEND.X263", "2278");
      if (fields.Contains((object) "2278") && string.Equals(((string) fields[(object) "2278"]).Trim(), ""))
        this.LoanData.TriggerCalculation("VEND.X263", "");
      this.syncSnapshotFieldToLoan(fields, "VEND.X264", "2281");
      this.syncSnapshotFieldToLoan(fields, "VEND.X265", "2282");
      this.syncSnapshotFieldToLoan(fields, "VEND.X266", "2283");
      this.syncSnapshotFieldToLoan(fields, "VEND.X267", "2284");
      this.syncSnapshotFieldToLoan(fields, "VEND.X271", "2279");
      this.syncSnapshotFieldToLoan(fields, "VEND.X272", "2280");
      this.syncSnapshotFieldToLoan(fields, "VEND.X273", "3055");
      if (syncOption == LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
        this.syncSnapshotFieldToLoan(fields, "VEND.X178", "3535");
      this.syncSnapshotFieldToLoan(fields, "4021", "3890");
      this.syncSnapshotFieldToLoan(fields, "4020", "4019");
      if ((!string.Equals(this.loanData.GetField("2626"), "correspondent", StringComparison.InvariantCultureIgnoreCase) || !EllieMae.EMLite.Common.Utils.ParseBoolean(this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableCommitmentTermFields"])) && (this.LoanData.Use2010RESPA || this.LoanData.Use2015RESPA))
        this.LoanData.TriggerCalculation("2400", "Y");
      if (this.loanData.Calculator != null)
        this.loanData.Calculator.FormCalculation("PurchaseAdvice", (string) null, (string) null);
      if (fields.ContainsKey((object) "2278") && !string.IsNullOrEmpty(fields[(object) "2278"].ToString()) && fields.ContainsKey((object) "3263"))
      {
        string str = fields[(object) "3263"].ToString();
        if (str != string.Empty)
        {
          try
          {
            InvestorTemplate templateSettings = (InvestorTemplate) this.sessionObjects.ConfigurationManager.GetTemplateSettings(TemplateSettingsType.Investor, new FileSystemEntry("\\" + str, FileSystemEntry.Types.File, (string) null));
            if (templateSettings != null)
              this.ApplyInvestorToLoan(templateSettings.CompanyInformation, (ContactInformation) null, false);
          }
          catch (Exception ex)
          {
            Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Error, "Cannot apply investor template " + str + " to loan! Error: " + ex.Message);
          }
        }
      }
      this.syncSnapshotFieldToLoan(fields, "996", "2286");
      if (!fields.Contains((object) "2288") || !((string) fields[(object) "2288"] != ""))
        return;
      this.syncSnapshotFieldToLoan(fields, "352", "2288");
    }

    private void syncSnapshotFieldToLoan(Hashtable fields, string targetID, string sourceID)
    {
      this.syncSnapshotFieldToLoan(fields, targetID, sourceID, true);
    }

    private void syncSnapshotFieldToLoan(
      Hashtable fields,
      string targetID,
      string sourceID,
      bool cleared)
    {
      if (fields.ContainsKey((object) sourceID))
      {
        FieldDefinition fieldDefinition = this.loanData.GetFieldDefinition(sourceID);
        FieldFormat format = FieldFormat.STRING;
        if (fieldDefinition != null)
          format = fieldDefinition.Format;
        string str = EllieMae.EMLite.Common.Utils.UnformatValue(fields[(object) sourceID].ToString(), format);
        if (targetID == "2")
        {
          if (EllieMae.EMLite.Common.Utils.ParseDouble((object) fields[(object) sourceID].ToString()) == EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("2")))
            return;
          Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Verbose, "syncSnapshotFieldToLoan: source ID - " + sourceID + "  target ID - " + targetID + "  Value - " + str);
          this.loanData.SetCurrentField(targetID, str);
          if (this.loanData.Calculator == null)
            return;
          this.loanData.Calculator.SpecialCalculation(CalculationActionID.UpdateTotalLoanAmountFromLock);
        }
        else
        {
          if (this.loanData.IsLocked(targetID))
            return;
          Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Verbose, "syncSnapshotFieldToLoan: source ID - " + sourceID + "  target ID - " + targetID + "  Value - " + str);
          if (fieldDefinition.Format == FieldFormat.ZIPCODE)
          {
            bool needsUpdate = false;
            str = EllieMae.EMLite.Common.Utils.FormatInput(str, FieldFormat.ZIPCODE, ref needsUpdate);
          }
          this.loanData.SetCurrentField(targetID, str);
        }
      }
      else
      {
        if (!cleared)
          return;
        Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Verbose, "syncSnapshotFieldToLoan: target ID - " + targetID + "  Value - Blank");
        this.loanData.SetCurrentField(targetID, "");
      }
    }

    public ServicingTransactionBase CreateNextServicingTransaction(
      ServicingTransactionTypes transType)
    {
      int num1 = 0;
      int num2 = 0;
      ServicingTransactionBase[] servicingTransactions = this.loanData.GetServicingTransactions(true);
      if (servicingTransactions != null)
      {
        foreach (ServicingTransactionBase servicingTransactionBase in servicingTransactions)
        {
          ++num1;
          if (servicingTransactionBase.TransactionType == transType)
            ++num2;
        }
      }
      ServicingTransactionBase servicingTransaction;
      switch (transType)
      {
        case ServicingTransactionTypes.Payment:
          servicingTransaction = (ServicingTransactionBase) new PaymentTransactionLog();
          break;
        case ServicingTransactionTypes.PaymentReversal:
          servicingTransaction = (ServicingTransactionBase) new PaymentReversalLog();
          break;
        case ServicingTransactionTypes.EscrowDisbursement:
          servicingTransaction = (ServicingTransactionBase) new EscrowDisbursementLog();
          break;
        case ServicingTransactionTypes.EscrowInterest:
          servicingTransaction = (ServicingTransactionBase) new EscrowInterestLog();
          break;
        case ServicingTransactionTypes.EscrowDisbursementReversal:
          servicingTransaction = (ServicingTransactionBase) new PaymentReversalLog();
          break;
        case ServicingTransactionTypes.Other:
          servicingTransaction = (ServicingTransactionBase) new OtherTransactionLog();
          break;
        default:
          throw new ArgumentException("Invalid transaction type specified");
      }
      servicingTransaction.TransactionType = transType;
      servicingTransaction.TransactionNo = num1 + 1;
      switch (servicingTransaction)
      {
        case PaymentTransactionLog _:
          ((PaymentTransactionLog) servicingTransaction).PaymentNo = num2 + 1;
          break;
        case EscrowDisbursementLog _:
          ((EscrowDisbursementLog) servicingTransaction).DisbursementNo = num2 + 1;
          break;
      }
      servicingTransaction.CreatedByID = this.sessionObjects.UserID;
      servicingTransaction.CreatedByName = this.sessionObjects.UserInfo.FullName;
      servicingTransaction.CreatedDateTime = DateTime.Now;
      return servicingTransaction;
    }

    public void PopulateNextServicingPaymentInformation(PaymentTransactionLog nextPayment)
    {
      nextPayment.PaymentIndexDate = EllieMae.EMLite.Common.Utils.ParseDate((object) this.loanData.GetField("SERVICE.X99"));
      nextPayment.StatementDate = EllieMae.EMLite.Common.Utils.ParseDate((object) this.loanData.GetField("SERVICE.X13"));
      nextPayment.PaymentDueDate = EllieMae.EMLite.Common.Utils.ParseDate((object) this.loanData.GetField("SERVICE.X14"));
      nextPayment.LatePaymentDate = EllieMae.EMLite.Common.Utils.ParseDate((object) this.loanData.GetField("SERVICE.X15"));
      nextPayment.PaymentReceivedDate = DateTime.Today;
      nextPayment.IndexRate = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X16"));
      nextPayment.InterestRate = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X17"));
      if (EllieMae.EMLite.Common.Utils.ToDouble(this.loanData.GetField("SERVICE.X91")) < 0.0)
        this.loanData.SetField("SERVICE.X91", "0");
      if (EllieMae.EMLite.Common.Utils.ToDouble(this.loanData.GetField("SERVICE.X92")) < 0.0)
        this.loanData.SetField("SERVICE.X92", "0");
      if (EllieMae.EMLite.Common.Utils.ToDouble(this.loanData.GetField("SERVICE.X93")) < 0.0)
        this.loanData.SetField("SERVICE.X93", "0");
      if (EllieMae.EMLite.Common.Utils.ToDouble(this.loanData.GetField("SERVICE.X104")) < 0.0)
        this.loanData.SetField("SERVICE.X104", "0");
      if (EllieMae.EMLite.Common.Utils.ToDouble(this.loanData.GetField("SERVICE.X112")) < 0.0)
        this.loanData.SetField("SERVICE.X112", "0");
      if (EllieMae.EMLite.Common.Utils.ToDouble(this.loanData.GetField("SERVICE.X113")) < 0.0)
        this.loanData.SetField("SERVICE.X113", "0");
      if (EllieMae.EMLite.Common.Utils.ToDouble(this.loanData.GetField("SERVICE.X114")) < 0.0)
        this.loanData.SetField("SERVICE.X114", "0");
      if (EllieMae.EMLite.Common.Utils.ToDouble(this.loanData.GetField("SERVICE.X115")) < 0.0)
        this.loanData.SetField("SERVICE.X115", "0");
      if (EllieMae.EMLite.Common.Utils.ToDouble(this.loanData.GetField("SERVICE.X116")) < 0.0)
        this.loanData.SetField("SERVICE.X116", "0");
      if (EllieMae.EMLite.Common.Utils.ToDouble(this.loanData.GetField("SERVICE.X117")) < 0.0)
        this.loanData.SetField("SERVICE.X117", "0");
      if (EllieMae.EMLite.Common.Utils.ToDouble(this.loanData.GetField("SERVICE.X118")) < 0.0)
        this.loanData.SetField("SERVICE.X118", "0");
      if (EllieMae.EMLite.Common.Utils.ToDouble(this.loanData.GetField("SERVICE.X119")) < 0.0)
        this.loanData.SetField("SERVICE.X119", "0");
      if (EllieMae.EMLite.Common.Utils.ToDouble(this.loanData.GetField("SERVICE.X120")) < 0.0)
        this.loanData.SetField("SERVICE.X120", "0");
      nextPayment.Principal = EllieMae.EMLite.Common.Utils.ArithmeticRounding(EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X18")), 2) + EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X91"));
      nextPayment.Interest = EllieMae.EMLite.Common.Utils.ArithmeticRounding(EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X19")), 2) + EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X92"));
      nextPayment.Escrow = EllieMae.EMLite.Common.Utils.ArithmeticRounding(EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X20")), 2) + EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X93"));
      nextPayment.LateFee = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X22"));
      nextPayment.BuydownSubsidyAmount = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X100")) + EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X104"));
      nextPayment.EscowTaxes = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X130")) + EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X112"));
      nextPayment.MortgageInsurance = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X131")) + EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X114"));
      nextPayment.HazardInsurance = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X132")) + EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X113"));
      nextPayment.FloodInsurance = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X133")) + EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X115"));
      nextPayment.CityPropertyTax = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X134")) + EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X116"));
      nextPayment.Other1Escrow = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X135")) + EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X117"));
      nextPayment.Other2Escrow = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X136")) + EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X119"));
      nextPayment.Other3Escrow = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X137")) + EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X120"));
      nextPayment.USDAMonthlyPremium = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X138")) + EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X118"));
      if (this.loanData.Calculator.GetISWCutoffDate() > nextPayment.LatePaymentDate)
        nextPayment.LateFee += EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X25"));
      nextPayment.MiscFee = EllieMae.EMLite.Common.Utils.ArithmeticRounding(EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X94")), 2);
      if (nextPayment.PaymentNo == 0)
        nextPayment.MiscFee = EllieMae.EMLite.Common.Utils.ArithmeticRounding(EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X23")), 2);
      nextPayment.PaymentMethod = ServicingPaymentMethods.None;
      ServicingTransactionBase[] servicingTransactions = this.loanData.GetServicingTransactions(true);
      if (servicingTransactions != null && servicingTransactions.Length != 0)
      {
        Hashtable hashtable = new Hashtable();
        for (int index = 0; index < servicingTransactions.Length; ++index)
        {
          ServicingTransactionBase servicingTransactionBase = servicingTransactions[index];
          if (servicingTransactionBase is PaymentReversalLog)
          {
            PaymentReversalLog paymentReversalLog = (PaymentReversalLog) servicingTransactionBase;
            hashtable.Add((object) paymentReversalLog.PaymentGUID, (object) null);
          }
        }
        for (int index = servicingTransactions.Length - 1; index >= 0; --index)
        {
          ServicingTransactionBase servicingTransactionBase = servicingTransactions[index];
          if (servicingTransactionBase is PaymentTransactionLog)
          {
            PaymentTransactionLog paymentTransactionLog = (PaymentTransactionLog) servicingTransactionBase;
            if (nextPayment.PaymentMethod == ServicingPaymentMethods.None)
            {
              nextPayment.InstitutionName = paymentTransactionLog.InstitutionName;
              nextPayment.InstitutionRouting = paymentTransactionLog.InstitutionRouting;
              nextPayment.AccountHolder = paymentTransactionLog.AccountHolder;
              nextPayment.AccountNumber = paymentTransactionLog.AccountNumber;
              nextPayment.Reference = paymentTransactionLog.Reference;
              nextPayment.PaymentMethod = paymentTransactionLog.PaymentMethod;
            }
            if (paymentTransactionLog.PaymentIndexDate == nextPayment.PaymentIndexDate && !hashtable.ContainsKey((object) paymentTransactionLog.TransactionGUID))
            {
              if (!this.loanData.IsLocked("SERVICE.X18"))
                nextPayment.Principal = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X91"));
              if (!this.loanData.IsLocked("SERVICE.X19"))
                nextPayment.Interest = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X92"));
              if (!this.loanData.IsLocked("SERVICE.X23"))
                nextPayment.MiscFee = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X94"));
              nextPayment.Escrow = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X93")) > 0.0 ? EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X93")) : 0.0;
              nextPayment.EscowTaxes = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X130")) > 0.0 ? EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X130")) : 0.0;
              nextPayment.MortgageInsurance = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X131")) > 0.0 ? EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X131")) : 0.0;
              nextPayment.HazardInsurance = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X132")) > 0.0 ? EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X132")) : 0.0;
              nextPayment.FloodInsurance = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X133")) > 0.0 ? EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X133")) : 0.0;
              nextPayment.CityPropertyTax = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X134")) > 0.0 ? EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X134")) : 0.0;
              nextPayment.Other1Escrow = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X135")) > 0.0 ? EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X135")) : 0.0;
              nextPayment.Other2Escrow = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X136")) > 0.0 ? EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X136")) : 0.0;
              nextPayment.Other3Escrow = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X137")) > 0.0 ? EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X137")) : 0.0;
              nextPayment.USDAMonthlyPremium = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X138")) > 0.0 ? EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X138")) : 0.0;
              if (!this.loanData.IsLocked("SERVICE.X22"))
                nextPayment.LateFee = EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X22"));
            }
          }
        }
      }
      if (nextPayment.Principal < 0.0)
        nextPayment.Principal = EllieMae.EMLite.Common.Utils.ArithmeticRounding(EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X18")), 2) + EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetField("SERVICE.X91"));
      if (nextPayment.Interest < 0.0)
        nextPayment.Interest = 0.0;
      if (nextPayment.Escrow < 0.0)
        nextPayment.Escrow = 0.0;
      if (nextPayment.MiscFee < 0.0)
        nextPayment.MiscFee = 0.0;
      if (nextPayment.LateFee < 0.0)
        nextPayment.LateFee = 0.0;
      nextPayment.TotalAmountReceived = nextPayment.Principal + nextPayment.Interest + nextPayment.Escrow + nextPayment.MiscFee + nextPayment.AdditionalPrincipal + nextPayment.AdditionalEscrow + nextPayment.LateFee + nextPayment.BuydownSubsidyAmount;
      nextPayment.TotalAmountDue = nextPayment.TotalAmountReceived;
      if (nextPayment.PaymentMethod != ServicingPaymentMethods.None)
        return;
      nextPayment.PaymentMethod = ServicingPaymentMethods.Check;
    }

    public bool VerifyAssignFirstMilestoneRole()
    {
      RolesMappingInfo[] usersRoleMapping = this.sessionObjects.BpmManager.GetUsersRoleMapping(this.sessionObjects.UserID);
      if (usersRoleMapping == null || usersRoleMapping.Length == 0)
        return false;
      RolesMappingInfo rolesMappingInfo1 = (RolesMappingInfo) null;
      foreach (RolesMappingInfo rolesMappingInfo2 in usersRoleMapping)
      {
        if (rolesMappingInfo2.RealWorldRoleID == RealWorldRoleID.LoanOfficer)
        {
          rolesMappingInfo1 = rolesMappingInfo2;
          break;
        }
      }
      EllieMae.EMLite.DataEngine.Log.MilestoneLog milestoneAt = this.loanData.GetLogList().GetMilestoneAt(1);
      if (rolesMappingInfo1 == null || milestoneAt.RoleID != rolesMappingInfo1.RoleIDList[0])
        return false;
      milestoneAt.SetLoanAssociate(this.sessionObjects.UserInfo);
      if ((this.sessionObjects.UserInfo.Fax ?? "") == string.Empty)
      {
        OrgInfo displayOrganization = this.configInfo.DisplayOrganization;
        if (displayOrganization != null)
          milestoneAt.LoanAssociateFax = displayOrganization.CompanyFax;
        else
          milestoneAt.LoanAssociateFax = string.Empty;
      }
      return true;
    }

    public BorrowerPair CreateBorrowerPair()
    {
      BorrowerPair borrowerPair = this.loanData.CreateBorrowerPair();
      this.loanData.SetBorrowerPair(borrowerPair);
      if (this.loanData.LinkedData != null)
        this.loanData.SyncPiggyBackFiles(this.SystemConfiguration.PiggybackSyncFields.GetSyncFields(), false, true, (string) null, (string) null, false);
      return borrowerPair;
    }

    public void DeleteBorrowerPair(BorrowerPair p)
    {
      this.DeleteBorrowerPairs(new BorrowerPair[1]{ p });
    }

    public void DeleteBorrowerPairs(BorrowerPair[] pairs)
    {
      bool flag = false;
      for (int index = 0; index < pairs.Length; ++index)
      {
        BorrowerPair pair = pairs[index];
        if (pair.Borrower.Id == this.loanData.CurrentBorrowerPair.Borrower.Id)
          flag = true;
        this.loanData.RemoveBorrowerPair(pair);
        if (this.LinkedLoan != null)
          this.LinkedLoan.LoanData.RemoveBorrowerPair(pair);
      }
      string[] syncFields = this.SystemConfiguration.PiggybackSyncFields.GetSyncFields();
      if (this.loanData.LinkedData != null)
        this.loanData.SyncPiggyBackFiles(syncFields, false, true, (string) null, (string) null, false);
      if (!flag)
        return;
      this.loanData.SetBorrowerPair(this.loanData.GetBorrowerPairs()[0]);
      if (this.loanData.LinkedData == null)
        return;
      this.loanData.SyncPiggyBackFiles(syncFields, false, true, (string) null, (string) null, false);
    }

    public void AddOperationLog(LogRecordBase rec)
    {
      this.loanData.GetLogList().AddRecord(rec, false);
      if (this.loan == null)
        return;
      LoanEventLogList loanEventLog = new LoanEventLogList();
      loanEventLog.InsertNonSystemLog(rec);
      this.loan.AddLoanEventLog(loanEventLog);
    }

    public static event EventHandler ReplaceTemplateExisting;

    private static void replaceTemplate(
      SessionObjects sessionObjects,
      MilestoneTemplate selectedTemplate,
      string loanTemplate,
      bool lockTemplate,
      bool createHistory,
      LoanData loanData,
      bool overRide)
    {
      LogList logList = loanData.GetLogList();
      IEnumerable<EllieMae.EMLite.Workflow.Milestone> milestones = sessionObjects.BpmManager.GetMilestones(false);
      RoleInfo[] allRoleFunctions = sessionObjects.BpmManager.GetAllRoleFunctions();
      if (!overRide && logList.MilestoneTemplate != null && logList.MilestoneTemplate.Equals(selectedTemplate))
        return;
      if (createHistory)
      {
        string str = "Template was manually changed because the Loan Template " + (loanTemplate != null ? loanTemplate.Substring(1) : "") + " was applied.";
        KeyValuePair<MilestoneTemplate, string> sender = new KeyValuePair<MilestoneTemplate, string>(selectedTemplate, str);
        if (LoanDataMgr.ReplaceTemplateExisting == null)
          return;
        LoanDataMgr.ReplaceTemplateExisting((object) sender, EventArgs.Empty);
      }
      else
      {
        logList.RemoveMilestone(0);
        logList.MilestoneTemplate = selectedTemplate;
        MilestoneTemplate.TemplateMilestones sequentialMilestones = selectedTemplate.SequentialMilestones;
        DateTime now = DateTime.Now;
        UserInfo userInfo = sessionObjects.UserInfo;
        DateTime date = new DateTime();
        foreach (TemplateMilestone templateMilestone in sequentialMilestones)
        {
          TemplateMilestone ms = templateMilestone;
          EllieMae.EMLite.Workflow.Milestone milestone = milestones.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (x => x.MilestoneID == ms.MilestoneID));
          EllieMae.EMLite.DataEngine.Log.MilestoneLog milestoneLog = logList.AddMilestone(milestone.Name, ms.DaysToComplete, ms.MilestoneID, ms.SortIndex, milestone.TPOConnectStatus, milestone.ConsumerStatus);
          if (string.Compare(milestone.Name, "Started", true) == 0)
          {
            milestoneLog.RoleID = 0;
            milestoneLog.RoleName = "File Starter";
            milestoneLog.Done = true;
            date = milestoneLog.Date = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            milestoneLog.Reviewed = true;
            milestoneLog.SetLoanAssociate(userInfo);
            milestoneLog.RoleRequired = "N";
          }
          else if (milestone.RoleID > RoleInfo.FileStarter.ID)
          {
            milestoneLog.RoleID = milestone.RoleID;
            RoleInfo roleInfo = ((IEnumerable<RoleInfo>) allRoleFunctions).FirstOrDefault<RoleInfo>((Func<RoleInfo, bool>) (x => x.RoleID == milestone.RoleID));
            if (roleInfo != null)
              milestoneLog.RoleName = roleInfo.RoleName;
            milestoneLog.RoleRequired = milestone.RoleRequired ? "Y" : "N";
            date = milestoneLog.Date = LoanDataMgr.AddDays(date, milestoneLog.Days, sessionObjects);
          }
          else
            date = milestoneLog.Date = LoanDataMgr.AddDays(date, milestoneLog.Days, sessionObjects);
        }
        foreach (EllieMae.EMLite.DataEngine.Log.MilestoneFreeRoleLog milestoneFreeRole in logList.GetAllMilestoneFreeRoles())
          logList.RemoveRecord((LogRecordBase) milestoneFreeRole);
        foreach (TemplateFreeRole templateFreeRole in selectedTemplate.FreeRoles.ToList<TemplateFreeRole>())
        {
          TemplateFreeRole fr = templateFreeRole;
          RoleInfo roleInfo = ((IEnumerable<RoleInfo>) allRoleFunctions).FirstOrDefault<RoleInfo>((Func<RoleInfo, bool>) (x => x.RoleID == fr.RoleID));
          if (roleInfo != null)
          {
            EllieMae.EMLite.DataEngine.Log.MilestoneFreeRoleLog rec = new EllieMae.EMLite.DataEngine.Log.MilestoneFreeRoleLog();
            rec.RoleID = fr.RoleID;
            rec.RoleName = roleInfo.RoleName;
            rec.MarkAsClean();
            logList.AddRecord((LogRecordBase) rec, false);
          }
        }
        logList.MSLock = lockTemplate;
        logList.ReAssignCustomMileStones();
        logList.ReAssignTasksMilestones();
      }
    }

    private void loanData_LogRecordAdded(object source, LogRecordEventArgs e)
    {
      if (e.LogRecord is EllieMae.EMLite.DataEngine.Log.DocumentLog)
      {
        this.LinkDocument(e.LogRecord as EllieMae.EMLite.DataEngine.Log.DocumentLog);
      }
      else
      {
        if (!(e.LogRecord is EllieMae.EMLite.DataEngine.Log.ConditionLog))
          return;
        this.LinkCondition(e.LogRecord as EllieMae.EMLite.DataEngine.Log.ConditionLog);
      }
    }

    private void loanData_LogRecordRemoved(object source, LogRecordEventArgs e)
    {
      if (!(e.LogRecord is EllieMae.EMLite.DataEngine.Log.DocumentLog))
        return;
      this.FileAttachments.Remove(e.LogRecord as EllieMae.EMLite.DataEngine.Log.DocumentLog);
    }

    public void SetEnhancedConditionTemplates()
    {
      this.enhancedConditionTemplates = new EnhancedConditionsRestClient(this).GetEnhancedConditionTemplates(true);
    }

    public void LinkDocument(EllieMae.EMLite.DataEngine.Log.DocumentLog doc)
    {
      DocumentTemplate byName1 = this.configInfo.DocumentTrackingSetup.GetByName(doc.Title);
      if (byName1 == null)
      {
        doc.Conditions.Clear(false);
      }
      else
      {
        List<EllieMae.EMLite.DataEngine.Log.ConditionLog> conditionLogList = new List<EllieMae.EMLite.DataEngine.Log.ConditionLog>();
        EllieMae.EMLite.DataEngine.Log.ConditionLog[] allConditions = this.loanData.GetLogList().GetAllConditions(false);
        if (this.LoanData.EnableEnhancedConditions)
        {
          if (this.enhancedConditionTemplates == null)
            this.SetEnhancedConditionTemplates();
          foreach (EllieMae.EMLite.DataEngine.Log.ConditionLog conditionLog in allConditions)
          {
            foreach (EnhancedConditionTemplate conditionTemplate in this.enhancedConditionTemplates)
            {
              if (conditionLog.Title == conditionTemplate.Title && conditionTemplate.AssignedTo != null)
              {
                foreach (EntityReferenceContract referenceContract in conditionTemplate.AssignedTo)
                {
                  if (byName1.Guid == referenceContract.entityId)
                    conditionLogList.Add(conditionLog);
                }
              }
            }
          }
        }
        else
        {
          foreach (EllieMae.EMLite.DataEngine.Log.ConditionLog conditionLog in allConditions)
          {
            ConditionTrackingSetup conditionTrackingSetup = (ConditionTrackingSetup) null;
            switch (conditionLog.ConditionType)
            {
              case ConditionType.Underwriting:
                conditionTrackingSetup = (ConditionTrackingSetup) this.configInfo.UnderwritingConditionTrackingSetup;
                break;
              case ConditionType.PostClosing:
                conditionTrackingSetup = (ConditionTrackingSetup) this.configInfo.PostClosingConditionTrackingSetup;
                break;
              case ConditionType.Preliminary:
                conditionTrackingSetup = (ConditionTrackingSetup) this.configInfo.UnderwritingConditionTrackingSetup;
                break;
              case ConditionType.Sell:
                conditionTrackingSetup = (ConditionTrackingSetup) this.configInfo.SellConditionTrackingSetup;
                break;
            }
            if (conditionTrackingSetup != null)
            {
              ConditionTemplate byName2 = conditionTrackingSetup.GetByName(conditionLog.Title);
              if (byName2 != null && byName2.ContainsDocument(byName1))
                conditionLogList.Add(conditionLog);
            }
          }
        }
        doc.Conditions.Replace(conditionLogList.ToArray());
      }
    }

    public void LinkCondition(EllieMae.EMLite.DataEngine.Log.ConditionLog cond)
    {
      ConditionTrackingSetup conditionTrackingSetup = (ConditionTrackingSetup) null;
      switch (cond.ConditionType)
      {
        case ConditionType.Underwriting:
          conditionTrackingSetup = (ConditionTrackingSetup) this.configInfo.UnderwritingConditionTrackingSetup;
          break;
        case ConditionType.PostClosing:
          conditionTrackingSetup = (ConditionTrackingSetup) this.configInfo.PostClosingConditionTrackingSetup;
          break;
        case ConditionType.Preliminary:
          conditionTrackingSetup = (ConditionTrackingSetup) this.configInfo.UnderwritingConditionTrackingSetup;
          break;
        case ConditionType.Sell:
          conditionTrackingSetup = (ConditionTrackingSetup) this.configInfo.SellConditionTrackingSetup;
          break;
      }
      if (conditionTrackingSetup == null)
        return;
      ConditionTemplate byName1 = conditionTrackingSetup.GetByName(cond.Title);
      if (byName1 != null)
      {
        foreach (EllieMae.EMLite.DataEngine.Log.DocumentLog allDocument in this.loanData.GetLogList().GetAllDocuments(false))
        {
          DocumentTemplate byName2 = this.configInfo.DocumentTrackingSetup.GetByName(allDocument.Title);
          if (byName2 != null && byName1.ContainsDocument(byName2))
            allDocument.Conditions.Add(cond);
          else
            allDocument.Conditions.Remove(cond);
        }
      }
      else
      {
        foreach (EllieMae.EMLite.DataEngine.Log.DocumentLog allDocument in this.loanData.GetLogList().GetAllDocuments(false))
          allDocument.Conditions.Remove(cond);
      }
    }

    public void AssignLoanAssociate(LoanAssociateLog associateLog, AclGroup group)
    {
      List<RealWorldRoleID> realWorldRoleIdList = new List<RealWorldRoleID>();
      realWorldRoleIdList.Add(RealWorldRoleID.None);
      RoleInfo roleById = this.getRoleByID(associateLog.RoleID);
      if (roleById != null)
        realWorldRoleIdList = this.GetRealWorldRoleID(roleById.RoleID);
      foreach (RealWorldRoleID realWorldRole in realWorldRoleIdList)
      {
        switch (realWorldRole)
        {
          case RealWorldRoleID.LoanOfficer:
            this.clearRealWorldRole(associateLog, realWorldRole, this.GetCurrentLOInLoanData());
            continue;
          case RealWorldRoleID.LoanProcessor:
            this.clearRealWorldRole(associateLog, realWorldRole, this.GetCurrentLPInLoanData());
            continue;
          case RealWorldRoleID.LoanCloser:
            this.clearRealWorldRole(associateLog, realWorldRole, this.GetCurrentCLInLoanData());
            continue;
          case RealWorldRoleID.Underwriter:
            this.clearRealWorldRole(associateLog, realWorldRole, this.GetCurrentUWInLoanData());
            continue;
          default:
            continue;
        }
      }
      associateLog.SetLoanAssociate(group);
    }

    public void AssignLoanAssociate(LoanAssociateLog associateLog, UserInfo user)
    {
      this.AssignLoanAssociate(associateLog, user, (ResolveLoanAssociateHandler) null);
    }

    public void AssignLoanAssociate(
      LoanAssociateLog associateLog,
      UserInfo user,
      ResolveLoanAssociateHandler conflictResolutionHandler)
    {
      List<RealWorldRoleID> realWorldRoleIdList = new List<RealWorldRoleID>();
      realWorldRoleIdList.Add(RealWorldRoleID.None);
      RoleInfo roleById = this.getRoleByID(associateLog.RoleID);
      if (roleById != null)
        realWorldRoleIdList = this.GetRealWorldRoleID(roleById.RoleID);
      if (realWorldRoleIdList.Contains(RealWorldRoleID.LoanOfficer) && !this.ValidateLOLicense(user))
        throw new Exception("The specified user is not licensed to originate loans in the specified state or the license expiration is expired.");
      EllieMae.EMLite.DataEngine.Log.MilestoneLog milestoneLog = associateLog as EllieMae.EMLite.DataEngine.Log.MilestoneLog;
      if (associateLog.LoanAssociateType != LoanAssociateType.User || associateLog.LoanAssociateID != user.Userid)
      {
        associateLog.SetLoanAssociate(user);
        if (milestoneLog != null && !milestoneLog.Done)
          milestoneLog.Reviewed = false;
      }
      if ((user.Phone ?? "") == "" || (user.Fax ?? "") == "")
      {
        OrgInfo avaliableOrganization = this.sessionObjects.OrganizationManager.GetFirstAvaliableOrganization(user.OrgId);
        if (avaliableOrganization != null)
        {
          if ((user.Phone ?? "") == "")
            associateLog.LoanAssociatePhone = avaliableOrganization.CompanyPhone;
          if ((user.Fax ?? "") == "")
            associateLog.LoanAssociateFax = avaliableOrganization.CompanyFax;
        }
      }
      foreach (RealWorldRoleID realWorldRole in realWorldRoleIdList)
      {
        switch (realWorldRole)
        {
          case RealWorldRoleID.LoanOfficer:
            this.assignRealWorldRole(realWorldRole, user, this.GetCurrentLOInLoanData(), conflictResolutionHandler);
            continue;
          case RealWorldRoleID.LoanProcessor:
            this.assignRealWorldRole(realWorldRole, user, this.GetCurrentLPInLoanData(), conflictResolutionHandler);
            continue;
          case RealWorldRoleID.LoanCloser:
            this.assignRealWorldRole(realWorldRole, user, this.GetCurrentCLInLoanData(), conflictResolutionHandler);
            continue;
          case RealWorldRoleID.Underwriter:
            this.assignRealWorldRole(realWorldRole, user, this.GetCurrentUWInLoanData(), conflictResolutionHandler);
            continue;
          default:
            continue;
        }
      }
      if (milestoneLog == null || !(milestoneLog.Stage == "Funding"))
        return;
      this.SetLoanFunder(user);
    }

    public void ClearLoanAssociate(LoanAssociateLog associateLog)
    {
      List<RealWorldRoleID> realWorldRoleIdList = new List<RealWorldRoleID>();
      realWorldRoleIdList.Add(RealWorldRoleID.None);
      RoleInfo roleById = this.getRoleByID(associateLog.RoleID);
      if (roleById != null)
        realWorldRoleIdList = this.GetRealWorldRoleID(roleById.RoleID);
      foreach (RealWorldRoleID realWorldRole in realWorldRoleIdList)
      {
        switch (realWorldRole)
        {
          case RealWorldRoleID.LoanOfficer:
            this.clearRealWorldRole(associateLog, realWorldRole, this.GetCurrentLOInLoanData());
            continue;
          case RealWorldRoleID.LoanProcessor:
            this.clearRealWorldRole(associateLog, realWorldRole, this.GetCurrentLPInLoanData());
            continue;
          case RealWorldRoleID.LoanCloser:
            this.clearRealWorldRole(associateLog, realWorldRole, this.GetCurrentCLInLoanData());
            continue;
          case RealWorldRoleID.Underwriter:
            this.clearRealWorldRole(associateLog, realWorldRole, this.GetCurrentUWInLoanData());
            continue;
          default:
            continue;
        }
      }
      associateLog.ClearLoanAssociate();
    }

    public bool ValidateLOLicense(UserInfo user)
    {
      DateTime dateTime;
      if (user.NMLSExpirationDate != DateTime.MaxValue)
      {
        DateTime date1 = DateTime.Today.Date;
        ref DateTime local = ref date1;
        dateTime = user.NMLSExpirationDate;
        DateTime date2 = dateTime.Date;
        if (local.CompareTo(date2) > 0)
          return false;
      }
      string field = this.loanData.GetField("14");
      if ((field ?? "") != "")
      {
        LOLicenseInfo loLicense = this.sessionObjects.OrganizationManager.GetLOLicense(user.Userid, field);
        if (loLicense != null && loLicense.Enabled)
        {
          if (loLicense.ExpirationDate != DateTime.MaxValue)
          {
            DateTime date3 = DateTime.Today.Date;
            ref DateTime local = ref date3;
            dateTime = loLicense.ExpirationDate;
            DateTime date4 = dateTime.Date;
            if (local.CompareTo(date4) <= 0)
              goto label_8;
          }
          else
            goto label_8;
        }
        return false;
      }
label_8:
      return true;
    }

    public List<RealWorldRoleID> GetRealWorldRoleID(int roleId)
    {
      List<RealWorldRoleID> realWorldRoleId = new List<RealWorldRoleID>();
      foreach (RolesMappingInfo roleMapping in this.SystemConfiguration.RoleMappings)
      {
        if (roleMapping.RoleIDList[0] == roleId)
          realWorldRoleId.Add(roleMapping.RealWorldRoleID);
      }
      return realWorldRoleId;
    }

    private void clearRealWorldRole(
      LoanAssociateLog associateLog,
      RealWorldRoleID realWorldRole,
      string currentUserID)
    {
      if (currentUserID == "" || associateLog.LoanAssociateType != LoanAssociateType.User || associateLog.LoanAssociateID != currentUserID)
        return;
      foreach (LoanAssociateLog allLoanAssociate in this.LoanData.GetLogList().GetAllLoanAssociates())
      {
        if (allLoanAssociate.Guid != associateLog.Guid && allLoanAssociate.RoleID == associateLog.RoleID && allLoanAssociate.LoanAssociateID == currentUserID)
          return;
      }
      switch (realWorldRole)
      {
        case RealWorldRoleID.LoanOfficer:
          this.ClearLoanOfficer();
          break;
        case RealWorldRoleID.LoanProcessor:
          this.ClearLoanProcessor();
          break;
        case RealWorldRoleID.LoanCloser:
          this.ClearLoanCloser();
          break;
        case RealWorldRoleID.Underwriter:
          this.ClearLoanUnderwriter();
          break;
      }
    }

    private void assignRealWorldRole(
      RealWorldRoleID realWorldRole,
      UserInfo user,
      string currentUserID,
      ResolveLoanAssociateHandler conflictResolutionHandler)
    {
      if (currentUserID == "" || currentUserID == user.Userid)
      {
        this.setRealWorldRoleUser(realWorldRole, user);
      }
      else
      {
        List<string> stringList = new List<string>();
        foreach (LoanAssociateLog allLoanAssociate in this.LoanData.GetLogList().GetAllLoanAssociates())
        {
          if (this.GetRealWorldRoleID(allLoanAssociate.RoleID).Contains(realWorldRole) && allLoanAssociate.LoanAssociateID != "" && allLoanAssociate.LoanAssociateType == LoanAssociateType.User && allLoanAssociate.LoanAssociateID != user.Userid && !stringList.Contains(allLoanAssociate.LoanAssociateID))
            stringList.Add(allLoanAssociate.LoanAssociateID);
        }
        if (stringList.Count == 0)
        {
          this.setRealWorldRoleUser(realWorldRole, user);
        }
        else
        {
          Hashtable users = this.sessionObjects.OrganizationManager.GetUsers(stringList.ToArray());
          if (users.Count == 0)
          {
            this.setRealWorldRoleUser(realWorldRole, user);
          }
          else
          {
            if (conflictResolutionHandler == null)
              return;
            UserInfo[] eligibleUsers = new UserInfo[users.Count + 1];
            eligibleUsers[0] = user;
            users.Values.CopyTo((Array) eligibleUsers, 1);
            UserInfo user1 = conflictResolutionHandler(realWorldRole, eligibleUsers, currentUserID);
            if (!(user1 != (UserInfo) null))
              return;
            this.setRealWorldRoleUser(realWorldRole, user1);
          }
        }
      }
    }

    private void setRealWorldRoleUser(RealWorldRoleID realWorldRole, UserInfo user)
    {
      switch (realWorldRole)
      {
        case RealWorldRoleID.LoanOfficer:
          this.SetLoanOfficer(user);
          break;
        case RealWorldRoleID.LoanProcessor:
          this.SetLoanProcessor(user);
          break;
        case RealWorldRoleID.LoanCloser:
          this.SetLoanCloser(user);
          break;
        case RealWorldRoleID.Underwriter:
          this.SetLoanUnderwriter(user);
          break;
      }
    }

    private RoleInfo getRoleByID(int roleId)
    {
      foreach (RoleInfo allRole in this.SystemConfiguration.AllRoles)
      {
        if (allRole.RoleID == roleId)
          return allRole;
      }
      return (RoleInfo) null;
    }

    public string[] GetLoanAssociateUsers(int roleId)
    {
      List<string> stringList = new List<string>();
      foreach (LoanAssociateLog assignedAssociate in this.loanData.GetLogList().GetAssignedAssociates(roleId))
      {
        if (assignedAssociate.LoanAssociateType == LoanAssociateType.User)
        {
          if (!stringList.Contains(assignedAssociate.LoanAssociateID))
            stringList.Add(assignedAssociate.LoanAssociateID);
        }
        else if (assignedAssociate.LoanAssociateType == LoanAssociateType.Group)
        {
          foreach (string str in this.sessionObjects.AclGroupManager.GetUsersInGroup(EllieMae.EMLite.Common.Utils.ParseInt((object) assignedAssociate.LoanAssociateID, -1), true))
          {
            if (!stringList.Contains(str))
              stringList.Add(str);
          }
        }
      }
      return stringList.ToArray();
    }

    public bool SyncAllEDisclosurePackageStatuses(bool clearNotification)
    {
      if (this.IsNew())
        return false;
      bool flag = EllieMae.EMLite.Common.Utils.CheckIf2015RespaTila(this.loanData.GetField("3969"));
      if (flag && this.IsPlatformLoan())
        return EDeliveryLoanSync.SyncLoan(this);
      this.SyncESignConsentData();
      return flag ? this.SyncEDisclosurePackageStatus2015(new List<EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log>((IEnumerable<EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log>) this.loanData.GetLogList().GetAllDisclosureTracking2015Log(false)), clearNotification) : this.SyncEDisclosurePackageStatus(new List<EllieMae.EMLite.DataEngine.Log.DisclosureTrackingLog>((IEnumerable<EllieMae.EMLite.DataEngine.Log.DisclosureTrackingLog>) this.loanData.GetLogList().GetAllDisclosureTrackingLog(false)), clearNotification);
    }

    public bool SyncEDisclosurePackageStatus(
      List<EllieMae.EMLite.DataEngine.Log.DisclosureTrackingLog> logList,
      bool clearNotification)
    {
      bool flag1 = false;
      if (this.loan == null)
        return false;
      bool flag2 = false;
      foreach (EllieMae.EMLite.DataEngine.Log.DisclosureTrackingLog disclosureTrackingLog in logList.ToArray())
      {
        if (disclosureTrackingLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure && disclosureTrackingLog.IsDisclosed)
        {
          flag2 = true;
          break;
        }
      }
      if (!flag2)
        return true;
      List<DisclosurePackage> disclosurePackageDetails = DisclosurePackageUtils.GetDisclosurePackageDetails(this, clearNotification, logList, this.IsPlatformLoan());
      foreach (EllieMae.EMLite.DataEngine.Log.DisclosureTrackingLog log in logList.ToArray())
      {
        if (log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure && !(log.eDisclosurePackageID == ""))
        {
          DisclosurePackage detailInfo = (DisclosurePackage) null;
          foreach (DisclosurePackage disclosurePackage in disclosurePackageDetails.ToArray())
          {
            if (string.Compare(log.eDisclosurePackageID, disclosurePackage.PackageGuid, StringComparison.OrdinalIgnoreCase) == 0)
            {
              detailInfo = disclosurePackage;
              break;
            }
          }
          if (detailInfo != null)
          {
            log.eDisclosureBorrowerName = detailInfo.BorrowerName;
            log.eDisclosureBorrowerEmail = detailInfo.BorrowerEmail;
            log.eDisclosureBorrowerAuthenticatedDate = detailInfo.BorrowerAuthenticatedDate;
            log.eDisclosureBorrowerAuthenticatedIP = detailInfo.BorrowerAuthenticatedIP;
            log.eDisclosureBorrowerViewMessageDate = detailInfo.BorrowerViewedDate;
            log.eDisclosureBorrowerAcceptConsentDate = detailInfo.BorrowerConsentAcceptedDate;
            log.eDisclosureBorrowerRejectConsentDate = detailInfo.BorrowerConsentRejectedDate;
            log.eDisclosureBorrowerAcceptConsentIP = detailInfo.BorrowerConsentAcceptedIP;
            log.eDisclosureBorrowerRejectConsentIP = detailInfo.BorrowerConsentRejectedIP;
            log.eDisclosureBorrowereSignedDate = detailInfo.BorrowereSignedDate;
            log.eDisclosureBorrowereSignedIP = detailInfo.BorrowereSignedIP;
            log.eDisclosureCoBorrowerName = detailInfo.CoborrowerName;
            log.eDisclosureCoBorrowerEmail = detailInfo.CoborrowerEmail;
            log.eDisclosureCoBorrowerAuthenticatedDate = detailInfo.CoborrowerAuthenticatedDate;
            log.eDisclosureCoBorrowerAuthenticatedIP = detailInfo.CoborrowerAuthenticatedIP;
            log.eDisclosureCoBorrowerViewMessageDate = detailInfo.CoborrowerViewedDate;
            log.eDisclosureCoBorrowerAcceptConsentDate = detailInfo.CoborrowerConsentAcceptedDate;
            log.eDisclosureCoBorrowerRejectConsentDate = detailInfo.CoborrowerConsentRejectedDate;
            log.eDisclosureCoBorrowerAcceptConsentIP = detailInfo.CoborrowerConsentAcceptedIP;
            log.eDisclosureCoBorrowerRejectConsentIP = detailInfo.CoborrowerConsentRejectedIP;
            log.eDisclosureCoBorrowereSignedDate = detailInfo.CoborrowereSignedDate;
            log.eDisclosureCoBorrowereSignedIP = detailInfo.CoborrowereSignedIP;
            log.eDisclosureLOName = detailInfo.LoanOfficerName;
            log.eDisclosureLOViewMessageDate = detailInfo.LoanOfficerViewedDate;
            log.eDisclosureLOeSignedDate = detailInfo.LoanOfficereSignedDate;
            log.eDisclosureLOeSignedIP = detailInfo.LoanOfficereSignedIP;
            log.EDisclosureBorrowerDocumentViewedDate = detailInfo.DocumentViewedDate_Borrower;
            log.EDisclosureCoborrowerDocumentViewedDate = detailInfo.DocumentViewedDate_CoBorrower;
            log.EDisclosureBorrowerLoanLevelConsent = detailInfo.EDisclosureBorrowerLoanLevelConsent;
            log.EDisclosureCoBorrowerLoanLevelConsent = detailInfo.EDisclosureCoBorrowerLoanLevelConsent;
            log.FulfillmentOrderedBy = detailInfo.FulfillmentOrderedBy;
            log.FullfillmentProcessedDate = detailInfo.FulfillmentProcessedDate;
            log.eDisclosurePackageCreatedDate = detailInfo.PackageCreatedDate;
            log.FulfillmentOrderedBy_CoBorrower = detailInfo.FulfillmentOrderedBy_CoBorrower;
            log.FullfillmentProcessedDate_CoBorrower = detailInfo.FulfillmentProcessedDate_CoBorrower;
            if (detailInfo.ConsentPDF != null && detailInfo.ConsentPDF.Length != 0)
            {
              log.eDisclosureConsentPDF = "eDisclosure_" + log.Guid + ".pdf";
              BinaryObject data = new BinaryObject(detailInfo.ConsentPDF);
              try
              {
                this.SaveSupportingData(log.eDisclosureConsentPDF, data);
              }
              catch (SecurityException ex)
              {
                if (ex.Message.Equals("Access Denied"))
                  log.HasAccess = false;
                else
                  throw;
              }
            }
            else
              log.eDisclosureConsentPDF = "";
            if (this.loanData.Calculator != null)
              log.SetReceivedDateFromCalc = this.loanData.Calculator.CalculateNeweDisclosureReceivedDate((DisclosureTrackingBase) log, detailInfo);
            flag1 = ((flag1 ? 1 : 0) | 1) != 0;
          }
        }
      }
      return flag1;
    }

    public bool SyncEDisclosurePackageStatus2015(
      List<EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log> logList,
      bool clearNotification)
    {
      bool flag1 = false;
      if (this.loan == null)
        return false;
      bool flag2 = EllieMae.EMLite.Common.Utils.CheckIf2015RespaTila(this.loanData.GetField("3969"));
      bool flag3 = this.IsPlatformLoan();
      if (flag2 & flag3)
        return EDeliveryLoanSync.SyncLoan(this);
      bool flag4 = false;
      System.TimeZoneInfo tz = (System.TimeZoneInfo) null;
      foreach (EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log disclosureTracking2015Log in logList.ToArray())
      {
        if (disclosureTracking2015Log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure && disclosureTracking2015Log.IsDisclosed)
        {
          tz = disclosureTracking2015Log.TimeZoneInfo;
          flag4 = true;
          break;
        }
      }
      if (!flag4)
        return true;
      List<EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log> source = new List<EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log>();
      bool flag5 = this.LoanData.LinkSyncType == LinkSyncType.ConstructionLinked;
      if (flag5)
        source = new List<EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log>((IEnumerable<EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log>) this.LoanData.LinkedData.GetLogList().GetAllDisclosureTracking2015Log(true));
      List<DisclosurePackage> disclosurePackageDetails = DisclosurePackageUtils.GetDisclosurePackageDetails(this, clearNotification, logList, tz, flag3);
      foreach (EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log disclosureTracking2015Log1 in logList.ToArray())
      {
        EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log log = disclosureTracking2015Log1;
        log.GetAllnboItems();
        if (log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure)
        {
          if (flag5)
          {
            EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log disclosureTracking2015Log2 = source.FirstOrDefault<EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log>((Func<EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log, bool>) (x => x.Guid == log.Guid));
            if (disclosureTracking2015Log2 != null)
            {
              log.eDisclosurePackageID = disclosureTracking2015Log2.eDisclosurePackageID;
              log.eDisclosureDisclosedMessage = disclosureTracking2015Log2.eDisclosureDisclosedMessage;
              log.eDisclosureManuallyFulfilledBy = disclosureTracking2015Log2.eDisclosureManuallyFulfilledBy;
              log.eDisclosureManualFulfillmentDate = disclosureTracking2015Log2.eDisclosureManualFulfillmentDate;
              log.eDisclosureManualFulfillmentMethod = disclosureTracking2015Log2.eDisclosureManualFulfillmentMethod;
              log.PresumedFulfillmentDate = disclosureTracking2015Log2.PresumedFulfillmentDate;
              log.ActualFulfillmentDate = disclosureTracking2015Log2.ActualFulfillmentDate;
              log.eDisclosureManualFulfillmentComment = disclosureTracking2015Log2.eDisclosureManualFulfillmentComment;
              log.LockedDisclosedDateField = disclosureTracking2015Log2.LockedDisclosedDateField;
              log.OriginalDisclosedDate = disclosureTracking2015Log2.OriginalDisclosedDate;
              log.LEDisclosedByBroker = disclosureTracking2015Log2.LEDisclosedByBroker;
              log.ReceivedDate = disclosureTracking2015Log2.ReceivedDate;
              log.LockedDisclosedByField = disclosureTracking2015Log2.LockedDisclosedByField;
              log.IsDisclosedByLocked = disclosureTracking2015Log2.IsDisclosedByLocked;
              log.IsLocked = disclosureTracking2015Log2.IsLocked;
              log.DisclosureMethod = disclosureTracking2015Log2.DisclosureMethod;
              log.DisclosedMethodOther = disclosureTracking2015Log2.DisclosedMethodOther;
              log.IntentToProceed = disclosureTracking2015Log2.IntentToProceed;
              log.IntentToProceedReceivedMethod = disclosureTracking2015Log2.IntentToProceedReceivedMethod;
              log.IntentToProceedDate = disclosureTracking2015Log2.IntentToProceedDate;
              log.IntentToProceedComments = disclosureTracking2015Log2.IntentToProceedComments;
              log.IntentToProceedReceivedMethodOther = disclosureTracking2015Log2.IntentToProceedReceivedMethodOther;
              log.BorrowerActualReceivedDate = disclosureTracking2015Log2.BorrowerActualReceivedDate;
              log.BorrowerDisclosedMethod = disclosureTracking2015Log2.BorrowerDisclosedMethod;
              log.BorrowerPresumedReceivedDate = disclosureTracking2015Log2.BorrowerPresumedReceivedDate;
              log.BorrowerType = disclosureTracking2015Log2.BorrowerType;
              log.IsBorrowerPresumedDateLocked = disclosureTracking2015Log2.IsBorrowerPresumedDateLocked;
              log.LockedBorrowerPresumedReceivedDate = disclosureTracking2015Log2.LockedBorrowerPresumedReceivedDate;
              log.BorrowerDisclosedMethodOther = disclosureTracking2015Log2.BorrowerDisclosedMethodOther;
              log.CoBorrowerActualReceivedDate = disclosureTracking2015Log2.CoBorrowerActualReceivedDate;
              log.CoBorrowerDisclosedMethod = disclosureTracking2015Log2.CoBorrowerDisclosedMethod;
              log.CoBorrowerPresumedReceivedDate = disclosureTracking2015Log2.CoBorrowerPresumedReceivedDate;
              log.CoBorrowerType = disclosureTracking2015Log2.CoBorrowerType;
              log.IsCoBorrowerPresumedDateLocked = disclosureTracking2015Log2.IsCoBorrowerPresumedDateLocked;
              log.LockedCoBorrowerPresumedReceivedDate = disclosureTracking2015Log2.LockedCoBorrowerPresumedReceivedDate;
              log.CoBorrowerDisclosedMethodOther = disclosureTracking2015Log2.CoBorrowerDisclosedMethodOther;
              log.eDisclosureManuallyFulfilledBy = disclosureTracking2015Log2.eDisclosureManuallyFulfilledBy;
              log.eDisclosureManualFulfillmentDate = disclosureTracking2015Log2.eDisclosureManualFulfillmentDate;
              log.eDisclosureManualFulfillmentMethod = disclosureTracking2015Log2.eDisclosureManualFulfillmentMethod;
              log.eDisclosureManualFulfillmentComment = disclosureTracking2015Log2.eDisclosureManualFulfillmentComment;
              log.PresumedFulfillmentDate = disclosureTracking2015Log2.PresumedFulfillmentDate;
              log.ActualFulfillmentDate = disclosureTracking2015Log2.ActualFulfillmentDate;
              log.eDisclosureConsentPDF = disclosureTracking2015Log2.eDisclosureConsentPDF;
              log.DisclosureType = disclosureTracking2015Log2.DisclosureType;
              log.eDisclosureBorrowerName = disclosureTracking2015Log2.eDisclosureBorrowerName;
              log.eDisclosureBorrowerEmail = disclosureTracking2015Log2.eDisclosureBorrowerEmail;
              log.eDisclosureBorrowerAuthenticatedDate = disclosureTracking2015Log2.eDisclosureBorrowerAuthenticatedDate;
              log.eDisclosureBorrowerAuthenticatedIP = disclosureTracking2015Log2.eDisclosureBorrowerAuthenticatedIP;
              log.eDisclosureBorrowerViewMessageDate = disclosureTracking2015Log2.eDisclosureBorrowerViewMessageDate;
              log.eDisclosureBorrowerAcceptConsentDate = disclosureTracking2015Log2.eDisclosureBorrowerAcceptConsentDate;
              log.eDisclosureBorrowerRejectConsentDate = disclosureTracking2015Log2.eDisclosureBorrowerRejectConsentDate;
              log.eDisclosureBorrowerAcceptConsentIP = disclosureTracking2015Log2.eDisclosureBorrowerAcceptConsentIP;
              log.eDisclosureBorrowerRejectConsentIP = disclosureTracking2015Log2.eDisclosureBorrowerRejectConsentIP;
              log.eDisclosureBorrowereSignedDate = disclosureTracking2015Log2.eDisclosureBorrowereSignedDate;
              log.eDisclosureBorrowereSignedIP = disclosureTracking2015Log2.eDisclosureBorrowereSignedIP;
              log.eDisclosureCoBorrowerName = disclosureTracking2015Log2.eDisclosureCoBorrowerName;
              log.eDisclosureCoBorrowerEmail = disclosureTracking2015Log2.eDisclosureCoBorrowerEmail;
              log.eDisclosureCoBorrowerAuthenticatedDate = disclosureTracking2015Log2.eDisclosureCoBorrowerAuthenticatedDate;
              log.eDisclosureCoBorrowerAuthenticatedIP = disclosureTracking2015Log2.eDisclosureCoBorrowerAuthenticatedIP;
              log.eDisclosureCoBorrowerViewMessageDate = disclosureTracking2015Log2.eDisclosureCoBorrowerViewMessageDate;
              log.eDisclosureCoBorrowerAcceptConsentDate = disclosureTracking2015Log2.eDisclosureCoBorrowerAcceptConsentDate;
              log.eDisclosureCoBorrowerRejectConsentDate = disclosureTracking2015Log2.eDisclosureCoBorrowerRejectConsentDate;
              log.eDisclosureCoBorrowerAcceptConsentIP = disclosureTracking2015Log2.eDisclosureCoBorrowerAcceptConsentIP;
              log.eDisclosureCoBorrowerRejectConsentIP = disclosureTracking2015Log2.eDisclosureCoBorrowerRejectConsentIP;
              log.eDisclosureCoBorrowereSignedDate = disclosureTracking2015Log2.eDisclosureCoBorrowereSignedDate;
              log.eDisclosureCoBorrowereSignedIP = disclosureTracking2015Log2.eDisclosureCoBorrowereSignedIP;
              log.eDisclosureLOName = disclosureTracking2015Log2.eDisclosureLOName;
              log.eDisclosureLOViewMessageDate = disclosureTracking2015Log2.eDisclosureLOViewMessageDate;
              log.eDisclosureLOeSignedDate = disclosureTracking2015Log2.eDisclosureLOeSignedDate;
              log.eDisclosureLOeSignedIP = disclosureTracking2015Log2.eDisclosureLOeSignedIP;
              log.IsWetSigned = disclosureTracking2015Log2.IsWetSigned;
              log.FulfillmentOrderedBy = disclosureTracking2015Log2.FulfillmentOrderedBy;
              log.FullfillmentProcessedDate = disclosureTracking2015Log2.FullfillmentProcessedDate;
              log.PresumedFulfillmentDate = disclosureTracking2015Log2.PresumedFulfillmentDate;
              log.eDisclosurePackageCreatedDate = disclosureTracking2015Log2.eDisclosurePackageCreatedDate;
              log.eDisclosureConsentPDF = disclosureTracking2015Log2.eDisclosureConsentPDF;
              log.FulfillmentOrderedBy_CoBorrower = disclosureTracking2015Log2.FulfillmentOrderedBy_CoBorrower;
              log.FullfillmentProcessedDate_CoBorrower = disclosureTracking2015Log2.FullfillmentProcessedDate_CoBorrower;
              log.EDisclosureBorrowerDocumentViewedDate = disclosureTracking2015Log2.EDisclosureBorrowerDocumentViewedDate;
              log.EDisclosureCoborrowerDocumentViewedDate = disclosureTracking2015Log2.EDisclosureCoborrowerDocumentViewedDate;
              log.EDisclosureBorrowerLoanLevelConsent = disclosureTracking2015Log2.EDisclosureBorrowerLoanLevelConsent;
              log.EDisclosureCoBorrowerLoanLevelConsent = disclosureTracking2015Log2.EDisclosureCoBorrowerLoanLevelConsent;
              foreach (KeyValuePair<string, INonBorrowerOwnerItem> borrowerOwnerCollection in disclosureTracking2015Log2.NonBorrowerOwnerCollections)
                log.NonBorrowerOwnerCollections[borrowerOwnerCollection.Key] = borrowerOwnerCollection.Value;
            }
            else
              continue;
          }
          if (!(log.eDisclosurePackageID == ""))
          {
            DisclosurePackage detailInfo = (DisclosurePackage) null;
            foreach (DisclosurePackage disclosurePackage in disclosurePackageDetails.ToArray())
            {
              if (string.Compare(log.eDisclosurePackageID, disclosurePackage.PackageGuid, StringComparison.OrdinalIgnoreCase) == 0)
              {
                detailInfo = disclosurePackage;
                break;
              }
            }
            if (detailInfo != null)
            {
              bool flag6 = false;
              bool flag7 = false;
              if (flag3)
              {
                if (!log.BorrowerLoanLevelConsentMapForCC)
                {
                  flag6 = !this.containBorrowerUpdates(log, detailInfo);
                  if (!flag6)
                  {
                    log.BorrowerLoanLevelConsentMapForCC = true;
                    log.resetBorrowerReceiveDate();
                  }
                }
                if (!log.CoBorrowerLoanLevelConsentMapForCC)
                {
                  flag7 = !this.containCoBorrowerUpdates(log, detailInfo);
                  if (!flag7)
                  {
                    log.CoBorrowerLoanLevelConsentMapForCC = true;
                    log.resetCoBorrowerReceiveDate();
                  }
                }
              }
              log.eDisclosureBorrowerName = detailInfo.BorrowerName;
              log.eDisclosureBorrowerEmail = detailInfo.BorrowerEmail;
              log.eDisclosureBorrowerAuthenticatedDate = detailInfo.BorrowerAuthenticatedDate;
              log.eDisclosureBorrowerAuthenticatedIP = detailInfo.BorrowerAuthenticatedIP;
              log.eDisclosureBorrowerViewMessageDate = detailInfo.BorrowerViewedDate;
              log.eDisclosureBorrowerAcceptConsentDate = detailInfo.BorrowerConsentAcceptedDate;
              log.eDisclosureBorrowerRejectConsentDate = detailInfo.BorrowerConsentRejectedDate;
              log.eDisclosureBorrowerAcceptConsentIP = detailInfo.BorrowerConsentAcceptedIP;
              log.eDisclosureBorrowerRejectConsentIP = detailInfo.BorrowerConsentRejectedIP;
              log.eDisclosureBorrowereSignedDate = detailInfo.BorrowereSignedDate;
              log.eDisclosureBorrowereSignedIP = detailInfo.BorrowereSignedIP;
              log.eDisclosureCoBorrowerName = detailInfo.CoborrowerName;
              log.eDisclosureCoBorrowerEmail = detailInfo.CoborrowerEmail;
              log.eDisclosureCoBorrowerAuthenticatedDate = detailInfo.CoborrowerAuthenticatedDate;
              log.eDisclosureCoBorrowerAuthenticatedIP = detailInfo.CoborrowerAuthenticatedIP;
              log.eDisclosureCoBorrowerViewMessageDate = detailInfo.CoborrowerViewedDate;
              log.eDisclosureCoBorrowerAcceptConsentDate = detailInfo.CoborrowerConsentAcceptedDate;
              log.eDisclosureCoBorrowerRejectConsentDate = detailInfo.CoborrowerConsentRejectedDate;
              log.eDisclosureCoBorrowerAcceptConsentIP = detailInfo.CoborrowerConsentAcceptedIP;
              log.eDisclosureCoBorrowerRejectConsentIP = detailInfo.CoborrowerConsentRejectedIP;
              log.eDisclosureCoBorrowereSignedDate = detailInfo.CoborrowereSignedDate;
              log.eDisclosureCoBorrowereSignedIP = detailInfo.CoborrowereSignedIP;
              log.eDisclosureLOName = detailInfo.LoanOfficerName;
              log.eDisclosureLOUserId = detailInfo.LoanOfficerUserId;
              log.eDisclosureLOViewMessageDate = detailInfo.LoanOfficerViewedDate;
              log.eDisclosureLOeSignedDate = detailInfo.LoanOfficereSignedDate;
              log.eDisclosureLOeSignedIP = detailInfo.LoanOfficereSignedIP;
              log.FulfillmentOrderedBy = detailInfo.FulfillmentOrderedBy;
              log.FullfillmentProcessedDate = detailInfo.FulfillmentProcessedDate;
              if (log.FullfillmentProcessedDate != DateTime.MinValue)
              {
                BusinessCalendar businessCalendar = this.sessionObjects.GetBusinessCalendar(CalendarType.RegZ);
                log.PresumedFulfillmentDate = businessCalendar.AddBusinessDays(log.FullfillmentProcessedDate, 3, true);
              }
              log.eDisclosurePackageCreatedDate = detailInfo.PackageCreatedDate;
              log.FulfillmentOrderedBy_CoBorrower = detailInfo.FulfillmentOrderedBy_CoBorrower;
              log.FullfillmentProcessedDate_CoBorrower = detailInfo.FulfillmentProcessedDate_CoBorrower;
              log.EDisclosureBorrowerDocumentViewedDate = detailInfo.DocumentViewedDate_Borrower;
              log.EDisclosureCoborrowerDocumentViewedDate = detailInfo.DocumentViewedDate_CoBorrower;
              if (flag3)
              {
                log.EDisclosureBorrowerLoanLevelConsent = flag6 ? detailInfo.EDisclosureBorrowerPackageLevelConsent : detailInfo.EDisclosureBorrowerLoanLevelConsent;
                log.EDisclosureCoBorrowerLoanLevelConsent = flag7 ? detailInfo.EDisclosureCoBorrowerPackageLevelConsent : detailInfo.EDisclosureCoBorrowerLoanLevelConsent;
              }
              else
              {
                log.EDisclosureBorrowerLoanLevelConsent = detailInfo.EDisclosureBorrowerLoanLevelConsent;
                log.EDisclosureCoBorrowerLoanLevelConsent = detailInfo.EDisclosureCoBorrowerLoanLevelConsent;
              }
              foreach (KeyValuePair<string, INonBorrowerOwnerItem> allnboItem in log.GetAllnboItems())
              {
                string key;
                if (flag3)
                  key = string.Join(" ", ((IEnumerable<string>) new string[4]
                  {
                    allnboItem.Value.FirstName,
                    allnboItem.Value.MidName,
                    allnboItem.Value.LastName,
                    allnboItem.Value.Suffix
                  }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str)))) + " " + allnboItem.Value.Email;
                else
                  key = allnboItem.Value.TRGuid;
                if (detailInfo.NBODetail.ContainsKey(key))
                {
                  log.SetnboAttributeValue(allnboItem.Key, (object) detailInfo.NBODetail[key].authenticatedDate, EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAuthenticatedDate);
                  log.SetnboAttributeValue(allnboItem.Key, (object) detailInfo.NBODetail[key].authenticatedIP, EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAuthenticatedIP);
                  log.SetnboAttributeValue(allnboItem.Key, (object) detailInfo.NBODetail[key].documentViewedDate, EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBODocumentViewedDate);
                  log.SetnboAttributeValue(allnboItem.Key, (object) detailInfo.NBODetail[key].consentAcceptedDate, EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAcceptConsentDate);
                  log.SetnboAttributeValue(allnboItem.Key, (object) detailInfo.NBODetail[key].consentRejectedDate, EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBORejectConsentDate);
                  if (detailInfo.NBODetail[key].consentAcceptedDate != DateTime.MinValue)
                    log.SetnboAttributeValue(allnboItem.Key, (object) detailInfo.NBODetail[key].consentIP, EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAcceptConsentIP);
                  if (detailInfo.NBODetail[key].consentRejectedDate != DateTime.MinValue)
                    log.SetnboAttributeValue(allnboItem.Key, (object) detailInfo.NBODetail[key].consentIP, EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBORejectConsentIP);
                  log.SetnboAttributeValue(allnboItem.Key, (object) detailInfo.NBODetail[key].eSignedDate, EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOSignedDate);
                  log.SetnboAttributeValue(allnboItem.Key, (object) detailInfo.NBODetail[key].eSignedIP, EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOeSignedIP);
                  log.SetnboAttributeValue(allnboItem.Key, (object) detailInfo.NBODetail[key].viewedDate, EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOViewMessageDate);
                  log.SetnboAttributeValue(allnboItem.Key, (object) detailInfo.NBODetail[key].loanLevelConsent, EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOLoanLevelConsent);
                  log.SetnboAttributeValue(allnboItem.Key, (object) detailInfo.NBODetail[key].eSignatures, EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOeSignatures);
                }
              }
              if (detailInfo.ConsentPDF != null && detailInfo.ConsentPDF.Length != 0)
              {
                log.eDisclosureConsentPDF = "eDisclosure_" + log.Guid + ".pdf";
                BinaryObject data = new BinaryObject(detailInfo.ConsentPDF);
                try
                {
                  this.SaveSupportingData(log.eDisclosureConsentPDF, data);
                }
                catch (SecurityException ex)
                {
                  if (ex.Message.Equals("Access Denied"))
                    log.HasAccess = false;
                  else
                    throw;
                }
              }
              else
                log.eDisclosureConsentPDF = "";
              if (this.loanData.Calculator != null)
              {
                this.loanData.Calculator.CalculateNew2015DisclosureReceivedDate((EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log) log, detailInfo, flag3);
                this.loanData.Calculator.CalculateLatestDisclosure2015((EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log) log);
              }
              flag1 = ((flag1 ? 1 : 0) | 1) != 0;
            }
          }
        }
      }
      return flag1;
    }

    private bool containBorrowerUpdates(EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log log, DisclosurePackage detailInfo)
    {
      DateTime authenticatedDate = detailInfo.BorrowerAuthenticatedDate;
      if (log.eDisclosureBorrowerAuthenticatedDate != detailInfo.BorrowerAuthenticatedDate || !string.IsNullOrWhiteSpace(detailInfo.BorrowerAuthenticatedIP) && log.eDisclosureBorrowerAuthenticatedIP != detailInfo.BorrowerAuthenticatedIP)
        return true;
      DateTime borrowerViewedDate = detailInfo.BorrowerViewedDate;
      if (log.eDisclosureBorrowerViewMessageDate != detailInfo.BorrowerViewedDate)
        return true;
      DateTime consentAcceptedDate = detailInfo.BorrowerConsentAcceptedDate;
      if (log.eDisclosureBorrowerAcceptConsentDate != detailInfo.BorrowerConsentAcceptedDate)
        return true;
      DateTime consentRejectedDate = detailInfo.BorrowerConsentRejectedDate;
      if (log.eDisclosureBorrowerRejectConsentDate != detailInfo.BorrowerConsentRejectedDate || !string.IsNullOrWhiteSpace(detailInfo.BorrowerConsentAcceptedIP) && log.eDisclosureBorrowerAcceptConsentIP != detailInfo.BorrowerConsentAcceptedIP || !string.IsNullOrWhiteSpace(detailInfo.BorrowerConsentRejectedIP) && log.eDisclosureBorrowerRejectConsentIP != detailInfo.BorrowerConsentRejectedIP)
        return true;
      DateTime borrowereSignedDate = detailInfo.BorrowereSignedDate;
      return log.eDisclosureBorrowereSignedDate != detailInfo.BorrowereSignedDate || !string.IsNullOrWhiteSpace(detailInfo.BorrowereSignedIP) && log.eDisclosureBorrowereSignedIP != detailInfo.BorrowereSignedIP;
    }

    private bool containCoBorrowerUpdates(
      EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log log,
      DisclosurePackage detailInfo)
    {
      DateTime authenticatedDate = detailInfo.CoborrowerAuthenticatedDate;
      if (log.eDisclosureCoBorrowerAuthenticatedDate != detailInfo.CoborrowerAuthenticatedDate || !string.IsNullOrWhiteSpace(detailInfo.CoborrowerAuthenticatedIP) && log.eDisclosureCoBorrowerAuthenticatedIP != detailInfo.CoborrowerAuthenticatedIP)
        return true;
      DateTime coborrowerViewedDate = detailInfo.CoborrowerViewedDate;
      if (log.eDisclosureCoBorrowerViewMessageDate != detailInfo.CoborrowerViewedDate)
        return true;
      DateTime consentAcceptedDate = detailInfo.CoborrowerConsentAcceptedDate;
      if (log.eDisclosureCoBorrowerAcceptConsentDate != detailInfo.CoborrowerConsentAcceptedDate)
        return true;
      DateTime consentRejectedDate = detailInfo.CoborrowerConsentRejectedDate;
      if (log.eDisclosureCoBorrowerRejectConsentDate != detailInfo.CoborrowerConsentRejectedDate || !string.IsNullOrWhiteSpace(detailInfo.CoborrowerConsentAcceptedIP) && log.eDisclosureCoBorrowerAcceptConsentIP != detailInfo.CoborrowerConsentAcceptedIP || !string.IsNullOrWhiteSpace(detailInfo.CoborrowerConsentRejectedIP) && log.eDisclosureCoBorrowerRejectConsentIP != detailInfo.CoborrowerConsentRejectedIP)
        return true;
      DateTime coborrowereSignedDate = detailInfo.CoborrowereSignedDate;
      return log.eDisclosureCoBorrowereSignedDate != detailInfo.CoborrowereSignedDate || !string.IsNullOrWhiteSpace(detailInfo.CoborrowereSignedIP) && log.eDisclosureCoBorrowereSignedIP != detailInfo.CoborrowereSignedIP;
    }

    public string WCNotAllowedMessage => this.wcNotAllowedMessage;

    public bool IsPlatformLoan(bool saveLoan = false, bool setCCSiteId = false)
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "Entering IsPlatformLoan");
      try
      {
        if (this.isConsumerConnectLoan(saveLoan, setCCSiteId))
          return true;
        if (this.isDeliveryPartnerLoan())
          return true;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "LoanDataMgr: Error in IsPlatformLoan. Ex: " + (object) ex);
      }
      return false;
    }

    private bool isConsumerConnectLoan(bool saveLoan, bool setCCSiteId)
    {
      this.wcNotAllowedMessage = string.Empty;
      string str1 = "This process can no longer be completed electronically. Any documents will have to be sent manually.";
      string str2 = "We are unable to send this request. Please ensure that you have a valid Consumer Connect site configured.";
      if (!string.IsNullOrEmpty(this.loanData.GetField("WebSiteId")))
      {
        if (setCCSiteId && !this.SessionObjects.StartupInfo.AllowWCRouting)
          this.wcNotAllowedMessage = str1;
        PerformanceMeter.Current.AddCheckpoint("EXIT - WebSiteId - isConsumerConnectLoan: False", 12893, nameof (isConsumerConnectLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        return false;
      }
      string simpleField = this.loanData.GetSimpleField("ConsumerConnectSiteID");
      if (simpleField == "0")
      {
        if (setCCSiteId && !this.SessionObjects.StartupInfo.AllowWCRouting)
          this.wcNotAllowedMessage = str1;
        PerformanceMeter.Current.AddCheckpoint("EXIT - ConsumerConnectSiteID - isConsumerConnectLoan: False", 12909, nameof (isConsumerConnectLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        return false;
      }
      if (!string.IsNullOrEmpty(simpleField))
      {
        PerformanceMeter.Current.AddCheckpoint("EXIT - ConsumerConnectSiteID - isConsumerConnectLoan: True", 12916, nameof (isConsumerConnectLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        return true;
      }
      if (!setCCSiteId)
        return false;
      string userId = this.loanData.GetField("LOID");
      if (string.IsNullOrEmpty(userId))
        userId = "0";
      if (!this.verifyEncompassServerConnection())
      {
        PerformanceMeter.Current.AddCheckpoint("EXIT - verifyEncompassServerConnection - isConsumerConnectLoan: False", 12936, nameof (isConsumerConnectLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        return false;
      }
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Verbose, "Creating EBSRestClient");
      EBSRestClient ebsRestClient = new EBSRestClient(this);
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Verbose, "Calling EBSRestClient.GetCCSiteId: " + userId);
      Task<string> ccSiteId = ebsRestClient.GetCCSiteId(userId);
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Verbose, "Waiting for Task");
      Task.WaitAll((Task) ccSiteId);
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Verbose, "Checking EBSRestClient.GetCCSiteId Response");
      string val = !string.IsNullOrEmpty(ccSiteId.Result) ? ccSiteId.Result : "0";
      if (val.Equals("0") && !this.SessionObjects.StartupInfo.AllowWCRouting)
      {
        this.wcNotAllowedMessage = str2;
        return false;
      }
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Verbose, "Setting ConsumerConnectSiteId Field: " + val);
      this.loanData.SetField("ConsumerConnectSiteID", val);
      if (saveLoan | setCCSiteId)
      {
        Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Verbose, "Calling LoanDataMgr.Save");
        this.Save();
      }
      PerformanceMeter.Current.AddCheckpoint("isConsumerConnectLoan: " + val, 12975, nameof (isConsumerConnectLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      return !val.Equals("0");
    }

    private bool isDeliveryPartnerLoan()
    {
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Verbose, "Creating DpfClient");
      DpfClient dpfClient = new DpfClient(this);
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Verbose, "Calling DpfClient.ResolveLoanSettings");
      Task<ResolveLoanSettingsResponseList> task = dpfClient.ResolveLoanSettings();
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Verbose, "Waiting for Task");
      Task.WaitAll((Task) task);
      Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Verbose, "Checking DpfClient.ResolveLoanSettings Response");
      if (task.Result != null && task.Result.Count > 0)
      {
        PerformanceMeter.Current.AddCheckpoint("isDeliveryPartnerLoan: True", 13001, nameof (isDeliveryPartnerLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        return true;
      }
      PerformanceMeter.Current.AddCheckpoint("isDeliveryPartnerLoan: False", 13006, nameof (isDeliveryPartnerLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      return false;
    }

    private bool verifyEncompassServerConnection()
    {
      bool flag = !this.SessionObjects.Offline;
      if (!flag)
        Tracing.Log(LoanDataMgr.sw, TraceLevel.Error, nameof (LoanDataMgr), "Lost connection to encompass server.");
      return flag;
    }

    private DateTime? parseDate(DateTime? consentDate, System.TimeZoneInfo timeZoneInfo)
    {
      if (!consentDate.HasValue)
        return new DateTime?(DateTime.MinValue);
      DateTime dateTime = consentDate.Value;
      if (timeZoneInfo != null)
        return DateTimeKind.Utc == dateTime.Kind ? new DateTime?(System.TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZoneInfo)) : new DateTime?(System.TimeZoneInfo.ConvertTime(dateTime, EllieMae.EMLite.Common.Utils.GetTimeZoneInfo("PST"), timeZoneInfo));
      string timeZoneCode = this.loanData.GetField("LE1.XG9") == "" ? this.loanData.GetField("LE1.X9") : this.loanData.GetField("LE1.XG9");
      return DateTimeKind.Utc == dateTime.Kind ? new DateTime?(System.TimeZoneInfo.ConvertTimeFromUtc(dateTime, EllieMae.EMLite.Common.Utils.GetTimeZoneInfo(timeZoneCode))) : new DateTime?(System.TimeZoneInfo.ConvertTime(dateTime, EllieMae.EMLite.Common.Utils.GetTimeZoneInfo("PST"), EllieMae.EMLite.Common.Utils.GetTimeZoneInfo(timeZoneCode)));
    }

    public bool SyncESignConsentData()
    {
      bool flag1 = EllieMae.EMLite.Common.Utils.CheckIf2015RespaTila(this.loanData.GetField("3969"));
      bool flag2 = this.IsPlatformLoan();
      if (flag1 & flag2)
        return EDeliveryLoanSync.SyncLoan(this);
      if (this.loanData.eConsentType == eConsentTypes.FullexternaleConsent)
        return false;
      System.TimeZoneInfo timeZoneInfo = (System.TimeZoneInfo) null;
      foreach (EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log disclosureTracking2015Log in ((IEnumerable<EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log>) this.loanData.GetLogList().GetAllDisclosureTracking2015Log(false)).ToArray<EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log>())
      {
        if (disclosureTracking2015Log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure && disclosureTracking2015Log.IsDisclosed)
        {
          timeZoneInfo = disclosureTracking2015Log.TimeZoneInfo;
          break;
        }
      }
      try
      {
        Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
        dictionary.Add("b1", new string[4]
        {
          "3984",
          "3985",
          "3986",
          "3987"
        });
        dictionary.Add("c1", new string[4]
        {
          "3988",
          "3989",
          "3990",
          "3991"
        });
        dictionary.Add("b2", new string[4]
        {
          "3992",
          "3993",
          "3994",
          "3995"
        });
        dictionary.Add("c2", new string[4]
        {
          "3996",
          "3997",
          "3998",
          "3999"
        });
        dictionary.Add("b3", new string[4]
        {
          "4023",
          "4024",
          "4025",
          "4026"
        });
        dictionary.Add("c3", new string[4]
        {
          "4027",
          "4028",
          "4029",
          "4030"
        });
        dictionary.Add("b4", new string[4]
        {
          "4031",
          "4032",
          "4033",
          "4034"
        });
        dictionary.Add("c4", new string[4]
        {
          "4035",
          "4036",
          "4037",
          "4038"
        });
        dictionary.Add("b5", new string[4]
        {
          "4039",
          "4040",
          "4041",
          "4042"
        });
        dictionary.Add("c5", new string[4]
        {
          "4043",
          "4044",
          "4045",
          "4046"
        });
        dictionary.Add("b6", new string[4]
        {
          "4047",
          "4048",
          "4049",
          "4050"
        });
        dictionary.Add("c6", new string[4]
        {
          "4051",
          "4052",
          "4053",
          "4054"
        });
        Security.UserConsentDataGetResponseUserConsentDataGetResponseBody[] source1;
        if (flag2)
        {
          List<EDeliveryConsentDetail> consentTrackingForCc = this.GetConsentTrackingForCC();
          List<Security.UserConsentDataGetResponseUserConsentDataGetResponseBody> source2 = new List<Security.UserConsentDataGetResponseUserConsentDataGetResponseBody>();
          if (consentTrackingForCc != null)
          {
            foreach (EDeliveryConsentDetail edeliveryConsentDetail in consentTrackingForCc)
            {
              string id = edeliveryConsentDetail.id;
              foreach (EDeliveryConsentOutput edeliveryConsentOutput in edeliveryConsentDetail.consentOutput)
              {
                Security.UserConsentDataGetResponseUserConsentDataGetResponseBody dataGetResponseBody = new Security.UserConsentDataGetResponseUserConsentDataGetResponseBody();
                if (string.IsNullOrEmpty(edeliveryConsentOutput.status))
                  dataGetResponseBody.ConsentStatus = new bool?();
                else if (string.Compare(edeliveryConsentOutput.status, "Accepted", true) == 0)
                  dataGetResponseBody.ConsentStatus = new bool?(true);
                else if (string.Compare(edeliveryConsentOutput.status, "Declined", true) == 0)
                  dataGetResponseBody.ConsentStatus = new bool?(false);
                dataGetResponseBody.ConsentIpAddress = edeliveryConsentOutput.ipAddress;
                dataGetResponseBody.ConsentDate = this.parseDate(new DateTime?(edeliveryConsentOutput.date), timeZoneInfo);
                dataGetResponseBody.User = new Security.User();
                dataGetResponseBody.User.EncompassContactGuid = id;
                dataGetResponseBody.User.UserEmail = edeliveryConsentOutput.email;
                dataGetResponseBody.User.UserFirstName = edeliveryConsentOutput.fullName;
                dataGetResponseBody.ApplicationId = "Consumer Connect";
                source2.Add(dataGetResponseBody);
              }
            }
          }
          source1 = source2.ToArray<Security.UserConsentDataGetResponseUserConsentDataGetResponseBody>();
        }
        else
        {
          BasicHttpBinding basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
          basicHttpBinding.MaxReceivedMessageSize = 2048000L;
          basicHttpBinding.MaxBufferSize = 2048000;
          string str = this.SessionObjects?.StartupInfo?.ServiceUrls?.ConsentServiceUrl;
          if (string.IsNullOrWhiteSpace(str) || !Uri.IsWellFormedUriString(str, UriKind.Absolute))
            str = "https://loancenter.elliemae.com/ConsentService/ConsentServiceController.svc";
          EndpointAddress remoteAddress = new EndpointAddress(str);
          ConsentServiceControllerClient controllerClient = new ConsentServiceControllerClient((System.ServiceModel.Channels.Binding) basicHttpBinding, remoteAddress);
          string ssoToken = this.getSsoToken();
          controllerClient.ChannelFactory.Endpoint.Behaviors.Add((IEndpointBehavior) new SsoTokenEndpointBehavior(ssoToken));
          source1 = controllerClient.UserConsentDataGet(new UserConsentDataGetRequest()
          {
            Security = new Security()
            {
              SecurityClientId = this.sessionObjects.CompanyInfo.ClientID,
              UserId = this.sessionObjects.UserID,
              Password = this.sessionObjects.UserPassword
            },
            UserConsentDataGetRequest1 = new Security.UserConsentDataGetRequestUserConsentDataGetRequestBody()
            {
              ClientId = this.sessionObjects.CompanyInfo.ClientID,
              LoanGuid = this.loanData.LinkSyncType != LinkSyncType.ConstructionLinked || this.LoanData.LinkedData == null ? this.loanData.GUID : this.loanData.LinkedData.GUID
            }
          }).UserConsentDataGetResponse1;
        }
        int num1 = ((IEnumerable<Security.UserConsentDataGetResponseUserConsentDataGetResponseBody>) source1).Where<Security.UserConsentDataGetResponseUserConsentDataGetResponseBody>((Func<Security.UserConsentDataGetResponseUserConsentDataGetResponseBody, bool>) (x => x.ConsentStatus.HasValue)).Count<Security.UserConsentDataGetResponseUserConsentDataGetResponseBody>();
        bool flag3 = ((IEnumerable<EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log>) this.loanData.GetLogList().GetAllIDisclosureTracking2015Log(true)).Count<EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log>() == 0;
        BorrowerPair[] borrowerPairs = this.loanData.GetBorrowerPairs();
        int num2 = 1;
        DateTime dateTime;
        bool? consentStatus;
        foreach (BorrowerPair borrowerPair in borrowerPairs)
        {
          string[] strArray1 = dictionary["b" + (object) num2];
          string[] strArray2 = dictionary["c" + (object) num2];
          string[] strArray3 = new string[4];
          if (source1.Length != 0 && num1 > 0)
          {
            foreach (string id in strArray1)
              this.loanData.SetField(id, "");
            foreach (string id in strArray2)
              this.loanData.SetField(id, "");
          }
          bool flag4 = false;
          bool flag5 = false;
          foreach (Security.UserConsentDataGetResponseUserConsentDataGetResponseBody dataGetResponseBody in source1)
          {
            if (!dataGetResponseBody.PackageId.HasValue)
            {
              bool flag6 = false;
              bool flag7 = false;
              bool flag8 = false;
              bool flag9 = false;
              if (!flag2)
              {
                if (dataGetResponseBody.User.UserFirstName.Trim().ToLower() == this.loanData.GetSimpleField("4000", borrowerPair).Trim().ToLower() && dataGetResponseBody.User.UserLastName.Trim().ToLower() == this.loanData.GetSimpleField("4002", borrowerPair).Trim().ToLower())
                {
                  if (dataGetResponseBody.User.UserEmail.Trim().ToLower() == this.loanData.GetSimpleField("1240", borrowerPair).Trim().ToLower())
                  {
                    flag6 = true;
                    flag4 = true;
                  }
                  else if (flag3 && !flag4)
                    flag8 = true;
                }
              }
              else
              {
                string str = string.Join(" ", ((IEnumerable<string>) new string[4]
                {
                  this.loanData.GetSimpleField("4000", borrowerPair),
                  this.loanData.GetSimpleField("4001", borrowerPair),
                  this.loanData.GetSimpleField("4002", borrowerPair),
                  this.loanData.GetSimpleField("4003", borrowerPair)
                }).Where<string>((Func<string, bool>) (x => !string.IsNullOrEmpty(x))));
                if (dataGetResponseBody.User.UserFirstName.Trim().ToLower() == str.Trim().ToLower())
                {
                  if (dataGetResponseBody.User.UserEmail.Trim().ToLower() == this.loanData.GetSimpleField("1240", borrowerPair).Trim().ToLower())
                  {
                    flag6 = true;
                    flag4 = true;
                  }
                  else if (!flag4)
                    flag8 = true;
                }
              }
              if (!flag2)
              {
                if (dataGetResponseBody.User.UserFirstName.Trim().ToLower() == this.loanData.GetSimpleField("4004", borrowerPair).Trim().ToLower() && dataGetResponseBody.User.UserLastName.Trim().ToLower() == this.loanData.GetSimpleField("4006", borrowerPair).Trim().ToLower() && !flag6)
                {
                  flag7 = !(this.loanData.GetSimpleField("1268", borrowerPair).Trim() == "") ? dataGetResponseBody.User.UserEmail.Trim().ToLower() == this.loanData.GetSimpleField("1268", borrowerPair).Trim().ToLower() : dataGetResponseBody.User.UserEmail.Trim().ToLower() == this.loanData.GetSimpleField("1240", borrowerPair).Trim().ToLower();
                  if (flag7)
                    flag5 = true;
                  if (flag3 && !flag7 && !flag5)
                    flag9 = true;
                }
              }
              else
              {
                string str = string.Join(" ", ((IEnumerable<string>) new string[4]
                {
                  this.loanData.GetSimpleField("4004", borrowerPair),
                  this.loanData.GetSimpleField("4005", borrowerPair),
                  this.loanData.GetSimpleField("4006", borrowerPair),
                  this.loanData.GetSimpleField("4007", borrowerPair)
                }).Where<string>((Func<string, bool>) (x => !string.IsNullOrEmpty(x))));
                if (dataGetResponseBody.User.UserFirstName.Trim().ToLower() == str.Trim().ToLower())
                {
                  flag7 = !(this.loanData.GetSimpleField("1268", borrowerPair).Trim() == "") ? dataGetResponseBody.User.UserEmail.Trim().ToLower() == this.loanData.GetSimpleField("1268", borrowerPair).Trim().ToLower() : dataGetResponseBody.User.UserEmail.Trim().ToLower() == this.loanData.GetSimpleField("1240", borrowerPair).Trim().ToLower();
                  if (flag7)
                    flag5 = true;
                  if (!flag7)
                    flag9 = true;
                }
              }
              bool flag10 = flag8 & flag9;
              if (flag6 | flag7 | flag10 && dataGetResponseBody.ConsentDate.HasValue)
              {
                dateTime = dataGetResponseBody.ConsentDate.Value;
                if (dateTime.Year != 1900)
                {
                  if (flag6 | flag8)
                    strArray3 = strArray1;
                  if (flag7 | flag9)
                    strArray3 = strArray2;
                  if (!this.loanData.IsLocked(strArray3[0]))
                  {
                    if (flag10)
                    {
                      this.loanData.SetField(strArray3[0], "");
                    }
                    else
                    {
                      consentStatus = dataGetResponseBody.ConsentStatus;
                      bool flag11 = true;
                      if (consentStatus.GetValueOrDefault() == flag11 & consentStatus.HasValue)
                      {
                        this.loanData.SetField(strArray3[0], "Accepted");
                      }
                      else
                      {
                        consentStatus = dataGetResponseBody.ConsentStatus;
                        bool flag12 = false;
                        if (consentStatus.GetValueOrDefault() == flag12 & consentStatus.HasValue)
                          this.loanData.SetField(strArray3[0], "Rejected");
                        else
                          this.loanData.SetField(strArray3[0], "Pending");
                      }
                    }
                  }
                  if (!this.loanData.IsLocked(strArray3[1]))
                    this.loanData.SetField(strArray3[1], string.Format("{0:MM/dd/yyyy}", (object) (flag10 ? new DateTime?() : this.parseDate(dataGetResponseBody.ConsentDate, timeZoneInfo))));
                  if (!this.loanData.IsLocked(strArray3[2]))
                    this.loanData.SetField(strArray3[2], flag10 ? "" : dataGetResponseBody.ConsentIpAddress);
                  if (!this.loanData.IsLocked(strArray3[3]))
                    this.loanData.SetField(strArray3[3], flag10 ? "" : dataGetResponseBody.ApplicationId);
                }
              }
            }
          }
          ++num2;
        }
        int additionalVestingParties = this.loanData.GetNumberOfAdditionalVestingParties();
        string str1 = "NBOC";
        for (int index = 1; index <= additionalVestingParties; ++index)
        {
          string str2 = this.loanData.GetNBOLinkedVesting(this.loanData.GetField("TR" + index.ToString("00") + "99")).ToString("00");
          string[] strArray = new string[4]
          {
            "17",
            "18",
            "19",
            "20"
          };
          if (source1.Length != 0 && num1 > 0)
          {
            foreach (string str3 in strArray)
              this.loanData.SetField(str1 + str2 + str3, "");
          }
          bool flag13 = false;
          foreach (Security.UserConsentDataGetResponseUserConsentDataGetResponseBody dataGetResponseBody in source1)
          {
            if (!dataGetResponseBody.PackageId.HasValue)
            {
              bool flag14 = false;
              bool flag15 = false;
              if (!flag2)
              {
                if (dataGetResponseBody.User.UserFirstName.Trim().ToLower() == this.loanData.GetSimpleField(str1 + str2 + "01").Trim().ToLower() && dataGetResponseBody.User.UserLastName.Trim().ToLower() == this.loanData.GetSimpleField(str1 + str2 + "03").Trim().ToLower())
                {
                  if (dataGetResponseBody.User.UserEmail.Trim().ToLower() == this.loanData.GetSimpleField(str1 + str2 + "11").Trim().ToLower())
                  {
                    flag14 = true;
                    flag13 = true;
                  }
                  else if (flag3 && !flag13)
                    flag15 = true;
                }
              }
              else
              {
                string str4 = string.Join(" ", ((IEnumerable<string>) new string[4]
                {
                  this.loanData.GetSimpleField(str1 + str2 + "01"),
                  this.loanData.GetSimpleField(str1 + str2 + "02"),
                  this.loanData.GetSimpleField(str1 + str2 + "03"),
                  this.loanData.GetSimpleField(str1 + str2 + "04")
                }).Where<string>((Func<string, bool>) (x => !string.IsNullOrEmpty(x))));
                if (dataGetResponseBody.User.UserFirstName.Trim().ToLower() == str4.Trim().ToLower())
                {
                  if (dataGetResponseBody.User.UserEmail.Trim().ToLower() == this.loanData.GetSimpleField(str1 + str2 + "11").Trim().ToLower())
                  {
                    flag14 = true;
                    flag13 = true;
                  }
                  else if (!flag13)
                    flag15 = true;
                }
              }
              if (flag14 | flag15 && dataGetResponseBody.ConsentDate.HasValue)
              {
                dateTime = dataGetResponseBody.ConsentDate.Value;
                if (dateTime.Year != 1900)
                {
                  if (flag15)
                  {
                    this.loanData.SetField(str1 + str2 + "17", "");
                  }
                  else
                  {
                    consentStatus = dataGetResponseBody.ConsentStatus;
                    bool flag16 = true;
                    if (consentStatus.GetValueOrDefault() == flag16 & consentStatus.HasValue)
                    {
                      this.loanData.SetField(str1 + str2 + "17", "Accepted");
                    }
                    else
                    {
                      consentStatus = dataGetResponseBody.ConsentStatus;
                      bool flag17 = false;
                      if (consentStatus.GetValueOrDefault() == flag17 & consentStatus.HasValue)
                        this.loanData.SetField(str1 + str2 + "17", "Rejected");
                      else
                        this.loanData.SetField(str1 + str2 + "17", "Pending");
                    }
                  }
                  this.loanData.SetField(str1 + str2 + "18", string.Format("{0:MM/dd/yyyy}", (object) (flag15 ? new DateTime?() : this.parseDate(dataGetResponseBody.ConsentDate, timeZoneInfo))));
                  this.loanData.SetField(str1 + str2 + "19", flag15 ? "" : dataGetResponseBody.ConsentIpAddress);
                  this.loanData.SetField(str1 + str2 + "20", flag15 ? "" : dataGetResponseBody.ApplicationId);
                }
              }
            }
          }
        }
        this.Calculator.LoadeSignConsentDate();
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDataMgr.sw, TraceLevel.Verbose, nameof (LoanDataMgr), "SyncESignConsentStatus Error: " + ex.Message);
        return false;
      }
    }

    private string getSsoToken()
    {
      string ssoToken = (string) null;
      if (this.SessionObjects.StartupInfo.RuntimeEnvironment == RuntimeEnvironment.Hosted)
      {
        int result = 5;
        int.TryParse(this.SessionObjects.ConfigurationManager.GetSsoTokenExpirationTimeForEdm(), out result);
        ssoToken = this.SessionObjects.IdentityManager.GetSsoToken((IEnumerable<string>) new string[1]
        {
          "Elli.Edm"
        }, result);
      }
      return ssoToken;
    }

    public string GetConsentPDF()
    {
      try
      {
        BasicHttpBinding basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
        basicHttpBinding.MaxReceivedMessageSize = 2048000L;
        basicHttpBinding.MaxBufferSize = 2048000;
        string str = this.SessionObjects?.StartupInfo?.ServiceUrls?.ConsentServiceUrl;
        if (string.IsNullOrWhiteSpace(str) || !Uri.IsWellFormedUriString(str, UriKind.Absolute))
          str = "https://loancenter.elliemae.com/ConsentService/ConsentServiceController.svc";
        EndpointAddress remoteAddress = new EndpointAddress(str);
        ConsentServiceControllerClient controllerClient = new ConsentServiceControllerClient((System.ServiceModel.Channels.Binding) basicHttpBinding, remoteAddress);
        string ssoToken = this.getSsoToken();
        controllerClient.ChannelFactory.Endpoint.Behaviors.Add((IEndpointBehavior) new SsoTokenEndpointBehavior(ssoToken));
        Security.ConsentPdfResponse[] source = controllerClient.ConsentPDFGet(new Security()
        {
          SecurityClientId = this.sessionObjects.CompanyInfo.ClientID,
          UserId = this.sessionObjects.UserID,
          Password = this.sessionObjects.UserPassword
        }, new Security.ConsentPDFGetRequestConsentPDFGetRequestBody()
        {
          ClientId = this.sessionObjects.CompanyInfo.ClientID,
          LoanGuid = this.loanData.LinkedData == null || this.loanData.LinkSyncType != LinkSyncType.ConstructionLinked ? this.loanData.GUID : this.loanData.LinkGUID
        });
        return ((IEnumerable<Security.ConsentPdfResponse>) source).Count<Security.ConsentPdfResponse>() > 0 ? source[0].ConsentPdf : (string) null;
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDataMgr.sw, TraceLevel.Verbose, nameof (LoanDataMgr), "GetConsentPDF Error: " + ex.Message);
        return (string) null;
      }
    }

    public List<EDeliveryConsentDetail> GetConsentTrackingForCC()
    {
      try
      {
        return new EDeliveryRestClient(this).GetLoanLevelConsentTracking().Result.consentTrackingDetails;
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDataMgr.sw, TraceLevel.Verbose, nameof (LoanDataMgr), "GetConsentTrackingForCC Error: " + ex.Message);
        return (List<EDeliveryConsentDetail>) null;
      }
    }

    private DateTime geteDisclosureReceivedDate(DisclosurePackage detailInfo)
    {
      if (!detailInfo.ContainCoBorrower)
        return detailInfo.BorrowereSignedDate;
      DateTime borrowereSignedDate = detailInfo.BorrowereSignedDate;
      DateTime coborrowereSignedDate = detailInfo.CoborrowereSignedDate;
      if (!(borrowereSignedDate != DateTime.MinValue) || !(coborrowereSignedDate != DateTime.MinValue))
        return DateTime.MinValue;
      return !(borrowereSignedDate > coborrowereSignedDate) ? coborrowereSignedDate : borrowereSignedDate;
    }

    public bool SubmitTPOLoanImportStatus(string loanGUID)
    {
      WebCenterImpotStatus centerImportStatus = this.sessionObjects.LoanManager.GetWebCenterImportStatus(loanGUID);
      return centerImportStatus == null || centerImportStatus.ImportID == string.Empty || this.SubmitTPOLoanImportStatus(centerImportStatus.ImportID, centerImportStatus.EMSiteID, centerImportStatus.LoanGUID, centerImportStatus.ImportDateTime);
    }

    public bool SubmitTPOLoanImportStatus(
      string loanImportID,
      string emSiteID,
      string loanGUID,
      DateTime importDateTime)
    {
      Tracing.Log(LoanDataMgr.sw, TraceLevel.Verbose, nameof (LoanDataMgr), "SubmitTPOLoanImportStatus:" + loanImportID + "," + emSiteID);
      importDateTime = importDateTime.ToUniversalTime();
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create("https://www.encompasswebcenter.com/WebCenterCommunications/LoanImportStatus.svc/ImportStatus?" + "emsiteid=" + HttpUtility.UrlEncode(emSiteID) + "&messageid=" + HttpUtility.UrlEncode(loanImportID) + "&importdate=" + HttpUtility.UrlEncode(importDateTime.ToString("M-dd-yyyy hh:mm:ss tt")));
      httpWebRequest.Method = "GET";
      StringBuilder stringBuilder = new StringBuilder();
      HttpWebResponse httpWebResponse = (HttpWebResponse) null;
      Stream stream = (Stream) null;
      StreamReader streamReader = (StreamReader) null;
      try
      {
        httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
        stream = httpWebResponse.GetResponseStream();
        streamReader = new StreamReader(stream);
        char[] buffer = new char[1024];
        for (int length = streamReader.Read(buffer, 0, buffer.Length); length > 0; length = streamReader.Read(buffer, 0, buffer.Length))
        {
          string str = new string(buffer, 0, length);
          stringBuilder.Append(str);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDataMgr.sw, TraceLevel.Verbose, nameof (LoanDataMgr), "SubmitTPOLoanImportStatus:" + loanImportID + "," + emSiteID + " Error: " + ex.Message);
        return false;
      }
      finally
      {
        streamReader?.Close();
        stream?.Close();
        httpWebResponse?.Close();
      }
      string str1 = stringBuilder.ToString();
      if (!(str1 != string.Empty) || str1.ToLower().IndexOf("succeed") <= -1)
        return false;
      this.sessionObjects.LoanManager.DeleteWebCenterImportID(loanImportID);
      return true;
    }

    public bool ValidateUnderwritingAdvancedConditions(string contactID)
    {
      bool flag = false;
      ExternalUserInfo userInfoByContactId = this.sessionObjects.ConfigurationManager.GetExternalUserInfoByContactId(contactID);
      if ((UserInfo) userInfoByContactId == (UserInfo) null)
        return true;
      ExternalOriginatorManagementData externalOrganization = this.sessionObjects.ConfigurationManager.GetExternalOrganization(false, userInfoByContactId.ExternalOrgID);
      if (externalOrganization == null)
        return true;
      BizRuleInfo underwritingConditions = (BizRuleInfo) this.sessionObjects.ConfigurationManager.GetExternalUnderwritingConditions(externalOrganization.oid);
      if (underwritingConditions == null)
        return true;
      ConditionEvaluators conditionEvaluators = new ConditionEvaluators(new BizRuleInfo[1]
      {
        underwritingConditions
      }, false);
      ExecutionContext context = new ExecutionContext(this.sessionObjects.UserInfo, this.loanData, (IServerDataProvider) new CustomCodeSessionDataProvider(this.sessionObjects));
      foreach (ConditionEvaluator conditionEvaluator in conditionEvaluators)
      {
        if (conditionEvaluator.Rule.Condition == BizRule.Condition.AdvancedCoding && conditionEvaluator.AppliesTo(context))
          flag = true;
      }
      return flag;
    }

    public EllieMae.EMLite.DataEngine.Log.LockRequestLog CreateLockRequestLog(
      EllieMae.EMLite.Common.RateLockRequestStatus status,
      bool display = true,
      string parentGuid = null)
    {
      return this.CreateLockRequestLog(this.sessionObjects.UserInfo, status, display, parentGuid);
    }

    public EllieMae.EMLite.DataEngine.Log.LockRequestLog CreateLockRequestLog(
      UserInfo requestingUser,
      EllieMae.EMLite.Common.RateLockRequestStatus status,
      bool display = true,
      string parentGuid = null)
    {
      EllieMae.EMLite.DataEngine.Log.LockRequestLog lockRequestLog = new EllieMae.EMLite.DataEngine.Log.LockRequestLog(this.loanData.GetLogList());
      lockRequestLog.Date = this.SessionObjects.Session.ServerTime;
      lockRequestLog.SetRequestingUser(requestingUser.Userid, requestingUser.FullName);
      lockRequestLog.LockRequestStatus = status;
      lockRequestLog.DisplayInLog = display;
      lockRequestLog.ReLockSequenceNumberForInactiveLock = LockUtils.GetReLockSequenceNumberForInactiveLock(this.loanData);
      if (!string.IsNullOrEmpty(parentGuid))
        lockRequestLog.ParentLockGuid = parentGuid;
      return lockRequestLog;
    }

    public EllieMae.EMLite.DataEngine.Log.LockRemovedLog CreateLockRemovedLog(string requestedGuid)
    {
      EllieMae.EMLite.DataEngine.Log.LockRemovedLog lockRemovedLog = new EllieMae.EMLite.DataEngine.Log.LockRemovedLog();
      lockRemovedLog.SetRemovedBy(this.sessionObjects.UserInfo);
      DateTime serverTime = this.SessionObjects.Session.ServerTime;
      lockRemovedLog.Date = serverTime;
      lockRemovedLog.TimeRemoved = serverTime.ToLongTimeString();
      lockRemovedLog.RequestGUID = requestedGuid;
      lockRemovedLog.AlertLoanOfficer = true;
      return lockRemovedLog;
    }

    public void SetDisclosureTracking2015LogType(EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log log)
    {
      log.DisclosureType = DisclosureTrackingLogUtils.GetDisclosureTracking2015LogType(this.loanData, log);
    }

    public void AddDisclosureTracking2015toLoanLog(EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log log)
    {
      using (PerformanceMeter.StartNew("LoanDataMgr.AddDisclosureTracking2015toLoanLog", 13672, nameof (AddDisclosureTracking2015toLoanLog), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs"))
      {
        Dictionary<string, string> dataList = new Dictionary<string, string>();
        try
        {
          dataList.Add("ActualLog", DisclosureTrackingLogUtils.GetLog((IDisclosureTrackingLog) log));
          this.SetDisclosureTracking2015LogType((EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log) log);
          string str = "";
          if (this.loanData.Calculator != null)
          {
            try
            {
              if (log.DisclosedForCD)
                str = this.loanData.Calculator.GetUCD(false, true);
              else if (log.DisclosedForLE)
                str = this.loanData.Calculator.GetUCD(true, true);
            }
            catch (Exception ex)
            {
              RemoteLogger.Write(TraceLevel.Warning, string.Format("AddDisclosureTracking2015toLoanLog: Error At GetUCD : {0}", (object) ex.ToString()));
              log.UCDCreationError = true;
            }
          }
          try
          {
            log.UCD = str;
            log.SetItemizationFields(HUDGFE2010Fields.WHOLEPOC_FIELDS, this.loanData);
          }
          catch (Exception ex)
          {
            if (string.IsNullOrWhiteSpace(log.UCD))
              log.UCDCreationError = true;
            RemoteLogger.Write(TraceLevel.Warning, string.Format("AddDisclosureTracking2015toLoanLog: Error At SetItemizationFields() : {0}", (object) ex.ToString()));
          }
          if (!this.loanData.GetLogList().ContainsRecord(log.Guid))
            this.loanData.GetLogList().AddRecord((LogRecordBase) log);
          this.LoanData.DisclosureTracking2015Created(log);
          if (this.loanData.Calculator != null)
            this.loanData.Calculator.CalculateLastDisclosedCDorLE((EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log) log);
          this.AppendCD3FieldsToDTSnapshot((EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log) log);
          if (this.loanData.Calculator != null)
          {
            this.loanData.Calculator.FormCalculation("CD3.X109", (string) null, (string) null);
            this.loanData.Calculator.FormCalculation("CLOSINGDISCLOSUREPAGE3", "", "");
            try
            {
              bool skipLockRequestSync = this.loanData.Calculator.SkipLockRequestSync;
              this.loanData.Calculator.SkipLockRequestSync = true;
              this.loanData.Calculator.CalculateAll(false);
              this.loanData.Calculator.SkipLockRequestSync = skipLockRequestSync;
            }
            catch (Exception ex)
            {
            }
          }
          double appliedCureAmount = 0.0;
          Hashtable triggerFields = (Hashtable) null;
          string cureLogComment = log.GetCureLogComment();
          if ((log.DisclosedForLE && log.DisclosureType == EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log.DisclosureTypeEnum.Revised || log.DisclosedForCD) && cureLogComment != string.Empty && RegulationAlerts.GetGoodFaithFeeVarianceViolationAlert(this.loanData) != null && this.loanData.Calculator != null)
          {
            appliedCureAmount = this.loanData.Calculator.GetRequiredVarianceCureAmount();
            triggerFields = this.loanData.Calculator.GetGFFVarianceAlertDetails();
          }
          if (this.loanData.Calculator != null)
            this.loanData.Calculator.UpdateLogs();
          log.CreateCureLog(appliedCureAmount, cureLogComment, triggerFields, this.sessionObjects.UserID, this.sessionObjects.UserInfo.FullName, true);
          log.PopulateLoanDataList(this.GetInitialUCD((EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log) log));
          this.loanData.RemoveAllGoodFaithChangeOfCircumstance();
          this.loanData.SetField("4462", "");
          this.SaveDisclosureTracking2015();
          DisclosureTrackingLogUtils.WriteLog(dataList, this, this.sessionObjects.UserID, "", "", "", true, log.Guid);
        }
        catch (Exception ex)
        {
          RemoteLogger.Write(TraceLevel.Warning, string.Format("Error at AddDisclosureTracking2015toLoanLog() : {0}", (object) ex.ToString()));
        }
      }
    }

    public void AppendCD3FieldsToDTSnapshot(EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log log)
    {
      EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log disclosureTracking2015Log = (EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log) null;
      if (log.DisclosedForCD)
        disclosureTracking2015Log = this.loanData.GetLogList().GetLatestIDisclosureTracking2015Log(EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log.DisclosureTrackingType.LE);
      string str1 = disclosureTracking2015Log != null ? disclosureTracking2015Log.GetDisclosedField("2") : this.loanData.GetSimpleField("2");
      log.AddDisclosedLoanInfo("CD3.XH87", str1 == "" ? "0.00" : str1);
      string str2 = disclosureTracking2015Log != null ? disclosureTracking2015Log.GetDisclosedField("CD3.X87") : string.Concat((object) EllieMae.EMLite.Common.Utils.ArithmeticRounding(EllieMae.EMLite.Common.Utils.ParseDecimal((object) this.loanData.GetSimpleField("2")), 0));
      log.AddDisclosedLoanInfo("CD3.X87", str2 == "" ? "0.00" : str2);
      string str3 = disclosureTracking2015Log != null ? disclosureTracking2015Log.GetDisclosedField("LE2.XSTJDV") : this.loanData.GetSimpleField("LE2.XSTJDV");
      log.AddDisclosedLoanInfo("CD3.XH88", str3 == "" ? "0.00" : str3);
      log.AddDisclosedLoanInfo("CD3.XH93", str3 == "" ? "0.00" : str3);
      string str4 = disclosureTracking2015Log != null ? disclosureTracking2015Log.GetDisclosedField("CD3.X88") : this.loanData.GetSimpleField("LE2.XSTJ");
      log.AddDisclosedLoanInfo("CD3.X88", str4 == "" ? "0.00" : str4);
      log.AddDisclosedLoanInfo("CD3.X93", str4 == "" ? "0.00" : str4);
      string str5 = disclosureTracking2015Log != null ? (EllieMae.EMLite.Common.Utils.ParseDouble((object) disclosureTracking2015Log.GetDisclosedField("CD3.X80")) == 0.0 ? (EllieMae.EMLite.Common.Utils.ParseDouble((object) disclosureTracking2015Log.GetDisclosedField("LE2.X31")) == 0.0 ? disclosureTracking2015Log.GetDisclosedField("1092") : disclosureTracking2015Log.GetDisclosedField("LE2.X31")) : disclosureTracking2015Log.GetDisclosedField("CD3.X80")) : (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetSimpleField("CD3.X80")) == 0.0 ? (EllieMae.EMLite.Common.Utils.ParseDouble((object) this.loanData.GetSimpleField("LE2.X31")) == 0.0 ? this.loanData.GetSimpleField("1092") : this.loanData.GetSimpleField("LE2.X31")) : this.loanData.GetSimpleField("CD3.X80"));
      log.AddDisclosedLoanInfo("CD3.XH90", str5 == "" ? "0.00" : str5);
      string str6 = disclosureTracking2015Log != null ? disclosureTracking2015Log.GetDisclosedField("CD3.X90") : string.Concat((object) EllieMae.EMLite.Common.Utils.ArithmeticRounding(EllieMae.EMLite.Common.Utils.ParseDecimal((object) this.loanData.GetSimpleField("1092")), 0));
      log.AddDisclosedLoanInfo("CD3.X90", str6 == "" ? "0.00" : str6);
      string str7 = disclosureTracking2015Log != null ? disclosureTracking2015Log.GetDisclosedField("LE2.X1") : this.loanData.GetSimpleField("LE2.X1");
      log.AddDisclosedLoanInfo("CD3.XH95", str7 == "" ? "0.00" : str7);
      string str8 = disclosureTracking2015Log != null ? disclosureTracking2015Log.GetDisclosedField("CD3.X95") : string.Concat((object) EllieMae.EMLite.Common.Utils.ArithmeticRounding(EllieMae.EMLite.Common.Utils.ParseDecimal((object) this.loanData.GetSimpleField("LE2.X1")), 0));
      log.AddDisclosedLoanInfo("CD3.X95", str8 == "" ? "0.00" : str8);
      string str9 = disclosureTracking2015Log != null ? disclosureTracking2015Log.GetDisclosedField("LE2.X2") : this.loanData.GetSimpleField("LE2.X2");
      log.AddDisclosedLoanInfo("CD3.XH96", str9 == "" ? "0.00" : str9);
      string str10 = disclosureTracking2015Log != null ? disclosureTracking2015Log.GetDisclosedField("CD3.X96") : string.Concat((object) EllieMae.EMLite.Common.Utils.ArithmeticRounding(EllieMae.EMLite.Common.Utils.ParseDecimal((object) this.loanData.GetSimpleField("LE2.X2")), 0));
      log.AddDisclosedLoanInfo("CD3.X96", str10 == "" ? "0.00" : str10);
      string str11 = disclosureTracking2015Log != null ? disclosureTracking2015Log.GetDisclosedField("L128") : this.loanData.GetSimpleField("L128");
      log.AddDisclosedLoanInfo("CD3.XH97", str11 == "" ? "0.00" : str11);
      string str12 = disclosureTracking2015Log != null ? disclosureTracking2015Log.GetDisclosedField("CD3.X97") : string.Concat((object) EllieMae.EMLite.Common.Utils.ArithmeticRounding(EllieMae.EMLite.Common.Utils.ParseDecimal((object) this.loanData.GetSimpleField("L128")), 0));
      log.AddDisclosedLoanInfo("CD3.X97", str12 == "" ? "0.00" : str12);
      string str13 = disclosureTracking2015Log != null ? disclosureTracking2015Log.GetDisclosedField("LE2.X3") : this.loanData.GetSimpleField("LE2.X3");
      log.AddDisclosedLoanInfo("CD3.XH98", str13 == "" ? "0.00" : str13);
      string str14 = disclosureTracking2015Log != null ? disclosureTracking2015Log.GetDisclosedField("CD3.X98") : string.Concat((object) EllieMae.EMLite.Common.Utils.ArithmeticRounding(EllieMae.EMLite.Common.Utils.ParseDecimal((object) this.loanData.GetSimpleField("LE2.X3")), 0));
      log.AddDisclosedLoanInfo("CD3.X98", str14 == "" ? "0.00" : str14);
      string str15 = disclosureTracking2015Log == null ? (!this.loanData.IsLocked("LE2.X100") ? this.loanData.GetSimpleField("LE2.X100DV") : this.loanData.GetSimpleField("LE2.X100")) : (!disclosureTracking2015Log.IsFieldLocked("LE2.X100") ? disclosureTracking2015Log.GetDisclosedField("LE2.X100DV") : disclosureTracking2015Log.GetDisclosedField("LE2.X100"));
      log.AddDisclosedLoanInfo("CD3.XH99", str15 == "" ? "0.00" : str15);
      string str16 = disclosureTracking2015Log == null ? string.Concat((object) EllieMae.EMLite.Common.Utils.ArithmeticRounding(EllieMae.EMLite.Common.Utils.ParseDecimal((object) this.loanData.GetSimpleField("LE2.X100")), 0)) : disclosureTracking2015Log.GetDisclosedField("CD3.X99");
      log.AddDisclosedLoanInfo("CD3.X99", str16 == "" ? "0.00" : str16);
      string str17 = disclosureTracking2015Log != null ? disclosureTracking2015Log.GetDisclosedField("LE2.X4") : this.loanData.GetSimpleField("LE2.X4");
      log.AddDisclosedLoanInfo("CD3.XH100", str17 == "" ? "0.00" : str17);
      string str18 = disclosureTracking2015Log != null ? disclosureTracking2015Log.GetDisclosedField("CD3.X100") : string.Concat((object) EllieMae.EMLite.Common.Utils.ArithmeticRounding(EllieMae.EMLite.Common.Utils.ParseDecimal((object) this.loanData.GetSimpleField("LE2.X4")), 0));
      log.AddDisclosedLoanInfo("CD3.X100", str18 == "" ? "0.00" : str18);
      Dictionary<string, string> fields = new Dictionary<string, string>();
      for (int index = 87; index <= 100; ++index)
      {
        if (index != 89 && index != 91 && index != 92 && index != 94)
        {
          fields.Add("CD3.XH" + (object) index, log.GetDisclosedField("CD3.XH" + (object) index));
          this.loanData.SetField("CD3.XH" + (object) index, log.GetDisclosedField("CD3.XH" + (object) index));
        }
      }
      log.AppendFieldValues(fields);
    }

    public Dictionary<string, string> GetInitialUCD(EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log log)
    {
      XmlDocument doc = new XmlDocument();
      if (log.UCD != "")
        doc.LoadXml(log.UCD);
      return new UCDXmlParser(doc).ParseXml();
    }

    public void SaveLockSnapshotRecapture(RecaptureUserDecision userDecision)
    {
      this.loan.SaveLockSnapshotRecapture(new LockSnapshotRecapture()
      {
        LoanGUID = this.loanData.GUID,
        LoanNumber = this.loanData.LoanNumber,
        UserDecision = userDecision,
        UserDecisionTimeStamp = DateTime.Now,
        UserName = this.sessionObjects.UserInfo.Userid
      });
    }

    public void GetLogRequestLogSnapshots(EllieMae.EMLite.DataEngine.Log.LockRequestLog[] lrls)
    {
      if (lrls == null || ((IEnumerable<EllieMae.EMLite.DataEngine.Log.LockRequestLog>) lrls).Count<EllieMae.EMLite.DataEngine.Log.LockRequestLog>() <= 0)
        return;
      Dictionary<string, bool> snapshotGuids = new Dictionary<string, bool>();
      EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log[] idisclosureTracking2015Log = this.loanData.GetLogList().GetAllIDisclosureTracking2015Log(false);
      for (int i = 0; i < lrls.Length; ++i)
      {
        if (string.IsNullOrEmpty(lrls[i].LockRequestSnapshotString))
        {
          EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log disclosureTracking2015Log = ((IEnumerable<EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log>) idisclosureTracking2015Log).FirstOrDefault<EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log>((Func<EllieMae.EMLite.DataEngine.Log.IDisclosureTracking2015Log, bool>) (x => x.Guid == lrls[i].Guid));
          bool flag = disclosureTracking2015Log.DisclosedForCD || disclosureTracking2015Log.DisclosedForLE;
          snapshotGuids.Add(lrls[i].Guid, flag);
        }
      }
      Dictionary<string, Dictionary<string, string>> loanSnapshots = this.GetLoanSnapshots(LogSnapshotType.LockRequest, snapshotGuids);
      for (int i = 0; i < snapshotGuids.Count; ++i)
      {
        if (string.IsNullOrEmpty(lrls[i].LockRequestSnapshotString))
        {
          Dictionary<string, string> d = loanSnapshots.Where<KeyValuePair<string, Dictionary<string, string>>>((Func<KeyValuePair<string, Dictionary<string, string>>, bool>) (s => s.Key.Contains(lrls[i].Guid))).Select<KeyValuePair<string, Dictionary<string, string>>, Dictionary<string, string>>((Func<KeyValuePair<string, Dictionary<string, string>>, Dictionary<string, string>>) (s => s.Value)).FirstOrDefault<Dictionary<string, string>>();
          if (d != null)
          {
            Hashtable lockRequestSnapshot = new Hashtable((IDictionary) d);
            lrls[i].AddLockRequestSnapshot(lockRequestSnapshot);
          }
        }
      }
    }

    private void calculator_FormCalculationTriggered(object sender, EventArgs e)
    {
      if (!((string) sender == "APPLYDDM"))
        return;
      this.CreateDDMTrigger();
      this.DDMTriggerExecute(DDMStartPopulationTrigger.UserRequest, true);
    }

    public void UpdateTPOConnectStatus()
    {
      if (this.loanData.TPOConnectStatus)
        return;
      EllieMae.EMLite.DataEngine.Log.MilestoneLog[] allMilestones = this.loanData.GetLogList().GetAllMilestones();
      List<EllieMae.EMLite.Workflow.Milestone> milestonesList = this.configInfo.MilestonesList;
      foreach (EllieMae.EMLite.DataEngine.Log.MilestoneLog milestoneLog in allMilestones)
      {
        EllieMae.EMLite.DataEngine.Log.MilestoneLog log = milestoneLog;
        EllieMae.EMLite.Workflow.Milestone milestone = milestonesList.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (m => m.MilestoneID.Equals(log.MilestoneID, StringComparison.OrdinalIgnoreCase)));
        if (milestone != null)
          log.TPOConnectStatus = milestone.TPOConnectStatus;
      }
      this.loanData.TPOConnectStatus = true;
    }

    public void CreateDDMTrigger()
    {
      if (this.sessionObjects.StartupInfo == null || !this.sessionObjects.StartupInfo.AllowDDM)
        return;
      DDMTrigger ddmTrigger = (DDMTrigger) null;
      if (this.sessionObjects.DDMTrigger != null)
      {
        ddmTrigger = (DDMTrigger) this.sessionObjects.DDMTrigger;
        FieldProvider fieldProvider = (FieldProvider) ddmTrigger.FieldProvider;
        if (!string.IsNullOrEmpty(ddmTrigger.LoanGUID) && ddmTrigger.LoanGUID == this.loanData.GUID && fieldProvider._Loan == this.loanData)
          return;
      }
      if (ddmTrigger == null || this.loanData.Settings == null || DateTime.Compare(ddmTrigger.DDMLastModifiedDateTime, this.loanData.Settings.DDMLastModifiedDateTime) != 0)
      {
        try
        {
          ddmTrigger = new DDMTrigger(this.sessionObjects, this.loanData.GUID, (IFieldProvider) null, true)
          {
            DDMLastModifiedDateTime = this.loanData.Settings.DDMLastModifiedDateTime
          };
          ddmTrigger.FieldProvider = (IFieldProvider) new FieldProvider(this.loanData, this.sessionObjects, this.configInfo, new DDMDataTableTrigger(this.sessionObjects, ddmTrigger.DDMOnDemandDataTableNames));
          this.sessionObjects.DDMTrigger = (object) ddmTrigger;
        }
        catch (Exception ex)
        {
          Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Error, "DDM - Cannot initialize DDM Trigger. Error: " + ex.Message);
          RemoteLogger.Write(TraceLevel.Info, "DDM - Cannot initialize DDM Trigger. Error: " + ex.Message);
          return;
        }
      }
      else
      {
        FieldProvider fieldProvider = (FieldProvider) ddmTrigger.FieldProvider;
        fieldProvider.Reset(this.loanData, this.configInfo);
        ddmTrigger.Reset(this.loanData.GUID, (IFieldProvider) fieldProvider);
        ddmTrigger.DDMLastModifiedDateTime = this.loanData.Settings.DDMLastModifiedDateTime;
      }
      this.loanData.DDMTrigger = (object) ddmTrigger;
      this.loanData.DDMOnDemandTriggerFields = ddmTrigger.DDMOnDemandTriggerFields;
      this.loanData.DDMOnDemandVirtualFields = ddmTrigger.DDMOnDemandVirtualFields;
    }

    public void DDMTriggerExecute(
      DDMStartPopulationTrigger startType,
      string fieldId = null,
      bool runCalc = false)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 14017, nameof (DDMTriggerExecute), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      if (this.LoanData == null || this.LoanData.DDMTrigger == null)
      {
        PerformanceMeter.Current.AddCheckpoint("EXIT - null", 14020, nameof (DDMTriggerExecute), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      }
      else
      {
        Cursor.Current = Cursors.WaitCursor;
        DDMTrigger ddmTrigger = (DDMTrigger) this.LoanData.DDMTrigger;
        if (ddmTrigger != null && string.Compare(this.LoanData.GUID, ddmTrigger.LoanGUID, true) != 0)
        {
          ddmTrigger.ReassignUnsavedLoanBacktoDDM(this.LoanData, this.ConfigInfo);
          this.executeDDMRules(startType, fieldId, runCalc);
          ddmTrigger.ReassignNewlyCreatedLoanBacktoDDM();
        }
        else
          this.executeDDMRules(startType, fieldId, runCalc);
        Cursor.Current = Cursors.Default;
        PerformanceMeter.Current.AddCheckpoint("END", 14038, nameof (DDMTriggerExecute), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      }
    }

    private void executeDDMRules(DDMStartPopulationTrigger startType, string fieldId = null, bool runCalc = false)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 14045, nameof (executeDDMRules), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        bool flag = false;
        DDMTrigger ddmTrigger = (DDMTrigger) this.LoanData.DDMTrigger;
        if (!this.IsNew() && this.loanData.DDMOnDemandTriggerFields != null && !this.loanData.DDMIsRequired && ddmTrigger.DDMOnDemandVirtualFields != null && ddmTrigger.DDMOnDemandVirtualFields.Count > 0)
        {
          foreach (KeyValuePair<string, string> demandVirtualField in ddmTrigger.DDMOnDemandVirtualFields)
          {
            if (demandVirtualField.Value != this.LoanData.GetSimpleField(demandVirtualField.Key))
            {
              this.loanData.DDMIsRequired = true;
              break;
            }
          }
        }
        if (!this.loanData.DDMIsRequired)
        {
          DateTime date = EllieMae.EMLite.Common.Utils.ParseDate((object) this.loanData.GetField("DMDDM.X1"));
          if (date == DateTime.MinValue)
            this.loanData.DDMIsRequired = true;
          else if (this.LoanData.DDMTrigger != null)
          {
            if (ddmTrigger == null)
              ddmTrigger = (DDMTrigger) this.LoanData.DDMTrigger;
            if (ddmTrigger != null && DateTime.Compare(date, ddmTrigger.DDMLastModifiedDateTime) < 0)
              this.loanData.DDMIsRequired = true;
          }
        }
        if (this.IsNew() || this.loanData.DDMOnDemandTriggerFields == null || this.loanData.DDMIsRequired)
        {
          Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "DDM - Apply DDM before saving loan!");
          if (ddmTrigger == null)
            ddmTrigger = (DDMTrigger) this.LoanData.DDMTrigger;
          flag = ddmTrigger != null && ddmTrigger.Execute(startType, fieldId);
        }
        else
          Tracing.Log(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "DDM - Skip DDM because none of field in DDM setting is changed!");
        if (flag)
        {
          this.loanData.Calculator.AddFeePaidToNameToLoan();
          if (this.LoanData.GetSimpleField("1172") == "FarmersHomeAdministration")
            this.LoanData.Calculator.FormCalculation("USDAMIP");
          this.LoanData.Calculator.EditCalcOnDemandEnum(CalcOnDemandEnum.PaymentSchedule | CalcOnDemandEnum.Update2010ItemizationFrom2015Itemization | CalcOnDemandEnum.FundingFees, true);
          bool skipLockRequestSync = this.loanData.Calculator.SkipLockRequestSync;
          this.LoanData.Calculator.SkipLockRequestSync = true;
          this.LoanData.Calculator.CalcGuiDependentLogicForDDM((ddmTrigger != null ? (FieldProvider) ddmTrigger.FieldProvider : (FieldProvider) null)?.UpdatedFieldIDsByDDM);
          if (runCalc)
            this.LoanData.Calculator.CalculateAll(false);
          this.LoanData.Calculator.SkipLockRequestSync = skipLockRequestSync;
          if (this.AfterDDMApplied != null)
            this.AfterDDMApplied((object) this, new EventArgs());
        }
        this.loanData.Calculator.FeeLinePaidToTrigger = CollectionsUtil.CreateCaseInsensitiveHashtable();
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 14130, nameof (executeDDMRules), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      }
    }

    public void DDMTriggerExecute(DDMStartPopulationTrigger startType, bool runCalc = false)
    {
      this.DDMTriggerExecute(startType, (string) null, runCalc);
    }

    public void GetUpdatedSellConditionSetup()
    {
      if (this.configInfo == null || this.configInfo.SellConditionTrackingSetup != null)
        return;
      this.configInfo.SellConditionTrackingSetup = new SellConditionTrackingSetup();
    }

    public void ValidateBestEfforDailyLimit()
    {
      DateTime serverEasternTime = new EncompassLockDeskHoursHelper((IClientSession) this.SessionObjects.Session, this.SessionObjects, this).GetServerEasternTime();
      DateTime result = new DateTime();
      if (!(this.loanData.GetField("TPO.X15") != "") || !DateTime.TryParse(this.loanData.GetField("2089"), out result) || !(serverEasternTime.Date == result.Date) || string.Compare(this.loanData.GetField("3965"), "Individual Best Efforts", true) != 0)
        return;
      EllieMae.EMLite.DataEngine.Log.LockConfirmLog confirmLockLog = this.loanData.GetLogList().GetConfirmLockLog();
      if (confirmLockLog != null && string.Equals(confirmLockLog.Log.GetLockCurrentStatus(), "Locked", StringComparison.CurrentCultureIgnoreCase) && confirmLockLog != null)
      {
        DateTime dateTimeConfirmed = confirmLockLog.DateTimeConfirmed;
        DateTime? date1 = confirmLockLog?.DateTimeConfirmed.Date;
        DateTime date2 = serverEasternTime.Date;
        if ((date1.HasValue ? (date1.HasValue ? (date1.GetValueOrDefault() == date2 ? 1 : 0) : 1) : 0) != 0)
          return;
      }
      List<ExternalOriginatorManagementData> organizationByTpoid = this.sessionObjects.ConfigurationManager.GetExternalOrganizationByTPOID(this.loanData.GetField("TPO.X15"));
      if (organizationByTpoid.Count <= 0)
        return;
      ExternalOriginatorManagementData originatorManagementData = organizationByTpoid[0];
      if (!originatorManagementData.CommitmentUseBestEffort && !originatorManagementData.CommitmentUseBestEffortLimited)
        return;
      if (originatorManagementData.BestEfforDailyLimitPolicy == ExternalOriginatorBestEffortDailyLimitPolicy.DontAllowLock && originatorManagementData.BestEffortDailyVolumeLimit > 0M)
      {
        double effortDailyLimit = this.sessionObjects.ConfigurationManager.GetBestEffortDailyLimit(this.loanData.GetField("TPO.X15"), serverEasternTime);
        if (originatorManagementData.BestEffortDailyVolumeLimit < (Decimal) (effortDailyLimit + this.loanData.FltVal("2965")))
        {
          if (this.IsFromPlatform)
            throw new ApplicationException(originatorManagementData.DailyLimitWarningMsg);
          throw new OverDailyLimitRateLockRejectedException(originatorManagementData.DailyLimitWarningMsg);
        }
      }
      this.sessionObjects.ConfigurationManager.UpdateBestEffortDailyLimit(this.loanData.GetField("TPO.X15"), serverEasternTime, this.loanData.FltVal("2965"));
    }

    public void ValidateBestEffortDailyLimit(
      ref string rateSheetId,
      string lockDate,
      string deliveryType,
      double loanAmount,
      out bool isUpdate)
    {
      isUpdate = false;
      string field = this.loanData.GetField("TPO.X15");
      DateTime result;
      if (string.IsNullOrWhiteSpace(field) || !DateTime.TryParse(lockDate, out result) || string.Compare(deliveryType, CorrespondentMasterDeliveryType.IndividualBestEfforts.ToDescription(), true) != 0)
        return;
      ExternalOriginatorManagementData originatorManagementData = this.sessionObjects.ConfigurationManager.GetExternalOrganizationByTPOID(field).FirstOrDefault<ExternalOriginatorManagementData>();
      if (originatorManagementData == null)
        return;
      rateSheetId = !originatorManagementData.ResetLimitForRatesheetId || string.IsNullOrWhiteSpace(rateSheetId) ? string.Empty : rateSheetId.Trim();
      if (originatorManagementData.BestEffortDailyVolumeLimit > 0M && originatorManagementData.BestEfforDailyLimitPolicy == ExternalOriginatorBestEffortDailyLimitPolicy.DontAllowLock)
      {
        double effortDailyLimit = this.sessionObjects.ConfigurationManager.GetBestEffortDailyLimit(field, result, rateSheetId, this.loanData.GUID);
        if (originatorManagementData.BestEffortDailyVolumeLimit < (Decimal) (effortDailyLimit + loanAmount))
        {
          if (this.IsFromPlatform)
            throw new ApplicationException(originatorManagementData.DailyLimitWarningMsg);
          throw new OverDailyLimitRateLockRejectedException(originatorManagementData.DailyLimitWarningMsg);
        }
      }
      isUpdate = true;
    }

    private void ResetBestEffortDailyLimit(Hashtable requestSnapshot)
    {
      string guid = this.loanData.GUID;
      string field = this.loanData.GetField("TPO.X15");
      string s = requestSnapshot.ContainsKey((object) "2149") ? Convert.ToString(requestSnapshot[(object) "2149"]) : "";
      string strA = requestSnapshot.ContainsKey((object) "3911") ? Convert.ToString(requestSnapshot[(object) "3911"]) : "";
      double loanAmount = requestSnapshot.ContainsKey((object) "2965") ? EllieMae.EMLite.Common.Utils.ParseDouble(requestSnapshot[(object) "2965"]) : 0.0;
      DateTime result;
      if (string.IsNullOrWhiteSpace(field) || !DateTime.TryParse(s, out result) || string.Compare(strA, CorrespondentMasterDeliveryType.IndividualBestEfforts.ToDescription(), true) != 0)
        return;
      this.sessionObjects.ConfigurationManager.ResetBestEffortDailyLimit(field, result, loanAmount, guid);
    }

    public bool AllowNewLockOutsideLDHours()
    {
      return this.SessionObjects.StartupInfo.ProductPricingPartner == null && (bool) this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.AllowNewLockOutsideLDHours"] & this.loanData.GetField("LOCKRATE.RATESTATUS") == "Locked";
    }

    public void SaveProgressForPendingLockRequest(
      EllieMae.EMLite.DataEngine.Log.LockRequestLog lockLog,
      Hashtable dataTables,
      UserInfo userInfo)
    {
      lockLog.AddLockRequestSnapshot(dataTables);
    }

    public void SaveForUpdateSellComparison(
      EllieMae.EMLite.DataEngine.Log.LockRequestLog requestLog,
      Hashtable requestSnapshot,
      UserInfo userInfo,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan,
      bool alertLoanOfficer = false)
    {
      using (Tracing.StartTimer(LoanDataMgr.sw, nameof (LoanDataMgr), TraceLevel.Info, "LoanDataMgr.SaveForUpdateSellComparison"))
      {
        requestSnapshot[(object) "2592"] = (object) this.SessionObjects.Session.ServerTime;
        requestLog.AddLockRequestSnapshot(requestSnapshot);
        requestLog.DisplayInLog = false;
        this.UpdateLockRequestLog(requestLog, requestSnapshot);
        this.loanData.GetLogList().AddRecord((LogRecordBase) requestLog);
        this.SyncSellComparisonToLoan(requestSnapshot, syncOption);
        this.updateOldLockStatus(requestLog, (List<string>) null);
        EllieMae.EMLite.DataEngine.Log.LockConfirmLog lockConfirmLog = this.CreateLockConfirmLog(requestLog, requestSnapshot, userInfo, alertLoanOfficer);
        this.loanData.GetLogList().AddRecord((LogRecordBase) lockConfirmLog);
        this.LoanData.TriggerCalculation("761", this.LoanData.GetField("761"));
      }
    }

    private void UpdateLockRequestLog(EllieMae.EMLite.DataEngine.Log.LockRequestLog requestLog, Hashtable requestSnapshot)
    {
      requestLog.NumOfDaysLocked = !requestSnapshot.ContainsKey((object) "2221") ? 0 : EllieMae.EMLite.Common.Utils.ParseInt(requestSnapshot[(object) "2221"], 0);
      requestLog.SellSideExpirationDate = !requestSnapshot.ContainsKey((object) "2222") ? DateTime.MinValue : EllieMae.EMLite.Common.Utils.ParseDate(requestSnapshot[(object) "2222"], DateTime.MinValue);
      requestLog.BuySideExpirationDate = !requestSnapshot.ContainsKey((object) "2151") ? DateTime.MinValue : EllieMae.EMLite.Common.Utils.ParseDate(requestSnapshot[(object) "2151"], DateTime.MinValue);
      if (requestSnapshot.ContainsKey((object) "2297"))
      {
        requestLog.SellSideDeliveryDate = EllieMae.EMLite.Common.Utils.ParseDate(requestSnapshot[(object) "2297"], DateTime.MinValue);
        requestLog.SellSideDeliveredBy = string.Concat(requestSnapshot[(object) "2278"]);
      }
      else
      {
        requestLog.SellSideDeliveryDate = DateTime.MinValue;
        requestLog.SellSideDeliveredBy = "";
      }
      requestLog.BuySideNumDayExtended = !requestSnapshot.ContainsKey((object) "3363") ? 0 : EllieMae.EMLite.Common.Utils.ParseInt(requestSnapshot[(object) "3363"], 0);
      requestLog.BuySideNumDayLocked = !requestSnapshot.ContainsKey((object) "2150") ? 0 : EllieMae.EMLite.Common.Utils.ParseInt(requestSnapshot[(object) "2150"], 0);
      requestLog.CumulatedDaystoExtend = !requestSnapshot.ContainsKey((object) "3431") ? 0 : EllieMae.EMLite.Common.Utils.ParseInt(requestSnapshot[(object) "3431"], 0);
      requestLog.InvestorName = !requestSnapshot.ContainsKey((object) "2278") ? string.Empty : string.Concat(requestSnapshot[(object) "2278"]);
      requestLog.BuySideNewLockExtensionDate = !requestSnapshot.ContainsKey((object) "3364") ? DateTime.MinValue : EllieMae.EMLite.Common.Utils.ParseDate(requestSnapshot[(object) "3364"], DateTime.MinValue);
      requestLog.SellSideNewLockExtensionDate = !requestSnapshot.ContainsKey((object) "3367") ? DateTime.MinValue : EllieMae.EMLite.Common.Utils.ParseDate(requestSnapshot[(object) "3367"], DateTime.MinValue);
      if (requestSnapshot.ContainsKey((object) "3366"))
        requestLog.SellSideNumDayExtended = EllieMae.EMLite.Common.Utils.ParseInt(requestSnapshot[(object) "3366"], 0);
      else
        requestLog.SellSideNumDayExtended = 0;
    }

    public EllieMae.EMLite.DataEngine.Log.LockConfirmLog CreateLockConfirmLog(
      EllieMae.EMLite.DataEngine.Log.LockRequestLog requestLog,
      Hashtable requestSnapshot,
      UserInfo userInfo,
      bool alertLoanOfficer = false)
    {
      EllieMae.EMLite.DataEngine.Log.LockConfirmLog lockConfirmLog = new EllieMae.EMLite.DataEngine.Log.LockConfirmLog();
      lockConfirmLog.BuySideExpirationDate = requestLog.BuySideExpirationDate;
      lockConfirmLog.SetConfirmingUser(userInfo);
      DateTime serverTime = this.SessionObjects.Session.ServerTime;
      lockConfirmLog.Date = EllieMae.EMLite.Common.Utils.ParseDate(requestSnapshot[(object) "2592"], serverTime);
      lockConfirmLog.DateConfirmed = requestSnapshot[(object) "2592"].ToString();
      lockConfirmLog.RequestGUID = requestLog.Guid;
      lockConfirmLog.SellSideDeliveryDate = requestLog.SellSideDeliveryDate;
      lockConfirmLog.SellSideDeliveredBy = requestLog.SellSideDeliveredBy;
      lockConfirmLog.SellSideExpirationDate = requestLog.SellSideExpirationDate;
      lockConfirmLog.AlertLoanOfficer = alertLoanOfficer;
      lockConfirmLog.CommitmentTermEnabled = EllieMae.EMLite.Common.Utils.ParseBoolean(this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableCommitmentTermFields"]);
      lockConfirmLog.EnableZeroParPricingRetail = EllieMae.EMLite.Common.Utils.ParseBoolean(this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingRetail"]);
      lockConfirmLog.EnableZeroParPricingWholesale = EllieMae.EMLite.Common.Utils.ParseBoolean(this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingWholesale"]);
      lockConfirmLog.Voided = requestLog.Voided;
      lockConfirmLog.DisplayInLog = false;
      lockConfirmLog.IncludeConfirmCnt = false;
      return lockConfirmLog;
    }

    private void InitializeNgLoan()
    {
      this.loanData?.FieldChangeTracker?.Clear();
      if (!this.sessionObjects.StartupInfo.UpdateXDBBasedOnFieldChanges)
        return;
      PerformanceMeter performanceMeter;
      Func<Loan> factory;
      if (this.IsNew())
      {
        string guid = this.loanData.GUID;
        performanceMeter = PerformanceMeter.StartNew("InitializeNGLoanTask", "InitializeNGLoanTask", true, 14433, nameof (InitializeNgLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        performanceMeter?.AddCheckpoint("Create Task to Initialize NG Loan start", 14434, nameof (InitializeNgLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        factory = (Func<Loan>) (() => Loan.Create(Guid.Parse(guid)));
      }
      else
      {
        if (this.loanLock == LoanInfo.LockReason.NotLocked || this.loanData == null)
        {
          this.ngLoanCreateTask = (Task) null;
          this.ngLoan = (Loan) null;
          return;
        }
        if (this.ngLoanCreateTask != null)
          return;
        performanceMeter = PerformanceMeter.StartNew("InitializeNGLoanTask", "InitializeNGLoanTask", true, 14448, nameof (InitializeNgLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        performanceMeter?.AddCheckpoint("Create Task to Initialize NG Loan start", 14449, nameof (InitializeNgLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        XmlDocument loanXml = (XmlDocument) this.loanData.ToXmlDocument().CloneNode(true);
        factory = (Func<Loan>) (() => this.ConvertFromLoanXml(loanXml));
      }
      this.ngLoanCreateTask = Task.Run((Action) (() =>
      {
        try
        {
          this.ngLoan = factory();
        }
        catch (Exception ex)
        {
          DiagUtility.LogManager.GetLogger(LoanDataMgr.sw).Write(Encompass.Diagnostics.Logging.LogLevel.ERROR, nameof (LoanDataMgr), "Exception while initializing NG Loan.", ex);
        }
      }));
      performanceMeter?.AddCheckpoint("Create Task to Initialize NG Loan end", 14467, nameof (InitializeNgLoan), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
    }

    private bool PopulateFieldChangesUsingNgDiff(PerformanceMeter pm)
    {
      if (!this.sessionObjects.StartupInfo.UpdateXDBBasedOnFieldChanges || this.ngLoanCreateTask == null)
      {
        this.loanData.FieldChangeTracker = new FieldChangeTracker()
        {
          IgnoreFieldChangeTrackingForXDB = true
        };
        return false;
      }
      this.ngLoanCreateTask.Wait(8000);
      if (this.ngLoan == null)
      {
        this.loanData.FieldChangeTracker = new FieldChangeTracker()
        {
          IgnoreFieldChangeTrackingForXDB = true
        };
        return false;
      }
      pm?.AddCheckpoint("PopulateFieldChangesUsingNgDiff start", 14491, nameof (PopulateFieldChangesUsingNgDiff), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
      try
      {
        Dictionary<string, LoanXDBField> fields = ((IEnumerable<LoanXDBField>) this.sessionObjects.LoanManager.GetLoanXDBTableList(false).GetAllFields()).ToDictionary<LoanXDBField, string, LoanXDBField>((Func<LoanXDBField, string>) (field => field.FieldIDWithCoMortgagor), (Func<LoanXDBField, LoanXDBField>) (field => field));
        if (fields.Count == 0)
        {
          this.LoanData.FieldChangeTracker = new FieldChangeTracker()
          {
            IgnoreFieldChangeTrackingForXDB = true
          };
          return false;
        }
        List<VirtualField> fieldDefs = new List<VirtualField>();
        foreach (string key in fields.Keys)
        {
          VirtualField field = VirtualFields.GetField(key);
          if (field != null)
            fieldDefs.Add(field);
        }
        Dictionary<string, string> oldVirtualFields = (Dictionary<string, string>) null;
        if (!this.IsNew())
          oldVirtualFields = LoanDataMgr.SetVirtualFields(this.ngLoan, (IEnumerable<FieldDefinition>) fieldDefs, (Dictionary<string, string>) null, false);
        this.ngLoan.StartFieldChangeTracking();
        this.ConvertFromLoanXml(this.loanData.ToXmlDocument(), this.ngLoan);
        LoanDataMgr.SetVirtualFields(this.ngLoan, (IEnumerable<FieldDefinition>) fieldDefs, oldVirtualFields, true);
        this.ngLoan.StopFieldChangeTracking();
        this.LoanData.FieldChangeTracker = new FieldChangeTracker()
        {
          FieldChanges = this.ngLoan.GetModifiedFieldIds().Where<KeyValuePair<string, Elli.Domain.Entity.FieldTrackingValue>>((Func<KeyValuePair<string, Elli.Domain.Entity.FieldTrackingValue>, bool>) (item => fields.ContainsKey(item.Key))).ToDictionary<KeyValuePair<string, Elli.Domain.Entity.FieldTrackingValue>, string, FieldChangeInfo>((Func<KeyValuePair<string, Elli.Domain.Entity.FieldTrackingValue>, string>) (item => item.Key), (Func<KeyValuePair<string, Elli.Domain.Entity.FieldTrackingValue>, FieldChangeInfo>) (item => new FieldChangeInfo(item.Key, this.GetBorrowerPairForIndex(this.LoanData, item.Value.PairIndex), item.Value.PriorValue, item.Value.NewValue))),
          IgnoreFieldChangeTrackingForXDB = false,
          IncludesVirtualFields = true,
          UseFieldChangesValues = true
        };
        this.ngLoan.ClearModifiedFieldIds();
        pm?.AddCheckpoint("PopulateFieldChangesUsingNgDiff End", 14537, nameof (PopulateFieldChangesUsingNgDiff), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        return true;
      }
      catch (Exception ex)
      {
        DiagUtility.LogManager.GetLogger(LoanDataMgr.sw).Write(Encompass.Diagnostics.Logging.LogLevel.ERROR, nameof (LoanDataMgr), "Exception while computing the field changes.", ex);
        this.loanData.FieldChangeTracker = new FieldChangeTracker()
        {
          IgnoreFieldChangeTrackingForXDB = true
        };
        pm?.AddCheckpoint("PopulateFieldChangesUsingNgDiff Failed", 14550, nameof (PopulateFieldChangesUsingNgDiff), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        return false;
      }
    }

    private BorrowerPair GetBorrowerPairForIndex(LoanData loanData, int pairIndex)
    {
      BorrowerPair[] borrowerPairs = loanData.GetBorrowerPairs();
      return pairIndex < 0 || pairIndex > borrowerPairs.Length - 1 ? (BorrowerPair) null : borrowerPairs[pairIndex];
    }

    private Loan ConvertFromLoanXml(XmlDocument loanXml, Loan loan = null)
    {
      string str = this.ngLoan == null ? "InitializeNGLoan" : "ApplyChangesOnNGLoan";
      using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew(str, str, true, 14568, nameof (ConvertFromLoanXml), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs"))
      {
        performanceMeter.AddCheckpoint(string.Format("{0} start", (object) str), 14571, nameof (ConvertFromLoanXml), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        loan = new Encompass360XmlFormat(loanXml).ReadFile(loan, this.loanData.Settings);
        performanceMeter.AddCheckpoint(string.Format("{0} stop", (object) str), 14575, nameof (ConvertFromLoanXml), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DataEngine\\LoanDataMgr.cs");
        return loan;
      }
    }

    private static Dictionary<string, string> SetVirtualFields(
      Loan loan,
      IEnumerable<FieldDefinition> fieldDefs,
      Dictionary<string, string> oldVirtualFields,
      bool trackChanges)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      foreach (FieldDefinition fieldDef in fieldDefs)
      {
        if (fieldDef is VirtualField field && !field.MultiInstance)
        {
          string str1 = (string) null;
          try
          {
            str1 = Elli.Domain.VirtualFieldCalcs.VirtualFields.GetValue(field, loan, loan.LoanSettings as ILoanSettings);
            if (string.IsNullOrWhiteSpace(str1))
              str1 = (string) null;
          }
          catch (Exception ex)
          {
          }
          if (trackChanges)
          {
            string str2;
            if (oldVirtualFields == null || !oldVirtualFields.TryGetValue(field.FieldID, out str2))
              str2 = (string) null;
            if (!string.Equals(str2, str1))
              loan.AddModifiedFieldId(field.FieldID, field.FieldID, 0, str2, str1, (object) str2, (object) str1, string.Empty);
          }
          if (str1 != null)
            dictionary[field.FieldID] = str1;
        }
      }
      return dictionary;
    }

    public void UpdateLockValidationStatus(EllieMae.EMLite.DataEngine.Log.LockRequestLog lrl, bool allowCalc = true)
    {
      if (!allowCalc || this.LockComparisonFieldsModified.Count <= 0 || lrl == null)
      {
        this.LockComparisonFieldsModified.Clear();
      }
      else
      {
        Hashtable lockRequestSnapshot = lrl.GetLockRequestSnapshot();
        string field = this.loanData.GetField("4788");
        this.CalcLockValidationStatus(lockRequestSnapshot, this.sessionObjects.StartupInfo.LockComparisonFields);
        if (string.Compare(field, this.loanData.GetField("4788"), StringComparison.CurrentCultureIgnoreCase) != 0 && !this.IsFromPlatform)
          this.addLockValidationLog(lrl.Guid);
        this.LockComparisonFieldsModified.Clear();
      }
    }

    private void addLockValidationLog(string lrlGuid)
    {
      EllieMae.EMLite.DataEngine.Log.LockValidationLog rec = new EllieMae.EMLite.DataEngine.Log.LockValidationLog();
      DateTime serverTime = this.SessionObjects.Session.ServerTime;
      rec.Date = serverTime;
      rec.SetStatusChangedByUser(this.sessionObjects.UserInfo);
      rec.LockValidationStatus = this.loanData.GetField("4788");
      rec.RequestGUID = lrlGuid;
      rec.DisplayInLog = false;
      this.loanData.GetLogList().AddRecord((LogRecordBase) rec);
    }

    private void CalcLockValidationStatus(
      Hashtable snapshot,
      IList<LockComparisonField> lockComparisonFields)
    {
      if ((lockComparisonFields != null ? (!lockComparisonFields.Any<LockComparisonField>() ? 1 : 0) : 1) != 0)
        return;
      bool flag = false;
      foreach (LockComparisonField lockComparisonField in (IEnumerable<LockComparisonField>) lockComparisonFields)
      {
        string loanFieldId = lockComparisonField.LoanFieldId;
        string lockRequestFieldId = lockComparisonField.LockRequestFieldId;
        string strA = this.loanData.GetField(loanFieldId);
        string strB = snapshot.ContainsKey((object) lockRequestFieldId) ? snapshot[(object) lockRequestFieldId].ToString() : string.Empty;
        if (string.Compare(strA, strB, true) != 0)
        {
          FieldDefinition field = EncompassFields.GetField(loanFieldId);
          if (field != null)
          {
            string format = field.Format.ToString();
            strA = this.formatValue(format, field.FormatValue(strA));
            strB = this.formatValue(format, field.FormatValue(strB));
          }
          if (string.Compare(lockRequestFieldId, "3875", true) == 0 && !string.IsNullOrWhiteSpace(strB) && !string.IsNullOrWhiteSpace(strA) && !LockUtils.GetZeroBasedParPricingSetting(this.sessionObjects, this.loanData))
          {
            if (EllieMae.EMLite.Common.Utils.ParseDecimal((object) strB) == 100M - EllieMae.EMLite.Common.Utils.ParseDecimal((object) strA))
              continue;
          }
          else if (string.Compare(strA, strB, true) == 0)
            continue;
          flag = true;
          break;
        }
      }
      if (flag)
        this.loanData.SetField("4788", "Needs Validation");
      else
        this.loanData.SetField("4788", string.Empty);
    }

    private void ClearLockValidationStatus(string lrlGuid)
    {
      if (!string.IsNullOrWhiteSpace(this.LoanData.GetField("4788")))
      {
        this.LoanData.SetField("4788", string.Empty);
        this.addLockValidationLog(lrlGuid);
      }
      this.LockComparisonFieldsModified.Clear();
    }

    private string formatValue(string format, string fieldValue)
    {
      string str = fieldValue;
      if (format.Equals("date", StringComparison.InvariantCultureIgnoreCase))
      {
        if (fieldValue == "01/01/1900")
          str = "//";
        return str;
      }
      if (fieldValue != null && fieldValue.Trim().Length == 0 && (format.Equals("integer", StringComparison.InvariantCultureIgnoreCase) || format.IndexOf("decimal", StringComparison.InvariantCultureIgnoreCase) >= 0))
      {
        int result = 0;
        string[] strArray = format.Split('_');
        if (strArray.Length == 1 || int.TryParse(strArray[strArray.Length - 1], out result))
          str = string.Format("{0:f" + (object) result + "}", (object) 0);
      }
      return str;
    }

    public enum ImportSource
    {
      None,
      CalyxPoint,
      FannieMae,
      Contour,
      Genesis,
    }

    public enum ImmediateExclusiveLockType
    {
      NoLock,
      Exclusive,
      NonExclusive,
    }

    public enum LockLoanSyncOption
    {
      noSync,
      syncLockToLoan,
      syncLoanToLoan,
    }
  }
}
