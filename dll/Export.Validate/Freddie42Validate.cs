// Decompiled with JetBrains decompiler
// Type: Encompass.Export.Freddie42Validate
// Assembly: Export.Validate, Version=1.0.7933.30763, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 617E5049-06C8-448B-B2D8-44769B16A732
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EMN\Export.Validate.dll

using EllieMae.EMLite.Export;
using System;
using System.Windows.Forms;

#nullable disable
namespace Encompass.Export
{
  internal class Freddie42Validate
  {
    private IBam loanData;
    private RequiredList missingFields = new RequiredList();

    internal Freddie42Validate(IBam loanData) => this.loanData = loanData;

    internal bool ValidateData(bool allowContinue)
    {
      this.checkLP();
      return !(this.missingFields.List != string.Empty) || new ValidationDialog(this.missingFields.List, allowContinue, this.loanData).ShowDialog() == DialogResult.OK;
    }

    internal string ValidateDataList()
    {
      this.checkLP();
      return this.missingFields.List;
    }

    private void checkLP()
    {
      this.checkBlankField("2", "Borrower requested loan amount");
      this.checkBlankField("3", "Interest rate");
      this.checkBlankField("4", "Loan term");
      this.checkBlankField("16", "Property number of units");
      this.checkBlankField("19", "Loan purpose");
      this.checkBlankField("36", "Borrower first name");
      this.checkBlankField("37", "Borrower last name");
      this.checkBlankField("38", "Borrower age");
      this.checkBlankField("52", "Borrower martial status");
      this.checkBlankField("65", "Borrower SSN");
      this.checkBlankField("315", "Company name");
      this.checkBlankField("317", "Loan officer");
      this.checkBlankField("325", "Due in");
      this.checkBlankField("356", "Appraised value");
      this.checkBlankField("364", "Loan number");
      this.checkBlankField("420", "Lien position");
      this.checkBlankField("608", "Amortization type");
      this.checkBlankField("1109", "Base loan amount");
      this.checkBlankField("1172", "Loan type - Must be: Conventional, FHA, VA");
      this.checkBlankField("1402", "Borrower date of birth");
      this.checkBlankField("1811", "Property will be");
      this.checkBlankField("CASASRN.X107", "Freddie processing point");
      this.checkBlankField("CASASRN.X14", "Freddie property type");
      this.checkBlankField("FR0104", "Borrower present address");
      this.checkBlankField("FR0106", "Borrower present city");
      this.checkBlankField("FR0107", "Borrower present state");
      this.checkBlankField("FR0108", "Borrower present zip");
      if (this.loanData.GetSimpleField("1416") != string.Empty)
      {
        this.checkBlankField("1417", "Borrower mailing city");
        this.checkBlankField("1418", "Borrower mailing state");
        this.checkBlankField("1419", "Borrower mailing zip");
      }
      bool flag = this.loanData.GetSimpleField("68") != string.Empty;
      if (flag)
      {
        this.checkBlankField("68", "Coborrower first name");
        this.checkBlankField("69", "Coborrower last name");
        this.checkBlankField("97", "Coborrower SSN");
        this.checkBlankField("70", "Coborrower age");
        this.checkBlankField("1403", "Coborrower date of birth");
      }
      if (this.loanData.GetSimpleField("479") == "FaceToFace")
      {
        if (this.loanData.GetSimpleField("188") != "Y")
        {
          this.checkBlankField("471", "Borrower sex");
          if (this.loanData.GetSimpleField("1523") != "HispanicOrLatino" && this.loanData.GetSimpleField("1523") != "NotHispanicOrLatino")
            this.checkBlankField("1523", "Borrower ethnicity");
          if (this.loanData.GetSimpleField("1524") != "Y" && this.loanData.GetSimpleField("1525") != "Y" && this.loanData.GetSimpleField("1526") != "Y" && this.loanData.GetSimpleField("1527") != "Y" && this.loanData.GetSimpleField("1528") != "Y")
            this.checkBlankField("1524", "Borrower race");
        }
        if (flag && this.loanData.GetSimpleField("189") != "Y")
        {
          this.checkBlankField("478", "Coborrower sex");
          if (this.loanData.GetSimpleField("1531") != "HispanicOrLatino" && this.loanData.GetSimpleField("1531") != "NotHispanicOrLatino")
            this.checkBlankField("1531", "Coborrower ethnicity");
          if (this.loanData.GetSimpleField("1532") != "Y" && this.loanData.GetSimpleField("1533") != "Y" && this.loanData.GetSimpleField("1534") != "Y" && this.loanData.GetSimpleField("1535") != "Y" && this.loanData.GetSimpleField("1536") != "Y")
            this.checkBlankField("1532", "Coborrower race");
        }
      }
      if (this.loanData.GetSimpleField("1172") == "FHA")
      {
        this.checkBlankField("1059", "FHA lender identifier");
        this.checkBlankField("1060", "FHA sponser identifier");
        this.checkBlankField("1132", "Borrower paid closing cost");
        this.checkBlankField("1040", "Agency case number");
        if (this.loanData.GetSimpleField("CASASRN.X88") != "Y" && this.loanData.GetSimpleField("CASASRN.X13") == string.Empty && this.loanData.GetSimpleField("CASASRN.X200") == string.Empty)
          this.missingFields.Add("CASASRN.X88", "Merged Credit is required for FHA loans");
      }
      if (this.loanData.GetSimpleField("1172") == "VA")
        this.checkBlankField("1325", "VA residual income");
      if (this.loanData.GetSimpleField("425") == "Y")
      {
        this.checkBlankField("1613", "Buydown months per adjustment");
        this.checkBlankField("1269", "Buydown rate percent");
      }
      if (this.loanData.GetSimpleField("608") == "AdjustableRate")
      {
        this.checkBlankField("688", "ARM Index value percent");
        this.checkBlankField("689", "ARM Index margin percent");
        this.checkBlankField("1014", "Qualifying rate");
        this.checkBlankField("247", "ARM life cap percent");
        if (this.loanData.GetSimpleField("690") != string.Empty)
        {
          this.checkBlankField("691", "Payment adjustment cap percent");
        }
        else
        {
          this.checkBlankField("694", "Rate adjustment period months");
          this.checkBlankField("695", "Rate adjustment cap percent");
          this.checkBlankField("696", "Rate 1st change months");
        }
      }
      if (this.loanData.GetSimpleField("19").IndexOf("Refinance") >= 0)
      {
        this.checkBlankField("24", "Refi year aquired");
        this.checkBlankField("26", "Refi existing lien");
        this.checkBlankField("25", "Refi original cost");
        if (this.isCashOut())
          this.checkBlankField("CASASRN.X79", "Freddie cash out amount");
      }
      if (this.loanData.GetSimpleField("CASASRN.X107") != "Prequal (No URLA)" || this.loanData.GetSimpleField("CASASRN.X89") == "1" || this.loanData.GetSimpleField("CASASRN.X89") == "2" || this.loanData.GetSimpleField("1172") == "FHA" || this.loanData.GetSimpleField("1172") == "VA")
      {
        this.checkBlankField("11", "Property address");
        this.checkBlankField("12", "Property city");
        this.checkBlankField("14", "Property state");
        this.checkBlankField("15", "Property zip");
        this.checkBlankField("13", "Property county");
      }
      if (this.loanData.GetSimpleField("CASASRN.X107") != "Prequal (No URLA)")
        this.checkBlankField("601", "Building status type");
      if (this.loanData.GetSimpleField("228") == string.Empty && this.loanData.GetSimpleField("228") == string.Empty && this.loanData.GetSimpleField("229") == string.Empty && this.loanData.GetSimpleField("1405") == string.Empty && this.loanData.GetSimpleField("232") == string.Empty && this.loanData.GetSimpleField("233") == string.Empty && this.loanData.GetSimpleField("234") == string.Empty)
        this.checkBlankField("228", "Must have proposed housing expense");
      if (this.loanData.GetSimpleField("119") == string.Empty && this.loanData.GetSimpleField("120") == string.Empty && this.loanData.GetSimpleField("121") == string.Empty && this.loanData.GetSimpleField("122") == string.Empty && this.loanData.GetSimpleField("123") == string.Empty && this.loanData.GetSimpleField("124") == string.Empty && this.loanData.GetSimpleField("125") == string.Empty && this.loanData.GetSimpleField("126") == string.Empty)
        this.checkBlankField("119", "Must have present housing expense");
      int numberOfMortgages = this.loanData.GetNumberOfMortgages();
      for (int index = 1; index <= numberOfMortgages; ++index)
        this.checkBlankField("FM" + index.ToString("00") + "18", "VOM property type");
      if (this.loanData.GetSimpleField("19") == "Purchase")
        this.checkBlankField("136", "Purchase price");
      if (this.loanData.GetSimpleField("BR0104") != string.Empty && this.intValue(this.loanData.GetSimpleField("BR0112")) < 2)
        this.checkBlankField("BR0204", "Borrower prior address required when less then 2 years at current");
      if (flag && this.loanData.GetSimpleField("CR0104") != string.Empty && this.intValue(this.loanData.GetSimpleField("CR0112")) < 2)
        this.checkBlankField("CR0204", "CoBorrower prior address required when less then 2 years at current");
      double num1 = this.doubleValue(this.loanData.GetSimpleField("CASASRN.X167"));
      double num2 = this.doubleValue(this.loanData.GetSimpleField("CASASRN.X168"));
      if (num1 == 0.0 && num2 == 0.0)
        return;
      this.checkBlankField("CASASRN.X167", "Heloc Subordinate Lien Amount");
      this.checkBlankField("CASASRN.X168", "Heloc Max Balance Amount");
      if (num2 > num1)
        return;
      this.missingFields.Add("CASASRN.X168", "Value must be greater than CASASRN.X167");
    }

