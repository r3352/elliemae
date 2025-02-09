// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.ThreadStaticScopeProvider`1
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using System;

#nullable disable
namespace Encompass.Diagnostics.Logging
{
  public class ThreadStaticScopeProvider<T> : ILoggerScopeProvider, IDisposable where T : ThreadStaticScopeProvider<T>
  {
    [ThreadStatic]
    private static ThreadStaticScopeProvider<T>.ScopeWrapper CurrentWrapper;
    private static LoggerScope _globalScope = new LoggerScope();

    static ThreadStaticScopeProvider()
    {
      ThreadStaticScopeProvider<T>._globalScope.SetInstance(string.Empty);
    }

    public ILoggerScope GetCurrent() => (ILoggerScope) this.GetCurrentScope();

    private LoggerScope GetCurrentScope()
    {
      return ThreadStaticScopeProvider<T>.CurrentWrapper?.Scope ?? ThreadStaticScopeProvider<T>._globalScope;
    }

    public ILoggerScope GetGlobal() => (ILoggerScope) ThreadStaticScopeProvider<T>._globalScope;

    public void ModifyGlobal(Action<ILoggerScopeBuilder> setupScope)
    {
      if (setupScope == null)
        return;
      setupScope((ILoggerScopeBuilder) ThreadStaticScopeProvider<T>._globalScope);
    }

    public IDisposable EnterNew(Action<ILoggerScopeBuilder> setupScope)
    {
      this.GetCurrent();
      LoggerScope scope = new LoggerScope(this.GetCurrentScope());
      if (setupScope != null)
        setupScope((ILoggerScopeBuilder) scope);
      return (IDisposable) new ThreadStaticScopeProvider<T>.ScopeWrapper(scope);
    }

    public void ExitCurrent() => ThreadStaticScopeProvider<T>.CurrentWrapper?.Dispose();

    public void Dispose() => ThreadStaticScopeProvider<T>._globalScope.Dispose();

    private class ScopeWrapper : IDisposable
    {
      private ThreadStaticScopeProvider<T>.ScopeWrapper _parent;

      public LoggerScope Scope { get; }

      public ScopeWrapper(LoggerScope scope)
      {
        this.Scope = scope;
        this._parent = ThreadStaticScopeProvider<T>.CurrentWrapper;
        ThreadStaticScopeProvider<T>.CurrentWrapper = this;
      }

      public void Dispose()
      {
        this.Scope.Dispose();
        ThreadStaticScopeProvider<T>.CurrentWrapper = this._parent;
        this._parent = (ThreadStaticScopeProvider<T>.ScopeWrapper) null;
      }
    }
  }
}
