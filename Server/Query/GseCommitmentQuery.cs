// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Query.GseCommitmentQuery
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils.Query;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server.Query
{
  internal class GseCommitmentQuery : QueryEngine
  {
    private LoanQuery loanQuery;
    private Dictionary<string, FieldFormat> fieldIdMap = new Dictionary<string, FieldFormat>();
    private UserInfo currentUser;

    public GseCommitmentQuery(UserInfo currentUser)
      : base(ClientContext.GetCurrent().Settings.ConnectionString, ClientContext.GetCurrent().Settings.DbServerType, currentUser, (ICriterionTranslator) new TradeFieldTranslator())
    {
      this.loanQuery = new LoanQuery(currentUser);
      this.currentUser = currentUser;
    }

    public override string PrimaryKeyTableIdentifier => "GseCommitmentDetails";

    public override string PrimaryKeyIdentifier => "GseCommitmentDetails.TradeID";

    public override string UserAccessQueryKeyColumnName => "TradeID";

    public override FieldSource GetFieldSource(string name)
    {
      switch (name.ToLower())
      {
        case "gsecommitmentdetails":
          return new FieldSource("GseCommitmentSummary", "left outer join FN_GetGseCommitmentSummaryInline(" + (object) 1 + ") GseCommitmentSummary on GseCommitmentDetails.TradeID = GseCommitmentSummary.TradeID");
        case "loan":
          return new FieldSource("Loan", "left outer join LoanSummary Loan on TradesAssignment.LoanGUID = Loan.GUID", new string[2]
          {
            "MbsPoolDetails",
            "TradesAssignment"
          });
        default:
          return this.loanQuery.GetFieldSource(name);
      }
    }

    public override string GetUserAccessFilterJoinClause(bool isExternalOrganization) => "";

    public override List<string> ParentTables => new List<string>();

    public override void SplitFiltersByReportsFor(FieldFilterList filterList)
    {
    }

    public override FieldFilterList GetParentFilters()
    {
      return this.Filters.Contains((object) ReportsFor.MBSPools) ? this.Filters[(object) ReportsFor.MBSPools] as FieldFilterList : new FieldFilterList();
    }

    public override string GetChildrenFilterSql(
      TradeReportParameters parameters,
      bool isExternalOrganization)
    {
      return string.Empty;
    }

    public override void GetCategories(List<ColumnInfo> fields)
    {
    }

    public override IQueryTerm[] UseNullByReportsFor(ReportsFor reportsFor, List<ColumnInfo> fields)
    {
      return (IQueryTerm[]) new List<DataField>().ToArray();
    }

    public override bool IsParentReportFor(ReportsFor reportsFor)
    {
      return reportsFor == ReportsFor.MBSPools;
    }

    public override string GetChildrenTableJoins(ReportsFor reportsFor, string tableJoins)
    {
      return new StringBuilder().ToString();
    }
  }
}
