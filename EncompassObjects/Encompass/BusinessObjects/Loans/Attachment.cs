// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Attachment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.SkyDrive;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Represents a document or image attached to the current loan.
  /// </summary>
  /// <remarks>Every <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Attachment.Loan" /> is capable of having any number of electronic
  /// document attachments associated with in via the eFolder in Encompass. Attachments
  /// can be created in a number of ways: scanning documents into Encompass, receipt of
  /// a fax via the Encompass Electronic Document Management feature, by conducting
  /// an ePASS transaction such as requesting a credit report, direct attachment
  /// via the Encompass UI or import through the API.
  /// <p>Once an attachment is added to the loan, it can be associated with a
  /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument" />. Once attached to a document, you can retrieve
  /// this document using the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Attachment.GetDocument" /> method.</p>
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
  public class Attachment : IAttachment, IDisposable
  {
    private const string className = "Attachment�";
    private Loan loan;
    private string name;
    private BinaryObject data;
    private BinaryObject dataOriginal;
    private byte[] byteData;
    private byte[] byteDataOriginal;
    private AttachmentPageImages pageImages;

    internal Attachment(Loan loan, string name)
    {
      this.loan = loan;
      this.name = name;
      if (this.IsImageAttachment)
      {
        ImageAttachment fileAttachment = (ImageAttachment) this.getFileAttachment();
        this.pageImages = new AttachmentPageImages(loan, fileAttachment.Pages);
        foreach (PageImage page in fileAttachment.Pages)
          this.pageImages.Add(new AttachmentPageImage(this.loan, this.name, page));
      }
      else
        this.pageImages = (AttachmentPageImages) null;
    }

    /// <summary>Gets the unique name of the attachment.</summary>
    /// <remarks>The Name of an attachment is unique within the loan and does
    /// not represent the name file that was originally imported into Encompass.
    /// The name can be used as a file name when saving the document to disk and
    /// includes the correct extension for its document type (e.g. a PDF document's
    /// Name will end in ".pdf").</remarks>
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
    public string Name => this.name;

    /// <summary>Gets the size of the attachment, in bytes.</summary>
    public int Size
    {
      get
      {
        this.ensureLoaded();
        return (int) this.data.Length;
      }
    }

    /// <summary>
    /// Gets the size of the original non-converted attachment, in bytes.
    /// </summary>
    /// <remarks>
    /// If the attachment was not added as an image attachment this will be the same as <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Attachment.Size" />.
    /// Throws an exception if the attachment was added as an image attachment but the option to keep the original was not selected.
    /// </remarks>
    public int SizeOriginal
    {
      get
      {
        if (!this.IsImageAttachment && !this.IsCloudAttachment)
          return this.Size;
        this.ensureOriginalLoaded();
        return (int) this.dataOriginal.Length;
      }
    }

    /// <summary>Returns the data from the attachment as a byte array.</summary>
    /// <example>
    /// The following code extracts the attachments from all Appraisals in the
    /// loan and posts them to a remote URL.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
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
    ///       // Get all Appraisals from the TrackedDocument list
    ///       LogEntryList appraisals = loan.Log.TrackedDocuments.GetDocumentsByTitle("Appraisal");
    /// 
    ///       // Post every page of every attachment to a URL on the web
    ///       foreach (TrackedDocument appraisal in appraisals)
    ///          foreach (Attachment att in appraisal.GetAttachments())
    ///          {
    ///             System.Net.WebClient wc = new System.Net.WebClient();
    ///             wc.UploadData("https://myserver.com/postdocument.aspx", att.Data);
    ///          }
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
    public byte[] Data
    {
      get
      {
        this.ensureLoaded();
        if (this.byteData == null)
          this.byteData = this.data.GetBytes();
        return this.byteData;
      }
    }

    /// <summary>
    /// Returns the data from the original non-converted attachment as a byte array.
    /// </summary>
    /// <remarks>
    /// If the attachment was not added as an image attachment this will be the same as <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Attachment.Data" />.
    /// Throws an exception if the attachment was added as an image attachment but the option to keep the original was not selected.
    /// </remarks>
    public byte[] DataOriginal
    {
      get
      {
        if (!this.IsImageAttachment && !this.IsCloudAttachment)
          return this.Data;
        this.ensureOriginalLoaded();
        if (this.byteDataOriginal == null)
          this.byteDataOriginal = this.dataOriginal.GetBytes();
        return this.byteDataOriginal;
      }
    }

    /// <summary>Gets or sets the title of the Attachment.</summary>
    public string Title
    {
      get => this.getFileAttachment().Title;
      set => this.getFileAttachment().Title = value ?? "";
    }

    /// <summary>Gets the date the attachment was added to the loan.</summary>
    public DateTime Date => this.getFileAttachment().Date;

    /// <summary>Gets or sets whether the Attachment is active</summary>
    public bool IsActive
    {
      get
      {
        DocumentLog linkedDocument = this.getLinkedDocument();
        return linkedDocument != null && linkedDocument.Files[this.name].IsActive;
      }
      set
      {
        FileAttachmentReference file = (this.getLinkedDocument() ?? throw new NullReferenceException("IsActive cannot be set on an Unassigned attachment.")).Files[this.name];
        if (value)
          file.MarkAsActive(this.loan.Session.UserID);
        else
          file.UnmarkAsActive();
      }
    }

    /// <summary>Returns whether the Attachment is an ImageAttachment</summary>
    public bool IsImageAttachment
    {
      get
      {
        bool isImageAttachment = false;
        if (this.getFileAttachment() is ImageAttachment)
          isImageAttachment = true;
        return isImageAttachment;
      }
    }

    /// <summary>Returns whether the Attachment is a CloudAttachment</summary>
    public bool IsCloudAttachment
    {
      get
      {
        bool isCloudAttachment = false;
        if (this.getFileAttachment() is CloudAttachment)
          isCloudAttachment = true;
        return isCloudAttachment;
      }
    }

    /// <summary>
    /// Returns the list of pages associated with this image attachment
    /// </summary>
    public AttachmentPageImages PageImages => this.pageImages;

    /// <summary>Saves the attachment to a particular path.</summary>
    /// <param name="path">The path, including file name, to which to save the attachment.
    /// </param>
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
    public void SaveToDisk(string path)
    {
      this.ensureLoaded();
      this.data.Write(path);
    }

    /// <summary>
    /// Saves the converted image attachment to a particular path in its original format.
    /// </summary>
    /// <param name="path">The path, including file name, to which to save the attachment.</param>
    /// <remarks>
    /// Throws an exception if the attachment was added as an image attachment but the option to keep the original was not selected.
    /// </remarks>
    public void SaveToDiskOriginal(string path)
    {
      if (this.IsImageAttachment || this.IsCloudAttachment)
      {
        this.ensureOriginalLoaded();
        this.dataOriginal.Write(path);
      }
      else if (this.loan.Unwrap().UseSkyDriveClassic)
      {
        NativeAttachment fileAttachment = (NativeAttachment) this.getFileAttachment();
        SkyDriveUrl driveUrlForObject = this.loan.Unwrap().LoanObject.GetSkyDriveUrlForObject(fileAttachment.ObjectId);
        if (driveUrlForObject == null)
          return;
        this.dataOriginal = this.DownloadFile(driveUrlForObject).Result;
        this.dataOriginal.Write(path);
      }
      else
        this.SaveToDisk(path);
    }

    /// <summary>
    /// Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument" /> to which the attachment is
    /// attached.
    /// </summary>
    /// <returns>Return the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument" /> to which the
    /// current attachment is attached, or <c>null</c> if this attachment is not
    /// attached to any document.</returns>
    /// <remarks>To attach an Attachment to a TrackedDocument, use the
    /// <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.Attach(EllieMae.Encompass.BusinessObjects.Loans.Attachment)" /> method.</remarks>
    /// <example>
    /// The following code extracts displays the names of all TrackedDocuments that
    /// have at least one PDF-type attachment.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
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
    ///       // Fetch all of the attachments with PDF extensions
    ///       foreach (Attachment att in loan.Attachments)
    ///       {
    ///          if (att.Name.ToLower().EndsWith(".pdf"))
    ///          {
    ///             // Determine the document to which it is attached, if any
    ///             TrackedDocument doc = att.GetDocument();
    /// 
    ///             if (doc != null)
    ///                Console.WriteLine("The document '" + doc.Title + "' has a PDF attachment.");
    ///          }
    ///       }
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
    public TrackedDocument GetDocument()
    {
      DocumentLog linkedDocument = this.getLinkedDocument();
      return linkedDocument == null ? (TrackedDocument) null : (TrackedDocument) this.loan.Log.TrackedDocuments.Find((LogRecordBase) linkedDocument, true);
    }

    /// <summary>
    /// Provides a string representation of the Attachment, which is the attachment's
    /// <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Attachment.Name" />.
    /// </summary>
    /// <returns>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Attachment.Name" /> of the attachment.</returns>
    public override string ToString() => this.Name;

    /// <summary>
    /// Refreshes the data of the attachment to ensure any changes have been read in.
    /// </summary>
    public void Refresh()
    {
      this.data = (BinaryObject) null;
      this.dataOriginal = (BinaryObject) null;
    }

    internal Loan Loan => this.loan;

    private void ensureLoaded()
    {
      if (this.data != null)
        return;
      DateTime now = DateTime.Now;
      long ticks = now.Ticks;
      string str1;
      if (this.IsImageAttachment)
      {
        str1 = "Exported ImageAttachment: ";
        ImageAttachment fileAttachment = (ImageAttachment) this.getFileAttachment();
        this.data = new BinaryObject(this.loan.Unwrap().FileAttachments.CreatePdf(fileAttachment));
      }
      else if (this.IsCloudAttachment)
      {
        str1 = "Exported CloudAttachment: ";
        CloudAttachment fileAttachment = (CloudAttachment) this.getFileAttachment();
        this.data = new BinaryObject(this.loan.Unwrap().FileAttachments.CreatePdf(new CloudAttachment[1]
        {
          fileAttachment
        }, AnnotationExportType.Public));
      }
      else
      {
        str1 = "Exported NativeAttachment: ";
        if (this.loan.Unwrap().UseSkyDriveClassic)
        {
          NativeAttachment fileAttachment = (NativeAttachment) this.getFileAttachment();
          this.data = new BinaryObject(this.loan.Unwrap().FileAttachments.CreateSDCNativePdf(fileAttachment));
        }
        else
          this.data = this.loan.Unwrap().GetSupportingData(this.name);
      }
      if (this.data == null)
        throw new Exception("The specified attachment no longer exists");
      string str2 = str1;
      now = DateTime.Now;
      // ISSUE: variable of a boxed type
      __Boxed<double> totalMilliseconds = (ValueType) TimeSpan.FromTicks(now.Ticks - ticks).TotalMilliseconds;
      RemoteLogger.Write(TraceLevel.Info, str2 + (object) totalMilliseconds + " ms");
    }

    private void ensureOriginalLoaded()
    {
      if (this.dataOriginal == null)
      {
        switch (this.getFileAttachment().AttachmentType)
        {
          case AttachmentType.Image:
            ImageAttachment fileAttachment1 = (ImageAttachment) this.getFileAttachment();
            if (fileAttachment1.Pages[0].NativeKey != null)
            {
              this.dataOriginal = new BinaryObject(this.loan.Unwrap().FileAttachments.DownloadNative(fileAttachment1.Pages[0]));
              break;
            }
            break;
          case AttachmentType.Cloud:
            CloudAttachment fileAttachment2 = (CloudAttachment) this.getFileAttachment();
            SkyDriveUrl driveUrlForObject = this.loan.Unwrap().LoanObject.GetSkyDriveUrlForObject(fileAttachment2.ObjectId);
            if (driveUrlForObject != null)
            {
              this.dataOriginal = this.DownloadFile(driveUrlForObject).Result;
              break;
            }
            break;
        }
      }
      if (this.dataOriginal == null)
        throw new Exception("The specified attachment does not have an original file");
    }

    private FileAttachment getFileAttachment() => this.loan.Unwrap().FileAttachments[this.name];

    /// <summary>
    /// Retrieves the Linked Doc for the current Attachment object
    /// </summary>
    /// <returns></returns>
    private DocumentLog getLinkedDocument()
    {
      return this.loan.Unwrap().FileAttachments.GetLinkedDocument(this.name, false);
    }

    /// <summary>Download the thumbnail images of this attachment.</summary>
    public void DownloadThumbnails()
    {
      if (!this.IsImageAttachment)
        return;
      ImageAttachment fileAttachment = (ImageAttachment) this.getFileAttachment();
      this.loan.Unwrap().FileAttachments.DownloadThumbnails(fileAttachment.Pages.ToArray());
    }

    /// <summary>Downloadsthe file from Skydrive URL</summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private async Task<BinaryObject> DownloadFile(SkyDriveUrl url)
    {
      BinaryObject retVal = (BinaryObject) null;
      try
      {
        using (HttpResponseMessage response = await this.submitRequest(url.url, url.authorizationHeader, "GET", (string) null, (HttpContent) null).ConfigureAwait(false))
        {
          using (Stream inputStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
            return new BinaryObject(inputStream);
        }
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, nameof (Attachment), "Encompass SDK:Attachements: Error in DownloadFile.", ex);
      }
      return retVal;
    }

    /// <summary>
    /// Method used to submit a request to the SkyDrive Streaming Service
    /// </summary>
    private async Task<HttpResponseMessage> submitRequest(
      string url,
      string authorizationHeader,
      string method,
      string acceptHeader,
      HttpContent content)
    {
      HttpResponseMessage httpResponseMessage;
      using (HttpRequestMessage request = new HttpRequestMessage())
      {
        request.Method = new HttpMethod(method);
        request.RequestUri = new Uri(url);
        if (!string.IsNullOrEmpty(authorizationHeader))
          request.Headers.Add("Authorization", authorizationHeader);
        if (!string.IsNullOrEmpty(acceptHeader))
          request.Headers.Add("Accept", acceptHeader);
        if (content != null)
          request.Content = content;
        ServicePointManager.FindServicePoint(request.RequestUri).ConnectionLeaseTimeout = 60000;
        HttpResponseMessage response = await new HttpClient()
        {
          Timeout = TimeSpan.FromMinutes(15.0)
        }.SendAsync(request).ConfigureAwait(false);
        if (response.IsSuccessStatusCode)
        {
          httpResponseMessage = response;
        }
        else
        {
          if (response.StatusCode == HttpStatusCode.NotFound)
            throw new HttpException((int) response.StatusCode, "Not Found");
          if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new HttpException((int) response.StatusCode, "Unauthorized Request");
          string message = "Response: " + (object) (int) response.StatusCode + " " + (object) response.StatusCode;
          string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
          IEnumerable<string> values;
          response.Headers.TryGetValues("X-Correlation-ID", out values);
          if (values != null)
            message = message + " CorrelationID=" + values.FirstOrDefault<string>();
          throw new HttpException((int) response.StatusCode, message);
        }
      }
      return httpResponseMessage;
    }

    /// <summary>TBD</summary>
    public void Dispose()
    {
      if (this.IsImageAttachment && this.data != null)
      {
        this.data.Dispose();
        this.data = (BinaryObject) null;
      }
      if (this.dataOriginal == null)
        return;
      this.dataOriginal.Dispose();
      this.dataOriginal = (BinaryObject) null;
    }
  }
}
