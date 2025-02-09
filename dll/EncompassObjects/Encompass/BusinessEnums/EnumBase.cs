// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.EnumBase
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  [ComVisible(false)]
  public abstract class EnumBase : IEnumerable, ICollection
  {
    private ArrayList items = new ArrayList();
    private IComparer comparer;

    internal EnumBase()
      : this((IComparer) new CaseInsensitiveComparer())
    {
    }

    internal EnumBase(IComparer comparer) => this.comparer = comparer;

    protected internal void AddItem(EnumItem item) => this.items.Add((object) item);

    protected internal EnumItem GetItem(int index) => (EnumItem) this.items[index];

    protected internal EnumItem GetItemByID(int id)
    {
      for (int index = 0; index < this.items.Count; ++index)
      {
        EnumItem itemById = (EnumItem) this.items[index];
        if (itemById.ID == id)
          return itemById;
      }
      return (EnumItem) null;
    }

    protected internal EnumItem GetItemByName(string name)
    {
      for (int index = 0; index < this.items.Count; ++index)
      {
        EnumItem itemByName = (EnumItem) this.items[index];
        if (this.comparer.Compare((object) itemByName.Name, (object) name) == 0)
          return itemByName;
      }
      return (EnumItem) null;
    }

    public IEnumerator GetEnumerator() => this.items.GetEnumerator();

    public bool IsSynchronized => this.items.IsSynchronized;

    public int Count => this.items.Count;

    public void CopyTo(Array array, int index) => this.items.CopyTo(array, index);

    public object SyncRoot => this.items.SyncRoot;
  }
}
