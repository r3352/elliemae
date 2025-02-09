// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.OrderedDocumentList
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class OrderedDocumentList : IEnumerable<OrderedDocument>, IEnumerable
  {
    private DocumentOrderLog orderLog;
    private List<OrderedDocument> documents = new List<OrderedDocument>();

    internal OrderedDocumentList(DocumentOrderLog orderLog) => this.orderLog = orderLog;

    internal OrderedDocumentList(DocumentOrderLog orderLog, XmlElement xml)
    {
      this.orderLog = orderLog;
      foreach (XmlElement selectNode in xml.SelectNodes("Document"))
        this.documents.Add(new OrderedDocument(selectNode));
    }

    public int Count => this.documents.Count;

    public void Add(OrderedDocument doc)
    {
      this.documents.Add(doc);
      this.orderLog.MarkAsDirty();
    }

    public void Remove(OrderedDocument doc)
    {
      this.documents.Remove(doc);
      this.orderLog.MarkAsDirty();
    }

    public void Clear()
    {
      this.documents.Clear();
      this.orderLog.MarkAsDirty();
    }

    public OrderedDocument this[int index] => this.documents[index];

    public bool Contains(string title) => this.GetDocumentByTitle(title) != null;

    public OrderedDocument GetDocumentByTitle(string title)
    {
      foreach (OrderedDocument documentByTitle in this)
      {
        if (string.Compare(documentByTitle.Title, title, true) == 0)
          return documentByTitle;
      }
      return (OrderedDocument) null;
    }

    public OrderedDocument GetDocumentByGuid(string guid)
    {
      foreach (OrderedDocument documentByGuid in this)
      {
        if (string.Compare(documentByGuid.Guid, guid, true) == 0)
          return documentByGuid;
      }
      return (OrderedDocument) null;
    }

    internal void ToXml(XmlElement xml)
    {
      foreach (OrderedDocument orderedDocument in this)
        orderedDocument.ToXml((XmlElement) xml.AppendChild((XmlNode) xml.OwnerDocument.CreateElement("Document")));
    }

    public IEnumerator<OrderedDocument> GetEnumerator()
    {
      return (IEnumerator<OrderedDocument>) this.documents.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
