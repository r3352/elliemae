// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.WorkflowManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.Common;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class WorkflowManager : ManagerBase
  {
    private IBpmManager workflowMgr;

    internal static WorkflowManager Instance => Session.DefaultInstance.WorkflowManager;

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetWorkflowManager();
    }

    internal WorkflowManager(Sessions.Session session)
      : base(session, ClientSessionCacheID.Workflow)
    {
      this.workflowMgr = this.session.SessionObjects.BpmManager;
    }

    public WorksheetInfo[] GetMsWorksheetInfos()
    {
      WorksheetInfo[] subject = (WorksheetInfo[]) this.GetSubjectFromCache("worksheets");
      if (subject == null)
      {
        subject = this.workflowMgr.GetMsWorksheetInfos();
        this.SetSubjectCache("worksheets", (object) subject);
      }
      return subject;
    }

    public WorksheetInfo GetMsWorksheetInfo(string milestoneID)
    {
      foreach (WorksheetInfo msWorksheetInfo in this.GetMsWorksheetInfos())
      {
        if (msWorksheetInfo.MilestoneID == milestoneID)
          return msWorksheetInfo;
      }
      return new WorksheetInfo(milestoneID, (RoleSummaryInfo) null, false, (string) null, false, (InputFormInfo) null);
    }

    public WorksheetInfo GetMsWorksheetInfo(int coreMilestoneID)
    {
      return this.GetMsWorksheetInfo(string.Concat((object) coreMilestoneID));
    }

    public string[] GetMilestoneIDsByRoleID(int roleID)
    {
      return this.workflowMgr.GetMilestoneIDsByRoleID(roleID);
    }

    public Hashtable GetAllMilestoneAlertMessages()
    {
      return this.workflowMgr.GetAllMilestoneAlertMessages();
    }

    public void SetMsWorksheetInfo(WorksheetInfo wsInfo)
    {
      this.workflowMgr.SetMsWorksheetInfo(wsInfo);
      this.ClearCache();
    }

    public void UpdateMsWorksheetAlertMessage(string milestoneID, string alertMsg)
    {
      this.UpdateMsWorksheetAlertMessages(new Dictionary<string, string>()
      {
        {
          milestoneID,
          alertMsg
        }
      });
    }

    public void UpdateMsWorksheetAlertMessages(Dictionary<string, string> alertMsgsToUpdate)
    {
      this.workflowMgr.UpdateMsWorksheetAlertMessages(alertMsgsToUpdate);
    }

    public void SetOrUpdateMsWorksheetAlertMessages(
      Dictionary<WorksheetInfo, string> alertMsgsToUpdate)
    {
      this.workflowMgr.SetOrUpdateMsWorksheetAlertMessages(alertMsgsToUpdate);
    }

    public RoleInfo[] GetAllRoleFunctions() => this.workflowMgr.GetAllRoleFunctions();

    public RoleInfo[] GetAllRoleFunctionsByPersonaID(int personaID)
    {
      return this.workflowMgr.GetAllRoleFunctionsByPersonaID(personaID);
    }

    public RoleInfo[] GetRoleFunctionsByUserID(string userID)
    {
      return this.workflowMgr.GetRoleFunctionsByUserID(userID);
    }

    public RoleInfo GetRoleFunction(int roleID) => this.workflowMgr.GetRoleFunction(roleID);

    public int SetRoleFunction(RoleInfo roleInfo) => this.workflowMgr.SetRoleFunction(roleInfo);

    public void DeleteRoleFunction(int roleID) => this.workflowMgr.DeleteRoleFunction(roleID);

    public RolesMappingInfo[] GetAllRoleMappingInfos() => this.workflowMgr.GetAllRoleMappingInfos();

    public RolesMappingInfo GetRoleMappingInfo(RealWorldRoleID realWorldRoleID)
    {
      return this.workflowMgr.GetRoleMappingInfo(realWorldRoleID);
    }

    public void UpdateRoleMappingInfos(RolesMappingInfo[] rolesMappingInfos)
    {
      this.workflowMgr.UpdateRoleMappingInfos(rolesMappingInfos);
    }

    public RolesMappingInfo[] GetUsersRoleMapping(string userID)
    {
      return this.workflowMgr.GetUsersRoleMapping(userID);
    }

    public RolesMappingInfo GetUsersRoleMapping(string userID, RealWorldRoleID realWorldRoleID)
    {
      return this.workflowMgr.GetUsersRoleMapping(userID, realWorldRoleID);
    }
  }
}
