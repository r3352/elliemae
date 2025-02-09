// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanContactRelationship
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Contacts;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
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

    public string LoanGuid => this.loanGuid;

    public int ContactID => this.contactId;

    public ContactType ContactType => this.contactType;

    public LoanContactRelationshipType RelationshipType => this.relation;

    public int BorrowerPairIndex => this.borrowerPairIndex;

    public Loan OpenLoan() => this.Session.Loans.Open(this.LoanGuid);

    public Contact OpenContact() => this.Session.Contacts.Open(this.ContactID, this.ContactType);
  }
}
