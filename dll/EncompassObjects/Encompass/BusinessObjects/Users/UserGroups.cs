// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.UserGroups
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.Encompass.Client;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  public class UserGroups : SessionBoundObject, IUserGroups, IEnumerable
  {
    private ArrayList groups;
    private UserGroup allUsers;

    internal UserGroups(Session session)
      : base(session)
    {
    }

    public int Count
    {
      get
      {
        this.ensureLoaded();
        return this.groups.Count;
      }
    }

    public UserGroup this[int index]
    {
      get
      {
        this.ensureLoaded();
        return (UserGroup) this.groups[index];
      }
    }

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

    public IEnumerator GetEnumerator()
    {
      this.ensureLoaded();
      return this.groups.GetEnumerator();
    }

    public UserGroup AllUsers
    {
      get
      {
        this.ensureLoaded();
        return this.allUsers;
      }
    }

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
