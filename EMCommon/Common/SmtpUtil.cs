// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.SmtpUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Net;
using System.Net.Mail;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class SmtpUtil
  {
    public static void SendMail(
      string smtpHost,
      int smtpPort,
      string smtpUserName,
      string smtpPassword,
      string fromEmail,
      string toEmails,
      string subject,
      string body,
      bool isBodyHtml,
      bool enableSSL)
    {
      MailMessage message = new MailMessage();
      message.From = new MailAddress(fromEmail);
      string str = toEmails;
      char[] separator = new char[2]{ ',', ';' };
      foreach (string addresses in str.Split(separator, StringSplitOptions.RemoveEmptyEntries))
        message.To.Add(addresses);
      message.Subject = subject;
      message.Body = body;
      message.IsBodyHtml = isBodyHtml;
      SmtpClient smtpClient = new SmtpClient();
      smtpClient.Host = smtpHost;
      if (smtpPort > 0)
        smtpClient.Port = smtpPort;
      smtpClient.EnableSsl = enableSSL;
      if ((smtpUserName ?? "").Trim() != "")
        smtpClient.Credentials = (ICredentialsByHost) new NetworkCredential(smtpUserName, smtpPassword);
      smtpClient.Send(message);
    }
  }
}
