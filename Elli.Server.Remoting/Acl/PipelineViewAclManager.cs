// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Acl.PipelineViewAclManager
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
  public class PipelineViewAclManager : SessionBoundObject, IPipelineViewAclManager
  {
    private const string className = "PipelineViewAclManager";
    private const string objKeyPrefix = "aclmanager#";

    public PipelineViewAclManager Initialize(ISession session)
    {
      this.InitializeInternal(session, "aclmanager#" + (object) 11);
      return this;
    }

    public virtual PersonaPipelineView GetPipelineView(int viewId)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (GetPipelineView), new object[1]
      {
        (object) viewId
      });
      try
      {
        return PipelineViewAclDbAccessor.GetPipelineView(viewId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
        return (PersonaPipelineView) null;
      }
    }

    private void updateCacheUserPipelineCache()
    {
      ClientContext.GetCurrent().Cache.Remove(string.Format("{0}_{1}", (object) "UserCustomPipeline", (object) this.Session.UserID));
      ClientContext.GetCurrent().Cache.Get<UserPipelineView[]>("UserCustomPipeline", this.Session.UserID, (Func<UserPipelineView[]>) (() => PipelineViewAclDbAccessor.GetUserCustomPipelineViews(this.Session.UserID)));
    }

    public virtual UserPipelineView GetUserCustomPipelineView(int viewId)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (GetUserCustomPipelineView), new object[1]
      {
        (object) viewId
      });
      try
      {
        return PipelineViewAclDbAccessor.GetUserPipelineView(viewId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
        return (UserPipelineView) null;
      }
    }

    public virtual PersonaPipelineView GetPersonaPipelineView(int personaID, string viewName)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (GetPersonaPipelineView), new object[2]
      {
        (object) personaID,
        (object) viewName
      });
      try
      {
        return PipelineViewAclDbAccessor.GetPersonaPipelineView(personaID, viewName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
        return (PersonaPipelineView) null;
      }
    }

    public virtual UserPipelineView GetUserPipelineView(string userID, string viewName)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (GetUserPipelineView), new object[2]
      {
        (object) userID,
        (object) viewName
      });
      try
      {
        return PipelineViewAclDbAccessor.GetUserPipelineView(userID, viewName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
        return (UserPipelineView) null;
      }
    }

    public virtual PersonaPipelineView[] GetPersonaPipelineViews(int personaID)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (GetPersonaPipelineViews), new object[2]
      {
        (object) personaID,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Pipeline)
      });
      try
      {
        return PipelineViewAclDbAccessor.GetPersonaPipelineViews(personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
        return (PersonaPipelineView[]) null;
      }
    }

    public virtual UserPipelineView[] GetUserCustomPipelineViews(string userId)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (GetUserCustomPipelineViews), new object[2]
      {
        (object) userId,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Pipeline)
      });
      try
      {
        return PipelineViewAclDbAccessor.GetUserCustomPipelineViews(userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
        return (UserPipelineView[]) null;
      }
    }

    public virtual PersonaPipelineView[] GetUserPipelineViews(string userId)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (GetUserPipelineViews), new object[2]
      {
        (object) userId,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Pipeline)
      });
      try
      {
        return PipelineViewAclDbAccessor.GetUserPipelineViews(userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
        return (PersonaPipelineView[]) null;
      }
    }

    public virtual PersonaPipelineView CreatePipelineView(PersonaPipelineView pipelineView)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (CreatePipelineView), new object[1]
      {
        (object) pipelineView
      });
      try
      {
        return PipelineViewAclDbAccessor.CreatePipelineView(pipelineView, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
        return (PersonaPipelineView) null;
      }
    }

    public virtual UserPipelineView CreatePipelineUserView(
      UserPipelineView pipelineView,
      string userId)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (CreatePipelineUserView), new object[1]
      {
        (object) pipelineView
      });
      try
      {
        return PipelineViewAclDbAccessor.CreatePipelineUserView(pipelineView, userId, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
        return (UserPipelineView) null;
      }
    }

    public virtual void UpdatePipelineView(PersonaPipelineView pipelineView)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (UpdatePipelineView), new object[1]
      {
        (object) pipelineView
      });
      try
      {
        PipelineViewAclDbAccessor.UpdatePipelineView(pipelineView, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdatePipelineView(UserPipelineView pipelineView)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (UpdatePipelineView), new object[1]
      {
        (object) pipelineView
      });
      try
      {
        if (!PipelineViewAclDbAccessor.UpdatePipelineUserViewInDatabase(pipelineView, this.Session.UserID))
          return;
        this.updateCacheUserPipelineCache();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateUserPipelineViewName(
      UserPipelineView pipelineView,
      string oldViewName)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (UpdateUserPipelineViewName), new object[1]
      {
        (object) pipelineView
      });
      try
      {
        if (!PipelineViewAclDbAccessor.UpdateUserPipelineViewName(pipelineView, oldViewName, this.Session.UserID))
          return;
        this.updateCacheUserPipelineCache();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeletePipelineView(int viewId)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (DeletePipelineView), new object[1]
      {
        (object) viewId
      });
      try
      {
        PipelineViewAclDbAccessor.DeletePipelineView(viewId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteUserCustomPipelineView(int viewId)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (DeleteUserCustomPipelineView), new object[1]
      {
        (object) viewId
      });
      try
      {
        if (!PipelineViewAclDbAccessor.DeleteUserCustomPipelineView(this.Session?.UserID, viewId))
          return;
        this.updateCacheUserPipelineCache();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteUserCustomPipelineView(string userId, string viewName)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (DeleteUserCustomPipelineView), new object[2]
      {
        (object) userId,
        (object) viewName
      });
      try
      {
        PipelineViewAclDbAccessor.DeleteUserCustomPipelineView(userId, viewName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DuplicatePipelineViews(int sourcePersonaID, int targetPersonaID)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (DuplicatePipelineViews), new object[2]
      {
        (object) sourcePersonaID,
        (object) targetPersonaID
      });
      try
      {
        PipelineViewAclDbAccessor.DuplicatePipelineViews(sourcePersonaID, targetPersonaID, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DuplicatePipelineViews(string sourcePersonaID, string targetPersonaID)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), "DuplicateUserPipelineViews", new object[2]
      {
        (object) sourcePersonaID,
        (object) targetPersonaID
      });
      try
      {
        if (!PipelineViewAclDbAccessor.DuplicateUserPipelineViews(sourcePersonaID, targetPersonaID, this.Session.UserID))
          return;
        this.updateCacheUserPipelineCache();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void ReplacePersonaPipelineViews(
      int personaID,
      PersonaPipelineView[] pipelineViews)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (ReplacePersonaPipelineViews), new object[2]
      {
        (object) personaID,
        (object) pipelineViews
      });
      try
      {
        PipelineViewAclDbAccessor.ReplacePersonaPipelineViews(personaID, pipelineViews, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void ReplaceUserPipelineViews(string userID, UserPipelineView[] pipelineViews)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (ReplaceUserPipelineViews), new object[2]
      {
        (object) userID,
        (object) pipelineViews
      });
      try
      {
        if (!PipelineViewAclDbAccessor.ReplaceUserPipelineViews(userID, pipelineViews, this.Session.UserID))
          return;
        this.updateCacheUserPipelineCache();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
      }
    }

    public bool PersonaPipelineViewWithNameExists(int personaID, string viewName)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (PersonaPipelineViewWithNameExists), new object[2]
      {
        (object) personaID,
        (object) viewName
      });
      try
      {
        return PipelineViewAclDbAccessor.PersonaPipelineViewWithNameExists(personaID, viewName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public bool IsDbSaveEnabled()
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (IsDbSaveEnabled), Array.Empty<object>());
      try
      {
        return PipelineViewAclDbAccessor.IsDbSaveEnabled();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public bool UserPipelineViewWithNameExists(string userID, string viewName)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (UserPipelineViewWithNameExists), new object[2]
      {
        (object) userID,
        (object) viewName
      });
      try
      {
        return PipelineViewAclDbAccessor.UserPipelineViewWithNameExists(userID, viewName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public UserPipelineView GetUserPipelineView(string userID, int viewID)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (GetUserPipelineView), new object[2]
      {
        (object) userID,
        (object) viewID
      });
      try
      {
        return PipelineViewAclDbAccessor.GetUserPipelineView(userID, viewID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
        return (UserPipelineView) null;
      }
    }

    public virtual UserPipelineView UpdatePipelineUserView(
      UserPipelineView pipelineView,
      string userId,
      string oldViewName)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (UpdatePipelineUserView), new object[3]
      {
        (object) pipelineView,
        (object) userId,
        (object) oldViewName
      });
      try
      {
        return PipelineViewAclDbAccessor.UpdatePipelineUserView(pipelineView, userId, oldViewName, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
      }
      return (UserPipelineView) null;
    }

    public void DeletePipelineUserViews(string userId, IEnumerable<string> viewIds)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (DeletePipelineUserViews), new object[1]
      {
        (object) viewIds
      });
      try
      {
        if (!PipelineViewAclDbAccessor.DeletePipelineUserViews(userId, viewIds))
          return;
        this.updateCacheUserPipelineCache();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
      }
    }

    public Dictionary<string, string> GetExistingPipelineUserViewIds(
      string userId,
      IEnumerable<string> viewIds)
    {
      this.onApiCalled(nameof (PipelineViewAclManager), nameof (GetExistingPipelineUserViewIds), new object[1]
      {
        (object) userId
      });
      try
      {
        return PipelineViewAclDbAccessor.GetExistingPipelineUserViewIds(userId, viewIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (PipelineViewAclManager), ex, this.Session.SessionInfo);
        return new Dictionary<string, string>();
      }
    }
  }
}
