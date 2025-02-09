// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Collections.IntegerList
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Collections
{
  [Serializable]
  public class IntegerList : ListBase
  {
    public IntegerList()
      : this(false)
    {
    }

    public IntegerList(bool requireUnique)
      : base(typeof (int), requireUnique)
    {
    }

    public IntegerList(IList source)
      : this(false, source)
    {
    }

    public IntegerList(bool requireUnique, IList source)
      : base(typeof (int), requireUnique, source)
    {
    }

    public int this[int index] => (int) this.List[index];

    public void Add(int value) => this.Add((object) value);

    public new void AddRange(IEnumerable values) => base.AddRange(values);

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
