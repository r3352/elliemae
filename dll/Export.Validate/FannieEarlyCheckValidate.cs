// Decompiled with JetBrains decompiler
// Type: Encompass.Export.FannieEarlyCheckValidate
// Assembly: Export.Validate, Version=1.0.7933.30763, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 617E5049-06C8-448B-B2D8-44769B16A732
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EMN\Export.Validate.dll

using EllieMae.EMLite.Export;
using System;
using System.Windows.Forms;

#nullable disable
namespace Encompass.Export
{
  internal class FannieEarlyCheckValidate
  {
    private IBam loanData;
    private RequiredList missingFields = new RequiredList();
    private string borPairMsg = string.Empty;
    private FannieEarlyCheckValidate.loanInformation loanInfo;
    protected internal bool IsUlad;
    protected internal bool IsOnlyUlad;

    internal FannieEarlyCheckValidate(IBam loanData, bool isOnlyULAD = false)
    {
      this.loanData = loanData;
      this.IsUlad = this.loanData.GetSimpleField("1825") == "2020";
      this.IsOnlyUlad = isOnlyULAD;
    }

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
      this.Check08A();
      this.Check99B();
      this.CheckLNC();
      this.CheckOthers();
      this.Check07A();
      int numberOfBorrowerPairs = this.loanData.GetNumberOfBorrowerPairs();
      for (int index = 1; index <= numberOfBorrowerPairs; ++index)
      {
        this.loanData.SetBorrowerPair(index - 1);
        if (index > 1)
          this.borPairMsg = " (Co-Mortgagor " + (object) index + ")";
        this.loanInfo.WithCob = !this.IsBlank("68") || !this.IsBlank("69");
        this.Check03A();
        this.Check03C();
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
      if (this.IsBlank("305"))
        this.missingFields.Add("305", "Lender Case No?T");
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
        if (!(this.loanInfo.ARMType == "OtherAmortizationType") || this.IsUlad)
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
      if (this.IsUlad)
      {
        if (this.IsBlank("URLA.X73"))
          this.missingFields.Add("URLA.X73", "Property Street?T");
        if (!this.IsBlank("URLA.X74") && this.IsBlank("URLA.X75"))
          this.missingFields.Add("URLA.X75", "Unit #?T");
      }
      else if (this.IsBlank("11"))
        this.missingFields.Add("11", "Property Street?T");
      if (this.IsBlank("12"))
        this.missingFields.Add("12", "Property City?T");
      if (this.IsBlank("14"))
        this.missingFields.Add("14", "Property State (CA,TX, etc...)?T");
      if (this.IsBlank("15"))
        this.missingFields.Add("15", "Property Zipcode?N");
      if (this.IsBlank("16"))
        this.missingFields.Add("16", "Financed Number Of Units?N");
      if (!this.IsBlank("18") || this.IsOnlyUlad)
        return;
      this.missingFields.Add("18", "Year Built?N");
    }

    private void Check02B()
    {
      if (this.loanInfo.Purpose == string.Empty)
        this.missingFields.Add("19", "Purpose Of Loan?C:Purchase:Refinance:ConstructionOnly:ConstructionToPermanent:Other");
      if (!this.IsBlank("1811") && !(this.loanData.GetSimpleField("1811") == "N"))
        return;
      this.missingFields.Add("1811", "Property Usage Type?C:PrimaryResidence:SecondHome:Investor");
    }

    private void Check02D()
    {
      if (this.loanInfo.Purpose.IndexOf("Refinance") == -1 || !this.IsBlank("299"))
        return;
      this.missingFields.Add("299", "Refinance Purpose Type?T");
    }

