// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.ServicingPrintLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System.Collections;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class ServicingPrintLog : LogRecordBase
  {
    public static readonly string XmlType = "ServicingPrint";
    private Hashtable allFields;

    public ServicingPrintLog() => this.allFields = new Hashtable();

    public ServicingPrintLog(LogList log, XmlElement e)
      : base(log, e)
    {
      if (this.allFields != null)
        this.allFields.Clear();
      this.allFields = new Hashtable();
      AttributeReader attributeReader = new AttributeReader(e);
      for (int index = 1; index <= 86; ++index)
        this.SetField("SERVICE.X" + index.ToString(), attributeReader.GetString("SERVICE.X" + index.ToString()));
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) ServicingPrintLog.XmlType);
      for (int index = 1; index < 86; ++index)
      {
        string field = this.GetField("SERVICE.X" + index.ToString());
        if (!(field == string.Empty) && !(field == "\\"))
          attributeWriter.Write("SERVICE.X" + index.ToString(), (object) field);
      }
    }

    public void SetField(string id, string val)
    {
      if (!this.allFields.ContainsKey((object) id))
      {
        this.allFields.Add((object) id, (object) val);
      }
      else
      {
        this.allFields[(object) id] = (object) val;
        this.MarkAsDirty();
      }
    }

    public string GetField(string id)
    {
      return !this.allFields.ContainsKey((object) id) ? "" : this.allFields[(object) id].ToString();
    }

    public string PrintedBy => this.GetField("SERVICE.X11");
  }
}
