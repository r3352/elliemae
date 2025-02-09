// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogHtmlEmailMessages
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.HtmlEmailMessage" /> objects held within
  /// a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLog" />.
  /// </summary>
  public class LogHtmlEmailMessages : LoanLogEntryCollection, ILogHtmlEmailMessages, IEnumerable
  {
    internal LogHtmlEmailMessages(Loan loan)
      : base(loan, typeof (HtmlEmailLog))
    {
    }

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.HtmlEmailMessage" /> from the collection based on its index.
    /// </summary>
    public HtmlEmailMessage this[int index] => (HtmlEmailMessage) this.LogEntries[index];

    /// <summary>
    /// Removes an <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.HtmlEmailMessage" /> from the log.
    /// </summary>
    /// <param name="message">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.HtmlEmailMessage" /> to be removed.
    /// The specified entry must be an instance that belongs to the
    /// current Loan object.</param>
    public void Remove(HtmlEmailMessage message) => this.RemoveEntry((LogEntry) message);

    /// <summary>Wraps a LogRecord in a LogEntry object.</summary>
    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new HtmlEmailMessage(this.Loan, (HtmlEmailLog) logRecord);
    }
  }
}
