// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.Caching.InMemoryCache
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using Elli.Service.Common.Caching.Timers;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.Service.Common.Caching
{
  public class InMemoryCache : ICache
  {
    private readonly object _monitor = new object();
    private Dictionary<Request, Response> _cachedResponses;
    private Dictionary<ITimer, Request> _timersToRequests;
    private readonly ITimerProvider _timerProvider;

    public InMemoryCache(ITimerProvider timerProvider)
    {
      this._timerProvider = timerProvider;
      this.Initialize();
    }

    private void Initialize()
    {
      this._cachedResponses = new Dictionary<Request, Response>();
      this._timersToRequests = new Dictionary<ITimer, Request>();
    }

    public Response GetCachedResponseFor(Request request)
    {
      lock (this._monitor)
        return !this._cachedResponses.ContainsKey(request) ? (Response) null : this._cachedResponses[request];
    }

    public void Store(Request request, Response response, TimeSpan expiration)
    {
      ITimer timer = this._timerProvider.GetTimer(expiration.TotalMilliseconds);
      lock (this._monitor)
      {
        if (this._cachedResponses.ContainsKey(request))
        {
          this._cachedResponses.Remove(request);
          ITimer key = this._timersToRequests.First<KeyValuePair<ITimer, Request>>((Func<KeyValuePair<ITimer, Request>, bool>) (keyValuePair => keyValuePair.Value.Equals((object) request))).Key;
          this.GetRidOfTimer(key);
          this._timersToRequests.Remove(key);
        }
        this._cachedResponses[request] = response;
        this._timersToRequests[timer] = request;
      }
      timer.Elapsed += new EventHandler(this.timer_Elapsed);
      timer.Start();
    }

    private void timer_Elapsed(object sender, EventArgs eventArgs)
    {
      ITimer key = (ITimer) sender;
      lock (this._monitor)
      {
        if (this._timersToRequests.ContainsKey(key))
        {
          this._cachedResponses.Remove(this._timersToRequests[key]);
          this._timersToRequests.Remove(key);
        }
      }
      key.Dispose();
    }

    public void Clear()
    {
      lock (this._monitor)
      {
        foreach (ITimer key in this._timersToRequests.Keys)
          this.GetRidOfTimer(key);
        this.Initialize();
      }
    }

    private void GetRidOfTimer(ITimer timer)
    {
      timer.Elapsed -= new EventHandler(this.timer_Elapsed);
      timer.Stop();
      timer.Dispose();
    }
  }
}
