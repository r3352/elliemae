// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.InterimServicing.EscrowSummaryViewModel
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

#nullable disable
namespace EllieMae.EMLite.DataEngine.InterimServicing
{
  public class EscrowSummaryViewModel(ServicingTransactionBase[] transactions) : 
    ServicingViewModelBase(transactions)
  {
    private double taxes;
    private double hazardInsurance;
    private double mortgageInsurance;
    private double floodInsurance;
    private double cityPropertyTax;
    private double other1;
    private double other2;
    private double other3;
    private double usdaMonthlyPremuim;
    private double totalEscrowPayment;

    public double Taxes => this.taxes;

    public double HazardInsurance => this.hazardInsurance;

    public double MortgageInsurance => this.mortgageInsurance;

    public double FloodInsurance => this.floodInsurance;

    public double CityPropertyTax => this.cityPropertyTax;

    public double Other1 => this.other1;

    public double Other2 => this.other2;

    public double Other3 => this.other3;

    public double UsdaMonthlyPremuim => this.usdaMonthlyPremuim;

    public double TotalEscrowPayment => this.totalEscrowPayment;

    public double TotalFeeCollected
    {
      get
      {
        return this.taxes + this.hazardInsurance + this.mortgageInsurance + this.floodInsurance + this.cityPropertyTax + this.other1 + this.other2 + this.other3 + this.usdaMonthlyPremuim;
      }
    }

    protected override void tallyAnnualSummarySubItems(PaymentTransactionLog payTransLog)
    {
      if (payTransLog == null)
        return;
      this.taxes += payTransLog.EscowTaxes;
      this.hazardInsurance += payTransLog.HazardInsurance;
      this.mortgageInsurance += payTransLog.MortgageInsurance;
      this.floodInsurance += payTransLog.FloodInsurance;
      this.cityPropertyTax += payTransLog.CityPropertyTax;
      this.other1 += payTransLog.Other1Escrow;
      this.other2 += payTransLog.Other2Escrow;
      this.other3 += payTransLog.Other3Escrow;
      this.usdaMonthlyPremuim += payTransLog.USDAMonthlyPremium;
      this.totalEscrowPayment += payTransLog.Escrow;
    }

    protected override void resetSubItems()
    {
      this.taxes = 0.0;
      this.hazardInsurance = 0.0;
      this.mortgageInsurance = 0.0;
      this.floodInsurance = 0.0;
      this.cityPropertyTax = 0.0;
      this.other1 = 0.0;
      this.other2 = 0.0;
      this.other3 = 0.0;
      this.usdaMonthlyPremuim = 0.0;
      this.totalEscrowPayment = 0.0;
    }

    public static EscrowSummaryViewModel GenerateAnnualSummary(
      ServicingTransactionBase[] transactions,
      int year)
    {
      EscrowSummaryViewModel annualSummary = new EscrowSummaryViewModel(transactions);
      annualSummary.GenerateAnnualSummary(year);
      return annualSummary;
    }
  }
}
