// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.UserGroup
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>
  /// Represents a User Group within Encompass, which can be used to assign access rights to users.
  /// </summary>
  /// <remarks>User Groups are used to assign a single set of access rights to group of users,
  /// eliminating the need to assign rights on a user-by-user basis.</remarks>
  public class UserGroup : SessionBoundObject, IUserGroup
  {
    internal const string AllUsersGroupName = "All Users�";
    private AclGroup group;

    internal UserGroup(Session session, AclGroup group)
      : base(session)
    {
      this.group = group;
    }

    /// <summary>Gets the unique identifier for the user group.</summary>
    public int ID => this.group.ID;

    /// <summary>Returns the name of the user group.</summary>
    public string Name => this.group.Name;

    /// <summary>Returns a collection of all users in the group.</summary>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Collections.UserList" /> containing the <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User">Users</see>
    /// associated with this group. This includes both users directly assigned to the group as
    /// well as users who belong to oganizations assigned to the group.</returns>
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

    /// <summary>Adds a user to the group.</summary>
    /// <param name="userToAdd">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User" /> to be added.</param>
    public void AddUser(User userToAdd)
    {
      if (userToAdd.IsNew)
        throw new InvalidOperationException("The User must be commited prior to adding it to a User Group");
      if (!(this.Name != "All Users"))
        return;
      this.GroupManager.AddUserToGroup(this.ID, userToAdd.ID);
    }

    /// <summary>
    /// Adds a branch of the organization hierarchy to the User Group.
    /// </summary>
    /// <param name="orgToAdd">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Organization" /> to be added to the group.</param>
    /// <param name="includeChildren">Indicates if the children of the specified organization
    /// are also included in the group.</param>
    public void AddOrganization(Organization orgToAdd, bool includeChildren)
    {
      this.ensureNotAllUsers();
      if (orgToAdd.IsNew)
        throw new InvalidOperationException("The Organization must be commited prior to adding it to a User Group");
      this.GroupManager.AddOrgToGroup(this.ID, orgToAdd.ID, includeChildren);
    }

    /// <summary>Removes a user from the User Group.</summary>
    /// <param name="userToRemove">The user to be removed.</param>
    /// <remarks>Note that if a user is included in a group implicitly (i.e. based on their
    /// place in the organization hierarchy), this method will not remove them from the group.
    /// This method will remove users who were explicitly added to the group only.</remarks>
    public void RemoveUser(User userToRemove)
    {
      this.ensureNotAllUsers();
      if (userToRemove.IsNew)
        return;
      this.GroupManager.DeleteUserFromGroup(this.ID, userToRemove.ID);
    }

    /// <summary>Removes the specified organization from the group.</summary>
    /// <param name="orgToRemove">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Organization" /> to be removed.</param>
    /// <remarks>If the specified organization was added such that all of its children
    /// are also included in the group, removing the organization will also remove all of
    /// its children.</remarks>
    public void RemoveOrganization(Organization orgToRemove)
    {
      this.ensureNotAllUsers();
      if (orgToRemove.IsNew)
        return;
      this.GroupManager.DeleteOrgFromGroup(this.ID, orgToRemove.ID);
    }

    /// <summary>Provides a string representation of the object.</summary>
    /// <returns>Returns the <see cref="P:EllieMae.Encompass.BusinessObjects.Users.UserGroup.Name" /> of the group.</returns>
    public override string ToString() => this.Name;

    /// <summary>
    /// Determines if two UserGroup objects represent the same group.
    /// </summary>
    /// <param name="obj">The UserGroup against which to compare.</param>
    /// <returns>Returns <c>true</c> if the two objects represent the same group,
    /// <c>false</c> otherwise.</returns>
    public override bool Equals(object obj)
    {
      return obj is UserGroup userGroup && this.ID == userGroup.ID;
    }

    /// <summary>Provides a hash code for the group.</summary>
    /// <returns>Returns the <see cref="P:EllieMae.Encompass.BusinessObjects.Users.UserGroup.ID" /> of the current group.</returns>
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
