// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.PostClosingCondition
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
  public class PostClosingCondition : Condition, IPostClosingCondition
  {
    private PostClosingConditionLog logItem;

    internal PostClosingCondition(Loan loan, PostClosingConditionLog logItem)
      : base(loan, (StandardConditionLog) logItem)
    {
      this.logItem = logItem;
    }

    /// <summary>Gets the type of log entry represented by the object.</summary>
    public override LogEntryType EntryType => LogEntryType.PostClosingCondition;

    /// <summary>Gets or sets the recipient of the item.</summary>
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

    /// <summary>
    /// Gets or sets the number of days from the date requested until the condition is due to be received.
    /// </summary>
    public int DaysToReceive
    {
      get => this.logItem.DaysTillDue;
      set => this.logItem.DaysTillDue = value;
    }

    /// <summary>Gets or sets settings for print internally</summary>
    public bool PrintInternally
    {
      get => this.logItem.IsInternal;
      set => this.logItem.IsInternal = value;
    }

    /// <summary>Gets or sets settings for print externally</summary>
    public bool PrintExternally
    {
      get => this.logItem.IsExternal;
      set => this.logItem.IsExternal = value;
    }

    /// <summary>
    /// Indicates if the condition has a <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.PostClosingCondition.DateExpected" /> set.
    /// </summary>
    public bool Expected => this.logItem.Expected;

    /// <summary>
    /// Gets or sets the date the condition is expected to be received.
    /// </summary>
    /// <remarks>To clear the date expected for the condition, set this property to <c>null</c>.</remarks>
    public object DateExpected
    {
      get => !this.logItem.Expected ? (object) null : (object) this.logItem.DateExpected;
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
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.PostClosingCondition.DateReceived" /> property must be set prior to setting this property.</remarks>
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
    /// Indicates if the condition has been marked as requested.
    /// </summary>
    public bool Requested => this.logItem.Requested;

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

    /// <summary>
    /// Gets or sets the ID of the user who has reviewed this Condition.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.PostClosingCondition.DateRequested" /> property must be set prior to setting this property.</remarks>
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
    /// Indicates if the condition has been marked as re-requested.
    /// </summary>
    public bool Rerequested => this.logItem.Rerequested;

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
    /// Gets or sets the ID of the user who has reviewed this Condition.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.PostClosingCondition.DateRequested" /> property must be set prior to setting this property.</remarks>
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

    /// <summary>Indicates if the condition has been marked as waived.</summary>
    public bool Sent => this.logItem.Sent;

    /// <summary>Gets or sets the date the condition was waived.</summary>
    /// <remarks>To clear the waived state of the condition, set this property to <c>null</c>.</remarks>
    public object DateSent
    {
      get => !this.logItem.Sent ? (object) null : (object) this.logItem.DateSent;
      set
      {
        if (value == null)
          this.logItem.UnmarkAsSent();
        else if (this.logItem.ReceivedBy != "")
          this.logItem.MarkAsSent((DateTime) value, this.logItem.SentBy);
        else
          this.logItem.MarkAsSent((DateTime) value, this.Loan.Session.UserID);
      }
    }

    /// <summary>
    /// Gets or sets the ID of the user who has cleared this Condition.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.PostClosingCondition.DateSent" /> property must be set prior to setting this property.</remarks>
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

    /// <summary>Gets or sets whether the condition has been cleared</summary>
    public bool Cleared
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Cleared;
      }
    }

    /// <summary>Gets or sets the date the condition was received.</summary>
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
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.PostClosingCondition.DateCleared" /> property must be set prior to setting this property.</remarks>
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
