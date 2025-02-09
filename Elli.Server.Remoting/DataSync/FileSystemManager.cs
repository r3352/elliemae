// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.DataSync.FileSystemManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.Interception;
using Elli.Server.Remoting.SessionObjects;
using EllieMae.EMLite.ClientServer;
using System.Collections.Generic;

#nullable disable
namespace Elli.Server.Remoting.DataSync
{
  public class FileSystemManager : SessionBoundObject
  {
    private string baseFolder = "";
    private string sourceXmlString = "";
    private const string className = "FileSystemManager";
    private Dictionary<string, IFileManager> fileMgr = new Dictionary<string, IFileManager>();

    public FileSystemManager(string baseFolder, string sourceXmlString, ISession session)
    {
      this.InitializeInternal(session, nameof (FileSystemManager).ToLower());
      this.baseFolder = baseFolder;
      this.sourceXmlString = sourceXmlString;
      this.constructDefaultManagers();
    }

    public FileSystemManager(ISession session)
      : this("", "", session)
    {
    }

    private void constructDefaultManagers()
    {
      this.fileMgr.Add("InputForms", (IFileManager) new InputFormManager());
      this.fileMgr.Add("AffiliatedBusinessArrangementsTemplates", (IFileManager) InterceptionUtils.NewInstance<FileResourceManager>().Initialize(this.Session));
      this.fileMgr.Add("FileResource", (IFileManager) InterceptionUtils.NewInstance<FileResourceManager>().Initialize(this.Session));
      this.fileMgr.Add("BR_LoanFolderRules", (IFileManager) new LoanFolderManager());
      this.fileMgr.Add("ReportSettings", (IFileManager) InterceptionUtils.NewInstance<FileResourceManager>().Initialize(this.Session));
      this.fileMgr.Add("DashboardTemplate", (IFileManager) InterceptionUtils.NewInstance<FileResourceManager>().Initialize(this.Session));
      this.fileMgr.Add("DashboardView", (IFileManager) InterceptionUtils.NewInstance<FileResourceManager>().Initialize(this.Session));
    }

    public void SetFileManager(string tableName, IFileManager newMgr)
    {
      if (this.fileMgr.ContainsKey(tableName))
        this.fileMgr[tableName] = newMgr;
      else
        this.fileMgr.Add(tableName, newMgr);
    }

    public List<FileSystemSyncRecord> GetAssociatedFileSystemEntries(
      TableDef tableDef,
      bool fetchData)
    {
      List<string> stringList = new List<string>();
      IFileManager fileManager = (IFileManager) null;
      if (this.fileMgr.ContainsKey(tableDef.TableName))
        fileManager = this.fileMgr[tableDef.TableName];
      return fileManager != null ? fileManager.GetAssociatedFileSystemEntities(this.sourceXmlString, tableDef, fetchData) : new List<FileSystemSyncRecord>();
    }

    public List<FileSystemSyncRecord> GetImportOption(TableDef tableDef)
    {
      List<string> stringList = new List<string>();
      IFileManager fileManager = (IFileManager) null;
      if (this.fileMgr.ContainsKey(tableDef.TableName))
        fileManager = this.fileMgr[tableDef.TableName];
      return fileManager != null ? fileManager.GetImportOption(this.sourceXmlString, tableDef) : new List<FileSystemSyncRecord>();
    }

    public List<FileSystemSyncRecord> GetData(List<FileSystemSyncRecord> emptyFileList)
    {
      List<FileSystemSyncRecord> data = new List<FileSystemSyncRecord>();
      foreach (FileSystemSyncRecord emptyFile in emptyFileList)
      {
        IFileManager fileManager = !this.fileMgr.ContainsKey(emptyFile.TableName) ? (IFileManager) null : this.fileMgr[emptyFile.TableName];
        if (fileManager != null)
        {
          FileSystemSyncRecord actualData = fileManager.GetActualData(emptyFile);
          data.Add(actualData);
        }
      }
      return data;
    }

    public bool Update(FileSystemSyncRecord fileSystemRecord)
    {
      IFileManager fileManager = (IFileManager) null;
      if (this.fileMgr.ContainsKey(fileSystemRecord.TableName))
        fileManager = this.fileMgr[fileSystemRecord.TableName];
      return fileManager.SynchronizeFileSystemSyncRecord(fileSystemRecord);
    }
  }
}
