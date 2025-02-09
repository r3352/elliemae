// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Acl.FeatureConfigsAclManager
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
using System.Collections.Generic;

#nullable disable
namespace Elli.Server.Remoting.Acl
{
  public class FeatureConfigsAclManager : SessionBoundObject, IFeatureConfigsAclManager
  {
    private const string className = "FeatureConfigsAclManager";
    private const string objKeyPrefix = "aclmanager#";

    public FeatureConfigsAclManager Initialize(ISession session)
    {
      this.InitializeInternal(session, "aclmanager#" + (object) 14);
      return this;
    }

    public virtual Dictionary<AclFeature, int> GetPermissions(AclFeature[] features, string userid)
    {
      this.onApiCalled(nameof (FeatureConfigsAclManager), nameof (GetPermissions), new object[2]
      {
        (object) features,
        (object) userid
      });
      try
      {
        return FeatureConfigsAclDbAccessor.GetPermissions(features, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeatureConfigsAclManager), ex, this.Session.SessionInfo);
        return (Dictionary<AclFeature, int>) null;
      }
    }

    public virtual Dictionary<AclFeature, int> GetPermissions(AclFeature[] features, int personaID)
    {
      this.onApiCalled(nameof (FeatureConfigsAclManager), nameof (GetPermissions), new object[2]
      {
        (object) features,
        (object) personaID
      });
      try
      {
        return FeatureConfigsAclDbAccessor.GetPermissions(features, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeatureConfigsAclManager), ex, this.Session.SessionInfo);
        return (Dictionary<AclFeature, int>) null;
      }
    }

    public virtual Dictionary<AclFeature, int> GetPermissions(
      AclFeature[] features,
      int[] personaIDs)
    {
      this.onApiCalled(nameof (FeatureConfigsAclManager), nameof (GetPermissions), new object[2]
      {
        (object) features,
        (object) personaIDs
      });
      try
      {
        return FeatureConfigsAclDbAccessor.GetPermissions(features, personaIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeatureConfigsAclManager), ex, this.Session.SessionInfo);
        return (Dictionary<AclFeature, int>) null;
      }
    }

    public virtual Dictionary<AclFeature, int> GetPermissions(
      AclFeature[] features,
      Persona[] personas)
    {
      this.onApiCalled(nameof (FeatureConfigsAclManager), nameof (GetPermissions), new object[2]
      {
        (object) features,
        (object) personas
      });
      try
      {
        return FeatureConfigsAclDbAccessor.GetPermissions(features, AclUtils.GetPersonaIDs(personas));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeatureConfigsAclManager), ex, this.Session.SessionInfo);
        return (Dictionary<AclFeature, int>) null;
      }
    }

    public virtual int GetPermission(AclFeature feature, string userid)
    {
      this.onApiCalled(nameof (FeatureConfigsAclManager), nameof (GetPermission), new object[2]
      {
        (object) feature,
        (object) userid
      });
      try
      {
        return FeatureConfigsAclDbAccessor.GetPermission(feature, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeatureConfigsAclManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual int GetPermission(AclFeature feature, int personaID)
    {
      this.onApiCalled(nameof (FeatureConfigsAclManager), "GetPermissions", new object[2]
      {
        (object) feature,
        (object) personaID
      });
      try
      {
        return FeatureConfigsAclDbAccessor.GetPermission(feature, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeatureConfigsAclManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }

    public virtual int CheckPermission(AclFeature feature, UserInfo userInfo)
    {
      this.onApiCalled(nameof (FeatureConfigsAclManager), nameof (CheckPermission), new object[2]
      {
        (object) feature,
        (object) userInfo
      });
      try
      {
        return FeatureConfigsAclDbAccessor.CheckPermission(feature, userInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeatureConfigsAclManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }

    public virtual int CheckPermission(AclFeature feature, string userID)
    {
      this.onApiCalled(nameof (FeatureConfigsAclManager), nameof (CheckPermission), new object[2]
      {
        (object) feature,
        (object) userID
      });
      UserInfo userInfo = this.Session.GetUserInfo();
      if (!userInfo.IsAdministrator())
      {
        if (!userInfo.IsSuperAdministrator())
        {
          try
          {
            return FeatureConfigsAclDbAccessor.CheckPermission(feature, userInfo);
          }
          catch (Exception ex)
          {
            Err.Reraise(nameof (FeatureConfigsAclManager), ex, this.Session.SessionInfo);
            return 0;
          }
        }
      }
      return int.MaxValue;
    }

    public virtual Dictionary<AclFeature, int> CheckPermissions(
      AclFeature[] features,
      UserInfo userInfo)
    {
      this.onApiCalled(nameof (FeatureConfigsAclManager), nameof (CheckPermissions), new object[2]
      {
        (object) features,
        (object) userInfo
      });
      try
      {
        return FeatureConfigsAclDbAccessor.CheckPermissions(features, userInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeatureConfigsAclManager), ex, this.Session.SessionInfo);
        return new Dictionary<AclFeature, int>();
      }
    }

    public virtual void SetPermission(AclFeature feature, string userid, int access)
    {
      this.onApiCalled(nameof (FeatureConfigsAclManager), nameof (SetPermission), new object[3]
      {
        (object) feature,
        (object) userid,
        (object) access
      });
      try
      {
        FeatureConfigsAclDbAccessor.SetPermission(feature, userid, access);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeatureConfigsAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermission(AclFeature feature, int personaID, int access)
    {
      this.onApiCalled(nameof (FeatureConfigsAclManager), nameof (SetPermission), new object[3]
      {
        (object) feature,
        (object) personaID,
        (object) access
      });
      try
      {
        FeatureConfigsAclDbAccessor.SetPermission(feature, personaID, access);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeatureConfigsAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermissions(Dictionary<AclFeature, int> featureAccesses, string userid)
    {
      this.onApiCalled(nameof (FeatureConfigsAclManager), nameof (SetPermissions), new object[2]
      {
        (object) featureAccesses,
        (object) userid
      });
      try
      {
        FeatureConfigsAclDbAccessor.SetPermissions(featureAccesses, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeatureConfigsAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermissions(Dictionary<AclFeature, int> featureAccesses, int personaID)
    {
      this.onApiCalled(nameof (FeatureConfigsAclManager), nameof (SetPermissions), new object[2]
      {
        (object) featureAccesses,
        (object) personaID
      });
      try
      {
        FeatureConfigsAclDbAccessor.SetPermissions(featureAccesses, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeatureConfigsAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void ClearUserSpecificSettings(string userid)
    {
      this.onApiCalled(nameof (FeatureConfigsAclManager), nameof (ClearUserSpecificSettings), new object[1]
      {
        (object) userid
      });
      try
      {
        FeatureConfigsAclDbAccessor.ClearUserSpecificSettings(userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeatureConfigsAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void ClearUserSpecificSettings(AclFeature feature, string userid)
    {
      this.onApiCalled(nameof (FeatureConfigsAclManager), nameof (ClearUserSpecificSettings), new object[2]
      {
        (object) feature,
        (object) userid
      });
      try
      {
        FeatureConfigsAclDbAccessor.ClearUserSpecificSettings(feature, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeatureConfigsAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DuplicateACLFeatureConfigs(int sourcePersonaID, int desPersonaID)
    {
      this.onApiCalled(nameof (FeatureConfigsAclManager), "DuplicateACLFeatures", new object[2]
      {
        (object) sourcePersonaID,
        (object) desPersonaID
      });
      try
      {
        FeatureConfigsAclDbAccessor.DuplicateACLFeatureConfigs(sourcePersonaID, desPersonaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FeatureConfigsAclManager), ex, this.Session.SessionInfo);
      }
    }
  }
}
