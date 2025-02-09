// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.UploadQueueItem
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.SkyDrive;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Xml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.eFolder.Files
{
  public class UploadQueueItem
  {
    private const string className = "UploadQueueItem�";
    private static readonly string sw = Tracing.SwEFolder;
    private string manifestFile;
    private FileAttachment attachment;
    private string loanGuid;
    private string loanNumber;
    private string borrowerName;
    private long originalFileSize;
    private long totalBytes;
    private long uploadedBytes;

    public UploadQueueItem(
      BackgroundAttachment attachment,
      string loanGuid,
      string loanNumber,
      string borrowerName,
      long originalFileSize)
    {
      this.attachment = (FileAttachment) new NativeAttachment(attachment, originalFileSize);
      this.loanGuid = loanGuid;
      this.loanNumber = loanNumber;
      this.borrowerName = borrowerName;
      this.originalFileSize = originalFileSize;
    }

    public UploadQueueItem(ConversionQueueItem conversionItem, PageImage[] pageList)
    {
      this.attachment = (FileAttachment) new ImageAttachment(conversionItem.Attachment, pageList);
      this.loanGuid = conversionItem.LoanGUID;
      this.loanNumber = conversionItem.LoanNumber;
      this.borrowerName = conversionItem.BorrowerName;
      this.originalFileSize = conversionItem.OriginalFileSize;
    }

    public UploadQueueItem(string manifestFile)
    {
      this.manifestFile = manifestFile;
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load(manifestFile);
      AttributeReader attributeReader = new AttributeReader(xmlDocument.DocumentElement);
      this.loanGuid = attributeReader.GetString(nameof (LoanGUID));
      this.loanNumber = attributeReader.GetString(nameof (LoanNumber));
      this.borrowerName = attributeReader.GetString(nameof (BorrowerName));
      this.originalFileSize = attributeReader.GetLong(nameof (OriginalFileSize));
      XmlElement elm = (XmlElement) xmlDocument.DocumentElement.SelectSingleNode("File") ?? (XmlElement) xmlDocument.DocumentElement.SelectSingleNode("Image");
      switch (elm.Name)
      {
        case "File":
          this.attachment = (FileAttachment) new NativeAttachment(elm, false);
          break;
        case "Image":
          this.attachment = (FileAttachment) new ImageAttachment(elm, false);
          break;
      }
    }

    public FileAttachment Attachment => this.attachment;

    public string LoanGUID => this.loanGuid;

    public string LoanNumber => this.loanNumber;

    public string BorrowerName => this.borrowerName;

    public long OriginalFileSize => this.originalFileSize;

    public void CreateManifest(string manifestFile)
    {
      this.manifestFile = manifestFile;
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml("<QueueItem/>");
      AttributeWriter attributeWriter = new AttributeWriter(xmlDocument.DocumentElement);
      attributeWriter.Write("LoanGUID", (object) this.loanGuid);
      attributeWriter.Write("LoanNumber", (object) this.loanNumber);
      attributeWriter.Write("BorrowerName", (object) this.borrowerName);
      attributeWriter.Write("OriginalFileSize", (object) this.originalFileSize);
      string name = "File";
      if (this.attachment is ImageAttachment)
        name = "Image";
      XmlElement element = xmlDocument.CreateElement(name);
      xmlDocument.DocumentElement.AppendChild((XmlNode) element);
      this.attachment.ToXml(element);
      xmlDocument.Save(this.manifestFile);
    }

    public void RemoveManifest()
    {
      List<string> stringList = new List<string>();
      stringList.Add(this.manifestFile);
      if (this.attachment is ImageAttachment)
      {
        ImageAttachment attachment = (ImageAttachment) this.attachment;
        foreach (PageImage page in attachment.Pages)
        {
          string str = Path.Combine(BackgroundAttachmentDialog.UploadPath, page.ZipKey);
          if (!stringList.Contains(str))
            stringList.Add(str);
        }
        string str1 = Path.Combine(BackgroundAttachmentDialog.UploadPath, attachment.Pages[0].Thumbnail.ZipKey);
        stringList.Add(str1);
        if (!string.IsNullOrEmpty(attachment.Pages[0].NativeKey))
        {
          string str2 = Path.Combine(BackgroundAttachmentDialog.UploadPath, attachment.Pages[0].NativeKey);
          stringList.Add(str2);
        }
      }
      else if (this.attachment is NativeAttachment)
      {
        string str = Path.Combine(BackgroundAttachmentDialog.UploadPath, this.attachment.ID);
        stringList.Add(str);
      }
      foreach (string path in stringList)
      {
        try
        {
          if (File.Exists(path))
            File.Delete(path);
        }
        catch (Exception ex)
        {
          Tracing.Log(UploadQueueItem.sw, TraceLevel.Error, nameof (UploadQueueItem), ex.ToString());
        }
      }
    }

    public void Upload(ISession session, ISessionStartupInfo startupInfo)
    {
      using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("eFldrUpldQItemUpld", "DOCS NDE-13455 Upload eFolder document", true, 252, nameof (Upload), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\UploadQueueItem.cs"))
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 254, nameof (Upload), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\UploadQueueItem.cs");
        Tracing.Log(UploadQueueItem.sw, nameof (UploadQueueItem), TraceLevel.Info, "Upload starts at" + DateTime.Now.ToString());
        List<string> stringList = new List<string>();
        if (this.attachment is ImageAttachment)
        {
          ImageAttachment attachment = (ImageAttachment) this.attachment;
          foreach (PageImage page in attachment.Pages)
          {
            if (!stringList.Contains(page.ZipKey))
              stringList.Add(page.ZipKey);
          }
          stringList.Add(attachment.Pages[0].Thumbnail.ZipKey);
          if (!string.IsNullOrEmpty(attachment.Pages[0].NativeKey))
            stringList.Add(attachment.Pages[0].NativeKey);
        }
        else if (this.attachment is NativeAttachment)
          stringList.Add(this.attachment.ID);
        this.totalBytes = 0L;
        this.uploadedBytes = 0L;
        foreach (string str in stringList)
          this.totalBytes += new FileInfo(BackgroundAttachmentDialog.UploadPath + str).Length;
        PerformanceMeter.Current.AddCheckpoint("Start Upload", 295, nameof (Upload), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\UploadQueueItem.cs");
        ILoan loan = (ILoan) null;
        try
        {
          loan = ((ILoanManager) session.GetObject("LoanManager")).OpenLoan(this.loanGuid);
          bool flag = false;
          foreach (LoanProperty loanPropertySetting in loan.GetLoanPropertySettings())
          {
            if (string.Equals(loanPropertySetting.Category, "LoanStorage", StringComparison.OrdinalIgnoreCase) && string.Equals(loanPropertySetting.Attribute, "SupportingData", StringComparison.OrdinalIgnoreCase))
            {
              if (string.Equals(loanPropertySetting.Value, "SkyDriveLite", StringComparison.OrdinalIgnoreCase))
              {
                flag = true;
                break;
              }
              break;
            }
          }
          SkyDriveStreamingClient driveStreamingClient = (SkyDriveStreamingClient) null;
          if (flag)
            driveStreamingClient = new SkyDriveStreamingClient(loan, startupInfo);
          foreach (string str in stringList)
          {
            string path1 = Path.Combine(BackgroundAttachmentDialog.UploadPath, str);
            using (BinaryObject data = new BinaryObject(path1))
            {
              using (PerformanceMeter.Current.BeginOperation("SaveSupportingData"))
              {
                Tracing.Log(UploadQueueItem.sw, nameof (UploadQueueItem), TraceLevel.Info, "Upload:filepath(" + path1 + ")", data.Length);
                if (flag)
                {
                  driveStreamingClient.UploadProgress += new DownloadProgressEventHandler(this.uploadProgress);
                  try
                  {
                    Task.WaitAll((Task) driveStreamingClient.SaveSupportingData(str, data));
                  }
                  finally
                  {
                    driveStreamingClient.UploadProgress -= new DownloadProgressEventHandler(this.uploadProgress);
                  }
                }
                else
                {
                  data.UploadProgress += new DownloadProgressEventHandler(this.uploadProgress);
                  try
                  {
                    Tracing.Log(UploadQueueItem.sw, TraceLevel.Verbose, nameof (UploadQueueItem), "SaveSupportingDataOnCIFs: " + str);
                    loan.SaveSupportingDataOnCIFs(str, data);
                  }
                  finally
                  {
                    data.UploadProgress -= new DownloadProgressEventHandler(this.uploadProgress);
                  }
                }
                performanceMeter.AddVariable("Name", (object) str);
                performanceMeter.AddVariable("Size", (object) data.Length);
              }
              string path2 = SystemSettings.DownloadDir + str;
              if (path2.ToLower().EndsWith(".findingshtml"))
                path2 += ".htm";
              else if (path2.ToLower().EndsWith(".creditprintfile"))
                path2 += ".txt";
              Tracing.Log(UploadQueueItem.sw, TraceLevel.Verbose, nameof (UploadQueueItem), "Write: " + path2);
              data.Write(path2);
              this.uploadedBytes += data.Length;
              this.OnFileAttachmentUploadProgress(new TransferProgressEventArgs(Convert.ToInt32(100L * this.uploadedBytes / this.totalBytes)));
            }
          }
          if (BackgroundAttachmentDialog.IsRegistered(this.loanGuid))
          {
            Tracing.Log(UploadQueueItem.sw, TraceLevel.Verbose, nameof (UploadQueueItem), "ReplaceBackgroundAttachment: " + this.attachment.ID);
            loan.ReplaceBackgroundAttachment(this.attachment);
          }
          else
          {
            Tracing.Log(UploadQueueItem.sw, TraceLevel.Verbose, nameof (UploadQueueItem), "SaveFileAttachments: " + this.attachment.ID);
            loan.SaveFileAttachments(new FileAttachment[1]
            {
              this.attachment
            });
          }
          this.OnFileAttachmentUploadProgress(new TransferProgressEventArgs(100));
        }
        finally
        {
          loan?.Close();
        }
        Tracing.Log(UploadQueueItem.sw, nameof (UploadQueueItem), TraceLevel.Info, "Upload ends at" + DateTime.Now.ToString());
        PerformanceMeter.Current.AddCheckpoint("END", 422, nameof (Upload), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\UploadQueueItem.cs");
      }
    }

    private void uploadProgress(object source, DownloadProgressEventArgs e)
    {
      TransferProgressEventArgs e1 = new TransferProgressEventArgs(Convert.ToInt32(100L * (this.uploadedBytes + e.BytesDownloaded) / this.totalBytes));
      this.OnFileAttachmentUploadProgress(e1);
      if (!e1.Cancel)
        return;
      e.Cancel = true;
    }

    public event TransferProgressEventHandler FileAttachmentUploadProgress;

    private void OnFileAttachmentUploadProgress(TransferProgressEventArgs e)
    {
      if (this.FileAttachmentUploadProgress == null)
        return;
      this.FileAttachmentUploadProgress((object) this, e);
    }
  }
}
