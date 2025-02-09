// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SystemAuditTrail.SystemAuditTrailXmlHelper
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer.SystemAuditTrail
{
  internal class SystemAuditTrailXmlHelper
  {
    private XmlDocument xmlDoc;

    public SystemAuditTrailXmlHelper()
    {
      this.xmlDoc = new XmlDocument();
      this.NewAuditTrailDoc();
    }

    public SystemAuditTrailXmlHelper(string data)
    {
      this.xmlDoc = new XmlDocument();
      this.xmlDoc.LoadXml(data);
    }

    public XmlDocument NewAuditTrailDoc()
    {
      this.xmlDoc.LoadXml("<AuditTrail><AuditRecord name=\"\" value = \"\"/></AuditTrail>");
      return this.xmlDoc;
    }

    public void AddNewAuditRecord(string name, string value)
    {
      XmlNode xmlNode = this.xmlDoc.SelectSingleNode("AuditTrail");
      if (xmlNode == null)
      {
        this.NewAuditTrailDoc();
        xmlNode = this.xmlDoc.SelectSingleNode("AuditTrail");
      }
      if (xmlNode.FirstChild.Attributes[nameof (name)].InnerText == "")
      {
        xmlNode.FirstChild.Attributes[nameof (name)].InnerText = name;
        xmlNode.FirstChild.Attributes[nameof (value)].InnerText = value;
      }
      else
      {
        XmlNode newChild = xmlNode.FirstChild.CloneNode(true);
        newChild.Attributes[nameof (name)].InnerText = name;
        newChild.Attributes[nameof (value)].InnerText = value;
        xmlNode.InsertBefore(newChild, xmlNode.FirstChild);
      }
    }

    public XmlDocument Data => this.xmlDoc;

    public Dictionary<string, string> GetAuditRecords()
    {
      Dictionary<string, string> auditRecords = new Dictionary<string, string>();
      XmlNodeList xmlNodeList = this.xmlDoc.SelectNodes("AuditTrail/AuditRecord");
      if (xmlNodeList != null && xmlNodeList.Count > 0)
      {
        foreach (XmlNode xmlNode in xmlNodeList)
        {
          string key = "";
          string str = "";
          if (xmlNode.Attributes["name"] != null)
            key = xmlNode.Attributes["name"].InnerText;
          if (xmlNode.Attributes["value"] != null)
            str = xmlNode.Attributes["value"].InnerText;
          if (auditRecords.ContainsKey(key))
            auditRecords[key] = str;
          else
            auditRecords.Add(key, str);
        }
      }
      return auditRecords;
    }
  }
}
