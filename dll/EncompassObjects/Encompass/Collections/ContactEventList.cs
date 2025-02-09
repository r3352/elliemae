// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ContactEventList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Contacts;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class ContactEventList : ListBase, IContactEventList
  {
    public ContactEventList()
      : base(typeof (ContactEvent))
    {
    }

    public ContactEventList(IList source)
      : base(typeof (ContactEvent), source)
    {
    }

    public ContactEvent this[int index]
    {
      get => (ContactEvent) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(ContactEvent value) => this.List.Add((object) value);

    public bool Contains(ContactEvent value) => this.List.Contains((object) value);

    public int IndexOf(ContactEvent value) => this.List.IndexOf((object) value);

    public void Insert(int index, ContactEvent value) => this.List.Insert(index, (object) value);

    public void Remove(ContactEvent value) => this.List.Remove((object) value);

    public ContactEvent[] ToArray()
    {
      ContactEvent[] array = new ContactEvent[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
