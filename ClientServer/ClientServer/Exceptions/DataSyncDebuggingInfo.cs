// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Exceptions.DataSyncDebuggingInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Exceptions
{
  [Serializable]
  public class DataSyncDebuggingInfo : Exception
  {
    public readonly string ErrorMessage;
    public readonly string SqlQuery;
    public readonly string SourceData;
    public readonly string KeyMapping;
    public readonly string TransformedData;
    public readonly string UpdateQuery;
    public readonly bool ContainError;
    public readonly string CategoryName;
    public readonly string UIKey;
    public readonly Dictionary<string, List<string>> NewRecordList;
    public readonly List<FileSystemSyncRecord> FilesToSync;

    public DataSyncDebuggingInfo(
      string errMsg,
      string sqlQuery,
      string srcData,
      Dictionary<string, Dictionary<string, string>> keyMapping,
      string transformedData,
      string updateQuery)
      : base(errMsg)
    {
      this.ErrorMessage = errMsg;
      this.ContainError = true;
      this.SqlQuery = sqlQuery;
      this.SourceData = srcData;
      this.KeyMapping = this.getMappingString(keyMapping);
      this.TransformedData = transformedData;
      this.UpdateQuery = updateQuery;
    }

    public DataSyncDebuggingInfo(
      string errMsg,
      Dictionary<string, List<string>> newRecordList,
      string category,
      string uiKey,
      List<FileSystemSyncRecord> filesToSync)
      : base(errMsg)
    {
      this.ContainError = !(errMsg == "");
      this.NewRecordList = newRecordList;
      this.CategoryName = category;
      this.UIKey = uiKey;
      this.FilesToSync = filesToSync;
    }

    protected DataSyncDebuggingInfo(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.ErrorMessage = info.GetString(nameof (ErrorMessage));
      this.ContainError = info.GetBoolean(nameof (ContainError));
      this.SqlQuery = info.GetString(nameof (SqlQuery));
      this.SourceData = info.GetString(nameof (SourceData));
      this.KeyMapping = info.GetString(nameof (KeyMapping));
      this.TransformedData = info.GetString(nameof (TransformedData));
      this.UpdateQuery = info.GetString(nameof (UpdateQuery));
      this.NewRecordList = (Dictionary<string, List<string>>) info.GetValue(nameof (NewRecordList), typeof (Dictionary<string, List<string>>));
      this.CategoryName = info.GetString(nameof (CategoryName));
      this.UIKey = info.GetString(nameof (UIKey));
      this.FilesToSync = (List<FileSystemSyncRecord>) info.GetValue(nameof (FilesToSync), typeof (List<FileSystemSyncRecord>));
    }

    private string getMappingString(
      Dictionary<string, Dictionary<string, string>> keyMapping)
    {
      if (keyMapping == null)
        return "";
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string key1 in keyMapping.Keys)
      {
        stringBuilder.AppendLine(key1 + ": ");
        Dictionary<string, string> dictionary = keyMapping[key1];
        foreach (string key2 in dictionary.Keys)
          stringBuilder.AppendLine("\t" + key2 + " -> " + dictionary[key2]);
      }
      return stringBuilder.ToString();
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("ErrorMessage", (object) this.ErrorMessage);
      info.AddValue("ContainError", this.ContainError);
      info.AddValue("SqlQuery", (object) this.SqlQuery);
      info.AddValue("SourceData", (object) this.SourceData);
      info.AddValue("KeyMapping", (object) this.KeyMapping);
      info.AddValue("TransformedData", (object) this.TransformedData);
      info.AddValue("UpdateQuery", (object) this.UpdateQuery);
      info.AddValue("NewRecordList", (object) this.NewRecordList);
      info.AddValue("CategoryName", (object) this.CategoryName);
      info.AddValue("UIKey", (object) this.UIKey);
      info.AddValue("FilesToSync", (object) this.FilesToSync);
    }

    public static string FormatXmlDocument(XmlDocument xmlDoc)
    {
      MemoryStream w1 = new MemoryStream();
      using (XmlTextWriter w2 = new XmlTextWriter((Stream) w1, (Encoding) null))
      {
        w2.Formatting = Formatting.Indented;
        xmlDoc.Save((XmlWriter) w2);
        return Encoding.ASCII.GetString(w1.GetBuffer());
      }
    }

    public override string ToString()
    {
      XmlDocument xmlDocument = new XmlDocument();
      XmlElement xmlElement = (XmlElement) xmlDocument.AppendChild((XmlNode) xmlDocument.CreateElement(nameof (DataSyncDebuggingInfo)));
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("ErrorMessage"))).SetAttribute("value", this.ErrorMessage);
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("TransformedData"))).SetAttribute("value", this.TransformedData);
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("SqlQuery"))).SetAttribute("value", this.SqlQuery);
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("SourceData"))).SetAttribute("value", this.SourceData);
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("ContainError"))).SetAttribute("value", this.ContainError ? "Y" : "N");
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("KeyMapping"))).SetAttribute("value", this.KeyMapping);
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("UpdateQuery"))).SetAttribute("value", this.UpdateQuery);
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("CategoryName"))).SetAttribute("value", this.CategoryName);
      ((XmlElement) xmlElement.AppendChild((XmlNode) xmlDocument.CreateElement("UIKey"))).SetAttribute("value", this.UIKey);
      return xmlDocument.OuterXml;
    }
  }
}
