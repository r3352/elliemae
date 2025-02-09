// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Acl.EnhancedConditionsAclManager
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
  public class EnhancedConditionsAclManager : SessionBoundObject, IEnhancedConditionsAclManager
  {
    private const string className = "EnhancedConditionsAclManager";
    private const string objKeyPrefix = "aclmanager#";

    public EnhancedConditionsAclManager Initialize(ISession session)
    {
      this.InitializeInternal(session, "aclmanager#" + (object) 16);
      return this;
    }

    public virtual Hashtable GetPermissions(
      int personaID,
      Guid conditionTypeID,
      AclEnhancedConditionType feature)
    {
      this.onApiCalled(nameof (EnhancedConditionsAclManager), nameof (GetPermissions), new object[3]
      {
        (object) conditionTypeID,
        (object) personaID,
        (object) feature
      });
      try
      {
        return EnhancedConditionsAclDbAccessor.GetPermissions(feature, personaID, conditionTypeID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (EnhancedConditionsAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual Hashtable GetPersonaPermissions(Guid conditionType, int personaID)
    {
      this.onApiCalled(nameof (EnhancedConditionsAclManager), "GetPermissions", new object[2]
      {
        (object) conditionType,
        (object) personaID
      });
      try
      {
        return EnhancedConditionsAclDbAccessor.GetPersonaPermissions(conditionType, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (EnhancedConditionsAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual void SetPermissions(
      AclEnhancedConditionType feature,
      int personaID,
      AclTriState access,
      Guid conditionTypeID)
    {
      this.onApiCalled(nameof (EnhancedConditionsAclManager), nameof (SetPermissions), new object[2]
      {
        (object) conditionTypeID,
        (object) personaID
      });
      try
      {
        EnhancedConditionsAclDbAccessor.SetPermission(feature, personaID, access, conditionTypeID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (EnhancedConditionsAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermissions(
      Dictionary<AclEnhancedConditionType, bool> features,
      int personaID,
      Guid conditionTypeID)
    {
      this.onApiCalled(nameof (EnhancedConditionsAclManager), nameof (SetPermissions), new object[2]
      {
        (object) conditionTypeID,
        (object) personaID
      });
      try
      {
        EnhancedConditionsAclDbAccessor.SetPermissions(features, personaID, conditionTypeID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (EnhancedConditionsAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteAllUserSpecificPermissions(string userID)
    {
      this.onApiCalled(nameof (EnhancedConditionsAclManager), nameof (DeleteAllUserSpecificPermissions), new object[1]
      {
        (object) userID
      });
      try
      {
        EnhancedConditionsAclDbAccessor.DeleteAllUserSpecificPermissions(userID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (EnhancedConditionsAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermissions(
      AclEnhancedConditionType feature,
      string userID,
      AclTriState access,
      Guid conditionTypeID)
    {
      this.onApiCalled(nameof (EnhancedConditionsAclManager), nameof (SetPermissions), new object[2]
      {
        (object) conditionTypeID,
        (object) userID
      });
      try
      {
        EnhancedConditionsAclDbAccessor.SetPermission(feature, userID, access, conditionTypeID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (EnhancedConditionsAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DuplicateEnhancedConditionTypeFeatures(
      int sourcePersonaID,
      int desPersonaID)
    {
      this.onApiCalled(nameof (EnhancedConditionsAclManager), "SetPermissions", new object[2]
      {
        (object) sourcePersonaID,
        (object) desPersonaID
      });
      try
      {
        EnhancedConditionsAclDbAccessor.DuplicateEnhancedConditionTypeFeatures(sourcePersonaID, desPersonaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (EnhancedConditionsAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual Hashtable GetPermissions(
      AclEnhancedConditionType[] features,
      Guid conditionTypeID,
      string userid)
    {
      this.onApiCalled(nameof (EnhancedConditionsAclManager), nameof (GetPermissions), new object[3]
      {
        (object) features,
        (object) conditionTypeID,
        (object) userid
      });
      try
      {
        return EnhancedConditionsAclDbAccessor.GetPermissions(features, conditionTypeID, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (EnhancedConditionsAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual Hashtable GetPermissions(
      AclEnhancedConditionType[] features,
      Guid conditionTypeID,
      int[] personas)
    {
      this.onApiCalled(nameof (EnhancedConditionsAclManager), nameof (GetPermissions), new object[3]
      {
        (object) features,
        (object) conditionTypeID,
        (object) personas
      });
      try
      {
        return EnhancedConditionsAclDbAccessor.GetPermissions(features, conditionTypeID, personas);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (EnhancedConditionsAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual Dictionary<string, Hashtable> GetPermissionsByUserInfo(
      AclEnhancedConditionType[] features,
      string userId)
    {
      this.onApiCalled(nameof (EnhancedConditionsAclManager), nameof (GetPermissionsByUserInfo), new object[2]
      {
        (object) features,
        (object) userId
      });
      try
      {
        return EnhancedConditionsAclDbAccessor.GetPermissions(features, userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (EnhancedConditionsAclManager), ex, this.Session.SessionInfo);
        return (Dictionary<string, Hashtable>) null;
      }
    }
  }
}
