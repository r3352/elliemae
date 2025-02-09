// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.CancelableMilestoneEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Loans.Logging;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Event arguments for the MilestoneEvent-related loan events.
  /// </summary>
  public class CancelableMilestoneEventArgs : MilestoneEventArgs
  {
    private bool cancel;

    internal CancelableMilestoneEventArgs(Loan loan, MilestoneEvent msEvent)
      : base(loan, msEvent)
    {
    }

    /// <summary>Gets or Sets the boolean value for Cancel</summary>
    public bool Cancel
    {
      get => this.cancel;
      set => this.cancel = value;
    }
  }
}
