// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.XmlHelperClasses.XEassemblyIdentity
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.Utils;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.XmlHelperClasses
{
  internal class XEassemblyIdentity : XmlElementBase
  {
    public readonly string Name;
    public Version Version;

    public XEassemblyIdentity(string name, Version version)
      : base("assemblyIdentity")
    {
      this.Name = name;
      this.Version = version;
    }

    public XEassemblyIdentity(XmlElement xmlElem)
      : base(xmlElem)
    {
      this.Name = xmlElem.GetAttribute("name");
      string attribute = xmlElem.GetAttribute("version");
      if (BasicUtils.IsNullOrEmpty(attribute))
        this.Version = (Version) null;
      else
        this.Version = new Version(attribute);
    }

    public override void CreateElement(XmlDocument xmldoc, XmlElement parent)
    {
      base.CreateElement(xmldoc, parent);
      if (this.Name != null)
        this.xmlElem.SetAttribute("name", this.Name);
      if (!(this.Version != (Version) null))
        return;
      this.xmlElem.SetAttribute("version", this.Version.ToString());
    }
  }
}
