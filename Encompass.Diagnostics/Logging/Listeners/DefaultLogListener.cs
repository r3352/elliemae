// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Listeners.DefaultLogListener
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Events;
using Encompass.Diagnostics.Logging.Filters;
using Encompass.Diagnostics.Logging.Targets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Encompass.Diagnostics.Logging.Listeners
{
  public class DefaultLogListener : ILogListener, IDisposable
  {
    private readonly ILogFilter _logFilter;
    private readonly IApplicationEventHandler _eventHandler;
    private readonly Dictionary<string, ILogTarget> _targetMap;
    private readonly Dictionary<ILogTarget, string> _reverseTargetMap;
    private bool disposed = false;

    public DefaultLogListener(ILogFilter logFilter, IApplicationEventHandler eventHandler)
    {
      this._eventHandler = ArgumentChecks.IsNotNull<IApplicationEventHandler>(eventHandler, nameof (eventHandler));
      this._targetMap = new Dictionary<string, ILogTarget>();
      this._reverseTargetMap = new Dictionary<ILogTarget, string>();
      this._logFilter = logFilter;
    }

    public void AddTarget(string key, ILogTarget target)
    {
      lock (this)
      {
        if (this._targetMap.ContainsKey(key) || this._reverseTargetMap.ContainsKey(target))
          throw new InvalidOperationException("LogListener or Key already added.");
        this._targetMap.Add(key, target);
        this._reverseTargetMap.Add(target, key);
      }
    }

    public bool RemoveTarget(ILogTarget target, bool dispose)
    {
      lock (this)
      {
        string key;
        if (!this._reverseTargetMap.TryGetValue(target, out key))
          return false;
        this._targetMap.Remove(key);
        this._reverseTargetMap.Remove(target);
        if (dispose)
        {
          try
          {
            target.Dispose();
          }
          catch
          {
          }
        }
        return true;
      }
    }

    public bool RemoveTarget(string key, bool dispose)
    {
      lock (this)
      {
        ILogTarget key1;
        if (!this._targetMap.TryGetValue(key, out key1))
          return false;
        this._targetMap.Remove(key);
        this._reverseTargetMap.Remove(key1);
        if (dispose)
        {
          try
          {
            key1.Dispose();
          }
          catch
          {
          }
        }
        return true;
      }
    }

    public IEnumerable<ILogTarget> GetLogTargets(Encompass.Diagnostics.Logging.Schema.Log log)
    {
      return this._logFilter.IsActiveFor(log) ? (IEnumerable<ILogTarget>) this._targetMap.Values : Enumerable.Empty<ILogTarget>();
    }

    public bool IsActiveFor(Encompass.Diagnostics.Logging.Schema.Log log)
    {
      return this._logFilter.IsActiveFor(log);
    }

    public void WriteToLogTargets(Encompass.Diagnostics.Logging.Schema.Log log)
    {
      if (!this.IsActiveFor(log))
        return;
      foreach (ILogTarget logTarget in this._targetMap.Values)
      {
        try
        {
          logTarget.Write(log);
          logTarget.Flush();
        }
        catch (Exception ex)
        {
          this._eventHandler.WriteApplicationEvent("Exception while writing log message: " + ex.GetFullStackTrace(), EventLogEntryType.Error, 1100);
          throw ex;
        }
      }
    }

    public void Dispose()
    {
      if (this.disposed)
        return;
      lock (this)
      {
        if (!this.disposed)
        {
          foreach (ILogTarget logTarget in this._targetMap.Values)
          {
            try
            {
              logTarget.Dispose();
            }
            catch (Exception ex)
            {
              this._eventHandler.WriteApplicationEvent("Exception while disposing the target: " + ex.GetFullStackTrace(), EventLogEntryType.Error, 1103);
              throw ex;
            }
          }
          this.disposed = true;
        }
      }
    }
  }
}
