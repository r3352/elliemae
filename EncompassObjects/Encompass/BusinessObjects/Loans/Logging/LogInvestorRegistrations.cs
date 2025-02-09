// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogInvestorRegistrations
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.InvestorRegistration" /> entries held within
  /// a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LoanLog" />.
  /// </summary>
  /// <example>
  /// The following code creates a new Conversation in a loan and flags it for
  /// followup by the Loan Officer.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
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
  ///       // Open a loan and lock it for writing
  ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
  ///       loan.Lock();
  /// 
  ///       // Add a new conversation event to the log
  ///       Conversation conv = loan.Log.Conversations.Add(DateTime.Now);
  ///       conv.HeldWith = "Thomas Smith";
  ///       conv.PhoneNumber = "555-555-5555";
  ///       conv.ContactMethod = ConversationContactMethod.Phone;
  /// 
  ///       // Save the loan
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
  public class LogInvestorRegistrations : 
    LoanLogEntryCollection,
    ILogInvestorRegistrations,
    IEnumerable
  {
    internal LogInvestorRegistrations(Loan loan)
      : base(loan, typeof (RegistrationLog))
    {
    }

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.InvestorRegistration">InvestorRegistration</see>
    /// from the collection based on its index.
    /// </summary>
    public InvestorRegistration this[int index] => (InvestorRegistration) this.LogEntries[index];

    /// <summary>Adds a InvestorRegistration to the loan's log.</summary>
    /// <returns>Returns the newly added <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.InvestorRegistration" />
    /// object.</returns>
    public InvestorRegistration Add(DateTime registrationDate)
    {
      return (InvestorRegistration) this.CreateEntry((LogRecordBase) new RegistrationLog()
      {
        RegisteredByID = this.Loan.Session.UserID,
        RegisteredByName = this.Loan.Session.GetCurrentUser().FullName,
        RegisteredDate = registrationDate
      });
    }

    /// <summary>
    /// Gets the current investor registration from the collection.
    /// </summary>
    /// <returns>Returns the current <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.InvestorRegistration" />. If no current registration
    /// exists, a <c>null</c> is returned.</returns>
    /// <remarks>The current registration is defined to be the registration added most recently to
    /// the loan. The registration may be expired, but is still considered current.</remarks>
    /// <include file="LogConversations.xml" path="Examples/Example[@name=&quot;LogConversations.GetCurrent&quot;]/*" />
    public InvestorRegistration GetCurrent()
    {
      foreach (InvestorRegistration current in (LoanLogEntryCollection) this)
      {
        if (current.Current)
          return current;
      }
      return (InvestorRegistration) null;
    }

    /// <summary>Wraps a LogRecord in a LogEntry object.</summary>
    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new InvestorRegistration(this.Loan, logRecord);
    }
  }
}
