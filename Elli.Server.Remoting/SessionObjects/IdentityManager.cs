// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.IdentityManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.IdentityServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class IdentityManager : SessionBoundObject, IIdentityManager
  {
    private const string ClassName = "IdentityManager";
    private const int MaxExpiration = 14400;
    private UserInfo userInfo;

    public IdentityManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (IdentityManager).ToLower());
      this.userInfo = session.GetUserInfo();
      return this;
    }

    public virtual string GetSsoToken(
      IEnumerable<string> requestedServices,
      int expirationInMinutes)
    {
      this.onApiCalled(nameof (IdentityManager), "GenerateSsoToken", Array.Empty<object>());
      if (expirationInMinutes > 14400)
        Err.Raise(nameof (IdentityManager), (ServerException) new ServerArgumentException("Invalid expiration. Expiration is too long."));
      if (ServerGlobals.RuntimeEnvironment != RuntimeEnvironment.Hosted)
        Err.Raise(nameof (IdentityManager), new ServerException("This feature is only supported in the hosted environment."));
      IssueTokenRequest ssoTokenRequest = this.CreateSsoTokenRequest(requestedServices, expirationInMinutes);
      SecurityTokenServiceClient tokenServiceClient = new SecurityTokenServiceClient();
      try
      {
        return tokenServiceClient.IssueSsoToken(ssoTokenRequest).Token;
      }
      catch (Exception ex)
      {
        Tracing.Log(true, "Error", nameof (IdentityManager), "Failed to retrieve the security token. " + ex.Message);
        Err.Raise(nameof (IdentityManager), new ServerException("Failed to retrieve the security token. " + ex.Message));
      }
      finally
      {
        if (tokenServiceClient.State != CommunicationState.Faulted)
          tokenServiceClient.Close();
        else
          tokenServiceClient.Abort();
      }
      return (string) null;
    }

    private IssueTokenRequest CreateSsoTokenRequest(
      IEnumerable<string> requestedServices,
      int expirationInMinutes)
    {
      IssueTokenRequest ssoTokenRequest = new IssueTokenRequest();
      ClientContext current = ClientContext.GetCurrent();
      ssoTokenRequest.UserId = this.userInfo.Userid;
      ssoTokenRequest.ExpirationInMinutes = new int?(expirationInMinutes);
      ssoTokenRequest.InstanceId = current.InstanceName;
      ssoTokenRequest.ClientId = current.ClientID;
      ssoTokenRequest.TokenType = EllieMae.EMLite.Server.IdentityServices.TokenType.Jwt;
      ssoTokenRequest.RequestedServices = requestedServices.ToArray<string>();
      ssoTokenRequest.Claims = new Dictionary<string, string>()
      {
        {
          "given_name",
          this.userInfo.FirstName
        },
        {
          "family_name",
          this.userInfo.LastName
        },
        {
          "email",
          this.userInfo.Email
        }
      };
      return ssoTokenRequest;
    }
  }
}
