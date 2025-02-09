// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ClassicLogFactory
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Events;
using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.Logging.Filters;
using Encompass.Diagnostics.Logging.Formatters;
using Encompass.Diagnostics.Logging.Listeners;
using Encompass.Diagnostics.Logging.Schema;
using Encompass.Diagnostics.Logging.Targets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class ClassicLogFactory
  {
    private readonly ILogManager _logManager;
    private readonly IApplicationEventHandler _eventHandler;
    private readonly IAsyncTargetWrapperFactory _asyncTargetWrapperFactory;
    private readonly ServerDiagConfigData.ClassicLogConfigData _config;
    private readonly ConcurrentDictionary<LogFormat, ILogFormatter> _formatters;
    private readonly Dictionary<string, ClassicLogFactory.IContextLogTargetFactory> _targetFactories;

    public ClassicLogFactory(
      ILogManager logManager,
      IApplicationEventHandler eventHandler,
      IAsyncTargetWrapperFactory asyncTargetWrapperFactory,
      ServerDiagConfigData config)
    {
      this._logManager = logManager;
      this._eventHandler = eventHandler;
      this._asyncTargetWrapperFactory = asyncTargetWrapperFactory;
      this._config = config.LogListeners.ClassicLog;
      this._formatters = new ConcurrentDictionary<LogFormat, ILogFormatter>();
      this._targetFactories = new Dictionary<string, ClassicLogFactory.IContextLogTargetFactory>();
      if (this._config.MultiFileTarget.Enabled)
        this._targetFactories.Add(this._config.MultiFileTarget.Name, (ClassicLogFactory.IContextLogTargetFactory) new ClassicLogFactory.MultiFileTargetFactory(this));
      if (this._config.SingleFileTarget.Enabled)
        this._targetFactories.Add(this._config.SingleFileTarget.Name, (ClassicLogFactory.IContextLogTargetFactory) new ClassicLogFactory.SingleFileTargetFactory(this));
      if (!this._config.EventTarget.Enabled)
        return;
      this._targetFactories.Add(this._config.EventTarget.Name, (ClassicLogFactory.IContextLogTargetFactory) new ClassicLogFactory.EventTargetFactory(this));
    }

    public ILogFormatter GetFormatter(LogFormat format)
    {
      return this._formatters.GetOrAdd(format, (Func<LogFormat, ILogFormatter>) (f =>
      {
        switch (f)
        {
          case LogFormat.PlainTextWithInstance:
            return (ILogFormatter) new PlainTextWithInstanceFormatter();
          case LogFormat.LegacyJson:
            return (ILogFormatter) new LegacyJsonLogFormatter();
          case LogFormat.PrettyLegacyJson:
            return (ILogFormatter) new PrettyLegacyJsonLogFormatter();
          case LogFormat.Json:
            return (ILogFormatter) new JsonLogFormatter();
          case LogFormat.PrettyJson:
            return (ILogFormatter) new PrettyJsonLogFormatter();
          default:
            return (ILogFormatter) new PlainTextFormatter();
        }
      }));
    }

    public ILogListener GetLogListener(ClientContext context, bool refresh)
    {
      string str1 = context == null ? "Core" : (string.IsNullOrEmpty(context.InstanceName) ? "DEFAULT" : context.InstanceName);
      Dictionary<string, LogLevelFilter> logLevels = this._config.LogLevels;
      string defaultLogLevelName = this._config.DefaultLogLevelName;
      ILogListener listener = (ILogListener) new DefaultLogListener((ILogFilter) new LevelBasedFilter((IDictionary<string, LogLevelFilter>) logLevels, defaultLogLevelName), DiagUtility.ApplicationEventHandler);
      this._targetFactories.ToList<KeyValuePair<string, ClassicLogFactory.IContextLogTargetFactory>>().ForEach((Action<KeyValuePair<string, ClassicLogFactory.IContextLogTargetFactory>>) (item => listener.AddTarget(item.Key, item.Value.GetTarget(context))));
      string str2;
      if (!refresh)
        str2 = string.Format("Server instance '{0}' started at {1}. Log ({2}) Level: {3}", (object) str1, (object) DateTime.Now, (object) defaultLogLevelName, (object) logLevels[defaultLogLevelName]);
      else
        str2 = string.Format("DiagConfig refreshed for server instance '{0}' at {1}. Log ({2}) Level: {3}", (object) str1, (object) DateTime.Now, (object) defaultLogLevelName, (object) logLevels[defaultLogLevelName]);
      string str3 = str2;
      CustomLevelLog customLevelLog = new CustomLevelLog("SYSTEM");
      customLevelLog.AppName = DiagUtility.LogManager.AppName;
      if (!string.IsNullOrEmpty(DiagUtility.LogManager.EnvironmentName))
        customLevelLog.Environment = DiagUtility.LogManager.EnvironmentName;
      customLevelLog.Version = DiagUtility.LogManager.Version;
      customLevelLog.Host = DiagUtility.LogManager.ComputerName;
      customLevelLog.InstanceId = str1;
      customLevelLog.Timestamp = DateTime.UtcNow;
      customLevelLog.Message = str3;
      customLevelLog.Level = Encompass.Diagnostics.Logging.LogLevel.INFO.Force();
      customLevelLog.Src = DiagUtility.LogManager.AppName;
      customLevelLog.Set<LogEventType>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.EventType, refresh ? LogEventType.Reset : LogEventType.Start);
      customLevelLog.Pid = Process.GetCurrentProcess().Id;
      customLevelLog.ThreadId = Thread.CurrentThread.ManagedThreadId;
      listener.WriteToLogTargets((Encompass.Diagnostics.Logging.Schema.Log) customLevelLog);
      return listener;
    }

    public void RemoveTargets(ILogListener listener)
    {
      this._targetFactories.ToList<KeyValuePair<string, ClassicLogFactory.IContextLogTargetFactory>>().ForEach((Action<KeyValuePair<string, ClassicLogFactory.IContextLogTargetFactory>>) (item => item.Value.RemoveTarget(listener)));
    }

    public void DisposeTargets()
    {
      this._targetFactories.Values.ToList<ClassicLogFactory.IContextLogTargetFactory>().ForEach((Action<ClassicLogFactory.IContextLogTargetFactory>) (factory => factory.Dispose()));
    }

    private interface IContextLogTargetFactory : IDisposable
    {
      ILogTarget GetTarget(ClientContext context);

      void RemoveTarget(ILogListener listener);
    }

    private class MultiFileTargetFactory : ClassicLogFactory.IContextLogTargetFactory, IDisposable
    {
      private readonly ClassicLogFactory _parent;
      private readonly string _rootFileName;
      private readonly List<ILogTarget> _allTargets;

      public MultiFileTargetFactory(ClassicLogFactory parent)
      {
        this._parent = parent;
        this._rootFileName = parent._config.MultiFileTarget.RootFileName;
        this._allTargets = new List<ILogTarget>();
      }

      private string GetLogDirPath(ClientContext context)
      {
        string logDirPath = context == null ? EnConfigurationSettings.GlobalSettings.AppBaseLogDirectory : context.Settings.LogDir;
        if (!this._rootFileName.StartsWith("EncompassServer"))
          logDirPath = logDirPath.Replace("\\banker\\", "\\" + this._rootFileName + "\\");
        return logDirPath;
      }

      private string GetRootName(ClientContext context)
      {
        string rootName = this._rootFileName;
        if (context != null)
        {
          string str = context.InstanceName;
          if (string.IsNullOrEmpty(str))
            str = "DEFAULT";
          rootName = rootName + "." + str;
        }
        return rootName;
      }

      public ILogTarget GetTarget(ClientContext context)
      {
        ServerDiagConfigData.ClassicLogConfigData.MultiFileTargetConfigData multiFileTarget = this._parent._config.MultiFileTarget;
        ILogTarget target = new FileLogTargetFactory(this._parent._logManager, this.GetLogDirPath(context), this.GetRootName(context), multiFileTarget.RolloverFrequency, multiFileTarget.AppendServerNameToLogFileName, multiFileTarget.AppendRolloverTimestampToActiveLog, this._parent._eventHandler, this._parent.GetFormatter(multiFileTarget.Format), this._parent._asyncTargetWrapperFactory).GetTarget();
        lock (this._allTargets)
          this._allTargets.Add(target);
        return target;
      }

      public void Dispose()
      {
        Parallel.ForEach<ILogTarget>((IEnumerable<ILogTarget>) this._allTargets, (Action<ILogTarget>) (target => target.Dispose()));
      }

      public void RemoveTarget(ILogListener listener)
      {
        listener.RemoveTarget(this._parent._config.MultiFileTarget.Name, false);
      }
    }

    private class SingleFileTargetFactory : ClassicLogFactory.IContextLogTargetFactory, IDisposable
    {
      private readonly ILogTarget _singletonTarget;

      public SingleFileTargetFactory(ClassicLogFactory parent)
      {
        ServerDiagConfigData.ClassicLogConfigData.SingleFileTargetConfigData singleFileTarget = parent._config.SingleFileTarget;
        this._singletonTarget = new FileLogTargetFactory(parent._logManager, singleFileTarget.BaseDir, singleFileTarget.RootFileName, singleFileTarget.RolloverFrequency, singleFileTarget.AppendServerNameToLogFileName, singleFileTarget.AppendRolloverTimestampToActiveLog, parent._eventHandler, parent.GetFormatter(singleFileTarget.Format), parent._asyncTargetWrapperFactory).GetTarget();
      }

      public ILogTarget GetTarget(ClientContext context) => this._singletonTarget;

      public void RemoveTarget(ILogListener listener)
      {
        listener.RemoveTarget(this._singletonTarget, false);
      }

      public void Dispose() => this._singletonTarget.Dispose();
    }

    private class EventTargetFactory : ClassicLogFactory.IContextLogTargetFactory, IDisposable
    {
      private readonly ILogTarget _singletonTarget;

      public EventTargetFactory(ClassicLogFactory parent)
      {
        this._singletonTarget = new EventLogTargetFactory(parent._logManager, parent.GetFormatter(parent._config.EventTarget.Format), parent._eventHandler, parent._asyncTargetWrapperFactory).GetTarget();
      }

      public ILogTarget GetTarget(ClientContext context) => this._singletonTarget;

      public void RemoveTarget(ILogListener listener)
      {
        listener.RemoveTarget(this._singletonTarget, false);
      }

      public void Dispose() => this._singletonTarget.Dispose();
    }
  }
}
