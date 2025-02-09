// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.TraceLog
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Diagnostics;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class TraceLog : ITraceLog
  {
    private const string name = "Server�";
    private const string description = "Encompass Server Log�";
    private static FileSystemWatcher fsw = new FileSystemWatcher(Path.GetDirectoryName(EnConfigurationSettings.AppSettings.ConfigurationFile), Path.GetFileName(EnConfigurationSettings.AppSettings.ConfigurationFile));
    private static TraceSwitch sw = new TraceSwitch("Server", "Encompass Server Log");
    private static object switchLock = new object();
    private static Process currentProcess = Process.GetCurrentProcess();
    public static TraceLog Instance = new TraceLog();

    static TraceLog()
    {
      try
      {
        TraceLog.fsw.Changed += new FileSystemEventHandler(TraceLog.configFileChanged);
        TraceLog.fsw.EnableRaisingEvents = true;
      }
      catch
      {
      }
      Elli.Log.Logger.WriteMessage += new Elli.Log.Logger.TraceMessageEventHandler(TraceLog.onTraceMessageWritten);
      EllieMae.EMLite.DataAccess.ServerGlobals.TraceLog = (ITraceLog) TraceLog.Instance;
    }

    private TraceLog()
    {
    }

    public void WriteErrorI(string category, string message)
    {
      TraceLog.WriteError(category, message);
    }

    public void WriteDebugI(string category, string message)
    {
      TraceLog.WriteDebug(category, message);
    }

    public void WriteWarningI(string category, string message)
    {
      TraceLog.WriteWarning(category, message);
    }

    public void WriteInfoI(string category, string message)
    {
      TraceLog.WriteInfo(category, message);
    }

    public void WriteVerboseI(string category, string message)
    {
      TraceLog.WriteVerbose(category, message);
    }

    public void WriteSqlTraceI(string category, DateTime start)
    {
      TraceLog.WriteSqlTrace(category, start);
    }

    public void WriteExceptionI(string category, Exception ex)
    {
      TraceLog.WriteException(category, ex);
    }

    public TraceLevel TraceLevelI
    {
      get => TraceLog.TraceLevel;
      set => TraceLog.TraceLevel = value;
    }

    public static TraceLevel TraceLevel
    {
      get
      {
        lock (TraceLog.switchLock)
          return EllieMae.EMLite.DataAccess.ServerGlobals.Debug ? TraceLevel.Verbose : TraceLog.sw.Level;
      }
      set
      {
        lock (TraceLog.switchLock)
          TraceLog.sw.Level = value;
      }
    }

    public static string DisplayName
    {
      get
      {
        lock (TraceLog.switchLock)
          return TraceLog.sw.DisplayName;
      }
    }

    public static void Write(TraceLevel level, string className, string message)
    {
      IClientContext current = ClientContext.GetCurrent(false);
      if (current == null)
        ServerGlobals.TraceLog.Write(level, className, message);
      else
        current.TraceLog.Write(level, className, message);
    }

    public static void Write(Encompass.Diagnostics.Logging.LogLevel level, string className, string message)
    {
      IClientContext current = ClientContext.GetCurrent(false);
      if (current == null)
        ServerGlobals.TraceLog.Write(level, className, message);
      else
        current.TraceLog.Write(level, className, message);
    }

    public static void WriteException(TraceLevel level, string className, Exception ex)
    {
      IClientContext current = ClientContext.GetCurrent(false);
      if (current == null)
        ServerGlobals.TraceLog.WriteException(level, className, ex);
      else
        current.TraceLog.WriteException(level, className, ex);
    }

    public static void WriteException(string className, Exception ex)
    {
      TraceLog.WriteException(TraceLevel.Error, className, ex);
    }

    public static void WriteInfo(string className, string message)
    {
      TraceLog.Write(TraceLevel.Info, className, message);
    }

    public static void WriteVerbose(string className, string message)
    {
      TraceLog.Write(TraceLevel.Verbose, className, message);
    }

    public static void WriteWarning(string className, string message)
    {
      TraceLog.Write(TraceLevel.Warning, className, message);
    }

    public static void WriteError(string className, string message)
    {
      TraceLog.Write(TraceLevel.Error, className, message);
    }

    public static void WriteDebug(string className, string message)
    {
      if (!EllieMae.EMLite.DataAccess.ServerGlobals.Debug)
        return;
      TraceLog.Write(TraceLevel.Verbose, className, message);
    }

    public static void WriteApi(string className, string apiName, params object[] parms)
    {
      if (ClientContext.GetCurrent(false) != null)
      {
        ClientContext.GetCurrent().RecordClassName(className);
        ClientContext.GetCurrent().RecordApiName(apiName);
        ClientContext.GetCurrent().RecordParms(parms);
      }
      if (ServerGlobals.APILatency != TimeSpan.Zero)
        Thread.Sleep(ServerGlobals.APILatency);
      if (ServerGlobals.APITrace || !EllieMae.EMLite.DataAccess.ServerGlobals.Debug)
        return;
      TraceLog.Write(TraceLevel.Verbose, className, "API: " + apiName + "(" + TraceLog.paramsToString(parms) + ") on thread " + (object) Thread.CurrentThread.GetHashCode());
    }

    public static void WriteApi(
      ISession session,
      string className,
      string apiName,
      params object[] parms)
    {
      if (ServerGlobals.APILatency != TimeSpan.Zero)
        Thread.Sleep(ServerGlobals.APILatency);
      ILogger logger = DiagUtility.LogManager.GetLogger("APITrace");
      if (!logger.IsEnabled(Encompass.Diagnostics.Logging.LogLevel.DEBUG))
        return;
      ApiTraceLog apiTrace = TraceLog.GetApiTrace(session, className, apiName, new DateTime?(), new DateTime?(), parms);
      logger.Write<ApiTraceLog>(apiTrace);
    }

    internal static void WriteApiTime(
      ISession session,
      string className,
      string apiName,
      DateTime startTime,
      params object[] parms)
    {
      if (ServerGlobals.APILatency != TimeSpan.Zero)
        Thread.Sleep(ServerGlobals.APILatency);
      ILogger logger = DiagUtility.LogManager.GetLogger("APITrace");
      if (!logger.IsEnabled(Encompass.Diagnostics.Logging.LogLevel.DEBUG))
        return;
      ApiTraceLog apiTrace = TraceLog.GetApiTrace(session, className, apiName, new DateTime?(startTime), new DateTime?(DateTime.UtcNow), parms);
      logger.Write<ApiTraceLog>(apiTrace);
    }

    public static void WriteSqlTrace(string commandText, DateTime start)
    {
      DateTime utcNow = DateTime.UtcNow;
      TimeSpan timeSpan = utcNow - start;
      ILogger logger = DiagUtility.LogManager.GetLogger("SQLTrace");
      if (!logger.IsEnabled(Encompass.Diagnostics.Logging.LogLevel.INFO) && !(timeSpan > TimeSpan.FromSeconds(5.0)))
        return;
      bool flag = logger.IsEnabled(Encompass.Diagnostics.Logging.LogLevel.DEBUG);
      SqlTraceLog log = new SqlTraceLog();
      if (timeSpan > TimeSpan.FromSeconds(5.0))
        log.Level = Encompass.Diagnostics.Logging.LogLevel.WARN;
      else if (flag)
        log.Level = Encompass.Diagnostics.Logging.LogLevel.DEBUG;
      else
        log.Level = Encompass.Diagnostics.Logging.LogLevel.INFO;
      if (flag)
        log.SetCallStack(new StackTrace(2));
      log.SetCommandText(commandText);
      log.Set<DateTime>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.StartTime, start);
      log.Set<DateTime>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.EndTime, utcNow);
      log.Set<double>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.DurationMS, (utcNow - start).TotalMilliseconds);
      logger.Write<SqlTraceLog>(log);
    }

    private static ApiTraceLog GetApiTrace(
      ISession session,
      string className,
      string apiName,
      DateTime? start,
      DateTime? end,
      params object[] parms)
    {
      ApiTraceLog apiTrace = new ApiTraceLog();
      IApiSourceContext current = APICallContext.GetCurrent();
      if (current != null)
        apiTrace.SetCallerInfo(current.SourceType.ToString(), current.SourceApp, current.SourceAssembly, current.SourceEvent);
      if (session != null)
        apiTrace.SetSessionInfo(session.LoginParams.AppName, session.UserID, session.SessionID);
      apiTrace.Message = className + "." + apiName + "(" + TraceLog.paramsToString(parms) + ")";
      if (start.HasValue && end.HasValue)
      {
        apiTrace.Set<DateTime>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.StartTime, start.Value);
        apiTrace.Set<DateTime>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.EndTime, end.Value);
        apiTrace.Set<double>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.DurationMS, (end.Value - start.Value).TotalMilliseconds);
      }
      return apiTrace;
    }

    private static void configFileChanged(object sender, FileSystemEventArgs e)
    {
      try
      {
        XmlDocument configFile = new XmlDocument();
        configFile.Load(e.FullPath);
        lock (TraceLog.switchLock)
          TraceLog.sw = TraceLog.readSwitch(configFile, TraceLog.sw.DisplayName);
      }
      catch
      {
      }
    }

    private static TraceSwitch readSwitch(XmlDocument configFile, string name)
    {
      TraceSwitch traceSwitch = new TraceSwitch(name, name);
      try
      {
        XmlElement xmlElement = (XmlElement) configFile.SelectSingleNode("configuration/system.diagnostics/switches/add[@name='" + name + "']");
        if (xmlElement != null)
          traceSwitch.Level = (TraceLevel) int.Parse(xmlElement.GetAttribute("value"));
      }
      catch
      {
      }
      return traceSwitch;
    }

    private static string paramsToString(object[] parms)
    {
      if (parms == null || parms.Length == 0)
        return "";
      StringBuilder stringBuilder = new StringBuilder();
      int result = int.MaxValue;
      int.TryParse(EnConfigurationSettings.AppSettings["APIParameterCountLoggingLimit"], out result);
      if (parms.Length > result)
        stringBuilder.Append(string.Format("Parameters count ({0}), exceeded the configured logging limit ({1}). Logging only till the configured limit. ", (object) parms.Length, (object) result));
      int num = Math.Min(parms.Length, result);
      for (int index = 0; index < num; ++index)
      {
        object parm = parms[index];
        switch (parm)
        {
          case null:
            stringBuilder.Append("null");
            break;
          case string _:
            stringBuilder.Append("\"").Append(parm).Append("\"");
            break;
          case Enum _:
            stringBuilder.Append(parm.GetType().Name).Append(".").Append(parm);
            break;
          case ValueType _:
            stringBuilder.Append(parm.ToString());
            break;
          default:
            if ((object) (parm as LoanIdentity) != null)
            {
              stringBuilder.Append("<LoanIdentity(").Append(parm.ToString()).Append(")>");
              break;
            }
            switch (parm)
            {
              case Array _:
                Array array = (Array) parm;
                Type elementType = array.GetType().GetElementType();
                stringBuilder.Append("<").Append(elementType.Name).Append("[").Append(array.Length).Append("]>");
                break;
              case BinaryObject _:
                BinaryObject binaryObject = (BinaryObject) parm;
                stringBuilder.Append("<BinaryObject:").Append(binaryObject.Length).Append(" byte>");
                break;
              default:
                stringBuilder.Append("<").Append(parm.GetType().Name).Append(">");
                break;
            }
            break;
        }
        stringBuilder.Append(index < parms.Length - 1 ? ", " : "");
      }
      return stringBuilder.ToString();
    }

    private static void onTraceMessageWritten(TraceMessageEventArgs e)
    {
      IClientContext current = ClientContext.GetCurrent(false);
      if (current != null)
      {
        current.TraceLog.Write(e.Level, e.Message, e.Category);
        e.Cancel = true;
      }
      else
      {
        ServerGlobals.TraceLog.Write(e.Level, e.Message, e.Category);
        e.Cancel = true;
      }
    }
  }
}
