// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.UserGroupList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Users;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class UserGroupList : ListBase, IUserGroupList
  {
    public UserGroupList()
      : base(typeof (UserGroup))
    {
    }

    public UserGroupList(IList source)
      : base(typeof (UserGroup), source)
    {
    }

    public UserGroup this[int index]
    {
      get => (UserGroup) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(UserGroup value) => this.List.Add((object) value);

    public bool Contains(UserGroup value) => this.List.Contains((object) value);

    public int IndexOf(UserGroup value) => this.List.IndexOf((object) value);

    public void Insert(int index, UserGroup value) => this.List.Insert(index, (object) value);

    public void Remove(UserGroup value) => this.List.Remove((object) value);

    public UserGroup[] ToArray()
    {
      UserGroup[] array = new UserGroup[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
