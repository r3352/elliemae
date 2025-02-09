// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanAttachments
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Files;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Attachment" /> objects that are associated with
  /// a loan.
  /// </summary>
  /// <remarks>Attachments represent documents which have been associated to the loan through
  /// the loan's eFolder. Any changes to the attachments collection will not be saved until
  /// the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.Commit" /> method is invoked on the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan" />.</remarks>
  /// <example>
  /// The following code demonstrates how to iterate over each attachment associated
  /// with a loan and extract it to disk.
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
  public class LoanAttachments : ILoanAttachments, IEnumerable
  {
    private static string[] validExtensions = new string[12]
    {
      ".pdf",
      ".doc",
      ".docx",
      ".txt",
      ".tif",
      ".jpg",
      ".jpeg",
      ".jpe",
      ".htm",
      ".html",
      ".emf",
      ".xps"
    };
    private Loan loan;
    private FileAttachmentCollection attachments;
    private Dictionary<string, Attachment> attachmentCache = new Dictionary<string, Attachment>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);

    internal LoanAttachments(Loan loan)
    {
      this.loan = loan;
      this.attachments = loan.Unwrap().FileAttachments;
    }

    /// <summary>Gets the number of attachments in the collection.</summary>
    public int Count => this.attachments.Count;

    /// <summary>
    /// Retrieves an <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Attachment" /> from the collection by index.
    /// </summary>
    /// <example>
    /// The following code demonstrates how to iterate over each attachment associated
    /// with a loan and extract it to disk.
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
    public Attachment this[int index]
    {
      get
      {
        string id = this.attachments[index].ID;
        if (!this.attachmentCache.ContainsKey(id))
          this.attachmentCache[id] = new Attachment(this.loan, id);
        return this.attachmentCache[id];
      }
    }

    /// <summary>
    /// Retrieves an <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Attachment" /> from the collection by name.
    /// </summary>
    /// <param name="name">The name of the attachment to retrieve.</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Attachment" /> if the name is value,
    /// <c>null</c> otherwise.</returns>
    public Attachment GetAttachmentByName(string name)
    {
      FileAttachment fileAttachment = (FileAttachment) null;
      foreach (FileAttachment attachment in this.attachments)
      {
        if (string.Compare(attachment.ID, name, true) == 0)
        {
          fileAttachment = attachment;
          break;
        }
      }
      if (fileAttachment == null)
        return (Attachment) null;
      if (!this.attachmentCache.ContainsKey(name))
        this.attachmentCache[fileAttachment.ID] = new Attachment(this.loan, fileAttachment.ID);
      return this.attachmentCache[fileAttachment.ID];
    }

    /// <summary>
    /// Creates a new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Attachment" /> to the loan from a file on disk.
    /// </summary>
    /// <param name="filePath">The path of a file that contains the attachment.</param>
    /// <returns>The new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Attachment" /> object.</returns>
    /// <remarks>The underlying loan must be saved prior to adding attachments.
    /// Attempting to add attachments to an unsaved loan will result in an exception
    /// being thrown.</remarks>
    /// <example>
    /// The following code demonstrates how to add a new attachment to a loan
    /// and associate it with an existing TrackedDocument record.
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
    ///       // Lock the loan since we will be modifying it
    ///       loan.Lock();
    /// 
    ///       // Create a new attachment by importing it from a TIFF document on disk
    ///       Attachment att = loan.Attachments.Add("C:\\Scanner Output\\MyAppraisal.tif");
    /// 
    ///       // Now attach the new Attachment to the Appraisal on the loan
    ///       LogEntryList appraisals = loan.Log.TrackedDocuments.GetDocumentsByTitle("Appraisal");
    /// 
    ///       if (appraisals.Count > 0)
    ///       {
    ///          TrackedDocument appraisal = (TrackedDocument) appraisals[0];
    ///          appraisal.Attach(att);
    ///       }
    /// 
    ///       // Save the changes to the loan, which commits the new attachment
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public Attachment Add(string filePath)
    {
      if (this.loan.IsNew)
        throw new InvalidOperationException("Attachments cannot be created until loan is Committed.");
      if (!this.isValidFileExtension(filePath))
        throw new ArgumentException("The specified file does not have a supported extension.");
      return this.GetAttachmentByName(this.loan.Unwrap().FileAttachments.AddAttachment(AddReasonType.SDK, filePath, "Untitled", (DocumentLog) null).ID);
    }

    /// <summary>
    /// Creates a new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Attachment" /> to the loan from a <see cref="T:EllieMae.Encompass.BusinessObjects.DataObject" />.
    /// </summary>
    /// <param name="data">The <see cref="T:EllieMae.Encompass.BusinessObjects.DataObject" /> containing the binary image of
    /// attachment.</param>
    /// <param name="fileExtension">The file extension for the object, e.g. "doc" or "pdf".</param>
    /// <returns>The new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Attachment" /> object.</returns>
    /// <remarks>The underlying loan must be saved prior to adding attachments.
    /// Attempting to add attachments to an unsaved loan will result in an exception
    /// being thrown.</remarks>
    public Attachment AddObject(DataObject data, string fileExtension)
    {
      if (this.loan.IsNew)
        throw new InvalidOperationException("Attachments cannot be created until loan is Committed.");
      if (!this.isValidFileExtension("test." + fileExtension))
        throw new ArgumentException("The specified file does not have a supported extension.");
      return this.GetAttachmentByName(this.loan.Unwrap().FileAttachments.AddAttachment(AddReasonType.SDK, data.Unwrap(), fileExtension, "Untitled", (DocumentLog) null).ID);
    }

    /// <summary>
    /// Creates a new Image <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Attachment" /> in the eFolder using the specified file.
    /// </summary>
    /// <param name="filePath">The path of a file that contains the attachment.</param>
    /// <returns>The new image <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Attachment" /> object.</returns>
    /// <remarks>The underlying loan must be saved prior to adding attachments.
    /// Attempting to add image attachments to an unsaved loan will result in an exception
    /// being thrown.</remarks>
    public Attachment AddImage(string filePath)
    {
      if (this.loan.IsNew)
        throw new InvalidOperationException("Attachments cannot be created until loan is Committed.");
      if (!this.isValidFileExtension(filePath))
        throw new ArgumentException("The specified file does not have a supported extension.");
      return this.GetAttachmentByName((!this.loan.Unwrap().UseSkyDrive ? this.loan.Unwrap().FileAttachments.AddAttachment(AddReasonType.SDKImage, filePath, "Untitled", (DocumentLog) null) : this.loan.Unwrap().FileAttachments.AddAttachment(AddReasonType.SDK, filePath, "Untitled", (DocumentLog) null)).ID);
    }

    /// <summary>
    /// Creates a new Image <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Attachment" /> in the eFolder from a <see cref="T:EllieMae.Encompass.BusinessObjects.DataObject" />.
    /// </summary>
    /// <param name="data">The <see cref="T:EllieMae.Encompass.BusinessObjects.DataObject" /> containing the binary image of the
    /// attachment.</param>
    /// <param name="fileExtension">The file extension for the object, e.g. "doc" or "pdf".</param>
    /// <returns>The new Image <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Attachment" /> object.</returns>
    /// <remarks>The underlying loan must be saved prior to adding attachments.
    /// Attempting to add attachments to an unsaved loan will result in an exception
    /// being thrown.</remarks>
    public Attachment AddObjectImage(DataObject data, string fileExtension)
    {
      if (this.loan.IsNew)
        throw new InvalidOperationException("Attachments cannot be created until loan is Committed.");
      if (!this.isValidFileExtension("test." + fileExtension))
        throw new ArgumentException("The specified file does not have a supported extension.");
      return this.GetAttachmentByName((!this.loan.Unwrap().UseSkyDrive ? this.loan.Unwrap().FileAttachments.AddAttachment(AddReasonType.SDKImage, data.Unwrap(), fileExtension, "Untitled", (DocumentLog) null) : this.loan.Unwrap().FileAttachments.AddAttachment(AddReasonType.SDK, data.Unwrap(), fileExtension, "Untitled", (DocumentLog) null)).ID);
    }

    /// <summary>Removes an attachment from the loan.</summary>
    /// <param name="attachment">The attachment to be removed.</param>
    /// <remarks>Removing an attachment will cause it to be detached from any
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.ReceivedDownload" /> or <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument" /> to which it
    /// was previously associated. Removed attachments are not recoverable once
    /// the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.Commit" /> method is invoked on the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan" />.
    /// </remarks>
    /// <example>
    /// The following code demonstrates how to remove all attachments from a loan that
    /// represent pages from received faxes.
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
    ///       // Lock the loan since we will be modifying it
    ///       loan.Lock();
    /// 
    ///       for (int i = loan.Attachments.Count - 1; i >= 0; i--)
    ///       {
    ///          Attachment att = loan.Attachments[i];
    /// 
    ///          // Check if the attachment has an associated TrackedDocument entry and, if so,
    ///          // remove it.
    ///          if (att.GetDocument() != null)
    ///             loan.Attachments.Remove(att);
    ///       }
    /// 
    ///       // Save the changes to the loan, which commits the new attachment
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Remove(Attachment attachment)
    {
      if (this.loan.IsNew)
        throw new InvalidOperationException("Attachments cannot be created until loan is Committed.");
      if (this.GetAttachmentByName(attachment.Name) == null)
        throw new ArgumentException("The specified attachment is not found");
      this.loan.Unwrap().FileAttachments.Remove(RemoveReasonType.SDK, attachment.Name);
      if (!this.attachmentCache.ContainsKey(attachment.Name))
        return;
      this.attachmentCache.Remove(attachment.Name);
    }

    /// <summary>Moves a Page Image from one attachment to another.</summary>
    /// <param name="sourceAttachment">The attachment to move the page image from.</param>
    /// <param name="attachmentPageImage">The page image to be moved.</param>
    /// <param name="targetAttachment">The attachment to move the page image to.</param>
    /// <remarks>
    /// </remarks>
    public void MovePageImage(
      Attachment sourceAttachment,
      AttachmentPageImage attachmentPageImage,
      Attachment targetAttachment)
    {
      this.loan.Unwrap().GetImageAttachment(targetAttachment.Name).Pages.AddRange(new PageImage[1]
      {
        attachmentPageImage.GetPageImage()
      });
      sourceAttachment.PageImages.Remove(attachmentPageImage);
    }

    /// <summary>
    /// Provides an enumerator for the collection of attachments
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetEnumerator()
    {
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < this.attachments.Count; ++index)
        arrayList.Add((object) this[index]);
      return arrayList.GetEnumerator();
    }

    /// <summary>
    /// Refreshes the attachment collection to ensure all attachments are up-to-date.
    /// </summary>
    public void Refresh()
    {
      this.attachments = this.loan.Unwrap().FileAttachments;
      this.attachmentCache.Clear();
    }

    private bool isValidFileExtension(string fileName)
    {
      string lower = Path.GetExtension(fileName).ToLower();
      for (int index = 0; index < LoanAttachments.validExtensions.Length; ++index)
      {
        if (lower == LoanAttachments.validExtensions[index])
          return true;
      }
      return false;
    }
  }
}
