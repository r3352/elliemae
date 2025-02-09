// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.MortgageInsuranceUtils
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class MortgageInsuranceUtils
  {
    public static LoanTypeEnum GetLoanType(LoanData loanData)
    {
      string field = loanData.GetField("1172");
      LoanTypeEnum loanType = LoanTypeEnum.Other;
      switch (field)
      {
        case "Conventional":
          loanType = LoanTypeEnum.Conventional;
          break;
        case "FHA":
          loanType = LoanTypeEnum.FHA;
          break;
        case "VA":
          loanType = LoanTypeEnum.VA;
          break;
      }
      return loanType;
    }

    public static bool IsStreamLined(LoanData loanData)
    {
      return loanData.GetField("MORNET.X40") == "StreamlineWithAppraisal" || loanData.GetField("MORNET.X40") == "StreamlineWithoutAppraisal";
    }

    public static bool recordMatched(
      FieldFilter[] filters,
      string loanType,
      bool isStreamLine,
      LoanData loanData)
    {
      string str1 = "";
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      for (int index = 0; index < filters.Length; ++index)
      {
        string simpleField = loanData.GetSimpleField(filters[index].FieldID);
        if ((loanType == "FHA" && filters[index].FieldID == "3042" && filters[index].OperatorType != OperatorTypes.EmptyDate || loanType == "FHA" & isStreamLine && filters[index].FieldID == "3432") && (simpleField == string.Empty || simpleField == "//"))
          simpleField = DateTime.Today.ToString("MM/dd/yyyy");
        string scriptCommands = filters[index].GetScriptCommands(simpleField);
        str1 = !(str1 == "") ? str1 + " " + scriptCommands : scriptCommands;
      }
      string str2 = str1.Trim().ToLower();
      if (str2 == "")
        return true;
      if (str2.EndsWith("and"))
        str2 = str2.Substring(0, str2.Length - 3);
      if (str2.EndsWith("or"))
        str2 = str2.Substring(0, str2.Length - 2);
      return Utils.CheckFilter(str2.Trim()) == "true";
    }

    public static MIRecord[] GetFilteredMIData(LoanData loanData, MIRecord[] records)
    {
      MortgageInsuranceUtils.GetLoanType(loanData);
      string field = loanData.GetField("1172");
      if (records == null || records.Length == 0)
        return (MIRecord[]) null;
      bool isStreamLine = MortgageInsuranceUtils.IsStreamLined(loanData);
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < records.Length; ++index)
      {
        if (records[index].Scenarios != null && records[index].Scenarios.Length != 0 && MortgageInsuranceUtils.recordMatched(records[index].Scenarios, field, isStreamLine, loanData))
          arrayList.Add((object) records[index]);
      }
      return arrayList.Count == 0 ? (MIRecord[]) null : (MIRecord[]) arrayList.ToArray(typeof (MIRecord));
    }
  }
}
