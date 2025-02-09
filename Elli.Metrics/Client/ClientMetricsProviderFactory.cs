// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.Client.ClientMetricsProviderFactory
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace Elli.Metrics.Client
{
  public static class ClientMetricsProviderFactory
  {
    private static ClientMetricsProviderFactory.DummyTimer _dummyTimer = new ClientMetricsProviderFactory.DummyTimer();

    private static IClientMetricsProvider _instance { get; set; }

    public static IClientMetricsProvider GetClientMetricsProvider()
    {
      return (IClientMetricsProvider) null;
    }

    public static void Initiate(
      string customer,
      string instance,
      string userId,
      string ClientVersion,
      bool enabled = false,
      string apiToken = "gFrGKy5i9_bqmbUaeaAeOQ",
      int timeSpan = 1000)
    {
      enabled = false;
      if (!enabled)
      {
        ClientMetricsProviderFactory._instance = (IClientMetricsProvider) null;
      }
      else
      {
        try
        {
          if (ClientMetricsProviderFactory._instance == null)
            ClientMetricsProviderFactory.GetClientMetricsProvider();
          if (ClientMetricsProviderFactory._instance == null)
            return;
          ClientMetricsProviderFactory._instance.Initiate(customer, instance, userId, ClientVersion, apiToken, timeSpan, enabled);
        }
        catch
        {
          ClientMetricsProviderFactory._instance = (IClientMetricsProvider) null;
        }
      }
    }

    public static void IncrementCounter(string name)
    {
      if (ClientMetricsProviderFactory._instance == null)
        return;
      try
      {
        ClientMetricsProviderFactory._instance.IncrementCounter(name);
      }
      catch
      {
      }
    }

    public static void IncrementCounter(string name, params SFxTag[] tags)
    {
      if (ClientMetricsProviderFactory._instance == null)
        return;
      try
      {
        ClientMetricsProviderFactory._instance.IncrementCounter(name, tags);
      }
      catch
      {
      }
    }

    public static void IncrementCounter(string name, params string[] tags)
    {
      if (ClientMetricsProviderFactory._instance == null)
        return;
      try
      {
        ClientMetricsProviderFactory._instance.IncrementCounter(name, tags);
      }
      catch
      {
      }
    }

    public static void IncrementErrorCounter(
      Exception ex,
      string description,
      [CallerFilePath] string sourceFilePath = "",
      [CallerMemberName] string member = "",
      [CallerLineNumber] int sourceLineNumber = -1)
    {
      if (ClientMetricsProviderFactory._instance == null)
        return;
      try
      {
        ClientMetricsProviderFactory._instance.IncrementErrorCounter(ex, description, sourceFilePath, member, sourceLineNumber);
      }
      catch
      {
      }
    }

    public static IDisposable GetIncrementalTimer(string name)
    {
      if (ClientMetricsProviderFactory._instance == null)
        return (IDisposable) ClientMetricsProviderFactory._dummyTimer;
      try
      {
        return ClientMetricsProviderFactory._instance.GetIncrementalTimer(name);
      }
      catch
      {
        return (IDisposable) ClientMetricsProviderFactory._dummyTimer;
      }
    }

    public static IDisposable GetIncrementalTimer(string name, params SFxTag[] tags)
    {
      if (ClientMetricsProviderFactory._instance == null)
        return (IDisposable) ClientMetricsProviderFactory._dummyTimer;
      try
      {
        return ClientMetricsProviderFactory._instance.GetIncrementalTimer(name, tags);
      }
      catch
      {
        return (IDisposable) ClientMetricsProviderFactory._dummyTimer;
      }
    }

    public static IDisposable GetIncrementalTimer(string name, params string[] tags)
    {
      if (ClientMetricsProviderFactory._instance == null)
        return (IDisposable) ClientMetricsProviderFactory._dummyTimer;
      try
      {
        return ClientMetricsProviderFactory._instance.GetIncrementalTimer(name, tags);
      }
      catch
      {
        return (IDisposable) ClientMetricsProviderFactory._dummyTimer;
      }
    }

    public static void RecordIncrementalTimerSample(string name, TimeSpan time)
    {
      if (ClientMetricsProviderFactory._instance == null)
        return;
      try
      {
        ClientMetricsProviderFactory._instance.RecordIncrementalTimerSample(name, time);
      }
      catch
      {
      }
    }

    public static void RecordIncrementalTimerSample(
      string name,
      TimeSpan time,
      params SFxTag[] tags)
    {
      if (ClientMetricsProviderFactory._instance == null)
        return;
      try
      {
        ClientMetricsProviderFactory._instance.RecordIncrementalTimerSample(name, time, tags);
      }
      catch
      {
      }
    }

    public static void RecordIncrementalTimerSample(
      string name,
      TimeSpan time,
      params string[] tags)
    {
      if (ClientMetricsProviderFactory._instance == null)
        return;
      try
      {
        ClientMetricsProviderFactory._instance.RecordIncrementalTimerSample(name, time, tags);
      }
      catch
      {
      }
    }

    public static void RecordIncrementalTimerSample(string name, long time)
    {
      if (ClientMetricsProviderFactory._instance == null)
        return;
      try
      {
        ClientMetricsProviderFactory._instance.RecordIncrementalTimerSample(name, time);
      }
      catch
      {
      }
    }

    public static void RecordIncrementalTimerSample(string name, long time, params SFxTag[] tags)
    {
      if (ClientMetricsProviderFactory._instance == null)
        return;
      try
      {
        ClientMetricsProviderFactory._instance.RecordIncrementalTimerSample(name, time, tags);
      }
      catch
      {
      }
    }

    public static void RecordIncrementalTimerSample(string name, long time, params string[] tags)
    {
      if (ClientMetricsProviderFactory._instance == null)
        return;
      try
      {
        ClientMetricsProviderFactory._instance.RecordIncrementalTimerSample(name, time, tags);
      }
      catch
      {
      }
    }

    private static string GetSignalFxAPIToken() => "gFrGKy5i9_bqmbUaeaAeOQ";

    private static bool GetSignalFxEnabled() => true;

    private static int GetSignalFxTimeSpan() => 1000;

    public class DummyTimer : MarshalByRefObject, IDisposable
    {
      public void Dispose()
      {
      }
    }
  }
}
