// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Query.SortField
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
  public class SortField : IWhiteListRemoteMethodParam
  {
    private IQueryTerm term;
    private FieldSortOrder sortOrder;

    public SortField(string fieldName, FieldSortOrder sortOrder)
      : this(fieldName, sortOrder, DataConversion.None)
    {
    }

    public SortField(string fieldName, FieldSortOrder sortOrder, DataConversion conversion)
      : this((IQueryTerm) new DataField(fieldName), sortOrder, conversion)
    {
    }

    public SortField(IQueryTerm term, FieldSortOrder sortOrder)
      : this(term, sortOrder, DataConversion.None)
    {
    }

    public SortField(IQueryTerm term, FieldSortOrder sortOrder, DataConversion conversion)
    {
      this.term = term != null ? QueryTerm.GetTermForFieldConversion("", term, conversion) : throw new ArgumentNullException(nameof (term));
      this.sortOrder = sortOrder;
    }

    public IQueryTerm Term => this.term;

    public FieldSortOrder SortOrder => this.sortOrder;

    public string ToSQLClause() => this.ToSQLClause((ICriterionTranslator) null);

    public string ToSQLClause(ICriterionTranslator fieldTranslator)
    {
      string str = this.term.ToString(fieldTranslator);
      if (str == "")
        return "";
      if (str.Equals("[Loan].[IsArchived]", StringComparison.OrdinalIgnoreCase))
        str = "Loan__IsArchived";
      return this.sortOrder == FieldSortOrder.Descending ? str + " desc" : str + " asc";
    }

    public string ToSQLClause(ICriterionTranslator fieldTranslator, bool withTableName)
    {
      string str = this.term.ToString(fieldTranslator, withTableName);
      if (str == "")
        return "";
      if (str.Equals("[Loan].[IsArchived]", StringComparison.OrdinalIgnoreCase))
        str = "Loan__IsArchived";
      return this.sortOrder == FieldSortOrder.Descending ? str + " desc" : str + " asc";
    }

    public string ToSQLClauseWithoutOrder(ICriterionTranslator fieldTranslator)
    {
      string str = this.term.ToString(fieldTranslator);
      return str == "" ? "" : str;
    }

    public void SerializeForLog(JsonWriter writer, JsonSerializer serializer)
    {
      serializer.Serialize(writer, (object) new JObject()
      {
        {
          "fieldName",
          (JToken) this.term.FieldName
        },
        {
          "sortOrder",
          (JToken) this.SortOrder.ToString()
        }
      });
    }
  }
}
