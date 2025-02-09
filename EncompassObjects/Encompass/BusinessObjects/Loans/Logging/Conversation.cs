// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.Conversation
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents a single conversation associated with a Loan.
  /// </summary>
  /// <remarks>The inherited Date property of a Conversation represents the
  /// date on which the conversation occurred.
  /// <p>Conversation instances become invalid
  /// when the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.Refresh">Refresh</see> method is
  /// invoked on the parent <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan">Loan</see> object. Attempting
  /// to access this object after invoking Refresh() will result in an
  /// exception.</p>
  /// </remarks>
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
  /// 
  /// class LoanReader
  /// {
  ///    public static void Main()
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("tcp://myserver:11091/", "mary", "maryspwd");
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
  public class Conversation : LogEntry, IConversation
  {
    private Loan loan;
    private ConversationLog convItem;

    internal Conversation(Loan loan, ConversationLog convItem)
      : base(loan, (LogRecordBase) convItem)
    {
      this.loan = loan;
      this.convItem = convItem;
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType" /> for the current entry.
    /// </summary>
    /// <remarks>This property will always return the value
    /// <see cref="F:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType.Conversation" />.</remarks>
    public override LogEntryType EntryType => LogEntryType.Conversation;

    /// <summary>
    /// Gets the name of the person with whom the conversation was held.
    /// </summary>
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
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("tcp://myserver:11091/", "mary", "maryspwd");
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
    public string HeldWith
    {
      get
      {
        this.EnsureValid();
        return this.convItem.Name;
      }
      set
      {
        this.EnsureEditable();
        this.convItem.Name = value ?? "";
      }
    }

    /// <summary>
    /// Gets or sets the name of the company at which the individual works with whom the
    /// conversation was held.
    /// </summary>
    public string Company
    {
      get
      {
        this.EnsureValid();
        return this.convItem.Company;
      }
      set
      {
        this.EnsureEditable();
        this.convItem.Company = value ?? "";
      }
    }

    /// <summary>
    /// Gets or sets the method of contact used for the conversation.
    /// </summary>
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
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("tcp://myserver:11091/", "mary", "maryspwd");
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
    public ConversationContactMethod ContactMethod
    {
      get
      {
        this.EnsureValid();
        return !this.convItem.IsEmail ? ConversationContactMethod.Phone : ConversationContactMethod.Email;
      }
      set
      {
        this.EnsureEditable();
        this.convItem.IsEmail = value == ConversationContactMethod.Email;
      }
    }

    /// <summary>Gets or sets the phone number called.</summary>
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
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("tcp://myserver:11091/", "mary", "maryspwd");
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
    public string PhoneNumber
    {
      get
      {
        this.EnsureValid();
        return this.convItem.Phone;
      }
      set
      {
        this.EnsureEditable();
        this.convItem.Phone = value ?? "";
      }
    }

    /// <summary>
    /// Gets or sets the e-mail addressed to which a message was sent.
    /// </summary>
    /// <example>
    /// The following code identifies all conversations within a loan.
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
    ///       session.Start("tcp://myserver:11091/", "mary", "maryspwd");
    /// 
    ///       // Open a loan and lock it for writing
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    /// 
    ///       // Add a new conversation event to the log
    ///       foreach (Conversation conv in loan.Log.Conversations)
    ///       {
    ///          // Check if an alert is specified
    ///          Console.WriteLine("Held With:  " + conv.HeldWith);
    ///          Console.WriteLine("Held By:    " + conv.HeldBy);
    ///          Console.WriteLine("Method:     " + conv.ContactMethod);
    ///          Console.WriteLine("Phone:      " + conv.PhoneNumber);
    ///          Console.WriteLine("E-mail:     " + conv.EmailAddress);
    ///       }
    /// 
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public string EmailAddress
    {
      get
      {
        this.EnsureValid();
        return this.convItem.Email;
      }
      set
      {
        this.EnsureEditable();
        this.convItem.Email = value ?? "";
      }
    }

    /// <summary>
    /// Gets or sets a flag indicating if this conversation should be displayed in the
    /// Log pane of the Encompass application for the current loan.
    /// </summary>
    public bool DisplayInLog
    {
      get
      {
        this.EnsureValid();
        return this.convItem.DisplayInLog;
      }
      set
      {
        this.EnsureEditable();
        this.convItem.DisplayInLog = value;
      }
    }

    /// <summary>
    /// Gets or sets the user's new comments since the loan was last saved.
    /// </summary>
    public string NewComments
    {
      get
      {
        this.EnsureValid();
        return this.convItem.NewComments;
      }
      set
      {
        this.EnsureEditable();
        this.convItem.NewComments = value ?? "";
      }
    }

    /// <summary>
    /// Gets the user ID of the user who created this conversation log entry.
    /// </summary>
    /// <example>
    /// The following code identifies all conversations within a loan.
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
    ///       session.Start("tcp://myserver:11091/", "mary", "maryspwd");
    /// 
    ///       // Open a loan and lock it for writing
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    /// 
    ///       // Add a new conversation event to the log
    ///       foreach (Conversation conv in loan.Log.Conversations)
    ///       {
    ///          // Check if an alert is specified
    ///          Console.WriteLine("Held With:  " + conv.HeldWith);
    ///          Console.WriteLine("Held By:    " + conv.HeldBy);
    ///          Console.WriteLine("Method:     " + conv.ContactMethod);
    ///          Console.WriteLine("Phone:      " + conv.PhoneNumber);
    ///          Console.WriteLine("E-mail:     " + conv.EmailAddress);
    ///       }
    /// 
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public string HeldBy
    {
      get
      {
        this.EnsureValid();
        return this.convItem.UserId;
      }
    }

    internal override void Dispose()
    {
      this.loan = (Loan) null;
      this.convItem = (ConversationLog) null;
    }
  }
}
