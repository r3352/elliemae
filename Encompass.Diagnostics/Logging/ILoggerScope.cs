// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.ILoggerScope
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Listeners;
using Encompass.Diagnostics.Logging.Schema;
using System;
using System.Collections.Generic;

#nullable disable
namespace Encompass.Diagnostics.Logging
{
  public interface ILoggerScope : IDisposable
  {
    string Instance { get; }

    Guid? TransactionId { get; }

    string CorrelationId { get; }

    IEnumerable<ILogListener> GetListeners();

    void WriteToLogTargets(Log log);

    bool IsActiveFor(Log log);
  }
}
