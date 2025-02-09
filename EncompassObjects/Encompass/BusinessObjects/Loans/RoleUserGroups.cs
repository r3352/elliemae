// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.RoleUserGroups
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Client;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Provides the collection or <see cref="T:EllieMae.Encompass.BusinessObjects.Users.UserGroup" /> objects associated with a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" />.
  /// </summary>
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

    /// <summary>Gets the number of UserGroups in the collection</summary>
    public int Count => this.userGroups.Count;

    /// <summary>Returns a UserGroup from the collection by index.</summary>
    /// <param name="index">The index of the desired UserGroup.</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.UserGroup" /> at the specified index in the collection.</returns>
    public UserGroup this[int index] => this.userGroups[index];

    /// <summary>Provides a enumerator for the collection.</summary>
    /// <returns>Returns an IEnumerator for enumerating the collection.</returns>
    public IEnumerator GetEnumerator() => (IEnumerator) this.userGroups.GetEnumerator();
  }
}
