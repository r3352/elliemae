// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.XmlHelperClasses.XEappFileGroupList
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.XmlHelperClasses
{
  internal class XEappFileGroupList : XmlElementBase
  {
    public List<XEappFileGroup> AppFileGroups;

    public XEappFileGroupList(List<XEappFileGroup> appFileGroups)
      : base("appFileGroupList")
    {
      this.AppFileGroups = appFileGroups;
    }

    public XEappFileGroupList(XmlElement xmlElem)
      : base(xmlElem)
    {
      foreach (XmlNode childNode in xmlElem.ChildNodes)
      {
        if (childNode.NodeType == XmlNodeType.Element)
        {
          XmlElement xmlElem1 = (XmlElement) childNode;
          if (!(xmlElem1.Name == "appFileGroup"))
            throw new Exception("Illegal element " + xmlElem1.Name + " in " + xmlElem1.OuterXml);
          this.AddAppFileGroup(new XEappFileGroup(xmlElem1));
        }
      }
    }

    public void AddAppFileGroup(XEappFileGroup appFileGroup)
    {
      if (this.AppFileGroups == null)
        this.AppFileGroups = new List<XEappFileGroup>();
      this.AppFileGroups.Add(appFileGroup);
    }

    public override void CreateElement(XmlDocument xmldoc, XmlElement parent)
    {
      base.CreateElement(xmldoc, parent);
      if (this.AppFileGroups == null)
        return;
      foreach (XmlElementBase appFileGroup in this.AppFileGroups)
        appFileGroup.CreateElement(xmldoc, this.xmlElem);
    }
  }
}
