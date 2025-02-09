// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.XmlHelperClasses.XEdependentAssembly
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.Utils;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.XmlHelperClasses
{
  internal class XEdependentAssembly : XmlElementBase
  {
    private string codebase;
    public long Size = -1;
    public string FileVersion;
    public string Group;
    public bool OnDemandOnly;
    public XEassemblyIdentity AssemblyIdentity;

    public string Codebase => this.codebase;

    public XEdependentAssembly(
      string codebase,
      long size,
      string fileVersion,
      XEassemblyIdentity assemblyIdentity,
      string group,
      bool onDemandOnly)
      : base("dependentAssembly")
    {
      this.codebase = codebase;
      this.Size = size;
      this.FileVersion = fileVersion;
      this.Group = group;
      this.OnDemandOnly = onDemandOnly;
      this.AssemblyIdentity = assemblyIdentity;
    }

    public void Copy(XEdependentAssembly depAsm)
    {
      this.codebase = depAsm.Codebase;
      this.Size = depAsm.Size;
      this.FileVersion = depAsm.FileVersion;
      this.Group = depAsm.Group;
      this.OnDemandOnly = depAsm.OnDemandOnly;
      this.AssemblyIdentity = depAsm.AssemblyIdentity;
    }

    public void AddDeployZipExtToCodebase() => this.codebase += ResolverConsts.DeployZipExt;

    public XEdependentAssembly(XmlElement xmlElem)
      : base(xmlElem)
    {
      this.codebase = xmlElem.GetAttribute(nameof (codebase));
      this.Size = long.Parse(xmlElem.GetAttribute("size"));
      this.FileVersion = xmlElem.GetAttribute("fileVersion");
      this.Group = xmlElem.GetAttribute("group");
      this.OnDemandOnly = (xmlElem.GetAttribute("onDemandOnly") ?? "").Trim().ToLower() == "true";
      foreach (XmlNode childNode in xmlElem.ChildNodes)
      {
        if (childNode.NodeType == XmlNodeType.Element)
        {
          XmlElement xmlElem1 = (XmlElement) childNode;
          this.AssemblyIdentity = xmlElem1.Name == "assemblyIdentity" ? new XEassemblyIdentity(xmlElem1) : throw new Exception("Illegal element " + xmlElem1.Name + " in " + xmlElem1.OuterXml);
        }
      }
    }

    public override void CreateElement(XmlDocument xmldoc, XmlElement parent)
    {
      base.CreateElement(xmldoc, parent);
      if (this.codebase != null)
        this.xmlElem.SetAttribute("codebase", this.codebase);
      if (this.Size >= 0L)
        this.xmlElem.SetAttribute("size", this.Size.ToString() ?? "");
      if (!BasicUtils.IsNullOrEmpty(this.FileVersion))
        this.xmlElem.SetAttribute("fileVersion", this.FileVersion);
      if (!BasicUtils.IsNullOrEmpty(this.Group))
        this.xmlElem.SetAttribute("group", this.Group);
      if (this.OnDemandOnly)
        this.xmlElem.SetAttribute("onDemandOnly", "true");
      if (this.AssemblyIdentity == null)
        return;
      this.AssemblyIdentity.CreateElement(xmldoc, this.xmlElem);
    }

    public void RemoveFromOwnerDocument()
    {
      this.xmlElem.ParentNode.RemoveChild((XmlNode) this.xmlElem);
    }
  }
}
