// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.UnderwritingCondition
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class UnderwritingCondition : Condition, IUnderwritingCondition
  {
    private UnderwritingConditionLog logItem;
    private Role forRole;

    internal UnderwritingCondition(Loan loan, UnderwritingConditionLog logItem)
      : base(loan, (StandardConditionLog) logItem)
    {
      this.logItem = logItem;
    }

    public override LogEntryType EntryType => LogEntryType.UnderwritingCondition;

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

    public bool Reviewed => this.logItem.Reviewed;

    public object DateReviewed
    {
      get => !this.logItem.Reviewed ? (object) null : (object) this.logItem.DateReviewed;
      set
      {
        if (value == null)
          this.logItem.UnmarkAsReviewed();
        else if (this.logItem.ReviewedBy != "")
          this.logItem.MarkAsReviewed((DateTime) value, this.logItem.ReviewedBy);
        else
          this.logItem.MarkAsReviewed((DateTime) value, this.Loan.Session.UserID);
      }
    }

    public string ReviewedBy
    {
      get => !this.logItem.Reviewed ? "" : this.logItem.ReviewedBy;
      set
      {
        if (!this.Reviewed)
          throw new InvalidOperationException("The DateReviewed property must be set prior to this operation");
        if ((value ?? "") == "")
          throw new ArgumentException("Invalid user ID specified");
        this.logItem.MarkAsReviewed(this.logItem.DateReviewed, value);
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

    public bool Waived => this.logItem.Waived;

    public object DateWaived
    {
      get => !this.logItem.Waived ? (object) null : (object) this.logItem.DateWaived;
      set
      {
        if (value == null)
          this.logItem.UnmarkAsWaived();
        else if (((StandardConditionLog) this.logItem).ReceivedBy != "")
          this.logItem.MarkAsWaived((DateTime) value, this.logItem.WaivedBy);
        else
          this.logItem.MarkAsWaived((DateTime) value, this.Loan.Session.UserID);
      }
    }

    public string WaivedBy
    {
      get => !this.logItem.Waived ? "" : this.logItem.WaivedBy;
      set
      {
        if (!this.Waived)
          throw new InvalidOperationException("The DateWaived property must be set prior to this operation");
        if ((value ?? "") == "")
          throw new ArgumentException("Invalid user ID specified");
        this.logItem.MarkAsWaived(this.logItem.DateWaived, value);
      }
    }

    public bool Rejected => this.logItem.Rejected;

    public object DateRejected
    {
      get => !this.logItem.Rejected ? (object) null : (object) this.logItem.DateRejected;
      set
      {
        if (value == null)
          this.logItem.UnmarkAsRejected();
        else if (this.logItem.RejectedBy != "")
          this.logItem.MarkAsRejected((DateTime) value, this.logItem.RejectedBy);
        else
          this.logItem.MarkAsRejected((DateTime) value, this.Loan.Session.UserID);
      }
    }

    public string RejectedBy
    {
      get => !this.logItem.Rejected ? "" : this.logItem.RejectedBy;
      set
      {
        if (!this.Rejected)
          throw new InvalidOperationException("The DateRejected property must be set prior to this operation");
        if ((value ?? "") == "")
          throw new ArgumentException("Invalid user ID specified");
        this.logItem.MarkAsRejected(this.logItem.DateRejected, value);
      }
    }

    public bool Fulfilled => this.logItem.Fulfilled;

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

    public bool ForInternalUse
    {
      get => this.logItem.IsInternal;
      set => this.logItem.IsInternal = value;
    }

    public bool ForExternalUse
    {
      get => this.logItem.IsExternal;
      set => this.logItem.IsExternal = value;
    }

    public Role ForRole
    {
      get
      {
        if (this.logItem.ForRoleID <= 0)
          return (Role) null;
        if (this.forRole != null && this.forRole.ID == this.logItem.ForRoleID)
          return this.forRole;
        this.forRole = this.Loan.Session.Loans.Roles.GetRoleByID(this.logItem.ForRoleID);
        return this.forRole;
      }
      set
      {
        this.logItem.ForRoleID = value != null ? value.ID : -1;
        this.forRole = value;
      }
    }

    public bool AllowToClear
    {
      get => this.logItem.AllowToClear;
      set => this.logItem.AllowToClear = value;
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
  }
}
