// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogReceivedDownloads
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Summary description for ReceivedFaxes.</summary>
  public class LogReceivedDownloads : LoanLogEntryCollection, ILogReceivedDownloads, IEnumerable
  {
    internal LogReceivedDownloads(Loan loan)
      : base(loan, typeof (DownloadLog))
    {
    }

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.ReceivedDownload">ReceivedDownload</see>
    /// from the collection based on its index.
    /// </summary>
    public ReceivedDownload this[int index] => (ReceivedDownload) this.LogEntries[index];

    /// <summary>
    /// Removes a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.ReceivedDownload" /> from the loan.
    /// </summary>
    /// <param name="download">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.ReceivedDownload" /> to be removed.
    /// The specified entry must be an instance that belongs to the
    /// current Loan object.</param>
    public void Remove(ReceivedDownload download)
    {
      if (download == null)
        throw new ArgumentNullException(nameof (download));
      this.RemoveEntry((LogEntry) download);
    }

    /// <summary>Wraps a LogRecord in a LogEntry object.</summary>
    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new ReceivedDownload(this.Loan, (DownloadLog) logRecord);
    }
  }
}
