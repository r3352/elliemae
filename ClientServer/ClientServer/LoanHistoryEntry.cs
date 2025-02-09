// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanHistoryEntry
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanHistoryEntry
  {
    private string objectID;
    private HistoryObjectType objectType;
    private long utcTicks;
    private string userID;
    private string details;
    private string linkedObjectID;
    private LinkedObjectType linkedObjectType;

    public LoanHistoryEntry(
      string objectID,
      HistoryObjectType objectType,
      string userID,
      string details)
      : this(objectID, objectType, userID, details, (string) null, LinkedObjectType.None)
    {
    }

    public LoanHistoryEntry(
      string objectID,
      HistoryObjectType objectType,
      string userID,
      string details,
      string linkedObjectID,
      LinkedObjectType linkedObjectType)
    {
      this.objectID = objectID;
      this.objectType = objectType;
      this.userID = userID;
      this.details = details;
      this.linkedObjectID = linkedObjectID;
      this.linkedObjectType = linkedObjectType;
    }

    public LoanHistoryEntry(XmlElement elm)
    {
      AttributeReader attributeReader = new AttributeReader(elm);
      this.objectID = attributeReader.GetString(nameof (ObjectID));
      this.utcTicks = attributeReader.GetLong("UtcTicks");
      this.userID = attributeReader.GetString(nameof (UserID));
      this.details = attributeReader.GetString(nameof (Details));
      this.linkedObjectID = attributeReader.GetString(nameof (LinkedObjectID), (string) null);
      switch (attributeReader.GetString(nameof (ObjectType)))
      {
        case "LogRecord":
          this.objectType = HistoryObjectType.LogRecord;
          break;
        case "FileAttachment":
          this.objectType = HistoryObjectType.FileAttachment;
          break;
        case "PageImage":
          this.objectType = HistoryObjectType.PageImage;
          break;
        default:
          this.objectType = HistoryObjectType.None;
          break;
      }
      switch (attributeReader.GetString(nameof (LinkedObjectType)))
      {
        case "LogRecord":
          this.linkedObjectType = LinkedObjectType.LogRecord;
          break;
        case "FileAttachment":
          this.linkedObjectType = LinkedObjectType.FileAttachment;
          break;
        case "PageImage":
          this.linkedObjectType = LinkedObjectType.PageImage;
          break;
        case "LogComment":
          this.linkedObjectType = LinkedObjectType.LogComment;
          break;
        default:
          this.linkedObjectType = LinkedObjectType.None;
          break;
      }
    }

    public string ObjectID => this.objectID;

    public HistoryObjectType ObjectType => this.objectType;

    public DateTime Date
    {
      get
      {
        DateTime dateTime = new DateTime(this.utcTicks, DateTimeKind.Utc);
        return dateTime.Date != DateTime.MinValue ? dateTime.ToLocalTime() : DateTime.MinValue;
      }
    }

    public string UserID => this.userID;

    public string Details => this.details;

    public string LinkedObjectID => this.linkedObjectID;

    public LinkedObjectType LinkedObjectType => this.linkedObjectType;

    public void ToXml(long utcTicks, XmlElement elm)
    {
      string str1 = (string) null;
      switch (this.objectType)
      {
        case HistoryObjectType.LogRecord:
          str1 = "LogRecord";
          break;
        case HistoryObjectType.FileAttachment:
          str1 = "FileAttachment";
          break;
        case HistoryObjectType.PageImage:
          str1 = "PageImage";
          break;
      }
      string str2 = (string) null;
      switch (this.linkedObjectType)
      {
        case LinkedObjectType.LogRecord:
          str2 = "LogRecord";
          break;
        case LinkedObjectType.FileAttachment:
          str2 = "FileAttachment";
          break;
        case LinkedObjectType.PageImage:
          str2 = "PageImage";
          break;
        case LinkedObjectType.LogComment:
          str2 = "LogComment";
          break;
      }
      AttributeWriter attributeWriter = new AttributeWriter(elm);
      attributeWriter.Write("ObjectID", (object) this.objectID);
      attributeWriter.Write("ObjectType", (object) str1);
      if (this.utcTicks > 0L)
      {
        attributeWriter.Write("UtcTicks", (object) this.utcTicks);
        attributeWriter.Write("MergedUtcTicks", (object) utcTicks);
      }
      else
        attributeWriter.Write("UtcTicks", (object) utcTicks);
      attributeWriter.Write("UserID", (object) this.userID);
      attributeWriter.Write("Details", (object) this.details);
      attributeWriter.Write("LinkedObjectID", (object) this.linkedObjectID);
      attributeWriter.Write("LinkedObjectType", (object) str2);
    }
  }
}
