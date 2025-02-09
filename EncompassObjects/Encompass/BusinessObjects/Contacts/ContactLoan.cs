// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactLoan
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.Encompass.Client;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>
  /// Represents the historical data for a closed loan that may be associated with one
  /// or more contacts.
  /// </summary>
  public class ContactLoan : SessionBoundObject, IContactLoan
  {
    private IContactManager mngr;
    private ContactLoanInfo info;

    internal ContactLoan(Session session, ContactLoanInfo info)
      : base(session)
    {
      this.mngr = session.Contacts.ContactManager;
      this.info = info;
    }

    /// <summary>
    /// Gets the unique identifier for this ContactLoan object.
    /// </summary>
    public int ID => this.info.LoanID;

    /// <summary>
    /// Returns the ID of the primary Borrower contact with which the loan is associated.
    /// </summary>
    public int BorrowerID => this.info.BorrowerID;

    /// <summary>Gets the status of the loan.</summary>
    public string LoanStatus => this.info.LoanStatus;

    /// <summary>Gets the appraised value of the property.</summary>
    public Decimal AppraisedValue => this.info.AppraisedValue;

    /// <summary>Gets the amount of the loan.</summary>
    public Decimal LoanAmount => this.info.LoanAmount;

    /// <summary>
    /// Gets the interest rate of the loan as a decimal between 0 and 1.
    /// </summary>
    public Decimal InterestRate => this.info.InterestRate;

    /// <summary>Gets the term of the loan, in months.</summary>
    public int Term => this.info.Term;

    /// <summary>Gets the purpose of the loan.</summary>
    public string Purpose => this.info.Purpose.ToString();

    /// <summary>Gets the amount of the down payment made on the loan.</summary>
    public Decimal DownPayment => this.info.DownPayment;

    /// <summary>
    /// Gets the Loan-to-Value ratio for the loan as a decimal between 0 and 1.
    /// </summary>
    public Decimal LTV => this.info.LTV;

    /// <summary>Returns the amortization method applied to the loan.</summary>
    public string Amortization => this.info.Amortization.ToString();

    /// <summary>Gets the date on which the loan was closed.</summary>
    public DateTime DateCompleted => this.info.DateCompleted;

    /// <summary>Gets the type of loan.</summary>
    public string LoanType => this.info.LoanType.ToString();

    /// <summary>Gets the lien position of this loan.</summary>
    public string LienPosition => this.info.LienPosition.ToString();
  }
}
