// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.FundingFee
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class FundingFee : IFundingFee
  {
    private FundingFee fee;

    internal FundingFee(FundingFee fee) => this.fee = fee;

    public string LineID => this.fee.LineID;

    public string CDLineID => this.fee.CDLineID;

    public bool BalanceChecked => this.fee.BalanceChecked;

    public string FeeDescription => this.fee.FeeDescription;

    public string FeeDescription2015 => this.fee.FeeDescription2015;

    public string Payee => this.fee.Payee;

    public string PaidBy => this.fee.PaidBy;

    public string PaidTo => this.fee.PaidTo;

    public double Amount => this.fee.Amount;

    public double POCAmount => this.fee.POCAmount;

    public string POCPaidBy => this.fee.POCPaidBy;

    public double PTCAmount => this.fee.PTCAmount;

    public string PTCPaidBy => this.fee.PTCPaidBy;

    public double POCBorrower2015 => this.fee.POCBorrower2015;

    public double POCSeller2015 => this.fee.POCSeller2015;

    public double POCBroker2015 => this.fee.POCBroker2015;

    public double POCLender2015 => this.fee.POCLender2015;

    public double POCOther2015 => this.fee.POCOther2015;

    public double PACBroker2015 => this.fee.PACBroker2015;

    public double PACLender2015 => this.fee.PACLender2015;

    public double PACOther2015 => this.fee.PACOther2015;
  }
}
