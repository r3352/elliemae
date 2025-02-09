// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Query.PgQuery.LoanFieldSources
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.SQE;
using Elli.SQE.DD;
using Elli.SQE.IO.PGSQL;
using Elli.SQE.IO.PGSQL.QueryDsl;
using EllieMae.EMLite.ClientServer.Query;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.Query.PgQuery
{
  internal class LoanFieldSources
  {
    private static LoanQueryableFieldSources _loanJQueryableFieldSources;
    private static LoanFieldSources _instance;

    public static LoanFieldSources Instance
    {
      get => LoanFieldSources._instance ?? (LoanFieldSources._instance = new LoanFieldSources());
    }

    private LoanFieldSources()
    {
      LoanFieldSources._loanJQueryableFieldSources = new LoanQueryableFieldSources((IQueryableFieldSourceBuilder) new JQueryableFieldSourceBuilder(new QueryableFieldSourceConvention()
      {
        GetPrimaryKeyTableAlias = (Func<string>) (() => "ElliLoanEntity"),
        GetPrimaryKeyTableJsonFieldName = (Func<string>) (() => "EntityData")
      }, (QueryableFieldCriterionDefConverter) new LoanQueryableFieldCriterionDefConverter((ICanonicalFragmentFormatter) new JCanonicalFragmentFormatter())));
    }

    public QueryableFieldSource this[string fieldId]
    {
      get => LoanFieldSources._loanJQueryableFieldSources.Get(fieldId);
    }

    public QueryableFieldSource GetByFieldName(string fieldName)
    {
      return LoanFieldSources._loanJQueryableFieldSources.GetByFieldName(fieldName);
    }

    public QueryableFieldSource Generate(IQueryTerm field)
    {
      return LoanFieldSources._loanJQueryableFieldSources.Generate(field);
    }
  }
}
