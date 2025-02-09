// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.XmlHelperClasses.XEdeploymentModule
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.XmlHelperClasses
{
  internal class XEdeploymentModule : XmlElementBase
  {
    public readonly string Name;
    private readonly string typeAttr;
    public readonly string TypeName;
    public readonly string AsmNameWOExt;

    public XEdeploymentModule(XmlElement xmlElem)
      : base(xmlElem)
    {
      this.Name = xmlElem.GetAttribute("name");
      this.typeAttr = xmlElem.GetAttribute("type");
      string[] strArray = this.typeAttr.Split(new char[1]
      {
        ','
      }, StringSplitOptions.RemoveEmptyEntries);
      this.TypeName = strArray[0].Trim();
      this.AsmNameWOExt = strArray[1].Trim();
    }

    public XEdeploymentModule(string name, string type, string asmNameWOExt)
      : base("deploymentModule")
    {
      this.Name = name.Trim();
      this.TypeName = type.Trim();
      this.AsmNameWOExt = asmNameWOExt.Trim();
      this.typeAttr = this.TypeName + ", " + this.AsmNameWOExt;
    }

    public override void CreateElement(XmlDocument xmldoc, XmlElement parent)
    {
      base.CreateElement(xmldoc, parent);
      if (this.Name != null)
        this.xmlElem.SetAttribute("name", this.Name);
      if (this.typeAttr == null)
        return;
      this.xmlElem.SetAttribute("type", this.typeAttr);
    }
  }
}
