// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.DocEngineEntity
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public class DocEngineEntity
  {
    private XmlElement entityXml;
    private Dictionary<string, string> attributeCache = new Dictionary<string, string>();

    public DocEngineEntity(XmlElement entityXml) => this.entityXml = entityXml;

    protected internal XmlElement XML => this.entityXml;

    public bool AttributeExists(string name)
    {
      return this.entityXml.Attributes.GetNamedItem(name) != null;
    }

    public string GetAttribute(string name)
    {
      if (!this.attributeCache.ContainsKey(name))
        this.attributeCache[name] = this.entityXml.GetAttribute(name);
      return this.attributeCache[name];
    }

    public string GetExtendedAttribute(string name)
    {
      if (!this.attributeCache.ContainsKey(name))
      {
        XmlElement xmlElement = (XmlElement) this.entityXml.SelectSingleNode(name);
        if (xmlElement == null)
          return "";
        this.attributeCache[name] = xmlElement.InnerText;
      }
      return this.attributeCache[name];
    }
  }
}
