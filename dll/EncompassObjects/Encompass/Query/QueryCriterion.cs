// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Query.QueryCriterion
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.Query;

#nullable disable
namespace EllieMae.Encompass.Query
{
  public abstract class QueryCriterion : IQueryCriterion
  {
    internal QueryCriterion()
    {
    }

    public QueryCriterion And(QueryCriterion criterion)
    {
      return (QueryCriterion) new BooleanLogicCriterion((BinaryOperator) 0, this, criterion);
    }

    public QueryCriterion Or(QueryCriterion criterion)
    {
      return (QueryCriterion) new BooleanLogicCriterion((BinaryOperator) 1, this, criterion);
    }

    public abstract QueryCriterion Clone();

    public abstract QueryCriterion Unwrap();
  }
}
