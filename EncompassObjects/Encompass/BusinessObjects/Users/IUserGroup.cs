// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.IUserGroup
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>Interface for UserGroup class</summary>
  /// <exclude />
  [Guid("795B2252-76B3-4c7f-B51C-E822612D5037")]
  public interface IUserGroup
  {
    int ID { get; }

    string Name { get; }

    UserList GetUsers();

    void AddUser(User userToAdd);

    void RemoveUser(User userToRemove);

    void AddOrganization(Organization orgToAdd, bool includeChildren);

    void RemoveOrganization(Organization orgToRemove);
  }
}
