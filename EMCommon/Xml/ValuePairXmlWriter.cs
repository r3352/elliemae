// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Xml.ValuePairXmlWriter
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Xml
{
  public class ValuePairXmlWriter
  {
    private string baseNodeName = string.Empty;
    private string nodeName = string.Empty;
    private string keyAttributeName = string.Empty;
    private string valueAttributeName = string.Empty;
    private XmlDocument xmlDoc;
    private XmlElement rootDoc;

    public ValuePairXmlWriter(string keyAttributeName, string valueAttributeName)
      : this("ValueList", "Value", keyAttributeName, valueAttributeName)
    {
    }

    public ValuePairXmlWriter(string xmlString, string keyAttributeName, string valueAttributeName)
      : this("ValueList", "Value", keyAttributeName, valueAttributeName)
    {
      if (!(xmlString != string.Empty))
        return;
      this.xmlDoc = new XmlDocument();
      this.xmlDoc.LoadXml(xmlString);
      this.rootDoc = this.xmlDoc.DocumentElement;
    }

    public Dictionary<string, string> GetKeyValuePair()
    {
      Dictionary<string, string> keyValuePair = new Dictionary<string, string>();
      if (this.xmlDoc != null)
      {
        XmlNodeList xmlNodeList = this.xmlDoc.SelectNodes("//" + this.nodeName);
        if (xmlNodeList != null && xmlNodeList.Count > 0)
        {
          foreach (XmlNode xmlNode in xmlNodeList)
          {
            if (xmlNode.Attributes[this.keyAttributeName] != null && xmlNode.Attributes[this.valueAttributeName] != null && !keyValuePair.ContainsKey(xmlNode.Attributes[this.keyAttributeName].Value))
              keyValuePair.Add(xmlNode.Attributes[this.keyAttributeName].Value, xmlNode.Attributes[this.valueAttributeName].Value);
          }
        }
      }
      return keyValuePair;
    }

    public ValuePairXmlWriter(
      string baseNodeName,
      string nodeName,
      string keyAttributeName,
      string valueAttributeName)
    {
      this.baseNodeName = baseNodeName;
      this.nodeName = nodeName;
      this.keyAttributeName = keyAttributeName;
      this.valueAttributeName = valueAttributeName;
      this.xmlDoc = new XmlDocument();
      this.xmlDoc.LoadXml("<" + this.baseNodeName + "></" + this.baseNodeName + ">");
      this.rootDoc = this.xmlDoc.DocumentElement;
    }

    public void Write(string key, string val)
    {
      if (key == string.Empty)
        return;
      XmlElement element = this.xmlDoc.CreateElement(this.nodeName);
      element.SetAttribute(this.keyAttributeName, key);
      element.SetAttribute(this.valueAttributeName, val);
      this.rootDoc.AppendChild((XmlNode) element);
    }

    public string ToXML() => this.xmlDoc.OuterXml;
  }
}
