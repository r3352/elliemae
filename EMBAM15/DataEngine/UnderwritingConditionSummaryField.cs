// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.UnderwritingConditionSummaryField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class UnderwritingConditionSummaryField(string fieldId, string description) : VirtualField(fieldId, description, FieldFormat.STRING)
  {
    public override VirtualFieldType VirtualFieldType
    {
      get => VirtualFieldType.UnderwritingConditionFields;
    }

    public override int ReportingDatabaseColumnSize => 4096;

    protected override string Evaluate(LoanData loan)
    {
      string str = string.Empty;
      bool flag1 = loan.GetField("ENHANCEDCOND.X1") == "Y";
      string logType = "Underwriting";
      foreach (ConditionLog conditionLog in !flag1 ? loan.GetLogList().GetAllConditions(ConditionType.Underwriting) : loan.GetLogList().GetAllConditions(ConditionType.Enhanced))
      {
        UnderwritingConditionLog underwritingConditionLog = flag1 ? (UnderwritingConditionLog) null : (UnderwritingConditionLog) conditionLog;
        EnhancedConditionLog enhancedConditionLog = flag1 ? (EnhancedConditionLog) conditionLog : (EnhancedConditionLog) null;
        bool flag2 = false;
        bool? nullable;
        switch (this.FieldID)
        {
          case "UWC.AC":
            if (underwritingConditionLog == null || !underwritingConditionLog.IsExternal || underwritingConditionLog.Waived || underwritingConditionLog.Cleared || !(underwritingConditionLog.PriorTo == "AC"))
            {
              if (enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType))
              {
                nullable = enhancedConditionLog.ExternalPrint;
                bool flag3 = true;
                if (!(nullable.GetValueOrDefault() == flag3 & nullable.HasValue) || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Waived") || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Cleared") || !this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Closing"))
                  break;
              }
              else
                break;
            }
            flag2 = true;
            break;
          case "UWC.ALL":
            if (underwritingConditionLog != null || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType))
            {
              flag2 = true;
              break;
            }
            break;
          case "UWC.INTERNAL":
            if (underwritingConditionLog == null || !underwritingConditionLog.IsInternal)
            {
              if (enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType))
              {
                nullable = enhancedConditionLog.InternalPrint;
                bool flag4 = true;
                if (!(nullable.GetValueOrDefault() == flag4 & nullable.HasValue))
                  break;
              }
              else
                break;
            }
            flag2 = true;
            break;
          case "UWC.NOTCLEARED":
            if (underwritingConditionLog == null || !underwritingConditionLog.IsExternal || underwritingConditionLog.Waived || underwritingConditionLog.Cleared)
            {
              if (enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType))
              {
                nullable = enhancedConditionLog.ExternalPrint;
                bool flag5 = true;
                if (!(nullable.GetValueOrDefault() == flag5 & nullable.HasValue) || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Waived") || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Cleared"))
                  break;
              }
              else
                break;
            }
            flag2 = true;
            break;
          case "UWC.OPENINTERNAL":
            if (underwritingConditionLog == null || !underwritingConditionLog.IsInternal || underwritingConditionLog.Waived || underwritingConditionLog.Cleared)
            {
              if (enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType))
              {
                nullable = enhancedConditionLog.InternalPrint;
                bool flag6 = true;
                if (!(nullable.GetValueOrDefault() == flag6 & nullable.HasValue) || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Waived") || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Cleared"))
                  break;
              }
              else
                break;
            }
            flag2 = true;
            break;
          case "UWC.PTA":
            if (underwritingConditionLog == null || !underwritingConditionLog.IsExternal || underwritingConditionLog.Waived || underwritingConditionLog.Cleared || !(underwritingConditionLog.PriorTo == "PTA"))
            {
              if (enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType))
              {
                nullable = enhancedConditionLog.ExternalPrint;
                bool flag7 = true;
                if (!(nullable.GetValueOrDefault() == flag7 & nullable.HasValue) || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Waived") || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Cleared") || !this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Approval"))
                  break;
              }
              else
                break;
            }
            flag2 = true;
            break;
          case "UWC.PTD":
            if (underwritingConditionLog == null || !underwritingConditionLog.IsExternal || underwritingConditionLog.Waived || underwritingConditionLog.Cleared || !(underwritingConditionLog.PriorTo == "PTD"))
            {
              if (enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType))
              {
                nullable = enhancedConditionLog.ExternalPrint;
                bool flag8 = true;
                if (!(nullable.GetValueOrDefault() == flag8 & nullable.HasValue) || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Waived") || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Cleared") || !this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Docs"))
                  break;
              }
              else
                break;
            }
            flag2 = true;
            break;
          case "UWC.PTF":
            if (underwritingConditionLog == null || !underwritingConditionLog.IsExternal || underwritingConditionLog.Waived || underwritingConditionLog.Cleared || !(underwritingConditionLog.PriorTo == "PTF"))
            {
              if (enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType))
              {
                nullable = enhancedConditionLog.ExternalPrint;
                bool flag9 = true;
                if (!(nullable.GetValueOrDefault() == flag9 & nullable.HasValue) || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Waived") || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Cleared") || !this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Funding"))
                  break;
              }
              else
                break;
            }
            flag2 = true;
            break;
          case "UWC.PTP":
            if (underwritingConditionLog == null || !underwritingConditionLog.IsExternal || underwritingConditionLog.Waived || underwritingConditionLog.Cleared || !(underwritingConditionLog.PriorTo == "PTP"))
            {
              if (enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType))
              {
                nullable = enhancedConditionLog.ExternalPrint;
                bool flag10 = true;
                if (!(nullable.GetValueOrDefault() == flag10 & nullable.HasValue) || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Waived") || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Cleared") || !this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Purchase"))
                  break;
              }
              else
                break;
            }
            flag2 = true;
            break;
        }
        if (flag2)
        {
          if (str != string.Empty)
            str += "\r\n";
          if (flag1)
            str = str + enhancedConditionLog.Title.Trim() + " - " + enhancedConditionLog.InternalDescription + "\t\t" + enhancedConditionLog.ExternalDescription;
          else
            str = str + underwritingConditionLog.Title.Trim() + " - " + underwritingConditionLog.Description;
        }
      }
      return str;
    }
  }
}
