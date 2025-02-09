// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.SortCriterionList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.Encompass.Query;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class SortCriterionList : ListBase, ISortCriterionList
  {
    public SortCriterionList()
      : base(typeof (SortCriterion))
    {
    }

    public SortCriterionList(IList source)
      : base(typeof (SortCriterion), source)
    {
    }

    public SortCriterion this[int index]
    {
      get => (SortCriterion) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(SortCriterion value) => this.List.Add((object) value);

    public bool Contains(SortCriterion value) => this.List.Contains((object) value);

    public int IndexOf(SortCriterion value) => this.List.IndexOf((object) value);

    public void Insert(int index, SortCriterion value) => this.List.Insert(index, (object) value);

    public void Remove(SortCriterion value) => this.List.Remove((object) value);

    public SortCriterion[] ToArray()
    {
      SortCriterion[] array = new SortCriterion[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }

    internal SortField[] ToSortFieldList()
    {
      SortField[] sortFieldList = new SortField[this.Count];
      for (int index = 0; index < this.Count; ++index)
        sortFieldList[index] = this[index].Unwrap();
      return sortFieldList;
    }
  }
}
