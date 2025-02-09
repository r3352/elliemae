// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.eFolder.PageThumbnail
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.eFolder;
using EllieMae.EMLite.Xml;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer.eFolder
{
  [Serializable]
  public class PageThumbnail
  {
    private const string className = "PageThumbnail�";
    private static readonly string sw = Tracing.SwEFolder;
    private PageImage page;
    private string imageKey;
    private string zipKey;
    private int width;
    private int height;
    private float horizontalResolution;
    private float verticalResolution;
    private string rotatedFile;

    internal PageThumbnail(
      string zipKey,
      string imageKey,
      int width,
      int height,
      float horizontalResolution,
      float verticalResolution)
    {
      this.zipKey = zipKey;
      this.imageKey = imageKey;
      this.width = width;
      this.height = height;
      this.horizontalResolution = horizontalResolution;
      this.verticalResolution = verticalResolution;
    }

    internal PageThumbnail(PageImage page, ThumbnailProperties properties)
    {
      this.page = page;
      this.imageKey = properties.ImageKey;
      this.width = properties.Width;
      this.height = properties.Height;
      this.horizontalResolution = properties.HorizontalResolution;
      this.verticalResolution = properties.VerticalResolution;
    }

    internal PageThumbnail(PageImage page, XmlElement elm)
    {
      this.page = page;
      AttributeReader attributeReader = new AttributeReader(elm);
      this.imageKey = attributeReader.GetString(nameof (ImageKey));
      this.zipKey = attributeReader.GetString(nameof (ZipKey));
      this.width = attributeReader.GetInteger(nameof (Width));
      this.height = attributeReader.GetInteger(nameof (Height));
      this.horizontalResolution = attributeReader.GetFloat(nameof (HorizontalResolution));
      this.verticalResolution = attributeReader.GetFloat(nameof (VerticalResolution));
    }

    public PageImage Page => this.page;

    public string ImageKey => this.imageKey;

    public string ZipKey => this.zipKey;

    public int Width => this.width;

    public int Height => this.height;

    public float HorizontalResolution => this.horizontalResolution;

    public float VerticalResolution => this.verticalResolution;

    public string GetRotatedFile()
    {
      Tracing.Log(PageThumbnail.sw, TraceLevel.Verbose, nameof (PageThumbnail), "GetRotatedFile: " + this.imageKey);
      return this.rotatedFile != null && File.Exists(this.rotatedFile) ? this.rotatedFile : (string) null;
    }

    public void SetRotatedFile(string filepath)
    {
      Tracing.Log(PageThumbnail.sw, TraceLevel.Verbose, nameof (PageThumbnail), "SetRotatedFile: " + filepath);
      this.rotatedFile = filepath;
    }

    internal void Rotate()
    {
      int width = this.width;
      this.width = this.height;
      this.height = width;
      float horizontalResolution = this.horizontalResolution;
      this.horizontalResolution = this.verticalResolution;
      this.verticalResolution = this.horizontalResolution;
      this.rotatedFile = (string) null;
    }

    public void SetZipKey(string zipKey) => this.zipKey = zipKey;

    public void ToXml(XmlElement elm)
    {
      AttributeWriter attributeWriter = new AttributeWriter(elm);
      attributeWriter.Write("ImageKey", (object) this.imageKey);
      attributeWriter.Write("ZipKey", (object) this.zipKey);
      attributeWriter.Write("Width", (object) this.width);
      attributeWriter.Write("Height", (object) this.height);
      attributeWriter.Write("HorizontalResolution", (object) this.horizontalResolution);
      attributeWriter.Write("VerticalResolution", (object) this.verticalResolution);
    }

    public override string ToString() => this.page.ToString();
  }
}
