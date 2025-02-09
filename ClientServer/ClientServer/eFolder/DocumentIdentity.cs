// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.eFolder.DocumentIdentity
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer.eFolder
{
  [Serializable]
  public class DocumentIdentity
  {
    private int pageIndex;
    private string docType;
    private string docSource;

    public DocumentIdentity(int pageIndex, string docType, string docSource)
    {
      this.pageIndex = pageIndex;
      this.docType = docType;
      this.docSource = docSource;
    }

    public DocumentIdentity(XmlElement elm)
    {
      AttributeReader attributeReader = new AttributeReader(elm);
      this.pageIndex = attributeReader.GetInteger(nameof (PageIndex));
      this.docType = attributeReader.GetString("Type");
      this.docSource = attributeReader.GetString("Source");
    }

    public int PageIndex => this.pageIndex;

    public string DocumentType => this.docType;

    public string DocumentSource => this.docSource;

    public void ToXml(XmlElement elm)
    {
      AttributeWriter attributeWriter = new AttributeWriter(elm);
      attributeWriter.Write("PageIndex", (object) this.pageIndex);
      attributeWriter.Write("Type", (object) this.docType);
      attributeWriter.Write("Source", (object) this.docSource);
    }
  }
}
