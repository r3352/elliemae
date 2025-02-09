// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ILoanConfigurationInfo
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
  public interface ILoanConfigurationInfo
  {
    ILoanSettings LoanSettings { get; set; }

    CompanyInfo CompanyInfo { get; set; }

    DocumentDefaultAccessRuleInfo[] DefaultDocumentAccessRules { get; set; }

    RoleInfo[] AllRoles { get; set; }

    List<EllieMae.EMLite.Workflow.Milestone> MilestonesList { get; set; }

    OrgInfo UserOrganiation { get; set; }

    OrgInfo DisplayOrganization { get; set; }

    RolesMappingInfo[] RoleMappings { get; set; }

    RolesMappingInfo[] UserRoleMappings { get; set; }

    ImageAttachmentSettings ImageAttachmentSettings { get; set; }

    WebCenterSettings WebCenterSettings { get; set; }

    DocumentTrackingSetup DocumentTrackingSetup { get; set; }

    DocumentGroupSetup DocumentGroupSetup { get; set; }

    UnderwritingConditionTrackingSetup UnderwritingConditionTrackingSetup { get; set; }

    PostClosingConditionTrackingSetup PostClosingConditionTrackingSetup { get; set; }

    SellConditionTrackingSetup SellConditionTrackingSetup { get; set; }

    string DefaultStackingOrderTemplateName { get; set; }

    FormPrintGroupDefault FormPrintGroupDefault { get; set; }

    bool LockLoanNumber { get; set; }

    EnableDisableSetting DisplayBusinessRuleOption { get; set; }

    InterviewerInfoSetting InterviewerPopulation { get; set; }

    string ContactHistoryUpdateMilestoneID { get; set; }

    Hashtable TasksSetup { get; set; }

    AclGroup[] UserAclGroups { get; set; }

    InputFormInfo[] AllForms { get; set; }

    InputFormInfo[] UserAccessibleForms { get; set; }

    FeeManagementSetting FeeManagementList { get; set; }

    FeeManagementPersonaInfo FeeManagementPersonaPermission { get; set; }

    string ClosedLoanBillingMilestoneID { get; set; }

    DateTime FieldRulesModificationTime { get; set; }

    DateTime CustomFieldsModificationTime { get; set; }

    DateTime TriggersModificationTime { get; set; }

    DateTime MilestoneRulesModificationTime { get; set; }

    DateTime PrintSelectionModificationTime { get; set; }

    DateTime RolesModificationTime { get; set; }

    FieldRuleInfo[] FieldRules { get; set; }

    TriggerInfo[] Triggers { get; set; }

    PrintSelectionRuleInfo[] PrintSelectionRules { get; set; }

    int[] ChannelOptions { get; set; }

    PiggybackFields PiggybackSyncFields { get; set; }

    LOCompensationSetting LoanOfficerCompensationSetting { get; set; }

    LoanCompDefaultPlan LoanCompDefaultPlan { get; set; }

    bool IsDuplicateLoanCheckGlobal { get; set; }

    bool IsDuplicateLoanCheckLoanOnly { get; set; }

    string SystemID { get; }

    CustomFieldsInfo CustomFields { get; }

    AlertSetupData AlertSetupData { get; }

    RoleInfo GetRoleFunction(int roleId);

    bool RequireCoCPriorDisclosure { get; set; }

    bool DelayAttachmentConversion { get; set; }
  }
}
