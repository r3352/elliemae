// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Logging.FileSystemLog
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Logging
{
  public class FileSystemLog : TraceListener, IApplicationLog
  {
    private const int MaxRetryCount = 10;
    private StreamWriter writer;
    private DateTime lastReconnectAttempt = DateTime.MinValue;

    public event EventHandler Disconnect;

    public event EventHandler Reconnect;

    public FileSystemLog(string logPath)
    {
      this.LogPath = logPath;
      this.connect();
    }

    public FileSystemLog(string logPath, StreamWriter logWriter)
    {
      this.LogPath = logPath;
      this.writer = logWriter;
    }

    public string LogPath { get; private set; }

    public void ChangePath(string newLogPath)
    {
      lock (this)
      {
        this.LogPath = newLogPath;
        this.reconnect(true);
      }
    }

    public override void Write(string text)
    {
      lock (this)
      {
        Exception innerException = (Exception) null;
        try
        {
          if (this.writer != null)
          {
            this.writer.Write(text);
            return;
          }
        }
        catch (IOException ex)
        {
          innerException = (Exception) ex;
          this.OnDisconnect(EventArgs.Empty);
        }
        if (this.shouldAttemptReconnect() && !this.reconnect(true))
          throw new Exception("Cannot reconnect to EncompassServer log file '" + this.LogPath + "'. Logging has been suspended.", innerException);
      }
    }

    public override void WriteLine(string text) => this.Write(text + Environment.NewLine);

    public override void Flush()
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
          this.OnDisconnect(EventArgs.Empty);
        }
        if (this.shouldAttemptReconnect() && !this.reconnect(true))
          throw new Exception("Cannot reconnect to EncompassServer log file '" + this.LogPath + "'. Logging has been suspended.", innerException);
      }
    }

    public override void Close()
    {
      lock (this)
      {
        try
        {
          if (this.writer == null)
            return;
          this.writer.Close();
        }
        catch
        {
        }
        finally
        {
          this.writer = (StreamWriter) null;
        }
      }
    }

    public void Reset()
    {
      lock (this)
      {
        this.Close();
        this.reconnect(false);
      }
    }

    private bool shouldAttemptReconnect()
    {
      return !(this.lastReconnectAttempt != DateTime.MinValue) || (DateTime.Now - this.lastReconnectAttempt).TotalMinutes > 5.0;
    }

    protected void OnDisconnect(EventArgs e)
    {
      try
      {
        if (this.Disconnect == null)
          return;
        this.Disconnect((object) this, e);
      }
      catch
      {
      }
    }

    protected void OnReconnect(EventArgs e)
    {
      try
      {
        if (this.Reconnect == null)
          return;
        this.Reconnect((object) this, e);
      }
      catch
      {
      }
    }

    private void connect()
    {
      Directory.CreateDirectory(Path.GetDirectoryName(this.LogPath));
      this.writer = new StreamWriter(this.LogPath, true);
    }

    private bool reconnect(bool append)
    {
      try
      {
        if (this.writer != null)
          this.writer.Close();
      }
      catch
      {
      }
      this.writer = (StreamWriter) null;
      int num = 0;
      while (true)
      {
        try
        {
          this.connect();
          this.lastReconnectAttempt = DateTime.MinValue;
          this.OnReconnect(EventArgs.Empty);
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
  }
}
