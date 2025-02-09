// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.Conversation
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
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

    public override LogEntryType EntryType => LogEntryType.Conversation;

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

    public bool DisplayInLog
    {
      get
      {
        this.EnsureValid();
        return ((LogRecordBase) this.convItem).DisplayInLog;
      }
      set
      {
        this.EnsureEditable();
        ((LogRecordBase) this.convItem).DisplayInLog = value;
      }
    }

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
