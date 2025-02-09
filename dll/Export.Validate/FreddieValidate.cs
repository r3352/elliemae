// Decompiled with JetBrains decompiler
// Type: Encompass.Export.FreddieValidate
// Assembly: Export.Validate, Version=1.0.7933.30763, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 617E5049-06C8-448B-B2D8-44769B16A732
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EMN\Export.Validate.dll

using EllieMae.EMLite.Export;
using System.Windows.Forms;

#nullable disable
namespace Encompass.Export
{
  internal class FreddieValidate
  {
    private IBam loanData;
    private RequiredList missingFields = new RequiredList();

    internal FreddieValidate(IBam loanData) => this.loanData = loanData;

    internal bool ValidateData(bool allowContinue)
    {
      this.CheckLP();
      return !(this.missingFields.List != string.Empty) || new ValidationDialog(this.missingFields.List, allowContinue, this.loanData).ShowDialog() == DialogResult.OK;
    }

    internal string ValidateDataList()
    {
      this.CheckLP();
      return this.missingFields.List;
    }

    private void CheckLP()
    {
      this.checkBlankField("1172", "Mortgage Type?C:Conventional:FHA:VA:Other:FarmersHomeAdministration");
      this.checkBlankField("997", "Seller Number?T");
      this.checkBlankField("CASASRN.X107", "Processing Point?C:Prequal:Application/Processing:Underwriting:Final Disposition:Post-Closing QC");
      this.checkBlankField("420", "Lien Type?C:First:Second");
      this.checkBlankField("16", "Financed Number Of Units?N");
      this.checkBlankField("1811", "Property Usage Type?C:PrimaryResidence:SecondHome:Investor");
      this.checkBlankField("4", "Loan Term (1~360)?N");
      this.checkBlankField("325", "Balloon Loan Term (1~360)?N");
      this.checkBlankField("608", "Amortization Type?C:AdjustableRate:Fixed:GraduatedPaymentMortgage:GrowingEquityMortgage:OtherAmortizationType");
      this.checkBlankField("CASASRN.X99", "All Monthly Payments?N");
      this.checkBlankField("1389", "Total Gross Monthly Income?N");
      this.checkBlankField("2", "Loan Amount?N");
      this.checkBlankField("CASASRN.X29", "Purpose Of Loan?C:Purchase:Regular refinance:Streamline refinance:Freddie-owned refinance:FHA Credit Qualifying refinance");
      this.checkBlankField("CASASRN.X14", "Property Type?C:Low-rise Condo (up to 4):High rise Condo (5 or more):Single Family Detached:Single Family Attached:Single Family Attached PUD:Single Family Detached PUD:2 to 4 Family:Multifamily:Co-op Apartment");
      this.checkBlankField("912", "Proposed Housing Expense Total?N");
      this.checkBlankField("CASASRN.X16", "Present Housing Expense?N");
      this.checkBlankField("425", "Buydown Indicator?T");
      this.checkBlankField("CASASRN.X77", "Retail Loan?T");
      this.checkBlankField("CASASRN.X78", "Reserves?N");
      this.checkBlankField("CASASRN.X30", "Second Trust Refi?T");
      this.checkBlankField("CASASRN.X114", "Affordable Product?C:Not Affordable product:Affordable Gold 5:Affordable Gold 3/2:Affordable Gold 97:Specially-negotiated Affordable 5:Specially-negotiated Affordable 3/2:Specially-negotiated Affordable 97");
      this.checkBlankField("CASASRN.X142", "Original Interest Rate?N");
      this.checkBlankField("36", "Primary Applicant First Name?T");
      this.checkBlankField("37", "Primary Applicant Last Name?T");
      this.checkBlankField("65", "Primary SSN#?T");
      this.checkBlankField("FR0104", "Primary Applicant Street Address?T");
      this.checkBlankField("FR0106", "Primary Applicant City Address?T");
      this.checkBlankField("FR0107", "Primary Applicant State?T");
      this.checkBlankField("FR0108", "Primary Applicant Zip code?T");
      this.checkBlankField("FR0115", "Borrower Residency Basis Type?T");
      this.checkBlankField("FE0115", "Employment Borrower Self Employed Indicator?T");
      this.checkBlankField("265", "Bankruptcy Indicator?T");
      this.checkBlankField("170", "Property Foreclosed Past Seven Years Indicator?T");
      this.checkBlankField("418", "Intent To Occupy Type?T");
      this.checkBlankField("470", "Race National Origin Type?T");
      this.checkBlankField("403", "Homeowner Past Three Years Type?T");
      this.checkBlankField("965", "Citizen?T");
      this.checkBlankField("471", "Gender Type?T");
      this.checkBlankField("52", "Marital Status Type?C:Unmarried:Married:Separated");
      this.checkBlankField("910", "Monthly Income Not Including Net Rental Income?N");
      this.checkBlankField("364", "Loan Number?T");
      if (this.loanData.GetSimpleField("19") == "Cash-Out Refinance")
        this.checkBlankField("CASASRN.X79", "Cash Out Amount?N");
      if (this.loanData.GetSimpleField("CASASRN.X29") == "P")
        this.checkBlankField("CASASRN.X109", "Net Purchase Price?N");
      if (this.loanData.GetSimpleField("CASASRN.X29") != "P")
      {
        this.checkBlankField("24", "Property Acquired Year?N");
        this.checkBlankField("25", "Property Original Cost Amount?N");
      }
      if (!(this.loanData.GetSimpleField("68") != string.Empty))
        return;
      this.checkBlankField("97", "Co-Applicant SSN#?T");
      this.checkBlankField("70", "Age At Application Years?N");
    }

    private void checkBlankField(string id, string desc)
    {
      string simpleField = this.loanData.GetSimpleField(id);
      if (id == "1811")
        id = "1811";
      if (simpleField != null && !(simpleField == string.Empty))
        return;
      this.missingFields.Add(id, desc);
    }
  }
}
