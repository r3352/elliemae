// Decompiled with JetBrains decompiler
// Type: Encompass.Export.FannieValidate
// Assembly: Export.Validate, Version=1.0.7933.30763, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 617E5049-06C8-448B-B2D8-44769B16A732
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EMN\Export.Validate.dll

using EllieMae.EMLite.Export;
using System;
using System.Windows.Forms;

#nullable disable
namespace Encompass.Export
{
  internal class FannieValidate
  {
    private IBam loanData;
    private RequiredList missingFields = new RequiredList();
    private string borPairMsg = string.Empty;
    private FannieValidate.loanInformation loanInfo;

    internal FannieValidate(IBam loanData) => this.loanData = loanData;

    internal bool ValidateData(bool allowContinue)
    {
      this.buildList();
      return !(this.missingFields.List != string.Empty) || new ValidationDialog(this.missingFields.List, allowContinue, this.loanData).ShowDialog() == DialogResult.OK;
    }

    internal string ValidateDataList()
    {
      this.buildList();
      return this.missingFields.List;
    }

    private void buildList()
    {
      this.CheckBasic();
      this.Check01A();
      this.Check02A();
      this.Check02B();
      this.Check02D();
      this.Check02E();
      this.Check07A();
      this.Check08A();
      this.Check99B();
      this.CheckLNC();
      this.CheckOthers();
      int numberOfBorrowerPairs = this.loanData.GetNumberOfBorrowerPairs();
      for (int index = 1; index <= numberOfBorrowerPairs; ++index)
      {
        this.loanData.SetBorrowerPair(index - 1);
        if (index > 1)
          this.borPairMsg = " (Co-Mortgagor " + (object) index + ")";
        this.loanInfo.WithCob = !this.IsBlank("68") || !this.IsBlank("69");
        this.Check03A();
        this.Check03B();
        this.Check03C();
        this.Check04AAnd04B();
        this.Check06C();
        this.Check06L();
        this.Check06G();
      }
      this.loanData.SetBorrowerPair(0);
    }

    private void CheckBasic()
    {
      this.loanInfo.LoanType = this.loanData.GetSimpleField("1172");
      this.loanInfo.ARMType = this.loanData.GetSimpleField("608");
      this.loanInfo.Purpose = this.loanData.GetSimpleField("19");
    }

    private void Check01A()
    {
      if (this.loanInfo.LoanType == string.Empty)
        this.missingFields.Add("1172", "Mortgage Type?C:Conventional:FHA:VA:Other:FarmersHomeAdministration");
      if ((this.loanInfo.LoanType == "FHA" || this.loanInfo.LoanType == "VA") && this.IsBlank("1040"))
        this.missingFields.Add("1040", "Agency Case Identifier?T");
      if (this.IsZero("2"))
        this.missingFields.Add("2", "Loan Amount?N");
      if (this.IsBlank("3"))
        this.missingFields.Add("3", "Interest Rate?N");
      if (this.IsBlank("4"))
        this.missingFields.Add("4", "Loan Term (1~360)?N");
      if (this.loanInfo.ARMType == string.Empty)
      {
        this.missingFields.Add("608", "Amortization Type?C:AdjustableRate:Fixed:GraduatedPaymentMortgage:GrowingEquityMortgage:OtherAmortizationType");
      }
      else
      {
        if (!(this.loanInfo.ARMType == "OtherAmortizationType"))
          return;
        if (this.IsBlank("994"))
          this.missingFields.Add("994", "Other Amortization Type Description?T");
        if (!this.IsBlank("248"))
          return;
        this.missingFields.Add("248", "ARM Description?T");
      }
    }

    private void Check02A()
    {
      if (this.IsBlank("11"))
        this.missingFields.Add("11", "Property Street?T");
      if (this.IsBlank("12"))
        this.missingFields.Add("12", "Property City?T");
      if (this.IsBlank("14"))
        this.missingFields.Add("14", "Property State (CA,TX, etc...)?T");
      if (this.IsBlank("15"))
        this.missingFields.Add("15", "Property Zipcode?N");
      if (!this.IsBlank("16"))
        return;
      this.missingFields.Add("16", "Financed Number Of Units?N");
    }

    private void Check02B()
    {
      if (this.loanInfo.Purpose == string.Empty)
        this.missingFields.Add("19", "Purpose Of Loan?C:Purchase:Refinance:ConstructionOnly:ConstructionToPermanent:Other");
      if (this.IsBlank("1811") || this.loanData.GetSimpleField("1811") == "N")
        this.missingFields.Add("1811", "Property Usage Type?C:PrimaryResidence:SecondHome:Investor");
      if (!this.IsBlank("1066"))
        return;
      this.missingFields.Add("1066", "Estate will be held in?C:FeeSimple:Leasehold");
    }

