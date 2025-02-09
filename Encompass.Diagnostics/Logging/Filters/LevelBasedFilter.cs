// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Filters.LevelBasedFilter
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Schema;
using System;
using System.Collections.Generic;

#nullable disable
namespace Encompass.Diagnostics.Logging.Filters
{
  public class LevelBasedFilter : ILogFilter
  {
    private const string DEFAULT_SWITCH = "*";
    private const LogLevelFilter DEFAULT_LEVEL = LogLevelFilter.Error;
    private readonly Dictionary<string, LogLevelFilter> _logLevels;
    private readonly string _defaultSwitch;

    public LevelBasedFilter(
      IDictionary<string, LogLevelFilter> logLevels,
      string defaultLevelSwitch = "*",
      LogLevelFilter defaultLevel = LogLevelFilter.Error)
    {
      this._defaultSwitch = defaultLevelSwitch;
      this._logLevels = new Dictionary<string, LogLevelFilter>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      if (logLevels != null)
      {
        foreach (KeyValuePair<string, LogLevelFilter> logLevel in (IEnumerable<KeyValuePair<string, LogLevelFilter>>) logLevels)
        {
          if (!this._logLevels.ContainsKey(logLevel.Key))
            this._logLevels[logLevel.Key] = logLevel.Value;
        }
      }
      if (this._logLevels.ContainsKey(defaultLevelSwitch))
        return;
      this._logLevels[defaultLevelSwitch] = defaultLevel;
    }

    public bool IsActiveFor(Log log)
    {
      string key = string.IsNullOrEmpty(log.Logger) || !this._logLevels.ContainsKey(log.Logger) ? this._defaultSwitch : log.Logger;
      return (log.Level & (Encompass.Diagnostics.Logging.LogLevel) this._logLevels[key]) != 0;
    }
  }
}
