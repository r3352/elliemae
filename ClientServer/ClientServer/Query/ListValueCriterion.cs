// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Query.ListValueCriterion
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Newtonsoft.Json.Linq;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Query
{
  [Serializable]
  public class ListValueCriterion : FieldValueCriterion
  {
    private Array valueList;
    private bool includeMatches;

    public ListValueCriterion(IQueryTerm field, Array valueList)
      : this(field, valueList, true)
    {
      this.valueList = valueList;
    }

    public ListValueCriterion(IQueryTerm field, Array valueList, bool includeMatches)
      : base(field)
    {
      this.valueList = valueList;
      this.includeMatches = includeMatches;
    }

    public ListValueCriterion(string fieldName, Array valueList)
      : this(fieldName, valueList, true)
    {
      this.valueList = valueList;
    }

    public ListValueCriterion(string fieldName, Array valueList, bool includeMatches)
      : base(fieldName)
    {
      this.valueList = valueList;
      this.includeMatches = includeMatches;
    }

    public Array ValueList => this.valueList;

    public bool IncludeMatches => this.includeMatches;

    public override string ToSQLClause(ICriterionTranslator fieldTranslator)
    {
      string str = this.Term.ToString(fieldTranslator);
      if (str.ToLower() == "[loan].[isarchived]" && this.valueList != null && this.valueList.Length >= 1)
        return str + " = " + (object) SQL.EncodeYNToBit(this.valueList.GetValue(0).ToString());
      if (!this.includeMatches)
        str += " not";
      return str + " in (" + SQL.EncodeArray(this.valueList) + ")";
    }

    public override bool IsExclusive() => this.includeMatches && this.valueList.Length <= 2;

    public override JObject ToJsonClause()
    {
      JObject jsonClause = new JObject();
      jsonClause.Add("canonicalName", (JToken) this.Term.FieldName);
      if (this.valueList != null && this.valueList.Length > 0)
      {
        JArray jarray = new JArray();
        for (int index = 0; index < this.valueList.Length; ++index)
          jarray.Add((JToken) Convert.ToString(this.valueList.GetValue(index)));
        jsonClause.Add("value", (JToken) jarray);
      }
      jsonClause.Add("matchType", (JToken) "Multivalue");
      jsonClause.Add("include", (JToken) this.includeMatches);
      return jsonClause;
    }
  }
}
