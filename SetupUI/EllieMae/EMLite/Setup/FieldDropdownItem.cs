// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FieldDropdownItem
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

#nullable disable
namespace EllieMae.EMLite.Setup
{
  internal class FieldDropdownItem
  {
    private string fieldId;
    private string description;

    public FieldDropdownItem(string fieldId, string description)
    {
      this.fieldId = fieldId;
      this.description = description;
    }

    public string FieldID => this.fieldId;

    public string Description => this.description;

    public override string ToString() => this.fieldId + " - " + this.description;

    public override bool Equals(object obj)
    {
      return obj is FieldDropdownItem fieldDropdownItem && string.Compare(fieldDropdownItem.FieldID, this.FieldID, true) == 0;
    }

    public override int GetHashCode() => this.fieldId.ToLower().GetHashCode();
  }
}
