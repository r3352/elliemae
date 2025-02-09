// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.DataTracLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class DataTracLog : LogRecordBase
  {
    public static readonly string XmlType = "DataTrac";
    private string createdBy = string.Empty;
    private string fileID = string.Empty;
    private string message = string.Empty;

    public DataTracLog(string createdBy, string fileID, string message)
    {
      this.date = DateTime.Now;
      this.createdBy = createdBy;
      this.fileID = fileID;
      this.message = message;
    }

    public DataTracLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.createdBy = attributeReader.GetString("Creator");
      this.fileID = attributeReader.GetString(nameof (FileID));
      this.message = attributeReader.GetString(nameof (Message));
      this.MarkAsClean();
    }

    public string CreatedBy => this.createdBy;

    public string FileID => this.fileID;

    public string Message => this.message;

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) DataTracLog.XmlType);
      attributeWriter.Write("Creator", (object) this.createdBy);
      attributeWriter.Write("FileID", (object) this.fileID);
      attributeWriter.Write("Message", (object) this.message);
    }
  }
}
