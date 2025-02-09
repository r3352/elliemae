// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.DocumentOrderLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class DocumentOrderLog : LogRecordBase
  {
    public static readonly string XmlType = "DocumentOrder";
    private DocumentOrderType orderType;
    private string userId;
    private string orderId;
    private LogRecordFieldList docFields;
    private OrderedDocumentList orderedDocs;
    private DocumentAudit audit;
    private DateTime dateFilesPurged = DateTime.MinValue;

    public DocumentOrderLog(
      DateTime orderDate,
      string orderId,
      DocumentOrderType orderType,
      string userId,
      DocumentAudit audit)
      : base(orderDate, "")
    {
      this.orderType = orderType;
      this.userId = userId;
      this.orderId = orderId;
      this.docFields = new LogRecordFieldList((LogRecordBase) this);
      this.orderedDocs = new OrderedDocumentList(this);
      this.audit = audit;
    }

    public DocumentOrderLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.orderType = (DocumentOrderType) Enum.Parse(typeof (DocumentOrderType), attributeReader.GetString(nameof (OrderType)), true);
      this.userId = attributeReader.GetString("UserID");
      this.orderId = attributeReader.GetString(nameof (OrderID));
      this.docFields = new LogRecordFieldList((LogRecordBase) this, (XmlElement) e.SelectSingleNode(nameof (DocumentFields)));
      this.orderedDocs = new OrderedDocumentList(this, (XmlElement) e.SelectSingleNode(nameof (Documents)));
      this.dateFilesPurged = attributeReader.GetDate("DatePurged", DateTime.MinValue);
      XmlElement e1 = (XmlElement) e.SelectSingleNode(nameof (Audit));
      if (e1 != null)
        this.audit = new DocumentAudit(e1);
      this.MarkAsClean();
    }

    public DocumentOrderType OrderType => this.orderType;

    public string OrderedByUserID => this.userId;

    public OrderedDocumentList Documents => this.orderedDocs;

    public DocumentAudit Audit => this.audit;

    public LogRecordFieldList DocumentFields => this.docFields;

    public string OrderID => this.orderId;

    public DateTime DateFilesPurged
    {
      get => this.dateFilesPurged;
      set
      {
        this.dateFilesPurged = value;
        this.MarkAsDirty();
      }
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) DocumentOrderLog.XmlType);
      attributeWriter.Write("Date", (object) this.Date);
      attributeWriter.Write("UserID", (object) this.userId);
      attributeWriter.Write("OrderID", (object) this.orderId);
      attributeWriter.Write("OrderType", (object) this.orderType);
      if (this.dateFilesPurged != DateTime.MinValue)
        attributeWriter.Write("DatePurged", (object) this.dateFilesPurged);
      this.docFields.ToXml((XmlElement) e.AppendChild((XmlNode) e.OwnerDocument.CreateElement("DocumentFields")));
      this.orderedDocs.ToXml((XmlElement) e.AppendChild((XmlNode) e.OwnerDocument.CreateElement("Documents")));
      if (this.audit == null)
        return;
      this.audit.ToXml((XmlElement) e.AppendChild((XmlNode) e.OwnerDocument.CreateElement("Audit")));
    }
  }
}
