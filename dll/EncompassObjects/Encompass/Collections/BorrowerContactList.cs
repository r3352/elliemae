// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.BorrowerContactList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Contacts;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class BorrowerContactList : ListBase, IBorrowerContactList
  {
    public BorrowerContactList()
      : base(typeof (BorrowerContact))
    {
    }

    public BorrowerContactList(IList source)
      : base(typeof (BorrowerContact), source)
    {
    }

    public BorrowerContact this[int index]
    {
      get => (BorrowerContact) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(BorrowerContact value) => this.List.Add((object) value);

    public bool Contains(BorrowerContact value) => this.List.Contains((object) value);

    public int IndexOf(BorrowerContact value) => this.List.IndexOf((object) value);

    public void Insert(int index, BorrowerContact value) => this.List.Insert(index, (object) value);

    public void Remove(BorrowerContact value) => this.List.Remove((object) value);

    public BorrowerContact[] ToArray()
    {
      BorrowerContact[] array = new BorrowerContact[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
