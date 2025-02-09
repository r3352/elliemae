// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Diagnostics.PerformancePublisher
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Diagnostics
{
  public static class PerformancePublisher
  {
    private const string className = "PerformancePublisher";
    private static readonly string sw = Tracing.SwRemoting;
    private static int MaxQueueSize = 7;
    private static int MaxMergeSize = 25;
    private static int MaxInputFormMergeSize = 25;
    private static int MaxDeepLinkMergeSize = 25;
    private static TimeSpan SubmissionInterval = TimeSpan.FromSeconds(30.0);
    private static Queue<PerformanceMeter> queuedMeters = new Queue<PerformanceMeter>();
    private static Queue<PerformanceMeter> importantKPIMeters = new Queue<PerformanceMeter>();
    private static List<PerformanceMeter> mergeMeters = new List<PerformanceMeter>();
    private static List<PerformanceMeter> mergeInputFormMeters = new List<PerformanceMeter>();
    private static List<PerformanceMeter> mergeDeepLinkMeters = new List<PerformanceMeter>();
    private static PerformanceMeter nextPingMeterToSend = (PerformanceMeter) null;
    private static Thread submissionThread = (Thread) null;
    private static bool settingsLoaded = false;
    private static readonly Random random = new Random();
    private static string clientId = (string) null;
    private static string userId = (string) null;
    private static string server = (string) null;
    private static bool hosted = false;

    public static void Start()
    {
      lock (typeof (PerformancePublisher))
      {
        if (PerformancePublisher.submissionThread != null)
          return;
        PerformancePublisher.submissionThread = new Thread(new ThreadStart(PerformancePublisher.submitQueuedData));
        PerformancePublisher.submissionThread.Priority = ThreadPriority.Lowest;
        PerformancePublisher.submissionThread.IsBackground = true;
        PerformancePublisher.submissionThread.Start();
        PerformanceMeter.MeterStopped += new PerformanceMeterEventHandler(PerformancePublisher.onPerformanceMeterStopped);
        if (!PerformanceMeter.Enabled)
          PerformanceMeter.MeterMode = PerformanceMeterMode.NoLog;
        Tracing.Log(PerformancePublisher.sw, nameof (PerformancePublisher), TraceLevel.Info, "PerformancePublisher thread started");
      }
    }

    public static void CloseInstance()
    {
      try
      {
        if (PerformancePublisher.submissionThread != null)
          PerformancePublisher.submissionThread.Abort();
      }
      catch
      {
      }
      try
      {
        lock (PerformancePublisher.mergeMeters)
        {
          if (PerformancePublisher.mergeMeters.Count > 0)
            PerformancePublisher.submitMeterData(PerformancePublisher.mergeMeters.ToArray());
        }
        lock (PerformancePublisher.mergeInputFormMeters)
        {
          if (PerformancePublisher.mergeInputFormMeters.Count > 0)
            PerformancePublisher.submitMeterData(PerformancePublisher.mergeInputFormMeters.ToArray(), "InputForms-Activity");
        }
        lock (PerformancePublisher.mergeDeepLinkMeters)
        {
          if (PerformancePublisher.mergeDeepLinkMeters.Count <= 0)
            return;
          PerformancePublisher.submitMeterData(PerformancePublisher.mergeDeepLinkMeters.ToArray(), "DeepLink-Activity");
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(PerformancePublisher.sw, nameof (PerformancePublisher), TraceLevel.Error, "PerformancePublisher Close Instance failed due to error: " + (object) ex);
      }
    }

    private static void onPerformanceMeterStopped(PerformanceMeter meter)
    {
      if (meter.IsDocsMeter || !meter.Publish || meter.Aborted)
        return;
      if (meter.CombinedPublication)
      {
        int num1 = -1;
        int num2 = -1;
        int num3 = -1;
        lock (typeof (PerformancePublisher))
        {
          num1 = PerformancePublisher.MaxMergeSize;
          num2 = PerformancePublisher.MaxInputFormMergeSize;
          num3 = PerformancePublisher.MaxDeepLinkMergeSize;
        }
        if (meter.Name == "InputForm.Load")
        {
          lock (PerformancePublisher.mergeInputFormMeters)
          {
            if (PerformancePublisher.mergeInputFormMeters.Count >= num2)
            {
              PerformancePublisher.submitMeterData(PerformancePublisher.mergeInputFormMeters.ToArray(), "InputForms-Activity");
              PerformancePublisher.mergeInputFormMeters.Clear();
            }
            PerformancePublisher.mergeInputFormMeters.Add(meter);
          }
        }
        else if (meter.Name.StartsWith("EncompassDesktop.DeepLink", StringComparison.InvariantCultureIgnoreCase))
        {
          lock (PerformancePublisher.mergeDeepLinkMeters)
          {
            if (PerformancePublisher.mergeDeepLinkMeters.Count >= num3)
            {
              PerformancePublisher.submitMeterData(PerformancePublisher.mergeDeepLinkMeters.ToArray(), "DeepLink-Activity");
              PerformancePublisher.mergeDeepLinkMeters.Clear();
            }
            PerformancePublisher.mergeDeepLinkMeters.Add(meter);
          }
        }
        else
        {
          lock (PerformancePublisher.mergeMeters)
          {
            if (PerformancePublisher.mergeMeters.Count >= num1)
            {
              PerformancePublisher.submitMeterData(PerformancePublisher.mergeMeters.ToArray());
              PerformancePublisher.mergeMeters.Clear();
            }
            PerformancePublisher.mergeMeters.Add(meter);
          }
        }
      }
      else if (meter.SslPublication)
      {
        try
        {
          if (!PerformancePublisher.isSessionStarted())
            return;
          if (!PerformancePublisher.settingsLoaded)
            PerformancePublisher.loadPerformanceSettings();
          lock (meter)
            PerformancePublisher.submitMeterData(meter);
        }
        catch (Exception ex)
        {
          Tracing.Log(PerformancePublisher.sw, nameof (PerformancePublisher), TraceLevel.Error, "PerformancePublisher thread terminated abnormally due to error: " + (object) ex);
        }
      }
      else
      {
        int num = -1;
        lock (typeof (PerformancePublisher))
          num = PerformancePublisher.MaxQueueSize;
        lock (PerformancePublisher.queuedMeters)
        {
          if (PerformancePublisher.isPingMeter(meter))
            PerformancePublisher.nextPingMeterToSend = meter;
          else if (PerformancePublisher.importantKPIMeters.Count < num && PerformancePublisher.isKPIMeter(meter))
          {
            PerformancePublisher.importantKPIMeters.Enqueue(meter);
          }
          else
          {
            if (PerformancePublisher.queuedMeters.Count >= num)
              return;
            PerformancePublisher.queuedMeters.Enqueue(meter);
          }
        }
      }
    }

    private static bool isPingMeter(PerformanceMeter meter) => meter.Name == "Client.Ping";

    private static bool isKPIMeter(PerformanceMeter meter)
    {
      return meter.Name == "Encompass.Login" || meter.Name == "Loan.Open" || meter.Name == "Loan.Save" || meter.Name == "Pipeline.Load" || meter.Name == "Pipeline.Refresh";
    }

    private static void submitQueuedData()
    {
      try
      {
        bool flag = true;
        DateTime dateTime = DateTime.Now;
        while (true)
        {
          do
          {
            Thread.Sleep(PerformancePublisher.SubmissionInterval);
            if (!PerformancePublisher.settingsLoaded && PerformancePublisher.isSessionStarted())
              PerformancePublisher.loadPerformanceSettings();
          }
          while (!PerformancePublisher.settingsLoaded);
          PerformanceMeter meter1 = (PerformanceMeter) null;
          PerformanceMeter meter2 = (PerformanceMeter) null;
          TimeSpan timeSpan = TimeSpan.FromSeconds(Math.Max(600.0, PerformancePublisher.SubmissionInterval.TotalSeconds * 3.0));
          lock (PerformancePublisher.queuedMeters)
          {
            if (DateTime.Now > dateTime)
            {
              meter2 = PerformancePublisher.nextPingMeterToSend;
              PerformancePublisher.nextPingMeterToSend = (PerformanceMeter) null;
              dateTime = DateTime.Now + timeSpan;
            }
            if (flag && PerformancePublisher.importantKPIMeters.Count > 0)
              meter1 = PerformancePublisher.importantKPIMeters.Dequeue();
            if (meter1 == null && PerformancePublisher.queuedMeters.Count > 0)
              meter1 = PerformancePublisher.queuedMeters.Dequeue();
            flag = !flag;
          }
          try
          {
            if (meter2 != null)
              PerformancePublisher.submitMeterData(meter2);
          }
          catch
          {
          }
          try
          {
            if (meter1 != null)
              PerformancePublisher.submitMeterData(meter1);
          }
          catch
          {
          }
        }
      }
      catch (ThreadAbortException ex)
      {
      }
      catch (Exception ex)
      {
        Tracing.Log(PerformancePublisher.sw, nameof (PerformancePublisher), TraceLevel.Error, "PerformancePublisher thread terminated abnormally due to error: " + (object) ex);
      }
    }

    private static void loadPerformanceSettings()
    {
      try
      {
        PerformancePublisher.clientId = Session.CompanyInfo.ClientID;
        PerformancePublisher.userId = Session.UserID;
        PerformancePublisher.server = Session.ServerIdentity == null ? "" : Session.ServerIdentity.Uri.Host;
        PerformancePublisher.hosted = Session.StartupInfo.RuntimeEnvironment == RuntimeEnvironment.Hosted;
        using (DiagnosticsService diagnosticsService = new DiagnosticsService(Session.StartupInfo?.ServiceUrls?.JedServicesUrl))
        {
          diagnosticsService.Timeout = 5000;
          PerformanceSettings performanceSettings = diagnosticsService.GetPerformanceSettings(PerformancePublisher.clientId);
          lock (typeof (PerformancePublisher))
          {
            PerformancePublisher.SubmissionInterval = TimeSpan.FromSeconds((double) performanceSettings.SubmissionInterval);
            PerformancePublisher.settingsLoaded = true;
          }
        }
        Tracing.Log(PerformancePublisher.sw, nameof (PerformancePublisher), TraceLevel.Info, "Performance settings loaded successfully. MaxQueueSize = " + (object) PerformancePublisher.MaxQueueSize + ", SubmissionInterval = " + PerformancePublisher.SubmissionInterval.TotalSeconds.ToString("0"));
      }
      catch (Exception ex)
      {
        Tracing.Log(PerformancePublisher.sw, nameof (PerformancePublisher), TraceLevel.Error, "Error retrieving settings from remote web service: " + (object) ex);
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
      string xmlData = (string) null;
      try
      {
        bool flag = false;
        lock (PerformancePublisher.random)
          flag = PerformancePublisher.random.Next(500) == 0;
        xmlData = !(Session.StartupInfo.AllowDetailedPerfMeter | flag) || !PerformancePublisher.isKPIMeter(meter) ? meter.GetXmlData() : meter.GetFullXmlData();
      }
      catch (Exception ex)
      {
      }
      using (DiagnosticsService asyncState = new DiagnosticsService(Session.StartupInfo?.ServiceUrls?.JedServicesUrl))
      {
        asyncState.Timeout = 5000;
        asyncState.BeginPostPerformanceData(PerformancePublisher.clientId, PerformancePublisher.userId, PerformancePublisher.server, PerformancePublisher.hosted, meter.Name, Convert.ToInt32(meter.Duration.TotalMilliseconds), xmlData, new AsyncCallback(PerformancePublisher.disposeDiagnosticsService), (object) asyncState);
      }
      Tracing.Log(PerformancePublisher.sw, nameof (PerformancePublisher), TraceLevel.Verbose, "Performance data for meter '" + meter.Name + "' published successfully.");
    }

    private static void disposeDiagnosticsService(IAsyncResult ar)
    {
      ((Component) ar.AsyncState).Dispose();
    }

    private static void submitMeterData(PerformanceMeter[] meters, string activityName = "MultipleActivity")
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml("<Data />");
      if (meters == null)
        return;
      for (int index = 0; index < meters.GetLength(0); ++index)
      {
        PerformanceMeter meter = meters[index];
        XmlElement xmlElement1 = (XmlElement) xmlDocument.DocumentElement.AppendChild((XmlNode) xmlDocument.CreateElement("Var"));
        xmlElement1.SetAttribute("Name", "ActivityName" + (object) index);
        xmlElement1.InnerText = meter.Name;
        XmlElement xmlElement2 = (XmlElement) xmlDocument.DocumentElement.AppendChild((XmlNode) xmlDocument.CreateElement("Var"));
        xmlElement2.SetAttribute("Name", "ActivityTime" + (object) index);
        xmlElement2.InnerText = Convert.ToInt32(meter.Duration.TotalMilliseconds).ToString();
        foreach (string key in (IEnumerable) meter.Variables.Keys)
        {
          XmlElement xmlElement3 = (XmlElement) xmlDocument.DocumentElement.AppendChild((XmlNode) xmlDocument.CreateElement("Var"));
          xmlElement3.SetAttribute("Name", key + (object) index);
          xmlElement3.InnerText = string.Concat(meter.Variables[(object) key]);
        }
      }
      using (DiagnosticsService diagnosticsService = new DiagnosticsService(Session.StartupInfo?.ServiceUrls?.JedServicesUrl))
      {
        diagnosticsService.Timeout = 5000;
        diagnosticsService.PostPerformanceData(PerformancePublisher.clientId, PerformancePublisher.userId, PerformancePublisher.server, PerformancePublisher.hosted, activityName, 0, xmlDocument.OuterXml);
      }
      Tracing.Log(PerformancePublisher.sw, nameof (PerformancePublisher), TraceLevel.Verbose, "Performance data for MultipleActivity published successfully.");
    }
  }
}
