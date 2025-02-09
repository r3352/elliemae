// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IMilestonesAclManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IMilestonesAclManager
  {
    AclTriState GetPermission(AclMilestone feature, string customMilestoneId, string userId);

    bool GetPermission(AclMilestone feature, string customMilestoneId, int personaId);

    Hashtable GetPermissions(AclMilestone feature, string customMilestoneId, int[] personaIDs);

    Hashtable GetPermissions(AclMilestone feature, string customMilestoneId, Persona[] personas);

    void SetPermission(
      AclMilestone feature,
      string customMilestoneId,
      string userid,
      object access);

    void SetPermission(AclMilestone feature, string customMilestoneId, int personaID, bool access);

    bool CheckPermission(AclMilestone feature, string customMilestoneId, UserInfo userInfo);

    Hashtable CheckPermissions(AclMilestone[] features, string milestoneStage);

    Hashtable CheckPermissions(AclMilestone[] features, string milestoneStage, string userID);

    Dictionary<string, AclTriState> CheckApplicationPermissions(
      AclMilestone feature,
      string[] customMilestoneIDs,
      UserInfo user);

    Hashtable GetPersonalPermission(AclMilestone[] features, string userId);

    void DuplicateACLMilestones(int sourcePersonaID, int desPersonaID);

    void DeleteUserSpecificSetting(string userID);

    void SynchronizeAdminRight();

    void SynchronizeBrokerSetting(string baseMilestoneID, string currentMilestoneID);

    void SynchronizePersonaSettingWithNewMilestone(int personaID, bool defaultAccess);

    Dictionary<string, bool> GetPermissionsForCustomMilestone(AclMilestone feature, int personaID);

    Dictionary<string, Hashtable> GetPermissions(
      List<EllieMae.EMLite.Workflow.Milestone> milestones,
      AclMilestone[] features,
      UserInfo currentuser);
  }
}
