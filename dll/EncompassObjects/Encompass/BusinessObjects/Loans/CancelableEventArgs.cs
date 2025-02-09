// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.CancelableEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class CancelableEventArgs : EventArgs
  {
    private Loan loan;
    private bool cancel;

    internal CancelableEventArgs(Loan loan, bool cancel)
    {
      this.loan = loan;
      this.cancel = cancel;
    }

    public Loan Loan => this.loan;

    public bool Cancel
    {
      get => this.cancel;
      set => this.cancel = value;
    }
  }
}
