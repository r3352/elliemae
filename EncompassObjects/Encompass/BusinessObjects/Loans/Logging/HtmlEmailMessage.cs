// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.HtmlEmailMessage
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
  /// Represents an html email message that is sent from the client
  /// of Encompass.
  /// </summary>
  public class HtmlEmailMessage : LogEntry, IHtmlEmailMessage
  {
    private Loan loan;
    private HtmlEmailLog logItem;

    internal HtmlEmailMessage(Loan loan, HtmlEmailLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.loan = loan;
      this.logItem = logItem;
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType" /> for the current entry.
    /// </summary>
    /// <remarks>This property will always return the value
    /// <see cref="F:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType.HtmlEmailMessage" />.</remarks>
    public override LogEntryType EntryType => LogEntryType.HtmlEmailMessage;

    /// <summary>
    /// Gets the date on which the Html email message was created.
    /// </summary>
    public DateTime Date => Convert.ToDateTime(base.Date);

    /// <summary>Gets the description of the EDM Transaction.</summary>
    public string Description
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Description;
      }
    }

    /// <summary>
    /// Gets the name of the user who initiated this transaction.
    /// </summary>
    public string Creator
    {
      get
      {
        this.EnsureValid();
        return this.logItem.CreatedBy;
      }
    }

    /// <summary>
    /// Gets the email address of the sender of the html message.
    /// </summary>
    public string Sender
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Sender;
      }
    }

    /// <summary>
    /// Gets the email addresses of the recipients of the email message.
    /// </summary>
    public string Recipient
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Recipient;
      }
    }

    /// <summary>Gets the subject of the html email message.</summary>
    public string Subject
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Subject;
      }
    }

    /// <summary>Gets the body of the html email message.</summary>
    public string Body
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Body;
      }
    }
  }
}
