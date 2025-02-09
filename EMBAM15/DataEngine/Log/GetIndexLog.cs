// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.GetIndexLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class GetIndexLog : LogRecordBase
  {
    public static readonly string XmlType = "MarginIndex";
    private string indexType = string.Empty;
    private string indexValue = string.Empty;
    private string userName = string.Empty;
    private string userID = string.Empty;

    public GetIndexLog(string indexType, string indexValue, string userName, string userID)
    {
      this.indexType = indexType;
      this.indexValue = indexValue;
      this.userName = userName;
      this.userID = userID;
      this.Date = DateTime.Now;
    }

    public string UserName => this.userName;

    public string IndexType => this.indexType;

    public string IndexValue => this.indexValue;

    public string UserId => this.userID;

    public GetIndexLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.userName = attributeReader.GetString(nameof (UserName));
      this.indexType = attributeReader.GetString(nameof (IndexType));
      this.indexValue = attributeReader.GetString(nameof (IndexValue));
      this.userID = attributeReader.GetString("UserID");
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) GetIndexLog.XmlType);
      attributeWriter.Write("IndexType", (object) this.indexType);
      attributeWriter.Write("IndexValue", (object) this.indexValue);
      attributeWriter.Write("UserName", (object) this.userName);
      attributeWriter.Write("UserID", (object) this.userID);
    }
  }
}
