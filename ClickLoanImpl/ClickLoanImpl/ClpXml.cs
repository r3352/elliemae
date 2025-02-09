// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClickLoanImpl.ClpXml
// Assembly: ClickLoanImpl, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 9549E162-7E74-49E9-BCDA-CB0A69B5F0B5
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClickLoanImpl.dll

using System.IO;
using System.Runtime.InteropServices;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClickLoanImpl
{
  [ComVisible(false)]
  public class ClpXml
  {
    private XmlDocument xmldoc;
    protected XmlElement root;

    public XmlElement Root => this.root;

    public ClpXml(string rootName)
    {
      this.xmldoc = new XmlDocument();
      this.root = this.xmldoc.CreateElement(rootName);
      this.xmldoc.AppendChild((XmlNode) this.root);
    }

    public ClpXml(XmlDocument xmldoc)
    {
      this.xmldoc = xmldoc;
      this.root = xmldoc.DocumentElement;
    }

    public XmlElement GetFirstChildElement(XmlElement curr, string name)
    {
      XmlNodeList elementsByTagName = curr.GetElementsByTagName(name);
      return elementsByTagName == null ? (XmlElement) null : (XmlElement) elementsByTagName.Item(0);
    }

    public XmlAttribute AddAttribute(XmlElement elm, string name, string val)
    {
      if (val == null)
        return (XmlAttribute) null;
      XmlAttribute attribute = this.xmldoc.CreateAttribute(name);
      attribute.Value = val;
      elm.SetAttributeNode(attribute);
      return attribute;
    }

    public XmlElement AddChildElement(XmlElement curr, string childName)
    {
      XmlElement element = this.xmldoc.CreateElement(childName);
      curr.AppendChild((XmlNode) element);
      return element;
    }

    public void AddChildWithAttribute(
      XmlElement curr,
      string childName,
      string attrName,
      string attrVal)
    {
      this.AddAttribute(this.AddChildElement(curr, childName), attrName, attrVal);
    }

    public string GetFirstChildAttribute(XmlElement curr, string childName, string attrName)
    {
      XmlElement firstChildElement = this.GetFirstChildElement(this.root, childName);
      if (firstChildElement == null)
        return (string) null;
      return !firstChildElement.HasAttribute(attrName) ? (string) null : firstChildElement.GetAttribute(attrName);
    }

    public override string ToString()
    {
      StringWriter w1 = new StringWriter();
      XmlTextWriter w2 = new XmlTextWriter((TextWriter) w1);
      w2.Formatting = Formatting.None;
      this.xmldoc.WriteTo((XmlWriter) w2);
      w2.Flush();
      w1.Flush();
      string str = w1.ToString();
      w1.Close();
      w2.Close();
      return str;
    }
  }
}
