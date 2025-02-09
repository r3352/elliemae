// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Customization.LoanActionDataSource
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using Elli.Common.Loan;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Customization
{
  public class LoanActionDataSource : LoanDataSource, ILoanActionDataSource
  {
    private IServerDataProvider provider;
    private Dictionary<string, List<string>> sfcFannieList;
    private Dictionary<string, List<string>> sfcFreddieList;
    private Dictionary<string, List<string>> sfcOtherList;

    public LoanActionDataSource(
      LoanData loan,
      UserInfo currentUser,
      bool readOnly,
      IServerDataProvider provider)
      : base(loan, currentUser, readOnly)
    {
      this.provider = provider;
    }

    public bool IsComplete(string loanActionName)
    {
      return this.getLoanActionByName(loanActionName) != null;
    }

    public bool ApplySpecialFeatureCodes(string[] codes, bool deleteExistLog)
    {
      if (deleteExistLog)
        this.Loan.RemoveSpecialFeatureCodes();
      if (codes == null || codes.Length == 0)
      {
        if (this.Loan.Calculator != null)
          this.Loan.Calculator.FormCalculation("SFC0001");
        return true;
      }
      if (this.sfcFannieList == null && this.sfcFreddieList == null && this.sfcOtherList == null && this.provider != null)
      {
        List<Dictionary<string, List<string>>> specialFeatureCodes = this.provider.GetActiveSpecialFeatureCodes();
        if (specialFeatureCodes != null)
        {
          this.sfcFannieList = specialFeatureCodes.Count > 0 ? specialFeatureCodes[0] : (Dictionary<string, List<string>>) null;
          this.sfcFreddieList = specialFeatureCodes.Count > 1 ? specialFeatureCodes[1] : (Dictionary<string, List<string>>) null;
          this.sfcOtherList = specialFeatureCodes.Count > 2 ? specialFeatureCodes[2] : (Dictionary<string, List<string>>) null;
        }
      }
      int specialFeatureCode = this.Loan.GetNumberOfSpecialFeatureCode();
      Dictionary<string, int> dictionary1 = new Dictionary<string, int>();
      Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
      Dictionary<string, int> dictionary3 = new Dictionary<string, int>();
      List<int> intList = new List<int>();
      for (int index = 1; index <= specialFeatureCode; ++index)
      {
        string field1 = this.Loan.GetField("SFC" + index.ToString("00") + "01");
        string field2 = this.Loan.GetField("SFC" + index.ToString("00") + "04");
        if (field1 == "")
        {
          intList.Add(index);
        }
        else
        {
          switch (field2)
          {
            case "FannieMae":
              dictionary1.Add(field1, index);
              continue;
            case "FreddieMac":
              dictionary2.Add(field1, index);
              continue;
            default:
              dictionary3.Add(field1, index);
              continue;
          }
        }
      }
      for (int index = 0; index < codes.Length; ++index)
      {
        List<string> stringList;
        if (this.sfcFannieList != null && this.sfcFannieList.ContainsKey(codes[index]))
          stringList = this.sfcFannieList[codes[index]];
        else if (this.sfcFreddieList != null && this.sfcFreddieList.ContainsKey(codes[index]))
          stringList = this.sfcFreddieList[codes[index]];
        else if (this.sfcOtherList != null && this.sfcOtherList.ContainsKey(codes[index]))
          stringList = this.sfcOtherList[codes[index]];
        else
          continue;
        if (stringList.Count >= 3)
        {
          string str1 = codes[index].IndexOf("~~") > -1 ? codes[index].Substring(codes[index].IndexOf("~~") + 2) : codes[index];
          string str2 = stringList[3];
          int num;
          if (str2 == "Fannie Mae" && dictionary1.ContainsKey(str1))
            num = dictionary1[str1];
          else if (str2 == "Freddie Mac" && dictionary2.ContainsKey(str1))
            num = dictionary2[str1];
          else if (str2 != "" && dictionary3.ContainsKey(str1))
            num = dictionary3[str1];
          else if (intList.Count > 0)
          {
            num = intList[0];
            intList.RemoveAt(0);
          }
          else
            num = this.Loan.NewSpecialFeatureCode();
          string str3 = "SFC" + num.ToString("00");
          this.Loan.SetCurrentField(str3 + "01", str1);
          this.Loan.SetCurrentField(str3 + "02", stringList == null || stringList.Count < 1 ? "" : stringList[1]);
          this.Loan.SetCurrentField(str3 + "03", stringList == null || stringList.Count < 2 ? "" : stringList[2]);
          if (stringList != null && stringList.Count >= 3 && (stringList[3] == "Fannie Mae" || stringList[3] == "Freddie Mac"))
          {
            this.Loan.SetCurrentField(str3 + "04", stringList[3].Replace(" ", ""));
            this.Loan.SetCurrentField(str3 + "05", "");
          }
          else
          {
            this.Loan.SetCurrentField(str3 + "04", "Other");
            this.Loan.SetCurrentField(str3 + "05", stringList == null || stringList.Count < 3 ? "" : stringList[3]);
          }
        }
      }
      if (this.Loan.Calculator != null)
        this.Loan.Calculator.FormCalculation("SFC0001");
      return true;
    }

    private LoanActionLog getLoanActionByName(string loanActionName)
    {
      foreach (LogRecordBase allDatedRecord in this.Loan.GetLogList().GetAllDatedRecords())
      {
        if (allDatedRecord is LoanActionLog)
        {
          LoanActionLog loanActionByName = (LoanActionLog) allDatedRecord;
          if (string.Compare(loanActionByName.LoanActionType.ToString(), LoanAction.GetActualLoanActionName(loanActionName), true) == 0)
            return loanActionByName;
        }
      }
      return (LoanActionLog) null;
    }
  }
}
