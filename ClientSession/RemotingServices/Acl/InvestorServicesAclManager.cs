// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.Acl.InvestorServicesAclManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices.Acl
{
  public class InvestorServicesAclManager : ManagerBase
  {
    private IInvestorServicesAclManager investorServicesAclManager;

    public InvestorServicesAclManager(Sessions.Session session)
      : base(session)
    {
      this.investorServicesAclManager = (IInvestorServicesAclManager) this.session.GetAclManager(AclCategory.InvestorServices);
    }

    public InvestorServiceAclInfo[] GetPermissions(
      AclFeature feature,
      int personaID,
      string category,
      InvestorServiceAclInfo[] availableServices)
    {
      return this.investorServicesAclManager.GetPermissions(feature, personaID, category, availableServices);
    }

    public InvestorServiceAclInfo.InvestorServicesDefaultSetting GetInvestorServicesDefaultSetting(
      AclFeature feature,
      int personaID,
      string category)
    {
      return this.investorServicesAclManager.GetInvestorServicesDefaultSetting(feature, personaID, category);
    }

    public InvestorServiceAclInfo[] GetPermissions(
      AclFeature feature,
      string userID,
      string category,
      Persona[] personaList,
      InvestorServiceAclInfo[] availableServices)
    {
      return this.investorServicesAclManager.GetPermissions(feature, userID, category, personaList, availableServices);
    }

    public InvestorServiceAclInfo[] GetPermissions(
      AclFeature[] features,
      string userID,
      Persona[] personaList,
      InvestorServiceAclInfo[] availableServices)
    {
      return this.investorServicesAclManager.GetPermissions(features, userID, personaList, availableServices);
    }

    public InvestorServiceAclInfo.InvestorServicesDefaultSetting GetInvestorServicesDefaultSetting(
      AclFeature feature,
      string userID,
      string category,
      Persona[] personaList)
    {
      return this.investorServicesAclManager.GetInvestorServicesDefaultSetting(feature, userID, category, personaList);
    }

    public InvestorServiceAclInfo.InvestorServicesDefaultSetting GetUserSpecificInvestorServicesDefaultSetting(
      AclFeature feature,
      string userID,
      string category,
      Persona[] personaList)
    {
      return this.investorServicesAclManager.GetUserSpecificInvestorServicesDefaultSetting(feature, userID, category, personaList);
    }

    public InvestorServiceAclInfo.InvestorServicesDefaultSetting GetPersonasInvestorServicesDefaultSetting(
      AclFeature feature,
      string userID,
      string category,
      Persona[] personaList)
    {
      return this.investorServicesAclManager.GetPersonasInvestorServicesDefaultSetting(feature, userID, category, personaList);
    }

    public void SetPermissions(
      AclFeature feature,
      InvestorServiceAclInfo[] InvestorServiceAclInfoList,
      string userid,
      string category,
      InvestorServiceAclInfo.InvestorServicesDefaultSetting defaultValue)
    {
      this.investorServicesAclManager.SetPermissions(feature, InvestorServiceAclInfoList, userid, category, defaultValue);
    }

    public void SetDefaultValue(
      AclFeature feature,
      string userid,
      string category,
      InvestorServiceAclInfo.InvestorServicesDefaultSetting defaultValue)
    {
      this.investorServicesAclManager.SetDefaultValue(feature, userid, category, defaultValue);
    }

    public void SetPermissions(
      AclFeature feature,
      InvestorServiceAclInfo[] InvestorServiceAclInfoList,
      int personaID,
      string category,
      InvestorServiceAclInfo.InvestorServicesDefaultSetting defaultValue)
    {
      this.investorServicesAclManager.SetPermissions(feature, InvestorServiceAclInfoList, personaID, category, defaultValue);
    }

    public void SetDefaultValue(
      AclFeature feature,
      int personaID,
      string category,
      InvestorServiceAclInfo.InvestorServicesDefaultSetting defaultValue)
    {
      this.investorServicesAclManager.SetDefaultValue(feature, personaID, category, defaultValue);
    }

    public void DuplicateACLInvestorServices(
      AclFeature feature,
      int sourcePersonaID,
      int desPersonaID,
      string category,
      InvestorServiceAclInfo[] availableServices)
    {
      this.investorServicesAclManager.DuplicateACLInvestorServices(feature, sourcePersonaID, desPersonaID, category, availableServices);
    }

    public bool GetUserApplicationRight(
      AclFeature feature,
      string partnerID,
      string category,
      List<InvestorServiceAclInfo> availableServices)
    {
      foreach (InvestorServiceAclInfo permission in this.investorServicesAclManager.GetPermissions(feature, this.session.UserID, category, this.session.UserInfo.UserPersonas, availableServices.ToArray()))
      {
        if (permission.ProviderCompanyCode == partnerID && permission.PersonaAccess == AclResourceAccess.ReadWrite)
          return true;
      }
      return false;
    }

    public override void ClearCaches(string key) => this.clearCache(key);
  }
}
