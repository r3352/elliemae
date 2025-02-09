// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.DocumentOrderDocuments
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class DocumentOrderDocuments : IEnumerable
  {
    private DocumentOrder order;
    private OrderedDocumentList documents;
    private Dictionary<string, OrderedDocument> orderedDocs = new Dictionary<string, OrderedDocument>();

    internal DocumentOrderDocuments(DocumentOrder order)
    {
      this.order = order;
      this.documents = order.OrderLog.Documents;
    }

    public int Count => this.documents.Count;

    public OrderedDocument this[int index] => this.toOrderedDocument(this.documents[index]);

    public OrderedDocument GetDocumentByGuid(string docGuid)
    {
      return this.orderedDocs.ContainsKey(docGuid) ? this.orderedDocs[docGuid] : this.toOrderedDocument(this.documents.GetDocumentByGuid(docGuid));
    }

    public IEnumerator GetEnumerator()
    {
      List<OrderedDocument> orderedDocumentList = new List<OrderedDocument>();
      foreach (OrderedDocument document in this.documents)
        orderedDocumentList.Add(this.toOrderedDocument(document));
      return (IEnumerator) orderedDocumentList.GetEnumerator();
    }

    private OrderedDocument toOrderedDocument(OrderedDocument doc)
    {
      if (doc == null)
        return (OrderedDocument) null;
      if (!this.orderedDocs.ContainsKey(doc.Guid))
        this.orderedDocs[doc.Guid] = new OrderedDocument(this.order, doc);
      return this.orderedDocs[doc.Guid];
    }
  }
}
