// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.ReportSettingsAccessor
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class ReportSettingsAccessor
  {
    private const string REPORT_SETTINGS = "ReportSettings";
    private const string REPORT_MILESTONES = "ReportMilestones";
    private const string REPORT_FIELDS = "ReportFields";
    private const string REPORT_FILTERS = "ReportFilters";
    private const string REPORT_FOLDERS = "ReportFolders";
    private const string MILESTONES = "Milestones";
    private const int REPORT_BATCH_LIMIT = 10;
    private const int PAUSE_MILLI_SECONDS = 10;

    public static ReportSettings GetReportSettings(FileSystemEntry fileSystemEntry)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append(string.Format("SELECT R.*, M.Name AS [FileStage] from {0} R ", (object) "ReportSettings"));
      dbQueryBuilder.Append(string.Format("LEFT OUTER JOIN {0} M ", (object) "Milestones"));
      dbQueryBuilder.Append("ON R.FileStageMilestoneID = M.MilestoneID");
      dbQueryBuilder.Append(string.Format(" WHERE FilePath = {0}", (object) SQL.EncodeString(fileSystemEntry.ToString())));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      ReportSettings reportSettings = (ReportSettings) null;
      if (dataRowCollection != null && dataRowCollection.Count != 0 && dataRowCollection[0]["MSAnyStage"] != DBNull.Value)
        return ReportSettingsAccessor.getReportSettings(fileSystemEntry.Name, dataRowCollection[0]);
      if (fileSystemEntry.Type == FileSystemEntry.Types.File)
      {
        using (ReportSettingsFile reportSettingsFile = ReportSettingsStore.CheckOut(fileSystemEntry))
          reportSettings = !reportSettingsFile.Exists ? (ReportSettings) null : reportSettingsFile.Settings;
      }
      else if (fileSystemEntry.Type == FileSystemEntry.Types.Folder)
        reportSettings = new ReportSettings(fileSystemEntry.Name);
      ReportSettingsAccessor.SaveReportSettings(fileSystemEntry, reportSettings);
      return ReportSettingsAccessor.GetReportSettings(fileSystemEntry);
    }

    public static Dictionary<FileSystemEntry, ReportSettings> GetFileSystemEntries(
      FileSystemEntry[] fileSystemEntryList)
    {
      if (fileSystemEntryList == null)
        return (Dictionary<FileSystemEntry, ReportSettings>) null;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      EllieMae.EMLite.Server.DbAccessManager.GetTable("ReportSettings");
      Dictionary<FileSystemEntry, ReportSettings> fileSystemEntries = new Dictionary<FileSystemEntry, ReportSettings>();
      foreach (FileSystemEntry fileSystemEntry in fileSystemEntryList)
      {
        if (fileSystemEntry.Type == FileSystemEntry.Types.File || fileSystemEntry.Type == FileSystemEntry.Types.Folder)
        {
          dbQueryBuilder.AppendLine(string.Format("SELECT COUNT(1) FROM [{0}] WHERE FilePath = {1}", (object) "ReportSettings", (object) SQL.EncodeString(fileSystemEntry.ToString())));
          int num = (int) dbQueryBuilder.ExecuteScalar();
          dbQueryBuilder.Reset();
          if (num == 0)
          {
            if (fileSystemEntry.Type == FileSystemEntry.Types.File)
            {
              using (ReportSettingsFile reportSettingsFile = ReportSettingsStore.CheckOut(fileSystemEntry))
                fileSystemEntries.Add(fileSystemEntry, reportSettingsFile.Settings);
            }
            else if (fileSystemEntry.Type == FileSystemEntry.Types.Folder)
            {
              ReportSettings reportSettings = new ReportSettings(fileSystemEntry.Name);
              fileSystemEntries.Add(fileSystemEntry, reportSettings);
            }
          }
        }
      }
      return fileSystemEntries;
    }

    public static void SaveReportSettings(
      Dictionary<FileSystemEntry, ReportSettings> fileSystemEntryList,
      string loginUserID,
      string userFullName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@ReportID", "INT");
      dbQueryBuilder.Declare("@MilestoneID", "VARCHAR(38)");
      int count = fileSystemEntryList.Keys.Count;
      int num = 0;
      foreach (FileSystemEntry key in fileSystemEntryList.Keys)
      {
        dbQueryBuilder.Append(ReportSettingsAccessor.GenerateSaveReportSettingsSql(key, fileSystemEntryList[key]));
        ++num;
        if (num == 10)
        {
          dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
          dbQueryBuilder.Reset();
          dbQueryBuilder.Declare("@ReportID", "INT");
          dbQueryBuilder.Declare("@MilestoneID", "VARCHAR(38)");
          Thread.Sleep(10);
          num = 0;
        }
        ReportSettingsAccessor.updateReportXml(key, fileSystemEntryList[key], loginUserID, userFullName);
      }
      if (!(dbQueryBuilder.ToString() != string.Empty))
        return;
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    public static void SaveReportSettings(
      FileSystemEntry fileSystemEntry,
      ReportSettings reportSettings)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@ReportID", "INT");
      dbQueryBuilder.Declare("@MilestoneID", "VARCHAR(38)");
      dbQueryBuilder.Append(ReportSettingsAccessor.GenerateSaveReportSettingsSql(fileSystemEntry, reportSettings));
      if (!(dbQueryBuilder.ToString() != string.Empty))
        return;
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    public static void SaveReportSettings(
      FileSystemEntry entry,
      ReportSettings settings,
      string loginUserID,
      string userFullName)
    {
      ReportSettingsAccessor.SaveReportSettings(entry, settings);
      ReportSettingsAccessor.updateReportXml(entry, settings, loginUserID, userFullName);
    }

    public static void DeleteReportSettingsObject(
      FileSystemEntry entry,
      string loginUserID,
      string userFullName)
    {
      int reportId = ReportSettingsAccessor.GetReportSettings(entry).ReportID;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (entry.Type == FileSystemEntry.Types.File)
      {
        dbQueryBuilder.AppendLine(string.Format("Delete FROM [{0}] WHERE ReportID = {1}", (object) "ReportFolders", (object) reportId));
        dbQueryBuilder.AppendLine(string.Format("Delete FROM [{0}] WHERE ReportID = {1}", (object) "ReportFields", (object) reportId));
        dbQueryBuilder.AppendLine(string.Format("Delete FROM [{0}] WHERE ReportID = {1}", (object) "ReportFilters", (object) reportId));
        dbQueryBuilder.AppendLine(string.Format("Delete FROM [{0}] WHERE ReportID = {1}", (object) "ReportMilestones", (object) reportId));
      }
      dbQueryBuilder.AppendLine(string.Format("Delete FROM [{0}] WHERE ReportID = {1}", (object) "ReportSettings", (object) reportId));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      ReportSettingsStore.Delete(entry);
      SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(loginUserID, userFullName, ActionType.TemplateDeleted, DateTime.Now, entry.Name, entry.Path));
    }

    public static void MoveReportSettingsObject(
      FileSystemEntry source,
      FileSystemEntry target,
      string loginUserID,
      string userFullName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (source.Type == FileSystemEntry.Types.Folder)
      {
        int reportId = ReportSettingsAccessor.GetReportSettings(source).ReportID;
        dbQueryBuilder.AppendLine(string.Format("UPDATE [{0}] SET FilePath = REPLACE(FilePath, {1}, {2})  WHERE FilePath like {3}", (object) "ReportSettings", (object) SQL.EncodeString(source.ToString()), (object) SQL.EncodeString(target.ToString()), (object) SQL.EncodeString(source.ToString() + "%")));
        dbQueryBuilder.AppendLine(string.Format("UPDATE [{0}] SET ReportTitle = {1} WHERE ReportID = {2}", (object) "ReportSettings", (object) SQL.EncodeString(target.Name), (object) reportId));
      }
      else if (source.Type == FileSystemEntry.Types.File)
      {
        int reportId = ReportSettingsAccessor.GetReportSettings(source).ReportID;
        dbQueryBuilder.AppendLine(string.Format("UPDATE [{0}] SET ReportTitle = {1}, FilePath = REPLACE(FilePath, {2}, {3})  WHERE ReportID  = {4}", (object) "ReportSettings", (object) SQL.EncodeString(target.Name), (object) SQL.EncodeString(source.ToString()), (object) SQL.EncodeString(target.ToString()), (object) reportId));
      }
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      ReportSettingsStore.Move(source, target);
      if (source.ParentFolder.Path == target.ParentFolder.Path)
      {
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(loginUserID, userFullName, ActionType.TemplateModified, DateTime.Now, source.Name, source.Path));
      }
      else
      {
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(loginUserID, userFullName, ActionType.TemplateDeleted, DateTime.Now, source.Name, source.Path));
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(loginUserID, userFullName, ActionType.TemplateCreated, DateTime.Now, target.Name, target.Path));
      }
    }

    public static void CopyReportSettingsObject(
      FileSystemEntry source,
      FileSystemEntry target,
      string loginUserID,
      string userFullName)
    {
      if (source.Type == FileSystemEntry.Types.File)
        ReportSettingsAccessor.copyFile(source, target);
      else
        ReportSettingsAccessor.copyFolder(source, target);
      ReportSettingsStore.Copy(source, target);
      SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(loginUserID, userFullName, ActionType.TemplateCreated, DateTime.Now, target.Name, target.Path));
    }

    public static void SaveReportSettings(FileSystemEntry[] fileSystemEntryList)
    {
      ReportSettings reportSettings = (ReportSettings) null;
      foreach (FileSystemEntry fileSystemEntry in fileSystemEntryList)
      {
        if (fileSystemEntry.Type != FileSystemEntry.Types.File && fileSystemEntry.Type != FileSystemEntry.Types.Folder)
          break;
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine(string.Format("SELECT COUNT(1) FROM [{0}] WHERE FilePath = {1}", (object) "ReportSettings", (object) SQL.EncodeString(fileSystemEntry.ToString())));
        if ((int) dbQueryBuilder.ExecuteScalar() == 0)
        {
          using (ReportSettingsFile reportSettingsFile = ReportSettingsStore.CheckOut(fileSystemEntry))
            reportSettings = !reportSettingsFile.Exists ? (ReportSettings) null : reportSettingsFile.Settings;
          ReportSettingsAccessor.SaveReportSettings(fileSystemEntry, reportSettings);
        }
      }
    }

    public static string[] GetMilestoneNamesByReportFilePaths(string[] computedFilePaths)
    {
      List<string> stringList = new List<string>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append(string.Format("SELECT DISTINCT (M.Name + ',' + M.MilestoneID) AS Name FROM [{0}] R ", (object) "ReportSettings"));
      dbQueryBuilder.Append(string.Format("INNER JOIN [{0}] RM ON R.ReportID = RM.ReportID ", (object) "ReportMilestones"));
      dbQueryBuilder.Append("INNER JOIN Milestones M ON RM.MilestoneID = M.MilestoneID ");
      dbQueryBuilder.Append(string.Format("WHERE R.ComputedFilePath in ({0})", (object) string.Join(",", SQL.EncodeString(computedFilePaths, true))));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          stringList.Add(dataRow["Name"].ToString());
      }
      return stringList.ToArray();
    }

    private static string GenerateSaveReportSettingsSql(
      FileSystemEntry fileSystemEntry,
      ReportSettings reportSettings)
    {
      if (fileSystemEntry.Type != FileSystemEntry.Types.File && fileSystemEntry.Type != FileSystemEntry.Types.Folder)
        return string.Empty;
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ReportSettings");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbValue dbValue1 = new DbValue("FilePath", (object) fileSystemEntry.ToString());
      DbValue dbValue2 = new DbValue("IsFolder", (object) (fileSystemEntry.Type == FileSystemEntry.Types.Folder ? 1 : 0));
      DbValueList values = new DbValueList();
      values.Add(new DbValue("ReportTitle", (object) fileSystemEntry.Name));
      values.Add(new DbValue("Version", (object) reportSettings.VerNo));
      values.Add(dbValue1);
      values.Add(new DbValue("ReportFor", (object) (int) reportSettings.ReportFor));
      values.Add(new DbValue("ReportType", (object) reportSettings.ReportType));
      values.Add(dbValue2);
      values.Add(new DbValue("FileStageMilestoneID", string.IsNullOrEmpty(reportSettings.FileStageMileStoneID) ? (object) (string) null : (object) reportSettings.FileStageMileStoneID));
      values.Add(new DbValue("TimeFrame", (object) reportSettings.TimeFrame));
      DateTime? nullable1 = new DateTime?();
      if (reportSettings.TimeFrom < DateTime.ParseExact("01011900", "MMddyyyy", (IFormatProvider) CultureInfo.InvariantCulture))
        reportSettings.TimeFrom = DateTime.MinValue;
      else
        nullable1 = new DateTime?(reportSettings.TimeFrom);
      values.Add(new DbValue("TimeFrom", (object) nullable1));
      DateTime? nullable2 = new DateTime?();
      if (reportSettings.TimeTo > DateTime.ParseExact("01012100", "MMddyyyy", (IFormatProvider) CultureInfo.InvariantCulture))
        reportSettings.TimeTo = DateTime.MinValue;
      else
        nullable2 = new DateTime?(reportSettings.TimeTo);
      values.Add(new DbValue("TimeTo", (object) nullable2));
      values.Add(new DbValue("PaperSize", (object) reportSettings.PaperSize));
      values.Add(new DbValue("PaperOrientation", (object) reportSettings.PaperOrientation));
      values.Add(new DbValue("TopMargin", (object) reportSettings.TopMargin));
      values.Add(new DbValue("BottomMargin", (object) reportSettings.BottomMargin));
      values.Add(new DbValue("LeftMargin", (object) reportSettings.LeftMargin));
      values.Add(new DbValue("RightMargin", (object) reportSettings.RightMargin));
      values.Add(new DbValue("FolderOption", (object) (int) reportSettings.FolderOption));
      values.Add(new DbValue("UseFieldInDB", (object) (reportSettings.UseFieldInDB ? 1 : 0)));
      values.Add(new DbValue("UseFilterFieldInDB", (object) (reportSettings.UseFilterFieldInDB ? 1 : 0)));
      values.Add(new DbValue("RelatedLoanFilterSource", (object) (int) reportSettings.RelatedLoanFilterSource));
      values.Add(new DbValue("RelatedLoanFieldSource", (object) (int) reportSettings.RelatedLoanFieldSource));
      values.Add(new DbValue("ForTPO", (object) SQL.EncodeFlag(reportSettings.ForTPO)));
      values.Add(new DbValue("IncludeChildFolder", (object) SQL.EncodeFlag(reportSettings.TpoFilterIncludeChildFolder)));
      values.Add(new DbValue("MSAnyStage", (object) SQL.EncodeFlag(reportSettings.MSAnyStage)));
      if (reportSettings.ReportID <= 0)
      {
        DbValueList dbValueList = new DbValueList();
        dbValueList.Add(dbValue1);
        dbValueList.Add(dbValue2);
        dbQueryBuilder.IfExists(table, dbValueList);
        dbQueryBuilder.Begin();
        dbQueryBuilder.Update(table, values, dbValueList);
        dbQueryBuilder.AppendLine(string.Format("SELECT @ReportID = ReportID from [{0}] WHERE FilePath = {1} and IsFolder = {2}", (object) "ReportSettings", (object) SQL.EncodeString(dbValue1.Value.ToString()), (object) dbValue2.Value.ToString()));
        dbQueryBuilder.End();
        dbQueryBuilder.Else();
        dbQueryBuilder.Begin();
        dbQueryBuilder.InsertInto(table, values, true, false);
        dbQueryBuilder.SelectIdentity("@ReportID");
        dbQueryBuilder.End();
      }
      else
      {
        dbQueryBuilder.AppendLine(string.Format("SET @ReportID = {0}", (object) reportSettings.ReportID));
        DbValue key = new DbValue("ReportID", (object) reportSettings.ReportID);
        dbQueryBuilder.Update(table, values, key);
      }
      dbQueryBuilder.AppendLine(string.Format("Delete FROM [{0}] WHERE ReportID = @ReportID", (object) "ReportFolders"));
      dbQueryBuilder.AppendLine(string.Format("Delete FROM [{0}] WHERE ReportID = @ReportID", (object) "ReportFields"));
      dbQueryBuilder.AppendLine(string.Format("Delete FROM [{0}] WHERE ReportID = @ReportID", (object) "ReportFilters"));
      dbQueryBuilder.AppendLine(string.Format("Delete FROM [{0}] WHERE ReportID = @ReportID", (object) "ReportMilestones"));
      if (fileSystemEntry.Type == FileSystemEntry.Types.File)
      {
        dbQueryBuilder.AppendLine(ReportSettingsAccessor.getInsertSqlForFields(reportSettings.Columns));
        dbQueryBuilder.AppendLine(ReportSettingsAccessor.getInsertSqlForMilestones(reportSettings.Milestones));
        if (reportSettings.Folders != null && ((IEnumerable<string>) reportSettings.Folders).Count<string>() > 0)
          dbQueryBuilder.AppendLine(ReportSettingsAccessor.getInsertSqlForFolders(reportSettings.Folders));
        if (reportSettings.Filters != null && ((IEnumerable<FieldFilter>) reportSettings.Filters).Count<FieldFilter>() > 0)
          dbQueryBuilder.AppendLine(ReportSettingsAccessor.getInsertSqlForFilters(reportSettings.Filters));
      }
      return dbQueryBuilder.ToString();
    }

    private static List<string> populateMilestonesForReport(string reportID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append(string.Format("SELECT M.Name FROM [{0}] M ", (object) "Milestones"));
      dbQueryBuilder.Append(string.Format("INNER JOIN [{0}] R ", (object) "ReportMilestones"));
      dbQueryBuilder.Append(string.Format("ON M.MilestoneID = R.MilestoneID WHERE ReportID = {0}", (object) reportID));
      return dbQueryBuilder.Execute().Cast<DataRow>().Select<DataRow, string>((System.Func<DataRow, string>) (row => row["Name"].ToString())).ToList<string>();
    }

    private static EllieMae.EMLite.ClientServer.Reporting.ColumnInfo[] populateFieldsForReport(
      string reportID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append(string.Format("SELECT * FROM [{0}] WHERE ReportID = {1} ORDER BY [ReportFieldsSerial] ASC", (object) "ReportFields", (object) reportID));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      List<EllieMae.EMLite.ClientServer.Reporting.ColumnInfo> columnInfoList = new List<EllieMae.EMLite.ClientServer.Reporting.ColumnInfo>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        columnInfoList.Add(new EllieMae.EMLite.ClientServer.Reporting.ColumnInfo(dataRow["ID"].ToString(), SQL.DecodeString(dataRow["Description"]), SQL.DecodeEnum<ColumnSortOrder>((object) dataRow["SortOrderID"].ToString()), SQL.DecodeEnum<ColumnSummaryType>((object) dataRow["SummaryTypeID"].ToString()), SQL.DecodeInt(dataRow["DecimalPlaces"]))
        {
          ExcelFormula = SQL.DecodeString(dataRow["ExcelFormula"]),
          IsExcelField = SQL.DecodeBoolean(dataRow["IsExcelField"]),
          CriterionName = SQL.DecodeString(dataRow["CriterionName"]),
          ComortPair = SQL.DecodeInt(dataRow["ComortPair"]),
          Format = SQL.DecodeEnum<FieldFormat>((object) dataRow["FormatID"].ToString())
        });
      return columnInfoList.ToArray();
    }

    private static string[] populateFoldersForReport(string reportID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ReportFolders");
      DbValue key = new DbValue("ReportID", (object) reportID);
      dbQueryBuilder.SelectFrom(table, new string[1]
      {
        "FolderName"
      }, key);
      return dbQueryBuilder.Execute().Cast<DataRow>().Select<DataRow, string>((System.Func<DataRow, string>) (row => row["FolderName"].ToString())).ToArray<string>();
    }

    private static FieldFilter[] populateFiltersForReport(string reportID)
    {
      List<FieldFilter> fieldFilterList = new List<FieldFilter>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append(string.Format("SELECT * FROM [{0}] WHERE ReportID = {1} ORDER BY [ReportFiltersSerial] ASC", (object) "ReportFilters", (object) reportID));
      foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        fieldFilterList.Add(new FieldFilter(SQL.DecodeEnum<FieldTypes>((object) dataRow["FieldTypeID"].ToString()), SQL.DecodeString(dataRow["FieldID"]), SQL.DecodeString(dataRow["CriterionName"]), SQL.DecodeString(dataRow["FieldDescription"]), SQL.DecodeEnum<OperatorTypes>((object) dataRow["OperatorTypeID"].ToString()), SQL.DecodeString(dataRow["ValueFrom"]), SQL.DecodeString(dataRow["ValueTo"]))
        {
          RightParentheses = SQL.DecodeInt(dataRow["RightParentheses"]),
          LeftParentheses = SQL.DecodeInt(dataRow["LeftParentheses"]),
          CriterionName = SQL.DecodeString(dataRow["CriterionName"]),
          JointToken = SQL.DecodeEnum<JointTokens>((object) dataRow["JointTokensID"].ToString())
        });
      return fieldFilterList.ToArray();
    }

    private static string getInsertSqlForFields(EllieMae.EMLite.ClientServer.Reporting.ColumnInfo[] columnInfoList)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (EllieMae.EMLite.ClientServer.Reporting.ColumnInfo columnInfo in columnInfoList)
      {
        dbQueryBuilder.AppendLine("insert into ReportFields");
        dbQueryBuilder.AppendLine("([ReportID], [Description], [FilterListOrder], [ExcelFormula], [IsExcelField], [CriterionName], [ComortPair], [DecimalPlaces], [SummaryTypeID], [SortOrderID], [ID], [FormatID])");
        dbQueryBuilder.AppendLine(string.Format("values (@ReportID, {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})", (object) SQL.EncodeString(columnInfo.Description), (object) columnInfo.FilterListOrder, (object) SQL.EncodeString(columnInfo.ExcelFormula), (object) (columnInfo.IsExcelField ? 1 : 0), (object) SQL.EncodeString(columnInfo.CriterionName), (object) columnInfo.ComortPair, (object) columnInfo.DecimalPlaces, (object) (int) columnInfo.SummaryType, (object) (int) columnInfo.SortOrder, (object) SQL.EncodeString(columnInfo.ID), (object) (int) columnInfo.Format));
      }
      return dbQueryBuilder.ToString();
    }

    private static string getInsertSqlForMilestones(List<string> milestoneNames)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (string milestoneName in milestoneNames)
      {
        dbQueryBuilder.AppendLine("SET @MilestoneID = NULL");
        dbQueryBuilder.AppendLine(string.Format("SELECT @MilestoneID = MilestoneID FROM [{0}] WHERE Name = {1} ", (object) "Milestones", (object) SQL.EncodeString(milestoneName)));
        dbQueryBuilder.If("@MilestoneID IS NOT NULL");
        dbQueryBuilder.Begin();
        dbQueryBuilder.AppendLine(string.Format("INSERT INTO [{0}] ([ReportID],[MilestoneID]) VALUES (@ReportID, @MilestoneID)", (object) "ReportMilestones"));
        dbQueryBuilder.End();
      }
      return dbQueryBuilder.ToString();
    }

    private static string getInsertSqlForFolders(string[] folderNames)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (string folderName in folderNames)
        dbQueryBuilder.AppendLine(string.Format("INSERT INTO [{0}] ([ReportID],[FolderName]) VALUES (@ReportID, {1})", (object) "ReportFolders", (object) SQL.EncodeString(folderName)));
      return dbQueryBuilder.ToString();
    }

    private static string getInsertSqlForFilters(FieldFilter[] fieldFilters)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (FieldFilter fieldFilter in fieldFilters)
      {
        dbQueryBuilder.AppendLine("INSERT INTO [ReportFilters]");
        dbQueryBuilder.AppendLine("([ReportID],[FilterOrder],[CriterionName],[RightParentheses],[LeftParentheses],[JointTokensID],[ValueTo],[ValueFrom],[OperatorTypeID],[FieldTypeID],[FieldID],[FieldDescription])");
        dbQueryBuilder.AppendLine(string.Format("VALUES (@ReportID,{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})", (object) 0, (object) SQL.EncodeString(fieldFilter.CriterionName), (object) fieldFilter.RightParentheses, (object) fieldFilter.LeftParentheses, (object) (int) fieldFilter.JointToken, (object) SQL.EncodeString(fieldFilter.ValueTo), (object) SQL.EncodeString(fieldFilter.ValueFrom), (object) (int) fieldFilter.OperatorType, (object) (int) fieldFilter.FieldType, (object) SQL.EncodeString(fieldFilter.FieldID), (object) SQL.EncodeString(fieldFilter.FieldDescription)));
      }
      return dbQueryBuilder.ToString();
    }

    private static ReportSettings getReportSettings(string reportName, DataRow rowData)
    {
      ReportSettings reportSettings = new ReportSettings(SQL.DecodeString((object) rowData["ReportTitle"].ToString()));
      reportSettings.ReportID = SQL.DecodeInt(rowData["ReportID"]);
      reportSettings.VerNo = SQL.DecodeDouble((object) rowData["Version"].ToString());
      reportSettings.ReportFor = SQL.DecodeEnum<ReportsFor>((object) rowData["ReportFor"].ToString());
      reportSettings.ReportTitle = reportName;
      reportSettings.ReportType = SQL.DecodeString(rowData["ReportType"]);
      reportSettings.FileStage = SQL.DecodeString(rowData["FileStage"]);
      reportSettings.TimeFrame = SQL.DecodeString(rowData["TimeFrame"]);
      reportSettings.TimeFrom = SQL.DecodeDateTime(rowData["TimeFrom"]);
      reportSettings.TimeTo = SQL.DecodeDateTime(rowData["TimeTo"]);
      reportSettings.PaperSize = SQL.DecodeString(rowData["PaperSize"]);
      reportSettings.PaperOrientation = SQL.DecodeString(rowData["PaperOrientation"]);
      reportSettings.TopMargin = SQL.DecodeDouble(rowData["TopMargin"]);
      reportSettings.BottomMargin = SQL.DecodeDouble(rowData["BottomMargin"]);
      reportSettings.LeftMargin = SQL.DecodeDouble(rowData["LeftMargin"]);
      reportSettings.RightMargin = SQL.DecodeDouble(rowData["RightMargin"]);
      reportSettings.forTPO = SQL.DecodeBoolean(rowData["forTPO"]);
      reportSettings.TpoFilterIncludeChildFolder = SQL.DecodeBoolean(rowData["IncludeChildFolder"]);
      reportSettings.MSAnyStage = SQL.DecodeBoolean(rowData["MSAnyStage"]);
      reportSettings.UseFieldInDB = SQL.DecodeBoolean(rowData["UseFieldInDB"]);
      reportSettings.UseFilterFieldInDB = SQL.DecodeBoolean(rowData["UseFilterFieldInDB"]);
      reportSettings.RelatedLoanFilterSource = SQL.DecodeEnum<RelatedLoanMatchType>((object) rowData["RelatedLoanFilterSource"].ToString());
      reportSettings.RelatedLoanFieldSource = SQL.DecodeEnum<RelatedLoanMatchType>((object) rowData["RelatedLoanFieldSource"].ToString());
      reportSettings.FolderOption = SQL.DecodeEnum<ReportFolderOption>((object) rowData["FolderOption"].ToString());
      if (!SQL.DecodeBoolean(rowData["IsFolder"]))
      {
        reportSettings.Columns = ReportSettingsAccessor.populateFieldsForReport(rowData["ReportID"].ToString());
        reportSettings.Milestones = ReportSettingsAccessor.populateMilestonesForReport(rowData["ReportID"].ToString());
        reportSettings.Folders = ReportSettingsAccessor.populateFoldersForReport(rowData["ReportID"].ToString());
        reportSettings.Filters = ReportSettingsAccessor.populateFiltersForReport(rowData["ReportID"].ToString());
      }
      return reportSettings;
    }

    private static void copyFile(FileSystemEntry source, FileSystemEntry target)
    {
      if (target.Type == FileSystemEntry.Types.Folder)
        target = new FileSystemEntry(target.Path, source.Name, FileSystemEntry.Types.File, target.Owner);
      ReportSettings reportSettings = ReportSettingsAccessor.GetReportSettings(source);
      reportSettings.ReportID = 0;
      ReportSettingsAccessor.SaveReportSettings(target, reportSettings);
    }

    private static void copyFolder(FileSystemEntry source, FileSystemEntry target)
    {
      FileSystemEntry[] directoryEntries = FileStore.GetDirectoryEntries(ReportSettingsStore.getReportFolderPath(source), FileSystemEntry.Types.All, source.Owner, source.Path, false, true, false);
      for (int index = 0; index < directoryEntries.Length; ++index)
      {
        if (directoryEntries[index].Type == FileSystemEntry.Types.File)
          ReportSettingsAccessor.copyFile(directoryEntries[index], target);
        else
          ReportSettingsAccessor.copyFolder(directoryEntries[index], new FileSystemEntry(target.Path, directoryEntries[index].Name, FileSystemEntry.Types.Folder, target.Owner));
      }
    }

    private static void updateReportXml(
      FileSystemEntry entry,
      ReportSettings settings,
      string loginUserID,
      string userFullName)
    {
      if (entry.Type == FileSystemEntry.Types.Folder)
        return;
      using (ReportSettingsFile reportSettingsFile = ReportSettingsStore.CheckOut(entry))
      {
        if (reportSettingsFile.Exists)
        {
          reportSettingsFile.CheckIn(settings);
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(loginUserID, userFullName, ActionType.TemplateModified, DateTime.Now, entry.Name, entry.Path));
        }
        else
        {
          reportSettingsFile.CreateNew(settings);
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(loginUserID, userFullName, ActionType.TemplateCreated, DateTime.Now, entry.Name, entry.Path));
        }
      }
    }
  }
}
