// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ExternalNotesList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  /// <summary>ExternalNotesList class</summary>
  public class ExternalNotesList : ListBase, IExternalNotesList
  {
    /// <summary>Constructs a new, empty list.</summary>
    public ExternalNotesList()
      : base(typeof (ExternalNote))
    {
    }

    /// <summary>
    /// Constructs a list initialized from the specified source.
    /// </summary>
    /// <param name="source">The list of items copied into the new object.</param>
    public ExternalNotesList(IList source)
      : base(typeof (ExternalNote), source)
    {
    }

    /// <summary>
    /// Provides access to an item from the list using its index within the list.
    /// </summary>
    public ExternalNote this[int index]
    {
      get => (ExternalNote) this.List[index];
      set => this.List[index] = (object) value;
    }

    /// <summary>Adds an item to the list.</summary>
    /// <param name="value">The item to be added.</param>
    public void Add(ExternalNote value) => this.List.Add((object) value);

    /// <summary>Determines if the list contains the specified item.</summary>
    /// <param name="value">The item to search for in the list.</param>
    /// <returns>Returns a boolean indication if the specified item is in the list.</returns>
    public bool Contains(ExternalNote value) => this.List.Contains((object) value);

    /// <summary>Removes an item from the list.</summary>
    /// <param name="value">The item to be removed from the list.</param>
    public void Remove(ExternalNote value) => this.List.Remove((object) value);

    /// <summary>Converts the list to an Array.</summary>
    /// <returns>An array containing the items from the list.</returns>
    public ExternalNote[] ToArray()
    {
      ExternalNote[] array = new ExternalNote[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
