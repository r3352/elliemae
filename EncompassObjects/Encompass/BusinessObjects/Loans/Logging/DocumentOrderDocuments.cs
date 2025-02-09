// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.DocumentOrderDocuments
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Provides the interface for retrieving the documents associated with a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.DocumentOrder" />.
  /// </summary>
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

    /// <summary>Gets the number of documents in the order.</summary>
    public int Count => this.documents.Count;

    /// <summary>Gets a document by index</summary>
    /// <param name="index">This index of the document to be returned.</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.OrderedDocument" /> specified by the index.</returns>
    public OrderedDocument this[int index] => this.toOrderedDocument(this.documents[index]);

    /// <summary>Retrieves a document by its unique identifier.</summary>
    /// <param name="docGuid">The GUID of the document to be retrieved.</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.OrderedDocument" /> with the matching GUID, or <c>null</c>
    /// if no matching document is found.</returns>
    public OrderedDocument GetDocumentByGuid(string docGuid)
    {
      return this.orderedDocs.ContainsKey(docGuid) ? this.orderedDocs[docGuid] : this.toOrderedDocument(this.documents.GetDocumentByGuid(docGuid));
    }

    /// <summary>Provides an enumerator for the collection</summary>
    /// <returns>Returns an enumerator for iterating over the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.OrderedDocument" /> objects
    /// in the collection.</returns>
    public IEnumerator GetEnumerator()
    {
      List<OrderedDocument> orderedDocumentList = new List<OrderedDocument>();
      foreach (EllieMae.EMLite.DataEngine.Log.OrderedDocument document in this.documents)
        orderedDocumentList.Add(this.toOrderedDocument(document));
      return (IEnumerator) orderedDocumentList.GetEnumerator();
    }

    /// <summary>
    /// Converts an OrderedDocument in the low-level DataEngine to an OrderedDocument in the API
    /// </summary>
    private OrderedDocument toOrderedDocument(EllieMae.EMLite.DataEngine.Log.OrderedDocument doc)
    {
      if (doc == null)
        return (OrderedDocument) null;
      if (!this.orderedDocs.ContainsKey(doc.Guid))
        this.orderedDocs[doc.Guid] = new OrderedDocument(this.order, doc);
      return this.orderedDocs[doc.Guid];
    }
  }
}
