// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.BizRuleType
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public enum BizRuleType
  {
    None = 0,
    MilestoneRules = 1,
    LoanAccess = 2,
    FieldAccess = 3,
    FieldRules = 4,
    InputForms = 5,
    Triggers = 6,
    PrintForms = 7,
    PrintSelection = 8,
    LoanFolderRules = 9,
    EmailTriggers = 10, // 0x0000000A
    AutomatedConditions = 11, // 0x0000000B
    AutomatedPurchaseConditions = 12, // 0x0000000C
    MilestoneTemplateConditions = 12, // 0x0000000C
    LoanActionAccess = 13, // 0x0000000D
    LoanActionCompletionRules = 14, // 0x0000000E
    AutoLockExclusionRules = 15, // 0x0000000F
    DDMFeeScenarios = 16, // 0x00000010
    DDMFeeRules = 17, // 0x00000011
    DDMDataPopTiming = 18, // 0x00000012
    DDMFieldScenarios = 19, // 0x00000013
    DDMDataTables = 20, // 0x00000014
    DDMFieldRules = 21, // 0x00000015
    ServiceWorkflowRules = 22, // 0x00000016
    AutomatedEnhancedConditions = 23, // 0x00000017
  }
}
