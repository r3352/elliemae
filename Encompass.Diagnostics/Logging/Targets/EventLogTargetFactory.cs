// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Targets.EventLogTargetFactory
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Events;
using Encompass.Diagnostics.Logging.Formatters;

#nullable disable
namespace Encompass.Diagnostics.Logging.Targets
{
  public class EventLogTargetFactory : LogTargetFactoryBase, ILogTargetFactory
  {
    private readonly ILogFormatter _formatter;
    private readonly IApplicationEventHandler _eventHandler;

    public EventLogTargetFactory(
      ILogManager logManager,
      ILogFormatter formatter,
      IApplicationEventHandler eventHandler)
      : base(logManager)
    {
      this._formatter = ArgumentChecks.IsNotNull<ILogFormatter>(formatter, nameof (formatter));
      this._eventHandler = ArgumentChecks.IsNotNull<IApplicationEventHandler>(eventHandler, nameof (eventHandler));
    }

    public EventLogTargetFactory(
      ILogManager logManager,
      ILogFormatter formatter,
      IApplicationEventHandler eventHandler,
      IAsyncTargetWrapperFactory asyncTargetWrapperFactory)
      : base(logManager, asyncTargetWrapperFactory)
    {
      this._formatter = ArgumentChecks.IsNotNull<ILogFormatter>(formatter, nameof (formatter));
      this._eventHandler = ArgumentChecks.IsNotNull<IApplicationEventHandler>(eventHandler, nameof (eventHandler));
    }

    protected override ILogTarget GetTargetInternal()
    {
      return (ILogTarget) new EventLogTarget(this._logManager.AppName, this._eventHandler, this._formatter);
    }
  }
}
