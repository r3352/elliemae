// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.UnderwritingCondition
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents the tracking information for an underwriting condition.
  /// </summary>
  public class UnderwritingCondition : Condition, IUnderwritingCondition
  {
    private UnderwritingConditionLog logItem;
    private Role forRole;

    internal UnderwritingCondition(Loan loan, UnderwritingConditionLog logItem)
      : base(loan, (StandardConditionLog) logItem)
    {
      this.logItem = logItem;
    }

    /// <summary>Gets the type of log entry represented by the object.</summary>
    public override LogEntryType EntryType => LogEntryType.UnderwritingCondition;

    /// <summary>Gets or sets the category of the condition.</summary>
    /// <remarks>Valid values recognized by Encompass are: Assets, Credit, Income, Legal,
    /// Property and Misc.</remarks>
    public string Category
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Category;
      }
      set
      {
        this.EnsureEditable();
        this.logItem.Category = value;
      }
    }

    /// <summary>
    /// Gets or sets the event prior to which the condition must be satisfied.
    /// </summary>
    /// <remarks>Valid values recognized by Encompass are: PTA, PTF, PTD or AC.</remarks>
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

    /// <summary>
    /// Indicates if the condition has been marked as received.
    /// </summary>
    public bool Received => this.logItem.Received;

    /// <summary>Gets or sets the date the condition was received.</summary>
    /// <remarks>To clear the received state of the condition, set this property to <c>null</c>.</remarks>
    public object DateReceived
    {
      get => !this.logItem.Received ? (object) null : (object) this.logItem.DateReceived;
      set
      {
        if (value == null)
          this.logItem.UnmarkAsReceived();
        else if (this.logItem.ReceivedBy != "")
          this.logItem.MarkAsReceived((DateTime) value, this.logItem.ReceivedBy);
        else
          this.logItem.MarkAsReceived((DateTime) value, this.Loan.Session.UserID);
      }
    }

    /// <summary>
    /// Gets or sets the ID of the user who has received this Condition.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.UnderwritingCondition.DateReceived" /> property must be set prior to setting this property.</remarks>
    public string ReceivedBy
    {
      get => !this.logItem.Received ? "" : this.logItem.ReceivedBy;
      set
      {
        if (!this.Received)
          throw new InvalidOperationException("The DateReceived property must be set prior to this operation");
        if ((value ?? "") == "")
          throw new ArgumentException("Invalid user ID specified");
        this.logItem.MarkAsReceived(this.logItem.DateReceived, value);
      }
    }

    /// <summary>
    /// Indicates if the condition has been marked as received.
    /// </summary>
    public bool Reviewed => this.logItem.Reviewed;

    /// <summary>Gets or sets the date the condition was reviewed.</summary>
    /// <remarks>To clear the reviewed state of the condition, set this property to <c>null</c>.</remarks>
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

    /// <summary>
    /// Gets or sets the ID of the user who has reviewed this Condition.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.UnderwritingCondition.DateReviewed" /> property must be set prior to setting this property.</remarks>
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

    /// <summary>Gets or sets whether the condition has been cleared</summary>
    public bool Cleared
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Cleared;
      }
    }

    /// <summary>Gets or sets the date the condition was cleared.</summary>
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

    /// <summary>
    /// Gets or sets the ID of the user who has cleared this Condition.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.UnderwritingCondition.DateCleared" /> property must be set prior to setting this property.</remarks>
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

    /// <summary>Indicates if the condition has been marked as waived.</summary>
    public bool Waived => this.logItem.Waived;

    /// <summary>Gets or sets the date the condition was waived.</summary>
    /// <remarks>To clear the waived state of the condition, set this property to <c>null</c>.</remarks>
    public object DateWaived
    {
      get => !this.logItem.Waived ? (object) null : (object) this.logItem.DateWaived;
      set
      {
        if (value == null)
          this.logItem.UnmarkAsWaived();
        else if (this.logItem.ReceivedBy != "")
          this.logItem.MarkAsWaived((DateTime) value, this.logItem.WaivedBy);
        else
          this.logItem.MarkAsWaived((DateTime) value, this.Loan.Session.UserID);
      }
    }

    /// <summary>
    /// Gets or sets the ID of the user who has cleared this Condition.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.UnderwritingCondition.DateWaived" /> property must be set prior to setting this property.</remarks>
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

    /// <summary>
    /// Indicates if the condition has been marked as rejected.
    /// </summary>
    public bool Rejected => this.logItem.Rejected;

    /// <summary>Gets or sets the date the condition was rejected.</summary>
    /// <remarks>To clear the rejected state of the condition, set this property to <c>null</c>.</remarks>
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

    /// <summary>
    /// Gets or sets the ID of the user who has rejected this Condition.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.UnderwritingCondition.DateRejected" /> property must be set prior to setting this property.</remarks>
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

    /// <summary>
    /// Indicates if the condition has been marked as fulfilled.
    /// </summary>
    public bool Fulfilled => this.logItem.Fulfilled;

    /// <summary>Gets or sets the date the condition was fulfilled.</summary>
    /// <remarks>To clear the fulfilled state of the condition, set this property to <c>null</c>.</remarks>
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

    /// <summary>
    /// Gets or sets the ID of the user who has fulfilled this Condition.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.UnderwritingCondition.DateFulfilled" /> property must be set prior to setting this property.</remarks>
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

    /// <summary>
    /// Gets or sets whether the condition is used for internal display.
    /// </summary>
    public bool ForInternalUse
    {
      get => this.logItem.IsInternal;
      set => this.logItem.IsInternal = value;
    }

    /// <summary>
    /// Gets or sets whether the condition is used for external display.
    /// </summary>
    public bool ForExternalUse
    {
      get => this.logItem.IsExternal;
      set => this.logItem.IsExternal = value;
    }

    /// <summary>
    /// Gets or sets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> which this conditiond is meant for.
    /// </summary>
    /// <remarks>If no role has been selected, this property will return <c>null</c>.</remarks>
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

    /// <summary>
    /// Gets or sets if a user in the specified <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.UnderwritingCondition.ForRole" /> is allowed to clear the condition.
    /// </summary>
    public bool AllowToClear
    {
      get => this.logItem.AllowToClear;
      set => this.logItem.AllowToClear = value;
    }

    /// <summary>
    /// Indicates if the condition has been marked as Requested.
    /// </summary>
    public bool Requested => this.logItem.Requested;

    /// <summary>Gets or sets the date the condition was Requested.</summary>
    /// <remarks>To clear the Requested state of the condition, set this property to <c>null</c>.</remarks>
    public object DateRequested
    {
      get => !this.logItem.Requested ? (object) null : (object) this.logItem.DateRequested;
      set
      {
        if (value == null)
          this.logItem.UnmarkAsRequested();
        else if (this.logItem.RequestedBy != "")
          this.logItem.MarkAsRequested((DateTime) value, this.logItem.RequestedBy);
        else
          this.logItem.MarkAsRequested((DateTime) value, this.Loan.Session.UserID);
      }
    }

    /// <summary>
    /// Gets or sets the ID of the user who has Requested this Condition.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.UnderwritingCondition.DateRequested" /> property must be set prior to setting this property.</remarks>
    public string RequestedBy
    {
      get => !this.logItem.Requested ? "" : this.logItem.RequestedBy;
      set
      {
        if (!this.Requested)
          throw new InvalidOperationException("The DateRequested property must be set prior to this operation");
        if ((value ?? "") == "")
          throw new ArgumentException("Invalid user ID specified");
        this.logItem.MarkAsRequested(this.logItem.DateRequested, value);
      }
    }

    /// <summary>
    /// Indicates if the condition has been marked as Rerequested.
    /// </summary>
    public bool Rerequested => this.logItem.Rerequested;

    /// <summary>Gets or sets the date the condition was Rerequested.</summary>
    /// <remarks>To clear the Rerequested state of the condition, set this property to <c>null</c>.</remarks>
    public object DateRerequested
    {
      get => !this.logItem.Rerequested ? (object) null : (object) this.logItem.DateRerequested;
      set
      {
        if (value == null)
          this.logItem.UnmarkAsRerequested();
        else if (this.logItem.RerequestedBy != "")
          this.logItem.MarkAsRerequested((DateTime) value, this.logItem.RerequestedBy);
        else
          this.logItem.MarkAsRerequested((DateTime) value, this.Loan.Session.UserID);
      }
    }

    /// <summary>
    /// Gets or sets the ID of the user who has Rerequested this Condition.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.UnderwritingCondition.DateRerequested" /> property must be set prior to setting this property.</remarks>
    public string RerequestedBy
    {
      get => !this.logItem.Rerequested ? "" : this.logItem.RerequestedBy;
      set
      {
        if (!this.Rerequested)
          throw new InvalidOperationException("The DateRerequested property must be set prior to this operation");
        if ((value ?? "") == "")
          throw new ArgumentException("Invalid user ID specified");
        this.logItem.MarkAsRerequested(this.logItem.DateRerequested, value);
      }
    }
  }
}
