// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Attachment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

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

    public string Name => this.name;

    public int Size
    {
      get
      {
        this.ensureLoaded();
        return (int) this.data.Length;
      }
    }

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

    public string Title
    {
      get => this.getFileAttachment().Title;
      set => this.getFileAttachment().Title = value ?? "";
    }

    public DateTime Date => this.getFileAttachment().Date;

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

    public AttachmentPageImages PageImages => this.pageImages;

    public void SaveToDisk(string path)
    {
      this.ensureLoaded();
      this.data.Write(path);
    }

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

    public TrackedDocument GetDocument()
    {
      DocumentLog linkedDocument = this.getLinkedDocument();
      return linkedDocument == null ? (TrackedDocument) null : (TrackedDocument) this.loan.Log.TrackedDocuments.Find((LogRecordBase) linkedDocument, true);
    }

    public override string ToString() => this.Name;

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
        }, (AnnotationExportType) 3));
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
        AttachmentType attachmentType = this.getFileAttachment().AttachmentType;
        if (attachmentType != 1)
        {
          if (attachmentType == 3)
          {
            CloudAttachment fileAttachment = (CloudAttachment) this.getFileAttachment();
            SkyDriveUrl driveUrlForObject = this.loan.Unwrap().LoanObject.GetSkyDriveUrlForObject(fileAttachment.ObjectId);
            if (driveUrlForObject != null)
              this.dataOriginal = this.DownloadFile(driveUrlForObject).Result;
          }
        }
        else
        {
          ImageAttachment fileAttachment = (ImageAttachment) this.getFileAttachment();
          if (fileAttachment.Pages[0].NativeKey != null)
            this.dataOriginal = new BinaryObject(this.loan.Unwrap().FileAttachments.DownloadNative(fileAttachment.Pages[0]));
        }
      }
      if (this.dataOriginal == null)
        throw new Exception("The specified attachment does not have an original file");
    }

    private FileAttachment getFileAttachment() => this.loan.Unwrap().FileAttachments[this.name];

    private DocumentLog getLinkedDocument()
    {
      return this.loan.Unwrap().FileAttachments.GetLinkedDocument(this.name, false);
    }

    public void DownloadThumbnails()
    {
      if (!this.IsImageAttachment)
        return;
      ImageAttachment fileAttachment = (ImageAttachment) this.getFileAttachment();
      this.loan.Unwrap().FileAttachments.DownloadThumbnails(fileAttachment.Pages.ToArray());
    }

    private async Task<BinaryObject> DownloadFile(SkyDriveUrl url)
    {
      BinaryObject retVal = (BinaryObject) null;
      try
      {
        using (HttpResponseMessage response = await this.submitRequest(url.url, url.authorizationHeader, "GET", (string) null, (HttpContent) null).ConfigureAwait(false))
        {
          using (Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
            return new BinaryObject(stream);
        }
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, nameof (Attachment), "Encompass SDK:Attachements: Error in DownloadFile.", ex);
      }
      return retVal;
    }

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
