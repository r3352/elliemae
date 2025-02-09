// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.AsmFileInfo
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.DeploymentHandlers;
using EllieMae.Encompass.AsmResolver.Utils;
using EllieMae.Encompass.AsmResolver.XmlHelperClasses;
using System;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace EllieMae.Encompass.AsmResolver
{
  public class AsmFileInfo : AppFileInfo
  {
    private readonly XEdependentAssembly xeDependentAssembly;
    public readonly string Name;

    public string Codebase
    {
      get => this.FilePath;
      set => this.FilePath = value;
    }

    public override long Size
    {
      get => this.size;
      set
      {
        this.size = value;
        if (this.xeDependentAssembly == null)
          return;
        this.xeDependentAssembly.Size = this.size;
      }
    }

    internal AsmFileInfo(XEdependentAssembly xeDependentAssembly)
      : this(xeDependentAssembly.AssemblyIdentity.Name, xeDependentAssembly.AssemblyIdentity.Version, xeDependentAssembly.FileVersion, xeDependentAssembly.Size.ToString() ?? "", xeDependentAssembly.Codebase, xeDependentAssembly.Group, xeDependentAssembly.OnDemandOnly)
    {
      this.xeDependentAssembly = xeDependentAssembly;
    }

    private AsmFileInfo(
      string asmName,
      Version version,
      string fileVersion,
      string size,
      string codebase,
      string group,
      bool onDemandOnly)
      : base(codebase, size, fileVersion, version, group, onDemandOnly)
    {
      this.Name = asmName;
    }

    public AsmFileInfo(string basePath, string asmPath)
      : base(basePath, asmPath)
    {
      this.Name = Path.GetFileNameWithoutExtension(asmPath);
    }

    public bool IsSameVersion(AsmFileInfo asmFileInfo)
    {
      if (asmFileInfo == null)
        throw new Exception("Null assembly file info while trying to compare the assembly versions. The assembly file info may not be present in the application manifest.");
      return this.isSameVersion(asmFileInfo.Version, asmFileInfo.FileVersion);
    }

    private bool isSameVersion(Version version, Version fileVersion)
    {
      return this.FileVersion == fileVersion;
    }

    public void AddDeployZipExtToCodebase()
    {
      if (this.xeDependentAssembly != null)
        this.xeDependentAssembly.AddDeployZipExtToCodebase();
      this.FilePath = this.xeDependentAssembly.Codebase;
    }

    public bool isCertExpired(string localPath)
    {
      bool flag = false;
      if (localPath == string.Empty)
        throw new Exception("Null assembly file info while trying to compare the assembly versions. The assembly file info may not be present in the application manifest.");
      try
      {
        string path2 = DeployUtils.RemoveDeployExtension(this.FilePath);
        string str = Path.Combine(localPath, path2);
        if (File.Exists(str))
          flag = CertificateHelper.isCertExpired(str);
      }
      catch
      {
        AssemblyResolver.WriteToEventLogS("Verify the cert on :" + this.FilePath, EventLogEntryType.Information);
        flag = false;
      }
      return flag;
    }
  }
}
