// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Query.QueryCriterion
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Query
{
  [Serializable]
  public abstract class QueryCriterion : IWhiteListRemoteMethodParam
  {
    public string ToSQLClause() => this.ToSQLClause((ICriterionTranslator) null);

    public JObject ToJson() => this.ToJsonClause();

    public QueryCriterion And(QueryCriterion criterion)
    {
      return criterion == null ? this : (QueryCriterion) new BinaryOperation(BinaryOperator.And, this, criterion);
    }

    public QueryCriterion Or(QueryCriterion criterion)
    {
      return criterion == null ? this : (QueryCriterion) new BinaryOperation(BinaryOperator.Or, this, criterion);
    }

    public abstract string[] GetTables(ICriterionTranslator fieldTranslator);

    public abstract string[] GetFields(ICriterionTranslator fieldTranslator);

    public abstract bool IsExclusive();

    public bool UsesTable(string tableName)
    {
      return this.UsesTable(tableName, (ICriterionTranslator) null);
    }

    public bool UsesTable(string tableName, ICriterionTranslator fieldTranslator)
    {
      foreach (string table in this.GetTables(fieldTranslator))
      {
        if (string.Compare(table, tableName, true) == 0)
          return true;
      }
      return false;
    }

    public bool UsesField(string fieldName, ICriterionTranslator fieldTranslator)
    {
      foreach (string field in this.GetFields(fieldTranslator))
      {
        if (string.Compare(field, fieldName, true) == 0)
          return true;
      }
      return false;
    }

    public bool UsesField(string fieldName)
    {
      return this.UsesField(fieldName, (ICriterionTranslator) null);
    }

    public abstract string ToSQLClause(ICriterionTranslator fieldTranslator);

    public abstract JObject ToJsonClause();

    public virtual QueryCriterion Translate(ICriterionTranslator fieldTranslator)
    {
      return fieldTranslator.TranslateCriterion(this);
    }

    public static QueryCriterion Join(QueryCriterion[] criteria, BinaryOperator op)
    {
      if (criteria == null || criteria.Length == 0)
        return (QueryCriterion) null;
      return criteria.Length == 1 ? criteria[0] : (QueryCriterion) new BinaryOperation(op, criteria);
    }

    public void SerializeForLog(JsonWriter writer, JsonSerializer serializer)
    {
      serializer.Serialize(writer, (object) this.ToJsonClause());
    }
  }
}
