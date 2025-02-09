// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Acl.FieldAccessAclManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.SessionObjects;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects.Acl;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Server.Remoting.Acl
{
  public class FieldAccessAclManager : SessionBoundObject, IFieldAccessAclManager
  {
    private const string className = "FieldAccessAclManager";
    private const string objKeyPrefix = "aclmanager#";

    public FieldAccessAclManager Initialize(ISession session)
    {
      this.InitializeInternal(session, "aclmanager#" + (object) 10);
      return this;
    }

    public virtual void SetFieldIDPermission(int personaID, string fieldID, AclTriState access)
    {
      this.onApiCalled(nameof (FieldAccessAclManager), nameof (SetFieldIDPermission), new object[3]
      {
        (object) personaID,
        (object) fieldID,
        (object) access
      });
      try
      {
        FieldAccessAclDbAccessor.SetFieldPermission(personaID, fieldID, access);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FieldAccessAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetFieldIDsPermission(
      int personaID,
      Dictionary<string, AclTriState> fieldList)
    {
      this.onApiCalled(nameof (FieldAccessAclManager), nameof (SetFieldIDsPermission), new object[2]
      {
        (object) personaID,
        (object) fieldList
      });
      try
      {
        FieldAccessAclDbAccessor.SetFieldsPermission(personaID, fieldList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FieldAccessAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual AclTriState GetFieldIDPermission(int personaID, string fieldID)
    {
      this.onApiCalled(nameof (FieldAccessAclManager), nameof (GetFieldIDPermission), new object[2]
      {
        (object) personaID,
        (object) fieldID
      });
      try
      {
        return FieldAccessAclDbAccessor.GetFieldPermission(personaID, fieldID, false);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FieldAccessAclManager), ex, this.Session.SessionInfo);
        return AclTriState.Unspecified;
      }
    }

    public virtual Dictionary<string, AclTriState> GetFieldIDsPermission(int personaID)
    {
      this.onApiCalled(nameof (FieldAccessAclManager), nameof (GetFieldIDsPermission), new object[2]
      {
        (object) personaID,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Pipeline)
      });
      try
      {
        return FieldAccessAclDbAccessor.GetFieldsPermission(personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FieldAccessAclManager), ex, this.Session.SessionInfo);
        return new Dictionary<string, AclTriState>();
      }
    }

    public virtual Dictionary<string, AclTriState> GetFieldIDsPermission(int[] personaIDs)
    {
      this.onApiCalled(nameof (FieldAccessAclManager), nameof (GetFieldIDsPermission), new object[1]
      {
        (object) personaIDs
      });
      try
      {
        return FieldAccessAclDbAccessor.GetFieldsPermission(personaIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FieldAccessAclManager), ex, this.Session.SessionInfo);
        return new Dictionary<string, AclTriState>();
      }
    }

    public virtual void SetFieldIDPermission(string userID, string fieldID, AclTriState access)
    {
      this.onApiCalled(nameof (FieldAccessAclManager), nameof (SetFieldIDPermission), new object[3]
      {
        (object) userID,
        (object) fieldID,
        (object) access
      });
      try
      {
        FieldAccessAclDbAccessor.SetFieldPermission(userID, fieldID, access);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FieldAccessAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetFieldIDsPermission(
      string userId,
      Dictionary<string, AclTriState> fieldList)
    {
      this.onApiCalled(nameof (FieldAccessAclManager), nameof (SetFieldIDsPermission), new object[2]
      {
        (object) userId,
        (object) fieldList
      });
      try
      {
        FieldAccessAclDbAccessor.SetFieldsPermission(userId, fieldList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FieldAccessAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual AclTriState GetFieldIDPermission(string userID, string fieldID)
    {
      this.onApiCalled(nameof (FieldAccessAclManager), nameof (GetFieldIDPermission), new object[2]
      {
        (object) userID,
        (object) fieldID
      });
      try
      {
        return FieldAccessAclDbAccessor.GetFieldPermission(userID, fieldID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FieldAccessAclManager), ex, this.Session.SessionInfo);
        return AclTriState.Unspecified;
      }
    }

    public virtual Dictionary<string, AclTriState> GetFieldIDsPermission(string userID)
    {
      this.onApiCalled(nameof (FieldAccessAclManager), nameof (GetFieldIDsPermission), new object[2]
      {
        (object) userID,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Pipeline)
      });
      try
      {
        return FieldAccessAclDbAccessor.GetFieldsPermission(userID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FieldAccessAclManager), ex, this.Session.SessionInfo);
        return new Dictionary<string, AclTriState>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      }
    }

    public virtual Dictionary<string, AclTriState> GetUserFieldIDAccess(
      string userID,
      int[] personaIDs)
    {
      Dictionary<string, AclTriState> fieldIdsPermission1 = this.GetFieldIDsPermission(userID);
      Dictionary<string, AclTriState> fieldIdsPermission2 = this.GetFieldIDsPermission(personaIDs);
      foreach (string key in fieldIdsPermission1.Keys)
      {
        if (fieldIdsPermission2.ContainsKey(key))
          fieldIdsPermission2[key] = fieldIdsPermission1[key];
      }
      return fieldIdsPermission2;
    }

    public virtual void SyncFieldIDList(
      string[] newFields,
      string[] removeFields,
      bool newFieldAccess)
    {
      this.onApiCalled(nameof (FieldAccessAclManager), nameof (SyncFieldIDList), new object[3]
      {
        (object) newFields,
        (object) removeFields,
        (object) newFieldAccess
      });
      try
      {
        FieldAccessAclDbAccessor.SyncFieldList(newFields, removeFields, newFieldAccess);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FieldAccessAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DuplicateFieldAccess(int sourcePersonaID, int desPersonaID)
    {
      this.onApiCalled(nameof (FieldAccessAclManager), nameof (DuplicateFieldAccess), new object[2]
      {
        (object) sourcePersonaID,
        (object) desPersonaID
      });
      try
      {
        FieldAccessAclDbAccessor.DuplicateFieldAccess(sourcePersonaID, desPersonaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FieldAccessAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void InitialColumnSync(int personaID, bool accessRight)
    {
      this.onApiCalled(nameof (FieldAccessAclManager), nameof (InitialColumnSync), new object[2]
      {
        (object) personaID,
        (object) accessRight
      });
      try
      {
        FieldAccessAclDbAccessor.InitialColumnSync(personaID, accessRight);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FieldAccessAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AddFieldPermissionToAllPersonas(string[] fields)
    {
      this.onApiCalled(nameof (FieldAccessAclManager), nameof (AddFieldPermissionToAllPersonas), (object[]) fields);
      try
      {
        FieldAccessAclDbAccessor.AddFieldPermissionToAllPersonas(fields);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FieldAccessAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateFeeManagementPermission(
      FeeManagementPersonaInfo feeManagementPersonaInfo)
    {
      this.onApiCalled(nameof (FieldAccessAclManager), nameof (UpdateFeeManagementPermission), new object[1]
      {
        (object) feeManagementPersonaInfo
      });
      try
      {
        FieldAccessAclDbAccessor.UpdateFeeManagementPermission(feeManagementPersonaInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FieldAccessAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual FeeManagementPersonaInfo GetFeeManagementPermission(int[] personIDs)
    {
      this.onApiCalled(nameof (FieldAccessAclManager), nameof (GetFeeManagementPermission), new object[1]
      {
        (object) personIDs
      });
      try
      {
        return FieldAccessAclDbAccessor.GetFeeManagementPermission(personIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FieldAccessAclManager), ex, this.Session.SessionInfo);
        return (FeeManagementPersonaInfo) null;
      }
    }
  }
}
