// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.BorrowerPair
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class BorrowerPair : IBorrowerPair
  {
    private BorrowerPair pair;
    private Borrower borrower;
    private Borrower coborrower;
    private Loan loan;

    internal BorrowerPair(Loan loan, BorrowerPair pair)
    {
      this.loan = loan;
      this.pair = pair;
      if (pair.Borrower != null)
        this.borrower = new Borrower(loan, pair.Borrower);
      if (pair.CoBorrower == null)
        return;
      this.coborrower = new Borrower(loan, pair.CoBorrower);
    }

    public Borrower Borrower => this.borrower;

    public Borrower CoBorrower => this.coborrower;

    public override string ToString()
    {
      return this.borrower.ToString() + "/" + this.coborrower.ToString();
    }

    public override bool Equals(object obj)
    {
      BorrowerPair objA = obj as BorrowerPair;
      return !object.Equals((object) objA, (object) null) && this.loan.Equals((object) objA.loan) && this.Borrower.ID == objA.Borrower.ID;
    }

    public override int GetHashCode()
    {
      return this.coborrower != (Borrower) null ? this.borrower.GetHashCode() ^ this.coborrower.GetHashCode() : this.borrower.GetHashCode();
    }

    public static bool operator ==(BorrowerPair pairA, BorrowerPair pairB)
    {
      return object.Equals((object) pairA, (object) pairB);
    }

    public static bool operator !=(BorrowerPair pairA, BorrowerPair pairB) => !(pairA == pairB);

    internal BorrowerPair Unwrap() => this.pair;

    internal static BorrowerPairList ToList(Loan loan, BorrowerPair[] pairs)
    {
      BorrowerPairList list = new BorrowerPairList();
      for (int index = 0; index < pairs.Length; ++index)
        list.Add(new BorrowerPair(loan, pairs[index]));
      return list;
    }
  }
}
