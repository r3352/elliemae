// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Listeners.ILogListener
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Schema;
using Encompass.Diagnostics.Logging.Targets;
using System;
using System.Collections.Generic;

#nullable disable
namespace Encompass.Diagnostics.Logging.Listeners
{
  public interface ILogListener : IDisposable
  {
    bool IsActiveFor(Log log);

    void WriteToLogTargets(Log log);

    IEnumerable<ILogTarget> GetLogTargets(Log log);

    void AddTarget(string key, ILogTarget logTarget);

    bool RemoveTarget(ILogTarget logTarget, bool dispose);

    bool RemoveTarget(string name, bool dispose);
  }
}
