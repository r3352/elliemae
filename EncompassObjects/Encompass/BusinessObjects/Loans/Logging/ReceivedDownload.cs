// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ReceivedDownload
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents a download that has been received and imported into Encompass.
  /// </summary>
  public class ReceivedDownload : LogEntry, IReceivedDownload
  {
    private Loan loan;
    private DownloadLog logItem;

    internal ReceivedDownload(Loan loan, DownloadLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.loan = loan;
      this.logItem = logItem;
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType" /> for the current entry.
    /// </summary>
    /// <remarks>This property will always return the value
    /// <see cref="F:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType.ReceivedDownload" />.</remarks>
    public override LogEntryType EntryType => LogEntryType.ReceivedDownload;

    /// <summary>Gets the Title of the Download.</summary>
    public string Title
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Title;
      }
    }

    /// <summary>Gets the Sender of the Download.</summary>
    public string Sender
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Sender;
      }
    }

    /// <summary>
    /// Gets the date on which the download was imported into Encompass.
    /// </summary>
    public DateTime Date => Convert.ToDateTime(base.Date);
  }
}
