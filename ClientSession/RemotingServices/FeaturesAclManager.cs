// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.FeaturesAclManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class FeaturesAclManager : EllieMae.EMLite.RemotingServices.Acl.ManagerBase
  {
    private IFeaturesAclManager featuresAclMgr;

    internal static FeaturesAclManager Instance => Session.DefaultInstance.FeaturesAclManager;

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetFeaturesAclManager();
    }

    internal FeaturesAclManager(Sessions.Session session)
      : base(session)
    {
      this.featuresAclMgr = (IFeaturesAclManager) this.session.GetAclManager(AclCategory.Features);
    }

    public override void ClearCaches(string temp) => this.clearCache(temp);

    public void CacheLoginUserAclRight(Hashtable userRights)
    {
      if (this.session.EncompassEdition == EncompassEdition.Broker && !this.session.UserInfo.IsAdministrator())
      {
        userRights.Remove((object) AclFeature.LoanMgmt_Delete);
        userRights.Remove((object) AclFeature.LoanMgmt_TF_Delete);
        userRights.Remove((object) AclFeature.LoanMgmt_TF_Restore);
        userRights.Remove((object) AclFeature.SettingsTab_Company_CurrentLogins);
        userRights.Remove((object) AclFeature.SettingsTab_Company_DocumentSetup);
        userRights.Remove((object) AclFeature.SettingsTab_Company_LoanReassignment);
        userRights.Remove((object) AclFeature.SettingsTab_Company_LogUsersOut);
        userRights.Remove((object) AclFeature.SettingsTab_Company_SystemAuditTrail);
        userRights.Remove((object) AclFeature.SettingsTab_Company_ConditionalApprovalLetter);
        userRights.Remove((object) AclFeature.SettingsTab_Company_ConditionSetup);
        userRights.Remove((object) AclFeature.SettingsTab_Company_DocumentSetup);
      }
      this.setSubjectCache("UserRights", (object) userRights);
    }

    public Hashtable GetPermissions(AclFeature[] features, string userid)
    {
      UserInfo userInfo = !(userid == this.session.UserID) ? this.session.OrganizationManager.GetUser(userid) : this.session.UserInfo;
      if (!userInfo.IsSuperAdministrator() && !userInfo.IsAdministrator())
        return this.featuresAclMgr.GetPermissions(features, userid);
      Hashtable permissions = new Hashtable();
      foreach (AclFeature feature in features)
        permissions[(object) feature] = (object) AclTriState.True;
      return permissions;
    }

    public Hashtable GetPermissions(AclFeature[] features, int personaID)
    {
      return this.featuresAclMgr.GetPermissions(features, personaID);
    }

    public Hashtable GetPermissions(AclFeature[] features, Persona persona)
    {
      return this.featuresAclMgr.GetPermissions(features, persona.ID);
    }

    public Hashtable GetPermissions(AclFeature[] features, int[] personaIDs)
    {
      return this.featuresAclMgr.GetPermissions(features, personaIDs);
    }

    public Hashtable GetPermissions(AclFeature[] features, Persona[] personas)
    {
      int[] personaIDs = (int[]) null;
      if (personas != null)
      {
        personaIDs = new int[personas.Length];
        for (int index = 0; index < personas.Length; ++index)
          personaIDs[index] = personas[index].ID;
      }
      return this.featuresAclMgr.GetPermissions(features, personaIDs);
    }

    public object GetPermission(AclFeature feature, string userid)
    {
      UserInfo user = this.session.OrganizationManager.GetUser(userid);
      return UserInfo.IsSuperAdministrator(user.Userid, user.UserPersonas) ? (object) AclTriState.True : (object) this.featuresAclMgr.GetPermission(feature, userid);
    }

    public bool GetPermission(AclFeature feature, int personaID)
    {
      return this.featuresAclMgr.GetPermission(feature, personaID);
    }

    public bool GetPermission(AclFeature feature, int[] personaIDs)
    {
      return (bool) this.featuresAclMgr.GetPermissions(new AclFeature[1]
      {
        feature
      }, personaIDs)[(object) feature];
    }

    public bool GetPermission(AclFeature feature, Persona persona)
    {
      return this.featuresAclMgr.GetPermission(feature, persona.ID);
    }

    public bool CheckPermission(AclFeature feature, UserInfo userInfo)
    {
      return UserInfo.IsSuperAdministrator(userInfo.Userid, userInfo.UserPersonas) || this.featuresAclMgr.CheckPermission(feature, userInfo);
    }

    public bool CheckPermission(AclFeature feature, string userId)
    {
      return this.featuresAclMgr.CheckPermission(feature, userId);
    }

    public Hashtable CheckPermissions(AclFeature[] features, UserInfo userInfo)
    {
      if (!UserInfo.IsSuperAdministrator(userInfo.Userid, userInfo.UserPersonas))
        return this.featuresAclMgr.CheckPermissions(features, userInfo);
      Hashtable hashtable = new Hashtable();
      foreach (AclFeature feature in features)
        hashtable[(object) feature] = (object) true;
      return hashtable;
    }

    public void SetPermission(AclFeature feature, string userid, AclTriState access)
    {
      this.featuresAclMgr.SetPermission(feature, userid, access);
    }

    public void SetPermission(AclFeature feature, int personaID, AclTriState access)
    {
      this.featuresAclMgr.SetPermission(feature, personaID, access);
    }

    public void SetPermission(AclFeature feature, Persona persona, AclTriState access)
    {
      this.featuresAclMgr.SetPermission(feature, persona.ID, access);
    }

    public void SetPermissions(Hashtable featureAccesses, string userid)
    {
      this.featuresAclMgr.SetPermissions(featureAccesses, userid);
    }

    public void SetPermissions(Hashtable featureAccesses, int personaID)
    {
      this.featuresAclMgr.SetPermissions(featureAccesses, personaID);
    }

    public void SetPermissions(Hashtable featureAccesses, Persona persona)
    {
      this.featuresAclMgr.SetPermissions(featureAccesses, persona.ID);
    }

    public void DisablePermission(AclFeature feature)
    {
      this.featuresAclMgr.DisablePermission(feature);
    }

    public bool GetUserApplicationRight(AclFeature feature)
    {
      if (this.session.EncompassEdition == EncompassEdition.Broker && FeatureSets.IsBankerOnlyFeature(feature))
        return false;
      if (this.session.UserInfo.IsSuperAdministrator())
        return true;
      Hashtable hashtable = new Hashtable();
      if (this.getSubjectsFromCache("UserRights") != null)
        hashtable = (Hashtable) this.getSubjectsFromCache("UserRights");
      return hashtable.Contains((object) feature) && (bool) hashtable[(object) feature];
    }

    public void DuplicateACLFeatures(int sourcePersonaID, int desPersonaID)
    {
      this.featuresAclMgr.DuplicateACLFeatures(sourcePersonaID, desPersonaID);
    }

    public string[] GetPersonaListByFeature(AclFeature[] features, AclTriState access)
    {
      return this.featuresAclMgr.GetPersonaListByFeature(features, access);
    }

    public string[] GetUserListByFeature(AclFeature[] features, AclTriState access)
    {
      return this.featuresAclMgr.GetUserListByFeature(features, access);
    }
  }
}
