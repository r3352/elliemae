// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.PostClosingCondition
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class PostClosingCondition : Condition, IPostClosingCondition
  {
    private PostClosingConditionLog logItem;

    internal PostClosingCondition(Loan loan, PostClosingConditionLog logItem)
      : base(loan, (StandardConditionLog) logItem)
    {
      this.logItem = logItem;
    }

    public override LogEntryType EntryType => LogEntryType.PostClosingCondition;

    public string Recipient
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Recipient;
      }
      set
      {
        this.EnsureEditable();
        this.logItem.Recipient = value ?? "";
      }
    }

    public int DaysToReceive
    {
      get => ((StandardConditionLog) this.logItem).DaysTillDue;
      set => ((StandardConditionLog) this.logItem).DaysTillDue = value;
    }

    public bool PrintInternally
    {
      get => this.logItem.IsInternal;
      set => this.logItem.IsInternal = value;
    }

    public bool PrintExternally
    {
      get => this.logItem.IsExternal;
      set => this.logItem.IsExternal = value;
    }

    public bool Expected => ((StandardConditionLog) this.logItem).Expected;

    public object DateExpected
    {
      get
      {
        return !((StandardConditionLog) this.logItem).Expected ? (object) null : (object) ((StandardConditionLog) this.logItem).DateExpected;
      }
    }

    public bool Received => ((StandardConditionLog) this.logItem).Received;

    public object DateReceived
    {
      get
      {
        return !((StandardConditionLog) this.logItem).Received ? (object) null : (object) ((StandardConditionLog) this.logItem).DateReceived;
      }
      set
      {
        if (value == null)
          ((StandardConditionLog) this.logItem).UnmarkAsReceived();
        else if (((StandardConditionLog) this.logItem).ReceivedBy != "")
          ((StandardConditionLog) this.logItem).MarkAsReceived((DateTime) value, ((StandardConditionLog) this.logItem).ReceivedBy);
        else
          ((StandardConditionLog) this.logItem).MarkAsReceived((DateTime) value, this.Loan.Session.UserID);
      }
    }

    public string ReceivedBy
    {
      get
      {
        return !((StandardConditionLog) this.logItem).Received ? "" : ((StandardConditionLog) this.logItem).ReceivedBy;
      }
      set
      {
        if (!this.Received)
          throw new InvalidOperationException("The DateReceived property must be set prior to this operation");
        if ((value ?? "") == "")
          throw new ArgumentException("Invalid user ID specified");
        ((StandardConditionLog) this.logItem).MarkAsReceived(((StandardConditionLog) this.logItem).DateReceived, value);
      }
    }

    public bool Requested => ((StandardConditionLog) this.logItem).Requested;

    public object DateRequested
    {
      get
      {
        return !((StandardConditionLog) this.logItem).Requested ? (object) null : (object) ((StandardConditionLog) this.logItem).DateRequested;
      }
      set
      {
        if (value == null)
          ((StandardConditionLog) this.logItem).UnmarkAsRequested();
        else if (((StandardConditionLog) this.logItem).RequestedBy != "")
          ((StandardConditionLog) this.logItem).MarkAsRequested((DateTime) value, ((StandardConditionLog) this.logItem).RequestedBy);
        else
          ((StandardConditionLog) this.logItem).MarkAsRequested((DateTime) value, this.Loan.Session.UserID);
      }
    }

    public string RequestedBy
    {
      get
      {
        return !((StandardConditionLog) this.logItem).Requested ? "" : ((StandardConditionLog) this.logItem).RequestedBy;
      }
      set
      {
        if (!this.Requested)
          throw new InvalidOperationException("The DateRequested property must be set prior to this operation");
        if ((value ?? "") == "")
          throw new ArgumentException("Invalid user ID specified");
        ((StandardConditionLog) this.logItem).MarkAsRequested(((StandardConditionLog) this.logItem).DateRequested, value);
      }
    }

    public bool Rerequested => ((StandardConditionLog) this.logItem).Rerequested;

    public object DateRerequested
    {
      get
      {
        return !((StandardConditionLog) this.logItem).Rerequested ? (object) null : (object) ((StandardConditionLog) this.logItem).DateRerequested;
      }
      set
      {
        if (value == null)
          ((StandardConditionLog) this.logItem).UnmarkAsRerequested();
        else if (((StandardConditionLog) this.logItem).RerequestedBy != "")
          ((StandardConditionLog) this.logItem).MarkAsRerequested((DateTime) value, ((StandardConditionLog) this.logItem).RerequestedBy);
        else
          ((StandardConditionLog) this.logItem).MarkAsRerequested((DateTime) value, this.Loan.Session.UserID);
      }
    }

    public string RerequestedBy
    {
      get
      {
        return !((StandardConditionLog) this.logItem).Rerequested ? "" : ((StandardConditionLog) this.logItem).RerequestedBy;
      }
      set
      {
        if (!this.Rerequested)
          throw new InvalidOperationException("The DateRerequested property must be set prior to this operation");
        if ((value ?? "") == "")
          throw new ArgumentException("Invalid user ID specified");
        ((StandardConditionLog) this.logItem).MarkAsRerequested(((StandardConditionLog) this.logItem).DateRerequested, value);
      }
    }

    public string RequestedFrom
    {
      get
      {
        return !((StandardConditionLog) this.logItem).Requested ? "" : ((StandardConditionLog) this.logItem).RequestedFrom;
      }
      set
      {
        if (!this.Requested)
          throw new InvalidOperationException("The Requested property must be set prior to this operation");
        ((StandardConditionLog) this.logItem).RequestedFrom = !((value ?? "") == "") ? value : throw new ArgumentException("Invalid user ID specified");
      }
    }

    public bool Sent => this.logItem.Sent;

    public object DateSent
    {
      get => !this.logItem.Sent ? (object) null : (object) this.logItem.DateSent;
      set
      {
        if (value == null)
          this.logItem.UnmarkAsSent();
        else if (((StandardConditionLog) this.logItem).ReceivedBy != "")
          this.logItem.MarkAsSent((DateTime) value, this.logItem.SentBy);
        else
          this.logItem.MarkAsSent((DateTime) value, this.Loan.Session.UserID);
      }
    }

    public string SentBy
    {
      get => !this.logItem.Sent ? "" : this.logItem.SentBy;
      set
      {
        if (!this.Sent)
          throw new InvalidOperationException("The DateSent property must be set prior to this operation");
        if ((value ?? "") == "")
          throw new ArgumentException("Invalid user ID specified");
        this.logItem.MarkAsSent(this.logItem.DateSent, value);
      }
    }

    public bool Cleared
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Cleared;
      }
    }

    public object DateCleared
    {
      get => !this.logItem.Cleared ? (object) null : (object) this.logItem.DateCleared;
      set
      {
        if (value == null)
          this.logItem.UnmarkAsCleared();
        else if (this.logItem.Cleared)
          this.logItem.MarkAsCleared((DateTime) value, this.logItem.ClearedBy);
        else
          this.logItem.MarkAsCleared((DateTime) value, this.Loan.Session.UserID);
      }
    }

    public string ClearedBy
    {
      get => !this.logItem.Cleared ? "" : this.logItem.ClearedBy;
      set
      {
        if (!this.Cleared)
          throw new InvalidOperationException("The DateCleared property must be set prior to this operation");
        if ((value ?? "") == "")
          throw new ArgumentException("Invalid user ID specified");
        this.logItem.MarkAsCleared(this.logItem.DateCleared, value);
      }
    }
  }
}
