// Decompiled with JetBrains decompiler
// Type: Elli.SQE.QueryDsl.QueryCriterionExtensions
// Assembly: Elli.SQE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 49809150-F4C8-4F09-B50E-E491713B0B30
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.dll

using EllieMae.EMLite.ClientServer.Query;

#nullable disable
namespace Elli.SQE.QueryDsl
{
  public static class QueryCriterionExtensions
  {
    public static IVisitable<IQueryCriterionVisitor> AsVisitable(this QueryCriterion obj)
    {
      return (IVisitable<IQueryCriterionVisitor>) new QueryCriterionVisitable(obj);
    }
  }
}
