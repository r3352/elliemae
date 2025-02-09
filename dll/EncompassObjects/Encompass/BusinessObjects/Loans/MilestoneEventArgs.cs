// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.MilestoneEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class MilestoneEventArgs : EventArgs
  {
    private Loan loan;
    private MilestoneEvent msEvent;

    internal MilestoneEventArgs(Loan loan, MilestoneEvent msEvent)
    {
      this.loan = loan;
      this.msEvent = msEvent;
    }

    public Loan Loan => this.loan;

    public MilestoneEvent MilestoneEvent => this.msEvent;
  }
}
