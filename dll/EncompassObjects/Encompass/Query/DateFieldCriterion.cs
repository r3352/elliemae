// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Query.DateFieldCriterion
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.Query;
using System;

#nullable disable
namespace EllieMae.Encompass.Query
{
  public class DateFieldCriterion : QueryCriterion, IDateFieldCriterion
  {
    private string fieldName = "";
    private DateTime fieldValue = DateTime.Now;
    private OrdinalFieldMatchType matchType;
    private DateFieldMatchPrecision precision;
    public static readonly DateTime EmptyDate = DateTime.MinValue;
    public static readonly DateTime NonEmptyDate = DateTime.MaxValue;

    public DateFieldCriterion()
    {
    }

    public DateFieldCriterion(
      string fieldName,
      DateTime value,
      OrdinalFieldMatchType matchType,
      DateFieldMatchPrecision precision)
    {
      this.fieldName = fieldName;
      this.fieldValue = value;
      this.matchType = matchType;
      this.precision = precision;
    }

    public string FieldName
    {
      get => this.fieldName;
      set => this.fieldName = value;
    }

    public DateTime Value
    {
      get => this.fieldValue;
      set => this.fieldValue = value;
    }

    public OrdinalFieldMatchType MatchType
    {
      get
      {
        return this.fieldValue == DateFieldCriterion.EmptyDate || this.fieldValue == DateFieldCriterion.NonEmptyDate ? OrdinalFieldMatchType.Equals : this.matchType;
      }
      set => this.matchType = value;
    }

    public DateFieldMatchPrecision Precision
    {
      get
      {
        return this.fieldValue == DateFieldCriterion.EmptyDate || this.fieldValue == DateFieldCriterion.NonEmptyDate ? DateFieldMatchPrecision.Exact : this.precision;
      }
      set => this.precision = value;
    }

    void IDateFieldCriterion.SetEmptyDateValue()
    {
      this.fieldValue = DateFieldCriterion.EmptyDate;
      this.matchType = OrdinalFieldMatchType.Equals;
    }

    void IDateFieldCriterion.SetNonEmptyDateValue()
    {
      this.fieldValue = DateFieldCriterion.NonEmptyDate;
      this.matchType = OrdinalFieldMatchType.Equals;
    }

    public override QueryCriterion Clone()
    {
      return (QueryCriterion) new DateFieldCriterion(this.FieldName, this.Value, this.MatchType, this.Precision);
    }

    public override QueryCriterion Unwrap()
    {
      return (QueryCriterion) new DateValueCriterion(this.FieldName, this.Value, (OrdinalMatchType) this.MatchType, (DateMatchPrecision) this.Precision);
    }
  }
}