    private bool isCashOut()
    {
      string simpleField = this.loanData.GetSimpleField("299");
      return simpleField == string.Empty && (this.loanData.GetSimpleField("19") == "Cash-Out Refinance" || simpleField.StartsWith("CashOut"));
    }

    private void checkBlankField(string id, string desc)
    {
      string simpleField = this.loanData.GetSimpleField(id);
      int num = desc.IndexOf(" - Must be: ");
      if (num >= 0)
      {
        bool flag = false;
        string str1 = desc.Substring(num + 12);
        char[] chArray = new char[1]{ ',' };
        foreach (string str2 in str1.Split(chArray))
        {
          if (simpleField.ToUpper().Trim() == str2.ToUpper().Trim())
          {
            flag = true;
            break;
          }
        }
        if (flag)
          return;
        this.missingFields.Add(id, desc);
      }
      else
      {
        if (simpleField != null && !(simpleField == string.Empty) && !(simpleField == "0") && !(simpleField == "0.00") && !(simpleField == "0.000"))
          return;
        this.missingFields.Add(id, desc);
      }
    }

    private void checkBlankUserSetting(string id, string desc)
    {
      string userSetting = this.loanData.GetUserSetting("LP", id);
      if (userSetting != null && !(userSetting == string.Empty))
        return;
      this.missingFields.Add(id, desc);
    }

    private void checkLengthOfUserSetting(string id, string desc, int length)
    {
      if (this.loanData.GetUserSetting("LP", id).Length == length)
        return;
      this.missingFields.Add(id, desc);
    }

    private int intValue(string val)
    {
      if (val == string.Empty)
        return 0;
      try
      {
        return int.Parse(val.Replace(",", string.Empty).Replace("]", string.Empty));
      }
      catch (Exception ex)
      {
        return 0;
      }
    }

    private double doubleValue(string val)
    {
      if (val == string.Empty || val == "Infinity")
        return 0.0;
      if (!val.StartsWith("*"))
      {
        if (!val.StartsWith("("))
          goto label_5;
      }
      val = val.Substring(1);
      val = val.Replace(")", "");
label_5:
      try
      {
        return double.Parse(val.Replace(",", string.Empty));
      }
      catch (Exception ex)
      {
        return 0.0;
      }
    }
  }
}
