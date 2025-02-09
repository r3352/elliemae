// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.WebServices.EncompassService
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Net;
using System.Web.Services;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.EMLite.Server.WebServices
{
  public abstract class EncompassService : WebService
  {
    private const string className = "EncompassService�";
    public ServiceCredentials Credentials;
    private SessionInfo sessionInfo;

    protected UserInfo GetUserInfo()
    {
      EllieMae.EMLite.Server.User latestVersion = UserStore.GetLatestVersion(this.Credentials.UserID);
      if (!latestVersion.Exists)
        throw new SoapException("User '" + this.Credentials.UserID + "' does not exist.", SoapException.ClientFaultCode);
      if (latestVersion.UserInfo.Status == UserInfo.UserStatusEnum.Disabled)
        throw new SoapException("User '" + this.Credentials.UserID + "' is disabled.", SoapException.ClientFaultCode);
      return latestVersion.UserInfo;
    }

    protected SessionInfo GetSessionInfo()
    {
      if (this.sessionInfo != null)
        return this.sessionInfo;
      if (this.Credentials == null)
        throw new Exception("The current method is not secure and does not have valid credentials.");
      this.sessionInfo = new SessionInfo(ServerIdentity.Offline, "WebMethod", SessionType.Service, this.Credentials.UserID, "", (IPAddress) null, "", DateTime.Now, "");
      return this.sessionInfo;
    }

    protected IDisposable SetCurrentContext()
    {
      return (IDisposable) ((this.Credentials != null ? ClientContext.Get(this.Credentials.Instance) : throw new Exception("The current method is not secure and does not have valid credentials.")) ?? throw new Exception("Invalid instance name specified '" + this.Credentials.Instance + "'.")).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?());
    }

    protected string formatMsg(string msg)
    {
      return this.Credentials != null ? "[" + this.GetSessionInfo().UserID + " / WebMethod] " + msg : "[WebMethod] " + msg;
    }

    protected void onWebMethodCalled(string className, string apiName, params object[] parms)
    {
    }
  }
}
