// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IUserGroupList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Users;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  /// <summary>Interface for UserGroupList class.</summary>
  /// <exclude />
  [Guid("6A6A7702-A5FB-4012-B3A9-FC52EF7A1FC3")]
  public interface IUserGroupList
  {
    UserGroup this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(UserGroup value);

    bool Contains(UserGroup value);

    int IndexOf(UserGroup value);

    void Insert(int index, UserGroup value);

    void Remove(UserGroup value);

    UserGroup[] ToArray();

    IEnumerator GetEnumerator();
  }
}
