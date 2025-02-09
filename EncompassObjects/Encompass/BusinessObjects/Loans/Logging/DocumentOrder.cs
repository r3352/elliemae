// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.DocumentOrder
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
  /// The DocumentOrder represents a transaction with the Encompass360 Document Service.
  /// </summary>
  /// <remarks>This order log can represent either a request for Opening or for Closing documents.
  /// The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.DocumentOrder.OrderType" /> property indicates which type of transaction occurred.</remarks>
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

    /// <summary>Gets the type of entry for this log item.</summary>
    public override LogEntryType EntryType => LogEntryType.DocumentOrder;

    /// <summary>
    /// Gets the type of document order represented by this log
    /// </summary>
    public DocumentOrderType OrderType => (DocumentOrderType) this.orderLog.OrderType;

    /// <summary>
    /// Returns the User ID of the user who placed the document order.
    /// </summary>
    public string OrderedBy => this.orderLog.OrderedByUserID;

    /// <summary>
    /// Indicates if the documents associated with the order are still available
    /// </summary>
    /// <remarks>The availability of the documents is determined by the company's document retention
    /// policy.</remarks>
    public bool DocumentsAvailable => this.orderLog.DateFilesPurged == DateTime.MinValue;

    /// <summary>
    /// Gets the date the Documents were purged from the Encompass system.
    /// </summary>
    /// <remarks>If the documents have been purged based on the company's retention policy, this property
    /// will return the date that occurred. If the documents have not been purged, this will return <c>null</c>.
    /// </remarks>
    public object DateDocumentsPurged
    {
      get
      {
        return !(this.orderLog.DateFilesPurged == DateTime.MinValue) ? (object) this.orderLog.DateFilesPurged : (object) null;
      }
    }

    /// <summary>
    /// Gets the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.OrderedDocument" /> objects associated with this order.
    /// </summary>
    public DocumentOrderDocuments Documents => this.documents;

    /// <summary>
    /// Gets the collection of field values associated with this order.
    /// </summary>
    /// <remarks>This field collection is read-only.</remarks>
    public DocumentOrderFields Fields => this.fields;

    /// <summary>Gets the DocumentOrderLog for this document</summary>
    internal DocumentOrderLog OrderLog => this.orderLog;
  }
}
