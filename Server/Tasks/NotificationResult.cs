// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Tasks.NotificationResult
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;

#nullable disable
namespace EllieMae.EMLite.Server.Tasks
{
  public class NotificationResult : IConcurrentUpdateNotificationResult
  {
    public bool IsNotificationSuppressed { get; set; }

    public bool IsSessionNotFound { get; set; }

    public bool NotificationFailed { get; set; }

    public string ErrorMessage { get; set; }

    public override string ToString()
    {
      if (this.NotificationFailed)
        return "Error encountered while sending notification: " + this.ErrorMessage;
      if (this.IsNotificationSuppressed)
        return "Notification suppressed since there is already an active notification present on the client.";
      return this.IsSessionNotFound ? "Notification was not sent as either the sessionid is not found on the current server." : "Notification sent.";
    }
  }
}
