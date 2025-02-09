// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Manifests.DeploymentManifest
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
  public class DeploymentManifest : ManifestBase
  {
    private XDDeployManifest xdDeployManifest;

    public DeploymentManifest(string xmlString)
      : this(xmlString, false)
    {
    }

    public DeploymentManifest(string xmlStringOrFilePath, bool fromFile)
    {
      XmlDocument xmldoc = new XmlDocument();
      if (fromFile)
        xmldoc.Load(xmlStringOrFilePath);
      else
        xmldoc.LoadXml(xmlStringOrFilePath);
      this.xdDeployManifest = new XDDeployManifest(xmldoc);
      if (this.xdDeployManifest.Dependencies == null)
        return;
      foreach (XEdependentAssembly dependency in this.xdDeployManifest.Dependencies)
        this.assemblies.Add(dependency.AssemblyIdentity.Name, new AsmFileInfo(dependency));
    }

    public DeploymentManifest(string manifestVersion, string asmIdName, string publishVersion)
      : this(manifestVersion, asmIdName, new Version(publishVersion))
    {
    }

    public DeploymentManifest(string manifestVersion, string asmIdName, Version publishVersion)
    {
      XEassemblyIdentity asmIdentity = new XEassemblyIdentity(asmIdName, publishVersion);
      this.xdDeployManifest = new XDDeployManifest(manifestVersion, asmIdentity, (List<XEdependentAssembly>) null);
    }

    protected override XmlDocument createXmlDocument() => this.xdDeployManifest.CreateDocument();

    private string getAppManifestPath(string nameWOExt)
    {
      return (this.GetAsmFileInfo(nameWOExt) ?? throw new Exception("Cannot find application manifest information for application '" + nameWOExt + "' in deployment manifest " + this.Name + ".")).Codebase;
    }

    public string GetPubLocAppFolder(string nameWOExt)
    {
      string appManifestPath = this.getAppManifestPath(nameWOExt);
      int length = appManifestPath.LastIndexOf("\\");
      return length > 0 ? appManifestPath.Substring(0, length) : "";
    }

    internal string GetUACAppFolder(string nameWOExt)
    {
      return DeployUtils.RemoveVersionSuffix(this.GetPubLocAppFolder(nameWOExt));
    }

    public string Name => this.xdDeployManifest.AssemblyIdentity.Name;

    public override Version AsmIdVersion
    {
      get => this.xdDeployManifest.AssemblyIdentity.Version;
      set => this.xdDeployManifest.AssemblyIdentity.Version = value;
    }

    public void AddOrReplaceDependency(string exeNameWOExt, string codebase, long size)
    {
      bool onDemandOnly = false;
      XEassemblyIdentity assemblyIdentity = new XEassemblyIdentity(exeNameWOExt, (Version) null);
      XEdependentAssembly xedependentAssembly = new XEdependentAssembly(codebase, size, (string) null, assemblyIdentity, (string) null, onDemandOnly);
      this.xdDeployManifest.AddDependency(xedependentAssembly, true);
      if (this.assemblies.ContainsKey(exeNameWOExt))
        this.assemblies.Remove(exeNameWOExt);
      this.assemblies.Add(exeNameWOExt, new AsmFileInfo(xedependentAssembly));
    }

    public void ChangeAppFolder(string exeNameWOExt, string newAppFolderFullPath)
    {
      if (!this.assemblies.ContainsKey(exeNameWOExt))
        return;
      string fileName = Path.GetFileName(newAppFolderFullPath);
      AsmFileInfo assembly = this.assemblies[exeNameWOExt];
      int startIndex = assembly.Codebase.IndexOf('\\');
      string str = assembly.Codebase.Substring(startIndex);
      string codebase = fileName + str;
      long length = new FileInfo(Path.Combine(newAppFolderFullPath, exeNameWOExt + ResolverConsts.AppManifestExt)).Length;
      this.AddOrReplaceDependency(exeNameWOExt, codebase, length);
    }

    public void ChangeAppFolder(string newAppFolderFullPath)
    {
      string[] array = new string[this.assemblies.Keys.Count];
      this.assemblies.Keys.CopyTo(array, 0);
      foreach (string exeNameWOExt in array)
        this.ChangeAppFolder(exeNameWOExt, newAppFolderFullPath);
    }
  }
}
