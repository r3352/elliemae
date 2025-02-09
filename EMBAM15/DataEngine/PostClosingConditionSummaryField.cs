// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.PostClosingConditionSummaryField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class PostClosingConditionSummaryField(string fieldId, string description) : VirtualField(fieldId, description, FieldFormat.STRING)
  {
    public override VirtualFieldType VirtualFieldType
    {
      get => VirtualFieldType.PostClosingConditionFields;
    }

    public override int ReportingDatabaseColumnSize => 4096;

    protected override string Evaluate(LoanData loan)
    {
      string str = string.Empty;
      bool flag1 = loan.GetField("ENHANCEDCOND.X1") == "Y";
      string logType = "Post-Closing";
      foreach (ConditionLog conditionLog in !flag1 ? loan.GetLogList().GetAllConditions(ConditionType.PostClosing) : loan.GetLogList().GetAllConditions(ConditionType.Enhanced))
      {
        PostClosingConditionLog closingConditionLog = flag1 ? (PostClosingConditionLog) null : (PostClosingConditionLog) conditionLog;
        EnhancedConditionLog enhancedConditionLog = flag1 ? (EnhancedConditionLog) conditionLog : (EnhancedConditionLog) null;
        bool flag2 = false;
        switch (this.FieldID)
        {
          case "PCC.ALL":
            if (closingConditionLog != null || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType))
            {
              flag2 = true;
              break;
            }
            break;
          case "PCC.NOTCLEARED":
            if (closingConditionLog != null && !closingConditionLog.Cleared || enhancedConditionLog != null && this.CheckEnhancedConditionType(enhancedConditionLog, logType) && !this.CheckEnhancedConditionStatus(enhancedConditionLog, "Cleared"))
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
            str = str + closingConditionLog.Title.Trim() + " - " + closingConditionLog.Description;
        }
      }
      return str;
    }

    public override bool AppliesToEdition(EncompassEdition edition)
    {
      return edition == EncompassEdition.Banker;
    }
  }
}
