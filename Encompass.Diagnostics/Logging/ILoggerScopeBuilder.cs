// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.ILoggerScopeBuilder
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Listeners;
using System;

#nullable disable
namespace Encompass.Diagnostics.Logging
{
  public interface ILoggerScopeBuilder : ILoggerScope, IDisposable
  {
    ILoggerScopeBuilder SetInstance(string instance);

    ILoggerScopeBuilder SetTransactionId(Guid? transactionId);

    ILoggerScopeBuilder SetCorrelationId(string correlationId);

    ILoggerScopeBuilder AddListener(string key, ILogListener logListener);

    ILoggerScopeBuilder RemoveListener(ILogListener logListener, out bool result);

    ILoggerScopeBuilder RemoveListener(ILogListener logListener);

    ILoggerScopeBuilder RemoveListener(
      string key,
      bool dispose,
      out bool result,
      out ILogListener logListener);

    ILoggerScopeBuilder RemoveListener(string key);
  }
}
