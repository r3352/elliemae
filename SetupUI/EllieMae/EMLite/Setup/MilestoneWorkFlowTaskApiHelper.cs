// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MilestoneWorkFlowTaskApiHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Workflow;
using Newtonsoft.Json;
using RestApiProxy;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public static class MilestoneWorkFlowTaskApiHelper
  {
    private static readonly string sw = Tracing.SwEFolder;
    private const string className = "MilestoneWorkFlowTaskApiHelper";
    private const string taskTemplatesBasePath = "/workflow/v1/templates/task/items";
    private const string taskGroupTemplatesBasePath = "/workflow/v1/templates/taskgroup/items";
    private const string taskTemplatesAllAccess = "/workflow/v1/templates/task/items?skipPersonaCheck={0}";
    private const string taskBasePath = "/workflow/v1/tasks";
    private const string taskFilter = "/workflow/v1/tasks?loanId={0}&types={1}";

    public static TaskTemplate[] GetTaskTemplates(bool allAccess = false)
    {
      Tracing.Log(MilestoneWorkFlowTaskApiHelper.sw, TraceLevel.Verbose, nameof (MilestoneWorkFlowTaskApiHelper), "Entering GetTaskTemplates");
      TaskTemplate[] taskTemplates = (TaskTemplate[]) null;
      try
      {
        taskTemplates = JsonConvert.DeserializeObject<TaskTemplate[]>(RestApiProxyFactory.GetApiCall(SetupUtil.ApiSessionId, allAccess ? string.Format("/workflow/v1/templates/task/items?skipPersonaCheck={0}", (object) allAccess) : "/workflow/v1/templates/task/items").Result.ResponseString);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, nameof (MilestoneWorkFlowTaskApiHelper), "Error in GetTaskTemplates.", ex);
      }
      return taskTemplates;
    }

    public static TaskGroupTemplate[] GetTaskGroupTemplates()
    {
      Tracing.Log(MilestoneWorkFlowTaskApiHelper.sw, TraceLevel.Verbose, nameof (MilestoneWorkFlowTaskApiHelper), "Entering GetTaskGroupTemplates");
      TaskGroupTemplate[] taskGroupTemplates = (TaskGroupTemplate[]) null;
      try
      {
        taskGroupTemplates = JsonConvert.DeserializeObject<TaskGroupTemplate[]>(RestApiProxyFactory.GetApiCall(SetupUtil.ApiSessionId, "/workflow/v1/templates/taskgroup/items").Result.ResponseString);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, nameof (MilestoneWorkFlowTaskApiHelper), "Error in GetTaskGroupTemplates.", ex);
      }
      return taskGroupTemplates;
    }

    public static WorkflowTask[] GetTasksBy(string loanId, string[] typeIds)
    {
      Tracing.Log(MilestoneWorkFlowTaskApiHelper.sw, TraceLevel.Verbose, nameof (MilestoneWorkFlowTaskApiHelper), "Entering GetTasks");
      try
      {
        return JsonConvert.DeserializeObject<WorkflowTask[]>(RestApiProxyFactory.GetApiCall(SetupUtil.ApiSessionId, string.Format("/workflow/v1/tasks?loanId={0}&types={1}", (object) loanId, (object) string.Join(",", typeIds))).Result.ResponseString);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, nameof (MilestoneWorkFlowTaskApiHelper), "Error in GetTasks.", ex);
        throw ex;
      }
    }
  }
}
