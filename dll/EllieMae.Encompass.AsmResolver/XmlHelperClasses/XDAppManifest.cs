// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.XmlHelperClasses.XDAppManifest
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.XmlHelperClasses
{
  internal class XDAppManifest : XDManifest
  {
    public XEassemblyDeployment AssemblyDeployment;
    public XEappFileGroupList AppFileGroupList;
    private List<XEfile> resourceFiles;

    public List<XEfile> ResourceFiles => this.resourceFiles;

    internal XDAppManifest(
      string manifestVersion,
      XEassemblyIdentity assemblyIdentity,
      List<XEdependentAssembly> dependencies,
      List<XEfile> resFiles)
      : base(manifestVersion, assemblyIdentity, dependencies)
    {
      this.resourceFiles = resFiles;
    }

    internal XDAppManifest(XmlDocument xmldoc)
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
            case "assemblyDeployment":
              this.AssemblyDeployment = new XEassemblyDeployment(xmlElem);
              continue;
            case "appFileGroupList":
              this.AppFileGroupList = new XEappFileGroupList(xmlElem);
              continue;
            case "dependentAssembly":
              this.AddDependency(new XEdependentAssembly(xmlElem), false);
              continue;
            case "file":
              this.AddFile(new XEfile(xmlElem), false);
              continue;
            default:
              throw new Exception("Illegal element " + xmlElem.Name + " in " + xmlElem.OuterXml);
          }
        }
      }
    }

    private XEfile getFile(string filePath)
    {
      if (this.resourceFiles == null)
        return (XEfile) null;
      foreach (XEfile resourceFile in this.resourceFiles)
      {
        if (resourceFile.Path == filePath)
          return resourceFile;
      }
      return (XEfile) null;
    }

    private void removeFile(XEfile file)
    {
      if (file == null)
        return;
      file.RemoveFromOwnerDocument();
      this.resourceFiles.Remove(file);
    }

    public void AddFile(XEfile file, bool replaceIfExists)
    {
      if (this.resourceFiles == null)
        this.resourceFiles = new List<XEfile>();
      XEfile file1 = this.getFile(file.Path);
      if (!replaceIfExists && file1 != null)
        throw new Exception(file.Path + ": resource file already exists in the manifest");
      if (file1 != null & replaceIfExists)
        file1.Copy(file);
      else
        this.resourceFiles.Add(file);
    }

    public void AddAppFileGroup(XEappFileGroup appFileGroup)
    {
      if (this.AppFileGroupList == null)
        this.AppFileGroupList = new XEappFileGroupList(new List<XEappFileGroup>());
      this.AppFileGroupList.AddAppFileGroup(appFileGroup);
    }

    public XEappFileGroup GetAppFileGroup(string groupName)
    {
      if (this.AppFileGroupList == null)
        return (XEappFileGroup) null;
      foreach (XEappFileGroup appFileGroup in this.AppFileGroupList.AppFileGroups)
      {
        if (appFileGroup.Name == groupName)
          return appFileGroup;
      }
      return (XEappFileGroup) null;
    }

    public XEappFileGroup[] GetAllAppFileGroups()
    {
      if (this.AppFileGroupList == null)
        return (XEappFileGroup[]) null;
      List<XEappFileGroup> xeappFileGroupList = new List<XEappFileGroup>();
      foreach (XEappFileGroup appFileGroup in this.AppFileGroupList.AppFileGroups)
        xeappFileGroupList.Add(appFileGroup);
      return xeappFileGroupList.ToArray();
    }

    public override XmlDocument CreateDocument()
    {
      XmlDocument document = base.CreateDocument();
      if (this.AssemblyIdentity != null)
        this.AssemblyIdentity.CreateElement(document, document.DocumentElement);
      if (this.AssemblyDeployment != null)
        this.AssemblyDeployment.CreateElement(document, document.DocumentElement);
      if (this.AppFileGroupList != null)
        this.AppFileGroupList.CreateElement(document, document.DocumentElement);
      if (this.dependencies != null)
      {
        foreach (XmlElementBase dependency in this.dependencies)
          dependency.CreateElement(document, document.DocumentElement);
      }
      if (this.resourceFiles != null)
      {
        foreach (XmlElementBase resourceFile in this.resourceFiles)
          resourceFile.CreateElement(document, document.DocumentElement);
      }
      return document;
    }
  }
}
