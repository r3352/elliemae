// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ContactOpportunityList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Contacts;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class ContactOpportunityList : ListBase, IContactOpportunityList
  {
    public ContactOpportunityList()
      : base(typeof (ContactOpportunity))
    {
    }

    public ContactOpportunityList(IList source)
      : base(typeof (ContactOpportunity), source)
    {
    }

    public ContactOpportunity this[int index]
    {
      get => (ContactOpportunity) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(ContactOpportunity value) => this.List.Add((object) value);

    public bool Contains(ContactOpportunity value) => this.List.Contains((object) value);

    public int IndexOf(ContactOpportunity value) => this.List.IndexOf((object) value);

    public void Insert(int index, ContactOpportunity value)
    {
      this.List.Insert(index, (object) value);
    }

    public void Remove(ContactOpportunity value) => this.List.Remove((object) value);

    public ContactOpportunity[] ToArray()
    {
      ContactOpportunity[] array = new ContactOpportunity[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
