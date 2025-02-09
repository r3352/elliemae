// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.SearchEngine.DBQueryGenerator.NumberQuery
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Server.ServerObjects.SearchEngine.Defs;
using EllieMae.EMLite.Server.ServerObjects.SearchEngine.Enums;
using EllieMae.EMLite.Server.ServerObjects.SearchEngine.Exceptions;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.SearchEngine.DBQueryGenerator
{
  public class NumberQuery : FilterQuery
  {
    public NumberQuery(SearchFilter filter) => this.Filter = filter;

    public override string GetSql()
    {
      return this.GetColumnWithTableAlias() + " " + this.GetSqlCondition() + this.GetDBValue();
    }

    private string GetSqlCondition()
    {
      if (this.Filter.Operator == FilterOperator.Equals)
        return "=";
      throw new SearchFilterInvalidOperatorException(string.Format("{0} operator not supported for filter '{1}'.", (object) this.Filter.Operator, (object) this.Filter.Name));
    }
  }
}
