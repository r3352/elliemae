// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServiceObjects.KafkaEvent.SingleUseKafkaEventHandlerContext
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServiceObjects.KafkaEvent
{
  public class SingleUseKafkaEventHandlerContext : IKafkaEventHandlerContext, IDisposable
  {
    private readonly ILogger _logger;

    public SingleUseKafkaEventHandlerContext()
    {
      this._logger = DiagUtility.LogManager.GetLogger("Kafka.Messages");
    }

    public void Add(KafkaEventResponseHandler handler)
    {
    }

    public void Complete(KafkaEventResponseHandler handler, KafkaEventLog log)
    {
      this._logger.Write<KafkaEventLog>(log);
    }

    public void Dispose()
    {
    }
  }
}