    private void Check02D()
    {
      if (this.loanInfo.Purpose.IndexOf("Refinance") == -1 || !this.IsBlank("299"))
        return;
      this.missingFields.Add("299", "Refinance Purpose Type?T");
    }

    private void Check02E()
    {
      if (this.loanInfo.Purpose != "Purchase")
        return;
      if (this.IsBlank("34"))
        this.missingFields.Add("34", "Down Payment Type Code?T");
      if (!this.IsBlank("1335"))
        return;
      this.missingFields.Add("1335", "Down Payment Amount?N");
    }

    private void Check03A()
    {
      if (this.IsBlank("36"))
        this.missingFields.Add("36", "Primary Applicant First Name" + this.borPairMsg + "?T");
      if (this.IsBlank("37"))
        this.missingFields.Add("37", "Primary Applicant Last Name" + this.borPairMsg + "?T");
      if (this.IsBlank("65"))
        this.missingFields.Add("65", "Primary SSN#" + this.borPairMsg + "?T");
      if (!this.loanInfo.WithCob)
        return;
      if (this.IsBlank("68"))
        this.missingFields.Add("68", "Co-Applicant First name" + this.borPairMsg + "?T");
      if (this.IsBlank("69"))
        this.missingFields.Add("69", "Co-Applicant Last name" + this.borPairMsg + "?T");
      if (!this.IsBlank("97"))
        return;
      this.missingFields.Add("97", "Co-Applicant SSN#" + this.borPairMsg + "?T");
    }

    private void Check03B()
    {
      if (!this.IsZero("53") && this.IsBlank("54"))
        this.missingFields.Add("54", "Applicant Dependent's Age" + this.borPairMsg + "?T");
      else if (this.IsZero("53") && !this.IsBlank("54"))
        this.missingFields.Add("53", "Applicant Number of Dependents" + this.borPairMsg + "?T");
      else if (!this.IsZero("53") && !this.IsBlank("54"))
      {
        if (this.intValue(this.loanData.GetSimpleField("53")) != this.loanData.GetSimpleField("54").Split(',').Length)
          this.missingFields.Add("53", "Applicant # of Deps. must match # of Ages" + this.borPairMsg + "?T");
      }
      if (!this.loanInfo.WithCob)
        return;
      if (!this.IsZero("85") && this.IsBlank("86"))
        this.missingFields.Add("86", "Co-applicant Dependent's Age" + this.borPairMsg + "?T");
      else if (this.IsZero("85") && !this.IsBlank("86"))
      {
        this.missingFields.Add("85", "Co-applicant Number of Dependents" + this.borPairMsg + "?T");
      }
      else
      {
        if (this.IsZero("85") || this.IsBlank("86"))
          return;
        if (this.intValue(this.loanData.GetSimpleField("85")) == this.loanData.GetSimpleField("86").Split(',').Length)
          return;
        this.missingFields.Add("85", "Co-applicant # of Deps. must match # of Ages" + this.borPairMsg + "?T");
      }
    }

    private void Check03C()
    {
      if (this.IsBlank("FR0104"))
        this.missingFields.Add("FR0104", "Primary Applicant Street Address" + this.borPairMsg + "?T");
      if (this.IsBlank("FR0106"))
        this.missingFields.Add("FR0106", "Primary Applicant City Address" + this.borPairMsg + "?T");
      if (this.IsBlank("FR0107"))
        this.missingFields.Add("FR0107", "Primary Applicant State" + this.borPairMsg + "?T");
      if (this.IsBlank("FR0108"))
        this.missingFields.Add("FR0108", "Primary Applicant Zip code" + this.borPairMsg + "?T");
      if (this.IsBlank("FR0112"))
        this.missingFields.Add("FR0112", "Primary Applicant Residency Duration Years" + this.borPairMsg + "?N");
      if (!this.loanInfo.WithCob)
        return;
      if (this.IsBlank("FR0204"))
        this.missingFields.Add("FR0204", "Co-applicant Street Address" + this.borPairMsg + "?T");
      if (this.IsBlank("FR0206"))
        this.missingFields.Add("FR0206", "Co-applicant City" + this.borPairMsg + "?T");
      if (this.IsBlank("FR0207"))
        this.missingFields.Add("FR0207", "Co-applicant State" + this.borPairMsg + "?T");
      if (this.IsBlank("FR0208"))
        this.missingFields.Add("FR0208", "Co-applicant Zip code" + this.borPairMsg + "?T");
      if (!this.IsBlank("FR0212"))
        return;
      this.missingFields.Add("FR0212", "Co-applicant Residency Duration Years" + this.borPairMsg + "?N");
    }

