// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.Client.MetricsFactory
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using Elli.Metrics.Client.Disabled;
using Metrics;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

#nullable disable
namespace Elli.Metrics.Client
{
  public class MetricsFactory : IMetricsFactory
  {
    private static readonly object LockObject = new object();
    private const bool Enabled = false;
    private static MetricsFactory _metricsFactory = (MetricsFactory) null;
    private static string _instance;
    private static string _smartClientVersion;
    private static string _userId;

    private MetricsFactory()
    {
    }

    public static void Initiate(
      string customer,
      string instance,
      string userId,
      string apiToken,
      string smartClientVersion,
      int timeSpan = 300,
      bool enabled = false)
    {
      lock (MetricsFactory.LockObject)
      {
        try
        {
          string str = string.IsNullOrWhiteSpace(instance) ? "Default" : instance;
          if (MetricsFactory._instance != null && MetricsFactory._instance == str && MetricsFactory._smartClientVersion == smartClientVersion && MetricsFactory._userId == userId)
            return;
          if (MetricsFactory._instance != null && Metric.Config != null)
            Metric.Config.Dispose();
          MetricsFactory._instance = str;
          MetricsFactory._smartClientVersion = smartClientVersion;
          MetricsFactory._userId = userId;
          if (ConfigurationManager.AppSettings["SignalFXTest"] == "on")
            customer = ConfigurationManager.AppSettings["SignalFXTestCustomerValue"];
          if (MetricsFactory._metricsFactory != null)
            return;
          MetricsFactory._metricsFactory = new MetricsFactory();
        }
        catch (Exception ex)
        {
          Trace.WriteLine(ex.Message);
          Trace.WriteLine(ex.StackTrace);
          MetricsFactory._instance = (string) null;
          MetricsFactory._metricsFactory = (MetricsFactory) null;
        }
      }
    }

    public static bool Initiated => MetricsFactory._instance != null;

    public static MetricsFactory GetInstance()
    {
      return MetricsFactory._metricsFactory != null ? MetricsFactory._metricsFactory : (MetricsFactory) null;
    }

    public static void EndSession()
    {
      if (MetricsFactory._metricsFactory == null)
        return;
      MetricsFactory._metricsFactory.ReleaseSfxConnection();
    }

    public bool MetricsCollectionEnabled => false;

    public static IClientMetricsRecorder CreateClientMetricsRecorder()
    {
      try
      {
      }
      catch
      {
      }
      return (IClientMetricsRecorder) new ClientMetricsRecorder();
    }

    public static void IncrementCounter(string name)
    {
      MetricsFactory.CreateClientMetricsRecorder().IncrementCounter(name);
    }

    public static void IncrementCounter(string name, params SFxTag[] tags)
    {
      MetricsFactory.CreateClientMetricsRecorder().IncrementCounter(name, tags);
    }

    public static void IncrementCounter(string name, params string[] tags)
    {
      MetricsFactory.CreateClientMetricsRecorder().IncrementCounter(name, tags);
    }

    public static void IncrementErrorCounter(
      Exception ex,
      string description,
      [CallerFilePath] string sourceFilePath = "",
      [CallerMemberName] string member = "",
      [CallerLineNumber] int sourceLineNumber = -1)
    {
      MetricsFactory.CreateClientMetricsRecorder().IncrementErrorCounter(ex, description, sourceFilePath, member, sourceLineNumber);
    }

    public static IDisposable GetIncrementalTimer(string name)
    {
      return MetricsFactory.CreateClientMetricsRecorder().IncrementalTimer(name);
    }

    public static IDisposable GetIncrementalTimer(string name, params SFxTag[] tags)
    {
      return MetricsFactory.CreateClientMetricsRecorder().IncrementalTimer(name, tags);
    }

    public static IDisposable GetOldIncrementalTimer(string name, params SFxTag[] tags)
    {
      return MetricsFactory.CreateClientMetricsRecorder().OldIncrementalTimer(name, tags);
    }

    public static IDisposable GetIncrementalTimer(string name, params string[] tags)
    {
      return MetricsFactory.CreateClientMetricsRecorder().IncrementalTimer(name, tags);
    }

    public static void RecordIncrementalTimerSample(string name, TimeSpan time)
    {
      MetricsFactory.CreateClientMetricsRecorder().RecordIncrementalTimerSample(name, time);
    }

    public static void RecordIncrementalTimerSample(
      string name,
      TimeSpan time,
      params SFxTag[] tags)
    {
      MetricsFactory.CreateClientMetricsRecorder().RecordIncrementalTimerSample(name, time, tags);
    }

    public static void RecordIncrementalTimerSample(
      string name,
      TimeSpan time,
      params string[] tags)
    {
      MetricsFactory.CreateClientMetricsRecorder().RecordIncrementalTimerSample(name, time, tags);
    }

    public static void RecordIncrementalTimerSample(string name, long time)
    {
      MetricsFactory.CreateClientMetricsRecorder().RecordIncrementalTimerSample(name, time);
    }

    public static void RecordIncrementalTimerSample(string name, long time, params SFxTag[] tags)
    {
      MetricsFactory.CreateClientMetricsRecorder().RecordIncrementalTimerSample(name, time, tags);
    }

    public static void RecordIncrementalTimerSample(string name, long time, params string[] tags)
    {
      MetricsFactory.CreateClientMetricsRecorder().RecordIncrementalTimerSample(name, time, tags);
    }

    private void ReleaseSfxConnection()
    {
      if (Metric.Config == null)
        return;
      Metric.Config.Dispose();
    }
  }
}
