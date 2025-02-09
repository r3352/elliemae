// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.DocumentOrder
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class DocumentOrder : LogEntry
  {
    private DocumentOrderLog orderLog;
    private DocumentOrderDocuments documents;
    private DocumentOrderFields fields;

    internal DocumentOrder(Loan loan, DocumentOrderLog orderLog)
      : base(loan, (LogRecordBase) orderLog)
    {
      this.orderLog = orderLog;
      this.documents = new DocumentOrderDocuments(this);
      this.fields = new DocumentOrderFields(this);
    }

    public override LogEntryType EntryType => LogEntryType.DocumentOrder;

    public DocumentOrderType OrderType => (DocumentOrderType) this.orderLog.OrderType;

    public string OrderedBy => this.orderLog.OrderedByUserID;

    public bool DocumentsAvailable => this.orderLog.DateFilesPurged == DateTime.MinValue;

    public object DateDocumentsPurged
    {
      get
      {
        return !(this.orderLog.DateFilesPurged == DateTime.MinValue) ? (object) this.orderLog.DateFilesPurged : (object) null;
      }
    }

    public DocumentOrderDocuments Documents => this.documents;

    public DocumentOrderFields Fields => this.fields;

    internal DocumentOrderLog OrderLog => this.orderLog;
  }
}
