// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Query.XRefValueCriterion
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Newtonsoft.Json.Linq;
using System;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Query
{
  [Serializable]
  public class XRefValueCriterion : FieldValueCriterion
  {
    private string xrefFieldName;
    private QueryCriterion xrefCriteria;
    private bool include = true;

    public XRefValueCriterion(string fieldName, string xrefFieldName)
      : this(fieldName, xrefFieldName, true)
    {
    }

    public XRefValueCriterion(string fieldName, string xrefFieldName, bool include)
      : this(fieldName, xrefFieldName, (QueryCriterion) null, include)
    {
    }

    public XRefValueCriterion(string fieldName, string xrefFieldName, QueryCriterion xrefCriteria)
      : this(fieldName, xrefFieldName, xrefCriteria, true)
    {
    }

    public XRefValueCriterion(
      string fieldName,
      string xrefFieldName,
      QueryCriterion xrefCriteria,
      bool include)
      : base(fieldName)
    {
      this.xrefFieldName = xrefFieldName;
      this.xrefCriteria = xrefCriteria;
      this.include = include;
    }

    public string XRefFieldName => this.xrefFieldName;

    public QueryCriterion XRefCriteria => this.xrefCriteria;

    public bool Include => this.include;

    public override string ToSQLClause(ICriterionTranslator translator)
    {
      string str1 = this.Term.ToString(translator);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("(" + str1 + " ");
      if (!this.include)
        stringBuilder.Append("not ");
      string sqlColumnName = DataField.TranslateName(this.xrefFieldName, translator).ToSqlColumnName();
      string str2 = "[" + DataField.GetTableName(this.xrefFieldName, translator) + "]";
      stringBuilder.Append("in (select " + sqlColumnName + " from " + str2);
      if (this.xrefCriteria != null)
        stringBuilder.Append(" where " + this.xrefCriteria.ToSQLClause(translator));
      stringBuilder.Append("))");
      return stringBuilder.ToString();
    }

    public override bool IsExclusive()
    {
      return this.Include && this.xrefCriteria != null && this.xrefCriteria.IsExclusive();
    }

    public override JObject ToJsonClause()
    {
      return new JObject()
      {
        {
          "canonicalName",
          (JToken) this.XRefFieldName
        },
        {
          "Include",
          (JToken) this.include
        }
      };
    }
  }
}
