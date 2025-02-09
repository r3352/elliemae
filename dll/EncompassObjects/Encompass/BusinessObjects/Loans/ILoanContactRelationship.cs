// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ILoanContactRelationship
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Contacts;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [Guid("DD392A8E-0DFF-4420-A6AB-3278BDBEB4B9")]
  public interface ILoanContactRelationship
  {
    string LoanGuid { get; }

    int ContactID { get; }

    ContactType ContactType { get; }

    LoanContactRelationshipType RelationshipType { get; }

    int BorrowerPairIndex { get; }

    Loan OpenLoan();

    Contact OpenContact();
  }
}
