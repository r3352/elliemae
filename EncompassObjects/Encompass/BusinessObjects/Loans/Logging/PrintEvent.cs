// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.PrintEvent
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Represents a print event associated with a Loan.</summary>
  /// <remarks>A Print event records a list of printed documents and the user
  /// who performed the action.
  /// </remarks>
  public class PrintEvent : LogEntry, IPrintEvent
  {
    private Loan loan;
    private PrintLog logItem;
    private PrintDocuments docs;

    internal PrintEvent(Loan loan, PrintLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.loan = loan;
      this.logItem = logItem;
      this.docs = new PrintDocuments((IEnumerable) logItem.ItemList);
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType" /> for the current entry.
    /// </summary>
    /// <remarks>This property will always return the value
    /// <see cref="F:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType.PrintEvent" />.</remarks>
    public override LogEntryType EntryType => LogEntryType.PrintEvent;

    /// <summary>Gets or sets the person who performed the printing.</summary>
    public string PerformedBy
    {
      get
      {
        this.EnsureValid();
        return this.logItem.PrintedBy;
      }
      set
      {
        this.EnsureEditable();
        this.logItem.PrintedBy = value ?? "";
      }
    }

    /// <summary>Gets or sets the person who performed the printing.</summary>
    public string PerformedByUserName
    {
      get
      {
        this.EnsureValid();
        return this.logItem.PrintedByFullName;
      }
      set
      {
        this.EnsureEditable();
        this.logItem.PrintedByFullName = value ?? "";
      }
    }

    /// <summary>Gets or sets the print action performed by the user.</summary>
    public PrintAction PrintAction
    {
      get
      {
        this.EnsureValid();
        return (PrintAction) this.logItem.Action;
      }
      set => this.logItem.Action = (PrintLog.PrintAction) value;
    }

    /// <summary>
    /// Gets the collection of printed documents that were included in this print event.
    /// </summary>
    public PrintDocuments Documents
    {
      get
      {
        this.EnsureValid();
        return this.docs;
      }
    }
  }
}
