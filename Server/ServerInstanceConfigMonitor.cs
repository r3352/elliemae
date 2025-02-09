// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerInstanceConfigMonitor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class ServerInstanceConfigMonitor
  {
    private const string className = "ServerInstanceConfigMonitor�";
    private readonly Timer _monitorTimer;
    private static ServerInstanceConfigMonitor Instance;

    public ServerInstanceConfigMonitor()
    {
      this._monitorTimer = new Timer(new TimerCallback(this.updateServerCache), (object) null, TimeSpan.Zero, ServerGlobals.InstanceConfigRefreshInterval);
    }

    private void updateServerCache(object state)
    {
      foreach (ClientContext clientContext in ClientContext.GetAll())
      {
        try
        {
          if (!clientContext.Settings.Disabled)
            clientContext.ResetCacheStore(false);
        }
        catch (Exception ex)
        {
          ServerGlobals.TraceLog.WriteError(nameof (ServerInstanceConfigMonitor), "Error attempting to reset cachestore for instance '" + clientContext.InstanceName + "'");
          ServerGlobals.TraceLog.WriteException(nameof (ServerInstanceConfigMonitor), ex);
        }
      }
    }

    public void ChangeInterval(TimeSpan timerInterval)
    {
      this._monitorTimer.Change(TimeSpan.Zero, timerInterval);
    }

    public static void Start()
    {
      if (ServerInstanceConfigMonitor.Instance != null)
        return;
      ServerInstanceConfigMonitor.Instance = new ServerInstanceConfigMonitor();
    }

    public static void ChangeTimerInterval(TimeSpan timerInterval)
    {
      if (ServerInstanceConfigMonitor.Instance == null)
        return;
      ServerInstanceConfigMonitor.Instance.ChangeInterval(timerInterval);
    }
  }
}
