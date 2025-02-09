// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.OrderedDocument
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class OrderedDocument
  {
    private string guid;
    private string title;
    private string signatureType;
    private string pairID;
    private string dataKey;
    private long size;
    private string docType;
    private string docCategory;
    private string templateGuid;
    private string docSvcDocumentID;
    private string overflowDataKey;
    private List<OrderedDocumentOverflow> overflows;

    public OrderedDocument(
      string guid,
      string title,
      string docType,
      string docCategory,
      string signatureType,
      string pairID,
      string dataKey,
      long size,
      string docSvcDocumentID,
      string overflowDataKey,
      List<OrderedDocumentOverflow> overflows)
      : this(guid, title, docType, docCategory, signatureType, pairID, dataKey, size, (string) null)
    {
      this.docSvcDocumentID = docSvcDocumentID;
      this.overflowDataKey = overflowDataKey;
      this.overflows = overflows;
    }

    public OrderedDocument(
      string guid,
      string title,
      string docType,
      string docCategory,
      string signatureType,
      string pairID,
      string dataKey,
      long size,
      string documentTemplateGuid)
    {
      this.guid = guid;
      this.title = title;
      this.docType = docType;
      this.docCategory = docCategory;
      this.signatureType = signatureType;
      this.pairID = pairID;
      this.dataKey = dataKey;
      this.size = size;
      this.templateGuid = documentTemplateGuid;
    }

    internal OrderedDocument(XmlElement xml)
    {
      AttributeReader attributeReader = new AttributeReader(xml);
      this.guid = attributeReader.GetString(nameof (Guid));
      this.title = attributeReader.GetString(nameof (Title));
      this.docType = attributeReader.GetString("Type");
      this.docCategory = attributeReader.GetString("Category");
      this.signatureType = attributeReader.GetString(nameof (SignatureType));
      this.pairID = attributeReader.GetString(nameof (PairID));
      this.dataKey = attributeReader.GetString(nameof (DataKey));
      this.size = attributeReader.GetLong(nameof (Size));
      this.templateGuid = attributeReader.GetString("Template", (string) null);
      this.docSvcDocumentID = attributeReader.GetString("DocSvcDocID", (string) null);
      this.overflowDataKey = attributeReader.GetString(nameof (OverflowDataKey));
      XmlNodeList xmlNodeList = xml.SelectNodes("OVERFLOWS/OVERFLOW");
      this.overflows = new List<OrderedDocumentOverflow>();
      foreach (XmlElement xml1 in xmlNodeList)
        this.overflows.Add(new OrderedDocumentOverflow(xml1));
    }

    public string Guid => this.guid;

    public string Title => this.title;

    public string DocumentType => this.docType;

    public string DocEngineCategory => this.docCategory;

    public string SignatureType => this.signatureType;

    public string PairID => this.pairID;

    public string DataKey => this.dataKey;

    public long Size => this.size;

    public string DocumentTemplateGuid => this.templateGuid;

    public string DocServiceDocumentID => this.docSvcDocumentID;

    public string OverflowDataKey => this.overflowDataKey;

    public List<OrderedDocumentOverflow> Overflows => this.overflows;

    internal void ToXml(XmlElement xml)
    {
      AttributeWriter attributeWriter1 = new AttributeWriter(xml);
      attributeWriter1.Write("Guid", (object) this.guid);
      attributeWriter1.Write("Title", (object) this.title);
      attributeWriter1.Write("Type", (object) this.docType);
      attributeWriter1.Write("Category", (object) this.docCategory);
      attributeWriter1.Write("SignatureType", (object) this.signatureType);
      attributeWriter1.Write("PairID", (object) this.pairID);
      attributeWriter1.Write("DataKey", (object) this.dataKey);
      attributeWriter1.Write("Size", (object) this.size);
      if (this.templateGuid != null)
        attributeWriter1.Write("Template", (object) this.templateGuid);
      if (this.docSvcDocumentID != null)
        attributeWriter1.Write("DocSvcDocID", (object) this.docSvcDocumentID);
      if (!string.IsNullOrEmpty(this.overflowDataKey))
        attributeWriter1.Write("OverflowDataKey", (object) this.overflowDataKey);
      if (this.overflows == null || this.overflows.Count <= 0)
        return;
      ElementWriter elementWriter = new ElementWriter(new ElementWriter(xml).Append("OVERFLOWS"));
      foreach (OrderedDocumentOverflow documentOverflow in this.overflows.ToArray())
      {
        AttributeWriter attributeWriter2 = new AttributeWriter(elementWriter.Append("OVERFLOW", documentOverflow.OriginalText));
        attributeWriter2.Write("PageNumber", (object) documentOverflow.PageNumber);
        attributeWriter2.Write("CoordinateBottom", (object) documentOverflow.CoordinateBottom);
        attributeWriter2.Write("CoordinateLeft", (object) documentOverflow.CoordinateLeft);
        attributeWriter2.Write("CoordinateRight", (object) documentOverflow.CoordinateRight);
        attributeWriter2.Write("CoordinateTop", (object) documentOverflow.CoordinateTop);
        attributeWriter2.Write("TemplateFieldName", (object) documentOverflow.TemplateFieldName);
      }
    }
  }
}
