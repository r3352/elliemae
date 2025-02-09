// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.BusinessRuleBase.BusinessRuleRestApiHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using RestApiProxy;
using System.Net.Http;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Setup.BusinessRuleBase
{
  public static class BusinessRuleRestApiHelper
  {
    public const string BasePath = "/encompass-cg";
    private const string ELLI_SESSION_KEY_NAME = "elli-session";

    public static string SessionId
    {
      get
      {
        return (Session.DefaultInstance.ServerIdentity != null ? Session.DefaultInstance.ServerIdentity.InstanceName : "LOCALHOST") + "_" + Session.DefaultInstance.SessionID;
      }
    }

    public static string ExportRule(string ruleUrl)
    {
      return RestApiProxyFactory.CreateOAPIGatewayRestApiProxy(BusinessRuleRestApiHelper.SessionId, "application/json", "elli-session").GetStringAsync("/encompass-cg" + ruleUrl).Result;
    }

    public static string ValidateBusinessRule(string ruleXml, string url)
    {
      string requestUri = "/encompass-cg" + url + "?view=xrefentity&format=xml";
      HttpClient gatewayRestApiProxy = RestApiProxyFactory.CreateOAPIGatewayRestApiProxy(BusinessRuleRestApiHelper.SessionId, "application/json", "elli-session");
      StringContent content = new StringContent(Utils.GetJavaScriptSerializer().Serialize((object) new BusinessRuleObject()
      {
        BusinessRuleXml = ruleXml
      }), Encoding.UTF8, "application/json");
      return gatewayRestApiProxy.PostAsync(requestUri, (HttpContent) content).Result.Content.ReadAsStringAsync().Result;
    }

    public static void ImportBusinessRule(string ruleXml, string url)
    {
      string requestUri = "/encompass-cg" + url;
      HttpClient gatewayRestApiProxy = RestApiProxyFactory.CreateOAPIGatewayRestApiProxy(BusinessRuleRestApiHelper.SessionId, "application/json", "elli-session");
      StringContent content = new StringContent(Utils.GetJavaScriptSerializer().Serialize((object) new BusinessRuleObject()
      {
        BusinessRuleXml = ruleXml
      }), Encoding.UTF8, "application/json");
      HttpResponseMessage result = gatewayRestApiProxy.PostAsync(requestUri, (HttpContent) content).Result;
    }
  }
}
