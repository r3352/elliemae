// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Acl.StandardWebFormAclManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.SessionObjects;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects.Acl;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Elli.Server.Remoting.Acl
{
  public class StandardWebFormAclManager : SessionBoundObject, IStandardWebFormAclManager
  {
    private const string className = "StandardWebFormAclManager";
    private const string objKeyPrefix = "aclmanager#";

    public StandardWebFormAclManager Initialize(ISession session)
    {
      this.InitializeInternal(session, "aclmanager#" + (object) 19);
      return this;
    }

    public virtual Hashtable GetPermissionsByPersonas(int[] personaIDs)
    {
      this.onApiCalled(nameof (StandardWebFormAclManager), "GetPermissionsByPersona", new object[1]
      {
        (object) personaIDs
      });
      try
      {
        return StandardWebFormsAclDbAccessor.GetPermissionsByPersonas(personaIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (StandardWebFormAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual Hashtable GetPermissionsByUser(string userid)
    {
      this.onApiCalled(nameof (StandardWebFormAclManager), nameof (GetPermissionsByUser), new object[1]
      {
        (object) userid
      });
      try
      {
        return StandardWebFormsAclDbAccessor.GetPermissionsByUser(userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (StandardWebFormAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual List<StandardWebFormInfo> GetFormsByPersona(int personaID)
    {
      this.onApiCalled(nameof (StandardWebFormAclManager), nameof (GetFormsByPersona), new object[1]
      {
        (object) personaID
      });
      try
      {
        return StandardWebFormsAclDbAccessor.GetFormsByPersona(personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (StandardWebFormAclManager), ex, this.Session.SessionInfo);
        return (List<StandardWebFormInfo>) null;
      }
    }

    public virtual List<StandardWebFormInfo> GetActiveForms()
    {
      this.onApiCalled(nameof (StandardWebFormAclManager), nameof (GetActiveForms), Array.Empty<object>());
      try
      {
        return StandardWebFormsAclDbAccessor.GetActiveForms();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (StandardWebFormAclManager), ex, this.Session.SessionInfo);
        return (List<StandardWebFormInfo>) null;
      }
    }

    public virtual List<StandardWebFormInfo> GetFormsByUser(string userid)
    {
      this.onApiCalled(nameof (StandardWebFormAclManager), nameof (GetFormsByUser), new object[1]
      {
        (object) userid
      });
      try
      {
        return StandardWebFormsAclDbAccessor.GetFormsByUser(userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (StandardWebFormAclManager), ex, this.Session.SessionInfo);
        return (List<StandardWebFormInfo>) null;
      }
    }

    public virtual void SetPermissions(StandardWebFormInfo[] services, int personaID)
    {
      this.onApiCalled(nameof (StandardWebFormAclManager), nameof (SetPermissions), new object[2]
      {
        (object) services,
        (object) personaID
      });
      try
      {
        StandardWebFormsAclDbAccessor.SetPermissions(services, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (StandardWebFormAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermissions(StandardWebFormInfo[] services, string userid)
    {
      this.onApiCalled(nameof (StandardWebFormAclManager), nameof (SetPermissions), new object[2]
      {
        (object) services,
        (object) userid
      });
      try
      {
        StandardWebFormsAclDbAccessor.SetPermissions(services, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (StandardWebFormAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DuplicateACLStandardWebForms(
      int sourcePersonaID,
      int desPersonaID,
      string userid)
    {
      this.onApiCalled(nameof (StandardWebFormAclManager), nameof (DuplicateACLStandardWebForms), new object[2]
      {
        (object) sourcePersonaID,
        (object) desPersonaID
      });
      try
      {
        StandardWebFormsAclDbAccessor.DuplicateACLStandardWebForms(sourcePersonaID, desPersonaID, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (StandardWebFormAclManager), ex, this.Session.SessionInfo);
      }
    }
  }
}
