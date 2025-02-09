// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogConversations
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
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.Conversation" /> entries held within
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
  public class LogConversations : LoanLogEntryCollection, ILogConversations, IEnumerable
  {
    internal LogConversations(Loan loan)
      : base(loan, typeof (ConversationLog))
    {
    }

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.Conversation">Conversation</see>
    /// from the collection based on its index.
    /// </summary>
    public Conversation this[int index] => (Conversation) this.LogEntries[index];

    /// <summary>Adds a Conversation to the loan's log.</summary>
    /// <param name="conversationDate">The date of the conversation.</param>
    /// <returns>Returns the newly added <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.Conversation">Conversation</see>
    /// object.</returns>
    /// <remarks>This method assumes that the user who held the conversation is the
    /// same as the currently logged in user. To create an entry for a different
    /// user, use the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogConversations.AddForUser(System.DateTime,EllieMae.Encompass.BusinessObjects.Users.User)" /> method.</remarks>
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
    public Conversation Add(DateTime conversationDate)
    {
      return (Conversation) this.CreateEntry((LogRecordBase) new ConversationLog(conversationDate, this.Loan.Session.UserID));
    }

    /// <summary>Adds a Conversation to the loan's log.</summary>
    /// <param name="conversationDate">The date of the conversation.</param>
    /// <param name="heldBy">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User" /> who held this conversation. This
    /// value must match the currently logged in user unless the logged in user is
    /// an administrator.</param>
    /// <returns>Returns the newly added <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.Conversation">Conversation</see>
    /// object.</returns>
    /// <example>
    /// The following code creates a new Conversation in a loan.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server -- we must log in as an
    ///       // administrator in order to invoke AddForUser().
    ///       Session session = new Session();
    ///       session.Start("myserver", "admin", "adminpwd");
    /// 
    ///       // Open a loan and lock it for writing
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    ///       loan.Lock();
    /// 
    ///       // Fetch the user we'll be adding the conversation for
    ///       User mary = session.Users.GetUser("mary");
    /// 
    ///       // Add a new conversation event to the log
    ///       Conversation conv = loan.Log.Conversations.AddForUser(DateTime.Now, mary);
    ///       conv.HeldWith = "James Hartley";
    ///       conv.Company = "Harley Appraisal Services";
    ///       conv.EmailAddress = "jhartl@hartleyappraisals.com";
    ///       conv.ContactMethod = ConversationContactMethod.Email;
    ///       conv.Comments = "Mary contacted James and scheduled the appraisal for 6/16";
    ///       conv.DisplayInLog = false;
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
    public Conversation AddForUser(DateTime conversationDate, User heldBy)
    {
      if (heldBy == null)
        throw new ArgumentNullException("milestone");
      if (heldBy.ID != this.Loan.Session.UserID && !this.Loan.Session.Unwrap().GetUser().GetUserInfo().IsAdministrator())
        throw new InvalidOperationException("User does not have sufficient rights to perform this operation.");
      return (Conversation) this.CreateEntry((LogRecordBase) new ConversationLog(conversationDate, heldBy.ID));
    }

    /// <summary>
    /// Removes a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.Conversation" /> from the log.
    /// </summary>
    /// <param name="conversation">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.Conversation" /> to be removed.
    /// The specified entry must be an instance that belongs to the
    /// current Loan object.</param>
    public void Remove(Conversation conversation) => this.RemoveEntry((LogEntry) conversation);

    /// <summary>Wraps a LogRecord in a LogEntry object.</summary>
    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new Conversation(this.Loan, (ConversationLog) logRecord);
    }
  }
}
