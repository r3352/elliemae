// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.XmlHelperClasses.XEfile
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.Utils;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.XmlHelperClasses
{
  internal class XEfile : XmlElementBase
  {
    private string path;
    public long Size = -1;
    public string FileVersion;
    public Version Version;
    private List<XEhash> hashList;
    public string Group;
    public bool OnDemandOnly;

    public string Path => this.path;

    public List<XEhash> HashList => this.hashList;

    public XEfile(
      string path,
      long size,
      string fileVersion,
      Version version,
      string group,
      bool onDemandOnly,
      List<XEhash> hashList)
      : base("file")
    {
      this.path = path;
      this.Size = size;
      this.FileVersion = fileVersion;
      this.Version = version;
      this.Group = group;
      this.OnDemandOnly = onDemandOnly;
      this.hashList = hashList;
    }

    public XEfile(XmlElement xmlElem)
      : base(xmlElem)
    {
      this.path = xmlElem.GetAttribute(nameof (path));
      this.Size = long.Parse(xmlElem.GetAttribute("size"));
      this.FileVersion = xmlElem.GetAttribute("fileVersion");
      string attribute = xmlElem.GetAttribute("version");
      this.Version = !BasicUtils.IsNullOrEmpty(attribute) ? new Version(attribute) : (Version) null;
      this.Group = xmlElem.GetAttribute("group");
      this.OnDemandOnly = (xmlElem.GetAttribute("onDemandOnly") ?? "").Trim().ToLower() == "true";
      foreach (XmlNode childNode in xmlElem.ChildNodes)
      {
        if (childNode.NodeType == XmlNodeType.Element)
        {
          XmlElement xmlElem1 = (XmlElement) childNode;
          if (!(xmlElem1.Name == "hash"))
            throw new Exception("Illegal element " + xmlElem1.Name + " in " + xmlElem1.OuterXml);
          this.AddHash(new XEhash(xmlElem1));
        }
      }
    }

    public void Copy(XEfile xeFile)
    {
      this.path = xeFile.Path;
      this.Size = xeFile.Size;
      this.FileVersion = xeFile.FileVersion;
      this.Version = xeFile.Version;
      this.hashList = xeFile.HashList;
      this.Group = xeFile.Group;
      this.OnDemandOnly = xeFile.OnDemandOnly;
    }

    public void AddHash(XEhash hash)
    {
      if (this.hashList == null)
        this.hashList = new List<XEhash>();
      this.hashList.Add(hash);
    }

    public void AddDeployZipExtToPath() => this.path += ResolverConsts.DeployZipExt;

    public override void CreateElement(XmlDocument xmldoc, XmlElement parent)
    {
      base.CreateElement(xmldoc, parent);
      if (this.path != null)
        this.xmlElem.SetAttribute("path", this.path);
      if (this.Size >= 0L)
        this.xmlElem.SetAttribute("size", this.Size.ToString() ?? "");
      if (!BasicUtils.IsNullOrEmpty(this.FileVersion))
        this.xmlElem.SetAttribute("fileVersion", this.FileVersion);
      if (this.Version != (Version) null)
        this.xmlElem.SetAttribute("version", this.Version.ToString());
      if (!BasicUtils.IsNullOrEmpty(this.Group))
        this.xmlElem.SetAttribute("group", this.Group);
      if (this.OnDemandOnly)
        this.xmlElem.SetAttribute("onDemandOnly", "true");
      if (this.hashList == null)
        return;
      foreach (XmlElementBase hash in this.hashList)
        hash.CreateElement(xmldoc, this.xmlElem);
    }

    public void RemoveFromOwnerDocument()
    {
      this.xmlElem.ParentNode.RemoveChild((XmlNode) this.xmlElem);
    }
  }
}
