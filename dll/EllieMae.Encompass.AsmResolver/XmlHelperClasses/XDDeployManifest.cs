// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.XmlHelperClasses.XDDeployManifest
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.XmlHelperClasses
{
  internal class XDDeployManifest : XDManifest
  {
    internal XDDeployManifest(
      string manifestVersion,
      XEassemblyIdentity asmIdentity,
      List<XEdependentAssembly> dependencies)
      : base(manifestVersion, asmIdentity, dependencies)
    {
    }

    internal XDDeployManifest(XmlDocument xmldoc)
      : base(xmldoc)
    {
      XmlNodeList childNodes = xmldoc.DocumentElement.ChildNodes;
      if (childNodes == null || childNodes.Count < 2)
        throw new Exception("Invalid deployment manifest: " + xmldoc.OuterXml);
      foreach (XmlNode xmlNode in childNodes)
      {
        if (xmlNode.NodeType == XmlNodeType.Element)
        {
          XmlElement xmlElem = (XmlElement) xmlNode;
          switch (xmlElem.Name)
          {
            case "assemblyIdentity":
              this.assemblyIdentity = new XEassemblyIdentity(xmlElem);
              continue;
            case "dependentAssembly":
              this.AddDependency(new XEdependentAssembly(xmlElem), true);
              continue;
            default:
              throw new Exception("Illegal element " + xmlElem.Name + " in " + xmlElem.OuterXml);
          }
        }
      }
    }

    public override XmlDocument CreateDocument()
    {
      XmlDocument document = base.CreateDocument();
      if (this.AssemblyIdentity != null)
        this.AssemblyIdentity.CreateElement(document, document.DocumentElement);
      if (this.dependencies != null)
      {
        foreach (XmlElementBase dependency in this.dependencies)
          dependency.CreateElement(document, document.DocumentElement);
      }
      return document;
    }
  }
}
