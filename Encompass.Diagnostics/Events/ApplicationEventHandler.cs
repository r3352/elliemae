// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Events.ApplicationEventHandler
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using System.Diagnostics;

#nullable disable
namespace Encompass.Diagnostics.Events
{
  public class ApplicationEventHandler : IApplicationEventHandler
  {
    private readonly string _applicationName;
    private readonly EventLog _appEventLog;

    public ApplicationEventHandler(string applicationName)
    {
      this._applicationName = ArgumentChecks.IsNotNullOrEmpty(applicationName, nameof (applicationName));
      this._appEventLog = new EventLog();
      this._appEventLog.BeginInit();
      this._appEventLog.Log = "Application";
      this._appEventLog.Source = "Encompass";
      this._appEventLog.EndInit();
    }

    public void WriteApplicationEvent(string message, EventLogEntryType eventType, int eventID)
    {
      try
      {
        this._appEventLog.WriteEntry("[" + this._applicationName + "] " + message, eventType, eventID);
      }
      catch
      {
      }
    }
  }
}
