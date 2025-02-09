// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.EmailTriggerLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class EmailTriggerLog : LogRecordBase
  {
    public static readonly string XmlType = "EmailTrigger";
    private string subject;
    private string body;
    private string senderId;
    private string[] recipientIds;
    private bool displayInLog;

    public EmailTriggerLog(
      string subject,
      string body,
      string senderId,
      string[] recipients,
      bool displayInLog)
      : base(DateTime.Now, "")
    {
      this.subject = subject;
      this.body = body;
      this.senderId = senderId;
      this.recipientIds = recipients;
      this.displayInLog = displayInLog;
    }

    public EmailTriggerLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.subject = attributeReader.GetString(nameof (Subject));
      this.body = attributeReader.GetString(nameof (Body));
      this.senderId = attributeReader.GetString("Sender");
      this.recipientIds = attributeReader.GetString("Recipients").Split(';');
      this.displayInLog = attributeReader.GetBoolean("InLog", true);
      this.MarkAsClean();
    }

    public string Subject => this.subject;

    public string Body => this.body;

    public string SenderUserID => this.senderId;

    public string[] RecipientUserIDs => this.recipientIds;

    public override bool DisplayInLog
    {
      get => this.displayInLog;
      set => this.displayInLog = value;
    }

    public override bool IsLoanOperationalLog => true;

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) EmailTriggerLog.XmlType);
      attributeWriter.Write("Subject", (object) this.Subject);
      attributeWriter.Write("Body", (object) this.Body);
      attributeWriter.Write("Sender", (object) this.SenderUserID);
      attributeWriter.Write("Recipients", (object) string.Join(";", this.RecipientUserIDs));
      attributeWriter.Write("InLog", (object) this.displayInLog);
    }
  }
}
