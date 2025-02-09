// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.XmlHelperClasses.XDConfiguration
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.XmlHelperClasses
{
  internal class XDConfiguration : XmlDocumentBase
  {
    public readonly XEdeploymentModules DeploymentModules;

    public XDConfiguration(XmlDocument xmldoc)
      : base(xmldoc)
    {
      XmlNodeList childNodes = xmldoc.DocumentElement.ChildNodes;
      if (childNodes == null)
        throw new Exception("Invalid configuration: " + xmldoc.OuterXml);
      foreach (XmlNode xmlNode in childNodes)
      {
        if (xmlNode.NodeType == XmlNodeType.Element)
        {
          XmlElement xmlElem = (XmlElement) xmlNode;
          this.DeploymentModules = xmlElem.Name == "deploymentModules" ? new XEdeploymentModules(xmlElem) : throw new Exception("Illegal element " + xmlElem.Name + " in " + xmlElem.OuterXml);
        }
      }
    }

    public XDConfiguration(XEdeploymentModules deployModules)
      : base("configuration", (string) null)
    {
      this.DeploymentModules = deployModules;
    }

    public override XmlDocument CreateDocument()
    {
      XmlDocument document = base.CreateDocument();
      if (this.DeploymentModules != null)
        this.DeploymentModules.CreateElement(document, document.DocumentElement);
      return document;
    }
  }
}
