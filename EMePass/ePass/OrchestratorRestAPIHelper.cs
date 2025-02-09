// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.OrchestratorRestAPIHelper
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.RemotingServices;
using RestApiProxy;
using System.Net.Http;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.ePass
{
  public class OrchestratorRestAPIHelper
  {
    private string _loanGUID;

    public OrchestratorRestAPIHelper(string loanGUID) => this._loanGUID = loanGUID;

    private string aggregatorAPIURL
    {
      get
      {
        return Session.StartupInfo.OAPIGatewayBaseUri + string.Format("/encservices/v1/loans/{0}/serviceOrder", (object) this._loanGUID);
      }
    }

    public static string SessionId
    {
      get
      {
        return (Session.DefaultInstance.ServerIdentity != null ? Session.DefaultInstance.ServerIdentity.InstanceName : "LOCALHOST") + "_" + Session.DefaultInstance.SessionID;
      }
    }

    public void PostLog(string postData)
    {
      HttpResponseMessage result = RestApiProxyFactory.CreateOAPIGatewayRestApiProxy(OrchestratorRestAPIHelper.SessionId, "application/json").PostAsync(this.aggregatorAPIURL, (HttpContent) new StringContent(postData, Encoding.UTF8, "application/json")).Result;
    }
  }
}
