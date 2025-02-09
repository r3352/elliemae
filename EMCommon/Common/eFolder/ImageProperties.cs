// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.eFolder.ImageProperties
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System.Drawing;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Common.eFolder
{
  public class ImageProperties
  {
    private string imageKey;
    private string imageFile;
    private Size size;
    private float horizontalResolution;
    private float verticalResolution;
    private ThumbnailProperties thumbnail;

    public ImageProperties(
      string imageKey,
      string imageFile,
      float horizontalResoulation,
      float verticalResoulation,
      Size size,
      ThumbnailProperties thumbnailProperties)
    {
      this.imageKey = imageKey;
      this.imageFile = imageFile;
      this.horizontalResolution = horizontalResoulation;
      this.verticalResolution = verticalResoulation;
      this.size = size;
      this.thumbnail = new ThumbnailProperties(thumbnailProperties.ImageKey, thumbnailProperties.ImageFile, thumbnailProperties.HorizontalResolution, thumbnailProperties.VerticalResolution, thumbnailProperties.Size);
    }

    public ImageProperties(string imageFile, Image image, string thumbnailFile, Image thumbnail)
    {
      this.imageKey = new FileInfo(imageFile).Name;
      this.imageFile = imageFile;
      this.size = image.Size;
      this.horizontalResolution = image.HorizontalResolution;
      this.verticalResolution = image.VerticalResolution;
      this.thumbnail = new ThumbnailProperties(thumbnailFile, thumbnail);
    }

    public string ImageKey => this.imageKey;

    public string ImageFile => this.imageFile;

    public Size Size => this.size;

    public int Width => this.size.Width;

    public int Height => this.size.Height;

    public float HorizontalResolution => this.horizontalResolution;

    public float VerticalResolution => this.verticalResolution;

    public ThumbnailProperties Thumbnail => this.thumbnail;
  }
}
