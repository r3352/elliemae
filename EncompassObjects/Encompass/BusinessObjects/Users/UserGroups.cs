// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.UserGroups
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.Encompass.Client;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>Summary description for UserGroups.</summary>
  public class UserGroups : SessionBoundObject, IUserGroups, IEnumerable
  {
    private ArrayList groups;
    private UserGroup allUsers;

    internal UserGroups(Session session)
      : base(session)
    {
    }

    /// <summary>
    /// Returns the number of <see cref="T:EllieMae.Encompass.BusinessObjects.Users.UserGroup" /> objects defined in Encompass.
    /// </summary>
    public int Count
    {
      get
      {
        this.ensureLoaded();
        return this.groups.Count;
      }
    }

    /// <summary>
    /// Retrieves a <see cref="T:EllieMae.Encompass.BusinessObjects.Users.UserGroup" /> by index from the collection.
    /// </summary>
    public UserGroup this[int index]
    {
      get
      {
        this.ensureLoaded();
        return (UserGroup) this.groups[index];
      }
    }

    /// <summary>Retrieves a User Group using it's unique ID.</summary>
    /// <param name="groupId">The ID of the group to be retrieved.</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.UserGroup" /> with the specified ID, or <c>null</c>
    /// if no group with the specified ID is found.</returns>
    public UserGroup GetGroupByID(int groupId)
    {
      this.ensureLoaded();
      foreach (UserGroup group in this.groups)
      {
        if (group.ID == groupId)
          return group;
      }
      return (UserGroup) null;
    }

    /// <summary>Retrieves a User Group using its name.</summary>
    /// <param name="groupName">The name of the desired group.</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.UserGroup" /> with the specified name, or <c>null</c>
    /// if no group with the specified name is found.</returns>
    public UserGroup GetGroupByName(string groupName)
    {
      this.ensureLoaded();
      foreach (UserGroup group in this.groups)
      {
        if (string.Compare(group.Name, groupName, true) == 0)
          return group;
      }
      return (UserGroup) null;
    }

    /// <summary>
    /// Returns an enumerator for iterating over the groups in the collection.
    /// </summary>
    /// <returns>Returns an enumerator for the collection.</returns>
    public IEnumerator GetEnumerator()
    {
      this.ensureLoaded();
      return this.groups.GetEnumerator();
    }

    /// <summary>Returns the "All Users" group.</summary>
    /// <remarks>This group automatically includes every user in Encompass and cannot
    /// have its membership modified.</remarks>
    public UserGroup AllUsers
    {
      get
      {
        this.ensureLoaded();
        return this.allUsers;
      }
    }

    /// <summary>
    /// Refreshes the UserGroup information to ensure it reflects what is in the Encompass
    /// system.
    /// </summary>
    public void Refresh() => this.groups = (ArrayList) null;

    private void ensureLoaded()
    {
      if (this.groups != null)
        return;
      this.groups = new ArrayList();
      foreach (AclGroup allGroup in ((IAclGroupManager) this.Session.GetObject("AclGroupManager")).GetAllGroups())
        this.groups.Add((object) new UserGroup(this.Session, allGroup));
      foreach (UserGroup group in this.groups)
      {
        if (group.Name == "All Users")
        {
          this.allUsers = group;
          break;
        }
      }
    }
  }
}
