// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.OrganizationList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Users;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class OrganizationList : ListBase, IOrganizationList
  {
    public OrganizationList()
      : base(typeof (Organization))
    {
    }

    public OrganizationList(IList source)
      : base(typeof (Organization), source)
    {
    }

    public Organization this[int index]
    {
      get => (Organization) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(Organization value) => this.List.Add((object) value);

    public bool Contains(Organization value) => this.List.Contains((object) value);

    public int IndexOf(Organization value) => this.List.IndexOf((object) value);

    public void Insert(int index, Organization value) => this.List.Insert(index, (object) value);

    public void Remove(Organization value) => this.List.Remove((object) value);

    public Organization[] ToArray()
    {
      Organization[] array = new Organization[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
