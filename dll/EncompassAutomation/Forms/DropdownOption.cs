// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.DropdownOption
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

#nullable disable
namespace EllieMae.Encompass.Forms
{
  public class DropdownOption
  {
    private string text;
    private string value;

    public DropdownOption(string text)
      : this(text, text)
    {
    }

    public DropdownOption(string text, string value)
    {
      this.text = text ?? "";
      this.value = value ?? "";
    }

    public string Text => this.text;

    public string Value => this.value;

    public bool IsEmpty() => this.Value == "" && this.Text == "";

    public override bool Equals(object obj)
    {
      DropdownOption dropdownOption = obj as DropdownOption;
      return !object.Equals(obj, (object) null) && string.Compare(dropdownOption.Value, this.Value, true) == 0;
    }

    public override int GetHashCode() => this.Value.ToUpper().GetHashCode();
  }
}
