// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServiceObjects.KafkaEvent.KafkaEventHandlerContextFactory
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server.ServiceObjects.KafkaEvent
{
  public class KafkaEventHandlerContextFactory : IDisposable
  {
    public static readonly KafkaEventHandlerContextFactory Instance = new KafkaEventHandlerContextFactory(KafkaUtils.UseAggregatedLogging);
    private readonly bool _aggregateLogs;
    private readonly AsyncLocal<ScopedKafkaEventHandlerContext> _wrapper = new AsyncLocal<ScopedKafkaEventHandlerContext>();

    public KafkaEventHandlerContextFactory(bool aggregateLogs)
    {
      this._aggregateLogs = aggregateLogs;
    }

    public IDisposable EnterScope(
      IClientContext context,
      string correlationId,
      Guid? transactionid)
    {
      return this._aggregateLogs ? (IDisposable) (this._wrapper.Value = new ScopedKafkaEventHandlerContext(this._wrapper.Value, context, correlationId, transactionid)) : (IDisposable) this;
    }

    public bool ExitScope(ScopedKafkaEventHandlerContext context)
    {
      ScopedKafkaEventHandlerContext eventHandlerContext = this._wrapper.Value;
      if (context != eventHandlerContext)
        return false;
      if (eventHandlerContext != null)
        this._wrapper.Value = eventHandlerContext._previous;
      return true;
    }

    public IKafkaEventHandlerContext Get()
    {
      return (this._aggregateLogs ? (IKafkaEventHandlerContext) this._wrapper.Value : (IKafkaEventHandlerContext) null) ?? (IKafkaEventHandlerContext) new SingleUseKafkaEventHandlerContext();
    }

    public void Dispose()
    {
    }
  }
}
