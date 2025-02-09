// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.PreliminaryCondition
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class PreliminaryCondition : Condition, IPreliminaryCondition
  {
    private PreliminaryConditionLog logItem;

    internal PreliminaryCondition(Loan loan, PreliminaryConditionLog logItem)
      : base(loan, (StandardConditionLog) logItem)
    {
      this.logItem = logItem;
    }

    public override LogEntryType EntryType => LogEntryType.PreliminaryCondition;

    public string Category
    {
      get
      {
        this.EnsureValid();
        return ((ConditionLog) this.logItem).Category;
      }
      set
      {
        this.EnsureEditable();
        ((ConditionLog) this.logItem).Category = value;
      }
    }

    public string PriorTo
    {
      get
      {
        this.EnsureValid();
        return this.logItem.PriorTo;
      }
      set
      {
        this.EnsureEditable();
        this.logItem.PriorTo = value;
      }
    }

    public bool AllowUnderwriterAccess
    {
      get
      {
        this.EnsureValid();
        return this.logItem.UnderwriterAccess;
      }
      set
      {
        this.EnsureEditable();
        this.logItem.UnderwriterAccess = value;
      }
    }

    public bool Fulfilled
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Fulfilled;
      }
    }

    public object DateFulfilled
    {
      get => !this.logItem.Fulfilled ? (object) null : (object) this.logItem.DateFulfilled;
      set
      {
        if (value == null)
          this.logItem.UnmarkAsFulfilled();
        else if (this.logItem.FulfilledBy != "")
          this.logItem.MarkAsFulfilled((DateTime) value, this.logItem.FulfilledBy);
        else
          this.logItem.MarkAsFulfilled((DateTime) value, this.Loan.Session.UserID);
      }
    }

    public string FulfilledBy
    {
      get => !this.logItem.Fulfilled ? "" : this.logItem.FulfilledBy;
      set
      {
        if (!this.Fulfilled)
          throw new InvalidOperationException("The DateFulfilled property must be set prior to this operation");
        if ((value ?? "") == "")
          throw new ArgumentException("Invalid user ID specified");
        this.logItem.MarkAsFulfilled(this.logItem.DateFulfilled, value);
      }
    }

    public bool Received => ((StandardConditionLog) this.logItem).Received;

    public bool Requested => ((StandardConditionLog) this.logItem).Requested;

    public bool Rerequested => ((StandardConditionLog) this.logItem).Rerequested;

    public object DateExpected
    {
      get
      {
        return !((StandardConditionLog) this.logItem).Expected ? (object) null : (object) ((StandardConditionLog) this.logItem).DateExpected;
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

    public int DaysToReceive
    {
      get => ((StandardConditionLog) this.logItem).DaysTillDue;
      set => ((StandardConditionLog) this.logItem).DaysTillDue = value;
    }
  }
}
