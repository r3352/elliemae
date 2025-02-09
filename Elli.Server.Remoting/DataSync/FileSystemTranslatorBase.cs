// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.DataSync.FileSystemTranslatorBase
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Elli.Server.Remoting.DataSync
{
  public class FileSystemTranslatorBase : IFileSystemTranslator
  {
    protected string currentFilePath = "";
    protected string basePath = "";

    public FileSystemTranslatorBase(string basePath, string currentFilePath)
    {
      this.currentFilePath = currentFilePath;
      this.basePath = basePath;
    }

    public virtual bool Synchronized(
      IFileSystemTranslator source,
      Dictionary<string, ConflictResolveMethod> conflictResolvers)
    {
      List<string> fileSystemEntries = source.GetFileSystemEntries();
      string basePath = source.GetBasePath();
      foreach (string str in fileSystemEntries)
      {
        string destinationPath = this.transformToDestinationPath(str, basePath);
        try
        {
          FileInfo fileInfo = new FileInfo(destinationPath);
          if (!fileInfo.Directory.Exists)
            fileInfo.Directory.Create();
          if (!(fileInfo.Extension == ""))
          {
            if (!conflictResolvers.ContainsKey(str))
              File.Copy(str, destinationPath, true);
            else if (conflictResolvers[str] == ConflictResolveMethod.OverWrite)
              File.Copy(str, destinationPath, true);
          }
        }
        catch (Exception ex)
        {
          throw new Exception("Failed to synchronize file system files", ex);
        }
      }
      return true;
    }

    public virtual List<string> CheckConflict(IFileSystemTranslator source)
    {
      List<string> fileSystemEntries1 = source.GetFileSystemEntries();
      List<string> fileSystemEntries2 = this.GetFileSystemEntries(this.transformToDestinationPath(Directory.GetParent(source.GetCurrentPath()).FullName + "\\", source.GetBasePath()));
      List<string> stringList = new List<string>();
      string basePath = source.GetBasePath();
      foreach (string destinationPath in fileSystemEntries2.ToArray())
      {
        string sourcePath = this.transformToSourcePath(destinationPath, basePath);
        if (fileSystemEntries1.Contains(sourcePath))
          stringList.Add(sourcePath);
      }
      return stringList;
    }

    public virtual List<string> GetFileSystemEntries()
    {
      return this.GetFileSystemEntries(this.currentFilePath);
    }

    public virtual List<string> GetFileSystemEntries(string filePath)
    {
      if (Path.GetFileName(filePath) != "")
        return new List<string>((IEnumerable<string>) new string[1]
        {
          filePath
        });
      if (!Directory.Exists(filePath))
        Directory.CreateDirectory(filePath);
      return new List<string>((IEnumerable<string>) Directory.GetFiles(filePath, "*", SearchOption.AllDirectories));
    }

    public virtual string GetBasePath() => this.basePath;

    public virtual string GetCurrentPath() => this.currentFilePath;

    protected string transformToSourcePath(string destinationPath, string sourceBasePath)
    {
      return sourceBasePath + destinationPath.Replace(this.basePath, "");
    }

    protected string transformToDestinationPath(string sourcePath, string sourceBasePath)
    {
      return this.basePath + sourcePath.Replace(sourceBasePath, "");
    }
  }
}
