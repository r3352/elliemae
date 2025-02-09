// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.StringList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  [Serializable]
  public class StringList : ListBase, IStringList
  {
    public StringList()
      : base(typeof (string))
    {
    }

    public StringList(IList source)
      : base(typeof (string), source)
    {
    }

    public string this[int index]
    {
      get => (string) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(string value) => this.List.Add((object) value);

    public bool Contains(string value) => this.List.Contains((object) value);

    public int IndexOf(string value) => this.List.IndexOf((object) value);

    public void Insert(int index, string value) => this.List.Insert(index, (object) value);

    public void Remove(string value) => this.List.Remove((object) value);

    public string[] ToArray()
    {
      string[] array = new string[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
