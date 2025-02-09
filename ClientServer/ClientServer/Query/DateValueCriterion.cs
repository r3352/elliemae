// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Query.DateValueCriterion
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Newtonsoft.Json.Linq;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Query
{
  [Serializable]
  public class DateValueCriterion : FieldValueCriterion
  {
    public static readonly DateTime NullValue = DateTime.MinValue;
    public static readonly DateTime NonNullValue = DateTime.MaxValue;
    private DateTime value;
    private OrdinalMatchType matchType;
    private DateMatchPrecision precision;

    public DateValueCriterion(
      IQueryTerm field,
      DateTime value,
      OrdinalMatchType matchType,
      DateMatchPrecision precision)
      : base(field)
    {
      this.value = value;
      this.matchType = matchType;
      this.precision = precision;
    }

    public DateValueCriterion(
      IQueryTerm field,
      DateTime value,
      OrdinalMatchType matchType,
      DateMatchPrecision precision,
      bool forceConversion)
      : base(field, forceConversion)
    {
      this.value = value;
      this.matchType = matchType;
      this.precision = precision;
    }

    public DateValueCriterion(
      string fieldName,
      DateTime value,
      OrdinalMatchType matchType,
      DateMatchPrecision precision)
      : base(fieldName)
    {
      this.value = value;
      this.matchType = matchType;
      this.precision = precision;
    }

    public DateValueCriterion(
      string fieldName,
      DateTime value,
      OrdinalMatchType matchType,
      DateMatchPrecision precision,
      bool forceConversion)
      : base(fieldName, forceConversion)
    {
      this.value = value;
      this.matchType = matchType;
      this.precision = precision;
    }

    public DateValueCriterion(string fieldName, DateTime value, OrdinalMatchType matchType)
      : this(fieldName, value, matchType, DateMatchPrecision.Exact)
    {
    }

    public DateValueCriterion(
      string fieldName,
      DateTime value,
      OrdinalMatchType matchType,
      bool forceConversion)
      : this(fieldName, value, matchType, DateMatchPrecision.Exact, forceConversion)
    {
    }

    public DateValueCriterion(string fieldName, DateTime value)
      : this(fieldName, value, OrdinalMatchType.Equals)
    {
    }

    public DateTime Value => this.value;

    public OrdinalMatchType MatchType => this.matchType;

    public DateMatchPrecision Precision => this.precision;

    public override string ToSQLClause(ICriterionTranslator translator)
    {
      string fieldName = this.Term.ToString(translator);
      if (this.ForceDataTypeConversion)
      {
        if (this.value == DateTime.MinValue)
          return "(ISDATE(" + fieldName + ") = 0)";
        if (this.value == DateTime.MaxValue)
          return "(ISDATE(" + fieldName + ") = 1)";
        return "((case when (ISDATE(" + fieldName + ") <> 0) and " + SQL.ToDateMatchClause(fieldName, this.value, this.matchType, this.precision) + " then 1 else 0 end) = 1)";
      }
      if (this.value == DateTime.MinValue)
        return "(" + fieldName + " is NULL)";
      return this.value == DateTime.MaxValue ? "(" + fieldName + " is not NULL)" : SQL.ToDateMatchClause(fieldName, this.value, this.matchType, this.precision);
    }

    public override bool IsExclusive() => this.MatchType == OrdinalMatchType.Equals;

    public override JObject ToJsonClause()
    {
      JObject jsonClause = new JObject();
      jsonClause.Add("canonicalName", (JToken) this.Term.FieldName);
      jsonClause.Add("value", (JToken) this.value.ToString());
      jsonClause.Add("matchType", (JToken) this.matchType.ToString());
      if (this.precision != DateMatchPrecision.Exact)
        jsonClause.Add("precision", (JToken) this.precision.ToString());
      return jsonClause;
    }
  }
}
