// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.DataSync.DataSyncDebuggingInfo
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Elli.Server.Remoting.DataSync
{
  public class DataSyncDebuggingInfo
  {
    public readonly string ErrorMessage;
    public readonly string SqlQuery;
    public readonly string SourceData;
    public readonly string KeyMapping;
    public readonly string TransformedData;
    public readonly string UpdateQuery;
    public readonly bool ContainError;
    public readonly Dictionary<string, List<string>> NewRecordList;

    public DataSyncDebuggingInfo(
      string errMsg,
      string sqlQuery,
      string srcData,
      Dictionary<string, Dictionary<string, string>> keyMapping,
      string transformedData,
      string updateQuery)
    {
      this.ErrorMessage = errMsg;
      this.ContainError = true;
      this.SqlQuery = sqlQuery;
      this.SourceData = srcData;
      this.KeyMapping = this.getMappingString(keyMapping);
      this.TransformedData = transformedData;
      this.UpdateQuery = updateQuery;
    }

    public DataSyncDebuggingInfo(Dictionary<string, List<string>> newRecordList)
    {
      this.ContainError = false;
      this.NewRecordList = newRecordList;
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
  }
}
