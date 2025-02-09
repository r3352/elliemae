// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.FileStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class FileStore
  {
    private const string className = "FileStore�";
    private const string mutexPrefix = "file://EM/�";
    private const int maxMutexNameLength = 255;
    private static readonly TimeSpan mutexTimeout = TimeSpan.FromSeconds(30.0);

    private FileStore()
    {
    }

    public static DataFile CheckOut(string path) => FileStore.CheckOut(path, MutexAccess.Write);

    public static DataFile getReadOnlyDataFile(string path)
    {
      if ((path ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (FileStore), new ServerException("Invalid file path"));
      DateTime now = DateTime.Now;
      try
      {
        DataFile readOnlyDataFile = new DataFile(path);
        TimeSpan timeSpan = DateTime.Now - now;
        TraceLog.WriteVerbose(nameof (FileStore), "File '" + path + "' checked out with access '" + (object) MutexAccess.Read + "' in " + timeSpan.TotalMilliseconds.ToString("0") + " ms");
        return readOnlyDataFile;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FileStore), ex);
        return (DataFile) null;
      }
    }

    public static DataFile CheckOut(string path, MutexAccess access)
    {
      if ((path ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (FileStore), new ServerException("Invalid file path"));
      DateTime now = DateTime.Now;
      SafeMutex innerLock = new SafeMutex((IClientContext) ClientContext.GetCurrent(), path, access);
      if (!innerLock.WaitOne(FileStore.mutexTimeout))
        Err.Raise(nameof (FileStore), new ServerException("Timeout waiting to obtain mutex on file '" + path + "'"));
      try
      {
        DataFile dataFile = new DataFile(path, innerLock);
        TimeSpan timeSpan = DateTime.Now - now;
        TraceLog.WriteVerbose(nameof (FileStore), "File '" + path + "' checked out with access '" + (object) access + "' in " + timeSpan.TotalMilliseconds.ToString("0") + " ms");
        return dataFile;
      }
      catch (Exception ex)
      {
        innerLock.ReleaseMutex();
        Err.Reraise(nameof (FileStore), ex);
        return (DataFile) null;
      }
    }

    public static DataFile GetLatestVersion(string path)
    {
      if ((path ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (FileStore), new ServerException("Invalid file path"));
      using (DataFile readOnlyDataFile = FileStore.getReadOnlyDataFile(path))
        return readOnlyDataFile;
    }

    public static DataFile CreateNew(string path, BinaryObject data)
    {
      if ((path ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (FileStore), new ServerException("Invalid file path"));
      if (data == null)
        Err.Raise(TraceLevel.Warning, nameof (FileStore), new ServerException("Null argument to CreateNew"));
      using (DataFile dataFile = FileStore.CheckOut(path))
      {
        if (dataFile.Exists)
          Err.Raise(TraceLevel.Warning, nameof (FileStore), new ServerException("Cannot overwrite file with path '" + path + "'"));
        dataFile.CheckIn(data);
        return dataFile;
      }
    }

    public static void Delete(string path)
    {
      if ((path ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (FileStore), new ServerException("Invalid file path"));
      using (DataFile dataFile = FileStore.CheckOut(path))
      {
        if (!dataFile.Exists)
          return;
        dataFile.Delete();
      }
    }

    public static void Update(string path, BinaryObject data)
    {
      if ((path ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (FileStore), new ServerException("Invalid file path"));
      if (data == null)
        throw new ArgumentNullException("Null argument to CreateNew", nameof (data));
      using (DataFile dataFile = FileStore.CheckOut(path))
        dataFile.CheckIn(data);
    }

    public static string[] GetDirectoryFileNames(string directory, bool includeExtensions)
    {
      return FileStore.GetDirectoryFileNames(directory, includeExtensions, false);
    }

    public static string[] GetDirectoryFileNames(
      string directory,
      bool includeExtensions,
      bool decodeFilenames)
    {
      string[] files = Directory.GetFiles(directory);
      string[] directoryFileNames = new string[files.Length];
      for (int index = 0; index < files.Length; ++index)
      {
        directoryFileNames[index] = !includeExtensions ? Path.GetFileNameWithoutExtension(files[index]) : Path.GetFileName(files[index]);
        if (decodeFilenames)
          directoryFileNames[index] = FileSystem.DecodeFilename(directoryFileNames[index]);
      }
      Array.Sort((Array) directoryFileNames, (IComparer) new CaseInsensitiveComparer());
      return directoryFileNames;
    }

    public static string[] GetDirectoryDirNames(string directory)
    {
      string[] directories = Directory.GetDirectories(directory);
      string[] directoryDirNames = new string[directories.Length];
      for (int index = 0; index < directories.Length; ++index)
        directoryDirNames[index] = Path.GetFileName(directories[index]);
      Array.Sort((Array) directoryDirNames, (IComparer) new CaseInsensitiveComparer());
      return directoryDirNames;
    }

    public static FileSystemEntry[] GetDirectoryEntries(
      string directory,
      FileSystemEntry.Types types)
    {
      return FileStore.GetDirectoryEntries(directory, types, (string) null, "\\", false, true, false);
    }

    public static FileSystemEntry[] GetDirectoryEntries(
      string directory,
      FileSystemEntry.Types types,
      string owner,
      string root,
      bool includeExtensions,
      bool decodeFilenames,
      bool recurse,
      bool checkSubFolders = false)
    {
      DateTime now = DateTime.Now;
      string[] directoryDirNames = FileStore.GetDirectoryDirNames(directory);
      string[] directoryFileNames = FileStore.GetDirectoryFileNames(directory, true);
      if (!directory.EndsWith("\\"))
        directory += "\\";
      if (!root.EndsWith("\\"))
        root += "\\";
      ArrayList arrayList = new ArrayList();
      bool flag1 = (types & FileSystemEntry.Types.Folder) > FileSystemEntry.Types.None;
      bool flag2 = (types & FileSystemEntry.Types.File) > FileSystemEntry.Types.None;
      if (flag1 | recurse)
      {
        for (int index = 0; index < directoryDirNames.Length; ++index)
        {
          if (flag1)
          {
            if (decodeFilenames)
            {
              if (checkSubFolders)
                arrayList.Add((object) new FileSystemEntry(root, FileSystem.DecodeFilename(directoryDirNames[index]), FileSystemEntry.Types.Folder, owner, new bool?(FileStore.HasSubfolders(directory + directoryDirNames[index]))));
              else
                arrayList.Add((object) new FileSystemEntry(root, FileSystem.DecodeFilename(directoryDirNames[index]), FileSystemEntry.Types.Folder, owner));
            }
            else if (checkSubFolders)
              arrayList.Add((object) new FileSystemEntry(root, directoryDirNames[index], FileSystemEntry.Types.Folder, owner, new bool?(FileStore.HasSubfolders(directory + directoryDirNames[index]))));
            else
              arrayList.Add((object) new FileSystemEntry(root, directoryDirNames[index], FileSystemEntry.Types.Folder, owner));
          }
          if (recurse)
            arrayList.AddRange((ICollection) FileStore.GetDirectoryEntries(directory + directoryDirNames[index], types, owner, root + directoryDirNames[index] + "\\", includeExtensions, decodeFilenames, recurse, checkSubFolders));
        }
      }
      if (flag2)
      {
        for (int index = 0; index < directoryFileNames.Length; ++index)
        {
          try
          {
            if (string.Compare(Path.GetExtension(directoryFileNames[index]), ".encbak", true) != 0)
            {
              FileInfo fileInfo = new FileInfo(directory + directoryFileNames[index]);
              string str = includeExtensions ? directoryFileNames[index] : Path.GetFileNameWithoutExtension(directoryFileNames[index]);
              if (decodeFilenames)
                arrayList.Add((object) new FileSystemEntry(root, FileSystem.DecodeFilename(str), FileSystemEntry.Types.File, owner, fileInfo.LastWriteTime));
              else
                arrayList.Add((object) new FileSystemEntry(root, str, FileSystemEntry.Types.File, owner, fileInfo.LastWriteTime));
            }
          }
          catch
          {
          }
        }
      }
      TimeSpan timeSpan = DateTime.Now - now;
      TraceLog.WriteVerbose(nameof (FileStore), "Loaded " + (object) arrayList.Count + " dir entries from '" + directory + "' in " + timeSpan.TotalMilliseconds.ToString("0") + " ms");
      return (FileSystemEntry[]) arrayList.ToArray(typeof (FileSystemEntry));
    }

    public static string[] GetSubdirectoryNames(string parentDirectory)
    {
      DirectoryInfo directoryInfo = new DirectoryInfo(parentDirectory);
      if (!directoryInfo.Exists)
        return new string[0];
      DirectoryInfo[] directories = directoryInfo.GetDirectories();
      string[] subdirectoryNames = new string[directories.Length];
      for (int index = 0; index < directories.Length; ++index)
        subdirectoryNames[index] = directories[index].Name;
      Array.Sort((Array) subdirectoryNames, (IComparer) new CaseInsensitiveComparer());
      return subdirectoryNames;
    }

    private static string generateUniqueMutexName(string path)
    {
      string s = path.ToLower().Replace("\\", "/");
      return "file://EM/".Length + s.Length <= (int) byte.MaxValue ? "file://EM/" + s : "file://EM/" + Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(s)));
    }

    public static string GetMD5CheckSum(BinaryObject data)
    {
      using (MD5 md5 = MD5.Create())
      {
        try
        {
          return Convert.ToBase64String(md5.ComputeHash(data.AsStream()));
        }
        catch (Exception ex)
        {
          TraceLog.WriteVerbose(nameof (FileStore), "getMD5CheckSum Error: " + ex.Message);
          return (string) null;
        }
      }
    }

    public static string GetMD5CheckSumAndInsert(BinaryObject data, string path, int fileType)
    {
      string md5CheckSum = FileStore.GetMD5CheckSum(data);
      if (md5CheckSum != null)
        FileInfoDbAccessor.InsertFileInfo(data.Length, md5CheckSum, path, fileType);
      return md5CheckSum;
    }

    private static bool HasSubfolders(string path)
    {
      IEnumerable<string> source = Directory.EnumerateDirectories(path);
      return source != null && source.Any<string>();
    }
  }
}
