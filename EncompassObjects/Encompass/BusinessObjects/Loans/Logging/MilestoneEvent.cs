// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessEnums;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents a single milestone-related event in the lifetime of a Loan.
  /// </summary>
  /// <remarks>
  /// Inspecting a loan's MilestoneEvents is useful for determining which stages of
  /// the loan lifetime sequence have already been completed and which remain to be
  /// done. The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntry.Date">Date</see> of a MilestoneEvent represents the point in time
  /// at which the associated <see cref="T:EllieMae.Encompass.BusinessEnums.Milestone" />
  /// has either been achieved (if the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent.Completed" /> flag is <c>true</c>) or
  /// is scheduled to be achieved (if <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent.Completed" /> is <c>false</c>).
  /// <p>Some MilestoneEvents' <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntry.Date">Date</see> may be set to <c>null</c>,
  /// which indicates that the loan has not progressed far enough for an estimated completion date
  /// to be available for the associated Milestone. In particular, until the
  /// the first milestone (other than File Started) is crossed, no milestones that
  /// occur afterward are assigned scheduled
  /// completion dates. Once a milestone is completed, all subsequent milestones'
  /// expected completion dates will be populated.</p>
  /// <p>As with all LogEntries, MilestoneEvent instances become invalid
  /// when the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.Refresh">Refresh</see> method is
  /// invoked on the parent <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan">Loan</see> object. Attempting
  /// to access this object after invoking refresh will result in an
  /// exception.</p>
  /// </remarks>
  /// <example>
  /// The following displays the next milestone to be achieved for each loan
  /// in the MyPipeline folder.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessEnums;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
  /// 
  /// class LoanReader
  /// {
  ///    public static void Main()
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Get the "My Pipeline" folder
  ///       LoanFolder fol = session.Loans.Folders["My Pipeline"];
  /// 
  ///       // Retrieve the folder's contents
  ///       LoanIdentityList ids = fol.GetContents();
  /// 
  ///       // Open each loan in the folder and check the expected closing date
  ///       for (int i = 0; i < ids.Count; i++)
  ///       {
  ///          // Open the next loan in the loop
  ///          Loan loan = fol.OpenLoan(ids[i].LoanName);
  /// 
  ///          // Check if this is in the Processing stage
  ///          if (loan.Log.MilestoneEvents.NextEvent != null)
  ///             Console.WriteLine("The next milestone for the loan \"" + loan.LoanName + "\" is " + loan.Log.MilestoneEvents.NextEvent.MilestoneName + ".");
  /// 
  ///          // Close the loan
  ///          loan.Close();
  ///       }
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class MilestoneEvent : LogEntry, IMilestoneEvent
  {
    private Milestone ms;
    private MilestoneLog logItem;
    private LoanAssociate loanAssociate;

    internal MilestoneEvent(Loan loan, MilestoneLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.logItem = logItem;
      this.ms = loan.Session.Loans.Milestones.GetItemByName(logItem.Stage);
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType" /> for the current entry.
    /// </summary>
    /// <remarks>This property will always return the value
    /// <see cref="F:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType.MilestoneEvent" />.</remarks>
    public override LogEntryType EntryType => LogEntryType.MilestoneEvent;

    /// <summary>
    /// Gets the MilestoneID of the Milestone related to the MilestoneEvent.
    /// </summary>
    public string MilestoneID => this.logItem.MilestoneID;

    /// <summary>
    /// Gets the TPOConnectStatus of the Milestone related to the MilestoneEvent.
    /// </summary>
    public string TPOConnectStatus => this.logItem.TPOConnectStatus;

    /// <summary>
    /// Gets the name of the milestone with which this entry is associated.
    /// </summary>
    /// <remarks>You may use this value to attempt to locate the corresponding
    /// <see cref="T:EllieMae.Encompass.BusinessEnums.Milestone">Milestone</see> object
    /// associated with this event. However, because the lifetime sequence of the current
    /// loan was determine when the loan was created, this name may not be a currently
    /// defined milestone in the Encompass system's default milestone sequence.
    /// <p>For a list of the pre-defined Milestones which are part of every loan's
    /// lifetime sequence, see the Remarks section for the <see cref="T:EllieMae.Encompass.BusinessEnums.Milestones" />
    /// object.</p>
    /// </remarks>
    /// <example>
    /// The following displays the next milestone to be achieved for each loan
    /// in the MyPipeline folder.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Get the "My Pipeline" folder
    ///       LoanFolder fol = session.Loans.Folders["My Pipeline"];
    /// 
    ///       // Retrieve the folder's contents
    ///       LoanIdentityList ids = fol.GetContents();
    /// 
    ///       // Open each loan in the folder and check the expected closing date
    ///       for (int i = 0; i < ids.Count; i++)
    ///       {
    ///          // Open the next loan in the loop
    ///          Loan loan = fol.OpenLoan(ids[i].LoanName);
    /// 
    ///          // Check if this is in the Processing stage
    ///          if (loan.Log.MilestoneEvents.NextEvent != null)
    ///             Console.WriteLine("The next milestone for the loan \"" + loan.LoanName + "\" is " + loan.Log.MilestoneEvents.NextEvent.MilestoneName + ".");
    /// 
    ///          // Close the loan
    ///          loan.Close();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public string MilestoneName
    {
      get
      {
        this.EnsureValid();
        return (EnumItem) this.ms != (EnumItem) null ? this.ms.Name : this.logItem.Stage;
      }
    }

    /// <summary>
    /// Gets or sets a flag indicating if this milestone has been reached.
    /// </summary>
    /// <remarks>Setting this property to <c>true</c> will indicate that this milestone has been
    /// completed and will advance the loan to the next stage in the lifetime sequence. Because
    /// a complete milestone must have a date associated with it, setting this property to
    /// <c>true</c> will automatically cause the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntry.Date" /> property to
    /// be set to the current date if previously <c>null</c>.
    /// <p>Additionally, if this property is set to <c>true</c> and the current milestone is not the next
    /// milestone scheduled to be completed, all prior, incomplete miletones are marked
    /// as completed and their completion dates are updated to match the date of this milestone event.</p>
    /// <p>If this property is set to <c>false</c> and one or more completed milestones follow
    /// this event, those milestones are set to incomplete and what were previously their
    /// completion dates become their new scheduled completion dates.</p>
    /// <p>Note that an exception will be raised if you attempt to modify
    /// this property for the <see cref="P:EllieMae.Encompass.BusinessEnums.Milestones.Started">Started</see>
    /// milestone, for which this property always returns <c>true</c>.</p>
    /// </remarks>
    /// <example>
    /// The following code generates a report of all the loans in the My Pipeline
    /// folder that have been sent for processing at some point in the past.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Get the "My Pipeline" folder
    ///       LoanFolder fol = session.Loans.Folders["My Pipeline"];
    /// 
    ///       // Retrieve the folder's contents
    ///       LoanIdentityList ids = fol.GetContents();
    /// 
    ///       // Get the "Processing" milestone
    ///       Milestone processing = session.Loans.Milestones.Processing;
    /// 
    ///       // Open each loan in the folder and check the expected closing date
    ///       for (int i = 0; i < ids.Count; i++)
    ///       {
    ///          // Open the next loan in the loop
    ///          Loan loan = fol.OpenLoan(ids[i].LoanName);
    /// 
    ///          // Check if this is in the Processing stage
    ///          MilestoneEvent evnt = loan.Log.MilestoneEvents.GetEventForMilestone(processing.Name);
    /// 
    ///          if (evnt.Completed)
    ///             Console.WriteLine("The loan \"" + loan.LoanName + "\" started processing on " + evnt.Date);
    /// 
    ///          // Close the loan
    ///          loan.Close();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public bool Completed
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Done;
      }
      set
      {
        if (this.logItem.Done == value)
          return;
        this.EnsureEditable();
        if (this.ms.Name == "Started")
          throw new InvalidOperationException("Completed property cannot be modified for this milestone.");
        this.Loan.Unwrap().SetMilestoneStatus(this.logItem.MilestoneID, value);
      }
    }

    /// <summary>
    /// Gets or sets the completion date (or expected completion date) for the
    /// current milestone.
    /// </summary>
    /// <remarks>
    /// When the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent.Completed" /> property is <c>true</c>, this date represents
    /// the date on which the milestone was achieved. If <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent.Completed" /> is <c>false</c>,
    /// this date represents the projected completion date, if one is available. If no
    /// projected date is available, this property returns null.
    /// <p>You may also use this property to modify the completion date or expected completion
    /// date of a milestone in a loan. However, the new date must not precede the date of any
    /// milestone earlier in the lifetime sequence or come after any milestone later in the
    /// lifetime sequence. If you wish to adjust the entire miletone schedule, use the
    /// <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent.AdjustDate(System.Object,System.Boolean,System.Boolean)" /> method.
    /// </p>
    /// <p><note type="implementnotes">Developers of COM-based clients will need to invoke the
    /// SetDate() method in order to modify the date on this object.
    /// </note></p>
    /// </remarks>
    public new object Date
    {
      get
      {
        this.EnsureValid();
        return base.Date;
      }
      set
      {
        this.EnsureEditable();
        this.logItem.Date = value == null ? DateTime.MaxValue : Convert.ToDateTime(value);
      }
    }

    /// <summary>
    /// Provides a COM-accessible method for setting the date on a Milestone event.
    /// </summary>
    /// <param name="newDate">The date for the milestone event.</param>
    void IMilestoneEvent.SetDate(object newDate) => this.Date = newDate;

    /// <summary>
    /// Adjusts the date of a milestone event and, if desired, the previous and successive
    /// milestones events.
    /// </summary>
    /// <param name="newDate">The new completion or expected completion date for the
    /// milestone event.</param>
    /// <param name="allowAdjustPastMilestones">Passing the value <c>true</c> allows the expected
    /// completion dates for past milestones to be adjusted based on the newly specified date for
    /// the current milestone. In particular, a <c>true</c> value will have the following effects:
    /// <list type="bullet">
    /// <item>Any incomplete milestone events which precede the current milestone in the lifetime
    /// sequence and which are scheduled to be completed after <c>newDate</c> will be modified to be
    /// due on <c>newDate</c>.</item>
    /// <item>Any unscheduled milestone events which precede the current milestone will
    /// be scheduled to be completed on <c>newDate</c>.</item>
    /// </list>
    /// If this parameter is <c>false</c>, the scheduled completion times of milestone events
    /// earlier in the sequence will never be modified. As a result, if an attempt is made to
    /// schedule this milestone prior to the scheduled date of a prior milestone, or if an attempt
    /// is made to schedule a milestone while preceded by an unscheduled milestone,
    /// this method will raise an exception.</param>
    /// <param name="allowAdjustFutureMilestones">If <c>true</c> and the next milestone event is
    /// not marked as completed, then any incomplete, scheduled, future milestone events will have their expected
    /// completion dates adjusted based on the difference between the current milestone event's
    /// prior completion date and the value specified by <c>newDate</c>. For example, if you modify the date of the Submittal
    /// milestone event by adding 2 days to it and the next milestone event (Approval) has not yet been completed, then
    /// the Approval milestone and all subsequent milestones will have two days added to their scheduled
    /// completion dates. If this parameter is <c>false</c>, the dates of subsequent milestones
    /// are not adjusted and the date specified by <c>newDate</c> may not fall after the date of
    /// any subsequent milestone.
    /// </param>
    /// <remarks>
    /// This method allows the schedule for milestone events to be changed
    /// either into the past or the future. However, the date of a milestone can never be adjusted
    /// so that it precedes the date of any previously completed milestone or comes after the
    /// date of any subsequently completed milestone.
    /// </remarks>
    /// <example>
    /// The following code opens a loan and extends the deadline for all future milestones
    /// by two days.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open the loan
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    ///       loan.Lock();
    /// 
    ///       // Extend the due date of all incomplete milestone events by 2 days
    ///       MilestoneEvent e = loan.Log.MilestoneEvents.NextEvent;
    /// 
    ///       if ((e != null) && (e.Date != null))
    ///          e.AdjustDate(Convert.ToDateTime(e.Date).AddDays(2), false, true);
    /// 
    ///       // Show the new milestone dates
    ///       foreach (MilestoneEvent ms in loan.Log.MilestoneEvents)
    ///          Console.WriteLine(ms.MilestoneName + ": " + ms.Date);
    /// 
    ///       loan.Commit();
    ///       loan.Unlock();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void AdjustDate(
      object newDate,
      bool allowAdjustPastMilestones,
      bool allowAdjustFutureMilestones)
    {
      this.EnsureEditable();
      this.logItem.AdjustDate(newDate == null ? DateTime.MaxValue : Convert.ToDateTime(newDate), allowAdjustPastMilestones, allowAdjustFutureMilestones);
    }

    /// <summary>Indicates if this MilestoneEvent is in alert status.</summary>
    /// <remarks>A MilestoneEvent is considered to be an alert if both of the following
    /// conditions are met:
    /// <list type="bullet">
    /// <item>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntry.Date">Date</see> is non-null and is on or before today.</item>
    /// <item>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent.Completed" /> property is <c>false</c>.</item>
    /// </list>
    /// </remarks>
    public override bool IsAlert
    {
      get
      {
        return this.Date != null && !this.Completed && Convert.ToDateTime(this.Date).Date <= DateTime.Today;
      }
    }

    /// <summary>
    /// Gets or sets the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent.LoanAssociate" /> who is attached to the current Milestone.
    /// </summary>
    /// <remarks>
    /// This property will return <c>null</c> if there is no Role associated with this milestone
    /// based on the business process defined by the Encompass system.
    /// </remarks>
    public LoanAssociate LoanAssociate
    {
      get
      {
        this.EnsureValid();
        if (this.loanAssociate == null)
          this.loanAssociate = this.createLoanAssociate();
        return this.loanAssociate;
      }
    }

    /// <summary>
    /// Gets or sets whether the loan associate for this milestone should be given write access to
    /// the Loan after the loan has transitioned to a later milestone.
    /// </summary>
    public bool LoanAssociateAccess
    {
      get
      {
        this.EnsureValid();
        return this.logItem.LoanAssociateAccess;
      }
      set
      {
        this.EnsureEditable();
        this.logItem.LoanAssociateAccess = value;
      }
    }

    internal Milestone GetMilestone()
    {
      this.EnsureValid();
      return this.ms;
    }

    private LoanAssociate createLoanAssociate()
    {
      if (this.logItem.RoleID < 0)
        return (LoanAssociate) null;
      Role roleById = this.Loan.Session.Loans.Roles.GetRoleByID(this.logItem.RoleID);
      return roleById == null ? (LoanAssociate) null : new LoanAssociate(this.Loan, (LoanAssociateLog) this.logItem, roleById);
    }

    internal string InternalName
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Stage;
      }
    }
  }
}
