// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.ConversationLog
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
  public class ConversationLog : LogRecordBase
  {
    public static readonly string XmlType = "Conv";
    private string userId = string.Empty;
    private string sessionUserName = string.Empty;
    private string name = string.Empty;
    private string company = string.Empty;
    private string phone = string.Empty;
    private string email = string.Empty;
    private bool inLog = true;
    private bool isEmail;
    private string newComments = string.Empty;

    public ConversationLog(DateTime date, string userId)
      : base(date, string.Empty)
    {
      this.userId = userId;
    }

    public ConversationLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.userId = attributeReader.GetString(nameof (UserId));
      this.name = attributeReader.GetString(nameof (Name));
      this.company = attributeReader.GetString(nameof (Company));
      this.phone = attributeReader.GetString(nameof (Phone));
      this.email = attributeReader.GetString(nameof (Email));
      this.inLog = attributeReader.GetBoolean("InLog");
      this.isEmail = attributeReader.GetBoolean(nameof (IsEmail));
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

    public string Name
    {
      get => this.name;
      set
      {
        this.name = value;
        this.MarkAsDirty();
      }
    }

    public string Company
    {
      get => this.company;
      set
      {
        this.company = value;
        this.MarkAsDirty();
      }
    }

    public string Phone
    {
      get => this.phone;
      set
      {
        this.phone = value;
        this.MarkAsDirty();
      }
    }

    public string Email
    {
      get => this.email;
      set
      {
        this.email = value;
        this.MarkAsDirty();
      }
    }

    public override bool DisplayInLog
    {
      get => this.inLog;
      set
      {
        this.inLog = value;
        this.MarkAsDirty();
      }
    }

    public bool IsEmail
    {
      get => this.isEmail;
      set
      {
        this.isEmail = value;
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
      return this.AlertList.ToPipelineAlerts(StandardAlertID.ConversationFollowUp, this.Name);
    }

    internal override bool IncludeInLog() => base.IncludeInLog() && this.inLog;

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) ConversationLog.XmlType);
      attributeWriter.Write("UserId", (object) this.userId);
      attributeWriter.Write("Name", (object) this.name);
      attributeWriter.Write("Company", (object) this.company);
      attributeWriter.Write("Phone", (object) this.phone);
      attributeWriter.Write("Email", (object) this.email);
      attributeWriter.Write("InLog", (object) this.inLog);
      attributeWriter.Write("IsEmail", (object) this.isEmail);
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
