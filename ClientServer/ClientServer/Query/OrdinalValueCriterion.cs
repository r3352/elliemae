// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Query.OrdinalValueCriterion
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Newtonsoft.Json.Linq;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Query
{
  [Serializable]
  public class OrdinalValueCriterion : FieldValueCriterion
  {
    private object value;
    private OrdinalMatchType matchType;

    public OrdinalValueCriterion(IQueryTerm field, object value, OrdinalMatchType matchType)
      : this(field, value, matchType, false)
    {
    }

    public OrdinalValueCriterion(
      IQueryTerm field,
      object value,
      OrdinalMatchType matchType,
      bool forceConversion)
      : base(field, forceConversion)
    {
      this.value = value == null || this.isNumericType(value.GetType()) ? value : throw new ArgumentException("Value must be a numeric type", nameof (value));
      this.matchType = matchType;
    }

    public OrdinalValueCriterion(string fieldName, object value, OrdinalMatchType matchType)
      : this(fieldName, value, matchType, false)
    {
    }

    public OrdinalValueCriterion(
      string fieldName,
      object value,
      OrdinalMatchType matchType,
      bool forceConversion)
      : this((IQueryTerm) new DataField(fieldName), value, matchType, forceConversion)
    {
    }

    public OrdinalValueCriterion(string fieldName, object value)
      : this(fieldName, value, OrdinalMatchType.Equals)
    {
    }

    public object Value => this.value;

    public OrdinalMatchType MatchType => this.matchType;

    public override string ToSQLClause(ICriterionTranslator translator)
    {
      string fieldName = this.Term.ToString(translator);
      if (!this.ForceDataTypeConversion || this.value == null)
        return SQL.ToOrdinalMatchClause(fieldName, this.value, this.matchType);
      return "((case when (isnumeric(isnull(" + fieldName + ",0)) <> 0 and " + SQL.ToOrdinalMatchClause("convert(money, isnull(" + fieldName + ",0))", this.value, this.matchType) + ") then 1 else 0 end) = 1)";
    }

    private bool isNumericType(Type t)
    {
      return t == typeof (int) || t == typeof (long) || t == typeof (Decimal) || t == typeof (float) || t == typeof (double);
    }

    public override bool IsExclusive() => this.MatchType == OrdinalMatchType.Equals;

    public override JObject ToJsonClause()
    {
      return new JObject()
      {
        {
          "canonicalName",
          (JToken) this.Term.FieldName
        },
        {
          "value",
          (JToken) this.value.ToString()
        },
        {
          "matchType",
          (JToken) this.matchType.ToString()
        }
      };
    }
  }
}
