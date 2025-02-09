// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.EscrowInterest
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.InterimServicing;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  /// <summary>
  /// Represents an escrow disbursement servicing transaction.
  /// </summary>
  public class EscrowInterest : ServicingTransaction, IEscrowInterest
  {
    internal EscrowInterest(Loan loan, EscrowInterestLog trans)
      : base(loan, (ServicingTransactionBase) trans)
    {
    }

    /// <summary>Gets or sets additional comments for the transaction.</summary>
    public string Comments
    {
      get => this.baseTrans.Comments;
      set
      {
        this.baseTrans.Comments = value;
        this.setLastModified();
      }
    }

    private EscrowInterestLog baseTrans => (EscrowInterestLog) this.Unwrap();
  }
}
