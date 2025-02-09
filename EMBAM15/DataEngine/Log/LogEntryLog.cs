// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.LogEntryLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Xml;
using System;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class LogEntryLog : LogRecordBase
  {
    public static readonly string XmlType = "Log";
    private string userId = string.Empty;
    private string sessionUserName = string.Empty;
    private string description = string.Empty;
    private string newComments = string.Empty;

    public LogEntryLog(DateTime date, string userId)
      : base(date, string.Empty)
    {
      this.userId = userId;
    }

    public LogEntryLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.userId = attributeReader.GetString(nameof (UserId));
      this.description = attributeReader.GetString("Desc");
    }

    public string UserId => this.userId;

    public string SessionUserName
    {
      get => this.sessionUserName;
      set
      {
        this.sessionUserName = value;
        this.MarkAsDirty();
      }
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

    public string NewComments
    {
      get => this.newComments;
      set
      {
        this.newComments = value;
        this.MarkAsDirty();
      }
    }

    public override PipelineInfo.Alert[] GetPipelineAlerts()
    {
      return this.AlertList.ToPipelineAlerts(StandardAlertID.TaskFollowUp, this.Description);
    }

    internal override bool IncludeInLog()
    {
      return base.IncludeInLog() && this.AlertList.GetMostCriticalAlert() == null;
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) LogEntryLog.XmlType);
      attributeWriter.Write("UserId", (object) this.userId);
      attributeWriter.Write("Desc", (object) this.description);
      attributeWriter.Write("Comments", (object) this.getMergedComments());
    }

    private string getMergedComments()
    {
      StringBuilder sb = new StringBuilder();
      if (string.Empty != this.NewComments || this.AlertList.DidUserFollowUp())
      {
        sb.Append(DateTime.Now.ToString("MM/dd/yy h:mm tt ") + "(" + Utils.CurrentTimeZoneName + ") " + this.SessionUserName);
        if (this.AlertList.DidUserFollowUp())
          sb.Append(" Followed Up");
        sb.Append(" > " + this.NewComments + "\r\n");
        if (string.Empty != this.Comments)
          sb.Append("----------------------------------------\r\n");
      }
      sb.Append(this.Comments);
      return sb.ReplaceWordChars().ToString();
    }
  }
}
