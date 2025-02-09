// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.BusinessObjects.Users;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents a rate lock request for the secondary marketing tool.
  /// </summary>
  /// <remarks>When a lock request is made, Encompass creates a snapshot of the lock-related fields
  /// which become fixed for that lock. You can access that snapshot's values through the
  /// <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest.Fields" /> collection, which represents a read-only set of loan fields. Only fields
  /// included in the lock request are included.</remarks>
  public class LockRequest : LogEntry, ILockRequest
  {
    private LockRequestLog logItem;
    private LockConfirmation confirmation;
    private LockDenial denial;
    private LockRequestFields fields;

    internal LockRequest(Loan loan, LockRequestLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.logItem = logItem;
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType" /> for this object.
    /// </summary>
    /// <remarks>Thie property will return LogEntryType.LockRequest.</remarks>
    public override LogEntryType EntryType => LogEntryType.LockRequest;

    /// <summary>
    /// Gets the date and time on which this request was made.
    /// </summary>
    public DateTime Date => this.logItem.DateTimeRequested;

    /// <summary>
    /// Gets or sets a flag indicating if the loan's Loan Officer should be alerted.
    /// </summary>
    public bool AlertLO
    {
      get => this.logItem.AlertLoanOfficer;
      set => this.logItem.AlertLoanOfficer = value;
    }

    /// <summary>Provides the expiration of the lock request.</summary>
    public object ExpirationDate
    {
      get => this.IsExtension ? this.Fields["3364"].Value : this.Fields["2151"].Value;
    }

    /// <summary>Indicates the expiration date of a lock extension</summary>
    public object ExtensionExpirationDate
    {
      get
      {
        return !(this.logItem.BuySideNewLockExtensionDate == DateTime.MinValue) ? (object) this.logItem.BuySideNewLockExtensionDate : (object) null;
      }
    }

    /// <summary>
    /// Gets or sets the Buy Side expiration date for the request.
    /// </summary>
    public object BuySideExpirationDate
    {
      get
      {
        return !(this.logItem.BuySideExpirationDate == DateTime.MinValue) ? (object) this.logItem.BuySideExpirationDate : (object) null;
      }
      set
      {
        this.logItem.BuySideExpirationDate = value == null ? DateTime.MinValue : Convert.ToDateTime(value);
      }
    }

    /// <summary>
    /// Indicates if the request represents an extension of a previous lock request
    /// </summary>
    public bool IsExtension => this.logItem.IsLockExtension;

    /// <summary>
    /// Indicates if the request represents a relock of a previous lock request.
    /// </summary>
    public bool IsRelock => this.logItem.IsRelock;

    /// <summary>
    /// Returns the number of days by which the buy-side lock is extended.
    /// </summary>
    /// <remarks>This property returns a valid value only if the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest.IsExtension" /> property
    /// returns <c>true</c>.</remarks>
    public int BuySideExtensionDays => this.logItem.BuySideNumDayExtended;

    /// <summary>Gets the user who made the lock request.</summary>
    public string RequestedBy => this.logItem.RequestedBy ?? "";

    /// <summary>Gets LockRequestLog.</summary>
    public LockRequestLog LockRequestLog => this.logItem;

    /// <summary>Gets or sets the status of the rate lock request.</summary>
    public LockRequestStatus Status
    {
      get
      {
        if (this.logItem.RequestedStatus == "")
          return LockRequestStatus.Pending;
        if (string.Compare(this.logItem.RequestedStatus, RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RequestDenied), true) == 0)
          return LockRequestStatus.Denied;
        if (this.SellSideExpirationDate != null && Convert.ToDateTime(this.SellSideExpirationDate) < DateTime.Today)
        {
          if (string.Compare(this.logItem.RequestedStatus, RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.Requested), true) == 0)
            return LockRequestStatus.RateExpired;
          if (string.Compare(this.logItem.RequestedStatus, RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked), true) == 0)
            return LockRequestStatus.LockExpired;
        }
        if (this.ExpirationDate != null && Convert.ToDateTime(this.ExpirationDate) < DateTime.Today)
        {
          if (string.Compare(this.logItem.RequestedStatus, RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.Requested), true) == 0)
            return LockRequestStatus.RateExpired;
          if (string.Compare(this.logItem.RequestedStatus, RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked), true) == 0)
            return LockRequestStatus.LockExpired;
        }
        return (LockRequestStatus) RateLockEnum.GetRateLockEnum(this.logItem.RequestedStatus);
      }
    }

    /// <summary>
    /// Gets or sets the sell-side delivery date on the request.
    /// </summary>
    public object SellSideDeliveryDate
    {
      get
      {
        return !(this.logItem.SellSideDeliveryDate == DateTime.MinValue) ? (object) this.logItem.SellSideDeliveryDate : (object) null;
      }
      set
      {
        this.logItem.SellSideDeliveryDate = value == null ? DateTime.MinValue : Convert.ToDateTime(value);
      }
    }

    /// <summary>
    /// Gets or sets the sell-side expiration date on the request.
    /// </summary>
    public object SellSideExpirationDate
    {
      get
      {
        return !(this.logItem.SellSideExpirationDate == DateTime.MinValue) ? (object) this.logItem.SellSideExpirationDate : (object) null;
      }
      set
      {
        this.logItem.SellSideExpirationDate = value == null ? DateTime.MinValue : Convert.ToDateTime(value);
      }
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockConfirmation" /> which has been provided for this lock request.
    /// </summary>
    /// <remarks>If the request is in the Active state, then there is no lock confirmatiom
    /// and this property will always return <c>null</c>. This property may also return <c>null</c>
    /// if this lock request has been superceded by a newer request.</remarks>
    public LockConfirmation Confirmation
    {
      get
      {
        if (this.Status == LockRequestStatus.Pending)
          return (LockConfirmation) null;
        if (this.confirmation == null)
        {
          foreach (LockConfirmation lockConfirmation in (LoanLogEntryCollection) this.Loan.Log.LockConfirmations)
          {
            if (lockConfirmation.LockRequest == this && (this.confirmation == null || lockConfirmation.Date > this.confirmation.Date))
              this.confirmation = lockConfirmation;
          }
        }
        return this.confirmation;
      }
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockDenial" /> which has been provided for this lock request.
    /// </summary>
    /// <remarks>If the request is in the Denied state, use this property to access
    /// the corresponding <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockDenial" /> object. Otherwise, this property will
    /// return <c>null</c>.</remarks>
    public LockDenial Denial
    {
      get
      {
        if (this.Status != LockRequestStatus.Denied)
          return (LockDenial) null;
        if (this.denial == null)
        {
          foreach (LockDenial lockDenial in (LoanLogEntryCollection) this.Loan.Log.LockDenials)
          {
            if (lockDenial.LockRequest == this)
            {
              this.denial = lockDenial;
              break;
            }
          }
        }
        return this.denial;
      }
    }

    /// <summary>Gets the snapshot fields for this lock request.</summary>
    /// <remarks>If changes are made to the values in the lock request snapshot, the
    /// <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequestFields.CommitChanges" /> method must be called to save those changes
    /// into the loan file. Otherwise, those changes will not appear in the Encompass user interface
    /// or be saved as part of the loan.</remarks>
    /// <example>
    ///       The following code retrieves the current lock request from a loan
    ///       and then sets the sell-side lock information in the request.
    ///       <code>
    ///         <![CDATA[
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
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open and lock the loan
    ///       Loan loan = session.Loans.Open(args[0]);
    ///       loan.Lock();
    /// 
    ///       // Retrieve the current lock request, if any, from the loan
    ///       LockRequest req = loan.Log.LockRequests.GetCurrent();
    /// 
    ///       // Set the sell-side lock date and period
    ///       req.Fields["2220"].Value = DateTime.Today.AddDays(2);
    ///       req.Fields["2221"].Value = 60;
    /// 
    ///       // Set the sell-side base rate and adjustments
    ///       req.Fields["2223"].Value = 6.35;
    ///       req.Fields["2224"].Value = "45 Day Lock Period";
    ///       req.Fields["2225"].Value = 0.05;
    /// 
    ///       // Set the sell-side base price and adjustments
    ///       req.Fields["2232"].Value = 101.15;
    ///       req.Fields["2233"].Value = "FICO >= 720";
    ///       req.Fields["2234"].Value = 0.125;
    /// 
    ///       // Commit the changes the to the fields
    ///       req.Fields.CommitChanges();
    /// 
    ///       // Save and close the loan file
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public LockRequestFields Fields
    {
      get
      {
        if (this.fields == null)
          this.fields = new LockRequestFields(this, this.logItem.GetLockRequestSnapshot());
        return this.fields;
      }
    }

    /// <summary>
    /// Determines if this request represents an active request or lock.
    /// </summary>
    /// <returns>Returns <c>true</c> is the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest.Status" /> is Pending or RateLocked,
    /// <c>false</c> otherwise.</returns>
    public bool IsActive()
    {
      return this.Status == LockRequestStatus.Pending || this.Status == LockRequestStatus.RateLocked;
    }

    /// <summary>
    /// Marks the currently pending lock request as locked, but does not create a confirmation.
    /// </summary>
    /// <remarks>Any pending changes made to the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest.Fields" /> collection will be committed
    /// when this method is called. Additionally, any calculated fields within the rate
    /// lock request will be updated.</remarks>
    /// <example>
    ///       The following code determines if the loan has a pending lock request and,
    ///       if so, populates the buy-side data for the request. It then locks
    ///       the request.
    ///       <code>
    ///         <![CDATA[
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
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open and lock the loan
    ///       Loan loan = session.Loans.Open(args[0]);
    ///       loan.Lock();
    /// 
    ///       // Retrieve the current lock request, if any, from the loan
    ///       LockRequest req = loan.Log.LockRequests.GetCurrent();
    /// 
    ///       // If the YSP is less than 1 basis point, we will deny the lock request
    ///       if (req.Status == LockRequestStatus.Pending)
    ///       {
    ///         // Set the buy-side fields for the request
    ///         req.Fields["2149"].Value = DateTime.Today;
    ///         req.Fields["2150"].Value = 45;
    /// 
    ///         // Set the buy-side base rate and adjustments
    ///         req.Fields["2152"].Value = 6.75;
    ///         req.Fields["2153"].Value = "45 Day Lock Period";
    ///         req.Fields["2154"].Value = 0.125;
    /// 
    ///         // Set the buy-side base price and adjustments
    ///         req.Fields["2161"].Value = 99.75;
    ///         req.Fields["2162"].Value = "FICO >= 720";
    ///         req.Fields["2163"].Value = 0.25;
    /// 
    ///         // Lock the request. There is no need to call the CommitChanges() method on the Fields
    ///         // collection since this will be done within the call to Lock().
    ///         req.Lock();
    ///       }
    /// 
    ///       // Save and close the loan file
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Lock()
    {
      this.EnsureValid();
      if (this.Status == LockRequestStatus.RateLocked)
        return;
      if (this.logItem.LockRequestStatus != RateLockRequestStatus.Requested)
        throw new InvalidOperationException("The current rate lock request is not active");
      this.Fields.CommitChanges();
      this.Loan.Unwrap().LockRateRequest(this.logItem, this.Fields.FieldTable, (UserInfo) null, false);
    }

    /// <summary>Confirms the lock request.</summary>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockConfirmation" /> record for the request.</returns>
    /// <remarks>This method is equivalent to calling <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest.Confirm(EllieMae.Encompass.BusinessObjects.Users.User)" /> and passing
    /// the currently logged in User as the confirmingUser.</remarks>
    /// <example>
    ///       The following code determines if the loan has a pending lock request and,
    ///       if so, populates the buy-side and sell-side data for the request. It then locks
    ///       and confirms the request.
    ///       <code>
    ///         <![CDATA[
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
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open and lock the loan
    ///       Loan loan = session.Loans.Open(args[0]);
    ///       loan.Lock();
    /// 
    ///       // Retrieve the current lock request, if any, from the loan
    ///       LockRequest req = loan.Log.LockRequests.GetCurrent();
    /// 
    ///       // If there's a pending lock request, lock and confirm the request
    ///       if (req != null && req.Status == LockRequestStatus.Pending)
    ///       {
    ///         // Set the Buy side lock date and period
    ///         req.Fields["2149"].Value = DateTime.Today;
    ///         req.Fields["2150"].Value = 45;
    /// 
    ///         // Set the buy-side base rate and adjustments
    ///         req.Fields["2152"].Value = 6.75;
    ///         req.Fields["2153"].Value = "45 Day Lock Period";
    ///         req.Fields["2154"].Value = 0.125;
    /// 
    ///         // Set the buy-side base price and adjustments
    ///         req.Fields["2161"].Value = 99.75;
    ///         req.Fields["2162"].Value = "FICO >= 720";
    ///         req.Fields["2163"].Value = 0.25;
    /// 
    ///         // Set the sell-side lock date and period
    ///         req.Fields["2220"].Value = DateTime.Today.AddDays(2);
    ///         req.Fields["2221"].Value = 60;
    /// 
    ///         // Set the sell-side base rate and adjustments
    ///         req.Fields["2223"].Value = 6.35;
    ///         req.Fields["2224"].Value = "45 Day Lock Period";
    ///         req.Fields["2225"].Value = 0.05;
    /// 
    ///         // Set the sell-side base price and adjustments
    ///         req.Fields["2232"].Value = 101.15;
    ///         req.Fields["2233"].Value = "FICO >= 720";
    ///         req.Fields["2234"].Value = 0.125;
    /// 
    ///         // Lock and confirm the rate
    ///         req.Confirm();
    ///       }
    /// 
    ///       // Save and close the loan file
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LockConfirmation Confirm() => this.Confirm((User) null);

    /// <summary>Confirms the lock request.</summary>
    /// <param name="confirmingUser">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User" /> who will be recorded as confirming the lock
    /// request. Pass <c>null</c> to indicate the currently logged in user.</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockConfirmation" /> record for the request.</returns>
    /// <remarks><p>Confirming a rate lock causes the buy-side information from the LockRequest
    /// to be copied into the loan's primary set of fields. For example, the note rate (field 3)
    /// is updated with the Net Buy Rate (2160) from the lock's <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest.Fields" /> collection.</p>
    /// <p>The the request has not already been locked, calling the Confirm method will also
    /// cause the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest.Lock" /> method to be invoked.</p>
    /// </remarks>
    /// <example>
    ///       The following code determines if the loan has a pending lock request and,
    ///       if so, populates the buy-side and sell-side data for the request. It then locks
    ///       and confirms the request.
    ///       <code>
    ///         <![CDATA[
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
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open and lock the loan
    ///       Loan loan = session.Loans.Open(args[0]);
    ///       loan.Lock();
    /// 
    ///       // Retrieve the current lock request, if any, from the loan
    ///       LockRequest req = loan.Log.LockRequests.GetCurrent();
    /// 
    ///       // If there's a pending lock request, lock and confirm the request
    ///       if (req != null && req.Status == LockRequestStatus.Pending)
    ///       {
    ///         // Set the Buy side lock date and period
    ///         req.Fields["2149"].Value = DateTime.Today;
    ///         req.Fields["2150"].Value = 45;
    /// 
    ///         // Set the buy-side base rate and adjustments
    ///         req.Fields["2152"].Value = 6.75;
    ///         req.Fields["2153"].Value = "45 Day Lock Period";
    ///         req.Fields["2154"].Value = 0.125;
    /// 
    ///         // Set the buy-side base price and adjustments
    ///         req.Fields["2161"].Value = 99.75;
    ///         req.Fields["2162"].Value = "FICO >= 720";
    ///         req.Fields["2163"].Value = 0.25;
    /// 
    ///         // Set the sell-side lock date and period
    ///         req.Fields["2220"].Value = DateTime.Today.AddDays(2);
    ///         req.Fields["2221"].Value = 60;
    /// 
    ///         // Set the sell-side base rate and adjustments
    ///         req.Fields["2223"].Value = 6.35;
    ///         req.Fields["2224"].Value = "45 Day Lock Period";
    ///         req.Fields["2225"].Value = 0.05;
    /// 
    ///         // Set the sell-side base price and adjustments
    ///         req.Fields["2232"].Value = 101.15;
    ///         req.Fields["2233"].Value = "FICO >= 720";
    ///         req.Fields["2234"].Value = 0.125;
    /// 
    ///         // Lock and confirm the rate
    ///         req.Confirm();
    ///       }
    /// 
    ///       // Save and close the loan file
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LockConfirmation Confirm(User confirmingUser)
    {
      this.EnsureValid();
      if (confirmingUser == null)
        confirmingUser = this.Loan.Session.GetCurrentUser();
      this.Fields.CommitChanges();
      this.Loan.Unwrap().LockRateRequest(this.logItem, this.Fields.FieldTable, confirmingUser.Unwrap(), true);
      this.confirmation = (LockConfirmation) null;
      return this.Confirmation;
    }

    /// <summary>Denies the lock request.</summary>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockDenial" /> record for the request.</returns>
    /// <remarks>This method is equalivant to calling the method <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest.Deny(EllieMae.Encompass.BusinessObjects.Users.User)" />
    /// and passing the currently logged in user as the denyingUser.</remarks>
    /// <example>
    ///       The following code denies the current lock request if the YSP is less than
    ///       one basis point.
    ///       <code>
    ///         <![CDATA[
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
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open and lock the loan
    ///       Loan loan = session.Loans.Open(args[0]);
    ///       loan.Lock();
    /// 
    ///       // Retrieve the current lock request, if any, from the loan
    ///       LockRequest req = loan.Log.LockRequests.GetCurrent();
    /// 
    ///       // If the YSP is less than 1 basis point, we will deny the lock request
    ///       if (!req.Fields["2277"].IsEmpty())
    ///       {
    ///         decimal ysp = req.Fields["2277"].ToDecimal();
    ///         if (ysp < 1M) req.Deny();
    ///       }
    /// 
    ///       // Save and close the loan file
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    ///     </example>
    public LockDenial Deny() => this.Deny((User) null);

    /// <summary>Denies the lock request.</summary>
    /// <param name="denyingUser">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User" /> who is denying the request. A <c>null</c>
    /// value can be passed to indicate the currently logged in user.</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockDenial" /> record for the request.</returns>
    public LockDenial Deny(User denyingUser)
    {
      this.EnsureValid();
      if (this.Status == LockRequestStatus.Denied)
        throw new InvalidOperationException("This request has already been denied");
      if (!this.IsActive())
        throw new InvalidOperationException("The current rate lock request is not active");
      if (denyingUser == null)
        denyingUser = this.Loan.Session.GetCurrentUser();
      this.Fields.CommitChanges();
      this.Loan.Unwrap().DenyRateRequest(this.logItem, this.Fields.FieldTable, denyingUser.Unwrap(), "");
      return this.Denial;
    }

    /// <summary>Creates a lock extension request</summary>
    /// <param name="daysToExtend">The number of days by which the lock is to be extended.</param>
    /// <param name="priceAdjustment">The price adjustment, if any, incurred by the extension.</param>
    /// <param name="comments">Any comments associated with the lock request extension.</param>
    /// <returns>Returns a new LockRequest object representing the extension request.</returns>
    /// <remarks>
    /// <p>A Lock request can only be extended if is is active and has been confirmed. Attempting to
    /// extend an lock which does not meet these requirements will result in an exception.</p>
    /// </remarks>
    public LockRequest Extend(int daysToExtend, Decimal priceAdjustment, string comments)
    {
      return this.Extend((User) null, daysToExtend, priceAdjustment, comments);
    }

    /// <summary>Creates a lock extension request</summary>
    /// <param name="extendingUser">The user acting as the requestor of the extension.</param>
    /// <param name="daysToExtend">The number of days by which the lock is to be extended.</param>
    /// <param name="priceAdjustment">The price adjustment, if any, incurred by the extension.</param>
    /// <param name="comments">Any comments associated with the lock request extension.</param>
    /// <returns>Returns a LockRequest object representing the extension request.</returns>
    /// <remarks>
    /// <p>A Lock request can only be extended if is is active and has been confirmed. Attempting to
    /// extend an lock which does not meet these requirements will result in an exception.</p>
    /// <p>If the <c>priceAdjutsment</c> parameter is set to zero and the system policy indicates that
    /// the price adjustment is calculated automatically, the adjustment will be calculated.</p>
    /// </remarks>
    public LockRequest Extend(
      User extendingUser,
      int daysToExtend,
      Decimal priceAdjustment,
      string comments)
    {
      LockUtils.ValidateLockExtension(this.Loan.Session.SessionObjects, this.Loan.Unwrap(), daysToExtend, priceAdjustment);
      LockExtensionUtils lockExtensionUtils = new LockExtensionUtils(this.Loan.Session.SessionObjects, this.Loan.LoanData);
      DateTime date = this.Fields["2220"].ToDate();
      if (date == DateTime.MinValue)
        date = this.Fields["2149"].ToDate();
      if (date == DateTime.MinValue)
        date = this.Fields["2089"].ToDate();
      if (date == DateTime.MinValue)
        date = this.Fields["3664"].ToDate();
      DateTime oriExpireDate = this.Fields["3358"].ToDate();
      if (oriExpireDate == DateTime.MinValue)
        oriExpireDate = this.Fields["3369"].ToDate();
      if (oriExpireDate == DateTime.MinValue)
        oriExpireDate = Convert.ToDateTime(this.ExpirationDate);
      if (!lockExtensionUtils.IsLockExtensionEnabled())
        throw new Exception("Lock Extensions are not permitted by policy.");
      int currentExtNumber = this.Loan.LoanData.GetLogList().GetCurrentExtNumber();
      if (lockExtensionUtils.IsCompanyControlled)
      {
        if (daysToExtend == 0)
          throw new Exception("Days to Extend cannot be 0");
        if (!lockExtensionUtils.HasPriceAdjustment(daysToExtend) || lockExtensionUtils.GetPriceAdjustment(daysToExtend) != priceAdjustment)
          throw new Exception("Days to Extend and Price Adjustment does not match company lock extension policies");
      }
      else if (lockExtensionUtils.IsCompanyControlledOccur)
      {
        if (daysToExtend == 0)
        {
          daysToExtend = lockExtensionUtils.GetExtensionDays(currentExtNumber + 1);
          if (daysToExtend == 0)
            throw new Exception("This loan has reached the maximum number of times allowed to be extended.");
        }
        if (priceAdjustment == 0M)
          priceAdjustment = lockExtensionUtils.GetPriceAdjustmentOccur(currentExtNumber + 1);
        if (lockExtensionUtils.GetExtensionDays(currentExtNumber + 1) != daysToExtend || lockExtensionUtils.GetPriceAdjustmentOccur(currentExtNumber + 1) != priceAdjustment)
          throw new Exception("Days to Extend and Price Adjustment does not match company lock extension policies");
      }
      else if (daysToExtend == 0)
        throw new Exception("Days to Extend cannot be 0");
      if (extendingUser == null)
        extendingUser = this.Loan.Session.GetCurrentUser();
      if (!lockExtensionUtils.IsCompanyControlledOccur && lockExtensionUtils.IfExceedNumExtensionsLimit(currentExtNumber + 1))
        throw new Exception("This loan has reached the maximum number of times allowed to be extended.");
      if (!lockExtensionUtils.IsCompanyControlledOccur && lockExtensionUtils.MaxDaysToExtend(oriExpireDate, date) < daysToExtend)
        throw new Exception("This request exceeds the maximum number of days allowed for an extension.");
      DateTime extensionExpirationDate = lockExtensionUtils.GetExtensionExpirationDate(Convert.ToDateTime(this.ExpirationDate), daysToExtend);
      int days = (Convert.ToDateTime(this.ExpirationDate) - date).Days;
      if (lockExtensionUtils.IfExceedsCapDays(date, extensionExpirationDate))
        throw new Exception("This loan has reached the maximum number of total lock days");
      if (priceAdjustment == 0M && lockExtensionUtils.IsCompanyControlled)
        priceAdjustment = lockExtensionUtils.GetPriceAdjustment(Convert.ToDateTime(this.ExpirationDate), daysToExtend);
      return (LockRequest) this.Loan.Log.Find((LogRecordBase) this.Loan.Unwrap().CreateExtendedRateLockRequest(extendingUser.Unwrap(), daysToExtend, extensionExpirationDate, priceAdjustment, comments), false);
    }
  }
}
