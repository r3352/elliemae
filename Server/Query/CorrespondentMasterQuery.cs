// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Query.CorrespondentMasterQuery
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
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.Query
{
  internal class CorrespondentMasterQuery : QueryEngine
  {
    private CorrespondentTradeQuery correspondentTradeQuery;
    private Dictionary<string, FieldFormat> fieldIdMap = new Dictionary<string, FieldFormat>();
    private string secondKeyTableIdentifier = "CorrespondentMasterDeliveryMethod";
    private string secondKeyIdentifier = "CorrespondentMasterDeliveryMethod.CorrespondentMasterDeliveryMethodID";
    private FieldFilterList list;

    public CorrespondentMasterQuery(UserInfo currentUser)
      : base(ClientContext.GetCurrent().Settings.ConnectionString, ClientContext.GetCurrent().Settings.DbServerType, currentUser, (ICriterionTranslator) new CorrespondentMasterFieldTranslator())
    {
      this.correspondentTradeQuery = new CorrespondentTradeQuery(currentUser);
    }

    public override string PrimaryKeyTableIdentifier => "CorrespondentMaster";

    public override string PrimaryKeyIdentifier => "CorrespondentMaster.CorrespondentMasterID";

    public override string UserAccessQueryKeyColumnName => "CorrespondentMasterID";

    public override FieldSource GetFieldSource(string name)
    {
      switch (name.ToLower())
      {
        case "tradecorrespondenttradesummary":
          return new FieldSource("TradeCorrespondentTradeSummary", "left outer join FN_GetCorrespondentTradeSummaryInline(" + (object) 1 + ") TradeCorrespondentTradeSummary on CorrespondentTradeDetails.TradeID = TradeCorrespondentTradeSummary.TradeID", new string[1]
          {
            "CorrespondentTradeDetails"
          });
        case "correspondenttradedetails":
          return new FieldSource("CorrespondentTradeDetails", "left outer join CorrespondentTradeDetails on CorrespondentMaster.CorrespondentMasterID = CorrespondentTradeDetails.CorrespondentMasterID", new string[1]
          {
            "CorrespondentMaster"
          });
        case "correspondentmasterdeliverymethod":
          return new FieldSource("CorrespondentMasterDeliveryMethod", "left outer join CorrespondentMasterDeliveryMethod on CorrespondentMaster.CorrespondentMasterID = CorrespondentMasterDeliveryMethod.CorrespondentMasterID", new string[1]
          {
            "CorrespondentMaster"
          });
        default:
          return this.correspondentTradeQuery.GetFieldSource(name);
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
          this.secondKeyTableIdentifier
        };
      }
    }

    public override FieldFilterList GetParentFilters()
    {
      return this.Filters.Contains((object) ReportsFor.CorrespondentMasters) ? this.Filters[(object) ReportsFor.CorrespondentMasters] as FieldFilterList : new FieldFilterList();
    }

    public override bool IsParentReportFor(ReportsFor reportsFor)
    {
      return reportsFor == ReportsFor.CorrespondentMasters;
    }

    public override void SplitFiltersByReportsFor(FieldFilterList filterList)
    {
      foreach (FieldFilter filter in (List<FieldFilter>) filterList)
      {
        if (this.IsParentFields(filter.CriterionName))
        {
          if (!this.Filters.Contains((object) ReportsFor.CorrespondentMasters))
            this.Filters.Add((object) ReportsFor.CorrespondentMasters, (object) new FieldFilterList());
          (this.Filters[(object) ReportsFor.CorrespondentMasters] as FieldFilterList).Add(filter);
        }
        else if (this.correspondentTradeQuery.IsParentFields(filter.CriterionName))
        {
          if (!this.Filters.Contains((object) ReportsFor.CorrespondentTrades))
            this.Filters.Add((object) ReportsFor.CorrespondentTrades, (object) new FieldFilterList());
          (this.Filters[(object) ReportsFor.CorrespondentTrades] as FieldFilterList).Add(filter);
        }
      }
    }

    private string GetDeliveryTypeFilters(FieldFilterList list)
    {
      FieldFilterList fieldFilterList = new FieldFilterList();
      string deliveryTypeFilters = "";
      foreach (FieldFilter fieldFilter in (List<FieldFilter>) list)
      {
        if (fieldFilter.CriterionName.Contains(this.secondKeyTableIdentifier))
          fieldFilterList.Add(fieldFilter);
      }
      if (fieldFilterList != null)
      {
        string filterClause = this.GetFilterClause(fieldFilterList.CreateEvaluator().ToQueryCriterion());
        if (!string.IsNullOrEmpty(filterClause))
          deliveryTypeFilters = deliveryTypeFilters + Environment.NewLine + filterClause;
      }
      return deliveryTypeFilters;
    }

    private FieldFilterList GetNonDeliveryTypeFilters(FieldFilterList list)
    {
      FieldFilterList deliveryTypeFilters = new FieldFilterList();
      foreach (FieldFilter fieldFilter in (List<FieldFilter>) list)
      {
        if (!fieldFilter.CriterionName.Contains(this.secondKeyTableIdentifier))
          deliveryTypeFilters.Add(fieldFilter);
      }
      return deliveryTypeFilters;
    }

    public override string GetChildrenFilterSql(
      TradeReportParameters parameters,
      bool isExternalOrganization)
    {
      this.list = parameters.FieldFilters;
      string childrenFilterSql = string.Empty;
      QueryCriterion combinedFilter = parameters.CreateCombinedFilter();
      if (combinedFilter != null)
      {
        if (combinedFilter.UsesTable(this.secondKeyTableIdentifier))
        {
          string str = "SELECT " + this.secondKeyIdentifier + ",CorrespondentMasterID, DeliveryType from " + this.secondKeyTableIdentifier + this.GetDeliveryTypeFilters(parameters.FieldFilters);
          childrenFilterSql = childrenFilterSql + "create table #corrDeliveryType_methodids ( CorrespondentMasterDeliveryMethodID varchar(38) PRIMARY KEY,CorrespondentMasterID int, DeliveryType int )" + Environment.NewLine;
          if (!string.IsNullOrEmpty(str))
            childrenFilterSql = childrenFilterSql + "insert into #corrDeliveryType_methodids " + str + Environment.NewLine;
        }
        if (this.Filters.Contains((object) ReportsFor.CorrespondentTrades))
        {
          string identitySelectionQuery = this.CreateIdentitySelectionQuery(this.GetNonDeliveryTypeFilters(parameters.FieldFilters).CreateEvaluator().ToQueryCriterion(), isExternalOrganization);
          childrenFilterSql = childrenFilterSql + "create table #correspondenttrade_ids ( TradeID int PRIMARY KEY )" + Environment.NewLine;
          if (!combinedFilter.UsesTable(this.secondKeyTableIdentifier))
          {
            string str = identitySelectionQuery.Replace(" distinct " + this.PrimaryKeyIdentifier, " ISNULL(" + this.correspondentTradeQuery.PrimaryKeyIdentifier + ", '')");
            if (!string.IsNullOrEmpty(str))
              childrenFilterSql = childrenFilterSql + "insert into #correspondenttrade_ids " + str + Environment.NewLine;
          }
          else
          {
            string str = identitySelectionQuery.Replace(" distinct " + this.PrimaryKeyIdentifier, " ISNULL(" + this.correspondentTradeQuery.PrimaryKeyIdentifier + ", ''), CorrespondentTradeDetails.CorrespondentMasterID, CorrespondentTradeDetails.DeliveryType");
            childrenFilterSql = childrenFilterSql + "create table #temp (TradeID int PRIMARY KEY, CorrespondentMasterID int, DeliveryType int)" + Environment.NewLine + "insert into #temp " + str + Environment.NewLine + "insert into  #correspondenttrade_ids select TradeID from #temp inner join #corrDeliveryType_methodids on #temp.CorrespondentMasterID = #corrDeliveryType_methodids.CorrespondentMasterID AND #temp.DeliveryType = #corrDeliveryType_methodids.DeliveryType" + Environment.NewLine;
          }
        }
      }
      return childrenFilterSql;
    }

    public override void GetCategories(List<ColumnInfo> fields)
    {
      foreach (ColumnInfo field in fields)
      {
        if (this.IsParentFields(field.CriterionName))
        {
          if (!this.Categories.Contains(ReportsFor.CorrespondentMasters))
            this.Categories.Add(ReportsFor.CorrespondentMasters);
        }
        else if (this.correspondentTradeQuery.IsParentFields(field.CriterionName) && !this.Categories.Contains(ReportsFor.CorrespondentTrades))
          this.Categories.Add(ReportsFor.CorrespondentTrades);
      }
    }

    public override string GetChildrenTableJoins(ReportsFor reportsFor, string tableJoins)
    {
      string[] strArray = tableJoins.Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.None);
      string str1 = "";
      string str2 = "";
      string str3 = "";
      bool flag1 = this.Filters.Contains((object) ReportsFor.CorrespondentTrades) && this.Filters.Contains((object) ReportsFor.CorrespondentMasters) && !string.IsNullOrEmpty(this.GetDeliveryTypeFilters((FieldFilterList) this.Filters[(object) ReportsFor.CorrespondentMasters]));
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      if (reportsFor == ReportsFor.CorrespondentTrades)
      {
        foreach (string str4 in strArray)
        {
          if (str4.Contains("left outer join CorrespondentTradeDetails"))
            flag2 = true;
          if (str4.Contains("left outer join FN_GetCorrespondentTradeSummaryInline"))
            flag3 = true;
          if (str4.Contains("left outer join CorrespondentMasterDeliveryMethod"))
            flag4 = true;
        }
      }
      if (reportsFor == ReportsFor.CorrespondentTrades)
      {
        foreach (string str5 in strArray)
        {
          if (str5.Contains("left outer join CorrespondentTradeDetails"))
          {
            str2 = !this.Filters.Contains((object) ReportsFor.CorrespondentTrades) ? str5 : str5 + " and Correspondenttradedetails.TradeID in (select TradeID from #correspondenttrade_ids)";
            if (flag4)
              str2 += " and Correspondenttradedetails.DeliveryType = CorrespondentMasterDeliveryMethod.DeliveryType and Correspondenttradedetails.CorrespondentMasterID = CorrespondentMasterDeliveryMethod.CorrespondentMasterID ";
          }
          if (str5.Contains("left outer join CorrespondentMasterDeliveryMethod"))
          {
            str1 = !this.Filters.Contains((object) ReportsFor.CorrespondentMasters) || string.IsNullOrEmpty(this.GetDeliveryTypeFilters((FieldFilterList) this.Filters[(object) ReportsFor.CorrespondentMasters])) ? str5 : str5 + " and CorrespondentMasterDeliveryMethod.CorrespondentMasterDeliveryMethodID in (select CorrespondentMasterDeliveryMethodID from #corrDeliveryType_methodids)";
            if (!flag2 & flag3)
              str1 = str1 + " left outer join CorrespondentDetails on CorrespondentMaster.CorrespondentMasterID = CorrespondentTradeDetails.CorrespondentMasterID" + str5 + " and Correspondenttradedetails.DeliveryType = CorrespondentMasterDeliveryMethod.DeliveryType and Correspondenttradedetails.CorrespondentMasterID = CorrespondentMasterDeliveryMethod.CorrespondentMasterID";
          }
          if (str5.Contains("left outer join FN_GetCorrespondentTradeSummaryInline"))
          {
            if (this.Filters.Contains((object) ReportsFor.CorrespondentTrades))
            {
              str3 = str5 + " and TradeCorrespondentTradeSummary.TradeID in (select TradeID from #correspondenttrade_ids)";
              if (flag4 && !flag2)
                str3 = str5 + " on TradeCorrespondentTradeSummary.TradeID = Correspondenttradedetails.TradeID";
            }
            else
              str3 = str5;
          }
        }
      }
      return str1 + " " + str2 + " " + str3;
    }

    public override IQueryTerm[] UseNullByReportsFor(ReportsFor reportsFor, List<ColumnInfo> fields)
    {
      return (IQueryTerm[]) DataField.CreateFields(fields.Select<ColumnInfo, string>((Func<ColumnInfo, string>) (f => f.CriterionName)).ToArray<string>());
    }
  }
}
