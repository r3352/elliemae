// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Targets.EventLogTarget
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Events;
using Encompass.Diagnostics.Logging.Formatters;
using System;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace Encompass.Diagnostics.Logging.Targets
{
  public class EventLogTarget : ILogTarget, IDisposable
  {
    private readonly EventLog _appEventLog;
    private readonly ILogFormatter _formatter;
    private readonly IApplicationEventHandler _eventHandler;

    public EventLogTarget(
      string source,
      IApplicationEventHandler eventHandler,
      ILogFormatter formatter)
    {
      this._formatter = ArgumentChecks.IsNotNull<ILogFormatter>(formatter, nameof (formatter));
      this._eventHandler = ArgumentChecks.IsNotNull<IApplicationEventHandler>(eventHandler, nameof (eventHandler));
      ArgumentChecks.IsNotNullOrEmpty(source, nameof (source));
      try
      {
        if (!EventLog.SourceExists(source))
          EventLog.CreateEventSource(source, "Application");
        while (!EventLog.SourceExists(source))
          Thread.Sleep(500);
      }
      catch (Exception ex)
      {
        this._eventHandler.WriteApplicationEvent("Exception while attempting to write log: " + ex.Message, EventLogEntryType.Error, 1102);
      }
      this._appEventLog = new EventLog();
      this._appEventLog.BeginInit();
      this._appEventLog.Log = "Application";
      this._appEventLog.Source = source;
      this._appEventLog.EndInit();
    }

    public void Write(Encompass.Diagnostics.Logging.Schema.Log log)
    {
      if (!log.SupportsFormatter(this._formatter.GetFormat()))
        return;
      try
      {
        this._appEventLog.WriteEntry(this._formatter.FormatLog(log), EventLogEntryType.Information);
      }
      catch (Exception ex)
      {
        this._eventHandler.WriteApplicationEvent("Exception while attempting to write log: " + ex.Message, EventLogEntryType.Warning, 1100);
      }
    }

    public void Dispose() => this._appEventLog.Close();

    public void Flush()
    {
    }
  }
}
