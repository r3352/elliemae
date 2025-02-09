// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.DDMRestApiHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.RemotingServices;
using RestApiProxy;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public static class DDMRestApiHelper
  {
    public const string DDMRulesBasePath = "/encompass/v1/settings/rules";
    public const string DDMFeeRuleExportFormat = "/encompass/v1/settings/rules/feerules/{0}?format={1}";
    public const string DDMFieldRuleExportFormat = "/encompass/v1/settings/rules/fieldrules/{0}?format={1}";
    public const string DDMFeeRuleScenarioExportFormat = "/encompass/v1/settings/rules/feerules/{0}/Scenarios/{1}?format={2}";
    public const string DDMFieldRuleScenarioExportFormat = "/encompass/v1/settings/rules/fieldrules/{0}/Scenarios/{1}?format={2}";
    public const string DDMFeeRuleValidateFormat = "/encompass/v1/settings/rules/feerules/{0}";
    public const string DDMFieldRuleValidateFormat = "/encompass/v1/settings/rules/fieldrules/{0}";
    public const string DDMFeeRuleImport = "/encompass/v1/settings/rules/feerules/import";
    public const string DDMFieldRuleImport = "/encompass/v1/settings/rules/fieldrules/import";

    public static string SessionId
    {
      get
      {
        return (Session.DefaultInstance.ServerIdentity != null ? Session.DefaultInstance.ServerIdentity.InstanceName : "LOCALHOST") + "_" + Session.DefaultInstance.SessionID;
      }
    }

    public static string ExportFeeRule(int feeRuleId, string format)
    {
      return RestApiProxyFactory.CreateOAPIGatewayRestApiProxy(DDMRestApiHelper.SessionId, "application/json").GetStringAsync(string.Format("/encompass/v1/settings/rules/feerules/{0}?format={1}", (object) feeRuleId, (object) format)).Result;
    }

    public static string ExportFieldRule(int fieldRuleId, string format)
    {
      return RestApiProxyFactory.CreateOAPIGatewayRestApiProxy(DDMRestApiHelper.SessionId, "application/json").GetStringAsync(string.Format("/encompass/v1/settings/rules/fieldrules/{0}?format={1}", (object) fieldRuleId, (object) format)).Result;
    }

    public static HttpResponseMessage ValidateFieldRule(string fieldRuleXml)
    {
      string url = string.Format("/encompass/v1/settings/rules/fieldrules/{0}", (object) "importValidations?format=xml&view=xrefentity");
      return DDMRestApiHelper.ValidateFeeFieldRule(fieldRuleXml, url);
    }

    public static HttpResponseMessage ValidateFeeRule(string feeRuleXml)
    {
      string url = string.Format("/encompass/v1/settings/rules/feerules/{0}", (object) "importValidations?format=xml&view=xrefentity");
      return DDMRestApiHelper.ValidateFeeFieldRule(feeRuleXml, url);
    }

    public static string ImportFeeRule(string feeRuleXml)
    {
      string url = "/encompass/v1/settings/rules/feerules/import";
      return DDMRestApiHelper.ImportFeeFieldRule(feeRuleXml, url);
    }

    public static string ImportFieldRule(string fieldRuleXml)
    {
      string url = "/encompass/v1/settings/rules/fieldrules/import";
      return DDMRestApiHelper.ImportFeeFieldRule(fieldRuleXml, url);
    }

    private static string ImportFeeFieldRule(string ruleXml, string url)
    {
      return RestApiProxyFactory.CreateOAPIGatewayRestApiProxy(DDMRestApiHelper.SessionId, "application/json").PostAsync(url, (HttpContent) new StringContent(new JavaScriptSerializer().Serialize((object) new FeeFieldRuleObject(ruleXml)), Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync().Result;
    }

    private static HttpResponseMessage ValidateFeeFieldRule(string ruleXml, string url)
    {
      return RestApiProxyFactory.CreateOAPIGatewayRestApiProxy(DDMRestApiHelper.SessionId, "application/json").PostAsync(url, (HttpContent) new StringContent(new JavaScriptSerializer().Serialize((object) new FeeFieldRuleObject(ruleXml)), Encoding.UTF8, "application/json")).Result;
    }
  }
}
