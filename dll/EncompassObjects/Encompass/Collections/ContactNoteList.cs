// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ContactNoteList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Contacts;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class ContactNoteList : ListBase, IContactNoteList
  {
    public ContactNoteList()
      : base(typeof (ContactNote))
    {
    }

    public ContactNoteList(IList source)
      : base(typeof (ContactNote), source)
    {
    }

    public ContactNote this[int index]
    {
      get => (ContactNote) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(ContactNote value) => this.List.Add((object) value);

    public bool Contains(ContactNote value) => this.List.Contains((object) value);

    public int IndexOf(ContactNote value) => this.List.IndexOf((object) value);

    public void Insert(int index, ContactNote value) => this.List.Insert(index, (object) value);

    public void Remove(ContactNote value) => this.List.Remove((object) value);

    public ContactNote[] ToArray()
    {
      ContactNote[] array = new ContactNote[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
