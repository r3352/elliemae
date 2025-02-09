// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.BorrowerPairList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class BorrowerPairList : ListBase, IBorrowerPairList
  {
    public BorrowerPairList()
      : base(typeof (BorrowerPair))
    {
    }

    public BorrowerPairList(IList source)
      : base(typeof (BorrowerPair), source)
    {
    }

    public BorrowerPair this[int index]
    {
      get => (BorrowerPair) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(BorrowerPair value) => this.List.Add((object) value);

    public bool Contains(BorrowerPair value) => this.List.Contains((object) value);

    public int IndexOf(BorrowerPair value) => this.List.IndexOf((object) value);

    public void Insert(int index, BorrowerPair value) => this.List.Insert(index, (object) value);

    public void Remove(BorrowerPair value) => this.List.Remove((object) value);

    public BorrowerPair[] ToArray()
    {
      BorrowerPair[] array = new BorrowerPair[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
