// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.AttachmentPageImage
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.eFolder;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class AttachmentPageImage : IAttachmentPageImage
  {
    private Loan loan;
    private string attachmentName;
    private PageImage pageImage;
    private PageImageThumbnail thumbnail;
    private PageImageAnnotations annotations;

    internal AttachmentPageImage(Loan loan, string attachmentName, PageImage pageImage)
    {
      this.loan = loan;
      this.attachmentName = attachmentName;
      this.pageImage = pageImage;
      this.thumbnail = new PageImageThumbnail(pageImage.Thumbnail);
      this.annotations = new PageImageAnnotations(this.pageImage);
      foreach (PageAnnotation annotation in pageImage.Annotations)
        this.annotations.AddToList(new PageImageAnnotation(annotation));
    }

    internal ImageAttachment GetImageAttachment()
    {
      return (ImageAttachment) this.loan.Unwrap().FileAttachments[this.attachmentName];
    }

    internal PageImage GetPageImage() => this.pageImage;

    public string AttachmentName => this.attachmentName;

    public string ImageKey => this.pageImage.ImageKey;

    public string ZipKey => this.pageImage.ZipKey;

    public int Width => this.pageImage.Width;

    public int Height => this.pageImage.Height;

    public float HorizontalResolution => this.pageImage.HorizontalResolution;

    public float VerticalResolution => this.pageImage.VerticalResolution;

    public PageImageThumbnail Thumbnail => this.thumbnail;

    public PageImageAnnotations Annotations => this.annotations;
  }
}
