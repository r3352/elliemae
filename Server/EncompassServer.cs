// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.EncompassServer
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.VersionInterface15;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class EncompassServer
  {
    protected const string className = "EncompassServer�";
    protected const int maxCriticalErrCount = 100;
    protected static Hashtable staticObjects = CollectionsUtil.CreateCaseInsensitiveHashtable();
    protected static bool isRunning = false;
    protected static DateTime startTime = DateTime.MinValue;
    protected static EncompassServerMode serverMode = EncompassServerMode.None;
    protected static string serverId = Guid.NewGuid().ToString();
    protected static ArrayList systemAlerts = new ArrayList();

    internal static event ServerEventHandler ServerEvent;

    protected EncompassServer()
    {
    }

    static EncompassServer()
    {
      ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
      try
      {
        EnCertificatePolicy.SetDefaultPolicy();
      }
      catch (Exception ex)
      {
        ServerGlobals.TraceLog.WriteError(nameof (EncompassServer), "Error in static initializer: " + (object) ex);
        throw;
      }
    }

    public static void SendTestMessage(int id, bool includeRemoteSessions)
    {
      TestMessage testMessage = new TestMessage(id);
      foreach (ClientContext clientContext in ClientContext.GetAll())
        clientContext.Sessions.BroadcastMessage((Message) testMessage, includeRemoteSessions);
    }

    public static bool IsRunningInProcess
    {
      get => EncompassServer.serverMode == EncompassServerMode.Offline;
    }

    public static bool IsRunning
    {
      get => EncompassServer.isRunning;
      set => EncompassServer.isRunning = value;
    }

    public static EncompassServerMode ServerMode
    {
      get => EncompassServer.serverMode;
      set => EncompassServer.serverMode = value;
    }

    public static string ServerID => EncompassServer.serverId;

    public static DateTime StartTime
    {
      get => EncompassServer.startTime;
      set => EncompassServer.startTime = value;
    }

    public static Hashtable StaticObjects => EncompassServer.staticObjects;

    public static int GetActiveSessionCount()
    {
      int activeSessionCount = 0;
      foreach (ClientContext clientContext in ClientContext.GetAll())
        activeSessionCount += clientContext.Sessions.SessionCount;
      return activeSessionCount;
    }

    public static object GetStaticObject(string objectName)
    {
      return EncompassServer.staticObjects[(object) objectName] ?? (object) null;
    }

    public static object GetService(string serviceName)
    {
      if (EncompassServer.serverMode != EncompassServerMode.Service)
        throw new Exception("Server has not been started in Service mode");
      return EncompassServer.GetStaticObject(serviceName);
    }

    public static SystemAlert[] GetSystemAlerts()
    {
      lock (EncompassServer.systemAlerts)
        return (SystemAlert[]) EncompassServer.systemAlerts.ToArray(typeof (SystemAlert));
    }

    public static void AddSystemAlert(SystemAlert err)
    {
      lock (EncompassServer.systemAlerts)
      {
        EncompassServer.systemAlerts.Insert(0, (object) err);
        if (EncompassServer.systemAlerts.Count <= 100)
          return;
        EncompassServer.systemAlerts.RemoveRange(100, EncompassServer.systemAlerts.Count - 100);
      }
    }

    public static void ClearSystemAlerts()
    {
      lock (EncompassServer.systemAlerts)
        EncompassServer.systemAlerts.Clear();
    }

    public static Version GetServerFileVersion()
    {
      object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyFileVersionAttribute), false);
      if (customAttributes.Length == 0)
        customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyVersionAttribute), false);
      if (customAttributes.Length == 0)
        throw new InvalidOperationException("The AssemblyFileVersion and AssemblyVersion attributes could not be found");
      return customAttributes[0] is AssemblyFileVersionAttribute ? new Version(((AssemblyFileVersionAttribute) customAttributes[0]).Version) : new Version(((AssemblyVersionAttribute) customAttributes[0]).Version);
    }

    public static JedVersion GetServerCompatibilityVersion()
    {
      Version serverFileVersion = EncompassServer.GetServerFileVersion();
      return new JedVersion(serverFileVersion.Major, serverFileVersion.Minor, serverFileVersion.Build);
    }

    public static void RaiseEvent(EllieMae.EMLite.ClientServer.Events.ServerEvent e)
    {
      EncompassServer.RaiseEvent(ClientContext.GetCurrent(false), e);
    }

    public static void RaiseEvent(IClientContext context, EllieMae.EMLite.ClientServer.Events.ServerEvent e)
    {
      if (EncompassServer.ServerEvent != null)
        ThreadPool.QueueUserWorkItem(new WaitCallback(EncompassServer.raiseEventInternal), (object) new object[2]
        {
          (object) context,
          (object) e
        });
      (context == null ? ServerGlobals.TraceLog : context.TraceLog).WriteVerbose(nameof (EncompassServer), "Raised event (" + e.GetType().Name + "): " + e.ToString());
    }

    private static void raiseEventInternal(object o)
    {
      object[] objArray = (object[]) o;
      EncompassServer.ServerEvent((ClientContext) objArray[0], (EllieMae.EMLite.ClientServer.Events.ServerEvent) objArray[1]);
    }

    public static string GetServerDllVersion()
    {
      string fileName = Assembly.GetExecutingAssembly().CodeBase;
      if (fileName.ToLower().StartsWith("file:///"))
        fileName = fileName.Substring(8);
      return FileVersionInfo.GetVersionInfo(fileName).FileVersion;
    }
  }
}
