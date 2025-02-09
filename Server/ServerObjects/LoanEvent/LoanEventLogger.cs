// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.LoanEvent.LoanEventLogger
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Metrics;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Logging;
using Encompass.Diagnostics.Logging.Targets;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.LoanEvent
{
  public class LoanEventLogger
  {
    private static BlockingCollection<QueuedEventMessage> _items = new BlockingCollection<QueuedEventMessage>();
    private static Task _backgroundAppenderTask;
    private static readonly string _processOwner;
    private static string ClassName = nameof (LoanEventLogger);
    private static ConcurrentDictionary<string, IApplicationLog> _logWriters = new ConcurrentDictionary<string, IApplicationLog>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);

    static LoanEventLogger()
    {
      LoanEventLogger._processOwner = Environment.MachineName + "." + (object) AppDomain.CurrentDomain.Id + "." + (object) Process.GetCurrentProcess().Id;
      ServerGlobals.TraceLog.WriteInfo(LoanEventLogger.ClassName, "Initializing the LoanEventLogger for backing up Loan Events...");
      LoanEventLogger._backgroundAppenderTask = LoanEventLogger.StartBackgroundAppenderTask();
    }

    public static string EventLogDir(IClientContext ctx)
    {
      string str = (string) ctx.Settings.GetServerSetting("Policies.ServerEventLogOverrideLocation");
      if (string.IsNullOrWhiteSpace(str))
        str = Path.Combine(ctx.Settings.LogDir, "events");
      return str;
    }

    public static string ProcessOwner => LoanEventLogger._processOwner;

    public static bool RegisterSource(string source, IClientContext ctx)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(source))
          source = "default";
        if (LoanEventLogger._logWriters.ContainsKey(source))
          return true;
        string rootName = string.Format("loan.{0}.{1}", (object) LoanEventLogger.ProcessOwner, (object) source);
        string str = LoanEventLogger.EventLogDir(ctx);
        if (!Directory.Exists(str))
          Directory.CreateDirectory(str);
        IApplicationLog applicationLog = (IApplicationLog) RollingFileSystemLog.Create(str, rootName, FileLogRolloverFrequency.Hourly);
        applicationLog.WriteLine(string.Empty);
        LoanEventLogger._logWriters.TryAdd(source, applicationLog);
        return true;
      }
      catch (Exception ex)
      {
        ServerGlobals.TraceLog.WriteError(LoanEventLogger.ClassName, "Failed to register source:  " + source + ".  Error=" + (object) ex);
      }
      return false;
    }

    public static void UnregisterSource(string sourceName)
    {
      if (string.IsNullOrWhiteSpace(sourceName))
        sourceName = "default";
      IApplicationLog applicationLog;
      LoanEventLogger._logWriters.TryRemove(sourceName, out applicationLog);
      applicationLog?.Close();
    }

    public static void Write(LoanEventMessage message, string source)
    {
      if (message == null)
        return;
      if (string.IsNullOrWhiteSpace(source))
        source = "default";
      if (!LoanEventLogger._logWriters.ContainsKey(source))
        throw new InvalidOperationException("The log source was not registered so cannot be written to.  Source=" + source);
      LoanEventLogger._items.Add(new QueuedEventMessage(source, message));
    }

    public static IList<string> GetLogDirs()
    {
      List<string> logDirs = new List<string>();
      foreach (IApplicationLog applicationLog in (IEnumerable<IApplicationLog>) LoanEventLogger._logWriters.Values)
        logDirs.Add(((RollingFileSystemLog) applicationLog).LogDir);
      return (IList<string>) logDirs;
    }

    private static Task StartBackgroundAppenderTask()
    {
      return Task.Factory.StartNew((Action) (() => LoanEventLogger.WriteToFile()), TaskCreationOptions.LongRunning);
    }

    private static void WriteToFile()
    {
      ServerGlobals.TraceLog.WriteInfo(LoanEventLogger.ClassName, "Starting background task for loan event logging on thread..." + (object) Thread.CurrentThread.ManagedThreadId);
      JsonSerializerSettings settings = new JsonSerializerSettings();
      settings.Formatting = Formatting.None;
      settings.NullValueHandling = NullValueHandling.Ignore;
      settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
      IServerEventMetricsRecorder eventMetricsRecorder = new MetricsFactory().CreateServerEventMetricsRecorder();
      while (true)
      {
        try
        {
          QueuedEventMessage queuedEventMessage = LoanEventLogger._items.Take();
          using (eventMetricsRecorder.RecordLogTime(ServerEventType.Loan))
          {
            string text = JsonConvert.SerializeObject((object) queuedEventMessage.Message, settings);
            IApplicationLog logWriter = LoanEventLogger._logWriters[queuedEventMessage.SourceName];
            logWriter.WriteLine(text);
            logWriter.Flush();
          }
        }
        catch (Exception ex)
        {
          eventMetricsRecorder.IncrementLogErrorCount(ServerEventType.Loan);
          ServerGlobals.TraceLog.WriteError(LoanEventLogger.ClassName, "Failed to write to event file:  " + (object) ex);
        }
      }
    }
  }
}
