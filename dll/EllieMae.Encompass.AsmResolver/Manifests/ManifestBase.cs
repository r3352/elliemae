// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Manifests.ManifestBase
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.Manifests
{
  public abstract class ManifestBase
  {
    protected Dictionary<string, AsmFileInfo> assemblies = new Dictionary<string, AsmFileInfo>();

    public abstract Version AsmIdVersion { get; set; }

    protected abstract XmlDocument createXmlDocument();

    public void Save(string filePath, Encoding encoding)
    {
      using (new FileMutex(filePath))
      {
        using (XmlTextWriter w = new XmlTextWriter(filePath, encoding))
        {
          w.Formatting = Formatting.Indented;
          this.createXmlDocument().Save((XmlWriter) w);
        }
      }
    }

    public AsmFileInfo GetAsmFileInfo(string asmName)
    {
      asmName = asmName.Trim();
      if (this.assemblies != null && this.assemblies.ContainsKey(asmName))
        return this.assemblies[asmName];
      if (AssemblyResolver.Instance != null)
        AssemblyResolver.Instance.WriteToEventLog(asmName + ": cannot find assembly info in the manifest.", EventLogEntryType.Error);
      return (AsmFileInfo) null;
    }

    public AsmFileInfo[] GetAllAsmFileInfos()
    {
      if (this.assemblies == null)
        return (AsmFileInfo[]) null;
      List<AsmFileInfo> asmFileInfoList = new List<AsmFileInfo>();
      foreach (string key in this.assemblies.Keys)
        asmFileInfoList.Add(this.assemblies[key]);
      return asmFileInfoList.ToArray();
    }

    public void ChangeDependentAssemblySize(string name, long size)
    {
      this.GetAsmFileInfo(name).Size = size;
    }
  }
}
