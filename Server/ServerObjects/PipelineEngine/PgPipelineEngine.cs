// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.PipelineEngine.PgPipelineEngine
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.PipelineEngine
{
  internal class PgPipelineEngine(ClientContext context) : PiplineEngineBase(context)
  {
    protected override PipelineInfo[] GeneratePipelineInternal(
      PipelineParameters parameters,
      out int totalCount)
    {
      string loanFolder = "";
      totalCount = -1;
      EllieMae.EMLite.Server.Query.PgQuery.LoanFieldTranslator fieldTranslator = new EllieMae.EMLite.Server.Query.PgQuery.LoanFieldTranslator();
      EllieMae.EMLite.Server.Query.PgQuery.LoanQuery loanQuery = new EllieMae.EMLite.Server.Query.PgQuery.LoanQuery(parameters.User, parameters.AccessRights, (ICriterionTranslator) fieldTranslator, loanFolder, parameters.LoanFolders != null || parameters.GuidList != null || Pipeline.IsExclusiveFilter(parameters.Filter));
      DataField[] fields = DataField.CreateFields(parameters.Fields);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder((IClientContext) ClientContext.GetCurrent());
      bool flag1 = parameters.DataToInclude == PipelineData.Fields && parameters.GuidList == null && parameters.IdentitySelectionQuery == null;
      Guid guid;
      if (!flag1)
      {
        if (parameters.GuidList == null || parameters.GuidList.Length < 1)
        {
          PipelineParameters pipelineParameters = parameters;
          string[] strArray = new string[1];
          guid = Guid.Empty;
          strArray[0] = guid.ToString();
          pipelineParameters.GuidList = strArray;
        }
        string str;
        if (parameters.GuidList != null)
        {
          if (parameters.GuidList.Length < 1)
          {
            PipelineParameters pipelineParameters = parameters;
            string[] strArray = new string[1];
            guid = Guid.Empty;
            strArray[0] = guid.ToString();
            pipelineParameters.GuidList = strArray;
          }
          str = "select string_to_array(" + Pipeline.EncodeParseableGuidList(parameters.GuidList, ",") + ", ',')";
        }
        else if (parameters.IdentitySelectionQuery != null && parameters.User != (UserInfo) null)
          str = "select Guid from LoanSummary where Guid in (" + parameters.IdentitySelectionQuery + ") and Guid in (" + loanQuery.CreateIdentitySelectionQuery(parameters.IsExternalOrganization) + ")";
        else
          str = parameters.IdentitySelectionQuery == null ? loanQuery.CreateIdentitySelectionQuery(Pipeline.CreateCombinedFilterCriterion(parameters.LoanFolders, parameters.Filter), parameters.IsExternalOrganization) : parameters.IdentitySelectionQuery;
        string text = Environment.NewLine + " with ElliLoanEntity as  (      select EntityId, EntityData      from  (select EntityId, EntityData from ElliLoan where EntityType = 1)  as ElliLoanEntity     where " + loanQuery.PrimaryKeyIdentifier + " =          any (              array (                  " + str + "             )::uuid[]          )  ) " + Environment.NewLine;
        dbQueryBuilder.AppendLine(text);
        loanQuery.SetPrimaryKeyTableIdentifier("ElliLoanEntity");
      }
      if (parameters.LoanFolders != null)
      {
        StringValueCriterion criterion = new StringValueCriterion("Loan.LoanFolder", EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) parameters.LoanFolders), StringMatchType.MultiValue);
        parameters.Filter = parameters.Filter == null ? (QueryCriterion) criterion : parameters.Filter.And((QueryCriterion) criterion);
      }
      if (parameters.Fields == null)
      {
        fields = DataField.CreateFields(new string[1]
        {
          "Loan.Guid"
        });
        dbQueryBuilder.AppendLine("select Loan.*");
      }
      else if (parameters.Fields.Length == 0)
      {
        fields = DataField.CreateFields(new string[1]
        {
          "Loan.Guid"
        });
        dbQueryBuilder.AppendLine("select Loan.Guid");
      }
      else
        dbQueryBuilder.AppendLine("select Loan.Guid, " + loanQuery.GetFieldSelectionList((IQueryTerm[]) fields));
      bool flag2 = parameters.PaginationInfo != null && parameters.PaginationInfo.Start >= 0 && parameters.PaginationInfo.Limit > 0;
      int? nullable1;
      if (flag2 && parameters.MaxCount.HasValue)
      {
        PipelinePagination paginationInfo = parameters.PaginationInfo;
        int limit1 = parameters.PaginationInfo.Limit;
        nullable1 = parameters.MaxCount;
        int valueOrDefault = nullable1.GetValueOrDefault();
        int limit2;
        if (!(limit1 > valueOrDefault & nullable1.HasValue))
        {
          limit2 = parameters.PaginationInfo.Limit;
        }
        else
        {
          nullable1 = parameters.MaxCount;
          limit2 = nullable1.Value;
        }
        paginationInfo.Limit = limit2;
        PipelineParameters pipelineParameters = parameters;
        nullable1 = new int?();
        int? nullable2 = nullable1;
        pipelineParameters.MaxCount = nullable2;
      }
      if (flag2)
        dbQueryBuilder.AppendLine(",COUNT(*) OVER() As \"TotalRowCount\" ");
      if (flag1)
      {
        dbQueryBuilder.Append(loanQuery.GetTableSelectionClause((IQueryTerm[]) fields, parameters.Filter, parameters.SortFields, false, true, parameters.IsExternalOrganization));
        dbQueryBuilder.Append(loanQuery.GetFilterClause(parameters.Filter, false));
      }
      else
        dbQueryBuilder.Append(loanQuery.GetTableSelectionClause((IQueryTerm[]) fields, (QueryCriterion) null, parameters.SortFields, false, false, parameters.IsExternalOrganization));
      if (parameters.SortFields != null && parameters.SortFields.Length != 0)
        dbQueryBuilder.Append(loanQuery.GetOrderByClause(parameters.SortFields));
      else if (flag2)
        dbQueryBuilder.AppendLine(" ORDER BY Guid ");
      nullable1 = parameters.MaxCount;
      if (nullable1.HasValue)
        dbQueryBuilder.Append(" limit " + (object) parameters.MaxCount);
      else if (flag2)
        dbQueryBuilder.AppendLine(string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", (object) parameters.PaginationInfo.Start, (object) parameters.PaginationInfo.Limit));
      dbQueryBuilder.Append(";");
      Bitmask bitmask = new Bitmask((object) parameters.DataToInclude);
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      PerformanceMeter.Current.AddNote("Loan Summary Records = " + (object) dataSet.Tables[0].Rows.Count);
      int count = dataSet.Tables[0].Rows.Count;
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < count; ++index)
      {
        Hashtable info = new Hashtable();
        DataRow row = dataSet.Tables[0].Rows[index];
        guid = new Guid(row["Guid"].ToString());
        string str1 = guid.ToString("B");
        if (!dictionary.ContainsKey(str1))
        {
          dictionary[str1] = true;
          foreach (DataColumn column in (InternalDataCollectionBase) row.Table.Columns)
          {
            string criterionName = QueryEngine.ColumnNameToCriterionName(column.ColumnName);
            if (row[column.ColumnName].GetType() == typeof (Guid))
            {
              Hashtable hashtable = info;
              string key = criterionName;
              guid = new Guid(EllieMae.EMLite.DataAccess.SQL.Decode(row[column.ColumnName]).ToString());
              string str2 = guid.ToString("B");
              hashtable[(object) key] = (object) str2;
            }
            else
              info[(object) criterionName] = EllieMae.EMLite.DataAccess.SQL.Decode(row[column.ColumnName]);
          }
          PipelineInfo.Alert[] alerts = (PipelineInfo.Alert[]) null;
          PipelineInfo.AlertSummaryInfo alertSummary = (PipelineInfo.AlertSummaryInfo) null;
          PipelineInfo.Borrower[] borrowers = (PipelineInfo.Borrower[]) null;
          LockInfo lockInfo = new LockInfo(str1);
          List<LockInfo> locks = new List<LockInfo>();
          Hashtable rights = (Hashtable) null;
          PipelineInfo.MilestoneInfo[] milestones = (PipelineInfo.MilestoneInfo[]) null;
          PipelineInfo.LoanAssociateInfo[] loanAssociates = (PipelineInfo.LoanAssociateInfo[]) null;
          PipelineInfo.TradeInfo assignedTrade = (PipelineInfo.TradeInfo) null;
          PipelineInfo.TradeInfo[] tradeAssignments = (PipelineInfo.TradeInfo[]) null;
          string[] rejectedInvestors = (string[]) null;
          arrayList.Add((object) new PipelineInfo(info, borrowers, alerts, alertSummary, loanAssociates, lockInfo, locks, rights, milestones, assignedTrade, tradeAssignments, rejectedInvestors));
        }
      }
      return (PipelineInfo[]) arrayList.ToArray(typeof (PipelineInfo));
    }
  }
}
