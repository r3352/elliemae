// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Acl.ExportServicesAclManager
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
  public class ExportServicesAclManager : SessionBoundObject, IExportServicesAclManager
  {
    private const string className = "ExportServicesAclManager";
    private const string objKeyPrefix = "aclmanager#";

    public ExportServicesAclManager Initialize(ISession session)
    {
      this.InitializeInternal(session, "aclmanager#" + (object) 13);
      return this;
    }

    public virtual ExportServiceAclInfo[] GetPermissions(
      AclFeature feature,
      int personaID,
      ExportServiceAclInfo[] availableServices)
    {
      this.onApiCalled(nameof (ExportServicesAclManager), nameof (GetPermissions), new object[3]
      {
        (object) feature,
        (object) personaID,
        (object) availableServices
      });
      try
      {
        return ExportServicesAclDbAccessor.GetPermissions(feature, personaID, availableServices);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ExportServicesAclManager), ex, this.Session.SessionInfo);
        return (ExportServiceAclInfo[]) null;
      }
    }

    public virtual ExportServiceAclInfo[] GetPermissions(
      AclFeature feature,
      string userID,
      Persona[] personaList,
      ExportServiceAclInfo[] availableServices)
    {
      this.onApiCalled(nameof (ExportServicesAclManager), nameof (GetPermissions), new object[4]
      {
        (object) feature,
        (object) userID,
        (object) personaList,
        (object) availableServices
      });
      try
      {
        return ExportServicesAclDbAccessor.GetPermissions(feature, userID, AclUtils.GetPersonaIDs(personaList), availableServices);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ExportServicesAclManager), ex, this.Session.SessionInfo);
        return (ExportServiceAclInfo[]) null;
      }
    }

    public virtual void SetPermissions(
      AclFeature feature,
      ExportServiceAclInfo[] exportServiceAclInfoList,
      string userid)
    {
      this.onApiCalled(nameof (ExportServicesAclManager), nameof (SetPermissions), new object[3]
      {
        (object) feature,
        (object) exportServiceAclInfoList,
        (object) userid
      });
      try
      {
        ExportServicesAclDbAccessor.SetPermissions(feature, exportServiceAclInfoList, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ExportServicesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermissions(
      AclFeature feature,
      ExportServiceAclInfo[] exportServiceAclInfoList,
      int personaID)
    {
      this.onApiCalled(nameof (ExportServicesAclManager), nameof (SetPermissions), new object[3]
      {
        (object) feature,
        (object) exportServiceAclInfoList,
        (object) personaID
      });
      try
      {
        ExportServicesAclDbAccessor.SetPermissions(feature, exportServiceAclInfoList, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ExportServicesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DuplicateACLExportServices(
      int sourcePersonaID,
      int desPersonaID,
      ExportServiceAclInfo[] availableServices)
    {
      this.onApiCalled(nameof (ExportServicesAclManager), nameof (DuplicateACLExportServices), new object[3]
      {
        (object) sourcePersonaID,
        (object) desPersonaID,
        (object) availableServices
      });
      try
      {
        ExportServicesAclDbAccessor.DuplicateACLServices(sourcePersonaID, desPersonaID, availableServices);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ExportServicesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual ExportServiceAclInfo.ExportServicesDefaultSetting GetExportServicesDefaultSetting(
      AclFeature feature,
      int personaID)
    {
      this.onApiCalled(nameof (ExportServicesAclManager), nameof (GetExportServicesDefaultSetting), new object[2]
      {
        (object) feature,
        (object) personaID
      });
      try
      {
        return ExportServicesAclDbAccessor.GetServicesDefaultSetting(feature, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ExportServicesAclManager), ex, this.Session.SessionInfo);
        return ExportServiceAclInfo.ExportServicesDefaultSetting.All;
      }
    }

    public virtual ExportServiceAclInfo.ExportServicesDefaultSetting GetExportServicesDefaultSetting(
      AclFeature feature,
      string userID,
      Persona[] personaList)
    {
      this.onApiCalled(nameof (ExportServicesAclManager), nameof (GetExportServicesDefaultSetting), new object[3]
      {
        (object) feature,
        (object) userID,
        (object) personaList
      });
      try
      {
        return ExportServicesAclDbAccessor.GetServicesDefaultSetting(feature, userID, AclUtils.GetPersonaIDs(personaList));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ExportServicesAclManager), ex, this.Session.SessionInfo);
        return ExportServiceAclInfo.ExportServicesDefaultSetting.All;
      }
    }

    public virtual ExportServiceAclInfo.ExportServicesDefaultSetting GetUserSpecificExportServicesDefaultSetting(
      AclFeature feature,
      string userID,
      Persona[] personaList)
    {
      this.onApiCalled(nameof (ExportServicesAclManager), nameof (GetUserSpecificExportServicesDefaultSetting), new object[3]
      {
        (object) feature,
        (object) userID,
        (object) personaList
      });
      try
      {
        return ExportServicesAclDbAccessor.GetUserSpecificServicesDefaultSetting(feature, userID, AclUtils.GetPersonaIDs(personaList));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ExportServicesAclManager), ex, this.Session.SessionInfo);
        return ExportServiceAclInfo.ExportServicesDefaultSetting.All;
      }
    }

    public virtual ExportServiceAclInfo.ExportServicesDefaultSetting GetPersonasExportServicesDefaultSetting(
      AclFeature feature,
      string userID,
      Persona[] personaList)
    {
      this.onApiCalled(nameof (ExportServicesAclManager), nameof (GetPersonasExportServicesDefaultSetting), new object[3]
      {
        (object) feature,
        (object) userID,
        (object) personaList
      });
      try
      {
        return ExportServicesAclDbAccessor.GetPersonasServicesDefaultSetting(feature, userID, AclUtils.GetPersonaIDs(personaList));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ExportServicesAclManager), ex, this.Session.SessionInfo);
        return ExportServiceAclInfo.ExportServicesDefaultSetting.All;
      }
    }

    public virtual void SetDefaultValue(
      AclFeature feature,
      string userid,
      ExportServiceAclInfo.ExportServicesDefaultSetting defaultValue)
    {
      this.onApiCalled(nameof (ExportServicesAclManager), nameof (SetDefaultValue), new object[3]
      {
        (object) feature,
        (object) userid,
        (object) defaultValue
      });
      try
      {
        ExportServicesAclDbAccessor.SetDefaultValue(feature, userid, defaultValue);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ExportServicesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetDefaultValue(
      AclFeature feature,
      int personaID,
      ExportServiceAclInfo.ExportServicesDefaultSetting defaultValue)
    {
      this.onApiCalled(nameof (ExportServicesAclManager), nameof (SetDefaultValue), new object[3]
      {
        (object) feature,
        (object) personaID,
        (object) defaultValue
      });
      try
      {
        ExportServicesAclDbAccessor.SetDefaultValue(feature, personaID, defaultValue);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ExportServicesAclManager), ex, this.Session.SessionInfo);
      }
    }
  }
}
