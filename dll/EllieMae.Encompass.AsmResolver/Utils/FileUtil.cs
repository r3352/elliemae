// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Utils.FileUtil
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.Utils
{
  public class FileUtil
  {
    public static long GetFileSize(string fullPath, bool throwExceptionIfFileMissing)
    {
      if (File.Exists(fullPath))
        return new FileInfo(fullPath).Length;
      if (throwExceptionIfFileMissing)
        throw new Exception(fullPath + ": file not found");
      return -1;
    }

    public static Version GetFileVersion(string fullPath)
    {
      if (!File.Exists(fullPath))
        return (Version) null;
      try
      {
        string version = FileVersionInfo.GetVersionInfo(fullPath).FileVersion;
        if (fullPath.ToLower().EndsWith("presentationcore.dll"))
        {
          int length = version.IndexOf(" ");
          if (length > 0)
            version = version.Substring(0, length);
        }
        return new Version(version);
      }
      catch
      {
        return (Version) null;
      }
    }

    public static string GetFileVersionAsString(string fullPath)
    {
      Version fileVersion = FileUtil.GetFileVersion(fullPath);
      return fileVersion == (Version) null ? (string) null : fileVersion.ToString();
    }

    public static Version GetAssemblyVersion(string fullPath)
    {
      if (!File.Exists(fullPath))
        return (Version) null;
      try
      {
        return AssemblyName.GetAssemblyName(fullPath).Version;
      }
      catch
      {
        return (Version) null;
      }
    }

    public static int FileDownloadBlockSize => 10485760;
  }
}
