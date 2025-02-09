// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.OtherTransaction
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
  public class OtherTransaction : ServicingTransaction, IOtherTransaction
  {
    internal OtherTransaction(Loan loan, OtherTransactionLog trans)
      : base(loan, (ServicingTransactionBase) trans)
    {
    }

    /// <summary>
    /// Gets or sets the name of the institution for the transaction.
    /// </summary>
    public string InstitutionName
    {
      get => this.baseTrans.InstitutionName;
      set
      {
        this.baseTrans.InstitutionName = value;
        this.setLastModified();
      }
    }

    /// <summary>
    /// Gets or sets the routing information for the transaction.
    /// </summary>
    public string InstitutionRouting
    {
      get => this.baseTrans.InstitutionRouting;
      set
      {
        this.baseTrans.InstitutionRouting = value;
        this.setLastModified();
      }
    }

    /// <summary>
    /// Gets or sets the account number used for the transaction.
    /// </summary>
    public string AccountNumber
    {
      get => this.baseTrans.AccountNumber;
      set
      {
        this.baseTrans.AccountNumber = value;
        this.setLastModified();
      }
    }

    /// <summary>
    /// Gets or sets the reference number for the transaction.
    /// </summary>
    public string Reference
    {
      get => this.baseTrans.Reference;
      set
      {
        this.baseTrans.Reference = value;
        this.setLastModified();
      }
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

    private OtherTransactionLog baseTrans => (OtherTransactionLog) this.Unwrap();
  }
}
