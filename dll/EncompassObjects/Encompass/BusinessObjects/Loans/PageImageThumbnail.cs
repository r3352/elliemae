// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.PageImageThumbnail
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.eFolder;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class PageImageThumbnail : IPageImageThumbnail
  {
    private PageThumbnail thumbnail;

    internal PageImageThumbnail(PageThumbnail pageThumbnail) => this.thumbnail = pageThumbnail;

    public string ImageKey => this.thumbnail.ImageKey;

    public string ZipKey => this.thumbnail.ZipKey;

    public int Width => this.thumbnail.Width;

    public int Height => this.thumbnail.Height;

    public float HorizontalResolution => this.thumbnail.HorizontalResolution;

    public float VerticalResolution => this.thumbnail.VerticalResolution;
  }
}
