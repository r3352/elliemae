// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.RoleUserGroups
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Client;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class RoleUserGroups : IRoleUserGroups, IEnumerable
  {
    private List<UserGroup> userGroups = new List<UserGroup>();

    internal RoleUserGroups(Session session, int[] userGroupIds)
    {
      if (userGroupIds == null)
        return;
      foreach (int userGroupId in userGroupIds)
      {
        UserGroup groupById = session.Users.Groups.GetGroupByID(userGroupId);
        if (groupById != null)
          this.userGroups.Add(groupById);
      }
    }

    public int Count => this.userGroups.Count;

    public UserGroup this[int index] => this.userGroups[index];

    public IEnumerator GetEnumerator() => (IEnumerator) this.userGroups.GetEnumerator();
  }
}
