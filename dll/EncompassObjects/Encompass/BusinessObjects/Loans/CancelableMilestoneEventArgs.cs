// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.CancelableMilestoneEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans.Logging;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class CancelableMilestoneEventArgs : MilestoneEventArgs
  {
    private bool cancel;

    internal CancelableMilestoneEventArgs(Loan loan, MilestoneEvent msEvent)
      : base(loan, msEvent)
    {
    }

    public bool Cancel
    {
      get => this.cancel;
      set => this.cancel = value;
    }
  }
}
