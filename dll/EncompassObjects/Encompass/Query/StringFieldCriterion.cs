// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Query.StringFieldCriterion
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.Query;

#nullable disable
namespace EllieMae.Encompass.Query
{
  public class StringFieldCriterion : QueryCriterion, IStringFieldCriterion
  {
    private string fieldName = "";
    private string fieldValue = "";
    private StringFieldMatchType matchType;
    private bool include = true;

    public StringFieldCriterion()
    {
    }

    public StringFieldCriterion(
      string fieldName,
      string value,
      StringFieldMatchType matchType,
      bool include)
    {
      this.fieldName = fieldName;
      this.fieldValue = value;
      this.matchType = matchType;
      this.include = include;
    }

    public string FieldName
    {
      get => this.fieldName;
      set => this.fieldName = value;
    }

    public string Value
    {
      get => this.fieldValue;
      set => this.fieldValue = value;
    }

    public StringFieldMatchType MatchType
    {
      get => this.matchType;
      set => this.matchType = value;
    }

    public bool Include
    {
      get => this.include;
      set => this.include = value;
    }

    public override QueryCriterion Clone()
    {
      return (QueryCriterion) new StringFieldCriterion(this.fieldName, this.fieldValue, this.matchType, this.include);
    }

    public override QueryCriterion Unwrap()
    {
      return (QueryCriterion) new StringValueCriterion(this.fieldName, this.fieldValue, (StringMatchType) this.matchType, this.include);
    }
  }
}
