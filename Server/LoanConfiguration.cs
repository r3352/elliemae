// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanConfiguration
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.Server.ServerObjects.Acl;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using EllieMae.EMLite.Server.ServerObjects.eFolder;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class LoanConfiguration
  {
    public static AlertSetupData GetAlertSetupData()
    {
      using (PerformanceMeter.Current.BeginOperation(nameof (GetAlertSetupData)))
        return new AlertSetupData()
        {
          MilestoneAlertMessages = WorkflowBpmDbAccessor.GetAllMilestoneAlertMessages(),
          Roles = WorkflowBpmDbAccessor.GetAllRoleFunctions(),
          AlertConfigList = AlertConfigAccessor.GetAlertConfigList()
        };
    }

    public static ILoanSettings GetLoanSettings()
    {
      using (PerformanceMeter.Current.BeginOperation(nameof (GetLoanSettings)))
      {
        ClientContext current = ClientContext.GetCurrent();
        ClientLoanSettings loanSettings = new ClientLoanSettings();
        loanSettings.SystemID = EncompassSystemDbAccessor.GetEncompassSystemInfo().SystemID;
        loanSettings.FieldSettings = LoanConfiguration.GetFieldSettings();
        loanSettings.AlertSetupData = LoanConfiguration.GetAlertSetupData();
        loanSettings.ComplianceSettings = current.Settings.GetServerSettings("Compliance");
        loanSettings.AlertSettings = current.Settings.GetServerSettings("Alert");
        loanSettings.DocumentDateTimeType = CalendarTypeConverter.ToDateTimeType((AutoDayCountSetting) current.Settings.GetServerSetting("Policies.DocumentTrackingDayCount"));
        AutoDayCountSetting serverSetting = (AutoDayCountSetting) current.Settings.GetServerSetting("Policies.MilestoneExpDayCount");
        loanSettings.MilestoneDateTimeType = CalendarTypeConverter.ToDateTimeType(serverSetting);
        loanSettings.MilestoneDateCalculator = (IMilestoneDateCalculator) new MilestoneDateCalculator(BusinessCalendarAccessor.GetBusinessCalendar(CalendarType.Business), serverSetting);
        loanSettings.Use10DigitLockSecondaryTradeFields = (bool) current.Settings.GetServerSetting("Policies.Use10DigitDecimalPricing");
        loanSettings.DDMLastModifiedDateTime = Utils.ParseDate((object) Company.GetCompanySettingFromDB("DDM", "LastModifiedDateTime"));
        loanSettings.MigrationData = LoanConfiguration.GetMigrationData();
        loanSettings.AllRoles = WorkflowBpmDbAccessor.GetAllRoleFunctions();
        loanSettings.EnableTempBuyDown = string.Compare(Company.GetCompanySetting("CLIENT", "EnableTempBuyDown"), "True", true) == 0;
        loanSettings.LoanAmountRounding = string.Compare(Company.GetCompanySetting("Policies", "LOANAMOUNTROUNDING"), "Enabled", true) == 0;
        loanSettings.Use5DecimalsForIndexRates = string.Compare(Company.GetCompanySetting("Policies", "ARMIndexPrecision"), "FiveDecimals", true) == 0;
        loanSettings.AllowHybridWithENoteClosing = string.Compare(Company.GetCompanySetting("eClose", "AllowHybridWithENoteClosing"), "Enabled", true) == 0;
        loanSettings.LoanExternalFields = (Func<string, Dictionary<string, string>>) (loanGuid => !string.Equals(Company.GetCompanySetting("LICENSE", "AIQINTEGRATIONMODEL"), "Marshall", StringComparison.InvariantCultureIgnoreCase) ? (Dictionary<string, string>) null : LoanExternalFieldsAccessor.GetLoanExternalFields(loanGuid));
        return (ILoanSettings) loanSettings;
      }
    }

    public static ILoanSettings GetLoanSettings(UserInfo userInfo, string hmdaProfileID = null)
    {
      ILoanSettings loanSettings = LoanConfiguration.GetLoanSettings();
      if (loanSettings != null)
      {
        OrgInfo orgInfo = OrganizationStore.GetLatestVersion(userInfo.OrgId).GetOrganizationInfo();
        if (string.IsNullOrEmpty(hmdaProfileID))
        {
          if (orgInfo != null)
          {
            if (orgInfo.HMDAProfileId <= 0)
              orgInfo = OrganizationStore.GetFirstOrganizationWithLEI(orgInfo.Oid);
            if (orgInfo != null && orgInfo.HMDAProfileId > 0)
            {
              HMDAProfile hmdaProfileById = HMDAProfileDbAccessor.GetHMDAProfileById(orgInfo.HMDAProfileId);
              if (hmdaProfileById != null)
              {
                ((ClientLoanSettings) loanSettings).HMDAInfo = new HMDAInformation(hmdaProfileById.HMDAProfileSetting);
                ((ClientLoanSettings) loanSettings).HMDAInfo.HMDAProfileID = hmdaProfileById.HMDAProfileID.ToString();
              }
            }
          }
        }
        else
        {
          HMDAProfile hmdaProfileById = HMDAProfileDbAccessor.GetHMDAProfileById(Utils.ParseInt((object) hmdaProfileID));
          if (hmdaProfileById != null)
          {
            ((ClientLoanSettings) loanSettings).HMDAInfo = new HMDAInformation(hmdaProfileById.HMDAProfileSetting);
            ((ClientLoanSettings) loanSettings).HMDAInfo.HMDAProfileID = hmdaProfileById.HMDAProfileID.ToString();
          }
        }
      }
      return loanSettings;
    }

    public static FieldSettings GetFieldSettings()
    {
      using (PerformanceMeter.Current.BeginOperation(nameof (GetFieldSettings)))
        return new FieldSettings()
        {
          CustomFields = SystemConfiguration.GetLoanCustomFields(),
          LockRequestAdditionalFields = SecondaryRegistrationAccessor.GetLRAdditionalFields()
        };
    }

    public static LoanMigrationData GetMigrationData()
    {
      using (PerformanceMeter.Current.BeginOperation(nameof (GetMigrationData)))
      {
        RoleInfo[] allRoleFunctions = WorkflowBpmDbAccessor.GetAllRoleFunctions();
        Hashtable hashtable = new Hashtable();
        foreach (RoleInfo roleInfo in allRoleFunctions)
          hashtable[(object) roleInfo.RoleID] = (object) new LoanMigrationData.RoleData(roleInfo.RoleID, roleInfo.RoleName);
        RolesMappingInfo[] roleMappingInfos = WorkflowBpmDbAccessor.GetAllRoleMappingInfos();
        Hashtable insensitiveHashtable1 = CollectionsUtil.CreateCaseInsensitiveHashtable();
        foreach (RolesMappingInfo rolesMappingInfo in roleMappingInfos)
        {
          if (rolesMappingInfo.RoleIDList.Length != 0)
          {
            LoanMigrationData.RoleData roleData = (LoanMigrationData.RoleData) hashtable[(object) rolesMappingInfo.RoleIDList[0]];
            if (roleData != null)
            {
              switch (rolesMappingInfo.RealWorldRoleID)
              {
                case RealWorldRoleID.LoanOfficer:
                  roleData.RealWorldRoleType = "LO";
                  break;
                case RealWorldRoleID.LoanProcessor:
                  roleData.RealWorldRoleType = "LP";
                  break;
                case RealWorldRoleID.LoanCloser:
                  roleData.RealWorldRoleType = "CL";
                  break;
              }
              if (roleData.RealWorldRoleType != "")
                insensitiveHashtable1[(object) roleData.RealWorldRoleType] = (object) roleData;
            }
          }
        }
        Hashtable insensitiveHashtable2 = CollectionsUtil.CreateCaseInsensitiveHashtable();
        foreach (DictionaryEntry milestoneRole in WorkflowBpmDbAccessor.GetMilestoneRoles())
        {
          RoleSummaryInfo roleSummaryInfo = (RoleSummaryInfo) milestoneRole.Value;
          LoanMigrationData.MilestoneData milestoneData = new LoanMigrationData.MilestoneData();
          milestoneData.MilestoneID = milestoneRole.Key.ToString();
          LoanMigrationData.RoleData roleData = (LoanMigrationData.RoleData) hashtable[(object) roleSummaryInfo.RoleID];
          if (roleData != null)
            milestoneData.AssociatedRole = roleData;
          insensitiveHashtable2[milestoneRole.Key] = (object) milestoneData;
        }
        List<EllieMae.EMLite.Workflow.Milestone> list = WorkflowBpmDbAccessor.GetMilestones(false).ToList<EllieMae.EMLite.Workflow.Milestone>();
        MilestoneTemplate milestoneTemplate = WorkflowBpmDbAccessor.GetDefaultMilestoneTemplate();
        return new LoanMigrationData(insensitiveHashtable1, list, milestoneTemplate, insensitiveHashtable2);
      }
    }

    public static LoanAlertMonitor GetLoanAlertMonitor()
    {
      return new LoanAlertMonitor(BusinessCalendarAccessor.GetBusinessCalendar(CalendarType.Business), BusinessCalendarAccessor.GetBusinessCalendar(CalendarType.Postal), LoanConfiguration.GetAlertSetupData());
    }

    public static ILoanConfigurationInfo GetLoanConfigurationInfo(
      UserInfo userInfo,
      LoanConfigurationParameters configParams = null,
      string loanFolder = null,
      string loanName = null,
      string hmdaProfileID = null)
    {
      ClientContext current = ClientContext.GetCurrent();
      ILoanConfigurationInfo configurationInfo = (ILoanConfigurationInfo) new LoanConfigurationInfo();
      configurationInfo.LoanSettings = LoanConfiguration.GetLoanSettings(userInfo, hmdaProfileID);
      configurationInfo.CompanyInfo = Company.GetCompanyInfo();
      configurationInfo.DefaultDocumentAccessRules = DocumentAccessRulesDbAccessor.GetDocumentDefaultAccessRules();
      configurationInfo.AllRoles = WorkflowBpmDbAccessor.GetAllRoleFunctions();
      configurationInfo.MilestonesList = WorkflowBpmDbAccessor.GetMilestones(false).ToList<EllieMae.EMLite.Workflow.Milestone>();
      configurationInfo.UserOrganiation = OrganizationStore.GetLatestVersion(userInfo.OrgId).GetOrganizationInfo();
      configurationInfo.DisplayOrganization = OrganizationStore.GetFirstAvaliableOrganization(userInfo.OrgId);
      configurationInfo.RoleMappings = WorkflowBpmDbAccessor.GetAllRoleMappingInfos();
      configurationInfo.UserRoleMappings = WorkflowBpmDbAccessor.GetUsersRoleMapping(userInfo.Userid);
      configurationInfo.ImageAttachmentSettings = ImageAttachmentSettingsStore.GetImageAttachmentSettings();
      configurationInfo.WebCenterSettings = WebCenterSettingsStore.GetWebCenterSettings();
      configurationInfo.DocumentTrackingSetup = DocumentTrackingConfiguration.GetDocumentTrackingSetup(userInfo);
      configurationInfo.DocumentGroupSetup = SystemConfigurationAccessor.GetDocumentGroupSetup();
      if (Company.GetCurrentEdition() != EncompassEdition.Broker)
      {
        configurationInfo.UnderwritingConditionTrackingSetup = (UnderwritingConditionTrackingSetup) UnderwritingConditionListAccessor.GetUnderwritingConditionList();
        configurationInfo.PostClosingConditionTrackingSetup = (PostClosingConditionTrackingSetup) PostClosingConditionListAccessor.GetPostClosingConditionList();
        configurationInfo.SellConditionTrackingSetup = UnderwritingConditionListAccessor.GetSellConditionList();
      }
      configurationInfo.DefaultStackingOrderTemplateName = Company.GetCompanySetting("StackingOrder", "Default");
      configurationInfo.FormPrintGroupDefault = FormPrintGroupDefault.All;
      configurationInfo.LockLoanNumber = (bool) current.Settings.GetServerSetting("Policies.LockLoanNumber");
      configurationInfo.InterviewerPopulation = (InterviewerInfoSetting) current.Settings.GetServerSetting("Policies.InterviewerPopulation");
      configurationInfo.DisplayBusinessRuleOption = (EnableDisableSetting) current.Settings.GetServerSetting("Components.DisplayBusinessRuleOption");
      configurationInfo.ContactHistoryUpdateMilestoneID = string.Concat(current.Settings.GetServerSetting("Policies.ContactUpdateMilestone"));
      configurationInfo.ClosedLoanBillingMilestoneID = string.Concat(current.Settings.GetServerSetting("License.ClosedLoanMilestone"));
      configurationInfo.TasksSetup = MilestoneTaskAccessor.GetMilestoneTasks();
      configurationInfo.UserAclGroups = AclGroupAccessor.GetGroupsOfUser(userInfo.Userid);
      configurationInfo.AllForms = InputForms.GetAllFormInfos();
      configurationInfo.ChannelOptions = SystemConfiguration.GetChannelOption();
      configurationInfo.LoanCompDefaultPlan = LOCompAccessor.GetDefaultLoanCompPlan();
      configurationInfo.IsDuplicateLoanCheckGlobal = EnableDisableSetting.Enabled == (EnableDisableSetting) current.Settings.GetServerSetting("Components.DuplicateLoanCheck");
      if (loanFolder != "" && loanName != "")
        configurationInfo.IsDuplicateLoanCheckLoanOnly = LoanDuplicateAccessor.GetDuplicateScreenSetting(userInfo.Userid, loanFolder, loanName).ToDisplay;
      InputFormsBpmDbAccessor accessor1 = (InputFormsBpmDbAccessor) BpmDbAccessor.GetAccessor(BizRuleType.InputForms);
      configurationInfo.UserAccessibleForms = InputFormsAclDbAccessor.GetAccessibleForms(userInfo);
      FieldRulesBpmDbAccessor accessor2 = (FieldRulesBpmDbAccessor) BpmDbAccessor.GetAccessor(BizRuleType.FieldRules);
      configurationInfo.FieldRulesModificationTime = accessor2.GetLastRuleModificationTime();
      if (configParams == null || configParams.FieldRulesModificationTime == DateTime.MinValue || configParams.FieldRulesModificationTime < configurationInfo.FieldRulesModificationTime)
        configurationInfo.FieldRules = (FieldRuleInfo[]) accessor2.GetRules(true);
      configurationInfo.CustomFieldsModificationTime = SystemConfiguration.GetLoanCustomFieldsModificationDate();
      TriggersBpmDbAccessor accessor3 = (TriggersBpmDbAccessor) BpmDbAccessor.GetAccessor(BizRuleType.Triggers);
      configurationInfo.TriggersModificationTime = accessor3.GetLastRuleModificationTime();
      if (configParams == null || configParams.TriggersModificationTime == DateTime.MinValue || configParams.TriggersModificationTime < configurationInfo.TriggersModificationTime)
        configurationInfo.Triggers = (TriggerInfo[]) accessor3.GetRules(true);
      PrintSelectionBpmDbAccessor accessor4 = (PrintSelectionBpmDbAccessor) BpmDbAccessor.GetAccessor(BizRuleType.PrintSelection);
      configurationInfo.PrintSelectionModificationTime = accessor4.GetLastRuleModificationTime();
      if (configParams == null || configParams.PrintSelectionModificationTime == DateTime.MinValue || configParams.PrintSelectionModificationTime < configurationInfo.PrintSelectionModificationTime)
        configurationInfo.PrintSelectionRules = (PrintSelectionRuleInfo[]) accessor4.GetRules(true);
      MilestoneRulesBpmDbAccessor accessor5 = (MilestoneRulesBpmDbAccessor) BpmDbAccessor.GetAccessor(BizRuleType.MilestoneRules);
      configurationInfo.MilestoneRulesModificationTime = accessor5.GetLastRuleModificationTime();
      configurationInfo.PiggybackSyncFields = LoanConfiguration.getPiggybackSyncFields();
      configurationInfo.LoanOfficerCompensationSetting = new LOCompensationSetting(Company.GetCompanySetting("LOCompensation", "Rule"));
      if ((bool) current.Settings.GetServerSetting("Policies.FeeListOption"))
      {
        configurationInfo.FeeManagementList = SystemConfiguration.GetFeeManagement();
        if (configurationInfo.FeeManagementList == null)
          configurationInfo.FeeManagementList = new FeeManagementSetting();
        else
          configurationInfo.FeeManagementList.CompanyOptIn = true;
        int[] personIDs = new int[userInfo.UserPersonas.Length];
        for (int index = 0; index < userInfo.UserPersonas.Length; ++index)
          personIDs[index] = userInfo.UserPersonas[index].ID;
        configurationInfo.FeeManagementPersonaPermission = FieldAccessAclDbAccessor.GetFeeManagementPermission(personIDs);
      }
      else
      {
        configurationInfo.FeeManagementList = new FeeManagementSetting();
        configurationInfo.FeeManagementList.CompanyOptIn = false;
      }
      configurationInfo.RequireCoCPriorDisclosure = (bool) current.Settings.GetServerSetting("Policies.RequireCoCPriorDisclosure");
      configurationInfo.DelayAttachmentConversion = (bool) current.Settings.GetServerSetting("Policies.DelayAttachmentConversion");
      return configurationInfo;
    }

    public static ILoanSpecificConfigurationInfo GetLoanSpecificConfigurationInfo(
      string userid,
      string loanFolder,
      string loanName,
      HMDAInformation hmdaInfo)
    {
      ClientContext.GetCurrent();
      ILoanSpecificConfigurationInfo configurationInfo = (ILoanSpecificConfigurationInfo) new LoanSpecificConfigurationInfo();
      if (loanFolder != "" && loanName != "")
        configurationInfo.IsDuplicateLoanCheckLoanOnly = LoanDuplicateAccessor.GetDuplicateScreenSetting(userid, loanFolder, loanName).ToDisplay;
      configurationInfo.HmdaInfo = hmdaInfo;
      return configurationInfo;
    }

    public static LoanDefaults GetLoanDefaultData(string userId)
    {
      return new LoanDefaults()
      {
        RESPAFields = SystemConfiguration.GetDefaultFields("RESPAFieldList"),
        PrivacyPolicyFields = SystemConfiguration.GetDefaultFields("PrivacyPolicyFieldList"),
        FHAConsumerChoiceFieldList = SystemConfiguration.GetDefaultFields("FHAConsumerChoiceFieldList"),
        DefaultProviders = User.GetDefaultProviderInfo(userId)
      };
    }

    private static PiggybackFields getPiggybackSyncFields()
    {
      try
      {
        Type valueType = typeof (PiggybackFields);
        using (BinaryObject systemSettings = SystemConfiguration.GetSystemSettings(valueType.Name))
          return systemSettings == null ? (PiggybackFields) null : (PiggybackFields) new XmlSerializer().Deserialize(systemSettings.OpenStream(), valueType);
      }
      catch (Exception ex)
      {
        return (PiggybackFields) null;
      }
    }

    public static SellConditionTrackingSetup GetUpdatedSellConditionSetup()
    {
      return UnderwritingConditionListAccessor.GetSellConditionList();
    }
  }
}
