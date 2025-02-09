// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.FileSystemSyncRecord
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class FileSystemSyncRecord : IXmlSerializable
  {
    private string displayName;
    private string rawData;
    private bool alreadyExist;
    private string id;
    private string tableName;
    private string uiKey;
    private bool isFolder;
    private Dictionary<string, string> specialSetting;
    private string hashCode;

    public FileSystemSyncRecord(
      string id,
      string UIKey,
      string displayName,
      string rawData,
      string tableName,
      bool isFolderType,
      string hashCode)
    {
      this.id = id;
      this.uiKey = UIKey;
      this.displayName = displayName;
      this.rawData = rawData;
      this.alreadyExist = false;
      this.tableName = tableName;
      this.isFolder = isFolderType;
      this.specialSetting = new Dictionary<string, string>();
      this.hashCode = hashCode;
    }

    public FileSystemSyncRecord(XmlDocument xmldoc)
    {
      this.id = xmldoc.SelectSingleNode("id\\@value").Value;
      this.uiKey = xmldoc.SelectSingleNode("uiKey\\@value").Value;
      this.displayName = xmldoc.SelectSingleNode("displayName\\@value").Value;
      this.rawData = xmldoc.SelectSingleNode("rawData\\@value").Value;
      this.alreadyExist = xmldoc.SelectSingleNode("alreadyExist\\@value").Value == "Y";
      this.tableName = xmldoc.SelectSingleNode("tableName\\@value").Value;
      this.isFolder = xmldoc.SelectSingleNode("isFolder\\@value").Value == "Y";
      this.parseSpecialSetting(xmldoc.SelectSingleNode("specialSetting\\@value").Value);
      this.hashCode = xmldoc.SelectSingleNode("hashCode\\@value").Value;
    }

    public FileSystemSyncRecord(string xmldocString, bool skipRawData)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xmldocString);
      this.id = xmlDocument.SelectSingleNode("//id/@value").Value;
      this.uiKey = xmlDocument.SelectSingleNode("//uiKey/@value").Value;
      this.displayName = xmlDocument.SelectSingleNode("//displayName/@value").Value;
      if (!skipRawData)
        this.rawData = xmlDocument.SelectSingleNode("//rawData/@value").Value;
      this.alreadyExist = xmlDocument.SelectSingleNode("//alreadyExist/@value").Value == "Y";
      this.tableName = xmlDocument.SelectSingleNode("//tableName/@value").Value;
      this.isFolder = xmlDocument.SelectSingleNode("//isFolder/@value").Value == "Y";
      this.parseSpecialSetting(xmlDocument.SelectSingleNode("//specialSetting/@value").Value);
      this.hashCode = xmlDocument.SelectSingleNode("//hashCode/@value").Value;
    }

    public FileSystemSyncRecord(XmlSerializationInfo info)
    {
      this.id = info.GetString(nameof (id));
      this.uiKey = info.GetString(nameof (uiKey));
      this.displayName = info.GetString(nameof (displayName));
      this.rawData = info.GetString(nameof (rawData));
      this.alreadyExist = info.GetBoolean(nameof (alreadyExist));
      this.tableName = info.GetString(nameof (tableName));
      this.isFolder = info.GetBoolean(nameof (isFolder));
      this.parseSpecialSetting(info.GetString(nameof (specialSetting)));
      this.hashCode = info.GetString(nameof (hashCode));
    }

    private void parseSpecialSetting(string settings)
    {
      this.specialSetting = new Dictionary<string, string>();
      string str1 = settings;
      string[] separator = new string[1]{ "&&;" };
      foreach (string str2 in str1.Split(separator, StringSplitOptions.RemoveEmptyEntries))
      {
        string key = str2.Substring(0, str2.IndexOf(":"));
        string str3 = str2.Replace(key + ":", "");
        this.specialSetting.Add(key, str3);
      }
    }

    private string getSpecialSettingString()
    {
      string specialSettingString = "";
      foreach (string key in this.specialSetting.Keys)
      {
        if (specialSettingString == "")
          specialSettingString = key + ":" + this.specialSetting[key];
        else
          specialSettingString = specialSettingString + "&&;" + key + ":" + this.specialSetting[key];
      }
      return specialSettingString;
    }

    public override int GetHashCode() => base.GetHashCode();

    public override bool Equals(object obj)
    {
      if ((object) (obj as FileSystemSyncRecord) == null)
        return false;
      FileSystemSyncRecord systemSyncRecord = (FileSystemSyncRecord) obj;
      return systemSyncRecord.UIKey == this.uiKey && this.id == systemSyncRecord.ID && this.displayName == systemSyncRecord.DisplayName;
    }

    public static bool operator ==(FileSystemSyncRecord o1, FileSystemSyncRecord o2)
    {
      return o1.Equals((object) o2);
    }

    public static bool operator !=(FileSystemSyncRecord o1, FileSystemSyncRecord o2)
    {
      return !o1.Equals((object) o2);
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("id", (object) this.id);
      info.AddValue("uiKey", (object) this.uiKey);
      info.AddValue("displayName", (object) this.displayName);
      info.AddValue("rawData", (object) this.rawData);
      info.AddValue("alreadyExist", (object) this.alreadyExist);
      info.AddValue("tableName", (object) this.tableName);
      info.AddValue("isFolder", (object) this.isFolder);
      info.AddValue("specialSetting", (object) this.getSpecialSettingString());
      info.AddValue("hashCode", (object) this.hashCode);
    }

    public object this[string propertyName]
    {
      get
      {
        return this.specialSetting.ContainsKey(propertyName) ? (object) this.specialSetting[propertyName] : (object) null;
      }
      set => this.specialSetting[propertyName] = string.Concat(value);
    }

    public string ID => this.id;

    public string UIKey => this.uiKey;

    public string DisplayName => this.displayName;

    public string RawData
    {
      get => this.rawData;
      set => this.rawData = value;
    }

    public bool AlreadyExist
    {
      get => this.alreadyExist;
      set => this.alreadyExist = value;
    }

    public string TableName => this.tableName;

    public bool IsFolder => this.isFolder;

    public string HashCode => this.hashCode;

    public override string ToString()
    {
      XmlDocument xmlDocument = new XmlDocument();
      XmlElement xmlElement = (XmlElement) xmlDocument.AppendChild((XmlNode) xmlDocument.CreateElement(nameof (FileSystemSyncRecord)));
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("id"))).SetAttribute("value", this.id);
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("uiKey"))).SetAttribute("value", this.uiKey);
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("displayName"))).SetAttribute("value", this.displayName);
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("rawData"))).SetAttribute("value", this.rawData);
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("alreadyExist"))).SetAttribute("value", this.alreadyExist ? "Y" : "N");
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("tableName"))).SetAttribute("value", this.tableName);
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("isFolder"))).SetAttribute("value", this.isFolder ? "Y" : "N");
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("specialSetting"))).SetAttribute("value", this.getSpecialSettingString());
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("hashCode"))).SetAttribute("value", this.hashCode);
      return xmlDocument.OuterXml;
    }
  }
}
