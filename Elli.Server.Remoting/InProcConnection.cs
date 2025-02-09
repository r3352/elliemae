// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.InProcConnection
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Server;
using System.Net;

#nullable disable
namespace Elli.Server.Remoting
{
  public class InProcConnection : ConnectionBase
  {
    private const string className = "InProcConnection";
    private static string sw = Tracing.SwRemoting;

    ~InProcConnection() => this.Dispose(false);

    public void OpenInProcess(string userId, string password, string appName)
    {
      this.OpenInProcess(userId, password, appName, true);
    }

    public void OpenInProcess(
      string userId,
      string password,
      string appName,
      bool licenseRequired)
    {
      this.OpenInProcess(userId, password, EnConfigurationSettings.InstanceName, appName, licenseRequired);
    }

    public void OpenInProcess(
      string userId,
      string password,
      string instanceName,
      string appName,
      bool licenseRequired)
    {
      this.ensureDisconnected();
      LoginParameters loginParams = new LoginParameters(userId, password, new ServerIdentity(instanceName), (string) null, appName, licenseRequired, true);
      if (userId == "(trusted)")
        throw new LoginException(loginParams, (IPAddress) null, LoginReturnCode.USERID_NOT_FOUND);
      RemotableEncompassServer.StartInProcess(appName.ToLower() == "encompass");
      this.session = (IClientSession) ((ILoginManager) EncompassServer.GetStaticObject("LoginManager")).Login(loginParams, (IServerCallback) this);
    }

    public void OpenTrusted()
    {
      this.ensureDisconnected();
      RemotableEncompassServer.StartInProcess();
      this.session = (IClientSession) ((ILoginManager) EncompassServer.GetStaticObject("LoginManager")).Login(new LoginParameters("(trusted)", "", new ServerIdentity(EnConfigurationSettings.InstanceName), (string) null, "", false), (IServerCallback) this);
    }

    public void OpenRemoteManagementInterface(string password)
    {
      this.ensureDisconnected();
      this.session = (IClientSession) ((ILoginManager) EncompassServer.GetStaticObject("LoginManager")).Manage(new LoginParameters("(rmi)", password, new ServerIdentity(EnConfigurationSettings.InstanceName), (string) null, "", false), (IServerCallback) this);
    }

    public IManagementSession ManagementSession
    {
      get
      {
        this.ensureConnected();
        return this.session as IManagementSession;
      }
    }

    public void OpenTitleFeesAccessorInterface(
      string instanceName,
      string orderUID,
      string loanGUID,
      string credentials)
    {
      this.ensureDisconnected();
      this.session = (IClientSession) ((ILoginManager) EncompassServer.GetStaticObject("LoginManager")).GetTitleFeesAccessor(instanceName, orderUID, loanGUID, credentials, (IServerCallback) this);
    }

    public void OpenData4CloAccessorInterface(
      string instanceName,
      string orderUID,
      string loanGUID,
      string credentials)
    {
      this.ensureDisconnected();
      this.session = (IClientSession) ((ILoginManager) EncompassServer.GetStaticObject("LoginManager")).GetData4CloAccessor(instanceName, orderUID, loanGUID, credentials, (IServerCallback) this);
    }

    public ITitleFeesAccessor TitleFeesAccessor
    {
      get
      {
        this.ensureConnected();
        return this.session as ITitleFeesAccessor;
      }
    }

    public IData4CloAccessor Data4CloAccessor
    {
      get
      {
        this.ensureConnected();
        return this.session as IData4CloAccessor;
      }
    }

    public override bool IsServerInProcess
    {
      get
      {
        try
        {
          this.ensureConnected();
        }
        catch
        {
          return false;
        }
        return true;
      }
    }
  }
}
