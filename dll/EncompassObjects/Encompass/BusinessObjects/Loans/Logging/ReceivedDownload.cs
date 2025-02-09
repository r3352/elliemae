// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ReceivedDownload
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
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

    public override LogEntryType EntryType => LogEntryType.ReceivedDownload;

    public string Title
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Title;
      }
    }

    public string Sender
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Sender;
      }
    }

    public DateTime Date => Convert.ToDateTime(base.Date);
  }
}
