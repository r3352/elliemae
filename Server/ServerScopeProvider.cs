// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerScopeProvider
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Encompass.Diagnostics.Logging;
using System;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class ServerScopeProvider : ILoggerScopeProvider, IDisposable
  {
    private readonly LoggerScope _globalScope;

    public ServerScopeProvider()
    {
      this._globalScope = new LoggerScope();
      this._globalScope.SetInstance("Core");
    }

    public ILoggerScope GetCurrent()
    {
      return (ILoggerScope) ClientContext.CurrentRequest ?? (ILoggerScope) this._globalScope;
    }

    public ILoggerScope GetGlobal() => (ILoggerScope) this._globalScope;

    public IDisposable EnterNew(Action<ILoggerScopeBuilder> setupScope) => (IDisposable) null;

    public void ExitCurrent()
    {
    }

    public void ModifyGlobal(Action<ILoggerScopeBuilder> setupScope)
    {
      if (setupScope == null)
        return;
      setupScope((ILoggerScopeBuilder) this._globalScope);
    }

    public void Dispose()
    {
      foreach (ClientContext clientContext in ClientContext.GetAll())
        clientContext.RemoveConfigChangeHandler();
      this._globalScope.Dispose();
    }
  }
}
