// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.DDM_RA_NonNumericFieldValues
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class DDM_RA_NonNumericFieldValues : FieldValueBase
  {
    private string[] values;

    public DDM_RA_NonNumericFieldValues(string FieldID, DDMCriteria criteria, string[] vals)
      : base(FieldID, criteria)
    {
      this.values = vals;
    }

    public string[] Values => this.values;

    public override string ToString() => this.GenerateDelimitedValueList(this.Values, "|");

    public override string ToDisplayString() => this.GenerateDelimitedValueList(this.Values, ",");
  }
}
