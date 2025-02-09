// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.EDMLog
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
  public class EDMLog : LogRecordBase
  {
    public static readonly string XmlType = "EDM";
    private string createdBy = string.Empty;
    private string description = string.Empty;
    private string url = string.Empty;
    private string[] docList;

    public EDMLog(string createdBy)
    {
      this.createdBy = createdBy;
      this.date = DateTime.Now;
    }

    public EDMLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.createdBy = attributeReader.GetString("Creator");
      this.description = attributeReader.GetString(nameof (Description));
      this.url = attributeReader.GetString(nameof (Url));
      List<string> stringList = new List<string>();
      foreach (XmlElement selectNode in e.SelectNodes("StringList"))
      {
        string str = new AttributeReader(selectNode).GetString("subItem1");
        stringList.Add(str);
      }
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

    public string[] Documents
    {
      get => this.docList;
      set
      {
        this.docList = value;
        this.MarkAsDirty();
      }
    }

    public string Url
    {
      get => this.url;
      set
      {
        this.url = value;
        this.MarkAsDirty();
      }
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) EDMLog.XmlType);
      attributeWriter.Write("Creator", (object) this.createdBy);
      attributeWriter.Write("Description", (object) this.description);
      attributeWriter.Write("Url", (object) this.url);
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
