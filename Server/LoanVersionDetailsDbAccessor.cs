// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanVersionDetailsDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class LoanVersionDetailsDbAccessor
  {
    private const string tableName = "[LoanVersionDetails]�";

    public static List<LoanVersionPipelineInfo> GenerateLoanVersionWithPagination(
      DateTime? startDate,
      DateTime? endDate,
      out int totalCount,
      string[] guidList = null,
      int start = 0,
      int limit = 0,
      int maxGuidsPerQuery = 1000)
    {
      return guidList == null ? LoanVersionDetailsDbAccessor.GetLoanVersionForPagination(startDate, endDate, out totalCount, start, limit, maxGuidsPerQuery) : LoanVersionDetailsDbAccessor.GetLoanVersionForGuids(startDate, endDate, guidList, out totalCount, maxGuidsPerQuery);
    }

    private static List<LoanVersionPipelineInfo> GetLoanVersionForPagination(
      DateTime? startDate,
      DateTime? endDate,
      out int totalcount,
      int start,
      int limit,
      int maxGuidsPerQuery)
    {
      totalcount = 0;
      start = start <= 0 ? 0 : start;
      limit = limit <= 0 || limit >= maxGuidsPerQuery ? maxGuidsPerQuery : limit;
      string str = LoanVersionDetailsDbAccessor.DateWhereClause(startDate, endDate);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select lvd.GuId as LoanId, lvd.DateCreated, lvd.VersionNumber, loanver.total_cnt");
      dbQueryBuilder.AppendLine("from [LoanVersionDetails] lvd");
      dbQueryBuilder.AppendLine("inner join (");
      dbQueryBuilder.AppendLine("  select *, Max(rn) over() as total_cnt");
      dbQueryBuilder.AppendLine("  from (");
      dbQueryBuilder.AppendLine("    select lvd.Guid, row_number() over(order by MAX(lvd.DateCreated) desc) rn");
      dbQueryBuilder.AppendLine("    from [LoanVersionDetails] lvd");
      if (!string.IsNullOrWhiteSpace(str))
        dbQueryBuilder.AppendLine("    " + str);
      dbQueryBuilder.AppendLine("    group by lvd.guid ");
      dbQueryBuilder.AppendLine("  ) loanVerRows");
      dbQueryBuilder.AppendLine(") loanver on loanver.guid = lvd.GuId");
      if (!string.IsNullOrWhiteSpace(str))
        dbQueryBuilder.AppendLine(str ?? "");
      dbQueryBuilder.AppendLine(string.Format("{0} loanver.rn between {1} and {2}", !string.IsNullOrWhiteSpace(str) ? (object) "and" : (object) "where", (object) (start + 1), (object) (limit + start)));
      dbQueryBuilder.AppendLine("order by loanver.rn, lvd.VersionNumber desc");
      DataTable table = dbQueryBuilder.ExecuteTableQuery();
      if (table.Rows.Count > 0)
        totalcount = Convert.ToInt32(table.Rows[0]["total_cnt"]);
      PerformanceMeter.Current.AddNote("Records = " + (object) table.Rows.Count);
      return LoanVersionDetailsDbAccessor.ConvertToList(table);
    }

    private static List<LoanVersionPipelineInfo> GetLoanVersionForGuids(
      DateTime? startDate,
      DateTime? endDate,
      string[] guidList,
      out int totalcount,
      int maxGuidsPerQuery)
    {
      guidList = ((IEnumerable<string>) guidList).Where<string>((System.Func<string, bool>) (x => !string.IsNullOrWhiteSpace(x))).Distinct<string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase).Take<string>(maxGuidsPerQuery).ToArray<string>();
      string str = LoanVersionDetailsDbAccessor.DateWhereClause(startDate, endDate);
      totalcount = 0;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("-- Create a temp table for the loan guid to return");
      dbQueryBuilder.AppendLine("declare @loan_guids table (guid varchar(38) primary key)");
      dbQueryBuilder.AppendLine("insert into @loan_guids select Guid from FN_ParseGuids(" + Pipeline.EncodeParseableGuidList(guidList) + ")");
      dbQueryBuilder.AppendLine("select lvd.GuId as LoanId, lvd.DateCreated, lvd.VersionNumber, loanver.total_cnt");
      dbQueryBuilder.AppendLine("from [LoanVersionDetails] lvd");
      dbQueryBuilder.AppendLine("inner join (");
      dbQueryBuilder.AppendLine("  select *, count(*) over () as total_cnt");
      dbQueryBuilder.AppendLine("  from (");
      dbQueryBuilder.AppendLine("    select lvd.Guid");
      dbQueryBuilder.AppendLine("    from [LoanVersionDetails] lvd");
      dbQueryBuilder.AppendLine("    inner join @loan_guids guids on guids.Guid = lvd.GuId");
      if (!string.IsNullOrWhiteSpace(str))
        dbQueryBuilder.AppendLine("    " + str);
      dbQueryBuilder.AppendLine("    group by lvd.guid");
      dbQueryBuilder.AppendLine("  ) loanVerRows");
      dbQueryBuilder.AppendLine(") loanver on loanver.guid = lvd.GuId");
      if (!string.IsNullOrWhiteSpace(str))
        dbQueryBuilder.AppendLine(str ?? "");
      dbQueryBuilder.AppendLine("order by lvd.VersionNumber desc");
      DataTable table = dbQueryBuilder.ExecuteTableQuery();
      if (table.Rows.Count > 0)
        totalcount = Convert.ToInt32(table.Rows[0]["total_cnt"]);
      PerformanceMeter.Current.AddNote("Records = " + (object) table.Rows.Count);
      return LoanVersionDetailsDbAccessor.ConvertToList(table);
    }

    private static string DateWhereClause(DateTime? startDate, DateTime? endDate)
    {
      List<string> stringList1 = new List<string>();
      DateTime dateTime;
      if (startDate.HasValue)
      {
        List<string> stringList2 = stringList1;
        dateTime = startDate.Value;
        string str = "lvd.DateCreated >= '" + dateTime.ToString("yyyy-MM-dd HH:mm:ss", (IFormatProvider) CultureInfo.InvariantCulture) + "'";
        stringList2.Add(str);
      }
      if (endDate.HasValue)
      {
        List<string> stringList3 = stringList1;
        dateTime = endDate.Value;
        string str = "lvd.DateCreated <= '" + dateTime.ToString("yyyy-MM-dd HH:mm:ss", (IFormatProvider) CultureInfo.InvariantCulture) + ".999'";
        stringList3.Add(str);
      }
      return !stringList1.Any<string>() ? string.Empty : "where " + string.Join(" and ", (IEnumerable<string>) stringList1);
    }

    private static List<LoanVersionPipelineInfo> ConvertToList(DataTable table)
    {
      List<LoanVersionPipelineInfo> list = new List<LoanVersionPipelineInfo>();
      foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
        list.Add(new LoanVersionPipelineInfo()
        {
          LoanId = Convert.ToString(row["LoanId"]),
          VersionNumber = Convert.ToInt32(row["VersionNumber"]),
          DateCreated = Convert.ToDateTime(row["DateCreated"])
        });
      return list;
    }
  }
}
