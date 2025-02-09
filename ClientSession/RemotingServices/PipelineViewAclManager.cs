// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.PipelineViewAclManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class PipelineViewAclManager : EllieMae.EMLite.RemotingServices.Acl.ManagerBase
  {
    private IPipelineViewAclManager pipelineViewAclMgr;

    internal PipelineViewAclManager(Sessions.Session session)
      : base(session)
    {
      this.pipelineViewAclMgr = (IPipelineViewAclManager) this.session.GetAclManager(AclCategory.PersonaPipelineView);
    }

    public override void ClearCaches(string temp) => this.clearCache(temp);

    public UserPipelineView CreatePipelineUserView(UserPipelineView pipelineView, string userId)
    {
      return this.pipelineViewAclMgr.CreatePipelineUserView(pipelineView, userId);
    }

    public PersonaPipelineView GetPipelineView(int viewId)
    {
      return this.pipelineViewAclMgr.GetPipelineView(viewId);
    }

    public UserPipelineView GetUserCustomPipelineView(int viewId)
    {
      return this.pipelineViewAclMgr.GetUserCustomPipelineView(viewId);
    }

    public PersonaPipelineView GetPersonaPipelineView(int personaID, string viewName)
    {
      return this.pipelineViewAclMgr.GetPersonaPipelineView(personaID, viewName);
    }

    public UserPipelineView GetUserPipelineView(string userID, string viewName)
    {
      return this.pipelineViewAclMgr.GetUserPipelineView(userID, viewName);
    }

    public PersonaPipelineView[] GetPersonaPipelineViews(int personaID)
    {
      return this.pipelineViewAclMgr.GetPersonaPipelineViews(personaID);
    }

    public PersonaPipelineView[] GetUserPipelineViews(string userId)
    {
      return this.pipelineViewAclMgr.GetUserPipelineViews(userId);
    }

    public UserPipelineView[] GetUserCustomPipelineViews(string userId)
    {
      return this.pipelineViewAclMgr.GetUserCustomPipelineViews(userId);
    }

    public PersonaPipelineView CreatePipelineView(PersonaPipelineView pipelineView)
    {
      return this.pipelineViewAclMgr.CreatePipelineView(pipelineView);
    }

    public void UpdatePipelineView(PersonaPipelineView pipelineView)
    {
      this.pipelineViewAclMgr.UpdatePipelineView(pipelineView);
    }

    public void DeletePipelineView(int viewId)
    {
      this.pipelineViewAclMgr.DeletePipelineView(viewId);
    }

    public void DeleteUserCustomPipelineView(int viewId)
    {
      this.pipelineViewAclMgr.DeleteUserCustomPipelineView(viewId);
    }

    public void DeleteUserCustomPipelineView(string userId, string viewName)
    {
      this.pipelineViewAclMgr.DeleteUserCustomPipelineView(userId, viewName);
    }

    public void DuplicatePipelineViews(int sourcePersonaID, int targetPersonaID)
    {
      this.pipelineViewAclMgr.DuplicatePipelineViews(sourcePersonaID, targetPersonaID);
    }

    public void ReplacePersonaPipelineViews(int personaID, PersonaPipelineView[] pipelineViews)
    {
      this.pipelineViewAclMgr.ReplacePersonaPipelineViews(personaID, pipelineViews);
    }

    public void ReplaceUserPipelineViews(string userID, UserPipelineView[] pipelineViews)
    {
      this.pipelineViewAclMgr.ReplaceUserPipelineViews(userID, pipelineViews);
    }

    public void UpdateUserPipelineViewName(UserPipelineView view, string oldViewName)
    {
      this.pipelineViewAclMgr.UpdateUserPipelineViewName(view, oldViewName);
    }

    public UserPipelineView UpdatePipelineUserView(
      UserPipelineView pipelineView,
      string userId,
      string oldViewName)
    {
      return this.pipelineViewAclMgr.UpdatePipelineUserView(pipelineView, userId, oldViewName);
    }

    public void UpdatePipelineView(UserPipelineView pipelineView)
    {
      this.pipelineViewAclMgr.UpdatePipelineView(pipelineView);
    }
  }
}
