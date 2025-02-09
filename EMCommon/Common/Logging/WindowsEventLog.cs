// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Logging.WindowsEventLog
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Logging;
using System;
using System.Configuration;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Common.Logging
{
  public class WindowsEventLog : IApplicationLog
  {
    private readonly EventLog appEventLog;

    public WindowsEventLog(string instanceId)
    {
      this.appEventLog = new EventLog();
      try
      {
        string str1 = "Application";
        string str2 = ConfigurationManager.AppSettings["AppNameforLog"];
        if (!string.IsNullOrWhiteSpace(instanceId))
          str2 = str2 + "_" + instanceId;
        this.appEventLog.BeginInit();
        this.appEventLog.Log = str1;
        this.appEventLog.Source = str2;
        this.appEventLog.EndInit();
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to initialize Application Event log: " + ex.Message, ex);
      }
    }

    public void Write(string text)
    {
      if (this.appEventLog == null)
        return;
      try
      {
        this.appEventLog.WriteEntry(text, EventLogEntryType.Information);
      }
      catch
      {
      }
    }

    public void WriteLine(string text) => this.Write(text + Environment.NewLine);

    public void Flush()
    {
    }

    public void Close()
    {
    }

    public void Reset() => this.Flush();
  }
}
