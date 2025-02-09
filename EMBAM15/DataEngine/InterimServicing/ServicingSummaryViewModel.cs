// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.InterimServicing.ServicingSummaryViewModel
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

#nullable disable
namespace EllieMae.EMLite.DataEngine.InterimServicing
{
  public class ServicingSummaryViewModel(ServicingTransactionBase[] transactions) : 
    ServicingViewModelBase(transactions)
  {
    private EscrowSummaryViewModel escrow;
    private double principal;
    private double interest;
    private double buydownSubsidyAmount;
    private double lateFee;
    private double miscFee;
    private double additionalPrincipal;
    private double additionalEscrow;

    public EscrowSummaryViewModel Escrow
    {
      get
      {
        if (this.escrow == null)
          this.escrow = new EscrowSummaryViewModel(this.Transactions);
        if (this.escrow.SelectedYear != this.SelectedYear)
          this.escrow.GenerateAnnualSummary(this.SelectedYear);
        return this.escrow;
      }
    }

    public double Principal => this.principal;

    public double Interest => this.interest;

    public double BuydownSubsidyAmount => this.buydownSubsidyAmount;

    public double LateFee => this.lateFee;

    public double MiscFee => this.miscFee;

    public double AdditionalPrincipal => this.additionalPrincipal;

    public double AdditionalEscrow => this.additionalEscrow;

    public double PnI => this.principal + this.additionalPrincipal + this.interest;

    public double TotalFeeCollected
    {
      get
      {
        return this.principal + this.interest + this.Escrow.TotalEscrowPayment + this.buydownSubsidyAmount + this.lateFee + this.miscFee + this.additionalPrincipal + this.additionalEscrow;
      }
    }

    protected override void tallyAnnualSummarySubItems(PaymentTransactionLog payTransLog)
    {
      if (payTransLog == null)
        return;
      this.principal += payTransLog.Principal;
      this.interest += payTransLog.Interest;
      this.buydownSubsidyAmount += payTransLog.BuydownSubsidyAmount;
      this.lateFee += payTransLog.LateFee;
      this.miscFee += payTransLog.MiscFee;
      this.additionalPrincipal += payTransLog.AdditionalPrincipal;
      this.additionalEscrow += payTransLog.AdditionalEscrow;
    }

    protected override void resetSubItems()
    {
      this.escrow = (EscrowSummaryViewModel) null;
      this.principal = 0.0;
      this.interest = 0.0;
      this.buydownSubsidyAmount = 0.0;
      this.lateFee = 0.0;
      this.miscFee = 0.0;
      this.additionalPrincipal = 0.0;
      this.additionalEscrow = 0.0;
    }

    public static ServicingSummaryViewModel GenerateAnnualSummary(
      ServicingTransactionBase[] transactions,
      int year)
    {
      ServicingSummaryViewModel annualSummary = new ServicingSummaryViewModel(transactions);
      annualSummary.GenerateAnnualSummary(year);
      return annualSummary;
    }
  }
}
