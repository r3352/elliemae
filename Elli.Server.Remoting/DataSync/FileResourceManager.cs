// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.DataSync.FileResourceManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.SessionObjects;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Elli.Server.Remoting.DataSync
{
  public class FileResourceManager : SessionBoundObject, IFileManager
  {
    private const string className = "FileResourceManager";
    private string filePathNodeName = string.Empty;
    private const string AFFILIATED_BUSINESS_ARRANGEMENT_TEMPLATES = "AffiliatedBusinessArrangementsTemplates";

    public FileResourceManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (FileResourceManager).ToLower());
      return this;
    }

    public virtual List<FileSystemSyncRecord> GetAssociatedFileSystemEntities(
      string xmlDataFile,
      TableDef tableDef,
      bool fetchData)
    {
      List<FileSystemSyncRecord> fileSystemEntities = new List<FileSystemSyncRecord>();
      Dictionary<FileSystemEntry, BinaryObject> dictionary = new Dictionary<FileSystemEntry, BinaryObject>();
      string elementXpath = tableDef.GetElementXPath(".");
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xmlDataFile);
      XmlNodeList xmlNodeList = xmlDocument.SelectNodes(elementXpath);
      bool flag1 = tableDef.TableName.Equals("ReportSettings", StringComparison.OrdinalIgnoreCase);
      bool flag2 = tableDef.TableName.Equals("DashboardTemplate", StringComparison.OrdinalIgnoreCase);
      bool flag3 = tableDef.TableName.Equals("DashboardView", StringComparison.OrdinalIgnoreCase);
      bool flag4 = tableDef.TableName.Equals("AffiliatedBusinessArrangementsTemplates", StringComparison.OrdinalIgnoreCase);
      this.filePathNodeName = this.getNodeName(tableDef.TableName);
      foreach (XmlNode xmlNode in xmlNodeList)
      {
        string innerText = xmlNode.SelectSingleNode(this.filePathNodeName).InnerText;
        bool flag5 = xmlNode.SelectSingleNode("IsFolder").InnerText == "true";
        FileResourceManager.FileType type;
        if (flag1)
        {
          type = FileResourceManager.FileType.Reports;
          if (flag5 && !innerText.EndsWith("\\"))
            innerText += "\\";
        }
        else if (flag2)
        {
          type = FileResourceManager.FileType.DashboardTemplate;
          if (flag5 && !innerText.EndsWith("\\"))
            innerText += "\\";
        }
        else if (flag3)
        {
          type = FileResourceManager.FileType.DashboardViewTemplate;
          if (flag5 && !innerText.EndsWith("\\"))
            innerText += "\\";
        }
        else
          type = !flag4 ? (FileResourceManager.FileType) Enum.Parse(typeof (FileResourceManager.FileType), xmlNode.SelectSingleNode("FileType").InnerText) : FileResourceManager.FileType.AffiliatedBusinessArrangements;
        Dictionary<FileSystemEntry, BinaryObject> fileEntities = this.getFileEntities(FileSystemEntry.Parse(innerText), type, true);
        foreach (FileSystemEntry key in fileEntities.Keys)
        {
          string str = fileEntities[key] != null ? fileEntities[key].ToString() : "";
          string hashB64 = HashUtil.ComputeHashB64(str);
          if (!fetchData)
            str = "";
          FileSystemSyncRecord systemSyncRecord = new FileSystemSyncRecord(key.ToString(), key.ToString() + type.ToString() + flag5.ToString(), this.getDisplayPath(type, key.ToString()), str, tableDef.TableName, key.Type == FileSystemEntry.Types.Folder, hashB64);
          systemSyncRecord["FileType"] = (object) Enum.GetName(typeof (FileResourceManager.FileType), (object) type);
          systemSyncRecord["FilePath"] = (object) key.ToString();
          systemSyncRecord["FileSystemEntry.Type"] = (object) Enum.GetName(typeof (FileSystemEntry.Types), (object) key.Type);
          if (!fileSystemEntities.Contains(systemSyncRecord))
            fileSystemEntities.Add(systemSyncRecord);
          if (fileEntities[key] != null)
            fileEntities[key].Dispose();
        }
      }
      return fileSystemEntities;
    }

    public virtual List<FileSystemSyncRecord> GetImportOption(string xmlDataFile, TableDef tableDef)
    {
      List<FileSystemSyncRecord> importOption = new List<FileSystemSyncRecord>();
      string elementXpath = tableDef.GetElementXPath(".");
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xmlDataFile);
      XmlNodeList xmlNodeList = xmlDocument.SelectNodes(elementXpath);
      bool flag1 = tableDef.TableName.Equals("ReportSettings", StringComparison.OrdinalIgnoreCase);
      bool flag2 = tableDef.TableName.Equals("DashboardTemplate", StringComparison.OrdinalIgnoreCase);
      bool flag3 = tableDef.TableName.Equals("DashboardView", StringComparison.OrdinalIgnoreCase);
      bool flag4 = tableDef.TableName.Equals("AffiliatedBusinessArrangementsTemplates", StringComparison.OrdinalIgnoreCase);
      this.filePathNodeName = this.getNodeName(tableDef.TableName);
      foreach (XmlNode xmlNode in xmlNodeList)
      {
        FileResourceManager.FileType type = !flag1 ? (!flag2 ? (!flag3 ? (!flag4 ? (FileResourceManager.FileType) Enum.Parse(typeof (FileResourceManager.FileType), xmlNode.SelectSingleNode("FileType").InnerText) : FileResourceManager.FileType.AffiliatedBusinessArrangements) : FileResourceManager.FileType.DashboardViewTemplate) : FileResourceManager.FileType.DashboardTemplate) : FileResourceManager.FileType.Reports;
        int num = xmlNode.SelectSingleNode("IsFolder").InnerText == "true" ? 1 : 0;
        FileSystemEntry entry = FileSystemEntry.Parse(xmlNode.SelectSingleNode(this.filePathNodeName).InnerText);
        Dictionary<FileSystemEntry, BinaryObject> fileEntities = this.getFileEntities(entry, type, true);
        if (fileEntities.Count > 0)
        {
          foreach (FileSystemEntry key in fileEntities.Keys)
          {
            if (key.Type != FileSystemEntry.Types.Folder)
            {
              string hashB64 = HashUtil.ComputeHashB64(fileEntities[key] != null ? fileEntities[key].ToString() : "");
              FileSystemSyncRecord systemSyncRecord = new FileSystemSyncRecord(key.ToString(), key.ToString() + type.ToString() + (key.Type == FileSystemEntry.Types.Folder ? "True" : "False"), this.getDisplayPath(type, key.ToString()), "", tableDef.TableName, key.Type == FileSystemEntry.Types.Folder, hashB64);
              systemSyncRecord.AlreadyExist = true;
              if (!importOption.Contains(systemSyncRecord))
                importOption.Add(systemSyncRecord);
              if (fileEntities[key] != null)
                fileEntities[key].Dispose();
            }
          }
        }
        else
        {
          FileSystemSyncRecord systemSyncRecord = new FileSystemSyncRecord(entry.ToString(), entry.ToString() + type.ToString() + (entry.Type == FileSystemEntry.Types.Folder ? "True" : "False"), this.getDisplayPath(type, entry.ToString()), "", tableDef.TableName, entry.Type == FileSystemEntry.Types.Folder, "");
          systemSyncRecord.AlreadyExist = false;
          if (!importOption.Contains(systemSyncRecord))
            importOption.Add(systemSyncRecord);
        }
      }
      return importOption;
    }

    public virtual FileSystemSyncRecord GetActualData(FileSystemSyncRecord emptyRecord)
    {
      if (emptyRecord.IsFolder)
        return emptyRecord;
      FileSystemEntry fileSystemEntry = FileSystemEntry.Parse(string.Concat(emptyRecord["FilePath"]));
      FileResourceManager.FileType type = (FileResourceManager.FileType) Enum.Parse(typeof (FileResourceManager.FileType), string.Concat(emptyRecord["FileType"]));
      Dictionary<FileSystemEntry, BinaryObject> fileEntities = this.getFileEntities(fileSystemEntry, type, false);
      string str = fileEntities[fileSystemEntry].ToString();
      emptyRecord.RawData = str;
      if (fileEntities.Count > 0)
      {
        foreach (FileSystemEntry key in fileEntities.Keys)
        {
          if (fileEntities[key] != null)
            fileEntities[key].Dispose();
        }
      }
      return emptyRecord;
    }

    public virtual bool SynchronizeFileSystemSyncRecord(FileSystemSyncRecord updatedRecord)
    {
      FileResourceManager.FileType fileType = (FileResourceManager.FileType) Enum.Parse(typeof (FileResourceManager.FileType), string.Concat(updatedRecord["FileType"]));
      FileSystemEntry entry = FileSystemEntry.Parse(string.Concat(updatedRecord["FilePath"]));
      BinaryObject data = new BinaryObject(updatedRecord.RawData, Encoding.Default);
      switch (fileType)
      {
        case FileResourceManager.FileType.LoanProgram:
        case FileResourceManager.FileType.ClosingCost:
        case FileResourceManager.FileType.MiscData:
        case FileResourceManager.FileType.FormList:
        case FileResourceManager.FileType.DocumentSet:
        case FileResourceManager.FileType.LoanTemplate:
        case FileResourceManager.FileType.UnderwritingConditionSet:
        case FileResourceManager.FileType.PostClosingConditionSet:
        case FileResourceManager.FileType.CampaignTemplate:
        case FileResourceManager.FileType.DashboardTemplate:
        case FileResourceManager.FileType.DashboardViewTemplate:
        case FileResourceManager.FileType.TaskSet:
        case FileResourceManager.FileType.ConditionalApprovalLetter:
        case FileResourceManager.FileType.SettlementServiceProviders:
        case FileResourceManager.FileType.AffiliatedBusinessArrangements:
          return this.syncTemplateSetting(fileType, entry, data);
        case FileResourceManager.FileType.CustomPrintForms:
        case FileResourceManager.FileType.BorrowerCustomLetters:
        case FileResourceManager.FileType.BizCustomLetters:
          return this.syncContactCustomLetters(fileType, entry, data);
        case FileResourceManager.FileType.PrintGroups:
          return this.syncPrintGroups(entry, data);
        case FileResourceManager.FileType.Reports:
          return this.syncReports(entry, data);
        default:
          return false;
      }
    }

    private bool syncReports(FileSystemEntry entry, BinaryObject data)
    {
      this.ensureReportPathExists(entry);
      if (entry.Type == FileSystemEntry.Types.Folder)
        return true;
      try
      {
        bool flag = true;
        using (ReportSettingsFile reportSettingsFile = ReportSettingsStore.CheckOut(entry))
        {
          if (!reportSettingsFile.Exists)
            flag = false;
          reportSettingsFile.CheckIn(data);
        }
        if (flag)
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.TemplateModified, DateTime.Now, entry.Name, entry.Path));
        else
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.TemplateCreated, DateTime.Now, entry.Name, entry.Path));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FileResourceManager), ex, this.Session.SessionInfo);
      }
      return true;
    }

    private bool syncContactCustomLetters(
      FileResourceManager.FileType fileType,
      FileSystemEntry entry,
      BinaryObject data)
    {
      CustomLetterType customLetterType = this.toCustomLetterType(fileType);
      this.ensureContactCustomLetterPathExists(entry, customLetterType);
      if (entry.Type == FileSystemEntry.Types.Folder)
        return true;
      try
      {
        bool flag = true;
        using (DataFile dataFile = CustomLetterStore.CheckOut(customLetterType, entry))
        {
          if (!dataFile.Exists)
            flag = false;
          dataFile.CheckIn(data);
        }
        if (flag)
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.TemplateModified, DateTime.Now, entry.Name, entry.Path));
        else
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.TemplateCreated, DateTime.Now, entry.Name, entry.Path));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FileResourceManager), ex, this.Session.SessionInfo);
      }
      return true;
    }

    private bool syncPrintGroups(FileSystemEntry entry, BinaryObject data)
    {
      this.ensureFormGroupPathExists(entry);
      if (entry.Type == FileSystemEntry.Types.Folder)
        return true;
      try
      {
        bool flag = true;
        using (FormGroup formGroup = FormGroups.CheckOut(entry))
        {
          if (!formGroup.Exists)
            flag = false;
          formGroup.CheckIn(data);
        }
        if (flag)
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.TemplateModified, DateTime.Now, entry.Name, entry.Path));
        else
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.TemplateCreated, DateTime.Now, entry.Name, entry.Path));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FileResourceManager), ex, this.Session.SessionInfo);
      }
      return true;
    }

    private bool syncTemplateSetting(
      FileResourceManager.FileType fileType,
      FileSystemEntry entry,
      BinaryObject data)
    {
      TemplateSettingsType templateSettingsTypes = this.toTemplateSettingsTypes(fileType);
      this.ensureTemplateSettingsPathExists(entry, templateSettingsTypes);
      if (entry.Type == FileSystemEntry.Types.Folder)
        return true;
      try
      {
        bool flag = true;
        using (TemplateSettings templateSettings = TemplateSettingsStore.CheckOut(templateSettingsTypes, entry))
        {
          if (!templateSettings.Exists)
            flag = false;
          templateSettings.CheckIn(data);
        }
        if (flag)
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.TemplateModified, DateTime.Now, entry.Name, entry.Path));
        else
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.TemplateCreated, DateTime.Now, entry.Name, entry.Path));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FileResourceManager), ex, this.Session.SessionInfo);
      }
      return true;
    }

    private Dictionary<FileSystemEntry, BinaryObject> getFileEntities(
      FileSystemEntry entry,
      FileResourceManager.FileType type,
      bool recursive)
    {
      Dictionary<FileSystemEntry, BinaryObject> fileEntities = new Dictionary<FileSystemEntry, BinaryObject>();
      List<FileSystemEntry> fileSystemEntryList = new List<FileSystemEntry>();
      FileSystemEntry fileSystemEntry1 = (FileSystemEntry) null;
      switch (type)
      {
        case FileResourceManager.FileType.LoanProgram:
        case FileResourceManager.FileType.ClosingCost:
        case FileResourceManager.FileType.MiscData:
        case FileResourceManager.FileType.FormList:
        case FileResourceManager.FileType.DocumentSet:
        case FileResourceManager.FileType.LoanTemplate:
        case FileResourceManager.FileType.UnderwritingConditionSet:
        case FileResourceManager.FileType.PostClosingConditionSet:
        case FileResourceManager.FileType.CampaignTemplate:
        case FileResourceManager.FileType.DashboardTemplate:
        case FileResourceManager.FileType.DashboardViewTemplate:
        case FileResourceManager.FileType.TaskSet:
        case FileResourceManager.FileType.ConditionalApprovalLetter:
        case FileResourceManager.FileType.SettlementServiceProviders:
        case FileResourceManager.FileType.AffiliatedBusinessArrangements:
          TemplateSettingsType templateSettingsTypes = this.toTemplateSettingsTypes(type);
          FileSystemEntry[] fileSystemEntryArray1;
          try
          {
            fileSystemEntryArray1 = TemplateSettingsStore.GetDirectoryEntries(templateSettingsTypes, entry.ParentFolder, entry.Type == FileSystemEntry.Types.Folder);
          }
          catch
          {
            fileSystemEntryArray1 = new FileSystemEntry[0];
          }
          foreach (FileSystemEntry fileSystemEntry2 in fileSystemEntryArray1)
          {
            if (fileSystemEntry2.Equals((object) entry))
            {
              fileSystemEntry1 = fileSystemEntry2;
              break;
            }
          }
          if (!recursive)
          {
            if (fileSystemEntry1 != null)
            {
              if (fileSystemEntry1.Type == FileSystemEntry.Types.Folder)
              {
                fileEntities.Add(fileSystemEntry1, (BinaryObject) null);
                break;
              }
              using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(templateSettingsTypes, fileSystemEntry1))
              {
                fileEntities.Add(fileSystemEntry1, latestVersion.Data);
                break;
              }
            }
            else
              break;
          }
          else
          {
            if (fileSystemEntry1 != null)
            {
              FileSystemEntry parentFolder = fileSystemEntry1.ParentFolder;
              if (parentFolder != null && !parentFolder.Equals((object) fileSystemEntry1))
              {
                for (; parentFolder != null && !parentFolder.Equals((object) parentFolder.ParentFolder); parentFolder = parentFolder.ParentFolder)
                  fileSystemEntryList.Add(parentFolder);
              }
              if (fileSystemEntry1.Type == FileSystemEntry.Types.File)
              {
                fileSystemEntryList.Add(fileSystemEntry1);
              }
              else
              {
                fileSystemEntryList.Add(fileSystemEntry1);
                fileSystemEntryList.AddRange((IEnumerable<FileSystemEntry>) TemplateSettingsStore.GetDirectoryEntries(templateSettingsTypes, fileSystemEntry1, true));
              }
            }
            foreach (FileSystemEntry fileSystemEntry3 in fileSystemEntryList.ToArray())
            {
              if (fileSystemEntry3.Type == FileSystemEntry.Types.Folder)
              {
                fileEntities.Add(fileSystemEntry3, (BinaryObject) null);
              }
              else
              {
                using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(templateSettingsTypes, fileSystemEntry3))
                  fileEntities.Add(fileSystemEntry3, latestVersion.Data);
              }
            }
            break;
          }
        case FileResourceManager.FileType.CustomPrintForms:
        case FileResourceManager.FileType.BorrowerCustomLetters:
        case FileResourceManager.FileType.BizCustomLetters:
          CustomLetterType customLetterType = this.toCustomLetterType(type);
          FileSystemEntry[] fileSystemEntryArray2;
          try
          {
            fileSystemEntryArray2 = CustomLetterStore.GetDirectoryEntries(customLetterType, entry.ParentFolder);
          }
          catch
          {
            fileSystemEntryArray2 = new FileSystemEntry[0];
          }
          foreach (FileSystemEntry fileSystemEntry4 in fileSystemEntryArray2)
          {
            if (fileSystemEntry4.Equals((object) entry))
            {
              fileSystemEntry1 = fileSystemEntry4;
              break;
            }
          }
          if (!recursive)
          {
            if (fileSystemEntry1 != null)
            {
              if (fileSystemEntry1.Type == FileSystemEntry.Types.Folder)
              {
                fileEntities.Add(fileSystemEntry1, (BinaryObject) null);
                break;
              }
              using (DataFile dataFile = CustomLetterStore.CheckOut(customLetterType, fileSystemEntry1))
              {
                fileEntities.Add(fileSystemEntry1, dataFile.GetData());
                break;
              }
            }
            else
              break;
          }
          else
          {
            if (fileSystemEntry1 != null)
            {
              FileSystemEntry parentFolder = fileSystemEntry1.ParentFolder;
              if (parentFolder != null && !parentFolder.Equals((object) fileSystemEntry1))
              {
                for (; parentFolder != null && !parentFolder.Equals((object) parentFolder.ParentFolder); parentFolder = parentFolder.ParentFolder)
                  fileSystemEntryList.Add(parentFolder);
              }
              if (fileSystemEntry1.Type == FileSystemEntry.Types.File)
              {
                fileSystemEntryList.Add(fileSystemEntry1);
              }
              else
              {
                fileSystemEntryList.Add(fileSystemEntry1);
                fileSystemEntryList.AddRange((IEnumerable<FileSystemEntry>) CustomLetterStore.GetDirectoryEntries(customLetterType, fileSystemEntry1));
              }
            }
            foreach (FileSystemEntry fileSystemEntry5 in fileSystemEntryList.ToArray())
            {
              if (fileSystemEntry5.Type == FileSystemEntry.Types.Folder)
              {
                fileEntities.Add(fileSystemEntry5, (BinaryObject) null);
              }
              else
              {
                using (DataFile dataFile = CustomLetterStore.CheckOut(customLetterType, fileSystemEntry5))
                  fileEntities.Add(fileSystemEntry5, dataFile.GetData());
              }
            }
            break;
          }
        case FileResourceManager.FileType.PrintGroups:
          FileSystemEntry[] fileSystemEntryArray3;
          try
          {
            fileSystemEntryArray3 = FormGroups.GetDirectoryEntries(entry.ParentFolder);
          }
          catch
          {
            fileSystemEntryArray3 = new FileSystemEntry[0];
          }
          foreach (FileSystemEntry fileSystemEntry6 in fileSystemEntryArray3)
          {
            if (fileSystemEntry6.Equals((object) entry))
            {
              fileSystemEntry1 = fileSystemEntry6;
              break;
            }
          }
          if (!recursive)
          {
            if (fileSystemEntry1 != null)
            {
              if (fileSystemEntry1.Type == FileSystemEntry.Types.Folder)
              {
                fileEntities.Add(fileSystemEntry1, (BinaryObject) null);
                break;
              }
              using (FormGroup formGroup = FormGroups.CheckOut(fileSystemEntry1))
              {
                fileEntities.Add(fileSystemEntry1, formGroup.GroupFile.GetData());
                break;
              }
            }
            else
              break;
          }
          else
          {
            if (fileSystemEntry1 != null)
            {
              FileSystemEntry parentFolder = fileSystemEntry1.ParentFolder;
              if (parentFolder != null && !parentFolder.Equals((object) fileSystemEntry1))
              {
                for (; parentFolder != null && !parentFolder.Equals((object) parentFolder.ParentFolder); parentFolder = parentFolder.ParentFolder)
                  fileSystemEntryList.Add(parentFolder);
              }
              if (fileSystemEntry1.Type == FileSystemEntry.Types.File)
              {
                fileSystemEntryList.Add(fileSystemEntry1);
              }
              else
              {
                fileSystemEntryList.Add(fileSystemEntry1);
                fileSystemEntryList.AddRange((IEnumerable<FileSystemEntry>) FormGroups.GetDirectoryEntries(fileSystemEntry1));
              }
            }
            foreach (FileSystemEntry fileSystemEntry7 in fileSystemEntryList.ToArray())
            {
              if (fileSystemEntry7.Type == FileSystemEntry.Types.Folder)
              {
                fileEntities.Add(fileSystemEntry7, (BinaryObject) null);
              }
              else
              {
                using (FormGroup formGroup = FormGroups.CheckOut(fileSystemEntry7))
                  fileEntities.Add(fileSystemEntry7, formGroup.GroupFile.GetData());
              }
            }
            break;
          }
        case FileResourceManager.FileType.Reports:
          FileSystemEntry[] fileSystemEntryArray4;
          try
          {
            fileSystemEntryArray4 = ReportSettingsStore.GetDirectoryEntries(entry.ParentFolder);
          }
          catch
          {
            fileSystemEntryArray4 = new FileSystemEntry[0];
          }
          foreach (FileSystemEntry fileSystemEntry8 in fileSystemEntryArray4)
          {
            if (fileSystemEntry8.Equals((object) entry))
            {
              fileSystemEntry1 = fileSystemEntry8;
              break;
            }
          }
          if (!recursive)
          {
            if (fileSystemEntry1 != null)
            {
              if (fileSystemEntry1.Type == FileSystemEntry.Types.Folder)
              {
                fileEntities.Add(fileSystemEntry1, (BinaryObject) null);
                break;
              }
              using (ReportSettingsFile reportSettingsFile = ReportSettingsStore.CheckOut(fileSystemEntry1))
              {
                fileEntities.Add(fileSystemEntry1, reportSettingsFile.Data.GetData());
                break;
              }
            }
            else
              break;
          }
          else
          {
            if (fileSystemEntry1 != null)
            {
              FileSystemEntry parentFolder = fileSystemEntry1.ParentFolder;
              if (parentFolder != null && !parentFolder.Equals((object) fileSystemEntry1))
              {
                for (; parentFolder != null && !parentFolder.Equals((object) parentFolder.ParentFolder); parentFolder = parentFolder.ParentFolder)
                  fileSystemEntryList.Add(parentFolder);
              }
              if (fileSystemEntry1.Type == FileSystemEntry.Types.File)
              {
                fileSystemEntryList.Add(fileSystemEntry1);
              }
              else
              {
                fileSystemEntryList.Add(fileSystemEntry1);
                fileSystemEntryList.AddRange((IEnumerable<FileSystemEntry>) ReportSettingsStore.GetDirectoryEntries(fileSystemEntry1));
              }
            }
            foreach (FileSystemEntry fileSystemEntry9 in fileSystemEntryList.ToArray())
            {
              if (fileSystemEntry9.Type == FileSystemEntry.Types.Folder)
              {
                fileEntities.Add(fileSystemEntry9, (BinaryObject) null);
              }
              else
              {
                using (ReportSettingsFile reportSettingsFile = ReportSettingsStore.CheckOut(fileSystemEntry9))
                  fileEntities.Add(fileSystemEntry9, reportSettingsFile.Data.GetData());
              }
            }
            break;
          }
      }
      return fileEntities;
    }

    private CustomLetterType toCustomLetterType(FileResourceManager.FileType type)
    {
      switch (type)
      {
        case FileResourceManager.FileType.CustomPrintForms:
          return CustomLetterType.Generic;
        case FileResourceManager.FileType.BorrowerCustomLetters:
          return CustomLetterType.Borrower;
        case FileResourceManager.FileType.BizCustomLetters:
          return CustomLetterType.BizPartner;
        default:
          throw new Exception("not a custom letter type");
      }
    }

    private TemplateSettingsType toTemplateSettingsTypes(FileResourceManager.FileType type)
    {
      switch (type)
      {
        case FileResourceManager.FileType.LoanProgram:
          return TemplateSettingsType.LoanProgram;
        case FileResourceManager.FileType.ClosingCost:
          return TemplateSettingsType.ClosingCost;
        case FileResourceManager.FileType.MiscData:
          return TemplateSettingsType.MiscData;
        case FileResourceManager.FileType.FormList:
          return TemplateSettingsType.FormList;
        case FileResourceManager.FileType.DocumentSet:
          return TemplateSettingsType.DocumentSet;
        case FileResourceManager.FileType.LoanTemplate:
          return TemplateSettingsType.LoanTemplate;
        case FileResourceManager.FileType.PrintGroups:
          return TemplateSettingsType.PrintFormGroups;
        case FileResourceManager.FileType.UnderwritingConditionSet:
          return TemplateSettingsType.UnderwritingConditionSet;
        case FileResourceManager.FileType.PostClosingConditionSet:
          return TemplateSettingsType.PostClosingConditionSet;
        case FileResourceManager.FileType.CampaignTemplate:
          return TemplateSettingsType.Campaign;
        case FileResourceManager.FileType.DashboardTemplate:
          return TemplateSettingsType.DashboardTemplate;
        case FileResourceManager.FileType.DashboardViewTemplate:
          return TemplateSettingsType.DashboardViewTemplate;
        case FileResourceManager.FileType.TaskSet:
          return TemplateSettingsType.TaskSet;
        case FileResourceManager.FileType.ConditionalApprovalLetter:
          return TemplateSettingsType.ConditionalLetter;
        case FileResourceManager.FileType.SettlementServiceProviders:
          return TemplateSettingsType.SettlementServiceProviders;
        case FileResourceManager.FileType.AffiliatedBusinessArrangements:
          return TemplateSettingsType.AffiliatedBusinessArrangements;
        default:
          throw new Exception("not a template setting type");
      }
    }

    private string getDisplayPath(FileResourceManager.FileType type, string path)
    {
      switch (type)
      {
        case FileResourceManager.FileType.LoanProgram:
          return "Loan Program:" + path;
        case FileResourceManager.FileType.ClosingCost:
          return "Closing Cost Template:" + path;
        case FileResourceManager.FileType.MiscData:
          return "Misc Data Template:" + path;
        case FileResourceManager.FileType.FormList:
          return "Form List Template:" + path;
        case FileResourceManager.FileType.DocumentSet:
          return "Document Set Template:" + path;
        case FileResourceManager.FileType.LoanTemplate:
          return "Loan Template:" + path;
        case FileResourceManager.FileType.CustomPrintForms:
          return "Custom Output Forms:" + path;
        case FileResourceManager.FileType.PrintGroups:
          return "Print Group:" + path;
        case FileResourceManager.FileType.Reports:
          return "Report Template:" + path;
        case FileResourceManager.FileType.BorrowerCustomLetters:
          return "Borrower Custom Letter:" + path;
        case FileResourceManager.FileType.BizCustomLetters:
          return "Business Custom Letter:" + path;
        case FileResourceManager.FileType.CampaignTemplate:
          return "Campaign Template:" + path;
        case FileResourceManager.FileType.DashboardTemplate:
          return "Dashboard Template:" + path;
        case FileResourceManager.FileType.DashboardViewTemplate:
          return "Dashboard View Template:" + path;
        case FileResourceManager.FileType.TaskSet:
          return "Task Set:" + path;
        case FileResourceManager.FileType.ConditionalApprovalLetter:
          return "Conditional Approval Letter:" + path;
        case FileResourceManager.FileType.SettlementServiceProviders:
          return "Settlement Service Provider:" + path;
        case FileResourceManager.FileType.AffiliatedBusinessArrangements:
          return "Affiliate Template:" + path;
        default:
          return path;
      }
    }

    private string getVirtualRootPath(FileResourceManager.FileType fileType)
    {
      switch (fileType)
      {
        case FileResourceManager.FileType.LoanProgram:
          return "Public:\\Public Loan Programs\\";
        case FileResourceManager.FileType.ClosingCost:
          return "Public:\\Public Closing Cost Templates\\";
        case FileResourceManager.FileType.MiscData:
          return "Public:\\Public Data Templates\\";
        case FileResourceManager.FileType.FormList:
          return "Public:\\Public Form Lists\\";
        case FileResourceManager.FileType.DocumentSet:
          return "Public:\\Public Document Sets\\";
        case FileResourceManager.FileType.LoanTemplate:
          return "Public:\\Public Loan Templates\\";
        case FileResourceManager.FileType.CustomPrintForms:
          return "Public:\\Public Custom Forms\\";
        case FileResourceManager.FileType.PrintGroups:
          return "Public:\\Public Forms Groups\\";
        case FileResourceManager.FileType.Reports:
          return "Public:\\Public Reports\\";
        case FileResourceManager.FileType.BorrowerCustomLetters:
          return "Public:\\Public Custom Letters\\";
        case FileResourceManager.FileType.BizCustomLetters:
          return "Public:\\Public Custom Letters\\";
        case FileResourceManager.FileType.CampaignTemplate:
          return "Public:\\Public Campaign Templates\\";
        case FileResourceManager.FileType.DashboardTemplate:
          return "Public:\\Public Dashboard Templates\\";
        case FileResourceManager.FileType.DashboardViewTemplate:
          return "Public:\\Public DashboardView Templates\\";
        case FileResourceManager.FileType.TaskSet:
          return "Public:\\Public Task Sets\\";
        case FileResourceManager.FileType.ConditionalApprovalLetter:
          return "Public:\\Public Conditional Approval Leter\\";
        case FileResourceManager.FileType.SettlementServiceProviders:
          return "Public:\\Public Settlement Service Providers\\";
        case FileResourceManager.FileType.AffiliatedBusinessArrangements:
          return "Public:\\Public Affiliates\\";
        default:
          return "";
      }
    }

    private string getExtension(FileResourceManager.FileType fileType)
    {
      switch (fileType)
      {
        case FileResourceManager.FileType.LoanProgram:
        case FileResourceManager.FileType.ClosingCost:
        case FileResourceManager.FileType.MiscData:
        case FileResourceManager.FileType.FormList:
        case FileResourceManager.FileType.DocumentSet:
        case FileResourceManager.FileType.LoanTemplate:
        case FileResourceManager.FileType.PrintGroups:
        case FileResourceManager.FileType.Reports:
        case FileResourceManager.FileType.CampaignTemplate:
        case FileResourceManager.FileType.DashboardTemplate:
        case FileResourceManager.FileType.DashboardViewTemplate:
        case FileResourceManager.FileType.TaskSet:
        case FileResourceManager.FileType.ConditionalApprovalLetter:
        case FileResourceManager.FileType.SettlementServiceProviders:
        case FileResourceManager.FileType.AffiliatedBusinessArrangements:
          return ".xml";
        case FileResourceManager.FileType.CustomPrintForms:
        case FileResourceManager.FileType.BorrowerCustomLetters:
        case FileResourceManager.FileType.BizCustomLetters:
          return ".doc";
        default:
          return "";
      }
    }

    private void ensureTemplateSettingsPathExists(FileSystemEntry entry, TemplateSettingsType type)
    {
      if (entry.Type != FileSystemEntry.Types.Folder)
      {
        this.ensureTemplateSettingsPathExists(entry.ParentFolder, type);
      }
      else
      {
        if (entry.ParentFolder != null)
          this.ensureTemplateSettingsPathExists(entry.ParentFolder, type);
        if (Directory.Exists(TemplateSettings.GetFolderPath(type, entry)))
          return;
        Directory.CreateDirectory(TemplateSettings.GetFolderPath(type, entry));
      }
    }

    private void ensureFormGroupPathExists(FileSystemEntry entry)
    {
      if (entry.Type != FileSystemEntry.Types.Folder)
      {
        this.ensureFormGroupPathExists(entry.ParentFolder);
      }
      else
      {
        if (entry.ParentFolder != null)
          this.ensureFormGroupPathExists(entry.ParentFolder);
        if (Directory.Exists(FormGroup.GetFolderPath(entry)))
          return;
        Directory.CreateDirectory(FormGroup.GetFolderPath(entry));
      }
    }

    private void ensureContactCustomLetterPathExists(FileSystemEntry entry, CustomLetterType type)
    {
      if (entry.Type != FileSystemEntry.Types.Folder)
      {
        this.ensureContactCustomLetterPathExists(entry.ParentFolder, type);
      }
      else
      {
        if (entry.ParentFolder != null)
          this.ensureContactCustomLetterPathExists(entry.ParentFolder, type);
        string customLetterPath = CustomLetterStore.getCustomLetterPath(type, entry);
        if (Directory.Exists(customLetterPath))
          return;
        Directory.CreateDirectory(customLetterPath);
      }
    }

    private void ensureReportPathExists(FileSystemEntry entry)
    {
      if (entry.Type != FileSystemEntry.Types.Folder)
      {
        this.ensureReportPathExists(entry.ParentFolder);
      }
      else
      {
        if (entry.ParentFolder != null)
          this.ensureReportPathExists(entry.ParentFolder);
        string reportFolderPath = ReportSettingsStore.getReportFolderPath(entry);
        if (Directory.Exists(reportFolderPath))
          return;
        Directory.CreateDirectory(reportFolderPath);
      }
    }

    private string getNodeName(string sourceTableName)
    {
      return sourceTableName.ToLower() == "dashboardtemplate" || sourceTableName.ToLower() == "dashboardview" || sourceTableName.ToLower() == "AffiliatedBusinessArrangementsTemplates".ToLower() ? "TemplatePath" : "FilePath";
    }

    internal enum FileType
    {
      None,
      LoanProgram,
      ClosingCost,
      MiscData,
      FormList,
      DocumentSet,
      LoanTemplate,
      CustomPrintForms,
      PrintGroups,
      Reports,
      BorrowerCustomLetters,
      BizCustomLetters,
      UnderwritingConditionSet,
      PostClosingConditionSet,
      CampaignTemplate,
      DashboardTemplate,
      DashboardViewTemplate,
      TaskSet,
      ConditionalApprovalLetter,
      SettlementServiceProviders,
      AffiliatedBusinessArrangements,
    }
  }
}
