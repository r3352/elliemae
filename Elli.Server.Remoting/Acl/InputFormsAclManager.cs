// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Acl.InputFormsAclManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.SessionObjects;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects.Acl;
using System;
using System.Collections;

#nullable disable
namespace Elli.Server.Remoting.Acl
{
  public class InputFormsAclManager : SessionBoundObject, IInputFormsAclManager
  {
    private const string className = "InputFormsAclManager";
    private const string objKeyPrefix = "aclmanager#";

    public InputFormsAclManager Initialize(ISession session)
    {
      this.InitializeInternal(session, "aclmanager#" + (object) 2);
      return this;
    }

    public virtual InputFormInfo[] GetFormInfos(InputFormType formType)
    {
      this.onApiCalled(nameof (InputFormsAclManager), nameof (GetFormInfos), new object[1]
      {
        (object) formType
      });
      try
      {
        return InputForms.GetFormInfos(formType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InputFormsAclManager), ex, this.Session.SessionInfo);
        return (InputFormInfo[]) null;
      }
    }

    public virtual InputFormInfo[] GetAccessibleForms(string userId, Persona[] personas)
    {
      return this.GetAccessibleForms(userId, personas, false);
    }

    public virtual InputFormInfo[] GetAccessibleForms(
      string userId,
      Persona[] personas,
      bool useReadReplica)
    {
      this.onApiCalled(nameof (InputFormsAclManager), nameof (GetAccessibleForms), new object[2]
      {
        (object) userId,
        (object) personas
      });
      try
      {
        return InputFormsAclDbAccessor.GetAccessibleForms(userId, personas, useReadReplica);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InputFormsAclManager), ex, this.Session.SessionInfo);
        return (InputFormInfo[]) null;
      }
    }

    public virtual InputFormInfo[] GetAccessibleForms(string userId)
    {
      this.onApiCalled(nameof (InputFormsAclManager), nameof (GetAccessibleForms), new object[1]
      {
        (object) userId
      });
      try
      {
        return InputFormsAclDbAccessor.GetAccessibleForms(userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InputFormsAclManager), ex, this.Session.SessionInfo);
        return (InputFormInfo[]) null;
      }
    }

    public virtual InputFormInfo[] GetAccessibleForms(int[] personaIDs)
    {
      this.onApiCalled(nameof (InputFormsAclManager), nameof (GetAccessibleForms), new object[1]
      {
        (object) personaIDs
      });
      try
      {
        return InputFormsAclDbAccessor.GetAccessibleForms(personaIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InputFormsAclManager), ex, this.Session.SessionInfo);
        return (InputFormInfo[]) null;
      }
    }

    public virtual InputFormInfo[] GetAccessibleForms(Persona[] personas)
    {
      this.onApiCalled(nameof (InputFormsAclManager), nameof (GetAccessibleForms), (object[]) personas);
      try
      {
        return InputFormsAclDbAccessor.GetAccessibleForms(personas);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InputFormsAclManager), ex, this.Session.SessionInfo);
        return (InputFormInfo[]) null;
      }
    }

    public virtual InputFormInfo[] GetAccessibleForms(int personaID)
    {
      this.onApiCalled(nameof (InputFormsAclManager), nameof (GetAccessibleForms), new object[1]
      {
        (object) personaID
      });
      try
      {
        return InputFormsAclDbAccessor.GetAccessibleForms(personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InputFormsAclManager), ex, this.Session.SessionInfo);
        return (InputFormInfo[]) null;
      }
    }

    public virtual InputFormInfo[] GetAccessibleForms()
    {
      this.onApiCalled(nameof (InputFormsAclManager), nameof (GetAccessibleForms), Array.Empty<object>());
      try
      {
        return InputFormsAclDbAccessor.GetAccessibleForms(this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InputFormsAclManager), ex, this.Session.SessionInfo);
        return (InputFormInfo[]) null;
      }
    }

    public virtual AclTriState GetPermission(string form, string userid)
    {
      this.onApiCalled(nameof (InputFormsAclManager), nameof (GetPermission), new object[2]
      {
        (object) form,
        (object) userid
      });
      try
      {
        return InputFormsAclDbAccessor.GetPermission(form, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InputFormsAclManager), ex, this.Session.SessionInfo);
        return AclTriState.Unspecified;
      }
    }

    public virtual bool GetPermission(string form, int personaID)
    {
      this.onApiCalled(nameof (InputFormsAclManager), nameof (GetPermission), new object[2]
      {
        (object) form,
        (object) personaID
      });
      try
      {
        return InputFormsAclDbAccessor.GetPermission(form, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InputFormsAclManager), ex, this.Session.SessionInfo);
        throw ex;
      }
    }

    public virtual Hashtable GetPermissions(string form, int[] personaIDs)
    {
      this.onApiCalled(nameof (InputFormsAclManager), nameof (GetPermissions), new object[2]
      {
        (object) form,
        (object) personaIDs
      });
      try
      {
        return InputFormsAclDbAccessor.GetPermissions(form, personaIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InputFormsAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual Hashtable GetPermissions(string form, Persona[] personas)
    {
      this.onApiCalled(nameof (InputFormsAclManager), nameof (GetPermissions), new object[2]
      {
        (object) form,
        (object) personas
      });
      try
      {
        return InputFormsAclDbAccessor.GetPermissions(form, personas);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InputFormsAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual Hashtable GetPermissionsForAllForms(string userid)
    {
      this.onApiCalled(nameof (InputFormsAclManager), nameof (GetPermissionsForAllForms), new object[1]
      {
        (object) userid
      });
      try
      {
        return InputFormsAclDbAccessor.GetPermissionsForAllForms(userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InputFormsAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual Hashtable GetPermissionsForAllForms(int personaID)
    {
      this.onApiCalled(nameof (InputFormsAclManager), nameof (GetPermissionsForAllForms), new object[1]
      {
        (object) personaID
      });
      try
      {
        return InputFormsAclDbAccessor.GetPermissionsForAllForms(personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InputFormsAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual Hashtable GetPermissionsForAllForms(int[] personaIDs)
    {
      this.onApiCalled(nameof (InputFormsAclManager), nameof (GetPermissionsForAllForms), new object[1]
      {
        (object) personaIDs
      });
      try
      {
        return InputFormsAclDbAccessor.GetPermissionsForAllForms(personaIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InputFormsAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual Hashtable GetPermissionsForAllForms(Persona[] personas)
    {
      this.onApiCalled(nameof (InputFormsAclManager), nameof (GetPermissionsForAllForms), (object[]) personas);
      try
      {
        return InputFormsAclDbAccessor.GetPermissionsForAllForms(personas);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InputFormsAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual void SetPermission(string form, string userid, object access)
    {
      this.onApiCalled(nameof (InputFormsAclManager), nameof (SetPermission), new object[3]
      {
        (object) form,
        (object) userid,
        access
      });
      try
      {
        InputFormsAclDbAccessor.SetPermission(form, userid, access);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InputFormsAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermission(string[] forms, int personaID, bool[] accesses)
    {
      this.onApiCalled(nameof (InputFormsAclManager), nameof (SetPermission), new object[3]
      {
        (object) forms,
        (object) personaID,
        (object) accesses
      });
      try
      {
        InputFormsAclDbAccessor.SetPermission(forms, personaID, accesses);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InputFormsAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetPermission(string form, int personaID, bool access)
    {
      this.onApiCalled(nameof (InputFormsAclManager), nameof (SetPermission), new object[3]
      {
        (object) form,
        (object) personaID,
        (object) access
      });
      try
      {
        InputFormsAclDbAccessor.SetPermission(form, personaID, access);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InputFormsAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual bool CheckPermission(string form)
    {
      this.onApiCalled(nameof (InputFormsAclManager), nameof (CheckPermission), new object[1]
      {
        (object) form
      });
      try
      {
        return InputFormsAclDbAccessor.CheckPermission(form, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InputFormsAclManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual Hashtable CheckPermissions(string[] forms)
    {
      this.onApiCalled(nameof (InputFormsAclManager), nameof (CheckPermissions), (object[]) forms);
      try
      {
        return InputFormsAclDbAccessor.CheckPermissions(forms, this.Session.GetUserInfo());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InputFormsAclManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual void DuplicateACLInputForms(int sourcePersonaID, int desPersonaID)
    {
      this.onApiCalled(nameof (InputFormsAclManager), nameof (DuplicateACLInputForms), new object[2]
      {
        (object) sourcePersonaID,
        (object) desPersonaID
      });
      try
      {
        InputFormsAclDbAccessor.DuplicateACLInputForms(sourcePersonaID, desPersonaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (InputFormsAclManager), ex, this.Session.SessionInfo);
      }
    }
  }
}
