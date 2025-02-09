// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.ComplianceReviewField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class ComplianceReviewField(string fieldId, string description) : VirtualField(fieldId, description, FieldFormat.STRING)
  {
    public override int ReportingDatabaseColumnSize => 4096;

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.ComplianceTestFields;

    protected override string Evaluate(LoanData loan)
    {
      string str = string.Empty;
      foreach (ComplianceTestLog allComplianceTest in loan.GetLogList().GetAllComplianceTests())
      {
        bool flag = false;
        switch (this.FieldID)
        {
          case "COMPLIANCEREVIEW.ALL":
            flag = true;
            break;
          case "COMPLIANCEREVIEW.ALERTS":
            if (allComplianceTest.ShowAlert)
            {
              flag = true;
              break;
            }
            break;
          case "COMPLIANCEREVIEW.PASSED":
            if (allComplianceTest.Result.ToLower() == "pass")
            {
              flag = true;
              break;
            }
            break;
          case "COMPLIANCEREVIEW.WARNINGS":
            if (allComplianceTest.Result.ToLower() == "warning")
            {
              flag = true;
              break;
            }
            break;
          case "COMPLIANCEREVIEW.FAILURES":
            if (allComplianceTest.Result.ToLower() == "fail")
            {
              flag = true;
              break;
            }
            break;
          case "COMPLIANCEREVIEW.ERRORS":
            if (allComplianceTest.Result.ToLower() == "error")
            {
              flag = true;
              break;
            }
            break;
        }
        if (flag)
        {
          if (str != string.Empty)
            str += "\r\n";
          str += allComplianceTest.Name;
          if (!string.IsNullOrEmpty(allComplianceTest.Details))
            str = str + " - " + allComplianceTest.Details;
        }
      }
      return str;
    }
  }
}
