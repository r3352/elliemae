// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanBorrowerPairs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class LoanBorrowerPairs : ILoanBorrowerPairs, IEnumerable
  {
    private Loan loan;
    private BorrowerPairList borrowerPairs;
    private BorrowerPair currentPair;

    internal LoanBorrowerPairs(Loan loan)
    {
      this.loan = loan;
      this.RefreshPairs();
    }

    public BorrowerPair this[int index] => this.borrowerPairs[index];

    public int Count => this.borrowerPairs.Count;

    public BorrowerPair Add()
    {
      this.loan.EnsureExclusive();
      if (this.Count >= 6)
        throw new InvalidOperationException("Cannot add more than 6 Borrower Pairs");
      BorrowerPair borrowerPair = this.loan.LoanData.CreateBorrowerPair();
      this.RefreshPairs();
      return new BorrowerPair(this.loan, borrowerPair);
    }

    public void Remove(BorrowerPair pair)
    {
      if (this.Count <= 1)
        throw new InvalidOperationException("Cannot delete all Borrower Pairs from loan");
      this.loan.EnsureExclusive();
      if (pair == this.Current)
      {
        for (int index = 0; index < this.Count; ++index)
        {
          if (this[index] != pair)
          {
            this.Current = this[index];
            break;
          }
        }
      }
      this.loan.LoanData.RemoveBorrowerPair(pair.Unwrap());
      this.RefreshPairs();
    }

    public void Swap(BorrowerPair pairA, BorrowerPair pairB)
    {
      this.loan.EnsureExclusive();
      this.loan.LoanData.SwapBorrowerPairs(new BorrowerPair[2]
      {
        pairA.Unwrap(),
        pairB.Unwrap()
      });
      this.RefreshPairs();
    }

    public BorrowerPair Current
    {
      get
      {
        if (this.loan.Unwrap().LoanData.CurrentBorrowerPair.Id != this.currentPair.Unwrap().Id)
          this.RefreshPairs();
        return this.currentPair;
      }
      set
      {
        this.loan.LoanData.SetBorrowerPair(value.Unwrap());
        this.currentPair = value;
      }
    }

    public IEnumerator GetEnumerator() => this.borrowerPairs.GetEnumerator();

    public void Refresh() => this.RefreshPairs();

    internal BorrowerPair GetPairByID(string pairId)
    {
      foreach (BorrowerPair pairById in this)
      {
        if (pairById.Borrower.ID == pairId)
          return pairById;
      }
      return (BorrowerPair) null;
    }

    internal BorrowerPair GetPair(BorrowerPair nativePair)
    {
      BorrowerPair pair = this.findPair(nativePair);
      if (pair != (BorrowerPair) null)
        return pair;
      this.RefreshPairs();
      return this.findPair(nativePair);
    }

    internal void RefreshPairs()
    {
      this.borrowerPairs = BorrowerPair.ToList(this.loan, this.loan.LoanData.GetBorrowerPairs());
      BorrowerPair currentBorrowerPair = this.loan.LoanData.CurrentBorrowerPair;
      if (currentBorrowerPair == null)
        this.currentPair = (BorrowerPair) null;
      else
        this.currentPair = new BorrowerPair(this.loan, currentBorrowerPair);
    }

    private BorrowerPair findPair(BorrowerPair nativePair)
    {
      foreach (BorrowerPair borrowerPair in (CollectionBase) this.borrowerPairs)
      {
        if (borrowerPair.Unwrap().Equals((object) nativePair))
          return borrowerPair;
      }
      return (BorrowerPair) null;
    }
  }
}
