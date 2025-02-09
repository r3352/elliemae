// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.XmlHelperClasses.XEappFileGroup
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.Utils;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.XmlHelperClasses
{
  internal class XEappFileGroup : XmlElementBase
  {
    public readonly string Name;
    public readonly string Codebase;
    public long Size = -1;

    public XEappFileGroup(string name, string codebase, long size)
      : base("appFileGroup")
    {
      this.Name = name;
      this.Codebase = codebase;
      this.Size = size;
    }

    public XEappFileGroup(XmlElement xmlElem)
      : base(xmlElem)
    {
      this.Name = xmlElem.GetAttribute("name");
      this.Codebase = xmlElem.GetAttribute("codebase");
      this.Size = long.Parse(xmlElem.GetAttribute("size"));
    }

    public override void CreateElement(XmlDocument xmldoc, XmlElement parent)
    {
      base.CreateElement(xmldoc, parent);
      if (this.Name != null)
        this.xmlElem.SetAttribute("name", this.Name);
      if (!BasicUtils.IsNullOrEmpty(this.Codebase))
        this.xmlElem.SetAttribute("codebase", this.Codebase);
      if (this.Size < 0L)
        return;
      this.xmlElem.SetAttribute("size", this.Size.ToString() ?? "");
    }
  }
}
