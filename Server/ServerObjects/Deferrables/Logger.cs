// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.Logger
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Common.Contact;
using Encompass.Diagnostics;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables
{
  public class Logger : Elli.MessageQueues.Loggers.ILogger
  {
    private const string ClassName = "Deferrables.Logger�";
    private static readonly string _environmentName = ConfigurationManager.AppSettings["EnvironmentName"] ?? "";

    public bool IsDebugEnable { get; set; }

    static Logger()
    {
      if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["EnableMetrics"]))
        return;
      string appSetting = ConfigurationManager.AppSettings["EnableMetrics"];
    }

    public void DebugFormat(string format, params object[] args)
    {
      Encompass.Diagnostics.Logging.ILogger logger = DiagUtility.DefaultLogger;
      logger.When(Encompass.Diagnostics.Logging.LogLevel.DEBUG, (Action) (() => logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, "Deferrables.Logger", string.Format(format, args))));
    }

    public void InfoFormat(string format, params object[] args)
    {
      Encompass.Diagnostics.Logging.ILogger logger = DiagUtility.DefaultLogger;
      logger.When(Encompass.Diagnostics.Logging.LogLevel.INFO, (Action) (() => logger.Write(Encompass.Diagnostics.Logging.LogLevel.INFO, "Deferrables.Logger", string.Format(format, args))));
    }

    public void WarnFormat(string format, params object[] args)
    {
      string machineName = string.Format("From {0}, ", (object) Environment.MachineName);
      string message = string.Format(format, args);
      DiagUtility.DefaultLogger.Write(Encompass.Diagnostics.Logging.LogLevel.WARN, "Deferrables.Logger", message);
      Task.Run((Action) (() => Logger.SendEmail("WARNING", machineName + message)));
    }

    public void ErrorFormat(string format, params object[] args)
    {
      string machineName = string.Format("From {0}, ", (object) Environment.MachineName);
      string message = string.Format(format, args);
      DiagUtility.DefaultLogger.Write(Encompass.Diagnostics.Logging.LogLevel.ERROR, "Deferrables.Logger", message);
      Task.Run((Action) (() => Logger.SendEmail("ERROR", machineName + message)));
    }

    public void Error(Exception exception)
    {
      string error = string.Format("From {0}, ", (object) Environment.MachineName) + exception.ToString();
      DiagUtility.DefaultLogger.Write(Encompass.Diagnostics.Logging.LogLevel.ERROR, "Deferrables.Logger", exception);
      Task.Run((Action) (() => Logger.SendEmail("ERROR", error)));
    }

    private static void SendEmail(string subject, string body)
    {
      try
      {
        subject = string.Format("{0}:{1}-{2}", (object) Logger._environmentName, (object) subject, (object) "Deferrables.Logger");
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["EBSMessageSender"], ConfigurationManager.AppSettings["EBSMessageSender"]);
        string appSetting = ConfigurationManager.AppSettings["EBSMessageRecipients"];
        if (string.IsNullOrWhiteSpace(appSetting))
          return;
        foreach (string str in appSetting.Split(";".ToCharArray()))
        {
          if (!string.IsNullOrWhiteSpace(str))
            mailMessage.To.Add(new MailAddress(str, str));
        }
        mailMessage.Subject = subject;
        mailMessage.Body = body;
        ContactUtils.Send(mailMessage);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError("Deferrables.Logger", ex.ToString());
      }
    }
  }
}
