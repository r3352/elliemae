// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Dashboard
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils.Query;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class Dashboard
  {
    private static readonly UserSettingsAccessor.PrimaryKeyValues s_pkDefaultView = new UserSettingsAccessor.PrimaryKeyValues("Dashboard.DefaultViewId", nameof (Dashboard), (string) null);

    private Dashboard()
    {
    }

    public static DashboardViewInfo[] GetDashboardViews(DashboardViewCollectionCriteria criteria)
    {
      if (criteria == null || criteria.UserId == null)
        return new DashboardViewInfo[0];
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT v.* FROM DashboardView v WHERE v.UserId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) criteria.UserId) + " ORDER BY v.ViewName");
      dbQueryBuilder.AppendLine("SELECT r.* FROM DashboardReport r WHERE r.ViewId IN (SELECT v.ViewId FROM DashboardView v WHERE v.UserId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) criteria.UserId) + " ) ORDER BY r.ReportId");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      int count = dataSet.Tables[0].Rows.Count;
      DashboardViewInfo[] dashboardViews = new DashboardViewInfo[count];
      if (count == 0)
        return dashboardViews;
      dataSet.Relations.Add("View_Reports", dataSet.Tables[0].Columns["ViewId"], dataSet.Tables[1].Columns["ViewId"]);
      for (int index = 0; index < count; ++index)
      {
        DataRow row = dataSet.Tables[0].Rows[index];
        DataRow[] childRows = row.GetChildRows("View_Reports");
        dashboardViews[index] = EllieMae.EMLite.Server.Dashboard.createDashboardViewInfo(row, childRows);
      }
      return dashboardViews;
    }

    public static DashboardViewInfo[] GetReferencedDashboardViews(string snapshotPath)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT v.* FROM DashboardView v Where v.ViewId IN (Select r.ViewId from DashboardReport r where r.DashboardTemplatePath IN (" + EllieMae.EMLite.DataAccess.SQL.EncodeString(snapshotPath) + ")) ORDER BY v.ViewName");
      dbQueryBuilder.AppendLine("SELECT r.* FROM DashboardReport r WHERE r.DashboardTemplatePath IN (" + EllieMae.EMLite.DataAccess.SQL.EncodeString(snapshotPath) + ") ORDER BY r.ReportId");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      int count = dataSet.Tables[0].Rows.Count;
      DashboardViewInfo[] referencedDashboardViews = new DashboardViewInfo[count];
      if (count == 0)
        return referencedDashboardViews;
      dataSet.Relations.Add("View_Reports", dataSet.Tables[0].Columns["ViewId"], dataSet.Tables[1].Columns["ViewId"]);
      for (int index = 0; index < count; ++index)
      {
        DataRow row = dataSet.Tables[0].Rows[index];
        DataRow[] childRows = row.GetChildRows("View_Reports");
        referencedDashboardViews[index] = EllieMae.EMLite.Server.Dashboard.createDashboardViewInfo(row, childRows);
      }
      return referencedDashboardViews;
    }

    public static DashboardViewInfo GetDashboardView(int viewId)
    {
      if (1 > viewId)
        return (DashboardViewInfo) null;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT v.* FROM DashboardView v WHERE v.ViewId = " + (object) viewId);
      dbQueryBuilder.AppendLine("SELECT r.* FROM DashboardView v INNER JOIN DashboardReport r ON v.ViewId = r.ViewId WHERE v.ViewId = " + (object) viewId);
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet.Tables[0].Rows.Count == 0)
        return (DashboardViewInfo) null;
      dataSet.Relations.Add("View_Reports", dataSet.Tables[0].Columns["ViewId"], dataSet.Tables[1].Columns["ViewId"]);
      DataRow row = dataSet.Tables[0].Rows[0];
      DataRow[] childRows = row.GetChildRows("View_Reports");
      return EllieMae.EMLite.Server.Dashboard.createDashboardViewInfo(row, childRows);
    }

    public static DashboardViewInfo GetDashboardView(string viewGuid)
    {
      if (viewGuid == "")
        return (DashboardViewInfo) null;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT v.* FROM DashboardView v WHERE v.Guid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(viewGuid));
      dbQueryBuilder.AppendLine("SELECT r.* FROM DashboardView v INNER JOIN DashboardReport r ON v.ViewId = r.ViewId WHERE v.Guid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(viewGuid));
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet.Tables[0].Rows.Count == 0)
        return (DashboardViewInfo) null;
      dataSet.Relations.Add("View_Reports", dataSet.Tables[0].Columns["ViewId"], dataSet.Tables[1].Columns["ViewId"]);
      DataRow row = dataSet.Tables[0].Rows[0];
      DataRow[] childRows = row.GetChildRows("View_Reports");
      return EllieMae.EMLite.Server.Dashboard.createDashboardViewInfo(row, childRows);
    }

    public static bool CheckDashboardViewExists(string templatePath)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT 1 FROM DashboardView v WHERE v.TemplatePath = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(templatePath));
      return Convert.ToInt32(dbQueryBuilder.ExecuteScalar()) > 0;
    }

    public static DashboardViewInfo SaveDashboardView(DashboardViewInfo viewInfo)
    {
      if (viewInfo == null)
        return viewInfo;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Declare("@viewId", "int");
      if (viewInfo.ViewId == 0)
      {
        dbQueryBuilder.InsertInto(DbAccessManager.GetTable("DashboardView"), EllieMae.EMLite.Server.Dashboard.createDbValueList(viewInfo), true, false);
        dbQueryBuilder.SelectIdentity("@viewId");
      }
      else
      {
        DbValue key = new DbValue("ViewId", (object) viewInfo.ViewId);
        dbQueryBuilder.Update(DbAccessManager.GetTable("DashboardView"), EllieMae.EMLite.Server.Dashboard.createDbValueList(viewInfo), key);
        dbQueryBuilder.SelectVar("@viewId", (object) viewInfo.ViewId);
      }
      dbQueryBuilder.AppendLine("DELETE FROM DashboardReport WHERE ViewId = " + (object) viewInfo.ViewId);
      foreach (DashboardReportInfo dashboardReportInfo in viewInfo.DashboardReportInfos)
        dbQueryBuilder.InsertInto(DbAccessManager.GetTable("DashboardReport"), EllieMae.EMLite.Server.Dashboard.createDbValueList(dashboardReportInfo), true, false);
      dbQueryBuilder.Select("@viewId");
      return EllieMae.EMLite.Server.Dashboard.GetDashboardView((int) dbQueryBuilder.ExecuteScalar());
    }

    public static DashboardViewInfo SyncDashboardView(
      string sourceViewGuid,
      int targetViewID,
      string targetViewName,
      string targetViewGuid,
      string targetTemplatePath)
    {
      if (sourceViewGuid == "" || targetViewGuid == "")
        return (DashboardViewInfo) null;
      DashboardViewInfo dashboardView = EllieMae.EMLite.Server.Dashboard.GetDashboardView(sourceViewGuid);
      if (dashboardView == null)
        return (DashboardViewInfo) null;
      dashboardView.ViewId = targetViewID;
      dashboardView.UserId = "";
      dashboardView.ViewName = targetViewName;
      dashboardView.Guid = new Guid(targetViewGuid);
      dashboardView.TemplatePath = targetTemplatePath;
      dashboardView.IsFolder = false;
      return EllieMae.EMLite.Server.Dashboard.SaveDashboardView(dashboardView);
    }

    public static void DeleteDashboardView(int viewId)
    {
      if (1 > viewId)
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("DELETE FROM DashboardView WHERE ViewId = " + (object) viewId);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static bool IsTemplateReferenced(string templatePath)
    {
      return EllieMae.EMLite.Server.Dashboard.isTemplateReferenced(templatePath, false);
    }

    public static bool IsTemplateReferencedByGlobalView(string templatePath)
    {
      return EllieMae.EMLite.Server.Dashboard.isTemplateReferenced(templatePath, true);
    }

    public static void UpdateTemplatePath(FileSystemEntry source, FileSystemEntry target)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (FileSystemEntry.Types.File == source.Type)
      {
        dbQueryBuilder.AppendLine("UPDATE DashboardReport SET DashboardTemplatePath = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) target.ToString()) + " WHERE DashboardTemplatePath LIKE " + EllieMae.EMLite.DataAccess.SQL.Encode((object) EllieMae.EMLite.DataAccess.SQL.Escape(source.ToString())) + ";");
        dbQueryBuilder.AppendLine("UPDATE company_settings SET [value] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) EllieMae.EMLite.DataAccess.SQL.Escape(target.Path)) + " where [Category] = 'Dashboard' and [attribute] = 'DefaultDrilldownView' and  [value] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) EllieMae.EMLite.DataAccess.SQL.Escape(source.Path)));
      }
      else
      {
        dbQueryBuilder.Declare("@srcLen", "int");
        dbQueryBuilder.SelectVar("@srcLen", (object) source.ToString().Length);
        dbQueryBuilder.AppendLine("UPDATE DashboardReport SET DashboardTemplatePath = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) target.ToString()) + " + SUBSTRING(DashboardTemplatePath, @srcLen + 1, Len(DashboardTemplatePath) - @srcLen) WHERE DashboardTemplatePath LIKE " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (EllieMae.EMLite.DataAccess.SQL.Escape(source.ToString()) + "%")) + "; ");
        dbQueryBuilder.AppendLine("UPDATE company_settings SET [value] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) target.ToString()) + " + SUBSTRING([value], @srcLen + 1, Len([value]) - @srcLen)  where [Category] = 'Dashboard' and [attribute] = 'DefaultDrilldownView' and  [value] LIKE " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (EllieMae.EMLite.DataAccess.SQL.Escape(source.ToString()) + "%")));
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void UpdateViewTemplatePath(FileSystemEntry source, FileSystemEntry target)
    {
      UserSettingsAccessor settingsAccessor = new UserSettingsAccessor();
      string str1 = source.ToString();
      string str2 = target.ToString();
      if (FileSystemEntry.Types.File == source.Type)
        settingsAccessor.UpdateValues(str1, str2, EllieMae.EMLite.Server.Dashboard.s_pkDefaultView);
      else
        settingsAccessor.ReplacePrefixInValues(str1, str2, EllieMae.EMLite.Server.Dashboard.s_pkDefaultView);
    }

    public static bool IsReferencedAsDefaultViewTemplatePath(FileSystemEntry source)
    {
      bool flag = false;
      if (FileSystemEntry.Types.File == source.Type)
      {
        DataRowCollection rows = new UserSettingsAccessor().GetRows(source.ToString(), EllieMae.EMLite.Server.Dashboard.s_pkDefaultView);
        flag = rows != null && rows.Count > 0;
      }
      return flag;
    }

    public static void RemoveAllDefaultViewReference(FileSystemEntry source)
    {
      if (FileSystemEntry.Types.File != source.Type)
        return;
      new UserSettingsAccessor().Delete(source.ToString(), EllieMae.EMLite.Server.Dashboard.s_pkDefaultView);
    }

    public static DataTable QueryDataForDashboardReport(
      DashboardDataCriteria reportCriteria,
      bool isExternalOrganization,
      bool excludeArchiveLoans)
    {
      DataTable dataTable = (DataTable) null;
      if (reportCriteria == null)
        return dataTable;
      switch (reportCriteria.ChartType)
      {
        case DashboardChartType.BarChart:
          dataTable = EllieMae.EMLite.Server.Dashboard.getBarChartData(reportCriteria, isExternalOrganization, excludeArchiveLoans);
          break;
        case DashboardChartType.TrendChart:
          dataTable = EllieMae.EMLite.Server.Dashboard.getTrendChartData(reportCriteria, isExternalOrganization, excludeArchiveLoans);
          break;
        case DashboardChartType.LoanTable:
          dataTable = EllieMae.EMLite.Server.Dashboard.getLoanTableData(reportCriteria, isExternalOrganization, excludeArchiveLoans);
          break;
        case DashboardChartType.UserTable:
          dataTable = EllieMae.EMLite.Server.Dashboard.getUserTableData(reportCriteria, isExternalOrganization, excludeArchiveLoans);
          break;
      }
      return dataTable;
    }

    public static string[] GetDashboardSnapshotPathsByViewTemplatePaths(
      FileSystemEntry[] fileSystemEntries)
    {
      if (fileSystemEntries == null || ((IEnumerable<FileSystemEntry>) fileSystemEntries).Count<FileSystemEntry>() == 0)
        return (string[]) null;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select DISTINCT D.TemplatePath from [DashboardTemplate] D ");
      dbQueryBuilder.AppendLine("INNER JOIN [DashboardReport] R ");
      dbQueryBuilder.AppendLine("ON D.TemplatePath = R.DashboardTemplatePath ");
      dbQueryBuilder.AppendLine("INNER JOIN [DashboardView] V ");
      dbQueryBuilder.AppendLine("ON V.ViewId = R.ViewId AND V.IsFolder = 0 WHERE ");
      bool flag = false;
      foreach (FileSystemEntry fileSystemEntry in fileSystemEntries)
      {
        if (fileSystemEntry.Type == FileSystemEntry.Types.File)
        {
          if (flag)
            dbQueryBuilder.Append(" OR ");
          dbQueryBuilder.AppendLine(string.Format("V.TemplatePath = {0}", (object) EllieMae.EMLite.DataAccess.SQL.EncodeString(fileSystemEntry.ToString())));
          flag = true;
        }
        else if (fileSystemEntry.Type == FileSystemEntry.Types.Folder)
        {
          if (flag)
            dbQueryBuilder.Append(" OR ");
          dbQueryBuilder.AppendLine(string.Format("V.TemplatePath like '{0}%'", (object) EllieMae.EMLite.DataAccess.SQL.EncodeString(fileSystemEntry.ToString(), false)));
          flag = true;
        }
      }
      DataRowCollection source = dbQueryBuilder.Execute();
      return source != null && source.Count > 0 ? source.Cast<DataRow>().Select<DataRow, string>((System.Func<DataRow, string>) (row => EllieMae.EMLite.DataAccess.SQL.DecodeString(row["TemplatePath"]))).ToArray<string>() : (string[]) null;
    }

    private static bool isTemplateReferenced(string templatePath, bool byGlobalView)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("SELECT COUNT(*) FROM DashboardView v INNER JOIN DashboardReport r ON v.ViewId = r.ViewId WHERE r.DashboardTemplatePath LIKE " + EllieMae.EMLite.DataAccess.SQL.Encode((object) templatePath));
      if (byGlobalView)
        dbQueryBuilder.Append(" AND v.UserId = ''");
      return (int) dbQueryBuilder.ExecuteScalar() != 0;
    }

    private static DataTable generateBarChartTestData()
    {
      DataTable barChartTestData = new DataTable("Bar Chart");
      barChartTestData.Columns.Add("Group By Name", typeof (string));
      barChartTestData.Columns.Add("Aggregate Value", typeof (Decimal));
      Random random = new Random();
      for (int index = 1; index <= 50; ++index)
        barChartTestData.Rows.Add((object) ("Group By Name " + index.ToString()), (object) random.Next(0, 100000));
      return barChartTestData;
    }

    private static DataTable generateTrendChartTestData()
    {
      DataTable trendChartTestData = new DataTable("Trend Chart");
      trendChartTestData.Columns.Add("Group By Name", typeof (string));
      for (int index = 1; index <= 12; ++index)
        trendChartTestData.Columns.Add("Month " + index.ToString(), typeof (Decimal));
      Random random = new Random();
      for (int index = 1; index <= 10; ++index)
        trendChartTestData.Rows.Add((object) ("Group By Name " + index.ToString()), (object) random.Next(0, 10000), (object) random.Next(0, 10000), (object) random.Next(0, 10000), (object) random.Next(0, 10000), (object) random.Next(0, 10000), (object) random.Next(0, 10000), (object) random.Next(0, 10000), (object) random.Next(0, 10000), (object) random.Next(0, 10000), (object) random.Next(0, 10000), (object) random.Next(0, 10000), (object) random.Next(0, 10000));
      return trendChartTestData;
    }

    private static DataTable generateUserTableTestData()
    {
      DataTable userTableTestData = new DataTable("User Table");
      userTableTestData.Columns.Add("User Id", typeof (string));
      userTableTestData.Columns.Add("Group By Name", typeof (string));
      for (int index = 1; index <= 3; ++index)
        userTableTestData.Columns.Add("Summary Column " + index.ToString(), typeof (Decimal));
      Random random = new Random();
      for (int index1 = 1; index1 <= 10; ++index1)
      {
        string str1 = "User " + index1.ToString();
        for (int index2 = 1; index2 <= 7; ++index2)
        {
          string str2 = "Group " + index2.ToString();
          userTableTestData.Rows.Add((object) str1, (object) str2, (object) (Decimal) random.Next(0, 500), (object) (Decimal) random.Next(0, 1000000), (object) (Decimal) random.Next(0, 5000));
        }
      }
      return userTableTestData;
    }

    private static DataTable getBarChartData(
      DashboardDataCriteria reportCriteria,
      bool isExternalOrganization,
      bool excludeArchiveLoans)
    {
      DataTable barChartData = (DataTable) null;
      if (!(reportCriteria is BarChartDataCriteria barChartCriteria))
        return barChartData;
      ReportFieldCache reportFieldCache = new ReportFieldCache();
      List<EllieMae.EMLite.Server.Dashboard.FieldInfo> fieldInfos = new List<EllieMae.EMLite.Server.Dashboard.FieldInfo>();
      string fieldId1 = reportFieldCache.GetFieldId(barChartCriteria.XAxisField);
      fieldInfos.Add(new EllieMae.EMLite.Server.Dashboard.FieldInfo(barChartCriteria.XAxisField, fieldId1, false));
      if ("Dashboard.LoanCount" != barChartCriteria.YAxisField)
      {
        string fieldId2 = reportFieldCache.GetFieldId(barChartCriteria.YAxisField);
        fieldInfos.Add(new EllieMae.EMLite.Server.Dashboard.FieldInfo(barChartCriteria.YAxisField, fieldId2, false));
      }
      if (EllieMae.EMLite.Server.Dashboard.applyBusinessRules(reportCriteria.CurrentUser, fieldInfos))
        throw new ApplicationException("Violation of Business Rule access rights");
      string cmdText = EllieMae.EMLite.Server.Dashboard.buildBarChartQuery(barChartCriteria, fieldInfos, isExternalOrganization, excludeArchiveLoans);
      using (DbAccessManager dbAccessManager = new DbAccessManager())
        return dbAccessManager.ExecuteTableQuery((IDbCommand) new SqlCommand(cmdText), EllieMae.EMLite.DataAccess.ServerGlobals.ReportSQLTimeout, DbTransactionType.Default);
    }

    private static bool applyBusinessRules(
      UserInfo currentUser,
      List<EllieMae.EMLite.Server.Dashboard.FieldInfo> fieldInfos)
    {
      bool flag = false;
      if (currentUser.IsSuperAdministrator())
        return flag;
      Hashtable hashtable = (Hashtable) new FieldAccessRuleEvaluator((FieldAccessRuleInfo[]) BpmDbAccessor.GetAccessor(BizRuleType.FieldAccess).GetRules(BizRule.Condition.Null, true)).Evaluate(currentUser.UserPersonas);
      if (hashtable == null || hashtable.Count == 0)
        return flag;
      foreach (EllieMae.EMLite.Server.Dashboard.FieldInfo fieldInfo in fieldInfos)
      {
        if (hashtable.ContainsKey((object) fieldInfo.FieldId) && (BizRule.FieldAccessRight) hashtable[(object) fieldInfo.FieldId] == BizRule.FieldAccessRight.Hide)
        {
          fieldInfo.IsHidden = true;
          flag = true;
        }
      }
      return flag;
    }

    private static string buildBarChartQuery(
      BarChartDataCriteria barChartCriteria,
      List<EllieMae.EMLite.Server.Dashboard.FieldInfo> fieldInfos,
      bool isExternalOrganization,
      bool excludeArchiveLoans)
    {
      EllieMae.EMLite.Server.Query.LoanQuery queryEngine = new EllieMae.EMLite.Server.Query.LoanQuery(barChartCriteria.CurrentUser);
      StringBuilder sqlWhereClause = new StringBuilder();
      sqlWhereClause.AppendLine(EllieMae.EMLite.Server.Dashboard.buildIdentityQuery((DashboardDataCriteria) barChartCriteria, queryEngine, isExternalOrganization, excludeArchiveLoans));
      string str1 = EllieMae.EMLite.Server.Dashboard.buildBarChartFieldList(barChartCriteria, queryEngine.FieldTranslator);
      StringBuilder stringBuilder = new StringBuilder();
      List<string> fieldCriterionNames = EllieMae.EMLite.Server.Dashboard.getFieldCriterionNames(fieldInfos);
      stringBuilder.AppendLine(queryEngine.GetFieldJoinClause((IQueryTerm[]) DataField.CreateFields(fieldCriterionNames.ToArray()), (QueryCriterion) null, (SortField[]) null));
      if (DashboardViewFilterType.Organization == barChartCriteria.ViewFilterType)
        EllieMae.EMLite.Server.Dashboard.buildViewLevelOrganizationFilter(sqlWhereClause, barChartCriteria.ViewFilterOrganizationId, barChartCriteria.ViewFilterIncludeChildren);
      else if (DashboardViewFilterType.UserGroup == barChartCriteria.ViewFilterType)
        EllieMae.EMLite.Server.Dashboard.buildViewLevelGroupFilter(sqlWhereClause, barChartCriteria.ViewFilterUserGroupId);
      string str2 = " " + EllieMae.EMLite.DataAccess.SQL.ToOrderByClause(new SortField[1]
      {
        new SortField("AggregateValue", barChartCriteria.SubsetType == DashboardSubsetType.Top ? FieldSortOrder.Descending : FieldSortOrder.Ascending)
      });
      string str3 = "TOP " + barChartCriteria.MaxBars.ToString() + " ";
      string str4 = "GROUP BY " + (object) queryEngine.FieldTranslator.TranslateName(barChartCriteria.XAxisField) + " ";
      string str5 = string.Empty;
      if (queryEngine.CurrentUser != (UserInfo) null)
        str5 = "-- Parameterizing userId \r\ndeclare @userId varchar(16)\r\nset @userId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) queryEngine.CurrentUser.Userid) + Environment.NewLine;
      return str5 + "SELECT " + str3 + str1 + " FROM LoanSummary Loan " + stringBuilder.ToString() + (object) sqlWhereClause + str4 + str2;
    }

    private static string buildIdentityQuery(
      DashboardDataCriteria reportCriteria,
      EllieMae.EMLite.Server.Query.LoanQuery queryEngine,
      bool isExternalOrganization,
      bool excludeArchiveLoans)
    {
      string str = queryEngine.CreateIdentitySelectionQuery(reportCriteria.GetReportCritera(), true, isExternalOrganization, excludeArchivedLoans: excludeArchiveLoans) ?? string.Empty;
      if (str.Length != 0)
        str = " WHERE Loan.Guid IN (" + str + ")";
      return str;
    }

    private static string buildBarChartFieldList(
      BarChartDataCriteria barChartCriteria,
      ICriterionTranslator fieldTranslator)
    {
      string str1 = ColumnSummaryType.Average == barChartCriteria.YAxisSummaryType ? "AVG" : (ColumnSummaryType.Total == barChartCriteria.YAxisSummaryType ? "SUM" : "COUNT");
      string sqlColumnName = fieldTranslator.TranslateName(barChartCriteria.XAxisField).ToSqlColumnName();
      string str2 = "COUNT" == str1 ? "*" : "CONVERT(MONEY, " + (object) fieldTranslator.TranslateName(barChartCriteria.YAxisField) + ")";
      return sqlColumnName + ", " + str1 + "(" + str2 + ") As [AggregateValue]";
    }

    private static List<string> getFieldCriterionNames(List<EllieMae.EMLite.Server.Dashboard.FieldInfo> fieldInfos)
    {
      List<string> fieldCriterionNames = new List<string>();
      foreach (EllieMae.EMLite.Server.Dashboard.FieldInfo fieldInfo in fieldInfos)
        fieldCriterionNames.Add(fieldInfo.CriterionName);
      return fieldCriterionNames;
    }

    private static void buildViewLevelOrganizationFilter(
      StringBuilder sqlWhereClause,
      int organizationId,
      bool includeChildren)
    {
      sqlWhereClause.Append(sqlWhereClause.Length == 0 ? " WHERE" : " AND Loan.Guid IN (");
      sqlWhereClause.Append("SELECT DISTINCT Loan.Guid FROM LoanSummary Loan ");
      sqlWhereClause.Append("INNER JOIN LoanAssociateUsers associate ON loan.Guid = associate.Guid ");
      sqlWhereClause.Append("INNER JOIN users ON associate.UserID = users.userid ");
      if (includeChildren)
        sqlWhereClause.Append("LEFT OUTER JOIN org_descendents ON users.org_id = org_descendents.descendent ");
      sqlWhereClause.Append("WHERE users.org_id = " + organizationId.ToString());
      if (includeChildren)
        sqlWhereClause.Append(" OR org_descendents.oid = " + organizationId.ToString());
      sqlWhereClause.AppendLine(")");
    }

    private static void buildViewLevelGroupFilter(StringBuilder sqlWhereClause, int userGroupId)
    {
      sqlWhereClause.Append(sqlWhereClause.Length == 0 ? " WHERE" : " AND Loan.Guid IN (");
      sqlWhereClause.Append("SELECT DISTINCT Loan.Guid FROM LoanSummary Loan ");
      sqlWhereClause.Append("INNER JOIN LoanAssociateUsers associate ON loan.Guid = associate.Guid ");
      sqlWhereClause.Append("INNER JOIN AclGroupMembers gu ON gu.GroupID = " + (object) userGroupId + " AND associate.UserID = gu.userId)");
    }

    private static DataTable getTrendChartData(
      DashboardDataCriteria reportCriteria,
      bool isExternalOrganization,
      bool excludeArchiveLoans)
    {
      DataTable trendChartData1 = (DataTable) null;
      if (!(reportCriteria is TrendChartDataCriteria trendChartCriteria))
        return trendChartData1;
      ReportFieldCache reportFieldCache = new ReportFieldCache();
      List<EllieMae.EMLite.Server.Dashboard.FieldInfo> fieldInfos = new List<EllieMae.EMLite.Server.Dashboard.FieldInfo>();
      string fieldId1 = reportFieldCache.GetFieldId(trendChartCriteria.XAxisField);
      fieldInfos.Add(new EllieMae.EMLite.Server.Dashboard.FieldInfo(trendChartCriteria.XAxisField, fieldId1, false));
      if ("Dashboard.LoanCount" != trendChartCriteria.YAxisField)
      {
        string fieldId2 = reportFieldCache.GetFieldId(trendChartCriteria.YAxisField);
        fieldInfos.Add(new EllieMae.EMLite.Server.Dashboard.FieldInfo(trendChartCriteria.YAxisField, fieldId2, false));
      }
      if ("Dashboard.NoGroupBy" != trendChartCriteria.GroupByField)
      {
        string fieldId3 = reportFieldCache.GetFieldId(trendChartCriteria.GroupByField);
        fieldInfos.Add(new EllieMae.EMLite.Server.Dashboard.FieldInfo(trendChartCriteria.GroupByField, fieldId3, false));
      }
      if (EllieMae.EMLite.Server.Dashboard.applyBusinessRules(reportCriteria.CurrentUser, fieldInfos))
        throw new ApplicationException("Violation of Business Rule access rights");
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(EllieMae.EMLite.Server.Dashboard.buildTimeSlotTable(trendChartCriteria));
      stringBuilder.Append(EllieMae.EMLite.Server.Dashboard.buildTrendChartQuery(trendChartCriteria, fieldInfos, isExternalOrganization, excludeArchiveLoans));
      stringBuilder.AppendLine("DROP TABLE #TimeSlot");
      DataTable dataTable = (DataTable) null;
      using (DbAccessManager dbAccessManager = new DbAccessManager())
        dataTable = dbAccessManager.ExecuteTableQuery((IDbCommand) new SqlCommand(stringBuilder.ToString()), EllieMae.EMLite.DataAccess.ServerGlobals.ReportSQLTimeout, DbTransactionType.Default);
      if (dataTable.Rows.Count == 0)
        return trendChartData1;
      DataTable trendChartData2 = new DataTable("Trend Chart");
      trendChartData2.Columns.Add("GroupByName", typeof (string));
      int timePeriodCount = trendChartCriteria.TimePeriodCount;
      for (int dataPoint = 1; dataPoint <= timePeriodCount; ++dataPoint)
        trendChartData2.Columns.Add(EllieMae.EMLite.Server.Dashboard.getTrendChartColumnName(dataPoint, trendChartCriteria), typeof (Decimal));
      for (int index = 0; index < trendChartCriteria.MaxLines && (int) dataTable.Rows[index]["SlotId"] == 0; ++index)
      {
        if (dataTable.Rows[index]["GroupByName"] is string str1)
        {
          DataRow row1 = trendChartData2.NewRow();
          row1["GroupByName"] = (object) str1;
          for (int columnIndex = 1; columnIndex <= timePeriodCount; ++columnIndex)
            row1[columnIndex] = (object) 0;
          foreach (DataRow row2 in (InternalDataCollectionBase) dataTable.Rows)
          {
            if ((int) row2["SlotId"] != 0 && row2["GroupByName"] is string str)
            {
              if (!(str1 != str))
              {
                try
                {
                  Decimal num = Convert.ToDecimal(row2["AggregateValue"]);
                  row1[(int) row2["SlotId"]] = (object) num;
                }
                catch
                {
                }
              }
            }
          }
          trendChartData2.Rows.Add(row1);
        }
      }
      return trendChartData2;
    }

    private static string getTrendChartColumnName(
      int dataPoint,
      TrendChartDataCriteria trendChartCriteria)
    {
      DateTime endDate = trendChartCriteria.TimePeriods[dataPoint].EndDate;
      string empty = string.Empty;
      string trendChartColumnName;
      switch (trendChartCriteria.TimeUnitType)
      {
        case DashboardTimeUnitType.Day:
        case DashboardTimeUnitType.Week:
          trendChartColumnName = endDate.ToString("MM/dd");
          break;
        case DashboardTimeUnitType.Month:
          trendChartColumnName = endDate.ToString("MMM yy");
          break;
        case DashboardTimeUnitType.Quarter:
          trendChartColumnName = "Q" + ((endDate.Month - 1) / 3 + 1).ToString() + " " + endDate.ToString("yy");
          break;
        default:
          trendChartColumnName = endDate.ToString("MM/dd/yy");
          break;
      }
      return trendChartColumnName;
    }

    private static string buildTimeSlotTable(TrendChartDataCriteria trendChartCriteria)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("CREATE TABLE #TimeSlot(SlotId INT PRIMARY KEY, MinDate DATETIME, MaxDate DATETIME)");
      for (int index = 0; index <= trendChartCriteria.TimePeriodCount; ++index)
      {
        stringBuilder.Append("INSERT INTO #TimeSlot VALUES (");
        stringBuilder.Append(index.ToString() + ", ");
        stringBuilder.Append(EllieMae.EMLite.DataAccess.SQL.Encode((object) trendChartCriteria.TimePeriods[index].StartDate.ToString("MM/dd/yyyy hh:mm:ss tt")) + ", ");
        stringBuilder.AppendLine(EllieMae.EMLite.DataAccess.SQL.Encode((object) trendChartCriteria.TimePeriods[index].EndDate.ToString("MM/dd/yyyy hh:mm:ss tt")) + ")");
      }
      return stringBuilder.ToString();
    }

    private static string buildTrendChartQuery(
      TrendChartDataCriteria trendChartCriteria,
      List<EllieMae.EMLite.Server.Dashboard.FieldInfo> fieldInfos,
      bool isExternalOrganization,
      bool excludeArchiveLoans)
    {
      EllieMae.EMLite.Server.Query.LoanQuery queryEngine = new EllieMae.EMLite.Server.Query.LoanQuery(trendChartCriteria.CurrentUser);
      StringBuilder sqlWhereClause = new StringBuilder();
      sqlWhereClause.Append(EllieMae.EMLite.Server.Dashboard.buildIdentityQuery((DashboardDataCriteria) trendChartCriteria, queryEngine, isExternalOrganization, excludeArchiveLoans));
      string trendChartFieldList = EllieMae.EMLite.Server.Dashboard.getTrendChartFieldList(trendChartCriteria, queryEngine.FieldTranslator);
      StringBuilder stringBuilder = new StringBuilder();
      List<string> fieldCriterionNames = EllieMae.EMLite.Server.Dashboard.getFieldCriterionNames(fieldInfos);
      stringBuilder.Append(queryEngine.GetFieldJoinClause((IQueryTerm[]) DataField.CreateFields(fieldCriterionNames.ToArray()), (QueryCriterion) null, (SortField[]) null));
      stringBuilder.AppendLine(EllieMae.EMLite.Server.Dashboard.buildJoinWithTimeSlotTable(trendChartCriteria.XAxisField, queryEngine.FieldTranslator));
      if (DashboardViewFilterType.Organization == trendChartCriteria.ViewFilterType)
        EllieMae.EMLite.Server.Dashboard.buildViewLevelOrganizationFilter(sqlWhereClause, trendChartCriteria.ViewFilterOrganizationId, trendChartCriteria.ViewFilterIncludeChildren);
      else if (DashboardViewFilterType.UserGroup == trendChartCriteria.ViewFilterType)
        EllieMae.EMLite.Server.Dashboard.buildViewLevelGroupFilter(sqlWhereClause, trendChartCriteria.ViewFilterUserGroupId);
      string str1 = "GROUP BY SlotId";
      if ("Dashboard.NoGroupBy" != trendChartCriteria.GroupByField)
        str1 = str1 + ", " + (object) queryEngine.FieldTranslator.TranslateName(trendChartCriteria.GroupByField) + " ";
      string str2 = " " + EllieMae.EMLite.DataAccess.SQL.ToOrderByClause(new SortField[2]
      {
        new SortField("SlotId", FieldSortOrder.Ascending),
        new SortField("AggregateValue", trendChartCriteria.SubsetType == DashboardSubsetType.Top ? FieldSortOrder.Descending : FieldSortOrder.Ascending)
      }) + " ";
      string str3 = string.Empty;
      if (queryEngine.CurrentUser != (UserInfo) null)
        str3 = "-- Parameterizing userId \r\ndeclare @userId varchar(16)\r\nset @userId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) queryEngine.CurrentUser.Userid) + Environment.NewLine;
      return str3 + "SELECT " + trendChartFieldList + " FROM LoanSummary Loan " + stringBuilder.ToString() + (object) sqlWhereClause + str1 + str2;
    }

    private static string getTrendChartFieldList(
      TrendChartDataCriteria trendChartCriteria,
      ICriterionTranslator fieldTranslator)
    {
      string str1 = ColumnSummaryType.Average == trendChartCriteria.YAxisSummaryType ? "AVG" : (ColumnSummaryType.Total == trendChartCriteria.YAxisSummaryType ? "SUM" : "COUNT");
      string str2 = "COUNT" == str1 ? "*" : "CONVERT(MONEY, " + (object) fieldTranslator.TranslateName(trendChartCriteria.YAxisField) + ")";
      string empty = string.Empty;
      return "SlotId, " + (!("Dashboard.NoGroupBy" == trendChartCriteria.GroupByField) ? fieldTranslator.TranslateName(trendChartCriteria.GroupByField).ToString() + " AS [GroupByName], " : "'" + (object) fieldTranslator.TranslateName(trendChartCriteria.YAxisField) + "' AS [GroupByName], ") + str1 + "(" + str2 + ") AS [AggregateValue]";
    }

    private static string buildJoinWithTimeSlotTable(
      string xAxisFieldCriterionName,
      ICriterionTranslator fieldTranslator)
    {
      string sqlColumnName = fieldTranslator.TranslateName(xAxisFieldCriterionName).ToSqlColumnName();
      return " INNER JOIN #TimeSlot ON " + sqlColumnName + " >= #TimeSlot.MinDate AND " + sqlColumnName + " <= #TimeSlot.MaxDate ";
    }

    private static DataTable getLoanTableData(
      DashboardDataCriteria reportCriteria,
      bool isExternalOrganization,
      bool excludeArchiveLoans)
    {
      DataTable loanTableData = (DataTable) null;
      if (!(reportCriteria is LoanTableDataCriteria loanTableCriteria) || loanTableCriteria.FieldCriterionNames.Count == 0)
        return loanTableData;
      ReportFieldCache reportFieldCache = new ReportFieldCache();
      List<EllieMae.EMLite.Server.Dashboard.FieldInfo> fieldInfos = new List<EllieMae.EMLite.Server.Dashboard.FieldInfo>(loanTableCriteria.FieldCriterionNames.Count);
      foreach (string fieldCriterionName in loanTableCriteria.FieldCriterionNames)
      {
        string fieldId = reportFieldCache.GetFieldId(fieldCriterionName);
        fieldInfos.Add(new EllieMae.EMLite.Server.Dashboard.FieldInfo(fieldCriterionName, fieldId, false));
      }
      bool flag = EllieMae.EMLite.Server.Dashboard.applyBusinessRules(reportCriteria.CurrentUser, fieldInfos);
      string cmdText = EllieMae.EMLite.Server.Dashboard.buildLoanTableQuery(loanTableCriteria, fieldInfos, isExternalOrganization, excludeArchiveLoans);
      using (DbAccessManager dbAccessManager = new DbAccessManager())
        loanTableData = dbAccessManager.ExecuteTableQuery((IDbCommand) new SqlCommand(cmdText), EllieMae.EMLite.DataAccess.ServerGlobals.ReportSQLTimeout, DbTransactionType.Default);
      if (flag)
      {
        for (int index = 0; index < fieldInfos.Count; ++index)
        {
          if (fieldInfos[index].IsHidden)
            loanTableData.Columns[index + 1].ColumnName = "_Hide_" + loanTableData.Columns[index + 1].ColumnName;
        }
      }
      return loanTableData;
    }

    private static string buildLoanTableQuery(
      LoanTableDataCriteria loanTableCriteria,
      List<EllieMae.EMLite.Server.Dashboard.FieldInfo> fieldInfos,
      bool isExternalOrganization,
      bool excludeArchiveLoans)
    {
      EllieMae.EMLite.Server.Query.LoanQuery queryEngine = new EllieMae.EMLite.Server.Query.LoanQuery(loanTableCriteria.CurrentUser);
      StringBuilder sqlWhereClause = new StringBuilder();
      string str1 = string.Empty;
      sqlWhereClause.Append(EllieMae.EMLite.Server.Dashboard.buildIdentityQuery((DashboardDataCriteria) loanTableCriteria, queryEngine, isExternalOrganization, excludeArchiveLoans));
      string encodedColumnNames = EllieMae.EMLite.Server.Dashboard.getEncodedColumnNames(fieldInfos, queryEngine.FieldTranslator);
      StringBuilder stringBuilder = new StringBuilder();
      string fieldJoinClause = queryEngine.GetFieldJoinClause((IQueryTerm[]) DataField.CreateFields(loanTableCriteria.FieldCriterionNames.ToArray()), (QueryCriterion) null, loanTableCriteria.SortFields);
      if (fieldInfos.Exists((Predicate<EllieMae.EMLite.Server.Dashboard.FieldInfo>) (x => x.FieldId == "5016")) && !fieldJoinClause.Contains("inner join AllLoanFolders LoanFolder"))
        str1 = " inner join AllLoanFolders LoanFolder on (Loan.LoanFolder = LoanFolder.FolderName) ";
      stringBuilder.AppendLine(fieldJoinClause);
      if (DashboardViewFilterType.Organization == loanTableCriteria.ViewFilterType)
        EllieMae.EMLite.Server.Dashboard.buildViewLevelOrganizationFilter(sqlWhereClause, loanTableCriteria.ViewFilterOrganizationId, loanTableCriteria.ViewFilterIncludeChildren);
      else if (DashboardViewFilterType.UserGroup == loanTableCriteria.ViewFilterType)
        EllieMae.EMLite.Server.Dashboard.buildViewLevelGroupFilter(sqlWhereClause, loanTableCriteria.ViewFilterUserGroupId);
      string str2 = string.Empty;
      if (loanTableCriteria.SortFields != null && loanTableCriteria.SortFields.Length != 0)
        str2 = " " + EllieMae.EMLite.DataAccess.SQL.ToOrderByClause(loanTableCriteria.SortFields, queryEngine.FieldTranslator);
      string str3 = string.Empty;
      if (0 < loanTableCriteria.MaxRows)
        str3 = "TOP " + loanTableCriteria.MaxRows.ToString() + " ";
      string str4 = string.Empty;
      if (queryEngine.CurrentUser != (UserInfo) null)
        str4 = "-- Parameterizing userId \r\ndeclare @userId varchar(16)\r\nset @userId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) queryEngine.CurrentUser.Userid) + Environment.NewLine;
      return str4 + "SELECT " + str3 + "Loan.Guid, " + encodedColumnNames + " FROM LoanSummary Loan " + str1 + stringBuilder.ToString() + (object) sqlWhereClause + str2;
    }

    public static string getEncodedColumnNames(
      List<EllieMae.EMLite.Server.Dashboard.FieldInfo> fieldInfos,
      ICriterionTranslator fieldTranslator)
    {
      if (fieldInfos == null || fieldInfos.Count == 0)
        return (string) null;
      List<string> stringList = new List<string>();
      foreach (EllieMae.EMLite.Server.Dashboard.FieldInfo fieldInfo in fieldInfos)
      {
        string str1 = !fieldInfo.IsHidden ? fieldTranslator.TranslateName(fieldInfo.CriterionName).ToSqlColumnName() : "NULL";
        if (str1 == "[Loan].[IsArchived]")
        {
          string str2 = "(case when " + str1 + " = 1 then 'Y' else (case when LoanFolder.Archive = 'Y' then 'Y' else 'N' end) end)";
          stringList.Add(str2 + " As " + QueryEngine.CriterionNameToColumnName(fieldInfo.CriterionName));
        }
        else
          stringList.Add(str1 + " As " + QueryEngine.CriterionNameToColumnName(fieldInfo.CriterionName));
      }
      return string.Join(", ", stringList.ToArray());
    }

    private static DataTable getUserTableData(
      DashboardDataCriteria reportCriteria,
      bool isExternalOrganization,
      bool excludeArchiveLoans)
    {
      DataTable userTableData = (DataTable) null;
      if (!(reportCriteria is UserTableDataCriteria userTableCriteria))
        return userTableData;
      ReportFieldCache fieldLookup = new ReportFieldCache();
      List<EllieMae.EMLite.Server.Dashboard.FieldInfo> fieldInfos = new List<EllieMae.EMLite.Server.Dashboard.FieldInfo>();
      fieldInfos.Add(new EllieMae.EMLite.Server.Dashboard.FieldInfo("users.userid", "userid", false));
      if ("Dashboard.NoGroupBy" != userTableCriteria.GroupByField)
      {
        string fieldId = fieldLookup.GetFieldId(userTableCriteria.GroupByField);
        fieldInfos.Add(new EllieMae.EMLite.Server.Dashboard.FieldInfo(userTableCriteria.GroupByField, fieldId, false));
      }
      fieldInfos.AddRange((IEnumerable<EllieMae.EMLite.Server.Dashboard.FieldInfo>) EllieMae.EMLite.Server.Dashboard.getUserTableSummaryFieldInfos(userTableCriteria, fieldLookup));
      if (EllieMae.EMLite.Server.Dashboard.applyBusinessRules(reportCriteria.CurrentUser, fieldInfos))
        throw new ApplicationException("Violation of Business Rule access rights");
      string cmdText = EllieMae.EMLite.Server.Dashboard.buildUserTableQuery(userTableCriteria, fieldInfos, fieldLookup, isExternalOrganization, excludeArchiveLoans);
      using (DbAccessManager dbAccessManager = new DbAccessManager())
        return dbAccessManager.ExecuteTableQuery((IDbCommand) new SqlCommand(cmdText), EllieMae.EMLite.DataAccess.ServerGlobals.ReportSQLTimeout, DbTransactionType.Default);
    }

    private static List<EllieMae.EMLite.Server.Dashboard.FieldInfo> getUserTableSummaryFieldInfos(
      UserTableDataCriteria userTableCriteria,
      ReportFieldCache fieldLookup)
    {
      List<EllieMae.EMLite.Server.Dashboard.FieldInfo> summaryFieldInfos = new List<EllieMae.EMLite.Server.Dashboard.FieldInfo>();
      if ("Dashboard.NoSummary" != userTableCriteria.SummaryField1 && "Dashboard.LoanCount" != userTableCriteria.SummaryField1)
      {
        string fieldId = fieldLookup.GetFieldId(userTableCriteria.SummaryField1);
        summaryFieldInfos.Add(new EllieMae.EMLite.Server.Dashboard.FieldInfo(userTableCriteria.SummaryField1, fieldId, false));
      }
      if ("Dashboard.NoSummary" != userTableCriteria.SummaryField2 && "Dashboard.LoanCount" != userTableCriteria.SummaryField2)
      {
        string fieldId = fieldLookup.GetFieldId(userTableCriteria.SummaryField2);
        summaryFieldInfos.Add(new EllieMae.EMLite.Server.Dashboard.FieldInfo(userTableCriteria.SummaryField2, fieldId, false));
      }
      if ("Dashboard.NoSummary" != userTableCriteria.SummaryField3 && "Dashboard.LoanCount" != userTableCriteria.SummaryField3)
      {
        string fieldId = fieldLookup.GetFieldId(userTableCriteria.SummaryField3);
        summaryFieldInfos.Add(new EllieMae.EMLite.Server.Dashboard.FieldInfo(userTableCriteria.SummaryField3, fieldId, false));
      }
      return summaryFieldInfos;
    }

    private static string buildUserTableQuery(
      UserTableDataCriteria userTableCriteria,
      List<EllieMae.EMLite.Server.Dashboard.FieldInfo> fieldInfos,
      ReportFieldCache fieldLookup,
      bool isExternalOrganization,
      bool excludeArchiveLoans)
    {
      EllieMae.EMLite.Server.Query.LoanQuery queryEngine = new EllieMae.EMLite.Server.Query.LoanQuery(userTableCriteria.CurrentUser);
      StringBuilder sqlWhereClause = new StringBuilder();
      sqlWhereClause.Append(EllieMae.EMLite.Server.Dashboard.buildIdentityQuery((DashboardDataCriteria) userTableCriteria, queryEngine, isExternalOrganization, excludeArchiveLoans));
      StringBuilder sqlTableJoins = new StringBuilder();
      sqlTableJoins.Append(queryEngine.GetFieldJoinClause((IQueryTerm[]) DataField.CreateFields(EllieMae.EMLite.Server.Dashboard.getCriterionNames(fieldInfos).ToArray()), (QueryCriterion) null, (SortField[]) null));
      sqlTableJoins.Append("INNER JOIN LoanAssociateUsers associate ON loan.Guid = associate.Guid ");
      sqlTableJoins.AppendLine("INNER JOIN users ON associate.UserID = users.userid");
      if (DashboardViewFilterType.Organization == userTableCriteria.ViewFilterType)
        EllieMae.EMLite.Server.Dashboard.buildViewLevelOrganizationFilter(sqlWhereClause, userTableCriteria.ViewFilterOrganizationId, userTableCriteria.ViewFilterIncludeChildren);
      else if (DashboardViewFilterType.UserGroup == userTableCriteria.ViewFilterType)
        EllieMae.EMLite.Server.Dashboard.buildViewLevelGroupFilter(sqlWhereClause, userTableCriteria.ViewFilterUserGroupId);
      if (-2 != userTableCriteria.RoleId && 0 < userTableCriteria.RoleId)
        EllieMae.EMLite.Server.Dashboard.buildTemplateLevelRoleFilter(userTableCriteria, sqlWhereClause, sqlTableJoins);
      if (-1 != userTableCriteria.OrganizationId)
      {
        if (0 <= userTableCriteria.OrganizationId)
          EllieMae.EMLite.Server.Dashboard.buildTemplateLevelOrganizationFilter(userTableCriteria, sqlWhereClause, sqlTableJoins);
      }
      else if (-1 != userTableCriteria.UserGroupId && 0 < userTableCriteria.UserGroupId)
        EllieMae.EMLite.Server.Dashboard.buildTemplateLevelGroupFilter(userTableCriteria, sqlTableJoins);
      string str1 = sqlTableJoins.ToString() + sqlWhereClause.ToString();
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendLine("CREATE TABLE #UserGuidPair (UserId VARCHAR(16) NOT NULL, Guid VARCHAR(38) NOT NULL)");
      stringBuilder1.AppendLine("INSERT INTO #UserGuidPair (UserId, Guid) (SELECT DISTINCT users.userid, Loan.Guid");
      stringBuilder1.AppendLine("FROM LoanSummary Loan " + str1 + ")");
      stringBuilder1.AppendLine("CREATE INDEX UserId_Guid ON #UserGuidPair(UserId, Guid)");
      StringBuilder stringBuilder2 = new StringBuilder();
      string str2 = string.Empty;
      if ("Dashboard.NoGroupBy" != userTableCriteria.GroupByField)
      {
        SortField sortField = new SortField(userTableCriteria.GroupByField, FieldSortOrder.Ascending);
        string fieldJoinClause = queryEngine.GetFieldJoinClause((IQueryTerm[]) null, (QueryCriterion) null, new SortField[1]
        {
          sortField
        });
        string sqlColumnName = queryEngine.FieldTranslator.TranslateName(userTableCriteria.GroupByField).ToSqlColumnName();
        str2 = fieldJoinClause + " INNER JOIN #GroupByValue ON " + sqlColumnName + " = Value";
        string str3 = string.Empty == fieldJoinClause ? "LoanSummary Loan" : sortField.Term.GetTableNames(queryEngine.FieldTranslator)[0];
        stringBuilder2.AppendLine("CREATE TABLE #GroupByValue (Value VARCHAR(255))");
        stringBuilder2.Append("INSERT INTO #GroupByValue (Value) ");
        stringBuilder2.Append("SELECT DISTINCT TOP 20 SUBSTRING(" + sqlColumnName + ", 1, 255) FROM LoanSummary Loan ");
        if ("LoanSummary Loan" != str3)
          stringBuilder2.Append(fieldJoinClause);
        stringBuilder2.Append("WHERE " + sqlColumnName + " IS NOT NULL ");
        stringBuilder2.Append("AND Loan.Guid IN (SELECT Guid FROM #UserGuidPair) ");
        stringBuilder2.AppendLine("ORDER BY SUBSTRING(" + sqlColumnName + ", 1, 255)");
      }
      string userTableFieldList = EllieMae.EMLite.Server.Dashboard.getUserTableFieldList(userTableCriteria, queryEngine.FieldTranslator);
      string empty = string.Empty;
      List<SortField> sortFieldList = new List<SortField>();
      sortFieldList.Add(new SortField("#UserGuidPair.UserId", FieldSortOrder.Ascending));
      if ("Dashboard.NoGroupBy" != userTableCriteria.GroupByField)
        sortFieldList.Add(new SortField(userTableCriteria.GroupByField, FieldSortOrder.Ascending));
      string str4 = " " + EllieMae.EMLite.DataAccess.SQL.ToOrderByClause(sortFieldList.ToArray(), queryEngine.FieldTranslator);
      StringBuilder stringBuilder3 = new StringBuilder();
      stringBuilder3.Append("GROUP BY #UserGuidPair.UserId");
      if ("Dashboard.NoGroupBy" != userTableCriteria.GroupByField)
        stringBuilder3.AppendLine(", " + (object) queryEngine.FieldTranslator.TranslateName(userTableCriteria.GroupByField));
      StringBuilder stringBuilder4 = new StringBuilder();
      stringBuilder4.Append("SELECT " + userTableFieldList + " FROM #UserGuidPair ");
      stringBuilder4.AppendLine("INNER JOIN LoanSummary Loan ON #UserGuidPair.Guid = Loan.Guid");
      if ("Dashboard.NoGroupBy" != userTableCriteria.GroupByField)
        stringBuilder4.AppendLine(str2);
      stringBuilder4.AppendLine(queryEngine.GetFieldJoinClause((IQueryTerm[]) DataField.CreateFields(EllieMae.EMLite.Server.Dashboard.getCriterionNames(EllieMae.EMLite.Server.Dashboard.getUserTableSummaryFieldInfos(userTableCriteria, fieldLookup)).ToArray()), (QueryCriterion) null, (SortField[]) null));
      stringBuilder4.AppendLine(stringBuilder3.ToString() + str4);
      StringBuilder stringBuilder5 = new StringBuilder();
      stringBuilder5.AppendLine("DROP TABLE #UserGuidPair");
      if ("Dashboard.NoGroupBy" != userTableCriteria.GroupByField)
        stringBuilder5.AppendLine("DROP TABLE #GroupByValue");
      string str5 = string.Empty;
      if (queryEngine.CurrentUser != (UserInfo) null)
        str5 = "-- Parameterizing userId \r\ndeclare @userId varchar(16)\r\nset @userId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) queryEngine.CurrentUser.Userid) + Environment.NewLine;
      return str5 + stringBuilder1.ToString() + stringBuilder2.ToString() + stringBuilder4.ToString() + stringBuilder5.ToString();
    }

    private static string getUserTableFieldList(
      UserTableDataCriteria userTableCriteria,
      ICriterionTranslator fieldTranslator)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("#UserGuidPair.UserId, ");
      if ("Dashboard.NoGroupBy" != userTableCriteria.GroupByField)
        stringBuilder.Append("SUBSTRING(" + (object) fieldTranslator.TranslateName(userTableCriteria.GroupByField) + ", 1, 255), ");
      if ("Dashboard.NoSummary" != userTableCriteria.SummaryField1)
        stringBuilder.Append(EllieMae.EMLite.Server.Dashboard.buildAggregateString(userTableCriteria.SummaryField1, userTableCriteria.SummaryType1, fieldTranslator) + ", ");
      if ("Dashboard.NoSummary" != userTableCriteria.SummaryField2)
        stringBuilder.Append(EllieMae.EMLite.Server.Dashboard.buildAggregateString(userTableCriteria.SummaryField2, userTableCriteria.SummaryType2, fieldTranslator) + ", ");
      if ("Dashboard.NoSummary" != userTableCriteria.SummaryField3)
        stringBuilder.Append(EllieMae.EMLite.Server.Dashboard.buildAggregateString(userTableCriteria.SummaryField3, userTableCriteria.SummaryType3, fieldTranslator) + ", ");
      return stringBuilder.ToString().Substring(0, stringBuilder.Length - 2);
    }

    private static string buildAggregateString(
      string fieldCriterionName,
      ColumnSummaryType summaryType,
      ICriterionTranslator fieldTranslator)
    {
      string str1 = ColumnSummaryType.Average == summaryType ? "AVG" : (ColumnSummaryType.Total == summaryType ? "SUM" : "COUNT");
      string str2 = "COUNT" == str1 ? "*" : "CONVERT(MONEY, " + (object) fieldTranslator.TranslateName(fieldCriterionName) + ")";
      string str3 = fieldCriterionName.Substring(fieldCriterionName.IndexOf('.') + 1);
      return str1 + "(" + str2 + ") AS [" + str3 + "]";
    }

    private static List<string> getCriterionNames(List<EllieMae.EMLite.Server.Dashboard.FieldInfo> fieldInfos)
    {
      List<string> criterionNames = new List<string>(fieldInfos.Count);
      foreach (EllieMae.EMLite.Server.Dashboard.FieldInfo fieldInfo in fieldInfos)
        criterionNames.Add(fieldInfo.CriterionName);
      return criterionNames;
    }

    private static void buildTemplateLevelRoleFilter(
      UserTableDataCriteria userTableCriteria,
      StringBuilder sqlWhereClause,
      StringBuilder sqlTableJoins)
    {
      sqlTableJoins.Append("INNER JOIN UserPersona up ON users.userid = up.userid ");
      sqlTableJoins.AppendLine("INNER JOIN RolePersonas rp ON up.personaID = rp.personaID");
      sqlWhereClause.Append(sqlWhereClause.Length == 0 ? " WHERE " : " AND ");
      StringBuilder stringBuilder = sqlWhereClause;
      int roleId = userTableCriteria.RoleId;
      string str1 = roleId.ToString();
      roleId = userTableCriteria.RoleId;
      string str2 = roleId.ToString();
      string str3 = "rp.roleID = " + str1 + " and associate.roleID = " + str2;
      stringBuilder.AppendLine(str3);
    }

    private static void buildTemplateLevelOrganizationFilter(
      UserTableDataCriteria userTableCriteria,
      StringBuilder sqlWhereClause,
      StringBuilder sqlTableJoins)
    {
      if (userTableCriteria.IncludeChildren)
        sqlTableJoins.AppendLine("LEFT OUTER JOIN org_descendents ON users.org_id = org_descendents.descendent");
      sqlWhereClause.Append(sqlWhereClause.Length == 0 ? " WHERE " : " AND ");
      StringBuilder stringBuilder1 = sqlWhereClause;
      int organizationId = userTableCriteria.OrganizationId;
      string str1 = "(users.org_id = " + organizationId.ToString() + " ";
      stringBuilder1.Append(str1);
      if (userTableCriteria.IncludeChildren)
      {
        StringBuilder stringBuilder2 = sqlWhereClause;
        organizationId = userTableCriteria.OrganizationId;
        string str2 = "OR org_descendents.oid = " + organizationId.ToString() + " ";
        stringBuilder2.Append(str2);
      }
      sqlWhereClause.Append(") ");
    }

    private static void buildTemplateLevelGroupFilter(
      UserTableDataCriteria userTableCriteria,
      StringBuilder sqlTableJoins)
    {
      sqlTableJoins.AppendLine("INNER JOIN AclGroupMembers gu ON (gu.GroupID = " + (object) userTableCriteria.UserGroupId + " and associate.UserID = gu.userId)");
    }

    private static DashboardViewInfo createDashboardViewInfo(DataRow viewRow, DataRow[] reportRows)
    {
      DashboardViewInfo dashboardViewInfo = EllieMae.EMLite.Server.Dashboard.convertRowToDashboardViewInfo(viewRow);
      int length = reportRows.Length;
      dashboardViewInfo.DashboardReportInfos = new DashboardReportInfo[length];
      for (int index = 0; index < length; ++index)
      {
        DataRow reportRow = reportRows[index];
        dashboardViewInfo.DashboardReportInfos[index] = EllieMae.EMLite.Server.Dashboard.convertRowToDashboardReportInfo(reportRow);
      }
      return dashboardViewInfo;
    }

    private static DashboardViewInfo convertRowToDashboardViewInfo(DataRow viewRow)
    {
      return new DashboardViewInfo((int) viewRow["ViewId"], (string) viewRow["UserId"], (string) viewRow["ViewName"], (DashboardViewFilterType) viewRow["ViewFilterType"], (int) viewRow["ViewFilterRoleId"], (string) viewRow["ViewFilterUserInRole"], (int) viewRow["ViewFilterOrganizationId"], (bool) viewRow["ViewFilterIncludeChildren"], (int) viewRow["ViewFilterUserGroupId"], (string) viewRow["ViewFilterTPOOrgId"], (bool) viewRow["ViewTPOFilterIncludeChildren"], viewRow["LayoutId"] == DBNull.Value ? 0 : (int) viewRow["LayoutId"], Convert.ToString(viewRow["TemplatePath"]), viewRow["IsFolder"] != DBNull.Value && Convert.ToBoolean(viewRow["IsFolder"]), (DashboardReportInfo[]) null, new Guid(string.Concat(viewRow["Guid"])));
    }

    private static DashboardReportInfo convertRowToDashboardReportInfo(DataRow reportRow)
    {
      return new DashboardReportInfo((int) reportRow["ReportId"], (int) reportRow["ViewId"], (int) reportRow["LayoutBlockNumber"], (string) reportRow["DashboardTemplatePath"], (string) reportRow["ParameterString"]);
    }

    private static bool findDashboardView(string userId, string viewName)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT COUNT(*) FROM DashboardView v WHERE v.UserId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) + " AND v.ViewName LIKE '" + EllieMae.EMLite.DataAccess.SQL.Escape(viewName) + "'");
      return (int) dbQueryBuilder.ExecuteScalar() != 0;
    }

    private static DbValueList createDbValueList(DashboardViewInfo viewInfo)
    {
      return new DbValueList()
      {
        {
          "UserId",
          (object) viewInfo.UserId
        },
        {
          "ViewName",
          (object) viewInfo.ViewName,
          (IDbEncoder) DbEncoding.EmptyStringAsNull
        },
        {
          "ViewFilterType",
          (object) (int) viewInfo.ViewFilterType
        },
        {
          "ViewFilterRoleId",
          (object) viewInfo.ViewFilterRoleId
        },
        {
          "ViewFilterUserInRole",
          (object) viewInfo.ViewFilterUserInRole
        },
        {
          "ViewFilterOrganizationId",
          (object) viewInfo.ViewFilterOrganizationId
        },
        {
          "ViewFilterIncludeChildren",
          (object) (viewInfo.ViewFilterIncludeChildren ? 1 : 0)
        },
        {
          "ViewFilterUserGroupId",
          (object) viewInfo.ViewFilterUserGroupId
        },
        {
          "ViewFilterTPOOrgId",
          (object) viewInfo.ViewFilterTPOOrgId
        },
        {
          "ViewTPOFilterIncludeChildren",
          (object) (viewInfo.ViewTPOFilterIncludeChildren ? 1 : 0)
        },
        {
          "TemplatePath",
          (object) viewInfo.TemplatePath
        },
        {
          "IsFolder",
          (object) (viewInfo.IsFolder ? 1 : 0)
        },
        {
          "LayoutId",
          (object) viewInfo.LayoutId
        },
        {
          "Guid",
          (object) viewInfo.Guid.ToString()
        }
      };
    }

    private static DbValueList createDbValueList(DashboardReportInfo reportInfo)
    {
      return new DbValueList()
      {
        {
          "ViewId",
          (object) "@viewId",
          (IDbEncoder) DbEncoding.None
        },
        {
          "LayoutBlockNumber",
          (object) reportInfo.LayoutBlockNumber
        },
        {
          "DashboardTemplatePath",
          (object) reportInfo.DashboardTemplatePath
        },
        {
          "ParameterString",
          (object) reportInfo.ParameterString
        }
      };
    }

    public class FieldInfo
    {
      private string criterionName = string.Empty;
      private string fieldId = string.Empty;
      private bool isHidden;

      public string CriterionName
      {
        get => this.criterionName;
        set => this.criterionName = value;
      }

      public string FieldId
      {
        get => this.fieldId;
        set => this.fieldId = value;
      }

      public bool IsHidden
      {
        get => this.isHidden;
        set => this.isHidden = value;
      }

      private FieldInfo()
      {
      }

      public FieldInfo(string criterionName, string fieldId, bool isHidden)
      {
        this.criterionName = criterionName;
        this.fieldId = fieldId;
        this.isHidden = isHidden;
      }
    }
  }
}
