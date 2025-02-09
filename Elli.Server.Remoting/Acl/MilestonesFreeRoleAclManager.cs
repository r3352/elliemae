// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Acl.MilestonesFreeRoleAclManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.SessionObjects;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Acl.Interfaces;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects.Acl;
using System;
using System.Collections;

#nullable disable
namespace Elli.Server.Remoting.Acl
{
  public class MilestonesFreeRoleAclManager : SessionBoundObject, IMilestonesFreeRoleAclManager
  {
    private const string className = "MilestonesFreeRoleAclManager";
    private const string objKeyPrefix = "aclmanager#";

    public MilestonesFreeRoleAclManager Initialize(ISession session)
    {
      this.InitializeInternal(session, "aclmanager#" + (object) 8);
      return this;
    }

    public virtual Hashtable GetPermissions(int personaID)
    {
      this.onApiCalled(nameof (MilestonesFreeRoleAclManager), nameof (GetPermissions), new object[1]
      {
        (object) personaID
      });
      try
      {
        return MilestoneFreeRoleAclDbAccessor.GetPermissions(-1, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesFreeRoleAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual bool GetPermission(int roleID, int personaID)
    {
      this.onApiCalled(nameof (MilestonesFreeRoleAclManager), nameof (GetPermission), new object[1]
      {
        (object) roleID
      });
      try
      {
        return MilestoneFreeRoleAclDbAccessor.GetPermission(roleID, new int[1]
        {
          personaID
        });
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesFreeRoleAclManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool GetPermission(int roleID, Persona[] personaList)
    {
      this.onApiCalled(nameof (MilestonesFreeRoleAclManager), nameof (GetPermission), new object[2]
      {
        (object) roleID,
        (object) personaList
      });
      try
      {
        return MilestoneFreeRoleAclDbAccessor.GetPermission(roleID, AclUtils.GetPersonaIDs(personaList));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesFreeRoleAclManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool GetPermission(int roleID, UserInfo userInfo)
    {
      this.onApiCalled(nameof (MilestonesFreeRoleAclManager), nameof (GetPermission), new object[2]
      {
        (object) roleID,
        (object) userInfo
      });
      try
      {
        return MilestoneFreeRoleAclDbAccessor.GetPermission(roleID, userInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesFreeRoleAclManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual Hashtable GetPersonalPermissions(UserInfo userInfo)
    {
      this.onApiCalled(nameof (MilestonesFreeRoleAclManager), "GetPersonalPermission", new object[1]
      {
        (object) userInfo
      });
      try
      {
        return MilestoneFreeRoleAclDbAccessor.GetPersonalPermission(-1, AclUtils.GetPersonaIDs(userInfo.UserPersonas), userInfo.Userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesFreeRoleAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual void SetPermission(int roleID, string userid, object access)
    {
      this.onApiCalled(nameof (MilestonesFreeRoleAclManager), nameof (SetPermission), new object[3]
      {
        (object) roleID,
        (object) userid,
        access
      });
      try
      {
        MilestoneFreeRoleAclDbAccessor.SetPermission(roleID, userid, access);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesFreeRoleAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermission(int roleID, int personaID, bool access)
    {
      this.onApiCalled(nameof (MilestonesFreeRoleAclManager), nameof (SetPermission), new object[3]
      {
        (object) roleID,
        (object) personaID,
        (object) access
      });
      try
      {
        MilestoneFreeRoleAclDbAccessor.SetPermission(roleID, personaID, access);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesFreeRoleAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DuplicateACLMilestonesFreeRole(int sourcePersonaID, int desPersonaID)
    {
      this.onApiCalled(nameof (MilestonesFreeRoleAclManager), "DuplicateACLMilestones", new object[2]
      {
        (object) sourcePersonaID,
        (object) desPersonaID
      });
      try
      {
        MilestoneFreeRoleAclDbAccessor.DuplicateACLMilestonesFreeRole(sourcePersonaID, desPersonaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesFreeRoleAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SynchronizeAdminRight()
    {
      this.onApiCalled(nameof (MilestonesFreeRoleAclManager), nameof (SynchronizeAdminRight), Array.Empty<object>());
      try
      {
        MilestoneFreeRoleAclDbAccessor.MilestoneFreeRoleCleanUp();
        MilestoneFreeRoleAclDbAccessor.SynchronizeAdminSetting();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesFreeRoleAclManager), ex, this.Session.SessionInfo);
      }
    }
  }
}
