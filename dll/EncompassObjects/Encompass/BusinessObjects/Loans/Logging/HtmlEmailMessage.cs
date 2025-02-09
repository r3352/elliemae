// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.HtmlEmailMessage
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
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

    public override LogEntryType EntryType => LogEntryType.HtmlEmailMessage;

    public DateTime Date => Convert.ToDateTime(base.Date);

    public string Description
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Description;
      }
    }

    public string Creator
    {
      get
      {
        this.EnsureValid();
        return this.logItem.CreatedBy;
      }
    }

    public string Sender
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Sender;
      }
    }

    public string Recipient
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Recipient;
      }
    }

    public string Subject
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Subject;
      }
    }

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
