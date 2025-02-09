// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.HtmlEmail.LoanStatusEmailTemplate
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.Server.Properties;
using System;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.HtmlEmail
{
  internal class LoanStatusEmailTemplate : IXmlSerializable
  {
    public string subject;
    public string emailBody;
    public string guid;

    public LoanStatusEmailTemplate()
    {
      this.subject = string.Empty;
      this.emailBody = string.Empty;
      this.guid = Guid.NewGuid().ToString();
    }

    public LoanStatusEmailTemplate(XmlSerializationInfo info)
    {
      this.guid = info.GetString("GUID");
      this.subject = info.GetString(nameof (subject));
      this.emailBody = info.GetString(nameof (emailBody));
    }

    public HtmlEmailTemplate MigrateData(string ownerID)
    {
      return new HtmlEmailTemplate(this.guid, ownerID)
      {
        Type = HtmlEmailTemplateType.StatusOnline,
        Subject = this.subject,
        Html = Resources.LoanStatusEmailTemplate.Replace("[[CustomBody]]", HttpUtility.HtmlEncode(this.emailBody))
      };
    }

    public void GetXmlObjectData(XmlSerializationInfo info) => throw new NotSupportedException();
  }
}
