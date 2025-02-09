// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Xml.FastXmlElement
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Xml
{
  public class FastXmlElement : XmlElement
  {
    private static readonly int MinAttributeCount = Utils.ParseInt((object) ConfigurationManager.AppSettings["FastXmlElement.MinAttributeCount"], 25);
    private Dictionary<string, XmlAttribute> _attributeMap;

    protected internal FastXmlElement(
      string prefix,
      string localName,
      string namespaceURI,
      XmlDocument doc)
      : base(prefix, localName, namespaceURI, doc)
    {
    }

    public override XmlAttribute GetAttributeNode(string localName, string namespaceURI)
    {
      if (this._attributeMap == null)
        return base.GetAttributeNode(localName, namespaceURI);
      XmlAttribute xmlAttribute;
      return this._attributeMap.TryGetValue(localName, out xmlAttribute) ? xmlAttribute : (XmlAttribute) null;
    }

    public override XmlAttribute GetAttributeNode(string name)
    {
      if (this._attributeMap == null)
        return base.GetAttributeNode(name);
      XmlAttribute xmlAttribute;
      return this._attributeMap.TryGetValue(name, out xmlAttribute) ? xmlAttribute : (XmlAttribute) null;
    }

    internal void AddAttributeToMap(XmlAttribute attribute)
    {
      if (this._attributeMap == null && this.Attributes.Count >= FastXmlElement.MinAttributeCount)
      {
        this._attributeMap = new Dictionary<string, XmlAttribute>();
        foreach (XmlAttribute attribute1 in (XmlNamedNodeMap) this.Attributes)
          this._attributeMap.Add(attribute1.Name, attribute1);
      }
      else
      {
        if (this._attributeMap == null)
          return;
        this._attributeMap.Add(attribute.Name, attribute);
      }
    }

    internal void RemoveAttributeFromMap(XmlAttribute attribute)
    {
      if (this._attributeMap == null)
        return;
      if (this._attributeMap.Count <= FastXmlElement.MinAttributeCount)
        this._attributeMap = (Dictionary<string, XmlAttribute>) null;
      else
        this._attributeMap.Remove(attribute.Name);
    }
  }
}
