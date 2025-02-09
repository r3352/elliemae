// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogAlerts
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Summary description for LogAlerts.</summary>
  public class LogAlerts : ILogAlerts, IEnumerable
  {
    private LogEntry logEntry;
    private ArrayList alerts = new ArrayList();

    internal LogAlerts(LogEntry logEntry)
    {
      this.logEntry = logEntry;
      foreach (EllieMae.EMLite.DataEngine.Log.LogAlert alert in (CollectionBase) logEntry.Unwrap().AlertList)
        this.alerts.Add((object) new LogAlert(logEntry, alert));
    }

    /// <summary>Gets the number of alerts in the collection.</summary>
    /// <remarks>A <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntry" /> can have at most three alerts.</remarks>
    public int Count => this.alerts.Count;

    /// <summary>
    /// Creates a new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogAlert" /> and adds it to the collection.
    /// </summary>
    /// <param name="roleToAlert">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> to be alerted.</param>
    /// <param name="dueDate">The date on which the alert is due.</param>
    /// <returns>Returns a new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogAlert" />.</returns>
    /// <remarks>A <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntry" /> can have at most three alerts. Attempting to add more than
    /// three alerts will result in an exception.</remarks>
    public LogAlert Add(Role roleToAlert, DateTime dueDate)
    {
      if (roleToAlert == null)
        throw new ArgumentNullException(nameof (roleToAlert));
      if (this.Count >= 3)
        throw new InvalidOperationException("At most three alerts can be created for a LogEntry");
      EllieMae.EMLite.DataEngine.Log.LogAlert logAlert1 = new EllieMae.EMLite.DataEngine.Log.LogAlert(this.logEntry.Loan.Session.UserID);
      logAlert1.DueDate = dueDate;
      logAlert1.RoleId = roleToAlert.ID;
      this.logEntry.Unwrap().AlertList.Add(logAlert1);
      LogAlert logAlert2 = new LogAlert(this.logEntry, logAlert1);
      this.alerts.Add((object) logAlert2);
      return logAlert2;
    }

    /// <summary>Indexer to access alerts by index.</summary>
    public LogAlert this[int index] => (LogAlert) this.alerts[index];

    /// <summary>Removes an alert from the collection.</summary>
    /// <param name="alertToRemove">The alert to be removed.</param>
    public void Remove(LogAlert alertToRemove) => this.alerts.Remove((object) alertToRemove);

    /// <summary>Provides a enumerator for the collection of alerts.</summary>
    /// <returns></returns>
    public IEnumerator GetEnumerator() => this.alerts.GetEnumerator();
  }
}
