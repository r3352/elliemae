// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanFolder
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.MessageServices.Message.Alerts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Serialization;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.Properties;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class LoanFolder : IComparable
  {
    private const string className = "LoanFolder�";
    private static readonly long _maxLoanFileSizeForLoggingWarning;
    private static readonly long _maxLoanFileSizeForThrowingError;
    private string name;
    private DirectoryInfo loanFolder;
    private LoanFolderInfo.LoanFolderType folderType = LoanFolderInfo.LoanFolderType.NotSpecified;
    private bool skipAddingEntityIds;
    private static object locker = new object();
    private static bool DisableAddEntityIdsOnOpenLoan;

    static LoanFolder()
    {
      if (!bool.TryParse(ConfigurationManager.AppSettings[nameof (DisableAddEntityIdsOnOpenLoan)], out LoanFolder.DisableAddEntityIdsOnOpenLoan))
        LoanFolder.DisableAddEntityIdsOnOpenLoan = false;
      long.TryParse(ConfigurationManager.AppSettings["MaxLoanFileSizeForLoggingWarning"], out LoanFolder._maxLoanFileSizeForLoggingWarning);
      long.TryParse(ConfigurationManager.AppSettings["MaxLoanFileSizeForThrowingError"], out LoanFolder._maxLoanFileSizeForThrowingError);
    }

    public LoanFolder(string name, bool skipAddingEntityIds)
    {
      this.name = name;
      this.skipAddingEntityIds = skipAddingEntityIds;
      if (name.IndexOf("\\") >= 0)
        this.loanFolder = new DirectoryInfo(name);
      else
        this.loanFolder = LoanFolder.getLoanFolder(name);
    }

    public LoanFolder(string name)
      : this(name, LoanFolder.DisableAddEntityIdsOnOpenLoan)
    {
    }

    public LoanFolder(DirectoryInfo dirInfo, bool skipAddingEntityIds)
    {
      this.name = dirInfo.Name;
      this.loanFolder = dirInfo;
      this.skipAddingEntityIds = skipAddingEntityIds;
    }

    public LoanFolder(DirectoryInfo dirInfo)
      : this(dirInfo, false)
    {
    }

    public string Name => this.name;

    public LoanFolderInfo.LoanFolderType FolderType
    {
      get
      {
        if (this.folderType == LoanFolderInfo.LoanFolderType.NotSpecified)
          this.folderType = LoanFolder.GetLoanFolderType(this.name);
        return this.folderType;
      }
    }

    public bool IncludeInDuplicateLoanCheck => LoanFolder.GetIncludeInDuplicateLoanCheck(this.name);

    public LoanIdentity[] GetContents() => this.GetContents(false, (string) null);

    public LoanIdentity[] GetContents(bool useERDB, string rdbLastUpdated)
    {
      DbQueryBuilder sql = new DbQueryBuilder();
      sql.Append("select Guid, LoanFolder, LoanName from LoanSummary where LoanFolder like '" + EllieMae.EMLite.DataAccess.SQL.Escape(this.loanFolder.Name) + "'");
      if (rdbLastUpdated != null)
      {
        string str = useERDB ? "ERDBLastUpdated" : "RDBLastUpdated";
        sql.Append(" and [" + str + "] < '" + rdbLastUpdated + "'");
      }
      return LoanFolder.execFolderContentQuery((EllieMae.EMLite.DataAccess.DbQueryBuilder) sql);
    }

    public LoanIdentity[] GetContents(
      UserInfo userInfo,
      LoanInfo.Right accessRights,
      bool isExternalOrganization)
    {
      DbQueryBuilder sql = new DbQueryBuilder();
      sql.Append("select Guid, LoanFolder, LoanName from LoanSummary");
      string userVisibleIdQuery = Pipeline.GetUserVisibleIDQuery(userInfo, this.Name, accessRights, isExternalOrganization);
      if (userVisibleIdQuery != "")
        sql.Append(" where Guid in (" + userVisibleIdQuery + ")");
      return LoanFolder.execFolderContentQuery((EllieMae.EMLite.DataAccess.DbQueryBuilder) sql);
    }

    public LoanIdentity[] GetContentsSDK(
      UserInfo userInfo,
      LoanInfo.Right accessRights,
      bool isExternalOrganization)
    {
      DbQueryBuilder sql = new DbQueryBuilder();
      sql.AppendLine("-- Create a temp table for the loan folders");
      sql.AppendLine("\r\n                            if object_id('tempdb..#loanFolders', 'U') is not null\r\n                            drop table #loanFolders\r\n                            create table #loanFolders(name varchar(250) primary key)");
      sql.AppendLine(string.Format("insert into #loanFolders values ({0})", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) this.Name)));
      sql.AppendLine(" declare @loanFolders StringTable ");
      sql.AppendLine(string.Format(" insert into @loanFolders values ({0}) ", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) this.Name)));
      sql.Append("select Guid, LoanFolder, LoanName from LoanSummary");
      string userVisibleIdQuery = Pipeline.GetUserVisibleIDQuery(userInfo, this.Name, accessRights, isExternalOrganization);
      if (userVisibleIdQuery != "")
        sql.Append(" where Guid in (" + userVisibleIdQuery + ")");
      return LoanFolder.execFolderContentQuery((EllieMae.EMLite.DataAccess.DbQueryBuilder) sql);
    }

    public PipelineInfo[] GetPipeline(
      UserInfo userInfo,
      LoanInfo.Right accessRights,
      bool isExternalOrganization)
    {
      return this.GetPipeline(userInfo, accessRights, (QueryCriterion) null, isExternalOrganization);
    }

    public PipelineInfo[] GetPipeline(
      UserInfo userInfo,
      LoanInfo.Right accessRights,
      QueryCriterion filter,
      bool isExternalOrganization)
    {
      return this.GetPipeline(userInfo, accessRights, (string[]) null, PipelineData.All, filter, isExternalOrganization);
    }

    public PipelineInfo[] GetPipeline(
      UserInfo userInfo,
      LoanInfo.Right accessRights,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization)
    {
      return this.GetPipeline(userInfo, accessRights, fields, dataToInclude, (QueryCriterion) null, isExternalOrganization);
    }

    public PipelineInfo[] GetPipeline(
      UserInfo userInfo,
      LoanInfo.Right accessRights,
      string[] fields,
      PipelineData dataToInclude,
      QueryCriterion filter,
      bool isExternalOrganization)
    {
      return this.GetPipeline(userInfo, accessRights, fields, dataToInclude, filter, (SortField[]) null, isExternalOrganization);
    }

    public PipelineInfo[] GetPipeline(
      UserInfo userInfo,
      LoanInfo.Right accessRights,
      string[] fields,
      PipelineData dataToInclude,
      QueryCriterion filter,
      SortField[] sortFields,
      bool isExternalOrganization)
    {
      return Pipeline.Generate(userInfo, this.Name, accessRights, fields, dataToInclude, filter, sortFields, isExternalOrganization);
    }

    public string[] GetLoanDirectoriesFromDisk()
    {
      ArrayList arrayList = new ArrayList();
      DirectoryInfo[] directories = this.loanFolder.GetDirectories();
      if (directories == null)
        return new string[0];
      for (int index = 0; index < directories.Length; ++index)
      {
        if (LoanNameFolderGenerator.IsLoanNameFolder(directories[index].Name))
        {
          foreach (DirectoryInfo directory in directories[index].GetDirectories())
            arrayList.Add((object) (directories[index].Name + "\\" + directory.Name));
        }
        else
          arrayList.Add((object) directories[index].Name);
      }
      return (string[]) arrayList.ToArray(typeof (string));
    }

    public long GetLoanSize(string loanName)
    {
      string loanPath = this.getLoanPath(loanName);
      if (!Directory.Exists(loanPath))
        return 0;
      long loanSize = 0;
      foreach (string file in Directory.GetFiles(loanPath))
        loanSize += new FileInfo(file).Length;
      return loanSize;
    }

    public static LoanFolderInfo.LoanFolderType GetLoanFolderType(string folderName)
    {
      if (folderName.ToLower() == SystemSettings.ArchiveFolder.ToLower())
        return LoanFolderInfo.LoanFolderType.Archive;
      if (folderName.ToLower() == SystemSettings.TrashFolder.ToLower())
        return LoanFolderInfo.LoanFolderType.Trash;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select [folderType] from [LoanFolder] where [folderName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) folderName));
      return EllieMae.EMLite.DataAccess.SQL.DecodeEnum<LoanFolderInfo.LoanFolderType>(dbQueryBuilder.ExecuteScalar(), LoanFolderInfo.LoanFolderType.Regular);
    }

    public LoanFolderInfo GetFolderInfo()
    {
      try
      {
        return new LoanFolderInfo(this.loanFolder.Name, this.FolderType, this.IncludeInDuplicateLoanCheck);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFolder), ex);
        return (LoanFolderInfo) null;
      }
    }

    public void Create()
    {
      try
      {
        if (!this.loanFolder.Exists)
          this.loanFolder.Create();
        string name = this.loanFolder.Name;
        if (!(name.ToLower() == SystemSettings.ArchiveFolder.ToLower()))
          return;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("delete from [LoanFolder] where folderName = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) name));
        dbQueryBuilder.AppendLine("insert into [LoanFolder] (folderName, folderType) values (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) name) + ", 2)");
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFolder), ex);
      }
    }

    public void Delete(bool forceDelete)
    {
      try
      {
        string name = this.loanFolder.Name;
        if (this.loanFolder.Exists)
          this.loanFolder.Delete(forceDelete);
        if (LoanNameFolderGenerator.GetMaxEntriesInAFolder(ClientContext.GetCurrent()) > 0)
          LoanNameFolderGenerator.DeleteLoanFolder(name);
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("delete from [LoanFolder] where folderName = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) name));
        dbQueryBuilder.AppendLine("delete from [AclGroupLoanFolderAccess] where folderName = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) name));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Raise(TraceLevel.Error, nameof (LoanFolder), new ServerException("Unable to delete loan folder " + this.loanFolder.Name + "; " + ex.Message));
      }
    }

    public void DeleteLoanFiles(string loanName)
    {
      this.ensureSafeLoanFolderName(loanName);
      string dataFilePath = this.getDataFilePath(loanName);
      if (!Directory.Exists(Path.GetDirectoryName(dataFilePath)))
        return;
      Directory.Delete(Path.GetDirectoryName(dataFilePath), true);
    }

    public void MoveLoanFiles(string sourceLoanName, LoanIdentity targetId)
    {
      this.ensureSafeLoanFolderName(sourceLoanName);
      this.ensureSafeLoanFolderName(targetId.LoanName);
      LoanFolder loanFolder = new LoanFolder(targetId.LoanFolder);
      string loanPath1 = this.getLoanPath(sourceLoanName);
      string loanPath2 = loanFolder.getLoanPath(targetId.LoanName);
      if (Company.GetCompanySetting("FEATURE", "ENABLENAMESPACE").ToLower() == "true")
      {
        if (!loanFolder.Exists)
          Err.Raise(TraceLevel.Error, nameof (LoanFolder), (ServerException) new ObjectNotFoundException("Target folder does not exist.", ObjectType.LoanFolder, (object) loanFolder.Name));
        if (!Directory.Exists(loanPath1))
          Err.Raise(TraceLevel.Error, nameof (LoanFolder), new ServerException("Source loan data folder '" + sourceLoanName + "' does not exist within loan folder '" + this.name + "'."));
        try
        {
          LoanFolder.DirectoryCopy(loanPath1, loanPath2, true);
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (LoanFolder), string.Format("Error occured during move loan file with namespace enabled - directory copy operation. Loan Guid: {0}, Exception type: {1}.", (object) sourceLoanName, (object) ex.GetType()));
          Err.Raise(TraceLevel.Error, nameof (LoanFolder), new ServerException("Some of the files failed to copy over to" + loanFolder.Name + ". " + ex.Message));
        }
      }
      else
      {
        if (Directory.Exists(loanPath2))
        {
          try
          {
            foreach (FileInfo file in new DirectoryInfo(loanPath1).GetFiles())
            {
              if (file.Length != 0L)
                file.CopyTo(Path.Combine(loanPath2, file.Name), true);
            }
            return;
          }
          catch
          {
            Err.Raise(TraceLevel.Error, nameof (LoanFolder), new ServerException("Some of the files failed to copy over to" + loanFolder.Name + "."));
          }
        }
        Directory.Move(loanPath1, loanPath2);
      }
    }

    private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
      DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
      if (!directoryInfo.Exists)
        Err.Raise(TraceLevel.Error, nameof (LoanFolder), new ServerException("Source directory does not exist or could not be found: " + sourceDirName));
      Directory.CreateDirectory(destDirName);
      foreach (FileInfo file in directoryInfo.GetFiles())
      {
        string destFileName = Path.Combine(destDirName, file.Name);
        file.CopyTo(destFileName, true);
      }
      if (!copySubDirs)
        return;
      foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
      {
        string destDirName1 = Path.Combine(destDirName, directory.Name);
        LoanFolder.DirectoryCopy(directory.FullName, destDirName1, copySubDirs);
      }
    }

    public void CopyLoanFiles(string sourceLoanName, LoanIdentity targetId)
    {
      this.ensureSafeLoanFolderName(sourceLoanName);
      this.ensureSafeLoanFolderName(targetId.LoanName);
      LoanFolder loanFolder = new LoanFolder(targetId.LoanFolder);
      if (!loanFolder.Exists)
        Err.Raise(TraceLevel.Error, nameof (LoanFolder), (ServerException) new ObjectNotFoundException("Target folder does not exist.", ObjectType.LoanFolder, (object) loanFolder.Name));
      string loanPath1 = this.getLoanPath(sourceLoanName);
      string loanPath2 = loanFolder.getLoanPath(targetId.LoanName);
      if (!Directory.Exists(loanPath1))
        Err.Raise(TraceLevel.Error, nameof (LoanFolder), new ServerException("Source loan data folder '" + sourceLoanName + "' does not exist within loan folder '" + this.name + "'."));
      try
      {
        LoanFolder.DirectoryCopy(loanPath1, loanPath2, true);
      }
      catch (Exception ex)
      {
        Err.Raise(TraceLevel.Error, nameof (LoanFolder), new ServerException("Some of the files failed to copy over to" + loanFolder.Name + ". " + ex.Message));
      }
    }

    public bool LoanFilesExist(string loanName) => File.Exists(this.getDataFilePath(loanName));

    public bool Exists => this.loanFolder.Exists;

    public bool IsEmpty() => this.loanFolder.GetDirectories().Length == 0;

    public XmlDocument ReadLoanXmlData(string loanName)
    {
      string filePath = this.getDataFilePath(loanName);
      try
      {
        XmlDocument xmlDocument = (XmlDocument) null;
        using (DataFile file = FileStore.CheckOut(filePath))
        {
          if (!file.Exists)
            return (XmlDocument) null;
          using (BinaryObject data = file.GetData(loanName, LoanFolder._maxLoanFileSizeForThrowingError, LoanFolder._maxLoanFileSizeForLoggingWarning))
            xmlDocument = !this.skipAddingEntityIds ? new LoanDataFormatter().AddMissingEntityIds(data, (Action<BinaryObject>) (d =>
            {
              Stopwatch stopwatch = new Stopwatch();
              stopwatch.Start();
              file.CheckIn(d, true);
              stopwatch.Stop();
              TraceLog.Write(Encompass.Diagnostics.Logging.LogLevel.INFO.Force(), nameof (LoanFolder), "Added missing entityIds to " + filePath + " in " + (object) stopwatch.ElapsedMilliseconds + "ms");
            })) : new LoanDataFormatter().ReadXmlData(data);
        }
        TraceLog.WriteVerbose(nameof (LoanFolder), "Read LoanData for loan \"" + this.loanFolder.Name + "\\" + loanName + "\" from " + filePath);
        return xmlDocument;
      }
      catch (FileSizeLimitExceededException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (LoanFolder), "Error reading loan - " + loanName + " xml data with exception - " + ex.GetFullStackTrace());
        Err.Raise(TraceLevel.Warning, nameof (LoanFolder), new ServerException("Cannot deserialize loan data file '" + filePath + "': " + ex.Message, ex));
        return (XmlDocument) null;
      }
    }

    public BinaryObject ReadRawLoanEvent(LoanIdentity loanIdentity)
    {
      BinaryObject binaryObject = (BinaryObject) null;
      switch ((StorageMode) ClientContext.GetCurrent().Settings.GetStorageSetting("DataStore.StorageMode"))
      {
        case StorageMode.PostgresOnly:
        case StorageMode.BothFileSystemPostgresMaster:
          try
          {
            string xmlContent = "";
            using (ILoanDataAccessor service = DataAccessFramework.Runtime.CreateService<ILoanDataAccessor>())
              xmlContent = service.GetLoanEventLogXml(new Guid(loanIdentity.Guid));
            if (!string.IsNullOrEmpty(xmlContent))
              binaryObject = new LoanDataFormatter().SerializeLoanEventLog(new LoanEventLogList(xmlContent));
            TraceLog.WriteVerbose(nameof (LoanFolder), "Read LoanData for loan \"" + loanIdentity.Guid);
            break;
          }
          catch (Exception ex)
          {
            Err.Raise(TraceLevel.Warning, nameof (LoanFolder), new ServerException("Cannot deserialize loan data file '" + loanIdentity.Guid + "': " + ex.Message));
            break;
          }
        default:
          string loanName = loanIdentity.LoanName;
          string dataEventFilePath = this.getDataEventFilePath(loanName);
          try
          {
            using (DataFile latestVersion = FileStore.GetLatestVersion(dataEventFilePath))
            {
              if (!latestVersion.Exists)
                return (BinaryObject) null;
              binaryObject = latestVersion.GetData();
            }
            TraceLog.WriteVerbose(nameof (LoanFolder), "Read LoanData for loan \"" + this.loanFolder.Name + "\\" + loanName + "\" from " + dataEventFilePath);
            break;
          }
          catch (Exception ex)
          {
            Err.Raise(TraceLevel.Warning, nameof (LoanFolder), new ServerException("Cannot deserialize loan data file '" + dataEventFilePath + "': " + ex.Message));
            break;
          }
      }
      return binaryObject;
    }

    public LoanData ReadLoanData(string loanName)
    {
      return this.ReadLoanData(loanName, (ILoanSettings) null);
    }

    public LoanEventLogList ReadLoanEventLog(LoanIdentity loanIdentity)
    {
      using (BinaryObject data = this.ReadRawLoanEvent(loanIdentity))
        return data == null ? new LoanEventLogList() : new LoanDataFormatter().DeserializeLoanEventLog(data);
    }

    public LoanData ReadLoanData(
      string loanName,
      ILoanSettings loanSettings,
      bool onlySavedData = false,
      string loanFolder = null)
    {
      XmlDocument loanDoc = this.ReadLoanXmlData(loanName);
      return loanDoc == null ? (LoanData) null : new LoanDataFormatter().Deserialize(loanDoc, loanSettings, onlySavedData, loanFolder);
    }

    public LoanData ReadLoanDataOfVersion(
      string loanName,
      int version,
      ILoanSettings loanSettings,
      bool onlySavedData = false,
      string loanFolder = null)
    {
      string versionFileLocation = this.GetVersionFileLocation(loanName, new int?(version));
      try
      {
        using (DataFile latestVersion = FileStore.GetLatestVersion(versionFileLocation))
        {
          if (!latestVersion.Exists)
            return (LoanData) null;
          using (BinaryObject data = latestVersion.GetData(loanName, LoanFolder._maxLoanFileSizeForThrowingError, LoanFolder._maxLoanFileSizeForLoggingWarning))
          {
            if (data == null)
              return (LoanData) null;
            TraceLog.WriteVerbose(nameof (LoanFolder), "Read LoanData for loan version \"" + this.loanFolder.Name + "\\" + loanName + "\" from " + versionFileLocation);
            return new LoanDataFormatter().Deserialize(data, loanSettings, onlySavedData, loanFolder);
          }
        }
      }
      catch (Exception ex)
      {
        Err.Raise(TraceLevel.Warning, nameof (LoanFolder), new ServerException("Cannot deserialize loan data file '" + versionFileLocation + "': " + ex.Message));
        return (LoanData) null;
      }
    }

    public LoanData ReadBlankLoanData(string templateName, ILoanSettings loanSettings)
    {
      string blankLoan = Resources.BlankLoan;
      PerformanceMeter current = PerformanceMeter.Current;
      current.AddCheckpoint("ReadBlankLoanData starts...", 604, nameof (ReadBlankLoanData), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\LoanFolder.cs");
      LoanData data = new LoanData(blankLoan, loanSettings);
      new LoanDataFormatter().PrepareSystemSpecificInformationFromPlatform(data);
      current.AddCheckpoint("ReadBlankLoanData optimized finished!", 607, nameof (ReadBlankLoanData), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\LoanFolder.cs");
      return data;
    }

    public void WriteRawLoanData(
      string loanName,
      BinaryObject data,
      bool allowOverwrite,
      ILoanSettings loanSettings = null,
      int? loanVersionNumber = null)
    {
      string dataFilePath = this.getDataFilePath(loanName);
      if (!allowOverwrite && File.Exists(dataFilePath))
        throw new DuplicateObjectException("Cannot serialize loan file to '" + dataFilePath + "'. A loan file already exists at that path.", ObjectType.Loan, (object) new LoanIdentity(this.loanFolder.Name, loanName));
      try
      {
        this.ensureDirectoryExists(dataFilePath);
        if (loanVersionNumber.HasValue)
        {
          int? nullable = loanVersionNumber;
          int num = 1;
          if (nullable.GetValueOrDefault() > num & nullable.HasValue)
          {
            string loanName1 = loanName;
            nullable = loanVersionNumber;
            int? version = nullable.HasValue ? new int?(nullable.GetValueOrDefault() - 1) : new int?();
            using (DataFile dataFile = FileStore.CheckOut(this.GetVersionFileLocation(loanName1, version), MutexAccess.Write))
            {
              if (File.Exists(dataFilePath))
              {
                DataFile latestVersion = FileStore.GetLatestVersion(dataFilePath);
                dataFile.CheckIn(latestVersion.GetData());
              }
            }
          }
        }
        using (DataFile dataFile = FileStore.CheckOut(dataFilePath, MutexAccess.Write))
          dataFile.CheckIn(data);
        using (DataFile dataFile = FileStore.CheckOut(this.GetVersionFileLocation(loanName, loanVersionNumber), MutexAccess.Write))
          dataFile.CheckIn(data);
        TraceLog.WriteVerbose(nameof (LoanFolder), "Wrote LoanData for loan \"" + this.loanFolder.Name + "\\" + loanName + "\" to " + dataFilePath);
      }
      catch (Exception ex)
      {
        Err.Raise(TraceLevel.Warning, nameof (LoanFolder), new ServerException("Cannot serialize loan data file '" + dataFilePath + "': " + ex.Message));
      }
    }

    private string GetVersionFileLocation(string loanName, int? version)
    {
      string dataFilePath = this.getDataFilePath(loanName);
      return dataFilePath.Replace(new FileInfo(dataFilePath).Name, "Versions") + "\\" + string.Format("{0:D5}_", (object) version) + "loan.em";
    }

    public void WriteLoanEventLog(LoanIdentity loanIdentity, LoanEventLogList logList)
    {
      this.WriteRawLoanEventLog(loanIdentity, new LoanDataFormatter().SerializeLoanEventLog(logList));
    }

    public void WriteRawLoanEventLog(LoanIdentity loanIdentity, BinaryObject data)
    {
      StorageMode storageSetting = (StorageMode) ClientContext.GetCurrent().Settings.GetStorageSetting("DataStore.StorageMode");
      Exception exception1 = (Exception) null;
      if (storageSetting != StorageMode.PostgresOnly)
      {
        string loanName = loanIdentity.LoanName;
        string dataEventFilePath = this.getDataEventFilePath(loanName);
        try
        {
          this.ensureDirectoryExists(dataEventFilePath);
          using (DataFile dataFile = FileStore.CheckOut(dataEventFilePath, MutexAccess.Write))
            dataFile.CheckIn(data);
          TraceLog.WriteVerbose(nameof (LoanFolder), "Wrote LoanData for loan \"" + this.loanFolder.Name + "\\" + loanName + "\" to " + dataEventFilePath);
        }
        catch (Exception ex)
        {
          exception1 = (Exception) new ServerException("Cannot serialize loan data file '" + dataEventFilePath + "': " + ex.Message + "\n");
        }
      }
      Exception exception2 = (Exception) null;
      if (storageSetting == StorageMode.PostgresOnly || storageSetting == StorageMode.BothFileSystemPostgresMaster || storageSetting == StorageMode.BothPostgresFileSystemMaster)
      {
        try
        {
          LoanEventLogList loanEventLogList = new LoanDataFormatter().DeserializeLoanEventLog(data);
          using (ILoanDataAccessor service = DataAccessFramework.Runtime.CreateService<ILoanDataAccessor>())
            service.SaveLoanEventLogXml(new Guid(loanIdentity.Guid), loanEventLogList.ToStream().ToString(Encoding.Default, false));
          TraceLog.WriteVerbose(nameof (LoanFolder), "Wrote LoanData for loan \"" + loanIdentity.Guid);
        }
        catch (Exception ex)
        {
          exception1 = (Exception) new ServerException("Cannot serialize loan data file '" + loanIdentity.Guid + "': " + ex.Message);
        }
      }
      if (exception2 == null && exception1 == null)
        return;
      Err.Raise(TraceLevel.Warning, nameof (LoanFolder), new ServerException(exception2.ToString() + "\n" + (object) exception1));
    }

    public void WriteLoanData(string loanName, LoanData data)
    {
      this.WriteLoanData(loanName, data, false);
    }

    public void WriteLoanData(
      string loanName,
      LoanData data,
      bool allowOverwrite,
      ILoanSettings loanSettings = null)
    {
      this.WriteRawLoanData(loanName, new LoanDataFormatter().Serialize(data, false), allowOverwrite, loanSettings, new int?(data.LoanVersionNumber));
    }

    public DateTime GetLoanDataModificationDate(string loanName)
    {
      FileInfo fileInfo = new FileInfo(this.getDataFilePath(loanName));
      return !fileInfo.Exists ? DateTime.MinValue : fileInfo.LastWriteTime;
    }

    public static LoanFolder GetLoanFolder(string loanFolderName, UserInfo currentUser = null)
    {
      try
      {
        LoanFolder loanFolder = (LoanFolder) null;
        string usersAccessibleLoanFolder = loanFolderName;
        if ((UserInfo) null != currentUser && !currentUser.IsSuperAdministrator())
          usersAccessibleLoanFolder = AclGroupLoanAccessor.GetUsersAccessibleLoanFolder(currentUser, loanFolderName);
        DirectoryInfo dirInfo = ((IEnumerable<DirectoryInfo>) new DirectoryInfo(ClientContext.GetCurrent().Settings.LoansDir).GetDirectories()).FirstOrDefault<DirectoryInfo>((System.Func<DirectoryInfo, bool>) (i => i.Name == usersAccessibleLoanFolder));
        if (dirInfo != null)
          loanFolder = new LoanFolder(dirInfo);
        return loanFolder;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFolder), ex);
        return (LoanFolder) null;
      }
    }

    public static LoanFolder[] GetAllLoanFolders(bool includeTrashFolder = true, UserInfo currentUser = null)
    {
      try
      {
        ArrayList arrayList = new ArrayList();
        Set set = (Set) null;
        if ((UserInfo) null != currentUser && !currentUser.IsSuperAdministrator())
        {
          set = (Set) new ListSet();
          foreach (object accessibleLoanFolder in AclGroupLoanAccessor.GetUsersAccessibleLoanFolders(currentUser))
            set.Add(accessibleLoanFolder);
        }
        foreach (DirectoryInfo directory in new DirectoryInfo(ClientContext.GetCurrent().Settings.LoansDir).GetDirectories())
        {
          if ((set == null || set.Contains((object) directory.Name)) && (includeTrashFolder || string.Compare(directory.Name, SystemSettings.TrashFolder, true) != 0))
            arrayList.Add((object) new LoanFolder(directory));
        }
        arrayList.Sort();
        return (LoanFolder[]) arrayList.ToArray(typeof (LoanFolder));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFolder), ex);
        return (LoanFolder[]) null;
      }
    }

    public static string[] GetAllLoanFolderNames(bool includeTrashFolder)
    {
      ArrayList arrayList = new ArrayList();
      DirectoryInfo directoryInfo = new DirectoryInfo(ClientContext.GetCurrent().Settings.LoansDir);
      try
      {
        foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
        {
          if (includeTrashFolder || string.Compare(directory.Name, SystemSettings.TrashFolder, true) != 0)
            arrayList.Add((object) directory.Name);
        }
        arrayList.Sort();
        return (string[]) arrayList.ToArray(typeof (string));
      }
      catch (DirectoryNotFoundException ex)
      {
        TraceLog.WriteVerbose(nameof (LoanFolder), "GetAllLoanFolderNames throw exception: " + ex.Message);
        return (string[]) null;
      }
    }

    public static string[] GetAllLoanFolderNames(bool includeTrashFolder, UserInfo currentUser)
    {
      if (currentUser.IsSuperAdministrator())
        return LoanFolder.GetAllLoanFolderNames(includeTrashFolder);
      ArrayList arrayList = new ArrayList();
      HashSet<string> stringSet = new HashSet<string>();
      foreach (string accessibleLoanFolder in AclGroupLoanAccessor.GetUsersAccessibleLoanFolders(currentUser))
        stringSet.Add(accessibleLoanFolder);
      foreach (DirectoryInfo directory in new DirectoryInfo(ClientContext.GetCurrent().Settings.LoansDir).GetDirectories())
      {
        if (stringSet.Contains(directory.Name) && (includeTrashFolder || string.Compare(directory.Name, SystemSettings.TrashFolder, true) != 0))
          arrayList.Add((object) directory.Name);
      }
      arrayList.Sort();
      return (string[]) arrayList.ToArray(typeof (string));
    }

    public static IList<LoanFolder> GetAllAccessibleFolders(
      bool includeTrashFolder,
      UserInfo currentUser,
      out IList<string> allFolders)
    {
      allFolders = (IList<string>) new List<string>();
      List<LoanFolder> accessibleFolders = new List<LoanFolder>();
      HashSet<string> stringSet = new HashSet<string>();
      foreach (string accessibleLoanFolder in AclGroupLoanAccessor.GetUsersAccessibleLoanFolders(currentUser))
        stringSet.Add(accessibleLoanFolder);
      foreach (DirectoryInfo directory in new DirectoryInfo(ClientContext.GetCurrent().Settings.LoansDir).GetDirectories())
      {
        if (stringSet.Contains(directory.Name) && (includeTrashFolder || string.Compare(directory.Name, SystemSettings.TrashFolder, true) != 0))
          accessibleFolders.Add(new LoanFolder(directory));
        allFolders.Add(directory.Name);
      }
      accessibleFolders.Sort();
      return (IList<LoanFolder>) accessibleFolders;
    }

    [PgReady]
    public static LoanFolderInfo[] GetLoanFolderTypesFromDatabase()
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.Append("select * from LoanFolder");
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
        if (dataRowCollection == null)
          return (LoanFolderInfo[]) null;
        List<LoanFolderInfo> loanFolderInfoList = new List<LoanFolderInfo>();
        for (int index = 0; index < dataRowCollection.Count; ++index)
          loanFolderInfoList.Add(new LoanFolderInfo((string) dataRowCollection[index]["folderName"], (LoanFolderInfo.LoanFolderType) dataRowCollection[index]["folderType"], EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(dataRowCollection[index]["DuplicateLoanCheck"])));
        loanFolderInfoList.Sort();
        return loanFolderInfoList.ToArray();
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder(DBReadReplicaFeature.Pipeline);
      dbQueryBuilder.Append("select * from LoanFolder");
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      if (dataRowCollection1 == null)
        return (LoanFolderInfo[]) null;
      List<LoanFolderInfo> loanFolderInfoList1 = new List<LoanFolderInfo>();
      for (int index = 0; index < dataRowCollection1.Count; ++index)
        loanFolderInfoList1.Add(new LoanFolderInfo((string) dataRowCollection1[index]["folderName"], (LoanFolderInfo.LoanFolderType) dataRowCollection1[index]["folderType"], (bool) dataRowCollection1[index]["DuplicateLoanCheck"]));
      loanFolderInfoList1.Sort();
      return loanFolderInfoList1.ToArray();
    }

    private static LoanFolderInfo[] loanFolderNamesToLoanFolderInfos(string[] folderNames)
    {
      if (folderNames == null)
        return (LoanFolderInfo[]) null;
      if (folderNames.Length == 0)
        return new LoanFolderInfo[0];
      LoanFolderInfo[] typesFromDatabase = LoanFolder.GetLoanFolderTypesFromDatabase();
      List<LoanFolderInfo> loanFolderInfoList = new List<LoanFolderInfo>();
      for (int index = 0; index < folderNames.Length; ++index)
      {
        LoanFolderInfo.LoanFolderType type = LoanFolderInfo.LoanFolderType.Regular;
        bool dupLoanCheck = true;
        bool flag = false;
        foreach (LoanFolderInfo loanFolderInfo in typesFromDatabase)
        {
          if (string.Compare(loanFolderInfo.Name, folderNames[index], StringComparison.OrdinalIgnoreCase) == 0)
          {
            type = loanFolderInfo.Type;
            dupLoanCheck = loanFolderInfo.IncludeInDuplicateLoanCheck;
            flag = true;
            break;
          }
        }
        if (!flag)
          LoanFolder.CreateLoanFolderEntryinDB(folderNames[index]);
        loanFolderInfoList.Add(new LoanFolderInfo(folderNames[index], type, dupLoanCheck));
      }
      loanFolderInfoList.Sort();
      return loanFolderInfoList.ToArray();
    }

    public static void CreateLoanFolderEntryinDB(string folderName)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("declare @folderName varchar(128)");
        dbQueryBuilder.AppendLine("set @folderName = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) folderName));
        dbQueryBuilder.AppendLine("if not exists (select folderName from LoanFolder where folderName = @folderName)\r\n                                begin\r\n                                    insert into LoanFolder (folderName, folderType)\r\n                                    select @folderName, 0\r\n                                end");
        dbQueryBuilder.Execute();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFolder), ex);
      }
    }

    public static LoanFolderInfo[] GetAllLoanFolderInfos(bool includeTrashFolder)
    {
      return LoanFolder.loanFolderNamesToLoanFolderInfos(LoanFolder.GetAllLoanFolderNames(includeTrashFolder));
    }

    public static LoanFolderInfo[] GetAllLoanFolderInfos(
      bool includeTrashFolder,
      UserInfo currentUser)
    {
      return LoanFolder.loanFolderNamesToLoanFolderInfos(LoanFolder.GetAllLoanFolderNames(includeTrashFolder, currentUser));
    }

    public static void SetLoanFolderType(
      string folderName,
      LoanFolderInfo.LoanFolderType folderType,
      UserInfo currentUser)
    {
      DbQueryBuilder sql = new DbQueryBuilder();
      Dictionary<string, IEnumerable<AlertChange>> dictionary = new Dictionary<string, IEnumerable<AlertChange>>();
      switch (folderType)
      {
        case LoanFolderInfo.LoanFolderType.Regular:
          sql.Append("delete from [LoanFolder] where folderName = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) folderName));
          sql.ExecuteNonQuery();
          sql.Reset();
          sql.Declare("@xrefId", "int");
          sql.Declare("@recId", "int");
          LoanFolder loanFolder1 = new LoanFolder(folderName);
          foreach (LoanIdentity content in loanFolder1.GetContents())
          {
            try
            {
              LoanData loanData = loanFolder1.ReadLoanData(content.LoanName);
              if (loanData == null)
              {
                TraceLog.WriteWarning(nameof (LoanFolder), "Reading loan data '" + content.LoanFolder + "/" + content.LoanName + "' returned null loan data.");
              }
              else
              {
                PipelineInfo pipelineInfo = loanData.ToPipelineInfo();
                sql.DeleteFrom(DbAccessManager.GetTable("LoanAlerts"), new DbValue("LoanXRefId", (object) content.XrefId));
                sql.AppendLine("select @xrefId = XRefId from LoanSummary where Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) content.Guid));
                PipelineInfo.Alert[] existingLoanAlerts = Loan.GetExistingLoanAlerts(content.Guid);
                Loan._constructAlertUpdateQuery(sql, (UserInfo) null, content, pipelineInfo, false, existingLoanAlerts);
                dictionary.Add(content.Guid, Loan.GetLoanAlertChanges(content, pipelineInfo, existingLoanAlerts, currentUser.userName));
              }
            }
            catch (Exception ex)
            {
              TraceLog.WriteError(nameof (LoanFolder), "Error reading loan data '" + content.LoanFolder + "/" + content.LoanName + "'. Exception details: " + ex.StackTrace);
            }
          }
          break;
        case LoanFolderInfo.LoanFolderType.Archive:
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.AppendFormat("if exists (select * from [LoanFolder] where folderName = {0}) update [LoanFolder] set folderType = {1} where folderName = {0} else insert into [LoanFolder] (folderName, folderType) values ({0}, {1})\r\n", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) folderName), (object) (int) folderType);
          stringBuilder.AppendFormat("delete from [LoanAlerts] where LoanXRefId in (select A.LoanXRefId from [LoanAlerts] as A inner join [LoanSummary] as S on A.LoanXRefId = S.xRefId WHERE S.LoanFolder = {0})", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) folderName));
          sql.Append(stringBuilder.ToString());
          LoanFolder loanFolder2 = new LoanFolder(folderName);
          foreach (LoanIdentity content in loanFolder2.GetContents())
          {
            LoanData loanData = loanFolder2.ReadLoanData(content.LoanName);
            if (loanData == null)
            {
              TraceLog.WriteWarning(nameof (LoanFolder), "Reading loan data '" + content.LoanFolder + "/" + content.LoanName + "' returned null loan data.");
            }
            else
            {
              PipelineInfo pipelineInfo = loanData.ToPipelineInfo();
              PipelineInfo.Alert[] existingLoanAlerts = Loan.GetExistingLoanAlerts(content.Guid);
              dictionary.Add(content.Guid, Loan.GetLoanAlertChanges(content, pipelineInfo, existingLoanAlerts, currentUser.userName, true));
            }
          }
          break;
        default:
          TraceLog.WriteWarning(nameof (LoanFolder), "Currently do not handle loan foldr type " + folderType.ToString());
          return;
      }
      sql.ExecuteNonQuery();
      foreach (KeyValuePair<string, IEnumerable<AlertChange>> keyValuePair in dictionary)
      {
        string key = keyValuePair.Key;
        foreach (AlertChange alertChange in keyValuePair.Value)
          Loan.PublishLoanAlertChangeKafkaEvent(key, currentUser.Userid, DateTime.Now, new List<AlertChange>()
          {
            alertChange
          });
      }
    }

    public static void SetIncludeInDuplicateLoanCheck(string folderName, bool dupLoanCheck)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendFormat("if exists (select * from [LoanFolder] where folderName = {0}) update [LoanFolder] set DuplicateLoanCheck = {1} where folderName = {0} else insert into [LoanFolder] (folderName,folderType, DuplicateLoanCheck) values ({0},'0', {1})\r\n", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) folderName), dupLoanCheck ? (object) "1" : (object) "0");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static bool GetIncludeInDuplicateLoanCheck(string folderName)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select [DuplicateLoanCheck] from [LoanFolder] where [folderName] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) folderName));
      return EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(dbQueryBuilder.ExecuteScalar(), false);
    }

    public static string GetAllIncludeInDuplicateLoanCheck()
    {
      string[] allLoanFolderNames = LoanFolder.GetAllLoanFolderNames(false);
      LoanFolderInfo[] typesFromDatabase = LoanFolder.GetLoanFolderTypesFromDatabase();
      string duplicateLoanCheck = "'";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select folderName from [LoanFolder] where [DuplicateLoanCheck] = '1'");
      foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        duplicateLoanCheck = duplicateLoanCheck + "'" + EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["folderName"]).Replace("'", "''''") + "'','";
      foreach (string str in allLoanFolderNames)
      {
        string folder = str;
        if (((IEnumerable<LoanFolderInfo>) typesFromDatabase).FirstOrDefault<LoanFolderInfo>((System.Func<LoanFolderInfo, bool>) (item => item.Name == folder)) == null && !duplicateLoanCheck.Contains(folder))
          duplicateLoanCheck = duplicateLoanCheck + "'" + EllieMae.EMLite.DataAccess.SQL.DecodeString((object) folder).Replace("'", "''''") + "'','";
      }
      if (duplicateLoanCheck.Length >= 2)
        duplicateLoanCheck = duplicateLoanCheck.Substring(0, duplicateLoanCheck.Length - 2);
      else if (duplicateLoanCheck.Length == 1)
        duplicateLoanCheck = duplicateLoanCheck.Substring(0, duplicateLoanCheck.Length - 1);
      return duplicateLoanCheck;
    }

    public static string[] GetAllLoanFolderNamesFromDatabase(bool includeTrashFolder)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select distinct LoanFolder from LoanSummary");
      if (!includeTrashFolder)
        dbQueryBuilder.Append(" where LoanFolder != '(Trash)'");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      string[] namesFromDatabase = new string[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        namesFromDatabase[index] = dataRowCollection[index][0].ToString();
      return namesFromDatabase;
    }

    private static DirectoryInfo getLoanFolder(string name)
    {
      return new DirectoryInfo(Path.Combine(ClientContext.GetCurrent().Settings.LoansDir, name));
    }

    private static LoanIdentity[] execFolderContentQuery(EllieMae.EMLite.DataAccess.DbQueryBuilder sql)
    {
      try
      {
        DataRowCollection dataRowCollection = sql.Execute(DbTransactionType.Snapshot);
        LoanIdentity[] loanIdentityArray = new LoanIdentity[dataRowCollection.Count];
        for (int index = 0; index < dataRowCollection.Count; ++index)
        {
          DataRow dataRow = dataRowCollection[index];
          loanIdentityArray[index] = new LoanIdentity(dataRow[nameof (LoanFolder)].ToString(), dataRow["LoanName"].ToString(), dataRow["Guid"].ToString());
        }
        return loanIdentityArray;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFolder), ex);
        return (LoanIdentity[]) null;
      }
    }

    private string getLoanPath(string loanName) => Path.Combine(this.loanFolder.FullName, loanName);

    public string GetFullLoanFilePath(string loanName)
    {
      return Path.Combine(this.getLoanPath(loanName), "loan.em");
    }

    private string getDataFilePath(string loanName)
    {
      return loanName.EndsWith("_loan.em") ? Path.Combine(this.getLoanPath(""), loanName) : Path.Combine(this.getLoanPath(loanName), "loan.em");
    }

    private string getDataEventFilePath(string loanName)
    {
      return Path.Combine(this.getLoanPath(loanName), "LoanEventLog.em");
    }

    private void ensureDirectoryExists(string filePath)
    {
      string directoryName = Path.GetDirectoryName(filePath);
      if (Directory.Exists(directoryName))
        return;
      Directory.CreateDirectory(directoryName);
    }

    private void ensureSafeLoanFolderName(string loanName)
    {
      LoanIdentity loanIdentity = new LoanIdentity(this.name, loanName);
      string[] strArray = loanName.Split('\\');
      if (!(strArray[strArray.Length - 1].Replace(".", "") == ""))
        return;
      Err.Raise(TraceLevel.Error, nameof (LoanFolder), new ServerException("The specified loan name is unsafe for this operation"));
    }

    public static int GetLoanCount()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select count(Guid) from LoanSummary");
      int loanCount = 0;
      try
      {
        loanCount = (int) dbQueryBuilder.ExecuteScalar();
      }
      catch
      {
      }
      return loanCount;
    }

    public static int GetLoanCount(string loanFolder)
    {
      loanFolder = (loanFolder ?? "").Trim();
      if (loanFolder == "")
        return LoanFolder.GetLoanCount();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select count(Guid) from LoanSummary where loanFolder = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanFolder));
      int loanCount = 0;
      try
      {
        loanCount = (int) dbQueryBuilder.ExecuteScalar();
      }
      catch
      {
      }
      return loanCount;
    }

    public static DateTime GetServerCurrentTime() => DateTime.Now;

    public static DateTime GetLoanCountLastUpdatedTime()
    {
      string companySetting = Company.GetCompanySetting("_Encompass", "LastUpdatedTime");
      DateTime minValue = DateTime.MinValue;
      if (!string.IsNullOrEmpty(companySetting))
        minValue = DateTime.Parse(companySetting);
      return minValue;
    }

    public static TimeSpan GetTimeSpanSinceLoanCountLastUpdated()
    {
      return DateTime.Now - LoanFolder.GetLoanCountLastUpdatedTime();
    }

    public static bool getApplyMergeSettings()
    {
      bool result;
      return bool.TryParse(Company.GetCompanySetting("FEATURE", "ENABLECONCURRENTDOCUMENTUPDATE"), out result) && result;
    }

    public static void SetLoanCountLastUpdatedTime()
    {
      Company.SetCompanySetting("_Encompass", "LastUpdatedTime", DateTime.Now.ToString());
    }

    public static ArrayList GetAllLenders(string folderName)
    {
      return LoanFolder.getLoanSummaryNames("Lender", folderName);
    }

    public static ArrayList GetAllInvestors(string folderName)
    {
      return LoanFolder.getLoanSummaryNames("Investor", folderName);
    }

    public static ArrayList GetAllBrokers(string folderName)
    {
      return LoanFolder.getLoanSummaryNames("Broker", folderName);
    }

    private static ArrayList getLoanSummaryNames(string columnName, string folderName)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("SELECT DISTINCT " + columnName + " AS Name FROM LoanSummary");
      if (string.Empty != folderName)
        dbQueryBuilder.Append(" WHERE LoanFolder = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) folderName));
      DataSet dataSet;
      try
      {
        dataSet = dbQueryBuilder.ExecuteSetQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanFolder), ex);
        return (ArrayList) null;
      }
      if (dataSet == null)
        return (ArrayList) null;
      ArrayList loanSummaryNames = new ArrayList();
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
        loanSummaryNames.Add((object) row["Name"].ToString());
      return loanSummaryNames;
    }

    public static void ImportMetaDataFiles(string loanId, string foldername)
    {
      string path = Path.Combine(ClientContext.GetCurrent().Settings.DraftLoansDir, loanId.Substring(0, 2), "{" + loanId + "}");
      string str1 = Path.Combine(ClientContext.GetCurrent().Settings.LoansDir, foldername, "{" + loanId + "}");
      string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
      if (files == null || files.Length == 0)
        return;
      lock (LoanFolder.locker)
      {
        if (!Directory.Exists(str1))
          Directory.CreateDirectory(str1);
        foreach (string str2 in files)
        {
          string fileName = Path.GetFileName(str2);
          string destFileName = Path.Combine(str1, fileName);
          File.Copy(str2, destFileName, true);
        }
      }
    }

    public int CompareTo(object obj)
    {
      if (!(obj is LoanFolder))
        return -1;
      LoanFolder loanFolder = (LoanFolder) obj;
      return string.Compare(this.Name.ToUpper(), loanFolder.Name.ToUpper());
    }
  }
}
