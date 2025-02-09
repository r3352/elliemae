// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.PreliminaryConditionSummaryField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class PreliminaryConditionSummaryField(string fieldId, string description) : VirtualField(fieldId, description, FieldFormat.STRING)
  {
    public override VirtualFieldType VirtualFieldType
    {
      get => VirtualFieldType.PreliminaryConditionFields;
    }

    public override int ReportingDatabaseColumnSize => 4096;

    protected override string Evaluate(LoanData loan)
    {
      string str = string.Empty;
      bool flag1 = loan.GetField("ENHANCEDCOND.X1") == "Y";
      ConditionLog[] conditionLogArray = !flag1 ? loan.GetLogList().GetAllConditions(ConditionType.Preliminary) : loan.GetLogList().GetAllConditions(ConditionType.Enhanced);
      string logType = "Preliminary";
      foreach (ConditionLog conditionLog in conditionLogArray)
      {
        PreliminaryConditionLog preliminaryConditionLog = flag1 ? (PreliminaryConditionLog) null : (PreliminaryConditionLog) conditionLog;
        EnhancedConditionLog enhancedConditionLog = flag1 ? (EnhancedConditionLog) conditionLog : (EnhancedConditionLog) null;
        bool flag2 = false;
        switch (this.FieldID)
        {
          case "PRECON.AC":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "AC" || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Closing"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.ACFULFILLED":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "AC" && preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Closing") && this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.ACOPEN":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "AC" && !preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Closing") && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.ALL":
            if (preliminaryConditionLog != null || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.ALLFULFILLED":
            if (preliminaryConditionLog != null && preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.ALLOPEN":
            if (preliminaryConditionLog != null && !preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTA":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTA" || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Approval"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTAFULFILLED":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTA" && preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Approval") && this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTAOPEN":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTA" && !preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Approval") && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTD":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTD" || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Docs"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTDFULFILLED":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTD" && preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Docs") && this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTDOPEN":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTD" && !preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Docs") && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTF":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTF" || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Funding"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTFFULFILLED":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTF" && preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Funding") && this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTFOPEN":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTF" && !preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Funding") && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTP":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTP" || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Purchase"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTPFULFILLED":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTP" && preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Purchase") && this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "PRECON.PTPOPEN":
            if (preliminaryConditionLog != null && preliminaryConditionLog.PriorTo == "PTP" && !preliminaryConditionLog.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Purchase") && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
        }
        if (flag2)
        {
          if (str != string.Empty)
            str += "\r\n";
          if (flag1)
            str = str + enhancedConditionLog.Title.Trim() + " - " + enhancedConditionLog.InternalDescription + "\t\t" + enhancedConditionLog.ExternalDescription;
          else
            str = str + preliminaryConditionLog.Title.Trim() + " - " + preliminaryConditionLog.Description;
        }
      }
      return str;
    }
  }
}
