// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.EDMTransaction
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class EDMTransaction : LogEntry, IEDMTransaction
  {
    private Loan loan;
    private EDMLog logItem;
    private EDMDocuments docs;

    internal EDMTransaction(Loan loan, EDMLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.loan = loan;
      this.logItem = logItem;
      this.docs = new EDMDocuments(logItem.Documents);
    }

    public override LogEntryType EntryType => LogEntryType.EDMTransaction;

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

    public EDMDocuments Documents
    {
      get
      {
        this.EnsureValid();
        return this.docs;
      }
    }
  }
}
