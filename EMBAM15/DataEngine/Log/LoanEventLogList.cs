// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.LoanEventLogList
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Serialization;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  [Serializable]
  public class LoanEventLogList : ISerializable
  {
    private XmlDocument xmlDoc;
    private Dictionary<string, LogRecordBase> nonSysLogList = new Dictionary<string, LogRecordBase>();
    private Dictionary<string, Dictionary<string, LogRecordBase>> sysLogList = new Dictionary<string, Dictionary<string, LogRecordBase>>();
    private XmlElement root;
    private const string className = "LoanEventLogList";
    private static readonly string sw = Tracing.SwDataEngine;
    private static Hashtable recordTypes = (Hashtable) null;
    private BinaryObject remoteData;

    public LoanEventLogList()
    {
      this.xmlDoc = new XmlDocument();
      this.root = this.xmlDoc.CreateElement("LOG");
      this.xmlDoc.AppendChild((XmlNode) this.root);
    }

    public LoanEventLogList(string xmlContent)
    {
      this.xmlDoc = new XmlDocument();
      this.xmlDoc.LoadXml(xmlContent);
      this.root = (XmlElement) this.xmlDoc.SelectSingleNode("LOG");
      this.parseXml();
    }

    public LoanEventLogList(XmlDocument xmlDoc)
    {
      this.xmlDoc = xmlDoc;
      this.root = (XmlElement) this.xmlDoc.SelectSingleNode("LOG");
      this.parseXml();
    }

    private LoanEventLogList(SerializationInfo info, StreamingContext context)
    {
      this.remoteData = (BinaryObject) info.GetValue("xml", typeof (BinaryObject));
      this.xmlDoc = new XmlDocument();
      this.xmlDoc.LoadXml(this.remoteData.ToString(Encoding.ASCII));
      this.root = (XmlElement) this.xmlDoc.SelectSingleNode("LOG");
      this.parseXml();
    }

    private void parseXml() => this.parseLogRecords(this.root.SelectNodes("Record"));

    private void parseLogRecords(XmlNodeList nodeList)
    {
      this.nonSysLogList.Clear();
      this.sysLogList.Clear();
      if (nodeList == null || nodeList.Count == 0)
        return;
      foreach (XmlNode node in nodeList)
      {
        if (node.Attributes["SysID"] != null && node.Attributes["SysID"].Value == "")
          this.parseSystemLog(node);
        else
          this.parseLog(node);
      }
    }

    private void parseSystemLog(XmlNode sysRecord)
    {
    }

    private void parseLog(XmlNode record)
    {
      LogRecordBase logRecordBase = this.deserializeRecord((XmlElement) record);
      if (logRecordBase == null)
        return;
      this.nonSysLogList.Add(logRecordBase.Guid, logRecordBase);
    }

    private LogRecordBase deserializeRecord(XmlElement e)
    {
      string typeName = e.GetAttribute("Type") ?? "";
      if (typeName == "")
        return (LogRecordBase) null;
      Type typeOfRecord = LoanEventLogList.getTypeOfRecord(typeName);
      if (typeOfRecord == (Type) null)
        return (LogRecordBase) null;
      try
      {
        ConstructorInfo constructor = typeOfRecord.GetConstructor(new Type[2]
        {
          typeof (LogList),
          e.GetType()
        });
        if (constructor == (ConstructorInfo) null)
          return (LogRecordBase) null;
        return (LogRecordBase) constructor.Invoke(new object[2]
        {
          null,
          (object) e
        });
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanEventLogList.sw, nameof (LoanEventLogList), TraceLevel.Error, "Error deserializing log entry of type '" + typeName + "': " + (object) ex);
        return (LogRecordBase) null;
      }
    }

    private static Type getTypeOfRecord(string typeName)
    {
      lock (typeof (LogList))
      {
        if (LoanEventLogList.recordTypes == null)
          LoanEventLogList.recordTypes = LoanEventLogList.loadLogRecordTypes();
      }
      return (Type) LoanEventLogList.recordTypes[(object) typeName];
    }

    private static Hashtable loadLogRecordTypes()
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      Type type1 = typeof (LogRecordBase);
      foreach (Type type2 in Assembly.GetExecutingAssembly().GetTypes())
      {
        if (type1.IsAssignableFrom(type2))
        {
          if (type2 != type1)
          {
            try
            {
              string key = (string) type2.GetField("XmlType", BindingFlags.Static | BindingFlags.Public)?.GetValue((object) null);
              if (!string.IsNullOrEmpty(key))
                insensitiveHashtable[(object) key] = (object) type2;
            }
            catch
            {
            }
          }
        }
      }
      return insensitiveHashtable;
    }

    public PrintLog[] GetPrintLogs()
    {
      List<PrintLog> printLogList = new List<PrintLog>();
      if (this.nonSysLogList != null && this.nonSysLogList.Count > 0)
      {
        foreach (LogRecordBase logRecordBase in this.nonSysLogList.Values)
        {
          if (logRecordBase is PrintLog)
            printLogList.Add((PrintLog) logRecordBase);
        }
      }
      return printLogList.ToArray();
    }

    public ExportLog[] GetExportLogs()
    {
      List<ExportLog> exportLogList = new List<ExportLog>();
      if (this.nonSysLogList != null && this.nonSysLogList.Count > 0)
      {
        foreach (LogRecordBase logRecordBase in this.nonSysLogList.Values)
        {
          if (logRecordBase is ExportLog)
            exportLogList.Add((ExportLog) logRecordBase);
        }
      }
      return exportLogList.ToArray();
    }

    public LogRecordBase[] GetAllNonSystemLogs()
    {
      List<LogRecordBase> logRecordBaseList = new List<LogRecordBase>();
      logRecordBaseList.AddRange((IEnumerable<LogRecordBase>) this.nonSysLogList.Values);
      return logRecordBaseList.ToArray();
    }

    public void InsertNonSystemLog(LogRecordBase newLog)
    {
      if (this.nonSysLogList.ContainsKey(newLog.Guid))
        this.nonSysLogList[newLog.Guid] = newLog;
      else
        this.nonSysLogList.Add(newLog.Guid, newLog);
    }

    private XmlDocument ToXmlDocument()
    {
      this.resetXml();
      foreach (LogRecordBase logEntry in this.nonSysLogList.Values)
        this.root.AppendChild((XmlNode) this.createXmlRecord(this.root, logEntry));
      return this.xmlDoc;
    }

    [Obsolete]
    public string ToXml() => this.ToXmlDocument().OuterXml;

    public Stream ToStream()
    {
      MemoryStream stream = StreamHelper.NewMemoryStream();
      using (XmlWriter writer = XmlHelper.CreateWriter((Stream) stream))
        this.ToXmlDocument().WriteTo(writer);
      return (Stream) stream;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (this.xmlDoc == null)
        info.AddValue("xml", (object) this.remoteData);
      else
        info.AddValue("xml", (object) new BinaryObject(this.ToStream(), false));
    }

    private void resetXml()
    {
      this.xmlDoc = new XmlDocument();
      this.root = this.xmlDoc.CreateElement("LOG");
      this.xmlDoc.AppendChild((XmlNode) this.root);
    }

    private XmlElement createXmlRecord(XmlElement parent, LogRecordBase logEntry)
    {
      XmlElement element = parent.OwnerDocument.CreateElement("Record");
      logEntry.ToXml(element);
      logEntry.UnsetNew();
      return element;
    }
  }
}
