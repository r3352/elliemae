// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.TemplateFilesToDBMigrationManager
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public class TemplateFilesToDBMigrationManager
  {
    private const string className = "TemplateFilesToDBMigrationManager�";

    public static bool MigrateEFolderViewsFromFilesToDB(
      string instanceName,
      TemplateSettingsType templateSettingsType)
    {
      try
      {
        using (ClientContext.Open(instanceName, false).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        {
          TemplateSettingsType templateSettingsType1 = TemplateSettingsType.DocumentTrackingView;
          if (Company.GetCompanySetting("MIGRATION", templateSettingsType1.ToString()).ToLower() == "true")
          {
            TraceLog.WriteWarning(nameof (TemplateFilesToDBMigrationManager), string.Format("******** Error: view {0} migration from files to Db is already completed for instance {1}. ********", (object) templateSettingsType, (object) instanceName));
            return true;
          }
          using (MSSQLAppDistributedLock appDistributedLock = new MSSQLAppDistributedLock(instanceName + "_" + (object) templateSettingsType, ClientContext.GetCurrent().Settings.ConnectionString))
          {
            int num = appDistributedLock.TakeLock(10);
            if (num == -1)
            {
              TraceLog.WriteWarning(nameof (TemplateFilesToDBMigrationManager), string.Format("******** Error: view {0} migration is already running on instance {1}. ********", (object) templateSettingsType, (object) instanceName));
              return false;
            }
            if (num < 0)
            {
              TraceLog.WriteError(nameof (TemplateFilesToDBMigrationManager), string.Format("******** Error: view {0} migration failed with errorcode {1}. ********", (object) templateSettingsType, (object) num));
              return false;
            }
            templateSettingsType1 = TemplateSettingsType.DocumentTrackingView;
            if (Company.GetCompanySetting("MIGRATION", templateSettingsType1.ToString()).ToLower() == "true")
            {
              TraceLog.WriteWarning(nameof (TemplateFilesToDBMigrationManager), string.Format("******** Error: view {0} migration from Files to DB migration is already completed for instance {1}. ********", (object) templateSettingsType, (object) instanceName));
              return true;
            }
            if (!TemplateFilesToDBMigrationManager.migrateEFolderViewsFromFilesToDB(instanceName, templateSettingsType))
              return false;
            templateSettingsType1 = TemplateSettingsType.DocumentTrackingView;
            Company.SetCompanySetting("MIGRATION", templateSettingsType1.ToString(), "true");
            return true;
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (TemplateFilesToDBMigrationManager), ex);
        return false;
      }
    }

    private static bool migrateEFolderViewsFromFilesToDB(
      string instanceName,
      TemplateSettingsType templateSettingsType)
    {
      ClientContext current = ClientContext.GetCurrent();
      IDataCache requestCache = ClientContext.CurrentRequest?.RequestCache;
      string correlationId = ClientContext.CurrentRequest?.CorrelationId;
      Guid? transactionId = (Guid?) ClientContext.CurrentRequest?.TransactionId;
      try
      {
        TraceLog.WriteWarning(nameof (TemplateFilesToDBMigrationManager), string.Format("Started migrating views {0} from Xml To Db for instance {1} at:{2} ", (object) templateSettingsType, (object) instanceName, (object) DateTime.Now.ToString()));
        EnGlobalSettings enGlobalSettings = new EnGlobalSettings(instanceName);
        if (!enGlobalSettings.Exists())
          Err.Raise(nameof (TemplateFilesToDBMigrationManager), new ServerException("The specified instance name is not valid"));
        string str1 = Path.Combine(enGlobalSettings.AppDataDirectory, "Users");
        TraceLog.WriteWarning(nameof (TemplateFilesToDBMigrationManager), "Path for view migration : " + str1);
        if (!Directory.Exists(str1))
        {
          TraceLog.WriteWarning(nameof (TemplateFilesToDBMigrationManager), "Directory not found : " + str1);
          return true;
        }
        if (!TemplateFilesToDBMigrationManager.IsTemplateTypeSupported(templateSettingsType))
        {
          TraceLog.WriteError(nameof (TemplateFilesToDBMigrationManager), string.Format("TemplateSettingsType {0} is currently not supported to migrate from Files to DB", (object) templateSettingsType));
          return false;
        }
        DataTable trackViewDataTable = TemplateFilesToDBMigrationManager.getDocTrackViewDataTable();
        List<string> userList = ((IEnumerable<UserInfo>) User.GetAllUsers((string) null)).Select<UserInfo, string>((System.Func<UserInfo, string>) (x => x.Userid)).ToList<string>();
        foreach (string str2 in ((IEnumerable<string>) Directory.GetDirectories(str1)).Select<string, string>(new System.Func<string, string>(Path.GetFileName)).Where<string>((System.Func<string, bool>) (f => userList.Contains(f))))
        {
          StringBuilder stringBuilder = new StringBuilder();
          string path1 = Path.Combine(str1, str2, "TemplateSettings", templateSettingsType.ToString());
          if (Directory.Exists(path1))
          {
            TemplateSettings templateSettings = (TemplateSettings) null;
            IEnumerable<string> source = Directory.EnumerateFiles(path1);
            stringBuilder.AppendFormat("Total views found for user {0} of type: {1} is {2}", (object) str2, (object) templateSettingsType, (object) (source != null ? new int?(source.Count<string>()) : new int?())).AppendLine();
            HashSet<string> stringSet = new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
            foreach (string path2 in source)
            {
              string str3 = FileSystem.DecodeFilename(Path.GetFileNameWithoutExtension(path2));
              FileSystemEntry entry = new FileSystemEntry("\\", str3, FileSystemEntry.Types.File, str2);
              try
              {
                using (current.MakeCurrent(requestCache, correlationId, transactionId, new bool?()))
                  templateSettings = TemplateSettingsStore.GetLatestVersion(templateSettingsType, entry);
                DocumentTrackingView data = (DocumentTrackingView) templateSettings?.Data;
                if (!FileSystem.IsValidName(str3) || !FileSystem.IsValidName(data?.Name))
                  stringBuilder.AppendFormat("view {0} is ignored for user {1} of type: {2} because The specified view name is invalid. The name must be non-empty and cannot contain the backslash (\\) character ", (object) data?.Name, (object) str2, (object) templateSettingsType).AppendLine();
                else if (data != null && data.Name.Length > 100 || str3.Length > 100)
                  stringBuilder.AppendFormat("view {0} is ignored for user {1} of type: {2} because Name is > 100 characters ", (object) data?.Name, (object) str2, (object) templateSettingsType).AppendLine();
                else if (!stringSet.Add(data?.Name?.Trim()))
                {
                  stringBuilder.AppendFormat("view {0} is ignored for user {1} of type: {2} because view with same name is already exist", (object) data?.Name, (object) str2, (object) templateSettingsType).AppendLine();
                }
                else
                {
                  DataRow dataRow = TemplateFilesToDBMigrationManager.MapTemplateDataToDataRow(data, trackViewDataTable.NewRow(), str2);
                  trackViewDataTable?.Rows.Add(dataRow);
                }
                current.Cache.Remove("TemplateSettingsStore_" + templateSettings.Identity.PhysicalPath);
              }
              catch (Exception ex)
              {
                if (!(ex is NullReferenceException) && (!(ex is ServerException) || !(ex.Message.ToLower() == "object does not exist")) && !(ex is XmlException))
                  throw ex;
                stringBuilder.AppendFormat("Invalid view {0} of type {1} of user {2} on instance {3} with exception {4} :", (object) path2, (object) templateSettingsType, (object) str2, (object) instanceName, (object) ex.Message).AppendLine();
              }
              finally
              {
                templateSettings?.Dispose();
              }
            }
            stringBuilder.AppendFormat("Total valid views found for user {0} of type: {1} is {2}", (object) str2, (object) templateSettingsType, (object) stringSet.Count);
            TraceLog.WriteWarning(nameof (TemplateFilesToDBMigrationManager), stringBuilder.ToString());
          }
        }
        int? nullable1;
        if (trackViewDataTable != null)
        {
          nullable1 = trackViewDataTable.Rows?.Count;
          int num = 0;
          if (nullable1.GetValueOrDefault() > num & nullable1.HasValue)
            TemplateFilesToDBMigrationManager.DoBulkCopyTemplateData(trackViewDataTable);
        }
        string str4 = instanceName;
        // ISSUE: variable of a boxed type
        __Boxed<TemplateSettingsType> local1 = (Enum) templateSettingsType;
        int? nullable2;
        if (trackViewDataTable == null)
        {
          nullable1 = new int?();
          nullable2 = nullable1;
        }
        else
        {
          DataRowCollection rows = trackViewDataTable.Rows;
          if (rows == null)
          {
            nullable1 = new int?();
            nullable2 = nullable1;
          }
          else
            nullable2 = new int?(rows.Count);
        }
        // ISSUE: variable of a boxed type
        __Boxed<int?> local2 = (System.ValueType) nullable2;
        TraceLog.WriteWarning(nameof (TemplateFilesToDBMigrationManager), string.Format("Total views migrated for instance {0} of type: {1} is {2}", (object) str4, (object) local1, (object) local2));
      }
      catch (Exception ex)
      {
        throw ex.InnerException;
      }
      TraceLog.WriteWarning(nameof (TemplateFilesToDBMigrationManager), string.Format("Completed Xml To Db {0} migration for instance {1} at: {2}", (object) templateSettingsType, (object) instanceName, (object) DateTime.Now.ToString()));
      return true;
    }

    private static void DoBulkCopyTemplateData(DataTable templateDataTable)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append("truncate table " + templateDataTable.TableName);
      dbQueryBuilder.ExecuteNonQuery();
      dbQueryBuilder.Reset();
      Dictionary<string, string> dictionary = templateDataTable != null ? templateDataTable.Columns.Cast<DataColumn>().Select(x => new
      {
        ColumnName = x.ColumnName
      }).ToDictionary(t => t.ColumnName, t => t.ColumnName) : (Dictionary<string, string>) null;
      templateDataTable.AcceptChanges();
      dbQueryBuilder.DoBulkCopy(templateDataTable.TableName, templateDataTable, dictionary);
    }

    private static bool IsTemplateTypeSupported(TemplateSettingsType templateSettingsType)
    {
      bool flag = false;
      if (templateSettingsType == TemplateSettingsType.DocumentTrackingView)
        flag = true;
      return flag;
    }

    private static DataRow MapTemplateDataToDataRow(
      DocumentTrackingView template,
      DataRow row,
      string userId)
    {
      row["Name"] = (object) template?.Name?.Trim();
      row["UserId"] = (object) userId;
      row["Filters"] = (object) template?.Filter?.ToXML();
      row["Layouts"] = (object) template?.Layout?.ToXML();
      row["DocumentGroup"] = (object) template?.DocGroup;
      row["StackingOrder"] = (object) template?.StackingOrder;
      row["CreatedBy"] = (object) "<system>";
      return row;
    }

    private static DataTable getDocTrackViewDataTable()
    {
      DataTable trackViewDataTable = new DataTable(EfolderDocTrackViewAccessor.eFolderDocTrackViewTblName);
      DataColumn column1 = new DataColumn("Name", typeof (string));
      DataColumn column2 = new DataColumn("UserId", typeof (string));
      trackViewDataTable.Columns.Add(column1);
      trackViewDataTable.Columns.Add(column2);
      trackViewDataTable.Columns.Add(new DataColumn("Filters", typeof (string)));
      trackViewDataTable.Columns.Add(new DataColumn("Layouts", typeof (string)));
      trackViewDataTable.Columns.Add(new DataColumn("DocumentGroup", typeof (string)));
      trackViewDataTable.Columns.Add(new DataColumn("StackingOrder", typeof (string)));
      trackViewDataTable.Columns.Add(new DataColumn("CreatedBy", typeof (string)));
      UniqueConstraint uniqueConstraint = new UniqueConstraint(new DataColumn[2]
      {
        column1,
        column2
      });
      trackViewDataTable.Constraints.Add((Constraint) uniqueConstraint);
      return trackViewDataTable;
    }
  }
}
