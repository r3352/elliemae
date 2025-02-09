// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.eFolder.SkyDriveClassic.SDCHelper
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.eFolder.SkyDriveClassic;
using EllieMae.EMLite.ClientServer.SkyDrive;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.eFolder;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DocumentConverter;
using EllieMae.EMLite.LoanUtils.SkyDrive;
using EllieMae.EMLite.RemotingServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.eFolder.SkyDriveClassic
{
  public class SDCHelper
  {
    private const string RELATIVESTRINGFORPARTNERFILEUPLOAD = "/�";
    private const string RELATIVESTRINGFORPARTNERFILEDOWNLOAD = ".meta/transformation/�";
    private const int OFFSETFORPARTNERFILEUPLOAD = 15;
    private const int OFFSETFORPARTNERFILEDOWNLOAD = 40;
    private const string FILENAME_DOCJSON = "document.json�";
    private const string FILENAME_VERSION_1_PDF = "version-1.pdf�";
    private const string Category = "eFolder�";
    private const string ActiveUser = "ActiveUser�";
    private const string className = "SDCHelper�";
    private static readonly string sw = Tracing.SwEFolder;
    private bool isEFolderActiveUser;
    private object nativeFileDownloadLock = new object();
    private static object _conversionLock = new object();

    public LoanDataMgr LoanDataMgr { get; set; }

    public static int DegreeOfParallelism
    {
      get
      {
        int result;
        return int.TryParse(EnConfigurationSettings.AppSettings["ParallelismForSDC"], out result) ? result : 5;
      }
    }

    public SDCHelper()
    {
    }

    public SDCHelper(LoanDataMgr loanDataMgr)
    {
      this.LoanDataMgr = loanDataMgr;
      this.isEFolderActiveUser = loanDataMgr.UseSkyDriveClassic && loanDataMgr.SessionObjects.ConfigurationManager.GetCompanySetting("eFolder", nameof (ActiveUser))?.ToUpper() == "TRUE";
    }

    public SDCHelper.FileDetails ConvertFileToPDF(
      string inputFilePath,
      string directoryName,
      string fileName)
    {
      try
      {
        PdfCreator pdfCreator = new PdfCreator();
        Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Calling ConvertFileToPDF for filename-" + fileName);
        (string pdfFilePath, int PageCount, long fileSize) pdf = pdfCreator.ConvertFileToPDF(inputFilePath, directoryName, fileName);
        return new SDCHelper.FileDetails(pdf.pdfFilePath, pdf.PageCount, pdf.fileSize);
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCHelper.sw, TraceLevel.Error, nameof (SDCHelper), string.Format("SkyDriveClassic: Error converting browsed for file-{0} into PDF file. Ex: {1}", (object) fileName, (object) ex));
        throw;
      }
    }

    public async Task<string> SavePartnerFile(
      SkyDriveUrl preSigned,
      string partnerFileName,
      BinaryObject data)
    {
      string str1;
      try
      {
        string originalPresigned = preSigned.url;
        Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Calling PreparePresignedUrlForPartnerFile for filename-" + partnerFileName);
        preSigned.url = this.PreparePresignedUrlForPartnerFile(partnerFileName, preSigned.url, "/", 15);
        SkyDriveStreamingClient driveStreamingClient = new SkyDriveStreamingClient(this.LoanDataMgr);
        string contentType = FileContentTypes.GetContentType(partnerFileName);
        Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Calling SavePartnerFile with contentType-" + contentType);
        string str2 = await driveStreamingClient.SavePartnerFile(preSigned, contentType, data);
        preSigned.url = originalPresigned;
        str1 = str2;
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCHelper.sw, TraceLevel.Error, nameof (SDCHelper), string.Format("SkyDriveClassic: Error in saving partner file for file-{0}. Ex: {1}", (object) partnerFileName, (object) ex));
        throw;
      }
      return str1;
    }

    public SkyDriveUrl GetPreSignedForPartnerFileUpload(string objectId, string fileName = null)
    {
      return new SkyDriveStreamingClient(this.LoanDataMgr).GetPresignedUrlForPartnerFilesUpload(objectId, fileName);
    }

    public async Task<BinaryObject> GetPartnerFile(
      SkyDriveUrl preSigned,
      string objectId,
      string partnerFile,
      string attachmentId = null)
    {
      BinaryObject partnerFile1;
      try
      {
        string originalPresigned = preSigned.url;
        string fileKey = objectId;
        Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Calling PreparePresignedUrlForPartnerFile for objectId-" + objectId + " and file-" + partnerFile);
        if (objectId.Contains("KS*"))
        {
          preSigned.url = this.PreparePresignedUrlForPartnerFile(partnerFile, preSigned.url, ".meta/transformation/", objectId.Length + 1);
          fileKey = attachmentId;
        }
        else
          preSigned.url = this.PreparePresignedUrlForPartnerFile(partnerFile, preSigned.url, ".meta/transformation/", 40);
        SkyDriveStreamingClient driveStreamingClient = new SkyDriveStreamingClient(this.LoanDataMgr);
        Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Calling GetPartnerFile for objectId-" + objectId + " and file-" + partnerFile);
        BinaryObject partnerFile2 = await driveStreamingClient.GetPartnerFile(preSigned, fileKey);
        preSigned.url = originalPresigned;
        partnerFile1 = partnerFile2;
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCHelper.sw, TraceLevel.Error, nameof (SDCHelper), string.Format("SkyDriveClassic: Error in downloading partner files from skydrive for skyDriveObjectId-{0} and file-{1}. Ex: {2}", (object) objectId, (object) partnerFile, (object) ex));
        throw;
      }
      return partnerFile1;
    }

    public SkyDriveUrl GetPreSignedForPartnerFileDownload(string objectId)
    {
      return new SkyDriveStreamingClient(this.LoanDataMgr).GetPresignedUrlForPartnerFilesDownload(objectId);
    }

    public bool HasDocJsonChanged(string oldJson, string newJson)
    {
      return !JToken.DeepEquals(JToken.Parse(oldJson), JToken.Parse(newJson));
    }

    private string PreparePresignedUrlForPartnerFile(
      string partnerFile,
      string preSignedUrl,
      string relativeString,
      int offset)
    {
      try
      {
        string str1 = preSignedUrl.Substring(0, preSignedUrl.LastIndexOf("/") + offset);
        string str2 = preSignedUrl.Substring(preSignedUrl.LastIndexOf("?"));
        return str1 + relativeString + partnerFile + str2;
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCHelper.sw, TraceLevel.Error, nameof (SDCHelper), string.Format("SkyDriveClassic: Error in preparing presigned Url for PartnerFile-{0}. Ex: {1}", (object) partnerFile, (object) ex));
        throw;
      }
    }

    public void DeletePages(SDCDocument sdcDocumentCopy, List<int> pageIDs)
    {
      try
      {
        List<int> splitPages = new List<int>();
        List<Bookmark> bookmarkList = new List<Bookmark>();
        SDCBookmarksHelper sdcBookmarksHelper = new SDCBookmarksHelper();
        foreach (int pageId in pageIDs)
        {
          int pageID = pageId;
          Pages pages = sdcDocumentCopy.Pages.Where<Pages>((Func<Pages, bool>) (a => a.Id == pageID)).FirstOrDefault<Pages>();
          sdcDocumentCopy.Pages.Remove(pages);
          --sdcDocumentCopy.PageCount;
          if (sdcDocumentCopy.Annotations != null && sdcDocumentCopy.Annotations.Where<Annotation>((Func<Annotation, bool>) (a => a.PageId == pageID)).Any<Annotation>())
            sdcDocumentCopy.Annotations.RemoveAll((Predicate<Annotation>) (a => a.PageId == pageID));
        }
        if (sdcDocumentCopy.Bookmarks == null)
          return;
        foreach (Pages page in sdcDocumentCopy.Pages)
        {
          if (!pageIDs.Contains(page.Id))
            splitPages.Add(page.Id);
        }
        foreach (Bookmark bookmark in sdcDocumentCopy.Bookmarks)
          bookmarkList.AddRange((IEnumerable<Bookmark>) sdcBookmarksHelper.AddBookmarkRecursively(bookmark, splitPages, new List<Bookmark>(), 0, 0));
        if (bookmarkList.Count == 0)
          bookmarkList = (List<Bookmark>) null;
        sdcDocumentCopy.Bookmarks = bookmarkList;
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCHelper.sw, TraceLevel.Error, nameof (SDCHelper), string.Format("SkyDriveClassic: Error removing or deleteing page from file. Ex: {0}", (object) ex));
        throw;
      }
    }

    public void UpdatePageOrder(SDCDocument sdcDocumentCopy, int oldIndex, int newIndex)
    {
      Pages page = sdcDocumentCopy.Pages[oldIndex];
      sdcDocumentCopy.Pages[oldIndex] = sdcDocumentCopy.Pages[newIndex];
      sdcDocumentCopy.Pages[newIndex] = page;
    }

    public List<int> GetPageIndexListForSplit(NativeAttachment attachment, List<int> displayIndices)
    {
      SDCDocument sdcDocument = attachment.CurrentSDCDocument == null ? attachment.OriginalSDCDocument : attachment.CurrentSDCDocument;
      List<int> indexListForSplit = new List<int>();
      foreach (int displayIndex in displayIndices)
        indexListForSplit.Add(sdcDocument.Pages[displayIndex - 1].Id);
      return indexListForSplit;
    }

    public string GetConvertedVersionFile(FileAttachment fileAttachment)
    {
      string convertedVersionFile = (string) null;
      if (fileAttachment is NativeAttachment attachment)
      {
        convertedVersionFile = attachment.GetConvertedFile();
        if (this.LoanDataMgr.IsAutosaveEnabled && string.IsNullOrEmpty(convertedVersionFile) && attachment.OriginalSDCDocument != null && attachment.CurrentSDCDocument != null)
        {
          string filepath = (string) null;
          string str1 = (string) null;
          string str2 = string.Format("version-{0}.pdf", (object) (attachment.CurrentSDCDocument.Version + 1));
          SystemSettings.GetTempFileNameWithGivenFileName(attachment.ObjectId, str2);
          try
          {
            str1 = this.DownloadVersionPdfFile(attachment.ObjectId, 1, attachment.ID);
            if (this.HasDocJsonChanged(JsonConvert.SerializeObject((object) attachment.OriginalSDCDocument), JsonConvert.SerializeObject((object) attachment.CurrentSDCDocument)))
            {
              if (!string.IsNullOrEmpty(str1))
              {
                filepath = this.GenerateVersionXFileAndUploadFileToSkydrive(attachment, str1, str2, false);
                ++attachment.CurrentSDCDocument.Version;
              }
            }
          }
          catch (Exception ex) when (ex.InnerException?.GetType() == typeof (HttpException) && ((HttpException) ex.InnerException).GetHttpCode() == 404)
          {
            Tracing.Log(SDCHelper.sw, TraceLevel.Error, nameof (SDCHelper), "SkyDriveClassic: version-1.pdf for skyDriveObjectId-" + attachment.ObjectId + " not found");
            SDCHelper.FileDetails convertedFileDetails;
            this.GenerateVersion1PdfFromOriginalFile(attachment, out convertedFileDetails);
            this.UploadVersionPdfFile(convertedFileDetails.FilePath, attachment.ObjectId, 1);
            filepath = this.GenerateVersionXFileAndUploadFileToSkydrive(attachment, convertedFileDetails.FilePath, str2, false);
            ++attachment.CurrentSDCDocument.Version;
          }
          if (filepath != null)
          {
            attachment.SetConvertedFile(filepath);
            convertedVersionFile = filepath;
          }
          else
          {
            attachment.SetConvertedFile(str1);
            convertedVersionFile = str1;
          }
        }
        else if (string.IsNullOrEmpty(convertedVersionFile))
        {
          try
          {
            Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Get presigned for download partner file for skyDriveObjectId-" + attachment.ObjectId);
            SkyDriveUrl partnerFileDownload = this.GetPreSignedForPartnerFileDownload(attachment.ObjectId);
            Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Started downloading partner file 'document.json' for skyDriveObjectId-" + attachment.ObjectId);
            bool flag = true;
            SDCDocument sdcDocument1 = (SDCDocument) null;
            lock (this.nativeFileDownloadLock)
            {
              try
              {
                Task<BinaryObject> partnerFile = this.GetPartnerFile(partnerFileDownload, attachment.ObjectId, "document.json", attachment.ID);
                Task.WaitAll((Task) partnerFile);
                if (partnerFile.Result != null)
                {
                  sdcDocument1 = JsonConvert.DeserializeObject<SDCDocument>(partnerFile.Result.ToString(Encoding.Default));
                  partnerFile.Result.Dispose();
                  if (!this.isEFolderActiveUser)
                  {
                    if (sdcDocument1.Pages != null)
                    {
                      if (sdcDocument1.PageCount == 0)
                      {
                        if (sdcDocument1.Pages.Any<Pages>())
                          goto label_22;
                      }
                      else
                        goto label_22;
                    }
                    flag = false;
                  }
                }
              }
              catch (Exception ex) when (ex.InnerException?.GetType() == typeof (HttpException) && ((HttpException) ex.InnerException).GetHttpCode() == 404)
              {
                flag = false;
              }
label_22:
              if (!flag)
              {
                SDCHelper.FileDetails convertedFileDetails;
                this.GenerateVersion1PdfFromOriginalFile(attachment, out convertedFileDetails);
                BinaryObject binaryObject = new BinaryObject(convertedFileDetails.FilePath);
                SDCDocument sdcDocument2 = new SDCDocument(1, "Incomplete", convertedFileDetails.PageCount)
                {
                  PageCount = convertedFileDetails.PageCount,
                  ContentLength = convertedFileDetails.FileSize
                };
                this.UploadPartnerFilesForNewAttachment(new Dictionary<string, BinaryObject>()
                {
                  {
                    "document.json",
                    new BinaryObject(sdcDocument2.ToCamelCaseJsonString(), Encoding.Default)
                  },
                  {
                    "version-1.pdf",
                    binaryObject
                  }
                }, attachment.ObjectId);
                attachment.SetConvertedFile(convertedFileDetails.FilePath);
                attachment.OriginalSDCDocument = sdcDocument2;
                convertedVersionFile = convertedFileDetails.FilePath;
                return convertedVersionFile;
              }
              attachment.OriginalSDCDocument = sdcDocument1;
              string str = string.Format("version-{0}.pdf", (object) sdcDocument1.Version);
              convertedVersionFile = SystemSettings.GetTempFileNameWithGivenFileName(attachment.ObjectId.Contains("KS*") ? attachment.ID : attachment.ObjectId, str);
              if (File.Exists(convertedVersionFile))
              {
                attachment.SetConvertedFile(convertedVersionFile);
                return convertedVersionFile;
              }
              Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Started downloading partner file " + str + " for skyDriveObjectId-" + attachment.ObjectId);
              try
              {
                convertedVersionFile = this.DownloadVersionPdfFile(attachment.ObjectId, sdcDocument1.Version, attachment.ID);
                attachment.SetConvertedFile(convertedVersionFile);
                return convertedVersionFile;
              }
              catch (Exception ex1) when (ex1.InnerException?.GetType() == typeof (HttpException) && ((HttpException) ex1.InnerException).GetHttpCode() == 404)
              {
                Tracing.Log(SDCHelper.sw, TraceLevel.Error, nameof (SDCHelper), "SkyDriveClassic: " + str + " for skyDriveObjectId-" + attachment.ObjectId + " not found");
                if (sdcDocument1.Version == 1)
                {
                  SDCHelper.FileDetails convertedFileDetails;
                  this.GenerateVersion1PdfFromOriginalFile(attachment, out convertedFileDetails);
                  this.UploadVersionPdfFile(convertedFileDetails.FilePath, attachment.ObjectId, 1);
                  attachment.SetConvertedFile(convertedFileDetails.FilePath);
                  convertedVersionFile = convertedFileDetails.FilePath;
                }
                else
                {
                  string filepath = (string) null;
                  try
                  {
                    string versionXPdfFilePath = this.DownloadVersionPdfFile(attachment.ObjectId, 1, attachment.ID);
                    if (sdcDocument1.Version > 1)
                    {
                      if (!string.IsNullOrEmpty(versionXPdfFilePath))
                        filepath = this.GenerateVersionXFileAndUploadFileToSkydrive(attachment, versionXPdfFilePath, str);
                    }
                  }
                  catch (Exception ex2) when (ex2.InnerException?.GetType() == typeof (HttpException) && ((HttpException) ex2.InnerException).GetHttpCode() == 404)
                  {
                    Tracing.Log(SDCHelper.sw, TraceLevel.Error, nameof (SDCHelper), "SkyDriveClassic: version-1.pdf for skyDriveObjectId-" + attachment.ObjectId + " not found");
                    SDCHelper.FileDetails convertedFileDetails;
                    this.GenerateVersion1PdfFromOriginalFile(attachment, out convertedFileDetails);
                    this.UploadVersionPdfFile(convertedFileDetails.FilePath, attachment.ObjectId, 1);
                    filepath = this.GenerateVersionXFileAndUploadFileToSkydrive(attachment, convertedFileDetails.FilePath, str);
                  }
                  attachment.SetConvertedFile(filepath);
                  convertedVersionFile = filepath;
                }
              }
            }
          }
          catch (Exception ex)
          {
            Tracing.Log(SDCHelper.sw, TraceLevel.Error, nameof (SDCHelper), string.Format("SkyDriveClassic: Error in GetConvertedFile method for skyDriveObjectId-{0} and exception-{1}", (object) attachment.ObjectId, (object) ex));
            throw;
          }
        }
      }
      return convertedVersionFile;
    }

    private void GenerateVersion1PdfFromOriginalFile(
      NativeAttachment attachment,
      out SDCHelper.FileDetails convertedFileDetails)
    {
      string inputFilePath = this.DownloadOriginalAttachment(attachment.ObjectId, attachment.ID);
      convertedFileDetails = this.ConvertFileToPDF(inputFilePath, attachment.ObjectId.Contains("KS*") ? attachment.ID : attachment.ObjectId, "version-1.pdf");
      if (convertedFileDetails != null)
        return;
      Tracing.Log(SDCHelper.sw, TraceLevel.Error, nameof (SDCHelper), "SkyDriveClassic: Error in PDF conversion for skyDriveObjectId-" + attachment.ObjectId);
    }

    private string GenerateVersionXFileAndUploadFileToSkydrive(
      NativeAttachment attachment,
      string versionXPdfFilePath,
      string versionXFileName,
      bool isSkyDriveUploadRequired = true)
    {
      Tracing.Log(SDCHelper.sw, TraceLevel.Error, nameof (SDCHelper), "SkyDriveClassic: Generate version-x.pdf file and upload file to skydrive for skyDriveObjectId-" + attachment.ObjectId);
      try
      {
        PdfCreator pdfCreator = new PdfCreator();
        SDCMapper sdcMapper = new SDCMapper();
        SDCDocument doc = isSkyDriveUploadRequired || attachment.CurrentSDCDocument == null ? attachment.OriginalSDCDocument : attachment.CurrentSDCDocument;
        Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Calling SplitFile");
        string[] strArray = pdfCreator.SplitFile(versionXPdfFilePath);
        string[] fileList = new string[doc.Pages.Count];
        int index = 0;
        foreach (Pages page in doc.Pages)
        {
          fileList[index] = strArray[page.Id - 1];
          if (page.Rotation > 0)
          {
            using (PdfEditor pdfEditor = new PdfEditor(fileList[index]))
              fileList[index] = pdfEditor.Rotate(page.Rotation, fileList[index]);
          }
          ++index;
        }
        Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Calling MergeFiles");
        string str = pdfCreator.MergeFiles(fileList);
        if (doc.Annotations != null && doc.Annotations.Any<Annotation>())
        {
          Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Calling PdfTextAnnotationMapper");
          List<PdfTextAnnotation> annotations = sdcMapper.PdfTextAnnotationMapper(doc);
          using (PdfEditor pdfEditor = new PdfEditor(str))
          {
            Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Calling AddAnnotations");
            str = pdfEditor.AddAnnotations(annotations, str);
          }
        }
        string withGivenFileName = SystemSettings.GetTempFileNameWithGivenFileName(attachment.ObjectId.Contains("KS*") ? attachment.ID : attachment.ObjectId, versionXFileName);
        File.Move(str, withGivenFileName);
        if (isSkyDriveUploadRequired)
        {
          Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Calling UploadVersionPdfFile for uploading versionX PDF file on skydrive for skydriveObjectID-" + attachment.ObjectId);
          this.UploadVersionPdfFile(withGivenFileName, attachment.ObjectId, doc.Version);
        }
        return withGivenFileName;
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCHelper.sw, TraceLevel.Error, nameof (SDCHelper), string.Format("SkyDriveClassic: Error generating version-x.pdf file and upload file to skydrive for skyDriveObjectId-{0}. Ex: {1}", (object) attachment.ObjectId, (object) ex));
        throw;
      }
    }

    public string DownloadOriginalAttachment(string sdObjectId, string attachmentId)
    {
      SkyDriveStreamingClient driveStreamingClient = new SkyDriveStreamingClient(this.LoanDataMgr);
      Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Calling GetPresignedUrlForPartnerFilesDownload for skydriveobjectID-" + sdObjectId);
      SkyDriveUrl partnerFilesDownload = driveStreamingClient.GetPresignedUrlForPartnerFilesDownload(sdObjectId);
      Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Calling DownloadFile for attachmentID-" + attachmentId);
      lock (this.nativeFileDownloadLock)
      {
        Task<string> task = driveStreamingClient.DownloadFile(partnerFilesDownload, attachmentId, true);
        Task.WaitAll((Task) task);
        return task.Result;
      }
    }

    private void UploadVersionPdfFile(string filePath, string sdObjectId, int version)
    {
      BinaryObject data = new BinaryObject(filePath);
      string str = string.Format(string.Format("version-{0}.pdf", (object) version));
      Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Get presigned for uploading partner file for skyDriveObjectId-" + sdObjectId);
      SkyDriveUrl partnerFileUpload = this.GetPreSignedForPartnerFileUpload(sdObjectId, str);
      Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Started uploading partner file " + str + " for skyDriveObjectId-" + sdObjectId);
      Task.WaitAll((Task) this.SavePartnerFile(partnerFileUpload, str, data));
      data.Dispose();
    }

    private string DownloadVersionPdfFile(string sdObjectId, int version, string attachmentId)
    {
      try
      {
        Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Get presigned for download partner file for skyDriveObjectId-" + sdObjectId);
        SkyDriveUrl partnerFileDownload = this.GetPreSignedForPartnerFileDownload(sdObjectId);
        string str = string.Format(string.Format("version-{0}.pdf", (object) version));
        Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Started downloading VersionX PDF file from skydrive with skydriveObjectID-" + sdObjectId);
        Task<BinaryObject> partnerFile;
        lock (this.nativeFileDownloadLock)
        {
          partnerFile = this.GetPartnerFile(partnerFileDownload, sdObjectId, str, attachmentId);
          Task.WaitAll((Task) partnerFile);
        }
        string withGivenFileName = SystemSettings.GetTempFileNameWithGivenFileName(sdObjectId.Contains("KS*") ? attachmentId : sdObjectId, str);
        partnerFile.Result.Write(withGivenFileName);
        partnerFile.Dispose();
        return withGivenFileName;
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCHelper.sw, TraceLevel.Error, nameof (SDCHelper), string.Format("SkyDriveClassic: Error in download PDF files from skydrive for skyDriveObjectId-{0}. Ex: {1}", (object) sdObjectId, (object) ex));
        throw;
      }
    }

    public async Task SavePartnerFilesToSkyDrive(NativeAttachment sdcAttachment)
    {
      string sdObjectId = (string) null;
      try
      {
        if (sdcAttachment.CurrentSDCDocument == null || sdcAttachment.OriginalSDCDocument == null)
          return;
        string newJson = JsonConvert.SerializeObject((object) sdcAttachment.CurrentSDCDocument);
        if (!this.HasDocJsonChanged(JsonConvert.SerializeObject((object) sdcAttachment.OriginalSDCDocument), newJson))
          return;
        int newDocVersion = sdcAttachment.OriginalSDCDocument.Version + 1;
        sdcAttachment.CurrentSDCDocument.Version = newDocVersion;
        string currentDocJsonString = sdcAttachment.CurrentSDCDocument.ToCamelCaseJsonString();
        BinaryObject documentBinaryData = new BinaryObject(currentDocJsonString, Encoding.Default);
        sdObjectId = sdcAttachment.ObjectId;
        Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Get presigned for uploading partner file for skyDriveObjectId-" + sdObjectId);
        SkyDriveUrl partnerFileUpload = this.GetPreSignedForPartnerFileUpload(sdObjectId, "document.json");
        Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Started uploading partner file 'document.json' for skyDriveObjectId-" + sdObjectId);
        string str = await this.SavePartnerFile(partnerFileUpload, "document.json", documentBinaryData);
        documentBinaryData.Dispose();
        sdcAttachment.OriginalSDCDocument = JsonConvert.DeserializeObject<SDCDocument>(currentDocJsonString);
        Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Started uploading partner file 'version-x.pdf' for skyDriveObjectId-" + sdObjectId);
        this.UploadVersionPdfFile(sdcAttachment.GetConvertedFile(), sdObjectId, newDocVersion);
        currentDocJsonString = (string) null;
        documentBinaryData = (BinaryObject) null;
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCHelper.sw, TraceLevel.Error, nameof (SDCHelper), string.Format("SkyDriveClassic: Error in saving partner files on skydrive for skyDriveObjectId-{0}. Ex: {1}", (object) sdObjectId, (object) ex));
        throw;
      }
    }

    public void SplitDocumentJson(
      SDCDocument sourceDocument,
      SDCDocument separatedJson,
      List<int> pageIndexes)
    {
      try
      {
        int index = 0;
        SDCAnnotationsHelper annotationsHelper = new SDCAnnotationsHelper();
        SDCBookmarksHelper sdcBookmarksHelper = new SDCBookmarksHelper();
        Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "Calling MoveSplitPageAnnotations");
        bool flag = annotationsHelper.MoveSplitPageAnnotations(sourceDocument, separatedJson, pageIndexes);
        Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "Calling MoveSplitPageBookMarks");
        if (sdcBookmarksHelper.MoveSplitPageBookMarks(sourceDocument, separatedJson, pageIndexes))
          flag = true;
        List<int> pageIDs = new List<int>();
        foreach (int pageIndex in pageIndexes)
        {
          int pageId = pageIndex;
          int id = separatedJson.Pages[index].Id;
          Pages pages = Utils.DeepClone<Pages>(sourceDocument.Pages.Single<Pages>((Func<Pages, bool>) (page => page.Id == pageId)));
          separatedJson.Pages[index] = pages;
          separatedJson.Pages[index].Id = id;
          if (pages.Rotation != 0)
            flag = true;
          ++index;
          Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "Calling DeletePages");
          pageIDs.Add(pageId);
        }
        this.DeletePages(sourceDocument, pageIDs);
        if (!flag)
          return;
        ++separatedJson.Version;
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCHelper.sw, TraceLevel.Error, nameof (SDCHelper), string.Format("SkyDriveClassic: Error in splitting document.json in split files. Ex: {0}", (object) ex));
        throw;
      }
    }

    public string CreateNextVersionFile(string filePath, int currentVesion)
    {
      string str = Path.GetDirectoryName(filePath) + ("\\" + string.Format(string.Format("version-{0}.pdf", (object) (currentVesion + 1))));
      if (File.Exists(str))
        File.Delete(str);
      File.Copy(filePath, str);
      filePath = str;
      return filePath;
    }

    public string RemoveEditsFromPage(
      NativeAttachment attachment,
      int pageId,
      string currentPagePath)
    {
      SDCDocument sdcDocument = attachment.CurrentSDCDocument == null ? Utils.DeepClone<SDCDocument>(attachment.OriginalSDCDocument) : Utils.DeepClone<SDCDocument>(attachment.CurrentSDCDocument);
      string str = string.Empty;
      try
      {
        if (sdcDocument.Pages.Single<Pages>((Func<Pages, bool>) (page => page.Id == pageId)).Rotation != 0)
        {
          SystemSettings.GetTempfileCounter();
          str = this.GetCopyPagePath(currentPagePath);
          File.Copy(currentPagePath, str);
          using (PdfEditor pdfEditor = new PdfEditor(str))
            str = pdfEditor.ResetPageRotation();
        }
        if (sdcDocument.Annotations != null && sdcDocument.Annotations.Any<Annotation>((Func<Annotation, bool>) (annots => annots.PageId == pageId)))
        {
          if (string.IsNullOrWhiteSpace(str))
          {
            str = this.GetCopyPagePath(currentPagePath);
            File.Copy(currentPagePath, str);
          }
          using (PdfEditor pdfEditor = new PdfEditor(str))
          {
            Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: DeletePageAnnotation: " + (object) pageId);
            str = pdfEditor.DeletePageAnnotations();
          }
        }
        if (string.IsNullOrWhiteSpace(str))
          return currentPagePath;
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCHelper.sw, TraceLevel.Error, nameof (SDCHelper), string.Format("SkyDriveClassic: Error while removeing any existing edits from the page. Ex: {0}", (object) ex));
        throw;
      }
      return str;
    }

    public void UploadPartnerFilesForNewAttachment(
      Dictionary<string, BinaryObject> partnerFileBinaryObjects,
      string objectId)
    {
      SkyDriveUrl presigned = (SkyDriveUrl) null;
      for (int i = 0; i < partnerFileBinaryObjects.Count; i++)
      {
        KeyValuePair<string, BinaryObject> keyValuePair = partnerFileBinaryObjects.ElementAt<KeyValuePair<string, BinaryObject>>(i);
        string partnerFileName = keyValuePair.Key;
        Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Get presigned for uploading partner file for skyDriveObjectId-" + objectId);
        if (partnerFileName == "document.json" || partnerFileName == "version-1.pdf")
        {
          Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "Calling GetPreSignedForPartnerFileUpload");
          presigned = this.GetPreSignedForPartnerFileUpload(objectId, partnerFileName);
        }
        Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Started uploading partner file '" + partnerFileName + "' for skyDriveObjectId-" + objectId);
        Task.WaitAll((Task) Task.Run<string>((Func<Task<string>>) (() => this.SavePartnerFile(presigned, partnerFileName, partnerFileBinaryObjects.ElementAt<KeyValuePair<string, BinaryObject>>(i).Value))));
        keyValuePair = partnerFileBinaryObjects.ElementAt<KeyValuePair<string, BinaryObject>>(i);
        keyValuePair.Value.Dispose();
      }
    }

    public SDCDocument CreateMergedSDCDocument(List<FileAttachment> fileList)
    {
      int pageCounter = 1;
      bool flag1 = false;
      bool flag2 = false;
      SDCDocument sdcDocumentCopy = new SDCDocument(1, "Incomplete", 0);
      List<Bookmark> source = new List<Bookmark>();
      Dictionary<int, int> pageLookOut = new Dictionary<int, int>();
      foreach (FileAttachment file in fileList)
      {
        source.Clear();
        pageLookOut.Clear();
        NativeAttachment nativeAttachment = (NativeAttachment) file;
        try
        {
          SDCDocument sdcDocument = this.LoanDataMgr.IsAutosaveEnabled || nativeAttachment.CurrentSDCDocument == null ? nativeAttachment.OriginalSDCDocument : nativeAttachment.CurrentSDCDocument;
          if (sdcDocument.Version > 1)
          {
            flag1 = true;
            flag2 = true;
          }
          else if (nativeAttachment.CurrentSDCDocument != null)
          {
            Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "Calling HasDocJsonChanged");
            if (this.HasDocJsonChanged(JsonConvert.SerializeObject((object) nativeAttachment.OriginalSDCDocument), JsonConvert.SerializeObject((object) nativeAttachment.CurrentSDCDocument)))
            {
              flag1 = true;
              flag2 = true;
            }
          }
          foreach (Pages page1 in sdcDocument.Pages)
          {
            Pages page = page1;
            Pages pages = new Pages()
            {
              Id = pageCounter,
              ContentLength = page.ContentLength,
              Rotation = page.Rotation,
              Page = page.Page,
              Thumbnail = page.Thumbnail,
              Size = page.Size
            };
            sdcDocumentCopy.Pages.Add(pages);
            if (flag2)
            {
              SDCAnnotationsHelper annotationsHelper = new SDCAnnotationsHelper();
              if (sdcDocument.Annotations != null && sdcDocument.Annotations.Where<Annotation>((Func<Annotation, bool>) (a => a.PageId == page.Id)).Any<Annotation>())
              {
                List<Annotation> list = sdcDocument.Annotations.Where<Annotation>((Func<Annotation, bool>) (a => a.PageId == page.Id)).ToList<Annotation>();
                Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "Calling MergeFilesAnnotations");
                foreach (Annotation fileAnnotation in list)
                  annotationsHelper.MergeFilesAnnotations(sdcDocumentCopy, fileAnnotation, pageCounter);
              }
            }
            pageLookOut.Add(page.Id, pageCounter);
            ++pageCounter;
          }
          if (sdcDocument.Bookmarks != null)
          {
            SDCBookmarksHelper sdcBookmarksHelper = new SDCBookmarksHelper();
            Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "Calling RebaseChildBookMark");
            source = sdcBookmarksHelper.RebaseChildBookMark(sdcDocument.Bookmarks, pageLookOut);
          }
          if (sdcDocument.Bookmarks != null)
          {
            if (source.Any<Bookmark>())
            {
              foreach (Bookmark bookmark1 in source)
              {
                Bookmark bookmark2 = new Bookmark()
                {
                  Id = bookmark1.Id,
                  Name = bookmark1.Name,
                  PageId = bookmark1.PageId,
                  Type = bookmark1.Type,
                  Top = bookmark1.Top,
                  Bottom = bookmark1.Bottom,
                  Left = bookmark1.Left,
                  Right = bookmark1.Right,
                  Children = bookmark1.Children
                };
                if (sdcDocumentCopy.Bookmarks == null)
                  sdcDocumentCopy.Bookmarks = new List<Bookmark>()
                  {
                    bookmark2
                  };
                else
                  sdcDocumentCopy.Bookmarks.Add(bookmark2);
              }
            }
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(SDCHelper.sw, TraceLevel.Error, nameof (SDCHelper), string.Format("SkyDriveClassic: Error merging SDCDocument file with skyDriveObjectId-{0} into new merged SDCDocument file. Ex: {1}", (object) nativeAttachment.ObjectId, (object) ex));
          throw;
        }
      }
      sdcDocumentCopy.PageCount = pageCounter - 1;
      if (flag1)
        sdcDocumentCopy.Version = 2;
      return sdcDocumentCopy;
    }

    public string CreateUnmodifiedMergedFile(List<FileAttachment> files)
    {
      List<string> stringList = new List<string>();
      PdfCreator pdfCreator = new PdfCreator();
      try
      {
        foreach (NativeAttachment file in files)
        {
          SDCDocument sdcDocument = file.CurrentSDCDocument == null ? file.OriginalSDCDocument : file.CurrentSDCDocument;
          Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "Calling SplitFile");
          string[] strArray = pdfCreator.SplitFile(file.GetConvertedFile());
          int index = 0;
          Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "Calling RemoveEditsFromPage");
          foreach (Pages page in sdcDocument.Pages)
          {
            string currentPagePath = strArray[index];
            string str = this.RemoveEditsFromPage(file, page.Id, currentPagePath);
            stringList.Add(str);
            ++index;
          }
        }
        return pdfCreator.MergeFiles(stringList.ToArray());
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCHelper.sw, TraceLevel.Error, nameof (SDCHelper), string.Format("SkyDriveClassic: Error in creating unmodified version of selected Files for merge. Ex: {0}", (object) ex));
        throw;
      }
    }

    public ImageProperties[] ConvertAttachmentToImages(string filePath)
    {
      ImageCreator imageCreator = new ImageCreator(Path.GetDirectoryName(filePath), ImageConversionType.Automatic, 150f, 150f, this.LoanDataMgr.UseSkyDriveClassic);
      Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Started converting documents into Images/Thumbnails");
      return imageCreator.ConvertFile(filePath);
    }

    public void UploadAttachmentImagesToSkyDrive(
      ImageProperties[] imageList,
      string sdObjectId,
      SDCDocument sdcDocument)
    {
      ConcurrentQueue<Exception> innerExceptions = new ConcurrentQueue<Exception>();
      try
      {
        Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), string.Format("SkyDriveClassic: Setting MaxDegreeOfParallelism to {0} for uploading images/thumbnails for skyDriveObjectId-{1} ", (object) SDCHelper.DegreeOfParallelism, (object) sdObjectId));
        ImageProperties[] source = imageList;
        ParallelOptions parallelOptions = new ParallelOptions();
        parallelOptions.MaxDegreeOfParallelism = SDCHelper.DegreeOfParallelism;
        Action<ImageProperties> body = (Action<ImageProperties>) (image =>
        {
          BinaryObject imageData = new BinaryObject(image.ImageFile);
          string imageFileName = "pages//" + Path.GetFileName(image.ImageFile);
          BinaryObject thumbnailData = new BinaryObject(image.Thumbnail.ImageFile);
          string thumbnailFileName = "thumbnails//" + Path.GetFileName(image.Thumbnail.ImageFile);
          Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Get presigned for uploading Images/Thumbnails for skyDriveObjectId-" + sdObjectId);
          SkyDriveUrl presigned = this.GetPreSignedForPartnerFileUpload(sdObjectId);
          Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Started uploading Images for skyDriveObjectId-" + sdObjectId);
          Task.WaitAll((Task) Task.Run<string>((Func<Task<string>>) (() => this.SavePartnerFile(presigned, imageFileName, imageData))));
          Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Started uploading Thumbnails for skyDriveObjectId-" + sdObjectId);
          Task.WaitAll((Task) Task.Run<string>((Func<Task<string>>) (() => this.SavePartnerFile(presigned, thumbnailFileName, thumbnailData))));
        });
        Parallel.ForEach<ImageProperties>((IEnumerable<ImageProperties>) source, parallelOptions, body);
      }
      catch (Exception ex)
      {
        innerExceptions.Enqueue(ex);
      }
      if (innerExceptions.Count > 0)
        throw new AggregateException("SkyDriveClassic: Error while uploading Images/Thumbnails for skyDriveObjectId-" + sdObjectId, (IEnumerable<Exception>) innerExceptions);
      this.MapDocumentJsonWithImages(imageList, sdcDocument);
      BinaryObject documentBinaryData = new BinaryObject(sdcDocument.ToCamelCaseJsonString(), Encoding.Default);
      Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Get presigned for uploading document json after image conversion for skyDriveObjectId-" + sdObjectId);
      SkyDriveUrl presigned1 = this.GetPreSignedForPartnerFileUpload(sdObjectId, "document.json");
      Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Started uploading document json after image conversion for skyDriveObjectId-" + sdObjectId);
      Task.WaitAll((Task) Task.Run<string>((Func<Task<string>>) (() => this.SavePartnerFile(presigned1, "document.json", documentBinaryData))));
      documentBinaryData.Dispose();
    }

    public void MapDocumentJsonWithImages(ImageProperties[] imageList, SDCDocument sdcDocument)
    {
      foreach (Pages page1 in sdcDocument.Pages)
      {
        Pages page = page1;
        ImageProperties imageProperties = ((IEnumerable<ImageProperties>) imageList).Where<ImageProperties>((Func<ImageProperties, bool>) (img => SDCUtils.GetPageIdFromFileName(img.ImageFile) == page.Id)).FirstOrDefault<ImageProperties>();
        if (imageProperties != null)
        {
          page.Page = true;
          page.Thumbnail = true;
          page.Size = new List<int>()
          {
            imageProperties.Width,
            imageProperties.Height
          };
        }
      }
    }

    private void DownloadImagesAndThumbnailsFromSkyDrive(
      string sdObjectId,
      string filePathDirectory,
      int[] pagecount)
    {
      ConcurrentDictionary<string, byte[]> imageDownloads = new ConcurrentDictionary<string, byte[]>();
      ConcurrentDictionary<string, byte[]> thumbnailDownloads = new ConcurrentDictionary<string, byte[]>();
      ConcurrentQueue<Exception> innerExceptions = new ConcurrentQueue<Exception>();
      try
      {
        Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), string.Format("SkyDriveClassic: Setting MaxDegreeOfParallelism to {0} for downloading images/thumbnails for skyDriveObjectId-{1} ", (object) SDCHelper.DegreeOfParallelism, (object) sdObjectId));
        int[] source = pagecount;
        ParallelOptions parallelOptions = new ParallelOptions();
        parallelOptions.MaxDegreeOfParallelism = SDCHelper.DegreeOfParallelism;
        Action<int> body = (Action<int>) (pageid =>
        {
          string partnerFile1 = "pages//" + (pageid.ToString() + ".png");
          string partnerFile2 = "thumbnails//" + (pageid.ToString() + ".png");
          Tracing.Log(SDCHelper.sw, TraceLevel.Verbose, nameof (SDCHelper), "SkyDriveClassic: Get presigned for download partner file for skyDriveObjectId-" + sdObjectId);
          SkyDriveUrl partnerFileDownload = this.GetPreSignedForPartnerFileDownload(sdObjectId);
          Task<BinaryObject> partnerFile3 = this.GetPartnerFile(partnerFileDownload, sdObjectId, partnerFile1);
          imageDownloads[pageid.ToString() + ".png"] = partnerFile3.Result.GetBytes();
          Task<BinaryObject> partnerFile4 = this.GetPartnerFile(partnerFileDownload, sdObjectId, partnerFile2);
          thumbnailDownloads[pageid.ToString() + ".png"] = partnerFile4.Result.GetBytes();
        });
        Parallel.ForEach<int>((IEnumerable<int>) source, parallelOptions, body);
      }
      catch (Exception ex)
      {
        innerExceptions.Enqueue(ex);
      }
      if (innerExceptions.Count > 0)
        throw new AggregateException("SkyDriveClassic: Error while downloading Images/Thumbnails for skyDriveObjectId-" + sdObjectId, (IEnumerable<Exception>) innerExceptions);
      this.SaveFilesInLocalCache(filePathDirectory + "\\pages\\", imageDownloads);
      this.SaveFilesInLocalCache(filePathDirectory + "\\thumbnails\\", thumbnailDownloads);
    }

    public void SaveFilesInLocalCache(
      string fileDirectoryPath,
      ConcurrentDictionary<string, byte[]> dict)
    {
      if (!Directory.Exists(fileDirectoryPath))
        Directory.CreateDirectory(fileDirectoryPath);
      foreach (KeyValuePair<string, byte[]> keyValuePair in dict)
        File.WriteAllBytes(Path.Combine(fileDirectoryPath, keyValuePair.Key), keyValuePair.Value);
    }

    private string GetCopyPagePath(string currentPagePath)
    {
      int tempfileCounter = SystemSettings.GetTempfileCounter();
      string path2 = Path.GetFileNameWithoutExtension(currentPagePath) + "-CopyPage" + (object) tempfileCounter + ".pdf";
      return Path.Combine(Path.GetDirectoryName(currentPagePath), path2);
    }

    public void RebaseDocJSonPageIds(SDCDocument doc)
    {
      if (doc.PageCount <= 0)
        return;
      Dictionary<int, int> pageIdMap = this.RebasePageIds(doc);
      List<Annotation> annotations = doc.Annotations;
      if ((annotations != null ? (annotations.Any<Annotation>() ? 1 : 0) : 0) != 0)
        SDCAnnotationsHelper.RebaseAnnotationPageIds(doc.Annotations, pageIdMap);
      List<Bookmark> bookmarks = doc.Bookmarks;
      if ((bookmarks != null ? (bookmarks.Any<Bookmark>() ? 1 : 0) : 0) == 0)
        return;
      SDCBookmarksHelper sdcBookmarksHelper = new SDCBookmarksHelper();
      doc.Bookmarks = sdcBookmarksHelper.RebasePageIndexesForSplitBookMarks(doc.Bookmarks, pageIdMap.Keys.ToList<int>());
    }

    private Dictionary<int, int> RebasePageIds(SDCDocument doc)
    {
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      int num = 1;
      for (int index = 0; index < doc.Pages.Count; ++index)
      {
        dictionary.Add(doc.Pages[index].Id, num);
        doc.Pages[index].Id = num;
        ++num;
      }
      return dictionary;
    }

    public class FileDetails
    {
      public FileDetails(string filepath, int pageCount, long fileSize)
      {
        this.FilePath = filepath;
        this.PageCount = pageCount;
        this.FileSize = fileSize;
      }

      public string FilePath { get; private set; }

      public int PageCount { get; private set; }

      public long FileSize { get; private set; }
    }
  }
}
