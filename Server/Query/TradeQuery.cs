// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Query.TradeQuery
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils.Query;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server.Query
{
  internal class TradeQuery : QueryEngine
  {
    private LoanQuery loanQuery;
    private Dictionary<string, FieldFormat> fieldIdMap = new Dictionary<string, FieldFormat>();

    public TradeQuery(UserInfo currentUser)
      : base(ClientContext.GetCurrent().Settings.ConnectionString, ClientContext.GetCurrent().Settings.DbServerType, currentUser, (ICriterionTranslator) new TradeFieldTranslator())
    {
      this.loanQuery = new LoanQuery(currentUser);
    }

    public override string PrimaryKeyTableIdentifier => "LoanTradeDetails";

    public override string PrimaryKeyIdentifier => "LoanTradeDetails.TradeID";

    public override string UserAccessQueryKeyColumnName => "TradeID";

    public override FieldSource GetFieldSource(string name)
    {
      switch (name.ToLower())
      {
        case "assignedsecuritytrade":
          return new FieldSource("AssignedSecurityTrade", "left outer join TradeAssignmentByTrade AssignedSecurityTrade on AssignedSecurityTrade.AssigneeTradeID = LoanTradeDetails.TradeID", new string[1]
          {
            "LoanTradeDetails"
          });
        case "assignedsecuritytradedetails":
          return new FieldSource("AssignedSecurityTradeDetails", "left outer join Trades AssignedSecurityTradeDetails on AssignedSecurityTradeDetails.TradeID = AssignedSecurityTrade.TradeID", new string[1]
          {
            "AssignedSecurityTrade"
          });
        case "loan":
          return new FieldSource("Loan", "left outer join LoanSummary Loan on TradesAssignment.LoanGUID = Loan.GUID", new string[2]
          {
            "LoanTradeDetails",
            "TradesAssignment"
          });
        case "securitytradedetails":
          return new FieldSource("SecurityTradeDetails", "left outer join SecurityTradeDetails SecurityTradeDetails on SecurityTradeDetails.TradeID = AssignedSecurityTrade.TradeID", new string[1]
          {
            "AssignedSecurityTrade"
          });
        case "securitytradesummary":
          return new FieldSource("SecurityTradeSummary", "left outer join FN_GetSecurityTradeSummaryInline(" + (object) 1 + ") SecurityTradesummary on SecurityTradeDetails.TradeID = SecurityTradeSummary.TradeID", new string[1]
          {
            "SecurityTradeDetails"
          });
        case "tradeloantradesummary":
          return new FieldSource("TradeLoanTradeSummary", "left outer join FN_GetLoanTradeSummaryInline(" + (object) 1 + ") TradeLoanTradeSummary on LoanTradeDetails.TradeID = TradeLoanTradeSummary.TradeID", new string[1]
          {
            "LoanTradeDetails"
          });
        case "tradesassignment":
          return new FieldSource("TradesAssignment", "left outer join TradeAssignment TradesAssignment on LoanTradeDetails.TradeID = TradesAssignment.TradeID", new string[1]
          {
            "LoanTradeDetails"
          });
        case "tradesmastercontract":
          return new FieldSource("TradesMasterContract", "left outer join MasterContracts TradesMasterContract on LoanTradeDetails.ContractID = TradesMasterContract.ContractID", new string[1]
          {
            "LoanTradeDetails"
          });
        default:
          return this.loanQuery.GetFieldSource(name);
      }
    }

    public override string GetUserAccessFilterJoinClause(bool isExternalOrganization) => "";

    public override List<string> ParentTables
    {
      get
      {
        return new List<string>()
        {
          "LoanTradeDetails",
          "TradeLoanTradeSummary",
          "TradesMasterContract",
          "AssignedSecurityTradeDetails"
        };
      }
    }

    public override void SplitFiltersByReportsFor(FieldFilterList filterList)
    {
      foreach (FieldFilter filter in (List<FieldFilter>) filterList)
      {
        if (this.IsParentFields(filter.CriterionName))
        {
          if (!this.Filters.Contains((object) ReportsFor.LoanTrades))
            this.Filters.Add((object) ReportsFor.LoanTrades, (object) new FieldFilterList());
          (this.Filters[(object) ReportsFor.LoanTrades] as FieldFilterList).Add(filter);
        }
        else
        {
          if (!this.Filters.Contains((object) ReportsFor.Loan))
            this.Filters.Add((object) ReportsFor.Loan, (object) new FieldFilterList());
          (this.Filters[(object) ReportsFor.Loan] as FieldFilterList).Add(filter);
        }
      }
    }

    public override void GetCategories(List<ColumnInfo> fields)
    {
      foreach (ColumnInfo field in fields)
      {
        if (this.IsParentFields(field.CriterionName))
        {
          if (!this.Categories.Contains(ReportsFor.LoanTrades))
            this.Categories.Add(ReportsFor.LoanTrades);
        }
        else if (!this.Categories.Contains(ReportsFor.Loan))
          this.Categories.Add(ReportsFor.Loan);
      }
    }

    public override FieldFilterList GetParentFilters()
    {
      return this.Filters.Contains((object) ReportsFor.LoanTrades) ? this.Filters[(object) ReportsFor.LoanTrades] as FieldFilterList : new FieldFilterList();
    }

    public override string GetChildrenFilterSql(
      TradeReportParameters parameters,
      bool isExternalOrganization)
    {
      string childrenFilterSql = string.Empty;
      if (this.Filters.Contains((object) ReportsFor.Loan) || parameters.CustomFilter != null)
      {
        if (!this.Filters.Contains((object) ReportsFor.Loan))
          this.Filters.Add((object) ReportsFor.Loan, (object) new FieldFilterList());
        string str = this.CreateIdentitySelectionQuery(parameters.CreateCombinedFilter(), isExternalOrganization).Replace(" distinct " + this.PrimaryKeyIdentifier, " distinct ISNULL(Loan.GUID, '') ");
        childrenFilterSql = childrenFilterSql + "create table #loan_guids ( LoanGUID varchar(38) PRIMARY KEY )" + Environment.NewLine;
        if (!string.IsNullOrEmpty(str))
          childrenFilterSql = childrenFilterSql + "insert into #loan_guids " + str + Environment.NewLine;
      }
      return childrenFilterSql;
    }

    public override IQueryTerm[] UseNullByReportsFor(ReportsFor reportsFor, List<ColumnInfo> fields)
    {
      List<DataField> dataFieldList = new List<DataField>();
      foreach (ColumnInfo field1 in fields)
      {
        DataField field2 = DataField.CreateField(field1.CriterionName, !field1.IsParent);
        if (field2 != null)
        {
          if (field2.UseNull && reportsFor == ReportsFor.Loan)
            field2.UseNull = false;
          dataFieldList.Add(field2);
        }
      }
      return (IQueryTerm[]) dataFieldList.ToArray();
    }

    public override bool IsParentReportFor(ReportsFor reportsFor)
    {
      return reportsFor == ReportsFor.LoanTrades;
    }

    public override string GetChildrenTableJoins(ReportsFor reportsFor, string tableJoins)
    {
      string[] strArray = tableJoins.Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.None);
      StringBuilder stringBuilder = new StringBuilder();
      if (reportsFor == ReportsFor.Loan)
      {
        foreach (string str in strArray)
        {
          if (str.Contains("TradeAssignment TradesAssignment") && this.Filters.Contains((object) ReportsFor.Loan))
            stringBuilder.AppendLine(str + " and TradesAssignment.LoanGUID in (select LoanGUID from #loan_guids)");
          else
            stringBuilder.AppendLine(str);
        }
      }
      return stringBuilder.ToString();
    }
  }
}
