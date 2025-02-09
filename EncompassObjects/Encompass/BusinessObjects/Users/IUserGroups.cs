// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.IUserGroups
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>Interface for UserGroups class</summary>
  /// <exclude />
  [Guid("0BCA99A5-EBE5-4a46-8F10-D5FD023960FC")]
  public interface IUserGroups
  {
    int Count { get; }

    UserGroup this[int index] { get; }

    UserGroup GetGroupByID(int groupId);

    UserGroup GetGroupByName(string groupName);

    IEnumerator GetEnumerator();

    UserGroup AllUsers { get; }

    void Refresh();
  }
}
