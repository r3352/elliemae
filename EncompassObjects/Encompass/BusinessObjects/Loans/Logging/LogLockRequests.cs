// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogLockRequests
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.LoanUtils.RateLocks;
using EllieMae.Encompass.BusinessObjects.Users;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest">LockRequests</see>
  /// held within a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLog" />.
  /// </summary>
  /// <example>
  ///       The following code creates a new Lock Request on an existing loan.
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
  ///       // Populate the lock request data in the loan. For many of these fields, we can simply
  ///       // copy the data from the basic loan information since we want to use that same information
  ///       // for the lock request. However, it's possible to pass information to the lock request
  ///       // which differs from the actual values in the loan.
  ///       loan.Fields["2951"].Value = loan.Fields["19"].Value;
  ///       loan.Fields["2952"].Value = loan.Fields["1172"].Value;
  ///       loan.Fields["2953"].Value = loan.Fields["608"].Value;
  ///       loan.Fields["2958"].Value = loan.Fields["420"].Value;
  /// 
  ///       // Populate the requested lock date and lock period
  ///       loan.Fields["2089"].Value = DateTime.Today;
  ///       loan.Fields["2090"].Value = 30;
  /// 
  ///       // Now create the new LockRequest. This is equivalent to pressing the Request Lock button
  ///       // on the Lock Request Form.
  ///       LockRequest request = loan.Log.LockRequests.Add();
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
  public class LogLockRequests : LoanLogEntryCollection, ILogLockRequests, IEnumerable
  {
    internal LogLockRequests(Loan loan)
      : base(loan, typeof (LockRequestLog))
    {
    }

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest">LockRequest</see>
    /// from the collection based on its index.
    /// </summary>
    public LockRequest this[int index] => (LockRequest) this.LogEntries[index];

    /// <summary>
    /// Creates a new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest" /> and adds it to the log.
    /// </summary>
    /// <returns>Returns the newly added <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest">LockRequest</see>
    /// object.</returns>
    /// <remarks>When you create a new LockRequest, any existing, current lock request will have
    /// its status automatically changed to <see cref="F:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequestStatus.Inactive" /></remarks>
    /// <example>
    ///       The following code creates a new Lock Request on an existing loan.
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
    ///       // Populate the lock request data in the loan. For many of these fields, we can simply
    ///       // copy the data from the basic loan information since we want to use that same information
    ///       // for the lock request. However, it's possible to pass information to the lock request
    ///       // which differs from the actual values in the loan.
    ///       loan.Fields["2951"].Value = loan.Fields["19"].Value;
    ///       loan.Fields["2952"].Value = loan.Fields["1172"].Value;
    ///       loan.Fields["2953"].Value = loan.Fields["608"].Value;
    ///       loan.Fields["2958"].Value = loan.Fields["420"].Value;
    /// 
    ///       // Populate the requested lock date and lock period
    ///       loan.Fields["2089"].Value = DateTime.Today;
    ///       loan.Fields["2090"].Value = 30;
    /// 
    ///       // Now create the new LockRequest. This is equivalent to pressing the Request Lock button
    ///       // on the Lock Request Form.
    ///       LockRequest request = loan.Log.LockRequests.Add();
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
    public LockRequest Add() => this.Add((User) null);

    /// <summary>
    /// Creates a new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest" /> and add it to the log.
    /// </summary>
    /// <param name="requestingUser">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User" /> who is issuing the request for the
    /// lock. If this value is <c>null</c>, the currently logged in user is assumed.</param>
    /// <returns>Returns the newly added <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest">LockRequest</see>
    /// object.</returns>
    /// <remarks><p>When you create a new LockRequest, any existing, current lock request will have
    /// its status automatically changed to <see cref="F:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequestStatus.Inactive" />.</p>
    /// <p>This method is supported in Banker Edition only.</p>
    /// </remarks>
    /// <example>
    ///       The following code creates a new Lock Request on an existing loan.
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
    ///       // Populate the lock request data in the loan. For many of these fields, we can simply
    ///       // copy the data from the basic loan information since we want to use that same information
    ///       // for the lock request. However, it's possible to pass information to the lock request
    ///       // which differs from the actual values in the loan.
    ///       loan.Fields["2951"].Value = loan.Fields["19"].Value;
    ///       loan.Fields["2952"].Value = loan.Fields["1172"].Value;
    ///       loan.Fields["2953"].Value = loan.Fields["608"].Value;
    ///       loan.Fields["2958"].Value = loan.Fields["420"].Value;
    /// 
    ///       // Populate the requested lock date and lock period
    ///       loan.Fields["2089"].Value = DateTime.Today;
    ///       loan.Fields["2090"].Value = 30;
    /// 
    ///       // Now create the new LockRequest. This is equivalent to pressing the Request Lock button
    ///       // on the Lock Request Form.
    ///       LockRequest request = loan.Log.LockRequests.Add();
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
    public LockRequest Add(User requestingUser)
    {
      if (!this.Loan.Session.SessionObjects.ServerLicense.IsBankerEdition)
        throw new NotSupportedException("The specified operation is not supported by the current version of Encompass");
      if (requestingUser == null)
        requestingUser = this.Loan.Session.GetCurrentUser();
      LockRequestLog rateLockRequest;
      try
      {
        rateLockRequest = this.Loan.Unwrap().CreateRateLockRequest(requestingUser.Unwrap(), false);
      }
      catch (RateLockRejectedException ex)
      {
        throw new RejectedRateLockException(ex.Message, (Exception) ex);
      }
      return this.Find((LogRecordBase) rateLockRequest, true) as LockRequest;
    }

    /// <summary>
    /// Returns the currently active or confirmed lock request.
    /// </summary>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest" /> which is pending or locked.</returns>
    /// <example>
    ///       The following code determines if the loan has a pending lock request and,
    ///       if so, populates the buy-side and side-data for the request. It the locks
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
    ///         // Commit the changes the to the fields
    ///         req.Fields.CommitChanges();
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
    public LockRequest GetCurrent()
    {
      foreach (LockRequest logEntry in (CollectionBase) this.LogEntries)
      {
        if (logEntry.IsActive())
          return logEntry;
      }
      return (LockRequest) null;
    }

    /// <summary>
    /// Returns the most recently confirmed <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest" /> object.
    /// </summary>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest" /> object if one if found, <c>null</c> otherwise.</returns>
    /// <remarks>
    /// This method will always return the LockRequest object which was confirmed most recently,
    /// even if that request is no longer active. A <c>null</c> return value indicates that there
    /// are no confirmed lock requests in the loan.
    /// </remarks>
    public LockRequest GetLastConfirmedLockRequest()
    {
      DateTime dateTime = DateTime.MinValue;
      LockRequest confirmedLockRequest = (LockRequest) null;
      foreach (LockRequest logEntry in (CollectionBase) this.LogEntries)
      {
        LockConfirmation confirmation = logEntry.Confirmation;
        if (confirmation != null && confirmation.Date > dateTime)
        {
          dateTime = confirmation.Date;
          confirmedLockRequest = logEntry;
        }
      }
      return confirmedLockRequest;
    }

    /// <summary>Wraps a LogRecord in a LogEntry object.</summary>
    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new LockRequest(this.Loan, (LockRequestLog) logRecord);
    }

    internal override bool IsRecordOfType(LogRecordBase logRecord)
    {
      return base.IsRecordOfType(logRecord) && !((LockRequestLog) logRecord).IsLockCancellation;
    }
  }
}
