// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanQueryEngine
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.ServerObjects.PipelineEngine;
using EllieMae.EMLite.Server.ServerObjects.QueryEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class LoanQueryEngine
  {
    private static string className = nameof (LoanQueryEngine);

    public static List<Hashtable> GenerateQueryEngineWithPagination(
      UserInfo userInfo,
      string[] loanFolders,
      string[] guidList,
      string[] fields,
      QueryCriterion filter,
      SortField[] sortFields,
      CalculateTotalCountEnum calculateTotalCountType,
      out int totalCount,
      int start = 0,
      int limit = 0,
      int? maxCount = null,
      bool excludeArchivedLoans = false)
    {
      DataSet withPagination = LoanQueryEngine.GenerateWithPagination(LoanQueryEngine.PrepParameters(userInfo, loanFolders, guidList, fields, filter, sortFields, start, limit, maxCount, excludeArchivedLoans), calculateTotalCountType, out totalCount);
      List<Hashtable> engineWithPagination = new List<Hashtable>();
      if (withPagination != null && withPagination.Tables != null && withPagination.Tables.Count > 0 && withPagination.Tables[0].Rows != null)
      {
        PerformanceMeter.Current.AddNote("Loan Records = " + (object) totalCount);
        engineWithPagination = LoanQueryEngine.DataSetToQueryEngineInfo(withPagination);
      }
      return engineWithPagination;
    }

    public static QueryEngineDetail[] GenerateQueryEngineReportWithPagination(
      UserInfo userInfo,
      string[] loanFolders,
      string[] guidList,
      string[] fields,
      QueryCriterion filter,
      SortField[] sortFields,
      out int totalCount,
      int start = 0,
      int limit = 0,
      int? maxCount = null,
      bool excludeArchivedLoans = false)
    {
      QueryEngineParameters parameters = LoanQueryEngine.PrepParameters(userInfo, loanFolders, guidList, fields, filter, sortFields, start, limit, maxCount, excludeArchivedLoans);
      DataSet withPagination = LoanQueryEngine.GenerateWithPagination(parameters, CalculateTotalCountEnum.NoWait, out totalCount, true);
      QueryEngineDetail[] reportWithPagination = new QueryEngineDetail[0];
      if (withPagination != null && withPagination.Tables != null && withPagination.Tables.Count > 0 && withPagination.Tables[0].Rows != null)
      {
        PerformanceMeter.Current.AddNote("Loan Records = " + (object) totalCount);
        reportWithPagination = (QueryEngineDetail[]) LoanQueryEngine.DataSetToReportQueryEngineInfo(parameters.GuidList, withPagination).ToArray(typeof (QueryEngineDetail));
      }
      return reportWithPagination;
    }

    private static QueryEngineParameters PrepParameters(
      UserInfo userInfo,
      string[] loanFolders,
      string[] guidList,
      string[] fields,
      QueryCriterion filter,
      SortField[] sortFields,
      int start = 0,
      int limit = 0,
      int? maxCount = null,
      bool excludeArchivedLoans = false)
    {
      if (guidList != null)
        guidList = ((IEnumerable<string>) guidList).Where<string>((System.Func<string, bool>) (x => x != null)).Distinct<string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase).ToArray<string>();
      PipelinePagination pipelinePagination1;
      if (start < 0 || limit <= 0)
        pipelinePagination1 = (PipelinePagination) null;
      else
        pipelinePagination1 = new PipelinePagination()
        {
          Start = start,
          Limit = limit
        };
      PipelinePagination pipelinePagination2 = pipelinePagination1;
      return new QueryEngineParameters()
      {
        User = userInfo,
        LoanFolders = loanFolders,
        GuidList = guidList,
        Fields = fields,
        Filter = filter,
        SortFields = sortFields,
        PaginationInfo = pipelinePagination2,
        MaxCount = maxCount,
        ExcludeArchivedLoans = excludeArchivedLoans
      };
    }

    private static DataSet GenerateWithPagination(
      QueryEngineParameters parameters,
      CalculateTotalCountEnum calculateTotalCountType,
      out int totalCount,
      bool isGroupedByLoan = false)
    {
      DataSet withPagination = (DataSet) null;
      CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
      int recordsCount = -1;
      totalCount = -1;
      try
      {
        ClientContext context = ClientContext.GetCurrent();
        string correlationId = ClientContext.CurrentRequest?.CorrelationId;
        Guid? transactionId = (Guid?) ClientContext.CurrentRequest?.TransactionId;
        IDataCache requestCache = ClientContext.CurrentRequest?.RequestCache;
        Task task = (Task) null;
        if (calculateTotalCountType != CalculateTotalCountEnum.NoCount)
          task = Task.Run((Action) (() =>
          {
            using (context.MakeCurrent(requestCache, correlationId, transactionId, new bool?()))
              recordsCount = LoanQueryEngine.GenerateTotalCount(parameters, isGroupedByLoan);
          }), cancellationTokenSource.Token);
        Stopwatch stopwatch = Stopwatch.StartNew();
        withPagination = LoanQueryEngine.Generate(parameters);
        stopwatch.Stop();
        if (task != null && !task.IsCompleted)
        {
          if (calculateTotalCountType == CalculateTotalCountEnum.WaitForCount)
            task.Wait();
          else if (stopwatch.ElapsedMilliseconds > 3000L || !task.Wait((int) (3000L - stopwatch.ElapsedMilliseconds)))
            cancellationTokenSource.Cancel();
        }
        totalCount = recordsCount;
      }
      finally
      {
        cancellationTokenSource?.Dispose();
      }
      return withPagination;
    }

    private static DataSet Generate(QueryEngineParameters parameters)
    {
      if (parameters == null)
        return (DataSet) null;
      EllieMae.EMLite.Server.Query.LoanQuery loanQuery = new EllieMae.EMLite.Server.Query.LoanQuery(parameters.User, parameters.LoanFolders);
      bool flag1 = ((IEnumerable<string>) parameters.Fields).Contains<string>("Loan.IsArchived");
      if (flag1)
        parameters.Fields = ((IEnumerable<string>) parameters.Fields).Concat<string>((IEnumerable<string>) new string[1]
        {
          "LoanFolder.FolderName"
        }).ToArray<string>();
      IQueryTerm[] fields = (IQueryTerm[]) DataField.CreateFields(parameters.Fields);
      bool applyUserAccessFiltering = true;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      try
      {
        string text = string.Empty;
        bool flag2 = parameters.PaginationInfo != null && parameters.PaginationInfo.Start >= 0 && parameters.PaginationInfo.Limit > 0;
        bool flag3 = parameters.GuidList != null && ((IEnumerable<string>) parameters.GuidList).Count<string>() == 1 && Utils.ParseBoolean((object) Company.GetCompanySetting("FEATURE", "EnableOptParseLoanGuids"));
        if (flag2)
        {
          int? nullable1 = parameters.MaxCount;
          if (nullable1.HasValue)
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
            QueryEngineParameters engineParameters = parameters;
            nullable1 = new int?();
            int? nullable2 = nullable1;
            engineParameters.MaxCount = nullable2;
          }
        }
        if (parameters.SortFields != null && parameters.SortFields.Length != 0)
        {
          List<SortField> list = ((IEnumerable<SortField>) parameters.SortFields).ToList<SortField>();
          if (parameters.GuidList == null & flag2 && !((IEnumerable<SortField>) parameters.SortFields).Any<SortField>((System.Func<SortField, bool>) (t => t.Term.FieldName.Equals("Loan.Guid", StringComparison.OrdinalIgnoreCase))))
            list.Add(new SortField("Loan.Guid", FieldSortOrder.Ascending));
          text = loanQuery.GetOrderByClause(list.ToArray(), flag1 && list.Exists((Predicate<SortField>) (x => x.Term.FieldName == "[Loan].[IsArchived]")));
        }
        else if (flag2)
          text = dbQueryBuilder.OrderBy(new List<SortColumn>()
          {
            new SortColumn("Loan.Guid", SortOrder.Ascending)
          }, true);
        if (parameters.LoanFolders != null)
          dbQueryBuilder.AppendLine(LoanQueryEngine.GeneratesQueryWithTempTable(parameters.LoanFolders));
        if (parameters.GuidList != null)
        {
          if (!flag3)
          {
            dbQueryBuilder.AppendLine("-- Create a temp table for the loan guids to return");
            dbQueryBuilder.AppendLine("declare @loan_guids table ( guid varchar(38) primary key )");
            dbQueryBuilder.AppendLine("insert into @loan_guids select Guid from (values " + LoanQueryEngine.GuidsForBulkInsert((IEnumerable<string>) parameters.GuidList) + ") as dt(Guid)");
          }
          dbQueryBuilder.AppendLine(LoanQueryEngine.GetSelectQuery(parameters.Fields, parameters.MaxCount, loanQuery, fields));
          dbQueryBuilder.Append(loanQuery.GetTableSelectionClause(fields, parameters.Filter, parameters.SortFields, false, applyUserAccessFiltering, false));
          if (!flag3)
          {
            dbQueryBuilder.AppendLine("inner join @loan_guids guids on Loan.Guid = guids.Guid");
            dbQueryBuilder.Append(loanQuery.GetFilterClause(parameters.Filter, parameters.ExcludeArchivedLoans));
          }
          else
          {
            string filterClause = loanQuery.GetFilterClause(parameters.Filter, parameters.ExcludeArchivedLoans);
            if (filterClause != string.Empty)
            {
              dbQueryBuilder.Append(filterClause);
              dbQueryBuilder.Append(" and Loan.Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.GuidList[0]));
            }
            else
              dbQueryBuilder.Append(" where Loan.Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) parameters.GuidList[0]));
          }
          if (!string.IsNullOrWhiteSpace(text))
            dbQueryBuilder.AppendLine(text);
        }
        else if (flag2)
        {
          if (Company.GetCompanySetting("PIPELINE", "CompatibilityMode") == "2008")
          {
            int num1 = parameters.PaginationInfo.Start + 1;
            int num2 = parameters.PaginationInfo.Start + parameters.PaginationInfo.Limit;
            dbQueryBuilder.AppendLine(";with rowNumberCte as (");
            dbQueryBuilder.AppendLine(LoanQueryEngine.GetSelectQuery(parameters.Fields, parameters.MaxCount, loanQuery, fields));
            dbQueryBuilder.AppendLine(string.Format(", row_number() over ({0}) as RowNum ", (object) text));
            dbQueryBuilder.Append(loanQuery.GetTableSelectionClause(fields, parameters.Filter, parameters.SortFields, false, applyUserAccessFiltering, false));
            dbQueryBuilder.Append(loanQuery.GetFilterClause(parameters.Filter, parameters.ExcludeArchivedLoans));
            dbQueryBuilder.AppendLine(")");
            dbQueryBuilder.AppendLine(string.Format("select * from rowNumberCte where RowNum between {0} and {1}", (object) num1, (object) num2));
          }
          else
          {
            dbQueryBuilder.AppendLine(LoanQueryEngine.GetSelectQuery(parameters.Fields, parameters.MaxCount, loanQuery, fields));
            dbQueryBuilder.Append(loanQuery.GetTableSelectionClause(fields, parameters.Filter, parameters.SortFields, false, applyUserAccessFiltering, false));
            dbQueryBuilder.Append(loanQuery.GetFilterClause(parameters.Filter, parameters.ExcludeArchivedLoans));
            if (!string.IsNullOrWhiteSpace(text))
              dbQueryBuilder.AppendLine(text);
            dbQueryBuilder.AppendLine(string.Format(" offset {0} rows fetch next {1} rows only ", (object) parameters.PaginationInfo.Start, (object) parameters.PaginationInfo.Limit));
          }
        }
        else
        {
          dbQueryBuilder.AppendLine(LoanQueryEngine.GetSelectQuery(parameters.Fields, parameters.MaxCount, loanQuery, fields));
          dbQueryBuilder.Append(loanQuery.GetTableSelectionClause(fields, parameters.Filter, parameters.SortFields, false, applyUserAccessFiltering, false));
          dbQueryBuilder.Append(loanQuery.GetFilterClause(parameters.Filter, parameters.ExcludeArchivedLoans));
          if (!string.IsNullOrWhiteSpace(text))
            dbQueryBuilder.AppendLine(text);
        }
        return dbQueryBuilder.ExecuteSetQuery(DbTransactionType.Snapshot);
      }
      catch (Exception ex)
      {
        Err.Reraise(LoanQueryEngine.className, ex);
      }
      return (DataSet) null;
    }

    private static string GuidsForBulkInsert(IEnumerable<string> guids)
    {
      return "(" + string.Join("),(", guids.Select<string, string>((System.Func<string, string>) (guid => EllieMae.EMLite.DataAccess.SQL.EncodeString(guid))).ToArray<string>()) + ")";
    }

    private static string GetSelectQuery(
      string[] fieldsList,
      int? maxcount,
      EllieMae.EMLite.Server.Query.LoanQuery loanQuery,
      IQueryTerm[] queryTerms)
    {
      string selectQuery = !maxcount.HasValue ? "select Loan.Guid" : "select top " + (object) maxcount + " Loan.Guid";
      if (fieldsList != null && fieldsList.Length != 0 && loanQuery != null && queryTerms != null)
        selectQuery = selectQuery + ", " + loanQuery.GetFieldSelectionList(queryTerms);
      return selectQuery;
    }

    private static List<Hashtable> DataSetToQueryEngineInfo(DataSet result)
    {
      List<Hashtable> queryEngineInfo = new List<Hashtable>();
      try
      {
        if (result == null || result.Tables == null || result.Tables.Count <= 0 || result.Tables[0].Rows == null)
          return queryEngineInfo;
        int count = result.Tables[0].Rows.Count;
        for (int index = 0; index < count; ++index)
        {
          Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
          DataRow row = result.Tables[0].Rows[index];
          foreach (DataColumn column in (InternalDataCollectionBase) row.Table.Columns)
          {
            string criterionName = EllieMae.EMLite.ReportingDbUtils.Query.QueryEngine.ColumnNameToCriterionName(column.ColumnName);
            insensitiveHashtable[(object) criterionName] = EllieMae.EMLite.DataAccess.SQL.Decode(row[column.ColumnName]);
          }
          queryEngineInfo.Add(insensitiveHashtable);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(LoanQueryEngine.className, ex);
      }
      return queryEngineInfo;
    }

    private static ArrayList DataSetToReportQueryEngineInfo(string[] guidList, DataSet result)
    {
      ArrayList reportQueryEngineInfo = new ArrayList();
      try
      {
        if (result == null || result.Tables == null || result.Tables.Count <= 0 || result.Tables[0].Rows == null)
          return reportQueryEngineInfo;
        foreach (string str in guidList == null || !((IEnumerable<string>) guidList).Any<string>() ? result.Tables[0].Rows.Cast<DataRow>().Select<DataRow, string>((System.Func<DataRow, string>) (row => row["Guid"].ToString())).Distinct<string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase) : (IEnumerable<string>) guidList)
        {
          DataRow[] source = result.Tables[0].Select("Guid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(str));
          List<Hashtable> details = new List<Hashtable>();
          for (int index = 0; index < ((IEnumerable<DataRow>) source).Count<DataRow>(); ++index)
          {
            DataRow dataRow = source[index];
            Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
            foreach (DataColumn column in (InternalDataCollectionBase) dataRow.Table.Columns)
            {
              string criterionName = EllieMae.EMLite.ReportingDbUtils.Query.QueryEngine.ColumnNameToCriterionName(column.ColumnName);
              insensitiveHashtable[(object) criterionName] = EllieMae.EMLite.DataAccess.SQL.Decode(dataRow[column.ColumnName]);
            }
            details.Add(insensitiveHashtable);
          }
          reportQueryEngineInfo.Add((object) new QueryEngineDetail(details));
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(LoanQueryEngine.className, ex);
      }
      return reportQueryEngineInfo;
    }

    private static int GenerateTotalCount(
      QueryEngineParameters parameters,
      bool isGroupedByLoan = false,
      bool applyUserAccessFiltering = true)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      EllieMae.EMLite.Server.Query.LoanQuery loanQuery = new EllieMae.EMLite.Server.Query.LoanQuery(parameters.User, parameters.LoanFolders);
      IQueryTerm[] fields = (IQueryTerm[]) DataField.CreateFields(parameters.Fields);
      if (parameters.LoanFolders != null)
        dbQueryBuilder.AppendLine(LoanQueryEngine.GeneratesQueryWithTempTable(parameters.LoanFolders));
      if (isGroupedByLoan)
      {
        string[] guidList = parameters.GuidList;
        if ((guidList != null ? (((IEnumerable<string>) guidList).Any<string>() ? 1 : 0) : 0) != 0)
          return parameters.GuidList.Length;
        dbQueryBuilder.AppendLine("SELECT COUNT(DISTINCT Loan.Guid)");
      }
      else
        dbQueryBuilder.AppendLine("SELECT COUNT(Loan.Guid)");
      dbQueryBuilder.AppendLine(loanQuery.GetTableSelectionClause(fields, parameters.Filter, (SortField[]) null, false, applyUserAccessFiltering, false));
      dbQueryBuilder.AppendLine(loanQuery.GetFilterClause(parameters.Filter, parameters.ExcludeArchivedLoans));
      return Convert.ToInt32(dbQueryBuilder.ExecuteScalar());
    }

    private static string GeneratesQueryWithTempTable(string[] loanFolders)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("-- Create a temp table for the loan folders for filtering");
      dbQueryBuilder.AppendLine("\r\nif object_id('tempdb..#loanFolders', 'U') is not null\r\n\tdrop table #loanFolders\r\ncreate table #loanFolders(name varchar(250) primary key)");
      string str = string.Join(", ", ((IEnumerable<string>) loanFolders).Select<string, string>((System.Func<string, string>) (f => "(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) f) + ")")));
      dbQueryBuilder.AppendLine(string.Format("insert into #loanFolders values {0}", (object) str.ToString()));
      return dbQueryBuilder.ToString();
    }
  }
}