    private void Check03A()
    {
      if (this.IsBlank("65"))
        this.missingFields.Add("65", "Primary SSN#" + this.borPairMsg + "?T");
      if (this.IsUlad)
      {
        if (this.IsBlank("4000"))
          this.missingFields.Add("4000", "Primary Applicant First Name" + this.borPairMsg + "?T");
        if (this.IsBlank("4002"))
          this.missingFields.Add("4002", "Primary Applicant Last Name" + this.borPairMsg + "?T");
        if (this.IsBlank("4006") && this.loanInfo.WithCob)
          this.missingFields.Add("4006", "Co-Applicant Last name" + this.borPairMsg + "?T");
      }
      else
      {
        if (this.IsBlank("36"))
          this.missingFields.Add("36", "Primary Applicant First Name" + this.borPairMsg + "?T");
        if (this.IsBlank("37"))
          this.missingFields.Add("37", "Primary Applicant Last Name" + this.borPairMsg + "?T");
        if (this.IsBlank("68") && this.loanInfo.WithCob)
          this.missingFields.Add("68", "Co-Applicant First name" + this.borPairMsg + "?T");
        if (this.IsBlank("69") && this.loanInfo.WithCob)
          this.missingFields.Add("69", "Co-Applicant Last name" + this.borPairMsg + "?T");
      }
      if (this.IsBlank("1402"))
        this.missingFields.Add("1402", "Primary Applicant Date of Birth" + this.borPairMsg + "?T");
      if (!this.loanInfo.WithCob)
        return;
      if (this.IsBlank("97"))
        this.missingFields.Add("97", "Co-Applicant SSN#" + this.borPairMsg + "?T");
      if (!this.IsBlank("1403"))
        return;
      this.missingFields.Add("1403", "Co-Applicant Date of Birth" + this.borPairMsg + "?T");
    }

