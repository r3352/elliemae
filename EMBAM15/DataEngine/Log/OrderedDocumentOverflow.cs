// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.OrderedDocumentOverflow
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class OrderedDocumentOverflow
  {
    private int pageNumber;
    private string coordinateTop;
    private string coordinateLeft;
    private string coordinateRight;
    private string coordinateBottom;
    private string templateFieldName;
    private string originalText;

    public OrderedDocumentOverflow(
      int pageNumber,
      string coordinateTop,
      string coordinateLeft,
      string coordinateRight,
      string coordinateBottom,
      string templateFieldName,
      string originalText)
    {
      this.pageNumber = pageNumber;
      this.coordinateBottom = coordinateBottom;
      this.coordinateLeft = coordinateLeft;
      this.coordinateRight = coordinateRight;
      this.coordinateTop = coordinateTop;
      this.templateFieldName = templateFieldName;
      this.originalText = originalText;
    }

    internal OrderedDocumentOverflow(XmlElement xml)
    {
      AttributeReader attributeReader = new AttributeReader(xml);
      this.pageNumber = attributeReader.GetInteger(nameof (PageNumber));
      this.coordinateBottom = attributeReader.GetString(nameof (CoordinateBottom));
      this.coordinateLeft = attributeReader.GetString(nameof (CoordinateLeft));
      this.coordinateRight = attributeReader.GetString(nameof (CoordinateRight));
      this.coordinateTop = attributeReader.GetString(nameof (CoordinateTop));
      this.templateFieldName = attributeReader.GetString(nameof (TemplateFieldName));
      this.originalText = xml.InnerText;
    }

    public int PageNumber => this.pageNumber;

    public string CoordinateTop => this.coordinateTop;

    public string CoordinateLeft => this.coordinateLeft;

    public string CoordinateRight => this.coordinateRight;

    public string CoordinateBottom => this.coordinateBottom;

    public string TemplateFieldName => this.templateFieldName;

    public string OriginalText => this.originalText;

    internal void ToXml(XmlElement xml)
    {
      AttributeWriter attributeWriter = new AttributeWriter(xml);
      attributeWriter.Write("PageNumber", (object) this.pageNumber);
      attributeWriter.Write("CoordinateBottom", (object) this.coordinateBottom);
      attributeWriter.Write("CoordinateLeft", (object) this.coordinateLeft);
      attributeWriter.Write("CoordinateRight", (object) this.coordinateRight);
      attributeWriter.Write("CoordinateTop", (object) this.coordinateTop);
      attributeWriter.Write("TemplateFieldName", (object) this.templateFieldName);
      xml.InnerText = this.originalText;
    }
  }
}
