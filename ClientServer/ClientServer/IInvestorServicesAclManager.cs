// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IInvestorServicesAclManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IInvestorServicesAclManager
  {
    InvestorServiceAclInfo[] GetPermissions(
      AclFeature feature,
      int personaID,
      string category,
      InvestorServiceAclInfo[] availableServices);

    InvestorServiceAclInfo.InvestorServicesDefaultSetting GetInvestorServicesDefaultSetting(
      AclFeature feature,
      int personaID,
      string category);

    InvestorServiceAclInfo[] GetPermissions(
      AclFeature feature,
      string userID,
      string category,
      Persona[] personaList,
      InvestorServiceAclInfo[] availableServices);

    InvestorServiceAclInfo[] GetPermissions(
      AclFeature[] features,
      string userID,
      Persona[] personaList,
      InvestorServiceAclInfo[] availableServices);

    InvestorServiceAclInfo.InvestorServicesDefaultSetting GetInvestorServicesDefaultSetting(
      AclFeature feature,
      string userID,
      string category,
      Persona[] personaList);

    InvestorServiceAclInfo.InvestorServicesDefaultSetting GetUserSpecificInvestorServicesDefaultSetting(
      AclFeature feature,
      string userID,
      string category,
      Persona[] personaList);

    InvestorServiceAclInfo.InvestorServicesDefaultSetting GetPersonasInvestorServicesDefaultSetting(
      AclFeature feature,
      string userID,
      string category,
      Persona[] personaList);

    void SetPermissions(
      AclFeature feature,
      InvestorServiceAclInfo[] InvestorServiceAclInfoList,
      string userid,
      string category,
      InvestorServiceAclInfo.InvestorServicesDefaultSetting defaultValue);

    void SetDefaultValue(
      AclFeature feature,
      string userid,
      string category,
      InvestorServiceAclInfo.InvestorServicesDefaultSetting defaultValue);

    void SetPermissions(
      AclFeature feature,
      InvestorServiceAclInfo[] InvestorServiceAclInfoList,
      int personaID,
      string category,
      InvestorServiceAclInfo.InvestorServicesDefaultSetting defaultValue);

    void SetDefaultValue(
      AclFeature feature,
      int personaID,
      string category,
      InvestorServiceAclInfo.InvestorServicesDefaultSetting defaultValue);

    void DuplicateACLInvestorServices(
      AclFeature feature,
      int sourcePersonaID,
      int desPersonaID,
      string category,
      InvestorServiceAclInfo[] availableServices);
  }
}