    private void Check03C()
    {
      if (this.IsUlad)
      {
        if (this.IsBlank("FR0126"))
          this.missingFields.Add("FR0126", "Primary Applicant Street Address" + this.borPairMsg + "?T");
        if (!this.IsBlank("FR0125") && this.IsBlank("FR0127"))
          this.missingFields.Add("FR0127", "Primary Applicant Unit#" + this.borPairMsg + "?T");
        if (this.IsBlank("FR0226") && this.loanInfo.WithCob)
          this.missingFields.Add("FR0226", "Co-applicant Street Address" + this.borPairMsg + "?T");
        if (!this.IsBlank("FR0225") && this.IsBlank("FR0227") && this.loanInfo.WithCob)
          this.missingFields.Add("FR0227", "Co-applicant Unit#" + this.borPairMsg + "?T");
      }
      else if (this.IsBlank("FR0104"))
        this.missingFields.Add("FR0104", "Primary Applicant Street Address" + this.borPairMsg + "?T");
      if (this.IsBlank("FR0106"))
        this.missingFields.Add("FR0106", "Primary Applicant City Address" + this.borPairMsg + "?T");
      if (this.IsBlank("FR0107"))
        this.missingFields.Add("FR0107", "Primary Applicant State" + this.borPairMsg + "?T");
      if (this.IsBlank("FR0108"))
        this.missingFields.Add("FR0108", "Primary Applicant Zip code" + this.borPairMsg + "?T");
      if (!this.loanInfo.WithCob)
        return;
      if (this.IsBlank("FR0204") && !this.IsUlad)
        this.missingFields.Add("FR0204", "Co-applicant Street Address" + this.borPairMsg + "?T");
      if (this.IsBlank("FR0206"))
        this.missingFields.Add("FR0206", "Co-applicant City" + this.borPairMsg + "?T");
      if (this.IsBlank("FR0207"))
        this.missingFields.Add("FR0207", "Co-applicant State" + this.borPairMsg + "?T");
      if (!this.IsBlank("FR0208"))
        return;
      this.missingFields.Add("FR0208", "Co-applicant Zip code" + this.borPairMsg + "?T");
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
          if (this.IsZero("FL" + index.ToString("00") + "11"))
            this.missingFields.Add("FL" + index.ToString("00") + "11", "Liability Monthly Payment" + this.borPairMsg + "?N");
          if (this.IsZero("FL" + index.ToString("00") + "12"))
            this.missingFields.Add("FL" + index.ToString("00") + "12", "Liability Months Left" + this.borPairMsg + "?N");
        }
      }
    }

    private void Check06G()
    {
      int numberOfMortgages = this.loanData.GetNumberOfMortgages();
      for (int index = 1; index <= numberOfMortgages; ++index)
      {
        if (this.IsBlank("FM" + index.ToString("00") + "24"))
          this.missingFields.Add("FM" + index.ToString("00") + "24", "(VOM) Property Status" + this.borPairMsg + "?T");
        if (this.IsZero("FM" + index.ToString("00") + "21"))
          this.missingFields.Add("FM" + index.ToString("00") + "21", "(VOM) Property Taxes,Insurance & Expenses" + this.borPairMsg + "?N");
        if (this.loanInfo.Purpose.IndexOf("Refinance") > -1 && this.IsBlank("FM" + index.ToString("00") + "28"))
          this.missingFields.Add("FM" + index.ToString("00") + "28", "(VOM) Subject Indicator" + this.borPairMsg + "?T");
      }
    }

    private void Check07A()
    {
      if (!(this.loanInfo.Purpose == "Purchase") || !this.IsZero("136"))
        return;
      this.missingFields.Add("136", "Purchase Price Amount?N");
    }

    private void Check08A()
    {
      if (this.IsBlank("418"))
        this.missingFields.Add("418", "Declarations - Intend to occupy?T");
      if (this.IsBlank("403"))
        this.missingFields.Add("403", "Declarations - Had ownership in...?T");
      if (this.loanData.GetSimpleField("403").StartsWith("Y") && !this.IsUlad)
      {
        if (this.IsBlank("981"))
          this.missingFields.Add("981", "Declarations - What type of property did you own?T");
      }
      else if (this.loanData.GetSimpleField("403").StartsWith("Y") && this.loanData.GetSimpleField("418").StartsWith("Y") && this.IsUlad && this.IsBlank("981"))
        this.missingFields.Add("981", "Declarations - What type of property did you own?T");
      if (!(this.loanData.GetField("69") != string.Empty))
        return;
      if (this.IsBlank("1343"))
        this.missingFields.Add("1343", "Declarations - Intend to occupy?T");
      if (this.IsUlad)
      {
        if (this.loanData.GetField("1343").StartsWith("Y") && this.IsBlank("1108"))
          this.missingFields.Add("1108", "Declarations - Had ownership in...?T");
      }
      else if (this.IsBlank("1108"))
        this.missingFields.Add("1108", "Declarations - Had ownership in...?T");
      if (this.loanData.GetField("1108").StartsWith("Y") && !this.IsUlad)
      {
        if (!this.IsBlank("1015"))
          return;
        this.missingFields.Add("1015", "Declarations - What type of property did you own?T");
      }
      else
      {
        if (!this.loanData.GetField("1108").StartsWith("Y") || !this.IsUlad || !this.loanData.GetField("1343").StartsWith("Y") || !this.IsBlank("1015"))
          return;
        this.missingFields.Add("1015", "Declarations - What type of property did you own?T");
      }
    }

    private void Check99B()
    {
      if (!this.IsZero("356") || !this.IsZero("1821"))
        return;
      this.missingFields.Add("356", "Property Appraised Value Amount?N");
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
      if (this.loanData.GetSimpleField("MORNET.X72").StartsWith("Y") && !this.IsUlad && this.IsBlank("MORNET.X27"))
        this.missingFields.Add("MORNET.X27", "Community Lending Product?T");
      if (this.loanInfo.ARMType == "AdjustableRate" && !this.IsUlad)
      {
        if (this.IsBlank("689"))
          this.missingFields.Add("689", "ARM Index Margin Percent?N");
        if (this.IsBlank("995"))
          this.missingFields.Add("995", "ARM Product Plan Number?N");
      }
      if (this.IsBlank("364"))
        this.missingFields.Add("364", "Loan Number?T");
      if (this.IsBlank("424") && !this.IsUlad)
      {
        this.missingFields.Add("424", "Repayment Type Code?T");
      }
      else
      {
        if (!this.IsBlank("424") || !this.IsUlad || !this.loanData.GetSimpleField("URLA.X239").StartsWith("Y"))
          return;
        this.missingFields.Add("424", "Repayment Type Code?T");
      }
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
