// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LogEntryEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Event arguments for the LogEntry-related loan events.</summary>
  public class LogEntryEventArgs : EventArgs
  {
    private Loan loan;
    private LogEntry logEntry;

    internal LogEntryEventArgs(Loan loan, LogEntry logEntry)
    {
      this.loan = loan;
      this.logEntry = logEntry;
    }

    /// <summary>Gets the Loan object</summary>
    public Loan Loan => this.loan;

    /// <summary>Gets the LogEntry object</summary>
    public LogEntry LogEntry => this.logEntry;
  }
}
