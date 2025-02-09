// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.TrackedItemList
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System.Collections;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class TrackedItemList : IEnumerable
  {
    private ArrayList data = new ArrayList();
    private bool isDirty;

    public void Add(object o)
    {
      this.data.Add(o);
      this.isDirty = true;
    }

    public void Clear()
    {
      this.data.Clear();
      this.isDirty = true;
    }

    public bool IsDirty
    {
      get => this.isDirty;
      set => this.isDirty = value;
    }

    public int Count => this.data.Count;

    public object this[int index] => this.data[index];

    public IEnumerator GetEnumerator() => this.data.GetEnumerator();
  }
}
