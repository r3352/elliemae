// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.StatusTrackingEntry
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class StatusTrackingEntry
  {
    private LogRecordBase _logRecord;
    private string _status;
    private string _user;
    private DateTime _date;

    public StatusTrackingEntry()
    {
    }

    public StatusTrackingEntry(string status, string userId)
    {
      this._status = status;
      this._user = userId;
      this._date = DateTime.UtcNow;
    }

    public StatusTrackingEntry(LogRecordBase logRecord, XmlElement e)
    {
      this._logRecord = logRecord;
      AttributeReader attributeReader = new AttributeReader(e);
      this._status = attributeReader.GetString(nameof (Status));
      this._user = attributeReader.GetString("User");
      this._date = attributeReader.GetUtcDate(nameof (Date));
    }

    public string Status
    {
      get => this._status;
      set
      {
        if (!(this._status != value))
          return;
        this._status = value;
        this.MarkAsDirty();
      }
    }

    public string UserId
    {
      get => this._user;
      set
      {
        if (!(this._user != value))
          return;
        this._user = value;
        this.MarkAsDirty();
      }
    }

    public DateTime Date
    {
      get => this._date;
      set
      {
        if (!(this._date != value))
          return;
        this._date = value;
        this.MarkAsDirty();
      }
    }

    internal void AttachToLogEntry(LogRecordBase logRecord) => this._logRecord = logRecord;

    internal void MarkAsDirty()
    {
      if (this._logRecord == null)
        return;
      this._logRecord.MarkAsDirty();
    }

    public void ToXml(XmlElement e)
    {
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Status", (object) this._status);
      attributeWriter.Write("User", (object) this._user);
      attributeWriter.Write("Date", (object) this._date);
    }
  }
}
