// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Diagnostics.ConfigChangeHandlers.RemotingLogConfigChangeHandler
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using Encompass.Diagnostics;
using Encompass.Diagnostics.Config;
using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.Logging.Filters;
using Encompass.Diagnostics.Logging.Listeners;
using Encompass.Diagnostics.Logging.Targets;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Common.Diagnostics.ConfigChangeHandlers
{
  public class RemotingLogConfigChangeHandler : 
    IDiagConfigChangeHandler<ClientDiagConfigData>,
    IDisposable
  {
    private readonly IRemotingLogConsumer _remotingLogConsumer;
    private readonly IAsyncTargetWrapperFactory _asyncTargetWrapperFactory;

    public RemotingLogConfigChangeHandler(IRemotingLogConsumer remotingLogConsumer)
    {
      this._remotingLogConsumer = remotingLogConsumer;
      this._asyncTargetWrapperFactory = (IAsyncTargetWrapperFactory) new AsyncTargetWrapperFactory(DiagUtility.ApplicationEventHandler, 20, 500, 30000);
    }

    public bool PerformsGlobalCleanup => false;

    public bool PerformsCleanup => true;

    public bool PerformsGlobalSetup => false;

    public bool PerformsSetup => true;

    public void GlobalCleanup(ClientDiagConfigData configData)
    {
    }

    public void Cleanup(ClientDiagConfigData configData)
    {
      DiagUtility.LoggerScopeProvider.ModifyGlobal((Action<ILoggerScopeBuilder>) (globalScope => globalScope.RemoveListener(configData.LogListeners.RemotingLog.Name)));
    }

    public void GlobalSetup(ClientDiagConfigData configData, bool refresh)
    {
    }

    public void Setup(ClientDiagConfigData configData, bool refresh)
    {
      if (!configData.LogListeners.RemotingLog.Enabled)
        return;
      DefaultLogListener serverLogListener = new DefaultLogListener((ILogFilter) new LevelBasedFilter((IDictionary<string, LogLevelFilter>) configData.LogListeners.RemotingLog.LogLevels, configData.LogListeners.RemotingLog.DefaultLogLevelName), DiagUtility.ApplicationEventHandler);
      serverLogListener.AddTarget(configData.LogListeners.RemotingLog.TargetName, this._asyncTargetWrapperFactory.WrapTarget((ILogTarget) new RemotingLogTarget(this._remotingLogConsumer)));
      DiagUtility.LoggerScopeProvider?.ModifyGlobal((Action<ILoggerScopeBuilder>) (globalScope => globalScope.AddListener(configData.LogListeners.RemotingLog.Name, (ILogListener) serverLogListener)));
    }

    public void Dispose() => this._asyncTargetWrapperFactory.Dispose();
  }
}