    private void Check04AAnd04B()
    {
      int numberOfEmployer = this.loanData.GetNumberOfEmployer(true);
      for (int index = 1; index <= numberOfEmployer; ++index)
      {
        if (!this.IsBlank("BE" + index.ToString("00") + "02"))
        {
          string id1 = "BE" + index.ToString("00") + "09";
          if (this.loanData.GetSimpleField(id1) == "N")
          {
            string id2 = "BE" + index.ToString("00") + "13";
            if (this.IsBlank(id2))
              this.missingFields.Add(id2, "Current Employment Years On Job" + this.borPairMsg + "?N");
            string id3 = "BE" + index.ToString("00") + "11";
            if (this.IsBlank(id3))
              this.missingFields.Add(id3, "Previous Employment Start Date" + this.borPairMsg + "?T");
            string id4 = "BE" + index.ToString("00") + "14";
            if (this.IsBlank(id4))
              this.missingFields.Add(id4, "Previous Employment End Date" + this.borPairMsg + "?T");
            string id5 = "BE" + index.ToString("00") + "12";
            if (this.IsBlank(id5))
              this.missingFields.Add(id5, "Income Employment Monthly Amount" + this.borPairMsg + "?N");
          }
          else if (this.loanData.GetSimpleField(id1) == string.Empty)
            this.missingFields.Add(id1, "Employer Name" + this.borPairMsg + "?T");
        }
      }
    }

    private void Check06C()
    {
      if (!this.IsBlank("1604") && this.IsZero("1605"))
        this.missingFields.Add("1605", "Stock/Bond Cash Or Market Value Amount" + this.borPairMsg + "?N");
      if (this.IsBlank("1604") && !this.IsZero("1605"))
        this.missingFields.Add("1604", "Stock/Bond Holder Name" + this.borPairMsg + "?T");
      if (!this.IsBlank("1606") && this.IsZero("1607"))
        this.missingFields.Add("1607", "Stock/Bond Cash Or Market Value Amount" + this.borPairMsg + "?N");
      if (this.IsBlank("1606") && !this.IsZero("1607"))
        this.missingFields.Add("1606", "Stock/Bond Holder Name" + this.borPairMsg + "?T");
      if (!this.IsBlank("1608") && this.IsZero("1609"))
        this.missingFields.Add("1609", "Stock/Bond Cash Or Market Value Amount" + this.borPairMsg + "?N");
      if (this.IsBlank("1608") && !this.IsZero("1609"))
        this.missingFields.Add("1608", "Stock/Bond Holder Name" + this.borPairMsg + "?T");
      int numberOfDeposits = this.loanData.GetNumberOfDeposits();
      for (int index = 1; index <= numberOfDeposits; ++index)
      {
        if (this.IsBlank("DD" + index.ToString("00") + "02"))
          this.missingFields.Add("DD" + index.ToString("00") + "02", "Deposit Account Holder Name" + this.borPairMsg + "?T");
        if (this.IsBlank("DD" + index.ToString("00") + "24"))
          this.missingFields.Add("DD" + index.ToString("00") + "24", "Person of Deposit Account" + this.borPairMsg + "?T");
        if (!this.IsZero("DD" + index.ToString("00") + "11") && this.IsBlank("DD" + index.ToString("00") + "08"))
          this.missingFields.Add("DD" + index.ToString("00") + "08", "Deposit Account Type1 #1" + this.borPairMsg + "?T");
        if (!this.IsZero("DD" + index.ToString("00") + "15") && this.IsBlank("DD" + index.ToString("00") + "12"))
          this.missingFields.Add("DD" + index.ToString("00") + "12", "Deposit Account Type1 #2" + this.borPairMsg + "?T");
        if (!this.IsZero("DD" + index.ToString("00") + "19") && this.IsBlank("DD" + index.ToString("00") + "16"))
          this.missingFields.Add("DD" + index.ToString("00") + "16", "Deposit Account Type1 #3" + this.borPairMsg + "?T");
        if (!this.IsZero("DD" + index.ToString("00") + "23") && this.IsBlank("DD" + index.ToString("00") + "20"))
          this.missingFields.Add("DD" + index.ToString("00") + "20", "Deposit Account Type1 #4" + this.borPairMsg + "?T");
      }
    }

