// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Diagnostics.PerformancePublisherDOCS
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Diagnostics
{
  public static class PerformancePublisherDOCS
  {
    private const string className = "PerformancePublisherDOCS";
    private static readonly string sw = Tracing.SwRemoting;
    public const int MAX_LIST_SIZE = 50;
    private static TimeSpan SubmissionInterval = TimeSpan.FromSeconds(10.0);
    private static List<PerformanceMeter> meters = new List<PerformanceMeter>();
    private static Thread submissionThread = (Thread) null;
    private static bool settingsLoaded = false;
    private static string clientId = (string) null;
    private static string userId = (string) null;
    private static string server = (string) null;
    private static bool hosted = false;
    private static double publishingRate = -1.0;

    public static void Start()
    {
      lock (typeof (PerformancePublisherDOCS))
      {
        if (PerformancePublisherDOCS.submissionThread != null)
          return;
        PerformancePublisherDOCS.submissionThread = new Thread(new ThreadStart(PerformancePublisherDOCS.submitQueuedData));
        PerformancePublisherDOCS.submissionThread.Priority = ThreadPriority.Lowest;
        PerformancePublisherDOCS.submissionThread.IsBackground = true;
        PerformancePublisherDOCS.submissionThread.Start();
        PerformanceMeter.MeterStopped += new PerformanceMeterEventHandler(PerformancePublisherDOCS.onPerformanceMeterStopped);
        if (!PerformanceMeter.Enabled)
          PerformanceMeter.MeterMode = PerformanceMeterMode.NoLog;
        Tracing.Log(PerformancePublisherDOCS.sw, nameof (PerformancePublisherDOCS), TraceLevel.Info, "PerformancePublisherDOCS thread started");
      }
    }

    public static void CloseInstance()
    {
      try
      {
        if (PerformancePublisherDOCS.submissionThread != null)
          PerformancePublisherDOCS.submissionThread.Abort();
      }
      catch
      {
      }
      try
      {
        Random random = new Random();
        lock (PerformancePublisherDOCS.meters)
        {
          foreach (PerformanceMeter meter in PerformancePublisherDOCS.meters)
          {
            if (PerformancePublisherDOCS.publishingRate >= 1.0 || PerformancePublisherDOCS.publishingRate > 0.0 && random.NextDouble() <= PerformancePublisherDOCS.publishingRate)
              PerformancePublisherDOCS.submitMeterData(meter);
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(PerformancePublisherDOCS.sw, nameof (PerformancePublisherDOCS), TraceLevel.Error, "PerformancePublisher Close Instance failed due to error: " + (object) ex);
      }
    }

    private static void onPerformanceMeterStopped(PerformanceMeter meter)
    {
      if (!meter.IsDocsMeter || meter.Aborted)
        return;
      lock (PerformancePublisherDOCS.meters)
      {
        if (PerformancePublisherDOCS.meters.Count >= 50)
          PerformancePublisherDOCS.meters.RemoveRange(0, PerformancePublisherDOCS.meters.Count - 50 + 1);
        PerformancePublisherDOCS.meters.Add(meter);
      }
    }

    private static void submitQueuedData()
    {
      int millisecondsTimeout1 = 5000;
      int millisecondsTimeout2 = 500;
      try
      {
        Random random = new Random();
        while (true)
        {
          PerformanceMeter meter = (PerformanceMeter) null;
          if (!PerformancePublisherDOCS.settingsLoaded && PerformancePublisherDOCS.isSessionStarted())
            PerformancePublisherDOCS.loadPerformanceSettings();
          lock (PerformancePublisherDOCS.meters)
          {
            if (PerformancePublisherDOCS.meters.Count > 0)
            {
              meter = PerformancePublisherDOCS.meters[0];
              PerformancePublisherDOCS.meters.RemoveAt(0);
            }
          }
          if (meter != null)
          {
            try
            {
              if (PerformancePublisherDOCS.publishingRate >= 1.0 || PerformancePublisherDOCS.publishingRate > 0.0 && random.NextDouble() <= PerformancePublisherDOCS.publishingRate)
              {
                PerformancePublisherDOCS.submitMeterData(meter);
                Thread.Sleep(millisecondsTimeout2);
              }
            }
            catch (Exception ex)
            {
              Tracing.Log(PerformancePublisherDOCS.sw, nameof (PerformancePublisherDOCS), TraceLevel.Error, "PerformancePublisher EXCEPTION publishing meter '" + meter.Name + "' - " + (object) ex);
            }
          }
          else
            Thread.Sleep(millisecondsTimeout1);
        }
      }
      catch (ThreadAbortException ex)
      {
      }
      catch (Exception ex)
      {
        Tracing.Log(PerformancePublisherDOCS.sw, nameof (PerformancePublisherDOCS), TraceLevel.Error, "PerformancePublisherDOCS thread terminated abnormally due to EXCEPTION - " + (object) ex);
      }
    }

    private static void loadPerformanceSettings()
    {
      try
      {
        PerformancePublisherDOCS.clientId = Session.CompanyInfo.ClientID;
        PerformancePublisherDOCS.userId = Session.UserID;
        PerformancePublisherDOCS.server = Session.ServerIdentity == null ? "" : Session.ServerIdentity.Uri.Host;
        PerformancePublisherDOCS.hosted = Session.StartupInfo.RuntimeEnvironment == RuntimeEnvironment.Hosted;
        PerformancePublisherDOCS.publishingRate = Session.StartupInfo.DocsPerformancePublishingRate;
        lock (typeof (PerformancePublisher))
          PerformancePublisherDOCS.settingsLoaded = true;
        Tracing.Log(PerformancePublisherDOCS.sw, nameof (PerformancePublisherDOCS), TraceLevel.Info, "DOCS Performance settings loaded successfully");
      }
      catch (Exception ex)
      {
        Tracing.Log(PerformancePublisherDOCS.sw, nameof (PerformancePublisherDOCS), TraceLevel.Error, "Error loading DOCS Performance settings: " + (object) ex);
      }
    }

    private static bool isSessionStarted()
    {
      try
      {
        return Session.StartupInfo != null;
      }
      catch
      {
        return false;
      }
    }

    private static void submitMeterData(PerformanceMeter meter)
    {
      if (meter == null)
        return;
      try
      {
        meter.AddVariable("EncompassVersion", (object) VersionInformation.CurrentVersion.DisplayVersionString);
        string fullXmlData = meter.GetFullXmlData();
        if (string.IsNullOrWhiteSpace(fullXmlData))
          return;
        using (DiagnosticsService asyncState = new DiagnosticsService(Session.StartupInfo?.ServiceUrls?.JedServicesUrl))
        {
          asyncState.Timeout = 5000;
          asyncState.BeginPostPerformanceData(PerformancePublisherDOCS.clientId, PerformancePublisherDOCS.userId, PerformancePublisherDOCS.server, PerformancePublisherDOCS.hosted, meter.Name, Convert.ToInt32(meter.Duration.TotalMilliseconds), fullXmlData, new AsyncCallback(PerformancePublisherDOCS.disposeDiagnosticsService), (object) asyncState);
        }
        Tracing.Log(PerformancePublisherDOCS.sw, nameof (PerformancePublisherDOCS), TraceLevel.Verbose, "Performance data for meter '" + meter.Name + "' published successfully.");
      }
      catch (Exception ex)
      {
        Tracing.Log(PerformancePublisherDOCS.sw, nameof (PerformancePublisherDOCS), TraceLevel.Error, "PerformancePublisherDOCS.submitMeterData Exception - " + ex.Message);
      }
    }

    private static void disposeDiagnosticsService(IAsyncResult ar)
    {
      ((Component) ar.AsyncState).Dispose();
    }
  }
}
