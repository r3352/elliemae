// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.GlobalLogConfigChangeHandler
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Encompass.Diagnostics;
using Encompass.Diagnostics.Config;
using Encompass.Diagnostics.Events;
using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.Logging.Listeners;
using Encompass.Diagnostics.Logging.Targets;
using System;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class GlobalLogConfigChangeHandler : 
    IDiagConfigChangeHandler<ServerDiagConfigData>,
    IDisposable
  {
    private readonly IAsyncTargetWrapperFactory _asyncTargetWrapperFactory;

    public GlobalLogConfigChangeHandler()
      : this(DiagUtility.ApplicationEventHandler)
    {
    }

    public GlobalLogConfigChangeHandler(IApplicationEventHandler eventHandler)
    {
      this._asyncTargetWrapperFactory = (IAsyncTargetWrapperFactory) new AsyncTargetWrapperFactory(eventHandler, 1, -1, -1);
    }

    public bool PerformsGlobalCleanup => true;

    public bool PerformsCleanup => true;

    public bool PerformsGlobalSetup => true;

    public bool PerformsSetup => true;

    public void GlobalCleanup(ServerDiagConfigData configData)
    {
      if (configData.LogListeners.ClassicLog.Enabled)
        ClientContextLogFactories.GetLogFactory<ClassicLogFactory>().DisposeTargets();
      ClientContextLogFactories.Clear();
    }

    public void Cleanup(ServerDiagConfigData configData)
    {
      DiagUtility.LoggerScopeProvider.ModifyGlobal((Action<ILoggerScopeBuilder>) (globalScope =>
      {
        bool result;
        ILogListener logListener;
        globalScope.RemoveListener(configData.LogListeners.ClassicLog.Name, false, out result, out logListener);
        if (!result)
          return;
        ClientContextLogFactories.GetLogFactory<ClassicLogFactory>().RemoveTargets(logListener);
        logListener.Dispose();
      }));
    }

    public void GlobalSetup(ServerDiagConfigData configData, bool refresh)
    {
      if (!configData.LogListeners.ClassicLog.Enabled)
        return;
      ClientContextLogFactories.AddLogFactory<ClassicLogFactory>(new ClassicLogFactory(DiagUtility.LogManager, DiagUtility.ApplicationEventHandler, this._asyncTargetWrapperFactory, configData));
    }

    public void Setup(ServerDiagConfigData configData, bool refresh)
    {
      DiagUtility.LoggerScopeProvider.ModifyGlobal((Action<ILoggerScopeBuilder>) (globalScope =>
      {
        if (!configData.LogListeners.ClassicLog.Enabled)
          return;
        globalScope.AddListener(configData.LogListeners.ClassicLog.Name, ClientContextLogFactories.GetLogFactory<ClassicLogFactory>().GetLogListener((ClientContext) null, refresh));
      }));
    }

    public void Dispose() => this._asyncTargetWrapperFactory.Dispose();
  }
}
