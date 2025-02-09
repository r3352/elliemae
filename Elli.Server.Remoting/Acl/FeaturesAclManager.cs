// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Acl.FeaturesAclManager
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
  public class FeaturesAclManager : SessionBoundObject, IFeaturesAclManager
  {
    private const string className = "FeaturesAclManager";
    private const string objKeyPrefix = "aclmanager#";

    public FeaturesAclManager Initialize(ISession session)
    {
      this.InitializeInternal(session, "aclmanager#" + (object) 0);
      return this;
    }

    public virtual Hashtable GetPermissions(AclFeature[] features, string userid)
    {
      this.onApiCalled(nameof (FeaturesAclManager), nameof (GetPermissions), new object[2]
      {
        (object) features,
        (object) userid
      });
      try
      {
        return FeaturesAclDbAccessor.GetPermissions(features, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeaturesAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual AclTriState GetPermission(AclFeature feature, string userid)
    {
      this.onApiCalled(nameof (FeaturesAclManager), nameof (GetPermission), new object[2]
      {
        (object) feature,
        (object) userid
      });
      try
      {
        return FeaturesAclDbAccessor.GetPermission(feature, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeaturesAclManager), ex, this.Session.SessionInfo);
        return AclTriState.Unspecified;
      }
    }

    public virtual Hashtable GetPermissions(AclFeature[] features, int personaID)
    {
      this.onApiCalled(nameof (FeaturesAclManager), nameof (GetPermissions), new object[2]
      {
        (object) features,
        (object) personaID
      });
      try
      {
        return FeaturesAclDbAccessor.GetPermissions(features, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeaturesAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual bool GetPermission(AclFeature feature, int personaID)
    {
      this.onApiCalled(nameof (FeaturesAclManager), "GetPermissions", new object[2]
      {
        (object) feature,
        (object) personaID
      });
      try
      {
        return FeaturesAclDbAccessor.GetPermission(feature, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeaturesAclManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual Hashtable GetPermissions(AclFeature[] features, int[] personaIDs)
    {
      this.onApiCalled(nameof (FeaturesAclManager), nameof (GetPermissions), new object[2]
      {
        (object) features,
        (object) personaIDs
      });
      try
      {
        return FeaturesAclDbAccessor.GetPermissions(features, personaIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeaturesAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual Hashtable GetPermissions(AclFeature[] features, Persona[] personas)
    {
      this.onApiCalled(nameof (FeaturesAclManager), nameof (GetPermissions), new object[2]
      {
        (object) features,
        (object) personas
      });
      try
      {
        return FeaturesAclDbAccessor.GetPermissions(features, AclUtils.GetPersonaIDs(personas));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeaturesAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual bool CheckPermission(AclFeature feature, UserInfo userInfo)
    {
      this.onApiCalled(nameof (FeaturesAclManager), nameof (CheckPermission), new object[2]
      {
        (object) feature,
        (object) userInfo
      });
      try
      {
        return FeaturesAclDbAccessor.CheckPermission(feature, userInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeaturesAclManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool CheckPermission(AclFeature feature, string userID)
    {
      this.onApiCalled(nameof (FeaturesAclManager), nameof (CheckPermission), new object[2]
      {
        (object) feature,
        (object) userID
      });
      UserInfo userById = User.GetUserById(userID);
      if (!userById.IsAdministrator())
      {
        if (!userById.IsSuperAdministrator())
        {
          try
          {
            return FeaturesAclDbAccessor.CheckPermission(feature, userById);
          }
          catch (Exception ex)
          {
            Err.Reraise(nameof (FeaturesAclManager), ex, this.Session.SessionInfo);
            return false;
          }
        }
      }
      return true;
    }

    public virtual Hashtable CheckPermissions(AclFeature[] features, UserInfo userInfo)
    {
      this.onApiCalled(nameof (FeaturesAclManager), nameof (CheckPermissions), new object[2]
      {
        (object) features,
        (object) userInfo
      });
      try
      {
        return FeaturesAclDbAccessor.CheckPermissions(features, userInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeaturesAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual void SetPermission(AclFeature feature, string userid, AclTriState access)
    {
      this.onApiCalled(nameof (FeaturesAclManager), nameof (SetPermission), new object[3]
      {
        (object) feature,
        (object) userid,
        (object) access
      });
      try
      {
        FeaturesAclDbAccessor.SetPermission(feature, userid, access);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeaturesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermission(AclFeature feature, int personaID, AclTriState access)
    {
      this.onApiCalled(nameof (FeaturesAclManager), nameof (SetPermission), new object[3]
      {
        (object) feature,
        (object) personaID,
        (object) access
      });
      try
      {
        FeaturesAclDbAccessor.SetPermission(feature, personaID, access);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeaturesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermissions(Hashtable featureAccesses, string userid)
    {
      this.onApiCalled(nameof (FeaturesAclManager), nameof (SetPermissions), new object[2]
      {
        (object) featureAccesses,
        (object) userid
      });
      try
      {
        FeaturesAclDbAccessor.SetPermissions(featureAccesses, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeaturesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermissions(Hashtable featureAccesses, int personaID)
    {
      this.onApiCalled(nameof (FeaturesAclManager), nameof (SetPermissions), new object[2]
      {
        (object) featureAccesses,
        (object) personaID
      });
      try
      {
        FeaturesAclDbAccessor.SetPermissions(featureAccesses, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeaturesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DisablePermission(AclFeature feature)
    {
      this.onApiCalled(nameof (FeaturesAclManager), "Disable permission", new object[1]
      {
        (object) feature
      });
      try
      {
        FeaturesAclDbAccessor.DisablePermission(feature);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeaturesAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual string[] GetPersonaListByFeature(AclFeature[] features, AclTriState access)
    {
      this.onApiCalled(nameof (FeaturesAclManager), "GetUserListByFeature", new object[2]
      {
        (object) features,
        (object) access
      });
      try
      {
        return FeaturesAclDbAccessor.GetPersonaListByFeature(features, access);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeaturesAclManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual string[] GetUserListByFeature(AclFeature[] features, AclTriState access)
    {
      this.onApiCalled(nameof (FeaturesAclManager), nameof (GetUserListByFeature), new object[2]
      {
        (object) features,
        (object) access
      });
      try
      {
        return FeaturesAclDbAccessor.GetUserListByFeature(features, access);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeaturesAclManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual void DuplicateACLFeatures(int sourcePersonaID, int desPersonaID)
    {
      this.onApiCalled(nameof (FeaturesAclManager), nameof (DuplicateACLFeatures), new object[2]
      {
        (object) sourcePersonaID,
        (object) desPersonaID
      });
      try
      {
        FeaturesAclDbAccessor.DuplicateACLFeatures(sourcePersonaID, desPersonaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeaturesAclManager), ex, this.Session.SessionInfo);
      }
    }
  }
}
