// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.MilestonesAclManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class MilestonesAclManager
  {
    private IMilestonesAclManager MilestonesAclMgr;
    private Hashtable cachedMilestones = new Hashtable();

    public MilestonesAclManager(Sessions.Session session)
    {
      this.MilestonesAclMgr = (IMilestonesAclManager) session.GetAclManager(AclCategory.Milestones);
    }

    public object GetPermission(AclMilestone feature, string customMilestoneId, string userid)
    {
      return (object) this.MilestonesAclMgr.GetPermission(feature, customMilestoneId, userid);
    }

    public bool GetPermission(AclMilestone feature, string customMilestoneId, int personaID)
    {
      return this.MilestonesAclMgr.GetPermission(feature, customMilestoneId, personaID);
    }

    public Hashtable GetPermissions(
      AclMilestone feature,
      string customMilestoneId,
      int[] personaIDs)
    {
      return this.MilestonesAclMgr.GetPermissions(feature, customMilestoneId, personaIDs);
    }

    public Hashtable GetPermissions(
      AclMilestone feature,
      string customMilestoneId,
      Persona[] personas)
    {
      return this.MilestonesAclMgr.GetPermissions(feature, customMilestoneId, personas);
    }

    public void SetPermission(
      AclMilestone feature,
      string customMilestoneId,
      string userid,
      object access)
    {
      this.MilestonesAclMgr.SetPermission(feature, customMilestoneId, userid, access);
    }

    public Dictionary<string, AclTriState> CheckApplicationPermissions(
      AclMilestone feature,
      string[] customMilestoneIDs,
      UserInfo user)
    {
      return this.MilestonesAclMgr.CheckApplicationPermissions(feature, customMilestoneIDs, user);
    }

    public void SetPermission(
      AclMilestone feature,
      string customMilestoneId,
      int personaID,
      bool access)
    {
      this.MilestonesAclMgr.SetPermission(feature, customMilestoneId, personaID, access);
    }

    public bool CheckPermission(AclMilestone feature, string customMilestoneId, UserInfo userInfo)
    {
      return this.MilestonesAclMgr.CheckPermission(feature, customMilestoneId, userInfo);
    }

    public Hashtable CheckPermissions(AclMilestone[] features, string milestoneStage)
    {
      return this.MilestonesAclMgr.CheckPermissions(features, milestoneStage);
    }

    public Hashtable CheckPermissions(
      AclMilestone[] features,
      string milestoneStage,
      string userID)
    {
      return this.MilestonesAclMgr.CheckPermissions(features, milestoneStage, userID);
    }

    public bool CheckPermission(AclMilestone feature, string milestoneStage)
    {
      Hashtable hashtable = this.MilestonesAclMgr.CheckPermissions(new AclMilestone[1]
      {
        feature
      }, milestoneStage);
      return hashtable != null && hashtable.ContainsKey((object) feature) && (bool) hashtable[(object) feature];
    }

    public bool CheckPermission(AclMilestone feature, string milestoneStage, string userID)
    {
      Hashtable hashtable = this.MilestonesAclMgr.CheckPermissions(new AclMilestone[1]
      {
        feature
      }, milestoneStage, userID);
      return hashtable != null && hashtable.ContainsKey((object) feature) && (bool) hashtable[(object) feature];
    }

    public void DuplicateACLMilestones(int sourcePersonaID, int desPersonaID)
    {
      this.MilestonesAclMgr.DuplicateACLMilestones(sourcePersonaID, desPersonaID);
    }

    public Hashtable GetPersonalPermission(AclMilestone[] features, string userId)
    {
      return this.MilestonesAclMgr.GetPersonalPermission(features, userId);
    }

    public void DeleteUserSpecificSetting(string userID)
    {
      this.MilestonesAclMgr.DeleteUserSpecificSetting(userID);
    }

    public void SynchronizeAdminRight() => this.MilestonesAclMgr.SynchronizeAdminRight();

    public void SynchronizePersonaSettingWithNewMilestone(int personaID, bool defaultAccess)
    {
      this.MilestonesAclMgr.SynchronizePersonaSettingWithNewMilestone(personaID, defaultAccess);
    }

    public void SynchronizeBrokerSetting(string baseMilestoneID, string currentMilestoneID)
    {
      this.MilestonesAclMgr.SynchronizeBrokerSetting(baseMilestoneID, currentMilestoneID);
    }

    public Dictionary<string, bool> GetPermissionsForMilestones(AclMilestone feature, int personaID)
    {
      return this.MilestonesAclMgr.GetPermissionsForCustomMilestone(feature, personaID);
    }
  }
}
