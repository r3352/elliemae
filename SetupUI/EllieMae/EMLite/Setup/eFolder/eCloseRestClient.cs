// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.eCloseRestClient
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.eFolder;
using EllieMae.EMLite.RemotingServices;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestApiProxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class eCloseRestClient
  {
    private const string className = "eCloseRestClient";
    private static readonly string sw = Tracing.SwEFolder;
    private string apiURL = string.Empty;
    private string baseURL = string.Empty;
    private const string simpleFileGetUrl = "/collaboration/v1/spaces/simplifile/organization";
    private const string dcsSetupGetUrl = "/collaboration/v1/spaces/evault/organizations";
    private static string _sessionId = string.Empty;
    private const string scope = "sc";

    public eCloseRestClient()
    {
      eCloseRestClient._sessionId = EnhancedConditionRestApiHelper.SessionId;
      this.baseURL = string.IsNullOrEmpty(Session.StartupInfo.OAPIGatewayBaseUri) ? EnConfigurationSettings.AppSettings["oAuth.Url"] : Session.StartupInfo.OAPIGatewayBaseUri;
    }

    public string GetLenderOrganizationId()
    {
      Tracing.Log(eCloseRestClient.sw, TraceLevel.Verbose, nameof (eCloseRestClient), "GetLenderOrganization");
      try
      {
        this.apiURL = this.baseURL + "/collaboration/v1/spaces/simplifile/organization";
        Task<ApiResponse> apiCall = RestApiProxyFactory.GetApiCall(eCloseRestClient._sessionId, this.apiURL);
        Task.WaitAll((Task) apiCall);
        ApiResponse result = apiCall.Result;
        return result.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<eCloseRestClient.eCloseApiResponse>(result.ResponseString).id : "Invalid request";
      }
      catch (Exception ex)
      {
        Tracing.Log(eCloseRestClient.sw, nameof (eCloseRestClient), TraceLevel.Error, ex.ToString());
        return ex.InnerException?.GetType() == typeof (HttpException) && ((HttpException) ex.InnerException).GetHttpCode() == 404 ? (string) null : "Invalid request:" + ex.Message;
      }
    }

    public string CreateLenderOrganization(JObject payload)
    {
      string empty = string.Empty;
      Tracing.Log(eCloseRestClient.sw, TraceLevel.Verbose, nameof (eCloseRestClient), nameof (CreateLenderOrganization));
      string lenderOrganization;
      try
      {
        this.apiURL = "/collaboration/v1/spaces/simplifile/organization";
        Task<string> apiAsync = new RestApiProxyFactory().PutCallToAPIAsync(eCloseRestClient._sessionId, this.apiURL, (object) payload);
        Task.WaitAll((Task) apiAsync);
        // ISSUE: reference to a compiler-generated field
        if (eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "id", typeof (eCloseRestClient), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__0.Target((CallSite) eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__0, JsonConvert.DeserializeObject<object>(apiAsync.Result));
        // ISSUE: reference to a compiler-generated field
        if (eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (eCloseRestClient), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target1 = eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__4.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p4 = eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__4;
        // ISSUE: reference to a compiler-generated field
        if (eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.Not, typeof (eCloseRestClient), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target2 = eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p3 = eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__3;
        // ISSUE: reference to a compiler-generated field
        if (eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__2 = CallSite<Func<CallSite, System.Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "IsNullOrEmpty", (IEnumerable<System.Type>) null, typeof (eCloseRestClient), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, System.Type, object, object> target3 = eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, System.Type, object, object>> p2 = eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__2;
        System.Type type = typeof (string);
        // ISSUE: reference to a compiler-generated field
        if (eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<System.Type>) null, typeof (eCloseRestClient), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__1.Target((CallSite) eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__1, obj1);
        object obj3 = target3((CallSite) p2, type, obj2);
        object obj4 = target2((CallSite) p3, obj3);
        if (target1((CallSite) p4, obj4))
        {
          // ISSUE: reference to a compiler-generated field
          if (eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (eCloseRestClient)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, string> target4 = eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__6.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, string>> p6 = eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__6;
          // ISSUE: reference to a compiler-generated field
          if (eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<System.Type>) null, typeof (eCloseRestClient), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj5 = eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__5.Target((CallSite) eCloseRestClient.\u003C\u003Eo__10.\u003C\u003Ep__5, obj1);
          lenderOrganization = target4((CallSite) p6, obj5);
        }
        else
          lenderOrganization = "Error";
      }
      catch (Exception ex)
      {
        lenderOrganization = "Error:" + ex.InnerException.Message;
        Tracing.Log(eCloseRestClient.sw, nameof (eCloseRestClient), TraceLevel.Error, ex.ToString());
      }
      return lenderOrganization;
    }

    public bool CheckDCSSetupStatus()
    {
      Tracing.Log(eCloseRestClient.sw, TraceLevel.Verbose, nameof (eCloseRestClient), nameof (CheckDCSSetupStatus));
      try
      {
        this.apiURL = this.baseURL + "/collaboration/v1/spaces/evault/organizations";
        Task<ApiResponse> apiCall = RestApiProxyFactory.GetApiCall(eCloseRestClient._sessionId, this.apiURL);
        Task.WaitAll((Task) apiCall);
        ApiResponse result = apiCall.Result;
        if (result.StatusCode != HttpStatusCode.OK)
          throw new Exception("CheckDCSSetupStatus API call failed.");
        return !(result.ResponseString == "[]");
      }
      catch (Exception ex)
      {
        Tracing.Log(eCloseRestClient.sw, nameof (eCloseRestClient), TraceLevel.Error, ex.ToString());
        throw ex;
      }
    }

    [Serializable]
    public class eCloseApiResponse
    {
      public string id { get; set; }

      public string spaceId { get; set; }

      public string name { get; set; }
    }
  }
}
