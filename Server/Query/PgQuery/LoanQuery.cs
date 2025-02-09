// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Query.PgQuery.LoanQuery
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.SQE;
using Elli.SQE.IO.PGSQL.QueryDsl;
using Elli.SQE.QueryDsl;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils.Query;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.Query.PgQuery
{
  public class LoanQuery : EllieMae.EMLite.Server.Query.LoanQuery
  {
    public const string PrimaryKeyTableAlias = "ElliLoanEntity�";
    public const string PrimaryKeyTableJsonFieldName = "EntityData�";
    public const string PrimaryKeyTableClause = " (select EntityId, EntityData from ElliLoan where EntityType = 1) �";
    private string _primaryKeyTableIdentifier;

    public LoanQuery(UserInfo currentUser)
      : base(currentUser)
    {
      this.PreserveColumnNameCase = true;
    }

    public LoanQuery(bool useERDB, UserInfo currentUser)
      : base(useERDB, currentUser)
    {
      this.PreserveColumnNameCase = true;
    }

    public LoanQuery(UserInfo user, LoanInfo.Right accessRight)
      : base(user, false, accessRight)
    {
      this.PreserveColumnNameCase = true;
    }

    public LoanQuery(bool useERDB, UserInfo user, LoanInfo.Right accessRight)
      : base(useERDB, user, accessRight)
    {
      this.PreserveColumnNameCase = true;
    }

    public LoanQuery(
      UserInfo user,
      LoanInfo.Right accessRight,
      ICriterionTranslator fieldTranslator)
      : base(user, false, accessRight, fieldTranslator)
    {
      this.PreserveColumnNameCase = true;
    }

    public LoanQuery(
      bool useERDB,
      UserInfo user,
      LoanInfo.Right accessRight,
      ICriterionTranslator fieldTranslator)
      : base(useERDB, false, user, accessRight, fieldTranslator)
    {
      this.PreserveColumnNameCase = true;
    }

    public LoanQuery(
      UserInfo user,
      LoanInfo.Right accessRight,
      ICriterionTranslator fieldTranslator,
      string loanFolder,
      bool optimizeForSmallResultSet)
      : base(user, accessRight, fieldTranslator, loanFolder, optimizeForSmallResultSet)
    {
      this.PreserveColumnNameCase = true;
    }

    public override string PrimaryKeyTableIdentifier
    {
      get
      {
        if (string.IsNullOrWhiteSpace(this._primaryKeyTableIdentifier))
          this._primaryKeyTableIdentifier = " (select EntityId, EntityData from ElliLoan where EntityType = 1) ElliLoanEntity";
        return this._primaryKeyTableIdentifier;
      }
    }

    public override string PrimaryKeyIdentifier => "ElliLoanEntity.EntityId";

    public override string UserAccessQueryKeyColumnName => "Guid";

    public override FieldSource GetFieldSource(string name)
    {
      switch (name.ToLower())
      {
        case "loan":
          return new FieldSource("loan", string.Format("inner join LoanSummary Loan on (Loan.Guid = {0})", (object) this.PrimaryKeyIdentifier), new string[1]
          {
            "ElliLoanEntity"
          });
        case "elliloanentity":
          return (FieldSource) null;
        default:
          QueryableFieldSource queryableFieldSource = LoanFieldSources.Instance[name];
          return queryableFieldSource == null ? base.GetFieldSource(name) : new FieldSource(queryableFieldSource.Name, queryableFieldSource.JoinClause, queryableFieldSource.Dependencies);
      }
    }

    public override string GetFilterClause(QueryCriterion criteria, bool excludeArchivedLoans = false)
    {
      string str1 = "(IsArchived = 0)";
      string filterClause = excludeArchivedLoans ? "where " + str1 + Environment.NewLine : "";
      if (criteria == null)
        return filterClause;
      QueryCriterionToSQLClauseVisitor<Elli.Domain.Mortgage.Loan> visitor = new QueryCriterionToSQLClauseVisitor<Elli.Domain.Mortgage.Loan>(this.FieldTranslator);
      string str2 = criteria.Translate(this.FieldTranslator).AsVisitable().Accept<string>((IQueryCriterionVisitor) visitor);
      if (string.IsNullOrEmpty(str2))
        return filterClause;
      if (excludeArchivedLoans)
        str2 = str2 + " and " + str1;
      return "where " + str2 + Environment.NewLine;
    }

    public void SetPrimaryKeyTableIdentifier(string primaryKeyTableIdentifier)
    {
      this._primaryKeyTableIdentifier = primaryKeyTableIdentifier;
    }
  }
}
