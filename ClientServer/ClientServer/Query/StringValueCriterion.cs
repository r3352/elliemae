// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Query.StringValueCriterion
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Query
{
  [Serializable]
  public class StringValueCriterion : FieldValueCriterion
  {
    private static Dictionary<string, string> quickBaseFieldValueLookupTable = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private string value;
    private StringMatchType matchType;
    private bool include = true;

    static StringValueCriterion()
    {
      string appSetting = ConfigurationManager.AppSettings["FilterByBaseFieldForContains"];
      if (string.IsNullOrWhiteSpace(appSetting) || !("true" == appSetting.ToLower()) && !("1" == appSetting))
        return;
      StringValueCriterion.quickBaseFieldValueLookupTable["CurrentLoanAssociate.FullName"] = string.Format("select [{0}] from [users] where [{0}]", (object) "FirstLastName");
      StringValueCriterion.quickBaseFieldValueLookupTable["CurrentLoanAssociate.first_name"] = string.Format("select [{0}] from [users] where [{0}]", (object) "first_name");
      StringValueCriterion.quickBaseFieldValueLookupTable["CurrentLoanAssociate.last_name"] = string.Format("select [{0}] from [users] where [{0}]", (object) "last_name");
    }

    public StringValueCriterion(
      IQueryTerm field,
      string value,
      StringMatchType matchType,
      bool include)
      : base(field)
    {
      this.value = value;
      this.matchType = matchType;
      this.include = include;
    }

    public StringValueCriterion(
      string fieldName,
      string value,
      StringMatchType matchType,
      bool include)
      : base(fieldName)
    {
      this.value = value;
      this.matchType = matchType;
      this.include = include;
    }

    public StringValueCriterion(string fieldName, string value, StringMatchType matchType)
      : this(fieldName, value, matchType, true)
    {
    }

    public StringValueCriterion(string fieldName, string value)
      : this(fieldName, value, StringMatchType.Exact)
    {
    }

    public string Value => this.value;

    public StringMatchType MatchType => this.matchType;

    public bool IncludeMatches => this.include;

    public static Dictionary<string, string> QuickBaseFieldValueLookupTable
    {
      get => StringValueCriterion.quickBaseFieldValueLookupTable;
    }

    public override string ToSQLClause(ICriterionTranslator translator)
    {
      string fieldName = this.Term.ToString(translator);
      string str = this.matchType != StringMatchType.Contains || !StringValueCriterion.quickBaseFieldValueLookupTable.ContainsKey(this.Term.FieldName) ? SQL.ToStringMatchClause(fieldName, this.value, this.matchType) : SQL.ToFastStringMatchClauseForContains(fieldName, StringValueCriterion.quickBaseFieldValueLookupTable[this.Term.FieldName], this.value);
      return this.include ? str : "NOT (" + str + ")";
    }

    public override bool IsExclusive() => this.IncludeMatches;

    public override JObject ToJsonClause()
    {
      JObject jsonClause = new JObject();
      jsonClause.Add("canonicalName", (JToken) this.Term.FieldName);
      if (!this.include && this.matchType == StringMatchType.Exact && this.value == "Y")
        jsonClause.Add("value", (JToken) "N");
      else
        jsonClause.Add("value", (JToken) (this.value != null ? this.value.ToString() : (string) null));
      jsonClause.Add("matchType", (JToken) (this.matchType == StringMatchType.CaseInsensitive ? StringMatchType.Exact.ToString() : this.matchType.ToString()));
      jsonClause.Add("include", (JToken) this.include);
      return jsonClause;
    }
  }
}
