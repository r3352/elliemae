// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.PaymentSchedule
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class PaymentSchedule
  {
    public PaymentSchedule()
    {
      this.CurrentRate = 0.0;
      this.Principal = 0.0;
      this.Interest = 0.0;
      this.MortgageInsurance = 0.0;
      this.Payment = 0.0;
      this.Balance = 0.0;
      this.BuydownSubsidyAmount = 0.0;
      this.OriginalNoteRate = 0.0;
      this.BalanceForMI = 0.0;
      this.USDAAnnualUPB = 0.0;
      this.USDAAnnualFee = 0.0;
      this.USDAMonthlyFee = 0.0;
      this.USDAMonthlyPayment = 0.0;
      this.RemainingLTV = 0.0;
    }

    public string PayDate { set; get; }

    public double CurrentRate { set; get; }

    public double Principal { set; get; }

    public double Interest { set; get; }

    public double MortgageInsurance { set; get; }

    public double Payment { set; get; }

    public double Balance { set; get; }

    public double BuydownSubsidyAmount { set; get; }

    public double OriginalNoteRate { set; get; }

    public double BalanceForMI { set; get; }

    public double USDAAnnualUPB { set; get; }

    public double USDAAnnualFee { set; get; }

    public double USDAMonthlyFee { set; get; }

    public double USDAMonthlyPayment { set; get; }

    public double RemainingLTV { set; get; }
  }
}
