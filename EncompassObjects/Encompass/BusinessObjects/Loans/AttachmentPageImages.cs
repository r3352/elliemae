// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.AttachmentPageImages
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.eFolder;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Represents the collection of AttachmentPageImage objects that are associated with an <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Attachment" />.
  /// </summary>
  /// <remarks>Attachments represent documents which have been associated to the loan through
  /// the loan's eFolder. If the loan is configured to use images, it can have an AttachmentPageImages collection.</remarks>
  public class AttachmentPageImages : IAttachmentPageImages, IEnumerable
  {
    private Loan loan;
    private List<AttachmentPageImage> pageImages = new List<AttachmentPageImage>();

    internal AttachmentPageImages(Loan loan, PageImageCollection currentCollection)
    {
      this.loan = loan;
    }

    /// <summary>
    /// Adds an item to the list. Internal since user should not be able to create a new PageImage instance.
    /// </summary>
    /// <param name="value">The item to be added.</param>
    internal void Add(AttachmentPageImage value) => this.pageImages.Add(value);

    /// <summary>
    /// Removes a page image from the ccollection. User should refresh list after calling.
    /// </summary>
    /// <param name="attachmentPageImage">The page image to be removed.</param>
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

    /// <summary>
    /// Gets the number of AttachmentPageImages in the collection.
    /// </summary>
    public int Count => this.pageImages.Count;

    /// <summary>
    /// Retrieves an AttachmentPageImage from the collection by index.
    /// </summary>
    public AttachmentPageImage this[int index] => this.pageImages[index];

    /// <summary>Provides a enumerator for the collection.</summary>
    /// <returns>Returns an IEnumerator for enumerating the collection.</returns>
    public IEnumerator GetEnumerator() => (IEnumerator) this.pageImages.GetEnumerator();
  }
}
