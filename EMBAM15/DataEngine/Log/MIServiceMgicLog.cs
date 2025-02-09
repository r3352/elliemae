// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.MIServiceMgicLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class MIServiceMgicLog : LogRecordBase
  {
    public static readonly string XmlType = nameof (MIServiceMgicLog);
    private string name = string.Empty;
    private string result = string.Empty;
    private string details = string.Empty;
    private bool showAlert;

    public MIServiceMgicLog(string name, string result, string details, bool showAlert)
    {
      this.name = name;
      this.result = result;
      this.details = details;
      this.showAlert = showAlert;
      this.date = DateTime.Now;
    }

    public MIServiceMgicLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.name = attributeReader.GetString(nameof (Name));
      this.result = attributeReader.GetString(nameof (Result));
      this.details = attributeReader.GetString(nameof (Details));
      this.showAlert = attributeReader.GetBoolean(nameof (ShowAlert));
      this.MarkAsClean();
    }

    public string Name => this.name;

    public string Result => this.result;

    public string Details => this.details;

    public bool ShowAlert
    {
      get => this.showAlert;
      set
      {
        this.showAlert = value;
        this.MarkAsDirty();
      }
    }

    public override PipelineInfo.Alert[] GetPipelineAlerts()
    {
      if (!this.showAlert)
        return (PipelineInfo.Alert[]) null;
      return new PipelineInfo.Alert[1]
      {
        new PipelineInfo.Alert(61, this.name, this.result, this.Date, this.Guid, this.Guid)
      };
    }

    public override string ToString() => this.name;

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) MIServiceMgicLog.XmlType);
      attributeWriter.Write("Name", (object) this.name);
      attributeWriter.Write("Result", (object) this.result);
      attributeWriter.Write("Details", (object) this.details);
      attributeWriter.Write("ShowAlert", (object) this.showAlert);
    }
  }
}
