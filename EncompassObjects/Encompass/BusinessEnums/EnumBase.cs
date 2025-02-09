// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.EnumBase
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>
  /// Provides a base class for all Business Enumerations in the Encompass Object Model.
  /// Not meant for use outside of the Encompass API.
  /// </summary>
  [ComVisible(false)]
  public abstract class EnumBase : IEnumerable, ICollection
  {
    private ArrayList items = new ArrayList();
    private IComparer comparer;

    /// <summary>Constructor</summary>
    internal EnumBase()
      : this((IComparer) new CaseInsensitiveComparer())
    {
    }

    /// <summary>EnumBase comparer</summary>
    /// <param name="comparer"></param>
    internal EnumBase(IComparer comparer) => this.comparer = comparer;

    /// <summary>Adds an item to the list</summary>
    /// <param name="item"></param>
    protected internal void AddItem(EnumItem item) => this.items.Add((object) item);

    /// <summary>GetItem</summary>
    /// <param name="index"></param>
    /// <returns></returns>
    protected internal EnumItem GetItem(int index) => (EnumItem) this.items[index];

    /// <summary>GetItemByID</summary>
    /// <param name="id"></param>
    /// <returns></returns>
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

    /// <summary>GetItemByName</summary>
    /// <param name="name"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Returns an enumerator for iterating over the items in the list.
    /// </summary>
    /// <returns>An IEnumerator object for the list.</returns>
    public IEnumerator GetEnumerator() => this.items.GetEnumerator();

    /// <summary>
    /// Gets a flag indicating if access to the object is synchronized across threads.
    /// </summary>
    public bool IsSynchronized => this.items.IsSynchronized;

    /// <summary>Returns the number of items in the list.</summary>
    public int Count => this.items.Count;

    /// <summary>
    /// Copies the items in the list into an Array, starting at the specified index.
    /// </summary>
    /// <param name="array">The array into which to copy the list's contents. The
    /// array must be able to hold objects of the appropriate type.</param>
    /// <param name="index">The index of the array at which copying is started.</param>
    public void CopyTo(Array array, int index) => this.items.CopyTo(array, index);

    /// <summary>
    /// An object to use for synchronized access to this list.
    /// </summary>
    public object SyncRoot => this.items.SyncRoot;
  }
}
