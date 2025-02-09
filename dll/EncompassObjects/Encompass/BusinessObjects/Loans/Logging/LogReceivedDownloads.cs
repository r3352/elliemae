// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogReceivedDownloads
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LogReceivedDownloads : LoanLogEntryCollection, ILogReceivedDownloads, IEnumerable
  {
    internal LogReceivedDownloads(Loan loan)
      : base(loan, typeof (DownloadLog))
    {
    }

    public ReceivedDownload this[int index] => (ReceivedDownload) this.LogEntries[index];

    public void Remove(ReceivedDownload download)
    {
      if (download == null)
        throw new ArgumentNullException(nameof (download));
      this.RemoveEntry((LogEntry) download);
    }

    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new ReceivedDownload(this.Loan, (DownloadLog) logRecord);
    }
  }
}
