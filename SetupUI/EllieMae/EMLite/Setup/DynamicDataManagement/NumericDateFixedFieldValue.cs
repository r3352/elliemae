// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.NumericDateFixedFieldValue
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class NumericDateFixedFieldValue : FieldValueBase
  {
    private string _value;

    public NumericDateFixedFieldValue(string FieldID, DDMCriteria criteria, string value)
      : base(FieldID, criteria)
    {
      this._value = value;
    }

    public string Values => this._value;

    public override string ToString() => this._value;

    public override string ToDisplayString() => this._value;
  }
}
