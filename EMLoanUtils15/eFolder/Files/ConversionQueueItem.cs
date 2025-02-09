// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.ConversionQueueItem
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.eFolder;
using EllieMae.EMLite.DocumentConverter;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Xml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.eFolder.Files
{
  public class ConversionQueueItem
  {
    private const string className = "ConversionQueueItem�";
    private static readonly string sw = Tracing.SwEFolder;
    private string manifestFile;
    private BackgroundAttachment attachment;
    private string loanGuid;
    private string loanNumber;
    private string borrowerName;
    private long originalFileSize;

    public ConversionQueueItem(
      BackgroundAttachment attachment,
      string loanGuid,
      string loanNumber,
      string borrowerName,
      long originalFileSize)
    {
      this.attachment = attachment;
      this.loanGuid = loanGuid;
      this.loanNumber = loanNumber;
      this.borrowerName = borrowerName;
      this.originalFileSize = originalFileSize;
    }

    public ConversionQueueItem(string manifestFile)
    {
      this.manifestFile = manifestFile;
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load(manifestFile);
      AttributeReader attributeReader = new AttributeReader(xmlDocument.DocumentElement);
      this.loanGuid = attributeReader.GetString(nameof (LoanGUID));
      this.loanNumber = attributeReader.GetString(nameof (LoanNumber));
      this.borrowerName = attributeReader.GetString(nameof (BorrowerName));
      this.originalFileSize = attributeReader.GetLong(nameof (OriginalFileSize));
      this.attachment = new BackgroundAttachment((XmlElement) xmlDocument.DocumentElement.SelectSingleNode("Background"), false);
    }

    public BackgroundAttachment Attachment => this.attachment;

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
      XmlElement element = xmlDocument.CreateElement("Background");
      xmlDocument.DocumentElement.AppendChild((XmlNode) element);
      this.attachment.ToXml(element);
      xmlDocument.Save(this.manifestFile);
    }

    public void RemoveManifest()
    {
      foreach (string path in new List<string>()
      {
        this.manifestFile,
        BackgroundAttachmentDialog.ConversionPath + this.attachment.ID + this.attachment.Extension
      })
      {
        try
        {
          if (File.Exists(path))
            File.Delete(path);
        }
        catch (Exception ex)
        {
          Tracing.Log(ConversionQueueItem.sw, TraceLevel.Error, nameof (ConversionQueueItem), ex.ToString());
        }
      }
    }

    public PageImage[] Convert(long maxZipFileSize)
    {
      Tracing.Log(ConversionQueueItem.sw, TraceLevel.Verbose, nameof (ConversionQueueItem), "Convert: " + this.attachment.ID);
      ImageCreator imageCreator = new ImageCreator(SystemSettings.DownloadDir, this.attachment.ConversionType, (float) this.attachment.DpiX, (float) this.attachment.DpiY);
      imageCreator.ProgressChanged += new ProgressEventHandler(this.creator_ProgressChanged);
      List<PageImage> pageImageList = new List<PageImage>();
      try
      {
        string str1 = Path.Combine(BackgroundAttachmentDialog.ConversionPath, this.attachment.ID + this.attachment.Extension);
        ImageProperties[] imagePropertiesArray = imageCreator.ConvertFile(str1);
        string str2 = "Thumbnails-" + Guid.NewGuid().ToString() + ".zip";
        using (ZipWriter zipWriter1 = new ZipWriter(Path.Combine(BackgroundAttachmentDialog.UploadPath, str2), 0))
        {
          string str3 = (string) null;
          ZipWriter zipWriter2 = (ZipWriter) null;
          foreach (ImageProperties properties in imagePropertiesArray)
          {
            if (zipWriter2 != null)
            {
              FileInfo fileInfo = new FileInfo(properties.ImageFile);
              if (zipWriter2.Size + fileInfo.Length > maxZipFileSize)
              {
                zipWriter2.CreateZip();
                zipWriter2.Dispose();
                zipWriter2 = (ZipWriter) null;
              }
            }
            if (zipWriter2 == null)
            {
              str3 = "Images-" + Guid.NewGuid().ToString() + ".zip";
              zipWriter2 = new ZipWriter(Path.Combine(BackgroundAttachmentDialog.UploadPath, str3), 0);
            }
            zipWriter2.AddFile(properties.ImageFile);
            zipWriter1.AddFile(properties.Thumbnail.ImageFile);
            PageImage pageImage = new PageImage(properties);
            pageImage.SetZipKey(str3);
            pageImage.Thumbnail.SetZipKey(str2);
            pageImageList.Add(pageImage);
          }
          zipWriter2.CreateZip();
          zipWriter2.Dispose();
          zipWriter1.CreateZip();
        }
        if (this.attachment.KeepNativeCopy)
        {
          string destFileName = Path.Combine(BackgroundAttachmentDialog.UploadPath, this.attachment.ID + this.attachment.Extension);
          File.Copy(str1, destFileName);
          string nativeKey = this.attachment.ID + this.attachment.Extension;
          foreach (PageImage pageImage in pageImageList)
            pageImage.SetNativeKey(nativeKey);
        }
        this.OnFileConversionProgress(new TransferProgressEventArgs(100));
      }
      finally
      {
        imageCreator.ProgressChanged -= new ProgressEventHandler(this.creator_ProgressChanged);
      }
      return pageImageList.ToArray();
    }

    private void creator_ProgressChanged(object source, ProgressEventArgs e)
    {
      TransferProgressEventArgs e1 = new TransferProgressEventArgs(e.PercentCompleted);
      this.OnFileConversionProgress(e1);
      if (!e1.Cancel)
        return;
      e.Abort = true;
    }

    public event TransferProgressEventHandler FileConversionProgress;

    internal void OnFileConversionProgress(TransferProgressEventArgs e)
    {
      if (this.FileConversionProgress == null)
        return;
      this.FileConversionProgress((object) this, e);
    }
  }
}
