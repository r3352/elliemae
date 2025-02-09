// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Services.WorkflowServiceClient
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Services
{
  public class WorkflowServiceClient
  {
    private static readonly string sw = Tracing.SwReportControl;
    private const string className = "WorkflowServiceClient�";
    private const string workflowProcessesSetupEndpoint = "/workflow/v1/processes�";

    public static WorkflowProcessResponse SetupWorkflowProcessAction(
      SessionObjects session,
      string accessToken,
      string instanceId)
    {
      WorkflowProcessResponse workflowProcessResponse = new WorkflowProcessResponse();
      string oapiGatewayBaseUri = session.StartupInfo.OAPIGatewayBaseUri;
      string str1 = "";
      string msg = "Calling Workflow processes method";
      try
      {
        Tracing.Log(WorkflowServiceClient.sw, TraceLevel.Verbose, nameof (WorkflowServiceClient), msg);
        string requestUriString = oapiGatewayBaseUri + "/workflow/v1/processes";
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.ContractResolver = (IContractResolver) new CamelCasePropertyNamesContractResolver();
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
        httpWebRequest.KeepAlive = false;
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Timeout = 30000;
        httpWebRequest.Headers["Authorization"] = accessToken;
        Random random = new Random();
        string[] strArray = new string[1]{ "createTask" };
        var data = new
        {
          processKey = "Auto_Validate_Process_" + instanceId,
          initialSteps = strArray,
          steps = new
          {
            createTask = new
            {
              stepType = "action",
              action = "urn:elli:task:action:task:create",
              parameters = new
              {
                type = "GatherBorrowerData_Template"
              }
            }
          }
        };
        string str2 = JsonConvert.SerializeObject((object) data, settings);
        using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
          streamWriter.Write(str2);
        using (WebResponse response = httpWebRequest.GetResponse())
        {
          using (Stream responseStream = response.GetResponseStream())
          {
            using (StreamReader streamReader = new StreamReader(responseStream))
              str1 = streamReader.ReadToEnd();
          }
        }
        workflowProcessResponse = JsonConvert.DeserializeObject<WorkflowProcessResponse>(str1);
      }
      catch (WebException ex)
      {
        WorkflowServiceClient.HandleWebException(ex, "Workflow processes API");
      }
      catch (Exception ex)
      {
        Tracing.Log(WorkflowServiceClient.sw, TraceLevel.Error, nameof (WorkflowServiceClient), string.Format("Workflow processes API Exception (Message {0})", (object) ex.Message));
      }
      return workflowProcessResponse;
    }

    private static void HandleWebException(WebException ex, string apiName)
    {
      HttpResponseError httpResponseError = (HttpResponseError) null;
      string end = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
      if (end != null)
        httpResponseError = JsonConvert.DeserializeObject<HttpResponseError>(end);
      if (httpResponseError == null || ex == null)
        return;
      Tracing.Log(WorkflowServiceClient.sw, TraceLevel.Error, nameof (WorkflowServiceClient), string.Format("{0} WebException (Message: {1}; Code: {2}; Summary: {3}; Details: {4} )", (object) apiName, (object) ex.Message, (object) httpResponseError.Code, (object) httpResponseError.Summary, (object) httpResponseError.Details));
    }
  }
}
