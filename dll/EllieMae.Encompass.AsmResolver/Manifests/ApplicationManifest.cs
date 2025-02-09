// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Manifests.ApplicationManifest
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.Utils;
using EllieMae.Encompass.AsmResolver.XmlHelperClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.Manifests
{
  public class ApplicationManifest : ManifestBase
  {
    private XDAppManifest xdAppManifest;
    private Dictionary<string, XEappFileGroup> appFileGroups;
    private Dictionary<string, ResFileInfo> resourceFiles = new Dictionary<string, ResFileInfo>();

    public ApplicationManifest(string xmlString)
      : this(xmlString, false)
    {
    }

    public ApplicationManifest(string xmlStringOrFilePath, bool fromFile)
    {
      XmlDocument xmldoc = new XmlDocument();
      if (fromFile)
        xmldoc.Load(xmlStringOrFilePath);
      else
        xmldoc.LoadXml(xmlStringOrFilePath);
      this.xdAppManifest = new XDAppManifest(xmldoc);
      this.appFileGroups = new Dictionary<string, XEappFileGroup>();
      if (this.xdAppManifest.Dependencies != null)
      {
        foreach (XEdependentAssembly dependency in this.xdAppManifest.Dependencies)
          this.assemblies.Add(dependency.AssemblyIdentity.Name, new AsmFileInfo(dependency));
      }
      if (this.xdAppManifest.AppFileGroupList != null)
      {
        foreach (XEappFileGroup appFileGroup in this.xdAppManifest.AppFileGroupList.AppFileGroups)
          this.appFileGroups.Add(appFileGroup.Name, appFileGroup);
      }
      if (this.xdAppManifest.ResourceFiles == null)
        return;
      foreach (XEfile resourceFile in this.xdAppManifest.ResourceFiles)
        this.resourceFiles.Add(DeployUtils.RemoveDeployExtension(resourceFile.Path).ToLower(), new ResFileInfo(resourceFile));
    }

    public ApplicationManifest(string manifestVersion, string asmIdName, string publishVersion)
      : this(manifestVersion, asmIdName, new Version(publishVersion))
    {
    }

    public ApplicationManifest(string manifestVersion, string asmIdName, Version publishVersion)
    {
      XEassemblyIdentity assemblyIdentity = new XEassemblyIdentity(asmIdName, publishVersion);
      this.xdAppManifest = new XDAppManifest(manifestVersion, assemblyIdentity, (List<XEdependentAssembly>) null, (List<XEfile>) null);
    }

    protected override XmlDocument createXmlDocument() => this.xdAppManifest.CreateDocument();

    internal AsmFileInfo[] GetAsmFileInfosOfGroup(string groupName)
    {
      List<AsmFileInfo> asmFileInfoList = new List<AsmFileInfo>();
      IEnumerator<KeyValuePair<string, AsmFileInfo>> enumerator = (IEnumerator<KeyValuePair<string, AsmFileInfo>>) this.assemblies.GetEnumerator();
      while (enumerator.MoveNext())
      {
        KeyValuePair<string, AsmFileInfo> current = enumerator.Current;
        if (current.Value.Group == groupName)
          asmFileInfoList.Add(current.Value);
      }
      return asmFileInfoList.ToArray();
    }

    internal AsmFileInfo[] GetStartupGroupAsmFileInfos()
    {
      return this.GetAsmFileInfosOfGroup(this.ExecutableName);
    }

    internal ResFileInfo[] GetResFileInfosOfGroup(string groupName)
    {
      List<ResFileInfo> resFileInfoList = new List<ResFileInfo>();
      IEnumerator<KeyValuePair<string, ResFileInfo>> enumerator = (IEnumerator<KeyValuePair<string, ResFileInfo>>) this.resourceFiles.GetEnumerator();
      while (enumerator.MoveNext())
      {
        KeyValuePair<string, ResFileInfo> current = enumerator.Current;
        if (current.Value.Group == groupName)
          resFileInfoList.Add(current.Value);
      }
      return resFileInfoList.ToArray();
    }

    internal ResFileInfo[] GetStartupGroupResFileInfos()
    {
      return this.GetResFileInfosOfGroup(this.ExecutableName);
    }

    internal AppFileInfo[] GetStartupGroupAppFileInfos()
    {
      AsmFileInfo[] groupAsmFileInfos = this.GetStartupGroupAsmFileInfos();
      ResFileInfo[] groupResFileInfos = this.GetStartupGroupResFileInfos();
      if (groupAsmFileInfos == null && groupResFileInfos == null)
        return (AppFileInfo[]) null;
      List<AppFileInfo> appFileInfoList = new List<AppFileInfo>();
      if (groupAsmFileInfos != null)
        appFileInfoList.AddRange((IEnumerable<AppFileInfo>) groupAsmFileInfos);
      if (groupResFileInfos != null)
        appFileInfoList.AddRange((IEnumerable<AppFileInfo>) groupResFileInfos);
      return appFileInfoList.ToArray();
    }

    internal AppFileInfo[] GetGroupAppFileInfos(string groupName)
    {
      AsmFileInfo[] fileInfosOfGroup1 = this.GetAsmFileInfosOfGroup(groupName);
      ResFileInfo[] fileInfosOfGroup2 = this.GetResFileInfosOfGroup(groupName);
      if (fileInfosOfGroup1 == null && fileInfosOfGroup2 == null)
        return (AppFileInfo[]) null;
      List<AppFileInfo> appFileInfoList = new List<AppFileInfo>();
      if (fileInfosOfGroup1 != null)
        appFileInfoList.AddRange((IEnumerable<AppFileInfo>) fileInfosOfGroup1);
      if (fileInfosOfGroup2 != null)
        appFileInfoList.AddRange((IEnumerable<AppFileInfo>) fileInfosOfGroup2);
      return appFileInfoList.ToArray();
    }

    internal Version GetVersion(string nameWithoutExt) => this.assemblies[nameWithoutExt]?.Version;

    internal Version GetFileVersion(string nameWithoutExt)
    {
      return this.assemblies[nameWithoutExt]?.FileVersion;
    }

    internal long GetAsmSize(string nameWithoutExt)
    {
      AsmFileInfo assembly = this.assemblies[nameWithoutExt];
      return assembly == null ? -1L : assembly.Size;
    }

    internal AppFileGroupInfo GetAppFileGroupInfo(string groupName)
    {
      if (!this.appFileGroups.ContainsKey(groupName))
        return (AppFileGroupInfo) null;
      XEappFileGroup appFileGroup = this.appFileGroups[groupName];
      return new AppFileGroupInfo(appFileGroup.Name, appFileGroup.Codebase, appFileGroup.Size);
    }

    internal AppFileGroupInfo GetStartupGroupInfo()
    {
      return this.GetAppFileGroupInfo(this.ExecutableName);
    }

    public override Version AsmIdVersion
    {
      get => this.xdAppManifest.AssemblyIdentity.Version;
      set => this.xdAppManifest.AssemblyIdentity.Version = value;
    }

    public string ExecutableName
    {
      get
      {
        string name = this.xdAppManifest.AssemblyIdentity.Name;
        return name.Substring(0, name.Length - ".exe".Length);
      }
    }

    internal long DownloadBlockSize
    {
      get
      {
        return this.xdAppManifest.AssemblyDeployment == null ? (long) FileUtil.FileDownloadBlockSize : this.xdAppManifest.AssemblyDeployment.DownloadBlockSize;
      }
    }

    internal bool DoBackgroundDownload
    {
      get
      {
        return this.xdAppManifest.AssemblyDeployment != null && this.xdAppManifest.AssemblyDeployment.BackgroundDownload;
      }
    }

    internal string BgDownloadThreadPriority
    {
      get
      {
        return this.xdAppManifest.AssemblyDeployment == null ? (string) null : this.xdAppManifest.AssemblyDeployment.BgDownloadThreadPriority;
      }
    }

    internal int BgDownloadInterval
    {
      get
      {
        return this.xdAppManifest.AssemblyDeployment == null ? 0 : this.xdAppManifest.AssemblyDeployment.BgDownloadInterval;
      }
    }

    internal HashInfo[] GetHashInfos(string filePath)
    {
      filePath = DeployUtils.RemoveDeployExtension(filePath).ToLower();
      return this.resourceFiles[filePath]?.GetHashInfos();
    }

    public ResFileInfo GetResFileInfo(string filePath)
    {
      filePath = filePath.Trim().ToLower();
      return this.resourceFiles == null || !this.resourceFiles.ContainsKey(filePath) ? (ResFileInfo) null : this.resourceFiles[filePath];
    }

    public ResFileInfo[] GetAllResFileInfos()
    {
      if (this.resourceFiles == null)
        return (ResFileInfo[]) null;
      List<ResFileInfo> resFileInfoList = new List<ResFileInfo>();
      foreach (string key in this.resourceFiles.Keys)
        resFileInfoList.Add(this.resourceFiles[key]);
      return resFileInfoList.ToArray();
    }

    public void ChangeAppFileGroupSize(string groupName, long newSize)
    {
      this.xdAppManifest.GetAppFileGroup(groupName).Size = newSize;
    }

    public void AddAppFileGroup(string groupName, string[] files, string codebase, long size)
    {
      List<XEdependentAssembly> dependencies = this.xdAppManifest.Dependencies;
      List<XEfile> resourceFiles = this.xdAppManifest.ResourceFiles;
      foreach (string file in files)
      {
        bool flag = false;
        foreach (XEdependentAssembly xedependentAssembly in dependencies)
        {
          if (file.ToLower() == xedependentAssembly.Codebase.ToLower())
          {
            xedependentAssembly.Group = groupName;
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          foreach (XEfile xefile in resourceFiles)
          {
            if (file.ToLower() == xefile.Path.ToLower())
            {
              xefile.Group = groupName;
              break;
            }
          }
        }
      }
      this.xdAppManifest.AddAppFileGroup(new XEappFileGroup(groupName, codebase, size));
    }

    public AppFileGroupInfo GetAppFileGroup(string groupName)
    {
      XEappFileGroup appFileGroup = this.xdAppManifest.GetAppFileGroup(groupName);
      return appFileGroup == null ? (AppFileGroupInfo) null : new AppFileGroupInfo(groupName, appFileGroup.Codebase, appFileGroup.Size);
    }

    public AppFileGroupInfo[] GetAllAppFileGroups()
    {
      XEappFileGroup[] allAppFileGroups1 = this.xdAppManifest.GetAllAppFileGroups();
      if (allAppFileGroups1 == null)
        return (AppFileGroupInfo[]) null;
      AppFileGroupInfo[] allAppFileGroups2 = new AppFileGroupInfo[allAppFileGroups1.Length];
      for (int index = 0; index < allAppFileGroups2.Length; ++index)
        allAppFileGroups2[index] = new AppFileGroupInfo(allAppFileGroups1[index].Name, allAppFileGroups1[index].Codebase, allAppFileGroups1[index].Size);
      return allAppFileGroups2;
    }

    public void AddOrReplaceDependency(
      string asmName,
      Version asmVersion,
      string codebase,
      long size,
      string fileVersion,
      string group,
      bool onDemandOnly,
      bool replaceIfExists)
    {
      XEassemblyIdentity assemblyIdentity = new XEassemblyIdentity(asmName, asmVersion);
      XEdependentAssembly xedependentAssembly = new XEdependentAssembly(codebase, size, fileVersion, assemblyIdentity, group, onDemandOnly);
      this.xdAppManifest.AddDependency(xedependentAssembly, replaceIfExists);
      if (replaceIfExists && this.assemblies.ContainsKey(asmName))
        this.assemblies.Remove(asmName);
      this.assemblies.Add(asmName, new AsmFileInfo(xedependentAssembly));
    }

    public void AddOrReplaceResFile(
      string appSrcFolder,
      string filePath,
      string digestMethod,
      string digestValue,
      string group,
      bool onDemandOnly,
      bool replaceIfExists)
    {
      long fileSize = FileUtil.GetFileSize(Path.Combine(appSrcFolder, filePath), true);
      XEfile xefile = new XEfile(filePath, fileSize, (string) null, (Version) null, group, onDemandOnly, new List<XEhash>()
      {
        new XEhash(digestMethod, digestValue)
      });
      this.xdAppManifest.AddFile(xefile, replaceIfExists);
      if (replaceIfExists && this.resourceFiles.ContainsKey(filePath.ToLower()))
        this.resourceFiles.Remove(filePath.ToLower());
      this.resourceFiles.Add(filePath, new ResFileInfo(xefile));
    }

    public void AddOrReplaceResFile(
      string appSrcFolder,
      string filePath,
      string fileVersion,
      Version version,
      string group,
      bool onDemandOnly,
      bool replaceIfExists)
    {
      long fileSize = FileUtil.GetFileSize(Path.Combine(appSrcFolder, filePath), true);
      XEfile xefile = new XEfile(filePath, fileSize, fileVersion, version, group, onDemandOnly, (List<XEhash>) null);
      this.xdAppManifest.AddFile(xefile, replaceIfExists);
      if (replaceIfExists && this.resourceFiles.ContainsKey(filePath.ToLower()))
        this.resourceFiles.Remove(filePath.ToLower());
      this.resourceFiles.Add(filePath, new ResFileInfo(xefile));
    }

    public void SetDeploymentInfo(
      long downloadBlockSize,
      string bgDlThreadPriority,
      int bgDownloadInterval)
    {
      this.xdAppManifest.AssemblyDeployment = new XEassemblyDeployment(downloadBlockSize, bgDlThreadPriority, bgDownloadInterval);
    }
  }
}
