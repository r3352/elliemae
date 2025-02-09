// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.PrintEvent
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class PrintEvent : LogEntry, IPrintEvent
  {
    private Loan loan;
    private PrintLog logItem;
    private PrintDocuments docs;

    internal PrintEvent(Loan loan, PrintLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.loan = loan;
      this.logItem = logItem;
      this.docs = new PrintDocuments((IEnumerable) logItem.ItemList);
    }

    public override LogEntryType EntryType => LogEntryType.PrintEvent;

    public string PerformedBy
    {
      get
      {
        this.EnsureValid();
        return this.logItem.PrintedBy;
      }
      set
      {
        this.EnsureEditable();
        this.logItem.PrintedBy = value ?? "";
      }
    }

    public string PerformedByUserName
    {
      get
      {
        this.EnsureValid();
        return this.logItem.PrintedByFullName;
      }
      set
      {
        this.EnsureEditable();
        this.logItem.PrintedByFullName = value ?? "";
      }
    }

    public PrintAction PrintAction
    {
      get
      {
        this.EnsureValid();
        return (PrintAction) this.logItem.Action;
      }
      set => this.logItem.Action = (PrintLog.PrintAction) value;
    }

    public PrintDocuments Documents
    {
      get
      {
        this.EnsureValid();
        return this.docs;
      }
    }
  }
}
