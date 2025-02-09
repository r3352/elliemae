// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Targets.FileLogTarget
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Events;
using Encompass.Diagnostics.Logging.Formatters;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

#nullable disable
namespace Encompass.Diagnostics.Logging.Targets
{
  public class FileLogTarget : ILogTarget, IDisposable
  {
    private const int MaxRetryCount = 10;
    private readonly IApplicationEventHandler _eventHandler;
    private readonly ILogFormatter _logFormatter;
    private StreamWriter writer = (StreamWriter) null;
    private DateTime lastReconnectAttempt = DateTime.MinValue;
    protected readonly string _logDir;
    protected string _fileName;

    public FileLogTarget(
      string logDir,
      string fileNameWithoutExtension,
      IApplicationEventHandler eventHandler,
      ILogFormatter logFormatter)
    {
      this._eventHandler = ArgumentChecks.IsNotNull<IApplicationEventHandler>(eventHandler, nameof (eventHandler));
      this._logFormatter = ArgumentChecks.IsNotNull<ILogFormatter>(logFormatter, nameof (logFormatter));
      this._logDir = ArgumentChecks.IsNotNull<string>(logDir, nameof (logDir));
      this._fileName = ArgumentChecks.IsNotNull<string>(fileNameWithoutExtension, nameof (fileNameWithoutExtension));
      this.Connect();
    }

    protected string _logPath => Path.Combine(this._logDir, this._fileName + ".log");

    private void Connect()
    {
      Directory.CreateDirectory(this._logDir);
      this.writer = new StreamWriter(this._logPath, true);
    }

    protected void Close()
    {
      try
      {
        if (this.writer != null)
        {
          this.writer.Flush();
          this.writer.Close();
        }
      }
      catch
      {
      }
      this.writer = (StreamWriter) null;
    }

    protected bool Reconnect()
    {
      this.Close();
      int num = 0;
      while (true)
      {
        try
        {
          this.Connect();
          this.lastReconnectAttempt = DateTime.MinValue;
          this._eventHandler.WriteApplicationEvent("Reconnected to log file at '" + this._logPath + "'", EventLogEntryType.Information, 1102);
          return true;
        }
        catch (IOException ex)
        {
        }
        if (++num <= 10)
          Thread.Sleep(100);
        else
          break;
      }
      this.lastReconnectAttempt = DateTime.Now;
      return false;
    }

    protected virtual void WriteInternal(Encompass.Diagnostics.Logging.Schema.Log log)
    {
      lock (this)
      {
        Exception innerException = (Exception) null;
        try
        {
          if (this.writer != null)
          {
            this._logFormatter.WriteFormatted(log, (TextWriter) this.writer);
            this.writer.WriteLine();
            return;
          }
        }
        catch (IOException ex)
        {
          innerException = (Exception) ex;
          this._eventHandler.WriteApplicationEvent("Disconected from log file at '" + this._logPath + "': " + ex.GetFullStackTrace(), EventLogEntryType.Warning, 1101);
        }
        if (this.shouldAttemptReconnect() && !this.Reconnect())
          throw new Exception("Cannot reconnect to EncompassServer log file '" + this._logPath + "'. Logging has been suspended.", innerException);
      }
    }

    public virtual void Write(Encompass.Diagnostics.Logging.Schema.Log log)
    {
      if (!log.SupportsFormatter(this._logFormatter.GetFormat()))
        return;
      this.WriteInternal(log);
    }

    private bool shouldAttemptReconnect()
    {
      return !(this.lastReconnectAttempt != DateTime.MinValue) || (DateTime.Now - this.lastReconnectAttempt).TotalMinutes > 5.0;
    }

    public void Flush()
    {
      lock (this)
      {
        Exception innerException = (Exception) null;
        try
        {
          if (this.writer != null)
          {
            this.writer.Flush();
            return;
          }
        }
        catch (IOException ex)
        {
          innerException = (Exception) ex;
          this._eventHandler.WriteApplicationEvent("Disconected from log file at '" + this._logPath + "': " + ex.GetFullStackTrace(), EventLogEntryType.Warning, 1101);
        }
        if (this.shouldAttemptReconnect() && !this.Reconnect())
          throw new Exception("Cannot reconnect to EncompassServer log file '" + this._logPath + "'. Logging has been suspended.", innerException);
      }
    }

    public void Dispose()
    {
      lock (this)
      {
        try
        {
          if (this.writer != null)
          {
            this.writer.Flush();
            this.writer.Dispose();
          }
        }
        catch
        {
        }
        this.writer = (StreamWriter) null;
      }
    }
  }
}
