// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ReportSettingsStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class ReportSettingsStore
  {
    private const string className = "ReportSettingsStore�";

    public static ReportSettingsFile CheckOut(FileSystemEntry entry)
    {
      if (Directory.Exists(ReportSettingsStore.getReportFolderPath(entry)))
        Err.Raise(TraceLevel.Warning, nameof (ReportSettingsStore), (ServerException) new DuplicateObjectException("Folder with this name already exists", ObjectType.Report, (object) entry));
      return new ReportSettingsFile(FileStore.CheckOut(ReportSettingsStore.getReportFilePath(entry)));
    }

    public static ReportSettingsFile GetLatestVersion(FileSystemEntry entry)
    {
      if (Directory.Exists(ReportSettingsStore.getReportFolderPath(entry)))
        Err.Raise(TraceLevel.Warning, nameof (ReportSettingsStore), (ServerException) new DuplicateObjectException("Folder with this name already exists", ObjectType.Report, (object) entry));
      return new ReportSettingsFile(FileStore.GetLatestVersion(ReportSettingsStore.getReportFilePath(entry)));
    }

    public static void Move(FileSystemEntry source, FileSystemEntry target)
    {
      if (source.Type == FileSystemEntry.Types.File)
        ReportSettingsStore.moveFile(source, target);
      else
        ReportSettingsStore.moveFolder(source, target);
    }

    private static void moveFile(FileSystemEntry source, FileSystemEntry target)
    {
      if (target.Type == FileSystemEntry.Types.Folder)
        target = new FileSystemEntry(target.Path, source.Name, FileSystemEntry.Types.File, target.Owner);
      if (ReportSettingsStore.ExistsOfAnyType(target))
        Err.Raise(TraceLevel.Warning, nameof (ReportSettingsStore), (ServerException) new DuplicateObjectException("An object with this name already exists", ObjectType.Report, (object) target));
      string reportFilePath1 = ReportSettingsStore.getReportFilePath(source);
      string reportFilePath2 = ReportSettingsStore.getReportFilePath(target);
      if (!ReportSettingsStore.Exists(source))
        Err.Raise(TraceLevel.Warning, nameof (ReportSettingsStore), (ServerException) new ObjectNotFoundException("Source object not found", ObjectType.Report, (object) source));
      if (source.IsPublic)
        AclGroupFileAccessor.DeleteFileResource(AclFileType.Reports, source);
      File.Move(reportFilePath1, reportFilePath2);
    }

    private static void moveFolder(FileSystemEntry source, FileSystemEntry target)
    {
      string reportFolderPath = ReportSettingsStore.getReportFolderPath(source);
      Directory.CreateDirectory(ReportSettingsStore.getReportFolderPath(target));
      FileSystemEntry[] directoryEntries = FileStore.GetDirectoryEntries(reportFolderPath, FileSystemEntry.Types.All, source.Owner, source.Path, false, true, false);
      for (int index = 0; index < directoryEntries.Length; ++index)
      {
        if (directoryEntries[index].Type == FileSystemEntry.Types.File)
          ReportSettingsStore.moveFile(directoryEntries[index], target);
        else
          ReportSettingsStore.moveFolder(directoryEntries[index], new FileSystemEntry(target.Path, directoryEntries[index].Name, FileSystemEntry.Types.Folder, target.Owner));
      }
      try
      {
        if (source.IsPublic)
          AclGroupFileAccessor.DeleteFileResource(AclFileType.Reports, source);
        Directory.Delete(reportFolderPath, false);
      }
      catch
      {
      }
    }

    public static FileSystemEntry[] GetAllPublicSystemEntries()
    {
      return FileStore.GetDirectoryEntries(ReportSettingsStore.getReportFolderPath(new FileSystemEntry("\\", FileSystemEntry.Types.Folder, (string) null)), FileSystemEntry.Types.All, (string) null, "\\", false, true, true);
    }

    public static void Delete(FileSystemEntry entry)
    {
      if (entry.Type == FileSystemEntry.Types.File)
      {
        using (ReportSettingsFile reportSettingsFile = ReportSettingsStore.CheckOut(entry))
        {
          if (!reportSettingsFile.Exists)
            return;
          if (entry.IsPublic)
            AclGroupFileAccessor.DeleteFileResource(AclFileType.Reports, entry);
          reportSettingsFile.Delete();
        }
      }
      else
      {
        string reportFolderPath = ReportSettingsStore.getReportFolderPath(entry);
        if (!Directory.Exists(reportFolderPath))
          return;
        if (entry.IsPublic)
          AclGroupFileAccessor.DeleteFileResource(AclFileType.Reports, entry);
        Directory.Delete(reportFolderPath, false);
      }
    }

    public static bool Exists(FileSystemEntry entry)
    {
      if (entry.Path == "\\")
        return true;
      return entry.Type == FileSystemEntry.Types.File ? File.Exists(ReportSettingsStore.getReportFilePath(entry)) : Directory.Exists(ReportSettingsStore.getReportFolderPath(entry));
    }

    public static bool ExistsOfAnyType(FileSystemEntry entry)
    {
      return entry.Path == "\\" || File.Exists(ReportSettingsStore.getReportFilePath(entry)) || Directory.Exists(ReportSettingsStore.getReportFolderPath(entry));
    }

    public static void CreateFolder(FileSystemEntry entry)
    {
      if (File.Exists(ReportSettingsStore.getReportFilePath(entry)))
        Err.Raise(TraceLevel.Warning, nameof (ReportSettingsStore), (ServerException) new DuplicateObjectException("Template with this name already exists", ObjectType.Report, (object) entry));
      Directory.CreateDirectory(ReportSettingsStore.getReportFolderPath(entry));
    }

    public static void Copy(FileSystemEntry source, FileSystemEntry target)
    {
      if (source.Type == FileSystemEntry.Types.File)
        ReportSettingsStore.copyFile(source, target);
      else
        ReportSettingsStore.copyFolder(source, target);
    }

    private static void copyFile(FileSystemEntry source, FileSystemEntry target)
    {
      if (target.Type == FileSystemEntry.Types.Folder)
        target = new FileSystemEntry(target.Path, source.Name, FileSystemEntry.Types.File, target.Owner);
      if (Directory.Exists(ReportSettingsStore.getReportFolderPath(target)))
        Err.Raise(TraceLevel.Warning, nameof (ReportSettingsStore), (ServerException) new DuplicateObjectException("An object with this name already exists", ObjectType.Report, (object) target));
      string reportFilePath1 = ReportSettingsStore.getReportFilePath(source);
      string reportFilePath2 = ReportSettingsStore.getReportFilePath(target);
      if (!File.Exists(reportFilePath1))
        Err.Raise(TraceLevel.Warning, nameof (ReportSettingsStore), (ServerException) new ObjectNotFoundException("Source object not found", ObjectType.Report, (object) source));
      File.Copy(reportFilePath1, reportFilePath2, true);
    }

    private static void copyFolder(FileSystemEntry source, FileSystemEntry target)
    {
      string reportFolderPath = ReportSettingsStore.getReportFolderPath(source);
      Directory.CreateDirectory(ReportSettingsStore.getReportFolderPath(target));
      FileSystemEntry[] directoryEntries = FileStore.GetDirectoryEntries(reportFolderPath, FileSystemEntry.Types.All, source.Owner, source.Path, false, true, false);
      for (int index = 0; index < directoryEntries.Length; ++index)
      {
        if (directoryEntries[index].Type == FileSystemEntry.Types.File)
          ReportSettingsStore.copyFile(directoryEntries[index], target);
        else
          ReportSettingsStore.copyFolder(directoryEntries[index], new FileSystemEntry(target.Path, directoryEntries[index].Name, FileSystemEntry.Types.Folder, target.Owner));
      }
    }

    public static FileSystemEntry[] GetDirectoryEntries(FileSystemEntry parentEntry)
    {
      string reportFolderPath = ReportSettingsStore.getReportFolderPath(parentEntry);
      if (!Directory.Exists(reportFolderPath))
        Err.Raise(TraceLevel.Warning, nameof (ReportSettingsStore), (ServerException) new ObjectNotFoundException("Specified folder does not exist", ObjectType.Report, (object) parentEntry));
      return FileStore.GetDirectoryEntries(reportFolderPath, FileSystemEntry.Types.All, parentEntry.Owner, parentEntry.Path, false, true, false);
    }

    public static FileSystemEntry[] GetDirectoryEntries(UserInfo user, FileSystemEntry parentEntry)
    {
      FileSystemEntry[] fsEntries = ReportSettingsStore.GetDirectoryEntries(parentEntry);
      if (parentEntry.IsPublic)
        fsEntries = AclGroupFileAccessor.ApplyUserAccessRights(user, fsEntries, AclFileType.Reports);
      return fsEntries;
    }

    public static FileSystemEntry[] GetAllFileEntries(string userId)
    {
      return FileStore.GetDirectoryEntries(ReportSettingsStore.getReportFolderPath(new FileSystemEntry("\\", FileSystemEntry.Types.Folder, userId)), FileSystemEntry.Types.File, userId, "\\", false, true, true);
    }

    private static string getReportObjectPath(FileSystemEntry entry)
    {
      return entry.Type == FileSystemEntry.Types.File ? ReportSettingsStore.getReportFilePath(entry) : ReportSettingsStore.getReportFolderPath(entry);
    }

    public static string getReportFilePath(FileSystemEntry entry)
    {
      return ReportSettingsStore.getReportFolderPath(entry) + ".xml";
    }

    public static string getReportFolderPath(FileSystemEntry entry)
    {
      string encodedPath = entry.GetEncodedPath();
      if (!DataFile.IsValidSubobjectName(encodedPath))
        Err.Raise(TraceLevel.Error, nameof (ReportSettingsStore), (ServerException) new ServerArgumentException("Invalid object name: \"" + encodedPath + "\""));
      IServerSettings settings = ClientContext.GetCurrent().Settings;
      string path = SystemUtil.CombinePath(entry.Owner != null ? settings.GetUserDataFolderPath(entry.Owner, "Reports") : settings.GetDataFolderPath("Reports"), encodedPath);
      if (entry.Path == "\\")
        Directory.CreateDirectory(path);
      return path;
    }
  }
}
