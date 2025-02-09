// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.eFolder.PageImage
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
  public class PageImage
  {
    private const string className = "PageImage�";
    private static readonly string sw = Tracing.SwEFolder;
    private ImageAttachment attachment;
    private string imageKey;
    private string zipKey;
    private string nativeKey;
    private int width;
    private int height;
    private float horizontalResolution;
    private float verticalResolution;
    private long fileSize;
    private int rotation;
    private PageThumbnail thumbnail;
    private PageAnnotationCollection annotations;
    private string rotatedFile;
    private bool isIdentified;
    private string identifiedType;
    private string identifiedSource;

    public PageImage(
      string zipKey,
      string imageKey,
      string nativeKey,
      int width,
      int height,
      float horizontalResolution,
      float verticalResoulation,
      long fileSize,
      string pageThumbnailZipKey,
      string thumbnailImageKey,
      int thumbnailWidth,
      int thumbnailHeight,
      float thumbnailHorizontalResolution,
      float thumbnailVerticalResolution)
    {
      this.zipKey = zipKey;
      this.imageKey = imageKey;
      this.nativeKey = nativeKey;
      this.width = width;
      this.height = height;
      this.horizontalResolution = horizontalResolution;
      this.verticalResolution = verticalResoulation;
      this.fileSize = fileSize;
      this.thumbnail = new PageThumbnail(pageThumbnailZipKey, thumbnailImageKey, thumbnailWidth, thumbnailHeight, thumbnailHorizontalResolution, thumbnailVerticalResolution);
      this.annotations = new PageAnnotationCollection(this);
    }

    public PageImage(
      string zipKey,
      string imageKey,
      string nativeKey,
      int width,
      int height,
      float horizontalResolution,
      float verticalResoulation,
      long fileSize,
      string pageThumbnailZipKey,
      string thumbnailImageKey,
      int thumbnailWidth,
      int thumbnailHeight,
      float thumbnailHorizontalResolution,
      float thumbnailVerticalResolution,
      int rotation)
      : this(zipKey, imageKey, nativeKey, width, height, horizontalResolution, verticalResoulation, fileSize, pageThumbnailZipKey, thumbnailImageKey, thumbnailWidth, thumbnailHeight, thumbnailHorizontalResolution, thumbnailVerticalResolution)
    {
      this.rotation = rotation;
    }

    public PageImage(ImageProperties properties)
    {
      this.imageKey = properties.ImageKey;
      this.width = properties.Width;
      this.height = properties.Height;
      this.horizontalResolution = properties.HorizontalResolution;
      this.verticalResolution = properties.VerticalResolution;
      this.fileSize = new FileInfo(properties.ImageFile).Length;
      this.thumbnail = new PageThumbnail(this, properties.Thumbnail);
      this.annotations = new PageAnnotationCollection(this);
    }

    public PageImage(ImageAttachment attachment, XmlElement elm)
    {
      this.attachment = attachment;
      AttributeReader attributeReader = new AttributeReader(elm);
      this.imageKey = attributeReader.GetString(nameof (ImageKey));
      this.zipKey = attributeReader.GetString(nameof (ZipKey));
      this.nativeKey = attributeReader.GetString(nameof (NativeKey), (string) null);
      this.width = attributeReader.GetInteger(nameof (Width));
      this.height = attributeReader.GetInteger(nameof (Height));
      this.horizontalResolution = attributeReader.GetFloat(nameof (HorizontalResolution));
      this.verticalResolution = attributeReader.GetFloat(nameof (VerticalResolution));
      this.fileSize = attributeReader.GetLong(nameof (FileSize));
      this.rotation = attributeReader.GetInteger(nameof (Rotation), 0);
      this.thumbnail = new PageThumbnail(this, (XmlElement) elm.SelectSingleNode("THUMBNAIL"));
      XmlElement elm1 = (XmlElement) elm.SelectSingleNode("ANNOTATIONS");
      if (elm1 != null)
        this.annotations = new PageAnnotationCollection(this, elm1);
      else
        this.annotations = new PageAnnotationCollection(this);
    }

    public ImageAttachment Attachment => this.attachment;

    public string ImageKey => this.imageKey;

    public string ZipKey => this.zipKey;

    public string NativeKey => this.nativeKey;

    public int Width => this.width;

    public int Height => this.height;

    public float HorizontalResolution => this.horizontalResolution;

    public float VerticalResolution => this.verticalResolution;

    public int Rotation => this.rotation;

    public long FileSize => this.fileSize;

    public PageThumbnail Thumbnail => this.thumbnail;

    public PageAnnotationCollection Annotations => this.annotations;

    internal DocumentIdentity Identity
    {
      get
      {
        return this.attachment != null && this.isIdentified ? new DocumentIdentity(this.attachment.Pages.IndexOf(this), this.identifiedType, this.identifiedSource) : (DocumentIdentity) null;
      }
      set
      {
        if (value != null)
        {
          this.isIdentified = true;
          this.identifiedType = value.DocumentType;
          this.identifiedSource = value.DocumentSource;
        }
        else
        {
          this.isIdentified = false;
          this.identifiedType = (string) null;
          this.identifiedSource = (string) null;
        }
      }
    }

    public string GetRotatedFile()
    {
      Tracing.Log(PageImage.sw, TraceLevel.Verbose, nameof (PageImage), "GetRotatedFile: " + this.imageKey);
      return this.rotatedFile != null && File.Exists(this.rotatedFile) ? this.rotatedFile : (string) null;
    }

    public void SetRotatedFile(string filepath)
    {
      Tracing.Log(PageImage.sw, TraceLevel.Verbose, nameof (PageImage), "SetRotatedFile: " + filepath);
      this.rotatedFile = filepath;
    }

    public void Rotate()
    {
      int width = this.width;
      this.width = this.height;
      this.height = width;
      float horizontalResolution = this.horizontalResolution;
      this.horizontalResolution = this.verticalResolution;
      this.verticalResolution = this.horizontalResolution;
      this.rotation += 90;
      if (this.rotation == 360)
        this.rotation = 0;
      this.thumbnail.Rotate();
      foreach (PageAnnotation annotation in this.annotations)
        annotation.Rotate();
      this.rotatedFile = (string) null;
      if (this.attachment == null)
        return;
      this.attachment.MarkAsDirty();
    }

    internal void SetAttachment(ImageAttachment attachment) => this.attachment = attachment;

    public void SetNativeKey(string nativeKey) => this.nativeKey = nativeKey;

    public void SetZipKey(string zipKey) => this.zipKey = zipKey;

    public void ToXml(XmlElement elm)
    {
      AttributeWriter attributeWriter = new AttributeWriter(elm);
      attributeWriter.Write("ImageKey", (object) this.imageKey);
      attributeWriter.Write("ZipKey", (object) this.zipKey);
      attributeWriter.Write("NativeKey", (object) this.nativeKey);
      attributeWriter.Write("Width", (object) this.width);
      attributeWriter.Write("Height", (object) this.height);
      attributeWriter.Write("HorizontalResolution", (object) this.horizontalResolution);
      attributeWriter.Write("VerticalResolution", (object) this.verticalResolution);
      attributeWriter.Write("FileSize", (object) this.fileSize);
      attributeWriter.Write("Rotation", (object) this.rotation);
      XmlElement element1 = elm.OwnerDocument.CreateElement("THUMBNAIL");
      elm.AppendChild((XmlNode) element1);
      this.thumbnail.ToXml(element1);
      XmlElement element2 = elm.OwnerDocument.CreateElement("ANNOTATIONS");
      elm.AppendChild((XmlNode) element2);
      this.annotations.ToXml(element2);
    }

    public override string ToString()
    {
      if (this.attachment == null)
        return string.Empty;
      string str = this.attachment.Title;
      if (this.attachment.Pages.Count > 1)
      {
        int num = this.attachment.Pages.IndexOf(this) + 1;
        str = str + " - Page " + (object) num;
      }
      return str;
    }
  }
}
