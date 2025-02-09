// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.XmlHelperClasses.XDManifest
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.XmlHelperClasses
{
  internal class XDManifest : XmlDocumentBase
  {
    protected XEassemblyIdentity assemblyIdentity;
    protected List<XEdependentAssembly> dependencies;

    public XEassemblyIdentity AssemblyIdentity => this.assemblyIdentity;

    public List<XEdependentAssembly> Dependencies => this.dependencies;

    internal XDManifest(
      string manifestVersion,
      XEassemblyIdentity assemblyIdentity,
      List<XEdependentAssembly> dependencies)
      : base("assembly", manifestVersion)
    {
      this.assemblyIdentity = assemblyIdentity;
      this.dependencies = dependencies;
    }

    internal XDManifest(XmlDocument xmldoc)
      : base(xmldoc)
    {
    }

    private XEdependentAssembly getDependency(string nameWOExt)
    {
      if (this.dependencies == null)
        return (XEdependentAssembly) null;
      foreach (XEdependentAssembly dependency in this.dependencies)
      {
        if (dependency.AssemblyIdentity.Name == nameWOExt)
          return dependency;
      }
      return (XEdependentAssembly) null;
    }

    private void removeDependency(XEdependentAssembly dep)
    {
      if (dep == null)
        return;
      dep.RemoveFromOwnerDocument();
      this.dependencies.Remove(dep);
    }

    public void AddDependency(XEdependentAssembly dependency, bool replaceIfExists)
    {
      if (this.dependencies == null)
        this.dependencies = new List<XEdependentAssembly>();
      string name = dependency.AssemblyIdentity.Name;
      XEdependentAssembly dependency1 = this.getDependency(name);
      if (!replaceIfExists && dependency1 != null)
        throw new Exception(name + ": assembly already exists in the manifest");
      if (dependency1 != null & replaceIfExists)
        dependency1.Copy(dependency);
      else
        this.dependencies.Add(dependency);
    }
  }
}
