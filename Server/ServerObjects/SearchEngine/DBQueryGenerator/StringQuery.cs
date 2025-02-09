// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.SearchEngine.DBQueryGenerator.StringQuery
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.Server.ServerObjects.SearchEngine.Defs;
using EllieMae.EMLite.Server.ServerObjects.SearchEngine.Enums;
using EllieMae.EMLite.Server.ServerObjects.SearchEngine.Exceptions;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.SearchEngine.DBQueryGenerator
{
  public class StringQuery : FilterQuery
  {
    public StringQuery(SearchFilter filter) => this.Filter = filter;

    public override string GetSql()
    {
      return this.GetColumnWithTableAlias() + " " + this.GetSqlConditionPrefix() + SQL.EncodeString(this.Filter.Value, false) + this.GetSqlConditionSuffix();
    }

    private string GetSqlConditionPrefix()
    {
      switch (this.Filter.Operator)
      {
        case FilterOperator.Equals:
        case FilterOperator.StartsWith:
          return "='";
        case FilterOperator.Like:
          return "like '%";
        default:
          throw new SearchFilterInvalidOperatorException(string.Format("{0} operator not supported for filter '{1}'.", (object) this.Filter.Operator, (object) this.Filter.Name));
      }
    }

    public string GetSqlOperator()
    {
      switch (this.Filter.Operator)
      {
        case FilterOperator.Equals:
          return "=";
        case FilterOperator.Like:
          return "like";
        default:
          throw new SearchFilterInvalidOperatorException(string.Format("{0} operator not supported for filter '{1}'.", (object) this.Filter.Operator, (object) this.Filter.Name));
      }
    }

    public string GetEncodedValue(bool encloseQuotes)
    {
      return SQL.EncodeString(this.Filter.Value, encloseQuotes);
    }

    public string GetValue(bool encloseQuotes)
    {
      return encloseQuotes ? "'" + this.Filter.Value + "'" : this.Filter.Value;
    }

    private string GetSqlConditionSuffix()
    {
      switch (this.Filter.Operator)
      {
        case FilterOperator.Equals:
          return "'";
        case FilterOperator.Like:
        case FilterOperator.StartsWith:
          return "%'";
        default:
          throw new SearchFilterInvalidOperatorException(string.Format("{0} operator not supported for filter '{1}'.", (object) this.Filter.Operator, (object) this.Filter.Name));
      }
    }
  }
}