    private void Check06L()
    {
      int exlcudingAlimonyJobExp = this.loanData.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
      {
        if (!this.IsZero("FL" + index.ToString("00") + "13"))
        {
          if (this.IsBlank("FL" + index.ToString("00") + "02"))
            this.missingFields.Add("FL" + index.ToString("00") + "02", "Account Holder of Liability" + this.borPairMsg + "?T");
          if (this.IsBlank("FL" + index.ToString("00") + "15"))
            this.missingFields.Add("FL" + index.ToString("00") + "15", "Person of Liability" + this.borPairMsg + "?T");
        }
      }
    }

    private void Check06G()
    {
      int numberOfMortgages = this.loanData.GetNumberOfMortgages();
      for (int index = 1; index <= numberOfMortgages; ++index)
      {
        if (this.IsBlank("FM" + index.ToString("00") + "19"))
          this.missingFields.Add("FM" + index.ToString("00") + "19", "(VOM) Real Estate Market Value Amount" + this.borPairMsg + "?N");
        if (this.loanInfo.Purpose.IndexOf("Refinance") > -1 && this.IsBlank("FM" + index.ToString("00") + "28"))
          this.missingFields.Add("FM" + index.ToString("00") + "28", "(VOM) Subject Indicator" + this.borPairMsg + "?T");
      }
    }

    private void Check07A()
    {
      if (this.loanInfo.Purpose != "Purchase" || !this.IsZero("136"))
        return;
      this.missingFields.Add("136", "Purchase Price Amount?N");
    }

    private void Check08A()
    {
      if (this.IsBlank("169"))
        this.missingFields.Add("169", "Declarations - Outstanding judgements?T");
      if (this.IsBlank("265"))
        this.missingFields.Add("265", "Declarations - Bankruptcy past 7 years?T");
      if (this.IsBlank("170"))
        this.missingFields.Add("170", "Declarations - Property forclosed?T");
      if (this.IsBlank("172"))
        this.missingFields.Add("172", "Declarations - Party to a lawsuit?T");
      if (this.IsBlank("1057"))
        this.missingFields.Add("1057", "Declarations - Obligated on a loan...?T");
      if (this.IsBlank("463"))
        this.missingFields.Add("463", "Declarations - Presently delinquent?T");
      if (this.IsBlank("173"))
        this.missingFields.Add("173", "Declarations - Obligated to pay alimony?T");
      if (this.IsBlank("174"))
        this.missingFields.Add("174", "Declarations - Downpayment borrowed?T");
      if (this.IsBlank("171"))
        this.missingFields.Add("171", "Declarations - Are you a co-maker?T");
      if (this.IsBlank("965"))
        this.missingFields.Add("965", "Declarations - U.S. Citizen?T");
      if (this.IsBlank("466"))
        this.missingFields.Add("466", "Declarations - Perminent resident alien?T");
      if (this.IsBlank("418"))
        this.missingFields.Add("418", "Declarations - Intend to occupy?T");
      if (this.IsBlank("403"))
        this.missingFields.Add("403", "Declarations - Had ownership in...?T");
      if (!(this.loanData.GetField("69") != string.Empty))
        return;
      if (this.IsBlank("175"))
        this.missingFields.Add("175", "Declarations - Outstanding judgements?T");
      if (this.IsBlank("266"))
        this.missingFields.Add("266", "Declarations - Bankruptcy past 7 years?T");
      if (this.IsBlank("176"))
        this.missingFields.Add("176", "Declarations - Property forclosed?T");
      if (this.IsBlank("178"))
        this.missingFields.Add("178", "Declarations - Party to a lawsuit?T");
      if (this.IsBlank("1197"))
        this.missingFields.Add("1197", "Declarations - Obligated on a loan...?T");
      if (this.IsBlank("464"))
        this.missingFields.Add("464", "Declarations - Presently delinquent?T");
      if (this.IsBlank("179"))
        this.missingFields.Add("179", "Declarations - Obligated to pay alimony?T");
      if (this.IsBlank("180"))
        this.missingFields.Add("180", "Declarations - Downpayment borrowed?T");
      if (this.IsBlank("177"))
        this.missingFields.Add("177", "Declarations - Are you a co-maker?T");
      if (this.IsBlank("985"))
        this.missingFields.Add("985", "Declarations - U.S. Citizen?T");
      if (this.IsBlank("467"))
        this.missingFields.Add("468", "Declarations - Perminent resident alien?T");
      if (this.IsBlank("1343"))
        this.missingFields.Add("1343", "Declarations - Intend to occupy?T");
      if (!this.IsBlank("1108"))
        return;
      this.missingFields.Add("1108", "Declarations - Had ownership in...?T");
    }

