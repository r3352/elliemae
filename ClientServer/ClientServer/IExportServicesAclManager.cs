// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IExportServicesAclManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IExportServicesAclManager
  {
    ExportServiceAclInfo[] GetPermissions(
      AclFeature feature,
      int personaID,
      ExportServiceAclInfo[] availableServices);

    ExportServiceAclInfo.ExportServicesDefaultSetting GetExportServicesDefaultSetting(
      AclFeature feature,
      int personaID);

    ExportServiceAclInfo[] GetPermissions(
      AclFeature feature,
      string userID,
      Persona[] personaList,
      ExportServiceAclInfo[] availableServices);

    ExportServiceAclInfo.ExportServicesDefaultSetting GetExportServicesDefaultSetting(
      AclFeature feature,
      string userID,
      Persona[] personaList);

    ExportServiceAclInfo.ExportServicesDefaultSetting GetUserSpecificExportServicesDefaultSetting(
      AclFeature feature,
      string userID,
      Persona[] personaList);

    ExportServiceAclInfo.ExportServicesDefaultSetting GetPersonasExportServicesDefaultSetting(
      AclFeature feature,
      string userID,
      Persona[] personaList);

    void SetPermissions(
      AclFeature feature,
      ExportServiceAclInfo[] exportServiceAclInfoList,
      string userid);

    void SetDefaultValue(
      AclFeature feature,
      string userid,
      ExportServiceAclInfo.ExportServicesDefaultSetting defaultValue);

    void SetPermissions(
      AclFeature feature,
      ExportServiceAclInfo[] exportServiceAclInfoList,
      int personaID);

    void SetDefaultValue(
      AclFeature feature,
      int personaID,
      ExportServiceAclInfo.ExportServicesDefaultSetting defaultValue);

    void DuplicateACLExportServices(
      int sourcePersonaID,
      int desPersonaID,
      ExportServiceAclInfo[] availableServices);
  }
}
