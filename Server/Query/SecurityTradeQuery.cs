// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Query.SecurityTradeQuery
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
  internal class SecurityTradeQuery : QueryEngine
  {
    private TradeQuery tradeQuery;
    private MbsPoolQuery mbsPoolQuery;
    private Dictionary<string, FieldFormat> fieldIdMap = new Dictionary<string, FieldFormat>();
    private UserInfo currentUser;

    public SecurityTradeQuery(UserInfo currentUser)
      : base(ClientContext.GetCurrent().Settings.ConnectionString, ClientContext.GetCurrent().Settings.DbServerType, currentUser, (ICriterionTranslator) new SecurityTradeFieldTranslator())
    {
      this.tradeQuery = new TradeQuery(currentUser);
      this.currentUser = currentUser;
    }

    public MbsPoolQuery MbsPoolQuery
    {
      get
      {
        if (this.mbsPoolQuery == null)
          this.mbsPoolQuery = new MbsPoolQuery(this.currentUser);
        return this.mbsPoolQuery;
      }
    }

    public override string PrimaryKeyTableIdentifier => "SecurityTradeDetails";

    public override string PrimaryKeyIdentifier => "SecurityTradeDetails.TradeID";

    public override string UserAccessQueryKeyColumnName => "TradeID";

    public override FieldSource GetFieldSource(string name)
    {
      switch (name.ToLower())
      {
        case "securitytradedetails":
          return new FieldSource("SecurityTradeDetails", "left outer join FN_GetSecurityTradeSummaryInline(" + (object) 1 + ") SecurityTradeSummary on SecurityTradeDetails.TradeID = SecurityTradeSummary.TradeID");
        case "securitytradesummary":
          return new FieldSource("SecurityTradeDetails", "left outer join FN_GetSecurityTradeSummaryInline(" + (object) 1 + ") SecurityTradeSummary on SecurityTradeDetails.TradeID = SecurityTradeSummary.TradeID");
        case "tradeassignmentbytrade":
          return new FieldSource("TradeAssignmentByTrade", "left outer join TradeAssignmentByTrade TradeAssignmentByTrade on SecurityTradeDetails.TradeID = TradeAssignmentByTrade.TradeID");
        case "loantradedetails":
          return new FieldSource("LoanTradeDetails", "left outer join LoanTradeDetails LoanTradeDetails on LoanTradeDetails.TradeID = TradeAssignmentByTrade.AssigneeTradeID", new string[1]
          {
            "TradeAssignmentByTrade"
          });
        case "tradeassignmentbytrade1":
          return new FieldSource("TradeAssignmentByTrade1", "left outer join TradeAssignmentByTrade TradeAssignmentByTrade1 on SecurityTradeDetails.TradeID = TradeAssignmentByTrade1.AssigneeTradeID");
        case "mbspooldetails":
          return new FieldSource("MbsPoolDetails", "left outer join MbsPoolDetails MbsPoolDetails on MbsPoolDetails.TradeID = TradeAssignmentByTrade1.TradeID", new string[1]
          {
            "TradeAssignmentByTrade1"
          });
        default:
          return this.tradeQuery.GetFieldSource(name) ?? this.MbsPoolQuery.GetFieldSource(name);
      }
    }

    public override string GetUserAccessFilterJoinClause(bool isExternalOrganization) => "";

    public override List<string> ParentTables
    {
      get
      {
        return new List<string>()
        {
          "SecurityTradeDetails",
          "SecurityTradeSummary"
        };
      }
    }

    public override void SplitFiltersByReportsFor(FieldFilterList filterList)
    {
      foreach (FieldFilter filter in (List<FieldFilter>) filterList)
      {
        if (this.IsParentFields(filter.CriterionName))
        {
          if (!this.Filters.Contains((object) ReportsFor.SecurityTrades))
            this.Filters.Add((object) ReportsFor.SecurityTrades, (object) new FieldFilterList());
          (this.Filters[(object) ReportsFor.SecurityTrades] as FieldFilterList).Add(filter);
        }
        else if (this.tradeQuery.IsParentFields(filter.CriterionName))
        {
          if (!this.Filters.Contains((object) ReportsFor.LoanTrades))
            this.Filters.Add((object) ReportsFor.LoanTrades, (object) new FieldFilterList());
          (this.Filters[(object) ReportsFor.LoanTrades] as FieldFilterList).Add(filter);
        }
        else if (this.MbsPoolQuery.IsParentFields(filter.CriterionName))
        {
          if (!this.Filters.Contains((object) ReportsFor.MBSPools))
            this.Filters.Add((object) ReportsFor.MBSPools, (object) new FieldFilterList());
          (this.Filters[(object) ReportsFor.MBSPools] as FieldFilterList).Add(filter);
        }
      }
    }

    public override FieldFilterList GetParentFilters()
    {
      return this.Filters.Contains((object) ReportsFor.SecurityTrades) ? this.Filters[(object) ReportsFor.SecurityTrades] as FieldFilterList : new FieldFilterList();
    }

    public override string GetChildrenFilterSql(
      TradeReportParameters parameters,
      bool isExternalOrganization)
    {
      string childrenFilterSql = string.Empty;
      FieldFilterList newFilterList1 = new FieldFilterList();
      string empty = string.Empty;
      if (this.Filters.Contains((object) ReportsFor.LoanTrades))
      {
        newFilterList1.AddRange((IEnumerable<FieldFilter>) (this.Filters[(object) ReportsFor.LoanTrades] as FieldFilterList));
        if (this.Filters.Contains((object) ReportsFor.SecurityTrades))
          newFilterList1.AddRange((IEnumerable<FieldFilter>) (this.Filters[(object) ReportsFor.SecurityTrades] as FieldFilterList));
        QueryEngine tradeQuery = (QueryEngine) this.tradeQuery;
        string str = this.CreateIdentitySelectionQuery(parameters.CreateCombinedFilter(newFilterList1), isExternalOrganization).Replace(" distinct " + this.PrimaryKeyIdentifier, " distinct ISNULL(" + this.tradeQuery.PrimaryKeyIdentifier + ", '') ");
        childrenFilterSql = childrenFilterSql + "create table #loantrade_ids ( TradeID int PRIMARY KEY )" + Environment.NewLine;
        if (!string.IsNullOrEmpty(str))
          childrenFilterSql = childrenFilterSql + "insert into #loantrade_ids " + str + Environment.NewLine;
      }
      FieldFilterList newFilterList2 = new FieldFilterList();
      if (this.Filters.Contains((object) ReportsFor.MBSPools))
      {
        newFilterList2.AddRange((IEnumerable<FieldFilter>) (this.Filters[(object) ReportsFor.MBSPools] as FieldFilterList));
        if (this.Filters.Contains((object) ReportsFor.SecurityTrades))
          newFilterList2.AddRange((IEnumerable<FieldFilter>) (this.Filters[(object) ReportsFor.SecurityTrades] as FieldFilterList));
        string str = this.CreateIdentitySelectionQuery(parameters.CreateCombinedFilter(newFilterList2), isExternalOrganization).Replace(" distinct " + this.PrimaryKeyIdentifier, " distinct ISNULL(" + this.MbsPoolQuery.PrimaryKeyIdentifier + ", '') ");
        childrenFilterSql = childrenFilterSql + "create table #mbspool_ids ( TradeID int PRIMARY KEY )" + Environment.NewLine;
        if (!string.IsNullOrEmpty(str))
          childrenFilterSql = childrenFilterSql + "insert into #mbspool_ids " + str + Environment.NewLine;
      }
      return childrenFilterSql;
    }

    public override void GetCategories(List<ColumnInfo> fields)
    {
      foreach (ColumnInfo field in fields)
      {
        if (this.IsParentFields(field.CriterionName))
        {
          if (!this.Categories.Contains(ReportsFor.SecurityTrades))
            this.Categories.Add(ReportsFor.SecurityTrades);
        }
        else if (this.tradeQuery.IsParentFields(field.CriterionName))
        {
          if (!this.Categories.Contains(ReportsFor.LoanTrades))
            this.Categories.Add(ReportsFor.LoanTrades);
        }
        else if (this.MbsPoolQuery.IsParentFields(field.CriterionName) && !this.Categories.Contains(ReportsFor.MBSPools))
          this.Categories.Add(ReportsFor.MBSPools);
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
          if (field2.UseNull && (reportsFor == ReportsFor.LoanTrades && field1.FieldID.StartsWith("LoanTrade") || reportsFor == ReportsFor.MBSPools && field1.FieldID.Contains("MBSPool")))
            field2.UseNull = false;
          dataFieldList.Add(field2);
        }
      }
      return (IQueryTerm[]) dataFieldList.ToArray();
    }

    public override bool IsParentReportFor(ReportsFor reportsFor)
    {
      return reportsFor == ReportsFor.SecurityTrades;
    }

    public override string GetChildrenTableJoins(ReportsFor reportsFor, string tableJoins)
    {
      string[] strArray = tableJoins.Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.None);
      StringBuilder stringBuilder = new StringBuilder();
      if (reportsFor == ReportsFor.LoanTrades)
      {
        foreach (string str in strArray)
        {
          if (str.Contains(" TradeAssignmentByTrade TradeAssignmentByTrade ") && this.Filters.Contains((object) ReportsFor.LoanTrades))
            stringBuilder.AppendLine(str + " and TradeAssignmentByTrade.AssigneeTradeID in (select TradeID from #loantrade_ids)");
          else if (!str.Contains(" TradeAssignmentByTrade TradeAssignmentByTrade1 ") && !str.Contains("MbsPoolDetails") && !str.Contains("FN_GetMbsPoolSummaryInline(1)") && !str.Contains("Trades AssignedSecurityTradeDetails1 "))
            stringBuilder.AppendLine(str);
        }
      }
      if (reportsFor == ReportsFor.MBSPools)
      {
        foreach (string str in strArray)
        {
          if (str.Contains(" TradeAssignmentByTrade TradeAssignmentByTrade1 ") && this.Filters.Contains((object) ReportsFor.MBSPools))
            stringBuilder.AppendLine(str + " and TradeAssignmentByTrade1.TradeID in (select TradeID from #mbspool_ids)");
          else if (!str.Contains(" TradeAssignmentByTrade TradeAssignmentByTrade ") && !str.Contains("LoanTradeDetails") && !str.Contains("FN_GetLoanTradeSummaryInline(1)") && !str.Contains("Trades AssignedSecurityTradeDetails "))
            stringBuilder.AppendLine(str);
        }
      }
      return stringBuilder.ToString();
    }
  }
}
