// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.DropdownOptionCollection
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Represents an ordered collection of <see cref="T:EllieMae.Encompass.Forms.DropdownOption" /> objects associated with a
  /// list-based control.
  /// </summary>
  public class DropdownOptionCollection : IEnumerable
  {
    private IDropdownOptionContainer parentControl;
    private bool allowEmptyValues = true;

    internal DropdownOptionCollection(IDropdownOptionContainer parentControl)
      : this(parentControl, true)
    {
    }

    internal DropdownOptionCollection(IDropdownOptionContainer parentControl, bool allowEmptyValues)
    {
      this.parentControl = parentControl;
      this.allowEmptyValues = allowEmptyValues;
    }

    /// <summary>
    /// Returns a <see cref="T:EllieMae.Encompass.Forms.DropdownOption" /> based on its index in the list.
    /// </summary>
    public DropdownOption this[int index] => (DropdownOption) this.getOptionsList()[index];

    /// <summary>Removes the DropdownOptions at the specified index.</summary>
    /// <param name="index">The index of the option to be removed.</param>
    public void RemoveAt(int index)
    {
      ArrayList optionsList = this.getOptionsList();
      optionsList.RemoveAt(index);
      this.setOptionsList(optionsList);
    }

    /// <summary>
    /// Inserts a new DropdownOption into a list at the specified index.
    /// </summary>
    /// <param name="index">The index at which to insert the option.</param>
    /// <param name="value">The <see cref="T:EllieMae.Encompass.Forms.DropdownOption" /> to be inserted.</param>
    public void Insert(int index, DropdownOption value)
    {
      this.ensureNotNull((object) value);
      this.ensureNotEmptyValue(value);
      ArrayList optionsList = this.getOptionsList();
      optionsList.Insert(index, (object) value);
      this.setOptionsList(optionsList);
    }

    /// <summary>Removes a DropdownOption from the collection.</summary>
    /// <param name="value">The option to be removed.</param>
    public void Remove(DropdownOption value)
    {
      this.ensureNotNull((object) value);
      ArrayList optionsList = this.getOptionsList();
      optionsList.Remove((object) value);
      this.setOptionsList(optionsList);
    }

    /// <summary>
    /// Determines if an option with the specified value is contained in the collection.
    /// </summary>
    /// <param name="value">The DropdownOption to search for.</param>
    /// <returns>Returns <c>true</c> if an option exists with the same <see cref="P:EllieMae.Encompass.Forms.DropdownOption.Value" />
    /// as the specified value, <c>false</c> otherwise.</returns>
    public bool Contains(DropdownOption value)
    {
      this.ensureNotNull((object) value);
      return this.getOptionsList().Contains((object) value);
    }

    /// <summary>Clears the list of options.</summary>
    public void Clear() => this.setOptionsList(new ArrayList());

    /// <summary>Returns the index of an option within the list.</summary>
    /// <param name="value">The DropdownOption the index of which is to be returned.</param>
    /// <returns>Returns the index of the option in the list, or -1 if not found.</returns>
    public int IndexOf(DropdownOption value)
    {
      this.ensureNotNull((object) value);
      return this.getOptionsList().IndexOf((object) value);
    }

    /// <summary>Adds a DropdownOption to the list of options</summary>
    /// <param name="value">The option to be added.</param>
    /// <returns>The index of the option within the list.</returns>
    public int Add(DropdownOption value)
    {
      this.ensureNotNull((object) value);
      this.ensureNotEmptyValue(value);
      ArrayList optionsList = this.getOptionsList();
      int num = optionsList.Add((object) value);
      this.setOptionsList(optionsList);
      return num;
    }

    /// <summary>Adds a new option to the dropdown list.</summary>
    /// <param name="text">The text of the option to add.</param>
    /// <param name="value">The underlying value of the option.</param>
    /// <returns>The index of the newly added option within the list.</returns>
    public int Add(string text, string value) => this.Add(new DropdownOption(text, value));

    /// <summary>Adds a new option to the dropdown list.</summary>
    /// <param name="text">The text (and value) of the option to add.</param>
    /// <returns>The index of the newly added option within the list.</returns>
    public int Add(string text) => this.Add(new DropdownOption(text));

    /// <summary>
    /// Adds a collection of DropdownOption objects to the list.
    /// </summary>
    /// <param name="optionList">The collection of DropdownOption objects to be
    /// added.</param>
    public void AddRange(ICollection optionList)
    {
      foreach (DropdownOption option in (IEnumerable) optionList)
        this.ensureNotEmptyValue(option);
      ArrayList optionsList = this.getOptionsList();
      optionsList.AddRange(optionList);
      this.setOptionsList(optionsList);
    }

    /// <summary>Gets the number of options in the collection.</summary>
    public int Count => this.getOptionsList().Count;

    /// <summary>
    /// Returns an array containing the ordered list of <see cref="T:EllieMae.Encompass.Forms.DropdownOption" /> objects.
    /// </summary>
    /// <returns></returns>
    public DropdownOption[] ToArray()
    {
      return (DropdownOption[]) this.getOptionsList().ToArray(typeof (DropdownOption));
    }

    /// <summary>Provides an enumerator for the collection.</summary>
    /// <returns></returns>
    public IEnumerator GetEnumerator() => this.getOptionsList().GetEnumerator();

    internal bool AllowEmptyValues => this.allowEmptyValues;

    internal bool AllowEditValues => this.parentControl.AllowEditValues;

    internal bool AllowRearrangeValues => this.parentControl.AllowRearrangeValues;

    private ArrayList getOptionsList()
    {
      return new ArrayList((ICollection) this.parentControl.ParseOptionsList());
    }

    private void setOptionsList(ArrayList options)
    {
      this.parentControl.RenderOptionsList((DropdownOption[]) options.ToArray(typeof (DropdownOption)));
    }

    private DropdownOption convertToOption(object value)
    {
      try
      {
        return (DropdownOption) value;
      }
      catch
      {
        throw new ArgumentException();
      }
    }

    private void ensureNotNull(object value)
    {
      if (value == null)
        throw new ArgumentNullException();
    }

    private void ensureNotEmptyValue(DropdownOption option)
    {
      if (!this.allowEmptyValues && (option.Value ?? "") == "")
        throw new ArgumentException("Control does not permit options without a Value.");
    }

    /// <summary>Provides a string representation of the collection.</summary>
    /// <returns>A description of the number of items in the collection.</returns>
    public override string ToString()
    {
      ArrayList optionsList = this.getOptionsList();
      int count = optionsList.Count;
      if (count > 0 && ((DropdownOption) optionsList[0]).IsEmpty())
        --count;
      if (count == 0)
        return "(none)";
      return count == 1 ? "(" + (object) count + " item)" : "(" + (object) count + " items)";
    }
  }
}
