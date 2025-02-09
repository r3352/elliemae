// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ListBase
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  /// <summary>
  /// Provides a base class for all of the strongly-typed lists in the
  /// EllieMae.Encompass.Collections namespace.
  /// </summary>
  [ComVisible(false)]
  public abstract class ListBase : CollectionBase
  {
    private Type listType;

    /// <summary>Constructor to initialize class variables</summary>
    /// <param name="listType"></param>
    protected internal ListBase(Type listType) => this.listType = listType;

    /// <summary>Constructor to initialize class variables.</summary>
    /// <param name="listType"></param>
    /// <param name="source"></param>
    protected internal ListBase(Type listType, IList source)
      : this(listType)
    {
      for (int index = 0; index < source.Count; ++index)
        this.List.Add(source[index]);
    }

    /// <summary>Returns an item from the list</summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public object GetItemAt(int index) => this.List[index];

    /// <summary>Sorts the objects in the list.</summary>
    /// <param name="comparer">An IComparer implementation for performing the sort.</param>
    public void Sort(IComparer comparer) => this.InnerList.Sort(comparer);

    /// <summary>Overrides of methods.</summary>
    /// <param name="value"></param>
    protected override void OnValidate(object value)
    {
      base.OnValidate(value);
      this.validateType(value);
    }

    /// <summary>Ensures an object is of a specified type.</summary>
    /// <param name="o"></param>
    protected void validateType(object o)
    {
      if (o != null && !this.listType.IsAssignableFrom(o.GetType()))
        throw new ApplicationException("Invalid object type for list.");
    }
  }
}
