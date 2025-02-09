// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.PageImageAnnotation
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.eFolder;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Represents an annotation for an attachment.</summary>
  /// <remarks>Every <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan" /> is capable of having any number of electronic
  /// document attachments associated with in via the eFolder in Encompass.
  /// If the loan is using images, the attachment can contain image pages with annotations.
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
  public class PageImageAnnotation : IPageImageAnnotation
  {
    private PageAnnotation annotation;
    private DateTime date;
    private string addedBy;
    private string text;
    private int left;
    private int top;
    private int width;
    private int height;

    /// <summary>Creates a new instance of the PageAnnotation class</summary>
    public PageImageAnnotation(
      string addedBy,
      string text,
      int left,
      int top,
      int width,
      int height)
    {
      this.date = DateTime.Now;
      this.addedBy = addedBy;
      this.text = text;
      this.left = left;
      this.top = top;
      this.width = width;
      this.height = height;
    }

    internal PageImageAnnotation(PageAnnotation pageAnnotation)
    {
      this.annotation = pageAnnotation;
      this.date = pageAnnotation.Date;
      this.addedBy = pageAnnotation.AddedBy;
      this.text = pageAnnotation.Text;
      this.left = pageAnnotation.Left;
      this.top = pageAnnotation.Top;
      this.width = pageAnnotation.Width;
      this.height = pageAnnotation.Height;
    }

    internal PageAnnotation getPageAnnotation() => this.annotation;

    /// <summary>Returns the date that the annotation was created</summary>
    public DateTime Date => this.date;

    /// <summary>Returns the user that created the annotation</summary>
    public string AddedBy => this.addedBy;

    /// <summary>Returns the text in the annotation</summary>
    public string Text => this.text;

    /// <summary>Returns the horizontal location of the annotation</summary>
    public int Left => this.left;

    /// <summary>Returns the vertical location of the annotation</summary>
    public int Top => this.top;

    /// <summary>Returns the width of the annotation</summary>
    public int Width => this.width;

    /// <summary>Returns the height of the annotation</summary>
    public int Height => this.height;
  }
}
