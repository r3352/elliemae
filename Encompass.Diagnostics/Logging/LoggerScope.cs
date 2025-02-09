// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.LoggerScope
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Listeners;
using Encompass.Diagnostics.Logging.Schema;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Encompass.Diagnostics.Logging
{
  public class LoggerScope : ILoggerScope, IDisposable, ILoggerScopeBuilder
  {
    private readonly ILoggerScope _parentScope = (ILoggerScope) null;
    private readonly LogListenerCollection _logListenerCollection;

    public LoggerScope(LoggerScope parentScope)
      : this()
    {
      if (parentScope != null)
      {
        this.Instance = parentScope.Instance;
        this.CorrelationId = parentScope.CorrelationId;
        this.TransactionId = parentScope.TransactionId;
      }
      this._parentScope = (ILoggerScope) parentScope;
    }

    public LoggerScope() => this._logListenerCollection = new LogListenerCollection();

    public string Instance { get; private set; }

    public Guid? TransactionId { get; private set; }

    public string CorrelationId { get; private set; }

    public virtual IEnumerable<ILogListener> GetListeners()
    {
      return this._parentScope == null ? this._logListenerCollection.GetListeners() : this._parentScope.GetListeners().Concat<ILogListener>(this._logListenerCollection.GetListeners());
    }

    public virtual void WriteToLogTargets(Log log)
    {
      this._parentScope?.WriteToLogTargets(log);
      this._logListenerCollection.WriteToLogTargets(log);
    }

    public virtual bool IsActiveFor(Log log)
    {
      ILoggerScope parentScope = this._parentScope;
      return (parentScope != null ? (parentScope.IsActiveFor(log) ? 1 : 0) : 0) != 0 || this._logListenerCollection.IsActiveFor(log);
    }

    public ILoggerScopeBuilder SetInstance(string instance)
    {
      this.Instance = instance;
      return (ILoggerScopeBuilder) this;
    }

    public ILoggerScopeBuilder SetTransactionId(Guid? transactionId)
    {
      this.TransactionId = transactionId;
      return (ILoggerScopeBuilder) this;
    }

    public ILoggerScopeBuilder SetCorrelationId(string correlationId)
    {
      this.CorrelationId = correlationId;
      return (ILoggerScopeBuilder) this;
    }

    public ILoggerScopeBuilder AddListener(string key, ILogListener logListener)
    {
      this._logListenerCollection.AddListener(key, logListener);
      return (ILoggerScopeBuilder) this;
    }

    public ILoggerScopeBuilder RemoveListener(ILogListener logListener, out bool result)
    {
      result = this._logListenerCollection.RemoveListener(logListener, true);
      return (ILoggerScopeBuilder) this;
    }

    public ILoggerScopeBuilder RemoveListener(ILogListener logListener)
    {
      return this.RemoveListener(logListener, out bool _);
    }

    public ILoggerScopeBuilder RemoveListener(
      string key,
      bool dispose,
      out bool result,
      out ILogListener logListener)
    {
      result = this._logListenerCollection.RemoveListener(key, dispose, out logListener);
      return (ILoggerScopeBuilder) this;
    }

    public ILoggerScopeBuilder RemoveListener(string key)
    {
      return this.RemoveListener(key, true, out bool _, out ILogListener _);
    }

    public void Dispose() => this._logListenerCollection.Dispose();
  }
}
