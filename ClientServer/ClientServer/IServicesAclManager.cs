// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IServicesAclManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IServicesAclManager
  {
    ServiceAclInfo[] GetPermissions(AclFeature feature, int personaID);

    ServiceAclInfo.ServicesDefaultSetting GetServicesDefaultSetting(
      AclFeature feature,
      int personaID);

    ServiceAclInfo[] GetPermissions(AclFeature feature, string userID, Persona[] personaList);

    ServiceAclInfo.ServicesDefaultSetting GetServicesDefaultSetting(
      AclFeature feature,
      string userID,
      Persona[] personaList);

    ServiceAclInfo.ServicesDefaultSetting GetUserSpecificServicesDefaultSetting(
      AclFeature feature,
      string userID,
      Persona[] personaList);

    ServiceAclInfo.ServicesDefaultSetting GetPersonasServicesDefaultSetting(
      AclFeature feature,
      string userID,
      Persona[] personaList);

    void SetPermissions(AclFeature feature, ServiceAclInfo[] serviceAclInfoList, string userid);

    void SetDefaultValue(
      AclFeature feature,
      string userid,
      ServiceAclInfo.ServicesDefaultSetting defaultValue);

    void SetPermissions(AclFeature feature, ServiceAclInfo[] serviceAclInfoList, int personaID);

    void SetDefaultValue(
      AclFeature feature,
      int personaID,
      ServiceAclInfo.ServicesDefaultSetting defaultValue);

    void DuplicateACLServices(int sourcePersonaID, int desPersonaID);
  }
}
