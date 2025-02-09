// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Client.ConnectionManager
// Assembly: Client, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: AD6D6217-37E4-4BE3-B44A-5E3BA190AF3A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Client.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Client
{
  internal static class ConnectionManager
  {
    private const string className = "ConnectionManager";
    private static readonly TimeSpan pingInterval = TimeSpan.FromSeconds(80.0);
    private static readonly string sw = Tracing.SwRemoting;
    public static readonly string ConnectionID = Guid.NewGuid().ToString();
    private static Dictionary<ServerIdentity, Thread> pingThreads = new Dictionary<ServerIdentity, Thread>();

    static ConnectionManager()
    {
      try
      {
        ConnectionManager.pingInterval = TimeSpan.FromSeconds((double) int.Parse(string.Concat(EnConfigurationSettings.GlobalSettings["PingInterval"])));
      }
      catch
      {
      }
    }

    public static void StartMonitoring(ServerIdentity serverId)
    {
      lock (ConnectionManager.pingThreads)
      {
        if (ConnectionManager.pingThreads.ContainsKey(serverId))
          return;
        Thread thread = new Thread(new ParameterizedThreadStart(ConnectionManager.runPingThread));
        thread.IsBackground = true;
        thread.Priority = ThreadPriority.Highest;
        thread.Start((object) serverId);
        ConnectionManager.pingThreads.Add(serverId, thread);
        Tracing.Log(ConnectionManager.sw, nameof (ConnectionManager), TraceLevel.Verbose, "Connection maintenance thread started for remote host " + (object) serverId);
      }
    }

    private static void runPingThread(object serverIdObj)
    {
      ServerIdentity key = (ServerIdentity) serverIdObj;
      try
      {
        string str1 = Environment.MachineName + "\\" + Environment.UserName + "\\" + Process.GetCurrentProcess().ProcessName;
        ILoginManager loginManager = (ILoginManager) Activator.GetObject(typeof (ILoginManager), new Uri(key.Uri, "LoginManager.rem").AbsoluteUri);
        while (true)
        {
          Thread.Sleep(ConnectionManager.pingInterval);
          using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("Client.Ping", "Client Ping Server", true, 70, nameof (runPingThread), "D:\\ws\\24.3.0.0\\EmLite\\Client\\ConnectionManager.cs"))
          {
            try
            {
              string str2 = str1 + "\\" + DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt");
              loginManager.Echo(ConnectionManager.ConnectionID, (object) str2);
              Tracing.Log(ConnectionManager.sw, nameof (ConnectionManager), TraceLevel.Verbose, "Echo returned successfully with request '" + str2 + "' from remote host " + (object) key);
            }
            catch (Exception ex)
            {
              performanceMeter.Abort();
              string msg = "Connection maintenance thread for remote host " + (object) key + " receieved exception: " + (object) ex + Environment.NewLine + Environment.NewLine + "Continuing...";
              Tracing.Log(ConnectionManager.sw, nameof (ConnectionManager), TraceLevel.Warning, msg);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(ConnectionManager.sw, nameof (ConnectionManager), TraceLevel.Warning, "Connection maintenance thread terminated for remote host " + (object) key + " due to error: " + (object) ex);
      }
      lock (ConnectionManager.pingThreads)
        ConnectionManager.pingThreads.Remove(key);
    }
  }
}
