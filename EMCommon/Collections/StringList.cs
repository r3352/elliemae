// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Collections.StringList
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Collections
{
  [Serializable]
  public class StringList : ListBase
  {
    public StringList()
      : this(false)
    {
    }

    public StringList(bool requireUnique)
      : base(typeof (string), requireUnique)
    {
    }

    public StringList(IList source)
      : this(false, source)
    {
    }

    public StringList(bool requireUnique, IList source)
      : base(typeof (string), requireUnique, source)
    {
    }

    public string this[int index] => (string) this.List[index];

    public void Add(string value) => this.Add((object) value);

    public new void AddRange(IEnumerable values) => base.AddRange(values);

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
