// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.AIQPropertyRentalData
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class AIQPropertyRentalData
  {
    public bool isSubjectProperty { get; set; }

    public string addressLineText { get; set; }

    public string cityName { get; set; }

    public string stateCode { get; set; }

    public string postalCode { get; set; }

    public string losId { get; set; }

    public string monthlyPayment { get; set; }

    public string paymentType { get; set; }

    public bool belongsToBoth { get; set; }

    public bool isSelected { get; set; }

    public int linkedEncompassVOMBlockNumber { get; set; }

    public bool ignoreForImport { get; set; }

    public AIQPropertyRentalData(AIQPropertyRentalData data)
    {
      this.linkedEncompassVOMBlockNumber = data.linkedEncompassVOMBlockNumber;
      this.isSubjectProperty = data.isSubjectProperty;
      this.addressLineText = data.addressLineText;
      this.cityName = data.cityName;
      this.stateCode = data.stateCode;
      this.postalCode = data.postalCode;
      this.ignoreForImport = true;
      this.monthlyPayment = data.monthlyPayment;
      this.paymentType = data.paymentType;
    }

    public AIQPropertyRentalData(JAIQPropertyRental data)
    {
      this.addressLineText = this.cityName = this.stateCode = this.postalCode = this.losId = string.Empty;
      this.linkedEncompassVOMBlockNumber = -1;
      this.isSubjectProperty = data.type == "SUBJECT_PROPERTY";
      if (data.address != null)
      {
        this.addressLineText = data.address.addressLineText;
        this.cityName = data.address.cityName;
        this.stateCode = data.address.stateCode;
        this.postalCode = data.address.postalCode;
      }
      this.losId = data.losId;
      this.ignoreForImport = false;
      IList<JAIQCurrentIncome> currentIncome = data?.currentIncome;
      if (currentIncome == null)
        return;
      foreach (JAIQCurrentIncome jaiqCurrentIncome in (IEnumerable<JAIQCurrentIncome>) currentIncome)
      {
        if (jaiqCurrentIncome.selected)
        {
          this.monthlyPayment = jaiqCurrentIncome.monthlyTotalAmount.ToString("N2");
          this.paymentType = jaiqCurrentIncome.type;
        }
      }
    }

    public string GetField(string id) => id == "AIQFM0020" ? this.monthlyPayment : "";

    public void import(LoanData loan, bool bOrC)
    {
      try
      {
        string str1 = "FM";
        int encompassVomBlockNumber;
        if (this.linkedEncompassVOMBlockNumber == -1)
        {
          this.linkedEncompassVOMBlockNumber = loan.NewMortgage("") + 1;
          if (this.belongsToBoth)
          {
            LoanData loanData = loan;
            string str2 = str1;
            encompassVomBlockNumber = this.linkedEncompassVOMBlockNumber;
            string str3 = encompassVomBlockNumber.ToString("00");
            string id = str2 + str3 + "46";
            loanData.SetField(id, "Both");
          }
          else if (bOrC)
          {
            LoanData loanData = loan;
            string str4 = str1;
            encompassVomBlockNumber = this.linkedEncompassVOMBlockNumber;
            string str5 = encompassVomBlockNumber.ToString("00");
            string id = str4 + str5 + "46";
            loanData.SetField(id, "Borrower");
          }
          else
          {
            LoanData loanData = loan;
            string str6 = str1;
            encompassVomBlockNumber = this.linkedEncompassVOMBlockNumber;
            string str7 = encompassVomBlockNumber.ToString("00");
            string id = str6 + str7 + "46";
            loanData.SetField(id, "CoBorrower");
          }
        }
        string str8 = str1;
        encompassVomBlockNumber = this.linkedEncompassVOMBlockNumber;
        string str9 = encompassVomBlockNumber.ToString("00");
        string str10 = str8 + str9;
        loan.SetField(str10 + "20", this.monthlyPayment);
        loan.SetField(str10 + "50", this.addressLineText);
        loan.SetField(str10 + "06", this.cityName);
        loan.SetField(str10 + "07", this.stateCode);
        loan.SetField(str10 + "08", this.postalCode);
        loan.SetField(str10 + "28", this.isSubjectProperty ? "Y" : "");
        DateTime now = DateTime.Now;
        loan.SetField(str10 + "60", "Income Analyzer " + now.ToString("MM/dd/yyyy") + " " + now.ToLongTimeString() + " " + Utils.CurrentTimeZoneName);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
