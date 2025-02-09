// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.UnderwritingConditionCountField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class UnderwritingConditionCountField(string fieldId, string description) : VirtualField(fieldId, description, FieldFormat.INTEGER)
  {
    public override VirtualFieldType VirtualFieldType
    {
      get => VirtualFieldType.UnderwritingConditionFields;
    }

    protected override string Evaluate(LoanData loan)
    {
      int num = 0;
      bool flag1 = loan.GetField("ENHANCEDCOND.X1") == "Y";
      string logType = "Underwriting";
      foreach (ConditionLog conditionLog in !flag1 ? loan.GetLogList().GetAllConditions(ConditionType.Underwriting) : loan.GetLogList().GetAllConditions(ConditionType.Enhanced))
      {
        UnderwritingConditionLog underwritingConditionLog = flag1 ? (UnderwritingConditionLog) null : (UnderwritingConditionLog) conditionLog;
        EnhancedConditionLog enhancedConditionLog = flag1 ? (EnhancedConditionLog) conditionLog : (EnhancedConditionLog) null;
        bool flag2 = false;
        bool? externalPrint;
        switch (this.FieldID)
        {
          case "UWC.ACCOUNT":
            if (underwritingConditionLog == null || !underwritingConditionLog.IsExternal || underwritingConditionLog.Cleared || underwritingConditionLog.Waived || !(underwritingConditionLog.PriorTo == "AC"))
            {
              if (enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType))
              {
                externalPrint = enhancedConditionLog.ExternalPrint;
                bool flag3 = true;
                if (!(externalPrint.GetValueOrDefault() == flag3 & externalPrint.HasValue) || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Waived") || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Cleared") || !this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Closing"))
                  break;
              }
              else
                break;
            }
            flag2 = true;
            break;
          case "UWC.ADDEDCOUNT":
            if (underwritingConditionLog != null && underwritingConditionLog.Status == ConditionStatus.Added || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType))
            {
              flag2 = true;
              break;
            }
            break;
          case "UWC.ALLACCOUNT":
            if (underwritingConditionLog != null && !underwritingConditionLog.Cleared && !underwritingConditionLog.Waived && underwritingConditionLog.PriorTo == "AC" || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Waived") && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Cleared") && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Closing"))
            {
              flag2 = true;
              break;
            }
            break;
          case "UWC.ALLCOUNT":
            if (underwritingConditionLog != null || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType))
            {
              flag2 = true;
              break;
            }
            break;
          case "UWC.ALLPTACOUNT":
            if (underwritingConditionLog != null && !underwritingConditionLog.Cleared && !underwritingConditionLog.Waived && underwritingConditionLog.PriorTo == "PTA" || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Waived") && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Cleared") && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Approval"))
            {
              flag2 = true;
              break;
            }
            break;
          case "UWC.ALLPTCPTFCOUNT":
            if (underwritingConditionLog != null && !underwritingConditionLog.Cleared && !underwritingConditionLog.Waived && (underwritingConditionLog.PriorTo == "PTC" || underwritingConditionLog.PriorTo == "PTF" || underwritingConditionLog.PriorTo == "AC") || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Waived") && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Cleared") && (this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Closing") || this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Funding")))
            {
              flag2 = true;
              break;
            }
            break;
          case "UWC.ALLPTDCOUNT":
            if (underwritingConditionLog != null && !underwritingConditionLog.Cleared && !underwritingConditionLog.Waived && underwritingConditionLog.PriorTo == "PTD" || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Waived") && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Cleared") && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Docs"))
            {
              flag2 = true;
              break;
            }
            break;
          case "UWC.ALLPTPCOUNT":
            if (underwritingConditionLog != null && !underwritingConditionLog.Cleared && !underwritingConditionLog.Waived && underwritingConditionLog.PriorTo == "PTP" || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Waived") && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Cleared") && this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Purchase"))
            {
              flag2 = true;
              break;
            }
            break;
          case "UWC.CLEAREDCOUNT":
            if (underwritingConditionLog != null && underwritingConditionLog.Status == ConditionStatus.Cleared || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedConditionStatus(enhancedConditionLog, "Cleared"))
            {
              flag2 = true;
              break;
            }
            break;
          case "UWC.FULFILLEDCOUNT":
            if (underwritingConditionLog != null && underwritingConditionLog.Status == ConditionStatus.Fulfilled || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedConditionStatus(enhancedConditionLog, "Fulfilled"))
            {
              flag2 = true;
              break;
            }
            break;
          case "UWC.OPENCOUNT":
            if (underwritingConditionLog != null && underwritingConditionLog.Status != ConditionStatus.Cleared && underwritingConditionLog.Status != ConditionStatus.Waived || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Waived") && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Cleared"))
            {
              flag2 = true;
              break;
            }
            break;
          case "UWC.OPENEXTERNALCOUNT":
            if (underwritingConditionLog == null || !underwritingConditionLog.IsExternal || underwritingConditionLog.Status == ConditionStatus.Cleared || underwritingConditionLog.Status == ConditionStatus.Waived)
            {
              if (enhancedConditionLog != null)
              {
                externalPrint = enhancedConditionLog.ExternalPrint;
                bool flag4 = true;
                if (!(externalPrint.GetValueOrDefault() == flag4 & externalPrint.HasValue) || !this.CheckEnhancedConditionType(enhancedConditionLog, logType) || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Waived") || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Cleared"))
                  break;
              }
              else
                break;
            }
            flag2 = true;
            break;
          case "UWC.PTACOUNT":
            if (underwritingConditionLog == null || !underwritingConditionLog.IsExternal || underwritingConditionLog.Cleared || underwritingConditionLog.Waived || !(underwritingConditionLog.PriorTo == "PTA"))
            {
              if (enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType))
              {
                externalPrint = enhancedConditionLog.ExternalPrint;
                bool flag5 = true;
                if (!(externalPrint.GetValueOrDefault() == flag5 & externalPrint.HasValue) || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Waived") || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Cleared") || !this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Approval"))
                  break;
              }
              else
                break;
            }
            flag2 = true;
            break;
          case "UWC.PTCPTFCOUNT":
            if (underwritingConditionLog == null || !underwritingConditionLog.IsExternal || underwritingConditionLog.Cleared || underwritingConditionLog.Waived || !(underwritingConditionLog.PriorTo == "PTC") && !(underwritingConditionLog.PriorTo == "PTF") && !(underwritingConditionLog.PriorTo == "AC"))
            {
              if (enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType))
              {
                externalPrint = enhancedConditionLog.ExternalPrint;
                bool flag6 = true;
                if (!(externalPrint.GetValueOrDefault() == flag6 & externalPrint.HasValue) || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Waived") || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Cleared") || !this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Closing") && !this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Funding"))
                  break;
              }
              else
                break;
            }
            flag2 = true;
            break;
          case "UWC.PTDCOUNT":
            if (underwritingConditionLog == null || !underwritingConditionLog.IsExternal || underwritingConditionLog.Cleared || underwritingConditionLog.Waived || !(underwritingConditionLog.PriorTo == "PTD"))
            {
              if (enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType))
              {
                externalPrint = enhancedConditionLog.ExternalPrint;
                bool flag7 = true;
                if (!(externalPrint.GetValueOrDefault() == flag7 & externalPrint.HasValue) || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Waived") || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Cleared") || !this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Docs"))
                  break;
              }
              else
                break;
            }
            flag2 = true;
            break;
          case "UWC.PTPCOUNT":
            if (underwritingConditionLog == null || !underwritingConditionLog.IsExternal || underwritingConditionLog.Cleared || underwritingConditionLog.Waived || !(underwritingConditionLog.PriorTo == "PTP"))
            {
              if (enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType))
              {
                externalPrint = enhancedConditionLog.ExternalPrint;
                bool flag8 = true;
                if (!(externalPrint.GetValueOrDefault() == flag8 & externalPrint.HasValue) || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Waived") || this.CheckEnhancedConditionStatus(enhancedConditionLog, "Cleared") || !this.CheckEnhancedLogPriorTo(enhancedConditionLog, "Purchase"))
                  break;
              }
              else
                break;
            }
            flag2 = true;
            break;
          case "UWC.RECEIVEDCOUNT":
            if (underwritingConditionLog != null && underwritingConditionLog.Status == ConditionStatus.Received || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedConditionStatus(enhancedConditionLog, "Received"))
            {
              flag2 = true;
              break;
            }
            break;
          case "UWC.REJECTEDCOUNT":
            if (underwritingConditionLog != null && underwritingConditionLog.Status == ConditionStatus.Rejected || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedConditionStatus(enhancedConditionLog, "Rejected"))
            {
              flag2 = true;
              break;
            }
            break;
          case "UWC.REVIEWEDCOUNT":
            if (underwritingConditionLog != null && underwritingConditionLog.Status == ConditionStatus.Reviewed || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedConditionStatus(enhancedConditionLog, "Reviewed"))
            {
              flag2 = true;
              break;
            }
            break;
          case "UWC.WAIVEDCOUNT":
            if (underwritingConditionLog != null && underwritingConditionLog.Status == ConditionStatus.Waived || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && this.CheckEnhancedConditionStatus(enhancedConditionLog, "Waived"))
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
