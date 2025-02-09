// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Listeners.LogListenerCollection
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Schema;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Encompass.Diagnostics.Logging.Listeners
{
  public class LogListenerCollection : IDisposable
  {
    private readonly Dictionary<string, ILogListener> _map;
    private readonly Dictionary<ILogListener, string> _reverseMap;

    public LogListenerCollection()
    {
      this._map = new Dictionary<string, ILogListener>();
      this._reverseMap = new Dictionary<ILogListener, string>();
    }

    public void AddListener(string key, ILogListener logListener)
    {
      lock (this)
      {
        if (this._map.ContainsKey(key) || this._reverseMap.ContainsKey(logListener))
          throw new InvalidOperationException("LogListener or Key already added.");
        this._map.Add(key, logListener);
        this._reverseMap.Add(logListener, key);
      }
    }

    public bool RemoveListener(ILogListener logListener, bool dispose)
    {
      lock (this)
      {
        string key;
        if (!this._reverseMap.TryGetValue(logListener, out key))
          return false;
        this._map.Remove(key);
        this._reverseMap.Remove(logListener);
        if (dispose)
        {
          try
          {
            logListener.Dispose();
          }
          catch
          {
          }
        }
        return true;
      }
    }

    public bool RemoveListener(string key, bool dispose)
    {
      return this.RemoveListener(key, dispose, out ILogListener _);
    }

    public bool RemoveListener(string key, bool dispose, out ILogListener logListener)
    {
      lock (this)
      {
        if (!this._map.TryGetValue(key, out logListener))
          return false;
        this._map.Remove(key);
        this._reverseMap.Remove(logListener);
        if (dispose)
        {
          try
          {
            logListener.Dispose();
          }
          catch
          {
          }
        }
        return true;
      }
    }

    public IEnumerable<ILogListener> GetListeners()
    {
      return (IEnumerable<ILogListener>) this._map.Values.ToList<ILogListener>();
    }

    public bool TryGetListener(string key, out ILogListener listener)
    {
      return this._map.TryGetValue(key, out listener);
    }

    public bool IsActiveFor(Log log)
    {
      return this._map.Values.ToList<ILogListener>().Any<ILogListener>((Func<ILogListener, bool>) (rule => rule.IsActiveFor(log)));
    }

    public void WriteToLogTargets(Log log)
    {
      foreach (ILogListener logListener in this._map.Values.ToList<ILogListener>())
        logListener.WriteToLogTargets(log);
    }

    public void Dispose()
    {
      lock (this)
      {
        foreach (ILogListener logListener in this._map.Values.ToList<ILogListener>())
          this.RemoveListener(logListener, true);
      }
    }
  }
}
