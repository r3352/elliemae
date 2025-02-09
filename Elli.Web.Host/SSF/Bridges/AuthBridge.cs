// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.SSF.Bridges.AuthBridge
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using Elli.Web.Host.SSF.Context;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

#nullable disable
namespace Elli.Web.Host.SSF.Bridges
{
  public class AuthBridge : Bridge
  {
    private const string className = "AuthBridge";
    private static readonly string sw = Tracing.SwThinThick;

    internal AuthBridge(SSFContext context)
      : base(context)
    {
    }

    public string getAccessToken()
    {
      Tracing.Log(AuthBridge.sw, TraceLevel.Verbose, nameof (AuthBridge), "Entering 'getAccessToken'");
      try
      {
        AccessToken accessToken = this.context.getAccessToken(false);
        return JsonConvert.SerializeObject((object) new AuthBridge.AccessTokenModel()
        {
          access_token = accessToken.Token,
          token_type = accessToken.Type,
          host_name = accessToken.HostName
        });
      }
      catch (Exception ex)
      {
        Tracing.Log(AuthBridge.sw, TraceLevel.Error, nameof (AuthBridge), "Error occured 'getAccessToken': " + ex.Message);
        throw;
      }
    }

    public string getUser()
    {
      Tracing.Log(AuthBridge.sw, TraceLevel.Verbose, nameof (AuthBridge), "Entering 'getUser'");
      try
      {
        ISessionStartupInfo startupInfo = Session.StartupInfo;
        UserInfo userInfo = startupInfo.UserInfo;
        return JsonConvert.SerializeObject((object) new AuthBridge.UserModel()
        {
          id = userInfo.Userid,
          realm = string.Format("Encompass\\{0}\\{1}", (object) startupInfo.ServerInstanceName, (object) userInfo.Userid),
          clientId = startupInfo.CompanyInfo.ClientID,
          email = userInfo.Email,
          firstName = userInfo.FirstName,
          lastName = userInfo.LastName,
          isAdmin = new bool?(userInfo.IsAdministrator())
        });
      }
      catch (Exception ex)
      {
        Tracing.Log(AuthBridge.sw, TraceLevel.Error, nameof (AuthBridge), "Error occurred 'getUser': " + ex.Message);
        throw;
      }
    }

    private class AccessTokenModel
    {
      public string access_token { get; set; }

      public string token_type { get; set; }

      public string host_name { get; set; }
    }

    private class AuthCodeModel
    {
      public string authorization_code { get; set; }

      public string issued_token_type { get; set; }

      public string token_type { get; set; }

      public int? expires_in { get; set; }

      public string urlToken { get; set; }
    }

    private class UserModel
    {
      public string id { get; set; }

      public string realm { get; set; }

      public string clientId { get; set; }

      public string email { get; set; }

      public string firstName { get; set; }

      public string lastName { get; set; }

      public bool? isAdmin { get; set; }
    }
  }
}
