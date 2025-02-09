// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogLockCancellationRequests
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Users;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockCancellationRequest">LockCancellationRequests</see>
  /// held within a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLog" />.
  /// </summary>
  public class LogLockCancellationRequests : LoanLogEntryCollection, IEnumerable
  {
    internal LogLockCancellationRequests(Loan loan)
      : base(loan, typeof (LockRequestLog))
    {
    }

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockCancellationRequest">LockCancellationRequest</see>
    /// from the collection based on its index.
    /// </summary>
    public LockCancellationRequest this[int index]
    {
      get => (LockCancellationRequest) this.LogEntries[index];
    }

    /// <summary>
    /// Creates a new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockCancellationRequest" /> and adds it to the log.
    /// </summary>
    /// <returns>Returns the newly added <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockCancellationRequest">LockCancellationRequest</see>
    /// object.</returns>
    /// <remarks>When you create a new LockCancellationRequest, any existing, current lock request will have
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
    public LockCancellationRequest Add() => this.Add((User) null, "");

    /// <summary>
    /// Creates a new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockCancellationRequest" /> and add it to the log.
    /// </summary>
    /// <param name="requestingUser">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User" /> who is issuing the request for the
    /// lock cancellation. If this value is <c>null</c>, the currently logged in user is assumed.</param>
    /// <returns>Returns the newly added <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockCancellationRequest">LockCancellationRequest</see>
    /// object.</returns>
    /// <param name="comment"></param>
    /// <remarks><p>When you create a new LockCancellationRequest, any existing, current lock request will have
    /// its status automatically changed to <see cref="F:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequestStatus.Inactive" />.</p>
    /// <p>This method is supported in Banker Edition only.</p>
    /// </remarks>
    public LockCancellationRequest Add(User requestingUser, string comment)
    {
      if (!this.Loan.Session.SessionObjects.ServerLicense.IsBankerEdition)
        throw new NotSupportedException("The specified operation is not supported by the current version of Encompass");
      if (requestingUser == null)
        requestingUser = this.Loan.Session.GetCurrentUser();
      return this.Find((LogRecordBase) (this.Loan.Unwrap().CreateRateLockCancellationRequest(requestingUser.Unwrap(), DateTime.Now, comment) ?? throw new InvalidOperationException("The cancellation request could not be created. This can happen if the loan does not have a currently active lock.")), true) as LockCancellationRequest;
    }

    /// <summary>
    /// Returns the currently active lock cancellation request.
    /// </summary>
    public LockCancellationRequest GetCurrent()
    {
      foreach (LockCancellationRequest logEntry in (CollectionBase) this.LogEntries)
      {
        if (logEntry.IsActive())
          return logEntry;
      }
      return (LockCancellationRequest) null;
    }

    /// <summary>Wraps a LogRecord in a LogEntry object.</summary>
    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new LockCancellationRequest(this.Loan, (LockRequestLog) logRecord);
    }

    internal override bool IsRecordOfType(LogRecordBase logRecord)
    {
      return base.IsRecordOfType(logRecord) && ((LockRequestLog) logRecord).IsLockCancellation;
    }
  }
}
