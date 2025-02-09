// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.InstanceLogConfigChangeHandler
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Encompass.Diagnostics.Config;
using Encompass.Diagnostics.Logging.Listeners;
using System;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class InstanceLogConfigChangeHandler : 
    IDiagConfigChangeHandler<ServerDiagConfigData>,
    IDisposable
  {
    private LogListenerCollection _logListenerCollection;
    private ClientContext _context;

    public InstanceLogConfigChangeHandler(
      LogListenerCollection logListenerCollection,
      ClientContext context)
    {
      this._logListenerCollection = logListenerCollection;
      this._context = context;
    }

    public bool PerformsGlobalCleanup => false;

    public bool PerformsCleanup => true;

    public bool PerformsGlobalSetup => false;

    public bool PerformsSetup => true;

    public void GlobalCleanup(ServerDiagConfigData configData)
    {
    }

    public void Cleanup(ServerDiagConfigData config)
    {
      ILogListener listener;
      if (!this._logListenerCollection.TryGetListener(config.LogListeners.ClassicLog.Name, out listener))
        return;
      ClientContextLogFactories.GetLogFactory<ClassicLogFactory>().RemoveTargets(listener);
      this._logListenerCollection.RemoveListener(config.LogListeners.ClassicLog.Name, true);
    }

    public void GlobalSetup(ServerDiagConfigData config, bool refresh)
    {
    }

    public void Setup(ServerDiagConfigData config, bool refresh)
    {
      if (!config.LogListeners.ClassicLog.Enabled)
        return;
      this._logListenerCollection.AddListener(config.LogListeners.ClassicLog.Name, ClientContextLogFactories.GetLogFactory<ClassicLogFactory>().GetLogListener(this._context, refresh));
    }

    public void Dispose()
    {
    }
  }
}
