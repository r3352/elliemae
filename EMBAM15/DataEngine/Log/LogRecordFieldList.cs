// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.LogRecordFieldList
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class LogRecordFieldList : IEnumerable<LogRecordField>, IEnumerable
  {
    private LogRecordBase log;
    private Dictionary<string, LogRecordField> fields = new Dictionary<string, LogRecordField>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);

    public LogRecordFieldList(LogRecordBase log) => this.log = log;

    public LogRecordFieldList(LogRecordBase log, XmlElement xml)
    {
      if (xml != null)
      {
        foreach (XmlElement selectNode in xml.SelectNodes("Field"))
        {
          string attribute1 = selectNode.GetAttribute("FieldID");
          string attribute2 = selectNode.GetAttribute("FieldValue");
          this.fields.Add(attribute1, new LogRecordField(this, attribute1, attribute2));
        }
      }
      this.log = log;
    }

    public int Count => this.fields.Count;

    public void SetField(string fieldID, string fieldValue)
    {
      if (this.fields.ContainsKey(fieldID))
        this.fields[fieldID].FieldValue = fieldValue;
      else
        this.fields.Add(fieldID, new LogRecordField(this, fieldID, fieldValue));
      this.MarkAsDirty();
    }

    public string GetField(string fieldID)
    {
      return this.fields.ContainsKey(fieldID) ? this.fields[fieldID].FieldValue : "";
    }

    public bool ContainsValue(string fieldID) => this.fields.ContainsKey(fieldID);

    public void Clear()
    {
      this.fields.Clear();
      this.MarkAsDirty();
    }

    public string[] GetFieldIDs()
    {
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) this.fields.Keys);
      return stringList.ToArray();
    }

    internal void MarkAsDirty() => this.log.MarkAsDirty();

    internal void ToXml(XmlElement xml)
    {
      foreach (string key in this.fields.Keys)
      {
        XmlElement xmlElement = (XmlElement) xml.AppendChild((XmlNode) xml.OwnerDocument.CreateElement("Field"));
        xmlElement.SetAttribute("FieldID", key);
        xmlElement.SetAttribute("FieldValue", this.fields[key].FieldValue);
      }
    }

    public IEnumerator<LogRecordField> GetEnumerator()
    {
      return (IEnumerator<LogRecordField>) this.fields.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
