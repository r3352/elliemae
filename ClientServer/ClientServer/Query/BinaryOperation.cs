// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Query.BinaryOperation
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Query
{
  [Serializable]
  public class BinaryOperation : QueryCriterion
  {
    private QueryCriterion[] criteria;
    private BinaryOperator op;

    public BinaryOperation(BinaryOperator op, QueryCriterion lhs, QueryCriterion rhs)
    {
      this.op = op;
      this.criteria = new QueryCriterion[2]{ lhs, rhs };
    }

    public BinaryOperation(BinaryOperator op, QueryCriterion[] lhs, QueryCriterion[] rhs)
    {
      this.op = op;
      this.criteria = new QueryCriterion[2]
      {
        (QueryCriterion) new BinaryOperation(BinaryOperator.And, lhs),
        (QueryCriterion) new BinaryOperation(BinaryOperator.And, rhs)
      };
    }

    public BinaryOperation(BinaryOperator op, params QueryCriterion[] rhs)
    {
      this.op = op;
      this.criteria = rhs;
    }

    public BinaryOperator Operator => this.op;

    public QueryCriterion[] Criteria => this.criteria;

    public override string[] GetTables(ICriterionTranslator fieldTranslator)
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      for (int index = 0; index < this.criteria.Length; ++index)
      {
        foreach (object table in this.criteria[index].GetTables(fieldTranslator))
          insensitiveHashtable[table] = (object) true;
      }
      string[] tables = new string[insensitiveHashtable.Count];
      insensitiveHashtable.Keys.CopyTo((Array) tables, 0);
      return tables;
    }

    public override string[] GetFields(ICriterionTranslator fieldTranslator)
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      for (int index = 0; index < this.criteria.Length; ++index)
      {
        foreach (object field in this.criteria[index].GetFields(fieldTranslator))
          insensitiveHashtable[field] = (object) true;
      }
      string[] fields = new string[insensitiveHashtable.Count];
      insensitiveHashtable.Keys.CopyTo((Array) fields, 0);
      return fields;
    }

    public override string ToSQLClause(ICriterionTranslator translator)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < this.criteria.Length; ++index)
      {
        if (index > 0)
          stringBuilder.Append(SQL.BinaryOperatorToString(this.op));
        stringBuilder.Append("(" + this.criteria[index].ToSQLClause(translator) + ")");
      }
      return stringBuilder.ToString();
    }

    public override QueryCriterion Translate(ICriterionTranslator fieldTranslator)
    {
      QueryCriterion[] queryCriterionArray = new QueryCriterion[this.criteria.Length];
      for (int index = 0; index < this.criteria.Length; ++index)
        queryCriterionArray[index] = this.criteria[index].Translate(fieldTranslator);
      return (QueryCriterion) new BinaryOperation(this.op, queryCriterionArray);
    }

    public override bool IsExclusive()
    {
      if (this.op == BinaryOperator.Or)
        return false;
      foreach (QueryCriterion criterion in this.criteria)
      {
        if (criterion.IsExclusive())
          return true;
      }
      return false;
    }

    public override JObject ToJsonClause()
    {
      JArray jarray = new JArray();
      JObject jsonClause = new JObject();
      for (int index = 0; index < this.criteria.Length; ++index)
      {
        if (this.criteria[index] is StringValueCriterion)
        {
          StringValueCriterion criterion = this.criteria[index] as StringValueCriterion;
          if (criterion.FieldName == "Loan.LoanFolder" && criterion.Value == "(Trash)")
            continue;
        }
        jarray.Add((JToken) this.criteria[index].ToJsonClause());
        if (index > 0 && jarray.Count > 0)
        {
          if (jarray.Count > 1)
          {
            jsonClause.Add("operator", (JToken) SQL.BinaryOperatorToString(this.op).ToUpper().TrimStart().TrimEnd());
            jsonClause.Add("terms", (JToken) jarray);
          }
          else
            jsonClause = (JObject) jarray[0];
        }
      }
      return jsonClause;
    }
  }
}
