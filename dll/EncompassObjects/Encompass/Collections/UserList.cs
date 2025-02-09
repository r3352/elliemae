// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.UserList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Users;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class UserList : ListBase, IUserList
  {
    public UserList()
      : base(typeof (User))
    {
    }

    public UserList(IList source)
      : base(typeof (User), source)
    {
    }

    public User this[int index]
    {
      get => (User) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(User value) => this.List.Add((object) value);

    public bool Contains(User value) => this.List.Contains((object) value);

    public int IndexOf(User value) => this.List.IndexOf((object) value);

    public void Insert(int index, User value) => this.List.Insert(index, (object) value);

    public void Remove(User value) => this.List.Remove((object) value);

    public User[] ToArray()
    {
      User[] array = new User[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
