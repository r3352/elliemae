// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.PipelineViewXmlToDbMigrationManager
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public class PipelineViewXmlToDbMigrationManager
  {
    private const string className = "PipelineViewXmlToDbMigrationManager�";

    public static void MigratePipelineViewFromXmlToDB(string instanceName)
    {
      ClientContext clientContext = ClientContext.Open(instanceName, false);
      using (clientContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
      {
        EnGlobalSettings enGlobalSettings = new EnGlobalSettings(instanceName);
        if (!enGlobalSettings.Exists())
          Err.Raise(nameof (PipelineViewXmlToDbMigrationManager), new ServerException("The specified instance name is not valid"));
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        TraceLog.WriteWarning(nameof (PipelineViewXmlToDbMigrationManager), "Start Migrating User Pipeline View Xml To DB For instance : " + instanceName);
        Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
        foreach (UserInfo allUser in User.GetAllUsers((string) null))
          insensitiveHashtable.Add((object) allUser.Userid, (object) null);
        TraceLog.WriteVerbose(nameof (PipelineViewXmlToDbMigrationManager), "Getting list of all users");
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("Select Distinct userid,name from Acl_UserPipelineViews");
        DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
        Dictionary<string, int> dictionary1 = new Dictionary<string, int>();
        if (dataTable.Rows.Count > 0)
        {
          int num = 0;
          foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
            dictionary1.Add(row["userid"].ToString() + "_" + row["name"].ToString().ToLower(), num++);
        }
        string str1 = Path.Combine(enGlobalSettings.AppDataDirectory, "Users");
        TraceLog.WriteVerbose(nameof (PipelineViewXmlToDbMigrationManager), "Path for migration : " + enGlobalSettings.AppDataDirectory);
        Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
        Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
        int num1 = 0;
        string path = string.Empty;
        string key = string.Empty;
        foreach (string enumerateDirectory in Directory.EnumerateDirectories(str1))
        {
          TemplateSettings templateSettings = (TemplateSettings) null;
          string str2 = enumerateDirectory + "\\TemplateSettings\\PipelineView";
          TraceLog.WriteVerbose(nameof (PipelineViewXmlToDbMigrationManager), "searchPath : " + str2);
          if (Directory.Exists(str2))
          {
            foreach (string enumerateFile in Directory.EnumerateFiles(str2))
            {
              try
              {
                path = Path.Combine(str1, str2, enumerateFile);
                ++num1;
                string str3 = enumerateDirectory.Substring(enumerateDirectory.LastIndexOf("\\") + 1);
                key = str3 + "-" + Path.GetFileName(path);
                FileSystemEntry entry = new FileSystemEntry("\\", FileSystem.DecodeFilename(Path.GetFileNameWithoutExtension(path)), FileSystemEntry.Types.File, str3);
                using (clientContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
                {
                  templateSettings = TemplateSettingsStore.GetLatestVersion(TemplateSettingsType.PipelineView, entry);
                  UserPipelineView data = (UserPipelineView) templateSettings?.Data;
                  if (insensitiveHashtable.ContainsKey((object) str3))
                  {
                    if (dictionary1.ContainsKey(str3 + "_" + data.Name.ToLower()) || PipelineViewXmlToDbMigrationManager.migratePipelineViewDB(data, str3))
                    {
                      lock (dictionary2)
                      {
                        if (!dictionary2.ContainsKey(key))
                          dictionary2.Add(key, path);
                        else
                          TraceLog.WriteWarning(nameof (PipelineViewXmlToDbMigrationManager), key + " File Exist");
                      }
                    }
                    else if (!dictionary3.ContainsKey(key))
                      dictionary3.Add(key, path);
                  }
                  else
                  {
                    TraceLog.WriteVerbose(nameof (PipelineViewXmlToDbMigrationManager), "User Not Found or User Can't have Pipeline View : " + str3);
                    lock (dictionary2)
                    {
                      if (!dictionary2.ContainsKey(key))
                        dictionary2.Add(key, path);
                      else
                        TraceLog.WriteWarning(nameof (PipelineViewXmlToDbMigrationManager), key + " File Exist for user");
                    }
                  }
                }
              }
              catch (Exception ex)
              {
                TraceLog.WriteError(nameof (PipelineViewXmlToDbMigrationManager), "Error migrating Pipeline XML file: " + enumerateFile + " with Error : " + ex.StackTrace);
                if (!dictionary3.ContainsKey(key))
                  dictionary3.Add(key, path);
              }
              finally
              {
                templateSettings?.Dispose();
              }
            }
          }
        }
        try
        {
          TraceLog.WriteWarning(nameof (PipelineViewXmlToDbMigrationManager), "Migrating User Pipeline View Xml completes all files For instance : " + instanceName);
          foreach (KeyValuePair<string, string> keyValuePair in dictionary3)
            PipelineViewXmlToDbMigrationManager.Movefiletobackup_PipelineView(keyValuePair.Value, keyValuePair.Key, "PipelineView_bkp_Error");
          Company.SetCompanySetting("MIGRATION", "MigrateUserPipelineView", "1");
          Company.SetCompanySetting("MIGRATION", "MigrationViewInProcess", "0");
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (PipelineViewXmlToDbMigrationManager), "Error updating flag MigrateUserPipelineView while running Migrating User Pipeline View Xml To DB : " + ex.Message);
          Err.Reraise(nameof (PipelineViewXmlToDbMigrationManager), ex);
        }
        TraceLog.WriteWarning(nameof (PipelineViewXmlToDbMigrationManager), "Completed Migrating User Pipeline View Xml To DB For instance : " + instanceName);
      }
    }

    private static bool migratePipelineViewDB(UserPipelineView userPipelineView, string userid)
    {
      if (userPipelineView == null)
        return false;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        string str1 = userPipelineView.OrgType.ToString();
        string externalOrgId = userPipelineView.ExternalOrgId;
        string str2 = string.IsNullOrEmpty(userPipelineView.LoanFolders) ? string.Empty : userPipelineView.LoanFolders;
        if (string.IsNullOrEmpty(str1) || str1 == "None")
          str1 = "Internal";
        dbQueryBuilder.AppendLine("Declare @viewID  int");
        dbQueryBuilder.AppendLineFormat("Insert into [Acl_UserPipelineViews] ([userid],[name],[loanFolders],[filterXml],[ownership],[orgType],[externalOrgId], [CreatedDate], [CreatedBy], [LastModifiedDate], [LastModifiedBy]) VALUES ({0},{1},{2},{3},{4},{5},{6}, {7}, {8}, {9}, {10})", (object) PipelineViewXmlToDbMigrationManager.EncodeString(userid), (object) PipelineViewXmlToDbMigrationManager.EncodeString(userPipelineView.Name), (object) PipelineViewXmlToDbMigrationManager.EncodeString(str2), (object) PipelineViewXmlToDbMigrationManager.EncodeString(userPipelineView.HasFilter ? userPipelineView.Filter?.ToXML() : string.Empty), (object) PipelineViewXmlToDbMigrationManager.EncodeString(userPipelineView.Ownership.ToString()), (object) PipelineViewXmlToDbMigrationManager.EncodeString(str1), (object) PipelineViewXmlToDbMigrationManager.EncodeString(string.IsNullOrEmpty(userPipelineView.ExternalOrgId) ? "0" : externalOrgId), (object) SQL.EncodeDateTime(DateTime.UtcNow), (object) SQL.EncodeString("<system>"), (object) SQL.EncodeDateTime(DateTime.UtcNow), (object) SQL.EncodeString("<system>"));
        dbQueryBuilder.AppendLine("Select @viewID = @@IDENTITY");
        foreach (UserPipelineViewColumn column in userPipelineView.Columns)
          dbQueryBuilder.AppendLineFormat("INSERT INTO [Acl_UserPipelineView_Columns]([viewID],[fieldDBName],[width],[orderIndex],[sortOrder],[sortPriority],[alignment],[isRequired]) VALUES(@viewID,{0},{1},{2},{3},{4},{5},{6})", (object) PipelineViewXmlToDbMigrationManager.EncodeString(column.ColumnDBName), (object) column.Width, (object) column.OrderIndex, (object) PipelineViewXmlToDbMigrationManager.EncodeString(column.SortOrder.ToString()), (object) column.SortPriority, (object) PipelineViewXmlToDbMigrationManager.EncodeString(column.Alignment), (object) (column.IsRequired ? 1 : 0));
        dbQueryBuilder.ExecuteNonQuery();
        return true;
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (PipelineViewXmlToDbMigrationManager), "Error creating migratePipelineView SQL table: " + ex.Message + ". Skipping " + userPipelineView.Name + " xml file.");
      }
      TraceLog.WriteVerbose(nameof (PipelineViewXmlToDbMigrationManager), "Return false");
      return false;
    }

    public static string EncodeString(string value)
    {
      return value == null ? "NULL" : "'" + value.Replace("'", "''") + "'";
    }

    private static void Movefiletobackup_PipelineView(
      string file,
      string DestFileName,
      string backupfolder)
    {
      try
      {
        if (file.IndexOf("PipelineView") <= 0)
          return;
        string[] strArray = DestFileName.Split('-');
        string path = file.Substring(0, file.IndexOf("Users")) + backupfolder + "\\" + strArray[0];
        int startIndex = file.LastIndexOf("\\");
        if (startIndex < 0)
          return;
        file.Substring(startIndex);
        TraceLog.WriteVerbose(nameof (PipelineViewXmlToDbMigrationManager), "File = " + file + ", DestFilename = " + strArray[1] + ", targetrootpath = " + path);
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        File.Copy(file, path + "\\" + strArray[1], true);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (PipelineViewXmlToDbMigrationManager), "Error updating flag MigrateUserPipelineView while running Migrating User Pipeline View Xml To DB : " + ex.Message);
      }
    }
  }
}
