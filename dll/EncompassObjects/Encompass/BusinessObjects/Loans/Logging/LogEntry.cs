// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntry
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public abstract class LogEntry : ILogEntry
  {
    private Loan loan;
    private LogRecordBase logItem;
    private LogAlerts alerts;

    internal LogEntry(Loan loan, LogRecordBase logItem)
    {
      this.loan = loan;
      this.logItem = logItem;
    }

    public string ID
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Guid;
      }
    }

    public object Date
    {
      get
      {
        this.EnsureValid();
        DateTime dateTime = this.logItem.Date;
        int year1 = dateTime.Year;
        dateTime = DateTime.MaxValue;
        int year2 = dateTime.Year;
        if (year1 == year2)
          return (object) null;
        return this.logItem.Date.Year == DateTime.MinValue.Year ? (object) null : (object) this.logItem.Date;
      }
    }

    public string Comments
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Comments;
      }
      set
      {
        this.EnsureEditable();
        this.logItem.Comments = value ?? "";
      }
    }

    public LogAlerts RoleAlerts
    {
      get
      {
        this.EnsureValid();
        if (this.alerts == null)
          this.alerts = new LogAlerts(this);
        return this.alerts;
      }
    }

    public virtual bool IsAlert => false;

    public abstract LogEntryType EntryType { get; }

    internal Loan Loan => this.loan;

    internal LogRecordBase Unwrap()
    {
      this.EnsureValid();
      return this.logItem;
    }

    internal void EnsureEditable() => this.EnsureValid();

    internal void EnsureValid()
    {
      if (this.logItem == null)
        throw new ObjectDisposedException("The object reference is no longer associated with a valid Loan instance.");
    }

    internal virtual void Dispose() => this.logItem = (LogRecordBase) null;
  }
}
