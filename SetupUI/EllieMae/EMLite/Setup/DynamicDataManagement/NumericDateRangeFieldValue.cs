// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.NumericDateRangeFieldValue
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class NumericDateRangeFieldValue : FieldValueBase
  {
    private string _minvalue;
    private string _maxvalue;

    public NumericDateRangeFieldValue(string FieldID, string minvalue, string maxvalue)
      : base(FieldID, DDMCriteria.Range)
    {
      this._minvalue = minvalue;
      this._maxvalue = maxvalue;
    }

    public string Minimum => this._minvalue;

    public string Maximum => this._maxvalue;

    public override string ToString() => this._minvalue + "|" + this._maxvalue;

    public override string ToDisplayString() => this._minvalue + "," + this._maxvalue;
  }
}
