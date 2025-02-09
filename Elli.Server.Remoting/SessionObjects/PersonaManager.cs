// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.PersonaManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Server;
using System;
using System.Diagnostics;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class PersonaManager : SessionBoundObject, IPersonaManager
  {
    private const string className = "PersonaManager";

    public PersonaManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (PersonaManager).ToLower());
      return this;
    }

    public virtual Persona GetPersonaByName(string personaName)
    {
      this.onApiCalled(nameof (PersonaManager), nameof (GetPersonaByName), new object[2]
      {
        (object) personaName,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Login)
      });
      try
      {
        return PersonaAccessor.GetPersonaByName(personaName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PersonaManager), ex, this.Session.SessionInfo);
        return (Persona) null;
      }
    }

    public virtual bool PersonaNameExists(string personaName)
    {
      this.onApiCalled(nameof (PersonaManager), nameof (PersonaNameExists), new object[1]
      {
        (object) personaName
      });
      try
      {
        return !(PersonaAccessor.GetPersonaByName(personaName) == (Persona) null);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PersonaManager), ex, this.Session.SessionInfo);
        throw ex;
      }
    }

    public virtual Persona[] GetAllPersonas()
    {
      this.onApiCalled(nameof (PersonaManager), nameof (GetAllPersonas), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Login)
      });
      try
      {
        return PersonaAccessor.GetAllPersonas();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PersonaManager), ex, this.Session.SessionInfo);
        return (Persona[]) null;
      }
    }

    public virtual Persona[] GetAllPersonas(PersonaType[] personaTypes)
    {
      this.onApiCalled(nameof (PersonaManager), nameof (GetAllPersonas), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Login)
      });
      try
      {
        return PersonaAccessor.GetPersonasOfTypes(personaTypes);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PersonaManager), ex, this.Session.SessionInfo);
        return (Persona[]) null;
      }
    }

    public virtual Persona[] GetAllCustomPersonas()
    {
      this.onApiCalled(nameof (PersonaManager), nameof (GetAllCustomPersonas), new object[1]
      {
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Login)
      });
      try
      {
        return PersonaAccessor.GetAllCustomPersonas();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PersonaManager), ex, this.Session.SessionInfo);
        return (Persona[]) null;
      }
    }

    public virtual Persona GetPersonaByID(int personaID)
    {
      this.onApiCalled(nameof (PersonaManager), nameof (GetPersonaByID), new object[2]
      {
        (object) personaID,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Login)
      });
      try
      {
        return PersonaAccessor.GetPersona(personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PersonaManager), ex, this.Session.SessionInfo);
        return (Persona) null;
      }
    }

    public virtual Persona CreatePersona(
      string name,
      bool aclFeaturesDefault,
      bool isInternal,
      bool isExternal)
    {
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_PersonasEdit))
      {
        Err.Raise(TraceLevel.Warning, nameof (PersonaManager), (ServerException) new SecurityException("User does not have the access right to create persona"));
        return (Persona) null;
      }
      this.onApiCalled(nameof (PersonaManager), nameof (CreatePersona), new object[2]
      {
        (object) name,
        (object) aclFeaturesDefault
      });
      try
      {
        Persona persona = PersonaAccessor.CreatePersona(name, aclFeaturesDefault, isInternal, isExternal);
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new PersonaAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.PersonaCreated, DateTime.Now, persona.ID, persona.Name));
        return persona;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PersonaManager), ex, this.Session.SessionInfo);
        return (Persona) null;
      }
    }

    public virtual void DeletePersona(int personaID)
    {
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_PersonasEdit))
        Err.Raise(TraceLevel.Warning, nameof (PersonaManager), (ServerException) new SecurityException("User does not have the access right to delete persona"));
      this.onApiCalled(nameof (PersonaManager), nameof (DeletePersona), new object[1]
      {
        (object) personaID
      });
      try
      {
        string name = this.GetPersonaByID(personaID).Name;
        PersonaAccessor.DeletePersona(personaID);
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new PersonaAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.PersonaDeleted, DateTime.Now, personaID, name));
        UserStore.RefreshCache();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PersonaManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeletePersona(Persona persona) => this.DeletePersona(persona.ID);

    public virtual void RenamePersona(int personaID, string newName)
    {
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_PersonasEdit))
        Err.Raise(TraceLevel.Warning, nameof (PersonaManager), (ServerException) new SecurityException("User does not have the access right to rename persona"));
      this.onApiCalled(nameof (PersonaManager), nameof (RenamePersona), new object[2]
      {
        (object) personaID,
        (object) newName
      });
      try
      {
        string name = this.GetPersonaByID(personaID).Name;
        PersonaAccessor.RenamePersona(personaID, newName);
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new PersonaAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.PersonaModified, DateTime.Now, personaID, name));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PersonaManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void RenamePersona(Persona persona, string newName)
    {
      this.RenamePersona(persona.ID, newName);
    }

    public virtual void UpdatePersonaOrder(Persona[] personaList)
    {
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_PersonasEdit))
        Err.Raise(TraceLevel.Warning, nameof (PersonaManager), (ServerException) new SecurityException("User does not have the access right to update persona order"));
      this.onApiCalled(nameof (PersonaManager), nameof (UpdatePersonaOrder), (object[]) personaList);
      try
      {
        PersonaAccessor.UpdatePersonaDisplayOrder(personaList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PersonaManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdatePersonaType(int personaID, bool isInternal, bool isExternal)
    {
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_PersonasEdit))
        Err.Raise(TraceLevel.Warning, nameof (PersonaManager), (ServerException) new SecurityException("User does not have the access right to update persona type"));
      this.onApiCalled(nameof (PersonaManager), nameof (UpdatePersonaType), new object[1]
      {
        (object) personaID
      });
      try
      {
        PersonaAccessor.UpdatePersonaType(personaID, isInternal, isExternal);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PersonaManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual int GetAssociatedUsersCount(int personaID)
    {
      this.onApiCalled(nameof (PersonaManager), nameof (GetAssociatedUsersCount), new object[1]
      {
        (object) personaID
      });
      try
      {
        return PersonaAccessor.GetAssociatedUsersCount(personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PersonaManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }
  }
}
