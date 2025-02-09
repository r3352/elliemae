// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.XmlHelperClasses.XEhash
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.Utils;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.XmlHelperClasses
{
  internal class XEhash : XmlElementBase
  {
    public readonly string Algorithm;
    public readonly string DigestValue;

    public XEhash(string algorithm, string digestValue)
      : base("hash")
    {
      this.Algorithm = algorithm;
      this.DigestValue = digestValue;
      this.sanityCheck();
    }

    public XEhash(XmlElement xmlElem)
      : base(xmlElem)
    {
      this.Algorithm = xmlElem.GetAttribute("algorithm");
      if (BasicUtils.IsNullOrEmpty(this.Algorithm))
        this.Algorithm = ResolverConsts.HashAlgorithmSha1;
      this.DigestValue = xmlElem.GetAttribute("digestValue");
      this.sanityCheck();
    }

    public override void CreateElement(XmlDocument xmldoc, XmlElement parent)
    {
      this.sanityCheck();
      base.CreateElement(xmldoc, parent);
      this.xmlElem.SetAttribute("digestValue", this.DigestValue);
      if (!(this.Algorithm != ResolverConsts.HashAlgorithmSha1))
        return;
      this.xmlElem.SetAttribute("algorithm", this.Algorithm);
    }

    private void sanityCheck()
    {
      if (this.DigestValue == null || this.Algorithm == null)
        throw new Exception("Hash Algorithm and/or DigestValue cannot be null");
    }
  }
}
