// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Query.NumericFieldCriterion
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.Query;

#nullable disable
namespace EllieMae.Encompass.Query
{
  public class NumericFieldCriterion : QueryCriterion, INumericFieldCriterion
  {
    private string fieldName = "";
    private double fieldValue;
    private OrdinalFieldMatchType matchType;

    public NumericFieldCriterion()
    {
    }

    public NumericFieldCriterion(string fieldName, double value, OrdinalFieldMatchType matchType)
    {
      this.fieldName = fieldName;
      this.fieldValue = value;
      this.matchType = matchType;
    }

    public string FieldName
    {
      get => this.fieldName;
      set => this.fieldName = value;
    }

    public double Value
    {
      get => this.fieldValue;
      set => this.fieldValue = value;
    }

    public OrdinalFieldMatchType MatchType
    {
      get => this.matchType;
      set => this.matchType = value;
    }

    public override QueryCriterion Clone()
    {
      return (QueryCriterion) new NumericFieldCriterion(this.fieldName, this.fieldValue, this.matchType);
    }

    public override QueryCriterion Unwrap()
    {
      return (QueryCriterion) new OrdinalValueCriterion(this.fieldName, (object) this.fieldValue, (OrdinalMatchType) this.matchType);
    }
  }
}
