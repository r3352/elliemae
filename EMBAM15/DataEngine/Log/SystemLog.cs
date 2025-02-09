// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.SystemLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class SystemLog : LogRecordBase
  {
    public static readonly string XmlType = "System";
    private string description = string.Empty;

    public SystemLog(DateTime date)
      : base(date, "")
    {
    }

    public SystemLog(LogList log, XmlElement e)
      : base(log, e)
    {
      this.description = new AttributeReader(e).GetString(nameof (Description));
    }

    public string Description
    {
      get => this.description;
      set
      {
        this.description = value;
        this.MarkAsDirty();
      }
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) SystemLog.XmlType);
      attributeWriter.Write("Description", (object) this.Description);
    }
  }
}
