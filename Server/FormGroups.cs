// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.FormGroups
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
  public sealed class FormGroups
  {
    private const string className = "FormGroups�";

    public static FormGroup CheckOut(FileSystemEntry entry)
    {
      if (Directory.Exists(FormGroup.GetFolderPath(entry)))
        Err.Raise(TraceLevel.Warning, nameof (FormGroups), (ServerException) new DuplicateObjectException("Folder with this name already exists", ObjectType.FormGroup, (object) entry));
      return new FormGroup(entry);
    }

    public static FormGroup GetLatestVersion(FileSystemEntry entry)
    {
      using (FormGroup latestVersion = FormGroups.CheckOut(entry))
        return latestVersion;
    }

    public static void CreateNew(FileSystemEntry entry, FormInfo[] forms)
    {
      if (FormGroups.ExistsOfAnyType(entry))
        Err.Raise(TraceLevel.Warning, nameof (FormGroups), (ServerException) new DuplicateObjectException("An object with this name already exists", ObjectType.FormGroup, (object) entry));
      using (FormGroup formGroup = FormGroups.CheckOut(entry))
        formGroup.CheckIn(forms);
    }

    public static void SaveForms(FileSystemEntry entry, FormInfo[] forms)
    {
      using (FormGroup formGroup = FormGroups.CheckOut(entry))
        formGroup.CheckIn(forms);
    }

    public static bool Exists(FileSystemEntry entry)
    {
      if (entry.Path == "\\")
        return true;
      return entry.Type == FileSystemEntry.Types.File ? File.Exists(FormGroup.GetFilePath(entry)) : Directory.Exists(FormGroup.GetFolderPath(entry));
    }

    public static bool ExistsOfAnyType(FileSystemEntry entry)
    {
      return entry.Path == "\\" || File.Exists(FormGroup.GetFilePath(entry)) || Directory.Exists(FormGroup.GetFolderPath(entry));
    }

    public static void Move(FileSystemEntry source, FileSystemEntry target)
    {
      if (source.Type == FileSystemEntry.Types.File)
        target = FormGroups.moveFile(source, target);
      else
        FormGroups.moveFolder(source, target);
      FormGroup.MoveFormGroupXRefs(source, target);
      PrintSelectionXrefStore.MoveFormGroupXRefs(source, target);
    }

    private static FileSystemEntry moveFile(FileSystemEntry source, FileSystemEntry target)
    {
      if (target.Type == FileSystemEntry.Types.Folder)
        target = new FileSystemEntry(target.Path, source.Name, FileSystemEntry.Types.File, target.Owner);
      if (FormGroups.ExistsOfAnyType(target))
        Err.Raise(TraceLevel.Warning, nameof (FormGroups), (ServerException) new DuplicateObjectException("An object with this name already exists", ObjectType.FormGroup, (object) target));
      string filePath1 = FormGroup.GetFilePath(source);
      string filePath2 = FormGroup.GetFilePath(target);
      if (!FormGroups.Exists(source))
        Err.Raise(TraceLevel.Warning, nameof (FormGroups), (ServerException) new ObjectNotFoundException("An object with this name already exists", ObjectType.FormGroup, (object) filePath1));
      if (source.IsPublic)
        AclGroupFileAccessor.DeleteFileResource(AclFileType.PrintGroups, source);
      File.Move(filePath1, filePath2);
      return target;
    }

    private static void moveFolder(FileSystemEntry source, FileSystemEntry target)
    {
      string folderPath = FormGroup.GetFolderPath(source);
      Directory.CreateDirectory(FormGroup.GetFolderPath(target));
      FileSystemEntry[] directoryEntries = FileStore.GetDirectoryEntries(folderPath, FileSystemEntry.Types.All, source.Owner, source.Path, false, true, false);
      for (int index = 0; index < directoryEntries.Length; ++index)
      {
        if (directoryEntries[index].Type == FileSystemEntry.Types.File)
          FormGroups.moveFile(directoryEntries[index], target);
        else
          FormGroups.moveFolder(directoryEntries[index], new FileSystemEntry(target.Path, directoryEntries[index].Name, FileSystemEntry.Types.Folder, target.Owner));
      }
      try
      {
        if (source.IsPublic)
          AclGroupFileAccessor.DeleteFileResource(AclFileType.PrintGroups, source);
        Directory.Delete(folderPath, false);
      }
      catch
      {
      }
    }

    public static void Copy(FileSystemEntry source, FileSystemEntry target)
    {
      if (source.Type == FileSystemEntry.Types.File)
        FormGroups.copyFile(source, target);
      else
        FormGroups.copyFolder(source, target);
    }

    private static void copyFile(FileSystemEntry source, FileSystemEntry target)
    {
      if (target.Type == FileSystemEntry.Types.Folder)
        target = new FileSystemEntry(target.Path, source.Name, FileSystemEntry.Types.File, target.Owner);
      FormInfo[] newForms = (FormInfo[]) null;
      using (FormGroup formGroup = FormGroups.CheckOut(source))
        newForms = formGroup.Forms;
      using (FormGroup formGroup = FormGroups.CheckOut(target))
        formGroup.CheckIn(newForms);
    }

    private static void copyFolder(FileSystemEntry source, FileSystemEntry target)
    {
      string folderPath = FormGroup.GetFolderPath(source);
      Directory.CreateDirectory(FormGroup.GetFolderPath(target));
      FileSystemEntry[] directoryEntries = FileStore.GetDirectoryEntries(folderPath, FileSystemEntry.Types.All, source.Owner, source.Path, false, true, false);
      for (int index = 0; index < directoryEntries.Length; ++index)
      {
        if (directoryEntries[index].Type == FileSystemEntry.Types.File)
          FormGroups.copyFile(directoryEntries[index], target);
        else
          FormGroups.copyFolder(directoryEntries[index], new FileSystemEntry(target.Path, directoryEntries[index].Name, FileSystemEntry.Types.Folder, target.Owner));
      }
    }

    public static void CreateFolder(FileSystemEntry entry)
    {
      if (File.Exists(FormGroup.GetFilePath(entry)))
        Err.Raise(TraceLevel.Warning, nameof (FormGroups), (ServerException) new DuplicateObjectException("An object with this name already exists", ObjectType.FormGroup, (object) entry));
      Directory.CreateDirectory(FormGroup.GetFolderPath(entry));
    }

    public static void Delete(FileSystemEntry entry)
    {
      if (entry.Type == FileSystemEntry.Types.File)
      {
        using (FormGroup formGroup = FormGroups.CheckOut(entry))
        {
          if (entry.IsPublic)
            AclGroupFileAccessor.DeleteFileResource(AclFileType.PrintGroups, entry);
          formGroup.Delete();
        }
      }
      else
        Directory.Delete(FormGroup.GetFolderPath(entry), false);
    }

    public static FileSystemEntry[] GetDirectoryEntries(FileSystemEntry parentEntry)
    {
      string folderPath = FormGroup.GetFolderPath(parentEntry);
      if (!Directory.Exists(folderPath))
        Err.Raise(TraceLevel.Warning, nameof (FormGroups), (ServerException) new ObjectNotFoundException("Specified folder does not exist", ObjectType.FormGroup, (object) parentEntry));
      return FileStore.GetDirectoryEntries(folderPath, FileSystemEntry.Types.All, parentEntry.Owner, parentEntry.Path, false, true, false);
    }

    public static FileSystemEntry[] GetDirectoryEntries(UserInfo user, FileSystemEntry parentEntry)
    {
      FileSystemEntry[] directoryEntries = FormGroups.GetDirectoryEntries(parentEntry);
      if (parentEntry.IsPublic)
        directoryEntries = AclGroupFileAccessor.ApplyUserAccessRights(user, directoryEntries, AclFileType.PrintGroups);
      FormGroups.populateTemplateProperties(directoryEntries);
      return directoryEntries;
    }

    public static FileSystemEntry[] GetAllFileEntries(string userId)
    {
      FileSystemEntry[] directoryEntries = FileStore.GetDirectoryEntries(FormGroup.GetFolderPath(new FileSystemEntry("\\", FileSystemEntry.Types.Folder, userId)), FileSystemEntry.Types.File, userId, "\\", false, true, true);
      FormGroups.populateTemplateProperties(directoryEntries);
      return directoryEntries;
    }

    public static FileSystemEntry[] GetAllPublicSystemEntries(bool populateProperties)
    {
      FileSystemEntry[] directoryEntries = FileStore.GetDirectoryEntries(FormGroup.GetFolderPath(new FileSystemEntry("\\", FileSystemEntry.Types.Folder, (string) null)), FileSystemEntry.Types.All, (string) null, "\\", false, true, true);
      if (populateProperties)
        FormGroups.populateTemplateProperties(directoryEntries);
      return directoryEntries;
    }

    private static void populateTemplateProperties(FileSystemEntry[] fs)
    {
      for (int index = 0; index < fs.Length; ++index)
      {
        FileSystemEntry f = fs[index];
        if (f.Type == FileSystemEntry.Types.File)
        {
          using (FormGroup latestVersion = FormGroups.GetLatestVersion(f))
          {
            if (!latestVersion.Exists)
              TraceLog.WriteError(nameof (FormGroups), "Form Group '" + f.Name + "' reported as non-existent");
            if (!f.Properties.ContainsKey((object) "Description"))
              f.Properties.Add((object) "Description", (object) "");
            f.Properties[(object) "Description"] = (object) latestVersion.FormDescription;
          }
        }
      }
    }
  }
}
