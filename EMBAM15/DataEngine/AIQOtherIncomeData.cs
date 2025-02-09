// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.AIQOtherIncomeData
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class AIQOtherIncomeData
  {
    public string losId { get; set; }

    public string monthlyPayment { get; set; }

    public string incomeType { get; set; }

    public string name { get; set; }

    public bool isSelected { get; set; }

    public int linkedEncompassOISBlockNumber { get; set; }

    public AIQOtherIncomeData(JAIQOtherIncomes data)
    {
      this.losId = this.name = this.monthlyPayment = this.incomeType = string.Empty;
      this.linkedEncompassOISBlockNumber = -1;
      this.losId = data.losId;
      this.name = data.name;
      IList<JAIQCurrentIncome> currentIncome = data?.currentIncome;
      if (currentIncome == null)
        return;
      foreach (JAIQCurrentIncome jaiqCurrentIncome in (IEnumerable<JAIQCurrentIncome>) currentIncome)
      {
        if (jaiqCurrentIncome.selected)
        {
          this.monthlyPayment = jaiqCurrentIncome.monthlyTotalAmount.ToString("N2");
          this.incomeType = jaiqCurrentIncome.type;
        }
      }
    }

    public string GetField(string id) => id == "AIQURLAROIS0022" ? this.monthlyPayment : "";

    public void import(LoanData loan, bool bOrC)
    {
      try
      {
        string str1 = "URLAROIS";
        int encompassOisBlockNumber;
        if (this.linkedEncompassOISBlockNumber == -1)
        {
          this.linkedEncompassOISBlockNumber = loan.NewOtherIncomeSource() + 1;
          if (bOrC)
          {
            LoanData loanData = loan;
            string str2 = str1;
            encompassOisBlockNumber = this.linkedEncompassOISBlockNumber;
            string str3 = encompassOisBlockNumber.ToString("00");
            string id = str2 + str3 + "02";
            loanData.SetField(id, "Borrower");
          }
          else
          {
            LoanData loanData = loan;
            string str4 = str1;
            encompassOisBlockNumber = this.linkedEncompassOISBlockNumber;
            string str5 = encompassOisBlockNumber.ToString("00");
            string id = str4 + str5 + "02";
            loanData.SetField(id, "CoBorrower");
          }
        }
        string str6 = str1;
        encompassOisBlockNumber = this.linkedEncompassOISBlockNumber;
        string str7 = encompassOisBlockNumber.ToString("00");
        string str8 = str6 + str7;
        loan.SetField(str8 + "22", this.monthlyPayment);
        string val;
        if (!EncompassFields.GetField("URLAROIS0018").Options.ContainsValue(this.incomeType))
        {
          val = "Other";
          loan.SetField(str8 + "19", this.incomeType);
        }
        else
          val = this.incomeType;
        loan.SetField(str8 + "18", val);
        DateTime now = DateTime.Now;
        loan.SetField(str8 + "23", "Income Analyzer " + now.ToString("MM/dd/yyyy") + " " + now.ToLongTimeString() + " " + Utils.CurrentTimeZoneName);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
