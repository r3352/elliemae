// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogConversations
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Users;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LogConversations : LoanLogEntryCollection, ILogConversations, IEnumerable
  {
    internal LogConversations(Loan loan)
      : base(loan, typeof (ConversationLog))
    {
    }

    public Conversation this[int index] => (Conversation) this.LogEntries[index];

    public Conversation Add(DateTime conversationDate)
    {
      return (Conversation) this.CreateEntry((LogRecordBase) new ConversationLog(conversationDate, this.Loan.Session.UserID));
    }

    public Conversation AddForUser(DateTime conversationDate, User heldBy)
    {
      if (heldBy == null)
        throw new ArgumentNullException("milestone");
      if (heldBy.ID != this.Loan.Session.UserID && !this.Loan.Session.Unwrap().GetUser().GetUserInfo().IsAdministrator())
        throw new InvalidOperationException("User does not have sufficient rights to perform this operation.");
      return (Conversation) this.CreateEntry((LogRecordBase) new ConversationLog(conversationDate, heldBy.ID));
    }

    public void Remove(Conversation conversation) => this.RemoveEntry((LogEntry) conversation);

    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new Conversation(this.Loan, (ConversationLog) logRecord);
    }
  }
}
