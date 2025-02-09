// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.DownloadLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class DownloadLog : LogRecordBase
  {
    public static string XmlType = "Download";
    private string fileSource = string.Empty;
    private string fileID = string.Empty;
    private string fileType = string.Empty;
    private string title = string.Empty;
    private string sender = string.Empty;
    private string documentID = string.Empty;
    private int barcodePage;
    private DateTime dateReceived = DateTime.MinValue.Date;
    private string receivedBy = string.Empty;

    public DownloadLog()
    {
    }

    public DownloadLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.fileSource = attributeReader.GetString(nameof (FileSource));
      this.fileID = attributeReader.GetString("DownloadID");
      this.fileType = attributeReader.GetString(nameof (FileType));
      this.title = attributeReader.GetString(nameof (Title));
      this.sender = attributeReader.GetString(nameof (Sender));
      this.documentID = attributeReader.GetString(nameof (DocumentID));
      this.barcodePage = attributeReader.GetInteger(nameof (BarcodePage));
      this.dateReceived = attributeReader.GetDate(nameof (DateReceived));
      this.receivedBy = attributeReader.GetString(nameof (ReceivedBy));
      this.MarkAsClean();
    }

    public string FileSource
    {
      get => this.fileSource;
      set
      {
        this.fileSource = value;
        this.MarkAsDirty();
      }
    }

    public string FileID
    {
      get => this.fileID;
      set
      {
        this.fileID = value;
        this.MarkAsDirty();
      }
    }

    public string FileType
    {
      get => this.fileType;
      set
      {
        this.fileType = value;
        this.MarkAsDirty();
      }
    }

    public string Title
    {
      get => this.title;
      set
      {
        this.title = value;
        this.MarkAsDirty();
      }
    }

    public string Sender
    {
      get => this.sender;
      set
      {
        this.sender = value;
        this.MarkAsDirty();
      }
    }

    public string DocumentID
    {
      get => this.documentID;
      set
      {
        this.documentID = value;
        this.MarkAsDirty();
      }
    }

    public int BarcodePage
    {
      get => this.barcodePage;
      set
      {
        this.barcodePage = value;
        this.MarkAsDirty();
      }
    }

    public bool Received => this.dateReceived.Date != DateTime.MinValue.Date;

    public DateTime DateReceived => this.dateReceived;

    public string ReceivedBy => this.receivedBy;

    public void MarkAsReceived(DateTime date, string user)
    {
      this.dateReceived = date;
      this.receivedBy = user;
      this.MarkAsDirty();
    }

    public void UnmarkAsReceived()
    {
      this.dateReceived = DateTime.MinValue;
      this.receivedBy = string.Empty;
      this.MarkAsDirty();
    }

    internal override bool IncludeInLog() => false;

    public override string ToString() => this.title;

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) DownloadLog.XmlType);
      attributeWriter.Write("FileSource", (object) this.fileSource);
      attributeWriter.Write("DownloadID", (object) this.fileID);
      attributeWriter.Write("FileType", (object) this.fileType);
      attributeWriter.Write("Title", (object) this.title);
      attributeWriter.Write("Sender", (object) this.sender);
      attributeWriter.Write("DocumentID", (object) this.documentID);
      attributeWriter.Write("BarcodePage", (object) this.barcodePage);
      attributeWriter.Write("DateReceived", (object) this.dateReceived);
      attributeWriter.Write("ReceivedBy", (object) this.receivedBy);
    }
  }
}
