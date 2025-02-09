// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.XmlHelperClasses.XmlDocumentBase
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System.Xml;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.XmlHelperClasses
{
  public class XmlDocumentBase
  {
    public readonly string Root;
    public readonly string ManifestVersion;

    public XmlDocumentBase(string root, string manifestVersion)
    {
      this.Root = root;
      this.ManifestVersion = manifestVersion;
    }

    public XmlDocumentBase(XmlDocument xmldoc)
    {
      this.Root = xmldoc.DocumentElement.Name;
      this.ManifestVersion = xmldoc.DocumentElement.GetAttribute("manifestVersion");
    }

    public virtual XmlDocument CreateDocument()
    {
      XmlDocument document = new XmlDocument();
      document.LoadXml("<?xml version=\"1.0\" encoding=\"us-ascii\"?><" + this.Root + "/>");
      if (this.ManifestVersion != null)
        document.DocumentElement.SetAttribute("manifestVersion", this.ManifestVersion);
      return document;
    }
  }
}
