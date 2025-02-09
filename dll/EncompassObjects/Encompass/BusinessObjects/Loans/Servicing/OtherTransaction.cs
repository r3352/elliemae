// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.OtherTransaction
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.InterimServicing;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  public class OtherTransaction : ServicingTransaction, IOtherTransaction
  {
    internal OtherTransaction(Loan loan, OtherTransactionLog trans)
      : base(loan, (ServicingTransactionBase) trans)
    {
    }

    public string InstitutionName
    {
      get => this.baseTrans.InstitutionName;
      set
      {
        this.baseTrans.InstitutionName = value;
        this.setLastModified();
      }
    }

    public string InstitutionRouting
    {
      get => this.baseTrans.InstitutionRouting;
      set
      {
        this.baseTrans.InstitutionRouting = value;
        this.setLastModified();
      }
    }

    public string AccountNumber
    {
      get => this.baseTrans.AccountNumber;
      set
      {
        this.baseTrans.AccountNumber = value;
        this.setLastModified();
      }
    }

    public string Reference
    {
      get => this.baseTrans.Reference;
      set
      {
        this.baseTrans.Reference = value;
        this.setLastModified();
      }
    }

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
