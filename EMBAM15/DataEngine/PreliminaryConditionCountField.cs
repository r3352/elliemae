// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.PreliminaryConditionCountField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class PreliminaryConditionCountField(string fieldId, string description) : VirtualField(fieldId, description, FieldFormat.INTEGER)
  {
    public override VirtualFieldType VirtualFieldType
    {
      get => VirtualFieldType.PreliminaryConditionFields;
    }

    protected override string Evaluate(LoanData loan)
    {
      int num = 0;
      bool flag1 = loan.GetField("ENHANCEDCOND.X1") == "Y";
      string logType = "Preliminary";
      foreach (ConditionLog conditionLog in !flag1 ? loan.GetLogList().GetAllConditions(ConditionType.Preliminary) : loan.GetLogList().GetAllConditions(ConditionType.Enhanced))
      {
        PreliminaryConditionLog preliminaryConditionLog = flag1 ? (PreliminaryConditionLog) null : (PreliminaryConditionLog) conditionLog;
        EnhancedConditionLog enhancedConditionLog = flag1 ? (EnhancedConditionLog) conditionLog : (EnhancedConditionLog) null;
        bool flag2 = false;
        switch (this.FieldID)
        {
          case "PRECON.ACCOUNT":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "AC" || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Closing"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.ACFULFILLEDCOUNT":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "AC" && preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Closing") && this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.ACOPENCOUNT":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "AC" && !preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Closing") && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.ALLCOUNT":
            if (preliminaryConditionLog != null || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.FULFILLEDCOUNT":
            if (preliminaryConditionLog != null && preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.OPENCOUNT":
            if (preliminaryConditionLog != null && !preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTACOUNT":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTA" || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Approval"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTAFULFILLEDCOUNT":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTA" && preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Approval") && this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTAOPENCOUNT":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTA" && !preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Approval") && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTDCOUNT":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTD" || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Docs"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTDFULFILLEDCOUNT":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTD" && preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Docs") && this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTDOPENCOUNT":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTD" && !preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Docs") && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTFCOUNT":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTF" || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Funding"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTFFULFILLEDCOUNT":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTF" && preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Funding") && this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTFOPENCOUNT":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTF" && !preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Funding") && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTPCOUNT":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTP" || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Purchase"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTPFULFILLEDCOUNT":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTP" && preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Purchase") && this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTPOPENCOUNT":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTP" && !preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Purchase") && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
        }
        if (flag2)
          ++num;
      }
      return num.ToString();
    }
  }
}
