// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.MilestoneEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Event arguments for the MilestoneEvent-related loan events.
  /// </summary>
  public class MilestoneEventArgs : EventArgs
  {
    private Loan loan;
    private MilestoneEvent msEvent;

    internal MilestoneEventArgs(Loan loan, MilestoneEvent msEvent)
    {
      this.loan = loan;
      this.msEvent = msEvent;
    }

    /// <summary>Gets the loan object</summary>
    public Loan Loan => this.loan;

    /// <summary>Gets the MilestoneEvent object</summary>
    public MilestoneEvent MilestoneEvent => this.msEvent;
  }
}
