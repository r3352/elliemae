// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServiceObjects.KafkaEvent.KafkaEventLogger
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Logging;
using Encompass.Diagnostics.Logging.Targets;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Server.ServiceObjects.KafkaEvent
{
  public class KafkaEventLogger
  {
    private static BlockingCollection<KafkaEventMessage> _items = new BlockingCollection<KafkaEventMessage>();
    private static Task _backgroundAppenderTask;
    private static readonly string _processOwner;
    private static string ClassName = nameof (KafkaEventLogger);
    private static ConcurrentDictionary<string, IApplicationLog> _logWriters = new ConcurrentDictionary<string, IApplicationLog>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);

    static KafkaEventLogger()
    {
      KafkaEventLogger._processOwner = Environment.MachineName + "." + (object) AppDomain.CurrentDomain.Id + "." + (object) Process.GetCurrentProcess().Id;
      ServerGlobals.TraceLog.WriteInfo(KafkaEventLogger.ClassName, "Initializing the KafkaEventLogger for backing up Kafka Events...");
      KafkaEventLogger._backgroundAppenderTask = KafkaEventLogger.StartBackgroundAppenderTask();
    }

    public static string EventLogDir(IClientContext ctx)
    {
      string str = (string) ctx.Settings.GetServerSetting("Policies.ServerEventLogOverrideLocation");
      if (string.IsNullOrWhiteSpace(str))
        str = Path.Combine(ctx.Settings.LogDir, "events");
      return str;
    }

    public static string ProcessOwner => KafkaEventLogger._processOwner;

    public static bool RegisterSource(string source, IClientContext ctx)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(source))
          source = "default";
        if (KafkaUtils.IsKafkaDebugLogEnabled)
        {
          string message = string.Format("KafkaEventLogger:RegisterSouce - Registering source for InstanceName:{0}", (object) source);
          TraceLog.WriteWarning(KafkaEventLogger.ClassName, message);
        }
        if (KafkaEventLogger._logWriters.ContainsKey(source))
          return true;
        string rootName = string.Format("kafka.{0}.{1}", (object) KafkaEventLogger.ProcessOwner, (object) source);
        string str = KafkaEventLogger.EventLogDir(ctx);
        if (!Directory.Exists(str))
          Directory.CreateDirectory(str);
        if (KafkaUtils.IsKafkaDebugLogEnabled)
        {
          string message = string.Format("KafkaEventLogger:RegisterSouce - The kafa file name that is registered is filename:{0} and the event log directory is : {1}", (object) rootName, (object) str);
          TraceLog.WriteWarning(KafkaEventLogger.ClassName, message);
        }
        IApplicationLog applicationLog = (IApplicationLog) RollingFileSystemLog.Create(str, rootName, FileLogRolloverFrequency.Hourly);
        applicationLog.WriteLine(string.Empty);
        KafkaEventLogger._logWriters.TryAdd(source, applicationLog);
        return true;
      }
      catch (Exception ex)
      {
        ServerGlobals.TraceLog.WriteError(KafkaEventLogger.ClassName, "Failed to register source:  " + source + ".  Error=" + (object) ex);
      }
      return false;
    }

    public static void UnregisterSource(string sourceName)
    {
      if (string.IsNullOrWhiteSpace(sourceName))
        sourceName = "default";
      IApplicationLog applicationLog;
      KafkaEventLogger._logWriters.TryRemove(sourceName, out applicationLog);
      applicationLog?.Close();
    }

    public static void Write(
      string source,
      string topic,
      string entityId,
      string instanceId,
      string messageType,
      string payload,
      QueueMessage message)
    {
      if (message == null)
        return;
      if (string.IsNullOrWhiteSpace(source))
        source = "default";
      if (KafkaUtils.IsKafkaDebugLogEnabled)
      {
        string message1 = string.Format("KafkaEventLogger: Write Kafka Event to log file for InstanceName:{0}, Topic:{1}, EntityId:{2}, InstanceId:{3}, MessageType:{4}", (object) source, (object) topic, (object) entityId, (object) instanceId, (object) messageType);
        TraceLog.WriteWarning(KafkaEventLogger.ClassName, message1);
      }
      if (!KafkaEventLogger._logWriters.ContainsKey(source))
      {
        string message2 = string.Format("KafkaEventLogger: The log source was not registered so cannot be written to Source:{0} ", (object) source);
        TraceLog.WriteWarning(KafkaEventLogger.ClassName, message2);
        throw new InvalidOperationException("The log source was not registered so cannot be written to.  Source=" + source);
      }
      KafkaEventLogger._items.Add(new KafkaEventMessage(source, topic, entityId, instanceId, messageType, payload, message));
    }

    public static IList<string> GetLogDirs()
    {
      List<string> logDirs = new List<string>();
      foreach (IApplicationLog applicationLog in (IEnumerable<IApplicationLog>) KafkaEventLogger._logWriters.Values)
        logDirs.Add(((RollingFileSystemLog) applicationLog).LogDir);
      return (IList<string>) logDirs;
    }

    private static Task StartBackgroundAppenderTask()
    {
      return Task.Factory.StartNew((Action) (() => KafkaEventLogger.WriteToFile()), TaskCreationOptions.LongRunning);
    }

    private static void WriteToFile()
    {
      ServerGlobals.TraceLog.WriteInfo(KafkaEventLogger.ClassName, "Starting background task for kafka event logging on thread..." + (object) Thread.CurrentThread.ManagedThreadId);
      JsonSerializerSettings settings = new JsonSerializerSettings();
      settings.Formatting = Formatting.None;
      settings.NullValueHandling = NullValueHandling.Ignore;
      settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
      settings.ContractResolver = (IContractResolver) new CamelCasePropertyNamesContractResolver();
      settings.TypeNameHandling = TypeNameHandling.All;
      while (true)
      {
        try
        {
          KafkaEventMessage kafkaEventMessage = KafkaEventLogger._items.Take();
          string text = JsonConvert.SerializeObject((object) kafkaEventMessage, settings);
          IApplicationLog logWriter = KafkaEventLogger._logWriters[kafkaEventMessage.SourceName];
          logWriter.WriteLine(text);
          logWriter.Flush();
        }
        catch (Exception ex)
        {
          ServerGlobals.TraceLog.WriteError(KafkaEventLogger.ClassName, "Failed to write to event file:  " + (object) ex);
        }
      }
    }
  }
}
