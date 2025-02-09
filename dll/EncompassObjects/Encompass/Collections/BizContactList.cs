// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.BizContactList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Contacts;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class BizContactList : ListBase, IBizContactList
  {
    public BizContactList()
      : base(typeof (BizContact))
    {
    }

    public BizContactList(IList source)
      : base(typeof (BizContact), source)
    {
    }

    public BizContact this[int index]
    {
      get => (BizContact) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(BizContact value) => this.List.Add((object) value);

    public bool Contains(BizContact value) => this.List.Contains((object) value);

    public int IndexOf(BizContact value) => this.List.IndexOf((object) value);

    public void Insert(int index, BizContact value) => this.List.Insert(index, (object) value);

    public void Remove(BizContact value) => this.List.Remove((object) value);

    public BizContact[] ToArray()
    {
      BizContact[] array = new BizContact[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
