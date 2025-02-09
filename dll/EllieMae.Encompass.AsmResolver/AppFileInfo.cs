// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.AppFileInfo
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.Utils;
using System;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace EllieMae.Encompass.AsmResolver
{
  public abstract class AppFileInfo
  {
    protected readonly string basePath;
    public string FilePath;
    protected long size = -1;
    public readonly Version FileVersion;
    public readonly Version Version;
    public readonly string Group;
    public readonly bool OnDemandOnly;

    public abstract long Size { get; set; }

    public AppFileInfo(
      string filePath,
      string size,
      string fileVersion,
      Version version,
      string group,
      bool onDemandOnly)
      : this(filePath, long.Parse(size), fileVersion, version, group, onDemandOnly)
    {
    }

    public AppFileInfo(
      string filePath,
      long size,
      string fileVersion,
      Version version,
      string group,
      bool onDemandOnly)
    {
      this.FilePath = filePath;
      this.size = size;
      if (this.FilePath.ToLower().EndsWith("presentationcore.dll") || this.FilePath.ToLower().EndsWith("presentationcore.dll.deploy.zip"))
      {
        int length = fileVersion.IndexOf(" ");
        if (length > 0)
          fileVersion = fileVersion.Substring(0, length);
      }
      try
      {
        if (!BasicUtils.IsNullOrEmpty(fileVersion))
          this.FileVersion = new Version(fileVersion);
      }
      catch (Exception ex)
      {
        int num = (int) AssemblyResolver.DisplayOrLogMessage(EventLogEntryType.Error, "Error getting file version of file " + this.FilePath + " with file version " + fileVersion + "\r\n" + ex.Message);
      }
      this.Version = version;
      this.Group = group;
      this.OnDemandOnly = onDemandOnly;
    }

    public AppFileInfo(string basePath, string filePath)
    {
      this.basePath = basePath;
      this.FilePath = filePath;
      string str = Path.Combine(basePath, filePath);
      if (!File.Exists(str))
        return;
      this.size = FileUtil.GetFileSize(str, false);
      if (!CLRUtil.HasAssemblyExt(str))
        return;
      try
      {
        this.Version = FileUtil.GetAssemblyVersion(str);
        this.FileVersion = FileUtil.GetFileVersion(str);
      }
      catch
      {
        this.Version = (Version) null;
        this.FileVersion = (Version) null;
      }
    }
  }
}
