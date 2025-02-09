// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Acl.ServicesAclManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.SessionObjects;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects.Acl;
using System;

#nullable disable
namespace Elli.Server.Remoting.Acl
{
  public class ServicesAclManager : SessionBoundObject, IServicesAclManager
  {
    private const string className = "ServicesAclManager";
    private const string objKeyPrefix = "aclmanager#";

    public ServicesAclManager Initialize(ISession session)
    {
      this.InitializeInternal(session, "aclmanager#" + (object) 9);
      return this;
    }

    public virtual ServiceAclInfo[] GetPermissions(AclFeature feature, int personaID)
    {
      this.onApiCalled(nameof (ServicesAclManager), nameof (GetPermissions), new object[2]
      {
        (object) feature,
        (object) personaID
      });
      try
      {
        return ServicesAclDbAccessor.GetPermissions(feature, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServicesAclManager), ex, this.Session.SessionInfo);
        return (ServiceAclInfo[]) null;
      }
    }

    public virtual ServiceAclInfo[] GetPermissions(
      AclFeature feature,
      string userID,
      Persona[] personaList)
    {
      this.onApiCalled(nameof (ServicesAclManager), nameof (GetPermissions), new object[3]
      {
        (object) feature,
        (object) userID,
        (object) personaList
      });
      try
      {
        return ServicesAclDbAccessor.GetPermissions(feature, userID, AclUtils.GetPersonaIDs(personaList));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServicesAclManager), ex, this.Session.SessionInfo);
        return (ServiceAclInfo[]) null;
      }
    }

    public virtual void SetPermissions(
      AclFeature feature,
      ServiceAclInfo[] serviceAclInfoList,
      string userid)
    {
      this.onApiCalled(nameof (ServicesAclManager), nameof (SetPermissions), new object[3]
      {
        (object) feature,
        (object) serviceAclInfoList,
        (object) userid
      });
      try
      {
        ServicesAclDbAccessor.SetPermissions(feature, serviceAclInfoList, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServicesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermissions(
      AclFeature feature,
      ServiceAclInfo[] serviceAclInfoList,
      int personaID)
    {
      this.onApiCalled(nameof (ServicesAclManager), nameof (SetPermissions), new object[3]
      {
        (object) feature,
        (object) serviceAclInfoList,
        (object) personaID
      });
      try
      {
        ServicesAclDbAccessor.SetPermissions(feature, serviceAclInfoList, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServicesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DuplicateACLServices(int sourcePersonaID, int desPersonaID)
    {
      this.onApiCalled(nameof (ServicesAclManager), nameof (DuplicateACLServices), new object[2]
      {
        (object) sourcePersonaID,
        (object) desPersonaID
      });
      try
      {
        ServicesAclDbAccessor.DuplicateACLServices(sourcePersonaID, desPersonaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServicesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ServiceAclInfo.ServicesDefaultSetting GetServicesDefaultSetting(
      AclFeature feature,
      int personaID)
    {
      this.onApiCalled(nameof (ServicesAclManager), nameof (GetServicesDefaultSetting), new object[2]
      {
        (object) feature,
        (object) personaID
      });
      try
      {
        return ServicesAclDbAccessor.GetServicesDefaultSetting(feature, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServicesAclManager), ex, this.Session.SessionInfo);
        return ServiceAclInfo.ServicesDefaultSetting.All;
      }
    }

    public virtual ServiceAclInfo.ServicesDefaultSetting GetServicesDefaultSetting(
      AclFeature feature,
      string userID,
      Persona[] personaList)
    {
      this.onApiCalled(nameof (ServicesAclManager), nameof (GetServicesDefaultSetting), new object[3]
      {
        (object) feature,
        (object) userID,
        (object) personaList
      });
      try
      {
        return ServicesAclDbAccessor.GetServicesDefaultSetting(feature, userID, AclUtils.GetPersonaIDs(personaList));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServicesAclManager), ex, this.Session.SessionInfo);
        return ServiceAclInfo.ServicesDefaultSetting.All;
      }
    }

    public virtual ServiceAclInfo.ServicesDefaultSetting GetUserSpecificServicesDefaultSetting(
      AclFeature feature,
      string userID,
      Persona[] personaList)
    {
      this.onApiCalled(nameof (ServicesAclManager), nameof (GetUserSpecificServicesDefaultSetting), new object[3]
      {
        (object) feature,
        (object) userID,
        (object) personaList
      });
      try
      {
        return ServicesAclDbAccessor.GetUserSpecificServicesDefaultSetting(feature, userID, AclUtils.GetPersonaIDs(personaList));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServicesAclManager), ex, this.Session.SessionInfo);
        return ServiceAclInfo.ServicesDefaultSetting.All;
      }
    }

    public virtual ServiceAclInfo.ServicesDefaultSetting GetPersonasServicesDefaultSetting(
      AclFeature feature,
      string userID,
      Persona[] personaList)
    {
      this.onApiCalled(nameof (ServicesAclManager), nameof (GetPersonasServicesDefaultSetting), new object[3]
      {
        (object) feature,
        (object) userID,
        (object) personaList
      });
      try
      {
        return ServicesAclDbAccessor.GetPersonasServicesDefaultSetting(feature, userID, AclUtils.GetPersonaIDs(personaList));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServicesAclManager), ex, this.Session.SessionInfo);
        return ServiceAclInfo.ServicesDefaultSetting.All;
      }
    }

    public virtual void SetDefaultValue(
      AclFeature feature,
      string userid,
      ServiceAclInfo.ServicesDefaultSetting defaultValue)
    {
      this.onApiCalled(nameof (ServicesAclManager), nameof (SetDefaultValue), new object[3]
      {
        (object) feature,
        (object) userid,
        (object) defaultValue
      });
      try
      {
        ServicesAclDbAccessor.SetDefaultValue(feature, userid, defaultValue);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServicesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetDefaultValue(
      AclFeature feature,
      int personaID,
      ServiceAclInfo.ServicesDefaultSetting defaultValue)
    {
      this.onApiCalled(nameof (ServicesAclManager), nameof (SetDefaultValue), new object[3]
      {
        (object) feature,
        (object) personaID,
        (object) defaultValue
      });
      try
      {
        ServicesAclDbAccessor.SetDefaultValue(feature, personaID, defaultValue);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ServicesAclManager), ex, this.Session.SessionInfo);
      }
    }
  }
}
