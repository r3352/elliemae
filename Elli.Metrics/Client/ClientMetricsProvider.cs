// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.Client.ClientMetricsProvider
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace Elli.Metrics.Client
{
  internal class ClientMetricsProvider : MarshalByRefObject, IClientMetricsProvider
  {
    private static ClientMetricsProvider.DummyTimer _dummyTimer = new ClientMetricsProvider.DummyTimer();

    public override object InitializeLifetimeService() => (object) null;

    public void Initiate(
      string customer,
      string instance,
      string userId,
      string smartClientVersion,
      string apiToken = "gFrGKy5i9_bqmbUaeaAeOQ",
      int timeSpan = 1000,
      bool enabled = false)
    {
      MetricsFactory.Initiate(customer, instance, userId, apiToken, smartClientVersion, timeSpan, enabled);
    }

    public void IncrementCounter(string name)
    {
      try
      {
        MetricsFactory.IncrementCounter(name);
      }
      catch
      {
      }
    }

    public void IncrementCounter(string name, params SFxTag[] tags)
    {
      try
      {
        MetricsFactory.IncrementCounter(name, tags);
      }
      catch
      {
      }
    }

    public void IncrementCounter(string name, params string[] tags)
    {
      try
      {
        MetricsFactory.IncrementCounter(name, tags);
      }
      catch
      {
      }
    }

    public void IncrementErrorCounter(
      Exception ex,
      string description,
      [CallerFilePath] string sourceFilePath = "",
      [CallerMemberName] string member = "",
      [CallerLineNumber] int sourceLineNumber = -1)
    {
      try
      {
        MetricsFactory.IncrementErrorCounter(ex, description, sourceFilePath, member, sourceLineNumber);
      }
      catch
      {
      }
    }

    public IDisposable GetIncrementalTimer(string name)
    {
      try
      {
        return (IDisposable) new ClientMetricsProvider.TimerProxy(MetricsFactory.GetIncrementalTimer(name));
      }
      catch
      {
        return (IDisposable) ClientMetricsProvider._dummyTimer;
      }
    }

    public IDisposable GetIncrementalTimer(string name, params SFxTag[] tags)
    {
      try
      {
        return (IDisposable) new ClientMetricsProvider.TimerProxy(MetricsFactory.GetIncrementalTimer(name, tags));
      }
      catch
      {
        return (IDisposable) ClientMetricsProvider._dummyTimer;
      }
    }

    public IDisposable GetOldIncrementalTimer(string name, params SFxTag[] tags)
    {
      try
      {
        return (IDisposable) new ClientMetricsProvider.TimerProxy(MetricsFactory.GetIncrementalTimer(name, tags));
      }
      catch
      {
        return (IDisposable) ClientMetricsProvider._dummyTimer;
      }
    }

    public IDisposable GetIncrementalTimer(string name, params string[] tags)
    {
      try
      {
        return (IDisposable) new ClientMetricsProvider.TimerProxy(MetricsFactory.GetIncrementalTimer(name, tags));
      }
      catch
      {
        return (IDisposable) ClientMetricsProvider._dummyTimer;
      }
    }

    public void RecordIncrementalTimerSample(string name, TimeSpan time)
    {
      try
      {
        MetricsFactory.RecordIncrementalTimerSample(name, time);
      }
      catch
      {
      }
    }

    public void RecordIncrementalTimerSample(string name, TimeSpan time, params SFxTag[] tags)
    {
      try
      {
        MetricsFactory.RecordIncrementalTimerSample(name, time, tags);
      }
      catch
      {
      }
    }

    public void RecordIncrementalTimerSample(string name, TimeSpan time, params string[] tags)
    {
      try
      {
        MetricsFactory.RecordIncrementalTimerSample(name, time, tags);
      }
      catch
      {
      }
    }

    public void RecordIncrementalTimerSample(string name, long time)
    {
      try
      {
        MetricsFactory.RecordIncrementalTimerSample(name, time);
      }
      catch
      {
      }
    }

    public void RecordIncrementalTimerSample(string name, long time, params SFxTag[] tags)
    {
      try
      {
        MetricsFactory.RecordIncrementalTimerSample(name, time, tags);
      }
      catch
      {
      }
    }

    public void RecordIncrementalTimerSample(string name, long time, params string[] tags)
    {
      try
      {
        MetricsFactory.RecordIncrementalTimerSample(name, time, tags);
      }
      catch
      {
      }
    }

    public class TimerProxy : MarshalByRefObject, IDisposable
    {
      private IDisposable _timer { get; set; }

      public TimerProxy(IDisposable timer) => this._timer = timer;

      public void Dispose() => this._timer.Dispose();
    }

    public class DummyTimer : MarshalByRefObject, IDisposable
    {
      public void Dispose()
      {
      }
    }
  }
}
