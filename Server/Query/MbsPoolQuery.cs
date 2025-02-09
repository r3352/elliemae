// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Query.MbsPoolQuery
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
  internal class MbsPoolQuery : QueryEngine
  {
    private LoanQuery loanQuery;
    private SecurityTradeQuery securityTradeQuery;
    private Dictionary<string, FieldFormat> fieldIdMap = new Dictionary<string, FieldFormat>();
    private UserInfo currentUser;

    public MbsPoolQuery(UserInfo currentUser)
      : base(ClientContext.GetCurrent().Settings.ConnectionString, ClientContext.GetCurrent().Settings.DbServerType, currentUser, (ICriterionTranslator) new TradeFieldTranslator())
    {
      this.loanQuery = new LoanQuery(currentUser);
      this.currentUser = currentUser;
    }

    public SecurityTradeQuery SecurityTradeQuery
    {
      get
      {
        if (this.securityTradeQuery == null)
          this.securityTradeQuery = new SecurityTradeQuery(this.currentUser);
        return this.securityTradeQuery;
      }
    }

    public override string PrimaryKeyTableIdentifier => "MbsPoolDetails";

    public override string PrimaryKeyIdentifier => "MbsPoolDetails.TradeID";

    public override string UserAccessQueryKeyColumnName => "TradeID";

    public override FieldSource GetFieldSource(string name)
    {
      switch (name.ToLower())
      {
        case "assignedsecuritytrade1":
          return new FieldSource("AssignedSecurityTrade1", "left outer join TradeAssignmentByTrade AssignedSecurityTrade1 on AssignedSecurityTrade1.TradeID = MbsPoolDetails.TradeID", new string[1]
          {
            "MbsPoolDetails"
          });
        case "assignedsecuritytradedetails1":
          return new FieldSource("AssignedSecurityTradeDetails1", "left outer join Trades AssignedSecurityTradeDetails1 on AssignedSecurityTradeDetails1.TradeID = AssignedSecurityTrade1.AssigneeTradeID", new string[1]
          {
            "AssignedSecurityTrade1"
          });
        case "loan":
          return new FieldSource("Loan", "left outer join LoanSummary Loan on TradesAssignment.LoanGUID = Loan.GUID", new string[2]
          {
            "MbsPoolDetails",
            "TradesAssignment"
          });
        case "securitytradedetails":
          return new FieldSource("SecurityTradeDetails", "left outer join SecurityTradeDetails SecurityTradeDetails on SecurityTradeDetails.TradeID = AssignedSecurityTrade1.AssigneeTradeID", new string[1]
          {
            "AssignedSecurityTrade1"
          });
        case "securitytradesummary":
          return new FieldSource("SecurityTradeSummary", "left outer join FN_GetSecurityTradeSummaryInline(" + (object) 1 + ") SecurityTradesummary on SecurityTradeDetails.TradeID = SecurityTradeSummary.TradeID", new string[1]
          {
            "SecurityTradeDetails"
          });
        case "tradembspoolsummary":
          return new FieldSource("TradeMbsPoolSummary", "left outer join FN_GetMbsPoolSummaryInline(" + (object) 1 + ") TradeMbsPoolSummary on MbsPoolDetails.TradeID = TradeMbsPoolSummary.TradeID", new string[1]
          {
            "MbsPoolDetails"
          });
        case "tradesassignment":
          return new FieldSource("TradesAssignment", "left outer join TradeAssignment TradesAssignment on MbsPoolDetails.TradeID = TradesAssignment.TradeID", new string[1]
          {
            "MbsPoolDetails"
          });
        case "tradesmastercontract1":
          return new FieldSource("TradesMasterContract1", "left outer join MasterContracts TradesMasterContract1 on MbsPoolDetails.ContractID = TradesMasterContract1.ContractID", new string[1]
          {
            "MbsPoolDetails"
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
          this.PrimaryKeyTableIdentifier,
          "TradeMbsPoolSummary",
          "TradesMasterContract1",
          "AssignedSecurityTradeDetails1"
        };
      }
    }

    public override void SplitFiltersByReportsFor(FieldFilterList filterList)
    {
      foreach (FieldFilter filter in (List<FieldFilter>) filterList)
      {
        if (this.IsParentFields(filter.CriterionName))
        {
          if (!this.Filters.Contains((object) ReportsFor.MBSPools))
            this.Filters.Add((object) ReportsFor.MBSPools, (object) new FieldFilterList());
          (this.Filters[(object) ReportsFor.MBSPools] as FieldFilterList).Add(filter);
        }
        else if (this.SecurityTradeQuery.IsParentFields(filter.CriterionName))
        {
          if (!this.Filters.Contains((object) ReportsFor.SecurityTrades))
            this.Filters.Add((object) ReportsFor.SecurityTrades, (object) new FieldFilterList());
          (this.Filters[(object) ReportsFor.SecurityTrades] as FieldFilterList).Add(filter);
        }
        else
        {
          if (!this.Filters.Contains((object) ReportsFor.Loan))
            this.Filters.Add((object) ReportsFor.Loan, (object) new FieldFilterList());
          (this.Filters[(object) ReportsFor.Loan] as FieldFilterList).Add(filter);
        }
      }
    }

    public override FieldFilterList GetParentFilters()
    {
      return this.Filters.Contains((object) ReportsFor.MBSPools) ? this.Filters[(object) ReportsFor.MBSPools] as FieldFilterList : new FieldFilterList();
    }

    public override string GetChildrenFilterSql(
      TradeReportParameters parameters,
      bool isExternalOrganization)
    {
      string childrenFilterSql = string.Empty;
      FieldFilterList fieldFilterList = new FieldFilterList();
      string empty = string.Empty;
      if (this.Filters.Contains((object) ReportsFor.Loan) || parameters.CustomFilter != null)
      {
        if (!this.Filters.Contains((object) ReportsFor.Loan))
          this.Filters.Add((object) ReportsFor.Loan, (object) new FieldFilterList());
        string str = this.CreateIdentitySelectionQuery(parameters.CreateCombinedFilter(), isExternalOrganization).Replace(" distinct " + this.PrimaryKeyIdentifier, " distinct ISNULL(Loan.GUID, '') ");
        childrenFilterSql = childrenFilterSql + "create table #loan_guids ( LoanGUID varchar(38) PRIMARY KEY )" + Environment.NewLine;
        if (!string.IsNullOrEmpty(str))
          childrenFilterSql = childrenFilterSql + "insert into #loan_guids " + str + Environment.NewLine;
      }
      FieldFilterList newFilterList = new FieldFilterList();
      if (this.Filters.Contains((object) ReportsFor.SecurityTrades))
      {
        newFilterList.AddRange((IEnumerable<FieldFilter>) (this.Filters[(object) ReportsFor.SecurityTrades] as FieldFilterList));
        if (this.Filters.Contains((object) ReportsFor.MBSPools))
          newFilterList.AddRange((IEnumerable<FieldFilter>) (this.Filters[(object) ReportsFor.MBSPools] as FieldFilterList));
        string str = this.CreateIdentitySelectionQuery(parameters.CreateCombinedFilter(newFilterList), isExternalOrganization).Replace(" distinct " + this.PrimaryKeyIdentifier, " distinct ISNULL(" + this.SecurityTradeQuery.PrimaryKeyIdentifier + ", '') ");
        childrenFilterSql = childrenFilterSql + "create table #security_ids ( TradeID int PRIMARY KEY )" + Environment.NewLine;
        if (!string.IsNullOrEmpty(str))
          childrenFilterSql = childrenFilterSql + "insert into #security_ids " + str + Environment.NewLine;
      }
      return childrenFilterSql;
    }

    public override void GetCategories(List<ColumnInfo> fields)
    {
      foreach (ColumnInfo field in fields)
      {
        if (this.IsParentFields(field.CriterionName))
        {
          if (!this.Categories.Contains(ReportsFor.MBSPools))
            this.Categories.Add(ReportsFor.MBSPools);
        }
        else if (this.SecurityTradeQuery.IsParentFields(field.CriterionName))
        {
          if (!this.Categories.Contains(ReportsFor.SecurityTrades))
            this.Categories.Add(ReportsFor.SecurityTrades);
        }
        else if (!this.Categories.Contains(ReportsFor.Loan))
          this.Categories.Add(ReportsFor.Loan);
      }
    }

    public override IQueryTerm[] UseNullByReportsFor(ReportsFor reportsFor, List<ColumnInfo> fields)
    {
      List<DataField> dataFieldList = new List<DataField>();
      foreach (ColumnInfo field1 in fields)
      {
        DataField field2 = DataField.CreateField(field1.CriterionName, !field1.IsParent);
        if (field2 != null)
        {
          if (field2.UseNull)
          {
            if (reportsFor == ReportsFor.SecurityTrades && field1.FieldID.StartsWith("SecurityTrade"))
              field2.UseNull = false;
            if (reportsFor == ReportsFor.Loan && !field1.FieldID.StartsWith("SecurityTrade"))
              field2.UseNull = false;
          }
          dataFieldList.Add(field2);
        }
      }
      return (IQueryTerm[]) dataFieldList.ToArray();
    }

    public override bool IsParentReportFor(ReportsFor reportsFor)
    {
      return reportsFor == ReportsFor.MBSPools;
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
          if (str.Contains(" TradeAssignment TradesAssignment ") && this.Filters.Contains((object) ReportsFor.Loan))
            stringBuilder.AppendLine(str + " and TradesAssignment.LoanGUID in (select LoanGUID from #loan_guids)");
          else if (!str.Contains(" TradeAssignmentByTrade AssignedSecurityTrade ") && !str.Contains("SecurityTradeDetails") && !str.Contains("FN_GetSecurityTradeSummaryInline(1)"))
            stringBuilder.AppendLine(str);
        }
      }
      if (reportsFor == ReportsFor.SecurityTrades)
      {
        foreach (string str in strArray)
        {
          if (str.Contains(" TradeAssignmentByTrade AssignedSecurityTrade ") && this.Filters.Contains((object) ReportsFor.SecurityTrades))
            stringBuilder.AppendLine(str + " and AssignedSecurityTrade.AssigneeTradeID in (select TradeID from #security_ids)");
          else if (!str.Contains(" TradeAssignment TradesAssignment ") && !str.Contains("Loan"))
            stringBuilder.AppendLine(str);
        }
      }
      return stringBuilder.ToString();
    }
  }
}
