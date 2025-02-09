// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.PageImageThumbnail
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.eFolder;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Represents a thumbnail for a page image.</summary>
  /// <remarks>Every <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan" /> is capable of having any number of electronic
  /// document attachments associated with in via the eFolder in Encompass.
  /// If the loan is using images, the attachment can contain image pages and thumbnails of those images.
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
  public class PageImageThumbnail : IPageImageThumbnail
  {
    private PageThumbnail thumbnail;

    internal PageImageThumbnail(PageThumbnail pageThumbnail) => this.thumbnail = pageThumbnail;

    /// <summary>Returns image key</summary>
    public string ImageKey => this.thumbnail.ImageKey;

    /// <summary>Returns zip key</summary>
    public string ZipKey => this.thumbnail.ZipKey;

    /// <summary>Returns width</summary>
    public int Width => this.thumbnail.Width;

    /// <summary>Returns height</summary>
    public int Height => this.thumbnail.Height;

    /// <summary>Returns horizontal resolution</summary>
    public float HorizontalResolution => this.thumbnail.HorizontalResolution;

    /// <summary>Returns vertical resolution</summary>
    public float VerticalResolution => this.thumbnail.VerticalResolution;
  }
}
