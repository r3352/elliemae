// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.Acl.ServicesAclManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices.Acl
{
  public class ServicesAclManager
  {
    private IServicesAclManager servicesAclManager;
    private Sessions.Session session;

    public ServicesAclManager(Sessions.Session session)
    {
      this.session = session;
      this.servicesAclManager = (IServicesAclManager) this.session.GetAclManager(AclCategory.Services);
    }

    public ServiceAclInfo[] GetPermissions(AclFeature feature, int personaID)
    {
      return this.servicesAclManager.GetPermissions(feature, personaID);
    }

    public ServiceAclInfo[] GetPermissions(
      AclFeature feature,
      string userID,
      Persona[] personaList)
    {
      return this.servicesAclManager.GetPermissions(feature, userID, personaList);
    }

    public bool HasServicePermission(string serviceTitle)
    {
      ServiceAclInfo[] permissions = this.servicesAclManager.GetPermissions(AclFeature.LoanTab_Other_ePASS, this.session.UserID, this.session.UserInfo.UserPersonas);
      bool flag = true;
      foreach (ServiceAclInfo serviceAclInfo in permissions)
      {
        if (string.Compare(serviceAclInfo.ServiceTitle, serviceTitle, StringComparison.OrdinalIgnoreCase) == 0)
          flag = serviceAclInfo.Access;
      }
      return flag;
    }

    public string[] GetServices(bool hasPermission)
    {
      ServiceAclInfo[] permissions = this.servicesAclManager.GetPermissions(AclFeature.LoanTab_Other_ePASS, this.session.UserID, this.session.UserInfo.UserPersonas);
      List<string> stringList = new List<string>();
      foreach (ServiceAclInfo serviceAclInfo in permissions)
      {
        if (serviceAclInfo.Access == hasPermission)
          stringList.Add(serviceAclInfo.ServiceTitle);
      }
      return stringList.ToArray();
    }

    public void SetPermissions(
      AclFeature feature,
      ServiceAclInfo[] serviceAclInfoList,
      string userid)
    {
      this.servicesAclManager.SetPermissions(feature, serviceAclInfoList, userid);
    }

    public void SetPermissions(
      AclFeature feature,
      ServiceAclInfo[] serviceAclInfoList,
      int personaID)
    {
      this.servicesAclManager.SetPermissions(feature, serviceAclInfoList, personaID);
    }

    public void DuplicateACLServices(int sourcePersonaID, int desPersonaID)
    {
      this.servicesAclManager.DuplicateACLServices(sourcePersonaID, desPersonaID);
    }

    public ServiceAclInfo.ServicesDefaultSetting GetServicesDefaultSetting(
      AclFeature feature,
      int personaID)
    {
      return this.servicesAclManager.GetServicesDefaultSetting(feature, personaID);
    }

    public ServiceAclInfo.ServicesDefaultSetting GetServicesDefaultSetting(
      AclFeature feature,
      string userID,
      Persona[] personaList)
    {
      return this.servicesAclManager.GetServicesDefaultSetting(feature, userID, personaList);
    }

    public ServiceAclInfo.ServicesDefaultSetting GetServicesDefaultSetting()
    {
      return this.servicesAclManager.GetServicesDefaultSetting(AclFeature.LoanTab_Other_ePASS, this.session.UserID, this.session.UserInfo.UserPersonas);
    }

    public ServiceAclInfo.ServicesDefaultSetting GetUserSpecificServicesDefaultSetting(
      AclFeature feature,
      string userID,
      Persona[] personaList)
    {
      return this.servicesAclManager.GetUserSpecificServicesDefaultSetting(feature, userID, personaList);
    }

    public ServiceAclInfo.ServicesDefaultSetting GetPersonasServicesDefaultSetting(
      AclFeature feature,
      string userID,
      Persona[] personaList)
    {
      return this.servicesAclManager.GetPersonasServicesDefaultSetting(feature, userID, personaList);
    }

    public void SetDefaultValue(
      AclFeature feature,
      string userid,
      ServiceAclInfo.ServicesDefaultSetting defaultValue)
    {
      this.servicesAclManager.SetDefaultValue(feature, userid, defaultValue);
    }

    public void SetDefaultValue(
      AclFeature feature,
      int personaID,
      ServiceAclInfo.ServicesDefaultSetting defaultValue)
    {
      this.servicesAclManager.SetDefaultValue(feature, personaID, defaultValue);
    }
  }
}
