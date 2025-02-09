// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.DefaultFieldsInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.XPath;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class DefaultFieldsInfo : ISerializable
  {
    private Hashtable map = new Hashtable();
    private string rootNode;

    public Hashtable Map
    {
      get => this.map;
      set => this.map = value;
    }

    public DefaultFieldsInfo(string xmlData, string rootNode)
    {
      this.rootNode = rootNode;
      this.map.Clear();
      XPathDocument xpathDocument;
      try
      {
        xpathDocument = new XPathDocument((TextReader) new StringReader(xmlData));
      }
      catch (Exception ex)
      {
        return;
      }
      XPathNodeIterator xpathNodeIterator = xpathDocument.CreateNavigator().Select("/" + rootNode + "/Field");
      while (xpathNodeIterator.MoveNext())
      {
        string attribute = xpathNodeIterator.Current.GetAttribute("id", string.Empty);
        string strA = xpathNodeIterator.Current.GetAttribute("value", string.Empty);
        if (string.Compare(attribute, "NOTICES.X65", true) == 0 || string.Compare(attribute, "NOTICES.X66", true) == 0 || string.Compare(attribute, "NOTICES.X67", true) == 0 || string.Compare(attribute, "NOTICES.X68", true) == 0 || string.Compare(attribute, "NOTICES.X69", true) == 0 || string.Compare(attribute, "NOTICES.X70", true) == 0 || string.Compare(attribute, "NOTICES.X71", true) == 0)
        {
          if (string.Compare(strA, "Yes - It is required/provides an Opt-Out", true) == 0)
            strA = "Yes";
          else if (string.Compare(strA, "No - Does not provide an Opt-Out", true) == 0)
            strA = "No";
          else if (string.Compare(strA, "No - We don't share", true) == 0)
            strA = "We Don't Share";
        }
        try
        {
          this.map[(object) attribute] = (object) strA;
        }
        catch
        {
        }
      }
    }

    private DefaultFieldsInfo(SerializationInfo info, StreamingContext context)
      : this(info.GetString("xml"), info.GetString(nameof (rootNode)))
    {
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("xml", (object) this.ToXMLString(this.rootNode));
      info.AddValue("rootNode", (object) this.rootNode);
    }

    public string GetField(string fieldID)
    {
      return !this.map.ContainsKey((object) fieldID) ? string.Empty : this.map[(object) fieldID].ToString();
    }

    public string ToXMLString(string rootNode)
    {
      XmlDocument xmlDocument = new XmlDocument();
      XmlElement xmlElement1 = (XmlElement) xmlDocument.AppendChild((XmlNode) xmlDocument.CreateElement(rootNode));
      IDictionaryEnumerator enumerator = this.map.GetEnumerator();
      while (enumerator.MoveNext())
      {
        string key = (string) enumerator.Key;
        string str = (string) enumerator.Value;
        if (key != string.Empty)
        {
          XmlElement xmlElement2 = (XmlElement) xmlElement1.AppendChild((XmlNode) xmlDocument.CreateElement("Field"));
          xmlElement2.SetAttribute("id", key);
          xmlElement2.SetAttribute("value", str);
        }
      }
      return xmlDocument.OuterXml;
    }

    public void SetField(string id, string val)
    {
      if (this.map.ContainsKey((object) id))
        this.map.Remove((object) id);
      this.map[(object) id] = (object) val;
    }
  }
}
