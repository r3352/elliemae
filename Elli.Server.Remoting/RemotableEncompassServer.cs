// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.RemotableEncompassServer
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.Installer;
using Elli.Server.Remoting.SessionObjects;
using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.Tasks;
using EllieMae.EMLite.VersionInterface15;
using Encompass.Diagnostics;
using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

#nullable disable
namespace Elli.Server.Remoting
{
  public class RemotableEncompassServer
  {
    protected const string className = "RemotableEncompassServer";
    protected const int maxCriticalErrCount = 100;

    internal static event ServerEventHandler ServerEvent;

    protected RemotableEncompassServer()
    {
    }

    static RemotableEncompassServer()
    {
      try
      {
        EnCertificatePolicy.SetDefaultPolicy();
      }
      catch (Exception ex)
      {
        ServerGlobals.TraceLog.WriteError(nameof (RemotableEncompassServer), "Error in static initializer: " + (object) ex);
        throw;
      }
    }

    public static void Start(EncompassServerMode mode)
    {
      lock (typeof (EncompassServer))
      {
        if (!EncompassServer.IsRunning)
        {
          VersionInformation.AutoReloadVersionInformation();
          if (mode != EncompassServerMode.TCP && mode != EncompassServerMode.HTTP)
            throw new ArgumentException("Invalid server mode specified");
          RemotableEncompassServer.createRemotingServiceObjects();
          EncompassServer.ServerMode = mode;
          ConnectionManager.Start();
          System.Runtime.Remoting.RemotingServices.Marshal((MarshalByRefObject) LoginManager.RemotingInstance, "LoginManager.rem", typeof (ILoginManager));
          System.Runtime.Remoting.RemotingServices.Marshal((MarshalByRefObject) VersionControl.RemotingInstance, "VersionControl.rem", typeof (IVersionControl));
          if (EnConfigurationSettings.GlobalSettings.ServerDiagnosticsInterfaceEnabled)
            System.Runtime.Remoting.RemotingServices.Marshal((MarshalByRefObject) DiagnosticsInterface.RemotingInstance, "DiagnosticsInterface.rem", typeof (IDiagnosticsInterface));
          ServerStatusMonitor.Start();
          ServerInstanceConfigMonitor.Start();
          if (!ServerGlobals.TaskSchedulerDisabled)
          {
            TaskQueue.Start();
            TaskScheduler.Start();
          }
          ServerUpdateInstaller.StartServerHotUpdateTimer();
          EncompassServer.IsRunning = true;
          EncompassServer.StartTime = DateTime.Now;
          ServerGlobals.TraceLog.WriteInfo(nameof (RemotableEncompassServer), "Server started in remote mode");
        }
        else
        {
          if (!EncompassServer.IsRunningInProcess)
            return;
          Err.Raise(TraceLevel.Error, nameof (RemotableEncompassServer), new EllieMae.EMLite.ClientServer.Exceptions.ServerException("Server already running in process"));
        }
      }
    }

    public static void StartInProcess(bool startEncServerTasks = false)
    {
      lock (typeof (EncompassServer))
      {
        if (!EncompassServer.IsRunning)
        {
          RemotableEncompassServer.createRemotingServiceObjects();
          EncompassServer.ServerMode = EncompassServerMode.Offline;
          EncompassServer.IsRunning = true;
          EncompassServer.StartTime = DateTime.Now;
          ServerStatusMonitor.Start();
          if (!ServerGlobals.TaskSchedulerDisabled & startEncServerTasks)
          {
            TaskQueue.Start();
            TaskScheduler.Start();
          }
          ServerGlobals.TraceLog.WriteInfo(nameof (RemotableEncompassServer), "Server started in in-process mode");
        }
        else
        {
          if (EncompassServer.IsRunningInProcess)
            return;
          Err.Raise(TraceLevel.Error, nameof (RemotableEncompassServer), new EllieMae.EMLite.ClientServer.Exceptions.ServerException("Server already running out of process"));
        }
      }
    }

