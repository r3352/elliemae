// Decompiled with JetBrains decompiler
// Type: Encompass.Export.CWLedaValidate
// Assembly: Export.Validate, Version=1.0.7933.30763, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 617E5049-06C8-448B-B2D8-44769B16A732
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EMN\Export.Validate.dll

using EllieMae.EMLite.Export;
using System;
using System.Windows.Forms;

#nullable disable
namespace Encompass.Export
{
  internal class CWLedaValidate
  {
    private IBam loanData;
    private RequiredList missingFields = new RequiredList();

    internal CWLedaValidate(IBam loanData) => this.loanData = loanData;

    internal bool ValidateData(bool allowContinue)
    {
      this.CheckLeda();
      return !(this.missingFields.List != string.Empty) || new ValidationDialog(this.missingFields.List, allowContinue, this.loanData).ShowDialog() == DialogResult.OK;
    }

    internal string ValidateDataList()
    {
      this.CheckLeda();
      return this.missingFields.List;
    }

    private void CheckLeda()
    {
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      string simpleField = this.loanData.GetSimpleField("1172");
      switch (this.loanData.GetSimpleField("608"))
      {
        case "Fixed":
          flag1 = true;
          break;
        case "AdjustableRate":
          flag2 = true;
          break;
      }
      if (this.loanData.GetSimpleField("CASASRN.X85") == "Y")
        flag3 = true;
      if (this.loanData.GetSimpleField("1172") == "HELOC")
        flag4 = true;
      bool flag5 = this.loanData.GetSimpleField("68").Trim() != "";
      this.checkBlankField("36", "Borrower First Name");
      this.checkBlankField("37", "Borrower Last Name");
      this.checkBlankField("1417", "Borrower Mailing City");
      this.checkBlankField("12", "Property City");
      this.checkBlankField("1396", "Property County Code");
      this.checkBlankField("1419", "Borrower Mailing Postal Code");
      this.checkBlankField("15", "Property Postal Code");
      this.checkBlankField("1418", "Borrower Mailing State");
      this.checkBlankField("14", "Property State");
      this.checkBlankField("11", "Property Street");
      this.checkBlankField("1416", "Borrower Mailing Street");
      this.checkBlankField("66", "Borrower Home Telephone Number");
      this.checkBlankField("65", "Borrower SSN");
      if (flag5)
      {
        this.checkBlankField("68", "Co-Borrower First Name");
        this.checkBlankField("69", "Co-Borrower Last Name");
        this.checkBlankField("1520", "Co-Borrower Mailing City");
        this.checkBlankField("1522", "Co-Borrower Mailing Postal Code");
        this.checkBlankField("1521", "Co-Borrower Mailing State");
        this.checkBlankField("1519", "Co-Borrower Mailing Street");
        this.checkBlankField("97", "Co-Borrower SSN");
        this.checkBlankField("70", "Co-Borrower Age");
        this.checkBlankField("98", "Co-Borrower Home Telephone Number");
        if (this.loanData.GetSimpleField("98").Trim() == "")
          this.checkBlankField("CE0117", "Co-Borrower Employer Telephone Number");
      }
      if (flag2 && this.doubleValue(this.loanData.GetSimpleField("688")) == 0.0)
        this.missingFields.Add("688", "ARM Index Value");
      if (this.doubleValue(this.loanData.GetSimpleField("3")) == 0.0)
        this.missingFields.Add("3", "Note Rate");
      if (simpleField == "VA" || simpleField == "FHA")
        this.checkBlankField("1040", "Agency Case No");
      if ((flag2 || flag3 || flag4) && this.doubleValue(this.loanData.GetSimpleField("247")) == 0.0)
        this.missingFields.Add("247", "ARM Life Cap");
      if (this.loanData.GetSimpleField("420") == "SecondLien" && this.loanData.GetSimpleField("14") == "TX" && this.doubleValue(this.loanData.GetSimpleField("799")) == 0.0)
        this.missingFields.Add("799", "Annual Percentage Rate");
      if (flag4 && this.doubleValue(this.loanData.GetSimpleField("1891")) == 0.0)
        this.missingFields.Add("1891", "HELOC Annual Fee");
      if (flag4 && this.doubleValue(this.loanData.GetSimpleField("1483")) == 0.0)
        this.missingFields.Add("1483", "HELOC Minimum Payment");
      if (flag4 && this.doubleValue(this.loanData.GetSimpleField("1986")) == 0.0)
        this.missingFields.Add("1986", "HELOC Termination Fee");
      if (flag2 && this.doubleValue(this.loanData.GetSimpleField("696")) == 0.0)
        this.missingFields.Add("696", "ARM 1st Change");
      if (flag4 && this.doubleValue(this.loanData.GetSimpleField("CASASRN.X168")) == 0.0)
        this.missingFields.Add("CASASRN.X168", "HELOC Credit Limit");
      if (this.loanData.GetSimpleField("66").Trim() == "")
        this.checkBlankField("BE0117", "Borrower Employer Telephone Number");
      this.checkBlankField("1500", "Flood Insurance Provider Name");
      if ((flag2 || flag3 || flag4) && this.doubleValue(this.loanData.GetSimpleField("1699")) == 0.0)
        this.missingFields.Add("1699", "ARM Floor Rate");
      if ((flag2 || flag3 || flag1) && this.doubleValue(this.loanData.GetSimpleField("642")) == 0.0)
        this.missingFields.Add("642", "Hazard Insurance Amount");
      if (flag2 || flag3 || flag1)
        this.checkBlankField("VEND.X166", "Hazard Insurance Reference #");
      if (flag4 && this.intValue(this.loanData.GetSimpleField("1889")) == 0)
        this.missingFields.Add("1889", "HELOC Draw Period");
      this.checkBlankField("672", "Late Charge Grace Period");
      if (this.doubleValue(this.loanData.GetSimpleField("674")) == 0.0)
        this.missingFields.Add("674", "Late Charge Rate");
      if (this.intValue(this.loanData.GetSimpleField("4")) == 0)
        this.missingFields.Add("4", "Loan Term");
      this.checkBlankField("1961", "Final Payment Date");
      if ((flag2 || flag3 || flag4) && this.doubleValue(this.loanData.GetSimpleField("689")) == 0.0)
        this.missingFields.Add("689", "ARM Index Margin");
      if (flag2 && this.doubleValue(this.loanData.GetSimpleField("247")) == 0.0)
        this.missingFields.Add("247", "ARM Life Cap");
      if (flag4 && this.doubleValue(this.loanData.GetSimpleField("1892")) == 0.0)
        this.missingFields.Add("1892", "HELOC Min. Advance Amt");
      if (flag3 && this.doubleValue(this.loanData.GetSimpleField("698")) == 0.0)
        this.missingFields.Add("698", "Negative Amortization Limit Percent");
      if (this.doubleValue(this.loanData.GetSimpleField("1109")) == 0.0)
        this.missingFields.Add("1109", "Loan Amount");
      if (this.doubleValue(this.loanData.GetSimpleField("5")) == 0.0)
        this.missingFields.Add("5", "Principal and Interest Payment Amount");
      if (flag3 && this.doubleValue(this.loanData.GetSimpleField("691")) == 0.0)
        this.missingFields.Add("691", "Negative Amortization Adj Cap");
      if ((flag2 || flag3 || flag4) && this.doubleValue(this.loanData.GetSimpleField("1700")) == 0.0)
        this.missingFields.Add("1700", "ARM Rounding Factor");
      if ((flag2 || flag3 || flag4) && this.intValue(this.loanData.GetSimpleField("694")) == 0)
        this.missingFields.Add("694", "ARM Adjustment Period");
      if ((flag2 || flag3) && this.doubleValue(this.loanData.GetSimpleField("695")) == 0.0)
        this.missingFields.Add("695", "ARM Adjustment Cap");
      if ((flag2 || flag3 || flag1) && this.doubleValue(this.loanData.GetSimpleField("356")) == 0.0)
        this.missingFields.Add("356", "Appraised Value");
      if (this.intValue(this.loanData.GetSimpleField("16")) == 0)
        this.missingFields.Add("16", "No Units");
      if (this.loanData.GetSimpleField("1884").Trim() != "Y")
        this.checkBlankField("1846", "Property Long Legal Description");
      if ((flag2 || flag3 || flag1) && this.doubleValue(this.loanData.GetSimpleField("136")) == 0.0)
        this.missingFields.Add("136", "Purchase Price");
      this.checkBlankField("682", "1st Payment Date");
      if (this.loanData.GetSimpleField("FM0128").Trim() == "Y" && this.doubleValue(this.loanData.GetSimpleField("FM0117")) == 0.0)
        this.missingFields.Add("FM0117", "Unpaid Principal Balance");
      if (this.loanData.GetSimpleField("425") == "Y" && flag1)
      {
        this.checkBlankField("CASASRN.X141", "Buydown Contributor Type");
        if (this.intValue(this.loanData.GetSimpleField("1613")) == 0)
          this.missingFields.Add("1613", "Buydown Duration Months");
      }
      if (flag4)
        this.checkBlankField("MORNET.X67", "Loan Documentation Type Code");
      if ((flag2 || flag3 || flag4) && this.intValue(this.loanData.GetSimpleField("696")) == 0)
        this.missingFields.Add("696", "ARM First Change Months");
      this.checkBlankField("1719", "Late Charge Type");
      this.checkBlankField("420", "Lien Position");
      this.checkBlankField("608", "Amortization Type");
      this.checkBlankField("19", "Purpose of Loan");
      this.checkBlankField("1172", "Loan Type");
      if (flag2 || flag3 || flag4)
        this.checkBlankField("CASASRN.X135", "ARM Index Type");
      if (flag2 || flag3 || flag4)
        this.checkBlankField("SYS.X1", "ARM Rounding Type");
      this.checkBlankField("1041", "Property Type");
      this.checkBlankField("1811", "Property Usage Type");
      if (!(simpleField == "VA") && !(simpleField == "FHA") || !flag2 && !flag3 && !flag1)
        return;
      this.checkBlankField("1039", "Section of Act Type");
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
