// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IPipelineViewAclManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IPipelineViewAclManager
  {
    PersonaPipelineView GetPipelineView(int viewId);

    PersonaPipelineView GetPersonaPipelineView(int personaID, string viewName);

    PersonaPipelineView[] GetPersonaPipelineViews(int personaID);

    PersonaPipelineView[] GetUserPipelineViews(string userId);

    PersonaPipelineView CreatePipelineView(PersonaPipelineView pipelineView);

    void UpdatePipelineView(PersonaPipelineView pipelineView);

    void UpdatePipelineView(UserPipelineView pipelineView);

    void ReplacePersonaPipelineViews(int personaID, PersonaPipelineView[] pipelineViews);

    void DeletePipelineView(int viewId);

    void DuplicatePipelineViews(int sourcePersonaID, int targetPersonaID);

    UserPipelineView[] GetUserCustomPipelineViews(string userId);

    void ReplaceUserPipelineViews(string userID, UserPipelineView[] pipelineViews);

    void DeleteUserCustomPipelineView(int viewId);

    void DeleteUserCustomPipelineView(string userId, string viewName);

    UserPipelineView GetUserCustomPipelineView(int viewId);

    UserPipelineView GetUserPipelineView(string userID, string viewName);

    void UpdateUserPipelineViewName(UserPipelineView view, string oldViewName);

    bool PersonaPipelineViewWithNameExists(int personaID, string viewName);

    bool IsDbSaveEnabled();

    UserPipelineView CreatePipelineUserView(UserPipelineView pipelineView, string userId);

    bool UserPipelineViewWithNameExists(string userID, string viewName);

    UserPipelineView GetUserPipelineView(string userID, int viewID);

    UserPipelineView UpdatePipelineUserView(
      UserPipelineView pipelineView,
      string userId,
      string oldViewName);

    void DeletePipelineUserViews(string userId, IEnumerable<string> viewIds);

    Dictionary<string, string> GetExistingPipelineUserViewIds(
      string userId,
      IEnumerable<string> viewIds);
  }
}
