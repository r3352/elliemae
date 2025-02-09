// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Targets.FileLogTargetFactory
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Events;
using Encompass.Diagnostics.Logging.Formatters;
using System;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace Encompass.Diagnostics.Logging.Targets
{
  public class FileLogTargetFactory : LogTargetFactoryBase, ILogTargetFactory
  {
    private readonly IApplicationEventHandler _eventHandler;
    private readonly ILogFormatter _logFormatter;
    private readonly FileLogRolloverFrequency _rolloverFrequency;
    private readonly string _logDir;
    protected readonly string _rootFileName;
    private readonly bool _appendRolloverTimestampToActiveLog;

    public FileLogTargetFactory(
      ILogManager logManager,
      string logDir,
      string rootFileName,
      FileLogRolloverFrequency rolloverFrequency,
      bool appendServerNameToLogFileName,
      bool appendRolloverTimestampToActiveLog,
      IApplicationEventHandler eventHandler,
      ILogFormatter logFormatter)
      : this(logManager, logDir, rootFileName, rolloverFrequency, appendServerNameToLogFileName, appendRolloverTimestampToActiveLog, eventHandler, logFormatter, (IAsyncTargetWrapperFactory) null)
    {
    }

    public FileLogTargetFactory(
      ILogManager logManager,
      string logDir,
      string rootFileName,
      FileLogRolloverFrequency rolloverFrequency,
      bool appendServerNameToLogFileName,
      bool appendRolloverTimestampToActiveLog,
      IApplicationEventHandler eventHandler,
      ILogFormatter logFormatter,
      IAsyncTargetWrapperFactory asyncTargetWrapperFactory)
      : base(logManager, asyncTargetWrapperFactory)
    {
      this._logDir = ArgumentChecks.IsNotNullOrEmpty(logDir, nameof (logDir));
      this._rootFileName = (string.IsNullOrEmpty(rootFileName) ? this._logManager.AppName : rootFileName) + (appendServerNameToLogFileName ? "." + Environment.MachineName : string.Empty);
      this._rolloverFrequency = rolloverFrequency;
      this._appendRolloverTimestampToActiveLog = appendRolloverTimestampToActiveLog;
      this._eventHandler = ArgumentChecks.IsNotNull<IApplicationEventHandler>(eventHandler, nameof (eventHandler));
      this._logFormatter = ArgumentChecks.IsNotNull<ILogFormatter>(logFormatter, nameof (logFormatter));
    }

    protected override ILogTarget GetTargetInternal()
    {
      string rootFileName = this._rootFileName;
      try
      {
        return this._rolloverFrequency == FileLogRolloverFrequency.None ? (ILogTarget) new FileLogTarget(this._logDir, rootFileName, this._eventHandler, this._logFormatter) : (this._appendRolloverTimestampToActiveLog ? (ILogTarget) new RollingFileLogTarget(this._logDir, rootFileName, this._rolloverFrequency, this._eventHandler, this._logFormatter) : (ILogTarget) new ArchivingFileLogTarget(this._logDir, rootFileName, this._rolloverFrequency, this._eventHandler, this._logFormatter));
      }
      catch (IOException ex)
      {
        string str = rootFileName + "." + (object) AppDomain.CurrentDomain.Id + "." + (object) Process.GetCurrentProcess().Id;
        return this._rolloverFrequency == FileLogRolloverFrequency.None ? (ILogTarget) new FileLogTarget(this._logDir, str, this._eventHandler, this._logFormatter) : (this._appendRolloverTimestampToActiveLog ? (ILogTarget) new RollingFileLogTarget(this._logDir, str, this._rolloverFrequency, this._eventHandler, this._logFormatter) : (ILogTarget) new ArchivingFileLogTarget(this._logDir, str, this._rolloverFrequency, this._eventHandler, this._logFormatter));
      }
    }
  }
}
