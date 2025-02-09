// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Tasks.ConcurrentNotificationPayload
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Server.Tasks
{
  public class ConcurrentNotificationPayload
  {
    public string UserId { get; set; }

    public string LoanId { get; set; }

    public string SessionId { get; set; }

    public string CorrealtionId { get; set; }

    public string InstanceName { get; set; }

    public DateTime NotificationTime { get; set; }
  }
}
