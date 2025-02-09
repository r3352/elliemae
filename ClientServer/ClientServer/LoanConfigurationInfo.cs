// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanConfigurationInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanConfigurationInfo : ILoanConfigurationInfo
  {
    public LoanConfigurationInfo()
    {
      this.LoanSettings = (ILoanSettings) null;
      this.CompanyInfo = (CompanyInfo) null;
      this.DefaultDocumentAccessRules = (DocumentDefaultAccessRuleInfo[]) null;
      this.AllRoles = (RoleInfo[]) null;
      this.MilestonesList = (List<EllieMae.EMLite.Workflow.Milestone>) null;
      this.UserOrganiation = (OrgInfo) null;
      this.DisplayOrganization = (OrgInfo) null;
      this.RoleMappings = (RolesMappingInfo[]) null;
      this.UserRoleMappings = (RolesMappingInfo[]) null;
      this.ImageAttachmentSettings = (ImageAttachmentSettings) null;
      this.WebCenterSettings = (WebCenterSettings) null;
      this.DocumentTrackingSetup = (DocumentTrackingSetup) null;
      this.DocumentGroupSetup = (DocumentGroupSetup) null;
      this.UnderwritingConditionTrackingSetup = (UnderwritingConditionTrackingSetup) null;
      this.PostClosingConditionTrackingSetup = (PostClosingConditionTrackingSetup) null;
      this.SellConditionTrackingSetup = (SellConditionTrackingSetup) null;
      this.DefaultStackingOrderTemplateName = (string) null;
      this.FormPrintGroupDefault = FormPrintGroupDefault.None;
      this.LockLoanNumber = false;
      this.DisplayBusinessRuleOption = EnableDisableSetting.Disabled;
      this.InterviewerPopulation = InterviewerInfoSetting.FileStarter;
      this.ContactHistoryUpdateMilestoneID = (string) null;
      this.TasksSetup = (Hashtable) null;
      this.UserAclGroups = (AclGroup[]) null;
      this.AllForms = (InputFormInfo[]) null;
      this.UserAccessibleForms = (InputFormInfo[]) null;
      this.FeeManagementList = (FeeManagementSetting) null;
      this.FeeManagementPersonaPermission = (FeeManagementPersonaInfo) null;
      this.ClosedLoanBillingMilestoneID = (string) null;
      this.FieldRulesModificationTime = DateTime.MinValue;
      this.CustomFieldsModificationTime = DateTime.MinValue;
      this.TriggersModificationTime = DateTime.MinValue;
      this.MilestoneRulesModificationTime = DateTime.MinValue;
      this.PrintSelectionModificationTime = DateTime.MinValue;
      this.FieldRules = (FieldRuleInfo[]) null;
      this.Triggers = (TriggerInfo[]) null;
      this.PrintSelectionRules = (PrintSelectionRuleInfo[]) null;
      this.ChannelOptions = (int[]) null;
      this.PiggybackSyncFields = (PiggybackFields) null;
      this.LoanOfficerCompensationSetting = (LOCompensationSetting) null;
      this.LoanCompDefaultPlan = (LoanCompDefaultPlan) null;
      this.IsDuplicateLoanCheckGlobal = false;
      this.IsDuplicateLoanCheckLoanOnly = true;
    }

    public ILoanSettings LoanSettings { get; set; }

    public CompanyInfo CompanyInfo { get; set; }

    public DocumentDefaultAccessRuleInfo[] DefaultDocumentAccessRules { get; set; }

    public RoleInfo[] AllRoles { get; set; }

    public List<EllieMae.EMLite.Workflow.Milestone> MilestonesList { get; set; }

    public OrgInfo UserOrganiation { get; set; }

    public OrgInfo DisplayOrganization { get; set; }

    public RolesMappingInfo[] RoleMappings { get; set; }

    public RolesMappingInfo[] UserRoleMappings { get; set; }

    public ImageAttachmentSettings ImageAttachmentSettings { get; set; }

    public WebCenterSettings WebCenterSettings { get; set; }

    public DocumentTrackingSetup DocumentTrackingSetup { get; set; }

    public DocumentGroupSetup DocumentGroupSetup { get; set; }

    public UnderwritingConditionTrackingSetup UnderwritingConditionTrackingSetup { get; set; }

    public PostClosingConditionTrackingSetup PostClosingConditionTrackingSetup { get; set; }

    public SellConditionTrackingSetup SellConditionTrackingSetup { get; set; }

    public string DefaultStackingOrderTemplateName { get; set; }

    public FormPrintGroupDefault FormPrintGroupDefault { get; set; }

    public bool LockLoanNumber { get; set; }

    public EnableDisableSetting DisplayBusinessRuleOption { get; set; }

    public InterviewerInfoSetting InterviewerPopulation { get; set; }

    public string ContactHistoryUpdateMilestoneID { get; set; }

    public Hashtable TasksSetup { get; set; }

    public AclGroup[] UserAclGroups { get; set; }

    public InputFormInfo[] AllForms { get; set; }

    public InputFormInfo[] UserAccessibleForms { get; set; }

    public FeeManagementSetting FeeManagementList { get; set; }

    public FeeManagementPersonaInfo FeeManagementPersonaPermission { get; set; }

    public string ClosedLoanBillingMilestoneID { get; set; }

    public DateTime FieldRulesModificationTime { get; set; }

    public DateTime CustomFieldsModificationTime { get; set; }

    public DateTime TriggersModificationTime { get; set; }

    public DateTime MilestoneRulesModificationTime { get; set; }

    public DateTime PrintSelectionModificationTime { get; set; }

    public DateTime RolesModificationTime { get; set; }

    public FieldRuleInfo[] FieldRules { get; set; }

    public TriggerInfo[] Triggers { get; set; }

    public PrintSelectionRuleInfo[] PrintSelectionRules { get; set; }

    public int[] ChannelOptions { get; set; }

    public PiggybackFields PiggybackSyncFields { get; set; }

    public LOCompensationSetting LoanOfficerCompensationSetting { get; set; }

    public LoanCompDefaultPlan LoanCompDefaultPlan { get; set; }

    public bool IsDuplicateLoanCheckGlobal { get; set; }

    public bool IsDuplicateLoanCheckLoanOnly { get; set; }

    public bool RequireCoCPriorDisclosure { get; set; }

    public bool DelayAttachmentConversion { get; set; }

    public string SystemID => this.LoanSettings.SystemID;

    public CustomFieldsInfo CustomFields => this.LoanSettings.FieldSettings.CustomFields;

    public AlertSetupData AlertSetupData => this.LoanSettings.AlertSetupData;

    public RoleInfo GetRoleFunction(int roleId)
    {
      foreach (RoleInfo allRole in this.AllRoles)
      {
        if (allRole.RoleID == roleId)
          return allRole;
      }
      return (RoleInfo) null;
    }
  }
}
