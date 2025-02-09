// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogMilestoneEvents
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LogMilestoneEvents : LoanLogEntryCollection, ILogMilestoneEvents, IEnumerable
  {
    private Hashtable msLookup;

    internal LogMilestoneEvents(Loan loan)
      : base(loan, typeof (MilestoneLog))
    {
    }

    public MilestoneEvent this[int index] => (MilestoneEvent) this.LogEntries[index];

    public MilestoneEvent GetEventForMilestone(string milestoneName)
    {
      return (milestoneName ?? "") != null ? (MilestoneEvent) this.msLookup[(object) milestoneName] : throw new ArgumentException("Invalid or null milestone name");
    }

    public MilestoneEvent GetEventByMilestoneID(string milestoneID)
    {
      if ((milestoneID ?? "") == null)
        throw new ArgumentException("Invalid or null milestone ID");
      foreach (MilestoneEvent eventByMilestoneId in (IEnumerable) this.msLookup.Values)
      {
        if (eventByMilestoneId.MilestoneID == milestoneID)
          return eventByMilestoneId;
      }
      return (MilestoneEvent) null;
    }

    public MilestoneEvent LastCompletedEvent
    {
      get
      {
        LogList logList = this.Loan.LoanData.GetLogList();
        int milestoneIndex = logList.GetMilestoneIndex(logList.NextStage);
        MilestoneEvent milestoneEvent = this[milestoneIndex];
        return milestoneEvent.Completed ? milestoneEvent : this[milestoneIndex - 1];
      }
    }

    public MilestoneEvent NextEvent
    {
      get
      {
        MilestoneEvent milestoneEvent = (MilestoneEvent) this.msLookup[(object) this.Loan.LoanData.GetLogList().NextStage];
        return !milestoneEvent.Completed ? milestoneEvent : (MilestoneEvent) null;
      }
    }

    internal MilestoneEvent GetEventByInternalName(string stageName)
    {
      foreach (MilestoneEvent eventByInternalName in (LoanLogEntryCollection) this)
      {
        if (eventByInternalName.InternalName == stageName)
          return eventByInternalName;
      }
      return (MilestoneEvent) null;
    }

    internal override LogEntryList GetLogEntriesFromLoan(LogList log)
    {
      LogEntryList logEntriesFromLoan = new LogEntryList();
      this.msLookup = CollectionsUtil.CreateCaseInsensitiveHashtable();
      int numberOfMilestones = log.GetNumberOfMilestones();
      for (int index = 0; index < numberOfMilestones; ++index)
      {
        MilestoneLog milestoneAt = log.GetMilestoneAt(index);
        this.msLookup.Add((object) milestoneAt.Stage, (object) this.Wrap((LogRecordBase) milestoneAt));
        logEntriesFromLoan.Add((LogEntry) this.msLookup[(object) milestoneAt.Stage]);
      }
      return logEntriesFromLoan;
    }

    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new MilestoneEvent(this.Loan, (MilestoneLog) logRecord);
    }
  }
}
