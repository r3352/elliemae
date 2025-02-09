// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Bpm.FieldSearchRuleType
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Bpm
{
  [Serializable]
  public enum FieldSearchRuleType
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
    AutomatedConditions = 11, // 0x0000000B
    PiggyBackingFields = 13, // 0x0000000D
    LoanCustomFields = 14, // 0x0000000E
    BorrowerCustomFields = 15, // 0x0000000F
    BusinessCustomFields = 16, // 0x00000010
    Alerts = 17, // 0x00000011
    LockRequestAdditionalFields = 18, // 0x00000012
    HtmlEmailTemplate = 19, // 0x00000013
    CompanyStatusOnline = 20, // 0x00000014
    TPOCustomFields = 21, // 0x00000015
    LoanActionAccess = 22, // 0x00000016
    LoanActionCompletionRules = 23, // 0x00000017
    DDMFeeScenarios = 24, // 0x00000018
    DDMFieldScenarios = 25, // 0x00000019
    DDMDataTables = 26, // 0x0000001A
    AutomatedEnhancedConditions = 27, // 0x0000001B
  }
}
