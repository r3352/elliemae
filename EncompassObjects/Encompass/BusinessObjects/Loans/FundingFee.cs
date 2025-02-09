// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.FundingFee
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Funding Fee that we are loading from Funding Worksheet.
  /// </summary>
  public class FundingFee : IFundingFee
  {
    private EllieMae.EMLite.DataEngine.FundingFee fee;

    /// <summary>Funding Fee constructor</summary>
    internal FundingFee(EllieMae.EMLite.DataEngine.FundingFee fee) => this.fee = fee;

    /// <summary>Gets line number of 2010/2015 Itemization.</summary>
    public string LineID => this.fee.LineID;

    /// <summary>
    /// Gets line number of Closing Disclosure Pgae 2. This is available only when loan is 2015 RESPA-TILA.
    /// </summary>
    public string CDLineID => this.fee.CDLineID;

    /// <summary>Gets funding fee checked by user</summary>
    public bool BalanceChecked => this.fee.BalanceChecked;

    /// <summary>
    /// Gets description of fee. This is available only when loan is 2010 RESPA.
    /// </summary>
    public string FeeDescription => this.fee.FeeDescription;

    /// <summary>
    /// Gets description of fee. This is available only when loan is 2015 RESPA-TILA.
    /// </summary>
    public string FeeDescription2015 => this.fee.FeeDescription2015;

    /// <summary>Gets Recipient of a miscellaneous charge.</summary>
    public string Payee => this.fee.Payee;

    /// <summary>Gets Paid By ID (Broker, Lender, Other)</summary>
    public string PaidBy => this.fee.PaidBy;

    /// <summary>
    /// Gets Paid To ID (Broker, Lender, Seller, Invistor, Affiliate, Other)
    /// </summary>
    public string PaidTo => this.fee.PaidTo;

    /// <summary>Gets Fee Amount</summary>
    public double Amount => this.fee.Amount;

    /// <summary>
    /// Gets the portion of the fee paid outside closing by borrower. This is available only when loan is 2010 RESPA.
    /// </summary>
    public double POCAmount => this.fee.POCAmount;

    /// <summary>
    /// Gets the portion of the fee paid outside closing by 3rd party. This is available only when loan is 2010 RESPA.
    /// </summary>
    public string POCPaidBy => this.fee.POCPaidBy;

    /// <summary>
    /// Gets the portion of the fee paid through closing amount. This is available only when loan is 2010 RESPA.
    /// </summary>
    public double PTCAmount => this.fee.PTCAmount;

    /// <summary>
    /// Gets the portion of the fee paid through closing by 3rd party. This is available only when loan is 2010 RESPA.
    /// </summary>
    public string PTCPaidBy => this.fee.PTCPaidBy;

    /// <summary>
    /// Gets the portion of the fee paid outside closing by the borrower. This is available only when loan is 2015 RESPA-TILA.
    /// </summary>
    public double POCBorrower2015 => this.fee.POCBorrower2015;

    /// <summary>
    /// Gets the portion of the fee paid outside closing by the seller. This is available only when loan is 2015 RESPA-TILA.
    /// </summary>
    public double POCSeller2015 => this.fee.POCSeller2015;

    /// <summary>
    /// Gets the portion of the fee paid outside closing by the broker. This is available only when loan is 2015 RESPA-TILA.
    /// </summary>
    public double POCBroker2015 => this.fee.POCBroker2015;

    /// <summary>
    /// Gets the portion of the fee paid outside closing by the lender. This is available only when loan is 2015 RESPA-TILA.
    /// </summary>
    public double POCLender2015 => this.fee.POCLender2015;

    /// <summary>
    /// Gets the portion of the fee paid outside closing by the other. This is available only when loan is 2015 RESPA-TILA.
    /// </summary>
    public double POCOther2015 => this.fee.POCOther2015;

    /// <summary>
    /// Gets the portion of the fee paid at closing by the broker. This is available only when loan is 2015 RESPA-TILA.
    /// </summary>
    public double PACBroker2015 => this.fee.PACBroker2015;

    /// <summary>
    /// Gets the portion of the fee paid at closing by the lender. This is available only when loan is 2015 RESPA-TILA.
    /// </summary>
    public double PACLender2015 => this.fee.PACLender2015;

    /// <summary>
    /// Gets the portion of the fee paid at closing by the other. This is available only when loan is 2015 RESPA-TILA.
    /// </summary>
    public double PACOther2015 => this.fee.PACOther2015;
  }
}
