// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.Acl.ExportServicesAclManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.RemotingServices.Acl
{
  public class ExportServicesAclManager
  {
    private IExportServicesAclManager exportServicesAclManager;
    private Sessions.Session session;
    private Dictionary<string, ExportServiceAclInfo> cachedInfos;

    public ExportServicesAclManager(Sessions.Session session)
    {
      this.session = session;
      this.exportServicesAclManager = (IExportServicesAclManager) this.session.GetAclManager(AclCategory.ExportServices);
    }

    public ExportServiceAclInfo[] GetPermissions(
      AclFeature feature,
      int personaID,
      ExportServiceAclInfo[] availableServices)
    {
      return this.exportServicesAclManager.GetPermissions(feature, personaID, availableServices);
    }

    public ExportServiceAclInfo.ExportServicesDefaultSetting GetExportServicesDefaultSetting(
      AclFeature feature,
      int personaID)
    {
      return this.exportServicesAclManager.GetExportServicesDefaultSetting(feature, personaID);
    }

    public ExportServiceAclInfo[] GetPermissions(
      AclFeature feature,
      string userID,
      Persona[] personaList,
      ExportServiceAclInfo[] availableServices)
    {
      return this.exportServicesAclManager.GetPermissions(feature, userID, personaList, availableServices);
    }

    public ExportServiceAclInfo.ExportServicesDefaultSetting GetExportServicesDefaultSetting(
      AclFeature feature,
      string userID,
      Persona[] personaList)
    {
      return this.exportServicesAclManager.GetExportServicesDefaultSetting(feature, userID, personaList);
    }

    public ExportServiceAclInfo.ExportServicesDefaultSetting GetUserSpecificExportServicesDefaultSetting(
      AclFeature feature,
      string userID,
      Persona[] personaList)
    {
      return this.exportServicesAclManager.GetUserSpecificExportServicesDefaultSetting(feature, userID, personaList);
    }

    public ExportServiceAclInfo.ExportServicesDefaultSetting GetPersonasExportServicesDefaultSetting(
      AclFeature feature,
      string userID,
      Persona[] personaList)
    {
      return this.exportServicesAclManager.GetPersonasExportServicesDefaultSetting(feature, userID, personaList);
    }

    public void SetPermissions(
      AclFeature feature,
      ExportServiceAclInfo[] exportServiceAclInfoList,
      string userid)
    {
      this.exportServicesAclManager.SetPermissions(feature, exportServiceAclInfoList, userid);
    }

    public void SetDefaultValue(
      AclFeature feature,
      string userid,
      ExportServiceAclInfo.ExportServicesDefaultSetting defaultValue)
    {
      this.exportServicesAclManager.SetDefaultValue(feature, userid, defaultValue);
    }

    public void SetPermissions(
      AclFeature feature,
      ExportServiceAclInfo[] exportServiceAclInfoList,
      int personaID)
    {
      this.exportServicesAclManager.SetPermissions(feature, exportServiceAclInfoList, personaID);
    }

    public void SetDefaultValue(
      AclFeature feature,
      int personaID,
      ExportServiceAclInfo.ExportServicesDefaultSetting defaultValue)
    {
      this.exportServicesAclManager.SetDefaultValue(feature, personaID, defaultValue);
    }

    public void DuplicateACLExportServices(
      int sourcePersonaID,
      int desPersonaID,
      ExportServiceAclInfo[] availableServices)
    {
      this.exportServicesAclManager.DuplicateACLExportServices(sourcePersonaID, desPersonaID, availableServices);
    }

    public bool GetUserApplicationRight(
      string category,
      List<ExportServiceAclInfo> availableServices)
    {
      foreach (ExportServiceAclInfo permission in this.exportServicesAclManager.GetPermissions(AclFeature.LoanMgmt_MgmtPipelineServices, this.session.UserID, this.session.UserInfo.UserPersonas, availableServices.ToArray()))
      {
        if (permission.ExportGroup == category && permission.PersonaAccess == AclResourceAccess.ReadWrite)
          return true;
      }
      return false;
    }

    public bool GetUserApplicationRightForPipelineServices(string category)
    {
      if (this.cachedInfos == null)
        this.cachedInfos = ((IEnumerable<ExportServiceAclInfo>) this.exportServicesAclManager.GetPermissions(AclFeature.LoanMgmt_MgmtPipelineServices, this.session.UserID, this.session.UserInfo.UserPersonas, ExportServiceAclInfo.GetExportServicesList(ServicesMapping.Categories.ToArray(), 229).ToArray())).ToDictionary<ExportServiceAclInfo, string>((Func<ExportServiceAclInfo, string>) (info => info.ExportGroup));
      return !this.cachedInfos.ContainsKey(category) || this.cachedInfos[category].CustomAccess != AclResourceAccess.None ? this.cachedInfos[category].CustomAccess == AclResourceAccess.ReadWrite : this.cachedInfos[category].PersonaAccess == AclResourceAccess.ReadWrite;
    }
  }
}
