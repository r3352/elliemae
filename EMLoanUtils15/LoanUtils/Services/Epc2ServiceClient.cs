// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Services.Epc2ServiceClient
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.ClientServer.EPC2;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Authentication;
using EllieMae.EMLite.Common.SimpleCache;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Services
{
  public class Epc2ServiceClient
  {
    private static readonly string sw = Tracing.SwReportControl;
    private const string className = "Epc2ServiceClient�";
    private const string getProductList = "/encservices/v1/products?categories={0}&applications={1}�";
    private const string GetProductById = "/partner/v2/products/{0}?view=options�";
    private const string getServiceSetupEndpoint = "/encservices/v1/serviceSetups?category={0}&providerIds={1}�";
    private const string patchServiceSetupEndpoint = "/encservices/v1/serviceSetups/{0}�";
    private const string postServiceSetupEndpoint = "/encservices/v1/serviceSetups�";
    private const string getServiceCredentialEndpoint = "/encservices/v1/serviceCredentials/?category={0}&providerIds={1}�";
    private const string postServiceCredentialEndpoint = "/encservices/v1/serviceCredentials�";
    private const string postUserCredentialEndpoint = "/encservices/v1/serviceCredentials/{0}/userCredentials�";
    private const string putUserCredentialEndpoint = "/encservices/v1/serviceCredentials/{0}/userCredentials/{1}�";

    public static async Task<List<Epc2Provider>> GetProviderList(
      SessionObjects session,
      string accessToken,
      string[] categories,
      string applications = "Encompass Smart Client�")
    {
      List<Epc2Provider> providerList1 = new List<Epc2Provider>();
      if (Epc2ServiceClient.IsShipDark(session))
        return providerList1;
      string oapiGatewayBaseUri = session.StartupInfo.OAPIGatewayBaseUri;
      string str1 = "";
      string str2 = string.Empty;
      if (categories != null && categories.Length != 0)
      {
        foreach (string category in categories)
        {
          if (str1 != "")
            str1 += ",";
          str1 += category;
        }
      }
      string requestUriString = oapiGatewayBaseUri + string.Format("/encservices/v1/products?categories={0}&applications={1}", (object) str1, (object) HttpUtility.UrlEncode(applications));
      Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Verbose, nameof (Epc2ServiceClient), "GetProviderList API call " + requestUriString);
      List<Epc2Provider> providerList2;
      try
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
        httpWebRequest.KeepAlive = false;
        httpWebRequest.Method = "GET";
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Timeout = 30000;
        httpWebRequest.Headers["Authorization"] = accessToken;
        using (WebResponse responseAsync = await httpWebRequest.GetResponseAsync())
        {
          using (Stream responseStream = responseAsync.GetResponseStream())
          {
            using (StreamReader streamReader = new StreamReader(responseStream))
              str2 = streamReader.ReadToEnd();
          }
        }
        providerList2 = JsonConvert.DeserializeObject<List<Epc2Provider>>(str2);
      }
      catch (WebException ex)
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Error, nameof (Epc2ServiceClient), string.Format("GetProviderList API WebException (Message {0})", (object) ex.Message));
        throw ex;
      }
      catch (Exception ex)
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Error, nameof (Epc2ServiceClient), string.Format("GetProviderList API Exception (Message {0})", (object) ex.Message));
        throw ex;
      }
      return providerList2;
    }

    public static async Task<Epc2Provider> GetProviderById(
      SessionObjects session,
      string accessToken,
      string providerId)
    {
      Epc2Provider providerById1 = new Epc2Provider();
      if (Epc2ServiceClient.IsShipDark(session))
        return providerById1;
      string oapiGatewayBaseUri = session.StartupInfo.OAPIGatewayBaseUri;
      string str = string.Empty;
      string requestUriString = oapiGatewayBaseUri + string.Format("/partner/v2/products/{0}?view=options", (object) providerId);
      Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Verbose, nameof (Epc2ServiceClient), "GetProviderById API call " + requestUriString);
      Epc2Provider providerById2;
      try
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
        httpWebRequest.KeepAlive = false;
        httpWebRequest.Method = "GET";
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Timeout = 30000;
        httpWebRequest.Headers["Authorization"] = accessToken;
        using (WebResponse responseAsync = await httpWebRequest.GetResponseAsync())
        {
          using (Stream responseStream = responseAsync.GetResponseStream())
          {
            using (StreamReader streamReader = new StreamReader(responseStream))
              str = streamReader.ReadToEnd();
          }
        }
        providerById2 = JsonConvert.DeserializeObject<Epc2Provider>(str);
      }
      catch (WebException ex)
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Error, nameof (Epc2ServiceClient), string.Format("GetProviderById API WebException (Message {0})", (object) ex.Message));
        throw ex;
      }
      catch (Exception ex)
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Error, nameof (Epc2ServiceClient), string.Format("GetProviderById API Exception (Message {0})", (object) ex.Message));
        throw ex;
      }
      return providerById2;
    }

    public static async Task<ServiceSetupEvaluatorResponse> GetServiceSetupEvaluatorResponse(
      SessionObjects session,
      string accessToken,
      string loanId,
      string providerId = "�")
    {
      ServiceSetupEvaluatorResponse evaluatorResponse1 = new ServiceSetupEvaluatorResponse();
      if (Epc2ServiceClient.IsShipDark(session))
        return evaluatorResponse1;
      string oapiGatewayBaseUri = session.StartupInfo.OAPIGatewayBaseUri;
      ServiceSetupEvaluatorRequest evaluatorRequest = new ServiceSetupEvaluatorRequest()
      {
        LoanId = loanId.Replace("{", "").Replace("}", ""),
        Categories = new List<string>() { "PRODUCTPRICING" },
        OrderTypes = new List<OrderTypeEnum>()
        {
          OrderTypeEnum.MANUAL
        }
      };
      if (!string.IsNullOrWhiteSpace(providerId))
        evaluatorRequest.ProviderId = providerId;
      string str1 = "";
      string msg1 = "Calling GetServiceSetupEvaluatorResponse method";
      ServiceSetupEvaluatorResponse evaluatorResponse2;
      try
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Verbose, nameof (Epc2ServiceClient), msg1);
        List<string> categories = evaluatorRequest.Categories;
        string requestUriString = oapiGatewayBaseUri + "/encservices/v1/evaluators/serviceSetups";
        string str2 = JsonConvert.SerializeObject((object) evaluatorRequest, new JsonSerializerSettings()
        {
          ContractResolver = (IContractResolver) new CamelCasePropertyNamesContractResolver()
        });
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
        httpWebRequest.KeepAlive = false;
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Timeout = 30000;
        httpWebRequest.Headers["Authorization"] = accessToken;
        using (Stream requestStream = httpWebRequest.GetRequestStream())
        {
          using (StreamWriter streamWriter = new StreamWriter(requestStream))
            streamWriter.Write(str2);
        }
        using (WebResponse responseAsync = await httpWebRequest.GetResponseAsync())
        {
          using (Stream responseStream = responseAsync.GetResponseStream())
          {
            using (StreamReader streamReader = new StreamReader(responseStream))
              str1 = streamReader.ReadToEnd();
          }
        }
        evaluatorResponse2 = JsonConvert.DeserializeObject<ServiceSetupEvaluatorResponse>(str1);
      }
      catch (Exception ex)
      {
        string msg2 = "Error in Epc2 service call to Get Service Setup Evaluator. Ex: " + (object) ex;
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Error, nameof (Epc2ServiceClient), msg2);
        throw ex;
      }
      return evaluatorResponse2;
    }

    private static bool IsShipDark(SessionObjects session)
    {
      string companySetting = session.ConfigurationManager.GetCompanySetting("POLICIES", "EPPS_EPC2_SHIP_DARK");
      return string.IsNullOrEmpty(companySetting) || string.Equals(companySetting, "true", StringComparison.CurrentCultureIgnoreCase);
    }

    public string ComposeEPassPayloadAndUrlForEPC2(
      string source,
      ProductPricingSetting setting,
      ServiceSetupEvaluatorResponse svcSetupResp)
    {
      if (svcSetupResp != null)
      {
        int? count = svcSetupResp.MatchingResults?.Count;
        int num = 0;
        if (count.GetValueOrDefault() > num & count.HasValue)
        {
          List<MatchingResult> matchingResults = svcSetupResp.MatchingResults;
          ServiceSetupResult svcSetupResult = matchingResults.Select<MatchingResult, ServiceSetupResult>((Func<MatchingResult, ServiceSetupResult>) (b => b.ServiceSetup)).Where<ServiceSetupResult>((Func<ServiceSetupResult, bool>) (d => d.ProviderId.Equals(setting.ProviderID, StringComparison.CurrentCultureIgnoreCase))).FirstOrDefault<ServiceSetupResult>();
          if (svcSetupResult != null)
          {
            MatchingResult matchingResult = matchingResults.Where<MatchingResult>((Func<MatchingResult, bool>) (b => b.ServiceSetupId.Equals(svcSetupResult.Id, StringComparison.CurrentCultureIgnoreCase))).FirstOrDefault<MatchingResult>();
            Epc2HostAdapter epc2HostAdapter = new Epc2HostAdapter();
            epc2HostAdapter.ServiceSetupId = matchingResult.ServiceSetupId;
            epc2HostAdapter.Scopes = matchingResult.Scopes;
            epc2HostAdapter.ServiceSetup = JsonConvert.DeserializeObject<ServiceSetupResult>(JsonConvert.SerializeObject((object) matchingResult.ServiceSetup));
            ProductInfo productInfo = new ProductInfo()
            {
              ProductName = svcSetupResult.ProductName,
              ListingName = setting.PartnerName,
              VendorPlatform = svcSetupResult.VendorPlatform
            };
            epc2HostAdapter.ServiceSetup.ProductInfo = productInfo;
            epc2HostAdapter.Source = source;
            string path = Path.Combine(Path.GetTempPath(), "ePass.PPE.Order.json");
            string contents = JsonConvert.SerializeObject((object) epc2HostAdapter, new JsonSerializerSettings()
            {
              ContractResolver = (IContractResolver) new CamelCasePropertyNamesContractResolver()
            });
            System.IO.File.WriteAllText(path, contents);
            string str = string.Format("https://_EPASS_SIGNATURE;ASSEMBLYRESOLVER;SSFBOOTSTRAP2;;FILE;{0}", (object) path);
            Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Info, nameof (Epc2ServiceClient), "Epc2 ePass URL: " + str);
            return str;
          }
        }
      }
      Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Error, nameof (Epc2ServiceClient), "Failed to Create url for EPC2 ePass. Listing Name-\"" + setting.PartnerName + "\", ServiceSetupEvaluatorResponse-\"" + JsonConvert.SerializeObject((object) svcSetupResp) + "\"");
      return (string) null;
    }

    public static async Task<List<ServiceSetupResult>> GetServiceSetupAsync(
      SessionObjects session,
      string accessToken,
      string category,
      string providerId)
    {
      List<ServiceSetupResult> serviceSetupResultList = new List<ServiceSetupResult>();
      string oapiGatewayBaseUri = session.StartupInfo.OAPIGatewayBaseUri;
      string str = string.Empty;
      string requestUriString = oapiGatewayBaseUri + string.Format("/encservices/v1/serviceSetups?category={0}&providerIds={1}", (object) category, (object) providerId);
      Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Verbose, nameof (Epc2ServiceClient), "GetServiceSetupAsync API call " + requestUriString);
      List<ServiceSetupResult> serviceSetupAsync;
      try
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
        httpWebRequest.KeepAlive = false;
        httpWebRequest.Method = "GET";
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Timeout = 30000;
        httpWebRequest.Headers["Authorization"] = accessToken;
        using (WebResponse responseAsync = await httpWebRequest.GetResponseAsync())
        {
          using (Stream responseStream = responseAsync.GetResponseStream())
          {
            using (StreamReader streamReader = new StreamReader(responseStream))
              str = streamReader.ReadToEnd();
          }
        }
        serviceSetupAsync = JsonConvert.DeserializeObject<List<ServiceSetupResult>>(str);
      }
      catch (WebException ex)
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Error, nameof (Epc2ServiceClient), string.Format("GetServiceSetupAsync API WebException (Message {0})", (object) ex.Message));
        throw ex;
      }
      catch (Exception ex)
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Error, nameof (Epc2ServiceClient), string.Format("GetServiceSetupAsync API Exception (Message {0})", (object) ex.Message));
        throw ex;
      }
      return serviceSetupAsync;
    }

    public static async Task<string> UpdateServiceSetupAsync(
      SessionObjects session,
      string accessToken,
      ServiceSetupResult serviceSetup)
    {
      string oapiGatewayBaseUri = session.StartupInfo.OAPIGatewayBaseUri;
      string str1 = "";
      string msg = "Calling UpdateServiceSetupAsync method";
      try
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Verbose, nameof (Epc2ServiceClient), msg);
        string requestUriString = oapiGatewayBaseUri + string.Format("/encservices/v1/serviceSetups/{0}", (object) serviceSetup.Id);
        string str2 = JsonConvert.SerializeObject((object) serviceSetup, new JsonSerializerSettings()
        {
          ContractResolver = (IContractResolver) new CamelCasePropertyNamesContractResolver()
        });
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
        httpWebRequest.KeepAlive = false;
        httpWebRequest.Method = "PATCH";
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Timeout = 30000;
        httpWebRequest.Headers["Authorization"] = accessToken;
        using (Stream requestStream = httpWebRequest.GetRequestStream())
        {
          using (StreamWriter streamWriter = new StreamWriter(requestStream))
            streamWriter.Write(str2);
        }
        using (WebResponse responseAsync = await httpWebRequest.GetResponseAsync())
        {
          using (Stream responseStream = responseAsync.GetResponseStream())
          {
            using (StreamReader streamReader = new StreamReader(responseStream))
              str1 = streamReader.ReadToEnd();
          }
        }
      }
      catch (WebException ex)
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Error, nameof (Epc2ServiceClient), string.Format("UpdateServiceSetupAsync API WebException (Message {0})", (object) ex.Message));
        throw ex;
      }
      catch (Exception ex)
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Error, nameof (Epc2ServiceClient), string.Format("UpdateServiceSetupAsync API Exception (Message {0})", (object) ex.Message));
        throw ex;
      }
      return str1;
    }

    public static async Task<ServiceSetupResult> CreateServiceSetupAsync(
      SessionObjects session,
      string accessToken,
      ServiceSetupResult serviceSetup)
    {
      ServiceSetupResult contracts = new ServiceSetupResult();
      string oapiGatewayBaseUri = session.StartupInfo.OAPIGatewayBaseUri;
      string str1 = "";
      string msg = "Calling CreateServiceSetupAsync method";
      try
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Verbose, nameof (Epc2ServiceClient), msg);
        string requestUriString = oapiGatewayBaseUri + "/encservices/v1/serviceSetups";
        string str2 = JsonConvert.SerializeObject((object) serviceSetup, new JsonSerializerSettings()
        {
          ContractResolver = (IContractResolver) new CamelCasePropertyNamesContractResolver()
        });
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
        httpWebRequest.KeepAlive = false;
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Timeout = 30000;
        httpWebRequest.Headers["Authorization"] = accessToken;
        using (Stream requestStream = httpWebRequest.GetRequestStream())
        {
          using (StreamWriter streamWriter = new StreamWriter(requestStream))
            streamWriter.Write(str2);
        }
        using (WebResponse responseAsync = await httpWebRequest.GetResponseAsync())
        {
          using (Stream responseStream = responseAsync.GetResponseStream())
          {
            using (StreamReader streamReader = new StreamReader(responseStream))
              str1 = streamReader.ReadToEnd();
          }
        }
        contracts = JsonConvert.DeserializeObject<ServiceSetupResult>(str1);
      }
      catch (WebException ex)
      {
        Epc2ServiceClient.HandleWebException(ex, "CreateServiceSetupAsync API");
      }
      catch (Exception ex)
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Error, nameof (Epc2ServiceClient), string.Format("CreateServiceSetupAsync API Exception (Message {0})", (object) ex.Message));
        throw ex;
      }
      return contracts;
    }

    private static void HandleWebException(WebException ex, string apiName)
    {
      HttpResponseError httpResponseError = (HttpResponseError) null;
      string end = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
      if (end != null)
        httpResponseError = JsonConvert.DeserializeObject<HttpResponseError>(end);
      if (httpResponseError != null && ex != null)
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Error, nameof (Epc2ServiceClient), string.Format("{0} WebException (Message: {1}; Code: {2}; Summary: {3}; Details: {4} )", (object) apiName, (object) ex.Message, (object) httpResponseError.Code, (object) httpResponseError.Summary, (object) httpResponseError.Details));
      throw ex;
    }

    public static async Task<ServiceCredentialResponse> CreateServiceCredentialAsync(
      SessionObjects session,
      string accessToken,
      ServiceCredentialRequest serviceCredential)
    {
      ServiceCredentialResponse credentialResponse = new ServiceCredentialResponse();
      string oapiGatewayBaseUri = session.StartupInfo.OAPIGatewayBaseUri;
      string str1 = "";
      string msg = "Calling CreateServiceCredentialAsync method";
      ServiceCredentialResponse serviceCredentialAsync;
      try
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Verbose, nameof (Epc2ServiceClient), msg);
        string requestUriString = oapiGatewayBaseUri + "/encservices/v1/serviceCredentials";
        string str2 = JsonConvert.SerializeObject((object) serviceCredential, new JsonSerializerSettings()
        {
          ContractResolver = (IContractResolver) new CamelCasePropertyNamesContractResolver()
        });
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
        httpWebRequest.KeepAlive = false;
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Timeout = 30000;
        httpWebRequest.Headers["Authorization"] = accessToken;
        using (Stream requestStream = httpWebRequest.GetRequestStream())
        {
          using (StreamWriter streamWriter = new StreamWriter(requestStream))
            streamWriter.Write(str2);
        }
        using (WebResponse responseAsync = await httpWebRequest.GetResponseAsync())
        {
          using (Stream responseStream = responseAsync.GetResponseStream())
          {
            using (StreamReader streamReader = new StreamReader(responseStream))
              str1 = streamReader.ReadToEnd();
          }
        }
        serviceCredentialAsync = JsonConvert.DeserializeObject<ServiceCredentialResponse>(str1);
      }
      catch (WebException ex)
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Error, nameof (Epc2ServiceClient), string.Format("CreateServiceCredentialAsync API WebException (Message {0})", (object) ex.Message));
        throw ex;
      }
      catch (Exception ex)
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Error, nameof (Epc2ServiceClient), string.Format("CreateServiceCredentialAsync API Exception (Message {0})", (object) ex.Message));
        throw ex;
      }
      return serviceCredentialAsync;
    }

    public static async Task<UserCredentialResponse> CreateUserCredentialAsync(
      SessionObjects session,
      string accessToken,
      UserCredentialRequest userCredential,
      string serviceCredentialId)
    {
      UserCredentialResponse credentialResponse = new UserCredentialResponse();
      string oapiGatewayBaseUri = session.StartupInfo.OAPIGatewayBaseUri;
      string str1 = "";
      string msg = "Calling CreateUserCredentialAsync method";
      UserCredentialResponse userCredentialAsync;
      try
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Verbose, nameof (Epc2ServiceClient), msg);
        string requestUriString = oapiGatewayBaseUri + string.Format("/encservices/v1/serviceCredentials/{0}/userCredentials", (object) serviceCredentialId);
        string str2 = JsonConvert.SerializeObject((object) userCredential, new JsonSerializerSettings()
        {
          ContractResolver = (IContractResolver) new CamelCasePropertyNamesContractResolver()
        });
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
        httpWebRequest.KeepAlive = false;
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Timeout = 30000;
        httpWebRequest.Headers["Authorization"] = accessToken;
        using (Stream requestStream = httpWebRequest.GetRequestStream())
        {
          using (StreamWriter streamWriter = new StreamWriter(requestStream))
            streamWriter.Write(str2);
        }
        using (WebResponse responseAsync = await httpWebRequest.GetResponseAsync())
        {
          using (Stream responseStream = responseAsync.GetResponseStream())
          {
            using (StreamReader streamReader = new StreamReader(responseStream))
              str1 = streamReader.ReadToEnd();
          }
        }
        userCredentialAsync = JsonConvert.DeserializeObject<UserCredentialResponse>(str1);
      }
      catch (WebException ex)
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Error, nameof (Epc2ServiceClient), string.Format("CreateUserCredentialAsync API WebException (Message {0})", (object) ex.Message));
        throw ex;
      }
      catch (Exception ex)
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Error, nameof (Epc2ServiceClient), string.Format("CreateUserCredentialAsync API Exception (Message {0})", (object) ex.Message));
        throw ex;
      }
      return userCredentialAsync;
    }

    public static async Task<UserCredentialResponse> UpdateUserCredentialAsync(
      SessionObjects session,
      string accessToken,
      UserCredentialRequest userCredential,
      string serviceCredentialId,
      string userCredentialId)
    {
      UserCredentialResponse credentialResponse1 = new UserCredentialResponse();
      string oapiGatewayBaseUri = session.StartupInfo.OAPIGatewayBaseUri;
      string str1 = "";
      string msg = "Calling UpdateUserCredentialAsync method";
      UserCredentialResponse credentialResponse2;
      try
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Verbose, nameof (Epc2ServiceClient), msg);
        string requestUriString = oapiGatewayBaseUri + string.Format("/encservices/v1/serviceCredentials/{0}/userCredentials/{1}", (object) serviceCredentialId, (object) userCredentialId);
        string str2 = JsonConvert.SerializeObject((object) userCredential, new JsonSerializerSettings()
        {
          ContractResolver = (IContractResolver) new CamelCasePropertyNamesContractResolver()
        });
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
        httpWebRequest.KeepAlive = false;
        httpWebRequest.Method = "PUT";
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Timeout = 30000;
        httpWebRequest.Headers["Authorization"] = accessToken;
        using (Stream requestStream = httpWebRequest.GetRequestStream())
        {
          using (StreamWriter streamWriter = new StreamWriter(requestStream))
            streamWriter.Write(str2);
        }
        using (WebResponse responseAsync = await httpWebRequest.GetResponseAsync())
        {
          using (Stream responseStream = responseAsync.GetResponseStream())
          {
            using (StreamReader streamReader = new StreamReader(responseStream))
              str1 = streamReader.ReadToEnd();
          }
        }
        credentialResponse2 = JsonConvert.DeserializeObject<UserCredentialResponse>(str1);
      }
      catch (WebException ex)
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Error, nameof (Epc2ServiceClient), string.Format("UpdateUserCredentialAsync API WebException (Message {0})", (object) ex.Message));
        throw ex;
      }
      catch (Exception ex)
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Error, nameof (Epc2ServiceClient), string.Format("UpdateUserCredentialAsync API Exception (Message {0})", (object) ex.Message));
        throw ex;
      }
      return credentialResponse2;
    }

    public static async Task<List<ServiceCredentialResponse>> GetServiceCredentialAsync(
      SessionObjects session,
      string accessToken,
      string category,
      string providerId)
    {
      List<ServiceCredentialResponse> credentialResponseList = new List<ServiceCredentialResponse>();
      string oapiGatewayBaseUri = session.StartupInfo.OAPIGatewayBaseUri;
      string str = string.Empty;
      string requestUriString = oapiGatewayBaseUri + string.Format("/encservices/v1/serviceCredentials/?category={0}&providerIds={1}", (object) category, (object) providerId);
      Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Verbose, nameof (Epc2ServiceClient), "GetServiceCredentialAsync API call " + requestUriString);
      List<ServiceCredentialResponse> serviceCredentialAsync;
      try
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
        httpWebRequest.KeepAlive = false;
        httpWebRequest.Method = "GET";
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Timeout = 30000;
        httpWebRequest.Headers["Authorization"] = accessToken;
        using (WebResponse responseAsync = await httpWebRequest.GetResponseAsync())
        {
          using (Stream responseStream = responseAsync.GetResponseStream())
          {
            using (StreamReader streamReader = new StreamReader(responseStream))
              str = streamReader.ReadToEnd();
          }
        }
        serviceCredentialAsync = JsonConvert.DeserializeObject<List<ServiceCredentialResponse>>(str);
      }
      catch (WebException ex)
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Error, nameof (Epc2ServiceClient), string.Format("GetServiceCredentialAsync API WebException (Message {0})", (object) ex.Message));
        throw ex;
      }
      catch (Exception ex)
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Error, nameof (Epc2ServiceClient), string.Format("GetServiceSetup API Exception (Message {0})", (object) ex.Message));
        throw ex;
      }
      return serviceCredentialAsync;
    }

    public static string GetEPC2EPassURL(
      SessionObjects session,
      string loanId,
      string providerId,
      string source,
      out string partnerName)
    {
      try
      {
        string accessToken = new OAuth2(session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(session), CacheItemRetentionPolicy.NoRetention).GetAccessToken(session.StartupInfo.ServerInstanceName, session.SessionID, "sc").TypeAndToken;
        ServiceSetupEvaluatorResponse result = Task.Run<ServiceSetupEvaluatorResponse>((Func<Task<ServiceSetupEvaluatorResponse>>) (async () => await Epc2ServiceClient.GetServiceSetupEvaluatorResponse(session, accessToken, loanId, providerId))).Result;
        ProductPricingSetting setting = session.StartupInfo.ProductPricingPartner;
        if (string.Compare(setting.ProviderID, providerId, true) != 0)
          setting = session.ConfigurationManager.GetProductPricingSettings().FirstOrDefault<ProductPricingSetting>((Func<ProductPricingSetting, bool>) (p => string.Compare(p.ProviderID, providerId, true) == 0));
        string epC2EpassUrl = new Epc2ServiceClient().ComposeEPassPayloadAndUrlForEPC2(source, setting, result);
        partnerName = setting?.PartnerName ?? "";
        return epC2EpassUrl;
      }
      catch (Exception ex)
      {
        Tracing.Log(Epc2ServiceClient.sw, TraceLevel.Error, nameof (Epc2ServiceClient), string.Format("Get Epc2 EPassURL Exception {0}", (object) ex));
        throw;
      }
    }
  }
}