    private void Check99B()
    {
      if (this.IsZero("356"))
        this.missingFields.Add("356", "Property Appraised Value Amount?N");
      if (string.Compare(this.loanData.GetSimpleField("CASASRN.X141"), "Borrower", StringComparison.InvariantCultureIgnoreCase) == 0)
      {
        if (!this.IsZero("1269"))
          return;
        this.missingFields.Add("1269", "Increase Rate Percent of Buydown?N");
      }
      else
      {
        if (!(this.loanData.GetSimpleField("425") == "Y") || !this.IsZero("4535"))
          return;
        this.missingFields.Add("4535", "Loan Info Buydown Rate 1?N");
      }
    }

    private void CheckLNC()
    {
      if (this.IsBlank("1041"))
        this.missingFields.Add("1041", "Property Type?C:Attached:Condominium:Cooperative:Detached:Highrise Condominium:Manufactured Housing:PUD");
      if (!this.IsBlank("420"))
        return;
      this.missingFields.Add("420", "Lien Type?C:First:Second");
    }

    private void CheckOthers()
    {
      if (this.loanInfo.LoanType == "FHA" || this.loanInfo.LoanType == "VA")
      {
        if (this.loanInfo.Purpose.IndexOf("Refinance") != -1 && this.IsBlank("MORNET.X40"))
          this.missingFields.Add("MORNET.X40", "Government Refinance Type?T");
        if (this.IsBlank("13"))
          this.missingFields.Add("13", "Property County?T");
        if (this.IsBlank("1039"))
          this.missingFields.Add("1039", "Section Of Act Type?T");
        if (this.loanData.GetSimpleField("MORNET.X72").StartsWith("Y"))
          this.missingFields.Add("MORNET.X72", "Uncheck Community Lending checkbox for FHA or VA loans?T");
      }
      if (this.loanInfo.ARMType == "AdjustableRate")
      {
        if (this.IsBlank("688"))
          this.missingFields.Add("688", "ARM Index Current Value Percent?N");
        if (this.IsBlank("689"))
          this.missingFields.Add("689", "ARM Index Margin Percent?N");
        if (this.IsBlank("995") && this.IsBlank("1014"))
          this.missingFields.Add("1014", "ARM Qualifying Rate Percent?N");
      }
      if (this.IsBlank("364"))
        this.missingFields.Add("364", "Loan Number?T");
      if (this.loanInfo.LoanType == "FHA")
      {
        if (this.IsBlank("1059"))
          this.missingFields.Add("1059", "Lender ID?T");
        if (this.IsBlank("1060"))
          this.missingFields.Add("1060", "Sponsor ID?T");
      }
      string simpleField1 = this.loanData.GetSimpleField("3238");
      if (simpleField1 != string.Empty && (!this.isNumeric(simpleField1) || simpleField1.Length > 12))
        this.missingFields.Add("3238", "NMLS ID - Must be numeric, and no more then 12 digits?T");
      string simpleField2 = this.loanData.GetSimpleField("3237");
      if (!(simpleField2 != string.Empty) || this.isNumeric(simpleField2) && simpleField2.Length <= 12)
        return;
      this.missingFields.Add("3237", "Company ID - Must be numeric, and no more then 12 digits?T");
    }

    private bool isNumeric(string val)
    {
      try
      {
        double.Parse(val);
        return true;
      }
      catch (Exception ex)
      {
        return false;
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

    private bool IsBlank(string id)
    {
      string simpleField = this.loanData.GetSimpleField(id);
      return simpleField == string.Empty || simpleField == null;
    }

    private bool IsZero(string id)
    {
      string simpleField = this.loanData.GetSimpleField(id);
      if (!(simpleField == string.Empty))
      {
        if (simpleField != null)
        {
          try
          {
            if (double.Parse(simpleField) == 0.0)
              return true;
          }
          catch
          {
          }
          return false;
        }
      }
      return true;
    }

    private struct loanInformation
    {
      public string LoanType;
      public string ARMType;
      public string Purpose;
      public bool WithCob;
    }
  }
}
