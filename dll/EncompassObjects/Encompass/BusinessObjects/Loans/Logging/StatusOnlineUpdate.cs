// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.StatusOnlineUpdate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class StatusOnlineUpdate : LogEntry, IStatusOnlineUpdate
  {
    private Loan loan;
    private StatusOnlineLog logItem;
    private StatusOnlineEvents events;

    internal StatusOnlineUpdate(Loan loan, StatusOnlineLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.loan = loan;
      this.logItem = logItem;
      this.events = new StatusOnlineEvents((IEnumerable) logItem.ItemList);
    }

    public override LogEntryType EntryType => LogEntryType.StatusOnlineUpdate;

    public DateTime Date => Convert.ToDateTime(base.Date);

    public string Description
    {
      get
      {
        this.EnsureValid();
        return ((SystemLog) this.logItem).Description;
      }
    }

    public string Creator
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Creator;
      }
    }

    public StatusOnlineEvents PublishedEvents
    {
      get
      {
        this.EnsureValid();
        return this.events;
      }
    }
  }
}
