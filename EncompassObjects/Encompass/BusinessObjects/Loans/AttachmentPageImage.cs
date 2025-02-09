// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.AttachmentPageImage
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.eFolder;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Represents a page image that can be part of a collection for an <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Attachment" />.
  /// </summary>
  /// <remarks>Every <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan" /> is capable of having any number of electronic
  /// document attachments associated with in via the eFolder in Encompass.
  /// If the loan is using images, the attachment can contain image pages.
  /// </remarks>
  /// <example>
  /// The following code demonstrates how to extract all of the attachments from
  /// a loan and save them to a directory on the local disk.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// 
  /// class ContactManager
  /// {
  ///    public static void Main(string[] args)
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.StartOffline("mary", "maryspwd");
  /// 
  ///       // Open the loan using the GUID specified on the command line
  ///       Loan loan = session.Loans.Open(args[0]);
  /// 
  ///       // Iterate over the list of attachments, saving them to the C:\Temp folder
  ///       foreach (Attachment att in loan.Attachments)
  ///          att.SaveToDisk("C:\\Temp\\" + att.Name);
  /// 
  ///       // Close the loan, discarding all of our changes
  ///       loan.Close();
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
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

    /// <summary>
    /// Get the attachment name for the current AttachmentPageImage
    /// </summary>
    public string AttachmentName => this.attachmentName;

    /// <summary>ImageKey</summary>
    public string ImageKey => this.pageImage.ImageKey;

    /// <summary>ZipKey</summary>
    public string ZipKey => this.pageImage.ZipKey;

    /// <summary>Width</summary>
    public int Width => this.pageImage.Width;

    /// <summary>Height</summary>
    public int Height => this.pageImage.Height;

    /// <summary>HorizontalResolution</summary>
    public float HorizontalResolution => this.pageImage.HorizontalResolution;

    /// <summary>VerticalResolution</summary>
    public float VerticalResolution => this.pageImage.VerticalResolution;

    /// <summary>Returns the thumbnail for this page</summary>
    public PageImageThumbnail Thumbnail => this.thumbnail;

    /// <summary>Returns the collection of annotations</summary>
    public PageImageAnnotations Annotations => this.annotations;
  }
}
