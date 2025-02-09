// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.XmlHelperClasses.XEdeploymentModules
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.XmlHelperClasses
{
  internal class XEdeploymentModules : XmlElementBase
  {
    private List<XEdeploymentModule> moduleList;

    public List<XEdeploymentModule> ModuleList => this.moduleList;

    public XEdeploymentModules(XmlElement xmlElem)
      : base(xmlElem)
    {
      foreach (XmlNode childNode in xmlElem.ChildNodes)
      {
        if (childNode.NodeType == XmlNodeType.Element)
        {
          XmlElement xmlElem1 = (XmlElement) childNode;
          if (!(xmlElem1.Name == "deploymentModule"))
            throw new Exception("Illegal element " + xmlElem1.Name + " in " + xmlElem1.OuterXml);
          this.AddDeployModule(new XEdeploymentModule(xmlElem1));
        }
      }
    }

    public XEdeploymentModules(List<XEdeploymentModule> moduleList)
      : base("deploymentModules")
    {
      this.moduleList = moduleList;
    }

    public void AddDeployModule(XEdeploymentModule module)
    {
      if (this.moduleList == null)
        this.moduleList = new List<XEdeploymentModule>();
      this.moduleList.Add(module);
    }

    public override void CreateElement(XmlDocument xmldoc, XmlElement parent)
    {
      base.CreateElement(xmldoc, parent);
      if (this.moduleList == null)
        return;
      foreach (XmlElementBase module in this.moduleList)
        module.CreateElement(xmldoc, this.xmlElem);
    }
  }
}
