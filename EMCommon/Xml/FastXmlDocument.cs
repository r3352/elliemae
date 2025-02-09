// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Xml.FastXmlDocument
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Xml
{
  public class FastXmlDocument : XmlDocument
  {
    public FastXmlDocument()
    {
      this.NodeInserted += new XmlNodeChangedEventHandler(FastXmlDocument.NodeInsertedHandler);
      this.NodeRemoving += new XmlNodeChangedEventHandler(FastXmlDocument.NodeRemovingHandler);
    }

    public FastXmlDocument(XmlNameTable nt)
      : base(nt)
    {
      this.NodeInserted += new XmlNodeChangedEventHandler(FastXmlDocument.NodeInsertedHandler);
      this.NodeRemoving += new XmlNodeChangedEventHandler(FastXmlDocument.NodeRemovingHandler);
    }

    protected internal FastXmlDocument(XmlImplementation imp)
      : base(imp)
    {
      this.NodeInserted += new XmlNodeChangedEventHandler(FastXmlDocument.NodeInsertedHandler);
      this.NodeRemoving += new XmlNodeChangedEventHandler(FastXmlDocument.NodeRemovingHandler);
    }

    public override XmlElement CreateElement(string prefix, string localName, string namespaceURI)
    {
      return (XmlElement) new FastXmlElement(prefix, localName, namespaceURI, (XmlDocument) this);
    }

    private static void NodeInsertedHandler(object sender, XmlNodeChangedEventArgs e)
    {
      if (!(e.NewParent is FastXmlElement newParent) || !(e.Node is XmlAttribute node))
        return;
      newParent.AddAttributeToMap(node);
    }

    private static void NodeRemovingHandler(object sender, XmlNodeChangedEventArgs e)
    {
      if (!(e.NewParent is FastXmlElement newParent) || !(e.Node is XmlAttribute node))
        return;
      newParent.RemoveAttributeFromMap(node);
    }

    ~FastXmlDocument()
    {
      this.NodeInserted -= new XmlNodeChangedEventHandler(FastXmlDocument.NodeInsertedHandler);
      this.NodeRemoving -= new XmlNodeChangedEventHandler(FastXmlDocument.NodeRemovingHandler);
    }
  }
}