    public static void SendTestMessage(int id, bool includeRemoteSessions)
    {
      TestMessage testMessage = new TestMessage(id);
      foreach (ClientContext clientContext in ClientContext.GetAll())
        clientContext.Sessions.BroadcastMessage((Message) testMessage, includeRemoteSessions);
    }

    public static void Stop(DisconnectEventArgument disconnectEventArg)
    {
      lock (typeof (EncompassServer))
      {
        if (!EncompassServer.IsRunning)
          return;
        if (EncompassServer.ServerMode != EncompassServerMode.Service)
        {
          LoginManager.RemotingInstance.Disconnect();
          foreach (ClientContext clientContext in ClientContext.GetAll())
          {
            using (clientContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
              clientContext.Sessions.TerminateAllSessions(disconnectEventArg, false);
          }
          ManagementSession.Sessions.TerminateAllSessions(DisconnectEventArgument.Force, false);
          foreach (RemotedObject remotedObject in (IEnumerable) EncompassServer.StaticObjects.Values)
            remotedObject.Disconnect();
        }
        EncompassServer.IsRunning = false;
        ServerGlobals.TraceLog.WriteInfo(nameof (RemotableEncompassServer), "Server stopped");
        DiagUtility.LoggerScopeProvider.Dispose();
      }
    }

    public static int GetActiveSessionCount()
    {
      int activeSessionCount = 0;
      foreach (ClientContext clientContext in ClientContext.GetAll())
        activeSessionCount += clientContext.Sessions.SessionCount;
      return activeSessionCount;
    }

    public static object GetService(string serviceName)
    {
      if (EncompassServer.ServerMode != EncompassServerMode.Service)
        throw new Exception("Server has not been started in Service mode");
      return EncompassServer.GetStaticObject(serviceName);
    }

    public static System.Version GetServerFileVersion()
    {
      object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyFileVersionAttribute), false);
      if (customAttributes.Length == 0)
        customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyVersionAttribute), false);
      if (customAttributes.Length == 0)
        throw new InvalidOperationException("The AssemblyFileVersion and AssemblyVersion attributes could not be found");
      return customAttributes[0] is AssemblyFileVersionAttribute ? new System.Version(((AssemblyFileVersionAttribute) customAttributes[0]).Version) : new System.Version(((AssemblyVersionAttribute) customAttributes[0]).Version);
    }

    public static JedVersion GetServerCompatibilityVersion()
    {
      System.Version serverFileVersion = RemotableEncompassServer.GetServerFileVersion();
      return new JedVersion(serverFileVersion.Major, serverFileVersion.Minor, serverFileVersion.Build);
    }

    public static void RaiseEvent(EllieMae.EMLite.ClientServer.Events.ServerEvent e)
    {
      RemotableEncompassServer.RaiseEvent(ClientContext.GetCurrent(false), e);
    }

    public static void RaiseEvent(IClientContext context, EllieMae.EMLite.ClientServer.Events.ServerEvent e)
    {
      if (RemotableEncompassServer.ServerEvent != null)
        ThreadPool.QueueUserWorkItem(new WaitCallback(RemotableEncompassServer.raiseEventInternal), (object) new object[2]
        {
          (object) context,
          (object) e
        });
      (context == null ? ServerGlobals.TraceLog : context.TraceLog).WriteVerbose(nameof (RemotableEncompassServer), "Raised event (" + e.GetType().Name + "): " + e.ToString());
    }

    private static void raiseEventInternal(object o)
    {
      object[] objArray = (object[]) o;
      RemotableEncompassServer.ServerEvent((ClientContext) objArray[0], (EllieMae.EMLite.ClientServer.Events.ServerEvent) objArray[1]);
    }

    private static void createRemotingServiceObjects()
    {
      EncompassServer.StaticObjects.Clear();
      EncompassServer.StaticObjects[(object) "loginmanager"] = (object) LoginManager.RemotingInstance;
      EncompassServer.StaticObjects[(object) "versioncontrol"] = (object) VersionControl.RemotingInstance;
      EncompassServer.StaticObjects[(object) "diagnosticsinterface"] = (object) DiagnosticsInterface.RemotingInstance;
    }
  }
}
