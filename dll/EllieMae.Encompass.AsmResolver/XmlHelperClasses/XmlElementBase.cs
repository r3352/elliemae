// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.XmlHelperClasses.XmlElementBase
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System.Xml;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.XmlHelperClasses
{
  public class XmlElementBase
  {
    protected XmlElement xmlElem;
    protected readonly string tagName;

    public XmlElementBase(string tagName) => this.tagName = tagName;

    public XmlElementBase(XmlElement xmlElem)
    {
      this.xmlElem = xmlElem;
      this.tagName = xmlElem.Name;
    }

    public virtual void CreateElement(XmlDocument xmldoc, XmlElement parent)
    {
      this.xmlElem = xmldoc.CreateElement(this.tagName);
      parent?.AppendChild((XmlNode) this.xmlElem);
    }
  }
}
