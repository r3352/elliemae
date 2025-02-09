// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Acl.LoConnectServiceAclManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.SessionObjects;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects.Acl;
using System;
using System.Collections;

#nullable disable
namespace Elli.Server.Remoting.Acl
{
  public class LoConnectServiceAclManager : SessionBoundObject, ILoConnectServiceAclManager
  {
    private const string className = "LoConnectServiceAclManager";
    private const string objKeyPrefix = "aclmanager#";

    public LoConnectServiceAclManager Initialize(ISession session)
    {
      this.InitializeInternal(session, "aclmanager#" + (object) 17);
      return this;
    }

    public virtual Hashtable GetPermissionsByPersona(
      LoConnectCustomServiceInfo[] services,
      int personaID)
    {
      this.onApiCalled(nameof (LoConnectServiceAclManager), nameof (GetPermissionsByPersona), new object[2]
      {
        (object) services,
        (object) personaID
      });
      try
      {
        return LoConnectCustomServicesAclDbAccessor.GetPermissions(services, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoConnectServiceAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual Hashtable GetPermissionsByPersona(
      LoConnectCustomServiceInfo service,
      int personaID)
    {
      this.onApiCalled(nameof (LoConnectServiceAclManager), nameof (GetPermissionsByPersona), new object[2]
      {
        (object) service,
        (object) personaID
      });
      try
      {
        return LoConnectCustomServicesAclDbAccessor.GetPermissions(service, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoConnectServiceAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual Hashtable GetPermissionsByPersonas(
      LoConnectCustomServiceInfo service,
      int[] personaIDs)
    {
      this.onApiCalled(nameof (LoConnectServiceAclManager), nameof (GetPermissionsByPersonas), new object[2]
      {
        (object) service,
        (object) personaIDs
      });
      try
      {
        return LoConnectCustomServicesAclDbAccessor.GetPermissions(service, personaIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoConnectServiceAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual Hashtable GetUserPermissionByPersonas(Persona[] personaIDs)
    {
      this.onApiCalled(nameof (LoConnectServiceAclManager), "GetUserPermissionsByPersonas", (object[]) personaIDs);
      try
      {
        return LoConnectCustomServicesAclDbAccessor.GetUserPermissionByPersonas(AclUtils.GetPersonaIDs(personaIDs));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoConnectServiceAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual Hashtable GetPermissionsByUser(LoConnectCustomServiceInfo service, string userid)
    {
      this.onApiCalled(nameof (LoConnectServiceAclManager), nameof (GetPermissionsByUser), new object[2]
      {
        (object) service,
        (object) userid
      });
      try
      {
        return LoConnectCustomServicesAclDbAccessor.GetPermissions(service, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoConnectServiceAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual Hashtable GetPermissionsByUser(
      LoConnectCustomServiceInfo[] services,
      string userid)
    {
      this.onApiCalled(nameof (LoConnectServiceAclManager), nameof (GetPermissionsByUser), new object[2]
      {
        (object) services,
        (object) userid
      });
      try
      {
        return LoConnectCustomServicesAclDbAccessor.GetPermissions(services, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoConnectServiceAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual Hashtable CheckPermission(LoConnectCustomServiceInfo service, UserInfo user)
    {
      this.onApiCalled(nameof (LoConnectServiceAclManager), nameof (CheckPermission), new object[2]
      {
        (object) service,
        (object) user
      });
      try
      {
        return LoConnectCustomServicesAclDbAccessor.CheckPermission(service, user);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoConnectServiceAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual Hashtable CheckPermission(LoConnectCustomServiceInfo[] services, UserInfo user)
    {
      this.onApiCalled(nameof (LoConnectServiceAclManager), nameof (CheckPermission), new object[2]
      {
        (object) services,
        (object) user
      });
      try
      {
        return LoConnectCustomServicesAclDbAccessor.CheckPermission(services, user);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoConnectServiceAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual void DeleteUserPermission(string userid)
    {
      this.onApiCalled(nameof (LoConnectServiceAclManager), nameof (DeleteUserPermission), new object[1]
      {
        (object) userid
      });
      try
      {
        LoConnectCustomServicesAclDbAccessor.DeleteUserPermission(userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoConnectServiceAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermissions(LoConnectCustomServiceInfo[] services, int personaID)
    {
      this.onApiCalled(nameof (LoConnectServiceAclManager), nameof (SetPermissions), new object[2]
      {
        (object) services,
        (object) personaID
      });
      try
      {
        LoConnectCustomServicesAclDbAccessor.SetPermission(services, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoConnectServiceAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermissions(LoConnectCustomServiceInfo[] services, string userid)
    {
      this.onApiCalled(nameof (LoConnectServiceAclManager), nameof (SetPermissions), new object[2]
      {
        (object) services,
        (object) userid
      });
      try
      {
        LoConnectCustomServicesAclDbAccessor.SetPermission(services, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoConnectServiceAclManager), ex, this.Session.SessionInfo);
      }
    }
  }
}
