// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.UserGroup
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  public class UserGroup : SessionBoundObject, IUserGroup
  {
    internal const string AllUsersGroupName = "All Users�";
    private AclGroup group;

    internal UserGroup(Session session, AclGroup group)
      : base(session)
    {
      this.group = group;
    }

    public int ID => this.group.ID;

    public string Name => this.group.Name;

    public UserList GetUsers()
    {
      string[] usersInGroup = this.GroupManager.GetUsersInGroup(this.group.ID, true);
      UserList users = new UserList();
      foreach (string userId in usersInGroup)
      {
        User user = this.Session.Users.GetUser(userId);
        if (user != null)
          users.Add(user);
      }
      return users;
    }

    public void AddUser(User userToAdd)
    {
      if (userToAdd.IsNew)
        throw new InvalidOperationException("The User must be commited prior to adding it to a User Group");
      if (!(this.Name != "All Users"))
        return;
      this.GroupManager.AddUserToGroup(this.ID, userToAdd.ID);
    }

    public void AddOrganization(Organization orgToAdd, bool includeChildren)
    {
      this.ensureNotAllUsers();
      if (orgToAdd.IsNew)
        throw new InvalidOperationException("The Organization must be commited prior to adding it to a User Group");
      this.GroupManager.AddOrgToGroup(this.ID, orgToAdd.ID, includeChildren);
    }

    public void RemoveUser(User userToRemove)
    {
      this.ensureNotAllUsers();
      if (userToRemove.IsNew)
        return;
      this.GroupManager.DeleteUserFromGroup(this.ID, userToRemove.ID);
    }

    public void RemoveOrganization(Organization orgToRemove)
    {
      this.ensureNotAllUsers();
      if (orgToRemove.IsNew)
        return;
      this.GroupManager.DeleteOrgFromGroup(this.ID, orgToRemove.ID);
    }

    public override string ToString() => this.Name;

    public override bool Equals(object obj)
    {
      return obj is UserGroup userGroup && this.ID == userGroup.ID;
    }

    public override int GetHashCode() => this.ID;

    internal AclGroup Unwrap() => this.group;

    private IAclGroupManager GroupManager
    {
      get => (IAclGroupManager) this.Session.GetObject("AclGroupManager");
    }

    private void ensureNotAllUsers()
    {
      if (this.Name == "All Users")
        throw new InvalidOperationException("The specified operation cannot be performed on the All Users group");
    }
  }
}
