// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.DataSyncRecord
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class DataSyncRecord : IXmlSerializable
  {
    private string tableName;
    private string uiKeyValue;
    private string dataXmlString;
    private string mappingXmlString;
    private bool isInitial;

    public DataSyncRecord(
      string tableName,
      string uiKeyValue,
      string dataXmlString,
      string mappingXmlString,
      bool isInitial)
    {
      this.tableName = tableName;
      this.uiKeyValue = uiKeyValue;
      this.dataXmlString = dataXmlString;
      this.mappingXmlString = mappingXmlString;
      this.isInitial = isInitial;
    }

    public DataSyncRecord(XmlDocument xmldoc)
    {
      this.tableName = xmldoc.SelectSingleNode("tableName\\@value").Value;
      this.uiKeyValue = xmldoc.SelectSingleNode("uiKeyValue\\@value").Value;
      this.dataXmlString = xmldoc.SelectSingleNode("dataXmlString\\@value").Value;
      this.mappingXmlString = xmldoc.SelectSingleNode("mappingXmlString\\@value").Value;
      this.isInitial = xmldoc.SelectSingleNode("isInitial\\@value").Value == "Y";
    }

    public DataSyncRecord(string xmldocString)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xmldocString);
      this.tableName = xmlDocument.SelectSingleNode("//tableName/@value").Value;
      this.uiKeyValue = xmlDocument.SelectSingleNode("//uiKeyValue/@value").Value;
      this.dataXmlString = xmlDocument.SelectSingleNode("//dataXmlString/@value").Value;
      this.mappingXmlString = xmlDocument.SelectSingleNode("//mappingXmlString/@value").Value;
      this.isInitial = xmlDocument.SelectSingleNode("//isInitial/@value").Value == "Y";
    }

    public DataSyncRecord(XmlSerializationInfo info)
    {
      this.tableName = info.GetString(nameof (tableName));
      this.uiKeyValue = info.GetString(nameof (uiKeyValue));
      this.dataXmlString = info.GetString(nameof (dataXmlString));
      this.mappingXmlString = info.GetString(nameof (mappingXmlString));
      this.isInitial = info.GetBoolean(nameof (isInitial));
    }

    public override int GetHashCode() => base.GetHashCode();

    public override bool Equals(object obj)
    {
      if ((object) (obj as DataSyncRecord) == null)
        return false;
      DataSyncRecord dataSyncRecord = (DataSyncRecord) obj;
      return dataSyncRecord.TableName == this.tableName && dataSyncRecord.UIKeyValue == this.uiKeyValue && dataSyncRecord.isInitial == this.isInitial;
    }

    public static bool operator ==(DataSyncRecord o1, DataSyncRecord o2) => o1.Equals((object) o2);

    public static bool operator !=(DataSyncRecord o1, DataSyncRecord o2) => !o1.Equals((object) o2);

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("tableName", (object) this.tableName);
      info.AddValue("uiKeyValue", (object) this.uiKeyValue);
      info.AddValue("dataXmlString", (object) this.dataXmlString);
      info.AddValue("mappingXmlString", (object) this.mappingXmlString);
      info.AddValue("isInitial", (object) this.isInitial);
    }

    public string TableName => this.tableName;

    public string UIKeyValue => this.uiKeyValue;

    public string DataXmlString => this.dataXmlString;

    public string MappingXmlString => this.mappingXmlString;

    public bool IsInitial => this.isInitial;

    public override string ToString()
    {
      XmlDocument xmlDocument = new XmlDocument();
      XmlElement xmlElement = (XmlElement) xmlDocument.AppendChild((XmlNode) xmlDocument.CreateElement(nameof (DataSyncRecord)));
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("tableName"))).SetAttribute("value", this.tableName);
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("uiKeyValue"))).SetAttribute("value", this.uiKeyValue);
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("dataXmlString"))).SetAttribute("value", this.dataXmlString);
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("mappingXmlString"))).SetAttribute("value", this.mappingXmlString);
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("isInitial"))).SetAttribute("value", this.isInitial ? "Y" : "N");
      return xmlDocument.OuterXml;
    }
  }
}
