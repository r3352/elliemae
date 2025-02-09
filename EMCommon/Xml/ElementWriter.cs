// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Xml.ElementWriter
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Xml
{
  public class ElementWriter
  {
    private XmlElement baseElement;

    public ElementWriter(XmlElement baseElement) => this.baseElement = baseElement;

    public XmlElement BaseElement => this.baseElement;

    public XmlElement Append(string name, string innerText)
    {
      XmlElement xmlElement = (XmlElement) this.baseElement.AppendChild((XmlNode) this.baseElement.OwnerDocument.CreateElement(name));
      xmlElement.InnerText = innerText;
      return xmlElement;
    }

    public XmlElement Append(string name)
    {
      return (XmlElement) this.baseElement.AppendChild((XmlNode) this.baseElement.OwnerDocument.CreateElement(name));
    }
  }
}
