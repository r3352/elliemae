// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.DropdownOptionCollection
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Forms
{
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

    public DropdownOption this[int index] => (DropdownOption) this.getOptionsList()[index];

    public void RemoveAt(int index)
    {
      ArrayList optionsList = this.getOptionsList();
      optionsList.RemoveAt(index);
      this.setOptionsList(optionsList);
    }

    public void Insert(int index, DropdownOption value)
    {
      this.ensureNotNull((object) value);
      this.ensureNotEmptyValue(value);
      ArrayList optionsList = this.getOptionsList();
      optionsList.Insert(index, (object) value);
      this.setOptionsList(optionsList);
    }

    public void Remove(DropdownOption value)
    {
      this.ensureNotNull((object) value);
      ArrayList optionsList = this.getOptionsList();
      optionsList.Remove((object) value);
      this.setOptionsList(optionsList);
    }

    public bool Contains(DropdownOption value)
    {
      this.ensureNotNull((object) value);
      return this.getOptionsList().Contains((object) value);
    }

    public void Clear() => this.setOptionsList(new ArrayList());

    public int IndexOf(DropdownOption value)
    {
      this.ensureNotNull((object) value);
      return this.getOptionsList().IndexOf((object) value);
    }

    public int Add(DropdownOption value)
    {
      this.ensureNotNull((object) value);
      this.ensureNotEmptyValue(value);
      ArrayList optionsList = this.getOptionsList();
      int num = optionsList.Add((object) value);
      this.setOptionsList(optionsList);
      return num;
    }

    public int Add(string text, string value) => this.Add(new DropdownOption(text, value));

    public int Add(string text) => this.Add(new DropdownOption(text));

    public void AddRange(ICollection optionList)
    {
      foreach (DropdownOption option in (IEnumerable) optionList)
        this.ensureNotEmptyValue(option);
      ArrayList optionsList = this.getOptionsList();
      optionsList.AddRange(optionList);
      this.setOptionsList(optionsList);
    }

    public int Count => this.getOptionsList().Count;

    public DropdownOption[] ToArray()
    {
      return (DropdownOption[]) this.getOptionsList().ToArray(typeof (DropdownOption));
    }

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
