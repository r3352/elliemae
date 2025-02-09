// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ILoanContacts
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Contacts;
using EllieMae.Encompass.Collections;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [Guid("3B7DB1DD-8CE2-4a93-A0C7-5FC26D8C0C67")]
  public interface ILoanContacts
  {
    LoanContactRelationship GetBizContactRelationship(LoanContactRelationshipType relation);

    LoanContactRelationshipList GetBizContactRelationships();

    LoanContactRelationship GetBorrowerRelationship(Borrower borrower);

    LoanContactRelationshipList GetBorrowerRelationships();

    LoanContactRelationshipList GetRelationshipsForContact(Contact contact);

    void LinkToBizContact(BizContact contact, LoanContactRelationshipType relationType);

    void UnlinkBizContact(LoanContactRelationshipType relationType);

    void UnlinkAllBizContacts();

    void LinkToBorrowerContact(BorrowerContact contact, Borrower loanBorrower);

    void UnlinkBorrower(Borrower loanBorrower);

    void UnlinkAllBorrowerContacts();

    void Unlink(LoanContactRelationship relation);

    IEnumerator GetEnumerator();
  }
}
