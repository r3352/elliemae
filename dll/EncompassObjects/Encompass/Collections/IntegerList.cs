// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IntegerList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  [Serializable]
  public class IntegerList : ListBase, IIntegerList
  {
    public IntegerList()
      : base(typeof (int))
    {
    }

    public IntegerList(IList source)
      : base(typeof (int), source)
    {
    }

    public int this[int index]
    {
      get => (int) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(int value) => this.List.Add((object) value);

    public bool Contains(int value) => this.List.Contains((object) value);

    public int IndexOf(int value) => this.List.IndexOf((object) value);

    public void Insert(int index, int value) => this.List.Insert(index, (object) value);

    public void Remove(int value) => this.List.Remove((object) value);

    public int[] ToArray()
    {
      int[] array = new int[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
