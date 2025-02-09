// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Tasks.TradeLoanUpdateNotificationPayload
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.MessageServices.Message;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.Tasks
{
  public class TradeLoanUpdateNotificationPayload
  {
    public string BatchJobId { get; set; }

    public string BatchJobStatus { get; set; }

    public BatchJobResult BatchJobResult { get; set; }

    public string TradeId { get; set; }

    public string TradeStatus { get; set; }

    public string SessionId { get; set; }

    public string CorrealtionId { get; set; }

    public string InstanceId { get; set; }

    public DateTime PublishTime { get; set; }

    public DateTime NotificationTime { get; set; }
  }
}
