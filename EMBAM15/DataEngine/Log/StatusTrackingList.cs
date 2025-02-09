// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.StatusTrackingList
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class StatusTrackingList
  {
    private LogRecordBase _logRecord;
    private Dictionary<string, StatusTrackingEntry> _dictionary = new Dictionary<string, StatusTrackingEntry>((IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase);

    public StatusTrackingList(LogRecordBase logRecord) => this._logRecord = logRecord;

    public StatusTrackingList(LogRecordBase logRecord, XmlElement parentElement, string name)
    {
      this._logRecord = logRecord;
      XmlElement xmlElement = (XmlElement) parentElement.SelectSingleNode(name);
      if (xmlElement == null)
        return;
      foreach (XmlElement selectNode in xmlElement.SelectNodes("Entry"))
      {
        StatusTrackingEntry statusTrackingEntry = new StatusTrackingEntry(logRecord, selectNode);
        this._dictionary[statusTrackingEntry.Status] = statusTrackingEntry;
      }
    }

    public void Add(StatusTrackingEntry entry)
    {
      this.Remove(entry);
      this._dictionary[entry.Status] = entry;
      entry.AttachToLogEntry(this._logRecord);
      this._logRecord.TrackChange("Status marked \"" + entry.Status + "\"");
    }

    public StatusTrackingEntry Add(string status, string user)
    {
      StatusTrackingEntry entry = new StatusTrackingEntry(status, user);
      this.Add(entry);
      return entry;
    }

    public void Remove(StatusTrackingEntry entry)
    {
      if (!this._dictionary.ContainsKey(entry.Status))
        return;
      this._dictionary.Remove(entry.Status);
      entry.AttachToLogEntry((LogRecordBase) null);
      this._logRecord.TrackChange("Status unmarked \"" + entry.Status + "\"");
    }

    public void Remove(string status)
    {
      if (!this._dictionary.ContainsKey(status))
        return;
      this.Remove(this._dictionary[status]);
    }

    public void ToXml(XmlElement e, string name)
    {
      if (this._dictionary.Count == 0)
        return;
      XmlElement element1 = e.OwnerDocument.CreateElement(name);
      e.AppendChild((XmlNode) element1);
      foreach (string key in this._dictionary.Keys)
      {
        XmlElement element2 = e.OwnerDocument.CreateElement("Entry");
        element1.AppendChild((XmlNode) element2);
        this._dictionary[key].ToXml(element2);
      }
    }

    public string[] GetStatusTrackingIDs()
    {
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) this._dictionary.Keys);
      return stringList.ToArray();
    }

    public StatusTrackingEntry GetStatusTrackingEntry(string statusTrackingId)
    {
      return this._dictionary.ContainsKey(statusTrackingId) ? this._dictionary[statusTrackingId] : (StatusTrackingEntry) null;
    }

    public List<StatusTrackingEntry> GetStatusTrackingEntries()
    {
      Dictionary<string, StatusTrackingEntry> dictionary = this._dictionary;
      return dictionary == null ? (List<StatusTrackingEntry>) null : dictionary.Values.OrderBy<StatusTrackingEntry, DateTime>((Func<StatusTrackingEntry, DateTime>) (o => o.Date)).ToList<StatusTrackingEntry>();
    }
  }
}
