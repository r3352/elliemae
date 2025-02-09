// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Acl.InvestorServicesAclManager
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
  public class InvestorServicesAclManager : SessionBoundObject, IInvestorServicesAclManager
  {
    private const string className = "InvestorServicesAclManager";
    private const string objKeyPrefix = "aclmanager#";

    public InvestorServicesAclManager Initialize(ISession session)
    {
      this.InitializeInternal(session, "aclmanager#" + (object) 15);
      return this;
    }

    public virtual InvestorServiceAclInfo[] GetPermissions(
      AclFeature feature,
      int personaID,
      string category,
      InvestorServiceAclInfo[] availableServices)
    {
      this.onApiCalled(nameof (InvestorServicesAclManager), nameof (GetPermissions), new object[4]
      {
        (object) feature,
        (object) personaID,
        (object) category,
        (object) availableServices
      });
      try
      {
        return InvestorServicesAclDbAccessor.GetPermissions(feature, personaID, category, availableServices);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InvestorServicesAclManager), ex, this.Session.SessionInfo);
        return (InvestorServiceAclInfo[]) null;
      }
    }

    public virtual InvestorServiceAclInfo[] GetPermissions(
      AclFeature feature,
      string userID,
      string category,
      Persona[] personaList,
      InvestorServiceAclInfo[] availableServices)
    {
      this.onApiCalled(nameof (InvestorServicesAclManager), nameof (GetPermissions), new object[5]
      {
        (object) feature,
        (object) userID,
        (object) category,
        (object) personaList,
        (object) availableServices
      });
      try
      {
        return InvestorServicesAclDbAccessor.GetPermissions(feature, userID, category, AclUtils.GetPersonaIDs(personaList), availableServices);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InvestorServicesAclManager), ex, this.Session.SessionInfo);
        return (InvestorServiceAclInfo[]) null;
      }
    }

    public virtual InvestorServiceAclInfo[] GetPermissions(
      AclFeature[] features,
      string userID,
      Persona[] personaList,
      InvestorServiceAclInfo[] availableServices)
    {
      this.onApiCalled(nameof (InvestorServicesAclManager), nameof (GetPermissions), new object[4]
      {
        (object) features,
        (object) userID,
        (object) personaList,
        (object) availableServices
      });
      try
      {
        return InvestorServicesAclDbAccessor.GetPermissions(features, userID, AclUtils.GetPersonaIDs(personaList), availableServices);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InvestorServicesAclManager), ex, this.Session.SessionInfo);
        return (InvestorServiceAclInfo[]) null;
      }
    }

    public virtual void SetPermissions(
      AclFeature feature,
      InvestorServiceAclInfo[] InvestorServiceAclInfoList,
      string userid,
      string category,
      InvestorServiceAclInfo.InvestorServicesDefaultSetting defaultValue)
    {
      this.onApiCalled(nameof (InvestorServicesAclManager), nameof (SetPermissions), new object[5]
      {
        (object) feature,
        (object) InvestorServiceAclInfoList,
        (object) userid,
        (object) category,
        (object) defaultValue
      });
      try
      {
        InvestorServicesAclDbAccessor.SetPermissions(feature, InvestorServiceAclInfoList, userid, category, defaultValue);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InvestorServicesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermissions(
      AclFeature feature,
      InvestorServiceAclInfo[] InvestorServiceAclInfoList,
      int personaID,
      string category,
      InvestorServiceAclInfo.InvestorServicesDefaultSetting defaultValue)
    {
      this.onApiCalled(nameof (InvestorServicesAclManager), nameof (SetPermissions), new object[5]
      {
        (object) feature,
        (object) InvestorServiceAclInfoList,
        (object) personaID,
        (object) category,
        (object) defaultValue
      });
      try
      {
        InvestorServicesAclDbAccessor.SetPermissions(feature, InvestorServiceAclInfoList, personaID, category, defaultValue);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InvestorServicesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DuplicateACLInvestorServices(
      AclFeature feature,
      int sourcePersonaID,
      int desPersonaID,
      string category,
      InvestorServiceAclInfo[] availableServices)
    {
      this.onApiCalled(nameof (InvestorServicesAclManager), nameof (DuplicateACLInvestorServices), new object[5]
      {
        (object) (int) feature,
        (object) sourcePersonaID,
        (object) desPersonaID,
        (object) category,
        (object) availableServices
      });
      try
      {
        InvestorServicesAclDbAccessor.DuplicateACLServices(feature, sourcePersonaID, desPersonaID, category, availableServices);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InvestorServicesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual InvestorServiceAclInfo.InvestorServicesDefaultSetting GetInvestorServicesDefaultSetting(
      AclFeature feature,
      int personaID,
      string category)
    {
      this.onApiCalled(nameof (InvestorServicesAclManager), nameof (GetInvestorServicesDefaultSetting), new object[3]
      {
        (object) feature,
        (object) personaID,
        (object) category
      });
      try
      {
        return InvestorServicesAclDbAccessor.GetServicesDefaultSetting(feature, personaID, category);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InvestorServicesAclManager), ex, this.Session.SessionInfo);
        return InvestorServiceAclInfo.InvestorServicesDefaultSetting.All;
      }
    }

    public virtual InvestorServiceAclInfo.InvestorServicesDefaultSetting GetInvestorServicesDefaultSetting(
      AclFeature feature,
      string userID,
      string category,
      Persona[] personaList)
    {
      this.onApiCalled(nameof (InvestorServicesAclManager), nameof (GetInvestorServicesDefaultSetting), new object[4]
      {
        (object) feature,
        (object) userID,
        (object) category,
        (object) personaList
      });
      try
      {
        return InvestorServicesAclDbAccessor.GetServicesDefaultSetting(feature, userID, category, AclUtils.GetPersonaIDs(personaList));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InvestorServicesAclManager), ex, this.Session.SessionInfo);
        return InvestorServiceAclInfo.InvestorServicesDefaultSetting.All;
      }
    }

    public virtual InvestorServiceAclInfo.InvestorServicesDefaultSetting GetUserSpecificInvestorServicesDefaultSetting(
      AclFeature feature,
      string userID,
      string category,
      Persona[] personaList)
    {
      this.onApiCalled(nameof (InvestorServicesAclManager), nameof (GetUserSpecificInvestorServicesDefaultSetting), new object[4]
      {
        (object) feature,
        (object) userID,
        (object) category,
        (object) personaList
      });
      try
      {
        return InvestorServicesAclDbAccessor.GetUserSpecificServicesDefaultSetting(feature, userID, category, AclUtils.GetPersonaIDs(personaList));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InvestorServicesAclManager), ex, this.Session.SessionInfo);
        return InvestorServiceAclInfo.InvestorServicesDefaultSetting.All;
      }
    }

    public virtual InvestorServiceAclInfo.InvestorServicesDefaultSetting GetPersonasInvestorServicesDefaultSetting(
      AclFeature feature,
      string userID,
      string category,
      Persona[] personaList)
    {
      this.onApiCalled(nameof (InvestorServicesAclManager), nameof (GetPersonasInvestorServicesDefaultSetting), new object[4]
      {
        (object) feature,
        (object) userID,
        (object) category,
        (object) personaList
      });
      try
      {
        return InvestorServicesAclDbAccessor.GetPersonasServicesDefaultSetting(feature, userID, category, AclUtils.GetPersonaIDs(personaList));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InvestorServicesAclManager), ex, this.Session.SessionInfo);
        return InvestorServiceAclInfo.InvestorServicesDefaultSetting.All;
      }
    }

    public virtual void SetDefaultValue(
      AclFeature feature,
      string userid,
      string category,
      InvestorServiceAclInfo.InvestorServicesDefaultSetting defaultValue)
    {
      this.onApiCalled(nameof (InvestorServicesAclManager), nameof (SetDefaultValue), new object[4]
      {
        (object) feature,
        (object) userid,
        (object) category,
        (object) defaultValue
      });
      try
      {
        InvestorServicesAclDbAccessor.SetDefaultValue(feature, userid, category, defaultValue);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InvestorServicesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetDefaultValue(
      AclFeature feature,
      int personaID,
      string category,
      InvestorServiceAclInfo.InvestorServicesDefaultSetting defaultValue)
    {
      this.onApiCalled(nameof (InvestorServicesAclManager), nameof (SetDefaultValue), new object[4]
      {
        (object) feature,
        (object) personaID,
        (object) category,
        (object) defaultValue
      });
      try
      {
        InvestorServicesAclDbAccessor.SetDefaultValue(feature, personaID, category, defaultValue);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InvestorServicesAclManager), ex, this.Session.SessionInfo);
      }
    }
  }
}
