// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ContactNoteList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Contacts;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  /// <summary>
  /// Represents a list of <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.ContactNote">ContactNote</see>
  /// objects.
  /// </summary>
  public class ContactNoteList : ListBase, IContactNoteList
  {
    /// <summary>Constructs a new, empty list.</summary>
    public ContactNoteList()
      : base(typeof (ContactNote))
    {
    }

    /// <summary>
    /// Constructs a list initialized from the specified source.
    /// </summary>
    /// <param name="source">The list of items copied into the new object.</param>
    public ContactNoteList(IList source)
      : base(typeof (ContactNote), source)
    {
    }

    /// <summary>
    /// Provides access to an item from the list using its index within the list.
    /// </summary>
    public ContactNote this[int index]
    {
      get => (ContactNote) this.List[index];
      set => this.List[index] = (object) value;
    }

    /// <summary>Adds an item to the list.</summary>
    /// <param name="value">The item to be added.</param>
    public void Add(ContactNote value) => this.List.Add((object) value);

    /// <summary>Determines if the list contains the specified item.</summary>
    /// <param name="value">The item to search for in the list.</param>
    /// <returns>Returns a boolean indication if the specified item is in the list.</returns>
    public bool Contains(ContactNote value) => this.List.Contains((object) value);

    /// <summary>Returns the index of the specified item.</summary>
    /// <param name="value">The value to search for in the list.</param>
    /// <returns>The index of the specified item, or -1 if not found.</returns>
    public int IndexOf(ContactNote value) => this.List.IndexOf((object) value);

    /// <summary>
    /// Inserts a new item into the list at the specified index.
    /// </summary>
    /// <param name="index">The index at which the item will be inserted.</param>
    /// <param name="value">The item to be inserted.</param>
    public void Insert(int index, ContactNote value) => this.List.Insert(index, (object) value);

    /// <summary>Removes an item from the list.</summary>
    /// <param name="value">The item to be removed from the list.</param>
    public void Remove(ContactNote value) => this.List.Remove((object) value);

    /// <summary>Converts the list to an Array.</summary>
    /// <returns>An array containing the items from the list.</returns>
    public ContactNote[] ToArray()
    {
      ContactNote[] array = new ContactNote[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
