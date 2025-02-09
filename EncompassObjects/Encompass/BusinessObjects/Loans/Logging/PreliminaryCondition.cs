// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.PreliminaryCondition
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
  /// Represents the tracking information for a preliminary condition.
  /// </summary>
  public class PreliminaryCondition : Condition, IPreliminaryCondition
  {
    private PreliminaryConditionLog logItem;

    internal PreliminaryCondition(Loan loan, PreliminaryConditionLog logItem)
      : base(loan, (StandardConditionLog) logItem)
    {
      this.logItem = logItem;
    }

    /// <summary>Gets the type of log entry represented by the object.</summary>
    public override LogEntryType EntryType => LogEntryType.PreliminaryCondition;

    /// <summary>Gets or sets the category of the condition.</summary>
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
    /// Indicates if the underwriter is allowed to have access to this condition.
    /// </summary>
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

    /// <summary>
    /// Indicates if the condition has been marked as received.
    /// </summary>
    public bool Fulfilled
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Fulfilled;
      }
    }

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
    /// Gets or sets the ID of the user who has received this Condition.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.PreliminaryCondition.DateFulfilled" /> property must be set prior to setting this property.</remarks>
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
    /// Indicates if the condition has been marked as received.
    /// </summary>
    public bool Received => this.logItem.Received;

    /// <summary>
    /// Indicates if the condition has been marked as requested.
    /// </summary>
    public bool Requested => this.logItem.Requested;

    /// <summary>
    /// Indicates if the condition has been marked as re-requested.
    /// </summary>
    public bool Rerequested => this.logItem.Rerequested;

    /// <summary>
    /// Gets or sets the date the condition is expected to be received.
    /// </summary>
    /// <remarks>To clear the date expected for the condition, set this property to <c>null</c>.</remarks>
    public object DateExpected
    {
      get => !this.logItem.Expected ? (object) null : (object) this.logItem.DateExpected;
    }

    /// <summary>
    /// Gets or sets the ID of the user who has received this Condition.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.PreliminaryCondition.DateReceived" /> property must be set prior to setting this property.</remarks>
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
    /// Gets or sets the ID of the user who has reviewed this Condition.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.PreliminaryCondition.DateRequested" /> property must be set prior to setting this property.</remarks>
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

    /// <summary>Gets or sets the date the condition was requested.</summary>
    /// <remarks>To clear the requested state of the condition, set this property to <c>null</c>.</remarks>
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

    /// <summary>Gets and sets who the condition was requested from</summary>
    public string RequestedFrom
    {
      get => !this.logItem.Requested ? "" : this.logItem.RequestedFrom;
      set
      {
        if (!this.Requested)
          throw new InvalidOperationException("The Requested property must be set prior to this operation");
        this.logItem.RequestedFrom = !((value ?? "") == "") ? value : throw new ArgumentException("Invalid user ID specified");
      }
    }

    /// <summary>
    /// Gets or sets the ID of the user who has reviewed this Condition.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.PreliminaryCondition.DateRequested" /> property must be set prior to setting this property.</remarks>
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

    /// <summary>Gets or sets the date the condition was requested.</summary>
    /// <remarks>To clear the requested state of the condition, set this property to <c>null</c>.</remarks>
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
    /// Gets or sets the number of days from the date requested until the condition is due to be received.
    /// </summary>
    public int DaysToReceive
    {
      get => this.logItem.DaysTillDue;
      set => this.logItem.DaysTillDue = value;
    }
  }
}
