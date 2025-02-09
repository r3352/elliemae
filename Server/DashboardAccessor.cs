// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.DashboardAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class DashboardAccessor
  {
    private const string className = "DashboardAccessor�";
    private const string DASHBOARD_TEMPLATE = "DashboardTemplate�";
    private const string DASHBOARD_TEMPLATE_FOLDER = "DashboardTemplateFolder�";
    private const string DASHBOARD_TEMPLATE_FILTER = "DashboardTemplateFilter�";
    private const string DASHBOARD_TEMPLATE_FIELD = "DashboardTemplateField�";
    private const int DASHBOARD_BATCH_LIMIT = 10;
    private const int PAUSE_MILLI_SECONDS = 10;
    private const string DASHBOARD_VIEW = "DashboardView�";

    public static FileSystemEntry[] GetDashboardSnapshotTemplateList(
      FileSystemEntry parentFolder,
      ISession session)
    {
      return DashboardAccessor.GetDashboardSnapshotTemplateList(parentFolder, session.GetUserInfo());
    }

    public static FileSystemEntry[] GetDashboardSnapshotTemplateList(
      FileSystemEntry parentFolder,
      UserInfo userInfo)
    {
      FileSystemEntry[] directoryEntries = TemplateSettingsStore.GetDirectoryEntries(userInfo, TemplateSettingsType.DashboardTemplate, parentFolder, FileSystemEntry.Types.All, false, true);
      if (directoryEntries != null && directoryEntries.Length != 0)
        DashboardAccessor.populateDashboardSnapshotTemplateDatabase(directoryEntries);
      return directoryEntries;
    }

    public static BinaryObject GetDashboardSnapshotTemplateSettings(FileSystemEntry entry)
    {
      DashboardAccessor.populateDashboardSnapshotTemplateDatabase(new FileSystemEntry[1]
      {
        entry
      });
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Declare("@DashboardTemplateID", "INT");
      dbQueryBuilder.AppendLine(string.Format("SELECT @DashboardTemplateID = [DashboardTemplateID] FROM {0} WHERE [TemplatePath] = {1}", (object) "DashboardTemplate", (object) SQL.EncodeString(entry.ToString())));
      dbQueryBuilder.AppendLine(string.Format("SELECT * FROM {0} WHERE [TemplatePath] = {1}", (object) "DashboardTemplate", (object) SQL.EncodeString(entry.ToString())));
      dbQueryBuilder.AppendLine(string.Format("SELECT * FROM {0} WHERE [DashboardTemplateID] = @DashboardTemplateID", (object) "DashboardTemplateFolder"));
      dbQueryBuilder.AppendLine(string.Format("SELECT * FROM {0} WHERE [DashboardTemplateID] = @DashboardTemplateID", (object) "DashboardTemplateFilter"));
      dbQueryBuilder.AppendLine(string.Format("SELECT * FROM {0} WHERE [DashboardTemplateID] = @DashboardTemplateID", (object) "DashboardTemplateField"));
      return DashboardAccessor.getDashboardSnapshotTemplateSettingsFromDataTable(dbQueryBuilder.ExecuteSetQuery(DbTransactionType.Default));
    }

    public static bool ExistsDashboardSnapshotTemplateSettings(
      FileSystemEntry entry,
      bool ofAnyType)
    {
      return DashboardAccessor.existsDashboardSnapshotTemplateSettings(entry, ofAnyType, false);
    }

    public static bool SaveDashboardSnapshotTemplateSettings(
      FileSystemEntry entry,
      BinaryObject data,
      bool isUpdate)
    {
      DbQueryBuilder sql = new DbQueryBuilder();
      sql.Declare("@DashboardTemplateID", "INT");
      sql.AppendLine(DashboardAccessor.generateDashboardSnapshotTemplateScript(entry, data, isUpdate).ToString());
      return DashboardAccessor.executeDashboardSQL(sql);
    }

    public static void RenameDashboardSnapshotTemplate(
      FileSystemEntry source,
      FileSystemEntry target)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (target.Type == FileSystemEntry.Types.Folder)
      {
        string str1 = DashboardAccessor.regenerateGUID(new FileSystemEntry(target.Path, source.Name, FileSystemEntry.Types.File, target.Owner), TemplateSettingsType.DashboardTemplate);
        string str2 = source.ToString();
        if (source.Type == FileSystemEntry.Types.File)
          str2 = str2.Remove(str2.LastIndexOf("\\") + 1);
        dbQueryBuilder.AppendLine(string.Format("UPDATE {0} SET Guid = {4}, [TemplatePath] = REPLACE([TemplatePath], {3}, {2}) WHERE [TemplatePath] = {1}", (object) "DashboardTemplate", (object) SQL.EncodeString(source.ToString()), (object) SQL.EncodeString(target.ToString()), (object) SQL.EncodeString(str2), (object) SQL.EncodeString(str1)));
      }
      else
      {
        string str = DashboardAccessor.regenerateGUID(target, TemplateSettingsType.DashboardTemplate);
        dbQueryBuilder.AppendLine(string.Format("UPDATE {0} SET Guid = {4}, [TemplateName] = {3}, [TemplatePath] = {2} WHERE [TemplatePath] = {1}", (object) "DashboardTemplate", (object) SQL.EncodeString(source.ToString()), (object) SQL.EncodeString(target.ToString()), (object) SQL.EncodeString(target.Name), (object) SQL.EncodeString(str)));
        target.Properties.Add((object) "IsNewTemplate", source.Properties[(object) "IsNewTemplate"]);
        target.Properties.Add((object) "IsValidTemplate", source.Properties[(object) "IsValidTemplate"]);
        target.Properties.Add((object) "Guid", (object) str);
        target.Properties.Add((object) "Name", (object) target.Name);
        target.Properties.Add((object) "Description", source.Properties[(object) "Description"]);
      }
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    public static void DuplicateDashboardSnapshotTemplate(FileSystemEntry target, ISession session)
    {
      DashboardAccessor.DuplicateDashboardSnapshotTemplate(target, session.GetUserInfo());
    }

    public static void DuplicateDashboardSnapshotTemplate(FileSystemEntry target, UserInfo userInfo)
    {
      if (target.Type == FileSystemEntry.Types.Folder)
      {
        FileSystemEntry[] directoryEntries = TemplateSettingsStore.GetDirectoryEntries(userInfo, TemplateSettingsType.DashboardTemplate, target, FileSystemEntry.Types.All, false, true);
        for (int index = 0; index < directoryEntries.Length; ++index)
        {
          if (directoryEntries[index].Type == FileSystemEntry.Types.Folder)
            DashboardAccessor.DuplicateDashboardSnapshotTemplate(directoryEntries[index], userInfo);
          DashboardAccessor.populateDashboardSnapshotTemplateDatabase(new FileSystemEntry[1]
          {
            directoryEntries[index]
          });
        }
      }
      DashboardAccessor.populateDashboardSnapshotTemplateDatabase(new FileSystemEntry[1]
      {
        target
      });
    }

    public static void DeleteDashboardSnapshotTemplate(FileSystemEntry entry)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("DELETE FROM {0} WHERE [TemplatePath] = {1}", (object) "DashboardTemplate", (object) SQL.EncodeString(entry.ToString())));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    private static bool existsDashboardSnapshotTemplateSettings(
      FileSystemEntry entry,
      bool ofAnyType,
      bool checkOnlyDB)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      bool flag = false;
      dbQueryBuilder.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE TemplatePath = {1}", (object) "DashboardTemplate", (object) SQL.EncodeString(entry.ToString())));
      if (Convert.ToInt32(dbQueryBuilder.ExecuteScalar()) > 0)
        flag = true;
      if (!checkOnlyDB && !flag)
        flag = !ofAnyType ? TemplateSettingsStore.Exists(TemplateSettingsType.DashboardTemplate, entry) : TemplateSettingsStore.ExistsOfAnyType(TemplateSettingsType.DashboardTemplate, entry);
      return flag;
    }

    private static FileSystemEntry[] getDashboardSnapshotTemplateFromDataTable(DataTable source)
    {
      if (!DashboardAccessor.hasData(source))
        return (FileSystemEntry[]) null;
      FileSystemEntry[] templateFromDataTable = new FileSystemEntry[source.Rows.Count];
      for (int index = 0; index < source.Rows.Count; ++index)
      {
        templateFromDataTable[index] = new FileSystemEntry("\\", SQL.DecodeString(source.Rows[index]["TemplateName"]), FileSystemEntry.Types.File, (string) null);
        templateFromDataTable[index].Properties.Add((object) "IsNewTemplate", (object) SQL.DecodeBoolean(source.Rows[index]["IsNewTemplate"]));
        templateFromDataTable[index].Properties.Add((object) "IsValidTemplate", (object) SQL.DecodeBoolean(source.Rows[index]["IsValidTemplate"]));
        templateFromDataTable[index].Properties.Add((object) "Guid", (object) SQL.DecodeString(source.Rows[index]["Guid"]));
        templateFromDataTable[index].Properties.Add((object) "Name", (object) SQL.DecodeString(source.Rows[index]["TemplateName"]));
        templateFromDataTable[index].Properties.Add((object) "Description", (object) SQL.DecodeString(source.Rows[index]["Description"]));
      }
      return templateFromDataTable;
    }

    private static BinaryObject getDashboardSnapshotTemplateSettingsFromDataTable(
      DataSet dsDashboardSnapshotTemplate)
    {
      if (dsDashboardSnapshotTemplate == null || dsDashboardSnapshotTemplate.Tables == null || dsDashboardSnapshotTemplate.Tables.Count == 0 || dsDashboardSnapshotTemplate.Tables[0].Rows == null || dsDashboardSnapshotTemplate.Tables[0].Rows.Count == 0)
        return (BinaryObject) null;
      DataRow row1 = dsDashboardSnapshotTemplate.Tables[0].Rows[0];
      DashboardTemplate settingsFromDataTable = new DashboardTemplate(SQL.DecodeString(row1["TemplateName"]), SQL.DecodeString(row1["Description"]));
      settingsFromDataTable.Guid = SQL.DecodeString(row1["Guid"]);
      settingsFromDataTable.VersionNumber = SQL.DecodeString(row1["VersionNumber"]);
      settingsFromDataTable.IsNewTemplate = SQL.DecodeBoolean(row1["IsNewTemplate"]);
      settingsFromDataTable.IsValidTemplate = SQL.DecodeBoolean(row1["IsValidTemplate"]);
      settingsFromDataTable.IsPredefinedTemplate = SQL.DecodeBoolean(row1["IsPredefinedTemplate"]);
      settingsFromDataTable.DataSourceType = SQL.DecodeEnum<DashboardDataSourceType>(row1["DataSourceType"]);
      settingsFromDataTable.ChartType = SQL.DecodeEnum<DashboardChartType>(row1["ChartType"]);
      settingsFromDataTable.DrillDownTemplate = SQL.DecodeString(row1["DrillDownTemplate"]);
      settingsFromDataTable.MaxBars = SQL.DecodeInt(row1["MaxBars"]);
      settingsFromDataTable.SubsetType = SQL.DecodeEnum<DashboardSubsetType>(row1["SubsetType"]);
      settingsFromDataTable.TimeFrameField = SQL.DecodeString(row1["TimeFrameField"]);
      settingsFromDataTable.XAxisField = SQL.DecodeString(row1["XAxisField"]);
      settingsFromDataTable.YAxisField = SQL.DecodeString(row1["YAxisField"]);
      settingsFromDataTable.YAxisSummaryType = SQL.DecodeEnum<ColumnSummaryType>(row1["YAxisSummaryType"]);
      settingsFromDataTable.MaxLines = SQL.DecodeInt(row1["MaxLines"]);
      settingsFromDataTable.GroupByField = SQL.DecodeString(row1["GroupByField"]);
      settingsFromDataTable.MaxRows = SQL.DecodeInt(row1["MaxRows"]);
      settingsFromDataTable.RoleId = SQL.DecodeInt(row1["RoleID"]);
      settingsFromDataTable.OrganizationId = SQL.DecodeInt(row1["OrganizationID"]);
      settingsFromDataTable.IncludeChildren = SQL.DecodeBoolean(row1["IncludeChildren"]);
      settingsFromDataTable.UserGroupId = SQL.DecodeInt(row1["UserGroupId"]);
      settingsFromDataTable.SummaryField1 = SQL.DecodeString(row1["SummaryField1"]);
      settingsFromDataTable.SummaryField2 = SQL.DecodeString(row1["SummaryField2"]);
      settingsFromDataTable.SummaryField3 = SQL.DecodeString(row1["SummaryField3"]);
      settingsFromDataTable.SummaryType1 = SQL.DecodeEnum<ColumnSummaryType>(row1["SummaryType1"]);
      settingsFromDataTable.SummaryType2 = SQL.DecodeEnum<ColumnSummaryType>(row1["SummaryType2"]);
      settingsFromDataTable.SummaryType3 = SQL.DecodeEnum<ColumnSummaryType>(row1["SummaryType3"]);
      settingsFromDataTable.IncludeMin = SQL.DecodeBoolean(row1["IncludeMin"]);
      settingsFromDataTable.IncludeMax = SQL.DecodeBoolean(row1["IncludeMax"]);
      settingsFromDataTable.IncludeAverage = SQL.DecodeBoolean(row1["IncludeAverage"]);
      settingsFromDataTable.IncludeTotal = SQL.DecodeBoolean(row1["IncludeTotal"]);
      if (dsDashboardSnapshotTemplate.Tables.Count > 1)
      {
        List<string> stringList = new List<string>();
        foreach (DataRow row2 in (InternalDataCollectionBase) dsDashboardSnapshotTemplate.Tables[1].Rows)
          stringList.Add(SQL.DecodeString(row2["FolderName"]));
        settingsFromDataTable.Folders = stringList;
      }
      if (dsDashboardSnapshotTemplate.Tables.Count > 2)
      {
        FieldFilterList fieldFilterList = new FieldFilterList();
        foreach (DataRow row3 in (InternalDataCollectionBase) dsDashboardSnapshotTemplate.Tables[2].Rows)
          fieldFilterList.Add(new FieldFilter()
          {
            FieldType = SQL.DecodeEnum<FieldTypes>(row3["FieldType"]),
            FieldID = SQL.DecodeString(row3["FieldID"]),
            CriterionName = SQL.DecodeString(row3["CriterionName"]),
            FieldDescription = SQL.DecodeString(row3["FieldDescription"]),
            OperatorType = SQL.DecodeEnum<OperatorTypes>(row3["OperatorType"]),
            ValueFrom = SQL.DecodeString(row3["ValueFrom"]),
            ValueTo = SQL.DecodeString(row3["ValueTo"]),
            JointToken = SQL.DecodeEnum<JointTokens>(row3["JointToken"]),
            LeftParentheses = SQL.DecodeInt(row3["LeftParentheses"]),
            RightParentheses = SQL.DecodeInt(row3["RightParentheses"]),
            ValueDescription = SQL.DecodeString(row3["ValueDescription"]),
            IsVolatile = SQL.DecodeBoolean(row3["Volatile"]),
            ForceDataConversion = SQL.DecodeBoolean(row3["ForceDataConversion"]),
            DataSource = SQL.DecodeEnum<FilterDataSource>(row3["DataSource"])
          });
        settingsFromDataTable.Filters = fieldFilterList;
      }
      if (dsDashboardSnapshotTemplate.Tables.Count > 3)
      {
        List<EllieMae.EMLite.ClientServer.Reporting.ColumnInfo> columnInfoList = new List<EllieMae.EMLite.ClientServer.Reporting.ColumnInfo>();
        foreach (DataRow row4 in (InternalDataCollectionBase) dsDashboardSnapshotTemplate.Tables[3].Rows)
          columnInfoList.Add(new EllieMae.EMLite.ClientServer.Reporting.ColumnInfo()
          {
            VersionNumber = SQL.DecodeString(row4["VersionNumber"]),
            Description = SQL.DecodeString(row4["Desc"]),
            ID = SQL.DecodeString(row4["ID"]),
            SortOrder = SQL.DecodeEnum<ColumnSortOrder>(row4["SortOrder"]),
            SummaryType = SQL.DecodeEnum<ColumnSummaryType>(row4["SummaryType"]),
            DecimalPlaces = SQL.DecodeInt(row4["DecimalPlaces"]),
            CriterionName = SQL.DecodeString(row4["CriterionName"]),
            ComortPair = SQL.DecodeInt(row4["ComortPair"]),
            IsExcelField = SQL.DecodeBoolean(row4["IsExcelField"]),
            ExcelFormula = SQL.DecodeString(row4["ExcelFormula"]),
            Format = SQL.DecodeEnum<FieldFormat>(row4["FieldFormat"])
          });
        settingsFromDataTable.Fields = columnInfoList;
      }
      return (BinaryObject) (BinaryConvertibleObject) settingsFromDataTable;
    }

    private static void populateDashboardSnapshotTemplateDatabase(FileSystemEntry[] entries)
    {
      DbQueryBuilder sql = new DbQueryBuilder();
      sql.Declare("@DashboardTemplateID", "INT");
      int length = entries.Length;
      int num = 0;
      for (int index = 0; index < entries.Length; ++index)
      {
        if (!DashboardAccessor.existsDashboardSnapshotTemplateSettings(entries[index], true, true))
        {
          if (entries[index].Type == FileSystemEntry.Types.Folder)
          {
            sql.AppendLine(DashboardAccessor.generateDashboardSnapshotTemplateFolderScript(entries[index]).ToString());
          }
          else
          {
            using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(TemplateSettingsType.DashboardTemplate, entries[index]))
            {
              if (latestVersion.Exists)
                sql.AppendLine(DashboardAccessor.generateDashboardSnapshotTemplateScript(entries[index], latestVersion.Data, false).ToString());
              else
                continue;
            }
          }
          ++num;
          if (num == 10)
          {
            DashboardAccessor.executeDashboardSQL(sql);
            sql.Reset();
            sql.Declare("@DashboardTemplateID", "INT");
            Thread.Sleep(10);
            num = 0;
          }
        }
      }
      DashboardAccessor.executeDashboardSQL(sql);
    }

    private static DbQueryBuilder generateDashboardSnapshotTemplateScript(
      FileSystemEntry entry,
      BinaryObject data,
      bool isUpdate)
    {
      if (entry.Type == FileSystemEntry.Types.Folder)
        return DashboardAccessor.generateDashboardSnapshotTemplateFolderScript(entry);
      DbQueryBuilder snapshotTemplateScript = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("DashboardTemplate");
      DbValueList values = new DbValueList();
      DashboardTemplate dashboardTemplate = (DashboardTemplate) data;
      values.Add("TemplateName", (object) dashboardTemplate.TemplateName);
      values.Add("TemplatePath", (object) entry.ToString());
      values.Add("Description", (object) dashboardTemplate.Description);
      values.Add("VersionNumber", (object) dashboardTemplate.VersionNumber);
      values.Add("IsNewTemplate", (object) SQL.EncodeFlag(dashboardTemplate.IsNewTemplate));
      values.Add("IsValidTemplate", (object) SQL.EncodeFlag(dashboardTemplate.IsValidTemplate));
      values.Add("IsPredefinedTemplate", (object) SQL.EncodeFlag(dashboardTemplate.IsPredefinedTemplate));
      values.Add("DataSourceType", (object) dashboardTemplate.DataSourceType.ToString());
      values.Add("ChartType", (object) dashboardTemplate.ChartType.ToString());
      values.Add("DrillDownTemplate", (object) dashboardTemplate.DrillDownTemplate);
      values.Add("MaxBars", (object) dashboardTemplate.MaxBars);
      values.Add("SubsetType", (object) dashboardTemplate.SubsetType.ToString());
      values.Add("TimeFrameField", (object) dashboardTemplate.TimeFrameField);
      values.Add("XAxisField", (object) dashboardTemplate.XAxisField);
      values.Add("YAxisField", (object) dashboardTemplate.YAxisField);
      values.Add("YAxisSummaryType", (object) dashboardTemplate.YAxisSummaryType.ToString());
      values.Add("MaxLines", (object) dashboardTemplate.MaxLines);
      values.Add("GroupByField", (object) dashboardTemplate.GroupByField);
      values.Add("MaxRows", (object) dashboardTemplate.MaxRows);
      values.Add("RoleID", (object) dashboardTemplate.RoleId);
      values.Add("OrganizationID", (object) dashboardTemplate.OrganizationId);
      values.Add("IncludeChildren", (object) SQL.EncodeFlag(dashboardTemplate.IncludeChildren));
      values.Add("UserGroupID", (object) dashboardTemplate.UserGroupId);
      values.Add("SummaryField1", (object) dashboardTemplate.SummaryField1);
      values.Add("SummaryField2", (object) dashboardTemplate.SummaryField2);
      values.Add("SummaryField3", (object) dashboardTemplate.SummaryField3);
      values.Add("SummaryType1", (object) dashboardTemplate.SummaryType1.ToString());
      values.Add("SummaryType2", (object) dashboardTemplate.SummaryType2.ToString());
      values.Add("SummaryType3", (object) dashboardTemplate.SummaryType3.ToString());
      values.Add("IncludeMin", (object) SQL.EncodeFlag(dashboardTemplate.IncludeMin));
      values.Add("IncludeMax", (object) SQL.EncodeFlag(dashboardTemplate.IncludeMax));
      values.Add("IncludeAverage", (object) SQL.EncodeFlag(dashboardTemplate.IncludeAverage));
      values.Add("IncludeTotal", (object) SQL.EncodeFlag(dashboardTemplate.IncludeTotal));
      values.Add("IsFolder", (object) SQL.EncodeFlag(false));
      DbValue dbValue = new DbValue("Guid", (object) dashboardTemplate.Guid);
      DbValueList keys = new DbValueList();
      keys.Add("Guid", (object) dashboardTemplate.Guid);
      keys.Add("TemplatePath", (object) entry.ToString());
      if (isUpdate)
      {
        snapshotTemplateScript.AppendLine(string.Format("SELECT @DashboardTemplateID = DashboardTemplateID FROM {0} WHERE [Guid] = {1} and [TemplatePath] = {2}", (object) "DashboardTemplate", (object) SQL.EncodeString(dashboardTemplate.Guid), (object) SQL.EncodeString(entry.ToString())));
        snapshotTemplateScript.Update(table, values, keys);
      }
      else
      {
        values.Add(dbValue);
        snapshotTemplateScript.InsertInto(table, values, true, false);
        snapshotTemplateScript.SelectIdentity("@DashboardTemplateID");
      }
      snapshotTemplateScript.AppendLine(string.Format("DELETE FROM {0} WHERE DashboardTemplateID = @DashboardTemplateID", (object) "DashboardTemplateFolder"));
      if (dashboardTemplate.Folders.Count > 0)
      {
        for (int index = 0; index < dashboardTemplate.Folders.Count; ++index)
        {
          snapshotTemplateScript.AppendLine(string.Format("INSERT INTO {0} ([DashboardTemplateID], [FolderName])", (object) "DashboardTemplateFolder"));
          snapshotTemplateScript.AppendLine(string.Format("VALUES(@DashboardTemplateID, {0})", (object) SQL.EncodeString(dashboardTemplate.Folders[index])));
        }
      }
      snapshotTemplateScript.AppendLine(string.Format("DELETE FROM {0} WHERE DashboardTemplateID = @DashboardTemplateID", (object) "DashboardTemplateFilter"));
      if (dashboardTemplate.Filters.Count > 0)
      {
        for (int index = 0; index < dashboardTemplate.Filters.Count; ++index)
        {
          FieldFilter filter = dashboardTemplate.Filters[index];
          snapshotTemplateScript.AppendLine(string.Format("INSERT INTO {0} ([DashboardTemplateID], [FieldType], [FieldID], [CriterionName], [FieldDescription], [OperatorType], [ValueFrom], [ValueTo], [JointToken], [LeftParentheses], [RightParentheses], [ValueDescription], [Volatile], [ForceDataConversion], [DataSource])", (object) "DashboardTemplateFilter"));
          snapshotTemplateScript.AppendLine(string.Format("VALUES(@DashboardTemplateID, {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13})", (object) SQL.EncodeString(filter.FieldType.ToString()), (object) SQL.EncodeString(filter.FieldID), (object) SQL.EncodeString(filter.CriterionName), (object) SQL.EncodeString(filter.FieldDescription), (object) SQL.EncodeString(filter.OperatorType.ToString()), (object) SQL.EncodeString(filter.ValueFrom), (object) SQL.EncodeString(filter.ValueTo), (object) SQL.EncodeString(filter.JointToken.ToString()), (object) filter.LeftParentheses, (object) filter.RightParentheses, (object) SQL.EncodeString(filter.ValueDescription), (object) SQL.EncodeFlag(filter.IsVolatile), (object) SQL.EncodeFlag(filter.ForceDataConversion), (object) SQL.EncodeString(filter.DataSource.ToString())));
        }
      }
      snapshotTemplateScript.AppendLine(string.Format("DELETE FROM {0} WHERE DashboardTemplateID = @DashboardTemplateID", (object) "DashboardTemplateField"));
      if (dashboardTemplate.Fields.Count > 0)
      {
        for (int index = 0; index < dashboardTemplate.Fields.Count; ++index)
        {
          EllieMae.EMLite.ClientServer.Reporting.ColumnInfo field = dashboardTemplate.Fields[index];
          snapshotTemplateScript.AppendLine(string.Format("INSERT INTO {0} ([DashboardTemplateID], [VersionNumber], [Desc], [ID], [SortOrder], [SummaryType], [DecimalPlaces], [CriterionName], [ComortPair], [IsExcelField], [ExcelFormula], [FieldFormat])", (object) "DashboardTemplateField"));
          snapshotTemplateScript.AppendLine(string.Format("VALUES(@DashboardTemplateID, {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})", (object) SQL.EncodeString(field.VersionNumber), (object) SQL.EncodeString(field.Description), (object) SQL.EncodeString(field.ID), (object) SQL.EncodeString(field.SortOrder.ToString()), (object) SQL.EncodeString(field.SummaryType.ToString()), (object) field.DecimalPlaces, (object) SQL.EncodeString(field.CriterionName), (object) field.ComortPair, (object) SQL.EncodeFlag(field.IsExcelField), (object) SQL.EncodeString(field.ExcelFormula), (object) SQL.EncodeString(field.Format.ToString())));
        }
      }
      return snapshotTemplateScript;
    }

    private static DbQueryBuilder generateDashboardSnapshotTemplateFolderScript(
      FileSystemEntry entry)
    {
      DbQueryBuilder templateFolderScript = new DbQueryBuilder();
      templateFolderScript.AppendLine(string.Format("INSERT INTO {0} ([Guid], [TemplateName], [TemplatePath], [IsFolder])", (object) "DashboardTemplate"));
      templateFolderScript.AppendLine(string.Format("VALUES({0}, {1}, {2}, {3})", (object) SQL.EncodeString(Guid.NewGuid().ToString()), (object) SQL.EncodeString(entry.Name), (object) SQL.EncodeString(entry.ToString()), (object) SQL.EncodeFlag(true)));
      return templateFolderScript;
    }

    public static FileSystemEntry[] GetDashboardViewTemplateList(
      FileSystemEntry parentFolder,
      ISession session)
    {
      return DashboardAccessor.GetDashboardViewTemplateList(parentFolder, session.GetUserInfo());
    }

    public static FileSystemEntry[] GetDashboardViewTemplateList(
      FileSystemEntry parentFolder,
      UserInfo userInfo)
    {
      FileSystemEntry[] directoryEntries = TemplateSettingsStore.GetDirectoryEntries(userInfo, TemplateSettingsType.DashboardViewTemplate, parentFolder, FileSystemEntry.Types.All, false, true);
      if (directoryEntries != null && directoryEntries.Length != 0)
        DashboardAccessor.populateDashboardViewTemplateDatabase(directoryEntries);
      return directoryEntries;
    }

    public static bool ExistsDashboardViewTemplateSettings(FileSystemEntry entry)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      bool flag = false;
      dbQueryBuilder.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE TemplatePath = {1}", (object) "DashboardView", (object) SQL.EncodeString(entry.ToString())));
      if (Convert.ToInt32(dbQueryBuilder.ExecuteScalar()) > 0)
        flag = true;
      return flag;
    }

    public static bool IsDashboardViewTemplatePathMissing(FileSystemEntry entry)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      bool flag = false;
      dbQueryBuilder.Append(string.Format("SELECT COUNT(*) FROM {0} WHERE [Guid] = {1} AND ([TemplatePath] IS NULL OR [IsFolder] IS NULL)", (object) "DashboardView", (object) SQL.EncodeString(entry.Properties[(object) "ViewGuid"].ToString())));
      if (Convert.ToInt32(dbQueryBuilder.ExecuteScalar()) > 0)
        flag = true;
      return flag;
    }

    public static bool SaveDashboardViewTemplateFolder(FileSystemEntry entry)
    {
      if (entry.Type != FileSystemEntry.Types.Folder)
        return false;
      DashboardAccessor.executeDashboardSQL(DashboardAccessor.generateDashboardViewTemplateScript(entry));
      return true;
    }

    public static bool SaveDashboardViewTemplateFile(FileSystemEntry entry, bool isUpdate)
    {
      if (entry.Type != FileSystemEntry.Types.File && !isUpdate)
        return false;
      DashboardAccessor.executeDashboardSQL(DashboardAccessor.generateDashboardViewTemplateScript(entry));
      return true;
    }

    public static void DeleteDashboardViewTemplate(FileSystemEntry entry)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("DELETE FROM {0} WHERE [TemplatePath] = {1}", (object) "DashboardView", (object) SQL.EncodeString(entry.ToString())));
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    public static void RenameDashboardViewTemplate(FileSystemEntry source, FileSystemEntry target)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (target.Type == FileSystemEntry.Types.Folder)
      {
        string str1 = DashboardAccessor.regenerateGUID(new FileSystemEntry(target.Path, source.Name, FileSystemEntry.Types.File, target.Owner), TemplateSettingsType.DashboardViewTemplate);
        string str2 = source.ToString();
        if (source.Type == FileSystemEntry.Types.File)
          str2 = str2.Remove(str2.LastIndexOf("\\") + 1);
        dbQueryBuilder.AppendLine(string.Format("UPDATE {0} SET [Guid] = {4}, [TemplatePath] = REPLACE([TemplatePath], {3}, {2}) WHERE [TemplatePath] = {1}", (object) "DashboardView", (object) SQL.EncodeString(source.ToString()), (object) SQL.EncodeString(target.ToString()), (object) SQL.EncodeString(str2), (object) SQL.EncodeString(str1)));
      }
      else
      {
        string str = DashboardAccessor.regenerateGUID(target, TemplateSettingsType.DashboardViewTemplate);
        dbQueryBuilder.AppendLine(string.Format("UPDATE {0} SET [Guid] = {4}, [ViewName] = {3}, [TemplatePath] = {2} WHERE [TemplatePath] = {1}", (object) "DashboardView", (object) SQL.EncodeString(source.ToString()), (object) SQL.EncodeString(target.ToString()), (object) SQL.EncodeString(target.Name), (object) SQL.EncodeString(str)));
        target.Properties.Add((object) "ViewName", (object) target.Name);
        target.Properties.Add((object) "ViewGuid", (object) str);
      }
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
    }

    private static void populateDashboardViewTemplateDatabase(FileSystemEntry[] entries)
    {
      DbQueryBuilder sql = new DbQueryBuilder();
      for (int index = 0; index < entries.Length; ++index)
      {
        if (entries[index].Type == FileSystemEntry.Types.Folder)
        {
          if (!DashboardAccessor.ExistsDashboardViewTemplateSettings(entries[index]))
            sql.AppendLine(DashboardAccessor.generateDashboardViewTemplateScript(entries[index]).ToString());
        }
        else if (entries[index].Type == FileSystemEntry.Types.File && DashboardAccessor.IsDashboardViewTemplatePathMissing(entries[index]))
          sql.AppendLine(DashboardAccessor.generateUpdateDashboardViewTemplateFileScript(entries[index]).ToString());
      }
      DashboardAccessor.executeDashboardSQL(sql);
    }

    private static DbQueryBuilder generateDashboardViewTemplateScript(FileSystemEntry entry)
    {
      DbQueryBuilder viewTemplateScript = new DbQueryBuilder();
      string str = (string) null;
      if (entry.Type == FileSystemEntry.Types.File)
      {
        using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(TemplateSettingsType.DashboardViewTemplate, entry))
        {
          if (latestVersion.Exists)
            str = ((DashboardViewTemplate) latestVersion.Data).ViewGuid;
        }
      }
      else
        str = Guid.NewGuid().ToString();
      viewTemplateScript.AppendLine(string.Format("INSERT INTO {0} ([Guid], [ViewName], [TemplatePath], [IsFolder])", (object) "DashboardView"));
      viewTemplateScript.AppendLine(string.Format("VALUES({0}, {1}, {2}, {3})", (object) SQL.EncodeString(str), (object) SQL.EncodeString(entry.Name), (object) SQL.EncodeString(entry.ToString()), (object) SQL.EncodeFlag(entry.Type != FileSystemEntry.Types.File)));
      return viewTemplateScript;
    }

    private static DbQueryBuilder generateUpdateDashboardViewTemplateFileScript(
      FileSystemEntry entry)
    {
      DbQueryBuilder templateFileScript = new DbQueryBuilder();
      templateFileScript.AppendLine(string.Format("UPDATE {0} SET ", (object) "DashboardView"));
      templateFileScript.AppendLine(string.Format(" [TemplatePath] = {0}, [IsFolder] = {1} ", (object) SQL.EncodeString(entry.ToString()), (object) SQL.EncodeFlag(false)));
      templateFileScript.AppendLine(string.Format(" WHERE [Guid] = {0}", (object) SQL.EncodeString(entry.Properties[(object) "ViewGuid"].ToString())));
      return templateFileScript;
    }

    private static string regenerateGUID(FileSystemEntry entry, TemplateSettingsType templateType)
    {
      string str = Guid.NewGuid().ToString();
      using (TemplateSettings templateSettings = TemplateSettingsStore.CheckOut(templateType, entry))
      {
        if (templateSettings.Exists)
        {
          BinaryObject data1 = (BinaryObject) null;
          switch (templateType)
          {
            case TemplateSettingsType.DashboardTemplate:
              DashboardTemplate data2 = (DashboardTemplate) templateSettings.Data;
              data2.Guid = str;
              data1 = (BinaryObject) (BinaryConvertibleObject) data2;
              break;
            case TemplateSettingsType.DashboardViewTemplate:
              DashboardViewTemplate data3 = (DashboardViewTemplate) templateSettings.Data;
              data3.ViewGuid = str;
              data1 = (BinaryObject) (BinaryConvertibleObject) data3;
              break;
          }
          templateSettings.CheckIn(data1);
        }
      }
      return str;
    }

    private static bool executeDashboardSQL(DbQueryBuilder sql)
    {
      if (string.IsNullOrEmpty(sql.ToString()))
        return false;
      sql.ExecuteNonQuery(DbTransactionType.Default);
      return true;
    }

    private static bool hasData(DataTable source) => source == null || source.Rows.Count == 0;
  }
}
