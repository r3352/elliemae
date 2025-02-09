// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogAlerts
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LogAlerts : ILogAlerts, IEnumerable
  {
    private LogEntry logEntry;
    private ArrayList alerts = new ArrayList();

    internal LogAlerts(LogEntry logEntry)
    {
      this.logEntry = logEntry;
      foreach (LogAlert alert in (CollectionBase) logEntry.Unwrap().AlertList)
        this.alerts.Add((object) new LogAlert(logEntry, alert));
    }

    public int Count => this.alerts.Count;

    public LogAlert Add(Role roleToAlert, DateTime dueDate)
    {
      if (roleToAlert == null)
        throw new ArgumentNullException(nameof (roleToAlert));
      if (this.Count >= 3)
        throw new InvalidOperationException("At most three alerts can be created for a LogEntry");
      LogAlert alert = new LogAlert(this.logEntry.Loan.Session.UserID);
      alert.DueDate = dueDate;
      alert.RoleId = roleToAlert.ID;
      this.logEntry.Unwrap().AlertList.Add(alert);
      LogAlert logAlert = new LogAlert(this.logEntry, alert);
      this.alerts.Add((object) logAlert);
      return logAlert;
    }

    public LogAlert this[int index] => (LogAlert) this.alerts[index];

    public void Remove(LogAlert alertToRemove) => this.alerts.Remove((object) alertToRemove);

    public IEnumerator GetEnumerator() => this.alerts.GetEnumerator();
  }
}
