// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Tracing
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common.Diagnostics;
using EllieMae.EMLite.Common.Diagnostics.ConfigChangeHandlers;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Config;
using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.Logging.Schema;
using Encompass.Diagnostics.PII;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [NoTrace]
  public class Tracing
  {
    private const string className = "Tracing";
    public static readonly string SwSYSTEM = "SYSTEM";
    public static readonly string SwCommon = "Common";
    public static readonly string SwRemoting = "Remoting";
    public static readonly string SwOutsideLoan = "OutsideLoan";
    public static readonly string SwDataEngine = "DataEngine";
    public static readonly string SwInputEngine = "InputEngine";
    public static readonly string SwPrintEngine = "PrintEngine";
    public static readonly string SwCustomLetters = "CustomLetters";
    public static readonly string SwEpass = "ePass";
    public static readonly string SwConcurrentUpdates = "ConcurrentUpdates";
    public static readonly string SwContact = "Contact";
    public static readonly string SwImportExport = "ImportExport";
    public static readonly string SwVersionControl = "VersionControl";
    public static readonly string SwReportControl = "Reporting";
    public static readonly string SwClickLoanProxy = "ClickLoanProxy";
    public static readonly string SwEFolder = "eFolder";
    public static readonly string SwStatusOnline = "StatusOnline";
    public static readonly string SwQuery = "Query";
    public static readonly string SwThinThick = "ThinThick";
    public static readonly string SwAutomation = "Automation";
    public static readonly string SwDeepLink = "DeepLink";
    public static string[] PredefinedSwitches = new string[21]
    {
      Tracing.SwSYSTEM,
      Tracing.SwCommon,
      Tracing.SwRemoting,
      Tracing.SwOutsideLoan,
      Tracing.SwDataEngine,
      Tracing.SwInputEngine,
      Tracing.SwPrintEngine,
      Tracing.SwCustomLetters,
      Tracing.SwEpass,
      Tracing.SwContact,
      Tracing.SwImportExport,
      Tracing.SwVersionControl,
      Tracing.SwReportControl,
      Tracing.SwClickLoanProxy,
      Tracing.SwEFolder,
      Tracing.SwStatusOnline,
      Tracing.SwQuery,
      Tracing.SwThinThick,
      Tracing.SwAutomation,
      Tracing.SwConcurrentUpdates,
      Tracing.SwDeepLink
    };
    private static bool initialized = false;
    private static string logFile = (string) null;
    private static bool debugEnabled = false;
    private static bool sqlTraceEnabled = false;
    private static ClassicLogConfigChangeHandler classicLogConfigChangeHandler;
    private static CloudLogConfigChangeHandler cloudLogConfigChangeHandler;
    private static Tracing.SendErrorToServerType senderFunc;
    private static Tracing.SendErrorToServerType milestoneDateSenderFunc;
    private static Tracing.SendErrorToServerType dtLogSenderFunc;
    private static readonly Dictionary<string, Encompass.Diagnostics.Logging.LogLevel> legacyLevelStrLogLevelMap = new Dictionary<string, Encompass.Diagnostics.Logging.LogLevel>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      {
        "DEBUG",
        Encompass.Diagnostics.Logging.LogLevel.DEBUG
      },
      {
        "VERBOSE",
        Encompass.Diagnostics.Logging.LogLevel.DEBUG
      },
      {
        "CALCULATION",
        Encompass.Diagnostics.Logging.LogLevel.DEBUG
      },
      {
        "DDM_DIAGNOTICS_DETAIL",
        Encompass.Diagnostics.Logging.LogLevel.DEBUG
      },
      {
        "DDM_DIAGNOSTICS_HEADER",
        Encompass.Diagnostics.Logging.LogLevel.DEBUG
      },
      {
        "DDM_DIAGNOSTICS_PERF",
        Encompass.Diagnostics.Logging.LogLevel.DEBUG
      },
      {
        "INFO",
        Encompass.Diagnostics.Logging.LogLevel.INFO
      },
      {
        "INFORMATION",
        Encompass.Diagnostics.Logging.LogLevel.INFO
      },
      {
        "WARNING",
        Encompass.Diagnostics.Logging.LogLevel.WARN
      },
      {
        "WARN",
        Encompass.Diagnostics.Logging.LogLevel.WARN
      },
      {
        "ERROR",
        Encompass.Diagnostics.Logging.LogLevel.ERROR
      },
      {
        "ERR",
        Encompass.Diagnostics.Logging.LogLevel.ERROR
      },
      {
        "INIT",
        Encompass.Diagnostics.Logging.LogLevel.DEBUG
      }
    };
    private static readonly Dictionary<string, LogEventType> legacyLevelStrEventTypeMap = new Dictionary<string, LogEventType>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      {
        "INIT",
        LogEventType.Start
      }
    };
    private static readonly Dictionary<string, string> legacyLevelStrLoggerNameMap = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      {
        "CALCULATION",
        "Calculation.Diagnostics"
      },
      {
        "DDM_DIAGNOTICS_DETAIL",
        "DDM.Diagnostics"
      },
      {
        "DDM_DIAGNOSTICS_HEADER",
        "DDM.Diagnostics"
      },
      {
        "DDM_DIAGNOSTICS_PERF",
        "DDM.Diagnostics"
      }
    };

    public static bool DefaultTraceListenerEnabled { get; private set; }

    public static void Init(string logFileBasePath) => Tracing.Init(logFileBasePath, false);

    public static void Init(string logFileBasePath, bool server)
    {
      if (Tracing.initialized)
        return;
      Tracing.initialized = true;
      Tracing.DefaultTraceListenerEnabled = true;
      string message = "Application starts at " + DateTime.Now.ToString();
      Directory.CreateDirectory(Path.GetDirectoryName(logFileBasePath));
      int num = 0;
label_2:
      Tracing.logFile = (string) null;
      FileStream fileStream;
      try
      {
        string str = num == 0 ? string.Empty : num.ToString();
        Tracing.logFile = logFileBasePath + str + ".log";
        fileStream = !server ? new FileStream(Tracing.logFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite) : new FileStream(Tracing.logFile, FileMode.Append, FileAccess.Write, FileShare.Read);
      }
      catch (Exception ex)
      {
        if (++num >= 1000)
          throw new Exception("Unable to create log file '" + logFileBasePath + "': " + ex.Message);
        goto label_2;
      }
      Trace.AutoFlush = true;
      Trace.Listeners.Add((TraceListener) new TextWriterTraceListener((Stream) fileStream));
      DiagUtility.SetLoggerScopeProvider((ILoggerScopeProvider) new ClientScopeProvider());
      EnConfigurationSettings.GlobalSettings.SettingsChanged += new EventHandler(Tracing.GlobalSettings_SettingsChanged);
      Tracing.refreshSettings();
      Tracing.classicLogConfigChangeHandler = new ClassicLogConfigChangeHandler();
      DiagConfig<ClientDiagConfigData>.Instance.AddHandler((IDiagConfigChangeHandler<ClientDiagConfigData>) Tracing.classicLogConfigChangeHandler);
      Tracing.cloudLogConfigChangeHandler = new CloudLogConfigChangeHandler();
      DiagConfig<ClientDiagConfigData>.Instance.AddHandler((IDiagConfigChangeHandler<ClientDiagConfigData>) Tracing.cloudLogConfigChangeHandler);
      if (server)
        message = "-------------------" + Environment.NewLine + message;
      Trace.WriteLine(message);
    }

    public static void InitSendToServerDelegate(Tracing.SendErrorToServerType del, string target)
    {
      switch (target)
      {
        case "MileStoneFinishedDateLog":
          if (Tracing.milestoneDateSenderFunc != null)
            break;
          Tracing.milestoneDateSenderFunc = del;
          break;
        case "DisclosureTrackingLogs":
          if (Tracing.dtLogSenderFunc != null)
            break;
          Tracing.dtLogSenderFunc = del;
          break;
        default:
          if (Tracing.senderFunc != null)
            break;
          Tracing.senderFunc = del;
          break;
      }
    }

    public static void SendBusinessRuleErrorToServer(TraceLevel level, string message)
    {
      try
      {
        if (Tracing.senderFunc == null)
          return;
        message = "Error guid: " + (object) Guid.NewGuid() + " - Business Rule Error: " + message;
        Tracing.senderFunc(level, message);
      }
      catch
      {
      }
    }

    public static void SendMessageToServer(TraceLevel level, string message)
    {
      try
      {
        if (Tracing.milestoneDateSenderFunc == null)
          return;
        Tracing.milestoneDateSenderFunc(level, message);
      }
      catch
      {
      }
    }

    public static void SendDTLogErrorMessageToServer(TraceLevel level, string message)
    {
      try
      {
        if (Tracing.dtLogSenderFunc == null)
          return;
        Tracing.dtLogSenderFunc(level, message);
      }
      catch
      {
      }
    }

    private static void GlobalSettings_SettingsChanged(object sender, EventArgs e)
    {
      Tracing.refreshSettings();
      DiagConfig<ClientDiagConfigData>.Instance.ReloadConfig();
    }

    public static string LogFile => Tracing.logFile;

    public static void Log(string sw, TraceLevel l, string className, string msg)
    {
      Tracing.Log(sw, className, l, msg);
    }

    public static void Log(string sw, string className, TraceLevel l, string msg)
    {
      DiagUtility.LogManager.GetLogger(sw).Write(l.ToLogLevel(), className, msg);
    }

    public static void Log(string sw, string className, TraceLevel l, string msg, long byteSize)
    {
      DiagUtility.LogManager.GetLogger(sw).Write(l.ToLogLevel(), className, msg + " size(" + Utils.FormatByteSize(byteSize) + ")");
    }

    public static TraceTimer StartTimer(string sw, string className, TraceLevel l, string msg)
    {
      ILogger logger = DiagUtility.LogManager.GetLogger(sw);
      TraceTimer traceTimer = (TraceTimer) null;
      Encompass.Diagnostics.Logging.LogLevel logLevel = l.ToLogLevel();
      if (logger.IsEnabled(logLevel))
        traceTimer = new TraceTimer(logger, className, msg, logLevel);
      return traceTimer;
    }

    public static void Log(bool trace, string levelStr, string className, string msg)
    {
      Encompass.Diagnostics.Logging.LogLevel logEventType1;
      if (!Tracing.legacyLevelStrLogLevelMap.TryGetValue(levelStr, out logEventType1))
        logEventType1 = Encompass.Diagnostics.Logging.LogLevel.INFO.Force();
      if (trace)
        logEventType1 = logEventType1.Force();
      string logger1;
      ILogger logger2 = !Tracing.legacyLevelStrLoggerNameMap.TryGetValue(levelStr, out logger1) ? DiagUtility.DefaultLogger : DiagUtility.LogManager.GetLogger(logger1);
      CustomLevelLog log = new CustomLevelLog(levelStr);
      log.Level = logEventType1;
      log.Src = className;
      log.Message = msg;
      LogEventType logEventType2;
      if (Tracing.legacyLevelStrEventTypeMap.TryGetValue(levelStr, out logEventType2))
        log.Set<LogEventType>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.EventType, logEventType2);
      logger2.Write<CustomLevelLog>(log);
    }

    public static string MaskPII(string message)
    {
      return MaskUtilities.MaskPII(message, Tracing.sqlTraceEnabled);
    }

    public static string Format(string levelStr, string className)
    {
      string str = "[" + DateTime.Now.ToString("MM/dd/yy H:mm:ss.ffff") + "] " + levelStr + " {" + Thread.CurrentThread.GetHashCode().ToString("000") + "}";
      if (className != "")
        str = str + " (" + className + ")";
      return str;
    }

    public static string Format(string levelStr, string className, DateTime startTime)
    {
      string str = "[" + startTime.ToString("MM/dd/yy H:mm:ss.ffff") + "] " + levelStr + " {" + Thread.CurrentThread.GetHashCode().ToString("000") + "}";
      if (className != "")
        str = str + " (" + className + ")";
      return str;
    }

    public static string Format(
      string levelStr,
      string className,
      DateTime startTime,
      string threadId)
    {
      string str = "[" + startTime.ToString("MM/dd/yy H:mm:ss.ffff") + "] " + levelStr + " {" + threadId.PadLeft(3, '0') + "}";
      if (className != "")
        str = str + " (" + className + ")";
      return str;
    }

    public static Dictionary<string, LogLevelFilter> GetLogLevels(string defaultLoggerName = "Server")
    {
      TraceSwitch traceSwitch1 = new TraceSwitch("Server", "");
      Dictionary<string, LogLevelFilter> levels = new Dictionary<string, LogLevelFilter>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
      {
        {
          defaultLoggerName,
          Tracing.debugEnabled ? LogLevelFilter.Verbose : LogLevelFilter.Error | traceSwitch1.Level.ToLogLevelFilter()
        }
      };
      foreach (string configuredSwitchName in Tracing.GetConfiguredSwitchNames())
      {
        try
        {
          TraceSwitch traceSwitch2 = new TraceSwitch(configuredSwitchName, "");
          levels[traceSwitch2.DisplayName] = Tracing.debugEnabled ? LogLevelFilter.Verbose : traceSwitch2.Level.ToLogLevelFilter();
        }
        catch (ConfigurationErrorsException ex)
        {
        }
      }
      foreach (string predefinedSwitch in Tracing.PredefinedSwitches)
      {
        if (!levels.ContainsKey(predefinedSwitch))
          levels[predefinedSwitch] = Tracing.debugEnabled ? LogLevelFilter.Verbose : LogLevelFilter.Error;
      }
      Tracing.AddLegacySwitches(levels);
      return levels;
    }

    private static IEnumerable<string> GetConfiguredSwitchNames()
    {
      HashSet<string> configuredSwitchNames = new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      string configurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load(configurationFile);
      XmlNodeList xmlNodeList = xmlDocument.SelectNodes("configuration/system.diagnostics/switches/add");
      if (xmlNodeList != null)
      {
        foreach (object obj in xmlNodeList)
        {
          XmlElement xmlElement = obj as XmlElement;
          configuredSwitchNames.Add(xmlElement.GetAttribute("name"));
        }
      }
      return (IEnumerable<string>) configuredSwitchNames;
    }

    private static void AddLegacySwitches(Dictionary<string, LogLevelFilter> levels)
    {
      levels["SQLTrace"] = !EnConfigurationSettings.GlobalSettings.SQLTrace ? LogLevelFilter.Warning : (EnConfigurationSettings.GlobalSettings.SQLTrace_IncludeAccessorName ? LogLevelFilter.Verbose : LogLevelFilter.Information);
      if (EnConfigurationSettings.GlobalSettings.APITrace || EnConfigurationSettings.GlobalSettings.Debug)
        levels["APITrace"] = LogLevelFilter.Verbose;
      if (EnConfigurationSettings.GlobalSettings.TraceLocks > 0)
        levels["LockDiagnostics"] = EnConfigurationSettings.GlobalSettings.TraceLocks > 1 ? LogLevelFilter.Verbose : LogLevelFilter.Warning;
      else
        levels["LockDiagnostics"] = LogLevelFilter.Off;
    }

    public static void Close()
    {
      Trace.WriteLine("Application stops at " + DateTime.Now.ToString());
      Trace.Close();
    }

    public static bool Debug
    {
      get => Tracing.debugEnabled;
      set => Tracing.debugEnabled = value;
    }

    public static bool IsSwitchActive(string logger, TraceLevel level)
    {
      return Tracing.debugEnabled || level == TraceLevel.Error || DiagUtility.LogManager.GetLogger(logger).IsEnabled(level.ToLogLevel());
    }

    public static string GetStackTrace() => Tracing.GetStackTrace(-1);

    public static string GetStackTrace(int numFrames)
    {
      List<string> stringList = new List<string>();
      StackTrace stackTrace = new StackTrace();
      int num = 0;
      foreach (StackFrame frame in stackTrace.GetFrames())
      {
        MethodBase method = frame.GetMethod();
        if (num != 0 || !(method == (MethodBase) null) && method.GetCustomAttributes(typeof (NoTraceAttribute), true).Length == 0 && !(method.DeclaringType == (Type) null) && method.DeclaringType.GetCustomAttributes(typeof (NoTraceAttribute), true).Length == 0)
        {
          stringList.Add("  at " + Tracing.GetStackFrameText(frame));
          if (numFrames > 0 && ++num >= numFrames)
            break;
        }
      }
      return string.Join(Environment.NewLine, stringList.ToArray());
    }

    public static string GetStackFrameText(StackFrame frame)
    {
      MethodBase method = frame.GetMethod();
      string str = method.DeclaringType.FullName + "." + method.Name + "(";
      int num = 0;
      foreach (ParameterInfo parameter in method.GetParameters())
        str = str + (num++ > 0 ? ", " : "") + parameter.ParameterType.FullName;
      return str + ")";
    }

    private static void refreshSettings()
    {
      lock (typeof (EnConfigurationSettings))
      {
        Tracing.debugEnabled = EnConfigurationSettings.GlobalSettings.Debug;
        Tracing.sqlTraceEnabled = EnConfigurationSettings.GlobalSettings.SQLTrace;
      }
    }

    public delegate void TraceMessageEventHandler(TraceMessageEventArgs e);

    public delegate void SendErrorToServerType(TraceLevel level, string message);
  }
}
