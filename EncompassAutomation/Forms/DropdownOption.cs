// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.DropdownOption
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Represents a single option in a DropdownBox or other list-based control.
  /// </summary>
  public class DropdownOption
  {
    private string text;
    private string value;

    /// <summary>Constructor for a new DropdownOption.</summary>
    /// <param name="text">The text of the option.</param>
    /// <remarks>Using this constructor, the <see cref="P:EllieMae.Encompass.Forms.DropdownOption.Value" /> of the option is set to the
    /// same value is the <see cref="P:EllieMae.Encompass.Forms.DropdownOption.Text" />.</remarks>
    public DropdownOption(string text)
      : this(text, text)
    {
    }

    /// <summary>
    /// Constructor for a new DropdownOption specifying both its text and value.
    /// </summary>
    /// <param name="text">The displayed text of the option.</param>
    /// <param name="value">The underlying value of the option.</param>
    public DropdownOption(string text, string value)
    {
      this.text = text ?? "";
      this.value = value ?? "";
    }

    /// <summary>Gets the displayed text of the option in the list.</summary>
    public string Text => this.text;

    /// <summary>
    /// Gets the value of the option when saved into the loan file.
    /// </summary>
    public string Value => this.value;

    /// <summary>
    /// Determines if the option is "empty", i.e. contains no text or value.
    /// </summary>
    /// <returns>Returns <c>true</c> if both the <see cref="P:EllieMae.Encompass.Forms.DropdownOption.Text" /> and <see cref="P:EllieMae.Encompass.Forms.DropdownOption.Value" />
    /// properties are empty strings.</returns>
    public bool IsEmpty() => this.Value == "" && this.Text == "";

    /// <summary>Provides an equality operator for DropdownOptions.</summary>
    /// <param name="obj">The DropdownOption to which to compare the current option.</param>
    /// <returns>Returns <c>true</c> if both options have the same <see cref="P:EllieMae.Encompass.Forms.DropdownOption.Value" />,
    /// <c>false</c> otherwise.</returns>
    public override bool Equals(object obj)
    {
      DropdownOption dropdownOption = obj as DropdownOption;
      return !object.Equals(obj, (object) null) && string.Compare(dropdownOption.Value, this.Value, true) == 0;
    }

    /// <summary>Provides a hash code for the option.</summary>
    /// <returns>Returns an interger has code based on the option's <see cref="P:EllieMae.Encompass.Forms.DropdownOption.Value" />.</returns>
    public override int GetHashCode() => this.Value.ToUpper().GetHashCode();
  }
}
