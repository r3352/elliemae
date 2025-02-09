// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.HtmlEmailLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class HtmlEmailLog : LogRecordBase
  {
    public static readonly string XmlType = "HtmlEmail";
    private string createdBy = string.Empty;
    private string description = string.Empty;
    private string sender = string.Empty;
    private string recipient = string.Empty;
    private string subject = string.Empty;
    private string body = string.Empty;
    private string[] docList;
    private bool readReceipt;
    private string linkedRefId = string.Empty;
    private string linkedRefType = string.Empty;

    public HtmlEmailLog(string createdBy)
    {
      this.createdBy = createdBy;
      this.date = DateTime.Now;
    }

    public HtmlEmailLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.createdBy = attributeReader.GetString("Creator");
      this.description = attributeReader.GetString(nameof (Description));
      this.sender = attributeReader.GetString(nameof (Sender));
      this.recipient = attributeReader.GetString(nameof (Recipient));
      this.subject = attributeReader.GetString(nameof (Subject));
      this.body = attributeReader.GetString(nameof (Body));
      this.readReceipt = attributeReader.GetBoolean(nameof (ReadReceipt), false);
      this.linkedRefId = attributeReader.GetString(nameof (LinkedRefId));
      this.linkedRefType = attributeReader.GetString(nameof (LinkedRefType));
      List<string> stringList = new List<string>();
      foreach (XmlElement selectNode in e.SelectNodes("Document"))
      {
        string str = new AttributeReader(selectNode).GetString("Title");
        stringList.Add(str);
      }
      this.docList = stringList.ToArray();
      this.MarkAsClean();
    }

    public string CreatedBy => this.createdBy;

    public string Description
    {
      get => this.description;
      set
      {
        this.description = value;
        this.MarkAsDirty();
      }
    }

    public string Sender
    {
      get => this.sender;
      set
      {
        this.sender = value;
        this.MarkAsDirty();
      }
    }

    public string Recipient
    {
      get => this.recipient;
      set
      {
        this.recipient = value;
        this.MarkAsDirty();
      }
    }

    public string Subject
    {
      get => this.subject;
      set
      {
        this.subject = value;
        this.MarkAsDirty();
      }
    }

    public string Body
    {
      get => this.body;
      set
      {
        this.body = value;
        this.MarkAsDirty();
      }
    }

    public bool ReadReceipt
    {
      get => this.readReceipt;
      set
      {
        this.readReceipt = value;
        this.MarkAsDirty();
      }
    }

    public string[] Documents
    {
      get => this.docList;
      set
      {
        this.docList = value;
        this.MarkAsDirty();
      }
    }

    public string LinkedRefId
    {
      get => this.linkedRefId;
      set
      {
        this.linkedRefId = value;
        this.MarkAsDirty();
      }
    }

    public string LinkedRefType
    {
      get => this.linkedRefType;
      set
      {
        this.linkedRefType = value;
        this.MarkAsDirty();
      }
    }

    public override bool IsLoanOperationalLog => true;

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) HtmlEmailLog.XmlType);
      attributeWriter.Write("Creator", (object) this.createdBy);
      attributeWriter.Write("Description", (object) this.description);
      attributeWriter.Write("Sender", (object) this.sender);
      attributeWriter.Write("Recipient", (object) this.recipient);
      attributeWriter.Write("Subject", (object) this.subject);
      attributeWriter.Write("Body", (object) this.body);
      attributeWriter.Write("ReadReceipt", (object) this.readReceipt);
      attributeWriter.Write("LinkedRefId", (object) this.linkedRefId);
      attributeWriter.Write("LinkedRefType", (object) this.linkedRefType);
      if (this.docList == null)
        return;
      foreach (string doc in this.docList)
      {
        XmlElement element = e.OwnerDocument.CreateElement("Document");
        e.AppendChild((XmlNode) element);
        new AttributeWriter(element).Write("Title", (object) doc);
      }
    }
  }
}
