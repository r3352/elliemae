// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EnhancedConditionRestApiHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.eFolder;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestApiProxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public static class EnhancedConditionRestApiHelper
  {
    public const string EnhancedConditionsBasePath = "/encompass/v3/settings/loan/conditions/";
    public const string EnhancedConditionsTypeGet = "/encompass/v3/settings/loan/conditions/types";
    public const string EnhancedConditionsTypeInActiveFlag = "?activeOnly=false";
    public const string EnhancedConditionsTypeActiveFlag = "?activeOnly=true";
    public const string EnhancedConditionsTypeDetailsFlag = "&view=detail";
    public const string EnhancedConditionsTypeGetById = "/encompass/v3/settings/loan/conditions/types/ID";
    public const string EnhancedConditionsTypeAddFlag = "?view=entity&action=add";
    public const string EnhancedConditionsTypeUpdateFlag = "?view=entity&action=update";
    public const string EnhancedConditionsTypeDeleteFlag = "?action=delete&templateOption=delete";
    public const string EnhancedConditionTemplate = "/encompass/v3/settings/loan/conditions/templates?action={0}&view={1}";
    public const string EnhancedConditionTemplateGet = "/encompass/v3/settings/loan/conditions/templates?activeOnly={0}&view={1}";
    private static readonly string sw = Tracing.SwEFolder;
    private const string className = "EnhancedConditionsRestAPIHelper";

    public static string SessionId
    {
      get
      {
        return (Session.DefaultInstance.ServerIdentity != null ? Session.DefaultInstance.ServerIdentity.InstanceName : "LOCALHOST") + "_" + Session.DefaultInstance.SessionID;
      }
    }

    public static EnhancedConditionType[] GetEnhancedConditionTypes(bool getActive = false, bool getDetails = false)
    {
      Tracing.Log(EnhancedConditionRestApiHelper.sw, TraceLevel.Verbose, "EnhancedConditionsRestAPIHelper", "Entering GetEnhancedConditionTypes");
      string formattedUrl = "/encompass/v3/settings/loan/conditions/types" + (getActive ? "?activeOnly=true" : "?activeOnly=false");
      if (getDetails)
        formattedUrl += "&view=detail";
      return JsonConvert.DeserializeObject<EnhancedConditionType[]>(RestApiProxyFactory.GetApiCall(EnhancedConditionRestApiHelper.SessionId, formattedUrl).Result.ResponseString);
    }

    public static EnhancedConditionType GetConditionTypeById(string Id)
    {
      Tracing.Log(EnhancedConditionRestApiHelper.sw, TraceLevel.Verbose, "EnhancedConditionsRestAPIHelper", "Entering GetConditionTypeId");
      return JsonConvert.DeserializeObject<EnhancedConditionType>(RestApiProxyFactory.GetApiCall(EnhancedConditionRestApiHelper.SessionId, "/encompass/v3/settings/loan/conditions/types/ID".Replace("ID", Id)).Result.ResponseString);
    }

    public static EnhancedConditionType[] AddEnhancedConditionTypes(
      ConditionTypeContract conditionType)
    {
      Tracing.Log(EnhancedConditionRestApiHelper.sw, TraceLevel.Verbose, "EnhancedConditionsRestAPIHelper", "Entering AddEnhancedConditionTypes");
      return EnhancedConditionRestApiHelper.AddConditionTypes((object) conditionType, true);
    }

    public static async Task<bool> DeleteEnhancedConditionTypes(params EnhancedConditionType[] types)
    {
      string apiAsync = await new RestApiProxyFactory().PatchCallToAPIAsync(EnhancedConditionRestApiHelper.SessionId, "/encompass/v3/settings/loan/conditions/types?action=delete&templateOption=delete", (object) ((IEnumerable<EnhancedConditionType>) types).Select(type => new
      {
        id = type.id
      }).ToArray());
      return true;
    }

    public static bool UpdateConditionTypes(object conditionType)
    {
      try
      {
        // ISSUE: reference to a compiler-generated field
        if (EnhancedConditionRestApiHelper.\u003C\u003Eo__19.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          EnhancedConditionRestApiHelper.\u003C\u003Eo__19.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, EnhancedConditionType[]>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (EnhancedConditionType[]), typeof (EnhancedConditionRestApiHelper)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, EnhancedConditionType[]> target = EnhancedConditionRestApiHelper.\u003C\u003Eo__19.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, EnhancedConditionType[]>> p1 = EnhancedConditionRestApiHelper.\u003C\u003Eo__19.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (EnhancedConditionRestApiHelper.\u003C\u003Eo__19.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          EnhancedConditionRestApiHelper.\u003C\u003Eo__19.\u003C\u003Ep__0 = CallSite<Func<CallSite, System.Type, object, bool, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "AddConditionTypes", (IEnumerable<System.Type>) null, typeof (EnhancedConditionRestApiHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = EnhancedConditionRestApiHelper.\u003C\u003Eo__19.\u003C\u003Ep__0.Target((CallSite) EnhancedConditionRestApiHelper.\u003C\u003Eo__19.\u003C\u003Ep__0, typeof (EnhancedConditionRestApiHelper), conditionType, false);
        EnhancedConditionType[] enhancedConditionTypeArray = target((CallSite) p1, obj);
        return enhancedConditionTypeArray != null && enhancedConditionTypeArray.Length != 0;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EnhancedConditionsRestAPIHelper", "Error in UpdateConditionTypes.", ex);
        throw ex;
      }
    }

    public static EnhancedConditionType[] AddConditionTypes(object conditionType, bool isAdd)
    {
      RestApiProxyFactory restApiProxyFactory = new RestApiProxyFactory();
      // ISSUE: reference to a compiler-generated field
      if (EnhancedConditionRestApiHelper.\u003C\u003Eo__20.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EnhancedConditionRestApiHelper.\u003C\u003Eo__20.\u003C\u003Ep__0 = CallSite<Func<CallSite, RestApiProxyFactory, string, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PatchCallToAPIAsync", (IEnumerable<System.Type>) null, typeof (EnhancedConditionRestApiHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = EnhancedConditionRestApiHelper.\u003C\u003Eo__20.\u003C\u003Ep__0.Target((CallSite) EnhancedConditionRestApiHelper.\u003C\u003Eo__20.\u003C\u003Ep__0, restApiProxyFactory, EnhancedConditionRestApiHelper.SessionId, "/encompass/v3/settings/loan/conditions/types" + (isAdd ? "?view=entity&action=add" : "?view=entity&action=update"), conditionType);
      // ISSUE: reference to a compiler-generated field
      if (EnhancedConditionRestApiHelper.\u003C\u003Eo__20.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EnhancedConditionRestApiHelper.\u003C\u003Eo__20.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, EnhancedConditionType[]>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (EnhancedConditionType[]), typeof (EnhancedConditionRestApiHelper)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, EnhancedConditionType[]> target1 = EnhancedConditionRestApiHelper.\u003C\u003Eo__20.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, EnhancedConditionType[]>> p3 = EnhancedConditionRestApiHelper.\u003C\u003Eo__20.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (EnhancedConditionRestApiHelper.\u003C\u003Eo__20.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EnhancedConditionRestApiHelper.\u003C\u003Eo__20.\u003C\u003Ep__2 = CallSite<Func<CallSite, System.Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", (IEnumerable<System.Type>) new System.Type[1]
        {
          typeof (EnhancedConditionType[])
        }, typeof (EnhancedConditionRestApiHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, System.Type, object, object> target2 = EnhancedConditionRestApiHelper.\u003C\u003Eo__20.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, System.Type, object, object>> p2 = EnhancedConditionRestApiHelper.\u003C\u003Eo__20.\u003C\u003Ep__2;
      System.Type type = typeof (JsonConvert);
      // ISSUE: reference to a compiler-generated field
      if (EnhancedConditionRestApiHelper.\u003C\u003Eo__20.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EnhancedConditionRestApiHelper.\u003C\u003Eo__20.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof (EnhancedConditionRestApiHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = EnhancedConditionRestApiHelper.\u003C\u003Eo__20.\u003C\u003Ep__1.Target((CallSite) EnhancedConditionRestApiHelper.\u003C\u003Eo__20.\u003C\u003Ep__1, obj1);
      object obj3 = target2((CallSite) p2, type, obj2);
      return target1((CallSite) p3, obj3);
    }

    public static EnhancedConditionType[] UpdateConditionTypesApi(object conditionType)
    {
      RestApiProxyFactory restApiProxyFactory = new RestApiProxyFactory();
      // ISSUE: reference to a compiler-generated field
      if (EnhancedConditionRestApiHelper.\u003C\u003Eo__21.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EnhancedConditionRestApiHelper.\u003C\u003Eo__21.\u003C\u003Ep__0 = CallSite<Func<CallSite, RestApiProxyFactory, string, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PatchCallToAPIAsync", (IEnumerable<System.Type>) null, typeof (EnhancedConditionRestApiHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = EnhancedConditionRestApiHelper.\u003C\u003Eo__21.\u003C\u003Ep__0.Target((CallSite) EnhancedConditionRestApiHelper.\u003C\u003Eo__21.\u003C\u003Ep__0, restApiProxyFactory, EnhancedConditionRestApiHelper.SessionId, "/encompass/v3/settings/loan/conditions/types?view=entity&action=update", conditionType);
      // ISSUE: reference to a compiler-generated field
      if (EnhancedConditionRestApiHelper.\u003C\u003Eo__21.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EnhancedConditionRestApiHelper.\u003C\u003Eo__21.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, EnhancedConditionType[]>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (EnhancedConditionType[]), typeof (EnhancedConditionRestApiHelper)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, EnhancedConditionType[]> target1 = EnhancedConditionRestApiHelper.\u003C\u003Eo__21.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, EnhancedConditionType[]>> p3 = EnhancedConditionRestApiHelper.\u003C\u003Eo__21.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (EnhancedConditionRestApiHelper.\u003C\u003Eo__21.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EnhancedConditionRestApiHelper.\u003C\u003Eo__21.\u003C\u003Ep__2 = CallSite<Func<CallSite, System.Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", (IEnumerable<System.Type>) new System.Type[1]
        {
          typeof (EnhancedConditionType[])
        }, typeof (EnhancedConditionRestApiHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, System.Type, object, object> target2 = EnhancedConditionRestApiHelper.\u003C\u003Eo__21.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, System.Type, object, object>> p2 = EnhancedConditionRestApiHelper.\u003C\u003Eo__21.\u003C\u003Ep__2;
      System.Type type = typeof (JsonConvert);
      // ISSUE: reference to a compiler-generated field
      if (EnhancedConditionRestApiHelper.\u003C\u003Eo__21.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EnhancedConditionRestApiHelper.\u003C\u003Eo__21.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Result", typeof (EnhancedConditionRestApiHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = EnhancedConditionRestApiHelper.\u003C\u003Eo__21.\u003C\u003Ep__1.Target((CallSite) EnhancedConditionRestApiHelper.\u003C\u003Eo__21.\u003C\u003Ep__1, obj1);
      object obj3 = target2((CallSite) p2, type, obj2);
      return target1((CallSite) p3, obj3);
    }

    public static string[] AddConditionTemplates(
      EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate[] conditionTemplates,
      bool isActiveDeactivatecall,
      string apiAction = "add",
      string viewEntity = "id")
    {
      IgnorePropertySerializer propertySerializer = new IgnorePropertySerializer();
      propertySerializer.IgnoreProperty(typeof (EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate), "CreatedBy", "CreatedDate", "LastModifiedBy", "LastModifiedDate");
      string content = JsonConvert.SerializeObject((object) conditionTemplates, new JsonSerializerSettings()
      {
        ContractResolver = (IContractResolver) propertySerializer,
        NullValueHandling = NullValueHandling.Ignore
      });
      if (apiAction.ToLower() == "update" && !isActiveDeactivatecall)
      {
        bool flag1 = true;
        bool flag2 = true;
        foreach (EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate conditionTemplate in conditionTemplates)
        {
          if (conditionTemplate.DaysToReceive.HasValue)
            flag1 = false;
          if (conditionTemplate.Owner != null)
            flag2 = false;
          if (!(flag1 | flag2))
            break;
        }
        if (flag1)
        {
          int startIndex = content.LastIndexOf('}');
          string str = ",\"DaysToReceive\":null";
          content = content.Insert(startIndex, str);
        }
        if (flag2)
        {
          int startIndex = content.LastIndexOf("}");
          string str = ",\"Owner\":null";
          content = content.Insert(startIndex, str);
        }
      }
      Task<ApiResponse> task = RestApiProxyFactory.PatchAPICall((HttpContent) new StringContent(content, Encoding.UTF8, "application/json"), EnhancedConditionRestApiHelper.SessionId, string.Format("/encompass/v3/settings/loan/conditions/templates?action={0}&view={1}", (object) apiAction, (object) viewEntity));
      if (task == null)
        return (string[]) null;
      if (task.Result.StatusCode == HttpStatusCode.Conflict)
        throw new HttpException((int) task.Result.StatusCode, task.Result.ResponseString);
      var anonymousTypeObject = new
      {
        id = new List<string>()
      };
      return JsonConvert.DeserializeAnonymousType(task.Result.ResponseString, anonymousTypeObject)?.id.ToArray();
    }

    public static EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate[] UpdateConditionTemplates(
      EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate[] conditionTemplate,
      string apiAction = "add",
      string viewEntity = "entity")
    {
      IgnorePropertySerializer propertySerializer = new IgnorePropertySerializer();
      propertySerializer.IgnoreProperty(typeof (EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate), "CreatedBy", "CreatedDate", "LastModifiedBy", "LastModifiedDate");
      Task<ApiResponse> task = RestApiProxyFactory.PatchAPICall((HttpContent) new StringContent(JsonConvert.SerializeObject((object) conditionTemplate, new JsonSerializerSettings()
      {
        ContractResolver = (IContractResolver) propertySerializer,
        NullValueHandling = NullValueHandling.Ignore
      }), Encoding.UTF8, "application/json"), EnhancedConditionRestApiHelper.SessionId, string.Format("/encompass/v3/settings/loan/conditions/templates?action={0}&view={1}", (object) apiAction, (object) viewEntity));
      if (task == null)
        return (EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate[]) null;
      if (task.Result.StatusCode == HttpStatusCode.Conflict)
        throw new HttpException((int) task.Result.StatusCode, task.Result.ResponseString);
      return JsonConvert.DeserializeObject<EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate[]>(task.Result.ResponseString);
    }

    public static EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate[] GetEnhancedConditionTemplates(
      string view = "detail",
      bool getActive = false)
    {
      Tracing.Log(EnhancedConditionRestApiHelper.sw, TraceLevel.Verbose, "EnhancedConditionsRestAPIHelper", "Entering GetEnhancedConditionTemplates");
      EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate[] conditionTemplates = (EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate[]) null;
      try
      {
        conditionTemplates = JsonConvert.DeserializeObject<EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate[]>(RestApiProxyFactory.GetApiCall(EnhancedConditionRestApiHelper.SessionId, string.Format("/encompass/v3/settings/loan/conditions/templates?activeOnly={0}&view={1}", (object) Convert.ToString(getActive), (object) view)).Result.ResponseString);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EnhancedConditionsRestAPIHelper", "Error in GetEnhancedConditionTemplates.", ex);
      }
      return conditionTemplates;
    }

    public static EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate[] GetEnhancedConditionTemplatesByConditionType(
      string conditionType,
      string view = "summary",
      bool getActive = false)
    {
      Tracing.Log(EnhancedConditionRestApiHelper.sw, TraceLevel.Verbose, "EnhancedConditionsRestAPIHelper", "Entering GetEnhancedConditionTemplates");
      EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate[] templatesByConditionType = (EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate[]) null;
      try
      {
        templatesByConditionType = JsonConvert.DeserializeObject<EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate[]>(RestApiProxyFactory.GetApiCall(EnhancedConditionRestApiHelper.SessionId, string.Format("/encompass/v3/settings/loan/conditions/templates?activeOnly={0}&view={1}&conditionTypes=" + conditionType, (object) Convert.ToString(getActive), (object) view)).Result.ResponseString);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EnhancedConditionsRestAPIHelper", "Error in GetEnhancedConditionTemplates.", ex);
      }
      return templatesByConditionType;
    }
  }
}
