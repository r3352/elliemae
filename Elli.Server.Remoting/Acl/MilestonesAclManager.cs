// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Acl.MilestonesAclManager
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
using System.Collections.Generic;

#nullable disable
namespace Elli.Server.Remoting.Acl
{
  public class MilestonesAclManager : SessionBoundObject, IMilestonesAclManager
  {
    private const string className = "MilestonesAclManager";
    private const string objKeyPrefix = "aclmanager#";

    public MilestonesAclManager Initialize(ISession session)
    {
      this.InitializeInternal(session, "aclmanager#" + (object) 5);
      return this;
    }

    public virtual AclTriState GetPermission(
      AclMilestone feature,
      string customMilestoneId,
      string userid)
    {
      this.onApiCalled(nameof (MilestonesAclManager), nameof (GetPermission), new object[3]
      {
        (object) feature,
        (object) customMilestoneId,
        (object) userid
      });
      try
      {
        return MilestonesAclDbAccessor.GetPermission(feature, customMilestoneId, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesAclManager), ex, this.Session.SessionInfo);
        return AclTriState.Unspecified;
      }
    }

    public virtual Dictionary<string, AclTriState> CheckApplicationPermissions(
      AclMilestone feature,
      string[] customMilestoneIDs,
      UserInfo user)
    {
      this.onApiCalled(nameof (MilestonesAclManager), nameof (CheckApplicationPermissions), new object[3]
      {
        (object) feature,
        (object) customMilestoneIDs,
        (object) user
      });
      try
      {
        return MilestonesAclDbAccessor.CheckApplicationPermissions(feature, customMilestoneIDs, user);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesAclManager), ex, this.Session.SessionInfo);
        Dictionary<string, AclTriState> dictionary = new Dictionary<string, AclTriState>();
        foreach (string customMilestoneId in customMilestoneIDs)
          dictionary.Add(customMilestoneId, AclTriState.False);
        return dictionary;
      }
    }

    public virtual bool GetPermission(
      AclMilestone feature,
      string customMilestoneId,
      int personaID)
    {
      this.onApiCalled(nameof (MilestonesAclManager), nameof (GetPermission), new object[3]
      {
        (object) feature,
        (object) customMilestoneId,
        (object) personaID
      });
      try
      {
        return MilestonesAclDbAccessor.GetPermission(feature, customMilestoneId, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesAclManager), ex, this.Session.SessionInfo);
        throw ex;
      }
    }

    public virtual Hashtable GetPermissions(
      AclMilestone feature,
      string customMilestoneId,
      int[] personaIDs)
    {
      this.onApiCalled(nameof (MilestonesAclManager), nameof (GetPermissions), new object[3]
      {
        (object) feature,
        (object) customMilestoneId,
        (object) personaIDs
      });
      try
      {
        return MilestonesAclDbAccessor.GetPermissions(feature, customMilestoneId, personaIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual Hashtable GetPermissions(
      AclMilestone feature,
      string customMilestoneId,
      Persona[] personas)
    {
      return this.GetPermissions(feature, customMilestoneId, AclUtils.GetPersonaIDs(personas));
    }

    public virtual Dictionary<string, Hashtable> GetPermissions(
      List<EllieMae.EMLite.Workflow.Milestone> milestones,
      AclMilestone[] features,
      UserInfo currentuser)
    {
      this.onApiCalled(nameof (MilestonesAclManager), nameof (GetPermissions), new object[2]
      {
        (object) features,
        (object) currentuser
      });
      try
      {
        return MilestonesAclDbAccessor.GetPermissions(milestones, features, currentuser);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesAclManager), ex, this.Session.SessionInfo);
        return (Dictionary<string, Hashtable>) null;
      }
    }

    public virtual void SetPermission(
      AclMilestone feature,
      string customMilestoneId,
      string userid,
      object access)
    {
      this.onApiCalled(nameof (MilestonesAclManager), nameof (SetPermission), new object[4]
      {
        (object) feature,
        (object) customMilestoneId,
        (object) userid,
        access
      });
      try
      {
        MilestonesAclDbAccessor.SetPermission(feature, customMilestoneId, userid, access);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermission(
      AclMilestone feature,
      string customMilestoneId,
      int personaID,
      bool access)
    {
      this.onApiCalled(nameof (MilestonesAclManager), nameof (SetPermission), new object[4]
      {
        (object) feature,
        (object) customMilestoneId,
        (object) personaID,
        (object) access
      });
      try
      {
        MilestonesAclDbAccessor.SetPermission(feature, customMilestoneId, personaID, access);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteUserSpecificSetting(string userID)
    {
      this.onApiCalled(nameof (MilestonesAclManager), nameof (DeleteUserSpecificSetting), new object[1]
      {
        (object) userID
      });
      try
      {
        MilestonesAclDbAccessor.DeleteUserSpecificSetting(userID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual bool CheckPermission(
      AclMilestone feature,
      string customMilestoneId,
      UserInfo userInfo)
    {
      this.onApiCalled(nameof (MilestonesAclManager), nameof (CheckPermission), new object[3]
      {
        (object) feature,
        (object) customMilestoneId,
        (object) userInfo
      });
      try
      {
        return MilestonesAclDbAccessor.CheckPermission(feature, customMilestoneId, userInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesAclManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual Hashtable CheckPermissions(AclMilestone[] features, string milestoneStage)
    {
      this.onApiCalled(nameof (MilestonesAclManager), nameof (CheckPermissions), new object[2]
      {
        (object) features,
        (object) milestoneStage
      });
      try
      {
        UserInfo userInfo = this.Session.GetUserInfo();
        return MilestonesAclDbAccessor.CheckPermissions(features, milestoneStage, userInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual Hashtable CheckPermissions(
      AclMilestone[] features,
      string milestoneStage,
      string userID)
    {
      this.onApiCalled(nameof (MilestonesAclManager), nameof (CheckPermissions), new object[2]
      {
        (object) features,
        (object) milestoneStage
      });
      try
      {
        UserInfo userById = User.GetUserById(userID);
        return MilestonesAclDbAccessor.CheckPermissions(features, milestoneStage, userById);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual void DuplicateACLMilestones(int sourcePersonaID, int desPersonaID)
    {
      this.onApiCalled(nameof (MilestonesAclManager), nameof (DuplicateACLMilestones), new object[2]
      {
        (object) sourcePersonaID,
        (object) desPersonaID
      });
      try
      {
        MilestonesAclDbAccessor.DuplicateACLMilestones(sourcePersonaID, desPersonaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual Hashtable GetPersonalPermission(AclMilestone[] features, string userId)
    {
      this.onApiCalled(nameof (MilestonesAclManager), nameof (GetPersonalPermission), new object[2]
      {
        (object) features,
        (object) userId
      });
      try
      {
        return MilestonesAclDbAccessor.GetPersonalPermission(features, userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual void SynchronizeAdminRight()
    {
      this.onApiCalled(nameof (MilestonesAclManager), nameof (SynchronizeAdminRight), Array.Empty<object>());
      try
      {
        MilestonesAclDbAccessor.SynchronizeAdminSetting();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SynchronizePersonaSettingWithNewMilestone(int personaID, bool defaultAccess)
    {
      this.onApiCalled(nameof (MilestonesAclManager), nameof (SynchronizePersonaSettingWithNewMilestone), Array.Empty<object>());
      try
      {
        MilestonesAclDbAccessor.SynchronizePersonaSettingWithNewMilestone(personaID, defaultAccess);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SynchronizeBrokerSetting(string baseMilestoneID, string currentMilestoneID)
    {
      this.onApiCalled(nameof (MilestonesAclManager), nameof (SynchronizeBrokerSetting), new object[2]
      {
        (object) baseMilestoneID,
        (object) currentMilestoneID
      });
      try
      {
        MilestonesAclDbAccessor.SynchronizeBrokerSetting(baseMilestoneID, currentMilestoneID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual Dictionary<string, bool> GetPermissionsForCustomMilestone(
      AclMilestone feature,
      int personaID)
    {
      this.onApiCalled(nameof (MilestonesAclManager), nameof (GetPermissionsForCustomMilestone), new object[2]
      {
        (object) feature,
        (object) personaID
      });
      try
      {
        return MilestonesAclDbAccessor.GetPermissionsForCustomMilestone(feature, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (MilestonesAclManager), ex, this.Session.SessionInfo);
        return (Dictionary<string, bool>) null;
      }
    }
  }
}
