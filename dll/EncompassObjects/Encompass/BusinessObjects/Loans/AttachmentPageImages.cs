// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.AttachmentPageImages
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.eFolder;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class AttachmentPageImages : IAttachmentPageImages, IEnumerable
  {
    private Loan loan;
    private List<AttachmentPageImage> pageImages = new List<AttachmentPageImage>();

    internal AttachmentPageImages(Loan loan, PageImageCollection currentCollection)
    {
      this.loan = loan;
    }

    internal void Add(AttachmentPageImage value) => this.pageImages.Add(value);

    public void Remove(AttachmentPageImage attachmentPageImage)
    {
      this.loan.Unwrap().GetImageAttachment(attachmentPageImage.AttachmentName).Pages.RemoveRange(new PageImage[1]
      {
        attachmentPageImage.GetPageImage()
      });
      attachmentPageImage.GetImageAttachment().Pages.RemoveRange(new PageImage[1]
      {
        attachmentPageImage.GetPageImage()
      });
    }

    public int Count => this.pageImages.Count;

    public AttachmentPageImage this[int index] => this.pageImages[index];

    public IEnumerator GetEnumerator() => (IEnumerator) this.pageImages.GetEnumerator();
  }
}
