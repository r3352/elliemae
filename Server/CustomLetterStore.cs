// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.CustomLetterStore
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
  public class CustomLetterStore
  {
    private const string className = "CustomLetterStore�";

    public static DataFile CheckOut(CustomLetterType letterType, FileSystemEntry entry)
    {
      return FileStore.CheckOut(CustomLetterStore.getCustomLetterPath(letterType, entry));
    }

    public static DataFile GetLatestVersion(CustomLetterType letterType, FileSystemEntry entry)
    {
      using (DataFile latestVersion = FileStore.GetLatestVersion(CustomLetterStore.getCustomLetterPath(letterType, entry)))
        return latestVersion;
    }

    public static DataFile CreateNew(
      CustomLetterType letterType,
      FileSystemEntry entry,
      BinaryObject data)
    {
      if (data == null)
        data = new BinaryObject(new byte[0]);
      using (DataFile dataFile = CustomLetterStore.CheckOut(letterType, entry))
      {
        if (dataFile.Exists)
          Err.Raise(TraceLevel.Warning, nameof (CustomLetterStore), (ServerException) new DuplicateObjectException("Custom letter already exists with this name", ObjectType.CustomLetter, (object) entry));
        dataFile.CreateNew(data);
        string md5CheckSum = FileStore.GetMD5CheckSum(data);
        if (md5CheckSum != null)
        {
          string filePath = entry.ToString();
          FileInfoDbAccessor.InsertFileInfo(data.Length, md5CheckSum, filePath, 7);
        }
        return dataFile;
      }
    }

    public static FileSystemEntry[] GetAllSystemEntries(CustomLetterType type, string userId)
    {
      return FileStore.GetDirectoryEntries(CustomLetterStore.getCustomLetterPath(type, new FileSystemEntry("\\", FileSystemEntry.Types.Folder, userId)), FileSystemEntry.Types.All, userId, "\\", true, true, true);
    }

    public static bool Exists(CustomLetterType type, FileSystemEntry entry)
    {
      if (entry.Path == "\\")
        return true;
      return entry.Type == FileSystemEntry.Types.File ? File.Exists(CustomLetterStore.getCustomLetterPath(type, entry)) : Directory.Exists(CustomLetterStore.getCustomLetterPath(type, entry));
    }

    public static bool ExistsOfAnyType(CustomLetterType type, FileSystemEntry entry)
    {
      return entry.Path == "\\" || File.Exists(CustomLetterStore.getCustomLetterPath(type, entry)) || Directory.Exists(CustomLetterStore.getCustomLetterPath(type, entry));
    }

    public static void Move(CustomLetterType type, FileSystemEntry source, FileSystemEntry target)
    {
      if (source.Type == FileSystemEntry.Types.File)
        target = CustomLetterStore.moveFile(type, source, target);
      else
        CustomLetterStore.moveFolder(type, source, target);
      if (type == CustomLetterType.Generic)
      {
        FormGroup.MoveCustomLetterXRefs(source, target);
        PrintSelectionXrefStore.MoveCustomLetterXRefs(source, target);
      }
      if (type != CustomLetterType.Borrower && CustomLetterType.BizPartner != type)
        return;
      CampaignProvider.UpdateDocumentIds(source, target);
    }

    private static FileSystemEntry moveFile(
      CustomLetterType type,
      FileSystemEntry source,
      FileSystemEntry target)
    {
      if (target.Type == FileSystemEntry.Types.Folder)
        target = new FileSystemEntry(target.Path, source.Name, FileSystemEntry.Types.File, target.Owner);
      string customLetterPath1 = CustomLetterStore.getCustomLetterPath(type, source);
      string customLetterPath2 = CustomLetterStore.getCustomLetterPath(type, target);
      if (!CustomLetterStore.Exists(type, source))
        Err.Raise(TraceLevel.Warning, nameof (CustomLetterStore), (ServerException) new ObjectNotFoundException("Source object does not exist.", ObjectType.CustomLetter, (object) source));
      if (source.IsPublic)
        AclGroupFileAccessor.DeleteFileResource(AclGroupFileAccessor.ConvertToAclFileType(type), source);
      File.Move(customLetterPath1, customLetterPath2);
      using (DataFile dataFile = CustomLetterStore.CheckOut(type, target))
      {
        BinaryObject data = BinaryObject.Marshal(dataFile.GetData());
        string md5CheckSum = FileStore.GetMD5CheckSum(data);
        if (md5CheckSum != null)
          FileInfoDbAccessor.ChangeFilePath(data.Length, md5CheckSum, source.ToString(), target.ToString(), 7);
      }
      return target;
    }

    private static void moveFolder(
      CustomLetterType type,
      FileSystemEntry source,
      FileSystemEntry target)
    {
      string customLetterPath = CustomLetterStore.getCustomLetterPath(type, source);
      Directory.CreateDirectory(CustomLetterStore.getCustomLetterPath(type, target));
      FileSystemEntry[] directoryEntries = FileStore.GetDirectoryEntries(customLetterPath, FileSystemEntry.Types.All, source.Owner, source.Path, true, true, false);
      for (int index = 0; index < directoryEntries.Length; ++index)
      {
        if (directoryEntries[index].Type == FileSystemEntry.Types.File)
          CustomLetterStore.moveFile(type, directoryEntries[index], target);
        else
          CustomLetterStore.moveFolder(type, directoryEntries[index], new FileSystemEntry(target.Path, directoryEntries[index].Name, FileSystemEntry.Types.Folder, target.Owner));
      }
      try
      {
        if (source.IsPublic)
          AclGroupFileAccessor.DeleteFileResource(AclGroupFileAccessor.ConvertToAclFileType(type), source);
        Directory.Delete(customLetterPath, false);
      }
      catch
      {
      }
    }

    public static void Copy(CustomLetterType type, FileSystemEntry source, FileSystemEntry target)
    {
      if (source.Type == FileSystemEntry.Types.File)
        CustomLetterStore.copyFile(type, source, target);
      else
        CustomLetterStore.copyFolder(type, source, target);
    }

    private static void copyFile(
      CustomLetterType type,
      FileSystemEntry source,
      FileSystemEntry target)
    {
      if (target.Type == FileSystemEntry.Types.Folder)
        target = new FileSystemEntry(target.Path, source.Name, FileSystemEntry.Types.File, target.Owner);
      if (Directory.Exists(CustomLetterStore.getCustomLetterPath(type, target)))
        Err.Raise(TraceLevel.Warning, nameof (CustomLetterStore), (ServerException) new DuplicateObjectException("An object with this name already exists", ObjectType.CustomLetter, (object) target));
      string customLetterPath1 = CustomLetterStore.getCustomLetterPath(type, source);
      string customLetterPath2 = CustomLetterStore.getCustomLetterPath(type, target);
      if (!File.Exists(customLetterPath1))
        Err.Raise(TraceLevel.Warning, nameof (CustomLetterStore), (ServerException) new ObjectNotFoundException("Source object not found", ObjectType.CustomLetter, (object) source));
      File.Copy(customLetterPath1, customLetterPath2, true);
      using (DataFile dataFile = CustomLetterStore.CheckOut(type, source))
      {
        BinaryObject data = BinaryObject.Marshal(dataFile.GetData());
        string md5CheckSum = FileStore.GetMD5CheckSum(data);
        if (md5CheckSum == null)
          return;
        FileInfoDbAccessor.CopyFile(data.Length, md5CheckSum, source.ToString(), target.ToString(), 7);
      }
    }

    private static void copyFolder(
      CustomLetterType type,
      FileSystemEntry source,
      FileSystemEntry target)
    {
      string customLetterPath = CustomLetterStore.getCustomLetterPath(type, source);
      Directory.CreateDirectory(CustomLetterStore.getCustomLetterPath(type, target));
      FileSystemEntry[] directoryEntries = FileStore.GetDirectoryEntries(customLetterPath, FileSystemEntry.Types.All, source.Owner, source.Path, true, true, false);
      for (int index = 0; index < directoryEntries.Length; ++index)
      {
        if (directoryEntries[index].Type == FileSystemEntry.Types.File)
          CustomLetterStore.copyFile(type, directoryEntries[index], target);
        else
          CustomLetterStore.copyFolder(type, directoryEntries[index], new FileSystemEntry(target.Path, directoryEntries[index].Name, FileSystemEntry.Types.Folder, target.Owner));
      }
    }

    public static void CreateFolder(CustomLetterType type, FileSystemEntry entry)
    {
      if (File.Exists(CustomLetterStore.getCustomLetterPath(type, entry)))
        Err.Raise(TraceLevel.Warning, nameof (CustomLetterStore), (ServerException) new DuplicateObjectException("Custom Letter with this name already exists", ObjectType.Template, (object) entry));
      Directory.CreateDirectory(CustomLetterStore.getCustomLetterPath(type, entry));
    }

    public static void Delete(CustomLetterType type, FileSystemEntry entry)
    {
      if (entry.Type == FileSystemEntry.Types.File)
      {
        using (DataFile dataFile = CustomLetterStore.CheckOut(type, entry))
        {
          if (!dataFile.Exists)
            return;
          if (entry.IsPublic)
            AclGroupFileAccessor.DeleteFileResource(AclGroupFileAccessor.ConvertToAclFileType(type), entry);
          dataFile.Delete();
          if (type != CustomLetterType.Generic)
            return;
          FormGroup.DeleteCustomLetterXRefs(entry);
          PrintSelectionXrefStore.DeleteCustomLetterXRefs(entry);
        }
      }
      else
      {
        string customLetterPath = CustomLetterStore.getCustomLetterPath(type, entry);
        if (!Directory.Exists(customLetterPath))
          return;
        if (entry.IsPublic)
          AclGroupFileAccessor.DeleteFileResource(AclGroupFileAccessor.ConvertToAclFileType(type), entry);
        Directory.Delete(customLetterPath, false);
      }
    }

    public static FileSystemEntry[] GetDirectoryEntries(
      CustomLetterType type,
      FileSystemEntry parentEntry)
    {
      string customLetterPath = CustomLetterStore.getCustomLetterPath(type, parentEntry);
      if (!Directory.Exists(customLetterPath))
        Err.Raise(TraceLevel.Warning, nameof (CustomLetterStore), (ServerException) new ObjectNotFoundException("Specified folder does not exist", ObjectType.CustomLetter, (object) parentEntry));
      return FileStore.GetDirectoryEntries(customLetterPath, FileSystemEntry.Types.All, parentEntry.Owner, parentEntry.Path, true, true, false);
    }

    public static FileSystemEntry[] GetDirectoryEntries(
      UserInfo user,
      CustomLetterType type,
      FileSystemEntry parentEntry)
    {
      FileSystemEntry[] fsEntries = CustomLetterStore.GetDirectoryEntries(type, parentEntry);
      if (parentEntry.IsPublic)
        fsEntries = AclGroupFileAccessor.ApplyUserAccessRights(user, fsEntries, CustomLetterStore.customLetterTypeToAclFileType(type));
      return fsEntries;
    }

    private static AclFileType customLetterTypeToAclFileType(CustomLetterType type)
    {
      switch (type)
      {
        case CustomLetterType.Borrower:
          return AclFileType.BorrowerCustomLetters;
        case CustomLetterType.BizPartner:
          return AclFileType.BizCustomLetters;
        case CustomLetterType.Generic:
          return AclFileType.CustomPrintForms;
        default:
          return AclFileType.CustomPrintForms;
      }
    }

    public static FileSystemEntry[] GetCustomLettersRecursively(
      CustomLetterType type,
      FileSystemEntry entry)
    {
      string customLetterPath = CustomLetterStore.getCustomLetterPath(type, entry);
      if (!Directory.Exists(customLetterPath))
        Err.Raise(TraceLevel.Warning, nameof (CustomLetterStore), (ServerException) new ObjectNotFoundException("Specified folder does not exist", ObjectType.CustomLetter, (object) entry));
      return FileStore.GetDirectoryEntries(customLetterPath, FileSystemEntry.Types.All, entry.Owner, entry.Path, true, true, true);
    }

    public static FileSystemEntry[] GetAllFileEntries(CustomLetterType type, string userId)
    {
      return FileStore.GetDirectoryEntries(CustomLetterStore.getCustomLetterPath(type, new FileSystemEntry("\\", FileSystemEntry.Types.Folder, userId)), FileSystemEntry.Types.File, userId, "\\", true, true, true);
    }

    public static string getCustomLetterPath(CustomLetterType letterType, FileSystemEntry entry)
    {
      ClientContext current = ClientContext.GetCurrent();
      string encodedPath = entry.GetEncodedPath();
      if (!DataFile.IsValidSubobjectName(encodedPath))
        Err.Raise(TraceLevel.Error, nameof (CustomLetterStore), (ServerException) new ServerArgumentException("Invalid object name: \"" + encodedPath + "\""));
      string str;
      if (entry.Owner == null)
      {
        switch (letterType)
        {
          case CustomLetterType.Borrower:
            str = current.Settings.GetDataFolderPath("BorLetters");
            break;
          case CustomLetterType.BizPartner:
            str = current.Settings.GetDataFolderPath("BizLetters");
            break;
          default:
            str = current.Settings.GetDataFolderPath("CustomLetters");
            break;
        }
      }
      else
      {
        switch (letterType)
        {
          case CustomLetterType.Borrower:
            str = current.Settings.GetUserDataFolderPath(entry.Owner, "BorLetters");
            break;
          case CustomLetterType.BizPartner:
            str = current.Settings.GetUserDataFolderPath(entry.Owner, "BizLetters");
            break;
          default:
            str = current.Settings.GetUserDataFolderPath(entry.Owner, "CustomLetters");
            break;
        }
      }
      string path = SystemUtil.CombinePath(str, encodedPath);
      if (entry.Path == "\\")
        Directory.CreateDirectory(path);
      return path;
    }

    public static BinaryObject GetCustomLetterData(DataFile letter, FileSystemEntry entry)
    {
      BinaryObject data = BinaryObject.Marshal(letter.GetData());
      string path = entry.ToString();
      FileStore.GetMD5CheckSumAndInsert(data, path, 7);
      return data;
    }
  }
}
