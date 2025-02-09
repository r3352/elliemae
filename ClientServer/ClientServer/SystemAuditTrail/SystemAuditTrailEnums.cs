// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SystemAuditTrail.SystemAuditTrailEnums
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.EMLite.ClientServer.SystemAuditTrail
{
  public class SystemAuditTrailEnums
  {
    public static AuditObjectType GetAuditObjectType(BizRuleType bizRuleType)
    {
      AuditObjectType auditObjectType = AuditObjectType.BizRule_None;
      switch (bizRuleType)
      {
        case BizRuleType.MilestoneRules:
          auditObjectType = AuditObjectType.BizRule_Milestone;
          break;
        case BizRuleType.LoanAccess:
          auditObjectType = AuditObjectType.BizRule_LoanAccess;
          break;
        case BizRuleType.FieldAccess:
          auditObjectType = AuditObjectType.BizRule_FieldAccess;
          break;
        case BizRuleType.FieldRules:
          auditObjectType = AuditObjectType.BizRule_FieldRule;
          break;
        case BizRuleType.InputForms:
          auditObjectType = AuditObjectType.BizRule_InputForm;
          break;
        case BizRuleType.Triggers:
          auditObjectType = AuditObjectType.BizRule_Trigger;
          break;
        case BizRuleType.LoanFolderRules:
          auditObjectType = AuditObjectType.BizRule_LoanFolderRules;
          break;
        case BizRuleType.DDMFeeScenarios:
          auditObjectType = AuditObjectType.DDM_FeeScenarios;
          break;
        case BizRuleType.DDMFeeRules:
          auditObjectType = AuditObjectType.DDM_FeeRules;
          break;
        case BizRuleType.DDMDataPopTiming:
          auditObjectType = AuditObjectType.DDM_DataPopTiming;
          break;
        case BizRuleType.DDMFieldScenarios:
          auditObjectType = AuditObjectType.DDM_FieldScenarios;
          break;
        case BizRuleType.DDMDataTables:
          auditObjectType = AuditObjectType.DDM_DataTables;
          break;
        case BizRuleType.DDMFieldRules:
          auditObjectType = AuditObjectType.DDM_FieldRules;
          break;
      }
      return auditObjectType;
    }
  }
}
