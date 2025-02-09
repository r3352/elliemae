// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.XmlHelperClasses.XEassemblyDeployment
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.Utils;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.XmlHelperClasses
{
  internal class XEassemblyDeployment : XmlElementBase
  {
    public readonly long DownloadBlockSize = (long) FileUtil.FileDownloadBlockSize;
    public readonly string BgDownloadThreadPriority;
    public readonly int BgDownloadInterval;

    public XEassemblyDeployment(
      long downloadBlockSize,
      string bgDownloadThreadPriority,
      int bgDownloadInterval)
      : base("assemblyDeployment")
    {
      this.DownloadBlockSize = downloadBlockSize;
      this.BgDownloadThreadPriority = bgDownloadThreadPriority;
      this.validateBgDownloadThreadPriorityString(bgDownloadThreadPriority);
      this.BgDownloadInterval = bgDownloadInterval;
    }

    public XEassemblyDeployment(XmlElement xmlElem)
      : base(xmlElem)
    {
      this.DownloadBlockSize = long.Parse(xmlElem.GetAttribute("downloadBlockSize"));
      this.BgDownloadThreadPriority = xmlElem.GetAttribute("bgDownloadThreadPriority");
      this.validateBgDownloadThreadPriorityString(this.BgDownloadThreadPriority);
      if (!((xmlElem.GetAttribute("bgDownloadInterval") ?? "").Trim() != ""))
        return;
      this.BgDownloadInterval = Convert.ToInt32(xmlElem.GetAttribute("bgDownloadInterval"));
    }

    private void validateBgDownloadThreadPriorityString(string priority)
    {
      if (!DeployUtils.ValidateBgDownloadThreadPriorityString(priority))
        throw new Exception(priority + ": invalid background download thread priority string");
    }

    public bool BackgroundDownload
    {
      get => DeployUtils.DoBackgroundDownload(this.BgDownloadThreadPriority);
    }

    public override void CreateElement(XmlDocument xmldoc, XmlElement parent)
    {
      base.CreateElement(xmldoc, parent);
      if (this.DownloadBlockSize >= 0L)
        this.xmlElem.SetAttribute("downloadBlockSize", this.DownloadBlockSize.ToString() ?? "");
      if (this.BgDownloadThreadPriority != null)
        this.xmlElem.SetAttribute("bgDownloadThreadPriority", this.BgDownloadThreadPriority);
      this.xmlElem.SetAttribute("bgDownloadInterval", this.BgDownloadInterval.ToString() ?? "");
    }
  }
}
