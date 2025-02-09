// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanContactRelationship
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Contacts;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Represents the relationship between a Contact and a Loan.
  /// </summary>
  public class LoanContactRelationship : SessionBoundObject, ILoanContactRelationship
  {
    private string loanGuid;
    private int contactId;
    private ContactType contactType;
    private LoanContactRelationshipType relation;
    private int borrowerPairIndex;

    internal LoanContactRelationship(
      Session session,
      string loanGuid,
      int contactId,
      ContactType contactType,
      LoanContactRelationshipType relationship,
      int pairIndex)
      : base(session)
    {
      this.loanGuid = loanGuid;
      this.contactId = contactId;
      this.contactType = contactType;
      this.relation = relationship;
      this.borrowerPairIndex = pairIndex;
    }

    /// <summary>
    /// Gets the Guid of the loan to which the contact is related.
    /// </summary>
    public string LoanGuid => this.loanGuid;

    /// <summary>
    /// Gets the ID of the contact to which the loan is related.
    /// </summary>
    public int ContactID => this.contactId;

    /// <summary>
    /// Indicates if the specified contact is a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Borrower" /> or <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BizContact" />.
    /// </summary>
    public ContactType ContactType => this.contactType;

    /// <summary>
    /// Indicates the type of relationship between the contact and the loan.
    /// </summary>
    public LoanContactRelationshipType RelationshipType => this.relation;

    /// <summary>
    /// Returns the Borrower Pair Index for a borrower-related contact.
    /// </summary>
    /// <remarks>This property will return -1 when the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.LoanContactRelationship.ContactType" /> property indicates
    /// this is a BizContact relationship.</remarks>
    public int BorrowerPairIndex => this.borrowerPairIndex;

    /// <summary>
    /// Opens the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan" /> represented in this relationship.
    /// </summary>
    /// <returns>Returns the specified <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan" /> object. The loan is not locked so you
    /// must call <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.Lock" /> prior to editing or saving the returned loan.</returns>
    public Loan OpenLoan() => this.Session.Loans.Open(this.LoanGuid);

    /// <summary>
    /// Opens the <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.Contact" /> represented in this relationship.
    /// </summary>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.Contact" /> object for the specified contact.</returns>
    public Contact OpenContact() => this.Session.Contacts.Open(this.ContactID, this.ContactType);
  }
}
