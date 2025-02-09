// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Borrower
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class Borrower : IBorrower
  {
    private Borrower borrower;
    private Loan loan;

    internal Borrower(Loan loan, Borrower borrower)
    {
      this.loan = loan;
      this.borrower = borrower;
    }

    public string FirstName => this.borrower.FirstName;

    public string LastName => this.borrower.LastName;

    public string ID => this.borrower.Id;

    public override string ToString() => this.FirstName + " " + this.LastName;

    public override bool Equals(object obj)
    {
      Borrower objA = obj as Borrower;
      return !object.Equals((object) objA, (object) null) && this.loan.Equals((object) objA.loan) && this.ID == objA.ID;
    }

    public override int GetHashCode() => this.borrower.Id.GetHashCode();

    public static bool operator ==(Borrower borA, Borrower borB)
    {
      return object.Equals((object) borA, (object) borB);
    }

    public static bool operator !=(Borrower borA, Borrower borB) => !(borA == borB);

    internal Borrower Unwrap() => this.borrower;
  }
}
