// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Query.NullValueCriterion
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Newtonsoft.Json.Linq;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Query
{
  [Serializable]
  public class NullValueCriterion : FieldValueCriterion
  {
    private bool includeNulls;

    public NullValueCriterion(IQueryTerm field, bool includeNulls)
      : base(field)
    {
      this.includeNulls = includeNulls;
    }

    public NullValueCriterion(string fieldName, bool includeNulls)
      : base(fieldName)
    {
      this.includeNulls = includeNulls;
    }

    public bool IncludeNulls
    {
      get => this.includeNulls;
      set => this.includeNulls = value;
    }

    public override string ToSQLClause(ICriterionTranslator translator)
    {
      return SQL.ToNullMatchClause(this.Term.ToString(translator), this.includeNulls);
    }

    public override bool IsExclusive() => false;

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
          (JToken) null
        },
        {
          "matchType",
          (JToken) (this.includeNulls ? OrdinalMatchType.Equals.ToString() : OrdinalMatchType.NotEquals.ToString())
        }
      };
    }
  }
}
