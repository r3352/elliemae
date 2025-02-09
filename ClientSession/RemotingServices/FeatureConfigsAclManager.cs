// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.FeatureConfigsAclManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class FeatureConfigsAclManager : EllieMae.EMLite.RemotingServices.Acl.ManagerBase
  {
    private IFeatureConfigsAclManager featureConfigsAclMgr;

    internal static FeatureConfigsAclManager Instance
    {
      get => Session.DefaultInstance.FeatureConfigsAclManager;
    }

    internal FeatureConfigsAclManager(Sessions.Session session)
      : base(session)
    {
      this.featureConfigsAclMgr = (IFeatureConfigsAclManager) this.session.GetAclManager(AclCategory.FeatureConfigs);
    }

    public override void ClearCaches(string temp) => this.clearCache(temp);

    public void CacheLoginUserAclRight(Dictionary<AclFeature, int> userRights)
    {
      if (this.session.EncompassEdition == EncompassEdition.Broker)
        this.session.UserInfo.IsAdministrator();
      this.setSubjectCache("UserRights", (object) userRights);
    }

    public Dictionary<AclFeature, int> GetPermissions(AclFeature[] features, string userid)
    {
      if (!(!(userid == this.session.UserID) ? this.session.OrganizationManager.GetUser(userid) : this.session.UserInfo).IsSuperAdministrator())
        return this.featureConfigsAclMgr.GetPermissions(features, userid);
      Dictionary<AclFeature, int> permissions = new Dictionary<AclFeature, int>();
      foreach (AclFeature feature in features)
        permissions[feature] = int.MaxValue;
      return permissions;
    }

    public Dictionary<AclFeature, int> GetPermissions(string userid)
    {
      return this.GetPermissions(FeatureSets.AllConfigs, userid);
    }

    public Dictionary<AclFeature, int> GetPermissions(AclFeature[] features, int personaID)
    {
      return this.featureConfigsAclMgr.GetPermissions(features, personaID);
    }

    public Dictionary<AclFeature, int> GetPermissions(AclFeature[] features, Persona persona)
    {
      return this.featureConfigsAclMgr.GetPermissions(features, persona.ID);
    }

    public Dictionary<AclFeature, int> GetPermissions(AclFeature[] features, int[] personaIDs)
    {
      return this.featureConfigsAclMgr.GetPermissions(features, personaIDs);
    }

    public Dictionary<AclFeature, int> GetPermissions(AclFeature[] features, Persona[] personas)
    {
      int[] personaIDs = (int[]) null;
      if (personas != null)
      {
        personaIDs = new int[personas.Length];
        for (int index = 0; index < personas.Length; ++index)
          personaIDs[index] = personas[index].ID;
      }
      return this.featureConfigsAclMgr.GetPermissions(features, personaIDs);
    }

    public int GetPermission(AclFeature feature, string userid)
    {
      return this.featureConfigsAclMgr.GetPermission(feature, userid);
    }

    public int GetPermission(AclFeature feature, int personaID)
    {
      return this.featureConfigsAclMgr.GetPermission(feature, personaID);
    }

    public int GetPermission(AclFeature feature, Persona persona)
    {
      return this.featureConfigsAclMgr.GetPermission(feature, persona.ID);
    }

    public int CheckPermission(AclFeature feature, UserInfo userInfo)
    {
      return UserInfo.IsSuperAdministrator(userInfo.Userid, userInfo.UserPersonas) ? int.MaxValue : this.featureConfigsAclMgr.CheckPermission(feature, userInfo);
    }

    public Dictionary<AclFeature, int> CheckPermissions(AclFeature[] features, UserInfo userInfo)
    {
      if (!UserInfo.IsSuperAdministrator(userInfo.Userid, userInfo.UserPersonas))
        return this.featureConfigsAclMgr.CheckPermissions(features, userInfo);
      Dictionary<AclFeature, int> dictionary = new Dictionary<AclFeature, int>();
      foreach (AclFeature feature in features)
        dictionary[feature] = int.MaxValue;
      return dictionary;
    }

    public void SetPermission(AclFeature feature, string userid, int access)
    {
      this.featureConfigsAclMgr.SetPermission(feature, userid, access);
    }

    public void SetPermission(AclFeature feature, int personaID, int access)
    {
      this.featureConfigsAclMgr.SetPermission(feature, personaID, access);
    }

    public void SetPermission(AclFeature feature, Persona persona, int access)
    {
      this.featureConfigsAclMgr.SetPermission(feature, persona.ID, access);
    }

    public void SetPermissions(Dictionary<AclFeature, int> featureAccesses, string userid)
    {
      this.featureConfigsAclMgr.SetPermissions(featureAccesses, userid);
    }

    public void SetPermissions(Dictionary<AclFeature, int> featureAccesses, int personaID)
    {
      this.featureConfigsAclMgr.SetPermissions(featureAccesses, personaID);
    }

    public void SetPermissions(Dictionary<AclFeature, int> featureAccesses, Persona persona)
    {
      this.featureConfigsAclMgr.SetPermissions(featureAccesses, persona.ID);
    }

    public void ClearUserSpecificSettings(string userid)
    {
      this.featureConfigsAclMgr.ClearUserSpecificSettings(userid);
    }

    public void ClearUserSpecificSettings(AclFeature feature, string userid)
    {
      this.featureConfigsAclMgr.ClearUserSpecificSettings(feature, userid);
    }

    public int GetUserApplicationRight(AclFeature feature)
    {
      if (this.session.EncompassEdition == EncompassEdition.Broker && FeatureSets.IsBankerOnlyFeature(feature))
        return 0;
      if (this.session.UserInfo.IsSuperAdministrator())
        return int.MaxValue;
      Dictionary<AclFeature, int> dictionary = new Dictionary<AclFeature, int>();
      if (this.getSubjectsFromCache("UserRights") != null)
        dictionary = (Dictionary<AclFeature, int>) this.getSubjectsFromCache("UserRights");
      return dictionary.ContainsKey(feature) ? dictionary[feature] : 0;
    }

    public void DuplicateACLFeatures(int sourcePersonaID, int desPersonaID)
    {
      this.featureConfigsAclMgr.DuplicateACLFeatureConfigs(sourcePersonaID, desPersonaID);
    }
  }
}
